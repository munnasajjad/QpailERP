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

public partial class app_CollectionAdjustment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //txtTDSRate.Attributes.Add("onkeyup", "__doPostBack('UpdatePanel1', '');");
        //txtVDSRate.Attributes.Add("onkeyup", "__doPostBack('UpdatePanel1', '');");

        if (!IsPostBack)
        {
            txtChqDate.Text = DateTime.Today.Date.ToShortDateString();
            txtColDate.Text = DateTime.Today.Date.ToShortDateString();
            txtChqPassDate.Text = DateTime.Today.Date.ToShortDateString();

            ddCustomer.DataBind();
            lvOrders.DataBind();
            GeneratePartyDetail();
        }
    }

    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox)
        { (Parent as TextBox).Text = string.Empty; }
        else
        {
            foreach (Control c in Parent.Controls)
                ClearControls(c);
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
    // Getting CollectionNo
    public string CollectionIDNo()
    {
        SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (count(CollectionID),0)+1001 )) from Collection", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        string CollectionNo = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();
        cmd.Connection.Dispose();
        return "Coll-" + CollectionNo;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string msg = "";
        try
        {
            if (Convert.ToDecimal(txtAdvanceBalance.Text) >= Convert.ToDecimal(txtReceived.Text))
            {
                // Check if all collection amount was supplied
                foreach (GridViewRow gvRow in ItemGrid.Rows)
                {
                    msg = "Invalid Collection Amount For Invoices!";
                    TextBox txtColl = gvRow.FindControl("lblDueAmount") as TextBox;
                    //decimal x = Convert.ToDecimal(txtColl.Text);
                }
                msg = "";

                //string isValid = "Valid";
                //if (ddCollMode.SelectedValue != "Cash")
                //{
                //    string chqno = SQLQuery.ReturnString("SELECT ChequeNo FROM Cheque where ChequeNo='" + txtDetail.Text + "'");
                //    if (chqno != "")
                //    {
                //        isValid = "inValid";
                //    }
                //    if (txtDetail.Text == "")
                //    {
                //        isValid = "inValid";
                //    }
                //}
                //===========================================================================

                if (cbAdvanceCollection.Checked == true)
                {
                    if (Convert.ToDecimal(txtReceived.Text) > 0 & txtReceived.Text != "")
                    {
                        //if (isValid == "Valid")
                        //{
                        SaveCollection();

                        //Inactivate Orders
                        if (Convert.ToDecimal(txtDue.Text) <= Convert.ToDecimal(txtCollection.Text))
                        {
                            string sqlStatement = LoadOrderList("");
                            SQLQuery.ExecNonQry("UPDATE sales SET IsActive=0 WHERE " + sqlStatement);
                        }
                        else
                        {
                            int[] indexes = this.lvOrders.GetSelectedIndices();
                            for (int index = 0; index < indexes.Length; index++)
                            {
                                string poNo = this.lvOrders.Items[indexes[index]].Value;
                                SQLQuery.ExecNonQry("Update Orders set DeliveryStatus='A' where OrderID='" + poNo + "'");

                                //int qtyBalance = Convert.ToInt32(SQLQuery.ReturnString("SELECT SUM(Quantity)-SUM(DeliveredQty) FROM OrderDetails where OrderID='" + poNo + "'"));

                                //if (qtyBalance <= 0)
                                //{
                                //    SQLQuery.ExecNonQry("Update Orders set DeliveryStatus='D' where OrderID='" + poNo + "'");
                                //}
                            }
                        }

                        txtCollection.Text = "0";
                        txtAdjust.Text = "0";
                        txtDetail.Text = "";

                        lvOrders.DataBind();
                        GeneratePartyDetail();
                        GridView1.DataBind();
                        ItemGrid.DataBind();
                        Notify("Saved Successfully", "success", lblMsg);
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Advance Collection Entry Saved Successfully.";  //================================
                        //}
                        //else
                        //{
                        //    Notify("Cheque No. already exist or Invalid!", "warn", lblMsg);
                        //}
                    }
                    else
                    {
                        Notify("Please input the Collected Amount Field", "warn", lblMsg);
                    }
                }
                else
                {
                    if (Convert.ToDecimal(txtCollection.Text) > 0 & txtCollection.Text != "")
                    {
                        //if (isValid == "Valid")
                        //{
                        SaveCollection();

                        //Inactivate Orders
                        if (Convert.ToDecimal(txtDue.Text) <= Convert.ToDecimal(txtCollection.Text))
                        {
                            string sqlStatement = LoadOrderList("");
                            SQLQuery.ExecNonQry("UPDATE sales SET IsActive=0 WHERE " + sqlStatement);
                        }
                        else
                        {
                            int[] indexes = this.lvOrders.GetSelectedIndices();
                            for (int index = 0; index < indexes.Length; index++)
                            {
                                string poNo = this.lvOrders.Items[indexes[index]].Value;
                                SQLQuery.ExecNonQry("Update Orders set DeliveryStatus='A' where OrderID='" + poNo + "'");

                                //int qtyBalance = Convert.ToInt32(SQLQuery.ReturnString("SELECT SUM(Quantity)-SUM(DeliveredQty) FROM OrderDetails where OrderID='" + poNo + "'"));

                                //if (qtyBalance <= 0)
                                //{
                                //    SQLQuery.ExecNonQry("Update Orders set DeliveryStatus='D' where OrderID='" + poNo + "'");
                                //}
                            }
                        }

                        txtCollection.Text = "0";
                        txtAdjust.Text = "0";
                        txtDetail.Text = "";

                        lvOrders.DataBind();
                        GeneratePartyDetail();
                        GridView1.DataBind();
                        ItemGrid.DataBind();
                        Notify("Saved Successfully", "success", lblMsg);
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Collection Entry Saved Successfully.";
                        //}
                        //else
                        //{
                        //    Notify("Cheque No. already exist or Invalid!", "warn", lblMsg);
                        //}
                    }
                    else
                    {
                        Notify("Please input the Collected Amount Field", "warn", lblMsg);
                    }
                }
            }
            else
            {
                Notify("The balance of advance amount can not be greater than the adjusting amount!", "warn", lblMsg);
            }

        }
        catch (Exception ex)
        {
            Notify("<b>" + msg + "</b> " + ex.ToString(), "error", lblMsg);
        }
    }

    private void SaveCollection()
    {
        string colId = CollectionIDNo();
        string dt = Convert.ToDateTime(txtChqPassDate.Text).ToString("yyyy-MM-dd");
        string lName = Page.User.Identity.Name.ToString();
        
        string isAppr = "P", descriptionForBank = "", description = "Cheque Payment: " + txtDetail.Text + ". Col ID: " + colId; ;
        /*if (ddCollMode.SelectedValue == "Cash")
        {
            isAppr = "A";
        }

        SQLQuery.Empty2Zero(txtAdjust);
        SqlCommand cmd2 = new SqlCommand("INSERT INTO Collection (CollectionNo, CollectionDate, CustomerID, CustomerName, SalesInvoiceNo, InvoiceAmt, TdsRate, TDS, VdsRate, VDS, BadDebt, TotalAmt, CollType, Custodian, AdjustmentAmt, CollectedAmt, Remark, IsApproved, EntryBy) VALUES (@CollectionNo, @CollectionDate, @CustomerID, @CustomerName, @SalesInvoiceNo, @InvoiceAmt, @TdsRate, @TDS, @VdsRate, @VDS, @BadDebt, @TotalAmt, @CollType, @Custodian, @AdjustmentAmt, @CollectedAmt, @Remark, @IsApproved, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@CollectionNo", colId);
        cmd2.Parameters.AddWithValue("@CollectionDate", Convert.ToDateTime(txtColDate.Text));
        cmd2.Parameters.AddWithValue("@CustomerID", ddCustomer.SelectedValue);
        cmd2.Parameters.AddWithValue("@CustomerName", ddCustomer.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@SalesInvoiceNo", GetOrderNo());

        cmd2.Parameters.AddWithValue("@InvoiceAmt", Convert.ToDecimal(txtTotal.Text));
        cmd2.Parameters.AddWithValue("@TdsRate", 0); //Convert.ToDecimal(txtTDSRate.Text));
        cmd2.Parameters.AddWithValue("@TDS", Convert.ToDecimal(txtTDS.Text));
        cmd2.Parameters.AddWithValue("@VdsRate", 0); //Convert.ToDecimal(txtVDSRate.Text));
        cmd2.Parameters.AddWithValue("@VDS", Convert.ToDecimal(txtVDS.Text));
        cmd2.Parameters.AddWithValue("@BadDebt", Convert.ToDecimal(txtAdjust.Text));
        cmd2.Parameters.AddWithValue("@TotalAmt", Convert.ToDecimal(txtTotal.Text) - Convert.ToDecimal(txtTDS.Text) - Convert.ToDecimal(txtVDS.Text));
        cmd2.Parameters.AddWithValue("@CollType", ddCollMode.SelectedValue);

        cmd2.Parameters.AddWithValue("@Custodian", txtCustodian.Text);
        cmd2.Parameters.AddWithValue("@AdjustmentAmt", Convert.ToDecimal(txtAdjust.Text));
        cmd2.Parameters.AddWithValue("@CollectedAmt", Convert.ToDecimal(txtReceived.Text));
        cmd2.Parameters.AddWithValue("@Remark", txtRemark.Text);
        cmd2.Parameters.AddWithValue("@IsApproved", isAppr);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        */

        string invNoforstatement = "";
        string tdsNotes = "";
        string badDebtNotes = "";

        //Save Gridview Data
        foreach (GridViewRow gvRow in ItemGrid.Rows)
        {
            Label lblInvoiceNo = gvRow.FindControl("lblInvoiceNo") as Label;
            invNoforstatement += lblInvoiceNo.Text + ", ";

            string invoiceNo = lblInvoiceNo.Text;
            string InvoiceDate = Convert.ToDateTime(gvRow.Cells[2].Text).ToString("yyyy-MM-dd");
            string MatuirityDate = Convert.ToDateTime(gvRow.Cells[3].Text).ToString("yyyy-MM-dd");
            string OverdueDays = gvRow.Cells[4].Text;

            Label lblInvoiceTotal = gvRow.FindControl("lblInvoiceTotal") as Label;
            string VATAmount = gvRow.Cells[6].Text;
            Label lblPayableAmount = gvRow.FindControl("lblPayableAmount") as Label;
            TextBox lblTDSRate = gvRow.FindControl("lblTDSRate") as TextBox;
            TextBox txtBadDebt = gvRow.FindControl("txtBadDebt") as TextBox;
            if (lblTDSRate != null && Convert.ToDecimal(lblTDSRate.Text) > 0)
            {
                tdsNotes += "Bill No. " + lblInvoiceNo.Text + " TDS Amount " + lblTDSRate.Text + ", ";
            }
            if (txtBadDebt != null && Convert.ToDecimal(txtBadDebt.Text) > 0)
            {
                badDebtNotes += "Bill No. " + lblInvoiceNo.Text + " Bad debt Amount " + txtBadDebt.Text + ", ";
            }

            CheckBox chkTDS = gvRow.FindControl("chkTDS") as CheckBox;

            decimal tdsAmt = Convert.ToDecimal(lblTDSRate.Text);
            int tdsVatIncl = 0;
            if (chkTDS.Checked)
            {
                tdsAmt = Convert.ToDecimal(lblTDSRate.Text);
                tdsVatIncl = 1;
            }

            TextBox lblVDSRate = gvRow.FindControl("lblVDSRate") as TextBox;
            CheckBox chkVDS = gvRow.FindControl("chkVDS") as CheckBox;
            decimal vdsAmt = Convert.ToDecimal(lblInvoiceTotal.Text) * Convert.ToDecimal(lblVDSRate.Text) / 100;
            int vdsVatIncl = 0;
            if (chkVDS.Checked)
            {
                vdsAmt = Convert.ToDecimal(lblPayableAmount.Text) * Convert.ToDecimal(lblVDSRate.Text) / 100;
                vdsVatIncl = 1;
            }

            decimal netPayable = Convert.ToDecimal(lblPayableAmount.Text) - tdsAmt - vdsAmt;
            Label lblCollectedAmount = gvRow.FindControl("lblCollectedAmount") as Label;// Old Received
            decimal receivable = netPayable - Convert.ToDecimal(lblCollectedAmount.Text);
            TextBox txtColl = gvRow.FindControl("lblDueAmount") as TextBox;

            if (txtColl.Text == "")
            {
                txtColl.Text = lblPayableAmount.Text;
            }

            SQLQuery.ExecNonQry(@"INSERT INTO [CollectionInvoices] (CollectionNo, [InvNo], [InvoiceDate], [MaturityDate], [PaymentDate], [OverdueDays], [InvoiceTotal], [VATAmount], [PayableAmount], [TDSRate], [TdsVatIncluded], [TDSAmount], [VDSrate], [VdsVatIncluded], [VDSAmount], [BadDebt], [NetPayable], [PreCollectedAmount], ReceivableAmount, [CollectedAmount], [EntryBy])
            VALUES ('" + colId + "', '" + invoiceNo + "', '" + InvoiceDate + "', '" + MatuirityDate + "', '" + Convert.ToDateTime(txtColDate.Text).ToString("yyyy-MM-dd") + "', '" + OverdueDays + "', '" + lblInvoiceTotal.Text + "', '" + VATAmount + "', '" + lblPayableAmount.Text + "', '" + lblTDSRate.Text + "', '" + tdsVatIncl + "', '" + tdsAmt + "', '" + lblVDSRate.Text + "', '" + vdsVatIncl + "', '" + vdsAmt + "', '" + txtBadDebt.Text + "', '" + netPayable + "', '" + lblCollectedAmount.Text + "', '" + receivable + "', '" + txtColl.Text.TrimStart().TrimEnd() + "', '" + lName + "')");


            //InActivate Orders
            decimal remainBalance = receivable - Convert.ToDecimal(txtColl.Text) - Convert.ToDecimal(txtBadDebt.Text);
            int isActive = 1;
            if (remainBalance < 1)
            {
                isActive = 0;
            }

            int cycleDays = SQLQuery.ReturnInvCycleDays(invoiceNo);
            SQLQuery.ExecNonQry("UPDATE CollectionInvoices set IsApproved='" + isActive + "', OverdueDays='" + cycleDays + "', PaymentDate='" + dt + "' where InvNo='" + invoiceNo + "'");
            SQLQuery.ExecNonQry("UPDATE Sales SET IsActive=" + isActive + ", TDSRate=" + lblTDSRate.Text + ", TDSAmount=" + tdsAmt + ", VDSrate=" + lblVDSRate.Text + ", VDSAmount=" + vdsAmt + ", BadDebt=" + txtBadDebt.Text + ", CollectedAmount=(CollectedAmount+" + txtColl.Text + "), DueAmount=" + remainBalance + "  where InvNo='" + invoiceNo + "'");

        }


        if (Convert.ToDecimal(txtTDS.Text) > 0)
        {
            string accHeadTds = SQLQuery.ReturnString("SELECT AccHeadTDS FROM Party WHERE PartyID = '" + ddCustomer.SelectedValue + "' AND Type = 'customer'");
            if (accHeadTds != "" && accHeadTds != "0")
            {
                description = "TDS deducted by " + ddCustomer.SelectedItem.Text + ". against " + tdsNotes + ". Collection# " + colId + ". " + txtRemark.Text;
                Accounting.VoucherEntry.AutoVoucherEntry("13", description, accHeadTds, "010104001", Convert.ToDecimal(txtTDS.Text), colId, "", lName, Convert.ToDateTime(txtColDate.Text).ToString("yyyy-MM-dd"), "1");
                Accounting.VoucherEntry.TransactionEntry(colId, txtColDate.Text, ddCustomer.SelectedValue, ddCustomer.SelectedItem.Text, description, "0", txtTDS.Text.ToString(), "0", "Collection", "Customer", "0101144455", lName, "1");
            }
        }

        decimal adjustAmt = (Convert.ToDecimal(txtAdjust.Text));
        if (Convert.ToDecimal(adjustAmt) > 0)
        {
            description = "Bad debt ( A/R) from " + ddCustomer.SelectedItem.Text + ". against " + badDebtNotes + ". Collection# " + colId;
            Accounting.VoucherEntry.AutoVoucherEntry("14", description, "040101021", "010104001", Convert.ToDecimal(adjustAmt), colId, "", lName, Convert.ToDateTime(txtColDate.Text).ToString("yyyy-MM-dd"), "1");
            VoucherEntry.TransactionEntry(colId, txtColDate.Text, ddCustomer.SelectedValue, ddCustomer.SelectedItem.Text, description, "0", adjustAmt.ToString(), "0", "Collection", "Customer", "040101021", lName, "1");
        }

        descriptionForBank = "Adjustment for " + ddCustomer.SelectedItem.Text + " Bill No.:" + invNoforstatement.TrimEnd(',');
        //Update party balance
        VoucherEntry.TransactionEntry(colId, dt, ddCustomer.SelectedValue, ddCustomer.SelectedItem.Text, descriptionForBank, txtReceived.Text, "0", "0", "Adjustment", "Adjustment", "AdvColl010105", lName, "1");

    }

    private string GetOrderNo()
    {
        string orders = "";
        int[] indexes = this.lvOrders.GetSelectedIndices();

        for (int index = 0; index < indexes.Length; index++)
        {
            orders += this.lvOrders.Items[indexes[index]].Value + ",";
        }
        orders = orders.TrimEnd(',');
        return orders;
    }

    protected void ddCollMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddCollMode.SelectedValue == "Cash")
        {
            chqpanel.Visible = false;
            lblAmtText.Text = "Collected Amount";
        }
        else
        {
            chqpanel.Visible = true;
            lblAmtText.Text = "Cheque Amount";
        }
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        GridView1.Visible = false;

        SqlDataAdapter da;
        SqlDataReader dr;
        DataSet ds;
        int recordcount = 0;
        int ic = 0;

        SqlCommand cmd2z = new SqlCommand("SELECT [ColType], [PartyName], [ChqDetail], [ChqDate], [PaidAmount] FROM [Collection] WHERE ([CollectionDate] = @CollectionDate) ORDER BY [CollectionID] DESC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2z.Parameters.Add("@CollectionDate", SqlDbType.DateTime).Value = txtColDate.Text;

        da = new SqlDataAdapter(cmd2z);
        ds = new DataSet("Board");

        cmd2z.Connection.Open();
        da.Fill(ds, "Board");
        cmd2z.Connection.Close();

        DataTable dt1 = ds.Tables["Board"];

        //GridView2.DataSource = dt1;
        //GridView2.DataBind();
    }

    protected void lbOrders_SelectedIndexChanged(object sender, EventArgs e)
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
        txtInvAmt.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(InvoiceTotal),0) AS totalAmt from sales WHERE " + sqlStatement);
        txtVATAmt.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(VATAmount),0) AS totalAmt from sales WHERE " + sqlStatement);
        txtTotal.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(PayableAmount),0) AS totalAmt from sales WHERE " + sqlStatement);
        txtPaid.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(CollectedAmount),0) AS totalAmt from sales WHERE " + sqlStatement);
        txtDue.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(DueAmount),0) AS totalAmt from sales WHERE " + sqlStatement);

        txtCollection.Text = txtDue.Text;

        if (Convert.ToDecimal(txtPaid.Text) > 0)
        {
            paidpanel.Visible = true;
        }
        else
        {
            paidpanel.Visible = false;
        }

        DataTable dt = SQLQuery.ReturnDataTable("SELECT InvoiceTotal, VATAmount, PayableAmount, CollectedAmount, DueAmount from sales WHERE " + sqlStatement);

        //ItemGrid.DataSource = cmd.ExecuteReader();
        //ItemGrid.DataBind();
        //btnEdit.Visible = true;
    }

    private string LoadOrderList(string sqlStatement)
    {
        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataRow dr1 = null;

        dt1.Columns.Add(new DataColumn("InvoiceNo", typeof(string)));
        dt1.Columns.Add(new DataColumn("InvoiceDate", typeof(DateTime)));
        dt1.Columns.Add(new DataColumn("MatuirityDate", typeof(DateTime)));
        dt1.Columns.Add(new DataColumn("OverdueDays", typeof(string)));
        dt1.Columns.Add(new DataColumn("InvoiceTotal", typeof(string)));
        dt1.Columns.Add(new DataColumn("VATAmount", typeof(string)));
        dt1.Columns.Add(new DataColumn("PayableAmount", typeof(string)));
        dt1.Columns.Add(new DataColumn("TDSRate", typeof(string)));
        dt1.Columns.Add(new DataColumn("TDSAmount", typeof(string)));
        dt1.Columns.Add(new DataColumn("VDSRate", typeof(string)));
        dt1.Columns.Add(new DataColumn("VDSAmount", typeof(string)));
        dt1.Columns.Add(new DataColumn("BadDebt", typeof(string)));
        dt1.Columns.Add(new DataColumn("NetPayable", typeof(string)));
        dt1.Columns.Add(new DataColumn("CollectedAmount", typeof(string)));
        dt1.Columns.Add(new DataColumn("DueAmount", typeof(string)));

        string orders = ""; string desc = "";
        int matDays = Convert.ToInt32(SQLQuery.ReturnString("Select MatuirityDays from Party where PartyID='" + ddCustomer.SelectedValue + "'"));

        int[] indexes = this.lvOrders.GetSelectedIndices();

        for (int index = 0; index < indexes.Length; index++)
        {
            string inv = lvOrders.Items[indexes[index]].Value;
            DateTime mDate = Convert.ToDateTime(SQLQuery.ReturnString("Select InvDate from Sales where InvNo='" + inv + "'"));

            mDate = mDate.AddDays(matDays);
            orders += inv + ",";

            //string poNo = SQLQuery.ReturnString("Select PONo from Sales where InvNo='" + inv + "'");

            //desc += "Inv# " + inv + " Maturity Date:" + mDate.ToString("dd/MM/yyyy") + " PO# " + poNo + " <br>";

            //Add Datarow
            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) SaleID, InvNo, InvDate, SalesMode, CustomerID, CustomerName, PONo, PODate, Period, DeliveryDate, DeliveryTime, DeliveryLocation, TransportDetail, 
                         MaturityDays, OverdueDays, Remarks, InvoiceTotal, VATPercent, VATAmount, PayableAmount, TDSRate, TDSAmount, VDSrate, VDSAmount,
                        BadDebt, CartonQty, ChallanNo, CollectedAmount, DueAmount, NetPayable, 
                         EntryBy, EntryDate, IsActive, OverDueDate, InWords, Warehouse, VatChalNo, VatChalDate from Sales where InvNo='" + inv + "'");

            foreach (DataRow drx in dtx.Rows)
            {
                dr1 = dt1.NewRow();
                dr1["InvoiceNo"] = inv;
                dr1["InvoiceDate"] = Convert.ToDateTime(drx["InvDate"].ToString()).ToString("yyyy-MM-dd");
                dr1["MatuirityDate"] = Convert.ToDateTime(mDate).ToString("yyyy-MM-dd");
                dr1["OverdueDays"] = drx["OverdueDays"].ToString();
                dr1["InvoiceTotal"] = drx["InvoiceTotal"].ToString();
                dr1["VATAmount"] = drx["VATAmount"].ToString();
                dr1["PayableAmount"] = drx["PayableAmount"].ToString();
                dr1["TDSRate"] = drx["TDSRate"].ToString();
                dr1["TDSAmount"] = drx["TDSAmount"].ToString();
                dr1["VDSRate"] = drx["VDSrate"].ToString();
                dr1["VDSAmount"] = drx["VDSAmount"].ToString();
                dr1["BadDebt"] = drx["BadDebt"].ToString();
                dr1["NetPayable"] = drx["NetPayable"].ToString();
                dr1["CollectedAmount"] = drx["CollectedAmount"].ToString();
                dr1["DueAmount"] = drx["DueAmount"].ToString();

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

    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GeneratePartyDetail();
            lvOrders.DataBind();
            GridView1.DataBind();
            SQLQuery.CalculateOverDueDays(ddCustomer.SelectedValue);
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
            lvOrders.DataBind();

            string today = DateTime.Now.ToString("yyyy-MM-dd");
            int mDays = (-1) * Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select MatuirityDays from Party where PartyID='" + ddCustomer.SelectedValue + "'")));

            string lastMaturityDate = DateTime.Today.AddDays(mDays).ToString("yyyy-MM-dd");

            lblImitured.Text = SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" + ddCustomer.SelectedValue + "'  AND IsActive=1 AND InvDate >'" + lastMaturityDate + "'");
            string totalMatured = SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" + ddCustomer.SelectedValue + "' AND IsActive=1 AND InvDate <='" + lastMaturityDate + "'");
            //string totalPaid = SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" + ddCustomer.SelectedValue + "' AND IsActive=1 AND InvDate<='" + lastMaturityDate + "'");
            lblMatured.Text = totalMatured;

            lblOverdue.Text = SQLQuery.CalculateOverDueDays(ddCustomer.SelectedValue);
            lblPendingChq.Text = SQLQuery.ReturnString("Select ISNULL(SUM(ChqAmt),0) FROM Cheque where PartyID='" + ddCustomer.SelectedValue + "' AND ([ChqStatus] <>'Cancelled') AND ([ChqStatus] <>'Passed')");
            lblCurrBalance.Text = Convert.ToString(Convert.ToDecimal(lblImitured.Text) + Convert.ToDecimal(lblMatured.Text));
            txtAdvanceBalance.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(Cr),0) - ISNULL(SUM(Dr),0) FROM Transactions WHERE HeadID= '" + ddCustomer.SelectedValue + "' AND AccHeadID='AdvColl010105'");
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        //CalcVDSTDS();
    }
    //protected void txtTDSRate_TextChanged(object sender, EventArgs e)
    //{
    //    CalcVDSTDS();
    //    txtTDSRate.Focus();
    //}

    //protected void txtVDSRate_TextChanged(object sender, EventArgs e)
    //{
    //    CalcVDSTDS();
    //    txtTDSRate.Focus();
    //}
    //private void CalcVDSTDS()
    //{
    //    SQLQuery.Empty2Zero(txtTDSRate);
    //    SQLQuery.Empty2Zero(txtVDSRate);

    //}
    protected void txtColDate_OnTextChanged(object sender, EventArgs e)
    {
        txtChqDate.Text = txtColDate.Text;
        txtChqPassDate.Text = txtColDate.Text;
    }

    protected void cbStatus_CheckedChanged(object sender, EventArgs e)
    {
        if (cbAdvanceCollection.Checked == true)
        {
            tblInstruction.Visible = false;
            divOrders.Visible = false;
            divVATAmount.Visible = false;
            divPayableAmount.Visible = false;
            paidpanel.Visible = false;
            divTDSAmount.Visible = false;
            divInvoiceAmount.Visible = false;
            divVDSAmount.Visible = false;
            divInvcDueAmt.Visible = false;
            divPaidAmt.Visible = false;
            divAdjAmt.Visible = false;
            Label18.Text = "Advance Amount";
            txtReceived.ReadOnly = false; //=======================
            divPaidAmount.Visible = false;
        }
        else
        {
            tblInstruction.Visible = true;
            divOrders.Visible = true;
            divVATAmount.Visible = true;
            divPayableAmount.Visible = true;
            paidpanel.Visible = true;
            divTDSAmount.Visible = true;
            divInvoiceAmount.Visible = true;
            divVDSAmount.Visible = true;
            divInvcDueAmt.Visible = true;
            divPaidAmt.Visible = true;
            divAdjAmt.Visible = true;
            Label18.Text = "Act.Cheque Amount :";
            txtReceived.ReadOnly = true; //========================
            divPaidAmount.Visible = true;
        }
    }


    protected void txtInvTotal_TextChanged(object sender, EventArgs e)
    {

    }
}
