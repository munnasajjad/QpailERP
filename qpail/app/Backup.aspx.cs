using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ionic.Zip;

public partial class Application_Backup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnBackup.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnBackup, null) + ";");

        try
        {
            // Retrieve the ConnectionString from App.config 
            string connectString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectString);
            // Retrieve the DataSource property.    
            txtDB.Text = builder.InitialCatalog;
        }
        catch (Exception ex)
        {
            lblError.Text = ex.ToString();
        }
    }

    protected void btnBackup_OnClick(object sender, EventArgs e)
    {
        string YourDBName = txtDB.Text;
        //IF SQL Server Authentication then Connection String  
        //con.ConnectionString = @"Server=MyPC\SqlServer2k8;database=" + YourDBName + ";uid=sa;pwd=password;";  

        //IF Window Authentication then Connection String  
        //con.ConnectionString = @"Server=MyPC\SqlServer2k8;database=Test;Integrated Security=true;";

        string fileName = YourDBName + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak";
        string backupDIR = Server.MapPath("../sql/"); //  //"E:\\BackupDB";
        string file2Delete=Server.MapPath("..\\SQL\\")+fileName;


        if (!System.IO.Directory.Exists(backupDIR))
        {
            System.IO.Directory.CreateDirectory(backupDIR);
        }
        try
        {
            SqlCommand cmd21 = new SqlCommand("backup database " + YourDBName + " to disk='" + backupDIR + fileName+"'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd21.Connection.Open();
            cmd21.ExecuteNonQuery();
            cmd21.Connection.Close();
            
            lblError.Text = "Backup operation completed successfully!";
            ZipFile(YourDBName, backupDIR + fileName, "~/sql/db.zip");
        }
        catch (Exception ex)
        {
            lblError.Text = "Error Occured During DB backup process !<br>" + ex.ToString();
        }  
    }

    private void ZipFile(string fn, string fileLocation, string saveLocation)
    {
        fn = fn + "-" + DateTime.Now.ToString("ddMMhh");
        Response.ContentType = "application/zip";
        Response.AddHeader("content-disposition", "attachment;filename=" + fn + ".zip");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        if (File.Exists(Server.MapPath(saveLocation)))
        {
            File.Delete(Server.MapPath(saveLocation));
        }

        ZipFile createZipFile = new ZipFile();

        createZipFile.Password = "3p3200170";
        createZipFile.Encryption = EncryptionAlgorithm.PkzipWeak;
        createZipFile.AddFile(fileLocation, string.Empty);
        createZipFile.Save(Server.MapPath(saveLocation));
        
        if (File.Exists(fileLocation))
        {
            File.Delete(fileLocation);
        }
        
        Response.TransmitFile(saveLocation);
        Response.End();
    }
}