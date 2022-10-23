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


public partial class app_Purchase_Return : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtReturnDate.Text = DateTime.Now.ToShortDateString();

            ddMode.DataBind();
            //ddSuppCategory.DataBind();
            ddVendor.DataBind();
            LoadInvoices();
            ReturnableQty();
            //ltrBillNo.Text = "Party LC/TT/Invoice No.: " + RunQuery.SQLQuery.ReturnString("Select BillNo FROM Purchase where InvNo='" + ddOrders.SelectedValue + "'");
            //string orderNo = base.Request.QueryString["ID"];
            //ddOrders.SelectedValue = orderNo;

            LoadReturnHistory();
            ltrBillNo.Text = "Party LC/TT/Invoice No.: " + SQLQuery.ReturnString("Select BillNo FROM Purchase where InvNo='" + ddOrders.SelectedValue + "'");
            LoadProducts();
        }
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
        SqlCommand cmd3 = new SqlCommand("SELECT ISNULL(MAX(ItemSl),0) FROM SalesGridTemp where (sid=" + sid + ")",
            new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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

    private decimal RawQtyinStock(string iCode, string invNo, string godown, string locationId)
    {
        try
        {
            decimal qty = 0;
            string subGrp = Inventory.GetItemSubGroup(iCode);

            if (subGrp == "10") //printing ink
            {
                string spec = SQLQuery.ReturnString("Select SizeRef from PurchaseDetails where InvNo='" + invNo + "' AND ItemCode='" + iCode + "'");
                qty = Convert.ToDecimal(Inventory.AvailableInkWeight(iCode, spec, godown, locationId));
            }
            else if (subGrp != "9") //Not Tin Plate
            {
                string purpose = SQLQuery.ReturnString("Select Purpose from PurchaseDetails where InvNo='" + invNo + "' AND ItemCode='" + iCode + "'");
                qty = Convert.ToDecimal(Inventory.PlasticRawWeight(purpose, iCode, godown, locationId));
            }
            else
            {
                string purpose = SQLQuery.ReturnString("Select Purpose from PurchaseDetails where InvNo='" + invNo +
                                          "' AND ItemCode='" + iCode + "'");
                qty = Convert.ToDecimal(Inventory.AvailableNonPrintedQty(purpose, "Raw Sheet", iCode, godown, locationId));
            }
            return qty;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            decimal BalanceQty = ReturnableQty();
            if (ddMode.SelectedValue == "1")
            {
                if (BalanceQty >= Convert.ToDecimal(txtQty.Text))
                {
                    decimal qtyinStock = RawQtyinStock(ddProduct.SelectedValue, ddOrders.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue);

                    if (qtyinStock >= Convert.ToDecimal(txtQty.Text))
                    {
                        InsertData(0, Convert.ToDecimal(txtQty.Text));
                        Notify("Send for replacement saved successfully.", "success", lblMsg);
                    }
                    else
                    {
                        Notify("Item is not available into stock!", "error", lblMsg);
                    }
                }
                else
                {
                    Notify("Invalid Return Quantity!", "error", lblMsg);
                }
            }
            else if (ddMode.SelectedValue == "2")
            {
                InsertData(Convert.ToDecimal(txtQty.Text), 0);
                Notify("Receive info successfully saved.", "success", lblMsg);
            }
            else
            {
                //InsertData("3");
                //Notify("Receive info successfully saved.", "success", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
        finally
        {
            ItemGrid.DataBind();
        }
    }

    private void InsertData(decimal inWeight, decimal outWeight)
    {
        int inQty = 0;
        int outQty = 0;
        string lName = Page.User.Identity.Name.ToString();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO PurchaseReturn (SupplierName, SupplierID, InvoiceNo, ReturnDate, GodownName, GodownId, Remark, SalesDetailID, ItemName, Qty, Price, Total, EntryBy) VALUES (@SupplierName, @SupplierID, @InvoiceNo, @ReturnDate, @GodownName, @GodownId, @Remark, @SalesDetailID, @ItemName, @Qty, @Price, @Total, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@SupplierName", ddVendor.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@SupplierID", ddVendor.SelectedValue);
        cmd2.Parameters.AddWithValue("@InvoiceNo", ddOrders.SelectedValue);
        cmd2.Parameters.AddWithValue("@ReturnDate", Convert.ToDateTime(txtReturnDate.Text).ToString("yyyy-MM-dd"));
        cmd2.Parameters.AddWithValue("@GodownName", ddGodown.SelectedItem.Text);

        cmd2.Parameters.AddWithValue("@GodownId", ddGodown.SelectedValue);
        cmd2.Parameters.AddWithValue("@Remark", txtRemarks.Text);
        cmd2.Parameters.AddWithValue("@SalesDetailID", ddProduct.SelectedValue);
        cmd2.Parameters.AddWithValue("@ItemName", ddProduct.SelectedItem.Text);

        if (inWeight > 0)
        {
            cmd2.Parameters.AddWithValue("@Qty", inWeight);
        }
        else
        {
            cmd2.Parameters.AddWithValue("@Qty", outWeight * (-1));
        }
        cmd2.Parameters.AddWithValue("@Price", 0);
        cmd2.Parameters.AddWithValue("@Total", 0);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        SQLQuery.ExecNonQry("Update PurchaseReturn Set BillNo='" + ddOrders.SelectedItem.Text + "', ItemCode='" + ddProduct.SelectedValue + "'  Where eid=(Select MAX(eid) from PurchaseReturn)");
        SQLQuery.ExecNonQry("Update PurchaseDetails Set ReturnQty=" + Convert.ToDecimal(txtQty.Text) + " Where id=" + ddProduct.SelectedValue);

        //Stock-transaction

        string itemType = "";
        string invNo = ddOrders.SelectedValue, iCode = ddProduct.SelectedValue;

        string retNo = RunQuery.SQLQuery.ReturnString("Select ISNULL(MAX(eid),0) from PurchaseReturn");
        string sizeId = SQLQuery.ReturnString("Select SizeRef from PurchaseDetails where InvNo='" + invNo + "' AND ItemCode='" + iCode + "'");

        string spec = SQLQuery.ReturnString("Select SizeRef from PurchaseDetails where InvNo='" + invNo + "' AND ItemCode='" + iCode + "'");
        string purpose = SQLQuery.ReturnString("Select Purpose from PurchaseDetails where InvNo='" + invNo + "' AND ItemCode='" + iCode + "'");

        string subGrp = Inventory.GetItemSubGroup(ddProduct.SelectedValue);
        if (subGrp == "9") //Tin Plate
        {
            itemType = "Raw Sheet";
            if (inWeight > 0)
            {
                decimal inQty1 = Convert.ToDecimal(SQLQuery.ReturnString("Select (pcs/Qty) from PurchaseDetails where InvNo='" + invNo + "' AND ItemCode='" + iCode + "'")) * inWeight;
                inQty = Convert.ToInt32(Math.Round(inQty1));
            }
            else
            {
                decimal outQty1 = Convert.ToDecimal(SQLQuery.ReturnString("Select (pcs/Qty) from PurchaseDetails where InvNo='" + invNo + "' AND ItemCode='" + iCode + "'")) * outWeight;
                outQty = Convert.ToInt32(Math.Round(outQty1));
            }
        }

        if (ddMode.SelectedValue == "3")
        {
            inQty = 0; outQty = 0; inWeight = 0;
            outWeight = 0;
        }

        string detail = "Purchase Return Date: " + txtDate.Text + ".";

        if ((inQty == 0 && inWeight > 0))
        {
            inQty = Convert.ToInt32(inWeight);
        }
        if ((outQty == 0 && outWeight > 0))
        {
            outQty = Convert.ToInt32(outWeight);
        }

        Inventory.SaveToStock(purpose, ddOrders.SelectedValue, "Purchase Return: " + ddMode.SelectedItem.Text, retNo, sizeId, ddVendor.SelectedValue, "", "", spec, ddProduct.SelectedValue, ddProduct.SelectedItem.Text, itemType, ddGodown.SelectedValue, "", "1", inQty, outQty, 0, inWeight, outWeight, "", txtRemarks.Text, "Return", "", lName, Convert.ToDateTime(txtReturnDate.Text).ToString("yyyy-MM-dd"));

    }


    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadInvoices();
        LoadReturnHistory();
    }
    protected void ddOrders_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddProduct.DataBind();
        LoadReturnHistory();
        ltrBillNo.Text = "Party LC/TT/Invoice No.: " + SQLQuery.ReturnString("Select BillNo FROM Purchase where InvNo='" + ddOrders.SelectedValue + "'");
        LoadProducts();
    }

    private void LoadReturnHistory()
    {
        if (ddOrders.SelectedValue != "")
        {
            string deliveryDate = RunQuery.SQLQuery.ReturnString("Select DeliveryDate from Sales where InvNo='" + ddOrders.SelectedValue + "'");
            if (deliveryDate != "")
            {
                txtReturnDate.Text = Convert.ToDateTime(deliveryDate).ToShortDateString();
            }
            //ddProduct.DataBind();
            ItemGrid.DataBind();

            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1) OrderDate, ChallanNo, ChallanDate  from Purchase where InvNo='" + ddOrders.SelectedValue + "'");

            foreach (DataRow drx in dtx.Rows)
            {
                txtDate.Text = drx["OrderDate"].ToString();
                txtChallanNo.Text = drx["ChallanNo"].ToString();
                txtChallanDate.Text = drx["ChallanDate"].ToString();
            }
        }
    }

    protected void ddMode_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadInvoices();
    }

    private void LoadInvoices()
    {
        try
        {
            if (ddMode.SelectedValue == "1")
            {
                ltrDate.Text = "Date of Sending :";
                ltrQty.Text = "Send Quantity : ";
                ltrStock.Text = "Send From Godown : ";
                PnlAmount.Visible = false;
                pnlStock.Visible = true;

                SQLQuery.PopulateDropDown(
                    "SELECT [InvNo], ([InvNo]+': Bill # '+BillNo+' Dt: '+CONVERT(varchar,OrderDate,103) +'') AS BillNo FROM [Purchase] WHERE SupplierID='" +
                    ddVendor.SelectedValue + "' ORDER BY PID DESC", ddOrders, "InvNo", "BillNo");
            }
            else
            {
                SQLQuery.PopulateDropDown("SELECT DISTINCT [InvoiceNo], BillNo FROM [PurchaseReturn] WHERE SupplierID='" + ddVendor.SelectedValue + "' AND Status='Send' Group by InvoiceNo,BillNo ORDER BY InvoiceNo,BillNo", ddOrders, "InvoiceNo", "BillNo");

                if (ddMode.SelectedValue == "2")
                {
                    ltrDate.Text = "Receive Date :";
                    ltrQty.Text = "Receive Quantity : ";
                    ltrStock.Text = "Received to Godown : ";
                    PnlAmount.Visible = false;
                    pnlStock.Visible = true;
                }
                else
                {
                    ltrDate.Text = "Adjustment Date :";
                    ltrQty.Text = "Adjust Quantity : ";
                    PnlAmount.Visible = true;
                    pnlStock.Visible = false;
                }
            }
            LoadProducts();

        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }

    private void LoadProducts()
    {
        try
        {
            if (ddMode.SelectedValue == "1")
            {
                SQLQuery.PopulateDropDown("SELECT ItemCode, ItemName + N' ' + COALESCE ((SELECT spec FROM Specifications WHERE (id = PurchaseDetails.SizeRef)), '') AS ItemName FROM [PurchaseDetails]  WHERE InvNo='" +
                    ddOrders.SelectedValue + "' ORDER BY [id]", ddProduct, "ItemCode", "ItemName");
            }
            else
            {
                SQLQuery.PopulateDropDown("SELECT Distinct [ItemCode], [ItemName] FROM [PurchaseReturn] WHERE InvoiceNo='" + ddOrders.SelectedValue + "' ORDER BY [ItemCode], [ItemName]", ddProduct, "ItemCode", "ItemName");
            }
            ReturnableQty();
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }

    private decimal ReturnableQty()
    {
        string detail = "Purpose: " + SQLQuery.ReturnString("Select Purpose from Purpose where pid=(Select Purpose from PurchaseDetails where InvNo='" + ddOrders.SelectedValue + "' AND ItemCode='" + ddProduct.SelectedValue + "')");
        detail += "<br>Grade: " + SQLQuery.ReturnString("SELECT GradeName FROM [ItemGrade] where GradeID=(Select Grade from PurchaseDetails where InvNo='" + ddOrders.SelectedValue + "' AND ItemCode='" + ddProduct.SelectedValue + "')");
        detail += "<br>Category: " + SQLQuery.ReturnString("SELECT CategoryName FROM [Categories] where CategoryID =(Select Category from PurchaseDetails where InvNo='" + ddOrders.SelectedValue + "' AND ItemCode='" + ddProduct.SelectedValue + "')");

        decimal pQty = Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(Qty),0) from PurchaseDetails where InvNo='" + ddOrders.SelectedValue +
                              "' AND ItemCode='" + ddProduct.SelectedValue + "'"));
        decimal rQty = Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(Qty),0) from PurchaseReturn where InvoiceNo='" + ddOrders.SelectedValue +
                              "' AND Status='Send' AND ItemCode='" + ddProduct.SelectedValue + "'"));
        decimal balanceQty = pQty - rQty;
        txtPurchasedQty.Text = detail + "<br>Purchased qty: " + pQty + "<br> Pending replacement: " + rQty;
        return balanceQty;
    }

    protected void ddProduct_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ReturnableQty();
    }

    protected void ddSuppCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddVendor.DataBind();
    }

}
