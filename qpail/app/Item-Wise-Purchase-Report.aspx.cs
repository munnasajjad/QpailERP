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

public partial class app_Item_Wise_Purchase_Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string dateM = DateTime.Now.Month.ToString();
            string dateY = DateTime.Now.Year.ToString();

            if (dateM.Length == 1)
            {
                dateM = "0" + dateM;
            }
            txtDateFrom.Text = "01/" + dateM + "/" + dateY;
            txtDateTo.Text = DateTime.Now.ToShortDateString();

            ddGroup.DataBind();
            bindGrp();
            CheckItemType(Convert.ToInt32(ddGroup.SelectedValue));
            //LoadGridData();
        }
    }

    private DataTable LoadGridData()
    {
        try
        {
            decimal total2 = (decimal)0.0;
            decimal qty2 = (decimal)0.0;
            decimal vat2 = (decimal)0.0;
            decimal TotalSales2 = (decimal)0.0;
            decimal TotalWeight2 = (decimal)0.0;

            string lName = Page.User.Identity.Name.ToString();
            string iName = ddItemName.SelectedValue;

            DateTime dtFrom = Convert.ToDateTime(txtDateFrom.Text);
            DateTime dtTo = Convert.ToDateTime(txtDateTo.Text);

            string query = "  ";

            if (ddSubGrp.SelectedValue != "0")//get all sub-group from item group
            {
                if (ddGrade.SelectedValue != "0") //get all grade from item sub-group
                {
                    if (ddCategory.SelectedValue != "0")
                    {
                        if (ddItemName.SelectedValue != "0")
                        {
                            query = "  PurchaseDetails.ItemCode  ='" + ddItemName.SelectedValue + "'  AND   ";
                        }
                        else
                        {
                            query = "  PurchaseDetails.ItemCode IN  (SELECT ProductID from [Products] where CategoryID  ='" + ddCategory.SelectedValue + "')  AND ";
                        }
                    }
                    else
                    {
                        query = "  PurchaseDetails.ItemCode IN (SELECT ProductID from [Products] where CategoryID IN(SELECT CategoryID from [Categories] where GradeID ='" + ddGrade.SelectedValue + "'))  AND ";
                    }
                }
                else
                {
                    query = "  PurchaseDetails.ItemCode IN (SELECT ProductID from [Products] where CategoryID IN(SELECT CategoryID from [Categories] where GradeID IN(SELECT GradeID FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "')) )  AND ";
                }
            }
            else if (ddGroup.SelectedValue != "0")//get all sub-group from item group
            {
                query = "  PurchaseDetails.ItemCode IN (SELECT ProductID from [Products] where CategoryID IN(SELECT CategoryID from [Categories] where GradeID IN(SELECT GradeID FROM [ItemGrade] where CategoryID IN(SELECT CategoryID FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue + "'))) )  AND ";
            }
            query += "";
            string str = @"SELECT        Purchase.BillNo, Purchase.BillDate, Purchase.SupplierName, Purchase.ChallanNo, Purchase.ChallanDate, PurchaseDetails.ItemName, PurchaseDetails.Manufacturer, PurchaseDetails.CountryOfOrigin, 
                         PurchaseDetails.PackSize, PurchaseDetails.Warrenty, PurchaseDetails.SerialNo, PurchaseDetails.ModelNo, PurchaseDetails.Specification, PurchaseDetails.UnitType, PurchaseDetails.SizeRef, 
                         PurchaseDetails.Qty, PurchaseDetails.Price, PurchaseDetails.SubTotal, PurchaseDetails.ItemDisc, PurchaseDetails.ItemVAT, PurchaseDetails.Total, PurchaseDetails.PriceWithVAT, 
                         PurchaseDetails.PriceWithoutVAT, Purchase.PurchaseDiscount, Purchase.OtherExp, Purchase.PurchaseTotal
                         FROM            PurchaseDetails INNER JOIN
                         Purchase ON Purchase.InvNo = PurchaseDetails.InvNo
                        where " + query + "  Purchase.BillDate >= '" +
                         dtFrom.ToString("yyyy-MM-dd") + "' AND Purchase.BillDate <= '" + dtTo.ToString("yyyy-MM-dd") +
                         "' ORDER BY Purchase.BillDate, PurchaseDetails.Id ";

            DataTable dt = RunQuery.SQLQuery.ReturnDataTable(str);

            GVrpt.DataSource = dt;
            GVrpt.DataBind();

            ltrQty.Text = qty2.ToString();
            ltrItemLoad.Text = total2.ToString();
            ltrTotalVat.Text = vat2.ToString();
            ltrGTAmt.Text = TotalSales2.ToString();
            ltrTotalWeight.Text = TotalWeight2.ToString();
            return dt;
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
            return null;
        }

    }

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    private decimal total = (decimal)0.0;
    private decimal qty = (decimal)0.0;
    private decimal TotalSales = (decimal)0.0;
    private decimal TotalWeight = (decimal)0.0;
    private decimal TotalPrice = (decimal)0.0;
    private decimal TtlSubTotal = (decimal)0.0;
    private decimal TotalItemDisc = (decimal)0.0;
    private decimal TotalItemVAT = (decimal)0.0;

    private decimal PurchaseDiscount = (decimal)0.0;
    private decimal OtherExp = (decimal)0.0;
    private decimal PurchaseTotal = (decimal)0.0;
    protected void GVrpt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            qty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Qty"));

            TotalPrice += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Price"));
            TtlSubTotal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "SubTotal"));
            TotalItemDisc += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ItemDisc"));
            TotalItemVAT += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ItemVAT"));

            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Total"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PriceWithoutVAT"));
            TotalWeight += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PriceWithVAT"));

            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PurchaseDiscount"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OtherExp"));
            TotalWeight += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PurchaseTotal"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[7].Text = "Total";
            e.Row.Cells[8].Text = Convert.ToString(qty);

            e.Row.Cells[10].Text = Convert.ToString(TotalPrice);
            e.Row.Cells[11].Text = Convert.ToString(TtlSubTotal);
            e.Row.Cells[12].Text = Convert.ToString(TotalItemDisc);
            e.Row.Cells[13].Text = Convert.ToString(TotalItemVAT);

            e.Row.Cells[14].Text = Convert.ToString(total);

            e.Row.Cells[15].Text = Convert.ToString(PurchaseDiscount);
            e.Row.Cells[16].Text = Convert.ToString(OtherExp);
            e.Row.Cells[17].Text = Convert.ToString(PurchaseTotal);

            //e.Row.Cells[13].Text = Convert.ToString(TotalSales - total);
            //e.Row.Cells[13].Text = Convert.ToString(TotalSales);  //String.Format("{0:c}", TotalSales);
            //e.Row.Cells[11].Text = Convert.ToString(TotalWeight);  //String.Format("{0:c}", TotalSales);
        }
    }

    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDateTime(txtDateFrom.Text) <= Convert.ToDateTime(txtDateTo.Text))
            {
                LoadGridData();
                //SecondGrid();
                string dtFrom = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
                string dtTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");
                string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/Item-Wise-Purchase-Report.aspx?Group=" + ddGroup.SelectedValue + "&SubGrp=" + ddSubGrp.SelectedValue + "&Grade=" + ddGrade.SelectedValue + "&Category=" + ddCategory.SelectedValue + "&ItemName=" + ddItemName.SelectedValue + "&dt1=" + dtFrom + "&dt2=" + dtTo;
                if1.Attributes.Add("src", urlx);


            }
            else
            {
                MessageBox("Invalid Date Range!");
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.ToString();
            MessageBox("Invalid Data! Check Error Message...");
        }
    }

    private int Quantity(string iName, string dtFrom, string dtTo)
    {
        SqlCommand cmd2 = new SqlCommand("SELECT ISNULL(SUM(Quantity),0) FROM [SaleDetails] WHERE ([productName] = '" + iName + "') and EntryDate >= @DateFrom AND EntryDate <= @DateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = dtFrom;
        cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = dtTo;
        cmd2.Connection.Open();
        int Qty = Convert.ToInt32(cmd2.ExecuteScalar());
        cmd2.Connection.Close();
        return Qty;
    }

    private string ProjectID(string lName)
    {
        //SqlCommand cmd3z = new SqlCommand("SELECT ProjectName FROM Projects where VID=(SELECT ProjectID  FROM Logins where LoginUserName='" + lName + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        SqlCommand cmd3z = new SqlCommand("SELECT ProjectID  FROM Logins where LoginUserName='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd3z.Connection.Open();
        string prjName = Convert.ToString(cmd3z.ExecuteScalar());
        cmd3z.Connection.Close();
        cmd3z.Connection.Dispose();
        return prjName;
    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string url = "./rptLedger.aspx?party=" + "" + "&dt1=" + txtDateFrom.Text + "&dt2=" + txtDateTo.Text;
        ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");
    }

    protected void btnExport_OnClick(object sender, EventArgs e)
    {
        //exportExcel(LoadGridData(), "Item-Purchase-History-Report");
    }
    private void exportExcel(DataTable data, string reportName)
    {
        var wb = new XLWorkbook();

        // Add DataTable as Worksheet
        wb.Worksheets.Add(data, reportName);

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

    public static class ResponseHelper
    {
        public static void Redirect(string url, string target, string windowFeatures)
        {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures))
            {

                context.Response.Redirect(url);
            }
            else
            {
                Page page = (Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException(
                        "Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(Page),
                    "Redirect",
                    script,
                    true);
            }
        }
    }

    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddItem_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    //protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (CheckBox1.Checked)
    //    {
    //        ddItemName.Enabled = false;
    //    }
    //    else
    //    {
    //        ddItemName.Enabled = true;
    //    }

    //    LoadGridData();
    //}

    protected void GVrpt_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVrpt.PageIndex = e.NewPageIndex;
        LoadGridData();
        GVrpt.PageIndex = e.NewPageIndex;
    }

    protected void ddGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddItemName.DataBind();
    }

    protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGrp();
        CheckItemType(Convert.ToInt32(ddGroup.SelectedValue));
    }

    private void bindGrp()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDownWithAll(gQuery, ddSubGrp, "CategoryID", "CategoryName");

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDownWithAll(gQuery, ddGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDownWithAll(gQuery, ddCategory, "CategoryID", "CategoryName");
        GetProductList();
    }

    protected void ddSubGrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDownWithAll(gQuery, ddGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDownWithAll(gQuery, ddCategory, "CategoryID", "CategoryName");
        GetProductList();
    }
    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDownWithAll(gQuery, ddCategory, "CategoryID", "CategoryName");
        GetProductList();
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetProductList();
    }

    private void GetProductList()
    {
        if (ddCategory.SelectedValue != "")
        {
            string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategory.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
            RunQuery.SQLQuery.PopulateDropDownWithAll(gQuery, ddItemName, "ProductID", "ItemName");

            //txtUnit.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddItemName.SelectedValue + "'");
            LoadSpecList("filter");

            if (IsPostBack)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
            }

            CheckItemType(Convert.ToInt32(ddGroup.SelectedValue));
            //recentInfo();
        }
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSpecList("filter");
        //txtQuantity.Focus();
        //recentInfo();
    }

    private void LoadSpecList(string filterDD)
    {
        //string gQuery = "SELECT [id], [spec] FROM [Specifications] ORDER BY [spec]";
        //lbFilter.Text = "Filter";
        //if (filterDD != "")
        //{
        //    lbFilter.Text = "Show-all";
        //    gQuery = "SELECT [id], [spec] FROM [Specifications] where  CAST(id AS nvarchar) in (Select distinct spec from stock where ProductID='" + ddItemName.SelectedValue + "') ORDER BY [spec]";
        //}

        //SQLQuery.PopulateDropDown(gQuery, ddSpec, "id", "spec");

    }
    private void CheckItemType(int iGrp)
    {
        if (ddSubGrp.SelectedValue == "10")
        {
            LoadSpecList("filter");
            //pnlSpec.Visible = true;
        }
        else
        {
            //pnlSpec.Visible = false;
        }


        if (iGrp <= 3)
        {
            //ddStockType.SelectedValue = "Raw";
            //ltrWarrenty.Text = "Qnty./Pack (Kg): ";
            //ltrSerial.Text = "No. of Packs: ";
            //PanelMachine.Visible = true;

            string subGrp = "";
            if (ddSubGrp.SelectedValue != "")
            {
                subGrp = ddSubGrp.SelectedItem.Text;
            }

            if (subGrp == "Tin Plate")
            {
                //pkSizeField.Attributes.Remove("class");
                //pkSizeField.Attributes.Add("class", "control-group");
            }
            else
            {
                //pkSizeField.Attributes.Remove("class");
                //pkSizeField.Attributes.Add("class", "control-group hidden");
                //SectionField.Attributes.Remove("class");
                //SectionField.Attributes.Add("class", "control-group hidden");
            }

        }
        else
        {
            //ddStockType.SelectedValue = "Fixed";
            //ltrWarrenty.Text = "Warrentry : ";
            //ltrSerial.Text = "Serial No. : ";
        }

        if (ddGroup.SelectedValue == "5")
        {
            //SectionField.Attributes.Remove("class");
            //SectionField.Attributes.Add("class", "control-group");
        }
    }
}