﻿using System;
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
    public partial class FormCurrentStockFinished : System.Web.UI.Page
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
            string igodown = Convert.ToString(Request.QueryString["godown"]);
            string ilocation = Convert.ToString(Request.QueryString["location"]);
            string packsize = Convert.ToString(Request.QueryString["pacsize"]);
            string subgroup = Convert.ToString(Request.QueryString["subgro"]);
            string customer = Convert.ToString(Request.QueryString["cust"]);
            string ibrand = Convert.ToString(Request.QueryString["brand"]);
            string dateFrom = Convert.ToString(Request.QueryString["dt1"]);


            DataTable dtx = BindItemGrid(igodown, ilocation, packsize, subgroup, customer, ibrand, dateFrom); //RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + item + @"')) and (EntryDate >= '" + dateFrom + "')  AND  (EntryDate <= '" + dateTo + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.InventoryReport);

            rpt.Load(Server.MapPath("CrptCurrentStockFinished.rpt"));

            //string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

            string datefield = "As on :" + Convert.ToDateTime(dateFrom).ToString(" dd/MM/yyyy");
            string rptName = "rpt "; //SQLQuery.ReturnString("SELECT [CategoryName] FROM [ItemSubGroup] WHERE CategoryID='" + subgroup + "'");
            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;

        }

        private DataTable BindItemGrid(string godown, string ddLocation, string packSize, string subGroup, string customer, string brand, string date)
        {
            DataSet ds = new DataSet();
            try
            {
                if (godown != "--- all ---")
                {
                    godown = " AND WarehouseID='" + godown + "' ";
                }
                else
                {
                    godown = "";
                }
                if (ddLocation != "--- all ---")
                {
                    ddLocation = " AND LocationID='" + ddLocation + "' ";
                }
                else
                {
                    ddLocation = "";
                }
                if (packSize != "--- all ---")
                {
                    packSize = " AND SizeID='" + packSize + "' ";
                }
                else
                {
                    packSize = "";
                }

                if (subGroup != "--- all ---")
                {
                    subGroup = " AND ProductID IN (Select ProductID from Products where CategoryID in (SELECT CategoryID FROM Categories WHERE GradeID IN (SELECT GradeID FROM ItemGrade WHERE CategoryID ='" + subGroup + "'))) ";
                }
                else
                {
                    subGroup = "";
                }

                if (customer != "--- all ---")
                {
                    customer = " AND Customer='" + customer + "' ";
                }
                else
                {
                    customer = "";
                }

                if (brand != "--- all ---")
                {
                    brand = " AND BrandID='" + brand + "' ";
                }
                else
                {
                    brand = "";
                }

                if (date != "")
                {
                    date = " AND EntryDate<='" + date + "' ";
                }

                string query = " FROM Stock where ItemGroup='2' " + godown + ddLocation + packSize + customer + brand + subGroup + date;
                string url = "";// SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";


                query = @"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
                    (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
                    " (SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
                    " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName, (SELECT Company FROM Party WHERE (PartyID = Stock.Customer)) AS Customer, " +
                    " (SELECT BrandName FROM CustomerBrands WHERE (BrandID = Stock.BrandID)) AS BrandID, (SELECT [BrandName] FROM [Brands] WHERE BrandID=Stock.SizeId) AS SizeId, " +
                    " ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " +
                    query + " " + "GROUP BY WarehouseID, ProductID, Customer, BrandID, SizeId HAVING (ISNULL(SUM(InQuantity), 0) - ISNULL(SUM(OutQuantity), 0) <> 0)";
                ds = SQLQuery.ReturnDataSet(query);
                //                }
                //                else //Raw Sheet
                //                {
                //                    query += " AND ItemType='" + ddType.SelectedValue + "' ";
                //                    ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
                //                                 (Select Purpose from Purpose where pid=Stock.Purpose) AS Purpose, (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
                //                                "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
                //                                " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName,  " +
                //                                " ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
                //                                "  " +
                //                                "GROUP BY WarehouseID, Purpose, ProductID ");
                //                }
                //            }

                //ltrtotal.Text = "Total Result: " + ds.Tables[0].Rows.Count.ToString();
                //GridView1.DataSource = ds.Tables[0];
                //GridView1.DataBind();
            }
            catch (Exception ex)
            {
                //lblMsg2.Attributes.Add("class", "xerp_warning");
                //lblMsg2.Text = "ERROR: " + ex.ToString();
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