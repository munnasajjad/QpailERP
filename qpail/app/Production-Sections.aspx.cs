using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class app_Production_Sections : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();

        if (!IsPostBack)
        {
            EditField.Attributes.Add("class", "form-group hidden");
            txtDept.Focus();
            ddDept.DataBind();
            GridView1.DataBind();
            ddGodown.DataBind();
            ddLocation.DataBind();
            ddWastageGodown.DataBind();
            ddWastageLocation.DataBind();
            ddSubGrp.DataBind();
            GetGrade();
            GetCategory();
        }

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
        SqlCommand cmd2 = new SqlCommand("INSERT INTO Sections (DepartmentID, SName, EntryBy, ProjectID, MachineLinkItemSubgroupID, GradeId, CategoryId, GodownID, LocationID, IsPrdSection) " +
                                         " VALUES (@DepartmentID, @SName, @EntryBy, @ProjectID,  @MachineLinkItemSubgroupID, '" + ddGrade.SelectedValue + "', '" + ddcategory.SelectedValue + "',@GodownID, @LocationID, 1)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@DepartmentID", ddDept.SelectedValue);
        cmd2.Parameters.AddWithValue("@SName", txtDept.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());
        cmd2.Parameters.AddWithValue("@ProjectID", lblProject.Text);
        cmd2.Parameters.AddWithValue("@MachineLinkItemSubgroupID", ddSubGrp.SelectedValue);
        cmd2.Parameters.AddWithValue("@GodownID", ddGodown.SelectedValue);
        cmd2.Parameters.AddWithValue("@LocationID", ddLocation.SelectedValue);
        cmd2.Parameters.AddWithValue("@WastageGodownId", ddWastageGodown.SelectedValue);
        cmd2.Parameters.AddWithValue("@WastageGodownLocationId", ddWastageLocation.SelectedValue);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE Sections SET DepartmentID='" + ddDept.SelectedValue + "', SName='" + txtDept.Text + "', MachineLinkItemSubgroupID='" + ddSubGrp.SelectedValue +
            "', GradeId='" + ddGrade.SelectedValue + "', CategoryId='" + ddcategory.SelectedValue + "', GodownID='"+ddGodown.SelectedValue+"', LocationID='"+ddLocation.SelectedValue+ "', WastageGodownId='" + ddWastageGodown.SelectedValue + "', WastageGodownLocationId='" + ddWastageLocation.SelectedValue + "', IsPrdSection=1 where (SID ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
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
        SqlCommand cmd7 = new SqlCommand("SELECT DepartmentID, SName, SID, MachineLinkItemSubgroupID, GradeId, CategoryId, GodownID, LocationID,  WastageGodownId, WastageGodownLocationId FROM Sections WHERE SID=@Departmentid", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Parameters.Add("@Departmentid", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            //EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            ddDept.SelectedValue = dr[0].ToString();
            txtDept.Text = dr[1].ToString();
            string subgrp = dr[3].ToString();
            ddSubGrp.SelectedValue = subgrp;
            GetGrade();

            if (dr[4].ToString() != "")
            {
                ddGrade.SelectedValue = dr[4].ToString();
            }
            GetCategory();
            if (dr[5].ToString() != "")
            {
                ddcategory.SelectedValue = dr[5].ToString();
            }
            ddGodown.DataBind();
            if (dr[6].ToString() != "")
            {
                ddGodown.SelectedValue = dr[6].ToString();
            }
            ddLocation.DataBind();
            if (dr[7].ToString() != "")
            {
                ddLocation.SelectedValue = dr[7].ToString();
            }
            ddWastageGodown.DataBind();
            if (dr[8].ToString() != "")
            {
                ddWastageGodown.SelectedValue = dr[8].ToString();
            }
            ddWastageLocation.DataBind();
            if (dr[9].ToString() != "")
            {
                ddWastageLocation.SelectedValue = dr[9].ToString();
            }
        }
        cmd7.Connection.Close();
        chkMachineSection();
        //ddSubGrp.DataBind();
    }

    protected void ddDept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        chkMachineSection();
    }

    private void chkMachineSection()
    {
        if (ddDept.SelectedValue == "5" || ddDept.SelectedItem.Text.IndexOf("Production") > -1)
        {
            pnlMachine.Visible = true;
            GetGrade();
        }
        else
        {
            pnlMachine.Visible = false;
            ddSubGrp.Items.Clear();
            ddSubGrp.Items.Add("");
            ddSubGrp.DataBind();

        }
    }

    protected void ddGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetCategory();
    }

    protected void ddcategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //GetCategory();
    }

    private void GetGrade()
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
    }
    private void GetCategory()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddcategory, "CategoryID", "CategoryName");
    }

    protected void ddSubGrp_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetGrade();
        GetCategory();
    }

    protected void ddGodown_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocation.DataBind();
    }
}
