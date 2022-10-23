using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Emp_Type : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //txtName.Text = "Drinks3";
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
                    lblId.Text = SQLQuery.ReturnString("select MAX(GroupSrNo) from EmpTypes ");
                    ExecuteUpdate();
                    Notify("Info Saved...", "success", lblMsg);
                }
                else
                {
                    ExecuteUpdate();
                    btnSave.Text = "Save";
                    Notify("Info Updated...", "success", lblMsg);
                }
                GridView1.DataBind();
            }
            else
            {
                lblMsg.Text = "Please type Item Group Name";
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
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

            string itemExist = SQLQuery.ReturnString("SELECT GroupName FROM EmpTypes WHERE GroupName ='" + txtName.Text + "'");

            if (itemExist != txtName.Text)
            {
                SqlCommand cmd2 = new SqlCommand("INSERT INTO EmpTypes (DutyGroup, GroupName, ProjectID, EntryBy)" +
                                                 " VALUES ('" + ddBasis.SelectedValue + "', @GroupName, @ProjectID, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.AddWithValue("@GroupName", txtName.Text);
                cmd2.Parameters.AddWithValue("@Description", txtDesc.Text);
                cmd2.Parameters.AddWithValue("@ProjectID", prjId);
                cmd2.Parameters.AddWithValue("@EntryBy", lName);

                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
                lblMsg.Text = "New Item Group Added Successfully";
                lblMsg.Attributes.Add("class", "xerp_success");
            }
            else
            {
                lblMsg.Text = "ERROR: Info already exist!";
                lblMsg.Attributes.Add("class", "xerp_error");
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "ERROR: " + ex.ToString();
            lblMsg.Attributes.Add("class", "xerp_error");
        }
        finally
        {
            GridView1.DataBind();
        }

    }

    private void ExecuteUpdate()
    {
        string dt = DateTime.Now.ToString("yyyy-MM-dd") + " ";

        SQLQuery.ExecNonQry("Update EmpTypes set DutyGroup='" + ddBasis.SelectedValue + "', GroupName='" +
                            txtName.Text + "', Description='" + txtDesc.Text +
                            "', MinDutyHour='" + txtMinHour.Text + "', " +
                            "GeneralDutyHour='" + txtGeneralHour.Text + "', " +
                            "CheckinTime='" + dt + txtCheckIn.Text + "', " +
                            "CheckoutTime='" + dt + txtCheckOut.Text + "', " +
                            "GraceMinutes='" + txtLateGraceMinutes.Text + "', " +
                            "Fooding='" + txtFooding.Text + "', AttnBonus ='" + txtAttnBonus.Text + "', " +
                            "IsShiftingDuty='" + CheckBoxValue(chkShift) + "'," +
                            "IsOvertimeAllowed='" + CheckBoxValue(chkOT) + "', " +
                            "IsHouseRentAllowed='" + CheckBoxValue(ChkHouseRent) + "', " +
                            "IsHolidayAllowance='" + CheckBoxValue(chkHoliday) + "' where GroupSrNo='" + lblId.Text + "'");
        txtName.Text = "";
        txtDesc.Text = ""; txtMinHour.Text = "";
        txtGeneralHour.Text = ""; txtCheckIn.Text = "";
        txtCheckOut.Text = ""; txtLateGraceMinutes.Text = "";
        txtFooding.Text = "";
        txtAttnBonus.Text = "";
        btnSave.Text = "Save";
    }

    private int CheckBoxValue(CheckBox cb)
    {
        int i = 0;
        if (cb.Checked)
        {
            i = 1;
        }
        return i;
    }
    protected void btnClear_Click1(object sender, EventArgs e)
    {
        txtName.Text = "";
        txtDesc.Text = "";
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
        //DropDownList1.SelectedValue = lblItemName.Text;
        EditMode(lblItemName.Text);

        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Edit mode activated ...";
    }

    private void EditMode(string id)
    {
        SqlCommand cmd7 = new SqlCommand("SELECT DutyGroup, GroupName, Description, MinDutyHour, GeneralDutyHour, CheckinTime, " +
                                         "CheckoutTime, GraceMinutes, Fooding, IsShiftingDuty,IsOvertimeAllowed, IsHouseRentAllowed, IsHolidayAllowance, AttnBonus  FROM [EmpTypes] WHERE GroupSrNo='" + id + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            //EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";
            lblId.Text = id;
            ddBasis.SelectedValue= dr[0].ToString();
            txtName.Text = dr[1].ToString();
            txtDesc.Text = dr[2].ToString();
            txtMinHour.Text = dr[3].ToString();
            txtGeneralHour.Text = dr[4].ToString();
            txtCheckIn.Text = dr[5].ToString().Substring(11,5);
            txtCheckOut.Text = dr[6].ToString().Substring(11, 5);
            txtLateGraceMinutes.Text = dr[7].ToString();
            txtFooding.Text = dr[8].ToString();

            setChkbox(chkShift, dr[9].ToString());
            setChkbox(chkOT, dr[10].ToString());
            setChkbox(ChkHouseRent, dr[11].ToString());
            setChkbox(chkHoliday, dr[12].ToString());

            txtAttnBonus.Text= dr[13].ToString();
        }
        cmd7.Connection.Close();
    }

    private void setChkbox(CheckBox ckbox, string value)
    {
        ckbox.Checked = true;
        if (value=="0")
        {
            ckbox.Checked = false;
        }
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            string isExist = RunQuery.SQLQuery.ReturnString("Select top(1) EmpSerial FROM EmployeeInfo WHERE SalaryBasis='" + lblItemCode.Text + "'");

            if (isExist == "")
            {
                SqlCommand cmd7 = new SqlCommand("DELETE EmpTypes WHERE GroupSrNo=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                cmd7.ExecuteNonQuery();
                cmd7.Connection.Close();

                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "Entry deleted successfully ...";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: Unable to delete! Please delete employee Code <b>" + isExist + "</b>";
            }

            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }
}
