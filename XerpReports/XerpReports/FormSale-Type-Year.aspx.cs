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
    public partial class FormSale_Type_Year : System.Web.UI.Page
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
        private DataTable LoadDataTable(string grade, string rType, string chk, string year )
        {
            //string sizeId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) SizeId FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");
            //string pId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) ProductID FROM [SaleDetails]  WHERE ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID='" + ddGrade.SelectedValue + "')) AND  ([productName] = '" + iName + "')");
            //string brandId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) BrandID FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");

            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;

            dt1.Columns.Add(new DataColumn("RowName", typeof(string)));
            dt1.Columns.Add(new DataColumn("January", typeof(string)));
            dt1.Columns.Add(new DataColumn("February", typeof(string)));
            dt1.Columns.Add(new DataColumn("March", typeof(string)));
            dt1.Columns.Add(new DataColumn("April", typeof(string)));
            dt1.Columns.Add(new DataColumn("May", typeof(string)));
            dt1.Columns.Add(new DataColumn("June", typeof(string)));
            dt1.Columns.Add(new DataColumn("July", typeof(string)));
            dt1.Columns.Add(new DataColumn("August", typeof(string)));
            dt1.Columns.Add(new DataColumn("September", typeof(string)));
            dt1.Columns.Add(new DataColumn("October", typeof(string)));
            dt1.Columns.Add(new DataColumn("November", typeof(string)));
            dt1.Columns.Add(new DataColumn("December", typeof(string)));
            dt1.Columns.Add(new DataColumn("Total", typeof(string)));

            //Customer List 
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT a.CustomerID, (Select Company from Party where PartyID=a.CustomerID) AS PartyName FROM Sales a ORDER BY [PartyName]");

            foreach (DataRow drx in dtx.Rows)
            {
                string customerID = drx["CustomerID"].ToString();
                string customerName = drx["PartyName"].ToString();

                dr1 = dt1.NewRow();
                dr1["RowName"] = customerName;
                dr1["January"] = CalcQty(customerID, year + "-01-01", year + "-02-01", grade, rType, chk);
                dr1["February"] = CalcQty(customerID, year + "-02-01", year + "-03-01", grade, rType, chk);
                dr1["March"] = CalcQty(customerID, year + "-03-01", year + "-04-01", grade, rType, chk);
                dr1["April"] = CalcQty(customerID, year + "-04-01", year + "-05-01", grade, rType, chk);
                dr1["May"] = CalcQty(customerID, year + "-05-01", year + "-06-01", grade, rType, chk);
                dr1["June"] = CalcQty(customerID, year + "-06-01", year + "-07-01", grade, rType, chk);
                dr1["July"] = CalcQty(customerID, year + "-07-01", year + "-08-01", grade, rType, chk);
                dr1["August"] = CalcQty(customerID, year + "-08-01", year + "-09-01", grade, rType, chk);
                dr1["September"] = CalcQty(customerID, year + "-09-01", year + "-10-01", grade, rType, chk);
                dr1["October"] = CalcQty(customerID, year + "-10-01", year + "-11-01", grade, rType, chk);
                dr1["November"] = CalcQty(customerID, year + "-11-01", year + "-12-01", grade, rType, chk);

                int nextYear = Convert.ToInt32(year) + 1;
                dr1["December"] = CalcQty(customerID, year + "-12-01", nextYear + "-01-01", grade, rType, chk);
                dr1["Total"] = CalcQty(customerID, year + "-01-01", nextYear + "-01-01", grade, rType, chk);

                dt1.Rows.Add(dr1);

            }

            
            return dt1;
        }

        private int CalcQty(string customerId, string dtFrom, string dtTo, string grade, string rType, string chk)
        {
            dtTo = Convert.ToDateTime(dtTo).AddDays(-1).ToString("yyyy-MM-dd");
            string colName = rType;
            int qty = 0;//Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(" + colName + "),0)  FROM [SaleDetails] WHERE ([SizeId] = '" + sizeId + "') and ([ProductID] = '" + pId + "') and([BrandID] = '" + brandId + "') and EntryDate >= '" + dtFrom + "' AND EntryDate < '" + dtTo + "' ")));
            if (chk=="1")
            {
                string query = " ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID<>'20')) ";
                qty = Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(" + colName + "),0)  FROM [SaleDetails] WHERE " + query + "  AND   InvNo IN (SELECT InvNo FROM Sales WHERE CustomerID = '" + customerId + "' and DeliveryDate >= '" + dtFrom + "' AND DeliveryDate <= '" + dtTo + "') ")));

                if (colName == "NetAmount")
                {
                    qty = Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select   ISNULL(SUM(ItemTotal), 0) + ISNULL(SUM(VAT), 0)   FROM [SaleDetails] WHERE " + query + "  AND   InvNo IN (SELECT InvNo FROM Sales WHERE CustomerID = '" + customerId + "' and DeliveryDate >= '" + dtFrom + "' AND DeliveryDate <= '" + dtTo + "') ")));
                }

            }
            else
            {
                qty = Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(" + colName + "),0)  FROM [SaleDetails] WHERE ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID='" + grade + "')) AND   InvNo IN (SELECT InvNo FROM Sales WHERE (CustomerID = '" + customerId + "') AND (DeliveryDate >= '" + dtFrom + "') AND (DeliveryDate <= '" + dtTo + "'))")));

                if (colName == "NetAmount")
                {
                    qty = Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(ItemTotal), 0) + ISNULL(SUM(VAT), 0) FROM [SaleDetails] WHERE ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID='" + grade + "')) AND   InvNo IN (SELECT InvNo FROM Sales WHERE (CustomerID = '" + customerId + "') AND (DeliveryDate >= '" + dtFrom + "') AND (DeliveryDate <= '" + dtTo + "'))")));
                }

            }
            return qty;
        }
        ReportDocument rpt = new ReportDocument();
        private void LoadGridData()
        {
            decimal total2 = (decimal)0.0;
            decimal qty2 = (decimal)0.0;
            decimal vat2 = (decimal)0.0;
            decimal TotalSales2 = (decimal)0.0;
            decimal TotalWeight2 = (decimal)0.0;

            string lName = Page.User.Identity.Name.ToString();
            string prjID = "1";
            string grade = Convert.ToString(Request.QueryString["igrade"]);
            string rType = Convert.ToString(Request.QueryString["iRType"]);
            string chk = Convert.ToString(Request.QueryString["ichk"]);
            string year = Convert.ToString(Request.QueryString["iyear"]);
            //string dtFrom = Convert.ToString(Request.QueryString["dt1"]);
            //string dtTo = Convert.ToString(Request.QueryString["dt2"]);

            DataTable dtx = LoadDataTable(grade, rType, chk, year); //RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.YearlySalesSum);
            
            rpt.Load(Server.MapPath("CrptSale-Type-Year.rpt"));

            string datefield = "Year: " + year+ ", Record Type:" + rType;
            //string rptName = SQLQuery.ReturnString("SELECT [CategoryName] FROM [ItemSubGroup] WHERE CategoryID='" + rType + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            //rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;

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