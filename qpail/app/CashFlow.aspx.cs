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

public partial class app_CashFlow : System.Web.UI.Page
{
    private object ddName;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtdateFrom.Text = "01/" + DateTime.Now.AddMonths(-1).ToString("MM/yyyy");
            txtdateTo.Text = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01").AddDays(-1).ToString("dd/MM/yyyy");
            //LoadDropdownInEdit();
            ddYear1.DataBind();
            ddYear2.DataBind();
            ddYear1.Items.RemoveAt(0);
            ddYear2.Items.RemoveAt(0);
            ddYear2.Items.RemoveAt(0);
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
    /*
    private void LoadDropdownInEdit()
    {
        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccount FROM CashFlow_Setup");
        foreach (DataRow drx in dtx.Rows)
        {
            foreach (GridViewRow gvRow in GridView2.Rows)
            {
                DropDownList ddName = gvRow.FindControl("ddName") as DropDownList;
                ddName.SelectedValue = drx["ControlAccount"].ToString();
            }
        }
    }
    */
    private void Notify(string msg, string type, Label lblNotify)
    {
      //  ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
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
    protected void btnSearch2_OnClick(object sender, EventArgs e)
    {
        string dt1 = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
        string dt2 = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");
        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormCashFlow.aspx= &dt1=" + txtdateFrom.Text + "&dt2=" + txtdateTo.Text + " &y1=" + txtdateFrom.Text + "&y2=" + txtdateTo.Text;
        if1.Attributes.Add("src", urlx);
    }

    private void GenerateCashFlow(string finYear)
    {
        //string dateFrom = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
        //string dateTo = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");

        if (Convert.ToInt32(ddFinancialYear.SelectedValue) > 5)
        {
            string prevYear = SQLQuery.ReturnString("Select MAX(Financial_Year_Number) from  tblFinancial_Year WHERE Financial_Year_Number<'" + ddFinancialYear.SelectedValue + "' ");
        }



        decimal opBalance = 0;
        string opDate = Convert.ToDateTime(SQLQuery.ReturnString("Select Opening_Date from   tblFinancial_Year WHERE Financial_Year='" + finYear + "'  ")).ToString("yyyy-MM-dd");
        string cloDate = Convert.ToDateTime(SQLQuery.ReturnString("Select Closing_Date from  tblFinancial_Year WHERE Financial_Year='" + finYear + "' ")).ToString("yyyy-MM-dd");

        SQLQuery.ExecNonQry("INSERT INTO [dbo].[Cash_Flow] (HeadType, HeadGroup, HeadId, HeadName, BalancePrevYear, Balance) VALUES ('Operating Activities', )");

        //Net profit before provision

        //decimal npbp = op + sumAmt4;
        //decimal npRate = 0;
        //if (sumAmt1 > 0)
        //{
        //    npRate = npbp / sumAmt1 * 100M;
        //}
        //body += "</table><br/><h4> </h4>" +
        //          "<table class='table'>" +
        //          "<tr><td><b>Net profit before provision</b></td><td><b>" + SQLQuery.FormatCashFlowNumber(npbp) + "<b></td></tr>" +
        //          "<tr><td>Net profit rate</td><td>" + Math.Round(npRate, 2) + "%</td></tr>";


    }


    private string HeadBalance(string cid, string btype)
    {
        string dateTo = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");
        decimal returnValue = 0;

        if (btype == "Dr")
        {
            returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE (AccountsHeadID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
            returnValue += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE (AccountsHeadID = '" + cid + "') and ISApproved ='A' and EntryDate<='" + dateTo + "'"));
        }
        else
        {
            returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0)  FROM HeadSetup WHERE (AccountsHeadID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
            returnValue += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0)  FROM VoucherDetails WHERE (AccountsHeadID = '" + cid + "') and ISApproved ='A' and EntryDate<='" + dateTo + "'"));
        }
        return returnValue.ToString();
    }


    /*
        private void GenerateExpenses()
        {
            SqlCommand cmd2 = new SqlCommand("SELECT DISTINCT AccountsHeadName, SUM(VoucherDR) - SUM(VoucherCR) AS Expense_Amount FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '04'))) and ISApproved ='A' and EntryDate>=@dateFrom and EntryDate<=@dateTo GROUP BY AccountsHeadName", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = txtDateF.Text;
            cmd2.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text);
            //cmd2.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text).AddDays(+1);
            //cmd2.Parameters.Add("@SalesTo", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;

            cmd2.Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            cmd2.Connection.Close();
            DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            da.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();

        } * */
    protected void btnShow2_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddYear1.SelectedValue) < Convert.ToInt32(ddYear2.SelectedValue) && Convert.ToInt32(ddYear1.SelectedValue)>0)
            {
               // SQLQuery.ExecNonQry("Delete CashFlow");

                string finYearId1 =SQLQuery.ReturnString(
                        "Select ISNULL(MAX(Financial_Year_Number),0) from tblFinancial_Year WHERE Financial_Year_Number<'" +
                        ddYear1.SelectedValue + "' ");
                string finYearId2 = ddYear1.SelectedValue;
                string finYearId3 =SQLQuery.ReturnString(
                        "Select ISNULL(MAX(Financial_Year_Number),0) from tblFinancial_Year WHERE Financial_Year_Number<'" +
                        ddYear2.SelectedValue + "' ");
                string finYearId4 = ddYear2.SelectedValue;
                
                string y1Date1 =
                    SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                          finYearId1 + "'");
                string y1Date2 =
                    SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                          finYearId1 + "'");

                string y2Date1 =
                    SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                          finYearId2 + "'");
                string y2Date2 =
                    SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                          finYearId2 + "'");
                string y3Date1 =
                    SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                          finYearId3 + "'");
                string y3Date2 =
                    SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                          finYearId3 + "'");

                string y4Date1 =
                    SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                          finYearId4 + "'");
                string y4Date2 =
                    SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year_Number='" +
                                          finYearId4 + "'");
               string yd1 = Convert.ToDateTime(y2Date2).ToString("yyyy-MM-dd");
               string yd2 = Convert.ToDateTime(y4Date2).ToString("yyyy-MM-dd");
                
                y1Date1 = Convert.ToDateTime(y1Date1).ToString("yyyy-MM-dd");
                y1Date2 = Convert.ToDateTime(y1Date2).ToString("yyyy-MM-dd");
                y2Date1 = Convert.ToDateTime(y2Date1).ToString("yyyy-MM-dd");
                y2Date2 = Convert.ToDateTime(y2Date2).ToString("yyyy-MM-dd");
                y3Date1 = Convert.ToDateTime(y3Date1).ToString("yyyy-MM-dd");
                y3Date2 = Convert.ToDateTime(y3Date2).ToString("yyyy-MM-dd");
                y4Date1 = Convert.ToDateTime(y4Date1).ToString("yyyy-MM-dd");
                y4Date2 = Convert.ToDateTime(y4Date2).ToString("yyyy-MM-dd");

                string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") +
                              "XerpReports/FormCashFlow.aspx?op1=" + finYearId1 + "&op2=" + finYearId2 + "&op3=" + finYearId3 + "&op4=" + finYearId4 + "&y1=" +
                              ddYear1.SelectedItem.Text + "&y2=" + ddYear2.SelectedItem.Text + "&1yd=" + yd1 + "&2yd=" + yd2;
                if1.Attributes.Add("src", urlx);


               
                //   decimal transferred1 = Convert.ToDecimal(GetEquity("transferred", y2Date2));//Transferred to Balance Sheet
                //   decimal transferred2 = Convert.ToDecimal(GetEquity("transferred", y4Date2));//

                //   SQLQuery.ExecNonQry("Insert INTO CashFlow (AccGroup, SubGroup, AccHeadName, Year1Balance, Year2Balance, ShowLine) VALUES ('Cash Flows from Operating Activities','Net Profit for the Year','Net Profit for the Year','" + transferred1 + "','" + transferred2 + "', '2')");


                //   search(y1Date1, y1Date2, 1);
                //   search(y2Date1, y2Date2, 2);
                //   search(y3Date1, y3Date2, 3);
                //   search(y4Date1, y4Date2, 4);


                //   SQLQuery.ExecNonQry("UPDATE [dbo].[CashFlow]   SET [Year1Balance] = (BalanceDr3-BalanceCr3),  [Year2Balance] = (BalanceDr4-BalanceCr4) WHERE ShowLine='1' ");
                //   SQLQuery.ExecNonQry("UPDATE [dbo].[CashFlow]   SET [Year1Balance] = (BalanceCr3-BalanceDr3),  [Year2Balance] = (BalanceCr4-BalanceDr4) WHERE ShowLine='0' ");

                //   SQLQuery.ExecNonQry("UPDATE [dbo].[CashFlow]   SET [AccHeadName] ='Increase in '+ AccHeadName WHERE Year1Balance< Year2Balance AND AccHeadName<>'Net Profit for the Year' ");
                //   SQLQuery.ExecNonQry("UPDATE [dbo].[CashFlow]   SET [AccHeadName] ='Decrease in '+ AccHeadName WHERE Year1Balance> Year2Balance AND AccHeadName<>'Net Profit for the Year' ");



            }
            else
            {
                Notify("Please select financial year properly", "error", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(),"error", lblMsg);
        }
    }


    private void search(string dateTo1, string dateTo2, int yearNo)
    {
        string amt1 = "",
            amt2 = "",
            ControlAccountsID = "",
            ControlAccountsName = "",
            amt2x = "",
            amt2y = "",
            amt3x = "",
            amt3y = "",
            amt4x = "",
            amt4y = "",
            amt5x = "",
            amt5y = "",
            Maindecks = "",
            UpperLevelp = "",
            UpperLevels = "",
            Quarter = "",
            mgo = "",
            tank = "",
            FPK1 = "",
            FPK2 = "",
            FPK3 = "",
            APK1 = "",
            APK2 = "",
            APK3 = "",
            Chainlocker = "",
            DB1 = "",
            DB2 = "",
            DB3 = "",
            DB4 = "",
            ProjectID = "",
            FMean = "",
            AMean = "",
            FAMean = "",
            MidMean = "",
            MeanOfMean = "",
            QuarterMean = "",
            MainMean = "",
            UpperMean = "",
            Density = "",
            Qty = "";
        decimal sumAmt1x = 0, sumAmt1y = 0, sumAmt2x = 0, sumAmt2y = 0;

        //Non-Current Assets
        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0101' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='01') ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt1 = SumByControl(ControlAccountsID, "Dr", dateTo1);
            amt2 = SumByControl(ControlAccountsID, "Dr", dateTo2);
            if (amt1 != "0.00")
            {
                //body += "<tr><td><b>" + ControlAccountsName + "</b></td><td>" + SQLQuery.FormatCashFlowNumber(amt1) + "</td></tr>";
                InsertPL("Non-Current Assets", ControlAccountsName, ControlAccountsName, SQLQuery.FormatCashFlowNumber(amt1), SQLQuery.FormatCashFlowNumber(amt2), "1", yearNo);

                sumAmt1x += Convert.ToDecimal(amt1);
                sumAmt1y += Convert.ToDecimal(amt2);
            }
        }
        //body += "<tr><td><b>Total non current Assets</b></td><td>" + SQLQuery.FormatCashFlowNumber(sumAmt1.ToString()) + "</td></tr>";
        InsertPL("Non-Current Assets", ControlAccountsName, "Total non current Assets",
            SQLQuery.FormatCashFlowNumber(sumAmt1x), SQLQuery.FormatCashFlowNumber(sumAmt1y), "1", yearNo);

        //Current Assets
        //body += "</table><br/><h4>Current Assets</h4>" +"<table class='table'>";

        dtx =
            SQLQuery.ReturnDataTable(
                @"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0101' ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt2x = SumByControl(ControlAccountsID, "Dr", dateTo1);
            amt2y = SumByControl(ControlAccountsID, "Dr", dateTo2);
            if (amt2x != "0.00")
            {
                //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatCashFlowNumber(amt2) + "</td></tr>";
                InsertPL("Current Assets", ControlAccountsName, ControlAccountsName, SQLQuery.FormatCashFlowNumber(amt2x),
                    SQLQuery.FormatCashFlowNumber(amt2y), "1", yearNo);

                sumAmt2x += Convert.ToDecimal(amt2x);
                sumAmt2y += Convert.ToDecimal(amt2y);
            }
        }
        /*
            body += "<tr><td><b>Total Current Assets</b></td><td>" + SQLQuery.FormatCashFlowNumber(sumAmt2.ToString()) + "</td></tr>";

            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Total Assets</strong></td><td>" + SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt1 + sumAmt2).ToString()) + "</td></tr>";
*/
        InsertPL("Current Assets", ControlAccountsName, "Total Current Assets",
            SQLQuery.FormatCashFlowNumber(sumAmt2x.ToString()), SQLQuery.FormatCashFlowNumber(sumAmt2y.ToString()), "1", yearNo);
        InsertPL("Current Assets", ControlAccountsName, "Total Assets",
            SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt1x + sumAmt2y).ToString()),
            SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt1x + sumAmt2y).ToString()), "1", yearNo);

        //Shareholders' Equity 
        decimal sumAmt3x = 0, sumAmt4x = 0, sumAmt5x = 0, sumAmt3y = 0, sumAmt4y = 0, sumAmt5y = 0;
        //body += "</table><br/><h4> holders' Equity</h4>" +"<table class='table'>";

        decimal transferred1 = Convert.ToDecimal(GetEquity("transferred", dateTo1));//Transferred to Balance Sheet
        decimal transferred2 = Convert.ToDecimal(GetEquity("transferred", dateTo2));//

        if (transferred1 < 0)
        {
            transferred1 = Convert.ToDecimal(GetEquity("npbp", dateTo1));
        }
        if (transferred2 < 0)
        {
            transferred2 = Convert.ToDecimal(GetEquity("npbp", dateTo2));
        }
        int i = 0;

        dtx =
            SQLQuery.ReturnDataTable(
                @"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID IN (Select AccountsID from Accounts Where GroupID='05') ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt3x = SumByControl(ControlAccountsID, "Cr", dateTo1);
            amt3y = SumByControl(ControlAccountsID, "Cr", dateTo2);

            if (i == 0) //Net profit after provision will sum with Retained Earnings (i for First Control A/C)
            {
                amt3x = Convert.ToString(Convert.ToDecimal(amt3x) + transferred1);
                amt3y = Convert.ToString(Convert.ToDecimal(amt3y) + transferred2);
                i++;
            }

            if (amt3x != "0.00")
            {
                //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatCashFlowNumber(amt3) + "</td></tr>";
                InsertPL("Shareholders Equity", ControlAccountsName, ControlAccountsName, SQLQuery.FormatCashFlowNumber(amt3x),
                    SQLQuery.FormatCashFlowNumber(amt3y), "0", yearNo);

                sumAmt3x += Convert.ToDecimal(amt3x);
                sumAmt3y += Convert.ToDecimal(amt3y);
            }
        }

        decimal grx = Convert.ToDecimal(GetEquity("gr", dateTo1));
        decimal gry = Convert.ToDecimal(GetEquity("gr", dateTo2));

        if (grx > 0)
        {
            sumAmt3x += grx;
            //body += "<tr><td>General reserve (10%)</td><td>" + SQLQuery.FormatCashFlowNumber(gr) + "</td></tr>";
            InsertPL("Shareholders Equity", ControlAccountsName, "General reserve (10%)", SQLQuery.FormatCashFlowNumber(grx), SQLQuery.FormatCashFlowNumber(gry), "0", yearNo);

        }

        //body += "</table><br/>" +"<table class='table'>" + "<tr><td><strong>Total Shareholders' Equity</strong></td><td>" + SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt3).ToString()) + "</td></tr>";
        InsertPL("Shareholders Equity", ControlAccountsName, "Total Shareholders Equity",
            SQLQuery.FormatCashFlowNumber(sumAmt3x), SQLQuery.FormatCashFlowNumber(sumAmt3y), "0", yearNo);



        //Non-Current Liabilities

        //body += "</table><br/><h4>Non-Current Liabilities</h4>" +"<table class='table'>";

        dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0201' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='02') ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt4x = SumByControl(ControlAccountsID, "Cr", dateTo1);
            amt4y = SumByControl(ControlAccountsID, "Cr", dateTo2);
            if (amt4x != "0.00")
            {
                //body += "<tr><td><b>" + ControlAccountsName + "</b></td><td>" + SQLQuery.FormatCashFlowNumber(amt4) + "</td></tr>";
                InsertPL("Non-Current Liabilities", ControlAccountsName, ControlAccountsName,
                    SQLQuery.FormatCashFlowNumber(amt4x), SQLQuery.FormatCashFlowNumber(amt4y), "0", yearNo);
                sumAmt4x += Convert.ToDecimal(amt4x);
            }
        }
        //body += "<tr><td><b>Total Long Term Loan</b></td><td>" + SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt4).ToString()) + "</td></tr>";
        InsertPL("Non-Current Liabilities", ControlAccountsName, "Total Long Term Loan",
            SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt4x).ToString()),
            SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt4y).ToString()), "0", yearNo);


        //Current liabilities
        //body += "</table><br/><h4>Current liabilities</h4>" +"<table class='table'>";

        dtx =
            SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0201' ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt5x = SumByControl(ControlAccountsID, "Cr", dateTo1);
            amt5y = SumByControl(ControlAccountsID, "Cr", dateTo2);
            if (amt5x != "0.00")
            {
                //body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatCashFlowNumber(amt5) + "</td></tr>";
                InsertPL("Current liabilities", ControlAccountsName, ControlAccountsName, SQLQuery.FormatCashFlowNumber(amt5x),
                    SQLQuery.FormatCashFlowNumber(amt5y), "0", yearNo);

                sumAmt5x += Convert.ToDecimal(amt5x);
                sumAmt5y += Convert.ToDecimal(amt5y);
            }
        }

        decimal tax1 = Convert.ToDecimal(GetEquity("tax", dateTo1));
        decimal tax2 = Convert.ToDecimal(GetEquity("tax", dateTo2));
        if (tax1 > 0)
        {
            sumAmt5x += tax1;
            //body += "<tr><td><strong>Provision For Tax (35%)</strong></td><td>" + tax + "</td></tr>";
            InsertPL("Current liabilities", ControlAccountsName, "Provision For Tax (35%)",
                SQLQuery.FormatCashFlowNumber(tax1), SQLQuery.FormatCashFlowNumber(tax2), "0", yearNo);

        }
        /*
            body += "<tr><td><b>Total current liabilities</b></td><td>" + SQLQuery.FormatCashFlowNumber(sumAmt5) + "</td></tr>";

            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Total Liabilities</strong></td><td>" + SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt4 + sumAmt5).ToString()) + "</td></tr>";
*/
        InsertPL("Current liabilities", ControlAccountsName, "Total current liabilities",
            SQLQuery.FormatCashFlowNumber(sumAmt5x), SQLQuery.FormatCashFlowNumber(sumAmt5y), "0", yearNo);
        InsertPL("Current liabilities", ControlAccountsName, "Total Liabilities",
            SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt4x + sumAmt5y).ToString()),
            SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt4x + sumAmt5y).ToString()), "0", yearNo);


        //Total
        decimal e_l = (sumAmt3x + sumAmt4x + sumAmt5x);
        decimal nav = (sumAmt1x + sumAmt2x) - (sumAmt4x + sumAmt5x);
        /*
            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Total Equity and Liabilities</strong></td><td>" + SQLQuery.FormatCashFlowNumber(Math.Round(e_l)) + "</td></tr>";


            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Net Asset Value (NAV) </strong></td><td>" + SQLQuery.FormatCashFlowNumber(Math.Round(nav)) + "</td></tr>";


            body += "</table><br/>" +
                      "<table class='table'>" +
                      "<tr><td><strong>Net Asset Value (NAV) per Share</strong></td><td>" + SQLQuery.FormatCashFlowNumber(Math.Round(nav / 60000M)) + "</td></tr>";
                      */
        InsertPL("Total Equity and Liabilities", ControlAccountsName, "Total Equity and Liabilities",
            SQLQuery.FormatCashFlowNumber(Math.Round(e_l)), SQLQuery.FormatCashFlowNumber(Math.Round(e_l)), "0", yearNo);
        //InsertPL("Current liabilities", ControlAccountsName, "Net Asset Value (NAV)",
        //    SQLQuery.FormatCashFlowNumber(Math.Round(nav)), SQLQuery.FormatCashFlowNumber(Math.Round(nav)), "0", yearNo);

        string noOfShares1 =
            SQLQuery.ReturnString("Select NoOFShares from Shareholdersequitydb WHERE FinYear='" + dateTo1 + "'");
        string noOfShares2 =
            SQLQuery.ReturnString("Select NoOFShares from Shareholdersequitydb WHERE FinYear='" + dateTo2 + "'");
        /*
        InsertPL("Current liabilities", ControlAccountsName, "Net Asset Value (NAV) per Share",
            SQLQuery.FormatCashFlowNumber(Math.Round(nav/Convert.ToDecimal(noOfShares1))),
            SQLQuery.FormatCashFlowNumber(Math.Round(nav/Convert.ToDecimal(noOfShares2))), "2", yearNo);

        //Ratio Analysis
        InsertPL("Debt/ Equity Ratio", ControlAccountsName, "Total Liabilities",
            SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt4x + sumAmt5x).ToString()),
            SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt4y + sumAmt5y).ToString()), "1", yearNo);
        InsertPL("Debt/ Equity Ratio", ControlAccountsName, "Total Equity",
            SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt3x)), SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt3y)), "0", yearNo);

        decimal shortLongTerm1 = Convert.ToDecimal(SumByControl("020101", "Cr", dateTo1)) -
                                 Convert.ToDecimal(SumByControl("020201", "Cr", dateTo1));
        decimal shortLongTerm2 = Convert.ToDecimal(SumByControl("020101", "Cr", dateTo2)) -
                                 Convert.ToDecimal(SumByControl("020201", "Cr", dateTo2));
        InsertPL("Debt/ Equity Ratio", ControlAccountsName, "Short Term Loan+Long Term Loan",
            SQLQuery.FormatCashFlowNumber(Math.Round(shortLongTerm1, 2).ToString()),
            SQLQuery.FormatCashFlowNumber(Math.Round(shortLongTerm2, 2).ToString()), "1", yearNo);
        InsertPL("Debt/ Equity Ratio", ControlAccountsName, "Total Equity",
            SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt3x)), SQLQuery.FormatCashFlowNumber(Math.Round(sumAmt3y)), "0", yearNo);
            */
        //ltrResult.Text = body;
        //ltrQR.Text = "From " + txtdateFrom.Text + " To " + txtdateTo.Text;

        //SQLQuery.ExecNonQry("Delete CashFlow WHERE FinYearId='"++"' ");

    }

    private static void InsertPL(string group, string subGrp, string head, string balance1, string balance2, string ShowLine, int yearNo)
    {
        if (yearNo == 1)
        {
            SQLQuery.ExecNonQry(
                "Insert INTO CashFlow (AccGroup, SubGroup, AccHeadName, BalanceDr1, BalanceCr1, ShowLine) VALUES ('" + group +
                "','" + subGrp + "','" + head + "','" + balance1 + "','" + balance2 + "', '" + ShowLine + "')");
        }
        else
        {
            //update
            SQLQuery.ExecNonQry( 
                "UPDATE [dbo].[CashFlow] SET BalanceDr"+ yearNo + " ='" + balance1 + "' , BalanceCr"+ yearNo + "='" + balance2 + "', ShowLine =  '" + ShowLine + "'" +
                "" + " WHERE AccGroup ='" + group + "' AND SubGroup = '" + subGrp + "'AND AccHeadName = '" + head + "'   ");
        }
    }

    private string SumByControl(string cid, string btype, string dateTo)
    {
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
                    "SELECT ISNULL(SUM(VoucherCR),0) - ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
                    cid + "'))) and ISApproved ='A' and  EntryDate<='" + dateTo + "'"));
        }
        return returnValue.ToString();
    }

    private decimal GetEquity(string rtype, string dateTo)
    {
        //string dateFrom = Convert.ToDateTime(txtdateTo.Text).AddMonths(-1).ToString("yyyy-MM") + "-01";
        //string dateTo = Convert.ToDateTime(Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM") + "-01").AddDays(-1).ToString("yyyy-MM-dd");

        string amt1 = "", ControlAccountsID = "", ControlAccountsName = "", amt2 = "", amt3 = "", amt4 = "", amt5 = "", Maindecks = "", UpperLevelp = "", UpperLevels = "", Quarter = "", mgo = "", tank = "", FPK1 = "", FPK2 = "", FPK3 = "", APK1 = "", APK2 = "", APK3 = "", Chainlocker = "", DB1 = "", DB2 = "", DB3 = "", DB4 = "", ProjectID = "", FMean = "", AMean = "", FAMean = "", MidMean = "", MeanOfMean = "", QuarterMean = "", MainMean = "", UpperMean = "", Density = "", Qty = "";
        decimal sumAmt1 = 0, sumAmt2 = 0;

        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0301' ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt1 = SumByControli(ControlAccountsID, "Cr", dateTo);
            if (amt1 != "0.00")
            {
                sumAmt1 += Convert.ToDecimal(amt1);
            }
        }
        dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0402' ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt2 = SumByControli(ControlAccountsID, "Dr", dateTo);
            if (amt2 != "0.00")
            {
                sumAmt2 += Convert.ToDecimal(amt2);
            }
        }

        //Gross profit
        decimal gp = 0;
        decimal gpRate = 0;
        if (sumAmt1 > 0)
        {
            gp = sumAmt1 - sumAmt2;
            gpRate = gp / sumAmt1 * 100M;
        }
        //Less : Operating Expenses:
        decimal sumAmt3 = 0, sumAmt4 = 0;

        dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0402' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='04') ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt3 = SumByControli(ControlAccountsID, "Dr", dateTo);
            if (amt3 != "0.00")
            {
                sumAmt3 += Convert.ToDecimal(amt3);
            }
        }

        //Operating Profit
        decimal op = gp - sumAmt3;

        dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0301' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='03') ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt4 = SumByControli(ControlAccountsID, "Cr", dateTo);
            if (amt4 != "0.00")
            {
                sumAmt4 += Convert.ToDecimal(amt4);
            }
        }

        //Net profit before provision
        decimal npbp = op + sumAmt4;
        decimal npRate = npbp / sumAmt1 * 100M;

        decimal tax = npbp * 35M / 100M;
        decimal gr = npbp / 10M; //10%= 10/100

        decimal transferred = npbp - tax - gr;
        decimal earning = (npbp - tax) / 60000M;

        decimal rValue = 0;
        if (rtype == "tax")//Provision For Tax (35%)
        {
            rValue = tax;
        }
        else if (rtype == "gr")//General reserve (10%)
        {
            rValue = gr;
        }
        else if (rtype == "npbp")//if loss happens
        {
            rValue = npbp;
        }
        else//Net profit after provision transferred to Balance sheet
        {
            rValue = transferred;
        }

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
