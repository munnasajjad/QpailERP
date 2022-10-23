using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_LLC_from_PI : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDate.Text = DateTime.Now.ToShortDateString();

            ddCustomer.DataBind();
            ddOrders.DataBind();
            BindItemGrid();
            //LoadSummesion("");

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");

            ddBrand.DataBind();
            ddGrade.DataBind();
            ddCategory.DataBind();
            ddProduct.DataBind();
            ddCustomer.Focus();
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
                }
                else
                {
                    Notify("ERROR: Product Already exist or product selection box is empty!", "warn", lblMsg);
                }
            }
            else
            {
                ExecuteUpdate();
                //pnlAdd.Visible = false;
                btnAdd.Text = "Add";

                Notify("Saved Successfully", "success", lblMsg);
                lblMsg2.Attributes.Add("class", "xerp_success");
                lblMsg2.Text = "Item updated successfully";
            }
            //ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);

        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg2);
        }
        finally
        {

            BindItemGrid();
        }
    }

    private void BindItemGrid()
    {
        string lName = Page.User.Identity.Name.ToString();
        string OrderID = ddOrders.SelectedValue;

        SqlCommand cmd = new SqlCommand("SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal FROM OrderDetails WHERE EntryBy=@EntryBy AND OrderID='" + OrderID + "' ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        cmd.Connection.Open();
        ItemGrid.EmptyDataText = "No items to view...";
        ItemGrid.DataSource = cmd.ExecuteReader();

        if (ddOrders.SelectedValue == "")
        {
            ItemGrid.DataSource = null;
        }
        ItemGrid.DataBind();
        cmd.Connection.Close();

        SqlCommand cmd7 = new SqlCommand("SELECT OrderDate, DeliveryDate, CustomerName, DeliveryAddress, TotalAmount, Discount, Vat, PayableAmount FROM [Orders] WHERE OrderID='" + OrderID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            txtDate.Text = Convert.ToDateTime(dr[0].ToString()).ToShortDateString();
            txtDeliveryDate.Text = Convert.ToDateTime(dr[1].ToString()).ToShortDateString();
            //ddCustomer.SelectedValue = dr[2].ToString();
            //ddCustomer.Enabled = false;
            ddBrand.DataBind();

            txtAddress.Text = dr[3].ToString();
            txtTotal.Text = dr[4].ToString();
            txtDiscount.Text = dr[5].ToString();
            txtVat.Text = dr[6].ToString();
            txtPay.Text = dr[7].ToString();
        }
        cmd7.Connection.Close();

        cmd7 = new SqlCommand("SELECT PiDate, PiTerms, LcTerms FROM [PI] WHERE PiNo='" + OrderID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            txtDate.Text = Convert.ToDateTime(dr[0].ToString()).ToShortDateString();
            txtTerms.Text = dr[1].ToString();

        }
        cmd7.Connection.Close();

        SqlCommand cmd2 = new SqlCommand("SELECT PiDate, LcNo, LcDate, LcExpDate, IssuingBank, IssueingBankBranch, AdvisingBankAccount, LcTerms FROM [PI] WHERE OrderID='" + OrderID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        SqlDataReader dr2 = cmd2.ExecuteReader();
        if (dr2.Read())
        {
            txtDate.Text = Convert.ToDateTime(dr2[0].ToString()).ToShortDateString();
            txtLCNo.Text = dr2[1].ToString();
            txtLCDate.Text = Convert.ToDateTime(dr2[2].ToString()).ToShortDateString();
            txtLcExpDate.Text = Convert.ToDateTime(dr2[3].ToString()).ToShortDateString();

            //ddIssueBank.SelectedValue = dr2[3].ToString();
            //txtBranch.Text = dr2[4].ToString();
            txtTerms.Text = dr2[7].ToString();
        }
        cmd2.Connection.Close();

        LoadSummesion(OrderID);
    }
    private void LoadSummesion(string OrderId)
    {
        //string OrderID = RunQuery.SQLQuery.ReturnString("Select OrderID from OrderDetails where Id='" + lblItemCode.Text + "'");
        ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Quantity),0) from OrderDetails where OrderID='" + OrderId + "'");
        string ttl = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(ItemTotal),0) from OrderDetails where OrderID='" + OrderId + "'");
        RunQuery.SQLQuery.ExecNonQry("Update Orders set TotalAmount=" + ttl + " where OrderID='" + OrderId + "'");
        txtTotal.Text = ttl;

        string vat = RunQuery.SQLQuery.ReturnString("Select Vat from Orders where OrderID='" + OrderId + "'");
        if (vat == "")
        {
            vat = "0";
        }
        txtPay.Text = vat;
        txtPay.Text = Convert.ToDecimal(Convert.ToDecimal(ttl) + Convert.ToDecimal(ttl) * Convert.ToDecimal(vat) / 100M).ToString("#.00");
    }

    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Quantity),0) from OrderDetails where OrderID='" + ddOrders.SelectedValue + "'");
        ltrWeight.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(TotalWeight),0) from OrderDetails where OrderID='" + ddOrders.SelectedValue + "'");
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
                Notify("Item deleted from order ...", "warn", lblMsg);
            }
            else
            {
                Notify("Order is Locked! There is some pending delivery...", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {

            Notify(ex.Message.ToString(), "error", lblMsg2);
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
            if (txtLCNo.Text != "" && txtLCDate.Text != "" && ddOrders.SelectedValue != "")
            {
                string isExist = RunQuery.SQLQuery.ReturnString("Select LcNo from PI where LcNo='" + txtLCNo.Text + "' ");
                if (isExist == "")
                {
                    LoadSummesion(ddOrders.SelectedValue);
                    UpdateOrder();

                    //if (txtEditPI.Text != "")
                    //{
                    //    RunQuery.SQLQuery.ExecNonQry("UPDATE PI SET PiNo='" + txtEditPI.Text + "' WHERE PiNo='" + ddOrders.SelectedValue + "'");
                    //    RunQuery.SQLQuery.ExecNonQry("UPDATE OrderDetails SET OrderID='" + txtEditPI.Text + "'  WHERE OrderID='" + ddOrders.SelectedValue + "'");
                    //    RunQuery.SQLQuery.ExecNonQry("UPDATE Orders SET OrderID='" + txtEditPI.Text + "' WHERE OrderID='" + ddOrders.SelectedValue + "'");
                    //    RunQuery.SQLQuery.ExecNonQry("UPDATE PO_Images SET PONo='" + txtEditPI.Text + "' WHERE PONo='" + ddOrders.SelectedValue + "'");
                    //}

                    ddOrders.DataBind();
                    //btnSave.Text = "Save";
                    //EditField.Attributes.Add("class", "form-group hidden");

                    Notify("LC info saved successfully.", "success", lblMsg);
                    //lblMsg.Attributes.Add("class", "xerp_success");
                    //lblMsg.Text = "LC info saved successfully.";
                }
                else
                {
                    Notify("LLC# already exist!", "warn", lblMsg);
                    //lblMsg.Attributes.Add("class", "xerp_error");
                    //lblMsg.Text = "LLC# already exist!";
                }
            }
            else
            {
                txtDeliveryDate.Focus();

                Notify("Invalid LLC No. or LLC Date!", "warn", lblMsg);
                //lblMsg.Attributes.Add("class", "xerp_error");
                //lblMsg.Text = "Invalid LLC No. or LLC Date!";
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

        cmd2.Parameters.AddWithValue("@OrderID", ddOrders.SelectedValue);
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
        cmd2.Parameters.AddWithValue("@UnitWeight", txtWeight.Text);
        cmd2.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@ItemTotal", Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@TotalWeight", Convert.ToDecimal(txtWeight.Text) * Convert.ToDecimal(txtQty.Text));

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();

    }

    private void UpdateOrder()
    {
        string lName = Page.User.Identity.Name.ToString();
        string orderDate = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        string lcDate = Convert.ToDateTime(txtLCDate.Text).ToString("yyyy-MM-dd");
        string expDate = Convert.ToDateTime(txtLcExpDate.Text).ToString("yyyy-MM-dd");

        RunQuery.SQLQuery.ExecNonQry("UPDATE Orders SET OrderType='LC', OrderID='" + txtLCNo.Text + "',  TotalAmount='" + txtTotal.Text + "', Vat='" + txtVat.Text + "', PayableAmount='" + txtPay.Text + "', DeliveryAddress='" + txtAddress.Text + "', DeliveryDate='" + Convert.ToDateTime(txtDeliveryDate.Text).ToString("yyyy-MM-dd") + "'   WHERE OrderID='" + ddOrders.SelectedValue + "'");
        RunQuery.SQLQuery.ExecNonQry("Update PI set PiDate='" + orderDate + "', LcNo='" + txtLCNo.Text + "', LcDate='" + lcDate + "', LcExpDate='" + expDate + "', IssuingBank='" + ddIssueBank.SelectedValue + "', IssueingBankBranch='" + txtBranch.Text + "', AdvisingBankAccount='" + ddBank.SelectedValue + "', LcTerms='" + txtTerms.Text + "', LcEntryBy='" + lName + "', LcEntryDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' Where PiNo='" + ddOrders.SelectedValue + "'");
        RunQuery.SQLQuery.ExecNonQry("UPDATE PO_Images SET PONo='" + txtLCNo.Text + "', ImgDescription='LC' WHERE EntryBy='" + lName + "' AND (PONo='' OR PONo='" + ddOrders.SelectedValue + "')");

        string PIExist = RunQuery.SQLQuery.ReturnString("Select OrderID FROM OrdersLC where OrderID='" + ddOrders.SelectedValue + "'");

        SqlCommand cmd = new SqlCommand("UPDATE OrderDetails SET OrderID='" + txtLCNo.Text + "'  WHERE OrderID='" + ddOrders.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        //if (PIExist == "")
        //{
        //    RunQuery.SQLQuery.ExecNonQry("Insert into OrdersLC (OrderID, LCNo, LCDate, LCExpireDate, IssueBank, IssueBranch, AdvisingBankAcc, EntryBy)" +
        //                         " Values ('" + ddOrders.SelectedValue + "','" + txtLCNo.Text + "','" + lcDate + "','" + lcExpDate + "','" + ddIssueBank.SelectedValue + "','" + txtBranch.Text + "','" + ddBank.SelectedValue + "','" + lName + "')");
        //}
        //else
        //{
        //    RunQuery.SQLQuery.ExecNonQry("Update OrdersLC Set LCNo='" + txtLCNo.Text + "', LCDate='" + lcDate + "', LCExpireDate='" + lcExpDate + "', IssueBank='" + ddIssueBank.SelectedValue + "', IssueBranch='" + txtBranch.Text + "', AdvisingBankAcc='" + ddBank.SelectedValue + "', EntryBy='" + lName + "' WHERE OrderID='" + ddOrders.SelectedValue + "'");
        //}        
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string delStatus = RunQuery.SQLQuery.ReturnString("Select DeliveryStatus from Orders WHERE OrderID='" + ddOrders.SelectedValue + "'");

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

            Response.Redirect("./Edit-PI.aspx");
        }
        else
        {
            Notify("This order has some pending delivery!", "warn", lblMsg);
            //lblMsg.Attributes.Add("class", "xerp_error");
            //lblMsg.Text = "<b>Unable to cancel.</b> This order has some pending delivery!";
        }
    }
    protected void ddOrders_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindItemGrid();
    }
    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand.DataBind();
        ddOrders.DataBind();
        BindItemGrid();

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
            pnlAdd.Visible = true;

            Notify("Info loaded in edit mode.", "info", lblMsg);
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
            ddCategory.SelectedValue = catID;
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

    protected void ddCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddProduct.DataBind();
    }
}