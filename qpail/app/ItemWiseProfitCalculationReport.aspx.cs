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

public partial class app_ItemWiseProfitCalculationReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSearch.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSearch, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDate.Text = "01" + DateTime.Now.ToString("dd/MM/yyyy").Substring(2);
            //txtDate.Text = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            //ddGroup.DataBind();
            //ddGroup.SelectedValue = "1";
            ddSubGroup.DataBind();
            ddCustomer.DataBind();
            GetGrade();
            GetCategory();
            GetProductList();
            GetpackSize();
        }
        //txtInv.Text = InvIDNo();
    }

    private void GetGrade()
    {
        string gQuery = "SELECT GradeID, GradeName FROM ItemGrade where CategoryID='" + ddSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY GradeName";
        SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
    }
    private void GetCategory()
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] WHERE GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
    }

    private void GetpackSize()
    {
        string gQuery = "SELECT BrandID, BrandName FROM [Brands] where BrandID='" + ddSize.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [BrandName]";
        //SQLQuery.PopulateDropDown(gQuery,ddSize, "BrandID", "BrandName");
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //BindItemGrid();
        string dateFrom = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

        string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") +
            "XerpReports/FormItemWiseProfitCalculationReport.aspx?item=" + ddItemName.SelectedValue + "&customer=" + ddCustomer.SelectedValue + "&brand=" + ddBrand.SelectedValue + "&packSize=" + ddSize.SelectedValue + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo;

        if1.Attributes.Add("src", url);
    }

    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(QtyBalance),0) from StockinDetailsRaw where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "pcs, " +
        //    RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(UnitWeight),0) from StockinDetailsRaw where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "kg";

    }


   protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetGrade();
        GetCategory();
        GetpackSize();
        GetProductList();

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
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategory.SelectedValue +
                        "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        SQLQuery.PopulateDropDown(gQuery, ddItemName, "ProductID", "ItemName");
        ddItemName.Items.Add("--- all ---");

        //ltrUnitType.Text = SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddItemName.SelectedValue + "'");

        //if (IsPostBack)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
        //}

        //LoadItemsPanel();
    }

}