using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using RunQuery;

namespace Oxford.XerpReports
{
    public partial class FormBankBook : System.Web.UI.Page
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
            string item = Convert.ToString(Request.QueryString["bank"]);
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);

            DataTable dtx = GetDatable(item, dateFrom, dateTo);

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.AccSummery);
            
            rpt.Load(Server.MapPath("CrptBankBook.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            string rptName = SQLQuery.ReturnString("SELECT (Select [BankName] FROM [Banks] where [BankId]=BankAccounts.BankID) +' - '+ACNo +' - '+ACName AS Bank from BankAccounts WHERE ACID='" + item + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;

        }
        private DataTable GetDatable(string ddBank, string txtDateFrom, string txtDateTo)
        {
            string customer = " "; string party = " where ACID ='" + ddBank + "'";

            if (ddBank != "---ALL---")
            {
                customer = " AND HeadID ='" + ddBank + "'";
            }

            decimal opBal = Convert.ToDecimal(SQLQuery.ReturnString("Select OpBalance FROM BankAccounts " + party));

            string opDate = " "; string invDate = " ";
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
            dt1.Columns.Add(new DataColumn("HEAD", typeof(string)));
            dt1.Columns.Add(new DataColumn("VoucherNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("Description", typeof(string)));
            dt1.Columns.Add(new DataColumn("Dr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Cr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));

            int recordcount = 0;
            int ic = 0;

            decimal debt = 0; decimal credit = 0;
            string date; string description;
            //get opening balance
            string query = " where TrType = 'Bank' " + customer;
            decimal currBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + query + opDate));

            decimal closeBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + query + closeDate));
            
            dr1 = dt1.NewRow();
            dr1["TrDate"] = Convert.ToDateTime(txtDateFrom).ToShortDateString();
            dr1["Inv"] = "";
            dr1["HEAD"] = "";
            dr1["Description"] = "Openning Balance";
            dr1["Dr"] = 0;
            dr1["Cr"] = 0;
            dr1["Balance"] = currBal;
            dt1.Rows.Add(dr1);

            query += invDate + closeDate;
            SqlCommand cmd2 = new SqlCommand("SELECT [TrDate], InvNo, TrGroup, TrType, [Description], [Dr], [Cr], [Balance] FROM [Transactions] " + query + "  ORDER BY [TrDate], TrID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            //cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = ddBank;
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
                    currBal = debt - credit + currBal;

                    string inv = ds.Tables[0].Rows[ic]["InvNo"].ToString();
                    string trGroup = ds.Tables[0].Rows[ic]["TrGroup"].ToString();

                    string lnk = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "/app/reports/voucher.aspx?inv=" + inv;
                    string link = SQLQuery.ReturnString("Select TOP(1) SerialNo from VoucherTmp where VoucherNo='" + inv + "' AND (Head5Dr='" + ddBank + "' OR Head5Cr='" + ddBank + "') AND (Amount= '" + debt + "' OR Amount= '" + credit + "') ");
                    //string compare=SQLQuery.ReturnString("Select AccountsHeadDrName ")
                    string dsc = description;

                    //if (trGroup == "Sales")
                    //{
                    //    link = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=" + inv;
                    //}

                    dr1 = dt1.NewRow();
                    dr1["TrDate"] = date;
                    //dr1["Inv"] = inv;
                    dr1["HEAD"] = "View";
                    dr1["VoucherNo"] = inv;
                    dr1["Description"] = dsc; 
                    dr1["Dr"] = debt;
                    dr1["Cr"] = credit;
                    dr1["Balance"] = currBal;

                    //if (ddStatus.SelectedValue == "Matured" && Convert.ToDateTime(lastMaturityDate) > Convert.ToDateTime(date))
                    //{
                    //    dt1.Rows.Add(dr1);
                    //}
                    //else if (ddStatus.SelectedValue == "Immatured" && Convert.ToDateTime(lastMaturityDate) < Convert.ToDateTime(date))
                    //{
                    //    dt1.Rows.Add(dr1);
                    //}
                    //else if (ddStatus.SelectedValue == "---ALL---")
                    //{
                    dt1.Rows.Add(dr1);
                    //}
                    ic++;

                } while (ic < recordcount);

            }
            else { }

            //set closing balance

            dr1 = dt1.NewRow();
            dr1["TrDate"] = Convert.ToDateTime(txtDateTo).ToShortDateString();
            dr1["Inv"] = "";
            dr1["HEAD"] = "";
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
        protected void viewmode(object sender, EventArgs e)
        {
            
        }


    }
}