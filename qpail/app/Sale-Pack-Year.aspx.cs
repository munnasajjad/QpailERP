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

public partial class app_Sale_Pack_Year : System.Web.UI.Page
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
            txtYear.Text = dateY;
            ddGrade.DataBind();
            ddItem.DataBind();
            LoadGridData();
        }
    }

    private DataTable LoadGridData()
    {

        string sizeId = ddItem.SelectedValue;

        //string sizeId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) SizeId FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");
        //string pId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) ProductID FROM [SaleDetails]  WHERE ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID='" + ddGrade.SelectedValue + "')) AND  ([productName] = '" + iName + "')");
        //string brandId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) BrandID FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");

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

        //Customer List 
        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT a.CustomerID, (Select Company from Party where PartyID=a.CustomerID) AS PartyName FROM Sales a ORDER BY [PartyName]");

        foreach (DataRow drx in dtx.Rows)
        {
            string customerID = drx["CustomerID"].ToString();
            string customerName = drx["PartyName"].ToString();

            dr1 = dt1.NewRow();
            dr1["RowName"] = customerName;
            dr1["January"] = CalcQty(customerID, sizeId, txtYear.Text + "-01-01", txtYear.Text + "-02-01");
            dr1["February"] = CalcQty(customerID, sizeId, txtYear.Text + "-02-01", txtYear.Text + "-03-01");
            dr1["March"] = CalcQty(customerID, sizeId, txtYear.Text + "-03-01", txtYear.Text + "-04-01");
            dr1["April"] = CalcQty(customerID, sizeId, txtYear.Text + "-04-01", txtYear.Text + "-05-01");
            dr1["May"] = CalcQty(customerID, sizeId, txtYear.Text + "-05-01", txtYear.Text + "-06-01");
            dr1["June"] = CalcQty(customerID, sizeId, txtYear.Text + "-06-01", txtYear.Text + "-07-01");
            dr1["July"] = CalcQty(customerID, sizeId, txtYear.Text + "-07-01", txtYear.Text + "-08-01");
            dr1["August"] = CalcQty(customerID, sizeId, txtYear.Text + "-08-01", txtYear.Text + "-09-01");
            dr1["September"] = CalcQty(customerID, sizeId, txtYear.Text + "-09-01", txtYear.Text + "-10-01");
            dr1["October"] = CalcQty(customerID, sizeId, txtYear.Text + "-10-01", txtYear.Text + "-11-01");
            dr1["November"] = CalcQty(customerID, sizeId, txtYear.Text + "-11-01", txtYear.Text + "-12-01");

            int nextYear = Convert.ToInt32(txtYear.Text) + 1;
            dr1["December"] = CalcQty(customerID, sizeId, txtYear.Text + "-12-01", nextYear + "-01-01");
            dr1["Total"] = CalcQty(customerID, sizeId, txtYear.Text + "-01-01", nextYear + "-01-01");

            dt1.Rows.Add(dr1);
        }


        GVrpt.DataSource = dt1;
        GVrpt.DataBind();

        return dt1;
    }

    private int CalcQty(string customerId, string sizeId, string dtFrom, string dtTo)
    {
        dtTo = Convert.ToDateTime(dtTo).AddDays(-1).ToString("yyyy-MM-dd");
        string colName = ddType.SelectedValue;
        int qty = 0;//Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(" + colName + "),0)  FROM [SaleDetails] WHERE ([SizeId] = '" + sizeId + "') and ([ProductID] = '" + pId + "') and([BrandID] = '" + brandId + "') and EntryDate >= '" + dtFrom + "' AND EntryDate < '" + dtTo + "' ")));
        if (CheckBox1.Checked)
        {
            qty = Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(" + colName + "),0)  FROM [SaleDetails] WHERE InvNo IN (Select InvNo from sales where ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID='" + ddGrade.SelectedValue + "')) AND CustomerID = '" + customerId + "' and DeliveryDate >= '" + dtFrom + "' AND DeliveryDate <= '" + dtTo + "') ")));
        }
        else
        {
            qty = Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(" + colName + "),0)  FROM [SaleDetails] WHERE  ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID='" + ddGrade.SelectedValue + "')) AND  ([SizeId] = '" + sizeId + "') AND InvNo IN (SELECT InvNo FROM Sales WHERE (CustomerID = '" + customerId + "') AND (DeliveryDate >= '" + dtFrom + "') AND (DeliveryDate <= '" + dtTo + "'))")));
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
        exportExcel(LoadGridData(), "Item Sales by Month");
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
