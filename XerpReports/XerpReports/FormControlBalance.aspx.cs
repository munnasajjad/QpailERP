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
    public partial class FormControlBalance : System.Web.UI.Page
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
            //decimal total2 = (decimal)0.0;
            //decimal qty2 = (decimal)0.0;
            //decimal vat2 = (decimal)0.0;
            //decimal TotalSales2 = (decimal)0.0;
            //decimal TotalWeight2 = (decimal)0.0;

            string lName = Page.User.Identity.Name.ToString();
            string prjID = "1";
            string item = Convert.ToString(Request.QueryString["parties"]);
            string dateFrom = Convert.ToString(Request.QueryString["dt1"]);

            DataTable dtx = GetData(item, dateFrom);

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.AccSummery);

            rpt.Load(Server.MapPath("CrptControlBalance.rpt"));

            string datefield = "As on : " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy");
            string rptName = SQLQuery.ReturnString("SELECT ControlAccountsName FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;

        }

        private DataTable GetData(string ddParties, string dateFrom)
        {
            try
            {/*string qry = " FROM VoucherDetails WHERE(AccountsHeadID = HeadSetup.AccountsHeadID) AND VoucherNo IN (Select VoucherNo from VoucherMaster where  (VoucherDate <= '" + Convert.ToDateTime(dtpDateTo).ToString("yyyy-MM-dd") + "')  AND Voucherpost<>'C') ";
                DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT AccountsHeadID, AccountsHeadName AS HEAD,
                             (SELECT ISNULL(SUM(VoucherDR), 0)-ISNULL(SUM(VoucherCR), 0) " + qry + @") + OpBalDr AS Balance                             
                             FROM HeadSetup WHERE (ControlAccountsID = '" + ddParties + @"') ORDER BY HEAD");

                string isLiability = ddParties.Substring(0, 2);
                if (isLiability == "02")
                {
                    dtx = SQLQuery.ReturnDataTable(@"SELECT AccountsHeadID, AccountsHeadName AS HEAD,
                             (SELECT ISNULL(SUM(VoucherCR), 0)-ISNULL(SUM(VoucherDR), 0) " + qry + @") + OpBalCr AS Balance                             
                             FROM HeadSetup WHERE (ControlAccountsID = '" + ddParties + @"') ORDER BY HEAD");
                }

                return dtx;*/

                string dtQuery =
                    @"SELECT DISTINCT AccountsHeadID, AccountsHeadName as HEAD, ISNULL(SUM(OpBalDr), 0) - ISNULL(SUM(OpBalCr), 0) +
                             (SELECT        ISNULL(SUM(VoucherDR), 0) - ISNULL(SUM(VoucherCR), 0) 
                               FROM            VoucherDetails
                               WHERE        (AccountsHeadID = HeadSetup.AccountsHeadID) AND (EntryDate <= '"+ Convert.ToDateTime(dateFrom).ToString("yyyy - MM - dd") + @"') AND (ISApproved <> 'C')) AS  Balance, '0' as Qty, '0' as Rate
FROM            HeadSetup
WHERE        (ControlAccountsID = '040101')
GROUP BY AccountsHeadID, AccountsHeadName, EntryID
ORDER BY AccountsHeadName";

                //decimal opBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT   isnull(sum(OpBalDr),0)-isnull(sum(OpBalCr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID]  IN (Select AccountsHeadID from HeadSetup where ControlAccountsID= '" + ddParties + "'))"));
                /*string opBal =" (SELECT   isnull(sum(OpBalDr),0)-isnull(sum(OpBalCr),0)  FROM [HeadSetup] WHERE [AccountsHeadID]=VoucherDetails.AccountsHeadID) ";
                string query = opBal + " + ISNULL(SUM([VoucherDR]),0) - ISNULL(SUM([VoucherCR]),0)";

                string opQty = " (SELECT   isnull(sum(QtyDr),0)-isnull(sum(QtyCr),0)  FROM [HeadSetup] WHERE [AccountsHeadID]=VoucherDetails.AccountsHeadID) ";
                string query2 = opQty+ " + ISNULL(SUM([InQty]),0) - ISNULL(SUM([OutQty]),0)";*/

                string isLiability = ddParties.Substring(0, 2);
                if (isLiability == "02")
                {
                    //opBal =" (SELECT   isnull(sum(OpBalCr),0)-isnull(sum(OpBalDr),0)  FROM [HeadSetup] WHERE [AccountsHeadID]=VoucherDetails.AccountsHeadID) ";
                    //query = opBal + " + ISNULL(SUM([VoucherCR]),0) - ISNULL(SUM([VoucherDR]),0)";

                    //opQty = " (SELECT   isnull(sum(QtyCr),0)-isnull(sum(QtyDr),0)  FROM [HeadSetup] WHERE [AccountsHeadID]=VoucherDetails.AccountsHeadID) ";
                    //query2 = opQty+ " + ISNULL(SUM([OutQty]),0) - ISNULL(SUM([InQty]),0)";


                     dtQuery =
                        @"SELECT DISTINCT AccountsHeadID, AccountsHeadName as HEAD, ISNULL(SUM(OpBalCr), 0) - ISNULL(SUM(OpBalDr), 0) +
                             (SELECT        ISNULL(SUM(VoucherCR), 0) - ISNULL(SUM(VoucherDR), 0) 
                               FROM            VoucherDetails
                               WHERE        (AccountsHeadID = HeadSetup.AccountsHeadID) AND (EntryDate <= '"+ Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + @"') AND (ISApproved <> 'C')) AS Balance, '0' as Qty, '0' as Rate
FROM            HeadSetup
WHERE        (ControlAccountsID = '"+ddParties+@"')
GROUP BY AccountsHeadID, AccountsHeadName, EntryID
ORDER BY AccountsHeadName";
                }


                DataTable dtx = SQLQuery.ReturnDataTable(dtQuery);
                   // @"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, " +
                  //    query + "As Balance," + query2 + " as Qty  FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" +
                 //     ddParties + @"'))  AND  (EntryDate <= '" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") +"')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");


                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("HEAD", typeof(string)));
                dt1.Columns.Add(new DataColumn("Qty", typeof(string)));
                dt1.Columns.Add(new DataColumn("Balance", typeof(string)));
                dt1.Columns.Add(new DataColumn("Rate", typeof(string)));

                foreach (DataRow drx in dtx.Rows)
                {
                    decimal qty = Convert.ToDecimal(drx["Qty"].ToString());

                    dr1 = dt1.NewRow();
                    dr1["HEAD"] = drx["HEAD"].ToString();
                    dr1["Qty"] = qty;
                    dr1["Balance"] = drx["Balance"].ToString();
                    dr1["Rate"] = "0";

                    if (qty > 0)
                    {
                        dr1["Rate"] = Convert.ToString(Convert.ToDecimal(drx["Balance"].ToString()) / qty);
                    }
                    dt1.Rows.Add(dr1);
                }


                return dt1;
            }
            catch (Exception ex)
            {
                return null;
                //Notify(ex.Message.ToString(), "error", lblMsg);
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
