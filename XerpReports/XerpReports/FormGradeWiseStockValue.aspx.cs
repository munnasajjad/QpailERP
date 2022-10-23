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
    public partial class FormGradeWiseStockValue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            LoadGridData();


        }

        ReportDocument rpt = new ReportDocument();
        private void LoadGridData()
        {



            string lName = Page.User.Identity.Name.ToString();
            string prjID = "1";
            string categoryName = Convert.ToString(Request.QueryString["CategoryName"]);
            string productId = Convert.ToString(Request.QueryString["ProductID"]);
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);

            DataTable dtx = LoadGridData(categoryName, dateFrom, dateTo, productId);

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.GradeStock);

            rpt.Load(Server.MapPath("CrptGradeWiseStockValue.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            // string rptName = SQLQuery.ReturnString("SELECT [Company] FROM [Party] WHERE PartyId='" + item + "'");
            //string rptType = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + reporType + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@itemName", SQLQuery.ReturnString("SELECT ProductName FROM vwGradeWiseStock WHERE ProductID='" + productId + "'"));
            rpt.SetParameterValue("@categoryName", categoryName);
            rpt.SetParameterValue("@rptType", productId);
            CrystalReportViewer1.ReportSource = rpt;

        }
        private DataTable LoadGridData(string category, string dtFrom, string dtTo, string itemId)
        {
            string cate2 = " "; string item = " ";
            decimal opBal = 0;
            decimal opQty = 0;
            if (category != "---ALL---" && category != null)
            {
                cate2 = " AND CategoryName ='" + category + "'";
                item = itemId;
                //  item = " WHERE ProductID ='" + itemId + "'";

                opQty = Convert.ToDecimal(SQLQuery.ReturnString("SELECT Opening  FROM  vwGradeWiseStock WHERE ProductID=" + item));
                opBal = Convert.ToDecimal(SQLQuery.ReturnString("SELECT  OpeningBalance FROM  vwGradeWiseStock WHERE ProductID=" + item));
            }

            string opDate = " "; string invDate = " ";
            if (dtFrom != "")
            {
                try
                {
                    invDate = Convert.ToDateTime(dtFrom).ToString("yyyy-MM-dd");
                    opDate = " AND  (PrdnDate < '" + invDate + "')";
                    invDate = " AND  (PrdnDate >= '" + invDate + "')";
                }
                catch (Exception) { }
            }

            string closeDate = " ";
            if (dtTo != "")
            {
                try
                {
                    closeDate = Convert.ToDateTime(dtTo).ToString("yyyy-MM-dd");
                    closeDate = " AND  (PrdnDate <= '" + closeDate + "')";
                }
                catch (Exception) { }
            }

            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("PrdnDate", typeof(string)));
            dt1.Columns.Add(new DataColumn("ProductName", typeof(string)));
            dt1.Columns.Add(new DataColumn("CategoryName", typeof(string)));
            dt1.Columns.Add(new DataColumn("StoreName", typeof(string)));
            dt1.Columns.Add(new DataColumn("IssuedAmount", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("ReceivedAmount", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));

            int recordcount = 0;
            int ic = 0;

            decimal issued = 0; decimal received = 0;
            string date; string description;
            string product = "";
            string cateName = "";
            string dateFrom = Convert.ToDateTime(dtFrom).ToString("yyyy-MM-dd");
            string dateTo = Convert.ToDateTime(dtTo).ToString("yyyy-MM-dd");

            //get opening balance        
            string query = " WHERE  ProductID = " + item + cate2 + opDate;
            decimal currBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(IssuedQty),0)-isnull(sum(ReceivedQty),0) FROM vwGradeWiseStock" + query));

            query = " WHERE  ProductID = " + item + cate2 + closeDate;
            decimal closeBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(IssuedQty),0)-isnull(sum(ReceivedQty),0) FROM vwGradeWiseStock" + query));



            dr1 = dt1.NewRow();
            dr1["PrdnDate"] = Convert.ToDateTime(dtFrom).ToShortDateString();
            dr1["ProductName"] = "";
            dr1["CategoryName"] = "";
            dr1["StoreName"] = "Openning Balance";
            dr1["IssuedAmount"] = 0;
            dr1["ReceivedAmount"] = 0;
            dr1["Balance"] = currBal;
            dt1.Rows.Add(dr1);

            query = " WHERE  ProductID =  " + item + cate2 + invDate + closeDate;
            SqlCommand cmd2 = new SqlCommand("SELECT  ProductID, CategoryName, StoreName, InType, ProductName, Price, ReceivedQty, IssuedQty, Opening, Closing, ReceivedAmount, IssuedAmount, OpeningBalance, ClosingBalance, PrdnDate FROM   vwGradeWiseStock " + query + "  ORDER BY PrdnDate DESC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            //cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = category;
            //cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
            //cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(dtTo).AddDays(+1).ToShortDateString();

            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataSet ds = new DataSet("Board");

            cmd2.Connection.Open();
            da.Fill(ds, "Board");

            SqlDataReader dr = cmd2.ExecuteReader();
            recordcount = ds.Tables[0].Rows.Count;
            cmd2.Connection.Close();

            if (recordcount > 0)
            {
                do
                {
                    date = Convert.ToDateTime(ds.Tables[0].Rows[ic]["PrdnDate"].ToString()).ToString("dd/MM/yyyy");
                    product = ds.Tables[0].Rows[ic]["ProductName"].ToString();
                    cateName = ds.Tables[0].Rows[ic]["CategoryName"].ToString();
                    description = ds.Tables[0].Rows[ic]["StoreName"].ToString();
                    issued = Convert.ToDecimal(ds.Tables[0].Rows[ic]["IssuedAmount"].ToString());
                    received = Convert.ToDecimal(ds.Tables[0].Rows[ic]["ReceivedAmount"].ToString());
                    currBal = issued - received + currBal;



                    dr1 = dt1.NewRow();
                    dr1["PrdnDate"] = date;
                    dr1["ProductName"] = product;
                    dr1["CategoryName"] = cateName;
                    dr1["StoreName"] = description;
                    dr1["IssuedAmount"] = issued;
                    dr1["ReceivedAmount"] = received;
                    dr1["Balance"] = currBal;

                    dt1.Rows.Add(dr1);
                    ic++;

                } while (ic < recordcount);

            }
            else { }

            //set closing balance    

            dr1 = dt1.NewRow();
            dr1["PrdnDate"] = Convert.ToDateTime(dtTo).ToShortDateString();
            dr1["ProductName"] = "";
            dr1["CategoryName"] = "";
            dr1["StoreName"] = "Closing Balance";
            dr1["IssuedAmount"] = 0;
            dr1["ReceivedAmount"] = 0;
            dr1["Balance"] = closeBal;
            dt1.Rows.Add(dr1);

            return dt1;
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