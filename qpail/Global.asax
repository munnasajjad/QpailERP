<%@ Application Language="C#" %>

<script runat="server">

    void app_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void app_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void app_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
    void app_BeginRequest(object sender, EventArgs e)
    {
        // Get the current path
        string CurrentURL_Path = Request.Path.ToLower();

        if (CurrentURL_Path.EndsWith(".aspx"))
        {
            //CurrentURL_Path = CurrentURL_Path.Trim("/");
            //string NewsID = CurrentPath.Substring(CurrentPath.IndexOf("/"));
            HttpContext MyContext = HttpContext.Current;
            //MyContext.RewritePath("/news-show.aspx?News=" + NewsID);
        }
    }
</script>
