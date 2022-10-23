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

public partial class app_Cheque_Payment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            lblDate2.Text = DateTime.Today.AddDays(-1).ToShortDateString();

            ddChqNo.DataBind();
            LoadChqInfo();

            Title = "Payment Cheque Processing - " + Title;
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (radApprove.Checked)
            {
                if (txtDate.Text != "" && ddBank.SelectedValue != "")
                {
                    if (ddType.SelectedValue == "In Hand")
                    {
                        ChqTransferToBank();
                        UpdateChqInfo();
                        ApproveDeposit();
                    }
                    else
                    {
                        ApproveDeposit();
                    }

                    Notify("Cheque info updated Successfully.", "success", lblMsg);
                }
                else
                {
                    Notify("Please Select Deposite Bank & Submission date Properly", "warn", lblMsg);
                    //lblMsg.Text = "Please Select Deposite Bank & Submission date Properly";
                    //lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#6e000a");
                }
            }
            else if (rbUpdate.Checked)
            {
                if (ddBank.SelectedValue != "")
                {
                    UpdateChqInfo();
                    ddChqNo.DataBind();
                    LoadChqInfo();

                        Notify("Cheque info updated successfully.", "success", lblMsg);
                }
                else
                {
                    Notify("Please Select Deposite Bank & Submission date Properly", "warn", lblMsg);
                    //lblMsg.Text = "Please Select Deposite Bank & Submission date Properly";
                    //lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#6e000a");
                }
            }
            else
            {
                if (ddType.SelectedValue == "In Hand")
                {
                    UpdateChqInfo();
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

    private void Notify(string pleaseSelectDepositeBankSubmissionDateProperly, Label type)
    {
        throw new NotImplementedException();
    }

    private void ChqTransferToBank()
    {
        string lName = Page.User.Identity.Name.ToString();
        string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        RunQuery.SQLQuery.ExecNonQry("UPDATE Cheque set ChqStatus='In Bank', DepositAccountID='" + ddBank.SelectedValue + "', DepositDate='" + dt + "', ApproveBy='" + lName + "', Remark='" + txtRemark.Text + "' where ChequeID='" + ddChqNo.SelectedValue + "'");
    }

    private void UpdateChqInfo()
    {
        RunQuery.SQLQuery.ExecNonQry("UPDATE Cheque set ChequeNo='" + txtChqBank.Text + "', BankAccNo='" + ddBank.SelectedValue + "', ChqDate='" + Convert.ToDateTime(txtChqDate.Text).ToString("yyyy-MM-dd") + "', ChqAmt='"+txtChqAmount.Text+"', ChequeName='" + txtCustomer.Text + "', Remark='" + txtRemark.Text + "' where ChequeID='" + ddChqNo.SelectedValue + "'");
    }
    private void ApproveDeposit()
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            string detail = "Payment: Cheque# " + ddChqNo.SelectedItem.Text;
            string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
            RunQuery.SQLQuery.ExecNonQry("UPDATE Cheque set ChqStatus='Passed', DepositAccountID='" + ddBank.SelectedValue + "', ApproveDate='" + dt + "', ApproveBy='" + lName + "', Remark='" + txtRemark.Text + "' where ChequeID='" + ddChqNo.SelectedValue + "'");

            string partyID = RunQuery.SQLQuery.ReturnString("SELECT PartyID FROM Cheque  where ChequeID='" + ddChqNo.SelectedValue + "' ");

            //Update party balance
            Accounting.VoucherEntry.TransactionEntry(txtID.Text, dt, partyID, txtCustomer.Text, detail, txtChqAmount.Text, "0", "0", "Payment", "Supplier", "010105005", lName, "1");
            Accounting.VoucherEntry.TransactionEntry(txtID.Text, dt, ddBank.SelectedValue, txtCustomer.Text, detail, "0", txtChqAmount.Text, "0", "Withdraw", "Bank", "010102002", lName, "1");
            //Cancel old voucher for edit
            VoucherEntry.AutoVoucherEntry("7", "", "", "", 0, detail, "", lName, dt, "0");
            /*Accounts Link disabled
            string voucherNo = "Auto-" + DateTime.Now.Year.ToString() + "-" + RunQuery.SQLQuery.ReturnString("Select ISNULL(MAX(VID),0)+1001 From VoucherMaster");
            VoucherEntry.InsertVoucherMaster(voucherNo, detail, "9", "020102006", "010101002", Convert.ToDecimal(txtChqAmount.Text), Convert.ToDecimal(txtChqAmount.Text), lName, dt, detail);
            */
            SqlCommand cmdxx = new SqlCommand("UPDATE Payment SET IsApproved='A' where PaymentNo='" + txtID.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmdxx.Connection.Open();
            cmdxx.ExecuteNonQuery();
            cmdxx.Connection.Close();
            cmdxx.Connection.Dispose();
/*
            //InActivate Orders
            decimal payingAmt = Convert.ToDecimal(txtChqAmount.Text);

            string[] allOrders = txtID.Text.Split(',');
            foreach (string InvID in allOrders)
            {
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
            }*/
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
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

        SqlCommand cmdxx = new SqlCommand("UPDATE Payment SET IsActive='C' where PaymentNo='" + txtID.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdxx.Connection.Open();
        cmdxx.ExecuteNonQuery();
        cmdxx.Connection.Close();
        cmdxx.Connection.Dispose();

        RunQuery.SQLQuery.ExecNonQry("UPDATE Cheque set ChqStatus='Cancelled', DepositAccountID='" + ddBank.SelectedValue + "', DepositDate='" + dt + "', ApproveBy='" + lName + "', Remark='" + txtRemark.Text + "' where ChequeID='" + ddChqNo.SelectedValue + "'");
        RunQuery.SQLQuery.ExecNonQry("UPDATE Purchase set IsApproved='P' where PaymentID='" + txtID.Text + "'");

    }


    protected void ddType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddChqNo.DataBind();
        LoadChqInfo();

    }
    protected void ddChqNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadChqInfo();
        //txtDate.Focus();
    }

    private void LoadChqInfo()
    {
        try
        {
            //ClearControls(Form);
            //txtDate.Text = DateTime.Today.AddDays(0).ToShortDateString();
            if (ddType.SelectedValue == "In Hand")
            {
                radApprove.Text = "Passed";
                radCancel.Text = "Returned";
                ltrRightTitle.Text = "Pending Cheque to Process";
                lblDateText.Text = "Cheque Passing Date:";
            }
            else
            {
                radApprove.Text = "Passed";
                radCancel.Text = "Bounced";
                ltrRightTitle.Text = "Cheque in Bank";
                lblDateText.Text = "Cheque Pass Date:";
            }

            SqlCommand cmd = new SqlCommand("SELECT TrID, ChequeNo, ChqBankBranch, ChqDate, ChqAmt, PartyID, ChequeName, Remark, BankAccNo FROM  Cheque WHERE (ChequeID = @ChequeID)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            cmd.Parameters.Add("@ChequeID", SqlDbType.VarChar).Value = ddChqNo.SelectedValue;

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                txtID.Text = dr[0].ToString();
                txtChqBank.Text = dr[1].ToString();

                txtBranch.Text = dr[2].ToString();
                txtChqDate.Text = Convert.ToDateTime(dr[3].ToString()).ToShortDateString();
                txtChqAmount.Text = dr[4].ToString();
                txtCustomer.Text = dr[6].ToString();
                txtRemark.Text = dr[7].ToString();

                string bank = dr[8].ToString();
                if (bank!="")
                {
                    ddBank.SelectedValue = bank;
                }

            }
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    protected void radCancel_OnCheckedChanged(object sender, EventArgs e)
    {
        lblDateText.Text = "Cheque Return Date:";
    }

    protected void radApprove_OnCheckedChanged(object sender, EventArgs e)
    {
        lblDateText.Text = "Cheque Pass Date:";
    }
}
