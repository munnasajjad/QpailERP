using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Operator_Item_Unit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //txtDept.Text = "Drinks3";
    }
    private void ExecuteInsert()
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            int prjId = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Connection.Close();

            SqlCommand cmd2 = new SqlCommand("INSERT INTO Units (UnitName, Company, ProjectID, EntryBy) VALUES (@GroupName, @Description, @ProjectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.AddWithValue("@GroupName", txtDept.Text);
            cmd2.Parameters.AddWithValue("@Description", txtDesc.Text);
            cmd2.Parameters.AddWithValue("@ProjectID", prjId);
            cmd2.Parameters.AddWithValue("@EntryBy", lName);

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();

            lblMsg.Text = "New Unit Added Successfully";
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in Save: " + ex.ToString();
        }
        finally
        {
            GridView1.DataBind();
            txtDept.Text = "";
            txtDesc.Text = "";
        }

    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        if (txtDept.Text != "")
        {
            ExecuteInsert();
        }
        else
        {
            lblMsg.Text = "Please type Item Unit Name";
        }
    }
    protected void btnClear_Click1(object sender, EventArgs e)
    {
        txtDept.Text = "";
        txtDesc.Text = "";
    }
}
