using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Exp_Types : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //txtDept.Text = "Drinks3";
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();

        if (!IsPostBack)
        {
            EditField.Attributes.Add("class", "form-group hidden");
            divSerial.Attributes.Add("class", "form-group hidden");
        }

    }
    private void ExecuteInsert()
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            int prjId = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Connection.Close();

            int displaySl = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select ISNULL(Max(DisplaySl),0) from ExpTypes")) + 10;

            SqlCommand cmd2 = new SqlCommand("INSERT INTO ExpTypes (ExpTypeName, Company, DisplaySl, ProjectID, EntryBy) VALUES (@GroupName, @Description,'" + displaySl + "' , @ProjectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.AddWithValue("@GroupName", txtDept.Text);
            cmd2.Parameters.AddWithValue("@Description", txtDesc.Text);
            cmd2.Parameters.AddWithValue("@ProjectID", prjId);
            cmd2.Parameters.AddWithValue("@EntryBy", lName);

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();

            lblMsg.Attributes.Add("class", "xerp_success");
            lblMsg.Text = "New Item Added Successfully";
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error in Save: " + ex.Message.ToString();
        }
        finally
        {
            GridView1.DataBind();
            txtDept.Text = "";
            txtDesc.Text = "";
        }

    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        if (txtDept.Text != "")
        {
            if (btnSave.Text == "Save")
            {
                ExecuteInsert();
                EditField.Attributes.Add("class", "form-group hidden");
                divSerial.Attributes.Add("class", "form-group hidden");
                txtDept.Focus();
            }
            else
            {
                ExecuteUpdate();

                txtDept.Text = "";
                txtDesc.Text = "";
                btnSave.Text = "Save";
                EditField.Attributes.Add("class", "form-group hidden");
                divSerial.Attributes.Add("class", "form-group hidden");
                GridView1.DataBind();
                DropDownList1.DataBind();
                pnl.Update();

                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Item Info Updated Successfully";
                MessageBox("Item Info Updated Successfully");
            }
        }
        else
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Please input Item Name";
        }
    }

    private void ExecuteUpdate()
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE ExpTypes SET ExpTypeName='" + txtDept.Text + "', Company='" + txtDesc.Text + "', DisplaySl='" + txtSerial.Text + "' where (ExpTypeName ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
        /*
        SqlCommand cmd = new SqlCommand("UPDATE Items SET CategoryName='" + txtDept.Text + "', GroupName='" + ddCategory.SelectedValue + "'  where (CategoryName ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();
         * */
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
        SqlCommand cmd7 = new SqlCommand("SELECT [ExpTypeName], [Company], DisplaySl FROM [ExpTypes] WHERE ExpTypeName=@ExpTypeName", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Parameters.Add("@ExpTypeName", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            EditField.Attributes.Add("class", "form-group");
            divSerial.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            txtDept.Text = dr[0].ToString();
            txtDesc.Text = dr[1].ToString();
            txtSerial.Text = dr[2].ToString();
        }
        cmd7.Connection.Close();
    }


    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        EditMode();
    }

    private void CancelForm()
    {
        txtDept.Text = "";
        txtDesc.Text = "";
        GridView1.DataBind();
        DropDownList1.DataBind();
        GridView1.EditIndex = -1;
        EditField.Attributes.Add("class", "form-group hidden");
        divSerial.Attributes.Add("class", "form-group hidden");
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
        MessageBox("Pls Update info from left panel.");
        txtDept.Focus();
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


    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            string isExist = SQLQuery.ReturnString("Select TOP(1) AccHeadID FROM ExpenseHeads WHERE AccHeadID='" + lblItemCode.Text + "'");

            if (isExist == "")
            {
                SqlCommand cmd7 = new SqlCommand("DELETE ExpTypes WHERE ExpTypesID=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                cmd7.ExecuteNonQuery();
                cmd7.Connection.Close();
                Notify("Delete command executed successfully.", "warn", lblMsg);
            }
            else
            {
                Notify("ERROR: The location has existing sales record!", "error", lblMsg);
            }

            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }
}
