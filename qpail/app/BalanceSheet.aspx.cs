using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_BalanceSheet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                txtdateFrom.Text = "01/" + DateTime.Now.AddMonths(-1).ToString("MM/yyyy");
                txtdateTo.Text = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01").AddDays(-1).ToString("dd/MM/yyyy");

                //search();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message.ToString();
        }
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        string dt2 = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");
        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormBalanceSheet.aspx?dateTo=" + dt2;
        if1.Attributes.Add("src", urlx);
    }
    protected void btnSearch_OnClick1(object sender, EventArgs e)
    {
        //search();

        string dt2 = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");
        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormBalanceSheet2.aspx";
        if1.Attributes.Add("src", urlx);
    }

    private void search()
    {
        try
        {
            //string dateFrom = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
            //string dateTo = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");
            string body =
                "<table class='table'>" +
                "<tr><th>Particulars</th><th style='text-align: right;'>Amount(TK.)</th></tr></table>" +
                "<h4>Non - Current Assets</h4>"+
                "<table class='table'>" ;
            string amt1 = "", ControlAccountsID = "", ControlAccountsName = "", amt2 = "", amt3 = "", amt4 = "", amt5 = "", Maindecks = "", UpperLevelp = "", UpperLevels = "", Quarter = "", mgo = "", tank = "", FPK1 = "", FPK2 = "", FPK3 = "", APK1 = "", APK2 = "", APK3 = "", Chainlocker = "", DB1 = "", DB2 = "", DB3 = "", DB4 = "", ProjectID = "", FMean = "", AMean = "", FAMean = "", MidMean = "", MeanOfMean = "", QuarterMean = "", MainMean = "", UpperMean = "", Density = "", Qty = "";
            decimal sumAmt1 = 0, sumAmt2 = 0;

            //Non-Current Assets
            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0101' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='01') ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt1 = SumByControl(ControlAccountsID, "Dr");
                if (amt1 != "0.00")
                {
                    body += "<tr><td><b>" + ControlAccountsName + "</b></td><td>" + SQLQuery.FormatBDNumber(amt1) + "</td></tr>";
                    sumAmt1 += Convert.ToDecimal(amt1);
                }
            }
            body += "<tr><td><b>Total non current Assets</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt1.ToString()) + "</td></tr>";

            //Current Assets
            body += "</table><br/><h4>Current Assets</h4>" +
                      "<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0101' ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt2 = SumByControl(ControlAccountsID, "Dr");
                if (amt2 != "0.00")
                {
                    body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt2) + "</td></tr>";
                    sumAmt2 += Convert.ToDecimal(amt2);
                }
            }
            body += "<tr><td><b>Total Current Assets</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt2.ToString())+ "</td></tr>";

            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Total Assets</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt1 + sumAmt2).ToString()) + "</td></tr>";

            //Shareholders' Equity 
            decimal sumAmt3 = 0, sumAmt4 = 0, sumAmt5 = 0;
            body += "</table><br/><h4>Shareholders' Equity</h4>" +
                      "<table class='table'>";

            decimal transferred = Convert.ToDecimal(GetEquity("transferred"));
            if (transferred<0)
            {
                transferred = Convert.ToDecimal(GetEquity("npbp"));
            }
            int i = 0;

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID IN (Select AccountsID from Accounts Where GroupID='05') ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt3 = SumByControl(ControlAccountsID, "Cr");
                
                if (i==0)//Net profit after provision will sum with Retained Earnings (i for First Control A/C)
                {
                    amt3 = Convert.ToString(Convert.ToDecimal(amt3) + transferred);
                    i++;
                }

                if (amt3 != "0.00")
                {
                    body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt3) + "</td></tr>";
                    sumAmt3 += Convert.ToDecimal(amt3);
                }
            }

            decimal gr = Convert.ToDecimal(GetEquity("gr"));

            if (gr>0)
            {
                sumAmt3 += gr;
                body += "<tr><td>General reserve (10%)</td><td>" + SQLQuery.FormatBDNumber(gr) + "</td></tr>";
            }
            
            body += "</table><br/>" +
                    "<table class='table'>" +
                    //"<tr><td>Net profit after provision</td><td>" + SQLQuery.FormatBDNumber(transferred) + "</td></tr>" +
                    "<tr><td><strong>Total Shareholders' Equity</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt3).ToString()) + "</td></tr>";
                    


            //Non-Current Liabilities

            body += "</table><br/><h4>Non-Current Liabilities</h4>" +
                      "<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0201' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='02') ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt4 = SumByControl(ControlAccountsID, "Cr");
                if (amt4 != "0.00")
                {
                    body += "<tr><td><b>" + ControlAccountsName + "</b></td><td>" + SQLQuery.FormatBDNumber(amt4) + "</td></tr>";
                    sumAmt4 += Convert.ToDecimal(amt4);
                }
            }
            body += "<tr><td><b>Total Long Term Loan</b></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt4).ToString()) + "</td></tr>";

            //Current liabilities
            body += "</table><br/><h4>Current liabilities</h4>" +
                      "<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0201' ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt5 = SumByControl(ControlAccountsID, "Cr");
                if (amt5 != "0.00")
                {
                    body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt5) + "</td></tr>";
                    sumAmt5 += Convert.ToDecimal(amt5);
                }
            }

            decimal tax = Convert.ToDecimal(GetEquity("tax"));
            if (tax>0)
            {
                sumAmt5 += tax;
                body += "<tr><td><strong>Provision For Tax (35%)</strong></td><td>" + tax + "</td></tr>";

            }
            
            body += "<tr><td><b>Total current liabilities</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt5) + "</td></tr>";

            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Total Liabilities</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt4 + sumAmt5).ToString()) + "</td></tr>";


            //Total
            decimal e_l = (sumAmt3 + sumAmt4 + sumAmt5);
            decimal nav = (sumAmt1 + sumAmt2)-(sumAmt4 + sumAmt5);

            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Total Equity and Liabilities</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(e_l)) + "</td></tr>";


            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Net Asset Value (NAV) </strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(nav)) + "</td></tr>";


            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Net Asset Value (NAV) per Share</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(nav /60000M)) + "</td></tr>";

            
            ltrResult.Text = body;
            //ltrQR.Text = "From " + txtdateFrom.Text + " To " + txtdateTo.Text;
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
            //DataTable dt = null;
            //return dt;
        }
    }

    private string SumByControl(string cid, string btype)
    {
        string dateFrom = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");
        decimal returnValue = 0;

        if (btype == "Dr")
        {
            returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0) - ISNULL(SUM(OpBalCr),0) FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
            returnValue += Convert.ToDecimal(SQLQuery.ReturnString(
                    "SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
                    cid + "'))) and ISApproved ='A' and  EntryDate<='" + dateTo + "'"));
        }
        else
        {
            returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0) - ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
            returnValue += Convert.ToDecimal(SQLQuery.ReturnString(
                    "SELECT ISNULL(SUM(VoucherCR),0) - ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
                    cid + "'))) and ISApproved ='A' and  EntryDate<='" + dateTo + "'"));
        }
        return returnValue.ToString();
    }

    private decimal GetEquity(string rtype)
    {
        //string dateFrom = Convert.ToDateTime(txtdateTo.Text).AddMonths(-1).ToString("yyyy-MM") + "-01";
        //string dateTo = Convert.ToDateTime(Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM") + "-01").AddDays(-1).ToString("yyyy-MM-dd");

        string amt1 = "", ControlAccountsID = "", ControlAccountsName = "", amt2 = "", amt3 = "", amt4 = "", amt5 = "", Maindecks = "", UpperLevelp = "", UpperLevels = "", Quarter = "", mgo = "", tank = "", FPK1 = "", FPK2 = "", FPK3 = "", APK1 = "", APK2 = "", APK3 = "", Chainlocker = "", DB1 = "", DB2 = "", DB3 = "", DB4 = "", ProjectID = "", FMean = "", AMean = "", FAMean = "", MidMean = "", MeanOfMean = "", QuarterMean = "", MainMean = "", UpperMean = "", Density = "", Qty = "";
        decimal sumAmt1 = 0, sumAmt2 = 0;

        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0301' ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt1 = SumByControli(ControlAccountsID, "Cr");
            if (amt1 != "0.00")
            {
                sumAmt1 += Convert.ToDecimal(amt1);
            }
        }
        dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0402' ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt2 = SumByControli(ControlAccountsID, "Dr");
            if (amt2 != "0.00")
            {
                sumAmt2 += Convert.ToDecimal(amt2);
            }
        }

        //Gross profit
        decimal gp = 0;
        decimal gpRate = 0;
        if (sumAmt1 > 0)
        {
            gp = sumAmt1 - sumAmt2;
            gpRate = gp / sumAmt1 * 100M;
        }
        //Less : Operating Expenses:
        decimal sumAmt3 = 0, sumAmt4 = 0;

        dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0402' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='04') ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt3 = SumByControli(ControlAccountsID, "Dr");
            if (amt3 != "0.00")
            {
                sumAmt3 += Convert.ToDecimal(amt3);
            }
        }

        //Operating Profit
        decimal op = gp - sumAmt3;

        dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0301' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='03') ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt4 = SumByControli(ControlAccountsID, "Cr");
            if (amt4 != "0.00")
            {
                sumAmt4 += Convert.ToDecimal(amt4);
            }
        }

        //Net profit before provision
        decimal npbp = op + sumAmt4;
        decimal npRate = npbp / sumAmt1 * 100M;
        
        decimal tax = npbp * 35M / 100M;
        decimal gr = npbp / 10M; //10%= 10/100

        decimal transferred = npbp - tax - gr;
        decimal earning = (npbp - tax) / 60000M;

        decimal rValue = 0;
        if (rtype=="tax")//Provision For Tax (35%)
        {
            rValue = tax;
        }
        else if (rtype == "gr")//General reserve (10%)
        {
            rValue = gr;
        }
        else if (rtype == "npbp")//if loss happens
        {
            rValue = npbp;
        }
        else//Net profit after provision transferred to Balance sheet
        {
            rValue = transferred;
        }

        return Math.Round(rValue,2);
    }

    private string SumByControli(string cid, string btype)
    {
        //string dateFrom = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");
        decimal returnValue = 0;

        if (btype == "Dr")
        {
            returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0) - ISNULL(SUM(OpBalCr),0) FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
            returnValue += Convert.ToDecimal(SQLQuery.ReturnString(
                    "SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
                    cid + "'))) and ISApproved ='A' and  EntryDate<='" + dateTo + "'"));
        }
        else
        {
            returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0) - ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
            returnValue += Convert.ToDecimal(SQLQuery.ReturnString(
                    "SELECT ISNULL(SUM(VoucherCR),0) - ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
                    cid + "'))) and ISApproved ='A' and  EntryDate<='" + dateTo + "'"));
        }
        return returnValue.ToString();
    }

    protected void btnShow2_OnClick(object sender, EventArgs e)
    {
        string op1 = SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year='" + ddYear1.SelectedValue +"'");
        string cl1 = SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year='" + ddYear1.SelectedValue + "'");
        string op2 = SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year='" + ddYear2.SelectedValue + "'");
        string cl2 = SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year='" + ddYear2.SelectedValue + "'");

        op1 = Convert.ToDateTime(op1).ToString("yyyy-MM-dd");
        cl1 = Convert.ToDateTime(cl1).ToString("yyyy-MM-dd");
        op2 = Convert.ToDateTime(op2).ToString("yyyy-MM-dd");
        cl2 = Convert.ToDateTime(cl2).ToString("yyyy-MM-dd");
        string year1 = SQLQuery.ReturnString("Select Financial_Year_Number from tblFinancial_Year WHere Financial_Year='" + ddYear1.SelectedValue + "'");
        string year2 = SQLQuery.ReturnString("Select Financial_Year_Number from tblFinancial_Year WHere Financial_Year='" + ddYear2.SelectedValue + "'");
        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormBalancesheet2.aspx?y1=" + op1 + "&y2="+ cl1 + "&y3=" + op2 + "&y4=" + cl2+ "&y11="+ year1 + "&y22="+ year2;
        if1.Attributes.Add("src", urlx);
    }
}