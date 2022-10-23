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


public partial class app_Round_Tin : System.Web.UI.Page
{
    private object cmde;

    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");        

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Today.Date.ToShortDateString();
            string lName = Page.User.Identity.Name.ToString();
            lblProject.Text = RunQuery.SQLQuery.ReturnString("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'");

            string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where CategoryID='14' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGradeRaw, "GradeID", "GradeName");

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");

            //Processed Item
            gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where CategoryID='17' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutSubGroup, "CategoryID", "CategoryName");

            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");

            ddCustomer.DataBind();
            ddBrand.DataBind();

            GetProcessedList();
            GetFinishedList();
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

    private void Notify(string msg, string type)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableProcessedQty(ddPurpose.SelectedValue, "", ddOutItem.SelectedValue,
            //        ddCompany2.SelectedValue, ddBrand2.SelectedValue, ddInputPack.SelectedValue, ddColor.SelectedValue, "4"));
            //decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableProcessedWeight(ddPurpose.SelectedValue, "", ddOutItem.SelectedValue,
            //                ddCompany2.SelectedValue, ddBrand2.SelectedValue, ddInputPack.SelectedValue, ddColor.SelectedValue, "4"));

            //if (inputAvailable >= Convert.ToDecimal(txtInputQtyPCS.Text) && inputAvailableKg >= Convert.ToDecimal(txtInputQtyKG.Text))
            //{
            if (btnSave.Text == "Save")
            {
                SaveProduction();
                UpdateProduction();
                GridView1.DataBind();
                ClearMasterArea();
                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Production saved successfully";
                Notify("Production saved successfully", "success");
            }
            else
            {
                UpdateProduction();
                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Entry updated successfully";
                Notify("Entry updated successfully", "success");
            }

            //}
            //else
            //{
            //    lblMsg.Attributes.Add("class", "xerp_error");
            //    lblMsg.Text = "Error: Input item is not available into stock!";
            //    Notify("Input item is not available into stock!", "error");
            //}

            ////else
            ////{
            ////    lblMsg.Attributes.Add("class", "xerp_error");
            ////    lblMsg.Text = "Error: Input quantity is not available into stock!";
            ////}
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
        string lName = Page.User.Identity.Name.ToString();
        string prdId = RunQuery.SQLQuery.ReturnString("Select 'PRD-RT-'+ CONVERT(varchar, (ISNULL (max(pid),0)+1001 )) from PrdnRoundTin");
        lblPrId.Text = prdId;

        SqlCommand cmd2 = new SqlCommand("INSERT INTO PrdnRoundTin (ProductionID, EntryBy) VALUES (@ProductionID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductionID", prdId);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void UpdateProduction()
    {
        RunQuery.SQLQuery.ExecNonQry(@"Update PrdnRoundTin SET Date='" +
                                     Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") +
                                     "', MachineNo='" + ddMachine.SelectedValue +
                                     "', LineNumber='" + ddLine.SelectedValue + "', Shift='" + ddShift.SelectedValue +
                                     "', Operator='" + ddOperator.SelectedValue + "', Purpose='" + ddPurpose.SelectedValue +
                                     "', CustomerID='" + ddCustomer.SelectedValue +
                                     "', Brand='" + ddBrand.SelectedValue + "',  PackSize='" + ddSize.SelectedValue +
                                     "',  ItemSubGroup='" + ddSubGrp.SelectedValue + "', ItemGrade='" +
                                     ddGradeRaw.SelectedValue + "',  ItemCategory='" + ddCategoryRaw.SelectedValue +
                                     "',  ItemName='" +
                                     ddItemNameRaw.SelectedValue + "', FinalOutputQty='" + txtFinalQty.Text +
                                     "', WeightPerPc='" + txtWeightPerPc.Text + "', OutputWeight='" +
                                     txtFinalProd.Text + "', WorkingHour='" + txtHour.Text +
                                     "', TimeWaste='" + txtTimeWaist.Text + "', ReasonWaist='" +
                                     txtReason.Text + "',  Remarks ='" +
                                     txtRemark.Text + "' WHERE ProductionID='" + lblPrId.Text + "' ");

        //Item Stock Entry
        //RunQuery.SQLQuery.ExecNonQry("DELETE Stock WHERE InvoiceID='" + lblPrId.Text + "'");
        RunQuery.SQLQuery.ExecNonQry("UPDATE PrdnRoundTinDetails SET PrdnID='" + lblPrId.Text + "' WHERE PrdnID=''");


        //Input Item stock-out
        DataTable citydt = RunQuery.SQLQuery.ReturnDataTable(@"Select   Item, Color, Company, Brand, PackSize, WeightPerPc, InputQty, InputWeight, Production, Rejected, NetProduction, 
                         ReusableWasteItem, ReusableWasteQty, NonusableWaste  from PrdnRoundTinDetails where PrdnID='" + lblPrId.Text + "'");

        foreach (DataRow citydr in citydt.Rows)
        {
            string Purpose = ddPurpose.SelectedValue;
            string Item = citydr["Item"].ToString();
            string Color = citydr["Color"].ToString();

            string Company = citydr["Company"].ToString();
            string Brand = citydr["Brand"].ToString();
            string PackSize = citydr["PackSize"].ToString();

            string WeightPerPc = citydr["WeightPerPc"].ToString();
            string InputQty = citydr["InputQty"].ToString();
            string InputWeight = citydr["InputWeight"].ToString();

            string Production = citydr["Production"].ToString();
            string Rejected = citydr["Rejected"].ToString();
            string NetProduction = citydr["NetProduction"].ToString();
            string ReusableWasteItem = citydr["ReusableWasteItem"].ToString();
            string ReusableWasteQty = citydr["ReusableWasteQty"].ToString();
            string NonusableWaste = citydr["NonusableWaste"].ToString();

            string ItemName = Stock.Inventory.GetProductName(Item);
            //decimal rejUsedKg = Convert.ToDecimal(RejectedUsed) * Convert.ToDecimal(Stock.Inventory.RawWeightQtyRatio("", "", ReusableItemUsed, "4"));
            decimal price = 0;// Inventory.LastNonprintedPrice(Purpose, itemType, Item);

            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- Round Tin", lblPrId.Text, PackSize,
                Company, Brand, Color, "", Item, ItemName, "", ddGodown.SelectedValue,
                ddLocation.SelectedValue, "3", 0, Convert.ToInt32(InputQty), price, 0, Convert.ToDecimal(InputWeight), "", "Stock-Out",
                "Processed used", "Round Tin", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            //Stock In

            decimal reUseKg = Convert.ToDecimal(WeightPerPc) * Convert.ToDecimal(ReusableWasteQty) / 1000M;
            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- Round Tin", lblPrId.Text, "",
                ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ReusableWasteItem,
                Stock.Inventory.GetProductName(ReusableWasteItem), "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", Convert.ToInt32(ReusableWasteQty), 0, 0,
                Convert.ToDecimal(reUseKg), 0, "", "Stock-In", "Wastage Item", "Round Tin",
                Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        }

        string nonUsableWeight = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(NonusableWaste),0) from PrdnRoundTinDetails where PrdnID='" + lblPrId.Text + "'");

        Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Round Tin", lblPrId.Text,
            "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", "1809", "Nonusable Westage", "", ddGodown.SelectedValue, ddLocation.SelectedValue,
            "7", 0, 0, 0, Convert.ToDecimal(nonUsableWeight), 0, "", "Stock-in", "Production", "Round Tin",
            Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));


        //Output Stock-in

        Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Round Tin", lblPrId.Text, ddSize.SelectedValue,
            ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ddItemNameRaw.SelectedValue,
            ddItemNameRaw.SelectedItem.Text, "", ddGodown.SelectedValue, ddLocation.SelectedValue, "2", Convert.ToInt32(txtFinalQty.Text), 0, 0,
            Convert.ToDecimal(txtFinalProd.Text), 0, "", "Stock-in", "Production", "Round Tin",
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

        GetFinishedList();
    }
    protected void ddCategoryRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddOutCategory.SelectedValue = ddCategoryRaw.SelectedValue;
        GetFinishedList();
    }
    protected void ddItemNameRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
        //LoadFinishedDD();
    }

    private void LoadFinishedDD()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddOutCategory.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
        StockDetails();
    }

    private void GetProcessedList()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddOutCategory.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutItem, "ProductID", "ItemName");
        StockDetails();
    }
    private void GetFinishedList()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategoryRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemNameRaw, "ProductID", "ItemName");
        StockDetails();
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
            RunQuery.SQLQuery.Empty2Zero(txtReusableWasteQty);
            string lName = Page.User.Identity.Name.ToString();

            decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableProcessedQty(ddPurpose.SelectedValue, "", ddOutItem.SelectedValue,
                    ddCompany2.SelectedValue, ddBrand2.SelectedValue, ddInputPack.SelectedValue, ddColor.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
            decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableProcessedWeight(ddPurpose.SelectedValue, "", ddOutItem.SelectedValue,
                            ddCompany2.SelectedValue, ddBrand2.SelectedValue, ddInputPack.SelectedValue, ddColor.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

            if (inputAvailable >= Convert.ToDecimal(txtInputQtyPCS.Text) && inputAvailableKg >= Convert.ToDecimal(txtInputQtyKG.Text))
            {
                if (txtInputQtyKG.Text != "" && txtNetPrdn.Text != "" && txtproduced.Text != "")
                {
                    if (btnAdd.Text == "Add")
                    {
                        InsertDetailData();
                        ClearDetailArea();
                        //SqlCommand cmde = new SqlCommand("SELECT ProductID FROM ProductionDetails WHERE ProductID ='" + ddOutItem.SelectedValue + "' AND Department='Shearing' AND   PrdnID =''", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                        //cmde.Connection.Open();
                        //string 
                        //string isExist= Convert.ToString(cmde.ExecuteScalar());
                        //cmde.Connection.Close();

                        //if (isExist == "")
                        //{
                        //    Accounting.VoucherEntry.ProductionItemEntry("", "Shearing", ddPurpose.SelectedValue, ddSize.SelectedValue, "", ddOutItem.SelectedValue, ddOutItem.SelectedItem.Text, txtSheetQty.Text, txtproduced.Text, txtPackPerSheet.Text, txtSheetQty.Text, txtRejected.Text, Page.User.Identity.Name.ToString());

                        lblMsg2.Attributes.Add("class", "xerp_success");
                        lblMsg2.Text = "New record added successfully";
                        Notify("New record added successfully", "success");
                        ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
                        //}
                        //else
                        //{
                        //    lblMsg2.Attributes.Add("class", "xerp_error");
                        //    lblMsg2.Text = "Error: Item already exist!";
                        //}
                    }
                    else
                    {
                        RunQuery.SQLQuery.ExecNonQry("UPDATE PrdnRoundTinDetails SET MachineNo= '" + ddMachine.SelectedValue + "', Line='" + ddLine.SelectedValue + "', Shift='" + ddShift.SelectedValue + "',  Subgroup='" +
                                                     ddOutSubGroup.SelectedValue + "', Grade='" + ddOutGrade.SelectedValue +
                                                     "', Category='" + ddOutCategory.SelectedValue + "', Item='" +
                                                     ddOutItem.SelectedValue + "', Color='" + ddColor.SelectedValue +
                                                     "', Company= '" + ddCompany2.SelectedValue + "', Brand='" + ddBrand2.SelectedValue + "', PackSize='" + ddInputPack.SelectedValue + "', WeightPerPc='" + txtWeightPc.Text + "', InputQty='" +
                                                     txtInputQtyPCS.Text + "', InputWeight='" + txtInputQtyKG.Text +
                                                     "', Production='" + txtproduced.Text + "', Rejected='" +
                                                     txtRejected.Text + "', NetProduction='" + txtNetPrdn.Text +
                                                     "', ReusableWasteItem='" + ddReusableUsed.SelectedValue +
                                                     "', ReusableWasteQty='" + txtReusableWasteQty.Text + "' where id='" +
                                                     lblEid.Text + "'");
                        //RunQuery.SQLQuery.ExecNonQry("Delete PrdnRoundTinDetails where id='" + lblEid.Text + "'");
                        //InsertDetailData();
                        //Accounting.VoucherEntry.ProductionItemEntry("", "Shearing", ddPurpose.SelectedValue, ddSize.SelectedValue, "", ddOutItem.SelectedValue, ddOutItem.SelectedItem.Text, txtSheetQty.Text, txtproduced.Text, txtPackPerSheet.Text, txtSheetQty.Text, txtRejected.Text, Page.User.Identity.Name.ToString());
                        ClearDetailArea();
                        lblMsg2.Attributes.Add("class", "xerp_success");
                        lblMsg2.Text = "Entry updated successfully";
                        Notify("Entry updated successfully", "success");
                    }
                }
                else
                {
                    lblMsg2.Attributes.Add("class", "xerp_error");
                    lblMsg2.Text = "Please fillup all mendatory fields...";
                    Notify("Please fillup all mendatory fields...", "error");
                }
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Error: Input item is not available into stock!";
                Notify("Input item is not available into stock!", "error");
                txtInputQtyPCS.Focus();
            }

            //lblMsg2.Attributes.Add("class", "xerp_error");
            //lblMsg2.Text = "Error: Input weight & output weight must have to be equel!";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "Error: " + ex.Message.ToString();
            Notify("Error: " + ex.Message.ToString(), "error");
        }
        finally
        {
            GridView2.DataBind();
            ddOutSubGroup.Focus();
        }

    }

    private void InsertDetailData()
    {
        SqlCommand cmd3 =
            new SqlCommand(
                @"INSERT INTO PrdnRoundTinDetails (Section, PrdnID, MachineNo, Line, Shift, Subgroup, Grade, Category, Item, Color, Company, Brand, PackSize,  WeightPerPc, InputQty, InputWeight, Production, Rejected, NetProduction, ReusableWasteItem, ReusableWasteQty, EntryBy)" +
                " VALUES ('Round Tin', '', '" + ddMachine.SelectedValue + "', '" + ddLine.SelectedValue + "', '" +
                ddShift.SelectedValue + "', '" + ddOutSubGroup.SelectedValue + "', '" + ddOutGrade.SelectedValue +
                "', '" + ddOutCategory.SelectedValue + "', '" + ddOutItem.SelectedValue + "', '" + ddColor.SelectedValue +
                "', '" + ddCompany2.SelectedValue + "', '" + ddBrand2.SelectedValue + "', '" + ddInputPack.SelectedValue + "',  '" + txtWeightPc.Text + "', '" + txtInputQtyPCS.Text + "', '" + txtInputQtyKG.Text + "', '" +
                txtproduced.Text + "', '" + txtRejected.Text + "', '" + txtNetPrdn.Text + "', '" +
                ddReusableUsed.SelectedValue + "', '" + txtReusableWasteQty.Text + "', @EntryBy)",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd3.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());
        cmd3.Connection.Open();
        cmd3.ExecuteNonQuery();
        cmd3.Connection.Close();

        //ddOutItem.Items.Remove(ddOutItem.SelectedItem.Text);
        txtInputQtyPCS.Text = txtNetPrdn.Text;
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
        txtReusableWasteQty.Text = "";
        GridView2.DataBind();
        btnAdd.Text = "Add";
    }
    private void ClearMasterArea()
    {
        ClearDetailArea();
        txtFinalQty.Text = "";
        txtWeightPerPc.Text = "";
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
            Notify("Error: " + ex.Message.ToString(), "error");
        }
    }

    private void EditMode(string entryID)
    {
        SqlCommand cmd = new SqlCommand("SELECT Subgroup, Grade, Category, Item, Color, WeightPerPc, InputQty, InputWeight, Production, Rejected, NetProduction, ReusableWasteItem, ReusableWasteQty FROM [PrdnRoundTinDetails] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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

            ddReusableUsed.SelectedValue = dr[11].ToString();
            txtReusableWasteQty.Text = dr[12].ToString();
        }
        cmd.Connection.Close();

        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1) MachineNo, Line, Shift, Company, Brand, PackSize  FROM [PrdnRoundTinDetails] WHERE Id='" + entryID + "'");

        foreach (DataRow drx in dtx.Rows)
        {
            ddShift.SelectedValue = drx["Shift"].ToString();
            ddCompany2.SelectedValue = drx["Company"].ToString();
            ddBrand2.DataBind();
            ddBrand2.SelectedValue = drx["Brand"].ToString();
            ddInputPack.SelectedValue = drx["PackSize"].ToString();
            StockDetails();
        }
    }

    protected void GridView2_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView2.Rows[index].FindControl("Label1") as Label;

            SqlCommand cmd7 = new SqlCommand("DELETE PrdnRoundTinDetails WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
        try
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
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.ToString();
            Notify("Error: " + ex.Message.ToString(), "error");
        }
    }

    private void CalcNonusableWeight()
    {
        DataTable citydt = RunQuery.SQLQuery.ReturnDataTable("Select Id, WeightPerPc, Rejected, ReusableWasteQty from PrdnRoundTinDetails where PrdnID='' Order by id");
        int i = 1;

        foreach (DataRow citydr in citydt.Rows)
        {
            string id = citydr["Id"].ToString();
            string weightPerPc = citydr["WeightPerPc"].ToString();
            string rejected = citydr["Rejected"].ToString();
            string reusableWasteQty = citydr["ReusableWasteQty"].ToString();

            decimal nonUsableQty = (Convert.ToDecimal(rejected) - Convert.ToDecimal(reusableWasteQty));// * Convert.ToDecimal(weightPerPc);

            decimal nonUsableWeight = 0;
            DataTable dt2 = RunQuery.SQLQuery.ReturnDataTable("Select TOP(" + i + ") Id, WeightPerPc from PrdnRoundTinDetails  where PrdnID='' order by id");
            foreach (DataRow dr2 in dt2.Rows)
            {
                string id2 = dr2["Id"].ToString();
                string value = dr2["WeightPerPc"].ToString();
                nonUsableWeight = nonUsableWeight + (Convert.ToDecimal(value) * nonUsableQty);
            }
            RunQuery.SQLQuery.ExecNonQry("Update PrdnRoundTinDetails set NonusableWaste='" + (nonUsableWeight / 1000M) + "' WHERE  id='" + id + "'");
            i++;
        }

        txtWeightPerPc.Text = RunQuery.SQLQuery.ReturnString("Select SUM(WeightPerPc)  from PrdnRoundTinDetails  where PrdnID='' ");
        string lastPrdn = RunQuery.SQLQuery.ReturnString("Select NetProduction  from PrdnRoundTinDetails Where id=(Select ISNULL(MAX(id),0)  from PrdnRoundTinDetails  where PrdnID='') ");
        if (lastPrdn != "")
        {
            txtFinalQty.Text = lastPrdn;
            txtFinalProd.Text = Convert.ToString(Convert.ToDecimal(lastPrdn) * Convert.ToDecimal(txtWeightPerPc.Text) / 1000M);
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

    protected void DropDownList13_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand2.Items.Clear();
        ListItem lst = new ListItem("--- all ---", "");
        ddBrand2.Items.Insert(ddBrand2.Items.Count, lst);
        //ddBrand.Items.Add("--- all ---");
        ddBrand2.DataBind();
        StockDetails();
    }

    protected void ddOutGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddOutSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutGrade, "GradeID", "GradeName");
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddOutGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddOutCategory, "CategoryID", "CategoryName");
        GetProcessedList();
    }

    protected void ddOutCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetProcessedList();
    }

    protected void ddOutItem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddGodown_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocation.DataBind();
        StockDetails();
    }

    private void StockDetails()
    {
        decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableProcessedQty(ddPurpose.SelectedValue, "", ddOutItem.SelectedValue,
                    ddCompany2.SelectedValue, ddBrand2.SelectedValue, ddInputPack.SelectedValue, ddColor.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
        decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableProcessedWeight(ddPurpose.SelectedValue, "", ddOutItem.SelectedValue,
                        ddCompany2.SelectedValue, ddBrand2.SelectedValue, ddInputPack.SelectedValue, ddColor.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

        ltrLastInfo.Text = "Available Stock: " + inputAvailable + " PCS, " + inputAvailableKg + "KG";
    }


    protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void ddBrand_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            string isExist = RunQuery.SQLQuery.ReturnString("Select ProductionID FROM PrdnRoundTin WHERE pid='" + lblItemCode.Text + "'");

            SQLQuery.ExecNonQry("Delete PrdnRoundTin where  ProductionID ='" + isExist + "'   ");
            SQLQuery.ExecNonQry("Delete PrdnRoundTinDetails where   PrdnID='" + isExist + "'   ");
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
