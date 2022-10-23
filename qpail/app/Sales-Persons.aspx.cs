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
using RunQuery;

public partial class app_Sales_Persons : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //txtName.Focus();
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string projectID = SQLQuery.ReturnString("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + Page.User.Identity.Name.ToString() + "'");
            
            string formtype = base.Request.QueryString["type"];

            if (formtype == "ref")
            {
                ltrFrmName.Text = "Referrer Info";
                ltrSubFrmName.Text = "Setup a new Referrer";

                DueLimitField.Attributes.Remove("class");
                DueLimitField.Attributes.Add("class", "control-group hidden");

                DesiField.Attributes.Remove("class");
                DesiField.Attributes.Add("class", "control-group hidden");

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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string projectID = Session["ProjectID"].ToString();
            string formtype = base.Request.QueryString["type"];
            
            if (txtCredit.Text == "")
            {
                txtCredit.Text = "0";
            }
            
            if (formtype == "ref")
            {
                formtype = "ref";
                Page.Title = "Referrers";
            }
            else
            {
                formtype = "SR";
                Page.Title = "Sales Representatives";
            }

            if (btnSave.Text == "Update")
            {
                UpdateInfo(projectID, formtype);
                btnSave.Text = "Save";
                ClearControls(Page);
                Notify("Your provided data has been updated...", "success", lblMsg);
            }
            else
            {
                //Check if item name already exists
                SqlCommand cmd1 = new SqlCommand("SELECT ReferrerName FROM Referrers WHERE ReferrerName ='" + txtName.Text + "' AND ProjectID=" + projectID, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd1.Connection.Open();
                string dupFind = Convert.ToString(cmd1.ExecuteScalar());
                cmd1.Connection.Close();

                if (dupFind == "")
                {
                    if (txtName.Text != "")
                    {
                        InsertInfo(projectID, formtype);
                        Notify("Your provided data has been saved...", "success", lblMsg);     
                        ClearControls(Page);
                        }
                    else
                    {
                        lblMsg.Attributes.Add("class", "xerp_error");
                        lblMsg.Text = "Please Fillup the Name field";
                    }
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Referrer already exist!";
                }
            }

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
        }
        finally
        {
            GridView1.DataBind();
        }
    }

    private void InsertInfo(string prjId, string frmType)
    {
        string lName = Page.User.Identity.Name.ToString();

        SqlConnection cnn = new SqlConnection();
        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        SqlCommand cmd = new SqlCommand();

        cmd.CommandText = "INSERT INTO Referrers (ReferrerName, Address, PhoneNo, MobileNo, Remarks, CreditLimit, Zone, Designation, Type, projectID, EntryBy)" +
                                "VALUES (@ReferrerName, @Address, @PhoneNo, @MobileNo, @Remarks, @CreditLimit, @Zone, @Designation, @Type, @projectID, @EntryBy)";
        cmd.CommandType = CommandType.Text;
        cmd.Connection = cnn;

        cmd.Parameters.AddWithValue("@ReferrerName", txtName.Text);
        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
        cmd.Parameters.AddWithValue("@PhoneNo", txtPhone.Text);
        cmd.Parameters.AddWithValue("@MobileNo", txtMobile.Text);
        cmd.Parameters.AddWithValue("@Remarks", txtEmail.Text);
        cmd.Parameters.AddWithValue("@CreditLimit", Convert.ToDecimal(txtCredit.Text));
        cmd.Parameters.AddWithValue("@Zone", ddZone.SelectedValue);
        cmd.Parameters.AddWithValue("@Designation", txtRemark.Text);
        cmd.Parameters.AddWithValue("@Type", frmType);
        cmd.Parameters.AddWithValue("@ProjectID", prjId);
        cmd.Parameters.AddWithValue("@EntryBy", lName);

        cnn.Open();
        int Success = cmd.ExecuteNonQuery();
        cnn.Close();
    }




    private void UpdateInfo(string prjId, string frmType)
    {
        string lName = Page.User.Identity.Name.ToString();
        //if (ddName.SelectedValue != "---Select---")
        //{
            //Create Sql Connection
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

            //Create Sql Command
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"UPDATE [dbo].[Referrers] SET [ReferrerName] = @ReferrerName, [Address] = @Address,[PhoneNo] = @PhoneNo
      ,[MobileNo] = @MobileNo ,[Remarks] = @Remarks ,[CreditLimit] = @CreditLimit ,[Zone] = @Zone ,[Type] = @Type  WHERE ReferrersID=@ReferrersID";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@ReferrerName", txtName.Text);
            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
            cmd.Parameters.AddWithValue("@PhoneNo", txtPhone.Text);
            cmd.Parameters.AddWithValue("@MobileNo", txtMobile.Text);
            cmd.Parameters.AddWithValue("@Remarks", txtEmail.Text);
            cmd.Parameters.AddWithValue("@CreditLimit", Convert.ToDecimal(txtCredit.Text));
            cmd.Parameters.AddWithValue("@Zone", ddZone.SelectedValue);
            cmd.Parameters.AddWithValue("@Designation", txtRemark.Text);
            cmd.Parameters.AddWithValue("@Type", frmType);
            cmd.Parameters.AddWithValue("@ReferrersID", lblEditId.Text);

            cnn.Open();
            int Success = cmd.ExecuteNonQuery();
            cnn.Close();
        //}
    }


    private void generateData()
    {
        SqlCommand cmd = new SqlCommand("SELECT VarOrganizationName, VarSupplierAddress, VarEmailAddress, VarPhoneNumber, varSupplierType, fltCreditLimit, OpBalance FROM  Party WHERE (VarSupplierName = @InvNo)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.Parameters.Add("@InvNo", SqlDbType.VarChar).Value = ddName.SelectedValue;

        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            //txtCompany.Text = dr[0].ToString();
            txtAddress.Text = dr[1].ToString();
            txtEmail.Text = dr[2].ToString();
            txtMobile.Text = dr[3].ToString();
            txtCredit.Text = dr[5].ToString();
            //txtBalance.Text = dr[6].ToString();
        }
        cmd.Connection.Close();
        cmd.Connection.Dispose();

    }


    protected void ddName_SelectedIndexChanged(object sender, EventArgs e)
    {
        generateData();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearControls(Page);
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        lblEditId.Text= lblItemName.Text;

        SqlCommand cmd7 = new SqlCommand("SELECT ReferrerName, Address, PhoneNo, MobileNo, Remarks, CreditLimit FROM [Referrers] WHERE ReferrersID='" + lblEditId.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            txtName.Text = dr[0].ToString();
            txtAddress.Text = dr[1].ToString();
            txtPhone.Text = dr[2].ToString();

            txtMobile.Text = dr[3].ToString();
            txtRemark.Text = dr[4].ToString();
            txtCredit.Text = dr[5].ToString();
        }
        cmd7.Connection.Close();
        btnSave.Text = "Update";
        Notify("Edit mode activated ...", "info", lblMsg);
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            string isExist = SQLQuery.ReturnString("Select TOP(1) Company FROM Party WHERE Referrer='" + lblItemCode.Text + "'");

            if (isExist == "")
            {
                SqlCommand cmd7 = new SqlCommand("DELETE Referrers WHERE ReferrersID=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                cmd7.ExecuteNonQuery();
                cmd7.Connection.Close();
                Notify("Data has been deleted!", "warn", lblMsg);  
            }
            else
            {
                Notify("Cant Delete... Referrer has existing Party (" + isExist + ")!", "error", lblMsg);
            }

            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg); 
        }
    }
}



