using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class app_SisterConcernWarehouseSetup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //txtName.Text = "Drinks3";
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
            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            int prjId = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Connection.Close();

            SqlCommand cmd2 = new SqlCommand("INSERT INTO Warehouses (Zone, StoreName, Address, Owner, Description, ProjectID, IsSisterConcern, EntryBy) VALUES (@Zone, @GroupName, @Address, @Owner, @Description, @ProjectID, @IsSisterConcern, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.AddWithValue("@Zone", ddZone.SelectedValue);
            cmd2.Parameters.AddWithValue("@GroupName", txtName.Text);
            cmd2.Parameters.AddWithValue("@Address", txtAddress.Text);
            cmd2.Parameters.AddWithValue("@Owner", txtOwner.Text);
            cmd2.Parameters.AddWithValue("@Description", txtRemarks.Text);
            cmd2.Parameters.AddWithValue("@ProjectID", prjId);
            cmd2.Parameters.AddWithValue("@IsSisterConcern", '1');
            cmd2.Parameters.AddWithValue("@EntryBy", lName);

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();

            txtName.Focus();
            lblMsg.Attributes.Add("class", "xerp_success");
            lblMsg.Text = "New Warehouse Added Successfully";
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in Save: " + ex.ToString();
        }
        finally
        {
            GridView1.DataBind();
            txtName.Text = "";
            txtAddress.Text = "";
        }

    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        if (txtName.Text != "")
        {
            ExecuteInsert();
        }
        else
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Please input Store Name";
        }
    }
    protected void btnClear_Click1(object sender, EventArgs e)
    {
        txtName.Text = "";
        txtAddress.Text = "";
    }


    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        DropDownList1.SelectedValue = lblItemName.Text;

        //ltrFrmName.Text = "Edit Items";
        //EditMode();
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //EditMode();
    }

}
