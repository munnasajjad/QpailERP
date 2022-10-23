using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_LedgerSub : System.Web.UI.Page
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
            txtDateFrom.Text = "01/" + dateM + "/" + dateY;
            txtDateTo.Text = DateTime.Now.ToShortDateString();
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
            //lblName.Text = dr[0].ToString();
            //lblArress.Text = dr[1].ToString();
            //Image1.ImageUrl = "../../" + dr[2].ToString();
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

        SqlCommand cmd2 = new SqlCommand("SELECT [EntryDate] as TrDate, [VoucherRowDescription] as Description, [VoucherDR] As Dr, [VoucherCR] As Cr, [VoucherNo] As Balance FROM [VoucherDetails] WHERE ([AccountsHeadID] IN (Select AccountsHeadID from HeadSetup where AccountsID=@AccountsID)) and  EntryDate >= @DateFrom AND EntryDate <= @DateTo  AND ISApproved<>'C' ORDER BY EntryDate, [SerialNo] ASC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Parameters.Add("@AccountsID", SqlDbType.VarChar).Value = ddParties.SelectedValue;
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

        //Check if the head is liability head
        string isLiability = ddParties.SelectedValue.Substring(0, 2);
        if (isLiability == "02")
        {
            decimal opBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(OpBalCr),0) - isnull(sum(OpBalDr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID]  IN (Select AccountsHeadID from HeadSetup where AccountsID= '" + ddParties.SelectedValue + "'))"));
            decimal preBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(VoucherCR),0) - isnull(sum(VoucherDR),0) FROM [VoucherDetails] WHERE ([AccountsHeadID]  IN (Select AccountsHeadID from HeadSetup where AccountsID= '" + ddParties.SelectedValue + "')) and   EntryDate < '" + Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd") + "'  and ISApproved='A'"));
            currBal = opBal + preBal;

            dr1 = dt1.NewRow();
            dr1["TrDate"] = Convert.ToDateTime(txtDateFrom.Text).ToShortDateString();
            dr1["Description"] = "Openning Balance";
            dr1["Dr"] = 0;
            dr1["Cr"] = 0;
            dr1["Balance"] = string.Format("{0:N2}", currBal);
            dt1.Rows.Add(dr1);

            if (recordcount > 0)
            {
                do
                {
                    date = Convert.ToDateTime(ds.Tables[0].Rows[ic]["TrDate"].ToString()).ToShortDateString();
                    description = ds.Tables[0].Rows[ic]["Description"].ToString();
                    debt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Dr"].ToString());
                    credit = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Cr"].ToString());
                    currBal = credit - debt + currBal;
                    dr1 = dt1.NewRow();
                    dr1["TrDate"] = date;
                    dr1["Description"] = description;
                    dr1["Dr"] = debt;
                    dr1["Cr"] = credit;
                    dr1["Balance"] = string.Format("{0:N2}", currBal);
                    dt1.Rows.Add(dr1);
                    ic++;

                } while (ic < recordcount);
            }
            else
            {
                //GridView1.DataSource = null;
            }

            //get closing balance
            currBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(VoucherCR),0) - isnull(sum(VoucherDR),0) FROM [VoucherDetails] WHERE ([AccountsHeadID]  IN (Select AccountsHeadID from HeadSetup where AccountsID= '" + ddParties.SelectedValue + "')) and   EntryDate < '" + Convert.ToDateTime(txtDateTo.Text).AddDays(+1).ToString("yyyy-MM-dd") + "'  and ISApproved='A'"));

            dr1 = dt1.NewRow();
            dr1["TrDate"] = Convert.ToDateTime(txtDateTo.Text).ToShortDateString();
            dr1["Description"] = "Closing Balance";
            dr1["Dr"] = 0;
            dr1["Cr"] = 0;
            dr1["Balance"] = string.Format("{0:N2}", (currBal + opBal));
            dt1.Rows.Add(dr1);
        }
        else
        {
            //get openning balance
            SqlCommand cmd2x = new SqlCommand("SELECT  isnull(sum(OpBalDr),0) - isnull(sum(OpBalCr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID] = @HeadName)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2x.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = ddParties.SelectedValue;
            cmd2x.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateFrom.Text).ToShortDateString();
            cmd2x.Connection.Open();
            decimal opBal = Convert.ToDecimal(cmd2x.ExecuteScalar());
            cmd2x.Connection.Close();

            SqlCommand cmd2y = new SqlCommand("SELECT  isnull(sum(VoucherDR),0) - isnull(sum(VoucherCR),0) FROM [VoucherDetails] WHERE ([AccountsHeadID]  IN (Select AccountsHeadID from HeadSetup where AccountsID= @HeadName)) and   EntryDate < @DateFrom  and ISApproved='A'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2y.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = ddParties.SelectedValue;
            cmd2y.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateFrom.Text).ToShortDateString();
            cmd2y.Connection.Open();
            decimal preBal = Convert.ToDecimal(cmd2y.ExecuteScalar());
            cmd2y.Connection.Close();

            currBal = opBal + preBal;

            dr1 = dt1.NewRow();
            dr1["TrDate"] = Convert.ToDateTime(txtDateFrom.Text).ToShortDateString();
            dr1["Description"] = "Openning Balance";
            dr1["Dr"] = 0;
            dr1["Cr"] = 0;
            dr1["Balance"] = string.Format("{0:N2}", currBal);
            dt1.Rows.Add(dr1);

            if (recordcount > 0)
            {
                do
                {
                    date = Convert.ToDateTime(ds.Tables[0].Rows[ic]["TrDate"].ToString()).ToShortDateString();
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
                    dr1["Balance"] = string.Format("{0:N2}", currBal);
                    dt1.Rows.Add(dr1);
                    ic++;

                } while (ic < recordcount);

            }
            else
            {

                //GridView1.DataSource = null;
            }

            //get closing balance
            SqlCommand cmd2z = new SqlCommand("SELECT isnull(sum(VoucherDR),0)-isnull(sum(VoucherCR),0) as balance FROM [VoucherDetails] WHERE ([AccountsHeadID]  IN (Select AccountsHeadID from HeadSetup where AccountsID= @HeadName)) and EntryDate < @DateTo and ISApproved='A'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2z.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = ddParties.SelectedValue;
            cmd2z.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateTo.Text).AddDays(+1).ToShortDateString();
            cmd2z.Connection.Open();
            currBal = Convert.ToDecimal(cmd2z.ExecuteScalar());
            cmd2z.Connection.Close();

            dr1 = dt1.NewRow();
            dr1["TrDate"] = Convert.ToDateTime(txtDateTo.Text).ToShortDateString();
            dr1["Description"] = "Closing Balance";
            dr1["Dr"] = 0;
            dr1["Cr"] = 0;
            dr1["Balance"] = string.Format("{0:N2}", (currBal + opBal));
            dt1.Rows.Add(dr1);
        }

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
        string url = "./rptLedger.aspx?party=" + ddParties.SelectedValue + "&dt1=" + txtDateFrom.Text + "&dt2=" + txtDateTo.Text;
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

    private decimal dr = (decimal)0.0;
    private decimal cr = (decimal)0.0;
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            dr += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "dr"));
            cr += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "cr"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "Total";
            e.Row.Cells[3].Text = string.Format("{0:N2}", dr);// Convert.ToString(dr);
            e.Row.Cells[4].Text = string.Format("{0:N2}", cr);// Convert.ToString(cr);
        }
    }

    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        //search();
        GridView1.PageIndex = e.NewPageIndex;
        //GridView1.DataBind();
    }
}