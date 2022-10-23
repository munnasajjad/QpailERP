using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class app_Production_Shift : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            EditField.Attributes.Add("class", "form-group hidden");
            txtDept.Focus();
        }

        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDept.Text != "")
            {
                if (btnSave.Text == "Save")
                {
                    ExecuteInsert();

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "New Shift Added Successfully";
                    txtDept.Focus();
                }
                else
                {
                    ExecuteUpdate();

                    txtDept.Text = "";
                    txtDesc.Text = "";
                    btnSave.Text = "Save";
                    EditField.Attributes.Add("class", "form-group hidden");
                    GridView1.DataBind();
                    DropDownList1.DataBind();
                    pnl.Update();

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Shift Info Updated Successfully";
                }
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Please write a Shift Name";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
        finally
        {
            GridView1.DataBind();
            txtDept.Text = "";
            txtDesc.Text = "";
        }
    }

    private void ExecuteInsert()
    {
        SqlCommand cmd2 = new SqlCommand("INSERT INTO Shifts (Section, DepartmentName, Description, ProjectID, EntryBy) VALUES ('" + ddSection.SelectedValue + "', @GroupName, @Description, @ProjectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@GroupName", txtDept.Text);
        cmd2.Parameters.AddWithValue("@Description", txtDesc.Text);
        cmd2.Parameters.AddWithValue("@ProjectID", lblProject.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE Shifts SET  Section='" + ddSection.SelectedValue + "', DepartmentName='" + txtDept.Text + "', Description='" + txtDesc.Text + "' where (Departmentid ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtDept.Text = "";
        txtDesc.Text = "";
        btnSave.Text = "Save";
        EditField.Attributes.Add("class", "form-group hidden");
        GridView1.DataBind();
        DropDownList1.DataBind();
        pnl.Update();

        lblMsg.Attributes.Add("class", "xerp_warning");
        lblMsg.Text = "Form Action Cancelled!";
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        EditMode();
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        DropDownList1.DataBind();
        DropDownList1.SelectedValue = lblItemName.Text;
        EditMode();
    }

    private void EditMode()
    {
        SqlCommand cmd7 = new SqlCommand("SELECT [DepartmentName], [Description], Departmentid FROM [Shifts] WHERE Departmentid=@Departmentid", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Parameters.Add("@Departmentid", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            txtDept.Text = dr[0].ToString();
            txtDesc.Text = dr[1].ToString();
        }
        cmd7.Connection.Close();
    }
    protected void ddSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
}
