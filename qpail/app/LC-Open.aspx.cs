using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms.VisualStyles;
using Accounting;
using RunQuery;

public partial class app_LC_Open : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");

        try
        {
            if (!IsPostBack)
            {
                string lName = Page.User.Identity.Name.ToString();
                lblLogin.Text = lName;

                SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Connection.Open();
                lblProject.Text = "1"; //Convert.ToString(cmd.ExecuteScalar());
                cmd.Connection.Close();

                txtExpiryDate.Text = DateTime.Now.ToShortDateString();
                txtShipDate.Text = DateTime.Now.ToShortDateString();
                txtArrivalDt.Text = DateTime.Now.ToShortDateString();
                txtDeliveryDt.Text = DateTime.Now.ToShortDateString();

                LoadFormInfo();

                ddGroup.DataBind();
                string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text +
                                "' ORDER BY [CategoryName]";
                SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

                gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue +
                         "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
                SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

                gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue +
                         "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
                SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
                GetProductList();

                ddSuppCategory.DataBind();
                ddManufacturer.DataBind();

                //Load Old LC
                int loadSl = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(max(sl),0) from LC"));
                LoadOldData(loadSl);
                GridView2.DataBind();
                //txtName.Focus();

                LoadItemsPanel();


                string formtype = Request.QueryString["type"];
                if (formtype == "edit")
                {
                    ddInvoice.DataBind();
                    txtEditLcNo.Text = ddInvoice.SelectedItem.Text;
                    txtNo.Text = txtEditLcNo.Text;

                    divinvoice.Attributes.Remove("Class");
                    divinvoice.Attributes.Add("Class", "control-group");
                    Page.Title = "LC Amendment";

                    //LCNoInput.Attributes.Remove("class");
                    //LCNoInput.Attributes.Add("class", "control-group hidden");
                    ltrLcNo.Text = "Edit LC No.";

                    ltrFrmName.Text = "LC Amendment";
                    ltrSubFrmName.Text = "Edit LC Information";

                    btnSave.Text = "Update LC Info";
                    LoadEditMode(ddInvoice.SelectedValue);


                    
                    RightPanel1.Visible = false;
                    RightPanel2.Visible = true;
                    GridView3.DataBind();
                }

            }

        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = ex.ToString();
        }
        finally
        {
            BindItemGrid();
        }
    }

    //Message or Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error                     
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    private void LoadEditMode(string inv)
    {
        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) sl, LCNo, LCType, OpenDate, Category, LCItem, HSCode, BanksId, BranchId, LcRef, LcFor, ExpiryDate, ShipDate, ArrivalDate, DeliveryDate, SupplierID, Origin, AgentID, InsuranceID, InsuranceNo, CnfID, BankId, LcMargin, LTR, BankExcRate, CustomExcRate, QtyUnit, TotalQty, Freight, CfrUSD, CfrBDT, BankBDT, Remarks, TransportMode, ForeignBank, OpeningBankCharge, ControlAccountsID, AccountsHeadID FROM [LC] WHERE (sl= '" + inv + "')");
        foreach (DataRow drx in dtx.Rows)
        {

            txtEditLcNo.Text = drx["LCNo"].ToString();
            txtNo.Text = drx["LCNo"].ToString();

            ddType.SelectedValue = drx["LCType"].ToString();
            txtDate.Text = drx["OpenDate"].ToString().Substring(0, 10);
            try
            {
                ddSuppCategory.SelectedValue = drx["LCItem"].ToString();
            }
            catch { }
            //HSCode.Text = drx["HSCode"].ToString();
            //LcRef.Text = drx["LcRef"].ToString();

            //txtFor.Text = drx["LcFor"].ToString();
            ddCompany.SelectedValue = drx["LcFor"].ToString();
            txtExpiryDate.Text = drx["ExpiryDate"].ToString().Substring(0, 10);
            txtShipDate.Text = drx["ShipDate"].ToString().Substring(0, 10);
            txtArrivalDt.Text = drx["ArrivalDate"].ToString().Substring(0, 10);
            txtDeliveryDt.Text = drx["DeliveryDate"].ToString().Substring(0, 10);

            string suppId = drx["SupplierID"].ToString();
            //ddSuppCategory.DataBind();
            //ddSuppCategory.SelectedValue = drx["Category"].ToString();
            //ddSuppCategory.SelectedValue = SQLQuery.ReturnString("Select Category from Party where PartyID='" + suppId + "'");
            ddManufacturer.DataBind(); try
            {
                ddManufacturer.SelectedValue = suppId;
            }
            catch { }
            ddCountry.SelectedValue = drx["Origin"].ToString();
            //ddAgentCategory.DataBind();
            //ddAgentCategory.SelectedValue = SQLQuery.ReturnString("Select Category from Party where PartyID='" + ddAgent.SelectedValue + "'");
            ddAgent.DataBind(); try
            {
                ddAgent.SelectedValue = drx["AgentID"].ToString();
            }
            catch { }
            ddInsurance.SelectedValue = drx["InsuranceID"].ToString();
            txtInsurNo.Text = drx["InsuranceNo"].ToString();
            ddCNF.SelectedValue = drx["CnfID"].ToString();
            ddBank.DataBind();
            ddBank.SelectedValue = drx["BanksId"].ToString();
            ddBankBranch.DataBind();
            ddBankBranch.SelectedValue = drx["BranchId"].ToString();
            ddBancAcc.SelectedValue = drx["BankId"].ToString();

            //ItemGrid.DataBind();
            BindItemGrid();

            txtBExRate.Text = SQLQuery.Make2Decimal(drx["BankExcRate"].ToString());
            txtCExRate.Text = SQLQuery.Make2Decimal(drx["CustomExcRate"].ToString());
            //QtyUnit.Text = drx["QtyUnit"].ToString();
            txtTtlQty.Text = drx["TotalQty"].ToString();
            txtFreight.Text = drx["Freight"].ToString();
            txtCfrUSD.Text = drx["CfrUSD"].ToString();
            txtCfrBDT.Text = drx["CfrBDT"].ToString();
            txtBankBDT.Text = drx["BankBDT"].ToString();
            txtMargin.Text = drx["LcMargin"].ToString();
            txtLTR.Text = drx["LTR"].ToString();
            txtForeignBank.Text = drx["ForeignBank"].ToString();
            txtOpeningBankCrg.Text = drx["OpeningBankCharge"].ToString();

            txtRemarks.Text = drx["Remarks"].ToString();
            ddMode.SelectedValue = drx["TransportMode"].ToString();
            ddAdvanceAginstControl.DataBind();
            ddAdvanceAginstControl.SelectedValue = drx["ControlAccountsID"].ToString();
            ddAdvanceAginstHead.DataBind();
            ddAdvanceAginstHead.SelectedValue = drx["AccountsHeadID"].ToString();
            //ddAdvanceAginstHead.DataBind(); try
            //{
            //    ddAdvanceAginstHead.SelectedValue = drx["AccountsHeadID"].ToString();
            //}
            //catch { }
        }
    }

    private void LoadItemsPanel()
    {
        if (ddSubGrp.SelectedValue == "10")
        {
            pnlSpec.Visible = true;
            LoadSpecList("filter");
        }
        else
        {
            pnlSpec.Visible = false;
        }

        string subGrp = "";
        if (ddSubGrp.SelectedValue != "")
        {
            subGrp = ddSubGrp.SelectedItem.Text;
        }

        if (subGrp == "Tin Plate")
        {
            pkSizeField.Attributes.Remove("class");
            thicknessField.Attributes.Remove("class");
            measurementField.Attributes.Remove("class");

            pkSizeField.Attributes.Add("class", "control-group");
            thicknessField.Attributes.Add("class", "control-group");
            measurementField.Attributes.Add("class", "control-group");

            ltrThickness.Text = "Thickness :";
            ltrMeasurement.Text = "Measurement :";
        }

        else if (ddGroup.SelectedItem.Text == "Raw Materials")
        {
            pkSizeField.Attributes.Remove("class");

            thicknessField.Attributes.Remove("class");
            measurementField.Attributes.Remove("class");

            pkSizeField.Attributes.Add("class", "control-group hidden");
            thicknessField.Attributes.Add("class", "control-group hidden");
            measurementField.Attributes.Add("class", "control-group hidden");
        }
        else
        {
            pkSizeField.Attributes.Remove("class");
            thicknessField.Attributes.Remove("class");
            measurementField.Attributes.Remove("class");

            pkSizeField.Attributes.Add("class", "control-group hidden");
            thicknessField.Attributes.Add("class", "control-group");
            measurementField.Attributes.Add("class", "control-group");

            pkSizeField.Attributes.Add("class", "control-group hidden");
            ltrThickness.Text = "Serial No. :";
            ltrMeasurement.Text = "Model No. :";
        }

        int iGrp = Convert.ToInt32(ddGroup.SelectedValue);
        if (iGrp <= 3)
        {
            //ddStockType.SelectedValue = "Raw";
            ltrWarrenty.Text = "Qnty./Pack (Kg): ";
            ltrSerial.Text = "No. of Packs: ";
            PanelWarrenty.Visible = true;

            subGrp = "";
            if (ddSubGrp.SelectedValue != "")
            {
                subGrp = ddSubGrp.SelectedItem.Text;
            }

            if (subGrp == "Tin Plate")
            {
                pkSizeField.Attributes.Remove("class");
                pkSizeField.Attributes.Add("class", "control-group");
            }
            else
            {
                pkSizeField.Attributes.Remove("class");
                pkSizeField.Attributes.Add("class", "control-group hidden");
                //SectionField.Attributes.Remove("class");
                //SectionField.Attributes.Add("class", "control-group hidden");
            }

        }
        else
        {
            //PanelWarrenty.Visible = false;
            //ddStockType.SelectedValue = "Fixed";
            ltrWarrenty.Text = "Warranty : ";
            ltrSerial.Text = "Specification : ";
        }

        if (ddGroup.SelectedValue == "5")
        {
            //SectionField.Attributes.Remove("class");
            //SectionField.Attributes.Add("class", "control-group");
        }

        GenerateHSCode();

    }

    private void GenerateHSCode()
    {
        //Get HS Code from item Grade
        txtHSCode.Text =
            SQLQuery.ReturnString("Select TOP(1)  HSCode from LcItems where ItemCode='" +
                                           ddItemName.SelectedValue + "' AND HSCode<>'' Order by EntryID desc ");
        if (txtHSCode.Text == "")
        {
            txtHSCode.Text =
                SQLQuery.ReturnString(
                    "SELECT  TOP(1) HSCode from LcItems where ItemCode IN (SELECT ProductID FROM Products WHERE CategoryID IN (SELECT CategoryID FROM Categories WHERE GradeID ='" +
                    ddGrade.SelectedValue + "')) AND HSCode<>'' Order by EntryID desc ");
        }
    }

    public static void ClearControls(Control Parent)
    {
        //Code for Clearing the form
        if (Parent is TextBox)
        {
            (Parent as TextBox).Text = string.Empty;
        }
        else
        {
            foreach (Control c in Parent.Controls) ClearControls(c);
        }
    }

    private void LoadFormInfo()
    {
        //string projectID = Convert.ToString(Session["ProjectID"]);
        //string formtype = Request.QueryString["type"];
        lblProject.Text = "1";

        //if (formtype == "create")
        //{
        ltrFrmName.Text = "LC General Information";
        ltrSubFrmName.Text = "Create New LC";
        txtDate.Text = DateTime.Now.ToShortDateString();
        txtNo.Focus();
        //}
        //else if (formtype == "edit")
        //{
        //    EditField.Attributes.Remove("class");
        //    EditField.Attributes.Add("class", "control-group");
        //    LCNoInput.Attributes.Remove("class");
        //    LCNoInput.Attributes.Add("class", "control-group hidden");
        //    ltrFrmName.Text = "LC Amendment";
        //    ltrSubFrmName.Text = "Edit LC Information";

        //    btnSave.Text = "Update LC Info";
        //    //EditMode();
        //}
        //else
        //{
        //    btnSave.Visible = false;
        //}

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtNo.Text != "")
            {
                string msghead = SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup WHERE ControlAccountsID='" + ddAdvanceAginstControl.SelectedValue + "' AND AccountsHeadID='" + ddAdvanceAginstHead.SelectedValue + "'");
                string isExistHead = SQLQuery.ReturnString("Select AccountsHeadID from LC WHERE AccountsHeadID='" + ddAdvanceAginstHead.SelectedValue + "'");
                if (isExistHead == "")
                {
                    if (btnSave.Text == "Update" && ddInvoice.Visible == true)
                    {
                        //UpdateInfo();
                        RefreshForm();

                        Notify("Info successfully updated for " + ddInvoice.SelectedItem.Text, "success", lblMsg);
                    }
                    else
                    {
                        if (txtEditLcNo.Text == "")
                        {
                            Saveinfo(txtNo.Text, "Save");
                            RefreshForm();
                            //Load Last LC
                            int loadSl = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(max(sl),0) from LC"));
                            LoadOldData(loadSl);
                            GridView2.DataBind();
                            Notify("New LC Saved Successfully.", "success", lblMsg);
                        }
                        else
                        {
                            SaveAmmendInfo();
                            SQLQuery.ExecNonQry("Delete LC where LCNo='" + txtEditLcNo.Text + "'");
                            Saveinfo(txtEditLcNo.Text, "Update");

                            //RefreshForm();
                            ddInvoice.DataBind();
                            LoadEditMode(ddInvoice.SelectedValue);
                            //Load Last LC
                            //int loadSl = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(max(sl),0) from LC"));
                            //LoadOldData(loadSl);
                            GridView3.DataBind();
                            Notify("LC Ammendment Saved Successfully.", "success", lblMsg);
                        }
                    }
                }
                else
                {
                    Notify("Account head -" + msghead + "- already exist !", "error", lblMsg);
                }
            }
            else
            {
                Notify("Please Enter LC No.", "warn", lblMsg);
            }

        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }

    private void Saveinfo(string LcNo, string action)
    {
       
            string description;

            txtNo.Text = LcNo.Trim();
            string formtype = base.Request.QueryString["type"];
            int projectID = Convert.ToInt32(lblProject.Text);
            string lName = Page.User.Identity.Name.ToString();
            string arrivalDt = Convert.ToDateTime(txtArrivalDt.Text).ToString("yyyy-MM-dd");
            string deliveryDt = Convert.ToDateTime(txtDeliveryDt.Text).ToString("yyyy-MM-dd");

            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO LC (LCNo, LCType, OpenDate, Category, LCItem, HSCode, LcRef, LcFor, ShipDate, ExpiryDate, SupplierID, Origin, AgentID, InsuranceID, InsuranceNo, CnfID, BanksId, BranchId, BankId, ControlAccountsID, AccountsHeadID, LcMargin, " +
                "ForeignBank, LTR, OpeningBankCharge, BankExcRate, CustomExcRate, QtyUnit, TotalQty, Freight, CfrUSD, CfrBDT, BankBDT, Remarks, TransportMode, LCCloseBy, LCClosedate, ProjectID, EntryBy)" +
                "VALUES (@LCNo, @LCType, @OpenDate, @Category, @LCItem, @HSCode, @LcRef, @LcFor, @ShipDate, @ExpiryDate, @SupplierID, @Origin, @AgentID, @InsuranceID, @InsuranceNo , @CnfID, @BanksId, @BranchId, @BankId, @ControlAccountsID, @AccountsHeadID, @LcMargin, " +
                "@ForeignBank, @LTR, @OpeningBankCharge, @BankExcRate, @CustomExcRate, @QtyUnit, @TotalQty, @Freight, @CfrUSD, @CfrBDT, @BankBDT, @Remarks, @TransportMode, @LCCloseBy, @LCClosedate, @ProjectID, @EntryBy)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@LCNo", txtNo.Text);
            cmd.Parameters.AddWithValue("@LCType", ddType.SelectedValue);
            cmd.Parameters.AddWithValue("@OpenDate", Convert.ToDateTime(txtDate.Text));
            cmd.Parameters.AddWithValue("@Category", ddSuppCategory.SelectedValue);
            cmd.Parameters.AddWithValue("@LCItem", ddSuppCategory.SelectedValue);
            cmd.Parameters.AddWithValue("@HSCode", txtHSCode.Text);
            cmd.Parameters.AddWithValue("@LcRef", ""); // txtReferrence.Text);
                                                       //cmd.Parameters.AddWithValue("@LcFor", txtFor.Text); // BanksId, BranchId
            cmd.Parameters.AddWithValue("@LcFor", ddCompany.SelectedValue);

            cmd.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(txtExpiryDate.Text));
            cmd.Parameters.AddWithValue("@ShipDate", Convert.ToDateTime(txtShipDate.Text));
            cmd.Parameters.AddWithValue("@SupplierID", ddManufacturer.SelectedValue);
            cmd.Parameters.AddWithValue("@Origin", ddCountry.SelectedValue);

            cmd.Parameters.AddWithValue("@AgentID", ddAgent.SelectedValue);
            cmd.Parameters.AddWithValue("@InsuranceID", ddInsurance.SelectedValue);
            cmd.Parameters.AddWithValue("@InsuranceNo", txtInsurNo.Text);
            cmd.Parameters.AddWithValue("@CnfID", ddCNF.SelectedValue);
            cmd.Parameters.AddWithValue("@BanksId", ddBank.SelectedValue);
            cmd.Parameters.AddWithValue("@BranchId", ddBankBranch.SelectedValue);
            cmd.Parameters.AddWithValue("@BankId", ddBancAcc.SelectedValue);
            cmd.Parameters.AddWithValue("@ControlAccountsID", ddAdvanceAginstControl.SelectedValue);
            cmd.Parameters.AddWithValue("@AccountsHeadID", ddAdvanceAginstHead.SelectedValue);
            cmd.Parameters.AddWithValue("@LcMargin", txtMargin.Text);
            cmd.Parameters.AddWithValue("@LTR", txtLTR.Text);
            cmd.Parameters.AddWithValue("@ForeignBank", txtForeignBank.Text);
            cmd.Parameters.AddWithValue("@OpeningBankCharge", txtOpeningBankCrg.Text);

            cmd.Parameters.AddWithValue("@BankExcRate", txtBExRate.Text);
            cmd.Parameters.AddWithValue("@CustomExcRate", txtCExRate.Text);
            cmd.Parameters.AddWithValue("@QtyUnit", txtQty.Text);
            cmd.Parameters.AddWithValue("@TotalQty", txtTtlQty.Text);
            cmd.Parameters.AddWithValue("@Freight", txtFreight.Text);

            cmd.Parameters.AddWithValue("@CfrUSD", txtCfrUSD.Text);
            cmd.Parameters.AddWithValue("@CfrBDT", txtCfrBDT.Text); //
            cmd.Parameters.AddWithValue("@BankBDT", txtBankBDT.Text);
            cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
            cmd.Parameters.AddWithValue("@TransportMode", ddMode.Text);
            cmd.Parameters.AddWithValue("@LCCloseBy", "");
            cmd.Parameters.AddWithValue("@LCClosedate", "");
            cmd.Parameters.AddWithValue("@ProjectID", projectID);
            cmd.Parameters.AddWithValue("@EntryBy", lName);

            cnn.Open();
            int Success = cmd.ExecuteNonQuery();
            cnn.Close();

            SQLQuery.ExecNonQry("UPDATE LC SET  ArrivalDate='" + arrivalDt + "', DeliveryDate='" + deliveryDt +
                                         "' where LCNo ='" + txtNo.Text + "'");

            SqlCommand cmd2 = new SqlCommand("UPDATE LcItems SET LCNo='" + txtNo.Text + "' where (LCNo ='')",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
            cmd2.Connection.Dispose();

            if (Convert.ToDecimal(txtMargin.Text) > 0)
            {
                string lcSL = RunQuery.SQLQuery.ReturnString("Select sl FROM LC where LCNo='" + txtNo.Text + "'");
                description = txtMarginPercent.Text + "Margin Paid for LC# " + txtNo.Text + ", LC value : " + txtCfrUSD.Text + " Exchange Rate : " + txtBExRate.Text;
                VoucherEntry.TransactionEntry(lcSL, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), ddAdvanceAginstHead.SelectedValue, ddAdvanceAginstHead.SelectedItem.Text, description, txtMargin.Text, "0", "0", "ImportLC", "LC", "1122334455", lName, "1");
                VoucherEntry.AutoVoucherEntry("11", description, ddAdvanceAginstHead.SelectedValue, "010101002", Convert.ToDecimal(txtMargin.Text), lcSL, "", lName, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), "1");
            }
            if (Convert.ToDecimal(txtOpeningBankCrg.Text) > 0)
            {
                string lcSL = RunQuery.SQLQuery.ReturnString("Select sl FROM LC where LCNo='" + txtNo.Text + "'");
                description = "Bank Opening Charge Paid for LC# " + txtNo.Text + ", LC value : " + txtCfrUSD.Text;
                VoucherEntry.TransactionEntry(lcSL, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), ddAdvanceAginstHead.SelectedValue, ddAdvanceAginstHead.SelectedItem.Text, description, txtOpeningBankCrg.Text, "0", "0", "ImportLC", "LC", "1122334455", lName, "1");
                VoucherEntry.AutoVoucherEntry("11", description, ddAdvanceAginstHead.SelectedValue, "010101002", Convert.ToDecimal(txtOpeningBankCrg.Text), lcSL, "", lName, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), "1");
            }

            if (action == "Save")
            {
                SQLQuery.ExecNonQry("Insert into LC_Duty_Calc (LCNo) VALUES ('" + txtNo.Text + "')");
                SQLQuery.ExecNonQry("Insert into LC_CNFTax_Calc (LCNo) VALUES ('" + txtNo.Text + "')");
                SQLQuery.ExecNonQry("Insert into LC_Insur_Calc (LCNo) VALUES ('" + txtNo.Text + "')");
                SQLQuery.ExecNonQry("Insert into LC_Bank_Calc (LCNo, CFRUSD, ExchRate, CFRBDT, LTR, Margin) VALUES ('" + txtNo.Text + "', " +
                    txtCfrUSD.Text + ", " + txtBExRate.Text + ", " + txtBankBDT.Text + ", " + txtLTR.Text + ", " + txtMargin.Text + ")");
            }
            else
            {
                SQLQuery.ExecNonQry("UPDATE LC_Amendment SET LCNo='" + txtNo.Text + "' WHERE LCNo='" + txtEditLcNo.Text + "'");
                SQLQuery.ExecNonQry("UPDATE LC_Duty_Calc SET LCNo='" + txtNo.Text + "' WHERE LCNo='" + txtEditLcNo.Text + "'");
                SQLQuery.ExecNonQry("UPDATE LC_CNFTax_Calc SET LCNo='" + txtNo.Text + "' WHERE LCNo='" + txtEditLcNo.Text + "'");
                SQLQuery.ExecNonQry("UPDATE LC_Insur_Calc SET LCNo='" + txtNo.Text + "' WHERE LCNo='" + txtEditLcNo.Text + "'");
                SQLQuery.ExecNonQry("UPDATE LC_Bank_Calc SET LCNo='" + txtNo.Text + "' WHERE LCNo='" + txtEditLcNo.Text + "'");
                SQLQuery.ExecNonQry("UPDATE LChistory SET LCNo='" + txtNo.Text + "' WHERE LCNo='" + txtEditLcNo.Text + "'");
            }
        
       
        
    }

    private string PartName(string partyId)
    {
        return SQLQuery.ReturnString("Select Company from Party where PartyID='" + partyId + "'");
    }

    private string BankName(string partyId)
    {
        return SQLQuery.ReturnString("Select BankName from Banks where BankId='" + partyId + "'");
    }

    private void SaveAmmendInfo()
    {
        string lName = Page.User.Identity.Name.ToString();
        //get old data first
        string oldCompany = "", oldZone = "", oldBal = "", oldDueLimit = "", oldRef = "";
        SqlCommand cmd7 = new SqlCommand("Select LCNo, LCType, OpenDate, Category, LcFor, ShipDate, ExpiryDate," +
            " SupplierID, Origin, AgentID, InsuranceID, CnfID, BankId, LcMargin, LTR, BankExcRate, CustomExcRate, QtyUnit, TotalQty, Freight, CfrUSD, CfrBDT, Remarks, TransportMode, LCCloseBy, LCClosedate, ProjectID, EntryBy, BankBDT, ArrivalDate, DeliveryDate, BanksId, BranchId, ControlAccountsID, AccountsHeadID, ForeignBank, OpeningBankCharge FROM [LC] WHERE LCNo=@LCNo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Parameters.Add("@LCNo", SqlDbType.VarChar).Value = txtEditLcNo.Text;
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();

        if (dr.Read())
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
            SQLQuery.Empty2Zero(txtBankBDT);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO LChistory (LCNo, LCType, OpenDate, Category, LCItem, HSCode, LcRef, LcFor, ShipDate, ExpiryDate, SupplierID, Origin, AgentID, InsuranceID, CnfID, BankId, LcMargin, " +
                                                "LTR, BankExcRate, CustomExcRate, QtyUnit, TotalQty, Freight, CfrUSD, CfrBDT, BankBDT, Remarks, TransportMode, LCCloseBy, LCClosedate, ProjectID, EntryBy, OpeningBankCharge, ForeignBank, ControlAccountsID, AccountsHeadID)" +
                                    "VALUES (@LCNo, @LCType, @OpenDate, @Category, @LCItem, @HSCode, @LcRef, @LcFor, @ShipDate, @ExpiryDate, @SupplierID, @Origin, @AgentID, @InsuranceID, @CnfID, @BankId, @LcMargin, " +
                                    "@LTR, @BankExcRate, @CustomExcRate, @QtyUnit, @TotalQty, @Freight, @CfrUSD, @CfrBDT, @BankBDT, @Remarks, @TransportMode, @LCCloseBy, @LCClosedate, @ProjectID, @EntryBy, @OpeningBankCharge, @ForeignBank,  @ControlAccountsID, @AccountsHeadID)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@LCNo", dr["LCNo"].ToString());
            cmd.Parameters.AddWithValue("@LCType", dr["LCType"].ToString());
            cmd.Parameters.AddWithValue("@OpenDate", Convert.ToDateTime(dr["OpenDate"].ToString()));
            cmd.Parameters.AddWithValue("@Category", dr["Category"].ToString());
            cmd.Parameters.AddWithValue("@LCItem", "0");//txtItem.Text);
            cmd.Parameters.AddWithValue("@HSCode", txtHSCode.Text);
            cmd.Parameters.AddWithValue("@LcRef", "0");// txtReferrence.Text);
            cmd.Parameters.AddWithValue("@LcFor", dr["LcFor"].ToString());
            cmd.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(dr["ExpiryDate"].ToString()));
            cmd.Parameters.AddWithValue("@ShipDate", Convert.ToDateTime(dr["ShipDate"].ToString()));
            cmd.Parameters.AddWithValue("@SupplierID", dr["SupplierID"].ToString());
            cmd.Parameters.AddWithValue("@Origin", dr["Origin"].ToString());

            cmd.Parameters.AddWithValue("@AgentID", dr["AgentID"].ToString());
            cmd.Parameters.AddWithValue("@InsuranceID", dr["InsuranceID"].ToString());
            cmd.Parameters.AddWithValue("@CnfID", dr["CnfID"].ToString());
            cmd.Parameters.AddWithValue("@BankId", dr["BankId"].ToString());
            cmd.Parameters.AddWithValue("@LcMargin", Convert.ToDecimal(dr["LcMargin"].ToString()));
            cmd.Parameters.AddWithValue("@LTR", Convert.ToDecimal(dr["LTR"].ToString()));
            cmd.Parameters.AddWithValue("@ForeignBank", Convert.ToDecimal(dr["ForeignBank"].ToString()));
            cmd.Parameters.AddWithValue("@OpeningBankCharge", Convert.ToDecimal(dr["OpeningBankCharge"].ToString()));
            cmd.Parameters.AddWithValue("@ControlAccountsID", dr["ControlAccountsID"].ToString());
            cmd.Parameters.AddWithValue("@AccountsHeadID", dr["AccountsHeadID"].ToString());

            cmd.Parameters.AddWithValue("@BankExcRate", Convert.ToDecimal(dr["BankExcRate"].ToString()));
            cmd.Parameters.AddWithValue("@CustomExcRate", Convert.ToDecimal(dr["CustomExcRate"].ToString()));
            cmd.Parameters.AddWithValue("@QtyUnit", dr["QtyUnit"].ToString());
            cmd.Parameters.AddWithValue("@TotalQty", dr["TotalQty"].ToString());
            cmd.Parameters.AddWithValue("@Freight", Convert.ToDecimal(dr["Freight"].ToString()));

            cmd.Parameters.AddWithValue("@CfrUSD", Convert.ToDecimal(dr["CfrUSD"].ToString()));
            cmd.Parameters.AddWithValue("@CfrBDT", Convert.ToDecimal(dr["CfrBDT"].ToString()));
            cmd.Parameters.AddWithValue("@BankBDT", Convert.ToDecimal(txtBankBDT.Text));
            cmd.Parameters.AddWithValue("@Remarks", dr["Remarks"].ToString());
            cmd.Parameters.AddWithValue("@TransportMode", dr["TransportMode"].ToString());
            cmd.Parameters.AddWithValue("@LCCloseBy", "");
            cmd.Parameters.AddWithValue("@LCClosedate", "");
            cmd.Parameters.AddWithValue("@ProjectID", Convert.ToInt32(lblProject.Text));
            cmd.Parameters.AddWithValue("@EntryBy", lName);

            cnn.Open();
            int Success = cmd.ExecuteNonQuery();
            cnn.Close();

            //Amnd History log
            Save_LC_Log("LCNo", dr[0].ToString(), txtNo.Text);
            Save_LC_Log("LC Type", dr[1].ToString(), ddType.SelectedValue);
            Save_LC_Log("Open Date", Convert.ToDateTime(dr[2].ToString()).ToString("dd/MM/yyyy"), Convert.ToDateTime(txtDate.Text).ToString("dd/MM/yyyy"));
            Save_LC_Log("Category", dr[3].ToString(), ddSuppCategory.SelectedValue);

            //Save_LC_Log("For", dr[4].ToString(), txtFor.Text);
            Save_LC_Log("For", dr[4].ToString(), ddCompany.SelectedValue);
            Save_LC_Log("Expiry Date", Convert.ToDateTime(dr[6].ToString()).ToString("dd/MM/yyyy"), Convert.ToDateTime(txtExpiryDate.Text).ToString("dd/MM/yyyy"));
            Save_LC_Log("Ship Date", Convert.ToDateTime(dr[5].ToString()).ToString("dd/MM/yyyy"), Convert.ToDateTime(txtShipDate.Text).ToString("dd/MM/yyyy"));
            Save_LC_Log("Supplier", PartName(dr[7].ToString()) , PartName(ddManufacturer.SelectedValue));
            Save_LC_Log("Origin", dr[8].ToString(), ddCountry.SelectedValue);

            Save_LC_Log("Agent", PartName(dr[9].ToString()), PartName(ddAgent.SelectedValue));
            Save_LC_Log("Insurance", BankName(dr[10].ToString()), BankName(ddInsurance.SelectedValue));
            Save_LC_Log("CNF", PartName(dr[11].ToString()), PartName(ddCNF.SelectedValue));
            Save_LC_Log("Bank", BankName(dr[12].ToString()), BankName(ddBank.SelectedValue));
            Save_LC_Log_decimal("LC Margin", dr[13].ToString(), txtMargin.Text);
            Save_LC_Log_decimal("LTR", dr[14].ToString(), txtLTR.Text);

            Save_LC_Log_decimal("Bank Exc. Rate", dr[15].ToString(), txtBExRate.Text);
            Save_LC_Log_decimal("Custom Exc. Rate", dr[16].ToString(), txtCExRate.Text);
            //Save_LC_Log_decimal("Qty. Unit", dr[17].ToString(), txtQty.Text);
            Save_LC_Log_decimal("Total Qty.", dr[18].ToString(), txtTtlQty.Text);
            Save_LC_Log_decimal("Freight", dr[19].ToString(), txtFreight.Text);

            Save_LC_Log_decimal("CFR USD", dr[20].ToString(), txtCfrUSD.Text);
            Save_LC_Log_decimal("CFR BDT", dr[21].ToString(), txtCfrBDT.Text);
            Save_LC_Log_decimal("FOB/ Bank BDT", dr[28].ToString(), txtBankBDT.Text);
            Save_LC_Log("Remarks", dr[22].ToString(), txtRemarks.Text);
            Save_LC_Log("Transport Mode", dr[23].ToString(), ddMode.Text);
            Save_LC_Log("Expiry Date", Convert.ToDateTime(dr[29].ToString()).ToString("dd/MM/yyyy"), Convert.ToDateTime(txtArrivalDt.Text).ToString("dd/MM/yyyy"));
            Save_LC_Log("Ship Date", Convert.ToDateTime(dr[30].ToString()).ToString("dd/MM/yyyy"), Convert.ToDateTime(txtDeliveryDt.Text).ToString("dd/MM/yyyy"));
            
        }

        cmd7.Connection.Close();

        //Create Sql Connection
        SqlConnection cnn3 = new SqlConnection();
        cnn3.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        //Create Sql Command
        SqlCommand cmd3 = new SqlCommand();
        cmd3.CommandText = "UPDATE LC SET LCNo =@LCNo, LCType =@LCType,BanksId =@BanksId,BranchId =@BranchId, OpenDate =@OpenDate, Category =@Category, HSCode='" + txtHSCode.Text + "', LcFor =@LcFor, " +
             "ExpiryDate =@ExpiryDate, ShipDate =@ShipDate, SupplierID =@SupplierID, Origin =@Origin, AgentID =@AgentID, InsuranceID =@InsuranceID," +
              "CnfID =@CnfID, BankId =@BankId, LcMargin =@LcMargin, LTR =@LTR, BankExcRate =@BankExcRate, CustomExcRate =@CustomExcRate," +
               "QtyUnit =@QtyUnit, TotalQty =@TotalQty, Freight =@Freight, CfrUSD =@CfrUSD, CfrBDT =@CfrBDT, BankBDT='" + txtBankBDT.Text + "', " +
            " Remarks =@Remarks, TransportMode =@TransportMode WHERE sl='" + ddInvoice.SelectedValue + "'";

        cmd3.CommandType = CommandType.Text;
        cmd3.Connection = cnn3;

        cmd3.Parameters.AddWithValue("@LCNo", txtNo.Text);
        cmd3.Parameters.AddWithValue("@LCType", ddType.SelectedValue);
        cmd3.Parameters.AddWithValue("@OpenDate", Convert.ToDateTime(txtDate.Text));
        cmd3.Parameters.AddWithValue("@Category", ddSuppCategory.SelectedValue);
        //cmd3.Parameters.AddWithValue("@LcFor", txtFor.Text);
        cmd3.Parameters.AddWithValue("@LcFor", ddCompany.SelectedValue);
        cmd3.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(txtExpiryDate.Text));
        cmd3.Parameters.AddWithValue("@ShipDate", Convert.ToDateTime(txtShipDate.Text));
        cmd3.Parameters.AddWithValue("@SupplierID", ddManufacturer.SelectedValue);
        cmd3.Parameters.AddWithValue("@Origin", ddCountry.SelectedValue);

        cmd3.Parameters.AddWithValue("@AgentID", ddAgent.SelectedValue);
        cmd3.Parameters.AddWithValue("@InsuranceID", ddInsurance.SelectedValue);
        cmd3.Parameters.AddWithValue("@CnfID", ddCNF.SelectedValue);
        cmd3.Parameters.AddWithValue("@BankId", ddBancAcc.SelectedValue);
        cmd3.Parameters.AddWithValue("@BanksId", ddBank.SelectedValue);
        cmd3.Parameters.AddWithValue("@BranchId", ddBankBranch.SelectedValue);
        cmd3.Parameters.AddWithValue("@LcMargin", txtMargin.Text);
        cmd3.Parameters.AddWithValue("@LTR", txtLTR.Text);

        cmd3.Parameters.AddWithValue("@BankExcRate", txtBExRate.Text);
        cmd3.Parameters.AddWithValue("@CustomExcRate", txtCExRate.Text);
        cmd3.Parameters.AddWithValue("@QtyUnit", txtQty.Text);
        cmd3.Parameters.AddWithValue("@TotalQty", txtTtlQty.Text);
        cmd3.Parameters.AddWithValue("@Freight", txtFreight.Text);

        cmd3.Parameters.AddWithValue("@CfrUSD", txtCfrUSD.Text);
        cmd3.Parameters.AddWithValue("@CfrBDT", txtCfrBDT.Text);
        cmd3.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        cmd3.Parameters.AddWithValue("@TransportMode", ddMode.Text);

        cnn3.Open();
        cmd3.ExecuteNonQuery();
        cnn3.Close();

    }

    private void Save_LC_Log(string dataType, string oldValue, string newValue)
    {
        if (oldValue.Trim()!=newValue.Trim())
        {
            SQLQuery.ExecNonQry("Insert into LC_Amendment (LCNo, DataType, OldData, NewData, EntryBy) VALUES ('"+ ddInvoice.SelectedItem.Text + "','"+ dataType + "','"+ oldValue + "','"+ newValue + "','"+User.Identity.Name+"')");
        }
    }
    private void Save_LC_Log_decimal(string dataType, string oldValue, string newValue)
    {
        string formtype = Request.QueryString["type"];
        if (formtype == "edit")
        {
            if (Convert.ToDecimal(oldValue) != Convert.ToDecimal(newValue))
            {
                SQLQuery.ExecNonQry("Insert into LC_Amendment (LCNo, DataType, OldData, NewData, EntryBy) VALUES ('" +
                                    ddInvoice.SelectedItem.Text + "','" + dataType + "','" + Convert.ToDecimal(oldValue) +
                                    "','" + Convert.ToDecimal(newValue) + "','" + User.Identity.Name + "')");
            }
        }
    }
    protected void ddInvoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        //EditMode();
        GenerateHSCode();
        LoadSpecList("filter");
        LoadEditMode(ddInvoice.SelectedValue);
        GridView3.DataBind();
        BindItemGrid();
    }

    private void GridItemEditMode(string entryID)
    {
        try
        {
            //int index = Convert.ToInt32(ItemGrid.SelectedIndex);
            //Label lblItemName = ItemGrid.Rows[index].FindControl("Label1") as Label;
            //lblEntryId.Text = lblItemName.Text;

            SqlCommand cmd7 = new SqlCommand("Select ItemCode, HSCode, ItemSizeID, Thickness, Measurement, qty, UnitPrice, CFRValue, pcs FROM [LcItems] WHERE EntryID=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Parameters.Add("@sl", SqlDbType.VarChar).Value = entryID;
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();

            if (dr.Read())
            {
                //btnSave.Text = "Update";
                string iCode = dr[0].ToString();
                txtHSCode.Text = dr[1].ToString();

                txtThickness.Text = dr[3].ToString();
                txtMeasure.Text = dr[4].ToString();
                txtQty.Text = dr[5].ToString();
                txtPrice.Text = dr[6].ToString();
                txtCFR.Text = dr[7].ToString();
                txtWeight.Text = dr[8].ToString();

                //ddGroup.SelectedValue=SQLQuery.ReturnString("Select GroupID from ItemSubGroup where CategoryID=(Select CategoryID from ItemGrade where GradeID=(Select ))")

                string catID = SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + iCode + "'");
                string grdID = SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
                string subID = SQLQuery.ReturnString("SELECT CategoryID FROM ItemGrade WHERE GradeID ='" + grdID + "'");
                string grpID = SQLQuery.ReturnString("SELECT GroupID FROM ItemSubGroup WHERE CategoryID ='" + subID + "'");

                //ItemGrid.DataBind();
                //pnl.Update();
                btnAdd.Text = "Update";
            }

            cmd7.Connection.Close();

            DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) Purpose, GradeId, CategoryId, ItemCode, HSCode, 
                                ItemSizeID, Spec, Thickness, Measurement, qty, UnitPrice, CFRValue, ReturnQty, NoOfPacks, QntyPerPack, 
                                Loading, Loaded, LandingPercent, LandingAmt, TotalUSD, TotalBDT, UnitCostBDT  FROM   [LcItems] WHERE EntryID='" + lblEntryId.Text + "'");

            foreach (DataRow drx in dtx.Rows)
            {
                ddPurpose.SelectedValue = drx["Purpose"].ToString();
                string iCode = drx["ItemCode"].ToString();

                string grpID = Stock.Inventory.GetItemGroup(iCode);
                ddGroup.SelectedValue = grpID;
                string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + grpID +
                                "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
                SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");
                ddSubGrp.SelectedValue = Stock.Inventory.GetItemSubGroup(iCode);

                gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue +
                         "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
                SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
                ddGrade.SelectedValue = drx["GradeId"].ToString();

                gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue +
                         "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
                SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
                ddCategory.SelectedValue = drx["CategoryId"].ToString();

                GetProductList();

                ddItemName.SelectedValue = iCode;
                txtQty.Text = drx["qty"].ToString();
                string spec = drx["Spec"].ToString();

                string size = drx["ItemSizeID"].ToString();
                if (size != "0")
                {
                    ddSize.SelectedValue = size;
                }

                //ddSpec.SelectedValue = drx["Spec"].ToString();

                txtThickness.Text = drx["Thickness"].ToString();
                txtMeasure.Text = drx["Measurement"].ToString();
                txtSerial.Text = drx["NoOfPacks"].ToString();
                txtWarrenty.Text = drx["QntyPerPack"].ToString();

                txtQty.Text = drx["qty"].ToString();
                txtPrice.Text = drx["UnitPrice"].ToString();
                txtCFR.Text = drx["CFRValue"].ToString();

                LoadItemsPanel();

                //Load color spec

                //Load color spec
                if (ddSubGrp.SelectedValue == "10") //Printing Ink
                {
                    LoadSpecList("");
                    pnlSpec.Visible = true;

                    string isExist = SQLQuery.ReturnString("Select spec from Specifications where id='" + spec + "'");
                    if (isExist == "")
                    {
                        ddSpec.Visible = false;
                        txtSpec.Visible = true;
                        lbSpec.Text = "Cancel";
                        txtSpec.Text = spec;
                    }
                    else
                    {
                        ddSpec.Visible = true;
                        txtSpec.Visible = false;
                        lbSpec.Text = "New";
                        ddSpec.SelectedValue = spec;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        RefreshForm();
        Notify("Action Cancelled!", "info", lblMsg);
    }

    private void RefreshForm()
    {
        ClearControls(Form);
        //ItemGrid.DataBind();
        BindItemGrid();
        ddInvoice.DataBind();
        //pnl.Update();
        //ItemGrid.EditIndex = -1;
        //EditField.Attributes.Remove("class");
        //EditField.Attributes.Add("class", "control-group hidden");

        btnDelete.Visible = false;
        LoadFormInfo();
    }

    /*
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand("SELECT HeadName FROM Transactions WHERE  TransactionGroup='Party' AND HeadID ='" + ddInvoice.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        string isFound = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();

        //SqlCommand cmdc = new SqlCommand("SELECT HeadName FROM Transactions WHERE  TransactionGroup='Party' AND HeadID ='" + ddInvoice.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //cmdc.Connection.Open();
        //string isFound2 = Convert.ToString(cmdc.ExecuteScalar());
        //cmdc.Connection.Close();

        if (isFound == "")
        {
            string lName = Page.User.Identity.Name.ToString();
            //get old data first
            string oldCompany = "", oldZone = "", oldBal = "", oldDueLimit = "", oldRef = "";
            SqlCommand cmd7 = new SqlCommand("SELECT Company, Zone, Referrer, Address, Phone, Email, Fax, IM, Website, ContactPerson, MobileNo, CreditLimit, OpBalance FROM [Party] WHERE PartyID=@PartyID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Parameters.Add("@PartyID", SqlDbType.VarChar).Value = ddInvoice.SelectedValue;
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                oldCompany = dr[0].ToString();
                oldZone = dr[1].ToString();
                oldRef = dr[2].ToString();
                //txtAddress.Text = dr[3].ToString();
                //txtLandPhone.Text = dr[4].ToString();
                //txtEmail.Text = dr[5].ToString();
                //txtFax.Text = dr[6].ToString();
                //txtIM.Text = dr[7].ToString();
                //txtWebsite.Text = dr[8].ToString();
                //txtName.Text = dr[9].ToString();
                //txtMobile.Text = dr[10].ToString();
                oldDueLimit = dr[11].ToString();
                oldBal = dr[12].ToString();

            }
            cmd7.Connection.Close();

            SqlCommand cmd3 = new SqlCommand("Delete Party where (PartyID= '" + ddInvoice.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd3.Connection.Open();
            cmd3.ExecuteNonQuery();
            cmd3.Connection.Close();

            //Delete History
            SqlCommand cmd2 = new SqlCommand("INSERT INTO PartyUpdateHistory (PartyID, OldCompanyName, NewCompanyName, OldZone, NewZone, OldReferrer, NewReferrer, OldCreditLimit, NewCreditLimit, OldOpBalance, NewOpBalance, UpdateBy)" +
                                        "VALUES ('" + ddInvoice.SelectedValue + "', '" + oldCompany + "', 'Deleted', '" + oldZone + "', 'Deleted', '" + oldRef + "', 'Deleted', '" + oldDueLimit + "', '0', '" + oldBal + "', '0', '" + lName + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();

            RefreshForm();
            lblMsg.Attributes.Add("class", "xerp_info");
            lblMsg.Text = "Party Info Deleted!";
        }
        else
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "This party already has previous transactions! Party can not be deleted...";
        }
    }
    */

    protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue +
                        "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue +
                 "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue +
                 "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
        GetProductList();
    }

    protected void ddSubGrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue +
                        "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue +
                 "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
        GetProductList();
    }

    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue +
                        "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
        GetProductList();
    }

    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetProductList();
    }

    private void GetProductList()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategory.SelectedValue +
                        "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        SQLQuery.PopulateDropDown(gQuery, ddItemName, "ProductID", "ItemName");

        ltrUnitType.Text = SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddItemName.SelectedValue + "'");

        if (IsPostBack)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
        }

        LoadItemsPanel();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            SQLQuery.Empty2Zero(txtWeight);
            string size = "", measure = "", thick = "";
            string subGrp = ddSubGrp.SelectedItem.Text;
            string desc = "";

            if (subGrp == "Tin Plate")
            {
                size = ddSize.SelectedValue;
                thick = txtThickness.Text;
                measure = txtMeasure.Text;

            }
            else if (ddGroup.SelectedValue == "1")//Raw Materials
            {

            }
            else if (ddGroup.SelectedValue == "4" || ddGroup.SelectedValue == "5")//Machineries & Electrical
            {
                size = ddSize.SelectedValue;
                thick = txtThickness.Text;
                measure = txtMeasure.Text;
                desc = "Country of Origin: " + ddCountry.SelectedItem.Text + ", Manufacturer: " + ddManufacturer.SelectedItem.Text;
            }
            else
            {
                thick = txtThickness.Text;
                measure = txtMeasure.Text;
            }
            string spec = "";
            if (ddSubGrp.SelectedValue == "10")//Printing Ink
            {
                spec = ddSpec.SelectedValue;
                if (txtSpec.Text != "" && lbSpec.Text == "Cancel" && ddSubGrp.SelectedValue == "10")//Insert Ink spec
                {
                    string isExist = RunQuery.SQLQuery.ReturnString("SELECT id FROM Specifications WHERE  spec ='" + txtSpec.Text + "'");
                    if (isExist == "")
                    {
                        SQLQuery.ExecNonQry("INSERT INTO Specifications (spec, EntryBy) VALUES ('" + txtSpec.Text + "', '" + lName + "') ");
                        spec = SQLQuery.ReturnString("SELECT MAX(id) FROM Specifications");
                        LoadSpecList(""); //ddSpec.DataBind();
                        ddSpec.SelectedValue = spec;
                    }
                    else
                    {
                        LoadSpecList("");
                        ddSpec.SelectedValue = isExist;
                    }
                }
                spec = ddSpec.SelectedValue;
            }
            //Amendment  History
            string oldItem = "", oldQty = "", oldPrice = "", oldCfr = "";
            string formtype = Request.QueryString["type"];
            if (formtype == "edit")
            {
                DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) Purpose, GradeId, CategoryId, ItemCode, HSCode, 
                                ItemSizeID, Spec, Thickness, Measurement, qty, UnitPrice, CFRValue, ReturnQty, NoOfPacks, QntyPerPack, 
                                Loading, Loaded, LandingPercent, LandingAmt, TotalUSD, TotalBDT, UnitCostBDT  FROM   [LcItems] WHERE EntryID='" + lblEntryId.Text + "'");

                foreach (DataRow drx in dtx.Rows)
                {
                    ddPurpose.SelectedValue = drx["Purpose"].ToString();
                    oldItem = Stock.Inventory.GetProductName(drx["ItemCode"].ToString());
                    
                    oldQty = drx["qty"].ToString();
                    oldPrice = drx["UnitPrice"].ToString();
                    oldCfr = drx["CFRValue"].ToString();
                }
            }

            if (btnAdd.Text == "Update")
            {
                
                SQLQuery.ExecNonQry(@"UPDATE [dbo].[LcItems]   SET 
                                            [GradeId] = '" + ddGrade.SelectedValue + "', [CategoryId] ='" +
                                    ddCategory.SelectedValue +
                                    "',[ItemCode] = '" + ddItemName.SelectedValue + "',[HSCode] = '" +
                                    txtHSCode.Text + "',[ItemSizeID] ='" + ddSize.SelectedValue + "',pcs='" +
                                    txtWeight.Text + "',[Spec] ='" + ddSpec.SelectedValue + "', [Thickness] = '" + txtThickness.Text +
                                    "',[Measurement] = '" + txtMeasure.Text + "', NoOfPacks='" + txtSerial.Text +
                                    "', QntyPerPack='" + txtWarrenty.Text + "',   [qty] = '" + txtQty.Text +
                                    "', FullDescription='" + desc + "', [UnitPrice] ='" + txtPrice.Text + "',[CFRValue] ='" + txtCFR.Text +
                                    "' WHERE EntryID='" + lblEntryId.Text + "'");

                //ItemGrid.DataBind();
                BindItemGrid();
                ItemGrid.SelectedIndex = (-1);
                btnAdd.Text = "Add to grid";

                string item = ddItemName.SelectedItem.Text;
                Save_LC_Log("Item updated", oldItem, item);
                Save_LC_Log_decimal("Qnty.: "+item, oldQty, txtQty.Text);
                Save_LC_Log_decimal("Unit Price: " + item, oldPrice, txtPrice.Text);
                Save_LC_Log_decimal("CFR: " + item, oldCfr, txtCFR.Text);
            }
            else
            {
                SQLQuery.Empty2Zero(txtCFR);

                //SqlCommand cmde = new SqlCommand("SELECT ItemCode FROM LcItems WHERE ItemCode ='" + ddItemName.SelectedValue + "' AND  LCNo ='' AND EntryBy ='" + lblLogin.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //cmde.Connection.Open();
                //string isExist = Convert.ToString(cmde.ExecuteScalar());
                //cmde.Connection.Close();

                if (ddItemName.SelectedValue != "")
                {
                    SqlCommand cmd2 = new SqlCommand("INSERT INTO LcItems (LCNo, Purpose, GradeId, CategoryId, ItemCode, HSCode, Thickness, ItemSizeID, Spec, Measurement, NoOfPacks, QntyPerPack,  qty, UnitPrice, CFRValue, Loading, Loaded, LandingPercent, LandingAmt, pcs, EntryBy, FullDescription) VALUES (@LCNo, '" +
                            ddPurpose.SelectedValue + "', '" + ddGrade.SelectedValue + "',  '" +
                            ddCategory.SelectedValue + "', @ItemCode, @HSCode, @Thickness, @ItemSizeID,  '" +
                            ddSpec.SelectedValue + "',  @Measurement, '" + txtSerial.Text + "','" + txtWarrenty.Text +
                            "', @qty, @UnitPrice, @CFRValue, @Loading, @Loaded, @LandingPercent, @LandingAmt, '" +
                            txtWeight.Text + "',  @EntryBy, '" + desc + "')",
                            new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                    cmd2.Parameters.AddWithValue("@LCNo", txtEditLcNo.Text);
                    cmd2.Parameters.AddWithValue("@ItemCode", ddItemName.SelectedValue);
                    cmd2.Parameters.AddWithValue("@HSCode", txtHSCode.Text);
                    cmd2.Parameters.AddWithValue("@Thickness", thick);
                    cmd2.Parameters.AddWithValue("@ItemSizeID", size);
                    cmd2.Parameters.AddWithValue("@Measurement", measure);

                    cmd2.Parameters.AddWithValue("@NoOfPacks", txtSerial.Text);
                    cmd2.Parameters.AddWithValue("@QntyPerPack", txtWarrenty.Text);
                    cmd2.Parameters.AddWithValue("@qty", txtQty.Text);
                    cmd2.Parameters.AddWithValue("@UnitPrice", txtPrice.Text);
                    cmd2.Parameters.AddWithValue("@CFRValue", txtCFR.Text);

                    //Loading, Loaded, LandingPercent, LandingAmt
                    cmd2.Parameters.AddWithValue("@Loading", "1");
                    cmd2.Parameters.AddWithValue("@Loaded", txtCFR.Text);
                    cmd2.Parameters.AddWithValue("@LandingPercent", "1.01");
                    cmd2.Parameters.AddWithValue("@LandingAmt", Convert.ToDecimal(txtCFR.Text) * 1.01M);
                    cmd2.Parameters.AddWithValue("@EntryBy", lName);

                    cmd2.Connection.Open();
                    cmd2.ExecuteNonQuery();
                    cmd2.Connection.Close();

                    string item = ddItemName.SelectedItem.Text;
                    Save_LC_Log("New item inserted: ", item, "Qnty.: " + txtQty.Text + ", Unit Price: " + txtPrice.Text+", CFR: " + txtCFR.Text);
                   
                    //ItemGrid.DataBind();
                    BindItemGrid();

                    txtQty.Text = "";
                    txtPrice.Text = "";
                    txtThickness.Text = "";
                    txtMeasure.Text = "";
                    txtCFR.Text = "";
                    ddGroup.Focus();
                }
                else
                {
                    Notify("ERROR: Invalid Product Name!", "warn", lblMsg2);
                }
            }

            if (txtBExRate.Text != "" && txtCExRate.Text != "" && txtCfrUSD.Text != "")
            {
                txtCfrBDT.Text = Convert.ToString(Convert.ToDecimal(txtCExRate.Text) * Convert.ToDecimal(txtCfrUSD.Text));
                txtBankBDT.Text = Convert.ToString(Convert.ToDecimal(txtBExRate.Text) * Convert.ToDecimal(txtCfrUSD.Text));
                if (txtMargin.Text != "")
                {
                    txtLTR.Text = Convert.ToString(Convert.ToDecimal(txtBankBDT.Text) - Convert.ToDecimal(txtMargin.Text));
                }
            }

            txtSerial.Text = "";
            txtPrice.Text = "";
            txtCFR.Text = "";
            txtWarrenty.Text = "";
            txtQty.Text = "";
            ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg2);
        }
        finally
        {
            //ItemGrid.DataBind();
            BindItemGrid();
        }
    }

    private void BindItemGrid()
    {
        string lName = Page.User.Identity.Name.ToString();
        string formtype = Request.QueryString["type"];
        if (formtype == "edit")
        {
            SQLQuery.PopulateGridView(ItemGrid, @"SELECT EntryID, 
                                                (Select Purpose from Purpose where pid=a.Purpose) as Purpose, 
                                                (Select GradeName from ItemGrade where GradeID=a.GradeId) as Grade, 
                                                (Select CategoryName from Categories where CategoryID=a.CategoryId) as Category, 
                                                (Select ItemName from Products where ProductID=a.ItemCode) as Product,                      
                                                [Thickness], (Select BrandName from Brands where BrandID=a.ItemSizeID) as Size, Measurement, (Select spec from Specifications where id=a.spec) as spec,
                                                CONVERT(varchar(10), qty) +' '+(Select UnitType from Products where ProductID=a.ItemCode) As QTY1, UnitPrice,  [CFRValue] 
                                                FROM [LcItems] a Where  LCNo='" + txtEditLcNo.Text + "' ORDER BY [EntryID]");

            //SQLQuery.PopulateGridView(GridExpenses, "SELECT EID, ExpHeadName, Amount FROM PurchaseExpenses WHERE  InvoiceNo='" + lblInvoice.Text + "' ORDER BY eid");
        }
        else
        {
            SQLQuery.PopulateGridView(ItemGrid, @"SELECT EntryID, 
                                                (Select Purpose from Purpose where pid=a.Purpose) as Purpose, 
                                                (Select GradeName from ItemGrade where GradeID=a.GradeId) as Grade, 
                                                (Select CategoryName from Categories where CategoryID=a.CategoryId) as Category, 
                                                (Select ItemName from Products where ProductID=a.ItemCode) as Product,                      
                                                [Thickness], (Select BrandName from Brands where BrandID=a.ItemSizeID) as Size, Measurement, (Select spec from Specifications where id=a.spec) as spec,
                                                CONVERT(varchar(10), qty) +' '+(Select UnitType from Products where ProductID=a.ItemCode) As QTY1, UnitPrice,  [CFRValue] 
                                                FROM [LcItems] a Where  LCNo='' AND EntryBy='" + lName + "' ORDER BY [EntryID]");
        }
    }

    protected void ItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label rIndex = ItemGrid.Rows[index].FindControl("Label1") as Label;

            SQLQuery.ExecNonQry("DELETE LcItems WHERE EntryID=" + rIndex.Text);

            //Ammd history
            Label Label2 = ItemGrid.Rows[index].FindControl("Label2") as Label;
            Label QTY9 = ItemGrid.Rows[index].FindControl("QTY9") as Label;
            Label UnitPrice = ItemGrid.Rows[index].FindControl("UnitPrice") as Label;
            Label CFRValue = ItemGrid.Rows[index].FindControl("CFRValue") as Label;
            Save_LC_Log("Item deleted: ", Label2.Text, "Qnty.: " + QTY9.Text + ", Unit Price: " + UnitPrice.Text + ", CFR: " + CFRValue.Text);

            lblMsg2.Attributes.Add("class", "xerp_warn");
            lblMsg2.Text = "LC Item Deleted ...";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }

        BindItemGrid();
        ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
    }


    private void LoadOldData(int lcSl)
    {
        try
        {
            SqlCommand cmd7 = new SqlCommand(@"Select sl, LCNo, LCType, OpenDate, Category, LCItem, HSCode, LcRef, LcFor, ExpiryDate, ShipDate, SupplierID, Origin, AgentID, InsuranceID, CnfID, BankId, " +
                    "LcMargin, LTR, BankExcRate, CustomExcRate, QtyUnit, TotalQty, Freight, CfrUSD, CfrBDT, Remarks, TransportMode, IsActive, LCCloseBy, LCClosedate, BankBDT, " +
                    " ArrivalDate, DeliveryDate, Status, InsuranceNo, BanksId, BranchId, OpeningBankCharge, ForeignBank FROM [LC] WHERE sl=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Parameters.Add("@sl", SqlDbType.Int).Value = lcSl;
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                //btnSave.Text = "Update";
                lblSl.Text = dr[0].ToString();
                lblLCNo.Text = dr[1].ToString();
                lblLCType.Text = dr[2].ToString();
                lblOpDate.Text = Convert.ToDateTime(dr[3].ToString()).ToShortDateString();
                //ddSuppCategory.SelectedValue = dr[4].ToString();
                //txtReferrence.Text = dr[5].ToString();
                //txtFor.Text = dr[6].ToString();
                //lblHS.Text = dr[6].ToString();
                lblDept.Text = dr[8].ToString();
                lblExDate.Text = Convert.ToDateTime(dr[9].ToString()).ToShortDateString();
                lblShipDate.Text = Convert.ToDateTime(dr[10].ToString()).ToShortDateString();

                lblSupplier.Text = SQLQuery.ReturnString("Select Company FROM Party where PartyID = " + dr[11].ToString());
                lblCountry.Text = dr[12].ToString();
                lblagent.Text = SQLQuery.ReturnString("Select Company FROM Party where PartyID = " + dr[13].ToString());
                lblInsurance.Text =SQLQuery.ReturnString("Select Company FROM Party where PartyID  = " + dr[14].ToString());
                lblInsuranceNo.Text = dr["InsuranceNo"].ToString();
                lblCNF.Text = SQLQuery.ReturnString("Select Company FROM Party where PartyID = " + dr[15].ToString());
                //, BanksId, BranchId
                lblBankAcc.Text = SQLQuery.ReturnString("Select (Select BankName FROM Banks where BankId =BankAccounts.BankID)+' - '+ ACNo +' - '+ACName FROM BankAccounts where ACID = " + dr["BankId"].ToString());
                lblBankBranch.Text = SQLQuery.ReturnString("Select BranchName FROM BankBranch where BranchId = " + dr["BranchId"].ToString());

                string acc = SQLQuery.ReturnString("Select (Select BankName FROM Banks where BankId =BankAccounts.BankID) FROM BankAccounts where ACID = " +dr[16].ToString());
                lblBank.Text = acc;

                lblMargin.Text = dr[17].ToString();
                lblLTR.Text = dr[18].ToString();
                lblLTR.Text = dr["ForeignBank"].ToString();
                lblOpeningBangCrg.Text = dr["OpeningBankCharge"].ToString();
                lblBankExcRate.Text = dr[19].ToString();
                lblCustExRate.Text = dr[20].ToString();
                //lblqt = dr[20].ToString();
                lblTtlQty.Text = dr[22].ToString();
                lblFreight.Text = dr[23].ToString();
                lblCfrUsd.Text = dr[24].ToString();
                lblCfrBdt.Text = dr[25].ToString();

                lblRemark.Text = dr[26].ToString();
                lblMode.Text = dr[27].ToString();

                string isActive = dr[28].ToString();
                if (isActive == "A")
                {
                    isActive = "Pending Shipment";
                }
                else if (isActive == "P")
                {
                    isActive = "On Port";
                }
                else if (isActive == "D")
                {
                    isActive = "Delivered";
                }
                else if (isActive == "S")
                {
                    isActive = "Closed by " + dr[29].ToString();
                    lblStatusColor.Attributes.Add("class", "xerp_green");
                }
                else if (isActive == "C")
                {
                    isActive = "Cancelled by " + dr[29].ToString();
                    lblStatusColor.Attributes.Add("class", "xerp_red");
                }

                lblStatus.Text = "Status: " + isActive.ToUpper() + " <br>" + dr[34].ToString(); ;
                lblBankBdt.Text = dr[31].ToString();
                lblArrivalDt.Text = Convert.ToDateTime(dr[32].ToString()).ToShortDateString();
                lblDeliveryDt.Text = Convert.ToDateTime(dr[33].ToString()).ToShortDateString();
                //btnDelete.Visible = true;
                //ltrSubFrmName.Text = "Edit Party";
                //lblMsg.Attributes.Add("class", "xerp_info");
                //lblMsg.Text = "Info loaded in edit mode.";
                //EditField.Attributes.Remove("class");
                //EditField.Attributes.Add("class", "control-group");

                //ItemGrid.DataBind();
                //pnl.Update();
            }
            cmd7.Connection.Close();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }



    protected void btnFirst_Click(object sender, EventArgs e)
    {
        int LcViewing = Convert.ToInt32(lblSl.Text);
        int LcSl = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(min(sl),0) from LC"));
        if (LcViewing != LcSl && LcSl != 0)
        {
            LoadOldData(LcSl);
            GridView2.DataBind();
        }
        else
        {
            Notify("You are viewing the first data!", "info", lblMsgNav);
        }
    }

    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        int LcViewing = Convert.ToInt32(lblSl.Text);
        int LcSl = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(max(sl),0) from LC where sl<" + LcViewing));
        if (LcViewing != LcSl && LcSl != 0)
        {
            LoadOldData(LcSl);
            GridView2.DataBind();
        }
        else
        {
            Notify("You are viewing the first data!", "warn", lblMsgNav);
        }
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        int LcViewing = Convert.ToInt32(lblSl.Text);
        int LcSl = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(min(sl),0) from LC where sl>" + LcViewing));
        if (LcViewing != LcSl && LcSl != 0)
        {
            LoadOldData(LcSl);
            GridView2.DataBind();
        }
        else
        {
            Notify("You are viewing the last data!", "warn", lblMsgNav);
        }
    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        int LcViewing = Convert.ToInt32(lblSl.Text);
        int LcSl = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(max(sl),0) from LC"));
        if (LcViewing != LcSl && LcSl != 0)
        {
            LoadOldData(LcSl);
            GridView2.DataBind();
        }
        else
        {
            Notify("You are viewing the last data!", "info", lblMsgNav);
        }
    }


    ///<summary>
    /// Suggestion or AutoComplete old thickness/measurement data
    ///</summary>

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, 
    // uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class AutoComplete : System.Web.Services.WebService
    {
        public AutoComplete()
        {
            //Uncomment the following line if using designed components
            //InitializeComponent();
        }

        [WebMethod]
        public string[] GetCompletionList(string prefixText, int count)
        {
            DataSet ds = new DataSet();
            string query = "Select Distinct Measurement from LcItems where order by Measurement";
            ds = SQLQuery.ReturnDataSet(query);
            DataTable dt = ds.Tables[0];

            //Then return List of string(txtItems) as result
            List<string> txtItems = new List<string>();
            String dbValues;

            foreach (DataRow row in dt.Rows)
            {
                //String From DataBase(dbValues)
                dbValues = row["Measurement"].ToString();
                dbValues = dbValues.ToLower();
                txtItems.Add(dbValues);
            }

            return txtItems.ToArray();
        }
    }

    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        txtTtlQty.Text = SQLQuery.ReturnString("Select SUM(qty) FROM [LcItems] Where  LCNo='" + txtEditLcNo.Text + "'");
        txtCfrUSD.Text = SQLQuery.ReturnString("Select SUM(CFRValue) FROM [LcItems] Where  LCNo='" + txtEditLcNo.Text + "'");
    }

    protected void lbSpec_OnClick(object sender, EventArgs e)
    {
        if (lbSpec.Text == "New")
        {
            ddSpec.Visible = false;
            txtSpec.Visible = true;
            lbSpec.Text = "Cancel";
            txtSpec.Focus();
        }
        else
        {
            ddSpec.Visible = true;
            txtSpec.Visible = false;
            lbSpec.Text = "New";
            LoadSpecList("filter");
            ddSpec.Focus();
        }
        lbFilter.Text = "Show-all";
    }

    protected void lbFilter_OnClick(object sender, EventArgs e)
    {
        if (lbFilter.Text == "Show-all")
        {
            LoadSpecList("");
            //lbFilter.Text = "Filter"
        }
        else
        {
            LoadSpecList("filter");
            //lbFilter.Text = "Show-all";
        }
        lbSpec.Text = "New";
        ddSpec.Visible = true;
        txtSpec.Visible = false;
    }

    private void LoadSpecList(string filterDD)
    {
        string gQuery = "SELECT [id], [spec] FROM [Specifications] ORDER BY [spec]";
        lbFilter.Text = "Filter";

        if (filterDD != "")
        {
            lbFilter.Text = "Show-all";
            gQuery = "SELECT [id], [spec] FROM [Specifications] where  CAST(id AS nvarchar) in (Select distinct spec from LcItems where ItemCode='" +
                ddItemName.SelectedValue + "') ORDER BY [spec]";
        }

        SQLQuery.PopulateDropDown(gQuery, ddSpec, "id", "spec");
        //QtyinStock();
    }

    protected void ddSpec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //QtyinStock();
    }

    protected void ddSuppCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddManufacturer.DataBind();
    }

    protected void ddItemName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSpecList("filter");
    }


    protected void ItemGrid_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(ItemGrid.SelectedIndex);
        Label lblItemName = ItemGrid.Rows[index].FindControl("Label1") as Label;
        lblEntryId.Text = lblItemName.Text;
        GridItemEditMode(lblItemName.Text);
    }

    protected void ddAgentCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddAgent.DataBind();
    }


    protected void btnSearchLC_OnClick(object sender, EventArgs e)
    {
        int searchLcSl = Convert.ToInt32(SQLQuery.ReturnString("SELECT ISNULL(MAX(sl),0) FROM LC WHERE LCNo LIKE '%" + txtSearchByLCNo.Text + "%'"));
        if (searchLcSl > 0)
        {
            LoadOldData(searchLcSl);
        }
        else
        {
            Notify("No LC Info found...", "warn", lblMsg);
        }
    }

    protected void ddType_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddType.SelectedValue == "LC")
        {

            margin.Visible = true;
            ltr.Visible = true;
            foreignBank.Visible = false;

        }
        else if (ddType.SelectedValue == "FTT")
        {
            margin.Visible = false;
            ltr.Visible = false;
            foreignBank.Visible = true;
        }
        else
        {
            margin.Visible = true;
            ltr.Visible = true;
        }
    }

    protected void btnHead_OnClick(object sender, EventArgs e)
    {
        if (btnAccHead.Text == "New")
        {
            ddAdvanceAginstHead.Visible = false;
            txtAccHead.Visible = true;
            btnAccHead.Text = "Cancel";
            txtAccHead.Focus();
        }
        else
        {
            ddAdvanceAginstHead.Visible = true;
            txtAccHead.Visible = false;
            btnAccHead.Text = "New";
            //LoadSpecList("filter");
            ddAdvanceAginstHead.Focus();
        }

    }
}