using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Drawing.Charts;
using RunQuery;
using Control = System.Web.UI.Control;
using DataTable = System.Data.DataTable;
using Label = System.Web.UI.WebControls.Label;
using TextBox = System.Web.UI.WebControls.TextBox;

public partial class app_ProductionOutput : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Today.Date.ToShortDateString();

            string lName = Page.User.Identity.Name.ToString();
            lblUser.Text = lName;
            lblProject.Text = RunQuery.SQLQuery.ReturnString("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'");

            ddSection.DataBind();
            LoadGodownAndLocationBySection();
            ddOperation.DataBind();
            ddShift.DataBind();
            ddMachine.DataBind();
            ddPurpose.DataBind();
            ddPurpose2.DataBind();
            ddGroup.DataBind();
            LoadDropDowns();
            ddCustomer.DataBind();
            ddCustomer.SelectedValue = "2027";
            ddBrand.DataBind();
            ddBrand.SelectedValue = "177";
            GetProductList();
            //StockDetails();
            StockDetailsInFifo();
            ddSize.DataBind();
            ddColor.DataBind();
            ddColor.SelectedValue = "1";

            string sum = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(ActProduction,0) FROM ProductionOutput WHERE ProductionID='' AND EntryBy='" + lblUser.Text + "' ");
            if (sum != "")
            {
                CalcConsumption();
            }

            if (IsExistDozingMixing(ddOutItem.SelectedValue))
            {
                pnlDozing.Visible = false;
                lnkHide.Visible = false;
            }
            BindDozingMixingGrid();
            LoadIMLForm();
            //Bind2ndGrid();
            //LoadPendingEntries();
        }
    }

    protected void LoadIMLForm()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["p"]))
        {
            imlGrade.Visible = true;
            imlCategory.Visible = true;
            imlItemName.Visible = true;
            imlUsePcs.Visible = true;
        }
        else
        {
            imlGrade.Visible = false;
            imlCategory.Visible = false;
            imlItemName.Visible = false;
            imlUsePcs.Visible = false;
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

    private string GetStdPrdn()
    {
        string stdPrdn = SQLQuery.ReturnString("Select StdPrdn from ProductionStandard WHERE Section='" + ddSection.SelectedValue +
                                   "' AND MachineNo='" + ddMachine.SelectedValue + "' AND Company='" +
                                   ddCustomer.SelectedValue + "' AND PackSize='" + ddSize.SelectedValue +
                                   "' AND  Operation='" + ddOperation.SelectedValue + "' ");
        txtStdHour.Text = stdPrdn;
        if (txtHour.Text != "" && stdPrdn != "")
        {
            txtCalculated.Text = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(stdPrdn) * (Convert.ToDecimal(txtHour.Text) + (Convert.ToDecimal(txtMin.Text) / 60M))));
        }

        return stdPrdn;
    }

    private void LoadPendingEntries()
    {
        //try
        //{
        string lName = Page.User.Identity.Name.ToString();
        string isExist = SQLQuery.ReturnString("SELECT pid FROM ProductionOutput WHERE ProductionID='' AND EntryBy='" + lName + "'");
        if (isExist == "")
        {
            SqlCommand cmd2 = new SqlCommand("INSERT INTO ProductionOutput (ProductionID, Date, EntryBy) VALUES (@ProductionID, @Date, @EntryBy)",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2.Parameters.AddWithValue("@ProductionID", "");
            cmd2.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
            cmd2.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());
            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
        }
        else
        {
            SqlCommand cmd = new SqlCommand(@"SELECT Date, Shift, MachineNo, LineNumber, Purpose, CustomerID, Brand, PackSize, Operation, OperatorID, CycleTime, 
                         CavityPcs, WorkingHour, TimeWaste, ReasonWaist, CalcProduction, ActProduction, Rejection, NetProduction, ItemWeight, TotalWeight, ActualWeight, ItemSubGroup, Grade, 
                         Category, ItemID, Color, FinalProduction, WasteWeight, ReUsableItem, Remarks FROM [ProductionOutput] WHERE ProductionID=''  AND EntryBy='" + lName + "' ",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txtDate.Text = Convert.ToDateTime(dr[0]).ToString("dd/MM/yyyy");
                ddShift.SelectedValue = dr[1].ToString();
                ddMachine.SelectedValue = dr[2].ToString();

                //ddLine.SelectedValue = dr[3].ToString();
                ddPurpose.SelectedValue = dr[4].ToString();

                ddCustomer.SelectedValue = dr[5].ToString();
                ddBrand.SelectedValue = dr[6].ToString();
                ddSize.SelectedValue = dr[7].ToString();

                ddOperation.SelectedValue = dr[8].ToString();

                string isEmpty = dr[9].ToString();
                if (isEmpty != "")
                {
                    ddOperator.SelectedValue = isEmpty;
                }
                //
                txtCycleTime.Text = dr[10].ToString();
                txtCavity.Text = dr[11].ToString();
                txtHour.Text = dr[12].ToString();

                txtTimeWaist.Text = dr[13].ToString();
                txtReason.Text = dr[14].ToString();
                //txtCalculated.Text = dr[15].ToString();
                txtproduced.Text = dr[16].ToString();
                txtRejected.Text = dr[17].ToString();

                txtFinalProd.Text = dr[18].ToString();
                txtWeightPc.Text = dr[19].ToString();
                txtNetWeight.Text = dr[20].ToString();
                txtActualWeight.Text = dr[21].ToString();
                //------------------
                string gQuery = "SELECT CategoryID, CategoryName FROM ItemSubGroup WHERE GroupID='3' AND ProjectID='" + lblProject.Text + "' ORDER BY CategoryName";
                SQLQuery.PopulateDropDown(gQuery, ddOutSubGroup, "CategoryID", "CategoryName");
                ddOutSubGroup.SelectedValue = dr[22].ToString();

                gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" +
                                ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text +
                                "' ORDER BY [GradeName]";
                RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

                ddOutGrade.SelectedValue = dr[23].ToString();
                gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" +
                         ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text +
                         "' ORDER BY [CategoryName]";
                RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");

                ddOutCategory.SelectedValue = dr[24].ToString();
                gQuery = "SELECT ProductID, ItemName FROM [Products] WHERE CategoryID='" +
                         ddOutCategory.SelectedValue + "' ORDER BY [ItemName]";
                RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");

                ddOutItem.SelectedValue = dr[25].ToString();
                //------------------------
                isEmpty = dr[26].ToString();
                if (isEmpty != "")
                {
                    ddColor.SelectedValue = isEmpty;
                }
                //
                //ddColor.SelectedValue = dr[26].ToString();

                txtFinalOutput.Text = dr[27].ToString();
                txtTotalWestage.Text = dr[28].ToString();

                isEmpty = dr[29].ToString();
                if (isEmpty != "")
                {
                    ddWasteStock.SelectedValue = isEmpty;
                }
                txtRemark.Text = dr[30].ToString();
            }
            cmd.Connection.Close();

        }
        //}
        //catch (Exception ex)
        //{
        //    Notify(ex.Message.ToString(), "error", lblMsg);
        //}
    }

    private void LoadDropDowns()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] WHERE GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGradeRaw, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");

        //Processed Item
        gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] WHERE (GroupID='3')  AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutSubGroup, "CategoryID", "CategoryName");

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");

        ddWasteStock.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            CalcConsumption();
            //decimal inputAvailable = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InWeight)-Sum(OutWeight),0) FROM Stock WHERE  ProductID='" + ddItemNameRaw.SelectedValue + "'"));

            //string ttlWeight = RunQuery.SQLQuery.ReturnString("Select SUM(TotalWeight)+SUM(WaistWeight) from ProductionInput WHERE PrdnID='' ");

            if (VerifyStockInFifo())
            {
                //if (Convert.ToDecimal(txtInputQtyKG.Text) - 1 <= Convert.ToDecimal(ttlWeight))
                //{
                if (btnSave.Text == "Save")
                {
                    LoadPendingEntries();
                    SaveProduction();
                    GridView1.DataBind();
                    //  Bind2ndGrid();

                    //string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
                    //txtDate.Text = dt;

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Production saved successfully";
                }
                else
                {
                    SQLQuery.ExecNonQry("DELETE Stock WHERE InvoiceID ='" + lblPrId.Text + "'   ");
                    UpdateProduction(" WHERE ProductionID='" + lblPrId.Text + "' ");
                    SQLQuery.ExecNonQry("UPDATE tblFifo SET OutType = '', OutTypeId = '', OutValue = '" + 0 + "' WHERE OutTypeId = '" + lblPrId.Text + "'");
                    SQLQuery.ExecNonQry("DELETE tblFifo WHERE InTypeId = '" + lblPrId.Text + "'");
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Entry updated successfully";
                }
                StockEntry();
                ClearControls(Form);
                //StockDetails();
                StockDetailsInFifo();
                ClearMasterArea();

                //}
                //else
                //{
                //lblMsg.Attributes.Add("class", "xerp_error");
                //lblMsg.Text = "Error: Input weight & output weight is not equel!";
                //}
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Error: Input quantity is not available into stock!";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_warning");
            lblMsg.Text = "Error: " + ex.ToString();
        }
    }

    private void SaveProduction()
    {
        string prdId = RunQuery.SQLQuery.ReturnString("Select 'PRD-PC-'+ CONVERT(varchar, (ISNULL (max(pid),0)+1001 )) from ProductionOutput");
        lblPrId.Text = prdId;

        UpdateProduction(" WHERE ProductionID='' AND EntryBy='" + User.Identity.Name + "' ");

    }

    private void UpdateProduction(string query)
    {
        string lName = Page.User.Identity.Name.ToString();
        string entryType = "ProductionOutput";
        RunQuery.SQLQuery.ExecNonQry(@"UPDATE ProductionOutput SET Date='" +
                                     Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") +
                                     @"', Shift='" +
                                     ddShift.SelectedValue +
                                     "', MachineNo='" +
                                     ddMachine.SelectedValue +
                                     "',MouldNo='" +
                                     ddMould.SelectedValue +
                                     "', LineNumber='',  Purpose='" +
                                     ddPurpose.SelectedValue +
                                     "', CustomerID='" +
                                     ddCustomer.SelectedValue +
                                     "', CustomerName='" +
                                     ddCustomer.SelectedItem.Text +
                                     "', Brand='" +
                                     ddBrand.SelectedValue +
                                     "', PackSize='" +
                                     ddSize.SelectedValue +
                                     "', Operation='" +
                                     ddOperation.SelectedValue +
                                     "', OperatorID='" +
                                     ddOperator.SelectedValue +
                                     "' " + query + " ");

        RunQuery.SQLQuery.Empty2Zero(txtCycleTime);
        RunQuery.SQLQuery.Empty2Zero(txtCavity);
        RunQuery.SQLQuery.Empty2Zero(txtHour);
        RunQuery.SQLQuery.Empty2Zero(txtMin);
        RunQuery.SQLQuery.Empty2Zero(txtTimeWaist);
        RunQuery.SQLQuery.Empty2Zero(txtCalculated);
        RunQuery.SQLQuery.Empty2Zero(txtproduced);
        RunQuery.SQLQuery.Empty2Zero(txtRejected);
        RunQuery.SQLQuery.Empty2Zero(txtFinalProd);
        RunQuery.SQLQuery.Empty2Zero(txtWeightPc);
        RunQuery.SQLQuery.Empty2Zero(txtNetWeight);
        RunQuery.SQLQuery.Empty2Zero(txtActualWeight);

        //decimal workingHr=

        RunQuery.SQLQuery.ExecNonQry(@"UPDATE ProductionOutput SET CycleTime='" +
                                     txtCycleTime.Text +
                                     "', CavityPcs='" +
                                     txtCavity.Text +
                                     "', WorkingHour='" + txtHour.Text +
                                     "', WorkingMin='" + txtMin.Text +
                                     "', TimeWaste='" +
                                     txtTimeWaist.Text +
                                     "', ReasonWaist='" +
                                     txtReason.Text +
                                     "', CalcProduction='" +
                                     txtCalculated.Text +
                                     "', ActProduction='" +
                                     txtproduced.Text +
                                     "', Rejection='" +
                                     txtRejected.Text +
                                     "', NetProduction='" +
                                     txtFinalProd.Text +
                                     "', ItemWeight='" +
                                     txtWeightPc.Text +
                                     "', TotalWeight='" +
                                     txtNetWeight.Text +
                                     "', ActualWeight='" +
                                     txtActualWeight.Text +
                                     "' " + query + " ");

        RunQuery.SQLQuery.Empty2Zero(txtFinalOutput);
        RunQuery.SQLQuery.Empty2Zero(txtTotalWestage);
        RunQuery.SQLQuery.Empty2Zero(txtOutputKg);
        RunQuery.SQLQuery.Empty2Zero(txtNonUsable);

        string iName = "";
        if (ddOutItem.SelectedValue != "")
        {
            iName = ddOutItem.SelectedItem.Text;
        }

        RunQuery.SQLQuery.ExecNonQry(@"UPDATE ProductionOutput SET ItemSubGroup='" +
                                     ddOutSubGroup.SelectedValue +
                                     "', Grade='" +
                                     ddOutGrade.SelectedValue +
                                     "', Category='" +
                                     ddOutCategory.SelectedValue +
                                     "', ItemID='" +
                                     ddOutItem.SelectedValue +
                                     "', ItemName='" +
                                     iName +
                                     "', Color='" +
                                     ddColor.SelectedValue +
                                     "', FinalProduction='" + txtFinalOutput.Text +
                                     "', FinalKg='" + txtOutputKg.Text +
                                     "', WasteWeight='" +
                                     txtTotalWestage.Text +
                                     "', ReUsableItem='" + ddWasteStock.Text +
                                     "', NonusableWeight='" + txtNonUsable.Text +
                                     "', Remarks ='" +
                                     txtRemark.Text +
                                     "', EntryType='" + entryType + "' " + query + " ");
        if (!string.IsNullOrEmpty(Request.QueryString["p"]))
        {
            SQLQuery.ExecNonQry(@"UPDATE ProductionOutput SET IMLGrade='" + ddIMLGrade.SelectedValue + "', IMLCategory = '" + ddIMLCategory.SelectedValue + "', IMLItemName='" + ddIMLItem.SelectedValue + "', IMLUsePcs='" + Convert.ToDecimal(txtIMLUse.Text) + "'");
        }
    }

    private void StockEntry()
    {
        decimal totalConsumption = 0;
        //Item Stock Entry
        RunQuery.SQLQuery.ExecNonQry("DELETE Stock WHERE InvoiceID='" + lblPrId.Text + "'");
        //RunQuery.SQLQuery.ExecNonQry("UPDATE ProductionInput SET PrdnID='" + lblPrId.Text + "' WHERE PrdnID=''  AND EntryBy='" + lblUser.Text + "'  ");
        RunQuery.SQLQuery.ExecNonQry("UPDATE ProductionOutput SET ProductionID='" + lblPrId.Text + "' WHERE ProductionID=''  AND EntryBy='" + lblUser.Text + "' ");

        //Input Item Stock-out
        //DataTable citydt = RunQuery.SQLQuery.ReturnDataTable("Select Purpose, ItemGroup, SizeId, BrandID, ProductID, ProductName, TotalWeight, ConsumedWeight from ProductionInput WHERE PrdnID='" + lblPrId.Text + "'");
        DataTable dozingMixingDataTable = SQLQuery.ReturnDataTable("SELECT Id, ProcessedItemId, DozingMixingItemPurpose, DozingMixingItemGroup, DozingMixingItemSubGroup, DozingMixingItemGrade, DozingMixingItemCategory, DozingMixingItem, Consumption, EntryDate FROM DozingMixing WHERE ProcessedItemId='" + ddOutItem.SelectedValue + "'");
        foreach (DataRow dozingMixing in dozingMixingDataTable.Rows)
        {
            //string processedItemId = dozingMixing["ProcessedItemId"].ToString();
            string dozingMixingPurpose = dozingMixing["DozingMixingItemPurpose"].ToString();
            string dozingMixingItemGroup = dozingMixing["DozingMixingItemGroup"].ToString();
            string dozingMixingItemId = dozingMixing["DozingMixingItem"].ToString();
            string dozingMixingItemName = SQLQuery.ReturnString("SELECT ItemName FROM [Products] WHERE ProductID = '" + dozingMixingItemId + "'");
            decimal dozingMixingConsumption = Convert.ToDecimal(dozingMixing["Consumption"].ToString());
            //dozingMixingConsumption = Convert.ToDecimal(txtproduced.Text) * (Convert.ToDecimal(txtInputQtyKG.Text) / 100);
            //decimal WaistWeight = Convert.ToDecimal(txtTotalWestage.Text);// Convert.ToDecimal(dozingMixing["ConsumedWeight"].ToString()) * 1000;

            //Input Item stock-out
            //Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- Plastic Container", lblPrId.Text, SizeId,
            //        ddCustomer.SelectedValue, BrandID, "", "", ProductID, ProductName, "", "8",
            //        "", "1", 0, 0, 0, 0, Convert.ToDecimal(TotalWeight), "", "Stock-Out",
            //        "Production Input", "Round Tin", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            Stock.Inventory.SaveToMachineStock(dozingMixingPurpose, lblPrId.Text, "Production-Plastic Container", lblPrId.Text, ddSize.SelectedValue,
                  ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", dozingMixingItemId, dozingMixingItemName, "", ddGodown.SelectedValue, ddLocation.SelectedValue,
                  ddMachine.SelectedValue, dozingMixingItemGroup, 0, 0, 0, 0, Convert.ToDecimal(dozingMixingConsumption), "", "Stock-Out",
                  "Production Input", "Round Tin", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            //Raw materials comsumption
            //Transaction Type 'Raw Materials' because In The Case of Stock Out, Raw Materials Will be Stocked Out
            totalConsumption += Stock.Inventory.FifoInsert(dozingMixingItemId, "ProductionOutput", ddCustomer.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddBrand.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd"), "", "", 0, dozingMixingConsumption, 0, "", "", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "Stock Out", lblPrId.Text, "0");
        }

        string description = "Production Output: Production ID# " + lblPrId.Text + "  ";
        Accounting.VoucherEntry.AutoVoucherEntry("3", description, "010106001", "010106001", totalConsumption, lblPrId.Text, "", Page.User.Identity.Name, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "1");

        SQLQuery.Empty2Zero(txtTotalWestage);
        if (Convert.ToDecimal(txtTotalWestage.Text) > 0)
        {
            //rejUsedKg = Convert.ToDecimal(ReUsableQty) * Convert.ToDecimal(Stock.Inventory.RawWeightQtyRatio("", "", ReusableItem, "8"));
            Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production-Plastic Container", lblPrId.Text, ddSize.SelectedValue,
                ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ddWasteStock.SelectedValue,
                Stock.Inventory.GetProductName(ddWasteStock.SelectedValue), "", ddGodown.SelectedValue, ddLocation.SelectedValue, ddGroup.SelectedValue, 0, 0, 0,
                Convert.ToDecimal(txtTotalWestage.Text), 0, "", "Stock-In", "Wastage Item", "Plastic Container", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        }

        //Output Item stock-in
        if (Convert.ToDecimal(txtOutputKg.Text) > 0)
        {
            decimal unitPrice = totalConsumption / (Convert.ToInt32(txtFinalOutput.Text));
            decimal unitWeight = Convert.ToDecimal(txtActualWeight.Text) / Convert.ToInt32(txtFinalOutput.Text);
            //Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Plastic Container", lblPrId.Text, ddSize.SelectedValue,
            //    ddCustomer.SelectedValue, ddBrand.SelectedValue, ddColor.SelectedValue, "", ddOutItem.SelectedValue, ddOutItem.SelectedItem.Text, "", "8",
            //    "", "3", Convert.ToInt32(txtFinalOutput.Text), 0, 0, Convert.ToDecimal(txtOutputKg.Text), 0, "", "Stock-in",
            //    "Production Output", "Plastic Container", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            Stock.Inventory.SaveToMachineStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Plastic Container", lblPrId.Text, ddSize.SelectedValue,
               ddCustomer.SelectedValue, ddBrand.SelectedValue, ddColor.SelectedValue, "", ddOutItem.SelectedValue, ddOutItem.SelectedItem.Text, "ProcessedItem", ddGodown.SelectedValue,
    ddLocation.SelectedValue, ddMachine.SelectedValue, "3", Convert.ToDecimal(txtFinalOutput.Text), 0, unitPrice, Convert.ToDecimal(txtOutputKg.Text), 0, "", "Stock-in",
               "Production Input", "Plastic Container", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            //fifo => OUT
            //string outId = SQLQuery.ReturnString("SELECT MAX(Id) FROM ProductionInput");
            //string productId = SQLQuery.ReturnString("SELECT ProductID FROM ProductionInput WHERE PrdnID='" + lblPrId.Text + "'");
            string fifoType = "ProcessedItem";// SQLQuery.ReturnString("Select GroupName from ItemGroup WHERE GroupSrNo='" + iGrp + "'").Replace(" ", "");

            if (ddOutItem.SelectedItem.Text.ToLower().Trim().EndsWith("lid") || ddOutItem.SelectedItem.Text.Trim().EndsWith("lead"))
            {
                fifoType = "SemiFinished";
            }
            
            if (!string.IsNullOrEmpty(Request.QueryString["p"]))
            {
                Stock.Inventory.FifoInsert(ddIMLItem.SelectedValue, "Production-IMLItem", ddCustomer.SelectedValue, ddSize.SelectedValue, "", ddBrand.SelectedValue, txtDate.Text, fifoType, "", 0, 0, Convert.ToDecimal(txtIMLUse.Text), ddGodown.SelectedValue, ddLocation.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd"), "Stock Out", lblPrId.Text, "0");
            }
            Stock.Inventory.FifoInsert(ddOutItem.SelectedValue, "ProductionOutput", ddCustomer.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddBrand.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd"), fifoType, lblPrId.Text, unitPrice, unitWeight, Convert.ToDecimal(txtFinalOutput.Text), ddGodown.SelectedValue, ddLocation.SelectedValue, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "", "", "0");
        }

        RunQuery.SQLQuery.Empty2Zero(txtNonUsable);
        string nonUsableWeight = txtNonUsable.Text; //RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(NonusableWaste),0) from PrdnRoundTinDetails WHERE PrdnID='" + lblPrId.Text + "'");
        if (Convert.ToDecimal(nonUsableWeight) > 0)
        {
            //Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Plastic Container", lblPrId.Text,
            //    ddSize.SelectedValue, ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", "1808", "Nonusable Westage", "", "8", "",
            //    "7", 0, 0, 0, Convert.ToDecimal(nonUsableWeight), 0, "", "Stock-in", "Production Output", "Plastic Container",
            //    Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            Stock.Inventory.SaveToMachineStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Plastic Container", lblPrId.Text,
                ddSize.SelectedValue, ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", "1808", "Nonusable Westage", "", ddGodown.SelectedValue, ddLocation.SelectedValue, "",
                "7", 0, 0, 0, Convert.ToDecimal(nonUsableWeight), 0, "", "Stock-in", "Production Output", "Plastic Container",
                Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            decimal unitPrice = totalConsumption / (Convert.ToInt32(txtFinalOutput.Text));
            string productId = SQLQuery.ReturnString("SELECT ProductID FROM ProductionInput WHERE PrdnID='" + lblPrId.Text + "'");

            Stock.Inventory.FifoInsert(ddOutItem.SelectedValue, "ProductionOutput", ddCustomer.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddBrand.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd"), "ProcessedItemNonusable", "", 0, Convert.ToDecimal(txtNonUsable.Text), 0, "", "",
                Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "Nonusable Output", lblPrId.Text, unitPrice.ToString());
        }

    }

    protected void ddOutItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDozingMixingGrid();
        //StockDetails();
        StockDetailsInFifo();
    }

    protected void LoadGodownAndLocationBySection()
    {
        string godownId = SQLQuery.ReturnString("SELECT GodownID FROM Sections WHERE SID = '" + ddSection.SelectedValue + "'");
        ddGodown.DataBind();
        ddGodown.SelectedValue = godownId;
        string locationID = SQLQuery.ReturnString("SELECT LocationID FROM Sections WHERE SID = '" + ddSection.SelectedValue + "'");
        ddLocation.DataBind();
        ddLocation.SelectedValue = locationID;
    }
    protected void ddSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGodownAndLocationBySection();
        ddShift.DataBind();
        ddMachine.DataBind();
        //ddLine.DataBind();
        //StockDetails();
        StockDetailsInFifo();
    }

    protected void ddGradeRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");
        //ddOutGrade.SelectedValue = ddGradeRaw.SelectedValue;
        //gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");
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
        //ddOutItem.SelectedValue = ddItemNameRaw.SelectedValue;
        //StockDetails();
        StockDetailsInFifo();
    }

    private void LoadFinishedDD()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] WHERE CategoryID='" + ddOutCategory.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
        //StockDetails();
        StockDetailsInFifo();
    }

    private void GetProductList()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] WHERE CategoryID='" + ddCategoryRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemNameRaw, "ProductID", "ItemName");

        gQuery = "SELECT ProductID, ItemName FROM [Products] WHERE CategoryID='" + ddOutCategory.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
        //StockDetails();
        StockDetailsInFifo();
    }

    //private decimal StockDetails()
    //{
    //    decimal availableWeight = 0;
    //    //ddGroup.SelectedValue = Stock.Inventory.GetItemGroup(ddItemNameRaw.SelectedValue);
    //    if (ddGroup.SelectedValue == "1")
    //    {
    //        string avWeight = Stock.Inventory.PlasticRawWeight(ddPurpose2.SelectedValue, ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue);
    //        if (!string.IsNullOrEmpty(avWeight))
    //        {
    //            availableWeight = Convert.ToDecimal(avWeight);
    //            ltrLastInfo.Text = "Stock Available: " + availableWeight + "KG";
    //        }
    //    }
    //    else
    //    {
    //        availableWeight = Convert.ToDecimal(Stock.Inventory.NonUsableWeight(ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue)); ;
    //        ltrLastInfo.Text = "Stock Available: " + availableWeight + "KG";
    //    }
    //    GetStdPrdn();
    //    return availableWeight;
    //}

    private void StockDetailsInFifo()
    {
        DataTable availableStockDataTable = SQLQuery.ReturnDataTable("SELECT MAX(InKg) AS InKg, InDate FROM tblFifo WHERE ItemCode = '" + ddItemNameRaw.SelectedValue + "' AND OutType = '' AND OutTypeId = ''  GROUP BY InDate");
        decimal availableStockInFifo = 0;
        foreach (DataRow availableStockDataTableRow in availableStockDataTable.Rows)
        {
            availableStockInFifo += Convert.ToDecimal(availableStockDataTableRow["InKg"].ToString());
        }
        ltrLastInfo.Text = "Stock Available: " + availableStockInFifo + " KG";
    }
    protected void ddOutCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] WHERE CategoryID='" + ddOutCategory.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
        //StockDetails();
        StockDetailsInFifo();
        ShowHideDozingMixingPanel();
        BindDozingMixingGrid();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {

            if (txtInputQtyKG.Text != "" && txtInputQtyKG.Text != "0")
            {
                if (btnAdd.Text == "Add")
                {
                    //if (StockDetails() >= Convert.ToDecimal(txtInputQtyKG.Text) || Stock.Inventory.StockEnabled() == "0")
                    //{
                    InsertDosenMixing();
                    ClearDetailArea();
                    LoadMachine();
                    lblMsg2.Attributes.Add("class", "xerp_success");
                    lblMsg2.Text = "Dosen/Mixing has been set successfully";
                    //}
                    //else
                    //{
                    //    lblMsg2.Attributes.Add("class", "xerp_error");
                    //    lblMsg2.Text = "Error: Item is not available into stock!";
                    //}
                    //}
                    ////else
                    ////{
                    ////    lblMsg2.Attributes.Add("class", "xerp_error");
                    ////    lblMsg2.Text = "Error: Item already exist!";
                    ////}
                }
                else
                {
                    //RunQuery.SQLQuery.ExecNonQry("Delete ProductionInput WHERE id='" + lblEid.Text + "'");
                    //UpdateDetailData(lblEid.Text);
                    UpdateDozingMixing(lblEid.Text);
                    //Accounting.VoucherEntry.ProductionItemEntry("", "Plastic Container", ddPurpose.SelectedValue, ddSize.SelectedValue, "", ddProduct.SelectedValue, ddProduct.SelectedItem.Text, txtThickness.Text, txtproduced.Text, txtPackPerSheet.Text, txtSheetWeight.Text, txtRejected.Text, Page.User.Identity.Name.ToString());
                    btnAdd.Text = "Add";
                    ClearDetailArea();

                    lblMsg2.Attributes.Add("class", "xerp_success");
                    lblMsg2.Text = "Entry updated successfully";
                }
                //if (btnSave.Text == "Save")
                //{
                //    UpdateProduction(" WHERE ProductionID='' AND EntryBy='" + User.Identity.Name + "' ");
                //}
                //else
                //{
                //    UpdateProduction(" WHERE ProductionID='" + lblPrId.Text + "' ");
                //}
                BindDozingMixingGrid();
                //CalcConsumption();
                ClearDetailArea();
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
        }
    }

    private void InsertDosenMixing()
    {
        string query = "INSERT INTO DozingMixing (ProcessedItemId, DozingMixingItemPurpose, DozingMixingItemGroup, DozingMixingItemSubGroup, DozingMixingItemGrade, DozingMixingItemCategory, DozingMixingItem, ConsumptionPercentage, Consumption)" +
                       " VALUES (@ProcessedItemId, @DozingMixingItemPurpose, @DozingMixingItemGroup, @DozingMixingItemSubGroup, @DozingMixingItemGrade, @DozingMixingItemCategory, @DozingMixingItem, @ConsumptionPercentage, @Consumption)";
        SqlCommand cmd3 = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd3.Parameters.AddWithValue("@ProcessedItemId", ddOutItem.SelectedValue);
        cmd3.Parameters.AddWithValue("@DozingMixingItemPurpose", ddPurpose2.SelectedValue);
        cmd3.Parameters.AddWithValue("@DozingMixingItemGroup", ddGroup.SelectedValue);
        cmd3.Parameters.AddWithValue("@DozingMixingItemSubGroup", ddSubGrp.SelectedValue);
        cmd3.Parameters.AddWithValue("@DozingMixingItemGrade", ddGradeRaw.SelectedValue);
        cmd3.Parameters.AddWithValue("@DozingMixingItemCategory", ddCategoryRaw.SelectedValue);
        cmd3.Parameters.AddWithValue("@DozingMixingItem", ddItemNameRaw.SelectedValue);
        cmd3.Parameters.AddWithValue("@ConsumptionPercentage", Convert.ToDecimal(txtInputQtyKG.Text));
        cmd3.Parameters.AddWithValue("@Consumption", 0);

        cmd3.Connection.Open();
        cmd3.ExecuteNonQuery();
        cmd3.Connection.Close();

    }
    private void UpdateDozingMixing(string id)
    {
        string query = "UPDATE DozingMixing SET DozingMixingItemGroup = '" + ddGroup.SelectedValue + "', DozingMixingItemSubGroup = '" + ddSubGrp.SelectedValue + "', DozingMixingItemGrade = '" + ddGradeRaw.SelectedValue + "', DozingMixingItemCategory = '" + ddCategoryRaw.SelectedValue + "', DozingMixingItem = '" + ddItemNameRaw.SelectedValue + "', ConsumptionPercentage = '" + Convert.ToDecimal(txtInputQtyKG.Text) + "' WHERE Id = '" + id + "'";
        SqlCommand cmd3 = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd3.Connection.Open();
        cmd3.ExecuteNonQuery();
        cmd3.Connection.Close();

    }

    private bool IsExistDozingMixing(string processItemId)
    {
        SqlCommand cmd = new SqlCommand("SELECT * FROM [DozingMixing] WHERE ProcessedItemId='" + processItemId + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        bool isExistDozingMixing = false;
        cmd.Connection.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.HasRows)
        { isExistDozingMixing = true; }
        cmd.Connection.Close();
        return isExistDozingMixing;
    }

    private void InsertDetailData()
    {
        SqlCommand cmd3 = new SqlCommand("INSERT INTO ProductionInput (PrdnID, Department, Purpose, SizeId, BrandID, ItemGroup, SubGroup, Grade, Category, ProductID, ProductName, TotalWeight, ConsumedWeight, EntryType, EntryBy)" +
                                                        " VALUES (@PrdnID, @Department, @Purpose, @SizeId, @BrandID, @ItemGroup, @SubGroup, @Grade, @Category, @ProductID, @ProductName, @TotalWeight, @ConsumedWeight, @EntryType, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd3.Parameters.AddWithValue("@PrdnID", "");
        cmd3.Parameters.AddWithValue("@Department", ddGroup.SelectedValue);
        cmd3.Parameters.AddWithValue("@Purpose", ddPurpose2.SelectedValue);
        cmd3.Parameters.AddWithValue("@SizeID", ddSize.SelectedValue); /*Size id ki khali thakbe?*/
        cmd3.Parameters.AddWithValue("@BrandID", ddBrand.SelectedValue); /*Brand id ki khali thakbe?*/
        cmd3.Parameters.AddWithValue("@ItemGroup", ddGroup.SelectedValue);
        cmd3.Parameters.AddWithValue("@SubGroup", ddSubGrp.SelectedValue);
        cmd3.Parameters.AddWithValue("@Grade", ddGradeRaw.SelectedValue);
        cmd3.Parameters.AddWithValue("@Category", ddCategoryRaw.SelectedValue);
        cmd3.Parameters.AddWithValue("@ProductID", ddItemNameRaw.SelectedValue);
        cmd3.Parameters.AddWithValue("@ProductName", ddItemNameRaw.SelectedItem.Text);
        cmd3.Parameters.AddWithValue("@TotalWeight", Convert.ToDecimal(txtInputQtyKG.Text));
        cmd3.Parameters.AddWithValue("@ConsumedWeight", 0);
        cmd3.Parameters.AddWithValue("@EntryType", "ProductionOutput");
        cmd3.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        cmd3.Connection.Open();
        cmd3.ExecuteNonQuery();
        cmd3.Connection.Close();

    }
    private void UpdateDetailData(string entryID)
    {
        RunQuery.SQLQuery.ExecNonQry(@"UPDATE [dbo].[ProductionInput]   SET 
      [Purpose] = '" + ddPurpose2.SelectedValue + "' ,[SizeId] = '" + ddSize.SelectedValue + "'" +
                                     ",[BrandID] = '" + ddBrand.SelectedValue + "' , [ItemGroup] = '" +
                                     ddGroup.SelectedValue + "',[SubGroup] = '" + ddSubGrp.SelectedValue +
                                     "',[Grade] = '" + ddGradeRaw.SelectedValue + "',[Category] = '" +
                                     ddCategoryRaw.SelectedValue + "',[ProductID] ='" + ddItemNameRaw.SelectedValue +
                                     "',[ProductName] = '" + ddItemNameRaw.SelectedItem.Text + "',[TotalWeight] = '" + txtInputQtyKG.Text +
                                     "',[ConsumedWeight] = '0' WHERE id='" + lblEid.Text + "'");

        ClearDetailArea();
    }

    private void ClearDetailArea()
    {
        txtInputQtyKG.Text = "";
        //Bind2ndGrid();
        //BindDozingMixingGrid();
    }
    private void ClearMasterArea()
    {
        ClearDetailArea();
        btnSave.Text = "Save";
        txtInputQtyKG.Text = "";
        txtHour.Text = "";
        txtTimeWaist.Text = "";
        txtReason.Text = "";
        txtFinalProd.Text = "";
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
            //EditMode(lblItemName.Text);
            EditDozingMixingData(lblEid.Text);
            btnAdd.Text = "UPDATE";
            //StockDetails();
            StockDetailsInFifo();
            lblMsg2.Attributes.Add("class", "xerp_info");
            lblMsg2.Text = "Edit mode activated ...";
            pnlDozing.Visible = true;
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    private void EditMode(string entryID)
    {
        SqlCommand cmd = new SqlCommand("SELECT Department, Purpose, SizeId, BrandID, ProductID, TotalWeight, ConsumedWeight, ItemGroup, SubGroup, Grade, Category FROM [ProductionInput] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            ddPurpose2.SelectedValue = dr[1].ToString();
            //LoadDropDowns();
            //ddSubGrp.SelectedValue = dr[1].ToString();
            //GradeCategoryList();
            //ddGradeRaw.SelectedValue = dr[2].ToString();

            //string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");
            //ddCategoryRaw.SelectedValue = dr[3].ToString();
            //GetProductList();
            //ddItemNameRaw.SelectedValue = dr[4].ToString();
            txtInputQtyKG.Text = dr[5].ToString();
            //txtSheetQty.Text = dr[6].ToString();

            ddGroup.SelectedValue = dr[7].ToString();
            LoadDropDowns();
            ddSubGrp.SelectedValue = dr[8].ToString();
            ddOutSubGroup.SelectedValue = ddSubGrp.SelectedValue;
            string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");
            GradeCategoryList();
            ddGradeRaw.SelectedValue = dr[9].ToString();
            ddOutGrade.SelectedValue = ddGradeRaw.SelectedValue;
            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");

            ddCategoryRaw.SelectedValue = dr[10].ToString();
            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");
            ddOutCategory.SelectedValue = ddCategoryRaw.SelectedValue;
            GetProductList();
            ddItemNameRaw.SelectedValue = dr[4].ToString();
            gQuery = "SELECT ProductID, ItemName FROM [Products] WHERE CategoryID='" + ddOutCategory.SelectedValue + "' ORDER BY [ItemName]";
            SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
            ddOutItem.SelectedValue = ddItemNameRaw.SelectedValue;
        }
        cmd.Connection.Close();
    }

    private void EditDozingMixingData(string id)
    {
        SqlCommand cmd = new SqlCommand("SELECT DozingMixingItemPurpose, DozingMixingItemGroup, DozingMixingItemSubGroup, DozingMixingItemGrade, DozingMixingItemCategory, DozingMixingItem, ConsumptionPercentage FROM DozingMixing WHERE Id='" + id + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            ddPurpose2.SelectedValue = reader[0].ToString();
            ddGroup.SelectedValue = reader[1].ToString();
            string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] WHERE GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");
            ddSubGrp.SelectedValue = reader[2].ToString();
            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            SQLQuery.PopulateDropDown(gQuery, ddGradeRaw, "GradeID", "GradeName");
            ddGradeRaw.SelectedValue = reader[3].ToString();
            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");
            ddCategoryRaw.SelectedValue = reader[4].ToString();
            GetProductList();
            ddItemNameRaw.SelectedValue = reader[5].ToString();
            txtInputQtyKG.Text = reader[6].ToString();
        }
        cmd.Connection.Close();
    }
    protected void GridView2_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView2.Rows[index].FindControl("Label1") as Label;

            SqlCommand cmd7 = new SqlCommand("DELETE DozingMixing WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();

            //Bind2ndGrid();
            BindDozingMixingGrid();

            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "Entry deleted successfully ...";
            btnAdd.Text = "Add";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    protected void GridView2_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //string ttlWeight = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(TotalWeight),0) from ProductionInput WHERE PrdnID='' ");

        //if (Convert.ToDecimal(ttlWeight) > 0)
        //{
        //    lblTotalWeight.Text = "<b>Total Output Weight: </b>" + ttlWeight + " kg.";
        //}

        //txtFinalProd.Text = RunQuery.SQLQuery.ReturnString("Select SUM(PrdnPcs) from ProductionInput WHERE PrdnID='' ");

    }

    //private void CalcConsumption()
    //{
    //    try
    //    {
    //        string prdnNo = "";
    //        if (btnSave.Text == "UPDATE")
    //        {
    //            prdnNo = lblPrId.Text;
    //        }
    //        SQLQuery.Empty2Zero(txtproduced);
    //        //Calculate Consumption of the item intofifo total production
    //        decimal ttlConsumed = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(TotalWeight),0) FROM ProductionInput WHERE PrdnID='" + prdnNo + "' AND EntryBy='" + User.Identity.Name + "' "));
    //        decimal actPrdn = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(ActualWeight,0) FROM ProductionOutput WHERE ProductionID='" + prdnNo + "' AND EntryBy='" + User.Identity.Name + "' "));
    //        if (!string.IsNullOrEmpty(txtActualWeight.Text))
    //        {
    //            actPrdn = Convert.ToDecimal(txtActualWeight.Text);
    //        }

    //        DataTable prdData = SQLQuery.ReturnDataTable("Select Id, TotalWeight FROM ProductionInput WHERE PrdnID='" + prdnNo + "' AND EntryBy='" + User.Identity.Name + "'  ");

    //        foreach (DataRow row in prdData.Rows)
    //        {
    //            string entryID = row["Id"].ToString();
    //            decimal iWeight = Convert.ToDecimal(row["TotalWeight"].ToString());

    //            //decimal consumption = (iWeight * actPrdn) / ttlConsumed;
    //            decimal consumption = (iWeight * actPrdn) / 100;

    //            SQLQuery.ExecNonQry("UPDATE ProductionInput SET ConsumedWeight='" + consumption + "' WHERE id='" + entryID + "'");
    //            //Bind2ndGrid(lblPrId.Text);
    //            BindDozingMixingGrid();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg.Attributes.Add("class", "xerp_error");
    //        lblMsg.Text = "Consumption calculation failed : " + ex.Message.ToString();
    //    }
    //}
    private void CalcConsumption()
    {
        try
        {
            string productionID = "";
            if (btnSave.Text == "UPDATE")
            {
                productionID = lblPrId.Text;
            }
            SQLQuery.Empty2Zero(txtproduced);
            SQLQuery.Empty2Zero(txtActualWeight);
            //Calculate Consumption of the item intofifo total production
            //decimal ttlConsumed = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(TotalWeight),0) FROM ProductionInput WHERE PrdnID='" + productionID + "' AND EntryBy='" + User.Identity.Name + "' "));
            //string actualWeight =
            //    SQLQuery.ReturnString("SELECT ISNULL(ActualWeight,0) FROM ProductionOutput WHERE ProductionID='" +
            //                          productionID + "' AND EntryBy='" + User.Identity.Name + "'");
            //decimal actualProduction = 0;
            //if (actualWeight != "")
            //{
            //    actualProduction = Convert.ToDecimal(actualWeight);
            //}
            decimal actualProduction = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(MAX(ActualWeight),0) FROM ProductionOutput WHERE ProductionID='" + productionID + "' AND EntryBy='" + User.Identity.Name + "'"));
            if (!string.IsNullOrEmpty(txtActualWeight.Text))
            {
                actualProduction = Convert.ToDecimal(txtActualWeight.Text);
            }

            DataTable dozingMixingDataTable = SQLQuery.ReturnDataTable("SELECT Id, ProcessedItemId, ConsumptionPercentage FROM DozingMixing WHERE ProcessedItemId='" + ddOutItem.SelectedValue + "'");

            foreach (DataRow row in dozingMixingDataTable.Rows)
            {
                string id = row["Id"].ToString();
                string processedItemId = row["ProcessedItemId"].ToString();
                decimal consumptionPercentage = Convert.ToDecimal(row["ConsumptionPercentage"].ToString());
                //decimal consumption = (iWeight * actPrdn) / ttlConsumed;
                decimal consumption = (consumptionPercentage * actualProduction) / 100;
                SQLQuery.ExecNonQry("UPDATE DozingMixing SET Consumption='" + consumption + "' WHERE Id='" + id + "'");
                //Bind2ndGrid(lblPrId.Text);
                BindDozingMixingGrid();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Consumption calculation failed : " + ex.Message.ToString();
        }
    }

    protected bool VerifyStockInFifo()
    {
        DataTable dozingMixingDataTable = SQLQuery.ReturnDataTable("SELECT Id, DozingMixingItem, Consumption FROM DozingMixing WHERE ProcessedItemId='" + ddOutItem.SelectedValue + "'");
        foreach (DataRow row in dozingMixingDataTable.Rows)
        {
            string id = row["Id"].ToString();
            string dozingMixingItem = row["DozingMixingItem"].ToString();
            decimal consumption = Convert.ToDecimal(row["Consumption"].ToString());

            DataTable availableStockDataTable = SQLQuery.ReturnDataTable("SELECT MAX(InKg) AS InKg, InDate FROM tblFifo WHERE ItemCode = '" + dozingMixingItem + "' AND OutType = '' AND OutTypeId = ''  GROUP BY InDate");
            decimal availableStockInFifo = 0;
            foreach (DataRow availableStockDataTableRow in availableStockDataTable.Rows)
            {
                availableStockInFifo += Convert.ToDecimal(availableStockDataTableRow["InKg"].ToString());
            }
            if (consumption > availableStockInFifo)
            { return false; }
        }
        return true;
    }

    protected void ddCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand.Items.Clear();
        //ListItem lst = new ListItem("--- all ---", "");
        //ddBrand.Items.Insert(ddBrand.Items.Count, lst);
        //ddBrand.Items.Add("--- all ---");
        ddBrand.DataBind();
    }

    protected void ddGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDropDowns();
        GetProductList();
    }

    protected void ddSubGrp_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GradeCategoryList();
        GetProductList();
        //ddOutSubGroup.SelectedValue = ddSubGrp.SelectedValue;
        //string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

        //gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");

        //gQuery = "SELECT ProductID, ItemName FROM [Products] WHERE CategoryID='" + ddOutCategory.SelectedValue + "' ORDER BY [ItemName]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
    }

    private void GradeCategoryList()
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGradeRaw, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");

        GetProductList();
    }
    protected void txtproduced_OnTextChanged(object sender, EventArgs e)
    {
        CalcConsumption();
        txtRejected.Focus();
    }

    protected void ddOutGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");
        //GetProductList();
        ShowHideDozingMixingPanel();
        BindDozingMixingGrid();
    }

    protected void ddOutItem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ShowHideDozingMixingPanel();
        BindDozingMixingGrid();
    }

    protected void ddPurpose2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //StockDetails();
        StockDetailsInFifo();
    }

    protected void ddOperation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //StockDetails();
        StockDetailsInFifo();
    }

    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    //        if (btnSave.Text == "UPDATE")
    //        {
    //            query = " WHERE PrdnID='" + lblPrId.Text + "' ";
    //        }

    //        query = @"SELECT Id, (Select Purpose from Purpose WHERE pid = a.Purpose) AS[Purpose2], 
    //                                     (Select GroupName from ItemGroup WHERE GroupSrNo = a.ItemGroup) AS Department,  
    //                                      (Select CategoryName from ItemSubGroup WHERE CategoryID = a.SubGroup) AS Purpose,    
    //                                        (SELECT[GradeName] FROM[ItemGrade] WHERE[GradeID] = a.Grade) AS SizeId, 
    //                                     (SELECT[CategoryName] FROM[Categories] WHERE[CategoryID] = a.Category) AS BrandID,
    //                                    ProductID, ProductName, TotalWeight, ConsumedWeight FROM [ProductionInput] a
    //                               WHERE Id=(SELECT MAX(pid) FROM ProductionOutput WHERE MachineNo='" + ddMachine.SelectedValue + "')" + query;

    //        query = @"SELECT P.Id, P.PrdnID, P.Department, P.Purpose, P.SizeId, P.BrandID, IG.GroupName AS ItemGroup, ISG.CategoryName AS SubGroup, 
    //G.GradeName AS Grade, C.CategoryName AS Category, P.ProductID, P.ProductName, P.TotalWeight, P.ConsumedWeight, P.EntryBy, P.EntryDate
    //FROM ProductionInput AS P INNER JOIN ItemGrade AS G ON P.Grade = G.GradeID INNER JOIN ItemGroup AS IG ON P.ItemGroup = IG.GroupSrNo INNER JOIN ItemSubGroup AS ISG ON P.SubGroup = ISG.CategoryID
    //INNER JOIN Categories AS C ON P.Category = C.CategoryID " + query;

    private void GetDozingMixingData()
    {
        string query = "SELECT Id, (SELECT ItemName FROM [Products] WHERE ProductID = DozingMixing.ProcessedItemId) AS ProcessedItemId, (SELECT GroupName FROM [ItemGroup] WHERE GroupSrNo = DozingMixingItemGroup) AS DozingMixingItemGroup, (SELECT CategoryName FROM [ItemSubGroup] WHERE CategoryID = DozingMixingItemSubGroup) AS DozingMixingItemSubGroup, ( SELECT GradeName FROM [ItemGrade] WHERE GradeID = DozingMixingItemGrade) AS DozingMixingItemGrade, (SELECT CategoryName FROM [Categories] WHERE CategoryID = DozingMixingItemCategory) AS DozingMixingItemCategory, (SELECT ItemName FROM [Products] WHERE ProductID = DozingMixingItem) AS DozingMixingItem, ConsumptionPercentage, Consumption FROM DozingMixing WHERE ProcessedItemId = '" + ddOutItem.SelectedValue + "'";
        DataTable dataTable = SQLQuery.ReturnDataTable(query);
        GridView2.DataSource = dataTable;
        GridView2.EmptyDataText = "No data dozing mixing data found!";
        GridView2.DataBind();
    }

    private void ShowHideDozingMixingPanel()
    {
        if (IsExistDozingMixing(ddOutItem.SelectedValue))
        {
            pnlDozing.Visible = false;
            lnkHide.Visible = true;
        }
        else
        {
            pnlDozing.Visible = true;
            lnkHide.Visible = true;
        }
    }
    private void BindDozingMixingGrid()
    {
        if (IsExistDozingMixing(ddOutItem.SelectedValue))
        {
            GetDozingMixingData();
        }
        else
        {
            GetDozingMixingData();
            pnlDozing.Visible = true;
            lnkHide.Visible = true;
        }
    }

    //    private void Bind2ndGrid(string productionID)
    //    {
    //        string query = @"SELECT P.Id, P.PrdnID, P.Department, P.Purpose, P.SizeId, P.BrandID, IG.GroupName AS ItemGroup, ISG.CategoryName AS SubGroup, 
    //G.GradeName AS Grade, C.CategoryName AS Category, P.ProductID, P.ProductName, P.TotalWeight, P.ConsumedWeight, P.EntryBy, P.EntryDate
    //FROM ProductionInput AS P INNER JOIN ItemGrade AS G ON P.Grade = G.GradeID INNER JOIN ItemGroup AS IG ON P.ItemGroup = IG.GroupSrNo INNER JOIN ItemSubGroup AS ISG ON P.SubGroup = ISG.CategoryID
    //INNER JOIN Categories AS C ON P.Category = C.CategoryID WHERE PrdnID = '" + productionID + "'";
    //        DataTable dtTable = SQLQuery.ReturnDataTable(query);
    //        GridView2.DataSource = dtTable;
    //        GridView2.DataBind();

    //        if (dtTable.Rows.Count > 0)
    //        {
    //            pnlDozing.Visible = false;
    //            lnkHide.Visible = false;
    //        }

    //    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
            lblEditId.Text = lblItemName.Text;
            btnSave.Text = "UPDATE";
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

        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1) pid, ProductionID, Date, Shift, MachineNo, LineNumber, Purpose, CustomerID, CustomerName, Brand, 
            PackSize, Operation, OperatorID, CycleTime, CavityPcs, WorkingHour, WorkingMin, TimeWaste, ReasonWaist, 
                         CalcProduction, ActProduction, Rejection, NetProduction, ItemWeight, TotalWeight, ActualWeight, 
