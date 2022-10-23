using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Control_Balance : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //txtDateFrom.Text = "01/" + DateTime.Now.ToString("MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            GetData();
        }
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        GetData();
        string dt1 = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormControlBalance.aspx?parties=" + ddParties.SelectedValue + "&dt1=" + dt1;
        if1.Attributes.Add("src", urlx);
    }

    private void GetData()
    {
        try
        {
            //decimal opBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT   isnull(sum(OpBalDr),0)-isnull(sum(OpBalCr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID]  IN (Select AccountsHeadID from HeadSetup where ControlAccountsID= '" + ddParties.SelectedValue + "'))"));
            string opBal =" (SELECT   isnull(sum(OpBalDr),0)-isnull(sum(OpBalCr),0)  FROM [HeadSetup] WHERE [AccountsHeadID]=VoucherDetails.AccountsHeadID) ";
            string query = opBal + " + ISNULL(SUM([VoucherDR]),0) - ISNULL(SUM([VoucherCR]),0)";

            string qtyBal = " (SELECT   isnull(sum(QtyDr),0)-isnull(sum(QtyCr),0)  FROM [HeadSetup] WHERE [AccountsHeadID]=VoucherDetails.AccountsHeadID) ";
            string query2 = qtyBal + " + ISNULL(SUM([InQty]),0) - ISNULL(SUM([OutQty]),0)";

            string isLiability = ddParties.SelectedValue.Substring(0, 2);
            if (isLiability == "02")
            {
                 opBal = " (SELECT   isnull(sum(OpBalCr),0)-isnull(sum(OpBalDr),0)  FROM [HeadSetup] WHERE [AccountsHeadID]=VoucherDetails.AccountsHeadID) ";//Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(OpBalCr),0) - isnull(sum(OpBalDr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID]  IN (Select AccountsHeadID from HeadSetup where ControlAccountsID= '" + ddParties.SelectedValue + "'))"));
                query = opBal + " + ISNULL(SUM([VoucherCR]),0) - ISNULL(SUM([VoucherDR]),0)";
                qtyBal = " (SELECT   isnull(sum(QtyCr),0)-isnull(sum(QtyDr),0)  FROM [HeadSetup] WHERE [AccountsHeadID]=VoucherDetails.AccountsHeadID) ";
                query2 = qtyBal + " + ISNULL(SUM([OutQty]),0) - ISNULL(SUM([InQty]),0)";
            }

            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, "+ query +
            "As Balance,"+ query2 + " as Qty FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='" + ddParties.SelectedValue + @"'))  AND  (EntryDate <= '" + Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd") + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");

            GridView1.DataSource = dtx;
            GridView1.DataBind();
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
    protected void GVrpt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow) {
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Balance"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "Total";
            e.Row.Cells[1].Text = SQLQuery.FormatBDNumber(Convert.ToString(total));
        }
    }

}