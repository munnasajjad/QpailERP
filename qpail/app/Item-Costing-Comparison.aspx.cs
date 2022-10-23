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

public partial class app_Item_Costing_Comparison : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddGroup.DataBind();
            GenerateSubGroup();
            GenerateGrade();
            GenerateCategory();
            GetProductList();
            search();
            txtInvDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtPODate.Text = DateTime.Now.ToString("dd/MM/yyyy");

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
            string customer = "  AND a.ItemCode IN (Select ProductID from Products where CategoryID='" + ddCategory.SelectedValue + "')  ";
            if (ddItemName.SelectedValue != "--- all ---" && ddItemName.SelectedValue != "")
            {
                customer = " AND a.ItemCode ='" + ddItemName.SelectedValue + "' ";

                if (ddSubGrp.SelectedValue == "10")
                {
                    if (ddSpec.SelectedValue != "--- all ---" && ddSpec.SelectedValue != "")
                    {
                        customer = customer + " AND a.Spec ='" + ddSpec.SelectedValue + "' ";
                    }
                }
            }

            string invNo = " ";
            string invDate = " ";
            if (txtInvDate.Text != "")
            {
                try
                {
                    invDate = Convert.ToDateTime(txtInvDate.Text).ToString("yyyy-MM-dd");
                    invDate = " AND a.LCNO IN (Select LCNo from LC WHERE  (OpenDate >= '" + invDate + "'))";
                }
                catch (Exception) { }
            }

            string poDate = " ";
            if (txtPODate.Text != "")
            {
                try
                {
                    poDate = Convert.ToDateTime(txtPODate.Text).ToString("yyyy-MM-dd");
                    poDate = " AND a.LCNO IN (Select LCNo from LC WHERE  (OpenDate <= '" + poDate + "'))";
                }
                catch (Exception) { }
            }
            
            string query = customer + invNo + invDate + poDate;
            string url = "LC-Preview.aspx?ref=";
            DataSet ds = SQLQuery.ReturnDataSet(@"SELECT a.EntryID, a.LCNo, '" + url + @"'+a.LCNo as link, 
                            (Select OpenDate from LC WHERE LCNo=a.LCNo) AS OpenDate,
							 (Select Purpose from Purpose where pid=a.Purpose) AS Purpose, 
                         c.ItemName, a.HSCode, a.ItemSizeID, a.pcs, a.NoOfPacks, a.QntyPerPack, 
						 (Select spec from Specifications where id= a.Spec) as spec, a.Thickness, a.Measurement, 
						 c.qty, a.UnitPrice, a.CFRValue, a.ReturnQty, a.Loading, a.Loaded, 
						 a.LandingPercent, a.LandingAmt, a.TotalUSD, a.TotalBDT, a.UnitCostBDT, 						  
                         b.InsuranceAmount, b.PenaltyDesc, b.PenaltyAmt, b.AV_Calculated, b.AV_Actual, b.CustomsDutyRate, b.CustomsDutyAmt, 
                         b.RDRate, b.RDAmt, b.SDRate, b.SDAmt, b.SurChargeRate, b.SurChargeAmt, 
						 b.VATRate, b.VATAmt, b.AITRate, b.AITAmt, b.ATVRate, b.ATVAmt, b.TotalDutyCalculated, 
                         b.TotalDutyActual, c.ItemDescription, c.unit, c.BankBDT, c.CustomDuty, 
						 c.VatAtv, c.VatAtvAit, c.CnfComTax, c.Insurance, c.CnfCharge, 
                         c.LcOpCost, c.BankInterest, c.Others, c.TotalImportCost, c.CostPerUnit, c.CostPerUnitVAA, c.CostPerUnitVA
                            FROM    LcItems AS a   JOIN
                         LC_Items_Duty AS b  on a.ItemCode=b.ItemCode and a.LCNo=b.LCNo JOIN      LC_Items_Costing AS c  
                         on a.itemcode=c.itemcode  and a.LCNo=c.LCNo
						 WHERE a.EntryID<>0 " + query + " ORDER BY a.EntryID desc");
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
        //ddItemName.DataBind();
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
            //qty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalQty"));
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalImportCost"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CostPerUnit"));
            cfrbdt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CostPerUnitVAA"));
            BankBDT += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CostPerUnitVA"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[55].Text = "Total";
            //e.Row.Cells[13].Text = Convert.ToString(qty);
            e.Row.Cells[56].Text = Convert.ToString(total);
            e.Row.Cells[57].Text = Convert.ToString(TotalSales);
            e.Row.Cells[58].Text = Convert.ToString(cfrbdt);
            e.Row.Cells[59].Text = Convert.ToString(BankBDT);
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
        exportExcel(search(), "LC-Items-All-Info");
    }

    private void GenerateSubGroup()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue + "' ORDER BY [CategoryName]";
        SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");
    }
    private void GenerateGrade()
    {
       string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' ORDER BY [GradeName]";
        SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
    }
    private void GenerateCategory()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' ORDER BY [CategoryName]";
        SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
    }

    protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerateSubGroup();
        GenerateGrade();
        GenerateCategory();
        GetProductList();
    }

    protected void ddSubGrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerateGrade();
        GenerateCategory();
        GetProductList();
    }

    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerateCategory();
        GetProductList();
    }

    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetProductList();
    }

    private void GetProductList()
    {
        ddItemName.Items.Clear();
        ListItem lst = new ListItem("--- all ---", "");
        ddItemName.Items.Insert(ddItemName.Items.Count, lst);
        //ddItemName.DataBind();

        string gQuery = "SELECT Distinct [ItemCode], (Select ItemName from Products where ProductID=LC_Items_Costing.ItemCode) As ItemName FROM  LC_Items_Costing " +
                        " WHERE  ItemCode IN (Select ProductID from Products where CategoryID='" + ddCategory.SelectedValue +
                        "')  ORDER BY [ItemName]";
        SQLQuery.PopulateDropDown(gQuery, ddItemName, "ItemCode", "ItemName");

        //ltrUnitType.Text = SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddItemName.SelectedValue + "'");

        if (IsPostBack)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
        }

        LoadItemsPanel();
    }

    private void LoadItemsPanel()
    {
        if (ddSubGrp.SelectedValue == "10")
        {
            ddSpec.Items.Clear();
            ListItem lst = new ListItem("--- all ---", "");
            //ddSpec.Items.Insert(ddSpec.Items.Count, lst);

            ddSpec.Visible = true;
            LoadSpecList("filter");
        }
        else
        {
            ddSpec.Visible = false;
        }
        search();
    }

    private void LoadSpecList(string filterDD)
    {
        string gQuery = "SELECT [id], [spec] FROM [Specifications] ORDER BY [spec]";

        if (filterDD != "")
        {
            gQuery = "SELECT [id], [spec] FROM [Specifications] where  CAST(id AS nvarchar) in (Select distinct spec from LcItems where ItemCode='" + ddItemName.SelectedValue + "') ORDER BY [spec]";
        }

        SQLQuery.PopulateDropDown(gQuery, ddSpec, "id", "spec");
        search();
    }

    protected void ddItemName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadItemsPanel();
    }
}