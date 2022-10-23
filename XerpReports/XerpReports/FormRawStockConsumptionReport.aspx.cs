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
    public partial class FormRawStockConsumptionReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();

            /*
            //if (!IsPostBack)
            //{
            string invNo = 
            string chlNo = Convert.ToString(Request.QueryString["dt"]);
            string pid = Convert.ToString(Request.QueryString["pid"]);
            if (pid != null)
            {
                pid = " CustomerID='" + pid + "' AND ";
            }

            if (invNo != "")
            {
                
                SqlCommand cmd = new SqlCommand(@"SELECT Id, InvNo, SizeId, ProductID, BrandID, ProductName, UnitCost, Quantity, ItemTotal, VatPercent, VAT, NetAmount, UnitWeight, TotalWeight, QtyPerCarton, 
                         TotalCarton, UnitType, PerviousDeliveredQty, QtyBalance, ItemChallanNo,
                        (SELECT Company from Party where PartyID=(Select CustomerID from Sales where InvNo=SaleDetails.InvNo))  as  EntryBy,
                       (Select InvDate from Sales where InvNo=SaleDetails.InvNo) as EntryDate, ReturnQty
                        FROM SaleDetails WHERE InvNo IN (Select InvNo from Sales where " + pid + " InvDate>='" + invNo + "' AND  InvDate<='" + chlNo + "') ", new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
                cmd.Connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                //Pathshala ds = new Pathshala();
                ds.Load(dr, LoadOption.OverwriteChanges, ds.SaleDetails);
                cmd.Connection.Close();
                ReportDocument rpt = new ReportDocument();
                rpt.Load(Server.MapPath("Sales-Statement.rpt"));

                string datefield = "From " + Convert.ToDateTime(invNo).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(chlNo).ToString("dd/MM/yyyy");

                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
                rpt.SetDataSource(ds); SQLQuery.LoadrptHeader(ds, rpt);
                rpt.SetParameterValue("@date", datefield);
                CrystalReportViewer1.ReportSource = rpt;
                //CrystalReportViewer1.DataBind();


                //}
            }*/
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
            string igodown = Convert.ToString(Request.QueryString["godown"]);
            string ipurpos = Convert.ToString(Request.QueryString["purpos"]);
            string icategory = Convert.ToString(Request.QueryString["category"]);
            string igrade = Convert.ToString(Request.QueryString["grade"]);
            string isubgro = Convert.ToString(Request.QueryString["subgro"]);
            string iType = Convert.ToString(Request.QueryString["type"]);
            string dateFrom = Convert.ToString(Request.QueryString["dt1"]);
            string dateTo = Convert.ToString(Request.QueryString["dt2"]);


            DataTable dtx = BindItemGrid(igodown, ipurpos, icategory, igrade, isubgro, iType, dateFrom, dateTo); //RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.InventoryReport);

            rpt.Load(Server.MapPath("CrptRawStockConsumptionReport.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            string rptName = SQLQuery.ReturnString("SELECT [CategoryName] FROM [ItemSubGroup] WHERE CategoryID='" + isubgro + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;

            //rpt.Close();
            //rpt.Clone();
            //rpt.Dispose();
            //rpt = null;
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }

        private DataTable BindItemGrid(string ddGodown, string ddPurpose, string ddcategory, string ddGrade, string ddSubGroup, string ddType, string dateFrom, string dateTo)
        {
            string dtFrom2 = dateFrom;
            string dtTo2 = dateTo;
            DataSet ds = new DataSet();
            try
            {
                string godown = " ";
                if (ddGodown != "--- all ---")
                {
                    godown = " AND WarehouseID='" + ddGodown + "' ";
                }
                string subGroup = " ";
                if (ddcategory != "--- all ---")
                {
                    subGroup = " AND ProductID IN (Select ProductID from Products where CategoryID  ='" + ddcategory + "') ";
                }
                else if (ddGrade != "--- all ---")
                {
                    subGroup = " AND ProductID IN (Select ProductID from Products where CategoryID in (SELECT CategoryID FROM Categories WHERE GradeID IN (SELECT GradeID FROM ItemGrade WHERE CategoryID ='" + ddSubGroup + "'))) ";
                }
                else if (ddSubGroup != "--- all ---")
                {
                    subGroup = " AND ProductID IN (Select ProductID from Products where CategoryID in (SELECT CategoryID FROM Categories WHERE GradeID IN (SELECT GradeID FROM ItemGrade WHERE CategoryID ='" + ddSubGroup + "'))) ";
                }
                
                if (dateFrom != "")
                {
                    dateFrom = " AND EntryDate>='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ";
                }
                
                if (dateTo != "")
                {
                    dateTo = " AND EntryDate<='" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "' ";
                }

                string query = " FROM Stock a where EntryId<>0 " + godown + subGroup + dateFrom + dateTo;
                string url = "";// SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";


                if (ddSubGroup == "10") //printing ink
                {
                    query = " FROM Stock where EntryId<>0 " + godown + subGroup + dateFrom + dateTo;
                    ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
                                              (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
                                                    "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
                                                    " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName, " +
                                                    @" (SELECT [spec] FROM [Specifications] WHERE  CAST(id AS nvarchar)=Stock.Spec) AS Spec, 

(Select ISNULL(SUM(InWeight), 0) - ISNULL(SUM(OutWeight), 0) from Stock
    WHERE WarehouseID = a.WarehouseID AND ProductID = a.ProductID  AND EntryDate <= '" + dtFrom2 + @"') as 'Prv.Balance',

 ISNULL(SUM(InWeight), 0) as Adjustment,
ISNULL(SUM(OutWeight), 0) as Consumption,

(Select ISNULL(SUM(InWeight), 0) - ISNULL(SUM(OutWeight), 0) from Stock
    WHERE WarehouseID = a.WarehouseID AND ProductID = a.ProductID  AND EntryDate <= '" + dtTo2 + @"') as 'Current Balance'
 " 
                                                     + query + " " +
                                                    "  " +
                                                    "GROUP BY WarehouseID, ProductID, Spec HAVING (ISNULL(SUM(InWeight), 0) - ISNULL(SUM(OutWeight), 0) <> 0)");
                }
                else if (ddSubGroup == "33" || ddSubGroup == "35") //HTF & IML
                {
                    ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = a.WarehouseID)) AS Warehouse, 
                                              (Select Purpose from Purpose where pid=a.Purpose) AS Purpose," +
                                                " (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = a.ProductID)))) AS Grade, " +
                                                    "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = a.ProductID))) AS Category, " +
                                                    " (SELECT ItemName FROM Products WHERE (ProductID = a.ProductID)) AS ProductName, " +
                                                    "  ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty " + query + " " +
                                                    "  " +
                                                    "GROUP BY WarehouseID, Purpose, ProductID  HAVING (ISNULL(SUM(InQuantity), 0) - ISNULL(SUM(OutQuantity), 0) <> 0)");
                }
                else if (ddSubGroup == "12") //METAL HANDLE
                {
                    ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = a.WarehouseID)) AS Warehouse, 
                                 (Select Purpose from Purpose where pid=a.Purpose) AS Purpose, (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = a.ProductID)))) AS Grade, " +
                                    "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = a.ProductID))) AS Category, " +
                                    " (SELECT ItemName FROM Products WHERE (ProductID = a.ProductID)) AS ProductName,  " +
                                    " ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
                                    "  " +
                                    "GROUP BY WarehouseID, Purpose, ProductID HAVING (ISNULL(SUM(InQuantity), 0) - ISNULL(SUM(OutQuantity), 0) <> 0)");
                }
                else if (ddSubGroup != "9") //Not Tin Plate : Plastic Raw materials || ddSubGroup != "12"
                {
                    ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = a.WarehouseID)) AS Warehouse, 
                                              (Select Purpose from Purpose where pid=a.Purpose) AS Purpose," +
                                                " (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = a.ProductID)))) AS Grade, " +
                                                    "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = a.ProductID))) AS Category, " +
                                                    @" (SELECT ItemName FROM Products WHERE (ProductID = a.ProductID)) AS ProductName, 
