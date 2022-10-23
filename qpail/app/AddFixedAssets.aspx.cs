using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml;
using ClosedXML.Excel;
using RunQuery;



public partial class Fixedassets : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnAdd.Attributes.Add("onclick",
            " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDate.Text = DateTime.Now.ToShortDateString();
            txtPurchaseDate.Text = DateTime.Now.ToShortDateString();

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            ddGroup.DataBind();
            ddSubGroup.DataBind();
            ddGrade.DataBind();
            ddCategory.DataBind();
            ddProduct.DataBind();

            //ddGodown.DataBind();
            //ddLocation.DataBind();

            QtyinFixedAssets();
            BindItemGrid();
            InvIDNo();
            DropDownList2.Visible = false;



        }
        //txtInv.Text = InvIDNo();
    }

    public string InvIDNo()
    {
        string lName = Page.User.Identity.Name.ToString();
        string yr = Convert.ToDateTime(txtPurchaseDate.Text).Year.ToString(); // DateTime.Now.Year.ToString();
        DateTime countForYear = Convert.ToDateTime(txtPurchaseDate.Text);

        SqlCommand cmd =
            new SqlCommand(
                "Select CONVERT(varchar, (ISNULL(COUNT(id),0)+ 1 )) from FixedAssets where EntryDate>=@EntryDate",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryDate", countForYear);
        cmd.Connection.Open();
        string InvNo = Convert.ToString(cmd.ExecuteScalar());
        while (InvNo.Length < 4)
        {
            InvNo = "0" + InvNo;
        }
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        InvNo = yr + "" + InvNo;



        string isExist = SQLQuery.ReturnString("Select ItemCode from FixedAssets where ItemCode='" + InvNo + "'  ");
        int i = 0;
        while (isExist != "")
        {
            i++;
            InvNo =
                SQLQuery.ReturnString("Select CONVERT(varchar, (ISNULL (COUNT(id),0)+ " + i +
                                      " )) from FixedAssets where EntryDate>='" + countForYear.ToString("yyyy-MM-dd") +
                                      "' AND  EntryDate<='" + countForYear.ToString("yyyy-12-31") + "'  ");
            while (InvNo.Length < 4)
            {
                InvNo = "0" + InvNo;
            }

            InvNo = yr + "" + InvNo;
            isExist = SQLQuery.ReturnString("Select ItemCode from FixedAssets where ItemCode='" + InvNo + "'  ");
        }

        txtItemCode.Text = InvNo;
        return InvNo;

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Visible)
            {

            }
            
            if (btnAdd.Text == "Add" && txtAmountBDT.Text != "")
            {
                SQLQuery.Empty2Zero(txtQty);
                InsertData();
                //if (ddParties.SelectedValue != "")
                //{
                //autovoucher();
                //}
                ClearForm();
                InvIDNo();
                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Item Added successfully ...";

            }
            else
            {
                // SQLQuery.ExecNonQry("Delete FixedAssets where (id ='" + lblOrderID.Text + "')");
                //InsertData();
                //ClearForm();
               // updatedata();
                lblMsg2.Attributes.Add("class", "xerp_warning");
                lblMsg2.Text = "Please fill in all the required fields";
            }
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "ERROR: " + ex.ToString();
        }
        finally
        {
            BindItemGrid();
        }
    }

    private void ClearForm()
    {
        btnAdd.Text = "Add";
        //txtPurchaseDate.Text = "";
        txtItemCode.Text = "";
        txtModel.Text = "";
        txtSpec.Text = "";
        txtWarranty.Text = "";
        txtQty.Text = "0";
        txtRateBDT.Text = "0";
    }

    private void InsertData()
    {
        if (cbAuto.Checked)
        {
            int i = Convert.ToInt32(txtQty.Text);
            while (i > 0)
            {
                string lName = Page.User.Identity.Name.ToString();
                string productName = ddProduct.SelectedItem.Text;

                int balance = Convert.ToInt32(txtQty.Text) - Convert.ToInt32(ltrCQty.Text);

                txtItemCode.Text = InvIDNo();
                
                SqlCommand cmd2 =
                    new SqlCommand(
                        "INSERT INTO FixedAssets (FixedAssetsType, OrderID, SizeId, ProductID, ProductName, BrandID, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy, CompanyFor,  QtyBalance, DeliveredQty,PurchaseCost,CurrentValue,Rate,ItemCode, InDate,ManufacturerName,LCNoFTT,Location,UnitPriceUSD,TotalValueUSD,PurchaseSource, DepreciationHeadId, LinkedAccHeadId, PaymentAccHeadId) VALUES ('FixedAssets', @OrderID, @SizeId, @ProductID, @ProductName, @BrandID, @UnitCost, @Quantity, @UnitWeight, '" +
                        ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, @CompanyFor, '" + balance + "', '" +
                        ltrCQty.Text + "' ,'" + txtRateBDT.Text + "','" + txtCurrentValue.Text + "','" +
                        txtAmountBDT.Text + "','" + txtItemCode.Text + "','" +
                        Convert.ToDateTime(txtPurchaseDate.Text).ToString("yyyy-MM-dd") + "','" + txtManufacture.Text +
                        "','" + txtLCNo.Text + "','" + txtLocation.Text + "', @UnitPriceUSD,'" + txtTotalUSD.Text + "',@PurchaseSource, @DepreciationHeadId, @LinkedAccHeadId, @PaymentAccHeadId)",
                        new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.AddWithValue("@OrderID", "");
                cmd2.Parameters.AddWithValue("@ProductID", ddProduct.SelectedValue);

                cmd2.Parameters.AddWithValue("@SizeId", "0");
                //cmd2.Parameters.AddWithValue("@ItemCode", txtItemCode.Text);
                cmd2.Parameters.AddWithValue("@ProductName", txtSpec.Text);
                cmd2.Parameters.AddWithValue("@BrandID", txtModel.Text);
                cmd2.Parameters.AddWithValue("@CompanyFor", txtWarranty.Text);

                cmd2.Parameters.AddWithValue("@UnitCost", txtRateBDT.Text);
                cmd2.Parameters.AddWithValue("@Quantity", "1"); // Convert.ToDecimal(txtQty.Text));
                cmd2.Parameters.AddWithValue("@UnitWeight", "0"); // Price

                cmd2.Parameters.AddWithValue("@ItemTotal", "0");
                cmd2.Parameters.AddWithValue("@TotalWeight", "0");
                cmd2.Parameters.AddWithValue("@EntryBy", lName);
                cmd2.Parameters.AddWithValue("@UnitPriceUSD", txtRateUSD.Text);
                cmd2.Parameters.AddWithValue("@PurchaseSource", ddSource.SelectedItem.Text);
                cmd2.Parameters.AddWithValue("@DepreciationHeadId", DropDownList4.SelectedValue);
                cmd2.Parameters.AddWithValue("@LinkedAccHeadId", ddSource.SelectedItem.Text);
                cmd2.Parameters.AddWithValue("@PaymentAccHeadId", DropDownList1.SelectedValue);
                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                


                string id = ddProduct.SelectedValue;
                string name = txtSpec.Text;
                string totalValue = txtAmountBDT.Text;
                string iCode = txtItemCode.Text;
                string depPercent =
                SQLQuery.ReturnString("SELECT Convert(int, Depreciation) FROM Products Where ProductID ='" + id + "'");
                if (depPercent == "" || depPercent == "0")
                {
                    depPercent = SQLQuery.ReturnString("SELECT Depreciationvalue FROM ItemGroup WHERE GroupSrNo= (Select GroupId from vwProducts WHERE id='" + id + "') ");
                }
                decimal deductAmt = ((Convert.ToDecimal(totalValue) * Convert.ToDecimal(depPercent) / 100M)) / 12M;
                decimal wdvm = (Convert.ToDecimal(totalValue) - deductAmt);
                decimal lifetimeMonthsQty = (100M * 12M) / Convert.ToDecimal(depPercent);
                decimal monthAmount = Convert.ToDecimal(totalValue) / lifetimeMonthsQty;
                string ename = Page.User.Identity.Name.ToString();
                string productnam = SQLQuery.ReturnString("SELECT  ProductName  FROM    FixedAssets WHERE ProductID ='" + id + "'");

                string loanAcHeadId = SQLQuery.ReturnString(@"SELECT AccountHead FROM Products WHERE ProductID='" + id + "'");

                string invno2 = SQLQuery.ReturnString("SELECT  LCNoFTT FROM    FixedAssets WHERE ItemCode ='" + iCode + "'");

                
                string drhead = DropDownList4.SelectedValue;
                string crhead = DropDownList1.SelectedValue;
                DateTime pDate = Convert.ToDateTime(this.txtPurchaseDate.Text);
                DateTime tillDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01")).AddDays(-1);
                string headname = SQLQuery.ReturnString("SELECT AccountsHeadName from HeadSetup where AccountsHeadID='" + loanAcHeadId + "'");


                string loanDescriptions2 = "Paid to "+ DropDownList1.SelectedItem.Text + " for  " + txtSpec.Text + " for " + headname  + " Lc/Inv no: " + invno2 + "'";
                Accounting.VoucherEntry.AutoVoucherEntry("15", loanDescriptions2, loanAcHeadId, crhead, Convert.ToDecimal(totalValue), invno2, "7", ename, pDate.ToString("yyyy-MM-dd"), "1");

                while (pDate <= tillDate)
                {
                    pDate = pDate.AddMonths(1);

                    SQLQuery.ExecNonQry(
                            "INSERT INTO [dbo].[Depreciation]([FixedAssetId],[DepDate],[DepreciationAmount],[ItemCode]) VALUES ('" +
                            id +
                            "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + deductAmt + "','" + name + "')");

                    string loanDescriptions = "'" + pDate.ToString("yyyy-MM-dd") + "' Monthly depreciation amount deducted for fixed assets  Lc/Inv no:'" + invno2 + "'";

                    Accounting.VoucherEntry.AutoVoucherEntry("15", loanDescriptions, drhead, loanAcHeadId,
                        Convert.ToDecimal(deductAmt), invno2, "7", ename, pDate.ToString("yyyy-MM-dd"), "1");

                    string dupinv =
                        SQLQuery.ReturnString("Select VoucherReferenceNo from VoucherMaster Where VoucherReferenceNo ='" +
                                              invno2 + "FixedAssetsWDV'");

                    //     SQLQuery.ExecNonQry("Delete VoucherMaster Where VoucherReferenceNo ='" + dupinv + "'");



                    DateTime past = Convert.ToDateTime(txtPurchaseDate.Text);

                    decimal deductAmtPerMonth = SQLQuery.GetDeprAmt(id, Convert.ToDecimal(totalValue), pDate);

                   // Accounting.VoucherEntry.AutoVoucherEntry("15", loanDescriptions, drhead, loanAcHeadId,
                     //   deductAmtPerMonth, invno2, "7", ename, DateTime.Now.ToString("yyyy-MM-dd"), "1");

                }


                cmd2.Connection.Close();
                i--;
            }
        }
        else
        {
            int i = Convert.ToInt32(txtQty.Text);
            while (i > 0)
            {
                string lName = Page.User.Identity.Name.ToString();
                string productName = ddProduct.SelectedItem.Text;

                int balance = Convert.ToInt32(txtQty.Text) - Convert.ToInt32(ltrCQty.Text);

                txtItemCode.Text = InvIDNo();


                SqlCommand cmd2 =
                    new SqlCommand(
                        "INSERT INTO FixedAssets (FixedAssetsType, OrderID, SizeId, ProductID, ProductName, BrandID, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy, CompanyFor,  QtyBalance, DeliveredQty,PurchaseCost,CurrentValue,Rate,ItemCode, InDate, UnitPriceUSD,TotalValueUSD,PurchaseSource, DepreciationHeadId, LinkedAccHeadId, PaymentAccHeadId) VALUES ('FixedAssets', @OrderID, @SizeId, @ProductID, @ProductName, @BrandID, @UnitCost, @Quantity, @UnitWeight, '" +
                        ltrUnit.Text + "', @ItemTotal, @TotalWeight, @EntryBy, @CompanyFor, '" + balance + "', '" +
                        ltrCQty.Text + "' ,'" + txtRateBDT.Text + "','" + txtCurrentValue.Text + "','" +
                        txtAmountBDT.Text + "','" + txtItemCode.Text + "','" +
                        Convert.ToDateTime(txtPurchaseDate.Text).ToString("yyyy-MM-dd") +
                        "',@UnitPriceUSD, @TotalValueUSD),@PurchaseSource,@DepreciationHeadId,@LinkedAccHeadId,@PaymentAccHeadId",
                        new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.AddWithValue("@OrderID", "");
                cmd2.Parameters.AddWithValue("@ProductID", ddProduct.SelectedValue);

                cmd2.Parameters.AddWithValue("@SizeId", "0");
                //cmd2.Parameters.AddWithValue("@ItemCode", txtItemCode.Text);
                cmd2.Parameters.AddWithValue("@ProductName", txtSpec.Text);
                cmd2.Parameters.AddWithValue("@BrandID", txtModel.Text);
                cmd2.Parameters.AddWithValue("@CompanyFor", txtWarranty.Text);

                cmd2.Parameters.AddWithValue("@UnitCost", txtRateBDT.Text);
                cmd2.Parameters.AddWithValue("@Quantity", "1"); // Convert.ToDecimal(txtQty.Text));
                cmd2.Parameters.AddWithValue("@UnitWeight", "0"); // Price

                cmd2.Parameters.AddWithValue("@ItemTotal", "0");
                cmd2.Parameters.AddWithValue("@TotalWeight", "0");
                cmd2.Parameters.AddWithValue("@EntryBy", lName);
                cmd2.Parameters.AddWithValue("@TotalValueUSD", txtTotalUSD.Text);
                cmd2.Parameters.AddWithValue("@UnitPriceUSD", txtRateUSD.Text);
                cmd2.Parameters.AddWithValue("@PurchaseSource", ddSource.SelectedItem.Text);
                cmd2.Parameters.AddWithValue("@DepreciationHeadId", DropDownList3.SelectedValue);
                cmd2.Parameters.AddWithValue("@LinkedAccHeadId", ddSource.SelectedItem.Text);
                cmd2.Parameters.AddWithValue("@PaymentAccHeadId", ddlchead.SelectedValue);
                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                


                string id = ddProduct.SelectedValue;
                string name = txtSpec.Text;
                string totalValue = txtAmountBDT.Text;
                string iCode = txtItemCode.Text;
                string depPercent =
                SQLQuery.ReturnString("SELECT Convert(int, Depreciation) FROM Products Where ProductID ='" + id + "'");
                if (depPercent == "" || depPercent == "0")
                {
                    depPercent = SQLQuery.ReturnString("SELECT Depreciationvalue FROM ItemGroup WHERE GroupSrNo= (Select GroupId from vwProducts WHERE id='" + id + "') ");
                }
                decimal deductAmt = ((Convert.ToDecimal(totalValue) * Convert.ToDecimal(depPercent) / 100M)) / 12M;
                decimal wdvm = (Convert.ToDecimal(totalValue) - deductAmt);
                decimal lifetimeMonthsQty = (100M * 12M) / Convert.ToDecimal(depPercent);
                decimal monthAmount = Convert.ToDecimal(totalValue) / lifetimeMonthsQty;
                string ename = Page.User.Identity.Name.ToString();
                string productnam = SQLQuery.ReturnString("SELECT  ProductName  FROM    FixedAssets WHERE ProductID ='" + id + "'");

                string loanAcHeadId = SQLQuery.ReturnString(@"SELECT AccountHead FROM Products WHERE ProductID='" + id + "'");

                string invno2 = SQLQuery.ReturnString("SELECT  LCNoFTT FROM    FixedAssets WHERE ItemCode ='" + iCode + "'");
                string headname = SQLQuery.ReturnString("SELECT AccountsHeadName from HeadSetup where AccountsHeadID='"+ loanAcHeadId + "'");

                string loanDescriptions2 = "Paid to '"+ ddlchead.SelectedItem.Text + " for "+ txtSpec.Text + " for "+ headname + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd")+"'Lc/Inv no:'" + invno2 + "'";

                DateTime pDate = Convert.ToDateTime(this.txtPurchaseDate.Text);
                DateTime tillDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01")).AddDays(-1);
                string drheadlc = ddlchead.SelectedValue;
                string crheadlc = DropDownList3.SelectedValue;

                Accounting.VoucherEntry.AutoVoucherEntry("15", loanDescriptions2, loanAcHeadId, drheadlc, Convert.ToDecimal(totalValue), invno2, "7", ename, pDate.ToString("yyyy-MM-dd"), "1");

                while (pDate <= tillDate)
                {

                    pDate = pDate.AddMonths(1);

                    SQLQuery.ExecNonQry(
                            "INSERT INTO [dbo].[Depreciation]([FixedAssetId],[DepDate],[DepreciationAmount],[ItemCode]) VALUES ('" +
                            id +
                            "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + deductAmt + "','" + name + "')");
                    string loanDescriptions = "'" + pDate.ToString("yyyy-MM-dd") + "' Monthly depreciation amount deducted for fixed assets  Lc/Inv no:'" + invno2 + "'";

                    Accounting.VoucherEntry.AutoVoucherEntry("15", loanDescriptions, drheadlc, loanAcHeadId,
                        Convert.ToDecimal(deductAmt), invno2, "7", ename, pDate.ToString("yyyy-MM-dd"), "1");

                    string dupinv =
                        SQLQuery.ReturnString("Select VoucherReferenceNo from VoucherMaster Where VoucherReferenceNo ='" +
                                              invno2 + "FixedAssetsWDV'");

                    //     SQLQuery.ExecNonQry("Delete VoucherMaster Where VoucherReferenceNo ='" + dupinv + "'");



                    DateTime past = Convert.ToDateTime(txtPurchaseDate.Text);

                    decimal deductAmtPerMonth = SQLQuery.GetDeprAmt(id, Convert.ToDecimal(totalValue), pDate);

                  //  Accounting.VoucherEntry.AutoVoucherEntry("15", loanDescriptions, "040202610", "010207001",
                   //     deductAmtPerMonth, invno2, "7", ename, DateTime.Now.ToString("yyyy-MM-dd"), "1");

                }






                cmd2.Connection.Close();
                i--;
            }

        }

    }

    private void BindItemGrid()
    {
        try
        {

            string lName = Page.User.Identity.Name.ToString();
            DataSet ds = SQLQuery.ReturnDataSet(@"SELECT Id, 
            (SELECT  [ItemName] FROM [Products] WHERE [ProductID]= FixedAssets.ProductID) AS ProductID, 
            ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, QtyBalance, DeliveredQty, UnitCost, 
            (SELECT  [Company] FROM [Party] WHERE [PartyID]= FixedAssets.Customer) AS Customer, BrandID, SizeId,CompanyFor,
            (SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=FixedAssets.Color) AS Color, CurrentValue, PurchaseCost, ItemCode,InDate
            FROM FixedAssets WHERE FixedAssetsType='FixedAssets' AND (OutDate IS NULL)  ORDER BY Id DESC");

            //cmd.CommandText = "SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal FROM FixedAssets WHERE EntryBy=@EntryBy AND OrderID='' ORDER BY Id";


            ItemGrid2.EmptyDataText = "No items to view...";
            ItemGrid2.DataSource = ds.Tables[0];
            ItemGrid2.DataBind();

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
            DataTable dt = null;

        }
    }

    //    string lName = Page.User.Identity.Name.ToString();
    //    DataSet ds = SQLQuery.ReturnDataSet(@"SELECT Id, 
    //        (SELECT  [ItemName] FROM [Products] WHERE [ProductID]= FixedAssets.ProductID) AS ProductID, 
    //        ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, QtyBalance, DeliveredQty, UnitCost, 
    //        (SELECT  [Company] FROM [Party] WHERE [PartyID]= FixedAssets.Customer) AS Customer, BrandID, SizeId,CompanyFor,
    //        (SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=FixedAssets.Color) AS Color, CurrentValue, PurchaseCost, ItemCode,InDate
    //        FROM FixedAssets WHERE FixedAssetsType='FixedAssets' AND (OutDate IS NULL)  ORDER BY Id");

    //    //cmd.CommandText = "SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal FROM FixedAssets WHERE EntryBy=@EntryBy AND OrderID='' ORDER BY Id";

    //    ds.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
    //    ds.Connection.Open();
    //    ItemGrid2.EmptyDataText = "No items to view...";
    //    ItemGrid2.DataSource = ds.Tables[0];
    //    ItemGrid2.DataBind();
    //    cmd.Connection.Close();
    //}

    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        //ltrQty.Text = SQLQuery.ReturnString("Select ISNULL(SUM(QtyBalance),0) from FixedAssetsTmp where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "pcs, " +
        //    SQLQuery.ReturnString("Select ISNULL(SUM(UnitWeight),0) from FixedAssetsTmp where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "kg";

    }

    protected void ItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = ItemGrid2.Rows[index].FindControl("lblEntryId") as Label;
            string Invcd = SQLQuery.ReturnString("SELECT [InvNo] FROM [Purchase] WHERE InvNo='InvID'");
            if (Invcd == "")
            {
                SQLQuery.ExecNonQry("DELETE FixedAssets WHERE Id=" + lblItemCode.Text);
            }
            else
            {
                SQLQuery.ExecNonQry("DELETE PurchaseDetails WHERE InvNo= Invcd");
                SQLQuery.ExecNonQry("DELETE FixedAssets WHERE Id=" + lblItemCode.Text);

            }
            //SQLQuery.ExecNonQry("DELETE FixedAssets WHERE Id=" + lblItemCode.Text);
            //SQLQuery.ExecNonQry("DELETE PurchaseDetails WHERE InvNo= Invcd");
            BindItemGrid();
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "Item deleted from order ...";

            btnAdd.Text = "Add";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }

    }

    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (btnSave.Text == "Save")
    //        {
    //            ExecuteInsert();
    //            txtQty.Text = "";

    //            QtyinFixedAssets();
    //            
    //            lblMsg.Attributes.Add("class", "xerp_success");
    //            lblMsg.Text = "FixedAssets adjustment saved successfully...";
    //        }
    //        else
    //        {
    //            btnSave.Text = "Save";
    //            //EditField.Attributes.Add("class", "form-group hidden");
    //            lblMsg.Attributes.Add("class", "xerp_success");
    //            //lblMsg.Text = "Info successfully updated for " + DropDownList1.SelectedItem.Text;
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        lblMsg.Attributes.Add("class", "xerp_error");
    //        lblMsg.Text = "Error: " + ex.ToString();
    //    }
    //    finally
    //    {
    //        BindItemGrid();
    //    }
    //}

    /*private void ExecuteInsert()
    {
        string orderNo = InvIDNo();
        string lName = Page.User.Identity.Name.ToString();

        SqlCommand cmd2 = new SqlCommand("INSERT INTO FixedAssets (OrderID, OrderDate, GodownID, GodownName, LocationID, LocationName, Remarks, TotalQty, EntryBy, ProjectId)" +
                                                    " VALUES (@OrderID, @OrderDate, @GodownID, @GodownName, @LocationID, @LocationName, @Remarks, @TotalQty, @EntryBy, @ProjectId)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@OrderID", orderNo); //ddCategory.SelectedValue);
        cmd2.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        
        cmd2.Parameters.AddWithValue("@Remarks", txtAddress.Text);
        cmd2.Parameters.AddWithValue("@TotalQty", ltrQty.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Parameters.AddWithValue("@ProjectId", lblProject.Text);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        SqlCommand cmd = new SqlCommand("UPDATE FixedAssetsTmp SET OrderID='" + orderNo + "' WHERE  EntryBy=@EntryBy AND OrderID=''  AND FixedAssetsType='FixedAssets' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryBy", lName);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        //Sock-in

        foreach (GridViewRow row in ItemGrid.Rows)
        {
            Label lblEntryId = row.FindControl("lblEntryId") as Label;
            string sizeId = SQLQuery.ReturnString("SELECT SizeID FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");
            string brandID = SQLQuery.ReturnString("SELECT brandID FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");
            string productID = SQLQuery.ReturnString("SELECT productID FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");
            string ProductName = SQLQuery.ReturnString("Select ItemName from Products where ProductID = (SELECT productID FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "'))");

            string spec = SQLQuery.ReturnString("SELECT ProductName FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");
            string itemType = SQLQuery.ReturnString("SELECT ItemType FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");
            string customer = SQLQuery.ReturnString("SELECT Customer FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");
            string color = SQLQuery.ReturnString("SELECT Color FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");
            string CompanyFor = SQLQuery.ReturnString("SELECT CompanyFor FROM FixedAssetsTmp where (Id='" + lblEntryId.Text + "')");


            Label lbliQty = row.FindControl("lbliQty") as Label;
            Label lblUnitWeight = row.FindControl("lblUnitWeight") as Label;
            Label lblQtyBalance = row.FindControl("lblQtyBalance") as Label;

            int inQty = Convert.ToInt32(lbliQty.Text); int outQty = 0;
            decimal unitCost = Convert.ToDecimal(lblUnitWeight.Text); decimal outWeight = 0;
            string type = "FixedAssets-in"; string detail = "FixedAssets Adjustment";
            //string ItemSerialNo = "Purchase Date: " + sizeId + ". Specification: " + spec + ". Model: " + brandID + ". Warranty: " + CompanyFor;

            if (inQty < 0)
            {
                inQty = 0; outQty = Convert.ToInt32(lbliQty.Text) * (-1);
                type = "FixedAssets-out";
            }
            if (unitCost < 0)
            {
                unitCost = 0; outWeight = Convert.ToDecimal(lblUnitWeight.Text) * (-1);
            }
            /*
            //Stock.Inventory.SaveToStock("", orderNo, detail, lblEntryId.Text, "", customer, "", color, "", productID, ProductName, itemType, ddGodown.SelectedValue, ddLocation.SelectedValue, ddGroup.SelectedValue, inQty, outQty, unitCost, 0, outWeight, spec, type, "Adjustment", ddLocation.SelectedItem.Text, lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            SqlCommand cmd3 = new SqlCommand(
                        "INSERT INTO FixedAssets ( EntryID, InvoiceID, EntryType, Purpose, RefNo, ItemGroup, ProductID, ProductName, ItemType, Customer, BrandID, SizeID, Color, Spec, Description, WarehouseID, LocationID, InQuantity, OutQuantity, Price, ItemSerialNo, Remark, Status, FixedAssetsLocation, EntryBy, EntryDate)" +
                        " VALUES (@EntryID, @InvoiceID, @EntryType,'" + Purpose + "', @RefNo, @ItemGroup, @ProductID, @ProductName, @ItemType, @Customer, @ProductID, @ProductName, @ItemType, @WarehouseID, @LocationID, @ItemGroup, @InQuantity, @OutQuantity, " + unitPrice + ", @InWeight, @OutWeight, @ItemSerialNo, @Remark, @Status, @FixedAssetsLocation, @EntryBy, '" + EntryDate + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd3.Parameters.AddWithValue("@EntryID", EntryID);
            cmd3.Parameters.AddWithValue("@InvoiceID", InvoiceID);
            cmd3.Parameters.AddWithValue("@EntryType", EntryType);
            cmd3.Parameters.AddWithValue("@SizeID", Purpose);
            cmd3.Parameters.AddWithValue("@Customer", Customer);
            cmd3.Parameters.AddWithValue("@BrandID", BrandID);
            cmd3.Parameters.AddWithValue("@Color", color);
            cmd3.Parameters.AddWithValue("@ProductID", ProductID);

            cmd3.Parameters.AddWithValue("@ProductName", ProductName);
            cmd3.Parameters.AddWithValue("@ItemType", ItemType);
            cmd3.Parameters.AddWithValue("@WarehouseID", WarehouseID);
            cmd3.Parameters.AddWithValue("@LocationID", LocationID);
            cmd3.Parameters.AddWithValue("@ItemGroup", ItemGroup);

            cmd3.Parameters.AddWithValue("@InQuantity", Convert.ToDecimal(InQuantity));
            cmd3.Parameters.AddWithValue("@OutQuantity", Convert.ToDecimal(OutQuantity));
            cmd3.Parameters.AddWithValue("@InWeight", Convert.ToDecimal(InWeight));
            cmd3.Parameters.AddWithValue("@OutWeight", Convert.ToDecimal(OutWeight));

            cmd3.Parameters.AddWithValue("@ItemSerialNo", ItemSerialNo);
            cmd3.Parameters.AddWithValue("@Remark", Remark);
            cmd3.Parameters.AddWithValue("@Status", Status);
            cmd3.Parameters.AddWithValue("@StockLocation", StockLocation);
            cmd3.Parameters.AddWithValue("@EntryBy", EntryBy);

            cmd3.Connection.Open();
            cmd3.ExecuteNonQuery();
            cmd3.Connection.Close();
        }
    }*/

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand("Delete FixedAssets WHERE EntryBy=@EntryBy AND OrderID='' ",
            new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.AddWithValue("@EntryBy", lName);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();
        Response.Redirect("./Order-Entry.aspx");
    }

    protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddSubGroup.DataBind();
        ddGrade.DataBind();

        ddCategory.DataBind();
        ddProduct.DataBind();
        QtyinFixedAssets();

    }

    protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddGrade.DataBind();
            ddCategory.DataBind();
            ddProduct.DataBind();
            QtyinFixedAssets();

        }
        catch (Exception ex)
        {

            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error: " + ex.ToString();
        }
    }


    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddCategory.DataBind();
        ddProduct.DataBind();
        QtyinFixedAssets();
    }

    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddProduct.DataBind();
        QtyinFixedAssets();
        ddCategory.Focus();
    }

    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinFixedAssets();
        txtQty.Focus();
        // 
    }


    private void QtyinFixedAssets()
    {
        try
        {
            ltrUnit.Text =
                SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddProduct.SelectedValue + "'");

            //txtCurrentQty.Text = Stock.Inventory.NonUsableFixedAssestsQty(ddProduct.SelectedValue, ddGodown.SelectedValue);
            SQLQuery.Empty2Zero(txtCurrentQty);
            ltrCQty.Text = txtCurrentQty.Text;

            //BindItemGrid();
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
        finally
        {
        }

    }

    private void TrackCustomer()
    {

    }

    protected void imgUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        lblMsg.Attributes.Add("class", "xerp_error");
        lblMsg.Text = "File Uploaded";
    }


    protected void ItemGrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //DropDownList1.DataBind();
            int index = Convert.ToInt32(ItemGrid2.SelectedIndex);
            Label lblItemName = ItemGrid2.Rows[index].FindControl("lblEntryId") as Label;
            lblOrderID.Text = lblItemName.Text;
            EditMode(lblItemName.Text);
            btnAdd.Text = "Update";

            lblMsg2.Attributes.Add("class", "xerp_info");
            lblMsg2.Text = "Edit mode activated ...";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    private void EditMode(string entryID)
    {
        SqlCommand cmd =
            new SqlCommand(
                @"SELECT ItemType, ProductID, SizeId, Customer, BrandID, Color, Quantity, UnitCost, GodownID, ProductName, CompanyFor, ItemCode, InDate, PurchaseCost, CurrentValue,ManufacturerName,LCNoFTT, Location, TotalValueUSD, UnitPriceUSD FROM [FixedAssets] WHERE Id='" +
                entryID + "'",
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            string productID = dr[1].ToString();

            ddGroup.SelectedValue = Stock.Inventory.GetItemGroup(productID);
            ddSubGroup.DataBind();
            ddSubGroup.SelectedValue = Stock.Inventory.GetItemSubGroup(productID);
            ddGrade.DataBind();
            ddGrade.SelectedValue = Stock.Inventory.GetItemGrade(productID);
            ddCategory.DataBind();
            ddCategory.SelectedValue = Stock.Inventory.GetItemCategory(productID);
            ddProduct.DataBind();
            ddProduct.SelectedValue = productID;

            txtAmountBDT.Text = dr["PurchaseCost"].ToString();
            txtCurrentValue.Text = dr["CurrentValue"].ToString();
            txtModel.Text = dr[4].ToString();
            //txtItemCode.Text = dr[10].ToString();
            txtSpec.Text = dr["ProductName"].ToString();
            txtWarranty.Text = dr[10].ToString();
            txtQty.Text = dr[6].ToString();
            txtRateBDT.Text = dr[7].ToString();
            txtPurchaseDate.Text = Convert.ToDateTime(dr["InDate"].ToString()).ToString("dd/MM/yyyy");
            txtManufacture.Text = dr["ManufacturerName"].ToString();
            txtLCNo.Text = dr["LCNoFTT"].ToString();
            txtLocation.Text = dr["Location"].ToString();
            txtTotalUSD.Text = dr["TotalValueUSD"].ToString();
            txtRateUSD.Text = dr["UnitPriceUSD"].ToString();
            //ddGodown.SelectedValue = dr[8].ToString();
        }
        cmd.Connection.Close();

    }

    protected void ddGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        // ddLocation.DataBind();
        //BindItemGrid();

        QtyinFixedAssets();
    }


    protected void cbAuto_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbAuto.Checked)
        {
            txtItemCode.Text = InvIDNo();
            txtQty.Text = "";
            txtItemCode.ReadOnly = true;
            txtQty.ReadOnly = false;
            txtQty.Focus();
        }
        else
        {
            txtQty.Text = "1";
            txtItemCode.Text = "";
            txtQty.ReadOnly = true;
            txtItemCode.ReadOnly = false;
            txtItemCode.Focus();

        }
    }


    protected void ItemGrid2_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ItemGrid2.PageIndex = e.NewPageIndex;
        BindItemGrid();
        ItemGrid2.PageIndex = e.NewPageIndex;

    }

    private void autovoucher()
    {
        // DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT id, Name, Depreciation, DepType, Rate, TotalQty, TotalValue FROM vw_FixedAssets");

        //   string pid = dtx.Rows[0]["id"].ToString();
        //   string pname = dtx.Rows[0]["Name"].ToString();
        string lName = Page.User.Identity.Name.ToString();
        string productname =
            SQLQuery.ReturnString("SELECT  ProductName  FROM    FixedAssets WHERE ProductID ='" +
                                  ddProduct.SelectedValue + "'");

        string prdAccHeadId =
            SQLQuery.ReturnString(@"SELECT AccountHead FROM Products WHERE ProductID='" + ddProduct.SelectedValue + "'");
        string loanDescription = "Transferred "+ txtQty.Text +" "+ddProduct.SelectedItem.Text+" to Fixed Assets for "+ddSource.SelectedValue+" No : " + @txtLCNo.Text;
        string invno =
            SQLQuery.ReturnString("SELECT  LCNoFTT FROM    FixedAssets WHERE ProductID ='" + ddProduct.SelectedValue +
                                  "'");
        if (ddSource.SelectedValue == "Local Purchase")
        {

            Accounting.VoucherEntry.AutoVoucherEntry("15", loanDescription, prdAccHeadId, "040202611",
                Convert.ToDecimal(txtAmountBDT.Text), invno, "7", lName, DateTime.Now.ToString("yyyy-MM-dd"), "1");
        }
        else
        {
            Accounting.VoucherEntry.AutoVoucherEntry("15", loanDescription, prdAccHeadId, ddlchead.SelectedValue,
                Convert.ToDecimal(txtAmountBDT.Text), invno, "7", lName, DateTime.Now.ToString("yyyy-MM-dd"), "1");

        }

        

    }


    //protected void txtCurrentValue_OnTextChanged(object sender, EventArgs e)
    //{
    //    string depamount = SQLQuery.ReturnString("SELECT Depreciationvalue FROM ItemGroup WHERE GroupSrNo ='" + ddGroup.SelectedValue+ "'");
    //    decimal deductAmt = ((Convert.ToDecimal(txtAmountBDT.Text) * Convert.ToDecimal(depamount) / 100M)) / 12M;
    //    DateTime date = Convert.ToDateTime(txtPurchaseDate.ToString());
    //    double daysGone = (DateTime.Now - date).TotalDays;
    //    decimal yearGone = Convert.ToDecimal(Math.Round(daysGone / 365.25, 0));
    //    decimal accmdep = ((Convert.ToDecimal(txtAmountBDT.Text) * Convert.ToDecimal(depamount) / 100M) * yearGone);

    //   // txtCurrentValue.Text = Convert.ToString(deductAmt);

    //}

    protected void ddSource_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddSource.SelectedValue == "Local Purchase")
        {
            pnlUSD.Visible = false;
            PanelBDT.Visible = true;
            
        }
        else
        {
            pnlUSD.Visible = true;
            PanelBDT.Visible = false;
        }
    }

    protected void txtCurrentValue_OnTextChanged(object sender, EventArgs e)
    {

    }

    protected void txtRateBDT_OnTextChanged(object sender, EventArgs e)
    {
        
        double totalvalue = Convert.ToDouble(txtRateBDT.Text)*Convert.ToDouble(txtQty.Text);
        txtAmountBDT.Text = Convert.ToString(totalvalue);
        
        decimal deductAmtPerMonth = 0;
        if (txtAmountBDT.Text != "" && txtPurchaseDate.Text!="")
        {
            DateTime past = Convert.ToDateTime(txtPurchaseDate.Text);

            deductAmtPerMonth = SQLQuery.GetDeprAmt(ddProduct.SelectedValue, Convert.ToDecimal(txtRateBDT.Text), past);
        }
        txtCurrentValue.Text = Convert.ToString(deductAmtPerMonth);


        string depPercent =
            SQLQuery.ReturnString("SELECT Convert(int, Depreciation) FROM Products Where ProductID ='" + ddProduct.SelectedValue + "'");
        if (depPercent == "" || depPercent == "0")
        {
            depPercent = SQLQuery.ReturnString("SELECT Depreciationvalue FROM ItemGroup WHERE GroupSrNo= (Select GroupId from vwProducts WHERE id='" + ddProduct.SelectedValue + "') ");
        }

        ltrDepRate.Text ="Rate"+ depPercent + "%";

    }

    private int GetMonthDifference(DateTime now, DateTime past)
    {
        
        int monthsApart = 12 * (now.Year - past.Year) + now.Month - past.Month;
        return Math.Abs(monthsApart);
    }

    protected void txtRateUSD_OnTextChanged(object sender, EventArgs e)
    {
        double totalusd = Convert.ToDouble(txtQty.Text)*Convert.ToDouble(txtRateUSD.Text);
        txtTotalUSD.Text = Convert.ToString(totalusd);
    }

    protected void txtQty_OnTextChanged(object sender, EventArgs e)
    {

        double totalvalue = Convert.ToDouble(txtRateBDT.Text)*Convert.ToDouble(txtQty.Text);
        txtAmountBDT.Text = Convert.ToString(totalvalue);

        double totalusd = Convert.ToDouble(txtQty.Text)*Convert.ToDouble(txtRateUSD.Text);
        txtTotalUSD.Text = Convert.ToString(totalusd);

    }
    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    //private void updatedata()
    //{
    //    if (btnAdd.Text == "Update")
    //    {
    //        RunQuery.SQLQuery.ExecNonQry("UPDATE [dbo].[FixedAssets] set ProductName='" + txtSpec.Text + "', PurchaseCost='" + txtRateBDT.Text + "', CurrentValue='" + txtCurrentValue.Text + "',Rate='" + txtAmountBDT.Text + "', ItemCode='" + ddGroup.SelectedValue + "', InDate='" + Convert.ToDateTime(txtPurchaseDate.Text).ToString("yyyy-MM-dd") + "', UnitPriceUSD='" + txtTotalUSD.Text + "', TotalValueUSD='" + txtRateUSD.Text + "'  where ProductID='" + ddProduct.SelectedValue + "'");

    //        Notify("Fixed Assets Stocks Register was updated Successfully.", "success", lblMsg);
    //        ClearForm();
    //    }
    //}
    protected void txtPurchaseDate_OnTextChanged(object sender, EventArgs e)
    {

        decimal deductAmtPerMonth = 0;
        if (txtAmountBDT.Text != "" && txtPurchaseDate.Text != "")
        {
        double totalvalue = Convert.ToDouble(txtRateBDT.Text) * Convert.ToDouble(txtQty.Text);
        txtAmountBDT.Text = Convert.ToString(totalvalue);

        double totalusd = Convert.ToDouble(txtQty.Text) * Convert.ToDouble(txtRateUSD.Text);
        txtTotalUSD.Text = Convert.ToString(totalusd);

        
            DateTime past = Convert.ToDateTime(txtPurchaseDate.Text);

            deductAmtPerMonth = SQLQuery.GetDeprAmt(ddProduct.SelectedValue, Convert.ToDecimal(txtRateBDT.Text), past);
        }
        txtCurrentValue.Text = Convert.ToString(deductAmtPerMonth);


        string depPercent =
            SQLQuery.ReturnString("SELECT Convert(int, Depreciation) FROM Products Where ProductID ='" + ddProduct.SelectedValue + "'");
        if (depPercent == "" || depPercent == "0")
        {
            depPercent = SQLQuery.ReturnString("SELECT Depreciationvalue FROM ItemGroup WHERE GroupSrNo= (Select GroupId from vwProducts WHERE id='" + ddProduct.SelectedValue + "') ");
        }

        ltrDepRate.Text = depPercent + "%";
    }

    //protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (DropDownList1.SelectedValue == "Cash at Bank")
    //    {
    //       // DropDownList12.selectedvalue = SQLQuery.ReturnString("SELECT     BankName FROM   Banks");
    //        PanelBDT.Visible = true;
    //    }
    //    else
    //    {
    //        pnlUSD.Visible = true;
    //        PanelBDT.Visible = false;
    //    }
    //}

    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DropDownList1.SelectedItem.Text == "Cash at Bank")
        {
            DropDownList2.Visible = false;

        }
        else
        {
            DropDownList2.Visible = false;
        }
    }
}