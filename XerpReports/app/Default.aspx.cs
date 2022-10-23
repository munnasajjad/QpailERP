using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Oxford.app
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblPending.Text = RunQuery.SQLQuery.ReturnString("Select Count(tid) FROM Tasks where Status='Pending'") + " Left";
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");                
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
            if (Convert.ToDateTime(txtDate.Text) >= DateTime.Today)
            {
                string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
                RunQuery.SQLQuery.ExecNonQry("Insert into Tasks (TaskName, TaskDetails, DeadLine, Priority, Status, EntryBy) VALUES ('Admin', '" + txtDetail.Text + "', '" + dt + "', '" + ddType.SelectedValue + "', 'Pending', '" + Page.User.Identity.Name.ToString() + "')");
                GridView1.DataBind();
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Schedule Date Must Be Greater Then Today.";
            }
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: "+ex.Message.ToString();
            }
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label CrID = GridView1.Rows[index].FindControl("Label1") as Label;
                RunQuery.SQLQuery.ReturnString("Update Tasks set Status='Done' where tid= " + CrID.Text);
                GridView1.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.RowIndex);
                Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;
                string OrderID = RunQuery.SQLQuery.ReturnString("Select Status from Tasks where tid='" + lblItemCode.Text + "'");

                if (OrderID == "Pending")
                {
                    SqlCommand cmd7 = new SqlCommand("DELETE Tasks WHERE tid=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmd7.Connection.Open();
                    cmd7.ExecuteNonQuery();
                    cmd7.Connection.Close();

                    GridView1.DataBind();
                    //uPanel.Update();
                    lblMsg.Attributes.Add("class", "xerp_warning");
                    lblMsg.Text = "Task deleted ...";
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Pending Task is Locked for delete...";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.Message.ToString();
            }
        }

    }
}