using System;
using System.Collections.Generic;

using System.Web;

/// <summary>
/// Summary description for URLRewriter
/// </summary>
public class URLRewriter : IHttpModule
{
    public void Init(HttpApplication inst)
    {
        inst.BeginRequest += new EventHandler(OnBeginRequest);
    }

    public void OnBeginRequest(Object s, EventArgs e)
    {
        HttpApplication inst = s as HttpApplication;
        string req_path = inst.Context.Request.Path;
        string trans_path = "";
        switch (req_path.ToLower())
        {
            case "localhost/courier/Cells/Pending-Delivery.aspx":
                trans_path = "http://localhost/courier/Cells/Pending-Delivery/physical/path/to/Pending";
                break;
            case "/virtual/path/to/page2":
                trans_path = "/physical/path/to/page.aspx?page=2";
                break;
            default:
                trans_path = "/";
                break;
        }
        inst.Context.Server.Transfer(trans_path);
    }

    public void Dispose() { }

}