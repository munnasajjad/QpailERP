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

public partial class app_Customer_Credit_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            search();
        }
    }


    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        search();
    }

    private void search()
    {
        try
        {
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("link", typeof(string)));
            dt1.Columns.Add(new DataColumn("Company", typeof(string)));
            dt1.Columns.Add(new DataColumn("imQty", typeof(string)));
            dt1.Columns.Add(new DataColumn("mQty", typeof(string)));
            dt1.Columns.Add(new DataColumn("mDays", typeof(string)));
            dt1.Columns.Add(new DataColumn("avgOverDays", typeof(string)));
            dt1.Columns.Add(new DataColumn("immatured", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("matured", typeof(decimal)));
            dt1.Columns.Add(new DataColumn("cLimit", typeof(string)));
            dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));

            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT PartyID, Company, Address, Phone, Email, Fax, IM, Website, ContactPerson, MobileNo, MatuirityDays, CreditLimit, OpBalance FROM Party WHERE (Type = 'customer') order by Company");

            foreach (DataRow drx in dtx.Rows)// Let x=1
            {
                string pid = drx["PartyID"].ToString();
                string name = drx["Company"].ToString();

                double mDays =Convert.ToDouble(drx["MatuirityDays"].ToString());
                string cLimit = drx["CreditLimit"].ToString();
                decimal opBalance = Convert.ToDecimal(drx["OpBalance"].ToString());

                if (mDays == 0)
                {
                    //mDays = -1;
                }
                string lastMaturityDate = DateTime.Today.AddDays(mDays*(-1)).ToString("yyyy-MM-dd");

                int imMatQty = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(COUNT(InvNo),0) from Sales where CustomerID='" + pid + "'  AND InvDate >'" + lastMaturityDate + "' "));
                int matQty = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(COUNT(InvNo),0) from Sales where  CustomerID='" + pid + "'  AND  IsActive=1 AND InvDate<='" + lastMaturityDate + "' "));

                string avgOverDays  = SQLQuery.CalculateOverDueDays(pid);

                string immatured = SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" + pid + "'  AND IsActive=1 AND InvDate>'" + lastMaturityDate + "'");
                string matured =Convert.ToString(opBalance + Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(DueAmount),0) from Sales where CustomerID='" + pid + "' AND IsActive=1 AND InvDate<='" + lastMaturityDate + "'")));
                string link = "Party-Detail.aspx?pid=" + pid;
                
                dr1 = dt1.NewRow();
                dr1["link"] = link;
                dr1["Company"] = name;
                dr1["imQty"] = imMatQty;
                dr1["mQty"] = matQty;
                dr1["mDays"] = mDays;
                dr1["avgOverDays"] = avgOverDays;
                dr1["immatured"] = immatured;
                dr1["matured"] = matured;
                dr1["cLimit"] = cLimit;
                dr1["Balance"] = Convert.ToString(Convert.ToDecimal(immatured) + Convert.ToDecimal(matured));
                dt1.Rows.Add(dr1);
                
            }

            ltrtotal.Text = dt1.Rows.Count.ToString();
            GridView1.DataSource = dt1;
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }
    
    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        search();
        GridView1.PageIndex = e.NewPageIndex;
        //GridView1.DataBind();
    }

    private decimal imb = (decimal)0.0;
    private decimal mb = (decimal)0.0;
    private decimal climit = (decimal)0.0;
    private decimal bal = (decimal)0.0;
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            climit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "cLimit"));
            imb += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "immatured"));
            mb += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "matured"));
            bal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Balance"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[5].Text = "Total";
            e.Row.Cells[6].Text = Convert.ToString(climit);
            e.Row.Cells[7].Text = Convert.ToString(imb);
            e.Row.Cells[8].Text = Convert.ToString(mb);
            e.Row.Cells[9].Text = Convert.ToString(bal);
        }
    }

}