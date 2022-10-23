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

public partial class Operator_Item_Category : System.Web.UI.Page
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
            PopulateDropDowns();
        }
    }
    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox) { (Parent as TextBox).Text = string.Empty; } else { foreach (Control c in Parent.Controls) ClearControls(c); }
    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        try
        {
            if (txtDept.Text != "" && ddGrade.SelectedValue != "" && ddSubGrp.SelectedValue != "")
            {
                if (btnSave.Text == "Save")
                {
                    SqlCommand cmde = new SqlCommand("SELECT CategoryName FROM Categories WHERE GradeID ='" + ddGrade.SelectedValue + "' AND  CategoryName ='" + txtDept.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmde.Connection.Open();
                    string isExist = Convert.ToString(cmde.ExecuteScalar());
                    cmde.Connection.Close();

                    if (isExist == "")
                    {

                        ExecuteInsert();
                        EditField.Attributes.Add("class", "form-group hidden");
                        txtDept.Text = "";
                        txtDesc.Text = "";

                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "New category added successfully";
                    }
                    else
                    {
                        lblMsg.Attributes.Add("class", "xerp_error");
                        lblMsg.Text = "Error: Category already exist!";
                    }
                }
                else
                {
                    ExecuteUpdate();
                    ClearControls(Form);
                    btnSave.Text = "Save";
                    EditField.Attributes.Add("class", "form-group hidden");

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Item category updated successfully";
                }

            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Please fillup all mendatory fields...";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error: " + ex.ToString();
        }
        finally
        {
            GridView1.DataBind();
            DropDownList1.DataBind();
            txtDept.Focus();
        }

    }
    private void ExecuteInsert()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        int prjId = Convert.ToInt32(cmd.ExecuteScalar());
        cmd.Connection.Close();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO Categories (GradeID, CategoryName, Description, ProjectID, EntryBy) VALUES (@GroupID, @GradeName, @Description, @ProjectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@GroupID", ddGrade.SelectedValue);
        cmd2.Parameters.AddWithValue("@GradeName", txtDept.Text);
        cmd2.Parameters.AddWithValue("@Description", txtDesc.Text);
        cmd2.Parameters.AddWithValue("@ProjectID", prjId);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE Categories SET CategoryName='" + txtDept.Text + "', Description='" + txtDesc.Text + "', GradeID='" + ddGrade.SelectedValue + "' where (CategoryID ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();

        //SqlCommand cmd = new SqlCommand("UPDATE Items SET ItemType='" + txtDept.Text + "', GroupID='" + ddGroup.SelectedValue + "'  where (ItemType ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmd.Connection.Open();
        //cmd.ExecuteNonQuery();
        //cmd.Connection.Close();
        //cmd.Connection.Dispose();
    }

    //get & bind dropdownlists
    private void PopulateDropDowns()
    {
        string gQuery = "SELECT GroupSrNo,[GroupName] FROM [ItemGroup] Where ProjectID='" + lblProject.Text + "' ORDER BY [GroupSrNo]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGroup, "GroupSrNo", "GroupName");

        gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
    }

    protected void btnClear_Click1(object sender, EventArgs e)
    {
        CancelForm();
    }


    private void EditMode()
    {
        string itemToEdit = DropDownList1.SelectedValue;

        string grdID = RunQuery.SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + itemToEdit + "'");
        string subID = RunQuery.SQLQuery.ReturnString("SELECT CategoryID FROM ItemGrade WHERE GradeID ='" + grdID + "'");
        string grpID = RunQuery.SQLQuery.ReturnString("SELECT GroupID FROM ItemSubGroup WHERE CategoryID ='" + subID + "'");

        ddGroup.SelectedValue = grpID;

        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + grpID + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");
        ddSubGrp.SelectedValue = subID;

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + subID + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
        ddGrade.SelectedValue = grdID;


        SqlCommand cmd7 = new SqlCommand("SELECT CategoryName, Description, GradeID FROM [Categories] WHERE CategoryID='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            txtDept.Text = dr[0].ToString();
            txtDesc.Text = dr[1].ToString();
            ddGrade.SelectedValue = dr[2].ToString();
        }
        cmd7.Connection.Close();

        //SqlCommand cmd1 = new SqlCommand("SELECT CategoryID FROM ItemGrade WHERE GradeID ='" + ddGrade.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmd1.Connection.Open();
        //ddSubGrp.SelectedValue = Convert.ToString(cmd1.ExecuteScalar());
        //cmd1.Connection.Close();

        //SqlCommand cmd = new SqlCommand("SELECT GroupID FROM ItemSubGroup WHERE CategoryID ='" + ddSubGrp.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmd.Connection.Open();
        //ddGroup.SelectedValue = Convert.ToString(cmd.ExecuteScalar());
        //cmd.Connection.Close();

    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Editing item changed!";
        EditMode();
    }

    private void CancelForm()
    {
        ClearControls(Form);
        GridView1.DataBind();
        DropDownList1.DataBind();
        GridView1.EditIndex = -1;
        EditField.Attributes.Add("class", "form-group hidden");
        btnSave.Text = "Save";

        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Action Cancelled!";
    }

    //Messagebox For Alerts    
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }


    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList1.DataBind();
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
            DropDownList1.SelectedValue = lblItemName.Text;
            EditMode();

            lblMsg.Attributes.Add("class", "xerp_info");
            lblMsg.Text = "Edit mode activated ...";
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }
    protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
    }
    protected void ddSubGrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
    }
    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            string isExist = RunQuery.SQLQuery.ReturnString("Select ItemName FROM Products WHERE CategoryID='" + lblItemCode.Text + "'");

            if (isExist == "")
            {
                SqlCommand cmd7 = new SqlCommand("DELETE Categories WHERE CategoryID=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                cmd7.ExecuteNonQuery();
                cmd7.Connection.Close();

                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "Entry deleted successfully ...";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: This category has existing products! Please delete: <b>" + isExist + "</b>";
            }

            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }
}
