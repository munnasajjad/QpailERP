using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class Cells_Ledger : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {            
            txtDateFrom.Text = DateTime.Now.ToShortDateString();
            txtDateTo.Text = DateTime.Now.ToShortDateString();
            LoadGridData();
        }
        //Get Branch/Centre Name
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmdxx = new SqlCommand("Select BranchName from Users where Username='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdxx.Connection.Open();
        Label1.Text = Convert.ToString(cmdxx.ExecuteScalar());
        cmdxx.Connection.Close();
        cmdxx.Connection.Dispose();
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        LoadGridData();
        GridView1.DataBind();
    }

    private void LoadGridData()
    {
        SqlCommand cmd2 = new SqlCommand("SELECT [TrDate], [Description], [Dr], [Cr], [Balance] FROM [Transactions] WHERE ([HeadName] = @HeadName) and   TrDate >= @DateFrom AND TrDate <= @DateTo ORDER BY [TrID] DESC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = ddBranches.SelectedValue;
        cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateFrom.Text).ToShortDateString();
        cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateTo.Text).AddDays(+1).ToShortDateString();

        cmd2.Connection.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd2);
        cmd2.Connection.Close();
        DataSet ds = new DataSet();
        //DataTable dt = new DataTable();
        da.Fill(ds);
        GridView1.DataSource = ds;
        GridView1.DataBind();
            
    }

}
