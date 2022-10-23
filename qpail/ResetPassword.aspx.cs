using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Web.Security;
using System.Configuration.Provider;

public partial class ResetPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void PasswordRecovery1_SendingMail(object sender, MailMessageEventArgs e)
    {
        SmtpClient mySmtpClient = new SmtpClient();
        mySmtpClient.EnableSsl = true;
        mySmtpClient.Send(e.Message);
        e.Cancel = true;

    }
}
