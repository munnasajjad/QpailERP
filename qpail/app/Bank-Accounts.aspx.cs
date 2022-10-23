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
using RunQuery;

public partial class app_Bank_Accounts : System.Web.UI.Page
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
            EditField.Attributes.Add("class", "form-group hidden");
            PopulateDropDowns();
        }
    }

    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox) { (Parent as TextBox).Text = string.Empty; } else { foreach (Control c in Parent.Controls) ClearControls(c); }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtName.Text != "" && ddType.SelectedValue != "" && ddBank.SelectedValue != "" && ddZone.SelectedValue != "")
            {
                SqlCommand cmde = new SqlCommand("SELECT ACNo FROM BankAccounts WHERE  ACNo ='" + txtName.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmde.Connection.Open();
                string isExist
                    = Convert.ToString(cmde.ExecuteScalar());
                cmde.Connection.Close();

                if (isExist == "")
                {
                    if (btnSave.Text == "Save")
                    {
                        ExecuteInsert();
                        EditField.Attributes.Add("class", "form-group hidden");

                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "New account added successfully";
                    }
                    else
                    {
                        ExecuteUpdate();
                        ClearControls(Form);
                        btnSave.Text = "Save";
                        EditField.Attributes.Add("class", "form-group hidden");

                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Account updated successfully";
                    }
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Error: Account No. already exist!";
                }
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Please fillup all mendatory fields...";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error: " + ex.Message.ToString();
        }
        finally
        {
            GridView1.DataBind();
            DropDownList1.DataBind();
            txtName.Focus();
        }
    }
    private void ExecuteInsert()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        int prjId = Convert.ToInt32(cmd.ExecuteScalar());
        cmd.Connection.Close();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO BankAccounts (TypeID, ACName, ACNo, BankID, ZoneID, Address, Email, ContactNo, OpBalance, ProjectID, EntryBy) VALUES (@TypeID, @ACName, @ACNo, @BankID, @ZoneID, @Address, @Email, @ContactNo, @OpBalance, @ProjectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@TypeID", ddType.SelectedValue);
        cmd2.Parameters.AddWithValue("@ACName", txtName.Text);
        cmd2.Parameters.AddWithValue("@ACNo", txtACno.Text);
        cmd2.Parameters.AddWithValue("@BankID", ddBank.SelectedValue);

        cmd2.Parameters.AddWithValue("@ZoneID", ddZone.SelectedValue);
        cmd2.Parameters.AddWithValue("@Address", txtAddress.Text);
        cmd2.Parameters.AddWithValue("@Email", txtEmail.Text);
        cmd2.Parameters.AddWithValue("@ContactNo", txtMobile.Text);

        if (txtBalance.Text=="")
        {
            txtBalance.Text = "0";
        }

        cmd2.Parameters.AddWithValue("@OpBalance", txtBalance.Text);
        cmd2.Parameters.AddWithValue("@ProjectID", lblProject.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE BankAccounts SET TypeID='" + ddType.SelectedValue + "', ACName='" + txtName.Text + "', ACNo='" + txtACno.Text + "'," +
            " BankID='" + ddBank.SelectedValue + "', ZoneID='" + ddZone.SelectedValue + "', Address='" + txtAddress.Text + "', Email='" + txtEmail.Text + "', ContactNo='" + txtMobile.Text + "'," +
            "OpBalance='" + txtBalance.Text + "' where (ACID ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();

        //SqlCommand cmd = new SqlCommand("UPDATE Items SET ItemType='" + txtName.Text + "', GroupID='" + ddGroup.SelectedValue + "'  where (ItemType ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmd.Connection.Open();
        //cmd.ExecuteNonQuery();
        //cmd.Connection.Close();
        //cmd.Connection.Dispose();
    }

    //get & bind dropdownlists
    private void PopulateDropDowns()
    {
        //string gQuery = "SELECT GroupSrNo,[GroupName] FROM [ItemGroup] Where ProjectID='" + lblProject.Text + "' ORDER BY [GroupSrNo]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGroup, "GroupSrNo", "GroupName");

        //gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddBank, "CategoryID", "CategoryName");

        //gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddBank.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

        //gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddZone, "CategoryID", "CategoryName");
    }

    protected void btnClear_Click1(object sender, EventArgs e)
    {
        CancelForm();
    }


    private void EditMode()
    {
        string itemToEdit = DropDownList1.SelectedValue;

        //string catID = RunQuery.SQLQuery.ReturnString("SELECT CategoryID FROM BankAccounts WHERE ProductID ='" + itemToEdit + "'");
        //string grdID = RunQuery.SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
        //string subID = RunQuery.SQLQuery.ReturnString("SELECT CategoryID FROM ItemGrade WHERE GradeID ='" + grdID + "'");
        //string grpID = RunQuery.SQLQuery.ReturnString("SELECT GroupID FROM ItemSubGroup WHERE CategoryID ='" + subID + "'");

        //ddGroup.SelectedValue = grpID;

        //string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + grpID + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddBank, "CategoryID", "CategoryName");
        //ddBank.SelectedValue = subID;

        //gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + subID + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
        //ddGrade.SelectedValue = grdID;

        //gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + grdID + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //RunQuery.SQLQuery.PopulateDropDown(gQuery, ddZone, "CategoryID", "CategoryName");
        //ddZone.SelectedValue = catID;


        SqlCommand cmd7 = new SqlCommand("SELECT TypeID, ACName, ACNo, BankID, ZoneID, Address, Email, ContactNo, OpBalance FROM [BankAccounts] WHERE ACID='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            ddType.SelectedValue = dr[0].ToString();
            txtName.Text = dr[1].ToString();
            txtACno.Text = dr[2].ToString();
            ddBank.SelectedValue = dr[3].ToString();
            ddZone.SelectedValue = dr[4].ToString();
            txtAddress.Text = dr[5].ToString();
            txtEmail.Text = dr[6].ToString();
            txtMobile.Text = dr[7].ToString();            
            txtBalance.Text = dr[8].ToString();
        }
        cmd7.Connection.Close();
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Editing item changed!";
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
        try
        {
            DropDownList1.DataBind();
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
            DropDownList1.SelectedValue = lblItemName.Text;
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
    
}
