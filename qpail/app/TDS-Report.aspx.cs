using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_TDS_Report : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDateFrom.Text = "01/" + DateTime.Now.ToString("MM/yyyy");
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
            //DataTable dtx =
            //    RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT AccountsHeadName FROM HeadSetup WHERE AccountsHeadID=VoucherDetails.AccountsHeadID ) AS HEAD, ISNULL(SUM([VoucherDR]),0) As Dr, ISNULL(SUM([VoucherCR]),0) As Cr FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where ControlAccountsID='"+ddParties.SelectedValue+ @"')) and (EntryDate >= '" + Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd") + "')  AND  (EntryDate <= '" + Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd") + "')  AND ISApproved<>'C' group by AccountsHeadID ORDER BY HEAD");

            //GridView1.DataSource = dtx;
            //GridView1.DataBind();

            string dt1 = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
            string dt2 = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

            string media = "pdf", mediaType= "application/pdf";
            if (rbWord.Checked)
            {
                media = "doc";
                mediaType= "application/msword";
            }
            else if(rbExcel.Checked)
            {
                media = "xls";
                mediaType= "application/excel";
            }

            string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormControlAccSummery.aspx?item=" + ddParties.SelectedValue + "&dt1=" + dt1 + "&dt2=" + dt2 + "&media=" + media;
            if1.Attributes.Add("src", urlx);
            //ltrObject.Text = "<object type='"+ mediaType + "' data='"+ urlx + "' height='800px' width='100%'></object>";
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

}