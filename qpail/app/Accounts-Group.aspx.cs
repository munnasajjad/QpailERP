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


public partial class app_Accounts_Group : System.Web.UI.Page
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

            string itemExist = SQLQuery.ReturnString("SELECT GroupName FROM ItemGroup WHERE GroupName ='" + txtDept.Text + "'");

            if (itemExist != txtDept.Text)
            {
                SqlCommand cmd2 = new SqlCommand("INSERT INTO ItemGroup (GroupName, Description, ProjectID, EntryBy) VALUES (@GroupName, @Description, @ProjectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.AddWithValue("@GroupName", txtDept.Text);
                cmd2.Parameters.AddWithValue("@Description", txtDesc.Text);
                cmd2.Parameters.AddWithValue("@ProjectID", prjId);
                cmd2.Parameters.AddWithValue("@EntryBy", lName);

                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
                lblMsg.Text = "New Item Group Added Successfully";
                lblMsg.Attributes.Add("class", "xerp_success");
            }
            else
            {
                lblMsg.Text = "ERROR: Info already exist!";
                lblMsg.Attributes.Add("class", "xerp_error");
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "ERROR: " + ex.ToString();
            lblMsg.Attributes.Add("class", "xerp_error");
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
            lblMsg.Text = "Please type Item Group Name";
        }
    }
    protected void btnClear_Click1(object sender, EventArgs e)
    {
        txtDept.Text = "";
        txtDesc.Text = "";
    }

    //Messagebox For Alerts    
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }


}
