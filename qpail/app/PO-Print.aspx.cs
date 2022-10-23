using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_PO_Print : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string poNo = Request.QueryString["poid"];
            if (poNo != null)
            {
                poNo = SQLQuery.ReturnString("Select pono from orders where OrderSl='" + poNo + "'");
                ltrOrderNo.Text = poNo;
                string detail = SQLQuery.ReturnString("Select OrderID from orders where pono='" + poNo + "'");
                if (poNo != detail)
                {
                    detail = SQLQuery.ReturnString("Select OrderType+'# '+Convert(varchar,OrderID)+'' from orders where pono='" + poNo + "'");
                    ltrDetail.Text ="&nbsp; &nbsp; &nbsp; &nbsp;"+ detail;
                }

                getMemberDetails(poNo);
                GridView1.DataBind();
                GridView3.DataBind();
            }
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "Print", "javascript:window.print();", true);
        }
    }

    private void getMemberDetails(string poNo)
    {
        //get board ID        
        SqlCommand cmd = new SqlCommand("SELECT CustomerName, OrderDate, DeliveryDate, TotalAmount, Vat, PayableAmount FROM Orders where pono='" + poNo + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.CommandType = CommandType.Text;
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            lblCustomer.Text = SQLQuery.ReturnString("Select Company from Party where PartyID='" + dr[0].ToString() + "'");
            lblPoDate.Text = Convert.ToDateTime(dr[1].ToString()).ToString("dd/MM/yyyy");
            lblDelDate.Text = Convert.ToDateTime(dr[2].ToString()).ToString("dd/MM/yyyy");

            lblPoAmount.Text = dr[3].ToString();
            vatPercent.Text = dr[4].ToString();
            lblNetAmount.Text = dr[5].ToString();
            lblVatAmount.Text = Convert.ToString(Convert.ToDecimal(lblNetAmount.Text) - Convert.ToDecimal(lblPoAmount.Text));

            string status = SQLQuery.ReturnString("Select DeliveryStatus  FROM Orders where pono='" + poNo + "'");
            if (status=="P")
            {
                ltrDeliveryStatus.Text = "Status: Pending";
            }
            if (status == "A")
            {
                ltrDeliveryStatus.Text = "Status: Partially Delivered";
            }
            if (status == "D")
            {
                ltrDeliveryStatus.Text = "Status: Delivered";
            }
        }
        cmd.Connection.Close();


    }

    private void GenerateMPOMsg()
    {
        SqlCommand cmd = new SqlCommand("Select Headline, FullNews from MessageCentre where MsgID=2", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        //cmd.Parameters.Add("@DID", SqlDbType.VarChar).Value = ddUserID.SelectedValue;

        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            //lblHeadline.Text = dr[0].ToString();
            //lblMsgBody.Text = dr[1].ToString();
        }
        cmd.Connection.Close();
        cmd.Connection.Dispose();
    }

}