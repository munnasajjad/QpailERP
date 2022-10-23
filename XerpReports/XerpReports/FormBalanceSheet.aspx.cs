using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using RunQuery;

namespace Oxford.XerpReports
{
    public partial class FormBalanceSheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();

        }

        ReportDocument rpt = new ReportDocument();
        private void LoadGridData()
        {
            //decimal total2 = (decimal)0.0;
            //decimal qty2 = (decimal)0.0;
            //decimal vat2 = (decimal)0.0;
            //decimal TotalSales2 = (decimal)0.0;
            //decimal TotalWeight2 = (decimal)0.0;

            string lName = Page.User.Identity.Name.ToString();
            string prjID = "1";
            //string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);

            search(dateTo);

            
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT AccGroup as [group],AccHeadName as head, BalanceDr as balance, ShowLine from PL order by id");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.IncomeStatement);
            
            rpt.Load(Server.MapPath("CrptBalanceSheet.rpt"));

            //string datefield = "From: " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy");
            string datefield = "As on :	" + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            //rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;

        }

        private static void InsertPL(string group, string subGrp, string head, string balance, string ShowLine)
        {
            SQLQuery.ExecNonQry("Insert INTO PL (AccGroup, SubGroup, AccHeadName, BalanceDr, ShowLine) VALUES ('" + group + "','" + subGrp + "','" + head + "','" + balance + "', '" + ShowLine + "')");
        }
        
       private void search(string dateTo)
        {
            try
            {
                SQLQuery.ExecNonQry("Delete PL");

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
                        InsertPL("Non-Current Assets", ControlAccountsName, ControlAccountsName, SQLQuery.FormatBDNumber(amt1), "0");

                      //SQLQuery.ExecNonQry("UPDATE [dbo].[PL] SET AccGroup ='Non-Current Assets', SubGroup ='" + ControlAccountsName + "', AccHeadName ='" + ControlAccountsName + "', BalanceDr ='" + amt1 + amt1 + "', ShowLine ='0' Where  AccGroup ='WDV(Fixed Assets)'");

                        sumAmt1 += Convert.ToDecimal(amt1);
                    }
                }
                //body += "<tr><td><b>Total non current Assets</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt1.ToString()) + "</td></tr>";
                InsertPL("Non-Current Assets", ControlAccountsName, "Total non current Assets", SQLQuery.FormatBDNumber(sumAmt1), "1");

                //Current Assets
                //body += "</table><br/><h4>Current Assets</h4>" +"<table class='table'>";

                dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0101' ");
                foreach (DataRow drx in dtx.Rows)
                {
                    ControlAccountsID = drx["ControlAccountsID"].ToString();
                    ControlAccountsName = drx["ControlAccountsName"].ToString();
                    amt2 = SumByControl(ControlAccountsID,"Dr", dateTo);
                    if (amt2 != "0.00")
                    {
                        //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt2) + "</td></tr>";
                        InsertPL("Current Assets", ControlAccountsName, ControlAccountsName, SQLQuery.FormatBDNumber(amt2), "0");

                        sumAmt2 += Convert.ToDecimal(amt2);
                    }
                }
                /*
                body += "<tr><td><b>Total Current Assets</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt2.ToString()) + "</td></tr>";

                body += "</table><br/>" +
                          "<table class='table'>" +
                          "<tr><td><strong>Total Assets</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt1 + sumAmt2).ToString()) + "</td></tr>";
*/
                InsertPL("Current Assets", ControlAccountsName, "Total Current Assets", SQLQuery.FormatBDNumber(sumAmt2.ToString()), "1");
                InsertPL("Current Assets", ControlAccountsName, "Total Assets", SQLQuery.FormatBDNumber(Math.Round(sumAmt1 + sumAmt2).ToString()), "2");
                
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
                        InsertPL("Shareholders Equity", ControlAccountsName, ControlAccountsName, SQLQuery.FormatBDNumber(amt3), "0");

                        sumAmt3 += Convert.ToDecimal(amt3);
                    }
                }

                decimal gr = Convert.ToDecimal(GetEquity("gr", dateTo));

                if (gr > 0)
                {
                    sumAmt3 += gr;
                    //body += "<tr><td>General reserve (10%)</td><td>" + SQLQuery.FormatBDNumber(gr) + "</td></tr>";
                   InsertPL("Shareholders Equity", ControlAccountsName, "General reserve (10%)", SQLQuery.FormatBDNumber(gr), "1");

                }

                //body += "</table><br/>" +"<table class='table'>" + "<tr><td><strong>Total Shareholders' Equity</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt3).ToString()) + "</td></tr>";
                InsertPL("Shareholders Equity", ControlAccountsName, "Total Shareholders Equity", SQLQuery.FormatBDNumber(sumAmt3), "1");



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
                        InsertPL("Non-Current Liabilities", ControlAccountsName, ControlAccountsName, SQLQuery.FormatBDNumber(amt4), "0");
                        sumAmt4 += Convert.ToDecimal(amt4);
                    }
                }
                //body += "<tr><td><b>Total Long Term Loan</b></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt4).ToString()) + "</td></tr>";
                InsertPL("Non-Current Liabilities", ControlAccountsName, "Total Long Term Loan", SQLQuery.FormatBDNumber(Math.Round(sumAmt4).ToString()), "1");


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
                        InsertPL("Current liabilities", ControlAccountsName, ControlAccountsName, SQLQuery.FormatBDNumber(amt5), "0");

                        sumAmt5 += Convert.ToDecimal(amt5);
                    }
                }

                decimal tax = Convert.ToDecimal(GetEquity("tax", dateTo));
                if (tax > 0)
                {
                    sumAmt5 += tax;
                    //body += "<tr><td><strong>Provision For Tax (35%)</strong></td><td>" + tax + "</td></tr>";
                    InsertPL("Current liabilities", ControlAccountsName, "Provision For Tax (35%)", SQLQuery.FormatBDNumber(tax), "1");

                }
                /*
                body += "<tr><td><b>Total current liabilities</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt5) + "</td></tr>";

                body += "</table><br/>" +
                          "<table class='table'>" +
                          "<tr><td><strong>Total Liabilities</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt4 + sumAmt5).ToString()) + "</td></tr>";
*/
                InsertPL("Current liabilities", ControlAccountsName, "Total current liabilities", SQLQuery.FormatBDNumber(sumAmt5), "1");
                InsertPL("Current liabilities", ControlAccountsName, "Total Liabilities", SQLQuery.FormatBDNumber(Math.Round(sumAmt4 + sumAmt5).ToString()), "2");


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
                InsertPL("Total Equity and Liabilities", ControlAccountsName, "Total Equity and Liabilities", SQLQuery.FormatBDNumber(Math.Round(e_l)), "1");
                InsertPL("Current liabilities", ControlAccountsName, "Net Asset Value (NAV)", SQLQuery.FormatBDNumber(Math.Round(nav)), "0");

                string noOfShares = SQLQuery.ReturnString("Select NoOFShares from Shareholdersequitydb WHERE FinYear=(Select top(1) Financial_Year from   tblFinancial_Year WHERE Opening_Date<='" + dateTo + "' AND Closing_Date>='" + dateTo + "')");
                if (noOfShares == "")
                {
                    noOfShares = SQLQuery.ReturnString("Select NoOFShares from Shareholdersequitydb WHERE id=(Select MAX(id) from Shareholdersequitydb) ");
                }

                InsertPL("Current liabilities", ControlAccountsName, "Net Asset Value (NAV) per Share", SQLQuery.FormatBDNumber(Math.Round(nav / Convert.ToDecimal(noOfShares))), "2");

                //Ratio Analysis
                InsertPL("Debt/ Equity Ratio", ControlAccountsName, "Total Liabilities", SQLQuery.FormatBDNumber(Math.Round(sumAmt4 + sumAmt5).ToString()), "1");
                InsertPL("Debt/ Equity Ratio", ControlAccountsName, "Total Equity", SQLQuery.FormatBDNumber(Math.Round(sumAmt3)), "0");

                decimal shortLongTerm=Convert.ToDecimal(SumByControl("020101", "Cr", dateTo))- Convert.ToDecimal(SumByControl("020201", "Cr", dateTo));
                InsertPL("Debt/ Equity Ratio", ControlAccountsName, "Short Term Loan+Long Term Loan", SQLQuery.FormatBDNumber(Math.Round(shortLongTerm,2).ToString()), "1");
                InsertPL("Debt/ Equity Ratio", ControlAccountsName, "Total Equity", SQLQuery.FormatBDNumber(Math.Round(sumAmt3)), "0");

                //ltrResult.Text = body;
                //ltrQR.Text = "From " + txtdateFrom.Text + " To " + txtdateTo.Text;
            }
            catch (Exception ex)
            {
                //lblMsg.Attributes.Add("class", "xerp_error");
               // lblMsg.Text = "ERROR: " + ex.ToString();
                //DataTable dt = null;
                //return dt;
            }
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
            decimal earning = (npbp - tax) / 60000M;

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

        protected void CrystalReportViewer1_OnUnload(object sender, EventArgs e)
        {
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();
        }
        protected override void OnUnload(EventArgs e)
        {
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();
        }
    }
}