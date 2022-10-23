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
    public partial class FormLCExpenseDetails : System.Web.UI.Page
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

            rpt.Load(Server.MapPath("CrptAccHeadLedger.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            string rptName = SQLQuery.ReturnString("SELECT [AccountsHeadName] FROM [HeadSetup] WHERE AccountsHeadID='" + item + "'");
            string rptControl = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID=(Select ControlAccountsID  FROM [HeadSetup] WHERE  AccountsHeadID='" + item + "')");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
            rpt.SetParameterValue("@rpCtName", rptControl);
            CrystalReportViewer1.ReportSource = rpt;

        }


        private DataTable dataatble(string head, string dtFrom, string dtTo)
        {
            SqlDataAdapter da;
            SqlDataReader dr;
            DataSet ds;
            int recordcount = 0;
            int ic = 0;

            SqlCommand cmd2 = new SqlCommand("SELECT VoucherNo, [EntryDate] as TrDate, [VoucherRowDescription] as Description, [VoucherDR] As Dr, [VoucherCR] As Cr, [VoucherNo] As Balance FROM [VoucherDetails] WHERE ([AccountsHeadID] = @HeadName) and  EntryDate >= @DateFrom AND EntryDate <= @DateTo ORDER BY EntryDate, [SerialNo] ASC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
            cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
            cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(dtTo).ToShortDateString();

            da = new SqlDataAdapter(cmd2);
            ds = new DataSet("Board");

            cmd2.Connection.Open();
            da.Fill(ds, "Board");

            dr = cmd2.ExecuteReader();
            recordcount = ds.Tables[0].Rows.Count;

            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("VoucherNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("TrDate", typeof(string)));
            dt1.Columns.Add(new DataColumn("Description", typeof(string)));
            dt1.Columns.Add(new DataColumn("Dr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Cr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));
            cmd2.Connection.Close();

            decimal debt = 0; decimal credit = 0; decimal currBal = 0;
            string date; string description;

            //Check if the head is liability head
            string isLiability = head.Substring(0, 2);
            if (isLiability == "02")
            {
                decimal opBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(OpBalCr),0) - isnull(sum(OpBalDr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID] = '" + head + "')"));
                decimal preBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(VoucherCR),0) - isnull(sum(VoucherDR),0) FROM [VoucherDetails] WHERE ([AccountsHeadID] = '" + head + "') and   EntryDate < '" + Convert.ToDateTime(dtFrom).ToString("yyyy-MM-dd") + "' "));
                currBal = opBal + preBal;

                dr1 = dt1.NewRow();
                dr1["VoucherNo"] = "";
                dr1["TrDate"] = Convert.ToDateTime(dtFrom).ToShortDateString();
                dr1["Description"] = "Openning Balance";
                dr1["Dr"] = 0;
                dr1["Cr"] = 0;
                dr1["Balance"] = string.Format("{0:N2}", currBal);
                dt1.Rows.Add(dr1);

                if (recordcount > 0)
                {
                    do
                    {
                        date = Convert.ToDateTime(ds.Tables[0].Rows[ic]["TrDate"].ToString()).ToShortDateString();
                        description = ds.Tables[0].Rows[ic]["Description"].ToString();
                        debt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Dr"].ToString());
                        credit = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Cr"].ToString());
                        currBal = credit - debt + currBal;
                        dr1 = dt1.NewRow();
                        dr1["VoucherNo"] = ds.Tables[0].Rows[ic]["VoucherNo"].ToString();
                        dr1["TrDate"] = date;
                        dr1["Description"] = description;
                        dr1["Dr"] = debt;
                        dr1["Cr"] = credit;
                        dr1["Balance"] = string.Format("{0:N2}", currBal);
                        dt1.Rows.Add(dr1);
                        ic++;

                    } while (ic < recordcount);
                }
                else
                {
                    //GridView1.DataSource = null;
                }

                //get closing balance
                currBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(VoucherCR),0) - isnull(sum(VoucherDR),0) FROM [VoucherDetails] WHERE ([AccountsHeadID] = '" + head + "') and   EntryDate < '" + Convert.ToDateTime(dtTo).AddDays(+1).ToString("yyyy-MM-dd")));

                dr1 = dt1.NewRow();
                dr1["VoucherNo"] = "";
                dr1["TrDate"] = Convert.ToDateTime(dtTo).ToShortDateString();
                dr1["Description"] = "Closing Balance";
                dr1["Dr"] = 0;
                dr1["Cr"] = 0;
                dr1["Balance"] = string.Format("{0:N2}", (currBal + opBal));
                dt1.Rows.Add(dr1);
            }
            else
            {
                //get openning balance        
                SqlCommand cmd2x = new SqlCommand("SELECT  isnull(sum(OpBalDr),0) - isnull(sum(OpBalCr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID] = @HeadName)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd2x.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
                cmd2x.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
                cmd2x.Connection.Open();
                decimal opBal = Convert.ToDecimal(cmd2x.ExecuteScalar());
                cmd2x.Connection.Close();

                SqlCommand cmd2y = new SqlCommand("SELECT  isnull(sum(VoucherDR),0) - isnull(sum(VoucherCR),0) FROM [VoucherDetails] WHERE ([AccountsHeadID] = @HeadName) and   EntryDate < @DateFrom ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd2y.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
                cmd2y.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
                cmd2y.Connection.Open();
                decimal preBal = Convert.ToDecimal(cmd2y.ExecuteScalar());
                cmd2y.Connection.Close();

                currBal = opBal + preBal;

                dr1 = dt1.NewRow();
                dr1["VoucherNo"] = "";
                dr1["TrDate"] = Convert.ToDateTime(dtFrom).ToShortDateString();
                dr1["Description"] = "Openning Balance";
                dr1["Dr"] = 0;
                dr1["Cr"] = 0;
                dr1["Balance"] = string.Format("{0:N2}", currBal);
                dt1.Rows.Add(dr1);

                if (recordcount > 0)
                {
                    do
                    {
                        date = Convert.ToDateTime(ds.Tables[0].Rows[ic]["TrDate"].ToString()).ToShortDateString();
                        description = ds.Tables[0].Rows[ic]["Description"].ToString();
                        debt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Dr"].ToString());
                        credit = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Cr"].ToString());
                        currBal = debt - credit + currBal;
                        //object debit2 = dt1.Compute("Sum(Dr)", "");
                        //object credit2 = dt1.Compute("Sum(Cr)", "");
                        //currBal = Convert.ToDecimal(debit2) - Convert.ToDecimal(credit2);

                        dr1 = dt1.NewRow();
                        dr1["VoucherNo"] = ds.Tables[0].Rows[ic]["VoucherNo"].ToString();
                        dr1["TrDate"] = date;
                        dr1["Description"] = description;
                        dr1["Dr"] = debt;
                        dr1["Cr"] = credit;
                        dr1["Balance"] = string.Format("{0:N2}", currBal);
                        dt1.Rows.Add(dr1);
                        ic++;

                    } while (ic < recordcount);

                }
                else
                {

                    //GridView1.DataSource = null;
                }

                //get closing balance        
                SqlCommand cmd2z = new SqlCommand("SELECT isnull(sum(VoucherDR),0)-isnull(sum(VoucherCR),0) as balance FROM [VoucherDetails] WHERE ([AccountsHeadID] = @HeadName) and EntryDate < @DateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd2z.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
                cmd2z.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(dtTo).AddDays(+1).ToShortDateString();
                cmd2z.Connection.Open();
                currBal = Convert.ToDecimal(cmd2z.ExecuteScalar());
                cmd2z.Connection.Close();

                dr1 = dt1.NewRow();
                dr1["VoucherNo"] = "";
                dr1["TrDate"] = Convert.ToDateTime(dtTo).ToShortDateString();
                dr1["Description"] = "Closing Balance";
                dr1["Dr"] = 0;
                dr1["Cr"] = 0;
                dr1["Balance"] = string.Format("{0:N2}", (currBal + opBal));
                dt1.Rows.Add(dr1);
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