(Select ISNULL(SUM(InWeight), 0) - ISNULL(SUM(OutWeight), 0) from Stock
    WHERE WarehouseID = a.WarehouseID AND ProductID = a.ProductID  AND EntryDate <= '" + dtFrom2 + @"') as 'Prv.Balance',

 ISNULL(SUM(InWeight), 0) as Adjustment,
ISNULL(SUM(OutWeight), 0) as Consumption,

(Select ISNULL(SUM(InWeight), 0) - ISNULL(SUM(OutWeight), 0) from Stock
    WHERE WarehouseID = a.WarehouseID AND ProductID = a.ProductID  AND EntryDate <= '" + dtTo2 + @"') as 'Current Balance'
 " + query + " " +"  " +
 "GROUP BY WarehouseID, Purpose, ProductID HAVING (ISNULL(SUM(InWeight), 0) - ISNULL(SUM(OutWeight), 0) <> 0)");
                }
                else
                {
                    if (ddType == "Printed Sheet")
                    {
                        query += " AND ItemType='" + ddType + "' ";
                        ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
                                 (Select Purpose from Purpose where pid=Stock.Purpose) AS Purpose, (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
                                    "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
                                    " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName, (SELECT Company FROM Party WHERE (PartyID = Stock.Customer)) AS Customer, " +
                                    " (SELECT BrandName FROM CustomerBrands WHERE (BrandID = Stock.BrandID)) AS BrandID, (SELECT [BrandName] FROM [Brands] WHERE BrandID=Stock.SizeId) AS SizeId, " +
                                    "(SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=Stock.Color) AS Color,  " +
                                    " ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
                                    "  " +
                                    "GROUP BY WarehouseID, Purpose, ProductID, Customer, BrandID, SizeId, Color HAVING (ISNULL(SUM(InQuantity), 0) - ISNULL(SUM(OutQuantity), 0) <> 0)");
                    }
                    else //Raw Sheet
                    {
                        query += " AND ItemType='" + ddType + "' ";
                        ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
                                 (Select Purpose from Purpose where pid=Stock.Purpose) AS Purpose, (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
                                    "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
                                    " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName,  " +
                                    " ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
                                    "  " +
                                    "GROUP BY WarehouseID, Purpose, ProductID HAVING (ISNULL(SUM(InQuantity), 0) - ISNULL(SUM(OutQuantity), 0) <> 0)");
                    }
                }

                return ds.Tables[0];
                //ltrtotal.Text = "Total Result: " + ds.Tables[0].Rows.Count.ToString();
                //GridView1.DataSource = ds.Tables[0];
                //GridView1.DataBind();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

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