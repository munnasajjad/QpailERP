using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Oxford.app
{
    public partial class Exp_History : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string dateM = DateTime.Now.Month.ToString();
                    string dateY = DateTime.Now.Year.ToString();

                    if (dateM.Length == 1)
                    {
                        dateM = "0" + dateM;
                    }
                    txtDateFrom.Text = "01/" + dateM + "/" + dateY;
                    txtDateTo.Text = DateTime.Now.ToShortDateString();
                    ddGroup.DataBind();
                    ddHead.Items.Clear();
                    ddHead.Items.Add("--- all ---");
                    ddHead.DataBind();
                    LoadGridData();
                }
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("Class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.ToString();
            }
        }

        protected void ddParties_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadGridData();
                SecondGrid();
                //lblMsg.Attributes.Add("Class", "xerp_success");
                //lblMsg.Text = "Ladger Loaded...";
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("Class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.ToString();
            }
        }

        private void LoadGridData()
        {
            SqlDataAdapter da;
            SqlDataReader dr;
            DataSet ds;
            int recordcount = 0;
            int ic = 0;

            //string server1 = " Server=" + ddServer.SelectedValue + " AND ";
            //string server2 = " Server: " + ddServer.SelectedItem.Text + ", ";
            //if (ddServer.SelectedValue == "--- all ---")
            //{
            //    server1 = "";
            //    server2 = "";
            //}

            string query = "";
            if (ddGroup.SelectedValue != "--- all ---")
            {
                query = " ExpGroup=" + ddGroup.SelectedValue + " AND ";
                //string customer2 = " ExpGroup: " + ddGroup.SelectedItem.Text + " ";

                if (ddHead.SelectedValue != "--- all ---")
                {
                    query = " ExpHead=" + ddHead.SelectedValue + " AND ";
                    //string reseller2 = ", ExpHead: " + ddHead.SelectedItem.Text + " ";
                }
            }

            SqlCommand cmd2 = new SqlCommand(@"SELECT   eid, ExpDate, ExpGroup, ExpHead, Description, Amount FROM [Expenses] WHERE " + query +
                    " ExpDate >= @DateFrom AND ExpDate <= @DateTo ORDER BY ExpDate, [eid] ASC",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            //cmd2.Parameters.Add("@PartyName", SqlDbType.VarChar).Value = ddParties.SelectedValue;
            cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateFrom.Text).ToShortDateString();
            cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateTo.Text).AddDays(+1).ToShortDateString();

            da = new SqlDataAdapter(cmd2);
            ds = new DataSet("Board");

            cmd2.Connection.Open();
            da.Fill(ds, "Board");

            dr = cmd2.ExecuteReader();
            recordcount = ds.Tables[0].Rows.Count;

            DataTable dt1 = new DataTable();
            DataRow dr1 = null;

            dt1.Columns.Add(new DataColumn("ExpDate", typeof(string)));
            dt1.Columns.Add(new DataColumn("ExpGroup", typeof(string)));

            dt1.Columns.Add(new DataColumn("ExpHead", typeof(string)));
            dt1.Columns.Add(new DataColumn("Description", typeof(string)));
            dt1.Columns.Add(new DataColumn("Amount", typeof(string)));
            cmd2.Connection.Close();

            string description;

            if (recordcount > 0)
            {
                do
                {
                    dr1 = dt1.NewRow();
                    dr1["ExpDate"] = Convert.ToDateTime(ds.Tables[0].Rows[ic]["ExpDate"].ToString()).ToString("dd/MM/yyyy");

                    string trType = ds.Tables[0].Rows[ic]["ExpGroup"].ToString();
                    dr1["ExpGroup"] = RunQuery.SQLQuery.ReturnString("Select Name FROM ExpenseTypes WHERE sl=" + trType);

                    string trHead = ds.Tables[0].Rows[ic]["ExpHead"].ToString();
                    dr1["ExpHead"] = RunQuery.SQLQuery.ReturnString("Select Name FROM ExpenseHeads WHERE sl=" + trHead);

                    dr1["Description"] = ds.Tables[0].Rows[ic]["Description"].ToString();
                    dr1["Amount"] = ds.Tables[0].Rows[ic]["Amount"].ToString();

                    dt1.Rows.Add(dr1);
                    ic++;

                } while (ic < recordcount);
            }
            else
            {
                //GridView1.DataSource = null;
            }
            GridView1.DataSource = dt1;
            GridView1.DataBind();

            //lblSummery.Text = server2 + customer2 + reseller2 + " Sales History from " + txtDateFrom.Text + " to " +
            //                 txtDateTo.Text;
        }

        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(txtDateFrom.Text) <= Convert.ToDateTime(txtDateTo.Text))
                {
                    LoadGridData();
                    SecondGrid();
                }
                else
                {
                    MessageBox("Invalid Date Range!");
                }
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("Class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.ToString();
            }
        }

        private void SecondGrid()
        {
            /*
        SqlCommand cmd = new SqlCommand("SELECT CollectionNo, CollectionDate, PartyName, PaidAmount, ChqDetail, ChqDate FROM Collection WHERE (IsApproved = 'p') and PartyName='" + ddParties.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet("Board");

        da.Fill(ds, "Board");
        SqlDataReader dr = cmd.ExecuteReader();
        DataTable dt = ds.Tables["Board"];

        cmd.Connection.Close();
        cmd.Connection.Dispose();
        GridView2.DataSource = dt;
        GridView2.DataBind();
        */
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //string url = "./rptLedger.aspx?party=" + ddParties.SelectedValue + "&dt1=" + txtDateFrom.Text + "&dt2=" + txtDateTo.Text;
            //ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");
        }

        public static class ResponseHelper
        {
            public static void Redirect(string url, string target, string windowFeatures)
            {
                HttpContext context = HttpContext.Current;

                if ((String.IsNullOrEmpty(target) ||
                     target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                    String.IsNullOrEmpty(windowFeatures))
                {

                    context.Response.Redirect(url);
                }
                else
                {
                    Page page = (Page)context.Handler;
                    if (page == null)
                    {
                        throw new InvalidOperationException(
                            "Cannot redirect to new window outside Page context.");
                    }
                    url = page.ResolveClientUrl(url);

                    string script;
                    if (!String.IsNullOrEmpty(windowFeatures))
                    {
                        script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                    }
                    else
                    {
                        script = @"window.open(""{0}"", ""{1}"");";
                    }

                    script = String.Format(script, url, target, windowFeatures);
                    ScriptManager.RegisterStartupScript(page,
                        typeof(Page),
                        "Redirect",
                        script,
                        true);
                }
            }
        }


        private decimal total = 0;
        private decimal TotalSales = (decimal)0.0;

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Amount"));
                //TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Cr"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].Text = "Total : ";
                e.Row.Cells[5].Text = Convert.ToString(total);
                //e.Row.Cells[11].Text = Convert.ToString(TotalSales); //String.Format("{0:c}", TotalSales);
            }
        }

        protected void ddGroup_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ddHead.Items.Clear();
            ListItem lst = new ListItem("--- all ---", "--- all ---");
            ddHead.Items.Insert(ddHead.Items.Count, lst);
            ddHead.DataBind();
            LoadGridData();
        }

        protected void ddHead_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGridData();
        }
    }
}