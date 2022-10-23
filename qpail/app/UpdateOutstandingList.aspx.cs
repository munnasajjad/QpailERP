using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_UpdateOutstandingList : System.Web.UI.Page
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
        DataTable dtxn = SQLQuery.ReturnDataTable(@"Select SaleID, InvNo, InvDate, SalesMode, CustomerID, CustomerName, PONo, PODate, Period, DeliveryDate, DeliveryTime, DeliveryLocation, TransportDetail, MaturityDays, OverdueDays, Remarks, InvoiceTotal, 
                         VATPercent, VATAmount, PayableAmount, TDSRate, TDSAmount, VDSrate, VDSAmount, CartonQty, ChallanNo, NetPayable, CollectedAmount, DueAmount, ProjectID, EntryBy, EntryDate, IsActive, OverDueDate, InWords, 
                         Warehouse, VatChalNo, VatChalDate, InvCompany FROM Sales WHERE (IsActive = '1') AND (InvDate <= '2019-09-16') AND (InvNo NOT IN (SELECT InvNo FROM TempOutstandingList))");
        foreach (DataRow drxn in dtxn.Rows)
        {
            SQLQuery.ExecNonQry("UPDATE Sales SET IsActive='" + 0 + "' WHERE InvNo ='" + drxn["InvNo"] + "'");
        }
        //}
        Notify("Process complite", "success", lblMsg);
    }
}