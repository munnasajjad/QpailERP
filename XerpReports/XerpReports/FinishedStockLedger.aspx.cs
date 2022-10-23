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
    public partial class FinishedStockLedger : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();
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
            string prjId = "1";
            string wHouseId = Convert.ToString(Request.QueryString["wHouseId"]);
            string item = Convert.ToString(Request.QueryString["item"]);
            string dateFrom = Convert.ToString(Request.QueryString["dt1"]);
            string dateTo = Convert.ToString(Request.QueryString["dt2"]);
            

            DataTable dtx = dataatble(wHouseId, item, dateFrom, dateTo); //RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=StockDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [StockDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.AccSummery);

            rpt.Load(Server.MapPath("CrptFinishedStockLedger.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            string rptName = SQLQuery.ReturnString("SELECT ProductName FROM [FinishedProducts] WHERE (pid='" + item + "')");
            string rptControl = SQLQuery.ReturnString("SELECT Company FROM [Party] WHERE PartyID IN (SELECT CompanyID FROM FinishedProducts WHERE pid='" + item + "')");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
            rpt.SetParameterValue("@rpCtName", rptControl);
            CrystalReportViewer1.ReportSource = rpt;
        }

        private DataTable dataatble(string wHouseId, string head, string dtFrom, string dtTo)
        {
            SqlDataAdapter da;
            SqlDataReader dr;
            DataSet ds;
            int recordcount = 0;
            int ic = 0;

            string productId = SQLQuery.ReturnString("SELECT ProductID FROM FinishedProducts WHERE pid='" + head + "'");
            string brandId = SQLQuery.ReturnString("SELECT BrandID FROM FinishedProducts WHERE pid='" + head + "'");
            string sizeId = SQLQuery.ReturnString("SELECT SizeID FROM FinishedProducts WHERE pid='" + head + "'");
            //SqlCommand cmd2 = new SqlCommand("SELECT [EntryDate] as TrDate, [VoucherRowDescription] as Description, [InKg] As Dr, [OutKg] As Cr, [VoucherNo] As Balance, InPcs, OutPcs, 0 as BalancePcs FROM [StockDetails] WHERE ([AccountsHeadID] = @HeadName) AND VoucherNo IN (SELECT VoucherNo FROM StockMaster WHERE VoucherDate >= @DateFrom AND VoucherDate <= @DateTo)  AND ISApproved<>'C' ORDER BY EntryDate, [SerialNo] ASC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            SqlCommand cmd2 = new SqlCommand("SELECT EntryID, InvoiceID, EntryType, Purpose, RefNo, ItemGroup, ProductID, ProductName As Description, ItemType, Customer, BrandID, SizeID, Color, Spec, Description, WarehouseID, LocationID, InQuantity As InPcs, OutQuantity As OutPcs, 0 as BalancePcs, InWeight As Dr, OutWeight As Cr, Price As Balance, ItemSerialNo, Remark, Status, StockLocation, EntryBy, EntryDate As TrDate FROM Stock WHERE([ProductID] = '" + productId + "') AND ([BrandID] = '" + brandId + "') AND ([SizeID] = '" + sizeId + "') AND ([WarehouseID] = '" + wHouseId + "') AND ([ItemGroup] ='2') AND (EntryDate >= @DateFrom AND EntryDate <= @DateTo)  ORDER BY EntryDate, [EntryID] ASC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
            cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
            cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(dtTo).ToShortDateString();

            da = new SqlDataAdapter(cmd2);
            ds = new DataSet("Board");

            cmd2.Connection.Open();
            da.Fill(ds, "Board");
            recordcount = ds.Tables[0].Rows.Count;

            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("TrDate", typeof(string)));
            dt1.Columns.Add(new DataColumn("Description", typeof(string)));
            dt1.Columns.Add(new DataColumn("Dr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Cr", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));

            dt1.Columns.Add(new DataColumn("InPcs", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("OutPcs", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("BalancePcs", typeof(decimal)));
            cmd2.Connection.Close();

            decimal debt = 0; decimal credit = 0; decimal currBal = 0; decimal debtPcs = 0; decimal creditPcs = 0; decimal currBalPcs = 0;
            string date; string description;

            //Check if the head is liability head
            //string isLiability = head.Substring(0, 2);
            //if (isLiability == "02")
            //{
                //decimal opBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(OpBalCr),0) - isnull(sum(OpBalDr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID] = '" + head + "')"));
                //decimal preBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(VoucherCR),0) - isnull(sum(VoucherDR),0) FROM [StockDetails] WHERE ([AccountsHeadID] = '" + head + "') AND   EntryDate < '" + Convert.ToDateTime(dtFrom).ToString("yyyy-MM-dd") + "'  AND ISApproved='A'"));
                //currBal = opBal + preBal;

                //dr1 = dt1.NewRow();
                //dr1["TrDate"] = Convert.ToDateTime(dtFrom).ToShortDateString();
                //dr1["Description"] = "Openning Balance";
                //dr1["Dr"] = 0;
                //dr1["Cr"] = 0;
                //dr1["Balance"] = string.Format("{0:N2}", currBal);
                //dt1.Rows.Add(dr1);

                //if (recordcount > 0)
                //{
                //    do
                //    {
                //        date = Convert.ToDateTime(ds.Tables[0].Rows[ic]["TrDate"].ToString()).ToShortDateString();
                //        description = ds.Tables[0].Rows[ic]["Description"].ToString();
                //        debt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Dr"].ToString());
                //        credit = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Cr"].ToString());
                //        currBal = credit - debt + currBal;
                //        dr1 = dt1.NewRow();
                //        dr1["TrDate"] = date;
                //        dr1["Description"] = description;
                //        dr1["Dr"] = debt;
                //        dr1["Cr"] = credit;
                //        dr1["Balance"] = string.Format("{0:N2}", currBal);
                //        dt1.Rows.Add(dr1);
                //        ic++;

                //    } while (ic < recordcount);
                //}
                //else
                //{
                //    //GridView1.DataSource = null;
                //}

                ////get closing balance
                //currBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT isnull(sum(VoucherCR),0) - isnull(sum(VoucherDR),0) FROM [StockDetails] WHERE ([AccountsHeadID] = '" + head + "') AND EntryDate < '" + Convert.ToDateTime(dtTo).AddDays(+1).ToString("yyyy-MM-dd") + "' AND ISApproved='A'"));

                //dr1 = dt1.NewRow();
                //dr1["TrDate"] = Convert.ToDateTime(dtTo).ToShortDateString();
                //dr1["Description"] = "Closing Balance";
                //dr1["Dr"] = 0;
                //dr1["Cr"] = 0;
                //dr1["Balance"] = string.Format("{0:N2}", (currBal + opBal));
                //dt1.Rows.Add(dr1);
            //}
            //else
            {
                //get openning balance Kg
                productId = SQLQuery.ReturnString("SELECT ProductID FROM FinishedProducts WHERE pid='"+ head + "'");
                brandId = SQLQuery.ReturnString("SELECT BrandID FROM FinishedProducts WHERE pid='" + head + "'");
                sizeId = SQLQuery.ReturnString("SELECT SizeID FROM FinishedProducts WHERE pid='" + head + "'");

                SqlCommand cmd2X = new SqlCommand("SELECT ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) FROM [Stock] WHERE ([ProductID] = @ProductID) AND ([BrandID] = @BrandID) AND ([SizeID] = @SizeID) AND ([WarehouseID] = @WarehouseID) AND ([ItemGroup] = '2') AND (EntryDate <= @DateFrom)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd2X.Parameters.Add("@WarehouseID", SqlDbType.VarChar).Value = wHouseId;
                cmd2X.Parameters.Add("@ProductID", SqlDbType.VarChar).Value = productId;
                cmd2X.Parameters.Add("@BrandID", SqlDbType.VarChar).Value = brandId;
                cmd2X.Parameters.Add("@SizeID", SqlDbType.VarChar).Value = sizeId;
                cmd2X.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
                cmd2X.Connection.Open();
                decimal opBal = Convert.ToDecimal(cmd2X.ExecuteScalar());
                cmd2X.Connection.Close();

                ////SqlCommand cmd2Y = new SqlCommand("SELECT isnull(sum(VoucherDR),0) - isnull(sum(VoucherCR),0) FROM [StockDetails] WHERE ([AccountsHeadID] = @HeadName) AND EntryDate < @DateFrom  AND ISApproved='A'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //SqlCommand cmd2Y = new SqlCommand("SELECT ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) FROM [Stock] WHERE ([ProductID] = '" + productId + "') AND ([BrandID] = '" + brandId + "') AND ([SizeID] = '" + sizeId + "') AND ([WarehouseID] = '" + wHouseId + "') AND ([ItemGroup] = '2') AND (EntryDate < @DateFrom)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                ////cmd2X.Parameters.Add("@ProductID", SqlDbType.VarChar).Value = productId;
                ////cmd2X.Parameters.Add("@BrandID", SqlDbType.VarChar).Value = brandId;
                ////cmd2X.Parameters.Add("@SizeID", SqlDbType.VarChar).Value = sizeId;
                //cmd2Y.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
                //cmd2Y.Connection.Open();
                //decimal preBal = Convert.ToDecimal(cmd2Y.ExecuteScalar());
                //cmd2Y.Connection.Close();

                currBal = opBal; //+ preBal;

                //get openning balance Pcs
                SqlCommand cmd2R = new SqlCommand("SELECT ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) FROM [Stock] WHERE ([ProductID] = '" + productId + "') AND ([BrandID] = '" + brandId + "') AND ([SizeID] = '" + sizeId + "') AND ([WarehouseID] = '" + wHouseId + "') AND ([ItemGroup] ='2') AND (EntryDate <= @DateFrom)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //cmd2X.Parameters.Add("@ProductID", SqlDbType.VarChar).Value = productId;
                //cmd2X.Parameters.Add("@BrandID", SqlDbType.VarChar).Value = brandId;
                //cmd2X.Parameters.Add("@SizeID", SqlDbType.VarChar).Value = sizeId;
                cmd2R.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
                cmd2R.Connection.Open();
                decimal opBalPcs = Convert.ToDecimal(cmd2R.ExecuteScalar());
                cmd2R.Connection.Close();

                ////SqlCommand cmd2Y = new SqlCommand("SELECT isnull(sum(VoucherDR),0) - isnull(sum(VoucherCR),0) FROM [StockDetails] WHERE ([AccountsHeadID] = @HeadName) AND EntryDate < @DateFrom  AND ISApproved='A'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //SqlCommand cmd2M = new SqlCommand("SELECT ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) FROM [Stock] WHERE ([ProductID] = '" + productId + "') AND ([BrandID] = '" + brandId + "') AND ([SizeID] = '" + sizeId + "') AND ([WarehouseID] = '" + wHouseId + "') AND ItemGroup='2' AND (EntryDate < @DateFrom)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                ////cmd2M.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
                //cmd2M.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
                //cmd2M.Connection.Open();
                //decimal preBalPcs = Convert.ToDecimal(cmd2M.ExecuteScalar());
                //cmd2M.Connection.Close();

                currBalPcs = opBalPcs;// + preBalPcs;

                dr1 = dt1.NewRow();
                dr1["TrDate"] = Convert.ToDateTime(dtFrom).ToShortDateString();
                dr1["Description"] = "Openning Balance";
                dr1["Dr"] = 0;
                dr1["Cr"] = 0;
                dr1["Balance"] = string.Format("{0:N2}", currBal);

                dr1["InPcs"] = 0;
                dr1["OutPcs"] = 0;
                dr1["BalancePcs"] = string.Format("{0:N2}", currBalPcs);
                dt1.Rows.Add(dr1);

                if (recordcount > 0)
                {
                    do
                    {
                        date = Convert.ToDateTime(ds.Tables[0].Rows[ic]["TrDate"].ToString()).ToShortDateString();
                        description = ds.Tables[0].Rows[ic]["Description"].ToString();
                        debt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Dr"].ToString());
                        credit = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Cr"].ToString());
                        currBal = debt - credit + currBal;
                        //object debit2 = dt1.Compute("Sum(Dr)", "");
                        //object credit2 = dt1.Compute("Sum(Cr)", "");
                        //currBal = Convert.ToDecimal(debit2) - Convert.ToDecimal(credit2);

                        debtPcs = Convert.ToDecimal(ds.Tables[0].Rows[ic]["InPcs"].ToString());
                        creditPcs = Convert.ToDecimal(ds.Tables[0].Rows[ic]["OutPcs"].ToString());
                        currBalPcs = debtPcs - creditPcs + currBalPcs;

                        dr1 = dt1.NewRow();
                        dr1["TrDate"] = date;
                        dr1["Description"] = description;
                        dr1["Dr"] = debt;
                        dr1["Cr"] = credit;
                        dr1["Balance"] = string.Format("{0:N2}", currBal);

                        dr1["InPcs"] = debtPcs;
                        dr1["OutPcs"] = creditPcs;
                        dr1["BalancePcs"] = string.Format("{0:N2}", currBalPcs);

                        dt1.Rows.Add(dr1);
                        ic++;

                    } while (ic < recordcount);

                }
                else
                {
                    //GridView1.DataSource = null;
                }

                //get closing balance Kg.     
                SqlCommand cmd2Z = new SqlCommand("SELECT ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) as balance FROM [Stock] WHERE ([ProductID] = '" + productId + "') AND ([BrandID] = '" + brandId + "') AND ([SizeID] = '" + sizeId + "') AND ([WarehouseID] = '" + wHouseId + "') AND ([ItemGroup] = '2') AND (EntryDate <= @DateTo)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //cmd2Z.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
                cmd2Z.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(dtTo).AddDays(+1).ToShortDateString();
                cmd2Z.Connection.Open();
                currBal = Convert.ToDecimal(cmd2Z.ExecuteScalar());
                cmd2Z.Connection.Close();

                //get closing balance Pcs.
                SqlCommand cmd2Zz = new SqlCommand("SELECT ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) as balance FROM [Stock] WHERE ([ProductID] = '" + productId + "') AND ([BrandID] = '" + brandId + "') AND ([SizeID] = '" + sizeId + "') AND ([WarehouseID] = '" + wHouseId + "') AND ([ItemGroup] = '2') AND (EntryDate <= @DateTo)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //cmd2Zz.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
                cmd2Zz.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(dtTo).AddDays(+1).ToShortDateString();
                cmd2Zz.Connection.Open();
                currBalPcs = Convert.ToDecimal(cmd2Zz.ExecuteScalar());
                cmd2Z.Connection.Close();

                dr1 = dt1.NewRow();
                dr1["TrDate"] = Convert.ToDateTime(dtTo).ToShortDateString();
                dr1["Description"] = "Closing Balance";
                dr1["Dr"] = 0;
                dr1["Cr"] = 0;
                //dr1["Balance"] = string.Format("{0:N2}", (currBal + opBal));
                dr1["Balance"] = string.Format("{0:N2}", (currBal));

                dr1["InPcs"] = 0;
                dr1["OutPcs"] = 0;
                //dr1["BalancePcs"] = string.Format("{0:N2}", (currBalPcs + opBalPcs));
                dr1["BalancePcs"] = string.Format("{0:N2}", (currBalPcs));
                dt1.Rows.Add(dr1);


                ////get openning balance Pcs
                //SqlCommand cmd2R = new SqlCommand("SELECT isnull(sum(OpeningPcs),0) FROM [Products] WHERE ([ProductID] = @HeadName)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //cmd2R.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
                //cmd2R.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
                //cmd2R.Connection.Open();
                //decimal opBalPcs = Convert.ToDecimal(cmd2R.ExecuteScalar());
                //cmd2R.Connection.Close();

                ////SqlCommand cmd2Y = new SqlCommand("SELECT isnull(sum(VoucherDR),0) - isnull(sum(VoucherCR),0) FROM [StockDetails] WHERE ([AccountsHeadID] = @HeadName) AND EntryDate < @DateFrom  AND ISApproved='A'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //SqlCommand cmd2M = new SqlCommand("SELECT isnull(sum(InPcs),0) - isnull(sum(OutPcs),0) FROM [StockDetails] WHERE ([AccountsHeadID] = @HeadName) AND EntryDate < @DateFrom  AND ISApproved='A'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //cmd2M.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
                //cmd2M.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
                //cmd2M.Connection.Open();
                //decimal preBalPcs = Convert.ToDecimal(cmd2M.ExecuteScalar());
                //cmd2M.Connection.Close();

                //currBalPcs = opBalPcs + preBalPcs;

                //dr1 = dt1.NewRow();
                ////dr1["TrDate"] = Convert.ToDateTime(dtFrom).ToShortDateString();
                ////dr1["Description"] = "Openning Balance";
                //dr1["InPcs"] = 0;
                //dr1["OutPcs"] = 0;
                //dr1["BalancePcs"] = string.Format("{0:N2}", currBalPcs);
                //dt1.Rows.Add(dr1);

                //if (recordcount > 0)
                //{
                //    do
                //    {
                //        //date = Convert.ToDateTime(ds.Tables[0].Rows[ic]["TrDate"].ToString()).ToShortDateString();
                //        //description = ds.Tables[0].Rows[ic]["Description"].ToString();
                //        debtPcs = Convert.ToDecimal(ds.Tables[0].Rows[ic]["InPcs"].ToString());
                //        creditPcs = Convert.ToDecimal(ds.Tables[0].Rows[ic]["OutPcs"].ToString());
                //        currBalPcs = debtPcs - creditPcs + currBalPcs;
                //        //object debit2 = dt1.Compute("Sum(Dr)", "");
                //        //object credit2 = dt1.Compute("Sum(Cr)", "");
                //        //currBal = Convert.ToDecimal(debit2) - Convert.ToDecimal(credit2);

                //        dr1 = dt1.NewRow();
                //        //dr1["TrDate"] = date;
                //        //dr1["Description"] = description;
                //        dr1["InPcs"] = debtPcs;
                //        dr1["OutPcs"] = creditPcs;
                //        dr1["BalancePcs"] = string.Format("{0:N2}", currBalPcs);
                //        dt1.Rows.Add(dr1);
                //        ic++;

                //    } while (ic < recordcount);

                //}
                ////get closing balance Pcs       
                //SqlCommand cmd2P = new SqlCommand("SELECT isnull(sum(InPcs),0)-isnull(sum(OutPcs),0) AS balance FROM [StockDetails] WHERE ([AccountsHeadID] = @HeadName) AND VoucherNo IN (SELECT VoucherNo FROM StockMaster WHERE VoucherDate < @DateTo) AND ISApproved='A'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //cmd2P.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = head;
                //cmd2P.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(dtTo).AddDays(+1).ToShortDateString();
                //cmd2P.Connection.Open();
                //currBalPcs = Convert.ToDecimal(cmd2P.ExecuteScalar());
                //cmd2P.Connection.Close();

                //dr1 = dt1.NewRow();
                ////dr1["TrDate"] = Convert.ToDateTime(dtTo).ToShortDateString();
                ////dr1["Description"] = "Closing Balance";
                //dr1["InPcs"] = 0;
                //dr1["OutPcs"] = 0;
                //dr1["BalancePcs"] = string.Format("{0:N2}", (currBalPcs + opBal));
                //dt1.Rows.Add(dr1);
            }

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