using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Sale_Colln_Summery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDateFrom.Text ="01-"+ DateTime.Now.ToString("MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            GetData();
        }
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        GetData();
    }

    private void GetData()
    {
        try
        {
            //DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT Distinct (Select Company from Party where partyid=Transactions.HeadID) as Customer, isnull(sum(Cr),0) as Amount FROM [Transactions] 
            //        where  TrType = 'Customer' AND (TrDate >= '" + Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd") +
            //               "')  AND  (TrDate <= '" + Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd") + "')  group by headid having isnull(sum(Cr),0)<>0 order by Amount desc");

            //GridView1.DataSource = dtx;
            //GridView1.DataBind();

            string dt1 = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
            string dt2 = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

            string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormCollectionSummByCustomers.aspx?dateFrom=" + dt1 + "&dateTo=" + dt2;
            if1.Attributes.Add("src", urlx);
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
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

    private decimal total = (decimal)0.0;
    //private decimal TotalSales = (decimal)0.0;
    //private decimal balance = (decimal)0.0;
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Amount"));
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