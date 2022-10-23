using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace Oxford.app
{
    public partial class VoucherCancel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InvIDNo();
                //txtAppDate.Text = DateTime.Now.ToShortDateString();
            }
        }
        //Messagebox For Alerts
        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        public void InvIDNo()
        {
            SqlCommand cmd = new SqlCommand("SELECT [VoucherNo] FROM [VoucherMaster] WHERE ([Voucherpost] = 'P') ORDER BY [VID]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            SqlDataReader dr1 = cmd.ExecuteReader();

            ddVID.DataSource = dr1;
            ddVID.DataValueField = "VoucherNo";
            ddVID.DataTextField = "VoucherNo";
            ddVID.DataBind();

            cmd.Connection.Close();
            cmd.Connection.Dispose();

            LoadVoucherDetail();
        }

        private void LoadVoucherDetail()
        {
            SqlCommand cmd = new SqlCommand("Select VoucherDate, ParticularID, VoucherEntryBy, VoucherEntryDate FROM VoucherMaster where VoucherNo='" + ddVID.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                txtDate.Text = Convert.ToString(Convert.ToDateTime(dr[0].ToString()).ToShortDateString());
                ddParticular.SelectedValue = dr[1].ToString();

            }
            cmd.Connection.Close();

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

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string item = e.Row.Cells[1].Text;
                foreach (Button button in e.Row.Cells[0].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                    }
                }
            }
        }
        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = (DataTable)ViewState["CurrentData"];
            dt.Rows[index].Delete();

            object sumDr = dt.Compute("Sum(Debit)", "");
            object sumCr = dt.Compute("Sum(Credit)", "");
            //txtPtotal.Text = Convert.ToString(sumP);
            //txtStotal.Text = Convert.ToString(sumS);

            ViewState["CurrentData"] = dt;
            int count = dt.Rows.Count;
            //ddProduct.SelectedValue = "--Select--";

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                saveData();

                DataTable dt = (DataTable)ViewState["CurrentData"];

                //dt.Clear();
                GridView2.DataSource = dt;

                //GridView2.Dispose();
                GridView2.DataBind();
                GridView1.DataBind();


                InvIDNo();
                MessageBox("The Voucher Cancelled Successfully.");

            }
            catch (Exception ex)
            {
                lblMsg.Text = "ERROR: " + ex.ToString();
                MessageBox("Unable to process your request! Please Check Error Message.");
            }
        }

        private void saveData()
        {
            SqlCommand cmd2x = new SqlCommand("UPDATE VoucherMaster set VoucherDate=@VoucherDate, Voucherpost='C', VoucherPostby=@VoucherPostby, Voucherpostdate=@Voucherpostdate where VoucherNo=@VoucherNo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            string invNo = ddVID.SelectedValue;
            string lName = Page.User.Identity.Name.ToString();

            cmd2x.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = invNo; //txtInv.Text;
            cmd2x.Parameters.Add("@VoucherDate", SqlDbType.DateTime).Value = txtDate.Text;
            cmd2x.Parameters.Add("@VoucherPostby", SqlDbType.VarChar).Value = lName;
            cmd2x.Parameters.Add("@Voucherpostdate", SqlDbType.DateTime).Value = DateTime.Now;


            cmd2x.Connection.Open();
            cmd2x.ExecuteNonQuery();
            cmd2x.Connection.Close();

            //Saving Gridview Data
            SqlCommand cmd2 = new SqlCommand("UPDATE VoucherDetails set EntryDate=@VoucherDate, ISApproved='C' where VoucherNo=@VoucherNo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = invNo; //txtInv.Text;
            cmd2.Parameters.Add("@VoucherDate", SqlDbType.DateTime).Value = txtDate.Text;

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string url = "./Reports/Voucher.aspx?Inv=" + ddVID.SelectedValue;
            ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");
        }
    }
}