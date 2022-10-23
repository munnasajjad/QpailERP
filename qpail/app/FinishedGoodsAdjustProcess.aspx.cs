using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_FinishedGoodsAdjustProdess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddParticular.DataBind();
            lblUser.Text = Page.User.Identity.Name.ToString();
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
        //DataTable dtxo = SQLQuery.ReturnDataTable(@"Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ ControlAccountsName As ControlAccountsName from ControlAccount where PrdnLinkId='" + ddParticular.SelectedValue + "'");
        //foreach (DataRow drxo in dtxo.Rows)
        //{
            DataTable dtxn = SQLQuery.ReturnDataTable(@"Select AccountsHeadID, AccountsHeadName from HeadSetup where ControlAccountsID='" + ddControlDr.SelectedValue + "'");
            foreach (DataRow drxn in dtxn.Rows)
            {
                SQLQuery.ExecNonQry("UPDATE VoucherDetails SET DeliveredPcs='" + 0 + "' WHERE AccountsHeadID='" + drxn["AccountsHeadID"] + "' AND ISApproved<>'C'");
                SQLQuery.ExecNonQry("UPDATE VoucherDetails SET DeliveredStatus='P' WHERE AccountsHeadID='" + drxn["AccountsHeadID"] + "' AND ISApproved<>'C'");
                int outPcs = 0;
                int totalOutPcs = 0;
                totalOutPcs =Convert.ToInt32(SQLQuery.ReturnString("SELECT ISNULL(SUM(OutPcs),0) FROM [VoucherDetails] WHERE AccountsHeadID='" + drxn["AccountsHeadID"] + "' AND ISApproved<>'C'"));
                DataTable dtx =SQLQuery.ReturnDataTable(@"SELECT SerialNo, InPcs, OutPcs FROM VoucherDetails WHERE AccountsHeadID='" + drxn["AccountsHeadID"] + "'AND (VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE (ParticularID = '0104'))) AND ISApproved ='A' ORDER BY SerialNo");
                int opPcs = Convert.ToInt32(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpPcsDr),0) - ISNULL(SUM(OpPcsCr),0) FROM [HeadSetup] WHERE AccountsHeadID='" + drxn["AccountsHeadID"] + "'"));

                if (totalOutPcs > opPcs) //greater than opQty
                {
                    int remainQty = totalOutPcs - opPcs;

                    foreach (DataRow drx in dtx.Rows)
                    {
                        int inPcs = Convert.ToInt32(drx["InPcs"].ToString());
                        outPcs += Convert.ToInt32(drx["OutPcs"].ToString());

                        if (remainQty >= inPcs)
                        {
                            SQLQuery.ExecNonQry("UPDATE VoucherDetails SET DeliveredStatus='D' WHERE SerialNo='" + drx["SerialNo"] + "' ");
                            SQLQuery.ExecNonQry("UPDATE VoucherDetails SET DeliveredPcs='" + drx["InPcs"] + "' WHERE SerialNo='" + drx["SerialNo"] + "' ");
                        }
                        //get the last entry
                        else if (remainQty > 0)
                        {
                            SQLQuery.ExecNonQry("UPDATE VoucherDetails SET DeliveredPcs='" + remainQty + "' WHERE SerialNo='" + drx["SerialNo"] + "' ");
                            SQLQuery.ExecNonQry("UPDATE VoucherDetails SET DeliveredStatus='P' WHERE SerialNo='" + drx["SerialNo"] + "' ");
                        }
                        remainQty -= inPcs;
                    }
                }
            }
        //}
        Notify("Process complite", "success", lblMsg);
    }
}