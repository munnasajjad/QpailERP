using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


public partial class Operator_Accounts_Head : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            populateGroup();
            popGID();
            populateSub();
            popSubID();
            populateControl();
            popControlID();
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
        ddGroup.DataValueField = "GroupName";
        ddGroup.DataTextField = "GroupName";
        ddGroup.DataBind();

        cmd.Connection.Close();
        cmd.Connection.Dispose();
    }
    private void populateSub()
    {
        SqlCommand cmd = new SqlCommand("Select AccountsID, AccountsName from Accounts where GroupID=@GroupID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.Add("@GroupID", SqlDbType.VarChar).Value = lblGroup.Text;
        cmd.Connection.Open();
        SqlDataReader Datalist = cmd.ExecuteReader();

        ddSub.DataSource = Datalist;
        ddSub.DataValueField = "AccountsName";
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
        cmd.Parameters.Add("@AccID", SqlDbType.VarChar).Value = lblSub.Text;
        cmd.Connection.Open();
        SqlDataReader Datalist = cmd.ExecuteReader();

        ddControl.DataSource = Datalist;
        ddControl.DataValueField = "ControlAccountsName";
        ddControl.DataTextField = "ControlAccountsName";
        ddControl.DataBind();

        cmd.Connection.Close();
        cmd.Connection.Dispose();
    }

    ///////////////////////////////////////////////
    /**** Populate ID's ****/
    ///////////////////////////////////////////////
    private void popGID()
    {
        SqlCommand cmd = new SqlCommand("Select GroupID, GroupName from AccountGroup where GroupName= @Group", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.Parameters.Add("@Group", SqlDbType.VarChar).Value = ddGroup.SelectedValue;

        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            string grp = dr[0].ToString();
            lblGroup.Text = grp;

            if (grp == "04")
            {
                bothpanel.Visible = true;
            }
            else
            {
                bothpanel.Visible = false;
            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
    }
    private void popSubID()
    {
        SqlCommand cmd = new SqlCommand("Select AccountsID, AccountsName from Accounts where AccountsName= @Acc", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.Parameters.Add("@Acc", SqlDbType.VarChar).Value = ddSub.SelectedValue;

        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            lblSub.Text = dr[0].ToString();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
    }
    private void popControlID()
    {
        SqlCommand cmd = new SqlCommand("Select * from ControlAccount where ControlAccountsName= @Control", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.Parameters.Add("@Control", SqlDbType.VarChar).Value = ddControl.SelectedValue;

        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            lblControl.Text = dr[0].ToString();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
    }

    //Generating Acc Head ID
    private string populateHeadID()
    {
        SqlConnection cnn = new SqlConnection();
        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        //SqlCommand cmd = new SqlCommand("Select max(convert(int, right(ControlAccountsID, 3)) + 1) from ControlAccount where ControlAccountsID=@ControlID", cnn);
        SqlCommand cmd = new SqlCommand("Select ISNULL(max(EntryID),0) from HeadSetup", cnn);
        cmd.Parameters.Add("@ControlID", SqlDbType.VarChar).Value = lblControl.Text;

        cnn.Open();
        Int32 maxID = (Int32)cmd.ExecuteScalar();
        maxID += 1;
        string accID = lblControl.Text;
        if (maxID.ToString().Length < 2)
        {
            accID = accID + "00" + maxID;
        }
        else if (maxID.ToString().Length < 3)
        {
            accID = accID + "0" + maxID;
        }
        else
        {
            accID = accID + maxID;
        }
        txtHeadID.Text = accID;
        return accID;
    }

    /// <summary>
    /// Load Combo Boxes ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        popGID();
        populateSub();
        popSubID();
        populateControl();
        popControlID();
        populateHeadID();
    }

    protected void ddSub_SelectedIndexChanged(object sender, EventArgs e)
    {
        popSubID();
        populateControl();
        popControlID();
        populateHeadID();
    }

    protected void ddControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        popControlID();
        populateHeadID();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtHeadName.Text != "" && txtOpBalance.Text != "")
            {
                string accID = populateHeadID();
                //Create Sql Connection
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

                //Create Sql Command
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO HeadSetup (GroupID, AccountsID, ControlAccountsID, AccountsHeadID, AccountsHeadName, AccountsOpeningBalance, EntryDate, Emark)" +
                                        "VALUES (@GroupID, @AccountsID, @ControlAccountsID, @AccountsHeadID, @AccountsHeadName, @AccountsOpeningBalance, @EntryDate, @Emark)";
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
                param[6] = new SqlParameter("@EntryDate", SqlDbType.DateTime);
                param[7] = new SqlParameter("@Emark", SqlDbType.VarChar);

                param[0].Value = lblGroup.Text;
                param[1].Value = lblSub.Text;
                param[2].Value = lblControl.Text;
                param[3].Value = accID;
                param[4].Value = txtHeadName.Text;
                param[5].Value = txtOpBalance.Text;
                param[6].Value = txtDate.Text;

                if (RadioButton1.Checked == true)
                {
                    param[7].Value = "HO";
                }
                else
                {
                    param[7].Value = "Both";
                }


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
                        /*
                        SqlCommand cmd2y = new SqlCommand("INSERT INTO VoucherDetails (VoucherNo, VoucherRowDescription, AccountsHeadID, AccountsHeadName, VoucherDR, VoucherCR, EntryDate)" +
                                                    "VALUES (@VoucherNo, @VoucherRowDescription, @AccountsHeadID, @AccountsHeadName, @VoucherDR, @VoucherCR, @EntryDate)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                        cmd2y.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = "0000";
                        cmd2y.Parameters.Add("@VoucherRowDescription", SqlDbType.VarChar).Value = "Opening Balance";
                        cmd2y.Parameters.Add("@AccountsHeadID", SqlDbType.VarChar).Value = accID;
                        cmd2y.Parameters.Add("@AccountsHeadName", SqlDbType.VarChar).Value = txtHeadName.Text;

                        decimal dr; decimal cr;
                        if (Convert.ToDecimal(txtOpBalance.Text) > 0)
                        {
                            dr = Convert.ToDecimal(txtOpBalance.Text);
                            cr = 0;
                        }
                        else
                        {
                            dr = 0;
                            cr = Convert.ToDecimal(txtOpBalance.Text) * -1M;
                        }

                        cmd2y.Parameters.Add("@VoucherDR", SqlDbType.Decimal).Value = dr;
                        cmd2y.Parameters.Add("@VoucherCR", SqlDbType.Decimal).Value = cr;
                        cmd2y.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = txtDate.Text;

                        cmd2y.Connection.Open();
                        cmd2y.ExecuteNonQuery();
                        cmd2y.Connection.Close();
                         */

                        lblMsg.Text = "New Accounts Head Successfully Added";
                        txtHeadName.Text = "";
                        txtOpBalance.Text = "0";
                        GridView2.DataBind();
                        MessageBox("New Accounts Head Successfully Added");
                    }
                }
                else
                {
                    lblMsg.CssClass = "isa_error";
                    lblMsg.Text = "Please setup the control account first under <b>'" + ddSub.SelectedValue + "'</b> Sub Account";
                    MessageBox("Please setup the control head for " + ddSub.SelectedValue + " ");
                }

            }
            else
            {
                lblMsg.Text = "Please Enter the Name of the Head";
            }


        }
        catch (Exception ex)
        {
            lblMsg.Text = "ERROR: " + ex.ToString();
            MessageBox("Error Occured! Check Error Message...");

        }


    }

}
