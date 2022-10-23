using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Acc_Rpt_Settings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //txtOpening.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //string date = Convert.ToDateTime(txtOpening.Text).AddDays(-1).ToString("yyyy-MM-dd");
            //BankBalance(date);
        }
    }
    
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        string q = "";
        foreach (GridViewRow row in GridView1.Rows)
        {
            Label lblSl = (Label) row.FindControl("Label1");
            int ckd1 = 0, ckd2 = 0;
            if (((CheckBox)row.FindControl("CheckBox1")).Checked)
            {
                ckd1 = 1;
            }
            if (((CheckBox)row.FindControl("CheckBox2")).Checked)
            {
                ckd2 = 1;
            }

            q += @" UPDATE       ControlAccount
                    SET ReceiptShow = '" + ckd1 + "' , PaymentShow = '" + ckd2 + "' WHERE  sl='" + lblSl.Text + "' ";

        }

        SQLQuery.ExecNonQry(q);
        GridView1.DataBind();
        Notify("Saved Successfully","success",lblMsg);

    }

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
}