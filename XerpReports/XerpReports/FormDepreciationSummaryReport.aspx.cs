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
    public partial class FormDepreciationSummaryReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();

        }

        private ReportDocument rpt = new ReportDocument();

        private void LoadGridData()
        {
            //decimal total2 = (decimal)0.0;
            //decimal qty2 = (decimal)0.0;
            //decimal vat2 = (decimal)0.0;
            //decimal TotalSales2 = (decimal)0.0;
            //decimal TotalWeight2 = (decimal)0.0;

            string lName = Page.User.Identity.Name.ToString();
            string prjID = "1";
            string groupid = Convert.ToString(Request.QueryString["item"]);
            string datefrom = Convert.ToString(Request.QueryString["datefrom"]);
            string dateto = Convert.ToString(Request.QueryString["dateto"]);
            string groupname = SQLQuery.ReturnString("Select GroupName From ItemGroup Where GroupSrNo='" + groupid + "'");

            if (groupid == "0")
            {
                DataTable dtx =
                    SQLQuery.ReturnDataTable(
                        @"SELECT        ItemGroup.GroupName, ItemGrade.GradeName, Products.ItemName, Categories.CategoryName, SUM(Depreciation.DepreciationAmount) as total, Depreciation.DepDate, Depreciation.ItemCode
FROM            ItemGroup INNER JOIN
                         ItemSubGroup INNER JOIN
                         ItemGrade INNER JOIN
                         Categories ON ItemGrade.GradeID = Categories.GradeID ON ItemSubGroup.CategoryID = ItemGrade.CategoryID ON ItemGroup.GroupSrNo = ItemSubGroup.GroupID INNER JOIN
                         Products ON Categories.CategoryID = Products.CategoryID INNER JOIN
                         Depreciation ON Products.ProductID = Depreciation.FixedAssetId
 Where (DepDate >= '" + datefrom + "') AND (DepDate <='" + dateto + "') GROUP BY ItemGroup.GroupName, ItemGrade.GradeName, Products.ItemName, Categories.CategoryName, Depreciation.DepreciationAmount, Depreciation.DepDate,Depreciation.ItemCode order by Depreciation.DepDate;");
                DataTableReader dr2 = dtx.CreateDataReader();

                XerpDataSet dsx = new XerpDataSet();
                dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.DepSummary);

                rpt.Load(Server.MapPath("CrptDepreciationSummary.rpt"));

                //string datefield = "From: " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy");
                //string datefield = "From" + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
                //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
                rpt.SetDataSource(dsx);
                SQLQuery.LoadrptHeader(dsx, rpt);
                //rpt.SetParameterValue("@date", datefield);
                rpt.SetParameterValue("@rptName", groupname);
                CrystalReportViewer1.ReportSource = rpt;


            }
            else
            {
                DataTable dtx =
                    SQLQuery.ReturnDataTable(
                        @"SELECT        ItemGroup.GroupName, ItemGrade.GradeName, Products.ItemName, Categories.CategoryName, SUM(Depreciation.DepreciationAmount) as total, Depreciation.DepDate, Depreciation.ItemCode
FROM            ItemGroup INNER JOIN
                         ItemSubGroup INNER JOIN
                         ItemGrade INNER JOIN
                         Categories ON ItemGrade.GradeID = Categories.GradeID ON ItemSubGroup.CategoryID = ItemGrade.CategoryID ON ItemGroup.GroupSrNo = ItemSubGroup.GroupID INNER JOIN
                         Products ON Categories.CategoryID = Products.CategoryID INNER JOIN
                         Depreciation ON Products.ProductID = Depreciation.FixedAssetId
 WHERE (ItemGroup.GroupSrNo = '" + groupid + "') AND (DepDate >='" + datefrom + "') AND (DepDate <='" + dateto + "') GROUP BY ItemGroup.GroupName, ItemGrade.GradeName, Products.ItemName, Categories.CategoryName, Depreciation.DepreciationAmount, Depreciation.DepDate,Depreciation.ItemCode order by Depreciation.DepDate");





                DataTableReader dr2 = dtx.CreateDataReader();

                XerpDataSet dsx = new XerpDataSet();
                dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.DepSummary);

                rpt.Load(Server.MapPath("CrptDepreciationSummary.rpt"));

                //string datefield = "From: " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy");
                //string datefield = "From" + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
                //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
                rpt.SetDataSource(dsx);
                SQLQuery.LoadrptHeader(dsx, rpt);
                //rpt.SetParameterValue("@date", datefield);
                rpt.SetParameterValue("@rptName", groupname);
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