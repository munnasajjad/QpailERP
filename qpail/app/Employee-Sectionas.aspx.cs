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

public partial class app_Employee_Sectionas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            EditField.Attributes.Add("class", "form-group hidden");
            txtDept.Focus();

            ddDept.DataBind();
            GridView1.DataBind();
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
                    lblMsg.Text = "New Section Added Successfully";
                    txtDept.Focus();
                }
                else
                {
                    ExecuteUpdate();

                    txtDept.Text = "";
                    //txtDesc.Text = "";
                    btnSave.Text = "Save";
                    EditField.Attributes.Add("class", "form-group hidden");
                    GridView1.DataBind();

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Section Info Updated Successfully";
                }
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Please write a Department Name";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error in Save: " + ex.Message.ToString();
        }
        finally
        {
            DropDownList1.DataBind();
            GridView1.DataBind();
            txtDept.Text = "";
            //txtDesc.Text = "";
        }
    }

    private void ExecuteInsert()
    {
        SqlCommand cmd2 = new SqlCommand("INSERT INTO Sections (DepartmentID, SName, EntryBy, ProjectID, SalaryRulesID) VALUES (@DepartmentID, @SName, @EntryBy, @ProjectID, '"+ddBasis.SelectedValue+"')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@DepartmentID", ddDept.SelectedValue);
        cmd2.Parameters.AddWithValue("@SName", txtDept.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());
        cmd2.Parameters.AddWithValue("@ProjectID", lblProject.Text);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        SQLQuery.ExecNonQry("UPDATE Sections SET DepartmentID='" + ddDept.SelectedValue + "', SName='" + txtDept.Text +
                "', SalaryRulesID= '" + ddBasis.SelectedValue + "' where (SID ='" + DropDownList1.SelectedValue + "')");
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtDept.Text = "";
        //txtDesc.Text = "";
        btnSave.Text = "Save";
        EditField.Attributes.Add("class", "form-group hidden");
        GridView1.DataBind();
        DropDownList1.DataBind();
        

        lblMsg.Attributes.Add("class", "xerp_warning");
        lblMsg.Text = "Form Action Cancelled!";
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            EditMode();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error: " + ex.Message.ToString();
        }
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        DropDownList1.SelectedValue = lblItemName.Text;
        EditMode();
    }

    private void EditMode()
    {
        SqlCommand cmd7 = new SqlCommand("SELECT [DepartmentID], [SName], SID, MachineLinkItemSubgroupID, SalaryRulesID FROM [Sections] WHERE SID=@Departmentid", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Parameters.Add("@Departmentid", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            //EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            ddDept.SelectedValue = dr[0].ToString();
            txtDept.Text = dr[1].ToString();
            string subgrp=dr[4].ToString();
            if (subgrp!="")
            {
                ddBasis.SelectedValue=subgrp;
            }
             
        }
        cmd7.Connection.Close();

        //chkMachineSection();
        //ddSubGrp.DataBind();
    }

    protected void ddDept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
        //chkMachineSection();
    }

    //private  void chkMachineSection()
    //{
    //    if (ddDept.SelectedValue == "5" || ddDept.SelectedItem.Text.IndexOf("Production")>-1 )
    //    {
    //        pnlMachine.Visible = true;
    //    }
    //    else
    //    {
    //        pnlMachine.Visible = false;
    //        ddSubGrp.Items.Clear();
    //        ddSubGrp.Items.Add("");
    //        ddSubGrp.DataBind();
    //    }
    //}
}
