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
    public partial class FormFixedAssetsLists : System.Web.UI.Page
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
            string dateFrom = Convert.ToString(Request.QueryString["dt1"]);
            string dateTo = Convert.ToString(Request.QueryString["dt2"]);

            DataTable dtx = dataatble(item, dateFrom, dateTo); //RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.AccSummery);

            rpt.Load(Server.MapPath("CrptFixedAssetsList.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            string rptName = SQLQuery.ReturnString("SELECT [GroupName] FROM [ItemGroup] WHERE GroupSrNo='" + item + "'");
            //string rptControl = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID=(Select ControlAccountsID  FROM [HeadSetup] WHERE  AccountsHeadID='" + item + "')");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
            //rpt.SetParameterValue("@rpCtName", rptControl);
            CrystalReportViewer1.ReportSource = rpt;
        }


        private DataTable dataatble(string head, string dtFrom, string dtTo)
        {
            SqlDataAdapter da;
            SqlDataReader dr;
            DataSet ds;
            int recordcount = 0;
            int ic = 0;


            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("Head", typeof(string)));
            dt1.Columns.Add(new DataColumn("TrDate", typeof(string)));
            dt1.Columns.Add(new DataColumn("Description", typeof(string)));
            dt1.Columns.Add(new DataColumn("Customer", typeof(string)));
            dt1.Columns.Add(new DataColumn("VoucherNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("Particulars", typeof(string)));
            dt1.Columns.Add(new DataColumn("Qty", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Amount", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("TDS", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Rate", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));

            dt1.Columns.Add(new DataColumn("Dr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Cr", typeof(decimal)));
            //dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));
            

            decimal debt = 0; decimal credit = 0; decimal currBal = 0;
            DateTime date; string description;
            string customer;
            string LCNo;
            string Location;
            decimal TotalValueUSD = 0; decimal assetCost = 0; decimal AccmDep = 0; decimal Totalvalue = 0; decimal qty = 0;


             ////get openning balance        
             //SqlCommand cmd2x = new SqlCommand("SELECT  isnull(sum(Quantity),0)-isnull(sum(DeliveredQty),0)  FROM FixedAssets WHERE ProductID = @HeadName AND InDate >= @DateFrom", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
             //cmd2x.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
             //cmd2x.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
             //cmd2x.Connection.Open();
             //decimal opBal = Convert.ToDecimal(cmd2x.ExecuteScalar());
             //cmd2x.Connection.Close();

             //currBal = opBal;

             //dr1 = dt1.NewRow();
             //dr1["Head"] = "";
             //dr1["TrDate"] = Convert.ToDateTime(dtFrom).ToShortDateString();
             //dr1["Description"] = "Openning Stock";
             //dr1["Dr"] = 0;
             //dr1["Cr"] = 0;
             //dr1["Balance"] = string.Format("{0:N2}", currBal);
             //dt1.Rows.Add(dr1);



             // SqlCommand cmd2 = new SqlCommand("SELECT [InDate], [ProductName],[Remark], SUM(Quantity) As Dr, SUM(DeliveredQty) As Cr FROM [FixedAssets] WHERE ([ProductID] = @HeadName) AND (InDate >= @DateTo) AND (OutDate <= @DateFrom) Group by [InDate], [ProductName], [Remark]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
             SqlCommand cmd2 = new SqlCommand(@"SELECT        COUNT(FixedAssets.Id) AS QTY, FixedAssets.InDate, FixedAssets.ProductName, SUM(FixedAssets.PurchaseCost) AS PurchaseAmount, FixedAssets.ManufacturerName, FixedAssets.LCNoFTT, Products.Depreciation, 
                         Products.ItemName, ItemSubGroup.CategoryName, Categories.CategoryName AS Grade, ItemGrade.GradeName, ItemGroup.GroupName, FixedAssets.Location, (FixedAssets.TotalValueUSD) AS TotalUSD
FROM            Categories INNER JOIN
                         FixedAssets INNER JOIN
                         Products ON FixedAssets.ProductID = Products.ProductID ON Categories.CategoryID = Products.CategoryID INNER JOIN
                         ItemGrade ON Categories.GradeID = ItemGrade.GradeID INNER JOIN
                         ItemSubGroup ON ItemGrade.CategoryID = ItemSubGroup.CategoryID INNER JOIN
                         ItemGroup ON ItemSubGroup.GroupID = ItemGroup.GroupSrNo

WHERE (ItemGroup.GroupSrNo = @HeadName) AND (InDate >= @DateFrom)
GROUP BY FixedAssets.InDate, FixedAssets.ProductName, FixedAssets.ManufacturerName, FixedAssets.LCNoFTT, Products.Depreciation, Products.ItemName, ItemSubGroup.CategoryName, Categories.CategoryName, 
                         ItemGrade.GradeName, ItemGroup.GroupName, FixedAssets.Location,FixedAssets.TotalValueUSD ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
            cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
            cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(dtTo).ToShortDateString();

            da = new SqlDataAdapter(cmd2);
            ds = new DataSet("Board");

            cmd2.Connection.Open();
            da.Fill(ds, "Board");

            dr = cmd2.ExecuteReader();
            recordcount = ds.Tables[0].Rows.Count;
            cmd2.Connection.Close();    

            if (recordcount > 0)
                {
                    do
                    {
                        date = Convert.ToDateTime(ds.Tables[0].Rows[ic]["InDate"].ToString());
                        description = ds.Tables[0].Rows[ic]["ProductName"].ToString();
                        customer = ds.Tables[0].Rows[ic]["ManufacturerName"].ToString();
                        LCNo = ds.Tables[0].Rows[ic]["LCNoFTT"].ToString();
                        Location = ds.Tables[0].Rows[ic]["Location"].ToString();

                        TotalValueUSD = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalUSD"].ToString());
                        assetCost = Convert.ToDecimal(ds.Tables[0].Rows[ic]["PurchaseAmount"].ToString());

                    decimal   DepRate = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Depreciation"].ToString());


                        qty = Convert.ToDecimal(ds.Tables[0].Rows[ic]["QTY"].ToString());

                        //debt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalUSD"].ToString());
                        //credit = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalUSD"].ToString());
                        
                        dr1 = dt1.NewRow();
                        dr1["Head"] = ds.Tables[0].Rows[ic]["ItemName"].ToString();
                        dr1["TrDate"] = date;
                        dr1["Description"] = description;
                        dr1["Customer"] = customer;
                        dr1["VoucherNo"] = LCNo;
                        dr1["Particulars"] = Location;
                         dr1["Balance"] = currBal;      
                        dr1["Qty"] = qty;
                        dr1["TDS"] = assetCost;
                        dr1["Amount"] = TotalValueUSD;
                        DateTime past = Convert.ToDateTime(date);
                        DateTime future = Convert.ToDateTime(dtTo);
                        string pid = SQLQuery.ReturnString("Select ProductID from FixedAssets Where ProductName ='" + description + "'");
                        AccmDep = GetDeprAmt(pid, Convert.ToDecimal(assetCost), past, future);
                        dr1["Rate"] = assetCost - AccmDep;
                        dr1["Balance"] = AccmDep;
                        dr1["Dr"] = debt;
                        dr1["Cr"] = credit;

                        //dr1["Balance"] = string.Format("{0:N2}", currBal);
                        dt1.Rows.Add(dr1);
                        ic++;

                    } while (ic < recordcount);

                }

                //get closing balance        
                //SqlCommand cmd2z = new SqlCommand("SELECT isnull(sum(Quantity),0)-isnull(sum(DeliveredQty),0) as balance FROM FixedAssets WHERE ProductID = @HeadName and OutDate <= @DateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //cmd2z.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
                //cmd2z.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(dtTo).AddDays(+1).ToShortDateString();
                //cmd2z.Connection.Open();
                //currBal = Convert.ToDecimal(cmd2z.ExecuteScalar());
                //cmd2z.Connection.Close();

                //dr1 = dt1.NewRow();
                //dr1["Head"] = "";
                //dr1["TrDate"] = Convert.ToDateTime(dtTo).ToShortDateString();
                //dr1["Description"] = "Closing Stock";
                //dr1["Dr"] = 0;
                //dr1["Cr"] = 0;
                //dr1["Balance"] = string.Format("{0:N2}", (currBal + opBal));
                //dt1.Rows.Add(dr1);
            

            return dt1;
        }

        private decimal GetDeprAmt(string productId, decimal purchAmt, DateTime pDate, DateTime fdate)
        {
            string depPercent =
                SQLQuery.ReturnString("SELECT Convert(int, Depreciation) FROM Products Where ProductID ='" + productId + "'");
            if (depPercent == "" || depPercent == "0")
            {
                depPercent = SQLQuery.ReturnString("SELECT Depreciationvalue FROM ItemGroup WHERE GroupSrNo= (Select GroupId from vwProducts WHERE id='" + productId + "') ");
            }

            decimal monthlyPercent = (Convert.ToDecimal(depPercent)) / 12M;
            decimal monthlyAmt = purchAmt * (monthlyPercent / 100M);
            int monthsApart = 12 * (fdate.Year - pDate.Year) + fdate.Month - pDate.Month;
            int qty = Math.Abs(monthsApart);

            decimal total = qty * monthlyAmt;
            return Math.Round(purchAmt - total);
        }


        public static decimal GetDeprAmt(string productId, decimal purchAmt, DateTime pDate)
        {
            string depPercent =
                SQLQuery.ReturnString("SELECT Convert(int, Depreciation) FROM Products Where ProductID ='" + productId + "'");
            if (depPercent == "" || depPercent == "0")
            {
                depPercent = SQLQuery.ReturnString("SELECT Depreciationvalue FROM ItemGroup WHERE GroupSrNo= (Select GroupId from vwProducts WHERE id='" + productId + "') ");
            }

            decimal monthlyPercent = (Convert.ToDecimal(depPercent)) / 12M;
            decimal monthlyAmt = purchAmt * (monthlyPercent / 100M);
            int monthsApart = 12 * (DateTime.Now.Year - pDate.Year) + DateTime.Now.Month - pDate.Month;
            int qty = Math.Abs(monthsApart);

            decimal total = qty * monthlyAmt;
            return Math.Round(purchAmt - total);
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