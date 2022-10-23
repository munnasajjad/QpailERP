using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public partial class RptMaturedBillStatement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();
        }

        ReportDocument rpt = new ReportDocument();
        private void LoadGridData()
        {
            string lName = Page.User.Identity.Name.ToString();
            string prjId = "1";
            string partyId = Convert.ToString(Request.QueryString["PartyId"]);
            DataTable dtx = new DataTable();
            if (partyId != "---Select---")
            {
                dtx = MaturedBill(partyId); //RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");
            }

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.MaturedBillStatement);

            rpt.Load(Server.MapPath("CrptMaturedBillStatement.rpt"));
            string maturedBillList = partyId;
            //string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [AccountsHeadName] FROM [HeadSetup] WHERE AccountsHeadID='" + item + "'");
            //string rptControl = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID=(Select ControlAccountsID  FROM [HeadSetup] WHERE  AccountsHeadID='" + item + "')");


            rpt.SetDataSource(dsx);
            SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@matuBillList", maturedBillList);
            //rpt.SetParameterValue("@date", datefield);
            //rpt.SetParameterValue("@rptName", rptName);
            //rpt.SetParameterValue("@rpCtName", rptControl);
            CrystalReportViewer1.ReportSource = rpt;
        }


        private DataTable MaturedBill(string partyId)
        {
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;

            dt1.Columns.Add(new DataColumn("Company", typeof(string)));
            dt1.Columns.Add(new DataColumn("InvDate", typeof(string)));
            dt1.Columns.Add(new DataColumn("InvNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("InvoiceTotal", typeof(string)));
            dt1.Columns.Add(new DataColumn("DueAmount", typeof(string)));
            dt1.Columns.Add(new DataColumn("DeliveryDaysCount", typeof(string)));
            dt1.Columns.Add(new DataColumn("MaturedDate", typeof(string)));
            if (partyId == "O404O")
            {
                DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT PartyID, Company, Address, Phone, Email, Fax, IM, Website, ContactPerson, MobileNo, MatuirityDays, CreditLimit, OpBalance FROM Party WHERE (Type = 'customer') ORDER BY Company");
                foreach (DataRow drx in dtx.Rows)
                {
                    string pid = drx["PartyID"].ToString();
                    double mDays = Convert.ToDouble(drx["MatuirityDays"].ToString());
                    string lastMaturityDate = DateTime.Today.AddDays(mDays * (-1)).ToString("yyyy-MM-dd");

                    DataTable dtx2 = SQLQuery.ReturnDataTable(@"SELECT SaleID, InvNo, InvDate, SalesMode, CustomerID, CustomerName, DeliveryDate, InvoiceTotal, PayableAmount, DueAmount FROM Sales WHERE (CustomerID = '" + pid + "') AND (InvDate <='" + lastMaturityDate + "') AND (IsActive=1) ORDER BY InvDate");
                    foreach (DataRow drx2 in dtx2.Rows)
                    {
                        string name = drx["Company"].ToString();
                        string invDate = drx2["InvDate"].ToString();
                        string invNo = drx2["InvNo"].ToString();
                        string invAmount = drx2["PayableAmount"].ToString();
                        string invDueAmount = drx2["DueAmount"].ToString();
                        double invDeliveryDaysCount = Convert.ToDouble(SQLQuery.ReturnString("SELECT DATEDIFF(day, '" + Convert.ToDateTime(drx2["DeliveryDate"]).ToString("yyyy-MM-dd") + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "') FROM Sales WHERE InvNo='" + drx2["InvNo"] + "'"));

                        double matuDates = Convert.ToDouble(Convert.ToDouble(mDays) - Convert.ToDouble(invDeliveryDaysCount));
                        string maturedDate = DateTime.Today.AddDays(matuDates).ToString("yyyy-MM-dd");

                        dr1 = dt1.NewRow();
                        dr1["Company"] = name;
                        dr1["InvDate"] = invDate;
                        dr1["InvNo"] = invNo;
                        dr1["InvoiceTotal"] = invAmount;
                        dr1["DueAmount"] = invDueAmount;
                        dr1["DeliveryDaysCount"] = invDeliveryDaysCount;
                        dr1["MaturedDate"] = maturedDate;
                        dt1.Rows.Add(dr1);
                    }

                }
            }
            else
            {
                DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT PartyID, Company, Address, Phone, Email, Fax, IM, Website, ContactPerson, MobileNo, MatuirityDays, CreditLimit, OpBalance FROM Party WHERE PartyID = '" + partyId + "' AND (Type = 'customer') ORDER BY Company");
                foreach (DataRow drx in dtx.Rows)
                {
                    string pid = drx["PartyID"].ToString();
                    double mDays = Convert.ToDouble(drx["MatuirityDays"].ToString());
                    string lastMaturityDate = DateTime.Today.AddDays(mDays * (-1)).ToString("yyyy-MM-dd");


                    DataTable dtx2 = SQLQuery.ReturnDataTable(@"SELECT SaleID, InvNo, InvDate, SalesMode, CustomerID, CustomerName, DeliveryDate, InvoiceTotal, PayableAmount, DueAmount FROM Sales WHERE (CustomerID = '" + pid + "') AND (InvDate <='" + lastMaturityDate + "') AND (IsActive=1) ORDER BY InvDate");
                    foreach (DataRow drx2 in dtx2.Rows)
                    {
                        string name = drx["Company"].ToString();
                        string invDate = drx2["InvDate"].ToString();
                        string invNo = drx2["InvNo"].ToString();
                        string invAmount = drx2["PayableAmount"].ToString();
                        string invDueAmount = drx2["DueAmount"].ToString();
                        double invDeliveryDaysCount = Convert.ToDouble(SQLQuery.ReturnString("SELECT DATEDIFF(day, '" + Convert.ToDateTime(drx2["DeliveryDate"]).ToString("yyyy-MM-dd") + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "') FROM Sales WHERE InvNo='" + drx2["InvNo"] + "'"));

                        double matuDates = Convert.ToDouble(Convert.ToDouble(mDays) - Convert.ToDouble(invDeliveryDaysCount));
                        string maturedDate = DateTime.Today.AddDays(matuDates).ToString("yyyy-MM-dd");

                        dr1 = dt1.NewRow();
                        dr1["Company"] = name;
                        dr1["InvDate"] = invDate;
                        dr1["InvNo"] = invNo;
                        dr1["InvoiceTotal"] = invAmount;
                        dr1["DueAmount"] = invDueAmount;
                        dr1["DeliveryDaysCount"] = invDeliveryDaysCount;
                        dr1["MaturedDate"] = maturedDate;
                        dt1.Rows.Add(dr1);
                    }

                }

            }


            return dt1;
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