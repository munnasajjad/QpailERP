using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Oxford
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DateTime time2 = new DateTime();
            string str = "30/12/3015 00:00:00"; // Service Validity
            time2 = Convert.ToDateTime(str);
            if (today > time2)
            {
                base.Response.Redirect("~/Default.aspx");
            }

            if(!IsPostBack)
            {
                Session.Abandon();
                Session.Contents.RemoveAll();
                System.Web.Security.FormsAuthentication.SignOut();
                //Response.Redirect("../Login.aspx");
            }
        }

        protected void Login1_LoggedIn(object sender, EventArgs e)
        {
            
        string rURL = Request.QueryString["ReturnUrl"];

        if (rURL != null)
        {
            Response.Redirect(rURL);
        }
        else
        {
            Response.Redirect("~/app/Default.aspx");
        }

        }

        protected void Login1_LoginError(object sender, EventArgs e)
        {
            MembershipUser user = Membership.GetUser(this.Login1.UserName);
            if ((user != null) && user.IsLockedOut)
            {
                this.Login1.FailureText = "&nbsp;<br/>Your account has been locked out for security reasons. <br/> Please contact us to unlock";
            }
            else
            {
                Login1.FailureText = "Invalid User name or password !";
            }
        }

        private void MessageBox(string msg)
        {
            Label child = new Label
            {
                Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>"
            };
            this.Page.Controls.Add(child);
        }

        protected void Login1_LoggingIn(object sender, LoginCancelEventArgs e)
        {


        }
    }
}