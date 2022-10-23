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

public partial class app_Stock_in : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

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

            ddGrade.DataBind();
            ddProduct.DataBind();

            ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");
            //txtInv.Focus();
            ddGodown.DataBind();
            ddLocation.DataBind();

        }
        //txtInv.Text = InvIDNo();
        
    }

    //Messagebox For Alerts    
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
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
                SqlCommand cmde = new SqlCommand("SELECT ProductName FROM StockinDetails WHERE SizeId ='" + ddSize.SelectedValue + "' AND ProductID ='" + ddProduct.SelectedValue + "' AND BrandID ='" + ddBrand.SelectedValue + "' AND  OrderID ='' AND EntryBy ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmde.Connection.Open();
                string isExist = Convert.ToString(cmde.ExecuteScalar());
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
                lblMsg2.Attributes.Add("class", "xerp_success");
                lblMsg2.Text = "Item updated successfully";
            }
            //ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);

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
    private void InsertData()
    {
        string lName = Page.User.Identity.Name.ToString();
        string productName = ddSize.SelectedItem.Text + " " + ddBrand.SelectedItem.Text + " " + ddProduct.SelectedItem.Text;

        if (chkMerge.Checked == true)
        {
            productName = ddSize.SelectedItem.Text + " " + ddBrand.SelectedItem.Text + " " + ddGrade.SelectedItem.Text + " " + ddProduct.SelectedItem.Text;
        }

        SqlCommand cmd2 = new SqlCommand("INSERT INTO StockinDetails (OrderID, SizeId, ProductID, BrandID, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy, CompanyFor) VALUES (@OrderID, @SizeId, @ProductID, @BrandID, @ProductName, @UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, @CompanyFor)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@OrderID", "");
        cmd2.Parameters.AddWithValue("@SizeId", ddSize.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductID", ddProduct.SelectedValue);
        cmd2.Parameters.AddWithValue("@BrandID", ddBrand.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductName", productName);

        cmd2.Parameters.AddWithValue("@UnitCost", "0");
        cmd2.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@UnitWeight", "0");

        cmd2.Parameters.AddWithValue("@ItemTotal", "0");
        cmd2.Parameters.AddWithValue("@TotalWeight", "0");
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@CompanyFor", ddCustomer.SelectedValue);

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

        SqlCommand cmd2 = new SqlCommand("UPDATE StockinDetails SET SizeId='" + ddSize.SelectedValue + "', ProductID='" + ddProduct.SelectedValue + "', BrandID='" + ddBrand.SelectedValue + "'," +
                                "ProductName=@ProductName, UnitCost=@UnitCost, UnitWeight=@UnitWeight, Quantity=@Quantity, " +
                                "ItemTotal=@ItemTotal, TotalWeight=@TotalWeight, CompanyFor=@CompanyFor  where (id ='" + lblOrderID.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductName", productName);
        cmd2.Parameters.AddWithValue("@UnitCost", 0);
        cmd2.Parameters.AddWithValue("@UnitWeight", 0);
        cmd2.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@ItemTotal", 0);
        cmd2.Parameters.AddWithValue("@TotalWeight", 0);
        cmd2.Parameters.AddWithValue("@CompanyFor", ddCustomer.SelectedValue);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();

    }

    private void BindItemGrid()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal FROM StockinDetails WHERE EntryBy=@EntryBy AND OrderID='' ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //cmd.CommandText = "SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal FROM StockinDetails WHERE EntryBy=@EntryBy AND OrderID='' ORDER BY Id";

        cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        cmd.Connection.Open();
        ItemGrid.EmptyDataText = "No items to view...";
        ItemGrid.DataSource = cmd.ExecuteReader();
        ItemGrid.DataBind();
        cmd.Connection.Close();

    }


    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Quantity),0) from StockinDetails where OrderID=''");
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
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;
            TextBox txtQty = ItemGrid.Rows[index].FindControl("txtQty") as TextBox;

            string OrderID = RunQuery.SQLQuery.ReturnString("Select OrderID from StockinDetails where Id='" + lblItemCode.Text + "'");
            string deliveredQty = RunQuery.SQLQuery.ReturnString("Select SUM(DeliveredQty) from StockinDetails where OrderID='" + OrderID + "'");

            if (deliveredQty == "0")
            {
                SqlCommand cmd7 = new SqlCommand("DELETE StockinDetails WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
            if (btnSave.Text == "Save")
            {
                if (ltrQty.Text == "0")
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Failed to save...<br> No products were added to the sales order!";
                }
                else
                {
                    ExecuteInsert();
                    //ClearControls(Page);

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Finished Stock-in process completed successfully...";
                }
            }
            else
            {
                //ExecuteUpdate();
                //ddStockin.DataBind();
                btnSave.Text = "Save";
                //EditField.Attributes.Add("class", "form-group hidden");

                lblMsg.Attributes.Add("class", "xerp_success");
                //lblMsg.Text = "Info successfully updated for " + DropDownList1.SelectedItem.Text;
            }

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error: " + ex.Message.ToString();
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

        SqlCommand cmd = new SqlCommand("UPDATE StockinDetails SET OrderID='" + orderNo + "' WHERE EntryBy=@EntryBy AND OrderID=''", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryBy", lName);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        //Sock-in

        foreach (GridViewRow row in ItemGrid.Rows)
        {
            Label lblEntryId = row.FindControl("lblEntryId") as Label;
            string sizeId = RunQuery.SQLQuery.ReturnString("SELECT SizeID FROM StockinDetails where (Id='" + lblEntryId.Text + "')");
            string brandID = RunQuery.SQLQuery.ReturnString("SELECT brandID FROM StockinDetails where (Id='" + lblEntryId.Text + "')");
            string productID = RunQuery.SQLQuery.ReturnString("SELECT productID FROM StockinDetails where (Id='" + lblEntryId.Text + "')");
            string ProductName = RunQuery.SQLQuery.ReturnString("SELECT ProductName FROM StockinDetails where (Id='" + lblEntryId.Text + "')");
            Label lbliQty = row.FindControl("lbliQty") as Label;

            //Accounting.VoucherEntry.StockEntry(orderNo, "Finished Items Stock-in", lblEntryId.Text, sizeId, brandID, productID, ProductName, ddGodown.SelectedValue, ddLocation.SelectedValue, "2", lbliQty.Text, "0", "0", "0", "", "Stock-in", "Finished", ddLocation.SelectedItem.Text, lName);
            
            //Consumed Processed items stock-out
            string companyFor = RunQuery.SQLQuery.ReturnString("SELECT Company from Party where PartyID=(Select CompanyFor FROM StockinDetails where (Id='" + lblEntryId.Text + "'))");

            DataTable citydt = RunQuery.SQLQuery.ReturnDataTable("Select RawItemCode, RawItemName, RequiredQuantity, Wastage, UnitType from Ingradiants where ItemCode='" + productID + "'");
            
            foreach (DataRow citydr in citydt.Rows)
            {
                string icode = citydr["RawItemCode"].ToString();
                string iName = citydr["RawItemName"].ToString();
                string reqQty = citydr["RequiredQuantity"].ToString();
                string waste = citydr["Wastage"].ToString();
                string unit = citydr["UnitType"].ToString();
                decimal rQty = Convert.ToDecimal(reqQty) * Convert.ToDecimal(lbliQty.Text);

                //Accounting.VoucherEntry.ProductionStockEntry(orderNo, "Finished Items Stock-in", sizeId, companyFor, icode, iName, "", "3", "0", rQty.ToString(), "Auto Stock-out of Processed item during finished Stock-in", "Production", "Processed", ddLocation.SelectedItem.Text, lName);
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("Delete StockinDetails WHERE EntryBy=@EntryBy AND OrderID=''", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
    }
    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        txtQty.Focus();
    }
    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddProduct.DataBind();
        QtyinStock();
    }

    private void QtyinStock()
    {
        ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");
        //txtStock.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where SizeID='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "'");
        //txtWeight.Text = RunQuery.SQLQuery.ReturnString("Select UnitWeight from StockinDetails  where Id=(Select ISNULL(MAX(Id),0) from StockinDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");
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
        SqlCommand cmd = new SqlCommand("SELECT SizeId, ProductID, BrandID, CompanyFor, Quantity, UnitCost, UnitWeight, DeliveredQty FROM [StockinDetails] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            ddSize.SelectedValue = dr[0].ToString();
            string productID = dr[1].ToString();
            string catID = RunQuery.SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + productID + "'");
            string grdID = RunQuery.SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
            ddGrade.SelectedValue = grdID;
            ddProduct.DataBind();
            ddProduct.SelectedValue = productID;

            ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + productID + "'");
            //txtStock.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where SizeID='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + productID + "'");

            ddCustomer.SelectedValue = dr[3].ToString();
            ddBrand.DataBind();
            ddBrand.SelectedValue = dr[2].ToString();
            //txtRate.Text 
            txtQty.Text = dr[4].ToString();
            //txtQty.Text = dr[5].ToString();

            //txtWeight.Text = dr[6].ToString();

            //txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtRate.Text));
        }
        cmd.Connection.Close();

    }
    protected void ddGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocation.DataBind();
    }
}