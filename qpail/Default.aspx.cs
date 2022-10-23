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

public partial class _Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        //DateTime dt = DateTime.Today;
        //DateTime dtc = new DateTime();
        //string D = "30/06/2016 00:00:00";
        //dtc = Convert.ToDateTime(D);
        //if (dt > dtc)
        //{
        //    Response.Redirect("~/Default2.aspx");
        //}

        Response.Redirect("~/Login.aspx");
    }
    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    protected void Login1_LoggedIn(object sender, EventArgs e)
    {
        // please don't use User.IsInRole here , because it will not be populated yet at this stage.

        if (Roles.IsUserInRole(Login1.UserName, "Admin"))
        {
            Response.Redirect("~/app/Default.aspx");
        }
        else if (Roles.IsUserInRole(Login1.UserName, "Branch"))
        {
            //if (Request.QueryString["ReturnUrl"] != null)
            //{
            //    Response.Redirect(Request.QueryString["ReturnUrl"]);
            //}
            //else
            //{
            Response.Redirect("~/Branch/Default.aspx");
            //}
        }
        else
        {
            Response.Redirect("~/Cells/Default.aspx");
        }
    }
    protected void Login1_LoginError(object sender, EventArgs e)
    {

        //String message = Login1.FailureText.ToString();
        MembershipUser membershipUser = Membership.GetUser(Login1.UserName);      //.GetUser(false);

        if (membershipUser != null)
        {
            bool IsLockedOut = membershipUser.IsLockedOut;

            //if (userInfo == null)
            //{
            //    Login1.errPayMsg.Text = String.Empty;
            //}

            //else if (!(userInfo.IsApproved))
            //    LoginErrorText.Text = "Your account has not been approved yet, Please follow instructions in the confirmation email";

            //else 
            if (IsLockedOut == true)
            {
                Login1.FailureText = "&nbsp;<br/>Your account has been locked out for security reasons. <br/> Please contact us to unlock";
                //Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                //MessageBox("Your account has been locked out for security reasons");
            }
            //Response.Redirect("Default.aspx");
            //else
            //{
            //    Login1.FailureText = String.Empty;
            //}
        }
        else
        {
            MessageBox("This is not the place you are supposed to enter");
            pnlLogin.Visible = false;
            PanelError.Visible = true;
            //Login1.FailureText = message;
            Label1.Text = "Invalid Login Attempt Detected!";
        }
    }
}
