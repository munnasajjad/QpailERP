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
    public partial class FormPlasticFinishingProduction : System.Web.UI.Page
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
        private DataTable BindItemGrid(string customer, string brand, string purpose, string item, string pCcolor, string dateFrom, string dateTo)
        {
            DataSet ds = new DataSet();
            try
            {
                if (customer != "--- all ---")
                {
                    customer = " AND CustomerID='" + customer + "' ";
                    if (brand != "--- all ---")
                    {
                        brand = " AND Brand='" + brand + "' ";
                        customer = "";
                    }
                    else
                    {
                        brand = ""; 
                    }
                }
                else
                {
                    customer = "";
                    brand = "";
                }
                if (pCcolor != "--- all ---")
                {
                    pCcolor = " AND (Color = '" + pCcolor + "')  ";
                }
                else
                {
                    pCcolor = "";
                }

                if (dateFrom != "")
                {
                    dateFrom = " AND Date>='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ";
                }
                
                if (dateTo != "")
                {
                    dateTo = " AND Date<='" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "' ";
                }
                item = " AND (ItemName = '" + item + "')  ";
                string query = " FROM PrdnPlasticFinish a where pid<>0 " + customer + brand + item +  dateFrom + dateTo;
                string url = "";
                    // SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";

                query = @" SELECT (CONVERT(varchar,Date,103) +'') AS Date, (Select Company from Party where PartyID=a.CustomerID) AS  [Customer], 
                        (Select BrandName from CustomerBrands where BrandID=a.Brand) AS  [Brand], 
                        (Select BrandName from Brands where BrandID=a.PackSize) AS  [Pack Size], 
                        (SELECT ItemName FROM Products WHERE (ProductID = a.ItemName)) AS [Product Name], 
                        (SELECT [DepartmentName] FROM [Colors] Where Departmentid=(Select TOP(1) Color from PrdnPlasticFinishDetails where PrdnID=a.ProductionID)) as Color, FinalOutput as Qty, OutputWeight as Weight " +
                    query;
                ds = SQLQuery.ReturnDataSet(query);

                //ltrtotal.Text = "Total Result: " + ds.Tables[0].Rows.Count.ToString();
                //GridView1.DataSource = ds.Tables[0];
                //GridView1.DataBind();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                //lblMsg2.Attributes.Add("class", "xerp_warning");
                //lblMsg2.Text = "ERROR: " + ex.Message.ToString();
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
            string customer = Convert.ToString(Request.QueryString["Customer"]);
            string brand = Convert.ToString(Request.QueryString["Brand"]);
            string purpose = Convert.ToString(Request.QueryString["purpose"]);
            string item = Convert.ToString(Request.QueryString["itemName"]);
            string color = Convert.ToString(Request.QueryString["pCcolor"]);
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);
            

            DataTable dt1 = BindItemGrid(customer, brand, purpose, item, color, dateFrom, dateTo);
            DataTableReader dr2 = dt1.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.PlasticContainerHistory);

            rpt.Load(Server.MapPath("CrptPlasticFinishingProduction.rpt"));

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