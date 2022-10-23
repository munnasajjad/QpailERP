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

public partial class app_HTF_Print : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");

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

            GetProductListRaw();
            GetProductListFinished();

            //  StockDetails();
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

    //private string GetStdPrdn()
    //{
    //    string stdPrdn = SQLQuery.ReturnString("Select StdPrdn from ProductionStandard where Section='38' AND MachineNo='" + ddMachine.SelectedValue + "' AND Company='" +
    //                               ddCustomer.SelectedValue + "' AND PackSize='" + ddSize.SelectedValue + "' ");

    //    if (stdPrdn == "")
    //    {
    //        stdPrdn = "0";
    //    }

    //    txtStdHour.Text = stdPrdn;
    //    SQLQuery.Empty2Zero(txtHour);
    //    SQLQuery.Empty2Zero(txtMin);

    //    if (txtHour.Text != "" && stdPrdn != "")
    //    {
    //        txtStdPrdn.Text = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(stdPrdn) * (Convert.ToDecimal(txtHour.Text) + (Convert.ToDecimal(txtMin.Text) / 60M))));
    //    }

    //    return stdPrdn;
    //}
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
            lblMsg.Attributes.Add("class", "xerp_warning");
            lblMsg.Text = "Error: " + ex.ToString();
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
                                     "', MachineNo='',LineNumber='', Purpose='" + ddPurpose.SelectedValue + "', CustomerID='" + ddCustomer.SelectedValue +
                                     "', Brand='" + ddBrand.SelectedValue + "', PackSize='" + ddSize.SelectedValue + "' , ItemCompany='" + ddInputCustomer.SelectedValue + "' , ItemBrand='" + ddInputBrand.SelectedValue + "' , ItemSubGroup='" + ddSubGrp.SelectedValue + "' , ItemGrade='" + ddGradeRaw.SelectedValue + "' , " +
                         " ItemCategory='" + ddCategoryRaw.SelectedValue + "' , ItemName='" + ddItemNameRaw.SelectedItem.Text + "' , ItemColor='" + ddColor2.SelectedValue + "' , InputQty='" + txtInputQtyPCS.Text + "' , WeightPerPack='" + txtWeightPc.Text + "' , InputWeightKg='" + txtInputWeight.Text +
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
        Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "HTF Printing", lblPrId.Text, ddSize.SelectedValue, ddInputCustomer.SelectedValue, ddBrand.SelectedValue, ddColor2.SelectedValue, "", ddItemNameRaw.SelectedValue, ddItemNameRaw.SelectedItem.Text, "", ddGodown.SelectedValue, ddLocation.SelectedValue, "3", 0, Convert.ToInt32(txtFinishGoods.Text), price, 0, 0, "", txtRemark.Text, "Stock-out", "", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        try
        {
            decimal totalProcessedItemConsumedAmount = Stock.Inventory.FifoInsert(ddItemNameRaw.SelectedValue, "Production-HTFPrinting", ddInputCustomer.SelectedValue, ProductionInputPackSizeDropdown.SelectedValue, ddColor2.SelectedValue, ddBrand.SelectedValue, txtDate.Text, "", "", 0, Convert.ToDecimal(txtFinishGoods.Text), Convert.ToDecimal(txtInputQtyPCS.Text), ddGodown.SelectedValue, ddLocation.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd"), "Stock Out", lblPrId.Text, "0");
            decimal HTFPrintingPerPiecePrice = totalProcessedItemConsumedAmount / Convert.ToDecimal(txtFinishGoods.Text);
            decimal unitWeight = Convert.ToDecimal(SQLQuery.ReturnString("SELECT InKg FROM tblFifo WHERE OutTypeId = '" + lblPrId.Text + "'"));

            Stock.Inventory.FifoInsert(ddHTFFoil.SelectedValue, "Production-HTFPrinting", ddCustomer.SelectedValue, ddSize.SelectedValue, ddColor2.SelectedValue, ddBrand.SelectedValue, txtDate.Text, "", "", 0, 0, Convert.ToDecimal(txtHTFUseQuantity.Text), ddGodown.SelectedValue, ddLocation.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd"), "Stock Out", lblPrId.Text, "0");

            Stock.Inventory.FifoInsert(ddOutItem.SelectedValue, "Production-HTFPrinting", ddCustomer.SelectedValue, ddSize.SelectedValue, ddColor2.SelectedValue, ddBrand.SelectedValue, txtDate.Text, "SemiFinished", lblPrId.Text, HTFPrintingPerPiecePrice, unitWeight, Convert.ToDecimal(txtFinishGoods.Text), ddGodown.SelectedValue, ddLocation.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd"), "", "", "0");


            string description = "HTF Printing Production ID# " + lblPrId.Text + "  ";
            Accounting.VoucherEntry.AutoVoucherEntry("3", description, "010106001", "010106001", totalProcessedItemConsumedAmount, lblPrId.Text, "", Page.User.Identity.Name, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "1");
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

    }
    private void ClearMasterArea()
    {
        txtRemark.Text = "";
        GridView1.DataBind();
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


