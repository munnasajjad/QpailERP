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

public partial class app_WebMethods : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    [WebMethod]
    public static string Insert_Data(string iCode, string iName, string iPrice, string lName, string Customer)
    {
        SqlCommand cmd7 = new SqlCommand("SELECT  name, price FROM  Items WHERE (ItemCode = @ItemCode)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        cmd7.Parameters.Add("@ItemCode", SqlDbType.VarChar).Value = iCode;
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            iName = dr[0].ToString();
            iPrice = dr[1].ToString();
        }
        cmd7.Connection.Close();
        cmd7.Connection.Dispose();

        decimal qty = 1;
        int sid = 0; //SaleID(lName);
        Int32 ItemSl = 0;// EntrySl(sid, iCode);

        //Check if item entry already exist
        SqlCommand cmd3z = new SqlCommand("SELECT COUNT(ItemCode) FROM SalesGridTemp where ItemCode='" + iCode + "' AND (sid=" + sid + ")", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd3z.Connection.Open();
        Int32 count = Convert.ToInt32(cmd3z.ExecuteScalar());
        cmd3z.Connection.Close();
        cmd3z.Connection.Dispose();

        try
        {
            // Insert Mode
            if (count == 0)
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into SalesGridTemp (SaleType, sid, Customer, ItemSl, ItemCode, ItemName, Qty, Price, Total) values(@SaleType, @sid, @Customer, @ItemSl, @ItemCode, @ItemName, @Qty, @Price, @Total)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@SaleType", "Table");
                cmd.Parameters.AddWithValue("@sid", sid);
                cmd.Parameters.AddWithValue("@Customer", Customer);

                cmd.Parameters.AddWithValue("@ItemSl", ItemSl);
                cmd.Parameters.AddWithValue("@ItemCode", iCode);
                cmd.Parameters.AddWithValue("@ItemName", iName);

                cmd.Parameters.AddWithValue("@Qty", qty);
                cmd.Parameters.AddWithValue("@Price", iPrice);
                cmd.Parameters.AddWithValue("@Total", qty * Convert.ToDecimal(iPrice));

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                //Update Mode
                SqlCommand cmd2 = new SqlCommand("SELECT Qty FROM SalesGridTemp where ItemCode='" + iCode + "' AND (sid=" + sid + ")", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd2.Connection.Open();
                qty = qty + Convert.ToDecimal(cmd2.ExecuteScalar());
                cmd2.Connection.Close();
                cmd2.Connection.Dispose();

                SqlCommand cmd2e = new SqlCommand("UPDATE SalesGridTemp SET Qty=" + qty + ", Price=" + iPrice + ", Total=@Total where ItemCode='" + iCode + "' AND (sid=" + sid + ")", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd2e.Connection.Open();
                cmd2e.Parameters.AddWithValue("@Total", qty * Convert.ToDecimal(iPrice));
                cmd2e.ExecuteNonQuery();
                cmd2e.Connection.Close();
                cmd2e.Connection.Dispose();
            }

            return "Success";
        }
        catch (Exception ex)
        {
            return "failure";
        }
    }

    [WebMethod]
    public static string UpdateGrid(string iCode, string iQty, string iPrice, string lName, string Customer)
    {
        decimal qty = Convert.ToDecimal(iQty);
        int sid = 0;// SaleID(lName);
        try
        {
            SqlCommand cmd2e = new SqlCommand("UPDATE SalesGridTemp SET Qty=" + qty + ", Price=" + iPrice + ", Total=@Total where ItemCode='" + iCode + "' AND (sid=" + sid + ")", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2e.Connection.Open();
            cmd2e.Parameters.AddWithValue("@Total", qty * Convert.ToDecimal(iPrice));
            cmd2e.ExecuteNonQuery();
            cmd2e.Connection.Close();
            cmd2e.Connection.Dispose();

            return "Success";
        }
        catch (Exception ex)
        {
            return "failure";
        }
    }

    [WebMethod]
    public static string UpdateOrderGrid(string iCode, string iQty, string iPrice)
    {
        decimal qty = Convert.ToDecimal(iQty);
        int sid = 0;// SaleID(lName);
        try
        {
            SqlCommand cmd2e = new SqlCommand("UPDATE OrderDetails SET Quantity=" + qty + ", UnitCost=" + iPrice + ", ItemTotal=@Total where Id='" + iCode + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2e.Connection.Open();
            cmd2e.Parameters.AddWithValue("@Total", qty * Convert.ToDecimal(iPrice));
            cmd2e.ExecuteNonQuery();
            cmd2e.Connection.Close();
            cmd2e.Connection.Dispose();

            return "Success";
        }
        catch (Exception ex)
        {
            return "failure";
        }
    }

}