using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using RunQuery;
using Stock;


public partial class app_Plastic_Finishing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Today.Date.ToShortDateString();
            string lName = Page.User.Identity.Name.ToString();
            lblProject.Text = RunQuery.SQLQuery.ReturnString("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'");

            string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where CategoryID='13' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGradeRaw, "GradeID", "GradeName");

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");

            //Processed Item
            gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='3' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutSubGroup, "CategoryID", "CategoryName");

            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='"+ddOutSubGroup.SelectedValue+"' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

            ddCustomer.DataBind();
            ddBrand.DataBind();

            GetProductList();
            GridView2.DataBind();
        }
    }
    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox)
        { (Parent as TextBox).Text = string.Empty; }
        else
        {
            foreach (Control c in Parent.Controls)
                ClearControls(c);
        }
    }

    //Message or Notify For Alerts
    private void Notify(string msg, string type)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblMsg.Attributes.Add("class", "xerp_" + type);
        lblMsg.Text = msg;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text == "Save")
            {
                SaveProduction();
                UpdateProduction();
                GridView1.DataBind();
                //lblMsg.Attributes.Add("class", "xerp_success");
                //lblMsg.Text = "Production saved successfully";
                ClearMasterArea();
                Notify("Production saved successfully", "success");
            }
            else
            {
                UpdateProduction();
                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Entry updated successfully";
            }

        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "Error: " + ex.ToString();
            Notify(ex.Message.ToString(), "error");
        }
        finally
        {

        }
    }

    private void SaveProduction()
    {

        string lName = Page.User.Identity.Name.ToString();
        string prdId = RunQuery.SQLQuery.ReturnString("Select 'PRD-PF-'+ CONVERT(varchar, (ISNULL (max(pid),0)+1001 )) from PrdnPlasticFinish");
        lblPrId.Text = prdId;

        SqlCommand cmd2 = new SqlCommand("INSERT INTO PrdnPlasticFinish (ProductionID, EntryBy) VALUES (@ProductionID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductionID", prdId);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

    }

    private void UpdateProduction()
    {
        RunQuery.SQLQuery.ExecNonQry(@"Update PrdnPlasticFinish SET Date='" +
                                     Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") +
                                     "', MachineNo='" + ddMachine.SelectedValue +
                                     "', LineNumber='" + ddLine.SelectedValue + "', Shift='" + ddShift.SelectedValue + "', Operator='" + ddOperator.SelectedValue + "', Purpose='" + ddPurpose.SelectedValue + "', CustomerID='" + ddCustomer.SelectedValue +
                                     "', Brand='" + ddBrand.SelectedValue + "', PackSize='" +
                                     ddSize.SelectedValue + "', ItemSubGroup='" + ddSubGrp.SelectedValue + "', ItemGrade='" + ddGradeRaw.SelectedValue + "', ItemCategory='" + ddCategoryRaw.SelectedValue + "',  ItemName='" +
                                     ddItemNameRaw.SelectedValue + "', FinalOutput='" + txtFinalQty.Text + "', WeightPerPc='" + txtItemWeight.Text + "', OutputWeight='" +
                                     txtFinalWeight.Text + "', WorkingHr='" + txtHour.Text + "', TimeWaist='" + txtTimeWaist.Text + "', Reason='" + txtReason.Text + "', Remarks ='" +
                                     txtRemark.Text + "' WHERE ProductionID='" + lblPrId.Text + "' ");

        //Item Stock Entry
        //RunQuery.SQLQuery.ExecNonQry("DELETE Stock WHERE InvoiceID='" + lblPrId.Text + "'");
        RunQuery.SQLQuery.ExecNonQry("UPDATE PrdnPlasticFinishDetails SET PrdnID='" + lblPrId.Text + "' WHERE PrdnID=''");



        //Input Item stock-out
        DataTable citydt = RunQuery.SQLQuery.ReturnDataTable(@"Select Company, Brand, Packsize, Item, Color, WeightPerPc, InputQty, InputWeight, Production, Rejected, 
                         NetProduction, ReusableWasteItem, ReusableWasteQty, ReusableWasteWeight from PrdnPlasticFinishDetails where PrdnID='" + lblPrId.Text + "'");

        foreach (DataRow citydr in citydt.Rows)
        {
            string Purpose = ddPurpose.SelectedValue;
            string Company = citydr["Company"].ToString();
            string Brand = citydr["Brand"].ToString();
            string PackSize = citydr["Packsize"].ToString();
            string Item = citydr["Item"].ToString();
            string Color = citydr["Color"].ToString();
            string WeightPerPc = citydr["WeightPerPc"].ToString();

            string InputQty = citydr["InputQty"].ToString();
            string InputWeight = citydr["InputWeight"].ToString();
            string Production = citydr["Production"].ToString();
            string Rejected = citydr["Rejected"].ToString();
            string NetProduction = citydr["NetProduction"].ToString();
            string ReusableWasteItem = citydr["ReusableWasteItem"].ToString();

            string ReusableWasteQty = citydr["ReusableWasteQty"].ToString();
            string ReusableWasteWeight = citydr["ReusableWasteWeight"].ToString();

            decimal price = 0;// Inventory.LastNonprintedPrice(Purpose, itemType, Item);
            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- Plastic Finishing", lblPrId.Text, PackSize,
                Company, Brand, Color, "", Item, Stock.Inventory.GetProductName(Item), "", "8",
                "", "3", 0, Convert.ToInt32(InputQty), 0, 0, Convert.ToDecimal(InputWeight), "", "Stock-Out",
                "Processed used", "Plastic Finishing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            //Stock In

            //decimal reUseKg = Convert.ToDecimal(InputWeight) - Convert.ToDecimal(ReusableWasteItem);
            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- Plastic Finishing", lblPrId.Text, "",
                ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ReusableWasteItem,
                Stock.Inventory.GetProductName(ReusableWasteItem), "", "8", "", "7", Convert.ToInt32(ReusableWasteQty), 0, 0,
                Convert.ToDecimal(ReusableWasteWeight), 0, "", "Stock-In", "Wastage Item", "Round Tin",
                Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        }


        string nonUsableWeight = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(NonusableWaste),0) from PrdnRoundTinDetails where PrdnID='" + lblPrId.Text + "'");

        Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Plastic Finishing", lblPrId.Text,
            "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", "1808", "Nonusable Westage", "", "8", "",
            "7", 0, 0, 0, Convert.ToDecimal(nonUsableWeight), 0, "", "Stock-in", "Production", "Plastic Finishing",
            Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));


        //Output Stock-in
        Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Plastic Finishing", lblPrId.Text, ddSize.SelectedValue,
            ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ddItemNameRaw.SelectedValue,
            ddItemNameRaw.SelectedItem.Text, "", "4", "3", "2", Convert.ToInt32(txtFinalQty.Text), 0, 0,
            Convert.ToDecimal(txtFinalWeight.Text), 0, "", "Stock-in", "Production", "Plastic Finishing",
            Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        StockDetails();
    }

    protected void ddOutItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }
    protected void ddSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddOperator.DataBind();
    }

    protected void ddGradeRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");

        GetProductList();
    }
    protected void ddCategoryRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddOutCategory.SelectedValue = ddCategoryRaw.SelectedValue;
        GetProductList();

    }
    protected void ddItemNameRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadFinishedDD();
    }

    private void LoadFinishedDD()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddOutCategory.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
        StockDetails();
    }

    private void GetProductList()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategoryRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemNameRaw, "ProductID", "ItemName");

        gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddOutCategory.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");

        StockDetails();
    }

    private void StockDetails()
    {
        decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableProcessedQty(ddPurpose.SelectedValue, "", ddOutItem.SelectedValue,
                    ddCompany2.SelectedValue, ddBrand2.SelectedValue, ddInputPack.SelectedValue, ddColor.SelectedValue, "8", "4"));
        decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableProcessedWeight(ddPurpose.SelectedValue, "", ddOutItem.SelectedValue,
                    ddCompany2.SelectedValue, ddBrand2.SelectedValue, ddInputPack.SelectedValue, ddColor.SelectedValue, "8", "4"));

        ltrCurrentStock.Text = "Available Stock: " + inputAvailable + " PCS, " + inputAvailableKg + "KG";

        //ltrReUsable.Text = "Available Stock: " + Stock.Inventory.AvailableNonPrintedQty("", "", ddReusableUsed.SelectedValue, "8") + " pc. " + Stock.Inventory.AvailableNonPrintedWeight("", "", ddReusableUsed.SelectedValue, "8") + " kg";
        //ltrColorStock.Text = "Available Stock: " + Stock.Inventory.AvailableInkWeight(ddColor.SelectedValue, ddSpec.SelectedValue, "8") + " kg";

    }
    protected void ddOutCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddOutCategory.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
        StockDetails();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            RunQuery.SQLQuery.Empty2Zero(txtRejected);
            string lName = Page.User.Identity.Name.ToString();
            if (txtInputQtyKG.Text != "" && txtNetPrdn.Text != "" && txtproduced.Text != "")
            {
                decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableProcessedQty(ddPurpose.SelectedValue, "", ddOutItem.SelectedValue,
                    ddCompany2.SelectedValue, ddBrand2.SelectedValue, ddInputPack.SelectedValue, ddColor.SelectedValue, "8", "4"));
                decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableProcessedWeight(ddPurpose.SelectedValue, "", ddOutItem.SelectedValue,
                            ddCompany2.SelectedValue, ddBrand2.SelectedValue, ddInputPack.SelectedValue, ddColor.SelectedValue, "8", "4"));

                string prdName = ddOutItem.SelectedItem.Text.ToLower();
                if (((inputAvailable >= Convert.ToDecimal(txtInputQtyPCS.Text) && inputAvailableKg >= Convert.ToDecimal(txtInputQtyKG.Text)) || prdName.IndexOf("lid")>-1)|| Stock.Inventory.StockEnabled() == "0")
                {
                    if (btnAdd.Text == "Add")
                    {
                        InsertDetailData();
                        txtFinalQty.Text = txtNetPrdn.Text;

                        lblMsg2.Attributes.Add("class", "xerp_success");
                        lblMsg2.Text = "New product added successfully";
                        Notify("New product added successfully", "success");
                    }
                    else
                    {
                        //RunQuery.SQLQuery.ExecNonQry("Delete PrdnPlasticFinishDetails where id='" + lblEid.Text + "'");
                        UpdateDetailData(lblEid.Text);
                        //Accounting.VoucherEntry.ProductionItemEntry("", "Shearing", ddPurpose.SelectedValue, ddSize.SelectedValue, "", ddOutItem.SelectedValue, ddOutItem.SelectedItem.Text, txtSheetQty.Text, txtproduced.Text, txtPackPerSheet.Text, txtSheetQty.Text, txtRejected.Text, Page.User.Identity.Name.ToString());
                        btnAdd.Text = "Add";
                        lblMsg2.Attributes.Add("class", "xerp_success");
                        lblMsg2.Text = "Entry updated successfully";
                    }

                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Error: Input quantity is not available into stock!";
                }
            }
            else
            {
                lblMsg2.Attributes.Add("class", "xerp_error");
                lblMsg2.Text = "Please fillup all mendatory fields...";
            }


            //lblMsg2.Attributes.Add("class", "xerp_error");
            //lblMsg2.Text = "Error: Input weight & output weight must have to be equel!";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "Error: " + ex.ToString();
            Notify(ex.Message.ToString(), "error");
        }
        finally
        {
            txtFinalQty.Text = RunQuery.SQLQuery.ReturnString("Select NetProduction from PrdnPlasticFinishDetails where id=(Select MAX(ID) from PrdnPlasticFinishDetails)");
            GridView2.DataBind();
            ddOutSubGroup.Focus();
        }

    }

    private void InsertDetailData()
    {
        SqlCommand cmd3 = new SqlCommand(@"INSERT INTO PrdnPlasticFinishDetails (Section, PrdnID, Company, Brand, Packsize, Subgroup, Grade, Category, Item, Color, WeightPerPc, InputQty, InputWeight, Production, Rejected, NetProduction, ReusableWasteItem, ReusableWasteQty, ReusableWasteWeight, EntryBy)" +
                                                        " VALUES ('Plastic Finishing', '',  '" + ddCompany2.SelectedValue + "', '" + ddBrand2.SelectedValue + "', '" + ddInputPack.SelectedValue + "', '" + ddOutSubGroup.SelectedValue + "', '" + ddOutGrade.SelectedValue + "', '" + ddOutCategory.SelectedValue + "', '" + ddOutItem.SelectedValue + "', '" + ddColor.SelectedValue + "', '" + txtWeightPc.Text + "', '" + txtInputQtyPCS.Text + "', '" + txtInputQtyKG.Text + "', '" + txtproduced.Text + "', '" + txtRejected.Text + "', '" + txtNetPrdn.Text + "', '" + ddReusableWaste.SelectedValue + "', '" + txtReusableWasteQty.Text + "', '" + "0" + "', @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd3.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        cmd3.Connection.Open();
        cmd3.ExecuteNonQuery();
        cmd3.Connection.Close();

        ddOutItem.Items.Remove(ddOutItem.SelectedItem.Text);
        txtInputQtyPCS.Text = txtNetPrdn.Text;
        txtFinalQty.Text = txtNetPrdn.Text;

        txtItemWeight.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(WeightPerPc),0) from PrdnPlasticFinishDetails where PrdnID=''");
        txtFinalWeight.Text = Convert.ToString(Convert.ToDecimal(txtFinalQty.Text) * Convert.ToDecimal(txtItemWeight.Text) / 1000M);

        ClearDetailArea();
    }
    private void UpdateDetailData(string entryID)
    {
        RunQuery.SQLQuery.ExecNonQry(@"UPDATE [dbo].[PrdnPlasticFinishDetails]
   SET 
      [Company] = '" + ddCompany2.SelectedValue + "' ,[Brand] = '" + ddBrand2.SelectedValue + "'" +
                                     ",[Packsize] = '" + ddInputPack.SelectedValue + "' , [Subgroup] = '" +
                                     ddOutSubGroup.SelectedValue + "',[Grade] = '" + ddOutGrade.SelectedValue +
                                     "',[Category] = '" + ddOutCategory.SelectedValue + "',[Item] = '" +
                                     ddOutItem.SelectedValue + "',[Color] ='" + ddColor.SelectedValue +
                                     "',[WeightPerPc] = '" + txtWeightPc.Text + "',[InputQty] = '" + txtInputQtyPCS.Text +
                                     "',[InputWeight] = '" + txtInputQtyKG.Text + "',[Production] = '" +
                                     txtproduced.Text + "',[Rejected] = '" + txtRejected.Text + "',[NetProduction] = '" +
                                     txtNetPrdn.Text + "',[ReusableWasteItem] = '" + ddReusableWaste.SelectedValue +
                                     "',[ReusableWasteQty] = '" + txtReusableWasteQty.Text +
                                     "',[ReusableWasteWeight] ='" + "0" + "' WHERE id='" + lblEid.Text + "'");
        ddOutItem.Items.Remove(ddOutItem.SelectedItem.Text);
        txtInputQtyPCS.Text = txtNetPrdn.Text;
        txtFinalQty.Text = txtNetPrdn.Text;

        txtItemWeight.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(WeightPerPc),0) from PrdnPlasticFinishDetails where PrdnID=''");
        txtFinalWeight.Text = Convert.ToString(Convert.ToDecimal(txtFinalQty.Text) * Convert.ToDecimal(txtItemWeight.Text) / 1000M);

        ClearDetailArea();
    }
    private void ClearDetailArea()
    {
        txtWeightPc.Text = "";
        //txtInputQtyPCS.Text = "";
        txtInputQtyKG.Text = "";
        txtproduced.Text = "";
        txtRejected.Text = "";
        txtNetPrdn.Text = "";
        GridView2.DataBind();
    }
    private void ClearMasterArea()
    {
        ClearDetailArea();
        txtFinalQty.Text = "";
        txtItemWeight.Text = "";
        txtHour.Text = "";
        txtTimeWaist.Text = "";
        txtReason.Text = "";
        txtFinalWeight.Text = "";
        txtRemark.Text = "";
        GridView1.DataBind();
    }


    protected void GridView2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //DropDownList1.DataBind();
            int index = Convert.ToInt32(GridView2.SelectedIndex);
            Label lblItemName = GridView2.Rows[index].FindControl("Label1") as Label;
            lblEid.Text = lblItemName.Text;
            EditMode(lblItemName.Text);
            btnAdd.Text = "Update";
            StockDetails();
            lblMsg2.Attributes.Add("class", "xerp_info");
            lblMsg2.Text = "Edit mode activated ...";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    private void EditMode(string entryID)
    {
        SqlCommand cmd = new SqlCommand("SELECT  Subgroup, Grade, Category, Item, Color, WeightPerPc, InputQty, InputWeight, Production, Rejected, NetProduction FROM [PrdnPlasticFinishDetails] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            ddOutSubGroup.SelectedValue = dr[0].ToString();
            string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

            ddOutGrade.SelectedValue = dr[1].ToString();
            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");

            ddOutCategory.SelectedValue = dr[2].ToString();

            LoadFinishedDD();
            ddOutItem.SelectedValue = dr[3].ToString();

            ddColor.SelectedValue = dr[4].ToString();
            txtWeightPc.Text = dr[5].ToString();
            txtInputQtyPCS.Text = dr[6].ToString();

            txtInputQtyKG.Text = dr[7].ToString();
            txtproduced.Text = dr[8].ToString();
            txtRejected.Text = dr[9].ToString();
            txtNetPrdn.Text = dr[10].ToString();
        }
        cmd.Connection.Close();


        //txtRingWaste
        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT Company, Brand, Packsize, ReusableWasteItem FROM [PrdnPlasticFinishDetails] WHERE Id='" + entryID + "'");

        foreach (DataRow drx in dtx.Rows)
        {
            ddCompany2.SelectedValue = drx["Company"].ToString();
            ddBrand2.DataBind();
            ddBrand2.SelectedValue = drx["Brand"].ToString();
            ddInputPack.SelectedValue = drx["Packsize"].ToString();
            ddReusableWaste.SelectedValue = drx["ReusableWasteItem"].ToString();
        }

        StockDetails();
    }

    protected void GridView2_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView2.Rows[index].FindControl("Label1") as Label;

            SqlCommand cmd7 = new SqlCommand("DELETE PrdnPlasticFinishDetails WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();

            GridView2.DataBind();

            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "Entry deleted successfully ...";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    protected void GridView2_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        CalcNonusableWeight();

        //string ttlWeight =
        //        RunQuery.SQLQuery.ReturnString(
        //            "Select SUM(TotalWeight)+SUM(WaistWeight) from ProductionDetails where PrdnID='' AND Department='Shearing'");

        //if (Convert.ToDecimal(ttlWeight) > 0)
        //{
        //    lblTotalWeight.Text = "<b>Total Output Weight: </b>" + ttlWeight + " kg.";
        //}

        //txtFinalProd.Text = RunQuery.SQLQuery.ReturnString(
        //            "Select SUM(PrdnPcs) from ProductionDetails where PrdnID='' AND Department='Shearing'");



    }

    protected void ddCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand.Items.Clear();
        ListItem lst = new ListItem("--- all ---", "");
        ddBrand.Items.Insert(ddBrand.Items.Count, lst);
        //ddBrand.Items.Add("--- all ---");
        ddBrand.DataBind();
    }


    protected void ddOutGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");
    }

    protected void ddOutCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        GetProductList();

    }
    protected void ddOutItem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }


    protected void ddCompany_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //ddCompany2.Items.Clear();
        //ListItem lst = new ListItem("--- all ---", "");
        //ddBrand2.Items.Insert(ddBrand2.Items.Count, lst);
        ////ddBrand.Items.Add("--- all ---");
        //ddBrand2.DataBind();
        //StockDetails();
    }
    private void CalcNonusableWeight()
    {
        DataTable citydt = RunQuery.SQLQuery.ReturnDataTable("Select Id, WeightPerPc, Rejected, ReusableWasteQty from PrdnPlasticFinishDetails where PrdnID='' Order by id");
        int i = 1;

        foreach (DataRow citydr in citydt.Rows)
        {
            string id = citydr["Id"].ToString();
            string weightPerPc = citydr["WeightPerPc"].ToString();
            string rejected = citydr["Rejected"].ToString();
            string reusableWasteQty = citydr["ReusableWasteQty"].ToString();

            decimal nonUsableQty = (Convert.ToDecimal(rejected) - Convert.ToDecimal(reusableWasteQty));// * Convert.ToDecimal(weightPerPc);

            decimal nonUsableWeight = 0;
            DataTable dt2 = RunQuery.SQLQuery.ReturnDataTable("Select TOP(" + i + ") Id, WeightPerPc from PrdnPlasticFinishDetails  where PrdnID='' order by id");
            foreach (DataRow dr2 in dt2.Rows)
            {
                string id2 = dr2["Id"].ToString();
                string value = dr2["WeightPerPc"].ToString();
                nonUsableWeight = nonUsableWeight + (Convert.ToDecimal(value) * nonUsableQty);
            }
            RunQuery.SQLQuery.ExecNonQry("Update PrdnPlasticFinishDetails set ReusableWasteWeight='" + (nonUsableWeight / 1000M) + "' WHERE  id='" + id + "'");
            i++;
        }

        //txtWeightPc.Text = RunQuery.SQLQuery.ReturnString("Select SUM(WeightPerPc)  from PrdnPlasticFinishDetails  where PrdnID='' ");
        string lastPrdn = RunQuery.SQLQuery.ReturnString("Select NetProduction  from PrdnPlasticFinishDetails Where id=(Select ISNULL(MAX(id),0)  from PrdnPlasticFinishDetails  where PrdnID='') ");
        if (lastPrdn != "" && txtWeightPc.Text != "")
        {
            txtFinalQty.Text = lastPrdn;
            txtFinalWeight.Text = Convert.ToString(Convert.ToDecimal(lastPrdn) * Convert.ToDecimal(txtWeightPc.Text) / 1000M);
        }
    }

    protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddBrand2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddCompany2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand2.Items.Clear();
        ListItem lst = new ListItem("--- all ---", "");
        ddBrand2.Items.Insert(ddBrand2.Items.Count, lst);
        //ddBrand.Items.Add("--- all ---");
        ddBrand2.DataBind();
        StockDetails();
    }
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            string isExist = RunQuery.SQLQuery.ReturnString("Select ProductionID FROM PrdnPlasticFinish WHERE pid='" + lblItemCode.Text + "'");

            SQLQuery.ExecNonQry("Delete PrdnPlasticFinish where ProductionID ='" + isExist + "'   ");
            SQLQuery.ExecNonQry("Delete PrdnPlasticFinishDetails where PrdnID='" + isExist + "'   ");
            SQLQuery.ExecNonQry("Delete  Stock WHERE InvoiceID ='" + isExist + "'   ");
            //SQLQuery.ExecNonQry("Delete  where   ='" + isExist + "'   ");

            GridView1.DataBind();
            Notify("Info deleted!", "warn", lblMsg);
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    protected void ddOutSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");
        GetProductList();
    }
}

