using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using RptRunQuery;
using RunQuery;
using Control = System.Web.UI.Control;
using Label = System.Web.UI.WebControls.Label;
using TextBox = System.Web.UI.WebControls.TextBox;

public partial class app_HTFPrint : System.Web.UI.Page
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

            string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where CategoryID='16' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGradeRaw, "GradeID", "GradeName");

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");

            //Color Item   

            gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where CategoryID='16' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutSubGroup, "CategoryID", "CategoryName");

            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");


            ddCustomer.DataBind();
            ddBrand.DataBind();

            ddSpec.DataBind();
            ddColorCategory.DataBind();
            //LoadColorDd();

            GetProductListRaw();
            GetProductListInk();
            GetProductListFinished();

            //  StockDetails();
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

    //Notify Alerts
    private void Notify(string msg, string type)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblMsg.Attributes.Add("class", "xerp_" + type);
        lblMsg.Text = msg;
    }

    private string GetStdPrdn()
    {
        string stdPrdn = SQLQuery.ReturnString("Select StdPrdn from ProductionStandard where Section='38' AND MachineNo='" + ddMachine.SelectedValue + "' AND Company='" +
                                   ddCustomer.SelectedValue + "' AND PackSize='" + ddSize.SelectedValue + "' ");

        if (stdPrdn == "")
        {
            stdPrdn = "0";
        }

        txtStdHour.Text = stdPrdn;
        SQLQuery.Empty2Zero(txtHour);
        SQLQuery.Empty2Zero(txtMin);

        if (txtHour.Text != "" && stdPrdn != "")
        {
            txtStdPrdn.Text = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(stdPrdn) * (Convert.ToDecimal(txtHour.Text) + (Convert.ToDecimal(txtMin.Text) / 60M))));
        }

        return stdPrdn;
    }
    private void FifoStockDetailsInput()
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //decimal inputAvailable = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where  ProductID='" + ddItemNameRaw.SelectedValue + "'"));

            //string ttlWeight = RunQuery.SQLQuery.ReturnString("Select SUM(TotalWeight)+SUM(WaistWeight) from PrdHTFPrintDetails where PrdnID='' AND Department='Shearing'");

            decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableProcessedQty(ddPurpose.SelectedValue, "", ddItemNameRaw.SelectedValue,
                        ddInputCustomer.SelectedValue, ddInputBrand.SelectedValue, ddSize.SelectedValue, ddColor2.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
            decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableProcessedWeight(ddPurpose.SelectedValue, "", ddItemNameRaw.SelectedValue,
                        ddInputCustomer.SelectedValue, ddInputBrand.SelectedValue, ddSize.SelectedValue, ddColor2.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

            SQLQuery.Empty2Zero(txtInputQtyPCS);
            SQLQuery.Empty2Zero(txtInputWeight);

            if ((inputAvailable >= Convert.ToDecimal(txtInputQtyPCS.Text) && inputAvailableKg >= Convert.ToDecimal(txtInputWeight.Text)) || Stock.Inventory.StockEnabled() == "0")
            {

                if (btnSave.Text == "Save")
                {
                    SaveProduction();
                    UpdateProduction();
                    StockDetails();

                    string pdt = txtDate.Text;
                    ClearControls(Form);
                    txtDate.Text = pdt;
                    GridView2.DataBind();
                    GridView1.DataBind();
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Production saved successfully";
                    Notify("Production saved successfully", "success");
                }
                else
                {
                    UpdateProduction();
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Entry updated successfully";
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
        string prdId = RunQuery.SQLQuery.ReturnString("Select 'PRD-HTF-'+ CONVERT(varchar, (ISNULL (max(pid),0)+1001 )) from PrdHTFPrint");
        lblPrId.Text = prdId;

        SqlCommand cmd2 = new SqlCommand("INSERT INTO PrdHTFPrint (ProductionID, EntryBy,RejHandleQty,FinishGoodsQty) VALUES (@ProductionID, @EntryBy,@RejHandleQty,@FinishGoodsQty)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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
        RunQuery.SQLQuery.ExecNonQry(@"Update PrdHTFPrint SET Date='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") +
                                     "', MachineNo='" + ddMachine.SelectedValue + "',LineNumber='" + ddLine.SelectedValue + "', Purpose='" + ddPurpose.SelectedValue + "', CustomerID='" + ddCustomer.SelectedValue +
                                     "', Brand='" + ddBrand.SelectedValue + "', PackSize='" + ddSize.SelectedValue + "' , ItemCompany='" + ddInputCustomer.SelectedValue + "' , ItemBrand='" + ddInputBrand.SelectedValue + "' , ItemSubGroup='" + ddSize.SelectedValue + "' , ItemGrade='" + ddSize.SelectedValue + "' , " +
                         " ItemCategory='" + ddSize.SelectedValue + "' , ItemName='" + ddSize.SelectedValue + "' , ItemColor='" + ddColor2.SelectedValue + "' , InputQty='" + txtInputQtyPCS.Text + "' , WeightPerPack='" + txtWeightPc.Text + "' , InputWeightKg='" + txtInputWeight.Text +
                         "', OutputSubGroup='" + ddOutSubGroup.SelectedValue + "' , OutputGrade='" + ddOutGrade.SelectedValue + "' , OutputCategory='" + ddOutCategory.SelectedValue + "' , OutputItem='" + ddOutItem.SelectedValue + "' , OutPutColor='" + ddOutputColor.SelectedValue + "' , FinalOutput='" + txtFinalOutput.Text + "' , FinalOutputKg='" + txtFinalOutputKg.Text + "', RejHandleQty='" + txtHandleReject.Text + "' ,FinishGoodsQty='" + txtFinishGoods.Text + "', " +
                         " Remarks ='" + txtRemark.Text + "' WHERE ProductionID='" + lblPrId.Text + "' ");

        //Item Stock Entry
        RunQuery.SQLQuery.ExecNonQry("DELETE Stock WHERE InvoiceID='" + lblPrId.Text + "'");
        RunQuery.SQLQuery.ExecNonQry("UPDATE PrdHTFPrintDetails SET PrdnID='" + lblPrId.Text +
                                     "' WHERE PrdnID='' AND Section='Offset Printing'");

        decimal price = 0;// Inventory.LastNonprintedPrice(Purpose, itemType, Item);
        //Input Item stock-out
        SQLQuery.Empty2Zero(txtInputWeight);
        //  Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "HTF Printing", lblPrId.Text, ddSize.SelectedValue, ddInputCustomer.SelectedValue, ddInputBrand.SelectedValue, ddColor2.SelectedValue, "", ddItemNameRaw.SelectedValue, ddItemNameRaw.SelectedItem.Text, "", "8", "", "3", 0, Convert.ToInt32(txtInputQtyPCS.Text), price, 0, Convert.ToDecimal(txtInputWeight.Text), "", "Stock-out", "Production", "HTF Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "HTF Printing", lblPrId.Text, ddShift.SelectedValue, ddInputCustomer.SelectedValue, ddBrand.SelectedValue, ddColor.SelectedValue, ddSpec.SelectedValue, ddItemNameRaw.SelectedValue, ddItemNameRaw.SelectedItem.Text, "", ddGodown.SelectedValue, ddLocation.SelectedValue, "3", 0, Convert.ToInt32(txtFinishGoods.Text), price, 0, 0, "", txtRemark.Text, "Stock-out", "", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        try
        {
            decimal totalSemiFinishedItemConsumed = Stock.Inventory.FifoInsert(ddItemNameRaw.SelectedValue, "Production-HTFPrinting", ddCustomer.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddBrand.SelectedValue, txtDate.Text, "", "", 0, Convert.ToDecimal(txtFinishGoods.Text), Convert.ToDecimal(txtInputQtyPCS.Text), ddGodown.SelectedValue, ddLocation.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd"), "Stock Out", lblPrId.Text, "0");
            decimal HTFPringPerPiecePrice = 0; /*totalSemiFinishedItemConsumed / Convert.ToDecimal(txtFinishGoods.Text);*/
            decimal unitWeight = Convert.ToDecimal(SQLQuery.ReturnString("SELECT InKg FROM tblFifo WHERE OutTypeId = '" + lblPrId.Text + "'"));
            Stock.Inventory.FifoInsert(ddOutItem.SelectedValue, "Production-HTFPrinting", ddCustomer.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddBrand.SelectedValue, txtDate.Text, "SemiFinished", lblPrId.Text, HTFPringPerPiecePrice, unitWeight, Convert.ToDecimal(txtFinishGoods.Text), ddGodown.SelectedValue, ddLocation.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd"), "", "", "0");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        //HTF Imression stock-in/out
        DataTable citydt = RunQuery.SQLQuery.ReturnDataTable(@"Select   InputQty, ReusableItemUsed, RejectedUsed, FinalInputQty, InkCategory, 
                        ColorName, Spec, InkConsum, SheetRejected, NetPrdnSheet, WorkingHr, WorkingMin, StdHourPerHour, ProjectedPrdn,
                         WastePcsItem, WastePcs, WasteKgItem, WasteKg   from PrdHTFPrintDetails where PrdnID='" + lblPrId.Text + "'");

        foreach (DataRow citydr in citydt.Rows)
        {
            string Purpose = ddPurpose.SelectedValue;
            string InputQty = citydr["InputQty"].ToString();
            string ReusableItemUsed = citydr["ReusableItemUsed"].ToString();
            string RejectedUsed = citydr["RejectedUsed"].ToString();
            string FinalInputQty = citydr["FinalInputQty"].ToString();

            string Spec = citydr["Spec"].ToString();
            string InkCategory = citydr["InkCategory"].ToString();
            string ColorName = citydr["ColorName"].ToString();
            int InkConsum = Convert.ToInt32(Convert.ToDecimal(citydr["InkConsum"].ToString()));
            string SheetRejected = citydr["SheetRejected"].ToString();

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


            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- HTF Printing", lblPrId.Text, "",
                ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ReusableItemUsed, Stock.Inventory.GetProductName(ReusableItemUsed), "", ddGodown.SelectedValue, ddLocation.SelectedValue,
                "7", 0, Convert.ToInt32(RejectedUsed), price, 0, ruWeight, "", "Stock-Out", "Wastage used", "HTF Printing",
                Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            //HTF Consumption
            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- HTF Printing", lblPrId.Text, "",
                ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ColorName,
                Stock.Inventory.GetProductName(ColorName), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "1", 0, InkConsum, price, 0, 0,
                "", "Stock-Out", "HTF used", "HTF Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            //Stock In
            decimal wasteKg = Convert.ToDecimal(txtWeightPc.Text) * Convert.ToDecimal(WastePcs) / 1000M;
            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- HTF Printing", lblPrId.Text, "",
                ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", WastePcsItem,
                Stock.Inventory.GetProductName(WastePcsItem), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", Convert.ToInt32(WastePcs), 0, price,
                wasteKg, 0, "", "Stock-In", "Wastage Item", "HTF Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- HTF Printing", lblPrId.Text, "",
                ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", WasteKgItem,
                Stock.Inventory.GetProductName(WasteKgItem), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", 0, 0, price,
                Convert.ToDecimal(WasteKg), 0, "", "Stock-In", "Wastage Item", "HTF Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        }

        //decimal outputWeight = Convert.ToDecimal(txtFinalOutputKg.Text); //Convert.ToInt32(txtTotalPack.Text)*Convert.ToDecimal(txtWeightPerPack);
        //Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- HTF Printing", lblPrId.Text, ddSize.SelectedValue, ddCustomer.SelectedValue, ddBrand.SelectedValue, ddOutputColor.SelectedValue, "", ddOutItem.SelectedValue, ddOutItem.SelectedItem.Text, "", "8", "", "3", Convert.ToInt32(txtFinalOutput.Text), 0, price, Convert.ToDecimal(txtFinalOutputKg.Text), 0, "", "Stock-in", "Production", "HTF Printing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

    }

    protected void ddColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddSpec.DataBind();
        StockDetails();
    }
    protected void ddSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddOperator.DataBind();
        GetStdPrdn();
    }

    protected void ddGradeRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddColorCategory, "CategoryID", "CategoryName");
        GetProductListRaw();
        FifoStockDetailsInput();
        ddOutGrade.SelectedValue = ddGradeRaw.SelectedValue;
    }
    protected void ddCategoryRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        // GetProductListRaw();

    }
    protected void ddItemNameRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        //StockDetails();
        string query = "SELECT P.ProductID, CONCAT(P.ItemName,'-',P.ProductID, '(',C.CategoryName,')') AS ItemName FROM Products AS P INNER JOIN Categories AS C ON P.CategoryID = C.CategoryID INNER JOIN ItemGrade AS G ON G.GradeID = C.GradeID WHERE P.ProductID='" + ddItemNameRaw.SelectedValue + "' AND P.ProjectID='" + lblProject.Text + "' ORDER BY P.ItemName";
        SQLQuery.PopulateDropDown(query, ddOutItem, "ProductID", "ItemName");
        FifoStockDetailsInput();
    }
    protected void ddSemifinishedPackSize_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FifoStockDetailsInput();
    }

    private void GetProductListRaw()
    {
        //string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategoryRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemNameRaw, "ProductID", "ItemName");
        string gQuery = "SELECT P.ProductID, CONCAT(P.ItemName,'-',P.ProductID, '(',C.CategoryName,')') AS ItemName FROM Products AS P INNER JOIN Categories AS C ON P.CategoryID = C.CategoryID INNER JOIN ItemGrade AS G ON G.GradeID = C.GradeID WHERE G.GradeID='" + ddGradeRaw.SelectedValue + "' AND P.ProjectID='" + lblProject.Text + "' ORDER BY P.ItemName";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemNameRaw, "ProductID", "ItemName");
        string query = "SELECT P.ProductID, CONCAT(P.ItemName,'-',P.ProductID, '(',C.CategoryName,')') AS ItemName FROM Products AS P INNER JOIN Categories AS C ON P.CategoryID = C.CategoryID INNER JOIN ItemGrade AS G ON G.GradeID = C.GradeID WHERE P.ProductID='" + ddItemNameRaw.SelectedValue + "' AND P.ProjectID='" + lblProject.Text + "' ORDER BY P.ItemName";
        SQLQuery.PopulateDropDown(query, ddOutItem, "ProductID", "ItemName");
        //StockDetails();
        FifoStockDetailsInput();
    }
    private void GetProductListInk()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddColorCategory.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddColor, "ProductID", "ItemName");


        //  StockDetails();
    }

    private void GetProductListFinished()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddOutCategory.SelectedValue + "'  ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");

        StockDetails();
    }


    private void StockDetails()
    {
        decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableProcessedQty(ddPurpose.SelectedValue, "", ddItemNameRaw.SelectedValue,
                    ddInputCustomer.SelectedValue, ddInputBrand.SelectedValue, ddSize.SelectedValue, ddColor2.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
        decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableProcessedWeight(ddPurpose.SelectedValue, "", ddItemNameRaw.SelectedValue,
                    ddInputCustomer.SelectedValue, ddInputBrand.SelectedValue, ddSize.SelectedValue, ddColor2.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

        ltrCurrentStock.Text = "Available Stock: " + inputAvailable + " PCS, " + inputAvailableKg + "KG";

        ltrReUsable.Text = "Available Stock: " + Stock.Inventory.NonUsableQty(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " pc. " + Stock.Inventory.NonUsableWeight(ddReusableUsed.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " kg";

        //ltrColorStock.Text = "Available Stock: " + Stock.Inventory.NonUsableQty(ddColor.SelectedValue, "8") + " pcs.";
        ltrColorStock.Text = "Available Stock: " + Stock.Inventory.QtyinStock(ddPurpose.SelectedValue, ddColor.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " pcs.";

        //if (inputAvailable > 0 && inputAvailableKg > 0)
        //{
        //    txtWeightSheet.Text = Convert.ToDecimal(inputAvailableKg / inputAvailable * 1000M).ToString("##.###");
        //    if (txtInputQtyPCS.Text != "")
        //    {
        //        txtInputWeight.Text = Convert.ToString(Convert.ToDecimal(txtInputQtyPCS.Text) * Convert.ToDecimal(txtWeightSheet.Text) / 1000M);
        //    }
        //}
        GetStdPrdn();
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
                        //RunQuery.SQLQuery.ExecNonQry("Delete PrdHTFPrintDetails where id='" + lblEid.Text + "'");
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
        SQLQuery.Empty2Zero(txtRejected);
        SQLQuery.Empty2Zero(txtReUsable);
        SQLQuery.Empty2Zero(txtWeightPc);

        decimal remain = Convert.ToDecimal(txtRejected.Text) - Convert.ToDecimal(txtReUsable.Text); //RunQuery.SQLQuery.ReturnString("Select ISNULL((SheetRejected),0)-ISNULL((WastePcs),0) from  PrdHTFPrintDetails where PrdnID=''");
        decimal wasteKg = Convert.ToDecimal(txtWeightPc.Text) * Convert.ToDecimal(remain) / 1000M;

        SqlCommand cmd3 = new SqlCommand("INSERT INTO PrdHTFPrintDetails (Section, PrdnID, MachineNo, LineNumber, InputQty, ReusableItemUsed, RejectedUsed, FinalInputQty, InkCategory, ColorName, Spec, InkConsum, Operator, SheetRejected, NetPrdnSheet, " +
                                         "WorkingHr, WorkingMin, StdHourPerHour, ProjectedPrdn, TimeWaste, Reason, Shift, WastePcsItem, WastePcs, WasteKgItem, WasteKg, EntryBy,SERejectedFoil,PrdnRejectedCont,QCRejectedCont,QCRejectedFoil)" +
                                                        " VALUES ('Offset Printing', '', '" + ddMachine.SelectedValue +
                                     "', '" + ddLine.SelectedValue + "',  '" + txtSheetQty.Text + "', '" + ddReusableUsed.SelectedValue + "', '" + txtRejUsed.Text + "', '" + txtFinalInput.Text + "', '" + ddColorCategory.SelectedValue + "', '" + ddColor.SelectedValue + "', '" + ddSpec.SelectedValue + "', '" + txtInkConsum.Text + "', '" + ddOperator.SelectedValue + "', '" + txtRejected.Text + "', '" + txtproduced.Text + "', '" +
                                     txtHour.Text + "', '" + txtMin.Text + "', '" + txtStdHour.Text + "', '" + txtStdPrdn.Text + "', '" + txtTimeWaist.Text + "', '" + txtReason.Text + "', '" + ddShift.SelectedValue + "','" + ddWasteStock.SelectedValue + "', '" + txtReUsable.Text + "', '" + ddRecycleKgItem.SelectedValue + "', '" + wasteKg + "', @EntryBy,'" + txtSERejectFoil.Text + "','" + txtPrRejContainer.Text + "','" + txtQCRejContainer.Text + "','" + txtQCRejectFoil.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd3.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        cmd3.Connection.Open();
        cmd3.ExecuteNonQuery();
        cmd3.Connection.Close();

        //RunQuery.SQLQuery.ExecNonQry("Update PrdHTFPrintDetails set WasteKg='" + CalcReusableWeight() + "' where ID=(Select max(id) from PrdHTFPrintDetails)");

        txtFinalOutput.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL((NetPrdnSheet),0) from PrdHTFPrintDetails where Id=(Select max(id) from PrdHTFPrintDetails where PrdnID='')");
        if (txtWeightPc.Text != "")
        {
            txtFinalOutputKg.Text = Convert.ToString((Convert.ToDecimal(txtWeightPc.Text) * Convert.ToDecimal(txtFinalOutput.Text) / 1000M) * 1.005M);
        }
    }

    private void UpdateDetailData()
    {
        SQLQuery.Empty2Zero(txtRejected);
        SQLQuery.Empty2Zero(txtReUsable);
        SQLQuery.Empty2Zero(txtWeightPc);
        decimal remain = Convert.ToDecimal(txtRejected.Text) - Convert.ToDecimal(txtReUsable.Text); //RunQuery.SQLQuery.ReturnString("Select ISNULL((SheetRejected),0)-ISNULL((WastePcs),0) from  PrdHTFPrintDetails where PrdnID=''");
        decimal wasteKg = Convert.ToDecimal(txtWeightPc.Text) * Convert.ToDecimal(remain) / 1000M;

        RunQuery.SQLQuery.ExecNonQry("UPDATE PrdHTFPrintDetails SET MachineNo='" + ddMachine.SelectedValue +
                                     "', LineNumber='" + ddLine.SelectedValue + "', InputQty='" + txtSheetQty.Text +
                                     "', ReusableItemUsed='" + ddReusableUsed.SelectedValue + "', RejectedUsed='" +
                                     txtRejUsed.Text + "', FinalInputQty='" + txtFinalInput.Text + "', InkCategory='" +
                                     ddColorCategory.SelectedValue + "', ColorName='" + ddColor.SelectedValue +
                                     "', Spec='" + ddSpec.SelectedValue + "', InkConsum='" + txtInkConsum.Text +
                                     "', Operator='" + ddOperator.SelectedValue + "', SheetRejected='" +
                                     txtRejected.Text + "', NetPrdnSheet='" + txtproduced.Text + "', WorkingHr='" + txtHour.Text +
                                     "', WorkingMin='" + txtMin.Text + "', StdHourPerHour='" + txtStdHour.Text + "', ProjectedPrdn='" + txtStdPrdn.Text + "', TimeWaste='" + txtTimeWaist.Text + "', Reason='" +
                                     txtReason.Text + "', Shift='" + ddShift.SelectedValue + "', WastePcsItem='" +
                                     ddWasteStock.SelectedValue + "', WastePcs='" + txtReUsable.Text +
                                     "', WasteKgItem='" + ddRecycleKgItem.SelectedValue + "', WasteKg='" + wasteKg + "',SERejectedFoil='" + txtSERejectFoil.Text + "',PrdnRejectedCont='" + txtPrRejContainer.Text + "',QCRejectedCont='" + txtQCRejContainer.Text + "',QCRejectedFoil='" + txtQCRejectFoil.Text + "' WHERE Id='" + lblEid.Text + "'");

        //RunQuery.SQLQuery.ExecNonQry("Update PrdHTFPrintDetails set WasteKg='" + CalcReusableWeight() + "' where ID=(Select max(id) from PrdHTFPrintDetails)");

        txtFinalOutput.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL((NetPrdnSheet),0) from PrdHTFPrintDetails where Id=(Select max(id) from PrdHTFPrintDetails where PrdnID='')");
        if (txtWeightPc.Text != "")
        {
            txtFinalOutputKg.Text = Convert.ToString((Convert.ToDecimal(txtWeightPc.Text) * Convert.ToDecimal(txtFinalOutput.Text) / 1000M) * 1.005M);
        }
    }

    private void ClearDetailArea()
    {
        txtSheetQty.Text = txtproduced.Text;
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
                         TimeWaste, Reason, Shift, WastePcsItem, WastePcs, WasteKgItem, WasteKg FROM [PrdHTFPrintDetails] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
                         Operator, SheetRejected, NetPrdnSheet, WorkingHr, WorkingMin, StdHourPerHour, ProjectedPrdn, TimeWaste, Reason, Shift, WastePcsItem, WastePcs, WasteKgItem, WasteKg,SERejectedFoil,PrdnRejectedCont,QCRejectedCont,QCRejectedFoil FROM PrdHTFPrintDetails WHERE Id='" + entryID + "'");

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

            string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddColorCategory.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddColor, "ProductID", "ItemName");

            ddColor.SelectedValue = citydr["ColorName"].ToString();
            //ddSpec.DataBind();
            ddSpec.SelectedValue = citydr["Spec"].ToString();

            txtInkConsum.Text = citydr["InkConsum"].ToString();
            ddOperator.SelectedValue = citydr["Operator"].ToString();
            txtRejected.Text = citydr["SheetRejected"].ToString();
            txtproduced.Text = citydr["NetPrdnSheet"].ToString();

            txtHour.Text = citydr["WorkingHr"].ToString();
            txtMin.Text = citydr["WorkingMin"].ToString();
            //txtHour.Text = citydr["WorkingHr"].ToString();
            txtStdPrdn.Text = citydr["ProjectedPrdn"].ToString();
            txtTimeWaist.Text = citydr["TimeWaste"].ToString();
            txtReason.Text = citydr["Reason"].ToString();
            ddShift.SelectedValue = citydr["Shift"].ToString();

            ddWasteStock.SelectedValue = citydr["WastePcsItem"].ToString();
            txtReUsable.Text = citydr["WastePcs"].ToString();
            ddRecycleKgItem.SelectedValue = citydr["WasteKgItem"].ToString();
            txtSERejectFoil.Text = citydr["SERejectedFoil"].ToString();
            txtPrRejContainer.Text = citydr["PrdnRejectedCont"].ToString();
            txtQCRejContainer.Text = citydr["QCRejectedCont"].ToString();
            txtQCRejectFoil.Text = citydr["QCRejectedFoil"].ToString();
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

            SqlCommand cmd7 = new SqlCommand("DELETE PrdHTFPrintDetails WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();

            GridView2.DataBind();

            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "Entry deleted successfully...";
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
        //            "Select SUM(TotalWeight)+SUM(WaistWeight) from ProductionDetails where PrdnID='' AND Department='Shearing'");

        //if (Convert.ToDecimal(ttlWeight) > 0)
        //{
        //    lblTotalWeight.Text = "<b>Total Output Weight: </b>" + ttlWeight + " kg.";
        //}

        //txtPackPerSheet.Text = RunQuery.SQLQuery.ReturnString(
        //            "Select SUM(PrdnPcs) from ProductionDetails where PrdnID='' AND Department='Shearing'");
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
        string remain = RunQuery.SQLQuery.ReturnString("Select ISNULL((SheetRejected),0)-ISNULL((WastePcs),0) from  PrdHTFPrintDetails where PrdnID=''");
        decimal value = Convert.ToDecimal(txtWeightPc.Text) * Convert.ToDecimal(remain) / 1000M;
        return value.ToString();
    }

    protected void ddOutGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");
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
        ddColorCategory.DataBind();
        GetProductListInk();
    }

    protected void ddColor2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
        FifoStockDetailsInput();
        ddOutputColor.SelectedValue = ddColor2.SelectedValue;
    }

    protected void ddReusableUsed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddInputBrand_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
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

            string isExist = RunQuery.SQLQuery.ReturnString("Select ProductionID FROM PrdHTFPrint WHERE pid='" + lblItemCode.Text + "'");

            SQLQuery.ExecNonQry("Delete PrdHTFPrint where ProductionID ='" + isExist + "'   ");
            SQLQuery.ExecNonQry("Delete PrdHTFPrintDetails where PrdnID='" + isExist + "'   ");
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
    protected void ddGodown_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocation.DataBind();
    }

    protected void ProductionInputPackSizeDropdown_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FifoStockDetailsInput();
    }
}


