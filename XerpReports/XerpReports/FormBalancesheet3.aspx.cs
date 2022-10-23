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
    public partial class FormBalancesheet3 : System.Web.UI.Page
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
            string opDate1 = Convert.ToString(Request.QueryString["y1"]);
            string clDate1 = Convert.ToString(Request.QueryString["y2"]);
            string opDate2 = Convert.ToString(Request.QueryString["y3"]);
            string clDate2 = Convert.ToString(Request.QueryString["y4"]);

            string y11 = Convert.ToString(Request.QueryString["y11"]);
            string y22 = Convert.ToString(Request.QueryString["y22"]);
            string finYearId11 = y11;
            string finYearId22 = y22;

            SQLQuery.ExecNonQry("Delete PL");

           // string finYearId1 =
                  //  SQLQuery.ReturnString(
                   //     "Select ISNULL(MAX(Financial_Year_Number),0) from tblFinancial_Year WHERE Financial_Year_Number='" + finYearId11 + "' ");
                string finYearId1 = finYearId11;
                string finYearId2 = finYearId22;
                //string finYearId3 =
                //     SQLQuery.ReturnString(
                //         "Select ISNULL(MAX(Financial_Year_Number),0) from tblFinancial_Year WHERE Financial_Year_Number<'" +
                //         finYearId11 + "' ");
                //string finYearId4 = finYearId22;
                 

                string y1Date1 =
                    SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                          finYearId1 + "'");
                string y1Date2 =
                    SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                          finYearId1 + "'");

                string y2Date1 =
                    SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                          finYearId2 + "'");
                string y2Date2 =
                    SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                          finYearId2 + "'");


             y1Date1 = Convert.ToDateTime(opDate1).ToString("yyyy-MM-dd");
             y1Date2 = Convert.ToDateTime(clDate1).ToString("yyyy-MM-dd");
             y2Date1 = Convert.ToDateTime(opDate2).ToString("yyyy-MM-dd");
             y2Date2 = Convert.ToDateTime(clDate2).ToString("yyyy-MM-dd");
                


            search(y1Date2, y2Date2, 1);
           // search(y2Date2, y2Date2, 1);
            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT AccGroup as [group],AccHeadName as head, BalanceDr as balance, BalanceCr as Cr, ShowLine from PL order by id");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.IncomeStatement);

            rpt.Load(Server.MapPath("CrptBalanceSheet2.rpt"));

            //string datefield = "From: " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy");
            string datefield = "As at :	" + Convert.ToDateTime(opDate1).ToString("MMMM dd,yyyy");
            string year1 = Convert.ToDateTime(clDate1).ToString("MMMM dd,yyyy");
            string year2 = Convert.ToDateTime(clDate2).ToString("MMMM dd,yyyy");
            //string datefield = "" + Convert.ToDateTime(dateTo1).ToString("dd/MM/yyyy") + "          " + Convert.ToDateTime(dateTo2).ToString("dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@y11", year1);
            rpt.SetParameterValue("@y22", year2);
            //rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;

        }

        private static void InsertPL(string @group, string subGrp, string head, string balance1, string balance2, string ShowLine, int yearNo)
        {
            SQLQuery.ExecNonQry("Insert INTO PL (AccGroup, SubGroup, AccHeadName, BalanceDr, BalanceCr, ShowLine) VALUES ('" + group + "','" + subGrp + "','" + head + "','" + balance1 + "','" + balance2 + "', '" + ShowLine + "')");
        }

        private void search(string dateTo1, string dateTo2, int yearNo)
        {
            try
            {
                SQLQuery.ExecNonQry("Delete PL");

                string amt1 = "", amt2 = "", ControlAccountsID = "", ControlAccountsName = "", amt2x = "", amt2y = "", amt3x = "", amt3y = "", amt4x = "", amt4y = "", amt5x = "", amt5y = "", Maindecks = "", UpperLevelp = "", UpperLevels = "", Quarter = "", mgo = "", tank = "", FPK1 = "", FPK2 = "", FPK3 = "", APK1 = "", APK2 = "", APK3 = "", Chainlocker = "", DB1 = "", DB2 = "", DB3 = "", DB4 = "", ProjectID = "", FMean = "", AMean = "", FAMean = "", MidMean = "", MeanOfMean = "", QuarterMean = "", MainMean = "", UpperMean = "", Density = "", Qty = "";
                decimal sumAmt1x = 0, sumAmt1y = 0, sumAmt2x = 0, sumAmt2y = 0;

                //Non-Current Assets
                DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0101' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='01') ");
                foreach (DataRow drx in dtx.Rows)
                {
                    ControlAccountsID = drx["ControlAccountsID"].ToString();
                    ControlAccountsName = drx["ControlAccountsName"].ToString();
                    amt1 = SumByControl(ControlAccountsID, "Dr", dateTo1);
                    amt2 = SumByControl(ControlAccountsID, "Dr", dateTo2);
                    if (amt1 != "0.00")
                    {
                        //body += "<tr><td><b>" + ControlAccountsName + "</b></td><td>" + SQLQuery.FormatBDNumber(amt1) + "</td></tr>";
                        InsertPL("Non-Current Assets", ControlAccountsName, ControlAccountsName, SQLQuery.FormatBDNumber(amt1), SQLQuery.FormatBDNumber(amt2), "0", yearNo);

                        sumAmt1x += Convert.ToDecimal(amt1);
                        sumAmt1y += Convert.ToDecimal(amt2);
                    }
                }
                //body += "<tr><td><b>Total non current Assets</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt1.ToString()) + "</td></tr>";
                InsertPL("Non-Current Assets", ControlAccountsName, "Total non current Assets", SQLQuery.FormatBDNumber(sumAmt1x), SQLQuery.FormatBDNumber(sumAmt1y), "1", yearNo);

                //Current Assets
                //body += "</table><br/><h4>Current Assets</h4>" +"<table class='table'>";

                dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0101' ");
                foreach (DataRow drx in dtx.Rows)
                {
                    ControlAccountsID = drx["ControlAccountsID"].ToString();
                    ControlAccountsName = drx["ControlAccountsName"].ToString();
                    amt2x = SumByControl(ControlAccountsID, "Dr", dateTo1);
                    amt2y = SumByControl(ControlAccountsID, "Dr", dateTo2);
                    if (amt2x != "0.00")
                    {
                        //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt2) + "</td></tr>";
                        InsertPL("Current Assets", ControlAccountsName, ControlAccountsName, SQLQuery.FormatBDNumber(amt2x), SQLQuery.FormatBDNumber(amt2y), "0", yearNo);

                        sumAmt2x += Convert.ToDecimal(amt2x);
                        sumAmt2y += Convert.ToDecimal(amt2y);
                    }
                }
                /*
                body += "<tr><td><b>Total Current Assets</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt2.ToString()) + "</td></tr>";

                body += "</table><br/>" +
                          "<table class='table'>" +
                          "<tr><td><strong>Total Assets</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt1 + sumAmt2).ToString()) + "</td></tr>";
*/
                InsertPL("Current Assets", ControlAccountsName, "Total Current Assets", SQLQuery.FormatBDNumber(sumAmt2x.ToString()), SQLQuery.FormatBDNumber(sumAmt2y.ToString()), "1", yearNo);
                InsertPL("Current Assets", ControlAccountsName, "Total Assets", SQLQuery.FormatBDNumber(Math.Round(sumAmt1x + sumAmt2x).ToString()), SQLQuery.FormatBDNumber(Math.Round(sumAmt1y + sumAmt2y).ToString()), "2", yearNo);

                //Shareholders' Equity 
                decimal sumAmt3x = 0, sumAmt4x = 0, sumAmt5x = 0, sumAmt3y = 0, sumAmt4y = 0, sumAmt5y = 0;
                //body += "</table><br/><h4>Shareholders' Equity</h4>" +"<table class='table'>";

                decimal transferred1 = Convert.ToDecimal(GetEquity("transferred", dateTo1));
                decimal transferred2 = Convert.ToDecimal(GetEquity("transferred", dateTo2));
                if (transferred1 < 0)
                {
                    transferred1 = Convert.ToDecimal(GetEquity("npbp", dateTo1));
                    transferred2 = Convert.ToDecimal(GetEquity("npbp", dateTo2));
                }
                int i = 0;

                dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID IN (Select AccountsID from Accounts Where GroupID='05') ");
                foreach (DataRow drx in dtx.Rows)
                {
                    ControlAccountsID = drx["ControlAccountsID"].ToString();
                    ControlAccountsName = drx["ControlAccountsName"].ToString();
                    amt3x = SumByControl(ControlAccountsID, "Cr", dateTo1);
                    amt3y = SumByControl(ControlAccountsID, "Cr", dateTo2);

                    if (i == 0)//Net profit after provision will sum with Retained Earnings (i for First Control A/C)
                    {
                        amt3x = Convert.ToString(Convert.ToDecimal(amt3x) + transferred1);
                        amt3y = Convert.ToString(Convert.ToDecimal(amt3y) + transferred2);
                        i++;
                    }

                    if (amt3x != "0.00")
                    {
                        //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt3) + "</td></tr>";
                        InsertPL("Shareholders Equity", ControlAccountsName, ControlAccountsName, SQLQuery.FormatBDNumber(amt3x), SQLQuery.FormatBDNumber(amt3y), "0", yearNo);

                        sumAmt3x += Convert.ToDecimal(amt3x);
                        sumAmt3y += Convert.ToDecimal(amt3y);
                    }
                }

                decimal grx = Convert.ToDecimal(GetEquity("grx", dateTo1));
                decimal gry = Convert.ToDecimal(GetEquity("gry", dateTo2));

                if (grx > 0)
                {
                    sumAmt3x += grx;
                    sumAmt3y += gry;
                    //body += "<tr><td>General reserve (10%)</td><td>" + SQLQuery.FormatBDNumber(gr) + "</td></tr>";
                    InsertPL("Shareholders Equity", ControlAccountsName, "General reserve (10%)", SQLQuery.FormatBDNumber(grx), SQLQuery.FormatBDNumber(gry), "1", yearNo);

                }

                //body += "</table><br/>" +"<table class='table'>" + "<tr><td><strong>Total Shareholders' Equity</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt3).ToString()) + "</td></tr>";
                InsertPL("Shareholders Equity", ControlAccountsName, "Total Shareholders Equity", SQLQuery.FormatBDNumber(sumAmt3x), SQLQuery.FormatBDNumber(sumAmt3y), "1", yearNo);



                //Non-Current Liabilities

                //body += "</table><br/><h4>Non-Current Liabilities</h4>" +"<table class='table'>";

                dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0201' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='02') ");
                foreach (DataRow drx in dtx.Rows)
                {
                    ControlAccountsID = drx["ControlAccountsID"].ToString();
                    ControlAccountsName = drx["ControlAccountsName"].ToString();
                    amt4x = SumByControl(ControlAccountsID, "Cr", dateTo1);
                    amt4y = SumByControl(ControlAccountsID, "Cr", dateTo2);
                    if (amt4x != "0.00")
                    {
                        //body += "<tr><td><b>" + ControlAccountsName + "</b></td><td>" + SQLQuery.FormatBDNumber(amt4) + "</td></tr>";
                        InsertPL("Non-Current Liabilities", ControlAccountsName, ControlAccountsName, SQLQuery.FormatBDNumber(amt4x), SQLQuery.FormatBDNumber(amt4y), "0", yearNo);
                        sumAmt4x += Convert.ToDecimal(amt4x);
                    }
                }
                //body += "<tr><td><b>Total Long Term Loan</b></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt4).ToString()) + "</td></tr>";
                InsertPL("Non-Current Liabilities", ControlAccountsName, "Total Long Term Loan", SQLQuery.FormatBDNumber(Math.Round(sumAmt4x).ToString()), SQLQuery.FormatBDNumber(Math.Round(sumAmt4y).ToString()), "1", yearNo);


                //Current liabilities
                //body += "</table><br/><h4>Current liabilities</h4>" +"<table class='table'>";

                dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0201' ");
                foreach (DataRow drx in dtx.Rows)
                {
                    ControlAccountsID = drx["ControlAccountsID"].ToString();
                    ControlAccountsName = drx["ControlAccountsName"].ToString();
                    amt5x = SumByControl(ControlAccountsID, "Cr", dateTo1);
                    amt5y = SumByControl(ControlAccountsID, "Cr", dateTo2);
                    if (amt5x != "0.00")
                    {
                        //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt5) + "</td></tr>";
                        InsertPL("Current liabilities", ControlAccountsName, ControlAccountsName, SQLQuery.FormatBDNumber(amt5x), SQLQuery.FormatBDNumber(amt5y), "0", yearNo);

                        sumAmt5x += Convert.ToDecimal(amt5x);
                        sumAmt5y += Convert.ToDecimal(amt5y);
                    }
                }

                decimal tax1 = Convert.ToDecimal(GetEquity("tax", dateTo1));
                decimal tax2 = Convert.ToDecimal(GetEquity("tax", dateTo2));
                if (tax1 > 0)
                {
                    sumAmt5x += tax1;
                    sumAmt5y += tax2;
                    //body += "<tr><td><strong>Provision For Tax (35%)</strong></td><td>" + tax + "</td></tr>";
                    InsertPL("Current liabilities", ControlAccountsName, "Provision For Tax (35%)", SQLQuery.FormatBDNumber(tax1), SQLQuery.FormatBDNumber(tax2), "1", yearNo);

                }
                /*
                body += "<tr><td><b>Total current liabilities</b></td><td>" + SQLQuery.FormatBDNumber(sumAmt5) + "</td></tr>";

                body += "</table><br/>" +
                          "<table class='table'>" +
                          "<tr><td><strong>Total Liabilities</strong></td><td>" + SQLQuery.FormatBDNumber(Math.Round(sumAmt4 + sumAmt5).ToString()) + "</td></tr>";
*/
                InsertPL("Current liabilities", ControlAccountsName, "Total current liabilities", SQLQuery.FormatBDNumber(sumAmt5x), SQLQuery.FormatBDNumber(sumAmt5y), "1", yearNo);
                InsertPL("Current liabilities", ControlAccountsName, "Total Liabilities", SQLQuery.FormatBDNumber(Math.Round(sumAmt4x + sumAmt5x).ToString()), SQLQuery.FormatBDNumber(Math.Round(sumAmt4y + sumAmt5y).ToString()), "2", yearNo);


                //Total
                decimal elx = (sumAmt3x + sumAmt4x + sumAmt5x);
                decimal ely = (sumAmt3y + sumAmt4y + sumAmt5y);
                decimal navx = (sumAmt1x + sumAmt2x) - (sumAmt4x + sumAmt5x);
                decimal navy = (sumAmt1y + sumAmt2y) - (sumAmt4y + sumAmt5y);
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
                InsertPL("Total Equity and Liabilities", ControlAccountsName, "Total Equity and Liabilities", SQLQuery.FormatBDNumber(Math.Round(elx)), SQLQuery.FormatBDNumber(Math.Round(ely)), "1", yearNo);
                InsertPL("Current liabilities", ControlAccountsName, "Net Asset Value (NAV)", SQLQuery.FormatBDNumber(Math.Round(navx)), SQLQuery.FormatBDNumber(Math.Round(navy)), "0", yearNo);

                string noOfShares1 = SQLQuery.ReturnString("Select NoOFShares from Shareholdersequitydb WHERE FinYear='" + dateTo1 + "'");
                string noOfShares2 = SQLQuery.ReturnString("Select NoOFShares from Shareholdersequitydb WHERE FinYear='" + dateTo2 + "'");

                InsertPL("Current liabilities", ControlAccountsName, "Net Asset Value (NAV) per Share", SQLQuery.FormatBDNumber(Math.Round(navx / Convert.ToDecimal(noOfShares1))), SQLQuery.FormatBDNumber(Math.Round(navy / Convert.ToDecimal(noOfShares2))), "2", yearNo);

                //Ratio Analysis
                InsertPL("Debt/ Equity Ratio", ControlAccountsName, "Total Liabilities", SQLQuery.FormatBDNumber(Math.Round(sumAmt4x + sumAmt5x).ToString()), SQLQuery.FormatBDNumber(Math.Round(sumAmt4y + sumAmt5y).ToString()), "1", yearNo);
                InsertPL("Debt/ Equity Ratio", ControlAccountsName, "Total Equity", SQLQuery.FormatBDNumber(Math.Round(sumAmt3x)), SQLQuery.FormatBDNumber(Math.Round(sumAmt3y)), "0", yearNo);

                decimal shortLongTerm1 = Convert.ToDecimal(SumByControl("020101", "Cr", dateTo1)) - Convert.ToDecimal(SumByControl("020201", "Cr", dateTo1));
                decimal shortLongTerm2 = Convert.ToDecimal(SumByControl("020101", "Cr", dateTo2)) - Convert.ToDecimal(SumByControl("020201", "Cr", dateTo2));
                InsertPL("Debt/ Equity Ratio", ControlAccountsName, "Short Term Loan+Long Term Loan", SQLQuery.FormatBDNumber(Math.Round(shortLongTerm1, 2).ToString()), SQLQuery.FormatBDNumber(Math.Round(shortLongTerm2, 2).ToString()), "1", yearNo);
                InsertPL("Debt/ Equity Ratio", ControlAccountsName, "Total Equity", SQLQuery.FormatBDNumber(Math.Round(sumAmt3x)), SQLQuery.FormatBDNumber(Math.Round(sumAmt3y)), "0", yearNo);

                //ltrResult.Text = body;
                //ltrQR.Text = "From " + txtdateFrom.Text + " To " + txtdateTo.Text;

                //SQLQuery.ExecNonQry("Delete CashFlow WHERE FinYearId='"++"' ");

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