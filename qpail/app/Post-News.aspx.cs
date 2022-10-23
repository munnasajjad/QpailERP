using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class AdminCentral_Post_News : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        txtHeading.Focus();
    }

    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string msgType = "News";
        SqlCommand cmd3 = new SqlCommand("INSERT INTO NewsUpdates (Headline, FullNews, Msgfor) VALUES  (@hl, @fn, @Msgfor)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd3.Parameters.Add("@hl", SqlDbType.NVarChar).Value = txtHeading.Text;
        cmd3.Parameters.Add("@fn", SqlDbType.NVarChar).Value = txtMsgBody.Text.ToString();
        cmd3.Parameters.Add("@Msgfor", SqlDbType.NVarChar).Value = msgType;

        cmd3.Connection.Open();
        if (txtMsgBody.Text != "")
        {
            cmd3.ExecuteNonQuery();
        }
        else
        { lblMsg.Text = "Add Something New at News Content"; }
        cmd3.Connection.Close();

        lblMsg.Text = "Successfully Saved The News";
        txtMsgBody.Text = "";
        MessageBox("Successfully Post the News.");
    }
}
