using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Application_Reports_CashStatement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            getCompanyInfo();
            string dateM = DateTime.Now.Month.ToString();
            string dateY = DateTime.Now.Year.ToString();

            if (dateM.Length == 1)
            {
                dateM = "0" + dateM;
            }
            txtDateFrom.Text = DateTime.Now.ToShortDateString(); //"01/" + dateM + "/" + dateY;
            txtDateTo.Text = DateTime.Now.ToShortDateString();
            LoadGridData();
        }
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
    protected void ddParties_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridData();
        SecondGrid();
    }

    private void LoadGridData()
    {
        SqlDataAdapter da;
        SqlDataReader dr;
        DataSet ds;
        int recordcount = 0;
        int ic = 0;

        SqlCommand cmd2 = new SqlCommand("SELECT [EntryDate] as TrDate, [VoucherRowDescription] as Description, [VoucherDR] As Dr, [VoucherCR] As Cr, [VoucherNo] As Balance FROM [VoucherDetails] WHERE ([AccountsHeadName] = @HeadName) and ISApproved='A' and  EntryDate >= @DateFrom AND EntryDate <= @DateTo ORDER BY [SerialNo] ASC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = "Cash";
        cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateFrom.Text).ToShortDateString();
        cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateTo.Text).ToShortDateString();

        da = new SqlDataAdapter(cmd2);
        ds = new DataSet("Board");

        cmd2.Connection.Open();
        da.Fill(ds, "Board");

        dr = cmd2.ExecuteReader();
        recordcount = ds.Tables[0].Rows.Count;

        DataTable dt1 = new DataTable();
        DataRow dr1 = null;
        dt1.Columns.Add(new DataColumn("TrDate", typeof(string)));
        dt1.Columns.Add(new DataColumn("Description", typeof(string)));
        dt1.Columns.Add(new DataColumn("Dr", typeof(decimal)));
        dt1.Columns.Add(new DataColumn("Cr", typeof(decimal)));
        dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));
        cmd2.Connection.Close();

        decimal debt = 0; decimal credit = 0; decimal currBal = 0;
        string date; string description;

        //get openning balance        
        SqlCommand cmd2x = new SqlCommand("SELECT isnull(sum(VoucherDR),0) FROM [VoucherDetails] WHERE ([AccountsHeadName] = @HeadName) and   EntryDate <= @DateFrom and ISApproved='A'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2x.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = "Cash";
        cmd2x.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateFrom.Text).ToShortDateString();
        cmd2x.Connection.Open();
        decimal debtx = Convert.ToDecimal(cmd2x.ExecuteScalar());
        cmd2x.Connection.Close();

        SqlCommand cmd2y = new SqlCommand("SELECT isnull(sum(VoucherCR),0) FROM [VoucherDetails] WHERE ([AccountsHeadName] = @HeadName) and   EntryDate <= @DateFrom  and ISApproved='A'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2y.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = "Cash";
        cmd2y.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateFrom.Text).ToShortDateString();
        cmd2y.Connection.Open();
        decimal creditx = Convert.ToDecimal(cmd2y.ExecuteScalar());
        cmd2y.Connection.Close();

        currBal = debtx - creditx;

        dr1 = dt1.NewRow();
        dr1["TrDate"] = Convert.ToDateTime(txtDateFrom.Text).ToShortDateString();
        dr1["Description"] = "Openning Balance";
        dr1["Dr"] = 0;
        dr1["Cr"] = 0;
        dr1["Balance"] = currBal;
        dt1.Rows.Add(dr1);

        if (recordcount > 0)
        {
            do
            {
                date = ds.Tables[0].Rows[ic]["TrDate"].ToString();
                description = ds.Tables[0].Rows[ic]["Description"].ToString();
                debt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Dr"].ToString());
                credit = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Cr"].ToString());
                currBal = debt - credit + currBal;
                //object debit2 = dt1.Compute("Sum(Dr)", "");
                //object credit2 = dt1.Compute("Sum(Cr)", "");
                //currBal = Convert.ToDecimal(debit2) - Convert.ToDecimal(credit2);

                dr1 = dt1.NewRow();
                dr1["TrDate"] = date;
                dr1["Description"] = description;
                dr1["Dr"] = debt;
                dr1["Cr"] = credit;
                dr1["Balance"] = currBal;
                dt1.Rows.Add(dr1);
                ic++;

            } while (ic < recordcount);

        }
        else
        {

            //GridView1.DataSource = null;
        }

        //get closing balance        
        SqlCommand cmd2z = new SqlCommand("SELECT isnull(sum(VoucherDR),0)-isnull(sum(VoucherCR),0) as balance FROM [VoucherDetails] WHERE ([AccountsHeadName] = @HeadName) and EntryDate <= @DateTo and ISApproved='A'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2z.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = "Cash";
        cmd2z.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateTo.Text).AddDays(+1).ToShortDateString();
        cmd2z.Connection.Open();
        currBal = Convert.ToDecimal(cmd2z.ExecuteScalar());
        cmd2z.Connection.Close();

        dr1 = dt1.NewRow();
        dr1["TrDate"] = Convert.ToDateTime(txtDateTo.Text).ToShortDateString();
        dr1["Description"] = "Closing Balance";
        dr1["Dr"] = 0;
        dr1["Cr"] = 0;
        dr1["Balance"] = currBal;
        dt1.Rows.Add(dr1);

        GridView1.DataSource = dt1;
        GridView1.DataBind();
    }
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDateTime(txtDateFrom.Text) <= Convert.ToDateTime(txtDateTo.Text))
            {
                LoadGridData();
                SecondGrid();
            }
            else
            {
                MessageBox("Invalid Date Range!");
            }
        }
        catch (Exception ex)
        {
            MessageBox("Invalid Date!");
        }
    }
    private void SecondGrid()
    {
        /*
        SqlCommand cmd = new SqlCommand("SELECT CollectionNo, CollectionDate, PartyName, PaidAmount, ChqDetail, ChqDate FROM Collection WHERE (IsApproved = 'p') and PartyName='" + ddParties.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet("Board");

        da.Fill(ds, "Board");
        SqlDataReader dr = cmd.ExecuteReader();
        DataTable dt = ds.Tables["Board"];

        cmd.Connection.Close();
        cmd.Connection.Dispose();
        GridView2.DataSource = dt;
        GridView2.DataBind();
        */
    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string url = "./rptLedger.aspx?party='Cash' &dt1=" + txtDateFrom.Text + "&dt2=" + txtDateTo.Text;
        ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");
    }
    public static class ResponseHelper
    {
        public static void Redirect(string url, string target, string windowFeatures)
        {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures))
            {

                context.Response.Redirect(url);
            }
            else
            {
                Page page = (Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException(
                        "Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(Page),
                    "Redirect",
                    script,
                    true);
            }
        }
    }
}