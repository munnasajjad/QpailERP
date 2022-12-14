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
    public partial class FormIncomeStatement2 : System.Web.UI.Page
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
            string opDate1 = Convert.ToString(Request.QueryString["op1"]);
            string clDate1 = Convert.ToString(Request.QueryString["cl1"]);
            string opDate2 = Convert.ToString(Request.QueryString["op2"]);
            string clDate2 = Convert.ToString(Request.QueryString["cl2"]);
            
            string y1 = Convert.ToString(Request.QueryString["y1"]);
            string y2 = Convert.ToString(Request.QueryString["y2"]);
            string endDate1 = SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" + y1 + "'");
            string endDate2 = SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" + y2 + "'");

            search(y1,y2);

            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT AccGroup as [group],AccHeadName as head, BalanceDr as balance, BalanceCr as Cr, ShowLine from PL order by id");

            DataTableReader dr2 = dtx.CreateDataReader();
            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.IncomeStatement);

            rpt.Load(Server.MapPath("CrptIncomeStatement2.rpt"));

            //string datefield = "From: " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy");
            string datefield = "As at :	" + Convert.ToDateTime(clDate1).ToString("MMMM dd.yyyy");
            string cl1 = Convert.ToDateTime(endDate1).ToString("dd.MM.yyyy");
            string cl2 = Convert.ToDateTime(endDate2).ToString("dd.MM.yyyy");
            //string datefield = "" + Convert.ToDateTime(clDate1).ToString("yyyy") + "           " + Convert.ToDateTime(clDate2).ToString("dd/MM/yyyy");
            // string datefield = "From" + Convert.ToDateTime(dateTo1).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo2).ToString("dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@y1", cl1);
            rpt.SetParameterValue("@y2", cl2);
            //rpt.SetParameterValue("@rptName", rptName);
            CrystalReportViewer1.ReportSource = rpt;

        }

        private static void InsertPL(string group, string subGrp, string head, string dr, string cr, string ShowLine)
        {
            SQLQuery.ExecNonQry("Insert INTO PL (AccGroup, SubGroup, AccHeadName, BalanceDr, BalanceCr, ShowLine) VALUES ('" + group + "','" + subGrp + "','" + head + "','" + dr + "', '" + cr + "', '" + ShowLine + "')");
        }
        private string search(string y1, string y2)
        {
            try
            {
                SQLQuery.ExecNonQry("Delete PL");

                string amt1x = "", amt1y = "",  amt2x = "", amt2y = "", amt3x = "", amt3y = "", amt4x = "", amt4y = "", amt5 = "", Maindecks = "", UpperLevelp = "", UpperLevels = "", Quarter = "", mgo = "", tank = "", FPK1 = "", FPK2 = "", FPK3 = "", APK1 = "", APK2 = "", APK3 = "", Chainlocker = "", DB1 = "", DB2 = "", DB3 = "", DB4 = "", ProjectID = "", FMean = "", AMean = "", FAMean = "", MidMean = "", MeanOfMean = "", QuarterMean = "", MainMean = "", UpperMean = "", Density = "", Qty = "";
                decimal sumAmt1x = 0, sumAmt1y = 0, sumAmt2x=0, sumAmt2y = 0;

                DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT id, AccGroup, AccHeadName, AccHeadID, ShowLine from Yearly_PL a WHERE Report='IS' AND (FinYear='" + y1 + "' OR FinYear='" + y2 + "') order by id ");
                
                foreach (DataRow drx in dtx.Rows)
                {
                    string id = drx["AccGroup"].ToString();
                    string AccGroup = drx["AccGroup"].ToString();
                    string headId = drx["AccHeadID"].ToString();
                    string headName = drx["AccHeadName"].ToString();
                    amt1x = SumControlByYear(headId, headName, y1);
                    amt1y = SumControlByYear(headId, headName, y2);

                    string isExist = SQLQuery.ReturnString("Select ID from PL  WHERE AccGroup='" + AccGroup + "' AND AccHeadName='" + headName + "' AND  SubGroup='" + headId + "'   ");
                    if (isExist == "")
                    {
                        InsertPL(AccGroup, headId, headName, amt1x, amt1y, drx["ShowLine"].ToString());
                    }
                        
                    
                }
                
                return "";

            }
            catch (Exception ex)
            {
                //lblMsg.Attributes.Add("class", "xerp_error");
                //lblMsg.Text = "ERROR: " + ex.ToString();
                //DataTable dt = null;
                return ex.ToString();
            }
        }

        private string SumControlByYear(string cid, string name, string year) 
        {
                return SQLQuery.ReturnString("SELECT BalanceDr FROM Yearly_PL WHERE AccHeadID='" + cid + "' AND AccHeadName='" + name + "' AND FinYear='" + year + "' AND Report='IS'");
           
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