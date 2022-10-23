using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class AdminCentral_Credit_Approval : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            try
            {
                PopulateInv();
                populateRequest();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "No Data Was Returned!";
            }
        }
    }
    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    public void PopulateInv()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT [AgentName] FROM [Credit4Approval] WHERE ([IsApproved] = 'false') ORDER BY [CLID]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader Reflist = cmd.ExecuteReader();

        ddBranch.DataSource = Reflist;
        ddBranch.DataValueField = "AgentName";
        ddBranch.DataTextField = "AgentName";
        ddBranch.DataBind();

        cmd.Connection.Close();
        cmd.Connection.Dispose();
    }
    private void populateRequest()
    {
        string branch = ddBranch.SelectedValue;
        //Get current Balance
        SqlCommand cmdb = new SqlCommand("SELECT RequestedAmt FROM Credit4Approval WHERE AgentName ='" + branch + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdb.Connection.Open();
        decimal bal = Convert.ToDecimal(cmdb.ExecuteScalar());
        cmdb.Connection.Close();
        cmdb.Connection.Dispose();

        if (bal > 0)
        {
            txtAmount.Text = bal.ToString();
        }
        else if (ddBranch.SelectedValue=="")
        {
            txtAmount.Text = "0";
        }
    }



    protected void ddBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        populateRequest();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ddBranch.SelectedValue != "")
        {
            //Credit Limit To Pending List
            SqlCommand cmd2r = new SqlCommand("UPDATE Credit4Approval SET ApprovedBy=@ApprovedBy, ApprovedAmt=@ApprovedAmt, IsApproved='true' where AgentName=@AgentName and IsApproved='false'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2r.Parameters.Add("@AgentName", SqlDbType.VarChar).Value = ddBranch.SelectedValue;
            cmd2r.Parameters.Add("@ApprovedAmt", SqlDbType.Decimal).Value = txtAmount.Text;
            cmd2r.Parameters.Add("@ApprovedBy", SqlDbType.VarChar).Value = Page.User.Identity.Name.ToString();

            cmd2r.Connection.Open();
            cmd2r.ExecuteNonQuery();
            cmd2r.Connection.Close();

            SqlCommand cmd2r1 = new SqlCommand("UPDATE Branches SET CreditLimit=@CreditLimit where BranchName=@BranchName", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2r1.Parameters.Add("@BranchName", SqlDbType.VarChar).Value = ddBranch.SelectedValue;
            cmd2r1.Parameters.Add("@CreditLimit", SqlDbType.Decimal).Value = txtAmount.Text;

            cmd2r1.Connection.Open();
            cmd2r1.ExecuteNonQuery();
            cmd2r1.Connection.Close();

            PopulateInv();
            MessageBox("Successfully Activated The Credit Limit for "+ ddBranch.SelectedValue);
        }
        else
        {
            MessageBox("Please Select The Centre Name Properly!");
        }
    }
}
