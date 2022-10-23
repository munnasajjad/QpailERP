using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using RptRunQuery;
using RunQuery;

public partial class app_Finyearprocess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //txtdateFrom.Text = "01/" + DateTime.Now.AddMonths(-1).ToString("MM/yyyy");
            //txtdateTo.Text =
            //    Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01")
            //        .AddDays(-1)
            //        .ToString("dd/MM/yyyy");
            ////LoadDropdownInEdit();
         //   string lpldate = SQLQuery.ReturnString("SELECT LastProcessDate  FROM   tblFinancial_Year WHERE Financial_Year_Number ='" + ddYear1.SelectedValue + "'");
            
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

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ddYear1.SelectedValue != "")
            {
                SQLQuery.ReturnString(
                    "SELECT LastProcessDate  FROM   tblFinancial_Year WHERE Financial_Year_Number ='" +
                    ddYear1.SelectedValue + "'");



                SQLQuery.ExecNonQry("Update tblFinancial_Year SET LastProcessDate ='" +
                                    DateTime.Now.ToString("yyyy-MM-dd") +
                                    "' WHERE Financial_Year_Number ='" + ddYear1.SelectedValue + "'");

                string txtdateFrom =
                    SQLQuery.ReturnString(
                        "SELECT Opening_Date  FROM   tblFinancial_Year WHERE Financial_Year_Number ='" +
                        ddYear1.SelectedValue + "'");
                string txtdateTo =
                    SQLQuery.ReturnString(
                        "SELECT Closing_Date  FROM   tblFinancial_Year WHERE Financial_Year_Number ='" +
                        ddYear1.SelectedValue + "'");
                string dateFrom = Convert.ToDateTime(txtdateFrom).ToString("yyyy-MM-dd");
                string dateTo = Convert.ToDateTime(txtdateTo).ToString("yyyy-MM-dd");


                //Process Income Statement

                InsertPL_Data(dateFrom, dateTo);
                InsertBL_Data(dateFrom, dateTo);
                InsertBL_Data2(dateFrom, dateTo);
                Notify("Saved Successfully", "success", lblMsg);
                lblMsg.Text = "Yearly Process completed Successfully";

            }

            else
            {
                Notify("Faild", "error", lblMsg);
                lblMsg.Text = "Yearly Process not saved";
            }

        }
        catch (Exception ex)
        {

            Notify(ex.ToString(), "error", lblMsg);

        }
    }

    private  void InsertPL_IS(string group, string subGrp, string head, string headid, string balance, string ShowLine)
    {
        
        SQLQuery.ExecNonQry("Insert INTO Yearly_PL (AccGroup, SubGroup, AccHeadName,AccHeadID, BalanceDr, ShowLine,FinYear, Report) VALUES ('" + group + "','" + subGrp + "','" + head + "','" + headid + "','" + balance + "', '" + ShowLine + "', '" + ddYear1.SelectedValue + "', 'IS')");
    }
    private string InsertPL_Data(string dateFrom, string dateTo)  
    {
        //try
        //{
            string finyear = SQLQuery.ReturnString("SELECT FinYear FROM Yearly_PL WHERE FinYear='"+ddYear1.SelectedValue+"'");

            SQLQuery.ExecNonQry("DELETE FROM Yearly_PL WHERE(FinYear = '"+ finyear + "') AND (Report = 'IS')");
            //string body = "<table class='table'>" + "<tr><th>Particulars</th><th style='text-align: right;'>Amount(TK.)</th></tr>";

            string amt1 = "", ControlAccountsID = "", ControlAccountsName = "", amt2 = "", amt3 = "", amt4 = "", amt5 = "", Maindecks = "", UpperLevelp = "", UpperLevels = "", Quarter = "", mgo = "", tank = "", FPK1 = "", FPK2 = "", FPK3 = "", APK1 = "", APK2 = "", APK3 = "", Chainlocker = "", DB1 = "", DB2 = "", DB3 = "", DB4 = "", ProjectID = "", FMean = "", AMean = "", FAMean = "", MidMean = "", MeanOfMean = "", QuarterMean = "", MainMean = "", UpperMean = "", Density = "", Qty = "";
            decimal sumAmt1 = 0, sumAmt2 = 0;

            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0301' ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt1 = SumByControl(ControlAccountsID, "Cr", dateFrom, dateTo);
                if (amt1 != "0.00")
                {
                    //body += "<tr><td><b>" + ControlAccountsName + "</b></td><td>" + SQLQuery.FormatBDNumber(amt1) + "</td></tr>";
                    InsertPL_IS("Sales", ControlAccountsName, ControlAccountsName, ControlAccountsID, SQLQuery.FormatBDNumber(amt1), "0");

                    sumAmt1 += Convert.ToDecimal(amt1);
                }
            }

            //Less : Direct expenses
            //body += "</table><br/><h4>Less : Direct expenses</h4>" + "<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName,ControlAccountsID FROM [ControlAccount] WHERE AccountsID='0402' ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt2 = SumByControl(ControlAccountsID, "Dr", dateFrom, dateTo);
                if (amt2 != "0.00")
                {
                    //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt2) + "</td></tr>";
                    InsertPL_IS("Less : Direct expenses", ControlAccountsName, ControlAccountsName, ControlAccountsID, SQLQuery.FormatBDNumber(amt2), "1");
                    sumAmt2 += Convert.ToDecimal(amt2);
                }
            }

            //Gross profit
            decimal gp = 0;
            decimal gpRate = 0;
            gp = sumAmt1 - sumAmt2;

            if (sumAmt1 > 0)
            {
                gpRate = gp / sumAmt1 * 100M;
            }
            /*
            body += "</table><br/><h4> </h4>" +
                      "<table class='table'>" +
                      "<tr><td><b>Gross profit</b></td><td><b>" + SQLQuery.FormatBDNumber(gp) + "</b></td></tr>" +
                      "<tr><td>Gross profit rate</td><td>" + Math.Round(gpRate) + "%</td></tr>";*/
            InsertPL_IS(" ", ControlAccountsName, "Gross profit", ControlAccountsID, SQLQuery.FormatBDNumber(gp), "0");
            InsertPL_IS(" ", ControlAccountsName, "Gross profit rate", ControlAccountsID, Math.Round(gpRate, 2) + "%", "0");

            //Less : Operating Expenses:
            decimal sumAmt3 = 0, sumAmt4 = 0;
            //body += "</table><br/><h4>Less : Operating Expenses:</h4>" + "<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0402' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='04') ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt3 = SumByControl(ControlAccountsID, "Dr", dateFrom, dateTo);
                if (amt3 != "0.00")
                {
                    //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt3) + "</td></tr>";
                    InsertPL_IS("Less : Operating Expenses", ControlAccountsName, ControlAccountsName, ControlAccountsID, SQLQuery.FormatBDNumber(amt3), "1");
                    sumAmt3 += Convert.ToDecimal(amt3);
                }
            }

            //Operating Profit
            decimal op = gp - sumAmt3;
            /* body += "</table><br/><h4> </h4>" +
                       "<table class='table'>" +
                       "<tr><td><b>Operating profit</b></td><td><b>" + SQLQuery.FormatBDNumber(op) + "<b></td></tr>";*/
            InsertPL_IS("Operating profit", ControlAccountsName, "Operating profit", ControlAccountsID, SQLQuery.FormatBDNumber(op), "0");


            //Add:Non operating income
            //body += "</table><br/><h4>Less : Non operating income:</h4>" + "<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0301' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='03') ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt4 = SumByControl(ControlAccountsID, "Cr", dateFrom, dateTo);
                if (amt4 != "0.00")
                {
                    //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt4) + "</td></tr>";
                    InsertPL_IS("Add : Non operating income:", ControlAccountsName, ControlAccountsName, ControlAccountsID, SQLQuery.FormatBDNumber(amt4), "1");

                    sumAmt4 += Convert.ToDecimal(amt4);
                }
            }

            //Net profit before provision
            decimal npbp = op + sumAmt4;
            decimal npRate = 0;
            if (sumAmt1 > 0)
            {
                npRate = npbp / sumAmt1 * 100M;
            }/*
                body += "</table><br/><h4> </h4>" +
                          "<table class='table'>" +
                          "<tr><td><b>Net profit before provision</b></td><td><b>" + SQLQuery.FormatBDNumber(npbp) + "<b></td></tr>" +
                          "<tr><td>Net profit rate</td><td>" + Math.Round(npRate, 2) + "%</td></tr>";
                          */
            InsertPL_IS("Net profit before provision", ControlAccountsName, "Net profit before provision", ControlAccountsID, SQLQuery.FormatBDNumber(npbp), "0");
            InsertPL_IS("Net profit before provision", ControlAccountsName, "Net profit rate", ControlAccountsID, Math.Round(npRate, 2) + "%", "0");


            if (npbp > 0)
            {
                decimal tax = npbp * 35M / 100M;
                decimal gr = npbp / 10M; //10%= 10/100
                                         /*body += "</table><br/><h4> </h4>" +
                                                   "<table class='table'>" +
                                                   "<tr><td>Provision For Tax (35%)</td><td>" + SQLQuery.FormatBDNumber(tax) + "</td></tr>" +
                                                   "<tr><td>General reserve (10%)</td><td>" + SQLQuery.FormatBDNumber(gr) + "</td></tr>";*/
                InsertPL_IS("Equity", ControlAccountsName, "Provision For Tax (35%)", ControlAccountsID, SQLQuery.FormatBDNumber(tax), "0");
                InsertPL_IS("Equity", ControlAccountsName, "General reserve (10%)", ControlAccountsID, SQLQuery.FormatBDNumber(gr), "1");


                string noOfShares = SQLQuery.ReturnString("Select NoOFShares from Shareholdersequitydb WHERE FinYear=(Select top(1) Financial_Year from   tblFinancial_Year WHERE Opening_Date<='" + dateFrom + "' AND Closing_Date>='" + dateTo + "')");
                if (noOfShares == "")
                {
                    noOfShares = SQLQuery.ReturnString("Select NoOFShares from Shareholdersequitydb WHERE id=(Select MAX(id) from Shareholdersequitydb) ");
                }


                decimal transferred = npbp - tax - gr;
                decimal earning = (npbp - tax) / Convert.ToDecimal(noOfShares);
                /*
                body += "</table><br/><h4> </h4>" +
                          "<table class='table'>" +
                          "<tr><td>Net profit  after provision transferred to Balance sheet</td><td>" + SQLQuery.FormatBDNumber(transferred) + "</td></tr>" +
                          "<tr><td>Earnings per share (60,000 share)</td><td>" + SQLQuery.FormatBDNumber(earning) + "</td></tr></table>";*/
                InsertPL_IS("Equity", ControlAccountsName, "Net profit  after provision transferred to Balance sheet", ControlAccountsID, SQLQuery.FormatBDNumber(transferred), "2");
                InsertPL_IS("Equity", ControlAccountsName, "Earnings per share (" + SQLQuery.FormatBDNumber(noOfShares).Substring(0, SQLQuery.FormatBDNumber(noOfShares).Length - 3) + " share)", ControlAccountsID, SQLQuery.FormatBDNumber(earning), "0");

            }

            //ltrResult.Text = body;
            //ltrQR.Text = "From " + txtdateFrom.Text + " To " + txtdateTo.Text;

            return "";

        //}
        //catch (Exception ex)
        //{

        //    Notify(ex.ToString(), "error",lblMsg);
        //    return ex.ToString();
        //}
    }

    private string SumByControl(string cid, string btype, string dateFrom, string dateTo)
    {
        if (btype == "Dr")
        {
            return SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
                    cid + "'))) and ISApproved ='A' and EntryDate>='" + dateFrom + "' and EntryDate<='" + dateTo + "'");
        }
        else
        {
            return SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0) - ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
                    cid + "'))) and ISApproved ='A' and EntryDate>='" + dateFrom + "' and EntryDate<='" + dateTo + "'");
        }
    }

    //BalanceSheet Process

    private  string InsertPL_BS(string group, string subGrp, string head, string headid, string balance, string showline)
    {
        return ("Insert INTO Yearly_PL (AccGroup, SubGroup, AccHeadName,AccHeadID, BalanceDr, ShowLine,FinYear,Report) VALUES ('" + group + "','" + subGrp + "','" + head + "','"+ headid + "','" + balance + "', '" + showline + "','"+ ddYear1.SelectedValue+ "','BL')");
    }
    private void  InsertBL_Data(string dateFrom, string dateTo)
    {
        //try
        //{
            string inserQuery = ("DELETE FROM Yearly_PL WHERE(FinYear = '" + ddYear1.SelectedValue + "') AND (Report = 'BL')");
            // SQLQuery.ExecNonQry("Delete Yearly_PL");

            string amt1 = "", ControlAccountsID = "", ControlAccountsName = "", amt2 = "", amt3 = "", amt4 = "", amt5 = "", Maindecks = "", UpperLevelp = "", UpperLevels = "", Quarter = "", mgo = "", tank = "", FPK1 = "", FPK2 = "", FPK3 = "", APK1 = "", APK2 = "", APK3 = "", Chainlocker = "", DB1 = "", DB2 = "", DB3 = "", DB4 = "", ProjectID = "", FMean = "", AMean = "", FAMean = "", MidMean = "", MeanOfMean = "", QuarterMean = "", MainMean = "", UpperMean = "", Density = "", Qty = "";
            decimal sumAmt1 = 0, sumAmt2 = 0;

            //Non-Current Assets
            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0101' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='01') ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt1 = SumByControl(ControlAccountsID, "Dr", dateTo);
                if (amt1 != "0.00")
                {
                    //body += "<tr><td><b>" + ControlAccountsName + "</b></td><td>" + SQLQuery.FormatBDNumber(amt1) + "</td></tr>";
                    inserQuery += InsertPL_BS("Non-Current Assets", ControlAccountsName, ControlAccountsName, ControlAccountsID, SQLQuery.FormatBDNumber(amt1), "0");

                    sumAmt1 += Convert.ToDecimal(amt1);
                }
            }
            //body += "<tr><td><b>Total non current Assets</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt1.ToString()) + "</td></tr>";
            inserQuery += InsertPL_BS("Non-Current Assets", ControlAccountsName, "Total non current Assets", ControlAccountsID, SQLQuery.FormatBDNumber(sumAmt1), "1");

            //Current Assets
            //body += "</table><br/><h4>Current Assets</h4>" +"<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0101' ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt2 = SumByControl(ControlAccountsID, "Dr", dateTo);
                if (amt2 != "0.00")
                {
                    //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt2) + "</td></tr>";
                    inserQuery += InsertPL_BS("Current Assets", ControlAccountsName, ControlAccountsName, ControlAccountsID, SQLQuery.FormatBDNumber(amt2), "0");

                    sumAmt2 += Convert.ToDecimal(amt2);
                }
            }
            /*
            body += "<tr><td><b>Total Current Assets</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt2.ToString()) + "</td></tr>";

            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Total Assets</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt1 + sumAmt2).ToString()) + "</td></tr>";
*/
            inserQuery += InsertPL_BS("Current Assets", ControlAccountsName, "Total Current Assets", ControlAccountsID, SQLQuery.FormatBDNumber(sumAmt2.ToString()), "1");
            inserQuery += InsertPL_BS("Current Assets", ControlAccountsName, "Total Assets", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(sumAmt1 + sumAmt2).ToString()), "2");

            //Shareholders' Equity 
            decimal sumAmt3 = 0, sumAmt4 = 0, sumAmt5 = 0;
            //body += "</table><br/><h4>Shareholders' Equity</h4>" +"<table class='table'>";

            decimal transferred = Convert.ToDecimal(GetEquity("transferred", dateTo));
            if (transferred < 0)
            {
                transferred = Convert.ToDecimal(GetEquity("npbp", dateTo));
            }
            int i = 0;

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID IN (Select AccountsID from Accounts Where GroupID='05') ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt3 = SumByControl(ControlAccountsID, "Cr", dateTo);

                if (i == 0)//Net profit after provision will sum with Retained Earnings (i for First Control A/C)
                {
                    amt3 = Convert.ToString(Convert.ToDecimal(amt3) + transferred);
                    i++;
                }

                if (amt3 != "0.00")
                {
                    //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt3) + "</td></tr>";
                    inserQuery += InsertPL_BS("Shareholders Equity", ControlAccountsName, ControlAccountsName, ControlAccountsID, SQLQuery.FormatBDNumber(amt3), "0");

                    sumAmt3 += Convert.ToDecimal(amt3);
                }
            }

            string gr_IS = SQLQuery.ReturnString("Select BalanceDr from  Yearly_PL WHERE FinYear='"+ddYear1.SelectedValue+ "' AND Report='IS' AND AccHeadName='General reserve (10%)'");
            decimal gr = Convert.ToDecimal(gr_IS);

            if (gr > 0)
            {
                sumAmt3 += gr;
                //body += "<tr><td>General reserve (10%)</td><td>" + SQLQuery.FormatBDNumber(gr) + "</td></tr>";
                inserQuery += InsertPL_BS("Shareholders Equity", ControlAccountsName, "General reserve (10%)", ControlAccountsID, SQLQuery.FormatBDNumber(gr), "1");

            }

            //body += "</table><br/>" +"<table class='table'>" + "<tr><td><strong>Total Shareholders' Equity</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt3).ToString()) + "</td></tr>";
            inserQuery += InsertPL_BS("Shareholders Equity", ControlAccountsName, "Total Shareholders Equity", ControlAccountsID, SQLQuery.FormatBDNumber(sumAmt3), "1");



            //Non-Current Liabilities

            //body += "</table><br/><h4>Non-Current Liabilities</h4>" +"<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0201' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='02') ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt4 = SumByControl(ControlAccountsID, "Cr", dateTo);
                if (amt4 != "0.00")
                {
                    //body += "<tr><td><b>" + ControlAccountsName + "</b></td><td>" + SQLQuery.FormatBDNumber(amt4) + "</td></tr>";
                    inserQuery += InsertPL_BS("Non-Current Liabilities", ControlAccountsName, ControlAccountsName, ControlAccountsID, SQLQuery.FormatBDNumber(amt4), "0");
                    sumAmt4 += Convert.ToDecimal(amt4);
                }
            }
            //body += "<tr><td><b>Total Long Term Loan</b></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt4).ToString()) + "</td></tr>";
            inserQuery += InsertPL_BS("Non-Current Liabilities", ControlAccountsName, "Total Long Term Loan", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(sumAmt4).ToString()), "1");


            //Current liabilities
            //body += "</table><br/><h4>Current liabilities</h4>" +"<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0201' ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt5 = SumByControl(ControlAccountsID, "Cr", dateTo);
                if (amt5 != "0.00")
                {
                    //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt5) + "</td></tr>";
                    inserQuery += InsertPL_BS("Current liabilities", ControlAccountsName, ControlAccountsName, ControlAccountsID, SQLQuery.FormatBDNumber(amt5), "0");

                    sumAmt5 += Convert.ToDecimal(amt5);
                }
            }

            decimal tax = Convert.ToDecimal(GetEquity("tax", dateTo));
            if (tax > 0)
            {
                sumAmt5 += tax;
                //body += "<tr><td><strong>Provision For Tax (35%)</strong></td><td>" + tax + "</td></tr>";
                inserQuery += InsertPL_BS("Current liabilities", ControlAccountsName, "Provision For Tax (35%)", ControlAccountsID, SQLQuery.FormatBDNumber(tax), "1");

            }
            /*
            body += "<tr><td><b>Total current liabilities</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt5) + "</td></tr>";

            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Total Liabilities</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt4 + sumAmt5).ToString()) + "</td></tr>";
*/
            inserQuery += InsertPL_BS("Current liabilities", ControlAccountsName, "Total current liabilities", ControlAccountsID, SQLQuery.FormatBDNumber(sumAmt5), "1");
            inserQuery += InsertPL_BS("Current liabilities", ControlAccountsName, "Total Liabilities", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(sumAmt4 + sumAmt5).ToString()), "2");


            //Total
            decimal e_l = (sumAmt3 + sumAmt4 + sumAmt5);
            decimal nav = (sumAmt1 + sumAmt2) - (sumAmt4 + sumAmt5);
            /*
            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Total Equity and Liabilities</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(e_l)) + "</td></tr>";


            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Net Asset Value (NAV) </strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(nav)) + "</td></tr>";


            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Net Asset Value (NAV) per Share</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(nav / 60000M)) + "</td></tr>";
                      */
            inserQuery += InsertPL_BS("Total Equity and Liabilities", ControlAccountsName, "Total Equity and Liabilities", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(e_l)), "1");
            inserQuery += InsertPL_BS("Current liabilities", ControlAccountsName, "Net Asset Value (NAV)", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(nav)), "0");

            string noOfShares = SQLQuery.ReturnString(" Select NoOFShares from Shareholdersequitydb WHERE FinYear=(Select top(1) Financial_Year from   tblFinancial_Year WHERE Opening_Date<='" + dateTo + "' AND Closing_Date>='" + dateTo + "')");
            if (noOfShares == "")
            {
                noOfShares = "60000";// SQLQuery.ReturnString(" Select NoOFShares from Shareholdersequitydb WHERE id=(Select MAX(id) from Shareholdersequitydb) ");
            }

            inserQuery += InsertPL_BS(" Current liabilities", ControlAccountsName, "Net Asset Value (NAV) per Share", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(nav / Convert.ToDecimal(noOfShares))), "2");

            //Ratio Analysis
            inserQuery += InsertPL_BS(" Debt/ Equity Ratio", ControlAccountsName, "Total Liabilities", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(sumAmt4 + sumAmt5).ToString()), "1");
            inserQuery += InsertPL_BS(" Debt/ Equity Ratio", ControlAccountsName, "Total Equity", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(sumAmt3)), "0");

            decimal shortLongTerm = Convert.ToDecimal(SumByControl("020101", "Cr", dateTo)) - Convert.ToDecimal(SumByControl("020201", "Cr", dateTo));
            inserQuery += InsertPL_BS(" Debt/ Equity Ratio", ControlAccountsName, "Short Term Loan+Long Term Loan", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(shortLongTerm, 2).ToString()), "1");
            inserQuery += InsertPL_BS(" Debt/ Equity Ratio", ControlAccountsName, "Total Equity", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(sumAmt3)), "0");

            //ltrResult.Text = body;
            //ltrQR.Text = "From " + txtdateFrom.Text + " To " + txtdateTo.Text;

             SQLQuery.ExecNonQry(inserQuery);

        //}
        //catch (Exception ex)
        //{
        //    lblMsg.Attributes.Add("class", "xerp_error");
        //    lblMsg.Text = "ERROR: " + ex.ToString();
        //    //DataTable dt = null;
        //    //return dt;
        //}
    }

    //Balance Sheet 2

    private string InsertPL_BS2(string group, string subGrp, string head, string headid, string balance, string showline)
    {
        return ("Insert INTO Yearly_PL2 (AccGroup, SubGroup, AccHeadName,AccHeadID, BalanceDr, ShowLine,FinYear,Report) VALUES ('" + group + "','" + subGrp + "','" + head + "','" + headid + "','" + balance + "', '" + showline + "','" + ddYear1.SelectedValue + "','BL')");
    }
    private void InsertBL_Data2(string dateFrom, string dateTo)
    {
        //try
        //{
            string inserQuery2 = ("DELETE FROM Yearly_PL2 WHERE(FinYear = '" + ddYear1.SelectedValue + "') AND (Report = 'BL')");
            // SQLQuery.ExecNonQry("Delete Yearly_PL");

            string amt1 = "", ControlAccountsID = "", ControlAccountsName = "", amt2 = "", amt3 = "", amt4 = "", amt5 = "", Maindecks = "", UpperLevelp = "", UpperLevels = "", Quarter = "", mgo = "", tank = "", FPK1 = "", FPK2 = "", FPK3 = "", APK1 = "", APK2 = "", APK3 = "", Chainlocker = "", DB1 = "", DB2 = "", DB3 = "", DB4 = "", ProjectID = "", FMean = "", AMean = "", FAMean = "", MidMean = "", MeanOfMean = "", QuarterMean = "", MainMean = "", UpperMean = "", Density = "", Qty = "";
            decimal sumAmt1 = 0, sumAmt2 = 0;

            //Non-Current Assets
            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0101' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='01') ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt1 = SumByControl(ControlAccountsID, "Dr", dateTo);
                //if (amt1 != "0.00")
                //{
                    //body += "<tr><td><b>" + ControlAccountsName + "</b></td><td>" + SQLQuery.FormatBDNumber(amt1) + "</td></tr>";
                    inserQuery2 += InsertPL_BS2("Non-Current Assets", ControlAccountsName, ControlAccountsName, ControlAccountsID, SQLQuery.FormatBDNumber(amt1), "0");

                    sumAmt1 += Convert.ToDecimal(amt1);
                //}
            }
            //body += "<tr><td><b>Total non current Assets</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt1.ToString()) + "</td></tr>";
            //inserQuery += InsertPL_BS("Non-Current Assets", ControlAccountsName, "Total non current Assets", ControlAccountsID, SQLQuery.FormatBDNumber(sumAmt1), "1");

            //Current Assets
            //body += "</table><br/><h4>Current Assets</h4>" +"<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0101' ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt2 = SumByControl(ControlAccountsID, "Dr", dateTo);
                //if (amt2 != "0.00")
                //{
                    //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt2) + "</td></tr>";
                    inserQuery2 += InsertPL_BS2("Current Assets", ControlAccountsName, ControlAccountsName, ControlAccountsID, SQLQuery.FormatBDNumber(amt2), "0");

                    sumAmt2 += Convert.ToDecimal(amt2);
                //}
            }
            /*
            body += "<tr><td><b>Total Current Assets</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt2.ToString()) + "</td></tr>";

            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Total Assets</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt1 + sumAmt2).ToString()) + "</td></tr>";
*/
          //  inserQuery += InsertPL_BS("Current Assets", ControlAccountsName, "Total Current Assets", ControlAccountsID, SQLQuery.FormatBDNumber(sumAmt2.ToString()), "1");
          // inserQuery += InsertPL_BS("Current Assets", ControlAccountsName, "Total Assets", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(sumAmt1 + sumAmt2).ToString()), "2");

            //Shareholders' Equity 
            decimal sumAmt3 = 0, sumAmt4 = 0, sumAmt5 = 0;
            //body += "</table><br/><h4>Shareholders' Equity</h4>" +"<table class='table'>";

            decimal transferred = Convert.ToDecimal(GetEquity("transferred", dateTo));
            //if (transferred < 0)
           // {
                transferred = Convert.ToDecimal(GetEquity("npbp", dateTo));
           // }
            int i = 0;

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID IN (Select AccountsID from Accounts Where GroupID='05') ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt3 = SumByControl(ControlAccountsID, "Cr", dateTo);

               // if (i == 0)//Net profit after provision will sum with Retained Earnings (i for First Control A/C)
               // {
                    amt3 = Convert.ToString(Convert.ToDecimal(amt3) + transferred);
                    i++;
               // }

               // if (amt3 != "0.00")
               // {
                    //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt3) + "</td></tr>";
                    inserQuery2 += InsertPL_BS2("Shareholders Equity", ControlAccountsName, ControlAccountsName, ControlAccountsID, SQLQuery.FormatBDNumber(amt3), "0");

                    sumAmt3 += Convert.ToDecimal(amt3);
               // }
            }

            string gr_IS = SQLQuery.ReturnString("Select BalanceDr from  Yearly_PL WHERE FinYear='" + ddYear1.SelectedValue + "' AND Report='IS' AND AccHeadName='General reserve (10%)'");
            decimal gr = Convert.ToDecimal(gr_IS);

           // if (gr > 0)
           // {
                sumAmt3 += gr;
                //body += "<tr><td>General reserve (10%)</td><td>" + SQLQuery.FormatBDNumber(gr) + "</td></tr>";
                inserQuery2 += InsertPL_BS2("Shareholders Equity", ControlAccountsName, "General reserve (10%)", ControlAccountsID, SQLQuery.FormatBDNumber(gr), "1");

           // }

            //body += "</table><br/>" +"<table class='table'>" + "<tr><td><strong>Total Shareholders' Equity</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt3).ToString()) + "</td></tr>";
         //   inserQuery += InsertPL_BS("Shareholders Equity", ControlAccountsName, "Total Shareholders Equity", ControlAccountsID, SQLQuery.FormatBDNumber(sumAmt3), "1");



            //Non-Current Liabilities

            //body += "</table><br/><h4>Non-Current Liabilities</h4>" +"<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0201' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='02') ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt4 = SumByControl(ControlAccountsID, "Cr", dateTo);
              //  if (amt4 != "0.00")
              //  {
                    //body += "<tr><td><b>" + ControlAccountsName + "</b></td><td>" + SQLQuery.FormatBDNumber(amt4) + "</td></tr>";
                    inserQuery2 += InsertPL_BS2("Non-Current Liabilities", ControlAccountsName, ControlAccountsName, ControlAccountsID, SQLQuery.FormatBDNumber(amt4), "0");
                    sumAmt4 += Convert.ToDecimal(amt4);
               // }
            }
            //body += "<tr><td><b>Total Long Term Loan</b></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt4).ToString()) + "</td></tr>";
          //  inserQuery += InsertPL_BS("Non-Current Liabilities", ControlAccountsName, "Total Long Term Loan", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(sumAmt4).ToString()), "1");


            //Current liabilities
            //body += "</table><br/><h4>Current liabilities</h4>" +"<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0201' ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt5 = SumByControl(ControlAccountsID, "Cr", dateTo);
              //  if (amt5 != "0.00")
              //  {
                    //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt5) + "</td></tr>";
                    inserQuery2 += InsertPL_BS2("Current liabilities", ControlAccountsName, ControlAccountsName, ControlAccountsID, SQLQuery.FormatBDNumber(amt5), "0");

                    sumAmt5 += Convert.ToDecimal(amt5);
              //  }
            }

            decimal tax = Convert.ToDecimal(GetEquity("tax", dateTo));
            if (tax > 0)
            {
                sumAmt5 += tax;
                //body += "<tr><td><strong>Provision For Tax (35%)</strong></td><td>" + tax + "</td></tr>";
                inserQuery2 += InsertPL_BS2("Current liabilities", ControlAccountsName, "Provision For Tax (35%)", ControlAccountsID, SQLQuery.FormatBDNumber(tax), "1");

            }
            /*
            body += "<tr><td><b>Total current liabilities</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt5) + "</td></tr>";

            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Total Liabilities</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt4 + sumAmt5).ToString()) + "</td></tr>";
*/
            inserQuery2 += InsertPL_BS2("Current liabilities", ControlAccountsName, "Total current liabilities", ControlAccountsID, SQLQuery.FormatBDNumber(sumAmt5), "1");
            inserQuery2 += InsertPL_BS2("Current liabilities", ControlAccountsName, "Total Liabilities", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(sumAmt4 + sumAmt5).ToString()), "2");


            //Total
            decimal e_l = (sumAmt3 + sumAmt4 + sumAmt5);
            decimal nav = (sumAmt1 + sumAmt2) - (sumAmt4 + sumAmt5);
            /*
            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Total Equity and Liabilities</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(e_l)) + "</td></tr>";


            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Net Asset Value (NAV) </strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(nav)) + "</td></tr>";


            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Net Asset Value (NAV) per Share</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(nav / 60000M)) + "</td></tr>";
                      */
            inserQuery2 += InsertPL_BS2("Total Equity and Liabilities", ControlAccountsName, "Total Equity and Liabilities", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(e_l)), "1");
            inserQuery2 += InsertPL_BS2("Current liabilities", ControlAccountsName, "Net Asset Value (NAV)", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(nav)), "0");

            string noOfShares = SQLQuery.ReturnString(" Select NoOFShares from Shareholdersequitydb WHERE FinYear=(Select top(1) Financial_Year from   tblFinancial_Year WHERE Opening_Date<='" + dateTo + "' AND Closing_Date>='" + dateTo + "')");
            if (noOfShares == "")
            {
                noOfShares = "60000";// SQLQuery.ReturnString(" Select NoOFShares from Shareholdersequitydb WHERE id=(Select MAX(id) from Shareholdersequitydb) ");
            }

            inserQuery2 += InsertPL_BS2(" Current liabilities", ControlAccountsName, "Net Asset Value (NAV) per Share", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(nav / Convert.ToDecimal(noOfShares))), "2");

            //Ratio Analysis
           // inserQuery += InsertPL_BS(" Debt/ Equity Ratio", ControlAccountsName, "Total Liabilities", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(sumAmt4 + sumAmt5).ToString()), "1");
           // inserQuery += InsertPL_BS(" Debt/ Equity Ratio", ControlAccountsName, "Total Equity", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(sumAmt3)), "0");

            decimal shortLongTerm = Convert.ToDecimal(SumByControl("020101", "Cr", dateTo)) - Convert.ToDecimal(SumByControl("020201", "Cr", dateTo));
           // inserQuery += InsertPL_BS(" Debt/ Equity Ratio", ControlAccountsName, "Short Term Loan+Long Term Loan", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(shortLongTerm, 2).ToString()), "1");
           // inserQuery += InsertPL_BS(" Debt/ Equity Ratio", ControlAccountsName, "Total Equity", ControlAccountsID, SQLQuery.FormatBDNumber(Math.Round(sumAmt3)), "0");

            //ltrResult.Text = body;
            //ltrQR.Text = "From " + txtdateFrom.Text + " To " + txtdateTo.Text;

            SQLQuery.ExecNonQry(inserQuery2);

        //}
        //catch (Exception ex)
        //{
        //    lblMsg.Attributes.Add("class", "xerp_error");
        //    lblMsg.Text = "ERROR: " + ex.ToString();
        //    //DataTable dt = null;
        //    //return dt;
        //}
    }




    private string SumByControl(string cid, string btype, string dateTo)
    {
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

    private decimal GetEquity(string rtype, string dateTo)
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
            amt1 = SumByControli(ControlAccountsID, "Cr", dateTo);
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
            amt2 = SumByControli(ControlAccountsID, "Dr", dateTo);
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
            amt3 = SumByControli(ControlAccountsID, "Dr", dateTo);
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
            amt4 = SumByControli(ControlAccountsID, "Cr", dateTo);
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
        //decimal earning = (npbp - tax) / 60000M;

        decimal rValue = 0;
        if (rtype == "tax")//Provision For Tax (35%)
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

        return Math.Round(rValue, 2);
    }
    private string SumByControli(string cid, string btype, string dateTo)
    {
        //string dateFrom = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
        //string dateTo = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");
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
                    "SELECT ISNULL(SUM(VoucherCR),0) - ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "'))) and ISApproved ='A' and  EntryDate<='" + dateTo + "'"));
        }
        return returnValue.ToString();
    }


    protected void ddYear1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string lpldate = SQLQuery.ReturnString("SELECT CONVERT(VARCHAR,LastProcessDate,103) FROM tblFinancial_Year WHERE Financial_Year_Number ='" + ddYear1.SelectedValue + "'");
        Notify("Last Process Date is "+ lpldate,"warn",lblMsg);
    }
}

