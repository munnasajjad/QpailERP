using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class app_Item_Names : System.Web.UI.Page
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
        }

    }
    private void ExecuteInsert()
    {
        try
        {
            string iName = txtName.Text.Trim();
            //iName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(iName.ToLower());

            string iCode = ""; //txtCode.Text;
            //if (chkCode.Checked == true)
            //{
                SqlCommand cmd7 = new SqlCommand("SELECT CONVERT(VARCHAR(10), ISNULL(MAX(id),1)) FROM Items", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                string ic = Convert.ToString(cmd7.ExecuteScalar());
                cmd7.Connection.Close();

                iCode = iName.Substring(0, 1).ToUpper() + ic;
            //}

            if (iCode != "")
            {
                string lName = Page.User.Identity.Name.ToString();
                SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Connection.Open();
                int prjId = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Connection.Close();

                //Check if item name already exists
                SqlCommand cmd1 = new SqlCommand("SELECT ItemCode FROM Items WHERE name ='" + txtName.Text + "' AND ProjectID=" + prjId, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd1.Connection.Open();
                string dupFind = Convert.ToString(cmd1.ExecuteScalar());
                cmd1.Connection.Close();

                if (dupFind == "")
                {
                    SqlCommand cmd2 = new SqlCommand("INSERT INTO Items (ItemType, ItemCode, GroupName, CategoryName,Brand, UnitType, name, price, ProjectID, EntryBy, IsActive) VALUES (@ItemType, @ItemCode, @GroupName, @CategoryName,@Brand, @UnitType, @name, @price, @ProjectID, @EntryBy, @IsActive)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    
                    cmd2.Parameters.AddWithValue("@GroupName", ddGroup.SelectedValue);
                    cmd2.Parameters.AddWithValue("@ItemType", ddSubGrp.SelectedValue);
                    cmd2.Parameters.AddWithValue("@CategoryName", ddCategory.SelectedValue);
                    cmd2.Parameters.AddWithValue("@Brand", " ");
                    cmd2.Parameters.AddWithValue("@UnitType", ddUnit.SelectedValue);
                    cmd2.Parameters.AddWithValue("@ItemCode", iCode);
                    cmd2.Parameters.AddWithValue("@name", iName);
                    cmd2.Parameters.AddWithValue("@price", "0");
                    cmd2.Parameters.AddWithValue("@ProjectID", prjId);
                    cmd2.Parameters.AddWithValue("@EntryBy", lName);

                    string isActive  = "True";                    
                    cmd2.Parameters.AddWithValue("@IsActive", isActive);

                    cmd2.Connection.Open();
                    cmd2.ExecuteNonQuery();
                    cmd2.Connection.Close();

                    txtName.Focus();
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "New Item Group Added Successfully";
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "This Item Already Exist in Database!";
                    MessageBox("This Item Already Exist in Database!");
                }
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Invalid Item Code!";
                MessageBox("Invalid Item Code!");
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in Save: " + ex.Message.ToString();
        }
        finally
        {
            GridView1.DataBind();
            txtName.Text = "";
        }

    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        try
        {
            if (txtName.Text != "")
            {
                if (btnSave.Text == "Save")
                {
                    ExecuteInsert();
                    EditField.Attributes.Add("class", "form-group hidden");
                    txtName.Focus();
                }
                else
                {
                    ExecuteUpdate();

                    txtName.Text = "";
                    txtDesc.Text = "";
                    btnSave.Text = "Save";
                    EditField.Attributes.Add("class", "form-group hidden");

                    GridView1.DataBind();
                    DropDownList1.DataBind();

                    lblMsg.Text = "Item Info Updated Successfully";
                    lblMsg.Attributes.Add("class", "xerp_success");
                    MessageBox("Item Info Updated Successfully");
                }
            }
            else
            {
                lblMsg.Text = "Please input Item Name";
                lblMsg.Attributes.Add("class", "xerp_error");
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message.ToString();
            lblMsg.Attributes.Add("class", "xerp_error");
        }
    }

    private void ExecuteUpdate()
    {
        string query="UPDATE Items SET  ItemType=@ItemType, GroupName=@GroupName, CategoryName=@CategoryName, Brand=@Brand, name=@name, UnitType=@UnitType, price=@price, IsActive=@IsActive  where (name =@ItemCode) AND ProjectID=@ProjectID";
        SqlCommand cmd = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd.Parameters.AddWithValue("@ItemType", ddSubGrp.SelectedValue);
        cmd.Parameters.AddWithValue("@GroupName", ddGroup.SelectedValue);
        cmd.Parameters.AddWithValue("@CategoryName", ddCategory.SelectedValue);
        cmd.Parameters.AddWithValue("@Brand", "");
        cmd.Parameters.AddWithValue("@name", txtName.Text);
        cmd.Parameters.AddWithValue("@UnitType", ddUnit.SelectedValue);
        cmd.Parameters.AddWithValue("@price", "0");

        cmd.Parameters.AddWithValue("@ItemCode", DropDownList1.SelectedValue);
        cmd.Parameters.AddWithValue("@ProjectID", lblProject.Text);

        string isActive = "True";
        
        cmd.Parameters.AddWithValue("@IsActive", isActive);

        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();
        ltrFrmName.Text = "Items Name Setup";
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

        ltrFrmName.Text = "Edit Items";
        EditMode();
    }


    private void EditMode()
    {
        SqlCommand cmd7 = new SqlCommand("SELECT  ItemType, GroupName, CategoryName, Brand, name, UnitType, price, IsActive FROM [Items] WHERE name=@ItemCode", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Parameters.Add("@ItemCode", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            ddSubGrp.SelectedValue = dr[0].ToString();
            ddGroup.SelectedValue = dr[1].ToString();
            ddCategory.DataBind();
            ddCategory.SelectedValue = dr[2].ToString();
            //ddBrand.SelectedValue = dr[3].ToString();
            txtName.Text = dr[4].ToString();
            ddUnit.SelectedValue = dr[5].ToString();
            //txtAmount.Text = dr[6].ToString();
            string isActive = dr[7].ToString();

            lblMsg.Attributes.Add("class", "xerp_info");
            lblMsg.Text = "Item Loaded in edit mode.";
            EditField.Attributes.Remove("class");
            EditField.Attributes.Add("class", "form-group");

            GridView1.DataBind();
            pnl.Update();
        }
        cmd7.Connection.Close();
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        EditMode();
    }

    private void CancelForm()
    {
        txtName.Text = "";
        txtDesc.Text = "";
        GridView1.DataBind();
        DropDownList1.DataBind();
        GridView1.EditIndex = -1;
        EditField.Attributes.Add("class", "form-group hidden");
        btnSave.Text = "Save";
        ltrFrmName.Text = "Items Name Setup";
        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Action Cancelled!";
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        CancelForm();
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblMsg.Text="Pls Update info from left panel.";
        lblMsg.Attributes.Add("class", "xerp_error");
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
        
        ltrFrmName.Text = "Edit Items";
        EditMode();
    }
}
