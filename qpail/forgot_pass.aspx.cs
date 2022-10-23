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

public partial class forgot_pass : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Branding_Settings();
    }
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {             
        string sp = txtUserName.Text.ToLower().Trim();

        SqlCommand cmd = new SqlCommand("Select UserID from Members where UserID=@UserID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = sp;
        cmd.Connection.Open();
        string spMatch = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();
        cmd.Connection.Dispose();
        if (sp == "")
        {
            Literal1.Text = "Enter a valid user id";
            txtUserName.Focus();
        }
        else
        {
            if (spMatch != txtUserName.Text)
            {
                Literal1.Text = "Enter a valid user id";
                txtUserName.Focus();
            }
            else
            {
                MembershipUser user = Membership.GetUser(txtUserName.Text);
                string eMail = user.Email;
                string s = "";
                int countAt = 0;

                foreach (char c in eMail)
                {
                    if (c.ToString() != "@")
                    {
                        if (countAt == 0)
                        {
                            if (c.ToString() == ".")
                            {
                                s += c;
                            }
                            else
                            {
                                s += "*";
                            }
                        }
                        else
                        {
                            s += c;
                        }
                    }
                    else
                    {
                        countAt = 1;
                        s += c;
                    }
                }
                Literal2.Text = s;

                Guid guid1 = Guid.NewGuid();

                SqlCommand cmd50 = new SqlCommand("UPDATE Users set PassResetLink='" + guid1.ToString() + "' where Username='" + txtUserName.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd50.Connection.Open();
                cmd50.ExecuteNonQuery();
                cmd50.Connection.Close();

                SqlCommand cmdx = new SqlCommand("Select UserID from Members where UserID=@UserID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmdx.Parameters.Add("@UserID", SqlDbType.VarChar).Value = sp;
                cmdx.Connection.Open();
                string fn = Convert.ToString(cmdx.ExecuteScalar());
                cmdx.Connection.Close();

                string resetLink = "http://www.tviexpress.biz/reset_pass.aspx?reset=" + guid1.ToString();

                //sending mail
                var fromAddress = new MailAddress("noreply@tviexpress.biz", "TVI Express");
                var toAddress = new MailAddress(eMail, fn);
                var replyTo = new MailAddress("noreply@tviexpress.biz");
                const string fromPassword = "jDvm63*4";
                string subject = "Reset Password";

                string body = "Dear  <b>" + fn + "</b>,<br><br>We recently received a request to recover the password for the User ID <b>" + txtUserName.Text + "</b>.<br><br>";
                body += "Please click on the link below to reset your password: <br><br>";
                body += "<a href='" + resetLink + "'>" + resetLink + "</a><br><br>";

                body += "If you have not requested the same, please do not click on the link and your password will not be changed. The link above will expire in 48 hours.<br><br>";

                body += "Sincerely,<br><br>";
                body += "TVI Express Support<br>Touching Lives Globally!<br>";

                var smtp = new SmtpClient();
                {
                    smtp.Host = "tviexpress.biz";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(fromAddress.Address, fromPassword);
                    smtp.Timeout = 20000;
                }

                using (var message = new MailMessage(fromAddress, toAddress))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    message.ReplyTo = replyTo;
                    smtp.Send(message);
                }




                Panel1.Visible = false;
                Panel2.Visible = true;
            }
        }
    }
    protected void btnSubmit_Click1(object sender, EventArgs e)
    {

    }


    private void Branding_Settings()
    {
        try
        {
            SqlCommand cmd = new SqlCommand("SELECT TOP (1) sid, DevelopedBy, ProviderAddress, LoginLogo, InnerLogo, SoftwareName, SoftwareMode, ProviderURL, TrialDate FROM settings_branding where IsActive=1", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                //string sid = dr[0].ToString();
                string provider = dr[1].ToString();
                //string Sender = dr[2].ToString();
                string logo = dr[3].ToString();
                //string logo = dr[4].ToString();
                string sName = dr[5].ToString();

                string sMode = dr[6].ToString();
                string url = dr[7].ToString();
                string tDate = dr[8].ToString();

                //Developed By <a href="//xtremebd.com">Extreme Solutions</a>
                ltrDeveloper.Text = "Developed By <a href='" + url + "'>" + provider + "</a>";
                imgLogo.ImageUrl = "./branding/" + logo;
                imgLogo.AlternateText = sName + " by " + provider;

            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
        catch (Exception ex)
        {
            
        }
    }
}