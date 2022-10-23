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
    public partial class frmSalesStatement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ////if (!IsPostBack)
            ////{
             
            LoadGridData();

            //string invNo = Convert.ToString(Request.QueryString["df"]);
            //string chlNo = Convert.ToString(Request.QueryString["dt"]);
            //string pid = Convert.ToString(Request.QueryString["pid"]);
            //if (pid!=null)
            //{
            //    pid = " CustomerID='" + pid + "' AND ";
            //}

            //if (invNo != "")
            //{
            //    //SqlCommand cmd7 = new SqlCommand(
            //    //       @"SELECT SaleID, InvNo, InvDate, (Select Address from DeliveryPoints where DeliveryPointsID=Sales.DeliveryLocation) AS SalesMode, CustomerID, CustomerName, PONo, PODate, '" + chlNo + @"' AS Period, DeliveryDate, DeliveryTime, DeliveryLocation, TransportDetail, 
            //    //         MaturityDays, OverdueDays, Remarks, InvoiceTotal, VATPercent, VATAmount, PayableAmount, CartonQty, (Select PhoneNo from DeliveryPoints where DeliveryPointsID=Sales.DeliveryLocation) AS ChallanNo, CollectedAmount, DueAmount, ProjectID, 
            //    //         EntryBy, EntryDate, IsActive, OverDueDate, InWords, VatChalNo  FROM Sales WHERE InvNo='" + invNo + "' ",
            //    //       new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
            //    //cmd7.Connection.Open();
            //    //SqlDataReader dr7 = cmd7.ExecuteReader();
            //    XerpDataSet ds = new XerpDataSet();
            //    //ds.Load(dr7, LoadOption.OverwriteChanges, ds.Sales);
            //    //cmd7.Connection.Close();


            //    SqlCommand cmd = new SqlCommand(@"SELECT Id, InvNo, SizeId, ProductID, BrandID, ProductName, UnitCost, Quantity, ItemTotal, VatPercent, VAT, NetAmount, UnitWeight, TotalWeight, QtyPerCarton, 
            //             TotalCarton, UnitType, PerviousDeliveredQty, QtyBalance, ItemChallanNo,
            //             (SELECT Company from Party where PartyID=(Select CustomerID from Sales where InvNo=SaleDetails.InvNo))  as  EntryBy,
            //             (Select InvDate from Sales where InvNo=SaleDetails.InvNo) as EntryDate, ReturnQty
            //             FROM SaleDetails WHERE InvNo IN (Select InvNo from Sales where "+ pid +" InvDate>='" + invNo + "' AND  InvDate<='" + chlNo + "') ORDER BY EntryDate ", new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
            //    cmd.Connection.Open();
            //    SqlDataReader dr = cmd.ExecuteReader();
            //    //Pathshala ds = new Pathshala();
            //    ds.Load(dr, LoadOption.OverwriteChanges, ds.SaleDetails);
            //    cmd.Connection.Close();
            //    //ReportDocument rpt = new ReportDocument();
            //    rpt.Load(Server.MapPath("Sales-Statement.rpt"));

            //    string datefield="From "+ Convert.ToDateTime(invNo).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(chlNo).ToString("dd/MM/yyyy");

            //    //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            //    rpt.SetDataSource(ds); SQLQuery.LoadrptHeader(ds, rpt);
            //    rpt.SetParameterValue("@date", datefield);
            //    CrystalReportViewer1.ReportSource = rpt;
            //    //CrystalReportViewer1.DataBind();


            //    //}
            //}
        }

        ReportDocument rpt = new ReportDocument();

        private void LoadGridData()
        {
            string invNo = Convert.ToString(Request.QueryString["df"]);
            string chlNo = Convert.ToString(Request.QueryString["dt"]);
            string pid = Convert.ToString(Request.QueryString["pid"]);
            if (pid != null)
            {
                pid = " CustomerID='" + pid + "' AND ";
            }

            if (invNo != "")
            {
                //SqlCommand cmd7 = new SqlCommand(
                //       @"SELECT SaleID, InvNo, InvDate, (Select Address from DeliveryPoints where DeliveryPointsID=Sales.DeliveryLocation) AS SalesMode, CustomerID, CustomerName, PONo, PODate, '" + chlNo + @"' AS Period, DeliveryDate, DeliveryTime, DeliveryLocation, TransportDetail, 
                //         MaturityDays, OverdueDays, Remarks, InvoiceTotal, VATPercent, VATAmount, PayableAmount, CartonQty, (Select PhoneNo from DeliveryPoints where DeliveryPointsID=Sales.DeliveryLocation) AS ChallanNo, CollectedAmount, DueAmount, ProjectID, 
                //         EntryBy, EntryDate, IsActive, OverDueDate, InWords, VatChalNo  FROM Sales WHERE InvNo='" + invNo + "' ",
                //       new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
                //cmd7.Connection.Open();
                //SqlDataReader dr7 = cmd7.ExecuteReader();
                XerpDataSet ds = new XerpDataSet();
                //ds.Load(dr7, LoadOption.OverwriteChanges, ds.Sales);
                //cmd7.Connection.Close();


                SqlCommand cmd = new SqlCommand(@"SELECT Id, InvNo, SizeId, ProductID, BrandID, ProductName, UnitCost, Quantity, ItemTotal, VatPercent, VAT, NetAmount, UnitWeight, TotalWeight, QtyPerCarton, 
                         TotalCarton, UnitType, PerviousDeliveredQty, QtyBalance, ItemChallanNo,
                         (SELECT Company from Party where PartyID=(Select CustomerID from Sales where InvNo=SaleDetails.InvNo))  as  EntryBy,
                         (Select InvDate from Sales where InvNo=SaleDetails.InvNo) as EntryDate, ReturnQty
                         FROM SaleDetails WHERE InvNo IN (Select InvNo from Sales where " + pid + " InvDate>='" + invNo + "' AND  InvDate<='" + chlNo + "') ORDER BY EntryDate", new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
                cmd.Connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                //Pathshala ds = new Pathshala();
                ds.Load(dr, LoadOption.OverwriteChanges, ds.SaleDetails);
                cmd.Connection.Close();
                //ReportDocument rpt = new ReportDocument();
                rpt.Load(Server.MapPath("Sales-Statement.rpt"));

                string datefield = "From " + Convert.ToDateTime(invNo).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(chlNo).ToString("dd/MM/yyyy");

                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
                rpt.SetDataSource(ds);
                SQLQuery.LoadrptHeader(ds, rpt);
                rpt.SetParameterValue("@date", datefield);
                CrystalReportViewer1.ReportSource = rpt;
                //CrystalReportViewer1.DataBind();
                //}
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