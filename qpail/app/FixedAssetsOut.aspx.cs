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


public partial class app_RemoveFixedAssets : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDate.Text = DateTime.Now.ToShortDateString();
            txtPurchaseDate.Text = DateTime.Now.ToShortDateString();
            //txtAQty.Text = SQLQuery.ReturnString("SELECT COUNT(QtyBalance) FROM FixedAssets");

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            ddGroup.DataBind();
            ddSubGroup.DataBind();
            ddGrade.DataBind();
            ddCategory.DataBind();
            ddProduct.DataBind();
            txtAQty.Text = availablestock();
            QtyinFixedAssets();
            BindItemGrid();
            // InvIDNo();


        }
        //txtInv.Text = InvIDNo();
    }

    public string InvIDNo()
    {
        string lName = Page.User.Identity.Name.ToString();
        string yr = Convert.ToDateTime(txtPurchaseDate.Text).Year.ToString(); // DateTime.Now.Year.ToString();
        DateTime countForYear = Convert.ToDateTime(txtPurchaseDate.Text);

        SqlCommand cmd =
            new SqlCommand(
                "Select CONVERT(varchar, (ISNULL(COUNT(id),0)+ 1 )) from FixedAssets where EntryDate>=@EntryDate",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryDate", countForYear);
        cmd.Connection.Open();
        string InvNo = Convert.ToString(cmd.ExecuteScalar());
        while (InvNo.Length < 4)
        {
            InvNo = "0" + InvNo;
        }
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        InvNo = yr + "" + InvNo;



        string isExist = SQLQuery.ReturnString("Select ItemCode from FixedAssets where ItemCode='" + InvNo + "'  ");
        int i = 0;
        while (isExist != "")
        {
            i++;
            InvNo =
                SQLQuery.ReturnString("Select CONVERT(varchar, (ISNULL (COUNT(id),0)+ " + i +
                                      " )) from FixedAssets where EntryDate>='" + countForYear.ToString("yyyy-MM-dd") +
                                      "' AND  EntryDate<='" + countForYear.ToString("yyyy-12-31") + "'  ");
            while (InvNo.Length < 4)
            {
                InvNo = "0" + InvNo;
            }

            InvNo = yr + "" + InvNo;
        }

        txtItemCode.Text = InvNo;
        return InvNo;

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnAdd.Text == "Stock Out")
            {
                if (Convert.ToDecimal(availablestock()) > 0)
                {
                    string lName = Page.User.Identity.Name.ToString();

                    SQLQuery.ExecNonQry("UPDATE [FixedAssets] SET [OutDate] = '" +
                                        Convert.ToDateTime(txtPurchaseDate.Text).ToString("yyyy-MM-dd") +
                                        "',[Remark] = '" +
                                        txtSpec.Text + "', [DeliveredQty] = '1', StockOutBy='" + lName +
                                        "'  WHERE  Id=(Select MIN(Id) from FixedAssets WHERE ItemCode='" +
                                        ddItemCode.Text + "') ");

                    txtAQty.Text = availablestock();
                    ddItemCode.DataBind();

                    lblMsg2.Attributes.Add("class", "xerp_success");
                    lblMsg2.Text = "Fixed Assets have been stocked out Successfully";
                }
                else
                {
                    lblMsg2.Attributes.Add("class", "xerp_warning");
                    lblMsg2.Text = "Please fill in all the required fields...";
                    }
            }
            else
            {
                string lName = Page.User.Identity.Name.ToString();
                SQLQuery.ExecNonQry("UPDATE [FixedAssets] SET [OutDate] = '" +
                                    Convert.ToDateTime(txtPurchaseDate.Text).ToString("yyyy-MM-dd") + "',[Remark] = '" +
                                    txtSpec.Text + "', [DeliveredQty] = '1', StockOutBy='" + lName +
                                    "'  WHERE  Id=(Select MIN(Id) from FixedAssets WHERE ItemCode='" +
                                    ddItemCode.Text + "') ");

                txtAQty.Text = availablestock();
                ddItemCode.DataBind();

                lblMsg2.Attributes.Add("class", "xerp_success");
                lblMsg2.Text = "Fixed Assets have been stocked out Successfully";
            }
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "ERROR: " + ex.ToString();
        }
        finally
        {
            //BindItemGrid();
            ClearForm();
        }
    }

    private void ClearForm()
    {
        btnAdd.Text = "Add";
        //txtPurchaseDate.Text = "";
        txtItemCode.Text = "";
        txtModel.Text = "";
        txtSpec.Text = "";
        txtWarranty.Text = "";
        txtDeliverQty.Text = "1";
        txtPrice.Text = "0";
    }

    private void BindItemGrid()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand(@"SELECT Id, 
            (SELECT  [ItemName] FROM [Products] WHERE [ProductID]= FixedAssets.ProductID) AS ProductID, 
            ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, QtyBalance, DeliveredQty, UnitCost, 
            (SELECT  [Company] FROM [Party] WHERE [PartyID]= FixedAssets.Customer) AS Customer, BrandID, SizeId,CompanyFor,
            (SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=FixedAssets.Color) AS Color, CurrentValue, PurchaseCost, ItemCode,Remark,OutDate,StockOutBy
            FROM FixedAssets WHERE FixedAssetsType='FixedAssets' AND (DeliveredQty >= '1')  ORDER BY Id",
            new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //cmd.CommandText = "SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal FROM FixedAssets WHERE EntryBy=@EntryBy AND OrderID='' ORDER BY Id";

        cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        cmd.Connection.Open();
        ItemGrid2.EmptyDataText = "No items to view...";
        ItemGrid2.DataSource = cmd.ExecuteReader();
        ItemGrid2.DataBind();
        txtAQty.Text = availablestock();
        cmd.Connection.Close();
    }

    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //ltrQty.Text = SQLQuery.ReturnString("Select ISNULL(SUM(QtyBalance),0) from FixedAssetsTmp where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "pcs, " +
        //    SQLQuery.ReturnString("Select ISNULL(SUM(UnitWeight),0) from FixedAssetsTmp where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "kg";

    }

    protected void ItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = ItemGrid2.Rows[index].FindControl("lblEntryId") as Label;

            SQLQuery.ExecNonQry("DELETE FixedAssets WHERE Id=" + lblItemCode.Text);
           // BindItemGrid();
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "Item deleted from order ...";

            btnAdd.Text = "Add";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }

    }

    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (btnSave.Text == "Save")
    //        {
    //            ExecuteInsert();
    //            txtQty.Text = "";

    //            QtyinFixedAssets();
    //            
    //            lblMsg.Attributes.Add("class", "xerp_success");
    //            lblMsg.Text = "FixedAssets adjustment saved successfully...";
    //        }
    //        else
    //        {
    //            btnSave.Text = "Save";
    //            //EditField.Attributes.Add("class", "form-group hidden");
    //            lblMsg.Attributes.Add("class", "xerp_success");
    //            //lblMsg.Text = "Info successfully updated for " + DropDownList1.SelectedItem.Text;
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg.Attributes.Add("class", "xerp_error");
    //        lblMsg.Text = "Error: " + ex.ToString();
    //    }
    //    finally
    //    {
    //        BindItemGrid();
    //    }
    //}

    /*private void ExecuteInsert()
    {
        string orderNo = InvIDNo();
        string lName = Page.User.Identity.Name.ToString();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO FixedAssets (OrderID, OrderDate, GodownID, GodownName, LocationID, LocationName, Remarks, TotalQty, EntryBy, ProjectId)" +
                                                    " VALUES (@OrderID, @OrderDate, @GodownID, @GodownName, @LocationID, @LocationName, @Remarks, @TotalQty, @EntryBy, @ProjectId)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@OrderID", orderNo); //ddCategory.SelectedValue);
        cmd2.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        
        cmd2.Parameters.AddWithValue("@Remarks", txtAddress.Text);
        cmd2.Parameters.AddWithValue("@TotalQty", ltrQty.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@ProjectId", lblProject.Text);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        SqlCommand cmd = new SqlCommand("UPDATE FixedAssetsTmp SET OrderID='" + orderNo + "' WHERE  EntryBy=@EntryBy AND OrderID=''  AND FixedAssetsType='FixedAssets' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryBy", lName);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        //Sock-in

        foreach (GridViewRow row in ItemGrid.Rows)
        {
            Label lblEntryId = row.FindControl("lblEntryId") as Label;
            string sizeId = SQLQuery.ReturnString("SELECT SizeID FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");
            string brandID = SQLQuery.ReturnString("SELECT brandID FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");
            string productID = SQLQuery.ReturnString("SELECT productID FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");
            string ProductName = SQLQuery.ReturnString("Select ItemName from Products where ProductID = (SELECT productID FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "'))");

            string spec = SQLQuery.ReturnString("SELECT ProductName FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");
            string itemType = SQLQuery.ReturnString("SELECT ItemType FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");
            string customer = SQLQuery.ReturnString("SELECT Customer FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");
            string color = SQLQuery.ReturnString("SELECT Color FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");
            string CompanyFor = SQLQuery.ReturnString("SELECT CompanyFor FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");


            Label lbliQty = row.FindControl("lbliQty") as Label;
            Label lblUnitWeight = row.FindControl("lblUnitWeight") as Label;
            Label lblQtyBalance = row.FindControl("lblQtyBalance") as Label;

            int inQty = Convert.ToInt32(lbliQty.Text); int outQty = 0;
            decimal unitCost = Convert.ToDecimal(lblUnitWeight.Text); decimal outWeight = 0;
            string type = "FixedAssets-in"; string detail = "FixedAssets Adjustment";
            //string ItemSerialNo = "Purchase Date: " + sizeId + ". Specification: " + spec + ". Model: " + brandID + ". Warranty: " + CompanyFor;

            if (inQty < 0)
            {
                inQty = 0; outQty = Convert.ToInt32(lbliQty.Text) * (-1);
                type = "FixedAssets-out";
            }
            if (unitCost < 0)
            {
                unitCost = 0; outWeight = Convert.ToDecimal(lblUnitWeight.Text) * (-1);
            }
            /*
            //Stock.Inventory.SaveToStock("", orderNo, detail, lblEntryId.Text, "", customer, "", color, "", productID, ProductName, itemType, ddGodown.SelectedValue, ddLocation.SelectedValue, ddGroup.SelectedValue, inQty, outQty, unitCost, 0, outWeight, spec, type, "Adjustment", ddLocation.SelectedItem.Text, lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            SqlCommand cmd3 = new SqlCommand(
                        "INSERT INTO FixedAssets ( EntryID, InvoiceID, EntryType, Purpose, RefNo, ItemGroup, ProductID, ProductName, ItemType, Customer, BrandID, SizeID, Color, Spec, Description, WarehouseID, LocationID, InQuantity, OutQuantity, Price, ItemSerialNo, Remark, Status, FixedAssetsLocation, EntryBy, EntryDate)" +
                        " VALUES (@EntryID, @InvoiceID, @EntryType,'" + Purpose + "', @RefNo, @ItemGroup, @ProductID, @ProductName, @ItemType, @Customer, @ProductID, @ProductName, @ItemType, @WarehouseID, @LocationID, @ItemGroup, @InQuantity, @OutQuantity, " + unitPrice + ", @InWeight, @OutWeight, @ItemSerialNo, @Remark, @Status, @FixedAssetsLocation, @EntryBy, '" + EntryDate + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd3.Parameters.AddWithValue("@EntryID", EntryID);
            cmd3.Parameters.AddWithValue("@InvoiceID", InvoiceID);
            cmd3.Parameters.AddWithValue("@EntryType", EntryType);
            cmd3.Parameters.AddWithValue("@SizeID", Purpose);
            cmd3.Parameters.AddWithValue("@Customer", Customer);
            cmd3.Parameters.AddWithValue("@BrandID", BrandID);
            cmd3.Parameters.AddWithValue("@Color", color);
            cmd3.Parameters.AddWithValue("@ProductID", ProductID);

            cmd3.Parameters.AddWithValue("@ProductName", ProductName);
            cmd3.Parameters.AddWithValue("@ItemType", ItemType);
            cmd3.Parameters.AddWithValue("@WarehouseID", WarehouseID);
            cmd3.Parameters.AddWithValue("@LocationID", LocationID);
            cmd3.Parameters.AddWithValue("@ItemGroup", ItemGroup);

            cmd3.Parameters.AddWithValue("@InQuantity", Convert.ToDecimal(InQuantity));
            cmd3.Parameters.AddWithValue("@OutQuantity", Convert.ToDecimal(OutQuantity));
            cmd3.Parameters.AddWithValue("@InWeight", Convert.ToDecimal(InWeight));
            cmd3.Parameters.AddWithValue("@OutWeight", Convert.ToDecimal(OutWeight));

            cmd3.Parameters.AddWithValue("@ItemSerialNo", ItemSerialNo);
            cmd3.Parameters.AddWithValue("@Remark", Remark);
            cmd3.Parameters.AddWithValue("@Status", Status);
            cmd3.Parameters.AddWithValue("@StockLocation", StockLocation);
            cmd3.Parameters.AddWithValue("@EntryBy", EntryBy);

            cmd3.Connection.Open();
            cmd3.ExecuteNonQuery();
            cmd3.Connection.Close();
        }
    }*/

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("Delete FixedAssets WHERE EntryBy=@EntryBy AND OrderID='' ",
            new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryBy", lName);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();
        Response.Redirect("./Order-Entry.aspx");
    }

    protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddSubGroup.DataBind();
        ddGrade.DataBind();

        ddCategory.DataBind();
        ddProduct.DataBind();
        QtyinFixedAssets();
        txtAQty.Text = availablestock();

    }

    protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddGrade.DataBind();
            ddCategory.DataBind();
            ddProduct.DataBind();
            QtyinFixedAssets();
            txtAQty.Text = availablestock();

        }
        catch (Exception ex)
        {

            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error: " + ex.ToString();
        }
    }


    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddCategory.DataBind();
        ddProduct.DataBind();
        QtyinFixedAssets();
        txtAQty.Text = availablestock();
    }

    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddProduct.DataBind();
        QtyinFixedAssets();
        txtAQty.Text = availablestock();
        ddCategory.Focus();
    }

    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinFixedAssets();

        // 
    }


    private void QtyinFixedAssets()
    {
        try
        {
            ltrUnit.Text =
                SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");

            //txtCurrentQty.Text = Stock.Inventory.NonUsableFixedAssestsQty(ddProduct.SelectedValue, ddGodown.SelectedValue);
            SQLQuery.Empty2Zero(txtCurrentQty);
            ltrCQty.Text = txtCurrentQty.Text;

            //BindItemGrid();
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
        finally
        {
        }

    }

    protected void imgUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        lblMsg.Attributes.Add("class", "xerp_error");
        lblMsg.Text = "File Uploaded";
    }


    protected void ItemGrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //DropDownList1.DataBind();
            int index = Convert.ToInt32(ItemGrid2.SelectedIndex);
            Label lblItemName = ItemGrid2.Rows[index].FindControl("lblEntryId") as Label;
            lblOrderID.Text = lblItemName.Text;
            EditMode(lblItemName.Text);
            btnAdd.Text = "Update";

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
        SqlCommand cmd =
            new SqlCommand(
                @"SELECT ItemType, ProductID, SizeId, Customer, BrandID, Color, Quantity, UnitCost, GodownID, ProductName, CompanyFor, ItemCode,Remark,OutDate,StockOutBy FROM [FixedAssets] WHERE Id='" +
                entryID + "'",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            string productID = dr[1].ToString();

            ddGroup.SelectedValue = Stock.Inventory.GetItemGroup(productID);
            ddSubGroup.DataBind();
            ddSubGroup.SelectedValue = Stock.Inventory.GetItemSubGroup(productID);
            ddGrade.DataBind();
            ddGrade.SelectedValue = Stock.Inventory.GetItemGrade(productID);
            ddCategory.DataBind();
            ddCategory.SelectedValue = Stock.Inventory.GetItemCategory(productID);
            ddProduct.DataBind();
            ddProduct.SelectedValue = productID;

            txtPurchaseDate.Text = dr[2].ToString();
            txtModel.Text = dr[4].ToString();
            //txtItemCode.Text = dr[10].ToString();
            txtSpec.Text = dr["Remark"].ToString();
            txtWarranty.Text = dr[10].ToString();
            txtDeliverQty.Text = dr[6].ToString();
            txtPrice.Text = dr[7].ToString();
            txtPurchaseDate.Text = Convert.ToDateTime(dr["OutDate"].ToString()).ToString("dd/MM/yyyy");
            //ddGodown.SelectedValue = dr[8].ToString();
        }
        cmd.Connection.Close();

    }

    protected void ddGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        // ddLocation.DataBind();
        //BindItemGrid();

        QtyinFixedAssets();
    }

    //protected void ddSubGrp_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
    //    RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

    //    gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
    //    RunQuery.SQLQuery.PopulateDropDown(gQuery, ddcategory, "CategoryID", "CategoryName");
    //}
    private string availablestock()
    {
        return
            SQLQuery.ReturnString(
                "SELECT ISNULL(SUM(Quantity),0) - ISNULL(SUM(DeliveredQty),0) FROM FixedAssets WHERE ProductID = '" +
                ddProduct.SelectedValue + "'");
    }
    
    
}