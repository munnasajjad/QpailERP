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
    public partial class FifoSummaryReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();
        }
        private DataTable BindItemGrid(string item, string brand, string size, string color, string transactionType, string godown, string dateFrom, string dateTo)
        {
            DataSet ds = new DataSet();
            try
            {
                if (item != "")
                {
                    item = " AND ItemCode='" + item + "' ";
                }
                else
                {
                    brand = "";
                }
                if (brand != "")
                {
                    brand = " AND Brand='" + brand + "' ";
                }
                else
                {
                    brand = "";
                }
                if (size != "")
                {
                    size = " AND SizeId='" + size + "' ";
                }
                else
                {
                    size = "";
                }
                if (color != "")
                {
                    color = " AND ColorId = '" + color + "' ";
                }
                else
                {
                    color = "";
                }
                if (transactionType != "")
                {
                    transactionType = " AND TransactionType = '" + transactionType + "' ";
                }
                else
                {
                    transactionType = "";
                }
                if (godown != "")
                {
                    godown = " AND GodownId = '" + godown + "' ";
                }
                else
                {
                    godown = "";
                }

                if (dateFrom != "")
                {
                    dateFrom = " AND Date>='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ";
                }

                if (dateTo != "")
                {
                    dateTo = " AND Date<='" + Convert.ToDateTime(dateTo).AddDays(1).ToString("yyyy-MM-dd") + "' ";
                }
                string query = " FROM vw_FIFO WHERE ItemCode<>0 " + item + brand + size + color + transactionType + godown + dateFrom + dateTo;
                query = @"SELECT ItemCode, TransactionType, SizeId, ColorId, BrandId, Type, SUM(InKg) AS InKg, SUM(InPcs) AS InPcs, SUM(OutKg) AS OutKg, SUM(OutPcs) AS OutPcs, SUM(Fraction) AS Fraction" + query+ " GROUP BY ItemCode, TransactionType, SizeId, ColorId, BrandId, Type";
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
            string lName = Page.User.Identity.Name.ToString();
            string prjID = "1";
            string item = Convert.ToString(Request.QueryString["item"]);
            string brand = Convert.ToString(Request.QueryString["brand"]);
            string size = Convert.ToString(Request.QueryString["size"]);
            string color = Convert.ToString(Request.QueryString["color"]);
            string transactionType = Convert.ToString(Request.QueryString["transactionType"]);
            string godown = Convert.ToString(Request.QueryString["godown"]);
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);


            DataTable dt1 = BindItemGrid(item, brand, size, color, transactionType, godown, dateFrom, dateTo);
            DataTableReader dr2 = dt1.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.vw_FIFO);

            rpt.Load(Server.MapPath("CrptFifoSummary.rpt"));

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