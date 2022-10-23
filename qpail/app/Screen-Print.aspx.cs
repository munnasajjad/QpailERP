using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using RunQuery;
using Control = System.Web.UI.Control;
using Label = System.Web.UI.WebControls.Label;
using TextBox = System.Web.UI.WebControls.TextBox;

public partial class app_Screen_Print : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Today.Date.ToShortDateString();
            string lName = Page.User.Identity.Name.ToString();
            lblProject.Text = SQLQuery.ReturnString("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'");

            ddPurpose.DataBind();
            ddCustomer.DataBind();
            ddBrand.DataBind();
            //LoadColorDd();
            populateddSubGrp();
            PopulateddGradeRaw();
            ddGradeRaw.SelectedValue = "40";
            PopulateddCategoryRaw();
            PopulateddOutSubGroup();
            PopulateddOutGrade();
            PopulateddOutCategory();
            GetProductListRaw();
            //GetProductListInk();
            GetProductListFinished();
            ddSemifinishedPackSize.DataBind();
            StockDetails();
            StockDetailsInput();
            ddWastageGodown.DataBind();
            string wastageGodownLocation = SQLQuery.ReturnString("SELECT WastageGodownLocationId FROM Sections WHERE SName = 'Screen Printing'");
            ddWastageGodownLocation.SelectedValue = wastageGodownLocation;
            //Bind2ndGrid();
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

    private void populateddSubGrp()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] WHERE CategoryID='16' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");
    }
    private void PopulateddGradeRaw()
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        SQLQuery.PopulateDropDown(gQuery, ddGradeRaw, "GradeID", "GradeName");
    }
    private void PopulateddCategoryRaw()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");
    }
    private void PopulateddOutSubGroup()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] WHERE CategoryID='16' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        SQLQuery.PopulateDropDown(gQuery, ddOutSubGroup, "CategoryID", "CategoryName");
    }
    private void PopulateddOutGrade()
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='16' ORDER BY [GradeName]";
        SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");
    }
    private void PopulateddOutCategory()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");
    }

    private void GetProductListRaw()
    {
        //string gQuery = "SELECT ProductID, CONCAT(ItemName,'-',ProductID) AS ItemName FROM [Products] WHERE CategoryID IN (SELECT CategoryID FROM [Categories] WHERE GradeID='" + ddGradeRaw.SelectedValue + "') AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        //string gQuery = "SELECT P.ProductID, CONCAT(P.ItemName,'-',P.ProductID, '(',C.CategoryName,')') AS ItemName FROM Products AS P INNER JOIN Categories AS C ON P.CategoryID = C.CategoryID INNER JOIN ItemGrade AS G ON G.GradeID = C.GradeID WHERE G.GradeID='" + ddGradeRaw.SelectedValue + "' AND P.ProjectID='" + lblProject.Text + "' ORDER BY P.ItemName";
        string gQuery = "SELECT ProductID, CombinedItemName, ItemName, GradeID, TransactionType, InType FROM FifoItemColorBrandSizeView WHERE GradeID = '" + ddGradeRaw.SelectedValue + "' AND InType = 'ProcessedItem' AND ProjectID='" + lblProject.Text + "' ORDER BY CombinedItemName";
        SQLQuery.PopulateDropDown(gQuery, ddItemNameRaw, "ProductID", "CombinedItemName");
        //txtReusableItem.Text = ddItemNameRaw.SelectedItem.Text + " " + ddSemifinishedPackSize.SelectedItem.Text;
        //SQLQuery.PopulateDropDown("SELECT P.ProductID, CONCAT(P.ItemName,'-',P.ProductID, '(',C.CategoryName,')') AS ItemName FROM Products AS P INNER JOIN Categories AS C ON P.CategoryID = C.CategoryID INNER JOIN ItemGrade AS G ON G.GradeID = C.GradeID WHERE P.ProductID='" + ddItemNameRaw.SelectedValue + "' AND P.ProjectID='" + lblProject.Text + "' ORDER BY P.ItemName", ddOutItem, "ProductID", "ItemName");
        StockDetails();
    }
    private void GetProductListFinished()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] WHERE CategoryID='" + ddOutCategory.SelectedValue + "' ORDER BY [ItemName]";
        SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
        StockDetails();
    }

    private void StockDetailsInput()
    {
        decimal inputAvailable = Stock.Inventory.AvailableProcessedItemQuantity(ddItemNameRaw.SelectedValue, "ProcessedItem", ddSemifinishedPackSize.SelectedValue, ddColor2.SelectedValue);
        decimal inputAvailableKg = Stock.Inventory.AvailableProcessedItemWeight(ddItemNameRaw.SelectedValue, "ProcessedItem", ddSemifinishedPackSize.SelectedValue, ddColor2.SelectedValue);

        ltrCurrentStock.Text = "Available Stock: " + inputAvailable + " PCS, " + inputAvailableKg + "KG";
        //ltrReUsable.Text = "Available Stock: " + Stock.Inventory.NonUsableQty(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " pc. " + Stock.Inventory.NonUsableWeight(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " kg";
        //ltrColorStock.Text = "Available Stock: " + Stock.Inventory.AvailableInkWeight(ddColor.SelectedValue, ddSpec.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " kg";
    }

    private bool VerifyStockInFifo()
    {
        decimal pcsAvailableInFifo = Stock.Inventory.AvailableProcessedItemQuantity(ddItemNameRaw.SelectedValue, "ProcessedItem", ddSemifinishedPackSize.SelectedValue, ddColor2.SelectedValue);
        if (Convert.ToDecimal(txtInputQtyPCS.Text) > pcsAvailableInFifo)
        { return false; }
        return true;
    }

    private void StockDetails()
    {

        decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableProcessedQty(ddPurpose.SelectedValue, "", ddItemNameRaw.SelectedValue, ddInputCustomer.SelectedValue, ddInputBrand.SelectedValue, ddSize.SelectedValue, ddColor2.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
        decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableProcessedWeight(ddPurpose.SelectedValue, "", ddItemNameRaw.SelectedValue,
                    ddInputCustomer.SelectedValue, ddInputBrand.SelectedValue, ddSize.SelectedValue, ddColor2.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

        ltrCurrentStock.Text = "Available Stock: " + inputAvailable + " PCS, " + inputAvailableKg + "KG";

        //if (inputAvailable > 0 && inputAvailableKg > 0)
        //{
        //    txtWeightSheet.Text = Convert.ToDecimal(inputAvailableKg / inputAvailable * 1000M).ToString("##.###");
        //    if (txtInputQtyPCS.Text != "")
        //    {
        //        txtInputWeight.Text = Convert.ToString(Convert.ToDecimal(txtInputQtyPCS.Text) * Convert.ToDecimal(txtWeightSheet.Text) / 1000M);
        //    }
        //}
    }

    //Notify Alerts
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
            //decimal inputAvailable = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock WHERE  ProductID='" + ddItemNameRaw.SelectedValue + "'"));
            //string ttlWeight = SQLQuery.ReturnString("Select SUM(TotalWeight)+SUM(WaistWeight) from PrdScreenPrintDetails WHERE PrdnID='' AND Department='Shearing'");

            decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableProcessedQty(ddPurpose.SelectedValue, "", ddItemNameRaw.SelectedValue,
                        ddInputCustomer.SelectedValue, ddInputBrand.SelectedValue, ddSize.SelectedValue, ddColor2.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
            decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableProcessedWeight(ddPurpose.SelectedValue, "", ddItemNameRaw.SelectedValue,
                        ddInputCustomer.SelectedValue, ddInputBrand.SelectedValue, ddSize.SelectedValue, ddColor2.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

            //if ((inputAvailable >= Convert.ToDecimal(txtInputQtyPCS.Text) && inputAvailableKg >= Convert.ToDecimal(txtInputWeight.Text)) || Stock.Inventory.StockEnabled() == "0")
            //{

            if (btnSave.Text == "Save")
            {
                if (VerifyStockInFifo())
                {
                    SaveProduction();
                    //SQLQuery.ExecNonQry("UPDATE PrdScreenPrintDetails SET PrdnID='" + lblPrId.Text +
                    //                             "' WHERE PrdnID='' AND Entryby='" + User.Identity.Name + "'");

                    UpdateProduction();
                    StockDetails();

                    string pdt = txtDate.Text;
                    txtDate.Text = pdt;
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Production saved successfully";
                    Notify("Production saved successfully", "success");
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Error: Input quantity is not available in FIFO!";
                }

            }
            else
            {
                SQLQuery.ExecNonQry("Delete  Stock WHERE InvoiceID ='" + lblPrId.Text + "'   ");
                SQLQuery.ExecNonQry("UPDATE tblFifo SET OutType = '', OutTypeId = '', OutValue = '" + 0 + "' WHERE OutTypeId = '" + lblPrId.Text + "'");
                SQLQuery.ExecNonQry("DELETE tblFifo WHERE InTypeId = '" + lblPrId.Text + "'");
                if (VerifyStockInFifo())
                {
                    UpdateProduction();
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Entry updated successfully";
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Error: Input quantity is not available in FIFO!";
                }
            }
            ClearControls(Form);
            ClearMasterArea();
            //}
            //else
            //{
            //    lblMsg.Attributes.Add("class", "xerp_error");
            //    lblMsg.Text = "Error: Input quantity is not available into stock!";
            //}
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error");
            //lblMsg2.Attributes.Add("class", "xerp_warning");
            //lblMsg2.Text = "Error: " + ex.ToString();
        }
    }

    private void SaveProduction()
    {
        string lName = Page.User.Identity.Name.ToString();
        string prdId = SQLQuery.ReturnString("SELECT 'PRD-SP-'+ CONVERT(varchar, (ISNULL (MAX(pid),0)+1001 )) FROM PrdScreenPrint");
        lblPrId.Text = prdId;

        SqlCommand cmd2 = new SqlCommand("INSERT INTO PrdScreenPrint (ProductionID, EntryBy,RejHandleQty) VALUES (@ProductionID, @EntryBy,@RejHandleQty)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductionID", prdId);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@RejHandleQty", txtHandleReject.Text);
        //cmd2.Parameters.AddWithValue("@FinishGoodsQty", txtFinishGoods.Text);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

    }

    private void UpdateProduction()
    {
        //string query = " WHERE ProductionID='" + lblPrId.Text + "' AND EntryBy='" + User.Identity.Name + "' ";
        //if (btnSave.Text == "Update")
        //{
        string query = " WHERE ProductionID='" + lblPrId.Text + "' ";
        //}
        SQLQuery.Empty2Zero(txtInputQtyPCS);
        SQLQuery.Empty2Zero(txtWeightPc);
        SQLQuery.Empty2Zero(txtInputWeight);
        SQLQuery.Empty2Zero(txtFinalOutput);
        SQLQuery.Empty2Zero(txtFinalOutputKg);
        decimal unitWeightForPrintingOutput = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(MAX(InKg), 0) FROM tblFifo WHERE ItemCode='" + ddItemNameRaw.SelectedValue + "' AND GodownId='" + ddGodown.SelectedValue + "' AND LocationId = '" + ddLocation.SelectedValue +
                                                                     "' AND OutTypeId = ''"));
        SQLQuery.ExecNonQry(@"Update PrdScreenPrint SET Date='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") +
                                     "', Purpose='" + ddPurpose.SelectedValue + "', CustomerID='" + ddCustomer.SelectedValue +
                                     "', Brand='" + ddBrand.SelectedValue + "', PackSize='" + ddSize.SelectedValue + "' , ItemCompany='" + ddInputCustomer.SelectedValue + "' , ItemBrand='" + ddInputBrand.SelectedValue + "' , ItemSubGroup='" + ddSubGrp.SelectedValue + "' , ItemGrade='" + ddGradeRaw.SelectedValue + "' , " +
                         " ItemCategory='" + ddCategoryRaw.SelectedValue + "' , ItemName='" + ddItemNameRaw.SelectedValue + "' , ItemColor='" + ddColor2.SelectedValue + "' , InputQty='" + Convert.ToInt32(txtInputQtyPCS.Text) + "' , WeightPerPack='" + Convert.ToDecimal(txtWeightPc.Text) + "' , InputWeightKg='" + Convert.ToDecimal(txtInputWeight.Text) +
                         "', OutputSubGroup='" + ddOutSubGroup.SelectedValue + "' , OutputGrade='" + ddOutGrade.SelectedValue + "' , OutputCategory='" + ddOutCategory.SelectedValue + "' , OutputItem='" + ddOutItem.SelectedValue + "' , OutPutColor='" + ddOutputColor.SelectedValue + "' , FinalOutput='" + Convert.ToInt32(txtFinalOutput.Text) + "' , FinalOutputKg='" + Convert.ToDecimal(txtFinalOutput.Text) * unitWeightForPrintingOutput + "' , " +
                         " Remarks ='" + txtRemark.Text + "' " + query + " ");

        //Item Stock Entry
        SQLQuery.ExecNonQry("DELETE Stock WHERE InvoiceID='" + lblPrId.Text + "'");
        decimal price = 0;// Inventory.LastNonprintedPrice(Purpose, itemType, Item);
        //Input Item stock-out
        Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production-Screen Printing", lblPrId.Text, ddSize.SelectedValue, ddInputCustomer.SelectedValue, ddInputBrand.SelectedValue, ddColor2.SelectedValue, "", ddItemNameRaw.SelectedValue, ddItemNameRaw.SelectedItem.Text, "", ddGodown.SelectedValue, ddLocation.SelectedValue, "3", Convert.ToInt32(txtFinalOutput.Text), 0, price, 0, Convert.ToDecimal(txtInputWeight.Text), "", "Stock-in", "Production", "Screen Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        try
        {
            //Stock.Inventory.FifoInsert(ddItemNameRaw.SelectedValue, "Production-ScreenPrinting", ddSize.SelectedValue, ddColor.SelectedValue, ddBrand.SelectedValue, txtDate.Text, "SemiFinished", lblPrId.Text, 0, Convert.ToDecimal(txtFinishGoods.Text), 0, DateTime.Now.ToString("yyyy-MM-dd"), "", "", "0");
            //Stock.Inventory.FifoInsert(ddItemNameRaw.SelectedValue, "Production-ScreenPrinting", ddSize.SelectedValue, ddColor.SelectedValue,ddBrand.SelectedValue, txtDate.Text, "SemiFinished", lblPrId.Text, 0, Convert.ToDecimal(txtFinishGoods.Text), 0,DateTime.Now.ToString("yyyy-MM-dd"), "", "", "0");
            SQLQuery.Empty2Zero(txtCrushingWastageQuantity);
            SQLQuery.Empty2Zero(txtReusableItemQuantity);
            decimal totalProcessedItemConsumedAmount = Stock.Inventory.FifoInsert(ddItemNameRaw.SelectedValue, "Production-ScreenPrinting", "", ddSize.SelectedValue, ddColor2.SelectedValue, ddBrand.SelectedValue, txtDate.Text, "", "", 0, Convert.ToDecimal(txtFinalOutput.Text), (Convert.ToDecimal(txtInputQtyPCS.Text) - Convert.ToDecimal(txtReusableItemQuantity.Text)), "", "", DateTime.Now.ToString("yyyy-MM-dd"), "Stock Out", lblPrId.Text, "0");
            decimal screenPrintPerPiecePrice = totalProcessedItemConsumedAmount / Convert.ToDecimal(txtFinalOutput.Text) + Convert.ToDecimal(txtCrushingWastageQuantity.Text);
            decimal unitWeight = Convert.ToDecimal(SQLQuery.ReturnString("SELECT InKg FROM tblFifo WHERE OutTypeId = '" + lblPrId.Text + "'"));

            Stock.Inventory.FifoInsert(ddOutItem.SelectedValue, "Production-ScreenPrinting", ddCustomer.SelectedValue, ddSize.SelectedValue, ddColor2.SelectedValue, ddBrand.SelectedValue, txtDate.Text, "SemiFinished", lblPrId.Text, screenPrintPerPiecePrice, unitWeight * Convert.ToDecimal(txtFinalOutput.Text), Convert.ToDecimal(txtFinalOutput.Text), "", "", DateTime.Now.ToString("yyyy-MM-dd"), "", "", "0");
            
            if (Convert.ToDecimal(txtCrushingWastageQuantity.Text) > 0)
            {
                Stock.Inventory.FifoInsert(ddCrushingWastageItem.SelectedValue, "CrushingWastage-ScreenPrinting", ddCustomer.SelectedValue, ddSize.SelectedValue, ddColor2.SelectedValue, ddBrand.SelectedValue, txtDate.Text, "ReusableWastage", lblPrId.Text, screenPrintPerPiecePrice, unitWeight * Convert.ToDecimal(txtCrushingWastageQuantity.Text), Convert.ToDecimal(txtCrushingWastageQuantity.Text), "", "", DateTime.Now.ToString("yyyy-MM-dd"), "", "", "0");
                Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "CrushingWastage-ScreenPrinting", lblPrId.Text, ddSize.SelectedValue,
                    ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ddCrushingWastageItem.SelectedValue,
                    Stock.Inventory.GetProductName(ddCrushingWastageItem.SelectedValue), "", ddWastageGodown.SelectedValue, ddWastageGodownLocation.SelectedValue, "", Convert.ToDecimal(txtCrushingWastageQuantity.Text), 0, 0,
                    Convert.ToDecimal(txtCrushingWastageQuantity.Text), 0, "", "Stock-In", "Wastage Item", "Plastic Container", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            }
            //Stock.Inventory.FifoInsert(ddOutItem.SelectedValue, "Production-ScreenPrinting", ddSize.SelectedValue, ddColor2.SelectedValue, ddBrand.SelectedValue, txtDate.Text, "SemiFinished", lblPrId.Text, screenPrintPerPiecePrice, unitWeight, Convert.ToDecimal(txtFinishGoods.Text), DateTime.Now.ToString("yyyy-MM-dd"), "", "", "0");

            //Auto voucher entry
            string description = "Screen Printing Production ID# " + lblPrId.Text + "  ";
            Accounting.VoucherEntry.AutoVoucherEntry("3", description, "010106001", "010106001", totalProcessedItemConsumedAmount, lblPrId.Text, "", Page.User.Identity.Name, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "1");

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        //Color Imression stock-in/out
        //DataTable citydt = SQLQuery.ReturnDataTable(@"SELECT InputQty, ReusableItemUsed, RejectedUsed, FinalInputQty, InkCategory, 
        //                ColorName, Spec, InkConsum, SheetRejected, NetPrdnSheet, godown, LocationID,
        //                 WastePcsItem, WastePcs, WasteKgItem, WasteKg FROM PrdScreenPrintDetails WHERE PrdnID='" + lblPrId.Text + "'");

        //foreach (DataRow citydr in citydt.Rows)
        //{
        //    string Purpose = ddPurpose.SelectedValue;
        //    string InputQty = citydr["InputQty"].ToString();
        //    string ReusableItemUsed = citydr["ReusableItemUsed"].ToString();
        //    string RejectedUsed = citydr["RejectedUsed"].ToString();
        //    string FinalInputQty = citydr["FinalInputQty"].ToString();

        //    string InkCategory = citydr["InkCategory"].ToString();
        //    string ColorName = citydr["ColorName"].ToString();
        //    string Spec = citydr["Spec"].ToString();
        //    string InkConsum = citydr["InkConsum"].ToString();
        //    string SheetRejected = citydr["SheetRejected"].ToString();

        //    string godown = citydr["godown"].ToString();
        //    string locationId = citydr["LocationID"].ToString();
        //    string NetPrdnSheet = citydr["NetPrdnSheet"].ToString();
        //    string WastePcsItem = citydr["WastePcsItem"].ToString();
        //    string WastePcs = citydr["WastePcs"].ToString();
        //    string WasteKgItem = citydr["WasteKgItem"].ToString();
        //    string WasteKg = citydr["WasteKg"].ToString();

        //    decimal ruWeight = Convert.ToDecimal(Stock.Inventory.NonUsableQty(ReusableItemUsed, ddGodown.SelectedValue, ddLocation.SelectedValue));
        //    if (ruWeight > 0)
        //    {
        //        ruWeight = Convert.ToDecimal(RejectedUsed) * Convert.ToDecimal(Stock.Inventory.NonUsableWeight(ReusableItemUsed, ddGodown.SelectedValue, ddLocation.SelectedValue)) / ruWeight;
        //    }

        //    //Wastage used
        //    Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production-Screen Printing", lblPrId.Text, "",
        //        ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ReusableItemUsed, Stock.Inventory.GetProductName(ReusableItemUsed), "", ddGodown.SelectedValue, ddLocation.SelectedValue,
        //        "7", 0, Convert.ToInt32(RejectedUsed), price, 0, ruWeight, "", "Stock-Out", "Wastage used", "Screen Printing",
        //        Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        //    //Ink Consumption
        //    Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production-Screen Printing", lblPrId.Text, "",
        //        ddCustomer.SelectedValue, ddBrand.SelectedValue, "", Spec, ColorName,
        //        Stock.Inventory.GetProductName(ColorName), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", 0, 0, price, 0, Convert.ToDecimal(InkConsum) / 1000M,
        //        "", "Stock-Out", "Ink used", "Screen Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        //    //Stock In wastage
        //    decimal wasteKg = Convert.ToDecimal(txtWeightPc.Text) * Convert.ToDecimal(WastePcs) / 1000M;
        //    Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production-Screen Printing", lblPrId.Text, "",
        //        ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", WastePcsItem,
        //        Stock.Inventory.GetProductName(WastePcsItem), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", Convert.ToInt32(WastePcs), 0, price,
        //        wasteKg, 0, "", "Stock-In", "Wastage Item", "Screen Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        //    Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production-Screen Printing", lblPrId.Text, "",
        //        ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", WasteKgItem,
        //        Stock.Inventory.GetProductName(WasteKgItem), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", 0, 0, price,
        //        Convert.ToDecimal(WasteKg), 0, "", "Stock-In", "Wastage Item", "Screen Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        //}

        //decimal outputWeight = Convert.ToDecimal(txtFinalOutputKg.Text); //Convert.ToInt32(txtTotalPack.Text)*Convert.ToDecimal(txtWeightPerPack);
        //Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Screen Printing", lblPrId.Text, ddSize.SelectedValue, ddCustomer.SelectedValue, ddBrand.SelectedValue, ddOutputColor.SelectedValue, "", ddOutItem.SelectedValue, ddOutItem.SelectedItem.Text, "", "8", "", "3", Convert.ToInt32(txtFinishGoods.Text), 0, price, Convert.ToDecimal(txtFinalOutputKg.Text), 0, "", "Stock-in", "Production", "Screen Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

    }

    protected void ddSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddOperator.DataBind();
    }

    protected void ddGradeRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");
        ////SQLQuery.PopulateDropDown(gQuery, ddColorCategory, "CategoryID", "CategoryName");

        GetProductListRaw();
        StockDetailsInput();
        txtReusableItem.Text = ddItemNameRaw.SelectedItem.Text + " " + ddSemifinishedPackSize.SelectedItem.Text;
        //ddOutGrade.SelectedValue = ddGradeRaw.SelectedValue;
    }
    protected void ddCategoryRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GetProductListRaw();

    }
    protected void ddItemNameRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
        StockDetailsInput();
        txtReusableItem.Text = ddItemNameRaw.SelectedItem.Text + " " + ddSemifinishedPackSize.SelectedItem.Text;
        //string gQuery = "SELECT P.ProductID, CONCAT(P.ItemName,'-',P.ProductID, '(',C.CategoryName,')') AS ItemName FROM Products AS P INNER JOIN Categories AS C ON P.CategoryID = C.CategoryID INNER JOIN ItemGrade AS G ON G.GradeID = C.GradeID WHERE P.ProductID='" + ddItemNameRaw.SelectedValue + "' AND P.ProjectID='" + lblProject.Text + "' ORDER BY P.ItemName";
        //SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
    }


    private void ClearMasterArea()
    {
        btnSave.Text = "Save";
        GridView1.DataBind();
        lblPrId.Text = "";
        txtRemark.Text = "";
        GridView1.DataBind();
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

    protected void ddInputCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddInputBrand.Items.Clear();
        ListItem lst = new ListItem("--- all ---", "");
        ddInputBrand.Items.Insert(ddInputBrand.Items.Count, lst);
        //ddBrand.Items.Add("--- all ---");
        ddInputBrand.DataBind();
        StockDetails();
    }

    private string CalcReusableWeight()
    {
        string remain = SQLQuery.ReturnString("Select ISNULL((SheetRejected),0)-ISNULL((WastePcs),0) from  PrdScreenPrintDetails WHERE PrdnID=''");
        decimal value = Convert.ToDecimal(txtWeightPc.Text) * Convert.ToDecimal(remain) / 1000M;
        return value.ToString();
    }

    protected void ddOutGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddOutGrade.SelectedValue + "'  ORDER BY [CategoryName]";
        SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");
        GetProductListRaw();
    }

    protected void ddOutCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetProductListFinished();
    }

    protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }
    protected void ddBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }
    protected void ddSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddSpec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }
    protected void ddColor2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //StockDetails();
        StockDetailsInput();
        ddOutputColor.SelectedValue = ddColor2.SelectedValue;

    }

    protected void ddReusableUsed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddInputBrand_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // StockDetails();
    }

    //Message & Notify For Alerts
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

            string isExist = SQLQuery.ReturnString("Select ProductionID FROM PrdScreenPrint WHERE pid='" + lblItemCode.Text + "'");

            SQLQuery.ExecNonQry("Delete PrdScreenPrint WHERE ProductionID ='" + isExist + "'   ");
            SQLQuery.ExecNonQry("Delete PrdScreenPrintDetails WHERE PrdnID='" + isExist + "'   ");
            SQLQuery.ExecNonQry("Delete  Stock WHERE InvoiceID ='" + isExist + "'   ");
            //SQLQuery.ExecNonQry("Delete  WHERE   ='" + isExist + "'   ");

            GridView1.DataBind();
            Notify("Info deleted!", "warn", lblMsg);
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }
    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
            lblEditId.Text = lblItemName.Text;
            btnSave.Text = "Update";
            EditMode();

            lblMsg.Attributes.Add("class", "xerp_info");
            lblMsg.Text = "Edit mode activated ...";
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }

    private void EditMode()
    {
        string itemToEdit = lblEditId.Text;

        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) pid, Section, ProductionID, Date, MachineNo, LineNumber, Purpose, CustomerID, Brand, PackSize, 
        ItemCompany, ItemBrand, ItemSubGroup, ItemGrade, ItemCategory, ItemName, ItemColor, InputQty,WeightPerPack, InputWeightKg, OutputSubGroup, OutputGrade, OutputCategory, OutputItem, OutPutColor, FinalOutput, FinalOutputKg, Remarks, EntryBy, EntryDate
        FROM PrdScreenPrint WHERE (pid = '" + itemToEdit + "')");
        foreach (DataRow drx in dtx.Rows)
        {
            lblPrId.Text = drx["ProductionID"].ToString();
            //Bind2ndGrid();
            txtDate.Text = Convert.ToDateTime(drx["Date"].ToString()).ToString("dd/MM/yyyy");

            ddPurpose.SelectedValue = drx["Purpose"].ToString();
            ddCustomer.SelectedValue = drx["CustomerID"].ToString();
            ddBrand.DataBind();
            ddBrand.SelectedValue = drx["Brand"].ToString();
            ddSize.SelectedValue = drx["PackSize"].ToString();

            ddInputCustomer.SelectedValue = drx["ItemCompany"].ToString();
            ddInputBrand.DataBind();
            ddInputBrand.SelectedValue = drx["ItemBrand"].ToString();
            //populateddSubGrp();
            //ddSubGrp.SelectedValue = drx["ItemSubGroup"].ToString();
            PopulateddGradeRaw();
            ddGradeRaw.SelectedValue = drx["ItemGrade"].ToString();
            PopulateddCategoryRaw();
            // ddCategoryRaw.SelectedValue = drx["ItemCategory"].ToString();
            GetProductListRaw();
            ddItemNameRaw.SelectedValue = drx["ItemName"].ToString();
            ddColor2.SelectedValue = drx["ItemColor"].ToString();
            txtInputQtyPCS.Text = drx["InputQty"].ToString();
            txtWeightPc.Text = Convert.ToString(Convert.ToDecimal(drx["WeightPerPack"].ToString()) / 100M);
            txtInputWeight.Text = drx["InputWeightKg"].ToString();

            ddOutSubGroup.SelectedValue = drx["OutputSubGroup"].ToString();
            PopulateddOutGrade();
            ddOutGrade.SelectedValue = drx["OutputGrade"].ToString();
            PopulateddOutCategory();
            // ddOutCategory.SelectedValue = drx["OutputCategory"].ToString();
            GetProductListFinished();
            ddOutItem.SelectedValue = drx["OutputItem"].ToString();
            ddOutputColor.SelectedValue = drx["OutPutColor"].ToString();
            txtFinalOutput.Text = drx["FinalOutput"].ToString();
            txtFinalOutputKg.Text = drx["FinalOutputKg"].ToString();
            txtRemark.Text = drx["Remarks"].ToString();

        }
    }

    protected void ddGodown_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocation.DataBind();
        StockDetails();
    }

    protected void txtFinalOutput_OnTextChanged(object sender, EventArgs e)
    {
        //decimal finishGoods = Convert.ToDecimal(txtFinalOutput.Text) - Convert.ToDecimal(txtHandleReject.Text);
        //txtFinishGoods.Text = finishGoods.ToString();
    }

    protected void txtRejUsed_OnTextChanged(object sender, EventArgs e)
    {
        //decimal finalInputQty = Convert.ToDecimal(txtSheetQty.Text)*Convert.ToDecimal(txtRejUsed.Text);
        //txtFinalInput.Text = finalInputQty.ToString();
    }

    protected void ddLocation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetailsInput();
    }

    protected void ddSemifinishedPackSize_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetailsInput();
        txtReusableItem.Text = ddItemNameRaw.SelectedItem.Text + " " + ddSemifinishedPackSize.SelectedItem.Text;
    }

}
