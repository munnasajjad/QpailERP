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
    public partial class FormLcExpenseSummaryReport : System.Web.UI.Page
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
            string lcNo = Convert.ToString(Request.QueryString["LcNo"]);

            string partyId = Convert.ToString(Request.QueryString["PartyId"]);
            string reporType = Convert.ToString(Request.QueryString["Types"]);
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);
            string lcNumber = SQLQuery.ReturnString(@"SELECT LCNo FROM LC WHERE sl ='" + lcNo + "'");
            DataTable dtx = LoadGridData(partyId, lcNo, dateFrom, dateTo, reporType);

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.LcExpenseSummary);

            rpt.Load(Server.MapPath("CrptLCExpenseSummary.rpt"));
            LcExpense(lcNo);

            //string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [Company] FROM [Party] WHERE PartyId='" + partyId + "'");
            //string rptType = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + reporType + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx);
            SQLQuery.LoadrptHeaderNew(dsx, rpt);
            rpt.SetParameterValue("@lcNo", lcNumber);
            //rpt.SetParameterValue("@date", datefield);
            //rpt.SetParameterValue("@rptName", rptName);
            //rpt.SetParameterValue("@rptType", reporType);
            CrystalReportViewer1.ReportSource = rpt;

        }
        private void LcExpense(string lcNo)
        {

          
           //string lcNumber = SQLQuery.ReturnString(@"SELECT LCNo FROM LC WHERE sl ='"+lcNo+"'");

            DataTable lcExpensesDtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT esl, LCno, TypeID, HeadID, Expdate, Amount, Description, EntryBy, EntryDate FROM LC_Expenses Where LCno="+lcNo+"");

            DataTableReader lcExpensesDrx = lcExpensesDtx.CreateDataReader();
            XerpDataSet dsx = new XerpDataSet();
            rpt.SetDataSource(dsx);
            dsx.Load(lcExpensesDrx, LoadOption.OverwriteChanges, dsx.LCExp);

            rpt.SetDataSource(dsx);
            rpt.Subreports["CrptLCExpenses.rpt"].SetDataSource((DataTable)dsx.LCExp);
            CrystalReportViewer1.ReportSource = rpt;
        }
        private DataTable LoadGridData(string partyId,string LcNo, string dtFrom, string dtTo, string type)
        {

            string lcNo = Convert.ToString(Request.QueryString["LcNo"]);
            string customer = " "; string party = " ";
            decimal opBal = 0;
            if (partyId != "---ALL---" && partyId != null)
            {
                customer = " AND HeadID ='" + partyId + "'";
                party = " where PartyID ='" + partyId + "'";
                opBal = Convert.ToDecimal(SQLQuery.ReturnString("Select OpBalance FROM Party " + party));
            }

            string opDate = " "; string invDate = " ";
            if (dtFrom != "")
            {
                try
                {
                    invDate = Convert.ToDateTime(dtFrom).ToString("yyyy-MM-dd");
                    opDate = " AND  (TrDate < '" + invDate + "')";
                    invDate = " AND  (TrDate >= '" + invDate + "')";
                }
                catch (Exception) { }
            }

            string closeDate = " ";
            if (dtTo != "")
            {
                try
                {
                    closeDate = Convert.ToDateTime(dtTo).ToString("yyyy-MM-dd");
                    closeDate = " AND  (TrDate <= '" + closeDate + "')";
                }
                catch (Exception) { }
            }

            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("LcMargin", typeof(string)));
            dt1.Columns.Add(new DataColumn("ActualInsuranceAmt", typeof(string)));
            dt1.Columns.Add(new DataColumn("TotalDutyCalculated", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("TotalCNF", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("TotalTransportAmt", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("LTR", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Unload", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("OpeningBankCharge", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("TotalAmount", typeof(decimal)));
            

            int recordcount = 0;
            int ic = 0;
            

            decimal debt = 0; decimal credit = 0; decimal totalQty = 0; decimal totalBag = 0;
            string date; string description; string item = "";
            string dateFrom = Convert.ToDateTime(dtFrom).ToString("yyyy-MM-dd");
            string dateTo = Convert.ToDateTime(dtTo).ToString("yyyy-MM-dd");

            ////get opening balance        
            //string query = " where TrType='Supplier' " + customer + opDate;
            //decimal currBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + query));

            //query = " where TrType='Supplier' " + customer + closeDate;
            //decimal closeBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + query));


            //dr1 = dt1.NewRow();
            //dr1["LCNumber"] = "";
            //dr1["Item"] = "";
            //dr1["QtyInKg"] = 0;
            //dr1["TotalBag"] = 0;
            //dr1["TotalTruck"] = 0;
            //dr1["Rate"] = 0;
            //dr1["TotalAmount"] = 0;
            //dr1["Description"] = "";
            //dt1.Rows.Add(dr1);
            //query = " where  TrType = 'Supplier'  " + customer + invDate + closeDate;

            SqlCommand cmd2 = new SqlCommand(@"SELECT LC.sl, LC.LCNo, LC.LCType, LC.OpenDate, LC.LcMargin, LC.LTR, LC.OpeningBankCharge, LC_Insur_Calc.ActualInsuranceAmt, LC_Duty_Calc.TotalDutyCalculated, LC_CNF.TotalAmount AS TotalCNF, LC_Transport.TotalTransportAmt, 
                        (SELECT ISNULL(SUM(Amount),0) FROM LC_Expenses WHERE (LCno= LC.sl) AND (HeadID = '37')) AS Unload
FROM            LC INNER JOIN
                         LC_Insur_Calc ON LC.LCNo = LC_Insur_Calc.LCNo INNER JOIN
                         LC_Duty_Calc ON LC.LCNo = LC_Duty_Calc.LCNo INNER JOIN
                         LC_CNF ON LC.sl = LC_CNF.LCNo INNER JOIN
                         LC_Transport ON LC.sl = LC_Transport.LCNo 
                              
WHERE        (LC.sl = '" + LcNo+"')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            //SqlCommand cmd2 = new SqlCommand(@"SELECT LCT.TransportID, LC.sl, LC.LCNo, LCT.PartyID, LC.TotalQty, LCT.TotalTruck, LCT.Rate, LCT.TotalTransportAmt, LCT.Description, LCT.PaidStatus, LCT.EntryBy, LCT.EntryDate
            //        FROM LC_Transport AS LCT INNER JOIN
            //             LC ON LCT.LCNo = LC.sl WHERE LCT.PartyID='" + partyId + "'  ORDER BY LCT.TransportID ASC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            ////cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = cust;
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
                    //date = Convert.ToDateTime(ds.Tables[0].Rows[ic]["TrDate"].ToString()).ToString("dd/MM/yyyy");
                    //string lcNumber = ds.Tables[0].Rows[ic]["LCNo"].ToString();
                    
                    //currBal = debt - credit + currBal;


                    //string trGroup = ds.Tables[0].Rows[ic]["LCNo"].ToString();
                    
                    string link = "#";
                    // if (trGroup != "")

                    //link = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=" + inv;
                    //DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1) LcItems.EntryID, LcItems.LCNo, LcItems.LCType, LcItems.ReceiveDate, LcItems.Purpose, LcItems.GradeId, LcItems.CategoryId, LcItems.ItemCode, LcItems.HSCode, LcItems.ItemSizeID, LcItems.CountryOfOrigin, 
                    // LcItems.Manufacturer, LcItems.pcs, LcItems.NoOfPacks, LcItems.QntyPerPack, LcItems.Spec, LcItems.Thickness, LcItems.Measurement, LcItems.FullDescription, LcItems.qty, LcItems.ShortageQty, LcItems.UnitPrice, 
                    // LcItems.CFRValue, LcItems.ReturnQty, LcItems.Loading, LcItems.Loaded, LcItems.LandingPercent, LcItems.LandingAmt, LcItems.TotalUSD, LcItems.TotalBDT, LcItems.UnitCostBDT, LcItems.EntryBy, LcItems.EntryDate, 
                    // LcItems.UsedQty, Products.ItemName, Products.UnitType FROM LcItems INNER JOIN
                    // Products ON LcItems.ItemCode = Products.ProductID  WHERE LcItems.LCNo ='" + lcNo + "'  ORDER BY LcItems.EntryID");
                    //string pName = "";
                    //foreach (DataRow drx in dtx.Rows)
                    //{

                    //    //pName += drx["ProductName"] + " - " + drx["UnitType"] + " <b>" + drx["Quantity"] + "</b> X দর <b>" + Math.Round(Convert.ToDecimal(drx["UnitCost"]), 0) + "</b> " + "= <b>" + SQLQuery.FormatBdNumberRount(Convert.ToDecimal(drx["Price"])) + "</b> <b>৳</b><br>";
                    //    pName += drx["ItemName"];
                    //    if (drx["UnitType"].ToString() == "KG")
                    //    {
                    //        totalQty = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalQty"].ToString()) / 1000M;
                    //        totalBag = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalQty"].ToString()) / 25M;
                    //    }
                    //    else
                    //    {
                    //        totalQty = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalQty"].ToString());
                    //        totalBag = 0;
                    //    }
                    //}
                    //item = pName;
                   // string amt = RunQuery.SQLQuery.ReturnString(@"SELECT  SUM(Amount) as Amount FROM LC_Expenses Where LCno=" + lcNo + "");
                    decimal margin = Convert.ToDecimal(ds.Tables[0].Rows[ic]["LcMargin"].ToString());
                    decimal insurance = Convert.ToDecimal(ds.Tables[0].Rows[ic]["ActualInsuranceAmt"].ToString());
                    decimal duty = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalDutyCalculated"].ToString());
                    decimal cnfCommission = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalCNF"].ToString());
                    decimal transport = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalTransportAmt"].ToString());
                    decimal ltr = Convert.ToDecimal(ds.Tables[0].Rows[ic]["LTR"].ToString());
                    decimal unload = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Unload"].ToString());
                    decimal openingBankCrg = Convert.ToDecimal(ds.Tables[0].Rows[ic]["OpeningBankCharge"].ToString());
                    decimal totalAmount = margin + insurance + duty + cnfCommission + transport + ltr + unload+ openingBankCrg;
                    
                    //description = ds.Tables[0].Rows[ic]["Description"].ToString();


                    dr1 = dt1.NewRow();
                    dr1["LcMargin"] = margin;
                    dr1["ActualInsuranceAmt"] = insurance;
                    dr1["TotalDutyCalculated"] = duty;
                    dr1["TotalCNF"] = cnfCommission;
                    dr1["TotalTransportAmt"] = transport;
                    dr1["LTR"] = ltr;
                    dr1["Unload"] = unload;
                    dr1["OpeningBankCharge"] = openingBankCrg;
                    dr1["TotalAmount"] = totalAmount;
                    
                    
                    dt1.Rows.Add(dr1);
                  
                    //dr1 = dt1.NewRow();
                    //dr1["LCNumber"] = margin;
                    //dr1["Item"] = item;
                    //dr1["TotalQty"] = totalQty;
                    //dr1["TotalBag"] = totalBag;
                    //dr1["TotalTruck"] = totalTruck;
                    //dr1["Rate"] = rate;
                    //dr1["TotalAmount"] = totalTransportAmt;
                    //dr1["Description"] = description;
                    //dt1.Rows.Add(dr1);

                    //if (type == "Matured" && Convert.ToDateTime(lastMaturityDate) > Convert.ToDateTime(date))
                    //{

                    //}
                    //else if (type == "Immatured" && Convert.ToDateTime(lastMaturityDate) < Convert.ToDateTime(date))
                    //{
                    //    dt1.Rows.Add(dr1);
                    //}
                    //else if (type == "---ALL---")
                    //{
                    //    dt1.Rows.Add(dr1);
                    //}
                    ic++;

                } while (ic < recordcount);

            }
            else { }

            //set closing balance    

            //dr1 = dt1.NewRow();
            //dr1["TrDate"] = Convert.ToDateTime(dtTo).ToShortDateString();
            //dr1["Inv"] = "";
            //dr1["Link"] = "";
            //dr1["Description"] = "Closing Balance";
            //dr1["Dr"] = 0;
            //dr1["Cr"] = 0;
            //dr1["Balance"] = 0;
            //dt1.Rows.Add(dr1);

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