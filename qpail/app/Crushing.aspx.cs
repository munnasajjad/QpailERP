using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Crushing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");
        btnAddOut.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAddOut, null) + ";");
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
            ddPurposeOut.DataBind();
            ddPurpose.SelectedValue = "1";
            ddPurposeOut.SelectedValue = "1";

            ddGroup.DataBind();
            ddGroupOut.DataBind();
            ddGroup.SelectedValue = "1";
            ddGroupOut.SelectedValue = "1";
            ddSubGroup.DataBind();
            ddSubgroupOut.DataBind();
            ddGrade.DataBind();
            ddGradeOut.DataBind();

            ddCategory.DataBind();
            ddCategoryOut.DataBind();
            ddProduct.DataBind();
            ddProductOut.DataBind();

            ddGodown.DataBind();
            //ddLocation.DataBind();

            ddCustomer.DataBind();
            ddBrand.DataBind();
            ddSize.DataBind();
            ddColor.DataBind();
            LoadSpecList("filter");
            LoadSpecListOut("filter");

            //LoadSummesion("");
            LoadFormControls();
            LoadFormControlsOut();
            QtyinStock();
            QtyinStockOut();
            BindItemGrid();
            BindItemOutGridView();
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

        SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (max(Id),0)+ 1 )) from CrushingMaster where OrderDate>=@OrderDate", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@OrderDate", countForYear);
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
                SqlCommand cmde = new SqlCommand("SELECT ProductName FROM Crushing WHERE   GodownID='" + ddGodown.SelectedValue + "' AND ProductID ='" + ddProduct.SelectedValue + "' AND  OrderID ='' AND EntryBy ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmde.Connection.Open();
                string isExist
                    = Convert.ToString(cmde.ExecuteScalar());
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

                    AddData();
                    BindItemGrid();

                    txtQty.Text = "";
                    //txtWeight.Text = "";
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

                SQLQuery.ExecNonQry("Delete Crushing where (id ='" + lblOrderID.Text + "')");
                AddData();
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
    protected void btnAddOut_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnAddOut.Text == "Add")
            {
                string lName = Page.User.Identity.Name.ToString();
                if (txtQtyOut.Text == "")
                {
                    txtQtyOut.Text = "0";
                }
                SQLQuery.Empty2Zero(txtPrice);

                //SizeId, ProductID, BrandID
                SqlCommand cmde = new SqlCommand("SELECT ProductName FROM Crushing WHERE   GodownID='" + ddGodown.SelectedValue + "' AND ProductID ='" + ddProductOut.SelectedValue + "' AND  OrderID ='' AND EntryBy ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmde.Connection.Open();
                string isExist = Convert.ToString(cmde.ExecuteScalar());
                cmde.Connection.Close();

                if (isExist == "" && ddProductOut.SelectedValue != "")
                {
                    if (ddSubgroupOut.SelectedValue == "10")
                    {
                        string spec = ddSpecOut.SelectedValue;
                        if (txtSpecOut.Text != "" && lbSpec.Text == "Cancel" && ddSubgroupOut.SelectedValue == "10")//Insert Ink spec
                        {
                            isExist = SQLQuery.ReturnString("SELECT id FROM Specifications WHERE  spec ='" + txtSpecOut.Text + "'");
                            if (isExist == "")
                            {
                                SQLQuery.ExecNonQry("INSERT INTO Specifications (spec, EntryBy) VALUES ('" + txtSpecOut.Text + "', '" + lName + "') ");
                                spec = SQLQuery.ReturnString("SELECT MAX(id) FROM Specifications");
                                LoadSpecListOut(""); //ddSpec.DataBind();
                                ddSpecOut.SelectedValue = spec;
                            }
                            else
                            {
                                LoadSpecListOut("");
                                ddSpecOut.SelectedValue = isExist;
                            }
                        }
                    }
                    //else
                    //{
                    //InsertData();
                    InsertItemOutData();
                    //}

                    txtQtyOut.Text = "";
                    //txtWeightOut.Text = "";
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

                SQLQuery.ExecNonQry("Delete Crushing where (id ='" + lblOrderID.Text + "')");
                //InsertData();
                InsertItemOutData();
                btnAddOut.Text = "Add";
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
            BindItemOutGridView();
        }
    }
    private void AddData()
    {
        string lName = Page.User.Identity.Name.ToString();
        string productName = ddProduct.SelectedItem.Text;

        string spec = "";
        if (ddSubGroup.SelectedValue == "10")
        {
            spec = ddSpec.SelectedValue;
        }

        int balance = Convert.ToInt32(txtQty.Text) - Convert.ToInt32(ltrCQty.Text);
        SqlCommand cmd2 = new SqlCommand("INSERT INTO Crushing (StockType, purpose, ItemType, OrderID, ProductID, ProductName, Manufacturer, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy, CompanyFor,  GodownID, QtyBalance, DeliveredQty, Spec) " +
                                         "VALUES ('Input', '" + ddPurpose.SelectedValue + "', 'Wastage', @OrderID, @ProductID, @ProductName, '" + txtDesc.Text + "',@UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, @CompanyFor, '" + ddGodown.SelectedValue + "', '" + balance + "', '" + ltrCQty.Text + "', '" + spec + "'  )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        if (ddType.SelectedValue == "Printed Sheet")
        {
            cmd2 = new SqlCommand(@"INSERT INTO Crushing (StockType, purpose, ItemType, OrderID, SizeId, ProductID, Customer, BrandID, ProductName, Color, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy, CompanyFor,  GodownID, QtyBalance, DeliveredQty)" +
                                        " VALUES ('Raw', '" + ddPurpose.SelectedValue + "', 'Store', @OrderID, @SizeId, @ProductID, '" + ddCustomer.SelectedValue + "', @BrandID, @ProductName, '" + ddColor.SelectedValue + "', @UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, @CompanyFor, '" + ddGodown.SelectedValue + "', '" + balance + "', '" + ltrCQty.Text + "'  )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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

    private void InsertItemOutData()
    {
        string lName = Page.User.Identity.Name.ToString();
        string productName = ddProductOut.SelectedItem.Text;

        string spec = "";
        if (ddSubgroupOut.SelectedValue == "10")
        {
            spec = ddSpecOut.SelectedValue;
        }
        ltrCQtyOut.Text = "0";
        decimal balance = Convert.ToDecimal(txtQtyOut.Text) - Convert.ToDecimal(ltrCQtyOut.Text);
        SqlCommand cmd2 = new SqlCommand("INSERT INTO Crushing (StockType, purpose, ItemType, OrderID, ProductID, ProductName, Manufacturer, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy, CompanyFor,  GodownID, QtyBalance, DeliveredQty, Spec) " +
                                         "VALUES ('Output', '" + ddPurposeOut.SelectedValue + "', 'Wastage', @OrderID, @ProductID, @ProductName, '" + txtDesc.Text + "',@UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, @CompanyFor, '" + ddGodown.SelectedValue + "', '" + balance + "', '" + ltrCQtyOut.Text + "', '" + spec + "'  )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        if (ddTypeOut.SelectedValue == "Printed Sheet")
        {
            cmd2 = new SqlCommand(@"INSERT INTO Crushing (StockType, purpose, ItemType, OrderID, SizeId, ProductID, Customer, BrandID, ProductName, Color, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy, CompanyFor,  GodownID, QtyBalance, DeliveredQty)" +
                                        " VALUES ('Output', '" + ddPurposeOut.SelectedValue + "', 'Wastage', @OrderID, @SizeId, @ProductID, '" + ddCustomer.SelectedValue + "', @BrandID, @ProductName, '" + ddColor.SelectedValue + "', @UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, @CompanyFor, '" + ddGodown.SelectedValue + "', '" + balance + "', '" + ltrCQtyOut.Text + "'  )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }
        cmd2.Parameters.AddWithValue("@OrderID", "");
        cmd2.Parameters.AddWithValue("@SizeId", ddSize.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductID", ddProductOut.SelectedValue);
        cmd2.Parameters.AddWithValue("@BrandID", ddBrand.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductName", productName);

        cmd2.Parameters.AddWithValue("@UnitCost", Convert.ToDecimal(txtPrice.Text));
        cmd2.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(txtQtyOut.Text));
        cmd2.Parameters.AddWithValue("@UnitWeight", txtWeightOut.Text);

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
        SqlCommand cmd2 = new SqlCommand("UPDATE Crushing SET SizeId='" + ddGroup.SelectedValue + "', ProductID='" + ddProduct.SelectedValue + "', BrandID='" + ddSubGroup.SelectedValue + "'," +
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
        //SqlCommand cmd = new SqlCommand(@"SELECT  Id, (SELECT  [Purpose] FROM [Purpose] WHERE [pid]= Crushing.Purpose) AS Purpose, 
        //                                    ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, QtyBalance, DeliveredQty, UnitWeight, 
        //                                    (SELECT  [Company] FROM [Party] WHERE [PartyID]= Crushing.Customer) AS Customer,
        //                                    (SELECT [BrandName] FROM [CustomerBrands] WHERE BrandID=Crushing.BrandID) AS BrandID,
        //                                    (SELECT [BrandName] FROM [Brands] WHERE BrandID=Crushing.SizeId) AS SizeId,
        //                                    (SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=Crushing.Color) AS Color,
        //                                    (SELECT [Spec] FROM [Specifications] WHERE id=Crushing.Spec) AS Spec
        //                                    FROM Crushing WHERE GodownID='" + ddGodown.SelectedValue + "' AND EntryBy=@EntryBy AND OrderID='' AND StockType='Raw'  ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        SqlCommand cmd = new SqlCommand(@"SELECT  Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, QtyBalance, DeliveredQty, UnitWeight, Manufacturer
                                            FROM Crushing WHERE GodownID='" + ddGodown.SelectedValue + "' AND EntryBy=@EntryBy AND OrderID=''   ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));


        cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        cmd.Connection.Open();
        ItemGrid.EmptyDataText = "No items to view...";
        ItemGrid.DataSource = cmd.ExecuteReader();
        ItemGrid.DataBind();
        cmd.Connection.Close();

    }
    private void BindItemOutGridView()
    {
        string lName = Page.User.Identity.Name.ToString();
        //SqlCommand cmd = new SqlCommand(@"SELECT  Id, (SELECT  [Purpose] FROM [Purpose] WHERE [pid]= Crushing.Purpose) AS Purpose, 
        //                                    ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, QtyBalance, DeliveredQty, UnitWeight, 
        //                                    (SELECT  [Company] FROM [Party] WHERE [PartyID]= Crushing.Customer) AS Customer,
        //                                    (SELECT [BrandName] FROM [CustomerBrands] WHERE BrandID=Crushing.BrandID) AS BrandID,
        //                                    (SELECT [BrandName] FROM [Brands] WHERE BrandID=Crushing.SizeId) AS SizeId,
        //                                    (SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=Crushing.Color) AS Color,
        //                                    (SELECT [Spec] FROM [Specifications] WHERE id=Crushing.Spec) AS Spec
        //                                    FROM Crushing WHERE GodownID='" + ddGodown.SelectedValue + "' AND EntryBy=@EntryBy AND OrderID='' AND StockType='Raw'  ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        SqlCommand cmd = new SqlCommand(@"SELECT  Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, QtyBalance, DeliveredQty, UnitWeight, Manufacturer
                                            FROM Crushing WHERE StockType='Output'  ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));


        cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        cmd.Connection.Open();
        ItemOutGridView.EmptyDataText = "No items to view...";
        ItemOutGridView.DataSource = cmd.ExecuteReader();
        ItemOutGridView.DataBind();
        cmd.Connection.Close();

    }


    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ltrQty.Text = SQLQuery.ReturnString("Select ISNULL(SUM(QtyBalance),0) from Crushing where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "pcs, " +
            SQLQuery.ReturnString("Select ISNULL(SUM(UnitWeight),0) from Crushing where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "kg";
        //QtyinStock();
    }
    protected void ItemOutGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ltrQtyOut.Text = SQLQuery.ReturnString("Select ISNULL(SUM(QtyBalance),0) from Crushing where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "pcs, " +
            SQLQuery.ReturnString("Select ISNULL(SUM(UnitWeight),0) from Crushing where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "kg";
        //QtyinStock();
    }
    protected void ItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = ItemGrid.Rows[index].FindControl("lblEntryId") as Label;
            TextBox txtQty = ItemGrid.Rows[index].FindControl("txtQty") as TextBox;

            string OrderID = SQLQuery.ReturnString("Select OrderID from Crushing where Id='" + lblItemCode.Text + "'");
            //string deliveredQty = SQLQuery.ReturnString("Select SUM(DeliveredQty) from Crushing where OrderID='" + OrderID + "'");

            //if (deliveredQty == "0")
            //{
            SqlCommand cmd7 = new SqlCommand("DELETE Crushing WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
    protected void ItemOutGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = ItemOutGridView.Rows[index].FindControl("lblEntryOutId") as Label;
            TextBox txtQty = ItemOutGridView.Rows[index].FindControl("txtQty") as TextBox;

            string OrderID = SQLQuery.ReturnString("Select OrderID from Crushing where Id='" + lblItemCode.Text + "'");
            //string deliveredQty = SQLQuery.ReturnString("Select SUM(DeliveredQty) from Crushing where OrderID='" + OrderID + "'");

            //if (deliveredQty == "0")
            //{
            SqlCommand cmd7 = new SqlCommand("DELETE Crushing WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();

            BindItemOutGridView();
            Notify("Item deleted from order ...", "success", lblMsg2);

            //}
            //else
            //{
            //    lblMsg2.Attributes.Add("class", "xerp_warning");
            //    lblMsg2.Text = "Order is Locked! There is some pending delivery...";
            //}
            btnAddOut.Text = "Add";

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

        if (txtWeight.Text == txtWeightOut.Text)
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
                        QtyinStockOut();
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
                Notify(ex.ToString(), "error", lblMsg);
            }
            finally
            {
                BindItemGrid();
                BindItemOutGridView();
            }
        }
        else
        {
            btnSave.Enabled = false;
        }
        
    }

    private void ExecuteInsert()
    {
        string orderNo = InvIDNo();
        string lName = Page.User.Identity.Name.ToString();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO CrushingMaster (OrderID, OrderDate, Remarks, EntryBy)" +
                                                    " VALUES (@OrderID, @OrderDate, @Remarks, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@OrderID", orderNo); //ddCategory.SelectedValue);
        cmd2.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd")); 
        cmd2.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
       

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        SqlCommand cmd = new SqlCommand("UPDATE Crushing SET OrderID='" + orderNo + "' WHERE  EntryBy=@EntryBy AND OrderID=''  ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryBy", lName);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        //Sock-in
        foreach (GridViewRow row in ItemGrid.Rows)
        {
            Label lblEntryId = row.FindControl("lblEntryId") as Label;

            string purpose = SQLQuery.ReturnString("SELECT Purpose FROM Crushing where (Id='" + lblEntryId.Text + "')");
            string sizeId = SQLQuery.ReturnString("SELECT SizeID FROM Crushing where (Id='" + lblEntryId.Text + "')");
            string brandID = SQLQuery.ReturnString("SELECT brandID FROM Crushing where (Id='" + lblEntryId.Text + "')");
            string productID = SQLQuery.ReturnString("SELECT productID FROM Crushing where (Id='" + lblEntryId.Text + "')");
            string ProductName = SQLQuery.ReturnString("SELECT ProductName FROM Crushing where (Id='" + lblEntryId.Text + "')");
            string detail = SQLQuery.ReturnString("SELECT Manufacturer FROM Crushing where (Id='" + lblEntryId.Text + "')");

            string itemType = SQLQuery.ReturnString("SELECT ItemType FROM Crushing where (Id='" + lblEntryId.Text + "')");
            string customer = SQLQuery.ReturnString("SELECT Customer FROM Crushing where (Id='" + lblEntryId.Text + "')");
            string color = SQLQuery.ReturnString("SELECT Color FROM Crushing where (Id='" + lblEntryId.Text + "')");
            string spec = SQLQuery.ReturnString("SELECT spec FROM Crushing where (Id='" + lblEntryId.Text + "')");
            decimal price = Convert.ToDecimal(SQLQuery.ReturnString("SELECT UnitCost FROM Crushing where (Id='" + lblEntryId.Text + "')"));

            Label lbliQty = row.FindControl("lbliQty") as Label;
            Label lblUnitWeight = row.FindControl("lblUnitWeight") as Label;
            Label lblQtyBalance = row.FindControl("lblQtyBalance") as Label;

            int inQty = Convert.ToInt32(Convert.ToDecimal(lbliQty.Text));
            decimal inWeight = Convert.ToDecimal(lblUnitWeight.Text);

            Stock.Inventory.SaveToStock(purpose, orderNo, detail, lblEntryId.Text, sizeId, customer, brandID, color, spec, productID, ProductName, itemType, ddGodown.SelectedValue, ddLocation.SelectedValue, ddGroup.SelectedValue, inQty, 0, price, inWeight, 0, "", "Stock-in", "Transfer", "Store", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), detail);
            Stock.Inventory.SaveToStock(purpose, orderNo, detail, lblEntryId.Text, sizeId, customer, brandID, color, spec, productID, ProductName, itemType, ddOutGodown.SelectedValue, "0", ddGroup.SelectedValue, 0, inQty, price, 0, inWeight, "", "Stock-out", "Transfer", "Store", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), detail);


            //Accounting.VoucherEntry.StockEntry(invNo, "Purchase", entryId, size, "", iCode, iName, ddGodown.SelectedValue, ddLocation.SelectedValue, iGrp, qty, "0", "0", "0", guaranty, "Purchase", stockType, stockLocation, lName);
            //Consumed Processed items stock-out
            //string companyFor = SQLQuery.ReturnString("SELECT Company from Party where PartyID=(Select CompanyFor FROM Crushing where (Id='" + lblEntryId.Text + "'))");

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
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("Delete Crushing WHERE EntryBy=@EntryBy AND OrderID=''  AND WarehouseID='" + ddGodown.SelectedValue + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
    protected void ddGroupOut_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddSubgroupOut.DataBind();
        ddGradeOut.DataBind();

        ddCategoryOut.DataBind();
        ddProductOut.DataBind();
        LoadSpecListOut("filter");
        QtyinStockOut();
        //if (ddGroup.SelectedValue == "1")
        //{

        //}

        if (ddGroupOut.SelectedValue == "5")
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
        LoadFormControlsOut();
        LoadSpecList("filter");
        QtyinStock();
    }
    protected void ddSubGroupOut_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddGradeOut.DataBind();
        ddCategoryOut.DataBind();
        ddProductOut.DataBind();
        LoadFormControlsOut();
        LoadSpecListOut("filter");
        QtyinStockOut();
    }

    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddCategory.DataBind();
        ddProduct.DataBind();
        LoadSpecList("filter");
        QtyinStock();
    }
    protected void ddGradeOut_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddCategory.DataBind();
        ddProduct.DataBind();
        LoadSpecListOut("filter");
        QtyinStockOut();
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddProduct.DataBind();
        LoadSpecList("filter");
        QtyinStock();
        ddCategory.Focus();
    }
    protected void ddCategoryOut_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddProductOut.DataBind();
        LoadSpecListOut("filter");
        QtyinStockOut();
        ddCategoryOut.Focus();
    }

    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSpecList("filter");
        QtyinStock();
        txtQty.Focus();
    }
    protected void ddProductOut_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSpecListOut("filter");
        QtyinStockOut();
        txtQtyOut.Focus();
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
                txtCurrentKg.Text = Stock.Inventory.AvailableInkWeight(ddProduct.SelectedValue, ddSpec.SelectedValue, ddOutGodown.SelectedValue, ddOutLocation.SelectedValue);
                txtPrice.Visible = true;
                txtPrice.Text = Stock.Inventory.LastInkPrice(ddProduct.SelectedValue, ddSpec.SelectedValue);
                txtQty.Text = "0";
                txtWeight.Focus();
            }
            else if (ddSubGroup.SelectedValue == "33" || ddSubGroup.SelectedValue == "35") //HTF & IML
            {
                txtCurrentQty.Text = "0";
                decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.QtyinStock(ddPurpose.SelectedValue, ddProduct.SelectedValue, ddOutGodown.SelectedValue, ddOutLocation.SelectedValue));

                txtCurrentKg.Text = "0";// inputAvailable.ToString();
                txtPrice.Visible = true;
                txtPrice.Text = Stock.Inventory.LastPlasticRawPrice(ddPurpose.SelectedValue, ddProduct.SelectedValue);
                txtCurrentQty.Text = inputAvailable.ToString();
                txtWeight.Focus();
            }
            else if (ddSubGroup.SelectedValue != "9") //All items except Tin Plates
            {
                txtCurrentQty.Text = "0";
                txtCurrentKg.Text = Stock.Inventory.PlasticRawWeight(ddPurpose.SelectedValue, ddProduct.SelectedValue, ddOutGodown.SelectedValue, ddOutLocation.SelectedValue);
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
                        ddCustomer.SelectedValue, ddBrand.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddOutGodown.SelectedValue, ddOutLocation.SelectedValue);

                    ltrCQty.Text = txtCurrentQty.Text;

                    txtCurrentKg.Text = Stock.Inventory.AvailablePrintedWeight(ddPurpose.SelectedValue, ddType.SelectedValue, ddProduct.SelectedValue,
                        ddCustomer.SelectedValue, ddBrand.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddOutGodown.SelectedValue, ddOutLocation.SelectedValue);
                    //txtPrice.Visible = false;
                }
                else
                {
                    txtCurrentQty.Text = Stock.Inventory.AvailableNonPrintedQty(ddPurpose.SelectedValue, ddType.SelectedValue, ddProduct.SelectedValue,
                        ddOutGodown.SelectedValue, ddOutLocation.SelectedValue);

                    ltrCQty.Text = txtCurrentQty.Text;
                    txtCurrentKg.Text = Stock.Inventory.AvailableNonPrintedWeight(ddPurpose.SelectedValue, ddType.SelectedValue, ddProduct.SelectedValue, ddOutGodown.SelectedValue, ddOutLocation.SelectedValue);
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
    private void QtyinStockOut()
    {
        try
        {
            ltrUnitOut.Text = SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");

            ltrCQtyOut.Text = "0";

            if (ddSubgroupOut.SelectedValue == "10") //printing ink
            {
                txtCurrentQtyOut.Text = "0";
                txtCurrentWeightOut.Text = Stock.Inventory.AvailableInkWeight(ddProduct.SelectedValue, ddSpec.SelectedValue, ddOutGodown.SelectedValue, ddOutLocation.SelectedValue);
                txtPriceOut.Visible = true;
                txtPriceOut.Text = Stock.Inventory.LastInkPrice(ddProduct.SelectedValue, ddSpec.SelectedValue);
                txtQtyOut.Text = "0";
                txtWeightOut.Focus();
            }
            else if (ddSubgroupOut.SelectedValue == "33" || ddSubgroupOut.SelectedValue == "35") //HTF & IML
            {
                txtCurrentQtyOut.Text = "0";
                decimal inputAvailable = Convert.ToDecimal(Stock.Inventory.QtyinStock(ddPurposeOut.SelectedValue, ddProductOut.SelectedValue, ddOutGodown.SelectedValue, ddOutLocation.SelectedValue));

                txtCurrentWeightOut.Text = "0";// inputAvailable.ToString();
                txtPriceOut.Visible = true;
                txtPriceOut.Text = Stock.Inventory.LastPlasticRawPrice(ddPurpose.SelectedValue, ddProduct.SelectedValue);
                txtCurrentQtyOut.Text = inputAvailable.ToString();
                txtWeightOut.Focus();
            }
            else if (ddSubgroupOut.SelectedValue != "9") //All items except Tin Plates
            {
                txtCurrentQtyOut.Text = "0";
                txtCurrentWeightOut.Text = Stock.Inventory.PlasticRawWeight(ddPurpose.SelectedValue, ddProduct.SelectedValue, ddOutGodown.SelectedValue, ddOutLocation.SelectedValue);
                txtPriceOut.Visible = true;
                txtPriceOut.Text = Stock.Inventory.LastPlasticRawPrice(ddProduct.SelectedValue, ddSpec.SelectedValue);
                txtQtyOut.Text = "0";
                txtWeightOut.Focus();
            }
            else
            {
                if (ddTypeOut.SelectedValue == "Printed Sheet")
                {
                    txtCurrentQtyOut.Text = Stock.Inventory.AvailablePrintedQty(ddPurposeOut.SelectedValue, ddTypeOut.SelectedValue, ddProductOut.SelectedValue,
                        ddCustomerOut.SelectedValue, ddBrandOut.SelectedValue, ddSizeOut.SelectedValue, ddColorOut.SelectedValue, ddOutGodown.SelectedValue, ddOutLocation.SelectedValue);

                    ltrCQtyOut.Text = txtCurrentQtyOut.Text;

                    txtCurrentWeightOut.Text = Stock.Inventory.AvailablePrintedWeight(ddPurposeOut.SelectedValue, ddTypeOut.SelectedValue, ddProductOut.SelectedValue,
                        ddCustomerOut.SelectedValue, ddBrandOut.SelectedValue, ddSizeOut.SelectedValue, ddColorOut.SelectedValue, ddOutGodown.SelectedValue, ddOutLocation.SelectedValue);
                    //txtPrice.Visible = false;
                }
                else
                {
                    txtCurrentQtyOut.Text = Stock.Inventory.AvailableNonPrintedQty(ddPurposeOut.SelectedValue, ddTypeOut.SelectedValue, ddProductOut.SelectedValue,
                        ddOutGodown.SelectedValue, ddOutLocation.SelectedValue);

                    ltrCQtyOut.Text = txtCurrentQtyOut.Text;
                    txtCurrentWeightOut.Text = Stock.Inventory.AvailableNonPrintedWeight(ddPurposeOut.SelectedValue, ddTypeOut.SelectedValue, ddProductOut.SelectedValue, ddOutGodown.SelectedValue, ddOutLocation.SelectedValue);
                    txtPriceOut.Visible = true;
                    txtPriceOut.Text = Stock.Inventory.LastNonprintedPrice(ddPurposeOut.SelectedValue, ddTypeOut.SelectedValue, ddProductOut.SelectedValue);
                }
            }

            BindItemOutGridView();
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
        finally { LoadFormControlsOut(); }

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
    protected void ItemOutGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //DropDownList1.DataBind();
            int index = Convert.ToInt32(ItemOutGridView.SelectedIndex);
            Label lblItemName = ItemOutGridView.Rows[index].FindControl("lblEntryOutId") as Label;
            lblOrderID.Text = lblItemName.Text;
            EditModeOut(lblItemName.Text);
            btnAddOut.Text = "Update";

            Notify("Edit mode activated", "info", lblMsg2);
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    private void EditMode(string entryID)
    {
        SqlCommand cmd = new SqlCommand(@"SELECT ItemType, ProductID, SizeId, Customer, BrandID, Color, Quantity, UnitWeight, GodownID, Manufacturer, UnitCost FROM [Crushing] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
    private void EditModeOut(string entryID)
    {
        SqlCommand cmd = new SqlCommand(@"SELECT ItemType, ProductID, SizeId, Customer, BrandID, Color, Quantity, UnitWeight, GodownID, Manufacturer, UnitCost FROM [Crushing] WHERE Id='" + entryID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            string productID = dr[1].ToString();
            ddGroupOut.SelectedValue = Stock.Inventory.GetItemGroup(productID);
            ddSubgroupOut.DataBind();
            ddSubgroupOut.SelectedValue = Stock.Inventory.GetItemSubGroup(productID);
            ddGradeOut.DataBind();
            ddGradeOut.SelectedValue = Stock.Inventory.GetItemGrade(productID);
            ddCategoryOut.DataBind();
            ddCategoryOut.SelectedValue = Stock.Inventory.GetItemCategory(productID);
            ddProductOut.DataBind();
            ddProductOut.SelectedValue = productID;

            LoadFormControlsOut();

            if (dr[0].ToString() == "Printed Sheet")
            {
                ddTypeOut.SelectedValue = "Printed Sheet";
                LoadFormControlsOut();
                ddSizeOut.SelectedValue = dr[2].ToString();
                ddCustomer.SelectedValue = dr[3].ToString();

                ddBrandOut.Items.Clear();
                ListItem lst = new ListItem("--- all ---", "");
                ddBrandOut.Items.Insert(ddBrand.Items.Count, lst);
                ddBrandOut.DataBind();

                ddBrandOut.SelectedValue = dr[4].ToString();
                ddColorOut.SelectedValue = dr[5].ToString();
            }
            else
            {
                ddTypeOut.SelectedValue = "Raw Sheet";
                LoadFormControlsOut();
            }

            txtPriceOut.Text = dr[10].ToString();
            txtQtyOut.Text = dr[6].ToString();
            txtWeightOut.Text = dr[7].ToString();
            txtDescOut.Text = dr[9].ToString();
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
    private void LoadFormControlsOut()
    {
        if (ddSubgroupOut.SelectedValue == "10")
        {
            pnlSpecOut.Visible = true;
        }
        else
        {
            pnlSpecOut.Visible = false;
        }

        if (ddSubgroupOut.SelectedValue == "9")
        {
            ddTypeAreaOut.Attributes.Remove("class");
            ddTypeAreaOut.Attributes.Add("class", "col-md-4");
        }
        else
        {
            ddTypeAreaOut.Attributes.Remove("class");
            ddTypeAreaOut.Attributes.Add("class", "col-md-4 hidden");
            ddTypeOut.SelectedValue = "Raw Sheet";
        }

        if (ddTypeOut.SelectedValue == "Printed Sheet")
        {
            pnlExtraOut.Visible = true;
        }
        else
        {
            pnlExtraOut.Visible = false;
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
    protected void ddCustomerOut_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrandOut.Items.Clear();
        ListItem lst = new ListItem("--- all ---", "");
        ddBrandOut.Items.Insert(ddBrand.Items.Count, lst);
        ddBrandOut.DataBind();

        QtyinStockOut();
    }

    protected void ddType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
    }
    protected void ddTypeOut_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStockOut();
    }

    protected void ddBrand_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
    }
    protected void ddBrandOut_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStockOut();
    }

    protected void ddColor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
    }
    protected void ddColorOut_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStockOut();
    }
    protected void ddSize_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
    }
    protected void ddSizeOut_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStockOut();
    }

    protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
    }
    protected void ddPurposeOut_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStockOut();
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
    protected void lbSpecOut_OnClick(object sender, EventArgs e)
    {
        if (lbSpecOut.Text == "New")
        {
            ddSpecOut.Visible = false;
            txtSpecOut.Visible = true;
            lbSpecOut.Text = "Cancel";
            txtSpecOut.Focus();
        }
        else
        {
            ddSpecOut.Visible = true;
            txtSpecOut.Visible = false;
            lbSpecOut.Text = "New";
            LoadSpecListOut("filter");
            ddSpecOut.Focus();
        }
        lbFilterOut.Text = "Show-all";
    }

    protected void ddSpec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
    }
    protected void ddSpecOut_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStockOut();
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
    protected void lbFilterOut_OnClick(object sender, EventArgs e)
    {
        if (lbFilterOut.Text == "Show-all")
        {
            LoadSpecListOut("");
            //lbFilter.Text = "Filter";
        }
        else
        {
            LoadSpecListOut("filter");
            //lbFilter.Text = "Show-all";
        }

        lbSpecOut.Text = "New";
        ddSpecOut.Visible = true;
        txtSpecOut.Visible = false;
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
    private void LoadSpecListOut(string filterDDOut)
    {
        string gQuery = "SELECT [id], [spec] FROM [Specifications] ORDER BY [spec]";
        lbFilterOut.Text = "Filter";
        if (filterDDOut != "")
        {
            lbFilterOut.Text = "Show-all";
            gQuery = "SELECT [id], [spec] FROM [Specifications] where  CAST(id AS nvarchar) in (Select distinct spec from stock where ProductID='" + ddProduct.SelectedValue + "') ORDER BY [spec]";
        }

        SQLQuery.PopulateDropDown(gQuery, ddSpecOut, "id", "spec");
        QtyinStockOut();
    }

    protected void ddSubgroupOut_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddGradeOut.DataBind();
        ddCategoryOut.DataBind();
        ddProductOut.DataBind();
        LoadFormControlsOut();
        LoadSpecListOut("filter");
        QtyinStock();
    }
}