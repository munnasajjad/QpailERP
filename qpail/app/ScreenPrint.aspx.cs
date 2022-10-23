using System;
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

public partial class app_ScreenPrint : System.Web.UI.Page
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

            ddPurpose.DataBind();
            ddCustomer.DataBind();
            ddBrand.DataBind();
            ddColorCategory.DataBind();
            //LoadColorDd();
            ddSpec.DataBind();

            populateddSubGrp();
            PopulateddGradeRaw();
            PopulateddCategoryRaw();
            PopulateddOutSubGroup();
            PopulateddOutGrade();
            PopulateddOutCategory();
            txtInkConsum.Text = "0";
            txtReUsable.Text = "0";
            txtRejUsed.Text = "0";
            GetProductListRaw();
            GetProductListInk();
            GetProductListFinished();

            StockDetails();
            Bind2ndGrid();
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
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");
    }
    private void PopulateddGradeRaw()
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGradeRaw, "GradeID", "GradeName");
    }
    private void PopulateddCategoryRaw()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");
    }
    private void PopulateddOutSubGroup()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] WHERE CategoryID='16' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutSubGroup, "CategoryID", "CategoryName");
    }
    private void PopulateddOutGrade()
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");
    }
    private void PopulateddOutCategory()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");
    }

    private void GetProductListRaw()
    {
        //string gQuery = "SELECT ProductID, CONCAT(ItemName,'-',ProductID) AS ItemName FROM [Products] WHERE CategoryID IN (SELECT CategoryID FROM [Categories] WHERE GradeID='" + ddGradeRaw.SelectedValue + "') AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        string gQuery = "SELECT P.ProductID, CONCAT(P.ItemName,'-',P.ProductID, '(',C.CategoryName,')') AS ItemName FROM Products AS P INNER JOIN Categories AS C ON P.CategoryID = C.CategoryID INNER JOIN ItemGrade AS G ON G.GradeID = C.GradeID WHERE G.GradeID='" + ddGradeRaw.SelectedValue + "' AND P.ProjectID='" + lblProject.Text + "' ORDER BY P.ItemName";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemNameRaw, "ProductID", "ItemName");
        SQLQuery.PopulateDropDown("SELECT P.ProductID, CONCAT(P.ItemName,'-',P.ProductID, '(',C.CategoryName,')') AS ItemName FROM Products AS P INNER JOIN Categories AS C ON P.CategoryID = C.CategoryID INNER JOIN ItemGrade AS G ON G.GradeID = C.GradeID WHERE P.ProductID='" + ddItemNameRaw.SelectedValue + "' AND P.ProjectID='" + lblProject.Text + "' ORDER BY P.ItemName", ddOutItem, "ProductID", "ItemName");
        StockDetails();
    }
    private void GetProductListInk()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] WHERE CategoryID='" + ddColorCategory.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddColor, "ProductID", "ItemName");

        ddSpec.DataBind();
        StockDetails();
    }

    private void GetProductListFinished()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] WHERE CategoryID='" + ddOutCategory.SelectedValue + "'  ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");

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

        ltrReUsable.Text = "Available Stock: " + Stock.Inventory.NonUsableQty(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " pc. " + Stock.Inventory.NonUsableWeight(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " kg";

        ltrColorStock.Text = "Available Stock: " + Stock.Inventory.AvailableInkWeight(ddColor.SelectedValue, ddSpec.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " kg";

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
            //decimal inputAvailable = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock WHERE  ProductID='" + ddItemNameRaw.SelectedValue + "'"));
            //string ttlWeight = RunQuery.SQLQuery.ReturnString("Select SUM(TotalWeight)+SUM(WaistWeight) from PrdScreenPrintDetails WHERE PrdnID='' AND Department='Shearing'");

            decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableProcessedQty(ddPurpose.SelectedValue, "", ddItemNameRaw.SelectedValue,
                        ddInputCustomer.SelectedValue, ddInputBrand.SelectedValue, ddSize.SelectedValue, ddColor2.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
            decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableProcessedWeight(ddPurpose.SelectedValue, "", ddItemNameRaw.SelectedValue,
                        ddInputCustomer.SelectedValue, ddInputBrand.SelectedValue, ddSize.SelectedValue, ddColor2.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

            if ((inputAvailable >= Convert.ToDecimal(txtInputQtyPCS.Text) && inputAvailableKg >= Convert.ToDecimal(txtInputWeight.Text)) || Stock.Inventory.StockEnabled() == "0")
            {
                if (VerifyStockInFifo())
                {
                    if (btnSave.Text == "Save")
                    {
                        SaveProduction();
                        RunQuery.SQLQuery.ExecNonQry("UPDATE PrdScreenPrintDetails SET PrdnID='" + lblPrId.Text +
                                                     "' WHERE PrdnID='' AND Entryby='" + User.Identity.Name + "'");

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
                        SQLQuery.ExecNonQry("Delete  Stock WHERE InvoiceID ='" + lblPrId.Text + "'   ");
                        SQLQuery.ExecNonQry("UPDATE tblFifo SET OutType = '', OutTypeId = '', OutValue = '" + 0 + "' WHERE OutTypeId = '" + lblPrId.Text + "'");
                        SQLQuery.ExecNonQry("DELETE tblFifo WHERE InTypeId = '" + lblPrId.Text + "'");
                        UpdateProduction();
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Entry updated successfully";
                    }
                    ClearControls(Form);
                    ClearMasterArea();
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Error: Input quantity is not available in FIFO!";
                }
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Error: Input quantity is not available into stock!";
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error");
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "Error: " + ex.ToString();
        }
    }

    private void SaveProduction()
    {
        string lName = Page.User.Identity.Name.ToString();
        string prdId = RunQuery.SQLQuery.ReturnString("Select 'PRD-SP-'+ CONVERT(varchar, (ISNULL (max(pid),0)+1001 )) from PrdScreenPrint");
        lblPrId.Text = prdId;

        SqlCommand cmd2 = new SqlCommand("INSERT INTO PrdScreenPrint (ProductionID, EntryBy,RejHandleQty,FinishGoodsQty) VALUES (@ProductionID, @EntryBy,@RejHandleQty,@FinishGoodsQty)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductionID", prdId);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@RejHandleQty", txtHandleReject.Text);
        cmd2.Parameters.AddWithValue("@FinishGoodsQty", txtFinishGoods.Text);

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
        SQLQuery.ExecNonQry(@"Update PrdScreenPrint SET Date='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") +
                                     "', Purpose='" + ddPurpose.SelectedValue + "', CustomerID='" + ddCustomer.SelectedValue +
                                     "', Brand='" + ddBrand.SelectedValue + "', PackSize='" + ddSize.SelectedValue + "' , ItemCompany='" + ddInputCustomer.SelectedValue + "' , ItemBrand='" + ddInputBrand.SelectedValue + "' , ItemSubGroup='" + ddSubGrp.SelectedValue + "' , ItemGrade='" + ddGradeRaw.SelectedValue + "' , " +
                         " ItemCategory='" + ddCategoryRaw.SelectedValue + "' , ItemName='" + ddItemNameRaw.SelectedValue + "' , ItemColor='" + ddColor2.SelectedValue + "' , InputQty='" + Convert.ToInt32(txtInputQtyPCS.Text) + "' , WeightPerPack='" + Convert.ToDecimal(txtWeightPc.Text) + "' , InputWeightKg='" + Convert.ToDecimal(txtInputWeight.Text) +
                         "', OutputSubGroup='" + ddOutSubGroup.SelectedValue + "' , OutputGrade='" + ddOutGrade.SelectedValue + "' , OutputCategory='" + ddOutCategory.SelectedValue + "' , OutputItem='" + ddOutItem.SelectedValue + "' , OutPutColor='" + ddOutputColor.SelectedValue + "' , FinalOutput='" + Convert.ToInt32(txtFinalOutput.Text) + "' , FinalOutputKg='" + Convert.ToDecimal(txtFinalOutputKg.Text) + "' , " +
                         " Remarks ='" + txtRemark.Text + "' " + query + " ");

        //Item Stock Entry
        RunQuery.SQLQuery.ExecNonQry("DELETE Stock WHERE InvoiceID='" + lblPrId.Text + "'");
        decimal price = 0;// Inventory.LastNonprintedPrice(Purpose, itemType, Item);
        //Input Item stock-out
        Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production-Screen Printing", lblPrId.Text, ddSize.SelectedValue, ddInputCustomer.SelectedValue, ddInputBrand.SelectedValue, ddColor2.SelectedValue, "", ddItemNameRaw.SelectedValue, ddItemNameRaw.SelectedItem.Text, "", ddGodown.SelectedValue, ddLocation.SelectedValue, "3", Convert.ToInt32(txtFinishGoods.Text), 0, price, 0, Convert.ToDecimal(txtInputWeight.Text), "", "Stock-in", "Production", "Screen Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        try
        {
            //Stock.Inventory.FifoInsert(ddItemNameRaw.SelectedValue, "Production-ScreenPrinting", ddSize.SelectedValue, ddColor.SelectedValue, ddBrand.SelectedValue, txtDate.Text, "SemiFinished", lblPrId.Text, 0, Convert.ToDecimal(txtFinishGoods.Text), 0, DateTime.Now.ToString("yyyy-MM-dd"), "", "", "0");
            //Stock.Inventory.FifoInsert(ddItemNameRaw.SelectedValue, "Production-ScreenPrinting", ddSize.SelectedValue, ddColor.SelectedValue,ddBrand.SelectedValue, txtDate.Text, "SemiFinished", lblPrId.Text, 0, Convert.ToDecimal(txtFinishGoods.Text), 0,DateTime.Now.ToString("yyyy-MM-dd"), "", "", "0");

            decimal totalProcessedItemConsumed = Stock.Inventory.FifoInsert(ddItemNameRaw.SelectedValue, "Production-ScreenPrinting",ddCustomer.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddBrand.SelectedValue, txtDate.Text, "", "", 0, Convert.ToDecimal(txtFinishGoods.Text), Convert.ToDecimal(txtInputQtyPCS.Text), ddGodown.SelectedValue, ddLocation.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd"), "Stock Out", lblPrId.Text, "0");
            decimal screenPrintPerPiecePrice = 0; /*totalProcessedItemConsumed / Convert.ToDecimal(txtFinishGoods.Text);*/
            decimal unitWeight = Convert.ToDecimal(SQLQuery.ReturnString("SELECT InKg FROM tblFifo WHERE OutTypeId = '" + lblPrId.Text + "'"));
            Stock.Inventory.FifoInsert(ddOutItem.SelectedValue, "Production-ScreenPrinting",ddCustomer.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddBrand.SelectedValue, txtDate.Text, "SemiFinished", lblPrId.Text, screenPrintPerPiecePrice, unitWeight, Convert.ToDecimal(txtFinishGoods.Text), ddGodown.SelectedValue, ddLocation.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd"), "", "", "0");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        //Color Imression stock-in/out
        DataTable citydt = RunQuery.SQLQuery.ReturnDataTable(@"SELECT InputQty, ReusableItemUsed, RejectedUsed, FinalInputQty, InkCategory, 
                        ColorName, Spec, InkConsum, SheetRejected, NetPrdnSheet, godown, LocationID,
                         WastePcsItem, WastePcs, WasteKgItem, WasteKg FROM PrdScreenPrintDetails WHERE PrdnID='" + lblPrId.Text + "'");

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

            string godown = citydr["godown"].ToString();
            string locationId = citydr["LocationID"].ToString();
            string NetPrdnSheet = citydr["NetPrdnSheet"].ToString();
            string WastePcsItem = citydr["WastePcsItem"].ToString();
            string WastePcs = citydr["WastePcs"].ToString();
            string WasteKgItem = citydr["WasteKgItem"].ToString();
            string WasteKg = citydr["WasteKg"].ToString();

            decimal ruWeight = Convert.ToDecimal(Stock.Inventory.NonUsableQty(ReusableItemUsed, ddGodown.SelectedValue, ddLocation.SelectedValue));
            if (ruWeight > 0)
            {
                ruWeight = Convert.ToDecimal(RejectedUsed) * Convert.ToDecimal(Stock.Inventory.NonUsableWeight(ReusableItemUsed, ddGodown.SelectedValue, ddLocation.SelectedValue)) / ruWeight;
            }

            //Wastage used
            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production-Screen Printing", lblPrId.Text, "",
                ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ReusableItemUsed, Stock.Inventory.GetProductName(ReusableItemUsed), "", ddGodown.SelectedValue, ddLocation.SelectedValue,
                "7", 0, Convert.ToInt32(RejectedUsed), price, 0, ruWeight, "", "Stock-Out", "Wastage used", "Screen Printing",
                Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            //Ink Consumption
            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production-Screen Printing", lblPrId.Text, "",
                ddCustomer.SelectedValue, ddBrand.SelectedValue, "", Spec, ColorName,
                Stock.Inventory.GetProductName(ColorName), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", 0, 0, price, 0, Convert.ToDecimal(InkConsum) / 1000M,
                "", "Stock-Out", "Ink used", "Screen Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            //Stock In wastage
            decimal wasteKg = Convert.ToDecimal(txtWeightPc.Text) * Convert.ToDecimal(WastePcs) / 1000M;
            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production-Screen Printing", lblPrId.Text, "",
                ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", WastePcsItem,
                Stock.Inventory.GetProductName(WastePcsItem), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", Convert.ToInt32(WastePcs), 0, price,
                wasteKg, 0, "", "Stock-In", "Wastage Item", "Screen Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production-Screen Printing", lblPrId.Text, "",
                ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", WasteKgItem,
                Stock.Inventory.GetProductName(WasteKgItem), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", 0, 0, price,
                Convert.ToDecimal(WasteKg), 0, "", "Stock-In", "Wastage Item", "Screen Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        }

        //decimal outputWeight = Convert.ToDecimal(txtFinalOutputKg.Text); //Convert.ToInt32(txtTotalPack.Text)*Convert.ToDecimal(txtWeightPerPack);
        //Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Screen Printing", lblPrId.Text, ddSize.SelectedValue, ddCustomer.SelectedValue, ddBrand.SelectedValue, ddOutputColor.SelectedValue, "", ddOutItem.SelectedValue, ddOutItem.SelectedItem.Text, "", "8", "", "3", Convert.ToInt32(txtFinishGoods.Text), 0, price, Convert.ToDecimal(txtFinalOutputKg.Text), 0, "", "Stock-in", "Production", "Screen Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

    }

    protected void ddColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddSpec.DataBind();
        StockDetails();
    }
    protected void ddSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddOperator.DataBind();
    }

    protected void ddGradeRaw_SelectedIndexChanged(object sender, EventArgs e)
    {

        //string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");
        ////RunQuery.SQLQuery.PopulateDropDown(gQuery, ddColorCategory, "CategoryID", "CategoryName");

        GetProductListRaw();
        StockDetailsInput();
        ddOutGrade.SelectedValue = ddGradeRaw.SelectedValue;


    }
    protected void ddCategoryRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GetProductListRaw();

    }
    protected void ddItemNameRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
        StockDetailsInput();
        string gQuery = "SELECT P.ProductID, CONCAT(P.ItemName,'-',P.ProductID, '(',C.CategoryName,')') AS ItemName FROM Products AS P INNER JOIN Categories AS C ON P.CategoryID = C.CategoryID INNER JOIN ItemGrade AS G ON G.GradeID = C.GradeID WHERE P.ProductID='" + ddItemNameRaw.SelectedValue + "' AND P.ProjectID='" + lblProject.Text + "' ORDER BY P.ItemName";
        SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
    }
    protected void ddColorCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] WHERE CategoryID='" + ddColorCategory.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddColor, "ProductID", "ItemName");
        StockDetails();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            RunQuery.SQLQuery.Empty2Zero(txtRejected);
            decimal availableQty = Convert.ToDecimal(Stock.Inventory.NonUsableQty(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

            if (txtproduced.Text != "" && txtproduced.Text != "0")
            {
                if (availableQty >= Convert.ToDecimal(txtRejUsed.Text))
                {
                    if (btnAdd.Text == "Add")
                    {
                        InsertDetailData();
                        //string value = txtproduced.Text;
                        //txtTotalPack.Text = txtproduced.Text;
                        InputQtyCalculation();
                        ClearDetailArea();
                        lblMsg2.Attributes.Add("class", "xerp_success");
                        lblMsg2.Text = "New color impression added successfully";
                        Notify("New color impression added successfully", "success");

                        if (ddColorCategory.SelectedValue == "271")//Varnish
                        {
                            //txtFinalOutput.Text = value;
                            //ColorPanel.Visible = false;
                            //txtFinalOutput.Focus();
                        }
                        else
                        {
                            //txtSheetQty.Text = value;
                            txtSheetQty.Focus();
                        }

                    }
                    else
                    {
                        //RunQuery.SQLQuery.ExecNonQry("Delete PrdScreenPrintDetails WHERE id='" + lblEid.Text + "'");
                        //InsertDetailData();
                        UpdateDetailData();
                        btnAdd.Text = "Add";
                        ClearDetailArea();

                        lblMsg2.Attributes.Add("class", "xerp_success");
                        lblMsg2.Text = "Entry updated successfully";
                    }
                }
                else
                {
                    Notify("Reusable container is not available into stock!", "error");
                }
            }
            else
            {
                lblMsg2.Attributes.Add("class", "xerp_error");
                lblMsg2.Text = "Please fillup all mendatory fields...";
            }

            Bind2ndGrid();
            //lblMsg2.Attributes.Add("class", "xerp_error");
            //lblMsg2.Text = "Error: Input weight & output weight must have to be equel!";
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error");
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "Error: " + ex.ToString();
        }

    }

    private void InputQtyCalculation()
    {
        //string inputQty = SQLQuery.ReturnString("SELECT (FinalInputQty- SheetRejected) AS ActualQty FROM [PrdScreenPrintDetails] WHERE PrdnID='' AND Section='Offset Printing'  ORDER BY [id]");
        //txtSheetQty.Text = inputQty;
        //decimal finalInput = Convert.ToDecimal(txtSheetQty.Text) + 0;
        //txtFinalInput.Text = finalInput.ToString();

        DataTable dtx = SQLQuery.ReturnDataTable("SELECT Id, InputQty, RejectedUsed, FinalInputQty,SheetRejected, NetPrdnSheet, WorkingHr, TimeWaste,Reason, WastePcs, WasteKg, EntryBy FROM [PrdScreenPrintDetails]  WHERE PrdnID = '' AND Section = 'Offset Printing' ORDER BY[id]");
        foreach (DataRow dtr in dtx.Rows)
        {
            txtSheetQty.Text = dtr["NetPrdnSheet"].ToString();
            txtRejUsed.Text = "0";
            txtFinalInput.Text = (Convert.ToDecimal(txtSheetQty.Text) + Convert.ToDecimal(txtRejUsed.Text)).ToString();

        }
    }

    private void InsertDetailData()
    {
        SQLQuery.Empty2Zero(txtWeightPc);

        SQLQuery.Empty2Zero(txtReUsable);
        decimal remain = Convert.ToDecimal(txtRejected.Text) - Convert.ToDecimal(txtReUsable.Text); //RunQuery.SQLQuery.ReturnString("Select ISNULL((SheetRejected),0)-ISNULL((WastePcs),0) from  PrdScreenPrintDetails WHERE PrdnID=''");
        decimal wasteKg = Convert.ToDecimal(txtWeightPc.Text) * remain / 1000M;

        SQLQuery.Empty2Zero(txtMin);
        decimal hrMin = Convert.ToDecimal(txtHour.Text) + (Convert.ToDecimal(txtMin.Text) / 60);
        SQLQuery.Empty2Zero(txtSheetQty);
        SQLQuery.Empty2Zero(txtRejUsed);
        SQLQuery.Empty2Zero(txtInkConsum);
        SQLQuery.Empty2Zero(txtInkConsum);

        SqlCommand cmd3 = new SqlCommand("INSERT INTO PrdScreenPrintDetails (Section, PrdnID, MachineNo, LineNumber, InputQty, ReusableItemUsed, RejectedUsed, FinalInputQty, InkCategory, ColorName, Spec, InkConsum, Operator, SheetRejected, NetPrdnSheet, WorkingHr, TimeWaste, Reason, Shift, WastePcsItem, WastePcs, WasteKgItem, WasteKg, godown, LocationID, EntryBy)" +
                                                        " VALUES ('Offset Printing', '" + lblPrId.Text + "', '" + ddMachine.SelectedValue +
                                     "', '" + ddLine.SelectedValue + "',  '" + txtSheetQty.Text + "', '" + ddReusableUsed.SelectedValue + "', '" + txtRejUsed.Text + "', '" + txtFinalInput.Text + "', '" + ddColorCategory.SelectedValue + "', '" + ddColor.SelectedValue + "', '" + ddSpec.SelectedValue + "', '" + txtInkConsum.Text + "', '" + ddOperator.SelectedValue + "', '" + txtRejected.Text + "', '" + txtproduced.Text + "', '" + hrMin + "', '" + txtTimeWaist.Text + "', '" + txtReason.Text + "', '" + ddShift.SelectedValue + "','" + ddWasteStock.SelectedValue + "', '" + txtReUsable.Text + "', '" + ddRecycleKgItem.SelectedValue + "', '" + wasteKg + "', '" + ddGodown.SelectedValue + "', '" + ddLocation.SelectedValue + "', @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd3.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        cmd3.Connection.Open();
        cmd3.ExecuteNonQuery();
        cmd3.Connection.Close();

        //RunQuery.SQLQuery.ExecNonQry("Update PrdScreenPrintDetails set WasteKg='" + CalcReusableWeight() + "' WHERE ID=(Select max(id) from PrdScreenPrintDetails)");

        txtFinalOutput.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL((NetPrdnSheet),0) from PrdScreenPrintDetails WHERE Id=(Select max(id) from PrdScreenPrintDetails WHERE PrdnID='" + lblPrId.Text + "')");
        if (txtWeightPc.Text != "")
        {
            txtFinalOutputKg.Text = Convert.ToString((Convert.ToDecimal(txtWeightPc.Text) * Convert.ToDecimal(txtFinalOutput.Text) / 1000M) * 1.005M);
        }
    }

    private void UpdateDetailData()
    {
        SQLQuery.Empty2Zero(txtWeightPc);
        decimal remain = Convert.ToDecimal(txtRejected.Text) - Convert.ToDecimal(txtReUsable.Text); //RunQuery.SQLQuery.ReturnString("Select ISNULL((SheetRejected),0)-ISNULL((WastePcs),0) from  PrdScreenPrintDetails WHERE PrdnID=''");
        decimal wasteKg = Convert.ToDecimal(txtWeightPc.Text) * Convert.ToDecimal(remain) / 1000M;

        SQLQuery.Empty2Zero(txtMin);
        decimal hrMin = Convert.ToDecimal(txtHour.Text) + (Convert.ToDecimal(txtMin.Text) / 60);

        RunQuery.SQLQuery.ExecNonQry("UPDATE PrdScreenPrintDetails SET MachineNo='" + ddMachine.SelectedValue +
                                     "', LineNumber='" + ddLine.SelectedValue + "', InputQty='" + txtSheetQty.Text +
                                     "', ReusableItemUsed='" + ddReusableUsed.SelectedValue + "', RejectedUsed='" +
                                     txtRejUsed.Text + "', FinalInputQty='" + txtFinalInput.Text + "', InkCategory='" +
                                     ddColorCategory.SelectedValue + "', ColorName='" + ddColor.SelectedValue +
                                     "', Spec='" + ddSpec.SelectedValue + "', InkConsum='" + txtInkConsum.Text +
                                     "', Operator='" + ddOperator.SelectedValue + "', SheetRejected='" +
                                     txtRejected.Text + "', NetPrdnSheet='" + txtproduced.Text + "', WorkingHr='" +
                                     hrMin + "', TimeWaste='" + txtTimeWaist.Text + "', Reason='" +
                                     txtReason.Text + "', Shift='" + ddShift.SelectedValue + "', WastePcsItem='" +
                                     ddWasteStock.SelectedValue + "', WastePcs='" + txtReUsable.Text +
                                     "', WasteKgItem='" + ddRecycleKgItem.SelectedValue + "', WasteKg='" + wasteKg + "', godown='" + ddGodown.SelectedValue + "', LocationID = '" + ddLocation.SelectedValue + "' WHERE Id='" + lblEid.Text + "'");

        //RunQuery.SQLQuery.ExecNonQry("Update PrdScreenPrintDetails set WasteKg='" + CalcReusableWeight() + "' WHERE ID=(Select max(id) from PrdScreenPrintDetails)");

        txtFinalOutput.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL((NetPrdnSheet),0) from PrdScreenPrintDetails WHERE Id=(Select max(id) from PrdScreenPrintDetails WHERE PrdnID='" + lblPrId.Text + "')");
        SQLQuery.Empty2Zero(txtWeightPc);
        SQLQuery.Empty2Zero(txtFinalOutput);
        if (txtWeightPc.Text != "")
        {
            txtFinalOutputKg.Text = Convert.ToString((Convert.ToDecimal(txtWeightPc.Text) * Convert.ToDecimal(txtFinalOutput.Text) / 1000M) * 1.005M);
        }
    }

    private void ClearDetailArea()
    {
        //  txtSheetQty.Text = txtproduced.Text;
        txtRejUsed.Text = "0";
        //  txtFinalInput.Text = "";
        txtInkConsum.Text = "";
        txtRejected.Text = "0";
        txtproduced.Text = "";
        txtHour.Text = "";
        txtTimeWaist.Text = "";
        txtReason.Text = "";
        txtReUsable.Text = "";

        Bind2ndGrid();
    }
    private void ClearMasterArea()
    {
        ClearDetailArea();
        btnSave.Text = "Save";
        GridView1.DataBind();
        txtHour.Text = "";
        txtTimeWaist.Text = "";
        txtReason.Text = "";
        lblPrId.Text = "";
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
            Notify(ex.Message.ToString(), "error");
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    private void EditMode(string entryID)
    {
        /*
        SqlCommand cmd = new SqlCommand(@"SELECT InputQty, RejectedUsed, FinalInputQty, InkCategory, ColorName, InkConsum, Operator, SheetRejected, NetPrdnSheet, WorkingHr, 
                         TimeWaste, Reason, Shift, WastePcsItem, WastePcs, WasteKgItem, WasteKg FROM [PrdScreenPrintDetails] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            txtSheetQty.Text = dr[0].ToString();
            txtRejUsed.Text = dr[1].ToString();
            txtFinalInput.Text = dr[2].ToString();

            //string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID=(SELECT GradeID FROM [Categories] WHERE CategoryID=(Select CategoryID from Products WHERE ProductID='" + pid + "') ) ORDER BY [CategoryName]";
            //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddColorCategory, "CategoryID", "CategoryName");

            //ddColorCategory.SelectedValue = RunQuery.SQLQuery.ReturnString("Select CategoryID from Products WHERE ProductID='" + pid + "'");
            //LoadFinishedDD();
            //ddColor.SelectedValue = pid;
            ddColorCategory.SelectedValue = dr[3].ToString();
            ddColor.SelectedValue = dr[4].ToString();
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
                ddRecycleKgItem.SelectedValue = dr[15].ToString();
        }
        cmd.Connection.Close();
        */

        DataTable citydt = RunQuery.SQLQuery.ReturnDataTable(@"SELECT    MachineNo, LineNumber, InputQty, ReusableItemUsed, RejectedUsed, FinalInputQty, InkCategory, ColorName, Spec, InkConsum, 
                         Operator, SheetRejected, NetPrdnSheet,   WorkingHr, TimeWaste, Reason, Shift, WastePcsItem, WastePcs, WasteKgItem, WasteKg, godown, LocationID FROM PrdScreenPrintDetails WHERE Id='" + entryID + "'");

        foreach (DataRow citydr in citydt.Rows)
        {
            ddMachine.SelectedValue = citydr["MachineNo"].ToString();
            ddLine.SelectedValue = citydr["LineNumber"].ToString();
            txtSheetQty.Text = citydr["InputQty"].ToString();
            //bind dd
            ddReusableUsed.SelectedValue = citydr["ReusableItemUsed"].ToString();
            txtRejUsed.Text = citydr["RejectedUsed"].ToString();
            txtFinalInput.Text = citydr["FinalInputQty"].ToString();

            ddColorCategory.SelectedValue = citydr["InkCategory"].ToString();

            string gQuery = "SELECT ProductID, ItemName FROM [Products] WHERE CategoryID='" + ddColorCategory.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddColor, "ProductID", "ItemName");

            ddColor.SelectedValue = citydr["ColorName"].ToString();
            ddSpec.DataBind();
            ddSpec.SelectedValue = citydr["Spec"].ToString();

            txtInkConsum.Text = citydr["InkConsum"].ToString();
            ddOperator.SelectedValue = citydr["Operator"].ToString();
            txtRejected.Text = citydr["SheetRejected"].ToString();
            txtproduced.Text = citydr["NetPrdnSheet"].ToString();

            txtHour.Text = citydr["WorkingHr"].ToString();
            txtTimeWaist.Text = citydr["TimeWaste"].ToString();
            txtReason.Text = citydr["Reason"].ToString();
            ddShift.SelectedValue = citydr["Shift"].ToString();

            ddWasteStock.SelectedValue = citydr["WastePcsItem"].ToString();
            txtReUsable.Text = citydr["WastePcs"].ToString();
            ddRecycleKgItem.SelectedValue = citydr["WasteKgItem"].ToString();
            ddGodown.SelectedValue = citydr["godown"].ToString();
            ddLocation.SelectedValue = citydr["LocationID"].ToString();
            //WasteKg = citydr["WasteKg"].ToString();
            StockDetails();
        }
    }
    protected void GridView2_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView2.Rows[index].FindControl("Label1") as Label;

            SqlCommand cmd7 = new SqlCommand("DELETE PrdScreenPrintDetails WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();

            Bind2ndGrid();

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
        //string ttlWeight =
        //        RunQuery.SQLQuery.ReturnString(
        //            "Select SUM(TotalWeight)+SUM(WaistWeight) from ProductionDetails WHERE PrdnID='' AND Department='Shearing'");

        //if (Convert.ToDecimal(ttlWeight) > 0)
        //{
        //    lblTotalWeight.Text = "<b>Total Output Weight: </b>" + ttlWeight + " kg.";
        //}

        //txtPackPerSheet.Text = RunQuery.SQLQuery.ReturnString(
        //            "Select SUM(PrdnPcs) from ProductionDetails WHERE PrdnID='' AND Department='Shearing'");
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

    protected void ddColorCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetProductListInk();
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
        string remain = RunQuery.SQLQuery.ReturnString("Select ISNULL((SheetRejected),0)-ISNULL((WastePcs),0) from  PrdScreenPrintDetails WHERE PrdnID=''");
        decimal value = Convert.ToDecimal(txtWeightPc.Text) * Convert.ToDecimal(remain) / 1000M;
        return value.ToString();
    }

    protected void ddOutGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");
        GetProductListRaw();
    }

    protected void ddOutCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // GetProductListFinished();
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

            string isExist = RunQuery.SQLQuery.ReturnString("Select ProductionID FROM PrdScreenPrint WHERE pid='" + lblItemCode.Text + "'");

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
    private void Bind2ndGrid()
    {
        string query = "  PrdnID='' AND EntryBy='" + User.Identity.Name + "' ";
        if (btnSave.Text == "Update")
        {
            query = "  PrdnID='" + lblPrId.Text + "' ";
        }

        query = @"SELECT Id, InputQty, RejectedUsed, FinalInputQty, 
                                    (Select CategoryName from Categories WHERE CategoryID=a.InkCategory) AS InkCategory, 
                                   (Select ItemName from Products WHERE ProductID=a.ColorName) AS ColorName, 
                                    InkConsum, 
                                   (Select EName from EmployeeInfo WHERE EmployeeInfoID=a.Operator) AS Operator, 
                                    SheetRejected, NetPrdnSheet, WorkingHr, TimeWaste, Reason, 
                                  (Select DepartmentName from Shifts WHERE Departmentid=a.Shift) AS  Shift, WastePcs, WasteKg, EntryBy
                                     FROM [PrdScreenPrintDetails] a 
                                    WHERE  " + query + " ORDER BY id";
        SQLQuery.PopulateGridView(GridView2, query);
        //CalcConsumption();
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

        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1) pid, Section, ProductionID, Date, MachineNo, LineNumber, Purpose, CustomerID, Brand, PackSize, 
        ItemCompany, ItemBrand, ItemSubGroup, ItemGrade, ItemCategory, ItemName, ItemColor, InputQty,WeightPerPack, InputWeightKg, OutputSubGroup, OutputGrade, OutputCategory, OutputItem, OutPutColor, FinalOutput, FinalOutputKg, Remarks, EntryBy, EntryDate
        FROM PrdScreenPrint WHERE (pid = '" + itemToEdit + "')");
        foreach (DataRow drx in dtx.Rows)
        {
            lblPrId.Text = drx["ProductionID"].ToString();
            Bind2ndGrid();
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
    }
}
