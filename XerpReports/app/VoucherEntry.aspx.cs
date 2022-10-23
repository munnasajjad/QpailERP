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
    public partial class VoucherEntry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToShortDateString();
                InvIDNo();
                
                RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHead, "");
                lblUser.Text = Page.User.Identity.Name.ToString();
                btnSave.Enabled = false;
            }
        }
        //Messagebox For Alerts
        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        public string InvIDNo()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (max(VID),0)+1001 )) from VoucherMaster", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Connection.Open();
                string InvNo = Convert.ToString(cmd.ExecuteScalar());
                string year = Convert.ToString(Convert.ToDateTime(txtDate.Text).Year); // "2011"; // DateTime.Now.Year.ToString();
                InvNo = "V-" + year + "-" + InvNo;
                txtVID.Text = InvNo;
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                return InvNo;
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Invalid Date";
                SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (max(VID),0)+1001 )) from VoucherMaster", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Connection.Open();
                string InvNo = Convert.ToString(cmd.ExecuteScalar());
                string year = Convert.ToString(DateTime.Now.Year);
                InvNo = "V-" + year + "-" + InvNo;
                txtVID.Text = InvNo;
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                return InvNo;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string isValid = ddAccHead.SelectedItem.Text.Substring(0, 3);
                if (isValid.Trim() != "__")
                {
                    if (Convert.ToDecimal(txtAmount.Text) > 0 && txtAmount.Text != "")
                    {
                        if (btnAdd.Text == "Add Data to Grid")
                        {
                            string lName = Page.User.Identity.Name.ToString();
                            SqlCommand cmde = new SqlCommand("SELECT AccountsHeadID FROM VoucherTmp WHERE AccountsHeadID ='" + ddAccHead.SelectedValue + "' AND EntryBy ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                            cmde.Connection.Open();
                            string isExist = Convert.ToString(cmde.ExecuteScalar());
                            cmde.Connection.Close();

                            if (isExist == "")
                            {
                                InsertData();
                                lblMsg.Attributes.Add("class", "xerp_success");
                                lblMsg.Text = "New Voucher Saved successfully";
                            }
                            else
                            {
                                lblMsg.Attributes.Add("class", "xerp_warning");
                                lblMsg.Text = "ERROR: Head Already exist!";
                            }
                        }
                        else
                        {
                            ExecuteUpdate();
                            btnAdd.Text = "Add Data to Grid";
                            lblMsg.Attributes.Add("class", "xerp_success");
                            lblMsg.Text = "Entry updated successfully";
                        }

                        GridView2.DataBind();
                    }
                    else
                    {
                        lblMsg.Attributes.Add("class", "xerp_warning");
                        lblMsg.Text = "ERROR: Head Already exist!";
                    }

                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_warning");
                    lblMsg.Text = "ERROR: Please select accounts head!";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = ex.Message.ToString();
            }
        }

        private void InsertData()
        {
            string lName = Page.User.Identity.Name.ToString();

            SqlCommand cmd2 = new SqlCommand(@"INSERT INTO VoucherTmp (Particular, VoucherRowDescription, AccountsHeadID, AccountsHeadName, VoucherDR, VoucherCR, EntryDate, EntryBy)
                                VALUES (@Particular, @VoucherRowDescription, @AccountsHeadID, @AccountsHeadName, @VoucherDR, @VoucherCR, @EntryDate, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.AddWithValue("@Particular", ddParticular.SelectedValue);
            cmd2.Parameters.AddWithValue("@VoucherRowDescription", txtDescription.Text);
            cmd2.Parameters.AddWithValue("@AccountsHeadID", ddAccHead.SelectedValue);
            cmd2.Parameters.AddWithValue("@AccountsHeadName", ddAccHead.SelectedItem.Text);

            decimal dr = 0;
            decimal cr = 0;
            if (RadioButton1.Checked == true)
            {
                dr = Convert.ToDecimal(txtAmount.Text);
            }
            else
            {
                cr = Convert.ToDecimal(txtAmount.Text);
            }

            cmd2.Parameters.AddWithValue("@VoucherDR", dr);
            cmd2.Parameters.AddWithValue("@VoucherCR", cr);
            cmd2.Parameters.AddWithValue("@EntryDate", DateTime.Now);
            cmd2.Parameters.AddWithValue("@EntryBy", lName);

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
        }

        private void ExecuteUpdate()
        {
            string lName = Page.User.Identity.Name.ToString();
            SqlCommand cmd2 = new SqlCommand("UPDATE VoucherTmp SET Particular='" + ddParticular.SelectedValue + "', VoucherRowDescription='" + txtDescription.Text + "', AccountsHeadID='" + ddAccHead.SelectedValue + "'," +
                                    "AccountsHeadName='" + ddAccHead.SelectedItem.Text + "', VoucherDR=@VoucherDR, VoucherCR=@VoucherCR  where (SerialNo ='" + lblSl.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            decimal dr = 0;
            decimal cr = 0;
            if (RadioButton1.Checked == true)
            {
                dr = Convert.ToDecimal(txtAmount.Text);
            }
            else
            {
                cr = Convert.ToDecimal(txtAmount.Text);
            }

            cmd2.Parameters.AddWithValue("@VoucherDR", dr);
            cmd2.Parameters.AddWithValue("@VoucherCR", cr);

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
            cmd2.Connection.Dispose();
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal ttl = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("Select SUM(VoucherDR)-Sum(VoucherCR) FROM VoucherTmp where EntryBy='" + Page.User.Identity.Name.ToString() + "'"));
            txtTTL.Text = RunQuery.SQLQuery.ReturnString("Select SUM(VoucherDR) FROM VoucherTmp where EntryBy='" + Page.User.Identity.Name.ToString() + "'");

            if (ttl == 0)
            {
                btnSave.Enabled = true;
            }
        }
        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.RowIndex);
                Label lblItemCode = GridView2.Rows[index].FindControl("lblSl") as Label;

                RunQuery.SQLQuery.ExecNonQry("DELETE VoucherTmp WHERE SerialNo=" + lblItemCode.Text);

                GridView2.DataBind();
                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "Entry removed successfully ...";

            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.Message.ToString();
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                decimal ttl = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("Select SUM(VoucherDR)-Sum(VoucherCR) FROM VoucherTmp where EntryBy='" + Page.User.Identity.Name.ToString() + "'"));
                if (ttl == 0)
                {
                    InvIDNo();
                    string invID = saveData();
                    RunQuery.SQLQuery.ExecNonQry("Delete VoucherTmp where EntryBy='" + Page.User.Identity.Name.ToString() + "'");
                    InvIDNo();
                    btnSave.Enabled = false;
                    GridView2.DataBind();
                    GridView1.DataBind();
                    txtTTL.Text = "0";

                    if (chkPrint.Checked)
                    {
                        string url = "./Voucher.aspx?Inv=" + invID;
                        ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");
                        Response.Write("<script>window.open(" + url + ", '_blank' );</script>");
                        //Response.Redirect = "Purchase.aspx";
                    }

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "New Voucher Saved Successfully.";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.Message.ToString();
            }
        }

        private string saveData()
        {
            SqlCommand cmd2x = new SqlCommand("INSERT INTO VoucherMaster (VoucherNo, VoucherDate, VoucherDescription, ParticularID, VoucherEntryBy, VoucherAmount)" +
                                                "VALUES (@VoucherNo, @VoucherDate, @VoucherDescription, @ParticularID, @VoucherEntryBy, @VoucherAmount)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            string invNo = InvIDNo();
            string lName = Page.User.Identity.Name.ToString();
            cmd2x.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = invNo; //
            cmd2x.Parameters.Add("@VoucherDate", SqlDbType.DateTime).Value = txtDate.Text;
            cmd2x.Parameters.Add("@VoucherDescription", SqlDbType.VarChar).Value = ddParticular.SelectedItem.Text;
            cmd2x.Parameters.Add("@ParticularID", SqlDbType.NVarChar).Value = RunQuery.SQLQuery.ReturnString("Select Particularsid FROM Particulars WHERE Particularsname='" + ddParticular.SelectedItem.Text + "'");
            cmd2x.Parameters.Add("@VoucherEntryBy", SqlDbType.VarChar).Value = lName;
            cmd2x.Parameters.Add("@VoucherAmount", SqlDbType.Decimal).Value = txtTTL.Text;

            //if (ddVendor.SelectedValue != "Select" && ddCustomer.SelectedValue != "Select" &&  ddProduct.SelectedValue != "Select")
            //{
            cmd2x.Connection.Open();
            int success = cmd2x.ExecuteNonQuery();
            cmd2x.Connection.Close();

            //Saving Gridview Data
            string acHead; string description; string amount; string dr; string cr;
            string vDetail = ddParticular.SelectedValue;

            foreach (GridViewRow GVRow in GridView2.Rows)
            {
                Label lblSl = GVRow.FindControl("lblSl") as Label;
                Label lblHeadId = GVRow.FindControl("lblHeadId") as Label;
                Label lblHeadName = GVRow.FindControl("lblHeadName") as Label;
                Label lblDesc = GVRow.FindControl("lblDesc") as Label;
                Label lblDr = GVRow.FindControl("lblDr") as Label;
                Label lblCr = GVRow.FindControl("lblCr") as Label;

                acHead = HttpUtility.HtmlDecode(lblHeadId.Text);
                string acHeadID = lblHeadId.Text;

                //insert details       
                SqlCommand cmd2y = new SqlCommand("INSERT INTO VoucherDetails (VoucherNo, VoucherRowDescription, AccountsHeadID, AccountsHeadName, VoucherDR, VoucherCR, EntryDate)" +
                                                    "VALUES (@VoucherNo, @VoucherRowDescription, @AccountsHeadID, @AccountsHeadName, @VoucherDR, @VoucherCR, @EntryDate)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2y.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = invNo;
                cmd2y.Parameters.Add("@VoucherRowDescription", SqlDbType.VarChar).Value = HttpUtility.HtmlDecode(lblDesc.Text);
                cmd2y.Parameters.Add("@AccountsHeadID", SqlDbType.VarChar).Value = acHeadID;
                cmd2y.Parameters.Add("@AccountsHeadName", SqlDbType.VarChar).Value = lblHeadName.Text;
                cmd2y.Parameters.Add("@VoucherDR", SqlDbType.Decimal).Value = lblDr.Text;
                cmd2y.Parameters.Add("@VoucherCR", SqlDbType.Decimal).Value = lblCr.Text;
                cmd2y.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = txtDate.Text;

                cmd2y.Connection.Open();
                cmd2y.ExecuteNonQuery();
                cmd2y.Connection.Close();
            }
            return invNo;
        }

        private void PopulateDropDown(DropDownList ddl, string query1, string query2)
        {


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

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ddAccHead.Items.Clear();
            RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHead, "");
        }
        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(GridView2.SelectedIndex);
                Label Label2 = GridView2.Rows[index].FindControl("lblSl") as Label;
                lblSl.Text = Label2.Text;

                SqlCommand cmd7 = new SqlCommand("SELECT [VoucherRowDescription], [AccountsHeadID], [AccountsHeadName], [VoucherDR], [VoucherCR]  FROM [VoucherTmp] WHERE SerialNo ='" + lblSl.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                SqlDataReader dr = cmd7.ExecuteReader();

                if (dr.Read())
                {
                    btnAdd.Text = "Update";
                    txtDescription.Text = dr[0].ToString();
                    ddAccHead.SelectedValue = dr[1].ToString();
                    //ddControl.SelectedValue = dr[2].ToString();
                    decimal bldr = Convert.ToDecimal(dr[3].ToString());
                    decimal blcr = Convert.ToDecimal(dr[4].ToString());

                    if (bldr > 0)
                    {
                        txtAmount.Text = bldr.ToString();
                        RadioButton1.Checked = true;
                        RadioButton2.Checked = false;
                    }
                    else
                    {
                        txtAmount.Text = blcr.ToString();
                        RadioButton2.Checked = true;
                        RadioButton1.Checked = false;
                    }

                    lblMsg.Attributes.Add("class", "xerp_info");
                    lblMsg.Text = "A/C info loaded in edit mode";
                }

                cmd7.Connection.Close();
                pan.Update();
            }
            catch (Exception ex)
            {
                lblMsg.CssClass = "xerp_error";
                lblMsg.Text = "ERROR: " + ex.ToString();
            }
        }
    }
}