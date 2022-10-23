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

public partial class app_Sales_Xclusive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDate.Text = DateTime.Now.ToShortDateString();

            BindItemGrid();
            //LoadSummesion("");
                       
            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            ddProduct.DataBind();
            ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");
        }
        //txtInv.Text = InvIDNo();
        getproducts();
    }

    //Messagebox For Alerts    
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    private void LoadSummesion(string OrderId)
    {
        //string OrderID = RunQuery.SQLQuery.ReturnString("Select OrderID from OrderDetails where Id='" + lblItemCode.Text + "'");
        ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Quantity),0) from OrderDetails where OrderID='" + OrderId + "'");
        txtTotal.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(ItemTotal),0) from OrderDetails where OrderID='" + OrderId + "'");
        if(txtVat.Text=="")
        {
            txtVat.Text = "0";
        }
        if (txtDiscount.Text == "")
        {
            txtDiscount.Text = "0";
        }
        
        txtPay.Text = Convert.ToString(Convert.ToDecimal(txtTotal.Text) - Convert.ToDecimal(txtDiscount.Text) + (Convert.ToDecimal(txtTotal.Text) * Convert.ToDecimal(txtVat.Text) / 100M));
    }
    
    public void getproducts()
    {
        SqlCommand cmd2y = new SqlCommand("SELECT ItemCode, ItemCode + ' -- ' + name + ' -- ' + CONVERT (nvarchar(50), price) AS Item FROM Items ORDER BY name", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2y.Connection.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd2y);
        DataSet ds = new DataSet("Board");
        da.Fill(ds, "Board");
        cmd2y.Connection.Close();

        DataTable citydt = ds.Tables["Board"];
        //citydt = citydt.Rows.Cast<DataRow>().Skip(int.Parse(i) - 1).Take(1).CopyToDataTable();

        string icode = "";
        string iName = "";

        StringBuilder sb = new StringBuilder();

        sb.Append("<Scr" + "ipt> \n");
        sb.Append("$(function(){  var currencies = [ \n");

        foreach (DataRow citydr in citydt.Rows)
        {
            icode = citydr["ItemCode"].ToString();
            iName = citydr["Item"].ToString();
            string item = "{ value: '" + iName + "', data: '" + icode + "' },\n";
            sb.Append(item);
        }

        sb.Append("];");
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
        DateTime countForYear = Convert.ToDateTime("01/01/"+yr+" 00:00:00");

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

    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");
        txtQty.Focus();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
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

        if (isExist == "")
        {
            string productName = ddSize.SelectedItem.Text + " " + ddBrand.SelectedItem.Text + " " + ddProduct.SelectedItem.Text;
            SqlCommand cmd2 = new SqlCommand("INSERT INTO OrderDetails (OrderID, SizeId, ProductID, BrandID, ProductName, UnitCost, Quantity, UnitType, ItemTotal, EntryBy) VALUES (@OrderID, @SizeId, @ProductID, @BrandID, @ProductName, @UnitCost, @Quantity, '" + ltrUnit.Text + "', @ItemTotal, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.AddWithValue("@OrderID", "");
            cmd2.Parameters.AddWithValue("@SizeId", ddSize.SelectedValue);
            cmd2.Parameters.AddWithValue("@ProductID", ddProduct.SelectedValue);
            cmd2.Parameters.AddWithValue("@BrandID", ddBrand.SelectedValue);
            cmd2.Parameters.AddWithValue("@ProductName", productName);

            cmd2.Parameters.AddWithValue("@UnitCost", Convert.ToDecimal(txtRate.Text));
            cmd2.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(txtQty.Text));
            cmd2.Parameters.AddWithValue("@ItemTotal", Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQty.Text));
            cmd2.Parameters.AddWithValue("@EntryBy", lName);

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();

            BindItemGrid();
        }
        else
        {
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "ERROR: Item Already exist! Delete from grid first...";
        }

        //ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
    }

    private void BindItemGrid()
    {
        string lName = Page.User.Identity.Name.ToString();
        String strConnString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "GetSalesGridTempBySID";
        //cmd.CommandText = "SELECT Id, ProductName, UnitCost, Quantity, UnitType, ItemTotal FROM OrderDetails WHERE EntryBy=@EntryBy AND OrderID='' ORDER BY Id";

        cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        cmd.Connection = con;

        con.Open();
        ItemGrid.EmptyDataText = "No items to view...";
        ItemGrid.DataSource = cmd.ExecuteReader();
        ItemGrid.DataBind();

        LoadSummesion("");
    }


    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void ItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;            
            TextBox txtQty = ItemGrid.Rows[index].FindControl("txtQty") as TextBox;

            string OrderID=RunQuery.SQLQuery.ReturnString("Select OrderID from OrderDetails where Id='"+ lblItemCode.Text +"'");
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
            if (txtDeliveryDate.Text != "")
            {
                if (btnSave.Text == "Save")
                {
                        ExecuteInsert();                        
                        //ClearControls(Page);
                        
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "New sales order saved successfully...";
                    
                }
                else
                {
                    ExecuteUpdate();
                    ddOrders.DataBind();
                    btnSave.Text = "Save";
                    //EditField.Attributes.Add("class", "form-group hidden");

                    lblMsg.Attributes.Add("class", "xerp_success");
                    //lblMsg.Text = "Info successfully updated for " + DropDownList1.SelectedItem.Text;
                }
            }
            else
            {
                txtDeliveryDate.Focus();
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Please input delivery date";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Erro: " + ex.ToString();
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
        string orderNo = InvIDNo();        
        string lName = Page.User.Identity.Name.ToString();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO Orders (OrderID, OrderDate, DeliveryDate, CustomerName, DeliveryAddress, TotalAmount, Discount, Vat, PayableAmount, EntryBy, ProjectId)"+
                                                    " VALUES (@OrderID, @OrderDate, @DeliveryDate, @CustomerName, @DeliveryAddress, @TotalAmount, @Discount, @Vat, @PayableAmount, @EntryBy, @ProjectId)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        
        cmd2.Parameters.AddWithValue("@OrderID", orderNo); //ddCategory.SelectedValue);
        cmd2.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(txtDate.Text));
        cmd2.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(txtDeliveryDate.Text));
        cmd2.Parameters.AddWithValue("@CustomerName", ddCustomer.SelectedValue);
        cmd2.Parameters.AddWithValue("@DeliveryAddress", txtAddress.Text);

        cmd2.Parameters.AddWithValue("@TotalAmount", Convert.ToDecimal(txtTotal.Text));
        cmd2.Parameters.AddWithValue("@Discount", Convert.ToDecimal(txtDiscount.Text));
        cmd2.Parameters.AddWithValue("@Vat", Convert.ToDecimal(txtVat.Text));
        cmd2.Parameters.AddWithValue("@PayableAmount", Convert.ToDecimal(txtPay.Text));
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@ProjectId", lblProject.Text);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        SqlCommand cmd = new SqlCommand("UPDATE OrderDetails SET OrderID='" + orderNo + "' WHERE EntryBy=@EntryBy AND OrderID=''", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryBy", lName);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();
    }

    private void ExecuteUpdate()
    {
        //SqlCommand cmd2 = new SqlCommand("UPDATE Banks SET BankName='" + txtName.Text + "', Description='" + txtAddress.Text + "', Phone='" + txtPhone.Text + "'," +
        //                        "ContactPerson=@ContactPerson, MobileNo=@MobileNo, url=@url, Email='" + txtEmail.Text + "' where (BankId ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //cmd2.Parameters.AddWithValue("@ContactPerson", txtContact.Text);
        //cmd2.Parameters.AddWithValue("@MobileNo", txtMobile.Text);
        //cmd2.Parameters.AddWithValue("@url", txtUrl.Text);

        //cmd2.Connection.Open();
        //cmd2.ExecuteNonQuery();
        //cmd2.Connection.Close();
        //cmd2.Connection.Dispose();

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
        Response.Redirect("./Sales-Xclusive.aspx?type=order");
    }
    
    protected void lbOrders_SelectedIndexChanged(object sender, EventArgs e)
    {
        string orders = "";
        int[] indexes = this.lbOrders.GetSelectedIndices();
        for (int index = 0; index < indexes.Length; index++)
        {
            orders += this.lbOrders.Items[indexes[index]].Text + ",";
        }
        txtInv.Text = orders.TrimEnd(',');
        LoadOrderDetails(orders);
    }

    private void LoadOrderDetails(string orders)
    {
        DataTable gridDt = new DataTable();

        gridDt.Columns.Add("Id");
        gridDt.Columns.Add("Name");
        DataRow dtrow2 = null;
        dtrow2 = gridDt.NewRow();
        dtrow2["Id"] = 12;
        dtrow2["Name"] = "Nush";
        gridDt.Rows.Add(dtrow2);

        string[] allOrders = orders.Split(',');
        foreach (string orderId in allOrders)
        {
            DataTable dt1 = new DataTable();
         
            dt1.Columns.Add("Id");
            dt1.Columns.Add("Name");
            DataRow dtrow1 = null;
            dtrow1 = dt1.NewRow();
            dtrow1["Id"] = 12;
            dtrow1["Name"] = "Mehran";
            dt1.Rows.Add(dtrow1);
            
            gridDt.Merge(dt1);
        }
    }

}