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


public partial class app_Documents_Upload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        if (!IsPostBack)
        {
            LoadGrid();
        }
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error                     
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    private void LoadGrid()
    {
        GridView1.DataSource = SQLQuery.ReturnDataTable(
                "SELECT id, [BusNo], [ImgDetail], [Img] FROM [ImportentDocuments] " +
                "WHERE DocType='Company' AND (BusNo Like '%" + txtSearch.Text + "%' OR ImgDetail Like '%" + txtSearch.Text + "%') ORDER BY [EntryDate] DESC");
        GridView1.DataBind();
    }

    private void saveData()
    {
        SqlCommand cmd1 = new SqlCommand("SELECT isnull(max(ID) + 1,1) FROM ImportentDocuments", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd1.Connection.Open();
        string fileName = cmd1.ExecuteScalar().ToString();
        cmd1.Connection.Close();

        string tExt = Path.GetFileName(ThumbUpload.PostedFile.ContentType);
        string thumbName = fileName + "." + tExt;
        ThumbUpload.SaveAs(Server.MapPath("./Docs/Admin/" + thumbName));

        SqlCommand cmd = new SqlCommand("INSERT INTO ImportentDocuments (BusNo, ImgDetail, Img, DocType, EntryBy)" +
                                    "VALUES (@BusNo, @ImgDetail, @Img, 'Company', '"+Page.User.Identity.Name.ToString()+"')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@BusNo", txtName.Text);
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
            LoadGrid();
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtName.Text = "";
        txtDescription.Text = "";
    }

    protected void txtSearch_OnTextChanged(object sender, EventArgs e)
    {
        LoadGrid();
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
            Notify("Edit mode activated ...", "info",lblMsg);
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
            SQLQuery.ExecNonQry("Delete ImportentDocuments where id='" + lblItemCode .Text+ "'");
            txtSearch.Text = "";
            LoadGrid();
            Notify("File has been deleted!", "warn", lblMsg);
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }

    }
}

