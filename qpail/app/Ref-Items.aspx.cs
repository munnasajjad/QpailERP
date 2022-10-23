using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_Ref_Items : System.Web.UI.Page
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
    protected void btnSave_Click1(object sender, EventArgs e)
    {
        try
        {
            if (txtDept.Text != "")
            {
                if (btnSave.Text == "Save")
                {

                    SqlCommand cmde = new SqlCommand("SELECT BrandName FROM RefItems WHERE  BrandName ='" + txtDept.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmde.Connection.Open();
                    string isExist = Convert.ToString(cmde.ExecuteScalar());
                    cmde.Connection.Close();

                    if (isExist == "")
                    {
                        ExecuteInsert();
                        EditField.Attributes.Add("class", "form-group hidden");
                        txtDept.Focus();

                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "New Category Added Successfully";
                    }
                    else
                    {
                        lblMsg.Attributes.Add("class", "xerp_error");
                        lblMsg.Text = "Error: Category name already exist!";
                    }

                }
                else
                {
                    ExecuteUpdate();

                    txtDept.Text = "";
                    txtDesc.Text = "";
                    btnSave.Text = "Save";
                    EditField.Attributes.Add("class", "form-group hidden");

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Item Category Info Updated Successfully";
                    //MessageBox("Item Category Info Updated Successfully");
                }

                GridView1.DataBind();
                DropDownList1.DataBind();

            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Please input Item Category Name";
            }

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error in Save: " + ex.Message.ToString();
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
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        int prjId = Convert.ToInt32(cmd.ExecuteScalar());
        cmd.Connection.Close();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO RefItems (CustomerID, BrandName, Description, ProjectID, EntryBy) VALUES (@CustomerID, @BrandName, @Description, @ProjectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Parameters.AddWithValue("@CustomerID", "");
        cmd2.Parameters.AddWithValue("@BrandName", txtDept.Text);
        cmd2.Parameters.AddWithValue("@Description", txtDesc.Text);
        cmd2.Parameters.AddWithValue("@ProjectID", prjId);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE RefItems SET BrandName='" + txtDept.Text + "', Description='" + txtDesc.Text + "', CustomerID='' where (BrandID ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();

        //SqlCommand cmd = new SqlCommand("UPDATE Items SET BrandName='" + txtDept.Text + "', CustomerID='" + ddCategory.SelectedValue + "'  where (BrandName ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmd.Connection.Open();
        //cmd.ExecuteNonQuery();
        //cmd.Connection.Close();
        //cmd.Connection.Dispose();
    }


    protected void btnClear_Click1(object sender, EventArgs e)
    {
        CancelForm();
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        int index = Convert.ToInt32(e.NewEditIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        DropDownList1.SelectedValue = lblItemName.Text;

        //GridView1.Columns[-1].Visible = false;

        EditMode();
    }


    private void EditMode()
    {
        SqlCommand cmd7 = new SqlCommand("SELECT CustomerID, [BrandName], [Description] FROM [RefItems] WHERE BrandID='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            //ddCategory.SelectedValue = dr[0].ToString();
            EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            txtDept.Text = dr[1].ToString();
            txtDesc.Text = dr[2].ToString();
        }
        cmd7.Connection.Close();
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        EditMode();
    }

    private void CancelForm()
    {
        txtDept.Text = "";
        txtDesc.Text = "";
        GridView1.DataBind();
        DropDownList1.DataBind();
        GridView1.EditIndex = -1;
        EditField.Attributes.Add("class", "form-group hidden");
        btnSave.Text = "Save";
        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Action Cancelled!";
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        CancelForm();
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblMsg.Attributes.Add("class", "xerp_error");
        MessageBox("Pls Update info from left panel.");
        txtDept.Focus();
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
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        DropDownList1.SelectedValue = lblItemName.Text;
        EditMode();
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
}
