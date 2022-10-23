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

public partial class app_LC_Date_Range : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

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
                customer = " AND Category ='" + ddCustomer.SelectedValue + "' ";
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
                    invDate = " AND  (OpenDate >= '" + invDate + "')";
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
                    poDate = " AND  (OpenDate <= '" + poDate + "')";
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
            string url = "LC-Preview.aspx?ref=";
            DataSet ds = SQLQuery.ReturnDataSet(@"SELECT sl, LCNo, '" + url + @"'+LCNo as link, LCType,LCItem,  OpenDate, Category, LCItem, HSCode, LcRef, LcFor, ExpiryDate, ShipDate, ArrivalDate, DeliveryDate, 
                (Select Company FROM Party where PartyID = LC.SupplierID) as  SupplierID, Origin, (Select Company FROM Party where PartyID =LC.AgentID ) AS  AgentID, 
                       (Select BankName FROM Banks where BankId=LC.InsuranceID) AS InsuranceID, (Select Company FROM Party where PartyID =LC.CnfID) AS  CnfID, 
                        (Select (Select BankName FROM Banks where BankId =BankAccounts.BankID)+' - '+ ACNo +' - '+ACName FROM BankAccounts where ACID = LC.BankId) AS  BankId, 
                        LcMargin, LTR, BankExcRate, CustomExcRate, QtyUnit, TotalQty, Freight, CfrUSD, CfrBDT, BankBDT, Remarks, TransportMode, IsActive, 
                        Status, LCCloseBy, LCClosedate, ProjectID, EntryBy, EntryDate FROM [LC] WHERE sl<>0 " + query + " ORDER BY OpenDate");
            //ltrtotal.Text = ds.Tables[0].Rows.Count.ToString();
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
        //ltrtotal.Text = "0";
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
    private decimal cfrbdt = (decimal)0.0;
    private decimal BankBDT = (decimal)0.0;
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            qty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalQty"));
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Freight"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CfrUSD"));
            cfrbdt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "cfrbdt"));
            BankBDT += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "BankBDT"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[13].Text = "Total";
            e.Row.Cells[18].Text = Convert.ToString(qty);
            e.Row.Cells[19].Text = Convert.ToString(total);
            e.Row.Cells[20].Text = Convert.ToString(TotalSales);
            e.Row.Cells[21].Text = Convert.ToString(cfrbdt);
            e.Row.Cells[22].Text = Convert.ToString(BankBDT);
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