using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class app_Stock_out : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //ddSl.DataBind();
            //ddProduct.DataBind();
            //PopulateData();
            PopulateWarehouse();
            //PopulateDescription();
        }
    }

    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox)
        { (Parent as TextBox).Text = string.Empty; }
        else
        {
            foreach (Control c in Parent.Controls)
                ClearControls(c);
        }
    }

    // Getting Product Code
    public void PopulateWarehouse()
    {
        ddCustomer.Items.Clear();

        SqlCommand cmd = new SqlCommand("SELECT WID, StoreName FROM Warehouses  WHERE (ProjectID =1) ORDER BY StoreName", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader Reflist = cmd.ExecuteReader();

        ddCustomer.DataSource = Reflist;
        ddCustomer.DataValueField = "WID";
        ddCustomer.DataTextField = "StoreName";
        ddCustomer.DataBind();

        cmd.Connection.Close();
        cmd.Connection.Dispose();

        PopulatePrdName();
    }
        
    public void PopulatePrdName()
    {
        SqlCommand cmd = new SqlCommand("SELECT DISTINCT [ProductName] FROM [Stock] WHERE WarehouseID='" + ddCustomer.SelectedValue + "' ORDER BY [ProductName]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader Reflist = cmd.ExecuteReader();

        ddProduct.DataSource = Reflist;
        ddProduct.DataValueField = "ProductName";
        ddProduct.DataTextField = "ProductName";
        ddProduct.DataBind();

        cmd.Connection.Close();
        cmd.Connection.Dispose();

        PopulateSerial();
    }
    public void PopulateSerial()
    {
        //SELECT [ItemSerialNo] FROM [Stock] WHERE ([Status] &lt;&gt; @Status) ORDER BY [EntryID]
        //SqlCommand cmd = new SqlCommand("SELECT DISTINCT [ItemSerialNo] FROM [Stock] WHERE WarehouseID='" + ddCustomer.SelectedValue + "' AND PartName='" + ddProduct.SelectedValue + "' AND OutQuantity>0 ORDER BY [ItemSerialNo]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmd.Connection.Open();
        //SqlDataReader Reflist = cmd.ExecuteReader();

        //ddSl.DataSource = Reflist;
        //ddSl.DataValueField = "ItemSerialNo";
        //ddSl.DataTextField = "ItemSerialNo";
        //ddSl.DataBind();

        //cmd.Connection.Close();
        //cmd.Connection.Dispose();
        
        //PopulateData();
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtPrice.Text != "" && txtInv.Text != "")
        {
            try
            {
                saveData();

                PopulateWarehouse();
                PopulateData();
                //GridView1.DataBind();
            }
            catch (Exception ex)
            {
                //Exception ex2 = ex;
                //string errorMessage = string.Empty;
                //while (ex2 != null)
                //{
                //    errorMessage += ex2.ToString();
                //    ex2 = ex2.InnerException;
                //    lblError.CssClass = "isa_error";
                //    lblError.Text = ex2.ToString();
                //}
                lblError.CssClass = "isa_error";
                lblError.Text = ex.ToString();
            }
        }
        else
        {
            lblError.CssClass = "isa_error";
            lblError.Text = "Item was not found in the stock!";
            MessageBox("Item was not found in the stock!");

        }
    }

    private void saveData()
    {        
        SqlCommand cmd2 = new SqlCommand("Delete from Stock where ItemSerialNo='" + ddSl.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        //Create Sql Connection
        SqlConnection cnn = new SqlConnection();
        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        //Create Sql Command
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO stockOut (Name, SalesPrice, Quantity, total, Warehouse, SerialNo)" +
                                "VALUES (@Name, @Cost, @Quantity, @total, @Warehouse, @SerialNo)";

        cmd.CommandType = CommandType.Text;
        cmd.Connection = cnn;

        //Parameter array Declaration
        SqlParameter[] param = new SqlParameter[6];

        param[0] = new SqlParameter("@Name", SqlDbType.VarChar, 100);
        param[1] = new SqlParameter("@Cost", SqlDbType.Float);
        param[2] = new SqlParameter("@Quantity", SqlDbType.Int);
        param[3] = new SqlParameter("@total", SqlDbType.Float);
        param[4] = new SqlParameter("@Warehouse", SqlDbType.VarChar, 200);
        param[5] = new SqlParameter("@SerialNo", SqlDbType.VarChar, 200);

        param[0].Value = ddProduct.SelectedValue;
        param[1].Value = txtPrice.Text;
        param[2].Value = "1";
        param[3].Value = txtPrice.Text;
        param[4].Value = DropDownList3.SelectedValue;
        param[5].Value = ddSl.SelectedValue;

        for (int i = 0; i < param.Length; i++)
        {
            cmd.Parameters.Add(param[i]);
        }

        cnn.Open();
        int Success = cmd.ExecuteNonQuery();
        cnn.Close();

        if (Success > 0)
        {
            //Generate max stock entry ID
            SqlCommand cmd4x = new SqlCommand("Select CONVERT(varchar, (ISNULL (max(EntryID),0)+1001 )) from stockOut", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd4x.Connection.Open();
            string seid = "SOID-"  + Convert.ToString(cmd4x.ExecuteScalar());
            cmd4x.Connection.Close();

            //Save Voucher     (old bad item)
            string invNo = seid;
            string vDetail = "Replacement Item Bad: " + ddProduct.SelectedValue + "; Sl#" + ddSl.SelectedValue + "; Stock Entry ID: " + " : " + seid;
            string acHeadDr = "Parts & Tools (Bad)";
            string acHeadCr = "Adjuted Stock";
            string dr = txtPrice.Text;
            string cr = dr;
            saveVoucher(vDetail, invNo, acHeadDr, acHeadCr, dr, cr);
                        
            lblError.CssClass = "isa_success";
            lblError.Text = "The Part Successfully Removed from stock";
            ClearControls(Page);
            //Photo.ImageUrl = "~/ShowPhoto.ashx?id=" + (param[2].Value);
            MessageBox("The Part Successfully Removed from stock");
        }

    }




    private void saveVoucher(string vDetail, string invNo, string acHeadDr, string acHeadCr, string dr, string cr)
    {
        if (Convert.ToDecimal(dr) > 0)
        {
            string lName = Page.User.Identity.Name.ToString();
            acHeadDr = HttpUtility.HtmlDecode(acHeadDr);
            acHeadCr = HttpUtility.HtmlDecode(acHeadCr);
            vDetail = HttpUtility.HtmlDecode(vDetail);
            //string amount = txtCollected.Text;

            SqlCommand cmd2x = new SqlCommand("INSERT INTO VoucherMaster (VoucherNo, VoucherDate, VoucherDescription, VoucherEntryBy, VoucherAmount)" +
                                                "VALUES (@VoucherNo, @VoucherDate, @VoucherDescription, @VoucherEntryBy, @VoucherAmount)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2x.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = invNo; //txtInv.Text;
            cmd2x.Parameters.Add("@VoucherDate", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString();
            cmd2x.Parameters.Add("@VoucherDescription", SqlDbType.VarChar).Value = vDetail; //ddParticular.SelectedValue;
            cmd2x.Parameters.Add("@VoucherEntryBy", SqlDbType.VarChar).Value = lName;
            cmd2x.Parameters.Add("@VoucherAmount", SqlDbType.Decimal).Value = dr;
            cmd2x.Connection.Open();
            int success = cmd2x.ExecuteNonQuery();
            cmd2x.Connection.Close();

            //Get Head ID
            SqlCommand cmd = new SqlCommand("SELECT AccountsHeadID FROM HeadSetup WHERE AccountsHeadName ='" + acHeadDr + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            string acHeadID = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            //insert Dr       
            SqlCommand cmd2y = new SqlCommand("INSERT INTO VoucherDetails (VoucherNo, VoucherRowDescription, AccountsHeadID, AccountsHeadName, VoucherDR, VoucherCR, EntryDate)" +
                                                "VALUES (@VoucherNo, @VoucherRowDescription, @AccountsHeadID, @AccountsHeadName, @VoucherDR, @VoucherCR, @EntryDate)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2y.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = invNo;
            cmd2y.Parameters.Add("@VoucherRowDescription", SqlDbType.VarChar).Value = HttpUtility.HtmlDecode(vDetail);
            cmd2y.Parameters.Add("@AccountsHeadID", SqlDbType.VarChar).Value = acHeadID;
            cmd2y.Parameters.Add("@AccountsHeadName", SqlDbType.VarChar).Value = acHeadDr;
            cmd2y.Parameters.Add("@VoucherDR", SqlDbType.Decimal).Value = dr;
            cmd2y.Parameters.Add("@VoucherCR", SqlDbType.Decimal).Value = 0;
            cmd2y.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString();

            cmd2y.Connection.Open();
            cmd2y.ExecuteNonQuery();
            cmd2y.Connection.Close();

            //insert cr       
            SqlCommand cmd2 = new SqlCommand("SELECT AccountsHeadID FROM HeadSetup WHERE AccountsHeadName ='" + acHeadCr + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2.Connection.Open();
            acHeadID = Convert.ToString(cmd2.ExecuteScalar());
            cmd2.Connection.Close();
            cmd2.Connection.Dispose();
            SqlCommand cmd2y1 = new SqlCommand("INSERT INTO VoucherDetails (VoucherNo, VoucherRowDescription, AccountsHeadID, AccountsHeadName, VoucherDR, VoucherCR, EntryDate)" +
                                                "VALUES (@VoucherNo, @VoucherRowDescription, @AccountsHeadID, @AccountsHeadName, @VoucherDR, @VoucherCR, @EntryDate)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2y1.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = invNo;
            cmd2y1.Parameters.Add("@VoucherRowDescription", SqlDbType.VarChar).Value = HttpUtility.HtmlDecode(vDetail);
            cmd2y1.Parameters.Add("@AccountsHeadID", SqlDbType.VarChar).Value = acHeadID;
            cmd2y1.Parameters.Add("@AccountsHeadName", SqlDbType.VarChar).Value = acHeadCr;
            cmd2y1.Parameters.Add("@VoucherDR", SqlDbType.Decimal).Value = 0;
            cmd2y1.Parameters.Add("@VoucherCR", SqlDbType.Decimal).Value = cr;
            cmd2y1.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString(); ;

            cmd2y1.Connection.Open();
            cmd2y1.ExecuteNonQuery();
            cmd2y1.Connection.Close();
        }
    }



    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        //PopulateDescription();
    }
    private void PopulateData()
    {
        SqlCommand cmd2y = new SqlCommand("Select a.InvoiceID, a.PurchaseBillNo, a.PartName, a.Warehouse, a.PartType, a.Warrenty, (Select PuchaseDate from Orders where InvNo=a.InvoiceID) As EntryDate from Stock a where a.ItemSerialNo=@ItemSerialNo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2y.Parameters.Add("@ItemSerialNo", SqlDbType.VarChar).Value = ddSl.SelectedValue;

        cmd2y.Connection.Open();
        SqlDataReader dr1y = cmd2y.ExecuteReader();
        if (dr1y.Read())
        {
            txtInv.Text = dr1y[0].ToString();
            txtBill.Text = dr1y[1].ToString();
            //ddProduct.SelectedValue = dr1y[2].ToString();
            //ddCustomer.SelectedValue = dr1y[3].ToString();
            txtPrice.Text = dr1y[4].ToString();

            string warr = dr1y[5].ToString();
            string pDate = Convert.ToDateTime(dr1y[6].ToString()).ToShortDateString();
            lblSummery.Text = "Purchased on: " + pDate + ", Warrenty: " + warr;

            SqlCommand cmdxx = new SqlCommand("SELECT PurchaseFrom FROM Orders where InvNo='" + txtInv.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmdxx.Connection.Open();
            string provider = Convert.ToString(cmdxx.ExecuteScalar());
            cmdxx.Connection.Close();
            cmdxx.Connection.Dispose();

            ddVendor.DataBind();
            ddVendor.SelectedValue = provider;

            //GridView1.DataBind();

            lblError.Text = "Purchased on: " + pDate + ", Warrenty: " + warr;
            lblError.CssClass = "isa_success";
        }
        else
        {
            lblSummery.Text = "";
            ClearControls(Page);
            lblError.CssClass = "isa_error";
            lblError.Text = "Item was not found in the stock!";
            MessageBox("Item was not found in the stock!");
        }
        cmd2y.Connection.Close();
        cmd2y.Connection.Dispose();
    }
    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulatePrdName();
    }
    protected void ddProduct_SelectedIndexChanged1(object sender, EventArgs e)
    {
        PopulateSerial();
    }
    protected void ddSl_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateData();
    }
}
