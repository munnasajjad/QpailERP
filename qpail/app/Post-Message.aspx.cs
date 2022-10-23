using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using RunQuery;

public partial class AdminCentral_Post_Message : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
        Label1.Text = Page.User.Identity.Name.ToString();
            GridView1.DataBind();
        }

    }

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try {
            if (txtSubject.Text.Length<1)
        {
            Notify("Please write a subject", "error", lblMsg);
        }
        else if (txtMsgBody.Text.Length < 5)
        {
            Notify("Please write bigger message body", "error", lblMsg);
        }
        else
        {
            SQLQuery.ExecNonQry("INSERT INTO Messaging (Sender, Receiver, Subject, BodyText) " +
                                "VALUES  ('"+Page.User.Identity.Name.ToString()+ "', '"+ddUsers.SelectedValue+ "', '"+txtSubject.Text+ "', '"+txtMsgBody.Text+"')");
                Notify("Message successfully send!", "success", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
        
    }
}
