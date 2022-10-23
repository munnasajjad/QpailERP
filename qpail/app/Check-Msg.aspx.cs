using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class AdminCentral_Check_Msg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //Get Branch/Centre Name
       lblBranch.Text = Page.User.Identity.Name.ToString();
        ListView1.DataBind();


    }
}
