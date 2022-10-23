using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class AdminCentral_Branch_Setup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
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
            SqlCommand cmd2x = new SqlCommand("INSERT INTO Branches (Type, BranchName, District, Area,  Address, ContactNo, EmailAddress, CreditLimit, OpenningBalance)" +
                                                "VALUES (@Type, @BranchName, @District, @Area, @Address, @ContactNo, @EmailAddress, @CreditLimit, @OpenningBalance)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2x.Parameters.Add("@Type", SqlDbType.VarChar).Value = "Branch"; //
            cmd2x.Parameters.Add("@BranchName", SqlDbType.VarChar).Value = txtName.Text;
            cmd2x.Parameters.Add("@District", SqlDbType.VarChar).Value = ddlArea.SelectedValue;
            cmd2x.Parameters.Add("@Area", SqlDbType.VarChar).Value = ddType.SelectedValue;
            cmd2x.Parameters.Add("@Address", SqlDbType.VarChar).Value = txtAddress.Text;
            cmd2x.Parameters.Add("@ContactNo", SqlDbType.VarChar).Value = txtMobile.Text;
            cmd2x.Parameters.Add("@EmailAddress", SqlDbType.VarChar).Value = txtEmail.Text;
            cmd2x.Parameters.Add("@CreditLimit", SqlDbType.Decimal).Value = txtLimit.Text;
            cmd2x.Parameters.Add("@OpenningBalance", SqlDbType.Decimal).Value = txtBalance.Text;

            cmd2x.Connection.Open();
            int success = cmd2x.ExecuteNonQuery();
            cmd2x.Connection.Close();


            //insert op balance
            string lName = Page.User.Identity.Name.ToString();
            SqlCommand cmd2 = new SqlCommand("INSERT INTO Transactions (HeadName, Description, Dr, Balance,  EntryBy)" +
                                                "VALUES (@HeadName, @Description, @Dr, @Balance, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = txtName.Text;
            cmd2.Parameters.Add("@Description", SqlDbType.VarChar).Value = "Openning Balance";
            cmd2.Parameters.Add("@Dr", SqlDbType.Decimal).Value = txtBalance.Text;
            cmd2.Parameters.Add("@Balance", SqlDbType.Decimal).Value = Convert.ToDecimal(txtBalance.Text);// *-1M;
            cmd2.Parameters.Add("@EntryBy", SqlDbType.VarChar).Value = lName;

            cmd2.Connection.Open();
            int success2 = cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();

            if (success > 0 && success2 > 0)
            {
                lblMsg.Text = "Succesfully Added " + txtName.Text + " Branch into Database!";
                ClearControls(Form);
                //GridView1.DataBind();
                MessageBox("New Branch Saved! " + txtName.Text);
            }
        }
        catch
        {
            txtName.Focus();
            MessageBox("Branch Name Already Exist in the Database!");
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }
}
