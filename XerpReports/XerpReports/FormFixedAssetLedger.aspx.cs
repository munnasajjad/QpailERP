using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using RunQuery;

namespace Oxford.XerpReports
{
    public partial class FormFixedAssetLedger : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadGridData();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        ReportDocument rpt = new ReportDocument();
        private int currdep;

        private void LoadGridData()
        {
            //decimal total2 = (decimal)0.0;
            //decimal qty2 = (decimal)0.0;
            //decimal vat2 = (decimal)0.0;
            //decimal TotalSales2 = (decimal)0.0;
            //decimal TotalWeight2 = (decimal)0.0;

            string lName = Page.User.Identity.Name.ToString();
            string prjID = "1";
            string item = Convert.ToString(Request.QueryString["item"]);
            string inv = Convert.ToString(Request.QueryString["lcinv"]);
            string purchaseDate = Convert.ToString(Request.QueryString["pdate"]);
            string purchaseAmount = Convert.ToString(Request.QueryString["cost"]);
            string depRate = Convert.ToString(Request.QueryString["depRate"]);
            string dt1 = Convert.ToString(Request.QueryString["date1"]);
            string dt2 = Convert.ToString(Request.QueryString["date2"]);
            string purchasedate = Convert.ToDateTime(purchaseDate).ToString("dd-MM-yyyy");

            DataTable dtx = dataatble(Convert.ToDateTime(purchasedate),Convert.ToDecimal(purchaseAmount), Convert.ToDecimal(depRate), item, dt1, dt2); //RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");

            DataTableReader dr2 = dtx.CreateDataReader();
            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.DepLedger);

            rpt.Load(Server.MapPath("CrptFixedAssetLedger.rpt"));

            string itemcode = SQLQuery.ReturnString("SELECT LCNoFTT  FROM FixedAssets WHERE LCNoFTT='" + inv + "'");
            //string datefield = "As on Date " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy");
            string rptName = SQLQuery.ReturnString(@"SELECT FixedAssets.ProductName FROM FixedAssets INNER JOIN Products ON FixedAssets.ProductID = Products.ProductID WHERE(FixedAssets.LCNoFTT ='" + inv + "')");

            string inDate = SQLQuery.ReturnString("SELECT InDate FROM FixedAssets WHERE LCNoFTT='" + inv + "'");
            string purchaseCost = SQLQuery.ReturnString("SELECT SUM(PurchaseCost) FROM FixedAssets WHERE LCNoFTT='" + inv + "'");

            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@itemcode", itemcode);
            rpt.SetParameterValue("@rptName", rptName);
            rpt.SetParameterValue("@purchaseDate", Convert.ToDateTime(inDate).ToString("dd-MMM-yyyy"));
            rpt.SetParameterValue("@purchaseCost", purchaseCost);
            //rpt.SetParameterValue("@depRate", depRate);

