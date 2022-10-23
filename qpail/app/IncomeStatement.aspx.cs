using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using RunQuery;

public partial class Application_Reports_IncomeStatement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
        {
                txtdateFrom.Text = "01/" + DateTime.Now.AddMonths(0).ToString("MM/yyyy");
                txtdateTo.Text = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01").AddDays(-1).ToString("dd/MM/yyyy");

                //search();

                //txtDateF.Text = DateTime.Now.AddDays(-30).ToShortDateString();
                //txtDateT.Text = DateTime.Now.ToShortDateString();

                //GenerateExpenses();
                //GenerateProfit();
                //getBalance();
                //getCompanyInfo();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message.ToString();
        }
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        //string s= search();
        //var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
        //var pdfBytes = htmlToPdf.GeneratePdf(s);
        //string savePath = Server.MapPath("./Docs/Print/") + "Income-Stat.pdf";
        //File.WriteAllBytes(savePath, pdfBytes);

        //string url = "./Docs/Print/Income-Stat.pdf";


        string dt1 = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
        string dt2 = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");

        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormIncomeStatement.aspx?dateFrom=" + dt1 + "&dateTo=" + dt2;
        if1.Attributes.Add("src", urlx);
    }
    protected void btnSearch2_OnClick(object sender, EventArgs e)
    {
        //string s= search();
        //var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
        //var pdfBytes = htmlToPdf.GeneratePdf(s);
        //string savePath = Server.MapPath("./Docs/Print/") + "Income-Stat.pdf";
        //File.WriteAllBytes(savePath, pdfBytes);

        //string url = "./Docs/Print/Income-Stat.pdf";

        string dt1 = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
        string dt2 = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");

        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormIncomeStatement2.aspx";
        if1.Attributes.Add("src", urlx);
    }

    private string SumByControl(string cid, string btype)
    {
        string dateFrom = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");
        if (btype == "Dr")
        {
            return SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
                    cid + "'))) and ISApproved ='A' and EntryDate>='" + dateFrom + "' and EntryDate<='" + dateTo + "'");
        }
        else
        {
            return SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0) - ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (ControlAccountsID = '" +
                    cid + "'))) and ISApproved ='A' and EntryDate>='" + dateFrom + "' and EntryDate<='" + dateTo + "'");
        }
    }

    private string search()
    {
        try
        {
            string dateFrom = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
            string dateTo = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");
            string body = "<table class='table' style='font:Arial'>" +
                          "<tr><th>Particulars</th><th style='text-align: right;'>Amount(TK.)</th></tr>";

            string amt1 = "", ControlAccountsID = "", ControlAccountsName = "", amt2 = "", amt3 = "", amt4 = "", amt5 = "", Maindecks = "", UpperLevelp = "", UpperLevels = "", Quarter = "", mgo = "", tank = "", FPK1 = "", FPK2 = "", FPK3 = "", APK1 = "", APK2 = "", APK3 = "", Chainlocker = "", DB1 = "", DB2 = "", DB3 = "", DB4 = "", ProjectID = "", FMean = "", AMean = "", FAMean = "", MidMean = "", MeanOfMean = "", QuarterMean = "", MainMean = "", UpperMean = "", Density = "", Qty = "";
            decimal sumAmt1 = 0, sumAmt2 = 0;

            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0301' ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt1 = SumByControl(ControlAccountsID, "Cr");
                if (amt1!= "0.00")
                {
                    body += "<tr><td><b>" + ControlAccountsName + "</b></td><td>" + SQLQuery.FormatBDNumber(amt1) + "</td></tr>";
                    sumAmt1 += Convert.ToDecimal(amt1);
                }
            }

            //Less : Direct expenses
            body += "</table><br/><h4>Less : Direct expenses</h4>" +
                      "<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID='0402' ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt2 = SumByControl(ControlAccountsID, "Dr");
                if (amt2 != "0.00")
                {
                    body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt2) + "</td></tr>";
                    sumAmt2 += Convert.ToDecimal(amt2);
                }
            }

            //Gross profit
            decimal gp = 0;
            decimal gpRate = 0;
                 gp = sumAmt1 - sumAmt2;

            if (sumAmt1>0)
            {
                 gpRate = gp / sumAmt1 * 100M;
            }

            body += "</table><br/><h4> </h4>" +
                      "<table class='table'>"+
                      "<tr><td><b>Gross profit</b></td><td><b>" + SQLQuery.FormatBDNumber(gp) + "</b></td></tr>" +
                      "<tr><td>Gross profit rate</td><td>" + Math.Round(gpRate) + "%</td></tr>";

            //Less : Operating Expenses:
            decimal sumAmt3 = 0, sumAmt4 = 0;
            body += "</table><br/><h4>Less : Operating Expenses:</h4>" +
                      "<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0402' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='04') ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt3 = SumByControl(ControlAccountsID, "Dr");
                if (amt3 != "0.00")
                {
                    body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt3) + "</td></tr>";
                    sumAmt3 += Convert.ToDecimal(amt3);
                }
            }

            //Operating Profit
            decimal op = gp - sumAmt3;
            body += "</table><br/><h4> </h4>" +
                      "<table class='table'>" +
                      "<tr><td><b>Operating profit</b></td><td><b>" + SQLQuery.FormatBDNumber(op) + "<b></td></tr>";

            //Add:Non operating income
            body += "</table><br/><h4>Less : Non operating income:</h4>" +
                      "<table class='table'>";

            dtx = SQLQuery.ReturnDataTable(@"SELECT ControlAccountsID, ControlAccountsName FROM [ControlAccount] WHERE AccountsID<>'0301' AND AccountsID IN (Select AccountsID from Accounts Where GroupID='03') ");
            foreach (DataRow drx in dtx.Rows)
            {
                ControlAccountsID = drx["ControlAccountsID"].ToString();
                ControlAccountsName = drx["ControlAccountsName"].ToString();
                amt4 = SumByControl(ControlAccountsID, "Cr");
                if (amt4 != "0.00")
                {
                    body += "<tr><td>" + ControlAccountsName + "</td><td>" + SQLQuery.FormatBDNumber(amt4) + "</td></tr>";
                    sumAmt4 += Convert.ToDecimal(amt4);
                }
            }

            //Net profit before provision
            decimal npbp = op + sumAmt4;
            decimal npRate =0;
            if (sumAmt1 > 0)
            {
                npRate = npbp/sumAmt1*100M;
            }
            body += "</table><br/><h4> </h4>" +
                      "<table class='table'>" +
                      "<tr><td><b>Net profit before provision</b></td><td><b>" + SQLQuery.FormatBDNumber(npbp) + "<b></td></tr>"+
                      "<tr><td>Net profit rate</td><td>" + Math.Round(npRate, 2) + "%</td></tr>";

            if (npbp>0)
            {
            decimal tax = npbp * 35M / 100M;
            decimal gr = npbp / 10M; //10%= 10/100
            body += "</table><br/><h4> </h4>" +
                      "<table class='table'>" +
                      "<tr><td>Provision For Tax (35%)</td><td>" + SQLQuery.FormatBDNumber(tax) + "</td></tr>" +
                      "<tr><td>General reserve (10%)</td><td>" + SQLQuery.FormatBDNumber(gr) + "</td></tr>";

            decimal transferred = npbp - tax - gr;
            decimal earning = (npbp - tax )/60000M;

            body += "</table><br/><h4> </h4>" +
                      "<table class='table'>" +
                      "<tr><td>Net profit  after provision transferred to Balance sheet</td><td>" + SQLQuery.FormatBDNumber(transferred) + "</td></tr>" +
                      "<tr><td>Earnings per share (60,000 share)</td><td>" + SQLQuery.FormatBDNumber(earning) + "</td></tr></table>";
            }
            
            ltrResult.Text = body;
            ltrQR.Text = "From " + txtdateFrom.Text + " To " + txtdateTo.Text;

            return body;

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
            //DataTable dt = null;
            return ex.ToString();
        }
    }

    /*
    protected void btnLoad_Click(object sender, EventArgs e)
    {
        //GridView1.DataSource = null;
        //GridView1.DataSourceID = null;

        GenerateExpenses();
        GenerateProfit();
        getBalance();

    }

    private void getBalance()
    {
        //get balance
        SqlCommand cmd2 = new SqlCommand("SELECT ISNULL(SUM(VoucherCR),0) - ISNULL(SUM(VoucherDR),0) AS Balance FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '03'))) and ISApproved ='A' and EntryDate>=@dateFrom and EntryDate<=@dateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = txtDateF.Text;
        cmd2.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text);
        //cmd2.Parameters.Add("@SalesTo", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;

        cmd2.Connection.Open();
        lblBalance.Text = Convert.ToString(cmd2.ExecuteScalar()) + " Tk.";
        cmd2.Connection.Close();

        lblDtFm.Text = txtDateF.Text;
        lblDtTo.Text = txtDateT.Text;
    }

    private void GenerateProfit()
    {
        SqlCommand cmd2 = new SqlCommand("SELECT EntryDate, AccountsHeadName, VoucherRowDescription, (VoucherCR - VoucherDR) AS Amount FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '03'))) and ISApproved ='A' and EntryDate>=@dateFrom and EntryDate<=@dateTo Order BY SerialNo desc", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = txtDateF.Text;
        cmd2.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text);
        //cmd2.Parameters.Add("@SalesTo", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;

        cmd2.Connection.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd2);
        cmd2.Connection.Close();
        DataSet ds = new DataSet();
        //DataTable dt = new DataTable();
        da.Fill(ds);
        GridView2.DataSource = ds;
        GridView2.DataBind();

    }

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

    }

    private void getCompanyInfo()
    {
        SqlConnection cnn = new SqlConnection();
        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        cmd.CommandText = "Select CompanyName, CompanyAddress, Logo From Company";
        cmd.CommandType = CommandType.Text;

        cnn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            lblName.Text = dr[0].ToString();
            lblArress.Text = dr[1].ToString();
            Image1.ImageUrl = "../../" + dr[2].ToString();
        }
        cnn.Close();
    }*/
    protected void btnShow2_OnClick(object sender, EventArgs e)
    {
        string startDate1 = SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year='" + ddYear1.SelectedValue + "'");
        string startDate2 = SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year='" + ddYear1.SelectedValue + "'");

        string endDate1 = SQLQuery.ReturnString("Select Opening_Date FROM    tblFinancial_Year WHERE  Financial_Year='" + ddYear2.SelectedValue + "'");
        string endDate2 = SQLQuery.ReturnString("Select Closing_Date FROM    tblFinancial_Year WHERE  Financial_Year='" + ddYear2.SelectedValue + "'");

        startDate1 = Convert.ToDateTime(startDate1).ToString("yyyy-MM-dd");
        startDate2 = Convert.ToDateTime(startDate2).ToString("yyyy-MM-dd");
        endDate1 = Convert.ToDateTime(endDate1).ToString("yyyy-MM-dd");
        endDate2 = Convert.ToDateTime(endDate2).ToString("yyyy-MM-dd");
        string y1 = SQLQuery.ReturnString("Select Financial_Year_Number FROM    tblFinancial_Year WHERE  Financial_Year='" + ddYear1.SelectedValue + "'");
        string y2 = SQLQuery.ReturnString("Select Financial_Year_Number FROM    tblFinancial_Year WHERE  Financial_Year='" + ddYear2.SelectedValue + "'");


        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormIncomeStatement2.aspx?op1=" + startDate1 + "&cl1=" + startDate2 + "&op2=" + endDate1 + "&cl2=" + endDate2 + "&y1=" + y1 + "&y2=" + y2;
        if1.Attributes.Add("src", urlx);
    }
}