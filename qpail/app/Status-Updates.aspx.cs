using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class Status_Updates : System.Web.UI.Page
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
                q = "Update " + q;
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
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
            SQLQuery.PopulateDropDown("Select sl, 'LC# '+ LCNo+' - Open Date: '+(CONVERT(varchar,OpenDate,103) +'') as LCNo from LC order by sl desc ", ddList, "sl", "LCNo");
            LoadGrid(type, ddList.SelectedItem.Text);
        }
        else if (type == "PO")
        {
            SQLQuery.PopulateDropDown("Select OrderSl, OrderID from Orders WHERE OrderType<>'LC' order by OrderSl desc ", ddList, "OrderSl", "OrderID");
            LoadGrid(type, ddList.SelectedItem.Text);
        }
        else if (type == "Purchase")
        {
            SQLQuery.PopulateDropDown("Select PID, BillNo from Purchase  order by PID desc ", ddList, "PID", "BillNo");
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
        string query = "SELECT EntryDate, id, Subject, [BusNo], [ImgDetail], [Img]+'#' as img FROM [ImportentDocuments] " +
                       "WHERE DocType='" + type + "' AND (BusNo Like '%" + lcNo + "%') ORDER BY [EntryDate] DESC, id desc";
        if (type == "Search")
        {
            query = "SELECT EntryDate, id, [BusNo], [ImgDetail], [Img] FROM [ImportentDocuments] WHERE DocType<>'Company' AND (BusNo Like '%" + lcNo + "%' OR ImgDetail Like '%" + lcNo + "%') ORDER BY [EntryDate] DESC";
        }
        GridView1.DataSource = SQLQuery.ReturnDataTable(query);
        GridView1.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtName.Text != "")
            {
                if (btnSave.Text == "Update")
                {
                    updateData();
                    btnSave.Text = "Save";
                    Notify("Status Updated Accordingly.", "success", lblMsg);
                    txtName.Text = "";
                    txtDescription.Text = "";
                }
                else
                {
                    saveData();
                    Notify("LC Status Saved Successfully", "success", lblMsg);
                    txtName.Text = "";
                    txtDescription.Text = "";
                }
            }
            else
            {
                Notify("Please input Status Summery...", "error", lblMsg);
                txtName.Focus();
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

    private void saveData()
    {
        string q = Request.QueryString["type"];
        string name = txtName.Text;
        if (ddList.Visible)
        {
            name = ddList.SelectedItem.Text;
        }

        string imgLink = "./images/no-img.png";
        if (ThumbUpload.HasFile)
        {
            SqlCommand cmd1 = new SqlCommand("SELECT isnull(max(ID) + 1,1) FROM ImportentDocuments",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd1.Connection.Open();
            string fileName = cmd1.ExecuteScalar().ToString();
            cmd1.Connection.Close();

            //string tExt = Path.GetFileName(ThumbUpload.PostedFile.ContentType);
            FileInfo fi = new FileInfo(ThumbUpload.PostedFile.FileName);
            string tExt = fi.Extension;
            string thumbName = fileName + "." + tExt;
            ThumbUpload.SaveAs(Server.MapPath("./Docs/Admin/" + thumbName));
            imgLink = "Docs/Admin/" + thumbName;
        }

        SqlCommand cmd = new SqlCommand("INSERT INTO ImportentDocuments (BusNo, subject, ImgDetail, Img, DocType, LinkID, EntryBy, EntryDate)" +
                                    "VALUES (@BusNo, '" + txtName.Text + "', @ImgDetail, @Img, '" + q + "', '" + ddList.SelectedValue + "', '" + Page.User.Identity.Name.ToString() + "', '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@BusNo", name);
        cmd.Parameters.AddWithValue("@ImgDetail", txtDescription.Text);
        cmd.Parameters.AddWithValue("@Img", imgLink);

        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();

        if (q == "LC")
        {
            SQLQuery.ExecNonQry("Update LC set status='" + txtName.Text + "' where sl='" + ddList.SelectedValue + "'");
        }
    }


    private void updateData()
    {
        if (ThumbUpload.HasFile)
        {
            SqlCommand cmd1 = new SqlCommand("SELECT isnull(max(ID) + 1,1) FROM ImportentDocuments",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd1.Connection.Open();
            string fileName = cmd1.ExecuteScalar().ToString();
            cmd1.Connection.Close();

            string tExt = Path.GetFileName(ThumbUpload.PostedFile.ContentType);
            string thumbName = fileName + "." + tExt;
            ThumbUpload.SaveAs(Server.MapPath("./Docs/Admin/" + thumbName));
            string  imgLink = "Docs/Admin/" + thumbName;

            SQLQuery.ExecNonQry("UPDATE ImportentDocuments SET img='" + imgLink + "' WHERE id='" + lblOrderID.Text + "' ");
        }

        SqlCommand cmd = new SqlCommand("UPDATE ImportentDocuments SET subject='" + txtName.Text + "', ImgDetail=@ImgDetail, EntryDate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' WHERE id='"+lblOrderID.Text+"' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@ImgDetail", txtDescription.Text);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();

        //if (q == "LC")
        //{
        //    SQLQuery.ExecNonQry("Update LC set status='" + txtName.Text + "' where sl='" + ddList.SelectedValue + "'");
        //}
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
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT BusNo, Subject, ImgDetail, Img, EntryDate, DocType, LinkID, EntryBy FROM ImportentDocuments WHERE ( id = '" + lblItemName.Text + "')");

            foreach (DataRow drx in dtx.Rows)
            {
                //ddList.SelectedValue = drx["LinkID"].ToString();
                txtName.Text = drx["Subject"].ToString();
                txtDescription.Text = drx["ImgDetail"].ToString();
                txtDate.Text = Convert.ToDateTime(drx["EntryDate"].ToString()).ToString("dd/MM/yyyy");

            }                      
                 
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
        txtName.Focus();
    }
}

