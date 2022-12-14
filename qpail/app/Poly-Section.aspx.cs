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

public partial class app_PolySection : System.Web.UI.Page
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
            ddOperation.DataBind();
            ddShift.DataBind();
            ddMachine.DataBind();
            //ddLine.DataBind();

            ddPurpose2.DataBind();
            ddGroup.DataBind();
            LoadDropDowns();
            ddCustomer.DataBind();
            ddBrand.DataBind();
            GetProductList();
            StockDetails();

            ddColor.DataBind();
            ddColor.SelectedValue = "1";

            string sum = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(ActProduction,0) FROM [PrdnPolyMaster] Where ProductionID='' AND EntryBy='" + lblUser.Text + "' ");
            if (sum != "")
            {
                CalcConsumption();
            }

           // LoadPendingEntries();
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
        string stdPrdn = SQLQuery.ReturnString("Select StdPrdn from ProductionStandard where Section='" + ddSection.SelectedValue +
                                   "' AND MachineNo='" + ddMachine.SelectedValue + "' AND Company='" +
                                   ddCustomer.SelectedValue + "' AND PackSize='" + ddSize.SelectedValue +
                                   "' AND  Operation='" + ddOperation.SelectedValue + "' ");
        txtStdHour.Text = stdPrdn;
        //if (txtHour.Text != "" && stdPrdn != "")
        //{
        //    txtCalculated.Text = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(stdPrdn) * (Convert.ToDecimal(txtHour.Text) + (Convert.ToDecimal(txtMin.Text) / 60M))));
        //}

        return stdPrdn;
    }

    private void LoadPendingEntries()
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            string isExist = RunQuery.SQLQuery.ReturnString("Select pid from [PrdnPolyMaster] where ProductionID='' AND EntryBy='" + lName + "'");
            if (isExist == "")
            {
                SqlCommand cmd2 = new SqlCommand(
                        "INSERT INTO [PrdnPolyMaster] (ProductionID, EntryBy) VALUES (@ProductionID, @EntryBy)",
                        new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd2.Parameters.AddWithValue("@ProductionID", "");
                cmd2.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());
                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
            }
            else
            {
                SqlCommand cmd = new SqlCommand(
                        @"SELECT Date, Shift, MachineNo, LineNumber, Purpose, CustomerID, Brand, PackSize, Operation, OperatorID, StartTime, 
                         EndTime, WorkingHour, TimeWaste, ReasonWaist, CalcProduction, ActProduction, Rejection, NetProduction,TotalRolling, ItemWeight, TotalWeight, ActualWeight,TotalScaleWeight, ItemSubGroup, Grade, 
                         Category, ItemID, Color, FinalProduction, WasteWeight, ReUsableItem, Remarks FROM [PrdnPolyMaster] WHERE ProductionID=''  AND EntryBy='" + lName + "' ",
                        new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtDate.Text = Convert.ToDateTime(dr[0].ToString()).ToString("dd/MM/yyyy");
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

                    txtSTime.Text = dr[10].ToString();
                    txtETime.Text = dr[11].ToString();
                    txtHour.Text = dr[12].ToString();

                    txtTimeWaist.Text = dr[13].ToString();
                    txtReason.Text = dr[14].ToString();
                    txtRolling.Text = dr[15].ToString();
                    txtproduced.Text = dr[16].ToString();
                    txtRejected.Text = dr[17].ToString();

                    txtFinalProd.Text = dr[18].ToString();
                    //txtWeightPc.Text = dr[19].ToString();
                    //txtNetWeight.Text = dr[20].ToString();
                    txtScaleWeight.Text = dr[21].ToString();

                    ddOutSubGroup.SelectedValue = dr[22].ToString();
                    string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" +
                                    ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text +
                                    "' ORDER BY [GradeName]";
                    RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

                    ddOutGrade.SelectedValue = dr[23].ToString();
                    gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" +
                             ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text +
                             "' ORDER BY [CategoryName]";
                    RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");

                    ddOutCategory.SelectedValue = dr[24].ToString();
                    gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" +
                             ddOutCategory.SelectedValue + "' ORDER BY [ItemName]";
                    RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");

                    ddOutItem.SelectedValue = dr[25].ToString();

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
        }
        catch (Exception ex)
        {

        }
    }

    private void LoadDropDowns()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGradeRaw, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");

        //Processed Item
        gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where CategoryID='13' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutSubGroup, "CategoryID", "CategoryName");

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");

        ddWasteStock.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            CalcConsumption();
            //decimal inputAvailable = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InWeight)-Sum(OutWeight),0) FROM Stock where  ProductID='" + ddItemNameRaw.SelectedValue + "'"));

            //string ttlWeight = RunQuery.SQLQuery.ReturnString("Select SUM(TotalWeight)+SUM(WaistWeight) from [PrdnPolyDetails] where PrdnID='' ");

            //if (inputAvailable >= Convert.ToDecimal(txtInputQtyKG.Text))
            //{
            //if (Convert.ToDecimal(txtInputQtyKG.Text) - 1 <= Convert.ToDecimal(ttlWeight))
            //{
            if (btnSave.Text == "Save")
            {
                SaveProduction();
                LoadPendingEntries();
                GridView1.DataBind();
                Bind2ndGrid();

                string dt = txtDate.Text;
                txtDate.Text = dt;

                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Production saved successfully";
            }
            else
            {

                SQLQuery.ExecNonQry("Delete Stock WHERE InvoiceID ='" + lblPrId.Text + "'   ");
                UpdateProduction(" WHERE ProductionID='" + lblPrId.Text + "' ");

                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Entry updated successfully";
            }

            StockEntry();
            ClearControls(Form);
            StockDetails();
            ClearMasterArea();

            //}
            //else
            //{
            //lblMsg.Attributes.Add("class", "xerp_error");
            //lblMsg.Text = "Error: Input weight & output weight is not equel!";
            //}
            //}
            //else
            //{
            //lblMsg.Attributes.Add("class", "xerp_error");
            //lblMsg.Text = "Error: Input quantity is not available into stock!";
            //}
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_warning");
            lblMsg.Text = "Error: " + ex.ToString();
        }
    }

    private void SaveProduction()
    {
        string prdId = RunQuery.SQLQuery.ReturnString("Select 'PRD-PB-'+ CONVERT(varchar, (ISNULL (max(pid),0)+1001 )) from [PrdnPolyMaster]");
        lblPrId.Text = prdId;

        UpdateProduction(" WHERE ProductionID='' AND EntryBy='" + User.Identity.Name + "' ");

    }

    private void UpdateProduction(string query)
    {
        string lName = Page.User.Identity.Name.ToString();
        RunQuery.SQLQuery.ExecNonQry(@"Update [PrdnPolyMaster] SET Date='" +
                                     Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") +
                                     @"', Shift='" +
                                     ddShift.SelectedValue +
                                     "', MachineNo='" +
                                     ddMachine.SelectedValue +
                                     "', LineNumber='', Purpose='" +
                                     ddPurpose.SelectedValue +
                                     "', CustomerID='" +
                                     ddCustomer.SelectedValue +
                                     "', CustomerName='" +
                                     ddCustomer.SelectedItem.Text +
                                     "', Brand='" +
                                     ddBrand.SelectedValue +
                                     "', PackSize='" +
                                     ddSize.SelectedValue +
                                     "', PolyDimension='" +
                                     ddDimension.SelectedValue +
                                     "', Operation='" +
                                     ddOperation.SelectedValue +
                                     "', OperatorID='" +
                                     ddOperator.SelectedValue +
                                     "' " + query + " ");

      
        RunQuery.SQLQuery.Empty2Zero(txtHour);
        RunQuery.SQLQuery.Empty2Zero(txtMin);
        RunQuery.SQLQuery.Empty2Zero(txtTimeWaist);
        RunQuery.SQLQuery.Empty2Zero(txtRolling);
        RunQuery.SQLQuery.Empty2Zero(txtproduced);
        RunQuery.SQLQuery.Empty2Zero(txtRejected);
        RunQuery.SQLQuery.Empty2Zero(txtFinalProd);
        //RunQuery.SQLQuery.Empty2Zero(txtWeightPc);
        //RunQuery.SQLQuery.Empty2Zero(txtNetWeight);
        RunQuery.SQLQuery.Empty2Zero(txtScaleWeight);

        // string sTime = txtSTime.Text;
        //  string eTime = txtETime.Text;
        //TimeSpan sTime=TimeSpan.Parse(txtSTime.Text);
        //TimeSpan eTime=TimeSpan.Parse(txtETime.Text);
        //string sTime= DateTime.Now.ToString("yyyy-MM-dd") + " " + txtSTime.Text;
        //string eTime= DateTime.Now.ToString("yyyy-MM-dd") + " " + txtETime.Text;



        RunQuery.SQLQuery.ExecNonQry(@"Update [PrdnPolyMaster] SET  StartTime='" + txtSTime.Text +
                                     "',  EndTime='" + txtETime.Text +
                                     "', WorkingHour='" + txtHour.Text +
                                     "', WorkingMin='" + txtMin.Text +
                                     "', TimeWaste='" +
                                     txtTimeWaist.Text +
                                     "', ReasonWaist='" +
                                     txtReason.Text +
                                     "',  ActProduction='" +
                                     txtproduced.Text +
                                     "', Rejection='" +
                                     txtRejected.Text +
                                     "', NetProduction='" +
                                     txtFinalProd.Text +
                                     "', TotalRolling='" + txtRolling.Text +
                                     "', TotalScaleWeight='" + txtScaleWeight.Text +
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

        RunQuery.SQLQuery.ExecNonQry(@"Update [PrdnPolyMaster] SET ItemSubGroup='" +
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
                                     "', WastePCS='" +
                                     txtWastagePCS.Text +
                                     "',WasteWeight='" +
                                     txtTotalWestage.Text +
                                     "', ReUsableItem='" + ddWasteStock.Text +
                                     "', NonusableWeight='" + txtNonUsable.Text +
                                     "', Remarks ='" +
                                     txtRemark.Text +
                                     "' " + query + " ");
    }

    private void StockEntry()
    {
        //Item Stock Entry
        RunQuery.SQLQuery.ExecNonQry("DELETE Stock WHERE InvoiceID='" + lblPrId.Text + "'");
        RunQuery.SQLQuery.ExecNonQry("UPDATE [PrdnPolyDetails] SET PrdnID='" + lblPrId.Text + "' WHERE PrdnID=''  AND EntryBy='" + lblUser.Text + "'  ");
        RunQuery.SQLQuery.ExecNonQry("UPDATE [PrdnPolyMaster] SET ProductionID='" + lblPrId.Text + "' WHERE ProductionID=''  AND EntryBy='" + lblUser.Text + "' ");

        //Input Item Stock-out
        DataTable citydt = RunQuery.SQLQuery.ReturnDataTable("Select Purpose, SizeId, BrandID, ProductID, ProductName, TotalWeight, ConsumedWeight from [PrdnPolyDetails] where PrdnID='" + lblPrId.Text + "'");

        foreach (DataRow citydr in citydt.Rows)
        {
            string Purpose = citydr["Purpose"].ToString();
            string SizeId = ddSize.SelectedValue;
            string BrandID = ddBrand.SelectedValue;
            string ProductID = citydr["ProductID"].ToString();
            string ProductName = citydr["ProductName"].ToString();

            decimal TotalWeight = Convert.ToDecimal(citydr["ConsumedWeight"].ToString());
            //decimal WaistWeight = Convert.ToDecimal(txtTotalWestage.Text);// Convert.ToDecimal(citydr["ConsumedWeight"].ToString()) * 1000;

            //Input Item stock-out
            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- Plastic Container", lblPrId.Text, SizeId,
                    ddCustomer.SelectedValue, BrandID, "", "", ProductID, ProductName, "", ddGodown.SelectedValue, ddLocation.SelectedValue, "1", 0, 0, 0, 0, Convert.ToDecimal(TotalWeight), "", "Stock-Out",
                    "Production Input", "Round Tin", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        }

        SQLQuery.Empty2Zero(txtTotalWestage);
        if (Convert.ToDecimal(txtTotalWestage.Text) > 0)
        {
            //rejUsedKg = Convert.ToDecimal(ReUsableQty) * Convert.ToDecimal(Stock.Inventory.RawWeightQtyRatio("", "", ReusableItem, "8"));
            Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Plastic Container", lblPrId.Text, ddSize.SelectedValue,
                ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ddWasteStock.SelectedValue,
                Stock.Inventory.GetProductName(ddWasteStock.SelectedValue), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", 0, 0, 0,
                Convert.ToDecimal(txtTotalWestage.Text), 0, "", "Stock-In", "Wastage Item", "Plastic Container", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        }

        //Output Item stock-in
        if (Convert.ToDecimal(txtOutputKg.Text) > 0)
        {
            Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Poly Section", lblPrId.Text, ddSize.SelectedValue,
                ddCustomer.SelectedValue, ddBrand.SelectedValue, ddColor.SelectedValue, "", ddOutItem.SelectedValue, ddOutItem.SelectedItem.Text, "", ddGodown.SelectedValue, ddLocation.SelectedValue, "3", Convert.ToInt32(txtFinalOutput.Text), 0, 0, Convert.ToDecimal(txtOutputKg.Text), 0, "", "Stock-in",
                "Production Output", "Poly Section", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));


            decimal totalConsumption = 100;
            decimal unitPrice = totalConsumption / (Convert.ToInt32(txtFinalOutput.Text));
            decimal unitWeight = Convert.ToDecimal(txtOutputKg.Text) / Convert.ToInt32(txtFinalOutput.Text);
            string  fifoType = "SemiFinished";
            //Stock.Inventory.FifoInsert(ddOutItem.SelectedValue, "ProductionOutput", ddCustomer.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddBrand.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd"), fifoType, lblPrId.Text, unitPrice, unitWeight, Convert.ToDecimal(txtFinalOutput.Text), ddGodown.SelectedValue, ddLocation.SelectedValue, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "", "", "0");
            Stock.Inventory.FifoInsert(ddOutItem.SelectedValue, "ProductionOutput", ddCustomer.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddBrand.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd"), fifoType, lblPrId.Text, 0, Convert.ToDecimal(txtOutputKg.Text), Convert.ToDecimal(txtFinalOutput.Text), ddGodown.SelectedValue, "",
                Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "Poly Production Output", "", unitPrice.ToString());
        }

        RunQuery.SQLQuery.Empty2Zero(txtNonUsable);
        string nonUsableWeight = txtNonUsable.Text; //RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(NonusableWaste),0) from PrdnRoundTinDetails where PrdnID='" + lblPrId.Text + "'");
        if (Convert.ToDecimal(nonUsableWeight) > 0)
        {
            Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Plastic Container", lblPrId.Text,
                ddSize.SelectedValue, ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", "1808", "Nonusable Westage", "", ddGodown.SelectedValue, ddLocation.SelectedValue,
                "7", 0, 0, 0, Convert.ToDecimal(nonUsableWeight), 0, "", "Stock-in", "Production Output", "Plastic Container",
                Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        }

    }

    protected void ddOutItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }
    protected void ddSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        string godownId = SQLQuery.ReturnString("SELECT GodownID FROM Sections WHERE SID = '" + ddSection.SelectedValue + "'");
        ddGodown.DataBind();
        ddGodown.SelectedValue = godownId;
        string locationID = SQLQuery.ReturnString("SELECT LocationID FROM Sections WHERE SID = '" + ddSection.SelectedValue + "'");
        ddLocation.DataBind();
        ddLocation.SelectedValue = locationID;
        ddShift.DataBind();
        ddMachine.DataBind();
        //ddLine.DataBind();
        StockDetails();
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
        StockDetails();
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

        gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddOutCategory.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
        StockDetails();
    }

    private decimal StockDetails()
    {
        decimal availableWeight = 0;
        //ddGroup.SelectedValue = Stock.Inventory.GetItemGroup(ddItemNameRaw.SelectedValue);
        if (ddGroup.SelectedValue == "1")
        {
            availableWeight = Convert.ToDecimal(Stock.Inventory.PlasticRawWeight(ddPurpose2.SelectedValue, ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue)); ;
            ltrLastInfo.Text = "Stock Available: " + availableWeight + "KG";
        }
        else
        {
            availableWeight = Convert.ToDecimal(Stock.Inventory.NonUsableWeight(ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue)); ;
            ltrLastInfo.Text = "Stock Available: " + availableWeight + "KG";
        }
       // GetStdPrdn();
        return availableWeight;
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
            SQLQuery.Empty2Zero(txtRejected);

            if (txtInputQtyKG.Text != "" && txtInputQtyKG.Text != "0")
            {
                if (btnAdd.Text == "Add")
                {
                    //SqlCommand cmde = new SqlCommand("SELECT ProductID FROM [PrdnPolyDetails] WHERE ProductID ='" + ddItemNameRaw.SelectedValue + "' AND   PrdnID =''", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    //cmde.Connection.Open();
                    //string isExist = Convert.ToString(cmde.ExecuteScalar());
                    //cmde.Connection.Close();

                    //if (isExist == "")
                    //{

                    if (StockDetails() >= Convert.ToDecimal(txtInputQtyKG.Text) || Stock.Inventory.StockEnabled() == "0")
                    {
                        InsertDetailData();
                        ClearDetailArea();

                        lblMsg2.Attributes.Add("class", "xerp_success");
                        lblMsg2.Text = "New product added successfully";
                    }

                    else
                    {
                        lblMsg2.Attributes.Add("class", "xerp_error");
                        lblMsg2.Text = "Error: Item is not available into stock!";
                    }
                    //}
                    //else
                    //{
                    //    lblMsg2.Attributes.Add("class", "xerp_error");
                    //    lblMsg2.Text = "Error: Item already exist!";
                    //}
                }
                else
                {
                    //RunQuery.SQLQuery.ExecNonQry("Delete [PrdnPolyDetails] where id='" + lblEid.Text + "'");
                    UpdateDetailData(lblEid.Text);
                    //Accounting.VoucherEntry.ProductionItemEntry("", "Plastic Container", ddPurpose.SelectedValue, ddSize.SelectedValue, "", ddProduct.SelectedValue, ddProduct.SelectedItem.Text, txtThickness.Text, txtproduced.Text, txtPackPerSheet.Text, txtSheetWeight.Text, txtRejected.Text, Page.User.Identity.Name.ToString());
                    btnAdd.Text = "Add";
                    ClearDetailArea();

                    lblMsg2.Attributes.Add("class", "xerp_success");
                    lblMsg2.Text = "Entry updated successfully";
                }
                if (btnSave.Text == "Save")
                {
                    UpdateProduction(" WHERE ProductionID='' AND EntryBy='" + User.Identity.Name + "' ");
                }
                else
                {
                    UpdateProduction(" WHERE ProductionID='" + lblPrId.Text + "' ");
                }
                CalcConsumption();
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

    private void InsertDetailData()
    {
        SqlCommand cmd3 = new SqlCommand("INSERT INTO [PrdnPolyDetails] (PrdnID, Department, Purpose, SizeId, BrandID, ItemGroup, SubGroup, Grade, Category, ProductID, ProductName, TotalWeight, ConsumedWeight, EntryBy)" +
                                                        " VALUES (@PrdnID, @Department, @Purpose, @SizeId, @BrandID, @ItemGroup, @SubGroup, @Grade, @Category, @ProductID, @ProductName, @TotalWeight, @ConsumedWeight, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd3.Parameters.AddWithValue("@PrdnID", "");
        cmd3.Parameters.AddWithValue("@Department", "");
        cmd3.Parameters.AddWithValue("@Purpose", ddPurpose2.SelectedValue);
        cmd3.Parameters.AddWithValue("@SizeID", "");
        cmd3.Parameters.AddWithValue("@BrandID", "");

        cmd3.Parameters.AddWithValue("@ItemGroup", ddGroup.SelectedValue);
        cmd3.Parameters.AddWithValue("@SubGroup", ddSubGrp.SelectedValue);
        cmd3.Parameters.AddWithValue("@Grade", ddGradeRaw.SelectedValue);
        cmd3.Parameters.AddWithValue("@Category", ddCategoryRaw.SelectedValue);

        cmd3.Parameters.AddWithValue("@ProductID", ddItemNameRaw.SelectedValue);
        cmd3.Parameters.AddWithValue("@ProductName", ddItemNameRaw.SelectedItem.Text);
        cmd3.Parameters.AddWithValue("@TotalWeight", Convert.ToDecimal(txtInputQtyKG.Text));

        cmd3.Parameters.AddWithValue("@ConsumedWeight", 0);
        cmd3.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        cmd3.Connection.Open();
        cmd3.ExecuteNonQuery();
        cmd3.Connection.Close();

        SqlCommand command = new SqlCommand("INSERT INTO [PrdnPolyMaster] (ProductionID, EntryBy)" +
                                                        " VALUES (@ProductionID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        command.Parameters.AddWithValue("@ProductionID", "");
        command.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();

    }
    private void UpdateDetailData(string entryID)
    {
        RunQuery.SQLQuery.ExecNonQry(@"UPDATE [dbo].[PrdnPolyDetails]   SET 
      [Purpose] = '" + ddPurpose2.SelectedValue + "' ,[SizeId] = ''" +
                                     ",[BrandID] = '' , [ItemGroup] = '" +
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
        Bind2ndGrid();
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
        SqlCommand cmd = new SqlCommand("SELECT Department, Purpose, SizeId, BrandID, ProductID, TotalWeight, ConsumedWeight, ItemGroup, SubGroup, Grade, Category FROM [PrdnPolyDetails] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            ddPurpose2.SelectedValue = dr[1].ToString();
            //LoadDropDowns();
            //ddSubGrp.SelectedValue = dr[1].ToString();
            //GradeCategoryList();
            //ddGradeRaw.SelectedValue = dr[2].ToString();

            //string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");

            //ddCategoryRaw.SelectedValue = dr[3].ToString();
            //GetProductList();
            //ddItemNameRaw.SelectedValue = dr[4].ToString();
            txtInputQtyKG.Text = dr[5].ToString();
            //txtSheetQty.Text = dr[6].ToString();

            ddGroup.SelectedValue = dr[7].ToString();
            LoadDropDowns();
            ddSubGrp.SelectedValue = dr[8].ToString();
            GradeCategoryList();
            ddGradeRaw.SelectedValue = dr[9].ToString();

            string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");

            ddCategoryRaw.SelectedValue = dr[10].ToString();
            GetProductList();
            ddItemNameRaw.SelectedValue = dr[4].ToString();
        }
        cmd.Connection.Close();

    }
    protected void GridView2_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView2.Rows[index].FindControl("Label1") as Label;

            SqlCommand cmd7 = new SqlCommand("DELETE [PrdnPolyDetails] WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();

            Bind2ndGrid();

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
        //string ttlWeight = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(TotalWeight),0) from [PrdnPolyDetails] where PrdnID='' ");

        //if (Convert.ToDecimal(ttlWeight) > 0)
        //{
        //    lblTotalWeight.Text = "<b>Total Output Weight: </b>" + ttlWeight + " kg.";
        //}

        //txtFinalProd.Text = RunQuery.SQLQuery.ReturnString("Select SUM(PrdnPcs) from [PrdnPolyDetails] where PrdnID='' ");

    }

    private void CalcConsumption()
    {
        try
        {
            string prdnNo = "";
            if (btnSave.Text == "Update")
            {
                prdnNo = lblPrId.Text;
            }
            RunQuery.SQLQuery.Empty2Zero(txtproduced);
            //Calculate Consumption of the item into total production
            decimal ttlConsumed = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT ISNULL(SUM(TotalWeight),0) FROM [PrdnPolyDetails] Where PrdnID='" + prdnNo + "' AND EntryBy='" + User.Identity.Name + "' "));
            decimal actPrdn = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT ISNULL(ActualWeight,0) FROM [PrdnPolyMaster] Where ProductionID='" + prdnNo + "' AND EntryBy='" + User.Identity.Name + "' "));
            //if (txtActualWeight.Text != "")
            //{
            //    actPrdn = Convert.ToDecimal(txtActualWeight.Text);
            //}
            DataTable prdData = RunQuery.SQLQuery.ReturnDataTable("Select Id, TotalWeight FROM [PrdnPolyDetails] Where PrdnID='" + prdnNo + "' AND EntryBy='" + User.Identity.Name + "'  ");

            foreach (DataRow row in prdData.Rows)
            {
                string entryID = row["Id"].ToString();
                decimal iWeight = Convert.ToDecimal(row["TotalWeight"].ToString());

                //decimal consumption = (iWeight * actPrdn) / ttlConsumed;
                decimal consumption = (iWeight * actPrdn) / 100;

                RunQuery.SQLQuery.ExecNonQry("UPDATE [PrdnPolyDetails] SET ConsumedWeight='" + consumption + "' WHERE id='" + entryID + "'");
                Bind2ndGrid();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Consumption calculation failed : " + ex.Message.ToString();
        }

    }

    protected void ddCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand.Items.Clear();
        ListItem lst = new ListItem("--- all ---", "");
        ddBrand.Items.Insert(ddBrand.Items.Count, lst);
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
    }

    private void GradeCategoryList()
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGradeRaw, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
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
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");
        GetProductList();
    }

    protected void ddOutItem_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddPurpose2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddOperation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }


    private void Bind2ndGrid()
    {
        string query = " WHERE PrdnID='' AND EntryBy='" + User.Identity.Name + "' ";
        if (btnSave.Text == "Update")
        {
            query = " WHERE PrdnID='" + lblPrId.Text + "' ";
        }

        query = @"SELECT Id, (Select Purpose from Purpose where pid = a.Purpose) AS[Purpose2], 
                                     (Select GroupName from ItemGroup where GroupSrNo = a.ItemGroup) AS Department,  
                                      (Select CategoryName from ItemSubGroup where CategoryID = a.SubGroup) AS Purpose,    
                                        (SELECT[GradeName] FROM[ItemGrade] WHERE[GradeID] = a.Grade) AS SizeId, 
                                     (SELECT[CategoryName] FROM[Categories] WHERE[CategoryID] = a.Category) AS BrandID,
                                    ProductID, ProductName, TotalWeight, ConsumedWeight FROM [PrdnPolyDetails] a " + query;
        SQLQuery.PopulateGridView(GridView2, query);
        //CalcConsumption();
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

        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1) pid, ProductionID, Date, Shift, MachineNo, LineNumber, Purpose, CustomerID, CustomerName, Brand, 
            PackSize, Operation, OperatorID, StartTime, EndTime, CycleTime, CavityPcs, WorkingHour, WorkingMin, TimeWaste, ReasonWaist, 
                         CalcProduction, ActProduction, Rejection, NetProduction,TotalRolling, ItemWeight, TotalWeight, ActualWeight,TotalScaleWeight, 
ItemSubGroup, Grade, Category, ItemID, ItemName, Color, FinalProduction, FinalKg, WasteWeight, ReUsableItem, NonusableWeight, Remarks, EntryBy, EntryDate
FROM            [PrdnPolyMaster] WHERE (pid = '" + itemToEdit + "')");
        foreach (DataRow drx in dtx.Rows)
        {
            lblPrId.Text = drx["ProductionID"].ToString();
            Bind2ndGrid();
            txtDate.Text = Convert.ToDateTime(drx["Date"].ToString()).ToString("dd/MM/yyyy");

            ddSection.SelectedValue = SQLQuery.ReturnString("Select Section from Shifts where Departmentid='" + drx["Shift"].ToString() + "'");
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
            txtSTime.Text = drx["StartTime"].ToString();

            txtETime.Text = drx["EndTime"].ToString();
            txtHour.Text = drx["WorkingHour"].ToString();
            txtMin.Text = drx["WorkingMin"].ToString();
            txtTimeWaist.Text = drx["TimeWaste"].ToString();
            txtReason.Text = drx["ReasonWaist"].ToString();

            txtRolling.Text = drx["TotalRolling"].ToString();
            txtproduced.Text = drx["ActProduction"].ToString();
            txtRejected.Text = drx["Rejection"].ToString();
            txtFinalProd.Text = drx["NetProduction"].ToString();

            //txtWeightPc.Text = drx["ItemWeight"].ToString();
            //txtNetWeight.Text = drx["TotalWeight"].ToString();
            txtScaleWeight.Text = drx["TotalScaleWeight"].ToString();

            // Generate subgrp under grp
            ddOutSubGroup.SelectedValue = drx["ItemSubGroup"].ToString();

            string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" +
                            ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text +
                            "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

            ddOutGrade.SelectedValue = drx["Grade"].ToString();
            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" +
                     ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text +
                     "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");
            ddOutCategory.SelectedValue = drx["Category"].ToString();

            gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" +
                     ddOutCategory.SelectedValue + "' ORDER BY [ItemName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
            ddOutItem.SelectedValue = drx["ItemID"].ToString();

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

            string isExist = RunQuery.SQLQuery.ReturnString("Select ProductionID FROM [PrdnPolyMaster] WHERE pid='" + lblItemCode.Text + "'");

            SQLQuery.ExecNonQry("Delete [PrdnPolyMaster] where ProductionID ='" + isExist + "'   ");
            SQLQuery.ExecNonQry("Delete [PrdnPolyDetails] where PrdnID='" + isExist + "'   ");
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

        gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddOutCategory.SelectedValue + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");

    }

    protected void txtETime_OnTextChanged(object sender, EventArgs e)
    {
        //string startTime;
        //string endTime;
        //startTime = txtSTime.Text;
        //endTime = txtETime.Text;


        //DateTime sTime = DateTime.Parse(startTime);
        //DateTime eTime = DateTime.Parse(endTime);
        //TimeSpan ts = eTime.Subtract(sTime);
        //txtHour.Text = ts.ToString();
        var time1 = txtSTime.Text;
        var time2 = txtETime.Text;


        var hour = 00;
        var minute = 00;
        var second = 00;


        var splitTime1 = time1.Split(':');
        var splitTime2 = time2.Split(':');
     


        hour = Convert.ToInt32(splitTime1[0]) + Convert.ToInt32(splitTime2[0]);
        minute = Convert.ToInt32(splitTime1[1]) + Convert.ToInt32(splitTime2[1]);
        hour = hour + minute / 60;
        minute = minute % 60;
       // second = Convert.ToInt32(splitTime1[2]) + Convert.ToInt32(splitTime2[2]);
        minute = minute + second / 60;
       second = second % 60;
     
        txtHour.Text= hour.ToString();
        txtMin.Text= minute.ToString();
    }
    protected void ddGodown_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocation.DataBind();
    }
}
