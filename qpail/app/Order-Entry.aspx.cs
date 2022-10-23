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
using System.Drawing;
using System.Drawing.Imaging;


public partial class app_Order_Entry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDate.Text = DateTime.Now.ToShortDateString();
            txtDeliveryDate.Text = DateTime.Now.ToShortDateString();

            //LoadSummesion("");

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            ddCustomer.DataBind();
            //string OldEntryExist= RunQuery.SQLQuery.ReturnString("Select BrandID FROM OrderDetails Where OrderID=''");
            //if(OldEntryExist!="")
            //{
            //    string cusID = RunQuery.SQLQuery.ReturnString("Select CustomerID FROM CustomerBrands Where BrandID='"+OldEntryExist+"'");
            //    ddCustomer.SelectedValue = cusID;
            //    ddCustomer.Enabled = false;
            //}
            //else
            //{
            //    ddCustomer.Enabled = true;
            //}

            ddBrand.DataBind();
            ddGrade.DataBind();
            ddProduct.DataBind();
            ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");

            BindItemGrid();
            txtInv.Focus();
        }
        //txtInv.Text = InvIDNo();
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

    public string InvIDNo()
    {
        string lName = Page.User.Identity.Name.ToString();
        string yr = DateTime.Now.Year.ToString();
        DateTime countForYear = Convert.ToDateTime("01/01/" + yr + " 00:00:00");

        SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (max(OrderSl),0)+ 1 )) from Orders where EntryDate>=@EntryDate", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnAdd.Text == "Add")
            {
                string lName = Page.User.Identity.Name.ToString();
                if (txtQty.Text == "")
                {
                    txtQty.Text = "1";
                }
                //SizeId, ProductID, BrandID
                SqlCommand cmde = new SqlCommand("SELECT ProductName FROM OrderDetails WHERE SizeId ='" + ddSize.SelectedValue + "' AND ProductID ='" + ddProduct.SelectedValue + "' AND BrandID ='" + ddBrand.SelectedValue + "' AND  OrderID =''  AND CustomerID='" + ddCustomer.SelectedValue + "'  AND EntryBy ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
                    lblMsg2.Attributes.Add("class", "xerp_warning");
                    lblMsg2.Text = "ERROR: Product Already exist or product selection box is empty!";
                    Notify("ERROR: Product Already exist or product selection box is empty!", "error", lblMsg);  
                }
            }
            else
            {
                ExecuteUpdate();
                btnAdd.Text = "Add";
                lblMsg2.Attributes.Add("class", "xerp_success");
                lblMsg2.Text = "Item updated successfully";
                Notify("Item updated successfully", "success", lblMsg);  
            }
            //ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);

        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
            Notify(ex.Message.ToString(), "error", lblMsg); 
        }
        finally
        {

            BindItemGrid();
        }
    }

    private string DataChk4Name(string val)
    {
        string val1 = val.ToLower().Trim();
        if (val1.IndexOf("n/a") > (-1) || val1.IndexOf("---") > (-1) || val1 == "all" || val1 == "n/a")
        {
            val1 = "";
        }
        else
        {
            val1 = val;
        }
        return val1;
    }


    private void InsertData()
    {
        string lName = Page.User.Identity.Name.ToString();
        string productName = DataChk4Name(ddSize.SelectedItem.Text) + " " + DataChk4Name(ddBrand.SelectedItem.Text) + " " + DataChk4Name(ddProduct.SelectedItem.Text);

        if (chkMerge.Checked == true)
        {
            productName = DataChk4Name(ddSize.SelectedItem.Text) + " " + DataChk4Name(ddBrand.SelectedItem.Text) + " " + DataChk4Name(ddGrade.SelectedItem.Text) + " " + DataChk4Name(ddProduct.SelectedItem.Text);
        }

        decimal weight = 0;
        if (txtWeight.Text != "")
        {
            weight = Convert.ToDecimal(txtWeight.Text);
        }

        SqlCommand cmd2 = new SqlCommand("INSERT INTO OrderDetails (OrderID, SizeId, ProductID, BrandID, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy,  CustomerID) VALUES (@OrderID, @SizeId, @ProductID, @BrandID, @ProductName, @UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, '" + ddCustomer.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@OrderID", "");
        cmd2.Parameters.AddWithValue("@SizeId", ddSize.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductID", ddProduct.SelectedValue);
        cmd2.Parameters.AddWithValue("@BrandID", ddBrand.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductName", productName.Trim());

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
        string productName = DataChk4Name(ddSize.SelectedItem.Text) + " " + DataChk4Name(ddBrand.SelectedItem.Text) + " " + DataChk4Name(ddProduct.SelectedItem.Text);

        if (chkMerge.Checked == true)
        {
            productName = DataChk4Name(ddSize.SelectedItem.Text) + " " + DataChk4Name(ddBrand.SelectedItem.Text) + " " + DataChk4Name(ddGrade.SelectedItem.Text) + " " + DataChk4Name(ddProduct.SelectedItem.Text);
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

    private void BindItemGrid()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, TotalWeight, UnitType, ItemTotal FROM OrderDetails WHERE EntryBy=@EntryBy AND CustomerID='" + ddCustomer.SelectedValue + "' AND OrderID='' ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //cmd.CommandText = "SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal FROM OrderDetails WHERE EntryBy=@EntryBy AND OrderID='' ORDER BY Id";

        cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        cmd.Connection.Open();
        ItemGrid.EmptyDataText = "No items to view...";
        ItemGrid.DataSource = cmd.ExecuteReader();
        ItemGrid.DataBind();
        cmd.Connection.Close();

        LoadSummesion("");
    }

    private void LoadSummesion(string OrderId)
    {
        string lName = Page.User.Identity.Name.ToString();
        ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Quantity),0) from OrderDetails where EntryBy='" + lName + "' AND  OrderID='" + OrderId + "' AND CustomerID='" + ddCustomer.SelectedValue + "' ");
        txtTotal.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(ItemTotal),0) from OrderDetails where EntryBy='" + lName + "' AND  OrderID='" + OrderId + "' AND CustomerID='" + ddCustomer.SelectedValue + "' ");
        string weight= RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(TotalWeight),0) from OrderDetails where  EntryBy='" + lName + "' AND  OrderID='" + OrderId + "' AND CustomerID='" + ddCustomer.SelectedValue + "' ");
        ltrWeight.Text = Convert.ToString(Convert.ToDecimal(weight));///1000M);

        if (txtVat.Text == "")
        {
            txtVat.Text = "0";
        }
        if (txtDiscount.Text == "")
        {
            txtDiscount.Text = "0";
        }

        txtPay.Text = Convert.ToString(Convert.ToDecimal(txtTotal.Text) - Convert.ToDecimal(txtDiscount.Text) + (Convert.ToDecimal(txtTotal.Text) * Convert.ToDecimal(txtVat.Text) / 100M));
    }


    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Quantity),0) from OrderDetails where OrderID=''");
        //ltrWeight.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(TotalWeight),0) from OrderDetails where OrderID=''");

        //string OldEntryExist = RunQuery.SQLQuery.ReturnString("Select BrandID FROM OrderDetails Where OrderID=''");
        //if (OldEntryExist != "")
        //{
        //    string cusID = RunQuery.SQLQuery.ReturnString("Select CustomerID FROM CustomerBrands Where BrandID='" + OldEntryExist + "'");
        //    ddCustomer.SelectedValue = cusID;
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
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;
            TextBox txtQty = ItemGrid.Rows[index].FindControl("txtQty") as TextBox;

            string OrderID = RunQuery.SQLQuery.ReturnString("Select OrderID from OrderDetails where Id='" + lblItemCode.Text + "'");
            string deliveredQty = RunQuery.SQLQuery.ReturnString("Select SUM(DeliveredQty) from OrderDetails where OrderID='" + OrderID + "'");

            if (deliveredQty == "0")
            {
                SqlCommand cmd7 = new SqlCommand("DELETE OrderDetails WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                cmd7.ExecuteNonQuery();
                cmd7.Connection.Close();

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
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.ToString();
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
            if (Convert.ToDateTime(txtDeliveryDate.Text) >= Convert.ToDateTime(txtDate.Text))
            {
                if (txtInv.Text != "")
                {
                    if (ltrQty.Text == "0")
                    {
                        lblMsg.Attributes.Add("class", "xerp_error");
                        lblMsg.Text = "Failed to save...<br> No products were added to the sales order!";
                    }
                    else
                    {
                        string isExist = RunQuery.SQLQuery.ReturnString("SELECT OrderID FROM Orders WHERE CustomerName ='" + ddCustomer.SelectedValue + "' AND OrderID ='" + txtInv.Text + "'");
                        if (isExist == "")
                        {
                            ExecuteInsert();

                            txtInv.Text = "";
                            txtDeliveryDate.Text = "";
                            txtAddress.Text = "";
                            txtVat.Text = "0";
                            txtDiscount.Text = "0";
                            //ClearControls(Page);

                            lblMsg.Attributes.Add("class", "xerp_success");
                            lblMsg.Text = "New sales order saved successfully...";
                            Notify("New sales order saved successfully...", "success", lblMsg);  
                        }
                        else
                        {
                            lblMsg.Attributes.Add("class", "xerp_error");
                            lblMsg.Text = "ERROR: PO# already exist for the customer!";
                            Notify( "ERROR: PO# already exist for the customer!", "warn", lblMsg); 
                        }
                    }
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Invalid PO#. Please input Customer Order No.";
                    Notify("Invalid PO#. Please input Customer Order No." , "warn", lblMsg); 
                }
            }
            else
            {
                txtDeliveryDate.Focus();
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Please input delivery date properly.";
                Notify("Please input delivery date properly.", "warn", lblMsg); 
            }
        }
        catch (Exception ex)
        {
            txtDeliveryDate.Focus();
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
            Notify(ex.Message.ToString(), "error", lblMsg); 
        }
        finally
        {
            txtDeliveryDate.Focus();
            BindItemGrid();
            ddOrders.DataBind();
        }
    }

    private void ExecuteInsert()
    {
        string orderNo = txtInv.Text; //InvIDNo();
        string lName = Page.User.Identity.Name.ToString();
        string totalAmt = RunQuery.SQLQuery.ReturnString("Select SUM(ItemTotal) from OrderDetails where OrderID=''");

        SqlCommand cmd2 = new SqlCommand("INSERT INTO Orders (OrderID, OrderDate, DeliveryDate, CustomerName, DeliveryAddress, TotalAmount, Discount, Vat, PayableAmount, EntryBy, ProjectId)" +
                                                    " VALUES (@OrderID, @OrderDate, @DeliveryDate, @CustomerName, @DeliveryAddress, @TotalAmount, @Discount, @Vat, @PayableAmount, @EntryBy, @ProjectId)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@OrderID", orderNo); //ddCategory.SelectedValue);
        cmd2.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(txtDate.Text));
        cmd2.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(txtDeliveryDate.Text));
        cmd2.Parameters.AddWithValue("@CustomerName", ddCustomer.SelectedValue);
        cmd2.Parameters.AddWithValue("@DeliveryAddress", txtAddress.Text);

        cmd2.Parameters.AddWithValue("@TotalAmount", Convert.ToDecimal(txtTotal.Text));
        cmd2.Parameters.AddWithValue("@Discount", Convert.ToDecimal(txtDiscount.Text));
        cmd2.Parameters.AddWithValue("@Vat", Convert.ToDecimal(txtVat.Text));
        cmd2.Parameters.AddWithValue("@PayableAmount", Convert.ToDecimal(txtTotal.Text) + (Convert.ToDecimal(txtTotal.Text) * Convert.ToDecimal(txtVat.Text) / 100M));
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@ProjectId", lblProject.Text);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        SqlCommand cmd = new SqlCommand("UPDATE OrderDetails SET OrderID='" + orderNo + "' WHERE EntryBy=@EntryBy AND OrderID='' AND CustomerID='" + ddCustomer.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryBy", lName);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        RunQuery.SQLQuery.ExecNonQry("UPDATE ImportentDocuments SET BusNo='" + orderNo + "' WHERE EntryBy='" + lName + "' AND BusNo='-'");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("Delete OrderDetails WHERE EntryBy=@EntryBy AND OrderID=''  AND CustomerID='" + ddCustomer.SelectedValue + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryBy", lName);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        ItemGrid.DataSource = null;
        ItemGrid.DataBind();
        ddCustomer.Enabled = true;
        //Response.Redirect("./Order-Entry.aspx");
    }

    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand.DataBind();
        BindItemGrid();
        ddCustomer.Focus();
    }

    protected void ddSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddProduct.DataBind();
        QtyinStock();
        ddSize.Focus();
    }
    protected void ddBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddProduct.DataBind();
        QtyinStock();
        ddBrand.Focus();
    }
    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        txtQty.Focus();
        ddProduct.Focus();
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
        txtWeight.Text = RunQuery.SQLQuery.ReturnString("Select UnitWeight from SaleDetails  where Id=(Select ISNULL(MAX(Id),0) from SaleDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");
        txtRate.Text = RunQuery.SQLQuery.ReturnString("Select UnitCost from SaleDetails  where Id=(Select ISNULL(MAX(Id),0) from SaleDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");

        if (txtWeight.Text == "")
        {
            txtWeight.Text = RunQuery.SQLQuery.ReturnString("Select UnitWeight from OrderDetails  where Id=(Select ISNULL(MAX(Id),0) from OrderDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");
            if (txtWeight.Text == "")
            {
                //txtWeight.Text = "0";
            }
        }
        if (txtRate.Text == "")
        {
            txtRate.Text = RunQuery.SQLQuery.ReturnString("Select UnitCost from OrderDetails  where Id=(Select ISNULL(MAX(Id),0) from OrderDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");
            if (txtRate.Text == "")
            {
                //txtRate.Text = "0";
            }
        }
        if (txtQty.Text != "" && txtRate.Text != "")
        {
            txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtRate.Text));
        }
    }
    private void TrackCustomer()
    {

    }
    protected void imgUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        lblMsg.Attributes.Add("class", "xerp_error");
        lblMsg.Text = "File Uploaded";
        Notify("File Uploaded", "info", lblMsg);  
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
            Notify("Edit mode activated ...", "warn", lblMsg); 
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
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

            ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + productID + "'");

            string brand = dr[2].ToString();
            ddCustomer.SelectedValue = RunQuery.SQLQuery.ReturnString("Select CustomerID FROM CustomerBrands where BrandID='" + brand + "'");
            ddBrand.DataBind();
            ddBrand.SelectedValue = brand;
            txtRate.Text = dr[3].ToString();
            txtWeight.Text = dr[4].ToString();
            txtQty.Text = dr[5].ToString();

            //txtWeight.Text = dr[6].ToString();

            txtStock.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where SizeID='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + productID + "'");
            txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtRate.Text));
        }
        cmd.Connection.Close();

    }
}