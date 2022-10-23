using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using RunQuery;

public partial class app_Customer_Delivery_Points : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //txtLocation.Text = "Drinks3";
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();

        if (!IsPostBack)
        {
            EditField.Attributes.Add("class", "form-group hidden");
            ddCompany.DataBind();
            GridView1.DataBind();
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

    private void ExecuteInsert()
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            int prjId = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Connection.Close();

            SqlCommand cmd2 = new SqlCommand("INSERT INTO DeliveryPoints (CustomerID, DeliveryPointName, ZoneID, Address, PhoneNo, Email, Contactperson, MobileNo, VehicleFare, projectID, EntryBy) VALUES (@CustomerID, @DeliveryPointName, @ZoneID, @Address, @PhoneNo, @Email, @Contactperson, @MobileNo, @VehicleFare, @projectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.AddWithValue("@CustomerID", ddCompany.SelectedValue);
            cmd2.Parameters.AddWithValue("@DeliveryPointName", txtLocation.Text);
            cmd2.Parameters.AddWithValue("@ZoneID", ddZone.SelectedValue);
            cmd2.Parameters.AddWithValue("@Address", txtAddress.Text);
            cmd2.Parameters.AddWithValue("@PhoneNo", txtLandPhone.Text);

            cmd2.Parameters.AddWithValue("@Email", txtEmail.Text);
            cmd2.Parameters.AddWithValue("@Contactperson", txtName.Text);
            cmd2.Parameters.AddWithValue("@MobileNo", txtMobile.Text);
            cmd2.Parameters.AddWithValue("@VehicleFare", Convert.ToInt32(txtFare.Text));

            cmd2.Parameters.AddWithValue("@ProjectID", prjId);
            cmd2.Parameters.AddWithValue("@EntryBy", lName);

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error in Save: " + ex.Message.ToString();
        }
        finally
        {
            GridView1.DataBind();
            ClearControls(Form);
        }

    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        if (txtLocation.Text != "")
        {
            if (btnSave.Text == "Save")
            {
                //Check if name already exists
                SqlCommand cmd1 = new SqlCommand("SELECT DeliveryPointName FROM DeliveryPoints WHERE DeliveryPointName ='" + txtLocation.Text + "' AND CustomerID='"+ddCompany.SelectedValue+"' AND ProjectID=" + lblProject.Text, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd1.Connection.Open();
                string dupFind = Convert.ToString(cmd1.ExecuteScalar());
                cmd1.Connection.Close();

                if (dupFind == "")
                {
                    ExecuteInsert();
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "New Delivery Point Added Successfully for " + ddCompany.SelectedItem.Text;

                    GridView1.DataBind();
                    DropDownList1.DataBind();
                    ClearControls(Form);
                    txtLocation.Focus();
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Location already exist!";
                }
            }
            else
            {
                ExecuteUpdate();

                txtLocation.Text = "";
                txtAddress.Text = "";
                btnSave.Text = "Save";
                EditField.Attributes.Add("class", "form-group hidden");

                GridView1.DataBind();
                DropDownList1.DataBind();

                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Info Updated Successfully";
                //MessageBox("Info Updated Successfully");
            }
        }
        else
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Please input Delivery Point!";
        }
    }

    private void ExecuteUpdate()
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE DeliveryPoints SET CustomerID='" + ddCompany.SelectedValue + "', DeliveryPointName='" + txtLocation.Text + "', ZoneID='" + ddZone.SelectedValue + "', Address='" + txtAddress.Text + "', PhoneNo='" + txtLandPhone.Text + "', Email='" + txtEmail.Text + "', Contactperson='" + txtName.Text + "', MobileNo='" + txtMobile.Text + "', VehicleFare='" + txtFare.Text + "' where (DeliveryPointsID ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();

        //SqlCommand cmd = new SqlCommand("UPDATE Items SET CategoryName='" + txtLocation.Text + "', GroupName='" + ddCompany.SelectedValue + "'  where (CategoryName ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmd.Connection.Open();
        //cmd.ExecuteNonQuery();
        //cmd.Connection.Close();
        //cmd.Connection.Dispose();
    }


    protected void btnClear_Click1(object sender, EventArgs e)
    {
        CancelForm();
    }
    
    private void EditMode()
    {
        SqlCommand cmd7 = new SqlCommand("SELECT CustomerID, DeliveryPointName, ZoneID, Address, PhoneNo, Email, Contactperson, MobileNo, VehicleFare FROM [DeliveryPoints] WHERE DeliveryPointsID='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {           
            ddCompany.SelectedValue = dr[0].ToString();
            txtLocation.Text = dr[1].ToString();

            ddZone.SelectedValue = dr[2].ToString();
            txtAddress.Text = dr[3].ToString();
            txtLandPhone.Text = dr[4].ToString();
            txtEmail.Text = dr[5].ToString();

            txtName.Text = dr[6].ToString();
            txtMobile.Text = dr[7].ToString();
            txtFare.Text = dr[8].ToString();            
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
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        CancelForm();
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        MessageBox("Pls Update info from left panel.");
        txtLocation.Focus();
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
        EditField.Attributes.Remove("class");
        EditField.Attributes.Add("class", "form-group");
        btnSave.Text = "Update";

    }
    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            string isExist
                = SQLQuery.ReturnString("Select TOP(1) DeliveryLocation FROM Sales WHERE DeliveryLocation='" + lblItemCode.Text + "'");

            if (isExist == "")
            {
                SqlCommand cmd7 = new SqlCommand("DELETE DeliveryPoints WHERE DeliveryPointsID=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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

    protected void ddCompany_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridView1.DataBind();
        }
        catch(Exception ex) {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }
}
