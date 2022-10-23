using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using RunQuery;


public partial class app_Company : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //txtName.Focus();
        Button1.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(Button1, null) + ";");
    }
    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    private void UpdateInfo()
    {
        string lName = Page.User.Identity.Name.ToString();
        string linkPath = "./Docs/Company/";
        if (FileUpload1.HasFile)
        {
            string photoUrl = SQLQuery.UploadImage(txtName.Text, FileUpload1, Server.MapPath(".\\Docs\\Company\\"), Server.MapPath(linkPath), linkPath, lName, "Company");
            SQLQuery.ExecNonQry("UPDATE Company SET Logo='" + photoUrl + "' WHERE CompanyID='" + CompanyIdHiddenField.Value + "'");
            string path = SQLQuery.ReturnString("SELECT PhotoURL FROM Photos WHERE PhotoID=(SELECT Logo FROM Company WHERE CompanyID='" + CompanyIdHiddenField.Value + "')");
            FileStream fs = new FileStream(Server.MapPath(path), System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] image = new byte[fs.Length];
            fs.Read(image, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            SqlCommand cmd7 = new SqlCommand("UPDATE Company SET Photo=@Photo WHERE CompanyID='" + CompanyIdHiddenField.Value + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Parameters.Add("@Photo", SqlDbType.Image).Value = image;
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();
            cmd7.Connection.Dispose();
        }

        SQLQuery.ExecNonQry("UPDATE Company SET CompanyName='" + txtName.Text + "', MobileNo='" + txtMobile.Text + "', Email='" + txtEmail.Text + "', ProjectName='" + txtProjectName.Text + "', CompanySpeciality='" + txtSpeciality.Text + "', CompanyAddress='" + txtAddress.Text + "', AuthorityName='" + txtAuthority.Text + "' WHERE CompanyID='" + CompanyIdHiddenField.Value + "'");
    }

    private void Memberinfo()
    {
        string lName = Page.User.Identity.Name.ToString();
        if (txtName.Text != "")
        {
            //Session["imgId"] = "";
            //if (FileUpload1.HasFile)
            //{
            //    Session["imgId"] = RunQuery.SQLQuery.UploadFile(HttpUtility.HtmlEncode(txtName.Text), FileUpload1,
            //        Server.MapPath(".\\Uploads\\Photos\\"), Server.MapPath("./Uploads/Photos/"), "./Uploads/Photos/",
            //        Page.User.Identity.Name.ToString(), "Company Registration");
            //}
            //else
            //{
            //    lblErrLoad.Text = "Please upload your company logo.";
            //}

            SQLQuery.ExecNonQry(@"INSERT INTO Company (CompanyName, MobileNo, Email, ProjectName, CompanySpeciality, CompanyAddress, AuthorityName)
                                  VALUES ('" + txtName.Text + "','" + txtMobile.Text + "','" + txtEmail.Text + "','" + txtProjectName.Text + "','" + txtSpeciality.Text + "','" + txtAddress.Text + "','" + txtAuthority.Text + "')");

        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Button1.Text == "Update")
        {
            try
            {
                UpdateInfo();

                Button1.Text = "Save Company";
                txtName.ReadOnly = false;
                TextClear();
                GridView1.DataBind();
                
                lblMsg.Attributes.Add("class", "xerp_info");
                lblMsg.Text = "Company Info Update Successfully";
                Notify("Company Info Update Successfully", "success", lblMsg);
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }
        else
        {
            try
            {
                Memberinfo();
                //MessageBox("New Company Setup is done.");
                TextClear();
                Notify("New Company Setup is successful", "success", lblMsg);
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }
    }

    private void TextClear()
    {
        txtName.Text = "";
        txtAddress.Text = "";
        txtAuthority.Text = "";
        txtEmail.Text = "";
        txtMobile.Text = "";
        txtProjectName.Text = "";
        txtSpeciality.Text = "";
    }


    protected void btnEdit_Click(object sender, EventArgs e)
    {
        //txtName.Visible = false;
        //ddName.Visible = true;
        Button1.Text = "Update";
        LoadEditMode("");
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        TextClear();
    }
    private void LoadEditMode(string companyId)
    {
        SqlCommand cmd = new SqlCommand("SELECT CompanyID, CompanyName, MobileNo, Email, ProjectName, CompanySpeciality, CompanyAddress, AuthorityName FROM Company WHERE CompanyID='" + companyId + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            CompanyIdHiddenField.Value = dr["CompanyID"].ToString();
            txtName.Text = dr["CompanyName"].ToString();
            txtMobile.Text = dr["MobileNo"].ToString();
            txtEmail.Text = dr["Email"].ToString();
            txtProjectName.Text = dr["ProjectName"].ToString();
            txtSpeciality.Text = dr["CompanySpeciality"].ToString();
            txtAddress.Text = dr["CompanyAddress"].ToString();
            txtAuthority.Text = dr["AuthorityName"].ToString();

            //string pType = dr[4].ToString();
            //if (pType == "C")
            //{ txtCountry.SelectedValue = "C"; }
            //else
            //{ txtCountry.SelectedValue = "S"; }
            //txtCredit.Text = dr[5].ToString();
            //txtBalance.Text = dr[6].ToString();
        }
        cmd.Connection.Close();
        cmd.Connection.Dispose();
        Notify("Edit Mode Activeted.", "success", lblMsg);
    }


    protected void ddName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //LoadEditMode();
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtName.ReadOnly = true;
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label lblCompanyId = GridView1.Rows[index].FindControl("Label1") as Label;
            
            Button1.Text = "Update";
            if (lblCompanyId != null) LoadEditMode(lblCompanyId.Text);
        }
        catch (Exception ex)
        {
            lblMsg.CssClass = "xerp_error";
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "ERROR: " + ex.ToString();

        }
    }
}