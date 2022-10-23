using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Production_AccountsLinkup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RunQuery.SQLQuery.PopulateDropDown("Select GroupID, GroupName from AccountGroup", ddGroup, "GroupID", "GroupName");
            PopulateSub();
            PopSubId();
            GridView2.DataBind();
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

    private void PopulateSub()
    {
        SqlCommand cmd = new SqlCommand("Select AccountsID, AccountsName from Accounts where GroupID='" + ddGroup.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader Datalist = cmd.ExecuteReader();

        ddSub.DataSource = Datalist;
        ddSub.DataValueField = "AccountsID";
        ddSub.DataTextField = "AccountsName";
        ddSub.DataBind();

        cmd.Connection.Close();
        cmd.Connection.Dispose();
    }

    private string PopSubId()
    {
        string gid = ddSub.SelectedValue; //RunQuery.SQLQuery.ReturnString("Select GroupID, GroupName from AccountGroup where GroupName= '" + ddGroup.SelectedValue + "'");
        string sid = RunQuery.SQLQuery.ReturnString("Select ISNULL(COUNT(sl),0)+1 from ControlAccount where AccountsID= '" + gid + "'");
        if (sid.Length < 2)
        {
            sid = "0" + sid;
        }
        sid = gid + sid;
        string isExist
            = RunQuery.SQLQuery.ReturnString("Select ControlAccountsID FROM ControlAccount where ControlAccountsID='" + sid + "'");

        int i = 2;
        while (isExist != "")
        {
            sid = RunQuery.SQLQuery.ReturnString("Select ISNULL(COUNT(sl),0)+" + i + " from ControlAccount where AccountsID= '" + gid + "'");
            if (sid.Length < 2)
            {
                sid = "0" + sid;
            }
            sid = gid + sid;
            isExist = RunQuery.SQLQuery.ReturnString("Select ControlAccountsID FROM ControlAccount where ControlAccountsID='" + sid + "'");
            i++;
        }

        txtNid.Text = sid;
        return sid;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtControl.Text != "")
            {
                if (btnSave.Text == "Update")
                {
                    if (lblOldSubAcId.Text != "")
                    {
                        int entryCount = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(ControlAccountsID) from HeadSetup where ControlAccountsID='" + lblOldSubAcId.Text + "'"));

                        if (entryCount <= 0)
                        {
                            RunQuery.SQLQuery.ExecNonQry("Update ControlAccount set ControlAccountsName='" + txtControl.Text + "', ControlAccountsID='" + txtNid.Text +
                                                         "', AccountsID='" + ddSub.SelectedValue + "', PrdnLinkId='" + ddParticular.SelectedValue + "' where ControlAccountsID='" + lblOldSubAcId.Text + "'");
                            lblMsg.Attributes.Add("class", "xerp_success");
                            lblMsg.Text = "Control-Accounts was updated Successfully.";
                        }
                        else
                        {
                            RunQuery.SQLQuery.ExecNonQry("Update ControlAccount set ControlAccountsName='" + txtControl.Text + "', PrdnLinkId='" + ddParticular.SelectedValue + "' where ControlAccountsID='" + lblOldSubAcId.Text + "'");
                            lblMsg.Attributes.Add("class", "xerp_warning");
                            lblMsg.Text = "Only name field was updated due to link with " + entryCount + " Control Heads...";
                        }
                    }
                    else
                    {
                        Notify("Please select control accounts name", "error", lblMsg);
                    }
                }
                else
                {
                    string groupId = ddSub.SelectedValue;
                    string subGroupId = PopSubId();
                    RunQuery.SQLQuery.ExecNonQry("Insert into ControlAccount (ControlAccountsID, ControlAccountsName, AccountsID)" +
                                                 "VALUES ('" + subGroupId + "', '" + txtControl.Text + "','" + groupId + "')");
                    txtControl.Text = "";
                    PopSubId();
                    GridView2.DataBind();
                    Notify("Successfully Added new Control Account", "success", lblMsg);
                }
                ClearForm();
            }
            else
            {
                Notify("Please Type the Name Correctly", "error", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
        finally
        {
            GridView2.DataBind();
        }

    }

    protected void ddSub_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopSubId();
    }

    protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Restrict User
            string permLevel = SQLQuery.ReturnString("Select UserLevel from Logins where LoginUserName='" + User.Identity.Name + "'");
            if (Convert.ToInt32(permLevel) > 1) //Permitted Only for super Admin
            {
                Notify("You have not authorized for <b>EDITING</b> the Link Up!", "error", lblMsg);
            }
            else
            {
                int index = Convert.ToInt32(GridView2.SelectedIndex);
                Label Label2 = GridView2.Rows[index].FindControl("Label1") as Label;
                txtControl.Text = Label2.Text;
                lblOldSubAcId.Text = Label2.Text;

                SqlCommand cmd7 = new SqlCommand("SELECT AccountsID, ControlAccountsID, ControlAccountsName  FROM [ControlAccount] WHERE ControlAccountsID ='" + Label2.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                SqlDataReader dr = cmd7.ExecuteReader();

                if (dr.Read())
                {
                    btnSave.Text = "Update";
                    PopulateSub();
                    ddSub.SelectedValue = dr[0].ToString();
                    txtNid.Text = dr[1].ToString();
                    txtControl.Text = dr[2].ToString();

                    lblMsg.Attributes.Add("class", "xerp_info");
                    lblMsg.Text = "A/C info loaded in edit mode";
                }

                cmd7.Connection.Close();
                upnl.Update();
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }
    protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView2.Rows[index].FindControl("Label1") as Label;

            int entryCount = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(ControlAccountsID) from HeadSetup where ControlAccountsID='" + lblItemCode.Text + "'"));

            if (entryCount <= 0)
            {
                RunQuery.SQLQuery.ExecNonQry("DELETE ControlAccount WHERE ControlAccountsID=" + lblItemCode.Text);

                ClearForm();

                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "Control-account deleted successfully ...";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "<b>ERROR: </b>Control-accounts is Locked due to link with " + entryCount + " Acc Heads...";
            }

            upnl.Update();
            GridView2.DataBind();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }

    }

    private void ClearForm()
    {
        btnSave.Text = "Update";
        PopSubId();
        txtControl.Text = "";
        GridView2.DataBind();
        upnl.Update();
        txtControl.Focus();
    }

    protected void ddGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateSub();
        PopSubId();
        GridView2.DataBind();
    }

    protected void ddParticular_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridView2.DataBind();
    }
}
