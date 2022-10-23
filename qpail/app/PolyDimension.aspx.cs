using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_PolyDimension246 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            BindGrid();
        }
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }


    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {


            string lName = Page.User.Identity.Name.ToString();
            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            int prjId = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Connection.Close();
            int displaySl = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("SELECT ISNULL(Max(DisplaySl),0) FROM PolyDimension")) + 10;
            if (btnSave.Text == "Save")
            {
                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                {
                    SqlCommand cmd2 = new SqlCommand("INSERT INTO PolyDimension (DimensionName, Company, PackSizeID, StandardWeight, ProjectID, DisplaySl, EntryBy) VALUES (@DimensionName, @Company, @PackSizeID, @StandardWeight, @ProjectID, @DisplaySl, @EntryBy)",
                        new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmd2.Parameters.AddWithValue("@DimensionName", txtDimensionName.Text);
                    cmd2.Parameters.AddWithValue("@Company", ddCompany.SelectedValue);
                    cmd2.Parameters.AddWithValue("@PackSizeID", ddPackSize.SelectedValue);
                    cmd2.Parameters.AddWithValue("@StandardWeight", txtStandardWeight.Text);
                    cmd2.Parameters.AddWithValue("@ProjectID", prjId);
                    cmd2.Parameters.AddWithValue("@DisplaySl", displaySl);
                    cmd2.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name);
                    cmd2.Connection.Open();
                    cmd2.ExecuteNonQuery();
                    cmd2.Connection.Close();
                    ClearControls();
                    Notify("Successfully Saved...", "success", lblMsg);
                }
                else
                {
                    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                }
            }
            else
            {
                if (SQLQuery.OparatePermission(lName, "Update") == "1")
                {
                    RunQuery.SQLQuery.ExecNonQry("UPDATE  PolyDimension SET DimensionName= '" + txtDimensionName.Text + "',  Company= '" + ddCompany.SelectedValue + "', PackSizeID='"+ddPackSize.SelectedValue+"', StandardWeight='" + txtStandardWeight.Text + "',   ProjectID= '" + prjId + "',  DisplaySl= '" + displaySl + "' WHERE PolyDimension.Id='" + lblId.Text + "' ");
                    ClearControls();
                    btnSave.Text = "Save";
                    Notify("Successfully Updated...", "success", lblMsg);
                }
                else
                {
                    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                }
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
        finally
        {
            BindGrid();
        }
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (SQLQuery.OparatePermission(lName, "Read") == "1")
            {
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label lblEditId = GridView1.Rows[index].FindControl("Label1") as Label;
                lblId.Text = lblEditId.Text;
                DataTable dt = SQLQuery.ReturnDataTable("SELECT DimensionName, Company, PackSizeID, StandardWeight, ProjectID, DisplaySl FROM PolyDimension WHERE PolyDimension.Id='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtDimensionName.Text = dtx["DimensionName"].ToString();
                    ddCompany.SelectedValue = dtx["Company"].ToString();
                    ddPackSize.SelectedValue = dtx["PackSizeID"].ToString();
                    txtStandardWeight.Text = dtx["StandardWeight"].ToString();
                    txtProjectID.Text = dtx["ProjectID"].ToString();
                    txtDisplaySl.Text = dtx["DisplaySl"].ToString();
                }
                btnSave.Text = "Update";
                Notify("Edit mode activated ...", "info", lblMsg);
            }
            else
            {
                Notify("You are not authorized to select this data", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;
            SQLQuery.ExecNonQry("DELETE PolyDimension WHERE PolyDimension.Id='" + lblId.Text + "' ");
            BindGrid();
            Notify("Successfully Deleted...", "success", lblMsg);
        }
        else
        {
            Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        }
    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("./Default.aspx");
    }

    private void BindGrid()
    {
        DataTable dt = SQLQuery.ReturnDataTable("SELECT PD.Id, PD.DimensionName, P.Company, PS.PackSize, PD.StandardWeight, PD.ProjectID, PD.DisplaySl FROM PolyDimension AS PD INNER JOIN Party AS P ON PD.Company = P.PartyID INNER JOIN PolyFinishedGoodsPackSize AS PS ON PD.PackSizeID = PS.Id WHERE PD.Id<>0 ");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }



    private void ClearControls()
    {
        txtDimensionName.Text = "";
        txtProjectID.Text = "";
        txtDisplaySl.Text = "";
        txtStandardWeight.Text = "";
    }

    protected void ddCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddPackSize.DataBind();
    }










}
