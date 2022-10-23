using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_hrm_shift : System.Web.UI.Page
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

                    btnSave.Text = "Save";
                    EditField.Attributes.Add("class", "form-group hidden");
                    GridView1.DataBind();
                    DropDownList1.DataBind();
                    pnl.Update();

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Shift Info Updated Successfully";
                }
                txtDept.Text = "";
                txtDesc.Text = "";
                txtCheckIn.Text = "";
                txtCheckOut.Text = "";
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
        string dt = DateTime.Now.ToString("yyyy-MM-dd") + " ";
        SqlCommand cmd2 = new SqlCommand("INSERT INTO EmpShifts (GroupSrNo, ShiftName, InTime, OutTime, Description, EntryBy) " +
                           "VALUES ('" + ddSection.SelectedValue + "', '" + txtDept.Text + "', '" + dt + txtCheckIn.Text +
                           "', '" + dt + txtCheckOut.Text + "', '" + txtDesc.Text + "', '" +
                           Page.User.Identity.Name.ToString() + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        string dt = DateTime.Now.ToString("yyyy-MM-dd") + " ";
        SqlCommand cmd2 = new SqlCommand("UPDATE EmpShifts SET ShiftName='" + txtDept.Text +
                "', InTime='" + dt + txtCheckIn.Text + "', OutTime= '" + dt + txtCheckOut.Text + "', Description='" + txtDesc.Text + "' where (sid ='" + DropDownList1.SelectedValue + "')",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
        SqlCommand cmd7 = new SqlCommand("SELECT  ShiftName, InTime, OutTime, Description FROM [EmpShifts] WHERE sid=@Departmentid", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Parameters.Add("@Departmentid", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            //EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            txtDept.Text = dr[0].ToString();
            txtCheckIn.Text = dr[1].ToString().Substring(11, 5); ;
            txtCheckOut.Text = dr[2].ToString().Substring(11, 5); ;
            txtDesc.Text = dr[3].ToString();
        }
        cmd7.Connection.Close();
    }
    protected void ddSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
}
