using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_PO_Date_Range : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        search();
    }

    private void search()
    {
        try
        {
            SQLQuery.ExecNonQry("update  Orders set PoNo=OrderID where PoNo is null");

            string customer = " ";
            if (ddCustomer.SelectedValue != "---ALL---")
            {
                customer = " AND CustomerName='" + ddCustomer.SelectedValue + "'";
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
                    invDate = " AND  (OrderDate >= '" + invDate + "')";
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
                    poDate = " AND  (OrderDate <= '" + poDate + "')";
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
            //string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";
            string url = "PO-Print.aspx?poid=";
            DataSet ds = SQLQuery.ReturnDataSet(@"SELECT OrderSl, '" + url + "'+(CONVERT(varchar,OrderSl)) as link, OrderType, OrderID, OrderDate, DeliveryDate, (SELECT [Company] FROM [Party] WHERE ([PartyID] = Orders.CustomerName)) AS CustomerName, " +
                                                " OrderType+'# '+OrderID as status ,DeliveryAddress, TotalAmount, Discount, Vat, PayableAmount, DeliveryStatus, PoNo FROM [Orders] WHERE OrderSl<>0 " + query + " ORDER BY OrderDate desc");
            //ltrtotal.Text = ds.Tables[0].Rows.Count.ToString();
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
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
        //ltrtotal.Text = "0";

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
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAmount"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PayableAmount"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[4].Text = "Total";
            e.Row.Cells[5].Text = Convert.ToString(total);
            e.Row.Cells[6].Text = Convert.ToString(TotalSales - total) + " Tk.";
            e.Row.Cells[7].Text = Convert.ToString(TotalSales); //String.Format("{0:c}", TotalSales);
        }
    }

}