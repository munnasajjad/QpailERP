using RunQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Atest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            BindTeamGridView();
        }
           
        //SQLQuery.PopulateDropDown("SELECT [sl], [DepartmentName] FROM [Departments]", ddHouseNo, "sl", "DepartmentName");
    }



    protected void btnInsert_Click(object sender, EventArgs e)
    {
        if (btnInsert.Text == "Save")
        {
            if (txtAddress.Text != "")
            {
                SQLQuery.ExecNonQry(@"INSERT INTO [dbo].[TeamTest]
                       ([Name] ,[Address] ,[Date],[EntryBy])
                        VALUES
                       ('" + ddName.SelectedValue + "', '" + txtAddress.Text + "','" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "','" + User.Identity.Name + "') ");
                BindTeamGridView();
            }
            else
            {
                lblMsg.Text = "You should write the Address, my friend!";
            }
               
        }
        else
        {
            SQLQuery.ExecNonQry(@"UPDATE [dbo].[TeamTest]
                    SET [Date] = '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "',[Name] = '" + ddName.SelectedValue + "'      ,[Address] = '" + txtAddress.Text + "'  where id='"+ lableIdHField.Value + "'");
            BindTeamGridView();
            lblMsg.Attributes.Add("class", "xerp_success");
            lblMsg.Text = "Hooray again!!!....";
        }
            
    }

    private void BindTeamGridView()
    {
        GridView1.DataSource = SQLQuery.ReturnDataTable(@"SELECT id, Name, Address, Date, EntryBy FROM TeamTest");
       
        GridView1.DataBind();
    }

    
    




    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label labelID = GridView1.Rows[index].FindControl("Label1") as Label;
        lableIdHField.Value = labelID.Text;
        DataTable dt = SQLQuery.ReturnDataTable("SELECT  id, Name, Address, Date,EntryBy FROM TeamTest where Id='" + labelID.Text + "'");
        foreach (DataRow dtx in dt.Rows)
        {
            txtDate.Text = Convert.ToDateTime(dtx["Date"].ToString()).ToString("dd/MM/yyyy");
            ddName.SelectedValue = dtx["Name"].ToString();
            txtAddress.Text = dtx["Address"].ToString();
        }
        btnInsert.Text = "Update";
        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Edit mode activeted....";
    }

    protected void GridView1_RowDeleting1(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;

        SQLQuery.ExecNonQry("DELETE FROM TeamTest WHERE (Id ='" + lblId.Text + "')");
        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "TeamTest data Circle deleted successfully";
        BindTeamGridView();
    }
}