using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Voucher_Edit_History : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtdateFrom.Text = DateTime.Now.ToString("01/MM/yyyy");
            txtdateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            //string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FromVoucherEditHistory.aspx?dateFrom=";
            //if1.Attributes.Add("src", urlx);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DataTable dt = SQLQuery.ReturnDataTable(@" SELECT [VID], [VoucherNo], [VoucherDate], [VoucherDescription], [VoucherEntryBy], [VoucherEntryDate], [VoucherPostby], [VoucherAmount], [VoucherEntryTime]
                        FROM [VoucherMasterEditHistory] WHERE VoucherNo IN (Select DISTINCT VoucherNo from VoucherMasterEditHistory WHERE Voucherpost<>'Save') AND VoucherEntryTime>='" + Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd") + "' AND  VoucherEntryTime<='" + Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd") + "' ORDER BY VoucherNo ");
        if (rbDelete.Checked)
        {
            dt = SQLQuery.ReturnDataTable(@"SELECT [VID], [VoucherNo], [VoucherDate], [VoucherDescription], [VoucherEntryBy], [VoucherEntryDate],  [VoucherPostby], [VoucherAmount], [VoucherEntryTime] FROM [VoucherMaster] WHERE VoucherNo IN (Select DISTINCT VoucherNo from VoucherMaster WHERE Voucherpost='C') AND VoucherEntryTime>='" + Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd") + "' AND  VoucherEntryTime<='" + Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd") + "'  ORDER BY VoucherNo ");
        }
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
}