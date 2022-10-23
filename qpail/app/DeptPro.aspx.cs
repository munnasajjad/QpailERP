using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_DeptPro : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtLastProcessDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtPresentProcessDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }
    
    protected void rbtMonthly_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtMonthly.Checked)
        {
            BindItemGrid("1");
        }
        else
        {
            BindItemGrid("2");
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


    private void BindItemGrid(string itemType)
    {
        if (btnSave.Text == "Update")
        {/*, SalePrice, Depreciation, FixedAssetsValue
FROM            vwProducts*/
            SqlCommand cmd = new SqlCommand(@"SELECT  ISNULL(TotalValue,0) as TotalValue, Rate, Name, ISNULL(Depreciation,0) as Depreciation, DepType,(ISNULL(Rate,0)- (ISNULL(Rate,0)*ISNULL(Depreciation,0)/100)) as NewValue
               FROM vw_FixedAssets WHERE  DepType='" + itemType + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            GridView2.EmptyDataText = "No data added ...";
            GridView2.DataSource = cmd.ExecuteReader();
            GridView2.DataBind();
            cmd.Connection.Close();
            
        }
        else
        {
            SqlCommand cmd = new SqlCommand(@"SELECT ISNULL(TotalValue,0) as TotalValue, Rate, Name,   ISNULL(Depreciation,0) as Depreciation, DepType,(ISNULL(Rate,0)- (ISNULL(Rate,0)*ISNULL(Depreciation,0)/100)) as NewValue
                   FROM  vw_FixedAssets WHERE  DepType='" + itemType + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            GridView2.EmptyDataText = "No data added ...";
            GridView2.DataSource = cmd.ExecuteReader();
            GridView2.DataBind();
            cmd.Connection.Close();
            
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {

    }
}