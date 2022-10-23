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
    public partial class Money_Collection : System.Web.UI.Page
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
                        cmd.CommandText = "Insert into Class (name, NameNumeric, TutionFee, ClassTeacher) Values (@name, @NameNumeric, @TutionFee, @ClassTeacher)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = cnn;


                        cmd.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd.Parameters.AddWithValue("@NameNumeric", txtNameNumeric.Text);
                        cmd.Parameters.AddWithValue("@TutionFee", txtTutionFee.Text);
                        cmd.Parameters.AddWithValue("@ClassTeacher", ddClassTeacher.SelectedValue);



                        /*** Command Execution ***/

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
                        SqlCommand cmd2 = new SqlCommand("UPDATE Class SET Name=@Name, NameNumeric=@NameNumeric, TutionFee=@TutionFee, ClassTeacher=@ClassTeacher where (sl ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));


                        cmd2.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd2.Parameters.AddWithValue("@NameNumeric", txtNameNumeric.Text);
                        cmd2.Parameters.AddWithValue("@TutionFee", txtTutionFee.Text);
                        cmd2.Parameters.AddWithValue("@ClassTeacher", ddClassTeacher.SelectedValue);


                        cmd2.Connection.Open();
                        cmd2.ExecuteNonQuery();
                        cmd2.Connection.Close();
                        cmd2.Connection.Dispose();

                        GridView1.DataBind();
                        //ClearControls(Form);
                        btnSave.Text = "Save";
                        EditField.Attributes.Add("class", "form-group hidden");

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
                lblMsg.Text = "Unable to Save: " + ex.ToString();
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
            SqlCommand cmd7 = new SqlCommand("SELECT sl, name, NameNumeric, TutionFee, ClassTeacher FROM [Class] WHERE sl='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
                txtNameNumeric.Text = dr[2].ToString();
                txtTutionFee.Text = dr[3].ToString();
                ddClassTeacher.SelectedValue = dr[4].ToString();
            }
            cmd7.Connection.Close();
        }
    }
}