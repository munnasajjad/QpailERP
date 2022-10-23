using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using RunQuery;

public partial class app_InvoiceWiseProfitCalculation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSearch.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSearch, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDate.Text = "01" + DateTime.Now.ToString("dd/MM/yyyy").Substring(2);
            //txtDate.Text = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();
            ddCustomer.DataBind();
        }
        //txtInv.Text = InvIDNo();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //BindItemGrid();
        string dateFrom = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

        string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") +
            "XerpReports/FormInvoiceWiseProfitCalculation.aspx?customer=" + ddCustomer.SelectedValue + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo;

        if1.Attributes.Add("src", url);
    }
}