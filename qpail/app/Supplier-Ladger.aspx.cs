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

public partial class app_Supplier_Ladger : System.Web.UI.Page
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
            txtDateFrom.Text = "01/" + dateM + "/" + dateY;
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
        string customer = " "; string party = " ";
        decimal opBal = 0;
        if (ddCustomer.SelectedValue != "---ALL---" && ddCustomer.SelectedValue != "")
        {
            customer = " AND HeadID ='" + ddCustomer.SelectedValue + "'";
            party = " where PartyID ='" + ddCustomer.SelectedValue + "'";
           opBal = Convert.ToDecimal(SQLQuery.ReturnString("Select OpBalance FROM Party " + party));
        }

        string type = " ";
        //if (ddStatus.SelectedValue != "---ALL---")
        //{
        //    type = " AND CustomerID ='" + ddStatus.SelectedValue + "'";
        //}

        string opDate = " "; string invDate = " ";
        if (txtDateFrom.Text != "")
        {
            try
            {
                invDate = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
                opDate = " AND  (TrDate < '" + invDate + "')";
                invDate = " AND  (TrDate >= '" + invDate + "')";
            }
            catch (Exception) { }
        }

        string closeDate = " ";
        if (txtDateTo.Text != "")
        {
            try
            {
                closeDate = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");
                closeDate = " AND  (TrDate <= '" + closeDate + "')";
            }
            catch (Exception) { }
        }

        DataTable dt1 = new DataTable();
        DataRow dr1 = null;
        dt1.Columns.Add(new DataColumn("TrDate", typeof(string)));
        dt1.Columns.Add(new DataColumn("Inv", typeof(string)));
        dt1.Columns.Add(new DataColumn("Link", typeof(string)));
        dt1.Columns.Add(new DataColumn("Description", typeof(string)));
        dt1.Columns.Add(new DataColumn("Dr", typeof(decimal)));
        dt1.Columns.Add(new DataColumn("Cr", typeof(decimal)));
        dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));

        int recordcount = 0;
        int ic = 0;

        decimal debt = 0; decimal credit = 0;
        string date; string description;
        string dateFrom = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

        //get opening balance        
        string query = " where TrType='Supplier' " + customer + opDate;
        decimal currBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + query));

        query = " where TrType='Supplier' " + customer + closeDate;
        decimal closeBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + query));

        //int mDays = Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select MatuirityDays from Party  " + party)));
        //string lastMaturityDate = DateTime.Today.AddDays(mDays * (-1)).ToString("yyyy-MM-dd");

        //if (ddStatus.SelectedValue == "Matured")
        //{
        //    currBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" +
        //                                  ddCustomer.SelectedValue + "' AND IsActive=1 AND InvDate<'" + lastMaturityDate + "' AND InvDate<'" + dateFrom + "'"));

        //    closeBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" +
        //                                  ddCustomer.SelectedValue + "' AND IsActive=1 AND InvDate<'" + lastMaturityDate + "' AND InvDate<='" + dateTo + "'"));
        //}     

        //if (ddStatus.SelectedValue == "Immatured")
        //{
        //    currBal = Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" + ddCustomer.SelectedValue + "'  AND IsActive=1 AND InvDate>'" +
        //                                  lastMaturityDate + "' AND InvDate<'" + dateFrom + "'"));
        //    closeBal = Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" + ddCustomer.SelectedValue + "'  AND IsActive=1 AND InvDate>'" +
        //    lastMaturityDate + "' AND InvDate<='" + dateTo + "'"));
        //}

        dr1 = dt1.NewRow();
        dr1["TrDate"] = Convert.ToDateTime(txtDateFrom.Text).ToShortDateString();
        dr1["Inv"] = "";
        dr1["Link"] = "";
        dr1["Description"] = "Opening Balance";
        dr1["Dr"] = 0;
        dr1["Cr"] = 0;
        dr1["Balance"] = currBal;
        dt1.Rows.Add(dr1);

        query = " where TrType='Supplier' " + customer + invDate + closeDate;
        SqlCommand cmd2 = new SqlCommand("SELECT [TrDate], InvNo, TrGroup, TrType, [Description], [Dr], [Cr], [Balance] FROM [Transactions] " + query + "  ORDER BY [TrID] ASC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = ddCustomer.SelectedValue;
        cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateFrom.Text).ToShortDateString();
        cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateTo.Text).AddDays(+1).ToShortDateString();

        SqlDataAdapter da = new SqlDataAdapter(cmd2);
        DataSet ds = new DataSet("Board");

        cmd2.Connection.Open();
        da.Fill(ds, "Board");

        SqlDataReader dr = cmd2.ExecuteReader();
        recordcount = ds.Tables[0].Rows.Count;
        cmd2.Connection.Close();

        if (recordcount > 0)
        {
            do
            {
                date = Convert.ToDateTime(ds.Tables[0].Rows[ic]["TrDate"].ToString()).ToString("dd/MM/yyyy");
                description = ds.Tables[0].Rows[ic]["Description"].ToString();
                debt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Dr"].ToString());
                credit = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Cr"].ToString());
                currBal = debt - credit + currBal;

                string inv = ds.Tables[0].Rows[ic]["InvNo"].ToString();
                string trGroup = ds.Tables[0].Rows[ic]["TrGroup"].ToString();
                string link = "#";
                if (trGroup == "Sales")
                {
                    link = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=" + inv;
                }
                if (trGroup == "Payment")
                {
                    
                }

                dr1 = dt1.NewRow();
                dr1["TrDate"] = date;
                dr1["Inv"] = inv;
                dr1["Link"] = link;
                dr1["Description"] = description;
                dr1["Dr"] = debt;
                dr1["Cr"] = credit;
                dr1["Balance"] = currBal;

                //if (ddStatus.SelectedValue == "Matured" && Convert.ToDateTime(lastMaturityDate) > Convert.ToDateTime(date))
                //{
                //    dt1.Rows.Add(dr1);
                //}
                //else if (ddStatus.SelectedValue == "Immatured" && Convert.ToDateTime(lastMaturityDate) < Convert.ToDateTime(date))
                //{
                //    dt1.Rows.Add(dr1);
                //}
                //else if (ddStatus.SelectedValue == "---ALL---")
                //{
                    dt1.Rows.Add(dr1);
                //}
                ic++;

            } while (ic < recordcount);

        }
        else { }

        //set closing balance    

        dr1 = dt1.NewRow();
        dr1["TrDate"] = Convert.ToDateTime(txtDateTo.Text).ToShortDateString();
        dr1["Inv"] = "";
        dr1["Link"] = "";
        dr1["Description"] = "Closing Balance";
        dr1["Dr"] = 0;
        dr1["Cr"] = 0;
        dr1["Balance"] = closeBal;
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
                //LoadGridData();
                //SecondGrid();

                string PartyID = ddCustomer.SelectedValue;
                string dateFrom = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
                string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

                string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") +
                    "XerpReports/FormSupplierLedger.aspx?supplierId=" + PartyID + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo;

                if1.Attributes.Add("src", url);
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

        /*try
        {
            if (Convert.ToDateTime(txtDateFrom.Text) <= Convert.ToDateTime(txtDateTo.Text))
            {
                //LoadGridData();
                //SecondGrid();

                string PartyID = ddCustomer.SelectedValue;
                string TypeID = ddStatus.SelectedValue;
                string dateFrom = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
                string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

                string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") +
                    "XerpReports/FormCustomerLadger.aspx?Cust=" + PartyID + "&Types="+TypeID+"&dateFrom=" + dateFrom + "&dateTo=" + dateTo;

                if1.Attributes.Add("src", url);
            }
            else
            {
                MessageBox("Invalid Date Range!");
            }
        }
        catch (Exception ex)
        {
            MessageBox("Invalid Date!");
        }*/
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
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Dr"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Cr"));
            //balance += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyBalance"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "Total";
            e.Row.Cells[3].Text = Convert.ToString(total);
            e.Row.Cells[4].Text = Convert.ToString(TotalSales);
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
