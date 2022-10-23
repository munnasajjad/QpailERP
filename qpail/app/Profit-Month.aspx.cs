using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Profit_Month : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string dateM = DateTime.Now.Month.ToString();
            string dateY = DateTime.Now.Year.ToString();

            if (dateM.Length == 1)
            {
                dateM = "0" + dateM;
            }
            txtYear.Text = dateY;
            //ddGrade.DataBind();
            LoadGridData();
        }
    }

    private void LoadGridData()
    {
        //string sizeId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) SizeId FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");
        //string pId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) ProductID FROM [SaleDetails]  WHERE ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID='" + ddGrade.SelectedValue + "')) AND  ([productName] = '" + iName + "')");
        //string brandId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) BrandID FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");

        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataRow dr1 = null;

        dt1.Columns.Add(new DataColumn("RowName", typeof(string)));
        dt1.Columns.Add(new DataColumn("January", typeof(string)));
        dt1.Columns.Add(new DataColumn("February", typeof(string)));
        dt1.Columns.Add(new DataColumn("March", typeof(string)));
        dt1.Columns.Add(new DataColumn("April", typeof(string)));
        dt1.Columns.Add(new DataColumn("May", typeof(string)));
        dt1.Columns.Add(new DataColumn("June", typeof(string)));
        dt1.Columns.Add(new DataColumn("July", typeof(string)));
        dt1.Columns.Add(new DataColumn("August", typeof(string)));
        dt1.Columns.Add(new DataColumn("September", typeof(string)));
        dt1.Columns.Add(new DataColumn("October", typeof(string)));
        dt1.Columns.Add(new DataColumn("November", typeof(string)));
        dt1.Columns.Add(new DataColumn("December", typeof(string)));
        dt1.Columns.Add(new DataColumn("Total", typeof(string)));

        //Customer List
        //DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT a.CustomerID, (Select Company from Party where PartyID=a.CustomerID) AS PartyName FROM Sales a ORDER BY [PartyName]");

        //foreach (DataRow drx in dtx.Rows)
        //{
        string customerID = "";// drx["CustomerID"].ToString();
                               //string customerName = drx["PartyName"].ToString();

        dr1 = dt1.NewRow();
        dr1["RowName"] = "Net profit before provision";
        dr1["January"] = CalcQty(customerID, txtYear.Text + "-01-01", txtYear.Text + "-02-01", "npbp");
        dr1["February"] = CalcQty(customerID, txtYear.Text + "-02-01", txtYear.Text + "-03-01", "npbp");
        dr1["March"] = CalcQty(customerID, txtYear.Text + "-03-01", txtYear.Text + "-04-01", "npbp");
        dr1["April"] = CalcQty(customerID, txtYear.Text + "-04-01", txtYear.Text + "-05-01", "npbp");
        dr1["May"] = CalcQty(customerID, txtYear.Text + "-05-01", txtYear.Text + "-06-01", "npbp");
        dr1["June"] = CalcQty(customerID, txtYear.Text + "-06-01", txtYear.Text + "-07-01", "npbp");
        dr1["July"] = CalcQty(customerID, txtYear.Text + "-07-01", txtYear.Text + "-08-01", "npbp");
        dr1["August"] = CalcQty(customerID, txtYear.Text + "-08-01", txtYear.Text + "-09-01", "npbp");
        dr1["September"] = CalcQty(customerID, txtYear.Text + "-09-01", txtYear.Text + "-10-01", "npbp");
        dr1["October"] = CalcQty(customerID, txtYear.Text + "-10-01", txtYear.Text + "-11-01", "npbp");
        dr1["November"] = CalcQty(customerID, txtYear.Text + "-11-01", txtYear.Text + "-12-01", "npbp");

        int nextYear = Convert.ToInt32(txtYear.Text) + 1;
        dr1["December"] = CalcQty(customerID, txtYear.Text + "-12-01", nextYear + "-01-01", "npbp");
        dr1["Total"] = CalcQty(customerID, txtYear.Text + "-01-01", nextYear + "-01-01", "npbp");
        dt1.Rows.Add(dr1);


        dr1 = dt1.NewRow();
        dr1["RowName"] = "Provision For Tax (35%)";
        dr1["January"] = CalcQty(customerID, txtYear.Text + "-01-01", txtYear.Text + "-02-01", "tax");
        dr1["February"] = CalcQty(customerID, txtYear.Text + "-02-01", txtYear.Text + "-03-01", "tax");
        dr1["March"] = CalcQty(customerID, txtYear.Text + "-03-01", txtYear.Text + "-04-01", "tax");
        dr1["April"] = CalcQty(customerID, txtYear.Text + "-04-01", txtYear.Text + "-05-01", "tax");
        dr1["May"] = CalcQty(customerID, txtYear.Text + "-05-01", txtYear.Text + "-06-01", "tax");
        dr1["June"] = CalcQty(customerID, txtYear.Text + "-06-01", txtYear.Text + "-07-01", "tax");
        dr1["July"] = CalcQty(customerID, txtYear.Text + "-07-01", txtYear.Text + "-08-01", "tax");
        dr1["August"] = CalcQty(customerID, txtYear.Text + "-08-01", txtYear.Text + "-09-01", "tax");
        dr1["September"] = CalcQty(customerID, txtYear.Text + "-09-01", txtYear.Text + "-10-01", "tax");
        dr1["October"] = CalcQty(customerID, txtYear.Text + "-10-01", txtYear.Text + "-11-01", "tax");
        dr1["November"] = CalcQty(customerID, txtYear.Text + "-11-01", txtYear.Text + "-12-01", "tax");

        //int nextYear = Convert.ToInt32(txtYear.Text) + 1;
        dr1["December"] = CalcQty(customerID, txtYear.Text + "-12-01", nextYear + "-01-01", "tax");
        dr1["Total"] = CalcQty(customerID, txtYear.Text + "-01-01", nextYear + "-01-01", "tax");

        dt1.Rows.Add(dr1);

        dr1 = dt1.NewRow();
            dr1["RowName"] = "Net profit  after provision";
            dr1["January"] = CalcQty(customerID, txtYear.Text + "-01-01", txtYear.Text + "-02-01","");
            dr1["February"] = CalcQty(customerID, txtYear.Text + "-02-01", txtYear.Text + "-03-01", "");
            dr1["March"] = CalcQty(customerID, txtYear.Text + "-03-01", txtYear.Text + "-04-01", "");
            dr1["April"] = CalcQty(customerID, txtYear.Text + "-04-01", txtYear.Text + "-05-01", "");
            dr1["May"] = CalcQty(customerID, txtYear.Text + "-05-01", txtYear.Text + "-06-01", "");
            dr1["June"] = CalcQty(customerID, txtYear.Text + "-06-01", txtYear.Text + "-07-01", "");
            dr1["July"] = CalcQty(customerID, txtYear.Text + "-07-01", txtYear.Text + "-08-01", "");
            dr1["August"] = CalcQty(customerID, txtYear.Text + "-08-01", txtYear.Text + "-09-01", "");
            dr1["September"] = CalcQty(customerID, txtYear.Text + "-09-01", txtYear.Text + "-10-01", "");
            dr1["October"] = CalcQty(customerID, txtYear.Text + "-10-01", txtYear.Text + "-11-01", "");
            dr1["November"] = CalcQty(customerID, txtYear.Text + "-11-01", txtYear.Text + "-12-01", "");

            //int nextYear = Convert.ToInt32(txtYear.Text) + 1;
            dr1["December"] = CalcQty(customerID, txtYear.Text + "-12-01", nextYear + "-01-01", "");
            dr1["Total"] = CalcQty(customerID, txtYear.Text + "-01-01", nextYear + "-01-01", "");

            dt1.Rows.Add(dr1);
        //}


        GVrpt.DataSource = dt1;
        GVrpt.DataBind();


    }

    private string CalcQty(string customerId, string dtFrom, string dtTo, string returnType)
    {
        dtTo = Convert.ToDateTime(dtTo).AddDays(-1).ToString("yyyy-MM-dd");
        //string colName = ddType.SelectedValue;
        int qty = 0;//Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(" + colName + "),0)  FROM [SaleDetails] WHERE ([SizeId] = '" + sizeId + "') and ([ProductID] = '" + pId + "') and([BrandID] = '" + brandId + "') and EntryDate >= '" + dtFrom + "' AND EntryDate < '" + dtTo + "' ")));

        string amt1 = "", ControlAccountsID = "", ControlAccountsName = "", amt2 = "", amt3 = "", amt4 = "", amt5 = "", Maindecks = "", UpperLevelp = "", UpperLevels = "", Quarter = "", mgo = "", tank = "", FPK1 = "", FPK2 = "", FPK3 = "", APK1 = "", APK2 = "", APK3 = "", Chainlocker = "", DB1 = "", DB2 = "", DB3 = "", DB4 = "", ProjectID = "", FMean = "", AMean = "", FAMean = "", MidMean = "", MeanOfMean = "", QuarterMean = "", MainMean = "", UpperMean = "", Density = "", Qty = "";
        decimal sumAmt1 = 0, sumAmt2 = 0;

        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0301' ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt1 = SumByControl(ControlAccountsID, "Cr", dtFrom, dtTo);
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
            amt2 = SumByControl(ControlAccountsID, "Dr", dtFrom, dtTo);
            if (amt2 != "0.00")
            {
                sumAmt2 += Convert.ToDecimal(amt2);
            }
        }

        //Gross profit
        decimal gp = 0;
        decimal gpRate = 0;
        gp = sumAmt1 - sumAmt2;

        if (sumAmt1 > 0)
        {
            gpRate = gp / sumAmt1 * 100M;
        }

        //Less : Operating Expenses:
        decimal sumAmt3 = 0, sumAmt4 = 0;
        dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0402' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='04') ");
        foreach (DataRow drx in dtx.Rows)
        {
            ControlAccountsID = drx["ControlAccountsID"].ToString();
            ControlAccountsName = drx["ControlAccountsName"].ToString();
            amt3 = SumByControl(ControlAccountsID, "Dr", dtFrom, dtTo);
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
            amt4 = SumByControl(ControlAccountsID, "Cr", dtFrom, dtTo);
            if (amt4 != "0.00")
            {
                sumAmt4 += Convert.ToDecimal(amt4);
            }
        }

        //Net profit before provision
        decimal npbp = op + sumAmt4;
        decimal npRate = 0;
        if (sumAmt1 > 0)
        {
            npRate = npbp / sumAmt1 * 100M;
        }

        decimal transferred = npbp;
        decimal tax = 0;
        if (npbp > 0)
        {
             tax = npbp*35M/100M;
            decimal gr = npbp/10M; //10%= 10/100
            transferred = npbp - tax - gr;
        }

        if (returnType== "npbp")
        {
            return SQLQuery.FormatBDNumber(npbp);
        }
        else if (returnType == "tax")
        {
            return SQLQuery.FormatBDNumber(tax);
        }
        else
        {
        return SQLQuery.FormatBDNumber(transferred);
        }
    }

    private string SumByControl(string cid, string btype, string dateFrom, string dateTo)
    {
         //dateFrom = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
         //dateTo = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");

        if (btype == "Dr")
        {
            return SQLQuery.ReturnString(
                    "SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
                    cid + "'))) and ISApproved ='A' and EntryDate>='" + dateFrom + "' and EntryDate<='" + dateTo + "'");
        }
        else
        {
            return SQLQuery.ReturnString(
                    "SELECT ISNULL(SUM(VoucherCR),0) - ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
                    cid + "'))) and ISApproved ='A' and EntryDate>='" + dateFrom + "' and EntryDate<='" + dateTo + "'");
        }
    }

    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            LoadGridData();

        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.ToString();
            MessageBox("Invalid Data! Check Error Message...");
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string url = "";// "./rptLedger.aspx?party=" + "" + "&dt1=" + txtDateFrom.Text + "&dt2=" + txtDateTo.Text;
        ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");
    }
    public static class ResponseHelper
    {
        public static void Redirect(string url, string target, string windowFeatures)
        {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures))
            {

                context.Response.Redirect(url);
            }
            else
            {
                Page page = (Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException(
                        "Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(Page),
                    "Redirect",
                    script,
                    true);
            }
        }
    }

    protected void ddItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridData();

    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        //if (CheckBox1.Checked)
        //{
        //    ddGrade.Enabled = false;
        //}
        //else
        //{
        //    ddGrade.Enabled = true;
        //}

        LoadGridData();
    }

    protected void GVrpt_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVrpt.PageIndex = e.NewPageIndex;
        LoadGridData();
        GVrpt.PageIndex = e.NewPageIndex;
    }

    protected void ddGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridData();
    }
}
