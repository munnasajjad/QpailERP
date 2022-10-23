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

public partial class Operator_Sale_Customer_Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtdateFrom.Text = "01" + DateTime.Now.ToString("dd/MM/yyyy").Substring(2);
            txtdateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            search();
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

            string query = dateFrom +  dateTo ;
            //string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";
            string url = "Sale-Details-Report.aspx?f="+Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd") +"&t="+Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd")+"&c=";
            DataSet ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT Company FROM Party WHERE (PartyID = Sales.CustomerID)) AS CustomerID, '" + url + "'+CustomerID as link, COUNT(InvNo) as Qty, " +
                                                /*"(Select SUM(Quantity) from SaleDetails where InvNo in (Select InvNo from Sales where CustomerID=Sales.CustomerID " + query + " ) ) AS iqty, " +
                                                "(Select SUM(TotalWeight) from SaleDetails where InvNo in (Select InvNo from Sales where SaleID<>0 " + query + " ) ) AS weight, " +*/
                                                "SUM(InvoiceTotal) AS InvoiceTotal, SUM(VATAmount) AS VATAmount, SUM(PayableAmount) AS PayableAmount  FROM Sales where SaleID<>0 " + query + " GROUP BY CustomerID");
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
        //ddCustomer.DataBind();
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
        exportExcel(search(), "Customers-Sales-Summary-Report");
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

    private int qty = (int)0.0;
    private decimal total = (decimal)0.0;
    private decimal TotalSales = (decimal)0.0;
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            qty += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "qty"));
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "InvoiceTotal"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PayableAmount"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[2].Text = Convert.ToString(qty);
            e.Row.Cells[3].Text = Convert.ToString(total.ToString("N"));
            e.Row.Cells[4].Text = (TotalSales - total).ToString("N");
            e.Row.Cells[5].Text = Convert.ToString(TotalSales.ToString("N")); //String.Format("{0:c}", TotalSales);
        }
    }

}