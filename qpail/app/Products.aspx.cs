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


public partial class AdminCentral_Products : System.Web.UI.Page
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

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        try
        {
           
       
       
            if (txtName.Text != "" && ddGrade.SelectedValue != "" && ddSubGrp.SelectedValue != "" && ddcategory.SelectedValue != "")
            {
                if (btnSave.Text == "Save")
                {
                    SQLQuery.Empty2Zero(txtQty);
                    SqlCommand cmde = new SqlCommand("SELECT ItemName FROM Products WHERE CategoryID ='" + ddcategory.SelectedValue + "' AND  ItemName ='" + txtName.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmde.Connection.Open();
                    string isExist = Convert.ToString(cmde.ExecuteScalar());
                    cmde.Connection.Close();

                    if (isExist == "")
                    {
                        ExecuteInsert();
                        EditField.Attributes.Add("class", "form-group hidden");
                        txtName.Text = "";
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "New product added successfully";
                    }
                    else
                    {
                        lblMsg.Attributes.Add("class", "xerp_error");
                        lblMsg.Text = "Error: Item already exist!";
                    }
                }
                else
                {
                    ExecuteUpdate();
                    ClearControls(Form);
                    btnSave.Text = "Save";
                    EditField.Attributes.Add("class", "form-group hidden");

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Item updated successfully";
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
        SQLQuery.Empty2Zero(txtQty);
      
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        int prjId = Convert.ToInt32(cmd.ExecuteScalar());
        cmd.Connection.Close();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO Products (CategoryID, ItemName, UnitType, Description, Weight, ProjectID, EntryBy,AccountHead,Depreciation) VALUES (@GroupID, @GradeName, @UnitType, @Description, @Weight, @ProjectID, @EntryBy,@AccountHead,@Depreciation)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@GroupID", ddcategory.SelectedValue);
        cmd2.Parameters.AddWithValue("@GradeName", txtName.Text);
        cmd2.Parameters.AddWithValue("@UnitType", ddUnit.SelectedValue);
        cmd2.Parameters.AddWithValue("@Description", txtDesc.Text);
        cmd2.Parameters.AddWithValue("@Weight", "0");//txtWeight.Text
        cmd2.Parameters.AddWithValue("@ProjectID", prjId);
     
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@AccountHead", ddAccHead.SelectedValue);
        cmd2.Parameters.AddWithValue("@Depreciation", txtQty.Text);
  
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE Products SET ItemName='" + txtName.Text + "', UnitType='" + ddUnit.SelectedValue + "', Description='" + txtDesc.Text + "', CategoryID='" + ddcategory.SelectedValue + "', Weight='"+txtWeight.Text+ "',AccountHead='"+ ddAccHead.SelectedValue + "',Depreciation='" + txtQty.Text+"' WHERE (ProductID ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();

        //SqlCommand cmd = new SqlCommand("UPDATE Items SET ItemType='" + txtName.Text + "', GroupID='" + ddGroup.SelectedValue + "'  WHERE (ItemType ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmd.Connection.Open();
        //cmd.ExecuteNonQuery();
        //cmd.Connection.Close();
        //cmd.Connection.Dispose();
    }

    //get & bind dropdownlists
    private void PopulateDropDowns()
    {
        string gQuery = "SELECT GroupSrNo,[GroupName] FROM [ItemGroup] WHERE GroupSrNo <>'11' AND ProjectID='" + lblProject.Text + "' ORDER BY [GroupSrNo]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGroup, "GroupSrNo", "GroupName");

        gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] WHERE GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddcategory, "CategoryID", "CategoryName");

        LoadDepreciationType();
    }

    protected void btnClear_Click1(object sender, EventArgs e)
    {
        CancelForm();
    }


    private void EditMode()
    {
        string itemToEdit = DropDownList1.SelectedValue;

        string catID = RunQuery.SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + itemToEdit + "'");
        string grdID = RunQuery.SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
        string subID = RunQuery.SQLQuery.ReturnString("SELECT CategoryID FROM ItemGrade WHERE GradeID ='" + grdID + "'");
        string grpID = RunQuery.SQLQuery.ReturnString("SELECT GroupID FROM ItemSubGroup WHERE CategoryID ='" + subID + "'");

        ddGroup.SelectedValue = grpID;

        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] WHERE GroupID='" + grpID + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");
        ddSubGrp.SelectedValue = subID;

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + subID + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
        ddGrade.SelectedValue = grdID;

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + grdID + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddcategory, "CategoryID", "CategoryName");
        ddcategory.SelectedValue = catID;


        SqlCommand cmd7 = new SqlCommand("SELECT ItemName, Description, CategoryID, UnitType, Weight,AccountHead,Depreciation FROM [Products] WHERE ProductID='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            txtName.Text = dr[0].ToString();
            txtDesc.Text = dr[1].ToString();
            //ddcategory.SelectedValue = dr[2].ToString();
            ddUnit.SelectedValue = dr[3].ToString();
            txtWeight.Text= dr[4].ToString();
            try
            {
                ddAccHead.SelectedValue = dr[5].ToString();
            }
            catch (Exception ex)
            {
                ddAccHead.Items.Add(new ListItem(" --- N/A --- ","0"));
                ddAccHead.SelectedValue = "0";
            }

            txtQty.Text = dr[6].ToString();
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
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }
    protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] WHERE GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddcategory, "CategoryID", "CategoryName");

        gQuery = "SELECT HeadSetup.AccountsHeadID, HeadSetup.AccountsHeadName FROM HeadSetup INNER JOIN ItemGroup ON HeadSetup.ControlAccountsID = ItemGroup.ControlAccountsID WHERE ItemGroup.GroupSrNo = '" + ddGroup.SelectedValue + "'";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddAccHead, "AccountsHeadID", "AccountsHeadName");
        

        //SQLQuery.PopulateDropDown(@"SELECT ControlAccount.ControlAccountsID, (SELECT AccountsName FROM Accounts WHERE (AccountsID = ControlAccount.AccountsID)) + ' > ' + ControlAccount.ControlAccountsName AS name
        //FROM ControlAccount INNER JOIN ItemGroup ON ControlAccount.sl = ItemGroup.ControlAccountsID
        //ORDER BY ControlAccount.ControlAccountsID", ddAccHead, "ControlAccountsID", "name");

        LoadDepreciationType();
    }
    private void LoadDepreciationType()
    {
        int depreciTypeId = int.Parse(SQLQuery.ReturnString("SELECT DepreciationType FROM ItemGroup WHERE GroupSrNo='" + ddGroup.SelectedValue + "'"));
        if (depreciTypeId == 0)
        {
            ltrDepType.Text = "N/A";
            divDepreciation.Visible = false;
           
        }
        else if (depreciTypeId == 1)
        {
            ltrDepType.Text = "Yearly";
            ltrDepType.Visible = true;
            divDepreciation.Visible = true;
        }
        else if (depreciTypeId == 2)
        {
            ltrDepType.Text = "Yearly";
            ltrDepType.Visible = true;
            divDepreciation.Visible = true;
        }
    }
    protected void ddSubGrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] WHERE CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddcategory, "CategoryID", "CategoryName");
    }
    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddcategory, "CategoryID", "CategoryName");
    }
    protected void ddcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            string isExist = RunQuery.SQLQuery.ReturnString("Select ProductName FROM Stock WHERE ProductID='" + lblItemCode.Text + "'");

            if (isExist == "")
            {
                SqlCommand cmd7 = new SqlCommand("DELETE Products WHERE ProductID=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                cmd7.ExecuteNonQuery();
                cmd7.Connection.Close();

                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "Entry deleted successfully ...";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR:  <b>" + isExist + "</b> is already exist into stock!";
            }

            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    protected void ddAccHeadDrUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }
}
