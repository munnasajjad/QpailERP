using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services.Description;
using RunQuery;

public partial class app_Cutting_Shearing : System.Web.UI.Page
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
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory2, "CategoryID", "CategoryName");

            ddPurpose.DataBind();

            GetProductList();
            Bind2ndGrid(lblPrId.Text);
            //GridView2.DataBind();
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedQty(ddPurpose.SelectedValue, "Raw Sheet", ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
            decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedWeight(ddPurpose.SelectedValue, "Raw Sheet", ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

            decimal ttlWeight = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(TotalWeight),0)+ ISNULL(SUM(WaistWeight),0) from PrdnShearingDetails where PrdnID='" + lblPrId.Text + "'"));
            ttlWeight = Convert.ToDecimal(txtNonUsable.Text) + ttlWeight;

            if (inputAvailable >= Convert.ToDecimal(txtInputQtyPCS.Text) && inputAvailableKg >= Convert.ToDecimal(txtInputQtyKG.Text))
            {
                if (Convert.ToDecimal(txtInputQtyKG.Text) == ttlWeight && Convert.ToDecimal(txtNonUsable.Text) >= 0)
                {
                    string msg = VerifyStock();
                    if (msg == "")
                    {
                        if (btnSave.Text == "Save")
                        {
                            SaveProduction();
                            UpdateProduction();
                            ClearMasterArea();
                            lblMsg.Attributes.Add("class", "xerp_success");
                            lblMsg.Text = "Production saved successfully";
                        }
                        else
                        {
                            UpdateProduction();
                            ClearMasterArea();
                            lblMsg.Attributes.Add("class", "xerp_success");
                            lblMsg.Text = "Entry updated successfully";
                        }
                    }
                    else
                    {
                        lblMsg.Attributes.Add("class", "xerp_stop");
                        lblMsg.Text = msg;
                    }
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Error: Input weight & output weight is not equal!";
                }
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Error: Input item is not available into stock!";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_warning");
            lblMsg.Text = "Error: " + ex.ToString();
        }
    }

    private string VerifyStock()
    {
        //DataTable citydt = RunQuery.SQLQuery.ReturnDataTable("Select  Id, Purpose, ProductID from ProductionDetails where PrdnID='" + lblPrId.Text + "'");

        //foreach (DataRow citydr in citydt.Rows)
        //{
        //    string Purpose = citydr["Purpose"].ToString();
        //    string SizeId = citydr["SizeId"].ToString();
        //    string BrandID = citydr["BrandID"].ToString();
        //    string ProductID = citydr["ProductID"].ToString();
        //    string ProductName = citydr["ProductName"].ToString();
        //    string Thickness = citydr["Thickness"].ToString();

        //    string PrdnPcs = citydr["PrdnPcs"].ToString();
        //    string PkPerSheet = citydr["PkPerSheet"].ToString();
        //    decimal TotalWeight = Convert.ToDecimal(citydr["TotalWeight"].ToString()) * 1000;
        //    decimal WaistWeight = Convert.ToDecimal(citydr["WaistWeight"].ToString()) * 1000;

        //    Accounting.VoucherEntry.StockEntry(lblPrId.Text, "Production", "Stock-in", "", "", ProductID, ProductName, "4", "", "2", PrdnPcs, "0", TotalWeight.ToString(), "0", "", "Shearing output production", "Raw", "Shearing", Page.User.Identity.Name.ToString());
        //    if (Convert.ToDecimal(WaistWeight) > 0)
        //    {
        //        Accounting.VoucherEntry.StockEntry(lblPrId.Text, "Production", "Stock-in", "", "", ProductID, ProductName, "4", "", "2", "0", "0", WaistWeight.ToString(), "0", "", "Shearing output wastage", "Waistage", "Shearing", Page.User.Identity.Name.ToString());
        //    }

        decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedQty(ddPurpose.SelectedValue, "Raw Sheet", ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
        decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedWeight(ddPurpose.SelectedValue, "Raw Sheet", ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));

        string msg = "";
        if (inputAvailable < Convert.ToDecimal(txtInputQtyPCS.Text) && inputAvailableKg < Convert.ToDecimal(txtInputQtyKG.Text))
        {
            msg = "ERROR: Shert Stock Qty.: " + inputAvailable + " PCS, " + inputAvailableKg + "KG";
        }
        return msg;
        //}
    }

    private void SaveProduction()
    {
        if (txtFinalProd.Text == "")
        {
            txtFinalProd.Text = "0";
        }
        string lName = Page.User.Identity.Name.ToString();
        string prdId = RunQuery.SQLQuery.ReturnString("Select 'PRD-SH-'+ CONVERT(varchar, (ISNULL (max(pid),0)+1001 )) from PrdnShearing");
        lblPrId.Text = prdId;

        SqlCommand cmd2 = new SqlCommand("INSERT INTO PrdnShearing (ProductionID, EntryBy) VALUES (@ProductionID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductionID", prdId);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

    }

    private void UpdateProduction()
    {
        RunQuery.SQLQuery.ExecNonQry(@"Update PrdnShearing SET Date='" +
                                     Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") +
                                     "', Purpose='" + ddPurpose.SelectedValue + "', SectionID='2', SectionName='Shearing', MachineNo='" + ddMachine.SelectedValue +
                                     "', LineNumber='" + ddLine.SelectedValue + "', ForCompany='" + ddCustomer.SelectedValue +
                                     "', Brand='" + ddBrand.SelectedValue + "', ItemID='" + ddItemNameRaw.SelectedValue + "', ItemName='" +
                                     ddItemNameRaw.SelectedItem.Text + "', Color='', ItemWeight='" + txtInputQtyKG.Text +
                                     "', InputQty='" + txtInputQtyPCS.Text + "', Process='', " +
                                     "CustomerID='', CustomerName='" + ddCustomer.SelectedItem.Text + "', Operation='', OperatorID='', NonUsableWeight='" + txtNonUsable.Text + "', WorkingHour='" +
                                     txtHour.Text + "', FinalProduction='" + txtFinalProd.Text +
                                     "', Rejection='0', TimeWaste='" + txtTimeWaist.Text + "', ReasonWaist='" +
                                     txtReason.Text + "', Shift='" + ddShift.SelectedValue + "', Remarks ='" +
                                     txtRemark.Text + "' WHERE ProductionID='" + lblPrId.Text + "' ");

        //Item Stock Entry
        RunQuery.SQLQuery.ExecNonQry("DELETE Stock WHERE InvoiceID='" + lblPrId.Text + "'");
        RunQuery.SQLQuery.ExecNonQry("UPDATE PrdnShearingDetails SET PrdnID='" + lblPrId.Text +
                                     "' WHERE PrdnID=' ' ");


        //Input Item stock-out
        Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Shearing", lblPrId.Text, "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ddItemNameRaw.SelectedValue, ddItemNameRaw.SelectedItem.Text, "Raw Sheet", ddGodown.SelectedValue, ddLocation.SelectedValue, "1", 0, Convert.ToInt32(txtInputQtyPCS.Text), 0, 0, Convert.ToDecimal(txtInputQtyKG.Text), "", "Stock-out", "Production", "Shearing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        //Output Item stock-in
        DataTable citydt = RunQuery.SQLQuery.ReturnDataTable("Select Purpose, ProductID, ProductName, PrdnPcs, TotalWeight, WasteItemID, WasteItemName, WasteQty, WaistWeight from PrdnShearingDetails where PrdnID='" + lblPrId.Text + "'");

        foreach (DataRow citydr in citydt.Rows)
        {
            string Purpose = citydr["Purpose"].ToString();
            string ProductID = citydr["ProductID"].ToString();
            string ProductName = citydr["ProductName"].ToString();
            string PrdnPcs = citydr["PrdnPcs"].ToString();
            string TotalWeight = citydr["TotalWeight"].ToString();

            string WasteItemID = citydr["WasteItemID"].ToString();
            string WasteItemName = citydr["WasteItemName"].ToString();
            string WasteQty = citydr["WasteQty"].ToString();
            string WaistWeight = citydr["WaistWeight"].ToString();

            Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- Shearing", lblPrId.Text, "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", ProductID, ProductName, "Processed Sheet", ddGodown.SelectedValue, ddLocation.SelectedValue, "1", Convert.ToInt32(PrdnPcs), 0, 0, Convert.ToDecimal(TotalWeight), 0, "", "Stock-in", "Production", "Shearing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            if (Convert.ToDecimal(WaistWeight) > 0)
            {
                Stock.Inventory.SaveToStock(Purpose, lblPrId.Text, "Production- Shearing", lblPrId.Text, "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", WasteItemID, WasteItemName, "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", Convert.ToInt32(WasteQty), 0, 0, Convert.ToDecimal(WaistWeight), 0, "", "Stock-in", "Production", "Shearing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            }

        }

        if (Convert.ToDecimal(txtNonUsable.Text) > 0)
        {
            Stock.Inventory.SaveToStock(ddPurpose.SelectedValue, lblPrId.Text, "Production- Shearing", lblPrId.Text, "", ddCustomer.SelectedValue, ddBrand.SelectedValue, "", "", "1809", "Nonusable Westage", "", ddGodown.SelectedValue, ddLocation.SelectedValue, "7", 0, 0, 0, Convert.ToDecimal(txtNonUsable.Text), 0, "", "Stock-in", "Production", "Shearing", Page.User.Identity.Name.ToString(), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        }
        StockDetails();
    }

    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
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
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory2, "CategoryID", "CategoryName");

        GetProductList();
    }
    protected void ddCategoryRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddCategory2.SelectedValue = ddCategoryRaw.SelectedValue;
        GetProductList();

    }
    protected void ddItemNameRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadFinishedDD();
    }

    protected void ddGodown_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocation.DataBind();
        StockDetails();
    }

    private void LoadFinishedDD()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategory2.SelectedValue + "'  AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddProduct, "ProductID", "ItemName");
        StockDetails();
    }

    private void GetProductList()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategoryRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemNameRaw, "ProductID", "ItemName");

        //gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategory2.SelectedValue + "' AND ProductID<>'" + ddItemNameRaw.SelectedValue + "'  AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        LoadFinishedDD();
        StockDetails();
    }

    private void StockDetails()
    {
        decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedQty(ddPurpose.SelectedValue, "Raw Sheet", ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
        decimal inputAvailableKg = Convert.ToDecimal(Stock.Inventory.AvailableNonPrintedWeight(ddPurpose.SelectedValue, "Raw Sheet", ddItemNameRaw.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue));
        if (inputAvailable > 0)
        {
            weightRatio.Text = Convert.ToString(inputAvailableKg / inputAvailable);
            if (txtInputQtyPCS.Text != "")
            {
                txtInputQtyKG.Text = Convert.ToString(Convert.ToDecimal(txtInputQtyPCS.Text) * Convert.ToDecimal(weightRatio.Text));
            }
        }

        ltrLastInfo.Text = "Stock Qty.: " + inputAvailable + " PCS, " + inputAvailableKg + "KG";
    }

    protected void ddCategory2_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategory2.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddProduct, "ProductID", "ItemName");
        StockDetails();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            RunQuery.SQLQuery.Empty2Zero(txtRejected);

            if (txtThickness.Text != "" && txtproduced.Text != "" && txtPackPerSheet.Text != "" && txtSheetWeight.Text != "" && txtRejected.Text != "")
            {
                if (btnAdd.Text == "Add")
                {
                    SqlCommand cmde = new SqlCommand("SELECT ProductID FROM PrdnShearingDetails WHERE ProductID ='" + ddProduct.SelectedValue + "' AND  PrdnID =''", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmde.Connection.Open();
                    string isExist
                        = Convert.ToString(cmde.ExecuteScalar());
                    cmde.Connection.Close();

                    if (isExist == "")
                    {
                        InsertData();
                        //Accounting.VoucherEntry.ProductionItemEntry("", "Shearing", ddPurpose.SelectedValue, ddSize.SelectedValue, "", ddProduct.SelectedValue, ddProduct.SelectedItem.Text, txtThickness.Text, txtproduced.Text, txtPackPerSheet.Text, txtSheetWeight.Text, txtRejected.Text, Page.User.Identity.Name.ToString());
                        ClearDetailArea();
                        lblMsg2.Attributes.Add("class", "xerp_success");
                        lblMsg2.Text = "New product added successfully";
                    }
                    else
                    {
                        lblMsg2.Attributes.Add("class", "xerp_error");
                        lblMsg2.Text = "Error: Item already exist!";
                    }
                }
                else
                {
                    //RunQuery.SQLQuery.ExecNonQry("Delete PrdnShearingDetails where id='" + lblEid.Text + "'");
                    //InsertData();
                    UpdateData();
                    //Accounting.VoucherEntry.ProductionItemEntry("", "Shearing", ddPurpose.SelectedValue, ddSize.SelectedValue, "", ddProduct.SelectedValue, ddProduct.SelectedItem.Text, txtThickness.Text, txtproduced.Text, txtPackPerSheet.Text, txtSheetWeight.Text, txtRejected.Text, Page.User.Identity.Name.ToString());
                    btnAdd.Text = "Add";
                    ClearDetailArea();
                    Bind2ndGrid(lblPrId.Text);

                    lblMsg2.Attributes.Add("class", "xerp_success");
                    lblMsg2.Text = "Entry updated successfully";
                }
            }
            else
            {
                lblMsg2.Attributes.Add("class", "xerp_error");
                lblMsg2.Text = "Please fillup all fields to add new data...";
            }


            //lblMsg2.Attributes.Add("class", "xerp_error");
            //lblMsg2.Text = "Error: Input weight & output weight must have to be equel!";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "Error: " + ex.ToString();
        }
        finally
        {
            GridView2.DataBind();
        }

    }

    private void InsertData()
    {
        RunQuery.SQLQuery.ExecNonQry(
                           @"INSERT INTO PrdnShearingDetails (PrdnID, Purpose, SizeId, Category, ProductID, ProductName, Thickness, PrdnPcs, PkPerSheet, TotalWeight, WasteItemID, WasteItemName, WasteQty, WaistWeight, EntryBy)" +
                           " VALUES ('" + lblPrId.Text + "', '" + ddPurpose.SelectedValue + "',  '" + ddSize.SelectedValue + "', '" + ddCategory2.SelectedValue + "', '" +
                           ddProduct.SelectedValue + "', '" + ddProduct.SelectedItem.Text + "', '" + txtThickness.Text + "', '" + txtproduced.Text + "', '" + txtPackPerSheet.Text +
                           "', '" + txtSheetWeight.Text + "', '" + ddWasteStock.SelectedValue + "', '" + ddWasteStock.SelectedItem.Text + "', '" + txtWasteQty.Text + "', '" +
                           txtRejected.Text + "', '" + Page.User.Identity.Name.ToString() + "')");

    }

    private void UpdateData()
    {
        RunQuery.SQLQuery.ExecNonQry(
                           @"UPDATE PrdnShearingDetails SET Purpose='" + ddPurpose.SelectedValue + "', SizeId='" + ddSize.SelectedValue + "', Category='" + ddCategory2.SelectedValue + "', ProductID='" +
                           ddProduct.SelectedValue + "', ProductName='" + ddProduct.SelectedItem.Text + "', Thickness='" + txtThickness.Text + "', PrdnPcs='" + txtproduced.Text + "', PkPerSheet='" + txtPackPerSheet.Text +
                           "', TotalWeight='" + txtSheetWeight.Text + "', WasteItemID='" + ddWasteStock.SelectedValue + "', WasteItemName='" + ddWasteStock.SelectedItem.Text + "', WasteQty='" + txtWasteQty.Text + "', WaistWeight='" +
                           txtRejected.Text + "'  WHERE id='" + lblEid.Text + "'");

    }

    private void Bind2ndGrid(string productionId)
    {
        string query = @"SELECT Id, (SELECT Purpose FROM Purpose WHERE pid=a.Purpose) AS Purpose, 
                                        (SELECT [BrandName] FROM [Brands] WHERE [BrandID] = a.SizeId) AS  SizeId, 
                                     (SELECT [CategoryName] FROM [Categories] WHERE [CategoryID] = a.Category) AS  Category, ProductID, ProductName, Thickness, 
                                    PrdnPcs, PkPerSheet, TotalWeight, WasteItemID, WasteItemName, WasteQty, 
                                        WaistWeight, EntryBy FROM [PrdnShearingDetails] a WHERE PrdnID= '" + productionId + "' ORDER BY [id] ASC";


        DataTable dtTable = SQLQuery.ReturnDataTable(query);
        GridView2.DataSource = dtTable;
        GridView2.DataBind();
    }

    private void Bind2ndGrid()
    {
        string query = @"SELECT Id, (SELECT Purpose FROM Purpose WHERE pid=a.Purpose) AS Purpose, 
                                        (SELECT [BrandName] FROM [Brands] WHERE [BrandID] = a.SizeId) AS  SizeId, 
                                     (SELECT [CategoryName] FROM [Categories] WHERE [CategoryID] = a.Category) AS  Category, ProductID, ProductName, Thickness, 
                                    PrdnPcs, PkPerSheet, TotalWeight, WasteItemID, WasteItemName, WasteQty, 
                                        WaistWeight, EntryBy FROM [PrdnShearingDetails] a WHERE PrdnID= '' ORDER BY [id] ASC";


        DataTable dtTable = SQLQuery.ReturnDataTable(query);
        GridView2.DataSource = dtTable;
        GridView2.DataBind();
    }

    private void ClearDetailArea()
    {
        txtThickness.Text = "";
        txtproduced.Text = "";
        txtPackPerSheet.Text = "";
        txtSheetWeight.Text = "";
        txtRejected.Text = "";
        txtWasteQty.Text = "";
        btnAdd.Text = "Add";
        Bind2ndGrid();
    }
    private void ClearMasterArea()
    {
        txtInputQtyPCS.Text = "";
        txtInputQtyKG.Text = "";
        ClearDetailArea();
        ddCustomer.SelectedValue = "";
        txtInputQtyPCS.Text = "";
        txtInputQtyKG.Text = "";
        txtHour.Text = "";
        txtTimeWaist.Text = "";
        txtReason.Text = "";
        txtFinalProd.Text = "";
        txtRemark.Text = "";
        txtNonUsable.Text = "";
        lblTotalWeight.Text = "0";

        lblPrId.Text = " ";
        lblEditId.Text = " ";
        btnAdd.Text = "Add";
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
        SqlCommand cmd = new SqlCommand("SELECT Purpose, SizeId, ProductID, Thickness, PrdnPcs, PkPerSheet, TotalWeight, WasteItemID, WasteItemName, WasteQty, WaistWeight FROM [PrdnShearingDetails] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            ddPurpose.SelectedValue = dr[0].ToString();
            ddSize.SelectedValue = dr[1].ToString();
            string pid = dr[2].ToString();

            string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID=(SELECT GradeID FROM [Categories] where CategoryID=(Select CategoryID from Products where ProductID='" + pid + "') ) ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory2, "CategoryID", "CategoryName");

            ddCategory2.SelectedValue = RunQuery.SQLQuery.ReturnString("Select CategoryID from Products where ProductID='" + pid + "'");
            LoadFinishedDD();
            ddProduct.SelectedValue = pid;
            txtThickness.Text = dr[3].ToString();
            txtproduced.Text = dr[4].ToString();
            txtPackPerSheet.Text = dr[5].ToString();
            txtSheetWeight.Text = dr[6].ToString();

            ddWasteStock.SelectedValue = dr[7].ToString();
            //txtSheetWeight.Text = dr[8].ToString();
            txtWasteQty.Text = dr[9].ToString();
            txtRejected.Text = dr[10].ToString();
        }
        cmd.Connection.Close();

    }
    protected void GridView2_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView2.Rows[index].FindControl("Label1") as Label;

            SqlCommand cmd7 = new SqlCommand("DELETE PrdnShearingDetails WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
        string ttlWeight = RunQuery.SQLQuery.ReturnString("Select SUM(TotalWeight)+SUM(WaistWeight) from PrdnShearingDetails where PrdnID='" + lblPrId.Text + "'");

        if (Convert.ToDecimal(ttlWeight) > 0)
        {
            lblTotalWeight.Text = "<b>Total Output Weight: </b>" + ttlWeight + " kg.";
        }

        txtFinalProd.Text = RunQuery.SQLQuery.ReturnString("Select SUM(PrdnPcs) from PrdnShearingDetails where PrdnID='" + lblPrId.Text + "' ");

        if (txtInputQtyKG.Text != "")
        {
            txtNonUsable.Text = Convert.ToString(Convert.ToDecimal(txtInputQtyKG.Text) - Convert.ToDecimal(ttlWeight));
        }
    }

    protected void ddCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand.Items.Clear();
        ListItem lst = new ListItem("--- all ---", "");
        ddBrand.Items.Insert(ddBrand.Items.Count, lst);
        ddBrand.DataBind();
    }

    protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
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
            EditMode();
            Bind2ndGrid(lblPrId.Text);
            btnSave.Text = "Update";
            lblMsg.Attributes.Add("class", "xerp_info");
            lblMsg.Text = "Edit mode activated ...";
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }


    private void EditMode()
    {
        string itemToEdit = lblEditId.Text;

        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1) pid, ProductionID, Date, Purpose, SectionID, SectionName, MachineNo, LineNumber, 
            ForCompany, Brand, ItemID, ItemName, Color, ItemWeight, InputQty, Process, CustomerID, CustomerName, Operation, 
            OperatorID, NonUsableWeight, WorkingHour, FinalProduction, Rejection, TimeWaste, ReasonWaist, Shift, Remarks, EntryBy, EntryDate
            FROM PrdnShearing WHERE (pid = '" + itemToEdit + "')");
        foreach (DataRow drx in dtx.Rows)
        {
            lblPrId.Text = drx["ProductionID"].ToString();
            txtDate.Text = Convert.ToDateTime(drx["Date"]).ToString("dd/MM/yyyy");
            ddPurpose.SelectedValue = drx["Purpose"].ToString();
            //ddsectio.Text = drx["SectionID"].ToString();

            ddMachine.SelectedValue = drx["MachineNo"].ToString();
            ddLine.SelectedValue = drx["LineNumber"].ToString();
            ddCustomer.SelectedValue = drx["ForCompany"].ToString();
            ddBrand.DataBind();
            ddBrand.SelectedValue = drx["Brand"].ToString();

            string itemId = drx["ItemID"].ToString();
            ddGradeRaw.SelectedValue = Stock.Inventory.GetItemGrade(itemId);
            string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory2, "CategoryID", "CategoryName");

            GetProductList();
            //ddItemNameRaw.SelectedValue = itemId;

            //txtQty.Text = drx["Color"].ToString();
            txtInputQtyKG.Text = drx["ItemWeight"].ToString();
            txtInputQtyPCS.Text = drx["InputQty"].ToString();
            //txtQty.Text = drx["Process"].ToString();
            //txtQty.Text = drx["CustomerID"].ToString();
            //txtQty.Text = drx["CustomerName"].ToString();

            //txtQty.Text = drx["Operation"].ToString();
            //txtQty.Text = drx["OperatorID"].ToString();
            txtNonUsable.Text = drx["NonUsableWeight"].ToString();
            txtHour.Text = drx["WorkingHour"].ToString();

            txtFinalProd.Text = drx["FinalProduction"].ToString();
            //txtQty.Text = drx["Rejection"].ToString();
            txtTimeWaist.Text = drx["TimeWaste"].ToString();
            txtReason.Text = drx["ReasonWaist"].ToString();

            ddShift.Text = drx["Shift"].ToString();
            txtRemark.Text = drx["Remarks"].ToString();
        }
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            string isExist = RunQuery.SQLQuery.ReturnString("Select ProductionID FROM PrdnShearing WHERE pid='" + lblItemCode.Text + "'");

            SQLQuery.ExecNonQry("Delete PrdnShearing where  ProductionID ='" + isExist + "'   ");
            SQLQuery.ExecNonQry("Delete PrdnShearingDetails where   PrdnID='" + isExist + "'   ");
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
