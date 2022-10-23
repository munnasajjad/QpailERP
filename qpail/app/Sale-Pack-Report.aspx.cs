using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using RunQuery;

public partial class app_Sale_Pack_Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
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

            ddCustomer.DataBind();
            ddItem.DataBind();
            LoadGridData();
        }
    }

    private DataTable LoadGridData()
    {
        decimal total2 = (decimal)0.0;
        decimal qty2 = (decimal)0.0;
        decimal vat2 = (decimal)0.0;
        decimal TotalSales2 = (decimal)0.0;
        decimal TotalWeight2 = (decimal)0.0;

        string lName = Page.User.Identity.Name.ToString();
        string prjID = ProjectID(lName);
        string iName = ddItem.SelectedValue;

        //string sizeId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) SizeId FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");
        //string pId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) ProductID FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");
        //string brandId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) BrandID FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");

        DateTime dtFrom = Convert.ToDateTime(txtDateFrom.Text);
        DateTime dtTo = Convert.ToDateTime(txtDateTo.Text);

        DataSet ds = new DataSet();

        DataTable dt1 = new DataTable();
        DataRow dr1 = null;

        dt1.Columns.Add(new DataColumn("InvNo", typeof(string)));
        dt1.Columns.Add(new DataColumn("LINK", typeof(string)));
        dt1.Columns.Add(new DataColumn("InvDate", typeof(string)));
        dt1.Columns.Add(new DataColumn("ProductName", typeof(string)));
        dt1.Columns.Add(new DataColumn("Quantity", typeof(string)));
        dt1.Columns.Add(new DataColumn("UnitCost", typeof(string)));
        dt1.Columns.Add(new DataColumn("UnitWeight", typeof(string)));
        dt1.Columns.Add(new DataColumn("ItemTotal", typeof(string)));
        dt1.Columns.Add(new DataColumn("TotalWeight", typeof(string)));
        dt1.Columns.Add(new DataColumn("VAT", typeof(string)));
        dt1.Columns.Add(new DataColumn("TotalAmt", typeof(string)));

        string query = " AND  ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID='" + ddGrade.SelectedValue + "'))  ";
        if (!CheckBox1.Checked) // Not All Size
        {
            query += " and  ([SizeId] = '" + iName + "') ";
        }
        if (!CheckBox2.Checked) // for single Customers
        {
            query += " AND InvNo IN (Select InvNo from sales where CustomerID = '" + ddCustomer.SelectedValue + "') ";
        }
        query+= "and InvNo in (Select InvNo FROM Sales WHERE InvDate >= '" + dtFrom.ToString("yyyy-MM-dd") + "' AND InvDate <= '" + dtTo.ToString("yyyy-MM-dd") + "')  ";
        DataTable dt = RunQuery.SQLQuery.ReturnDataTable("SELECT [InvNo], [ProductName], [Quantity], [UnitType], [UnitCost], [ItemTotal], [UnitWeight], [TotalWeight], [VAT], NetAmount, (Select InvDate FROM Sales WHERE InvNo=SaleDetails.InvNo) AS [InvDate], [ReturnQty] FROM [SaleDetails] WHERE ID<>0 " + query + " ORDER BY Id DESC ");


        foreach (DataRow dr in dt.Rows)
        {
            dr1 = dt1.NewRow();
            dr1["InvNo"] = Convert.ToString(dr["InvNo"]);
            string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=";
            dr1["Link"] = url + Convert.ToString(dr["InvNo"]);
            dr1["InvDate"] = Convert.ToDateTime(dr["InvDate"]).ToString("dd/MM/yyyy");
            dr1["ProductName"] = Convert.ToString(dr["ProductName"]);
            dr1["Quantity"] = Convert.ToString(dr["Quantity"]);// + " " + Convert.ToString(dr["UnitType"]);
            dr1["UnitCost"] = Convert.ToString(dr["UnitCost"]);
            dr1["UnitWeight"] = Convert.ToString(dr["UnitWeight"]);
            dr1["ItemTotal"] = Convert.ToString(dr["ItemTotal"]);
            dr1["TotalWeight"] = Convert.ToString(dr["TotalWeight"]);

            dr1["VAT"] = Convert.ToDecimal(Convert.ToString(dr["VAT"])) - Convert.ToDecimal(Convert.ToString(dr["ItemTotal"]));
            dr1["TotalAmt"] = Convert.ToString(dr["VAT"]);
            dt1.Rows.Add(dr1);

            total2 += Convert.ToDecimal(Convert.ToString(dr["ItemTotal"]));
            vat2 += Convert.ToDecimal(Convert.ToString(dr["VAT"])) - Convert.ToDecimal(Convert.ToString(dr["ItemTotal"]));
            qty2 += Convert.ToDecimal(Convert.ToString(dr["Quantity"]));
            TotalSales2 += Convert.ToDecimal(Convert.ToString(dr["VAT"]));
            TotalWeight2 += Convert.ToDecimal(Convert.ToString(dr["TotalWeight"]));

        }

        GridView1.DataSource = dt1;
        GridView1.DataBind();

        ltrQty.Text = qty2.ToString();
        ltrItemLoad.Text = total2.ToString();
        ltrTotalVat.Text = vat2.ToString();
        ltrGTAmt.Text = TotalSales2.ToString();
        ltrTotalWeight.Text = TotalWeight2.ToString();
        return dt;
    }

    private decimal total = (decimal)0.0;
    private decimal qty = (decimal)0.0;
    private decimal TotalSales = (decimal)0.0;
    private decimal TotalWeight = (decimal)0.0;
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            qty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Quantity"));
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ItemTotal"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
            TotalWeight += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalWeight"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[2].Text = Convert.ToString(qty);
            e.Row.Cells[4].Text = Convert.ToString(total);
            e.Row.Cells[5].Text = Convert.ToString(TotalSales - total);
            e.Row.Cells[6].Text = Convert.ToString(TotalSales); //String.Format("{0:c}", TotalSales);
            e.Row.Cells[8].Text = Convert.ToString(TotalWeight);  //String.Format("{0:c}", TotalSales);
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

    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridData();
    }
    protected void ddItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridData();
    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBox1.Checked)
        {
            ddItem.Enabled = false;
        }
        else
        {
            ddItem.Enabled = true;
        }

        LoadGridData();
    }

    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        LoadGridData();
        GridView1.PageIndex = e.NewPageIndex;
    }
    protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBox2.Checked)
        {
            ddCustomer.Enabled = false;
        }
        else
        {
            ddCustomer.Enabled = true;
        }

        LoadGridData();
    }

    protected void ddGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridData();
    }

    protected void btnPrint_OnClick(object sender, EventArgs e)
    {
        try
        {
            GridView1.AllowPaging = false;
            //GridView1.DataBind();
            LoadGridData();

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);
            string gridHTML = sw.ToString().Replace("\"", "'").Replace(System.Environment.NewLine, "");
            gridHTML += "Grand Total Quantity (Pcs.): " + ltrQty.Text + "<br>";
            gridHTML += "Grand Item Total (Tk.): " + ltrItemLoad.Text + "<br>";
            gridHTML += "Grand Total VAT (Tk.): " + ltrTotalVat.Text + "<br>";
            gridHTML += "Grand Total Amount (Tk.): " + ltrGTAmt.Text + "<br>";
            gridHTML += "Grand Total Weight (kg.): " + ltrTotalWeight.Text + "<br>";

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
        catch (Exception ex)
        {
            Response.Write("ERROR: " + ex.ToString());
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }
    protected void btnExport_OnClick(object sender, EventArgs e)
    {
        exportExcel(LoadGridData(), "Delivery-Items-Report");
    }
    private void exportExcel(DataTable data, string reportName)
    {
        var wb = new XLWorkbook();

        // Add DataTable as Worksheet
        wb.Worksheets.Add(data);

        // Create Response
        HttpResponse response = Response;

        //Prepare the response
        response.Clear();
        response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        response.AddHeader("content-disposition", "attachment;filename=" + reportName + ".xlsx");

        //Flush the workbook to the Response.OutputStream
        using (MemoryStream MyMemoryStream = new MemoryStream())
        {
            wb.SaveAs(MyMemoryStream);
            MyMemoryStream.WriteTo(response.OutputStream);
            MyMemoryStream.Close();
        }

        response.End();
    }

}