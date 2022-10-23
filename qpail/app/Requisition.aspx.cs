using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;
using Stock;

public partial class app_Requisition : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Page.Form.Attributes.Add("enctype", "multipart/form-data");
            //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
            //btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");
            //btnOthExp.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnOthExp, null) + ";");

            //PartyBalance(ddVendor.SelectedValue, ddGodown.SelectedValue);

            if (!IsPostBack)
            {
                //InvIDNo();
                string lName = Page.User.Identity.Name.ToString();
                lblProject.Text = RunQuery.SQLQuery.ReturnString("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'");
                txtQuantity.Text = "";
                txtRate.Text = "";

                txtDate.Text = DateTime.Now.ToShortDateString();

                //LoadFormInfo();

                ddGroup.DataBind();
                string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
                SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

                gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
                SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

                gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
                SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");

                //ddSuppCategory.DataBind();
                //ddVendor.DataBind();
                GetProductList();
                BindItemGrid();
                txtFor.Focus();
                ddPurpose.DataBind();
                ddPurpose.SelectedValue = "1";
                //cash-chq payment
                ddMode.DataBind();
                LoadPayMode();

                string formtype = Request.QueryString["type"];
                if (formtype == "edit")
                {
                    ddInvoice.DataBind();
                    pnlReturnHistory.Visible = true;
                    GridView2.DataBind();
                    string invId = Request.QueryString["inv"];
                    if (invId != null)
                    {
                        ddInvoice.SelectedValue = invId;
                    }
                    lblInvoice.Text = ddInvoice.SelectedValue;
                    LoadEditMode(ddInvoice.SelectedValue);
                    //GetTotalAmount();

                    divinvoice.Attributes.Remove("Class");
                    divinvoice.Attributes.Add("Class", "control-group");
                    Page.Title = "Purchase Edit";
                    ltrHead.Text = "Purchase Edit";
                }
            }

            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }

    //Message or Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    private void ClearForm()
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Cl", "$('input[type=text]').val('');", true);
    }


    // Getting ExpenseID
    public string InvIdNo()
    {
        string InvNo = "Req-" + SQLQuery.ReturnString("Select CONVERT(varchar, (ISNULL (COUNT(Id),0)+1001 )) FROM Requisition");
        string isExist = SQLQuery.ReturnString("Select ReqNo FROM Requisition where ReqNo='" + InvNo + "' ");
        int i = 1001;
        while (isExist != "")
        {
            InvNo = "Req-" + SQLQuery.ReturnString("Select CONVERT(varchar, (ISNULL (COUNT(Id),0)+" + i + " )) FROM Requisition");
            isExist = SQLQuery.ReturnString("Select ReqNo FROM Requisition where ReqNo='" + InvNo + "' ");
            i++;
        }
        return InvNo;
    }


    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSpecList("filter");
        txtQuantity.Focus();
        RecentInfo();
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ddVendor.SelectedValue != "")
            //{
                //if (Convert.ToDecimal(lblItemAmount.Text) > 0)
                //{
                    string invNo = InvIdNo();
                    //decimal ttlAmt = Convert.ToDecimal(GetTotalAmount());
                    //if (ttlAmt > 0)
                    //{
                        if (lblInvoice.Text == "")
                        {
                            SaveData(invNo);
                            Notify("New Purchase Saved Successfully.", "success", lblMsg2);
                            ClearForm();
                        }
                        else
                        {
                            SQLQuery.ExecNonQry("Delete Requisition WHERE  ReqNo=  '" + lblInvoice.Text + "'");
                            //SQLQuery.ExecNonQry("Delete  Transactions WHERE  InvNo=  '" + lblInvoice.Text + "'");

                            SaveData(lblInvoice.Text);
                            Notify("Requisition info Successfully Updated.", "success", lblMsg2);
                        }

                        //string url = "./invoice.aspx?Inv=" + invNo;
                        //ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");
                        //pnl.Update();

                        ItemGrid2.DataBind();
                        string formtype = Request.QueryString["type"];
                        if (formtype == "edit")
                        {
                            Page.Title = "Purchase Edit";
                            ltrHead.Text = "Purchase Edit";
                        }
                        else
                        {
                            Page.Title = "Purchase Entry";
                        }

                        GridView1.DataBind();

                    //}
                    //else
                    //{
                    //    lblMsg2.Attributes.Add("class", "xerp_error");
                    //    lblMsg2.Text = "ERROR: No item was found in Purchase order!";
                    //    Notify("ERROR: No item was found in Purchase order!", "warn", lblMsg2);
                    //}
                //}
                //else
                //{
                //    lblMsg.Attributes.Add("class", "xerp_warning");
                //    lblMsg.Text = "ERROR: Item value should be greater then 0";
                //    Notify("ERROR: Item value should be greater then 0", "warn", lblMsg);
                //}
            //}
            //else
            //{
            //    Notify("ERROR: Please select supplier...", "warn", lblMsg);
            //}
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "Error: " + ex.ToString();
        }
        finally
        {
            BindItemGrid();
            //
        }
    }

    private void SaveData(string invNo)
    {
        string lName = Page.User.Identity.Name.ToString();
        //string otherExpense = SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) from PurchaseExpenses where InvoiceNo='" + lblInvoice.Text + "'");

        SqlCommand cmd2 = new SqlCommand("INSERT INTO Requisition (Date, ReqNo, FromDept, Remarks, EntryBy)" +
                                                            "VALUES (@Date, @ReqNo, @FromDept, @Remarks, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.Add("@Date", SqlDbType.DateTime).Value = txtDate.Text;
        cmd2.Parameters.Add("@ReqNo", SqlDbType.NVarChar).Value = invNo;
        cmd2.Parameters.Add("@FromDept", SqlDbType.NVarChar).Value = 0;
        
        //cmd2.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = txtOrderDate.Text;
        //cmd2.Parameters.Add("@SupplySource", SqlDbType.NVarChar).Value = ddType.SelectedValue;
        //cmd2.Parameters.Add("@BillNo", SqlDbType.NVarChar).Value = txtReqNo.Text;
        //cmd2.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = txtDate.Text;
        //cmd2.Parameters.Add("@SupplierID", SqlDbType.NVarChar).Value = ddVendor.SelectedValue;
        //cmd2.Parameters.Add("@SupplierName", SqlDbType.NVarChar).Value = ddVendor.SelectedItem.Text;

        decimal invTotal = 0;// Convert.ToDecimal(GetTotalAmount());
        string iTotal = lblItemAmount.Text;

        //cmd2.Parameters.Add("@ChallanNo", SqlDbType.NVarChar).Value = txtChallanNo.Text;
        //cmd2.Parameters.Add("@ItemTotal", SqlDbType.Decimal).Value = iTotal;
        //cmd2.Parameters.Add("@PurchaseDiscount", SqlDbType.Decimal).Value = txtPDiscount.Text;
        //cmd2.Parameters.Add("@VatService", SqlDbType.Decimal).Value = txtVat.Text;
        //cmd2.Parameters.Add("@OtherExp", SqlDbType.Decimal).Value = lblOthersTotal.Text;
        //cmd2.Parameters.Add("@PurchaseTotal", SqlDbType.Decimal).Value = invTotal;

        //cmd2.Parameters.Add("@TransportType", SqlDbType.NVarChar).Value = txtTransportType.Text;
        //cmd2.Parameters.Add("@WarehouseID", SqlDbType.NVarChar).Value = ddGodown.SelectedValue;
        //cmd2.Parameters.Add("@StockLocationID", SqlDbType.NVarChar).Value = ddLocation.SelectedValue;
        //cmd2.Parameters.Add("@PaymentMode", SqlDbType.NVarChar).Value = ddMode.SelectedValue;
        //cmd2.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = txtPaid.Text;
        cmd2.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = txtRemark.Text;
        cmd2.Parameters.Add("@EntryBy", SqlDbType.VarChar).Value = lName;

        cmd2.Connection.Open();
        int success = cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        string reqId = SQLQuery.ReturnString("Select MAX(Id) from Requisition"); //txtInvoiceNo.Text;
        //SQLQuery.ExecNonQry("UPDATE purchase set ChallanDate='" + txtChallanDate.Text + "' WHERE InvNo='" + invNo + "'");

        /*Accounts Link disabled
        //Cancel old voucher for edit
        VoucherEntry.AutoVoucherEntry("7", "", "", 0, invNo, lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "0");
        //Insert new voucher for purchase
        string voucherNo = "Auto-" + DateTime.Now.Year.ToString() + "-" + RunQuery.SQLQuery.ReturnString("Select ISNULL(MAX(VID),0)+1001 From VoucherMaster");
        VoucherEntry.InsertVoucherMaster(voucherNo, "Purchase from " + ddVendor.SelectedItem.Text , "6", ddInventoriesHead.SelectedValue, "020102006", invTotal, invTotal, lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), invNo);
        */

        //update order details
        SqlCommand cmd = new SqlCommand("UPDATE RequisitionDetails SET ReqId='" + reqId + "' WHERE  ReqId='" + lblInvoice.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryBy", lName);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        //SQLQuery.ExecNonQry("UPDATE PurchaseExpenses SET InvoiceNo='" + invNo + "' WHERE EntryBy='" + lName + "' AND InvoiceNo='" + lblInvoice.Text + "'");


        //if (FileUpload1.HasFile)
        //{
        //    string photoURL = SQLQuery.UploadImage("Purchase: " + invNo, FileUpload1, Server.MapPath("..\\Uploads\\Photos\\"), Server.MapPath("../Uploads/Photos/"), Page.User.Identity.Name.ToString(), "Group");
        //    SQLQuery.ExecNonQry("UPDATE purchase SET Doc='" + photoURL + "' where (InvoiceNo='" + lblInvoice.Text + "')");
        //}


        //Party Transaction
        //string accHead = RunQuery.SQLQuery.ReturnString("Select AccHeadID FROM  Settings_Transaction where TransactionType='Purchase'");
        //Accounting.VoucherEntry.TransactionEntry(invNo, txtDate.Text, ddVendor.SelectedValue, ddVendor.SelectedItem.Text, "Purchase", "0", invTotal.ToString(), "0", "Purchase", "Supplier", accHead, lName, "1");


        //SQLQuery.ExecNonQry("Delete Stock where InvoiceID='" + invNo + "'");
        //SQLQuery.ExecNonQry("DELETE LcItems WHERE LCNo='" + invNo + "' AND LCType='Purchase'");

        //insert stock qty
        foreach (GridViewRow row in ItemGrid.Rows)
        {
            Label lblEntryId = row.FindControl("lblEntryId") as Label;
            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT Id, Purpose, Itemgroup, ItemCode, ItemName, Qty, Price, Total, Manufacturer, PackSize, Warrenty, SerialNo, Specification, UnitType, SizeRef, StockType, Pcs, CountryOfOrigin FROM [RequisitionDetails] WHERE Id='" + lblEntryId.Text + "'");

            foreach (DataRow drx in dtx.Rows)
            {
                string entryId = drx["Id"].ToString();
                string Purpose = drx["Purpose"].ToString();
                string iGrp = drx["Itemgroup"].ToString();
                string iCode = drx["ItemCode"].ToString();
                string iName = drx["ItemName"].ToString();
                decimal qty = Convert.ToDecimal(drx["Qty"].ToString());
                string price = drx["Price"].ToString();
                string total = drx["Total"].ToString();

                //ItemCode, ItemName, Qty, Price, Total, Manufacturer, CountryOfOrigin, PackSize, Warrenty, SerialNo,
                //UnitType, SizeRef, StockType, StockLocation, PerviousDeliveredQty, QtyBalance, EntryBy, EntryDate, ReturnQty
                //string Manufacturer = drx["Manufacturer"].ToString();
                string CountryOfOrigin = drx["CountryOfOrigin"].ToString();
                string PackSize = drx["PackSize"].ToString();
                string Warrenty = drx["Warrenty"].ToString();
                string SerialNo = drx["SerialNo"].ToString();
                //string UnitType = drx["UnitType"].ToString();

                string SizeRef = drx["SizeRef"].ToString();
                string StockType = drx["StockType"].ToString();
                string Specification = drx["Specification"].ToString();

                string pcs1 = drx["pcs"].ToString();
                int pcs = Convert.ToInt32(pcs1.Substring(0, pcs1.Length - 3));

                /*decimal itemCostwithVat = Accounting.VoucherEntry.CalculateItemCosting(Convert.ToDecimal(price),
                    Convert.ToDecimal(txtVat.Text), Convert.ToDecimal(otherExpense),
                    Convert.ToDecimal(txtPDiscount.Text), Convert.ToDecimal(lblItemAmount.Text), Convert.ToDecimal(total), qty);

                decimal itemCostwithoutVat = Accounting.VoucherEntry.CalculateItemCosting(Convert.ToDecimal(price),
                    0, Convert.ToDecimal(otherExpense), Convert.ToDecimal(txtPDiscount.Text), Convert.ToDecimal(lblItemAmount.Text), Convert.ToDecimal(total), qty);*/

                //SQLQuery.ExecNonQry("UPDATE RequisitionDetails SET PriceWithVAT='" + itemCostwithVat + "', PriceWithoutVAT ='" + itemCostwithoutVat + "' WHERE id='" + lblEntryId.Text + "'");

                string detail = SerialNo + " - " + Warrenty;
                if (Convert.ToDecimal(ddGroup.SelectedValue) > 3 && Convert.ToDecimal(ddGroup.SelectedValue) != 7)//Machines, Electric, stationaries, others except wastage.
                {
                    detail = "Purchase Date: " + txtDate.Text + ". Specification: " + SerialNo + ". Origin: " + CountryOfOrigin +
                                  ". Warranty: " + Warrenty;
                    pcs = Convert.ToInt32(qty);
                }

                //else if (pcs==0 && qty>0)
                //{
                //    pcs = Convert.ToInt32(qty);
                //}

                /*Stock.Inventory.SaveToStock(Purpose, invNo,
                    "Purchase from " + ddVendor.SelectedItem.Text, lblEntryId.Text, PackSize, "", "", "",
                    SizeRef, iCode, iName, StockType, ddGodown.SelectedValue, ddLocation.SelectedValue,
                    iGrp, pcs, 0, Convert.ToDecimal(price), qty, 0, detail, "Stock-in", "Purchase",
                    ddLocation.SelectedItem.Text, lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), Specification);*/

                //Accounting.VoucherEntry.StockEntry(invNo, "Purchase", entryId, size, "", iCode, iName, ddGodown.SelectedValue, ddLocation.SelectedValue, iGrp, qty, "0", "0", "0", guaranty, "Stock-in", stockType, stockLocation, lName);



                //Insert for items consumptions during sales
                if (iGrp == "1")
                {
                    /*SQLQuery.ExecNonQry("INSERT INTO LcItems (LCNo, LCType, ReceiveDate, Purpose, GradeId, CategoryId, ItemCode, HSCode, Thickness, Measurement, NoOfPacks, QntyPerPack,  qty, UnitPrice, TotalBDT, Loading, LandingPercent, pcs, EntryBy, FullDescription, Manufacturer, CountryOfOrigin, UnitCostBDT)" +
                                                                     " VALUES ('" + invNo + "', '" + ddType.SelectedValue + "', '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + Purpose + "', '" + Inventory.GetItemGrade(iCode) + "', '" + Inventory.GetItemCategory(iCode) + "', '" + iCode + "', '', '" + StockType + "', '" + SizeRef + "', '" + SerialNo + "','" + Warrenty +
                                                                                    "', '" + qty + "', '" + price + "', '" + total + "', '0',  '0',  '" + 0 + "',  '" + lName + "', 'Local Purchase from " + ddVendor.SelectedItem.Text + "','" + ddManufacturer.SelectedValue + "','" + CountryOfOrigin + "','" + itemCostwithVat / qty + "')");*/


                }
            }
        }


        /*if (Convert.ToDecimal(txtPaid.Text) > 0)
        {
            //Payment Entry
            string payId = invNo; //RunQuery.SQLQuery.ReturnString("Select 'Pay-'+ CONVERT(varchar, (ISNULL (max(PaymentID),0)+1001 )) from Payment");
            SqlCommand cmd2 = new SqlCommand("INSERT INTO Payment (PaymentNo, PaymentDate, PartyID, PartyName, PurchaseInvNo, PaidAmount, PayType, Remark, EntryBy) VALUES (@PaymentNo, @PaymentDate, @PartyID, @PartyName, @PurchaseInvNo, @PaidAmount, @PayType, @Remark, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.AddWithValue("@PaymentNo", payId);
            cmd2.Parameters.AddWithValue("@PaymentDate", Convert.ToDateTime(txtDate.Text));
            //cmd2.Parameters.AddWithValue("@PartyID", ddVendor.SelectedValue);
            //cmd2.Parameters.AddWithValue("@PartyName", ddVendor.SelectedItem.Text);
            cmd2.Parameters.AddWithValue("@PurchaseInvNo", invNo);

            cmd2.Parameters.AddWithValue("@PaidAmount", Convert.ToDecimal(txtPaid.Text));
            cmd2.Parameters.AddWithValue("@PayType", ddMode.SelectedValue);
            cmd2.Parameters.AddWithValue("@Remark", ddMode.SelectedValue + " payment for " + invNo + ". " + txtRemark.Text);
            cmd2.Parameters.AddWithValue("@EntryBy", lName);

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();


            //insert transaction
            if (ddMode.SelectedValue == "Cash")
            {
                accHead = RunQuery.SQLQuery.ReturnString("Select AccHeadID FROM  Settings_Transaction where TransactionType='Payment'");
                //Accounting.VoucherEntry.TransactionEntry(invNo, txtDate.Text, ddVendor.SelectedValue, ddVendor.SelectedItem.Text, "Cash paid for purchase", txtPaid.Text, "0", "0", "Payment", "Supplier", accHead, lName, "1");

                accHead = RunQuery.SQLQuery.ReturnString("Select AccHeadID FROM  Settings_Transaction where TransactionType='Cash'");


                /*Accounts Link disabled
                //Insert new voucher for cash payment
                voucherNo = "Auto-" + DateTime.Now.Year.ToString() + "-" + RunQuery.SQLQuery.ReturnString("Select ISNULL(MAX(VID),0)+1001 From VoucherMaster");
                VoucherEntry.InsertVoucherMaster(voucherNo, "Paid to " + ddVendor.SelectedItem.Text, "9", "020102006", "010101001", Convert.ToDecimal(txtPaid.Text), Convert.ToDecimal(txtPaid.Text), lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), invNo);
                */
          /*}
            else if (ddMode.SelectedValue == "Cheque")
            {
                string chqBank = RunQuery.SQLQuery.ReturnString("Select BankID FROM BankAccounts where ACID='" + ddBank.SelectedValue + "'");
                string chqBankBranch = RunQuery.SQLQuery.ReturnString("Select Address FROM BankAccounts where ACID='" + ddBank.SelectedValue + "'");
                SqlCommand cmd4 = new SqlCommand("INSERT INTO Cheque (TrType, TrID, ChequeNo, ChqBank, ChqBankBranch, ChqDate, ChqAmt, PartyID, ChequeName, Remark, EntryBy)" +
                                                            " VALUES (@TrType, @TrID, @ChequeNo, @ChqBank, @ChqBankBranch, @ChqDate, @ChqAmt, @PartyID, @ChequeName, @Remark, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd4.Parameters.AddWithValue("@TrType", "Payment");
                cmd4.Parameters.AddWithValue("@TrID", payId);
                cmd4.Parameters.AddWithValue("@ChequeNo", txtDetail.Text);
                cmd4.Parameters.AddWithValue("@ChqBank", chqBank);
                cmd4.Parameters.AddWithValue("@ChqBankBranch", chqBankBranch);

                cmd4.Parameters.AddWithValue("@ChqDate", Convert.ToDateTime(txtChqDate.Text));
                cmd4.Parameters.AddWithValue("@ChqAmt", Convert.ToDecimal(txtPaid.Text));
                //cmd4.Parameters.AddWithValue("@PartyID", Convert.ToInt32(ddVendor.SelectedValue));
                //cmd4.Parameters.AddWithValue("@ChequeName", ddVendor.SelectedItem.Text);
                cmd4.Parameters.AddWithValue("@Remark", txtRemark.Text);
                cmd4.Parameters.AddWithValue("@EntryBy", lName);

                cmd4.Connection.Open();
                cmd4.ExecuteNonQuery();
                cmd4.Connection.Close();
            }
        }*/
    }
    private void PartyBalance(string vendor, string customer)
    {
        //Get current Balance
        SqlCommand cmd = new SqlCommand("SELECT isnull(SUM(Dr),0) - isnull(SUM(Cr),0) FROM Transactions WHERE HeadName ='" + vendor + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        //vBalance.Text = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        //Get current Balance
        SqlCommand cmd2 = new SqlCommand("SELECT isnull(SUM(Dr),0) - isnull(SUM(Cr),0) FROM Transactions WHERE HeadName ='" + customer + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        //cBalance.Text = Convert.ToString(cmd2.ExecuteScalar());
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
    }


    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        double price = 0; double rate = 0;
        if (txtQuantity.Text != "" && txtRate.Text != "")
        {
            price = Convert.ToDouble(txtQuantity.Text);
            rate = Convert.ToDouble(txtRate.Text);
        }

        if (price > 0 && rate > 0)
        {
            txtAmt.Text = Convert.ToString(price * rate);
        }
        else
        {
            lblMsg.Text = "Please Type Rate & Price Properly";
        }
    }
    protected void txtSaleRate_TextChanged(object sender, EventArgs e)
    {
        double price = 0; double rate = 0;
        if (txtQuantity.Text != "")
        {
            price = Convert.ToDouble(txtQuantity.Text);
            //rate = Convert.ToDouble(txtSaleRate.Text);
        }
        if (price > 0 && rate > 0)
        {
            //txtSaleTaka.Text = Convert.ToString(price * rate);
        }
        else
        {
            lblMsg2.Text = "Please Type Rate & Price Properly";
        }
    }

    protected void btnPConv_Click(object sender, EventArgs e)
    {
        //decimal pAmt = Convert.ToDecimal(TextBox1.Text) * Convert.ToDecimal(TextBox2.Text) + Convert.ToDecimal(TextBox3.Text);
        //txtQuantity.Text = pAmt.ToString();
    }

    protected void txtQuantity_TextChanged(object sender, EventArgs e)
    {
        double price = 0;
        double prate = 0;
        double rate = 0;

        if (txtQuantity.Text != "" && txtRate.Text != "")
        {
            price = Convert.ToDouble(txtQuantity.Text);
            prate = Convert.ToDouble(txtRate.Text);
            //rate = Convert.ToDouble(txtSaleRate.Text);
        }
        if (price > 0 && prate > 0 && rate > 0)
        {
            //txtSaleTaka.Text = Convert.ToString(price * rate);
            txtAmt.Text = Convert.ToString(price * prate);
            //txtQuantity.Focus();
        }
    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (txtQuantity.Text == "")
            {
                txtQuantity.Text = "1";
            }

            //SizeId, ProductID, BrandID
            SqlCommand cmde = new SqlCommand("SELECT ItemName FROM RequisitionDetails WHERE ItemCode ='" + ddItemName.SelectedValue + "' AND  ReqId ='" + lblInvoice.Text + "' AND EntryBy ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmde.Connection.Open();
            string isExist = Convert.ToString(cmde.ExecuteScalar());
            cmde.Connection.Close();

            string productName = ddItemName.SelectedItem.Text;// +" -" + ddCategory.SelectedItem.Text + " " + ddSubGrp.SelectedItem.Text;

            if (btnAdd.Text == "Add to grid")
            {
                //if (isExist == "")
                //{
                string spec = "";
                if (ddSubGrp.SelectedValue == "10")//Printing Ink
                {
                    spec = ddSpec.SelectedValue;
                    if (txtSpec.Text != "" && lbSpec.Text == "Cancel" && ddSubGrp.SelectedValue == "10")//Insert Ink spec
                    {
                        isExist = RunQuery.SQLQuery.ReturnString("SELECT id FROM Specifications WHERE  spec ='" + txtSpec.Text + "'");
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
                SQLQuery.Empty2Zero(txtWeight);
                SQLQuery.Empty2Zero(lblLastQty);
                SQLQuery.Empty2Zero(lblLastPrice);
                
                SqlCommand cmd2 = new SqlCommand("INSERT INTO RequisitionDetails (ReqId, Purpose, Itemgroup, SubGroup, Grade, Category, ItemCode, ItemName, Qty, Price, SubTotal, ItemDisc, ItemVAT, Total, Warrenty, SerialNo, ModelNo, Specification, UnitType, SizeRef, StockType, Pcs, AvailableQty, LastPurDate, LastPurQty, LastPurPrice, EntryBy) " +
                                                 "VALUES (@ReqId, '" + ddPurpose.SelectedValue + "', @Itemgroup, '" + ddSubGrp.SelectedValue + "', '" + ddGrade.SelectedValue + "', '" + ddCategory.SelectedValue + "',   @ItemCode, @ItemName, @Qty, @Price, @SubTotal, @ItemDisc, @ItemVAT,  @Total, @Warrenty, @SerialNo, '" + txtModel.Text + "','" + txtSpecification.Text + "', @UnitType, @SizeRef, @StockType,'" + txtWeight.Text + "', '" + txtAvailableQty.Text + "',  @LastPurDate, @LastPurQty, @LastPurPrice, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.AddWithValue("@ReqId", "");
                cmd2.Parameters.AddWithValue("@Itemgroup", ddGroup.SelectedValue);
                cmd2.Parameters.AddWithValue("@ItemCode", ddItemName.SelectedValue);
                cmd2.Parameters.AddWithValue("@ItemName", productName);
                cmd2.Parameters.AddWithValue("@Qty", Convert.ToDecimal(txtQuantity.Text));
                cmd2.Parameters.AddWithValue("@Price", 0); //Convert.ToDecimal(txtRate.Text));

                //decimal subTotal = Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQuantity.Text);
                SQLQuery.Empty2Zero(txtItemDisc); SQLQuery.Empty2Zero(txtItemVAT);
                cmd2.Parameters.AddWithValue("@SubTotal", 0); //subTotal);
                cmd2.Parameters.AddWithValue("@ItemDisc", 0); //Convert.ToDecimal(txtItemDisc.Text));
                cmd2.Parameters.AddWithValue("@ItemVAT", 0); //Convert.ToDecimal(txtItemVAT.Text));

                cmd2.Parameters.AddWithValue("@Total", 0); //subTotal - Convert.ToDecimal(txtItemDisc.Text) + Convert.ToDecimal(txtItemVAT.Text));
                cmd2.Parameters.AddWithValue("@Warrenty", txtWarrenty.Text);
                cmd2.Parameters.AddWithValue("@SerialNo", txtSerial.Text);
                cmd2.Parameters.AddWithValue("@UnitType", ltrUnitType.Text);
                cmd2.Parameters.AddWithValue("@SizeRef", spec);

                string itemType = ddGroup.SelectedItem.Text;
                if (ddSubGrp.SelectedValue == "9") //Tin Plate
                {
                    itemType = "Raw Sheet";
                }
                cmd2.Parameters.AddWithValue("@StockType", itemType);
                
                cmd2.Parameters.AddWithValue("@LastPurDate", SqlDbType.DateTime).Value = lblLastDate.Text;
                cmd2.Parameters.AddWithValue("@LastPurQty", Convert.ToDecimal(lblLastQty.Text));
                cmd2.Parameters.AddWithValue("@LastPurPrice", Convert.ToDecimal(lblLastPrice.Text));
                cmd2.Parameters.AddWithValue("@EntryBy", lName);

                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();

                //string stockLocation = "";
                //if (ddGroup.SelectedValue != "0") //(ddGroup.SelectedValue == "5")
                //{
                //    stockLocation = ddSection.SelectedValue;
                //}

                //SQLQuery.ExecNonQry("UPDATE RequisitionDetails SET Manufacturer='" + ddManufacturer.SelectedValue + "', CountryOfOrigin='" + txtCountry.Text + "', PackSize='" + ddSize.SelectedValue + "' where Id =(SELECT MAX(Id) FROM RequisitionDetails where EntryBy='" + Page.User.Identity.Name.ToString() + "')");
                ClearDetailForm();
                lblMsg2.Attributes.Add("class", "xerp_success");
                lblMsg2.Text = "Item added to grid";
                //}
                //else
                //{
                //    lblMsg2.Attributes.Add("class", "xerp_warning");
                //    lblMsg2.Text = "ERROR: Item Already exist! Delete from grid first...";
                //}
            }
            else
            {
                ExecuteUpdate(productName);

                btnAdd.Text = "Add to grid";
                ClearDetailForm();
                lblMsg2.Attributes.Add("class", "xerp_success");
                lblMsg2.Text = "Item updated successfully.";
            }

            //ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);

        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "Error: " + ex.ToString();
        }
        finally
        {
            BindItemGrid();
            ItemGrid.SelectedIndex = (-1);

        }
    }

   /*private string GetTotalAmount()
    {
        
        string lName = Page.User.Identity.Name.ToString();
        string amt = SQLQuery.ReturnString("SELECT ISNULL(SUM(Total),0)  FROM RequisitionDetails WHERE EntryBy='" + lName + "' AND InvNo=''");

        SQLQuery.Empty2Zero(txtPDiscount);
        SQLQuery.Empty2Zero(txtVat);

        if (lblInvoice.Text == "")
        {
            lblOthersTotal.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(Amount),0) FROM PurchaseExpenses WHERE  EntryBy='" + lName +
                                      "' AND InvoiceNo=''") + " ";
        }
        else
        {
            amt = SQLQuery.ReturnString("SELECT ISNULL(SUM(Total),0)  FROM RequisitionDetails WHERE InvNo='" + lblInvoice.Text + "'");
            lblOthersTotal.Text = "" + SQLQuery.ReturnString("SELECT ISNULL(SUM(Amount),0) FROM PurchaseExpenses WHERE InvoiceNo='" + lblInvoice.Text + "'");
        }

        lblItemAmount.Text = amt;
        txtGTotal.Text = Convert.ToString(Convert.ToDecimal(amt) - Convert.ToDecimal(txtPDiscount.Text) + Convert.ToDecimal(lblOthersTotal.Text) + Convert.ToDecimal(txtVat.Text));
        return txtGTotal.Text;
    }*/

    private void ClearDetailForm()
    {
        txtSpec.Text = "";
        txtWeight.Text = "";
        txtSerial.Text = "";
        txtWarrenty.Text = "";
        //ddManufacturer.SelectedValue = "";
        txtCountry.Text = "";
        txtQuantity.Text = "";
        txtRate.Text = "";
        txtItemDisc.Text = "";
        txtItemVAT.Text = "";
        txtAmt.Text = "";
        lblLastDate.Text = "";
        lblLastQty.Text = "";
        lblLastPrice.Text = "";
        txtAvailableQty.Text = "0";
    }

    private void ExecuteUpdate(string productName)
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

        SQLQuery.Empty2Zero(txtItemDisc); SQLQuery.Empty2Zero(txtItemVAT);
        SQLQuery.Empty2Zero(lblLastQty); SQLQuery.Empty2Zero(lblLastPrice);
        SqlCommand cmd2 = new SqlCommand("UPDATE RequisitionDetails SET " +@"Purpose= '" + ddPurpose.SelectedValue + "', [ItemGroup] ='" + ddGroup.SelectedValue + "', [SubGroup] ='" + ddSubGrp.SelectedValue + "'," +
                                         "[Grade] = '" + ddGrade.SelectedValue + "',[Category] = '" + ddCategory.SelectedValue + "' ,[ItemCode] = '" + ddItemName.SelectedValue + "'," +
                                         "[ItemName] = '" + ddItemName.SelectedItem.Text + "', Pcs='" + txtWeight.Text + "', [Qty] = '" + txtQuantity.Text + "',[Price] = '" + txtRate.Text + "'," +
                                         "[SubTotal] = '" + 0 + "',[ItemDisc] = '" + txtItemDisc.Text + "',[ItemVAT] = '" + txtItemVAT.Text + "'," +
                                         "[Total] = '" + (0) + "', [CountryOfOrigin] = '" + txtCountry.Text + "', [PackSize] = '" + ddSize.SelectedValue + "'," +
                                         "[Warrenty] = '" + txtWarrenty.Text + "',[SerialNo] = '" + txtSerial.Text + "', ModelNo='" + txtModel.Text + "', Specification='" + txtSpecification.Text + "', " +
                                         "[UnitType] = '" + ltrUnitType.Text + "' ,[SizeRef] ='" + spec + "',[StockType] = @StockType, AvailableQty='" + txtAvailableQty.Text + "', LastPurDate=@LastPurDate, LastPurQty=@LastPurQty, LastPurPrice=@LastPurPrice where id ='" + lblOrderID.Text + "' ",
            new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductName", productName);
        cmd2.Parameters.AddWithValue("@Qty", Convert.ToDecimal(txtQuantity.Text));
        cmd2.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtRate.Text));
        cmd2.Parameters.AddWithValue("@Warrenty", txtWarrenty.Text);
        cmd2.Parameters.AddWithValue("@SerialNo", txtSerial.Text);
        //cmd2.Parameters.AddWithValue("@SizeRef", txtRef.Text);

        string itemType = ddGroup.SelectedItem.Text;
        if (ddSubGrp.SelectedValue == "9") //Tin Plate
        {
            itemType = "Raw Sheet";
        }
        cmd2.Parameters.AddWithValue("@StockType", itemType);

        cmd2.Parameters.AddWithValue("@LastPurDate", SqlDbType.DateTime).Value = lblLastDate.Text;
        cmd2.Parameters.AddWithValue("@LastPurQty", Convert.ToDecimal(lblLastQty.Text));
        cmd2.Parameters.AddWithValue("@LastPurPrice", Convert.ToDecimal(lblLastPrice.Text));
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
    }

    private void BindItemGrid()
    {
        string lName = Page.User.Identity.Name.ToString();

        /* SqlCommand cmd = new SqlCommand(@"SELECT Id, (SELECT GradeName FROM [ItemGrade] where CategoryID=((SELECT CategoryName FROM [Categories] where CategoryID=) SELECT CategoryID FROM [Products] where ProductID='"+ddItemName.SelectedValue+"')) As Grade,"+
         "((SELECT CategoryName FROM [Categories] where CategoryID=) SELECT CategoryID FROM [Products] where ProductID='"+ddItemName.SelectedValue+"') As Category, "+
         " ,Itemgroup, ItemCode, ItemName, Qty, Price, Total, Manufacturer, CountryOfOrigin, PackSize, Warrenty, SerialNo, UnitType, SizeRef FROM RequisitionDetails WHERE EntryBy=@EntryBy AND InvNo='' ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
         */

        string formtype = Request.QueryString["type"];
        if (formtype == "edit")
        {
            SQLQuery.PopulateGridView(ItemGrid, @"SELECT Id, (Select Purpose from Purpose where pid=RequisitionDetails.Purpose) as Purpose,  (SELECT GradeName FROM [ItemGrade] where GradeID=(SELECT GradeID FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=RequisitionDetails.ItemCode))) As Grade, (SELECT CategoryName FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=RequisitionDetails.ItemCode)) As Category,  Itemgroup, ItemCode, ItemName, Qty, Price, Total, Manufacturer, CountryOfOrigin, PackSize, Warrenty, SerialNo, ModelNo, Specification, UnitType, (SElect spec from Specifications where id=RequisitionDetails.SizeRef) as SizeRef
                                                 FROM RequisitionDetails WHERE  ReqId='" + lblInvoice.Text + "' ORDER BY Id");
            //SQLQuery.PopulateGridView(GridExpenses, "SELECT EID, ExpHeadName, Amount FROM PurchaseExpenses WHERE  InvoiceNo='" + lblInvoice.Text + "' ORDER BY eid");
        }
        else
        {
            SqlCommand cmd2 = new SqlCommand(@"SELECT Id, (Select Purpose from Purpose where pid=RequisitionDetails.Purpose) as Purpose,  (SELECT GradeName FROM [ItemGrade] where GradeID=(SELECT GradeID FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=RequisitionDetails.ItemCode))) As Grade, (SELECT CategoryName FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=RequisitionDetails.ItemCode)) As Category,  Itemgroup, ItemCode, ItemName, Qty, Price, Total, Manufacturer, CountryOfOrigin, PackSize, Warrenty, SerialNo, ModelNo, Specification, UnitType, (SElect spec from Specifications where id=RequisitionDetails.SizeRef)+' '+Specification as SizeRef
                                                 FROM RequisitionDetails WHERE   EntryBy='" + lName + "' AND  ReqId='" + lblInvoice.Text + "' ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
            cmd2.Connection.Open();
            ItemGrid.EmptyDataText = "No items to view...";
            ItemGrid.DataSource = cmd2.ExecuteReader();
            ItemGrid.DataBind();
            cmd2.Connection.Close();

            //SQLQuery.PopulateGridView(GridExpenses, "SELECT EID, ExpHeadName, Amount FROM PurchaseExpenses WHERE  EntryBy='" + lName + "' AND InvoiceNo='" + lblInvoice.Text + "' ORDER BY eid");
            SqlCommand cmd = new SqlCommand("SELECT EID, ExpHeadName, Amount FROM PurchaseExpenses WHERE  EntryBy='" + lName + "' AND InvoiceNo='" + lblInvoice.Text + "' ORDER BY eid", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            GridExpenses.EmptyDataText = "No items to view...";
            GridExpenses.DataSource = cmd.ExecuteReader();
            GridExpenses.DataBind();
            cmd.Connection.Close();
        }
    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //string item = e.Row.Cells[1].Text;
        //foreach (Button button in e.Row.Cells[0].Controls.OfType<Button>())
        //{
        //    if (button.CommandName == "Delete")
        //    {
        //        button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
        //    }
        //}
        //}
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

    protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrp();
        CheckItemType(Convert.ToInt32(ddGroup.SelectedValue));
    }

    private void BindGrp()
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

    private void GetProductList()
    {
        if (ddCategory.SelectedValue != "")
        {
            string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategory.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemName, "ProductID", "ItemName");

            //txtUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddItemName.SelectedValue + "'");
            LoadSpecList("filter");

            if (IsPostBack)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
            }

            CheckItemType(Convert.ToInt32(ddGroup.SelectedValue));
            RecentInfo();
        }
    }
    private void RecentInfo()
    {
        ltrUnitType.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddItemName.SelectedValue + "'");
        ltrUnitTypeLast.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddItemName.SelectedValue + "'");
        //Last Purchase Info
        lblLastDate.Text = RunQuery.SQLQuery.ReturnString("Select convert(varchar,OrderDate,103) FROM Purchase WHERE InvNo=(SELECT InvNo from PurchaseDetails where Id = (Select MAX(Id) from  PurchaseDetails where ItemCode='" + ddItemName.SelectedValue + "'))");
        lblLastQty.Text = RunQuery.SQLQuery.ReturnString("Select Qty from PurchaseDetails where Id = (Select MAX(Id) from  PurchaseDetails where ItemCode='" + ddItemName.SelectedValue + "')");
        lblLastPrice.Text = RunQuery.SQLQuery.ReturnString("Select Price from PurchaseDetails where Id = (Select MAX(Id) from  PurchaseDetails where ItemCode='" + ddItemName.SelectedValue + "')");
        if (ddItemName.SelectedValue != "")
        {
            //string lastSupplier = RunQuery.SQLQuery.ReturnString("Select SupplierName FROM Requisition where Id = (Select ReqId from RequisitionDetails where Id = (Select MAX(Id) from  RequisitionDetails where ItemCode='" + ddItemName.SelectedValue + "'))");
            //string lastInvoice = RunQuery.SQLQuery.ReturnString("Select ReqNo FROM Requisition where Id = (Select ReqId from RequisitionDetails where Id = (Select MAX(Id) from  RequisitionDetails where ItemCode='" + ddItemName.SelectedValue + "'))");
            txtAvailableQty.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(InQuantity),0) - ISNULL(SUM(OutQuantity),0) FROM Stock WHERE ProductID='" + ddItemName.SelectedValue + "' AND (Status = 'Purchase')");
            if (lblLastQty.Text != "")
            {
                ltrLastInfo.Text = "Date: " + lblLastDate.Text + ", Qty.: " + lblLastQty.Text + ", Unit Price.: " + lblLastPrice.Text;
            }
        }
    }
    protected void ddMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadPayMode();
    }

    private void LoadPayMode()
    {
        if (ddMode.SelectedValue == "Cheque")
        {
            chqpanel.Visible = true;
            lblAmt.Text = "Cheque Amount";
            CollField.Attributes.Remove("Class");
            CollField.Attributes.Add("Class", "control-group");
        }
        else if (ddMode.SelectedValue == "Cash")
        {
            chqpanel.Visible = false;
            lblAmt.Text = "Paid Amount";
            txtPaid.Focus();
            CollField.Attributes.Remove("Class");
            CollField.Attributes.Add("Class", "control-group");
        }
        else
        {
            chqpanel.Visible = false;
            CollField.Attributes.Remove("Class");
            CollField.Attributes.Add("Class", "control-group hidden");
        }
    }
    protected void ddGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocation.DataBind();
    }
    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //string ttlQty = RunQuery.SQLQuery.ReturnString("Select SUM(Qty) from RequisitionDetails where InvNo='" + lblInvoice.Text + "' AND EntryBy='" + Page.User.Identity.Name.ToString() + "'");

        //GetTotalAmount();
    }
    protected void ItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;
            TextBox txtQty = ItemGrid.Rows[index].FindControl("txtQty") as TextBox;

            SqlCommand cmd7 = new SqlCommand("DELETE RequisitionDetails WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();

            BindItemGrid();
            btnAdd.Text = "Add to grid";
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "Item deleted from order ...";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }

    }
    protected void ItemGrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(ItemGrid.SelectedIndex);
            Label lblItemName = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;
            if (lblItemName != null)
            {
                lblOrderID.Text = lblItemName.Text;
                EditMode(lblItemName.Text);
            }
            btnAdd.Text = "Update";

            Notify("Edit mode activated ...", "info", lblMsg2);
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "Error: " + ex.ToString();
        }
    }

    private void EditMode(string entryId)
    {
        SqlCommand cmd =new SqlCommand("SELECT ReqId, Itemgroup, ItemCode, ItemName, Qty, Price, Total, Warrenty, SerialNo, UnitType, SizeRef, Manufacturer, CountryOfOrigin, PackSize, EntryBy, pcs, Purpose, ModelNo, Specification, AvailableQty, LastPurDate, LastPurQty, LastPurPrice, Status, EntryBy FROM [RequisitionDetails] WHERE Id='" + entryId + "'",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            //ddSize.SelectedValue = dr[0].ToString();

            string iGrp = dr[1].ToString();
            ddGroup.SelectedValue = iGrp;

            CheckItemType(Convert.ToInt32(iGrp));

            string iCode = dr[2].ToString();
            //txtRate.Text = dr[3].ToString();
            txtQuantity.Text = dr[4].ToString();
            txtRate.Text = dr[5].ToString();
            txtAmt.Text = dr[6].ToString();

            txtWarrenty.Text = dr[7].ToString();
            txtSerial.Text = dr[8].ToString();
            ltrUnitType.Text = dr[9].ToString();
            string spec = dr[10].ToString();


            //ddManufacturer.SelectedValue = dr[11].ToString();
            txtCountry.Text = dr[12].ToString();
            string size = dr[13].ToString();
            if (size != "")
            {
                ddSize.SelectedValue = size;
            }

            txtWeight.Text = dr[15].ToString();

            string purpose = dr[16].ToString();
            if (purpose != "")
            {
                ddPurpose.SelectedValue = purpose;
            }

            txtModel.Text = dr[17].ToString();
            txtSpecification.Text = dr[18].ToString();

            string catId =RunQuery.SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + iCode + "'");
            string grdId =RunQuery.SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catId + "'");
            string subId =RunQuery.SQLQuery.ReturnString("SELECT CategoryID FROM ItemGrade WHERE GradeID ='" + grdId + "'");
            string grpId =RunQuery.SQLQuery.ReturnString("SELECT GroupID FROM ItemSubGroup WHERE CategoryID ='" + subId + "'");


            ddGroup.SelectedValue = grpId;

            string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + grpId +"' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");
            ddSubGrp.SelectedValue = subId;

            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + subId + "' AND ProjectID='" +lblProject.Text + "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
            ddGrade.SelectedValue = grdId;

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + grdId + "' AND ProjectID='" +lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
            ddCategory.SelectedValue = catId;

            gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + catId + "' AND ProjectID='" +lblProject.Text + "' ORDER BY [ItemName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemName, "ProductID", "ItemName");
            ddItemName.SelectedValue = iCode;

            //Load color spec
            if (ddSubGrp.SelectedValue == "10") //Printing Ink
            {
                LoadSpecList("");
                pnlSpec.Visible = true;

                string isExist = SQLQuery.ReturnString("Select spec from Specifications where id='" + spec + "'");
                if (isExist == "")
                {
                    ddSpec.Visible = false;
                    txtSpec.Visible = true;
                    lbSpec.Text = "Cancel";
                    txtSpec.Text = spec;
                }
                else
                {
                    ddSpec.Visible = true;
                    txtSpec.Visible = false;
                    lbSpec.Text = "New";
                    ddSpec.SelectedValue = spec;
                }
            }
            
            txtAvailableQty.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(InQuantity),0) - ISNULL(SUM(OutQuantity),0) FROM Stock WHERE ProductID='" + ddItemName.SelectedValue + "' AND (Status = 'Purchase')");
            RecentInfo();
            //CheckItemType(Convert.ToInt32(iGrp));

        }
        cmd.Connection.Close();
        cmd = new SqlCommand("SELECT SubTotal, ItemDisc, ItemVAT FROM [RequisitionDetails] WHERE Id='" + entryId + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            txtSubTotal.Text = dr[0].ToString();
            txtItemDisc.Text = dr[1].ToString();
            txtItemVAT.Text = dr[2].ToString();
        }
        cmd.Connection.Close();
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
                SectionField.Attributes.Remove("class");
                SectionField.Attributes.Add("class", "control-group hidden");
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
            SectionField.Attributes.Remove("class");
            SectionField.Attributes.Add("class", "control-group");
        }
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
            //lbFilter.Text = "Filter"
        }
        else
        {
            LoadSpecList("filter");
            //lbFilter.Text = "Show-all";
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
    protected void ddSpec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //QtyinStock();
    }

    protected void btnOthExp_OnClick(object sender, EventArgs e)
    {
        try
        {
            string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
            RunQuery.SQLQuery.ExecNonQry("Insert into PurchaseExpenses (InvoiceNo, ExpDate, ExpID, ExpHeadName, Amount, Description, EntryBy) Values ('" + lblInvoice.Text + "', '" +
                            dt + "', '" + ddotherExp.SelectedValue + "', '" + ddotherExp.SelectedItem.Text + "', '" + txtOtherExpenseAmount.Text + "', '', '" +
                            Page.User.Identity.Name.ToString() + "')");

            BindItemGrid();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg2);
        }
    }

    protected void DropDownList2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //ddVendor.DataBind();
    }

    protected void ddInvoice_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridView2.DataBind();

        lblInvoice.Text = ddInvoice.SelectedValue;
        LoadEditMode(ddInvoice.SelectedValue);
    }

    private void LoadEditMode(string inv)
    {
        try
        {
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1) PurchaseFor, OrderDate, SupplySource, BillNo, BillDate, SupplierID, SupplierName, ChallanNo, ChallanDate, ItemTotal, PurchaseDiscount, 
                         VatService, PurchaseTotal, TransportType, WarehouseID, StockLocationID, PaymentMode, PaidAmount, Remarks FROM Requisition WHERE (ReqNo = '" + inv + "')");

            foreach (DataRow drx in dtx.Rows)
            {
                txtFor.Text = drx["PurchaseFor"].ToString();
                //txtOrderDate.Text = SQLQuery.LocalDateFormat(drx["OrderDate"].ToString());// Convert.ToDateTime(drx["OrderDate"].ToString()).ToString("dd/MM/yyyy");
                //ddType.SelectedValue = drx["SupplySource"].ToString();
                txtReqNo.Text = drx["BillNo"].ToString();
                txtDate.Text = SQLQuery.LocalDateFormat(drx["BillDate"].ToString()); //drx["BillDate"].ToString();

                string sid = drx["SupplierID"].ToString();
                string sCategory = SQLQuery.ReturnString("Select Category FROM Party where PartyID='" + sid + "'");
                //ddSuppCategory.SelectedValue = sCategory;
                //ddVendor.DataBind();
                //ddVendor.SelectedValue = sid;

                //txtChallanNo.Text = drx["ChallanNo"].ToString();
                //txtChallanDate.Text = SQLQuery.LocalDateFormat(drx["ChallanDate"].ToString());// drx["ChallanDate"].ToString();

                BindItemGrid();

                txtPDiscount.Text = drx["PurchaseDiscount"].ToString();
                txtVat.Text = drx["VatService"].ToString();

                //

                lblItemAmount.Text = drx["PurchaseTotal"].ToString();
                txtTransportType.Text = drx["TransportType"].ToString();
                ddGodown.SelectedValue = drx["WarehouseID"].ToString();
                string loc = drx["StockLocationID"].ToString();
                if (loc != "")
                {
                    ddLocation.SelectedValue = loc;
                }
                ddMode.SelectedValue = drx["PaymentMode"].ToString();
                LoadPayMode();
                txtPaid.Text = drx["PaidAmount"].ToString();
                txtRemark.Text = drx["Remarks"].ToString();

                string bank = SQLQuery.ReturnString("Select ACID from BankAccounts where BankID=(Select ChqBank from Cheque where TrID='" + inv + "')");
                if (bank != "")
                {
                    ddBank.SelectedValue = bank;
                    txtDetail.Text = SQLQuery.ReturnString("Select ChequeNo from Cheque where TrID='" + inv + "'");
                    txtChqDate.Text = SQLQuery.ReturnString("Select ChqDate from Cheque where TrID='" + inv + "'");
                }


            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg2);
        }
    }

    protected void GridExpenses_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridExpenses.Rows[index].FindControl("lblEntryId") as Label;

            SqlCommand cmd7 = new SqlCommand("DELETE PurchaseExpenses WHERE eid=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();


            BindItemGrid();
            btnAdd.Text = "Add to grid";
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "Item deleted from order ...";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }

    }

    protected void GridExpenses_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}
