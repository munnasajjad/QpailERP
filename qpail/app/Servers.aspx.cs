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
using System.Net;
using System.Net.Mail;
using Accounting;

public partial class AdminCentral_Servers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        txtDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
    }
    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox)
        { (Parent as TextBox).Text = string.Empty; }
        else
        {
            foreach (Control c in Parent.Controls)
                ClearControls(c);
        }
    }
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        saveData();
    }
    private void saveData()
    {
        try
        {
            //Creating User
            //CreateUser(txtLogin.Text, txtName.Text, txtIP.Text);

            SqlCommand cmd2x = new SqlCommand("INSERT INTO Servers (IPAddress, Alias, LoginID, Password, DatabaseName, DBUser, DBPassword, Port, Enabled, Type, OpDate, Remarks, EntryBy)" +
                                                "VALUES (@IPAddress, @Alias, @LoginID, @Password, @DatabaseName, @DBUser, @DBPassword, @Port, @Enabled, @Type, @OpDate, @Remarks, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2x.Parameters.Add("@IPAddress", SqlDbType.NVarChar).Value = txtIP.Text;
            cmd2x.Parameters.Add("@Alias", SqlDbType.NVarChar).Value = txtName.Text;
            cmd2x.Parameters.Add("@LoginID", SqlDbType.NVarChar).Value = txtLogin.Text;
            cmd2x.Parameters.Add("@Password", SqlDbType.NVarChar).Value = txtPassword.Text;

            cmd2x.Parameters.Add("@DatabaseName", SqlDbType.NVarChar).Value = txtDB.Text;
            cmd2x.Parameters.Add("@DBUser", SqlDbType.NVarChar).Value = txtDbUser.Text;
            cmd2x.Parameters.Add("@DBPassword", SqlDbType.NVarChar).Value = txtDbPass.Text;
            cmd2x.Parameters.Add("@Port", SqlDbType.NVarChar).Value = txtPort.Text;
            cmd2x.Parameters.Add("@Enabled", SqlDbType.NVarChar).Value = ddEnabled.SelectedValue;
            cmd2x.Parameters.Add("@Type", SqlDbType.NVarChar).Value = ddType.SelectedValue;
            cmd2x.Parameters.Add("@OpDate", SqlDbType.DateTime).Value = txtDate.Text;
            cmd2x.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = txtReamarks.Text;
            cmd2x.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = Page.User.Identity.Name.ToString();


            cmd2x.Connection.Open();
            int success = cmd2x.ExecuteNonQuery();
            cmd2x.Connection.Close();

            //Accounting.VoucherEntry.TransactionEntry("", txtName.Text, "Openning Balance for vendor", 0, Convert.ToInt32(txtBalance.Text), Convert.ToDateTime(txtDate.Text), Page.User.Identity.Name.ToString());

            lblMsg.Text = "Succesfully Added " + txtName.Text + " into Database!";
            ClearControls(Form);
            GridView1.DataBind();
            MessageBox("New Info has been accepted & saved! " + txtName.Text);

        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.ToString();
            txtName.Focus();
            MessageBox(ex.Message.ToString());
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        ClearControls(Form);
    }


    private void CreateUser(string memberID, string password, string email)
    {
        string msg = "";
        string passwordQuestion = "What is your pass?";

        MembershipCreateStatus createStatus;
        MembershipUser newUser = Membership.CreateUser(memberID, password, email, passwordQuestion, password, true, out createStatus);
        switch (createStatus)
        {
            case MembershipCreateStatus.Success:
                msg = "The user account was successfully created! " + memberID;
                break;
            case MembershipCreateStatus.DuplicateUserName:
                msg = "There already exists a user with this username." + memberID;
                break;

            case MembershipCreateStatus.DuplicateEmail:
                msg = "There already exists a user with this email address." + memberID;
                break;
            case MembershipCreateStatus.InvalidEmail:
                msg = "There email address you provided in invalid." + memberID;
                break;
            case MembershipCreateStatus.InvalidAnswer:
                msg = "There security answer was invalid." + memberID;
                break;
            case MembershipCreateStatus.InvalidPassword:
                msg = "The password you provided is invalid. It must be seven characters long and have at least one non-alphanumeric character." + memberID;

                break;
            default:
                msg = "There was an unknown error; the user account was NOT created." + memberID;
                break;
        }

        Roles.AddUserToRole(memberID, "Servers");
        //CreateAccountResults.Text = CreateAccountResults.Text + "<br>" + msg;

    }

}
