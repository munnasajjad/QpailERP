using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using RunQuery;


public partial class app_MachineStockRaw : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        btnSearch.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSearch, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDateFrom.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            ddGroup.DataBind();
            ddGroup.SelectedValue = "1";
            ddSubGroup.DataBind();

            ddGodown.DataBind();
            //QtyinStock();
            QtyinStock();
            GetGrade();
            GetCategory();
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

        string dt1 = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
        string dt2 = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");


        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormMachineStockRaw.aspx?subgro=" + ddSubGroup.SelectedValue + "&Grade =" + ddGrade.SelectedValue + "&Category=" + ddcategory.SelectedValue + "&Type=" + ddType.SelectedValue + "&dt1=" + dt1 + "&dt2=" + dt2;
        if1.Attributes.Add("src", urlx);
    }

    private DataTable BindItemGrid()
    {
        DataSet ds = new DataSet();

        try
        {


          

            string subGroup = " ";
            if (ddcategory.SelectedValue != "--- all ---")
            {
                subGroup = " AND ProductID IN (Select ProductID from Products where CategoryID  ='" +
                           ddcategory.SelectedValue + "') ";
            }
            else if (ddGrade.SelectedValue != "--- all ---")
            {
                subGroup =
                    " AND ProductID IN (Select ProductID from Products where CategoryID in (SELECT CategoryID FROM Categories WHERE GradeID IN (SELECT GradeID FROM ItemGrade WHERE CategoryID ='" +
                    ddSubGroup.SelectedValue + "'))) ";
            }
            else if (ddSubGroup.SelectedValue != "--- all ---")
            {
                subGroup =
                    " AND ProductID IN (Select ProductID from Products where CategoryID in (SELECT CategoryID FROM Categories WHERE GradeID IN (SELECT GradeID FROM ItemGrade WHERE CategoryID ='" +
                    ddSubGroup.SelectedValue + "'))) ";
            }

            string dateFrom = " ";
            if (txtDateFrom.Text != "")
            {
                dateFrom = " AND EntryDate<='" + Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd") + "' ";
            }
            string date = " ";
            if (txtDate.Text != "")
            {
                date = " AND EntryDate<='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' ";
            }

            string query = " FROM MachineStock where EntryId<>0 " + subGroup + date;
            string url = "";
            // SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";


            if (ddSubGroup.SelectedValue == "10") //printing ink
            {
                query = " FROM MachineStock where EntryId<>0 " + subGroup + date;
                ds = SQLQuery.ReturnDataSet(@"SELECT ProductID,ProductName,MachineID,tblFifo.InValue AS Price,SUM(InWeight) as ReceivedQty,SUM(OutWeight) as IssuedQty, (SUM(InWeight)-SUM(OutWeight)) AS Opening,((SUM(InWeight)-SUM(OutWeight))+SUM(InWeight)-SUM(OutWeight)) as Closing FROM MachineStock INNER JOIN tblFifo ON MachineStock.ProductID=tblFifo.ItemCode " + subGroup + " " + query + " Group by ProductName,ProductID,tblFifo.InValue,MachineID");
                //ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = MachineStock.WarehouseID)) AS Warehouse, 
                //                              (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = MachineStock.ProductID)))) AS Grade, " +
                //               "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = MachineStock.ProductID))) AS Category, " +
                //               " (SELECT ItemName FROM Products WHERE (ProductID = MachineStock.ProductID)) AS ProductName, " +
                //               " (SELECT [spec] FROM [Specifications] WHERE  CAST(id AS nvarchar)=MachineStock.Spec) AS Spec, " +
                //               "  ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " + query + " " +
                //               "  " +
                //               "GROUP BY WarehouseID, ProductID, Spec HAVING (ISNULL(SUM(InWeight), 0) - ISNULL(SUM(OutWeight), 0) <> 0)");
            }
            else if (ddSubGroup.SelectedValue == "33" || ddSubGroup.SelectedValue == "35") //HTF & IML
            {
                ds = SQLQuery.ReturnDataSet(@"SELECT ProductID,ProductName,MachineID,tblFifo.InValue AS Price,SUM(InWeight) as ReceivedQty,SUM(OutWeight) as IssuedQty, (SUM(InWeight)-SUM(OutWeight)) AS Opening,((SUM(InWeight)-SUM(OutWeight))+SUM(InWeight)-SUM(OutWeight)) as Closing FROM MachineStock INNER JOIN tblFifo ON MachineStock.ProductID=tblFifo.ItemCode " + subGroup + " " + date + " Group by ProductName,ProductID,tblFifo.InValue,MachineID");
                //ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = MachineStock.WarehouseID)) AS Warehouse, 
                //                              (Select Purpose from Purpose where pid=MachineStock.Purpose) AS Purpose," +
                //                " (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = MachineStock.ProductID)))) AS Grade, " +
                //                "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = MachineStock.ProductID))) AS Category, " +
                //                " (SELECT ItemName FROM Products WHERE (ProductID = MachineStock.ProductID)) AS ProductName, " +
                //                "  ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty " + query + " " +
                //                "  " +
                //                "GROUP BY WarehouseID, Purpose, ProductID  HAVING (ISNULL(SUM(InQuantity), 0) - ISNULL(SUM(OutQuantity), 0) <> 0)");
            }
            else if (ddSubGroup.SelectedValue == "12") //METAL HANDLE
            {
                ds = SQLQuery.ReturnDataSet(@"SELECT ProductID,ProductName,MachineID,tblFifo.InValue AS Price,SUM(InWeight) as ReceivedQty,SUM(OutWeight) as IssuedQty, (SUM(InWeight)-SUM(OutWeight)) AS Opening,((SUM(InWeight)-SUM(OutWeight))+SUM(InWeight)-SUM(OutWeight)) as Closing FROM MachineStock INNER JOIN tblFifo ON MachineStock.ProductID=tblFifo.ItemCode " + subGroup + " " + date + " Group by ProductName,ProductID,tblFifo.InValue,MachineID");
                //ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = MachineStock.WarehouseID)) AS Warehouse, 
                //                 (Select Purpose from Purpose where pid=MachineStock.Purpose) AS Purpose, (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = MachineStock.ProductID)))) AS Grade, " +
                //               "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = MachineStock.ProductID))) AS Category, " +
                //               " (SELECT ItemName FROM Products WHERE (ProductID = MachineStock.ProductID)) AS ProductName,  " +
                //               " ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " +
                //               query + " " +
                //               "  " +
                //               "GROUP BY WarehouseID, Purpose, ProductID HAVING (ISNULL(SUM(InQuantity), 0) - ISNULL(SUM(OutQuantity), 0) <> 0)");
            }
            else if (ddSubGroup.SelectedValue != "9")
            //Not Tin Plate : Plastic Raw materials || ddSubGroup.SelectedValue != "12"
            {
                
                ds = SQLQuery.ReturnDataSet(@"SELECT  ProductID,ProductName,MachineID,AVG(tblFifo.InValue) AS Price,SUM(InWeight) as ReceivedQty,SUM(OutWeight) as IssuedQty, (SUM(InWeight)-SUM(OutWeight)) AS Opening,((SUM(InWeight)-SUM(OutWeight))+SUM(InWeight)-SUM(OutWeight)) as Closing FROM MachineStock INNER JOIN tblFifo ON MachineStock.ProductID=tblFifo.ItemCode " + subGroup + " " + date + " Group by ProductName,ProductID,tblFifo.InValue,MachineID");
               
            }
            else
            {

                if (ddType.SelectedValue == "Printed Sheet")
                {
                    query += " AND ItemType='" + ddType.SelectedValue + "' ";
                    ds = SQLQuery.ReturnDataSet(@"SELECT  ProductID,ProductName,MachineID,AVG(tblFifo.InValue) AS Price,SUM(InWeight) as ReceivedQty,SUM(OutWeight) as IssuedQty, (SUM(InWeight)-SUM(OutWeight)) AS Opening,((SUM(InWeight)-SUM(OutWeight))+SUM(InWeight)-SUM(OutWeight)) as Closing FROM MachineStock INNER JOIN tblFifo ON MachineStock.ProductID=tblFifo.ItemCode " + subGroup + " " + query + " Group by ProductName,ProductID,tblFifo.InValue,MachineID");
                    //ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = MachineStock.WarehouseID)) AS Warehouse, 
                    //             (Select Purpose from Purpose where pid=MachineStock.Purpose) AS Purpose, (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = MachineStock.ProductID)))) AS Grade, " +
                    //        "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = MachineStock.ProductID))) AS Category, " +
                    //        " (SELECT ItemName FROM Products WHERE (ProductID = MachineStock.ProductID)) AS ProductName, (SELECT Company FROM Party WHERE (PartyID = MachineStock.Customer)) AS Customer, " +
                    //        " (SELECT BrandName FROM CustomerBrands WHERE (BrandID = MachineStock.BrandID)) AS BrandID, (SELECT [BrandName] FROM [Brands] WHERE BrandID=MachineStock.SizeId) AS SizeId, " +
                    //        "(SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=MachineStock.Color) AS Color,  " +
                    //        " ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " +
                    //        query + " " +
                    //        "  " +
                    //        "GROUP BY WarehouseID, Purpose, ProductID, Customer, BrandID, SizeId, Color HAVING (ISNULL(SUM(InQuantity), 0) - ISNULL(SUM(OutQuantity), 0) <> 0)");
                }
                else //Raw Sheet
                {
                    query += " AND ItemType='" + ddType.SelectedValue + "' ";
                    ds =SQLQuery.ReturnDataSet(@"SELECT  ProductID,ProductName,MachineID,AVG(tblFifo.InValue) AS Price,SUM(InWeight) as ReceivedQty,SUM(OutWeight) as IssuedQty, (SUM(InWeight)-SUM(OutWeight)) AS Opening,((SUM(InWeight)-SUM(OutWeight))+SUM(InWeight)-SUM(OutWeight)) as Closing FROM MachineStock INNER JOIN tblFifo ON MachineStock.ProductID=tblFifo.ItemCode " + subGroup + " " + query + " Group by ProductName,ProductID,tblFifo.InValue,MachineID");
                    //ds = SQLQuery.ReturnDataSet(@"SELECT DISTINCT (SELECT StoreName FROM Warehouses WHERE (WID = MachineStock.WarehouseID)) AS Warehouse, 
                    //             (Select Purpose from Purpose where pid=MachineStock.Purpose) AS Purpose, (SELECT GradeName FROM ItemGrade WHERE GradeID =(SELECT GradeID FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = MachineStock.ProductID)))) AS Grade, " +
                    //       "(SELECT CategoryName FROM Categories WHERE CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = MachineStock.ProductID))) AS Category, " +
                    //       " (SELECT ItemName FROM Products WHERE (ProductID = MachineStock.ProductID)) AS ProductName,  " +
                    //       " ISNULL(SUM(InQuantity),0)-ISNULL(SUM(OutQuantity),0) AS Qty, ISNULL(SUM(InWeight),0)-ISNULL(SUM(OutWeight),0) AS Weight " +
                    //       query + " " +
                    //       "  " +
                    //       "GROUP BY WarehouseID, Purpose, ProductID HAVING (ISNULL(SUM(InQuantity), 0) - ISNULL(SUM(OutQuantity), 0) <> 0)");
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


    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(QtyBalance),0) from StockinDetailsRaw where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "pcs, " +
        //    RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(UnitWeight),0) from StockinDetailsRaw where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "kg";

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
       // QtyinStock();
      //  BindItemGrid();
    }

    protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    {
       // QtyinStock();
       // BindItemGrid();
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

    private void GetGrade()
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
    }
    private void GetCategory()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        SQLQuery.PopulateDropDown(gQuery, ddcategory, "CategoryID", "CategoryName");
    }


    protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        GetGrade();
        GetCategory();
        BindItemGrid();

    }

    protected void ddGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetCategory();
    }

    protected void ddcategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //GetCategory();
    }

    protected void btnPrint_OnClick(object sender, EventArgs e)
    {
        //GridView1.AllowPaging = false;
        //GridView1.DataBind();
        BindItemGrid();

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