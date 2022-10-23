using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class app_Warehouse_Areas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //txtDept.Text = "Drinks3";
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();

        if (!IsPostBack)
        {
            txtDept.Focus();
            EditField.Attributes.Add("class", "form-group hidden");

            ddWarehouse.DataBind();
            GridView1.DataBind();
        }

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

            SqlCommand cmd3 = new SqlCommand("SELECT AreaName FROM WareHouseAreas WHERE Warehouse ='" + ddWarehouse.SelectedValue + "' AND AreaName='" + txtDept.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd3.Connection.Open();
            string isExist = Convert.ToString(cmd3.ExecuteScalar());
            cmd3.Connection.Close();

            if (isExist == "")
            {
                SqlCommand cmd2 = new SqlCommand("INSERT INTO WareHouseAreas (Warehouse, AreaName, Description, ProjectID, EntryBy) VALUES (@GroupName, @CategoryName, @Description, @ProjectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.AddWithValue("@GroupName", ddWarehouse.SelectedValue);
                cmd2.Parameters.AddWithValue("@CategoryName", txtDept.Text);
                cmd2.Parameters.AddWithValue("@Description", txtDesc.Text);
                cmd2.Parameters.AddWithValue("@ProjectID", prjId);
                cmd2.Parameters.AddWithValue("@EntryBy", lName);

                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();

                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "New warehouse area added successfully";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Area Name Already Exist!";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in Save: " + ex.Message.ToString();
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
            if (btnSave.Text == "Save")
            {
                ExecuteInsert();
                EditField.Attributes.Add("class", "form-group hidden");
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
                lblMsg.Text = "Item Area Info Updated Successfully";
                MessageBox("Item Area Info Updated Successfully");
            }
        }
        else
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Please input Area Name";
        }
    }

    private void ExecuteUpdate()
    {
        //SqlCommand cmd2 = new SqlCommand("UPDATE Categories SET CategoryName='" + txtDept.Text + "', Description='" + txtDesc.Text + "', GroupName='" + ddWarehouse.SelectedValue + "' where (CategoryName ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmd2.Connection.Open();
        //cmd2.ExecuteNonQuery();
        //cmd2.Connection.Close();
        //cmd2.Connection.Dispose();

        //SqlCommand cmd = new SqlCommand("UPDATE Items SET CategoryName='" + txtDept.Text + "'  where (CategoryName ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
        SqlCommand cmd7 = new SqlCommand("SELECT [CategoryName], [Description] FROM [Categories] WHERE CategoryName='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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

    protected void ddWarehouse_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
}
