using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Accounting;
using RunQuery;


public partial class app_Payment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            txtChqDate.Text = DateTime.Today.Date.ToShortDateString();
            txtColDate.Text = DateTime.Today.Date.ToShortDateString();
            lvOrders.DataBind();
            ddVendor.DataBind();
            lblBalance.Text = Bal(ddVendor.SelectedValue);
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

    private string Bal(string partyId)
    {
        string actBal = Convert.ToString(Accounting.VoucherEntry.SupplierBalance(partyId));
        string chq = SQLQuery.ReturnString("Select ISNULL(SUM(ChqAmt),0) from Cheque where PartyID='" + partyId + "' AND ChqStatus='In Hand' ");
        decimal gross = Convert.ToDecimal(actBal) - Convert.ToDecimal(chq);//SQLQuery.ReturnString("Select ISNULL(SUM(ChqAmt),0) from Cheque where PartyID='" + partyId + "' AND ChqStatus='Passed' "));
        actBal = "Actual Balance: " + actBal + ", Pending Chq: " + chq;
        return actBal;
    }

    private bool IsChequeNo(bool status = true)
    {
        if (ddMode.SelectedValue == "Cheque")
        {
            string chqno = SQLQuery.ReturnString("SELECT ChequeNo FROM Cheque WHERE ChequeNo='" + txtDetail.Text + "' AND TrType='Payment'");
            if (txtDetail.Text == "")
            {
                status = false;
            }
        }
        return status;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string msg = "";
        try
        {
            string mpo = Page.User.Identity.Name.ToString();
            mpo = mpo.Trim();

            foreach (GridViewRow gvRow in ItemGrid.Rows)
            {
                msg = "Invalid Collection Amount For Invoices!";
                TextBox txtColl = gvRow.FindControl("lblDueAmount") as TextBox;
                decimal x = Convert.ToDecimal(txtColl.Text);
            }
            msg = "";

            if (Convert.ToDecimal(txtReceived.Text) > 0)
            {
                if (IsChequeNo())
                {
                    SavePayment();
                    txtReceived.Text = "0";
                    ltrInv.Text = "";
                    GridView1.DataBind();
                    Notify("Payment Entry Saves Successfully.", "success", lblMsg);
                }
                else
                {
                    Notify("Please input valid cheque No!", "warn", lblMsg);
                }

            }
            else
            {
                lblMsg.Text = "Please Check Payment Amount Properly";
                lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#6e000a");
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    private void SavePayment()
    {
        string lName = Page.User.Identity.Name.ToString();
        string dt = Convert.ToDateTime(txtChqDate.Text).ToString("yyyy-MM-dd");
        string payId = RunQuery.SQLQuery.ReturnString("Select 'Pay-'+ CONVERT(varchar, (ISNULL (max(PaymentID),0)+1001 )) from Payment");
        string collDate = txtChqDate.Text;
        string description = "Cheque Payment: " + txtDetail.Text + ". Pay ID: " + payId;
        string isAppr = "P";
        if (ddMode.SelectedValue == "Cash")
        {
            collDate = txtColDate.Text;
            isAppr = "A";
        }

        if (Convert.ToDecimal(txtReceived.Text) > 0)
        {
            //Payment Entry
            

            SqlCommand cmd2 =new SqlCommand("INSERT INTO Payment (PaymentNo, PaymentDate, PartyID, PartyName, PurchaseInvNo, PaidAmount, PayType, Remark, EntryBy) VALUES (@PaymentNo, @PaymentDate, @PartyID, @PartyName, @PurchaseInvNo, @PaidAmount, @PayType, @Remark, @EntryBy)",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.AddWithValue("@PaymentNo", payId);
            cmd2.Parameters.AddWithValue("@PaymentDate", Convert.ToDateTime(txtColDate.Text));
            cmd2.Parameters.AddWithValue("@PartyID", ddVendor.SelectedValue);
            cmd2.Parameters.AddWithValue("@PartyName", ddVendor.SelectedItem.Text);
            cmd2.Parameters.AddWithValue("@PurchaseInvNo", ddPaidTo.SelectedValue);

            cmd2.Parameters.AddWithValue("@TdsRate", 0); //Convert.ToDecimal(txtTDSRate.Text));
            cmd2.Parameters.AddWithValue("@TDS", Convert.ToDecimal(txtTDSAmount.Text));
            cmd2.Parameters.AddWithValue("@TotalAmt", Convert.ToDecimal(txtTotal.Text) - Convert.ToDecimal(txtTDSAmount.Text));
            cmd2.Parameters.AddWithValue("@CollectedAmt", Convert.ToDecimal(txtReceived.Text));

            cmd2.Parameters.AddWithValue("@PaidAmount", Convert.ToDecimal(txtReceived.Text));
            cmd2.Parameters.AddWithValue("@PayType", ddMode.SelectedValue);
            cmd2.Parameters.AddWithValue("@Remark", txtRemark.Text);
            cmd2.Parameters.AddWithValue("@EntryBy", lName);

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();

            string invNoforstatement = "";
            string tdsNotes = "";
            foreach (GridViewRow gvRow in ItemGrid.Rows)
            {
                Label lblInvoiceNo = gvRow.FindControl("lblInvoiceNo") as Label;
                invNoforstatement += lblInvoiceNo.Text + ", ";

                string invoiceNo = lblInvoiceNo.Text;
                string InvoiceDate = Convert.ToDateTime(gvRow.Cells[2].Text).ToString("yyyy-MM-dd");


                Label lblInvoiceTotal = gvRow.FindControl("lblInvoiceTotal") as Label;
                string VATAmount = gvRow.Cells[4].Text;
                Label lblPayableAmount = gvRow.FindControl("lblPayableAmount") as Label;
                TextBox lblTDSRate = gvRow.FindControl("lblTDSRate") as TextBox;

                //if (lblTDSRate != null && Convert.ToDecimal(lblTDSRate.Text) > 0)
                //{
                //    tdsNotes += "Bill No. " + lblInvoiceNo.Text + " TDS Amount " + lblTDSRate.Text + ", ";
                //}


                decimal tdsAmt = Convert.ToDecimal(lblTDSRate.Text);
                // tdsAmt = Convert.ToDecimal(lblTDSRate.Text);



                //TextBox lblVDSRate = gvRow.FindControl("lblVDSRate") as TextBox;
                //CheckBox chkVDS = gvRow.FindControl("chkVDS") as CheckBox;
                //decimal vdsAmt = Convert.ToDecimal(lblInvoiceTotal.Text) * Convert.ToDecimal(lblVDSRate.Text) / 100;
                //int vdsVatIncl = 0;
                //if (chkVDS.Checked)
                //{
                //    vdsAmt = Convert.ToDecimal(lblPayableAmount.Text) * Convert.ToDecimal(lblVDSRate.Text) / 100;
                //    vdsVatIncl = 1;
                //}
              
                decimal netPayable = Convert.ToDecimal(lblPayableAmount.Text) - tdsAmt;
                Label lblCollectedAmount = gvRow.FindControl("lblCollectedAmount") as Label;// Old Received
              
                decimal payable = netPayable-Convert.ToDecimal(lblCollectedAmount.Text);

                TextBox txtColl = gvRow.FindControl("lblDueAmount") as TextBox;
                SQLQuery.Empty2Zero(txtColl);

                SQLQuery.ExecNonQry(@"INSERT INTO [dbo].[PaymentInvoices]([PID],[InvNo],[InvoiceDate],[PaymentDate],[InvoiceTotal],[VATAmount],[PayableAmount],[TDSAmount],[NetPayable],[PrePaymentAmount],[Payable],[PaidAmount],[EntryBy],EntryDate,IsApproved)
     VALUES ('"+payId+"','" +invoiceNo+"','"+InvoiceDate+"','"+Convert.ToDateTime(txtColDate.Text).ToString("yyyy-MM-dd")+"','"+lblInvoiceTotal.Text+"','"+VATAmount+"','"+lblPayableAmount.Text+"','"+tdsAmt+"','"+netPayable+"','"+lblCollectedAmount.Text+"','"+payable+"','"+txtColl.Text.TrimStart().TrimEnd()+"','"+lName+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+isAppr+"')");

                //insert transaction
                if (ddMode.SelectedValue == "Cash")
                {
                    string accHead =RunQuery.SQLQuery.ReturnString("Select AccHeadID FROM  Settings_Transaction where TransactionType='Payment'");

                    Accounting.VoucherEntry.TransactionEntry(payId, txtColDate.Text, ddVendor.SelectedValue,
                        ddVendor.SelectedItem.Text, "Cash Payment", txtReceived.Text, "0", "0", "Payment", "Supplier",accHead, lName, "1");

                 
                    /*
                    accHead =RunQuery.SQLQuery.ReturnString("Select AccHeadID FROM  Settings_Transaction where TransactionType='Cash'");
                    Accounts Link disabled
                //Cancel old voucher for edit
                VoucherEntry.AutoVoucherEntry("7", "", "", 0, payId, lName, Convert.ToDateTime(txtColDate.Text).ToString("yyyy-MM-dd"), "0");

                string voucherNo = "Auto-" + DateTime.Now.Year.ToString() + "-" + RunQuery.SQLQuery.ReturnString("Select ISNULL(MAX(VID),0)+1001 From VoucherMaster");
                VoucherEntry.InsertVoucherMaster(voucherNo, "Paid to " + ddVendor.SelectedItem.Text, "9", "020102006", "010101001", Convert.ToDecimal(txtPaid.Text), Convert.ToDecimal(txtPaid.Text), lName, Convert.ToDateTime(txtColDate.Text).ToString("yyyy-MM-dd"), payId);
                */
                }
                else if (ddMode.SelectedValue == "Cheque")
                {
                    //string chqBank = RunQuery.SQLQuery.ReturnString("Select BankName FROM  Banks where BankId=(Select BankID FROM BankAccounts where ACID='" + ddBank.SelectedValue + "')");
                    string chqBank =SQLQuery.ReturnString("Select BankID FROM BankAccounts where ACID='" + ddBank.SelectedValue +"'");
                    string chqBankBranch =SQLQuery.ReturnString("Select Address FROM BankAccounts where ACID='" + ddBank.SelectedValue +"'");

                    SqlCommand cmd4 =
                        new SqlCommand(
                            "INSERT INTO Cheque (TrType, TrID, ChequeNo, ChqBank, ChqBankBranch, BankAccNo, ChqDate, ChqAmt, PartyID, ChequeName, Remark, EntryBy)" +
                            " VALUES (@TrType, @TrID, @ChequeNo, @ChqBank, @ChqBankBranch, '" + ddBank.SelectedValue +
                            "', @ChqDate, @ChqAmt, @PartyID, @ChequeName, @Remark, @EntryBy)",
                            new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                    cmd4.Parameters.AddWithValue("@TrType", "Payment");
                    cmd4.Parameters.AddWithValue("@TrID", payId);
                    cmd4.Parameters.AddWithValue("@ChequeNo", txtDetail.Text);
                    cmd4.Parameters.AddWithValue("@ChqBank", chqBank);
                    cmd4.Parameters.AddWithValue("@ChqBankBranch", chqBankBranch);

                    cmd4.Parameters.AddWithValue("@ChqDate", Convert.ToDateTime(txtChqDate.Text));
                    cmd4.Parameters.AddWithValue("@ChqAmt", Convert.ToDecimal(txtReceived.Text));
                    cmd4.Parameters.AddWithValue("@PartyID", Convert.ToInt32(ddVendor.SelectedValue));
                    cmd4.Parameters.AddWithValue("@ChequeName", ddVendor.SelectedItem.Text);
                    cmd4.Parameters.AddWithValue("@Remark", txtRemark.Text);
                    cmd4.Parameters.AddWithValue("@EntryBy", lName);

                    cmd4.Connection.Open();
                    cmd4.ExecuteNonQuery();
                    cmd4.Connection.Close();
                }



                int[] indexes = lvOrders.GetSelectedIndices();
             
                for (int index = 0; index < indexes.Length; index++)
                {

                    string inv = lvOrders.Items[indexes[index]].Value;
                    string previousPayment = "0";
                    previousPayment = Convert.ToDecimal(SQLQuery.ReturnString("SELECT PreviousPayment FROM Purchase WHERE InvNo='" + invoiceNo + "'")).ToString();
                    string prePayment = (Convert.ToDecimal(previousPayment) + Convert.ToDecimal(txtColl.Text)).ToString();
                    decimal remainBalance = payable - Convert.ToDecimal(txtColl.Text) ;
                    string isActive ="P";
                    if (remainBalance < 1)
                    {
                        isActive = "A";
                    }
                  
                   // SQLQuery.ExecNonQry("Update Purchase SET PaymentID='" + payId + "', IsApproved='" + isActive + "' WHERE InvNo='" + inv + "'");
                    SQLQuery.ExecNonQry("UPDATE PaymentInvoices SET IsApproved='" + isActive + "'  where InvNo='" + inv + "'");
                    SQLQuery.ExecNonQry("UPDATE Purchase SET PaymentID='" + payId + "', IsApproved= '" + isActive + "',  ItemTotal='" + remainBalance + "', PurchaseTotal='" + remainBalance + "',PreviousPayment='" + prePayment + "',PaidAmount='" + txtColl.Text + "' where InvNo='" + inv + "'");

                }
                lvOrders.DataBind();
            }

            if (Convert.ToDecimal(txtTDSAmount.Text) > 0)
            {
                string accHeadTds = SQLQuery.ReturnString("SELECT AccHeadTDS FROM Party WHERE PartyID = '" + ddVendor.SelectedValue + "' AND Type = 'vendor'");
                if (accHeadTds != "" && accHeadTds != "0")
                {
                    description = "TDS deducted by " + ddVendor.SelectedItem.Text + ". against " + tdsNotes + ". Payment# " + payId + ". " + txtRemark.Text;
                    Accounting.VoucherEntry.AutoVoucherEntry("13", description, accHeadTds, "010104001", Convert.ToDecimal(txtTDSAmount.Text), payId, "", lName, Convert.ToDateTime(txtColDate.Text).ToString("yyyy-MM-dd"), "1");
                    Accounting.VoucherEntry.TransactionEntry(payId, txtColDate.Text, ddVendor.SelectedValue, ddVendor.SelectedItem.Text, description, "0", txtTDSAmount.Text.ToString(), "0", "Payment", "Vendor", "0101144455", lName, "1");
                }
            }
            if (ddMode.SelectedValue == "Cash")
            {
                description = "Cash Payment to " + ddVendor.SelectedItem.Text + ", Bill No.: " + invNoforstatement.TrimEnd(',') + " Payment# " + payId + ". " + txtRemark.Text;
                VoucherEntry.TransactionEntry(payId, txtColDate.Text, ddVendor.SelectedValue, ddVendor.SelectedItem.Text, description, "0", txtReceived.Text, "0", "Payment", "Vendor", "1122334455", lName, "1");
                VoucherEntry.AutoVoucherEntry("5", description, "010101001", "010104001", Convert.ToDecimal(txtReceived.Text), payId, "", lName, Convert.ToDateTime(txtColDate.Text).ToString("yyyy-MM-dd"), "1");
            }
            else if (ddMode.SelectedValue == "Cheque")
            {
                SqlCommand cmd4 = new SqlCommand("INSERT INTO Cheque (TrType, TrID, ChequeNo, ChqBank, ChqBankBranch, ChqDate, ChqAmt, PartyID, ChequeName, Remark, EntryBy, BankAccNo, ChqStatus, DepositAccountID, ApproveDate, ApproveBy)" +
                                                            " VALUES (@TrType, @TrID, @ChequeNo, @ChqBank, @ChqBankBranch, @ChqDate, @ChqAmt, @PartyID, @ChequeName, @Remark, @EntryBy, @BankAccNo, @ChqStatus, @DepositAccId, @ApproveDate, @ApproveBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd4.Parameters.AddWithValue("@TrType", "Payment");
                cmd4.Parameters.AddWithValue("@TrID", payId);
                cmd4.Parameters.AddWithValue("@ChequeNo", txtDetail.Text);
                cmd4.Parameters.AddWithValue("@ChqBank", ddBank.SelectedValue);
               cmd4.Parameters.AddWithValue("@ChqBankBranch", txtBranch.Text);

                cmd4.Parameters.AddWithValue("@ChqDate", Convert.ToDateTime(txtChqDate.Text));
                //cmd4.Parameters.AddWithValue("@ChqAmt", Convert.ToDecimal(txtReceived.Text));
                cmd4.Parameters.AddWithValue("@ChqAmt", Convert.ToDecimal(txtReceived.Text));
                cmd4.Parameters.AddWithValue("@PartyID", Convert.ToInt32(ddVendor.SelectedValue));
                cmd4.Parameters.AddWithValue("@ChequeName", ddVendor.SelectedItem.Text);
                cmd4.Parameters.AddWithValue("@Remark", txtRemark.Text);
                cmd4.Parameters.AddWithValue("@EntryBy", lName);
                cmd4.Parameters.AddWithValue("@BankAccNo", ddBank.SelectedValue); //New
                cmd4.Parameters.AddWithValue("@ChqStatus", "Passed"); //New
                cmd4.Parameters.AddWithValue("@DepositAccId", Convert.ToInt32(ddBank.SelectedValue)); //New
                cmd4.Parameters.AddWithValue("@ApproveDate", dt); //New
                cmd4.Parameters.AddWithValue("@ApproveBy", lName); //New

                cmd4.Connection.Open();
                cmd4.ExecuteNonQuery();
                cmd4.Connection.Close();

                //if (txtRemark.Text == "")
                //{
                //    descriptionForBank = "Collection from " + ddCustomer.SelectedItem.Text + " Cheque# " + txtDetail.Text + ", Bill No.: " + invNoforstatement.TrimEnd(',') + " Collection# " + colId;
                //}
                //else
                //{
                //    descriptionForBank = txtRemark.Text;
                //}

               string descriptionForBank = "Payment to " + ddVendor.SelectedItem.Text + " Cheque# " + txtDetail.Text + ", Bill No.: " + invNoforstatement.TrimEnd(',') + " Payment# " + payId + ". " + txtRemark.Text;
                //Update party balance
                Accounting.VoucherEntry.TransactionEntry(payId, dt, ddVendor.SelectedValue, ddVendor.SelectedItem.Text, descriptionForBank, txtReceived.Text, "0",  "0", "Payment", "Vendor", "010105005", lName, "1");

                //Insert Bank Tranction
                Accounting.VoucherEntry.AutoVoucherEntry("5", descriptionForBank, "010101002", "010104001", Convert.ToDecimal(txtReceived.Text), payId, "", lName, dt, "1");
                Accounting.VoucherEntry.TransactionEntry(payId, dt, ddBank.SelectedValue, ddBank.SelectedItem.Text, descriptionForBank,  "0", txtReceived.Text, "0", "Deposit", "Bank", "010102002", lName, "1");
            }
            else
            {
                description = "Other Payment to " + ddVendor.SelectedItem.Text + ", Bill No.: " + invNoforstatement.TrimEnd(',') + " Payment# " + payId + ". " + txtRemark.Text;
                VoucherEntry.TransactionEntry(payId, txtColDate.Text, ddVendor.SelectedValue, ddVendor.SelectedItem.Text, description, "0", txtReceived.Text, "0", "Collection", "Vendor", "1122334455", lName, "1");
                VoucherEntry.AutoVoucherEntry("5", description, "1122334455", "010104001", Convert.ToDecimal(txtReceived.Text), payId, "", lName, Convert.ToDateTime(txtColDate.Text).ToString("yyyy-MM-dd"), "1");
            }

        }
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddMode.SelectedValue == "Cheque")
        {
            chqpanel.Visible = true;
        }
        else
        {
            chqpanel.Visible = false;
            txtReceived.Focus();
        }
    }

    protected void lvOrders_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadOrderDetails();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }

    }
    private void LoadOrderDetails()
    {
        string sqlStatement = LoadOrderList("");

        txtInvAmt.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(ItemTotal),0) AS totalAmt from Purchase WHERE " + sqlStatement);
        txtVATAmt.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(VATService),0) AS totalAmt from Purchase WHERE " + sqlStatement);
        txtTotal.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(PurchaseTotal),0) AS totalAmt from Purchase WHERE " + sqlStatement);
      
        txtPaid.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(PreviousPayment),0) AS totalAmt from Purchase WHERE " + sqlStatement);
        txtDue.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(Payable),0) AS totalAmt from Purchase WHERE " + sqlStatement);
        //txtReceived.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(PaidAmount),0) AS totalAmt from Purchase WHERE " + sqlStatement);

        txtReceived.Text = txtDue.Text;

        if (Convert.ToDecimal(txtPaid.Text) > 0)
        {
            paidpanel.Visible = true;
        }
        else
        {
            paidpanel.Visible = false;
        }

        DataTable dt = SQLQuery.ReturnDataTable("SELECT  ItemTotal, VATService, PurchaseTotal, PreviousPayment, Payable from Purchase WHERE " + sqlStatement);

        //ItemGrid.DataSource = cmd.ExecuteReader();
        //ItemGrid.DataBind();
        //btnEdit.Visible = true;
    }

    //private string LoadOrderList(string sqlStatement)
    //{
    //    string orders = ""; string desc = "";
    //    decimal amt = 0;
    //    int matDays = Convert.ToInt32(SQLQuery.ReturnString("Select MatuirityDays from Party where PartyID='" + ddVendor.SelectedValue + "'"));

    //    int[] indexes = this.lvOrders.GetSelectedIndices();

    //    for (int index = 0; index < indexes.Length; index++)
    //    {
    //        string inv = lvOrders.Items[indexes[index]].Value;
    //        //DateTime mDate = Convert.ToDateTime(SQLQuery.ReturnString("Select InvDate from Sales where InvNo='" + inv + "'"));

    //        //mDate = mDate.AddDays(matDays);
    //        //orders += inv + ",";

    //        string ttl = SQLQuery.ReturnString("Select PurchaseTotal from Purchase where InvNo='" + inv + "'");

    //        desc += "Inv# " + inv + " Total Amount: " + ttl + " <br>";
    //        amt += Convert.ToDecimal(ttl);
    //    }

    //    orders = orders.TrimEnd(',');
    //    ltrInv.Text = desc;
    //    txtPaid.Text = amt.ToString();


    //    //int i = 0;

    //    //string[] allOrders = orders.Split(',');
    //    //foreach (string orderId in allOrders)
    //    //{
    //    //    if (i == 0)
    //    //    {
    //    //        sqlStatement = " (InvNo ='" + orderId + "') ";
    //    //    }
    //    //    else
    //    //    {
    //    //        sqlStatement = sqlStatement + "OR (InvNo ='" + orderId + "') ";
    //    //    }
    //    //    i++;
    //    //}
    //    return sqlStatement;
    //}

    private string LoadOrderList(string sqlStatement)
    {
        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataRow dr1 = null;

        dt1.Columns.Add(new DataColumn("InvoiceNo", typeof(string)));
        dt1.Columns.Add(new DataColumn("OrderDate", typeof(DateTime)));
        //dt1.Columns.Add(new DataColumn("MatuirityDate", typeof(DateTime)));
        //dt1.Columns.Add(new DataColumn("OverdueDays", typeof(string)));
        dt1.Columns.Add(new DataColumn("ItemTotal", typeof(string)));
        dt1.Columns.Add(new DataColumn("VatService", typeof(string)));
        dt1.Columns.Add(new DataColumn("PurchaseTotal", typeof(string)));
        //dt1.Columns.Add(new DataColumn("TDSRate", typeof(string)));
        dt1.Columns.Add(new DataColumn("TDSAmount", typeof(string)));
        //dt1.Columns.Add(new DataColumn("VDSRate", typeof(string)));
        //dt1.Columns.Add(new DataColumn("VDSAmount", typeof(string)));
        //dt1.Columns.Add(new DataColumn("BadDebt", typeof(string)));
        dt1.Columns.Add(new DataColumn("NetPayable", typeof(string)));
        dt1.Columns.Add(new DataColumn("PreviousPayment", typeof(string)));
        dt1.Columns.Add(new DataColumn("Payable", typeof(string)));
        dt1.Columns.Add(new DataColumn("PaidAmount", typeof(string)));

        string orders = ""; string desc = "";
        int matDays = Convert.ToInt32(SQLQuery.ReturnString("Select MatuirityDays from Party where PartyID='" + ddVendor.SelectedValue + "'"));

        int[] indexes = this.lvOrders.GetSelectedIndices();

        for (int index = 0; index < indexes.Length; index++)
        {
            string inv = lvOrders.Items[indexes[index]].Value;
            DateTime mDate = Convert.ToDateTime(SQLQuery.ReturnString("Select OrderDate from Purchase where InvNo='" + inv + "'"));

            mDate = mDate.AddDays(matDays);
            orders += inv + ",";

            //string poNo = SQLQuery.ReturnString("Select PONo from Sales where InvNo='" + inv + "'");

            //desc += "Inv# " + inv + " Maturity Date:" + mDate.ToString("dd/MM/yyyy") + " PO# " + poNo + " <br>";

            //Add Datarow
            //DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) SaleID, InvNo, InvDate, SalesMode, CustomerID, CustomerName, PONo, PODate, Period, DeliveryDate, DeliveryTime, DeliveryLocation, TransportDetail, 
            //             MaturityDays, OverdueDays, Remarks, InvoiceTotal, VATPercent, VATAmount, PayableAmount, TDSRate, TDSAmount, VDSrate, VDSAmount,
            //            BadDebt, CartonQty, ChallanNo, CollectedAmount, DueAmount, NetPayable, 
            //             EntryBy, EntryDate, IsActive, OverDueDate, InWords, Warehouse, VatChalNo, VatChalDate from Sales where InvNo='" + inv + "'");
            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) PID, InvNo, PurchaseFor, OrderDate, SupplySource, BillNo, BillDate, SupplierID, SupplierName, ChallanNo, ChallanDate, ItemTotal, PurchaseDiscount, VatService, OtherExp, PurchaseTotal, TransportType, 
                         WarehouseID, StockLocationID, PaymentMode, TDSAmount, NetPayable, PreviousPayment, Payable, PaidAmount, Remarks, PaymentID, IsApproved, ReceiveDate, Doc, EntryBy, EntryDate
             from Purchase where InvNo='" + inv + "'");

            foreach (DataRow drx in dtx.Rows)
            {
                dr1 = dt1.NewRow();
                dr1["InvoiceNo"] = inv;
                dr1["OrderDate"] = Convert.ToDateTime(drx["OrderDate"].ToString()).ToString("yyyy-MM-dd");
                //dr1["MatuirityDate"] = Convert.ToDateTime(mDate).ToString("yyyy-MM-dd");
                //dr1["OverdueDays"] = drx["OverdueDays"].ToString();
                dr1["ItemTotal"] = drx["ItemTotal"].ToString();
                dr1["VatService"] = drx["VatService"].ToString();
                dr1["PurchaseTotal"] = drx["PurchaseTotal"].ToString();
                dr1["PaidAmount"] = drx["PaidAmount"].ToString();
                //dr1["TDSRate"] = drx["TDSRate"].ToString();
                dr1["TDSAmount"] = drx["TDSAmount"].ToString();
                //dr1["VDSRate"] = drx["VDSrate"].ToString();
                //dr1["VDSAmount"] = drx["VDSAmount"].ToString();
                //dr1["BadDebt"] = drx["BadDebt"].ToString();
                dr1["NetPayable"] = drx["NetPayable"].ToString();
                dr1["PreviousPayment"] = drx["PreviousPayment"].ToString();
                dr1["Payable"] = drx["Payable"].ToString();
                dr1["PaidAmount"] = drx["PaidAmount"].ToString();

                dt1.Rows.Add(dr1);
            }
        }

        orders = orders.TrimEnd(',');
        //txtInv.Text = orders;
        ltrInv.Text = desc;

        int i = 0;

        string[] allOrders = orders.Split(',');
        foreach (string orderId in allOrders)
        {
            if (i == 0)
            {
                sqlStatement = " (InvNo ='" + orderId + "') ";
            }
            else
            {
                sqlStatement = sqlStatement + "OR (InvNo ='" + orderId + "') ";
            }
            i++;
        }


        //ItemGrid.EmptyDataText = "Select Invoice No. Press <em>Ctrl+Click</em> for multiple selection";
        ItemGrid.DataSource = dt1;
        ItemGrid.DataBind();

        return sqlStatement;
    }
    protected void ddVendor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GeneratePartyDetail();
            lvOrders.DataBind();
            GridView1.DataBind();
            lblBalance.Text = Bal(ddVendor.SelectedValue);
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    private void GeneratePartyDetail()
    {
        try
        {
            /*
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            int mDays = (-1) * Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select MatuirityDays from Party where PartyID='" + ddCustomer.SelectedValue + "'")));

            string lastMaturityDate = DateTime.Today.AddDays(mDays).ToString("yyyy-MM-dd");

            lblImitured.Text = SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" + ddCustomer.SelectedValue + "'  AND IsActive=1 AND InvDate>'" + lastMaturityDate + "'");
            string totalMatured = SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" + ddCustomer.SelectedValue + "' AND IsActive=1 AND InvDate<='" + lastMaturityDate + "'");
            //string totalPaid = SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" + ddCustomer.SelectedValue + "' AND IsActive=1 AND InvDate<='" + lastMaturityDate + "'");
            lblMatured.Text = totalMatured;

            lblOverdue.Text = SQLQuery.CalculateOverDueDays(ddCustomer.SelectedValue);
            lblPendingChq.Text = SQLQuery.ReturnString("Select ISNULL(SUM(ChqAmt),0) FROM Cheque where PartyID='" + ddCustomer.SelectedValue + "' AND ([ChqStatus] <>'Cancelled') AND ([ChqStatus] <>'Passed')");
            lblCurrBalance.Text = Convert.ToString(Convert.ToDecimal(lblImitured.Text) + Convert.ToDecimal(lblMatured.Text));
            //SQLQuery.ReturnString("Select SUM(Dr)-Sum(Cr) FROM Transactions where HeadID='" + ddCustomer.SelectedValue + "'");
             */
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    protected void txtInvTotal_OnTextChanged(object sender, EventArgs e)
    {

    }

    protected void lblTDSRate_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
           

            foreach (GridViewRow gvRow in ItemGrid.Rows)
            {
             
                Label txtPayableAmount = gvRow.FindControl("lblPayableAmount") as Label;
                TextBox txtTDS = gvRow.FindControl("lblTDSRate") as TextBox;
                txtTDSAmount.Text = txtTDS.Text;


                string netPayable = (Convert.ToDecimal(txtPayableAmount.Text) - Convert.ToDecimal(txtTDS.Text)).ToString();
               
                Label lblNetPayable=gvRow.FindControl("lblNetPayable") as Label;
                lblNetPayable.Text = netPayable;


                Label lblCollectedAmount = gvRow.FindControl("lblCollectedAmount") as Label;
                SQLQuery.Empty2Zero(lblCollectedAmount);
                string payable = (Convert.ToDecimal(netPayable) - Convert.ToDecimal(lblCollectedAmount.Text)).ToString();
               
                Label lblCollectable = gvRow.FindControl("lblCollectable") as Label;
                lblCollectable.Text = payable;
                txtInvTotal.Text = payable;

                TextBox lblDueAmount = gvRow.FindControl("lblDueAmount") as TextBox;
               // lblDueAmount.Text = payable;
               // txtPaid.Text = lblDueAmount.Text;

                //decimal netPayable = Convert.ToDecimal(lblPayableAmount.Text) - tdsAmt - vdsAmt;
                //Label lblCollectedAmount = gvRow.FindControl("lblCollectedAmount") as Label;// Old Received
                //decimal receivable = netPayable - Convert.ToDecimal(lblCollectedAmount.Text);
                //TextBox txtColl = gvRow.FindControl("lblDueAmount") as TextBox;

            }
        }
        catch (Exception ex)
        {

            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    protected void lblDueAmount_OnTextChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in ItemGrid.Rows)
        {
            TextBox lblDueAmount = gvRow.FindControl("lblDueAmount") as TextBox;
            txtReceived.Text = lblDueAmount.Text;
        }
    }
}
