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
    public partial class ControlStockSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();
        }

        ReportDocument rpt = new ReportDocument();
        private void LoadGridData()
        {
            
            string lName = Page.User.Identity.Name.ToString();
            string prjId = "1"; decimal currBalKg = 0; decimal currBalPcs = 0;
            string item = Convert.ToString(Request.QueryString["item"]);
            string dateFrom = Convert.ToString(Request.QueryString["dt1"]);
            string dateTo = Convert.ToString(Request.QueryString["dt2"]);
           
            string qry = " FROM VoucherDetails WHERE(AccountsHeadID = HeadSetup.AccountsHeadID) AND VoucherNo IN (Select VoucherNo from VoucherMaster where  (VoucherDate >= '" + dateFrom + "')  AND  (VoucherDate <= '" + dateTo + "')  AND Voucherpost<>'C')";
            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT AccountsHeadID, AccountsHeadName AS HEAD,
                             (SELECT ISNULL(SUM(InQty), 0) " + qry + @") AS Dr,
                             (SELECT ISNULL(SUM(OutQty), 0) " + qry + @") AS Cr,
                             (SELECT ISNULL(SUM(InPcs), 0) " + qry + @") AS InPcs,
                             (SELECT ISNULL(SUM(OutPcs), 0) " + qry + @") AS OutPcs
                             FROM HeadSetup WHERE (ControlAccountsID = '" + item + @"') ORDER BY HEAD");


            DataTable dt1 = new DataTable();
            DataRow dr1 = dt1.NewRow();
            dt1.Columns.Add(new DataColumn("HeadID", typeof(string)));
            dt1.Columns.Add(new DataColumn("HEAD", typeof(string)));
            dt1.Columns.Add(new DataColumn("Dr", typeof(string)));
            dt1.Columns.Add(new DataColumn("Cr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));

            dt1.Columns.Add(new DataColumn("InPcs", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("OutPcs", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("BalancePcs", typeof(decimal)));

            foreach (DataRow drx in dtx.Rows)
            {
                currBalKg = Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(InQty),0)- isnull(sum(OutQty),0)+ (SELECT isnull(sum(QtyDr),0)- isnull(sum(QtyCr),0) from HeadSetup where AccountsHeadID='" + drx["AccountsHeadID"] + "') FROM [VoucherDetails] WHERE ([AccountsHeadID] ='" + drx["AccountsHeadID"] + "' AND VoucherNo IN (Select VoucherNo from VoucherMaster where  (VoucherDate <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "')) AND ISApproved='A')"));
                currBalPcs = Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(InPcs),0)-isnull(sum(OutPcs),0) + (SELECT isnull(sum(OpPcsDr),0)-isnull(sum(OpPcsCr),0) from HeadSetup where AccountsHeadID='" + drx["AccountsHeadID"] + "') FROM [VoucherDetails] WHERE ([AccountsHeadID] ='" + drx["AccountsHeadID"] + "' AND VoucherNo IN (Select VoucherNo from VoucherMaster where  (VoucherDate <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "')) AND ISApproved='A')"));

                string headId = drx["AccountsHeadID"].ToString();
                string headName = drx["HEAD"].ToString();
                string dr = drx["Dr"].ToString();
                string cr = drx["Cr"].ToString();

                string inPsc = drx["InPcs"].ToString();
                string outPcs = drx["OutPcs"].ToString();

                //string bal = Convert.ToString(Convert.ToDecimal(dr) + Convert.ToDecimal(currBalKg) - Convert.ToDecimal(cr));
                //string balPcs = Convert.ToString(Convert.ToDecimal(inPsc) + Convert.ToDecimal(currBalPcs) - Convert.ToDecimal(outPcs));
                string isLiability = headId.Substring(0, 2);
                if (isLiability == "02")
                {
                    // bal = Convert.ToString(Convert.ToDecimal(cr) + Convert.ToDecimal(currBalKg) - Convert.ToDecimal(dr));
                    //balPcs = Convert.ToString(Convert.ToDecimal(outPcs) + Convert.ToDecimal(currBalPcs) - Convert.ToDecimal(inPsc));
                    currBalKg = Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(OutQty),0)- isnull(sum(InQty),0) + (SELECT isnull(sum(QtyDr),0)- isnull(sum(QtyCr),0) from HeadSetup where AccountsHeadID='" + drx["AccountsHeadID"] + "') FROM [VoucherDetails] WHERE ([AccountsHeadID] ='" + drx["AccountsHeadID"] + "' AND VoucherNo IN (Select VoucherNo from VoucherMaster where  (VoucherDate <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "')) AND ISApproved='A')"));
                    currBalPcs = Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(OutPcs),0)-isnull(sum(InPcs),0) + (SELECT isnull(sum(OpPcsDr),0)-isnull(sum(OpPcsCr),0) from HeadSetup where AccountsHeadID='" + drx["AccountsHeadID"] + "') FROM [VoucherDetails] WHERE ([AccountsHeadID] ='" + drx["AccountsHeadID"] + "' AND VoucherNo IN (Select VoucherNo from VoucherMaster where  (VoucherDate <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "')) AND ISApproved='A')"));

                }

                dr1 = dt1.NewRow();
                dr1["HeadID"] = headId;
                dr1["HEAD"] = headName;
                dr1["Dr"] = dr.ToString();
                dr1["Cr"] = cr.ToString();
                dr1["Balance"] = currBalKg;

                dr1["InPcs"] = inPsc;
                dr1["OutPcs"] = outPcs;
                dr1["BalancePcs"] = string.Format("{0:N2}", currBalPcs);
                dt1.Rows.Add(dr1);
            }


            DataTableReader dr2 = dt1.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.AccSummery);

            rpt.Load(Server.MapPath("CrptControlStockSummary.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx);
            SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
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