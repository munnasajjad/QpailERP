using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_DataCorrection : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GV1.DataSource = SQLQuery.ReturnDataTable(@"SELECT DISTINCT (Select top(1) AccountsHeadName from VoucherDetails where AccountsHeadID=VoucherDetails.AccountsHeadID) as HeadName, AccountsHeadID AS OLDID, 
                             (SELECT        AccountsHeadID
                               FROM            HeadSetup
                               WHERE        (AccountsHeadName = VoucherDetails.AccountsHeadName)) AS HeadID
FROM            VoucherDetails
WHERE        (AccountsHeadID NOT IN
                             (SELECT        AccountsHeadID
                               FROM            HeadSetup AS HeadSetup_1))
ORDER BY OLDID");
            GV1.DataBind();
        }

    }

    protected void btnCorrection_OnClick(object sender, EventArgs e)
    {
        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT AccountsHeadID AS OLDID,
                             (SELECT AccountsHeadID FROM HeadSetup
                               WHERE (AccountsHeadName = VoucherDetails.AccountsHeadName)) AS newHeadID FROM VoucherDetails
                               WHERE (AccountsHeadID NOT IN (SELECT AccountsHeadID FROM HeadSetup AS HeadSetup_1))
                               ORDER BY OLDID");

        foreach (DataRow drx in dtx.Rows)
        {
           string oldID = drx["OLDID"].ToString();
            string newheadID = drx["newHeadID"].ToString();
            SQLQuery.ExecNonQry("Update HeadSetup SET AccountsHeadID='"+ oldID + "' WHERE  AccountsHeadID='"+ newheadID + "' ");
        }
        Response.Write("Voucher Details Process complete!");
        
        dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT AccountsHeadDr AS OLDID,
                             (SELECT AccountsHeadID FROM HeadSetup
                               WHERE (AccountsHeadName = VoucherTmp.AccountsHeadDr)) AS newHeadID FROM VoucherTmp
                               WHERE (AccountsHeadDr NOT IN (SELECT AccountsHeadID FROM HeadSetup AS HeadSetup_1))
                               ORDER BY OLDID");

        foreach (DataRow drx in dtx.Rows)
        {
            string oldID = drx["OLDID"].ToString();
            string newheadID = drx["newHeadID"].ToString();
            SQLQuery.ExecNonQry("Update VoucherTmp SET AccountsHeadDr='" + oldID + "' WHERE  AccountsHeadDr='" + newheadID + "' ");
        }
        Response.Write("AccountsHeadDr Process complete!");

        dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT DISTINCT AccountsHeadCr AS OLDID,
                             (SELECT AccountsHeadID FROM HeadSetup
                               WHERE (AccountsHeadName = VoucherTmp.AccountsHeadCr)) AS newHeadID FROM VoucherTmp
                               WHERE (AccountsHeadCr NOT IN (SELECT AccountsHeadID FROM HeadSetup AS HeadSetup_1))
                               ORDER BY OLDID");

        foreach (DataRow drx in dtx.Rows)
        {
            string oldID = drx["OLDID"].ToString();
            string newheadID = drx["newHeadID"].ToString();
            SQLQuery.ExecNonQry("Update VoucherTmp SET AccountsHeadCr='" + oldID + "' WHERE  AccountsHeadCr='" + newheadID + "' ");
        }
        Response.Write("AccountsHeadCr Process complete!");

    }
}