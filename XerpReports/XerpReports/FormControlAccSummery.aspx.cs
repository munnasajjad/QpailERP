﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using RunQuery;

namespace Oxford.XerpReports
{
    public partial class FormControlAccSummery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();

            /*
            //if (!IsPostBack)
            //{
            string invNo = 
            string chlNo = Convert.ToString(Request.QueryString["dt"]);
            string pid = Convert.ToString(Request.QueryString["pid"]);
            if (pid != null)
            {
                pid = " CustomerID='" + pid + "' AND ";
            }

            if (invNo != "")
            {
                
                SqlCommand cmd = new SqlCommand(@"SELECT Id, InvNo, SizeId, ProductID, BrandID, ProductName, UnitCost, Quantity, ItemTotal, VatPercent, VAT, NetAmount, UnitWeight, TotalWeight, QtyPerCarton, 
                         TotalCarton, UnitType, PerviousDeliveredQty, QtyBalance, ItemChallanNo,
                        (SELECT Company from Party where PartyID=(Select CustomerID from Sales where InvNo=SaleDetails.InvNo))  as  EntryBy,
                       (Select InvDate from Sales where InvNo=SaleDetails.InvNo) as EntryDate, ReturnQty
                        FROM SaleDetails WHERE InvNo IN (Select InvNo from Sales where " + pid + " InvDate>='" + invNo + "' AND  InvDate<='" + chlNo + "') ", new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
                cmd.Connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                //Pathshala ds = new Pathshala();
                ds.Load(dr, LoadOption.OverwriteChanges, ds.SaleDetails);
                cmd.Connection.Close();
                ReportDocument rpt = new ReportDocument();
                rpt.Load(Server.MapPath("Sales-Statement.rpt"));

                string datefield = "From " + Convert.ToDateTime(invNo).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(chlNo).ToString("dd/MM/yyyy");

                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
                rpt.SetDataSource(ds); SQLQuery.LoadrptHeader(ds, rpt);
                rpt.SetParameterValue("@date", datefield);
                CrystalReportViewer1.ReportSource = rpt;
                //CrystalReportViewer1.DataBind();

                //}
            }*/
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
            string prjID = "1"; decimal opBal = 0; decimal closingBalance = 0;
            string item = Convert.ToString(Request.QueryString["item"]);
            string dateFrom = Convert.ToString(Request.QueryString["dt1"]);
            string dateTo = Convert.ToString(Request.QueryString["dt2"]);
            string media = Convert.ToString(Request.QueryString["media"]);
            //DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT TOP (1) AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ORDER BY AccountsHeadName DESC ) AS HEAD, AccountsHeadID, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");

            string qry = " FROM VoucherDetails WHERE(AccountsHeadID = HeadSetup.AccountsHeadID) AND VoucherNo IN (Select VoucherNo from VoucherMaster where  (VoucherDate >= '" + dateFrom + "')  AND  (VoucherDate <= '" + dateTo + "')  AND Voucherpost<>'C')";
            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT AccountsHeadID, AccountsHeadName AS HEAD,
                             (SELECT ISNULL(SUM(VoucherDR), 0) " + qry + @") AS Dr,
                             (SELECT ISNULL(SUM(VoucherCR), 0) " + qry + @") AS Cr
                             FROM HeadSetup WHERE (ControlAccountsID = '" + item + @"') ORDER BY HEAD");

            DataTable dt1 = new DataTable();
            DataRow dr1 = dt1.NewRow();
            dt1.Columns.Add(new DataColumn("HeadID", typeof(string)));
            dt1.Columns.Add(new DataColumn("HEAD", typeof(string)));
            dt1.Columns.Add(new DataColumn("OpeningBalance", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Dr", typeof(string)));
            dt1.Columns.Add(new DataColumn("Cr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));

            foreach (DataRow drx in dtx.Rows)
            {
                string headId = drx["AccountsHeadID"].ToString();
                string headName = drx["HEAD"].ToString();
                string dr = drx["Dr"].ToString();
                string cr = drx["Cr"].ToString();

                opBal = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0)-ISNULL(SUM(VoucherCR),0) + (SELECT ISNULL(SUM(OpBalDr),0)-ISNULL(SUM(OpBalCr),0) FROM HeadSetup WHERE AccountsHeadID='" + drx["AccountsHeadID"] + "') FROM [VoucherDetails] WHERE ([AccountsHeadID] ='" + drx["AccountsHeadID"] + "' AND VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE  (VoucherDate <= '" + Convert.ToDateTime(dateFrom).AddDays(-1).ToString("yyyy-MM-dd") + "')) AND ISApproved='A')"));
                closingBalance = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0)-ISNULL(SUM(VoucherCR),0) + (SELECT ISNULL(SUM(OpBalDr),0)-ISNULL(SUM(OpBalCr),0) FROM HeadSetup WHERE AccountsHeadID='" + drx["AccountsHeadID"] + "') FROM [VoucherDetails] WHERE ([AccountsHeadID] ='" + drx["AccountsHeadID"] + "' AND VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE  (VoucherDate <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "')) AND ISApproved='A')"));

                string isLiability = headId.Substring(0, 2);
                if (isLiability == "02")
                {
                    opBal = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0)-ISNULL(SUM(VoucherDR),0) + (SELECT ISNULL(SUM(OpBalCr),0)-ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE AccountsHeadID='" + drx["AccountsHeadID"] + "') FROM [VoucherDetails] WHERE ([AccountsHeadID] ='" + drx["AccountsHeadID"] + "' AND VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE  (VoucherDate <= '" + Convert.ToDateTime(dateFrom).AddDays(-1).ToString("yyyy-MM-dd") + "')) AND ISApproved='A')"));
                    closingBalance = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0)-ISNULL(SUM(VoucherDR),0) + (SELECT ISNULL(SUM(OpBalCr),0)-ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE AccountsHeadID='" + drx["AccountsHeadID"] + "') FROM [VoucherDetails] WHERE ([AccountsHeadID] ='" + drx["AccountsHeadID"] + "' AND VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE  (VoucherDate <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "')) AND ISApproved='A')"));
                }

                //string bal = SQLQuery.ReturnString(@"SELECT " + query + "As Balance FROM [VoucherDetails] WHERE ([AccountsHeadID] ='" + headId + @"')  AND  (EntryDate <= '" + dateTo +  "')  AND ISApproved<>'C'");
                if (opBal > 0 || Convert.ToDecimal(dr) > 0 || Convert.ToDecimal(cr) > 0 || closingBalance > 0)
                {
                    dr1 = dt1.NewRow();
                    dr1["HeadID"] = headId;
                    dr1["HEAD"] = headName;
                    dr1["OpeningBalance"] = opBal;
                    dr1["Dr"] = dr.ToString();
                    dr1["Cr"] = cr.ToString();
                    //dr1["Balance"] = bal;
                    dr1["Balance"] = closingBalance;
                    dt1.Rows.Add(dr1);
                }

            }

            DataTableReader dr2 = dt1.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.AccSummery);

            rpt.Load(Server.MapPath("CrptControlAccSummery.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx);
            SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
            //CrystalReportViewer1.ReportSource = rpt;

            if (media=="pdf")
            {
                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, false, "CrptControlAccSummery.rpt");
            }
            else if (media == "xls")
            {
                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook, HttpContext.Current.Response, false, "CrptControlAccSummery.rpt");
            }
            else if (media == "doc")
            {
                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.WordForWindows, HttpContext.Current.Response, false, "CrptControlAccSummery.rpt");
            }
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