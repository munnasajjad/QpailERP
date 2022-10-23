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
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using RunQuery;

public partial class app_Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {

        }

    }

    protected void btnSend_OnClick(object sender, EventArgs e)
    {
        try
        {
            //string body = "<b>Some body text</b><br/>2nd line!";   
            //lblMsg.Text = XERPSecurity.XERPSecure.EmailPostmarkapp("rony@extreme.com.bd", "Test Email Postmarkapp", body);
            //DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT   BrandID, OldBId, CustomerID, BrandName, Description, ProjectID, EntryBy, EntryDate 
            //FROM CustomerBrands WHERE (CustomerID = '5488')");
            //foreach (DataRow dr in dtx.Rows)
            //{
            //    SQLQuery.ExecNonQry(@"update    FinishedProducts set BrandID=(select BrandID FROM           CustomerBrands
            //        WHERE (CustomerID = '5488') and OldBId='" + dr["OldBId"] + @"')
            //        WHERE (CompanyID = '5488') and BrandID='" + dr["OldBId"] + @"'");
            //}
            //lblMsg.Text = "Finished";

            //int count = 0;
            //DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT id, InvNo FROM matureInvlist");
            //foreach (DataRow dr in dtx.Rows)
            //{
            //    SqlCommand cmd2 = new SqlCommand(@"UPDATE Sales SET IsActive=@IsActive WHERE (InvNo= '" + dr["InvNo"] + @"')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            //    cmd2.Parameters.AddWithValue("@IsActive", 0);

            //    cmd2.Connection.Open();
            //    int rowAffected = cmd2.ExecuteNonQuery();
            //    cmd2.Connection.Close();
            //    if (rowAffected > 0)
            //    {
            //        count++;
            //    }
            //}
            //lblMsg.Text = "Update Total: " + count;
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.ToString();
        }
    }
}