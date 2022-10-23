using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using RunQuery;

public partial class app_Collection_LC : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            //txtChqDate.Text = DateTime.Today.Date.ToShortDateString();
            txtColDate.Text = DateTime.Today.Date.ToShortDateString();

            ddCustomer.DataBind();
            GeneratePartyDetail();
            chkInvoices.DataBind();
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
        try
        {
            string isValid = "Valid";

            if (Convert.ToDecimal(txtCollection.Text) > 0 & txtCollection.Text != "")
            {
                if (isValid == "Valid")
                {
                    SaveCollection();



                    txtCollection.Text = "0";
                    GridView1.DataBind();
                    Notify("Collection Entry Saved Successfully.", "success", lblMsg);   
                }
                else
                {
                    Notify("Collection No. already exist or Invalid!", "error", lblMsg);   
                }
            }
            else
            {
                Notify("Please input the Collected Amount Field", "error", lblMsg);   
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);   
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

    private void SaveCollection()
    {
        string colID = CollectionIDNo();

        string lName = Page.User.Identity.Name.ToString();
        string isAppr = "A", detail = "LLC Bank Payment " + txtRemark.Text + ". Col ID: " + colID; ;
        SQLQuery.Empty2Zero(txtAdjust);

        SqlCommand cmd2 = new SqlCommand("INSERT INTO Collection (CollectionNo, CollectionDate, CustomerID, CustomerName, SalesInvoiceNo, InvoiceAmt, CollType, CollectedAmt, AdjustmentAmt, Remark, IsApproved, EntryBy) VALUES (@CollectionNo, @CollectionDate, @CustomerID, @CustomerName, @SalesInvoiceNo, @InvoiceAmt, @CollType, @CollectedAmt, @AdjustmentAmt, @Remark, @IsApproved, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@CollectionNo", colID);
        cmd2.Parameters.AddWithValue("@CollectionDate", Convert.ToDateTime(txtColDate.Text));
        cmd2.Parameters.AddWithValue("@CustomerID", ddCustomer.SelectedValue);
        cmd2.Parameters.AddWithValue("@CustomerName", ddCustomer.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@SalesInvoiceNo", lvOrders.SelectedValue);

        cmd2.Parameters.AddWithValue("@InvoiceAmt", Convert.ToDecimal(txtTotal.Text));
        //cmd2.Parameters.AddWithValue("@TDS", Convert.ToDecimal(txtTDS.Text));
        //cmd2.Parameters.AddWithValue("@VDS", Convert.ToDecimal(txtVDS.Text));
        //cmd2.Parameters.AddWithValue("@TotalAmt", Convert.ToDecimal(txtTotal.Text) - Convert.ToDecimal(txtTDS.Text) - Convert.ToDecimal(txtVDS.Text));
        cmd2.Parameters.AddWithValue("@CollType", "LLC");

        cmd2.Parameters.AddWithValue("@CollectedAmt", Convert.ToDecimal(txtCollection.Text));
        cmd2.Parameters.AddWithValue("@AdjustmentAmt", Convert.ToDecimal(txtAdjust.Text));
        cmd2.Parameters.AddWithValue("@Remark", txtRemark.Text + " Invoice No: " + lblChecked.Text);
        cmd2.Parameters.AddWithValue("@IsApproved", isAppr);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();


        //SQLQuery.ExecNonQry("Update Sales SET ");





        /*
        //if (DropDownList1.SelectedValue == "Cash")
        //{
            detail = "Collection: Coll. ID: " + colID;
            //Update party balance
            string accHead = RunQuery.SQLQuery.ReturnString("Select AccHeadID FROM  Settings_Transaction where TransactionType='Collection'");
            Accounting.VoucherEntry.TransactionEntry(colID, txtColDate.Text, ddCustomer.SelectedValue, ddCustomer.SelectedItem.Text, detail, "0", txtCollection.Text, "0", "Collection", "Customer", accHead, lName, "1");
            //Update cash balance
            accHead = RunQuery.SQLQuery.ReturnString("Select AccHeadID FROM  Settings_Transaction where TransactionType='Cash'");
            Accounting.VoucherEntry.AccVoucher(detail, colID, accHead, "", txtCollection.Text, "0", lName, "1");

            //InActivate Orders
            decimal payingAmt = Convert.ToDecimal(txtCollection.Text);
            
                string InvID = lvOrders.SelectedValue;
                decimal remainBalance = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT PayableAmount-CollectedAmount FROM Sales where InvNo='" + InvID + "'"));

                if (payingAmt > 0)
                {
                    if (remainBalance == payingAmt)
                    {
                        RunQuery.SQLQuery.ExecNonQry("UPDATE Sales SET IsActive=0, CollectedAmount=" + remainBalance + " where InvNo='" + InvID + "'");
                        payingAmt = 0;
                    }
                    else if (remainBalance > payingAmt)
                    {
                        RunQuery.SQLQuery.ExecNonQry("UPDATE Sales SET IsActive=1, CollectedAmount=" + payingAmt + " where InvNo='" + InvID + "'");
                        payingAmt = 0;
                    }
                    else
                    {
                        RunQuery.SQLQuery.ExecNonQry("UPDATE Sales SET IsActive=0, CollectedAmount=" + remainBalance + " where InvNo='" + InvID + "'");
                        payingAmt = payingAmt - remainBalance;
                    }
                }           

        //}
        //else
        //{
        //    SqlCommand cmd4 = new SqlCommand("INSERT INTO Cheque (TrType, TrID, ChequeNo, ChqBank, ChqBankBranch, ChqDate, ChqAmt, PartyID, ChequeName, Remark, EntryBy)" +
        //                                                " VALUES (@TrType, @TrID, @ChequeNo, @ChqBank, @ChqBankBranch, @ChqDate, @ChqAmt, @PartyID, @ChequeName, @Remark, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //    cmd4.Parameters.AddWithValue("@TrType", "Collection");
        //    cmd4.Parameters.AddWithValue("@TrID", colID);
        //    cmd4.Parameters.AddWithValue("@ChequeNo", txtDetail.Text);
        //    cmd4.Parameters.AddWithValue("@ChqBank", ddBank.SelectedValue);
        //    cmd4.Parameters.AddWithValue("@ChqBankBranch", txtBranch.Text);

        //    cmd4.Parameters.AddWithValue("@ChqDate", Convert.ToDateTime(txtChqDate.Text));
        //    cmd4.Parameters.AddWithValue("@ChqAmt", Convert.ToDecimal(txtCollection.Text));
        //    cmd4.Parameters.AddWithValue("@PartyID", Convert.ToInt32(ddCustomer.SelectedValue));
        //    cmd4.Parameters.AddWithValue("@ChequeName", ddCustomer.SelectedItem.Text);
        //    cmd4.Parameters.AddWithValue("@Remark", txtRemark.Text);
        //    cmd4.Parameters.AddWithValue("@EntryBy", lName);

        //    cmd4.Connection.Open();
        //    cmd4.ExecuteNonQuery();
        //    cmd4.Connection.Close();
        //}
         * 
         * */
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
        chkInvoices.DataBind();
        LoadOrderDetails();
    }

    private void LoadOrderDetails()
    {
        string orderId = lvOrders.SelectedValue;

        SqlCommand cmd = new SqlCommand("SELECT PiDate, PiNo, LcDate, LcExpDate, IssuingBank, IssueingBankBranch, AdvisingBankAccount, OrderID FROM  PI WHERE (LcNo = @InvNo)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.Parameters.Add("@InvNo", SqlDbType.VarChar).Value = orderId;

        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            txtPIDate.Text = Convert.ToDateTime(dr[0].ToString()).ToString("dd/MM/yyyy");
            txtLLCNo.Text = Convert.ToString(dr[1]);
            txtLLCDate.Text = Convert.ToDateTime(dr[2].ToString()).ToString("dd/MM/yyyy");
            txtLcExpDate.Text = Convert.ToDateTime(dr[3].ToString()).ToString("dd/MM/yyyy");
            txtIssuerBank.Text = SQLQuery.ReturnString("Select [BankName] FROM [Banks] where [BankId]='"+ Convert.ToString(dr[4])+"'");
            ddBank.SelectedValue = Convert.ToString(dr[6]);

            string poNo = Convert.ToString(dr[7]);
            txtTotal.Text = SQLQuery.ReturnString("Select PayableAmount from Orders where OrderID='" + orderId + "'");
            //txtPrePaid.Text = SQLQuery.ReturnString("Select PayableAmount from Orders where PoNo='" + poNo + "'");
        }


        txtDeliverd.Text = RunQuery.SQLQuery.ReturnString("SELECT ISNULL(SUM(PayableAmount),0) AS totalAmt from sales WHERE PONo= '" + orderId + "'");
        txtPrePaid.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(CollectedAmt),0) AS totalAmt from Collection WHERE SalesInvoiceNo= '" + orderId + "' AND CollType='LLC'");



        //txtDue.Text = Convert.ToString(Convert.ToDecimal(txtTotal.Text) - Convert.ToDecimal(txtPrePaid.Text), CultureInfo.InvariantCulture);
        //txtCollection.Text = Convert.ToString(Convert.ToDecimal(txtDeliverd.Text) - Convert.ToDecimal(txtPrePaid.Text));


        /*
        //txtInv.Text = orders;
        //ltrInv.Text = "Selected PI# " + orderId;

        string sqlStatement = " (InvNo ='" + orderId + "') ";
            
        sqlStatement = "SELECT ISNULL(SUM(PayableAmount),0) AS totalAmt WHERE " + sqlStatement;

         cmd = new SqlCommand(sqlStatement, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        string amt = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();

        txtTotal.Text = amt;
        txtCollection.Text = amt;

        //ItemGrid.EmptyDataText = "Select PO No. For Items to view...";
        //ItemGrid.DataSource = cmd.ExecuteReader();
        //ItemGrid.DataBind();
        //btnEdit.Visible = true;
         * */

        CalculateAmount();
    }

    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        GeneratePartyDetail();
        GridView1.DataBind();
    }

    private void GeneratePartyDetail()
    {
        lvOrders.DataBind();

        SqlCommand cmd2y = new SqlCommand("SELECT CollectionID, SalesInvoiceNo FROM Collection  Where IsApproved='P'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2y.Connection.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd2y);
        DataSet ds = new DataSet();
        da.Fill(ds, "Products");
        cmd2y.Connection.Close();
        int recordcount = ds.Tables[0].Rows.Count;
        DataTable dt = ds.Tables["Products"];

        string colID = "", invNo = "";
        decimal id = 0, md = 0, od = 0;
        string dtt = DateTime.Now.ToString("yyyy-MM-dd");

        foreach (DataRow row in dt.Rows)
        {
            colID = row["CollectionID"].ToString();
            invNo = row["SalesInvoiceNo"].ToString();

            string[] allOrders = invNo.Split(',');
            foreach (string InvNo in allOrders)
            {
                SqlCommand cmd = new SqlCommand("SELECT DeliveryDate, MaturityDays, OverdueDays, PayableAmount, CollectedAmount FROM  Sales WHERE (InvNo = @InvNo)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Connection.Open();
                cmd.Parameters.Add("@InvNo", SqlDbType.VarChar).Value = InvNo;

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    DateTime DeliveryDate = Convert.ToDateTime(dr[0].ToString());
                    DateTime MaturityDate = DeliveryDate.AddDays(Convert.ToInt32(dr[1]));
                    DateTime OverdueDate = MaturityDate.AddDays(Convert.ToInt32(dr[2]));
                    decimal balance = Convert.ToDecimal(dr[3].ToString()) - Convert.ToDecimal(dr[4].ToString());

                    if (OverdueDate < DateTime.Now)
                    {
                        od += balance;
                    }
                    else if (MaturityDate < DateTime.Now)
                    {
                        md += balance;
                    }
                    else
                    {
                        id += balance;
                    }
                }
                cmd.Connection.Close();

            }
        }

        //lblImitured.Text = id.ToString();
        lblMatured.Text = md.ToString();
        //lblOverdue.Text = od.ToString();
        //lblPendingChq.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(ChqAmt),0) FROM Cheque where PartyID='" + ddCustomer.SelectedValue + "' AND ([ChqStatus] <>'Cancelled') AND ([ChqStatus] <>'Passed')");
        //lblCurrBalance.Text = RunQuery.SQLQuery.ReturnString("Select SUM(Dr)-Sum(Cr) FROM Transactions where HeadID='" + ddCustomer.SelectedValue + "' AND ([TrType]='Customer')");

        LoadOrderDetails();

    }

    protected void chkInvoices_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CalculateAmount();
    }

    private void CalculateAmount()
    {
        string inv = string.Empty; string s = string.Empty;
        decimal deliverAmt = 0;decimal collAmt = 0;decimal invAmt = 0;

        for (int i = 0; i < chkInvoices.Items.Count; i++)
        {
            if (chkInvoices.Items[i].Selected)
            {
                inv = chkInvoices.Items[i].Value;
                invAmt = Convert.ToDecimal(SQLQuery.ReturnString("Select PayableAmount from Sales where InvNo='" + inv + "'"));
                collAmt += Convert.ToDecimal(SQLQuery.ReturnString("Select CollectedAmount from Sales where InvNo='" + inv + "'"));
                //dueAmt += Convert.ToDecimal(SQLQuery.ReturnString("Select DueAmount from Sales where InvNo='" + inv + "'"));

                deliverAmt = deliverAmt+ invAmt;
                s += inv + " -- Amount: " + invAmt + "<br>";
            }
        }

        lblChecked.Text = s.TrimEnd(';');

        decimal adj = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(AdjustmentAmt),0) AS totalAmt from Collection WHERE SalesInvoiceNo= '" + lvOrders.SelectedValue + "' AND CollType='LLC'"));

        //txtDeliverd.Text = deliverAmt.ToString(); 
        //txtPrePaid.Text = collAmt.ToString();
        txtDue.Text = Convert.ToString(Convert.ToDecimal(txtDeliverd.Text) - Convert.ToDecimal(txtPrePaid.Text) + adj);
        lblAdjustAmt.Text = adj.ToString();
    }
}
