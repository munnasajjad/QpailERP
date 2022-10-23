using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
//For converting HTML TO PDF- START
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html.simpleparser;
using System.IO;
using System.util;
using System.Text.RegularExpressions;
using RunQuery;

//For converting HTML TO PDF- END

public partial class Application_Reports_Voucher : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string v = Request.QueryString["inv"];

        if (v != null)
        {
            v = v.Trim(); v = v.ToUpper();
            lblInv.Text = v;
            lblInv.NavigateUrl = "../VoucherEntry.aspx?id=" + v;

            SqlCommand cmd2 = new SqlCommand("SELECT VoucherDate, VoucherDescription, VoucherEntryBy, VoucherPostby, Voucherpostdate, VoucherAmount FROM VoucherMaster where VoucherNo='" + v + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2.Connection.Open();
            SqlDataReader dr = cmd2.ExecuteReader();

            if (dr.Read())
            {
                lblDate.Text = Convert.ToDateTime(dr[0].ToString()).ToShortDateString();
                lblDesc.Text = dr[1].ToString();

                lblEntryBy.Text = dr[2].ToString();
                lblAppBy.Text = dr[3].ToString();

                string postDate = dr[4].ToString();
                if (postDate != "")
                {
                    lblDDate.Text = Convert.ToDateTime(postDate).ToShortDateString();
                }
                else
                {
                    lblDDate.Text = "(Pending)";
                }

                lblTtlAmount.Text = SQLQuery.FormatBDNumber(dr[5].ToString());

                cmd2.Connection.Close();
                cmd2.Connection.Dispose();

                //Get current Balance
                /*
                SqlCommand cmd = new SqlCommand("SELECT VarSupplierAddress FROM Party WHERE VarSupplierName ='" + pName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Connection.Open();
                lblAddress.Text = Convert.ToString(cmd.ExecuteScalar());
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                 * */

                lblPDate.Text = DateTime.Now.ToShortDateString();
                //getCompanyInfo();

            }
        }


    }


    private void getCompanyInfo()
    {
        SqlConnection cnn = new SqlConnection();
        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        cmd.CommandText = "Select CompanyName, CompanyAddress, Logo From Company";
        cmd.CommandType = CommandType.Text;

        cnn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            lblName.Text = dr[0].ToString();
            lblArress.Text = dr[1].ToString();
            //Image1.ImageUrl = "../../" + dr[2].ToString();
        }
        cnn.Close();
    }



    protected override void Render(HtmlTextWriter writer)
    {
        MemoryStream mem = new MemoryStream();
        StreamWriter twr = new StreamWriter(mem);
        HtmlTextWriter myWriter = new HtmlTextWriter(twr);
        base.Render(myWriter);
        myWriter.Flush();
        myWriter.Dispose();
        StreamReader strmRdr = new StreamReader(mem);
        strmRdr.BaseStream.Position = 0;
        string pageContent = strmRdr.ReadToEnd();
        strmRdr.Dispose();
        mem.Dispose();
        writer.Write(pageContent);
        
        CreatePDFDocument(pageContent);
        //ConvertHTMLToPDF2(pageContent);
        //Convert2PDF(pageContent.Trim());//Download

        //For images
        //iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance((Server.MapPath("/") + "123451.jpg"));
        //document.Add(gif);
    }

    public void CreatePDFDocument(string strHtml)
    {

        string strFileName = HttpContext.Current.Server.MapPath("Voucher.pdf");
        // step 1: creation of a document-object
        Document document = new Document();
        // step 2:
        // we create a writer that listens to the document
        PdfWriter.GetInstance(document, new FileStream(strFileName, FileMode.Create));
        StringReader se = new StringReader(strHtml);
        HTMLWorker obj = new HTMLWorker(document);
        document.Open();
        obj.Parse(se);
        document.Close();
        ShowPdf(strFileName);

    }


    private void Convert2PDF(string htmlContent)
    {
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);
        //GridView1.AllowPaging = false;
        //GridView1.DataBind();
        //GridView1.RenderControl(hw);
        //StringReader sr = new StringReader(sw.ToString());

        StringReader sr = new StringReader(htmlContent);
        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();
        Response.Write(pdfDoc);
        Response.End();
    }

    public void ShowPdf(string strFileName)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);
        Response.ContentType = "application/pdf";
        Response.WriteFile(strFileName);
        Response.Flush();
        Response.Clear();
    }


    //New code
    protected void ConvertHTMLToPDF(string HTMLCode)
    {
        HttpContext context = HttpContext.Current;

        //Render PlaceHolder to temporary stream
        System.IO.StringWriter stringWrite = new StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

        StringReader reader = new StringReader(HTMLCode);

        //Create PDF document
        Document doc = new Document(PageSize.A4);
        HTMLWorker parser = new HTMLWorker(doc);
        PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~") + "/HTMLToPDF.pdf",

        FileMode.Create));
        doc.Open();

        /********************************************************************************/
        var interfaceProps = new Dictionary<string, Object>();
        var ih = new ImageHander() { BaseUri = Request.Url.ToString() };

        interfaceProps.Add(HTMLWorker.IMG_PROVIDER, ih);

        foreach (IElement element in HTMLWorker.ParseToList(
        new StringReader(HTMLCode), null))
        {
            doc.Add(element);
        }
        doc.Close();
        Response.End();

        /********************************************************************************/

    }

    //handle Image relative and absolute URL's
    public class ImageHander : IImageProvider
    {
        public string BaseUri;
        public iTextSharp.text.Image GetImage(string src,
        IDictionary<string, string> h,
        ChainedProperties cprops,
        IDocListener doc)
        {
            string imgPath = string.Empty;

            if (src.ToLower().Contains("http://") == false)
            {
                imgPath = HttpContext.Current.Request.Url.Scheme + "://" +

                HttpContext.Current.Request.Url.Authority + src;
            }
            else
            {
                imgPath = src;
            }

            return iTextSharp.text.Image.GetInstance(imgPath);
        }
    }

    protected void ConvertHTMLToPDF2(string HTMLCode)
    {
        HttpContext context = HttpContext.Current;

        //Render PlaceHolder to temporary stream
        System.IO.StringWriter stringWrite = new StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

        /********************************************************************************/
        //Try adding source strings for each image in content
        string tempPostContent = getImage(HTMLCode);
        /*********************************************************************************/

        StringReader reader = new StringReader(tempPostContent);

        //Create PDF document
        Document doc = new Document(PageSize.A4);
        HTMLWorker parser = new HTMLWorker(doc);
        PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~") + "./App_Data/HTMLToPDF.pdf",

        FileMode.Create));
        doc.Open();

        try
        {
            //Parse Html and dump the result in PDF file
            parser.Parse(reader);
        }
        catch (Exception ex)
        {
            //Display parser errors in PDF.
            Paragraph paragraph = new Paragraph("Error!" + ex.Message);
            Chunk text = paragraph.Chunks[0] as Chunk;
            if (text != null)
            {
                text.Font.Color = BaseColor.RED;
            }
            doc.Add(paragraph);
        }
        finally
        {
            doc.Close();
        }
    }

    public string getImage(string input)
    {
        if (input == null)
            return string.Empty;
        string tempInput = input;
        string pattern = @"<img(.|\n)+?>";
        string src = string.Empty;
        HttpContext context = HttpContext.Current;

        //Change the relative URL's to absolute URL's for an image, if any in the HTML code.
        foreach (Match m in Regex.Matches(input, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline |

        RegexOptions.RightToLeft))
        {
            if (m.Success)
            {
                string tempM = m.Value;
                string pattern1 = "src=[\'|\"](.+?)[\'|\"]";
                Regex reImg = new Regex(pattern1, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Match mImg = reImg.Match(m.Value);

                if (mImg.Success)
                {
                    src = mImg.Value.ToLower().Replace("src=", "").Replace("\"", "");

                    if (src.ToLower().Contains("http://") == false)
                    {
                        //Insert new URL in img tag
                        src = "src=\"" + context.Request.Url.Scheme + "://" +
                        context.Request.Url.Authority + src + "\"";
                        try
                        {
                            tempM = tempM.Remove(mImg.Index, mImg.Length);
                            tempM = tempM.Insert(mImg.Index, src);

                            //insert new url img tag in whole html code
                            tempInput = tempInput.Remove(m.Index, m.Length);
                            tempInput = tempInput.Insert(m.Index, tempM);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
        }
        return tempInput;
    }

    string getSrc(string input)
    {
        string pattern = "src=[\'|\"](.+?)[\'|\"]";
        System.Text.RegularExpressions.Regex reImg = new System.Text.RegularExpressions.Regex(pattern,
        System.Text.RegularExpressions.RegexOptions.IgnoreCase |

        System.Text.RegularExpressions.RegexOptions.Multiline);
        System.Text.RegularExpressions.Match mImg = reImg.Match(input);
        if (mImg.Success)
        {
            return mImg.Value.Replace("src=", "").Replace("\"", ""); ;
        }

        return string.Empty;
    }


}