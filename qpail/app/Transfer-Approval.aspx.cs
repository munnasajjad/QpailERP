using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Cells_Transfer_Approval : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Get Branch/Centre Name
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmdxx = new SqlCommand("Select BranchName from Users where Username='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdxx.Connection.Open();
        string branch = Convert.ToString(cmdxx.ExecuteScalar());
        cmdxx.Connection.Close();
        cmdxx.Connection.Dispose();
        lblBranch.Text = branch;
    }

    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    // Getting Pending transfers    
    private void generateData()
    {
        SqlCommand cmd = new SqlCommand("Select TransferDate, TransferFrom, TransferAmount, Description from Transfers where TransferInvoiceID=@InvNo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.Parameters.Add("@InvNo", SqlDbType.VarChar).Value = ddType.SelectedValue;
        //cmd.Parameters.Add("@PinNo", SqlDbType.VarChar).Value = txtPin.Text;

        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            string date = dr[0].ToString();
            string Sender = dr[1].ToString();
            string amount = dr[2].ToString();
            string desc = dr[3].ToString();
            InvNo.Text = ddType.SelectedValue;

            ddBranch.SelectedValue = Sender;
            txtDescription.Text = desc + ". Sent Date: " + date;
            txtAmount.Text = amount;

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        else
        {
            lblMsg.Text = "Invalid Transfer";
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        saveData();
        ddType.DataBind();
    }
    private void saveData()
    {
        try
        {
            if (ddType.SelectedValue == InvNo.Text)
            {
                string lName = Page.User.Identity.Name.ToString();
                //Get current Balance
                SqlCommand cmdi = new SqlCommand("SELECT isnull(SUM(Dr),0) - isnull(SUM(Cr),0) FROM Transactions WHERE HeadName =(Select BranchName from Users where Username='" + lName + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmdi.Connection.Open();
                decimal cBalance = Convert.ToDecimal(cmdi.ExecuteScalar());
                cmdi.Connection.Close();
                cmdi.Connection.Dispose();

                //Inactive the order
                SqlCommand cmd = new SqlCommand("update Transfers set IsApproved='A' WHERE TransferInvoiceID =@InvNo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Parameters.Add("@InvNo", SqlDbType.VarChar).Value = InvNo.Text;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                //insert op balance        
                SqlCommand cmd2 = new SqlCommand("INSERT INTO Transactions (HeadName, Description, Dr, Balance,  EntryBy)" +
                                                    "VALUES (@HeadName, @Description, @Cr, @Balance, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = lblBranch.Text;
                cmd2.Parameters.Add("@Description", SqlDbType.VarChar).Value = "Cash Transfer ID: " + InvNo.Text;
                cmd2.Parameters.Add("@Cr", SqlDbType.Decimal).Value = txtAmount.Text;
                cmd2.Parameters.Add("@Balance", SqlDbType.Decimal).Value = cBalance - Convert.ToDecimal(txtAmount.Text);
                cmd2.Parameters.Add("@EntryBy", SqlDbType.VarChar).Value = lName;

                cmd2.Connection.Open();
                int success2 = cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
                MessageBox("Transfer Approval successfull");
            }
            else
            { MessageBox("Failed to initiate the transfer order! Save Failed."); }
        }
        catch (Exception ex)
        {
            lblErrLoad.Text = ex.ToString();
            MessageBox("Failed to Approve The Transfer Order!");
        }
    }
    protected void txtLoad_Click(object sender, EventArgs e)
    {
        generateData();
    }
         
}
