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

public partial class app_rptRawConsumption : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSearch.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSearch, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDate.Text = DateTime.Now.ToString("01/MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            ddGroup.DataBind();
            ddGroup.SelectedValue = "1";
            ddSubGroup.DataBind();

            ddGodown.DataBind();
            //QtyinStock();
            BindItemGrid();
        }
        //txtInv.Text = InvIDNo();
    }

    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        BindItemGrid();


        //string dt1 = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");

        //string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormStockInOut.aspx?godown=" + ddGodown + "&purpos=" + ddPurpose + "&subgro=" + ddSubGroup + "&dt1=" + dt1;
        //if1.Attributes.Add("src", urlx);
    }

    private DataTable BindItemGrid()
    {
        DataSet ds = new DataSet();
        try
        {
            string godown = " ";
            if (ddGodown.SelectedValue != "--- all ---")
            {
                godown = " AND WarehouseID='" + ddGodown.SelectedValue + "' ";
            }
            string subGroup = " ";
            if (ddSubGroup.SelectedValue != "--- all ---")
            {
                subGroup = " AND ProductID IN (Select ProductID from Products where CategoryID in (SELECT CategoryID FROM Categories WHERE GradeID IN (SELECT GradeID FROM ItemGrade WHERE CategoryID ='" + ddSubGroup.SelectedValue + "'))) ";
            }
            string date = " ";
            if (txtDate.Text != "")
            {
                date = " AND EntryDate<'" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' ";
            }

            string query = " FROM Stock where ItemGroup='1' " + godown + subGroup + date;
            string url = "";// SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";


            if (ddSubGroup.SelectedValue == "10") //printing ink
            {
                query = " FROM Stock where ItemGroup='1' " + godown + subGroup + date;
                ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
                                              (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
                                                "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
                                                " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName, " +
                                                " (SELECT [spec] FROM [Specifications] WHERE  CAST(id AS nvarchar)=Stock.Spec) AS Spec, " +
                                                "  ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
                                                "  " +
                                                "GROUP BY WarehouseID, ProductID, Spec ");
            }
            else if (ddSubGroup.SelectedValue != "9") //Not Tin Plate : Plastic Raw materials
            {
                ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
                                              (Select Purpose from Purpose where pid=Stock.Purpose) AS Purpose," +
                                            " (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
                                                "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
                                                " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName, " +
                                                "  ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, " +
                                                "  ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
                                                "  " +
                                                "GROUP BY WarehouseID, Purpose, ProductID ");
            }
            else
            {
                if (ddType.SelectedValue == "Printed Sheet")
                {
                    query += " AND ItemType='" + ddType.SelectedValue + "' ";
                    ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
                                 (Select Purpose from Purpose where pid=Stock.Purpose) AS Purpose, (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
                                "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
                                " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName, (SELECT Company FROM Party WHERE (PartyID = Stock.Customer)) AS Customer, " +
                                " (SELECT BrandName FROM CustomerBrands WHERE (BrandID = Stock.BrandID)) AS BrandID, (SELECT [BrandName] FROM [Brands] WHERE BrandID=Stock.SizeId) AS SizeId, " +
                                "(SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=Stock.Color) AS Color,  " +
                                " ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
                                "  " +
                                "GROUP BY WarehouseID, Purpose, ProductID, Customer, BrandID, SizeId, Color ");
                }
                else //Raw Sheet
                {
                    query += " AND ItemType='" + ddType.SelectedValue + "' ";
                    ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
                                 (Select Purpose from Purpose where pid=Stock.Purpose) AS Purpose, (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
                                "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
                                " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName,  " +
                                " ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
                                "  " +
                                "GROUP BY WarehouseID, Purpose, ProductID ");
                }
            }

            ltrtotal.Text = "Total Result: " + ds.Tables[0].Rows.Count.ToString();
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
        finally
        {

        }

        return ds.Tables[0];
    }

    private void ItemIssued()
    {
        string godown = " ";
        if (ddGodown.SelectedValue != "--- all ---")
        {
            godown = " AND WarehouseID='" + ddGodown.SelectedValue + "' ";
        }
        string subGroup = " ";
        if (ddSubGroup.SelectedValue != "--- all ---")
        {
            subGroup = " AND ProductID IN (Select ProductID from Products where CategoryID in (SELECT CategoryID FROM Categories WHERE GradeID IN (SELECT GradeID FROM ItemGrade WHERE CategoryID ='" + ddSubGroup.SelectedValue + "'))) ";
        }
        string dateFrom = " ";
        if (txtDate.Text != "")
        {
            dateFrom = " AND EntryDate>='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' ";
        }
        string dateTo = " ";
        if (txtDate.Text != "")
        {
            dateTo = " AND EntryDate<='" + Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd") + "' ";
        }
        string query = " FROM Stock where ItemGroup='1' " + godown + subGroup + dateFrom + dateTo;
        string url = "";// SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";

        //Issued from store
        DataTable dataTable = SQLQuery.ReturnDataTable(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
                                              (Select Purpose from Purpose where pid=Stock.Purpose) AS Purpose," +
                                            " (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
                                                "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
                                                " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName, " +
                                                "  ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, " +
                                                "  ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
                                                "  " +
                                                "GROUP BY WarehouseID, Purpose, ProductID ");

    }

    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(QtyBalance),0) from StockinDetailsRaw where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "pcs, " +
        //    RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(UnitWeight),0) from StockinDetailsRaw where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "kg";

    }

    protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        BindItemGrid();
    }



    private void QtyinStock()
    {
        try
        {
            tinPlate.Attributes.Remove("class");
            tinPlate.Attributes.Add("class", "col-md-4 hidden");

            //InkGrid.Visible = false;
            //NonPrintedGrid.Visible = false;
            //PrintedGrid.Visible = false;
            //ltrUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");

            if (ddSubGroup.SelectedValue == "10") //printing ink
            {
                //InkGrid.Visible = true;
                //NonPrintedGrid.Visible = false;
                //PrintedGrid.Visible = false;

            }
            else if (ddSubGroup.SelectedValue == "9")
            {
                tinPlate.Attributes.Remove("class");
                tinPlate.Attributes.Add("class", "col-md-4");
            }
            else
            {
                if (ddType.SelectedValue == "Printed Sheet")
                {
                    //InkGrid.Visible = false;
                    //NonPrintedGrid.Visible = false;
                    //PrintedGrid.Visible = true;

                    //txtCurrentQty.Text = Stock.Inventory.AvailablePrintedQty(ddPurpose.SelectedValue, ddType.SelectedValue, ddProduct.SelectedValue,
                    //    ddCustomer.SelectedValue, ddBrand.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddGodown.SelectedValue);

                    //txtCurrentKg.Text = Stock.Inventory.AvailablePrintedWeight(ddPurpose.SelectedValue, ddType.SelectedValue, ddProduct.SelectedValue,
                    //    ddCustomer.SelectedValue, ddBrand.SelectedValue, ddSize.SelectedValue, ddColor.SelectedValue, ddGodown.SelectedValue);

                }
                else
                {
                    //InkGrid.Visible = false;
                    //NonPrintedGrid.Visible = true;
                    //PrintedGrid.Visible = false;

                    //txtCurrentQty.Text = Stock.Inventory.AvailableNonPrintedQty(ddPurpose.SelectedValue, ddType.SelectedValue, ddProduct.SelectedValue, ddGodown.SelectedValue);

                    //ltrCQty.Text = txtCurrentQty.Text;
                    //txtCurrentKg.Text = Stock.Inventory.AvailableNonPrintedWeight(ddPurpose.SelectedValue, ddType.SelectedValue, ddProduct.SelectedValue, ddGodown.SelectedValue);
                }
            }
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
        exportExcel(BindItemGrid(), "Raw Stock");
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

}