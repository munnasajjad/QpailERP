using CrystalDecisions.CrystalReports.Engine;
using RunQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Oxford.XerpReports
{
    public partial class FromCashFlow : System.Web.UI.Page
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
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);

            search(dateFrom, dateTo);
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT AccGroup as [group], SubGroup as [SubGrp], AccHeadName as [head], OpeningBalance, BalanceDr as [Dr], BalanceCr as [Cr], ClosingBalance, ShowLine from PL order by id");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.IncomeStatement);

            rpt.Load(Server.MapPath("CrptTrialbal.rpt"));

            //string datefield = "From: " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy");
            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString(" dd/MM/yyyy") + " To " + Convert.ToDateTime(dateTo).ToString(" dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            //rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;

        }

        private static void InsertPL(string head, string group, string subGrp, string opBal, string dr, string cr, string clBal)
        {
            SQLQuery.ExecNonQry("Insert INTO PL (AccGroup, SubGroup, AccHeadName, OpeningBalance, BalanceDr, BalanceCr, ClosingBalance) VALUES ('" + group + "','" + subGrp + "','" + head + "','" + opBal + "','" + dr + "', '" + cr + "','" + clBal + "')");
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

        private string search(string dateFrom, string dateTo)
        {
            try
            {
                SQLQuery.ExecNonQry("Delete PL");
                string acGroupPrev = "", acSubPrev = "", acControlPrev = "", acHeadPrev = "";
                string acGroup = "", acSub = "", acControl = "", acHead = "";
                string acGroupTxt = "", acSubTxt = "", acControlTxt = "", acHeadTxt = "";
                decimal grandTotalDr = 0; decimal grandTotalCr = 0;

                DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT sl, GroupID, GroupName, ProjectID FROM AccountGroup Order by sl");

                foreach (DataRow drx in dtx.Rows)
                {
                    decimal totalDr = 0; decimal totalCr = 0;
                    string acGroupID = drx["GroupID"].ToString();
                    acGroup = drx["GroupName"].ToString();

                    int subCount = Convert.ToInt32(SQLQuery.ReturnString(@"SELECT COUNT(AccountsID) FROM Accounts WHERE GroupID='" + acGroupID + "'"));
                    DataTable dtx2 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT sl, GroupID, AccountsID, AccountsName, ProjectID FROM Accounts WHERE GroupID='" + acGroupID + "' Order by sl");

                    foreach (DataRow drx2 in dtx2.Rows)
                    {
                        string acSubID = drx2["AccountsID"].ToString();
                        acSub = drx2["AccountsName"].ToString();

                        DataTable dtx3 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT sl, ControlAccountsID, ControlAccountsName, ProjectID FROM ControlAccount  WHERE AccountsID='" + acSubID + "'  Order by sl");
                        foreach (DataRow drx3 in dtx3.Rows)
                        {
                            string acControlID = drx3["ControlAccountsID"].ToString();
                            acControl = drx3["ControlAccountsName"].ToString();

                            decimal opBal = Convert.ToDecimal(OpClBalance(acControlID, "Dr", dateFrom));
                            decimal balDr = Convert.ToDecimal(ControlBalance(dateFrom, acControlID, "Dr", dateTo));
                            decimal balCr = Convert.ToDecimal(ControlBalance(dateFrom, acControlID, "Cr", dateTo));
                            decimal clBal = Convert.ToDecimal(OpClBalance(acControlID, "Dr", dateTo));

                            if (balDr > balCr)
                            {
                                balDr = balDr - balCr;
                                balCr = 0;
                            }
                            else if (balCr > balDr)
                            {
                                opBal = opBal * -1;
                                balCr = balCr - balDr;
                                balDr = 0;
                                clBal = clBal * -1;
                            }

                            if (acControl == "Retained Earnings")
                            {
                                decimal transferred = Convert.ToDecimal(GetEquity("transferred", dateTo));
                                if (transferred < 0)
                                {
                                    transferred = Convert.ToDecimal(GetEquity("npbp", dateTo));
                                }

                                balCr = balCr + transferred;
                            }
                            else if (acControl == "General reserve")
                            {
                                balCr = Convert.ToDecimal(GetEquity("gr", dateTo));
                            }

                            InsertPL(acGroup, acSub, acControl, opBal.ToString(), balDr.ToString(), balCr.ToString(), clBal.ToString());


                            /*
                            DataTable dtx4 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT EntryID, AccountsHeadID, AccountsHeadName, OpBalDr, OpBalCr, ProjectID FROM HeadSetup WHERE ControlAccountsID='" + acControlID + "'  Order by ControlAccountsID");

                            foreach (DataRow drx4 in dtx4.Rows)
                            {
                                string acHeadID = drx4["AccountsHeadID"].ToString();
                                acHead = drx4["AccountsHeadName"].ToString();
                                
                                decimal balDr = Convert.ToDecimal(HeadBalance(acHeadID, "Dr", dateTo));
                                decimal balCr = Convert.ToDecimal(HeadBalance(acHeadID, "Cr", dateTo));

                                if (balDr > balCr)
                                {
                                    balDr = balDr - balCr;
                                    balCr = 0;
                                }
                                else if (balCr > balDr)
                                {
                                    balCr = balCr - balDr;
                                    balDr = 0;
                                }

                                if (balDr > 0 || balCr > 0)
                                {
                                    //body += "<tr>" + comparePrevValue(acGroup, acGroupPrev) + comparePrevValue(acSub, acSubPrev) + comparePrevValue(acControl, acControlPrev) + "<td>" + acHeadID + "</td><td>" + acHead + "</td><td class='a-right'>" + SQLQuery.FormatBDNumber(balDr) + " </td><td>" + SQLQuery.FormatBDNumber(balCr) + "</td></tr>";
                                    //InsertPL(acGroup, acSub, acControl, SQLQuery.FormatBDNumber(amt1), "0");

                                    totalDr += Convert.ToDecimal(ControlBalance(acHeadID, "Dr", dateTo));
                                    totalCr += Convert.ToDecimal(ControlBalance(acHeadID, "Cr", dateTo));
                                }
                                //}
                                acGroupPrev = acGroup;
                                acSubPrev = acSub;
                                acControlPrev = acControl;

                            }
                            */

                            acControlPrev = acControl;
                        }
                        acSubPrev = acSub;
                    }
                    acGroupPrev = acGroup;
                    //body += "<tr style='background-color: #D3D3D3;border-bottom: 6px solid #0071C8;'><td></td><td></td><td></td><td></td><td class='a-right'><b>Total " + acGroup + ":</b></td><td class='a-right'>" + SQLQuery.FormatBDNumber(totalDr) + " </td><td class='a-right'>" + SQLQuery.FormatBDNumber(totalCr) + "</td></tr>";
                    grandTotalDr += totalDr; grandTotalCr += totalCr;
                }
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

        private string ControlBalance(string dateFrom, string cid, string btype, string dateTo)
        {
            decimal returnValue = 0;

            if (btype == "Dr")
            {
                returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate >='" + dateFrom + "' AND OpDate <='" + dateTo + "'"));
                returnValue += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE AccountsHeadID IN (Select AccountsHeadID from HeadSetup WHERE ControlAccountsID = '" + cid + "') and ISApproved ='A' and EntryDate>='" + dateFrom + "' and EntryDate<='" + dateTo + "'"));
            }
            else
            {
                returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0)  FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate >='" + dateFrom + "' AND OpDate <='" + dateTo + "'"));
                returnValue += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0)  FROM VoucherDetails WHERE AccountsHeadID IN (Select AccountsHeadID from HeadSetup WHERE ControlAccountsID = '" + cid + "') and ISApproved ='A' and EntryDate>='" + dateFrom + "' and EntryDate<='" + dateTo + "'"));
            }
            return returnValue.ToString();
        }

        private string OpClBalance(string cid, string btype, string dateTo)
        {
            decimal returnValue = 0;

            if (btype == "Dr")
            {
                returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0)-ISNULL(SUM(OpBalCr),0) FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate <'" + dateTo + "'"));
                returnValue += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0)-ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE AccountsHeadID IN (Select AccountsHeadID from HeadSetup WHERE ControlAccountsID = '" + cid + "') and ISApproved ='A' and EntryDate<'" + dateTo + "'"));
            }
            else
            {
                returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0)-ISNULL(SUM(OpBalDr),0)  FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate <'" + dateTo + "'"));
                returnValue += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0)-ISNULL(SUM(VoucherDR),0)  FROM VoucherDetails WHERE AccountsHeadID IN (Select AccountsHeadID from HeadSetup WHERE ControlAccountsID = '" + cid + "') and ISApproved ='A' and EntryDate<'" + dateTo + "'"));
            }
            return returnValue.ToString();
        }
        private string SumByControl(string cid, string btype, string dateTo)
        {
            if (btype == "Dr")
            {
                return
                    SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "'))) and ISApproved ='A' and EntryDate<='" + dateTo + "'");
            }
            else
            {
                return
                    SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0) - ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "'))) and ISApproved ='A' and EntryDate<='" + dateTo + "'");
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