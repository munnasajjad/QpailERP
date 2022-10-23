using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
//For converting HTML TO PDF- START
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;
using System.Text.RegularExpressions;

public partial class Application_VoucherApproval : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToShortDateString();
            InvIDNo();

            ddParticular.DataBind();
            PopulateAcHeads();

            lblUser.Text = Page.User.Identity.Name.ToString();
            //btnSave.Enabled = false;
        }
    }

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
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
            //ddParticular.SelectedValue = dr[1].ToString();

        }
        cmd.Connection.Close();

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string isValid = ddAccHeadDr.SelectedItem.Text.Substring(0, 3);
            if (isValid.Trim() != "__")
            {
                isValid = ddAccHeadCr.SelectedItem.Text.Substring(0, 3);
            }
            else
            {
                ddAccHeadDr.Focus();
                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "ERROR: Please select accounts head!";
                return;
            }

            if (isValid.Trim() != "__")
            {
                if (Convert.ToDecimal(txtAmount.Text) > 0 && txtAmount.Text != "")
                {
                    if (btnAdd.Text == "Add to Grid")
                    {
                        InsertData();
                    }
                    else
                    {
                        ExecuteUpdate();
                        btnAdd.Text = "Add to Grid";
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Entry updated successfully";
                    }

                    txtDescription.Text = "";
                    txtAmount.Text = "";
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
                ddAccHeadCr.Focus();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = ex.ToString();
            txtAmount.Focus();
        }
    }

    private void InsertData()
    {
        string lName = Page.User.Identity.Name.ToString();

        SqlCommand cmd2 = new SqlCommand(@"INSERT INTO VoucherTmp (Particular, VoucherRowDescription, AccountsHeadDr, AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName, Amount, EntryDate, EntryBy)
                                VALUES (@Particular, @VoucherRowDescription, @AccountsHeadDr, @AccountsHeadDrName, @AccountsHeadCr, @AccountsHeadCrName, @Amount, @EntryDate, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@Particular", ddParticular.SelectedValue);
        cmd2.Parameters.AddWithValue("@VoucherRowDescription", txtDescription.Text);
        cmd2.Parameters.AddWithValue("@AccountsHeadDr", ddAccHeadDr.SelectedValue);
        cmd2.Parameters.AddWithValue("@AccountsHeadDrName", ddAccHeadDr.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@AccountsHeadCr", ddAccHeadCr.SelectedValue);
        cmd2.Parameters.AddWithValue("@AccountsHeadCrName", ddAccHeadCr.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@Amount", txtAmount.Text);
        cmd2.Parameters.AddWithValue("@EntryDate", DateTime.Now);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd2 = new SqlCommand("UPDATE VoucherTmp SET Particular='" + ddParticular.SelectedValue + "', VoucherRowDescription='" + txtDescription.Text + "', AccountsHeadDr='" + ddAccHeadDr.SelectedValue + "'," +
                                "AccountsHeadDrName='" + ddAccHeadDr.SelectedItem.Text + "', AccountsHeadCr='" + ddAccHeadCr.SelectedValue + "',AccountsHeadCrName='" + ddAccHeadCr.SelectedItem.Text + "', Amount=@Amount  where (SerialNo ='" + lblSl.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@Amount", txtAmount.Text);
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        txtTTL.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp where EntryBy='" + Page.User.Identity.Name.ToString() + "'");
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
            btnAdd.Text = "Add to Grid";
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "ERROR: " + ex.ToString();
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            decimal ttl = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp where EntryBy='" + Page.User.Identity.Name.ToString() + "'"));
            if (ttl > 0)
            {
                InvIDNo();
                string invID = saveData();
                RunQuery.SQLQuery.ExecNonQry("Delete VoucherTmp where EntryBy='" + Page.User.Identity.Name.ToString() + "'");
                InvIDNo();
                //btnSave.Enabled = false;
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
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: No data was found into current voucher!";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }

    private string saveData()
    {
        SqlCommand cmd2x = new SqlCommand("INSERT INTO VoucherMaster (VoucherNo, VoucherDate, VoucherDescription, ParticularID, VoucherEntryBy, VoucherAmount)" +
                                            "VALUES (@VoucherNo, @VoucherDate, @VoucherDescription, @ParticularID, @VoucherEntryBy, @VoucherAmount)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        string invNo = ddVID.SelectedValue;
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
        string acHead; string acHeadID; string description; string amount; string dr; string cr;
        string vDetail = ddParticular.SelectedValue;

        foreach (GridViewRow GVRow in GridView2.Rows)
        {
            Label lblSl = GVRow.FindControl("lblSl") as Label;
            Label lblHeadDr = GVRow.FindControl("lblHeadIdDr") as Label;
            Label lblHeadNameDr = GVRow.FindControl("lblHeadNameDr") as Label;
            Label lblHeadCr = GVRow.FindControl("lblHeadIdCr") as Label;
            Label lblHeadNameCr = GVRow.FindControl("lblHeadNameCr") as Label;
            Label lblDesc = GVRow.FindControl("lblDesc") as Label;
            Label lblDr = GVRow.FindControl("lblDr") as Label;

            //Dr insert
            acHeadID = lblHeadDr.Text;
            acHead = HttpUtility.HtmlDecode(lblHeadNameDr.Text);

            SqlCommand cmd2y = new SqlCommand("INSERT INTO VoucherDetails (VoucherNo, VoucherRowDescription, AccountsHeadID, AccountsHeadName, VoucherDR, VoucherCR, EntryDate)" +
                                                "VALUES (@VoucherNo, @VoucherRowDescription, @AccountsHeadID, @AccountsHeadName, @VoucherDR, @VoucherCR, @EntryDate)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2y.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = invNo;
            cmd2y.Parameters.Add("@VoucherRowDescription", SqlDbType.VarChar).Value = HttpUtility.HtmlDecode(lblDesc.Text);
            cmd2y.Parameters.Add("@AccountsHeadID", SqlDbType.VarChar).Value = acHeadID;
            cmd2y.Parameters.Add("@AccountsHeadName", SqlDbType.VarChar).Value = acHead;
            cmd2y.Parameters.Add("@VoucherDR", SqlDbType.Decimal).Value = lblDr.Text;
            cmd2y.Parameters.Add("@VoucherCR", SqlDbType.Decimal).Value = "0";
            cmd2y.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = txtDate.Text;
            cmd2y.Connection.Open();
            cmd2y.ExecuteNonQuery();
            cmd2y.Connection.Close();

            TransactionEntry(acHeadID, lblDr.Text, "0", invNo, "dr");

            //Cr Insert
            acHeadID = lblHeadCr.Text;
            acHead = HttpUtility.HtmlDecode(lblHeadNameCr.Text);

            cmd2y = new SqlCommand("INSERT INTO VoucherDetails (VoucherNo, VoucherRowDescription, AccountsHeadID, AccountsHeadName, VoucherDR, VoucherCR, EntryDate)" +
                                                "VALUES (@VoucherNo, @VoucherRowDescription, @AccountsHeadID, @AccountsHeadName, @VoucherDR, @VoucherCR, @EntryDate)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2y.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = invNo;
            cmd2y.Parameters.Add("@VoucherRowDescription", SqlDbType.VarChar).Value = HttpUtility.HtmlDecode(lblDesc.Text);
            cmd2y.Parameters.Add("@AccountsHeadID", SqlDbType.VarChar).Value = acHeadID;
            cmd2y.Parameters.Add("@AccountsHeadName", SqlDbType.VarChar).Value = acHead;
            cmd2y.Parameters.Add("@VoucherDR", SqlDbType.Decimal).Value = "0";
            cmd2y.Parameters.Add("@VoucherCR", SqlDbType.Decimal).Value = lblDr.Text;
            cmd2y.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = txtDate.Text;
            cmd2y.Connection.Open();
            cmd2y.ExecuteNonQuery();
            cmd2y.Connection.Close();

            TransactionEntry(acHeadID, "0", lblDr.Text, invNo, "cr");
        }
        return invNo;
    }

    private void TransactionEntry(string acHeadID, string dr, string cr, string invNo, string headType)
    {
        string lName = Page.User.Identity.Name.ToString();
        string controlId = RunQuery.SQLQuery.ReturnString("Select ControlAccountsID from HeadSetup where AccountsHeadID='" + acHeadID + "'");
        //Auto Transaction Entry: Party/Vendor (020101), Customer (), Cash (010101), Bank (010102)
        if (controlId == "020101")//Payables/Liabilities
        {
            string supplierId = RunQuery.SQLQuery.ReturnString("Select PID from Vendors where SyncAccountHead='" + acHeadID + "'");
            //Accounting.VoucherEntry.TransactionEntry(txtDate.Text, supplierId, "", "", "", "0", "0", "", "0", dr, cr, "0", "Auto Entry From Voucher Entry", "Vendor", "Purchase", lName, "", "0", invNo);
        }
        else if (controlId == "010105")
        {
            string customerId = RunQuery.SQLQuery.ReturnString("Select PID from Resellers where SyncAccountHead='" + acHeadID + "'");
            //Accounting.VoucherEntry.TransactionEntry(txtDate.Text, customerId, "", "", "", "0", "0", "", "0", dr, cr, "0", "Auto Entry From Voucher Entry", "Customer", "Sales", lName, "", "0", invNo);
        }
        else if (controlId == "010101")
        {

            string bankId = RunQuery.SQLQuery.ReturnString("Select PID from VACCBankCash where SyncAccountHead='" + acHeadID + "'");
            if (headType == "dr")
            {
                //Accounting.VoucherEntry.TransactionEntry(txtDate.Text, "", "", "Cash", bankId, "0", "0", "", "0", dr, cr, "0", "Auto Entry From Voucher Posting", "Bank", ddParticular.SelectedItem.Text, lName, "", "0", invNo);
            }
            else
            {
                //Accounting.VoucherEntry.TransactionEntry(txtDate.Text, "", "", "Cash", bankId, "0", "0", "", "0", cr, dr, "0", "Auto Entry From Voucher Posting", "Bank", ddParticular.SelectedItem.Text, lName, "", "0", invNo);
            }
        }
        else if (controlId == "010102")
        {
            string bankId = RunQuery.SQLQuery.ReturnString("Select PID from VACCBankCash where SyncAccountHead='" + acHeadID + "'");
            if (ddAccHeadDr.SelectedValue == "010102")
            {
                //Accounting.VoucherEntry.TransactionEntry(txtDate.Text, "", "", "Bank", bankId, "0", "0", "", "0", dr, cr, "0", "Auto Entry From Voucher Posting", "Bank", ddParticular.SelectedItem.Text, lName, "", "0", invNo);
            }
            else
            {
                //Accounting.VoucherEntry.TransactionEntry(txtDate.Text, "", "", "Bank", bankId, "0", "0", "", "0", cr, dr, "0", "Auto Entry From Voucher Posting", "Bank", ddParticular.SelectedItem.Text, lName, "", "0", invNo);
            }
        }
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
        PopulateAcHeads();
    }
    protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView2.SelectedIndex);
            Label Label2 = GridView2.Rows[index].FindControl("lblSl") as Label;
            lblSl.Text = Label2.Text;

            PopulateAcHeads();

            SqlCommand cmd7 = new SqlCommand("SELECT [VoucherRowDescription], AccountsHeadDr, AccountsHeadCr, Amount  FROM [VoucherTmp] WHERE SerialNo ='" + lblSl.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();

            if (dr.Read())
            {
                btnAdd.Text = "Update";
                txtDescription.Text = dr[0].ToString();
                try
                {
                    ddAccHeadDr.SelectedValue = dr[1].ToString();
                    ddAccHeadCr.SelectedValue = dr[2].ToString();
                }
                catch
                {
                    RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHeadDr, "");
                    RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHeadCr, "");
                    ddAccHeadDr.SelectedValue = dr[1].ToString();
                    ddAccHeadCr.SelectedValue = dr[2].ToString();
                }

                txtAmount.Text = Convert.ToString(dr[3].ToString());

                lblMsg.Attributes.Add("class", "xerp_info");
                lblMsg.Text = "A/C info loaded in edit mode";
            }

            cmd7.Connection.Close();
            //pan.Update();
        }
        catch (Exception ex)
        {
            lblMsg.CssClass = "xerp_error";
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "ERROR: " + ex.ToString();

        }
    }

    private void PopulateAcHeads()
    {
        try
        {
            ddAccHeadDr.Items.Clear();
            ddAccHeadCr.Items.Clear();

            if (ddParticular.SelectedItem.Text == "Salary")
            {
                RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND ControlAccountsID='040114' ");
                RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND AccountsID='0101' ");
            }
            else if (ddParticular.SelectedItem.Text == "Receipt")
            {
                RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND (ControlAccountsID='010101'  OR  ControlAccountsID='010102')  ");
                RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND GroupID<>'04' AND GroupID<>'02' AND  ControlAccountsID<>'010101' AND  ControlAccountsID<>'010102'  ");
            }
            else if (ddParticular.SelectedItem.Text == "Purchase")
            {
                RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND GroupID='04' "); //Expense
                RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND AccountsID='0201' ");//Suppliers
            }
            else if (ddParticular.SelectedItem.Text == "Sales")
            {
                RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND ControlAccountsID='010105' "); //Customer
                RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND GroupID='03' "); //Income
            }
            else if (ddParticular.SelectedItem.Text == "Payment")
            {
                RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND GroupID<>'01'  AND GroupID<>'03' ");//exclude assets & income
                RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND (ControlAccountsID='010101'  OR  ControlAccountsID='010102') "); // only cash & bank
            }
            else // Journal
            {
                RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHeadDr, "");
                RunQuery.SQLQuery.PopulateMultiDropDown(ddAccHeadCr, "");
            }
        }
        catch (Exception ex)
        {
            lblMsg.CssClass = "xerp_error";
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }

    protected void ddParticular_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateAcHeads();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtDate.Text = DateTime.Now.ToShortDateString();
        InvIDNo();
        ddParticular.DataBind();
        PopulateAcHeads();

        RunQuery.SQLQuery.ExecNonQry("Delete VoucherTmp where EntryBy='" + Page.User.Identity.Name.ToString() + "'");
    }

}
