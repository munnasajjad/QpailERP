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
    public partial class FormCheque_Receipt_Payment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();

        }

        ReportDocument rpt = new ReportDocument();

        private void LoadGridData()
        {
            string lName = Page.User.Identity.Name.ToString();
            string prjID = "1";
            string datefrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateto = Convert.ToString(Request.QueryString["dateTo"]);

            string reportTypeId = Convert.ToString(Request.QueryString["ReportTypeId"]);

            //DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] 
            //WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");

            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT [ChequeNo], [ChequeName], [ChqAmt], [ChqDate],[ChqStatus] FROM [Cheque] WHERE  EntryDate>='" + Convert.ToDateTime(datefrom).ToString("yyyy-MM-dd") +
                                                "' AND  EntryDate<='" + Convert.ToDateTime(dateto).ToString("yyyy-MM-dd") + "' AND TrType='" + reportTypeId + "'");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.Cheque);

            rpt.Load(Server.MapPath("CrptCheque-Receipt-Pyment.rpt"));

            string datefield = "From " + Convert.ToDateTime(datefrom).ToString("dd/MM/yyyy") + "  To  " + Convert.ToDateTime(dateto).ToString("dd/MM/yyyy");
            string rptName = "";
            if (reportTypeId== "Payment")
            {
                rptName = "Payment Statement";
            }
            else
            {
                rptName = "Receipt Statement";
            }

            
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;

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