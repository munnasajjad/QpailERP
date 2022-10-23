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
public partial class app_Finished_Items2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            //txtDate.Text = DateTime.Now.ToShortDateString();

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();


            LoadSize();
            LoadCustomer();

            LoadBrand();
            LoadSubGroup();
            LoadGrade();
            LoadCategory();
            LoadProduct();
            ItemExist();

            //ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");
            //txtInv.Focus();
            //ddGodown.DataBind();
            //ddLocation.DataBind();

            //BindItemGrid();
            //LoadSummesion("");

            LoadContainer();
            LoadLid();
            LoadHandle();
            LoadAcHeadSticker();

            /*LoadRMCHead();
            LoadInventoriesHead();
            LoadPurposeRaw();


            LoadGroup();
            LoadSubGroupRaw();
            LoadGradeRaw();
            LoadCategoryRaw();
            LoadProductRaw();

            LoadCustomerRaw();
            LoadBrandRaw();
            LoadSizeRaw();
            LoadColorRaw();*/
            LoadSpecList("filter");

            //LoadSummesion("");
            LoadFormControls();
            QtyinStockRaw();
            BindItemGrid();
            BindGridView1();
        }
        //txtInv.Text = InvIDNo();

    }



    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    private void LoadSize()
    {
        SQLQuery.PopulateDropDown("SELECT [BrandID], [BrandName] FROM [Brands] order by DisplaySl", ddSize, "BrandID", "BrandName");
    }
    private void LoadCustomer()
    {
        SQLQuery.PopulateDropDown("SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'customer') ORDER BY [Company]", ddCustomer, "PartyID", "Company");
    }

    private void LoadBrand()
    {
        SQLQuery.PopulateDropDown("SELECT [BrandID], [BrandName] FROM [CustomerBrands] WITH (NOLOCK) WHERE (([CustomerID] = '" + ddCustomer.SelectedValue + "') ) ORDER BY BrandName", ddBrand, "BrandID", "BrandName");
    }

    private void LoadSubGroup()
    {
        SQLQuery.PopulateDropDown("SELECT CategoryID, CategoryName FROM [ItemSubGroup] where (GroupID = 2) ", ddSubGroup, "CategoryID", "CategoryName");
    }

    private void LoadGrade()
    {
        SQLQuery.PopulateDropDown("SELECT GradeID, GradeName from ItemGrade where CategoryID = '" + ddSubGroup.SelectedValue + "' ORDER BY [GradeName]", ddGrade, "GradeID", "GradeName");
    }

    private void LoadCategory()
    {
        SQLQuery.PopulateDropDown("SELECT CategoryID, CategoryName FROM [Categories] where GradeID = '" + ddGrade.SelectedValue + "' ORDER BY [CategoryName]", ddCategory, "CategoryID", "CategoryName");
    }

    private void LoadProduct()
    {
        SQLQuery.PopulateDropDown("SELECT ProductID, [ItemName] FROM [Products] WHERE ([CategoryID] = '" + ddCategory.SelectedValue + "') ORDER BY [ItemName]", ddProduct, "ProductID", "ItemName");
    }


    /*********************************************/

    /********************************************/
    /* private void LoadGroup()
     {
         SQLQuery.PopulateDropDown("SELECT GroupSrNo, [GroupName] FROM [ItemGroup] WHERE GroupSrNo<>2 ORDER BY [GroupSrNo]", ddGroup, "GroupSrNo", "GroupName");
     }
     private void LoadSubGroupRaw()
     {
         SQLQuery.PopulateDropDown("SELECT CategoryID, CategoryName FROM [ItemSubGroup] WHERE (GroupID = '" + ddGroup.SelectedValue + "')", ddSubGroupRaw, "CategoryID", "CategoryName");
     }
     private void LoadGradeRaw()
     {
         SQLQuery.PopulateDropDown("SELECT GradeID, GradeName from ItemGrade WHERE CategoryID = '" + ddSubGroupRaw.SelectedValue + "' ORDER BY [GradeName]", ddGradeRaw, "GradeID", "GradeName");
     }
     private void LoadCategoryRaw()
     {
         SQLQuery.PopulateDropDown("SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID = '" + ddGradeRaw.SelectedValue + "' ORDER BY [CategoryName]", ddCategoryRaw, "CategoryID", "CategoryName");
     }
     private void LoadProductRaw()
     {
         SQLQuery.PopulateDropDown("SELECT ProductID, [ItemName] FROM [Products] WHERE [CategoryID] = '" + ddCategoryRaw.SelectedValue + "' ORDER BY [ItemName]", ddProductRaw, "ProductID", "ItemName");
     }


     private void LoadCustomerRaw()
     {
         SQLQuery.PopulateDropDown("SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'Customer') ORDER BY [Company]", ddCustomerRaw, "PartyID", "Company");
     }
     private void LoadBrandRaw()
     {
         SQLQuery.PopulateDropDown("SELECT [BrandID], [BrandName] FROM [CustomerBrands] WHERE ([CustomerID] = '" + ddCustomerRaw.SelectedValue + "') Order by BrandName", ddBrandRaw, "BrandID", "BrandName");
     }
     private void LoadSizeRaw()
     {
         SQLQuery.PopulateDropDown("SELECT [BrandID], [BrandName] FROM [Brands] ORDER BY DisplaySl", ddSizeRaw, "BrandID", "BrandName");
     }
     private void LoadColorRaw()
     {
         SQLQuery.PopulateDropDown("SELECT [DepartmentName], [Departmentid] FROM [Colors] ORDER BY [DepartmentName]", ddColorRaw, "DepartmentId", "DepartmentName");
     }*/

    private void LoadContainer()
    {
        SQLQuery.PopulateDropDown("Select AccountsHeadID, (Select ControlAccountsName from ControlAccount where ControlAccountsID=[HeadSetup].ControlAccountsID)+' > '+AccountsHeadName as AccountsHeadName from [HeadSetup]  Order by AccountsHeadID", ddContainer, "AccountsHeadID", "AccountsHeadName");
        ddContainer.Items.Insert(0, new ListItem("---Select---", "0"));
    }
    private void LoadLid()
    {
        SQLQuery.PopulateDropDown("Select AccountsHeadID, (Select ControlAccountsName from ControlAccount where ControlAccountsID=[HeadSetup].ControlAccountsID)+' > '+AccountsHeadName as AccountsHeadName from [HeadSetup]  Order by AccountsHeadID", ddLid, "AccountsHeadID", "AccountsHeadName");
        ddLid.Items.Insert(0, new ListItem("---Select---", "0"));
    }
    private void LoadHandle()
    {
        SQLQuery.PopulateDropDown("Select AccountsHeadID, (Select ControlAccountsName from ControlAccount where ControlAccountsID=[HeadSetup].ControlAccountsID)+' > '+AccountsHeadName as AccountsHeadName from [HeadSetup]  Order by AccountsHeadID", ddHandle, "AccountsHeadID", "AccountsHeadName");
        ddHandle.Items.Insert(0, new ListItem("---Select---", "0"));
    }
    private void LoadAcHeadSticker()
    {
        SQLQuery.PopulateDropDown("Select AccountsHeadID, (Select ControlAccountsName from ControlAccount where ControlAccountsID=[HeadSetup].ControlAccountsID)+' > '+AccountsHeadName as AccountsHeadName from [HeadSetup]  Order by AccountsHeadID", ddAcHeadSticker, "AccountsHeadID", "AccountsHeadName");
        ddAcHeadSticker.Items.Insert(0, new ListItem("---Select---", "0"));
    }
    /*private void LoadRMCHead()
    {
        SQLQuery.PopulateDropDown("SELECT AccountsHeadID, AccountsHeadName FROM [HeadSetup]  WHERE IsActive='1' AND ControlAccountsID  ='040201' ORDER BY [AccountsHeadID] DESC", ddRMCHead, "AccountsHeadID", "AccountsHeadName");
    }
    private void LoadInventoriesHead()
    {
        SQLQuery.PopulateDropDown("SELECT AccountsHeadID, AccountsHeadName FROM [HeadSetup]  WHERE IsActive='1' AND ControlAccountsID  ='010106' ORDER BY [AccountsHeadID] DESC", ddInventoriesHead, "AccountsHeadID", "AccountsHeadName");
    }
    private void LoadPurposeRaw()
    {
        SQLQuery.PopulateDropDown("SELECT [pid], [Purpose] FROM [Purpose] order by Purpose", ddPurposeRaw, "pid", "Purpose");
    }*/



    public string ItemExist()
    {
        string itemFound = "";
        try
        {
            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT pid, ProductCode, ProductName, AccHeadIdContainer, AccHeadIdLid, AccHeadIdHandle, AccHeadIdSticker, UnitPrice, UnitWeight FROM FinishedProducts WITH (NOLOCK) WHERE SizeID ='" + ddSize.SelectedValue + "' AND ProductID ='" + ddProduct.SelectedValue + "' AND BrandID ='" + ddBrand.SelectedValue + "'");
            foreach (DataRow drx in dtx.Rows)
            {
                txtPrdCode.Text = drx["ProductCode"].ToString();
                txtPrdName.Text = drx["ProductName"].ToString();

                ddContainer.SelectedValue = drx["AccHeadIdContainer"].ToString();
                ddLid.SelectedValue = drx["AccHeadIdLid"].ToString();
                ddHandle.SelectedValue = drx["AccHeadIdHandle"].ToString();
                ddAcHeadSticker.SelectedValue = drx["AccHeadIdSticker"].ToString();

                txtUnitPrice.Text = drx["UnitPrice"].ToString();
                txtWeight.Text = drx["UnitWeight"].ToString();
                itemFound = drx["pid"].ToString();
            }
            if (dtx.Rows.Count == 0)
            {
                txtPrdCode.Text = "";
                txtPrdName.Text = ddSize.SelectedItem.Text + " " + ddBrand.SelectedItem.Text + " " + ddProduct.SelectedItem.Text;
                txtUnitPrice.Text = "";
                txtWeight.Text = "";
                itemFound = "";
            }
            BindItemGrid();
        }
        catch (Exception e)
        {

        }
        lblOrderID.Text = itemFound;
        return itemFound;
    }

    public string InvIDNo()
    {
        string lName = Page.User.Identity.Name.ToString();
        string yr = DateTime.Now.Year.ToString();
        DateTime countForYear = Convert.ToDateTime("01/01/" + yr + " 00:00:00");

        SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (max(Stockinl),0)+ 1 )) from Stockin where EntryDate>=@EntryDate", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryDate", countForYear);
        cmd.Connection.Open();
        string InvNo = Convert.ToString(cmd.ExecuteScalar());
        if (InvNo.Length < 2)
        {
            InvNo = "000" + InvNo;
        }
        else if (InvNo.Length < 3)
        {
            InvNo = "00" + InvNo;
        }
        else if (InvNo.Length < 4)
        {
            InvNo = "0" + InvNo;
        }
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        InvNo = InvNo + "/" + yr.Substring(2, 2);
        return InvNo;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (txtUnitPrice.Text == "")
            {
                txtUnitPrice.Text = "1";
            }
            //SizeId, ProductID, BrandID
            string isExist = SQLQuery.ReturnString(@"SELECT pid FROM FinishedProducts WHERE SizeID ='" + ddSize.SelectedValue + "' AND ProductID ='" + ddProduct.SelectedValue + "' AND BrandID ='" + ddBrand.SelectedValue + "'"); //ItemExist();
            if (isExist == "")
            {
                InsertData();
                lblMsg2.Attributes.Add("class", "xerp_success");
                lblMsg2.Text = "Item saved successfully";
            }
            else
            {
                SQLQuery.ExecNonQry("UPDATE FinishedProducts SET CompanyID='" + ddCustomer.SelectedValue + "', BrandID='" + ddBrand.SelectedValue + "', SizeID='" + ddSize.SelectedValue + "', SubGroup='" + ddSubGroup.SelectedValue + "', Grade='" + ddGrade.SelectedValue + "', Category='" + ddCategory.SelectedValue + "', ProductID='" + ddProduct.SelectedValue + "', ProductCode='" + txtPrdCode.Text + "', ProductName='" + txtPrdName.Text + "', AccHeadIdContainer='" + ddContainer.SelectedValue + "', AccHeadIdLid='" + ddLid.SelectedValue + "', AccHeadIdHandle='" + ddHandle.SelectedValue + "', AccHeadIdSticker='" + ddAcHeadSticker.SelectedValue + "', UnitPrice='" + txtUnitPrice.Text + "', UnitWeight='" + txtWeight.Text + "'" +
                                     "  where (pid ='" + isExist + "')");
                lblMsg2.Attributes.Add("class", "xerp_success");
                lblMsg2.Text = "Item updated successfully";
            }

        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "ERROR: " + ex.ToString();
        }
        finally
        {
            BindItemGrid();
            BindGridView1();
        }
    }
    private void InsertData()
    {
        string lName = Page.User.Identity.Name.ToString();
        string productName = txtPrdName.Text;

        SqlCommand cmd2 = new SqlCommand("INSERT INTO FinishedProducts (CompanyID, BrandID, SizeID, SubGroup, Grade, Category, ProductID, ProductCode, ProductName, AccHeadIdContainer, AccHeadIdLid, AccHeadIdHandle, AccHeadIdSticker, UnitPrice, UnitWeight, EntryBy) " +
                                         "VALUES ('" + ddCustomer.SelectedValue + "', @BrandID, @SizeId, '" + ddSubGroup.SelectedValue + "', '" + ddGrade.SelectedValue + "', '" + ddCategory.SelectedValue + "',  @ProductID, '" + txtPrdCode.Text + "', @ProductName, @AccHeadIdContainer, @AccHeadIdLid, @AccHeadIdHandle, @AccHeadIdSticker, @UnitCost, @UnitWeight, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@SizeId", ddSize.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductID", ddProduct.SelectedValue);
        cmd2.Parameters.AddWithValue("@BrandID", ddBrand.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductName", productName);

        cmd2.Parameters.AddWithValue("@AccHeadIdContainer", ddContainer.SelectedValue);
        cmd2.Parameters.AddWithValue("@AccHeadIdLid", ddLid.SelectedValue);
        cmd2.Parameters.AddWithValue("@AccHeadIdHandle", ddHandle.SelectedValue);
        cmd2.Parameters.AddWithValue("@AccHeadIdSticker", ddAcHeadSticker.SelectedValue);

        cmd2.Parameters.AddWithValue("@UnitCost", txtUnitPrice.Text);
        cmd2.Parameters.AddWithValue("@UnitWeight", txtWeight.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        SQLQuery.ExecNonQry("Update BomList SET FinishedID='" + ItemExist() + "' WHERE FinishedID=''");
    }

    //private void ExecuteUpdate()
    //{
    //    SqlCommand cmd2 = new SqlCommand(, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

    //    cmd2.Connection.Open();
    //    cmd2.ExecuteNonQuery();
    //    cmd2.Connection.Close();
    //    cmd2.Connection.Dispose();

    //}

    private void BindItemGrid()
    {/*
        string lName = Page.User.Identity.Name.ToString();
        string isExist = SQLQuery.ReturnString(@"SELECT pid FROM FinishedProducts WHERE SizeID ='" + ddSize.SelectedValue + "' AND ProductID ='" + ddProduct.SelectedValue + "' AND BrandID ='" + ddBrand.SelectedValue + "'");
        SqlCommand cmd = new SqlCommand(@"SELECT  Id, InventoryHead, RMCHead, (SELECT  [Purpose] FROM [Purpose] WHERE [pid]= BomList.Purpose) AS Purpose, 
                                            ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, QtyBalance, DeliveredQty, UnitWeight, 
                                            (SELECT  [Company] FROM [Party] WHERE [PartyID]= BomList.Customer) AS Customer,
                                            (SELECT [BrandName] FROM [CustomerBrands] WHERE BrandID=BomList.BrandID) AS BrandID,
                                            (SELECT [BrandName] FROM [Brands] WHERE BrandID=BomList.SizeId) AS SizeId,
                                            (SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=BomList.Color) AS Color,
                                            (SELECT [Spec] FROM [Specifications] WHERE id=BomList.Spec) AS Spec
                                            FROM BomList WHERE  FinishedID='" + isExist + "'  ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //cmd.CommandText = "SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal FROM StockinDetailsRaw WHERE EntryBy=@EntryBy AND OrderID='' ORDER BY Id";

        //cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        cmd.Connection.Open();
        ItemGrid.EmptyDataText = "No items to view...";
        ItemGrid.DataSource = cmd.ExecuteReader();
        ItemGrid.DataBind();
        cmd.Connection.Close();*/

    }
    protected void lbRefresh_OnClick(object sender, EventArgs e)
    {
        if (ddCustomer.SelectedValue != null)
        {
            BindGridView1();
        }
        else
        {
            Notify("Please select customer name", "info", lblMsg);
        }

        Notify("Data has been refreshed", "info", lblMsg);
    }


    private void BindGridView1()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand(@"SELECT [pid], [ProductCode], [ProductName], [UnitPrice], [UnitWeight] FROM [FinishedProducts] WHERE ([CompanyID] = '" + ddCustomer.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        /*cmd.Connection.Open();
        GridView1.DataSource = cmd.ExecuteReader();
        GridView1.DataBind();
        cmd.Connection.Close();*/
        
        cmd.Connection.Open();

        SimpleGrid.DataSource = cmd.ExecuteReader();
        SimpleGrid.DataBind();
        cmd.Connection.Close();
    }



    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(QtyBalance),0) from StockinDetails where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'");
        //ltrWeight.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(TotalWeight),0) from StockinDetails where OrderID=''");

        //if (ltrQty.Text != "0")
        //{
        //    ddCustomer.Enabled = false;
        //}
        //else
        //{
        //    ddCustomer.Enabled = true;
        //}
    }
    protected void ItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            //int index = Convert.ToInt32(e.RowIndex);
            //Label lblItemCode = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;
            //TextBox txtQty = ItemGrid.Rows[index].FindControl("txtQty") as TextBox;

            //string OrderID = RunQuery.SQLQuery.ReturnString("Select OrderID from StockinDetails where Id='" + lblItemCode.Text + "'");
            //string deliveredQty = RunQuery.SQLQuery.ReturnString("Select SUM(DeliveredQty) from StockinDetails where OrderID='" + OrderID + "'");

            //if (deliveredQty == "0")
            //{
            //SqlCommand cmd7 = new SqlCommand("DELETE FinishedProducts WHERE pid=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            //cmd7.Connection.Open();
            //cmd7.ExecuteNonQuery();
            //cmd7.Connection.Close();

            //BindItemGrid();
            //lblMsg2.Attributes.Add("class", "xerp_warning");
            //lblMsg2.Text = "Item deleted from BOM list ...";
            //}
            //else
            //{
            //    lblMsg2.Attributes.Add("class", "xerp_warning");
            //    lblMsg2.Text = "Order is Locked! There is some pending delivery...";
            //}
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }

    }

    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadBrand();
        QtyinStock();
        BindGridView1(); 
    }

    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        //txtUnitPrice.Focus();
    }
    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCategory();
        LoadProduct();
        QtyinStock();
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProduct();
        QtyinStock();
        //LoadCategory();
    }

    private void QtyinStock()
    {
        ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");
        ////txtCurrentQty.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where SizeID='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "'  AND WarehouseID='" + ddGodown.SelectedValue + "' ");
        //ltrCQty.Text = txtUnitPrice.Text;
        //txtCurrentKg.Text = RunQuery.SQLQuery.ReturnString("Select  ISNULL(sum(InWeight)-Sum(OutWeight),0) FROM Stock WHERE  SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "'");
        ItemExist();
        //BindItemGrid();
    }
    private void TrackCustomer()
    {

    }
    //protected void imgUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    //{
    //    lblMsg.Attributes.Add("class", "xerp_error");
    //    lblMsg.Text = "File Uploaded";
    //}


    protected void ItemGrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    //DropDownList1.DataBind();
        //    int index = Convert.ToInt32(ItemGrid.SelectedIndex);
        //    Label lblItemName = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;
        //    //lblOrderID.Text = lblItemName.Text; // lblOrderID was used for finishedProducts PID
        //    //EditMode(lblItemName.Text);
        //    btnAdd.Text = "Update";

        //    lblMsg2.Attributes.Add("class", "xerp_info");
        //    lblMsg2.Text = "Edit mode activated ...";
        //}
        //catch (Exception ex)
        //{
        //    lblMsg2.Attributes.Add("class", "xerp_error");
        //    lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        //}
    }
    /*
    private void EditMode(string entryID)
    {
        SqlCommand cmd = new SqlCommand("SELECT SizeId, ProductID, BrandID, CompanyFor, Quantity, UnitCost, UnitWeight, DeliveredQty, QtyBalance FROM [StockinDetails] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            ddSize.SelectedValue = dr[0].ToString();
            string productID = dr[1].ToString();
            string catID = RunQuery.SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + productID + "'");
            string grdID = RunQuery.SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
            ddGrade.SelectedValue = grdID;
            LoadCategory();
            ddCategory.SelectedValue = catID;
            SQLQuery.PopulateDropDown("SELECT ProductID, [ItemName] FROM [Products] WHERE [CategoryID] = '"+ ddCategory.SelectedValue + "' ORDER BY [ItemName]", ddProduct, "ProductID", "ItemName");
            ddProduct.SelectedValue = productID;

            ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + productID + "'");
            //txtStock.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where SizeID='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + productID + "'");

            ddCustomer.SelectedValue = dr[3].ToString();
            LoadBrand();
            ddBrand.SelectedValue = dr[2].ToString();
            //txtRate.Text
            txtUnitPrice.Text = dr[4].ToString();
            //txtWeight.Text = dr[6].ToString();

            ltrCQty.Text = dr[8].ToString();

            //txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtUnitPrice.Text) * Convert.ToDecimal(txtRate.Text));
        }
        cmd.Connection.Close();

    }*/
    //protected void ddGodown_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ddLocation.DataBind();
    //    BindItemGrid();
    //}

    protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGrade();
        LoadCategory();
        LoadProduct();
        QtyinStock();
    }


    protected void ddType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStockRaw();
    }

    protected void ddBrand_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string brand = ddBrand.SelectedValue;
        ItemExist();
        //ddCustomer.SelectedValue=lblCompany.Text;
        //LoadBrand();
        //ddBrand.SelectedValue = brand;
    }

    protected void ddColor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStockRaw();
    }

    protected void ddSize_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ItemExist();
        QtyinStock();
    }

    protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStockRaw();
    }

    //protected void ddCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //   /* ddBrand.Items.Clear();
    //    ListItem lst = new ListItem("--- all ---", "");
    //    ddBrand.Items.Insert(ddBrand.Items.Count, lst);
    //    LoadBrand();

    //    QtyinStock();*/
    //}

    protected void ddGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void lbSpec_OnClick(object sender, EventArgs e)
    {
        //if (lbSpec.Text == "New")
        //{
        //    ddSpecRaw.Visible = false;
        //    txtSpec.Visible = true;
        //    lbSpec.Text = "Cancel";
        //    txtSpec.Focus();
        //}
        //else
        //{
        //    ddSpecRaw.Visible = true;
        //    txtSpec.Visible = false;
        //    lbSpec.Text = "New";
        //    LoadSpecList("filter");
        //    ddSpecRaw.Focus();
        //}
        //lbFilter.Text = "Show-all";
    }

    private void LoadSpecList(string filterDD)
    {
        //string gQuery = "SELECT [id], [spec] FROM [Specifications] ORDER BY [spec]";
        //lbFilter.Text = "Filter";
        //if (filterDD != "")
        //{
        //    lbFilter.Text = "Show-all";
        //    gQuery = "SELECT [id], [spec] FROM [Specifications] where  CAST(id AS nvarchar) in (Select distinct spec from stock where ProductID='" + ddProduct.SelectedValue + "') ORDER BY [spec]";
        //}

        //SQLQuery.PopulateDropDown(gQuery, ddSpecRaw, "id", "spec");
        //QtyinStockRaw();
    }

    protected void lbFilter_OnClick(object sender, EventArgs e)
    {
        //if (lbFilter.Text == "Show-all")
        //{
        //    LoadSpecList("");
        //    //lbFilter.Text = "Filter";
        //}
        //else
        //{
        //    LoadSpecList("filter");
        //    //lbFilter.Text = "Show-all";
        //}

        //lbSpec.Text = "New";
        //ddSpecRaw.Visible = true;
        //txtSpec.Visible = false;
    }
    protected void ddSpec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStockRaw();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        /*try
        {
            if (btnAdd.Text == "Add")
            {
                string lName = Page.User.Identity.Name.ToString();

                //string isExist = ItemExist();

                if (ddProductRaw.SelectedValue != "")
                {
                    if (ddSubGroup.SelectedValue == "10")
                    {
                        string spec = ddSpecRaw.SelectedValue;
                        if (txtSpec.Text != "" && lbSpec.Text == "Cancel" && ddSubGroup.SelectedValue == "10")//Insert Ink spec
                        {
                            string isExistSpec = SQLQuery.ReturnString("SELECT id FROM Specifications WHERE  spec ='" + txtSpec.Text + "'");
                            if (isExistSpec == "")
                            {
                                SQLQuery.ExecNonQry("INSERT INTO Specifications (spec, EntryBy) VALUES ('" + txtSpec.Text + "', '" + lName + "') ");
                                spec = SQLQuery.ReturnString("SELECT MAX(id) FROM Specifications");
                                LoadSpecList(""); //ddSpec.DataBind();
                                ddSpecRaw.SelectedValue = spec;
                            }
                            else
                            {
                                LoadSpecList("");
                                ddSpecRaw.SelectedValue = isExistSpec;
                            }
                        }
                    }
                    //else
                    //{
                    addData();
                    //}

                    //txtUnitPrice.Text = "";
                    //txtWeight.Text = "";
                    //else
                    //{
                    //   lblMsg2.Attributes.Add("class", "xerp_error");
                    //   lblMsg2.Text = "ERROR: You must edit quantity for adjustment!";
                    //}
                }
                else
                {
                    Notify("ERROR: Product Already exist or product selection box is empty!", "warn", lblMsg);
                }
            }
            else
            {
                //ExecuteUpdate();
                SQLQuery.ExecNonQry("Delete BomList where (id ='" + lblOrderID.Text + "')");
                addData();
                btnAdd.Text = "Add";
                Notify("Item updated successfully", "success", lblMsg2);
            }
            //ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);

        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
        finally
        {
            BindItemGrid();
        }*/
    }
    private void addData()
    {
        /*string lName = Page.User.Identity.Name.ToString();
        string productName = ddProductRaw.SelectedItem.Text;
        SQLQuery.Empty2Zero(txtQtyRaw);
        SQLQuery.Empty2Zero(txtWeightRaw);
        string spec = "";
        if (ddSubGroupRaw.SelectedValue == "10")
        {
            spec = ddSpecRaw.SelectedValue;
        }

        int balance = 0;// Convert.ToInt32(txtUnitPrice.Text) - Convert.ToInt32(ltrCQty.Text);
        SqlCommand cmd2 = new SqlCommand("INSERT INTO BomList (InventoryHead, RMCHead, StockType, purpose, ItemType, FinishedID, ProductID, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy, CompanyFor,  QtyBalance, DeliveredQty, Spec) " +
                                         "VALUES ('" + ddInventoriesHead.SelectedValue + "', '" + ddRMCHead.SelectedValue + "', 'Raw', '" + ddPurposeRaw.SelectedValue + "', '" + ddTypeRaw.SelectedValue + "', @FinishedID, @ProductID, @ProductName, @UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, @CompanyFor, '" + balance + "', '" + ltrCQty.Text + "', '" + spec + "'  )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        if (ddTypeRaw.SelectedValue == "Printed Sheet")
        {
            cmd2 = new SqlCommand(@"INSERT INTO BomList (InventoryHead, RMCHead, StockType, purpose, ItemType, FinishedID, SizeId, ProductID, Customer, BrandID, ProductName, Color, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy, CompanyFor,  QtyBalance, DeliveredQty)" +
                                        " VALUES ('" + ddInventoriesHead.SelectedValue + "', '" + ddRMCHead.SelectedValue + "', 'Raw', '" + ddPurposeRaw.SelectedValue + "', '" + ddTypeRaw.SelectedValue + "', @FinishedID, @SizeId, @ProductID, '" + ddCustomerRaw.SelectedValue + "', @BrandID, @ProductName, '" + ddColorRaw.SelectedValue + "', @UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, @CompanyFor, '" + balance + "', '" + ltrCQty.Text + "'  )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }
        cmd2.Parameters.AddWithValue("@FinishedID", SQLQuery.ReturnString(@"SELECT pid FROM FinishedProducts WHERE SizeID ='" + ddSize.SelectedValue + "' AND ProductID ='" + ddProduct.SelectedValue + "' AND BrandID ='" + ddBrand.SelectedValue + "'")); //ItemExist());
        cmd2.Parameters.AddWithValue("@SizeId", ddSizeRaw.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductID", ddProductRaw.SelectedValue);
        cmd2.Parameters.AddWithValue("@BrandID", ddBrandRaw.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductName", productName);

        cmd2.Parameters.AddWithValue("@UnitCost", 0);
        cmd2.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(txtQtyRaw.Text));
        cmd2.Parameters.AddWithValue("@UnitWeight", txtWeightRaw.Text);

        cmd2.Parameters.AddWithValue("@ItemTotal", "0");
        cmd2.Parameters.AddWithValue("@TotalWeight", "0");
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@CompanyFor", ddCustomerRaw.SelectedItem.Text);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();*/
    }

    protected void ddSubGrpRaw_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //LoadGradeRaw();
        //LoadCategoryRaw();
        //LoadProductRaw();
        LoadFormControls();
        LoadSpecList("filter");
        QtyinStockRaw();
    }

    private void QtyinStockRaw()
    {/*
        try
        {
            ltrUnit.Text = SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProductRaw.SelectedValue + "'");

            ltrCQty.Text = "0";

            if (ddSubGroupRaw.SelectedValue == "10") //printing ink
            {
                //txtCurrentQty.Text = "0";
                //txtCurrentKg.Text = Stock.Inventory.AvailableInkWeight(ddProductRaw.SelectedValue, ddSpecRaw.SelectedValue, ddGodownRaw.SelectedValue);
                //txtPrice.Visible = true;
                //txtPrice.Text = Stock.Inventory.LastInkPrice(ddProductRaw.SelectedValue, ddSpecRaw.SelectedValue);
                txtUnitPrice.Text = "0";
                txtWeight.Focus();
            }
            else if (ddSubGroupRaw.SelectedValue == "33" || ddSubGroupRaw.SelectedValue == "35") //HTF & IML
            {
                //txtCurrentQty.Text = "0";
                //decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.QtyinStock(ddPurposeRaw.SelectedValue, ddProductRaw.SelectedValue, ddGodownRaw.SelectedValue));

                //txtCurrentKg.Text = "0";// inputAvailable.ToString();
                //txtPrice.Visible = true;
                //txtPrice.Text = Stock.Inventory.LastPlasticRawPrice(ddPurposeRaw.SelectedValue, ddProductRaw.SelectedValue);
                //txtCurrentQty.Text = inputAvailable.ToString();
                txtWeight.Focus();
            }
            else if (ddSubGroupRaw.SelectedValue != "9") //All items except Tin Plates
            {
                //txtCurrentQty.Text = "0";
                //txtCurrentKg.Text = Stock.Inventory.PlasticRawWeight(ddPurposeRaw.SelectedValue, ddProductRaw.SelectedValue, ddGodownRaw.SelectedValue);
                //txtPrice.Visible = true;
                //txtPrice.Text = Stock.Inventory.LastPlasticRawPrice(ddProductRaw.SelectedValue, ddSpecRaw.SelectedValue);
                txtUnitPrice.Text = "0";
                txtWeight.Focus();
            }
            else
            {
                if (ddTypeRaw.SelectedValue == "Printed Sheet")
                {
                    //txtCurrentQty.Text = Stock.Inventory.AvailablePrintedQty(ddPurposeRaw.SelectedValue, ddTypeRaw.SelectedValue, ddProductRaw.SelectedValue,
                    //    ddCustomerRaw.SelectedValue, ddBrandRaw.SelectedValue, ddSizeRaw.SelectedValue, ddColorRaw.SelectedValue, ddGodownRaw.SelectedValue);

                    //ltrCQty.Text = txtCurrentQty.Text;

                    //txtCurrentKg.Text = Stock.Inventory.AvailablePrintedWeight(ddPurposeRaw.SelectedValue, ddTypeRaw.SelectedValue, ddProductRaw.SelectedValue,
                    //    ddCustomerRaw.SelectedValue, ddBrandRaw.SelectedValue, ddSizeRaw.SelectedValue, ddColorRaw.SelectedValue, ddGodownRaw.SelectedValue);

                }
                else
                {
                    //txtCurrentQty.Text = Stock.Inventory.AvailableNonPrintedQty(ddPurposeRaw.SelectedValue, ddTypeRaw.SelectedValue, ddProductRaw.SelectedValue,
                    //    ddGodownRaw.SelectedValue);

                    //ltrCQty.Text = txtCurrentQty.Text;
                    //txtCurrentKg.Text = Stock.Inventory.AvailableNonPrintedWeight(ddPurposeRaw.SelectedValue, ddTypeRaw.SelectedValue, ddProductRaw.SelectedValue, ddGodownRaw.SelectedValue);
                    //txtPrice.Visible = true;
                    //txtPrice.Text = Stock.Inventory.LastNonprintedPrice(ddPurposeRaw.SelectedValue, ddTypeRaw.SelectedValue, ddProductRaw.SelectedValue);
                }
            }

            BindItemGrid();
            BindGridView1();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
        finally { LoadFormControls(); }
        */
    }

    private void LoadFormControls()
    {
        /* if (ddSubGroupRaw.SelectedValue == "10")
         {
             pnlSpec.Visible = true;
         }
         else
         {
             pnlSpec.Visible = false;
         }

         if (ddSubGroupRaw.SelectedValue == "9")
         {
             ddTypeArea.Attributes.Remove("class");
             ddTypeArea.Attributes.Add("class", "col-md-4");
         }
         else
         {
             ddTypeArea.Attributes.Remove("class");
             ddTypeArea.Attributes.Add("class", "col-md-4 hidden");
             ddTypeRaw.SelectedValue = "Raw Sheet";
         }

         if (ddTypeRaw.SelectedValue == "Printed Sheet")
         {
             pnlExtra.Visible = true;
         }
         else
         {
             pnlExtra.Visible = false;
         }*/
    }

    protected void ddGradeRaw_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //LoadCategoryRaw();
        //LoadProductRaw();
        LoadSpecList("filter");
        QtyinStockRaw();
    }

    protected void ddCategoryRaw_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //LoadProductRaw();
        LoadSpecList("filter");
        QtyinStockRaw();
        //ddCategoryRaw.Focus();
    }

    protected void ddProductRaw_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSpecList("filter");
        QtyinStockRaw();
        txtUnitPrice.Focus();
    }

    protected void ddCustomerRaw_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //ddBrandRaw.Items.Clear();
        //ListItem lst = new ListItem("--- all ---", "");
        //ddBrandRaw.Items.Insert(ddBrand.Items.Count, lst);
        //LoadBrandRaw();

        QtyinStockRaw();
    }



    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            //int index = Convert.ToInt32(e.RowIndex);
            //Label lblItemDelete = GridView1.Rows[index].FindControl("lblDeleteId") as Label;
            ////TextBox txtQty = ItemGrid.Rows[index].FindControl("txtQty") as TextBox;

            //string prdId = RunQuery.SQLQuery.ReturnString("Select ProductID from FinishedProducts where pId='" + lblItemDelete.Text + "'");
            //string sizeId = RunQuery.SQLQuery.ReturnString("Select SizeID from FinishedProducts where pId='" + lblItemDelete.Text + "'");
            //string brandId = RunQuery.SQLQuery.ReturnString("Select BrandID from FinishedProducts where pId='" + lblItemDelete.Text + "'");

            //int deliveredQty = Convert.ToInt32(SQLQuery.ReturnString("Select COUNT(ID) from SaleDetails where SizeId='" + sizeId + "' AND ProductID='" + prdId + "' AND BrandID='" + brandId + "'"));

            //if (deliveredQty == 0)
            //{
            //    SqlCommand cmd7 = new SqlCommand("DELETE FinishedProducts WHERE pid=" + lblItemDelete.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            //    cmd7.Connection.Open();
            //    cmd7.ExecuteNonQuery();
            //    cmd7.Connection.Close();

            //    BindGridView1();

            //    Notify("Item deleted from Finished Products list...", "success", lblMsg2);
            //}
            //else
            //{
            //    Notify("This product can not be deleted as sold earlier.", "error", lblMsg2);
            //}
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
    }


}