using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using RunQuery;

public partial class Operator_Accounts_Chart_Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //getBalance();
            //getCompanyInfo();
            //GenerateCoA();
            string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormChartOfAccounts.aspx?dateFrom=";
            if1.Attributes.Add("src", urlx);
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

    }*/

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
    private void getBalance()
    {
        //get balance
        SqlCommand cmd2 = new SqlCommand("SELECT Count(*) FROM HeadSetup", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //cmd2.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = txtDateF.Text;
        //cmd2.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text);
        //cmd2.Parameters.Add("@SalesTo", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;

        cmd2.Connection.Open();
        lblBalance.Text = Convert.ToString(cmd2.ExecuteScalar()) + " ";
        cmd2.Connection.Close();
    }
    
    private void GenerateCoA()
    {
        string body = "<table class='table-bordered table-striped'><tr><th>A/C Group</th><th>Sub A/C</th><th>Control A/C</th><th>A/C Head ID</th><th>Accounts Head</th><th>Op.Bal (Dr.)</th><th>Op.Bal (Cr.)</th></tr>  ";
        string acGroupPrev = "", acSubPrev = "", acControlPrev = "", acHeadPrev = "";
        string acGroup = "", acSub = "", acControl = "", acHead = "";
        string acGroupTxt = "", acSubTxt = "", acControlTxt = "", acHeadTxt = "";

        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT sl, GroupID, GroupName, ProjectID FROM AccountGroup Order by sl");

        foreach (DataRow drx in dtx.Rows)
        {
            string acGroupID = drx["GroupID"].ToString();
            acGroup = drx["GroupName"].ToString();
            
            int subCount = Convert.ToInt32(SQLQuery.ReturnString(@"SELECT COUNT(AccountsID) FROM Accounts WHERE GroupID='" + acGroupID + "'"));
            DataTable dtx2 =RunQuery.SQLQuery.ReturnDataTable(@"SELECT sl, GroupID, AccountsID, AccountsName, ProjectID FROM Accounts WHERE GroupID='" + acGroupID +"' Order by sl");
            
            foreach (DataRow drx2 in dtx2.Rows)
            {
                string acSubID = drx2["AccountsID"].ToString();
                acSub = drx2["AccountsName"].ToString();

                int controlCount = Convert.ToInt32(SQLQuery.ReturnString(@"SELECT COUNT(ControlAccountsID) FROM ControlAccount WHERE AccountsID='" + acSubID + "'"));
                if (controlCount == 0)
                {
                    body += "<tr>" + comparePrevValue(acGroup, acGroupPrev) + comparePrevValue(acSub, acSubPrev) + "<td> </td><td> </td><td> </td><td> </td><td> </td></tr>";
                }

                DataTable dtx3 =RunQuery.SQLQuery.ReturnDataTable(@"SELECT sl, ControlAccountsID, ControlAccountsName, ProjectID FROM ControlAccount  WHERE AccountsID='" +acSubID + "'  Order by sl");
                foreach (DataRow drx3 in dtx3.Rows)
                {
                    string acControlID = drx3["ControlAccountsID"].ToString();
                    acControl = drx3["ControlAccountsName"].ToString();
                    
                    int headCount = Convert.ToInt32(SQLQuery.ReturnString(@"SELECT COUNT(AccountsHeadID) FROM HeadSetup WHERE ControlAccountsID='" + acControlID + "'"));
                    if (headCount==0)
                    {
                        body += "<tr>" + comparePrevValue(acGroup, acGroupPrev) + comparePrevValue(acSub, acSubPrev) + comparePrevValue(acControl, acControlPrev) + "<td> </td><td> </td><td> </td><td> </td></tr>";
                        acGroupPrev = acGroup;
                        acSubPrev = acSub;
                        acControlPrev = acControl;
                    }
                    else
                    {
                        DataTable dtx4 =RunQuery.SQLQuery.ReturnDataTable(@"SELECT EntryID, AccountsHeadID, AccountsHeadName, OpBalDr, OpBalCr, ProjectID FROM HeadSetup WHERE ControlAccountsID='" +acControlID + "'  Order by EntryID");

                        foreach (DataRow drx4 in dtx4.Rows)
                        {
                            string acHeadID = drx4["AccountsHeadID"].ToString();
                            acHead = drx4["AccountsHeadName"].ToString();

                            body += "<tr>" + comparePrevValue(acGroup,acGroupPrev) + comparePrevValue(acSub, acSubPrev) + comparePrevValue(acControl, acControlPrev) + "<td>"+ acHeadID + "</td><td>"+ acHead + "</td><td>"+ drx4["OpBalDr"].ToString() + " </td><td>" + drx4["OpBalCr"].ToString() + "</td></tr>";
                            acGroupPrev = acGroup;
                            acSubPrev = acSub;
                            acControlPrev = acControl;
                        }
                    }
                    acControlPrev = acControl;
                }
                acSubPrev = acSub;
            }
                    acGroupPrev = acGroup;
        }

        body += "</table>";
        ltrBody.Text = body;
    }

    private string comparePrevValue(string newValue, string prevValue)
    {
        string result = "<td><b>" + newValue + "</b></td>";
        if (newValue== prevValue)
        {
            result = "<td style='border-top-style: hidden;'> </td>";
        }
        return result;
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

}
