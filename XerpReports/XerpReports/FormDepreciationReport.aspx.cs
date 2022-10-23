using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using RunQuery;

namespace Oxford.XerpReports
{
    public partial class FormDepreciationReport : System.Web.UI.Page
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
        private int currdep;

        private void LoadGridData()
        {
            //decimal total2 = (decimal)0.0;
            //decimal qty2 = (decimal)0.0;
            //decimal vat2 = (decimal)0.0;
            //decimal TotalSales2 = (decimal)0.0;
            //decimal TotalWeight2 = (decimal)0.0;

            string lName = Page.User.Identity.Name.ToString();
            string prjID = "1";
            //string item = Convert.ToString(Request.QueryString["item"]);
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);
            string rpttype = Convert.ToString(Request.QueryString["rptype"]);

            if ( rpttype == "0" )
            {
                DataTable dtx = dataatble(dateFrom, dateTo);
                DataTableReader dr2 = dtx.CreateDataReader();

                XerpDataSet dsx = new XerpDataSet();
                dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.DepReport);

                rpt.Load(Server.MapPath("CrptDepRp.rpt"));
                //string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
                string rptName = SQLQuery.ReturnString("SELECT [AccountsHeadName] FROM [HeadSetup]");
                //string rptControl = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID=(Select ControlAccountsID  FROM [HeadSetup] WHERE  AccountsHeadID='" + item + "')");
                string datefield = "As at " + Convert.ToDateTime(dateTo).ToString("MMMM dd,yyyy");
                string opdate = "As at " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy");
                string cldate = "As at " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
                rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
                rpt.SetParameterValue("@date", datefield);
                rpt.SetParameterValue("@opdate", opdate);
                rpt.SetParameterValue("@cldate", cldate);
                rpt.SetParameterValue("@rptName", rptName);
                //rpt.SetParameterValue("@rpCtName", rptControl);
                CrystalReportViewer1.ReportSource = rpt;
            }
            else
            {
                DataTable dtx = dataatble2(dateFrom, dateTo);
                DataTableReader dr2 = dtx.CreateDataReader();

                XerpDataSet dsx = new XerpDataSet();
                dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.DepReport);

                rpt.Load(Server.MapPath("CrptDepRp.rpt"));
                //string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
                string rptName = SQLQuery.ReturnString("SELECT [AccountsHeadName] FROM [HeadSetup]");
                //string rptControl = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID=(Select ControlAccountsID  FROM [HeadSetup] WHERE  AccountsHeadID='" + item + "')");
                string datefield = "As at " + Convert.ToDateTime(dateTo).ToString("MMMM dd,yyyy");
                string opdate = "As at " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy");
                string cldate = "As at " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
                rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
                rpt.SetParameterValue("@date", datefield);
                rpt.SetParameterValue("@opdate", opdate);
                rpt.SetParameterValue("@cldate", cldate);
                rpt.SetParameterValue("@rptName", rptName);
                //rpt.SetParameterValue("@rpCtName", rptControl);
                CrystalReportViewer1.ReportSource = rpt;
            }
             //RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");

            

            
        }

        

        private DataTable dataatble(string dtFrom, string dtTo)
        {

            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("Particulars", typeof(string)));
            dt1.Columns.Add(new DataColumn("CostOpBL", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("CostAddition", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("CostClBL", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("ABCD", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Rate", typeof(string)));

            dt1.Columns.Add(new DataColumn("DepOpBL", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("DepCharged", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("DepClBL", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("WDVBalance", typeof(decimal)));

            dt1.Columns.Add(new DataColumn("Dr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Cr", typeof(decimal)));
            //dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));


            decimal debt = 0; decimal credit = 0; decimal currBal = 0; currdep = 0;

            decimal TotalValueUSD = 0; decimal AssetCost = 0; decimal Totalvalue = 0;


            // SqlCommand cmd2 = new SqlCommand("SELECT [InDate], [ProductName],[Remark], SUM(Quantity) As Dr, SUM(DeliveredQty) As Cr FROM [FixedAssets] WHERE ([ProductID] = @HeadName) AND (InDate >= @DateTo) AND (OutDate <= @DateFrom) Group by [InDate], [ProductName], [Remark]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            DataTable dt2 = SQLQuery.ReturnDataTable(@"SELECT  DISTINCT Products.Depreciation,ItemGroup.GroupSrNo, ItemGroup.GroupName,ItemGroup.DepreciationType,ItemGroup.Depreciationvalue FROM            Categories INNER JOIN
                         FixedAssets INNER JOIN
                         Products ON FixedAssets.ProductID = Products.ProductID ON Categories.CategoryID = Products.CategoryID INNER JOIN
                         ItemGrade ON Categories.GradeID = ItemGrade.GradeID INNER JOIN
                         ItemSubGroup ON ItemGrade.CategoryID = ItemSubGroup.CategoryID INNER JOIN
                         ItemGroup ON ItemSubGroup.GroupID = ItemGroup.GroupSrNo WHERE  (ItemGroup.DepreciationType = '2') OR (ItemGroup.DepreciationType = '1')");

            //recordcount = dt2.Rows.Count;


            foreach (DataRow drow in dt2.Rows)
            {

                string groupId = drow["GroupSrNo"].ToString();

                decimal opAssetValue = 0, opDepr = 0, clAssetValue = 0, clDepr = 0;
                DataTable dt = SQLQuery.ReturnDataTable(@"SELECT        COUNT(FixedAssets.Id) AS QTY, FixedAssets.InDate, FixedAssets.ProductName,FixedAssets.ProductID, SUM(FixedAssets.PurchaseCost) AS PurchaseAmount, FixedAssets.ManufacturerName, FixedAssets.LCNoFTT, Products.Depreciation, 
                         Products.ItemName, ItemSubGroup.CategoryName, Categories.CategoryName AS Grade, ItemGrade.GradeName,ItemGroup.GroupSrNo, ItemGroup.GroupName,ItemGroup.Depreciationvalue, FixedAssets.Location, SUM(FixedAssets.TotalValueUSD) AS TotalUSD
FROM            Categories INNER JOIN
                         FixedAssets INNER JOIN
                         Products ON FixedAssets.ProductID = Products.ProductID ON Categories.CategoryID = Products.CategoryID INNER JOIN
                         ItemGrade ON Categories.GradeID = ItemGrade.GradeID INNER JOIN
                         ItemSubGroup ON ItemGrade.CategoryID = ItemSubGroup.CategoryID INNER JOIN
                         ItemGroup ON ItemSubGroup.GroupID = ItemGroup.GroupSrNo
 WHERE ItemGroup.GroupSrNo='" + groupId + "' AND (InDate>='" + dtFrom + @"')
GROUP BY FixedAssets.InDate, FixedAssets.ProductName,FixedAssets.ProductID, FixedAssets.ManufacturerName, FixedAssets.LCNoFTT, Products.Depreciation, Products.ItemName, ItemSubGroup.CategoryName, Categories.CategoryName, 
                         ItemGrade.GradeName,ItemGroup.GroupSrNo, ItemGroup.GroupName,ItemGroup.Depreciationvalue, FixedAssets.Location  ORDER BY ItemGroup.GroupSrNo");

                foreach (DataRow dr in dt.Rows)
                {
                    //decimal assetCost = Convert.ToDecimal(dr["PurchaseAmount"].ToString());
                    //DateTime date = Convert.ToDateTime(dr["InDate"].ToString());
                    //decimal depRate = Convert.ToDecimal(dr["Depreciationvalue"].ToString());
                    //DateTime past = Convert.ToDateTime(dtFrom);
                    //double daysGone = (DateTime.Now - date).TotalDays;
                    //decimal yearGone = Convert.ToDecimal(Math.Round(daysGone / 365.25, 0));
                    //decimal accmDep = assetCost * (depRate / 100M) * yearGone;
                    decimal assetCost = Convert.ToDecimal(dr["PurchaseAmount"].ToString());
                    DateTime date = Convert.ToDateTime(dr["InDate"].ToString());
                    string productid = (dr["ProductID"]).ToString();
                    
                    decimal deductAmtPerMonth = 0;
                    decimal assetCost1 = opAssetValue;

                    DateTime past = Convert.ToDateTime(date);
                    DateTime future = Convert.ToDateTime(dtFrom);

                    deductAmtPerMonth = GetDeprAmt(productid, Convert.ToDecimal(assetCost), past, future);

                    opAssetValue += assetCost;
                    opDepr += deductAmtPerMonth;
                }

                dt = SQLQuery.ReturnDataTable(@"SELECT        COUNT(FixedAssets.Id) AS QTY, FixedAssets.InDate, FixedAssets.ProductName,FixedAssets.ProductID, SUM(FixedAssets.PurchaseCost) AS PurchaseAmount, FixedAssets.ManufacturerName, FixedAssets.LCNoFTT, (Products.Depreciation) AS Depreciation, 
                         Products.ItemName, ItemSubGroup.CategoryName, Categories.CategoryName AS Grade, ItemGrade.GradeName,ItemGroup.GroupSrNo, ItemGroup.GroupName,ItemGroup.Depreciationvalue, FixedAssets.Location, SUM(FixedAssets.TotalValueUSD) AS TotalUSD
FROM            Categories INNER JOIN
                         FixedAssets INNER JOIN
                         Products ON FixedAssets.ProductID = Products.ProductID ON Categories.CategoryID = Products.CategoryID INNER JOIN
                         ItemGrade ON Categories.GradeID = ItemGrade.GradeID INNER JOIN
                         ItemSubGroup ON ItemGrade.CategoryID = ItemSubGroup.CategoryID INNER JOIN
                         ItemGroup ON ItemSubGroup.GroupID = ItemGroup.GroupSrNo
 WHERE ItemGroup.GroupSrNo='" + groupId + "' AND (InDate<='" + dtTo + @"')
GROUP BY FixedAssets.InDate, FixedAssets.ProductName,FixedAssets.ProductID, FixedAssets.ManufacturerName, FixedAssets.LCNoFTT, Products.Depreciation, Products.ItemName, ItemSubGroup.CategoryName, Categories.CategoryName, 
                         ItemGrade.GradeName,ItemGroup.GroupSrNo, ItemGroup.GroupName,ItemGroup.Depreciationvalue, FixedAssets.Location ORDER BY ItemGroup.GroupSrNo");

                foreach (DataRow dr in dt.Rows)
                {
                    decimal assetCost = Convert.ToDecimal(dr["PurchaseAmount"].ToString());
                    DateTime date = Convert.ToDateTime(dr["InDate"].ToString());
                    string productid = (dr["ProductID"]).ToString();
                    //decimal depRate = 0;
                    //if (!string.IsNullOrEmpty(dr["Depreciationvalue"].ToString()))
                    //{
                    //    Convert.ToDecimal(dr["Depreciationvalue"].ToString());
                    //}

                    //double daysGone = (DateTime.Now - date).TotalDays;
                    //decimal yearGone = Convert.ToDecimal(Math.Round(daysGone / 365.25, 0));
                    //decimal accmDep = opAssetValue * (depRate / 100M) * yearGone;
                    // New line
                    decimal deductAmtPerMonth = 0;
                    decimal assetCost1 = opAssetValue;
                    
                    DateTime past = Convert.ToDateTime(date);
                    DateTime future = Convert.ToDateTime(dtTo);

                    deductAmtPerMonth = GetDeprAmt(productid, Convert.ToDecimal(assetCost), past, future);
                    
                    //Closing Line



                    clAssetValue += assetCost;
                    clDepr += deductAmtPerMonth;

                }


                dr1 = dt1.NewRow();
                dr1["Particulars"] = drow["GroupName"];
                dr1["CostOpBL"] = opAssetValue;
                currBal = clAssetValue - opAssetValue;
                dr1["CostAddition"] = currBal;
                dr1["CostClBL"] = clAssetValue;
                //dr1["ABCD"] = clAssetValue - opDepr;
                dr1["ABCD"] = opDepr;
                decimal abcd = (int)clAssetValue + opDepr;
                dr1["Rate"] = drow["Depreciationvalue"];

                //New line
                //decimal deductAmtPerMonth = 0;
                //decimal assetCost1 = opAssetValue;
                //string productid = SQLQuery.ReturnString("SELECT id FROM vwProducts WHERE   GroupId ='" + groupId + "'");
                
                
                //DateTime past = Convert.ToDateTime(dtFrom);
                //DateTime future = Convert.ToDateTime(dtTo);

                //deductAmtPerMonth = GetDeprAmt(productid, Convert.ToDecimal(assetCost1), past, future);
                //dr1["DepOpBL"] = (deductAmtPerMonth);
                //Closing Line

               

                string opdep = Convert.ToString(opDepr);
                if (opdep == "" || opdep == "0")
                {
               
                    dr1["DepOpBL"] = "0";
                    
                    dr1["DepCharged"] = "0";
                    dr1["DepClBL"] ="0";
                    
                    dr1["WDVBalance"] = (clAssetValue);
                    
                    dt1.Rows.Add(dr1);
                }
                else
                {
                    dr1["DepOpBL"] = clAssetValue - opDepr;
                    decimal depopbl = clAssetValue - opDepr;
                    // decimal depchared =(int) (drow["Depreciation"])* currBal;
                    currdep = (int)(clDepr - opDepr);
                    decimal cldepbal = (int)(clDepr + opDepr);

                    decimal chargeddp = (Convert.ToDecimal(drow["Depreciationvalue"]) / 100M) * opDepr;

                    dr1["DepCharged"] = chargeddp;
                    dr1["DepClBL"] = depopbl + chargeddp;
                    decimal webcl = depopbl + chargeddp;
                    dr1["WDVBalance"] = (clAssetValue) - (webcl);


                    //dr1["CostOpBL"] = debt;
                    //dr1["CostClBL"] = credit;

                    //dr1["Balance"] = string.Format("{0:N2}", currBal);
                    dt1.Rows.Add(dr1);
                    //ic++;
                }

            }

            //get closing balance        
            //SqlCommand cmd2z = new SqlCommand("SELECT isnull(sum(Quantity),0)-isnull(sum(DeliveredQty),0) as balance FROM FixedAssets WHERE ProductID = @HeadName and OutDate <= @DateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            //cmd2z.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
            //cmd2z.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(dtTo).AddDays(+1).ToShortDateString();
            //cmd2z.Connection.Open();
            //currBal = Convert.ToDecimal(cmd2z.ExecuteScalar());
            //cmd2z.Connection.Close();

            //dr1 = dt1.NewRow();
            //dr1["Head"] = "";
            //dr1["TrDate"] = Convert.ToDateTime(dtTo).ToShortDateString();
            //dr1["Description"] = "Closing Stock";
            //dr1["Dr"] = 0;
            //dr1["Cr"] = 0;
            //dr1["Balance"] = string.Format("{0:N2}", (currBal + opBal));
            //dt1.Rows.Add(dr1);


            return dt1;
        }
        private DataTable dataatble2(string dtFrom, string dtTo)
        {
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("Particulars", typeof(string)));
            dt1.Columns.Add(new DataColumn("CostOpBL", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("CostAddition", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("CostClBL", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("ABCD", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Rate", typeof(string)));

            dt1.Columns.Add(new DataColumn("DepOpBL", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("DepCharged", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("DepClBL", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("WDVBalance", typeof(decimal)));

            dt1.Columns.Add(new DataColumn("Dr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Cr", typeof(decimal)));
            //dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));


            decimal debt = 0;
            decimal credit = 0;
            decimal currBal = 0;
            currdep = 0;

            decimal TotalValueUSD = 0;
            decimal AssetCost = 0;
            decimal Totalvalue = 0;


            // SqlCommand cmd2 = new SqlCommand("SELECT [InDate], [ProductName],[Remark], SUM(Quantity) As Dr, SUM(DeliveredQty) As Cr FROM [FixedAssets] WHERE ([ProductID] = @HeadName) AND (InDate >= @DateTo) AND (OutDate <= @DateFrom) Group by [InDate], [ProductName], [Remark]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            DataTable dt2 =
                SQLQuery.ReturnDataTable(
                    @"SELECT  DISTINCT Products.Depreciation,ItemGroup.GroupSrNo, ItemGroup.GroupName,ItemGroup.DepreciationType,ItemGroup.Depreciationvalue FROM            Categories INNER JOIN
                         FixedAssets INNER JOIN
                         Products ON FixedAssets.ProductID = Products.ProductID ON Categories.CategoryID = Products.CategoryID INNER JOIN
                         ItemGrade ON Categories.GradeID = ItemGrade.GradeID INNER JOIN
                         ItemSubGroup ON ItemGrade.CategoryID = ItemSubGroup.CategoryID INNER JOIN
                         ItemGroup ON ItemSubGroup.GroupID = ItemGroup.GroupSrNo WHERE  (ItemGroup.DepreciationType = '2') OR (ItemGroup.DepreciationType = '1')");

            //recordcount = dt2.Rows.Count;


            foreach (DataRow drow in dt2.Rows)
            {

                string groupId = drow["GroupSrNo"].ToString();

                decimal opAssetValue = 0, opDepr = 0, clAssetValue = 0, clDepr = 0;
                DataTable dt =
                    SQLQuery.ReturnDataTable(@"SELECT        FixedAssets.Id, FixedAssets.InDate, FixedAssets.ProductName,FixedAssets.ProductID, FixedAssets.PurchaseCost, FixedAssets.ManufacturerName, FixedAssets.LCNoFTT, Products.Depreciation, 
                         Products.ItemName, ItemSubGroup.CategoryName, Categories.CategoryName AS Grade, ItemGrade.GradeName,ItemGroup.GroupSrNo, ItemGroup.GroupName,ItemGroup.Depreciationvalue, FixedAssets.Location,FixedAssets.TotalValueUSD
FROM            Categories INNER JOIN
                         FixedAssets INNER JOIN
                         Products ON FixedAssets.ProductID = Products.ProductID ON Categories.CategoryID = Products.CategoryID INNER JOIN
                         ItemGrade ON Categories.GradeID = ItemGrade.GradeID INNER JOIN
                         ItemSubGroup ON ItemGrade.CategoryID = ItemSubGroup.CategoryID INNER JOIN
                         ItemGroup ON ItemSubGroup.GroupID = ItemGroup.GroupSrNo
 WHERE ItemGroup.GroupSrNo='" + groupId + "' AND (InDate<='" + dtFrom + "') AND (InDate <='" + dtTo + @"')
GROUP BY FixedAssets.Id,FixedAssets.InDate, FixedAssets.ProductName,FixedAssets.ProductID,FixedAssets.PurchaseCost, FixedAssets.ManufacturerName, FixedAssets.LCNoFTT, Products.Depreciation, Products.ItemName, ItemSubGroup.CategoryName, Categories.CategoryName, 
                         ItemGrade.GradeName,ItemGroup.GroupSrNo, ItemGroup.GroupName,ItemGroup.Depreciationvalue, FixedAssets.Location,FixedAssets.TotalValueUSD  ORDER BY ItemGroup.GroupSrNo");

                foreach (DataRow dr in dt.Rows)
                {
                    
                    decimal opassetCost = Convert.ToDecimal(dr["PurchaseCost"].ToString());
                    DateTime date = Convert.ToDateTime(dr["InDate"].ToString());
                    string productid = (dr["ProductID"]).ToString();
                    string productname = (dr["ProductName"]).ToString();

                    decimal deductAmtPerMonth = 0;
                    decimal assetCost1 = opAssetValue;
                    string getopamount = SQLQuery.ReturnString(@"SELECT        FixedAssets.PurchaseCost
FROM            Categories INNER JOIN
                         FixedAssets INNER JOIN
                         Products ON FixedAssets.ProductID = Products.ProductID ON Categories.CategoryID = Products.CategoryID INNER JOIN
                         ItemGrade ON Categories.GradeID = ItemGrade.GradeID INNER JOIN
                         ItemSubGroup ON ItemGrade.CategoryID = ItemSubGroup.CategoryID INNER JOIN
                         ItemGroup ON ItemSubGroup.GroupID = ItemGroup.GroupSrNo
WHERE        (FixedAssets.ProductName = '" + productname + "') AND (FixedAssets.InDate <= '" + dtFrom + "')");

                    string getclamount = SQLQuery.ReturnString(@"SELECT        FixedAssets.PurchaseCost
FROM            Categories INNER JOIN
                         FixedAssets INNER JOIN
                         Products ON FixedAssets.ProductID = Products.ProductID ON Categories.CategoryID = Products.CategoryID INNER JOIN
                         ItemGrade ON Categories.GradeID = ItemGrade.GradeID INNER JOIN
                         ItemSubGroup ON ItemGrade.CategoryID = ItemSubGroup.CategoryID INNER JOIN
                         ItemGroup ON ItemSubGroup.GroupID = ItemGroup.GroupSrNo
WHERE        (FixedAssets.ProductName = '" + productname + "') AND (FixedAssets.InDate <= '" + dtTo + "')");
                    DateTime past = Convert.ToDateTime(date);
                    DateTime future = Convert.ToDateTime(dtFrom);

                    opDepr = GetDeprAmt(productid, Convert.ToDecimal(getopamount), past, future);
                    DateTime past2 = Convert.ToDateTime(date);
                    DateTime future2 = Convert.ToDateTime(dtTo);

                    clDepr = GetDeprAmt(productid, Convert.ToDecimal(getclamount), past2, future2);

                    dr1 = dt1.NewRow();
                    dr1["Particulars"] = productname;
                    dr1["CostOpBL"] = Convert.ToDecimal(getopamount);
                    currBal = Convert.ToDecimal(getclamount) - Convert.ToDecimal(getopamount);
                    dr1["CostAddition"] = currBal;
                    dr1["CostClBL"] = Convert.ToDecimal(getclamount);
                    //dr1["ABCD"] = clAssetValue - opDepr;
                    dr1["ABCD"] = opDepr;
                    decimal abcd = (int)Convert.ToDecimal(getclamount) + opDepr;
                    dr1["Rate"] = drow["Depreciationvalue"];

                    
                    string opdep = Convert.ToString(opDepr);
                    if (opdep == "" || opdep == "0")
                    {

                        dr1["DepOpBL"] = "0";

                        dr1["DepCharged"] = "0";
                        dr1["DepClBL"] = "0";

                        dr1["WDVBalance"] = (Convert.ToDecimal(getclamount));

                        dt1.Rows.Add(dr1);
                    }
                    else
                    {
                        decimal depopbl = Convert.ToDecimal(getclamount) - opDepr;
                        decimal depclbl = Convert.ToDecimal(getclamount) - clDepr;
                        decimal depchange = depclbl - depopbl;

                        dr1["DepOpBL"] = depopbl;

                        // decimal depchared =(int) (drow["Depreciation"])* currBal;
                        currdep = (int)(clDepr - opDepr);
                        decimal cldepbal = (int)(clDepr + opDepr);

                        decimal chargeddp = (Convert.ToDecimal(drow["Depreciationvalue"]) / 100M) * opDepr;

                        dr1["DepCharged"] = depchange;
                        dr1["DepClBL"] = depclbl;
                        decimal webcl = depopbl + chargeddp;
                        dr1["WDVBalance"] = (Convert.ToDecimal(getclamount)) - (depclbl);


                        //dr1["CostOpBL"] = debt;
                        //dr1["CostClBL"] = credit;

                        //dr1["Balance"] = string.Format("{0:N2}", currBal);
                        dt1.Rows.Add(dr1);
                        //ic++;
                    }
                    SQLQuery.ExecNonQry("INSERT INTO [dbo].[Shareholdersequity] ([AccHeadName] ,[OpeningBalance] ,[BalanceDr1] ,[BalanceDr2] ,[BalanceDr3] ,[BalanceDr4] ,[BalanceCr1] ,[BalanceCr2] ,[BalanceCr3] ,[BalanceCr4] ,[ClosingBalance]) VALUES ('" + productname + "', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0')");
                    //opAssetValue += opassetCost;
                    //opDepr += deductAmtPerMonth;
                }

                


               

            }

            
            return dt1;
        }

        private decimal GetDeprAmt(string productId, decimal purchAmt, DateTime pDate, DateTime fdate)
        {
            string depPercent =
                SQLQuery.ReturnString("SELECT Convert(int, Depreciation) FROM Products Where ProductID ='" + productId + "'");
            if (depPercent == "" || depPercent == "0")
            {
                depPercent = SQLQuery.ReturnString("SELECT Depreciationvalue FROM ItemGroup WHERE GroupSrNo= (Select GroupId from vwProducts WHERE id='" + productId + "') ");
            }

            decimal monthlyPercent = (Convert.ToDecimal(depPercent)) / 12M;
            decimal monthlyAmt = purchAmt * (monthlyPercent / 100M);
            int monthsApart = 12 * (fdate.Year - pDate.Year) + fdate.Month - pDate.Month;
            int qty = Math.Abs(monthsApart);

            decimal total = qty * monthlyAmt;
            return Math.Round(purchAmt - total);
        }
        
      
        private string Depreciation(string groupId)
        {
            DataTable dt = SQLQuery.ReturnDataTable(@"SELECT AVG(Products.Depreciation) AS Depreciation FROM Products CROSS JOIN ItemGroup WHERE ItemGroup.GroupSrNo='" + groupId + "'");



            return "";
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