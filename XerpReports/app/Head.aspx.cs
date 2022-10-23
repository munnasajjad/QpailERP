using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace Oxford.app
{
    public partial class Head : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

            if (!IsPostBack)
            {
                populateGroup();
                populateSub();
                //popSubID();
                populateControl();
                //popControlID();
                populateHeadID();
                txtDate.Text = DateTime.Now.ToShortDateString();
                txtOpBalance.Text = "0";
            }
        }

        //Messagebox For Alerts
        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }


        ///////////////////////////////////////////////
        /**** Populate Combo Boxes ****/
        ///////////////////////////////////////////////
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
        private void populateSub()
        {
            SqlCommand cmd = new SqlCommand("Select AccountsID, AccountsName from Accounts where GroupID=@GroupID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Parameters.Add("@GroupID", SqlDbType.VarChar).Value = ddGroup.SelectedValue;
            cmd.Connection.Open();
            SqlDataReader Datalist = cmd.ExecuteReader();

            ddSub.DataSource = Datalist;
            ddSub.DataValueField = "AccountsID";
            ddSub.DataTextField = "AccountsName";
            ddSub.DataBind();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
            //populateControl();
            //popControlID();
        }
        private void populateControl()
        {
            SqlCommand cmd = new SqlCommand("Select ControlAccountsID, ControlAccountsName from ControlAccount where AccountsID=@AccID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Parameters.Add("@AccID", SqlDbType.VarChar).Value = ddSub.SelectedValue;
            cmd.Connection.Open();
            SqlDataReader Datalist = cmd.ExecuteReader();

            ddControl.DataSource = Datalist;
            ddControl.DataValueField = "ControlAccountsID";
            ddControl.DataTextField = "ControlAccountsName";
            ddControl.DataBind();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        //Generating Acc Head ID
        private string populateHeadID()
        {
            string accID = ddControl.SelectedValue; //RunQuery.SQLQuery.ReturnString("Select GroupID from AccountGroup where GroupName= = '"+ddGroup.SelectedValue+"'"
            //"Select AccountsID, AccountsName from Accounts where AccountsName= '"+ddSub.SelectedValue+"'"
            //"Select * from ControlAccount where ControlAccountsName= '"+ddControl.SelectedValue+"'"
            string maxID = RunQuery.SQLQuery.ReturnString("Select ISNULL(COUNT(EntryID),0)+1 from HeadSetup WHERE ControlAccountsID='" + ddControl.SelectedValue + "'");

            if (maxID.Length < 2)
            {
                maxID = "00" + maxID;
            }
            else if (maxID.Length < 3)
            {
                maxID = "0" + maxID;
            }
            string AcHeadId = accID + maxID;
            string isExist = RunQuery.SQLQuery.ReturnString("Select AccountsHeadID from HeadSetup where AccountsHeadID='" + AcHeadId + "'");
            while (isExist != "")
            {
                maxID = Convert.ToString(Convert.ToInt32(maxID) + 1);

                if (maxID.Length < 2)
                {
                    maxID = "00" + maxID;
                }
                else if (maxID.Length < 3)
                {
                    maxID = "0" + maxID;
                }
                AcHeadId = accID + maxID;
                isExist = RunQuery.SQLQuery.ReturnString("Select AccountsHeadID from HeadSetup where AccountsHeadID='" + AcHeadId + "'");
            }

            txtHeadID.Text = AcHeadId;
            return AcHeadId;
        }

        /// <summary>
        /// Load Combo Boxes ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateSub();
            //popSubID();
            populateControl();
            //popControlID();
            populateHeadID();
        }

        protected void ddSub_SelectedIndexChanged(object sender, EventArgs e)
        {
            //popSubID();
            populateControl();
            //popControlID();
            populateHeadID();
        }

        protected void ddControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //popControlID();
            populateHeadID();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    decimal bldr = Convert.ToDecimal(txtOpBalance.Text);
                    decimal blcr = Convert.ToDecimal(txtOpBalance.Text);

                    if (RadioButton1.Checked == true)
                    {
                        blcr = 0;
                    }
                    else
                    {
                        bldr = 0;
                    }

                    decimal isActive = 1;
                    if (chkDisable.Checked == true)
                    {
                        isActive = 0;
                    }

                    int EntryCount = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(SerialNo) from VoucherDetails where AccountsHeadID='" + txtHeadID.Text + "'"));

                    if (EntryCount <= 0)
                    {
                        RunQuery.SQLQuery.ExecNonQry("Update HeadSetup set GroupID='" + ddGroup.SelectedValue + "', AccountsID='" + ddSub.SelectedValue + "', ControlAccountsID='" + ddControl.SelectedValue + "', AccountsHeadID='" + txtHeadID.Text + "', AccountsHeadName='" + txtHeadName.Text + "', OpDate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', OpBalDr='" + bldr + "', OpBalCr='" + blcr + "', isActive='" + isActive + "' WHERE AccountsHeadID='" + lblHeadID.Text + "'");
                        lblMsg.CssClass = "xerp_success";
                        lblMsg.Text = "Accounts Head was updated successfully!";
                    }
                    else
                    {
                        RunQuery.SQLQuery.ExecNonQry("Update HeadSetup set AccountsHeadName='" + txtHeadName.Text + "', OpDate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', OpBalDr='" + bldr + "', OpBalCr='" + blcr + "', isActive='" + isActive + "' WHERE AccountsHeadID='" + txtHeadID.Text + "'");
                        lblMsg2.Attributes.Add("class", "xerp_warning");
                        lblMsg2.Text = "<b>Head was not fully updated! </b> A/C Head is Locked due to old transtions...";
                    }

                    ClearForm();


                }
                else
                {
                    if (txtHeadName.Text != "" && txtOpBalance.Text != "")
                    {
                        string accID = populateHeadID();
                        //Create Sql Connection
                        SqlConnection cnn = new SqlConnection();
                        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

                        //Create Sql Command
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "INSERT INTO HeadSetup (GroupID, AccountsID, ControlAccountsID, AccountsHeadID, AccountsHeadName, AccountsOpeningBalance, OpDate, Emark, OpBalDr, OpBalCr)" +
                                                "VALUES (@GroupID, @AccountsID, @ControlAccountsID, @AccountsHeadID, @AccountsHeadName, @AccountsOpeningBalance, @OpDate, @Emark, @OpBalDr, @OpBalCr)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = cnn;

                        //Parameter array Declaration
                        SqlParameter[] param = new SqlParameter[8];

                        param[0] = new SqlParameter("@GroupID", SqlDbType.VarChar, 2);
                        param[1] = new SqlParameter("@AccountsID", SqlDbType.VarChar, 4);
                        param[2] = new SqlParameter("@ControlAccountsID", SqlDbType.VarChar, 6);
                        param[3] = new SqlParameter("@AccountsHeadID", SqlDbType.VarChar);
                        param[4] = new SqlParameter("@AccountsHeadName", SqlDbType.VarChar);
                        param[5] = new SqlParameter("@AccountsOpeningBalance", SqlDbType.Decimal);
                        param[6] = new SqlParameter("@OpDate", SqlDbType.DateTime);
                        param[7] = new SqlParameter("@Emark", SqlDbType.VarChar);

                        param[0].Value = ddGroup.SelectedValue;
                        param[1].Value = ddSub.SelectedValue;
                        param[2].Value = ddControl.SelectedValue;
                        param[3].Value = accID;
                        param[4].Value = txtHeadName.Text;
                        param[5].Value = txtOpBalance.Text;
                        param[6].Value = txtDate.Text;


                        decimal drAmount = 0;
                        decimal crAmount = 0;

                        if (RadioButton1.Checked == true)
                        {
                            drAmount = Convert.ToDecimal(txtOpBalance.Text);
                        }
                        else
                        {
                            crAmount = Convert.ToDecimal(txtOpBalance.Text);
                        }

                        cmd.Parameters.Add("@OpBalDr", SqlDbType.Decimal).Value = drAmount;
                        cmd.Parameters.Add("@OpBalCr", SqlDbType.Decimal).Value = crAmount;

                        //if (RadioButton1.Checked == true)
                        //{
                        //    param[7].Value = "HO";
                        //}
                        //else
                        //{
                        param[7].Value = "Both";
                        //}


                        /*** Looping Fields ***/
                        for (int i = 0; i < param.Length; i++)
                        {
                            cmd.Parameters.Add(param[i]);
                        }

                        /*** Command Execution ***/
                        if (ddControl.SelectedValue != "")
                        {
                            cnn.Open();
                            int Success = cmd.ExecuteNonQuery();
                            cnn.Close();

                            if (Success > 0)
                            {
                                lblMsg.Attributes.Add("class", "xerp_success");
                                lblMsg.Text = "New Accounts Head Successfully Added";
                                txtHeadName.Text = "";
                                txtOpBalance.Text = "0";
                                GridView2.DataBind();
                                populateHeadID();

                                txtHeadName.Focus();
                            }
                        }
                        else
                        {
                            lblMsg.CssClass = "xerp_error";
                            lblMsg.Text = "Please setup the control account first under <b>'" + ddSub.SelectedValue + "'</b> Sub Account";
                            MessageBox("Please setup the control head for " + ddSub.SelectedValue + " ");
                        }

                    }
                    else
                    {
                        lblMsg.CssClass = "xerp_error";
                        lblMsg.Text = "Please Enter the Name of the Head";
                    }
                }
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
                Label lblItemCode = GridView2.Rows[index].FindControl("Label2") as Label;

                int EntryCount = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(SerialNo) from VoucherDetails where AccountsHeadID='" + lblItemCode.Text + "'"));

                if (EntryCount <= 0)
                {
                    RunQuery.SQLQuery.ExecNonQry("DELETE HeadSetup WHERE AccountsHeadID=" + lblItemCode.Text);

                    GridView2.DataBind();

                    lblMsg.Attributes.Add("class", "xerp_warning");
                    lblMsg.Text = "A/C Head deleted successfully ...";
                    upnl.Update();
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_warning");
                    lblMsg.Text = "<b>ERROR: </b>A/C Head is Locked due to old transtions...";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.Message.ToString();
            }

        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            upnl.Update();
        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(GridView2.SelectedIndex);
                Label Label2 = GridView2.Rows[index].FindControl("Label2") as Label;
                txtHeadID.Text = Label2.Text;
                lblHeadID.Text = Label2.Text;

                SqlCommand cmd7 = new SqlCommand("SELECT GroupID, AccountsID, ControlAccountsID, AccountsHeadName, OpDate, OpBalDr, OpBalCr, IsActive  FROM [HeadSetup] WHERE AccountsHeadID ='" + Label2.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                SqlDataReader dr = cmd7.ExecuteReader();

                if (dr.Read())
                {
                    btnSave.Text = "Update";
                    populateGroup();
                    populateSub();
                    populateControl();

                    ddGroup.SelectedValue = dr[0].ToString();
                    populateSub();
                    ddSub.SelectedValue = dr[1].ToString();

                    populateControl();
                    ddControl.SelectedValue = dr[2].ToString();
                    //populateHeadID();

                    txtHeadName.Text = dr[3].ToString();
                    txtDate.Text = Convert.ToDateTime(dr[4].ToString()).ToShortDateString();
                    decimal bldr = Convert.ToDecimal(dr[5].ToString());
                    decimal blcr = Convert.ToDecimal(dr[6].ToString());

                    if (bldr > 0)
                    {
                        txtOpBalance.Text = bldr.ToString();
                        RadioButton1.Checked = true;
                        RadioButton2.Checked = false;
                    }
                    else
                    {
                        txtOpBalance.Text = blcr.ToString();
                        RadioButton2.Checked = true;
                        RadioButton1.Checked = false;
                    }

                    decimal isActive = Convert.ToDecimal(dr[7].ToString());
                    chkDisable.Visible = true;
                    if (isActive == 1)
                    {
                        chkDisable.Checked = false;
                    }
                    else
                    {
                        chkDisable.Checked = true;
                    }

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
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblMsg.CssClass = "xerp_stop";
            lblMsg.Text = "Entry form was Form Refreshed!";
        }
        private void ClearForm()
        {
            populateGroup();
            populateSub();
            populateControl();
            populateHeadID();
            txtDate.Text = DateTime.Now.ToShortDateString();
            txtOpBalance.Text = "0";
            btnSave.Text = "Save New Head";
            txtHeadName.Text = "";
            chkDisable.Visible = false;
            GridView2.DataBind();
            upnl.Update();
        }

        protected void btnSubRefresh_Click(object sender, EventArgs e)
        {
            populateSub();
            populateHeadID();
        }
        protected void btnControlRefresh_Click(object sender, EventArgs e)
        {
            populateControl();
            populateHeadID();
        }

    }
}