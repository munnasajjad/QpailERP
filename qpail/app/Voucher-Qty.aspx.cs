using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accounting;
using RunQuery;

public partial class app_Voucher_Qty : System.Web.UI.Page
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
            SQLQuery.Empty2Zero(txtRate);
            txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQty.Text));
            //string isValid = ddAccHeadDr.SelectedItem.Text.Substring(0, 3);
            //if (isValid.Trim() != "__")
            //{
            //    isValid = ddAccHeadCr.SelectedItem.Text.Substring(0, 3);
            //}
            //else
            //{
            //    ddAccHeadDr.Focus();
            //    lblMsg.Attributes.Add("class", "xerp_warning");
            //    lblMsg.Text = "ERROR: Please select accounts head!";
            //    return;
            //}

            //if (isValid.Trim() != "__")
            //{
            if (Convert.ToDecimal(txtQty.Text) > 0 && txtQty.Text != "")
            {
                if (btnAdd.Text == "Add to Grid")
                {
                    InsertData();
                    //BindItemGrid();
                }
                else
                {
                    ExecuteUpdate();
                    btnAdd.Text = "Add to Grid";
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Entry updated successfully";
                    //BindItemGrid4Edit();
                }
                BindItemGrid();
                txtDescription.Text = "";
                txtAmount.Text = "";
                txtRate.Text = "";
                txtQty.Text = "";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "ERROR: Head Already exist!";
            }

            //}
            //else
            //{
            //    lblMsg.Attributes.Add("class", "xerp_warning");
            //    lblMsg.Text = "ERROR: Please select accounts head!";
            //    ddAccHeadCr.Focus();
            //}
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
        SqlCommand cmd2 = new SqlCommand(@"INSERT INTO VoucherTmp (VoucherNo, Particular, VoucherRowDescription, AccountsHeadDr, AccountsHeadDrName, Head5Dr, Name5Dr, AccountsHeadCr, AccountsHeadCrName, Head5Cr, Name5Cr, Amount, EntryDate, EntryBy, Rate, Qty)
                                VALUES ('', @Particular, @VoucherRowDescription, @AccountsHeadDr, @AccountsHeadDrName, '" + ddHead5Dr.SelectedValue + "', '" + h5NameDr + "', @AccountsHeadCr, @AccountsHeadCrName, '" + ddHead5Cr.SelectedValue + "', '" + h5NameCr + "', @Amount, @EntryDate, @EntryBy, '" + txtRate.Text + "','" + txtQty.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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
        SqlCommand cmd2 = new SqlCommand("UPDATE VoucherTmp SET Particular='" + ddParticular.SelectedValue + "', VoucherRowDescription='" + txtDescription.Text + "', AccountsHeadDr='" + ddAccHeadDr.SelectedValue + "'," +
                                "AccountsHeadDrName='" + ddAccHeadDr.SelectedItem.Text + "', Head5Dr='" + ddHead5Dr.SelectedValue + "', Name5Dr='" + h5NameDr + "', AccountsHeadCr='" + ddAccHeadCr.SelectedValue + "',AccountsHeadCrName='" + ddAccHeadCr.SelectedItem.Text + "', Head5Cr='" + ddHead5Cr.SelectedValue + "', Name5Cr='" + h5NameCr + "', Amount=@Amount, Rate='" + txtRate.Text + "', Qty='" + txtQty.Text + "' where (SerialNo ='" + lblSl.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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
            string lName = Page.User.Identity.Name.ToString();
            string query = "";
            if (txtEditVoucherNo.Text == "")
            {
                query = " AND EntryBy='" + lName + "'";
            }

            decimal ttl = Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(qty),0) FROM VoucherTmp  WHERE VoucherNo='" + txtEditVoucherNo.Text + "' " + query));
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
                InvIDNo();


                btnSave.Text = "Save Voucher";
                txtEditVoucherNo.Text = "";
                BindItemGrid();
                GridView1.DataBind();
                txtTTL.Text = "0";
                txtTQty.Text = "0";

                if (chkPrint.Checked)
                {
                    string url = "./Voucher.aspx?Inv=" + voucherNo;
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
        int success = cmd2x.ExecuteNonQuery();
        cmd2x.Connection.Close();

        string query = "";
        if (txtEditVoucherNo.Text == "")
        {
            query = " AND EntryBy='" + lName + "'";
        }

        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT SerialNo, Particular, VoucherRowDescription, AccountsHeadDr, Rate, Qty,
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
            string qty = drx["qty"].ToString();
            string rate = drx["Rate"].ToString();

            VoucherEntry.InsertVoucherDetailsWithQty(voucherNo, description, acHeadDr, amount, 0, dt, qty, "0", rate);
            VoucherEntry.InsertVoucherDetailsWithQty(voucherNo, description, acHeadCr, 0, amount, dt, "0", qty, rate);

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

            SqlCommand cmd7 = new SqlCommand(@"SELECT [VoucherRowDescription],  AccountsHeadDr, AccountsHeadCr, Amount, Particular, Head5Dr, Head5Cr, VoucherRowDescription, qty, rate
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
                ddControlDr.SelectedValue = dr[1].ToString().Substring(0, 6);
                LoadDr();
                ddAccHeadDr.SelectedValue = dr[1].ToString();
                Load5thHead(ddHead5Dr, ddAccHeadDr);
                if (dr[5].ToString() != "")
                {
                    ddHead5Dr.SelectedValue = dr[5].ToString();
                }

                ddControlCr.SelectedValue = dr[2].ToString().Substring(0, 6);
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
                txtQty.Text = Convert.ToString(dr[8].ToString());
                txtRate.Text = Convert.ToString(dr[9].ToString());

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
        {/*
            string q = @"SELECT        ControlAccountsID,
                             (SELECT        AccountsName
                               FROM            Accounts
                               WHERE        (AccountsID = ControlAccount.AccountsID)) + ' > ' + ControlAccountsName AS name
                                FROM            ControlAccount
                                WHERE        (AccountsID NOT IN
                                                             (SELECT        AccountsID
                                                               FROM            Accounts AS Accounts_1
                                                               WHERE        (GroupID <> '02') AND (GroupID <> '04')))
                                ORDER BY ControlAccountsID";

            if (ddParticular.SelectedItem.Text == "Receipt")
            {
                SQLQuery.PopulateDropDown("Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount where (ControlAccountsID='010101'  OR  ControlAccountsID='010102') Order by ControlAccountsID", ddControlDr, "ControlAccountsID", "name");
                SQLQuery.PopulateDropDown(q, ddControlCr, "ControlAccountsID", "name");

            }
            else if (ddParticular.SelectedItem.Text == "Payment")
            {
                q = @"SELECT        ControlAccountsID,
                             (SELECT        AccountsName
                               FROM            Accounts
                               WHERE        (AccountsID = ControlAccount.AccountsID)) + ' > ' + ControlAccountsName AS name
                                FROM            ControlAccount
                                WHERE        (AccountsID NOT IN
                                                             (SELECT        AccountsID
                                                               FROM            Accounts AS Accounts_1
                                                               WHERE        (GroupID <> '01') AND (GroupID <> '03')))
                                ORDER BY ControlAccountsID";
                SQLQuery.PopulateDropDown(q, ddControlDr, "ControlAccountsID", "name");
                SQLQuery.PopulateDropDown("Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount Where (ControlAccountsID='010101'  OR  ControlAccountsID='010102') Order by ControlAccountsID", ddControlCr, "ControlAccountsID", "name");
            }
            else // Journal
            {*/
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

            /*
        ddAccHeadDr.Items.Clear();
        ddAccHeadCr.Items.Clear();

        if (ddParticular.SelectedItem.Text == "Salary")
        {
            SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND ControlAccountsID='040114' ");
            SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND AccountsID='0101' ");
        }
        else if (ddParticular.SelectedItem.Text == "Receipt")
        {
            SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND (ControlAccountsID='010101'  OR  ControlAccountsID='010102')  ");
            SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND GroupID<>'04' AND GroupID<>'02' AND  ControlAccountsID<>'010101' AND  ControlAccountsID<>'010102'  ");
        }
        else if (ddParticular.SelectedItem.Text == "Purchase")
        {
            SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND GroupID='04' "); //Expense
            SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND AccountsID='0201' ");//Suppliers
        }
        else if (ddParticular.SelectedItem.Text == "Sales")
        {
            SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND ControlAccountsID='010105' "); //Customer
            SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND GroupID='03' "); //Income
        }
        else if (ddParticular.SelectedItem.Text == "Payment")
        {
            SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND GroupID<>'01'  AND GroupID<>'03' ");//exclude assets & income
            SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND (ControlAccountsID='010101'  OR  ControlAccountsID='010102') "); // only cash & bank
        }
        else // Journal
        {
            SQLQuery.PopulateMultiDropDown(ddAccHeadDr, "");
            SQLQuery.PopulateMultiDropDown(ddAccHeadCr, "");
        }
  */
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

        if (ddAccHead.SelectedValue == "010101002")// || ddAccHead.SelectedValue == "040301001" || ddAccHead.SelectedValue == "040302001")//Bank
        {
            SQLQuery.PopulateDropDown("Select ACID, (Select BankName from Banks where BankId=BankAccounts.BankID)+'- '+ACNo as bank from BankAccounts ", ddHead5, "ACID", "bank");

        }
        else if (ddAccHead.SelectedValue == "010104001" || ddAccHead.SelectedValue == "030101001")//Customer
        {
            SQLQuery.PopulateDropDown("Select PartyID, Company from Party where Type='customer' order by Company ", ddHead5, "PartyID", "Company");

        }
        else if (ddAccHead.SelectedValue == "020102006")//Suppliers
        {
            //temporary disabled
            //SQLQuery.PopulateDropDown("Select PartyID, Company from Party where Type='vendor' order by Company ", ddHead5, "PartyID", "Company");

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
        if (headId == "010101002")// || headId == "040301001" || headId == "040302001")//Bank
        {
            type = "Bank";
        }
        else if (headId == "010104001" || headId == "030101001")//Customer
        {
            type = "Customer";
        }
        else if (headId == "020102006")//Suppliers
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
    {/*
        SqlCommand cmd = new SqlCommand(@"SELECT [SerialNo], [VoucherRowDescription], AccountsHeadDr, AccountsHeadDrName+ '<br/>'+ Name5Dr AS AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName+ '<br/>'+ Name5Cr AS AccountsHeadCrName, Amount , qty, rate
                                            FROM[VoucherTmp] WHERE ([EntryBy] ='" + Page.User.Identity.Name + "') AND VoucherNo ='" + txtEditVoucherNo.Text + "' ORDER BY [SerialNo]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        GridView2.EmptyDataText = "No data added ...";
        GridView2.DataSource = cmd.ExecuteReader();
        GridView2.DataBind();
        cmd.Connection.Close();
        
        txtTTL.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp where VoucherNo='" + txtEditVoucherNo.Text + "' AND EntryBy='" + Page.User.Identity.Name.ToString() + "'");
        */

        if (btnSave.Text == "Update")
        {
            SqlCommand cmd = new SqlCommand(@"SELECT [SerialNo], [VoucherRowDescription], AccountsHeadDr, AccountsHeadDrName+ '<br/>'+ Name5Dr AS AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName+ '<br/>'+ Name5Cr AS AccountsHeadCrName, Amount , qty, rate
                                            FROM[VoucherTmp] WHERE (VoucherNo ='" + txtEditVoucherNo.Text + "') ORDER BY [SerialNo]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            GridView2.EmptyDataText = "No data added ...";
            GridView2.DataSource = cmd.ExecuteReader();
            GridView2.DataBind();
            cmd.Connection.Close();
            txtTQty.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Qty),0) FROM VoucherTmp where VoucherNo='" + txtEditVoucherNo.Text + "'");
            txtTTL.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp where VoucherNo='" + txtEditVoucherNo.Text + "'");
        }
        else
        {
            SqlCommand cmd = new SqlCommand(@"SELECT [SerialNo], [VoucherRowDescription], AccountsHeadDr, AccountsHeadDrName+ '<br/>'+ Name5Dr AS AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName+ '<br/>'+ Name5Cr AS AccountsHeadCrName, Amount , qty, rate
                                            FROM[VoucherTmp] WHERE (VoucherNo ='" + txtEditVoucherNo.Text + "') AND ([EntryBy] ='" + Page.User.Identity.Name + "') ORDER BY [SerialNo]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            GridView2.EmptyDataText = "No data added ...";
            GridView2.DataSource = cmd.ExecuteReader();
            GridView2.DataBind();
            cmd.Connection.Close();
            txtTQty.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Qty),0) FROM VoucherTmp where VoucherNo='" + txtEditVoucherNo.Text + "' AND EntryBy='" + Page.User.Identity.Name.ToString() + "'");
            txtTTL.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp where VoucherNo='" + txtEditVoucherNo.Text + "'  AND EntryBy='" + Page.User.Identity.Name.ToString() + "'");
        }
    }

    private void BindItemGrid4Edit()
    {
        SqlCommand cmd = new SqlCommand(@"SELECT [SerialNo], [VoucherRowDescription], AccountsHeadDr, AccountsHeadDrName+ '<br/>'+ Name5Dr AS AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName+ '<br/>'+ Name5Cr AS AccountsHeadCrName, Amount  , qty, rate
                                            FROM[VoucherTmp] WHERE VoucherNo ='" + txtEditVoucherNo.Text + "' ORDER BY [SerialNo]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        GridView2.EmptyDataText = "No data added ...";
        GridView2.DataSource = cmd.ExecuteReader();
        GridView2.DataBind();
        cmd.Connection.Close();
        txtTQty.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Qty),0) FROM VoucherTmp where VoucherNo='" + txtEditVoucherNo.Text + "'");
        txtTTL.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp where VoucherNo='" + txtEditVoucherNo.Text + "'");
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Restrict User
            string permLevel = SQLQuery.ReturnString("Select UserLevel from Logins where LoginUserName='" + User.Identity.Name + "'");
            if (Convert.ToInt32(permLevel) > 1) //Permitted Only for super Admin
            {
                Notify("You are not authorized for <b>EDITING</b> the voucher!", "error", lblMsg);
            }
            else
            {
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label lblID = GridView1.Rows[index].FindControl("Label1") as Label;

                DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) VID, VoucherNo, VoucherDate, VoucherDescription, 
                ParticularID, VoucherEntryBy, VoucherEntryDate, Voucherpost, VoucherPostby, Voucherpostdate, VoucherReferenceNo, VoucherAmount 
                FROM VoucherMaster WHERE (VoucherNo = '" + lblID.Text + "')");

                foreach (DataRow drx in dtx.Rows)
                {
                    txtEditVoucherNo.Text = drx["VoucherNo"].ToString();
                    ddParticular.SelectedValue = drx["ParticularID"].ToString();
                    PopulateSubAcc();
                    txtTTL.Text = drx["VoucherAmount"].ToString();
                    //txtTQty.Text= SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp where VoucherNo='" + txtEditVoucherNo.Text + "'");
                }

                BindItemGrid4Edit();
                btnSave.Text = "Update";
                Notify("Voucher Entry is in edit mode!", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
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

                string voucherNo = lblItemCode.Text;
                SQLQuery.ExecNonQry("Update VoucherMaster Set Voucherpost='C' where VoucherNo='" + voucherNo + "'");
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
