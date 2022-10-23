using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Oxford.app
{
    public partial class SectionSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }


        //Code for Clearing the form
        //public static void ClearControls(Control Parent)
        //{
        //    if (Parent is TextBox)
        //    { (Parent as TextBox).Text = string.Empty; }
        //    else
        //    {
        //        foreach (Control c in Parent.Controls)
        //            ClearControls(c);
        //    }
        //}


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string isExist = RunQuery.SQLQuery.ReturnString("Select name from Section where name='" + txtName.Text + "' AND Class='" + ddClass.SelectedValue + "' ");
                if (txtName.Text != "" && isExist=="")
                {
                 
                    if (btnSave.Text == "Save")
                    {
                        SqlConnection cnn = new SqlConnection();
                        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

                        //string lName = txtTeacherID.Text;
                        //lName = lName.Trim();
                        //Create Sql Command
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "Insert into Section (name, Class) Values (@name, @Class)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = cnn;
                        
                        cmd.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Class", ddClass.SelectedValue);
                        
                        if (txtName.Text != "")
                        {
                            cnn.Open();
                            int Success = cmd.ExecuteNonQuery();
                            cnn.Close();
                            if (Success > 0)
                            {
                                lblMsg.Text = "Successfully Updated!";
                                lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff6230");
                                //ClearControls(Page);
                            }
                        }
                        else
                        {
                            lblMsg.Text = "Error : Name field is Missing !";
                            lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff6230");
                        }
                    }
                    else
                    {
                        SqlCommand cmd2 = new SqlCommand("UPDATE Section SET Name=@Name, Class=@Class where (sl ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                        
                        cmd2.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd2.Parameters.AddWithValue("@Class", ddClass.SelectedValue);                        
                        
                        cmd2.Connection.Open();
                        cmd2.ExecuteNonQuery();
                        cmd2.Connection.Close();
                        cmd2.Connection.Dispose();

                        btnSave.Text = "Save";
                        EditField.Attributes.Add("class", "form-group hidden");
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Updated Successfully";
                    }

                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Data already exist!";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Unable to Save: " + ex.ToString();
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
            SqlCommand cmd7 = new SqlCommand("SELECT sl, name, Class FROM [Section] WHERE sl='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                EditField.Attributes.Remove("class");
                EditField.Attributes.Add("class", "form-group");
                btnSave.Text = "Update";

                //txtDept.Text = dr[0].ToString();
                //txtDesc.Text = dr[1].ToString();
                //ddSubGrp.SelectedValue = dr[2].ToString();


                txtName.Text = dr[1].ToString();
                ddClass.SelectedValue = dr[2].ToString();
            }
            cmd7.Connection.Close();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label lblsl = (Label)GridView1.Rows[e.RowIndex].FindControl("Label1");
            string sl = lblsl.Text, msg = "";

            string isExist = RunQuery.SQLQuery.ReturnString("Select Class from Students where Class='" + sl + "' ");
            if (isExist == "")
            {
                isExist = RunQuery.SQLQuery.ReturnString("Select Class from Section where Class='" + sl + "' ");
                if (isExist == "")
                {
                    RunQuery.SQLQuery.ExecNonQry("DELETE FROM Class WHERE (sl = '" + sl + "')");
                    GridView1.DataBind();
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Class deleted successfully.";
                }
                else
                {
                    msg = "This class already has sections!";
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "ERROR: " + msg;
                    e.Cancel = true;
                }
            }
            else
            {
                msg = "This class already has students!";
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + msg;
                e.Cancel = true;
            }
        }
    }
}