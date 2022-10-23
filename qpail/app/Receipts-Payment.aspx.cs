using RunQuery;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_Receipts_Payment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            string opdate = Convert.ToDateTime(SQLQuery.ReturnString("Select datefrom from ProcessDate")).ToString("yyyy-MM-dd");

            string cldate = Convert.ToDateTime(SQLQuery.ReturnString("Select dateto from ProcessDate")).ToString("yyyy-MM-dd");

            txtOpening.Text = Convert.ToDateTime(opdate).ToString("dd/MM/yyyy");
            txtClosing.Text = Convert.ToDateTime(cldate).ToString("dd/MM/yyyy");
            // txtOpening.Text = "01/" + DateTime.Now.AddMonths(-1).ToString("MM/yyyy");
            // txtClosing.Text = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01").AddDays(-1).ToString("dd/MM/yyyy");

            ShowRpt(opdate, cldate);

        }
    }
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    private void ShowRpt(string opdate, string cldate)
    {
        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormRece-Pay.aspx?&y1_OpDate=" + opdate + "&y1_ClDate=" + cldate;
        //string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormRece-Pay.aspx?&y1_OpDate=" + opdate + "&y1_ClDate=" + cldate + "&y2_OpDate=" + y2_OpDate + "&y2_ClDate=" + y2_ClDate + "&OpYear=" + ddYear1.SelectedValue + "&ClYear=" + ddYear2.SelectedValue;
        if1.Attributes.Add("src", urlx);
        if1.EnableViewState = true;
    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        try
        {
            //string y1_OpDate = SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year='" + ddYear1.SelectedValue + "'");
            //string y1_ClDate = SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year='" + ddYear1.SelectedValue + "'");
            //string y2_OpDate = SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year='" + ddYear2.SelectedValue + "'");
            //string y2_ClDate = SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year='" + ddYear2.SelectedValue + "'");

            //y1_OpDate = Convert.ToDateTime(y1_OpDate).ToString("yyyy-MM-dd");
            //y1_ClDate = Convert.ToDateTime(y1_ClDate).ToString("yyyy-MM-dd");
            //y2_OpDate = Convert.ToDateTime(y2_OpDate).ToString("yyyy-MM-dd");
            //y2_ClDate = Convert.ToDateTime(y2_ClDate).ToString("yyyy-MM-dd");

            string opdate = Convert.ToDateTime(SQLQuery.ReturnString("Select datefrom from ProcessDate")).ToString("yyyy-MM-dd");

            string cldate = Convert.ToDateTime(SQLQuery.ReturnString("Select dateto from ProcessDate")).ToString("yyyy-MM-dd");



            //if (btnShow2.Text != "Show Report")
            //{
            //    Notify("Process started ... ", "success", lblMsg);
            SQLQuery.ExecNonQry("Delete ProcessDate");
            opdate = Convert.ToDateTime(txtOpening.Text).ToString("yyyy-MM-dd");

            cldate = Convert.ToDateTime(txtClosing.Text).ToString("yyyy-MM-dd");

            SQLQuery.ExecNonQry("INSERT INTO [dbo].[ProcessDate]([datefrom],[dateto]) VALUES  ('" + opdate + "','" + cldate + "')");
            Search(opdate, cldate);
            //    Notify("Saved Successfully", "success", lblMsg);
            //    lblMsg.Text = "Process completed Successfully";
            //}

            //string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormRece-Pay.aspx?&y1_OpDate=" + opdate + "&y1_ClDate=" + cldate;
            ////string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormRece-Pay.aspx?&y1_OpDate=" + opdate + "&y1_ClDate=" + cldate + "&y2_OpDate=" + y2_OpDate + "&y2_ClDate=" + y2_ClDate + "&OpYear=" + ddYear1.SelectedValue + "&ClYear=" + ddYear2.SelectedValue;
            //if1.Attributes.Add("src", urlx);
            //if1.EnableViewState = true;

            InsertVoucherTmp(opdate, cldate);
            ShowRpt(opdate, cldate);
        }

        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "success", lblMsg);
            lblMsg.Text = ex.ToString();
        }

    }


    private string Search(string dateFrom, string dateTo)
    {
        SQLQuery.ExecNonQry("Delete ReceiptPayment");
        string acGroupPrev = "", acSubPrev = "", acControlPrev = "", acHeadPrev = "";
        string acType = "", acSub = "", acControl = "", acHead = "";
        string acGroupTxt = "", acSubTxt = "", acControlTxt = "", acHeadTxt = "";
        decimal grandTotalDr = 0; decimal grandTotalCr = 0;

        Opclbalance("Opening balances:", "010101001", "Dr", Convert.ToDateTime(dateFrom).AddDays(-1).ToString("yyyy-MM-dd"));//Cash
                                                                                                                             //Opclbalance("Opening balances:", "010101002", "Dr", dateFrom, dateTo);//Bank
                                                                                                                             //RHeadBalance("", "010101002", "Dr", dateFrom, dateTo);
        BankBalance(Convert.ToDateTime(dateFrom).AddDays(-1).ToString("yyyy-MM-dd"), "Dr");
        //WithdrawFromBank(dateFrom, dateTo);
        ReceiptfromCustomer(dateFrom, dateTo);

        //SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
        // VALUES ('Interest Received','a) Bank Deposits','" + 0 + "','" + 0 + "')");

        //SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
        //  VALUES ('','b) Loans, Advances','" + 0 + "','" + 0 + "')");

        //RHeadBalance("Interest Received", "a) Bank Deposits", "Dr", dateFrom, dateTo);
        //RHeadBalance(" ", "b) Loans, Advances", "Dr", dateFrom, dateTo);

        string genQuery = "";
        DataTable dt = new DataTable();



        ////All Receipts
        dt = SQLQuery.ReturnDataTable(@"SELECT        ControlAccount.ControlAccountsName, HeadSetup.AccountsHeadID, HeadSetup.AccountsHeadName, ISNULL(SUM(VoucherTmp.Amount), 0) AS Amount
FROM            ControlAccount INNER JOIN
                         HeadSetup ON ControlAccount.ControlAccountsID = HeadSetup.ControlAccountsID INNER JOIN
                         VoucherTmp ON HeadSetup.AccountsHeadID = VoucherTmp.AccountsHeadCr INNER JOIN
                         VoucherMaster ON VoucherTmp.VoucherNo = VoucherMaster.VoucherNo
                WHERE  (ControlAccount.ReceiptShow = 'true') AND (VoucherTmp.ISApproved = 'A') AND    ((VoucherTmp.AccountsHeadDr = '010101001') OR
                                         (VoucherTmp.AccountsHeadDr = '010101002')) AND    ((VoucherTmp.AccountsHeadCr <> '010101001') AND
                                         (VoucherTmp.AccountsHeadCr <> '010101002'))  AND (VoucherMaster.VoucherDate >='" + dateFrom + "') AND (VoucherMaster.VoucherDate <='" + dateTo + @"')
                GROUP BY HeadSetup.AccountsHeadID, HeadSetup.AccountsHeadName, ControlAccount.ControlAccountsName");

        foreach (DataRow drow in dt.Rows)
        {

            string controlName = drow["ControlAccountsName"].ToString();
            if (controlName == "Trade Receivables")
            {
                //controlName = "Paid to Suppliers";
                //ReceiptfromCustomer(dateFrom, dateTo);//ignore to show the amount
            }
            else
            {
                genQuery += Repaymenttype(controlName, drow["AccountsHeadID"].ToString(), drow["AccountsHeadName"].ToString(), Convert.ToDecimal(drow["Amount"].ToString()), "Dr", dateFrom, dateTo);
            }



        }






        /*
        ////Short Term Loan
         dt = SQLQuery.ReturnDataTable(@"SELECT   HeadSetup.AccountsHeadID,      HeadSetup.AccountsHeadName, ISNULL(SUM(VoucherDetails.VoucherDR), 0) - ISNULL(SUM(VoucherDetails.VoucherCR), 0) AS Amount
FROM            HeadSetup INNER JOIN
                         VoucherDetails ON HeadSetup.AccountsHeadID = VoucherDetails.AccountsHeadID INNER JOIN
                         VoucherMaster ON VoucherDetails.VoucherNo = VoucherMaster.VoucherNo
WHERE        (VoucherDetails.ISApproved = 'A') AND ((HeadSetup.ControlAccountsID = '020101')OR(HeadSetup.ControlAccountsID = '020201')OR(HeadSetup.ControlAccountsID = '020202')) AND (VoucherMaster.VoucherDate >='" + dateFrom + "') AND (VoucherMaster.VoucherDate <='" + dateTo + @"')
        GROUP BY HeadSetup.AccountsHeadID, HeadSetup.AccountsHeadName");
        foreach (DataRow drow in dt.Rows)
        {
           genQuery+=  Repaymenttype("Loans and Borrowings", drow["AccountsHeadID"].ToString(), drow["AccountsHeadName"].ToString(), Convert.ToDecimal(drow["Amount"].ToString()), "Dr", dateFrom, dateTo);
        }


        //SQLQuery.ExecNonQry(genQuery);
        //genQuery = "";

        ////Misc. Receipts
        //dt = SQLQuery.ReturnDataTable("SELECT EntryID, GroupID, AccountsID, ControlAccountsID, AccountsHeadID, AccountsHeadName, OpBalDr, OpBalCr, QtyDr, QtyCr, OpPcsDr, OpPcsCr, Rate, AccountsOpeningBalance, ProjectID, OpDate, ServerDateTime,Emark, IsActive FROM HeadSetup WHERE(ControlAccountsID = '030201') AND OpDate>= '" + dateFrom + "' AND OpDate<= '" + dateTo + "' ");
        dt = SQLQuery.ReturnDataTable(@"SELECT    HeadSetup.AccountsHeadID,     HeadSetup.AccountsHeadName, ISNULL(SUM(VoucherDetails.VoucherDR), 0) - ISNULL(SUM(VoucherDetails.VoucherCR), 0) AS Amount
FROM            HeadSetup INNER JOIN
                         VoucherDetails ON HeadSetup.AccountsHeadID = VoucherDetails.AccountsHeadID INNER JOIN
                         VoucherMaster ON VoucherDetails.VoucherNo = VoucherMaster.VoucherNo
WHERE        (VoucherDetails.ISApproved = 'A') AND (HeadSetup.ControlAccountsID = '030201') AND (VoucherMaster.VoucherDate >='" + dateFrom + "') AND (VoucherMaster.VoucherDate <='" + dateTo + @"')
        GROUP BY HeadSetup.AccountsHeadID, HeadSetup.AccountsHeadName");
        foreach (DataRow drow in dt.Rows)
        {
            genQuery += Repaymenttype("Misc. Receipts", drow["AccountsHeadID"].ToString(), drow["AccountsHeadName"].ToString(), Convert.ToDecimal(drow["Amount"].ToString()), "Dr", dateFrom, dateTo);
        }
        */

        //SQLQuery.ExecNonQry(genQuery);
        //genQuery = "";

        //////Credit part


        ////All Payments
        dt = SQLQuery.ReturnDataTable(@"SELECT ControlAccount.ControlAccountsName, HeadSetup.AccountsHeadID, HeadSetup.AccountsHeadName, ISNULL(SUM(VoucherTmp.Amount), 0) AS Amount
FROM ControlAccount INNER JOIN
                         HeadSetup ON ControlAccount.ControlAccountsID = HeadSetup.ControlAccountsID INNER JOIN
                         VoucherTmp ON HeadSetup.AccountsHeadID = VoucherTmp.AccountsHeadDr INNER JOIN
                         VoucherMaster ON VoucherTmp.VoucherNo = VoucherMaster.VoucherNo
WHERE (ControlAccount.PaymentShow = 'true') AND    (VoucherTmp.ISApproved = 'A') AND    ((VoucherTmp.AccountsHeadCr = '010101001') OR
                         (VoucherTmp.AccountsHeadCr = '010101002')) AND    ((VoucherTmp.AccountsHeadDr <> '010101001') AND
                                         (VoucherTmp.AccountsHeadDr <> '010101002'))  AND (VoucherMaster.VoucherDate >='" + dateFrom + "') AND (VoucherMaster.VoucherDate <='" + dateTo + @"')
GROUP BY HeadSetup.AccountsHeadID, HeadSetup.AccountsHeadName, ControlAccount.ControlAccountsName");

        foreach (DataRow drow in dt.Rows)
        {
            if (drow["AccountsHeadName"].ToString().ToLower().Contains("salary")) //Separate bank and cash
            {
                //Cash paid amount
                string cashPaidAmt = SQLQuery.ReturnString(@"Select ISNULL(SUM(VoucherTmp.Amount), 0) from VoucherTmp INNER JOIN
                         VoucherMaster ON VoucherTmp.VoucherNo = VoucherMaster.VoucherNo WHERE    (VoucherTmp.ISApproved = 'A') AND (VoucherTmp.AccountsHeadDr = '" + drow["AccountsHeadID"].ToString() + "')  AND (VoucherTmp.AccountsHeadCr = '010101001')  AND (VoucherMaster.VoucherDate >='" + dateFrom + "') AND (VoucherMaster.VoucherDate <='" + dateTo + "') ");
                if (Convert.ToDecimal(cashPaidAmt) != 0)
                {
                    genQuery += Repaymenttype(drow["ControlAccountsName"].ToString(), drow["AccountsHeadID"].ToString(),
                        drow["AccountsHeadName"].ToString() + " (Paid from Cash)", Convert.ToDecimal(cashPaidAmt), "Cr",
                        dateFrom, dateTo);
                }

                //Bank paid amount
                string bankPaidAmt = SQLQuery.ReturnString(@"Select ISNULL(SUM(VoucherTmp.Amount), 0) from VoucherTmp INNER JOIN
                         VoucherMaster ON VoucherTmp.VoucherNo = VoucherMaster.VoucherNo WHERE    (VoucherTmp.ISApproved = 'A') AND (VoucherTmp.AccountsHeadDr = '" + drow["AccountsHeadID"].ToString() + "')  AND (VoucherTmp.AccountsHeadCr = '010101002')  AND (VoucherMaster.VoucherDate >='" + dateFrom + "') AND (VoucherMaster.VoucherDate <='" + dateTo + "') ");
                if (Convert.ToDecimal(bankPaidAmt) != 0)
                {
                    genQuery += Repaymenttype(drow["ControlAccountsName"].ToString(), drow["AccountsHeadID"].ToString(),
                        drow["AccountsHeadName"].ToString() + " (Paid from Bank)", Convert.ToDecimal(bankPaidAmt), "Cr",
                        dateFrom, dateTo);
                }
            }
            else
            {
                string controlName = drow["ControlAccountsName"].ToString();
                if (drow["ControlAccountsName"].ToString() == "Trade payable")
                {
                    controlName = "Paid to Suppliers";
                }
                genQuery += Repaymenttype(controlName, drow["AccountsHeadID"].ToString(), drow["AccountsHeadName"].ToString(), Convert.ToDecimal(drow["Amount"].ToString()), "Cr", dateFrom, dateTo);

            }
        }


        //SQLQuery.ExecNonQry(genQuery);

        Opclbalance("Closing Balances", "010101001", "Cr", dateTo);
        //Opclbalance("Closing Balances", "010101002", "Cr", dateFrom, dateTo);
        //PHeadBalance("", "010101002", "Cr", dateTo, clDate2);
        BankBalance(dateTo, "Cr");

        //Notify("Saved Successfully", "success", lblMsg);
        //lblMsg.Text = "Yearly Process completed Successfully";

        return "";
    }

    private void Opclbalance(string rType, string headId, string btype, string opDate)
    {
        decimal returnValue = 0;
        //if (headId == "010101001")
        //{
        //    headId = "010101001";
        //}
        if (btype == "Dr")
        {

            decimal op1Bal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(OpBalDr),0) - isnull(sum(OpBalCr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID] = '" + headId + "')"));
            decimal preBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(VoucherDR),0) - isnull(sum(VoucherCR),0) FROM [VoucherDetails] WHERE ([AccountsHeadID] = '" + headId + "') and   EntryDate <= '" + Convert.ToDateTime(opDate).ToString("yyyy-MM-dd") + "'  and ISApproved='A'"));
            decimal currBal = op1Bal + preBal;
            // if (opBal != 0 || y1Bal != 0 || y2Bal != 0)
            {
                string headName = SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup WHERE AccountsHeadID='" + headId + "'");
                SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
                                VALUES ('" + rType + "','" + headName.Replace("'", "''") + "','" + currBal + "','" + currBal + "')");
            }
        }
        else
        {
            decimal op1Bal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(OpBalDr),0) - isnull(sum(OpBalCr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID] = '" + headId + "')"));
            decimal preBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(VoucherDR),0) - isnull(sum(VoucherCR),0) FROM [VoucherDetails] WHERE ([AccountsHeadID] = '" + headId + "') and   EntryDate <= '" + Convert.ToDateTime(opDate).ToString("yyyy-MM-dd") + "'  and ISApproved='A'"));
            decimal currBal = op1Bal + preBal;
            // if (opBal != 0 || y1Bal != 0 || y2Bal != 0)
            {
                string headName = SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup WHERE AccountsHeadID='" + headId + "'");

                string isExist = SQLQuery.ReturnString("Select MIN(id) from ReceiptPayment WHERE PaymentsHead IS NULL");
                if (isExist != "")
                {
                    SQLQuery.ExecNonQry(@"UPDATE [ReceiptPayment] SET [PaymentsType]='" + rType + "',[PaymentsHead]='" + headName.Replace("'", "''") + "',[PYear1]='" + currBal + "',[PYear2]='" + currBal + "' WHERE id='" + isExist + "' ");
                }
                else
                {
                    SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([PaymentsType],[PaymentsHead],[PYear1],[PYear2]) 
                                VALUES ('" + rType + "','" + headName.Replace("'", "''") + "','" + currBal + "','" + currBal + "')");
                }
            }
        }

    }

    private void RHeadBalance(string rType, string headId, string btype, string dateFrom, string dateTo)
    {
        decimal returnValue = 0;
        //if (headId== "010101001")
        //{
        //    headId = "010101001";
        //}
        if (btype == "Dr")
        {
            decimal opBal = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0)-ISNULL(SUM(OpBalCr),0) FROM HeadSetup WHERE (AccountsHeadID = '" + headId + "') AND OpDate>= '" + dateFrom + "' AND OpDate<= '" + dateTo + "' "));
            decimal y1Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0)-ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A' AND VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));
            decimal y2Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0)-ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A' AND VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));
            if (opBal != 0 || y1Bal != 0 || y2Bal != 0)
            {

                string headName = SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup WHERE AccountsHeadID='" + headId + "'");
                SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
                                VALUES ('" + rType + "','" + headName.Replace("'", "''") + "','" + y1Bal + "','" + y2Bal + "')");
            }
        }
        else
        {
            decimal opBal = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0)-ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE (AccountsHeadID = '" + headId + "') AND OpDate>= '" + dateFrom + "' AND OpDate<= '" + dateTo + "' "));
            decimal y1Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0)-ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A'  and VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));
            decimal y2Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0)-ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A'  and VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));
            if (opBal != 0 || y1Bal != 0 || y2Bal != 0)
            {
                string headName = SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup WHERE AccountsHeadID='" + headId + "'");

                string isExist = SQLQuery.ReturnString("Select MIN(id) from ReceiptPayment WHERE PaymentsHead IS NULL");
                if (isExist != "")
                {
                    SQLQuery.ExecNonQry(@"UPDATE [ReceiptPayment] SET [PaymentsType]='" + rType + "',[PaymentsHead]='" + headName.Replace("'", "''") + "',[PYear1]='" + y1Bal + "',[PYear2]='" + y2Bal + "' WHERE id='" + isExist + "' ");
                }
                else
                {
                    SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([PaymentsType],[PaymentsHead],[PYear1],[PYear2]) 
                                VALUES ('" + rType + "','" + headName.Replace("'", "''") + "','" + y1Bal + "','" + y2Bal + "')");
                }
            }
        }

    }
    private string Repaymenttype(string rType, string headId, string headName, decimal amount, string btype, string dateFrom, string dateTo)
    {
        decimal returnValue = 0;
        string query = "";
        //if (headId== "010101001")
        //{
        //    headId = "010101001";
        //}
        if (btype == "Dr")
        {
            //decimal opBal = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0)-ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE (AccountsHeadID = '" + headId + "') AND OpDate>= '" + dateFrom + "' AND OpDate<= '" + dateTo + "' "));
            //decimal y1Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A'  and VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));
            //decimal y2Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A'  and VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));

            //string headName = SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup WHERE AccountsHeadID='" + headId + "'");
            query += (@" INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
                                VALUES ('" + rType + "','" + headName.Replace("'", "''") + "','" + amount + "','" + amount + "')");

        }
        else
        {
            //decimal opBal = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0)-ISNULL(SUM(OpBalCr),0) FROM HeadSetup WHERE (AccountsHeadID = '" + headId + "') AND OpDate>= '" + dateFrom + "' AND OpDate<= '" + dateTo + "' "));
            //decimal y1Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A' AND VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));
            //decimal y2Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A' AND VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));

            //string headName = SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup WHERE AccountsHeadID='" + headId + "'");

            string isExist = SQLQuery.ReturnString("Select MIN(id) from ReceiptPayment WHERE PaymentsHead IS NULL");
            if (isExist != "")
            {
                query += (@" UPDATE [ReceiptPayment] SET [PaymentsType]='" + rType + "',[PaymentsHead]='" + headName.Replace("'", "''") + "',[PYear1]='" + amount + "',[PYear2]='" + amount + "' WHERE id='" + isExist + "' ");
            }
            else
            {
                query += (@" INSERT INTO[dbo].[ReceiptPayment]([PaymentsType],[PaymentsHead],[PYear1],[PYear2]) 
                                VALUES ('" + rType + "','" + headName.Replace("'", "''") + "','" + amount + "','" + amount + "')");
            }

        }

        if (amount != 0)
        {

            SQLQuery.ExecNonQry(query);
        }

        return query;
    }

    private void BankBalance(string dateFrom, string type)
    {
        decimal returnValue = 0;
        //if (type == "Dr")
        //{
        //    type = "Dr";
        //}

        DataTable dt = SQLQuery.ReturnDataTable("SELECT ACID, TypeID, ACName, ACNo, BankID, ZoneID, Address, Email, ContactNo, OpBalance, ProjectID, EntryBy, EntryDate FROM BankAccounts WHERE ACNAME NOT LIKE '%LATR%' ");
        foreach (DataRow drow in dt.Rows)
        {
            string bid = drow["ACID"].ToString();
            string balop = drow["OpBalance"].ToString();
            string Bid = drow["BankId"].ToString();
            string acNo = drow["ACNo"].ToString();
            decimal opBal = Convert.ToDecimal(balop);

            // decimal currBal1 = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate >= '" + dateFrom + "') AND  (TrDate <= '" + dateTo + "')"));
            // decimal currBal2 = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate >= '" + dateFrom + "') AND  (TrDate <= '" + dateTo + "')"));
            decimal currBal1 = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate <= '" + dateFrom + "') "));
            decimal currBal2 = currBal1;// opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate <= '" + dateTo + "')"));

            if (currBal1 != 0)
            {
                string headName = SQLQuery.ReturnString("Select BankName from Banks WHERE BankId='" + Bid + "'") +
                                  "- " + acNo;
                if (type == "Dr")
                {
                    SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
                                VALUES ('Opening balances:','" + headName + "','" + currBal1 + "','" + currBal1 + "')");
                }
                else
                {
                    string isExist = SQLQuery.ReturnString("Select MIN(id) from ReceiptPayment WHERE PaymentsHead IS NULL");
                    if (isExist != "")
                    {
                        SQLQuery.ExecNonQry(@" UPDATE [ReceiptPayment] SET [PaymentsType]='Closing Balances',[PaymentsHead]='" + headName.Replace("'", "''") + "',[PYear1]='" + currBal2 + "',[PYear2]='" + currBal2 + "' WHERE id='" + isExist + "' ");
                    }
                    else
                    {
                        SQLQuery.ExecNonQry(
                            @"INSERT INTO[dbo].[ReceiptPayment]([PaymentsType],[PaymentsHead],[PYear1],[PYear2]) 
                                VALUES ('Closing Balances','" + headName + "','" + currBal2 + "','" + currBal2 +
                            "')");
                    }
                }
            }
        }
    }

    private void WithdrawFromBank(string dateFrom, string dateTo)
    {
        decimal returnValue = 0;
        DataTable dt = SQLQuery.ReturnDataTable("SELECT ACID, TypeID, ACName, ACNo, BankID, ZoneID, Address, Email, ContactNo, OpBalance, ProjectID, EntryBy, EntryDate FROM BankAccounts WHERE ACNAME NOT LIKE '%LATR%' ");
        foreach (DataRow drow in dt.Rows)
        {
            string bid = drow["ACID"].ToString();
            //string balop = drow["OpBalance"].ToString();
            string Bid = drow["BankId"].ToString();
            string acNo = drow["ACNo"].ToString();
            //decimal opBal = Convert.ToDecimal(balop);

            // decimal currBal1 = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate >= '" + dateFrom + "') AND  (TrDate <= '" + dateTo + "')"));
            // decimal currBal2 = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate >= '" + dateFrom + "') AND  (TrDate <= '" + dateTo + "')"));
            decimal currBal1 = Convert.ToDecimal(SQLQuery.ReturnString(@"SELECT        isnull(sum(VoucherTmp.Amount),0) 
FROM            VoucherTmp INNER JOIN
                         VoucherMaster ON VoucherTmp.VoucherNo = VoucherMaster.VoucherNo
WHERE        (VoucherTmp.AccountsHeadDr = '010101001')
             AND VoucherTmp.Head5Cr ='" + bid + "' AND  (VoucherMaster.VoucherDate >= '" + dateFrom + "') AND  (VoucherMaster.VoucherDate <= '" + dateTo + "')"));
            decimal currBal2 = currBal1;// opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate <= '" + dateTo + "')"));

            if (currBal1 != 0)
            {
                string headName = SQLQuery.ReturnString("Select BankName from Banks WHERE BankId='" + Bid + "'") +
                                  "- " + acNo;
                //if (type == "Dr")
                //{
                SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
                                VALUES ('Cash WithDrawn from Bank:','" + headName + "','" + currBal1 + "','" + currBal1 + "')");
                //}
                //else
                //{
                //    SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([PaymentsType],[PaymentsHead],[PYear1],[PYear2]) 
                //                VALUES ('Opening balances:','" + headName + "','" + currBal2 + "','" + currBal2 + "')");
                //}
            }
        }
    }

    private void ReceiptfromCustomer(string dateFrom, string dateTo)
    {
        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT        Party.PartyID, Party.Type, Party.Category, Party.Company, Party.ReferrenceItems, Party.Zone, Party.Referrer, Party.Address, Party.Phone, Party.Email, Party.Fax, Party.IM, Party.Website, Party.ContactPerson, Party.MobileNo, 
                         Party.MatuirityDays, Party.CreditLimit, Party.OpBalance, ISNULL(SUM(Transactions.Cr), 0) - ISNULL(SUM(Transactions.Dr), 0) AS Amount
                        FROM            Party INNER JOIN
                                                 Transactions ON Party.PartyID = Transactions.HeadID
                        WHERE        (Party.Type = 'customer') AND (Transactions.TrType = 'customer') AND (Transactions.TrGroup <> 'Sales') AND (Transactions.AccHeadID <> '0101144455') AND (Transactions.TrDate >= '" + dateFrom + @"') AND 
                                                 (Transactions.TrDate <= '" + dateTo + @"')
                        GROUP BY Party.PartyID, Party.Type, Party.Category, Party.Company, Party.ReferrenceItems, Party.Zone, Party.Referrer, Party.Address, Party.Phone, Party.Email, Party.Fax, Party.IM, Party.Website, Party.ContactPerson, Party.MobileNo, 
                                                 Party.MatuirityDays, Party.CreditLimit, Party.OpBalance
                        ORDER BY amount DESC");

        foreach (DataRow drow in dt.Rows)
        {
            string company = drow["Company"].ToString();
            string balop = drow["Amount"].ToString();
            decimal amt = Convert.ToDecimal(balop);
            /* 
                      decimal currBal1 = Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(cr),0)-isnull(sum(dr),0) FROM [Transactions] " +
                                                 " where TrType = 'customer' AND TrGroup='Collection' AND AccHeadID<>'0101144455' " + " AND HeadID ='" + pid + "' AND (TrDate >= '" + dateFrom + "') AND (TrDate <= '" + dateTo + "')"));
                      decimal currBal2 = currBal1;
                    opBal +
                                      Convert.ToDecimal(
                                          SQLQuery.ReturnString(
                                              "SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " +
                                              " where TrType = 'customer' AND TrGroup='Collection' AND AccHeadID<>'0101144455'  " + " AND HeadID ='" + pid + "' AND (TrDate >= '" + dateTo + "') AND (TrDate <= '" + dateTo + "')"));
                      */
            if (amt != 0)
            {
                SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
                                VALUES ('Receipt from Customer','" + company + "','" + amt + "','" + amt + "')");
            }
        }
    }

    private void InsertVoucherTmp(string dateFrom, string dateTo)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            string q = "Delete VoucherTmpRP ";

            DataTable dt = SQLQuery.ReturnDataTable(
                @"SELECT        VoucherDetails.SerialNo, VoucherDetails.VoucherNo, VoucherDetails.SLNO, VoucherDetails.VoucherRowDescription, VoucherDetails.AccountsHeadID, VoucherDetails.AccountsHeadName, VoucherDetails.VoucherDR, 
                         VoucherDetails.VoucherCR, VoucherDetails.Rate, VoucherDetails.InQty, VoucherDetails.OutQty, VoucherDetails.InPcs, VoucherDetails.OutPcs, VoucherDetails.DeliveredPcs, VoucherDetails.DeliveredStatus, 
                         VoucherDetails.projectName, VoucherDetails.EntryDate, VoucherDetails.SubprojectName, VoucherDetails.ISApproved, VoucherMaster.VoucherDate
FROM            VoucherDetails INNER JOIN
                         VoucherMaster ON VoucherDetails.VoucherNo = VoucherMaster.VoucherNo WHERE VoucherDetails.VoucherNo NOT IN (Select VoucherNo from VoucherTmp) AND VoucherDetails.VoucherDR>0 AND VoucherMaster.VoucherDate>='" + dateFrom + "' AND VoucherMaster.VoucherDate<='" + dateTo + "' ");

            foreach (DataRow dr in dt.Rows)
            {
                string crHeadId =
                    SQLQuery.ReturnString("Select TOP(1) AccountsHeadID from VoucherDetails WHERE VoucherNo='" + dr["VoucherNo"] +
                                          "' AND VoucherCR='" + dr["VoucherDR"] + "'  ");

                q += (@" INSERT INTO VoucherTmp (VoucherNo, Particular, VoucherRowDescription, AccountsHeadDr, AccountsHeadDrName, Head5Dr, Name5Dr, AccountsHeadCr, AccountsHeadCrName, Head5Cr, Name5Cr, Amount, projectName, EntryDate, EntryBy)
                                VALUES ('" + dr["VoucherNo"] + "', '" + 9 + "', '" + dr["VoucherRowDescription"] + "', '" + dr["AccountsHeadID"] + "', '" + dr["AccountsHeadName"] + "', '" + 0 + "', 'Head Name Dr', '" + crHeadId + "', '" + crHeadId + "', '" + 0 + "', 'Head Name Cr', '" + dr["VoucherDR"] + "', 'Re-Constructed', '" + Convert.ToDateTime(dr["VoucherDate"]).ToString("yyyy-MM-dd") + "', '" + lName + "')");


            }
            SQLQuery.ExecNonQry(q);
        }

        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "success", lblMsg);
            lblMsg.Text = ex.ToString();
        }

    }

    protected void RadioButton1_OnCheckedChanged(object sender, EventArgs e)
    {
        if (showButton.Checked)
        {
            txtOpening.Enabled = false;
            txtClosing.Enabled = false;
            btnShow2.Text = "Show Report";

        }
        else
        {
            txtOpening.Enabled = true;
            txtClosing.Enabled = true;
            btnShow2.Text = "Run Process";



        }

    }
}
