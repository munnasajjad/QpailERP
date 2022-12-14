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

public partial class app_FIFO_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddCustomer.DataBind();
            string dateM = DateTime.Now.Month.ToString();
            string dateY = DateTime.Now.Year.ToString();

            if (dateM.Length == 1)
            {
                dateM = "0" + dateM;
            }
            txtDateFrom.Text ="01/"+DateTime.Now.AddMonths(-6).ToString("MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToShortDateString();
            //ddSuppCategory.DataBind();
            ddCustomer.DataBind();
            LoadGridData();
        }
    }

    protected void btnReset_OnClick(object sender, EventArgs e)
    {
        ddCustomer.DataBind();
        txtDateFrom.Text = "";
        txtDateTo.Text = "";

        GridView1.DataSource = null;
        GridView1.DataBind();
        ltrtotal.Text = "0";

    }
    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridData();
        SecondGrid();
    }

    private void LoadGridData()
    {
        try
        {
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("RDate", typeof(string)));
            dt1.Columns.Add(new DataColumn("Product", typeof(string)));
            dt1.Columns.Add(new DataColumn("Description", typeof(string)));
            dt1.Columns.Add(new DataColumn("Qty", typeof(string)));
            dt1.Columns.Add(new DataColumn("Rate", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Amount", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Used", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));

            int recordcount = 0;
            int ic = 0;

            decimal debt = 0;
            decimal credit = 0;
            string date;
            string description;
            string dateFrom = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
            string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

            SqlCommand cmd2 =
                new SqlCommand(
                    @"SELECT EntryID, LCNo, LCType, ReceiveDate, Purpose, GradeId, CategoryId, ItemCode, HSCode, ItemSizeID, CountryOfOrigin, Manufacturer, pcs, NoOfPacks, 
                         QntyPerPack, Spec, Thickness, Measurement, FullDescription, qty, UnitPrice, CFRValue, ReturnQty, Loading, Loaded, LandingPercent, LandingAmt, TotalUSD, 
                         TotalBDT, UnitCostBDT, EntryBy, EntryDate, UsedQty
                        FROM [LcItems] where  ItemCode='" + ddCustomer.SelectedValue + "' AND ReceiveDate>='" + dateFrom +
                    "' AND ReceiveDate<='" + dateTo + "'  ORDER BY [ReceiveDate],EntryID",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataSet ds = new DataSet("Board");
            cmd2.Connection.Open();
            da.Fill(ds, "Board");
            recordcount = ds.Tables[0].Rows.Count;
            cmd2.Connection.Close();

            if (recordcount > 0)
            {
                do
                {
                    dr1 = dt1.NewRow();
                    dr1["RDate"] = Convert.ToDateTime(ds.Tables[0].Rows[ic]["ReceiveDate"].ToString()).ToString("dd/MM/yyyy");
                    dr1["Product"] = ddCustomer.SelectedItem.Text;
                    dr1["Description"] = ds.Tables[0].Rows[ic]["FullDescription"].ToString();
                    dr1["Qty"] = ds.Tables[0].Rows[ic]["qty"].ToString();
                    dr1["Rate"] = ds.Tables[0].Rows[ic]["UnitCostBDT"].ToString();
                    dr1["Amount"] = Convert.ToDecimal(ds.Tables[0].Rows[ic]["qty"].ToString()) * Convert.ToDecimal(ds.Tables[0].Rows[ic]["UnitCostBDT"].ToString());
                    dr1["Used"] = ds.Tables[0].Rows[ic]["UsedQty"].ToString();
                    dr1["Balance"] = Convert.ToDecimal(ds.Tables[0].Rows[ic]["qty"].ToString()) - Convert.ToDecimal(ds.Tables[0].Rows[ic]["UsedQty"].ToString());

                    dt1.Rows.Add(dr1);
                    ic++;

                } while (ic < recordcount);

            }

            GridView1.DataSource = dt1;
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.ToString();
        }
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
        SqlCommand cmd = new SqlCommand("SELECT CollectionNo, CollectionDate, PartyName, PaidAmount, ChqDetail, ChqDate FROM Collection WHERE (IsApproved = 'p') and PartyName='" + ddCustomer.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet("Board");

        da.Fill(ds, "Board");
        SqlDataReader dr = cmd.ExecuteReader();
        DataTable dt = ds.Tables["Board"];

        cmd.Connection.Close();
        cmd.Connection.Dispose();
        //GridView2.DataSource = dt;
        //GridView2.DataBind();

    }

    private decimal total = (decimal)0.0;
    private decimal TotalSales = (decimal)0.0;
    //private decimal balance = (decimal)0.0;
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Dr"));
            //TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Cr"));
            //balance += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyBalance"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            //e.Row.Cells[2].Text = "Total";
            //e.Row.Cells[3].Text = Convert.ToString(total);
            //e.Row.Cells[4].Text = Convert.ToString(TotalSales);
            //e.Row.Cells[4].Text = Convert.ToString(balance); //String.Format("{0:c}", TotalSales);
        }
    }
    protected void ddCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridData();
    }

    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        //search();
        GridView1.PageIndex = e.NewPageIndex;
        //GridView1.DataBind();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string url = "./rptLedger.aspx?party=" + ddCustomer.SelectedValue + "&dt1=" + txtDateFrom.Text + "&dt2=" + txtDateTo.Text;
        ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");
    }
    protected void ddSuppCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddCustomer.DataBind();
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
