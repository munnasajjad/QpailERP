using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

public partial class app_Docs_Upload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void AsyncFileUpload1_UploadComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        //string filePath = Request.PhysicalApplicationPath + "app\\Docs\\PO\\" + AsyncFileUpload1.PostedFile.FileName;
        //AsyncFileUpload1.SaveAs(filePath);

        SqlCommand cmd1 = new SqlCommand("SELECT isnull(max(iid) + 1,1) FROM PO_Images", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd1.Connection.Open();
        string fileName = cmd1.ExecuteScalar().ToString();
        cmd1.Connection.Close();

        string tExt = Path.GetFileName(AsyncFileUpload1.PostedFile.ContentType);
        fileName = fileName + "." + tExt;

        //Delete the existing file first
        RunQuery.SQLQuery.ExecNonQry("Delete PO_Images where PONo=''");

        string strFolder = Server.MapPath(".\\PO\\");
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
            img.Save(Server.MapPath("./PO/" + fileName), jgpEncoder, myEncoderParameters);
        }
        else
        {
            img.Save(Server.MapPath("./PO/" + fileName));
        }

        SqlCommand cmd = new SqlCommand("INSERT INTO PO_Images (PONo, POimgLink, imgSl, EntryBy)" +
                                    "VALUES (@PONo, @POimgLink, @imgSl, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@PONo", "");
        cmd.Parameters.AddWithValue("@POimgLink", "Docs/PO/" + fileName);
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