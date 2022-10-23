using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using RunQuery;

public partial class app_Search_Invoice : System.Web.UI.Page
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
            string customer = " ";
            if (ddCustomer.SelectedValue != "---ALL---")
            {
                customer = " AND CustomerID='" + ddCustomer.SelectedValue + "'";
            }

            string invNo = " ";
            if (txtInvNo.Text != "")
            {
                invNo = " AND  (InvNo LIKE '%" + txtInvNo.Text + "%')";
            }

            string invDate = " ";
            if (txtInvDate.Text != "")
            {
                try
                {
                    invDate = Convert.ToDateTime(txtInvDate.Text).ToString("yyyy-MM-dd");
                    invDate = " AND  (InvDate = '" + invDate + "')";
                }
                catch (Exception) { }
            }

            string poNo = " ";
            if (txtPoNo.Text != "")
            {
                poNo = " AND  (PONo  LIKE '%" + txtPoNo.Text + "%')";
            }

            string poDate = " ";
            if (txtPODate.Text != "")
            {
                try
                {
                    poDate = Convert.ToDateTime(txtPODate.Text).ToString("yyyy-MM-dd");
                    poDate = " AND  (PODate = '" + poDate + "')";
                }
                catch (Exception) { }
            }

            string mode = " ";
            if (ddSalesMode.SelectedValue != "---ALL---")
            {
                mode = " AND SalesMode='" + ddSalesMode.SelectedValue + "'";
            }

            string godown = " ";
            if (ddWarehouse.SelectedValue != "---ALL---")
            {
                godown = " AND Warehouse='" + ddWarehouse.SelectedValue + "'";
            }

            string challanNo = " ";
            if (txtChallanNo.Text != "")
            {
                challanNo = " AND  (ChallanNo LIKE '%" + txtChallanNo.Text + "%')";
            }

            string vatChallanNo = " ";
            if (txtVatChallan.Text != "")
            {
                vatChallanNo += " AND  (VatChalNo LIKE '%" + txtVatChallan.Text + "%')";
            }

            string query = customer + invNo + invDate + poNo + poDate + mode + godown + challanNo + vatChallanNo;
            string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=";
            DataSet ds = SQLQuery.ReturnDataSet(@"SELECT SaleID, InvNo, '"+url+"'+InvNo as link, InvDate, SalesMode, CustomerID, CustomerName, PONo, PODate, Period, DeliveryDate, DeliveryTime, DeliveryLocation, TransportDetail, "+
                         " MaturityDays, OverdueDays, Remarks, InvoiceTotal, VATPercent, VATAmount, PayableAmount, CartonQty, ChallanNo, CollectedAmount, DueAmount, ProjectID, "+
                         " EntryBy, EntryDate, IsActive, OverDueDate, InWords, Warehouse, VatChalNo, VatChalDate FROM [Sales] WHERE SaleID<>0 " + query + " ORDER BY InvDate desc");
            ltrtotal.Text = ds.Tables[0].Rows.Count.ToString();
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
        ddSalesMode.DataBind();
        ddWarehouse.DataBind();
        txtPoNo.Text = "";
        txtVatChallan.Text = "";
        txtInvNo.Text = "";
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
}