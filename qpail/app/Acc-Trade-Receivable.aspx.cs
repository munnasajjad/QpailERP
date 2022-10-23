using System;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Acc_Trade_Receivable : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtdateTo1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            GetData();
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

        string dt1 = Convert.ToDateTime(txtdateTo1.Text).ToString("yyyy-MM-dd");
        
        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormTradeReceivables.aspx?dateFrom=" + dt1;
        if1.Attributes.Add("src", urlx);

    }

    private decimal total = (decimal)0.0;
    //private decimal TotalSales = (decimal)0.0;
    //private decimal balance = (decimal)0.0;
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Balance"));
            //TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Cr"));
            //balance += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyBalance"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "Total";
            e.Row.Cells[1].Text = string.Format("{0:N2}", total);
        }
    }
}