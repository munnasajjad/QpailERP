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

public partial class app_Stock_Adj_Machine : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDate.Text = DateTime.Now.ToShortDateString();

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            ddGroup.DataBind();
            ddSubGroup.DataBind();
            ddGrade.DataBind();
            ddCategory.DataBind();
            ddProduct.DataBind();

            ddGodown.DataBind();
            ddLocation.DataBind();

            QtyinStock();
            BindItemGrid();
            GridView1.DataBind();
        }
        //txtInv.Text = InvIDNo();
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

        InvNo = "Mac-Adj-" + InvNo + "/" + yr.Substring(2, 2);
        return InvNo;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnAdd.Text == "Add")
            {
                SQLQuery.Empty2Zero(txtQty);
                InsertData();
                ClearForm();
            }
            else
            {
                SQLQuery.ExecNonQry("Delete StockinDetailsRaw where (id ='" + lblOrderID.Text + "')");
                InsertData();
                ClearForm();
                lblMsg2.Attributes.Add("class", "xerp_success");
                lblMsg2.Text = "Item updated successfully";
            }
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
        finally
        {
            BindItemGrid();
        }
    }

    private void ClearForm()
    {
        btnAdd.Text = "Add";
        txtPurchaseDate.Text = "";
        txtModel.Text = "";
        txtSpec.Text = "";
        txtWarranty.Text = "";
        txtQty.Text = "";
        txtPrice.Text = "";
    }

    private void InsertData()
    {
        string lName = Page.User.Identity.Name.ToString();
        string productName = ddProduct.SelectedItem.Text;

        int balance = Convert.ToInt32(txtQty.Text) - Convert.ToInt32(ltrCQty.Text);
        SqlCommand cmd2 = new SqlCommand("INSERT INTO StockinDetailsRaw (StockType, OrderID, SizeId, ProductID, ProductName, BrandID, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy, CompanyFor, GodownID, LocationID, QtyBalance, DeliveredQty) " +
                                         "VALUES ('Machine', @OrderID, @SizeId, @ProductID, @ProductName, @BrandID, @UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, @CompanyFor, '" + ddGodown.SelectedValue + "', '" + ddLocation.SelectedValue + "','" + balance + "', '" + ltrCQty.Text + "'  )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@OrderID", "");
        cmd2.Parameters.AddWithValue("@ProductID", ddProduct.SelectedValue);

        cmd2.Parameters.AddWithValue("@SizeId", txtPurchaseDate.Text);
        cmd2.Parameters.AddWithValue("@ProductName", txtSpec.Text);
        cmd2.Parameters.AddWithValue("@BrandID", txtModel.Text);
        cmd2.Parameters.AddWithValue("@CompanyFor", txtWarranty.Text);

        cmd2.Parameters.AddWithValue("@UnitCost", txtPrice.Text);
        cmd2.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@UnitWeight", "0");// Price

        cmd2.Parameters.AddWithValue("@ItemTotal", "0");
        cmd2.Parameters.AddWithValue("@TotalWeight", "0");
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        SQLQuery.ExecNonQry("UPDATE StockinDetailsRaw SET Manufacturer='" + txtManufacturer.Text + "', CountryOrigin='" + txtCountry.Text + "', AgentName='" + txtAgent.Text + "' WHERE Id=(Select MAX(Id) from StockinDetailsRaw)  ");
    }

    private void BindItemGrid()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand(@"SELECT Id, 
            (SELECT  [ItemName] FROM [Products] WHERE [ProductID]= StockinDetailsRaw.ProductID) AS ProductID, 
            ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, QtyBalance, DeliveredQty, UnitCost, 
            (SELECT  [Company] FROM [Party] WHERE [PartyID]= StockinDetailsRaw.Customer) AS Customer, BrandID, SizeId,CompanyFor,
            (SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=StockinDetailsRaw.Color) AS Color, Manufacturer, CountryOrigin, AgentName
            FROM StockinDetailsRaw WHERE GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '" + ddLocation.SelectedValue + "' AND EntryBy=@EntryBy AND OrderID='' AND StockType='Machine'  ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //cmd.CommandText = "SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal FROM StockinDetailsRaw WHERE EntryBy=@EntryBy AND OrderID='' ORDER BY Id";

        cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        cmd.Connection.Open();
        ItemGrid.EmptyDataText = "No items to view...";
        ItemGrid.DataSource = cmd.ExecuteReader();
        ItemGrid.DataBind();
        cmd.Connection.Close();
    }

    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ltrQty.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(QtyBalance),0) FROM StockinDetailsRaw WHERE OrderID='' AND GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '" + ddLocation.SelectedValue + "'") + "pcs, " +
            SQLQuery.ReturnString("SELECT ISNULL(SUM(UnitWeight),0) FROM StockinDetailsRaw WHERE OrderID='' AND GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '" + ddLocation.SelectedValue + "'") + "kg";

    }
    protected void ItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;

            SQLQuery.ExecNonQry("DELETE StockinDetailsRaw WHERE Id=" + lblItemCode.Text);
            BindItemGrid();
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text == "Save")
            {
                ExecuteInsert();
                txtQty.Text = "";

                QtyinStock();
                GridView1.DataBind();
                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Stock adjustment saved successfully...";
            }
            else
            {
                btnSave.Text = "Save";
                //EditField.Attributes.Add("class", "form-group hidden");
                lblMsg.Attributes.Add("class", "xerp_success");
                //lblMsg.Text = "Info successfully updated for " + DropDownList1.SelectedItem.Text;
            }

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error: " + ex.ToString();
        }
        finally
        {
            BindItemGrid();
        }
    }

    private void ExecuteInsert()
    {
        string orderNo = InvIDNo();
        string lName = Page.User.Identity.Name.ToString();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO Stockin (OrderID, OrderDate, GodownID, GodownName, LocationID, LocationName, Remarks, TotalQty, EntryBy, ProjectId)" +
                                                    " VALUES (@OrderID, @OrderDate, @GodownID, @GodownName, @LocationID, @LocationName, @Remarks, @TotalQty, @EntryBy, @ProjectId)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@OrderID", orderNo); //ddCategory.SelectedValue);
        cmd2.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        cmd2.Parameters.AddWithValue("@GodownID", ddGodown.SelectedValue);
        cmd2.Parameters.AddWithValue("@GodownName", ddGodown.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@LocationID", ddLocation.SelectedValue);

        cmd2.Parameters.AddWithValue("@LocationName", ddLocation.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@Remarks", txtAddress.Text);
        cmd2.Parameters.AddWithValue("@TotalQty", ltrQty.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@ProjectId", lblProject.Text);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        SqlCommand cmd = new SqlCommand("UPDATE StockinDetailsRaw SET OrderID='" + orderNo + "' WHERE  EntryBy=@EntryBy AND OrderID='' AND StockType='Machine'  AND GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '" + ddLocation.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryBy", lName);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        //Sock-in

        foreach (GridViewRow row in ItemGrid.Rows)
        {
            Label lblEntryId = row.FindControl("lblEntryId") as Label;
            string sizeId = SQLQuery.ReturnString("SELECT SizeID FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");
            string brandID = SQLQuery.ReturnString("SELECT brandID FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");
            string productID = SQLQuery.ReturnString("SELECT productID FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");
            string ProductName = SQLQuery.ReturnString("Select ItemName from Products where ProductID = (SELECT productID FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "'))");

            string spec = SQLQuery.ReturnString("SELECT ProductName FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");
            string itemType = SQLQuery.ReturnString("SELECT ItemType FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");
            string customer = SQLQuery.ReturnString("SELECT Customer FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");
            string color = SQLQuery.ReturnString("SELECT Color FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");
            string CompanyFor = SQLQuery.ReturnString("SELECT CompanyFor FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");

            string manu = SQLQuery.ReturnString("SELECT Manufacturer FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");
            string agent = SQLQuery.ReturnString("SELECT CountryOrigin FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");
            string country = SQLQuery.ReturnString("SELECT AgentName FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");

            Label lbliQty = row.FindControl("lbliQty") as Label;
            Label lblUnitWeight = row.FindControl("lblUnitWeight") as Label;
            Label lblQtyBalance = row.FindControl("lblQtyBalance") as Label;

            int inQty = Convert.ToInt32(lbliQty.Text); int outQty = 0;
            decimal unitCost = Convert.ToDecimal(lblUnitWeight.Text); decimal outWeight = 0;
            string type = "Stock-in"; string detail = "Stock Adjustment";
            string ItemSerialNo = GenerateDesc("Purchase Date: ", sizeId) +
                                  GenerateDesc(". Manufacturer: ", manu) + GenerateDesc(". Agent: ", agent) +
                                  GenerateDesc(". Country of Origin: ", country) +
                                  GenerateDesc(". Specification: ", spec) + GenerateDesc(". Model: ", brandID) +
                                  GenerateDesc(". Warranty: ", CompanyFor);

            //"Purchase Date: " + sizeId + ". Specification: " + spec + ". Model: " + brandID + ". Warranty: " + CompanyFor;

            if (inQty < 0)
            {
                inQty = 0; outQty = Convert.ToInt32(lbliQty.Text) * (-1);
                type = "Stock-out";
            }
            if (unitCost < 0)
            {
                unitCost = 0; outWeight = Convert.ToDecimal(lblUnitWeight.Text) * (-1);
            }

            Stock.Inventory.SaveToStock("", orderNo, detail, lblEntryId.Text, "", customer, "", color, "", productID, ProductName, itemType, ddGodown.SelectedValue, ddLocation.SelectedValue, ddGroup.SelectedValue, inQty, outQty, unitCost, 0, outWeight, ItemSerialNo, type, "Adjustment", ddLocation.SelectedItem.Text, lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        }
    }

    private string GenerateDesc(string type, string value)
    {
        string result = "";
        if (value != "")
        {
            result = type + ": " + value;
        }
        return result;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("Delete StockinDetailsRaw WHERE EntryBy=@EntryBy AND OrderID=''  AND WarehouseID='" + ddGodown.SelectedValue + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
        QtyinStock();

    }

    protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddGrade.DataBind();
            ddCategory.DataBind();
            ddProduct.DataBind();
            QtyinStock();

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
        QtyinStock();
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddProduct.DataBind();
        QtyinStock();
        ddCategory.Focus();
    }

    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        txtQty.Focus();
        GridView1.DataBind();
    }


    private void QtyinStock()
    {
        try
        {
            ltrUnit.Text = SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");

            txtCurrentQty.Text = Stock.Inventory.NonUsableQty(ddProduct.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue);
            ltrCQty.Text = txtCurrentQty.Text;

            GridView1.DataBind();
            BindItemGrid();
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
        finally { }

    }
    private void TrackCustomer()
    {

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
            int index = Convert.ToInt32(ItemGrid.SelectedIndex);
            Label lblItemName = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;
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
        SqlCommand cmd = new SqlCommand(@"SELECT ItemType, ProductID, SizeId, Customer, BrandID, Color, Quantity, 
        UnitCost, GodownID, LocationID, ProductName, CompanyFor, Manufacturer, CountryOrigin, AgentName FROM [StockinDetailsRaw] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
            txtSpec.Text = dr[9].ToString();
            txtWarranty.Text = dr[10].ToString();
            txtQty.Text = dr[6].ToString();
            txtPrice.Text = dr[7].ToString();

            ddGodown.SelectedValue = dr[8].ToString();
            ddLocation.SelectedValue = dr[9].ToString();
            txtManufacturer.Text = dr[11].ToString();
            txtAgent.Text = dr[12].ToString();
            txtCountry.Text = dr[13].ToString();
        }
        cmd.Connection.Close();

    }
    protected void ddGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocation.DataBind();
        BindItemGrid();

        QtyinStock();
    }

    protected void GridView1_OnRowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        QtyinStock();
    }

}