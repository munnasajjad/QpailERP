using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

public partial class app_LC_Return : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtReturnDate.Text = DateTime.Now.ToShortDateString();

            ddOrders.DataBind();
            string orderNo = base.Request.QueryString["ID"];
            ddOrders.SelectedValue = orderNo;

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();
        }
    }

    //Messagebox For Alerts    
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
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
                ScriptManager.RegisterStartupScript(page, typeof(Page), "Redirect", script, true);
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddProduct.SelectedValue != "" && txtQty.Text != "")
            {
                InsertData();

                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Return info successfully saved.";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Error: Item Qty cannot be empty!";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error: " + ex.ToString();
        }
        finally
        {
            ddOrders.DataBind();
        }
    }

    private void InsertData()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd2 = new SqlCommand("INSERT INTO LCReturn (VendorName, VendorID, InvoiceNo, ReturnDate, WastageStock, WastageStockID, ReplacementStock, ReplacementStockID, Remark, SalesDetailID, ItemName, Qty, Price, Total, EntryBy, ByMode, TransportDetails) VALUES (@CustomerName, @CustomerID, @InvoiceNo, @ReturnDate, @WastageStock, @WastageStockID, @ReplacementStock, @ReplacementStockID, @Remark, @SalesDetailID, @ItemName, @Qty, @Price, @Total, @EntryBy, '" + txtMode.Text + "', '" + txtTransport.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@CustomerName", ddVendor.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@CustomerID", ddVendor.SelectedValue);
        cmd2.Parameters.AddWithValue("@InvoiceNo", ddOrders.SelectedValue);
        cmd2.Parameters.AddWithValue("@ReturnDate", txtReturnDate.Text);
        cmd2.Parameters.AddWithValue("@WastageStock","");// ddWasteStock.SelectedItem.Text);

        cmd2.Parameters.AddWithValue("@WastageStockID", "");// ddWasteStock.SelectedValue);
        cmd2.Parameters.AddWithValue("@ReplacementStock", "");// ddReplaceStock.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@ReplacementStockID", "");// ddReplaceStock.SelectedValue);
        cmd2.Parameters.AddWithValue("@Remark", txtRemarks.Text);
        cmd2.Parameters.AddWithValue("@SalesDetailID", ddProduct.SelectedValue);

        cmd2.Parameters.AddWithValue("@ItemName", ddProduct.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@Qty", Convert.ToDecimal(txtQty.Text));
        cmd2.Parameters.AddWithValue("@Price", 0);
        cmd2.Parameters.AddWithValue("@Total", 0);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        string retID = RunQuery.SQLQuery.ReturnString("Select ISNULL(MAX(eid),0) FROM LCReturn");
        RunQuery.SQLQuery.ExecNonQry("Update LCReturn_Images Set PONo='" + retID + "' Where EntryBy='" + lName + "' AND PONo=" + ddOrders.SelectedValue);
        RunQuery.SQLQuery.ExecNonQry("Update LcItems Set LcItems='" + retID + "' Where EntryID=" + ddProduct.SelectedValue);
        
        /* //Stock-out//
        string retNo = RunQuery.SQLQuery.ReturnString("Select ISNULL(MAX(eid),0) from LCReturn");
        string sizeID = RunQuery.SQLQuery.ReturnString("Select SizeId from LCReturn Where id=" + ddProduct.SelectedValue);
        string BrandID = RunQuery.SQLQuery.ReturnString("Select BrandID from LCReturn Where id=" + ddProduct.SelectedValue);
        string ProductID = RunQuery.SQLQuery.ReturnString("Select SizeId from LCReturn Where id=" + ddProduct.SelectedValue);
        Accounting.VoucherEntry.StockEntry(ddOrders.SelectedValue, "Stock-out: Sale Return# " + retNo, sizeID, BrandID, ProductID, ddProduct.SelectedItem.Text, "ddReplaceStock.SelectedValue", "2", "0", txtQty.Text, "", "Sales Return", "Finished", "", lName);
        //Sock-in
        Accounting.VoucherEntry.StockEntry(ddOrders.SelectedValue, "Wastage Stock-in: Sale Return# " + retNo, sizeID, BrandID, ProductID, ddProduct.SelectedItem.Text, "ddWasteStock.SelectedValue", "2", txtQty.Text, "0", "", "Sales Return", "Finished", "", lName);
        */

    }


    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddOrders.DataBind();
        LoadReturnHistory();
    }
    protected void ddOrders_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadReturnHistory();
    }

    private void LoadReturnHistory()
    {
        txtReturnDate.Text = Convert.ToDateTime(RunQuery.SQLQuery.ReturnString("Select DeliveryDate from Sales where InvNo='" + ddOrders.SelectedValue + "'")).ToShortDateString();
        ddProduct.DataBind();
        ItemGrid.DataBind();
    }

    protected void AsyncFileUpload1_UploadComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        SqlCommand cmd1 = new SqlCommand("SELECT isnull(max(iid) + 1,1) FROM LCReturn_Images", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd1.Connection.Open();
        string fileName = cmd1.ExecuteScalar().ToString();
        cmd1.Connection.Close();

        string tExt = Path.GetFileName(AsyncFileUpload1.PostedFile.ContentType);
        fileName = fileName + "." + tExt;

        string strFolder = Server.MapPath(".\\LC-Return\\");
        string strFullPath = strFolder + fileName;

        if (File.Exists(strFullPath))
        {
            File.Delete(strFullPath);
        }
        
        var file = AsyncFileUpload1.PostedFile.InputStream;
        System.Drawing.Image img = System.Drawing.Image.FromStream(file, false, false);

        Size size = new Size(850, 1180);
        img = resizeImage(img, size);

        if (tExt == "jpeg")
        {
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            img.Save(Server.MapPath("./Docs/LC-Return/" + fileName), jgpEncoder, myEncoderParameters);
        }
        else
        {
            img.Save(Server.MapPath("./Docs/LC-Return/" + fileName));
        }

        SqlCommand cmd = new SqlCommand("INSERT INTO LCReturn_Images (PONo, POimgLink, imgSl, EntryBy)" +
                                    "VALUES (@PONo, @POimgLink, @imgSl, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@PONo", "");
        cmd.Parameters.AddWithValue("@POimgLink", "Docs/LC-Return/" + fileName);
        cmd.Parameters.AddWithValue("@imgSl", "1");
        cmd.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();

        AsyncFileUpload1.Visible = false;
    }

    //Image Resizing
    public static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, System.Drawing.Size size)
    {
        return (System.Drawing.Image)(new System.Drawing.Bitmap(imgToResize, size));
    }

    private ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }
    //End of Resize Image        
}