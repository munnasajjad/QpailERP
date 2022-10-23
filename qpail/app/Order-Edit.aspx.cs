using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;

public partial class app_Order_Edit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDate.Text = DateTime.Now.ToShortDateString();

            //string orderNo = base.Request.QueryString["ID"];
            //ddOrders.SelectedValue= orderNo;
            //LoadSummesion("");

            ddCustomer.DataBind();
            ddOrders.DataBind();
            txtPoNo.Text = ddOrders.SelectedValue;
            BindItemGrid();

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            ddProduct.DataBind();
            ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");
        }

        //getproducts();
    }


    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    
    public static Int32 EntrySl(int sid, string iCode)
    {
        SqlCommand cmd3 = new SqlCommand("SELECT ISNULL(MAX(ItemSl),0) FROM SalesGridTemp where (sid=" + sid + ")", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd3.Connection.Open();
        Int32 ItemSl = 1 + Convert.ToInt32(cmd3.ExecuteScalar());
        cmd3.Connection.Close();
        cmd3.Connection.Dispose();

        return ItemSl;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddOrders.SelectedValue!="")
            {
                
            if (btnAdd.Text == "Add")
            {
                string lName = Page.User.Identity.Name.ToString();
                if (txtQty.Text == "")
                {
                    txtQty.Text = "1";
                }
                //SizeId, ProductID, BrandID
                SqlCommand cmde = new SqlCommand("SELECT ProductName FROM OrderDetails WHERE SizeId ='" + ddSize.SelectedValue + "' AND ProductID ='" + ddProduct.SelectedValue + "' AND BrandID ='" + ddBrand.SelectedValue + "' AND  OrderID ='' AND EntryBy ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmde.Connection.Open();
                string isExist = Convert.ToString(cmde.ExecuteScalar());
                cmde.Connection.Close();

                if (isExist == "" && ddProduct.SelectedValue != "")
                {
                    InsertData();
                    txtQty.Text = "";
                    txtRate.Text = "";
                    txtAmount.Text = "0";
                    txtWeight.Text = "";
                    ddSize.Focus();
                }
                else
                {
                    Notify("ERROR: Product Already exist or product selection box is empty!", "warn", lblMsg2);
                }
            }
            else
            {
                ExecuteUpdate();
                
                txtQty.Text = "";
                txtRate.Text = "";
                txtAmount.Text = "0";
                txtWeight.Text = "";

                btnAdd.Text = "Add";
                Notify("Item updated successfully" , "success", lblMsg);
            }
            //ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);

            }
            else
            {
                Notify("Invalid Order No.!", "error", lblMsg);    
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(),"error", lblMsg); 
        }
        finally
        {

            BindItemGrid();
        }
    }

    private void BindItemGrid()
    {
        if (ddOrders.SelectedValue != "")
        {
            string lName = Page.User.Identity.Name.ToString();
            string OrderID = ddOrders.SelectedValue;

            SqlCommand cmd = new SqlCommand("SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal FROM OrderDetails WHERE OrderID='" + OrderID + "' ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
            cmd.Connection.Open();
            ItemGrid.EmptyDataText = "No items to view...";
            ItemGrid.DataSource = cmd.ExecuteReader();
            ItemGrid.DataBind();
            cmd.Connection.Close();

            SqlCommand cmd7 = new SqlCommand("SELECT OrderDate, DeliveryDate, CustomerName, DeliveryAddress, TotalAmount, Discount, Vat, PayableAmount FROM [Orders] WHERE OrderID='" + OrderID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                txtDate.Text = Convert.ToDateTime(dr[0].ToString()).ToShortDateString();
                txtDeliveryDate.Text = Convert.ToDateTime(dr[1].ToString()).ToShortDateString();
                //ddCustomer.SelectedValue= dr[2].ToString();
                //ddCustomer.Enabled = false;
                ddBrand.DataBind();

                txtAddress.Text = dr[3].ToString();
                txtTotal.Text = dr[4].ToString();
                txtDiscount.Text = dr[5].ToString();
                txtVat.Text = dr[6].ToString();
                txtPay.Text = dr[7].ToString();
                btnSave.Enabled = true;
            }
            cmd7.Connection.Close();
            LoadSummesion(OrderID);
        }
        else
        {
            txtAddress.Text = "";
            txtTotal.Text = "";
            txtVat.Text = "";
            txtPay.Text = "";
            btnSave.Enabled = false;

            ItemGrid.DataSource = null;
            ItemGrid.DataBind();
        }
    }
    private void LoadSummesion(string OrderId)
    {
        //string OrderID = RunQuery.SQLQuery.ReturnString("Select OrderID from OrderDetails where Id='" + lblItemCode.Text + "'");
        ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Quantity),0) from OrderDetails where OrderID='" + OrderId + "'");
        string ttl = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(ItemTotal),0) from OrderDetails where OrderID='" + OrderId + "'");
        RunQuery.SQLQuery.ExecNonQry("Update Orders set TotalAmount=" + ttl + " where OrderID='" + OrderId + "'");
        txtTotal.Text = ttl;
    }

    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Quantity),0) from OrderDetails where OrderID ='" + ddOrders.SelectedValue + "'");
        ltrWeight.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(TotalWeight),0) from OrderDetails where OrderID ='" + ddOrders.SelectedValue + "'");
    }
    protected void ItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;
            TextBox txtQty = ItemGrid.Rows[index].FindControl("txtQty") as TextBox;

            string OrderID = RunQuery.SQLQuery.ReturnString("Select OrderID from OrderDetails where Id='" + lblItemCode.Text + "'");
            string deliveredQty = RunQuery.SQLQuery.ReturnString("Select SUM(DeliveredQty) from OrderDetails where OrderID='" + OrderID + "'");

            if (deliveredQty == "0")
            {
                SqlCommand cmd7 = new SqlCommand("SELECT  SizeId, ProductID, BrandID, ProductName, UnitCost, Quantity, UnitType FROM [OrderDetails] WHERE Id='" + lblItemCode.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                SqlDataReader dr = cmd7.ExecuteReader();
                if (dr.Read())
                {
                    SqlCommand cmd2 = new SqlCommand("INSERT INTO OrdItemDeleted (OrderID, SizeId, ProductID, BrandID, ProductName, UnitCost, Quantity, UnitType, ItemTotal, EntryBy) VALUES (@OrderID, @SizeId, @ProductID, @BrandID, @ProductName, @UnitCost, @Quantity, '" + ltrUnit.Text + "', @ItemTotal, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                    cmd2.Parameters.AddWithValue("@OrderID", ddOrders.SelectedValue);
                    cmd2.Parameters.AddWithValue("@SizeId", dr[0].ToString());
                    cmd2.Parameters.AddWithValue("@ProductID", dr[1].ToString());
                    cmd2.Parameters.AddWithValue("@BrandID", dr[2].ToString());
                    cmd2.Parameters.AddWithValue("@ProductName", dr[3].ToString());

                    cmd2.Parameters.AddWithValue("@UnitCost", Convert.ToDecimal(dr[4].ToString()));
                    cmd2.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(dr[5].ToString()));
                    cmd2.Parameters.AddWithValue("@ItemTotal", Convert.ToDecimal(dr[4].ToString()) * Convert.ToDecimal(dr[5].ToString()));
                    cmd2.Parameters.AddWithValue("@UnitType", dr[6].ToString());
                    cmd2.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

                    cmd2.Connection.Open();
                    cmd2.ExecuteNonQuery();
                    cmd2.Connection.Close();
                }

                cmd7.Connection.Close();

                SqlCommand cmd = new SqlCommand("DELETE OrderDetails WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                BindItemGrid();
                lblMsg2.Attributes.Add("class", "xerp_warning");
                lblMsg2.Text = "Item deleted from order ...";
            }
            else
            {
                lblMsg2.Attributes.Add("class", "xerp_warning");
                lblMsg2.Text = "Order is Locked! There is some pending delivery...";
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg); 
        }

    }

    public static class ResponseHelper
    {
        public static void Redirect(string url, string target, string windowFeatures)
        {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures))
            {

                context.Response.Redirect(url);
            }
            else
            {
                Page page = (Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException(
                        "Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(Page),
                    "Redirect",
                    script,
                    true);
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDeliveryDate.Text != "" && ddOrders.SelectedValue != "") 
            {
                if (txtPoNo.Text != "" && ddOrders.SelectedValue != "")
                {
                    RunQuery.SQLQuery.ExecNonQry("Update Orders set OrderID='" + txtPoNo.Text + "' Where OrderID='" + ddOrders.SelectedValue + "'");
                    RunQuery.SQLQuery.ExecNonQry("Update OrderDetails set OrderID='" + txtPoNo.Text + "' Where OrderID='" + ddOrders.SelectedValue + "'");
                }
                LoadSummesion(ddOrders.SelectedValue);
                UpdateOrder();
                ddOrders.DataBind();
                BindItemGrid();

                btnSave.Text = "Save";
                //EditField.Attributes.Add("class", "form-group hidden");

                Notify("Saved Successfully", "success", lblMsg);
                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "PO Info updated successfully.";
            }
            else
            {
                txtDeliveryDate.Focus();
                Notify("Please input delivery date", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg); 
        }
        finally
        {
            //txtDeliveryDate.Focus();
            BindItemGrid();
            //ddOrders.DataBind();
        }
    }

    private void InsertData()
    {
        string lName = Page.User.Identity.Name.ToString(); 
        string size = ddSize.SelectedItem.Text;
        if (size == "N/A")
        {
            size = "";
        }

        string productName = size + " " + ddBrand.SelectedItem.Text + " " + ddProduct.SelectedItem.Text;

        if (chkMerge.Checked == true)
        {
            productName = size + " " + ddBrand.SelectedItem.Text + " " + ddGrade.SelectedItem.Text + " " + ddProduct.SelectedItem.Text;
        }

        decimal weight = 0;
        if (txtWeight.Text != "")
        {
            weight = Convert.ToDecimal(txtWeight.Text);
        }

        SqlCommand cmd2 = new SqlCommand("INSERT INTO OrderDetails (OrderID, SizeId, ProductID, BrandID, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy) VALUES (@OrderID, @SizeId, @ProductID, @BrandID, @ProductName, @UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@OrderID", ddOrders.SelectedValue);
        cmd2.Parameters.AddWithValue("@SizeId", ddSize.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductID", ddProduct.SelectedValue);
        cmd2.Parameters.AddWithValue("@BrandID", ddBrand.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductName", productName);

        cmd2.Parameters.AddWithValue("@UnitCost", Convert.ToDecimal(txtRate.Text));
        cmd2.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@UnitWeight", weight);

        cmd2.Parameters.AddWithValue("@ItemTotal", Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@TotalWeight", weight * Convert.ToDecimal(txtQty.Text)/1000M);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        string lName = Page.User.Identity.Name.ToString();
        string size = ddSize.SelectedItem.Text;
        if (size == "N/A")
        {
            size = "";
        }

        string productName = size + " " + ddBrand.SelectedItem.Text + " " + ddProduct.SelectedItem.Text;

        if (chkMerge.Checked == true)
        {
            productName = size + " " + ddBrand.SelectedItem.Text + " " + ddGrade.SelectedItem.Text + " " + ddProduct.SelectedItem.Text;
        }

        SqlCommand cmd2 = new SqlCommand("UPDATE OrderDetails SET SizeId='" + ddSize.SelectedValue + "', ProductID='" + ddProduct.SelectedValue + "', BrandID='" + ddBrand.SelectedValue + "'," +
                                "ProductName=@ProductName, UnitCost=@UnitCost, UnitWeight=@UnitWeight, Quantity=@Quantity, " +
                                "ItemTotal=@ItemTotal, TotalWeight=@TotalWeight  where (id ='" + lblOrderID.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductName", productName);
        cmd2.Parameters.AddWithValue("@UnitCost", Convert.ToDecimal(txtRate.Text));
        cmd2.Parameters.AddWithValue("@UnitWeight", txtWeight.Text);
        cmd2.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@ItemTotal", Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@TotalWeight", Convert.ToDecimal(txtWeight.Text) * Convert.ToDecimal(txtQty.Text)/1000M);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();

    }


    private void UpdateOrder()
    {
        string orderDate = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        string delDate = Convert.ToDateTime(txtDeliveryDate.Text).ToString("yyyy-MM-dd");

        RunQuery.SQLQuery.ExecNonQry("Update Orders set OrderDate='" + orderDate + "', DeliveryDate='" + delDate + "', DeliveryAddress='" + txtAddress.Text + "', TotalAmount='" + txtTotal.Text + "' Where OrderID='" + ddOrders.SelectedValue + "'");

    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string delStatus = RunQuery.SQLQuery.ReturnString("Select DeliveryStatus from Orders Where WHERE OrderID='" + ddOrders.SelectedValue + "'");

        if (delStatus == "P")
        {
            string lName = Page.User.Identity.Name.ToString();
            SqlCommand cmd1 = new SqlCommand("Update Orders set DeliveryStatus='C', UpdateBy=@EntryBy, UpdateDate=@UpdateDate WHERE OrderID='" + ddOrders.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd1.Parameters.AddWithValue("@EntryBy", lName);
            cmd1.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
            cmd1.Connection.Open();
            cmd1.ExecuteNonQuery();
            cmd1.Connection.Close();
            cmd1.Connection.Dispose();

            Response.Redirect("./Order-Edit.aspx");
        }
        else
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "<b>Unable to cancel.</b> This order has some pending delivery!";
        }
    }
    protected void ddOrders_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtPoNo.Text = ddOrders.SelectedValue;
        BindItemGrid();
    }
    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {

        ddBrand.DataBind();
        ddOrders.DataBind();
        BindItemGrid();

        if(ddOrders.SelectedValue=="")
        {
            txtQty.Text = "";
            txtRate.Text = "";
            txtAmount.Text = "0";
            txtWeight.Text = "";

            btnAdd.Text = "Add";
        }
    }
    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        txtQty.Focus();
    }
    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddCategory.DataBind();
        ddProduct.DataBind();
        QtyinStock();
        ddGrade.Focus();
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddProduct.DataBind();
        QtyinStock();
        ddCategory.Focus();
    }

    private void QtyinStock()
    {
        ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");
        txtStock.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where SizeID='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "'");
        txtWeight.Text = RunQuery.SQLQuery.ReturnString("Select UnitWeight from OrderDetails  where Id=(Select ISNULL(MAX(Id),0) from OrderDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");
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
            Notify(ex.Message.ToString(), "error", lblMsg); 
        }
    }

    private void EditMode(string entryID)
    {
        SqlCommand cmd = new SqlCommand("SELECT SizeId, ProductID, BrandID, UnitCost, UnitWeight, Quantity, DeliveredQty FROM [OrderDetails] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            ddSize.SelectedValue = dr[0].ToString();
            string productID = dr[1].ToString();
            string catID = RunQuery.SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + productID + "'");
            string grdID = RunQuery.SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
            ddGrade.SelectedValue = grdID;
            ddCategory.DataBind();
            ddCategory.SelectedValue = RunQuery.SQLQuery.ReturnString("Select CategoryID from Products where ProductID='" + productID + "'");
            
            ddProduct.DataBind();
            ddProduct.SelectedValue = productID;

            ddBrand.SelectedValue = dr[2].ToString();
            txtRate.Text = dr[3].ToString();
            txtWeight.Text = dr[4].ToString();
            txtQty.Text = dr[5].ToString();

            //txtWeight.Text = dr[6].ToString();

            ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + productID + "'");
            txtStock.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where SizeID='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + productID + "'");

            txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtRate.Text));
        }
        cmd.Connection.Close();

    }
}