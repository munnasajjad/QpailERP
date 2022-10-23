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

public partial class app_LC_Items : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //btnAdd.Attributes.Add("onclick"," this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToShortDateString();
            ddLCNo.DataBind();
            lblProject.Text = "1";
            BindItemGrid();
            ddPurpose.DataBind();
            ddPurpose.SelectedValue = "1";
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

            GetProductList();
            BindItemGrid();
            LoadInfo();


            string formtype = Request.QueryString["type"];
            if (formtype == "edit")
            {
                string invId = Request.QueryString["inv"];
                if (invId != null)
                {
                    //ddInvoice.SelectedValue = invId;
                }

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

    private void Status()
    {
        //if (cbStatus.Checked)
        //{
        //    ddGodown.Visible = true;
        //}
        //else
        //{
        //    ddGodown.Visible = false;
        //}
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            string query = "";
            if (txtLCNo.Text != "")
            {

                decimal ttl =
                    Convert.ToDecimal(
                        SQLQuery.ReturnString("Select ISNULL(SUM(TotalBDT),0) FROM LcItems  WHERE LCNo='" +
                                              ddLCNo.SelectedValue + "' "));
                if (ttl > 0)
                {
                    InsertLCNo();
                    saveLCItems();

                    Session["EditLCNo"] = "";

                    //btnSave.Text = "Save";
                    //txtEditVoucherNo.Text = "";
                    BindItemGrid();

                    ddLCNo.Visible = true;
                    txtLCNo.Visible = false;
                    //lbLCNo.Text = "New";
                    ddLCNo.Focus();

                    LoadInfo();
                    GridView1.DataBind();
                    ///txtTTL.Text = "0";

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "LC Items Saved Successfully.";
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "ERROR: LC number can not be empty!";
                }
            }else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "ERROR: No data was found into current entry!";
                }
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

    private void InsertLCNo()
    {
        string lName = Page.User.Identity.Name.ToString();
        string isExist = SQLQuery.ReturnString("Select LCNo from LC_Master WHERE LCHeadID='" + ddLCNo.SelectedValue + "'");
        //SQLQuery.ExecNonQry("Delete LC_Master where LCHeadID='" + ddLCNo.SelectedValue + "'");
        if (isExist == "")
        {
            SQLQuery.ExecNonQry("INSERT INTO LC_Master (LCHeadID, LCNo, ReceiveDate, Qty, Unit, ExpAmt, InventoryHeadID, StockInGodown, EntryBy)" +
                                   "VALUES ('" + ddLCNo.SelectedValue + "', '" + txtLCNo.Text + "', '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + txtLCQty.Text + "', 'Kg', '" + txtTTL.Text + "', '', '" + ddGodown.SelectedValue + "', '" + lName + "')");

        }
        else
        {
            SQLQuery.ExecNonQry(@"UPDATE  LC_Master SET LCNo='" + txtLCNo.Text + "', ReceiveDate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', Qty='" + txtLCQty.Text + "', Unit='Kg', ExpAmt='" + txtTTL.Text + "', InventoryHeadID='', StockInGodown='" + ddGodown.SelectedValue + "' WHERE LCHeadID='" + ddLCNo.SelectedValue + "'");
        }

        string orderNo = "LC" + ddLCNo.SelectedValue;
        /*
        //Update voucher first
        VoucherEntry.AutoVoucherEntry("11", ddInventoriesHead.SelectedValue, ddLCNo.SelectedValue, Convert.ToDecimal(txtTTL.Text), orderNo, lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "0");
        //New voucher entry
        string voucherNo = "Auto-" + DateTime.Now.Year.ToString() + "-" + RunQuery.SQLQuery.ReturnString("Select ISNULL(MAX(VID),0)+1001 From VoucherMaster");
        string rate = Convert.ToString(Convert.ToDecimal(txtTTL.Text) / Convert.ToDecimal(txtLCQty.Text));
        //VoucherEntry.AutoVoucherEntry("11", ddInventoriesHead.SelectedValue, ddLCNo.SelectedValue, Convert.ToDecimal(txtTTL.Text), orderNo, lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "1");
        VoucherEntry.InsertVoucherWithQty(voucherNo, ddLCNo.SelectedItem.Text, "6", ddInventoriesHead.SelectedValue, ddLCNo.SelectedValue, Convert.ToDecimal(txtTTL.Text), lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), orderNo, txtLCQty.Text, rate);
        */
    }

    private void saveLCItems()
    {
        string refNo = ddLCNo.SelectedValue;
            SQLQuery.ExecNonQry("Delete Stock WHERE InvoiceID='" + refNo + "' AND EntryType='LC Item Received'");
            SQLQuery.ExecNonQry("Delete Stockin WHERE OrderID='" + refNo + "' AND Remarks='LC Item Received'");
            SQLQuery.ExecNonQry("Insert into Stockin (OrderID, OrderDate, GodownID, GodownName, LocationName, Remarks, TotalQty, EntryBy)" +
                                " VALUES ('" + refNo + "','"+Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") +"','"+ddGodown.SelectedValue+ "','" + ddGodown.SelectedItem.Text + "', '', 'LC Item Received','"+txtTTL.Text+"','"+User.Identity.Name+"')");

                string voucherAmt = txtTTL.Text;// SQLQuery.ReturnString("Select ISNULL(SUM(VoucherAmount),0) FROM VoucherMaster WHERE VoucherReferenceNo='" + refNo + "'");
                string iTtl = SQLQuery.ReturnString("Select ISNULL(SUM(UnitPrice),0) FROM LcItems WHERE LCNo='" + refNo + "'");

                //insert stock qty
                DataTable dtx1 = SQLQuery.ReturnDataTable(@"SELECT EntryID, LCNo, Purpose, GradeId, CategoryId, ItemCode, HSCode, ItemSizeID, pcs, NoOfPacks, QntyPerPack, Spec, Thickness, Measurement, qty, 
                                                                UnitPrice, CFRValue, EntryBy, ReturnQty, Loading, Loaded, LandingPercent, LandingAmt, TotalUSD, TotalBDT, UnitCostBDT, FullDescription
                                                                FROM [LcItems] WHERE LCNo='" + refNo + "'");
                int pcs = 0;
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
                    decimal price = Convert.ToDecimal(TotalBDT) / Convert.ToDecimal(qty);
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

            decimal uPrice = Convert.ToDecimal(UnitPrice) * Convert.ToDecimal(voucherAmt) / Convert.ToDecimal(iTtl);

            if (SQLQuery.ReturnString("Select UnitType from Products where ProductID='" + ItemCode + "'") == "PCS")
                    {
                        pcs = Convert.ToInt32(Convert.ToDecimal(qty));
                        qty = "0";
                uPrice = uPrice / Convert.ToDecimal(pcs);
            }
            else
            {
                uPrice = uPrice / Convert.ToDecimal(qty);
            }


            SQLQuery.ExecNonQry("Update LcItems SET UnitCostBDT='"+ uPrice + "', ReceiveDate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' WHERE EntryID='" + EntryID + "' ");

            Inventory.SaveToStock(Purpose, refNo, "LC Item Received", EntryID, ItemSizeID, "", "", "",
                        Spec, ItemCode, Inventory.GetProductName(ItemCode), "", ddGodown.SelectedValue, "",
                        itemGroup, Convert.ToDecimal(pcs), 0, uPrice, Convert.ToDecimal(qty), 0,
                        detail, "", "LC", "", User.Identity.Name, DateTime.Now.ToString("yyyy-MM-dd"), Thickness);

                    Notify("Items inserted into stock.", "success", lblMsg);

                    GridView1.DataBind();
                    GridView1.Visible = true;

        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        //PopulateSubAcc();
    }

    protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {/*
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

            cmd7.Connection.Close();*/
            //pan.Update();

        }
        catch (Exception ex)
        {
            lblMsg.CssClass = "xerp_error";
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "ERROR: " + ex.ToString();

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
        //GridView2.EmptyDataText = "No data added ...";
        //GridView2.DataSource = cmd.ExecuteReader();
        //GridView2.DataBind();
        cmd.Connection.Close();

        LoadInfo();
    }

    //private void BindItemGrid4Edit()
    //{
    //    SqlCommand cmd =
    //        new SqlCommand(
    //            @"SELECT [SerialNo], [VoucherRowDescription], AccountsHeadDr, AccountsHeadDrName+ '<br/>'+ Name5Dr AS AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName+ '<br/>'+ Name5Cr AS AccountsHeadCrName, Amount 
    //                                        FROM[VoucherTmp] WHERE VoucherNo ='" + txtEditVoucherNo.Text +
    //            "' ORDER BY [SerialNo]",
    //            new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
    //    cmd.Connection.Open();
    //    //GridView2.EmptyDataText = "No data added ...";
    //    //GridView2.DataSource = cmd.ExecuteReader();
    //    //GridView2.DataBind();
    //    cmd.Connection.Close();

    //    txtTTL.Text =
    //        SQLQuery.ReturnString("Select ISNULL(SUM(VoucherDR),0)-ISNULL(SUM(VoucherCR),0) FROM VoucherDetails where AccountsHeadID='" +
    //                              ddLCNo.SelectedValue + "' ");
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
        AddToGrid();
    }

    private void AddToGrid()
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

            SQLQuery.Empty2Zero(txtQuantity);

            if (ddItemName.SelectedValue != "")
            {
                string lcNo = ddLCNo.SelectedValue;
                //if (lbLCNo.Text == "Old")
                //{
                //    lcNo = "";
                //}
                SQLQuery.Empty2Zero(txtShortage);
                SqlCommand cmd2 = new SqlCommand("INSERT INTO LcItems (LCNo, LCType, Purpose, GradeId, CategoryId, ItemCode, HSCode, Thickness, Measurement, NoOfPacks, QntyPerPack,  qty, UnitPrice, TotalBDT, Loading, LandingPercent, pcs, EntryBy, FullDescription, Manufacturer, CountryOfOrigin, ShortageQty)" +
                                                             " VALUES (@LCNo, 'LC', '" + ddPurpose.SelectedValue + "', '" + ddGrade.SelectedValue + "', '" + ddCategory.SelectedValue + "', @ItemCode, @HSCode, '" + txtSpecification.Text + "', '" + txtModel.Text + "', '" + txtSerial.Text + "','" + txtWarrenty.Text +
                                                                            "', @qty, @UnitPrice, @TotalBDT, @Loading,  @LandingPercent,  '" + txtWeight.Text + "',  @EntryBy, '" + ddLCNo.SelectedItem.Text + "','" + ddManufacturer.SelectedValue + "','" + txtCountry.Text + "','" + txtShortage.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.AddWithValue("@LCNo", lcNo);
                cmd2.Parameters.AddWithValue("@ItemCode", ddItemName.SelectedValue);
                cmd2.Parameters.AddWithValue("@HSCode", txtHSCode.Text);
                cmd2.Parameters.AddWithValue("@NoOfPacks", txtSpecification.Text);
                cmd2.Parameters.AddWithValue("@ItemSizeID", size);
                cmd2.Parameters.AddWithValue("@Measurement", measure);

                cmd2.Parameters.AddWithValue("@Thickness", txtSerial.Text);
                cmd2.Parameters.AddWithValue("@QntyPerPack", txtWarrenty.Text);
                cmd2.Parameters.AddWithValue("@qty", Convert.ToDecimal(txtQuantity.Text));

                decimal price = 0;
                if (Convert.ToDecimal(txtQuantity.Text)!=0)
                {
                    price= Convert.ToDecimal(txtItemAmt.Text) / Convert.ToDecimal(txtQuantity.Text);
                }

                cmd2.Parameters.AddWithValue("@UnitPrice", price);
                cmd2.Parameters.AddWithValue("@TotalBDT", Convert.ToDecimal(txtItemAmt.Text));

                cmd2.Parameters.AddWithValue("@Loading", "1");
                cmd2.Parameters.AddWithValue("@LandingPercent", "1.01");
                cmd2.Parameters.AddWithValue("@EntryBy", lName);

                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();

                string item = ddItemName.SelectedItem.Text;
                //Save_LC_Log("New item inserted: ", item, "Qnty.: " + txtQty.Text + ", Unit Price: " + txtPrice.Text + ", CFR: " + txtCFR.Text);

                //ItemGrid.DataBind();

                //SQLQuery.ExecNonQry(@"UPDATE  LcItems SET LCNo='" + ddLCNo.SelectedValue + "' WHERE LCNo='' AND EntryBy='" + lName + "'");

                BindItemGrid();

                //txtQuantity.Text = "";
                //txtRate.Text = "";
                //txtThickness.Text = "";
                //txtMeasure.Text = "";
                //txtCFR.Text = "";
                ddGroup.Focus();
            }
            else
            {
                Notify("ERROR: Invalid Product Name!", "warn", lblMsg2);
            }


            txtSerial.Text = "";
            //txtRate.Text = "";
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
                //txtRate.Text = dr[6].ToString();
                txtWarrenty.Text = dr[7].ToString();
                txtWeight.Text = dr[8].ToString();
                ddManufacturer.SelectedValue = dr[9].ToString();
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
                //txtRate.Text = drx["UnitPrice"].ToString();
                ddManufacturer.SelectedValue = drx["Manufacturer"].ToString();
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
    private void LoadInfo()
    {
        try {
        GridView1.DataBind();

        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) LCHeadID, LCNo, ReceiveDate, Qty, Unit, ExpAmt, InventoryHeadID, StockInGodown, EntryBy FROM   LC_Master WHERE (LCHeadID = '" + ddLCNo.SelectedValue + "')");
        foreach (DataRow drx in dtx.Rows)
        {
            txtLCNo.Text = drx["LCNo"].ToString();
            txtDate.Text = Convert.ToDateTime(drx["ReceiveDate"].ToString()).ToString("dd/MM/yyyy");
            txtLCQty.Text = drx["Qty"].ToString();
            //ddUnit.SelectedValue = drx["Unit"].ToString();

            txtTTL.Text = drx["ExpAmt"].ToString();
            ddGodown.SelectedValue = drx["StockInGodown"].ToString();
            //ddInventoriesHead.SelectedValue = drx["InventoryHeadID"].ToString();

        }

        if (dtx.Rows.Count==0)
        {
            txtLCNo.Text = "";
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtLCQty.Text = "";

            txtTTL.Text =
                SQLQuery.ReturnString("Select ISNULL(SUM(VoucherDR),0)-ISNULL(SUM(VoucherCR),0) FROM VoucherDetails where AccountsHeadID='" +
                                      ddLCNo.SelectedValue + "' AND ISApproved<>'C' ");

        }


        SQLQuery.PopulateGridView(ItemGrid, @"SELECT EntryID, 
                                                (Select Purpose from Purpose where pid=a.Purpose) as Purpose, 
                                                (Select GradeName from ItemGrade where GradeID=a.GradeId) as Grade, 
                                                (Select CategoryName from Categories where CategoryID=a.CategoryId) as Category, 
                                                (Select ItemName from Products where ProductID=a.ItemCode) as Product,                      
                                                [Thickness], (Select BrandName from Brands where BrandID=a.ItemSizeID) as Size, Measurement, (Select spec from Specifications where id=a.spec) as spec,
                                                qty, (Select UnitType from Products where ProductID=a.ItemCode) As Unit, UnitPrice,  [CFRValue] , TotalBDT
                                                FROM [LcItems] a Where  LCNo='" + ddLCNo.SelectedValue + "' ORDER BY [EntryID]");
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }

    protected void ddLCNo_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        LoadInfo();

    }

    protected void lbLCNo_OnClick(object sender, EventArgs e)
    {

    }

    protected void cbStatus_OnCheckedChanged(object sender, EventArgs e)
    {
        Status();
    }
}