            //rpt.SetParameterValue("@rpCtName", rptControl);
            CrystalReportViewer1.ReportSource = rpt;
        }

        // 20 percent khoy hoy 12       mashe
        // 1    "       "   "   12/20      "
        // 100  "       "   "   (12*100)/20  "

        private DataTable dataatble(DateTime purDate, decimal purAmount, decimal depRate, string item,string datefrom, string dateto)
        {
            SQLQuery.ExecNonQry("Delete TempFixedAssetLedger");
            decimal lifetimeMonthsQty = (100M*12M)/ Convert.ToDecimal(depRate);
            decimal monthAmount =  purAmount/ lifetimeMonthsQty;
            decimal totalyear = lifetimeMonthsQty / 12;

            DataTable dt1 = new DataTable();
            DataRow dr1 = null;

            dt1.Columns.Add(new DataColumn("Months", typeof(string)));
            dt1.Columns.Add(new DataColumn("Depreciation", typeof(string)));
            dt1.Columns.Add(new DataColumn("WDV", typeof(string)));
            dt1.Columns.Add(new DataColumn("ECOYear", typeof(string)));

            int i = 1;
            while (lifetimeMonthsQty >= i)
            {
                //dr1 = dt1.NewRow();
                //dr1["Months"] = purDate.AddMonths(i).ToString("MMM-yyyy");
                string date = purDate.AddMonths(i).ToString("MMM-yyyy");
                //dr1["Depreciation"] = monthAmount;
                purAmount -= monthAmount;
                //dr1["WDV"] = purAmount;
                //dr1["ECOYear"] = totalyear;
                //dt1.Rows.Add(dr1);
                
                SQLQuery.ExecNonQry("INSERT INTO [dbo].[TempFixedAssetLedger] ([Inv],[ProductName],[Month],[Depreciation],[WDV],[Date]) VALUES ('" + 0 +"','"+ 0 + "','" + date + "','" + monthAmount + "','" + purAmount + "','" + purDate.AddMonths(i).ToString("yyyy-MM-dd") + "')");
                i++;
            }
            decimal purAmount2 = '0';
            DataTable dt = SQLQuery.ReturnDataTable("SELECT Month,Depreciation,WDV FROM TempFixedAssetLedger WHERE Date >='"+ datefrom + "' AND Date <='"+ dateto +"' ");

            string amount1 = SQLQuery.ReturnString("SELECT Depreciation FROM TempFixedAssetLedger WHERE Date >='" + datefrom + "' AND Date <='" + dateto + "' "); 
            string amount2 = SQLQuery.ReturnString("SELECT WDV FROM TempFixedAssetLedger WHERE Date >='" + datefrom + "' AND Date <='" + dateto + "' ");
            dr1 = dt1.NewRow();
            dr1["Months"] = "Opening Balance";
            dr1["Depreciation"] = "0";
            dr1["WDV"] = amount2;
            dr1["ECOYear"] = totalyear;
            dt1.Rows.Add(dr1);

            foreach (DataRow dr in dt.Rows)
            {
                dr1 = dt1.NewRow();
                dr1["Months"] = Convert.ToString(dr["Month"].ToString());
                dr1["Depreciation"] = Convert.ToDecimal(dr["Depreciation"].ToString());
                
                dr1["WDV"] = Convert.ToDecimal(dr["WDV"].ToString());
                dr1["ECOYear"] = totalyear;
                dt1.Rows.Add(dr1);
                purAmount += Convert.ToDecimal(dr["Depreciation"].ToString());
                
                purAmount2 += Convert.ToDecimal(dr["WDV"].ToString());

            }
           // DataTable dt2 = SQLQuery.ReturnDataTable("SELECT Month,Depreciation,WDV FROM TempFixedAssetLedger WHERE Date >='" + datefrom + "' AND Date <='" + dateto + "' ");
            //dr1 = dt1.NewRow();
            //dr1["Months"] = "Total :";
            //dr1["Depreciation"] = purAmount;
            //purAmount -= monthAmount;
            //dr1["WDV"] = "0";
            //dr1["ECOYear"] = totalyear;
            //dt1.Rows.Add(dr1);

            return dt1;
        }

        //private string GenerateBalance1(string groupId, string cldate)
        //{
        //    DataTable dt = SQLQuery.ReturnDataTable(@"SELECT ItemGroup.GroupSrNo, ItemGroup.GroupName, ItemGroup.Description, ItemGroup.ProjectID, ItemGroup.EntryBy AS Expr1, ItemGroup.DepreciationType, FixedAssets.Id, FixedAssets.InDate, FixedAssets.ProductID, 
        //                 Products.ProductID AS Expr2, Products.Depreciation, FixedAssets.PurchaseCost
        //                 FROM            Products INNER JOIN
        //                 FixedAssets ON Products.ProductID = FixedAssets.ProductID CROSS JOIN
        //                 ItemGroup  WHERE ItemGroup.GroupSrNo='" + groupId + "' AND (InDate='" + cldate + "') ORDER BY ItemGroup.GroupSrNo");

        //    return "";
        //}
        private string Depreciation(string groupId)
        {
            DataTable dt = SQLQuery.ReturnDataTable(@"SELECT AVG(Products.Depreciation) AS Depreciation FROM Products CROSS JOIN ItemGroup WHERE ItemGroup.GroupSrNo='" + groupId + "'");



            return "";
        }





        protected void CrystalReportViewer1_OnUnload(object sender, EventArgs e)
        {
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();
        }
        protected override void OnUnload(EventArgs e)
        {
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();
        }
    }
}