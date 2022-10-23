﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class Application_Reports_CancelledVouchers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDateF.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            txtDateT.Text = DateTime.Now.ToShortDateString();

            //GenerateExpenses();
            //GenerateProfit();
            //getBalance();
            getCompanyInfo();
        }
    }
    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    protected void btnLoad_Click(object sender, EventArgs e)
    {
        try
        {
            //GridView1.DataSource = null;
            //GridView1.DataSourceID = null;

            //GenerateExpenses();
            //GenerateProfit();
            //getBalance();
        }
        catch (Exception ex)
        {
            //lblMsg.Text= "ERROR : " + ex.ToString();
            MessageBox("Error Occured! Please Check the error Message.");
        }
    }

    private void getBalance()
    {
        //get balance
        SqlCommand cmd2 = new SqlCommand("SELECT ISNULL(SUM(VoucherDR),0) - ISNULL(SUM(VoucherCR),0) AS Balance FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '03') or (GroupID = '04'))) and ISApproved ='A' and EntryDate>=@dateFrom and EntryDate<=@dateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = txtDateF.Text;
        cmd2.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text);
        //cmd2.Parameters.Add("@SalesTo", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;

        cmd2.Connection.Open();
        //lblBalance.Text = Convert.ToString(cmd2.ExecuteScalar()) + " Tk.";
        cmd2.Connection.Close();
    }

    private void GenerateProfit()
    {
        SqlCommand cmd2 = new SqlCommand("SELECT DISTINCT AccountsHeadName, SUM(VoucherDR) - SUM(VoucherCR) AS Revenue_Amount FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '03'))) and ISApproved ='A' and EntryDate>=@dateFrom and EntryDate<=@dateTo GROUP BY AccountsHeadName", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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
        try
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
            //GridView1.DataSource = ds;
            //GridView1.DataBind();
        }
        catch (Exception ex)
        {
            //lblMsg.Text = "ERROR : " + ex.ToString();
            MessageBox("Error Occured! Please Check the error Message.");
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
}