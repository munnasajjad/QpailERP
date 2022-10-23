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
using Stock;

public partial class app_LCVoucher : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        btnAdd.Attributes.Add("onclick",
            " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToShortDateString();
            InvIDNo();
            ddLCNo.DataBind();
            ddParticular.DataBind();
            PopulateSubAcc();
            lblProject.Text = "1";
            lblUser.Text = Page.User.Identity.Name.ToString();
            BindItemGrid();
            ddPurpose.DataBind();
            ddGroup.DataBind();
            
            string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" +
                            ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue +
                     "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue +
                     "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");

            //ddSuppCategory.DataBind();
            //ddVendor.DataBind();
            GetProductList();
            BindItemGrid();
            //txtFor.Focus();

            //cash-chq payment
            //ddMode.DataBind();
            //LoadPayMode();

            string formtype = Request.QueryString["type"];
            if (formtype == "edit")
            {
                //ddInvoice.DataBind();
                //pnlReturnHistory.Visible = true;
                GridView2.DataBind();
                string invId = Request.QueryString["inv"];
                if (invId != null)
                {
                    //ddInvoice.SelectedValue = invId;
                }
                //lblInvoice.Text = ddInvoice.SelectedValue;
                //LoadEditMode(ddInvoice.SelectedValue);
                //GetTotalAmount();

                //divinvoice.Attributes.Remove("Class");
                //divinvoice.Attributes.Add("Class", "control-group");
                //Page.Title = "Purchase Edit";
                //ltrHead.Text = "Purchase Edit";
            }
        }

        GridView1.DataBind();
    }


    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    private void GetProductList()
    {
        if (ddCategory.SelectedValue != "")
        {
            string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategory.SelectedValue +
                            "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemName, "ProductID", "ItemName");

            ltrUnitType.Text = SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddItemName.SelectedValue + "'");
            LoadSpecList("filter");

            if (IsPostBack)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
            }

            CheckItemType(Convert.ToInt32(ddGroup.SelectedValue));
            //recentInfo();
        }
    }

    private void CheckItemType(int iGrp)
    {
        if (ddSubGrp.SelectedValue == "10")
        {
            LoadSpecList("filter");
            pnlSpec.Visible = true;
        }
        else
        {
            pnlSpec.Visible = false;
        }


        if (iGrp <= 3)
        {
            ddStockType.SelectedValue = "Raw";
            ltrWarrenty.Text = "Qnty./Pack (Kg): ";
            ltrSerial.Text = "No. of Packs: ";
            PanelMachine.Visible = true;

            string subGrp = "";
            if (ddSubGrp.SelectedValue != "")
            {
                subGrp = ddSubGrp.SelectedItem.Text;
            }

            if (subGrp == "Tin Plate")
            {
                pkSizeField.Attributes.Remove("class");
                pkSizeField.Attributes.Add("class", "control-group");
            }
            else
            {
                pkSizeField.Attributes.Remove("class");
                pkSizeField.Attributes.Add("class", "control-group hidden");
                //SectionField.Attributes.Remove("class");
                //SectionField.Attributes.Add("class", "control-group hidden");
            }

        }
        else
        {
            //PanelWarrenty.Visible = false;
            ddStockType.SelectedValue = "Fixed";
            ltrWarrenty.Text = "Warrentry : ";
            ltrSerial.Text = "Serial No. : ";
        }

        if (ddGroup.SelectedValue == "5")
        {
            //SectionField.Attributes.Remove("class");
            //SectionField.Attributes.Add("class", "control-group");
        }
    }

    public string InvIDNo()
    {
        try
        {
            SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (max(VID),0)+1001 )) from VoucherMaster",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            string InvNo = Convert.ToString(cmd.ExecuteScalar());
            string year = Convert.ToString(Convert.ToDateTime(txtDate.Text).Year);
                // "2011"; // DateTime.Now.Year.ToString();
            InvNo = "V-" + year + "-" + InvNo;
            txtVID.Text = InvNo;
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            return InvNo;
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Invalid Date";
            SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (max(VID),0)+1001 )) from VoucherMaster",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
        SqlCommand cmd2 =
            new SqlCommand(
                @"INSERT INTO VoucherTmp (VoucherNo, Particular, VoucherRowDescription, AccountsHeadDr, AccountsHeadDrName, Head5Dr, Name5Dr, AccountsHeadCr, AccountsHeadCrName, Head5Cr, Name5Cr, Amount, EntryDate, EntryBy)
                                VALUES ('', @Particular, @VoucherRowDescription, @AccountsHeadDr, @AccountsHeadDrName, '" +
                ddHead5Dr.SelectedValue + "', '" + h5NameDr + "', @AccountsHeadCr, @AccountsHeadCrName, '" +
                ddHead5Cr.SelectedValue + "', '" + h5NameCr + "', @Amount, @EntryDate, @EntryBy)",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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
        SqlCommand cmd2 =
            new SqlCommand("UPDATE VoucherTmp SET Particular='" + ddParticular.SelectedValue + "', VoucherRowDescription='" +
                txtDescription.Text + "', AccountsHeadDr='" + ddAccHeadDr.SelectedValue + "'," +
                "AccountsHeadDrName='" + ddAccHeadDr.SelectedItem.Text + "', Head5Dr='" + ddHead5Dr.SelectedValue +
                "', Name5Dr='" + h5NameDr + "', AccountsHeadCr='" + ddAccHeadCr.SelectedValue + "',AccountsHeadCrName='" +
                ddAccHeadCr.SelectedItem.Text + "', Head5Cr='" + ddHead5Cr.SelectedValue + "', Name5Cr='" + h5NameCr +
                "', Amount=@Amount  where (SerialNo ='" + lblSl.Text + "')",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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

    private void Status()
    {
        if (cbStatus.Checked)
        {
            ddGodown.Visible = true;
        }
        else
        {
            ddGodown.Visible = false;
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

            decimal ttl =
                Convert.ToDecimal(
                    SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp  WHERE VoucherNo='" +
                                          txtEditVoucherNo.Text + "' " + query));
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

                saveVoucher(voucherNo);
                InvIDNo();

                if (lbLCNo.Text == "Old")
                {
                    InsertLCNo();
                    ddLCNo.DataBind();
                    lbLCNo.Text = "New";
                    txtLCNo.Visible = false;
                    ddLCNo.Visible = true;
                    BindItemGrid();
                    GridView1.DataBind();
                }
                else
                {
                    if (cbStatus.Checked)
                    {
                        SQLQuery.ExecNonQry("Update LC set isActive='D' WHERE LCNO='" + ddLCNo.SelectedValue + "'");
                    }

                }

                Session["EditLCNo"] = "";

                btnSave.Text = "Save Voucher";
                txtEditVoucherNo.Text = "";
                BindItemGrid();

                ddLCNo.Visible = true;
                txtLCNo.Visible = false;
                lbLCNo.Text = "New";
                ddLCNo.Focus();

                BindItemGridPrd();
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
                lblMsg.Text = "New Voucher Saved Successfully.";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: No data was found into current voucher!";
            }
            saveLC();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
        finally
        {
            GridView1.DataBind();
            GridView1.Visible = true;

        }
    }

    private void saveVoucher(string voucherNo)
    {
        string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        SqlCommand cmd2x =
            new SqlCommand(
                "INSERT INTO VoucherMaster (VoucherNo, VoucherDate, VoucherDescription, ParticularID, VoucherReferenceNo, VoucherEntryBy, VoucherAmount)" +
                "VALUES (@VoucherNo, @VoucherDate, @VoucherDescription, @ParticularID, @VoucherReferenceNo, @VoucherEntryBy, @VoucherAmount)",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        string lName = Page.User.Identity.Name.ToString();
        cmd2x.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = voucherNo;
        cmd2x.Parameters.Add("@VoucherDate", SqlDbType.DateTime).Value = dt;
        cmd2x.Parameters.Add("@VoucherDescription", SqlDbType.VarChar).Value = ddParticular.SelectedItem.Text;
        cmd2x.Parameters.Add("@ParticularID", SqlDbType.NVarChar).Value = ddParticular.SelectedValue;
        //SQLQuery.ReturnString("Select Particularsid FROM Particulars WHERE Particularsname='" + ddParticular.SelectedItem.Text + "'");

        string refNo = ddLCNo.SelectedValue;
        if (lbLCNo.Text == "Old")
        {
            refNo = txtLCNo.Text;
        }
        //cmd2x.Parameters.Add("@VoucherReferenceNo", SqlDbType.NVarChar).Value = ddLCNo.SelectedValue;
        cmd2x.Parameters.Add("@VoucherReferenceNo", SqlDbType.NVarChar).Value = refNo;
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

            AutoTransaction(voucherNo, acHeadDr, Head5Dr, Name5Dr, amount.ToString(), "Dr",
                description + " Voucher# " + voucherNo);
            AutoTransaction(voucherNo, acHeadCr, Head5Cr, Name5Cr, amount.ToString(), "Cr",
                description + " Voucher# " + voucherNo);
        }

        SQLQuery.ExecNonQry("Update VoucherTmp Set VoucherNo='" + voucherNo + "'  WHERE VoucherNo='" +
                            txtEditVoucherNo.Text + "' " + query);

    }

    private void saveLC()
    {
        string refNo = ddLCNo.SelectedValue;
        if (lbLCNo.Text == "Old")
        {
            refNo = txtLCNo.Text;
        }

        if (cbStatus.Checked)
        {
            //Inactivate LC
            SQLQuery.ExecNonQry("UPDATE LC Set IsActive='D' WHERE LCNO='" + ddLCNo.SelectedValue + "'");

            Notify("LC Closed successfully...", "success", lblMsg);
            string isExist = SQLQuery.ReturnString("Select InvoiceID from Stock where InvoiceID='" + refNo + "'");
            if (isExist == "")
            {
                string vAmt = SQLQuery.ReturnString("Select ISNULL(SUM(VoucherAmount),0) FROM VoucherMaster WHERE VoucherReferenceNo='" + refNo + "'");
                string iTtl = SQLQuery.ReturnString("Select ISNULL(SUM(UnitPrice),0) FROM LcItems WHERE LCNo='" + ddLCNo.SelectedValue + "'");

                //insert stock qty       
                DataTable dtx1 =SQLQuery.ReturnDataTable(@"SELECT EntryID, LCNo, Purpose, GradeId, CategoryId, ItemCode, HSCode, ItemSizeID, pcs, NoOfPacks, QntyPerPack, Spec, Thickness, Measurement, qty, 
                                                                UnitPrice, CFRValue, EntryBy, ReturnQty, Loading, Loaded, LandingPercent, LandingAmt, TotalUSD, TotalBDT, UnitCostBDT, FullDescription
                                                                FROM [LcItems] WHERE LCNo='" + refNo + "'");
                decimal pcs = 0;
                foreach (DataRow drx in dtx1.Rows)
                {
                    string EntryID = drx["EntryID"].ToString();
                    string LCNo = drx["LCNo"].ToString();
                    string Purpose = drx["Purpose"].ToString();
                    string GradeId = drx["GradeId"].ToString();
                    string CategoryId = drx["CategoryId"].ToString();
                    string ItemCode = drx["ItemCode"].ToString();
                    string HSCode = drx["HSCode"].ToString();
                    string ItemSizeID = drx["ItemSizeID"].ToString();
                    string noPcs = drx["pcs"].ToString();
                    string NoOfPacks = drx["NoOfPacks"].ToString();
                    string QntyPerPack = drx["QntyPerPack"].ToString();
                    string Spec = drx["Spec"].ToString();
                    string Thickness = drx["Thickness"].ToString();
                    string Measurement = drx["Measurement"].ToString();
                    string qty = drx["qty"].ToString();
                    string UnitPrice = drx["UnitPrice"].ToString();
                    string CFRValue = drx["CFRValue"].ToString();
                    string EntryBy = drx["EntryBy"].ToString();
                    string ReturnQty = drx["ReturnQty"].ToString();
                    string Loading = drx["Loading"].ToString();
                    string Loaded = drx["Loaded"].ToString();
                    string LandingPercent = drx["LandingPercent"].ToString();
                    string LandingAmt = drx["LandingAmt"].ToString();
                    string TotalUSD = drx["TotalUSD"].ToString();
                    string TotalBDT = drx["TotalBDT"].ToString();
                    string UnitCostBDT = drx["UnitCostBDT"].ToString();
                    decimal price = Convert.ToDecimal(TotalBDT)/Convert.ToDecimal(qty);
                    string fullDescription = drx["FullDescription"].ToString();

                    string detail = fullDescription + " - " + NoOfPacks + " - " + QntyPerPack;
                    string itemGroup = Stock.Inventory.GetItemGroup(ItemCode);
                    if (Convert.ToDecimal(itemGroup) > 3 && Convert.ToDecimal(itemGroup) != 7)
                        //Machines, Electric, stationaries, others except wastage.
                    {
                        detail = "Model# : " + Measurement + ". " + ". Warranty: " + QntyPerPack + ", Serial # " +
                                 Thickness + ", Specification " + NoOfPacks + ", " + fullDescription;
                        pcs = Convert.ToInt32(Convert.ToDecimal(qty));

                    }
                    

                    if (SQLQuery.ReturnString("Select UnitType from Products where ProductID='" + ItemCode + "'") =="PCS")
                    {
                        pcs = Convert.ToInt32(Convert.ToDecimal(qty));
                        qty = "0";
                    }

                    //Stock.Inventory.SaveToStock(Purpose, invNo,
                    //    "Purchase from " + ddVendor.SelectedItem.Text, lblEntryId.Text, PackSize, "", "", "",
                    //    SizeRef, iCode, iName, StockType, ddGodown.SelectedValue, ddLocation.SelectedValue,
                    //    iGrp, pcs, 0, Convert.ToDecimal(price), qty, 0, detail, "Stock-in", "Purchase",
                    //    ddLocation.SelectedItem.Text, lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

                    decimal uPrice = Convert.ToDecimal(UnitPrice)*Convert.ToDecimal(vAmt)/Convert.ToDecimal(iTtl);

                    Inventory.SaveToStock(Purpose, refNo, "LC Import from " + txtManufacturer.Text, EntryID, ItemSizeID, "", "","",
                        Spec, ItemCode, Inventory.GetProductName(ItemCode), "", ddGodown.SelectedValue, "",
                        itemGroup, Convert.ToDecimal(pcs), 0, uPrice, Convert.ToDecimal(qty), 0,
                        detail, "", "LC", "", User.Identity.Name, DateTime.Now.ToString("yyyy-MM-dd"));

                    Notify("LC Closed, Items inserted into stock.", "success", lblMsg);
                    
                        GridView1.DataBind();
                        GridView1.Visible = true;
                    
                    
                }
                //ddName.DataBind();
                //LoadLCData();
                //ItemGrid.DataBind();
                //Refresh();
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
                Page page = (Page) context.Handler;
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

            SqlCommand cmd7 =
                new SqlCommand(@"SELECT [VoucherRowDescription],  AccountsHeadDr, AccountsHeadCr, Amount, Particular, Head5Dr, Head5Cr, VoucherRowDescription
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

                //ddLCNo.SelectedValue = Convert.ToString(dr[8].ToString());
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
/*
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
            SQLQuery.PopulateDropDown(
                "Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount  Order by ControlAccountsID",
                ddControlDr, "ControlAccountsID", "name");
            SQLQuery.PopulateDropDown(
                "Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount  Order by ControlAccountsID",
                ddControlCr, "ControlAccountsID", "name");
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
            SQLQuery.PopulateDropDown(
                "Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID='" +
                ddControlDr.SelectedValue + "' ", ddAccHeadDr, "AccountsHeadID", "AccountsHeadName");
            SQLQuery.PopulateDropDown(
                "Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID='" +
                ddControlCr.SelectedValue + "' ", ddAccHeadCr, "AccountsHeadID", "AccountsHeadName");

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

        SQLQuery.ExecNonQry("Delete VoucherTmp WHERE VoucherNo='' AND EntryBy ='" + Page.User.Identity.Name.ToString() +
                            "'");
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

        if (ddAccHead.SelectedValue == "010101002")
            // || ddAccHead.SelectedValue == "040301001" || ddAccHead.SelectedValue == "040302001")//Bank
        {
            SQLQuery.PopulateDropDown(
                "Select ACID, (Select BankName from Banks where BankId=BankAccounts.BankID)+'- '+ACNo as bank from BankAccounts ",
                ddHead5, "ACID", "bank");

        }
        else if (ddAccHead.SelectedValue == "010104001" || ddAccHead.SelectedValue == "030101001") //Customer
        {
            SQLQuery.PopulateDropDown("Select PartyID, Company from Party where Type='customer' order by Company ",
                ddHead5, "PartyID", "Company");

        }
        else if (ddAccHead.SelectedValue == "020102001") //Suppliers
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

        string particular =
            SQLQuery.ReturnString("Select Detail FROM Particulars where Particularsid='" + ddParticular.SelectedValue +
                                  "'");
        if (ddParticular.SelectedValue == "4") //Collection
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


    private void AutoTransaction(string invoiceNo, string headId, string head5, string Name5, string amt, string side,
        string desc)
    {
        string type = "";
        if (headId == "010101002") // || headId == "040301001" || headId == "040302001")//Bank
        {
            type = "Bank";
        }
        else if (headId == "010104001" || headId == "030101001") //Customer
        {
            type = "Customer";
        }
        else if (headId == "020102001") //Suppliers
        {
            type = "Supplier";
        }

        string dr = "0", cr = amt;
        if (side == "Dr")
        {
            cr = "0";
            dr = amt;
        }

        if (type != "")
        {
            VoucherEntry.TransactionEntry(invoiceNo, txtDate.Text, head5, Name5, desc, dr, cr, "0", "Voucher", type,
                headId, Page.User.Identity.Name, "1");
        }

    }

    private void BindItemGrid()
    {
        SqlCommand cmd =
            new SqlCommand(@"SELECT [SerialNo], [VoucherRowDescription], AccountsHeadDr, AccountsHeadDrName+ '<br/>'+ Name5Dr AS AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName+ '<br/>'+ Name5Cr AS AccountsHeadCrName, Amount 
                                            FROM[VoucherTmp] WHERE ([EntryBy] ='" + Page.User.Identity.Name +
                "') AND VoucherNo ='" + txtEditVoucherNo.Text + "' ORDER BY [SerialNo]",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        GridView2.EmptyDataText = "No data added ...";
        GridView2.DataSource = cmd.ExecuteReader();
        GridView2.DataBind();
        cmd.Connection.Close();

        txtTTL.Text =
            SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp where VoucherNo='" +
                                  txtEditVoucherNo.Text + "' AND EntryBy='" + Page.User.Identity.Name.ToString() + "'");
        BindItemGridPrd();
    }

    private void BindItemGrid4Edit()
    {
        SqlCommand cmd =
            new SqlCommand(
                @"SELECT [SerialNo], [VoucherRowDescription], AccountsHeadDr, AccountsHeadDrName+ '<br/>'+ Name5Dr AS AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName+ '<br/>'+ Name5Cr AS AccountsHeadCrName, Amount 
                                            FROM[VoucherTmp] WHERE VoucherNo ='" + txtEditVoucherNo.Text +
                "' ORDER BY [SerialNo]",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        GridView2.EmptyDataText = "No data added ...";
        GridView2.DataSource = cmd.ExecuteReader();
        GridView2.DataBind();
        cmd.Connection.Close();

        txtTTL.Text =
            SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp where VoucherNo='" +
                                  txtEditVoucherNo.Text + "'");
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Restrict User
            string permLevel =
                SQLQuery.ReturnString("Select UserLevel from Logins where LoginUserName='" + User.Identity.Name + "'");
            if (Convert.ToInt32(permLevel) > 1) //Permitted Only for super Admin
            {
                Notify("You are not authorized for <b>EDITING</b> the voucher!", "error", lblMsg);
            }
            else
            {
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label lblID = GridView1.Rows[index].FindControl("Label1") as Label;

                DataTable dtx =SQLQuery.ReturnDataTable(@"SELECT TOP (1) VID, VoucherNo, LCNo, VoucherDate, VoucherDescription, 
                ParticularID, VoucherEntryBy, VoucherEntryDate, Voucherpost, VoucherPostby, Voucherpostdate, 
                VoucherReferenceNo, VoucherAmount FROM VoucherMaster WHERE (VoucherNo = '" + lblID.Text + "')");

                foreach (DataRow drx in dtx.Rows)
                {
                    txtEditVoucherNo.Text = drx["VoucherNo"].ToString();
                    ddParticular.SelectedValue = drx["ParticularID"].ToString();
                    PopulateSubAcc();
                    txtTTL.Text = drx["VoucherAmount"].ToString();
                    Session["EditLCNo"] = drx["VoucherReferenceNo"].ToString();
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
            string permLevel =
                SQLQuery.ReturnString("Select UserLevel from Logins where LoginUserName='" + User.Identity.Name + "'");
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
        SQLQuery.PopulateDropDown("Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID='" +
            ddControlDr.SelectedValue + "' ", ddAccHeadDr, "AccountsHeadID", "AccountsHeadName");
        Load5thHead(ddHead5Dr, ddAccHeadDr);
    }

    protected void ddControlCr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCr();
    }

    private void LoadCr()
    {
        SQLQuery.PopulateDropDown("Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID='" +
            ddControlCr.SelectedValue + "' ", ddAccHeadCr, "AccountsHeadID", "AccountsHeadName");
        Load5thHead(ddHead5Cr, ddAccHeadCr);
    }

    //protected void ItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        int index = Convert.ToInt32(e.RowIndex);
    //        Label lblItemCode = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;
    //        TextBox txtQty = ItemGrid.Rows[index].FindControl("txtQty") as TextBox;

    //        SqlCommand cmd7 = new SqlCommand("DELETE LcItems WHERE EntryID=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
    //        cmd7.Connection.Open();
    //        cmd7.ExecuteNonQuery();
    //        cmd7.Connection.Close();

    //        BindItemGrid();
    //        Button1.Text = "Add to grid";
    //        lblMsg2.Attributes.Add("class", "xerp_warning");
    //        lblMsg2.Text = "Item deleted from order ...";
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg2.Attributes.Add("class", "xerp_error");
    //        lblMsg2.Text = "ERROR: " + ex.Message.ToString();
    //    }

    //}
    protected void ItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label rIndex = ItemGrid.Rows[index].FindControl("Label1") as Label;

            SQLQuery.ExecNonQry("DELETE LcItems WHERE EntryID=" + rIndex.Text);

            //Ammd history
            Label Label2 = ItemGrid.Rows[index].FindControl("Label2") as Label;
            Label QTY9 = ItemGrid.Rows[index].FindControl("QTY9") as Label;
            Label UnitPrice = ItemGrid.Rows[index].FindControl("UnitPrice") as Label;
            Label CFRValue = ItemGrid.Rows[index].FindControl("CFRValue") as Label;
            //Save_LC_Log("Item deleted: ", Label2.Text, "Qnty.: " + QTY9.Text + ", Unit Price: " + UnitPrice.Text + ", CFR: " + CFRValue.Text);
            Button1.Text = "Add to grid";
            lblMsg2.Attributes.Add("class", "xerp_warn");
            lblMsg2.Text = "LC Item Deleted ...";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }

        BindItemGrid();
        ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
    }

    //protected void ItemGrid_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int index = Convert.ToInt32(ItemGrid.SelectedIndex);
    //        Label lblItemName = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;
    //        lblOrderID.Text = lblItemName.Text;
    //        EditMode(lblItemName.Text);
    //        Button1.Text = "Update";

    //        Notify("Edit mode activated ...", "info", lblMsg2);
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg2.Attributes.Add("class", "xerp_error");
    //        lblMsg2.Text = "ERROR: " + ex.Message.ToString();
    //        Notify(ex.Message.ToString(), "error", lblMsg);
    //        lblMsg.Text = "Error: " + ex.ToString();
    //    }
    //}


    //private void GridItemEditMode(string entryID)
    //{
    //    try
    //    {
    //        //int index = Convert.ToInt32(ItemGrid.SelectedIndex);
    //        //Label lblItemName = ItemGrid.Rows[index].FindControl("Label1") as Label;
    //        //lblEntryId.Text = lblItemName.Text;

    //        SqlCommand cmd7 = new SqlCommand("Select ItemCode, HSCode, ItemSizeID, Thickness, Measurement, qty, UnitPrice, CFRValue, pcs FROM [LcItems] WHERE EntryID=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
    //        cmd7.Parameters.Add("@sl", SqlDbType.VarChar).Value = entryID;
    //        cmd7.Connection.Open();
    //        SqlDataReader dr = cmd7.ExecuteReader();

    //        if (dr.Read())
    //        {
    //            //btnSave.Text = "Update";
    //            string iCode = dr[0].ToString();
    //            txtHSCode.Text = dr[1].ToString();

    //            txtThickness.Text = dr[3].ToString();
    //            txtMeasure.Text = dr[4].ToString();
    //            txtQty.Text = dr[5].ToString();
    //            txtPrice.Text = dr[6].ToString();
    //            txtCFR.Text = dr[7].ToString();
    //            txtWeight.Text = dr[8].ToString();

    //            //ddGroup.SelectedValue=SQLQuery.ReturnString("Select GroupID from ItemSubGroup where CategoryID=(Select CategoryID from ItemGrade where GradeID=(Select ))")

    //            string catID = SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + iCode + "'");
    //            string grdID = SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
    //            string subID = SQLQuery.ReturnString("SELECT CategoryID FROM ItemGrade WHERE GradeID ='" + grdID + "'");
    //            string grpID = SQLQuery.ReturnString("SELECT GroupID FROM ItemSubGroup WHERE CategoryID ='" + subID + "'");

    //            //ItemGrid.DataBind();
    //            //pnl.Update();
    //            btnAdd.Text = "Update";
    //        }

    //        cmd7.Connection.Close();

    //        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) Purpose, GradeId, CategoryId, ItemCode, HSCode, 
    //                            ItemSizeID, Spec, Thickness, Measurement, qty, UnitPrice, CFRValue, ReturnQty, NoOfPacks, QntyPerPack, 
    //                            Loading, Loaded, LandingPercent, LandingAmt, TotalUSD, TotalBDT, UnitCostBDT  FROM   [LcItems] WHERE EntryID='" + lblEntryId.Text + "'");

    //        foreach (DataRow drx in dtx.Rows)
    //        {
    //            ddPurpose.SelectedValue = drx["Purpose"].ToString();
    //            string iCode = drx["ItemCode"].ToString();

    //            string grpID = Stock.Inventory.GetItemGroup(iCode);
    //            ddGroup.SelectedValue = grpID;
    //            string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + grpID +
    //                            "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
    //            SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");
    //            ddSubGrp.SelectedValue = Stock.Inventory.GetItemSubGroup(iCode);

    //            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue +
    //                     "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
    //            SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
    //            ddGrade.SelectedValue = drx["GradeId"].ToString();

    //            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue +
    //                     "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
    //            SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
    //            ddCategory.SelectedValue = drx["CategoryId"].ToString();

    //            GetProductList();

    //            ddItemName.SelectedValue = iCode;
    //            txtQty.Text = drx["qty"].ToString();
    //            string spec = drx["Spec"].ToString();

    //            string size = drx["ItemSizeID"].ToString();
    //            if (size != "0")
    //            {
    //                ddSize.SelectedValue = size;
    //            }

    //            //ddSpec.SelectedValue = drx["Spec"].ToString();

    //            txtThickness.Text = drx["Thickness"].ToString();
    //            txtMeasure.Text = drx["Measurement"].ToString();
    //            txtSerial.Text = drx["NoOfPacks"].ToString();
    //            txtWarrenty.Text = drx["QntyPerPack"].ToString();

    //            txtQty.Text = drx["qty"].ToString();
    //            txtPrice.Text = drx["UnitPrice"].ToString();
    //            txtCFR.Text = drx["CFRValue"].ToString();

    //            LoadItemsPanel();

    //            //Load color spec

    //            //Load color spec
    //            if (ddSubGrp.SelectedValue == "10") //Printing Ink
    //            {
    //                LoadSpecList("");
    //                pnlSpec.Visible = true;

    //                string isExist = SQLQuery.ReturnString("Select spec from Specifications where id='" + spec + "'");
    //                if (isExist == "")
    //                {
    //                    ddSpec.Visible = false;
    //                    txtSpec.Visible = true;
    //                    lbSpec.Text = "Cancel";
    //                    txtSpec.Text = spec;
    //                }
    //                else
    //                {
    //                    ddSpec.Visible = true;
    //                    txtSpec.Visible = false;
    //                    lbSpec.Text = "New";
    //                    ddSpec.SelectedValue = spec;
    //                }
    //            }

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Notify(ex.Message.ToString(), "error", lblMsg);
    //    }
    //}
    //private void EditMode(string entryID)
    //{
    //    //SqlCommand cmd =new SqlCommand("SELECT InvNo, Itemgroup, ItemCode, ItemName, Qty, Price, Total, Warrenty, SerialNo, UnitType, SizeRef, Manufacturer, CountryOfOrigin, PackSize, EntryBy, pcs, Purpose, ModelNo, Specification FROM [LcItems] WHERE Id='" + entryID + "'",
    //    SqlCommand cmd = new SqlCommand("SELECT  LCNo, Purpose, GradeId, CategoryId, ItemCode, HSCode, ItemSizeID, pcs, NoOfPacks, QntyPerPack, Spec, Thickness, Measurement, FullDescription, qty, UnitPrice, CFRValue, ReturnQty, Loading, Loaded, LandingPercent, LandingAmt, TotalUSD, TotalBDT, UnitCostBDT FROM[LcItems] WHERE EntryID = '" + entryID + "'",
    //        new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
    //    cmd.Connection.Open();
    //    SqlDataReader dr = cmd.ExecuteReader();
    //    if (dr.Read())
    //    {
    //        ddSize.SelectedValue = dr[0].ToString();

    //        string iGrp = dr[1].ToString();
    //        ddGroup.SelectedValue = iGrp;

    //        CheckItemType(Convert.ToInt32(iGrp));

    //        string iCode = dr[2].ToString();
    //        //txtRate.Text = dr[3].ToString();
    //        txtQuantity.Text = dr[4].ToString();
    //        txtRate.Text = dr[5].ToString();
    //        //txtAmt.Text = dr[6].ToString();

    //        txtWarrenty.Text = dr[7].ToString();
    //        txtSerial.Text = dr[8].ToString();
    //        ltrUnitType.Text = dr[9].ToString();
    //        string spec = dr[10].ToString();


    //        txtManufacturer.Text = dr[11].ToString();
    //        txtCountry.Text = dr[12].ToString();
    //        string size = dr[13].ToString();
    //        if (size != "")
    //        {
    //            ddSize.SelectedValue = size;
    //        }

    //        txtWeight.Text = dr[15].ToString();

    //        string purpose = dr[16].ToString();
    //        if (purpose != "")
    //        {
    //            ddPurpose.SelectedValue = purpose;
    //        }

    //        txtModel.Text = dr[17].ToString();
    //        txtSpecification.Text = dr[18].ToString();

    //        string catID =RunQuery.SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + iCode + "'");
    //        string grdID =RunQuery.SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
    //        string subID =RunQuery.SQLQuery.ReturnString("SELECT CategoryID FROM ItemGrade WHERE GradeID ='" + grdID + "'");
    //        string grpID =RunQuery.SQLQuery.ReturnString("SELECT GroupID FROM ItemSubGroup WHERE CategoryID ='" + subID + "'");


    //        ddGroup.SelectedValue = grpID;

    //        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + grpID +
    //                        "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
    //        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");
    //        ddSubGrp.SelectedValue = subID;

    //        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + subID + "' AND ProjectID='" +
    //                 lblProject.Text + "' ORDER BY [GradeName]";
    //        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
    //        ddGrade.SelectedValue = grdID;

    //        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + grdID + "' AND ProjectID='" +
    //                 lblProject.Text + "' ORDER BY [CategoryName]";
    //        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
    //        ddCategory.SelectedValue = catID;

    //        gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + catID + "' AND ProjectID='" +
    //                 lblProject.Text + "' ORDER BY [ItemName]";
    //        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemName, "ProductID", "ItemName");
    //        ddItemName.SelectedValue = iCode;

    //        //Load color spec
    //        if (ddSubGrp.SelectedValue == "10") //Printing Ink
    //        {
    //            LoadSpecList("");
    //            pnlSpec.Visible = true;

    //            string isExist = SQLQuery.ReturnString("Select spec from Specifications where id='" + spec + "'");
    //            if (isExist == "")
    //            {
    //                ddSpec.Visible = false;
    //                txtSpec.Visible = true;
    //                lbSpec.Text = "Cancel";
    //                txtSpec.Text = spec;
    //            }
    //            else
    //            {
    //                ddSpec.Visible = true;
    //                txtSpec.Visible = false;
    //                lbSpec.Text = "New";
    //                ddSpec.SelectedValue = spec;
    //            }
    //        }

    //        //CheckItemType(Convert.ToInt32(iGrp));

    //    }
    //    cmd.Connection.Close();

    //    cmd = new SqlCommand("SELECT SubTotal, ItemDisc, ItemVAT FROM [LCVoucherProducts] WHERE Id='" + entryID + "'",
    //        new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
    //    cmd.Connection.Open();
    //    dr = cmd.ExecuteReader();
    //    if (dr.Read())
    //    {
    //        //txtSubTotal.Text = dr[0].ToString();
    //        //txtItemDisc.Text = dr[1].ToString();
    //        //txtItemVAT.Text = dr[2].ToString();
    //    }
    //    cmd.Connection.Close();
    //}
    private void InsertLCNo()
    {
        //txtLCNo.Text = LcNo.Trim();
        //string formtype = base.Request.QueryString["type"];
        //int projectID = Convert.ToInt32(lblProject.Text);
        string lName = Page.User.Identity.Name.ToString();

        SQLQuery.ExecNonQry("INSERT INTO LC (LCNo,  EntryBy)" +
                                   "VALUES ('"+ txtLCNo.Text + "', '" + lName + "')");

        SQLQuery.ExecNonQry(@"UPDATE  LcItems SET LCNo='" + txtLCNo.Text + "' WHERE LCNo='' AND EntryBy='" + lName + "'");

    }
    

    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //string ttlQty = RunQuery.SQLQuery.ReturnString("Select SUM(Qty) from LCVoucherProducts where InvNo='" + lblInvoice.Text + "' AND EntryBy='" + Page.User.Identity.Name.ToString() + "'");

        //GetTotalAmount();
    }

    protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGrp();
        CheckItemType(Convert.ToInt32(ddGroup.SelectedValue));
        
    }
    private void bindGrp()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
        GetProductList();
    }

    protected void ddSubGrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
        GetProductList();
    }
    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
        GetProductList();
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetProductList();
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSpecList("filter");
        txtQuantity.Focus();
        //recentInfo();
    }
    protected void lbSpec_OnClick(object sender, EventArgs e)
    {
        if (lbSpec.Text == "New")
        {
            ddSpec.Visible = false;
            txtSpec.Visible = true;
            lbSpec.Text = "Cancel";
            txtSpec.Focus();
        }
        else
        {
            ddSpec.Visible = true;
            txtSpec.Visible = false;
            lbSpec.Text = "New";
            LoadSpecList("filter");
            ddSpec.Focus();
        }
        lbFilter.Text = "Show-all";
    }
    protected void lbFilter_OnClick(object sender, EventArgs e)
    {
        if (lbFilter.Text == "Show-all")
        {
            LoadSpecList("");
            ////lbFilter.Text = "Filter"
        }
        else
        {
            LoadSpecList("filter");
            ////lbFilter.Text = "Show-all";
        }
        lbSpec.Text = "New";
        ddSpec.Visible = true;
        txtSpec.Visible = false;
    }
    private void LoadSpecList(string filterDD)
    {
        string gQuery = "SELECT [id], [spec] FROM [Specifications] ORDER BY [spec]";
        lbFilter.Text = "Filter";
        if (filterDD != "")
        {
            lbFilter.Text = "Show-all";
            gQuery = "SELECT [id], [spec] FROM [Specifications] where  CAST(id AS nvarchar) in (Select distinct spec from stock where ProductID='" + ddItemName.SelectedValue + "') ORDER BY [spec]";
        }

        SQLQuery.PopulateDropDown(gQuery, ddSpec, "id", "spec");

        //QtyinStock();
    }

    private void GenerateHSCode()
    {
        //Get HS Code from item Grade
        txtHSCode.Text =
            SQLQuery.ReturnString("Select TOP(1)  HSCode from LcItems where ItemCode='" +
                                           ddItemName.SelectedValue + "' AND HSCode<>'' Order by EntryID desc ");
        if (txtHSCode.Text == "")
        {
            txtHSCode.Text =
                SQLQuery.ReturnString(
                    "SELECT  TOP(1) HSCode from LcItems where ItemCode IN (SELECT ProductID FROM Products WHERE CategoryID IN (SELECT CategoryID FROM Categories WHERE GradeID ='" +
                    ddGrade.SelectedValue + "')) AND HSCode<>'' Order by EntryID desc ");
        }
    }
    protected void ddSpec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //QtyinStock();
    }
    
    protected void btnAddItem_Click(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            SQLQuery.Empty2Zero(txtWeight);
            string size = "", measure = "", thick = "";
            string subGrp = ddSubGrp.SelectedItem.Text;
            string desc = "";

            if (subGrp == "Tin Plate")
            {
                size = ddSize.SelectedValue;
                //thick = txtThickness.Text;
                //measure = txtMeasure.Text;

            }
            else if (ddGroup.SelectedValue == "1")//Raw Materials
            {

            }
            else if (ddGroup.SelectedValue == "4" || ddGroup.SelectedValue == "5")//Machineries & Electrical
            {
                size = ddSize.SelectedValue;
                //thick = txtThickness.Text;
                //measure = txtMeasure.Text;
                ///desc = "Country of Origin: " + ddCountry.SelectedItem.Text + ", Manufacturer: " + ddManufacturer.SelectedItem.Text;
            }
            else
            {
                //thick = txtThickness.Text;
                //measure = txtMeasure.Text;
            }
            string spec = "";
            if (ddSubGrp.SelectedValue == "10")//Printing Ink
            {
                spec = ddSpec.SelectedValue;
                if (txtSpec.Text != "" && lbSpec.Text == "Cancel" && ddSubGrp.SelectedValue == "10")//Insert Ink spec
                {
                    string isExist = RunQuery.SQLQuery.ReturnString("SELECT id FROM Specifications WHERE  spec ='" + txtSpec.Text + "'");
                    if (isExist == "")
                    {
                        SQLQuery.ExecNonQry("INSERT INTO Specifications (spec, EntryBy) VALUES ('" + txtSpec.Text + "', '" + lName + "') ");
                        spec = SQLQuery.ReturnString("SELECT MAX(id) FROM Specifications");
                        LoadSpecList(""); //ddSpec.DataBind();
                        ddSpec.SelectedValue = spec;
                    }
                    else
                    {
                        LoadSpecList("");
                        ddSpec.SelectedValue = isExist;
                    }
                }
                spec = ddSpec.SelectedValue;
            }
            //Amendment  History
            string oldItem = "", oldQty = "", oldPrice = "", oldCfr = "";
            string formtype = Request.QueryString["type"];
            if (formtype == "edit")
            {
                DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) Purpose, GradeId, CategoryId, ItemCode, HSCode, 
                                ItemSizeID, Spec, Thickness, Measurement, qty, UnitPrice, CFRValue, ReturnQty, NoOfPacks, QntyPerPack, 
                                Loading, Loaded, LandingPercent, LandingAmt, TotalUSD, TotalBDT, UnitCostBDT, Manufacturer, CountryOfOrigin  FROM   [LcItems] WHERE EntryID='" + lblInvoice + "'");

                foreach (DataRow drx in dtx.Rows)
                {
                    ddPurpose.SelectedValue = drx["Purpose"].ToString();
                    oldItem = Stock.Inventory.GetProductName(drx["ItemCode"].ToString());

                    oldQty = drx["qty"].ToString();
                    oldPrice = drx["UnitPrice"].ToString();
                    oldCfr = drx["CFRValue"].ToString();
                }
            }

            if (Button1.Text == "Update")
            {

                SQLQuery.ExecNonQry(@"UPDATE [dbo].[LcItems]   SET [GradeId] = '" + ddGrade.SelectedValue + "', [CategoryId] = '" + ddCategory.SelectedValue +
                                    "',[ItemCode] = '" + ddItemName.SelectedValue + "',[HSCode] = '" +
                                    txtHSCode.Text + "',[ItemSizeID] ='" + ddSize.SelectedValue + "',pcs='" +
                                    txtWeight.Text + "',[Spec] ='" + ddSpec.SelectedValue + "', Thickness='" + txtSerial.Text +
                                    "', NoOfPacks='" + txtSpecification.Text + "', Measurement='" + txtModel.Text+"', QntyPerPack='" + txtWarrenty.Text + "', [qty] = '" + txtQuantity.Text +
                                    "', FullDescription='" + desc + "', [UnitPrice] ='" + txtRate.Text + "', [Manufacturer] ='" + txtManufacturer.Text + "', [CountryOfOrigin] ='" + txtCountry.Text + "' WHERE EntryID='" + lblInvoice.Text + "'");

                //ItemGrid.DataBind();
                BindItemGrid();
                ItemGrid.SelectedIndex = (-1);
                ClearDetailForm();
                Button1.Text = "Add to grid";

                string item = ddItemName.SelectedItem.Text;
                //Save_LC_Log("Item updated", oldItem, item);
                //Save_LC_Log_decimal("Qnty.: " + item, oldQty, txtQty.Text);
                //Save_LC_Log_decimal("Unit Price: " + item, oldPrice, txtPrice.Text);
                //Save_LC_Log_decimal("CFR: " + item, oldCfr, txtCFR.Text);
            }
            else
            {
                SQLQuery.Empty2Zero(txtQuantity);

                //SqlCommand cmde = new SqlCommand("SELECT ItemCode FROM LcItems WHERE ItemCode ='" + ddItemName.SelectedValue + "' AND  LCNo ='' AND EntryBy ='" + lblLogin.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //cmde.Connection.Open();
                //string isExist = Convert.ToString(cmde.ExecuteScalar());
                //cmde.Connection.Close();

                if (ddItemName.SelectedValue != "")
                {
                    string lcNo = ddLCNo.SelectedValue;
                    if (lbLCNo.Text=="Old")
                    {
                        lcNo = "";
                    }

                    SqlCommand cmd2 = new SqlCommand("INSERT INTO LcItems (LCNo, Purpose, GradeId, CategoryId, ItemCode, HSCode, Thickness, Measurement, NoOfPacks, QntyPerPack,  qty, UnitPrice, Loading, LandingPercent, pcs, EntryBy, FullDescription, Manufacturer, CountryOfOrigin)" +
                                                                 " VALUES (@LCNo, '" + ddPurpose.SelectedValue + "', '" + ddGrade.SelectedValue + "', '" + ddCategory.SelectedValue + "', @ItemCode, @HSCode, '" + txtSpecification.Text+"', '"+ txtModel.Text + "', '" + txtSerial.Text + "','" + txtWarrenty.Text +
                                                                                "', @qty, @UnitPrice,  @Loading,  @LandingPercent,  '" + txtWeight.Text + "',  @EntryBy, '" + desc + "','" + txtManufacturer.Text + "','" + txtCountry.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                    cmd2.Parameters.AddWithValue("@LCNo", lcNo);
                    cmd2.Parameters.AddWithValue("@ItemCode", ddItemName.SelectedValue);
                    cmd2.Parameters.AddWithValue("@HSCode", txtHSCode.Text);
                    cmd2.Parameters.AddWithValue("@NoOfPacks", txtSpecification.Text);
                    cmd2.Parameters.AddWithValue("@ItemSizeID", size);
                    cmd2.Parameters.AddWithValue("@Measurement", measure);

                    cmd2.Parameters.AddWithValue("@Thickness", txtSerial.Text);
                    cmd2.Parameters.AddWithValue("@QntyPerPack", txtWarrenty.Text);
                    cmd2.Parameters.AddWithValue("@qty", Convert.ToDecimal(txtQuantity.Text));
                    cmd2.Parameters.AddWithValue("@UnitPrice", Convert.ToDecimal(txtRate.Text));
                    //cmd2.Parameters.AddWithValue("@CFRValue", txtCFR.Text);

                    //Loading, Loaded, LandingPercent, LandingAmt
                    cmd2.Parameters.AddWithValue("@Loading", "1");
                    //cmd2.Parameters.AddWithValue("@Loaded", txtCFR.Text);
                    cmd2.Parameters.AddWithValue("@LandingPercent", "1.01");
                    //cmd2.Parameters.AddWithValue("@LandingAmt", Convert.ToDecimal(txtCFR.Text) * 1.01M);
                    cmd2.Parameters.AddWithValue("@EntryBy", lName);

                    cmd2.Connection.Open();
                    cmd2.ExecuteNonQuery();
                    cmd2.Connection.Close();

                    string item = ddItemName.SelectedItem.Text;
                    //Save_LC_Log("New item inserted: ", item, "Qnty.: " + txtQty.Text + ", Unit Price: " + txtPrice.Text + ", CFR: " + txtCFR.Text);

                    //ItemGrid.DataBind();
                    
                    SQLQuery.ExecNonQry(@"UPDATE  LcItems SET LCNo='" + ddLCNo.SelectedValue + "' WHERE LCNo='' AND EntryBy='" + lName + "'");
                    
                    BindItemGrid();

                    txtQuantity.Text = "";
                    txtRate.Text = "";
                    //txtThickness.Text = "";
                    //txtMeasure.Text = "";
                    //txtCFR.Text = "";
                    ddGroup.Focus();
                }
                else
                {
                    Notify("ERROR: Invalid Product Name!", "warn", lblMsg2);
                }
            }

            //if (txtBExRate.Text != "" && txtCExRate.Text != "" && txtCfrUSD.Text != "")
            //{
            //    //txtCfrBDT.Text = Convert.ToString(Convert.ToDecimal(txtCExRate.Text) * Convert.ToDecimal(txtCfrUSD.Text));
            //    //txtBankBDT.Text = Convert.ToString(Convert.ToDecimal(txtBExRate.Text) * Convert.ToDecimal(txtCfrUSD.Text));
            //    if (txtMargin.Text != "")
            //    {
            //        //txtLTR.Text = Convert.ToString(Convert.ToDecimal(txtBankBDT.Text) - Convert.ToDecimal(txtMargin.Text));
            //    }
            //}

            txtSerial.Text = "";
            txtRate.Text = "";
            //txtCFR.Text = "";
            txtWarrenty.Text = "";
            txtQuantity.Text = "";
            ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg2);
        }
        finally
        {
            //ItemGrid.DataBind();
            BindItemGrid();
        }
    }

    //protected void btnAddItem_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string lName = Page.User.Identity.Name.ToString();
    //        if (txtQuantity.Text == "")
    //        {
    //            txtQuantity.Text = "1";
    //        }

    //        //SizeId, ProductID, BrandID
    //        SqlCommand cmde = new SqlCommand("SELECT ItemName FROM [LcItems] WHERE ItemCode ='" + ddItemName.SelectedValue + "' AND  LCNo ='" + lblInvoice.Text + "' AND EntryBy ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
    //        cmde.Connection.Open();
    //        string isExist = Convert.ToString(cmde.ExecuteScalar());
    //        cmde.Connection.Close();

    //        string productName = ddItemName.SelectedItem.Text;// +" -" + ddCategory.SelectedItem.Text + " " + ddSubGrp.SelectedItem.Text;

    //        if (Button1.Text == "Add to grid")
    //        {
    //            //if (isExist == "")
    //            //{
    //            string spec = "";
    //            if (ddSubGrp.SelectedValue == "10")//Printing Ink
    //            {
    //                spec = ddSpec.SelectedValue;
    //                if (txtSpec.Text != "" && lbSpec.Text == "Cancel" && ddSubGrp.SelectedValue == "10")//Insert Ink spec
    //                {
    //                    isExist = RunQuery.SQLQuery.ReturnString("SELECT id FROM Specifications WHERE  spec ='" + txtSpec.Text + "'");
    //                    if (isExist == "")
    //                    {
    //                        RunQuery.SQLQuery.ExecNonQry("INSERT INTO Specifications (spec, EntryBy) VALUES ('" + txtSpec.Text + "', '" + lName + "') ");
    //                        spec = RunQuery.SQLQuery.ReturnString("SELECT MAX(id) FROM Specifications");
    //                        LoadSpecList(""); //ddSpec.DataBind();
    //                        ddSpec.SelectedValue = spec;
    //                    }
    //                    else
    //                    {
    //                        LoadSpecList("");
    //                        ddSpec.SelectedValue = isExist;
    //                    }
    //                }
    //                spec = ddSpec.SelectedValue;
    //            }
    //            SQLQuery.Empty2Zero(txtWeight);
    //            string desc = "";
    //            //SqlCommand cmd2 = new SqlCommand("INSERT INTO LCVoucherProducts (InvNo, Purpose, Itemgroup, SubGroup, Grade, Category, ItemCode, ItemName, Qty, Price, Warrenty, SerialNo, ModelNo, Specification, UnitType, SizeRef, StockType, pcs, EntryBy) " +
    //            //                                 "VALUES (@InvNo, '" + ddPurpose.SelectedValue + "', @Itemgroup, '" + ddSubGrp.SelectedValue + "', '" + ddGrade.SelectedValue + "', '" + ddCategory.SelectedValue + "',   @ItemCode, @ItemName, @Qty, @Price, @Warrenty, @SerialNo, '" + txtModel.Text + "','" + txtSpecification.Text + "', @UnitType, @SizeRef, @StockType,'" + txtWeight.Text + "', @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

    //            //cmd2.Parameters.AddWithValue("@InvNo", "");
    //            //cmd2.Parameters.AddWithValue("@Itemgroup", ddGroup.SelectedValue);
    //            //cmd2.Parameters.AddWithValue("@ItemCode", ddItemName.SelectedValue);
    //            //cmd2.Parameters.AddWithValue("@ItemName", productName);
    //            //cmd2.Parameters.AddWithValue("@Qty", Convert.ToDecimal(txtQuantity.Text));
    //            //cmd2.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtRate.Text));

    //            ////decimal subTotal = Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQuantity.Text);
    //            ////SQLQuery.Empty2Zero(txtItemDisc); SQLQuery.Empty2Zero(txtItemVAT);
    //            ////cmd2.Parameters.AddWithValue("@SubTotal", subTotal);
    //            ////cmd2.Parameters.AddWithValue("@ItemDisc", Convert.ToDecimal(txtItemDisc.Text));
    //            ////cmd2.Parameters.AddWithValue("@ItemVAT", Convert.ToDecimal(txtItemVAT.Text));

    //            ////cmd2.Parameters.AddWithValue("@Total", subTotal - Convert.ToDecimal(txtItemDisc.Text) + Convert.ToDecimal(txtItemVAT.Text));
    //            //cmd2.Parameters.AddWithValue("@Warrenty", txtWarrenty.Text);
    //            //cmd2.Parameters.AddWithValue("@SerialNo", txtSerial.Text);
    //            //cmd2.Parameters.AddWithValue("@UnitType", ltrUnitType.Text);
    //            //cmd2.Parameters.AddWithValue("@SizeRef", spec);

    //            //string itemType = ddGroup.SelectedItem.Text;
    //            //if (ddSubGrp.SelectedValue == "9") //Tin Plate
    //            //{
    //            //    itemType = "Raw Sheet";
    //            //}
    //            //cmd2.Parameters.AddWithValue("@StockType", itemType);
    //            //cmd2.Parameters.AddWithValue("@EntryBy", lName);



    //            SqlCommand cmd2 = new SqlCommand("INSERT INTO LcItems (LCNo, Purpose, GradeId, CategoryId, ItemCode, NoOfPacks, QntyPerPack,  qty, UnitPrice, Loading, LandingPercent, pcs, EntryBy, FullDescription)" +
    //                                                             " VALUES (@LCNo, '" + ddPurpose.SelectedValue + "', '" + ddGrade.SelectedValue + "', '" + ddCategory.SelectedValue + "', @ItemCode, '" + txtSerial.Text + "','" + txtWarrenty.Text +
    //                                        "', @qty, @UnitPrice,  @Loading,  @LandingPercent,  '" + txtWeight.Text + "',  @EntryBy, '" + desc + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

    //            cmd2.Parameters.AddWithValue("@LCNo", "");
    //            cmd2.Parameters.AddWithValue("@ItemCode", ddItemName.SelectedValue);
    //            //cmd2.Parameters.AddWithValue("@HSCode", txtHSCode.Text);
    //            //cmd2.Parameters.AddWithValue("@Thickness", thick);
    //            //cmd2.Parameters.AddWithValue("@ItemSizeID", size);
    //            //cmd2.Parameters.AddWithValue("@Measurement", measure);

    //            cmd2.Parameters.AddWithValue("@NoOfPacks", txtSerial.Text);
    //            cmd2.Parameters.AddWithValue("@QntyPerPack", txtWarrenty.Text);
    //            cmd2.Parameters.AddWithValue("@qty", Convert.ToDecimal(txtQuantity.Text));
    //            cmd2.Parameters.AddWithValue("@UnitPrice", Convert.ToDecimal(txtRate.Text));
    //            //cmd2.Parameters.AddWithValue("@CFRValue", txtCFR.Text);

    //            //Loading, Loaded, LandingPercent, LandingAmt
    //            cmd2.Parameters.AddWithValue("@Loading", "1");
    //            //cmd2.Parameters.AddWithValue("@Loaded", txtCFR.Text);
    //            cmd2.Parameters.AddWithValue("@LandingPercent", "1.01");
    //            //cmd2.Parameters.AddWithValue("@LandingAmt", Convert.ToDecimal(txtCFR.Text) * 1.01M);
    //            cmd2.Parameters.AddWithValue("@EntryBy", lName);


    //            cmd2.Connection.Open();
    //            cmd2.ExecuteNonQuery();
    //            cmd2.Connection.Close();

    //            string stockLocation = "";
    //            if (ddGroup.SelectedValue == "5")
    //            {
    //                stockLocation = ddSection.SelectedValue;
    //            }

    //            SQLQuery.ExecNonQry("UPDATE LcItems SET Manufacturer='" + txtManufacturer.Text + "', CountryOfOrigin='" + txtCountry.Text + "', PackSize='" + ddSize.SelectedValue + "', StockLocation='" + stockLocation + "'  where id =(SELECT MAX(ID) FROM LcItems where EntryBy='" + Page.User.Identity.Name.ToString() + "')");
    //            ClearDetailForm();
    //            lblMsg2.Attributes.Add("class", "xerp_success");
    //            lblMsg2.Text = "Item added to grid";
    //            //}
    //            //else
    //            //{
    //            //    lblMsg2.Attributes.Add("class", "xerp_warning");
    //            //    lblMsg2.Text = "ERROR: Item Already exist! Delete from grid first...";
    //            //}
    //        }
    //        else
    //        {
    //            ExecuteUpdateItem(productName);

    //            Button1.Text = "Add to grid";
    //            ClearDetailForm();
    //            lblMsg2.Attributes.Add("class", "xerp_success");
    //            lblMsg2.Text = "Item updated successfully.";
    //        }

    //        //ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);

    //    }
    //    catch (Exception ex)
    //    {
    //        Notify(ex.Message.ToString(), "error", lblMsg);
    //        lblMsg.Text = "Error: " + ex.ToString();
    //    }
    //    finally
    //    {
    //        BindItemGrid();

    //        ItemGrid.SelectedIndex = (-1);

    //    }
    //}


    private void ClearDetailForm()
    {
        txtSpec.Text = "";
        txtWeight.Text = "";
        txtSerial.Text = "";
        txtWarrenty.Text = "";
        txtManufacturer.Text = "";
        txtCountry.Text = "";
        txtQuantity.Text = "";
        txtRate.Text = "";
        txtModel.Text = "";
        txtSpecification.Text = "";
        txtManufacturer.Text = "";
        txtCountry.Text = "";
        //txtItemDisc.Text = "";
        //txtItemVAT.Text = "";
        //txtAmt.Text = "";
    }

    private void ExecuteUpdateItem(string productName)
    {
        string lName = Page.User.Identity.Name.ToString();
        //string stockLocation = "";
        //if (ddGroup.SelectedValue == "5")
        //{
        //    stockLocation = ddSection.SelectedValue;
        //}

        //decimal ttl = Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQuantity.Text);

        string spec = "";
        if (ddSubGrp.SelectedValue == "10")//Printing Ink
        {
            spec = ddSpec.SelectedValue;
            if (txtSpec.Text != "" && lbSpec.Text == "Cancel" && ddSubGrp.SelectedValue == "10")//Insert Ink spec
            {
                string isExist = RunQuery.SQLQuery.ReturnString("SELECT id FROM Specifications WHERE  spec ='" + txtSpec.Text + "'");
                if (isExist == "")
                {
                    RunQuery.SQLQuery.ExecNonQry("INSERT INTO Specifications (spec, EntryBy) VALUES ('" + txtSpec.Text + "', '" + lName + "') ");
                    spec = RunQuery.SQLQuery.ReturnString("SELECT MAX(id) FROM Specifications");
                    LoadSpecList(""); //ddSpec.DataBind();
                    ddSpec.SelectedValue = spec;
                }
                else
                {
                    LoadSpecList("");
                    ddSpec.SelectedValue = isExist;
                }
            }
            spec = ddSpec.SelectedValue;
        }

        //SQLQuery.Empty2Zero(txtItemDisc); SQLQuery.Empty2Zero(txtItemVAT);
        string desc = "";
        SqlCommand cmd2 = new SqlCommand("UPDATE [dbo].[LcItems]   SET [GradeId] = '" + ddGrade.SelectedValue + "', [CategoryId] = '" +ddCategory.SelectedValue +
                                    "',[ItemCode] = '" + ddItemName.SelectedValue + "',[ItemSizeID] ='" + ddSize.SelectedValue + "',pcs='" +
                                    txtWeight.Text + "',[Spec] ='" + ddSpec.SelectedValue + "', NoOfPacks='" + txtSerial.Text +
                                    "', QntyPerPack='" + txtWarrenty.Text + "', [qty] = '" + txtQuantity.Text +
                                    "', FullDescription='" + desc + "', [UnitPrice] ='" + txtRate.Text + "', [Manufacturer] ='" + txtManufacturer.Text + "', [CountryOfOrigin] ='" + txtCountry.Text + "' WHERE EntryID='" + lName + "' ",
            new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //cmd2.Parameters.AddWithValue("@ProductName", productName);
        //cmd2.Parameters.AddWithValue("@Qty", Convert.ToDecimal(txtQuantity.Text));
        //cmd2.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtRate.Text));
        //cmd2.Parameters.AddWithValue("@Warrenty", txtWarrenty.Text);
        //cmd2.Parameters.AddWithValue("@SerialNo", txtSerial.Text);
        //cmd2.Parameters.AddWithValue("@SizeRef", txtRef.Text);

        string itemType = ddGroup.SelectedItem.Text;
        if (ddSubGrp.SelectedValue == "9") //Tin Plate
        {
            itemType = "Raw Sheet";
        }
        cmd2.Parameters.AddWithValue("@StockType", itemType);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
    }
    private void GridItemEditMode(string entryID)
    {
        try
        {
            //int index = Convert.ToInt32(ItemGrid.SelectedIndex);
            //Label lblItemName = ItemGrid.Rows[index].FindControl("Label1") as Label;
            //lblEntryId.Text = lblItemName.Text;

            SqlCommand cmd7 = new SqlCommand("Select ItemCode, HSCode, ItemSizeID, NoOfPacks, Thickness, Measurement, qty, UnitPrice, QntyPerPack, pcs, Manufacturer, CountryOfOrigin FROM [LcItems] WHERE EntryID=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Parameters.Add("@sl", SqlDbType.VarChar).Value = entryID;
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();

            if (dr.Read())
            {
                //Button1.Text = "Update";
                string iCode = dr[0].ToString();
                txtHSCode.Text = dr[1].ToString();

                txtSpecification.Text = dr[2].ToString();
                txtSerial.Text = dr[3].ToString();
                txtModel.Text = dr[4].ToString();
                txtQuantity.Text = dr[5].ToString();
                txtRate.Text = dr[6].ToString();
                txtWarrenty.Text = dr[7].ToString();
                txtWeight.Text = dr[8].ToString();
                txtManufacturer.Text = dr[9].ToString();
                txtCountry.Text = dr[10].ToString();

                //ddGroup.SelectedValue =SQLQuery.ReturnString("Select GroupID from ItemSubGroup where CategoryID=(Select CategoryID from ItemGrade where GradeID=(Select ))")

                string catID = SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + iCode + "'");
                string grdID = SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
                string subID = SQLQuery.ReturnString("SELECT CategoryID FROM ItemGrade WHERE GradeID ='" + grdID + "'");
                string grpID = SQLQuery.ReturnString("SELECT GroupID FROM ItemSubGroup WHERE CategoryID ='" + subID + "'");

                //ItemGrid.DataBind();
                //pnl.Update();
                Button1.Text = "Update";
            }

            cmd7.Connection.Close();

            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) Purpose, GradeId, CategoryId, ItemCode, HSCode, 
                                ItemSizeID, Spec, Thickness, Measurement, qty, UnitPrice, CFRValue, ReturnQty, NoOfPacks, QntyPerPack, 
                                Loading, Loaded, LandingPercent, LandingAmt, TotalUSD, TotalBDT, UnitCostBDT, Manufacturer, CountryOfOrigin  FROM   [LcItems] WHERE EntryID='" + lblInvoice.Text + "'");

            foreach (DataRow drx in dtx.Rows)
            {
                ddPurpose.SelectedValue = drx["Purpose"].ToString();
                string iCode = drx["ItemCode"].ToString();

                string grpID = Stock.Inventory.GetItemGroup(iCode);
                ddGroup.SelectedValue = grpID;
                string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + grpID +
                                "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
                SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");
                ddSubGrp.SelectedValue = Stock.Inventory.GetItemSubGroup(iCode);

                gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue +
                         "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
                SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
                ddGrade.SelectedValue = drx["GradeId"].ToString();

                gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue +
                         "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
                SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
                ddCategory.SelectedValue = drx["CategoryId"].ToString();

                GetProductList();

                ddItemName.SelectedValue = iCode;
                txtQuantity.Text = drx["qty"].ToString();
                string spec = drx["Spec"].ToString();

                string size = drx["ItemSizeID"].ToString();
                if (size != "")
                {
                    ddSize.SelectedValue = size;
                }

                ddSpec.SelectedValue = drx["Spec"].ToString();

                txtSerial.Text = drx["NoOfPacks"].ToString();
                txtModel.Text = drx["Measurement"].ToString();
                txtSpecification.Text = drx["Thickness"].ToString();
                txtWarrenty.Text = drx["QntyPerPack"].ToString();

                txtQuantity.Text = drx["qty"].ToString();
                txtRate.Text = drx["UnitPrice"].ToString();
                txtManufacturer.Text = drx["Manufacturer"].ToString();
                txtCountry.Text = drx["CountryOfOrigin"].ToString();

                //txtCFR.Text = drx["CFRValue"].ToString();


                //LoadItemsPanel();

                //Load color spec

                //Load color spec
                if (ddSubGrp.SelectedValue == "10") //Printing Ink
                {
                    LoadSpecList("");
                    pnlSpec.Visible = true;

                    string isExist = SQLQuery.ReturnString("Select spec from Specifications where id='" + spec + "'");
                    if (lbLCNo.Text == "New")
                    {
                        ddLCNo.Visible = false;
                        txtLCNo.Visible = true;
                        lbLCNo.Text = "Old";
                        txtLCNo.Focus();
                    }
                    else
                    {
                        ddLCNo.Visible = true;
                        txtLCNo.Visible = false;
                        lbLCNo.Text = "New";
                        //LoadSpecList("filter");
                        ddLCNo.Focus();
                    }
                }

            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }
    protected void ItemGrid_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(ItemGrid.SelectedIndex);
        Label lblItemName = ItemGrid.Rows[index].FindControl("Label1") as Label;
        lblInvoice.Text = lblItemName.Text;
        GridItemEditMode(lblItemName.Text);
    }
    private void BindItemGridPrd()
    {
        string lName = Page.User.Identity.Name.ToString();

        /* SqlCommand cmd = new SqlCommand(@"SELECT Id, (SELECT GradeName FROM [ItemGrade] where CategoryID=((SELECT CategoryName FROM [Categories] where CategoryID=) SELECT CategoryID FROM [Products] where ProductID='"+ddItemName.SelectedValue+"')) As Grade,"+
         "((SELECT CategoryName FROM [Categories] where CategoryID=) SELECT CategoryID FROM [Products] where ProductID='"+ddItemName.SelectedValue+"') As Category, "+
         " ,Itemgroup, ItemCode, ItemName, Qty, Price, Total, Manufacturer, CountryOfOrigin, PackSize, Warrenty, SerialNo, UnitType, SizeRef FROM LCVoucherProducts WHERE EntryBy=@EntryBy AND InvNo='' ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
         */

        string editLCNo = Convert.ToString(Session["EditLCNo"]);
        if (!string.IsNullOrEmpty(editLCNo)|| lbLCNo.Text=="New")
        {
            SQLQuery.PopulateGridView(ItemGrid, @"SELECT EntryID, 
                                                (Select Purpose from Purpose where pid=a.Purpose) as Purpose, 
                                                (Select GradeName from ItemGrade where GradeID=a.GradeId) as Grade, 
                                                (Select CategoryName from Categories where CategoryID=a.CategoryId) as Category, 
                                                (Select ItemName from Products where ProductID=a.ItemCode) as Product,                      
                                                [Thickness], (Select BrandName from Brands where BrandID=a.ItemSizeID) as Size, Measurement, (Select spec from Specifications where id=a.spec) as spec,
                                                CONVERT(varchar(10), qty) +' '+(Select UnitType from Products where ProductID=a.ItemCode) As QTY1, UnitPrice,  [CFRValue] 
                                                FROM [LcItems] a Where  LCNo='" + ddLCNo.SelectedValue + "' ORDER BY [EntryID]");
            //SQLQuery.PopulateGridView(GridExpenses, "SELECT EID, ExpHeadName, Amount FROM PurchaseExpenses WHERE  InvoiceNo='" + lblInvoice.Text + "' ORDER BY eid");
        }
        else
        {
            SqlCommand cmd2 = new SqlCommand(@"SELECT EntryID, 
                                                (Select Purpose from Purpose where pid=a.Purpose) as Purpose, 
                                                (Select GradeName from ItemGrade where GradeID=a.GradeId) as Grade, 
                                                (Select CategoryName from Categories where CategoryID=a.CategoryId) as Category, 
                                                (Select ItemName from Products where ProductID=a.ItemCode) as Product,                      
                                                [Thickness], (Select BrandName from Brands where BrandID=a.ItemSizeID) as Size, Measurement, (Select spec from Specifications where id=a.spec) as spec,
                                                CONVERT(varchar(10), qty) +' '+(Select UnitType from Products where ProductID=a.ItemCode) As QTY1, UnitPrice,  [CFRValue] 
                                                FROM [LcItems] a Where  LCNo='' AND EntryBy='" + lName + "' ORDER BY [EntryID]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
            cmd2.Connection.Open();
            ItemGrid.EmptyDataText = "No items to view...";
            ItemGrid.DataSource = cmd2.ExecuteReader();
            ItemGrid.DataBind();
            cmd2.Connection.Close();

            //SQLQuery.PopulateGridView(GridExpenses, "SELECT EID, ExpHeadName, Amount FROM PurchaseExpenses WHERE  EntryBy='" + lName + "' AND InvoiceNo='" + lblInvoice.Text + "' ORDER BY eid");
            //SqlCommand cmd = new SqlCommand("SELECT EID, ExpHeadName, Amount FROM PurchaseExpenses WHERE  EntryBy='" + lName + "' AND InvoiceNo='" + lblInvoice.Text + "' ORDER BY eid", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            //cmd.Connection.Open();
            ////GridExpenses.EmptyDataText = "No items to view...";
            ////GridExpenses.DataSource = cmd.ExecuteReader();
            ////GridExpenses.DataBind();
            //cmd.Connection.Close();
        }
    }

    protected void ddLCNo_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindItemGridPrd();

    }
   
    protected void lbLCNo_OnClick(object sender, EventArgs e)
    {
        if (lbLCNo.Text == "New")
        {
            ddLCNo.Visible = false;
            txtLCNo.Visible = true;
            lbLCNo.Text = "Old";
            txtLCNo.Focus();
            GridView1.Visible = false;
            Session["EditLCNo"]="";
            BindItemGridPrd();
        }
        else
        {
            ddLCNo.Visible = true;
            txtLCNo.Visible = false;
            lbLCNo.Text = "New";
            //LoadSpecList("filter");
            ddLCNo.Focus();
            GridView1.Visible = true;
            BindItemGridPrd();
        }
    }

    protected void cbStatus_OnCheckedChanged(object sender, EventArgs e)
    {
        Status();
    }
}