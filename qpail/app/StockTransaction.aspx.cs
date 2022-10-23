using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accounting;
using RunQuery;

public partial class app_StockTransaction : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToShortDateString();
            InvIdNo();

            ddParticular.DataBind();
            PopulateSubAcc();
            lblUser.Text = Page.User.Identity.Name.ToString();
            BindItemGrid();
        }
    }

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }


    public string InvIdNo()
    {
        try
        {
            SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (max(VID),0)+1001 )) from VoucherMaster", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            string invNo = Convert.ToString(cmd.ExecuteScalar());
            string year = Convert.ToString(Convert.ToDateTime(txtDate.Text).Year); // "2011"; // DateTime.Now.Year.ToString();
            invNo = "V-" + year + "-" + invNo;
            txtVID.Text = invNo;
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            return invNo;
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Invalid Date";
            SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (max(VID),0)+1001 )) from VoucherMaster", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            string invNo = Convert.ToString(cmd.ExecuteScalar());
            string year = Convert.ToString(DateTime.Now.Year);
            invNo = "V-" + year + "-" + invNo;
            txtVID.Text = invNo;
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            Notify(ex.Message, "error", lblMsg);
            return invNo;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string isValid = ddAccHeadDr.SelectedItem.Text.Substring(0, 3);
            if (isValid.Trim() != "__")
            {
                isValid = ddAccHeadCr.SelectedItem.Text.Substring(0, 3);
            }
            else
            {
                ddAccHeadDr.Focus();
                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "ERROR: Please select accounts head!";
                return;
            }

            if (isValid.Trim() != "__")
            {
                if (Convert.ToDecimal(txtAmount.Text) > 0 && txtAmount.Text != "")
                {
                    if (btnAdd.Text == "Add to Grid")
                    {
                        InsertData();
                    }
                    else
                    {
                        ExecuteUpdate();
                        btnAdd.Text = "Add to Grid";
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Entry updated successfully";
                    }

                    txtDescription.Text = "";
                    txtAmount.Text = "0";
                    txtKg.Text = "0";
                    txtPcs.Text = "0";
                    txtRate.Text = "0";
                    BindItemGrid();
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_warning");
                    lblMsg.Text = "ERROR: Head Amount not found!";
                }

            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "ERROR: Please select accounts head!";
                ddAccHeadCr.Focus();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            Notify(ex.Message, "error", lblMsg);
            lblMsg.Text = ex.ToString();
            txtAmount.Focus();
        }
    }

    private void InsertData()
    {
        string lName = Page.User.Identity.Name.ToString();

        string h5NameDr = "";
        if (ddHead5Dr.SelectedValue != "")
        {
            h5NameDr = ddHead5Dr.SelectedItem.Text;
        }
        string h5NameCr = "";
        if (ddHead5Cr.SelectedValue != "")
        {
            h5NameCr = ddHead5Cr.SelectedItem.Text;
        }

        SqlCommand cmd2;
        if (btnSave.Text != "Update")
        {
            cmd2 = new SqlCommand(@"INSERT INTO VoucherTmp (VoucherNo, Particular, VoucherRowDescription, AccountsHeadDr, AccountsHeadDrName, Head5Dr, Name5Dr, AccountsHeadCr, AccountsHeadCrName, Head5Cr, Name5Cr, Amount, Qty, Pcs, Rate, EntryDate, EntryBy)
                                VALUES ('', @Particular, @VoucherRowDescription, @AccountsHeadDr, @AccountsHeadDrName, '" + ddHead5Dr.SelectedValue + "', '" + h5NameDr + "', @AccountsHeadCr, @AccountsHeadCrName, '" + ddHead5Cr.SelectedValue + "', '" + h5NameCr + "', @Amount, @Qty, @Pcs, @Rate, @EntryDate, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }
        else
        {
            cmd2 = new SqlCommand(@"INSERT INTO VoucherTmp (VoucherNo, Particular, VoucherRowDescription, AccountsHeadDr, AccountsHeadDrName, Head5Dr, Name5Dr, AccountsHeadCr, AccountsHeadCrName, Head5Cr, Name5Cr, Amount, Qty, Pcs, Rate, EntryDate, EntryBy)
                                VALUES (@VoucherNo, @Particular, @VoucherRowDescription, @AccountsHeadDr, @AccountsHeadDrName, '" + ddHead5Dr.SelectedValue + "', '" + h5NameDr + "', @AccountsHeadCr, @AccountsHeadCrName, '" + ddHead5Cr.SelectedValue + "', '" + h5NameCr + "', @Amount, @Qty, @Pcs, @Rate, @EntryDate, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }

        cmd2.Parameters.AddWithValue("@VoucherNo", txtEditVoucherNo.Text);
        cmd2.Parameters.AddWithValue("@Particular", ddParticular.SelectedValue);
        cmd2.Parameters.AddWithValue("@VoucherRowDescription", txtDescription.Text);
        cmd2.Parameters.AddWithValue("@AccountsHeadDr", ddAccHeadDr.SelectedValue);
        cmd2.Parameters.AddWithValue("@AccountsHeadDrName", ddAccHeadDr.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@AccountsHeadCr", ddAccHeadCr.SelectedValue);
        cmd2.Parameters.AddWithValue("@AccountsHeadCrName", ddAccHeadCr.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@Amount", txtAmount.Text);
        cmd2.Parameters.AddWithValue("@Qty", txtKg.Text);
        cmd2.Parameters.AddWithValue("@Pcs", txtPcs.Text);
        cmd2.Parameters.AddWithValue("@Rate", txtRate.Text);
        cmd2.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
        cmd2.Parameters.AddWithValue("@EntryBy", lName);
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
    }

    private void ExecuteUpdate()
    {
        string lName = Page.User.Identity.Name.ToString();

        string h5NameDr = "";
        if (ddHead5Dr.SelectedValue != "")
        {
            h5NameDr = ddHead5Dr.SelectedItem.Text;
        }
        string h5NameCr = "";
        if (ddHead5Cr.SelectedValue != "")
        {
            h5NameCr = ddHead5Cr.SelectedItem.Text;
        }
        string query = " UPDATE VoucherTmp SET Particular='" + ddParticular.SelectedValue + "', VoucherRowDescription='" + txtDescription.Text + "'," +
                       " AccountsHeadDr='" + ddAccHeadDr.SelectedValue + "'," + " AccountsHeadDrName='" + ddAccHeadDr.SelectedItem.Text + "'," +
                       " Head5Dr='" + ddHead5Dr.SelectedValue + "', Name5Dr='" + h5NameDr + "', AccountsHeadCr='" + ddAccHeadCr.SelectedValue +"', " +
                       " AccountsHeadCrName='" + ddAccHeadCr.SelectedItem.Text + "', Head5Cr='" + ddHead5Cr.SelectedValue + "', Name5Cr='" + h5NameCr + "', " +
                       " Amount=@Amount, Qty=@Qty, Pcs=@Pcs, Rate=@Rate  where (SerialNo ='" + lblSl.Text + "')";
        SqlCommand cmd2 = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@Amount", txtAmount.Text);
        cmd2.Parameters.AddWithValue("@Qty", txtKg.Text);
        cmd2.Parameters.AddWithValue("@Pcs", txtPcs.Text);
        cmd2.Parameters.AddWithValue("@Rate", txtRate.Text);
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //txtTTL.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM StockTmp where VoucherNo='' AND EntryBy='" + Page.User.Identity.Name.ToString() + "'");
    }
    protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView2.Rows[index].FindControl("lblSl") as Label;
            if (lblItemCode != null) SQLQuery.ExecNonQry("DELETE VoucherTmp WHERE SerialNo=" + lblItemCode.Text);

            BindItemGrid();
            lblMsg.Attributes.Add("class", "xerp_warning");
            lblMsg.Text = "Entry removed successfully ...";
            btnAdd.Text = "Add to Grid";

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "ERROR: " + ex.ToString();
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            string query = "";
            if (txtEditVoucherNo.Text == "")
            {
                query = " AND EntryBy='" + lName + "'";
            }

            decimal ttl = Convert.ToDecimal(SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp  WHERE VoucherNo='" + txtEditVoucherNo.Text + "' " + query));
            if (ttl > 0)
            {
                string voucherNo = InvIdNo();
                if (btnSave.Text == "Update")
                {
                    voucherNo = txtEditVoucherNo.Text;
                    SQLQuery.ExecNonQry("Delete VoucherMaster where VoucherNo='" + voucherNo + "'");
                    SQLQuery.ExecNonQry("Delete VoucherDetails where VoucherNo='" + voucherNo + "'");
                    //SQLQuery.ExecNonQry("Delete Transactions where InvNo='" + voucherNo + "'");
                    SQLQuery.ExecNonQry("Delete Stock where InvoiceID='" + voucherNo + "'");
                    SQLQuery.ExecNonQry("Update VoucherDetails set ISApproved='C' where VoucherNo IN (Select VoucherNo from VoucherMaster WHERE VoucherReferenceNo='" + voucherNo + "')");
                    SQLQuery.ExecNonQry("Update VoucherMaster set Voucherpost='C' where VoucherReferenceNo='" + voucherNo + "'");
                }

                SaveData(voucherNo);
                InvIdNo();

                btnSave.Text = "Save Voucher";
                txtEditVoucherNo.Text = "";
                BindItemGrid();
                GridView1.DataBind();
                txtTTL.Text = "0";

                if (chkPrint.Checked)
                {
                    string url = "./Voucher.aspx?Inv=" + voucherNo;
                    ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");
                    Response.Write("<script>window.open(" + url + ", '_blank' );</script>");
                    //Response.Redirect = "Purchase.aspx";
                }

                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "New Stock Transaction Saved Successfully.";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: No data was found into current voucher!";
            }

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }

    private void SaveData(string voucherNo)
    {
        string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        SqlCommand cmd2X = new SqlCommand("INSERT INTO VoucherMaster (VoucherNo, VoucherDate, VoucherDescription, ParticularID, VoucherEntryBy, VoucherAmount)" +
                                          "VALUES (@VoucherNo, @VoucherDate, @VoucherDescription, @ParticularID, @VoucherEntryBy, @VoucherAmount)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        string lName = Page.User.Identity.Name.ToString();
        cmd2X.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = voucherNo;
        cmd2X.Parameters.Add("@VoucherDate", SqlDbType.DateTime).Value = dt;
        cmd2X.Parameters.Add("@VoucherDescription", SqlDbType.VarChar).Value = ddParticular.SelectedItem.Text;
        cmd2X.Parameters.Add("@ParticularID", SqlDbType.NVarChar).Value = ddParticular.SelectedValue; //SQLQuery.ReturnString("Select Particularsid FROM Particulars WHERE Particularsname='" + ddParticular.SelectedItem.Text + "'");
        cmd2X.Parameters.Add("@VoucherEntryBy", SqlDbType.VarChar).Value = lName;
        cmd2X.Parameters.Add("@VoucherAmount", SqlDbType.Decimal).Value = txtTTL.Text;

        cmd2X.Connection.Open();
        int success = cmd2X.ExecuteNonQuery();
        cmd2X.Connection.Close();

        string query = "";
        if (txtEditVoucherNo.Text == "")
        {
            query = " AND EntryBy='" + lName + "'";
        }

        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT SerialNo, Particular, VoucherRowDescription, AccountsHeadDr, 
                AccountsHeadDrName, Head5Dr, Name5Dr, AccountsHeadCr, AccountsHeadCrName, Head5Cr, Name5Cr, Amount, Qty, Pcs, Rate, projectName, EntryDate, ISApproved
                FROM VoucherTmp WHERE VoucherNo='" + txtEditVoucherNo.Text + "' " + query);

        foreach (DataRow drx in dtx.Rows)
        {
            string particular = drx["Particular"].ToString();
            string description = drx["VoucherRowDescription"].ToString();
            string acHeadDr = drx["AccountsHeadDr"].ToString();
            string head5Dr = drx["Head5Dr"].ToString();
            string name5Dr = drx["Name5Dr"].ToString();
            string acHeadCr = drx["AccountsHeadCr"].ToString();
            string head5Cr = drx["Head5Cr"].ToString();
            string name5Cr = drx["Name5Cr"].ToString();
            decimal amount = Convert.ToDecimal(drx["Amount"].ToString());
            decimal kg = Convert.ToDecimal(drx["Qty"].ToString());
            decimal pcs = Convert.ToDecimal(drx["Pcs"].ToString());
            string rate = drx["Rate"].ToString();

            //Finished goods output
            string wHouseAreaLocDr = SQLQuery.ReturnString("SELECT AreaID FROM WareHouseAreas WHERE Warehouse='" + head5Dr + "'");
            string wHouseAreaLocCr = SQLQuery.ReturnString("SELECT AreaID FROM WareHouseAreas WHERE Warehouse='" + head5Cr + "'");
            string sizeIdDr = SQLQuery.ReturnString("SELECT SizeID FROM FinishedProducts WHERE pid ='" + acHeadDr + "'");
            string customerDr = SQLQuery.ReturnString("SELECT CompanyID FROM FinishedProducts WHERE pid ='" + acHeadDr + "'");
            string brandIdDr = SQLQuery.ReturnString("SELECT BrandID FROM FinishedProducts WHERE pid ='" + acHeadDr + "'");
            string prodIdDr = SQLQuery.ReturnString("SELECT ProductID FROM FinishedProducts WHERE pid ='" + acHeadDr + "'");
            string prodNameDr = SQLQuery.ReturnString("SELECT ProductName FROM FinishedProducts WHERE pid ='" + acHeadDr + "'");
            string prodNameCr = SQLQuery.ReturnString("SELECT ItemName FROM Products WHERE ProductID  ='" + acHeadCr + "'");
            string itemTypeDr = SQLQuery.ReturnString("SELECT (SELECT ItemName FROM Products WHERE ProductID=FinishedProducts.ProductID AND CategoryID=FinishedProducts.Category) FROM  FinishedProducts WHERE pid ='" + acHeadDr + "'");
            string itemGroupDr = SQLQuery.ReturnString("SELECT (SELECT GroupID FROM ItemSubGroup WHERE CategoryID=FinishedProducts.SubGroup) FROM  FinishedProducts WHERE pid ='" + acHeadDr + "'");
            string inWeight = SQLQuery.ReturnString("SELECT UnitWeight FROM FinishedProducts WHERE pid ='" + acHeadDr + "'");
            //End Finished goods output
            //if (particular == "0104")
            //{
            //    VoucherEntry.AutoVoucherEntryFinishedGoods("13", description, "010136001", acHeadCr, amount, kg, pcs, rate, voucherNo, lName, dt, "1");
            //}
            //else
            //{
                VoucherEntry.InsertStockDetails(voucherNo, description, acHeadDr, amount, 0, kg, 0, pcs, 0, rate, dt, "Stock Tran 361");
                VoucherEntry.InsertStockDetails(voucherNo, description, acHeadCr, 0, amount, 0, kg, 0, pcs, rate, dt, "Stock Tran 362");
            //}
            
            //AutoTransaction(voucherNo, acHeadDr, head5Dr, name5Dr, amount.ToString(), "Dr", description + " Voucher# " + voucherNo);
            //AutoTransaction(voucherNo, acHeadCr, head5Cr, name5Cr, amount.ToString(), "Cr", description + " Voucher# " + voucherNo);

            if (particular == "0101")//Stock issue to production floor
            {
                prodNameDr = SQLQuery.ReturnString("SELECT ItemName FROM Products WHERE ProductID ='" + acHeadDr + "'");
                prodNameCr = SQLQuery.ReturnString("SELECT ItemName FROM Products WHERE ProductID ='" + acHeadCr + "'");
                itemGroupDr = SQLQuery.ReturnString("SELECT (SELECT GroupID FROM ItemSubGroup WHERE CategoryID= (SELECT CategoryID FROM ItemGrade WHERE GradeID=(SELECT GradeID FROM Categories WHERE CategoryID= (SELECT CategoryID FROM  Products  WHERE ProductID ='" + acHeadDr + "'))))");
                string itemGroupCr = SQLQuery.ReturnString("SELECT (SELECT GroupID FROM ItemSubGroup WHERE CategoryID= (SELECT CategoryID FROM ItemGrade WHERE GradeID=(SELECT GradeID FROM Categories WHERE CategoryID= (SELECT CategoryID FROM  Products  WHERE ProductID ='" + acHeadCr + "'))))");
                
                Stock.Inventory.SaveToStock(particular, voucherNo, "Stock Transaction", voucherNo, "0", "0", "0", "0", "0", acHeadDr, prodNameDr, "0", head5Dr, wHouseAreaLocDr, itemGroupDr, Convert.ToDecimal(pcs), 0, amount, Convert.ToDecimal(kg), 0, "", "Stock-in", "Production Output", "Stock issue to production floor", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), description);
                Stock.Inventory.SaveToStock(particular, voucherNo, "Stock Transaction", voucherNo, "0", "0", "0", "0", "0", acHeadCr, prodNameCr, "0", head5Cr, wHouseAreaLocCr, itemGroupCr, 0, Convert.ToDecimal(pcs), amount, 0, Convert.ToDecimal(kg), "", "Stock-out", "Production Input", "Stock issue to production floor", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), description);
            }
            if (particular == "0102")//Transfer to machine
            {
                prodNameDr = SQLQuery.ReturnString("SELECT ItemName FROM Products WHERE ProductID ='" + acHeadDr + "'");
                prodNameCr = SQLQuery.ReturnString("SELECT ItemName FROM Products WHERE ProductID ='" + acHeadCr + "'");
                itemGroupDr = SQLQuery.ReturnString("SELECT (SELECT GroupID FROM ItemSubGroup WHERE CategoryID= (SELECT CategoryID FROM ItemGrade WHERE GradeID=(SELECT GradeID FROM Categories WHERE CategoryID= (SELECT CategoryID FROM  Products  WHERE ProductID ='" + acHeadDr + "'))))");
                string itemGroupCr = SQLQuery.ReturnString("SELECT (SELECT GroupID FROM ItemSubGroup WHERE CategoryID= (SELECT CategoryID FROM ItemGrade WHERE GradeID=(SELECT GradeID FROM Categories WHERE CategoryID= (SELECT CategoryID FROM  Products  WHERE ProductID ='" + acHeadCr + "'))))");
                string wHouseId = "10";
                Stock.Inventory.SaveToStock(particular, voucherNo, "Stock Transaction", voucherNo, "0", "0", "0", "0", "0", acHeadDr, prodNameDr, "0", wHouseId, head5Dr, itemGroupDr, Convert.ToDecimal(pcs), 0, amount, Convert.ToDecimal(kg), 0, "", "Stock-in", "Production Output", "Transfer to machine", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), description);
                Stock.Inventory.SaveToStock(particular, voucherNo, "Stock Transaction", voucherNo, "0", "0", "0", "0", "0", acHeadCr, prodNameCr, "0", head5Cr, wHouseAreaLocCr, itemGroupCr, 0, Convert.ToDecimal(pcs), amount, 0, Convert.ToDecimal(kg), "", "Stock-out", "Production Input", "Transfer to machine", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), description);
            }
            if (particular == "0103")//Semi-finished production
            {
                prodNameDr = SQLQuery.ReturnString("SELECT ItemName FROM Products WHERE ProductID ='" + acHeadDr + "'");
                prodNameCr = SQLQuery.ReturnString("SELECT ItemName FROM Products WHERE ProductID ='" + acHeadCr + "'");
                itemGroupDr = SQLQuery.ReturnString("SELECT (SELECT GroupID FROM ItemSubGroup WHERE CategoryID= (SELECT CategoryID FROM ItemGrade WHERE GradeID=(SELECT GradeID FROM Categories WHERE CategoryID= (SELECT CategoryID FROM  Products  WHERE ProductID ='" + acHeadDr + "'))))");
                string itemGroupCr = SQLQuery.ReturnString("SELECT (SELECT GroupID FROM ItemSubGroup WHERE CategoryID= (SELECT CategoryID FROM ItemGrade WHERE GradeID=(SELECT GradeID FROM Categories WHERE CategoryID= (SELECT CategoryID FROM  Products  WHERE ProductID ='" + acHeadCr + "'))))");
                string wHouseId = "10";
                Stock.Inventory.SaveToStock(particular, voucherNo, "Stock Transaction", voucherNo, "0", "0", "0", "0", "0", acHeadDr, prodNameDr, "0", head5Dr, wHouseAreaLocDr, itemGroupDr, Convert.ToDecimal(pcs), 0, amount, Convert.ToDecimal(kg), 0, "", "Stock-in", "Production Output", "Semi-finished production", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), description);
                Stock.Inventory.SaveToStock(particular, voucherNo, "Stock Transaction", voucherNo, "0", "0", "0", "0", "0", acHeadCr, prodNameCr, "0", wHouseId, head5Cr, itemGroupCr, 0, Convert.ToDecimal(pcs), amount, 0, Convert.ToDecimal(kg), "", "Stock-out", "Production Input", "Semi-finished production", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), description);

            }
            if (particular== "0104")//Finished goods output
            {
                Stock.Inventory.SaveToStock(particular, voucherNo, "Stock Transaction", voucherNo, sizeIdDr, customerDr, brandIdDr, "", "", prodIdDr, prodNameDr, itemTypeDr, head5Dr, wHouseAreaLocDr, itemGroupDr, Convert.ToDecimal(pcs), 0, amount, Convert.ToDecimal(kg), 0, "", "Stock-in", "Production", "Finished goods output", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), description);
                Stock.Inventory.SaveToStock(particular, voucherNo, "Stock Transaction", voucherNo, "0", "0", "0", "", "", acHeadCr, prodNameCr, "0", head5Cr, wHouseAreaLocCr, "0", 0, Convert.ToDecimal(pcs), amount, 0, Convert.ToDecimal(kg), "", "Stock-out", "Processed used", "Finished goods output", lName, Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), description);
            }
        }

        SQLQuery.ExecNonQry("Update VoucherTmp Set VoucherNo='" + voucherNo + "'  WHERE VoucherNo='" + txtEditVoucherNo.Text + "' " + query);
    }

    public static class ResponseHelper
    {
        public static void Redirect(string url, string target, string windowFeatures)
        {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures))
            {

                context.Response.Redirect(url);
            }
            else
            {
                Page page = (Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException(
                        "Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(Page),
                    "Redirect",
                    script,
                    true);
            }
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        PopulateSubAcc();
    }
    protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView2.SelectedIndex);
            Label label2 = GridView2.Rows[index].FindControl("lblSl") as Label;
            if (label2 != null) lblSl.Text = label2.Text;
            /*SqlCommand cmd7 = new SqlCommand(@"SELECT [VoucherRowDescription],  AccountsHeadDr, AccountsHeadCr, Amount, Particular, Head5Dr, Head5Cr, VoucherRowDescription, qty, rate
                                        FROM [VoucherTmp] WHERE SerialNo ='" + lblSl.Text + "'",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));*/
            SqlCommand cmd7 = new SqlCommand(@"SELECT [VoucherRowDescription], AccountsHeadDr, AccountsHeadCr, Amount, Particular, Head5Dr, Head5Cr, VoucherRowDescription, Qty, Pcs, rate
                                               FROM [VoucherTmp] WHERE SerialNo ='" + lblSl.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                btnAdd.Text = "Update";
                txtDescription.Text = dr[0].ToString();
                ddParticular.SelectedValue = Convert.ToString(dr[4].ToString());
                PopulateSubAcc();

                ddControlDr.SelectedValue = dr[1].ToString().Substring(0, 6);
                
                LoadDr();
                ddAccHeadDr.SelectedValue = dr[1].ToString();
                //Load5ThHead(ddHead5Dr, ddAccHeadDr);
                Load5ThDr(ddHead5Dr, ddAccHeadDr);
                if (dr[5].ToString() != "")
                {
                    ddHead5Dr.SelectedValue = dr[5].ToString();
                }
                ddControlCr.SelectedValue = dr[2].ToString().Substring(0, 6);
                //ddControlCr.SelectedValue = SQLQuery.ReturnString("Select AccountsHeadID from HeadSetup where ControlAccountsID='" + dr[2].ToString() + "'");
                LoadCr();

                ddAccHeadCr.SelectedValue = dr[2].ToString();
                //Load5ThHead(ddHead5Cr, ddAccHeadCr);
                Load5ThCr(ddHead5Cr, ddAccHeadCr);
                if (dr[6].ToString() != "")
                {
                    ddHead5Cr.SelectedValue = dr[6].ToString();
                }

                txtDescription.Text = Convert.ToString(dr[7].ToString());
                txtAmount.Text = Convert.ToString(dr[3].ToString());
                txtKg.Text = Convert.ToString(dr[8].ToString());
                txtPcs.Text = Convert.ToString(dr[9].ToString());
                txtRate.Text = Convert.ToString(dr[10].ToString());
                lblMsg.Attributes.Add("class", "xerp_info");
                lblMsg.Text = "Stock Transaction info loaded in edit mode";
            }

            cmd7.Connection.Close();
            //pan.Update();

        }
        catch (Exception ex)
        {
            lblMsg.CssClass = "xerp_error";
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "ERROR: " + ex.ToString();

        }
    }

    private void PopulateSubAcc()
    {
        try
        {
            /*
            string q = @"SELECT ControlAccountsID,
                             (SELECT AccountsName
                               FROM Accounts
                               WHERE (AccountsID = ControlAccount.AccountsID)) + ' > ' + ControlAccountsName AS name
                                FROM ControlAccount
                                WHERE (AccountsID NOT IN
                                                             (SELECT        AccountsID
                                                               FROM            Accounts AS Accounts_1
                                                               WHERE        (GroupID <> '02') AND (GroupID <> '04')))
                                ORDER BY ControlAccountsID";

            if (ddParticular.SelectedItem.Text == "Receipt")
            {
                SQLQuery.PopulateDropDown("Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount where (ControlAccountsID='010101'  OR  ControlAccountsID='010102') Order by ControlAccountsID", ddControlDr, "ControlAccountsID", "name");
                SQLQuery.PopulateDropDown(q, ddControlCr, "ControlAccountsID", "name");

            }
            else if (ddParticular.SelectedItem.Text == "Payment")
            {
                q = @"SELECT        ControlAccountsID,
                             (SELECT        AccountsName
                               FROM            Accounts
                               WHERE        (AccountsID = ControlAccount.AccountsID)) + ' > ' + ControlAccountsName AS name
                                FROM            ControlAccount
                                WHERE        (AccountsID NOT IN
                                                             (SELECT        AccountsID
                                                               FROM            Accounts AS Accounts_1
                                                               WHERE        (GroupID <> '01') AND (GroupID <> '03')))
                                ORDER BY ControlAccountsID";
                SQLQuery.PopulateDropDown(q, ddControlDr, "ControlAccountsID", "name");
                SQLQuery.PopulateDropDown("Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount Where (ControlAccountsID='010101'  OR  ControlAccountsID='010102') Order by ControlAccountsID", ddControlCr, "ControlAccountsID", "name");
            }
            else // Journal 
            {*/
            //SQLQuery.PopulateDropDown("Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount  Order by ControlAccountsID", ddControlDr, "ControlAccountsID", "name");
            //SQLQuery.PopulateDropDown("Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount  Order by ControlAccountsID", ddControlCr, "ControlAccountsID", "name");
            //}

            //*** Stock issue to production floor || Transfer to machine || Semi-finished production || Finished goods output || Recycle transfer to store || Reject goods transfer to crush ***//
            if (ddParticular.SelectedValue == "0101" || ddParticular.SelectedValue == "0102" || ddParticular.SelectedValue == "0103" || ddParticular.SelectedValue == "0104" || ddParticular.SelectedValue == "0105" || ddParticular.SelectedValue == "0106")
            {
                SQLQuery.PopulateDropDown("Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount where PrdnLinkId='" + ddParticular.SelectedValue + "' Order by ControlAccountsID", ddControlDr, "ControlAccountsID", "name");
                SQLQuery.PopulateDropDown("Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount Order by ControlAccountsID", ddControlCr, "ControlAccountsID", "name");
            }
            else
            {
                SQLQuery.PopulateDropDown("Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount  Order by ControlAccountsID", ddControlDr, "ControlAccountsID", "name");
                SQLQuery.PopulateDropDown("Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount  Order by ControlAccountsID", ddControlCr, "ControlAccountsID", "name");
            }

            PopulateHeads();
        }
        catch (Exception ex)
        {
            lblMsg.CssClass = "xerp_error";
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "ERROR: " + ex.ToString();
        }

    }

    private void PopulateHeads()
    {
        try
        {
            //if (ddParticular.SelectedValue == "0104") //Product by customer (Finished goods output)
            //{
            //    SQLQuery.PopulateDropDown("Select pid, ProductName from FinishedProducts WHERE CompanyID='" + ddControlDr.SelectedValue + "' ", ddAccHeadDr, "pid", "ProductName");
            //    //SQLQuery.PopulateDropDown("Select ProductID, ItemName from Products WHERE CategoryID='" + ddControlCr.SelectedValue + "' ", ddAccHeadCr, "ProductID", "ItemName");
            //    SQLQuery.PopulateDropDown("Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID='" + ddControlCr.SelectedValue + "' ", ddAccHeadCr, "AccountsHeadID", "AccountsHeadName");
            //}
            //else
            //{
                //SQLQuery.PopulateDropDown("Select ProductID, ItemName from Products WHERE CategoryID='" + ddControlDr.SelectedValue + "' ", ddAccHeadDr, "ProductID", "ItemName");
                //SQLQuery.PopulateDropDown("Select ProductID, ItemName from Products WHERE CategoryID='" + ddControlCr.SelectedValue + "' ", ddAccHeadCr, "ProductID", "ItemName");
                SQLQuery.PopulateDropDown("Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID='" + ddControlDr.SelectedValue + "' ", ddAccHeadDr, "AccountsHeadID", "AccountsHeadName");
                SQLQuery.PopulateDropDown("Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID='" + ddControlCr.SelectedValue + "' ", ddAccHeadCr, "AccountsHeadID", "AccountsHeadName");

            //}

            /*
        ddAccHeadDr.Items.Clear();
        ddAccHeadCr.Items.Clear();

        if (ddParticular.SelectedItem.Text == "Salary")
        {
            SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND ControlAccountsID='040114' ");
            SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND AccountsID='0101' ");
        }
        else if (ddParticular.SelectedItem.Text == "Receipt")
        {
            SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND (ControlAccountsID='010101'  OR  ControlAccountsID='010102')  ");
            SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND GroupID<>'04' AND GroupID<>'02' AND  ControlAccountsID<>'010101' AND  ControlAccountsID<>'010102'  ");
        }
        else if (ddParticular.SelectedItem.Text == "Purchase")
        {
            SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND GroupID='04' "); //Expense
            SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND AccountsID='0201' ");//Suppliers
        }
        else if (ddParticular.SelectedItem.Text == "Sales")
        {
            SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND ControlAccountsID='010105' "); //Customer
            SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND GroupID='03' "); //Income
        }
        else if (ddParticular.SelectedItem.Text == "Payment")
        {
            SQLQuery.PopulateMultiDropDown(ddAccHeadDr, " AND GroupID<>'01'  AND GroupID<>'03' ");//exclude assets & income
            SQLQuery.PopulateMultiDropDown(ddAccHeadCr, " AND (ControlAccountsID='010101'  OR  ControlAccountsID='010102') "); // only cash & bank
        }
        else // Journal
        {
            SQLQuery.PopulateMultiDropDown(ddAccHeadDr, "");
            SQLQuery.PopulateMultiDropDown(ddAccHeadCr, "");
        }
  */
            //Load5ThHead(ddHead5Dr, ddAccHeadDr);
            //Load5ThHead(ddHead5Cr, ddAccHeadCr);
            Load5ThDr(ddHead5Dr, ddAccHeadDr);
            Load5ThCr(ddHead5Cr, ddAccHeadCr);
            //txtDescription.Text = SQLQuery.ReturnString("Select Detail FROM Particulars where Particularsid='" + ddParticular.SelectedValue + "'");

        }
        catch (Exception ex)
        {
            lblMsg.CssClass = "xerp_error";
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }

    private void PopulateGodowns()
    {
        try
        {
            //if (ddParticular.SelectedValue == "0102") //Machine
            //{
            //    SQLQuery.PopulateDropDown("Select mid, MachineNo +' '+(Select SName from Sections where SID=Machines.Section)+'- '+Description as Machines from Machines", ddHead5Dr, "mid", "Machines");
            //    SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Cr, "WID", "StoreName");
            //}
            //else
            //{
                SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Dr, "WID", "StoreName");
                SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Cr, "WID", "StoreName");
            //}

        }
        catch (Exception ex)
        {
            lblMsg.CssClass = "xerp_error";
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }
    protected void ddParticular_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateSubAcc();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtDate.Text = DateTime.Now.ToShortDateString();
        InvIdNo();
        ddParticular.DataBind();
        PopulateSubAcc();

        SQLQuery.ExecNonQry("Delete VoucherTmp WHERE VoucherNo='' AND EntryBy ='" + Page.User.Identity.Name.ToString() + "'");
    }

    protected void lbRefresh_OnClick(object sender, EventArgs e)
    {
        PopulateSubAcc();
        Notify("Data has been refreshed", "info", lblMsg);
    }

    protected void txtDate_OnTextChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    protected void ddAccHeadDr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //Load5ThHead(ddHead5Dr, ddAccHeadDr);
        Load5ThDr(ddHead5Dr, ddAccHeadDr);

        //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "GroupDropDownList()", true);
        //PopulateGodowns();
    }

    protected void ddAccHeadCr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //Load5ThHead(ddHead5Cr, ddAccHeadCr);
        ddHead5Cr.DataBind();
        Load5ThCr(ddHead5Cr, ddAccHeadCr);
        GenerateNarration();
    }
    private void Load5ThDr(DropDownList ddHead5, DropDownList ddAccHead)
    {
        //if (ddParticular.SelectedValue == "0102") //Machine
        //{
        //    //SQLQuery.PopulateDropDown("Select mid, MachineNo +' '+(Select SName from Sections where SID=Machines.Section)+'- '+Description as Machines from Machines", ddHead5Dr, "mid", "Machines");
        //    SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Cr, "WID", "StoreName");
        //    ddHead5Cr.SelectedValue = "8";
        //}
        //else
        //{
        //    ddHead5.Items.Clear();
        //    if (ddParticular.SelectedValue == "0101" || ddParticular.SelectedValue == "0103")//Stock issue to production floor || Semi-finished production
        //    {
        //        SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Dr, "WID", "StoreName");
        //        ddHead5Dr.SelectedValue = "8";
        //    }
        //    else
        //    {
        //        SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Dr, "WID", "StoreName");
        //        ddHead5Dr.SelectedValue = "4";
        //    }
        //}

        ddHead5.Visible = true;

        if (ddAccHead.SelectedValue == "0104")// || ddAccHead.SelectedValue == "040301001" || ddAccHead.SelectedValue == "040302001")//Bank
        {
            //SQLQuery.PopulateDropDown("Select ACID, (Select BankName from Banks where BankId=BankAccounts.BankID)+'- '+ACNo as bank from BankAccounts ", ddHead5, "ACID", "bank");
            //SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Dr, "WID", "StoreName");
            //ddHead5Dr.SelectedValue = "4";

        }
        //else if (ddAccHead.SelectedValue == "010104001" || ddAccHead.SelectedValue == "030101001")//Customer
        //{
        //    SQLQuery.PopulateDropDown("Select PartyID, Company from Party where Type='customer' order by Company ", ddHead5, "PartyID", "Company");

        //}
        //else if (ddAccHead.SelectedValue == "020102006")//Suppliers
        //{
        //    //temporary disabled
        //    //SQLQuery.PopulateDropDown("Select PartyID, Company from Party where Type='vendor' order by Company ", ddHead5, "PartyID", "Company");

        //}
        else
        {
            ddHead5.Items.Clear();
            ddHead5.Visible = false;
            GenerateNarration();
        }
    }
    private void Load5ThCr(DropDownList ddHead5, DropDownList ddAccHead)
    {
        //if (ddParticular.SelectedValue == "0102" || ddParticular.SelectedValue == "0104") //Machine || Finished goods output
        //{
        //    SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Cr, "WID", "StoreName");
        //    ddHead5Cr.SelectedValue = "8";
        //}
        //else
        //{
        //    ddHead5.Items.Clear();
        //    if (ddParticular.SelectedValue == "0101")//Stock issue to production floor
        //    {
        //        SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Cr, "WID","StoreName");
        //        ddHead5Cr.SelectedValue = "4";
        //    }
        //    else if (ddParticular.SelectedValue == "0103")//Semi-finished production
        //    {
        //        //SQLQuery.PopulateDropDown("Select mid, MachineNo +' '+(Select SName from Sections where SID=Machines.Section)+'- '+Description as Machines from Machines", ddHead5Cr, "mid", "Machines");
        //        SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Cr, "WID", "StoreName");
        //        ddHead5Cr.SelectedValue = "8";
        //    }
        //    else
        //    {
        //        SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Cr, "WID", "StoreName");
        //    }
        //}
        ddHead5.Visible = true;

        if (ddAccHead.SelectedValue == "0104")// || ddAccHead.SelectedValue == "040301001" || ddAccHead.SelectedValue == "040302001")//Bank
        {
            //SQLQuery.PopulateDropDown("Select ACID, (Select BankName from Banks where BankId=BankAccounts.BankID)+'- '+ACNo as bank from BankAccounts ", ddHead5, "ACID", "bank");
            //SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Cr, "WID", "StoreName");
            //ddHead5Dr.SelectedValue = "4";
        }
        //else if (ddAccHead.SelectedValue == "010104001" || ddAccHead.SelectedValue == "030101001")//Customer
        //{
        //    SQLQuery.PopulateDropDown("Select PartyID, Company from Party where Type='customer' order by Company ", ddHead5, "PartyID", "Company");

        //}
        //else if (ddAccHead.SelectedValue == "020102006")//Suppliers
        //{
        //    //temporary disabled
        //    //SQLQuery.PopulateDropDown("Select PartyID, Company from Party where Type='vendor' order by Company ", ddHead5, "PartyID", "Company");

        //}
        else
        {
            ddHead5.Items.Clear();
            ddHead5.Visible = false;
            GenerateNarration();
        }
    }

    private void Load5ThHead(DropDownList ddHead5, DropDownList ddAccHead)
    {
        ////ddHead5.Visible = true;

        //if (ddParticular.SelectedValue == "0102") //Machine || ddAccHead.SelectedValue == "040301001" || ddAccHead.SelectedValue == "040302001")//Bank
        //{
        //    SQLQuery.PopulateDropDown("Select mid, MachineNo +' '+(Select SName from Sections where SID=Machines.Section)+'- '+Description as Machines from Machines", ddHead5Dr, "mid", "Machines");
        //    SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Cr, "WID", "StoreName");

        //}
        ////if (ddParticular.SelectedValue == "0104") //Product by customer
        ////{
        ////    SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Dr, "WID", "StoreName");
        ////    SQLQuery.PopulateDropDown("Select WID, StoreName from Warehouses", ddHead5Cr, "WID", "StoreName");

        ////}
        ////if (ddAccHead.SelectedValue == "010101002")// || ddAccHead.SelectedValue == "040301001" || ddAccHead.SelectedValue == "040302001")//Bank
        ////{
        ////    SQLQuery.PopulateDropDown("Select ACID, (Select BankName from Banks where BankId=BankAccounts.BankID)+'- '+ACNo as bank from BankAccounts ", ddHead5, "ACID", "bank");

        ////}
        ////else if (ddAccHead.SelectedValue == "010104001" || ddAccHead.SelectedValue == "030101001")//Customer
        ////{
        ////    SQLQuery.PopulateDropDown("Select PartyID, Company from Party where Type='customer' order by Company ", ddHead5, "PartyID", "Company");

        ////}
        ////else if (ddAccHead.SelectedValue == "020102006")//Suppliers
        ////{
        ////    //temporary disabled
        ////    //SQLQuery.PopulateDropDown("Select PartyID, Company from Party where Type='vendor' order by Company ", ddHead5, "PartyID", "Company");

        ////}
        //else
        //{
        //    ddHead5.Items.Clear();
        //    //ddHead5.Visible = false;
        //    PopulateGodowns();
        //    GenerateNarration();
        //}
    }

    private void GenerateNarration()
    {
        //Naration builder
        string particular = SQLQuery.ReturnString("Select Detail FROM Particulars where Particularsid='" + ddParticular.SelectedValue + "'");
        if (ddParticular.SelectedValue == "4") //Collection
        {
            txtDescription.Text = "Received from " + ddAccHeadCr.SelectedItem.Text.Trim();
        }
        else if (ddParticular.SelectedValue == "9") //Payment
        {
            txtDescription.Text = "Paid to " + ddAccHeadDr.SelectedItem.Text.Trim() + " for ...";
        }
        else if (ddAccHeadDr.SelectedValue != "" && ddAccHeadCr.SelectedValue != "") // for journal
        {
            string devider = " for ";
            if (ddAccHeadDr.SelectedItem.Text.IndexOf(" for ") != -1)
            {
                devider = " to ";
            }
            string narrationbuilder = devider + ddAccHeadCr.SelectedItem.Text.Trim();
            if (ddAccHeadDr.SelectedValue != "" && ddAccHeadCr.SelectedValue != "")
            {
                txtDescription.Text = ddAccHeadDr.SelectedItem.Text.Trim() + narrationbuilder;
            }
        }
    }


    private void AutoTransaction(string invoiceNo, string headId, string head5, string Name5, string amt, string side, string desc)
    {
        string type = "";
        if (headId == "010101002") // || headId == "040301001" || headId == "040302001")//Bank
        {
            type = "Bank";
        }
        else if (headId == "010104001" || headId == "030101001")//Customer
        {
            type = "Customer";
        }
        else if (headId == "020102006")//Suppliers
        {
            type = "Supplier";
        }

        string dr = "0", cr = amt;
        if (side == "Dr")
        {
            cr = "0"; dr = amt;
        }

        if (type != "")
        {
            VoucherEntry.TransactionEntry(invoiceNo, txtDate.Text, head5, Name5, desc, dr, cr, "0", "Voucher", type, headId, Page.User.Identity.Name, "1");
        }

    }
    private void BindItemGrid()
    {
        if (btnSave.Text == "Update")
        {
            SqlCommand cmd = new SqlCommand(@"SELECT [SerialNo], [VoucherRowDescription], AccountsHeadDr, AccountsHeadDrName+ '<br/>'+ Name5Dr AS AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName+ '<br/>'+ Name5Cr AS AccountsHeadCrName, Amount, Qty, Pcs, rate 
                                            FROM [VoucherTmp] WHERE (VoucherNo ='" + txtEditVoucherNo.Text + "') ORDER BY [SerialNo]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            GridView2.EmptyDataText = "No data added ...";
            GridView2.DataSource = cmd.ExecuteReader();
            GridView2.DataBind();
            cmd.Connection.Close();

            txtTTL.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp where VoucherNo='" + txtEditVoucherNo.Text + "'");
        }
        else
        {
            SqlCommand cmd = new SqlCommand(@"SELECT [SerialNo], [VoucherRowDescription], AccountsHeadDr, AccountsHeadDrName+ '<br/>'+ Name5Dr AS AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName+ '<br/>'+ Name5Cr AS AccountsHeadCrName, Amount, Qty, Pcs, rate 
                                            FROM [VoucherTmp] WHERE (VoucherNo ='" + txtEditVoucherNo.Text + "') AND ([EntryBy] ='" + Page.User.Identity.Name + "') ORDER BY [SerialNo]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            GridView2.EmptyDataText = "No data added ...";
            GridView2.DataSource = cmd.ExecuteReader();
            GridView2.DataBind();
            cmd.Connection.Close();

            txtTTL.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp where VoucherNo='" + txtEditVoucherNo.Text + "'  AND EntryBy='" + Page.User.Identity.Name.ToString() + "'");
        }
    }

    private void BindItemGrid4Edit()
    {
        SqlCommand cmd = new SqlCommand(@"SELECT [SerialNo], [VoucherRowDescription], AccountsHeadDr, AccountsHeadDrName+ '<br/>'+ Name5Dr AS AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName+ '<br/>'+ Name5Cr AS AccountsHeadCrName, Amount, Qty, Pcs, rate 
                                            FROM [VoucherTmp] WHERE VoucherNo ='" + txtEditVoucherNo.Text + "' ORDER BY [SerialNo]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        GridView2.EmptyDataText = "No data added ...";
        GridView2.DataSource = cmd.ExecuteReader();
        GridView2.DataBind();
        cmd.Connection.Close();

        txtTTL.Text = SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM VoucherTmp WHERE VoucherNo='" + txtEditVoucherNo.Text + "'");
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Restrict User
            string permLevel = SQLQuery.ReturnString("Select UserLevel from Logins where LoginUserName='" + User.Identity.Name + "'");
            if (Convert.ToInt32(permLevel) > 1) //Permitted Only for super Admin
            {
                Notify("You have not authorized for <b>EDITING</b> the voucher!", "error", lblMsg);
            }
            else
            {
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;

                if (lblId != null)
                {
                    DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) VID, VoucherNo, VoucherDate, VoucherDescription, 
                    ParticularID, VoucherEntryBy, VoucherEntryDate, Voucherpost, VoucherPostby, Voucherpostdate, VoucherReferenceNo, VoucherAmount 
                    FROM VoucherMaster WHERE (VoucherNo = '" + lblId.Text + "')");

                    foreach (DataRow drx in dtx.Rows)
                    {
                        txtEditVoucherNo.Text = drx["VoucherNo"].ToString();
                        ddParticular.SelectedValue = drx["ParticularID"].ToString();
                        PopulateSubAcc();
                        txtTTL.Text = drx["VoucherAmount"].ToString();
                    }
                }

                BindItemGrid4Edit();
                btnSave.Text = "Update";
                Notify("Voucher Entry is in edit mode!", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            //Restrict User
            string permLevel = SQLQuery.ReturnString("Select UserLevel from Logins where LoginUserName='" + User.Identity.Name + "'");
            if (Convert.ToInt32(permLevel) > 1) //Permitted Only for super Admin
            {
                Notify("You are not authorized for <b>DELETING</b> the voucher!", "error", lblMsg);
            }
            else
            {
                int index = Convert.ToInt32(e.RowIndex);
                Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

                string lName = Page.User.Identity.Name.ToString();
                string dateNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (lblItemCode != null)
                {
                    string voucherNo = lblItemCode.Text;
                    SQLQuery.ExecNonQry("Update VoucherMaster Set Voucherpost='C', VoucherPostby= '" + lName + "', Voucherpostdate='" + dateNow + "' where VoucherNo='" + voucherNo + "'");
                    SQLQuery.ExecNonQry("Update VoucherDetails Set ISApproved='C' where VoucherNo='" + voucherNo + "'");
                    //SQLQuery.ExecNonQry("Delete Transactions where InvNo='" + voucherNo + "'");
                    SQLQuery.ExecNonQry("Delete Stock where InvoiceID='" + voucherNo + "'");

                    GridView1.DataBind();
                    Notify("The Stock Transaction " + voucherNo + " has been cancelled successfully!", "warn", lblMsg);
                }
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    protected void ddControlDr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDr();
    }

    private void LoadDr()
    {
        ////SQLQuery.PopulateDropDown("Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID='" + ddControlDr.SelectedValue + "' ", ddAccHeadDr, "AccountsHeadID", "AccountsHeadName");
        //if (ddParticular.SelectedValue == "0104")
        //{
        //    SQLQuery.PopulateDropDown("Select pid, ProductName from FinishedProducts WHERE CompanyID='" + ddControlDr.SelectedValue + "' ", ddAccHeadDr, "pid", "ProductName");
        //}
        //else
        //{
            //SQLQuery.PopulateDropDown("Select ProductID, ItemName from Products WHERE CategoryID='" + ddControlDr.SelectedValue + "' ", ddAccHeadDr, "ProductID", "ItemName");
            SQLQuery.PopulateDropDown("Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID='" + ddControlDr.SelectedValue + "' ", ddAccHeadDr, "AccountsHeadID", "AccountsHeadName");
            Load5ThDr(ddHead5Dr, ddAccHeadDr);
        //}
        //Load5ThHead(ddHead5Dr, ddAccHeadDr);
        
    }
    protected void ddControlCr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCr();
    }
    private void LoadCr()
    {
        SQLQuery.PopulateDropDown("Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID='" + ddControlCr.SelectedValue + "' ", ddAccHeadCr, "AccountsHeadID", "AccountsHeadName");
        //Load5ThHead(ddHead5Cr, ddAccHeadCr);
        Load5ThCr(ddHead5Cr, ddAccHeadCr);
    }

}