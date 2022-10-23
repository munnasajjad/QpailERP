using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using RptRunQuery;
using RunQuery;

public partial class app_ShareholderEquity : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddFinancialYear.DataBind();
            ddFinancialYear.Items.RemoveAt(0);
            ddFinancialYear.Items.RemoveAt(0);
            //ddFinancialYear.Items.RemoveAt(0);
            // txtdateFrom.Text = "01/" + DateTime.Now.AddMonths(-1).ToString("MM/yyyy");
            // txtdateTo.Text = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01").AddDays(-1).ToString("dd/MM/yyyy");
            //LoadDropdownInEdit();
            SelectGridData();
        }
    }

    private void SelectGridData()
    {
        //foreach (GridViewRow gvRow in GridView2.Rows)
        //{
        //    Label txtAccountType = gvRow.FindControl("txtAccountType") as Label;
        //    DropDownList ddName = gvRow.FindControl("ddName") as DropDownList;

        //    ddName.SelectedValue =SQLQuery.ReturnString("Select ControlAccount from    CashFlow_Setup WHERE Accounts_Type='" +txtAccountType.Text + "'");

        //}
    }
  
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {/*
        try
        {
            SQLQuery.ExecNonQry("Delete [CashFlow_Setup]");
            foreach (GridViewRow gvRow in GridView2.Rows)
            {
                Label txtAccountType = gvRow.FindControl("txtAccountType") as Label;
                DropDownList ddName = gvRow.FindControl("ddName") as DropDownList;

                SQLQuery.ExecNonQry("INSERT INTO[dbo].[CashFlow_Setup]([Accounts_Type],[ControlAccount]) VALUES('" + txtAccountType.Text + "', '" + ddName.SelectedValue + "')");

                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Accounts Link Updated Successfully.";
            }

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }*/
    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        try
        {/*
            SQLQuery.ExecNonQry("Delete [CashFlow_Setup]");
            foreach (GridViewRow gvRow in GridView2.Rows)
            {
                Label txtAccountType = gvRow.FindControl("txtAccountType") as Label;
                DropDownList ddName = gvRow.FindControl("ddName") as DropDownList;

                SQLQuery.ExecNonQry("INSERT INTO[dbo].[CashFlow_Setup]([Accounts_Type],[ControlAccount]) VALUES('" + txtAccountType.Text + "', '" + ddName.SelectedValue + "')");

                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Accounts Link Updated Successfully.";
            }
            */
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }
    
    protected void btnShow2_OnClick(object sender, EventArgs e)
    {
        try
        {
                SQLQuery.ExecNonQry("Delete Shareholdersequity");

                string finYearId2 = ddYear1.SelectedValue;
                string finYearId1 = SQLQuery.ReturnString("Select ISNULL(MAX(Financial_Year_Number),0) from tblFinancial_Year WHERE Financial_Year_Number<'" + finYearId2 + "' ");
                string finYearId0 = SQLQuery.ReturnString("Select ISNULL(MAX(Financial_Year_Number),0) from tblFinancial_Year WHERE Financial_Year_Number<'" + finYearId1 + "' ");
                
            if (Convert.ToInt32(finYearId0) >= 0)
            {
                decimal transferred1 = Convert.ToDecimal(GetEquity(finYearId0));//Transferred to Balance Sheet
                decimal transferred2 = Convert.ToDecimal(GetEquity(finYearId1));//
                decimal transferred3 = Convert.ToDecimal(GetEquity(finYearId2));//
                SQLQuery.ExecNonQry("Insert INTO Shareholdersequity (AccGroup, SubGroup, AccHeadName, Year1Balance, Year2Balance,Year3Balance, ShowLine) VALUES ('Cash Flows from Operating Activities','Net Profit for the Year','Net Profit for the Year','" + transferred1 + "','" + transferred2 + "','"+ transferred3 + "','2')");
                
                //Prepare
                search(finYearId0, finYearId1, finYearId2);

                int yearProcessed =Convert.ToInt32(SQLQuery.ReturnString("Select COUNT(ID) from Shareholdersequity "));
                if (yearProcessed <= 0)
                {
                    Notify("The financial year "+ddFinancialYear.SelectedItem.Text+" has not been processed yet! ", "error", lblMsg);
                    return;
                }
             
                //Process 
                string y1Date1 = SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" + finYearId1 + "'");
                string y1Date2 = SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" + finYearId1 + "'");

                string y2Date1 =
                    SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                          finYearId2 + "'");
                string y2Date2 =
                    SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                          finYearId2 + "'");
               
                y1Date1 = Convert.ToDateTime(y1Date1).ToString("yyyy-MM-dd");
                y1Date2 = Convert.ToDateTime(y1Date2).ToString("yyyy-MM-dd");
                y2Date1 = Convert.ToDateTime(y2Date1).ToString("yyyy-MM-dd");
                y2Date2 = Convert.ToDateTime(y2Date2).ToString("yyyy-MM-dd");
                string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") +
                              "XerpReports/FormShareholerEquity.aspx?op1=" + y1Date1 + "&cl1=" + y1Date2 + "&op2=" +
                              y2Date1+ "&cl2=" + y2Date2;
                if1.Attributes.Add("src", urlx);

            }
            else
            {
                Notify("Please select financial year properly", "error", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }


    private void search(string year1ID, string year2ID, string year3ID)
    {
            string yearID = year1ID;
        int yearNo = 1;

        while (yearNo <= 4)
        {
            DataTable dtx = SQLQuery.ReturnDataTable(
                    @"SELECT  ID, AccGroup, SubGroup, AccHeadName, AccHeadID, OpeningBalance, BalanceDr, BalanceCr, ClosingBalance, Ysnactive, ShowLine, FinYear, Report
                        FROM Yearly_PL WHERE (AccGroup = 'Shareholders Equity') AND (Report = 'BL') AND (FinYear = '" + yearID + "')");

            foreach (DataRow drx in dtx.Rows)
            {
                InsertPL(drx["AccGroup"].ToString(), drx["SubGroup"].ToString(), drx["AccHeadName"].ToString(), SQLQuery.FormatCashFlowNumber(drx["BalanceDr"].ToString().Replace(",","")), "0", yearNo);
            }
            
            yearNo++;
            if (yearNo >= 2 && yearNo <=3)
            {
                yearID = year2ID;
            }
            else
            {
                yearID = year3ID;
            }
        }
    }

    private static void InsertPL(string group, string subGrp, string head, string balance1, string ShowLine, int yearNo)
    {
        if (yearNo == 1)
        {
            SQLQuery.ExecNonQry(
                "Insert INTO Shareholdersequity (AccGroup, SubGroup, AccHeadName, BalanceDr1, ShowLine) VALUES ('" + group +
                "','" + subGrp + "','" + head + "','" + balance1 + "', '" + ShowLine + "')");
        }
        else
        {
            //update
            SQLQuery.ExecNonQry(
                "UPDATE [dbo].[Shareholdersequity] SET BalanceDr" + yearNo + " ='" + balance1 + "' , ShowLine =  '" + ShowLine + "'" +
                "" + " WHERE AccGroup ='" + group + "' AND SubGroup = '" + subGrp + "'AND AccHeadName = '" + head + "'   ");
        }
    }

    private string SumByControl(string cid, string btype, string dateTo)
    {
        decimal  returnValue = 0;

        if (btype == "Dr")
        {
            returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0) - ISNULL(SUM(OpBalCr),0) FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
            returnValue += Convert.ToDecimal(SQLQuery.ReturnString(
                    "SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
                    cid + "'))) and ISApproved ='A' and  EntryDate<='" + dateTo + "'"));
        }
        else
        {
            returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0) - ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
            returnValue += Convert.ToDecimal(SQLQuery.ReturnString(
                    "SELECT ISNULL(SUM(VoucherCR),0) - ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
                    cid + "'))) and ISApproved ='A' and  EntryDate<='" + dateTo + "'"));
        }
        return returnValue.ToString();
    }

    private decimal GetEquity(string year)
    {
        decimal rValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT   ISNULL(SUM(CONVERT(decimal, REPLACE(BalanceDr, ',', ''))), 0)  FROM Yearly_PL WHERE (Report = 'IS') AND (FinYear = '" + year + "') AND (AccHeadName = 'Net profit  after provision transferred to Balance sheet') ").Replace(",",""));
        return Math.Round(rValue, 2);
    }
    private string SumByControli(string cid, string btype, string dateTo)
    {
        //string dateFrom = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
        //string dateTo = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");
        decimal returnValue = 0;

        if (btype == "Dr")
        {
            returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0) - ISNULL(SUM(OpBalCr),0) FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
            returnValue += Convert.ToDecimal(SQLQuery.ReturnString(
                    "SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
                    cid + "'))) and ISApproved ='A' and  EntryDate<='" + dateTo + "'"));
        }
        else
        {
            returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0) - ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
            returnValue += Convert.ToDecimal(SQLQuery.ReturnString(
                    "SELECT ISNULL(SUM(VoucherCR),0) - ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" + cid + "'))) and ISApproved ='A' and  EntryDate<='" + dateTo + "'"));
        }
        return returnValue.ToString();
    }

}