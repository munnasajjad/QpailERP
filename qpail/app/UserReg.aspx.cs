using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using RunQuery;

public partial class Application_UserReg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtTime.Text = DateTime.Now.ToString();
    }
    protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {
        DropDownList ddCounter = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddCounter");
        DropDownList ddEmploye = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddEmploye");
        TextBox txtUser = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");

        if (ddCounter.SelectedValue == "Head Office")
        {
            Roles.AddUserToRole(CreateUserWizard1.UserName, "Admin");
        }
        else
        {
            Roles.AddUserToRole(CreateUserWizard1.UserName, "Operator");
        }

        SqlCommand cmd2c = new SqlCommand("INSERT INTO Logins (LoginUserName, EmployeeInfoID, CounterName)" +
                               "VALUES ('" + txtUser.Text + "','" + ddEmploye.SelectedValue + "', '" + ddCounter.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        GridView1.DataBind();
        cmd2c.Connection.Open();
        cmd2c.ExecuteNonQuery();
        cmd2c.Connection.Close();

    }
    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //DropDownList1.DataBind();
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label PID = GridView1.Rows[index].FindControl("Label1") as Label;
            lblid.Text = PID.Text;
            
            Panel1.Visible = true;
            pnlRegister.Visible = false;

            lblMsg.Attributes.Add("class", "isa_info");
            lblMsg.Text = "Edit mode activated ...";

            EditMode();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "isa_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }

    private void EditMode()
    {
        SqlCommand cmd7 = new SqlCommand("SELECT LoginUserName, EmployeeInfoID, CounterName FROM [Logins] WHERE LID='" + lblid.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            txtUserX.Text = dr[0].ToString();

            string eid = SQLQuery.ReturnString("SELECT EmployeeInfoID FROM [EmployeeInfo] WHERE EmployeeInfoID='" + dr[1].ToString() + "'");
            if (eid!="")
            {
                ddEmployeX.SelectedValue = eid;    
            }
            eid = SQLQuery.ReturnString("SELECT CounterName FROM [Counters] WHERE CounterName='" + dr[2].ToString() + "'");
            if (eid != "")
            {
                ddCounterX.SelectedValue = eid;
            }
            
        }
        cmd7.Connection.Close();
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;
            
            string uName = SQLQuery.ReturnString("Select LoginUserName from Logins where lid='" + lblItemCode.Text + "' ");
            SQLQuery.ExecNonQry(@"SELECT UserId, RoleId
                                into #temp
                                FROM  aspnet_UsersInRoles
                                WHERE UserId= (Select UserId from aspnet_Users where LoweredUserName='" + uName.ToLower() + "' )" +
                                @"DELETE FROM dbo.aspnet_Membership WHERE UserId IN (Select UserId from #temp)
                                DELETE FROM dbo.aspnet_UsersInRoles WHERE UserId IN (Select UserId from #temp)
                                DELETE FROM dbo.aspnet_Profile WHERE UserId IN (Select UserId from #temp)
                                DELETE FROM dbo.aspnet_PersonalizationPerUser WHERE UserId IN (Select UserId from #temp)
                                DELETE FROM dbo.aspnet_Users WHERE UserId IN (Select UserId from #temp)");

            SQLQuery.ExecNonQry("DELETE Logins WHERE LID=" + lblItemCode.Text);

            GridView1.DataBind();
            lblMsg.Attributes.Add("class", "isa_warning");
            lblMsg.Text = "Entry removed successfully ...";

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "isa_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("UserReg.aspx");
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        SQLQuery.ExecNonQry("Update Logins set EmployeeInfoID='" + ddEmployeX.SelectedValue + "', CounterName='" + ddCounterX.SelectedValue + "' where LID='" + lblid.Text + "' ");
        lblMsg.Text = "Login info Updated Successfully";

        Panel1.Visible = false;
        pnlRegister.Visible = true;
    }
}
