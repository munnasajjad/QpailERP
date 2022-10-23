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
    public partial class FormLCExpenseReport : System.Web.UI.Page
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
            string partyId = Convert.ToString(Request.QueryString["PartyId"]);
            string reportType = Convert.ToString(Request.QueryString["ReportType"]);
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);

            DataTable dtx = LoadGridData(partyId, dateFrom, dateTo, reportType);

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.LcExpenseRpt);

            if (reportType=="TRANSPORT")
            {
                rpt.Load(Server.MapPath("CrptLCExpensesReport.rpt"));
            }
            else if (reportType == "CNF")
            {
                rpt.Load(Server.MapPath("CrptLCExpensesCnfReport.rpt"));
            }
            else
            {
                rpt.Load(Server.MapPath("CrptLCExpensesInsuranceReport.rpt"));
            }
            

            //string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [Company] FROM [Party] WHERE PartyId='" + partyId + "'");
            //string rptType = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + reporType + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeaderNew(dsx, rpt);
            //rpt.SetParameterValue("@date", datefield);
            //rpt.SetParameterValue("@rptName", rptName);
            //rpt.SetParameterValue("@rptType", reporType);
            CrystalReportViewer1.ReportSource = rpt;

        }
        private DataTable LoadGridData(string partyId, string dtFrom, string dtTo, string reportType)
        {
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
            dt1.Columns.Add(new DataColumn("LCNumber", typeof(string)));
            dt1.Columns.Add(new DataColumn("Item", typeof(string)));
            dt1.Columns.Add(new DataColumn("TotalQty", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("TotalBag", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("TotalTruck", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Rate", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("TotalAmount", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Description", typeof(string)));

            dt1.Columns.Add(new DataColumn("PortCharge", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("ShipingCharge", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Commission", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Miscellaneous", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("OtherCharge", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("TotalCnf", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("CnfDescription", typeof(string)));

            dt1.Columns.Add(new DataColumn("InsuranceNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("MarineAmt", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("WarSrccAmt", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("VatAmt", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("StampDutyAmount", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("CalculatedInsuranceAmt", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("RebateOnPremium", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("ActualInsuranceAmt", typeof(decimal)));
            

            int recordcount = 0;
            int ic = 0;

            decimal debt = 0; decimal credit = 0; decimal totalQty = 0; decimal totalBag = 0;
            string date; string description;string item="";
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
            //query = " where  LCT.TrType = '" + party + "' AND LCT.TrGroup ='ImportLC'" + customer + invDate + closeDate;

            SqlCommand cmd2 = new SqlCommand();
             if (reportType == "CNF")
            {
                cmd2 = new SqlCommand(@"SELECT        LC.sl, LC.LCNo AS LCNumber, LC.LCItem, LC.TotalQty, Cnf.PartyID, Cnf.PortCharge, Cnf.ShipingCharge, Cnf.ReceiptAmt, Cnf.Miscellaneous, Cnf.OtherCharge, Cnf.Commission, Cnf.TotalAmount AS TotalCnf, 
                         Cnf.Description AS CnfDescription, LC.LCNo AS LCNumber, LC.LCItem AS Item, '0' AS TotalQty, 0 AS TotalBag, 0 AS TotalTruck, 0 AS Rate, 0 AS TotalAmount, '0' AS Description, 0 AS InsuranceNo, 0 AS MarineAmt, 0 AS VatAmt, 0 AS WarSrccAmt,
                         0 AS StampDutyAmount, 0 AS CalculatedInsuranceAmt, 0 AS RebateOnPremium, 0 AS ActualInsuranceAmt
FROM            LC_CNF AS Cnf INNER JOIN
                         LC ON Cnf.LCNo = LC.sl WHERE Cnf.PartyID ='" + partyId + "'ORDER BY  Cnf.PartyID ASC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            }
            else if (reportType == "TRANSPORT")
            {
                 cmd2 = new SqlCommand(@"SELECT LCT.TransportID, LC.sl, LC.LCItem, LC.LCNo AS LCNumber, LCT.PartyID, LC.TotalQty, LCT.TotalTruck, LCT.Rate, LCT.TotalTransportAmt AS TotalAmount, LCT.Description, LCT.PaidStatus, LCT.EntryBy, LCT.EntryDate,
                    0 AS PortCharge, 0 AS ShipingCharge, 0 AS Miscellaneous, 0 AS OtherCharge, 0 AS Commission, 0 AS TotalCnf, '0' AS CnfDescription,
                    0 AS InsuranceNo, 0 AS WarSrccAmt, 0 AS MarineAmt, 0 AS VatAmt, 0 AS StampDutyAmount, 0 AS CalculatedInsuranceAmt, 0 AS RebateOnPremium, 0 AS ActualInsuranceAmt
                    FROM LC_Transport AS LCT INNER JOIN
                         LC ON LCT.LCNo = LC.sl WHERE LCT.PartyID='" + partyId + "'  ORDER BY LCT.PartyID ASC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            }
            
            else
            {
                cmd2 = new SqlCommand(@"SELECT       LC.LCNo AS LCNumber, LC.TotalQty, LC.InsuranceNo, INS.WarSrccAmt, INS.MarineAmt, INS.VatAmt, INS.StampDutyAmount, INS.CalculatedInsuranceAmt, 0 AS RebateOnPremium, INS.ActualInsuranceAmt, LC.LCItem AS Item, 
                         Party.PartyID, 0 AS TotalQty, 0 AS TotalBag, 0 AS TotalTruck, 0 AS Rate, 0 AS TotalAmount, '0' AS Description, 0 AS PortCharge, 0 AS ShipingCharge, 0 AS Miscellaneous, 0 AS OtherCharge, 0 AS Commission, 0 AS TotalCnf, 
                         '0' AS CnfDescription
                    FROM            LC_Insur_Calc AS INS INNER JOIN
                         LC ON INS.LCNo = LC.LCNo INNER JOIN
                         Party ON LC.InsuranceID = Party.PartyID
                    WHERE        (LC.InsuranceID ='" + partyId + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            }

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
                    string lcNumber = ds.Tables[0].Rows[ic]["LCNumber"].ToString();
                    //currBal = debt - credit + currBal;

                    
                    string trGroup = ds.Tables[0].Rows[ic]["LCNumber"].ToString();
                    string link = "#";
                    if (trGroup != "")
                    {
                        //link = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=" + inv;
                        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1) LcItems.EntryID, LcItems.LCNo, LcItems.LCType, LcItems.ReceiveDate, LcItems.Purpose, LcItems.GradeId, LcItems.CategoryId, LcItems.ItemCode, LcItems.HSCode, LcItems.ItemSizeID, LcItems.CountryOfOrigin, 
                         LcItems.Manufacturer, LcItems.pcs, LcItems.NoOfPacks, LcItems.QntyPerPack, LcItems.Spec, LcItems.Thickness, LcItems.Measurement, LcItems.FullDescription, LcItems.qty, LcItems.ShortageQty, LcItems.UnitPrice, 
                         LcItems.CFRValue, LcItems.ReturnQty, LcItems.Loading, LcItems.Loaded, LcItems.LandingPercent, LcItems.LandingAmt, LcItems.TotalUSD, LcItems.TotalBDT, LcItems.UnitCostBDT, LcItems.EntryBy, LcItems.EntryDate, 
                         LcItems.UsedQty, Products.ItemName, Products.UnitType FROM LcItems INNER JOIN
                         Products ON LcItems.ItemCode = Products.ProductID  WHERE LcItems.LCNo ='" + lcNumber + "'  ORDER BY LcItems.EntryID");
                        string pName = "";
                        foreach (DataRow drx in dtx.Rows)
                        {
                            
                            //pName += drx["ProductName"] + " - " + drx["UnitType"] + " <b>" + drx["Quantity"] + "</b> X দর <b>" + Math.Round(Convert.ToDecimal(drx["UnitCost"]), 0) + "</b> " + "= <b>" + SQLQuery.FormatBdNumberRount(Convert.ToDecimal(drx["Price"])) + "</b> <b>৳</b><br>";
                            pName += drx["ItemName"];
                            if (drx["UnitType"].ToString()=="KG")
                            {
                                totalQty = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalQty"].ToString())/1000M;
                                totalBag = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalQty"].ToString()) / 25M;
                            }
                            else
                            {
                                totalQty = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalQty"].ToString());
                                totalBag = 0;
                            }
                        }
                        item = pName;
                    }
                    
                   decimal totalTruck = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalTruck"].ToString());
                   decimal rate = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Rate"].ToString());
                   decimal totalTransportAmt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalAmount"].ToString());
                    description = ds.Tables[0].Rows[ic]["Description"].ToString();
                    

                    decimal portCharge = Convert.ToDecimal(ds.Tables[0].Rows[ic]["PortCharge"].ToString());
                    decimal shipingCharge = Convert.ToDecimal(ds.Tables[0].Rows[ic]["ShipingCharge"].ToString());
                    decimal cnfCommission = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Commission"].ToString());
                    decimal miscellaneous = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Miscellaneous"].ToString());
                    decimal otherCharge = Convert.ToDecimal(ds.Tables[0].Rows[ic]["OtherCharge"].ToString());
                    decimal totalCnf = Convert.ToDecimal(ds.Tables[0].Rows[ic]["TotalCnf"].ToString());
                   string cnfDescription = ds.Tables[0].Rows[ic]["CnfDescription"].ToString();

                    string InsuranceNo = ds.Tables[0].Rows[ic]["InsuranceNo"].ToString();
                    decimal marineAmt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["MarineAmt"].ToString());
                    decimal warSrccAmt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["WarSrccAmt"].ToString());
                    decimal vatAmt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["VatAmt"].ToString());
                    decimal stampDutyAmount = Convert.ToDecimal(ds.Tables[0].Rows[ic]["StampDutyAmount"].ToString());
                    decimal calculatedInsuranceAmt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["CalculatedInsuranceAmt"].ToString());
                    decimal rebateOnPremium = marineAmt/100*75;
                    rebateOnPremium = Convert.ToDecimal(ds.Tables[0].Rows[ic]["RebateOnPremium"].ToString());
                    decimal actualInsuranceAmt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["ActualInsuranceAmt"].ToString());


                    dr1 = dt1.NewRow();
                    dr1["LCNumber"] = lcNumber;
                    dr1["Item"] = item;
                    dr1["TotalQty"] = totalQty;
                    dr1["TotalBag"] = totalBag;
                    dr1["TotalTruck"] = totalTruck;
                    dr1["Rate"] = rate;
                    dr1["TotalAmount"] = totalTransportAmt;
                    dr1["Description"] = description;

                    dr1["PortCharge"] = portCharge;
                    dr1["ShipingCharge"] = shipingCharge;
                    dr1["Commission"] = cnfCommission;
                    dr1["Miscellaneous"] = miscellaneous;
                    dr1["OtherCharge"] = otherCharge;
                    dr1["TotalCnf"] = totalCnf;
                    dr1["CnfDescription"] = cnfDescription;

                    dr1["InsuranceNo"] = InsuranceNo;
                    dr1["MarineAmt"] = marineAmt;
                    dr1["WarSrccAmt"] = warSrccAmt;
                    dr1["VatAmt"] = vatAmt;
                    dr1["StampDutyAmount"] = stampDutyAmount;
                    dr1["CalculatedInsuranceAmt"] = calculatedInsuranceAmt;
                    dr1["RebateOnPremium"] = rebateOnPremium;
                    dr1["ActualInsuranceAmt"] = actualInsuranceAmt;
                    dt1.Rows.Add(dr1);

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