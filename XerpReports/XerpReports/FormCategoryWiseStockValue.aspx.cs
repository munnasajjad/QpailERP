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
    public partial class FormCategoryWiseStockValue : System.Web.UI.Page
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

            string icategory = Convert.ToString(Request.QueryString["category"]);
            string igrade = Convert.ToString(Request.QueryString["grade"]);
            string isubgro = Convert.ToString(Request.QueryString["subgro"]);
            string iType = Convert.ToString(Request.QueryString["type"]);
            string dateFrom = Convert.ToString(Request.QueryString["dt1"]);
            string dateTo = Convert.ToString(Request.QueryString["dt2"]);



            /*  DataTable dtx = BindItemGrid(icategory, igrade, isubgro, iType, dateFrom, dateTo);*/ //RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");
            DataTable dtx = BindItemGrid(isubgro, dateFrom, dateTo); //RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.StockCategoryValue);

            rpt.Load(Server.MapPath("CrptCategoryWiseStockValue.rpt"));

            string datefield = "As on :" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd");
            string rptName = SQLQuery.ReturnString("SELECT [CategoryName] FROM [ItemSubGroup] WHERE CategoryID='" + isubgro + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;


        }

        private DataTable BindItemGrid(string ddSubGroup, string dateForm, string AsOn)
        {
            DataSet ds = new DataSet();
            try
            {

                string subGroup = " ";
                
                if (ddSubGroup != "--- all ---")
                {
                    subGroup = " WHERE MachineStock.ProductID IN (Select ProductID from Products where CategoryID in (SELECT CategoryID FROM Categories WHERE GradeID IN (SELECT GradeID FROM ItemGrade WHERE CategoryID ='" + ddSubGroup + "'))) ";
                }

                string dateFrom = " ";
                if (dateForm != "")
                {
                    dateFrom = " AND MachineStock.EntryDate>='" + Convert.ToDateTime(dateForm).ToString("yyyy-MM-dd") + "' ";
                }

                string dateTo = " ";
                if (AsOn != "")
                {
                    dateTo = " AND MachineStock.EntryDate<='" + Convert.ToDateTime(AsOn).ToString("yyyy-MM-dd") + "' ";
                }


                string query = " FROM MachineStock where EntryId<>0 " + subGroup + dateFrom;
                string url = "";// SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";


                if (ddSubGroup == "10") //printing ink
                {
                    query = " FROM MachineStock where EntryId<>0 " + subGroup + dateTo;
                    ds = SQLQuery.ReturnDataSet(@"SELECT MachineStock.ProductID,  ItemSubGroup.CategoryName, MachineStock.ProductName,  AVG(tblFifo.InValue) AS Price, SUM(MachineStock.InWeight) AS ReceivedQty, SUM(MachineStock.OutWeight) AS IssuedQty, 
                         SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) AS Opening, SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) + SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) AS Closing, 
                        (SUM(MachineStock.InWeight)*tblFifo.InValue) AS ReceivedAmount,  (SUM(MachineStock.OutWeight)*tblFifo.InValue) AS IssuedAmount, ((SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight))*tblFifo.InValue) as OpeningBalance,
						 ((SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) + SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight))*tblFifo.InValue) as ClosingBalance
                         FROM Products INNER JOIN
                         Categories ON Products.CategoryID = Categories.CategoryID INNER JOIN
                         MachineStock INNER JOIN
                         tblFifo ON MachineStock.ProductID = tblFifo.ItemCode ON Products.ProductID = MachineStock.ProductID INNER JOIN
                         ItemGrade ON Categories.GradeID = ItemGrade.GradeID INNER JOIN
                         ItemSubGroup ON ItemGrade.CategoryID = ItemSubGroup.CategoryID 
						 " + subGroup + " " + dateTo + " GROUP BY MachineStock.ProductName, MachineStock.ProductID, tblFifo.InValue, ItemSubGroup.CategoryName");
                }
                else if (ddSubGroup == "33" || ddSubGroup == "35") //HTF & IML
                {
                    ds = SQLQuery.ReturnDataSet(@"SELECT MachineStock.ProductID,  ItemSubGroup.CategoryName, MachineStock.ProductName,  AVG(tblFifo.InValue) AS Price, SUM(MachineStock.InWeight) AS ReceivedQty, SUM(MachineStock.OutWeight) AS IssuedQty, 
                         SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) AS Opening, SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) + SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) AS Closing, 
                        (SUM(MachineStock.InWeight)*tblFifo.InValue) AS ReceivedAmount,  (SUM(MachineStock.OutWeight)*tblFifo.InValue) AS IssuedAmount, ((SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight))*tblFifo.InValue) as OpeningBalance,
						 ((SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) + SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight))*tblFifo.InValue) as ClosingBalance
                         FROM Products INNER JOIN
                         Categories ON Products.CategoryID = Categories.CategoryID INNER JOIN
                         MachineStock INNER JOIN
                         tblFifo ON MachineStock.ProductID = tblFifo.ItemCode ON Products.ProductID = MachineStock.ProductID INNER JOIN
                         ItemGrade ON Categories.GradeID = ItemGrade.GradeID INNER JOIN
                         ItemSubGroup ON ItemGrade.CategoryID = ItemSubGroup.CategoryID 
						 " + subGroup + " " + dateTo + " GROUP BY MachineStock.ProductName, MachineStock.ProductID, tblFifo.InValue, ItemSubGroup.CategoryName");
                }
                else if (ddSubGroup == "12") //METAL HANDLE
                {
                    ds = SQLQuery.ReturnDataSet(@"SELECT MachineStock.ProductID,  ItemSubGroup.CategoryName, MachineStock.ProductName,  AVG(tblFifo.InValue) AS Price, SUM(MachineStock.InWeight) AS ReceivedQty, SUM(MachineStock.OutWeight) AS IssuedQty, 
                         SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) AS Opening, SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) + SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) AS Closing, 
                        (SUM(MachineStock.InWeight)*tblFifo.InValue) AS ReceivedAmount,  (SUM(MachineStock.OutWeight)*tblFifo.InValue) AS IssuedAmount, ((SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight))*tblFifo.InValue) as OpeningBalance,
						 ((SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) + SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight))*tblFifo.InValue) as ClosingBalance
                         FROM Products INNER JOIN
                         Categories ON Products.CategoryID = Categories.CategoryID INNER JOIN
                         MachineStock INNER JOIN
                         tblFifo ON MachineStock.ProductID = tblFifo.ItemCode ON Products.ProductID = MachineStock.ProductID INNER JOIN
                         ItemGrade ON Categories.GradeID = ItemGrade.GradeID INNER JOIN
                         ItemSubGroup ON ItemGrade.CategoryID = ItemSubGroup.CategoryID 
						 " + subGroup + " " + dateTo + " GROUP BY MachineStock.ProductName, MachineStock.ProductID, tblFifo.InValue, ItemSubGroup.CategoryName");
                }
                else if (ddSubGroup != "9") //Not Tin Plate : Plastic Raw materials || ddSubGroup != "12"
                {
                    // query = " FROM MachineStock "+ dateFrom +  dateTo;
                    ds = SQLQuery.ReturnDataSet(@"SELECT MachineStock.ProductID,  ItemSubGroup.CategoryName, MachineStock.ProductName,  AVG(tblFifo.InValue) AS Price, SUM(MachineStock.InWeight) AS ReceivedQty, SUM(MachineStock.OutWeight) AS IssuedQty, 
                         SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) AS Opening, SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) + SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) AS Closing, 
                        (SUM(MachineStock.InWeight)*tblFifo.InValue) AS ReceivedAmount,  (SUM(MachineStock.OutWeight)*tblFifo.InValue) AS IssuedAmount, ((SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight))*tblFifo.InValue) as OpeningBalance,
						 ((SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight) + SUM(MachineStock.InWeight) - SUM(MachineStock.OutWeight))*tblFifo.InValue) as ClosingBalance
                         FROM Products INNER JOIN
                         Categories ON Products.CategoryID = Categories.CategoryID INNER JOIN
                         MachineStock INNER JOIN
                         tblFifo ON MachineStock.ProductID = tblFifo.ItemCode ON Products.ProductID = MachineStock.ProductID INNER JOIN
                         ItemGrade ON Categories.GradeID = ItemGrade.GradeID INNER JOIN
                         ItemSubGroup ON ItemGrade.CategoryID = ItemSubGroup.CategoryID 
						 " + subGroup + " " + dateTo + " GROUP BY MachineStock.ProductName, MachineStock.ProductID, tblFifo.InValue, ItemSubGroup.CategoryName");


                }
                else
                {

                    //if (ddType == "Printed Sheet")
                    //{
                    //    query += " AND ItemType='" + ddType + "' ";
                    //    ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT ProductID,ProductName,MachineID,AVG(tblFifo.InValue) AS Price,SUM(InWeight) as ReceivedQty,SUM(OutWeight) as IssuedQty, (SUM(InWeight)-SUM(OutWeight)) AS Opening,((SUM(InWeight)-SUM(OutWeight))+SUM(InWeight)-SUM(OutWeight)) as Closing FROM MachineStock INNER JOIN tblFifo ON MachineStock.ProductID=tblFifo.ItemCode " + subGroup + " " + dateTo + " GROUP BY MachineStock.ProductID, MachineStock.ProductName, MachineStock.MachineID, tblFifo.InValue");
                    //}
                    //else //Raw Sheet
                    //{
                    //    query += " AND ItemType='" + ddType + "' ";
                    //    ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT ProductID,ProductName,MachineID,AVG(tblFifo.InValue) AS Price,SUM(InWeight) as ReceivedQty,SUM(OutWeight) as IssuedQty, (SUM(InWeight)-SUM(OutWeight)) AS Opening,((SUM(InWeight)-SUM(OutWeight))+SUM(InWeight)-SUM(OutWeight)) as Closing FROM MachineStock INNER JOIN tblFifo ON MachineStock.ProductID=tblFifo.ItemCode " + subGroup + " " + dateTo + " GROUP BY MachineStock.ProductID, MachineStock.ProductName, MachineStock.MachineID, tblFifo.InValue");
                    //}
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

            }

            return ds.Tables[0];
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