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


public partial class app_Power_Press : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Today.Date.ToShortDateString();
            string lName = Page.User.Identity.Name.ToString();
            lblProject.Text = RunQuery.SQLQuery.ReturnString("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'");

            string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where CategoryID='9' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='9' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGradeRaw, "GradeID", "GradeName");

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND CategoryName<>'Body' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");

            //Processed Item
            gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where CategoryID='17' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutSubGroup, "CategoryID", "CategoryName");

            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='17' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddOutGrade.SelectedValue + "'  AND CategoryName<>'Body' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");

            ddPurpose.DataBind();
            ddCustomer.DataBind();
            ddBrand.DataBind();
            ddSize.DataBind();
            ddColor.DataBind();

            ddItemNameRaw.DataBind();
            ddSize.DataBind();
            ddColor.DataBind();
            ddReusableUsed.DataBind();
            ddGodown.DataBind();

            GetRawProductList();
            GetFinishedProductList();
            GridView2.DataBind();
            GetStandardProduction();

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

    private void Notify(string msg, string type)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            CalculateWeight();
            decimal inputAvailable = 0, inputAvailableKg = 0;
            if (ddstockType.SelectedValue == "Printed Sheet")
            {
                inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailablePrintedQty(ddPurpose.SelectedValue, "Printed Sheet", ddItemNameRaw.SelectedValue,
                              ddCustomer2.SelectedValue, ddBrand2.SelectedValue, ddSize2.SelectedValue, ddColor.SelectedValue,
                              ddGodown.SelectedValue, ddLocation.SelectedValue));
                inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailablePrintedWeight(ddPurpose.SelectedValue, "Printed Sheet", ddItemNameRaw.SelectedValue,
                               ddCustomer2.SelectedValue, ddBrand2.SelectedValue, ddSize2.SelectedValue, ddColor.SelectedValue,
                               ddGodown.SelectedValue, ddLocation.SelectedValue));
            }
            else
            {
                inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedQty(ddPurpose.SelectedValue, "Processed Sheet", ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
                inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedWeight(ddPurpose.SelectedValue, "Processed Sheet", ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
            }

            if (inputAvailable >= Convert.ToDecimal(txtInputQtyPCS.Text) && inputAvailableKg >= Convert.ToDecimal(txtInputWeight.Text))
            {
                if (btnSave.Text == "Save")
                {
                    SaveProduction();
                    UpdateProduction();
                    ClearMasterArea();
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Production data saved successfully";
                    Notify("Production data saved successfully", "success");
                }
                else
                {
                    UpdateProduction();
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Entry updated successfully";
                    Notify("Entry updated successfully", "success");
                }

            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Error: Input quantity is not available into stock!";
                Notify("Input quantity is not available into stock!", "error");
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_warning");
            lblMsg.Text = "Error: " + ex.ToString();
            Notify("Error: " + ex.Message.ToString(), "error");
        }
    }

    private void SaveProduction()
    {
        if (txtFinalProd.Text == "")
        {
            txtFinalProd.Text = "0";
        }
        string lName = Page.User.Identity.Name.ToString();
        string prdId = RunQuery.SQLQuery.ReturnString("Select 'PRD-PP-'+ CONVERT(varchar, (ISNULL (max(pid),0)+1001 )) from PrdnPowerPress");
        lblPrId.Text = prdId;

        SqlCommand cmd2 = new SqlCommand("INSERT INTO PrdnPowerPress (ProductionID, EntryBy) VALUES (@ProductionID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductionID", prdId);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

    }

    private void UpdateProduction()
    {
        string color = "";
        if (ddstockType.SelectedValue == "Printed Sheet")
        {
            color = ddColor.SelectedValue;
        }

        RunQuery.SQLQuery.ExecNonQry(@"Update PrdnPowerPress SET Date='" +
                                     Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") +
                                     "', MachineNo='" + ddMachine.SelectedValue +
                                     "', LineNumber='" + ddLine.SelectedValue + "', Purpose='" + ddPurpose.SelectedValue + "',  CustomerID='" + ddCustomer.SelectedValue +
                                     "', Brand='" + ddBrand.SelectedValue + "', PackSize='" + ddSize.SelectedValue + "', InputCustomer='" + ddCustomer2.SelectedValue +
                                     "', InputBrand='" + ddBrand2.SelectedValue + "', InputPackSize='" + ddSize2.SelectedValue + "', ItemSubGroup='" + ddSubGrp.SelectedValue + "', ItemGrade='" + ddGradeRaw.SelectedValue + "', ItemCategory='" + ddCategoryRaw.SelectedValue + "', ItemName='" +
                                     ddItemNameRaw.SelectedValue + "', Color='" + color + "', StockType='" + ddstockType.SelectedValue + "' WHERE ProductionID='" + lblPrId.Text + "' ");


        RunQuery.SQLQuery.ExecNonQry(@"Update PrdnPowerPress SET  InputSheetQty='" + txtInputQtyPCS.Text +
                                     "', PackPerSheet='" + txtPackPerSheet.Text + "', TotalInputPack='" + txtTotalPack.Text + "',  InputWeight='" + txtInputWeight.Text + "', " +
                                     " WeightPerPack='" + txtWeightPerPack.Text + "',  ReusableSheetItem='" + ddReusableSheetItem.SelectedValue + "',  ReusableSheetUsed='" + txtReusableSheetUsed.Text +
                                     "', ReusablePackPerSheet='" + txtReusablePackPerSheet.Text + "', NetInputPackPcs ='" + txtFinalProd.Text + "' WHERE ProductionID='" + lblPrId.Text + "' ");

        RunQuery.SQLQuery.ExecNonQry(@"Update PrdnPowerPress SET OutputItem='" + ddOutItem.SelectedValue + "',  FinalOutput='" + txtFinalOutput.Text +
                                             "', DepartmentEfficiency='" + txtDeptEfficiency.Text + "', Remarks ='" + txtRemark.Text + "' WHERE ProductionID='" + lblPrId.Text + "' ");

        RunQuery.SQLQuery.ExecNonQry("UPDATE PrdnPowerPressDetails SET PrdnID='" + lblPrId.Text +
                                     "' WHERE PrdnID='' ");
        //Item Stock Entry

        //Input Item stock-out
        Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Power Press", lblPrId.Text, ddSize2.SelectedValue, ddCustomer2.SelectedValue, ddBrand2.SelectedValue, color, "", ddItemNameRaw.SelectedValue, ddItemNameRaw.SelectedItem.Text, ddstockType.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue, "1", 0, Convert.ToInt32(txtInputQtyPCS.Text), 0, 0, Convert.ToDecimal(txtInputWeight.Text), "", "Stock-out", "Production", "Power Press", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        //Reusable Sheet Weight
        decimal qty = Convert.ToDecimal(Stock.Inventory.NonUsableQty(ddReusableSheetItem.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

        decimal rejUsedKg = 0;
        if (qty > 0)
        {
            rejUsedKg = Convert.ToDecimal(Stock.Inventory.NonUsableWeight(ddReusableSheetItem.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue)) / qty;
            rejUsedKg = Convert.ToDecimal(txtReusableSheetUsed.Text) * rejUsedKg;
        }
        Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Power Press", lblPrId.Text, "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ddReusableSheetItem.SelectedValue, ddReusableSheetItem.SelectedItem.Text, "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", 0, Convert.ToInt32(txtReusableSheetUsed.Text), 0, 0, rejUsedKg, "", "Stock-out", "Production", "Power Press", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        //Color Imression stock-in/out
        DataTable citydt = RunQuery.SQLQuery.ReturnDataTable(@"Select   InputQty, ReusableItemUsed, RejectedUsed, RejectUsedKg, 
                                                                FinalInputQty, OperationName, StdProduction, Operator, Rejected, 
                         NetPrdnSheet, ReusableItem, ReUsableQty, ReUsableWeightPerSheet,  NonUsableWasteWeight, RingWasteItem, RingWastePcs, RingWeightPerPack  from PrdnPowerPressDetails where PrdnID='" + lblPrId.Text + "'");

        foreach (DataRow citydr in citydt.Rows)
        {
            string Purpose = ddPurpose.SelectedValue;
            string InputQty = citydr["InputQty"].ToString();
            string ReusableItemUsed = citydr["ReusableItemUsed"].ToString();
            string RejectedUsed = citydr["RejectedUsed"].ToString();
            string RejectUsedKg = citydr["RejectUsedKg"].ToString();
            string FinalInputQty = citydr["FinalInputQty"].ToString();

            string OperationName = citydr["OperationName"].ToString();
            string StdProduction = citydr["StdProduction"].ToString();
            string Operator = citydr["Operator"].ToString();
            string Rejected = citydr["Rejected"].ToString();

            string NetPrdnSheet = citydr["NetPrdnSheet"].ToString();
            string ReusableItem = citydr["ReusableItem"].ToString();
            string ReUsableQty = citydr["ReUsableQty"].ToString();
            string ReUsableWeightPerSheet = citydr["ReUsableWeightPerSheet"].ToString();
            string NonUsableWasteWeight = citydr["NonUsableWasteWeight"].ToString();

            string RingWasteItem = citydr["RingWasteItem"].ToString();
            string RingWastePcs = citydr["RingWastePcs"].ToString();
            string RingWeightPerPack = citydr["RingWeightPerPack"].ToString();

            string ReusableItemName = Stock.Inventory.GetProductName(ReusableItemUsed);

            //decimal rejUsedKg = Convert.ToDecimal(RejectedUsed) * Convert.ToDecimal(Stock.Inventory.RawWeightQtyRatio("", "", ReusableItemUsed, "4"));
            
                Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- Power Press", lblPrId.Text, "",
                    ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ReusableItemUsed, ReusableItemName, "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", 0, Convert.ToInt32(RejectedUsed), 0, 0, Convert.ToDecimal(RejectUsedKg), "", "Stock-Out",
                    "Wastage used", "Power Press", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            
            //Stock In
            
            //rejUsedKg = Convert.ToDecimal(ReUsableQty) * Convert.ToDecimal(Stock.Inventory.RawWeightQtyRatio("", "", ReusableItem, "4"));
                decimal rejOutputWeight = Convert.ToDecimal(ReUsableWeightPerSheet) * Convert.ToDecimal(ReUsableQty) / 1000M;
            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- Power Press", lblPrId.Text, "",
                ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ReusableItem,
                Stock.Inventory.GetProductName(ReusableItem), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", Convert.ToInt32(ReUsableQty), 0, 0,
                rejOutputWeight, 0, "", "Stock-In", "Wastage Item", "Power Press", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            //rejUsedKg = Convert.ToDecimal(ReUsableQty) * Convert.ToDecimal(Stock.Inventory.RawWeightQtyRatio("", "", ReusableItem, "4"));
            decimal ringWasteWeight = Convert.ToDecimal(RingWastePcs) * Convert.ToDecimal(RingWeightPerPack) / 1000M;
            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- Power Press", lblPrId.Text, "",
                ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", RingWasteItem,
                Stock.Inventory.GetProductName(RingWasteItem), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", Convert.ToInt32(RingWastePcs), 0, 0,
                ringWasteWeight, 0, "", "Stock-In", "Wastage Item", "Power Press", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        }

        string nonUsableWeight = lblNonusableWeight.Text; //RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(NonUsableWasteWeight),0) from PrdnPowerPressDetails where PrdnID='" + lblPrId.Text + "'");
        
            Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Power Press", lblPrId.Text,
                "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", "1809", "Nonusable Westage", "", ddGodown.SelectedValue, ddLocation.SelectedValue,
                "7", 0, 0, 0, Convert.ToDecimal(nonUsableWeight), 0, "", "Stock-in", "Production", "Power Press",
                Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        

        //body er jonno pack, othoba sheet weight lagbe 
        decimal outputWeight = Convert.ToDecimal(txtWeightPerPack.Text) * Convert.ToDecimal(txtFinalOutput.Text) / 1000M; // Convert.ToDecimal(lblTotalOutputWeight.Text); //
        
            Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Power Press", lblPrId.Text,
                ddSize.SelectedValue, ddCustomer.SelectedValue, ddBrand.SelectedValue, ddOutputColor.SelectedValue, "",
                ddOutItem.SelectedValue, ddOutItem.SelectedItem.Text, "", ddGodown.SelectedValue, ddLocation.SelectedValue, "3",
                Convert.ToInt32(txtFinalOutput.Text), 0, 0, outputWeight, 0, "", "Stock-in", "Production", "Power Press",
                Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

       
        StockDetails();
    }


    protected void ddOutItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }
    protected void ddSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddGradeRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND CategoryName<>'Body'   AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");

        GetRawProductList();
    }
    protected void ddCategoryRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddOutCategory.SelectedValue = ddCategoryRaw.SelectedValue;
        GetRawProductList();

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

    private void GetRawProductList()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategoryRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemNameRaw, "ProductID", "ItemName");

        if (ddCategoryRaw.SelectedItem.Text == "Ring")
        {
            ringPanel.Visible = true;
        }
        else
        {
            ringPanel.Visible = false;
            txtRingWaste.Text = "0";
            txtRingWeightPerPack.Text = "0";
        }
        StockDetails();
    }
    private void GetFinishedProductList()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddOutCategory.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
        StockDetails();
    }

    private void StockDetails()
    {
        try { 
        decimal inputAvailable = 0, inputAvailableKg = 0;
        string stockInfo = "";
        if (ddstockType.SelectedValue == "Printed Sheet")
        {
            inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailablePrintedQty(ddPurpose.SelectedValue, "Printed Sheet", ddItemNameRaw.SelectedValue,
                          ddCustomer2.SelectedValue, ddBrand2.SelectedValue, ddSize2.SelectedValue, ddColor.SelectedValue,
                          ddGodown.SelectedValue, ddLocation.SelectedValue));
            inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailablePrintedWeight(ddPurpose.SelectedValue, "Printed Sheet", ddItemNameRaw.SelectedValue,
                           ddCustomer2.SelectedValue, ddBrand2.SelectedValue, ddSize2.SelectedValue, ddColor.SelectedValue,
                           ddGodown.SelectedValue, ddLocation.SelectedValue));

            ltrLastInfo.Text = "Available Stock: " + inputAvailable + " PCS, " + inputAvailableKg + "KG";
        }
        else
        {
            inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedQty(ddPurpose.SelectedValue, "Processed Sheet", ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
            inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedWeight(ddPurpose.SelectedValue, "Processed Sheet", ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

            ltrLastInfo.Text = "Available Stock: " + inputAvailable + " PCS, " + inputAvailableKg + "KG";
        }


        ltrReUsable.Text = "Available Stock: " + Stock.Inventory.NonUsableQty(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " pcs., " + Stock.Inventory.NonUsableWeight(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " kg";

        ltrReUsableSheet.Text = "Available Stock: " + Stock.Inventory.NonUsableQty(ddReusableSheetItem.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " pcs., " + Stock.Inventory.NonUsableWeight(ddReusableSheetItem.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " kg";


        if (inputAvailable > 0 && inputAvailableKg > 0)
        {
            txtWeightRatio.Text = Convert.ToString(inputAvailableKg / inputAvailable);
        }


        /*
        //Reusable weight
        decimal qty = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedQty(ddPurpose.SelectedValue, ddReusableUsed.SelectedValue, ddGodown.SelectedValue));
        decimal weight = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedWeight(ddPurpose.SelectedValue, ddReusableUsed.SelectedValue, ddGodown.SelectedValue));
        ltrReusableAvailable.Text = qty + "pcs, " + weight + "kg";
        if (qty > 0 && weight > 0)
        {
            lblWeightPerPc.Text = Convert.ToString(weight / qty);
        }
        else
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "Reusable item is not available into stock!";
        }
        */
        //Verify Stock Type
        if (ddstockType.SelectedValue == "Processed Sheet")
        {
            PnlColor.Visible = false;
        }
        else
        {
            PnlColor.Visible = true;
        }

        if (ddCategoryRaw.SelectedItem.Text == "Ring")
        {
            ringPanel.Visible = true;
        }
        else
        {
            ringPanel.Visible = false;
        }

        CalculateWeight();
           
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.ToString();
            Notify("Error: " + ex.Message.ToString(), "error");
        }
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
            RunQuery.SQLQuery.Empty2Zero(txtStdPrdn);

            string lName = Page.User.Identity.Name.ToString();
            if (txtSheetQty.Text != "" && txtPackPerSheet.Text != "" && txtRejected.Text != "")
            {
                if (btnAdd.Text == "Add")
                {
                    if (txtStdPrdn.ReadOnly == false)
                    {
                        RunQuery.SQLQuery.ExecNonQry("INSERT INTO ProductionStandard ( Section, MachineNo, ItemGrade, ItemCategory, PackSize, Operation, StdPrdn, Remarks, EntryBy) VALUES ('1','" + ddMachine.SelectedValue + "','" + ddOutGrade.SelectedValue + "','" + ddCategoryRaw.SelectedValue + "','" + ddSize.SelectedValue + "','" + ddOperation.SelectedValue + "','" + txtStdPrdn.Text + "','Added automatically from Power Press','" + lName + "')");
                    }

                    InsertDetailData();

                    lblMsg2.Attributes.Add("class", "xerp_success");
                    lblMsg2.Text = "New operation added successfully";
                    Notify("New operation added successfully", "success");

                }
                else
                {
                    //Item Weight from Weight Ratio
                    decimal qty = Convert.ToDecimal(Stock.Inventory.NonUsableQty(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

                    decimal rejUsedKg = 0;
                    if (qty > 0)
                    {
                        rejUsedKg = Convert.ToDecimal(Stock.Inventory.NonUsableWeight(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue)) / qty;
                        rejUsedKg = Convert.ToDecimal(txtRejUsed.Text) * rejUsedKg;
                    }
                    //decimal rejUsedKg = Convert.ToDecimal(txtRejUsed.Text) * Convert.ToDecimal(Stock.Inventory.RawWeightQtyRatio("", "", ddReusableUsed.SelectedValue, "4"));
                    decimal nonUsableWasteWeight = 0;
                    if (txtRejected.Text != "" && txtReUsable.Text != "" && txtInputWeight.Text != "" && txtTotalPack.Text != "" && txtTotalPack.Text != "0")
                    {
                        nonUsableWasteWeight = ((Convert.ToDecimal(txtRejected.Text) - Convert.ToDecimal(txtReUsable.Text)) *
                                                        (Convert.ToDecimal(txtInputWeight.Text) / Convert.ToDecimal(txtTotalPack.Text)));
                    }
                    if (nonUsableWasteWeight < 0)
                    {
                        nonUsableWasteWeight = 0;
                    }

                    decimal opEfficiency = 0;
                    if (txtStdPrdn.Text != "0")
                    {
                        opEfficiency = ((Convert.ToDecimal(txtFinalProd.Text) / Convert.ToDecimal(txtHour.Text)) / Convert.ToDecimal(txtStdPrdn.Text));
                    }

                    //RunQuery.SQLQuery.ExecNonQry("Delete PrdnPowerPressDetails where id='" + lblEid.Text + "'");
                    //InsertDetailData();
                    RunQuery.SQLQuery.ExecNonQry(@"UPDATE PrdnPowerPressDetails SET InputQty='" + txtSheetQty.Text +
                                                 "', ReusableItemUsed='" +
                                                 ddReusableUsed.SelectedValue + "', RejectedUsed='" + txtRejUsed.Text +
                                                 "', RejectUsedKg='" + rejUsedKg +
                                                 "', FinalInputQty='" + txtFinalInput.Text + "', OperationName='" +
                                                 ddOperation.SelectedValue + "', StdProduction='" +
                                                 txtStdPrdn.Text + "', Operator='" + ddOperator.SelectedValue +
                                                 "', Rejected='" + txtRejected.Text +
                                                 "', NetPrdnSheet='" + txtFinalProd.Text + "', WorkingHr='" +
                                                 txtHour.Text + "', TimeWaste='" + txtTimeWaist.Text +
                                                 "', Reason='" + txtReason.Text + "', Shift='" + ddShift.SelectedValue +
                                                 "', ReusableItem='" +
                                                 ddReuseWaste.SelectedValue + "', ReUsableQty='" + txtReUsable.Text +
                                                 "', NonUsableWasteWeight='" +
                                                 nonUsableWasteWeight + "', OperatorEfficiency='" + opEfficiency +
                                                 "', RingWasteItem='" + ddRingWasteItem.SelectedValue +
                                                 "', RingWastePcs='" + txtRingWaste.Text + "', RingWeightPerPack='" +
                                                 txtRingWeightPerPack.Text + "' WHERE id='" + lblEid.Text + "'");


                    RunQuery.SQLQuery.ExecNonQry(@"UPDATE PrdnPowerPressDetails SET ReUsableWeightPerSheet='" + txtReusableWeightPerPack.Text + "', RingWasteItem='" +
                       ddRingWasteItem.SelectedValue + "', RingWastePcs='" + txtRingWaste.Text + "', RingWeightPerPack='" + txtRingWeightPerPack.Text +
                       "' WHERE id='" + lblEid.Text + "'");

                    CalculateWeight();
                    ClearDetailArea();
                    btnAdd.Text = "Add";
                    lblMsg2.Attributes.Add("class", "xerp_success");
                    lblMsg2.Text = "Entry updated successfully";
                    Notify("Entry updated successfully", "info");
                }
            }
            else
            {
                lblMsg2.Attributes.Add("class", "xerp_error");
                lblMsg2.Text = "Please fillup all mendatory fields...";
                Notify("Please fillup all mendatory fields...", "warn");
            }


            //lblMsg2.Attributes.Add("class", "xerp_error");
            //lblMsg2.Text = "Error: Input weight & output weight must have to be equel!";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "Error: " + ex.ToString();
            Notify("Error: " + ex.Message.ToString(), "error");
        }
        finally
        {
            GridView2.DataBind();
            txtSheetQty.Focus();
        }

    }

    private void InsertDetailData()
    {
        //Item Weight from Weight Ratio
        decimal qty = Convert.ToDecimal(Stock.Inventory.NonUsableQty(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

        decimal rejUsedKg = 0;
        if (qty > 0)
        {
            rejUsedKg = Convert.ToDecimal(Stock.Inventory.NonUsableWeight(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue)) / qty;
            rejUsedKg = Convert.ToDecimal(txtRejUsed.Text) * rejUsedKg;
        }
        //Non usable stock in KG
        decimal nonUsableWasteWeight = 0;
        if (txtRejected.Text != "" && txtReUsable.Text != "" && txtInputWeight.Text != "" && txtTotalPack.Text != "" && txtTotalPack.Text != "0")
        {
            nonUsableWasteWeight = ((Convert.ToDecimal(txtRejected.Text) - Convert.ToDecimal(txtReUsable.Text)) *
                                            (Convert.ToDecimal(txtInputWeight.Text) / Convert.ToDecimal(txtTotalPack.Text)));
        }
        decimal opEfficiency = 0;
        if (txtStdPrdn.Text != "0")
        {
            opEfficiency = ((Convert.ToDecimal(txtFinalProd.Text) / Convert.ToDecimal(txtHour.Text)) / Convert.ToDecimal(txtStdPrdn.Text));
        }

        string ringwasteItem = "";
        int ringwastePcs = 0;
        decimal RingWeightPerPack = 0;

        if (ddCategoryRaw.SelectedItem.Text == "Ring")
        {
            ringwasteItem = ddRingWasteItem.SelectedValue;
             ringwastePcs = Convert.ToInt32(txtRingWaste.Text);
            RingWeightPerPack = Convert.ToDecimal(txtRingWeightPerPack.Text);
        }
        
        SqlCommand cmd3 = new SqlCommand(@"INSERT INTO PrdnPowerPressDetails (Section, PrdnID, 
                            InputQty, ReusableItemUsed, RejectedUsed, RejectUsedKg, FinalInputQty, 
                            OperationName, StdProduction, Operator, Rejected, NetPrdnSheet, WorkingHr, TimeWaste, 
                            Reason, Shift, ReusableItem, ReUsableQty, ReUsableWeightPerSheet, NonUsableWasteWeight, OperatorEfficiency, RingWasteItem, RingWastePcs, RingWeightPerPack, EntryBy)" +
                                         " VALUES ('Power Press', '', '" + txtSheetQty.Text + "', '" +
                                         ddReusableUsed.SelectedValue + "', '" + txtRejUsed.Text + "', '" + rejUsedKg +
                                         "', '" + txtFinalInput.Text + "', '" + ddOperation.SelectedValue + "', '" +
                                         txtStdPrdn.Text + "', '" + ddOperator.SelectedValue + "', '" + txtRejected.Text +
                                         "', '" + txtFinalProd.Text + "', '" + txtHour.Text + "', '" + txtTimeWaist.Text +
                                         "', '" + txtReason.Text + "', '" + ddShift.SelectedValue + "', '" +
                                         ddReuseWaste.SelectedValue + "', '" + txtReUsable.Text + "', '"+txtReusableWeightPerPack.Text+"', '" +
                                         nonUsableWasteWeight + "', '" + opEfficiency + "', '" + ringwasteItem + "', '" + ringwastePcs + "', '" + RingWeightPerPack + "', @EntryBy)",
            new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd3.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        cmd3.Connection.Open();
        cmd3.ExecuteNonQuery();
        cmd3.Connection.Close();

        string sumReUsableQty = "0";//RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(ReUsableQty),0) From PrdnPowerPressDetails where PrdnID=''");
        lblLastPrdnPack.Text = Convert.ToString(Convert.ToDecimal(txtFinalProd.Text) + Convert.ToDecimal(sumReUsableQty));

        txtSheetQty.Text = txtFinalProd.Text;
        txtFinalOutput.Text = txtFinalProd.Text;
        ClearDetailArea();
    }

    private void ClearDetailArea()
    {
        //txtSheetQty.Text = "";
        txtRejUsed.Text = "";
        txtFinalInput.Text = "";
        txtRejected.Text = "";
        txtFinalProd.Text = "";
        //txtStdPrdn
        txtHour.Text = "";
        txtTimeWaist.Text = "";
        txtReason.Text = "";
        txtReUsable.Text = "";
        txtEfficiency.Text = "";

        txtReUsable.Text = "";
        txtReusableWeightPerPack.Text = "";
        txtRingWaste.Text = "";
        txtRingWeightPerPack.Text = "";
        GridView2.DataBind();
    }
    private void ClearMasterArea()
    {
        ClearDetailArea();

        txtInputQtyPCS.Text = "";
        txtPackPerSheet.Text = "";
        txtTotalPack.Text = "";
        txtInputWeight.Text = "";
        txtWeightPerPack.Text = "";
        txtReusableSheetUsed.Text = "";
        txtReusablePackPerSheet.Text = "";

        txtHour.Text = "";
        txtTimeWaist.Text = "";
        txtReason.Text = "";
        txtFinalOutput.Text = "";
        txtDeptEfficiency.Text = "";
        txtRemark.Text = "";
        GridView1.DataBind();

        lblTotalInputWeight.Text = "0";
            lblTotalOutputWeight.Text = "0";
            lblNonusableWeight.Text = "0";
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
            StockDetails();
            btnAdd.Text = "Update";

            lblMsg2.Attributes.Add("class", "xerp_info");
            lblMsg2.Text = "Edit mode activated ...";
            Notify("Edit mode activated ...", "warn");
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.ToString();
            Notify("Error: " + ex.Message.ToString(), "error");
        }
    }

    private void EditMode(string entryID)
    {
        SqlCommand cmd = new SqlCommand(@"SELECT InputQty, ReusableItemUsed, RejectedUsed, FinalInputQty, OperationName, StdProduction, Operator, Rejected, NetPrdnSheet, 
                         WorkingHr, TimeWaste, Reason, Shift, ReusableItem, ReUsableQty, NonUsableWasteWeight, OperatorEfficiency, ReUsableWeightPerSheet FROM [PrdnPowerPressDetails] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            txtSheetQty.Text = dr[0].ToString();
            ddReusableUsed.SelectedValue = dr[1].ToString();
            txtRejUsed.Text = dr[2].ToString();

            txtFinalInput.Text = dr[3].ToString();
            ddOperation.SelectedValue = dr[4].ToString();
            txtStdPrdn.Text = dr[5].ToString();
            ddOperator.SelectedValue = dr[6].ToString();
            txtRejected.Text = dr[7].ToString();

            txtFinalProd.Text = dr[8].ToString();
            txtHour.Text = dr[9].ToString();
            txtTimeWaist.Text = dr[10].ToString();
            txtReason.Text = dr[11].ToString();
            ddShift.SelectedValue = dr[12].ToString();

            ddReuseWaste.SelectedValue = dr[13].ToString();
            txtReUsable.Text = dr[14].ToString();//
            txtEfficiency.Text = dr[16].ToString();
            txtReusableWeightPerPack.Text = dr[17].ToString();
        }
        cmd.Connection.Close();

        //txtRingWaste
        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT RingWasteItem, RingWastePcs, RingWeightPerPack FROM [PrdnPowerPressDetails] WHERE Id='" + entryID + "'");

        foreach (DataRow drx in dtx.Rows)
        {
            ddRingWasteItem.SelectedValue = drx["RingWasteItem"].ToString();
            txtRingWaste.Text = drx["RingWastePcs"].ToString();
            txtRingWeightPerPack.Text = drx["RingWeightPerPack"].ToString();
        }
    }
    protected void GridView2_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView2.Rows[index].FindControl("Label1") as Label;

            SqlCommand cmd7 = new SqlCommand("DELETE PrdnPowerPressDetails WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
            Notify("Error: " + ex.Message.ToString(), "error");
        }
    }

    protected void GridView2_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
            StockDetails();
    }

    private void CalculateWeight()
    {
        if (txtInputWeight.Text != "" && lblLastPrdnPack.Text != "" && txtWeightPerPack.Text != "" && txtReusableSheetUsed.Text != "")
        {
            //Total Input-Output Weight
            //decimal rejUsedKg = Convert.ToDecimal(txtReusableSheetUsed.Text) * Convert.ToDecimal(Stock.Inventory.RawWeightQtyRatio("", "", ddReusableSheetItem.SelectedValue, "4"));
            decimal qty = Convert.ToDecimal(Stock.Inventory.NonUsableQty(ddReusableSheetItem.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

            decimal rejUsedKg = 0;
            if (qty > 0)
            {
               rejUsedKg = Convert.ToDecimal(Stock.Inventory.NonUsableWeight(ddReusableSheetItem.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue))/qty ;
               rejUsedKg = Convert.ToDecimal(txtReusableSheetUsed.Text)*rejUsedKg;
            }
            string ttlRejectUsedKg = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(RejectUsedKg),0) From PrdnPowerPressDetails where PrdnID=''");
            decimal ttlinputWeight = Convert.ToDecimal(txtInputWeight.Text) + rejUsedKg + (Convert.ToDecimal(ttlRejectUsedKg));
            lblTotalInputWeight.Text = ttlinputWeight.ToString("##.###") + "";

            decimal ttlRejectKg = 0;
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT ReUsableQty, ReUsableWeightPerSheet, RingWastePcs, RingWeightPerPack  FROM PrdnPowerPressDetails WHERE (PrdnID = '')");
            foreach (DataRow drx in dtx.Rows)
            {
                string ReUsableQty = drx["ReUsableQty"].ToString();
                string ReUsableWeightPerSheet = drx["ReUsableWeightPerSheet"].ToString();
                string RingWastePcs = drx["RingWastePcs"].ToString();
                string RingWeightPerPack = drx["RingWeightPerPack"].ToString();
                ttlRejectKg += ((Convert.ToDecimal(ReUsableQty)*Convert.ToDecimal(ReUsableWeightPerSheet)) +
                               (Convert.ToDecimal(RingWastePcs)*Convert.ToDecimal(RingWeightPerPack)));
            }
            //decimal ttlRejectKg = RunQuery.SQLQuery.ReturnString(@"SELECT ISNULL((SUM(ReUsableQty) * AVG(ReUsableWeightPerSheet)) + (SUM(RingWastePcs) * AVG(RingWeightPerPack)),0) AS Expr1 FROM PrdnPowerPressDetails WHERE (PrdnID = '')");
            decimal ttlOutputWeight = (Convert.ToDecimal(lblLastPrdnPack.Text) * (Convert.ToDecimal(txtWeightPerPack.Text) / 1000M)) + (Convert.ToDecimal(ttlRejectKg)/1000M);
            lblTotalOutputWeight.Text = ttlOutputWeight.ToString("##.###") + "";

            decimal nonUseKg = ttlinputWeight - ttlOutputWeight;
            if (nonUseKg>=0)
            {
                lblNonusableWeight.Text = Convert.ToDecimal(nonUseKg).ToString("##.###") + "";
            }
            else
            {
                lblNonusableWeight.Text = "0";
            }
        }
    }

    protected void ddCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand.Items.Clear();
        ListItem lst = new ListItem("--- all ---", "");
        ddBrand.Items.Insert(ddBrand.Items.Count, lst);
        //ddBrand.Items.Add("--- all ---");
        ddBrand.DataBind();
        StockDetails();
    }

    protected void ddOperation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetStandardProduction();
    }

    private void GetStandardProduction()
    {
        string stdPrdn = RunQuery.SQLQuery.ReturnString("SELECT StdPrdn FROM [ProductionStandard] WHERE MachineNo='" +
                                           ddMachine.SelectedValue + "' AND ItemCategory='" +
                                           ddCategoryRaw.SelectedValue + "' AND PackSize='" + ddSize.SelectedValue +
                                           "'  AND Operation='" + ddOperation.SelectedValue + "'   ");
        if (stdPrdn == "")
        {
            txtStdPrdn.ReadOnly = false;
        }
        else
        {
            txtStdPrdn.Text = stdPrdn;
            //txtStdPrdn.ReadOnly = true;
        }

    }

    protected void ddstockType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddGodown_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddReusableUsed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddColor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddReusableSheetItem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
        //Notify("You have selected: " + ddReusableSheetItem.SelectedItem.Text, "info");
    }

    protected void ddOutGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddOutGrade.SelectedValue + "'  AND CategoryName<>'Body' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");

        GetFinishedProductList();
    }

    protected void ddOutCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetFinishedProductList();
    }

    protected void ddCustomer2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand2.Items.Clear();
        ListItem lst = new ListItem("--- all ---", "");
        ddBrand2.Items.Insert(ddBrand2.Items.Count, lst);
        ddBrand2.DataBind();

        StockDetails();
    }

    protected void ddBrand2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddSize2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            string isExist = RunQuery.SQLQuery.ReturnString("Select ProductionID FROM PrdnPowerPress WHERE pid='" + lblItemCode.Text + "'");

            SQLQuery.ExecNonQry("Delete PrdnPowerPress where  ProductionID ='" + isExist + "'   ");
            SQLQuery.ExecNonQry("Delete PrdnPowerPressDetails where   PrdnID='" + isExist + "'   ");
            SQLQuery.ExecNonQry("Delete  Stock WHERE InvoiceID ='" + isExist + "'   ");
            //SQLQuery.ExecNonQry("Delete  where   ='" + isExist + "'   ");

            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }
}
