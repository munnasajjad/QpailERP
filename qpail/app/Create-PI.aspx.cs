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


public partial class app_Create_PI : System.Web.UI.Page
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

            ddCustomer.DataBind();
            ddOrders.DataBind();
            BindItemGrid();
            //LoadSummesion("");

            ddBrand.DataBind();
            ddGrade.DataBind();
            ddCategory.DataBind();
            ddProduct.DataBind();
            ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");
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

    private void LoadSummesion(string OrderId)
    {
        //string OrderID = RunQuery.SQLQuery.ReturnString("Select OrderID from OrderDetails where Id='" + lblItemCode.Text + "'");
        ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Quantity),0) from OrderDetails where OrderID='" + OrderId + "'");
        txtTotal.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(ItemTotal),0) from OrderDetails where OrderID='" + OrderId + "'");

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
                SqlCommand cmde = new SqlCommand("SELECT ProductName FROM OrderDetails WHERE SizeId ='" + ddSize.SelectedValue + "' AND ProductID ='" + ddProduct.SelectedValue + "' AND BrandID ='" + ddBrand.SelectedValue + "' AND  OrderID ='' AND EntryBy ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmde.Connection.Open();
                string isExist
                    = Convert.ToString(cmde.ExecuteScalar());
                cmde.Connection.Close();

                if (isExist == "" && ddProduct.SelectedValue != "")
                {
                    InsertData();
                }
                else
                {
                    lblMsg2.Attributes.Add("class", "xerp_warning");
                    lblMsg2.Text = "ERROR: Product Already exist or product selection box is empty!";
                }
            }
            else
            {
                ExecuteUpdate();
                btnAdd.Text = "Add";
                pnlAdd.Visible = false;
                lblMsg2.Attributes.Add("class", "xerp_success");
                lblMsg2.Text = "Item updated successfully";
            }
            //ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);

        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
        finally
        {
            BindItemGrid();
        }
    }
    private void InsertData()
    {
        string lName = Page.User.Identity.Name.ToString();
        string productName = ddSize.SelectedItem.Text + " " + ddBrand.SelectedItem.Text + " " + ddProduct.SelectedItem.Text;

        if (chkMerge.Checked == true)
        {
            productName = ddSize.SelectedItem.Text + " " + ddBrand.SelectedItem.Text + " " + ddGrade.SelectedItem.Text + " " + ddProduct.SelectedItem.Text;
        }

        decimal weight = 0;
        if (txtWeight.Text != "")
        {
            weight = Convert.ToDecimal(txtWeight.Text);
        }

        SqlCommand cmd2 = new SqlCommand("INSERT INTO OrderDetails (OrderID, SizeId, ProductID, BrandID, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy) VALUES (@OrderID, @SizeId, @ProductID, @BrandID, @ProductName, @UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@OrderID", "");
        cmd2.Parameters.AddWithValue("@SizeId", ddSize.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductID", ddProduct.SelectedValue);
        cmd2.Parameters.AddWithValue("@BrandID", ddBrand.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductName", productName);

        cmd2.Parameters.AddWithValue("@UnitCost", Convert.ToDecimal(txtRate.Text));
        cmd2.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@UnitWeight", weight);

        cmd2.Parameters.AddWithValue("@ItemTotal", Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@TotalWeight", weight * Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        string lName = Page.User.Identity.Name.ToString();
        string productName = ddSize.SelectedItem.Text + " " + ddBrand.SelectedItem.Text + " " + ddProduct.SelectedItem.Text;

        if (chkMerge.Checked == true)
        {
            productName = ddSize.SelectedItem.Text + " " + ddBrand.SelectedItem.Text + " " + ddGrade.SelectedItem.Text + " " + ddProduct.SelectedItem.Text;
        }

        SqlCommand cmd2 = new SqlCommand("UPDATE OrderDetails SET SizeId='" + ddSize.SelectedValue + "', ProductID='" + ddProduct.SelectedValue + "', BrandID='" + ddBrand.SelectedValue + "'," +
                                "ProductName=@ProductName, UnitCost=@UnitCost, UnitWeight=@UnitWeight, Quantity=@Quantity, " +
                                "ItemTotal=@ItemTotal, TotalWeight=@TotalWeight  where (id ='" + lblOrderID.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductName", productName);
        cmd2.Parameters.AddWithValue("@UnitCost", Convert.ToDecimal(txtRate.Text));
        cmd2.Parameters.AddWithValue("@UnitWeight",txtWeight.Text);
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
        string OrderID = ddOrders.SelectedValue;

        
        if (ddOrders.SelectedValue != "")
        {
        SqlCommand cmd = new SqlCommand("SELECT Id, ProductName, UnitCost, Quantity, UnitWeight,TotalWeight, UnitType, ItemTotal FROM OrderDetails WHERE OrderID='" + OrderID + "' ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
            txtPODate.Text = Convert.ToDateTime(dr[0].ToString()).ToShortDateString();
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
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
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
            if (Convert.ToDateTime(txtDeliveryDate.Text) >= Convert.ToDateTime(txtPODate.Text))
            {
                if (txtInv.Text!="")
                {
                    if (ltrQty.Text == "0")
                    {
                        lblMsg.Attributes.Add("class", "xerp_error");
                        lblMsg.Text = "Failed to save...<br> No products were added to the sales order!";
                    }
                    else
                    {
                        string isExist= RunQuery.SQLQuery.ReturnString("Select OrderID from orders where OrderID='" + txtInv.Text + "' ");
                        if (isExist == "")
                        {
                            ExecuteInsert();
                            txtVat.Text = "0";
                            txtDiscount.Text = "0";
                            txtInv.Text = "";
                            txtDate.Text = "";
                            txtDeliveryDate.Text = "";
                            txtAddress.Text = "";
                            txtTerms.Text = "";
                            

                            ddCustomer.DataBind();
                            ddOrders.DataBind();
                            BindItemGrid();
                            //LoadSummesion("");

                            ddBrand.DataBind();
                            ddGrade.DataBind();
                            ddCategory.DataBind();
                            ddProduct.DataBind();

                            //ClearControls(Page);
                            lblMsg.Attributes.Add("class", "xerp_success");
                            lblMsg.Text = "New proforma invoice saved successfully...";
                        }
                        else
                        {
                            lblMsg.Attributes.Add("class", "xerp_error");
                            lblMsg.Text = "PI# already exist!";
                        }
                    }
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Invalid PI Number... Please input Customer PI No. properly.";
                }
            }
            else
            {
                txtDeliveryDate.Focus();
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Please input delivery date properly.";
            }
        }
        catch (Exception ex)
        {
            txtDeliveryDate.Focus();
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Input delivery date. " + ex.Message.ToString();
        }
        finally
        {
            txtDeliveryDate.Focus();
            BindItemGrid();
            //ddOrders.DataBind();
        }
    }

    private void ExecuteInsert()
    {
        string lName = Page.User.Identity.Name.ToString();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO PI (OrderID, PiNo, PiDate, LcNo, LcDate, LcExpDate, IssuingBank, IssueingBankBranch, AdvisingBankAccount,PiTerms, EntryBy, ProjectId)" +
                                                    " VALUES (@OrderID, @PiNo, @PiDate, @LcNo, @LcDate, @LcExpDate, @IssuingBank, @IssueingBankBranch, @AdvisingBankAccount,@PiTerms, @EntryBy, @ProjectId)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Parameters.AddWithValue("@OrderID", ddOrders.SelectedValue);
        cmd2.Parameters.AddWithValue("@PiNo", txtInv.Text); //ddCategory.SelectedValue);
        cmd2.Parameters.AddWithValue("@PiDate", Convert.ToDateTime(txtDate.Text));
        cmd2.Parameters.AddWithValue("@LcNo", "");
        cmd2.Parameters.AddWithValue("@LcDate", string.Empty);
        cmd2.Parameters.AddWithValue("@LcExpDate", string.Empty);

        cmd2.Parameters.AddWithValue("@IssuingBank", string.Empty);
        cmd2.Parameters.AddWithValue("@IssueingBankBranch", string.Empty);
        cmd2.Parameters.AddWithValue("@AdvisingBankAccount", string.Empty);
        cmd2.Parameters.AddWithValue("@PiTerms", txtTerms.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@ProjectId", lblProject.Text);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        SqlCommand cmd = new SqlCommand("UPDATE OrderDetails SET OrderID='" + txtInv.Text + "'  WHERE OrderID='" + ddOrders.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        RunQuery.SQLQuery.ExecNonQry("UPDATE Orders SET OrderType='PI', PoNo='" + ddOrders.SelectedValue + "', OrderID='" + txtInv.Text + "',  TotalAmount='" + txtTotal.Text + "', Vat='" + txtVat.Text + "', PayableAmount='" + txtPay.Text + "', DeliveryAddress='" + txtAddress.Text + "', DeliveryDate='" + Convert.ToDateTime(txtDeliveryDate.Text).ToString("yyyy-MM-dd") + "'   WHERE OrderID='" + ddOrders.SelectedValue + "'");
        RunQuery.SQLQuery.ExecNonQry("UPDATE PO_Images SET PONo='" + txtInv.Text + "', ImgDescription='Proforma Invoices' WHERE EntryBy='" + lName + "' AND (PONo='' OR PONo='" + ddOrders.SelectedValue + "')");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("Delete OrderDetails WHERE EntryBy=@EntryBy AND OrderID=''", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryBy", lName);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();
        Response.Redirect("./Order-Entry.aspx");
    }

    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand.DataBind();
        ddOrders.DataBind();
        BindItemGrid();
        ddOrders.Focus();
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
    }

    private void QtyinStock()
    {
        ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");
        txtStock.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where SizeID='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "'");
        txtWeight.Text = RunQuery.SQLQuery.ReturnString("Select UnitWeight from SaleDetails  where Id=(Select ISNULL(MAX(Id),0) from SaleDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");
        txtRate.Text = RunQuery.SQLQuery.ReturnString("Select UnitCost from SaleDetails  where Id=(Select ISNULL(MAX(Id),0) from SaleDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");

        if(txtWeight.Text=="")
        {
            txtWeight.Text = RunQuery.SQLQuery.ReturnString("Select UnitWeight from OrderDetails  where Id=(Select ISNULL(MAX(Id),0) from OrderDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");
            if (txtWeight.Text == "")
            {
                txtWeight.Text = "0";
            }
        }
        if (txtRate.Text == "")
        {
            txtRate.Text = RunQuery.SQLQuery.ReturnString("Select UnitCost from OrderDetails  where Id=(Select ISNULL(MAX(Id),0) from OrderDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");
            if (txtRate.Text == "")
            {
                txtRate.Text = "0";
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
            pnlAdd.Visible = true;

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
            ddCategory.SelectedValue = catID;
            ddProduct.DataBind();
            ddProduct.SelectedValue = productID;

            string brand = dr[2].ToString();
            ddCustomer.SelectedValue = RunQuery.SQLQuery.ReturnString("Select CustomerID FROM CustomerBrands where BrandID='" + brand + "'");
            ddBrand.DataBind();
            ddBrand.SelectedValue = brand;
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
    protected void ddOrders_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindItemGrid();
        txtInv.Focus();
    }

    protected void ddCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
}