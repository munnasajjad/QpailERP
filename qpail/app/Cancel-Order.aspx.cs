using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class AdminCentral_Cancel_Order : System.Web.UI.Page
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
    }

    // Getting Pending order Lists
    public void PopulateInv()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("SELECT OrderStatement FROM Orders where IsActive='true'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader Reflist = cmd.ExecuteReader();

        ddInvNo.DataSource = Reflist;
        ddInvNo.DataValueField = "OrderStatement";
        ddInvNo.DataTextField = "OrderStatement";
        ddInvNo.DataBind();

        cmd.Connection.Close();
        cmd.Connection.Dispose();
    }
    // Getting Pending order Lists

    private void generateData()
    {
        SqlCommand cmd = new SqlCommand("Select TransactionType, PuchaseDate, PurchaseFrom, SenderAddress, SenderMobile, PurchaseTaka, SalesTo, ReceiverAddress, ReceiverMobile, ServiceCharge, ToBePaid, Collected, SenderBranchID, InvNo, Description from Orders where OrderStatement=@InvNo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.Parameters.Add("@InvNo", SqlDbType.VarChar).Value = ddInvNo.SelectedValue;
        cmd.Parameters.Add("@PinNo", SqlDbType.VarChar).Value = txtPin.Text;

        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            string type = dr[0].ToString();
            string pDate = dr[1].ToString();
            string Sender = dr[2].ToString();
            string sAddress = dr[3].ToString();
            string sMobile = dr[4].ToString();
            string sentAmount = dr[5].ToString();

            string receiver = dr[6].ToString();
            string rAddress = dr[7].ToString();
            string rMobile = dr[8].ToString();
            decimal serviceCharge = Convert.ToDecimal(dr[9].ToString());
            string toBePaid = dr[10].ToString();

            string collected = dr[11].ToString();
            string senderBranch = dr[12].ToString();
            InvNo.Text = dr[13].ToString();
            string description = dr[14].ToString();

            txtSender.Text = Sender;
            txtSMobile.Text = sMobile;
            txtReceiver.Text = receiver;
            txtRMobile.Text = rMobile;

            //ddCashHead.Text = dr[14].ToString();
            decimal due = Convert.ToDecimal(toBePaid) - Convert.ToDecimal(collected);
            txtPaid.Text = sentAmount.ToString();
            txtCollected.Text = due.ToString();
            txtDescription.Text = type + ", Sender:" + Sender + ", Mobile#" + sMobile + ", Branch:" + senderBranch + ", For:" + receiver + ". " + description;
            if (due > 0)
            {
                lblDue.Text = "DUE: " + due.ToString();
                Panel1.Visible = true;
                lblDue.ForeColor = System.Drawing.ColorTranslator.FromHtml("#eb0258");
            }
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        else
        {
            lblMsg.Text = "Invalid EPIN";
            MessageBox("Invalid PIN");
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //verify descripton with inv no.
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmdxx = new SqlCommand("Select InvNo from Orders where OrderStatement=@InvNo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdxx.Parameters.Add("@InvNo", SqlDbType.VarChar).Value = ddInvNo.SelectedValue;
        cmdxx.Connection.Open();
        string invID = Convert.ToString(cmdxx.ExecuteScalar());
        cmdxx.Connection.Close();
        cmdxx.Connection.Dispose();

        if (invID == InvNo.Text)
        {
            saveData();
            PopulateInv();
            //inactivateOrder();            
            //generateData();    
            MessageBox("Successfully Deleted the Booking Order.");
        }
        else
        {
            PopulateInv();
            MessageBox("Cancel Failed! Please Click the Load Button After Selecting Order.");
        }
        

    }
    private void saveData()
    {
        try
        {
            //Inactive the order
            SqlCommand cmd = new SqlCommand("UPDATE Orders SET IsActive='false', SmsStatus='C' WHERE InvNo =@InvNo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Parameters.Add("@InvNo", SqlDbType.VarChar).Value = InvNo.Text;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();

            //Delete Transaction
            string invID=InvNo.Text;
            SqlCommand cmd2 = new SqlCommand("Delete Transactions WHERE Description like '%"+invID+"%'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
            cmd2.Connection.Dispose();

        }
        catch (Exception ex)
        {
            lblMsg.Text=ex.ToString();
        }
    }
    private void inactivateOrder()
    {
        SqlCommand cmd3 = new SqlCommand("UPDATE Orders SET IsActive='false' WHERE InvNo=@DID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd3.Parameters.Add("@DID", SqlDbType.VarChar).Value = InvNo.Text;

        cmd3.Connection.Open();
        cmd3.ExecuteNonQuery();
        cmd3.Connection.Close();
    }


    protected void ddInvNo_SelectedIndexChanged1(object sender, EventArgs e)
    {
        //generateData();
        //lblMsg.Text = "Data loaded";
    }

    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        // define a javascript alertbox containing the string passed in as argument

        // create a new label
        Label lbl = new Label();

        // add the javascript to fire an alertbox to the label and
        // add the string argument passed to the subroutine as the
        // message payload for the alertbox
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";

        // add the label to the page to display the alertbox
        Page.Controls.Add(lbl);
    }

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        Panel1.Visible = false;
        txtPaid.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0a6e00");
        generateData();
    }

}
