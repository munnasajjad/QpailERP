using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class app_Expenses : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtDate.Text = DateTime.Now.ToShortDateString();
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

    }

    string ExpenseNo;
    // Getting ExpenseID
    public void ExpenseIDNo()
    {
        SqlCommand cmd = new SqlCommand("Select 'E'+ CONVERT(varchar, (ISNULL (max(ExpenseEntryID),0)+1001 )) from Expenses", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        ExpenseNo = Convert.ToString(cmd.ExecuteScalar());

        cmd.Connection.Close();
        cmd.Connection.Dispose();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        ExpenseIDNo();
        if(txtAmount.Text!="0" & txtAmount.Text!="")
        {
            System.Threading.Thread.Sleep(1000);
            SqlCommand cmd2 = new SqlCommand("INSERT INTO Expenses (ExpDate, ExpHeadName, Description, Amount, Entryby, ExpenseID)" +
                                        "VALUES (@ExpDate, @ExpHeadName, @Description, @Amount, @Entryby, @ExpenseID)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            string lName = Page.User.Identity.Name.ToString();

            cmd2.Parameters.Add("@ExpDate", SqlDbType.DateTime).Value = txtDate.Text;
            cmd2.Parameters.Add("@ExpHeadName", SqlDbType.VarChar).Value = ddHeadName.SelectedValue;
            cmd2.Parameters.Add("@Description", SqlDbType.VarChar).Value = txtDescription.Text;
            cmd2.Parameters.Add("@Amount", SqlDbType.Decimal).Value = txtAmount.Text;
            cmd2.Parameters.Add("@Entryby", SqlDbType.VarChar).Value = lName.Trim();
            cmd2.Parameters.Add("@ExpenseID", SqlDbType.VarChar).Value = ExpenseNo;
            
            cmd2.Connection.Open();
            int success = cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();

            if (success > 0)
            {
                lblMsg.Text = "The Expense Has Been Successfully Saved";
                lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0a6e01");
                txtAmount.Text = "0";
                GridView1.DataBind();
            }

        }
        else
        {
            lblMsg.Text = "Please Check the Members ID and Amount Properly";
            lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#6e000a");
        }


    }
}
