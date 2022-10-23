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

public partial class app_Stock_Adj_Waste : System.Web.UI.Page
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
            //ddGroup.SelectedValue = "3";
            ddSubGroup.DataBind();
            ddGrade.DataBind();
            ddCategory.DataBind();
            ddProduct.DataBind();

            ddGodown.DataBind();
            ddLocation.DataBind();

            ddCustomer.DataBind();
            ddBrand.DataBind();
            ddSize.DataBind();
            ddColor.DataBind();

            //LoadSummesion("");
            LoadFormControls();
            QtyinStock();
            BindItemGrid();
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
                    txtQty.Text = "0";
                }
                //SizeId, ProductID, BrandID
                SqlCommand cmde = new SqlCommand("SELECT ProductName FROM StockinDetailsRaw WHERE  ProductID ='" + ddProduct.SelectedValue + "' AND  OrderID ='' AND EntryBy ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmde.Connection.Open();
                string isExist = Convert.ToString(cmde.ExecuteScalar());
                cmde.Connection.Close();

                if (isExist == "" && ddProduct.SelectedValue != "")
                {
                    //if (ltrCQty.Text != txtQty.Text)
                    //{
                    InsertData();
                    txtQty.Text = "";
                    txtWeight.Text = "";
                    //}
                    //else
                    //{
                    //   lblMsg2.Attributes.Add("class", "xerp_error");
                    //   lblMsg2.Text = "ERROR: You must edit quantity for adjustment!";
                    //}
                }
                else
                {
                    lblMsg2.Attributes.Add("class", "xerp_warning");
                    lblMsg2.Text = "ERROR: Product Already exist or product selection box is empty!";
                }
            }
            else
            {
                //ExecuteUpdate();
                RunQuery.SQLQuery.ExecNonQry("Delete StockinDetailsRaw where (id ='" + lblOrderID.Text + "')");
                InsertData();
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
        string productName = ddProduct.SelectedItem.Text;

        int balance = Convert.ToInt32(txtQty.Text) - Convert.ToInt32(ltrCQty.Text);
        SqlCommand cmd2 = new SqlCommand("INSERT INTO StockinDetailsRaw (StockType, OrderID, ProductID, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy, CompanyFor,  GodownID, LocationID, QtyBalance, DeliveredQty) VALUES ('Waste', @OrderID, @ProductID, @ProductName, @UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, @CompanyFor, '" + ddGodown.SelectedValue + "', '"+ddLocation.SelectedValue+"','" + balance + "', '" + ltrCQty.Text + "'  )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //if (ddType.SelectedValue == "Printed Sheet")
        //{
        //    cmd2 = new SqlCommand(@"INSERT INTO StockinDetailsRaw (ItemType, OrderID, SizeId, ProductID, Customer, BrandID, ProductName, Color, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy, CompanyFor,  GodownID, QtyBalance, DeliveredQty)" +
        //                                " VALUES ('" + ddType.SelectedValue + "', @OrderID, @SizeId, @ProductID, '" + ddCustomer.SelectedValue + "', @BrandID, @ProductName, '" + ddColor.SelectedValue + "', @UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, @CompanyFor, '" + ddGodown.SelectedValue + "', '" + balance + "', '" + ltrCQty.Text + "'  )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //}
        cmd2.Parameters.AddWithValue("@OrderID", "");
        cmd2.Parameters.AddWithValue("@SizeId", ddSize.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductID", ddProduct.SelectedValue);
        cmd2.Parameters.AddWithValue("@BrandID", ddBrand.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductName", productName);

        cmd2.Parameters.AddWithValue("@UnitCost", "0");
        cmd2.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@UnitWeight", txtWeight.Text);

        cmd2.Parameters.AddWithValue("@ItemTotal", "0");
        cmd2.Parameters.AddWithValue("@TotalWeight", "0");
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@CompanyFor", ddCustomer.SelectedItem.Text);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        RunQuery.SQLQuery.Empty2Zero(txtWeight);
        RunQuery.SQLQuery.Empty2Zero(txtQty);
        string lName = Page.User.Identity.Name.ToString();
        string productName = ddProduct.SelectedItem.Text;

        int balance = Convert.ToInt32(txtQty.Text) - Convert.ToInt32(ltrCQty.Text);
        SqlCommand cmd2 = new SqlCommand("UPDATE StockinDetailsRaw SET SizeId='" + ddGroup.SelectedValue + "', ProductID='" + ddProduct.SelectedValue + "', BrandID='" + ddSubGroup.SelectedValue + "'," +
                                "ProductName=@ProductName, UnitCost=@UnitCost, UnitWeight=@UnitWeight, Quantity=@Quantity, " +
                                "ItemTotal=@ItemTotal, TotalWeight=@TotalWeight, CompanyFor=@CompanyFor, QtyBalance='" + balance + "'  where (id ='" + lblOrderID.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductName", productName);
        cmd2.Parameters.AddWithValue("@UnitCost", 0);
        cmd2.Parameters.AddWithValue("@UnitWeight", txtWeight.Text);
        cmd2.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@ItemTotal", 0);
        cmd2.Parameters.AddWithValue("@TotalWeight", 0);
        cmd2.Parameters.AddWithValue("@CompanyFor", ddGroup.SelectedValue);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
    }

    private void BindItemGrid()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand(@"SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, QtyBalance, DeliveredQty, UnitWeight, 
(SELECT  [Company] FROM [Party] WHERE [PartyID]= StockinDetailsRaw.Customer) AS Customer,
(SELECT [BrandName] FROM [CustomerBrands] WHERE BrandID=StockinDetailsRaw.BrandID) AS BrandID,
(SELECT [BrandName] FROM [Brands] WHERE BrandID=StockinDetailsRaw.SizeId) AS SizeId,
(SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=StockinDetailsRaw.Color) AS Color
FROM StockinDetailsRaw WHERE GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '"+ddLocation.SelectedValue+"' AND EntryBy=@EntryBy AND OrderID='' AND StockType='Waste'  ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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
        ltrQty.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(SUM(QtyBalance),0) FROM StockinDetailsRaw WHERE OrderID='' AND GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '" + ddLocation.SelectedValue + "'") + "pcs, " +
            RunQuery.SQLQuery.ReturnString("SELECT ISNULL(SUM(UnitWeight),0) FROM StockinDetailsRaw WHERE OrderID='' AND GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '" + ddLocation.SelectedValue + "'") + "kg";

    }
    protected void ItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;
            TextBox txtQty = ItemGrid.Rows[index].FindControl("txtQty") as TextBox;

            string OrderID = RunQuery.SQLQuery.ReturnString("Select OrderID from StockinDetailsRaw where Id='" + lblItemCode.Text + "'");
            //string deliveredQty = RunQuery.SQLQuery.ReturnString("Select SUM(DeliveredQty) from StockinDetailsRaw where OrderID='" + OrderID + "'");

            //if (deliveredQty == "0")
            //{
            SqlCommand cmd7 = new SqlCommand("DELETE StockinDetailsRaw WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();

            BindItemGrid();
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "Item deleted from order ...";
            //}
            //else
            //{
            //    lblMsg2.Attributes.Add("class", "xerp_warning");
            //    lblMsg2.Text = "Order is Locked! There is some pending delivery...";
            //}
            btnAdd.Text = "Add";
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
                //if (ltrQty.Text == "0")
                //{
                //    lblMsg.Attributes.Add("class", "xerp_error");
                //    lblMsg.Text = "ERROR: <br> No items were added to the adjustment!";
                //}
                //else
                //{
                ExecuteInsert();
                //ClearControls(Page);
                txtQty.Text = "";
                txtWeight.Text = "";

                QtyinStock();
                GridView1.DataBind();
                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Stock adjustment saved successfully...";
                //}
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

        SqlCommand cmd = new SqlCommand("UPDATE StockinDetailsRaw SET OrderID='" + orderNo + "' WHERE  EntryBy=@EntryBy AND OrderID=''  AND StockType='Waste'  AND GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '" + ddLocation.SelectedValue + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryBy", lName);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        //Sock-in

        foreach (GridViewRow row in ItemGrid.Rows)
        {
            Label lblEntryId = row.FindControl("lblEntryId") as Label;
            string sizeId = RunQuery.SQLQuery.ReturnString("SELECT SizeID FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");
            string brandID = RunQuery.SQLQuery.ReturnString("SELECT brandID FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");
            string productID = RunQuery.SQLQuery.ReturnString("SELECT productID FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");
            string ProductName = RunQuery.SQLQuery.ReturnString("SELECT ProductName FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");

            string itemType = RunQuery.SQLQuery.ReturnString("SELECT ItemType FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");
            string customer = RunQuery.SQLQuery.ReturnString("SELECT Customer FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");
            string color = RunQuery.SQLQuery.ReturnString("SELECT Color FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "')");

            Label lbliQty = row.FindControl("lbliQty") as Label;
            Label lblUnitWeight = row.FindControl("lblUnitWeight") as Label;
            Label lblQtyBalance = row.FindControl("lblQtyBalance") as Label;

            int inQty = Convert.ToInt32(lbliQty.Text); int outQty = 0;
            decimal inWeight = Convert.ToDecimal(lblUnitWeight.Text); decimal outWeight = 0;
            string type = "Stock-in"; string detail = "Stock Adjustment";

            if (inQty < 0)
            {
                inQty = 0; outQty = Convert.ToInt32(lbliQty.Text) * (-1);
                type = "Stock-out";
            }
            if (inWeight < 0)
            {
                inWeight = 0; outWeight = Convert.ToDecimal(lblUnitWeight.Text) * (-1);
            }

            if (lbliQty.Text == lblQtyBalance.Text)
            {
                detail = "Opening Stock";
            }

            Stock.Inventory.SaveToStock("", orderNo, detail, lblEntryId.Text, sizeId, customer, brandID, color, "", productID, ProductName, itemType, ddGodown.SelectedValue, ddLocation.SelectedValue, ddGroup.SelectedValue, inQty, outQty, 0, inWeight, outWeight, "", "Stock-in", "Adjustment", ddLocation.SelectedItem.Text, lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            
            
            //Accounting.VoucherEntry.StockEntry(invNo, "Purchase", entryId, size, "", iCode, iName, ddGodown.SelectedValue, ddLocation.SelectedValue, iGrp, qty, "0", "0", "0", guaranty, "Purchase", stockType, stockLocation, lName);
            //Consumed Processed items stock-out
            //string companyFor = RunQuery.SQLQuery.ReturnString("SELECT Company from Party where PartyID=(Select CompanyFor FROM StockinDetailsRaw where (Id='" + lblEntryId.Text + "'))");

            //DataTable citydt = RunQuery.SQLQuery.ReturnDataTable("Select RawItemCode, RawItemName, RequiredQuantity, Wastage, UnitType from Ingradiants where ItemCode='" + productID + "'");

            //foreach (DataRow citydr in citydt.Rows)
            //{
            //    string icode = citydr["RawItemCode"].ToString();
            //    string iName = citydr["RawItemName"].ToString();
            //    string reqQty = citydr["RequiredQuantity"].ToString();
            //    string waste = citydr["Wastage"].ToString();
            //    string unit = citydr["UnitType"].ToString();
            //    decimal rQty = Convert.ToDecimal(reqQty) * Convert.ToDecimal(lbliQty.Text);

            //    Accounting.VoucherEntry.ProductionStockEntry(orderNo, "Finished Items Stock-in", sizeId, companyFor, icode, iName, "", "3", "0", rQty.ToString(), "Auto Stock-out of Processed item during finished Stock-in", "Production", "Processed", ddLocation.SelectedItem.Text, lName);
            //}

        }
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
        //if (ddGroup.SelectedValue == "1")
        //{

        //}

        if (ddGroup.SelectedValue == "5")
        {
            //PanelWarrenty.Visible = true;
        }
        else
        {
            //PanelWarrenty.Visible = false;
        }
    }

    protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddGrade.DataBind();
            ddCategory.DataBind();
            ddProduct.DataBind();
            LoadFormControls();
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
    }


    private void QtyinStock()
    {
        try
        {
            ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");

            //if (ddType.SelectedValue == "Printed Sheet")
            //{
            //    txtCurrentQty.Text = Stock.Inventory.AvailablePrintedQty(ddPurpose.SelectedValue, ddProduct.SelectedValue,
            //        ddCustomer.SelectedValue, ddBrand.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue,
            //        ddGodown.SelectedValue);

            //    ltrCQty.Text = txtCurrentQty.Text;

            //    txtCurrentKg.Text = Stock.Inventory.AvailablePrintedWeight(ddPurpose.SelectedValue, ddProduct.SelectedValue,
            //        ddCustomer.SelectedValue, ddBrand.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue,
            //        ddGodown.SelectedValue);
               
            //}
            //else
            //{
            txtCurrentQty.Text = Stock.Inventory.NonUsableQty(ddProduct.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue);
                 ltrCQty.Text = txtCurrentQty.Text;
                 txtCurrentKg.Text = Stock.Inventory.NonUsableWeight(ddProduct.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue);
                 //}
            GridView1.DataBind();
            BindItemGrid();
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
        finally { LoadFormControls(); }

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
        SqlCommand cmd = new SqlCommand(@"SELECT ItemType, ProductID, SizeId, Customer, BrandID, Color, Quantity, UnitWeight, GodownID, LocationID FROM [StockinDetailsRaw] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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

            LoadFormControls();

            if (dr[0].ToString() == "Printed Sheet")
            {
                ddType.SelectedValue = "Printed Sheet";
                LoadFormControls();
                ddSize.SelectedValue = dr[2].ToString();
                ddCustomer.SelectedValue = dr[3].ToString();

                ddBrand.Items.Clear();
                ListItem lst = new ListItem("--- all ---", "");
                ddBrand.Items.Insert(ddBrand.Items.Count, lst);
                ddBrand.DataBind();

                ddBrand.SelectedValue = dr[4].ToString();
                ddColor.SelectedValue = dr[5].ToString();
            }
            else
            {
                ddType.SelectedValue = "Raw Sheet";
                LoadFormControls();
            }

            //txtRate.Text 
            txtQty.Text = dr[6].ToString();
            txtWeight.Text = dr[7].ToString();
            ddGodown.SelectedValue = dr[8].ToString();
            ddLocation.SelectedValue = dr[9].ToString();
            //ltrCQty.Text = dr[8].ToString();
            //txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtRate.Text));
        }
        cmd.Connection.Close();

    }
    protected void ddGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocation.DataBind();
        BindItemGrid();

        QtyinStock();
    }

    private void LoadFormControls()
    {

        if (ddSubGroup.SelectedValue == "17")
        {
            ddTypeArea.Attributes.Remove("class");
            ddTypeArea.Attributes.Add("class", "col-md-4");
        }
        else
        {
            ddTypeArea.Attributes.Remove("class");
            ddTypeArea.Attributes.Add("class", "col-md-4 hidden");
            ddType.SelectedValue = "Raw Sheet";
        }

        if (ddType.SelectedValue == "Printed Sheet")
        {
            pnlExtra.Visible = true;
        }
        else
        {
            pnlExtra.Visible = false;
        }
    }

    protected void ddCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand.Items.Clear();
        ListItem lst = new ListItem("--- all ---", "");
        ddBrand.Items.Insert(ddBrand.Items.Count, lst);
        ddBrand.DataBind();

        QtyinStock();
    }

    protected void ddType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
    }

    protected void ddBrand_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
    }

    protected void ddColor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
    }

    protected void ddSize_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
    }

    protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
    }

    protected void GridView1_OnRowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        QtyinStock();
    }

}