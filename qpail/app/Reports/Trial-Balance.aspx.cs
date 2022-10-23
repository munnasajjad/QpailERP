using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Application_Reports_Trial_Balance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDateF.Text = DateTime.Now.AddDays(-3000).ToShortDateString();
            txtDateT.Text = DateTime.Now.ToShortDateString();

            GenerateAssets();
            GenerateLiabilities();
            GenerateExpenses();
            GenerateProfit();
            GenerateEquity();
            getBalance();
            getCompanyInfo();
        }
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
    }
    protected void btnLoad_Click(object sender, EventArgs e)
    {
        //GridView1.DataSource = null;
        //GridView1.DataSourceID = null;        
        GenerateAssets();
        GenerateLiabilities();
        GenerateExpenses();
        GenerateProfit();
        GenerateEquity();
        getBalance();

    }

    private void getBalance()
    {
        //get balance
        SqlCommand cmd2 = new SqlCommand("SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) AS Balance FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '01'))) and ISApproved ='A' and EntryDate<=@dateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text);
        cmd2.Connection.Open();
        int asset = Convert.ToInt32(cmd2.ExecuteScalar());
        lblTotalAsset.Text = asset + " Tk.";
        cmd2.Connection.Close();

        //get balance
        SqlCommand cmd2liab = new SqlCommand("SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) AS Balance FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '02'))) and ISApproved ='A' and EntryDate<=@dateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2liab.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text);
        cmd2liab.Connection.Open();
        int liab = Convert.ToInt32(cmd2liab.ExecuteScalar());
        lblTotalLiab.Text = liab + " Tk.";
        cmd2liab.Connection.Close();


        //get balance
        SqlCommand cmd2eq = new SqlCommand("SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) AS Balance FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '05'))) and ISApproved ='A' and EntryDate<=@dateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2eq.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text);
        cmd2eq.Connection.Open();
        int eq = Convert.ToInt32(cmd2eq.ExecuteScalar());
        lblTotalCapital.Text = eq + " Tk.";
        cmd2eq.Connection.Close();

        Label2.Text = Convert.ToString(asset - liab);
        Label3.Text = Convert.ToString(asset - liab - eq);

       
        //get income balance
        SqlCommand cmd21 = new SqlCommand("SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) AS Balance FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '03'))) and ISApproved ='A' and EntryDate>=@dateFrom and EntryDate<=@dateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd21.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = txtDateF.Text;
        cmd21.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text);
        //cmd21.Parameters.Add("@SalesTo", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;

        cmd21.Connection.Open();
        lblIncomeTotal.Text = Convert.ToString(cmd21.ExecuteScalar()) + " Tk.";
        cmd21.Connection.Close();

        //expense
        SqlCommand cmd22 = new SqlCommand("SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) AS Balance FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '04'))) and ISApproved ='A' and EntryDate>=@dateFrom and EntryDate<=@dateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd22.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = txtDateF.Text;
        cmd22.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text);
        //cmd22.Parameters.Add("@SalesTo", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;

        cmd22.Connection.Open();
        lblExpenseTotal.Text = Convert.ToString(cmd22.ExecuteScalar()) + " Tk.";
        cmd22.Connection.Close();

    }

    private void GenerateAssets()
    {
        SqlCommand cmd2 = new SqlCommand("SELECT DISTINCT AccountsHeadName, ((Select OpBalDr-OpBalCr from HeadSetup where AccountsHeadName=VoucherDetails.AccountsHeadName) + SUM(VoucherDR) - SUM(VoucherCR)) AS Balance FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '01'))) and ISApproved ='A' and EntryDate<=@dateTo GROUP BY AccountsHeadName", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = txtDateF.Text;
        cmd2.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text);

        cmd2.Connection.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd2);
        cmd2.Connection.Close();
        DataSet ds = new DataSet();
        //DataTable dt = new DataTable();
        da.Fill(ds);
        GridView2.DataSource = ds;
        GridView2.DataBind();

    }

    private void GenerateLiabilities()
    {
        SqlCommand cmd2 = new SqlCommand("SELECT DISTINCT AccountsHeadName, ((Select OpBalDr-OpBalCr from HeadSetup where AccountsHeadName=VoucherDetails.AccountsHeadName) + SUM(VoucherDR) - SUM(VoucherCR)) AS Balance FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '02'))) and ISApproved ='A' and EntryDate<=@dateTo GROUP BY AccountsHeadName", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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

    private void GenerateProfit()
    {
        SqlCommand cmd2 = new SqlCommand("SELECT DISTINCT AccountsHeadName, ((Select OpBalCr-OpBalDr from HeadSetup where AccountsHeadName=VoucherDetails.AccountsHeadName) + SUM(VoucherCR) - SUM(VoucherDR)) AS Revenue_Amount FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '03'))) and ISApproved ='A' and EntryDate>=@dateFrom and EntryDate<=@dateTo GROUP BY AccountsHeadName", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = txtDateF.Text;
        cmd2.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text);
        //cmd2.Parameters.Add("@SalesTo", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;

        cmd2.Connection.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd2);
        cmd2.Connection.Close();
        DataSet ds = new DataSet();
        //DataTable dt = new DataTable();
        da.Fill(ds);
        grvIncome.DataSource = ds;
        grvIncome.DataBind();
    }

    private void GenerateExpenses()
    {
        try
        {
            SqlCommand cmd2 = new SqlCommand("SELECT DISTINCT AccountsHeadName, ((Select OpBalDr-OpBalCr from HeadSetup where AccountsHeadName=VoucherDetails.AccountsHeadName) + SUM(VoucherDR) - SUM(VoucherCR)) AS Expense_Amount FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '04'))) and ISApproved ='A' and EntryDate>=@dateFrom and EntryDate<=@dateTo GROUP BY AccountsHeadName", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = txtDateF.Text;
            cmd2.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text);

            cmd2.Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            cmd2.Connection.Close();
            DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            da.Fill(ds);
            grvExpense.DataSource = ds;
            grvExpense.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "ERROR : " + ex.ToString();
           // MessageBox("Error Occured! Please Check the error Message.");
        }
    }

    private void GenerateEquity()
    {
        SqlCommand cmd2 = new SqlCommand("SELECT DISTINCT AccountsHeadName, ((Select OpBalDr-OpBalCr from HeadSetup where AccountsHeadName=VoucherDetails.AccountsHeadName) + SUM(VoucherDR) - SUM(VoucherCR)) AS Balance FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '05'))) and ISApproved ='A' and  EntryDate<=@dateTo GROUP BY AccountsHeadName", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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
        GridView3.DataSource = ds;
        GridView3.DataBind();

    }
}