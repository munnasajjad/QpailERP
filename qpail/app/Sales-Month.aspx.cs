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

public partial class app_Sales_Month : System.Web.UI.Page
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
            txtYear.Text =  dateY;
            //txtDateTo.Text = DateTime.Now.ToShortDateString();

            ddCustomer.DataBind();
            ddGrade.DataBind();
            ddItem.DataBind();
            LoadGridData();
        }
    }

    private DataTable LoadGridData()
    {
       
        string iName = ddItem.SelectedValue;

        string sizeId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) SizeId FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");
        string pId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) ProductID FROM [SaleDetails]  WHERE ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID='" + ddGrade.SelectedValue + "')) AND  ([productName] = '" + iName + "')");
        string brandId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) BrandID FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");

        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataRow dr1 = null;

        dt1.Columns.Add(new DataColumn("RowName", typeof(string)));
        dt1.Columns.Add(new DataColumn("January", typeof(string)));
        dt1.Columns.Add(new DataColumn("February", typeof(string)));
        dt1.Columns.Add(new DataColumn("March", typeof(string)));
        dt1.Columns.Add(new DataColumn("April", typeof(string)));
        dt1.Columns.Add(new DataColumn("May", typeof(string)));
        dt1.Columns.Add(new DataColumn("June", typeof(string)));
        dt1.Columns.Add(new DataColumn("July", typeof(string)));
        dt1.Columns.Add(new DataColumn("August", typeof(string)));
        dt1.Columns.Add(new DataColumn("September", typeof(string)));
        dt1.Columns.Add(new DataColumn("October", typeof(string)));
        dt1.Columns.Add(new DataColumn("November", typeof(string)));
        dt1.Columns.Add(new DataColumn("December", typeof(string)));
        dt1.Columns.Add(new DataColumn("Total", typeof(string)));

        //Add Quantity
        dr1 = dt1.NewRow();
        dr1["RowName"] = "Quantity (Pcs.)";
        dr1["January"] = CalcQty("Quantity", sizeId, pId, brandId, txtYear.Text + "-01-01", txtYear.Text + "-02-01");
        dr1["February"] = CalcQty("Quantity", sizeId, pId, brandId, txtYear.Text + "-02-01", txtYear.Text + "-03-01");
        dr1["March"] = CalcQty("Quantity", sizeId, pId, brandId, txtYear.Text + "-03-01", txtYear.Text + "-04-01");
        dr1["April"] = CalcQty("Quantity", sizeId, pId, brandId, txtYear.Text + "-04-01", txtYear.Text + "-05-01");
        dr1["May"] = CalcQty("Quantity", sizeId, pId, brandId, txtYear.Text + "-05-01", txtYear.Text + "-06-01");
        dr1["June"] = CalcQty("Quantity", sizeId, pId, brandId, txtYear.Text + "-06-01", txtYear.Text + "-07-01");
        dr1["July"] = CalcQty("Quantity", sizeId, pId, brandId, txtYear.Text + "-07-01", txtYear.Text + "-08-01");
        dr1["August"] = CalcQty("Quantity", sizeId, pId, brandId, txtYear.Text + "-08-01", txtYear.Text + "-09-01");
        dr1["September"] = CalcQty("Quantity", sizeId, pId, brandId, txtYear.Text + "-09-01", txtYear.Text + "-10-01");
        dr1["October"] = CalcQty("Quantity", sizeId, pId, brandId, txtYear.Text + "-10-01", txtYear.Text + "-11-01");
        dr1["November"] = CalcQty("Quantity", sizeId, pId, brandId, txtYear.Text + "-11-01", txtYear.Text + "-12-01");

        int nextYear = Convert.ToInt32(txtYear.Text) + 1;
        dr1["December"] = CalcQty("Quantity", sizeId, pId, brandId, txtYear.Text + "-12-01", nextYear + "-01-01");
        dr1["Total"] = CalcQty("Quantity", sizeId, pId, brandId, txtYear.Text + "-01-01", nextYear + "-01-01");

        dt1.Rows.Add(dr1);

        //Add Amount
        dr1 = dt1.NewRow();
        dr1["RowName"] = "Amount (Tk.)";
        dr1["January"] = CalcQty("ItemTotal", sizeId, pId, brandId, txtYear.Text + "-01-01", txtYear.Text + "-02-01");
        dr1["February"] = CalcQty("ItemTotal", sizeId, pId, brandId, txtYear.Text + "-02-01", txtYear.Text + "-03-01");
        dr1["March"] = CalcQty("ItemTotal", sizeId, pId, brandId, txtYear.Text + "-03-01", txtYear.Text + "-04-01");
        dr1["April"] = CalcQty("ItemTotal", sizeId, pId, brandId, txtYear.Text + "-04-01", txtYear.Text + "-05-01");
        dr1["May"] = CalcQty("ItemTotal", sizeId, pId, brandId, txtYear.Text + "-05-01", txtYear.Text + "-06-01");
        dr1["June"] = CalcQty("ItemTotal", sizeId, pId, brandId, txtYear.Text + "-06-01", txtYear.Text + "-07-01");
        dr1["July"] = CalcQty("ItemTotal", sizeId, pId, brandId, txtYear.Text + "-07-01", txtYear.Text + "-08-01");
        dr1["August"] = CalcQty("ItemTotal", sizeId, pId, brandId, txtYear.Text + "-08-01", txtYear.Text + "-09-01");
        dr1["September"] = CalcQty("ItemTotal", sizeId, pId, brandId, txtYear.Text + "-09-01", txtYear.Text + "-10-01");
        dr1["October"] = CalcQty("ItemTotal", sizeId, pId, brandId, txtYear.Text + "-10-01", txtYear.Text + "-11-01");
        dr1["November"] = CalcQty("ItemTotal", sizeId, pId, brandId, txtYear.Text + "-11-01", txtYear.Text + "-12-01");

        dr1["December"] = CalcQty("ItemTotal", sizeId, pId, brandId, txtYear.Text + "-12-01", nextYear + "-01-01");
        dr1["Total"] = CalcQty("ItemTotal", sizeId, pId, brandId, txtYear.Text + "-01-01", nextYear + "-01-01");

        dt1.Rows.Add(dr1);

        //Add VAT
        dr1 = dt1.NewRow();
        dr1["RowName"] = "VAT (Tk.)";
        dr1["January"] = CalcQty("VAT", sizeId, pId, brandId, txtYear.Text + "-01-01", txtYear.Text + "-02-01");
        dr1["February"] = CalcQty("VAT", sizeId, pId, brandId, txtYear.Text + "-02-01", txtYear.Text + "-03-01");
        dr1["March"] = CalcQty("VAT", sizeId, pId, brandId, txtYear.Text + "-03-01", txtYear.Text + "-04-01");
        dr1["April"] = CalcQty("VAT", sizeId, pId, brandId, txtYear.Text + "-04-01", txtYear.Text + "-05-01");
        dr1["May"] = CalcQty("VAT", sizeId, pId, brandId, txtYear.Text + "-05-01", txtYear.Text + "-06-01");
        dr1["June"] = CalcQty("VAT", sizeId, pId, brandId, txtYear.Text + "-06-01", txtYear.Text + "-07-01");
        dr1["July"] = CalcQty("VAT", sizeId, pId, brandId, txtYear.Text + "-07-01", txtYear.Text + "-08-01");
        dr1["August"] = CalcQty("VAT", sizeId, pId, brandId, txtYear.Text + "-08-01", txtYear.Text + "-09-01");
        dr1["September"] = CalcQty("VAT", sizeId, pId, brandId, txtYear.Text + "-09-01", txtYear.Text + "-10-01");
        dr1["October"] = CalcQty("VAT", sizeId, pId, brandId, txtYear.Text + "-10-01", txtYear.Text + "-11-01");
        dr1["November"] = CalcQty("VAT", sizeId, pId, brandId, txtYear.Text + "-11-01", txtYear.Text + "-12-01");

        dr1["December"] = CalcQty("VAT", sizeId, pId, brandId, txtYear.Text + "-12-01", nextYear + "-01-01");
        dr1["Total"] = CalcQty("VAT", sizeId, pId, brandId, txtYear.Text + "-01-01", nextYear + "-01-01");

        dt1.Rows.Add(dr1);

        //Add NetAmount
        dr1 = dt1.NewRow();
        dr1["RowName"] = "Net Amount (Tk.)";
        dr1["January"] = CalcQty("NetAmount", sizeId, pId, brandId, txtYear.Text + "-01-01", txtYear.Text + "-02-01");
        dr1["February"] = CalcQty("NetAmount", sizeId, pId, brandId, txtYear.Text + "-02-01", txtYear.Text + "-03-01");
        dr1["March"] = CalcQty("NetAmount", sizeId, pId, brandId, txtYear.Text + "-03-01", txtYear.Text + "-04-01");
        dr1["April"] = CalcQty("NetAmount", sizeId, pId, brandId, txtYear.Text + "-04-01", txtYear.Text + "-05-01");
        dr1["May"] = CalcQty("NetAmount", sizeId, pId, brandId, txtYear.Text + "-05-01", txtYear.Text + "-06-01");
        dr1["June"] = CalcQty("NetAmount", sizeId, pId, brandId, txtYear.Text + "-06-01", txtYear.Text + "-07-01");
        dr1["July"] = CalcQty("NetAmount", sizeId, pId, brandId, txtYear.Text + "-07-01", txtYear.Text + "-08-01");
        dr1["August"] = CalcQty("NetAmount", sizeId, pId, brandId, txtYear.Text + "-08-01", txtYear.Text + "-09-01");
        dr1["September"] = CalcQty("NetAmount", sizeId, pId, brandId, txtYear.Text + "-09-01", txtYear.Text + "-10-01");
        dr1["October"] = CalcQty("NetAmount", sizeId, pId, brandId, txtYear.Text + "-10-01", txtYear.Text + "-11-01");
        dr1["November"] = CalcQty("NetAmount", sizeId, pId, brandId, txtYear.Text + "-11-01", txtYear.Text + "-12-01");

        dr1["December"] = CalcQty("NetAmount", sizeId, pId, brandId, txtYear.Text + "-12-01", nextYear + "-01-01");
        dr1["Total"] = CalcQty("NetAmount", sizeId, pId, brandId, txtYear.Text + "-01-01", nextYear + "-01-01");

        dt1.Rows.Add(dr1);



        GVrpt.DataSource = dt1;
        GVrpt.DataBind();

        return dt1;
    }

    private int CalcQty(string colName, string sizeId, string pId, string brandId, string dtFrom, string dtTo)
    {
        dtTo = Convert.ToDateTime(dtTo).AddDays(1).ToString("yyyy-MM-dd");
        int qty = 0;//Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(" + colName + "),0)  FROM [SaleDetails] WHERE ([SizeId] = '" + sizeId + "') and ([ProductID] = '" + pId + "') and([BrandID] = '" + brandId + "') and EntryDate >= '" + dtFrom + "' AND EntryDate < '" + dtTo + "' ")));
        if (CheckBox1.Checked)
        {
            qty = Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(" + colName + "),0)  FROM [SaleDetails] WHERE InvNo IN (Select InvNo from sales where ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID='"+ddGrade.SelectedValue+"')) AND CustomerID = '" + ddCustomer.SelectedValue + "' and DeliveryDate >= '" + dtFrom + "' AND DeliveryDate <= '" + dtTo + "') ")));
        }
        else
        {
            qty = Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(" + colName + "),0)  FROM [SaleDetails] WHERE ([SizeId] = '" + sizeId + "') and ([ProductID] = '" + pId + "') and([BrandID] = '" + brandId + "') and EntryDate >= '" + dtFrom + "' AND EntryDate <= '" + dtTo + "' ")));
        }
        return qty;
    }


    private decimal total = (decimal)0.0;
    private decimal qty = (decimal)0.0;
    private decimal TotalSales = (decimal)0.0;
    private decimal TotalWeight = (decimal)0.0;
    protected void GVrpt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    qty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Quantity"));
        //    total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ItemTotal"));
        //    TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
        //    TotalWeight += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalWeight"));
        //}
        //else if (e.Row.RowType == DataControlRowType.Footer)
        //{
        //    e.Row.Cells[1].Text = "Total";
        //    e.Row.Cells[2].Text = Convert.ToString(qty);
        //    e.Row.Cells[4].Text = Convert.ToString(total);
        //    e.Row.Cells[5].Text = Convert.ToString(TotalSales - total);
        //    e.Row.Cells[6].Text = Convert.ToString(TotalSales);  //String.Format("{0:c}", TotalSales);
        //    e.Row.Cells[8].Text = Convert.ToString(TotalWeight);  //String.Format("{0:c}", TotalSales);
        //}
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
                LoadGridData();
           
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
        string url = "";// "./rptLedger.aspx?party=" + "" + "&dt1=" + txtDateFrom.Text + "&dt2=" + txtDateTo.Text;
        ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");
    }

    protected void btnExport_OnClick(object sender, EventArgs e)
    {
        exportExcel(LoadGridData(), "Item-Sales-by-Month-Report");
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

    }
    protected void ddItem_SelectedIndexChanged(object sender, EventArgs e)
    {

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

    protected void GVrpt_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVrpt.PageIndex = e.NewPageIndex;
        LoadGridData();
        GVrpt.PageIndex = e.NewPageIndex;
    }

    protected void ddGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddItem.DataBind();
        LoadGridData();
    }
}
