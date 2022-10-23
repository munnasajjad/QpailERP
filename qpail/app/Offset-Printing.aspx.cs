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


public partial class app_Offset_Printing : System.Web.UI.Page
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

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");

            //Color Item   
            /*
            gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where CategoryID='17' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutSubGroup, "CategoryID", "CategoryName");

            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='17' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddColorCategory, "CategoryID", "CategoryName");
            */

            ddPurpose.DataBind();
            ddCustomer.DataBind();
            ddBrand.DataBind();
            ddSize.DataBind();
            ddColorCategory.DataBind();

            LoadColorDD();

            GetInputRawProducts();
            GetPrintedSheetList();
            LoadOutputItems();
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

    private void LoadOutputItems()
    {
        string catId = "0";
        pnlTinBodyOutput.Visible = false;
        //pnlTinBodyOutput2.Visible = false;

        if (ddCategoryRaw.SelectedValue == "285")
        {
            pnlTinBodyOutput2.Attributes.Remove("class");
            pnlTinBodyOutput3.Attributes.Add("class", "hidden");
            catId = "17";
            pnlTinBodyOutput.Visible = true;
            pnlTinBodyOutput2.Visible = true;
            txtPackPerSheet.Text = "";
            txtWeightPerPack.Text = "";
            txtTotalPack.Text = "";
            //Label27.Text = "Final Input (Kg) : ";
        }
        else
        {
            pnlTinBodyOutput2.Attributes.Add("class", "hidden");
            pnlTinBodyOutput3.Attributes.Remove("class");
            txtPackPerSheet.Text = "0";
            txtWeightPerPack.Text = "0";
            txtTotalPack.Text = "0";
            //Label27.Text = "Final Output (Kg) : ";
        }

        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where CategoryID='" + catId + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutSubGroup, "CategoryID", "CategoryName");

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + catId + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");

        GetPrintedSheetList();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedQty(ddPurpose.SelectedValue, "Processed Sheet", ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
            decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedWeight(ddPurpose.SelectedValue, "Processed Sheet", ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

            if (inputAvailable >= Convert.ToDecimal(txtInputQtyPCS.Text) && inputAvailableKg >= Convert.ToDecimal(txtInputWeight.Text))
            {
                //if (Convert.ToDecimal(txtTotalPack.Text) - 1 <= Convert.ToDecimal(ttlWeight))
                //{
                if (btnSave.Text == "Save")
                {
                    SaveProduction();
                    UpdateProduction();

                    string pdt = txtDate.Text;
                    ClearControls(Form);
                    lblTotalWeight.Text = "";
                    txtDate.Text = pdt;
                    LoadOutputItems();
                    StockDetails();
                    GridView2.DataBind();
                    GridView1.DataBind();

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Production saved successfully";

                    Notify("Offset printing production saved successfully", "success");
                    //lblMsg2.Attributes.Add("class", "xerp_success");
                }
                else
                {
                    UpdateProduction();
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Entry updated successfully";
                }
                //    }
                //    else
                //    {
                //        lblMsg.Attributes.Add("class", "xerp_error");
                //        lblMsg.Text = "Error: Input weight & output weight is not equel!";
                //    }
            }
            else
            {
                //lblMsg.Attributes.Add("class", "xerp_error");
                Notify("Error: Input item is not available into stock!", "error");  //lblMsg.Text = ;
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error");
            lblMsg.Attributes.Add("class", "xerp_warning");
            lblMsg.Text = "Error: " + ex.ToString();
        }
    }

    private void SaveProduction()
    {
        if (txtTotalPack.Text == "")
        {
            txtTotalPack.Text = "0";
        }
        string lName = Page.User.Identity.Name.ToString();
        string prdId = RunQuery.SQLQuery.ReturnString("Select 'PRD-OP-'+ CONVERT(varchar, (ISNULL (max(pid),0)+1001 )) from PrdOffsetPrint");
        lblPrId.Text = prdId;

        SqlCommand cmd2 = new SqlCommand("INSERT INTO PrdOffsetPrint (ProductionID, EntryBy) VALUES (@ProductionID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductionID", prdId);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

    }

    private void UpdateProduction()
    {
        RunQuery.SQLQuery.ExecNonQry(@"Update PrdOffsetPrint SET Section='Offset Printing', Date='" +
                                     Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") +
                                     "', MachineNo='" + ddMachine.SelectedValue +
                                     "', LineNumber='" + ddLine.SelectedValue + "', Purpose='" + ddPurpose.SelectedValue + "' , CustomerID='" + ddCustomer.SelectedValue +
                                     "', CustomerName='" + ddCustomer.SelectedItem.Text + "', Brand='" + ddBrand.SelectedValue + "', PackSize='" + ddSize.SelectedValue + "' , ItemSubGroup='" + ddSubGrp.SelectedValue +
                                     "', ItemGrade='" + ddGradeRaw.SelectedValue + "', ItemCategory='" +
                                     ddCategoryRaw.SelectedValue + "', ItemName='" + ddItemNameRaw.SelectedValue +
                                     "', Product='" + ddItemNameRaw.SelectedItem.Text + "',  InputSheetQty='" + txtInputQtyPCS.Text + "',WeightPerSheet='" + txtWeightSheet.Text + "', InputWeight='" + txtInputWeight.Text + "', OutputItem='" + ddOutItem.SelectedValue + "',    OutputColor='" + ddOutputColor.SelectedValue + "',  FinalOutput='" + txtFinalOutput.Text +
                                     "', PackPerSheet='" + txtPackPerSheet.Text + "', TotalPack='" + txtTotalPack.Text + "', Remarks ='" +
                                     txtRemark.Text + "' WHERE ProductionID='" + lblPrId.Text + "' ");

        //Item Stock Entry
        RunQuery.SQLQuery.ExecNonQry("DELETE Stock WHERE InvoiceID='" + lblPrId.Text + "'");
        RunQuery.SQLQuery.ExecNonQry("UPDATE PrdOffsetPrintDetails SET PrdnID='" + lblPrId.Text +
                                     "' WHERE PrdnID='' AND Section='Offset Printing'");

        decimal price = 0;// Inventory.LastNonprintedPrice(Purpose, itemType, Item);
        //Input Item stock-out
        Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Offset Printing", lblPrId.Text,
            "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ddItemNameRaw.SelectedValue,
            ddItemNameRaw.SelectedItem.Text, "Processed Sheet", ddGodown.SelectedValue, ddLocation.SelectedValue, "1", 0, Convert.ToInt32(txtInputQtyPCS.Text),
            price, 0, Convert.ToDecimal(txtInputWeight.Text), "", "Stock-out", "Production", "Offset Printing",
            Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        //Color Imression stock-in/out
        DataTable citydt = RunQuery.SQLQuery.ReturnDataTable("Select  InputQty, ReusableItemUsed, RejectedUsed, FinalInputQty, InkCategory, " +
                                                             "ColorName, Spec, InkConsum, SheetRejected, NetPrdnSheet, ReusableItem, ReUsableQty, " +
                                                             "NonUsableWasteWeight     from PrdOffsetPrintDetails where PrdnID='" + lblPrId.Text + "'");

        foreach (DataRow citydr in citydt.Rows)
        {
            string Purpose = ddPurpose.SelectedValue;
            string InputQty = citydr["InputQty"].ToString();
            string ReusableItemUsed = citydr["ReusableItemUsed"].ToString();
            string RejectedUsed = citydr["RejectedUsed"].ToString();
            string FinalInputQty = citydr["FinalInputQty"].ToString();

            string InkCategory = citydr["InkCategory"].ToString();
            string ColorName = citydr["ColorName"].ToString();
            string Spec = citydr["Spec"].ToString();
            string InkConsum = citydr["InkConsum"].ToString();
            string SheetRejected = citydr["SheetRejected"].ToString();

            string NetPrdnSheet = citydr["NetPrdnSheet"].ToString();
            string ReusableItem = citydr["ReusableItem"].ToString();
            string ReUsableQty = citydr["ReUsableQty"].ToString();
            string NonUsableWasteWeight = citydr["NonUsableWasteWeight"].ToString();

            string ReusableItemName = Stock.Inventory.GetProductName(ReusableItemUsed);
            
            //decimal rejUsedKg = Convert.ToDecimal(RejectedUsed) * Convert.ToDecimal(Stock.Inventory.RawWeightQtyRatio("", "", ReusableItemUsed, "4"));
            string nonUsableQty = Stock.Inventory.NonUsableQty(ReusableItemUsed, ddGodown.SelectedValue, ddLocation.SelectedValue);
            decimal ratio = 0;
            if (nonUsableQty!="0")
            {
                ratio= Convert.ToDecimal(Stock.Inventory.NonUsableWeight(ReusableItemUsed, ddGodown.SelectedValue, ddLocation.SelectedValue)) / Convert.ToDecimal(nonUsableQty);    
            }
            
            decimal rejUsedKg = Convert.ToDecimal(RejectedUsed) * ratio;

            //Stock In
            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- Offset Printing", lblPrId.Text, "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ReusableItemUsed, ReusableItemName, "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", 0, Convert.ToInt32(RejectedUsed), price, 0, rejUsedKg, "", "Stock-Out", "Wastage used", "Offset Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            //Save spec to db during add
            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- Offset Printing", lblPrId.Text, "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", Spec, ColorName, Stock.Inventory.GetProductName(ColorName), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", 0, 0, price, 0, Convert.ToDecimal(InkConsum) / 1000M, "", "Stock-Out", "Ink used", "Offset Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            if (Convert.ToDecimal(ReUsableQty) > 0)
            {
                rejUsedKg = Convert.ToDecimal(ReUsableQty) * Convert.ToDecimal(txtWeightSheet.Text) / 1000M * Convert.ToDecimal(txtColorWeightConstant.Text);
                Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- Offset Printing", lblPrId.Text, "",
                    ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ReusableItem,
                    Stock.Inventory.GetProductName(ReusableItem), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", Convert.ToInt32(ReUsableQty), 0, 0,
                    rejUsedKg, 0, "", "Stock-In", "Wastage Item", "Offset Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            }
        }

        //body er jonno pack, othoba sheet weight lagbe
        decimal outputWeight = Convert.ToDecimal(txtFinalOutputKg.Text); //Convert.ToInt32(txtTotalPack.Text)*Convert.ToDecimal(txtWeightPerPack);

        if (ddCategoryRaw.SelectedValue == "285") //Save body Packs
        {
            outputWeight = Convert.ToDecimal(txtWeightPerPack.Text) * Convert.ToDecimal(txtTotalPack.Text) / 1000M;
            Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Offset Printing", lblPrId.Text, ddSize.SelectedValue, ddCustomer.SelectedValue, ddBrand.SelectedValue, ddOutputColor.SelectedValue, "", ddOutItem.SelectedValue, ddOutItem.SelectedItem.Text, "", ddGodown.SelectedValue, ddLocation.SelectedValue, "3", Convert.ToInt32(txtTotalPack.Text), 0, price, outputWeight, 0, "", "Stock-in", "Production", "Offset Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            //Nonusable wastage for body
            Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Offset Printing", lblPrId.Text, "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", "1809", "Nonusable Wastage", "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", 0, 0, price, Convert.ToDecimal(txtNonusableWeight.Text), 0, "", "Stock-in", "Production", "Offset Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        }
        else
        {
            Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Offset Printing", lblPrId.Text, ddSize.SelectedValue, ddCustomer.SelectedValue, ddBrand.SelectedValue, ddOutputColor.SelectedValue, "", ddItemNameRaw.SelectedValue, ddItemNameRaw.SelectedItem.Text, "Printed Sheet", ddGodown.SelectedValue, ddLocation.SelectedValue, "3", Convert.ToInt32(txtFinalOutput.Text), 0, price, Convert.ToDecimal(outputWeight), 0, "", "Stock-in", "Production", "Offset Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            string nonUsableWeight = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(NonUsableWasteWeight),0) from PrdOffsetPrintDetails where PrdnID='" + lblPrId.Text + "'");
            if (Convert.ToDecimal(nonUsableWeight) > 0)
            {
                Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Offset Printing", lblPrId.Text, "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", "1809", "Nonusable Westage", "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", 0, 0, price, Convert.ToDecimal(nonUsableWeight), 0, "", "Stock-in", "Production", "Offset Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            }

            //Nonusable wastage for bottom
            decimal reusableInputWeight = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT ISNULL(SUM(RejectedUsedKg),0) from PrdOffsetPrintDetails where PrdnID='" + lblPrId.Text + "'"));
            decimal inputWeight = ((reusableInputWeight) + Convert.ToDecimal(txtInputWeight.Text)) * Convert.ToDecimal(txtColorWeightConstant.Text);
            decimal nonUsableWeight2 = Convert.ToDecimal(inputWeight) - Convert.ToDecimal(txtFinalOutputKg.Text);

            //Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Offset Printing", lblPrId.Text,
            //    "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", "1809", "Nonusable Wastage", "", "4", "",
            //    "7", 0, 0, Convert.ToDecimal((nonUsableWeight2).ToString("##.###")), 0, "", "Stock-in", "Production", "Offset Printing",
            //    Page.User.Identity.Name.ToString());

            Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Offset Printing", lblPrId.Text,
                    "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", "1809", "Nonusable Wastage", "", ddGodown.SelectedValue, ddLocation.SelectedValue,
                    "7", 0, 0, 0, nonUsableWeight2, 0, "", "Stock-in", "Production", "Offset Printing",
                    Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        }

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
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddColorCategory, "CategoryID", "CategoryName");

        GetInputRawProducts();
    }
    protected void ddCategoryRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddColorCategory.SelectedValue = ddCategoryRaw.SelectedValue;
        GetInputRawProducts();
        LoadOutputItems();
    }
    protected void ddItemNameRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    private void LoadColorDD()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddColorCategory.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddColor, "ProductID", "ItemName");
        ddSpec.DataBind();
        StockDetails();
    }

    private void GetInputRawProducts()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategoryRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemNameRaw, "ProductID", "ItemName");
        StockDetails();
    }

    private void GetPrintedSheetList()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddOutCategory.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
        StockDetails();
    }

    private void StockDetails()
    {
        decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedQty(ddPurpose.SelectedValue, "Processed Sheet", ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
        decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedWeight(ddPurpose.SelectedValue, "Processed Sheet", ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

        ltrLastInfo.Text = "Available Stock: " + inputAvailable + " PCS, " + inputAvailableKg + "KG";

        ltrColorStock.Text = "Available Stock: " + Stock.Inventory.AvailableInkWeight(ddColor.SelectedValue, ddSpec.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " kg";
        
        ltrReUsable.Text = "Available Stock: " + Stock.Inventory.NonUsableQty(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " pc. " + Stock.Inventory.NonUsableWeight(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " kg";

        if (inputAvailable > 0 && inputAvailableKg > 0)
        {
            txtWeightSheet.Text = Convert.ToDecimal(inputAvailableKg / inputAvailable * 1000M).ToString("##.###");
            if (txtInputQtyPCS.Text != "")
            {
                txtInputWeight.Text = Convert.ToString(Convert.ToDecimal(txtInputQtyPCS.Text) * Convert.ToDecimal(txtWeightSheet.Text) / 1000M);
            }
        }

    }
    protected void ddColorCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddColorCategory.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddColor, "ProductID", "ItemName");
        StockDetails();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtInputWeight.Text != "")
            {
                RunQuery.SQLQuery.Empty2Zero(txtRejUsed);
                RunQuery.SQLQuery.Empty2Zero(txtRejected);
                RunQuery.SQLQuery.Empty2Zero(txtReUsable);

                decimal availableWeight = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InWeight)-Sum(OutWeight),0) FROM Stock where  ProductID='" + ddColor.SelectedValue + "'"));
                if (Convert.ToDecimal(txtInkConsum.Text) / 1000M <= availableWeight)
                {
                    string value = txtproduced.Text;
                    if (Convert.ToDecimal(txtRejected.Text) >= Convert.ToDecimal(txtReUsable.Text))
                    {
                        if (btnAdd.Text == "Add")
                        {
                            InsertDetailData();

                            Notify("New color impresion added successfully", "success");
                            lblMsg2.Attributes.Add("class", "xerp_success");
                            lblMsg2.Text = "New color impresion added successfully";

                        }
                        else
                        {
                            UpdatePrdnDetail();
                            //RunQuery.SQLQuery.ExecNonQry("Delete PrdOffsetPrintDetails where id='" + lblEid.Text + "'");
                            //InsertDetailData();
                            //Accounting.VoucherEntry.ProductionItemEntry("", "Plastic Container", ddPurpose.SelectedValue, ddSize.SelectedValue, "", ddProduct.SelectedValue, ddProduct.SelectedItem.Text, txtThickness.Text, txtproduced.Text, txtPackPerSheet.Text, txtSheetWeight.Text, txtRejected.Text, Page.User.Identity.Name.ToString());
                            btnAdd.Text = "Add";
                            ClearDetailArea();

                            lblMsg2.Attributes.Add("class", "xerp_success");
                            lblMsg2.Text = "Entry updated successfully";
                        }


                        //if (ddColorCategory.SelectedValue == "271")//Varnish
                        //{
                        txtSheetQty.Text = value;
                        txtFinalOutput.Text = value;

                        //ColorPanel.Visible = false;
                        decimal reusableInputWeight = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT SUM(RejectedUsedKg) from PrdOffsetPrintDetails where PrdnID=''"));

                        txtReusableInputWeight.Text = reusableInputWeight.ToString();
                        string inputWeight = "";

                        if (ddCategoryRaw.SelectedValue == "285") //Save body Packs
                        {
                            txtColorWeightConstant.Text = "1.005";
                            if (txtInputWeight.Text != "")
                            {
                                txtFinalInputKg.Text = Convert.ToString(((reusableInputWeight) + Convert.ToDecimal(txtInputWeight.Text)) * Convert.ToDecimal(txtColorWeightConstant.Text));
                            }
                            pnlTinBodyOutput3.Attributes.Add("class", "hidden");
                            pnlTinBodyOutput2.Attributes.Remove("class");
                        }
                        else
                        {
                            txtColorWeightConstant.Text = "1.003";
                            if (txtFinalOutput.Text != "")
                            {
                                txtFinalOutputKg.Text = Convert.ToString(Convert.ToDecimal(txtFinalOutput.Text) * Convert.ToDecimal(txtWeightSheet.Text) * Convert.ToDecimal(txtColorWeightConstant.Text) / 1000M);
                                inputWeight = Convert.ToString(((reusableInputWeight) + Convert.ToDecimal(txtInputWeight.Text)) * Convert.ToDecimal(txtColorWeightConstant.Text));

                                string nuw = Convert.ToDecimal(Convert.ToDecimal(inputWeight) - Convert.ToDecimal(txtFinalOutputKg.Text)).ToString("##.###"); //RunQuery.SQLQuery.ReturnString("Select SUM(NonUsableWasteWeight) from PrdOffsetPrintDetails where PrdnID=''");
                                lblTotalWeight.Text = "<b>Total Input Weight: </b>" + inputWeight + " kg., Nonusable Weight: " + nuw;
                            }

                            pnlTinBodyOutput2.Attributes.Add("class", "hidden");
                            pnlTinBodyOutput3.Attributes.Remove("class");
                        }

                        //ColorPanel.Visible = false;
                        decimal reusableOutputWeight = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT SUM(ReUsableQty) from PrdOffsetPrintDetails where PrdnID=''"));
                        txtReusableOutput.Text = Convert.ToString((reusableOutputWeight * Convert.ToDecimal(txtWeightSheet.Text) / 1000M) * Convert.ToDecimal(txtColorWeightConstant.Text));
                        
                        txtSheetQty.Text = value;
                        txtSheetQty.Focus();

                        ClearDetailArea();
                    }
                    else
                    {
                        lblMsg2.Attributes.Add("class", "xerp_error");
                        lblMsg2.Text = "Reusable westage quantity can't be greater then rejected sheet quantity.";
                    }
                }
                else
                {
                    Notify("Color Ink is not available into stock", "error");
                    lblMsg2.Attributes.Add("class", "xerp_error");
                    lblMsg2.Text = "Color Ink is not available into stock";
                }
            }
            else
            {
                Notify("Please enter input sheet weight", "error");
                lblMsg2.Attributes.Add("class", "xerp_error");
                lblMsg2.Text = "Please enter input sheet weight";
            }

            //lblMsg2.Attributes.Add("class", "xerp_error");
            //lblMsg2.Text = "Error: Input weight & output weight must have to be equel!";
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error");
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "Error: " + ex.Message.ToString();
        }
    }

    private void InsertDetailData()
    {
        //Non usable stock in KG
        decimal nonUsableWasteWeight = ((Convert.ToDecimal(txtRejected.Text) - Convert.ToDecimal(txtReUsable.Text)) * (Convert.ToDecimal(txtWeightSheet.Text) / 1000M));
        if (nonUsableWasteWeight<0)
        {
            nonUsableWasteWeight = 0;
        }

        decimal consumedPerSheetGm = 0;
        if (txtInkConsum.Text != "0" && txtInkConsum.Text != "")
        {
            consumedPerSheetGm = Convert.ToDecimal(txtInkConsum.Text) / Convert.ToDecimal(txtFinalInput.Text);
        }
        string reUsableQty = Stock.Inventory.NonUsableQty(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue);
        decimal ratio = 0;
        if (reUsableQty != "0")
        {
            ratio = Convert.ToDecimal(Stock.Inventory.NonUsableWeight(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue)) / Convert.ToDecimal(reUsableQty);
        }
            
        decimal rejUsedKg = Convert.ToDecimal(txtRejUsed.Text) * ratio;

        SqlCommand cmd3 = new SqlCommand("INSERT INTO PrdOffsetPrintDetails (Section, PrdnID, InputQty, ReusableItemUsed, RejectedUsed, RejectedUsedKg, FinalInputQty, InkCategory, ColorName, spec, InkConsum, Operator, SheetRejected, NetPrdnSheet, WorkingHr, TimeWaste, Reason, Shift, ReusableItem, ReUsableQty, NonUsableWasteWeight, ConsumedPerSheet, EntryBy)" +
                                                        " VALUES ('Offset Printing', '', '" + txtSheetQty.Text + "', '" + ddReusableUsed.SelectedValue + "', '" + txtRejUsed.Text + "', '" + rejUsedKg + "', '" + txtFinalInput.Text + "', '" + ddColorCategory.SelectedValue + "', '" + ddColor.SelectedValue + "', '" + ddSpec.SelectedValue + "',  '" + txtInkConsum.Text + "', '" + ddOperator.SelectedValue + "', '" + txtRejected.Text + "', '" + txtproduced.Text + "', '" + txtHour.Text + "', '" + txtTimeWaist.Text + "', '" + txtReason.Text + "', '" + ddShift.SelectedValue + "', '" + ddWasteStock.SelectedValue + "', '" + txtReUsable.Text + "', '" + nonUsableWasteWeight + "','" + consumedPerSheetGm + "', @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //SqlCommand cmd3 = new SqlCommand("INSERT INTO PrdOffsetPrintDetails (Section, PrdnID, InputQty, ReusableItemUsed, RejectedUsed, FinalInputQty, InkCategory, ColorName, InkConsum, Operator, SheetRejected, NetPrdnSheet, WorkingHr, TimeWaste, Reason, Shift, ReusableItem, ReUsableQty, NonUsableWasteWeight, EntryBy)" +
        //                                              " VALUES ('Offset Printing', '', '" + txtSheetQty.Text + "', '" + ddReusableUsed.Text + "', '" + txtRejUsed.Text + "', '" + txtFinalInput.Text + "', '" + ddColorCategory.SelectedValue + "', '" + ddColor.SelectedValue + "', '" + txtInkConsum.Text + "', '" + ddOperator.SelectedValue + "', '" + txtRejected.Text + "', '" + txtproduced.Text + "', '" + txtHour.Text + "', '" + txtTimeWaist.Text + "', '" + txtReason.Text + "', '" + ddShift.SelectedValue + "', '" + ddWasteStock.SelectedValue + "', '" + txtReUsable.Text + "', '" + nonUsableWasteWeight + "', @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd3.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        cmd3.Connection.Open();
        cmd3.ExecuteNonQuery();
        cmd3.Connection.Close();

        decimal inkWeightPerPc = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT SUM(ConsumedPerSheet) from PrdOffsetPrintDetails where PrdnID=''")) / 1000M;
        //if (txtFinalOutput.Text != "")
        //{
        //    txtFinalOutputKg.Text = Convert.ToString((Convert.ToDecimal(txtWeightSheet.Text) + inkWeightPerPc) * Convert.ToDecimal(txtFinalOutput.Text)/1000M);
        //}
        txtInkPerSheet.Text = inkWeightPerPc.ToString();
    }

    private void UpdatePrdnDetail()
    {
        //Non usable stock in KG
        decimal nonUsableWasteWeight = ((Convert.ToDecimal(txtRejected.Text) - Convert.ToDecimal(txtReUsable.Text)) * (Convert.ToDecimal(txtWeightSheet.Text) / 1000M));

        decimal consumedPerSheetGm = 0;
        if (txtInkConsum.Text != "0" && txtInkConsum.Text != "")
        {
            consumedPerSheetGm = Convert.ToDecimal(txtInkConsum.Text) / Convert.ToDecimal(txtFinalInput.Text);
        }
        string reUsableQty = Stock.Inventory.NonUsableQty(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue);
        decimal ratio = 0;
        if (reUsableQty != "0")
        {
            ratio = Convert.ToDecimal(Stock.Inventory.NonUsableWeight(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue)) / Convert.ToDecimal(reUsableQty);
        }

        decimal rejUsedKg = Convert.ToDecimal(txtRejUsed.Text) * ratio;

        string cmd3 = "UPDATE PrdOffsetPrintDetails SET InputQty='" + txtSheetQty.Text + "', ReusableItemUsed='" +
                ddReusableUsed.SelectedValue + "', RejectedUsed='" + txtRejUsed.Text + "', RejectedUsedKg='" + rejUsedKg +
                "', FinalInputQty='" + txtFinalInput.Text + "', InkCategory='" + ddColorCategory.SelectedValue +
                "', ColorName='" + ddColor.SelectedValue + "', spec='" + ddSpec.SelectedValue + "', InkConsum='" +
                txtInkConsum.Text + "', Operator='" + ddOperator.SelectedValue + "', SheetRejected='" + txtRejected.Text +
                "', NetPrdnSheet='" + txtproduced.Text + "', WorkingHr='" + txtHour.Text + "', TimeWaste='" +
                txtTimeWaist.Text + "', Reason='" + txtReason.Text + "', Shift='" + ddShift.SelectedValue +
                "', ReusableItem='" + ddWasteStock.SelectedValue + "', ReUsableQty='" + txtReUsable.Text +
                "', NonUsableWasteWeight='" + nonUsableWasteWeight + "', ConsumedPerSheet='" + consumedPerSheetGm + "' WHERE Id='" + lblEntryId.Text+ "'";
       RunQuery.SQLQuery.ExecNonQry(cmd3);
        decimal inkWeightPerPc = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT SUM(ConsumedPerSheet) from PrdOffsetPrintDetails where PrdnID=''")) / 1000M;
        //if (txtFinalOutput.Text != "")
        //{
        //    txtFinalOutputKg.Text = Convert.ToString((Convert.ToDecimal(txtWeightSheet.Text) + inkWeightPerPc) * Convert.ToDecimal(txtFinalOutput.Text)/1000M);
        //}
        txtInkPerSheet.Text = inkWeightPerPc.ToString();
    }

    private void ClearDetailArea()
    {
        txtRejUsed.Text = "";
        txtFinalInput.Text = "";
        txtInkConsum.Text = "";
        txtRejected.Text = "";
        txtproduced.Text = "";
        txtHour.Text = "";
        txtTimeWaist.Text = "";
        txtReason.Text = "";
        txtReUsable.Text = "";
        GridView2.DataBind();
    }
    private void ClearMasterArea()
    {
        ClearDetailArea();
        //txtFor.Text = "";
        //txtInputQtyKG.Text = "";
        txtHour.Text = "";
        txtTimeWaist.Text = "";
        txtReason.Text = "";
        //txtFinalProd.Text = "";
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
            lblEntryId.Text = lblItemName.Text;

            EditMode(lblItemName.Text);
            btnAdd.Text = "Update";
            StockDetails();

            Notify("Edit mode activated ...", "info");
            lblMsg2.Attributes.Add("class", "xerp_info");
            lblMsg2.Text = "Edit mode activated ...";
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error");
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    private void EditMode(string entryID)
    {
        SqlCommand cmd = new SqlCommand(@"SELECT InputQty, RejectedUsed, FinalInputQty, InkCategory, ColorName, InkConsum, Operator, SheetRejected, NetPrdnSheet, WorkingHr, 
                         TimeWaste, Reason, Shift, ReusableItem, ReUsableQty FROM [PrdOffsetPrintDetails] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            txtSheetQty.Text = dr[0].ToString();
            txtRejUsed.Text = dr[1].ToString();
            txtFinalInput.Text = dr[2].ToString();

            //string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID=(SELECT GradeID FROM [Categories] where CategoryID=(Select CategoryID from Products where ProductID='" + pid + "') ) ORDER BY [CategoryName]";
            //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddColorCategory, "CategoryID", "CategoryName");

            //ddColorCategory.SelectedValue = RunQuery.SQLQuery.ReturnString("Select CategoryID from Products where ProductID='" + pid + "'");
            //LoadFinishedDD();
            //ddColor.SelectedValue = pid;
            ddColorCategory.SelectedValue = dr[3].ToString();

            string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddColorCategory.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddColor, "ProductID", "ItemName");

            ddColor.SelectedValue = dr[4].ToString();
            ddSpec.DataBind();
            ddSpec.SelectedValue = RunQuery.SQLQuery.ReturnString(@"SELECT Spec FROM [PrdOffsetPrintDetails] WHERE Id='" + entryID + "'");

            txtInkConsum.Text = dr[5].ToString();
            ddOperator.SelectedValue = dr[6].ToString();
            txtRejected.Text = dr[7].ToString();

            txtproduced.Text = dr[8].ToString();
            txtHour.Text = dr[9].ToString();
            txtTimeWaist.Text = dr[10].ToString();
            txtReason.Text = dr[11].ToString();
            ddShift.SelectedValue = dr[12].ToString();
            ddWasteStock.SelectedValue = dr[13].ToString();
            txtReUsable.Text = dr[14].ToString();
        }
        cmd.Connection.Close();

    }
    protected void GridView2_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView2.Rows[index].FindControl("Label1") as Label;

            SqlCommand cmd7 = new SqlCommand("DELETE PrdOffsetPrintDetails WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();

            GridView2.DataBind();

            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "Entry deleted successfully ...";
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error");
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    protected void GridView2_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            //    string ttlWeight = RunQuery.SQLQuery.ReturnString("SELECT SUM(RejectedUsedKg) from PrdOffsetPrintDetails where PrdnID=''"); 
            //    //RunQuery.SQLQuery.ReturnString("Select SUM(TotalWeight)+SUM(WaistWeight) from PrdOffsetPrintDetails where PrdnID='' ");

            //if (ttlWeight !="")
            //{
            //    lblTotalWeight.Text = "<b>Total Output Weight: </b>" + ttlWeight + " kg.";
            //}

            //txtTotalPack.Text = RunQuery.SQLQuery.ReturnString("Select SUM(PrdnPcs) from PrdOffsetPrintDetails where PrdnID='' ");
        }
        catch (Exception ex)
        {
            //throw;
            Notify(ex.Message.ToString(), "error");
        }
    }

    protected void ddCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand.Items.Clear();
        ListItem lst = new ListItem("--- all ---", "");
        ddBrand.Items.Insert(ddBrand.Items.Count, lst);
        ddBrand.DataBind();
    }

    protected void ddColorCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //ddColor.DataBind();
        LoadColorDD();
    }

    protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddSpec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddReusableUsed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }
    protected void ddColor_SelectedIndexChanged1(object sender, EventArgs e)
    {
        ddSpec.DataBind();
        StockDetails();
    }

    protected void ddOutGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");

        GetPrintedSheetList();
    }

    protected void ddOutCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetPrintedSheetList();
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            string isExist = RunQuery.SQLQuery.ReturnString("Select ProductionID FROM PrdOffsetPrint WHERE pid='" + lblItemCode.Text + "'");

            SQLQuery.ExecNonQry("Delete PrdOffsetPrint where  ProductionID ='" + isExist + "'   ");
            SQLQuery.ExecNonQry("Delete PrdOffsetPrintDetails where   PrdnID='" + isExist + "'   ");
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
    protected void ddGodown_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocation.DataBind();
        StockDetails();
    }
}
