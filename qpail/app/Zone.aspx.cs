using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class app_Zone : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //txtDept.Text = "Drinks3";

        string formtype = base.Request.QueryString["type"];
        if(formtype=="sales")
        {
            ltrPageTitle.Text = "Sales Zone Setup";
            ddCategory.Enabled = false;
            Page.Title = "Sales Zones";
        }
        else if (formtype == "purchase")
        {
            ltrPageTitle.Text = "Purchase Zone Setup";
            Page.Title = "Purchase Zones";
        }
        else
        {
            btnSave.Visible = false;
        }
                
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
    private void ExecuteInsert()
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            string formtype = base.Request.QueryString["type"];
            int prjId = Convert.ToInt32(Session["projID"]);

            SqlCommand cmd2 = new SqlCommand("INSERT INTO Areas (AreaName, ServiceCharge, Country, AreaType, ProjectID, EntryBy) VALUES (@AreaName, @ServiceCharge, @Country, @AreaType, @ProjectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.AddWithValue("@Country", ddCategory.SelectedValue);
            cmd2.Parameters.AddWithValue("@AreaName", txtDept.Text);
            cmd2.Parameters.AddWithValue("@ServiceCharge", 0); //txtDesc.Text);
            cmd2.Parameters.AddWithValue("@AreaType", formtype);
            cmd2.Parameters.AddWithValue("@ProjectID", prjId);
            cmd2.Parameters.AddWithValue("@EntryBy", lName);

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();

            lblMsg.Attributes.Add("class", "xerp_success");
            lblMsg.Text = "New Zone Added Successfully";
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

                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Zone Info Updated Successfully";
            }
        }
        else
        {
            lblMsg.Text = "Please input Zone Name";
        }
    }

    private void ExecuteUpdate()
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE Areas SET AreaName='" + txtDept.Text + "', Country='" + ddCategory.SelectedValue + "' where (AreaName ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();

        //SqlCommand cmd = new SqlCommand("UPDATE Items SET CategoryName='" + txtDept.Text + "', GroupName='" + ddCategory.SelectedValue + "'  where (CategoryName ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
        SqlCommand cmd7 = new SqlCommand("SELECT [AreaName], [Country] FROM [Areas] WHERE AreaName='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
        //GridView1.DataBind();
        DropDownList1.DataBind();
        GridView1.EditIndex = -1;
        EditField.Attributes.Add("class", "form-group hidden");
        btnSave.Text = "Save";
        lblMsg.Text = "Action Cancelled!";
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        CancelForm();
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblMsg.Text="Pls Update info from left panel.";
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

    private void Bindgrid(string type)
    {
        SqlCommand cmd = new SqlCommand("SELECT [CategoryName], [Description] FROM [Categories] WHERE CategoryName='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        adapter.Fill(ds, "Products");

        GridView1.DataSource = ds;
        GridView1.DataBind();
    }
}
