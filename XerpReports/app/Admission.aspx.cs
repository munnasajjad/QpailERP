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
    public partial class Admission : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            if (!IsPostBack)
            {
                txtSession.Text = DateTime.Now.Year.ToString();
                txtDOB.Text = DateTime.Now.AddYears(-5).ToString("dd/MM/yyyy");
                txtApplicationDate.Text = DateTime.Now.AddYears(-0).ToString("dd/MM/yyyy");
                ddClass.DataBind();
                lblStudentID.Text = GenerateStudentID();
                GridView1.DataBind();

                Image1.Attributes.Add("Style","display:none;");

                string qstring = Request.QueryString["Id"];
                if (qstring != null)
                {
                    DropDownList1.DataBind();
                    DropDownList1.SelectedValue = qstring;
                    EditMode();

                    lblMsg.Attributes.Add("class", "xerp_info");
                    lblMsg.Text = "Edit mode activated ...";
                }

            }
        }


        //Code for Clearing the form
        public void ClearControls()
        {            
            txtStudentNameB.Text = "";
            txtStudentNameE.Text = "";
            
            txtFatherNameB.Text = "";
            txtFatherNameE.Text = "";
            txtFatherOccupation.Text = "";
            txtFatherIncome.Text = "";
            txtMotherNameB.Text = "";
            txtMotherNameE.Text = "";
            txtMotherOccupation.Text = "";
            txtMotherIncome.Text = "";
            txtPresentAddress.Text = "";
            txtPhoneOffice.Text = "";
            txtPhoneHouse.Text = "";
            txtPhoneMobile.Text = "";
            txtPermanentAddress.Text = "";
            txtGuardianName.Text = "";
            txtGuardianRelation.Text = "";
            txtGuardianAddress.Text = "";
            txtGuardianOccupation.Text = "";
            //txtDOB.Text = "";
            //txtStudentAge.Text = "";
            txtExSchool.Text = "";
            //ddReligion.SelectedValue = "";
            txtNationality.Text = "";
            txtVaccineInfo.Text = "";
            //txtApplicationDate.Text = "";
            txtBloodGroup.Text = "";
            txtHobby.Text = "";            
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string studentID = RunQuery.SQLQuery.ReturnString("Select StudentID from Students where sl='" + DropDownList1.SelectedValue + "'");

                if (txtStudentNameE.Text != "" && txtFatherNameE.Text != "" && txtPhoneMobile.Text != "")
                {
                    if (btnSave.Text == "Save")
                    {
                        studentID = GenerateStudentID();
                        SqlConnection cnn = new SqlConnection();
                        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

                        SqlCommand cmd = new SqlCommand();
                        //cmd.CommandText = "Insert into Students (StudentID, StudentNameB, StudentNameE, Class, FatherNameB, FatherNameE, FatherOccupation, FatherIncome, MotherNameB, MotherNameE, MotherOccupation, MotherIncome, PresentAddress, PhoneOffice, PhoneHouse, PhoneMobile, PermanentAddress, GuardianName, GuardianRelation, GuardianAddress, GuardianOccupation, DOB, StudentAge, ExSchool, Religion, Nationality, VaccineInfo, ApplicationDate, BloodGroup, Hobby, StudentPhoto) Values (@StudentID, @StudentNameB, @StudentNameE, @Class, @FatherNameB, @FatherNameE, @FatherOccupation, @FatherIncome, @MotherNameB, @MotherNameE, @MotherOccupation, @MotherIncome, @PresentAddress, @PhoneOffice, @PhoneHouse, @PhoneMobile, @PermanentAddress, @GuardianName, @GuardianRelation, @GuardianAddress, @GuardianOccupation, @DOB, @StudentAge, @ExSchool, @Religion, @Nationality, @VaccineInfo, @ApplicationDate, @BloodGroup, @Hobby, @StudentPhoto)";
                        cmd.CommandText = "Insert into Students (StudentID, EntryBy) Values (@StudentID, @EntryBy)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = cnn;
                        cmd.Parameters.AddWithValue("@StudentID", studentID);
                        cmd.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

                        cnn.Open();
                        int Success = cmd.ExecuteNonQuery();
                        cnn.Close();
                        if (Success > 0)
                        {
                            lblMsg.Attributes.Add("class", "xerp_success");
                            lblMsg.Text = "Successfully Saved Students Informations";
                            lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff6230");
                            //ClearControls(Page);
                        }

                    }
                    else
                    {
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Updated Successfully";
                    }

                    //DropDownList1.DataBind();
                    //DropDownList1.SelectedValue = RunQuery.SQLQuery.ReturnString("Select sl from Students where StudentID='" + studentID + "'");
                    txtStudentAge.Text = RunQuery.SQLQuery.ToAgeString(Convert.ToDateTime(txtDOB.Text));

                    SqlCommand cmd2 = new SqlCommand("UPDATE Students SET  StudentNameB=@StudentNameB, StudentNameE=@StudentNameE, Class=@Class, FatherNameB=@FatherNameB, FatherNameE=@FatherNameE, FatherOccupation=@FatherOccupation, FatherIncome=@FatherIncome, MotherNameB=@MotherNameB, MotherNameE=@MotherNameE, MotherOccupation=@MotherOccupation, MotherIncome=@MotherIncome, PresentAddress=@PresentAddress, PhoneOffice=@PhoneOffice, PhoneHouse=@PhoneHouse, PhoneMobile=@PhoneMobile, PermanentAddress=@PermanentAddress, GuardianName=@GuardianName, GuardianRelation=@GuardianRelation, GuardianAddress=@GuardianAddress, GuardianOccupation=@GuardianOccupation, DOB=@DOB, StudentAge=@StudentAge, ExSchool=@ExSchool, Religion=@Religion, Nationality=@Nationality, VaccineInfo=@VaccineInfo, ApplicationDate=@ApplicationDate, BloodGroup=@BloodGroup, Hobby=@Hobby, Gender='" + ddGender.SelectedValue + "', Session='"+txtSession.Text+"' where (StudentID ='" + studentID + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                    cmd2.Parameters.AddWithValue("@StudentNameB", txtStudentNameB.Text);
                    cmd2.Parameters.AddWithValue("@StudentNameE", txtStudentNameE.Text);
                    cmd2.Parameters.AddWithValue("@Class", ddClass.Text);
                    cmd2.Parameters.AddWithValue("@FatherNameB", txtFatherNameB.Text);
                    cmd2.Parameters.AddWithValue("@FatherNameE", txtFatherNameE.Text);
                    cmd2.Parameters.AddWithValue("@FatherOccupation", txtFatherOccupation.Text);
                    cmd2.Parameters.AddWithValue("@FatherIncome", txtFatherIncome.Text);
                    cmd2.Parameters.AddWithValue("@MotherNameB", txtMotherNameB.Text);
                    cmd2.Parameters.AddWithValue("@MotherNameE", txtMotherNameE.Text);
                    cmd2.Parameters.AddWithValue("@MotherOccupation", txtMotherOccupation.Text);
                    cmd2.Parameters.AddWithValue("@MotherIncome", txtMotherIncome.Text);
                    cmd2.Parameters.AddWithValue("@PresentAddress", txtPresentAddress.Text);
                    cmd2.Parameters.AddWithValue("@PhoneOffice", txtPhoneOffice.Text);
                    cmd2.Parameters.AddWithValue("@PhoneHouse", txtPhoneHouse.Text);
                    cmd2.Parameters.AddWithValue("@PhoneMobile", txtPhoneMobile.Text);
                    cmd2.Parameters.AddWithValue("@PermanentAddress", txtPermanentAddress.Text);
                    cmd2.Parameters.AddWithValue("@GuardianName", txtGuardianName.Text);
                    cmd2.Parameters.AddWithValue("@GuardianRelation", txtGuardianRelation.Text);
                    cmd2.Parameters.AddWithValue("@GuardianAddress", txtGuardianAddress.Text);
                    cmd2.Parameters.AddWithValue("@GuardianOccupation", txtGuardianOccupation.Text);
                    cmd2.Parameters.AddWithValue("@DOB", txtDOB.Text);
                    cmd2.Parameters.AddWithValue("@StudentAge", txtStudentAge.Text);
                    cmd2.Parameters.AddWithValue("@ExSchool", txtExSchool.Text);
                    cmd2.Parameters.AddWithValue("@Religion", ddReligion.SelectedValue);
                    cmd2.Parameters.AddWithValue("@Nationality", txtNationality.Text);
                    cmd2.Parameters.AddWithValue("@VaccineInfo", txtVaccineInfo.Text);
                    cmd2.Parameters.AddWithValue("@ApplicationDate", Convert.ToDateTime(txtApplicationDate.Text));
                    cmd2.Parameters.AddWithValue("@BloodGroup", txtBloodGroup.Text);
                    cmd2.Parameters.AddWithValue("@Hobby", txtHobby.Text);

                    cmd2.Connection.Open();
                    cmd2.ExecuteNonQuery();
                    cmd2.Connection.Close();
                    cmd2.Connection.Dispose();


                    if (FileUpload1.HasFile)
                    {
                        string sPhoto = "StudentsPhoto/" + studentID + "." + UploadImage(studentID);
                        cmd2 = new SqlCommand("UPDATE Students SET StudentPhoto=@StudentPhoto where (StudentID ='" + studentID + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                        cmd2.Parameters.AddWithValue("@StudentPhoto", sPhoto);
                        cmd2.Connection.Open();
                        cmd2.ExecuteNonQuery();
                        cmd2.Connection.Close();
                        cmd2.Connection.Dispose();
                    }
                    if (FileUpload2.HasFile)
                    {
                        string sPhoto = "StudentsPhoto/" + studentID + "-ss." + UploadImage(studentID);
                        cmd2 = new SqlCommand("UPDATE Students SET StudentSignature=@StudentPhoto where (StudentID ='" + studentID + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                        cmd2.Parameters.AddWithValue("@StudentPhoto", sPhoto);
                        cmd2.Connection.Open();
                        cmd2.ExecuteNonQuery();
                        cmd2.Connection.Close();
                        cmd2.Connection.Dispose();
                    }
                    if (FileUpload3.HasFile)
                    {
                        string sPhoto = "StudentsPhoto/" + studentID + "-sg." + UploadImage(studentID);
                        cmd2 = new SqlCommand("UPDATE Students SET GuardianSignature=@StudentPhoto where (StudentID ='" + studentID + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                        cmd2.Parameters.AddWithValue("@StudentPhoto", sPhoto);
                        cmd2.Connection.Open();
                        cmd2.ExecuteNonQuery();
                        cmd2.Connection.Close();
                        cmd2.Connection.Dispose();
                    }

                    ClearControls();

                    GridView1.DataBind();
                    btnSave.Text = "Save";
                    EditField.Attributes.Remove("class");
                    EditField.Attributes.Add("class", "form-group hidden");

                    Image1.ImageUrl = "";
                    Image1.Attributes.Add("Style", "display:none;");
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Error : Please check the missing fields!";
                    lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff6230");
                }
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = ex.Message.ToString();
                lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff6230");
            }
        }

        private string UploadImage(string StudentID)
        {
            string tExt = Path.GetFileName(FileUpload1.PostedFile.ContentType);
            string fileName = StudentID + "." + tExt;

            //Delete the existing file first
            //RunQuery.SQLQuery.ExecNonQry("Delete PO_Images where PONo=''");

            string strFolder = Server.MapPath(".\\StudentsPhoto\\");
            string strFullPath = strFolder + fileName;

            if (File.Exists(strFullPath))
            {
                File.Delete(strFullPath);
            }
            var file = FileUpload1.PostedFile.InputStream;
            System.Drawing.Image img = System.Drawing.Image.FromStream(file, false, false);
            img.Save(Server.MapPath("./StudentsPhoto/" + fileName));
            return tExt;
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
            SqlCommand cmd7 = new SqlCommand("SELECT StudentID, StudentNameB, StudentNameE, Class, FatherNameB, FatherNameE, FatherOccupation, FatherIncome, MotherNameB, MotherNameE, MotherOccupation, MotherIncome, PresentAddress, PhoneOffice, PhoneHouse, PhoneMobile, PermanentAddress, GuardianName, GuardianRelation, GuardianAddress, GuardianOccupation, DOB, StudentAge, ExSchool, Religion, Nationality, VaccineInfo, ApplicationDate, BloodGroup, Hobby, StudentPhoto, Gender FROM [Students] WHERE sl='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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


                lblStudentID.Text = dr[0].ToString();
                txtStudentNameB.Text = dr[1].ToString();
                txtStudentNameE.Text = dr[2].ToString();
                ddClass.SelectedValue = dr[3].ToString();
                txtFatherNameB.Text = dr[4].ToString();
                txtFatherNameE.Text = dr[5].ToString();
                txtFatherOccupation.Text = dr[6].ToString();
                txtFatherIncome.Text = dr[7].ToString();
                txtMotherNameB.Text = dr[8].ToString();
                txtMotherNameE.Text = dr[9].ToString();
                txtMotherOccupation.Text = dr[10].ToString();
                txtMotherIncome.Text = dr[11].ToString();
                txtPresentAddress.Text = dr[12].ToString();
                txtPhoneOffice.Text = dr[13].ToString();
                txtPhoneHouse.Text = dr[14].ToString();
                txtPhoneMobile.Text = dr[15].ToString();
                txtPermanentAddress.Text = dr[16].ToString();
                txtGuardianName.Text = dr[17].ToString();
                txtGuardianRelation.Text = dr[18].ToString();
                txtGuardianAddress.Text = dr[19].ToString();
                txtGuardianOccupation.Text = dr[20].ToString();
                txtDOB.Text = dr[21].ToString();
                txtStudentAge.Text = dr[22].ToString();
                txtExSchool.Text = dr[23].ToString();
                ddReligion.SelectedValue = dr[24].ToString();
                txtNationality.Text = dr[25].ToString();
                txtVaccineInfo.Text = dr[26].ToString();
                txtApplicationDate.Text = dr[27].ToString();
                txtBloodGroup.Text = dr[28].ToString();
                txtHobby.Text = dr[29].ToString();
                Image1.ImageUrl = dr[30].ToString();
                ddGender.SelectedValue = dr[31].ToString();

                Image1.Attributes.Remove("Style");
            }
            cmd7.Connection.Close();
        }

        protected void txtSession_TextChanged(object sender, EventArgs e)
        {
            lblStudentID.Text = GenerateStudentID();
            GridView1.DataBind();
        }
        private string GenerateStudentID()
        {
            try
            {
                string classIDFormat = RunQuery.SQLQuery.ReturnString("Select IDFormat from Class where sl='" + ddClass.SelectedValue + "'");

                string session = txtSession.Text;
                if (session.Length >= 4)
                {
                    session = session.Substring(0, 4);
                }
                else
                {
                    session = session.Substring(0, 2);
                }

                int sCounter = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(StudentID) from Students where Class='" + ddClass.SelectedValue + "'")) + 1;
                string studentID = classIDFormat + session + sCounter.ToString();
                if (sCounter.ToString().Length < 2)
                {
                    studentID = classIDFormat + session + "0" + sCounter.ToString();
                }

                //chk if exist
                string isExist = RunQuery.SQLQuery.ReturnString("Select StudentID from Students where StudentID='" + studentID + "'");
                while (isExist != "")
                {
                    sCounter++;
                    studentID = classIDFormat + session + sCounter.ToString();
                    if (sCounter.ToString().Length < 2)
                    {
                        studentID = classIDFormat + session + "0" + sCounter.ToString();
                    }
                    isExist = RunQuery.SQLQuery.ReturnString("Select StudentID from Students where StudentID='" + studentID + "'");
                }

                return studentID;
            }

            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}