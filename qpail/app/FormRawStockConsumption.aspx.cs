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

public partial class app_FormRawStockConsumption : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        btnSearch.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSearch, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            txtDate.Text = "01" + DateTime.Now.ToString("dd/MM/yyyy").Substring(2);
            string lName = Page.User.Identity.Name.ToString();
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
            QtyinStock();
            GetGrade();
            GetCategory();
            ////BindItemGrid();

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
        ////BindItemGrid();

        string dateFrom = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/FormRawStockConsumptionReport.aspx?godown=" + ddGodown.SelectedValue + "&purpos=" + ddPurpose.SelectedValue + "&subgro=" + ddSubGroup.SelectedValue + "&Grade =" + ddGrade.SelectedValue + "&Category=" + ddcategory.SelectedValue + "&Type=" + ddType.SelectedValue + "&dt1=" + dateFrom + "&dt2=" + dateTo;
        if1.Attributes.Add("src", urlx);
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
            //BindItemGrid();
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
        //BindItemGrid();
    }

    protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        //BindItemGrid();
    }

    protected void btnExport_OnClick(object sender, EventArgs e)
    {
        //exportExcel(BindItemGrid(), "Raw Stock");
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
        //BindItemGrid();

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