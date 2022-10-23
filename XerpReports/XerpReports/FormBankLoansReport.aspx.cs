using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Workflow.Activities;
using CrystalDecisions.CrystalReports.Engine;
using RunQuery;

namespace Oxford.XerpReports
{
    public partial class FormBankLoansReport : System.Web.UI.Page
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
            string prjID = "1";
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);
            string loanTypeId = Convert.ToString(Request.QueryString["LoanTypeId"]);
            string code = Convert.ToString(Request.QueryString["Code"]);

            //DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] 
            //WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");
            if (code=="0") //0==All
            {
                DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT Id, LoanType, ACHeadId, Code, ReceivedDate, InterestRate, Duration, Rcvamount, Note, DATEADD(DAY, CONVERT(INT, Duration), ReceivedDate) AS Maturedate,  DATEDIFF(DAY, ReceivedDate, CONVERT(date, SYSDATETIME())) AS CrossDay, ((CONVERT(DECIMAL,Rcvamount) * InterestRate/ 100 )/360*Duration) as Interest, CONVERT(DECIMAL, Rcvamount) * InterestRate / 100 / 360 * DATEDIFF(DAY, ReceivedDate, CONVERT(date, SYSDATETIME())) AS Interest2  FROM BankLoan WHERE LoanType='" + loanTypeId + "'  AND ReceivedDate >='" + dateFrom + "' AND ReceivedDate <='" + dateTo + "' ");

                DataTableReader dr2 = dtx.CreateDataReader();

                XerpDataSet dsx = new XerpDataSet();
                dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.BankLoan);

                rpt.Load(Server.MapPath("CrptBankLoansReport.rpt"));

                //string datefield = "As on: " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy");
                string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
                string rptName = SQLQuery.ReturnString("SELECT [LoanType] FROM [LoanTypes] WHERE LoanTypeId='" + loanTypeId + "'");

                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
                rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
                rpt.SetParameterValue("@date", datefield);
                rpt.SetParameterValue("@rptName", rptName);
                CrystalReportViewer1.ReportSource = rpt;
            }
            else
            {
                DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT Id, LoanType, ACHeadId, Code, ReceivedDate, InterestRate, Duration, Rcvamount, Note, DATEADD(DAY, CONVERT(INT, Duration), ReceivedDate) AS Maturedate,  DATEDIFF(DAY, ReceivedDate, CONVERT(date, SYSDATETIME())) AS CrossDay, ((CONVERT(DECIMAL,Rcvamount) * InterestRate/ 100 )/360*Duration) as Interest, CONVERT(DECIMAL, Rcvamount) * InterestRate / 100 / 360 * DATEDIFF(DAY, ReceivedDate, CONVERT(date, SYSDATETIME())) AS Interest2  FROM BankLoan WHERE LoanType='" + loanTypeId + "' AND Id='" + code + "' AND ReceivedDate >='" + dateFrom + "' AND ReceivedDate <='" + dateTo + "' ");

                DataTableReader dr2 = dtx.CreateDataReader();

                XerpDataSet dsx = new XerpDataSet();
                dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.BankLoan);

                rpt.Load(Server.MapPath("CrptBankLoansReport.rpt"));

                //string datefield = "As on: " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy");
                string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
                string rptName = SQLQuery.ReturnString("SELECT [LoanType] FROM [LoanTypes] WHERE LoanTypeId='" + loanTypeId + "'");

                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
                rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
                rpt.SetParameterValue("@date", datefield);
                rpt.SetParameterValue("@rptName", rptName);
                CrystalReportViewer1.ReportSource = rpt;

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