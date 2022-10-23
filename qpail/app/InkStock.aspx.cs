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
using System.Runtime.InteropServices;
using Accounting;
using RunQuery;

public partial class app_Ink_Stock : System.Web.UI.Page
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
            ddPurpose.DataBind();
            ddPurpose.SelectedValue = "1";
            ddGroup.DataBind();
            ddGroup.SelectedValue = "1";
            ddSubGroup.DataBind();
            ddSubGroup.SelectedValue = "10";
            ddGrade.DataBind();
            ddGrade.SelectedValue = "32";
            ddCategory.DataBind();
            ddCategory.SelectedValue = "226";
            ddProduct.DataBind();

            ddGodown.DataBind();
            ddLocation.DataBind();
            LoadSpecList("filter");

            //LoadSummesion("");
            LoadFormControls();
            QtyinStock();
            BindItemGrid();
        }
        //txtInv.Text = InvIDNo();
    }

    //Messagebox For Alerts    
    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    public string InvIDNo()
    {
        string lName = Page.User.Identity.Name.ToString();
        string yr = DateTime.Now.Year.ToString();
        DateTime countForYear = Convert.ToDateTime("01/01/" + yr + " 00:00:00");

        SqlCommand cmd = new SqlCommand("SELECT CONVERT(varchar, (ISNULL (max(Stockinl),0)+ 1 )) FROM Stockin WHERE EntryDate>=@EntryDate", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
                SQLQuery.Empty2Zero(txtPrice);

                //SizeId, ProductID, BrandID
                SqlCommand command = new SqlCommand("SELECT ProductName FROM StockDetailsInk WHERE  ProductID ='" + ddProduct.SelectedValue + "' AND  OrderID ='' AND EntryBy ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                command.Connection.Open();
                string isExist = Convert.ToString(command.ExecuteScalar());
                command.Connection.Close();

                if (isExist == "" && ddProduct.SelectedValue != "")
                {
                    string specification = ddSpec.SelectedValue;
                    if (txtSpec.Text != "" && lbSpec.Text == "Cancel" && ddSubGroup.SelectedValue == "10")//Insert Ink spec
                    {
                        isExist = SQLQuery.ReturnString("SELECT id FROM Specifications WHERE  spec ='" + txtSpec.Text + "'");
                        if (isExist == "")
                        {
                            SQLQuery.ExecNonQry("INSERT INTO Specifications (spec, EntryBy) VALUES ('" + txtSpec.Text + "', '" + lName + "') ");
                            specification = SQLQuery.ReturnString("SELECT MAX(id) FROM Specifications");
                            LoadSpecList(""); //ddSpec.DataBind();
                            ddSpec.SelectedValue = specification;
                        }
                        else
                        {
                            LoadSpecList("");
                            ddSpec.SelectedValue = isExist;
                        }
                    }
                    //else
                    //{
                    InsertData();
                    //}

                    txtQty.Text = "";
                    txtWeight.Text = "";
                    //else
                    //{
                    //   lblMsg2.Attributes.Add("class", "xerp_error");
                    //   lblMsg2.Text = "ERROR: You must edit quantity for adjustment!";
                    //}
                }
                else
                {
                    Notify("ERROR: Product Already exist or product selection box is empty!", "warn", lblMsg);
                }
            }
            else
            {
                //ExecuteUpdate();
                SQLQuery.ExecNonQry("DELETE StockDetailsInk WHERE (Id ='" + lblOrderID.Text + "')");
                InsertData();
                btnAdd.Text = "Add";
                Notify("Item updated successfully", "success", lblMsg2);
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
        //decimal previousStock = Convert.ToDecimal(SQLQuery.ReturnString("SELECT TOP(1) FROM"))
        decimal consumedWeight = Convert.ToDecimal(txtCurrentKg.Text) - Convert.ToDecimal(txtWeight.Text);
        //int balance = Convert.ToInt32(txtQty.Text) - Convert.ToInt32(ltrCQty.Text);
        SqlCommand cmd2 = new SqlCommand("INSERT INTO StockDetailsInk (OrderID, StockType, Purpose, SubGroup, Grade, Category, GodownID, LocationID, ProductID, ProductName, Specification, UnitType, PreviousStockQuantity, CurrentStockQuantity, ConsumedWeight, EntryBy, EntryDate) " +
                                         "VALUES (@OrderID, @StockType, @Purpose, @SubGroup, @Grade, @Category, @GodownID, @LocationID, @ProductID, @ProductName, @Specification, @UnitType, @PreviousStockQuantity, @CurrentStockQuantity, @ConsumedWeight, @EntryBy, @EntryDate)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@OrderID", "");
        cmd2.Parameters.AddWithValue("@StockType", "Raw");
        cmd2.Parameters.AddWithValue("@Purpose", ddPurpose.SelectedValue);
        cmd2.Parameters.AddWithValue("@SubGroup", ddSubGroup.SelectedValue);
        cmd2.Parameters.AddWithValue("@Grade", ddGrade.SelectedValue);
        cmd2.Parameters.AddWithValue("@Category", ddCategory.SelectedValue);
        cmd2.Parameters.AddWithValue("@GodownID", ddGodown.SelectedValue);
        cmd2.Parameters.AddWithValue("@LocationID", ddLocation.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductID", ddProduct.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductName", ddProduct.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@Specification", ddSpec.SelectedValue);
        cmd2.Parameters.AddWithValue("@UnitType", ltrUnit.Text);
        cmd2.Parameters.AddWithValue("@PreviousStockQuantity", Convert.ToDecimal(txtCurrentKg.Text));
        cmd2.Parameters.AddWithValue("@CurrentStockQuantity", Convert.ToDecimal(txtWeight.Text));
        cmd2.Parameters.AddWithValue("@ConsumedWeight", consumedWeight);
        cmd2.Parameters.AddWithValue("@ItemTotal", "0");
        cmd2.Parameters.AddWithValue("@TotalWeight", "0");
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        SQLQuery.Empty2Zero(txtWeight);
        SQLQuery.Empty2Zero(txtQty);
        string lName = Page.User.Identity.Name.ToString();
        string productName = ddProduct.SelectedItem.Text;

        int balance = Convert.ToInt32(txtQty.Text) - Convert.ToInt32(ltrCQty.Text);
        SqlCommand cmd2 = new SqlCommand("UPDATE StockinDetailsRaw SET SizeId='" + ddGroup.SelectedValue + "', ProductID='" + ddProduct.SelectedValue + "', BrandID='" + ddSubGroup.SelectedValue + "'," +
                                "ProductName=@ProductName, UnitCost=@UnitCost, UnitWeight=@UnitWeight, Quantity=@Quantity, " +
                                "ItemTotal=@ItemTotal, TotalWeight=@TotalWeight, CompanyFor=@CompanyFor, QtyBalance='" + balance + "'  WHERE (id ='" + lblOrderID.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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
        SqlCommand cmd = new SqlCommand(@"SELECT SDI.Id, P.Purpose, ISG.CategoryName AS SubGroup, IG.GradeName AS Grade, C.CategoryName AS Category, SDI.ProductID, ProductName, S.Spec AS Specification, PreviousStockQuantity, CurrentStockQuantity, ConsumedWeight FROM StockDetailsInk AS SDI 
        INNER JOIN Purpose AS P ON P.pid = SDI.Purpose 
        INNER JOIN ItemSubGroup AS ISG ON ISG.CategoryID = SDI.SubGroup
        INNER JOIN ItemGrade AS IG ON IG.GradeID = SDI.Grade
        INNER JOIN Categories AS C ON C.CategoryID = SDI.Category
        INNER JOIN Specifications AS S ON S.id = SDI.Specification WHERE SDI.GodownID='" + ddGodown.SelectedValue + "' AND SDI.LocationID = '" + ddLocation.SelectedValue + "' AND SDI.EntryBy= '" + lName + "' AND SDI.OrderID='' AND SDI.StockType='Raw'  ORDER BY SDI.Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //cmd.CommandText = "SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal FROM StockinDetailsRaw WHERE EntryBy=@EntryBy AND OrderID='' ORDER BY Id";

        //cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        cmd.Connection.Open();
        ItemGrid.EmptyDataText = "No items to view...";
        ItemGrid.DataSource = cmd.ExecuteReader();
        ItemGrid.DataBind();
        cmd.Connection.Close();

    }


    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ltrQty.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(ConsumedWeight),0) FROM StockDetailsInk WHERE OrderID='' AND GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '" + ddLocation.SelectedValue + "'") + "kg";
        //QtyinStock();
    }
    protected void ItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;
            //if (deliveredQty == "0")
            //{
            SqlCommand cmd7 = new SqlCommand("DELETE StockDetailsInk WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();

            BindItemGrid();
            Notify("Item deleted from order ...", "success", lblMsg2);

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
            if (btnSave.Text == "Save")
            {
                if (ltrQty.Text == "0")
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "ERROR: <br> No items were added!";
                }
                else
                {
                    ExecuteInsert();
                    //ClearControls(Page);
                    txtQty.Text = "";
                    txtWeight.Text = "";

                    QtyinStock();
                    GridView1.DataBind();
                    Notify("Ink consumption saved successfully...", "success", lblMsg);
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
            Notify(ex.Message.ToString(), "error", lblMsg);
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
        cmd2.Parameters.AddWithValue("@Remarks", ddRemark.SelectedValue);
        cmd2.Parameters.AddWithValue("@TotalQty", ltrQty.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@ProjectId", lblProject.Text);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        SqlCommand cmd = new SqlCommand("UPDATE StockDetailsInk SET OrderID='" + orderNo + "' WHERE  EntryBy='" + lName + "' AND OrderID=''  AND StockType='Raw'  AND GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '" + ddLocation.SelectedValue + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        //Sock-in

        foreach (GridViewRow row in ItemGrid.Rows)
        {
            Label lblEntryId = row.FindControl("lblEntryId") as Label;

            string purpose = SQLQuery.ReturnString("SELECT Purpose FROM StockDetailsInk WHERE (Id='" + lblEntryId.Text + "')");
            string productID = SQLQuery.ReturnString("SELECT ProductID FROM StockDetailsInk WHERE (Id='" + lblEntryId.Text + "')");
            string productName = SQLQuery.ReturnString("SELECT ProductName FROM StockDetailsInk WHERE (Id='" + lblEntryId.Text + "')");
            string specification = SQLQuery.ReturnString("SELECT Specification FROM StockDetailsInk WHERE (Id='" + lblEntryId.Text + "')");
            decimal price = Convert.ToDecimal(SQLQuery.ReturnString("SELECT TOP(1) Price FROM Stock WHERE ProductID = '" + productID + "' ORDER BY EntryID DESC"));
            Label labelConsumedWeight = row.FindControl("labelConsumedWeight") as Label;


            Stock.Inventory.SaveToStock(purpose, orderNo, "InkConsumption", lblEntryId.Text, "", "", "", "", specification, productID, productName, "", ddGodown.SelectedValue, ddLocation.SelectedValue, ddGroup.SelectedValue, 0, 0, price, 0, Convert.ToDecimal(labelConsumedWeight.Text), "", "Stock-Out", "", ddLocation.SelectedItem.Text, lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            Stock.Inventory.FifoInsert(productID, "InkConsumption", "", specification, "", "", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "", "", 0, Convert.ToDecimal(labelConsumedWeight.Text), 0, ddGodown.SelectedValue, ddLocation.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd"), "Stock Out", orderNo, "0");
        }
        //Cost of goods sold data
        int masterTableId = Convert.ToInt32(SQLQuery.ReturnString("SELECT MAX(Stockinl) AS Stockinl FROM Stockin WHERE OrderID<>''"));
        InsertCostOfGoodsSoldData(masterTableId, orderNo);
    }

    protected decimal InsertCostOfGoodsSoldData(int masterTableId, string orderID)
    {
        string lName = Page.User.Identity.Name;
        decimal grossTotalAmount = 0;
        foreach (GridViewRow row in ItemGrid.Rows)
        {
            Label labelProductID = row.FindControl("labelProductID") as Label;
            Label labelProductName = row.FindControl("labelProductName") as Label;
            Label labelConsumedWeight = row.FindControl("labelConsumedWeight") as Label;
            //TextBox txtRate = row.FindControl("txtPrice") as TextBox;
            //TextBox txtExcessQuantity = row.FindControl("txtExcessQuantity") as TextBox;
            //decimal totalQuantity = Convert.ToDecimal(labelConsumedWeight.Text) + Convert.ToDecimal(txtExcessQuantity.Text);
            decimal consumedAmount = Convert.ToDecimal(labelConsumedWeight.Text);
            grossTotalAmount += consumedAmount;
            TextBox txtRemarks = row.FindControl("txtRemarks") as TextBox;
            //Need to specify condition
            string inKg = SQLQuery.ReturnString("SELECT InKg FROM tblFifo WHERE ItemCode = '" + labelProductID.Text + "' AND (InType = 'RawMaterials')");
            decimal unitWeight = Convert.ToDecimal(inKg);

            SqlCommand cmd = new SqlCommand("INSERT INTO BOQConsumptionFinal (SalesDetailsMasterId, ComponentId, ComponentName, DeliveredQuantity, Rate, ExcessQuantity, TotalQuantity, TotalWeight, ConsumedAmount, Remarks, EntryBy, EntryDate)" +
                                            " VALUES (@SalesDetailsMasterId, @ComponentId, @ComponentName, @DeliveredQuantity, @Rate, @ExcessQuantity, @TotalQuantity, @TotalWeight, @ConsumedAmount, @Remarks, @EntryBy, @EntryDate)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd.Parameters.AddWithValue("@SalesDetailsMasterId", masterTableId);
            cmd.Parameters.AddWithValue("@ComponentId", Convert.ToDecimal(labelProductID.Text));
            cmd.Parameters.AddWithValue("@ComponentName", labelProductName.Text);
            cmd.Parameters.AddWithValue("@DeliveredQuantity", Convert.ToDecimal(labelConsumedWeight.Text));
            cmd.Parameters.AddWithValue("@Rate", "");
            cmd.Parameters.AddWithValue("@ExcessQuantity", "");
            cmd.Parameters.AddWithValue("@TotalQuantity", consumedAmount);
            cmd.Parameters.AddWithValue("@TotalWeight", unitWeight * consumedAmount);
            cmd.Parameters.AddWithValue("@ConsumedAmount", consumedAmount);
            cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
            cmd.Parameters.AddWithValue("@EntryBy", lName);
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            //Stock.Inventory.FifoInsert(labelProductID.Text, "Ink", "", "", "", "", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "", "", 0, totalQuantity, totalQuantity, "", "", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "RawMaterialsConsumption", orderID, "");

        }

        //Cr Head id ki hobe, VoucherRefType ki hobe,
        VoucherEntry.AutoVoucherEntry("6", "Bill of Quantity Material Cost Invoice No.: " + orderID, "040201017", "010106001", grossTotalAmount, orderID, "0", Page.User.Identity.Name, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "1");

        //Cost of Goods sold
        //string amt = SQLQuery.ReturnString(@"SELECT   SUM(ConsumedAmount)  FROM            BOQConsumptionFinal
        //                        where SalesDetailsMasterId IN (Select  Id  from SaleDetails WHERE InvNo='" + orderID + "' )");
        //VoucherEntry.AutoVoucherEntry("6", "Sales Invoice No.: " + orderID, "040201017", "010106001", grossTotalAmount, orderID, "0", Page.User.Identity.Name, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "1");


        return grossTotalAmount;
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
        LoadSpecList("filter");
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
        ddGrade.DataBind();
        ddCategory.DataBind();
        ddProduct.DataBind();
        LoadFormControls();
        LoadSpecList("filter");
        QtyinStock();
    }


    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddCategory.DataBind();
        ddProduct.DataBind();
        LoadSpecList("filter");
        QtyinStock();
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddProduct.DataBind();
        LoadSpecList("filter");
        QtyinStock();
        ddCategory.Focus();
    }
    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSpecList("filter");
        QtyinStock();
        txtQty.Focus();
    }
    private void QtyinStock()
    {
        try
        {
            ltrUnit.Text = SQLQuery.ReturnString("Select UnitType FROM Products WHERE ProductID='" + ddProduct.SelectedValue + "'");

            ltrCQty.Text = "0";

            if (ddSubGroup.SelectedValue == "10") //printing ink
            {
                txtCurrentQty.Text = "0";
                txtCurrentKg.Text = Stock.Inventory.AvailableInkWeight(ddProduct.SelectedValue, ddSpec.SelectedValue, ddGodown.SelectedValue, ddLocation.SelectedValue);
                txtPrice.Visible = true;
                txtPrice.Text = Stock.Inventory.LastInkPrice(ddProduct.SelectedValue, ddSpec.SelectedValue);
                txtQty.Text = "0";
                txtWeight.Focus();
            }
            BindItemGrid();
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
        finally { LoadFormControls(); }

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

            Notify("Edit mode activated", "info", lblMsg2);
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }
    private void EditMode(string entryID)
    {
        SqlCommand cmd = new SqlCommand(@"SELECT Id, Purpose, SubGroup, Grade, Category, GodownID, LocationID, ProductID, Specification, CurrentStockQuantity, ConsumedWeight, EntryDate, EntryBy
        FROM StockDetailsInk WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            ddPurpose.SelectedValue = reader["Purpose"].ToString();
            ddGroup.DataBind();
            ddSubGroup.DataBind();
            ddSubGroup.SelectedValue = reader["SubGroup"].ToString();
            ddGrade.DataBind();
            ddGrade.SelectedValue = reader["Grade"].ToString();
            ddCategory.DataBind();
            ddCategory.SelectedValue = reader["Category"].ToString();
            ddProduct.DataBind();
            ddProduct.SelectedValue = reader["ProductID"].ToString();
            LoadFormControls();
            //txtRate.Text 
            txtWeight.Text = reader["CurrentStockQuantity"].ToString();
            ddGodown.DataBind();
            ddGodown.SelectedValue = reader["GodownID"].ToString();
            ddLocation.DataBind();
            ddLocation.SelectedValue = reader["LocationID"].ToString();
            //ltrCQty.Text = dr[8].ToString();
            //txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtRate.Text));
        }
        reader.Close();
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
        if (ddSubGroup.SelectedValue == "10")
        {
            pnlSpec.Visible = true;
        }
        else
        {
            pnlSpec.Visible = false;
        }
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

    protected void lbSpec_OnClick(object sender, EventArgs e)
    {
        if (lbSpec.Text == "New")
        {
            ddSpec.Visible = false;
            txtSpec.Visible = true;
            lbSpec.Text = "Cancel";
            txtSpec.Focus();
        }
        else
        {
            ddSpec.Visible = true;
            txtSpec.Visible = false;
            lbSpec.Text = "New";
            LoadSpecList("filter");
            ddSpec.Focus();
        }
        lbFilter.Text = "Show-all";
    }

    protected void ddSpec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
    }

    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        QtyinStock();
    }

    protected void GridView1_OnRowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        QtyinStock();
    }

    protected void lbFilter_OnClick(object sender, EventArgs e)
    {
        if (lbFilter.Text == "Show-all")
        {
            LoadSpecList("");
            //lbFilter.Text = "Filter";
        }
        else
        {
            LoadSpecList("filter");
            //lbFilter.Text = "Show-all";
        }

        lbSpec.Text = "New";
        ddSpec.Visible = true;
        txtSpec.Visible = false;
    }

    private void LoadSpecList(string filterDD)
    {
        string gQuery = "SELECT [id], [spec] FROM [Specifications] ORDER BY [spec]";
        lbFilter.Text = "Filter";
        if (filterDD != "")
        {
            lbFilter.Text = "Show-all";
            gQuery = "SELECT [id], [spec] FROM [Specifications] WHERE  CAST(id AS nvarchar) in (Select distinct spec from stock WHERE ProductID='" + ddProduct.SelectedValue + "') ORDER BY [spec]";
        }

        SQLQuery.PopulateDropDown(gQuery, ddSpec, "id", "spec");
        QtyinStock();
    }
}