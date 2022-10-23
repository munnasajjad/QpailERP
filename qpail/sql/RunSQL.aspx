<%
    // Sample code for executing a T-SQL file using an ASP.NET page
    // Copyright (C) Microsoft Corporation, 2007.  All rights reserved.
    
    // Written as a sample with use in conjuction with the SQL Server Database Publishing Wizard
    // For more information visit http://www.codeplex.com/sqlhost/
    
    // **************************************************************************
    // Note: Please ensure that you delete this page once your database has been published to the remote server
    // **************************************************************************
      
     %>

<%@ Page Language="C#" AutoEventWireup="true"  %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>


<%
    // **************************************************************************
    // Update these variables here
    // **************************************************************************
    
    // Url of the T-SQL file you want to run
    string fileUrl = Server.MapPath("script.txt");  
    
    // Connection string to the server you want to execute against
    string connectionString = @"Server=.\MSSQLSERVER2012;Initial Catalog=xtreme_xerp;User ID=xerp;Password=Up95r0!u";
    
    // Timeout of batches (in seconds)
    int timeout = 600;


 %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Executing T-SQL</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
    <%
        SqlConnection conn = null;                   
        try
        {
            this.Response.Write(String.Format("Opening url {0}<BR>", fileUrl));
            
            // read file
            WebRequest request = WebRequest.Create(fileUrl);
            using (StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                this.Response.Write("Connecting to SQL Server database...<BR>");
                
                // Create new connection to database
                conn = new SqlConnection(connectionString);               
                
                conn.Open();

                while (!sr.EndOfStream)
                {
                    StringBuilder sb = new StringBuilder();
                    SqlCommand cmd = conn.CreateCommand();
                    
                    while (!sr.EndOfStream)
                    {
                        string s = sr.ReadLine();
                        if (s != null && s.ToUpper().Trim().Equals("GO"))
                        {
                            break;
                        }
                        
                        sb.AppendLine(s);
                    }

                    // Execute T-SQL against the target database
                    cmd.CommandText = sb.ToString();
                    cmd.CommandTimeout = timeout;

                    cmd.ExecuteNonQuery();
                }

            }
            this.Response.Write("T-SQL file executed successfully");
        }
        catch (Exception ex)
        {
            this.Response.Write(String.Format("An error occured: {0}", ex.ToString()));
        }
        finally
        {
            // Close out the connection
            //
            if (conn != null)
            {
                try
                {
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception e)
                {
                    this.Response.Write(String.Format(@"Could not close the connection.  Error was {0}", e.ToString()));
                }
            }
        }                       
                
        
         %>
</body>
</html>
