using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class app_Emp_Salary_Process : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            PopulateMonths();
        }
    }

    //Populate Month
    public void PopulateMonths()
    {
        SqlCommand cmd = new SqlCommand("Select * from [Months] where Finalize=0", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();

        SqlDataReader Deptlist = cmd.ExecuteReader();

        ddMonth.DataSource = Deptlist;
        ddMonth.DataValueField = "MonthName";
        ddMonth.DataTextField = "MonthName";
        ddMonth.DataBind();

        cmd.Connection.Close();
        cmd.Connection.Dispose();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DateTime startDt;
        DateTime endDt;
        //get start date and end date of month
        SqlCommand cmd = new SqlCommand("Select DateFrom, DateTo from Months where MonthName='" + ddMonth.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            startDt = Convert.ToDateTime(dr[0].ToString());
            endDt = Convert.ToDateTime(dr[1].ToString());


        }
    }
}
