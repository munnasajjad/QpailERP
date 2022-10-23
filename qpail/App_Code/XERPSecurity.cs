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
using System.IO;
using System.Net;
using System.Net.Mail;
using PostmarkDotNet;
using RunQuery;

/// <summary>
/// Summary description for SendEmail
/// </summary>

namespace XERPSecurity
{
    public class XERPSecure
    {
        public static int CheckPermissionLevel(string user1)
        {
            string query = SQLQuery.ReturnString("Select UserLevel from logins where LoginUserName ='" + user1 + "'");
            return Convert.ToInt32(query);
        }
        public static int HideMainMenu(string user1, string deptId)
        {
            int toHide = 1;
            int permissionLevel = CheckPermissionLevel(user1);
            if (permissionLevel>2)// Normal User
            {
                string empDept = SQLQuery.ReturnString("Select DepartmentID from EmployeeInfo where EmployeeInfoID =(Select EmployeeInfoID from logins where LoginUserName ='" + user1 + "')");
                if (empDept == deptId)
                {
                    toHide = 0;
                    //menuPanelId.Visible = false;
                }
            }

            return toHide;
        }

        public static void ProcessEmail(string Sender, string Receiver, string Subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("noreply@tviexpress.biz", "TVI Express");

            message.To.Add(new MailAddress("ronyjob@gmail.com"));
            //message.To.Add(new MailAddress("recipient2@foo.bar.com"));
            //message.To.Add(new MailAddress("recipient3@foo.bar.com"));

            //message.CC.Add(new MailAddress("carboncopy@foo.bar.com"));
            message.Subject = "This is my new subject";
            message.Body = "This is the new content<br/>" + DateTime.Now.ToString();
            message.IsBodyHtml = true;

            using (SmtpClient client = new SmtpClient())
            {
                client.Send(message);
            }

        }
        public static string EmailPostmarkapp(string Receiver, string Subject, string body)
        {
            var postmark = new PostmarkClient("1c947ef0-050a-40f6-8e72-a1207b09a27b");

            var message = new PostmarkMessage
            {
                To = Receiver,
                From = "info@extreme.com.bd", // This must be a verified sender signature
                Subject = Subject,
                TextBody = "plain text",
                HtmlBody = body
            };

            var response = postmark.SendMessage(message);
            /*
            // Send an email asynchronously:
            var message = new PostmarkMessage()
            {
                To = "rony@extreme.com.bd",
                From = "info@extreme.com.bd",
                TrackOpens = true,
                Subject = "A complex email",
                TextBody = "Plain Text Body",
                HtmlBody = "<html><body><img src=\"cid:embed_name.jpg\"/></body></html>",
                Tag = "New Year's Email Campaign",
                Headers = new WebHeaderCollection(){
    {"X-CUSTOM-HEADER", "Header content"}
  }
            };

            var imageContent = File.ReadAllBytes("test.jpg");
            message.AddAttachment(imageContent, "test.jpg", "image/jpg", "cid:embed_name.jpg");

            var client = new PostmarkClient("1c947ef0-050a-40f6-8e72-a1207b09a27b");
            var sendResult = await client.Sen(message);

            if (sendResult.Status == PostmarkStatus.Success)
            {
            // Handle success
            }
            else
        {
            // Resolve issue.
        }
        */

            return response.ToString();
        }
    }
}