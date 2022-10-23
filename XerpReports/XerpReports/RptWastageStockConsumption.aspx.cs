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
    public partial class RptWastageStockConsumption : System.Web.UI.Page
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
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT Id, SLNo, GroupName, Description, Warehouse, Purpose, Grade, Category, ProductName, Weight FROM RptProduction ORDER BY Id ");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.RptProduction);

            rpt.Load(Server.MapPath("CrptWastageStockConsumption.rpt"));
            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");

            //string datefield = "As on :" + Convert.ToDateTime(dateTo).ToString(" dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            //rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;

        }

        private static void InsertRptProduction(string sLNo, string groupName, string description, string Warehouse, string Purpose, string Grade, string Category, string ProductName, string Weight)
        {
            SQLQuery.ExecNonQry("Insert INTO RptProduction (SLNo, GroupName, Description, Warehouse, Purpose, Grade, Category, ProductName, Weight)" +
                                " VALUES ('" + sLNo + "','" + groupName + "','" + description + "','" + Warehouse + "','" + Purpose + "','" + Grade + "','" + Category + "','" + ProductName + "','" + Weight + "')");
        }

        private decimal GetEquity(string rtype, string dateFrom, string dateTo)
        {
            //string dateFrom = Convert.ToDateTime(txtdateTo.Text).AddMonths(-1).ToString("yyyy-MM") + "-01";
            //string dateTo = Convert.ToDateTime(Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM") + "-01").AddDays(-1).ToString("yyyy-MM-dd");

            //string amt1 = "", ControlAccountsID = "", ControlAccountsName = "", amt2 = "", amt3 = "", amt4 = "", amt5 = "", Maindecks = "", UpperLevelp = "", UpperLevels = "", Quarter = "", mgo = "", tank = "", FPK1 = "", FPK2 = "", FPK3 = "", APK1 = "", APK2 = "", APK3 = "", Chainlocker = "", DB1 = "", DB2 = "", DB3 = "", DB4 = "", ProjectID = "", FMean = "", AMean = "", FAMean = "", MidMean = "", MeanOfMean = "", QuarterMean = "", MainMean = "", UpperMean = "", Density = "", Qty = "";
            string sLNo = "", groupName = "", description = "", Warehouse = "", Purpose = "", Grade = "", Category = "", ProductName = "";
            decimal sumAmt1 = 0, sumAmt2 = 0, Weight = 0;
            //Opening Balance
            //        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
            //                                                (Select Purpose from Purpose where pid=Stock.Purpose) AS Purpose, (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade,
            //                                                (SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category,  (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName,   ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight  
            //                        FROM Stock where EntryId<>0  AND (WarehouseID = '8') AND  ProductID IN (Select ProductID from Products where CategoryID ='86')  AND EntryDate<'" + dateFrom + "' GROUP BY WarehouseID, Purpose, ProductID HAVING (ISNULL(SUM(InWeight), 0) - ISNULL(SUM(OutWeight), 0) <> 0)");
            //        foreach (DataRow drx in dtx.Rows)
            //        {
            //            Warehouse = drx["Warehouse"].ToString();
            //            Purpose = drx["Purpose"].ToString();
            //            Grade = drx["Grade"].ToString();
            //            Category = drx["Category"].ToString();
            //            ProductName = drx["ProductName"].ToString();
            //            Weight += Convert.ToDecimal(drx["Weight"].ToString());
            //        }
            //        InsertRptProduction(sLNo, "Opening Balance", description, Warehouse, Purpose, Grade, Category, "Production Floor (PPCP mixture)", Weight.ToString());
            //        Weight = 0;

            //        dtx = SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
            //            (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade,
            //(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category,  (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName,
            //ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty,  ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight  FROM Stock where EntryId<>0  AND ProductID IN (Select ProductID from Products where CategoryID ='1411')  AND EntryDate<='" + dateFrom + "' GROUP BY WarehouseID, ProductID HAVING (ISNULL(SUM(InWeight), 0) - ISNULL(SUM(OutWeight), 0) <> 0)");
            //        foreach (DataRow drx in dtx.Rows)
            //        {
            //            Warehouse = drx["Warehouse"].ToString();
            //            Purpose = "";
            //            Grade = drx["Grade"].ToString();
            //            Category = drx["Category"].ToString();
            //            ProductName = drx["ProductName"].ToString();
            //            Weight += Convert.ToDecimal(drx["Weight"].ToString());

            //        }
            //        InsertRptProduction(sLNo, "Opening Balance", description, Warehouse, Purpose, Grade, Category, "Usuable Wastage (PPCP)", Weight.ToString());
            //        Weight = 0;
            //        //Current Month Addition/Issued
            //        dtx = SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
            //                                                (Select Purpose from Purpose where pid=Stock.Purpose) AS Purpose, (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade,
            //                                                (SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category,  (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName, ISNULL(SUM(InWeight),0) AS Weight  
            //                        FROM Stock where EntryId<>0  AND (WarehouseID = '8') AND  ProductID IN (Select ProductID from Products where CategoryID ='86')  AND EntryDate>='" + dateFrom + "'  AND  EntryDate<='" + dateTo + "' GROUP BY WarehouseID, Purpose, ProductID HAVING (ISNULL(SUM(InWeight), 0) - ISNULL(SUM(OutWeight), 0) <> 0)");
            //        foreach (DataRow drx in dtx.Rows)
            //        {
            //            Warehouse = drx["Warehouse"].ToString();
            //            Purpose = drx["Purpose"].ToString();
            //            Grade = drx["Grade"].ToString();
            //            Category = drx["Category"].ToString();
            //            ProductName = drx["ProductName"].ToString();
            //            Weight += Convert.ToDecimal(drx["Weight"].ToString());
            //        }
            //        InsertRptProduction(sLNo, "Current Month Addition/Issued", description, Warehouse, Purpose, Grade, Category, "Store Issue (PPCP)", Weight.ToString());

            //Color Wise PPCP Reusable Wastage
            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
                (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade,
				(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category,  (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName,
				ISNULL(SUM(InWeight),0) AS Weight  FROM Stock where EntryId<>0 AND (WarehouseID = '8') AND ProductID IN (Select ProductID from Products where CategoryID ='1411')  AND EntryDate>='" + dateFrom + "'  AND  EntryDate<='" + dateTo + "' GROUP BY WarehouseID, ProductID HAVING (ISNULL(SUM(InWeight), 0) <> 0)");
            foreach (DataRow drx in dtx.Rows)
            {
                Warehouse = drx["Warehouse"].ToString();
                Purpose = "";
                Grade = drx["Grade"].ToString();
                Category = drx["Category"].ToString();
                ProductName = drx["ProductName"].ToString();
                Weight = Convert.ToDecimal(drx["Weight"].ToString());

                InsertRptProduction(sLNo, "Current Month Addition/Issued", description, Warehouse, Purpose, Grade, Category, ProductName, Weight.ToString());

            }

            //Color Wise Master Batch [Injection grade only]
            /*dtx = SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
                (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade,
				(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category,  (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName,
				ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight  FROM Stock where EntryId<>0 AND (WarehouseID = '8') AND ProductID IN (Select ProductID from Products where CategoryID ='1411')  AND EntryDate>='" + dateFrom + "'  AND  EntryDate<='" + dateTo + "' GROUP BY WarehouseID, ProductID HAVING (ISNULL(SUM(InWeight), 0) - ISNULL(SUM(OutWeight), 0) <> 0)");*/

            //dtx = SQLQuery.ReturnDataTable(@"SELECT CategoryID, CategoryName FROM Categories WHERE (GradeID = '34')");
            //foreach (DataRow drx in dtx.Rows)
            //{
            //    Warehouse = "";
            //    Purpose = "";
            //    Grade = "";
            //    Category = drx["CategoryID"].ToString();
            //    ProductName = drx["CategoryName"].ToString();
            //    Weight = Convert.ToDecimal(SQLQuery.ReturnString(@"Select ISNULL(SUM(InWeight), 0) FROM Stock
            //            WHERE (EntryID <> 0) AND (WarehouseID = '8') AND (ProductID IN
            //                 (SELECT ProductID FROM Products WHERE (CategoryID = '" + drx["CategoryID"].ToString() + "'))) AND (EntryDate >= '" + dateFrom + "') AND (EntryDate <= '" + dateTo + "')"));
            //    if (Weight != 0)
            //    {
            //        InsertRptProduction(sLNo, "Current Month Addition/Issued", description, Warehouse, Purpose, Grade, Category, ProductName, Weight.ToString());
            //    }
            //}
            //Gross profit
            decimal gp = 0;
            decimal gpRate = 0;
            if (sumAmt1 > 0)
            {
                gp = sumAmt1 - sumAmt2;
                gpRate = gp / sumAmt1 * 100M;
            }

            //Total Raw Material:
            decimal sumAmt3 = 0, sumAmt4 = 0;

            //dtx = SQLQuery.ReturnDataTable(@"SELECT ISNULL(SUM(Weight),0) AS Weight FROM rptProduction WHERE GroupName='Opening Balance' OR GroupName='Current Month Addition/Issued'");
            //foreach (DataRow drx in dtx.Rows)
            //{
            //    Warehouse = "Total Raw Material";
            //    Purpose = "";
            //    Grade = "";
            //    Category = "";
            //    ProductName = "(Opening Balance+Current Month Addition)";
            //    Weight = Convert.ToDecimal(drx["Weight"].ToString());

            //    InsertRptProduction(sLNo, "Total Raw Material", description, Warehouse, Purpose, Grade, Category, ProductName, Weight.ToString());

            //}

            //Consumption/ Production PPCP only
            decimal op = gp - sumAmt3;

            //dtx = SQLQuery.ReturnDataTable(@"SELECT DISTINCT ItemID, (SELECT ItemName FROM Products WHERE ProductID=PrdPlasticCont.ItemID) as ItemName, (SELECT GradeName FROM ItemGrade WHERE GradeID=PrdPlasticCont.Grade) AS Grade, (SELECT CategoryName FROM Categories WHERE CategoryID='295') AS Category
            //                               FROM PrdPlasticCont WHERE ItemID IN (Select ProductID from Products WHERE CategoryID='295') ORDER BY ItemID");
            //foreach (DataRow drx in dtx.Rows)
            //{
            //    Warehouse = "";
            //    string itemID = drx["ItemID"].ToString();
            //    Purpose = "";
            //    Grade = drx["Grade"].ToString();
            //    Category = drx["Category"].ToString();
            //    ProductName = drx["ItemName"].ToString();
            //    Weight = Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(ConsumedWeight),0) from PrdPlasticContDetails where PrdnID IN (Select ProductionID from PrdPlasticCont WHERE ItemID='" + itemID + "'  AND Date>='" + dateFrom + "'  AND  Date<='" + dateTo + "') "));
            //    if (Convert.ToDecimal(Weight) > 0)
            //    {
            //        InsertRptProduction(sLNo, "Consumption/ Production PPCP only", description, Warehouse, Purpose, Grade, Category, ProductName, Weight.ToString());
            //    }
            //}

            ////Consumption/ Production PPCP only
            //string ttlIssue = SQLQuery.ReturnString("SELECT ISNULL(SUM(Weight),0) AS Weight FROM rptProduction WHERE GroupName = 'Opening Balance' OR GroupName = 'Current Month Addition/Issued'");
            //string ttlCons = SQLQuery.ReturnString("SELECT ISNULL(SUM(Weight), 0) AS Weight FROM rptProduction WHERE GroupName = 'Consumption/ Production PPCP only'");
            //dtx = SQLQuery.ReturnDataTable(@"SELECT ISNULL(SUM(Weight),0) AS Weight FROM rptProduction WHERE GroupName = 'Opening Balance' OR GroupName = 'Current Month Addition/Issued'");
            //foreach (DataRow drx in dtx.Rows)
            //{
            //    Warehouse = "Material Remaining at Production Floor";
            //    Purpose = "";
            //    Grade = "";
            //    Category = "";
            //    ProductName = "PPCP mixture add Floor";
            //    Weight = Convert.ToDecimal(ttlIssue) - Convert.ToDecimal(ttlCons);

            //    InsertRptProduction(sLNo, "Material Remaining at Production Floor", description, Warehouse, Purpose, Grade, Category, ProductName, Weight.ToString());

            //}

            ////Actual Material Remaining at Production Floor

            //dtx = SQLQuery.ReturnDataTable(@"SELECT ISNULL(SUM(Weight),0) AS Weight FROM rptProduction WHERE GroupName = 'Opening Balance' OR GroupName = 'Current Month Addition/Issued'");
            //foreach (DataRow drx in dtx.Rows)
            //{
            //    Warehouse = "Actual Material Remaining at Production Floor";
            //    Purpose = "";
            //    Grade = "";
            //    Category = "";
            //    ProductName = "Actual PPCP mixture add Floor found during Inventory";
            //    Weight = 0;
            //    InsertRptProduction(sLNo, "Actual Material Remaining at Production Floor", description, Warehouse, Purpose, Grade, Category, ProductName, Weight.ToString());

            //}
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

            //if (btype == "Dr")
            //{
            //    returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0) - ISNULL(SUM(OpBalCr),0) FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
            //    returnValue += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
            //            cid + "'))) and ISApproved ='A' and  EntryDate<='" + dateTo + "'"));
            //}
            //else
            //{
            //    returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0) - ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
            //    returnValue += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0) - ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "'))) and ISApproved ='A' and  EntryDate<='" + dateTo + "'"));
            //}
            return returnValue.ToString();
        }

        private string search(string dateFrom, string dateTo)
        {
            try
            {
                SQLQuery.ExecNonQry("Delete RptProduction");
                //string acGroupPrev = "", acSubPrev = "", acControlPrev = "", acHeadPrev = "";
                //string acGroup = "", acSub = "", acControl = "", acHead = "";
                //string acGroupTxt = "", acSubTxt = "", acControlTxt = "", acHeadTxt = "";
                //decimal grandTotalDr = 0; decimal grandTotalCr = 0;

                //DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT sl, GroupID, GroupName, ProjectID FROM AccountGroup Order by sl");

                //foreach (DataRow drx in dtx.Rows)
                //{
                //    decimal totalDr = 0; decimal totalCr = 0;
                //    string acGroupID = drx["GroupID"].ToString();
                //    acGroup = drx["GroupName"].ToString();

                //    int subCount = Convert.ToInt32(SQLQuery.ReturnString(@"SELECT COUNT(AccountsID) FROM Accounts WHERE GroupID='" + acGroupID + "'"));
                //    DataTable dtx2 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT sl, GroupID, AccountsID, AccountsName, ProjectID FROM Accounts WHERE GroupID='" + acGroupID + "' Order by sl");

                //    foreach (DataRow drx2 in dtx2.Rows)
                //    {
                //        string acSubID = drx2["AccountsID"].ToString();
                //        acSub = drx2["AccountsName"].ToString();

                //        DataTable dtx3 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT sl, ControlAccountsID, ControlAccountsName, ProjectID FROM ControlAccount  WHERE AccountsID='" + acSubID + "'  Order by sl");
                //        foreach (DataRow drx3 in dtx3.Rows)
                //        {
                //            string acControlID = drx3["ControlAccountsID"].ToString();
                //            acControl = drx3["ControlAccountsName"].ToString();

                //            decimal balDr = Convert.ToDecimal(ControlBalance(acControlID, "Dr", dateFrom, dateTo));
                //            decimal balCr = Convert.ToDecimal(ControlBalance(acControlID, "Cr", dateFrom, dateTo));

                //            if (balDr > balCr)
                //            {
                //                balDr = balDr - balCr;
                //                balCr = 0;
                //            }
                //            else if (balCr > balDr)
                //            {
                //                balCr = balCr - balDr;
                //                balDr = 0;
                //            }

                //            if (acControl == "Retained Earnings")
                //            {
                //                decimal transferred = Convert.ToDecimal(GetEquity("transferred", dateFrom, dateTo));
                //                if (transferred < 0)
                //                {
                //                    transferred = Convert.ToDecimal(GetEquity("npbp", dateFrom, dateTo));
                //                }

                //                balCr = balCr + transferred;
                //            }
                //            else if (acControl == "General reserve")
                //            {
                //                balCr = Convert.ToDecimal(GetEquity("gr", dateFrom, dateTo));
                //            }

                //            //InsertRptProduction(sLNo, groupName, description, Warehouse, Purpose, Grade, Category, ProductName, qty.ToString());


                //            /*
                //            DataTable dtx4 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT EntryID, AccountsHeadID, AccountsHeadName, OpBalDr, OpBalCr, ProjectID FROM HeadSetup WHERE ControlAccountsID='" + acControlID + "'  Order by ControlAccountsID");

                //            foreach (DataRow drx4 in dtx4.Rows)
                //            {
                //                string acHeadID = drx4["AccountsHeadID"].ToString();
                //                acHead = drx4["AccountsHeadName"].ToString();
                                
                //                decimal balDr = Convert.ToDecimal(HeadBalance(acHeadID, "Dr", dateTo));
                //                decimal balCr = Convert.ToDecimal(HeadBalance(acHeadID, "Cr", dateTo));

                //                if (balDr > balCr)
                //                {
                //                    balDr = balDr - balCr;
                //                    balCr = 0;
                //                }
                //                else if (balCr > balDr)
                //                {
                //                    balCr = balCr - balDr;
                //                    balDr = 0;
                //                }

                //                if (balDr > 0 || balCr > 0)
                //                {
                //                    //body += "<tr>" + comparePrevValue(acGroup, acGroupPrev) + comparePrevValue(acSub, acSubPrev) + comparePrevValue(acControl, acControlPrev) + "<td>" + acHeadID + "</td><td>" + acHead + "</td><td class='a-right'>" + SQLQuery.FormatBDNumber(balDr) + " </td><td>" + SQLQuery.FormatBDNumber(balCr) + "</td></tr>";
                //                    //InsertPL(acGroup, acSub, acControl, SQLQuery.FormatBDNumber(amt1), "0");

                //                    totalDr += Convert.ToDecimal(ControlBalance(acHeadID, "Dr", dateTo));
                //                    totalCr += Convert.ToDecimal(ControlBalance(acHeadID, "Cr", dateTo));
                //                }
                //                //}
                //                acGroupPrev = acGroup;
                //                acSubPrev = acSub;
                //                acControlPrev = acControl;

                //            }
                //            */

                //            acControlPrev = acControl;
                //        }
                //        acSubPrev = acSub;
                //    }
                //    acGroupPrev = acGroup;
                //    //body += "<tr style='background-color: #D3D3D3;border-bottom: 6px solid #0071C8;'><td></td><td></td><td></td><td></td><td class='a-right'><b>Total " + acGroup + ":</b></td><td class='a-right'>" + SQLQuery.FormatBDNumber(totalDr) + " </td><td class='a-right'>" + SQLQuery.FormatBDNumber(totalCr) + "</td></tr>";
                //    grandTotalDr += totalDr; grandTotalCr += totalCr;
                //}
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

        //private string ControlBalance(string cid, string btype, string dateFrom, string dateTo)
        //{
        //    decimal returnValue = 0;

        //    if (btype == "Dr")
        //    {
        //        returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
        //        returnValue += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE AccountsHeadID IN (Select AccountsHeadID from HeadSetup WHERE ControlAccountsID = '" + cid + "') and ISApproved ='A' and EntryDate<='" + dateTo + "'"));
        //    }
        //    else
        //    {
        //        returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0)  FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
        //        returnValue += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0)  FROM VoucherDetails WHERE AccountsHeadID IN (Select AccountsHeadID from HeadSetup WHERE ControlAccountsID = '" + cid + "') and ISApproved ='A' and EntryDate<='" + dateTo + "'"));
        //    }
        //    return returnValue.ToString();
        //}

        //private string SumByControl(string cid, string btype, string dateTo)
        //{
        //    if (btype == "Dr")
        //    {
        //        return
        //            SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "'))) and ISApproved ='A' and EntryDate<='" + dateTo + "'");
        //    }
        //    else
        //    {
        //        return
        //            SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0) - ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "'))) and ISApproved ='A' and EntryDate<='" + dateTo + "'");
        //    }
        //}

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