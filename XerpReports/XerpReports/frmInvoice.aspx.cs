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
    public partial class frmInvoice : System.Web.UI.Page
    {
        ReportDocument rpt = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            string invNo = Convert.ToString(Request.QueryString["inv"]);
            int isIncludingSd = int.Parse(SQLQuery.ReturnString(@"SELECT ISNULL(SUM(SDRate),0) FROM Sales WHERE (InvNo='" + invNo + "')"));
            if (invNo != "")
            {
                SqlCommand cmd7 = new SqlCommand(@"SELECT SaleID, InvNo, InvDate, (Select Address from DeliveryPoints where DeliveryPointsID=Sales.DeliveryLocation) AS SalesMode, CustomerID, CustomerName, PONo, PODate, Period, DeliveryDate, DeliveryTime, DeliveryLocation, TransportDetail, 
                         MaturityDays, OverdueDays, Remarks, InvoiceTotal, SDRate, SDAmount, VATPercent, VATAmount, PayableAmount, CartonQty, (Select PhoneNo from DeliveryPoints where DeliveryPointsID=Sales.DeliveryLocation) AS ChallanNo, CollectedAmount, DueAmount, ProjectID, 
                         EntryBy, EntryDate, IsActive, OverDueDate, InWords, VatChalNo, (Select CompanyName from Company where CompanyID=Sales.InvCompany) as InvCompany, (Select CompanyAddress from Company where CompanyID=Sales.InvCompany) as InvAddress  FROM Sales WHERE InvNo='" + invNo + "' ",
                       new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
                cmd7.Connection.Open();
                SqlDataReader dr7 = cmd7.ExecuteReader();
                XerpDataSet ds = new XerpDataSet();
                ds.Load(dr7, LoadOption.OverwriteChanges, ds.Sales);
                cmd7.Connection.Close();

                SqlCommand cmd = new SqlCommand(@"SELECT Id, InvNo, SizeId, ProductID, BrandID, ProductName, UnitCost, Quantity, ItemTotal, VatPercent, VAT, NetAmount, UnitWeight, TotalWeight, QtyPerCarton, 
                         TotalCarton, UnitType, PerviousDeliveredQty, QtyBalance, ItemChallanNo, EntryBy, EntryDate, ReturnQty,  (Select ProductCode from FinishedProducts WHERE pid=(Select TOP(1) FinishedPID from OrderDetails where OrderID=SaleDetails.InvNo AND SizeId=SaleDetails.SizeId AND  ProductID=SaleDetails.ProductID AND  BrandID=SaleDetails.BrandID)) as ItemCode
                        FROM SaleDetails WHERE InvNo='" + invNo + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
                cmd.Connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                //Pathshala ds = new Pathshala();
                ds.Load(dr, LoadOption.OverwriteChanges, ds.SaleDetails);
                cmd7.Connection.Close();
                rpt.Load(isIncludingSd == 0
                    ? Server.MapPath("rptInvoice.rpt")
                    : Server.MapPath("rptInvoiceWithSupplementaryDuty.rpt"));
                //rpt.Load(Server.MapPath("rptInvoice.rpt"));

                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
                rpt.SetDataSource(ds);
                //SQLQuery.LoadrptHeader(ds, rpt);
                CrystalReportViewer1.ReportSource = rpt;
                //CrystalReportViewer1.DataBind();


                //}

                DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT ItemChallanNo FROM SaleDetails WHERE InvNo='" + invNo + "'");

                foreach (DataRow drx in dtx.Rows)
                {
                    string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmChallan.aspx?inv=" + invNo+"&ch="+ drx["ItemChallanNo"].ToString();
                    challanNo.Text += " <a href='"+url+"' target='_blank'>"+ drx["ItemChallanNo"].ToString()+"</a> ";
                }

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