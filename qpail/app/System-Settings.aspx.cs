using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_System_Settings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string query = "SELECT CompanyID, CompanyName FROM [Company]  ORDER BY [CompanyID]";
            //txtCurrentPosition.Text = SQLQuery.ReturnString("");
            SQLQuery.PopulateDropDown(query, ddLevelX, "CompanyID", "CompanyName");
            //DropDownList ddLevel = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddLevel");
            //SQLQuery.PopulateDropDown(query, ddLevel, "LevelID", "LevelName");
            clearForm();
        }
    }

    private void clearForm()
    {
        string invId = SQLQuery.ReturnString("Select ShortDescription from Settings_Project where sid=5");
        if (invId == "0")
        {
            rbAuto.Checked = false;
            rbManual.Checked = true;
        }
        else
        {
            rbManual.Checked = false;
            rbAuto.Checked = true;
        }

        ddLevelX.SelectedValue = SQLQuery.ReturnString("Select ShortDescription from Settings_Project where sid=6");
        getDetail();
    }

    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        clearForm();
    }

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        string invId = "1";
        if (rbManual.Checked)
        {
            invId = "0";
        }

        SQLQuery.ExecNonQry("Update Company set CompanyName='" + txtCompanyName.Text + "', CompanyAddress='" + txtAddress.Text + "' where CompanyID='" + ddLevelX.SelectedValue + "' ");
        SQLQuery.ExecNonQry("Update Settings_Project set ShortDescription='" + invId + "' where sid='5' ");
        SQLQuery.ExecNonQry("Update Settings_Project set ShortDescription='" + ddLevelX.SelectedValue + "' where sid='6' ");
        Notify("Updated Successfully", "success", lblMsg);

        //Panel1.Visible = false;
        //pnlRegister.Visible = true;
    }

    protected void ddCounter_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //DropDownList ddEmploye = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddEmploye");
        //ddEmploye.DataBind();
    }
    private void getDetail()
    {

            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1) CompanyName, CompanyAddress FROM Company WHERE (CompanyID = '" + ddLevelX.SelectedValue + "')");
            foreach (DataRow drx in dtx.Rows)
            {
                txtCompanyName.Text = drx["CompanyName"].ToString();
                txtAddress.Text = drx["CompanyAddress"].ToString();
            }



    }


    protected void ddLevelX_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        getDetail();
    }
}
