using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Currencies : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
    }

    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox)
        { (Parent as TextBox).Text = string.Empty; }
        else
        {
            foreach (Control c in Parent.Controls)
                ClearControls(c);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCName.Text != "" && txtSymble.Text != "" && txtRate.Text!="")
            {
                SqlCommand cmd10z = new SqlCommand("INSERT INTO Currencies (CrName, ShortCode, Country, xRate) VALUES ('" + txtCName.Text + "', '" + txtSymble.Text + "', NULL, '" + txtRate.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd10z.CommandType = CommandType.Text;

                cmd10z.Connection.Open();
                cmd10z.ExecuteNonQuery();
                cmd10z.Connection.Close();
                ClearControls(Page);
                GridView1.DataBind();
                lblMsg.Text = "Successfully Saved The New Currency";
            }
            else
            {
                MessageBox("Empty data is not accepted.");
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Unable to Save: " + ex.ToString();
        }
    }
}