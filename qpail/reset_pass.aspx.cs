using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.IO;

public partial class reset_pass : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
            try
            {
                string resetid = base.Request.QueryString["reset"];

                if (resetid == "")
                {
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("Select Username from Users where PassResetLink=@UserID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = resetid;
                    cmd.Connection.Open();
                    resetid = Convert.ToString(cmd.ExecuteScalar());
                    cmd.Connection.Close();

                    if (resetid == "")
                    {
                        Panel1.Visible = false;
                        Panel2.Visible = true;
                    }
                    else
                    {
                        Literal2.Text = resetid;
                    }
                }
            }
            catch
            {
                Response.Redirect("Default.aspx");
            }
        
    }
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        if (txtPass.Text.Length < 5)
        {
            txtPass.Focus();
            MessageBox("Type min. 5 Characters for password.");
        }
        else if (txtPass.Text != txtPassConfirm.Text)
        {
            txtPassConfirm.Focus();
            MessageBox("Confirm your password.");
        }
        else
        {
            string newPassword = txtPass.Text;
            MembershipUser user = Membership.GetUser(Literal2.Text);
            user.ChangePassword(user.ResetPassword(), newPassword);

            SqlCommand cmd50 = new SqlCommand("UPDATE Users set PassResetLink='' where Username='" + Literal2.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd50.Connection.Open();
            cmd50.ExecuteNonQuery();
            cmd50.Connection.Close();

            Response.Redirect("Login.aspx");
        }
    }
}