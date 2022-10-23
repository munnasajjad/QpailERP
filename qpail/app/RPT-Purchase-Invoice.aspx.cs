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

public partial class app_RPT_Purchase_Invoice : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

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
                customer = " AND SupplierID='" + ddCustomer.SelectedValue + "'";
                //SQLQuery.CalculateOverDueDays(ddCustomer.SelectedValue);
            }

            string invDate = " ";
            if (txtInvDate.Text != "")
            {
                try
                {
                    invDate = Convert.ToDateTime(txtInvDate.Text).ToString("yyyy-MM-dd");
                    invDate = " AND  (OrderDate >= '" + invDate + "')";
                }
                catch (Exception) { }
            }

            string poDate = " ";
            if (txtPODate.Text != "")
            {
                try
                {
                    poDate = Convert.ToDateTime(txtPODate.Text).ToString("yyyy-MM-dd");
                    poDate = " AND  (OrderDate <= '" + poDate + "')";
                }
                catch (Exception) { }
            }

            string query = customer + invDate +  poDate;
            string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=";
            DataSet ds = SQLQuery.ReturnDataSet(@"SELECT PID, InvNo, PurchaseFor, OrderDate, SupplySource, BillNo, BillDate, SupplierID, SupplierName, ChallanNo, ChallanDate, ItemTotal, PurchaseDiscount, 
                         VatService, OtherExp, PurchaseTotal, TransportType, WarehouseID, Remarks
                            FROM [Purchase] WHERE PID<>0 " + query + " ORDER BY OrderDate desc");
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
        txtInvDate.Text = "";
        txtPODate.Text = "";

        GridView1.DataSource = null;
        GridView1.DataBind();
        ltrtotal.Text = "0";

    }

    protected void btnExport_OnClick(object sender, EventArgs e)
    {
        exportExcel(search(), "Purchases-by-Invoice-Report");
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
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ItemTotal"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PurchaseTotal"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[8].Text = "Total";
            e.Row.Cells[9].Text = Convert.ToString(total);
            //e.Row.Cells[7].Text = Convert.ToString(TotalSales - total);
            e.Row.Cells[13].Text = Convert.ToString(TotalSales) + " Tk."; //String.Format("{0:c}", TotalSales);
        }
    }

}