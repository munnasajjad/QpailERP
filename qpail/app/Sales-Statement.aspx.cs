using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Sales_Statement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtdateFrom.Text = "01" + DateTime.Now.ToString("dd/MM/yyyy").Substring(2);
            txtdateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //search();
        }

    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        string dateFrom = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");

        string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmSalesStatement.aspx?df="+ dateFrom + "&dt=" + dateTo;

        if1.Attributes.Add("src", url);
        //ResponseHelper.Redirect(url, "_blank", "fullscreen=1");
    }

    public static class ResponseHelper
    {
        public static void Redirect(string url, string target, string windowFeatures)
        {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_top", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures))
            {
                context.Response.Redirect(url);
            }
            else
            {
                Page page = (Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page, typeof(Page), "Redirect", script, true);
            }
        }
    }

}