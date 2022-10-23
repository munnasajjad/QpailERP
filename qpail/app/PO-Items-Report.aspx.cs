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

public partial class app_PO_Items_Report : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddCustomer.DataBind();
            ddPo.DataBind();
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
            DataSet ds = SQLQuery.ReturnDataSet(@"SELECT ProductName, UnitCost, Quantity, DeliveredQty, QtyBalance, DeliveryInvoice, UnitType FROM OrderDetails WHERE (OrderID = '"+ddPo.SelectedValue+"') ORDER BY Id desc");
            ltrtotal.Text = ds.Tables[0].Rows.Count.ToString();
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();

            ltrtotal.Text = SQLQuery.ReturnString("Select SUM(Quantity)  FROM OrderDetails WHERE (OrderID = '" + ddPo.SelectedValue + "') ");
            ltrQty.Text = SQLQuery.ReturnString("Select SUM(DeliveredQty)  FROM OrderDetails WHERE (OrderID = '" + ddPo.SelectedValue + "') ");
            ltrItemLoad.Text = SQLQuery.ReturnString("Select SUM(QtyBalance)  FROM OrderDetails WHERE (OrderID = '" + ddPo.SelectedValue + "') ");
            //ltrGTAmt.Text = SQLQuery.ReturnString("Select SUM(Quantity) from SaleDetails FROM OrderDetails WHERE (OrderID = '" + ddPo.SelectedValue + "') ");

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
        string url = "./PO-Print.aspx?poid=" + SQLQuery.ReturnString("Select OrderSl from Orders where PoNo='" + ddPo.SelectedValue + "'");
       RunQuery.ResponseHelper.Redirect(url, "_blank", "menubar=0,width=1200,height=768");
        //Response.Redirect("./PO-Print.aspx?poid=" + SQLQuery.ReturnString("Select OrderSl from Orders where PoNo='" + ddPo.SelectedValue + "'"));
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
            qty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Quantity"));
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DeliveredQty"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyBalance"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "Total";
            e.Row.Cells[3].Text = Convert.ToString(qty);
            e.Row.Cells[4].Text = Convert.ToString(total);
            e.Row.Cells[5].Text = Convert.ToString(TotalSales) ; //String.Format("{0:c}", TotalSales);
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
        ddPo.DataBind();
        search();
        Button1.Visible = true;
    }

    protected void ddStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddPo.DataBind();
        search();
    }

    protected void ddPo_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        search();
    }
}