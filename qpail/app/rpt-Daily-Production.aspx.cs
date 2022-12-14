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

public partial class app_rpt_Daily_Production : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSearch.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSearch, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            GetGrade();
            GetCategory();
            GetProductList();
            GetpackSize();

            //ddGodown.DataBind();
            //QtyinStock();
            QtyinStock();
            BindItemGrid();

        }
        //txtInv.Text = InvIDNo();
    }

    private void GetGrade()
    {
        //string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        //SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
    }
    private void GetCategory()
    {
        //string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
    }

    private void GetpackSize()
    {
        //string gQuery = "SELECT BrandID, BrandName FROM [Brands] where BrandID='" + ddSize.SelectedValue + "' AND ProjectID='" + lblProject.Text + "'ORDER BY [BrandName]";
        //SQLQuery.PopulateDropDown(gQuery,ddSize, "BrandID", "BrandName");
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        BindItemGrid();
        string dateFrom = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

        string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") +
            "XerpReports/FormDailyPrdn.aspx?dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&type=detail";

        if1.Attributes.Add("src", url);
    }

    private DataTable BindItemGrid()
    {
        DataSet ds = new DataSet();
        try
        {

            string dateFrom = " ";
            if (txtDate.Text != "")
            {
                dateFrom = " AND Date>='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' ";
            }

            string dateTo = " ";
            if (txtDate.Text != "")
            {
                dateTo = " AND Date<='" + Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd") + "' ";
            }

            string query = " FROM PrdPlasticCont a where pid<>0 " + dateFrom + dateTo;
            string url = "";// SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";

            query = @" SELECT (CONVERT(varchar,Date,103) +'') AS Date, (Select Company from Party where PartyID=a.CustomerID) AS  [Customer], 
                        (Select BrandName from CustomerBrands where BrandID=a.Brand) AS  [Brand], 
                        (Select BrandName from Brands where BrandID=a.PackSize) AS  [Pack Size], 
                        (SELECT ItemName FROM Products WHERE (ProductID = a.ItemID)) AS [Product Name], 
                          (SELECT [DepartmentName] FROM [Colors] Where Departmentid=a.Color) as Color, FinalProduction as Qty, FinalKg as Weight " + query;
            ds = SQLQuery.ReturnDataSet(query);

            ltrtotal.Text = "Total Result: " + ds.Tables[0].Rows.Count.ToString();
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
            return null;
        }

        /*
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand(@"SELECT  Id, (SELECT  [Purpose] FROM [Purpose] WHERE [pid]= StockinDetailsRaw.Purpose) AS Purpose, 
ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, QtyBalance, DeliveredQty, UnitWeight, 
(SELECT  [Company] FROM [Party] WHERE [PartyID]= StockinDetailsRaw.Customer) AS Customer,
(SELECT [BrandName] FROM [CustomerBrands] WHERE BrandID=StockinDetailsRaw.BrandID) AS BrandID,
(SELECT [BrandName] FROM [Brands] WHERE BrandID=StockinDetailsRaw.SizeId) AS SizeId,
(SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=StockinDetailsRaw.Color) AS Color,
(SELECT [Spec] FROM [Specifications] WHERE id=StockinDetailsRaw.Spec) AS Spec
FROM StockinDetailsRaw WHERE GodownID='" + ddGodown.SelectedValue + "' AND EntryBy=@EntryBy AND OrderID='' AND StockType='Raw'  ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //cmd.CommandText = "SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal FROM StockinDetailsRaw WHERE EntryBy=@EntryBy AND OrderID='' ORDER BY Id";

        cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        cmd.Connection.Open();
        //ItemGrid.EmptyDataText = "No items to view...";
        //ItemGrid.DataSource = cmd.ExecuteReader();
        //ItemGrid.DataBind();
        cmd.Connection.Close();
        */

    }


    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(QtyBalance),0) from StockinDetailsRaw where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "pcs, " +
        //    RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(UnitWeight),0) from StockinDetailsRaw where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "kg";

    }


    private void QtyinStock()
    {
        try
        {

            GridView1.DataBind();
            BindItemGrid();
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
        //finally { LoadFormControls(); }

    }

    protected void ddGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        BindItemGrid();
    }

    protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        BindItemGrid();
    }

    protected void btnExport_OnClick(object sender, EventArgs e)
    {
        //exportExcel(BindItemGrid(), "Plastic-Container-History");

        string dateFrom = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

        string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") +
            "XerpReports/FormDailyPrdn.aspx?dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&type=summery";

        if1.Attributes.Add("src", url);
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


    protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        GetGrade();
        GetCategory();
        BindItemGrid();
        GetpackSize();

    }

    protected void ddGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetCategory();
    }

    protected void ddcategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetProductList();
    }
    private void GetProductList()
    {
        //string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategory.SelectedValue +
        //                "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        //SQLQuery.PopulateDropDown(gQuery, ddItemName, "ProductID", "ItemName");

        //ltrUnitType.Text = SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddItemName.SelectedValue + "'");

        //if (IsPostBack)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
        //}

        //LoadItemsPanel();
    }

    protected void ddGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnAllShift_OnClick(object sender, EventArgs e)
    {
        string dateFrom = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

        string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") +
            "XerpReports/DailyPrdnAllShift.aspx?dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&type=AllShift";

        if1.Attributes.Add("src", url);
    }
}