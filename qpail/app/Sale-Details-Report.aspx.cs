using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using RunQuery;

public partial class app_Sale_Details_Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                txtdateFrom.Text = "01" + DateTime.Now.ToString("dd/MM/yyyy").Substring(2);
            txtdateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            string c = Request.QueryString["c"];
            string f = Request.QueryString["f"];
            string t = Request.QueryString["t"];
            if (c != null)
            {
                ddCustomer.SelectedValue = c;
            }
            if (f != null)
            {
                txtdateFrom.Text = Convert.ToDateTime(f).ToString("dd/MM/yyyy");
            }
            if (t != null)
            {
                txtdateTo.Text = Convert.ToDateTime(t).ToString("dd/MM/yyyy");
            }

            search();
            }
            
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message.ToString();
        }
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        search();
    }

    private DataTable search()
    {
        try
        {
           string customer = " ";
            if (ddCustomer.SelectedValue != "---ALL---")
            {
                customer = " AND CustomerID ='" + ddCustomer.SelectedValue + "'";
            }

            string dateFrom = " ";
            if (txtdateFrom.Text != "")
            {
                try
                {
                    dateFrom = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
                    dateFrom = " AND  (InvDate >= '" + dateFrom + "')";
                }
                catch (Exception) { }
            }

            string dateTo = " ";
            if (txtdateTo.Text != "")
            {
                try
                {
                    dateTo = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");
                    dateTo = " AND  (InvDate <= '" + dateTo + "')";
                }
                catch (Exception) { }
            }

            string query = customer + dateFrom + dateTo;
            DataSet ds = SQLQuery.ReturnDataSet(
                @"SELECT DISTINCT 
                         ProductName, AVG(UnitCost) AS UnitCost, SUM(Quantity) AS Quantity, SUM(ItemTotal) AS ItemTotal, AVG(VatPercent) AS VatPercent, SUM(VAT) AS NetAmount, SUM(VAT)-SUM(ItemTotal) AS VAT,
                          AVG(UnitWeight) AS UnitWeight, SUM(TotalWeight) AS TotalWeight FROM SaleDetails WHERE (InvNo IN (Select InvNo from Sales where SaleID<>0  " + query + " ))  GROUP BY ProductName");//AND  (BrandID IN (SELECT BrandID FROM CustomerBrands WHERE  (CustomerID = '" + ddCustomer.SelectedValue + "'))) GROUP BY ProductName");
            ltrtotal.Text = ds.Tables[0].Rows.Count.ToString();
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
            return null;
        }
        
    }
    protected void btnReset_OnClick(object sender, EventArgs e)
    {
        ddCustomer.DataBind();
        //ddSalesMode.DataBind();
        //ddWarehouse.DataBind();
        //txtPoNo.Text = "";
        //txtVatChallan.Text = "";
        //txtInvNo.Text = "";
        txtdateFrom.Text = "";
        txtdateTo.Text = "";

        GridView1.DataSource = null;
        GridView1.DataBind();
        ltrtotal.Text = "0";

    }
    protected void btnExport_OnClick(object sender, EventArgs e)
    {
        exportExcel(search(), "Customers-Sale-Details-Report");
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

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label CrID = GridView1.Rows[index].FindControl("Label1") as Label;

            string isPending = SQLQuery.ReturnString("Select Status from Tasks  where tid= " + CrID.Text);

            if (isPending == "Done")
            {
                SQLQuery.ReturnString("Update Tasks set Status='Pending' where tid= " + CrID.Text);
            }
            else
            {
                SQLQuery.ReturnString("Update Tasks set Status='Done' where tid= " + CrID.Text);
            }

            GridView1.DataBind();
        }
        catch (Exception ex)
        {

        }
    }

    protected void GridView1_OnSorting(object sender, GridViewSortEventArgs e)
    {

    }

    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        search();
        GridView1.PageIndex = e.NewPageIndex;
        //GridView1.DataBind();
    }

    private decimal total = (decimal)0.0;
    private decimal TotalSales = (decimal)0.0;
    private decimal total2 = (decimal)0.0;
    private decimal TotalSales2 = (decimal)0.0;
    private decimal TotalWeight = (decimal)0.0;
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Quantity"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ItemTotal"));
            total2 += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "VAT"));
            TotalSales2 += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NetAmount"));
            TotalWeight += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalWeight"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "Total";
            e.Row.Cells[3].Text = Convert.ToString(total);
            e.Row.Cells[4].Text = Convert.ToString(TotalSales);
            e.Row.Cells[6].Text = Convert.ToString(total2);
            e.Row.Cells[7].Text = Convert.ToString(TotalSales2);
            e.Row.Cells[9].Text = Convert.ToString(TotalWeight); 
        }
    }

}