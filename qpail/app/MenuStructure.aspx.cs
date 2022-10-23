using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_MenuStructure : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick", " disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            lblUser.Text = Page.User.Identity.Name.ToString();
            //lblProjectID.Text = SQLQuery.ProjectID(lblUser.Text);
            getPriority();
            ddFormGroup.DataBind();
            ddSubGroup.DataBind();
            GridView1.DataBind();
        }
    }

    private void getPriority()
    {
        txtPriority.Text = SQLQuery.ReturnString("Select COUNT(sl) from MenuStructure");
    }
    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int isBlocked = 0;
            if (chkActive.Checked)
            {
                isBlocked = 1;
            }

            if (btnSave.Text == "Save")
            {
                SQLQuery.ExecNonQry("INSERT INTO MenuStructure (MenuGroup, MenuSubGroup, FormName, PageName, HTMLControlID, Priority, IsBlocked, EntryBy) " +
                    "VALUES  ('" + ddFormGroup.SelectedValue + "', '" + ddSubGroup.SelectedValue + "','" +
                    txtName.Text + "', '" + txtSubject.Text + "', '" + txtMsgBody2.Text + "', '" + txtPriority.Text + "', '" + isBlocked +
                    "', '" + User.Identity.Name + "' )");

                Notify("Menu item added Successfully", "success", lblMsg);
            }
            else
            {
                SQLQuery.ExecNonQry("UPDATE MenuStructure SET MenuGroup='" + ddFormGroup.SelectedValue + "', " +
                    "MenuSubGroup='" + ddSubGroup.SelectedValue + "', FormName='" +
                    txtName.Text + "', PageName='" + txtSubject.Text + "', HTMLControlID='" + txtMsgBody2.Text + "', Priority='" + txtPriority.Text + "', IsBlocked='" + isBlocked +
                    "' WHERE sl='" + lblID.Text + "' ");

                Notify("Menu item update Successfully", "success", lblMsg);
            }
            btnSave.Text = "Save";
            getPriority();
            GridView1.DataBind();
            txtName.Text = "";
            txtSubject.Text = "";
            txtMsgBody2.Text = "";
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }

    }

    protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    protected void ddFormGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddSubGroup.DataBind();
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label PID = GridView1.Rows[index].FindControl("Label1") as Label;
            lblID.Text = PID.Text;

            EditMode();
            btnSave.Text = "Update";
            lblMsg.Attributes.Add("class", "xerp_warn");
            Notify("Edit mode activated ...", "info", lblMsg);
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }

    private void EditMode()
    {
        DataTable dt = SQLQuery.ReturnDataTable(@"Select TOP(1) sl, MenuGroup, MenuSubGroup, FormName, PageName, HTMLControlID, Priority, IsBlocked, EntryBy, EntryDate
                            FROM MenuStructure WHERE SL='" + lblID.Text + "'");
        foreach (DataRow dr in dt.Rows)
        {
            ddSubGroup.SelectedValue = dr["MenuSubGroup"].ToString();
            txtName.Text = dr["FormName"].ToString();
            txtSubject.Text = dr["PageName"].ToString();
            txtMsgBody2.Text = dr["HTMLControlID"].ToString();
            txtPriority.Text = dr["Priority"].ToString();
            string act = dr["IsBlocked"].ToString();
            chkActive.Checked = false;
            if (act == "1")
            {
                chkActive.Checked = true;
            }
        }

    }
    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            SQLQuery.ExecNonQry("Delete MenuStructure WHERE sl='" + lblItemCode.Text + "'");
            GridView1.DataBind();
            lblMsg.Attributes.Add("class", "xerp_warning");

            Notify("Entry removed successfully ...", "success", lblMsg);
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }
}
