using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Settings_User_Permission : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string query = "SELECT LevelID, LevelName FROM [UserLevel] WHERE LevelID> (Select UserLevel from logins where LoginUserName ='" + Page.User.Identity.Name.ToString() + "') ORDER BY [LevelID]";
            //txtCurrentPosition.Text = SQLQuery.ReturnString("");
            SQLQuery.PopulateDropDown(query, ddLevelX, "LevelID", "LevelName");

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
    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //DropDownList1.DataBind();
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label PID = GridView1.Rows[index].FindControl("Label1") as Label;
            lblid.Text = PID.Text;
            Notify("Edit mode activated", "info", lblMsg);

            EditMode();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    private void EditMode()
    {
        SqlCommand cmd7 = new SqlCommand("SELECT LoginUserName, EmployeeInfoID, UserLevel FROM [Logins] WHERE LID='" + lblid.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            txtUserX.Text = dr[0].ToString();

            string eid = SQLQuery.ReturnString("SELECT EmployeeInfoID FROM [EmployeeInfo] WHERE EmployeeInfoID='" + dr[1].ToString() + "'");
            if (eid != "")
            {
                ddCounterX.SelectedValue = SQLQuery.ReturnString("SELECT DepartmentID FROM [EmployeeInfo] WHERE EmployeeInfoID='" + dr[1].ToString() + "'");
                ddEmployeX.DataBind();
                ddEmployeX.SelectedValue = eid;
            }

            eid = dr[2].ToString(); //SQLQuery.ReturnString("SELECT UserLevel FROM [Counters] WHERE UserLevel='" + dr[2].ToString() + "'");
            if (eid != "")
            {
                ddLevelX.SelectedValue = eid;
            }

        }
        cmd7.Connection.Close();
        chkPermissions();
    }

    private void chkPermissions()
    {
        if (isBlocked("SALES", CheckBox1) != "1")
        {
            isBlocked("SALES1", CheckBox2);
            isBlocked("SALES2", CheckBox3);
            isBlocked("SALES3", CheckBox4);
            isBlocked("SalesBOQReports", salesBOQCheckBox);
            isBlocked("SALES4", CheckBox5);
        }
        if (isBlocked("PURCHASE", CheckBox6) != "1")
        {
            isBlocked("PURCHASE1", CheckBox7);
            isBlocked("PURCHASE2", CheckBox8);
            isBlocked("PURCHASE3", CheckBox9);
            isBlocked("PURCHASE4", CheckBox10);
            isBlocked("PURCHASE5", CheckBox10m);
        }
        if (isBlocked("INVENTORY", CheckBox11) != "1")
        {
            isBlocked("INVENTORY1", CheckBox12);
            isBlocked("INVENTORY2", CheckBox13);
            isBlocked("INVENTORY3", CheckBox14);
            isBlocked("INVENTORY4", CheckBox15);
            isBlocked("INVENTORY5", CheckBox135);
        }
        if (isBlocked("INVENTORY NEW", ChkInventoryNew) != "1")
        {
            isBlocked("INVENTORYNEW1", ChkStockTransaction);
            isBlocked("INVENTORYNEW2", ChkStockTransaction);
            //isBlocked("INVENTORY3", sub12);
            //isBlocked("INVENTORY4", sub13);
            //isBlocked("INVENTORY5", sub14);
        }
        if (isBlocked("PRODUCTION", CheckBox16) != "1")
        {
            isBlocked("PRODUCTION1", CheckBox17);
            isBlocked("PRODUCTION2", CheckBox18);
            isBlocked("PRODUCTION3", CheckBox20);
            //isBlocked("PRODUCTION4", CheckBox5);
        }
        if (isBlocked("ACCOUNTS", CheckBox21) != "1")
        {
            isBlocked("ACCOUNTS1", CheckBox22);
            isBlocked("ACCOUNTS2", CheckBox24);
            isBlocked("ACCOUNTS3", CheckBox25);
        }
        if (isBlocked("PAYROLL", CheckBox26) != "1")
        {
            isBlocked("PAYROLL1", CheckBox27);
            isBlocked("PAYROLL2", CheckBox28);
            isBlocked("PAYROLL3", CheckBox30);
        }
        if (isBlocked("CRM", CheckBox31) != "1")
        {
            isBlocked("CRM1", CheckBox34);
            isBlocked("CR2", CheckBox32);
            isBlocked("CR3", CheckBox33);
        }
        if (isBlocked("FACTS", CheckBox36) != "1")
        {
            isBlocked("FACTS1", CheckBox37);
            isBlocked("FACTS2", CheckBox38);
            isBlocked("FACTS3", CheckBox39);
        }
        if (isBlocked("ADMIN", CheckBox41) != "1")
        {
            isBlocked("ADMIN1", CheckBox42);
            isBlocked("ADMIN2", CheckBox43);
            isBlocked("ADMIN3", CheckBox44);
            isBlocked("ADMIN4", CheckBox45);
            isBlocked("ADMIN5", CheckBox45d);
        }
        showHide(Panel1, CheckBox1);
        showHide(Panel2, CheckBox6);
        showHide(Panel3, CheckBox11);
        showHide(Panel4, CheckBox16);
        showHide(Panel5, CheckBox21);
        showHide(Panel7, CheckBox16);
        showHide(Panel6, CheckBox31);
        showHide(Panel8, CheckBox36);
        showHide(Panel9, CheckBox41);
        showHide(Panel10, ChkInventoryNew);

    }
    private string isBlocked(string formName, CheckBox chkBox)
    {
        string user = txtUserX.Text;
        string isBlockEd2 = SQLQuery.ReturnString("Select IsBlocked from UserForms where UserID='" + user + "' AND  FormName='" + formName + "'");
        if (isBlockEd2 == "1")
        {
            chkBox.Checked = false;
        }
        else
        {
            chkBox.Checked = true;
        }
        return isBlockEd2;
    }


    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        //Response.Redirect("New-user.aspx");
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        if (txtUserX.Text != "")
        {

            SQLQuery.ExecNonQry("Update Logins set EmployeeInfoID='" + ddEmployeX.SelectedValue + "', UserLevel='" +
                                ddLevelX.SelectedValue + "' where LID='" + lblid.Text + "' ");
            SQLQuery.ExecNonQry("Delete UserForms where UserID='" + txtUserX.Text + "' ");
            setPermissions();
            Notify("Saved Successfully", "success", lblMsg);
        }
        else
        {
            Notify("Invalid User!", "error", lblMsg);
        }
    }
    private void setPermissions()
    {
        if (SetBlock("SALES", CheckBox1) == "1")
        {
            //setBlock("SALES", CheckBox1);
        }
        else
        {
            SetBlock("SALES1", CheckBox2);
            SetBlock("SALES2", CheckBox3);
            SetBlock("SALES3", CheckBox4);
            SetBlock("SalesBOQReports", salesBOQCheckBox);
            SetBlock("SALES4", CheckBox5);
        }
        if (SetBlock("PURCHASE", CheckBox6) == "1")
        {
            //setBlock("PURCHASE", CheckBox6);
        }
        else
        {
            SetBlock("PURCHASE1", CheckBox7);
            SetBlock("PURCHASE2", CheckBox8);
            SetBlock("PURCHASE3", CheckBox9);
            SetBlock("PURCHASE4", CheckBox10);
            SetBlock("PURCHASE5", CheckBox10m);
        }
        if (SetBlock("INVENTORY", CheckBox11) == "1")
        {
            //setBlock("INVENTORY", CheckBox11);
        }
        else
        {
            SetBlock("INVENTORY1", CheckBox12);
            SetBlock("INVENTORY2", CheckBox13);
            SetBlock("INVENTORY3", CheckBox14);
            SetBlock("INVENTORY4", CheckBox15);
            SetBlock("INVENTORY5", CheckBox135);
        }

        if (SetBlock("PRODUCTION", CheckBox16) == "1")
        {
            //setBlock("PRODUCTION", CheckBox16);
        }
        else
        {
            SetBlock("PRODUCTION1", CheckBox17);
            SetBlock("PRODUCTION2", CheckBox18);
            SetBlock("PRODUCTION3", CheckBox20);
            //setBlock("PRODUCTION4", CheckBox5);
        }
        if (SetBlock("ACCOUNTS", CheckBox21) == "1")
        {
            //setBlock("ACCOUNTS", CheckBox21);
        }
        else
        {
            SetBlock("ACCOUNTS1", CheckBox22);
            SetBlock("ACCOUNTS2", CheckBox24);
            SetBlock("ACCOUNTS3", CheckBox25);
        }
        if (SetBlock("PAYROLL", CheckBox26) == "1")
        {
            //setBlock("PAYROLL", CheckBox16);
        }
        else
        {
            SetBlock("PAYROLL1", CheckBox27);
            SetBlock("PAYROLL2", CheckBox28);
            SetBlock("PAYROLL3", CheckBox30);
        }
        if (SetBlock("CRM", CheckBox31) == "1")
        {
            //setBlock("CRM", CheckBox31);
        }
        else
        {
            SetBlock("CRM1", CheckBox34);
            SetBlock("CR2", CheckBox32);
            SetBlock("CR3", CheckBox33);
        }
        if (SetBlock("FACTS", CheckBox36) == "1")
        {
            //setBlock("FACTS", CheckBox36);
        }
        else
        {
            SetBlock("FACTS1", CheckBox37);
            SetBlock("FACTS2", CheckBox38);
            SetBlock("FACTS3", CheckBox39);
        }
        if (SetBlock("ADMIN", CheckBox41) == "1")
        {
            //setBlock("ADMIN", CheckBox41);
        }
        else
        {
            SetBlock("ADMIN1", CheckBox42);
            SetBlock("ADMIN2", CheckBox43);
            SetBlock("ADMIN3", CheckBox44);
            SetBlock("ADMIN4", CheckBox45);
            SetBlock("ADMIN5", CheckBox45d);
        }
    }
    private string SetBlock(string formName, CheckBox chkBox)
    {
        string user = txtUserX.Text;
        string isBlockEd2 = "0";
        if (!chkBox.Checked)
        {
            isBlockEd2 = "1";
            SQLQuery.ExecNonQry("INSERT INTO UserForms (UserID, FormName, IsBlocked, EntryBy) VALUES ('"+ user + "', '"+ formName + "', '"+ isBlockEd2 + "', '"+User.Identity.Name+"')");
        }
        return isBlockEd2;
    }

    protected void ddCounter_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //DropDownList ddEmploye = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddEmploye");
        //ddEmploye.DataBind();
    }

    private void showHide(Panel panelName, CheckBox chkBox)
    {
        if (chkBox.Checked)
        {
            panelName.Visible = true;
        }
        else
        {
            panelName.Visible = false;
        }
    }
    protected void CheckBox1_OnCheckedChanged(object sender, EventArgs e)
    {
        showHide(Panel1, CheckBox1);
    }

    protected void CheckBox6_OnCheckedChanged(object sender, EventArgs e)
    {
        showHide(Panel2, CheckBox6);
    }

    protected void CheckBox11_OnCheckedChanged(object sender, EventArgs e)
    {
        showHide(Panel3, CheckBox11);
    }

    protected void CheckBox16_OnCheckedChanged(object sender, EventArgs e)
    {
        showHide(Panel4, CheckBox16);
    }

    protected void CheckBox21_OnCheckedChanged(object sender, EventArgs e)
    {
        showHide(Panel5, CheckBox21);
    }

    protected void CheckBox26_OnCheckedChanged(object sender, EventArgs e)
    {
        showHide(Panel7, CheckBox26);
    }

    protected void CheckBox31_OnCheckedChanged(object sender, EventArgs e)
    {
        showHide(Panel6, CheckBox31);
    }

    protected void CheckBox36_OnCheckedChanged(object sender, EventArgs e)
    {
        showHide(Panel8, CheckBox36);
    }

    protected void CheckBox41_OnCheckedChanged(object sender, EventArgs e)
    {
        showHide(Panel9, CheckBox41);
    }

    protected void ChkInventoryNew_OnCheckedChanged(object sender, EventArgs e)
    {
        showHide(Panel10, ChkInventoryNew);
    }
}
