using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class app_NewsEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            populateNews();

        }
    }
    private void populateNews()
    {
        SqlCommand cmd1 = new SqlCommand("SELECT Headline FROM NewsUpdates", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd1.Connection.Open();
        SqlDataReader Reflist = cmd1.ExecuteReader();

        ddProject.DataSource = Reflist;
        ddProject.DataValueField = "Headline";
        ddProject.DataTextField = "Headline";
        ddProject.DataBind();

        cmd1.Connection.Close();
        cmd1.Connection.Dispose();
        populateDetails();
    
    }
    private void populateDetails()
    {
        SqlCommand cmd1 = new SqlCommand("SELECT FullNews FROM NewsUpdates where Headline='"+ddProject.SelectedValue+"'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd1.Connection.Open();

        string detail = Convert.ToString(cmd1.ExecuteScalar());
        msgMPO.Text = detail;
        cmd1.Connection.Close();
        cmd1.Connection.Dispose();
        
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlCommand cmd3 = new SqlCommand("Update NewsUpdates set FullNews=@fn  where Headline='" + ddProject.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd3.Parameters.Add("@hl", SqlDbType.NVarChar).Value = ddProject.SelectedValue;
        cmd3.Parameters.Add("@fn", SqlDbType.NVarChar).Value = msgMPO.Text.ToString();

        cmd3.Connection.Open();
        if (msgMPO.Text != "")
        {
            cmd3.ExecuteNonQuery();
        }
        else
        { lblMsg.Text = "Add Something New at News Content"; }
        cmd3.Connection.Close();

        lblMsg.Text = "Successfully Saved The News";
    }

    private void LoadData()
    {

    }
    protected void ddProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        populateDetails();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand("Delete NewsUpdates  where Headline='" + ddProject.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();

        //cmd.Parameters.Add("@pkg", SqlDbType.Int).Value = txtId.Text;

        cmd.ExecuteReader();
        lblMsg.Text = "News Data Deleted Successfully";

        cmd.Connection.Close();
        cmd.Connection.Dispose();
        populateNews();
    }
}
