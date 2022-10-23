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
using RunQuery;


public partial class Sales_Return : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
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
            ddProduct.DataBind();
            getproducts();
            LoadReturnHistory();
        }

    }

    //Messagebox For Alerts    
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
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
    
    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
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
            if (ddProduct.SelectedValue != "" && txtQty.Text!="")
            {
                InsertData();
                ItemGrid.DataBind();
                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Return info successfully saved.";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Error: Item Qty cannot be empty!";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error: " + ex.ToString();
        }
        finally
        {            
            ddOrders.DataBind();
        }
    }

    private void InsertData()
    {
        string lName = Page.User.Identity.Name.ToString();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO SaleReturn (CustomerName, CustomerID, InvoiceNo, ReturnDate, WastageStock, WastageStockID, ReplacementStock, ReplacementStockID, Remark, SalesDetailID, ItemName, Qty, Price, Total, EntryBy) VALUES (@CustomerName, @CustomerID, @InvoiceNo, @ReturnDate, @WastageStock, @WastageStockID, @ReplacementStock, @ReplacementStockID, @Remark, @SalesDetailID, @ItemName, @Qty, @Price, @Total, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@CustomerName", ddCustomer.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@CustomerID", ddCustomer.SelectedValue);
        cmd2.Parameters.AddWithValue("@InvoiceNo", ddOrders.SelectedValue);
        cmd2.Parameters.AddWithValue("@ReturnDate", Convert.ToDateTime(txtReturnDate.Text).ToString("yyyy-MM-dd"));
        cmd2.Parameters.AddWithValue("@WastageStock", ddWasteStock.SelectedItem.Text);

        cmd2.Parameters.AddWithValue("@WastageStockID", ddWasteStock.SelectedValue);
        cmd2.Parameters.AddWithValue("@ReplacementStock", ddReplaceStock.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@ReplacementStockID", ddReplaceStock.SelectedValue);
        cmd2.Parameters.AddWithValue("@Remark", txtRemarks.Text);
        cmd2.Parameters.AddWithValue("@SalesDetailID", ddProduct.SelectedValue);

        cmd2.Parameters.AddWithValue("@ItemName", ddProduct.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@Qty", Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@Price", 0);
        cmd2.Parameters.AddWithValue("@Total", 0);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        RunQuery.SQLQuery.ExecNonQry("Update SaleDetails Set ReturnQty=" + Convert.ToDecimal(txtQty.Text) + " Where id=" + ddProduct.SelectedValue);
        
        //Stock-out
        string retNo= RunQuery.SQLQuery.ReturnString("Select ISNULL(MAX(eid),0) from SaleReturn");
        string sizeID = RunQuery.SQLQuery.ReturnString("Select SizeId from SaleDetails Where Id=" + ddProduct.SelectedValue);
        string BrandID = RunQuery.SQLQuery.ReturnString("Select BrandID from SaleDetails Where Id=" + ddProduct.SelectedValue);
        string ProductID = RunQuery.SQLQuery.ReturnString("Select SizeId from SaleDetails Where Id=" + ddProduct.SelectedValue);

        Accounting.VoucherEntry.StockEntry(ddOrders.SelectedValue, "Sale Return", retNo, sizeID, BrandID, ProductID, ddProduct.SelectedItem.Text, ddReplaceStock.SelectedValue, "", "2", "0", txtQty.Text, "0", "0", "", "Sales Return", "Finished", "", lName);
        //Sock-in
        Accounting.VoucherEntry.StockEntry(ddOrders.SelectedValue, "Wastage Stock-in", retNo, sizeID, BrandID, ProductID, ddProduct.SelectedItem.Text, ddWasteStock.SelectedValue, "", "2", txtQty.Text, "0", "0", "0", "", "Sales Return", "Finished", "", lName);
    }


    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddOrders.DataBind();
        ddProduct.DataBind();
        LoadReturnHistory();
    }
    protected void ddOrders_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddProduct.DataBind();
        LoadReturnHistory();
    }

    private void LoadReturnHistory()
    {
        txtReturnDate.Text = Convert.ToDateTime(RunQuery.SQLQuery.ReturnString("Select DeliveryDate from Sales where InvNo='" + ddOrders.SelectedValue + "'")).ToShortDateString();
        ddProduct.DataBind();
        ItemGrid.DataBind();
        txtQty.Text = SQLQuery.ReturnString("Select Quantity from SaleDetails where Id='" + ddProduct.SelectedValue + "' ");
    }

    protected void ddProduct_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtQty.Text = SQLQuery.ReturnString("Select Quantity from SaleDetails where Id='" + ddProduct.SelectedValue + "' ");
    }
}