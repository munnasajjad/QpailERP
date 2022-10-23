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
    public partial class FormItem_Purchase_History : System.Web.UI.Page
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
            string iGroup = Convert.ToString(Request.QueryString["Group"]);
            string iSubGrp = Convert.ToString(Request.QueryString["SubGrp"]);
            string iGrade = Convert.ToString(Request.QueryString["Grade"]);
            string iCategory = Convert.ToString(Request.QueryString["Category"]);
            string iItemName = Convert.ToString(Request.QueryString["ItemName"]);
            string dateFrom = Convert.ToString(Request.QueryString["dt1"]);
            string dateTo = Convert.ToString(Request.QueryString["dt2"]);



            string totalPAmt = "";//SQLQuery.ReturnString("Select ISNULL(SUM(PurchaseTotal),0) from Purchase WHERE BillDate >= '" + dateFrom + "' AND BillDate <= '" + dateTo + "' ");
            string ttlinvdis = "";// SQLQuery.ReturnString("Select ISNULL(SUM(PurchaseDiscount),0) from Purchase WHERE BillDate >= '" + dateFrom + "' AND BillDate <= '" + dateTo + "' ");
            string ttlinvOthExp = "";// SQLQuery.ReturnString("Select ISNULL(SUM(OtherExp),0) from Purchase WHERE BillDate >= '" + dateFrom + "' AND BillDate <= '" + dateTo + "' ");

            DataTable dtx = LoadGridData(iGroup, iSubGrp, iGrade, iCategory, iItemName, dateFrom, dateTo,out totalPAmt,out ttlinvdis,out ttlinvOthExp); //RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.PurchaseHistory);
            
            rpt.Load(Server.MapPath("CrptItem-Purchase-History.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [CategoryName] FROM [ItemSubGroup] WHERE CategoryID='" + isubgro + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            
            rpt.SetParameterValue("@total", totalPAmt);
            rpt.SetParameterValue("@ttlinvdis", ttlinvdis);
            rpt.SetParameterValue("@ttlinvOthExp", ttlinvOthExp);
            CrystalReportViewer1.ReportSource = rpt;

            //rpt.Close();
            //rpt.Clone();
            //rpt.Dispose();
            //rpt = null;
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //return dtx;
        }
       
    private DataTable LoadGridData(string ddGroup, string ddSubGrp, string ddGrade, string ddCategory,  string ddItemName,  string dateFrom, string dateTo, out string totalPAmt, out string ttlinvdis, out string ttlinvOthExp)
        
        {
            try
            {
                decimal total2 = (decimal)0.0;
                decimal qty2 = (decimal)0.0;
                decimal vat2 = (decimal)0.0;
                decimal TotalSales2 = (decimal)0.0;
                decimal TotalWeight2 = (decimal)0.0;

                string lName = Page.User.Identity.Name.ToString();
                string iName = ddItemName;

                //DateTime dtFrom = Convert.ToDateTime(dateFrom);
                //DateTime dtTo = Convert.ToDateTime(dateTo);

                string query = " ";
                string querySUM = " ";

                if (ddSubGrp != "0")//get all sub-group from item group
                {
                    if (ddGrade != "0") //get all grade from item sub-group
                    {
                        if (ddCategory != "0")
                        {
                            if (ddItemName != "0")
                            {
                                query = "  PurchaseDetails.ItemCode  ='" + ddItemName + "'  AND   ";
                            }
                            else
                            {
                                query = "  PurchaseDetails.ItemCode IN  (SELECT ProductID from [Products] where CategoryID  ='" + ddCategory + "')  AND ";
                            }
                        }
                        else
                        {
                            query = "  PurchaseDetails.ItemCode IN (SELECT ProductID from [Products] where CategoryID IN(SELECT CategoryID from [Categories] where GradeID ='" + ddGrade + "'))  AND ";
                        }
                    }
                    else
                    {
                        query = "  PurchaseDetails.ItemCode IN (SELECT ProductID from [Products] where CategoryID IN(SELECT CategoryID from [Categories] where GradeID IN(SELECT GradeID FROM [ItemGrade] where CategoryID='" + ddSubGrp + "')) )  AND ";
                    }
                }
                else if (ddGroup != "0")//get all sub-group from item group
                {
                    query = "  PurchaseDetails.ItemCode IN (SELECT ProductID from [Products] where CategoryID IN(SELECT CategoryID from [Categories] where GradeID IN(SELECT GradeID FROM [ItemGrade] where CategoryID IN(SELECT CategoryID FROM [ItemSubGroup] where GroupID='" + ddGroup + "'))) )  AND ";
                }
                query += "";
                string str = @"SELECT        Purchase.BillNo, Purchase.BillDate, Purchase.SupplierName, Purchase.ChallanNo, Purchase.ChallanDate, PurchaseDetails.ItemName, PurchaseDetails.Manufacturer, PurchaseDetails.CountryOfOrigin, 
                         PurchaseDetails.PackSize, PurchaseDetails.Warrenty, PurchaseDetails.SerialNo, PurchaseDetails.ModelNo, PurchaseDetails.Specification, PurchaseDetails.UnitType, PurchaseDetails.SizeRef, 
                         PurchaseDetails.Qty, PurchaseDetails.Price, PurchaseDetails.SubTotal, PurchaseDetails.ItemDisc, PurchaseDetails.ItemVAT, PurchaseDetails.Total, PurchaseDetails.PriceWithVAT, 
                         PurchaseDetails.PriceWithoutVAT, Purchase.PurchaseDiscount, Purchase.OtherExp, Purchase.PurchaseTotal
                    FROM            PurchaseDetails INNER JOIN
                         Purchase ON Purchase.InvNo = PurchaseDetails.InvNo
                        where " + query + "  Purchase.BillDate >= '" +
                             dateFrom + "' AND Purchase.BillDate <= '" + dateTo +
                             "' ORDER BY Purchase.BillDate, PurchaseDetails.Id ";

                DataTable dt = RunQuery.SQLQuery.ReturnDataTable(str);

                if (query.Length>3)
                {
                    query= " AND invno in (Select invno from purchasedetails where " + query + " invno<>'')";
                }

                 totalPAmt = SQLQuery.ReturnString("Select ISNULL(SUM(Purchase.PurchaseTotal),0) from Purchase  WHERE Purchase.BillDate >= '" + dateFrom + "' AND Purchase.BillDate <= '" + dateTo + "' "+query);
                 ttlinvdis = SQLQuery.ReturnString("Select ISNULL(SUM(Purchase.PurchaseDiscount),0) from  Purchase  WHERE Purchase.BillDate >= '" + dateFrom + "' AND Purchase.BillDate <= '" + dateTo + "'  " + query);
                ttlinvOthExp = SQLQuery.ReturnString("Select ISNULL(SUM(Purchase.OtherExp),0) from Purchase  WHERE Purchase.BillDate >= '" + dateFrom + "' AND Purchase.BillDate <= '" + dateTo + "'  " + query);


                //GVrpt.DataSource = dt;
                //GVrpt.DataBind();

                //ltrQty.Text = qty2.ToString();
                //ltrItemLoad.Text = total2.ToString();
                //ltrTotalVat.Text = vat2.ToString();
                //ltrGTAmt.Text = TotalSales2.ToString();
                //ltrTotalWeight.Text = TotalWeight2.ToString();
                return dt;
            }
            catch (Exception ex)
            {
                //Notify(ex.ToString(), "error", lblMsg);
                totalPAmt = "";
                ttlinvdis = "";
                ttlinvOthExp = "";
                return null;
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