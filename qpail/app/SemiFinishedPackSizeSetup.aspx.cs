using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class app_SemiFinishedProductSetup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //txtPackSize.Text = "Drinks3";
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();

        if (!IsPostBack)
        {
            EditField.Attributes.Add("class", "form-group hidden");
            //divSerial.Attributes.Add("class", "form-group hidden");
        }

    }
    private void ExecuteInsert()
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            int projectId = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Connection.Close();

            //int displaySl = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select ISNULL(Max(DisplaySl),0) from Brands")) + 10;

            SqlCommand cmd2 = new SqlCommand("INSERT INTO SemiFinishedPackSize (Size, Description, ProjectId, EntryBy, EntryDate) VALUES (@Size, @Description, @ProjectId, @EntryBy, @EntryDate)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.AddWithValue("@Size", txtPackSize.Text);
            cmd2.Parameters.AddWithValue("@Description", txtDescription.Text);
            cmd2.Parameters.AddWithValue("@ProjectID", projectId);
            cmd2.Parameters.AddWithValue("@EntryBy", lName);
            cmd2.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")));

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();

            lblMsg.Attributes.Add("class", "xerp_success");
            lblMsg.Text = "New Item Added Successfully";
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error in Save: " + ex.Message.ToString();
        }
        finally
        {
            GridView1.DataBind();
            txtPackSize.Text = "";
            txtDescription.Text = "";
        }

    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        if (txtPackSize.Text != "")
        {
            if (btnSave.Text == "Save")
            {
                ExecuteInsert();
                EditField.Attributes.Add("class", "form-group hidden");
                //divSerial.Attributes.Add("class", "form-group hidden");
                txtPackSize.Focus();
            }
            else
            {
                ExecuteUpdate();

                txtPackSize.Text = "";
                txtDescription.Text = "";
                btnSave.Text = "Save";
                EditField.Attributes.Add("class", "form-group hidden");
                //divSerial.Attributes.Add("class", "form-group hidden");
                GridView1.DataBind();
                ddSemiFinishedPackSize.DataBind();
                pnl.Update();

                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Item Info Updated Successfully";
                MessageBox("Item Info Updated Successfully");
            }
        }
        else
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Please input Item Name";
        }
    }

    private void ExecuteUpdate()
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE SemiFinishedPackSize SET Size='" + txtPackSize.Text + "', Description='" + txtDescription.Text + "' WHERE (Id ='" + ddSemiFinishedPackSize.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
        /*
        SqlCommand cmd = new SqlCommand("UPDATE Items SET CategoryName='" + txtPackSize.Text + "', GroupName='" + ddCategory.SelectedValue + "'  where (CategoryName ='" + ddSemiFinishedPackSize.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();
         * */
    }


    protected void btnClear_Click1(object sender, EventArgs e)
    {
        CancelForm();
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        int index = Convert.ToInt32(e.NewEditIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        ddSemiFinishedPackSize.SelectedValue = lblItemName.Text;

        //GridView1.Columns[-1].Visible = false;

        EditMode();
    }

    private void EditMode()
    {
        SqlCommand cmd7 = new SqlCommand("SELECT [Size], [Description] FROM [SemiFinishedPackSize] WHERE Id='"+ddSemiFinishedPackSize.SelectedValue+"'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            EditField.Attributes.Add("class", "form-group");
            //divSerial.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            txtPackSize.Text = dr[0].ToString();
            txtDescription.Text = dr[1].ToString();
            //txtSerial.Text = dr[2].ToString();
        }
        cmd7.Connection.Close();
    }


    protected void ddSemiFinishedPackSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        EditMode();
    }

    private void CancelForm()
    {
        txtPackSize.Text = "";
        txtDescription.Text = "";
        GridView1.DataBind();
        ddSemiFinishedPackSize.DataBind();
        GridView1.EditIndex = -1;
        EditField.Attributes.Add("class", "form-group hidden");
        //divSerial.Attributes.Add("class", "form-group hidden");
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
        txtPackSize.Focus();
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
        Label idLabel = GridView1.Rows[index].FindControl("idLabel") as Label;
        ddSemiFinishedPackSize.SelectedValue = idLabel.Text;
        EditMode();
    }
}
