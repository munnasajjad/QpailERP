using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class app_Fact_Height_Spec : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //txtHeading.Focus();
        if (!IsPostBack)
        {
            ddType.DataBind();
            ddCategory.DataBind();
            ddSize.DataBind();
            SelectData();
        }
    }

    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string isExist

                = RunQuery.SQLQuery.ReturnString("SELECT DeclarationText FROM [Declarations]  where (DeclarationType='Consum' AND CatID ='" + ddCategory.SelectedValue + "' AND SizeID='" + ddSize.SelectedValue + "')");
            if (isExist == "")
            {
                ExecuteInsert();
            }
            else
            {
                ExecuteUpdate();
            }

            SelectData();
            lblMsg.Attributes.Add("class", "xerp_success");
            lblMsg.Text = "Successfully Saved!";
            //txtMsgBody.Text = "";
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    protected void ddType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddCategory.DataBind();
        SelectData();
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectData();
    }
    protected void ddSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectData();
    }
    private void SelectData()
    {
        string isExist = RunQuery.SQLQuery.ReturnString("SELECT DeclarationText FROM [Declarations]  where (DeclarationType='Consum' AND CatID ='" + ddCategory.SelectedValue + "' AND SizeID='" + ddSize.SelectedValue + "')");
        if (isExist == "")
        {
            ltrBody.Text = HttpUtility.HtmlDecode(isExist);
            txtMsgBody.Text = HttpUtility.HtmlDecode(isExist);
            ltrBody.Visible = false;
            txtMsgBody.Visible = true;
            lbEdit.Visible = false;
            btnSave.Visible = true;
        }
        else
        {
            ltrBody.Text = HttpUtility.HtmlDecode(isExist);
            txtMsgBody.Text = HttpUtility.HtmlDecode(isExist);
            ltrBody.Visible = true;
            txtMsgBody.Visible = false;
            lbEdit.Visible = true;
            btnSave.Visible = false;
            //SqlCommand cmd7 = new SqlCommand("SELECT DeclarationText, EntryBy FROM [Declarations]  where (DeclarationType='Consum' AND CatID ='" + ddCategory.SelectedValue + "' AND SizeID='" + ddSize.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            //cmd7.Connection.Open();
            //SqlDataReader dr = cmd7.ExecuteReader();
            //if (dr.Read())
            //{
            //    txtMsgBody.Text = dr[0].ToString();
            //}
            //cmd7.Connection.Close();
        }
    }

    private void ExecuteInsert()
    {
        SqlCommand cmd2 = new SqlCommand("INSERT INTO Declarations (DeclarationType, SubGroupID, CatID, SizeID, DeclarationText, EntryBy) VALUES ('Consum', @SubGroupID, @CatID, @SizeID, @DeclarationText, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Parameters.AddWithValue("@SubGroupID", ddType.SelectedValue);
        cmd2.Parameters.AddWithValue("@CatID", ddCategory.SelectedValue);
        cmd2.Parameters.AddWithValue("@SizeID", ddSize.SelectedValue);
        cmd2.Parameters.AddWithValue("@DeclarationText", HttpUtility.HtmlEncode(txtMsgBody.Text.ToString()));
        cmd2.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE Declarations SET DeclarationText='" + HttpUtility.HtmlEncode(txtMsgBody.Text.ToString()) + "' where (DeclarationType='Consum' AND CatID ='" + ddCategory.SelectedValue + "' AND SizeID='" + ddSize.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
    }

    protected void lbEdit_Click(object sender, EventArgs e)
    {
        ltrBody.Visible = false;
        txtMsgBody.Visible = true;
        lbEdit.Visible = false;
        btnSave.Visible = true;
    }
}
