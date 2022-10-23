using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_ControlStockSummary : System.Web.UI.Page
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
    protected void btnShow_OnClick(object sender, EventArgs e)
    {
        GetData();
    }
    
    private void GetData()
    {
        try
        {
            string dt1 = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
            string dt2 = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

            string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/ControlStockSummary.aspx?item=" + ddParties.SelectedValue + "&dt1=" + dt1 + "&dt2=" + dt2;
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
    
}