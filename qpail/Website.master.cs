using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;


public partial class Website : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void loginform_LoggedIn(object sender, EventArgs e)
    {
        // please don't use User.IsInRole here , because it will not be populated yet at this stage.

        if (Roles.IsUserInRole(loginform.UserName, "Admin"))
            Response.Redirect("~/Application/Dashboard.aspx");
        else if (Roles.IsUserInRole(loginform.UserName, "MPO"))
            Response.Redirect("~/MPO_Area/Default.aspx");
        else if (Roles.IsUserInRole(loginform.UserName, "Members"))
            Response.Redirect("~/Members_Area/Default.aspx");
    }
}
