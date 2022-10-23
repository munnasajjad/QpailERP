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
    public partial class FormChartOfAccounts : System.Web.UI.Page
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
            //string dateTo = Convert.ToString(Request.QueryString["dateTo"]);

            //search(dateFrom, dateTo);
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT HeadSetup.AccountsHeadID, HeadSetup.AccountsHeadName, HeadSetup.OpBalDr, HeadSetup.OpBalCr, AccountGroup.GroupName, Accounts.AccountsName, ControlAccount.ControlAccountsName
FROM            HeadSetup INNER JOIN
                         AccountGroup ON HeadSetup.GroupID = AccountGroup.GroupID INNER JOIN
                         Accounts ON HeadSetup.AccountsID = Accounts.AccountsID INNER JOIN
                         ControlAccount ON HeadSetup.ControlAccountsID = ControlAccount.ControlAccountsID 
						 GROUP BY HeadSetup.AccountsHeadID, HeadSetup.AccountsHeadName, HeadSetup.OpBalDr, HeadSetup.OpBalCr, AccountGroup.GroupName, Accounts.AccountsName, ControlAccount.ControlAccountsName
						 HAVING OpBalDr>0 OR OpBalCr>0  ORDER BY HeadSetup.AccountsHeadID");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.ChartAcc);
            
            rpt.Load(Server.MapPath("CrptChartOfAccounts.rpt"));

            //string datefield = "From: " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy");
            //string datefield = "From" + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            //rpt.SetParameterValue("@date", datefield);
            //rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;

        }

        private static void InsertPL(string group, string subGrp, string head, string balance, string ShowLine)
        {
            SQLQuery.ExecNonQry("Insert INTO PL (AccGroup, SubGroup, AccHeadName, BalanceDr, ShowLine) VALUES ('" + group + "','" + subGrp + "','" + head + "','" + balance + "', '" + ShowLine + "')");
        }
        private string search(string dateFrom, string dateTo)
        {
            try
            {
                SQLQuery.ExecNonQry("Delete PL");
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
                        InsertPL("Sales", ControlAccountsName, ControlAccountsName, SQLQuery.FormatBDNumber(amt1), "0");

                        sumAmt1 += Convert.ToDecimal(amt1);
                    }
                }

                //Less : Direct expenses
                //body += "</table><br/><h4>Less : Direct expenses</h4>" + "<table class='table'>";

                dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0402' ");
                foreach (DataRow drx in dtx.Rows)
                {
                    ControlAccountsID = drx["ControlAccountsID"].ToString();
                    ControlAccountsName = drx["ControlAccountsName"].ToString();
                    amt2 = SumByControl(ControlAccountsID, "Dr", dateFrom, dateTo);
                    if (amt2 != "0.00")
                    {
                        //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt2) + "</td></tr>";
                        InsertPL("Less : Direct expenses", ControlAccountsName, ControlAccountsName, SQLQuery.FormatBDNumber(amt2), "1");
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
                InsertPL(" ", ControlAccountsName, "Gross profit", SQLQuery.FormatBDNumber(gp), "0");
                InsertPL(" ", ControlAccountsName, "Gross profit rate", Math.Round(gpRate, 2) + "%", "0");

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
                        InsertPL("Less : Operating Expenses", ControlAccountsName, ControlAccountsName, SQLQuery.FormatBDNumber(amt3), "1");
                        sumAmt3 += Convert.ToDecimal(amt3);
                    }
                }

                //Operating Profit
                decimal op = gp - sumAmt3;
                /* body += "</table><br/><h4> </h4>" +
                           "<table class='table'>" +
                           "<tr><td><b>Operating profit</b></td><td><b>" + SQLQuery.FormatBDNumber(op) + "<b></td></tr>";*/
                InsertPL("Operating profit", ControlAccountsName, "Operating profit", SQLQuery.FormatBDNumber(op), "0");


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
                        InsertPL("Less : Non operating income:", ControlAccountsName, ControlAccountsName, SQLQuery.FormatBDNumber(amt4), "1");

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
                InsertPL("Net profit before provision", ControlAccountsName, "Net profit before provision", SQLQuery.FormatBDNumber(npbp), "0");
                InsertPL("Net profit before provision", ControlAccountsName, "Net profit rate", Math.Round(npRate, 2) + "%", "0");


                if (npbp > 0)
                {
                    decimal tax = npbp * 35M / 100M;
                    decimal gr = npbp / 10M; //10%= 10/100
                                             /*body += "</table><br/><h4> </h4>" +
                                                       "<table class='table'>" +
                                                       "<tr><td>Provision For Tax (35%)</td><td>" + SQLQuery.FormatBDNumber(tax) + "</td></tr>" +
                                                       "<tr><td>General reserve (10%)</td><td>" + SQLQuery.FormatBDNumber(gr) + "</td></tr>";*/
                    InsertPL("Equity", ControlAccountsName, "Provision For Tax (35%)", SQLQuery.FormatBDNumber(tax), "0");
                    InsertPL("Equity", ControlAccountsName, "General reserve (10%)", SQLQuery.FormatBDNumber(gr), "1");


                    decimal transferred = npbp - tax - gr;
                    decimal earning = (npbp - tax) / 60000M;
                    /*
                    body += "</table><br/><h4> </h4>" +
                              "<table class='table'>" +
                              "<tr><td>Net profit  after provision transferred to Balance sheet</td><td>" + SQLQuery.FormatBDNumber(transferred) + "</td></tr>" +
                              "<tr><td>Earnings per share (60,000 share)</td><td>" + SQLQuery.FormatBDNumber(earning) + "</td></tr></table>";*/
                    InsertPL("Equity", ControlAccountsName, "Net profit  after provision transferred to Balance sheet", SQLQuery.FormatBDNumber(transferred), "2");
                    InsertPL("Equity", ControlAccountsName, "Earnings per share (60,000 share)", SQLQuery.FormatBDNumber(earning), "0");

                }

                //ltrResult.Text = body;
                //ltrQR.Text = "From " + txtdateFrom.Text + " To " + txtdateTo.Text;

                return "";

            }
            catch (Exception ex)
            {
                //lblMsg.Attributes.Add("class", "xerp_error");
                //lblMsg.Text = "ERROR: " + ex.ToString();
                //DataTable dt = null;
                return ex.ToString();
            }
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