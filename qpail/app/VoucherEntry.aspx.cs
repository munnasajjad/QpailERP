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
using System.Web.UI.WebControls.Adapters;
using Accounting;
using RunQuery;

public partial class Application_VoucherEntry : System.Web.UI.Page
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
            PopulateSubAcc();

            lblUser.Text = Page.User.Identity.Name.ToString();
            BindItemGrid();

            string editId = Convert.ToString(Request.QueryString["id"]);
            if (editId != null)
            {
                LoadEditMode(editId);
            }

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
            Notify(ex.Message.ToString(), "error", lblMsg);
            return InvNo;
        }
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
                    BindItemGrid();
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

        string h5NameDr = "";
        if (ddHead5Dr.SelectedValue != "")
        {
            h5NameDr = ddHead5Dr.SelectedItem.Text;
        }
        string h5NameCr = "";
        if (ddHead5Cr.SelectedValue != "")
        {
            h5NameCr = ddHead5Cr.SelectedItem.Text;
        }

        SqlCommand cmd2 = new SqlCommand();
        if (btnSave.Text != "Update")
        {
            cmd2 = new SqlCommand(@"INSERT INTO VoucherTmp (VoucherNo, Particular, VoucherRowDescription, AccountsHeadDr, AccountsHeadDrName, Head5Dr, Name5Dr, AccountsHeadCr, AccountsHeadCrName, Head5Cr, Name5Cr, Amount, EntryDate, EntryBy)
                                VALUES ('', @Particular, @VoucherRowDescription, @AccountsHeadDr, @AccountsHeadDrName, '" + ddHead5Dr.SelectedValue + "', '" + h5NameDr + "', @AccountsHeadCr, @AccountsHeadCrName, '" + ddHead5Cr.SelectedValue + "', '" + h5NameCr + "', @Amount, @EntryDate, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }
        else
        {
            cmd2 = new SqlCommand(@"INSERT INTO VoucherTmp (VoucherNo, Particular, VoucherRowDescription, AccountsHeadDr, AccountsHeadDrName, Head5Dr, Name5Dr, AccountsHeadCr, AccountsHeadCrName, Head5Cr, Name5Cr, Amount, EntryDate, EntryBy)
                                VALUES (@VoucherNo, @Particular, @VoucherRowDescription, @AccountsHeadDr, @AccountsHeadDrName, '" + ddHead5Dr.SelectedValue + "', '" + h5NameDr + "', @AccountsHeadCr, @AccountsHeadCrName, '" + ddHead5Cr.SelectedValue + "', '" + h5NameCr + "', @Amount, @EntryDate, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }

        cmd2.Parameters.AddWithValue("@VoucherNo", txtEditVoucherNo.Text);
        cmd2.Parameters.AddWithValue("@Particular", ddParticular.SelectedValue);
        cmd2.Parameters.AddWithValue("@VoucherRowDescription", txtDescription.Text);
        cmd2.Parameters.AddWithValue("@AccountsHeadDr", ddAccHeadDr.SelectedValue);
        cmd2.Parameters.AddWithValue("@AccountsHeadDrName", ddAccHeadDr.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@AccountsHeadCr", ddAccHeadCr.SelectedValue);
        cmd2.Parameters.AddWithValue("@AccountsHeadCrName", ddAccHeadCr.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@Amount", txtAmount.Text);
        cmd2.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        string lName = Page.User.Identity.Name.ToString();

        string h5NameDr = "";
        if (ddHead5Dr.SelectedValue != "")
        {
            h5NameDr = ddHead5Dr.SelectedItem.Text;
        }
        string h5NameCr = "";
        if (ddHead5Cr.SelectedValue != "")
        {
            h5NameCr = ddHead5Cr.SelectedItem.Text;
        }
        string query = "UPDATE VoucherTmp SET Particular='" + ddParticular.SelectedValue + "', VoucherRowDescription='" +
                       txtDescription.Text + "', AccountsHeadDr='" + ddAccHeadDr.SelectedValue + "'," +
                       "AccountsHeadDrName='" + ddAccHeadDr.SelectedItem.Text + "', Head5Dr='" + ddHead5Dr.SelectedValue +
                       "', Name5Dr='" + h5NameDr + "', AccountsHeadCr='" + ddAccHeadCr.SelectedValue +
                       "',AccountsHeadCrName='" + ddAccHeadCr.SelectedItem.Text + "', Head5Cr='" +
                       ddHead5Cr.SelectedValue + "', Name5Cr='" + h5NameCr + "', Amount=@Amount  where (SerialNo ='" +
                       lblSl.Text + "')";
        SqlCommand cmd2 = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@Amount", txtAmount.Text);
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //txtTTL.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp where VoucherNo='' AND EntryBy='" + Page.User.Identity.Name.ToString() + "'");
    }
    protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView2.Rows[index].FindControl("lblSl") as Label;

            if (btnSave.Text == "Update")
            {
                SQLQuery.ExecNonQry(@"INSERT INTO VoucherTmpDeleteHistory SELECT SerialNo, VoucherNo, Particular, VoucherRowDescription, AccountsHeadDr, AccountsHeadDrName, Head5Dr, Name5Dr, AccountsHeadCr, AccountsHeadCrName, Head5Cr, Name5Cr, Amount, projectName, EntryDate, 
                         ISApproved, EntryBy, Qty, Pcs, rate FROM VoucherTmp WHERE SerialNo ='" + lblItemCode.Text + "'");
                SQLQuery.ExecNonQry("UPDATE VoucherTmpDeleteHistory SET  ISApproved='D', EntryBy='" + User.Identity.Name + "'  WHERE SerialNo=(Select MAX(SerialNo) from VoucherTmpDeleteHistory)");
            }


            SQLQuery.ExecNonQry("DELETE VoucherTmp WHERE SerialNo=" + lblItemCode.Text);

            BindItemGrid();
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
            string lName = Page.User.Identity.Name.ToLower().ToString();

            //Block the back date data entry
            string permLevel = SQLQuery.ReturnString("Select UserLevel from Logins where LoginUserName='" + User.Identity.Name + "'");
            if ((Convert.ToInt32(permLevel) >= 1) && (lName != "md aminul") && (lName != "showkat") && (lName != "forhad") && (lName != "ahtesam") && (lName != "nasir123") && (lName != "riaz") && (lName != "zahid") && (lName != "zakir") && (lName != "nazrul") && Convert.ToDateTime(txtDate.Text) <= DateTime.Today.AddDays(-5)) //Permitted Only for super Admin
            {
                string leftDate = DateTime.Today.AddMonths(-2).ToString("dd/MM/yyyy");
                string vDate = Convert.ToDateTime(txtDate.Text).ToString("dd/MM/yyyy");

                if ((Convert.ToInt32(permLevel) >= 1) && (lName == "shakib" || lName == "saiful") && Convert.ToDateTime(vDate) >= Convert.ToDateTime(leftDate)) //Permitted Only for super Admin
                {
                    //Notify("You're not authorized for back dated voucher entry!", "error", lblMsg);
                    //return;
                }
                else
                {
                    
                    Notify("You're not authorized for back dated voucher entry!", "error", lblMsg);
                    return;
                }
                
            }


            string query = "";
            if (txtEditVoucherNo.Text == "")
            {
                query = " AND EntryBy='" + lName + "'";
            }

            decimal ttl = Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp  WHERE VoucherNo='" + txtEditVoucherNo.Text + "' " + query));
            if (ttl > 0)
            {
                string voucherNo = InvIDNo();
                
                if (btnSave.Text == "Update")
                {
                    voucherNo = txtEditVoucherNo.Text;
                    SQLQuery.ExecNonQry("Delete VoucherMaster where VoucherNo='" + voucherNo + "'");
                    SQLQuery.ExecNonQry("Delete VoucherDetails where VoucherNo='" + voucherNo + "'");
                    SQLQuery.ExecNonQry("Delete Transactions where InvNo='" + voucherNo + "'");
                }

                saveData(voucherNo);

                SQLQuery.ExecNonQry(@"INSERT INTO VoucherMasterEditHistory
                                                    SELECT        VID, VoucherNo, VoucherDate, VoucherDescription, ParticularID, VoucherEntryBy, VoucherEntryDate, Voucherpost, VoucherPostby, Voucherpostdate, VoucherReferenceNo, VoucherAmount, projectName, VoucherEntryTime
                                                    FROM            VoucherMaster WHERE        VoucherNo =  '" + voucherNo + "'");

                SQLQuery.ExecNonQry(@"INSERT INTO VoucherDetailsEditHistory
                            SELECT        SerialNo, VoucherNo, SLNO, VoucherRowDescription, AccountsHeadID, AccountsHeadName, VoucherDR, VoucherCR, Rate, InQty, OutQty, InPcs, OutPcs, DeliveredPcs, DeliveredStatus, projectName, EntryDate, SubprojectName, 
                            ISApproved FROM            VoucherDetails WHERE        VoucherNo ='" + voucherNo + "'");
                                
                SQLQuery.ExecNonQry(@"Update VoucherMasterEditHistory SET VoucherEntryBy='" + User.Identity.Name + "',  VoucherEntryTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', Voucherpost='"+ btnSave.Text + "' WHERE VID=(Select MAX(VID) FROM VoucherMasterEditHistory)");
                SQLQuery.ExecNonQry(@"Update VoucherDetailsEditHistory SET SLNO=(Select MAX(VID) FROM VoucherMasterEditHistory),  EntryDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' WHERE SerialNo IN (Select SerialNo FROM VoucherDetails WHERE VoucherNo='" + voucherNo + "' )");

                InvIDNo();

                btnSave.Text = "Save Voucher";
                txtEditVoucherNo.Text = "";
                BindItemGrid();
                GridView1.DataBind();
                txtTTL.Text = "0";

                if (chkPrint.Checked)
                {
                    string url = "./Voucher.aspx?Inv=" + voucherNo;
                    ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");
                    Response.Write("<script>window.open(" + url + ", '_blank' );</script>");
                    //Response.Redirect = "Purchase.aspx";
                }

                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Voucher Update Successfully.";
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

    private void saveData(string voucherNo)
    {
        string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        SqlCommand cmd2x = new SqlCommand("INSERT INTO VoucherMaster (VoucherNo, VoucherDate, VoucherDescription, ParticularID, VoucherEntryBy, VoucherAmount)" +
                                            "VALUES (@VoucherNo, @VoucherDate, @VoucherDescription, @ParticularID, @VoucherEntryBy, @VoucherAmount)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        string lName = Page.User.Identity.Name.ToString();
        cmd2x.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = voucherNo;
        cmd2x.Parameters.Add("@VoucherDate", SqlDbType.DateTime).Value = dt;
        cmd2x.Parameters.Add("@VoucherDescription", SqlDbType.VarChar).Value = ddParticular.SelectedItem.Text;
        cmd2x.Parameters.Add("@ParticularID", SqlDbType.NVarChar).Value = ddParticular.SelectedValue; //SQLQuery.ReturnString("Select Particularsid FROM Particulars WHERE Particularsname='" + ddParticular.SelectedItem.Text + "'");
        cmd2x.Parameters.Add("@VoucherEntryBy", SqlDbType.VarChar).Value = lName;
        cmd2x.Parameters.Add("@VoucherAmount", SqlDbType.Decimal).Value = txtTTL.Text;

        cmd2x.Connection.Open();
        cmd2x.ExecuteNonQuery();
        cmd2x.Connection.Close();

        string query = "";
        if (txtEditVoucherNo.Text == "")
        {
            query = " AND EntryBy='" + lName + "'";
        }

        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT SerialNo, Particular, VoucherRowDescription, AccountsHeadDr, 
                AccountsHeadDrName, Head5Dr, Name5Dr, AccountsHeadCr, AccountsHeadCrName, Head5Cr, Name5Cr, Amount, projectName, EntryDate, ISApproved
                FROM VoucherTmp WHERE VoucherNo='" + txtEditVoucherNo.Text + "' " + query);

        foreach (DataRow drx in dtx.Rows)
        {
            string description = drx["VoucherRowDescription"].ToString();
            string acHeadDr = drx["AccountsHeadDr"].ToString();
            string Head5Dr = drx["Head5Dr"].ToString();
            string Name5Dr = drx["Name5Dr"].ToString();
            string acHeadCr = drx["AccountsHeadCr"].ToString();
            string Head5Cr = drx["Head5Cr"].ToString();
            string Name5Cr = drx["Name5Cr"].ToString();
            decimal amount = Convert.ToDecimal(drx["Amount"].ToString());

            VoucherEntry.InsertVoucherDetails(voucherNo, description, acHeadDr, amount, 0, dt);
            VoucherEntry.InsertVoucherDetails(voucherNo, description, acHeadCr, 0, amount, dt);

            AutoTransaction(voucherNo, acHeadDr, Head5Dr, Name5Dr, amount.ToString(), "Dr", description + " Voucher# " + voucherNo);
            AutoTransaction(voucherNo, acHeadCr, Head5Cr, Name5Cr, amount.ToString(), "Cr", description + " Voucher# " + voucherNo);
        }

        SQLQuery.ExecNonQry("Update VoucherTmp Set VoucherNo='" + voucherNo + "'  WHERE VoucherNo='" + txtEditVoucherNo.Text + "' " + query);

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
        PopulateSubAcc();
    }
    protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView2.SelectedIndex);
            Label Label2 = GridView2.Rows[index].FindControl("lblSl") as Label;
            lblSl.Text = Label2.Text;

            SqlCommand cmd7 = new SqlCommand(@"SELECT [VoucherRowDescription],  AccountsHeadDr, AccountsHeadCr, Amount, Particular, Head5Dr, Head5Cr, VoucherRowDescription
                                        FROM [VoucherTmp] WHERE SerialNo ='" + lblSl.Text + "'",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();

            if (dr.Read())
            {
                btnAdd.Text = "Update";
                txtDescription.Text = dr[0].ToString();
                ddParticular.SelectedValue = Convert.ToString(dr[4].ToString());
                PopulateSubAcc();

                //try
                //{
                ddControlDr.SelectedValue = SQLQuery.ReturnString("Select ControlAccountsID from HeadSetup where AccountsHeadID='" + dr[1].ToString() + "'");
                LoadDr();
                ddAccHeadDr.SelectedValue = dr[1].ToString();
                Load5thHead(ddHead5Dr, ddAccHeadDr);
                if (dr[5].ToString() != "")
                {
                    ddHead5Dr.SelectedValue = dr[5].ToString();
                }

                ddControlCr.SelectedValue = SQLQuery.ReturnString("Select ControlAccountsID from HeadSetup where AccountsHeadID='" + dr[2].ToString() + "'");
                LoadCr();

                ddAccHeadCr.SelectedValue = dr[2].ToString();
                Load5thHead(ddHead5Cr, ddAccHeadCr);
                if (dr[6].ToString() != "")
                {
                    ddHead5Cr.SelectedValue = dr[6].ToString();
                }
                //}
                //catch
                //{
                //    //SQLQuery.PopulateMultiDropDown(ddAccHeadDr, "");
                //    //SQLQuery.PopulateMultiDropDown(ddAccHeadCr, "");
                //    ddAccHeadDr.SelectedValue = dr[1].ToString();
                //    Load5thHead(ddHead5Dr, ddAccHeadDr);
                //    ddAccHeadCr.SelectedValue = dr[2].ToString();
                //    Load5thHead(ddHead5Cr, ddAccHeadCr);
                //}

                txtDescription.Text = Convert.ToString(dr[7].ToString());
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

    private void PopulateSubAcc()
    {
        try
        {
            SQLQuery.PopulateDropDown("Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount  Order by ControlAccountsID", ddControlDr, "ControlAccountsID", "name");
            SQLQuery.PopulateDropDown("Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount  Order by ControlAccountsID", ddControlCr, "ControlAccountsID", "name");
            //}

            PopulateHeads();
        }
        catch (Exception ex)
        {
            lblMsg.CssClass = "xerp_error";
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "ERROR: " + ex.ToString();
        }

    }

    private void PopulateHeads()
    {
        try
        {
            SQLQuery.PopulateDropDown("Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID='" + ddControlDr.SelectedValue + "' ", ddAccHeadDr, "AccountsHeadID", "AccountsHeadName");
            SQLQuery.PopulateDropDown("Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID='" + ddControlCr.SelectedValue + "' ", ddAccHeadCr, "AccountsHeadID", "AccountsHeadName");

     
            Load5thHead(ddHead5Dr, ddAccHeadDr);
            Load5thHead(ddHead5Cr, ddAccHeadCr);
            //txtDescription.Text = SQLQuery.ReturnString("Select Detail FROM Particulars where Particularsid='" + ddParticular.SelectedValue + "'");

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
        PopulateSubAcc();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtDate.Text = DateTime.Now.ToShortDateString();
        InvIDNo();
        ddParticular.DataBind();
        PopulateSubAcc();

        SQLQuery.ExecNonQry("Delete VoucherTmp WHERE VoucherNo='' AND EntryBy ='" + Page.User.Identity.Name.ToString() + "'");
    }

    protected void lbRefresh_OnClick(object sender, EventArgs e)
    {
        PopulateSubAcc();
        Notify("Data has been refreshed", "info", lblMsg);
    }

    protected void txtDate_OnTextChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    protected void ddAccHeadDr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Load5thHead(ddHead5Dr, ddAccHeadDr);
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "GroupDropDownList()", true);
    }

    protected void ddAccHeadCr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Load5thHead(ddHead5Cr, ddAccHeadCr);
        GenerateNarration();
    }
    private void Load5thHead(DropDownList ddHead5, DropDownList ddAccHead)
    {
        ddHead5.Visible = true;

        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT ID, Code FROM BankLoan WHERE ACHeadId='" + ddAccHead.SelectedValue + "'");//Short Term & Long Term Loan
        if (dt.Rows.Count > 0)
        {
            SQLQuery.PopulateDropDown("SELECT ID, Code FROM BankLoan WHERE ACHeadId='" + ddAccHead.SelectedValue + "' AND Status='0'", ddHead5, "ID", "Code");
        }
        else if (ddAccHead.SelectedValue == "010101002") // || ddAccHead.SelectedValue == "040301001" || ddAccHead.SelectedValue == "040302001")//Bank
        {
            SQLQuery.PopulateDropDown("Select ACID, (Select BankName from Banks where BankId=BankAccounts.BankID)+'- '+ACNo as bank from BankAccounts ", ddHead5, "ACID", "bank");

        }
        else if (ddAccHead.SelectedValue == "010104001" || ddAccHead.SelectedValue == "030101001")//Customer
        {
            SQLQuery.PopulateDropDown("Select PartyID, Company from Party where Type='customer' order by Company ", ddHead5, "PartyID", "Company");

        }
        else if (ddAccHead.SelectedValue == "020102037")//Suppliers
        {
            //temporary disabled
            SQLQuery.PopulateDropDown("Select PartyID, Company from Party where Type='vendor' order by Company ", ddHead5, "PartyID", "Company");

        } 
        else
        {
            ddHead5.Items.Clear();
            ddHead5.Visible = false;
            GenerateNarration();
        }
    }

    private void GenerateNarration()
    {
        //Naration builder

        string particular = SQLQuery.ReturnString("Select Detail FROM Particulars where Particularsid='" + ddParticular.SelectedValue + "'");
        if (ddParticular.SelectedValue == "4")//Collection
        {
            txtDescription.Text = "Received from " + ddAccHeadCr.SelectedItem.Text.Trim();
        }
        else if (ddParticular.SelectedValue == "9") //Payment
        {
            txtDescription.Text = "Paid to " + ddAccHeadDr.SelectedItem.Text.Trim() + " for ...";
        }
        else if (ddAccHeadDr.SelectedValue != "" && ddAccHeadCr.SelectedValue != "") // for journal
        {
            string devider = " for ";
            if (ddAccHeadDr.SelectedItem.Text.IndexOf(" for ") != -1)
            {
                devider = " to ";
            }
            string narrationbuilder = devider + ddAccHeadCr.SelectedItem.Text.Trim();
            if (ddAccHeadDr.SelectedValue != "" && ddAccHeadCr.SelectedValue != "")
            {
                txtDescription.Text = ddAccHeadDr.SelectedItem.Text.Trim() + narrationbuilder;
            }
        }
    }


    private void AutoTransaction(string invoiceNo, string headId, string head5, string Name5, string amt, string side, string desc)
    {
        string type = "";
        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT ID, Code FROM BankLoan WHERE ACHeadId='" + headId + "'");//Short Term & Long Term Loan
        if (dt.Rows.Count > 0)
        {
            type = "Loan";
        }
        else if (headId == "010101002") //|| headId == "040301001" || headId == "040302001")//Bank
        {
            type = "Bank";
        }
        else if (headId == "010104001" || headId == "030101001")//Customer
        {
            type = "Customer";
        }
        else if (headId == "020102037")//Suppliers
        {
            type = "Supplier";
        }

        string dr = "0", cr = amt;
        if (side == "Dr")
        {
            cr = "0"; dr = amt;
        }

        if (type != "")
        {
            VoucherEntry.TransactionEntry(invoiceNo, txtDate.Text, head5, Name5, desc, dr, cr, "0", "Voucher", type, headId, Page.User.Identity.Name, "1");
        }

    }
    private void BindItemGrid()
    {
        if (btnSave.Text == "Update")
        {
            SqlCommand cmd = new SqlCommand(@"SELECT [SerialNo], [VoucherRowDescription], AccountsHeadDr, AccountsHeadDrName+ '<br/>'+ Name5Dr AS AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName+ '<br/>'+ Name5Cr AS AccountsHeadCrName, Amount 
                                            FROM[VoucherTmp] WHERE (VoucherNo ='" + txtEditVoucherNo.Text + "') ORDER BY [SerialNo]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            GridView2.EmptyDataText = "No data added ...";
            GridView2.DataSource = cmd.ExecuteReader();
            GridView2.DataBind();
            cmd.Connection.Close();

            txtTTL.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp where VoucherNo='" + txtEditVoucherNo.Text + "'");
        }
        else
        {
            SqlCommand cmd = new SqlCommand(@"SELECT [SerialNo], [VoucherRowDescription], AccountsHeadDr, AccountsHeadDrName+ '<br/>'+ Name5Dr AS AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName+ '<br/>'+ Name5Cr AS AccountsHeadCrName, Amount 
                                            FROM[VoucherTmp] WHERE (VoucherNo ='" + txtEditVoucherNo.Text + "') AND ([EntryBy] ='" + Page.User.Identity.Name + "') ORDER BY [SerialNo]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            GridView2.EmptyDataText = "No data added ...";
            GridView2.DataSource = cmd.ExecuteReader();
            GridView2.DataBind();
            cmd.Connection.Close();

            txtTTL.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp where VoucherNo='" + txtEditVoucherNo.Text + "'  AND EntryBy='" + Page.User.Identity.Name.ToString() + "'");
        }
    }

    private void BindItemGrid4Edit()
    {
        SqlCommand cmd = new SqlCommand(@"SELECT [SerialNo], [VoucherRowDescription], AccountsHeadDr, AccountsHeadDrName+ '<br/>'+ Name5Dr AS AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName+ '<br/>'+ Name5Cr AS AccountsHeadCrName, Amount 
                                            FROM[VoucherTmp] WHERE VoucherNo ='" + txtEditVoucherNo.Text + "' ORDER BY [SerialNo]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        GridView2.EmptyDataText = "No data added ...";
        GridView2.DataSource = cmd.ExecuteReader();
        GridView2.DataBind();
        cmd.Connection.Close();

        txtTTL.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp WHERE VoucherNo='" + txtEditVoucherNo.Text + "'");
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Restrict User
            string permLevel = SQLQuery.ReturnString("Select UserLevel from Logins where LoginUserName='" + User.Identity.Name + "'");
            if (Convert.ToInt32(permLevel) > 1) //Permitted Only for super Admin
            {
                Notify("You have not authorized for <b>EDITING</b> the voucher!", "error", lblMsg);
            }
            else
            {
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label lblID = GridView1.Rows[index].FindControl("Label1") as Label;

                LoadEditMode(lblID.Text);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }
    private void LoadEditMode(string voucherNo)
    {

        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) VID, VoucherNo, VoucherDate, VoucherDescription, 
                ParticularID, VoucherEntryBy, VoucherEntryDate, Voucherpost, VoucherPostby, Voucherpostdate, VoucherReferenceNo, VoucherAmount 
                FROM VoucherMaster WHERE (VoucherNo = '" + voucherNo + "')");

        foreach (DataRow drx in dtx.Rows)
        {
            txtEditVoucherNo.Text = drx["VoucherNo"].ToString();
            txtDate.Text=Convert.ToDateTime(drx["VoucherDate"].ToString()).ToString("dd/MM/yyyy");
            GridView1.DataBind();
            ddParticular.SelectedValue = drx["ParticularID"].ToString();
            PopulateSubAcc();
            txtTTL.Text = drx["VoucherAmount"].ToString();
        }

        BindItemGrid4Edit();
        btnSave.Text = "Update";
        Notify("Voucher# "+ voucherNo + " is in edit mode!", "warn", lblMsg);
    }
    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            //Restrict User
            string permLevel = SQLQuery.ReturnString("Select UserLevel from Logins where LoginUserName='" + User.Identity.Name + "'");
            if (Convert.ToInt32(permLevel) > 1) //Permitted Only for super Admin
            {
                Notify("You are not authorized for <b>DELETING</b> the voucher!", "error", lblMsg);
            }
            else
            {
                int index = Convert.ToInt32(e.RowIndex);
                Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;
                string lName = Page.User.Identity.Name.ToString();
                string dateNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string voucherNo = lblItemCode.Text;
                SQLQuery.ExecNonQry("Update VoucherMaster Set Voucherpost='C', VoucherPostby= '" + lName + "', Voucherpostdate='" + dateNow + "' where VoucherNo='" + voucherNo + "'");
                SQLQuery.ExecNonQry("Update VoucherDetails Set ISApproved='C' where VoucherNo='" + voucherNo + "'");
                SQLQuery.ExecNonQry("Delete Transactions where InvNo='" + voucherNo + "'");

                GridView1.DataBind();
                Notify("The voucher " + voucherNo + " has been cancelled successfully!", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    protected void ddControlDr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDr();
    }

    private void LoadDr()
    {
        SQLQuery.PopulateDropDown("Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID='" + ddControlDr.SelectedValue + "' ", ddAccHeadDr, "AccountsHeadID", "AccountsHeadName");
        Load5thHead(ddHead5Dr, ddAccHeadDr);
    }
    protected void ddControlCr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCr();
    }
    private void LoadCr()
    {
        SQLQuery.PopulateDropDown("Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID='" + ddControlCr.SelectedValue + "' ", ddAccHeadCr, "AccountsHeadID", "AccountsHeadName");
        Load5thHead(ddHead5Cr, ddAccHeadCr);
    }
}
