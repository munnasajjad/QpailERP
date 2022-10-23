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
    public partial class FormDailyPrdn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadGridData();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

        }
        private void LoadGridData()
        {
            string lName = Page.User.Identity.Name.ToString();
            string prjID = "1";

            string type = Convert.ToString(Request.QueryString["type"]);
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);

            DataTable dt1 = BindItemGrid(dateFrom, dateTo, type);
            DataTableReader dr2 = dt1.CreateDataReader();
            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.DailyPrdn);

            if (type == "summery")
            {
                //Sub report
                DataTable dt1x =SQLQuery.ReturnDataTable(@"SELECT (SELECT BrandName  FROM Brands  WHERE (BrandID = a.PackSize)) AS PackSize,     (SELECT ItemName    FROM Products    WHERE (ProductID = a.ItemID)) AS Product, SUM(FinalProduction) AS PrdnQty, SUM(FinalKg) AS RMCons, SUM(Rejection) AS RejQty FROM PrdPlasticCont AS a WHERE (ActProduction > 0) AND (Date >='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ) AND (Date <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "') GROUP BY PackSize, ItemID Order by ItemID");
                DataTableReader dr2x = dt1x.CreateDataReader();
                //XerpDataSet dsxx = new XerpDataSet();
                dsx.Load(dr2x, LoadOption.OverwriteChanges, dsx.PrdnSummery);

                rpt.Load(Server.MapPath("CrptDailyPrdn.rpt"));
            }
            else
            {
                rpt.Load(Server.MapPath("rptDailyPrdnSheet.rpt"));
            }
            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            CrystalReportViewer1.ReportSource = rpt;

        }
        private DataTable BindItemGrid(string dateFrom, string dateTo, string type)
        {
            DataSet ds = new DataSet();
            try
            {
                if (dateFrom != "")
                {
                    dateFrom = " AND PrdPlasticCont.Date>='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ";
                }

                if (dateTo != "")
                {
                    dateTo = " AND PrdPlasticCont.Date<='" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "' ";
                }

                string query = "   where PrdPlasticCont.ActProduction>0 " + dateFrom + dateTo;
                string url = "";// SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";

                if (type == "summery")
                {
                        query = @"SELECT        (SELECT        BrandName
                          FROM            Brands
                          WHERE        (BrandID = PrdPlasticCont.PackSize)) + ' ' +
                             (SELECT        ItemName
                               FROM            Products
                               WHERE        (ProductID = PrdPlasticCont.ItemID)) + ' ' +
                             (SELECT        BrandName
                               FROM            CustomerBrands
                               WHERE        (BrandID = PrdPlasticCont.Brand)) AS ProductName,
                             (SELECT        DepartmentName
                               FROM            Colors
                               WHERE        (Departmentid = PrdPlasticCont.Color)) AS Color, SUM(PrdPlasticCont.FinalProduction) AS Qty, SUM(PrdPlasticCont.FinalKg) AS Weight, AVG(CONVERT(decimal, PrdPlasticCont.CycleTime)) 
                         AS CycleTime, SUM(PrdPlasticCont.WorkingHour + PrdPlasticCont.WorkingMin / 60) AS Running, SUM(PrdPlasticCont.CalcProduction) AS Projected, SUM(PrdPlasticCont.FinalProduction) AS Actual, 
                         SUM(PrdPlasticCont.Rejection) AS wastage, SUM(PrdPlasticCont.FinalProduction) AS FinalPrdn, SUM(PrdPlasticCont.FinalKg) AS FinalKg, SUM(PrdPlasticCont.WasteWeight) AS wasteWt, 
                         Machines.MachineNo AS Machine, Machines.Description AS Brand
FROM            PrdPlasticCont INNER JOIN
                         Machines ON PrdPlasticCont.MachineNo = Machines.mid " + query + @"
GROUP BY PrdPlasticCont.MachineNo, PrdPlasticCont.Brand, PrdPlasticCont.PackSize, PrdPlasticCont.ItemID, PrdPlasticCont.Color, Machines.MachineNo, Machines.Description 
ORDER BY Machine  ";

                }
                else
                {
                    query = @"SELECT        (SELECT        Company
                          FROM            Party
                          WHERE        (PartyID = PrdPlasticCont.CustomerID)) AS Customer,
                             (SELECT        BrandName
                               FROM            Brands
                               WHERE        (BrandID = PrdPlasticCont.PackSize)) + ' ' +
                             (SELECT        ItemName
                               FROM            Products
                               WHERE        (ProductID = PrdPlasticCont.ItemID)) + ' ' +
                             (SELECT        BrandName
                               FROM            CustomerBrands
                               WHERE        (BrandID = PrdPlasticCont.Brand)) AS ProductName,
                             (SELECT        'Shift - ' + DepartmentName AS Expr1
                               FROM            Shifts
                               WHERE        (Departmentid = PrdPlasticCont.Shift)) AS Shifts,
                             (SELECT        DepartmentName
                               FROM            Colors
                               WHERE        (Departmentid = PrdPlasticCont.Color)) AS Color, SUM(PrdPlasticCont.FinalProduction) AS Qty, SUM(PrdPlasticCont.FinalKg) AS Weight, AVG(CONVERT(decimal, PrdPlasticCont.CycleTime)) 
                         AS CycleTime, SUM(PrdPlasticCont.WorkingHour + PrdPlasticCont.WorkingMin / 60) AS Running, SUM(PrdPlasticCont.CalcProduction) AS Projected, SUM(PrdPlasticCont.FinalProduction) AS Actual, 
                         SUM(PrdPlasticCont.Rejection) AS wastage, SUM(PrdPlasticCont.Rejection) * 100 / SUM(PrdPlasticCont.ActProduction) AS wastePercent, Machines_1.MachineNo AS Machine, Machines_1.Description AS Brand
    FROM PrdPlasticCont INNER JOIN
                             Machines AS Machines_1 ON PrdPlasticCont.MachineNo = Machines_1.mid  " + query + @"
    GROUP BY Machines_1.MachineNo, PrdPlasticCont.MachineNo, PrdPlasticCont.CustomerID, PrdPlasticCont.Brand, PrdPlasticCont.PackSize, PrdPlasticCont.ItemID, PrdPlasticCont.Shift, PrdPlasticCont.Color, Machines_1.Description
    ORDER BY Machines_1.MachineNo, PrdPlasticCont.Shift";

                }

                ds = SQLQuery.ReturnDataSet(query);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        ReportDocument rpt = new ReportDocument();


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