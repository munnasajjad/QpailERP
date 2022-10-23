using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Work_Slots : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //txtCode.Text = "Drinks3";


        if (!IsPostBack)
        {
            ddItem.DataBind();
            ddProgram.DataBind();
            ddFavrication.DataBind();

            ddGroup.DataBind();
            ddhead.DataBind();
            GridView1.DataBind();
            GridView2.DataBind();
        }
    }
    private void ExecuteInsert()
    {
        try
        {
            SQLQuery.ExecNonQry("INSERT INTO itemcostingbm (ItemId, ProgramId, FabricationId, ProductCode, Details, Size, Units, EntryBy)" +
                                " VALUES ('" + ddItem.SelectedValue + "','" + ddProgram.SelectedValue + "','" + ddFavrication.SelectedValue + "','" + txtCode.Text + "','" + txtDescription.Text + "','" + ddSize.SelectedValue + "','" + ddUnit.SelectedValue + "','" + Page.User.Identity.Name.ToString() + "')");


        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in Save: " + ex.ToString();
        }

    }

    private void updateData(string sl)
    {
        //if (FileUpload1.HasFile)
        //{
        //    string photoURL = RunQuery.SQLQuery.UploadImage(txtCode.Text, FileUpload1, Server.MapPath(".\\Uploads\\Photos\\"), Server.MapPath("./Uploads/Photos/"), Page.User.Identity.Name.ToString(), "Sample Library");
        //    RunQuery.SQLQuery.ExecNonQry("UPDATE ItemCostingbm SET PhotoId='" + photoURL + "' where (cid ='" + sl + "')");
        //}

        SqlCommand cmd2 = new SqlCommand("UPDATE ItemCostingbm SET ItemId=@ItemId, ProgramId=@ProgramId, " +
                                         " FabricationId=@FabricationId, ProductCode=@ProductCode, Details=@Details, Size=@Size, " +
                                         " Units=@Units where (cid ='" + sl + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ItemId", ddItem.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProgramId", ddProgram.SelectedValue);
        cmd2.Parameters.AddWithValue("@FabricationId", ddFavrication.SelectedValue);
        cmd2.Parameters.AddWithValue("@ProductCode", txtCode.Text);
        cmd2.Parameters.AddWithValue("@Details", txtDescription.Text);
        cmd2.Parameters.AddWithValue("@Size", ddSize.SelectedValue);
        cmd2.Parameters.AddWithValue("@Units", ddUnit.SelectedValue);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
    }


    protected void btnSave_Click1(object sender, EventArgs e)
    {
        try
        {
            if (txtCode.Text != "")
            {
                if (btnSave.Text == "Save")
                {
                    ExecuteInsert();
                    string maxSl = SQLQuery.ReturnString("Select ISNULL(MAX(CID),0) from ItemCostingbm");
                    updateData(maxSl);
                    SQLQuery.ExecNonQry("Update ItemCostingDetail Set ProductCode='" + maxSl + "' Where ProductCode=' ' ");
                    ClearForm();
                    Notify("Info Saved...", "success", lblMsg);
                }
                else
                {
                    updateData(lblId.Text);
                    btnSave.Text = "Save";
                    ClearForm();
                    Notify("Info Updated...", "success", lblMsg);
                }

            }
            else
            {
                lblMsg.Text = "Please type Item Group Name";
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
        finally
        {
            GridView1.DataBind();
        }

    }
    protected void btnClear_Click1(object sender, EventArgs e)
    {
        ClearForm();
    }

    private void ClearForm()
    {
        btnSave.Text = "Save";
        txtCode.Text = "";
        txtDescription.Text = "";

        GridView1.DataBind();
        lblCodeSl.Text = " ";
        GridView2.DataBind();
        Image1.ImageUrl = string.Empty;
    }

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        lblId.Text = lblItemName.Text;
        lblCodeSl.Text = lblItemName.Text;

        EditMode(lblItemName.Text);
        GridView2.DataBind();

        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Edit mode activated ...";
    }

    private void EditMode(string id)
    {
        SqlCommand cmd7 = new SqlCommand("SELECT ItemId, ProgramId, FabricationId, ProductCode, Details, Size, Units, PhotoId FROM [itemcostingbm] WHERE Cid='" + id + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            //EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";
            lblId.Text = id;
            ddItem.SelectedValue = dr[0].ToString();
            ddProgram.SelectedValue = dr[1].ToString();
            ddFavrication.SelectedValue = dr[2].ToString();
            txtCode.Text = dr[3].ToString();
            txtDescription.Text = dr[4].ToString();

            ddSize.SelectedValue = dr[5].ToString();
            ddUnit.SelectedValue = dr[6].ToString();

            string photoUrl = SQLQuery.ReturnString("Select PhotoURL  from Photos where PhotoID='" + dr[7].ToString() + "'");
            Image1.ImageUrl = photoUrl;
            HyperLink1.NavigateUrl = photoUrl;
            Image1.Visible = true;
        }
        cmd7.Connection.Close();
    }

    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            //string isExist = RunQuery.SQLQuery.ReturnString("Select top(1) WarehouseID FROM Stock WHERE WarehouseID='" + lblItemCode.Text + "'");

            //if (isExist == "")
            //{
            SqlCommand cmd7 = new SqlCommand("DELETE itemcostingbm WHERE CID=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();

            SQLQuery.ExecNonQry("Delete ItemCostingDetail where ProductCode='" + lblItemCode.Text + "'");

            lblMsg.Attributes.Add("class", "xerp_warning");
            lblMsg.Text = "Entry deleted successfully ...";
            //}
            //else
            //{
            //    lblMsg.Attributes.Add("class", "xerp_error");
            //    lblMsg.Text = "ERROR: This warehouse has existing items! ";
            //}

            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    protected void ddItem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    protected void ddGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddhead.DataBind();
    }
    protected void btnAdd_OnClick(object sender, EventArgs e)
    {
        try
        {
            decimal total = Convert.ToDecimal(txtPrice.Text) * Convert.ToDecimal(txtConsumption.Text);
            if (ddhead.SelectedValue != "")
            {
                string prdCode = " ";
                if (btnSave.Text != "Save")
                {
                    prdCode = lblId.Text;
                    lblCodeSl.Text = prdCode;
                }

                if (btnAdd.Text != "Update")
                {
                    int displaySerial = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(MAX(DisplaySerial),0) from ItemCostingDetail where ProductCode='" + prdCode + "'")) + 1;
                    SQLQuery.ExecNonQry("INSERT INTO ItemCostingDetail (ProductCode, CostingGroupId, CostingHeadId, UnitID, UnitPrice, Consumption, TotalCost, DisplaySerial)" +
                                " VALUES ('" + prdCode + "','" + ddGroup.SelectedValue + "','" + ddhead.SelectedValue + "','" + ddCostUnit.SelectedValue + "','" + txtPrice.Text + "','" + txtConsumption.Text + "','" + total + "','" + displaySerial + "')");

                    string maxSl = SQLQuery.ReturnString("Select ISNULL(MAX(sl),0) from ItemCostingDetail");
                    UpdateDetailData(maxSl, prdCode);
                    ClearForm2();
                    Notify("Info Added...", "success", lblMsg);
                }
                else
                {
                    UpdateDetailData(lblSl.Text, prdCode);
                    btnSave.Text = "Save";
                    ClearForm2();
                    Notify("Info Updated...", "success", lblMsg);
                }
            }
            else
            {
                Notify("Please input head name...", "error", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    private void UpdateDetailData(string sl, string pid)
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE ItemCostingDetail SET ProductCode=@ProductCode, CostingGroupId=@CostingGroupId, " +
                                         " CostingHeadId=@CostingHeadId, UnitID=@UnitID, UnitPrice=@UnitPrice, Consumption=@Consumption, " +
                                         " TotalCost=@TotalCost where (sl ='" + sl + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductCode", pid);
        cmd2.Parameters.AddWithValue("@CostingGroupId", ddGroup.SelectedValue);
        cmd2.Parameters.AddWithValue("@CostingHeadId", ddhead.SelectedValue);
        cmd2.Parameters.AddWithValue("@UnitID", ddCostUnit.SelectedValue);
        cmd2.Parameters.AddWithValue("@UnitPrice", txtPrice.Text);
        cmd2.Parameters.AddWithValue("@Consumption", txtConsumption.Text);
        cmd2.Parameters.AddWithValue("@TotalCost", Convert.ToDecimal(txtPrice.Text) * Convert.ToDecimal(txtConsumption.Text));

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();

    }
    private void ClearForm2()
    {
        lblCodeSl.Text = " ";
        btnAdd.Text = "Add Costing Info";
        txtPrice.Text = "";
        txtConsumption.Text = "";
        txtTotal.Text = "";
        GridView2.DataBind();
    }

    protected void GridView2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView2.SelectedIndex);
        Label lblItemName = GridView2.Rows[index].FindControl("Label1") as Label;
        lblSl.Text = lblItemName.Text;

        SqlCommand cmd7 = new SqlCommand("SELECT CostingGroupId, CostingHeadId, UnitID, UnitPrice, Consumption, TotalCost FROM [ItemCostingDetail] WHERE sl='" + lblSl.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            btnAdd.Text = "Update";
            ddGroup.SelectedValue = dr[0].ToString();
            ddhead.DataBind();
            ddhead.SelectedValue = dr[1].ToString();
            ddCostUnit.SelectedValue = dr[2].ToString();
            txtPrice.Text = dr[3].ToString();
            txtConsumption.Text = dr[4].ToString();
            txtTotal.Text = dr[5].ToString();
        }
        cmd7.Connection.Close();
        Notify("Edit mode activated ...", "info", lblMsg2);
    }

    protected void GridView2_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView2.Rows[index].FindControl("Label1") as Label;

            SQLQuery.ExecNonQry("Delete ItemCostingDetail where sl='" + lblItemCode.Text + "'");
            Notify("Entry deleted successfully ...", "warn", lblMsg2);
            GridView2.DataBind();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg2);
        }
    }

    protected void GridView2_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        txtNetTotal.Text = SQLQuery.ReturnString("Select SUM(TotalCost) from ItemCostingDetail where ProductCode='" + lblCodeSl.Text + "'");
    }
    protected void FireRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string command = e.CommandName;
            string autoId = e.CommandArgument.ToString();
            int sl = Convert.ToInt32(autoId);
            string pCode = SQLQuery.ReturnString("Select ProductCode from ItemCostingDetail where sl='" + sl + "'");


            switch (command)
            {
                case "MoveUp":
                    int thisDs = Convert.ToInt32(SQLQuery.ReturnString("Select  DisplaySerial from ItemCostingDetail where sl='" + sl + "' "));

                    int moveSl = Convert.ToInt32(SQLQuery.ReturnString("Select TOP(1) sl from ItemCostingDetail where DisplaySerial<" + thisDs +
                                                  " AND ProductCode='" + pCode + "' Order by DisplaySerial desc"));
                    int lastDs = Convert.ToInt32(SQLQuery.ReturnString("Select DisplaySerial from ItemCostingDetail where sl='" + moveSl + "'"));

                    SQLQuery.ExecNonQry("Update ItemCostingDetail SET DisplaySerial= '" + thisDs + "' where sl= '" + moveSl + "' ");
                    SQLQuery.ExecNonQry("Update ItemCostingDetail SET DisplaySerial= '" + lastDs + "' where sl= '" + sl + "' ");
                    break;
                case "MoveDown":
                    thisDs = Convert.ToInt32(SQLQuery.ReturnString("Select  DisplaySerial from ItemCostingDetail where sl='" + sl + "' "));

                    moveSl = Convert.ToInt32(SQLQuery.ReturnString("Select TOP(1) sl from ItemCostingDetail where DisplaySerial>" + thisDs +
                                                 " AND ProductCode='" + pCode + "' Order by DisplaySerial "));
                    lastDs = Convert.ToInt32(SQLQuery.ReturnString("Select DisplaySerial from ItemCostingDetail where sl='" + moveSl + "'"));

                    SQLQuery.ExecNonQry("Update ItemCostingDetail SET DisplaySerial= '" + thisDs + "' where sl= '" + moveSl + "' ");
                    SQLQuery.ExecNonQry("Update ItemCostingDetail SET DisplaySerial= '" + lastDs + "' where sl= '" + sl + "' ");
                    break;

            }

            GridView2.DataBind();

        }
        catch (Exception)
        {

        }
    }
}
