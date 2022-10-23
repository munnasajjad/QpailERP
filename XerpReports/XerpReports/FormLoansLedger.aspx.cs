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
    public partial class FormLoansLedger : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();
        }

        ReportDocument rpt = new ReportDocument();
        private void LoadGridData()
        {
            string lName = Page.User.Identity.Name;
            string prjID = "1";

            string bankLoanId = Convert.ToString(Request.QueryString["Id"]);
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);

            DataTable dtx = dtable(bankLoanId, dateFrom, dateTo);

            DataTableReader dr2 = dtx.CreateDataReader(); 

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.AccSummery);

            rpt.Load(Server.MapPath("CrptLoansLedger.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            string rptName = SQLQuery.ReturnString(@"SELECT LoanTypes.AccountsHeadName + ', Loan#  ' + BankLoan.Code  
FROM            LoanTypes INNER JOIN
                         BankLoan ON LoanTypes.LoanType = BankLoan.LoanType WHERE BankLoan.ID='" + bankLoanId + "'");
            
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;

        }
        private DataTable dtable(string ddCode, string txtDateFrom, string txtDateTo)
        {
            string customer = " "; string sQuery = " where ID  ='" + ddCode + "'";

            if (ddCode != "---ALL---")
            {
                customer = " AND HeadID ='" + ddCode + "'";
            }

            //decimal opBal = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(Rcvamount),0) FROM BankLoan " + sQuery));
            decimal opBal = 0;//Convert.ToDecimal(RunQuery.SQLQuery.ReturnString(@"SELECT ISNULL(SUM(HeadSetup.OpBalCr), 0) - ISNULL(SUM(HeadSetup.OpBalDr), 0) FROM            HeadSetup INNER JOIN
                         //BankLoan ON HeadSetup.AccountsHeadID = BankLoan.ACHeadId WHERE (BankLoan.Id='" + ddCode + "')"));
            string opDate = ""; string invDate = " ";
            if (txtDateFrom != "")
            {
                try
                {
                    invDate = Convert.ToDateTime(txtDateFrom).ToString("yyyy-MM-dd");
                    opDate = " AND  (TrDate < '" + invDate + "')";
                    invDate = " AND  (TrDate >= '" + invDate + "')";
                }
                catch (Exception) { }
            }

            string closeDate = " ";
            if (txtDateTo != "")
            {
                try
                {
                    closeDate = Convert.ToDateTime(txtDateTo).ToString("yyyy-MM-dd");
                    closeDate = " AND  (TrDate <= '" + closeDate + "')";
                }
                catch (Exception) { }
            }

            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("TrDate", typeof(string)));
            dt1.Columns.Add(new DataColumn("Inv", typeof(string)));
            dt1.Columns.Add(new DataColumn("Link", typeof(string)));
            dt1.Columns.Add(new DataColumn("Description", typeof(string)));
            dt1.Columns.Add(new DataColumn("Dr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Cr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));

            int recordcount = 0;
            int ic = 0;

            decimal debt = 0; decimal credit = 0;
            string date; string description;
            //get opening balance
            string query = " where TrType = 'Loan' " + customer;
             decimal currBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Cr),0)-isnull(sum(Dr),0) FROM [Transactions] " + query + opDate));

             decimal closeBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Cr),0)-isnull(sum(Dr),0) FROM [Transactions] " + query + closeDate));

            dr1 = dt1.NewRow();
            dr1["TrDate"] = Convert.ToDateTime(txtDateFrom).ToShortDateString();
            dr1["Inv"] = "";
            dr1["Link"] = "";
            dr1["Description"] = "Openning Balance";
            dr1["Dr"] = 0;
            dr1["Cr"] = 0;
            dr1["Balance"] = currBal;
            dt1.Rows.Add(dr1);

            query += invDate + closeDate;
            SqlCommand cmd2 = new SqlCommand("SELECT [TrDate], InvNo, TrGroup, TrType, [Description], [Dr], [Cr], [Balance] FROM [Transactions] " + query + "  ORDER BY [TrDate], TrID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            //cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = ddCode;
            //cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateFrom).ToString("yyyy-MM-dd");
            //cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateTo).AddDays(+1).ToString("yyyy-MM-dd");

            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataSet ds = new DataSet("Board");

            cmd2.Connection.Open();
            da.Fill(ds, "Board");

            SqlDataReader dr = cmd2.ExecuteReader();
            recordcount = ds.Tables[0].Rows.Count;
            cmd2.Connection.Close();

            if (recordcount > 0)
            {
                do
                {
                    date = Convert.ToDateTime(ds.Tables[0].Rows[ic]["TrDate"].ToString()).ToString("dd/MM/yyyy");
                    description = ds.Tables[0].Rows[ic]["Description"].ToString();
                    debt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Dr"].ToString());
                    credit = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Cr"].ToString());
                    currBal = credit -  debt + currBal;

                    string inv = ds.Tables[0].Rows[ic]["InvNo"].ToString();
                    string trGroup = ds.Tables[0].Rows[ic]["TrGroup"].ToString();
                    
                    string link = SQLQuery.ReturnString("Select TOP(1) SerialNo from VoucherTmp where VoucherNo='" + inv + "' AND (Head5Dr='" + ddCode + "' OR Head5Cr='" + ddCode + "') AND (Amount= '" + debt + "' OR Amount= '" + credit + "') ");
                    
                    dr1 = dt1.NewRow();
                    dr1["TrDate"] = date;
                    dr1["Inv"] = inv;
                    dr1["Link"] = link;
                    dr1["Description"] = description;
                    dr1["Dr"] = debt;
                    dr1["Cr"] = credit;
                    dr1["Balance"] = currBal;
                    
                    dt1.Rows.Add(dr1);
                    
                    ic++;

                } while (ic < recordcount);

            }

            //set closing balance
            dr1 = dt1.NewRow();
            dr1["TrDate"] = Convert.ToDateTime(txtDateTo).ToShortDateString();
            dr1["Inv"] = "";
            dr1["Link"] = "";
            dr1["Description"] = "Closing Balance";
            dr1["Dr"] = 0;
            dr1["Cr"] = 0;
            dr1["Balance"] = closeBal;
            dt1.Rows.Add(dr1);

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