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
    public partial class FormShareholerEquity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadGridData();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
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
            string op1Date = Convert.ToString(Request.QueryString["op1"]);
            string cl1Date = Convert.ToString(Request.QueryString["cl1"]);
            string op2Date = Convert.ToString(Request.QueryString["op2"]);
            string cl2Date = Convert.ToString(Request.QueryString["cl2"]);
            //string y1 = Convert.ToString(Request.QueryString["y1"]);
            //string y2 = Convert.ToString(Request.QueryString["y2"]);
            
            DataTable dt=new DataTable();
            dt.Columns.Add(new DataColumn("Particulars", typeof(string)));
            dt.Columns.Add(new DataColumn("ShareCapital", typeof(decimal)));
            dt.Columns.Add(new DataColumn("RetEarning", typeof(decimal)));
            dt.Columns.Add(new DataColumn("THolidayReserve", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Total", typeof(decimal)));
            dt.Columns.Add(new DataColumn("GeneralReserve", typeof(decimal)));
            dt.Columns.Add(new DataColumn("TotalShare", typeof(string)));
            //dt.Columns.Add("Particulars");
            //dt.Columns.Add("RetEarning");
            //dt.Columns.Add("THolidayReserve");
            //dt.Columns.Add("Total");
            //dt.Columns.Add("GeneralReserve");
            //dt.Columns.Add("TotalShare");
            //dt.Columns.Add("TRetEarning");
            //dt.Columns.Add("TTHolidayReserve");

            //search(op1Date, cl1Date);
            //search(op2Date, cl2Date);

            string sc1 = SQLQuery.ReturnString("Select  BalanceDr1 from Shareholdersequity WHERE AccHeadName='Share Capital' ");
            string re1 = SQLQuery.ReturnString("Select  BalanceDr1 from Shareholdersequity WHERE AccHeadName='Retained Earnings' ");
            string th1 = SQLQuery.ReturnString("Select  BalanceDr1 from Shareholdersequity WHERE AccHeadName='Tex Holiday reserve' ");
            string Gr1 = SQLQuery.ReturnString("Select  BalanceDr1 from Shareholdersequity WHERE AccHeadName='General reserve (10%)' ");

            string sc2 = SQLQuery.ReturnString("Select  BalanceDr2 from Shareholdersequity WHERE AccHeadName='Share Capital' ");
            string re2 = SQLQuery.ReturnString("Select  BalanceDr2 from Shareholdersequity WHERE AccHeadName='Retained Earnings' ");
            string th2 = SQLQuery.ReturnString("Select  BalanceDr2 from Shareholdersequity WHERE AccHeadName='Tex Holiday reserve' ");
            string Gr2 = SQLQuery.ReturnString("Select  BalanceDr2 from Shareholdersequity WHERE AccHeadName='General reserve (10%)' ");

            string sc3 = SQLQuery.ReturnString("Select  BalanceDr3 from Shareholdersequity WHERE AccHeadName='Share Capital' ");
            string re3 = SQLQuery.ReturnString("Select  BalanceDr3 from Shareholdersequity WHERE AccHeadName='Retained Earnings' ");
            string th3 = SQLQuery.ReturnString("Select  BalanceDr3 from Shareholdersequity WHERE AccHeadName='Tex Holiday reserve' ");
            string Gr3 = SQLQuery.ReturnString("Select  BalanceDr3 from Shareholdersequity WHERE AccHeadName='General reserve (10%)' ");

            string sc4 = SQLQuery.ReturnString("Select  BalanceDr4 from Shareholdersequity WHERE AccHeadName='Share Capital' ");
            string re4 = SQLQuery.ReturnString("Select  BalanceDr4 from Shareholdersequity WHERE AccHeadName='Retained Earnings' ");
            string th4 = SQLQuery.ReturnString("Select  BalanceDr4 from Shareholdersequity WHERE AccHeadName='Tex Holiday reserve' ");
            string Gr4 = SQLQuery.ReturnString("Select  BalanceDr4 from Shareholdersequity WHERE AccHeadName='General reserve (10%)' ");

            string npy2 = SQLQuery.ReturnString("Select Top(1) Year2Balance from Shareholdersequity WHERE AccHeadName='Net Profit for the Year' ");

            DataRow dtRow = dt.NewRow();
            dtRow["Particulars"] = Convert.ToDateTime(op1Date).ToString("MMMM dd,yyyy");
            dtRow["ShareCapital"] = Convert.ToDecimal(sc1);
            dtRow["GeneralReserve"] = Convert.ToDecimal(Gr1);
            dtRow["RetEarning"] = Convert.ToDecimal(re1);
            dtRow["THolidayReserve"] = Convert.ToDecimal(th1);
            dtRow["Total"] = Convert.ToDecimal(sc1) + Convert.ToDecimal(Gr1) + Convert.ToDecimal(re1) + Convert.ToDecimal(th1);
            dt.Rows.Add(dtRow);

            dtRow = dt.NewRow();
            dtRow["Particulars"] = "General Reserve (10%)";
            dtRow["ShareCapital"] = "0";
            dtRow["GeneralReserve"] = Convert.ToDecimal(Gr2) - Convert.ToDecimal(Gr1);
            dtRow["RetEarning"] = "0";
            dtRow["THolidayReserve"] = "0";
            dtRow["Total"] = Convert.ToDecimal(Gr2) - Convert.ToDecimal(Gr1);
            dt.Rows.Add(dtRow);

            dtRow = dt.NewRow();
            dtRow["Particulars"] = "Tex Holiday reserve";
            dtRow["ShareCapital"] = "0";
            dtRow["GeneralReserve"] = "0";
            dtRow["RetEarning"] = "0";
            dtRow["THolidayReserve"] = Convert.ToDecimal(th2) - Convert.ToDecimal(th1);
            dtRow["Total"] = Convert.ToDecimal(th2) - Convert.ToDecimal(th1);
            dt.Rows.Add(dtRow);


            dtRow = dt.NewRow();
            dtRow["Particulars"] = "Net Profit For the year";
            dtRow["ShareCapital"] = "0";
            dtRow["GeneralReserve"] = "0";
            dtRow["RetEarning"] = Convert.ToDecimal(re2) - Convert.ToDecimal(re1);
            dtRow["THolidayReserve"] ="0";
            dtRow["Total"] = Convert.ToDecimal(re2) - Convert.ToDecimal(re1);
            //dtRow["Total"] = Convert.ToString((Convert.ToDecimal(sc2) + Convert.ToDecimal(re2) + Convert.ToDecimal(th2)) - (Convert.ToDecimal(sc1) + Convert.ToDecimal(re1) + Convert.ToDecimal(th1)));
            dt.Rows.Add(dtRow);
            
            dtRow = dt.NewRow();
            dtRow["Particulars"] = "Share Capital";
            dtRow["ShareCapital"] = Convert.ToDecimal(sc2) - Convert.ToDecimal(sc1);
            dtRow["GeneralReserve"] = "0";
            dtRow["RetEarning"] = "0";
            dtRow["THolidayReserve"] = "0";
            dtRow["Total"] = Convert.ToDecimal(sc2) - Convert.ToDecimal(sc1);
            //dtRow["Total"] = Convert.ToString((Convert.ToDecimal(sc2) + Convert.ToDecimal(re2) + Convert.ToDecimal(th2)) - (Convert.ToDecimal(sc1) + Convert.ToDecimal(re1) + Convert.ToDecimal(th1)));
            dt.Rows.Add(dtRow);

            dtRow = dt.NewRow();
            dtRow["Particulars"] = "Balance " + Convert.ToDateTime(cl1Date).ToString("MMMM dd,yyyy");
            dtRow["ShareCapital"] = Convert.ToDecimal(sc2);
            dtRow["GeneralReserve"] = Convert.ToDecimal(Gr2);
            dtRow["RetEarning"] = Convert.ToDecimal(re2);
            dtRow["THolidayReserve"] = Convert.ToDecimal(th3);
            dtRow["Total"] = Convert.ToDecimal(sc2) + Convert.ToDecimal(Gr2) + Convert.ToDecimal(re2) + Convert.ToDecimal(th2);
            dt.Rows.Add(dtRow);

            dtRow = dt.NewRow();
            dtRow["TotalShare"] ="/n";
            dt.Rows.Add(dtRow);

            dtRow = dt.NewRow();
            dtRow["Particulars"] = Convert.ToDateTime(op2Date).ToString("MMMM dd,yyyy");
            dtRow["ShareCapital"] = Convert.ToDecimal(sc3);
            dtRow["GeneralReserve"] = Convert.ToDecimal(Gr3);
            dtRow["RetEarning"] = Convert.ToDecimal(re3);
            dtRow["THolidayReserve"] = Convert.ToDecimal(th3);
            //dtRow["Total"] = Convert.ToString(Convert.ToDecimal(sc1) + Convert.ToDecimal(re1) + Convert.ToDecimal(th1) + Convert.ToDecimal(re2) - Convert.ToDecimal(re1));
            dtRow["Total"] = Convert.ToDecimal(sc3) + Convert.ToDecimal(Gr3) + Convert.ToDecimal(re3) + Convert.ToDecimal(th3);
            dt.Rows.Add(dtRow);

            dtRow = dt.NewRow();
            dtRow["Particulars"] = "General Reserve (10%)";
            dtRow["ShareCapital"] = "0";
            dtRow["GeneralReserve"] = Convert.ToDecimal(Gr4) - Convert.ToDecimal(Gr3);
            dtRow["RetEarning"] = "0";
            dtRow["THolidayReserve"] = "0";
            dtRow["Total"] = Convert.ToDecimal(Gr4) - Convert.ToDecimal(Gr3);
            dt.Rows.Add(dtRow);

            dtRow = dt.NewRow();
            dtRow["Particulars"] = "Tex Holiday reserve";
            dtRow["ShareCapital"] = "0";
            dtRow["GeneralReserve"] = "0";
            dtRow["RetEarning"] = "0";
            dtRow["THolidayReserve"] = Convert.ToDecimal(th4) - Convert.ToDecimal(th3);
            dtRow["Total"] = Convert.ToDecimal(th4) - Convert.ToDecimal(th3);
            dt.Rows.Add(dtRow);

            dtRow = dt.NewRow();
            dtRow["Particulars"] = "Net Profit For the year";
            dtRow["ShareCapital"] = "0";
            dtRow["GeneralReserve"] = "0";
            dtRow["RetEarning"] = Convert.ToDecimal(re4) -Convert.ToDecimal(re3);
            dtRow["THolidayReserve"] = "0";
            dtRow["Total"] = Convert.ToDecimal(re4) - Convert.ToDecimal(re3);
            dt.Rows.Add(dtRow);

            dtRow = dt.NewRow();
            dtRow["Particulars"] = "Share Capital";
            dtRow["ShareCapital"] = Convert.ToDecimal(sc4) - Convert.ToDecimal(sc3);
            dtRow["GeneralReserve"] = "0";
            dtRow["RetEarning"] = "0";
            dtRow["THolidayReserve"] = "0";
            dtRow["Total"] = Convert.ToDecimal(sc4) - Convert.ToDecimal(sc3);
            //dtRow["Total"] = Convert.ToString((Convert.ToDecimal(sc2) + Convert.ToDecimal(re2) + Convert.ToDecimal(th2)) - (Convert.ToDecimal(sc1) + Convert.ToDecimal(re1) + Convert.ToDecimal(th1)));
            dt.Rows.Add(dtRow);

            dtRow = dt.NewRow();
            dtRow["Particulars"] = "Balance " + Convert.ToDateTime(cl2Date).ToString("MMMM dd,yyyy");
            dtRow["ShareCapital"] = Convert.ToDecimal(sc4);
            dtRow["GeneralReserve"] = Convert.ToDecimal(Gr4);
            dtRow["RetEarning"] = Convert.ToDecimal(re4);
            dtRow["THolidayReserve"] = Convert.ToDecimal(th4);
            dtRow["Total"] = Convert.ToDecimal(sc4) + Convert.ToDecimal(Gr4) + Convert.ToDecimal(re4) + Convert.ToDecimal(th4);
            dt.Rows.Add(dtRow);
            

            DataTableReader dr2 = dt.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.ShareEquity);


            rpt.Load(Server.MapPath("CrptShareholderEquity.rpt"));

            string datefield = "For the year ended " + Convert.ToDateTime(cl2Date).ToString("MMMM dd,yyyy");
            //string datefield = "opDate " + Convert.ToDateTime(opDate).ToString(" dd/MM/yyyy") + " clDate " + Convert.ToDateTime(clDate).ToString(" dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@y1", cl2Date);
            //rpt.SetParameterValue("@y2", y2);
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
                string acType = "", acSub = "", acControl = "", acHead = "";
                string acGroupTxt = "", acSubTxt = "", acControlTxt = "", acHeadTxt = "";
                decimal grandTotalDr = 0; decimal grandTotalCr = 0;

                DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT ID, Accounts_Type, ControlAccount  FROM CashFlow_Setup");

                foreach (DataRow drx in dtx.Rows)
                {
                    decimal totalDr = 0; decimal totalCr = 0;
                    acControl = drx["ControlAccount"].ToString();
                    acType = drx["Accounts_Type"].ToString();

                    //int subCount = Convert.ToInt32(SQLQuery.ReturnString(@"SELECT COUNT(AccountsID) FROM Accounts WHERE GroupID='" + acGroupID + "'"));
                    DataTable dtx2 = SQLQuery.ReturnDataTable(@"SELECT EntryID, GroupID, AccountsID, ControlAccountsID, AccountsHeadID, AccountsHeadName, OpBalDr, OpBalCr, QtyDr, QtyCr, OpPcsDr, OpPcsCr, Rate, AccountsOpeningBalance, ProjectID, OpDate, ServerDateTime, Emark, IsActive FROM HeadSetup WHERE ControlAccountsID ='" + acControl + "'");

                    foreach (DataRow drx2 in dtx2.Rows)
                    {
                        string acID = drx2["AccountsHeadID"].ToString();
                        acHead = drx2["AccountsHeadName"].ToString();

                        decimal balDr = Convert.ToDecimal(HeadBalance(dateFrom, acID, "Dr", dateTo));
                        decimal balCr = Convert.ToDecimal(HeadBalance(dateFrom, acID, "Cr", dateTo));

                        //if (balDr > balCr)
                        //{
                        //    balDr = balDr - balCr;
                        //    balCr = 0;
                        //}
                        //else if (balCr > balDr)
                        //{
                        //    balCr = balCr - balDr;
                        //    balDr = 0;
                        //}

                        //if (acControl == "Retained Earnings")
                        //{
                        //    decimal transferred = Convert.ToDecimal(GetEquity("transferred", dateTo));
                        //    if (transferred < 0)
                        //    {
                        //        transferred = Convert.ToDecimal(GetEquity("npbp", dateTo));
                        //    }

                        //    balCr = balCr + transferred;
                        //}
                        //else if (acControl == "General reserve")
                        //{
                        //    balCr = Convert.ToDecimal(GetEquity("gr", dateTo));
                        //}

                        InsertPL(acHead, acType, acID, "0", balDr.ToString(), balCr.ToString(), "0");


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

                                totalDr += Convert.ToDecimal(HeadBalance(acHeadID, "Dr", dateTo));
                                totalCr += Convert.ToDecimal(HeadBalance(acHeadID, "Cr", dateTo));
                            }
                            //}
                            acGroupPrev = acGroup;
                            acSubPrev = acSub;
                            acControlPrev = acControl;

                        }
                        */

                        acControlPrev = acControl;
                        //}
                        acSubPrev = acSub;
                    }
                    //acGroupPrev = acGroup;
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

        private string HeadBalance(string dateFrom, string cid, string btype, string dateTo)
        {
            decimal returnValue = 0;

            if (btype == "Dr")
            {
                returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE (AccountsHeadID = '" + cid + "') AND OpDate >='" + dateFrom + "' AND OpDate <='" + dateTo + "'"));
                returnValue += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + cid + "' and ISApproved ='A' and EntryDate>='" + dateFrom + "' and EntryDate<='" + dateTo + "'"));
            }
            else
            {
                returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0)  FROM HeadSetup WHERE (AccountsHeadID = '" + cid + "') AND OpDate >='" + dateFrom + "' AND OpDate <='" + dateTo + "'"));
                returnValue += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0)  FROM VoucherDetails WHERE AccountsHeadID = '" + cid + "' and ISApproved ='A' and EntryDate>='" + dateFrom + "' and EntryDate<='" + dateTo + "'"));
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