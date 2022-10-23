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
    public partial class FormCashFlow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try{
            LoadGridData();

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
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
            string opDate1 = Convert.ToString(Request.QueryString["op1"]);
            string clDate2 = Convert.ToString(Request.QueryString["op2"]);
            string opDate3 = Convert.ToString(Request.QueryString["op3"]);
            string clDate4 = Convert.ToString(Request.QueryString["op4"]);
            string y1 = Convert.ToString(Request.QueryString["1yd"]);
            string y2 = Convert.ToString(Request.QueryString["2yd"]);


            string endDate1 =
                SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                      clDate2 + "'");
            string endDate2 =
                SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                      clDate4 + "'");
            string op1 = Convert.ToDateTime(endDate1).ToString("yyyy-MM-dd");
            string cl2 = Convert.ToDateTime(endDate2).ToString("yyyy-MM-dd");

            search2();

            //Cash Flows from Operating Activities
            decimal transferred1 = Convert.ToDecimal(GetEquity("transferred", op1)); //Transferred to Balance Sheet
            decimal transferred2 = Convert.ToDecimal(GetEquity("transferred", cl2)); //
            SQLQuery.ExecNonQry(
                "Insert INTO CashFlow (ID, AccGroup, SubGroup, AccHeadName, Year1Balance, Year2Balance, ShowLine) VALUES ('01','Cash Flows from Operating Activities','Net Profit for the Year','Net Profit for the Year','" +
                transferred1 + "','" + transferred2 + "', '2')");

            //Cash flow from Investing activities

            string endDate11 =
                SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                      clDate2 + "'");
            string endDate22 =
                SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                      clDate2 + "'");
            string endDate33 =
                SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                      clDate4 + "'");
            string endDate44 =
                SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                      clDate4 + "'");

            string endDate111 = Convert.ToDateTime(endDate11).ToString("yyyy-MM-dd");
            string endDate222 = Convert.ToDateTime(endDate22).ToString("yyyy-MM-dd");
            string endDate333 = Convert.ToDateTime(endDate33).ToString("yyyy-MM-dd");
            string endDate444 = Convert.ToDateTime(endDate44).ToString("yyyy-MM-dd");

            decimal transferred11 = Convert.ToDecimal(Getequsition(endDate111, endDate222));
            decimal transferred22 = Convert.ToDecimal(Getequsition(endDate333, endDate444));
            decimal transferred33 = Convert.ToDecimal(Getequsition(endDate333, endDate444));
            decimal transferred44 = Convert.ToDecimal(Getequsition(endDate444, endDate444));

            //decimal amty1 = (Convert.ToDecimal(transferred22) - Convert.ToDecimal(transferred11));
            //decimal amty2 = (Convert.ToDecimal(transferred44) - Convert.ToDecimal(transferred33));
            //SQLQuery.ExecNonQry(
            //    "Insert INTO CashFlow (ID, AccGroup, SubGroup, AccHeadName, Year1Balance, Year2Balance, ShowLine) VALUES ('02','Cash Flows from Investing activities','Acquisition of Property , Plant & Equipment','Acquisition of Property , Plant & Equipment','" +
            //    transferred11 + "','" + transferred22 + "', '2')");

            // decimal transferred41 = Getinvestment(endDate111, endDate222);
            //  decimal transferred42 = Convert.ToDecimal(Getinvestment(endDate333, endDate444));
            // decimal transferred43 = Convert.ToDecimal(Getinvestment(endDate333, endDate444));
            // decimal transferred45 = Convert.ToDecimal(Getinvestment(endDate444, endDate444));
            //  SQLQuery.ExecNonQry("Insert INTO CashFlow (ID, AccGroup, SubGroup, AccHeadName, Year1Balance, Year2Balance, ShowLine) VALUES ('02','Cash Flows from Investing activities','Investment','Investment','" + transferred11 + "','" + transferred22 + "', '2')");
            //            string deprate = SQLQuery.ReturnString(@"SELECT        ISNULL(SUM(Products.Depreciation), 0)/2 AS deprate
            //            FROM Categories INNER JOIN
            //                         FixedAssets INNER JOIN
            //                         Products ON FixedAssets.ProductID = Products.ProductID ON Categories.CategoryID = Products.CategoryID INNER JOIN
            //                         ItemGrade ON Categories.GradeID = ItemGrade.GradeID INNER JOIN
            //                         ItemSubGroup ON ItemGrade.CategoryID = ItemSubGroup.CategoryID INNER JOIN
            //                         ItemGroup ON ItemSubGroup.GroupID = ItemGroup.GroupSrNo
            //WHERE(ItemGroup.DepreciationType = '2') OR
            //                 (ItemGroup.DepreciationType = '1') AND InDate <='"+ endDate111 + "'");
            //decimal depdeduc1 = (Convert.ToDecimal(transferred11)*Convert.ToDecimal(deprate)/100M);
            //decimal depdeduc2 = (Convert.ToDecimal(transferred22)*Convert.ToDecimal(deprate) /100M);
            //string dep1 = SQLQuery.ReturnString("SELECT DepreciationAmount FROM Depreciation WHERE DepDate >= '" + endDate111 + "' AND DepDate <= '" + endDate222 + "'");
            //string dep2 = SQLQuery.ReturnString("SELECT DepreciationAmount FROM Depreciation WHERE DepDate >= '" + endDate333 + "' AND DepDate <= '" + endDate444 + "'");

            //SQLQuery.ExecNonQry("Insert INTO CashFlow (ID, AccGroup, SubGroup, AccHeadName, Year1Balance, Year2Balance, ShowLine) VALUES ('03','Cash Flows from Investing activities','Investment','Investment','0','0', '2')");
            //SQLQuery.ExecNonQry("Insert INTO CashFlow (ID, AccGroup, SubGroup, AccHeadName, Year1Balance, Year2Balance, ShowLine) VALUES ('04','Cash Flows from Investing activities','Depreciation','Depreciation','" + dep1 + "','" + dep2 + "', '2')");
            //SQLQuery.ExecNonQry("UPDATE [dbo].[CashFlow]   SET [AccHeadName] ='Profit on '+ AccHeadName WHERE Year1Balance< Year2Balance AND AccGroup='Non-Current Assets'");
            //SQLQuery.ExecNonQry("UPDATE [dbo].[CashFlow]   SET [AccHeadName] ='Loss on '+ AccHeadName WHERE Year1Balance> Year2Balance AND AccGroup='Non-Current Assets'");
            //SQLQuery.ExecNonQry("UPDATE [dbo].[CashFlow]   SET [AccHeadName] ='Increase in '+ AccHeadName WHERE Year1Balance < Year2Balance AND AccHeadName<>'Net Profit for the Year'");
            //SQLQuery.ExecNonQry("UPDATE [dbo].[CashFlow]   SET [AccHeadName] ='Decrease in '+ AccHeadName WHERE Year1Balance > Year2Balance AND AccHeadName<>'Net Profit for the Year'");


            search(opDate1, clDate2, opDate3, clDate4);
            //SQLQuery.ExecNonQry(
            //    "UPDATE [dbo].[CashFlow]   SET [AccHeadName] ='Increase in '+ AccHeadName WHERE (Year1Balance > ('0') OR Year2Balance > ('0')) AND AccGroup<>('Cash Flows from Operating Activities') AND AccGroup<>('Cash Flows from Investing activities')");
            //SQLQuery.ExecNonQry(
            //    "UPDATE [dbo].[CashFlow]   SET [AccHeadName] ='Decrease in '+ AccHeadName WHERE (Year1Balance < ('0') OR Year2Balance < ('0')) AND AccGroup<>('Cash Flows from Operating Activities') AND AccGroup<>('Cash Flows from Investing activities')");

            DataTable dtx =
                RunQuery.SQLQuery.ReturnDataTable(
                    @"SELECT AccGroup, AccHeadName, Year1Balance, Year2Balance FROM CashFlow");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.CashFlow);


            rpt.Load(Server.MapPath("CrptCashFlow.rpt"));

            string datefield = "For the year ended " + Convert.ToDateTime(endDate2).ToString("MMMM dd,yyyy");
            string op = Convert.ToDateTime(endDate1).ToString("dd.MM.yyyy");
            string cl = Convert.ToDateTime(endDate2).ToString("dd.MM.yyyy");
            //string datefield = "opDate " + Convert.ToDateTime(opDate).ToString(" dd/MM/yyyy") + " clDate " + Convert.ToDateTime(clDate).ToString(" dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx);
            SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@y1", op);
            rpt.SetParameterValue("@y2", cl);
            //rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;


        }

        private void search2()
        {
            SQLQuery.ExecNonQry("Delete CashFlow");
        }



        private static void InsertPL(string id, string group, string subGrp, string head, string dr, string cr,
            string ShowLine)
        {
            if (Convert.ToDecimal(cr.Replace(",","")) > Convert.ToDecimal(dr.Replace(",", "")))
            {
                head = "Increase in " + head;
            }
            else if (Convert.ToDecimal(cr.Replace(",", "")) < Convert.ToDecimal(dr.Replace(",", "")))
            {
                head = "Decrease in " + head;
            }

            if (Convert.ToDecimal(cr.Replace(",", "")) != Convert.ToDecimal(dr.Replace(",", "")))
            {
                SQLQuery.ExecNonQry(
                    "Insert INTO CashFlow (ID, AccGroup, SubGroup, AccHeadName, Year1Balance, Year2Balance, ShowLine) VALUES ('" +
                    id + "','" + group + "','" + subGrp + "','" + head + "','" + dr + "', '" + cr + "', '" + ShowLine +
                    "')");
            }
        }

        private string search(string y1, string y2, string y3, string y4)
        {
            try
            {
                //SQLQuery.ExecNonQry("Delete CashFlow");

                string amt1x = "",
                    amt1y = "",
                    ControlAccountsID = "",
                    ControlAccountsName = "",
                    amt2x = "",
                    amt2y = "",
                    amt3x = "",
                    amt3y = "",
                    amt4x = "",
                    amt4y = "",
                    amt5 = "",
                    Maindecks = "",
                    UpperLevelp = "",
                    UpperLevels = "",
                    Quarter = "",
                    mgo = "",
                    tank = "",
                    FPK1 = "",
                    FPK2 = "",
                    FPK3 = "",
                    APK1 = "",
                    APK2 = "",
                    APK3 = "",
                    Chainlocker = "",
                    DB1 = "",
                    DB2 = "",
                    DB3 = "",
                    DB4 = "",
                    ProjectID = "",
                    FMean = "",
                    AMean = "",
                    FAMean = "",
                    MidMean = "",
                    MeanOfMean = "",
                    QuarterMean = "",
                    MainMean = "",
                    UpperMean = "",
                    Density = "",
                    Qty = "";

                decimal sumAmt1x = 0, sumAmt1y = 0, sumAmt2x = 0, sumAmt2y = 0;

                DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT DISTINCT
Yearly_PL2.AccGroup, Yearly_PL2.SubGroup, Yearly_PL2.AccHeadID,Yearly_PL2.AccHeadName
FROM            Yearly_PL2 INNER JOIN
                         ControlAccount ON Yearly_PL2.AccHeadID = ControlAccount.ControlAccountsID   WHERE (ControlAccount.CashFlowGroup = 'Adjustment to reconcile net income to net cash') AND
(Yearly_PL2.Report = 'BL') AND (Yearly_PL2.FinYear='" +
                                                         y1 + "' OR Yearly_PL2.FinYear='" + y2 +
                                                         "' OR Yearly_PL2.FinYear='" + y3 +
                                                         "' OR Yearly_PL2.FinYear='" + y4 + "')");

                foreach (DataRow drx in dtx.Rows)
                {
                    string AccGroup = drx["AccGroup"].ToString();
                    string headId = drx["AccHeadID"].ToString();
                    string headName = drx["AccHeadName"].ToString();
                    string id = SQLQuery.ReturnString("SELECT MIN(ID) FROM Yearly_PL2 WHERE AccHeadID='" + headId +
                                                      "' AND  AccHeadName='" + headName + "'  AND Report='BL'");

                    amt1x = SumControlByYear(headId, headName, y1);
                    amt1y = SumControlByYear(headId, headName, y2);
                    amt2x = SumControlByYear(headId, headName, y3);
                    amt2y = SumControlByYear(headId, headName, y4);
                    decimal amty1 = (Convert.ToDecimal(amt1y) - Convert.ToDecimal(amt1x));
                    decimal amty2 = (Convert.ToDecimal(amt2y) - Convert.ToDecimal(amt2x));

                    InsertPL(id, "Adjustment to reconcile net income to net cash", headName, headName,
                        Convert.ToString(amty1), Convert.ToString(amty2), "2");
                    sumAmt1x += amty1;
                    sumAmt1y += amty2;

                }

                //if ((sumAmt1x > 0) || (sumAmt1y > 0))
                //{
                    InsertPL("", "", "", "Net cash provided by Operating activities", Convert.ToString(sumAmt1x),
                        Convert.ToString(sumAmt1y), "2");
                //}

                decimal suma1 = sumAmt1x;
                decimal suma2 = sumAmt1y;

                decimal sumAmt1xx = 0;
                decimal sumAmt1yy = 0;

                DataTable dt = SQLQuery.ReturnDataTable(@"SELECT DISTINCT
Yearly_PL2.AccGroup, Yearly_PL2.SubGroup, Yearly_PL2.AccHeadID,Yearly_PL2.AccHeadName
FROM            Yearly_PL2 INNER JOIN
                         ControlAccount ON Yearly_PL2.AccHeadID = ControlAccount.ControlAccountsID   WHERE (ControlAccount.CashFlowGroup = 'Cash flow from Investing activities') AND
(Yearly_PL2.Report = 'BL') AND (Yearly_PL2.FinYear='" +
                                                        y1 + "' OR Yearly_PL2.FinYear='" + y2 +
                                                        "' OR Yearly_PL2.FinYear='" + y3 +
                                                        "' OR Yearly_PL2.FinYear='" + y4 + "')");

                foreach (DataRow drx in dt.Rows)
                {
                    string AccGroup = drx["AccGroup"].ToString();
                    string headId = drx["AccHeadID"].ToString();
                    string headName = drx["AccHeadName"].ToString();
                    string id = SQLQuery.ReturnString("SELECT MIN(ID) FROM Yearly_PL2 WHERE AccHeadID='" + headId +
                                                      "' AND  AccHeadName='" + headName + "'  AND Report='BL'");

                    amt1x = SumControlByYear(headId, headName, y1);
                    amt1y = SumControlByYear(headId, headName, y2);
                    amt2x = SumControlByYear(headId, headName, y3);
                    amt2y = SumControlByYear(headId, headName, y4);
                    decimal amty1 = (Convert.ToDecimal(amt1y) - Convert.ToDecimal(amt1x));
                    decimal amty2 = (Convert.ToDecimal(amt2y) - Convert.ToDecimal(amt2x));

                    InsertPL(id, "Cash flow from Investing activities", headName, headName, Convert.ToString(amty1),
                        Convert.ToString(amty2), "2");
                    sumAmt1xx += amty1;
                    sumAmt1yy += amty2;
                }

               // if ((sumAmt1x > 0) || (sumAmt1y > 0))
               // {
                    InsertPL("", "", "", "Net cash used in  Investing activities", Convert.ToString(sumAmt1xx), Convert.ToString(sumAmt1yy), "2");
               // }

                decimal sumb1 = sumAmt1x;
                decimal sumb2 = sumAmt1y;
                decimal sumAmta1x = 0;
                decimal sumAmtb1y = 0;
                DataTable dt2 = SQLQuery.ReturnDataTable(@"SELECT DISTINCT
Yearly_PL2.AccGroup, Yearly_PL2.SubGroup, Yearly_PL2.AccHeadID,Yearly_PL2.AccHeadName
FROM            Yearly_PL2 INNER JOIN
                         ControlAccount ON Yearly_PL2.AccHeadID = ControlAccount.ControlAccountsID   WHERE (ControlAccount.CashFlowGroup = 'Cash flow from Financing activities') AND
(Yearly_PL2.Report = 'BL') AND (Yearly_PL2.FinYear='" + y1 + "' OR Yearly_PL2.FinYear='" + y2 + "' OR Yearly_PL2.FinYear='" +
                                                         y3 +
                                                         "' OR Yearly_PL2.FinYear='" + y4 + "')");

                foreach (DataRow drx in dt2.Rows)
                {
                    string AccGroup = drx["AccGroup"].ToString();
                    string headId = drx["AccHeadID"].ToString();
                    string headName = drx["AccHeadName"].ToString();
                    string id = SQLQuery.ReturnString("SELECT MIN(ID) FROM Yearly_PL2 WHERE AccHeadID='" + headId +
                                                      "' AND  AccHeadName='" + headName + "'  AND Report='BL'");

                    amt1x = SumControlByYear(headId, headName, y1);
                    amt1y = SumControlByYear(headId, headName, y2);
                    amt2x = SumControlByYear(headId, headName, y3);
                    amt2y = SumControlByYear(headId, headName, y4);
                    decimal amty1 = (Convert.ToDecimal(amt1y) - Convert.ToDecimal(amt1x));
                    decimal amty2 = (Convert.ToDecimal(amt2y) - Convert.ToDecimal(amt2x));

                    InsertPL(id, "Cash flow from Financing activities", headName, headName, Convert.ToString(amty1),
                        Convert.ToString(amty2), "2"); 
                    sumAmta1x += amty1;
                    sumAmtb1y += amty2;
                }

               // if ((sumAmt1x > 0) || (sumAmt1y > 0))
               // {


                    InsertPL("", "", "", "Net cash used by  Financing activities", Convert.ToString(sumAmta1x), Convert.ToString(sumAmtb1y), "2");
               // }

                decimal sumc1 = sumAmta1x;
                decimal sumc2 = sumAmtb1y;
                decimal sumAmt11x = 0;
                decimal sumAmt12y = 0;

//                DataTable dt3 = SQLQuery.ReturnDataTable(@"SELECT DISTINCT Yearly_PL2.AccGroup, Yearly_PL2.SubGroup, Yearly_PL2.AccHeadID,Yearly_PL2.AccHeadName
//FROM            Yearly_PL2 INNER JOIN    ControlAccount ON Yearly_PL2.AccHeadID = ControlAccount.ControlAccountsID   WHERE (Yearly_PL2.AccHeadName = 'Cash & Cash equivalent') AND (Yearly_PL2.Report = 'BL')");

//                foreach (DataRow drx in dt2.Rows)
//                {
//                    string AccGroup = drx["AccGroup"].ToString();
//                    string headId = "010101";
//                    string headName = "Cash & Cash equivalent";
//                    string id = SQLQuery.ReturnString("SELECT MIN(ID) FROM Yearly_PL2 WHERE AccHeadID='" + headId +
//                                                      "' AND  AccHeadName='" + headName + "'  AND Report='BL'");

//                    amt1x = SumControlByYear(headId, headName, y1);
//                    amt1y = SumControlByYear(headId, headName, y2);
//                    amt2x = SumControlByYear(headId, headName, y3);
//                    amt2y = SumControlByYear(headId, headName, y4);

//                    decimal amty1 = (Convert.ToDecimal(amt1y) - Convert.ToDecimal(amt1x));
//                    decimal amty2 = (Convert.ToDecimal(amt2y) - Convert.ToDecimal(amt2x));

                   
//                    sumAmt11x += amty1;
//                    sumAmt12y += amty2;
//                }
                string val1 = SumControlByYear("010101", "Cash & Cash equivalent", y1);
                string val2 = SumControlByYear("010101", "Cash & Cash equivalent", y2);
                string val3 = SumControlByYear("010101", "Cash & Cash equivalent", y3);
                string val4 = SumControlByYear("010101", "Cash & Cash equivalent", y4);

                //string val2 = SQLQuery.ReturnString("SELECT ISNULL(([BalanceDr]),0) FROM Yearly_PL2 WHERE AccHeadID='010101' AND  AccHeadName='Cash & Cash equivalent' AND  FinYear='" + y2 + "' AND Report='BL'");
                

                //string val3 = SQLQuery.ReturnString("SELECT ISNULL(([BalanceDr]),0) FROM Yearly_PL2 WHERE AccHeadID='010101' AND  AccHeadName='Cash & Cash equivalent' AND  FinYear='" + y3 + "' AND Report='BL'");

               

                //string val4 = SQLQuery.ReturnString("SELECT ISNULL(([BalanceDr]),0) FROM Yearly_PL2 WHERE AccHeadID='010101' AND  AccHeadName='Cash & Cash equivalent' AND  FinYear='" + y4 + "' AND Report='BL'");
                
                
                //SQLQuery.ExecNonQry();
                decimal amty111 = (Convert.ToDecimal(val2) - Convert.ToDecimal(val1));
                decimal amty222 = (Convert.ToDecimal(val4) - Convert.ToDecimal(val3));

                InsertPL("", "", "", "Cash & cash equivalent increases during the year", Convert.ToString(amty111), Convert.ToString(amty222), "2");

                InsertPL("", "", "", "Add: Cash & cash equivalent at beginning of the year", Convert.ToString((Convert.ToDecimal(val1))), Convert.ToString((Convert.ToDecimal(val3))), "2");

               
                InsertPL("", "", "", "Cash & cash equivalent closing of the year", Convert.ToString(Convert.ToDecimal(val2)), Convert.ToString(Convert.ToDecimal(val4)), "2");



                return "";


            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
                //lblMsg.Attributes.Add("class", "xerp_error");
                //lblMsg.Text = "ERROR: " + ex.ToString();
                //DataTable dt = null;
                return ex.ToString();
            }

        }
    

    private string Getequsition(string date1, string date2)
        {
            string val = SQLQuery.ReturnString(@"SELECT SUM(FixedAssets.PurchaseCost) AS PurchaseAmount FROM            Categories INNER JOIN
                         FixedAssets INNER JOIN
                         Products ON FixedAssets.ProductID = Products.ProductID ON Categories.CategoryID = Products.CategoryID INNER JOIN
                         ItemGrade ON Categories.GradeID = ItemGrade.GradeID INNER JOIN
                         ItemSubGroup ON ItemGrade.CategoryID = ItemSubGroup.CategoryID INNER JOIN
                         ItemGroup ON ItemSubGroup.GroupID = ItemGroup.GroupSrNo

WHERE (InDate>='" + date1 + @"') AND (InDate<='" + date2 + @"') AND (ItemGroup.DepreciationType = '2') OR (ItemGroup.DepreciationType = '1')");


            if (val == "")
            {
                val = "0";
            }
            return val;
        }
        private string SumControlByYear(string cid, string name, string year)
        {
            string val= SQLQuery.ReturnString("SELECT BalanceDr FROM Yearly_PL2 WHERE AccHeadID='" + cid + "' AND  AccHeadName='" + name + "' AND  FinYear='" + year + "' AND Report='BL'");
            if (val == "")
            {
                val = "0";
            }
            return val;
        }

        //    private decimal Getinvestment(string year1, string year2)
        //    {
        //        decimal a = 0;
        //    DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT Distinct (Select Company from Party where partyid=Transactions.HeadID) as Customer,
        //(Select OpBalance from Party where partyid=Transactions.HeadID)+ isnull(sum(Dr),0)-isnull(sum(Cr),0) as Balance FROM[Transactions]
        //                 where TrType = 'Supplier'   AND(TrDate >= '" + year1 + @"') AND(TrDate <= '" + year2 + @"')  group by headid having((Select OpBalance from Party where partyid= Transactions.HeadID)+ isnull(sum(Dr),0)-isnull(sum(Cr),0))<>0");
        //        foreach (DataRow drx in dtx.Rows)
        //        {
        //            string customer = drx["Customer"].ToString();
        //            string total = drx["Balance"].ToString();
                    
        //            a += Convert.ToDecimal(total);

        //        }

        //        return a;
        //    }

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