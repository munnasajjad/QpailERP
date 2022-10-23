using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class app_Unlock : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    protected void btnSave0_Click(object sender, EventArgs e)
    {
        MembershipUser membershipUser = Membership.GetUser(txtUid.Text);      //.GetUser(false);
        try
        {
            if (txtUid.Text != "")
            {
                bool IsApproved = membershipUser.IsLockedOut;
                if (IsApproved == true)
                {
                    lblCurrEmail.Text = "This user is blocked";
                    
                    RadioButton2.Checked = false;
                    RadioButton1.Checked = true;
                    btnSave.Text = "Unblock";
                    lblCurrEmail.ForeColor = System.Drawing.ColorTranslator.FromHtml("red");

                }
                else
                {
                    lblCurrEmail.Text = "User was not blocked";
                    RadioButton1.Checked = false;
                    RadioButton2.Checked = true;
                    btnSave.Text = "Block";
                    lblCurrEmail.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0066ff");
                }
            }
        }
        catch (Exception ex)
        {
            lblCurrEmail.Text = "You Typed an Invalid ID"+ex;
        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (RadioButton1.Checked == true)
            {
                SqlCommand cmd3 = new SqlCommand("update aspnet_Membership set failedpasswordattemptcount=0", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd3.Connection.Open();
                cmd3.ExecuteNonQuery();
                cmd3.Connection.Close();

                MembershipUser membershipUser = Membership.GetUser(txtUid.Text); // Use (false); instead of (txtUid.Text) to get current user.
                membershipUser.UnlockUser();
                MessageBox("User Successfully Unlocked");
            }
            else if (RadioButton2.Checked == true)
            {
                MembershipUser membershipUser = Membership.GetUser(txtUid.Text); // Use (false); instead of (txtUid.Text) to get current user.
                LockUser(membershipUser);
                MessageBox("User Successfully Locked");
            }
            else
            {
                SqlCommand cmd3x = new SqlCommand("update aspnet_Membership set islockedout='false', failedpasswordattemptcount=0", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd3x.Connection.Open();
                cmd3x.ExecuteNonQuery();
                cmd3x.Connection.Close();
                MessageBox("All Locked Users Successfully Unlocked");
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "You Typed an Invalid ID" + ex;
        }
        finally
        {
            GridView1.DataBind();
        }
    }
    public static bool LockUser(MembershipUser user)
    {
        try
        {
            for (int i = 0; i < Membership.MaxInvalidPasswordAttempts; i++)
                Membership.ValidateUser(user.UserName, "thisisandummypasswordonlytolocktheuser");

            return user.IsLockedOut;
        }
        catch (Exception)
        {
            throw;
        }

    }
}
