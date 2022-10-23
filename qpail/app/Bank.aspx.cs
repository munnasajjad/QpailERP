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


public partial class Bank : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();

        if (!IsPostBack)
        {
            string projectID = Convert.ToString(Session["ProjectID"]);
            string formtype = base.Request.QueryString["type"];
            lblProject.Text = projectID;

            if (formtype == "insurance")
            {
                ltrFrmName.Text = "Insurance Info";
                ltrEntryForm.Text = "Insurance Company Setup";
                ltrName.Text = "Company Name";
                ltrPhone.Text = "Phone No.";
                ltrAddress.Text = "Location/Address";
                Literal5.Text = "Insurance Code";
                Page.Title = "Insurance";
            }
            else if (formtype == "bank")
            {
                ltrFrmName.Text = "Banks Info";
                ltrEntryForm.Text = "New Bank Setup";
                ltrName.Text = "Bank Name";
                ltrPhone.Text = "Head-Office Phone No.";
                ltrAddress.Text = "Head-Office Address";
                Literal5.Text = "Swift Code";
                Page.Title = "Banks";
            }
            txtName.Focus();
            EditField.Attributes.Add("class", "form-group hidden");
        }

    }

    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox) { (Parent as TextBox).Text = string.Empty; } else { foreach (Control c in Parent.Controls) ClearControls(c); }
    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        try
        {
            if (txtName.Text != "")
            {
                if (btnSave.Text == "Save")
                {
                    //Check if item name already exists
                    string formtype = base.Request.QueryString["type"];
                    SqlCommand cmd1 = new SqlCommand("SELECT BankName FROM Banks WHERE BankName ='" + txtName.Text + "' AND Type='" + formtype + "' AND ProjectID=" + lblProject.Text, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmd1.Connection.Open();
                    string dupFind = Convert.ToString(cmd1.ExecuteScalar());
                    cmd1.Connection.Close();

                    if (dupFind == "")
                    {
                        ExecuteInsert();
                        EditField.Attributes.Add("class", "form-group hidden");

                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "New " + formtype + " saved successfully...";
                    }
                }
                else
                {
                    ExecuteUpdate();

                    btnSave.Text = "Save";
                    EditField.Attributes.Add("class", "form-group hidden");

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Info successfully updated for " + DropDownList1.SelectedItem.Text;
                }
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Please input name field";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error in Save: " + ex.Message.ToString();
        }
        finally
        {
            ClearControls(Form);
            GridView1.DataBind();
            DropDownList1.DataBind();
            txtName.Focus();
        }
    }

    private void ExecuteInsert()
    {
        string formtype = base.Request.QueryString["type"];
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd2 = new SqlCommand("INSERT INTO Banks (BankName, Description, Type, Phone, ContactPerson, MobileNo, Email, url, ProjectID, EntryBy, SwiftCode) VALUES (@BankName, @Description, @Type, @Phone, @ContactPerson, @MobileNo, @Email, @url, @ProjectID, @EntryBy, '"+txtSwift.Text+"')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@BankName", txtName.Text); //ddCategory.SelectedValue);
        cmd2.Parameters.AddWithValue("@Description", txtAddress.Text);

        cmd2.Parameters.AddWithValue("@ContactPerson", txtContact.Text);
        cmd2.Parameters.AddWithValue("@MobileNo", txtMobile.Text);
        cmd2.Parameters.AddWithValue("@url", txtUrl.Text);

        cmd2.Parameters.AddWithValue("@Type", formtype);
        cmd2.Parameters.AddWithValue("@Phone", txtPhone.Text);
        cmd2.Parameters.AddWithValue("@Email", txtEmail.Text);
        cmd2.Parameters.AddWithValue("@ProjectID", lblProject.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE Banks SET BankName='" + txtName.Text + "', Description='" + txtAddress.Text + "', Phone='" + txtPhone.Text + "',"+
                                "ContactPerson=@ContactPerson, MobileNo=@MobileNo, url=@url, Email='" + txtEmail.Text + "', SwiftCode='"+txtSwift.Text+"' where (BankId ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ContactPerson", txtContact.Text);
        cmd2.Parameters.AddWithValue("@MobileNo", txtMobile.Text);
        cmd2.Parameters.AddWithValue("@url", txtUrl.Text);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();

        //SqlCommand cmd = new SqlCommand("UPDATE Items SET ItemType='" + txtName.Text + "', GroupName='" + ddCategory.SelectedValue + "'  where (ItemType ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmd.Connection.Open();
        //cmd.ExecuteNonQuery();
        //cmd.Connection.Close();
        //cmd.Connection.Dispose();
    }


    protected void btnClear_Click1(object sender, EventArgs e)
    {
        CancelForm();
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        int index = Convert.ToInt32(e.NewEditIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        DropDownList1.SelectedValue = lblItemName.Text;

        //GridView1.Columns[-1].Visible = false;

        EditMode();
    }


    private void EditMode()
    {
        SqlCommand cmd7 = new SqlCommand("SELECT BankName, Description, Phone, Email, ContactPerson, MobileNo, url, SwiftCode FROM [Banks] WHERE BankId='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            EditField.Attributes.Remove("class");
            EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            txtName.Text = dr[0].ToString();
            txtAddress.Text= dr[1].ToString();
            txtPhone.Text = dr[2].ToString();
            txtEmail.Text = dr[3].ToString();

            txtContact.Text = dr[4].ToString();
            txtMobile.Text = dr[5].ToString();
            txtUrl.Text = dr[6].ToString();
            txtSwift.Text = dr[7].ToString();
        }

        cmd7.Connection.Close();
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        EditMode();
    }

    private void CancelForm()
    {
        ClearControls(Form);
        GridView1.DataBind();
        DropDownList1.DataBind();
        GridView1.EditIndex = -1;
        EditField.Attributes.Add("class", "form-group hidden");
        btnSave.Text = "Save";
        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Action Cancelled!";
        txtName.Focus();
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        CancelForm();
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        MessageBox("Pls Update info from left panel.");
        txtName.Focus();
    }


    //Messagebox For Alerts    
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }


    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        DropDownList1.SelectedValue = lblItemName.Text;
        EditMode();
    }
}
