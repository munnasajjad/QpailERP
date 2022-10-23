using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RunQuery;

public partial class Bank_Ladger : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        if (!IsPostBack)
        {
            ddBank.DataBind();
            txtDateFrom.Text = "01/" + DateTime.Now.ToString("MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            LoadGridData();
        }
    }


    protected void ddBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridData();
        //SecondGrid();
    }
    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        LoadGridData();
        GridView1.PageIndex = e.NewPageIndex;
        //GridView1.DataBind();
    }
    private DataTable LoadGridData()
    {
        string customer = " "; string party = " where ACID ='" + ddBank.SelectedValue + "'";

        if (ddBank.SelectedValue != "---ALL---")
        {
            customer = " AND HeadID ='" + ddBank.SelectedValue + "'";
        }

        decimal opBal = Convert.ToDecimal(SQLQuery.ReturnString("Select OpBalance FROM BankAccounts " + party));

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
        //get opening balance
        string query = " where TrType = 'Bank' " + customer;
        decimal currBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + query + opDate));

        decimal closeBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + query + closeDate));

        //int mDays = Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select MatuirityDays from Party  " + party)));
        //string lastMaturityDate = DateTime.Today.AddDays(mDays * (-1)).ToString("yyyy-MM-dd");

        //if (ddStatus.SelectedValue == "Matured")
        //{
        //    currBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" +
        //                                  ddBank.SelectedValue + "' AND IsActive=1 AND InvDate<'" + lastMaturityDate + "' AND InvDate<'" + dateFrom + "'"));

        //    closeBal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" +
        //                                  ddBank.SelectedValue + "' AND IsActive=1 AND InvDate<'" + lastMaturityDate + "' AND InvDate<='" + dateTo + "'"));

        //}

        //if (ddStatus.SelectedValue == "Immatured")
        //{
        //    currBal = Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" + ddBank.SelectedValue + "'  AND IsActive=1 AND InvDate>'" +
        //                                  lastMaturityDate + "' AND InvDate<'" + dateFrom + "'"));
        //    closeBal = Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" + ddBank.SelectedValue + "'  AND IsActive=1 AND InvDate>'" +
        //                                  lastMaturityDate + "' AND InvDate<='" + dateTo + "'"));
        //}

        dr1 = dt1.NewRow();
        dr1["TrDate"] = Convert.ToDateTime(txtDateFrom.Text).ToShortDateString();
        dr1["Inv"] = "";
        dr1["Link"] = "";
        dr1["Description"] = "Openning Balance";
        dr1["Dr"] = 0;
        dr1["Cr"] = 0;
        dr1["Balance"] = currBal;
        dt1.Rows.Add(dr1);

        query += invDate + closeDate;
        SqlCommand cmd2 = new SqlCommand("SELECT [TrDate], InvNo, TrGroup, TrType, [Description], [Dr], [Cr], [Balance] FROM [Transactions] " + query + "  ORDER BY [TrDate], TrID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmd2.Parameters.Add("@HeadName", SqlDbType.VarChar).Value = ddBank.SelectedValue;
        //cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
        //cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateTo.Text).AddDays(+1).ToString("yyyy-MM-dd");

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


                string link = SQLQuery.ReturnString("Select TOP(1) SerialNo from VoucherTmp where VoucherNo='"+ inv + "' AND (Head5Dr='" + ddBank.SelectedValue + "' OR Head5Cr='" + ddBank.SelectedValue + "') AND (Amount= '" + debt + "' OR Amount= '" + credit + "') ");
                //string compare=SQLQuery.ReturnString("Select AccountsHeadDrName ")


                //if (trGroup == "Sales")
                //{
                //    link = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=" + inv;
                //}

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

        return dt1;
    }

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDateTime(txtDateFrom.Text) <= Convert.ToDateTime(txtDateTo.Text))
            {
                //LoadGridData();
                string bankId = ddBank.SelectedValue;
                string dateFrom = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
                string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

                string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") +
                    "XerpReports/FormBankBook.aspx?bank=" + bankId + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo;
                if1.Attributes.Add("src", url);
            }
            else
            {
                Notify("Invalid Date Range!", "error", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }
    private void SecondGrid()
    {
        SqlCommand cmd = new SqlCommand("SELECT CollectionNo, CollectionDate, PartyName, PaidAmount, ChqDetail, ChqDate FROM Collection WHERE (IsApproved = 'p') and PartyName='" + ddBank.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
            e.Row.Cells[3].Text = string.Format("{0:N2}", total);// Convert.ToString(total.ToString("{0:N2}"));
            e.Row.Cells[4].Text = string.Format("{0:N2}", TotalSales);
            //e.Row.Cells[4].Text = Convert.ToString(balance); //String.Format("{0:c}", TotalSales);
        }
    }
    protected void ddBank_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //ddPO.DataBind();
        //search();
    }

    protected void btnReset_OnClick(object sender, EventArgs e)
    {
        GridView1.AllowPaging = false;
        LoadGridData();
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        GridView1.RenderControl(hw);
        string gridHTML = sw.ToString().Replace("\"", "'").Replace(System.Environment.NewLine, "");
        StringBuilder sb = new StringBuilder();
        sb.Append("<script type = 'text/javascript'>");
        sb.Append("window.onload = new function(){");
        sb.Append("var printWin = window.open('', '', 'left=0");
        sb.Append(",top=0,width=1000,height=600,status=0');");
        sb.Append("printWin.document.write(\"");
        sb.Append(gridHTML);
        sb.Append("\");");
        sb.Append("printWin.document.close();");
        sb.Append("printWin.focus();");
        sb.Append("printWin.print();");
        sb.Append("printWin.close();};");
        sb.Append("</script>");
        ClientScript.RegisterStartupScript(this.GetType(), "GridPrint", sb.ToString());
        GridView1.AllowPaging = true;
        LoadGridData();

    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (ddBank.Items.Count > 0)
        {
            string htmlBody = "<div  style='text-align:center'>" + RptRunQuery.RptSQLQuery.GetHeader("1") + "</div><hr/>";
            htmlBody += "<div  style='text-align:center'><b><u>Bank Book</u></b></div><br/><br/>";
            htmlBody += "<i>Account Name: " + ddBank.SelectedItem.Text + "</i><br/>";
            htmlBody += "<i>Date: from " + txtDateFrom.Text + " to: "+txtDateTo.Text+ "</i><br/><br/>";

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            //Page page = new Page();
            //HtmlForm f = new HtmlForm();
            //page.Controls.Add(f);
            //f.Controls.Add(GridView1);
            //HttpContext.Current.Server.Execute(page, sw, true);

            LoadGridData();
            GridView1.RenderControl(hw);
            string gridHTML = sw.ToString().Replace("\"", "'").Replace(System.Environment.NewLine, "");
            StringBuilder sb = new StringBuilder();
            sb.Append(gridHTML);
            htmlBody += sb.ToString();

            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            var pdfBytes = htmlToPdf.GeneratePdf(htmlBody);
            string savePath = Server.MapPath("./Docs/Print/") + "Bank-Book.pdf";
            File.WriteAllBytes(savePath, pdfBytes);

            string url = "./Docs/Print/Bank-Book.pdf";
            ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
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
