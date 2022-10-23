using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration.Provider;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Data.SqlClient;
using RunQuery;

public partial class AdminCentral_New_User : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string query = "SELECT LevelID, LevelName FROM [UserLevel] WHERE LevelID > (Select UserLevel from logins where LoginUserName ='" + Page.User.Identity.Name.ToString() + "') ORDER BY [LevelID]";
            //txtCurrentPosition.Text = SQLQuery.ReturnString("");      
            SQLQuery.PopulateDropDown(query, ddLevelX, "LevelID", "LevelName");
            DropDownList ddLevel = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddLevel");
            SQLQuery.PopulateDropDown(query, ddLevel, "LevelID", "LevelName");
        }
    }
    protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {
        DropDownList ddLevel = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddLevel");
        DropDownList ddCounter = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddCounter");
        DropDownList ddEmploye = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddEmploye");
        TextBox txtUser = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");

        Roles.AddUserToRole(CreateUserWizard1.UserName, ddLevel.SelectedItem.Text);
        SqlCommand cmd2c = new SqlCommand("INSERT INTO Logins (LoginUserName, EmployeeInfoID, UserLevel)" +
                               "VALUES ('" + txtUser.Text + "','" + ddEmploye.SelectedValue + "', '" + ddLevel.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2c.Connection.Open();
        cmd2c.ExecuteNonQuery();
        cmd2c.Connection.Close();
        GridView1.DataBind();
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
        SqlCommand cmd7 = new SqlCommand("SELECT LoginUserName, EmployeeInfoID, UserLevel FROM [Logins] WHERE LID='" + lblid.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            txtUserX.Text = dr[0].ToString();

            string eid = SQLQuery.ReturnString("SELECT EmployeeInfoID FROM [EmployeeInfo] WHERE EmployeeInfoID='" + dr[1].ToString() + "'");
            if (eid != "")
            {
                ddCounterX.SelectedValue = SQLQuery.ReturnString("SELECT DepartmentID FROM [EmployeeInfo] WHERE EmployeeInfoID='" + dr[1].ToString() + "'");
                ddEmployeX.DataBind();
                ddEmployeX.SelectedValue = eid;
            }

            eid = dr[2].ToString(); //SQLQuery.ReturnString("SELECT UserLevel FROM [Counters] WHERE UserLevel='" + dr[2].ToString() + "'");
            if (eid != "")
            {
                ddLevelX.SelectedValue = eid;
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
            SQLQuery.ExecNonQry(@"SELECT UserId, RoleId into #temp
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
        Response.Redirect("New-user.aspx");
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        SQLQuery.ExecNonQry("Update Logins set EmployeeInfoID='" + ddEmployeX.SelectedValue + "', UserLevel='" + ddLevelX.SelectedValue + "', LastModifiedBy='" + Page.User.Identity.Name + "', LastModifiedOn='" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE LID='" + lblid.Text + "' ");
        lblMsg.Text = "Login info Updated Successfully";

        Panel1.Visible = false;
        pnlRegister.Visible = true;
    }

    protected void ddCounter_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddEmploye = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddEmploye");
        ddEmploye.DataBind();
    }
}
