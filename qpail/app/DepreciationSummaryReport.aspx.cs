using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using RunQuery;

public partial class app_DepreciationSummaryReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtdateFrom.Text = "01/" + DateTime.Now.AddMonths(0).ToString("MM/yyyy");
            txtdateTo.Text = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01").AddDays(-1).ToString("dd/MM/yyyy");
        }
        
    }

    //Messagebox For Alerts    
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (ddGroup.SelectedValue != "")
        {                 
            string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormDepreciationSummaryReport.aspx?item=" + ddGroup.SelectedValue+"&datefrom="+ Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd") +"&dateto="+ Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd") ;
            if1.Attributes.Add("src", urlx);
        }
        else
        {
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "ERROR: Data not found!";
        }
    }


    protected void ddGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
}