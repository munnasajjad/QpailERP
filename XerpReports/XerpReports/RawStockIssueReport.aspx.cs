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
    public partial class RawStockIssueReport : System.Web.UI.Page
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
            string igroup = Convert.ToString(Request.QueryString["group"]);
            string icategory = Convert.ToString(Request.QueryString["category"]);
            string igrade = Convert.ToString(Request.QueryString["grade"]);
            string isubgro = Convert.ToString(Request.QueryString["subgro"]);
            //string iType = Convert.ToString(Request.QueryString["type"]);
            string dateFrom = Convert.ToString(Request.QueryString["dt1"]);
            string dateTo = Convert.ToString(Request.QueryString["dt2"]);

            search(dateFrom, dateTo);
            BindItemGrid(igodown, igroup, icategory, igrade, isubgro, dateFrom, dateTo); //RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");

            
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT Id, SLNo, GroupName, Description, Warehouse, Purpose, Grade, Category, ProductName, Weight FROM RptProduction WHERE Weight<>'0' ORDER BY Id ");

            DataTableReader dr2 = dtx.CreateDataReader();
            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.RptProduction);
            rpt.Load(Server.MapPath("CrptRawStockIssue.rpt"));
            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");
            string rptGroup = SQLQuery.ReturnString("SELECT [GroupName] FROM [ItemGroup] WHERE GroupSrNo='" + igroup + "'");
            string rptSubgro = SQLQuery.ReturnString("SELECT CategoryName FROM [ItemSubGroup] WHERE CategoryID='" + isubgro + "'");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            //rpt.SetParameterValue("@rptName", rptName);
            rpt.SetParameterValue("@group", rptGroup);
            rpt.SetParameterValue("@subgro", rptSubgro);
            CrystalReportViewer1.ReportSource = rpt;
        }

        
        private void BindItemGrid(string ddGodown, string ddGroup, string ddcategory, string ddGrade, string ddSubGroup, string dt1, string dt2)
        {
            DataSet ds = new DataSet();
            try
            {
                string godown = " ";
                if (ddGodown != "--- all ---")
                {
                    godown = " AND WarehouseID='" + ddGodown + "' ";
                }
                string group = " ";
                if (ddGroup != "--- all ---")
                {
                    group = " AND GroupSrNo='" + ddGroup + "' ";
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

                string date = " ";
                if (dt1 != "")
                {
                    date = " AND EntryDate>='" + Convert.ToDateTime(dt1).ToString("yyyy-MM-dd") + "' AND EntryDate<='" + Convert.ToDateTime(dt2).ToString("yyyy-MM-dd") + "'";
                }

                string query = " FROM Stock where EntryId<>0 " + godown + subGroup + date;
                
                string sLNo = "", groupName = "", description = "", Warehouse = "", Purpose = "", Grade = "", Category = "", ProductName = "";
                decimal Weight = 0;

                DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
                                                            (Select Purpose from Purpose where pid=Stock.Purpose) AS Purpose, (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade,
                                                            (SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category,  (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName, ISNULL(SUM(InWeight),0) AS Weight " + query + " GROUP BY WarehouseID, Purpose, ProductID HAVING (ISNULL(SUM(InWeight), 0) - ISNULL(SUM(OutWeight), 0) <> 0)");
                foreach (DataRow drx in dtx.Rows)
                {
                    Warehouse = drx["Warehouse"].ToString();
                    Purpose = drx["Purpose"].ToString();
                    Grade = drx["Grade"].ToString();
                    Category = drx["Category"].ToString();
                    ProductName = drx["ProductName"].ToString();
                    Weight = Convert.ToDecimal(drx["Weight"].ToString());

                    InsertRptProduction(sLNo, "Current Month Addition/Issued", description, Warehouse, Purpose, Grade, Category, ProductName, Weight.ToString());

                }

            }
            catch (Exception ex)
            {
                //lblMsg2.Attributes.Add("class", "xerp_warning");
                //lblMsg2.Text = "ERROR: " + ex.Message.ToString();
            }
            finally
            {

            }
           
        }
        private static void InsertRptProduction(string sLNo, string groupName, string description, string Warehouse, string Purpose, string Grade, string Category, string ProductName, string Weight)
        {
            SQLQuery.ExecNonQry("Insert INTO RptProduction (SLNo, GroupName, Description, Warehouse, Purpose, Grade, Category, ProductName, Weight)" +
                                " VALUES ('" + sLNo + "','" + groupName + "','" + description + "','" + Warehouse + "','" + Purpose + "','" + Grade + "','" + Category + "','" + ProductName + "','" + Weight + "')");
        }

        private string search(string dt1, string dt2)
        {
            try
            {
                SQLQuery.ExecNonQry("Delete RptProduction");
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