
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_MouldInfo240 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            txtReceiveDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
            if (btnSave.Text == "Save")
            {
                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                {
                    RunQuery.SQLQuery.ExecNonQry(" INSERT INTO MouldInfo (MouldName, MouldNo, Supplier, ReceiveDate, LCNo, Cavity, CycleTime, Warranty, Description, EntryBy) VALUES ('" + txtMouldName.Text + "', '" + txtMouldNo.Text + "', '" + txtSupplier.Text + "', '" + Convert.ToDateTime(txtReceiveDate.Text).ToString("yyyy-MM-dd") + "', '" + txtLCNo.Text + "', '" + txtCavity.Text + "', '" + txtCycleTime.Text + "', '" + txtWarranty.Text + "', '" + txtDescription.Text + "', '" + User.Identity.Name + "')    ");
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
                    RunQuery.SQLQuery.ExecNonQry(" Update  MouldInfo SET MouldName= '" + txtMouldName.Text + "',  MouldNo= '" + txtMouldNo.Text + "',  Supplier= '" + txtSupplier.Text + "',  ReceiveDate= '" + Convert.ToDateTime(txtReceiveDate.Text).ToString("yyyy-MM-dd") + "',  LCNo= '" + txtLCNo.Text + "',  Cavity= '" + txtCavity.Text + "',  CycleTime= '" + txtCycleTime.Text + "',  Warranty= '" + txtWarranty.Text + "',  Description= '" + txtDescription.Text + "' WHERE MouldInfo.Id='" + lblId.Text + "' ");
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
                DataTable dt = SQLQuery.ReturnDataTable(" Select Id, MouldName,MouldNo,Supplier,ReceiveDate,LCNo,Cavity,CycleTime,Warranty,Description FROM MouldInfo WHERE MouldInfo.Id='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtMouldName.Text = dtx["MouldName"].ToString();
                    txtMouldNo.Text = dtx["MouldNo"].ToString();
                    txtSupplier.Text = dtx["Supplier"].ToString();
                    txtReceiveDate.Text = Convert.ToDateTime(dtx["ReceiveDate"].ToString()).ToString("dd-MM-yyyy");
                    txtLCNo.Text = dtx["LCNo"].ToString();
                    txtCavity.Text = dtx["Cavity"].ToString();
                    txtCycleTime.Text = dtx["CycleTime"].ToString();
                    txtWarranty.Text = dtx["Warranty"].ToString();
                    txtDescription.Text = dtx["Description"].ToString();

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
            RunQuery.SQLQuery.ExecNonQry(" Delete MouldInfo WHERE MouldInfo.Id='" + lblId.Text + "' ");
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
        DataTable dt = SQLQuery.ReturnDataTable(" SELECT * FROM MouldInfo WHERE MouldInfo.Id<>0 ");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }



    private void ClearControls()
    {
        txtMouldName.Text = "";
        txtMouldNo.Text = "";
        txtSupplier.Text = "";
        txtLCNo.Text = "";
        txtCavity.Text = "";
        txtCycleTime.Text = "";
        txtWarranty.Text = "";
        txtDescription.Text = "";

    }










}
