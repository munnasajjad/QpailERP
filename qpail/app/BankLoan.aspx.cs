using RunQuery;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml;

public partial class app_BankLoan : System.Web.UI.Page
{
    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ddLoanType.DataBind();
                ddAcheadId.DataBind();
                txtRevAmount.Text = "0";
                txtInterest.Text = "0";
                txtDuration.Text = "0";
                txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
               // MyClass();
               //getLoanBalance(ddAcheadId.SelectedValue);
                GridView2.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.ToString();
        }
    }
    //private void MyClass()
    //{
    //    try
    //    {
    //        string isExist = SQLQuery.ReturnString("Select id from BankLoan where ACHeadId ='" + ddAcheadId.SelectedValue + "'");
    //        if (isExist == "")
    //        {

    //            //ddLoanType.SelectedValue = "";
    //            txtDate.Text = "";
    //            txtCode.Text = "";
    //            txtInterest.Text = "";
    //            txtDuration.Text = "";
    //            btnSave.Text = "Save";
    //        }
    //        else // Load Edit Mode
    //        {
    //            DataTable dt = SQLQuery.ReturnDataTable("SELECT ACHeadId,LoanType, code, ReceivedDate, InterestRate, Duration   FROM       BankLoan where ACHeadId ='" + ddAcheadId.SelectedValue + "'");
    //            foreach (DataRow dtx in dt.Rows)
    //            {

    //                ddLoanType.SelectedValue = dtx["LoanType"].ToString();
    //                txtDate.Text = Convert.ToDateTime(dtx["ReceivedDate"].ToString()).ToString("dd/MM/yyyy");
    //                txtCode.Text = dtx["code"].ToString();
    //                txtInterest.Text = dtx["InterestRate"].ToString();
    //                txtDuration.Text = dtx["Duration"].ToString();

    //            }

    //            btnSave.Text = "Update";
    //            lblMsg.Attributes.Add("class", "xerp_info");
    //            lblMsg.Text = "Edit mode activeted....";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg.Text = ex.ToString();
    //    }
    //    finally
    //    {

    //        //GridView2.DataBind();
    //        //ddLoanType.DataBind();

    //    }
    //}


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text == "Save")
            {
                SQLQuery.Empty2Zero(txtRevAmount);
                string lName = Page.User.Identity.Name.ToString();
                if (txtCode.Text != "" && Convert.ToDecimal(txtRevAmount.Text) > 0)
                {
                    //InsertLoanType();
                    SQLQuery.ExecNonQry(@"INSERT INTO [dbo].[BankLoan] ([ACHeadId],[LoanType],[code],[ReceivedDate],[InterestRate],[Duration],[Rcvamount],[Note],[Status])
                                          VALUES ('" + ddAcheadId.SelectedValue + "','" + ddLoanType.SelectedValue + "','" + txtCode.Text + "','" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "','" + txtInterest.Text + "','" + txtDuration.Text + "','" + txtRevAmount.Text + "','" + txtNote.Text + "','"+ddStatus.SelectedValue+"')");



                    //Insert Loan VoucherEntry
                    string colId = SQLQuery.ReturnString("SELECT MAX(ID) FROM BankLoan");

                    string loanAccHeadId = SQLQuery.ReturnString(@"SELECT AccountsHeadID FROM LoanTypes WHERE AccountsHeadID='" + ddAcheadId.SelectedValue + "'");
                    string loanDescription = ddLoanType.SelectedItem.Text + " Loan Received from " + txtCode.Text;
                    Accounting.VoucherEntry.AutoVoucherEntry("15", loanDescription, "010101002", loanAccHeadId, Convert.ToDecimal(txtRevAmount.Text), colId, "", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "1");
                    //Insert Loan Tranction
                    Accounting.VoucherEntry.TransactionEntry(colId, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), ddBancAcc.SelectedValue, ddBancAcc.SelectedItem.Text, loanDescription, txtRevAmount.Text, "0", "0", "Deposit", "Bank", "010102002", lName, "1");
                    //Insert Loan Tranction for loan ledger
                    Accounting.VoucherEntry.TransactionEntry(colId, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), colId, ddAcheadId.SelectedItem.Text, loanDescription,  "0", txtRevAmount.Text, "0", "LoanReceived", "Loan", loanAccHeadId, lName, "1");
                }
                else
                {
                    lblMsg.Text = "Please fillup all mendatory fields...";

                }
            }
            else // UPDATE
            {
                //InsertLoanType();
                //bool status = cbStatus.Checked;
                
                SQLQuery.ExecNonQry(@"UPDATE [dbo].[BankLoan] SET [LoanType] = '" + ddLoanType.SelectedValue + "',[ReceivedDate] = '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "',[code] = '" + txtCode.Text + "',[InterestRate]='" + txtInterest.Text + "',[Duration]='" + txtDuration.Text + "',[Rcvamount]='" + txtRevAmount.Text + "',[Note]='" + txtNote.Text + "', [Status]='"+ cbStatus.Checked + "' WHERE Id ='" + LoanIdHField.Value + "'");
                //BindGrid(); 
                cbStatus.Checked = false;
                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Update successful!!!....";
                GridView2.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.ToString();

        }
        finally
        {
          //  MyClass();
            ClearControls(Form);
            GridView2.DataBind();
            //ddAcheadId.DataBind();
            //ddLoanType.DataBind();
            //txtTextBox7.Focus();

        }

    }

    private void InsertLoanType()
    {
        string loanType = "";
        
        if (lbLLoanType.Text == "Cancel")
        {
            loanType = ddLoanType.SelectedValue;
            if (txtLoanType.Text != "" && lbLLoanType.Text == "Cancel")//Insert Loan Type
            {
                string loanTypeIdExist = RunQuery.SQLQuery.ReturnString("SELECT LoanTypeId FROM LoanTypes WHERE LoanType='" + txtLoanType.Text + "'");
                if (loanTypeIdExist == "")
                {
                    RunQuery.SQLQuery.ExecNonQry(@"INSERT INTO LoanTypes (LoanType, EntryBy) 
                                                   VALUES ('" + txtLoanType.Text + "', '" + Page.User.Identity.Name + "') ");
                    loanType = RunQuery.SQLQuery.ReturnString("SELECT MAX(LoanTypeId) FROM LoanTypes");
                    ddLoanType.DataBind();
                    ddLoanType.SelectedValue = loanType;
                }
                else
                {
                    ddLoanType.DataBind();
                    ddLoanType.SelectedValue = loanTypeIdExist;
                }
            }
            loanType = ddLoanType.SelectedValue;
        }
    }

    //private void BindGrid()
    //{
    //    GridView1.DataSource = SQLQuery.ReturnDataTable(@"SELECT ACHeadId, LoanType, code, ReceivedDate, InterestRate, Duration FROM BankLoan");
    //    GridView1.DataBind();
    //}



    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox) { (Parent as TextBox).Text = string.Empty; } else { foreach (Control c in Parent.Controls) ClearControls(c); }
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        Label lblId = GridView2.Rows[index].FindControl("Label1") as Label;

        SQLQuery.ExecNonQry("DELETE FROM BankLoan WHERE (id ='" + lblId.Text + "')");
        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Shareholders Equity data Circle deleted successfully";


    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView2.SelectedIndex);
            Label label2 = GridView2.Rows[index].FindControl("Label1") as Label;
            if (label2 != null)
            {
                LoanIdHField.Value = label2.Text;
                DataTable dt = SQLQuery.ReturnDataTable("SELECT ACHeadId, LoanType, code, ReceivedDate, InterestRate, Duration,Rcvamount,Note,Status FROM BankLoan where Id='" + label2.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    ddLoanType.SelectedValue = dtx["LoanType"].ToString();
                    ddAcheadId.DataBind();
                    ddAcheadId.SelectedValue = dtx["ACHeadId"].ToString();
                    txtDate.Text = Convert.ToDateTime(dtx["ReceivedDate"].ToString()).ToString("dd/MM/yyyy");
                    txtCode.Text = dtx["code"].ToString();
                    txtRevAmount.Text = dtx["Rcvamount"].ToString();
                    txtNote.Text = dtx["note"].ToString();
                    txtInterest.Text = dtx["InterestRate"].ToString();
                    txtDuration.Text = dtx["Duration"].ToString();
                    string chkStatus = SQLQuery.ReturnString("SELECT Status FROM BankLoan where Id='"+label2.Text+"'");
                    if (chkStatus != "True")
                    {
                        cbStatus.Checked = false;
                    }
                    else
                    {
                        cbStatus.Checked = true;
                    }
                    // cbStatus.DataBind();
                    // chkPermissions();
                }
            }
            btnSave.Text = "Update";
            lblMsg.Attributes.Add("class", "xerp_info");
            lblMsg.Text = "Edit mode activeted....";
            GridView2.DataBind();
           // chkPermissions();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

   
    private void ClearForm()
    {
        ddLoanType.Text = "";
        txtDate.Text = "";
        txtCode.Text = "";

        txtInterest.Text = "0.00";
        //txtDuration.Text = "";
        GridView2.DataBind();
        //upnl.Update();

    }
    private string getLoanBalance(string headId)
    {

        //decimal opBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT   isnull(sum(OpBalDr),0)-isnull(sum(OpBalCr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID]  IN (Select AccountsHeadID from HeadSetup where ControlAccountsID= '" + ddParties + "'))"));
        string opBal = " (SELECT   isnull(sum(OpBalDr),0)-isnull(sum(OpBalCr),0)  FROM [HeadSetup] WHERE [AccountsHeadID]='" + headId + "') ";
        string query = opBal + " + ISNULL(SUM([VoucherDR]),0) - ISNULL(SUM([VoucherCR]),0)";

        string opQty = " (SELECT   isnull(sum(QtyDr),0)-isnull(sum(QtyCr),0)  FROM [HeadSetup] WHERE [AccountsHeadID]='" + headId + "') ";
        string query2 = opQty + " + ISNULL(SUM([InQty]),0) - ISNULL(SUM([OutQty]),0)";

        string isLiability = headId.Substring(0, 2);
        if (isLiability == "02")
        {
            opBal = " (SELECT   isnull(sum(OpBalCr),0)-isnull(sum(OpBalDr),0)  FROM [HeadSetup] WHERE [AccountsHeadID]='" + headId + "') ";
            query = opBal + " + ISNULL(SUM([VoucherCR]),0) - ISNULL(SUM([VoucherDR]),0)";

            opQty = " (SELECT   isnull(sum(QtyCr),0)-isnull(sum(QtyDr),0)  FROM [HeadSetup] WHERE [AccountsHeadID]='" + headId + "') ";
            query2 = opQty + " + ISNULL(SUM([OutQty]),0) - ISNULL(SUM([InQty]),0)";
        }

        string balance = SQLQuery.FormatBDNumber(SQLQuery.ReturnString(@"SELECT " + query + " As Balance FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" +
                headId + @"'))    AND ISApproved<>'C' "));
        txtBalance.Text = balance;
        return balance;
    }

    protected void ddAcheadId_SelectedIndexChanged(object sender, EventArgs e)
    {
        //MyClass();
        getLoanBalance(ddAcheadId.SelectedValue);
    }
    //protected void ddAcheadId_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    MyClass();
    //}

    protected void ddLoanType_SelectedIndexChanged(object sender, EventArgs e)
    {

        ddAcheadId.DataBind();
        //MyClass();
        getLoanBalance(ddAcheadId.SelectedValue);


    }
    //Latest update
    protected void ddSpec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //QtyinStock();
    }
    
    protected void lbLLoanType_OnClick(object sender, EventArgs e)
    {
        if (lbLLoanType.Text == "New")
        {
            ddLoanType.Visible = false;
            txtLoanType.Visible = true;
            lbLLoanType.Text = "Cancel";
            txtLoanType.Focus();
        }
        else
        {
            ddLoanType.Visible = true;
            txtLoanType.Visible = false;
            lbLLoanType.Text = "New";
            ddLoanType.DataBind();
            ddLoanType.Focus();
        }
        
    }

}