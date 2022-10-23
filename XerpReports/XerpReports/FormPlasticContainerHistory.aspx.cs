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
    public partial class FormPlasticContainerHistory : System.Web.UI.Page
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
        private DataTable BindItemGrid(string purpose, string item, string dateFrom, string dateTo, string cust, string pCcolor, string pkSize)
        {
            DataSet ds = new DataSet();
            try
            {
                if (item != "--- all ---")
                {
                    item = " AND (PrdPlasticCont.ItemID = '" + item + "')  ";
                }
                else
                {
                    item = "";
                }

                if (dateFrom != "")
                {
                    dateFrom = " AND PrdPlasticCont.Date>='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ";
                }

                if (dateTo != "")
                {
                    dateTo = " AND PrdPlasticCont.Date<='" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "' ";
                }
                
                if (cust != "")
                {
                    cust = " AND PrdPlasticCont.CustomerID='" + cust + "' ";
                }
                if (pCcolor != "--- all ---")
                {
                    pCcolor = " AND PrdPlasticCont.Color='" + pCcolor + "' ";
                }
                if (pkSize != "--- all ---")
                {
                    pkSize = " AND PrdPlasticCont.PackSize='" + pkSize + "' ";
                }
                else
                {
                    pkSize = "";

                }
                string query = "  where PrdPlasticCont.pid<>0 " + item + dateFrom + dateTo + cust + pCcolor + pkSize;
                string url = "";// SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";
                /*
                query = @" SELECT (CONVERT(varchar,Date,103) +'') AS Date, (Select Company from Party where PartyID=a.CustomerID) AS  [Customer], 
                        (Select BrandName from CustomerBrands where BrandID=a.Brand) AS  [Brand], 
                        (Select BrandName from Brands where BrandID=a.PackSize) AS  [Pack Size], 
                        (SELECT ItemName FROM Products WHERE (ProductID = a.ItemID)) AS [Product Name], 
                        (SELECT [DepartmentName] FROM [Colors] Where Departmentid=a.Color) as Color, FinalProduction as Qty, FinalKg as Weight " + query+ " Order by Date";*/

                query = @"SELECT  (CONVERT(varchar,Date,103) +'') AS Date, Party.Company AS  [Customer], CustomerBrands.BrandName AS  [Brand], Brands.BrandName AS  [Pack Size], Colors.DepartmentName AS [Color], Products.ItemName AS [Product Name], SUM(PrdPlasticCont.FinalProduction) AS Qty, SUM(PrdPlasticCont.FinalKg) AS  Weight
FROM            Brands INNER JOIN
                         Party INNER JOIN
                         PrdPlasticCont ON Party.PartyID = PrdPlasticCont.CustomerID INNER JOIN
                         CustomerBrands ON PrdPlasticCont.Brand = CustomerBrands.BrandID ON Brands.BrandID = PrdPlasticCont.PackSize INNER JOIN
                         Colors ON PrdPlasticCont.Color = Colors.Departmentid INNER JOIN
                         Products ON PrdPlasticCont.ItemID = Products.ProductID   " + query + @" 
GROUP BY Party.Company, CustomerBrands.BrandName, Brands.BrandName, Colors.DepartmentName, Products.ItemName, Date";

                ds = SQLQuery.ReturnDataSet(query);
                
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
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
            string purpose = Convert.ToString(Request.QueryString["purpose"]);
            string item = Convert.ToString(Request.QueryString["item"]);
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);
            string cust = Convert.ToString(Request.QueryString["cust"]);
            string color = Convert.ToString(Request.QueryString["pCcolor"]);
            string pkSize = Convert.ToString(Request.QueryString["pkSize"]);

            DataTable dt1 = BindItemGrid(purpose, item, dateFrom, dateTo, cust, color, pkSize);
            DataTableReader dr2 = dt1.CreateDataReader();
            
            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.PlasticContainerHistory);
            
            rpt.Load(Server.MapPath("CrptPlasticContaiHistory.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
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