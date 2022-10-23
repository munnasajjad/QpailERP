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

public partial class Super_Admin_ResetPassword : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        this.btnSave.Attributes.Add("onclick", " this.disabled = true; " + base.ClientScript.GetPostBackEventReference(this.btnSave, null) + ";");
        if (!IsPostBack)
        {
            string lName = User.Identity.Name.Trim().ToLower();
            if (lName != "rony" && lName != "")
            {
                Response.Redirect("./Default.aspx");
            }
        }
        }

        protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string text = this.txtUid.Text;
            string newPassword = this.txtPassword.Text;
            MembershipUser user = Membership.GetUser(text);
            user.ChangePassword(user.ResetPassword(), newPassword);
            this.txtPassword.Text = "";
            this.lblMsg.Text = "Password Reset Successfull.";
            this.MessageBox("Password Reset Successfull.");
        }
        catch (Exception exception)
        {
            this.lblMsg.Text = "ERROR: " + exception.ToString();
            this.MessageBox("Password Reset Failed!");
        }
    }

    protected void btnSave0_Click(object sender, EventArgs e)
    {
        try
        {
            string uid=txtUid.Text.ToLower().Trim();

            if (uid != "" || uid!="rony")
            {
                MembershipUser user = Membership.GetUser(txtUid.Text);
                this.lblCurrEmail.Text = user.Email;

                //SqlCommand cmd2 = new SqlCommand("Select Password from TrPassword where MemberID= '" + txtUid.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //cmd2.Connection.Open();
                //lblTrpass.Text = Convert.ToString(cmd2.ExecuteScalar());
                //cmd2.Connection.Close();

                //lblName.Text = RunQuery.SQLQuery.ReturnString("SELECT FirstName + ' ' + LastName FROM Members WHERE (UserID = '" + uid + "')");
            }
            else
            {
                lblCurrEmail.Text = "You Typed an Invalid ID";
            }
        }
        catch (Exception ex)
        {
            this.lblCurrEmail.Text = ex.Message.ToString();
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

}