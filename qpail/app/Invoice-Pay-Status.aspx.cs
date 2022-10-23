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

public partial class app_Invoice_Pay_Status : System.Web.UI.Page
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
            search();
        }
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        search();
        Button1.Visible = true;
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
            else
            {
                DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'customer')");

                foreach (DataRow drx in dtx.Rows)
                {
                    string pid = drx["PartyID"].ToString();
                    SQLQuery.CalculateOverDueDays(pid);
                }
                  
            }

            string invDate = " ";
            if (txtDateFrom.Text != "")
            {
                try
                {
                    invDate = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
                    invDate = " AND  (InvDate >= '" + invDate + "')";
                }
                catch (Exception) { }
            }

            string poDate = " ";
            if (txtDateTo.Text != "")
            {
                try
                {
                    poDate = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");
                    poDate = " AND  (InvDate <= '" + poDate + "')";
                }
                catch (Exception) { }
            }

            string pay = "";
            int mDays = Convert.ToInt32(Convert.ToDecimal(SQLQuery.ReturnString("Select MatuirityDays from Party where PartyID='" + ddCustomer.SelectedValue + "'")));
                string lastMaturityDate = DateTime.Now.AddDays(mDays * (-1)).ToString("yyyy-MM-dd");
            
            if (ddStatus.SelectedValue == "Matured")
            {
                pay = " AND IsActive=1 AND InvDate<'" + lastMaturityDate + "'";
            }
            if (ddStatus.SelectedValue == "Immatured")
            {
                pay = " AND IsActive=1 AND InvDate>='" + lastMaturityDate + "'";
            }
            if (ddStatus.SelectedValue == "Partial")
            {
                pay = " AND IsActive=1 AND DueAmount>=1 AND CollectedAmount>=1";
            }
            if (ddStatus.SelectedValue == "LLC")
            {
                pay = " AND SalesMode='LC' ";
            }
            if (ddStatus.SelectedValue == "Paid")
            {
                pay = " AND IsActive=0";
            }
            
            string query = customer + invDate + poDate + pay;
            string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=";
            DataSet ds = SQLQuery.ReturnDataSet(@"SELECT SaleID, InvNo, '" + url + "'+InvNo as link, InvDate, SalesMode, CustomerID, CustomerName, PONo, PODate, Period, DeliveryDate, DeliveryTime, DeliveryLocation, TransportDetail, " +
                         " MaturityDays, OverdueDays, Remarks, (Select SUM(Quantity) from SaleDetails where InvNo=Sales.InvNo) as qty, " +
                                                "InvoiceTotal, VATPercent, VATAmount, PayableAmount, CartonQty, ChallanNo, CollectedAmount, DueAmount, ProjectID, " +
                         " EntryBy, EntryDate, IsActive, OverDueDate, InWords, Warehouse, VatChalNo, VatChalDate FROM [Sales] WHERE SaleID<>0 " + query + " ORDER BY InvDate desc");
            ltrtotal.Text = ds.Tables[0].Rows.Count.ToString();
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();

            ltrQty.Text = SQLQuery.ReturnString("Select SUM(Quantity) from SaleDetails where InvNo IN (Select InvNo  FROM [Sales] WHERE SaleID<>0 " + query + ")");
            ltrItemLoad.Text = SQLQuery.ReturnString("Select SUM(InvoiceTotal) FROM [Sales] WHERE (SaleID<>0 " + query + ")");
            ltrTotalVat.Text = SQLQuery.ReturnString("Select SUM(VATAmount) FROM [Sales] WHERE (SaleID<>0 " + query + ")");
            ltrGTAmt.Text = SQLQuery.ReturnString("Select SUM(PayableAmount) FROM [Sales] WHERE (SaleID<>0 " + query + ")");

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
        txtDateFrom.Text = "";
        txtDateTo.Text = "";

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
            e.Row.Cells[5].Text = "Total";
            e.Row.Cells[6].Text = Convert.ToString(qty);
            e.Row.Cells[7].Text = Convert.ToString(total);
            e.Row.Cells[8].Text = Convert.ToString(TotalSales - total);
            e.Row.Cells[9].Text = Convert.ToString(TotalSales) + " Tk."; //String.Format("{0:c}", TotalSales);
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

    protected void ddCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        search();
        Button1.Visible = true;
    }
}