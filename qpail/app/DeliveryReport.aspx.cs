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

public partial class app_DeliveryReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtInvDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtPODate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            ddCustomer.DataBind();
            //search();
        }
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        //search();
        Button1.Visible = true;

        string dateFrom = Convert.ToDateTime(txtInvDate.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtPODate.Text).ToString("yyyy-MM-dd");

        string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") +
            "XerpReports/DeliveryReport.aspx?cust=" + ddCustomer.SelectedValue + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo;

        if1.Attributes.Add("src", url);
    }

    private DataTable search()
    {
        try
        {
            string customer = " ";
            if (ddCustomer.SelectedValue != "---ALL---")
            {
                customer = " AND CustomerID='" + ddCustomer.SelectedValue + "'";
                SQLQuery.CalculateOverDueDays(ddCustomer.SelectedValue);
            }

            string invNo = " ";
            //if (txtInvNo.Text != "")
            //{
            //    invNo = " AND  (InvNo LIKE '%" + txtInvNo.Text + "%')";
            //}

            string invDate = " ";
            if (txtInvDate.Text != "")
            {
                try
                {
                    invDate = Convert.ToDateTime(txtInvDate.Text).ToString("yyyy-MM-dd");
                    invDate = " AND  (InvDate >= '" + invDate + "')";
                }
                catch (Exception) { }
            }

            string poNo = " ";
            //if (txtPoNo.Text != "")
            //{
            //    poNo = " AND  (PONo = '" + txtPoNo.Text + "')";
            //}

            string poDate = " ";
            if (txtPODate.Text != "")
            {
                try
                {
                    poDate = Convert.ToDateTime(txtPODate.Text).ToString("yyyy-MM-dd");
                    poDate = " AND  (InvDate <= '" + poDate + "')";
                }
                catch (Exception) { }
            }

            string mode = " ";
            //if (ddSalesMode.SelectedValue != "---ALL---")
            //{
            //    mode = " AND SalesMode='" + ddSalesMode.SelectedValue + "'";
            //}

            string godown = " ";
            //if (ddWarehouse.SelectedValue != "---ALL---")
            //{
            //    godown = " AND Warehouse='" + ddWarehouse.SelectedValue + "'";
            //}

            string challanNo = " ";
            //if (txtChallanNo.Text != "")
            //{
            //    challanNo = " AND  (ChallanNo LIKE '%" + txtChallanNo.Text + "%')";
            //}

            string vatChallanNo = " ";
            //if (txtVatChallan.Text != "")
            //{
            //    vatChallanNo += " AND  (VatChalNo LIKE '%" + txtVatChallan.Text + "%')";
            //}

            string query = customer + invNo + invDate + poNo + poDate + mode + godown + challanNo + vatChallanNo;
            //string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=";
            DataSet ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT SaleDetails.ItemChallanNo, Sales.InvDate AS PODate, SUM(SaleDetails.Quantity) AS Quantity, Sales.CustomerID
FROM            Sales INNER JOIN
                         SaleDetails ON Sales.InvNo = SaleDetails.InvNo
WHERE  SaleDetails.Id<>0 " + query + " GROUP BY Sales.InvDate, SaleDetails.ItemChallanNo, Sales.CustomerID  ORDER BY Sales.InvDate");
            ltrtotal.Text = ds.Tables[0].Rows.Count.ToString();
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();

            //ltrQty.Text = SQLQuery.ReturnString("Select SUM(Quantity) from SaleDetails where InvNo IN (Select InvNo  FROM [Sales] WHERE SaleID<>0 " + query + ")");
            //ltrItemLoad.Text = SQLQuery.ReturnString("Select SUM(InvoiceTotal) FROM [Sales] WHERE (SaleID<>0 " + query + ")");
            //ltrTotalVat.Text = SQLQuery.ReturnString("Select SUM(VATAmount) FROM [Sales] WHERE (SaleID<>0 " + query + ")");
            //ltrGTAmt.Text = SQLQuery.ReturnString("Select SUM(PayableAmount) FROM [Sales] WHERE (SaleID<>0 " + query + ")");

            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
            DataTable dt = null;
            return dt;
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
        txtInvDate.Text = "";
        txtPODate.Text = "";

        GridView1.DataSource = null;
        GridView1.DataBind();
        ltrtotal.Text = "0";
        Button1.Visible = false;
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

    private decimal qty = (decimal)0.0;
    private decimal total = (decimal)0.0;
    private decimal TotalSales = (decimal)0.0;
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            qty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "qty"));
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "InvoiceTotal"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PayableAmount"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[5].Text = "Total: ";
            e.Row.Cells[7].Text = Convert.ToString(qty);
            e.Row.Cells[8].Text = Convert.ToString(total);
            e.Row.Cells[9].Text = Convert.ToString(TotalSales - total);
            e.Row.Cells[10].Text = Convert.ToString(TotalSales) + " Tk."; //String.Format("{0:c}", TotalSales);
        }
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

    protected void Button1_OnClick(object sender, EventArgs e)
    {
        exportExcel(search(), "Invoice-Date-Range");
    }
}