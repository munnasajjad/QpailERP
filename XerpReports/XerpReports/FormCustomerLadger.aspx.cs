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
    public partial class FormCustomerLadger : System.Web.UI.Page
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
            string item = Convert.ToString(Request.QueryString["Cust"]);
            string reporType = Convert.ToString(Request.QueryString["Types"]);
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);

            DataTable dtx = LoadGridData(item, dateFrom, dateTo, reporType);

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.AccSummery);
            
            rpt.Load(Server.MapPath("CrptCustomerLedger.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            string rptName = SQLQuery.ReturnString("SELECT [Company] FROM [Party] WHERE PartyId='" + item + "'");
            //string rptType = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + reporType + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
            rpt.SetParameterValue("@rptType", reporType);
            CrystalReportViewer1.ReportSource = rpt;

        }
        private DataTable LoadGridData(string cust, string dtFrom, string dtTo, string type)
        {
            string customer = " "; string party = " ";
            decimal opBal = 0;
            if (cust != "---ALL---" && cust != null)
            {
                customer = " AND HeadID ='" + cust + "'";
                party = " where PartyID ='" + cust + "'";
                opBal = Convert.ToDecimal(SQLQuery.ReturnString("Select OpBalance FROM Party " + party));
            }
            
            string opDate = " "; string invDate = " ";
            if (dtFrom != "")
            {
                try
                {
                    invDate = Convert.ToDateTime(dtFrom).ToString("yyyy-MM-dd");
                    opDate = " AND  (TrDate < '" + invDate + "')";
                    invDate = " AND  (TrDate >= '" + invDate + "')";
                }
                catch (Exception) { }
            }

            string closeDate = " ";
            if (dtTo != "")
            {
                try
                {
                    closeDate = Convert.ToDateTime(dtTo).ToString("yyyy-MM-dd");
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
            string dateFrom = Convert.ToDateTime(dtFrom).ToString("yyyy-MM-dd");
            string dateTo = Convert.ToDateTime(dtTo).ToString("yyyy-MM-dd");

            //get opening balance        
            string query = " where  TrType = 'Customer'  " + customer + opDate;
            decimal currBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + query));

            query = " where  TrType = 'Customer'  " + customer + closeDate;
            decimal closeBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + query));

            int mDays = Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("SELECT MatuirityDays from Party" + party)));
            string lastMaturityDate = DateTime.Today.AddDays(mDays * (-1)).ToString("yyyy-MM-dd");

            if (type == "Matured")
            {
                currBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" +
                                              cust + "' AND IsActive=1 AND InvDate<'" + lastMaturityDate + "' AND InvDate<'" + dateFrom + "'"));

                closeBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" +
                                              cust + "' AND IsActive=1 AND InvDate<'" + lastMaturityDate + "' AND InvDate<='" + dateTo + "'"));
            }

            if (type == "Immatured")
            {
                currBal = Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" + cust + "'  AND IsActive=1 AND InvDate >'" + lastMaturityDate + "' AND InvDate <'" + dateFrom + "'"));
                closeBal = Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" + cust + "'  AND IsActive=1 AND InvDate>'" + lastMaturityDate + "' AND InvDate <='" + dateTo + "'"));
            }

            dr1 = dt1.NewRow();
            dr1["TrDate"] = Convert.ToDateTime(dtFrom).ToShortDateString();
            dr1["Inv"] = "";
            dr1["Link"] = "";
            dr1["Description"] = "Openning Balance";
            dr1["Dr"] = 0;
            dr1["Cr"] = 0;
            dr1["Balance"] = currBal;
            dt1.Rows.Add(dr1);

            query = " where  TrType = 'Customer'  " + customer + invDate + closeDate;
            SqlCommand cmd2 = new SqlCommand("SELECT [TrDate], InvNo, TrGroup, TrType, [Description], [Dr], [Cr], [Balance] FROM [Transactions] " + query + "  ORDER BY [TrDate] ASC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            //cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = cust;
            //cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
            //cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(dtTo).AddDays(+1).ToShortDateString();

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
                    currBal = debt - credit + currBal;

                    string inv = ds.Tables[0].Rows[ic]["InvNo"].ToString();
                    string trGroup = ds.Tables[0].Rows[ic]["TrGroup"].ToString();
                    string link = "#";
                    if (trGroup == "Sales")
                    {
                        //link = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=" + inv;
                    }

                    dr1 = dt1.NewRow();
                    dr1["TrDate"] = date;
                    dr1["Inv"] = inv;
                    dr1["Link"] = link;
                    dr1["Description"] = description;
                    dr1["Dr"] = debt;
                    dr1["Cr"] = credit;
                    dr1["Balance"] = currBal;

                    if (type == "Matured" && Convert.ToDateTime(lastMaturityDate) > Convert.ToDateTime(date))
                    {
                        dt1.Rows.Add(dr1);
                    }
                    else if (type == "Immatured" && Convert.ToDateTime(lastMaturityDate) < Convert.ToDateTime(date))
                    {
                        dt1.Rows.Add(dr1);
                    }
                    else if (type == "---ALL---")
                    {
                        dt1.Rows.Add(dr1);
                    }
                    ic++;

                } while (ic < recordcount);

            }
            else { }

            //set closing balance    

            dr1 = dt1.NewRow();
            dr1["TrDate"] = Convert.ToDateTime(dtTo).ToShortDateString();
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