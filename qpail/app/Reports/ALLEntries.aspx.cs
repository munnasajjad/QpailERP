using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Application_Reports_ALLEntries : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDateFrom.Text = DateTime.Today.ToShortDateString();
            txtDateTo.Text = DateTime.Today.ToShortDateString();
            getCompanyInfo();
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {

    }

    private void getCompanyInfo()
    {
        SqlConnection cnn = new SqlConnection();
        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        cmd.CommandText = "Select CompanyName, CompanyAddress, Logo From Company";
        cmd.CommandType = CommandType.Text;

        cnn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            lblName.Text = dr[0].ToString();
            lblArress.Text = dr[1].ToString();
            Image1.ImageUrl = "../../" + dr[2].ToString();
        }
        cnn.Close();
    }
}