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
    public partial class SubAcc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                populateGroup();
            }
            popGID();
        }

        //Code for Clearing the form
        public static void ClearControls(Control Parent)
        {
            //if (Parent is TextBox)
            //{ (Parent as TextBox).Text = string.Empty; }
            //else
            //{
            //    foreach (Control c in Parent.Controls)
            //        ClearControls(c);
            //}
        }



        private void populateGroup()
        {
            SqlCommand cmd = new SqlCommand("Select GroupID, GroupName from AccountGroup", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            SqlDataReader Grouplist = cmd.ExecuteReader();

            ddGroup.DataSource = Grouplist;
            ddGroup.DataValueField = "GroupID";
            ddGroup.DataTextField = "GroupName";
            ddGroup.DataBind();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        private string popGID()
        {
            string gid = ddGroup.SelectedValue; //RunQuery.SQLQuery.ReturnString("Select GroupID, GroupName from AccountGroup where GroupName= '" + ddGroup.SelectedValue + "'");
            string sid = RunQuery.SQLQuery.ReturnString("Select ISNULL(COUNT(sl),0)+1 from Accounts where GroupID= '" + ddGroup.SelectedValue + "'");
            if (sid.Length < 2)
            {
                sid = "0" + sid;
            }
            sid = gid + sid;
            txtSAID.Text = sid;
            return sid;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string groupID = ddGroup.SelectedValue;
                string subGroupID = popGID();

                if (txtSubAcc.Text != "")
                {
                    if (btnSave.Text == "Update")
                    {
                        int EntryCount = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(AccountsID) from ControlAccount where AccountsID='" + lblOldSubAcId.Text + "'"));

                        if (EntryCount <= 0)
                        {
                            RunQuery.SQLQuery.ExecNonQry("Update Accounts set AccountsName='" + txtSubAcc.Text + "', AccountsID='" + txtSAID.Text + "', GroupID='" + ddGroup.SelectedValue + "'  where AccountsID='" + lblOldSubAcId.Text + "'");
                            lblMsg.Attributes.Add("class", "xerp_success");
                            lblMsg.Text = "Sub-Accounts was updated Successfully.";
                        }
                        else
                        {
                            RunQuery.SQLQuery.ExecNonQry("Update Accounts set AccountsName='" + txtSubAcc.Text + "'  where AccountsID='" + lblOldSubAcId.Text + "'");
                            lblMsg.Attributes.Add("class", "xerp_warning");
                            lblMsg.Text = "Only name field was updated due to link with " + EntryCount + " Control Heads...";
                        }
                        ClearForm();
                    }
                    else
                    {
                        RunQuery.SQLQuery.ExecNonQry("Insert into Accounts (AccountsID, AccountsName, GroupID)" +
                                                        "VALUES ('" + subGroupID + "', '" + txtSubAcc.Text + "','" + groupID + "')");
                        ClearForm();
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Successfully Added new Sub-Accounts";
                    }
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "Please Type the Accounts Name Correctly";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR" + ex.Message.ToString();
            }
        }

        protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            popGID();
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.RowIndex);
                Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

                int EntryCount = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(AccountsID) from ControlAccount where AccountsID='" + lblItemCode.Text + "'"));

                if (EntryCount <= 0)
                {
                    RunQuery.SQLQuery.ExecNonQry("DELETE Accounts WHERE AccountsID=" + lblItemCode.Text);

                    ClearForm();

                    lblMsg.Attributes.Add("class", "xerp_warning");
                    lblMsg.Text = "Sub-account deleted successfully ...";
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "<b>ERROR: </b>Sub-accounts is Locked due to link with " + EntryCount + " Control Heads...";
                }

                upnl.Update();
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.Message.ToString();
            }

        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label Label2 = GridView1.Rows[index].FindControl("Label1") as Label;
                txtSAID.Text = Label2.Text;
                lblOldSubAcId.Text = Label2.Text;

                SqlCommand cmd7 = new SqlCommand("SELECT GroupID, AccountsID, AccountsName  FROM [Accounts] WHERE AccountsID ='" + Label2.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                SqlDataReader dr = cmd7.ExecuteReader();

                if (dr.Read())
                {
                    btnSave.Text = "Update";
                    populateGroup();
                    ddGroup.SelectedValue = dr[0].ToString();
                    txtSAID.Text = dr[1].ToString();
                    txtSubAcc.Text = dr[2].ToString();

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
        private void ClearForm()
        {
            //populateGroup();
            popGID();
            txtSubAcc.Text = "";
            GridView1.DataBind();
            upnl.Update();
        }
    }
}