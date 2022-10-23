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
using Accounting;
using RunQuery;

public partial class app_Sales_Direct : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDate.Text = DateTime.Now.ToShortDateString();
            txtDeliveryDate.Text = DateTime.Now.ToShortDateString();
            txtChallanDate.Text = DateTime.Now.ToShortDateString();

            if (DateTime.Now>Convert.ToDateTime("2017-09-01"))
            {
                Response.Redirect("Sales-Fixed-Items.aspx");
            }

            ddCustomer.DataBind();
            ddAddress.DataBind();

            GeneratePartyDetail();
            BindItemGrid();

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
            ltrLastInv.Text = RunQuery.SQLQuery.ReturnString("Select InvNo from Sales WHERE SaleID= (Select MAX(SaleID) from Sales) ");

            txtTime.Text = DateTime.Now.AddHours(1).ToString("hh") + ":00 " + DateTime.Now.AddHours(1).ToString("tt");
        }
        //txtInv.Text = InvIDNo();

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
        string lName = Page.User.Identity.Name.ToString();
        ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Quantity),0) from OrderDetails where EntryBy='" + lName + "' AND  OrderID='" + OrderId + "' AND CustomerID='" + ddCustomer.SelectedValue + "' ");
        txtTotal.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(ItemTotal),0) from OrderDetails where EntryBy='" + lName + "' AND  OrderID='" + OrderId + "' AND CustomerID='" + ddCustomer.SelectedValue + "' ");

        if (txtVat.Text == "")
        {
            txtVat.Text = "0";
        }
        if (txtDiscount.Text == "")
        {
            txtDiscount.Text = "0";
        }

        txtPay.Text = Convert.ToString(Convert.ToDecimal(txtTotal.Text) - Convert.ToDecimal(txtDiscount.Text) + (Convert.ToDecimal(txtTotal.Text) * Convert.ToDecimal(txtVat.Text) / 100M));
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

        SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (max(SaleID),0)+ 1 )) from Sales where EntryDate>=@EntryDate", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
        GeneratePartyDetail();
        ItemGrid.DataSource = null;
        BindItemGrid();
        ddAddress.DataBind();
    }

    private void GeneratePartyDetail()
    {
        //ddAddress.DataBind();
        string creditMatuirityDays = RunQuery.SQLQuery.ReturnString("Select MatuirityDays FROM Party where PartyID='" + ddCustomer.SelectedValue + "'");

        txtMDays.Text = creditMatuirityDays;
        txtOverDueDate.Text = Convert.ToString(Convert.ToDateTime(txtDeliveryDate.Text).AddDays(Convert.ToInt32(creditMatuirityDays)).ToShortDateString());  //creditMatuirityDays;
    }

    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Quantity),0) from OrderDetails where OrderID=''");

        decimal totalQty = 0, totalWeight = 0, totalAmount = 0;
        foreach (GridViewRow row in ItemGrid.Rows)
        {
            TextBox txtpQty = row.FindControl("txtpQty") as TextBox;
            Label lblTotalWeight = row.FindControl("lblTotalWeight") as Label;
            Label lblSubTotal = row.FindControl("lblSubTotal") as Label;

            totalQty += Convert.ToDecimal(txtpQty.Text);
            totalWeight += Convert.ToDecimal(lblTotalWeight.Text);
            totalAmount += Convert.ToDecimal(lblSubTotal.Text);
        }
        ltrQty.Text = totalQty.ToString();
        ltrWeight.Text = Convert.ToString(totalWeight/1000M);
        txtTotal.Text = totalAmount.ToString();

        decimal vat = 0;
        if (txtVat.Text != "")
        {
            vat = Convert.ToDecimal(txtVat.Text);
        }
        lblVatAmt.Text = "Total VAT Amount: " + (totalAmount/100*vat);

        txtPay.Text = Convert.ToString(totalAmount + (totalAmount / 100 * vat));
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

                        txtInvoiceNo.Text = RunQuery.SQLQuery.GenerateInvoiceNo();
                        ltrLastInv.Text = RunQuery.SQLQuery.ReturnString("Select InvNo from Sales WHERE SaleID= (Select MAX(SaleID) from Sales) ");
                        //lblMsg.Attributes.Add("class", "xerp_success");
                        //lblMsg.Text = "New sales order saved successfully...";
                        Notify("New sales order saved successfully...", "success", lblMsg);
                    }
                    else
                    {
                        txtInvoiceNo.Focus();
                        Notify("Invoice No. Already Exist!" + shortStock, "warn", lblMsg2);
                        //lblMsg2.Attributes.Add("class", "xerp_error");
                        //lblMsg2.Text = "Invoice No. Already Exist!" + shortStock;
                    }
                }
                else
                {
                    BindItemGrid();
                    //lblMsg2.Attributes.Add("class", "xerp_error");
                    //lblMsg2.Text = "<b>Stort Stock:</b> " + shortStock;
                    Notify("<b>Stort Stock:</b> " + shortStock, "warn", lblMsg2);
                }
            }
            else
            {
                BindItemGrid();
                //lblMsg.Attributes.Add("class", "xerp_error");
                //lblMsg.Text = "Please select PO# first...";
                Notify("Please select PO# first...", "warn", lblMsg2);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
        finally
        {
            txtDeliveryDate.Focus();
            BindItemGrid();
        }
    }

    private void ExecuteInsert()
    {
        string orderNo = txtInvoiceNo.Text;

        string invId = SQLQuery.ReturnString("Select ShortDescription from Settings_Project where sid=5");
        if (invId == "1")
        {
            orderNo = SQLQuery.GenerateInvoiceNo();
        }

        string lName = Page.User.Identity.Name.ToString();
        decimal totalAmount = 0, vatAmount = 0, payable = 0;
        string podate = txtPODate.Text;

        if (txtPODate.Text!="")
        {
            //podate = Convert.ToDateTime(txtPODate.Text).ToString("yyyy-MM-dd");
        }

        foreach (GridViewRow row in ItemGrid.Rows)
        {
            TextBox lblPName = row.FindControl("lblPName") as TextBox;
            TextBox txtpQty = row.FindControl("txtpQty") as TextBox;
            TextBox txtPrice = row.FindControl("txtPrice") as TextBox;
            TextBox txtItemWeight = row.FindControl("txtItemWeight") as TextBox;
            //Label lblQty = row.FindControl("lblQty") as Label;
            TextBox lblpQty = row.FindControl("txtpQty") as TextBox;
            TextBox txtItemChallanNo = row.FindControl("txtItemChallanNo") as TextBox;

            Label lblEntryId = row.FindControl("lblEntryId") as Label;
            string sizeId = RunQuery.SQLQuery.ReturnString("SELECT SizeID FROM OrderDetails where Id='" + lblEntryId.Text + "'");
            string brandID = RunQuery.SQLQuery.ReturnString("SELECT brandID FROM OrderDetails where Id='" + lblEntryId.Text + "'");
            string productID = RunQuery.SQLQuery.ReturnString("SELECT productID FROM OrderDetails where Id='" + lblEntryId.Text + "'");

            string stockQty = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where SizeID='" + sizeId + "' AND BrandID='" + brandID + "' AND ProductID='" + productID + "'");
            decimal orderQty = Convert.ToDecimal(txtpQty.Text);
            decimal availableQty = Convert.ToDecimal(stockQty);

            SQLQuery.ExecNonQry("Update OrderDetails set ProductName='"+ lblPName.Text + "' where Id='" + lblEntryId.Text + "'");
            SqlCommand cmd = new SqlCommand("INSERT INTO SaleDetails (InvNo, SizeId, ProductID, BrandID, ProductName, UnitCost, UnitWeight, Quantity, ItemTotal, TotalWeight, UnitType, VAT, PerviousDeliveredQty, QtyBalance, ItemChallanNo, EntryBy)" +
                                                        " VALUES (@InvNo, @SizeId, @ProductID, @BrandID, @ProductName, @UnitCost, @UnitWeight, @Quantity, @ItemTotal, @TotalWeight, @UnitType, @VAT, @PerviousDeliveredQty, @QtyBalance, '" + txtItemChallanNo.Text + "', @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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

            decimal ttlWeight = Convert.ToDecimal(txtpQty.Text) * Convert.ToDecimal(txtItemWeight.Text) / 1000M;
            cmd.Parameters.AddWithValue("@TotalWeight", ttlWeight);
            cmd.Parameters.AddWithValue("@VAT", iTotal + (iTotal * Convert.ToDecimal(txtVat.Text) / 100M));

            cmd.Parameters.AddWithValue("@UnitType", "PCS");
            cmd.Parameters.AddWithValue("@PerviousDeliveredQty", Convert.ToInt32(lblpQty.Text));
            cmd.Parameters.AddWithValue("@QtyBalance", 0);
            cmd.Parameters.AddWithValue("@EntryBy", lName);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();


            //Item entry to stock
            Stock.Inventory.SaveToStock("", orderNo, "Direct Invoice", orderNo, sizeId,
            ddCustomer.SelectedValue, brandID, "", "", productID, lblPName.Text, "", ddWarehouse.SelectedValue, "0",
                "2", 0, Convert.ToInt32(txtpQty.Text), Convert.ToDecimal(txtPrice.Text), 0, ttlWeight, "", "Stock-out",
                "Sales", "Finished Stock", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));


            totalAmount += Convert.ToDecimal(txtpQty.Text) * Convert.ToDecimal(txtPrice.Text);

            /*
                        //Update order details table
                        int qtyBalance = 0, deliveryBalance = Convert.ToInt32(orderQty);//
                        int[] indexes = this.lvOrders.GetSelectedIndices();
                        for (int index = 0; index < indexes.Length; index++)
                        {
                            string poNo = this.lvOrders.Items[indexes[index]].Text;
                            //PO Order Remaining Qty
                            string eid = SQLQuery.ReturnString("SELECT Id FROM OrderDetails where OrderID='" + poNo + "' AND SizeId='" + sizeId + "' AND  ProductID='" + productID + "' AND BrandID='" + brandID + "'");
                            int EntryID = 0;
                            if (eid != "")
                            {
                                EntryID = Convert.ToInt32(eid);
                            }

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
                        if (deliveryBalance > 0)
                        {
                            SQLQuery.ExecNonQry("INSERT INTO SalesExcessItems (ProductName, InvoiceID, ExcessQty, EntryBy) VALUES ('" + lblPName.Text + "', '" + orderNo + "', '" + deliveryBalance + "', '" + lName + "')");
                        }
                        */
        }

        txtTotal.Text = totalAmount.ToString();
        Empty2Zero(txtVat);
        vatAmount = totalAmount * Convert.ToDecimal(txtVat.Text) / 100;
        txtPay.Text = Convert.ToString(totalAmount + vatAmount);
        string inWords = SQLQuery.DecimalToWords(Convert.ToDecimal(txtPay.Text).ToString("#.00"));

        //Save Transaction
        SqlCommand cmd2 = new SqlCommand("INSERT INTO Sales (InvNo, InvDate, SalesMode, CustomerID, CustomerName, PONo, PODate, Period, DeliveryDate, DeliveryTime, DeliveryLocation, TransportDetail, MaturityDays, OverDueDate, " +
                         " Remarks, InvoiceTotal, VATPercent, VATAmount, PayableAmount, CartonQty, ChallanNo, CollectedAmount, DueAmount, ProjectID, EntryBy, IsActive, InWords, Warehouse, VatChalNo, VatChalDate, InvCompany)" +
                                                    " VALUES (@InvNo, @InvDate, @SalesMode, @CustomerID, @CustomerName, @PONo, @PODate, @Period, @DeliveryDate, @DeliveryTime, @DeliveryLocation, @TransportDetail, @MaturityDays, @OverDueDate, " +
                         " @Remarks, @InvoiceTotal, @VATPercent, @VATAmount, @PayableAmount, @CartonQty, @ChallanNo, @CollectedAmount, @DueAmount, @ProjectID, @EntryBy, @IsActive, '" + inWords + "', '" + ddWarehouse.SelectedValue + "', '" + txtVatNo.Text + "', '" + txtChallanDate.Text + "', '"+ ddLevelX.SelectedValue+ "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@InvNo", orderNo);
        cmd2.Parameters.AddWithValue("@InvDate", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        cmd2.Parameters.AddWithValue("@SalesMode", "Direct");
        cmd2.Parameters.AddWithValue("@CustomerID", ddCustomer.SelectedValue);
        cmd2.Parameters.AddWithValue("@CustomerName", ddCustomer.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@PONo", txtPoNo.Text);
        cmd2.Parameters.AddWithValue("@PODate", podate);
        cmd2.Parameters.AddWithValue("@Period", txtPeriod.Text);

        cmd2.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(txtDeliveryDate.Text));
        cmd2.Parameters.AddWithValue("@DeliveryTime", txtTime.Text);
        cmd2.Parameters.AddWithValue("@DeliveryLocation", ddAddress.SelectedValue);
        cmd2.Parameters.AddWithValue("@TransportDetail", txtTransport.Text);
        cmd2.Parameters.AddWithValue("@MaturityDays", Convert.ToInt32(txtMDays.Text));
        cmd2.Parameters.AddWithValue("@OverDueDate", Convert.ToDateTime(txtOverDueDate.Text).ToString("yyyy-MM-dd"));

        cmd2.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        cmd2.Parameters.AddWithValue("@InvoiceTotal", Convert.ToDecimal(txtTotal.Text));
        cmd2.Parameters.AddWithValue("@VATPercent", Convert.ToDecimal(txtVat.Text));
        cmd2.Parameters.AddWithValue("@VATAmount", vatAmount);
        cmd2.Parameters.AddWithValue("@PayableAmount", Convert.ToDecimal(txtPay.Text));
        cmd2.Parameters.AddWithValue("@CartonQty", txtCarton.Text);

        cmd2.Parameters.AddWithValue("@ChallanNo", txtChallanNo.Text);
        cmd2.Parameters.AddWithValue("@CollectedAmount", 0);
        cmd2.Parameters.AddWithValue("@DueAmount", Convert.ToDecimal(txtPay.Text));
        cmd2.Parameters.AddWithValue("@ProjectID", lblProject.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@IsActive", 1);

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

        // 2 Auto voucher: For Customer & for VAT
        VoucherEntry.AutoVoucherEntry("6", "", "010104001", "030101001", Convert.ToDecimal(txtTotal.Text), orderNo, "", lName, Convert.ToDateTime(txtDeliveryDate.Text).ToString("yyyy-MM-dd"),"1");
        VoucherEntry.AutoVoucherEntry("10", "", "010104001", "010108007", vatAmount, orderNo, "", lName, Convert.ToDateTime(txtDeliveryDate.Text).ToString("yyyy-MM-dd"),"1");

        //InActivae PO
        if (succ > 0)
        {
            //int[] indexes = this.lvOrders.GetSelectedIndices();
            //for (int index = 0; index < indexes.Length; index++)
            //{
            //string poNo = this.lvOrders.Items[indexes[index]].Text;
            SQLQuery.ExecNonQry("Update Orders set DeliveryStatus='A' where OrderID='" + txtInvoiceNo.Text + "'");

            int qtyBalance = Convert.ToInt32(SQLQuery.ReturnString("SELECT ISNULL(SUM(Quantity),0)-ISNULL(SUM(DeliveredQty),0) FROM OrderDetails where OrderID='" + txtInvoiceNo.Text + "'"));

            if (qtyBalance <= 0)
            {
                SQLQuery.ExecNonQry("Update Orders set DeliveryStatus='D' where OrderID='" + txtInvoiceNo.Text + "'");
            }
            //}

            SQLQuery.ExecNonQry("UPDATE OrderDetails SET OrderID='" + orderNo + "' WHERE EntryBy='" + lName + "' AND CustomerID='" + ddCustomer.SelectedValue + "' AND OrderID=''");

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

            if (chkNonVat.Checked)
            {
            string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice-nvat.aspx?inv=" + orderNo;
            ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");

            }
            else
            {
            string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=" + orderNo;
            ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");


            }

        }

    }

    private string Empty2Zero(TextBox textBox)
    {
        if (textBox.Text == "")
        {
            textBox.Text = "0";
        }
        return "0";
    }

    //private void UpdateDeliveredQty(string orderNo, int EntryID, int delivered, int remain, int isActive)
    //{
    //    int preDelivered = Convert.ToInt32(SQLQuery.ReturnString("SELECT ISNULL(DeliveredQty,0) FROM OrderDetails where id=" + EntryID));
    //    delivered = delivered + preDelivered;
    //    SQLQuery.ExecNonQry("UPDATE OrderDetails SET DeliveryInvoice='" + orderNo + "', DeliveredQty=" + delivered + ", QtyBalance=" + remain + " WHERE Id=" + EntryID);
    //}

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
        //LoadOrderDetails();

        string shortStock = VerifyStock();
        if (shortStock != "")
        {
            //LoadOrderDetails();
            lblMsg2.Attributes.Add("class", "xerp_stop");
            lblMsg2.Text = "<b>Stort Stock:</b> " + shortStock;
            Notify("<b>Stort Stock:</b> " + shortStock, "error", lblMsg2);
        }
    }

    //private void LoadOrderDetails()
    //{
    //    string orders = "";
    //    int[] indexes = this.lvOrders.GetSelectedIndices();

    //    for (int index = 0; index < indexes.Length; index++)
    //    {
    //        orders += this.lvOrders.Items[indexes[index]].Text + ",";
    //    }
    //    orders = orders.TrimEnd(',');
    //    txtInv.Text = orders;
    //    if (ddSalesMode.SelectedValue == "LC")
    //    {
    //        ltrInv.Text = "Selected PI# " + orders;
    //    }
    //    else
    //    {
    //        ltrInv.Text = "Selected PO# " + orders;
    //    }


    //    string sqlStatement = "";
    //    int i = 0;

    //    string[] allOrders = orders.Split(',');
    //    string orderLast = "";
    //    foreach (string orderId in allOrders)
    //    {
    //        if (i == 0)
    //        {
    //            sqlStatement = " (OrderID ='" + orderId + "') ";
    //        }
    //        else
    //        {
    //            sqlStatement = sqlStatement + "OR (OrderID ='" + orderId + "') ";
    //        }
    //        orderLast = orderId;
    //        i++;
    //    }

    //    sqlStatement = "SELECT DISTINCT ProductName, (Select UnitCost from SaleDetails  where Id=(Select ISNULL(MAX(Id),0) from SaleDetails WHERE ProductName=OrderDetails.ProductName)) AS rate, SUM(Quantity) AS qty, SUM(ItemTotal) AS amt, SUM(DeliveredQty) AS dQty, (SUM(Quantity)-SUM(DeliveredQty)) AS pQty, " +
    //                    " AVG(UnitWeight) AS UnitWeight, SUM(TotalWeight) AS TotalWeight,  SizeId, ProductID, BrandID FROM OrderDetails" +
    //                    " WHERE " + sqlStatement +
    //                    " GROUP BY ProductName, SizeId,ProductID,BrandID";

    //    DataTable dt = RunQuery.SQLQuery.ReturnDataTable(sqlStatement);
    //    DataTable dt1 = new DataTable();
    //    DataRow dr1 = null;
    //    dt1.Columns.Add(new DataColumn("ProductName", typeof(string)));
    //    dt1.Columns.Add(new DataColumn("rate", typeof(string)));
    //    dt1.Columns.Add(new DataColumn("qty", typeof(string)));
    //    dt1.Columns.Add(new DataColumn("amt", typeof(string)));
    //    dt1.Columns.Add(new DataColumn("dQty", typeof(string)));

    //    dt1.Columns.Add(new DataColumn("pQty", typeof(string)));
    //    dt1.Columns.Add(new DataColumn("UnitWeight", typeof(string)));
    //    dt1.Columns.Add(new DataColumn("TotalWeight", typeof(string)));
    //    dt1.Columns.Add(new DataColumn("iCode", typeof(string)));
    //    dt1.Columns.Add(new DataColumn("QtyPack", typeof(string)));

    //    foreach (DataRow citydr in dt.Rows)
    //    {
    //        string ProductName = citydr["ProductName"].ToString();
    //        string rate = citydr["rate"].ToString();
    //        string qty = citydr["qty"].ToString();

    //        string dQty = citydr["dQty"].ToString();
    //        string pQty = citydr["pQty"].ToString();
    //        string UnitWeight = citydr["UnitWeight"].ToString();
    //        UnitWeight = Convert.ToDecimal(UnitWeight).ToString("0.00#");

    //        string SizeId = citydr["SizeId"].ToString();
    //        string ProductID = citydr["ProductID"].ToString();
    //        string BrandID = citydr["BrandID"].ToString();
    //        string iCode = SizeId + "-" + ProductID + "-" + BrandID;

    //        if (rate == "")
    //        {
    //            rate = RunQuery.SQLQuery.ReturnString("Select UnitCost from OrderDetails where Id=(Select ISNULL(MAX(Id),0) from OrderDetails WHERE SizeId='" + SizeId + "' AND BrandID='" + BrandID + "' AND ProductID='" + ProductID + "')");
    //            if (rate == "")
    //            {
    //                rate = "0";
    //            }
    //        }
    //        string amt = Convert.ToString(Convert.ToDecimal(qty) * Convert.ToDecimal(rate)); //citydr["amt"].ToString();
    //        string TotalWeight = Convert.ToString(Convert.ToDecimal(qty) * Convert.ToDecimal(UnitWeight)); //citydr["TotalWeight"].ToString();

    //        dr1 = dt1.NewRow();
    //        dr1["ProductName"] = ProductName;
    //        dr1["rate"] = rate;
    //        dr1["qty"] = qty;
    //        dr1["amt"] = amt;
    //        dr1["dQty"] = dQty;
    //        dr1["pQty"] = pQty;
    //        dr1["UnitWeight"] = UnitWeight;
    //        dr1["TotalWeight"] = TotalWeight;
    //        dr1["iCode"] = iCode;
    //        dr1["QtyPack"] = "0";
    //        dt1.Rows.Add(dr1);
    //        //Accounting.VoucherEntry.ProductionStockEntry(orderNo, "Finished Items Stock-in", sizeId, companyFor, icode, iName, "", "3", "0", rQty.ToString(), "Auto Stock-out of Processed item during finished Stock-in", "Production", "Processed", ddLocation.SelectedItem.Text, lName);
    //    }

    //    ItemGrid.EmptyDataText = "Select PO No. For Items to view...";
    //    ItemGrid.DataSource = dt1;

    //    //SqlCommand cmd = new SqlCommand(sqlStatement, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
    //    //cmd.Connection.Open();
    //    //ItemGrid.DataSource = cmd.ExecuteReader();
    //    //cmd.Connection.Close();

    //    txtAddress.Text = RunQuery.SQLQuery.ReturnString("Select DeliveryAddress from Orders where OrderID ='" + orderLast + "'");
    //    ItemGrid.DataBind();
    //    //btnEdit.Visible = true;
    //}


    //Effected Line# 131, 440
    private string VerifyStock()
    {
        string shortItem = "";// "<span style='line-height:14px;'>";
        int shortQty = 0;

        foreach (GridViewRow row in ItemGrid.Rows)
        {
            TextBox lblPName = row.FindControl("lblPName") as TextBox;
            TextBox txtpQty = row.FindControl("txtpQty") as TextBox;
            Label lblEntryId = row.FindControl("lblEntryId") as Label;

            string sizeId = RunQuery.SQLQuery.ReturnString("SELECT SizeID FROM OrderDetails where Id='" + lblEntryId.Text + "'");
            string brandID = RunQuery.SQLQuery.ReturnString("SELECT brandID FROM OrderDetails where Id='" + lblEntryId.Text + "'");
            string productID = RunQuery.SQLQuery.ReturnString("SELECT productID FROM OrderDetails where Id='" + lblEntryId.Text + "'");

            string stockQty = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where SizeID='" + sizeId + "' AND BrandID='" + brandID + "' AND ProductID='" + productID + "'");
            decimal orderQty = Convert.ToDecimal(txtpQty.Text);
            decimal availableQty = Convert.ToDecimal(stockQty);
            if (orderQty > availableQty)
            {
                shortQty++;
                shortItem = shortItem + "<br>" + lblPName.Text + ": Available Qty. " + stockQty + " PCS.";
                // commented for not checking stock...
            }
        }

        return shortItem;
    }


    private void QtyinStock()
    {
        ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");
        txtStock.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where SizeID='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "'");
        txtWeight.Text = RunQuery.SQLQuery.ReturnString("Select UnitWeight from SaleDetails  where Id=(Select ISNULL(MAX(Id),0) from SaleDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");
        txtRate.Text = RunQuery.SQLQuery.ReturnString("Select UnitCost from SaleDetails  where Id=(Select ISNULL(MAX(Id),0) from SaleDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");

        if (txtWeight.Text == "")
        {
            txtWeight.Text = RunQuery.SQLQuery.ReturnString("Select UnitWeight from OrderDetails  where Id=(Select ISNULL(MAX(Id),0) from OrderDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");
            if (txtWeight.Text == "")
            {
                //txtWeight.Text = "0";
            }
        }
        if (txtRate.Text == "")
        {
            txtRate.Text = RunQuery.SQLQuery.ReturnString("Select UnitCost from OrderDetails  where Id=(Select ISNULL(MAX(Id),0) from OrderDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");
            if (txtRate.Text == "")
            {
                //txtRate.Text = "0";
            }
        }
        if (txtQty.Text != "" && txtRate.Text != "")
        {
            txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtRate.Text));
        }
    }
    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        txtQty.Focus();
        ddProduct.Focus();
    }
    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddCategory.DataBind();
        ddProduct.DataBind();
        QtyinStock();
        ddGrade.Focus();
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddProduct.DataBind();
        QtyinStock();
        ddCategory.Focus();
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
                SqlCommand cmde = new SqlCommand("SELECT ProductName FROM OrderDetails WHERE SizeId ='" + ddSize.SelectedValue + "' AND ProductID ='" + ddProduct.SelectedValue + "' AND BrandID ='" + ddBrand.SelectedValue + "' AND CustomerID='" + ddCustomer.SelectedValue + "'  AND  OrderID ='' AND EntryBy ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
                    //lblMsg2.Attributes.Add("class", "xerp_warning");
                    //lblMsg2.Text = "ERROR: Product Already exist or product selection box is empty!";
                }
            }
            else
            {
                ExecuteUpdate();
                btnAdd.Text = "Add";
                Notify("Saved Successfully", "success", lblMsg);
                //lblMsg2.Attributes.Add("class", "xerp_success");
                //lblMsg2.Text = "Item updated successfully";
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

    private string DataChk4Name(string val)
    {
       string val1 = val.ToLower().Trim();
        if (val1.IndexOf("n/a")>(-1) || val1.IndexOf("---") > (-1) || val1=="all")
        {
            val1 = "";
        }
        else
        {
            val1 = val;
        }
        return val1;
    }

    private string GenerateProductName()
    {
        string productName = SQLQuery.ReturnString("Select ProductName from SaleDetails  where Id=(Select ISNULL(MAX(Id),0) from SaleDetails WHERE SizeId='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + ddProduct.SelectedValue + "')");
        if (productName=="")
        {
            productName = DataChk4Name(ddSize.SelectedItem.Text) + " " + DataChk4Name(ddBrand.SelectedItem.Text) + " " + DataChk4Name(ddProduct.SelectedItem.Text);

            if (chkMerge.Checked == true)
            {
                productName = DataChk4Name(ddSize.SelectedItem.Text) + " " + DataChk4Name(ddBrand.SelectedItem.Text) + " " + DataChk4Name(ddGrade.SelectedItem.Text) + " " + DataChk4Name(ddProduct.SelectedItem.Text);
            }
        }
        return productName;
    }

    private void InsertData()
    {
        string lName = Page.User.Identity.Name.ToString();
        string productName = GenerateProductName();

        decimal weight = 0;
        if (txtWeight.Text != "")
        {
            weight = Convert.ToDecimal(txtWeight.Text);
        }

        SqlCommand cmd2 = new SqlCommand("INSERT INTO OrderDetails (  CustomerID, OrderID, SizeId, ProductID, BrandID, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy) VALUES ('" + ddCustomer.SelectedValue + "', @OrderID, @SizeId, @ProductID, @BrandID, @ProductName, @UnitCost, @Quantity, @UnitWeight, '" + ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@OrderID", "");
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
        string productName = GenerateProductName();

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


    private void BindItemGrid()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, TotalWeight, UnitType, ItemTotal, QtyPack FROM OrderDetails WHERE EntryBy=@EntryBy AND CustomerID='" + ddCustomer.SelectedValue + "' AND OrderID='' ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        cmd.Connection.Open();
        ItemGrid.EmptyDataText = "No items to view...";
        ItemGrid.DataSource = cmd.ExecuteReader();
        ItemGrid.DataBind();
        cmd.Connection.Close();

        LoadSummesion("");
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
                SqlCommand cmd7 = new SqlCommand("DELETE OrderDetails WHERE Id=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                cmd7.ExecuteNonQuery();
                cmd7.Connection.Close();

                BindItemGrid();

                Notify("Item deleted from order ...", "warn", lblMsg2);
                //lblMsg2.Attributes.Add("class", "xerp_warning");
                //lblMsg2.Text = "Item deleted from order ...";
            }
            else
            {
                Notify("Order is Locked! There is some pending delivery...", "warn", lblMsg2);
                //lblMsg2.Attributes.Add("class", "xerp_warning");
                //lblMsg2.Text = "Order is Locked! There is some pending delivery...";
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg2);
        }
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

            Notify("Edit mode activated ...", "info", lblMsg2);
            //lblMsg2.Attributes.Add("class", "xerp_info");
            //lblMsg2.Text = "Edit mode activated ...";
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg2);
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


            ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + productID + "'");
            txtStock.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where SizeID='" + ddSize.SelectedValue + "' AND BrandID='" + ddBrand.SelectedValue + "' AND ProductID='" + productID + "'");

            string brand = dr[2].ToString();
            ddCustomer.SelectedValue = RunQuery.SQLQuery.ReturnString("Select CustomerID FROM CustomerBrands where BrandID='" + brand + "'");
            ddBrand.DataBind();
            ddBrand.SelectedValue = brand;
            txtRate.Text = dr[3].ToString();
            txtWeight.Text = dr[4].ToString();
            txtQty.Text = dr[5].ToString();

            //txtWeight.Text = dr[6].ToString();

            txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtRate.Text));
        }
        cmd.Connection.Close();

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

    protected void lbRefresh_OnClick(object sender, EventArgs e)
    {
        ddSize.DataBind();
        ddBrand.DataBind();
        ddGrade.DataBind();
        ddCategory.DataBind();
        ddProduct.DataBind();
        Notify("Data has been refreshed", "info", lblMsg2);
    }
}