using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Document_Library : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        if (!IsPostBack)
        {
            string q = Request.QueryString["type"];
            if (q != null && q != "Search")
            {
                Loaddd(q);
                q = "Upload " + q;
            }
            else
            {
                entryPanel.Visible = false;
                SearchPanel.Visible = true;
                LoadGrid("Search", txtSearch.Text);
                txtSearch.Focus();
            }
            Page.Title = q + " Documents";
            headName.Text = q;
        }
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error                     
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    private void Loaddd(string type)
    {
        if (type == "LC")
        {
            SQLQuery.PopulateDropDown("Select sl, LCNo+' Op.Date:'+(CONVERT(varchar,OpenDate,103)) as LCNo from LC order by sl desc ", ddList, "sl", "LCNo");
            LoadGrid(type, ddList.SelectedItem.Text);
        }
        else if (type == "PO")
        {
            SQLQuery.PopulateDropDown("Select OrderSl, OrderID from Orders WHERE OrderType<>'LC' order by OrderSl desc ", ddList, "OrderSl", "OrderID");
            LoadGrid(type, ddList.SelectedItem.Text);
        }
        else if (type == "Purchase")
        {
            SQLQuery.PopulateDropDown("Select PID, InvNo+ ' Bill# '+  BillNo+' Date:'+(CONVERT(varchar,OrderDate,103)) as BillNo  from Purchase  order by PID desc ", ddList, "PID", "BillNo");
            LoadGrid(type, ddList.SelectedItem.Text);
        }
        else if (type == "LLC")
        {
            SQLQuery.PopulateDropDown("Select OrderSl, OrderID from Orders WHERE OrderType='LC' order by OrderSl desc ", ddList, "OrderSl", "OrderID");
            LoadGrid(type, ddList.SelectedItem.Text);
        }
        else if (type == "Search")
        {
            
        }
        else
        {
            ddList.Visible = false;
            txtName.Visible = true;
            LoadGrid("Others", "");
        }
    }
    private void LoadGrid(string type, string lcNo)
    {
        string query = "SELECT id, [BusNo], [ImgDetail], [Img] FROM [ImportentDocuments] " +
                       "WHERE DocType='" + type + "' AND (BusNo Like '%" + lcNo + "%' OR ImgDetail Like '%" + lcNo +
                       "%') ORDER BY [EntryDate] DESC";
        if (type=="Search")
        {
            query = "SELECT id, [BusNo], [ImgDetail], [Img] FROM [ImportentDocuments] WHERE DocType<>'Company' AND (BusNo Like '%" + lcNo + "%' OR ImgDetail Like '%" + lcNo + "%') ORDER BY [EntryDate] DESC";
        }
        GridView1.DataSource = SQLQuery.ReturnDataTable(query);
        GridView1.DataBind();
    }

    private void saveData()
    {
        string q = Request.QueryString["type"];
        string name = txtName.Text;
        if (ddList.Visible)
        {
            name = ddList.SelectedItem.Text;
        }

        SqlCommand cmd1 = new SqlCommand("SELECT isnull(max(ID) + 1,1) FROM ImportentDocuments", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd1.Connection.Open();
        string fileName = cmd1.ExecuteScalar().ToString();
        cmd1.Connection.Close();

        string tExt = Path.GetFileName(ThumbUpload.PostedFile.ContentType);
        string thumbName = fileName + "." + tExt;
        ThumbUpload.SaveAs(Server.MapPath("./Docs/Admin/" + thumbName));

        SqlCommand cmd = new SqlCommand("INSERT INTO ImportentDocuments (BusNo, ImgDetail, Img, DocType, LinkID, EntryBy)" +
                                    "VALUES (@BusNo, @ImgDetail, @Img, '" + q + "', '" + ddList.SelectedValue + "', '" + Page.User.Identity.Name.ToString() + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@BusNo", name);
        cmd.Parameters.AddWithValue("@ImgDetail", txtDescription.Text);
        cmd.Parameters.AddWithValue("@Img", "Docs/Admin/" + thumbName);

        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ThumbUpload.HasFile)
            {
                saveData();
                Notify("New Document Successfully Uploaded", "success", lblMsg);
            }
            else
            {
                Notify("No file found to upload!", "error", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
        finally
        {
            string q = Request.QueryString["type"];
            LoadGrid(q, ddList.SelectedItem.Text);
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtName.Text = "";
        txtDescription.Text = "";
    }

    protected void txtSearch_OnTextChanged(object sender, EventArgs e)
    {
        LoadGrid("Search", txtSearch.Text);
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //DropDownList1.DataBind();
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label lblItemName = GridView1.Rows[index].FindControl("lblEntryId") as Label;
            lblOrderID.Text = lblItemName.Text;

            txtName.Text = SQLQuery.ReturnString("Select BusNo from ImportentDocuments where id='" + lblItemName.Text + "'");
            txtDescription.Text = SQLQuery.ReturnString("Select ImgDetail from ImportentDocuments where id='" + lblItemName.Text + "'");

            btnSave.Text = "Update";
            Notify("Edit mode activated ...", "info", lblMsg);
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("lblEntryId") as Label;
            SQLQuery.ExecNonQry("Delete ImportentDocuments where id='" + lblItemCode.Text + "'");
            txtSearch.Text = "";
            string q = Request.QueryString["type"];
            LoadGrid(q, ddList.SelectedItem.Text);
            Notify("File has been deleted!", "warn", lblMsg);
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }

    }

    protected void ddList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string q = Request.QueryString["type"];
        LoadGrid(q, ddList.SelectedItem.Text);
    }
}

