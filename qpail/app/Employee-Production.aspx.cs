using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class app_Employee_Production : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");        

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Today.Date.ToShortDateString();
            string lName = Page.User.Identity.Name.ToString();
            lblProject.Text = RunQuery.SQLQuery.ReturnString("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'");
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string mpo = Page.User.Identity.Name.ToString();
            mpo = mpo.Trim();

            //if (Convert.ToDecimal(txtPaid.Text) > 0)
            //{
            SavePayment();
            GridView1.DataBind();
            //    MessageBox("Payment Entry Saves Successfully.");
            //}
            //else
            //{
            //    lblMsg.Text = "Please Check Payment Amount Properly";
            //    lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#6e000a");
            //}
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_warning");
            lblMsg.Text = "Error: " + ex.ToString();
        }
    }

    private void SavePayment()
    {
        if (txtFinalProd.Text == "")
        {
            txtFinalProd.Text = "0";
        }
        string lName = Page.User.Identity.Name.ToString();
        string prdId = RunQuery.SQLQuery.ReturnString("Select 'PRD-'+ CONVERT(varchar, (ISNULL (max(pid),0)+1001 )) from Production");

        SqlCommand cmd2 = new SqlCommand("INSERT INTO Production (ProductionID, Date, SectionID, SectionName, MachineNo, LineNumber, SizeID, Purpose, ItemID, ItemName, Color, Process, CustomerID, CustomerName, Operation, OperatorID, Hour, Production, Rejection, TimeWaste, ReasonWaist, Shift, FinalProduction, EntryBy) VALUES (@ProductionID, @Date, @SectionID, @SectionName, @MachineNo, @LineNumber, @SizeID, @Purpose, @ItemID, @ItemName, @Color, @Process, @CustomerID, @CustomerName, @Operation, @OperatorID, @Hour, @Production, @Rejection, @TimeWaste, @ReasonWaist, @Shift, @FinalProduction, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductionID", prdId);
        cmd2.Parameters.AddWithValue("@Date", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd")); 
        cmd2.Parameters.AddWithValue("@SectionID", ddSection.SelectedValue);
        cmd2.Parameters.AddWithValue("@SectionName", ddSection.SelectedItem.Text);

        cmd2.Parameters.AddWithValue("@MachineNo", ddMachine.SelectedValue);
        cmd2.Parameters.AddWithValue("@LineNumber", ddLine.SelectedValue);
        cmd2.Parameters.AddWithValue("@SizeID", ddSize.SelectedValue);
        cmd2.Parameters.AddWithValue("@Purpose", txtPurpose.Text);
        cmd2.Parameters.AddWithValue("@ItemID", ddProduct.SelectedValue);
        cmd2.Parameters.AddWithValue("@ItemName", ddProduct.SelectedItem.Text);

        cmd2.Parameters.AddWithValue("@Color",ddColor.SelectedValue);
        cmd2.Parameters.AddWithValue("@Process", ddProcess.SelectedValue);
        cmd2.Parameters.AddWithValue("@CustomerID", txtFor.Text);
        cmd2.Parameters.AddWithValue("@CustomerName", txtFor.Text);
        cmd2.Parameters.AddWithValue("@Operation", ddOperation.SelectedValue);
        cmd2.Parameters.AddWithValue("@OperatorID", ddOperator.SelectedValue);
        cmd2.Parameters.AddWithValue("@Hour", txtHour.Text);

        cmd2.Parameters.AddWithValue("@Production", txtproduced.Text);
        cmd2.Parameters.AddWithValue("@Rejection", txtRejected.Text);
        cmd2.Parameters.AddWithValue("@TimeWaste", Convert.ToInt32(txtTimeWaist.Text));
        cmd2.Parameters.AddWithValue("@ReasonWaist", txtReason.Text);
        cmd2.Parameters.AddWithValue("@Shift", ddShift.SelectedValue);
        cmd2.Parameters.AddWithValue("@FinalProduction", txtFinalProd.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        int finalProd = Convert.ToInt32(txtFinalProd.Text);
        if (finalProd > 0)
        {
            //Accounting.VoucherEntry.ProductionStockEntry(prdId, "Processed items stock-in from production", ddSize.SelectedValue, txtFor.Text, ddProduct.SelectedValue, ddProduct.SelectedItem.Text, ddSection.SelectedValue, "3", txtproduced.Text, "0", txtRemark.Text, "Production", "Processed", "", lName);
            //Accounting.VoucherEntry.ProductionStockEntry(prdId, "Wastage items stock-in from production", ddSize.SelectedValue, txtFor.Text, ddProduct.SelectedValue, ddProduct.SelectedItem.Text, ddSection.SelectedValue, "3", txtRejected.Text, "0", txtRemark.Text, "Production", "Wastage", "", lName);
        }
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddMode.SelectedValue == "Cheque")
        //{
        //    chqpanel.Visible = true;
        //}
        //else
        //{
        //    chqpanel.Visible = false;
        //    txtPaid.Focus();
        //}
    }
    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddOperator.DataBind();
    }
}
