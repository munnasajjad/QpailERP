using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_StockSubsidiary : System.Web.UI.Page
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
            if (txtDept.Text != "" && ddCategory.SelectedValue != "" && ddSubGrp.SelectedValue != "")
            {
                if (btnSave.Text == "Save")
                {
                    SqlCommand cmde = new SqlCommand("SELECT GradeName FROM ItemGrade WHERE CategoryID ='" + ddSubGrp.SelectedValue + "' AND  GradeName ='" + txtDept.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmde.Connection.Open();
                    string isExist

                        = Convert.ToString(cmde.ExecuteScalar());
                    cmde.Connection.Close();

                    if (isExist == "")
                    {
                        ExecuteInsert();
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "New Sub-group Added Successfully";
                    }
                    else
                    {
                        lblMsg.Attributes.Add("class", "xerp_error");
                        lblMsg.Text = "Error: Grade already exist!";
                    }
                }
                else
                {
                    SqlCommand cmde = new SqlCommand("SELECT GradeName FROM ItemGrade WHERE CategoryID ='" + ddSubGrp.SelectedValue + "' AND  GradeName ='" + txtDept.Text + "' AND  GradeID <>'" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmde.Connection.Open();
                    string isExist = Convert.ToString(cmde.ExecuteScalar());
                    cmde.Connection.Close();

                    if (isExist == "")
                    {
                        ExecuteUpdate();
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Item Sub-group Updated Successfully";
                    }
                    else
                    {
                        lblMsg.Attributes.Add("class", "xerp_error");
                        lblMsg.Text = "Error: Grade already exist!";
                    }

                }

                EditField.Attributes.Add("class", "form-group hidden");
                //ClearControls(Form);
                btnSave.Text = "Save";
                txtDept.Text = "";
                txtDesc.Text = "";
                txtDept.Focus();

            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Please input all mendatory fields...";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error: " + ex.Message.ToString();
        }
        finally
        {
            GridView1.DataBind();
            DropDownList1.DataBind();

        }

    }
    private void ExecuteInsert()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        int prjId = Convert.ToInt32(cmd.ExecuteScalar());
        cmd.Connection.Close();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO ItemGrade (CategoryID, GradeName, Description, ProjectID, EntryBy) VALUES (@GroupID, @GradeName, @Description, @ProjectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@GroupID", ddSubGrp.SelectedValue);
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
        SqlCommand cmd2 = new SqlCommand("UPDATE ItemGrade SET GradeName='" + txtDept.Text + "', Description='" + txtDesc.Text + "', CategoryID='" + ddSubGrp.SelectedValue + "' where (GradeID ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();

        //SqlCommand cmd = new SqlCommand("UPDATE Items SET ItemType='" + txtDept.Text + "', GroupID='" + ddCategory.SelectedValue + "'  where (ItemType ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmd.Connection.Open();
        //cmd.ExecuteNonQuery();
        //cmd.Connection.Close();
        //cmd.Connection.Dispose();
    }


    protected void btnClear_Click1(object sender, EventArgs e)
    {
        CancelForm();
    }


    private void EditMode()
    {
        SqlCommand cmd = new SqlCommand("SELECT GroupID FROM ItemSubGroup WHERE CategoryID ='" + ddSubGrp.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        ddCategory.SelectedValue = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();

        ddSubGrp.DataBind();
        SqlCommand cmd7 = new SqlCommand("SELECT [GradeName], [Description], CategoryID FROM [ItemGrade] WHERE GradeID='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            txtDept.Text = dr[0].ToString();
            txtDesc.Text = dr[1].ToString();
            ddSubGrp.SelectedValue = dr[2].ToString();
        }
        cmd7.Connection.Close();
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
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
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddSubGrp.DataBind();
        txtDept.Focus();

    }
    protected void ddSubGrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
        txtDept.Focus();

    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            string isExist = RunQuery.SQLQuery.ReturnString("Select CategoryName FROM Categories WHERE GradeID='" + lblItemCode.Text + "'");

            if (isExist == "")
            {
                SqlCommand cmd7 = new SqlCommand("DELETE ItemGrade WHERE GradeID=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                cmd7.ExecuteNonQuery();
                cmd7.Connection.Close();

                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "Entry deleted successfully ...";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: This grade has existing category! Please delete <b>" + isExist + "</b>";
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