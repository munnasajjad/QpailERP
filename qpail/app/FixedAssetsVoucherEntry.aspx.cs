
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using RunQuery;

public partial class app_FixedAssetsVoucher239 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            txtPurchaseDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            BindGrid();
            ddlist.DataBind();
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
                    RunQuery.SQLQuery.ExecNonQry(" INSERT INTO FixedAssetsVoucher (PurchaseType, AccountsHeadName, AccountHead, PurchaseDate, DepMethod, EcoLife, SystemProcessRun, Amount) VALUES ('" + ddlist.SelectedItem + "', '" + ddAccountsHeadName.SelectedValue + "', '" + ddAccountHead.SelectedValue + "', '" + Convert.ToDateTime(txtPurchaseDate.Text).ToString("yyyy-MM-dd") + "', '" + txtDepMethod.Text + "', '" + txtEcoLife.Text + "', '" + ddProcesstype.SelectedValue + "', '" + txtAmount.Text + "')    ");
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
                    RunQuery.SQLQuery.ExecNonQry(" Update  FixedAssetsVoucher SET PurchaseType= '" + ddlist.SelectedItem + "',  AccountsHeadName= '" + ddAccountsHeadName.SelectedValue + "',  AccountHead= '" + ddAccountHead.SelectedValue + "',  PurchaseDate= '" + Convert.ToDateTime(txtPurchaseDate.Text).ToString("yyyy-MM-dd") + "',  DepMethod= '" + txtDepMethod.Text + "',  EcoLife= '" + txtEcoLife.Text + "',  SystemProcessRun= '" + ddProcesstype.SelectedValue + "',  Amount= '" + txtAmount.Text + "' WHERE FixedAssetsVoucher.Id='" + lblId.Text + "' ");
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
                DataTable dt = SQLQuery.ReturnDataTable(" Select Id, PurchaseType,AccountsHeadName,AccountHead,PurchaseDate,DepMethod,EcoLife,SystemProcessRun,Amount FROM FixedAssetsVoucher WHERE FixedAssetsVoucher.Id='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    ddlist.SelectedValue = dtx["PurchaseType"].ToString();
                    ddAccountsHeadName.SelectedValue = dtx["AccountsHeadName"].ToString();
                    ddAccountHead.SelectedValue = dtx["AccountHead"].ToString();
                    txtPurchaseDate.Text = Convert.ToDateTime(dtx["PurchaseDate"].ToString()).ToString("dd-MM-yyyy");
                    txtDepMethod.Text = dtx["DepMethod"].ToString();
                    txtEcoLife.Text = dtx["EcoLife"].ToString();
                    ddProcesstype.SelectedValue = dtx["SystemProcessRun"].ToString();
                    txtAmount.Text = dtx["Amount"].ToString();

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
            RunQuery.SQLQuery.ExecNonQry(" Delete FixedAssetsVoucher WHERE FixedAssetsVoucher.Id='" + lblId.Text + "' ");
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
        DataTable dt = SQLQuery.ReturnDataTable(" SELECT * FROM FixedAssetsVoucher WHERE FixedAssetsVoucher.Id<>0 ");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }



    private void ClearControls()
    {
        ddlist.SelectedValue = "";
        //ddAccountsHeadName.SelectedValue=""; 
        //ddAccountHead.SelectedValue = ""; 
        txtDepMethod.Text = "";
        txtEcoLife.Text = "";
        ddProcesstype.SelectedValue = "";
        txtAmount.Text = "";

    }


    protected void ddlist_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlist.SelectedValue == "Local")
        {
            extrapanel.Visible = false;
            
        }
        else if (ddlist.SelectedValue== "Transfer")
        {
            extrapanel.Visible = false;
            
        }
        else if (ddlist.SelectedValue == "Other")
        {
            extrapanel.Visible = false;
            
        }
        else
        {
            extrapanel.Visible = true;
            
        }


    }

   
}
