using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security;
using System.IO;

namespace Oxford.app
{
    public partial class Coll_Group : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClearForm();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text != "")
                {
                    if (btnSave.Text == "Save")
                    {
                        SqlConnection cnn = new SqlConnection();
                        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

                        //string lName = txtTeacherID.Text;
                        //lName = lName.Trim();
                        //Create Sql Command
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "Insert into ExpenseTypes (Name, Remark, ReRun, EntryBy) Values (@Name, @Remark, @ReRun, @EntryBy)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = cnn;

                        cmd.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Remark", txtDescription.Text);
                        cmd.Parameters.AddWithValue("@ReRun", ddType.SelectedValue);
                        cmd.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

                            cnn.Open();
                            int Success = cmd.ExecuteNonQuery();
                            cnn.Close();
                            if (Success > 0)
                            {
                                ClearForm();
                                lblMsg.Attributes.Add("class", "xerp_success");
                                lblMsg.Text = "Successfully Saved!";
                            }
                        
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE ExpenseTypes SET Name=@Name, Remark=@Remark, ReRun=@ReRun where (sl ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                        cmd.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Remark", txtDescription.Text);
                        cmd.Parameters.AddWithValue("@ReRun", ddType.SelectedValue);
                        
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                        cmd.Connection.Dispose();
                        
                        ClearForm();

                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Updated Successfully";
                    }
                }
                else
                {
                    //MessageBox("Empty data is not accepted.");
                }
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Unable to Save: " + ex.Message.ToString();
            }
            finally
            {
                GridView1.DataBind();
            }
        }



        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditMode();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList1.DataBind();
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label PID = GridView1.Rows[index].FindControl("Label1") as Label;
                DropDownList1.SelectedValue = PID.Text;
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

        private void EditMode()
        {
            SqlCommand cmd7 = new SqlCommand("SELECT name, ReRun FROM [ExpenseTypes] WHERE sl='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                EditField.Attributes.Remove("class");
                EditField.Attributes.Add("class", "form-group");
                btnSave.Text = "Update";

                txtName.Text = dr[0].ToString();
                ddType.SelectedValue = dr[1].ToString();
                //ddSubGrp.SelectedValue = dr[2].ToString();


                //ddMonth.SelectedValue = dr[1].ToString();
                //ddMonth.SelectedValue = dr[4].ToString();
            }
            cmd7.Connection.Close();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtDescription.Text = "";
            btnSave.Text = "Save";
            EditField.Attributes.Add("class", "form-group hidden");
            GridView1.DataBind();
            txtName.Focus();
        }
    }
}