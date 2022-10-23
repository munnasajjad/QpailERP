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
    public partial class Requisition : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();
        }

        ReportDocument rpt = new ReportDocument();
        private void LoadGridData()
        {
            string lName = Page.User.Identity.Name.ToString();
            string prjId = "1";
            string reqId = Convert.ToString(Request.QueryString["ReqNo"]);
            if (reqId != "")
            {
                XerpDataSet dsx = new XerpDataSet();

                SqlCommand cmd = new SqlCommand(@"SELECT Id, ReqId, (SELECT Date FROM Requisition WHERE Id=RequisitionDetails.ReqId) AS Date, (SELECT ReqNo FROM Requisition WHERE Id=RequisitionDetails.ReqId) AS ReqNo, (SELECT Purpose FROM Purpose WHERE (Pid = RequisitionDetails.Purpose)) AS Purpose, ItemName, Pcs, Qty, Price, CountryOfOrigin, PackSize, Warrenty, SerialNo, ModelNo, Specification, UnitType, SizeRef, StockType, AvailableQty, 
                         LastPurDate, LastPurQty, LastPurPrice, Status, (SELECT Remarks FROM Requisition WHERE Id=RequisitionDetails.ReqId) AS Remarks, EntryBy FROM RequisitionDetails WHERE ReqId=(SELECT Id FROM Requisition WHERE ReqNo='" + reqId + "') ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
                cmd.Connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                dsx.Load(dr, LoadOption.OverwriteChanges, dsx.RequisitionDetails);
                cmd.Connection.Close();

                rpt.Load(Server.MapPath("CrptRequisition.rpt"));

                //string datefield = "As on :" + Convert.ToDateTime(dateFrom).ToString(" dd/MM/yyyy");
                //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
                rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
                //rpt.SetParameterValue("@date", datefield);
                //rpt.SetParameterValue("@rptName", rptName);
                CrystalReportViewer1.ReportSource = rpt;
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