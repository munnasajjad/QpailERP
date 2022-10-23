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

public partial class app_Production_Machines : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            EditField.Attributes.Add("class", "form-group hidden");
            txtDept.Focus();
            ddSection.DataBind();
            LoadMachineName();
            GridView1.DataBind();
        }

        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDept.Text != "")
            {
                if (btnSave.Text == "Save")
                {
                    ExecuteInsert();

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "New Machine Added Successfully";
                    txtDept.Focus();
                }
                else
                {
                    ExecuteUpdate();

                    txtDept.Text = "";
                    txtDesc.Text = "";
                    btnSave.Text = "Save";
                    EditField.Attributes.Add("class", "form-group hidden");

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Machine Info Updated Successfully";
                }
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Please write a Machine Name";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error in Save: " + ex.Message.ToString();
        }
        finally
        {
            GridView1.DataBind();
            DropDownList1.DataBind();
            //pnl.Update();
            txtDept.Text = "";
            txtDesc.Text = "";
        }
    }

    private void ExecuteInsert()
    {
        SqlCommand cmd2 = new SqlCommand("INSERT INTO Machines (Section, Machine, MachineName, MachineNo, ProductionPerHour, Description, ProjectID, EntryBy) VALUES ('" + ddSection.SelectedValue + "', '', '', @MachineNo, '" + txtPrdHr.Text + "', @Description, @ProjectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@MachineNo", txtDept.Text);
        cmd2.Parameters.AddWithValue("@Description", txtDesc.Text);
        cmd2.Parameters.AddWithValue("@ProjectID", lblProject.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE Machines SET Section='" + ddSection.SelectedValue + "', Machine ='', MachineName='', MachineNo='" + txtDept.Text + "', Description='" + txtDesc.Text + "', ProductionPerHour='" + txtPrdHr.Text + "' where (mid ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtDept.Text = "";
        txtDesc.Text = "";
        btnSave.Text = "Save";
        EditField.Attributes.Add("class", "form-group hidden");
        GridView1.DataBind();
        DropDownList1.DataBind();
        //pnl.Update();

        lblMsg.Attributes.Add("class", "xerp_warning");
        lblMsg.Text = "Form Action Cancelled!";
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        EditMode();
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        DropDownList1.DataBind();
        DropDownList1.SelectedValue = lblItemName.Text;
        EditMode();
    }

    private void EditMode()
    {
        SqlCommand cmd7 = new SqlCommand("SELECT [MachineNo], [Description], mid, Section, Machine, ProductionPerHour FROM [Machines] WHERE mid=@mid", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Parameters.Add("@mid", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            txtDept.Text = dr[0].ToString();
            txtDesc.Text = dr[1].ToString();
            //ddSection.SelectedValue = dr[3].ToString();
            //ddMachine.SelectedValue = dr[4].ToString();
            txtPrdHr.Text = dr[5].ToString();
        }
        cmd7.Connection.Close();
    }
    protected void ddSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadMachineName();
    }

    private void LoadMachineName()
    {
        /*string query = "Select ProductID, (ProductName+' '+ ItemSerialNo) AS  Machine FROM Stock where ProductID IN " +
                       "(Select ProductID from Products WHERE CategoryID IN " +
                       "(Select CategoryID from Categories where GradeID IN " +
                       "(Select GradeID from ItemGrade where CategoryID =" +
                       "(Select MachineLinkItemSubgroupID from Sections WHERE SID= '" + ddSection.SelectedValue + "'))))";
        */
        string query = "Select ProductID, ProductName AS  Machine FROM Stock where ProductID IN " +
                       "(Select ProductID from Products WHERE CategoryID IN " +
                       "(Select CategoryId from Sections WHERE SID= '" + ddSection.SelectedValue + "'))";

        //RunQuery.SQLQuery.PopulateDropDown(query, ddMachine, "ProductID", "Machine");
        MachineDetail();
    }

    private void MachineDetail()
    {
        //ltrLastInfo.Text = SQLQuery.ReturnString("Select top(1) ItemSerialNo from stock where ProductID = '" + ddMachine.SelectedValue + "' order by EntryID desc ");
    }

    protected void ddMachine_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        MachineDetail();
    }
}
