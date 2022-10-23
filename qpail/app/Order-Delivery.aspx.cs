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
using RptRunQuery;
using RunQuery;

public partial class app_Order_Delivery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDate.Text = DateTime.Now.ToShortDateString();
            txtDeliveryDate.Text = DateTime.Now.ToShortDateString();

            ddSalesMode.DataBind();
            ddCustomer.DataBind();
            //lvOrders.DataBind();
            LoadOrders();

            GeneratePartyDetail();
            //BindItemGrid();
            //LoadSummesion("");


            //Auto or Manual Invoice ID generation
            string invId = SQLQuery.ReturnString("Select ShortDescription from Settings_Project where sid=5");
            if (invId == "1")
            {
                txtInvoiceNo.ReadOnly = true;
            }
            LoadInvoicingCompany();

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            txtInvoiceNo.Text = RunQuery.SQLQuery.GenerateInvoiceNo();

            if (lvOrders.Items.Count > 0)
            {
                lvOrders.SelectedIndex = 0;
                LoadOrderDetails();
            }
            else
            {
                ItemGrid.EmptyDataText = "No PO Selected!";
                ItemGrid.DataSource = null;
                ItemGrid.DataBind();
            }

            ltrLastInv.Text = RunQuery.SQLQuery.ReturnString("Select InvNo from Sales WHERE SaleID= (Select MAX(SaleID) from Sales) ");
        }
        //txtInv.Text = InvIDNo();
        //pnlPO.Visible = false;
        //if (ddSalesMode.SelectedValue == "PO")
        //{
        //    pnlPO.Visible = true;
        //}

    }


    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    private void LoadInvoicingCompany()
    {
        string query = "SELECT CompanyID, CompanyName FROM [Company]  ORDER BY [CompanyID]";
        SQLQuery.PopulateDropDown(query, ddLevelX, "CompanyID", "CompanyName");
        ddLevelX.SelectedValue = SQLQuery.ReturnString("Select ShortDescription from Settings_Project where sid=6");
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
        //if (txtTDS.Text == "")
        //{
        //    txtTDS.Text = "0";
        //}

        //txtPay.Text = Convert.ToString(Convert.ToDecimal(txtTotal.Text) - Convert.ToDecimal(txtTDS.Text) + (Convert.ToDecimal(txtTotal.Text) * Convert.ToDecimal(txtVat.Text) / 100M));
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

    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        GeneratePartyDetail();
        ItemGrid.DataSource = null;
        ItemGrid.DataBind();
        ddAddress.DataBind();
    }

    private void GeneratePartyDetail()
    {
        //lvOrders.DataBind();
        LoadOrders();
        if (ddSalesMode.SelectedValue == "LC")
        {
            txtMDays.Text = "0";
            txtOverDueDate.Text = txtDeliveryDate.Text;  //creditMatuirityDays;
        }
        else
        {
            string creditMatuirityDays = RunQuery.SQLQuery.ReturnString("Select MatuirityDays FROM Party where PartyID='" + ddCustomer.SelectedValue + "'");

            txtMDays.Text = creditMatuirityDays;
            txtOverDueDate.Text = Convert.ToString(Convert.ToDateTime(txtDeliveryDate.Text).AddDays(Convert.ToInt32(creditMatuirityDays)).ToShortDateString());  //creditMatuirityDays;
        }
        ddAddress.DataBind();
    }

    //private void BindItemGrid()
    //{
    //    string lName = Page.User.Identity.Name.ToString();
    //    String strConnString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
    //    SqlConnection con = new SqlConnection(strConnString);
    //    SqlCommand cmd = new SqlCommand();
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    cmd.CommandText = "GetSalesGridTempBySID";
    //    //cmd.CommandText = "SELECT Id, ProductName, UnitCost, Quantity, UnitType, ItemTotal FROM OrderDetails WHERE EntryBy=@EntryBy AND OrderID='' ORDER BY Id";

    //    cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
    //    cmd.Connection = con;

    //    con.Open();
    //    ItemGrid.EmptyDataText = "Select PO# for items list...";
    //    ItemGrid.DataSource = cmd.ExecuteReader();
    //    ItemGrid.DataBind();

    //    LoadSummesion("");
    //}


    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Quantity),0) from OrderDetails where OrderID=''");

        decimal totalQty = 0, totalWeight = 0, totalAmount = 0;
        foreach (GridViewRow row in ItemGrid.Rows)
        {
            Label lblQty = row.FindControl("lblQty") as Label;
            Label lblTotalWeight = row.FindControl("lblTotalWeight") as Label;
            Label lblSubTotal = row.FindControl("lblSubTotal") as Label;

            totalQty += Convert.ToDecimal(lblQty.Text);
            totalWeight += Convert.ToDecimal(lblTotalWeight.Text);
            totalAmount += Convert.ToDecimal(lblSubTotal.Text);
        }
        ltrQty.Text = totalQty.ToString();
        ltrWeight.Text =Convert.ToString(totalWeight/1000M);
        txtTotal.Text = totalAmount.ToString();

        txtPay.Text = Convert.ToString(totalAmount + (totalAmount / 100 * Convert.ToDecimal(txtVat.Text)));
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
                ScriptManager.RegisterStartupScript(page, typeof(Page), "Redirect", script, true);
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDecimal(txtTotal.Text) > 0)
            {
                string shortStock = VerifyStock();
                if (shortStock == "")
                {
                    string isExist = RunQuery.SQLQuery.ReturnString("Select InvNo from Sales where InvNo='" + txtInvoiceNo.Text + "'");
                    if (isExist == "")
                    {

                        ExecuteInsert();
                        LoadOrders();
                        //lvOrders.DataBind();

                        if (lvOrders.Items.Count>0)
                        {
                            lvOrders.SelectedIndex = 0;
                            LoadOrderDetails();
                        }
                        else
                        {
                            ItemGrid.EmptyDataText = "No PO Selected!";
                            ItemGrid.DataSource = null;
                            ItemGrid.DataBind();
                        }

                        Notify("Saved Successfully", "success", lblMsg);
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "New sales order saved successfully...";
                    }
                    else
                    {
                        txtInvoiceNo.Focus();
                        Notify("Invoice No. Already Exist!", "warn", lblMsg);
                    }
                }
                else
                {
                    LoadOrderDetails();
                    Notify("<b>Short Stock:</b> " + shortStock, "warn", lblMsg);
                }
            }
            else
            {
                LoadOrderDetails();

                Notify("Please select PO# first...", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
        finally
        {
            txtInvoiceNo.Text = RunQuery.SQLQuery.GenerateInvoiceNo();
            ltrLastInv.Text = RunQuery.SQLQuery.ReturnString("Select InvNo from Sales WHERE SaleID= (Select MAX(SaleID) from Sales) ");
            txtDeliveryDate.Focus();
        }
    }

    private void ExecuteInsert()
    {
        string orderNo = txtInvoiceNo.Text;
        string invId = SQLQuery.ReturnString("Select ShortDescription from Settings_Project where sid=5");
        if (invId == "1")// auto inv id
        {
            orderNo = SQLQuery.GenerateInvoiceNo();
        }

        string lName = Page.User.Identity.Name.ToString();
        decimal totalAmount = 0, vatAmount = 0, payable = 0;

        //if (chkPrint.Checked)
        //{
        //    RptSQLQuery.ExecNonQry("Delete Sales where InvNo='" + orderNo + "'");
        //    RptSQLQuery.ExecNonQry("Delete SaleDetails where InvNo='" + orderNo + "'");
        //}

        foreach (GridViewRow row in ItemGrid.Rows)
        {
            TextBox lblPName = row.FindControl("lblPName") as TextBox;
            TextBox txtpQty = row.FindControl("txtpQty") as TextBox;//deliver qty
            TextBox txtPrice = row.FindControl("txtPrice") as TextBox;
            TextBox txtItemWeight = row.FindControl("txtItemWeight") as TextBox;
            Label lblQty = row.FindControl("lblQty") as Label;
            TextBox lblpQty = row.FindControl("txtpQty") as TextBox;
            TextBox txtItemChallanNo = row.FindControl("txtItemChallanNo") as TextBox;

            TextBox txtVatPercent = row.FindControl("txtVatPercent") as TextBox;
            if (txtVatPercent.Text == "")
            {
                txtVatPercent.Text = "0";
            }
            TextBox txtQtyPack = row.FindControl("txtQtyPack") as TextBox;
            Label lblQtyPackTotal = row.FindControl("lblQtyPackTotal") as Label;

            string sizeId = RunQuery.SQLQuery.ReturnString("SELECT SizeID FROM OrderDetails where Id=(Select max(id) FROM OrderDetails where ProductName='" + lblPName.Text + "')");
            string brandID = RunQuery.SQLQuery.ReturnString("SELECT brandID FROM OrderDetails where Id=(Select max(id) FROM OrderDetails where ProductName='" + lblPName.Text + "')");
            string productID = RunQuery.SQLQuery.ReturnString("SELECT productID FROM OrderDetails where Id=(Select max(id) FROM OrderDetails where ProductName='" + lblPName.Text + "')");

            string stockQty = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where SizeID='" + sizeId + "' AND BrandID='" + brandID + "' AND ProductID='" + productID + "'");
            decimal orderQty = Convert.ToDecimal(txtpQty.Text);
            decimal availableQty = Convert.ToDecimal(stockQty);

            SqlCommand cmd = new SqlCommand("INSERT INTO SaleDetails (InvNo, SizeId, ProductID, BrandID, ProductName, UnitCost, UnitWeight, Quantity, ItemTotal, TotalWeight, UnitType, VatPercent, VAT, NetAmount, QtyPerCarton, TotalCarton, PerviousDeliveredQty, QtyBalance, ItemChallanNo, EntryBy)" +
                                                        " VALUES (@InvNo, @SizeId, @ProductID, @BrandID, @ProductName, @UnitCost, @UnitWeight, @Quantity, @ItemTotal, @TotalWeight, @UnitType, @VatPercent, @VAT, @NetAmount, @QtyPerCarton, @TotalCarton, @PerviousDeliveredQty, @QtyBalance, '"+txtItemChallanNo.Text+"', @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd.Parameters.AddWithValue("@InvNo", orderNo);
            cmd.Parameters.AddWithValue("@SizeId", sizeId);
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            cmd.Parameters.AddWithValue("@ProductName", lblPName.Text);

            cmd.Parameters.AddWithValue("@UnitCost", Convert.ToDecimal(txtPrice.Text));
            cmd.Parameters.AddWithValue("@UnitWeight", Convert.ToDecimal(txtItemWeight.Text));
            cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtpQty.Text));

            decimal iTotal = Convert.ToDecimal(txtpQty.Text) * Convert.ToDecimal(txtPrice.Text);
            cmd.Parameters.AddWithValue("@ItemTotal", iTotal);

            decimal ttlWeight = Convert.ToDecimal(txtpQty.Text)*Convert.ToDecimal(txtItemWeight.Text)/1000M;
            cmd.Parameters.AddWithValue("@TotalWeight", ttlWeight);
            cmd.Parameters.AddWithValue("@VatPercent", txtVatPercent.Text); //iTotal + (iTotal * Convert.ToDecimal(txtVat.Text) / 100M));
            cmd.Parameters.AddWithValue("@VAT", (iTotal * Convert.ToDecimal(txtVatPercent.Text) / 100M));

            cmd.Parameters.AddWithValue("@NetAmount", iTotal + (iTotal * Convert.ToDecimal(txtVatPercent.Text) / 100M));
            cmd.Parameters.AddWithValue("@QtyPerCarton", Convert.ToInt32(txtQtyPack.Text));
            cmd.Parameters.AddWithValue("@TotalCarton", Convert.ToInt32(lblQtyPackTotal.Text));

            cmd.Parameters.AddWithValue("@UnitType", "PCS");
            cmd.Parameters.AddWithValue("@PerviousDeliveredQty", Convert.ToInt32(lblpQty.Text));
            cmd.Parameters.AddWithValue("@QtyBalance", Convert.ToInt32(lblQty.Text));
            cmd.Parameters.AddWithValue("@EntryBy", lName);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            //if (chkPrint.Checked)
            //{
            //    RptSQLQuery.ExecNonQry(
            //        "INSERT INTO SaleDetails (InvNo, SizeId, ProductID, BrandID, ProductName, UnitCost, UnitWeight, Quantity, ItemTotal, TotalWeight, UnitType, VatPercent, VAT, NetAmount, QtyPerCarton, TotalCarton, PerviousDeliveredQty, QtyBalance, EntryBy)" +
            //        " VALUES ('" + orderNo + "', '" + sizeId + "', '" + productID + "', '" + brandID + "', '" + lblPName.Text + "', '" + txtPrice.Text + "', '" + txtItemWeight.Text + "', '" + txtpQty.Text + "', '" + iTotal + "', '" + ttlWeight + "', 'PCS', '" + txtVatPercent.Text + "', '" + (iTotal * Convert.ToDecimal(txtVatPercent.Text) / 100M) + "', '" + (iTotal + (iTotal * Convert.ToDecimal(txtVatPercent.Text) / 100M)) + "', '" + Convert.ToInt32(txtQtyPack.Text) + "', '" + Convert.ToInt32(lblQtyPackTotal.Text) + "', '" + Convert.ToInt32(lblpQty.Text) + "', '" + Convert.ToInt32(lblQty.Text) + "', '" + lName + "')");
            //}
            //Item entry to stock
            //Accounting.VoucherEntry.StockEntry(orderNo, "Sale Invoice", orderNo, sizeId, brandID, productID, lblPName.Text, ddWarehouse.SelectedValue, "", "2", "0", txtpQty.Text, "0", "0", "", "Stock-out", "Finished", "", lName);
            Stock.Inventory.SaveToStock("", orderNo, ddSalesMode.SelectedValue + " Item Delivery", orderNo, sizeId,
                ddCustomer.SelectedValue, brandID, "", "", productID, lblPName.Text, "", ddWarehouse.SelectedValue, "0",
                "2", 0, Convert.ToInt32(txtpQty.Text), Convert.ToDecimal(txtPrice.Text), 0, ttlWeight, "", "Stock-out",
                "Sales", "Finished Stock", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

            totalAmount += Convert.ToDecimal(txtpQty.Text) * Convert.ToDecimal(txtPrice.Text);

            //Update order details table
            int qtyBalance = 0, deliveryBalance = Convert.ToInt32(orderQty);//
            int[] indexes = this.lvOrders.GetSelectedIndices();
            for (int index = 0; index < indexes.Length; index++)
            {
                string poNo = this.lvOrders.Items[indexes[index]].Value;
                //PO Order Remaining Qty
                string eid = SQLQuery.ReturnString("SELECT Id FROM OrderDetails where OrderID='" + poNo + "' AND SizeId='" + sizeId + "' AND  ProductID='" + productID + "' AND BrandID='" + brandID + "'");
                int EntryID = 0;
                if (eid != "")
                {
                    EntryID = Convert.ToInt32(eid);

                int oQty = Convert.ToInt32(SQLQuery.ReturnString("SELECT ISNULL(SUM(Quantity),0) FROM OrderDetails where id=" + EntryID));
                int delivered = Convert.ToInt32(SQLQuery.ReturnString("SELECT ISNULL(DeliveredQty,0) FROM OrderDetails where id=" + EntryID));
                qtyBalance = qtyBalance + oQty - delivered;

                if (oQty > 0 && deliveryBalance > 0)
                {
                    if (oQty == deliveryBalance) //Total delivered Qty: orderQty
                    {
                        UpdateDeliveredQty(orderNo, EntryID, Convert.ToInt32(oQty), 0, 0);
                        deliveryBalance = 0;
                    }
                    else if (oQty > deliveryBalance)
                    {
                        UpdateDeliveredQty(orderNo, EntryID, Convert.ToInt32(deliveryBalance), oQty - deliveryBalance, 1);
                        deliveryBalance = oQty - deliveryBalance;
                    }
                    else
                    {
                        UpdateDeliveredQty(orderNo, EntryID, Convert.ToInt32(oQty), deliveryBalance - oQty, 0);
                        deliveryBalance -= oQty;
                    }
                }

                }
                //SQLQuery.ExecNonQry("UPDATE OrderDetails SET DeliveryInvoice='" + orderNo + "', DeliveredQty=(DeliveredQty+" + delivered + ") WHERE Id=" + EntryID);
                //SQLQuery.ExecNonQry("UPDATE OrderDetails SET QtyBalance=(Quantity-DeliveredQty) WHERE Id=" + EntryID);
            }
            if (deliveryBalance > 0)
            {
                SQLQuery.ExecNonQry("INSERT INTO SalesExcessItems (ProductName, InvoiceID, ExcessQty, EntryBy) VALUES ('" + lblPName.Text + "', '" + orderNo + "', '" + deliveryBalance + "', '" + lName + "')");
            }

        }

        txtTotal.Text = totalAmount.ToString();
        RunQuery.SQLQuery.Empty2Zero(txtVat);
        vatAmount = Convert.ToDecimal(txtVat.Text);//totalAmount * Convert.ToDecimal(txtVat.Text) / 100;
        txtPay.Text = Convert.ToString(totalAmount + vatAmount);
        string inWords = SQLQuery.DecimalToWords(Convert.ToDecimal(txtPay.Text).ToString("#.00"));
        //Save Transaction
        SqlCommand cmd2 = new SqlCommand("INSERT INTO Sales (InvNo, InvDate, SalesMode, CustomerID, CustomerName, PONo, Period, DeliveryDate, DeliveryTime, DeliveryLocation, TransportDetail, MaturityDays, OverDueDate, " +
                         " Remarks, InvoiceTotal, VATPercent, VATAmount, PayableAmount, CartonQty, ChallanNo, CollectedAmount, DueAmount, ProjectID, EntryBy, IsActive, InWords, Warehouse, VatChalNo, VatChalDate)" +
                                                    " VALUES (@InvNo, @InvDate, @SalesMode, @CustomerID, @CustomerName, @PONo, @Period, @DeliveryDate, @DeliveryTime, @DeliveryLocation, @TransportDetail, @MaturityDays, @OverDueDate, " +
                         " @Remarks, @InvoiceTotal, @VATPercent, @VATAmount, @PayableAmount, @CartonQty, @ChallanNo, @CollectedAmount, @DueAmount, @ProjectID, @EntryBy, @IsActive, '"+inWords+"', '" + ddWarehouse.SelectedValue + "', '" + txtVatNo.Text + "', '" + txtChallanDate .Text+ "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@InvNo", orderNo);
        cmd2.Parameters.AddWithValue("@InvDate", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        cmd2.Parameters.AddWithValue("@SalesMode", ddSalesMode.SelectedValue);
        cmd2.Parameters.AddWithValue("@CustomerID", ddCustomer.SelectedValue);
        cmd2.Parameters.AddWithValue("@CustomerName", ddCustomer.SelectedItem.Text.ToUpper());
        cmd2.Parameters.AddWithValue("@PONo", txtInv.Text);
        cmd2.Parameters.AddWithValue("@Period", txtPeriod.Text);

        cmd2.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(txtDeliveryDate.Text));
        cmd2.Parameters.AddWithValue("@DeliveryTime", txtTime.Text);
        cmd2.Parameters.AddWithValue("@DeliveryLocation", ddAddress.SelectedValue);
        cmd2.Parameters.AddWithValue("@TransportDetail", txtTransport.Text);
        cmd2.Parameters.AddWithValue("@MaturityDays", Convert.ToInt32(txtMDays.Text));
        cmd2.Parameters.AddWithValue("@OverDueDate", Convert.ToDateTime(txtOverDueDate.Text).ToString("yyyy-MM-dd"));

        cmd2.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        cmd2.Parameters.AddWithValue("@InvoiceTotal", Convert.ToDecimal(txtTotal.Text));
        cmd2.Parameters.AddWithValue("@VATPercent", TextBox1.Text); //Convert.ToDecimal(txtVat.Text));
        cmd2.Parameters.AddWithValue("@VATAmount", vatAmount);
        cmd2.Parameters.AddWithValue("@PayableAmount", Convert.ToDecimal(txtPay.Text));
        cmd2.Parameters.AddWithValue("@CartonQty", txtCarton.Text);

        cmd2.Parameters.AddWithValue("@ChallanNo", txtChallanNo.Text);
        cmd2.Parameters.AddWithValue("@CollectedAmount", 0);
        cmd2.Parameters.AddWithValue("@DueAmount", Convert.ToDecimal(txtPay.Text));
        cmd2.Parameters.AddWithValue("@ProjectID", lblProject.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        if (ddSalesMode.SelectedValue=="PO")
        {
            cmd2.Parameters.AddWithValue("@IsActive", 1);
        }
        else //LC Devery Item Marking is 2
        {
            cmd2.Parameters.AddWithValue("@IsActive", 2);
        }


        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        SqlCommand cmd4 = new SqlCommand("INSERT INTO Transactions (InvNo, TrDate, HeadID, HeadName, Description, Dr, Cr, Balance, TrGroup, TrType, AccHeadID, EntryBy, ProjectID)" +
                                                    " VALUES (@InvNo, @TrDate, @HeadID, @HeadName, @Description, @Dr, @Cr, @Balance, @TrGroup, @TrType, @AccHeadID, @EntryBy, @ProjectID)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd4.Parameters.AddWithValue("@InvNo", orderNo);
        cmd4.Parameters.AddWithValue("@TrDate", Convert.ToDateTime(txtDeliveryDate.Text));
        cmd4.Parameters.AddWithValue("@HeadID", ddCustomer.SelectedValue);
        cmd4.Parameters.AddWithValue("@HeadName", ddCustomer.SelectedItem.Text);
        cmd4.Parameters.AddWithValue("@Description", "Invoice No. : " + orderNo);

        cmd4.Parameters.AddWithValue("@Dr", Convert.ToDecimal(txtPay.Text));
        cmd4.Parameters.AddWithValue("@Cr", 0);
        cmd4.Parameters.AddWithValue("@Balance", 0);
        cmd4.Parameters.AddWithValue("@TrGroup", "Sales");
        cmd4.Parameters.AddWithValue("@TrType", "Customer");

        cmd4.Parameters.AddWithValue("@AccHeadID", "030101014");
        cmd4.Parameters.AddWithValue("@EntryBy", lName);
        cmd4.Parameters.AddWithValue("@ProjectID", "1");

        cmd4.Connection.Open();
        int succ = cmd4.ExecuteNonQuery();
        cmd4.Connection.Close();

        //InActivae PO
        if (succ > 0)
        {
            int[] indexes = this.lvOrders.GetSelectedIndices();
            for (int index = 0; index < indexes.Length; index++)
            {
                string poNo = this.lvOrders.Items[indexes[index]].Value;
                SQLQuery.ExecNonQry("Update Orders set DeliveryStatus='A' where OrderID='" + poNo + "'");

                int qtyBalance = Convert.ToInt32(SQLQuery.ReturnString("SELECT SUM(Quantity)-SUM(DeliveredQty) FROM OrderDetails where OrderID='" + poNo + "'"));

                if (qtyBalance <= 0)
                {
                    SQLQuery.ExecNonQry("Update Orders set DeliveryStatus='D' where OrderID='" + poNo + "'");
                }
            }
        }


        if (chkPrint.Checked)
        {
            /*
            string address = SQLQuery.ReturnString("Select Address from Party where PartyID='" + ddCustomer.SelectedValue + "'");

            RptSQLQuery.ExecNonQry(@"INSERT INTO Sales (InvNo, InvDate, SalesMode, CustomerID, CustomerName, PONo, Period, DeliveryDate, DeliveryTime, DeliveryLocation, TransportDetail, MaturityDays, OverDueDate, " +
                         " Remarks, InvoiceTotal, VATPercent, VATAmount, PayableAmount, CartonQty, ChallanNo, CollectedAmount, DueAmount, ProjectID, EntryBy, IsActive, InWords)" +
                                                    " VALUES ('" + orderNo + "', '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + address + "', '" + ddCustomer.SelectedValue + "', '" + ddCustomer.SelectedItem.Text + "', '" + txtInv.Text + "', '" + txtPeriod.Text + "', '" + Convert.ToDateTime(txtDeliveryDate.Text) + "', '" + txtTime.Text + "', '" + txtAddress.Text + "', '" + txtTransport.Text + "', '" + Convert.ToInt32(txtMDays.Text) + "', '" + Convert.ToDateTime(txtOverDueDate.Text).ToString("yyyy-MM-dd") + "', " +
                         " '" + txtRemarks.Text + "',  '" + Convert.ToDecimal(txtTotal.Text) + "',  '" + TextBox1.Text + "',  '" + vatAmount + "',  '" + Convert.ToDecimal(txtPay.Text) + "',  '" + txtCarton.Text + "',  '" + txtChallanNo.Text + "',  '" + Convert.ToDecimal(txtPay.Text) + "', '0', '" + lblProject.Text + "',  '" + lName + "', '1', '" + inWords + "')");
            */
            string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=" + orderNo;
            ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");

        }


        if (chkDO.Checked)
        {
            string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmChallan.aspx?inv=" + orderNo;
            ResponseHelper.Redirect(url, "_blank", "menubar=1,width=800,height=600");
            //Response.Write(String.Format("window.open('{0}','_blank')", ResolveUrl(url)));

            //string script = string.Format("window.open('{0}');", url);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "newPage" + UniqueID, script, true);
        }

    }

    private void UpdateDeliveredQty(string orderNo, int EntryID, int delivered, int remain, int isActive)
    {
        //int preDelivered = Convert.ToInt32(SQLQuery.ReturnString("SELECT ISNULL(DeliveredQty,0) FROM OrderDetails where id=" + EntryID));
        //delivered = delivered + preDelivered;
        //SQLQuery.ExecNonQry("UPDATE OrderDetails SET DeliveryInvoice='" + orderNo + "', DeliveredQty=" + delivered + ", QtyBalance=" + remain + " WHERE Id=" + EntryID);
        SQLQuery.ExecNonQry("UPDATE OrderDetails SET DeliveryInvoice='" + orderNo + "', DeliveredQty=(DeliveredQty+" + delivered + ") WHERE Id=" + EntryID);
        SQLQuery.ExecNonQry("UPDATE OrderDetails SET QtyBalance=(Quantity-DeliveredQty) WHERE Id=" + EntryID);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //string lName = Page.User.Identity.Name.ToString();
        //SqlCommand cmd = new SqlCommand("Delete OrderDetails WHERE EntryBy=@EntryBy AND OrderID=''", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmd.Parameters.AddWithValue("@EntryBy", lName);
        //cmd.Connection.Open();
        //cmd.ExecuteNonQuery();
        //cmd.Connection.Close();
        //cmd.Connection.Dispose();
        //Response.Redirect("./Sales-Xclusive.aspx?type=order");
    }

    protected void lbOrders_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadOrderDetails();

        string shortStock = VerifyStock();
        if (shortStock != "")
        {
            LoadOrderDetails();
            lblMsg2.Attributes.Add("class", "xerp_stop");
            lblMsg2.Text = "<b>Short Stock:</b> " + shortStock;
        }
    }

    private void LoadOrderDetails()
    {
        try
        {
            string orders = "";
            int[] indexes = this.lvOrders.GetSelectedIndices();

            for (int index = 0; index < indexes.Length; index++)
            {
                orders += this.lvOrders.Items[indexes[index]].Value + ",";
            }
            orders = orders.TrimEnd(',');
            txtInv.Text = orders;
            ltrInv.Text = orders;

            //if (ddSalesMode.SelectedValue == "LC")
            //{
            //    ltrInv.Text = "Selected PI# " + orders;
            //}
            //else
            //{
            //    ltrInv.Text = "Selected PO# " + orders;
            //}


            string sqlStatement = "";
            int i = 0;

            string[] allOrders = orders.Split(',');
            string orderLast = "";
            foreach (string orderId in allOrders)
            {
                if (i == 0)
                {
                    sqlStatement = " (OrderID ='" + orderId + "') ";
                }
                else
                {
                    sqlStatement = sqlStatement + "OR (OrderID ='" + orderId + "') ";
                }
                orderLast = orderId;
                i++;
            }

            sqlStatement = "SELECT DISTINCT ProductName, (Select UnitCost from SaleDetails  where Id=(Select ISNULL(MAX(Id),0) from SaleDetails WHERE ProductName=OrderDetails.ProductName)) AS rate, SUM(Quantity) AS qty, SUM(ItemTotal) AS amt, AVG(VATPercent) AS VATPercent, SUM(VATAmount) AS VATAmount, SUM(DeliveredQty) AS dQty, (SUM(Quantity)-SUM(DeliveredQty)) AS pQty, " +
                " AVG(UnitWeight) AS UnitWeight, SUM(TotalWeight) AS TotalWeight,  SizeId, ProductID, BrandID FROM OrderDetails" +
                " WHERE " + sqlStatement +
                " GROUP BY ProductName, SizeId,ProductID,BrandID";

            DataTable dt = RunQuery.SQLQuery.ReturnDataTable(sqlStatement);
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("ProductName", typeof (string)));
            dt1.Columns.Add(new DataColumn("rate", typeof (string)));
            dt1.Columns.Add(new DataColumn("qty", typeof (string)));
            dt1.Columns.Add(new DataColumn("amt", typeof (string)));
            dt1.Columns.Add(new DataColumn("VATPercent", typeof (string)));
            dt1.Columns.Add(new DataColumn("VATAmount", typeof (string)));
            dt1.Columns.Add(new DataColumn("dQty", typeof (string)));

            dt1.Columns.Add(new DataColumn("pQty", typeof (string)));
            dt1.Columns.Add(new DataColumn("UnitWeight", typeof (string)));
            dt1.Columns.Add(new DataColumn("TotalWeight", typeof (string)));
            dt1.Columns.Add(new DataColumn("iCode", typeof (string)));
            dt1.Columns.Add(new DataColumn("QtyPack", typeof (string)));


            foreach (DataRow citydr in dt.Rows)
            {
                string ProductName = citydr["ProductName"].ToString();
                string rate = citydr["rate"].ToString();
                string qty = citydr["qty"].ToString();

                string dQty = citydr["dQty"].ToString();
                string pQty = "0";// citydr["pQty"].ToString();
                string UnitWeight = citydr["UnitWeight"].ToString();
                UnitWeight = Convert.ToDecimal(UnitWeight).ToString("0.00#");

                string SizeId = citydr["SizeId"].ToString();
                string ProductID = citydr["ProductID"].ToString();
                string BrandID = citydr["BrandID"].ToString();
                string iCode = SizeId + "-" + ProductID + "-" + BrandID;

                if (rate == "")
                {
                    rate =RunQuery.SQLQuery.ReturnString(
                            "Select UnitCost from OrderDetails where Id=(Select ISNULL(MAX(Id),0) from OrderDetails WHERE SizeId='" +
                            SizeId + "' AND BrandID='" + BrandID + "' AND ProductID='" + ProductID + "')");
                    if (rate == "")
                    {
                        rate = "0";
                    }
                }
                string amt = Convert.ToString(Convert.ToDecimal(qty)*Convert.ToDecimal(rate));
                //citydr["amt"].ToString();
                string vatPercent = "15"; //citydr["VATPercent"].ToString();
                string vatAmt = Convert.ToString(Convert.ToDecimal(amt)*Convert.ToDecimal(vatPercent)/100M);
                string TotalWeight = Convert.ToString(Convert.ToDecimal(qty)*Convert.ToDecimal(UnitWeight));
                //citydr["TotalWeight"].ToString();

                dr1 = dt1.NewRow();
                dr1["ProductName"] = ProductName;
                dr1["rate"] = rate;
                dr1["qty"] = qty;
                dr1["amt"] = amt;
                dr1["VATPercent"] = vatPercent;
                dr1["VATAmount"] = vatAmt;

                dr1["dQty"] = dQty;
                dr1["pQty"] = pQty;
                dr1["UnitWeight"] = UnitWeight;
                dr1["TotalWeight"] = TotalWeight;
                dr1["iCode"] = iCode;
                dr1["QtyPack"] = "0";
                dt1.Rows.Add(dr1);

            }

            ItemGrid.EmptyDataText = "Select PO No. For Items to view...";
            ItemGrid.DataSource = dt1;

            //SqlCommand cmd = new SqlCommand(sqlStatement, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            //cmd.Connection.Open();
            //ItemGrid.DataSource = cmd.ExecuteReader();
            //cmd.Connection.Close();

            //txtAddress.Text = RunQuery.SQLQuery.ReturnString("Select DeliveryAddress from Orders where OrderID ='" + orderLast + "'");
            ItemGrid.DataBind();
            //btnEdit.Visible = true;
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    private string VerifyStock()
    {
        string shortItem = "";// "<span style='line-height:14px;'>";
        int shortQty = 0;

        foreach (GridViewRow row in ItemGrid.Rows)
        {
            TextBox lblPName = row.FindControl("lblPName") as TextBox;
            TextBox txtpQty = row.FindControl("txtpQty") as TextBox;
            Label lblEntryId = row.FindControl("lblEntryId") as Label;

            string sizeId = RunQuery.SQLQuery.ReturnString("SELECT SizeID FROM OrderDetails where Id=(Select max(id) FROM OrderDetails where ProductName='" + lblPName.Text + "')");
            string brandID = RunQuery.SQLQuery.ReturnString("SELECT brandID FROM OrderDetails where Id=(Select max(id) FROM OrderDetails where ProductName='" + lblPName.Text + "')");
            string productID = RunQuery.SQLQuery.ReturnString("SELECT productID FROM OrderDetails where Id=(Select max(id) FROM OrderDetails where ProductName='" + lblPName.Text + "')");

            string stockQty = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where SizeID='" + sizeId + "' AND BrandID='" + brandID + "' AND ProductID='" + productID + "'");
            decimal orderQty = Convert.ToDecimal(txtpQty.Text);
            decimal availableQty = Convert.ToDecimal(stockQty);
            if (orderQty > availableQty)
            {
                shortQty++;
                shortItem = shortItem + "<br>" + lblPName.Text + ": Available Qty. " + stockQty + " PCS.";
            }
        }

        if (Stock.Inventory.StockEnabled()=="0")
        {
            shortItem = "";
        }

        return shortItem;
    }
    protected void ddSalesMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadOrders();
    }

    private void LoadOrders()
    {
        //lvOrders.DataBind();
        if (ddSalesMode.SelectedValue == "LC")
        {
            SQLQuery.PopulateListView("SELECT [OrderID], (OrderID+'  Dated: '+ (CONVERT(varchar,(Select LcDate from PI where LcNo=Orders.OrderID),103))) AS InvDetail  FROM [Orders] WHERE CustomerName='" + ddCustomer.SelectedValue + "' AND ([DeliveryStatus] = 'A' OR [DeliveryStatus] = 'P') AND OrderType='" + ddSalesMode.SelectedValue + "'", lvOrders, "OrderID", "InvDetail");
            lblPONo.Text = "LC No.";
        }
        else
        {
            SQLQuery.PopulateListView("SELECT [OrderID], (OrderID+'  Dated: '+ (CONVERT(varchar, OrderDate,103))) AS InvDetail   FROM [Orders] WHERE CustomerName='" + ddCustomer.SelectedValue + "' AND ([DeliveryStatus] = 'A' OR [DeliveryStatus] = 'P') AND OrderType='" + ddSalesMode.SelectedValue + "'", lvOrders, "OrderID", "InvDetail");
            lblPONo.Text = "PO No.";
        }
    }

}