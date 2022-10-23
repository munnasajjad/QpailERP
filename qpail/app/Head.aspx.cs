using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


public partial class Application_Head : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string month = DateTime.Now.Month.ToString();
            if (month.Length<2)
            {
                month = "0" + month;
            }
            txtDate.Text = "01/" + month+"/" + DateTime.Now.Year;

            populateGroup();
            populateSub();
            //popSubID();
            populateControl();
            //popControlID();
            populateHeadID();

            AutoSelectDrCr();
            txtOpBalance.Text = "0";
            txtQty.Text = "0";
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


    protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        populateSub();
        populateControl();
        populateHeadID();
        AutoSelectDrCr();
    }

    protected void ddSub_SelectedIndexChanged(object sender, EventArgs e)
    {
        populateControl();
        populateHeadID();
        AutoSelectDrCr();
    }

    protected void ddControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        populateHeadID();
        AutoSelectDrCr();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string eMark = "S";
            if (RadioButton1.Checked == true)
            {
                eMark = "U";
            }

            decimal bldr = Convert.ToDecimal(txtOpBalance.Text);
            decimal blcr = Convert.ToDecimal(txtOpBalance.Text);
            decimal sbldr = Convert.ToDecimal(txtQty.Text);
            decimal sblcr = Convert.ToDecimal(txtQty.Text);

            decimal sdrPcs = Convert.ToInt32(txtPcs.Text);
            decimal scrPcs = Convert.ToInt32(txtPcs.Text);

            if (RadioButton1.Checked)

            {
                blcr = 0;sblcr= 0; scrPcs = 0;
            }
            else
            {
                bldr = 0;sbldr= 0; sdrPcs = 0;
            }

            decimal isActive = 0;
            if (cbStatus.Checked)
            {
                isActive = 1;
            }

            if (btnSave.Text == "Update")
            {
                int EntryCount = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(SerialNo) from VoucherDetails where AccountsHeadID='" + lblHeadID.Text + "'"));

                if (EntryCount == 0)
                {
                    RunQuery.SQLQuery.ExecNonQry("Update HeadSetup set GroupID='" + ddGroup.SelectedValue + "', AccountsID='" + ddSub.SelectedValue + "', ControlAccountsID='" + ddControl.SelectedValue + "', AccountsHeadID='" + txtHeadID.Text + "', AccountsHeadName='" + txtHeadName.Text.Trim().Replace("  ", " ") + "', OpDate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', OpBalDr='" + bldr + "', OpBalCr='" + blcr + "', QtyDr='" + sbldr + "', QtyCr='" + sblcr + "', OpPcsDr='" + sdrPcs + "', OpPcsCr='" + scrPcs + "', Rate='" + txtRate.Text + "', isActive='" + isActive + "', Emark= '" + eMark + "' WHERE AccountsHeadID='" + lblHeadID.Text + "'");
                    lblMsg.CssClass = "xerp_success";
                    lblMsg.Text = "Accounts Head was updated successfully!";
                }
                else
                {
                    RunQuery.SQLQuery.ExecNonQry("Update HeadSetup set  GroupID='" + ddGroup.SelectedValue + "', AccountsID='" + ddSub.SelectedValue + "', ControlAccountsID='" + ddControl.SelectedValue + "', AccountsHeadName='" + txtHeadName.Text.Trim().Replace("  "," ") + "', OpDate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', OpBalDr='" + bldr + "', OpBalCr='" + blcr + "', QtyDr='" + sbldr + "', QtyCr='" + sblcr + "', OpPcsDr='" + sdrPcs + "', OpPcsCr='" + scrPcs + "', Rate='" + txtRate.Text + "', isActive='" + isActive + "', Emark= '" + eMark + "'  WHERE AccountsHeadID='" + lblHeadID.Text + "'");
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
                    cmd.CommandText = "INSERT INTO HeadSetup (GroupID, AccountsID, ControlAccountsID, AccountsHeadID, AccountsHeadName, AccountsOpeningBalance, OpDate, Emark, OpBalDr, OpBalCr,QtyDr, QtyCr, OpPcsDr, OpPcsCr, Rate)" +
                                      "VALUES (@GroupID, @AccountsID, @ControlAccountsID, @AccountsHeadID, @AccountsHeadName, @AccountsOpeningBalance, @OpDate, @Emark, @OpBalDr, @OpBalCr,@QtyDr, @QtyCr, @OpPcsDr, @OpPcsCr, @Rate)";
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
                    param[4].Value = txtHeadName.Text.Trim().Replace("  ", " ");
                    param[5].Value = txtOpBalance.Text;
                    param[6].Value = txtDate.Text;


                    decimal drAmount = 0, drQty=0,crQty=0;
                    decimal crAmount = 0, drPcs=0, crPcs=0;

                    if (RadioButton1.Checked == true)
                    {
                        drAmount = Convert.ToDecimal(txtOpBalance.Text);
                        drQty = Convert.ToDecimal(txtQty.Text);
                        drPcs = Convert.ToInt32(txtPcs.Text);
                    }
                    else
                    {
                        crAmount = Convert.ToDecimal(txtOpBalance.Text);
                        crQty = Convert.ToDecimal(txtQty.Text);
                        crPcs = Convert.ToInt32(txtPcs.Text);
                    }

                    cmd.Parameters.Add("@OpBalDr", SqlDbType.Decimal).Value = drAmount;
                    cmd.Parameters.Add("@OpBalCr", SqlDbType.Decimal).Value = crAmount;
                    cmd.Parameters.Add("@QtyDr", SqlDbType.Decimal).Value = drQty;
                    cmd.Parameters.Add("@QtyCr", SqlDbType.Decimal).Value = crQty;//QtyDr, QtyCr

                    cmd.Parameters.Add("@OpPcsDr", SqlDbType.Decimal).Value = drPcs;
                    cmd.Parameters.Add("@OpPcsCr", SqlDbType.Decimal).Value = crPcs;
                    cmd.Parameters.Add("@Rate", SqlDbType.Decimal).Value = txtRate.Text;

                    //if (RadioButton1.Checked == true)
                    //{
                    //    param[7].Value = "HO";
                    //}
                    //else
                    //{
                    param[7].Value = eMark;
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
                            txtHeadName.Text = "";
                            txtOpBalance.Text = "0";
                            txtRate.Text = "0";
                            txtQty.Text = "0";
                            txtPcs.Text = "0";
                            GridView2.DataBind();
                            populateHeadID();
                            Notify("New Accounts Head Successfully Added", "success", lblMsg);
                            txtHeadName.Focus();
                        }
                    }
                    else
                    {
                        string msg = "Please setup the control account first under <b>'" + ddSub.SelectedValue + "'</b> Sub Account";
                        Notify(msg, "error", lblMsg);
                    }

                }
                else
                {
                    Notify("Please Enter the Name of the Head", "error", lblMsg);
                }
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
            Label lblItemCode = GridView2.Rows[index].FindControl("Label2") as Label;

            int EntryCount = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(SerialNo) from VoucherDetails where AccountsHeadID='" + lblItemCode.Text + "'"));

            if (EntryCount <= 0)
            {
                RunQuery.SQLQuery.ExecNonQry("DELETE HeadSetup WHERE AccountsHeadID=" + lblItemCode.Text);

                GridView2.DataBind();

                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "A/C Head deleted successfully ...";

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

    }

    protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView2.SelectedIndex);
            Label Label2 = GridView2.Rows[index].FindControl("Label2") as Label;
            txtHeadID.Text = Label2.Text;
            lblHeadID.Text = Label2.Text;

            SqlCommand cmd7 = new SqlCommand("SELECT GroupID, AccountsID, ControlAccountsID, AccountsHeadName, OpDate, OpBalDr, OpBalCr, IsActive, Emark, QtyDr, QtyCr, OpPcsDr, OpPcsCr, Rate  FROM [HeadSetup] WHERE AccountsHeadID ='" + Label2.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
                    txtQty.Text = dr[9].ToString();
                    txtPcs.Text = dr[11].ToString();
                }
                else
                {
                    txtOpBalance.Text = blcr.ToString();
                    txtQty.Text = dr[10].ToString();
                    txtPcs.Text = dr[12].ToString();
                }

                decimal isActive = Convert.ToDecimal(dr[7].ToString());

                if (isActive == 1)
                {
                    cbStatus.Checked = true;
                }
                else
                {
                    cbStatus.Checked = false;
                }

                string status = dr[8].ToString();

                if (status=="U")
                {
                    RadioButton1.Checked = true;
                    RadioButton2.Checked = false;
                }
                else
                {
                    RadioButton2.Checked = true;
                    RadioButton1.Checked = false;
                }
                txtRate.Text = dr[13].ToString();
                lblMsg.Attributes.Add("class", "xerp_info");
                lblMsg.Text = "A/C info loaded in edit mode";
            }

            cmd7.Connection.Close();

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
        lblMsg.CssClass = "xerp_info";
        lblMsg.Text = "Entry form was Form Refreshed!";
    }
    private void ClearForm()
    {
        //populateGroup();
        //populateSub();
        //populateControl();
        populateHeadID();
        //txtDate.Text = DateTime.Now.ToShortDateString();
        txtOpBalance.Text = "0";
        btnSave.Text = "Save New Head";
        txtHeadName.Text = "";

        GridView2.DataBind();

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

    private void AutoSelectDrCr()
    {
        if (ddGroup.SelectedValue == "01" || ddGroup.SelectedValue == "03")
        {
            RadioButton2.Checked = false;
            RadioButton1.Checked = true;
        }
        else
        {
            RadioButton1.Checked = false;
            RadioButton2.Checked = true;
        }
    }
}
