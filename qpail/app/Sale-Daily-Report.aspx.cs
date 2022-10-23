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

public partial class Operator_Sale_Daily_Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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

    private void LoadGridData()
    {
        string lName = Page.User.Identity.Name.ToString();
        string prjID = ProjectID(lName);
        string iName = "";

        DateTime dtFrom = Convert.ToDateTime(txtDateFrom.Text);
        DateTime dtTo = Convert.ToDateTime(txtDateTo.Text);

        DataSet ds = new DataSet();
        SqlDataReader dr;
        int recordcount = 0;
        int ic = 0;
        int qty = 0;

        SqlCommand cmd2y = new SqlCommand();

        if (ChkAll.Checked == true)
        {
            cmd2y = new SqlCommand("SELECT name FROM Items  Where ProjectID='" + prjID + "' AND ItemType Not Like 'Raw%' AND  ItemType Not Like 'Semi%' AND IsActive='true' ORDER BY name", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }
        else
        {
            cmd2y = new SqlCommand("SELECT name FROM Items  Where ProjectID='" + prjID + "' AND ItemType Not Like 'Raw%' AND  ItemType Not Like 'Semi%' ORDER BY name", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }

        cmd2y.Connection.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd2y);
        da.Fill(ds, "Products");
        cmd2y.Connection.Close();

        recordcount = ds.Tables[0].Rows.Count;
        DataTable dt = ds.Tables["Products"];

        DataTable dt1 = new DataTable();
        DataRow dr1 = null;
        dt1.Columns.Add(new DataColumn("iName", typeof(string)));
        dt1.Columns.Add(new DataColumn("Six2Nine", typeof(string)));
        dt1.Columns.Add(new DataColumn("Nine2Twelve", typeof(string)));
        dt1.Columns.Add(new DataColumn("Twelve2Fifteen", typeof(string)));
        dt1.Columns.Add(new DataColumn("Fifteen2Eighteen", typeof(string)));

        dt1.Columns.Add(new DataColumn("Eighteen2TwentyOne", typeof(string)));
        dt1.Columns.Add(new DataColumn("TwentyOne2TwentyFour", typeof(string)));
        dt1.Columns.Add(new DataColumn("Total", typeof(string)));
        dt1.Columns.Add(new DataColumn("Amount", typeof(string)));

        int sixQty = 0; int nineQty = 0; int twelveQty = 0; int fifteenQty = 0; int eighteenQty = 0; int TwentyOneQty = 0; int totalQty = 0;

        if (recordcount > 0)
        {
            foreach (DataRow row in dt.Rows) //Each Items Will be looped
            {
                iName = row["name"].ToString();

                //if(dtFrom<dtTo) //Counting Each Days Sale Qty by Time
                //{
                do
                {
                    sixQty = sixQty + Quantity(iName, dtFrom.ToShortDateString() + " 00:00:00", dtFrom.ToShortDateString() + " 08:59:59");
                    nineQty = nineQty + Quantity(iName, dtFrom.ToShortDateString() + " 09:00:00", dtFrom.ToShortDateString() + " 11:59:59");
                    twelveQty = twelveQty + Quantity(iName, dtFrom.ToShortDateString() + " 12:00:00", dtFrom.ToShortDateString() + " 14:59:59");
                    fifteenQty = fifteenQty + Quantity(iName, dtFrom.ToShortDateString() + " 15:00:00", dtFrom.ToShortDateString() + " 17:59:59");
                    eighteenQty = eighteenQty + Quantity(iName, dtFrom.ToShortDateString() + " 18:00:00", dtFrom.ToShortDateString() + " 20:59:59");
                    TwentyOneQty = TwentyOneQty + Quantity(iName, dtFrom.ToShortDateString() + " 21:00:00", dtFrom.ToShortDateString() + " 23:59:59");

                    dtFrom = dtFrom.AddDays(+1);
                } while (dtFrom < dtTo);

                //}
                //else
                //{
                //    sixQty = sixQty + Quantity(iName, dtFrom.ToShortDateString() + " 00:00:00", dtFrom.ToShortDateString() + " 08:59:59");
                //    nineQty = nineQty + Quantity(iName, dtFrom.ToShortDateString() + " 09:00:00", dtFrom.ToShortDateString() + " 11:59:59");
                //    twelveQty = twelveQty + Quantity(iName, dtFrom.ToShortDateString() + " 12:00:00", dtFrom.ToShortDateString() + " 14:59:59");
                //    fifteenQty = fifteenQty + Quantity(iName, dtFrom.ToShortDateString() + " 15:00:00", dtFrom.ToShortDateString() + " 17:59:59");
                //    eighteenQty = eighteenQty + Quantity(iName, dtFrom.ToShortDateString() + " 18:00:00", dtFrom.ToShortDateString() + " 20:59:59");
                //    TwentyOneQty = TwentyOneQty + Quantity(iName, dtFrom.ToShortDateString() + " 21:00:00", dtFrom.ToShortDateString() + " 23:59:59");

                //}
                totalQty = sixQty + nineQty + twelveQty + fifteenQty + eighteenQty + TwentyOneQty;

                dtFrom = Convert.ToDateTime(txtDateFrom.Text);
                SqlCommand cmd2 = new SqlCommand("SELECT ISNULL(SUM(ItemTotal),0) FROM [SaleDetails] WHERE ([productName] = '" + iName + "') and EntryDate >= @DateFrom AND EntryDate <= @DateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = dtFrom;
                cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = dtTo;
                cmd2.Connection.Open();
                decimal amt = Convert.ToDecimal(cmd2.ExecuteScalar());
                cmd2.Connection.Close();

                if (ChkAll.Checked == true)
                {
                    dr1 = dt1.NewRow();
                    dr1["iName"] = iName;
                    dr1["Six2Nine"] = sixQty;
                    dr1["Nine2Twelve"] = nineQty;
                    dr1["Twelve2Fifteen"] = twelveQty;
                    dr1["Fifteen2Eighteen"] = fifteenQty;
                    dr1["Eighteen2TwentyOne"] = eighteenQty;
                    dr1["TwentyOne2TwentyFour"] = TwentyOneQty;
                    dr1["Total"] = totalQty;
                    dr1["Amount"] = amt;
                    dt1.Rows.Add(dr1);
                }
                else
                {
                    if (amt > 0)
                    {
                        dr1 = dt1.NewRow();
                        dr1["iName"] = iName;
                        dr1["Six2Nine"] = sixQty;
                        dr1["Nine2Twelve"] = nineQty;
                        dr1["Twelve2Fifteen"] = twelveQty;
                        dr1["Fifteen2Eighteen"] = fifteenQty;
                        dr1["Eighteen2TwentyOne"] = eighteenQty;
                        dr1["TwentyOne2TwentyFour"] = TwentyOneQty;
                        dr1["Total"] = totalQty;
                        dr1["Amount"] = amt;
                        dt1.Rows.Add(dr1);
                    }
                }

                dtFrom = Convert.ToDateTime(txtDateFrom.Text);
                sixQty = 0; nineQty = 0; twelveQty = 0; fifteenQty = 0; eighteenQty = 0; TwentyOneQty = 0; totalQty = 0;
            }
        }

        GVrpt.DataSource = dt1;
        GVrpt.DataBind();
        upnl.Update();
    }

    int total = 0;
    private decimal TotalSales = (decimal)0.0;
    protected void GVrpt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            total += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Total"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Amount"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[7].Text = "Total";
            e.Row.Cells[8].Text = Convert.ToString(total);
            e.Row.Cells[9].Text = Convert.ToString(TotalSales) + " Tk."; //String.Format("{0:c}", TotalSales);
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
                //SecondGrid();
            }
            else
            {
                MessageBox("Invalid Date Range!");
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.ToString();
            MessageBox("Invalid Data! Check Error Message...");
        }
    }

    private int Quantity(string iName, string dtFrom, string dtTo)
    {
        SqlCommand cmd2 = new SqlCommand("SELECT ISNULL(SUM(Quantity),0) FROM [SaleDetails] WHERE ([productName] = '" + iName + "') and EntryDate >= @DateFrom AND EntryDate <= @DateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = dtFrom;
        cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = dtTo;
        cmd2.Connection.Open();
        int Qty = Convert.ToInt32(cmd2.ExecuteScalar());
        cmd2.Connection.Close();
        return Qty;
    }


    private void SecondGrid()
    {
        SqlCommand cmd = new SqlCommand("SELECT CollectionNo, CollectionDate, PartyName, PaidAmount, ChqDetail, ChqDate FROM Collection WHERE (IsApproved = 'p') and PartyName='" + "" + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet("Board");

        da.Fill(ds, "Board");
        SqlDataReader dr = cmd.ExecuteReader();
        DataTable dt = ds.Tables["Board"];

        cmd.Connection.Close();
        cmd.Connection.Dispose();
        GVrpt.DataSource = dt;
        GVrpt.DataBind();

    }
    private string ProjectID(string lName)
    {
        //SqlCommand cmd3z = new SqlCommand("SELECT ProjectName FROM Projects where VID=(SELECT ProjectID  FROM Logins where LoginUserName='" + lName + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        SqlCommand cmd3z = new SqlCommand("SELECT ProjectID  FROM Logins where LoginUserName='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd3z.Connection.Open();
        string prjName = Convert.ToString(cmd3z.ExecuteScalar());
        cmd3z.Connection.Close();
        cmd3z.Connection.Dispose();
        return prjName;
    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string url = "./rptLedger.aspx?party=" + "" + "&dt1=" + txtDateFrom.Text + "&dt2=" + txtDateTo.Text;
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
