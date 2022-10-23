using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_BankLoansReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddLoanType.DataBind();
            LoadLoanCode();
            txtDateFrom.Text = "01/" + DateTime.Now.ToString("MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            GetData();
           

        }
    }

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    public void LoadLoanCode()
    {
        try
        {
            ddLoanCode.Items.Clear();
            string strQuery = @"SELECT Id, Code FROM [BankLoan] WHERE LoanType='" + ddLoanType.SelectedValue + "' ORDER BY Code";
            SQLQuery.PopulateDropDown(strQuery,ddLoanCode,"Id", "Code");
            ddLoanCode.Items.Insert(0, new ListItem("---All---", "0"));
        }
        catch (Exception exception)
        {
            Notify(exception.ToString(), "error", lblMsg);
        }
    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        GetData();
    }

    private void GetData()
    {
        //        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT Distinct (Select Company from Party where partyid=Transactions.HeadID) as Customer,
        //(Select OpBalance from Party where partyid=Transactions.HeadID)+ isnull(sum(Dr),0)-isnull(sum(Cr),0) as Balance FROM [Transactions] 
        // where  TrType = 'Customer'   AND  (TrDate <= '"+ Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd") + "')  group by headid having ((Select OpBalance from Party where partyid=Transactions.HeadID)+ isnull(sum(Dr),0)-isnull(sum(Cr),0))<>0 order by balance desc ");

        //        GridView1.DataSource = dtx;
        //        GridView1.DataBind();
        //"XerpReports/FormCustomerLadger.aspx?Cust=" + PartyID + "&Types=" + TypeID + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo;
        string dateFrom = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormBankLoansReport.aspx?LoanTypeId=" + ddLoanType.SelectedValue + "&Code="+ddLoanCode.SelectedValue+ "&dateFrom=" + dateFrom + "&dateTo=" + dateTo;
        if1.Attributes.Add("src", urlx);
    }

    private decimal total = (decimal)0.0;
    //private decimal TotalSales = (decimal)0.0;
    //private decimal balance = (decimal)0.0;
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Balance"));
            //TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Cr"));
            //balance += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyBalance"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "Total";
            e.Row.Cells[1].Text = string.Format("{0:N2}", total);
        }
    }

    //protected void ddLoanType_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    SqlCommand cmd = new SqlCommand("SELECT [Code] FROM [BankLoan] where LoanType='" + ddLoanType.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
    //    cmd.Connection.Open();
    //    SqlDataReader dataList = cmd.ExecuteReader();

    //    ddLoanType.DataSource = dataList;
    //    ddLoanType.DataValueField = "Code";
    //    ddLoanType.DataTextField = "Code";
    //    ddLoanType.DataBind();
    //    cmd.Connection.Close();
    //    cmd.Connection.Dispose();
    //}
    protected void ddLoanType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadLoanCode();
    }
}