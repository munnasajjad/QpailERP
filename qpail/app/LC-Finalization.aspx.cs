using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using RunQuery;
using Stock;

public partial class app_LC_Finalization : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        ddName.DataBind();
        LoadLCData();
        if (ddName.Items.Count > 0)
        {
            ltrLC.Text = ddName.SelectedItem.Text;
        }

        ItemGrid.DataBind();
    }

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    protected void ddName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadLCData();
        ltrLC.Text = ddName.SelectedItem.Text;
        ItemGrid.DataBind();
    }


    private void LoadLCData()
    {
        try
        {
            SqlCommand cmd7 = new SqlCommand("Select LCNo, OpenDate, Category, LCType, HSCode, LcRef, LcFor, ShipDate, ExpiryDate, SupplierID, Origin, AgentID, InsuranceID, CnfID, BankId, LcMargin, LTR, BankExcRate, CustomExcRate, QtyUnit, TotalQty, Freight, CfrUSD, CfrBDT, Remarks, TransportMode, LCCloseBy, LCClosedate, BankBDT, EntryBy FROM [LC] WHERE sl=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Parameters.Add("@sl", SqlDbType.VarChar).Value = ddName.SelectedValue;
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                //btnSave.Text = "Update";
                //lblLCNo.Text = dr[0].ToString();
                lblOpDate.Text = dr[1].ToString();// Convert.ToDateTime(dr[1].ToString()).ToShortDateString();
                //lblGrp.Text = SQLQuery.ReturnString("Select GroupName FROM ItemGroup where GroupSrNo = " + dr[2].ToString());
                lblLCType.Text = dr[3].ToString();
                //txtHSCode.Text = dr[4].ToString();
                //txtReferrence.Text = dr[5].ToString();
                lblDept.Text = dr[6].ToString();
                lblShipDate.Text = Convert.ToDateTime(dr[7].ToString()).ToShortDateString();
                lblExDate.Text = Convert.ToDateTime(dr[8].ToString()).ToShortDateString();
                lblSupplier.Text = SQLQuery.ReturnString("Select Company FROM Party where PartyID = " + dr[9].ToString());
                lblCountry.Text = dr[10].ToString();
                lblagent.Text = SQLQuery.ReturnString("Select Company FROM Party where PartyID = " + dr[11].ToString());
                txtInsurance.Text = SQLQuery.ReturnString("Select BankName FROM Banks where BankId = " + dr[12].ToString());
                txtCNF.Text = SQLQuery.ReturnString("Select Company FROM Party where PartyID = " + dr[13].ToString());
                //lblBank.Text = SQLQuery.ReturnString("Select BankName FROM Banks where BankId = " + dr[14].ToString());
                string margin = dr[15].ToString();
                string ltr = dr[16].ToString();
                string bExRate = dr[17].ToString();
                //txtCExRate.Text = dr[18].ToString();
                //txtCustomExRate.Text = dr[18].ToString();

                //txtQty.Text = dr[19].ToString();
                //txtTtlQty.Text = dr[20].ToString();
                txtTtlQty.Text = dr[20].ToString();
                txtFreight.Text = dr[21].ToString();
                string cfrUSD = dr[22].ToString();
                //txtCfrBDT.Text = dr[23].ToString();
                txtRemarks.Text = dr[24].ToString();
                ddMode.Text = dr[25].ToString();
                string bankBDT = dr[28].ToString();
                //btnCancel.Visible = true;
                //ltrSubFrmName.Text = "Edit Party";
                //lblMsg.Attributes.Add("class", "xerp_info");
                //lblMsg.Text = "Info loaded in edit mode.";
                EditField.Attributes.Remove("class");
                EditField.Attributes.Add("class", "control-group");

                //ItemGrid.DataBind();
                //pnl.Update();

                cmd7.Connection.Close();


                RunQuery.SQLQuery.ExecNonQry("UPDATE LC_Bank_Calc SET CFRUSD='" + cfrUSD + "', ExchRate='" + bExRate + "', CFRBDT='" + bankBDT + "', LTR='" + ltr + "', Margin='" + margin + "' WHERE LCNo='" + ddName.SelectedItem.Text + "'");
            }

            cmd7 = new SqlCommand("Select ArrivalDate, DeliveryDate FROM [LC] WHERE sl=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Parameters.Add("@sl", SqlDbType.VarChar).Value = ddName.SelectedValue;
            cmd7.Connection.Open();
            dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                lblArrivalDt.Text = Convert.ToDateTime(dr[0].ToString()).ToShortDateString();
                lblDeliveryDt.Text = Convert.ToDateTime(dr[1].ToString()).ToShortDateString();

                cmd7.Connection.Close();
            }

            //GridView2.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }


    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lcNo = ddName.SelectedItem.Text;
            if (rbClose.Checked)
            {
                if (ddLocation.SelectedValue != "")
                {
                    //Inactivate LC
                    SQLQuery.ExecNonQry("UPDATE LC Set IsActive='D' WHERE sl='" + ddName.SelectedValue + "'");

                    Notify("LC Closed successfully...", "success", lblMsg);
                    string lName = Page.User.Identity.Name.ToString();
                    string isExist = SQLQuery.ReturnString("Select InvoiceID from Stock where InvoiceID='" + lcNo + "' AND Status='LC'");
                    if (isExist == "")
                    {
                        //insert stock qty       
                        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT EntryID, LCNo, Purpose, GradeId, CategoryId, ItemCode, HSCode, ItemSizeID, pcs, NoOfPacks, QntyPerPack, Spec, Thickness, Measurement, qty, 
                                                                UnitPrice, CFRValue, EntryBy, ReturnQty, Loading, Loaded, LandingPercent, LandingAmt, TotalUSD, TotalBDT, UnitCostBDT, FullDescription
                                                                FROM [LcItems] WHERE LCNo='" + lcNo + "'");
                        decimal pcs = 0;
                        foreach (DataRow drx in dtx.Rows)
                        {
                            string EntryID = drx["EntryID"].ToString();
                            string LCNo = drx["LCNo"].ToString();
                            string Purpose = drx["Purpose"].ToString();
                            string GradeId = drx["GradeId"].ToString();
                            string CategoryId = drx["CategoryId"].ToString();
                            string ItemCode = drx["ItemCode"].ToString();
                            string HSCode = drx["HSCode"].ToString();
                            string ItemSizeID = drx["ItemSizeID"].ToString();
                            string noPcs = drx["pcs"].ToString();
                            string NoOfPacks = drx["NoOfPacks"].ToString();
                            string QntyPerPack = drx["QntyPerPack"].ToString();
                            string Spec = drx["Spec"].ToString();
                            string Thickness = drx["Thickness"].ToString();
                            string Measurement = drx["Measurement"].ToString();
                            string qty = drx["qty"].ToString();
                            foreach (GridViewRow gvRow in ItemGrid.Rows)
                            {
                                Label lblItemCode = gvRow.FindControl("lblItemCode") as Label;
                                if (lblItemCode.Text == drx["ItemCode"].ToString())
                                {
                                    TextBox txtReceivedQty = gvRow.FindControl("txtReceivedQty") as TextBox;
                                    if (txtReceivedQty != null) qty = txtReceivedQty.Text;
                                }
                            }
                            string UnitPrice = drx["UnitPrice"].ToString();
                            string CFRValue = drx["CFRValue"].ToString();
                            string EntryBy = drx["EntryBy"].ToString();
                            string ReturnQty = drx["ReturnQty"].ToString();
                            string Loading = drx["Loading"].ToString();
                            string Loaded = drx["Loaded"].ToString();
                            string LandingPercent = drx["LandingPercent"].ToString();
                            string LandingAmt = drx["LandingAmt"].ToString();
                            string TotalUSD = drx["TotalUSD"].ToString();
                            string TotalBDT = drx["TotalBDT"].ToString();
                            string UnitCostBDT = drx["UnitCostBDT"].ToString();
                            decimal price = Convert.ToDecimal(TotalBDT) / Convert.ToDecimal(qty);
                            string fullDescription = drx["FullDescription"].ToString();

                            //TextBox txtReceiveQty = drx.FindControl("lblVDSRate") as TextBox;
                            //decimal vdsAmt = Convert.ToDecimal(lblInvoiceTotal.Text)*Convert.ToDecimal(txtReceiveQty.Text)/100;

                            string detail = fullDescription + " - " + NoOfPacks + " - " + QntyPerPack;
                            string itemGroup = Stock.Inventory.GetItemGroup(ItemCode);
                            if (Convert.ToDecimal(itemGroup) > 3 && Convert.ToDecimal(itemGroup) != 7)//Machines, Electric, stationaries, others except wastage.
                            {
                                detail = "Model# : " + Measurement + ". " + ". Warranty: " + QntyPerPack + ", Serial # " + Thickness + ", Specification " + NoOfPacks + ", " + fullDescription;
                                pcs = Convert.ToInt32(Convert.ToDecimal(qty));

                            }



                            if (SQLQuery.ReturnString("Select UnitType from Products where ProductID='" + ItemCode + "'") == "PCS")
                            {
                                pcs = Convert.ToInt32(Convert.ToDecimal(qty));
                                qty = "0";
                            }

                            //Stock.Inventory.SaveToStock(Purpose, invNo,
                            //    "Purchase from " + ddVendor.SelectedItem.Text, lblEntryId.Text, PackSize, "", "", "",
                            //    SizeRef, iCode, iName, StockType, ddGodown.SelectedValue, ddLocation.SelectedValue,
                            //    iGrp, pcs, 0, Convert.ToDecimal(price), qty, 0, detail, "Stock-in", "Purchase",
                            //    ddLocation.SelectedItem.Text, lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));

                            Inventory.SaveToStock(Purpose, lcNo, "Import from " + lblSupplier.Text, EntryID, ItemSizeID, "", "", "",
                                Spec, ItemCode, Inventory.GetProductName(ItemCode), "", ddGodown.SelectedValue, ddLocation.SelectedValue,
                                itemGroup, Convert.ToDecimal(pcs), 0, price, Convert.ToDecimal(qty), 0,
                                detail, "", "LC", "", lName, DateTime.Now.ToString("yyyy-MM-dd"));
                            try
                            {
                                Stock.Inventory.FifoInsert(ItemCode, "LCItemReceived","", ItemSizeID, "","", DateTime.Now.ToString("yyyy-MM-dd"), "RawMaterials", EntryID, Convert.ToDecimal(UnitPrice), Convert.ToDecimal(qty),0, ddGodown.SelectedValue, ddLocation.SelectedValue, DateTime.Now.ToString("yyyy -MM-dd HH:mm"), "", "", "0");
                            }
                            catch (Exception ex)
                            {
                                Notify(ex.ToString(), "error", lblMsg);

                            }


                            Notify("LC Closed, Items inserted into stock.", "success", lblMsg);

                            //        //Insert for depriciation add
                            //        string depreciationType = SQLQuery.ReturnString(@"SELECT DepreciationType FROM  ItemGroup WHERE GroupSrNo='" + itemGroup + "'");
                            //        if (depreciationType == "2")
                            //        {
                            //            //AutoVoucherEntry

                            //          //  string productname = SQLQuery.ReturnString("SELECT  ProductName  FROM    FixedAssets WHERE ProductID ='" + ItemCode + "'");

                            //            string productlinkedhead = SQLQuery.ReturnString(@"SELECT AccountHead FROM Products WHERE ProductID='" + ItemCode + "'");
                            //            string lclinkedhead = SQLQuery.ReturnString("SELECT AccountsHeadID FROM LC WHERE (LCNo = '"+ LCNo + "')");
                            //            string totalvalue = SQLQuery.ReturnString("SELECT CfrUSD FROM LC WHERE (LCNo = '" + LCNo + "')");
                            //            string loanDescription = " Fixed Assets transferred from LC Purchase";
                            //         //   string invno = SQLQuery.ReturnString("SELECT  LCNoFTT FROM    FixedAssets WHERE ProductID ='" + ItemCode + "'");

                            //            Accounting.VoucherEntry.AutoVoucherEntry("15", loanDescription, productlinkedhead, lclinkedhead, Convert.ToDecimal(totalvalue), LCNo, "7", lName, DateTime.Now.ToString("yyyy-MM-dd"), "1");
                            //            //AutoVoucherEND
                            //            string iqty = txtTtlQty.Text;
                            //             int i = Convert.ToInt32(Convert.ToDecimal(iqty));
                            //            while (i > 0)
                            //            {
                            //                //string lName = Page.User.Identity.Name.ToString();
                            //                //string productName = ddProduct.SelectedItem.Text;

                            //                //int balance = Convert.ToInt32(qty) - Convert.ToInt32(qty);

                            //                string itemCode = AssetsItemcode();
                            //                string space12 = " ";
                            //                string destxt = " L/C Type :";
                            //                string fadescription = "LCNO:"+(LCNo) + (space12) + (destxt) + (space12) + (lblLCType.Text);

                            //                SqlCommand cmd2 = new SqlCommand(@"INSERT INTO FixedAssets (FixedAssetsType, OrderID, SizeId, ProductID, ProductName, BrandID, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, TotalWeight, EntryBy,QtyBalance, DeliveredQty,PurchaseCost,CurrentValue,Rate,ItemCode,InDate,InvID,ManufacturerName,LCNoFTT,Location,TotalValueUSD) 
                            //VALUES ('FixedAssets', @OrderID, @SizeId, @ProductID, @ProductName, @BrandID, @UnitCost, @Quantity, @UnitWeight, 'PCS', @ItemTotal, @TotalWeight, @EntryBy," + qty + ", '0','" + UnitPrice + "','"+ TotalBDT + "','" + UnitPrice + "','" + itemCode + "', '" + Convert.ToDateTime(lblOpDate.Text).ToString("yyyy-MM-dd") + "','0','"+ lblSupplier.Text + "','"+ LCNo + "','"+ lblCountry.Text + "','"+ UnitPrice + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                            //                cmd2.Parameters.AddWithValue("@OrderID", "0");
                            //                cmd2.Parameters.AddWithValue("@ProductID", ItemCode);
                            //                cmd2.Parameters.AddWithValue("@SizeId", "0");
                            //                //cmd2.Parameters.AddWithValue("@ItemCode", itemCode);
                            //                cmd2.Parameters.AddWithValue("@ProductName", fadescription);
                            //                cmd2.Parameters.AddWithValue("@BrandID", "0");
                            //                //cmd2.Parameters.AddWithValue("@CompanyFor", txtWarranty.Text);

                            //                cmd2.Parameters.AddWithValue("@UnitCost", "0");
                            //                cmd2.Parameters.AddWithValue("@Quantity", "1");// Convert.ToDecimal(txtQty.Text));
                            //                cmd2.Parameters.AddWithValue("@UnitWeight", "0");// Price

                            //                cmd2.Parameters.AddWithValue("@ItemTotal", "0");
                            //                cmd2.Parameters.AddWithValue("@TotalWeight", "0");
                            //                cmd2.Parameters.AddWithValue("@EntryBy", lName);

                            //                cmd2.Connection.Open();
                            //                cmd2.ExecuteNonQuery();
                            //                cmd2.Connection.Close();
                            //                  i--;
                            //            }


                            //        }
                        }
                        ddName.DataBind();
                        LoadLCData();
                        ItemGrid.DataBind();
                        Refresh();
                    }
                }
                else
                {
                    Notify("Please select godown location!", "info", lblMsg);
                }
            }
            else
            {
                SQLQuery.ExecNonQry("UPDATE LC Set IsActive='A' WHERE sl='" + ddName.SelectedValue + "'");
                Notify("LC Re-Opened for editing!", "info", lblMsg);
            }

            //}
            //else
            //{
            //    Notify("ERROR: LC item costing calc. has to be done for finalization!", "error", lblMsg);  
            //}
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }

    protected void rbClose_OnCheckedChanged(object sender, EventArgs e)
    {
        lblLoadType.Text = "D";
        if (rbClose.Checked)
        {
            lblLoadType.Text = "A";
        }
        ddName.DataBind();

        LoadLCData();
        if (ddName.Items.Count > 0)
        {
            ltrLC.Text = ddName.SelectedItem.Text;
        }

        ItemGrid.DataBind();
    }

    private decimal qty = (decimal)0.0;
    private decimal fob = (decimal)0.0;
    private decimal tcd = (decimal)0.0;
    private decimal va = (decimal)0.0;
    private decimal vaa = (decimal)0.0;
    private decimal cct = (decimal)0.0;
    private decimal insur = (decimal)0.0;
    private decimal cnf = (decimal)0.0;
    private decimal loc = (decimal)0.0;
    private decimal bi = (decimal)0.0;
    private decimal oth = (decimal)0.0;
    private decimal tic = (decimal)0.0;

    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            qty += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "qty"));
            fob += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "BankBDT"));
            tcd += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CustomDuty"));
            va += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "VatAtv"));
            vaa += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "VatAtvAit"));
            cct += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CnfComTax"));
            insur += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Insurance"));
            cnf += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CnfCharge"));
            loc += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "LcOpCost"));
            bi += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "BankInterest"));
            oth += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Others"));
            tic += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalImportCost"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[4].Text = "Total";
            e.Row.Cells[5].Text = Convert.ToString(qty);
            e.Row.Cells[8].Text = Convert.ToString(fob);
            e.Row.Cells[9].Text = Convert.ToString(tcd);
            e.Row.Cells[10].Text = Convert.ToString(va);
            e.Row.Cells[11].Text = Convert.ToString(vaa);
            e.Row.Cells[12].Text = Convert.ToString(cct);
            e.Row.Cells[13].Text = Convert.ToString(insur);
            e.Row.Cells[14].Text = Convert.ToString(cnf);
            e.Row.Cells[15].Text = Convert.ToString(loc);
            e.Row.Cells[16].Text = Convert.ToString(bi);
            e.Row.Cells[17].Text = Convert.ToString(oth);
            e.Row.Cells[18].Text = Convert.ToString(tic);
        }
    }
    public string AssetsItemcode()
    {
        string lName = Page.User.Identity.Name.ToString();
        string yr = Convert.ToDateTime(lblOpDate.Text).Year.ToString();// DateTime.Now.Year.ToString();
        DateTime countForYear = Convert.ToDateTime(lblOpDate.Text);

        SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL(COUNT(id),0)+ 1 )) from FixedAssets where EntryDate>=@EntryDate", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
            InvNo = SQLQuery.ReturnString("Select CONVERT(varchar, (ISNULL (COUNT(id),0)+ " + i + " )) from FixedAssets where EntryDate>='" + countForYear.ToString("yyyy-MM-dd") + "' AND  EntryDate<='" + countForYear.ToString("yyyy-12-31") + "'  ");
            while (InvNo.Length < 4)
            {
                InvNo = "0" + InvNo;
            }

            InvNo = yr + "" + InvNo;
            isExist = SQLQuery.ReturnString("Select ItemCode from FixedAssets where ItemCode='" + InvNo + "'  ");
        }

        //txtItemCode.Text = InvNo;
        return InvNo;

    }
}
