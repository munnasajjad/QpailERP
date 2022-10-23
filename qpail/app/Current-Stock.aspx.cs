using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class app_Current_Stock : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //txtDateF.Text = DateTime.Now.ToShortDateString();
            //txtDateT.Text = DateTime.Now.ToShortDateString();
            GenerateGrid();
            GenerateSchedule();
        }
    }

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        //GridView1.DataSource = null;
        //GridView1.DataSourceID = null;

        GenerateGrid();
        GenerateSchedule();
    }

    private void GenerateGrid()
    {
        string bus = DropDownList2.SelectedValue;

        SqlCommand cmd2 = new SqlCommand("Select  PartName, Warehouse, ItemSerialNo, Warrenty, Status  FROM Stock where  OutQuantity<1 AND Warehouse=@BusNo ORDER BY EntryID DESC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.Add("@BusNo", SqlDbType.VarChar).Value = bus;

        cmd2.Connection.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd2);
        cmd2.Connection.Close();
        DataSet ds = new DataSet();
        //DataTable dt = new DataTable();
        da.Fill(ds);
        GridView1.DataSource = ds;
        GridView1.DataBind();


        //summery
        //SqlCommand cmdJ4 = new SqlCommand("SELECT SUM(Quantity) FROM StockIn where Warehouse=@BusNo and StockInDate>=@dateFrom and StockInDate<=@dateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //cmdJ4.Parameters.Add("@BusNo", SqlDbType.VarChar).Value = bus;
        //cmdJ4.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = txtDateF.Text;
        //cmdJ4.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text).AddDays(+1);

        //cmdJ4.Connection.Open();
        //Label2.Text = Convert.ToString(cmdJ4.ExecuteScalar());
        //cmdJ4.Connection.Close();

        //SqlCommand cmdJ4x = new SqlCommand("SELECT SUM(Price) FROM StockIn where Warehouse='" + bus + "' and StockInDate>=@dateFrom and StockInDate<=@dateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //cmdJ4x.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = txtDateF.Text;
        //cmdJ4x.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text).AddDays(+1);

        //cmdJ4x.Connection.Open();
        //Label3.Text = Convert.ToString(cmdJ4x.ExecuteScalar());
        //cmdJ4x.Connection.Close();

    }

    private void GenerateSchedule()
    {/*
        string bus = DropDownList2.SelectedValue;

        SqlCommand cmd2 = new SqlCommand("SELECT ScheduleName,  Driver, Asst, Conductor, IsActive  FROM Schedules where CoachNo=@BusNo and StartTime>=@dateFrom and StartTime<=@dateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.Add("@BusNo", SqlDbType.VarChar).Value = bus;
        cmd2.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = txtDateF.Text;
        cmd2.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text).AddDays(+1);

        cmd2.Connection.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd2);
        cmd2.Connection.Close();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        da.Fill(ds);
        GridView2.DataSource = ds;
        GridView2.DataBind();
        **/
    }

    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerateGrid();
        GenerateSchedule();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //decimal rowTotal = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Sold Qty."));
            //grdTotal = grdTotal + rowTotal;
            //grdTotal += rowTotal;
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            //Label lbl = (Label)e.Row.FindControl("lblTotal");
            //e.Row.Cells[3].Text = rowTotal.ToString("c");
            //lbl.Text = grdTotal.ToString("c");
        }
    }
}
