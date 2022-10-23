using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using RunQuery;


public partial class Operator_Sale_Item_Report : System.Web.UI.Page
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

            ddCustomer.DataBind();
            ddGrade.DataBind();
            ddItem.DataBind();
            LoadGridData();
        }
    }

    private void LoadGridData()
    {
        /*
         decimal total2 = (decimal)0.0;
         decimal qty2 = (decimal)0.0;
         decimal vat2 = (decimal)0.0;
         decimal TotalSales2 = (decimal)0.0;
         decimal TotalWeight2 = (decimal)0.0;
         */
        string lName = Page.User.Identity.Name.ToString();
        string prjID = ProjectID(lName);
        string iName = ddItem.SelectedValue;

        string sizeId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) SizeId FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");
        string pId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) ProductID FROM [SaleDetails]  WHERE ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID='" + ddGrade.SelectedValue + "')) AND  ([productName] = '" + iName + "')");
        string brandId = RunQuery.SQLQuery.ReturnString("SELECT TOP(1) BrandID FROM [SaleDetails] WHERE ([productName] = '" + iName + "')");

        string dtFrom = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
        string dtTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");


        /*
        DataSet ds = new DataSet();

        DataTable dt1 = new DataTable();
        DataRow dr1 = null;

        dt1.Columns.Add(new DataColumn("InvNo", typeof(string)));
        dt1.Columns.Add(new DataColumn("LINK", typeof(string)));
        dt1.Columns.Add(new DataColumn("EntryDate", typeof(string)));
        dt1.Columns.Add(new DataColumn("ProductName", typeof(string)));
        dt1.Columns.Add(new DataColumn("Quantity", typeof(string)));
        dt1.Columns.Add(new DataColumn("UnitCost", typeof(string)));
        dt1.Columns.Add(new DataColumn("UnitWeight", typeof(string)));
        dt1.Columns.Add(new DataColumn("ItemTotal", typeof(string)));
        dt1.Columns.Add(new DataColumn("TotalWeight", typeof(string)));
        dt1.Columns.Add(new DataColumn("VAT", typeof(string)));
        dt1.Columns.Add(new DataColumn("TotalAmt", typeof(string)));

        string str = "SELECT [InvNo], [ProductName], [Quantity], [UnitType], [UnitCost], [ItemTotal], [UnitWeight], [TotalWeight], [VAT], NetAmount, [EntryDate], [ReturnQty] FROM [SaleDetails] WHERE ([SizeId] = '" +
            sizeId + "') and ([ProductID] = '" + pId + "') and([BrandID] = '" + brandId + "') and InvNo IN (Select InvNo from Sales WHERE  InvDate >= '" +
            dtFrom.ToString("yyyy-MM-dd") + "' AND InvDate <= '" + dtTo.ToString("yyyy-MM-dd") + "') ORDER BY Id DESC ";

        if (CheckBox1.Checked)
        {
            str = "SELECT [InvNo], [ProductName], [Quantity], [UnitType], [UnitCost], [ItemTotal], [UnitWeight], [TotalWeight], [VAT], NetAmount, [EntryDate], [ReturnQty] FROM [SaleDetails] WHERE InvNo IN (Select InvNo from sales where ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID='"+ddGrade.SelectedValue+"')) AND CustomerID = '" + ddCustomer.SelectedValue + "' and DeliveryDate >= '" + dtFrom.ToString("yyyy-MM-dd") + "' AND DeliveryDate <= '" + dtTo.ToString("yyyy-MM-dd") + "') ORDER BY Id DESC ";
        }
        DataTable dt = RunQuery.SQLQuery.ReturnDataTable(str);


        foreach (DataRow dr in dt.Rows)
        {
            dr1 = dt1.NewRow();
            dr1["InvNo"] = Convert.ToString(dr["InvNo"]);
            string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/frmInvoice.aspx?inv=";
            dr1["Link"] = url + Convert.ToString(dr["InvNo"]);
            dr1["EntryDate"] = Convert.ToDateTime(dr["EntryDate"]).ToString("dd/MM/yyyy");
            dr1["ProductName"] = Convert.ToString(dr["ProductName"]);
            dr1["Quantity"] = Convert.ToString(dr["Quantity"]);// + " " + Convert.ToString(dr["UnitType"]);
            dr1["UnitCost"] = Convert.ToString(dr["UnitCost"]);
            dr1["UnitWeight"] = Convert.ToString(dr["UnitWeight"]);
            dr1["ItemTotal"] = Convert.ToString(dr["ItemTotal"]);
            dr1["TotalWeight"] = Convert.ToString(dr["TotalWeight"]);

            dr1["VAT"] = Convert.ToDecimal(Convert.ToString(dr["VAT"])) - Convert.ToDecimal(Convert.ToString(dr["ItemTotal"])) ;
            dr1["TotalAmt"] = Convert.ToString(dr["VAT"]);
            dt1.Rows.Add(dr1);

            total2 += Convert.ToDecimal(Convert.ToString(dr["ItemTotal"]));
            vat2 += Convert.ToDecimal(Convert.ToString(dr["VAT"])) - Convert.ToDecimal(Convert.ToString(dr["ItemTotal"]));
            qty2 += Convert.ToDecimal(Convert.ToString(dr["Quantity"]));
            TotalSales2 += Convert.ToDecimal(Convert.ToString(dr["VAT"]));
            TotalWeight2 += Convert.ToDecimal(Convert.ToString(dr["TotalWeight"]));

        }
        */

        string chk = "0";
        if (CheckBox1.Checked)
        {
            chk = "1";
        }

        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/Sale_Items.aspx?iCode=" + iName + "&iGrade=" + ddGrade.SelectedValue + "&chk=" + chk + "&cust=" + ddCustomer.SelectedValue + "&dt1=" + dtFrom + "&dt2=" + dtTo;
        if1.Attributes.Add("src", urlx);

        //GVrpt.DataSource = dt1;
        //GVrpt.DataBind();

        //ltrQty.Text = qty2.ToString();
        //ltrItemLoad.Text = total2.ToString();
        //ltrTotalVat.Text = vat2.ToString();
        //ltrGTAmt.Text = TotalSales2.ToString();
        //ltrTotalWeight.Text = TotalWeight2.ToString();

    }

    private decimal total = (decimal)0.0;
    private decimal qty = (decimal)0.0;
    private decimal TotalSales = (decimal)0.0;
    private decimal TotalWeight = (decimal)0.0;
    protected void GVrpt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            qty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Quantity"));
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ItemTotal"));
            TotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
            TotalWeight += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalWeight"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[2].Text = Convert.ToString(qty);
            e.Row.Cells[4].Text = Convert.ToString(total);
            e.Row.Cells[5].Text = Convert.ToString(TotalSales-total);
            e.Row.Cells[6].Text = Convert.ToString(TotalSales);  //String.Format("{0:c}", TotalSales);
            e.Row.Cells[8].Text = Convert.ToString(TotalWeight);  //String.Format("{0:c}", TotalSales);
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
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        if(CheckBox1.Checked)
        {
            ddItem.Enabled = false;
        }
        else
        {
            ddItem.Enabled = true;
        }

        LoadGridData();
    }

    protected void GVrpt_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVrpt.PageIndex = e.NewPageIndex;
        LoadGridData();
        GVrpt.PageIndex = e.NewPageIndex;
    }

    protected void ddGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddItem.DataBind();
    }
}
