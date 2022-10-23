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
    public partial class Teachers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (!IsPostBack)
            {
                txtTeacherID.Text = RunQuery.SQLQuery.ReturnInvNo("Teachers", "sl", "TeacherID");
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
                        
                        //Create Sql Command
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "Insert into Teachers (TeacherID, Name, DOB, Gender, address, phone, email, EntryBy) Values (@TeacherID, @Name, @DOB, @Gender, @address, @phone, @email, '" + Page.User.Identity.Name.ToString() + "')";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = cnn;
                        
                        cmd.Parameters.AddWithValue("@TeacherID", txtTeacherID.Text);
                        cmd.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(txtDOB.Text));
                        cmd.Parameters.AddWithValue("@Gender", ddGender.Text);
                        cmd.Parameters.AddWithValue("@address", txtaddress.Text);
                        cmd.Parameters.AddWithValue("@phone", txtphone.Text);
                        cmd.Parameters.AddWithValue("@email", txtemail.Text);
                        
                        /*** Command Execution ***/

                        if (txtName.Text != "" && txtDOB.Text != "")
                        {
                            cnn.Open();
                            int Success = cmd.ExecuteNonQuery();
                            cnn.Close();
                            if (Success > 0)
                            {
                                DropDownList1.DataBind();
                                updateData(RunQuery.SQLQuery.ReturnString("select max(sl) from Teachers"));

                                lblMsg.Attributes.Add("class", "xerp_success");
                                lblMsg.Text = "Successfully Saved Teachers Informations";
                                //lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff6230");
                                //ClearControls(Page);
                            }
                        }
                        else
                        {
                            lblMsg.Attributes.Add("class", "xerp_error");
                            lblMsg.Text = "Error : Date of birth or Name field is Missing !";
                            //lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff6230");
                        }
                    }
                    else
                    {
                        updateData(DropDownList1.SelectedValue);
                        ClearForm();
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Updated Successfully";
                    }

                }
                else
                {
                    //MessageBox("Empty data is not accepted.");
                }
                        GridView1.DataBind();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Unable to Save: " + ex.ToString();
            }
        }

        private void updateData(string sl)
        {            
            if (FileUpload1.HasFile)
            {
                string photoURL = RunQuery.SQLQuery.UploadImage(txtName.Text, FileUpload1, Server.MapPath(".\\Uploads\\Photos\\"), Server.MapPath("./Uploads/Photos/"), Page.User.Identity.Name.ToString(), "Teacher");
                RunQuery.SQLQuery.ExecNonQry("UPDATE Teachers SET Photo='" + photoURL + "' where (sl ='" + sl + "')");
            }

            SqlCommand cmd2 = new SqlCommand("UPDATE Teachers SET TeacherID=@TeacherID, Name=@Name, DOB=@DOB, Gender=@Gender, address=@address, phone=@phone, email=@email, education='"+txtEducation.Text+"', Designation='"+txtDesignation.Text+"' where (sl ='" + sl + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.AddWithValue("@TeacherID", txtTeacherID.Text);
            cmd2.Parameters.AddWithValue("@Name", txtName.Text);
            cmd2.Parameters.AddWithValue("@DOB", Convert.ToDateTime(txtDOB.Text));
            cmd2.Parameters.AddWithValue("@Gender", ddGender.Text);
            cmd2.Parameters.AddWithValue("@address", txtaddress.Text);
            cmd2.Parameters.AddWithValue("@phone", txtphone.Text);
            cmd2.Parameters.AddWithValue("@email", txtemail.Text);
            
            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
            cmd2.Connection.Dispose();
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
            SqlCommand cmd7 = new SqlCommand("SELECT sl, TeacherID, Name, DOB, Gender, address, phone, email, Photo, education, Designation FROM [Teachers] WHERE sl='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                EditField.Attributes.Remove("class");
                EditField.Attributes.Add("class", "form-group");
                btnSave.Text = "Update";

                txtTeacherID.Text = dr[1].ToString();
                txtName.Text = dr[2].ToString();
                txtDOB.Text = dr[3].ToString();
                ddGender.SelectedValue = dr[4].ToString();
                txtaddress.Text = dr[5].ToString();
                txtphone.Text = dr[6].ToString();
                txtemail.Text = dr[7].ToString();
                string photoId=dr[8].ToString();
                Image1.ImageUrl = RunQuery.SQLQuery.ReturnString("Select PhotoURL  from Photos where PhotoID='" + photoId + "'");
                txtEducation.Text = dr[9].ToString();
                txtDesignation.Text = dr[10].ToString();
            }
            cmd7.Connection.Close();
        }

        private void ClearForm()
        {
                btnSave.Text = "Save";
                EditField.Attributes.Add("class", "form-group hidden");

                txtTeacherID.Text = RunQuery.SQLQuery.ReturnInvNo("Teachers", "sl", "TeacherID");
                txtName.Text = "";
                txtDOB.Text = "";
                
                txtaddress.Text = "";
                txtphone.Text = "";
                txtemail.Text = "";
                string photoId ="";
                Image1.ImageUrl =string.Empty;
                txtEducation.Text = "";
                txtDesignation.Text = "";
                        
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

    }
}