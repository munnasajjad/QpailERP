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

public partial class app_FixedAssetlist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSearch.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSearch, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            //txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDateFrom.Text = "01/" + DateTime.Now.ToString("MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            //ddGroup.DataBind();
            //ddGroup.SelectedValue = "1";
            //ddSubGroup.DataBind();

            //ddGodown.DataBind();
            ////QtyinStock();
            //BindItemGrid();
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
        //BindItemGrid();
        string dt1 = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
        string dt2 = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormFixedAssetsLists.aspx?item=" + ddGroup.SelectedValue + "&dt1=" + dt1+ "&dt2=" + dt2;
        if1.Attributes.Add("src", urlx);
    }

    //private DataTable BindItemGrid()
    //{
    //    DataSet ds = new DataSet();
    //    try
    //    {
    //        string godown = " ";
    //        if (ddGodown.SelectedValue != "--- all ---")
    //        {
    //            godown = " AND WarehouseID='" + ddGodown.SelectedValue + "' ";
    //        }
    //        //string purpose = " ";
    //        //if (ddPurpose.SelectedValue != "--- all ---")
    //        //{
    //        //    purpose = " AND Purpose='" + ddPurpose.SelectedValue + "' ";
    //        //}
    //        string subGroup = " ";
    //        if (ddSubGroup.SelectedValue != "--- all ---")
    //        {
    //            subGroup = " AND ProductID IN (Select ProductID from Products where CategoryID in (SELECT CategoryID FROM Categories WHERE GradeID IN (SELECT GradeID FROM ItemGrade WHERE CategoryID ='" + ddSubGroup.SelectedValue + "'))) ";
    //        }
    //        string date = " ";
    //        if (txtDate.Text != "")
    //        {
    //            date = " AND EntryDate<='" + Convert.ToDateTime(txtDate.Text).AddDays(1).ToString("yyyy-MM-dd") + "' ";
    //        }

    //        string query = " FROM Stock where EntryID<>0 " + godown + subGroup + date;
    //        string url = "";// SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";


    //        //if (ddSubGroup.SelectedValue == "10") //printing ink
    //        //{
    //        //query = " FROM Stock where ItemGroup='1' " + godown + subGroup + date;
    //        ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
    //                                          (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
    //                                        " (SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
    //                                        " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName, " +
    //                                        " ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty,  ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
    //                                        " " +
    //                                        " GROUP BY WarehouseID, ProductID HAVING (ISNULL(SUM(InQuantity), 0) - ISNULL(SUM(OutQuantity), 0) <> 0)");
    //        //            }
    //            else if (ddSubGroup.SelectedValue != "9") //Not Tin Plate : Plastic Raw materials
    //            {
    //                ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
    //                                              (Select Purpose from Purpose where pid=Stock.Purpose) AS Purpose," +
    //                                            " (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
    //                                                "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
    //                                                " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName, " +
    //                                                "  ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
    //                                                "  " +
    //                                                "GROUP BY WarehouseID, Purpose, ProductID ");
    //            }
    //            else
    //            {

    //                if (ddType.SelectedValue == "Printed Sheet")
    //                {
    //                    query += " AND ItemType='" + ddType.SelectedValue + "' ";
    //                    ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
    //                                 (Select Purpose from Purpose where pid=Stock.Purpose) AS Purpose, (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
    //                                "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
    //                                " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName, (SELECT Company FROM Party WHERE (PartyID = Stock.Customer)) AS Customer, " +
    //                                " (SELECT BrandName FROM CustomerBrands WHERE (BrandID = Stock.BrandID)) AS BrandID, (SELECT [BrandName] FROM [Brands] WHERE BrandID=Stock.SizeId) AS SizeId, " +
    //                                "(SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=Stock.Color) AS Color,  " +
    //                                " ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
    //                                "  " +
    //                                "GROUP BY WarehouseID, Purpose, ProductID, Customer, BrandID, SizeId, Color ");
    //                }
    //                else //Raw Sheet
    //                {
    //                    query += " AND ItemType='" + ddType.SelectedValue + "' ";
    //                    ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
    //                                 (Select Purpose from Purpose where pid=Stock.Purpose) AS Purpose, (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
    //                                "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
    //                                " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName,  " +
    //                                " ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
    //                                "  " +
    //                                "GROUP BY WarehouseID, Purpose, ProductID ");
    //                }
    //            }

    //        ltrtotal.Text = "Total Result: " + ds.Tables[0].Rows.Count.ToString();
    //        GridView1.DataSource = ds.Tables[0];
    //        GridView1.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg2.Attributes.Add("class", "xerp_warning");
    //        lblMsg2.Text = "ERROR: " + ex.Message.ToString();
    //    }
    //    finally
    //    {

    //    }

    //    return ds.Tables[0];
    //}


    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(QtyBalance),0) from StockinDetailsRaw where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "pcs, " +
        //    RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(UnitWeight),0) from StockinDetailsRaw where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "kg";

    }

    //protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindItemGrid();
    //}



    //protected void ddGodown_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindItemGrid();
    //}

    //protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindItemGrid();
    //}

    protected void btnExport_OnClick(object sender, EventArgs e)
    {
       // exportExcel(BindItemGrid(), "Current-Stock-Other-Report");
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

    
    protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddSubGroup.DataBind();
        ddGrade.DataBind();

        ddCategory.DataBind();
        ddProduct.DataBind();
        QtyinFixedAssets();

    }

    protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddGrade.DataBind();
            ddCategory.DataBind();
            ddProduct.DataBind();
            QtyinFixedAssets();

        }
        catch (Exception ex)
        {

            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
    }


    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddCategory.DataBind();
        ddProduct.DataBind();
        QtyinFixedAssets();
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddProduct.DataBind();
        QtyinFixedAssets();
        ddCategory.Focus();
    }

    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinFixedAssets();
        txtQty.Focus();
        // 
    }


    private void QtyinFixedAssets()
    {
        try
        {
            ltrUnit.Text = SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");

            //txtCurrentQty.Text = Stock.Inventory.NonUsableFixedAssestsQty(ddProduct.SelectedValue, ddGodown.SelectedValue);
            SQLQuery.Empty2Zero(txtCurrentQty);
            ltrCQty.Text = txtCurrentQty.Text;

            //BindItemGrid();
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
        finally { }

    }

    
}