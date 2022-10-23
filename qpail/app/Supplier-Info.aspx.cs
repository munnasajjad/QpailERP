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

public partial class Operator_Supplier_Info : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //txtName.Focus();
        Button1.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(Button1, null) + ";");
        
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd7 = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        lblProject.Text = Convert.ToString(cmd7.ExecuteScalar());
        cmd7.Connection.Close();


    }
    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    private void UpdateInfo()
    {
        string lName = Page.User.Identity.Name.ToString();
        
        if (ddName.SelectedValue != "---Select---")
        {
            //Create Sql Connection
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

            //Create Sql Command
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE Party SET VarOrganizationName=@Company, VarSupplierAddress=@Address, varSupplierType= @Country, VarEmailAddress=@Email, VarPhoneNumber=@Mobile, OpBalance=@OpBalance, fltCreditLimit=@Credit, VarZone=@VarZone where VarSupplierName=@Name";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            //Parameter array Declaration
            SqlParameter[] param = new SqlParameter[9];

            param[0] = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
            param[1] = new SqlParameter("@Company", SqlDbType.NVarChar, 50);
            param[2] = new SqlParameter("@Address", SqlDbType.NVarChar, 300);
            param[3] = new SqlParameter("@Country", SqlDbType.NVarChar, 50);
            param[4] = new SqlParameter("@Email", SqlDbType.NVarChar, 50);
            param[5] = new SqlParameter("@Mobile", SqlDbType.NVarChar, 50);
            param[6] = new SqlParameter("@OpBalance", SqlDbType.Decimal);
            param[7] = new SqlParameter("@Credit", SqlDbType.Decimal);
            param[8] = new SqlParameter("@VarZone", SqlDbType.NVarChar, 50);

            param[0].Value = ddName.SelectedValue;
            param[1].Value = txtCompany.Text;
            param[2].Value = txtAddress.Text;
            param[3].Value = txtCountry.SelectedValue;
            param[4].Value = txtEmail.Text;
            param[5].Value = txtMobile.Text;
            param[6].Value = txtBalance.Text;
            param[7].Value = txtCredit.Text;
            param[8].Value = ddArea.SelectedValue;

            /*** Looping Fields ***/
            for (int i = 0; i < param.Length; i++)
            {
                cmd.Parameters.Add(param[i]);
            }

            /*** Command Execution ***/

            cnn.Open();
            int Success = cmd.ExecuteNonQuery();
            cnn.Close();

            SqlCommand cmdxx = new SqlCommand("SELECT HeadName FROM [Transactions] where HeadName='" + ddName.SelectedValue + "' and Description='Openning Balance'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmdxx.Connection.Open();
            string hn = Convert.ToString(cmdxx.ExecuteScalar());
            cmdxx.Connection.Close();
            cmdxx.Connection.Dispose();

            if (hn == "")
            {
                //vendor Part
                decimal debit = 0; decimal credit = 0;
                if (txtCountry.SelectedValue == "C")
                {
                    debit = Convert.ToDecimal(txtBalance.Text);
                }
                else
                {
                    credit = Convert.ToDecimal(txtBalance.Text);
                }

                SqlCommand cmd2 = new SqlCommand("INSERT INTO Transactions (HeadName, Description, Dr, Cr, Balance,  EntryBy, InvNo)" +
                                            "VALUES (@HeadName, @Description, @Dr, @Cr, @Balance, @EntryBy, @InvNo)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = ddName.SelectedValue;
                cmd2.Parameters.Add("@Description", SqlDbType.VarChar).Value = "Openning Balance";
                cmd2.Parameters.Add("@Dr", SqlDbType.Decimal).Value = debit;
                cmd2.Parameters.Add("@Cr", SqlDbType.Decimal).Value = credit;
                cmd2.Parameters.Add("@Balance", SqlDbType.Decimal).Value = debit - credit;
                cmd2.Parameters.Add("@EntryBy", SqlDbType.VarChar).Value = lName;
                cmd2.Parameters.Add("@InvNo", SqlDbType.VarChar).Value = "na";

                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
            }
            else
            {
                //vendor Part
                decimal debit = 0; decimal credit = 0;
                if (txtCountry.SelectedValue == "C")
                {
                    debit = Convert.ToDecimal(txtBalance.Text);
                }
                else
                {
                    credit = Convert.ToDecimal(txtBalance.Text);
                }

                SqlCommand cmd2 = new SqlCommand("UPDATE Transactions SET Dr=@Dr, Cr=@Cr, Balance=@Balance where HeadName='" + ddName.SelectedValue + "' and Description='Openning Balance'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.Add("@Dr", SqlDbType.Decimal).Value = debit;
                cmd2.Parameters.Add("@Cr", SqlDbType.Decimal).Value = credit;
                cmd2.Parameters.Add("@Balance", SqlDbType.Decimal).Value = debit - credit;
                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
            }
        }
        else
        {
            lblMsg.Text = "Please Enter the Name of the Party";
        }
    }
    private void Memberinfo()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd7 = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        int prjId = Convert.ToInt32(cmd7.ExecuteScalar());
        cmd7.Connection.Close();

        if (txtName.Text != "")
        {
            //Create Sql Connection
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

            //Create Sql Command
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO Party (VarSupplierName, VarOrganizationName, VarSupplierAddress, varSupplierType, VarEmailAddress, VarPhoneNumber, OpBalance, fltCreditLimit, VarZone, ProjectID, EntryBy)" +
                                    "VALUES (@Name, @Company, @Address, @Country, @Email, @Mobile, @OpBalance, @Credit, @VarZone, @ProjectID, @EntryBy)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            //Parameter array Declaration
            SqlParameter[] param = new SqlParameter[9];

            param[0] = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
            param[1] = new SqlParameter("@Company", SqlDbType.NVarChar, 50);
            param[2] = new SqlParameter("@Address", SqlDbType.NVarChar, 300);
            param[3] = new SqlParameter("@Country", SqlDbType.NVarChar, 50);
            param[4] = new SqlParameter("@Email", SqlDbType.NVarChar, 50);
            param[5] = new SqlParameter("@Mobile", SqlDbType.NVarChar, 50);
            param[6] = new SqlParameter("@OpBalance", SqlDbType.Decimal);
            param[7] = new SqlParameter("@Credit", SqlDbType.Decimal);
            param[8] = new SqlParameter("@VarZone", SqlDbType.NVarChar, 50);


            param[0].Value = txtName.Text;
            param[1].Value = txtCompany.Text;
            param[2].Value = txtAddress.Text;
            param[3].Value = txtCountry.SelectedValue;
            param[4].Value = txtEmail.Text;
            param[5].Value = txtMobile.Text;
            param[6].Value = txtBalance.Text;
            param[7].Value = txtCredit.Text;
            param[8].Value = ddArea.SelectedValue;
            
            cmd.Parameters.AddWithValue("@ProjectID", prjId);
            cmd.Parameters.AddWithValue("@EntryBy", lName);

            /*** Looping Fields ***/
            for (int i = 0; i < param.Length; i++)
            {
                cmd.Parameters.Add(param[i]);
            }

            /*** Command Execution ***/

            cnn.Open();
            int Success = cmd.ExecuteNonQuery();
            cnn.Close();

            if (Success > 0)
            {
                //vendor Part
                decimal debit = 0; decimal credit = 0;
                if (txtCountry.SelectedValue == "C")
                {
                    debit = Convert.ToDecimal(txtBalance.Text);
                }
                else
                {
                    credit = Convert.ToDecimal(txtBalance.Text);
                }

                SqlCommand cmd2 = new SqlCommand("INSERT INTO Transactions (HeadName, Description, Dr, Cr, Balance,  EntryBy, InvNo)" +
                                            "VALUES (@HeadName, @Description, @Dr, @Cr, @Balance, @EntryBy, @InvNo)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = txtName.Text;
                cmd2.Parameters.Add("@Description", SqlDbType.VarChar).Value = "Openning Balance";
                cmd2.Parameters.Add("@Dr", SqlDbType.Decimal).Value = debit;
                cmd2.Parameters.Add("@Cr", SqlDbType.Decimal).Value = credit;
                cmd2.Parameters.Add("@Balance", SqlDbType.Decimal).Value = debit - credit;
                cmd2.Parameters.Add("@EntryBy", SqlDbType.VarChar).Value = lName;
                cmd2.Parameters.Add("@InvNo", SqlDbType.VarChar).Value = "na";

                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();

                lblMsg.Text = "New Party Successfully Added";
                ClearControls(Page);
                txtBalance.Text = "0"; txtCredit.Text = "0";
                GridView1.DataBind();
                //Photo.ImageUrl = "~/ShowPhoto.ashx?id=" + (param[2].Value);
            }

        }
        else
        {

            lblMsg.Text = "Please Enter the Name of the Party";
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Button1.Text == "Update" && ddName.Visible == true)
        {
            try
            {
                UpdateInfo();
                MessageBox("Party info successfully updated");
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
                MessageBox("New Party Setup is done.");
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }
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

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        txtName.Visible = false;
        ddName.Visible = true;
        Button1.Text = "Update";
        generateData();

    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        txtName.Visible = false;
        ddName.Visible = true;
        Button1.Text = "Update";
        generateData();
    }
    private void generateData()
    {
        SqlCommand cmd = new SqlCommand("SELECT VarOrganizationName, VarSupplierAddress, VarEmailAddress, VarPhoneNumber, varSupplierType, fltCreditLimit, OpBalance, VarZone FROM  Party WHERE (VarSupplierName = @InvNo)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.Parameters.Add("@InvNo", SqlDbType.VarChar).Value = ddName.SelectedValue;

        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            txtCompany.Text = dr[0].ToString();
            txtAddress.Text = dr[1].ToString();
            txtEmail.Text = dr[2].ToString();
            txtMobile.Text = dr[3].ToString();
            string pType = dr[4].ToString();
            if (pType == "C")
            { txtCountry.SelectedValue = "C"; }
            else
            { txtCountry.SelectedValue = "S"; }
            txtCredit.Text = dr[5].ToString();
            txtBalance.Text = dr[6].ToString();
            ddArea.SelectedValue = dr[7].ToString();
        }
        cmd.Connection.Close();
        cmd.Connection.Dispose();

    }


    protected void ddName_SelectedIndexChanged(object sender, EventArgs e)
    {
        generateData();
    }
}



