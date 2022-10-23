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

public partial class app_Sale_Pack_Company : System.Web.UI.Page
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

            //ddCustomer.DataBind();
             ddPackSize.DataBind();
            //LoadGridData();
        }
    }
    private decimal total2 = (decimal)0.0;
    private decimal qty2 = (decimal)0.0;
    private decimal vat2 = (decimal)0.0;
    private decimal TotalSales2 = (decimal)0.0;
    private decimal TotalWeight2 = (decimal)0.0;

    private DataTable LoadGridData()
    {
        string lName = Page.User.Identity.Name.ToString();

        DateTime dtFrom = Convert.ToDateTime(txtDateFrom.Text);
        DateTime dtTo = Convert.ToDateTime(txtDateTo.Text);

        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataRow dr1 = null;

        dt1.Columns.Add(new DataColumn("Company", typeof(string)));
        dt1.Columns.Add(new DataColumn("qty", typeof(string)));
        dt1.Columns.Add(new DataColumn("itemttl", typeof(string)));
        dt1.Columns.Add(new DataColumn("vat", typeof(string)));
        dt1.Columns.Add(new DataColumn("gttl", typeof(string)));
        dt1.Columns.Add(new DataColumn("weight", typeof(string)));

        DataTable dt = SQLQuery.ReturnDataTable("SELECT PartyID, Company from Party where Type='customer' ORDER BY [Company]");

        foreach (DataRow dr in dt.Rows)
        {
            string query = " id<>0 ";
            string pid = Convert.ToString(dr["PartyID"]);
            dr1 = dt1.NewRow();
            dr1["Company"] = Convert.ToString(dr["Company"]);

            int i = 0;
            foreach (ListItem item in ddPackSize.Items)
            {
                if (item.Selected)
                {
                    if (i == 0)
                    {
                        query += " and  ([SizeId] = '" + item.Value + "' ";
                    }
                    else
                    {
                        query += " OR  [SizeId] = '" + item.Value + "' ";
                    }
                    i++;
                }
            }
            if (i > 0)
            {
                query += ") ";
            }
            if (i==0)
            {
                query = " id=0 ";
            }

            query += " AND InvNo IN (Select InvNo from sales where CustomerID = '" + pid + "' and DeliveryDate >= '" + dtFrom.ToString("yyyy-MM-dd") + "' AND DeliveryDate <= '" + dtTo.ToString("yyyy-MM-dd") + "')  ";
            if (chkAll.Checked)
            {
                query += " AND ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID<>'20')) ";
            }
            else
            {
                 query += " AND ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID='" + ddGrade.SelectedValue + "')) ";
            }

            dr1["qty"] = SQLQuery.ReturnString("Select ISNULL(SUM(Quantity),0) from SaleDetails where " + query);
            dr1["itemttl"] = SQLQuery.ReturnString("Select ISNULL(SUM(ItemTotal),0) from SaleDetails where " + query);
            dr1["gttl"] = SQLQuery.ReturnString("Select ISNULL(SUM(VAT),0) from SaleDetails where " + query);
            dr1["vat"] = Convert.ToDecimal(dr1["gttl"]) - Convert.ToDecimal(dr1["itemttl"]);
            dr1["weight"] = SQLQuery.ReturnString("Select ISNULL(SUM(TotalWeight),0) from SaleDetails where " + query);

            decimal qty0 = Convert.ToDecimal(Convert.ToString(dr1["qty"]));
            if (qty0 > 0)
            {
                dt1.Rows.Add(dr1);
            }

            //if (gradeId != "20") //Exclude Tin Products
            //{
            qty2 += Convert.ToDecimal(Convert.ToString(dr1["qty"]));
            total2 += Convert.ToDecimal(Convert.ToString(dr1["itemttl"]));
            vat2 += Convert.ToDecimal(Convert.ToString(dr1["vat"]));
            TotalSales2 += Convert.ToDecimal(Convert.ToString(dr1["gttl"]));
            TotalWeight2 += Convert.ToDecimal(Convert.ToString(dr1["weight"]));
            //}

        }

        GVrpt.DataSource = dt1;
        GVrpt.DataBind();

        return dt1;
    }

    //private decimal total = (decimal)0.0;
    //private decimal qty = (decimal)0.0;
    //private decimal  vat = (decimal)0.0;
    //private decimal TotalSales = (decimal)0.0;
    //private decimal TotalWeight = (decimal)0.0;

    protected void GVrpt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //qty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "qty"));
            //total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "itemttl"));
            //vat += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "vat"));
            //TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "gttl"));
            //TotalWeight += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "weight"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total : ";
            e.Row.Cells[2].Text = Convert.ToString(qty2);
            e.Row.Cells[3].Text = Convert.ToString(total2);
            e.Row.Cells[4].Text = Convert.ToString(vat2);
            e.Row.Cells[5].Text = Convert.ToString(TotalSales2); //String.Format("{0:c}", TotalSales);
            e.Row.Cells[6].Text = Convert.ToString(TotalWeight2);  //String.Format("{0:c}", TotalSales);

            total2 = (decimal)0.0;
            qty2 = (decimal)0.0;
            vat2 = (decimal)0.0;
            TotalSales2 = (decimal)0.0;
            TotalWeight2 = (decimal)0.0;
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

    protected void btnExport_OnClick(object sender, EventArgs e)
    {
        exportExcel(LoadGridData(), "Pack-Size-Sales-Summary-Report");
    }
    private void exportExcel(DataTable data, string reportName)
    {
        var wb = new XLWorkbook();

        // Add DataTable as Worksheet
        wb.Worksheets.Add(data, reportName);

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

    protected void GVrpt_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVrpt.PageIndex = e.NewPageIndex;
        LoadGridData();
        GVrpt.PageIndex = e.NewPageIndex;
    }
    
    protected void ddPackSize_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridData();
    }

    protected void ddGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridData();
    }
    protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAll.Checked)
        {
            ddGrade.Enabled = false;
        }
        else
        {
            ddGrade.Enabled = true;
        }

        LoadGridData();
    }

}
