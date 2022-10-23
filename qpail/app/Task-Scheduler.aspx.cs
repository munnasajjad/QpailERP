using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class app_Task_Scheduler : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToShortDateString();
            txtEDate.Text = DateTime.Now.ToShortDateString();
        }
    }

    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox)
        { (Parent as TextBox).Text = string.Empty; }
        else
        {
            foreach (Control c in Parent.Controls)
                ClearControls(c);
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        //if (txtName.Text != "")
        //{
        try
        {
            saveData();
            GridView1.DataBind();
            MessageBox("Successfully posted the Task info.");
        }
        catch (Exception ex)
        {
            lblMsg.Text = "ERROR! " + ex.ToString();
            lblMsg.CssClass = "isa_error";
            MessageBox("Faled to saved the Task info!");
        }

        //}
        //else
        //{
        //    lblMsg.Text = "ERROR! Company Name Should not be empty!";
        //    lblMsg.CssClass = "isa_error";
        //}

    }
    private void saveData()
    {
        SqlCommand cmd2 = new SqlCommand("INSERT INTO Campaigns (StartDate, Subject, ProjectInterested, CampaignDetails, Remarks, ReqStatus, IsClosed, EntryBy, Department, EndDate)" +
                                            "VALUES (@ReqDate, @CustomerName, @ProjectInterested, @RequirementDetails, @ContactNo, @ReqStatus, @IsClosed, @EntryBy, @Department, @EndDate)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.Add("@ReqDate", SqlDbType.DateTime).Value = txtDate.Text;
        cmd2.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = ddCustomer.SelectedValue;
        cmd2.Parameters.Add("@ProjectInterested", SqlDbType.VarChar).Value = ddProject.SelectedValue;
        cmd2.Parameters.Add("@RequirementDetails", SqlDbType.VarChar).Value = txtDetails.Text;
        cmd2.Parameters.Add("@ContactNo", SqlDbType.VarChar).Value = txtContact.Text;
        cmd2.Parameters.Add("@ReqStatus", SqlDbType.VarChar).Value = "Opened";
        cmd2.Parameters.Add("@IsClosed", SqlDbType.Int).Value = 0;
        cmd2.Parameters.Add("@EntryBy", SqlDbType.VarChar).Value = Page.User.Identity.Name.ToString();
        cmd2.Parameters.Add("@Department", SqlDbType.VarChar).Value = ddDepartment.SelectedValue;
        cmd2.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = txtEDate.Text;

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        //SqlCommand cmd2x = new SqlCommand("UPDATE Company set VarComapnyName=@VarComapnyName, VarCompayAddress=@VarCompayAddress where VarComapnyName=@VarComapnyName", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //cmd2x.Parameters.Add("@VarComapnyName", SqlDbType.VarChar).Value = txtCompanyName.Text;
        //cmd2x.Parameters.Add("@VarCompayAddress", SqlDbType.VarChar).Value = txtAddress.Content;

        //cmd2x.Connection.Open();
        //cmd2x.ExecuteNonQuery();
        //cmd2x.Connection.Close();
    }



    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearControls(Page);
    }
}