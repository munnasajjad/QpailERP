using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_OutstandingList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnShow_OnClick(object sender, EventArgs e)
    {
        try
        {
            //string dt1 = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
            //string dt2 = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

            string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/RptOutstandingList.aspx?PartyId=" + ddParties.SelectedValue;
            if1.Attributes.Add("src", urlx);
        }
        catch (Exception ex)
        {

        }
    }
}