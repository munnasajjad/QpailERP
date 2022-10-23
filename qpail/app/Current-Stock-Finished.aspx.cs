using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using RunQuery;

public partial class app_Current_Stock_Finished : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        btnSearch.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSearch, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            ddGroup.DataBind();
            ddGroup.SelectedValue = "1";
            ddSubGroup.DataBind();

            ddGodown.DataBind();
            ddCustomer.DataBind();
            ddBrand.DataBind();

            //QtyinStock();
            //BindItemGrid();
        }
        //txtInv.Text = InvIDNo();
    }

    //Messagebox For Alerts
    //private void MessageBox(string msg)
    //{
    //    Label lbl = new Label();
    //    lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
    //    Page.Controls.Add(lbl);
    //}

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //BindItemGrid();

        string dt1 = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormCurrentStockFinished.aspx?godown=" + ddGodown.SelectedValue + "&location=" + ddLocation.SelectedValue + "&pacsize=" + ddSize.SelectedValue + "&subgro=" + ddSubGroup.SelectedValue + "&cust=" + ddCustomer.SelectedValue + "&brand=" + ddBrand.SelectedValue + "&dt1=" + dt1;
        if1.Attributes.Add("src", urlx);
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
            string location = " ";
            if (ddLocation.SelectedValue != "--- all ---")
            {
                location = " AND LocationID='" + ddLocation.SelectedValue + "' ";
            }
            string packSize = " ";
            if (ddSize.SelectedValue != "--- all ---")
            {
                packSize = " AND SizeID='" + ddSize.SelectedValue + "' ";
            }
            string subGroup = " ";
            if (ddSubGroup.SelectedValue != "--- all ---")
            {
                subGroup = " AND ProductID IN (Select ProductID from Products where CategoryID in (SELECT CategoryID FROM Categories WHERE GradeID IN (SELECT GradeID FROM ItemGrade WHERE CategoryID ='" + ddSubGroup.SelectedValue + "'))) ";
            }

            string customer = " ";
            if (ddCustomer.SelectedValue != "--- all ---")
            {
                customer = " AND Customer='" + ddCustomer.SelectedValue + "' ";
            }
            string brand = " ";
            if (ddBrand.SelectedValue != "--- all ---")
            {
                brand = " AND BrandID='" + ddBrand.SelectedValue + "' ";
            }

            string date = " ";
            if (txtDate.Text != "")
            {
                date = " AND EntryDate<='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' ";
            }

            string query = " FROM Stock where ItemGroup='2' " + godown + location + packSize + customer + brand + subGroup + date;
            string url = "";// SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";


            //if (ddSubGroup.SelectedValue == "10") //printing ink
            //{
            //query = " FROM Stock where ItemGroup='1' " + godown + subGroup + date;
            //            ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
            //                                              (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
            //                                            "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
            //                                            " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName, " +
            //                                            "  ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty,  ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
            //                                            "  " +
            //                                            "GROUP BY WarehouseID, ProductID ");
            //            }
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

            query = @"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = Stock.WarehouseID)) AS Warehouse, 
                  (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))) AS Grade, " +
                "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID))) AS Category, " +
                " (SELECT ItemName FROM Products WHERE (ProductID = Stock.ProductID)) AS ProductName, (SELECT Company FROM Party WHERE (PartyID = Stock.Customer)) AS Customer, " +
                " (SELECT BrandName FROM CustomerBrands WHERE (BrandID = Stock.BrandID)) AS BrandID, (SELECT [BrandName] FROM [Brands] WHERE BrandID=Stock.SizeId) AS SizeId, " +
                " ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " +
                query + " " + "GROUP BY WarehouseID, ProductID, Customer, BrandID, SizeId HAVING (ISNULL(SUM(InQuantity), 0) - ISNULL(SUM(OutQuantity), 0) <> 0)";
            ds = SQLQuery.ReturnDataSet(query);
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

            ltrtotal.Text = "Total Result: " + ds.Tables[0].Rows.Count.ToString();
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "ERROR: " + ex.ToString();
        }
        finally
        {

        }

        return ds.Tables[0];
    }


    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(QtyBalance),0) from StockinDetailsRaw where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "pcs, " +
        //    RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(UnitWeight),0) from StockinDetailsRaw where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "kg";

    }

    protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //BindItemGrid();
    }



    protected void ddGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddLocation.Items.Clear();
        ddLocation.Items.Add("--- all ---");
        ddLocation.DataBind();
        //BindItemGrid();
    }

    protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //BindItemGrid();
    }

    protected void btnExport_OnClick(object sender, EventArgs e)
    {
        exportExcel(BindItemGrid(), "Finished Stock");
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

    protected void ddCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand.DataBind();
        //BindItemGrid();
    }

    protected void ddBrand_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //BindItemGrid();
    }

    protected void btnPrint_OnClick(object sender, EventArgs e)
    {
        //GridView1.AllowPaging = false;
        //GridView1.DataBind();
        //BindItemGrid();

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        GridView1.RenderControl(hw);
        string gridHTML = sw.ToString().Replace("\"", "'").Replace(System.Environment.NewLine, "");
        StringBuilder sb = new StringBuilder();
        sb.Append("<script type = 'text/javascript'>");
        sb.Append("window.onload = new function(){");
        sb.Append("var printWin = window.open('', '', 'left=0");
        sb.Append(",top=0,width=1000,height=600,status=0');");
        sb.Append("printWin.document.write(\"");
        sb.Append(gridHTML);
        sb.Append("\");");
        sb.Append("printWin.document.close();");
        sb.Append("printWin.focus();");
        sb.Append("printWin.print();");
        sb.Append("printWin.close();};");
        sb.Append("</script>");
        ClientScript.RegisterStartupScript(this.GetType(), "GridPrint", sb.ToString());
        //GridView1.AllowPaging = true;
        //GridView1.DataBind();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }
}