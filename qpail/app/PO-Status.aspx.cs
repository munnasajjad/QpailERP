using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_PO_Status : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddCustomer.DataBind();
            //ddPO.DataBind();
            search();
        }
    }


    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        search();
    }

    private void search()
    {
        try
        {
            //SQLQuery.ExecNonQry("update  Orders set PoNo=OrderID where PoNo is null");

            string customer = " ";
            if (ddCustomer.SelectedValue != "---ALL---")
            {
                customer = " AND CustomerID ='" + ddCustomer.SelectedValue + "'";
            }

            string invDate = " ";
            if (txtInvDate.Text != "")
            {
                try
                {
                    invDate = Convert.ToDateTime(txtInvDate.Text).ToString("yyyy-MM-dd");
                    invDate = " AND  (EntryDate >= '" + invDate + "')";
                }
                catch (Exception) { }
            }

            string poDate = " ";
            if (txtPODate.Text != "")
            {
                try
                {
                    poDate = Convert.ToDateTime(txtPODate.Text).ToString("yyyy-MM-dd");
                    poDate = " AND  (EntryDate <= '" + poDate + "')";
                }
                catch (Exception) { }
            }

            string mode = " ";
            //if (ddSalesMode.SelectedValue != "---ALL---")
            //{
            //    mode = " AND SalesMode='" + ddSalesMode.SelectedValue + "'";
            //}

            string query = customer + invDate + poDate + mode;
            //string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmPO.aspx?inv=";
            DataSet ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT ProductName, SUM(Quantity) AS Quantity, SUM(DeliveredQty) AS Delivered, (SUM(Quantity)-SUM(DeliveredQty)) AS QtyBalance , SUM(QtyBalance) AS ActBalance FROM OrderDetails WHERE (BrandID IN (SELECT BrandID FROM CustomerBrands WHERE (OrderDetails.QtyBalance > 0)  " + query + ")) GROUP BY ProductName");
            //ltrtotal.Text = ds.Tables[0].Rows.Count.ToString();
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
            //ltrtotal.Text = GridView1.Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
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
    private decimal balance = (decimal)0.0;
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Quantity"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Delivered"));
            balance += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyBalance"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[2].Text = Convert.ToString(total);
            e.Row.Cells[3].Text = Convert.ToString(TotalSales);
            e.Row.Cells[4].Text = Convert.ToString(balance); //String.Format("{0:c}", TotalSales);
        }
    }

    protected void ddCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //ddPO.DataBind();
        search();
    }
}