using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class app_Packaging_Consumptions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();

        if (!IsPostBack)
        {
            ddFinished.DataBind();
            //lrlSavedBox.Text = "Ingradiants for " + ddFinished.SelectedItem.Text; 

            ddRawItem.DataBind();
            ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType from Products where ProductID='" + ddRawItem.SelectedValue + "'");
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

            SqlCommand cmd2 = new SqlCommand("INSERT INTO Ingradiants (ItemCode, ItemName, RawItemCode, RawItemName, RequiredQuantity, Wastage, UnitType, ProjectCode, EntryBy) VALUES (@ItemCode, @ItemName, @RawItemCode, @RawItemName, @RequiredQuantity, @Wastage, @UnitType, @ProjectCode, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.AddWithValue("@ItemCode", ddFinished.SelectedValue);
            cmd2.Parameters.AddWithValue("@ItemName", ddFinished.SelectedItem.Text);
            cmd2.Parameters.AddWithValue("@RawItemCode", ddRawItem.SelectedValue);
            cmd2.Parameters.AddWithValue("@RawItemName", ddRawItem.SelectedItem.Text);
            cmd2.Parameters.AddWithValue("@RequiredQuantity", txtDept.Text);
            cmd2.Parameters.AddWithValue("@Wastage", txtWastage.Text);
            cmd2.Parameters.AddWithValue("@UnitType", ltrUnit.Text);
            cmd2.Parameters.AddWithValue("@ProjectCode", prjId);
            cmd2.Parameters.AddWithValue("@EntryBy", lName);

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
            
            lblMsg.Attributes.Add("class", "xerp_success");
            lblMsg.Text = "New Consumption Item Added Successfully";
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error: " + ex.Message.ToString();
        }
        finally
        {
            GridView1.DataBind();
            txtDept.Text = "";
            //txtDesc.Text = "";
        }

    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        if (txtDept.Text != "")
        {
            ExecuteInsert();
            GridView1.DataBind();
        }
        else
        {
            lblMsg.Text = "Please input Item Category Name";
        }
    }
    protected void btnClear_Click1(object sender, EventArgs e)
    {
        txtDept.Text = "";
        //txtDesc.Text = "";
    }
    protected void ddRawItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType from Products where ProductID='" + ddRawItem.SelectedValue + "'");        
    }
    protected void ddFinished_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        string itemName = lblItemName.Text;

        RunQuery.SQLQuery.ExecNonQry("Delete Ingradiants where IID='"+itemName+"'");
        GridView1.DataBind();
    }
}
