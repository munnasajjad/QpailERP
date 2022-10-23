using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security;
using System.IO;

namespace Oxford.app
{
    public partial class Control : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                populateSub();
                popSubID();
            }
        }

        private void populateSub()
        {
            SqlCommand cmd = new SqlCommand("Select AccountsID, AccountsName from Accounts", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            SqlDataReader Datalist = cmd.ExecuteReader();

            ddSub.DataSource = Datalist;
            ddSub.DataValueField = "AccountsID";
            ddSub.DataTextField = "AccountsName";
            ddSub.DataBind();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        private string popSubID()
        {
            string gid = ddSub.SelectedValue; //RunQuery.SQLQuery.ReturnString("Select GroupID, GroupName from AccountGroup where GroupName= '" + ddGroup.SelectedValue + "'");
            string sid = RunQuery.SQLQuery.ReturnString("Select ISNULL(COUNT(sl),0)+1 from ControlAccount where AccountsID= '" + gid + "'");
            if (sid.Length < 2)
            {
                sid = "0" + sid;
            }
            sid = gid + sid;
            string isExist = RunQuery.SQLQuery.ReturnString("Select ControlAccountsID FROM ControlAccount where ControlAccountsID='" + sid + "'");

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
                        int EntryCount = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(ControlAccountsID) from HeadSetup where ControlAccountsID='" + lblOldSubAcId.Text + "'"));

                        if (EntryCount <= 0)
                        {
                            RunQuery.SQLQuery.ExecNonQry("Update ControlAccount set ControlAccountsName='" + txtControl.Text + "', ControlAccountsID='" + txtNid.Text + "', AccountsID='" + ddSub.SelectedValue + "'  where ControlAccountsID='" + lblOldSubAcId.Text + "'");
                            lblMsg.Attributes.Add("class", "xerp_success");
                            lblMsg.Text = "Control-Accounts was updated Successfully.";
                        }
                        else
                        {
                            RunQuery.SQLQuery.ExecNonQry("Update ControlAccount set ControlAccountsName='" + txtControl.Text + "'  where AccountsID='" + lblOldSubAcId.Text + "'");
                            lblMsg.Attributes.Add("class", "xerp_warning");
                            lblMsg.Text = "Only name field was updated due to link with " + EntryCount + " Control Heads...";
                        }
                        ClearForm();
                    }
                    else
                    {

                        string groupID = ddSub.SelectedValue;
                        string subGroupID = popSubID();
                        RunQuery.SQLQuery.ExecNonQry("Insert into ControlAccount (ControlAccountsID, ControlAccountsName, AccountsID)" +
                                                        "VALUES ('" + subGroupID + "', '" + txtControl.Text + "','" + groupID + "')");
                        txtControl.Text = "";
                        popSubID();
                        GridView2.DataBind();
                        lblMsg.Text = "Successfully Added new Control Account";
                    }
                }
                else { lblMsg.Text = "Please Type the Name Correctly"; }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "ERROR" + ex.Message.ToString();
            }

        }

        protected void ddSub_SelectedIndexChanged(object sender, EventArgs e)
        {
            popSubID();
        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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
                    populateSub();
                    ddSub.SelectedValue = dr[0].ToString();
                    txtNid.Text = dr[1].ToString();
                    txtControl.Text = dr[2].ToString();

                    lblMsg.Attributes.Add("class", "xerp_info");
                    lblMsg.Text = "A/C info loaded in edit mode";
                }

                cmd7.Connection.Close();
                upnl.Update();
            }
            catch (Exception ex)
            {
                lblMsg.CssClass = "xerp_error";
                lblMsg.Text = "ERROR: " + ex.ToString();
            }
        }
        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.RowIndex);
                Label lblItemCode = GridView2.Rows[index].FindControl("Label1") as Label;

                int EntryCount = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(ControlAccountsID) from HeadSetup where ControlAccountsID='" + lblItemCode.Text + "'"));

                if (EntryCount <= 0)
                {
                    RunQuery.SQLQuery.ExecNonQry("DELETE ControlAccount WHERE ControlAccountsID=" + lblItemCode.Text);

                    ClearForm();

                    lblMsg.Attributes.Add("class", "xerp_warning");
                    lblMsg.Text = "Control-account deleted successfully ...";
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "<b>ERROR: </b>Control-accounts is Locked due to link with " + EntryCount + " Acc Heads...";
                }

                upnl.Update();
                GridView2.DataBind();
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.Message.ToString();
            }

        }

        private void ClearForm()
        {
            //populateGroup();
            popSubID();
            txtControl.Text = "";
            GridView2.DataBind();
            upnl.Update();
        }
    }
}