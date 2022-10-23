using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using RunQuery;

public partial class Operator_Employee_Info : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        

        if (!IsPostBack)
        {
            ddDepartment.DataBind();
            ddSection.DataBind();
            GridView1.DataBind();
            //ImgPhoto.Visible = false;

            string s = Request.QueryString["id"];
            if (s!=null)
            {
                EditMode(s);
            }
        }
    }


    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }


    // Save Button functionalities---
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string empSerial = txtEid.Text;
            if (chkCode.Checked || txtEid.Text == "")
            {
                empSerial = DateTime.Now.Year.ToString().Substring(2,2) + "000" + RunQuery.SQLQuery.ReturnString("SELECT isnull(max(EmployeeInfoID) + 1,1) FROM EmployeeInfo");
                txtEid.Text = empSerial;
            }

            if (btnSave.Text == "Save")
            {
                string isExist = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) EName FROM EmployeeInfo where EmpSerial='" + empSerial + "'");
                if (isExist == "")
                {
                    saveEmployee(empSerial);
                    empSerial = RunQuery.SQLQuery.ReturnString("SELECT max(EmployeeInfoID) FROM EmployeeInfo");
                    updateEmployee(empSerial);
                    saveEmployeeHistory(empSerial);
                    Notify("New employee added successfully", "success", lblMsg);
                }
                else
                {
                    Notify("Error: Card No. already exist for " + isExist, "warn", lblMsg);
                }
            }
            else
            {
                updateEmployee(lblEid.Text);
                btnSave.Text = "";
                saveEmployeeHistory(lblEid.Text);
                Notify("Employee updated successfully", "success", lblMsg);
            }

            clearForm();
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
        finally
        {
            HttpRuntime.UnloadAppDomain();
            GridView1.DataBind();
        }
    }

    private void saveEmployee(string empSerial)
    {
        string lName = Page.User.Identity.Name.ToString();
        //RunQuery.SQLQuery.ExecNonQry("Update Employee_Images Set EmployeeID='" + empSerial + "' where EmployeeID='' AND EntryBy='" + lName + "'");

        SqlCommand cmd2 = new SqlCommand("INSERT INTO EmployeeInfo (DepartmentID, SectionID, Designation, EmpSerial, EName, FathersName, MothersName, NID, ContactAddress, PermanentAddress, ContactNumber, Email, Age, Experience, SalaryBasis, Salary, Remark, ProjectID, EntryBy) VALUES (@DepartmentID, @SectionID, @Designation, @EmpSerial, @EName, @FathersName, @MothersName, @NID, @ContactAddress, @PermanentAddress, @ContactNumber, @Email, @Age, @Experience, @SalaryBasis, @Salary, @Remark, @ProjectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@DepartmentID", ddDepartment.SelectedValue);
        cmd2.Parameters.AddWithValue("@SectionID", ddSection.SelectedValue);
        cmd2.Parameters.AddWithValue("@Designation", ddDesignation.SelectedValue);
        cmd2.Parameters.AddWithValue("@EmpSerial", empSerial);

        cmd2.Parameters.AddWithValue("@EName", txtEname.Text);
        cmd2.Parameters.AddWithValue("@FathersName", txtFather.Text);
        cmd2.Parameters.AddWithValue("@MothersName", txtMother.Text);
        cmd2.Parameters.AddWithValue("@NID", txtNID.Text);
        cmd2.Parameters.AddWithValue("@ContactAddress", txtCaddress.Text);

        cmd2.Parameters.AddWithValue("@PermanentAddress", txtPermanent.Text);
        cmd2.Parameters.AddWithValue("@ContactNumber", txtCno.Text);
        cmd2.Parameters.AddWithValue("@Email", txtEmail.Text);
        cmd2.Parameters.AddWithValue("@Age", txtAge.Text);
        cmd2.Parameters.AddWithValue("@Experience", txtExp.Text);
        cmd2.Parameters.AddWithValue("@SalaryBasis", ddBasis.SelectedValue);
        cmd2.Parameters.AddWithValue("@Salary", txtSalary.Text);
        cmd2.Parameters.AddWithValue("@Remark", txtRemark.Text);

        cmd2.Parameters.AddWithValue("@ProjectID", "1");
        cmd2.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void updateEmployee(string empSerial)
    {
        string lName = Page.User.Identity.Name.ToString();
        string linkPath = "./Docs/Employee/Photo/";
        if (FileUpload2.HasFile)
        {
            string photoURL = RunQuery.SQLQuery.UploadImage(empSerial, FileUpload2, Server.MapPath(".\\Docs\\Employee\\Photo\\"), Server.MapPath(linkPath), linkPath, Page.User.Identity.Name.ToString(), "Employee");
            SQLQuery.ExecNonQry("UPDATE EmployeeInfo SET Photo='" + photoURL + "' where (EmployeeInfoID ='" + empSerial + "')");
            //RunQuery.SQLQuery.ExecNonQry("Update Employee_Images Set EmployeeID='" + empSerial + "' where EmployeeID='' AND EntryBy='" + lName + "'");
        }
        if (FileUpload1.HasFile)
        {
            linkPath = "./Docs/Employee/CV/";
            string photoURL = RunQuery.SQLQuery.UploadFile(empSerial, FileUpload1, Server.MapPath(".\\Docs\\Employee\\CV\\"), Server.MapPath(linkPath), linkPath, Page.User.Identity.Name.ToString(), "Employee");
            SQLQuery.ExecNonQry("UPDATE EmployeeInfo SET CV='" + photoURL + "' where (EmployeeInfoID ='" + empSerial + "')");
        }

        SqlCommand cmd2 = new SqlCommand("UPDATE EmployeeInfo Set DepartmentID=@DepartmentID, SectionID=@SectionID, Designation=@Designation, " +
                                         "EmpSerial=@EmpSerial, CardNo='" + txtCardNo.Text + "', EName=@EName, FathersName=@FathersName, " +
                                         "MothersName=@MothersName, NID=@NID, ContactAddress=@ContactAddress, PermanentAddress=@PermanentAddress, " +
                                         "ContactNumber=@ContactNumber, Email=@Email, Age=@Age, Experience=@Experience, SalaryBasis=@SalaryBasis, " +
                                         "Salary=@Salary, Education='" + txtEducation.Text + "',  Skills='" + txtSkills.Text + "', Remark=@Remark where EmployeeInfoID='" + empSerial + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@DepartmentID", ddDepartment.SelectedValue);
        cmd2.Parameters.AddWithValue("@SectionID", ddSection.SelectedValue);
        cmd2.Parameters.AddWithValue("@Designation", ddDesignation.SelectedValue);
        cmd2.Parameters.AddWithValue("@EmpSerial", txtEid.Text);

        cmd2.Parameters.AddWithValue("@EName", txtEname.Text);
        cmd2.Parameters.AddWithValue("@FathersName", txtFather.Text);
        cmd2.Parameters.AddWithValue("@MothersName", txtMother.Text);
        cmd2.Parameters.AddWithValue("@NID", txtNID.Text);
        cmd2.Parameters.AddWithValue("@ContactAddress", txtCaddress.Text);

        cmd2.Parameters.AddWithValue("@PermanentAddress", txtPermanent.Text);
        cmd2.Parameters.AddWithValue("@ContactNumber", txtCno.Text);
        cmd2.Parameters.AddWithValue("@Email", txtEmail.Text);
        cmd2.Parameters.AddWithValue("@Age", txtAge.Text);
        cmd2.Parameters.AddWithValue("@Experience", txtExp.Text);
        cmd2.Parameters.AddWithValue("@SalaryBasis", ddBasis.SelectedValue);
        cmd2.Parameters.AddWithValue("@Salary", txtSalary.Text);
        cmd2.Parameters.AddWithValue("@Remark", txtRemark.Text);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        if (txtDate.Text != "")
        {
            RunQuery.SQLQuery.ExecNonQry("UPDATE EmployeeInfo SET  JoiningDate='" + Convert.ToDateTime(txtJoinDate.Text).ToString("yyyy-MM-dd") + "' where (EmployeeInfoID ='" + empSerial + "')");
        }

        if (txtDate.Text != "")
        {
            RunQuery.SQLQuery.ExecNonQry("UPDATE EmployeeInfo SET  DateOfBirth='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' where (EmployeeInfoID ='" + empSerial + "')");
        }

    }

    private void clearForm()
    {
        txtEid.Text = "";
        txtEname.Text = "";
        txtFather.Text = "";
        txtMother.Text = "";
        txtNID.Text = "";
        txtCaddress.Text = "";
        txtPermanent.Text = "";
        txtCno.Text = "";
        txtEmail.Text = "";
        txtAge.Text = "";
        txtExp.Text = "";
        txtSalary.Text = "";
        txtRemark.Text = "";

        txtCardNo.Text = "";
        txtEducation.Text = "";
        txtSkills.Text = "";
        hypCV.Text = "";
        Image1.Visible = false;

        btnSave.Text = "Save";
    }

    private void saveEmployeeHistory(string empSerial)
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd2 = new SqlCommand("INSERT INTO EmployeeInfo_History (DepartmentID, SectionID, Designation, EmpSerial, EName, FathersName, MothersName, NID, ContactAddress, PermanentAddress, ContactNumber, Email, Age, Experience, SalaryBasis, Salary, Remark, ProjectID, EntryBy) VALUES (@DepartmentID, @SectionID, @Designation, @EmpSerial, @EName, @FathersName, @MothersName, @NID, @ContactAddress, @PermanentAddress, @ContactNumber, @Email, @Age, @Experience, @SalaryBasis, @Salary, @Remark, @ProjectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@DepartmentID", ddDepartment.SelectedValue);
        cmd2.Parameters.AddWithValue("@SectionID", ddSection.SelectedValue);
        cmd2.Parameters.AddWithValue("@Designation", ddDesignation.SelectedValue);
        cmd2.Parameters.AddWithValue("@EmpSerial", empSerial);
        //cmd2.Parameters.AddWithValue("@JoiningDate", Convert.ToDateTime(txtJoinDate.Text).ToString("yyyy-MM-dd"));

        cmd2.Parameters.AddWithValue("@EName", txtEname.Text);
        cmd2.Parameters.AddWithValue("@FathersName", txtFather.Text);
        cmd2.Parameters.AddWithValue("@MothersName", txtMother.Text);
        cmd2.Parameters.AddWithValue("@NID", txtNID.Text);
        cmd2.Parameters.AddWithValue("@ContactAddress", txtCaddress.Text);

        cmd2.Parameters.AddWithValue("@PermanentAddress", txtPermanent.Text);
        cmd2.Parameters.AddWithValue("@ContactNumber", txtCno.Text);
        cmd2.Parameters.AddWithValue("@Email", txtEmail.Text);
        cmd2.Parameters.AddWithValue("@Age", txtAge.Text);
        cmd2.Parameters.AddWithValue("@Experience", txtExp.Text);
        cmd2.Parameters.AddWithValue("@SalaryBasis", ddBasis.SelectedValue);
        cmd2.Parameters.AddWithValue("@Salary", txtSalary.Text);
        cmd2.Parameters.AddWithValue("@Remark", txtRemark.Text);

        cmd2.Parameters.AddWithValue("@ProjectID", "1");
        cmd2.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    protected void ddDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddSection.DataBind();
        GridView1.DataBind();
    }


    private void DeletePhoto()
    {
        string lName = Page.User.Identity.Name.ToString();
        RunQuery.SQLQuery.ExecNonQry("Delete Employee_Images where EmployeeID='' AND EntryBy='" + lName + "'");
    }
    /*
    protected void AsyncFileUpload1_UploadComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        DeletePhoto();

        SqlCommand cmd1 = new SqlCommand("SELECT isnull(max(iid) + 1,1) FROM Employee_Images", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd1.Connection.Open();
        string fileName = cmd1.ExecuteScalar().ToString();
        cmd1.Connection.Close();

        string tExt = Path.GetFileName(AsyncFileUpload1.PostedFile.ContentType);
        fileName = fileName + "." + tExt;

        string strFolder = Server.MapPath(".\\Employee\\");
        string strFullPath = strFolder + fileName;

        if (File.Exists(strFullPath))
        {
            File.Delete(strFullPath);
        }

        var file = AsyncFileUpload1.PostedFile.InputStream;
        System.Drawing.Image img = System.Drawing.Image.FromStream(file, false, false);

        Size size = new Size(300, 350);
        img = resizeImage(img, size);

        if (tExt == "jpeg")
        {
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            img.Save(Server.MapPath("./Docs/Employee/" + fileName), jgpEncoder, myEncoderParameters);
        }
        else
        {
            img.Save(Server.MapPath("./Docs/Employee/" + fileName));
        }

        string eid = "";
        if (btnSave.Text != "Save")
        {
            eid = txtEid.Text;
            RunQuery.SQLQuery.ExecNonQry("Delete Employee_Images where EmployeeID='" + eid + "' AND DocumentName='Photograph'");
        }

        SqlCommand cmd = new SqlCommand("INSERT INTO Employee_Images (EmployeeID, DocumentName, ImgLink, imgSl, EntryBy)" +
                                    "VALUES (@EmployeeID, @DocumentName, @ImgLink, @imgSl, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EmployeeID", eid);
        cmd.Parameters.AddWithValue("@DocumentName", "Photograph");
        cmd.Parameters.AddWithValue("@ImgLink", "Docs/Employee/" + fileName);
        cmd.Parameters.AddWithValue("@imgSl", "1");
        cmd.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();

        ImgPhoto.ImageUrl = "./Docs/Employee/" + fileName;
        ImgPhoto.Visible = true;
        //AsyncFileUpload1.Visible = false;
        //pnl.Update();
    }
    
    //Image Resizing
    public static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, System.Drawing.Size size)
    {
        return (System.Drawing.Image)(new System.Drawing.Bitmap(imgToResize, size));
    }

    private ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }
    //End of Resize Image        
     */

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        EditMode(lblItemName.Text);
    }

    private void EditMode(string entryID)
    {
        SqlCommand cmd7 = new SqlCommand("SELECT DepartmentID, SectionID, Designation, EmpSerial, JoiningDate, EName, FathersName, MothersName, NID, ContactAddress, PermanentAddress, ContactNumber, Email, Age, Experience, SalaryBasis, Salary, Remark, ProjectID, EntryBy FROM [EmployeeInfo] WHERE EmployeeInfoID=@EmployeeInfoID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Parameters.Add("@EmployeeInfoID", SqlDbType.VarChar).Value = entryID;
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            ddDepartment.SelectedValue = dr[0].ToString();
            ddSection.DataBind();
            ddSection.SelectedValue = dr[1].ToString();
            ddDesignation.SelectedValue = dr[2].ToString();
            txtEid.Text = dr[3].ToString();
            txtEid.Text = dr[3].ToString();
            //if (dr[4].ToString() == "")
            //{
            //    txtJoinDate.Text = Convert.ToDateTime(dr[4].ToString()).ToShortDateString();
            //}
            //else
            //{
            //    txtJoinDate.Text = "";
            //}
            txtEname.Text = dr[5].ToString();
            txtFather.Text = dr[6].ToString();
            txtMother.Text = dr[7].ToString();
            txtNID.Text = dr[8].ToString();
            txtCaddress.Text = dr[9].ToString();
            txtPermanent.Text = dr[10].ToString();
            txtCno.Text = dr[11].ToString();
            txtEmail.Text = dr[12].ToString();
            txtAge.Text = dr[13].ToString();
            txtExp.Text = dr[14].ToString();
            ddBasis.SelectedValue = dr[15].ToString();
            txtSalary.Text = dr[16].ToString();
            txtRemark.Text = dr[17].ToString();

        }
        cmd7.Connection.Close();

        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) DepartmentID, SectionID, Designation, Photo, EName, FathersName, MothersName, NID, DateOfBirth, ContactAddress, 
                         PermanentAddress, ContactNumber, Email, Age, EmpSerial, CardNo, JoiningDate, Experience, SalaryBasis, Salary, Education, Skills, Remark, CV, IsActive FROM EmployeeInfo WHERE (EmployeeInfoID = '" + entryID + "')");

        foreach (DataRow drx in dtx.Rows)
        {
            string dob = drx["DateOfBirth"].ToString();
            if (dob != "")
            {
                txtDate.Text = Convert.ToDateTime(dob).ToString("dd/MM/yyyy");
            }
            dob = drx["JoiningDate"].ToString();
            if (dob != "")
            {
                txtJoinDate.Text = Convert.ToDateTime(dob).ToString("dd/MM/yyyy");
            }

            txtCardNo.Text = drx["CardNo"].ToString();
            txtEducation.Text = drx["Education"].ToString();
            txtSkills.Text = drx["Skills"].ToString();

            //string imgURL = RunQuery.SQLQuery.ReturnString("Select ImgLink from Employee_Images where EmployeeID='" + entryID + "' AND DocumentName='Photograph'");
            //ImgPhoto.ImageUrl = imgURL;
            //ImgPhoto.Visible = true;

            Image1.Visible = false;
            string photo = drx["Photo"].ToString();
            if (photo != "")
            {
                photo = SQLQuery.ReturnString("Select PhotoURL from photos where PhotoID='" + photo + "'");
                HyperLink1.NavigateUrl = photo;
                Image1.Visible = true;
                Image1.ImageUrl = photo;
            }

            string cv = drx["CV"].ToString();
            if (cv != "")
            {
                hypCV.Text = "Download CV File";
                hypCV.NavigateUrl = SQLQuery.ReturnString("Select PhotoURL from photos where PhotoID='" + cv + "'");
            }
        }

        lblEid.Text = entryID;
        btnSave.Text = "Update";
        Notify("You are in edit mode!", "warn", lblMsg);
    }

    protected void Button1_OnClick(object sender, EventArgs e)
    {
        clearForm();
    }

    protected void ddSection_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
}