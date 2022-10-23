﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accounting;
using RunQuery;

public partial class app_InkIssue : System.Web.UI.Page
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
            ddGrade.DataBind();
            ddCategory.DataBind();
            ddProduct.DataBind();

            ddGodown.DataBind();
            //ddLocation.DataBind();

            ddCustomer.DataBind();
            ddBrand.DataBind();
            ddSize.DataBind();
            ddColor.DataBind();
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

        SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (max(Id),0)+ 1 )) from InkIssueMaster where EntryDate>=@EntryDate", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
                SqlCommand cmde = new SqlCommand("SELECT ProductName FROM InkIssueDetails WHERE   GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '" + ddLocation.SelectedValue + "' AND ProductID ='" + ddProduct.SelectedValue + "' AND  OrderID ='' AND EntryBy ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmde.Connection.Open();
                string isExist = Convert.ToString(cmde.ExecuteScalar());
                cmde.Connection.Close();

                if (isExist == "" && ddProduct.SelectedValue != "")
                {
                    if (ddSubGroup.SelectedValue == "10")
                    {
                        string spec = ddSpec.SelectedValue;
                        if (txtSpec.Text != "" && lbSpec.Text == "Cancel" && ddSubGroup.SelectedValue == "10")//Insert Ink spec
                        {
                            isExist = SQLQuery.ReturnString("SELECT id FROM Specifications WHERE  spec ='" + txtSpec.Text + "'");
                            if (isExist == "")
                            {
                                SQLQuery.ExecNonQry("INSERT INTO Specifications (spec, EntryBy) VALUES ('" + txtSpec.Text + "', '" + lName + "') ");
                                spec = SQLQuery.ReturnString("SELECT MAX(id) FROM Specifications");
                                LoadSpecList(""); //ddSpec.DataBind();
                                ddSpec.SelectedValue = spec;
                            }
                            else
                            {
                                LoadSpecList("");
                                ddSpec.SelectedValue = isExist;
                            }
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
                SQLQuery.Empty2Zero(txtPrice);

                SQLQuery.ExecNonQry("Delete InkIssueDetails where (id ='" + lblOrderID.Text + "')");
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
        string productName = ddProduct.SelectedItem.Text;

        string spec = "";
        if (ddSubGroup.SelectedValue == "10")
        {
            spec = ddSpec.SelectedValue;
        }

        int balance = Convert.ToInt32(txtQty.Text) - Convert.ToInt32(ltrCQty.Text);
        SqlCommand cmd2 = new SqlCommand("INSERT INTO InkIssueDetails (StockType, purpose, ItemType, OrderID, ProductID, ProductName, Manufacturer, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy, CompanyFor, GodownID, LocationID, QtyBalance, DeliveredQty, Spec) " +
                                         "VALUES ('Raw', '" + ddPurpose.SelectedValue + "', 'Store', @OrderID, @ProductID, @ProductName, '" + txtDesc.Text + "',@UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, @CompanyFor, '" + ddGodown.SelectedValue + "', '" + ddLocation.SelectedValue + "','" + balance + "', '" + ltrCQty.Text + "', '" + spec + "'  )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        if (ddType.SelectedValue == "Printed Sheet")
        {
            cmd2 = new SqlCommand(@"INSERT INTO InkIssueDetails (StockType, purpose, ItemType, OrderID, SizeId, ProductID, Customer, BrandID, ProductName, Color, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy, CompanyFor, GodownID, LocationID, QtyBalance, DeliveredQty)" +
                                        " VALUES ('Raw', '" + ddPurpose.SelectedValue + "', 'Store', @OrderID, @SizeId, @ProductID, '" + ddCustomer.SelectedValue + "', @BrandID, @ProductName, '" + ddColor.SelectedValue + "', @UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, @CompanyFor, '" + ddGodown.SelectedValue + "', '" + ddLocation.SelectedValue + "','" + balance + "', '" + ltrCQty.Text + "'  )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }
        cmd2.Parameters.AddWithValue("@OrderID", "");
        cmd2.Parameters.AddWithValue("@SizeId", ddSize.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductID", ddProduct.SelectedValue);
        cmd2.Parameters.AddWithValue("@BrandID", ddBrand.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductName", productName);

        cmd2.Parameters.AddWithValue("@UnitCost", Convert.ToDecimal(txtPrice.Text));
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
        SQLQuery.Empty2Zero(txtWeight);
        SQLQuery.Empty2Zero(txtQty);
        string lName = Page.User.Identity.Name.ToString();
        string productName = ddProduct.SelectedItem.Text;

        int balance = Convert.ToInt32(txtQty.Text) - Convert.ToInt32(ltrCQty.Text);
        SqlCommand cmd2 = new SqlCommand("UPDATE InkIssueDetails SET SizeId='" + ddGroup.SelectedValue + "', ProductID='" + ddProduct.SelectedValue + "', BrandID='" + ddSubGroup.SelectedValue + "'," +
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
        //SqlCommand cmd = new SqlCommand(@"SELECT  Id, (SELECT  [Purpose] FROM [Purpose] WHERE [pid]= InkIssueDetails.Purpose) AS Purpose, 
        //                                    ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, QtyBalance, DeliveredQty, UnitWeight, 
        //                                    (SELECT  [Company] FROM [Party] WHERE [PartyID]= InkIssueDetails.Customer) AS Customer,
        //                                    (SELECT [BrandName] FROM [CustomerBrands] WHERE BrandID=InkIssueDetails.BrandID) AS BrandID,
        //                                    (SELECT [BrandName] FROM [Brands] WHERE BrandID=InkIssueDetails.SizeId) AS SizeId,
        //                                    (SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=InkIssueDetails.Color) AS Color,
        //                                    (SELECT [Spec] FROM [Specifications] WHERE id=InkIssueDetails.Spec) AS Spec
        //                                    FROM InkIssueDetails WHERE GodownID='" + ddGodown.SelectedValue + "' AND EntryBy=@EntryBy AND OrderID='' AND StockType='Raw'  ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        SqlCommand cmd = new SqlCommand(@"SELECT  Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, QtyBalance, DeliveredQty, UnitWeight, Manufacturer
                                            FROM InkIssueDetails WHERE GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '" + ddLocation.SelectedValue + "' AND EntryBy=@EntryBy AND OrderID='' AND StockType='Raw'  ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));


        cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        cmd.Connection.Open();
        ItemGrid.EmptyDataText = "No items to view...";
        ItemGrid.DataSource = cmd.ExecuteReader();
        ItemGrid.DataBind();
        cmd.Connection.Close();

    }


    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ltrQty.Text = SQLQuery.ReturnString("Select ISNULL(SUM(QtyBalance),0) from InkIssueDetails where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '" + ddLocation.SelectedValue + "'") + "pcs, " +
            SQLQuery.ReturnString("Select ISNULL(SUM(UnitWeight),0) from InkIssueDetails where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '" + ddLocation.SelectedValue + "'") + "kg";
        //QtyinStock();
    }
    protected void ItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;
            TextBox txtQty = ItemGrid.Rows[index].FindControl("txtQty") as TextBox;

            string OrderID = SQLQuery.ReturnString("Select OrderID from InkIssueDetails where Id='" + lblItemCode.Text + "'");
            //string deliveredQty = SQLQuery.ReturnString("Select SUM(DeliveredQty) from InkIssueDetails where OrderID='" + OrderID + "'");

            //if (deliveredQty == "0")
            //{
            SqlCommand cmd7 = new SqlCommand("DELETE InkIssueDetails WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
                    lblMsg.Text = "ERROR: <br> No items were added to the adjustment!";
                }
                else
                {
                    ExecuteInsert();
                    //ClearControls(Page);
                    txtQty.Text = "";
                    txtWeight.Text = "";

                    QtyinStock();
                    GridView1.DataBind();
                    Notify("Stock adjustment saved successfully...", "success", lblMsg);
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

        SqlCommand cmd2 = new SqlCommand("INSERT INTO InkIssueMaster (OrderID, OrderDate, GodownID, GodownName, LocationID, LocationName, ToGodownID, ToLocationID, Remarks, TotalQty, EntryBy, ProjectId)" +
                                                    " VALUES (@OrderID, @OrderDate, @GodownID, @GodownName, @LocationID, @LocationName, @ToGodownID, @ToLocationID, @Remarks, @TotalQty, @EntryBy, @ProjectId)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@OrderID", orderNo); //ddCategory.SelectedValue);
        cmd2.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        cmd2.Parameters.AddWithValue("@GodownID", ddFromGodown.SelectedValue);
        cmd2.Parameters.AddWithValue("@GodownName", ddFromGodown.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@LocationID", ddFromLocation.SelectedValue);
        cmd2.Parameters.AddWithValue("@LocationName", ddFromLocation.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@ToGodownID", ddGodown.SelectedValue);
        cmd2.Parameters.AddWithValue("@ToLocationID", ddLocation.SelectedValue);
        cmd2.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        cmd2.Parameters.AddWithValue("@TotalQty", ltrQty.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@ProjectId", lblProject.Text);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        SqlCommand cmd = new SqlCommand("UPDATE InkIssueDetails SET OrderID='" + orderNo + "' WHERE  EntryBy=@EntryBy AND OrderID=''  AND StockType='Raw'  AND GodownID='" + ddGodown.SelectedValue + "' AND LocationID = '" + ddLocation.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryBy", lName);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        //Sock-in

        foreach (GridViewRow row in ItemGrid.Rows)
        {
            Label lblEntryId = row.FindControl("lblEntryId") as Label;

            string purpose = SQLQuery.ReturnString("SELECT Purpose FROM InkIssueDetails where (Id='" + lblEntryId.Text + "')");
            string sizeId = SQLQuery.ReturnString("SELECT SizeID FROM InkIssueDetails where (Id='" + lblEntryId.Text + "')");
            string brandID = SQLQuery.ReturnString("SELECT brandID FROM InkIssueDetails where (Id='" + lblEntryId.Text + "')");
            string productID = SQLQuery.ReturnString("SELECT productID FROM InkIssueDetails where (Id='" + lblEntryId.Text + "')");
            string ProductName = SQLQuery.ReturnString("SELECT ProductName FROM InkIssueDetails where (Id='" + lblEntryId.Text + "')");
            string detail = SQLQuery.ReturnString("SELECT Manufacturer FROM InkIssueDetails where (Id='" + lblEntryId.Text + "')");

            string itemType = SQLQuery.ReturnString("SELECT ItemType FROM InkIssueDetails where (Id='" + lblEntryId.Text + "')");
            string customer = SQLQuery.ReturnString("SELECT Customer FROM InkIssueDetails where (Id='" + lblEntryId.Text + "')");
            string color = SQLQuery.ReturnString("SELECT Color FROM InkIssueDetails where (Id='" + lblEntryId.Text + "')");
            string spec = SQLQuery.ReturnString("SELECT spec FROM InkIssueDetails where (Id='" + lblEntryId.Text + "')");
            decimal price = Convert.ToDecimal(SQLQuery.ReturnString("SELECT UnitCost FROM InkIssueDetails where (Id='" + lblEntryId.Text + "')"));

            Label lbliQty = row.FindControl("lbliQty") as Label;
            Label lblUnitWeight = row.FindControl("lblUnitWeight") as Label;
            Label lblQtyBalance = row.FindControl("lblQtyBalance") as Label;

            int inQty = Convert.ToInt32(lbliQty.Text);
            decimal inWeight = Convert.ToDecimal(lblUnitWeight.Text);

            Stock.Inventory.SaveToStock(purpose, orderNo, detail, lblEntryId.Text, sizeId, customer, brandID, color, spec, productID, ProductName, itemType, ddGodown.SelectedValue, ddLocation.SelectedValue, ddGroup.SelectedValue, inQty, 0, price, inWeight, 0, "", "Stock-in", "Transfer", "Store", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), detail);
            Stock.Inventory.SaveToStock(purpose, orderNo, detail, lblEntryId.Text, sizeId, customer, brandID, color, spec, productID, ProductName, itemType, ddFromGodown.SelectedValue, ddFromLocation.SelectedValue, ddGroup.SelectedValue, 0, inQty, price, 0, inWeight, "", "Stock-out", "Transfer", "Store", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), detail);


            //Accounting.VoucherEntry.StockEntry(invNo, "Purchase", entryId, size, "", iCode, iName, ddGodown.SelectedValue, ddLocation.SelectedValue, iGrp, qty, "0", "0", "0", guaranty, "Purchase", stockType, stockLocation, lName);
            //Consumed Processed items stock-out
            //string companyFor = SQLQuery.ReturnString("SELECT Company from Party where PartyID=(Select CompanyFor FROM InkIssueDetails where (Id='" + lblEntryId.Text + "'))");

            //DataTable citydt = SQLQuery.ReturnDataTable("Select RawItemCode, RawItemName, RequiredQuantity, Wastage, UnitType from Ingradiants where ItemCode='" + productID + "'");

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
        int masterIDMax = Convert.ToInt32("SELECT ISNULL(MAX(Id), 0) AS MaxId FROM InkIssueMaster WHERE OrderID <> ''");
        InsertCostOfGoodsSoldData(masterIDMax, orderNo);
    }
    protected decimal InsertCostOfGoodsSoldData(int masterTableId, string orderID)
    {
        string lName = Page.User.Identity.Name;
        decimal grossTotalAmount = 0;
        foreach (GridViewRow row in ItemGrid.Rows)
        {
            Label labelProductId = row.FindControl("labelProductID") as Label;
            Label labelProductName = row.FindControl("labelProductName") as Label;
            Label labelQuantity = row.FindControl("lbliQty") as Label;
            //TextBox txtRate = row.FindControl("txtPrice") as TextBox;
            //TextBox txtExcessQuantity = row.FindControl("txtExcessQuantity") as TextBox;
            //decimal totalQuantity = Convert.ToDecimal(labelQuantity.Text) + Convert.ToDecimal(txtExcessQuantity.Text);
            decimal consumedAmount = Convert.ToDecimal(labelQuantity.Text);
            grossTotalAmount += consumedAmount;
            //TextBox txtRemarks = row.FindControl("txtRemarks") as TextBox;

            //Need to defice intype
            string inKg = SQLQuery.ReturnString("SELECT InKg FROM tblFifo WHERE ItemCode = '" + labelProductId.Text + "' AND (InType = 'RawMaterials')");
            decimal unitWeight = Convert.ToDecimal(inKg);

            SqlCommand cmd = new SqlCommand("INSERT INTO BOQConsumptionFinal (SalesDetailsMasterId, ComponentId, ComponentName, DeliveredQuantity, Rate, ExcessQuantity, TotalQuantity, TotalWeight, ConsumedAmount, Remarks, EntryBy, EntryDate)" +
                                            " VALUES (@SalesDetailsMasterId, @ComponentId, @ComponentName, @DeliveredQuantity, @Rate, @ExcessQuantity, @TotalQuantity, @TotalWeight, @ConsumedAmount, @Remarks, @EntryBy, @EntryDate)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd.Parameters.AddWithValue("@SalesDetailsMasterId", masterTableId);
            cmd.Parameters.AddWithValue("@ComponentId", Convert.ToDecimal(labelProductId.Text));
            cmd.Parameters.AddWithValue("@ComponentName", labelProductName.Text);
            cmd.Parameters.AddWithValue("@DeliveredQuantity", Convert.ToDecimal(labelQuantity.Text));
            cmd.Parameters.AddWithValue("@Rate", "");
            cmd.Parameters.AddWithValue("@ExcessQuantity", "");
            cmd.Parameters.AddWithValue("@TotalQuantity", consumedAmount);
            cmd.Parameters.AddWithValue("@TotalWeight", unitWeight * consumedAmount);
            cmd.Parameters.AddWithValue("@ConsumedAmount", consumedAmount);
            cmd.Parameters.AddWithValue("@Remarks", "");
            cmd.Parameters.AddWithValue("@EntryBy", lName);
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            //Stock.Inventory.FifoInsert(labelProductId.Text, "RawMaterialsConsumption", ddCustomer.SelectedValue, "", "", "", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "", "", 0, consumedAmount, totalQuantity, "", "", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "RawMaterialsConsumption", orderID, "");

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
        SqlCommand cmd = new SqlCommand("Delete InkIssueDetails WHERE EntryBy=@EntryBy AND OrderID=''  AND WarehouseID='" + ddGodown.SelectedValue + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
            ltrUnit.Text = SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");

            ltrCQty.Text = "0";

            if (ddSubGroup.SelectedValue == "10") //printing ink
            {
                txtCurrentQty.Text = "0";
                txtCurrentKg.Text = Stock.Inventory.AvailableInkWeight(ddProduct.SelectedValue, ddSpec.SelectedValue, ddFromGodown.SelectedValue, ddFromLocation.SelectedValue);
                txtPrice.Visible = true;
                txtPrice.Text = Stock.Inventory.LastInkPrice(ddProduct.SelectedValue, ddSpec.SelectedValue);
                txtQty.Text = "0";
                txtWeight.Focus();
            }
            else if (ddSubGroup.SelectedValue == "33" || ddSubGroup.SelectedValue == "35") //HTF & IML
            {
                txtCurrentQty.Text = "0";
                decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.QtyinStock(ddPurpose.SelectedValue, ddProduct.SelectedValue, ddFromGodown.SelectedValue, ddFromLocation.SelectedValue));

                txtCurrentKg.Text = "0";// inputAvailable.ToString();
                txtPrice.Visible = true;
                txtPrice.Text = Stock.Inventory.LastPlasticRawPrice(ddPurpose.SelectedValue, ddProduct.SelectedValue);
                txtCurrentQty.Text = inputAvailable.ToString();
                txtWeight.Focus();
            }
            else if (ddSubGroup.SelectedValue != "9") //All items except Tin Plates
            {
                txtCurrentQty.Text = "0";
                txtCurrentKg.Text = Stock.Inventory.PlasticRawWeight(ddPurpose.SelectedValue, ddProduct.SelectedValue, ddFromGodown.SelectedValue, ddFromLocation.SelectedValue);
                txtPrice.Visible = true;
                txtPrice.Text = Stock.Inventory.LastPlasticRawPrice(ddProduct.SelectedValue, ddSpec.SelectedValue);
                txtQty.Text = "0";
                txtWeight.Focus();
            }
            else
            {
                if (ddType.SelectedValue == "Printed Sheet")
                {
                    txtCurrentQty.Text = Stock.Inventory.AvailablePrintedQty(ddPurpose.SelectedValue, ddType.SelectedValue, ddProduct.SelectedValue,
                        ddCustomer.SelectedValue, ddBrand.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddFromGodown.SelectedValue, ddFromLocation.SelectedValue);

                    ltrCQty.Text = txtCurrentQty.Text;

                    txtCurrentKg.Text = Stock.Inventory.AvailablePrintedWeight(ddPurpose.SelectedValue, ddType.SelectedValue, ddProduct.SelectedValue,
                        ddCustomer.SelectedValue, ddBrand.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddFromGodown.SelectedValue, ddFromLocation.SelectedValue);
                    //txtPrice.Visible = false;
                }
                else
                {
                    txtCurrentQty.Text = Stock.Inventory.AvailableNonPrintedQty(ddPurpose.SelectedValue, ddType.SelectedValue, ddProduct.SelectedValue,
                        ddFromGodown.SelectedValue, ddFromLocation.SelectedValue);

                    ltrCQty.Text = txtCurrentQty.Text;
                    txtCurrentKg.Text = Stock.Inventory.AvailableNonPrintedWeight(ddPurpose.SelectedValue, ddType.SelectedValue, ddProduct.SelectedValue, ddFromGodown.SelectedValue, ddFromLocation.SelectedValue);
                    txtPrice.Visible = true;
                    txtPrice.Text = Stock.Inventory.LastNonprintedPrice(ddPurpose.SelectedValue, ddType.SelectedValue, ddProduct.SelectedValue);
                }
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
        SqlCommand cmd = new SqlCommand(@"SELECT ItemType, ProductID, SizeId, Customer, BrandID, Color, Quantity, UnitWeight, GodownID, LocationID, Manufacturer, UnitCost FROM [InkIssueDetails] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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

            txtPrice.Text = dr[10].ToString();
            txtQty.Text = dr[6].ToString();
            txtWeight.Text = dr[7].ToString();
            txtDesc.Text = dr[9].ToString();
            //ltrCQty.Text = dr[8].ToString();
            //txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtRate.Text));
        }
        cmd.Connection.Close();

    }
    protected void ddGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddLocation.DataBind();
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

        if (ddSubGroup.SelectedValue == "9")
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
            gQuery = "SELECT [id], [spec] FROM [Specifications] where  CAST(id AS nvarchar) in (Select distinct spec from stock where ProductID='" + ddProduct.SelectedValue + "') ORDER BY [spec]";
        }

        SQLQuery.PopulateDropDown(gQuery, ddSpec, "id", "spec");
        QtyinStock();
    }
}