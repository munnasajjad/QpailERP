using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_BankBranch : System.Web.UI.Page
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
            //EditField.Attributes.Add("class", "form-group hidden");
            ddBank.DataBind();
        }
    }
    private void ExecuteInsert()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        int prjId = Convert.ToInt32(cmd.ExecuteScalar());
        cmd.Connection.Close();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO BankBranch (BankID, BranchName, BranchAddress, Phone, EntryBy) " +
                                         "VALUES (@BankID, @BranchName, @BranchAddress, @Phone, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Parameters.AddWithValue("@BankID", ddBank.SelectedValue);
        cmd2.Parameters.AddWithValue("@BranchName", txtBranch.Text);
        cmd2.Parameters.AddWithValue("@BranchAddress", txtAddress.Text);
        cmd2.Parameters.AddWithValue("@Phone", txtPhone.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        try
        {
            if (txtBranch.Text != "")
            {
                if (btnSave.Text == "Save")
                {

                    SqlCommand cmde = new SqlCommand("SELECT BranchName FROM BankBranch WHERE BankID ='" + ddBank.SelectedValue + "' AND  BranchName ='" + txtBranch.Text.TrimStart().Trim() + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmde.Connection.Open();
                    string isExist = Convert.ToString(cmde.ExecuteScalar());
                    cmde.Connection.Close();

                    if (isExist == "")
                    {
                        ExecuteInsert();
                        //EditField.Attributes.Add("class", "form-group hidden");
                        txtBranch.Focus();
                        ClearForm();
                        Notify("Saved Successfully", "success", lblMsg);
                        lblMsg.Text = "New Branch Added Successfully";
                    }
                    else
                    {
                        Notify("Error: Branch name already exist!", "warn", lblMsg);
                    }

                }
                else
                {
                    SqlCommand cmde = new SqlCommand("SELECT BranchName FROM BankBranch WHERE BankID ='" + ddBank.SelectedValue + "' AND BranchID <>'" + BranchIdHField.Value + "' AND  BranchName ='" + txtBranch.Text.TrimStart().Trim() + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmde.Connection.Open();
                    string isExist = Convert.ToString(cmde.ExecuteScalar());
                    cmde.Connection.Close();

                    if (isExist == "")
                    {

                        ExecuteUpdate();

                        btnSave.Text = "Save";
                        //EditField.Attributes.Add("class", "form-group hidden");
                        ClearForm();
                        Notify("Updated Successfully", "success", lblMsg);
                        lblMsg.Text = "Branch Name Updated Successfully";
                    }
                    else
                    {
                        Notify("Error: Branch name already exist!", "warn", lblMsg);
                    }
                }

                GridView1.DataBind();
                //DropDownList1.DataBind();

            }
            else
            {
                Notify("Input Brnch name", "error", lblMsg);
            }

        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
        finally
        {
            GridView1.DataBind();
            txtBranch.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
        }
    }

    private void ExecuteUpdate()
    {                                                   //BranchID, BankID, BranchName, BranchAddress, Phone, EntryBy, EntryDate FROM            BankBranch
        SqlCommand cmd2 = new SqlCommand("UPDATE BankBranch SET BranchName='" + txtBranch.Text + "', BranchAddress='" + txtAddress.Text + "' , Phone='" + txtPhone.Text + "', BankID='" + ddBank.SelectedValue + "' where (BranchID ='" + BranchIdHField.Value + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();

        //SqlCommand cmd = new SqlCommand("UPDATE Items SET BranchName='" + txtBranch.Text + "', CustomerID='" + ddBank.SelectedValue + "'  where (BranchName ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
        BranchIdHField.Value = lblItemName.Text;

        //GridView1.Columns[-1].Visible = false;

        EditMode();
    }


    private void EditMode()
    {
        SqlCommand cmd7 = new SqlCommand("SELECT BankID, BranchName, BranchAddress, Phone, EntryBy FROM BankBranch WHERE BranchID='" + BranchIdHField.Value + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            ddBank.SelectedValue = dr["BankID"].ToString();
            //EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            txtBranch.Text = dr["BranchName"].ToString();
            txtAddress.Text = dr["BranchAddress"].ToString();
            txtPhone.Text = dr["Phone"].ToString();
            
        }
        cmd7.Connection.Close();
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        EditMode();
    }

    private void CancelForm()
    {
        txtBranch.Text = "";
        txtAddress.Text = "";
        txtPhone.Text = "";
        GridView1.DataBind();
        //DropDownList1.DataBind();
        GridView1.EditIndex = -1;
        //EditField.Attributes.Add("class", "form-group hidden");
        btnSave.Text = "Save";
        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Action Cancelled!";
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        CancelForm();
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        txtBranch.Focus();
    }


    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        BranchIdHField.Value = lblItemName.Text;
        EditMode();
    }
    protected void ddBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            int entryCount = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("SELECT COUNT(BranchId) FROM LC WHERE BranchId='" + lblItemCode.Text + "'"));

            if (entryCount <= 0)
            {
                RunQuery.SQLQuery.ExecNonQry("DELETE BankBranch WHERE BranchID=" + lblItemCode.Text);
                ClearForm();

                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "Branch deleted successfully ...";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "<b>ERROR: </b>Branch is Locked due to use in LC...";
            }

            
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }

    }
    private void ClearForm()
    {
        txtBranch.Text = "";
        txtAddress.Text = "";
        txtPhone.Text = "";
        GridView1.DataBind();
    }
}
