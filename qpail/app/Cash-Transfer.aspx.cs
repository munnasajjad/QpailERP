using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Cells_Cash_Transfer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            PopulateInv();
            //generateData();
            //txtDate.Focus();
        }
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

        // Getting Pending order Lists
    public void PopulateInv()
    {
        //string lName = Page.User.Identity.Name.ToString();
        //SqlCommand cmd = new SqlCommand("SELECT OrderStatement FROM Orders where IsActive='true' and ReceiverBranchID=(Select BranchName from Users where Username='" + lName + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmd.Connection.Open();
        //SqlDataReader Reflist = cmd.ExecuteReader();

        //ddInvNo.DataSource = Reflist;
        //ddInvNo.DataValueField = "OrderStatement";
        //ddInvNo.DataTextField = "OrderStatement";
        //ddInvNo.DataBind();

        //cmd.Connection.Close();
        //cmd.Connection.Dispose();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //Get Branch/Centre Name
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmdxx = new SqlCommand("Select BranchName from Users where Username='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdxx.Connection.Open();
        string branch = Convert.ToString(cmdxx.ExecuteScalar());
        cmdxx.Connection.Close();
        cmdxx.Connection.Dispose();

        //Get current Balance
        SqlCommand cmd = new SqlCommand("SELECT isnull(SUM(Dr),0) - isnull(SUM(Cr),0) FROM Transactions WHERE HeadName =(Select BranchName from Users where Username='" + lName + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        decimal cBalance = Convert.ToDecimal(cmd.ExecuteScalar());
        cmd.Connection.Close();
        cmd.Connection.Dispose();
        
        try
            {
                decimal trAmount = Convert.ToDecimal(txtAmount.Text);      
            if (cBalance > trAmount)
            {
             //Get Branch/Centre Name
                SqlCommand cmdxx2 = new SqlCommand("Select ISNULL(max(TransferID),1000) from Transfers", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmdxx2.Connection.Open();
                int trIDi = Convert.ToInt32(cmdxx2.ExecuteScalar()) + 1000;
                cmdxx2.Connection.Close();
                cmdxx2.Connection.Dispose();
                string trID = "TR-" + DateTime.Now.Year.ToString() + "-" + trIDi;

                //insert transaction       
                SqlCommand cmd2x = new SqlCommand("INSERT INTO Transactions (HeadName, Description, Cr, Balance,  EntryBy)" +
                                                    "VALUES (@HeadName, @Description, @Cr, @Balance, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2x.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = branch;
                cmd2x.Parameters.Add("@Description", SqlDbType.VarChar).Value = "Cash Transfer ID: " + trID;
                cmd2x.Parameters.Add("@Cr", SqlDbType.Decimal).Value = txtAmount.Text;
                cmd2x.Parameters.Add("@Balance", SqlDbType.Decimal).Value = cBalance - Convert.ToDecimal(txtAmount.Text);
                cmd2x.Parameters.Add("@EntryBy", SqlDbType.VarChar).Value = lName;

                cmd2x.Connection.Open();
                int successx = cmd2x.ExecuteNonQuery();
                cmd2x.Connection.Close();

                //insert Transfer      
                SqlCommand cmd2 = new SqlCommand("INSERT INTO Transfers (TransferFrom, TransferTo, TransferAmount, Description,  TransferBy, TransferInvoiceID)" +
                                                    "VALUES (@HeadName, @Description, @Cr, @Balance, @EntryBy, @TransferInvoiceID)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = branch;
                cmd2.Parameters.Add("@Description", SqlDbType.VarChar).Value = ddBranch.SelectedValue;
                cmd2.Parameters.Add("@Cr", SqlDbType.Decimal).Value = trAmount;
                cmd2.Parameters.Add("@Balance", SqlDbType.VarChar).Value = txtDescription.Text;
                cmd2.Parameters.Add("@EntryBy", SqlDbType.VarChar).Value = lName;
                cmd2.Parameters.Add("@TransferInvoiceID", SqlDbType.VarChar).Value = trID;

                cmd2.Connection.Open();
                int success2 = cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();

                MessageBox("SAVED! The Transfer will be done after approval from " + ddBranch.SelectedValue + " Branch");
            }
            
            
        else
        {
            MessageBox("Unsufficient Balance to Process The Request."); 
        }
            }
        catch (Exception ex)
        {
            MessageBox("Please fillup All Required Fields!");
        }
    }


}