ItemSubGroup, Grade, Category, ItemID, ItemName, Color, FinalProduction, FinalKg, WasteWeight, ReUsableItem, NonusableWeight, Remarks, EntryBy, EntryDate
FROM            ProductionOutput WHERE (pid = '" + itemToEdit + "')");
        foreach (DataRow drx in dtx.Rows)
        {
            lblPrId.Text = drx["ProductionID"].ToString();
            //Bind2ndGrid(lblPrId.Text);
            txtDate.Text = Convert.ToDateTime(drx["Date"].ToString()).ToString("dd/MM/yyyy");

            ddSection.SelectedValue = SQLQuery.ReturnString("SELECT Section FROM Shifts WHERE Departmentid='" + drx["Shift"].ToString() + "'");
            ddShift.DataBind();
            ddMachine.DataBind();
            ddShift.SelectedValue = drx["Shift"].ToString();
            ddMachine.SelectedValue = drx["MachineNo"].ToString();
            //ddLine.SelectedValue = drx["LineNumber"].ToString();
            ddPurpose.SelectedValue = drx["Purpose"].ToString();
            ddCustomer.SelectedValue = drx["CustomerID"].ToString();
            ddBrand.DataBind();
            ddBrand.SelectedValue = drx["Brand"].ToString();

            ddSize.SelectedValue = drx["PackSize"].ToString();
            ddOperation.DataBind();
            ddOperation.SelectedValue = drx["Operation"].ToString();
            ddOperator.SelectedValue = drx["OperatorID"].ToString();
            txtCycleTime.Text = drx["CycleTime"].ToString();

            txtCavity.Text = drx["CavityPcs"].ToString();
            txtHour.Text = drx["WorkingHour"].ToString();
            txtMin.Text = drx["WorkingMin"].ToString();
            txtTimeWaist.Text = drx["TimeWaste"].ToString();
            txtReason.Text = drx["ReasonWaist"].ToString();

            txtCalculated.Text = drx["CalcProduction"].ToString();
            txtproduced.Text = drx["ActProduction"].ToString();
            txtRejected.Text = drx["Rejection"].ToString();
            txtFinalProd.Text = drx["NetProduction"].ToString();

            txtWeightPc.Text = drx["ItemWeight"].ToString();
            txtNetWeight.Text = drx["TotalWeight"].ToString();
            txtActualWeight.Text = drx["ActualWeight"].ToString();

            // Generate subgrp under grp
            ddOutSubGroup.SelectedValue = drx["ItemSubGroup"].ToString();

            string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");
            ddOutGrade.SelectedValue = drx["Grade"].ToString();

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");
            ddOutCategory.SelectedValue = drx["Category"].ToString();

            gQuery = "SELECT ProductID, ItemName FROM [Products] WHERE CategoryID='" + ddOutCategory.SelectedValue + "' ORDER BY [ItemName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
            ddOutItem.SelectedValue = drx["ItemID"].ToString();

            BindDozingMixingGrid();

            ddColor.SelectedValue = drx["Color"].ToString();
            txtFinalOutput.Text = drx["FinalProduction"].ToString();
            txtOutputKg.Text = drx["FinalKg"].ToString();
            txtTotalWestage.Text = drx["WasteWeight"].ToString();
            ddWasteStock.SelectedValue = drx["ReUsableItem"].ToString();
            txtNonUsable.Text = drx["NonusableWeight"].ToString();
            txtRemark.Text = drx["Remarks"].ToString();
        }
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

            string productionId = RunQuery.SQLQuery.ReturnString("Select ProductionID FROM ProductionOutput WHERE pid='" + lblItemCode.Text + "'");

            SQLQuery.ExecNonQry("Delete ProductionOutput WHERE ProductionID ='" + productionId + "'");
            SQLQuery.ExecNonQry("Delete ProductionInput WHERE PrdnID='" + productionId + "'");
            SQLQuery.ExecNonQry("Delete  Stock WHERE InvoiceID ='" + productionId + "'");
            SQLQuery.ExecNonQry("UPDATE tblFifo SET OutType = '', OutTypeId = '', OutValue = '" + 0 + "' WHERE OutTypeId = '" + productionId + "'");
            SQLQuery.ExecNonQry("DELETE tblFifo WHERE InTypeId = '" + productionId + "'");
            //SQLQuery.ExecNonQry("Delete  WHERE   ='" + isExist + "'   ");

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
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");

        gQuery = "SELECT ProductID, ItemName FROM [Products] WHERE CategoryID='" + ddOutCategory.SelectedValue + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
        ShowHideDozingMixingPanel();
        BindDozingMixingGrid();
    }


    protected void ddMachine_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //Bind2ndGrid();
        BindDozingMixingGrid();
    }

    protected void lnkDozing_OnClick(object sender, EventArgs e)
    {
        pnlDozing.Visible = true;
        lnkHide.Visible = true;
        lnkDozing.Visible = false;
    }

    protected void lnkHide_OnClick(object sender, EventArgs e)
    {
        pnlDozing.Visible = false;
        lnkHide.Visible = false;
        lnkDozing.Visible = true;
    }
    private void LoadMachine()
    {
        string machineNo = SQLQuery.ReturnString("SELECT MachineNo FROM ProductionOutput WHERE (ProductionID ='" + lblPrId.Text + "') ORDER BY pid DESC");
        lblMachineNo.Text = machineNo;
    }
    protected void ddGodown_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocation.DataBind();
    }
    protected void ddIMLCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetProductListIML();
    }
    private void GetProductListIML()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddIMLCategory.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddIMLItem, "ProductID", "ItemName");
        //ddSpec.DataBind();
        //StockDetails();
    }
    //private decimal StockDetails()
    //{
    //    decimal availableWeight = 0;

    //    //ddGroup.SelectedValue = Stock.Inventory.GetItemGroup(ddItemNameRaw.SelectedValue);
    //    if (ddGroup.SelectedValue == "1")
    //    {
    //        string avWeight = Stock.Inventory.IMLRawWeight(ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue);
    //        if (!string.IsNullOrEmpty(avWeight))
    //        {
    //            availableWeight = Convert.ToDecimal(avWeight);

    //            ltrLastInfo.Text = "Stock Available: " + availableWeight + "KG";
    //        }
    //    }
    //    else
    //    {
    //        availableWeight = Convert.ToDecimal(Stock.Inventory.NonUsableWeight(ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
    //        ltrLastInfo.Text = "Stock Available: " + availableWeight + "KG";
    //    }
    //    GetStdPrdn();
    //    return availableWeight;
    //}
    protected void ddIMLItem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetailsIML();
    }
    private void StockDetailsIML()
    {
        literalIMLStock.Text = "Available Stock: " + Stock.Inventory.QtyinStock(ddPurpose.SelectedValue, ddIMLItem.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue) + " pcs.";
        GetStdPrdn();
    }
}