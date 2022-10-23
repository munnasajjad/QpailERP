using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Holidays : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            SqlCommand cmd7 = new SqlCommand("SELECT Saturday, Sunday, Monday, Tuesday, Wednesday, Thursday, Friday FROM [Weekdays] WHERE sl='1'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                setChkbox(chkSaturday, dr[0].ToString());
                setChkbox(chkSunday, dr[1].ToString());
                setChkbox(ChkMonday, dr[2].ToString());

                setChkbox(chkThursday, dr[3].ToString());
                setChkbox(chkWednesday, dr[4].ToString());
                setChkbox(chkThursday, dr[5].ToString());
                setChkbox(chkFriday, dr[6].ToString());
            }
            cmd7.Connection.Close();
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
            DateTime dt = Convert.ToDateTime(txtDate.Text);
            string itemExist = SQLQuery.ReturnString("SELECT DayName FROM Holidays WHERE Date ='" + dt.ToString("yyyy-MM-dd") + "'");

            if (itemExist == "")
            {
                SqlCommand cmd2 = new SqlCommand("INSERT INTO Holidays (HolidayType, DayName, Date, Weekday, EntryBy)" +
                                                 " VALUES ('" + ddType.SelectedValue + "', @DayName, @Date, @Weekday, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.AddWithValue("@DayName", txtName.Text);
                cmd2.Parameters.AddWithValue("@Date", dt.ToString("yyyy-MM-dd"));
                cmd2.Parameters.AddWithValue("@Weekday", dt.ToString("dddd"));
                cmd2.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
                Notify("New Holiday Added Successfully", "success", lblMsg);
            }
            else
            {
                lblMsg.Text = "ERROR: Day already exist!";
                lblMsg.Attributes.Add("class", "xerp_error");
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
        finally
        {
            GridView1.DataBind();
        }

    }

    private void ExecuteUpdate()
    {
        DateTime dt = Convert.ToDateTime(txtDate.Text);
        SqlCommand cmd2 = new SqlCommand("Update Holidays SET HolidayType='" + ddType.SelectedValue + "', DayName=@DayName, Date=@Date, Weekday=@Weekday " +
                                                 " Where sl='" + lblId.Text + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@DayName", txtName.Text);
        cmd2.Parameters.AddWithValue("@Date", dt.ToString("yyyy-MM-dd"));
        cmd2.Parameters.AddWithValue("@Weekday", dt.ToString("dddd"));
        cmd2.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        txtName.Text = "";
        txtDate.Text = "";
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
        txtDate.Text = "";
        btnSave.Text = "Save";
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
        SqlCommand cmd7 = new SqlCommand("SELECT HolidayType, DayName, Date FROM [Holidays] WHERE sl='" + id + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            btnSave.Text = "Update";
            lblId.Text = id;
            ddType.SelectedValue = dr[0].ToString();
            txtName.Text = dr[1].ToString();
            txtDate.Text = Convert.ToDateTime(dr[2].ToString()).ToString("dd/MM/yyyy");
        }
        cmd7.Connection.Close();
    }

    private void setChkbox(CheckBox ckbox, string value)
    {
        ckbox.Checked = true;
        if (value == "0")
        {
            ckbox.Checked = false;
        }
    }
    
    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            //string isExist = RunQuery.SQLQuery.ReturnString("Select top(1) EmpSerial FROM EmployeeInfo WHERE SalaryBasis='" + lblItemCode.Text + "'");

            //if (isExist == "")
            //{
                SqlCommand cmd7 = new SqlCommand("DELETE Holidays WHERE sl=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                cmd7.ExecuteNonQuery();
                cmd7.Connection.Close();

                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "Entry deleted successfully ...";
            //}
            //else
            //{
            //    lblMsg.Attributes.Add("class", "xerp_error");
            //    lblMsg.Text = "ERROR: Unable to delete! Please delete employee Code <b>" + isExist + "</b>";
            //}

            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    protected void chkSaturday_OnCheckedChanged(object sender, EventArgs e)
    {
        SQLQuery.ExecNonQry("Update Weekdays set Purpose='Weekly Holiday', Saturday='" + CheckBoxValue(chkSaturday) + "', Sunday='" + CheckBoxValue(chkSunday) + "', Monday='" + CheckBoxValue(ChkMonday) + "', Tuesday='" + CheckBoxValue(chkTuesday) + "', Wednesday='" + CheckBoxValue(chkWednesday) + "', Thursday='" + CheckBoxValue(chkThursday) + "', Friday='" + CheckBoxValue(chkFriday) + "'");
    }
}
