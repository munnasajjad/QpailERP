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

public partial class app_Cheque : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            lblDate2.Text = DateTime.Today.AddDays(-1).ToShortDateString();
            
            ddType.DataBind();
            ddChqNo.DataBind();
            LoadChqInfo();
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (radApprove.Checked == true)
            {
                if (txtDate.Text != "" && ddBank.SelectedValue != "")
                {
                    if (ddType.SelectedValue == "In Hand")
                    {
                        ChqTransferToBank();
                    }
                    else
                    {
                        ApproveDeposit();
                    }
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Cheque info updated Successfully.";
                }
                else
                {
                    lblMsg.Text = "Please Select Deposite Bank & Sumission date Properly";
                    lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#6e000a");
                }
            }
            else
            {
                if (ddType.SelectedValue == "In Hand")
                {
                    CancelPayment();
                    lblMsg.Text = "Cheque has been cancelled successfully!";
                }
                else
                {
                    ChqReturn();
                    lblMsg.Text = "Cheque returned to In-Hand Status!";
                }

                lblMsg.Attributes.Add("class", "xerp_warning");
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg); 
        }
        finally
        {
            GridView1.DataBind();
            ddChqNo.DataBind();
        }
    }

    private void ChqTransferToBank()
    {
        string lName=Page.User.Identity.Name.ToString();
        string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        RunQuery.SQLQuery.ExecNonQry("UPDATE Cheque set ChqStatus='In Bank', DepositAccountID='" + ddBank.SelectedValue + "', DepositDate='" + dt + "', ApproveBy='" + lName + "', Remark='" + txtRemark.Text + "' where ChequeID='" + ddChqNo.SelectedValue + "'");
    }

    private void ApproveDeposit()
    {
        string lName = Page.User.Identity.Name.ToString();
        string detail = "Collection: Cheque# " + ddChqNo.SelectedItem.Text;
        string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        string partyId = RunQuery.SQLQuery.ReturnString("SELECT PartyID FROM Cheque  where ChequeID='" + ddChqNo.SelectedValue + "' ");

        RunQuery.SQLQuery.ExecNonQry("UPDATE Cheque set ChqStatus='Passed', DepositAccountID='" + ddBank.SelectedValue + "', ApproveDate='" + dt + "', ApproveBy='" + lName + "', Remark='" + txtRemark.Text + "' where ChequeID='" + ddChqNo.SelectedValue + "'");

        //Update party balance
        Accounting.VoucherEntry.TransactionEntry(txtID.Text, dt, partyId, txtCustomer.Text, detail, "0", txtChqAmount.Text, "0", "Collection", "Customer", "010105005", lName, "1");
        Accounting.VoucherEntry.TransactionEntry(txtID.Text, dt, ddBank.SelectedValue, ddBank.SelectedItem.Text, detail, txtChqAmount.Text, "0", "0", "Deposit", "Bank", "010102002", lName, "1");
        //VoucherEntry.AutoVoucherEntry("5", "010101002", "010104001", Convert.ToDecimal(txtChqAmount.Text), txtID.Text, lName, dt, "1");


        SqlCommand cmdxx = new SqlCommand("UPDATE Collection SET IsApproved='A' where CollectionNo='" + txtID.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdxx.Connection.Open();
        cmdxx.ExecuteNonQuery();
        cmdxx.Connection.Close();
        cmdxx.Connection.Dispose();

        //InActivate Orders
        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT InvNo, TDSRate, TDSAmount, VDSrate, VDSAmount, CollectedAmount, ReceivableAmount-CollectedAmount AS remainBalance FROM CollectionInvoices WHERE (CollectionNo = '" + txtID.Text + "')");

        foreach (DataRow drx in dtx.Rows)
        {
            string invoiceNo = drx["InvNo"].ToString();
            string TDSRate = drx["TDSRate"].ToString();
            string tdsAmt = drx["TDSAmount"].ToString();
            string VDSrate = drx["VDSrate"].ToString();
            string VDSAmount = drx["VDSAmount"].ToString();
            string CollectedAmount = drx["CollectedAmount"].ToString();
            decimal remainBalance = Convert.ToDecimal(drx["remainBalance"].ToString());

            detail = "Collection: Coll. ID: " + txtID.Text;
           
            int isActive = 0;
            if (remainBalance < 1)
            {
                isActive = 1;
            }

            int CycleDays = SQLQuery.ReturnInvCycleDays(invoiceNo);
            SQLQuery.ExecNonQry("UPDATE CollectionInvoices set IsApproved='" + isActive + "', OverdueDays='" + CycleDays + "', PaymentDate='" + dt + "' where InvNo='" + invoiceNo + "'");

            //decimal remainBalance = Convert.ToDecimal(SQLQuery.ReturnString("SELECT PayableAmount-CollectedAmount FROM Sales where InvNo='" + InvID + "'"));
            //decimal sumColAmt = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(CollectedAmount),0) FROM CollectionInvoices where InvNo='" + InvID + "'"));

            SQLQuery.ExecNonQry("UPDATE Sales SET IsActive=" + isActive + ", TDSRate=" + TDSRate + ", TDSAmount=" + tdsAmt + ", VDSrate=" + VDSrate + ", VDSAmount=" + VDSAmount + ", CollectedAmount=(CollectedAmount+" + CollectedAmount + "), DueAmount=" + remainBalance + "  where InvNo='" + invoiceNo + "'");

            string customerId = SQLQuery.ReturnString("Select CustomerID FROM Collection WHERE CollectionNo='" + txtID.Text + "'");
            string customerName = SQLQuery.ReturnString("Select CustomerName FROM Collection WHERE CollectionNo='" + txtID.Text + "'");
            //Accounting.VoucherEntry.TransactionEntry(invoiceNo, txtDate.Text, customerId, customerName, "Cash Collection", "0", CollectedAmount, "0", "Collection", "Customer", "1122334455", lName, "1");

        }



        //decimal payingAmt = Convert.ToDecimal(txtChqAmount.Text);

        //string[] allOrders = txtID.Text.Split(',');
        //foreach (string InvID in allOrders)
        //{
            //if (payingAmt > 0)
            //{
            //    if (remainBalance == payingAmt)
            //    {
            //        RunQuery.SQLQuery.ExecNonQry("UPDATE Sales SET IsActive=0, CollectedAmount=" + remainBalance + ", DueAmount=(DueAmount-" + remainBalance + ")  where InvNo='" + InvID + "'");
            //        payingAmt = 0;
            //    }
            //    else if (remainBalance > payingAmt)
            //    {
            //        RunQuery.SQLQuery.ExecNonQry("UPDATE Sales SET IsActive=1, CollectedAmount=" + payingAmt + ", DueAmount=(DueAmount-" + remainBalance + ")  where InvNo='" + InvID + "'");
            //        payingAmt = 0;
            //    }
            //    else
            //    {
            //        RunQuery.SQLQuery.ExecNonQry("UPDATE Sales SET IsActive=0, CollectedAmount=" + remainBalance + ", DueAmount=(DueAmount-" + remainBalance + ")  where InvNo='" + InvID + "'");
            //        payingAmt = payingAmt - remainBalance;
            //    }
            //}
        //}
    }

    private void ChqReturn()
    {
        string lName = Page.User.Identity.Name.ToString();
        string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");

        RunQuery.SQLQuery.ExecNonQry("UPDATE Cheque set ChqStatus='In Hand', DepositAccountID='" + ddBank.SelectedValue + "', DepositDate='" + dt + "', ApproveBy='" + lName + "', Remark='Returned Cheque: " + txtRemark.Text + "' where ChequeID='" + ddChqNo.SelectedValue + "' ");
    }
    private void CancelPayment()
    {
        string lName = Page.User.Identity.Name.ToString();
        string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");

        SqlCommand cmdxx = new SqlCommand("UPDATE Collection SET IsApproved='C' where CollectionNo='" + txtID.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdxx.Connection.Open();
        cmdxx.ExecuteNonQuery();
        cmdxx.Connection.Close();
        cmdxx.Connection.Dispose();

        //SQLQuery.ExecNonQry("UPDATE Sales SET IsActive=1, CollectedAmount=0, DueAmount=" + remainBalance + "  where InvNo='" + invoiceNo + "'");
        RunQuery.SQLQuery.ExecNonQry("UPDATE Cheque set ChqStatus='Cancelled', DepositAccountID='" + ddBank.SelectedValue + "', DepositDate='" + dt + "', ApproveBy='" + lName + "', Remark='" + txtRemark.Text + "' where ChequeID='" + ddChqNo.SelectedValue + "'");
        SQLQuery.ExecNonQry("DELETE CollectionInvoices where CollectionNo='" + txtID.Text + "'");
    }


    protected void ddType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddChqNo.DataBind();
        LoadChqInfo();

    }
    protected void ddChqNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadChqInfo();
            txtDate.Focus();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg); 
        }
    }

    private void LoadChqInfo()
    {
        //ClearControls(Form);
        txtDate.Text = DateTime.Today.AddDays(0).ToShortDateString();
        if (ddType.SelectedValue == "In Hand")
        {
            radApprove.Text = "Submitted";
            radCancel.Text = "Returned (Cancel)";
            ltrRightTitle.Text = "Cheque in Hand";
            lblDateText.Text = "Cheque Submit Date:";
        }
        else
        {
            radApprove.Text = "Passed";
            radCancel.Text = "Bounced";
            ltrRightTitle.Text = "Cheque in Bank";
            lblDateText.Text = "Cheque Pass Date:";
        }

        SqlCommand cmd = new SqlCommand("SELECT TrID, ChqBank, ChqBankBranch, ChqDate, ChqAmt, PartyID, ChequeName, Remark, DepositAccountID FROM  Cheque WHERE (ChequeID = @ChequeID)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.Parameters.Add("@ChequeID", SqlDbType.VarChar).Value = ddChqNo.SelectedValue;

        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            txtID.Text = dr[0].ToString();
            string bankID = dr[1].ToString();

            txtChqBank.Text = RunQuery.SQLQuery.ReturnString("SELECT BankName FROM Banks  where BankId='" + bankID + "'");
            txtBranch.Text = dr[2].ToString();
            txtChqDate.Text = Convert.ToDateTime(dr[3].ToString()).ToShortDateString();
            txtChqAmount.Text = dr[4].ToString();
            txtCustomer.Text = dr[6].ToString();
            txtRemark.Text = dr[7].ToString();

            if (ddType.SelectedValue != "In Hand")
            {
                ddBank.SelectedValue = dr[8].ToString();
            }
        }
        cmd.Connection.Close();
        cmd.Connection.Dispose();
    }

}
