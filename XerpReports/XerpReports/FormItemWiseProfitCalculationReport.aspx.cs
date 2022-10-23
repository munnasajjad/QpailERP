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
    public partial class FormItemWiseProfitCalculationReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();
        }
        private DataTable BindItemGrid(string customer, string packSize, string brand, string item, string dateFrom, string dateTo)
        {
            DataSet ds = new DataSet();
            try
            {
                if (customer != "")
                {
                    customer = " AND CustomerID='" + customer + "' ";
                }
                else
                {
                    customer = "";
                }
                if (packSize != "")
                {
                    packSize = " AND SizeId='" + packSize + "' ";
                }
                else
                {
                    packSize = "";
                }
                if (brand != "")
                {
                    brand = " AND BrandID='" + brand + "' ";
                }
                else
                {
                    brand = "";
                }

                if (dateFrom != "")
                {
                    dateFrom = " AND EntryDate>='" + Convert.ToDateTime(dateFrom).AddDays(1).ToString("yyyy-MM-dd") + "' ";
                }

                if (dateTo != "")
                {
                    dateTo = " AND EntryDate<='" + Convert.ToDateTime(dateTo).AddDays(1).ToString("yyyy-MM-dd") + "' ";
                }

                if (item != "--- all ---")
                {
                    item = " AND (ProductID = '" + item + "')  ";
                }
                else
                {
                    item = "";
                }
                string query = " FROM OrderDetails WHERE Id<>0 " + customer + brand + packSize + item + dateFrom + dateTo;
                string url = "";
                // SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";

                query = @"SELECT ProductID, ProductName, SUM(Quantity) AS Quantity, SUM(ItemTotal) AS ItemTotal, SUM(ProfitAmount) AS ProfitAmount " + query + " GROUP BY ProductID, ProductName";
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
            string brand = Convert.ToString(Request.QueryString["brand"]);
            string item = Convert.ToString(Request.QueryString["item"]);
            string customer = Convert.ToString(Request.QueryString["customer"]);
            string packSize = Convert.ToString(Request.QueryString["packSize"]);
            string color = Convert.ToString(Request.QueryString["color"]);
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);


            DataTable dt1 = BindItemGrid(customer, packSize, brand, item, dateFrom, dateTo);
            DataTableReader dr2 = dt1.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.ItemWiseProfit);

            rpt.Load(Server.MapPath("CrptItemWiseProfit.rpt"));

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