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

public partial class app_Production_Standard : System.Web.UI.Page
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

    private void PopulateDropDowns()
    {
        ddSection.DataBind();
        ddMachine.DataBind();
        ddOperation.DataBind();

        ddSubGrp.DataBind();
        ddGrade.DataBind();
        ddcategory.DataBind();
        ddPackSize.DataBind();

        GetStdPrdn();

        //string gQuery = "SELECT GroupSrNo,[GroupName] FROM [ItemGroup] Where ProjectID='" + lblProject.Text + "' ORDER BY [GroupSrNo]";
        //SQLQuery.PopulateDropDown(gQuery, ddGroup, "GroupSrNo", "GroupName");

        //gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

        //gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        //SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

        //gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //SQLQuery.PopulateDropDown(gQuery, ddcategory, "CategoryID", "CategoryName");
    }

    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox) { (Parent as TextBox).Text = string.Empty; } else { foreach (Control c in Parent.Controls) ClearControls(c); }
    }

    private string GetStdPrdn()
    {
        string stdPrdn = "0";
        if (ddSubGrp.SelectedValue=="16") // Plastic
        {
            stdPrdn = SQLQuery.ReturnString("Select spid from ProductionStandard where Section='" + ddSection.SelectedValue +
                                      "' AND MachineNo='" + ddMachine.SelectedValue + "' AND Company='" +
                                      ddCustomer.SelectedValue + "' AND PackSize='" + ddPackSize.SelectedValue +
                                      "' AND  Operation='" + ddOperation.SelectedValue + "' ");
        }
        else
        {
            stdPrdn = SQLQuery.ReturnString("SELECT spid FROM [ProductionStandard] WHERE MachineNo='" +
                                           ddMachine.SelectedValue + "' AND ItemCategory='" +
                                           ddcategory.SelectedValue + "' AND PackSize='" + ddPackSize.SelectedValue +
                                           "'  AND Operation='" + ddOperation.SelectedValue + "'   ");
        }

        if (stdPrdn != "")
        {
            btnSave.Text = "Update";
            txtStdPrdn.Text = SQLQuery.ReturnString("Select StdPrdn from ProductionStandard where spid='" + stdPrdn + "'");
            lblEntryId.Text = stdPrdn;
        }
        else
        {
            btnSave.Text = "Save";
            lblEntryId.Text = "";
            txtStdPrdn.Text = "";
        }
        GridView1.DataBind();
        return stdPrdn;
    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        try
        {
            if (txtStdPrdn.Text != "" && ddGrade.SelectedValue != "")
            {
                    if (btnSave.Text == "Save")
                    {
                        ExecuteInsert();
                        //lblEntryId.Text = SQLQuery.ReturnString("Select MAX(spid) from ProductionStandard");
                        //ExecuteUpdate();
                        EditField.Attributes.Add("class", "form-group hidden");
                        //GetStdPrdn();
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "New data added successfully";
                    }
                    else
                    {
                        ExecuteUpdate();
                        //GetStdPrdn();\
                        btnSave.Text = "Save";
                    }
               
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Please fill-up all mendatory fields...";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error: " + ex.Message.ToString();
        }
        finally
        {
            GetStdPrdn();
            DropDownList1.DataBind();
        }
    }
    private void ExecuteInsert()
    {
        string lName = Page.User.Identity.Name.ToString();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO ProductionStandard (Section, MachineNo, Company, SubGroup, ItemGrade, ItemCategory, PackSize, Operation, StdPrdn, Remarks, EntryBy) " +
                                         "VALUES ('" + ddSection.SelectedValue + "','" + ddMachine.SelectedValue + "','" + ddCustomer.SelectedValue
                                         + "','" + ddSubGrp.SelectedValue + "','" + ddGrade.SelectedValue + "','" + ddcategory.SelectedValue + "','" + ddPackSize.SelectedValue + "','" + ddOperation.SelectedValue + "','" + txtStdPrdn.Text + "','" + txtDesc.Text + "','" + lName + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        string query = " WHERE MachineNo='" +
                                           ddMachine.SelectedValue + "' AND ItemCategory='" +
                                           ddcategory.SelectedValue + "' AND PackSize='" + ddPackSize.SelectedValue +
                                           "'  AND Operation='" + ddOperation.SelectedValue + "'   ";

        if (ddSubGrp.SelectedValue == "16") // Plastic
        {
            query = " where Section='" + ddSection.SelectedValue +
                                      "' AND MachineNo='" + ddMachine.SelectedValue + "' AND Company='" +
                                      ddCustomer.SelectedValue + "' AND PackSize='" + ddPackSize.SelectedValue +
                                      "' AND  Operation='" + ddOperation.SelectedValue + "' ";
        }

        SQLQuery.ExecNonQry("UPDATE ProductionStandard SET StdPrdn='" + txtStdPrdn.Text + "' " + query );
    }

    //get & bind dropdownlists
   

    protected void btnClear_Click1(object sender, EventArgs e)
    {
        CancelForm();
    }


    private void EditMode()
    {
        string itemToEdit = DropDownList1.SelectedValue;
        string catID = SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + itemToEdit + "'");
        string grdID = SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
        string subID = SQLQuery.ReturnString("SELECT CategoryID FROM ItemGrade WHERE GradeID ='" + grdID + "'");
        string grpID = SQLQuery.ReturnString("SELECT GroupID FROM ItemSubGroup WHERE CategoryID ='" + subID + "'");

        //ddGroup.SelectedValue = grpID;

        //string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + grpID + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");
        //ddSubGrp.SelectedValue = subID;

        //gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + subID + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        //SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
        //ddGrade.SelectedValue = grdID;

        //gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + grdID + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //SQLQuery.PopulateDropDown(gQuery, ddcategory, "CategoryID", "CategoryName");
        //ddcategory.SelectedValue = catID;


        SqlCommand cmd7 = new SqlCommand("SELECT ItemName, Description, CategoryID, UnitType, Weight FROM [Products] WHERE ProductID='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            txtStdPrdn.Text = dr[0].ToString();
            txtDesc.Text = dr[1].ToString();
            //ddcategory.SelectedValue = dr[2].ToString();
            //ddUnit.SelectedValue = dr[3].ToString();
            //txtWeight.Text = dr[4].ToString();
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
        GridView1.DataBind();
        //string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

        //gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        //SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

        //gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //SQLQuery.PopulateDropDown(gQuery, ddcategory, "CategoryID", "CategoryName");
    }
    protected void ddSubGrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
        ddGrade.DataBind();
        //string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        //SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

        //gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //SQLQuery.PopulateDropDown(gQuery, ddcategory, "CategoryID", "CategoryName");
    }
    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //SQLQuery.PopulateDropDown(gQuery, ddcategory, "CategoryID", "CategoryName");
        ddcategory.DataBind();
        GetStdPrdn();
    }
    protected void ddcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetStdPrdn();
    }

    protected void ddSection_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddMachine.DataBind();
        ddOperation.DataBind();
        GetStdPrdn();
    }

    protected void ddSubGrp_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddGrade.DataBind(); 
        ddcategory.DataBind(); GetStdPrdn();
    }

    protected void ddCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetStdPrdn();
    }

    protected void ddPackSize_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetStdPrdn();
    }

    protected void ddOperation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetStdPrdn();
    }
}
