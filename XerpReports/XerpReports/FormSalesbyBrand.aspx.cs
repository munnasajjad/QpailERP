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
    public partial class FormSalesbyBrand : System.Web.UI.Page
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
            decimal total2 = (decimal)0.0;
            decimal qty2 = (decimal)0.0;
            decimal vat2 = (decimal)0.0;
            decimal TotalSales2 = (decimal)0.0;
            decimal TotalWeight2 = (decimal)0.0;

            string lName = Page.User.Identity.Name.ToString();
            string prjID = "1";
            //cust=" + ddCustomer.SelectedValue + "&dt1=" + dtFrom + "&dt2=" + dtTo;
            string size = Convert.ToString(Request.QueryString["size"]);
            string grade = Convert.ToString(Request.QueryString["iGrade"]);
            string chk = Convert.ToString(Request.QueryString["chk"]);
            string chk2 = Convert.ToString(Request.QueryString["chk2"]);
            string cust = Convert.ToString(Request.QueryString["cust"]);
            string brand = Convert.ToString(Request.QueryString["brand"]);
            string dateFrom = Convert.ToString(Request.QueryString["dt1"]);
            string dateTo = Convert.ToString(Request.QueryString["dt2"]);
            //string sizeId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) SizeId FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");
            //string pId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) ProductID FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");
            //string brandId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) BrandID FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");

            //DateTime dtFrom = Convert.ToDateTime(txtDateFrom.Text);
            //DateTime dateTo = Convert.ToDateTime(txtDateTo.Text);

            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;

            dt1.Columns.Add(new DataColumn("InvNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("LINK", typeof(string)));
            dt1.Columns.Add(new DataColumn("EntryDate", typeof(string)));
            dt1.Columns.Add(new DataColumn("ProductName", typeof(string)));
            dt1.Columns.Add(new DataColumn("Quantity", typeof(string)));
            dt1.Columns.Add(new DataColumn("UnitCost", typeof(string)));
            dt1.Columns.Add(new DataColumn("UnitWeight", typeof(string)));
            dt1.Columns.Add(new DataColumn("ItemTotal", typeof(string)));
            dt1.Columns.Add(new DataColumn("TotalWeight", typeof(string)));
            dt1.Columns.Add(new DataColumn("VAT", typeof(string)));
            dt1.Columns.Add(new DataColumn("TotalAmt", typeof(string)));

            string query = " AND  ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID='" + grade + "'))  ";
            if (chk=="0") // Not All Size
            {
                query += " and  ([SizeId] = '" + size + "')";
            }

            string brandQuery = " AND BrandID='" + brand + "'";
            if (chk2 == "1") // All Brands of customer
            {
                brandQuery = " AND InvNo IN (Select InvNo from sales where CustomerID = '" + cust +
                             "' and DeliveryDate >= '" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' AND DeliveryDate <= '" + dateTo + "')  ";
            }
            else
            {
                brandQuery += " AND InvNo IN (Select InvNo from sales where  DeliveryDate >= '" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' AND DeliveryDate <= '" + dateTo + "')  ";
            }
            query += brandQuery;
            DataTable dt = RunQuery.SQLQuery.ReturnDataTable("SELECT [InvNo], [ProductName], [Quantity], [UnitType], [UnitCost], [ItemTotal], [UnitWeight], [TotalWeight], [VAT], NetAmount, [EntryDate], [ReturnQty] FROM [SaleDetails] WHERE ID<>0 " + query + " ORDER BY EntryDate ASC ");


            foreach (DataRow dr in dt.Rows)
            {
                dr1 = dt1.NewRow();
                dr1["InvNo"] = Convert.ToString(dr["InvNo"]);
                string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=";
                dr1["Link"] = url + Convert.ToString(dr["InvNo"]);
                dr1["EntryDate"] = Convert.ToDateTime(dr["EntryDate"]).ToString("dd/MM/yyyy");
                dr1["ProductName"] = Convert.ToString(dr["ProductName"]);
                dr1["Quantity"] = Convert.ToString(dr["Quantity"]);// + " " + Convert.ToString(dr["UnitType"]);
                dr1["UnitCost"] = Convert.ToString(dr["UnitCost"]);
                dr1["UnitWeight"] = Convert.ToString(dr["UnitWeight"]);
                dr1["ItemTotal"] = Convert.ToString(dr["ItemTotal"]);
                dr1["TotalWeight"] = Convert.ToString(dr["TotalWeight"]);

                dr1["VAT"] = Convert.ToDecimal(Convert.ToString(dr["VAT"])) - Convert.ToDecimal(Convert.ToString(dr["ItemTotal"]));
                dr1["TotalAmt"] = Convert.ToString(dr["VAT"]);
                dt1.Rows.Add(dr1);
            }

            DataTableReader dr2 = dt1.CreateDataReader();


            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.Sales_Item);
            
            rpt.Load(Server.MapPath("CrptSalebyBrand.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            string rptName = SQLQuery.ReturnString("SELECT [Company] FROM [Party] WHERE PartyID='" + cust + "'");
            string rptBrand = SQLQuery.ReturnString("SELECT [BrandName] FROM [CustomerBrands] WHERE BrandID='" + brand + "'");
            string rptSize = SQLQuery.ReturnString("SELECT [BrandName] FROM [Brands] WHERE BrandID='" + size + "'");
            string rptGread = SQLQuery.ReturnString("SELECT [GradeName] FROM [ItemGrade] WHERE GradeID='" + grade + "'");
            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
            rpt.SetParameterValue("@rptbName", rptBrand);
            rpt.SetParameterValue("@rptSNames", rptSize);
            rpt.SetParameterValue("@rptGName", rptGread);
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