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
    public partial class FormQtyDateLedger : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();

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

            rpt.Load(Server.MapPath("rptQtyDateLedger.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            string rptName = SQLQuery.ReturnString("SELECT [AccountsHeadName] FROM [HeadSetup] WHERE AccountsHeadID='" + item + "'");
            string rptControl = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID=(Select ControlAccountsID  FROM [HeadSetup] WHERE  AccountsHeadID='" + item + "')");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
            rpt.SetParameterValue("@rpCtName", rptControl);
            CrystalReportViewer1.ReportSource = rpt;

        }


        private DataTable dataatble(string head, string dtFrom, string dtTo)
        {
            DateTime datadate = Convert.ToDateTime(dtFrom);
            DateTime dateTo = Convert.ToDateTime(dtTo);

            SqlDataAdapter da;
            SqlDataReader dr;
            DataSet ds;
            int recordcount = 0;
            int ic = 0;

            decimal debt = 0;
            decimal credit = 0;
            decimal currBal = 0;
            string date;
            string description;

            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("TrDate", typeof(string)));
            dt1.Columns.Add(new DataColumn("Description", typeof(string)));
            dt1.Columns.Add(new DataColumn("Dr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Cr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));


            //Check if the head is liability head
            string isLiability = head.Substring(0, 2);
            if (isLiability == "02")
            {
                decimal opBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString(
                    "SELECT  isnull(sum(QtyCr),0) - isnull(sum(QtyDr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID] = '" +
                    head + "')"));
                decimal preBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString(
                    "SELECT  isnull(sum(OutQty),0) - isnull(sum(InQty),0) FROM [VoucherDetails] WHERE ([AccountsHeadID] = '" +
                    head + "') and   EntryDate < '" +
                    datadate.ToString("yyyy-MM-dd") + "'  and ISApproved='A'"));
                currBal = opBal + preBal;
            }
            else
            {
                decimal opBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString(
                    "SELECT  isnull(sum(QtyDr),0) - isnull(sum(QtyCr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID] = '" +
                    head + "')"));
                decimal preBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString(
                    "SELECT  isnull(sum(InQty),0) - isnull(sum(OutQty),0) FROM [VoucherDetails] WHERE ([AccountsHeadID] = '" +head + "') and   EntryDate < '" +
                    datadate.ToString("yyyy-MM-dd") + "'  and ISApproved='A'"));
                currBal = opBal + preBal;
            }
            dr1 = dt1.NewRow();
            dr1["TrDate"] = datadate;
            dr1["Description"] = "Opening Balance";
            dr1["Dr"] = 0;
            dr1["Cr"] = 0;
            dr1["Balance"] = string.Format("{0:N2}", currBal);
            dt1.Rows.Add(dr1);



            while (datadate <= dateTo)
            {
                string query = @"SELECT  ISNULL(SUM([InQty]),0) As Dr, ISNULL(SUM([OutQty]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] = '" +
                    head + "') and  EntryDate = '" + datadate.ToString("yyyy-MM-dd") + "'  AND ISApproved<>'C' ";
                if (isLiability == "02")
                {
                     query = @"SELECT  ISNULL(SUM([OutQty]),0) As Dr, ISNULL(SUM([InQty]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] = '" +
                    head + "') and  EntryDate = '" + datadate.ToString("yyyy-MM-dd") + "'  AND ISApproved<>'C' ";

                }

                DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(query);

                foreach (DataRow drx in dtx.Rows)
                {
                    dr1 = dt1.NewRow();
                    dr1["TrDate"] = datadate;
                    dr1["Description"] ="Total stock-in & stock-out quantity";
                    string drValue = drx["Dr"].ToString();
                    dr1["Dr"] = drx["Dr"].ToString();
                    dr1["Cr"] = drx["Cr"].ToString();
                    if (isLiability == "02")
                    {
                        currBal = Convert.ToDecimal(drx["Cr"].ToString()) - Convert.ToDecimal(drx["Dr"].ToString()) + currBal;
                    }
                    else
                    {
                        currBal = Convert.ToDecimal(drx["Dr"].ToString()) - Convert.ToDecimal(drx["Cr"].ToString()) + currBal;
                    }
                    dr1["Balance"] = string.Format("{0:N2}", currBal);

                    if (Convert.ToDecimal(drx["Cr"].ToString()) > 0 || Convert.ToDecimal(drx["Dr"].ToString()) > 0)
                    {
                        dt1.Rows.Add(dr1);
                    }

                }

                datadate = datadate.AddDays(1);
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