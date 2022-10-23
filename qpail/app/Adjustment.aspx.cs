using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class app_Adjustment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            //txtDate.Text = DateTime.Today.Date.ToShortDateString();
            txtDate.Text = DateTime.Today.Date.ToShortDateString();
            //txtDate.Focus();
            //PopulateMember();
            //MemberDetails();
            //txtInsNo.Text = "1";

            //Get current Balance
            ddParties.DataBind();
            GetBalance();
        }
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

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCollection.Text != "")
            {
                SaveCollection();
                GetBalance();

                txtCollection.Text = "0";
                txtDetail.Text = "";
                GridView1.DataBind();
                lblMsg.Text = "Adjustment Entry Saved Successfully.";
                Notify("Adjustment Entry Saved Successfully.","success",lblMsg);
            }
            else
            {
                Notify("Saving failed...", "error", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }

    private void SaveCollection()
    {
        string adjDate =Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        string isAppr = "P";

        string colID = AdjustmentIDNo();
        decimal amtC = Convert.ToDecimal(ltrBalance.Text);
        decimal amtD = Convert.ToDecimal(txtCollection.Text);
        decimal adjAmt = amtD - amtC;

        SqlCommand cmd2 = new SqlCommand("INSERT INTO Adjustment (AdjustmentDate, PartyName, CurrentBalance, NewBalance, PaidAmount, AdjustmentNo, EntryBy, ColType, ChqDetail, ChqDate, IsApproved)" +
                                    "VALUES (@AdjustmentDate, @PartyName, '" + amtC + "','" + amtD + "', @PaidAmount, @AdjustmentNo, @EntryBy, @ColType, @ChqDetail, @ChqDate, @IsApproved)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        string lName = Page.User.Identity.Name.ToString();

        cmd2.Parameters.Add("@AdjustmentDate", SqlDbType.DateTime).Value = adjDate; //DateTime.Now.ToShortDateString();
        cmd2.Parameters.Add("@PartyName", SqlDbType.VarChar).Value = ddParties.SelectedValue;

        cmd2.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = adjAmt;
        cmd2.Parameters.Add("@AdjustmentNo", SqlDbType.VarChar).Value = colID;
        cmd2.Parameters.Add("@EntryBy", SqlDbType.VarChar).Value = lName.Trim();

        cmd2.Parameters.Add("@ColType", SqlDbType.VarChar).Value = "";
        cmd2.Parameters.Add("@ChqDetail", SqlDbType.VarChar).Value = txtDetail.Text;
        cmd2.Parameters.Add("@ChqDate", SqlDbType.DateTime).Value = DateTime.Now; //txtDate.Text;
        cmd2.Parameters.Add("@IsApproved", SqlDbType.VarChar).Value = isAppr;

        cmd2.Connection.Open();
        int success = cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        if (adjAmt > 0)
            {
                amtD = adjAmt;
                amtC = 0;
            }
            else
            {
                amtD = 0;
                amtC = adjAmt * (-1);
            }
            
            Accounting.VoucherEntry.TransactionEntry(colID, adjDate, ddParties.SelectedValue, ddParties.SelectedItem.Text, "" + txtDetail.Text, amtD.ToString(), amtC.ToString(), "0", "Adjustment", "Supplier", "010100000", lName, "1");
          
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (DropDownList1.SelectedValue == "Cash")
        //{
        //    chqpanel.Visible = false;
        //}
        //else
        //{
        //    chqpanel.Visible = true;
        //}
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        GridView1.DataSource = null;
        SqlDataAdapter da;
        SqlDataReader dr;
        DataSet ds;
        int recordcount = 0;
        int ic = 0;

        SqlCommand cmd2z = new SqlCommand("SELECT [ColType], [PartyName], [ChqDetail], [ChqDate], [PaidAmount] FROM [Collection] WHERE ([CollectionDate] = @CollectionDate) ORDER BY [AdjustmentID] DESC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2z.Parameters.Add("@CollectionDate", SqlDbType.DateTime).Value = txtDate.Text;

        da = new SqlDataAdapter(cmd2z);
        ds = new DataSet("Board");

        cmd2z.Connection.Open();
        da.Fill(ds, "Board");
        cmd2z.Connection.Close();

        DataTable dt1 = ds.Tables["Board"];

        GridView1.DataSource = dt1;
        GridView1.DataBind();
    }
    // Getting CollectionNo
    public string AdjustmentIDNo()
    {
        SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (count(AdjustmentID),0)+1001 )) from Adjustment", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        string CollectionNo = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();
        cmd.Connection.Dispose();
        return "Adj-" + CollectionNo;
    }

    private void GetBalance()
    {
        ltrBalance.Text = Convert.ToString(Accounting.VoucherEntry.SupplierBalance(ddParties.SelectedValue));
        }

    protected void ddParties_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetBalance();
    }
}
