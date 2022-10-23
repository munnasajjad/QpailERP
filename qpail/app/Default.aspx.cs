using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Bibliography;
using RunQuery;
using Label = System.Web.UI.WebControls.Label;

public partial class AdminCentral_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Get Branch/Centre Name
            string lName = Page.User.Identity.Name.ToString();
            SqlCommand cmdxx = new SqlCommand("Select BranchName from Users where Username='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmdxx.Connection.Open();
            string branch = Convert.ToString(cmdxx.ExecuteScalar());
            cmdxx.Connection.Close();
            cmdxx.Connection.Dispose();

            //Get current Balance
            SqlCommand cmd = new SqlCommand("SELECT isnull(SUM(Dr),0) - isnull(SUM(Cr),0) FROM Transactions WHERE HeadName =(Select BranchName from Users where Username='" + lName + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            decimal cBalance = Convert.ToDecimal(cmd.ExecuteScalar());
            cmd.Connection.Close();
            cmd.Connection.Dispose();

            //News Scroll
            SqlCommand cmdxxe = new SqlCommand("Select FullNews from NewsUpdates where MsgID=(Select max (MsgID) from NewsUpdates where Msgfor='News')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmdxxe.Connection.Open();
            lblNews.Text = Convert.ToString(cmdxxe.ExecuteScalar());
            cmdxxe.Connection.Close();
            cmdxxe.Connection.Dispose();

            //Get Pending Amount
            SqlCommand cmd2 = new SqlCommand("SELECT isnull(SUM(PurchaseTaka),0) FROM Orders WHERE isactive='true' and ReceiverBranchID in (SELECT [BranchName] FROM [Branches])", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2.Connection.Open();
            //decimal pending = Convert.ToDecimal(cmd2.ExecuteScalar());
            cmd2.Connection.Close();
            cmd2.Connection.Dispose();

            //Get Collection Amount
            SqlCommand cmd2d = new SqlCommand("SELECT isnull(SUM(Dr),0) FROM Transactions WHERE Description like 'Booking Entry%' and   EntryDate >= @DateFrom AND EntryDate <= @DateTo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2d.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString();
            cmd2d.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = DateTime.Now.AddDays(+1).ToShortDateString();

            cmd2d.Connection.Open();
            decimal collecToday = Convert.ToDecimal(cmd2d.ExecuteScalar());
            cmd2d.Connection.Close();
            cmd2d.Connection.Dispose();

            //get cells credit limit
            SqlCommand cmdq = new SqlCommand("Select Isnull(Sum(CreditLimit),0) from Branches where Type='Branch'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmdq.Connection.Open();
            //decimal cLimit = Convert.ToDecimal(cmdq.ExecuteScalar());
            cmdq.Connection.Close();
            cmdq.Connection.Dispose();
            //decimal limitBalance = cLimit - cBalance;

            //lblBalance.Text = cBalance.ToString() + " Tk.";
            //lblCollection.Text = collecToday.ToString() + " Tk.";
            //lblPending.Text = pending.ToString() + " Tk.";
            //lblLimit.Text = cLimit.ToString() + " Tk.";
            //lblRemain.Text = limitBalance.ToString() + " Tk.";

            txtDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            BtnCheck();

            MaturedBill("O404O");

            //DepProcessTime
            Monthlyprocess();
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Convert.ToDateTime(txtDate.Text) >= DateTime.Today)
        {
            string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
            RunQuery.SQLQuery.ExecNonQry("Insert into Tasks (TaskName, TaskDetails, DeadLine, Priority, Status, EntryBy) VALUES ('Admin', '" + txtDetail.Text + "', '" + dt + "', '" + ddType.SelectedValue + "', 'Pending', '" + Page.User.Identity.Name.ToString() + "')");
            GridView2.DataBind();
        }
        else
        {
            lblMsg.Text = "Schedule Date Must Be Greater Then Today.";
        }
    }
    protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView2.SelectedIndex);

            Label CrID = GridView2.Rows[index].FindControl("Label1") as Label;
            RunQuery.SQLQuery.ReturnString("Update Tasks set Status='Done' where tid= " + CrID.Text);
            GridView2.DataBind();
        }
        catch (Exception ex)
        {

        }
    }
    protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView2.Rows[index].FindControl("Label1") as Label;
            string OrderID = RunQuery.SQLQuery.ReturnString("Select Status from Tasks where tid='" + lblItemCode.Text + "'");

            if (OrderID == "Pending")
            {
                SqlCommand cmd7 = new SqlCommand("DELETE Tasks WHERE tid=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                cmd7.ExecuteNonQuery();
                cmd7.Connection.Close();

                GridView2.DataBind();
                uPanel.Update();
                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "Task deleted ...";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Pending Task is Locked for delete...";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    private void BtnCheck()
    {
        //Requisition
        string blocked = SQLQuery.ReturnString("Select IsBlocked from FormAccessSecurity where UserID='" + User.Identity.Name + "' AND MenuItemID='7' ");
        if (blocked == "0")
        {
            OrderEntry.Visible = false;
        }
        //PO Approval
        blocked = SQLQuery.ReturnString("Select IsBlocked from FormAccessSecurity where UserID='" + User.Identity.Name + "' AND MenuItemID='13' ");
        if (blocked == "0")
        {
            menuID171.Visible = false;
        }
        //Product SetUp
        blocked = SQLQuery.ReturnString("Select IsBlocked from FormAccessSecurity where UserID='" + User.Identity.Name + "' AND MenuItemID='18' ");
        if (blocked == "0")
        {
            menuID167.Visible = false;
        }
        //PO Receive
        blocked = SQLQuery.ReturnString("Select IsBlocked from FormAccessSecurity where UserID='" + User.Identity.Name + "' AND MenuItemID='51' ");
        if (blocked == "0")
        {
            menuID168.Visible = false;
        }

        //Stock Out
        blocked = SQLQuery.ReturnString("Select IsBlocked from FormAccessSecurity where UserID='" + User.Identity.Name + "' AND MenuItemID='54' ");
        if (blocked == "0")
        {
            menuID169.Visible = false;
        }
        //Product Name
        blocked = SQLQuery.ReturnString("Select IsBlocked from FormAccessSecurity where UserID='" + User.Identity.Name + "' AND MenuItemID='83' ");
        if (blocked == "0")
        {
            menuID170.Visible = false;
        }

        //blocked = SQLQuery.ReturnString("Select IsBlocked from FormAccessSecurity where UserID='" + User.Identity.Name + "' AND MenuItemID='79' ");
        //if (blocked == "0")
        //{
        //    menuID172.Visible = false;
        //}

        //blocked = SQLQuery.ReturnString("Select IsBlocked from FormAccessSecurity where UserID='" + User.Identity.Name + "' AND MenuItemID='79' ");
        //if (blocked == "0")
        //{
        //    menuID173.Visible = false;
        //}
        //Daily Attendance
        blocked = SQLQuery.ReturnString("Select IsBlocked from FormAccessSecurity where UserID='" + User.Identity.Name + "' AND MenuItemID='171' ");
        if (blocked == "0")
        {
            menuID174.Visible = false;
        }
        //LC Costing
        blocked = SQLQuery.ReturnString("Select IsBlocked from FormAccessSecurity where UserID='" + User.Identity.Name + "' AND MenuItemID='61' ");
        if (blocked == "0")
        {
            menuID.Visible = false;
        }
        //Voucher Entry
        blocked = SQLQuery.ReturnString("Select IsBlocked from FormAccessSecurity where UserID='" + User.Identity.Name + "' AND MenuItemID='143' ");
        if (blocked == "0")
        {
            menuID175.Visible = false;
        }

        //blocked = SQLQuery.ReturnString("Select IsBlocked from FormAccessSecurity where UserID='" + User.Identity.Name + "' AND MenuItemID='79' ");
        //if (blocked == "0")
        //{
        //    menuID176.Visible = false;
        //}
    }

    private void MaturedBill(string partyId)
    {
        DataTable dt1 = new DataTable();
        DataRow dr1 = null;

        dt1.Columns.Add(new DataColumn("Company", typeof(string)));
        dt1.Columns.Add(new DataColumn("MatuirityDays", typeof(string)));
        dt1.Columns.Add(new DataColumn("InvDate", typeof(string)));
        dt1.Columns.Add(new DataColumn("InvNo", typeof(string)));
        dt1.Columns.Add(new DataColumn("InvoiceTotal", typeof(string)));
        dt1.Columns.Add(new DataColumn("DeliveryDaysCount", typeof(string)));
        dt1.Columns.Add(new DataColumn("MaturedDate", typeof(string)));
        if (partyId == "O404O")
        {
            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT PartyID, Company, Address, Phone, Email, Fax, IM, Website, ContactPerson, MobileNo, MatuirityDays, CreditLimit, OpBalance FROM Party WHERE (Type = 'customer') ORDER BY Company");
            foreach (DataRow drx in dtx.Rows)
            {
                string pid = drx["PartyID"].ToString();
                double mDays = Convert.ToDouble(drx["MatuirityDays"].ToString());
                string lastMaturityDate = DateTime.Today.AddDays(mDays * (-1)).ToString("yyyy-MM-dd");

                DataTable dtx2 = SQLQuery.ReturnDataTable(@"SELECT TOP (200) SaleID, InvNo, InvDate, SalesMode, CustomerID, CustomerName, DeliveryDate, InvoiceTotal FROM Sales WHERE (CustomerID = '" + pid + "') AND (InvDate <='" + lastMaturityDate + "') AND (IsActive=1)");
                foreach (DataRow drx2 in dtx2.Rows)
                {
                    string name = drx["Company"].ToString();
                    string matuirityDays = drx["MatuirityDays"].ToString();
                    string invDate = drx2["InvDate"].ToString();
                    string invNo = drx2["InvNo"].ToString();
                    string invAmount = drx2["InvoiceTotal"].ToString();
                    double invDeliveryDaysCount = Convert.ToDouble(SQLQuery.ReturnString("SELECT DATEDIFF(day, '" + Convert.ToDateTime(drx2["DeliveryDate"]).ToString("yyyy-MM-dd") + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "') FROM Sales WHERE InvNo='" + drx2["InvNo"] + "'"));

                    double matuDates = Convert.ToDouble(Convert.ToDouble(mDays) - Convert.ToDouble(invDeliveryDaysCount));
                    string maturedDate = DateTime.Today.AddDays(matuDates).ToString("dd-MM-yyyy");

                    dr1 = dt1.NewRow();
                    dr1["Company"] = name;
                    dr1["MatuirityDays"] = matuirityDays;
                    dr1["InvDate"] = Convert.ToDateTime(invDate).ToString("dd-MM-yyyy");
                    dr1["InvNo"] = invNo;
                    dr1["InvoiceTotal"] = invAmount;
                    dr1["DeliveryDaysCount"] = invDeliveryDaysCount;
                    dr1["MaturedDate"] = maturedDate;
                    dt1.Rows.Add(dr1);
                }

            }
        }
        else
        {
            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT PartyID, Company, Address, Phone, Email, Fax, IM, Website, ContactPerson, MobileNo, MatuirityDays, CreditLimit, OpBalance FROM Party WHERE PartyID = '" + partyId + "' AND (Type = 'customer') ORDER BY Company");
            foreach (DataRow drx in dtx.Rows)
            {
                string pid = drx["PartyID"].ToString();
                double mDays = Convert.ToDouble(drx["MatuirityDays"].ToString());
                string lastMaturityDate = DateTime.Today.AddDays(mDays * (-1)).ToString("yyyy-MM-dd");


                DataTable dtx2 = SQLQuery.ReturnDataTable(@"SELECT TOP (200) SaleID, InvNo, InvDate, SalesMode, CustomerID, CustomerName, DeliveryDate, InvoiceTotal FROM Sales WHERE (CustomerID = '" + pid + "') AND (InvDate <='" + lastMaturityDate + "') AND (IsActive=1)");
                foreach (DataRow drx2 in dtx2.Rows)
                {
                    string name = drx["Company"].ToString();
                    string matuirityDays = drx["MatuirityDays"].ToString();
                    string invDate = drx2["InvDate"].ToString();
                    string invNo = drx2["InvNo"].ToString();
                    string invAmount = drx2["InvoiceTotal"].ToString();
                    double invDeliveryDaysCount = Convert.ToDouble(SQLQuery.ReturnString("SELECT DATEDIFF(day, '" + Convert.ToDateTime(drx2["DeliveryDate"]).ToString("yyyy-MM-dd") + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "') FROM Sales WHERE InvNo='" + drx2["InvNo"] + "'"));

                    double matuDates = Convert.ToDouble(Convert.ToDouble(mDays) - Convert.ToDouble(invDeliveryDaysCount));
                    string maturedDate = DateTime.Today.AddDays(matuDates).ToString("yyyy-MM-dd");

                    dr1 = dt1.NewRow();
                    dr1["Company"] = name;
                    dr1["MatuirityDays"] = matuirityDays;
                    dr1["InvDate"] = Convert.ToDateTime(invDate).ToString("dd-MM-yyyy");
                    dr1["InvNo"] = invNo;
                    dr1["InvoiceTotal"] = invAmount;
                    dr1["DeliveryDaysCount"] = invDeliveryDaysCount;
                    dr1["MaturedDate"] = maturedDate;
                    dt1.Rows.Add(dr1);
                }

            }

        }

        GridView4.DataSource = dt1;
        GridView4.DataBind();
    }

    protected void GridView4_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView4.PageIndex = e.NewPageIndex;
        MaturedBill(ddParties.SelectedValue);
        GridView4.PageIndex = e.NewPageIndex;
    }


    private decimal total = (decimal)0.0;
    protected void GridView4_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField deliveryDaysCountHiddenField = e.Row.FindControl("deliveryDaysCountHiddenField") as HiddenField;
            HiddenField matuirityDaysHiddenField = e.Row.FindControl("matuirityDaysHiddenField") as HiddenField;
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "InvoiceTotal"));
            if (deliveryDaysCountHiddenField != null && (matuirityDaysHiddenField != null && int.Parse(matuirityDaysHiddenField.Value) < int.Parse(deliveryDaysCountHiddenField.Value)))
            {
                //e.Row.BackColor = Color.Red;
                e.Row.Cells[6].BackColor = Color.Yellow;
            }

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "Invoice Total";
            e.Row.Cells[4].Text = Convert.ToString(total);
            
        }
    }

    protected void ddParties_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        MaturedBill(ddParties.SelectedValue);
        GridView4.DataBind();
    }

    private void Monthlyprocess()
    {
        string isexist = SQLQuery.ReturnString("SELECT DepComplete FROM MonthlyProcess WHERE Year = '" + DateTime.Now.ToString("yyyy")+"' AND Month ='"+ DateTime.Now.ToString("MM")+"'");
        
        if (isexist == "")// DateTime.Now.ToString("yyyy") || isexist2 != DateTime.Now.ToString("MM"))
        {
            SQLQuery.ExecNonQry("INSERT INTO [dbo].[MonthlyProcess] ([Year],[Month],DepComplete) VALUES ('" + DateTime.Now.ToString("yyyy") +"','"+DateTime.Now.ToString("MM")+"','0')");
            
        }
        else if (isexist == "0")
        {
            decimal ttlDr = 0; decimal ttdwv = 0;
           SQLQuery.ExecNonQry("Update [dbo].[MonthlyProcess] SET DepComplete = '1' WHERE [Year] = '" + DateTime.Now.ToString("yyyy") + "' AND [Month]= '" + DateTime.Now.ToString("MM") + "' ");
        
            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT id, Name, Depreciation, DepType, Rate, TotalQty, TotalValue, ItemCode, ProductName FROM vw_FixedAssets");
            foreach (DataRow drx in dtx.Rows)
            {
                string id = drx["id"].ToString();
                string name = drx["ProductName"].ToString();
                string depreciation = drx["Depreciation"].ToString();
                string depType = drx["DepType"].ToString();
                string rate = drx["Rate"].ToString(); //Yearly Rate
                string totalQty = drx["TotalQty"].ToString();
                string totalValue = drx["TotalValue"].ToString();
                string iCode = drx["ItemCode"].ToString();
                string depPercent =
                SQLQuery.ReturnString("SELECT Convert(int, Depreciation) FROM Products Where ProductID ='" + id + "'");
                if (depPercent == "" || depPercent == "0")
                {
                    depPercent = SQLQuery.ReturnString("SELECT Depreciationvalue FROM ItemGroup WHERE GroupSrNo= (Select GroupId from vwProducts WHERE id='" + id + "') ");
                }
                decimal deductAmt = ((Convert.ToDecimal(totalValue)*Convert.ToDecimal(depPercent) /100M))/12M;
                decimal wdvm = (Convert.ToDecimal(totalValue) - deductAmt);
                decimal lifetimeMonthsQty = (100M * 12M) / Convert.ToDecimal(depPercent);
                decimal monthAmount = Convert.ToDecimal(totalValue) / lifetimeMonthsQty;
                string ename = Page.User.Identity.Name.ToString();
                string productnam = SQLQuery.ReturnString("SELECT  ProductName  FROM    FixedAssets WHERE ProductID ='" + id + "'");

                string loanAcHeadId = SQLQuery.ReturnString(@"SELECT AccountHead FROM Products WHERE ProductID='" + id + "'");
                
                string invno2 = SQLQuery.ReturnString("SELECT  LCNoFTT FROM    FixedAssets WHERE ItemCode ='" + iCode + "'");
                string depid = SQLQuery.ReturnString("SELECT DepreciationHeadId  FROM FixedAssets Where ItemCode='"+ iCode + "'");

                string loanDescriptions = "'"+Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd")+"' Monthly depreciation amount deducted for fixed assets  Lc/Inv no:'" + invno2 + "'";

                SQLQuery.ExecNonQry("INSERT INTO [dbo].[Depreciation]([FixedAssetId],[DepDate],[DepreciationAmount],[ItemCode]) VALUES ('" + id +
                    "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + deductAmt + "','"+ name + "')");

                Accounting.VoucherEntry.AutoVoucherEntry("15", loanDescriptions, depid, loanAcHeadId,  Convert.ToDecimal(deductAmt), invno2, "7", ename, DateTime.Now.ToString("yyyy-MM-dd"), "1");

                string dupinv = SQLQuery.ReturnString("Select VoucherReferenceNo from VoucherMaster Where VoucherReferenceNo ='"+ invno2 + "FixedAssetsWDV'");
                
               //     SQLQuery.ExecNonQry("Delete VoucherMaster Where VoucherReferenceNo ='" + dupinv + "'");
                

                string txtPurchaseDate = SQLQuery.ReturnString("SELECT  InDate FROM    FixedAssets WHERE ItemCode ='" + iCode + "'");
                DateTime past = Convert.ToDateTime(txtPurchaseDate);

                decimal deductAmtPerMonth = SQLQuery.GetDeprAmt(id, Convert.ToDecimal(totalValue), past);

               // Accounting.VoucherEntry.AutoVoucherEntry("15", loanDescriptions, depid, loanAcHeadId, deductAmtPerMonth, invno2, "7", ename, DateTime.Now.ToString("yyyy-MM-dd"), "1");
                ttlDr += deductAmt ;
                ttdwv += Convert.ToDecimal(totalValue);
            }

            string pid = dtx.Rows[0]["id"].ToString();
            //string pname = dtx.Rows[0]["Name"].ToString();
            string lName = Page.User.Identity.Name.ToString();
            //string productname = SQLQuery.ReturnString("SELECT  ProductName  FROM    FixedAssets WHERE ProductID ='" + pid + "'");

            //string loanAccHeadId = SQLQuery.ReturnString(@"SELECT AccountHead FROM Products WHERE ProductID='" + pid + "'");
            string loanDescription = " Accumulated depreciation for Fixed Assets";
            string invno = SQLQuery.ReturnString("SELECT  LCNoFTT FROM    FixedAssets WHERE ProductID ='" + pid + "'");
            decimal wdv = (ttdwv - ttlDr);
           // Accounting.VoucherEntry.AutoVoucherEntry("15", "loanDescription", "040202610", "010207001",wdv, invno, "7", lName, DateTime.Now.ToString("yyyy-MM-dd"), "1");
            SQLQuery.ExecNonQry("Update [dbo].[MonthlyProcess] SET DepComplete = '2' WHERE [Year] = '" + DateTime.Now.ToString("yyyy") + "' AND [Month]= '" + DateTime.Now.ToString("MM") + "' ");


        }
    }
}
