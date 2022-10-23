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

using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using Accounting;
using RunQuery;

public partial class app_LC_Expenses : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToShortDateString();
            ddName.DataBind();
            ltrLC.Text = RunQuery.SQLQuery.ReturnString("Select LCNo from LC where sl='" + ddName.SelectedValue + "'");

            ddType.DataBind();
            ddHead.DataBind();
            LoadLCData();
            //LoadDutyData();
            GetInsuranceData();
            GetBankExpData();

            GridView1.DataBind();
            GridView2.DataBind();
            //CalcDuty();
            //GridView2.SelectedIndex = -1;
        }
    }
    protected void ddName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ltrLC.Text = RunQuery.SQLQuery.ReturnString("Select LCNo from LC where sl='" + ddName.SelectedValue + "'");
        LoadLCData();
        //LoadDutyData();

        GetInsuranceData();
        //GridView1.DataBind();
        GridView2.DataBind();
    }
    protected void ddType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddHead.DataBind();
        //GridView1.DataBind();
    }


    protected void LinkButton0_Click(object sender, EventArgs e)
    {
        ShowPanel(Panel0, LinkButton0);

        LinkButton1.Enabled = true;

        step1r.Attributes.Remove("class");
        step1r.Attributes.Add("class", "step-dark-right");
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        //GridView2.DataBind();
        //GridView2.SelectedIndex = -1;

        //ShowPanel(Panel1, LinkButton1);

        //step1l.Attributes.Remove("class");
        //step1l.Attributes.Add("class", "step-dark-left");
        //step1r.Attributes.Remove("class");
        //step1r.Attributes.Add("class", "step-dark-right");

        //CalculateInsurance();

        ShowPanel(Panel1, LinkButton1);

        LinkButton1.Enabled = true;

        step1l.Attributes.Remove("class");
        step1l.Attributes.Add("class", "step-dark-left");
        step8r.Attributes.Remove("class");
        step8r.Attributes.Add("class", "step-dark-right");
        //step3l.Attributes.Remove("class");
        //step3l.Attributes.Add("class", "step-dark-left");
        //step3r.Attributes.Remove("class");
        //step3r.Attributes.Add("class", "step-dark-right");

        CalculateInsurance();
    }

    protected void LinkButton7_OnClick(object sender, EventArgs e)
    {
        ShowPanel(Panel3, LinkButton7);

        LinkButton7.Enabled = true;

        step3l.Attributes.Remove("class");
        step3l.Attributes.Add("class", "step-dark-left");
        step3r.Attributes.Remove("class");
        step3r.Attributes.Add("class", "step-dark-right");

        //CalculateInsurance();

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
                lblOpDate.Text = Convert.ToDateTime(dr[1].ToString()).ToShortDateString();
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
                txtInsurance.Text = SQLQuery.ReturnString("Select Company FROM Party where PartyID = " + dr[12].ToString());
                InsuranceIdHField.Value = dr[12].ToString();
                txtCNF.Text = SQLQuery.ReturnString("Select Company FROM Party where PartyID = " + dr[13].ToString());
                //lblBank.Text = SQLQuery.ReturnString("Select BankName FROM Banks where BankId = " + dr[14].ToString());
                string margin = dr[15].ToString();
                string ltr = dr[16].ToString();
                string bExRate = dr[17].ToString();
                //txtCExRate.Text = dr[18].ToString();
                txtCustomExRate.Text = dr[18].ToString();

                //txtQty.Text = dr[19].ToString();
                txtTtlQty.Text = dr[20].ToString();
                txtFreight.Text = dr[21].ToString();
                string cfrUSD = dr[22].ToString();
                txtCfrBDT.Text = dr[23].ToString();
                txtRemarks.Text = dr[24].ToString();
                ddMode.Text = dr[25].ToString();
                string bankBDT = dr[28].ToString();
                //btnCancel.Visible = true;
                //ltrSubFrmName.Text = "Edit Party";
                //lblMsg.Attributes.Add("class", "xerp_info");
                //lblMsg.Text = "Info loaded in edit mode.";
                EditField.Attributes.Remove("class");
                EditField.Attributes.Add("class", "control-group");

                //GridView1.DataBind();
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

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }


    protected void txtQty_TextChanged(object sender, System.EventArgs e)
    {
        //string test = "t";
        //if (lblItemEntryId.Text != "")
        //{
        CalcDuty();
        //}
        //else
        //{
        //    lblMsg2.Attributes.Add("class", "xerp_stop");
        //    lblMsg2.Text = "Please select item first";
        //}
    }
    private void CalcDuty()
    {
        try
        {
            decimal totalUSD = 0, totalBDT = 0;
            for (int idx = 0; idx < GridView2.Rows.Count; idx++)
            {
                Label lblIID = (Label)GridView2.Rows[idx].FindControl("Label1");
                Label lblCFRValue = (Label)GridView2.Rows[idx].FindControl("lblCFRValue");
                TextBox txtLoading = (TextBox)GridView2.Rows[idx].FindControl("txtLoading");
                TextBox txtLanding = (TextBox)GridView2.Rows[idx].FindControl("txtLanding");

                decimal loading = 0;
                if (txtLoading.Text != "")
                {
                    loading = Convert.ToDecimal(txtLoading.Text);
                }

                decimal landing = 0;
                if (txtLanding.Text != "")
                {
                    landing = Convert.ToDecimal(txtLanding.Text); //1M + (Convert.ToDecimal(txtLanding.Text) / 100M); // 1% = 1.01
                }
                else
                {
                    txtLanding.Text = "1.01";
                }

                decimal loaded = Convert.ToDecimal(lblCFRValue.Text) * loading;
                decimal landingAmt = loaded * landing;
                decimal itotalBDT = landingAmt * Convert.ToDecimal(txtCustomExRate.Text);

                RunQuery.SQLQuery.ExecNonQry(@"UPDATE LcItems SET 
                                              Loading=" + loading + ", Loaded=" + loaded + ", LandingPercent=" + txtLanding.Text +
                                              ", LandingAmt=" + landingAmt + ", TotalUSD=" + landingAmt + ", TotalBDT=" + itotalBDT +
                                              "WHERE EntryID= " + lblIID.Text);
                totalUSD += landingAmt;
                totalBDT += itotalBDT;
            }


            SQLQuery.ExecNonQry(@"UPDATE     LC_Duty_Calc SET 
                                              ItemTotalUSD=" + totalUSD + ", ItemTotalBDT=" + totalBDT + ", CustomExRate=" + Convert.ToDecimal(txtCustomExRate.Text) +
                                          "WHERE LCNo= '" + RunQuery.SQLQuery.ReturnString("Select LCNo from LC where sl='" + ddName.SelectedValue + "'") + "'");

            LoadDutyData(lblItemEntryId.Text);
            GridView2.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
        }
    }


    protected void txtDuties_TextChanged(object sender, System.EventArgs e)
    {
        try
        {
            if (lblItemEntryId.Text != "")
            {
                CalcAV();
                CalcDuties();
                LoadDutyData(lblItemEntryId.Text);
                UpdateLcDutyCalc();
            }
            else
            {
                lblMsg2.Attributes.Add("class", "xerp_stop");
                lblMsg2.Text = "Please select item first";
            }
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = ex.ToString();
        }
    }
    protected void txtATAmt_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (lblItemEntryId.Text != "")
            {
                CalcAV();
                CalcDuties();
                LoadDutyData(lblItemEntryId.Text);
                UpdateLcDutyCalc();
            }
            else
            {
                lblMsg2.Attributes.Add("class", "xerp_stop");
                lblMsg2.Text = "Please select item first";
            }
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = ex.ToString();
        }
    }

    protected void txtDFCVATFP_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (lblItemEntryId.Text != "")
            {
                CalcAV();
                CalcDuties();
                LoadDutyData(lblItemEntryId.Text);
                UpdateLcDutyCalc();
            }
            else
            {
                lblMsg2.Attributes.Add("class", "xerp_stop");
                lblMsg2.Text = "Please select item first";
            }
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = ex.ToString();
        }
    }
    protected void txtTtlActuDuty_OnTextChanged(object sender, EventArgs e)
    {
        txtTtlActuDuty = txtTtlCalcDuty;
    }

    private void CalcAV()
    {
        EmptytoZero(txtInsurPercent);
        EmptytoZero(txtOtherAmount);
        EmptytoZero(txtAVActual);
        EmptytoZero(txtiTtlBDT);

        decimal totalBDT = Convert.ToDecimal(txtiTtlBDT.Text);
        decimal insurancePercent = Convert.ToDecimal(txtInsurPercent.Text);
        //decimal insuranceAmt = totalBDT * (1 / 100M);//1% insurence
        decimal penaltyAmt = Convert.ToDecimal(txtOtherAmount.Text);
        decimal avCalc = totalBDT + insurancePercent + penaltyAmt;

        RunQuery.SQLQuery.ExecNonQry(@"UPDATE LC_Items_Duty SET InsuranceAmount=" + insurancePercent + ",  PenaltyDesc='" + txtOtherDesc.Text + "', PenaltyAmt=" + Convert.ToDecimal(txtOtherAmount.Text) +
                                      " , AV_Calculated=" + avCalc + ", AV_Actual='" + txtAVActual.Text + "'   WHERE EntryID= '" + lblItemEntryId.Text + "'");

        //GridView2.DataBind();
        txtAVCalc.Text = avCalc.ToString("#.##");
    }


    private void CalcDuties()
    {
        //string lcNo = RunQuery.SQLQuery.ReturnString("Select LCNo from LC where sl='" + ddName.SelectedValue + "'");

        EmptytoZero(txtATAmt);
        EmptytoZero(txtATVRate);
        EmptytoZero(txtAITRate);
        EmptytoZero(txtVATRate);
        EmptytoZero(txtSurChRate);
        EmptytoZero(txtSDRate);
        EmptytoZero(txtRDRate);
        EmptytoZero(txtCDRate);
        EmptytoZero(txtDFCVATFP);

        decimal cd = Convert.ToDecimal(txtCDRate.Text);
        decimal rd = Convert.ToDecimal(txtRDRate.Text);
        decimal sd = Convert.ToDecimal(txtSDRate.Text);
        decimal sc = Convert.ToDecimal(txtSurChRate.Text);
        decimal vat = Convert.ToDecimal(txtVATRate.Text);
        decimal ait = Convert.ToDecimal(txtAITRate.Text);
        decimal at = Convert.ToDecimal(txtATAmt.Text);
        decimal atv = Convert.ToDecimal(txtATVRate.Text);
        decimal dfcvat = Convert.ToDecimal(txtDFCVATFP.Text);

        decimal AV = Convert.ToDecimal(txtAVActual.Text);
        //decimal cdAmt = AV * (cd / 100M);
        //decimal rdAmt = AV * (rd / 100M);
        //decimal sdAmt = AV * (sd / 100M);
        //decimal scAmt = AV * (sc / 100M);
        //decimal vatAmt = AV * (vat / 100M);
        //decimal aitAmt = AV * (ait / 100M);

        decimal cdAmt = cd;
        decimal rdAmt = rd;
        decimal sdAmt = sd;
        decimal scAmt = sc;
        decimal vatAmt = vat;
        decimal aitAmt = ait;

        //decimal atvCalc = (AV + cdAmt + rdAmt + sdAmt) + (((AV + cdAmt + rdAmt + sdAmt) * 26.67M) / 100M);
        //decimal atvAmt = atvCalc * (atv / 100M);
        decimal atAmt = at;
        decimal atvAmt = atv;
        decimal dfcvatAmt = dfcvat;
        decimal ttlDuty = cdAmt + rdAmt + sdAmt + scAmt + vatAmt + aitAmt + atAmt + atvAmt + dfcvatAmt;

        RunQuery.SQLQuery.ExecNonQry(@"UPDATE      LC_Items_Duty   SET    AV_Actual=" + AV + ", " +
                        "CustomsDutyRate=" + cd + ", CustomsDutyAmt=" + cdAmt + ", RDRate=" + rd + ", RDAmt=" + rdAmt + ", SDRate=" + sd + ", SDAmt=" + sdAmt + ", SurChargeRate=" + sc + ", SurChargeAmt=" + scAmt + ", VATRate=" + vat + ", VATAmt=" + vatAmt + ", AITRate=" + ait + ", AITAmt=" + aitAmt + ", ATAmt=" + atAmt + ", ATVRate=" + atv + ", ATVAmt=" + atvAmt + ", DFCVATFP=" + dfcvatAmt + ", " +
                        "TotalDutyCalculated=" + ttlDuty + ", TotalDutyActual=" + txtTtlActuDuty.Text +
                        "    WHERE EntryID= '" + lblItemEntryId.Text + "'");

    }


    private void UpdateLcDutyCalc()
    {
        //Compare to check items Duty Step Completed
        int i1 = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(EntryID) from LcItems where LCNo='" + ltrLC.Text + "'"));
        int i2 = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(EntryID) from   LC_Items_Duty where LCNo='" + ltrLC.Text + "' AND TotalDutyActual>0"));

        if (i1 == i2)
        {
            LinkButton2.Enabled = true;

            SqlCommand cmd7 = new SqlCommand(@"Select SUM(InsuranceAmount), SUM(PenaltyAmt), SUM(AV_Calculated), SUM(AV_Actual), SUM(CustomsDutyRate), SUM(CustomsDutyAmt), SUM(RDRate), SUM(RDAmt), 
                         SUM(SDRate), SUM(SDAmt), SUM(SurChargeRate), SUM(SurChargeAmt), SUM(VATRate), SUM(VATAmt), SUM(AITRate), SUM(AITAmt), SUM(ATVRate), SUM(ATVAmt), SUM(TotalDutyCalculated), SUM(TotalDutyActual) FROM [LC_Items_Duty] WHERE LCNo=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd7.Parameters.Add("@sl", SqlDbType.VarChar).Value = ltrLC.Text;
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                RunQuery.SQLQuery.ExecNonQry(@"UPDATE LC_Duty_Calc SET CompleteStep=1, InsuranceAmount=" + dr[0].ToString() + ", PenaltyAmt=" + dr[1].ToString() + ", AV_Calculated=" + dr[2].ToString() + ", AV_Actual=" + dr[3].ToString() + ", CustomsDutyRate=" + dr[4].ToString() + ", CustomsDutyAmt=" + dr[5].ToString() + ", RDRate=" + dr[6].ToString() + ", RDAmt=" + dr[7].ToString() + ", SDRate=" + dr[8].ToString() + ", SDAmt=" + dr[9].ToString() +
                    ", SurChargeRate=" + dr[10].ToString() + ", SurChargeAmt=" + dr[11].ToString() + ", VATRate=" + dr[12].ToString() + ", VATAmt=" + dr[13].ToString() + ", AITRate=" + dr[14].ToString() + ", AITAmt=" + dr[15].ToString() +
                    ", ATVRate=" + dr[16].ToString() + ", ATVAmt=" + dr[17].ToString() + ", TotalDutyCalculated=" + dr[18].ToString() +
                                        " , TotalDutyActual=" + dr[19].ToString() + "  WHERE LCNo= '" + ltrLC.Text + "'");

                cmd7.Connection.Close();
            }

            cmd7 = new SqlCommand(@"Select SUM(Loaded), SUM(LandingAmt), SUM(TotalUSD), SUM(TotalBDT) FROM [LcItems] WHERE LCNo=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd7.Parameters.Add("@sl", SqlDbType.VarChar).Value = ltrLC.Text;
            cmd7.Connection.Open();
            dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                RunQuery.SQLQuery.ExecNonQry(@"UPDATE LC_Duty_Calc SET  Loaded=" + dr[0].ToString() + ", Landing=" + dr[1].ToString() + ", ItemTotalUSD=" + dr[2].ToString() + ", ItemTotalBDT=" + dr[3].ToString() +
                    "  WHERE LCNo= '" + ltrLC.Text + "'");

                cmd7.Connection.Close();
            }
        }
        else
        {
            LinkButton2.Enabled = false;
        }

    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            RunQuery.SQLQuery.ExecNonQry("DELETE FROM LC_Expenses WHERE esl =" + lblItemCode.Text);

            GridView1.DataBind();
            lblMsg.Attributes.Add("class", "xerp_warning");
            lblMsg.Text = "Entry removed successfully ...";
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
        }
    }

    protected void btnSaveDutyN_Click(object sender, EventArgs e)
    {
        try
        {
            UpdateLcDutyCalc();

            //Compare to check items Duty Step Completed
            int i1 = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(EntryID) from LcItems where LCNo='" + ltrLC.Text + "'"));
            int i2 = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(EntryID) from LC_Items_Duty where LCNo='" + ltrLC.Text + "' AND TotalDutyActual>0"));

            if (i1 == i2)
            {
                //LinkButton2.Enabled = true;
                //LoadPanel2();

                ShowPanel(pnlCNFCommission, LinkButton8);

                try
                {
                    SqlCommand cmd7 = new SqlCommand(@"SELECT TOP (200) [CnfID],[LCNo],[PartyID],[PortCharge],[ShipingCharge],[ReceiptAmt],[Miscellaneous],[OtherCharge],[Commission],[TotalAmount],[Description],[PaidStatus],[EntryBy],[EntryDate]
                        FROM LC_CNF WHERE LCNo=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmd7.Parameters.Add("@sl", SqlDbType.VarChar).Value = ddName.SelectedValue;
                    cmd7.Connection.Open();
                    SqlDataReader dr = cmd7.ExecuteReader();
                    if (dr.Read())
                    {
                        ddCNFAgent.SelectedValue = dr["PartyID"].ToString();
                        txtPortCharge.Text = dr["PortCharge"].ToString();
                        txtShippingCharge.Text = dr["ShipingCharge"].ToString();
                        txtReceiptCnf.Text = dr["ReceiptAmt"].ToString();
                        txtMiscellaneousCnf.Text = dr["Miscellaneous"].ToString();
                        txtOtherChargeCnf.Text = dr["OtherCharge"].ToString();
                        txtCommissionCnf.Text = dr["Commission"].ToString();
                        txtTotalAmtCnf.Text = dr["TotalAmount"].ToString();
                        txtDescriptionCnf.Text = dr["Description"].ToString();


                        EditField.Attributes.Remove("class");
                        EditField.Attributes.Add("class", "control-group");

                        cmd7.Connection.Close();
                    }

                }

                catch (Exception ex)
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    lblMsg.Text = "ERROR: " + ex.Message.ToString();
                }
                //16..06..2020 lc relation with accoutns
                string lName = Page.User.Identity.Name.ToString();
                SQLQuery.Empty2Zero(txtTtlActuDuty);
                string description;
                string accHeadID = RunQuery.SQLQuery.ReturnString("Select AccountsHeadID FROM LC where LCNo='" + ddName.SelectedItem.Text + "'");
                string accHeadName = RunQuery.SQLQuery.ReturnString("SELECT H.AccountsHeadName FROM LC  INNER JOIN HeadSetup As H ON LC.AccountsHeadID = H.AccountsHeadID WHERE (LC.LCNo = '" + ddName.SelectedItem.Text + "')");
                string lcValue = RunQuery.SQLQuery.ReturnString("Select CfrUSD FROM LC where LCNo='" + ddName.SelectedItem.Text + "'");
                string totalDuty = RunQuery.SQLQuery.ReturnString("SELECT SUM(TotalDutyActual) FROM LC_Items_Duty WHERE (LCNo = '" + ddName.SelectedItem.Text + "')");
                string voucherMId = SQLQuery.ReturnString("SELECT  VID FROM VoucherMaster WHERE  VoucherReferenceNo='" + ddName.SelectedValue + "' AND VoucherRefType='LCDuty'");
                if (Convert.ToDecimal(txtTtlActuDuty.Text) > 0)
                {
                    description = "Duty Paid for LC# " + ddName.SelectedItem.Text + ", LC value : " + lcValue + " USD.";
                    //VoucherEntry.TransactionEntry(ddName.SelectedItem.Text, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), accHeadID, accHeadName, description, txtTtlActuDuty.Text, "0", "0", "ImportLC", "LC", "1122334455", lName, "1");
                    if (voucherMId == "")
                    {
                        VoucherEntry.AutoVoucherEntry("11", description, accHeadID, "010101002", Convert.ToDecimal(totalDuty), ddName.SelectedValue, "LCDuty", lName, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), "1");
                        voucherMId = SQLQuery.ReturnString("SELECT VID FROM VoucherMaster WHERE  VoucherReferenceNo='" + ddName.SelectedValue + "' AND VoucherRefType='LCDuty'");
                        SQLQuery.ExecNonQry("UPDATE VoucherMaster SET Voucherpost='C' where VID='" + voucherMId + "'");
                        SQLQuery.ExecNonQry("UPDATE VoucherDetails SET ISApproved='C' where VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE VID='" + voucherMId + "')");
                    }
                    else
                    {
                        SQLQuery.ExecNonQry("DELETE VoucherMaster WHERE VID='" + voucherMId + "' ");
                        VoucherEntry.AutoVoucherEntry("11", description, accHeadID, "010101002", Convert.ToDecimal(totalDuty), ddName.SelectedValue, "LCDuty", lName, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), "1");
                        voucherMId = SQLQuery.ReturnString("SELECT VID FROM VoucherMaster WHERE  VoucherReferenceNo='" + ddName.SelectedValue + "' AND VoucherRefType='LCDuty'");
                        SQLQuery.ExecNonQry("UPDATE VoucherMaster SET Voucherpost='C' where VID='" + voucherMId + "'");
                        SQLQuery.ExecNonQry("UPDATE VoucherDetails SET ISApproved='C' where VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE VID='" + voucherMId + "')");
                    }
                }
               /* 
               //custom duty
                if (Convert.ToDecimal(txtCDRate.Text) > 0)
                {
                    description = "Custom Duty Paid for LC# " + ddName.SelectedItem.Text + ", LC value : " + lcValue + " USD.";
                    VoucherEntry.TransactionEntry(ddName.SelectedItem.Text, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), accHeadID, accHeadName, description, txtCDRate.Text, "0", "0", "ImportLC", "LC", "1122334455", lName, "1");
                    VoucherEntry.AutoVoucherEntry("11", description, accHeadID, "010101002", Convert.ToDecimal(txtCDRate.Text), ddName.SelectedItem.Text, "", lName, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), "1");
                }
                //txtAITRate
                if (Convert.ToDecimal(txtAITRate.Text) > 0)
                {
                    description = "AIT Paid for LC# " + ddName.SelectedItem.Text + ", LC value : " + lcValue + " USD.";
                    VoucherEntry.TransactionEntry(ddName.SelectedItem.Text, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), accHeadID, accHeadName, description, txtCDRate.Text, "0", "0", "ImportLC", "LC", "1122334455", lName, "1");
                    VoucherEntry.AutoVoucherEntry("11", description, accHeadID, "010101002", Convert.ToDecimal(txtAITRate.Text), ddName.SelectedItem.Text, "", lName, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), "1");
                }
                */

                //step4l.Attributes.Remove("class");
                //step4l.Attributes.Add("class", "step-dark-left");
                //step4r.Attributes.Remove("class");
                //step4r.Attributes.Add("class", "step-dark-right");

                step8l.Attributes.Remove("class");
                step8l.Attributes.Add("class", "step-dark-left");
                step7r.Attributes.Remove("class");
                step7r.Attributes.Add("class", "step-dark-right");
            }
            else
            {
                LinkButton8.Enabled = false;
                lblMsg.Attributes.Add("class", "xerp_stop");
                lblMsg.Text = "ERROR: Please input custom duty for all items.";
                lblMsg2.Attributes.Add("class", "xerp_stop");
                lblMsg2.Text = "ERROR: Please input custom duty for all items.";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }
    protected void btnSaveTransport_OnClick(object sender, EventArgs e)
    {
        try
        {
            UpdateLcDutyCalc();

            //1/6/2020 transaction transport
            string descripton = "Transport agent " + ddTransportAgency.SelectedItem.Text + " LC#" + ltrLC.Text + " " + txtTransportDescription.Text;
            string isTrIdExist = SQLQuery.ReturnString("SELECT TrID FROM Transactions WHERE InvNo='" + ddName.SelectedValue + "' AND TrGroup='ImportLC' AND TrType='Transport'");
            if (isTrIdExist == "")
            {
                //Party Transaction
                PartyTransction(ddTransportAgency.SelectedValue, ddTransportAgency.SelectedItem.Text, txtTotalTransportAmt.Text, descripton, "Transport");
            }
            else
            {
                SQLQuery.ExecNonQry("UPDATE Transactions SET Description='" + descripton + "', Cr='" + Convert.ToDecimal(txtTotalTransportAmt.Text) + "' WHERE TrID='" + isTrIdExist + "'");
            }
            //Compare to check items Duty Step Completed
            int i1 = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(EntryID) from LcItems where LCNo='" + ltrLC.Text + "'"));
            int i2 = Convert.ToInt32(RunQuery.SQLQuery.ReturnString("Select COUNT(EntryID) from LC_Items_Duty where LCNo='" + ltrLC.Text + "' AND TotalDutyActual>0"));

            string lName = Page.User.Identity.Name.ToString();
            string description;
            string accHeadID = RunQuery.SQLQuery.ReturnString("Select AccountsHeadID FROM LC where LCNo='" + ddName.SelectedItem.Text + "'");
            string accHeadName = RunQuery.SQLQuery.ReturnString("SELECT H.AccountsHeadName FROM LC  INNER JOIN HeadSetup As H ON LC.AccountsHeadID = H.AccountsHeadID WHERE (LC.LCNo = '" + ddName.SelectedItem.Text + "')");
            string lcValue = RunQuery.SQLQuery.ReturnString("Select CfrUSD FROM LC where LCNo='" + ddName.SelectedItem.Text + "'");
            string voucherMId = SQLQuery.ReturnString("SELECT  VID FROM VoucherMaster WHERE  VoucherReferenceNo='" + ddName.SelectedValue + "' AND VoucherRefType='LCTransport'");
            if (Convert.ToDecimal(txtTotalTransportAmt.Text) > 0)
            {
                description = "Transport Charge Paid to " + ddTransportAgency.SelectedItem.Text + " for LC# " + ddName.SelectedItem.Text + ", LC value : " + lcValue + " USD.";
                //VoucherEntry.TransactionEntry(ddName.SelectedItem.Text, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), accHeadID, accHeadName, description, txtTotalTransportAmt.Text, "0", "0", "ImportLC", "LC", "1122334455", lName, "1");
                if (voucherMId == "")
                {
                    VoucherEntry.AutoVoucherEntry("11", description, accHeadID, "010101002", Convert.ToDecimal(txtTotalTransportAmt.Text), ddName.SelectedValue, "LCTransport", lName, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), "1");
                    voucherMId = SQLQuery.ReturnString("SELECT VID FROM VoucherMaster WHERE  VoucherReferenceNo='" + ddName.SelectedValue + "' AND VoucherRefType='LCTransport'");
                    SQLQuery.ExecNonQry("UPDATE VoucherMaster SET Voucherpost='C' where VID='" + voucherMId + "'");
                    SQLQuery.ExecNonQry("UPDATE VoucherDetails SET ISApproved='C' where VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE VID='" + voucherMId + "')");

                }
                else
                {
                    VoucherEntry.AutoVoucherEntry("11", description, accHeadID, "010101002", Convert.ToDecimal(txtTotalTransportAmt.Text), ddName.SelectedItem.Text, "LCTransport", lName, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), "1");

                    SQLQuery.ExecNonQry("DELETE VoucherMaster WHERE VID='" + voucherMId + "' ");
                    VoucherEntry.AutoVoucherEntry("11", description, accHeadID, "010101002", Convert.ToDecimal(txtTotalTransportAmt.Text), ddName.SelectedValue, "LCTransport", lName, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), "1");
                    voucherMId = SQLQuery.ReturnString("SELECT VID FROM VoucherMaster WHERE  VoucherReferenceNo='" + ddName.SelectedValue + "' AND VoucherRefType='LCTransport'");
                    SQLQuery.ExecNonQry("UPDATE VoucherMaster SET Voucherpost='C' where VID='" + voucherMId + "'");
                    SQLQuery.ExecNonQry("UPDATE VoucherDetails SET ISApproved='C' where VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE VID='" + voucherMId + "')");


                }
                {
                }
            }
            

            if (i1 == i2)
            {
                //LinkButton2.Enabled = true;
                //LoadPanel2();


                ShowPanel(Panel5, LinkButton5);
                CalculateInsurance();
                CalculateBankExp();

                step5l.Attributes.Remove("class");
                step5l.Attributes.Add("class", "step-dark-left");
                step5r.Attributes.Remove("class");
                step5r.Attributes.Add("class", "step-dark-right");
            }
            else
            {
                LinkButton2.Enabled = false;
                lblMsg.Attributes.Add("class", "xerp_stop");
                lblMsg.Text = "ERROR: Please input custom duty for all items.";
                lblMsg2.Attributes.Add("class", "xerp_stop");
                lblMsg2.Text = "ERROR: Please input custom duty for all items.";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }

    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        try
        {
            LoadPanel2();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    private void ShowPanel(Panel pnl, LinkButton linkBtn)
    {
        Panel0.Visible = false;
        Panel1.Visible = false;
        Panel2.Visible = false;
        Panel3.Visible = false;
        Panel4.Visible = false;
        Panel5.Visible = false;
        Panel6.Visible = false;
        pnlCNFCommission.Visible = false;
        panelTransport.Visible = false;

        pnl.Visible = true;

        LinkButton0.CssClass = "";
        LinkButton1.CssClass = "";
        LinkButton2.CssClass = "";
        LinkButton3.CssClass = "";
        LinkButton4.CssClass = "";
        LinkButton5.CssClass = "";
        LinkButton6.CssClass = "";
        LinkButton7.CssClass = "";
        LinkButton8.CssClass = "";

        linkBtn.CssClass = "ActiveStep";
    }


    private void LoadPanel2()
    {
        step2l.Attributes.Remove("class");
        step2l.Attributes.Add("class", "step-dark-left");
        step2r.Attributes.Remove("class");
        step2r.Attributes.Add("class", "step-dark-right");

        ShowPanel(Panel2, LinkButton2);

        LinkButton2.Enabled = true;
        LinkButton3.Enabled = true;
        LinkButton4.Enabled = true;
        LinkButton5.Enabled = true;

        lblAVActual.Text = RunQuery.SQLQuery.ReturnString("SELECT  AV_Actual FROM LC_Duty_Calc  WHERE (LCNo = '" + ltrLC.Text + "')");
        string itemUSD = RunQuery.SQLQuery.ReturnString("SELECT CfrUSD FROM LC WHERE (LCNo = '" + ltrLC.Text + "')");
        RunQuery.SQLQuery.ExecNonQry("Update LC_Insur_Calc SET BaseUSD='" + itemUSD + "' WHERE (LCNo = '" + ltrLC.Text + "')");
        lblAVInsurance.Text = Convert.ToString(Convert.ToDecimal(itemUSD) * 1.10M); //10% of CFR VALUE

        lblCDuty.Text = RunQuery.SQLQuery.ReturnString("SELECT  TotalDutyActual FROM   LC_Duty_Calc  WHERE (LCNo = '" + ltrLC.Text + "')");

        decimal av = Convert.ToDecimal(lblAVActual.Text);
        decimal ctb1 = 0, ctb2 = 0, ctb3 = 0, ctb4 = 0;
        string desc = "";

        if (av <= 500000M)
        {
            ctb1 = av / 100M; //1% of AV.
            desc = "1% on AV: " + ctb1.ToString();
        }
        else if (av > 500000M && av <= 1000000M)
        {
            ctb1 = 500000M / 100M; //1% of first 5lac
            ctb2 = (av - 500000M) * 0.75M / 100;
            desc = "1% on First 5lac: " + ctb1.ToString() + "<br> + 0.75% on Rest Amount: " + ctb2.ToString();
        }
        else if (av > 1000000M && av <= 2000000M)
        {
            ctb1 = 500000M / 100M; //1% of first 5lac
            ctb2 = 500000M * 0.75M / 100;
            ctb3 = (av - 1000000M) * 0.50M / 100;
            desc = "1% on First 5lac: " + ctb1.ToString() + "<br> + 0.75% on Next 5Lac: " + ctb2.ToString() + "<br> + 0.50% on Rest Amount: " + ctb3.ToString();
        }
        else if (av > 2000000M)
        {
            ctb1 = 500000M / 100M; //1% of first 5lac
            ctb2 = 500000M * 0.75M / 100;//0.75% of next 5lac
            ctb3 = 1000000M * 0.50M / 100;
            ctb4 = (av - 2000000M) * 0.25M / 100;
            desc = "1% on First 5lac: " + ctb1.ToString() + "<br> + 0.75% on Next 5Lac: " + ctb2.ToString() + "<br> + 0.50% on Next 10Lac: " + ctb3.ToString() + "<br> + 0.25% on Rest Amount: " + ctb4.ToString();

        }

        lblCTB.Text = desc;
        txtTaxBasis.Text = (ctb1 + ctb2 + ctb3 + ctb4).ToString();
        RunQuery.SQLQuery.ExecNonQry("UPDATE     LC_CNFTax_Calc SET CalculatedTaxBase=" + txtTaxBasis.Text + "  WHERE LCNo='" + ltrLC.Text + "'");

        GetCnfdata();
    }

    protected void txtTaxbasActual_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CNFCommissionTax();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    private void CNFCommissionTax()
    {
        if (txtTaxbasActual.Text == "")
        {
            txtTaxbasActual.Text = "0";
        }

        if (txtActualCNFCom.Text == "")
        {
            txtActualCNFCom.Text = "0";
        }

        decimal taxBase = Convert.ToDecimal(txtTaxbasActual.Text);
        decimal vat = taxBase * Convert.ToDecimal(txtCtbVAT.Text) / 100M;
        decimal ait = taxBase * Convert.ToDecimal(txtCtbAIT.Text) / 100M;
        decimal calc_Comm_tax = vat + ait + Convert.ToDecimal(txtDocProcessing.Text) + Convert.ToDecimal(txtCSFee.Text) + Convert.ToDecimal(txtCnfOtherExp.Text);
        decimal calc_Cust_tax = calc_Comm_tax + Convert.ToDecimal(txtTtlCalcDuty.Text);
        decimal act_Cust_tax = Convert.ToDecimal(lblCDuty.Text) + Convert.ToDecimal(txtActualCNFCom.Text);

        RunQuery.SQLQuery.ExecNonQry(@"UPDATE     LC_CNFTax_Calc   SET 
                        ActualTaxBase=" + txtTaxbasActual.Text + ", VATRate=" + txtCtbVAT.Text + ", VATAmount=" + vat + ", AITRate=" + txtCtbAIT.Text + ", AITAmount=" + ait +
                        ", DocumentProcessing=" + txtDocProcessing.Text + ", CSFee=" + txtCSFee.Text + ", OtherExpense=" + txtCnfOtherExp.Text + ", CalculatedComTax=" + calc_Comm_tax +
                        ", ActualComTax=" + txtActualCNFCom.Text + ", CalculatedCustomTax=" + calc_Cust_tax + ", ActualCustomTax=" + act_Cust_tax +
                        " WHERE LCNo='" + ltrLC.Text + "'");

        GetCnfdata();

    }
    private void GetCnfdata()
    {

        SqlCommand cmd7 = new SqlCommand(@"SELECT   ActualTaxBase, VATRate, VATAmount, AITRate, AITAmount, DocumentProcessing, CSFee, OtherExpense, CalculatedComTax, 
                                        ActualComTax, ActualCustomTax
                                        FROM LC_CNFTax_Calc WHERE LCNo=@LCNo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd7.Parameters.Add("@LCNo", SqlDbType.VarChar).Value = ltrLC.Text;
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            //ActualTaxBase, VATRate, VATAmount, AITRate, AITAmount, DocumentProcessing, CSFee, OtherExpense, 
            txtTaxbasActual.Text = dr[0].ToString();
            txtCtbVAT.Text = dr[1].ToString();
            Label3.Text = dr[2].ToString();
            txtCtbAIT.Text = dr[3].ToString();
            Label4.Text = dr[4].ToString();
            txtDocProcessing.Text = dr[5].ToString();
            txtCSFee.Text = dr[6].ToString();
            txtCnfOtherExp.Text = dr[7].ToString();

            //CalculatedComTax, ActualComTax, CalculatedCustomTax, ActualCustomTax
            txtCalculatedCNFCom.Text = dr[8].ToString();
            txtActualCNFCom.Text = dr[9].ToString();
            txtTotalCNFDuty.Text = dr[10].ToString();
            SQLQuery.Empty2Zero(txtActualCNFCom);
            txtCNFComm.Text = txtActualCNFCom.Text;
        }


    }
    protected void btnPanel2_Click(object sender, EventArgs e)
    {
        try
        {

            //ShowPanel(Panel3, LinkButton3);

            //step3l.Attributes.Remove("class");
            //step3l.Attributes.Add("class", "step-dark-left");
            //step3r.Attributes.Remove("class");
            //step3r.Attributes.Add("class", "step-dark-right");
            //CalculateInsurance();

            GridView2.DataBind();
            GridView2.SelectedIndex = -1;

            ShowPanel(Panel1, LinkButton1);

            //step1l.Attributes.Remove("class");
            //step1l.Attributes.Add("class", "step-dark-left");
            //step1r.Attributes.Remove("class");
            //step1r.Attributes.Add("class", "step-dark-right");

            step8l.Attributes.Remove("class");
            step8l.Attributes.Add("class", "step-dark-left");
            step8r.Attributes.Remove("class");
            step8r.Attributes.Add("class", "step-dark-right");

            CalculateInsurance();


        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    #region : insurance //////////////////////////////////////////////////////////////////////////// INSURANCE ////////////////////////

    protected void txtInsExRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateInsurance();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }
    protected void txtRevatOnPremium_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateInsurance();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        try
        {
            ShowPanel(Panel3, LinkButton3);

            LinkButton3.Enabled = true;

            step3l.Attributes.Remove("class");
            step3l.Attributes.Add("class", "step-dark-left");
            step3r.Attributes.Remove("class");
            step3r.Attributes.Add("class", "step-dark-right");

            CalculateInsurance();
        }
        catch (Exception ex)
        {

            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    private void CalculateInsurance()
    {
        try
        {
            EmptytoZero(txtInsExRate);
            EmptytoZero(txtActInsur);
            EmptytoZero(txtMarine);
            EmptytoZero(txtWarSRCC);

            EmptytoZero(txtVATInsur);
            EmptytoZero(txtStampDuty);

            string itemUSD = lblAVInsurance.Text;//RunQuery.SQLQuery.ReturnString("SELECT SUM(CFRValue) AS Expr1 FROM LcItems WHERE (LCNo = '" + ltrLC.Text + "')");
            decimal bdtBase = Convert.ToDecimal(itemUSD) * Convert.ToDecimal(txtInsExRate.Text);
            //decimal bdtBase = Convert.ToDecimal(lblAVInsurance.Text);
            if (itemUSD == "0.00" || itemUSD == "")//New code 11/5/2020
            {
                itemUSD = RunQuery.SQLQuery.ReturnString("SELECT CfrUSD FROM LC WHERE (LCNo = '" + ltrLC.Text + "')");
                RunQuery.SQLQuery.ExecNonQry("Update LC_Insur_Calc SET BaseUSD='" + itemUSD + "' WHERE (LCNo = '" + ltrLC.Text + "')");
                bdtBase = Convert.ToDecimal(itemUSD) * Convert.ToDecimal(txtInsExRate.Text);
            }


            //decimal marine = bdtBase * Convert.ToDecimal(txtMarine.Text) / 100M;
            decimal marine = Convert.ToDecimal(txtMarine.Text);
            //decimal warSrcc = bdtBase * Convert.ToDecimal(txtWarSRCC.Text) / 100M;
            decimal warSrcc = Convert.ToDecimal(txtWarSRCC.Text);
            decimal vat = (marine + warSrcc) * (Convert.ToDecimal(txtVATInsur.Text) / 100M);

            decimal stamp = Convert.ToDecimal(txtStampDuty.Text); //bdtBase * Convert.ToDecimal(txtStampDuty.Text) / 100M;

            decimal calcinsurAmt = marine + warSrcc + vat + stamp;
            decimal DiscountAmt = (marine + warSrcc) * (Convert.ToDecimal(txtRevatOnPremium.Text) / 100M);
            decimal insurToBePaid = (calcinsurAmt - DiscountAmt);
            txtActInsur.Text = insurToBePaid.ToString(CultureInfo.InvariantCulture);

            RunQuery.SQLQuery.ExecNonQry(@"UPDATE     LC_Insur_Calc   SET 
                        BaseUSD=" + itemUSD + ", ExchRate=" + txtInsExRate.Text + ", BaseBDT=" + bdtBase + ", MarineRate=" + txtMarine.Text + ", MarineAmt=" + marine +
                            ", WarSrccRate=" + txtWarSRCC.Text + ", WarSrccAmt=" + warSrcc + ", VatRate=" + txtVATInsur.Text + ", VatAmt=" + vat +
                            ", StampDutyRate=" + 1 + ", StampDutyAmount=" + stamp + ", CalculatedInsuranceAmt=" + calcinsurAmt + ", DiscountRate=" + txtRevatOnPremium.Text + ",DiscountAmt=" + DiscountAmt + ", ActualInsuranceAmt=" + txtActInsur.Text +
                            " WHERE LCNo='" + ltrLC.Text + "'");

            GetInsuranceData();
        }
        catch (Exception ex)
        {

            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    private void EmptytoZero(TextBox txtBox)
    {
        if (txtBox.Text == "")
        {
            txtBox.Text = "0";
        }
    }

    private void GetInsuranceData()
    {
        SqlCommand cmd7 = new SqlCommand(@"SELECT   BaseUSD, ExchRate, BaseBDT, MarineRate, MarineAmt, WarSrccRate, WarSrccAmt, VatRate, VatAmt, StampDutyRate, StampDutyAmount, 
                         CalculatedInsuranceAmt, ActualInsuranceAmt, DiscountRate, DiscountAmt
                                        FROM    LC_Insur_Calc WHERE LCNo=@LCNo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd7.Parameters.Add("@LCNo", SqlDbType.VarChar).Value = ltrLC.Text;
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            //ActualTaxBase, VATRate, VATAmount, AITRate, AITAmount, DocumentProcessing, CSFee, OtherExpense, 
            lblAVInsurance.Text = dr[0].ToString();
            txtInsExRate.Text = dr[1].ToString();
            txtInsurBDT.Text = dr[2].ToString();
            txtMarine.Text = dr[3].ToString();
            Label7.Text = dr[4].ToString();
            txtWarSRCC.Text = dr[5].ToString();
            Label8.Text = dr[6].ToString();
            txtVATInsur.Text = dr[7].ToString();

            //CalculatedComTax, ActualComTax, CalculatedCustomTax, ActualCustomTax
            Label5.Text = dr[8].ToString();
            txtStampDuty.Text = dr[10].ToString();
            //Label6.Text = dr[10].ToString();
            txtCalcInsur.Text = dr[11].ToString();
            LblDiscountAmt.Text = dr["DiscountAmt"].ToString();
            txtActInsur.Text = dr[12].ToString();
        }
    }

    protected void btnPanel3_Click(object sender, EventArgs e)
    {
        try
        {
            //ShowPanel(Panel4, LinkButton4);

            //CalculateInsurance();
            //CalculateBankExp();

            //step4l.Attributes.Remove("class");
            //step4l.Attributes.Add("class", "step-dark-left");
            //step4r.Attributes.Remove("class");
            //step4r.Attributes.Add("class", "step-dark-right");

            //LinkButton2.Enabled = true;
            //LoadPanel2();

            //1/6/2020 transaction insurance

            string isTrIdExist = SQLQuery.ReturnString("SELECT TrID FROM Transactions WHERE InvNo='" + ddName.SelectedValue + "' AND TrGroup='ImportLC' AND TrType='Insurance'");
            if (isTrIdExist == "")
            {
                //Party Transaction
                string descripton = "Insurance company " + txtInsurance.Text + " LC#" + ltrLC.Text;
                PartyTransction(InsuranceIdHField.Value, txtInsurance.Text, txtActInsur.Text, descripton, "Insurance");
            }
            else
            {
                SQLQuery.ExecNonQry("UPDATE Transactions SET  Cr='" + Convert.ToDecimal(txtActInsur.Text) + "' WHERE TrID='" + isTrIdExist + "'");
            }

            GridView2.DataBind();
            GridView2.SelectedIndex = -1;

            //ShowPanel(Panel1, LinkButton1);
            ShowPanel(Panel4, LinkButton4);

            //step1l.Attributes.Remove("class");
            //step1l.Attributes.Add("class", "step-dark-left");
            //step8r.Attributes.Remove("class");
            //step8r.Attributes.Add("class", "step-dark-right");
            step4l.Attributes.Remove("class");
            step4l.Attributes.Add("class", "step-dark-left");
            step3r.Attributes.Remove("class");
            step3r.Attributes.Add("class", "step-dark-right");

            CalculateInsurance();

            string lName = Page.User.Identity.Name.ToString();
            string description;
            string accHeadID = RunQuery.SQLQuery.ReturnString("Select AccountsHeadID FROM LC where LCNo='" + ddName.SelectedItem.Text + "'");
            string accHeadName = RunQuery.SQLQuery.ReturnString("SELECT H.AccountsHeadName FROM LC  INNER JOIN HeadSetup As H ON LC.AccountsHeadID = H.AccountsHeadID WHERE (LC.LCNo = '" + ddName.SelectedItem.Text + "')");
            string voucherMId = SQLQuery.ReturnString("SELECT VID FROM VoucherMaster WHERE  VoucherReferenceNo='" + ddName.SelectedValue + "' AND VoucherRefType='LCInsurance'");
            
            if (Convert.ToDecimal(txtActInsur.Text) > 0)
            {
                description = "Insurance Premium Paid to " + txtInsurance.Text + " for LC# " + ddName.SelectedItem.Text + ", LC value : " + txtActInsur.Text;
                //VoucherEntry.TransactionEntry(ddName.SelectedItem.Text, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), accHeadID, accHeadName, description, txtActInsur.Text, "0", "0", "ImportLC", "LCVoucher", "1122334455", lName, "1");
                
                if (voucherMId == "")
                {
                    VoucherEntry.AutoVoucherEntry("11", description, accHeadID, "010101002", Convert.ToDecimal(txtActInsur.Text), ddName.SelectedValue, "LCInsurance", lName, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), "1");
                    voucherMId = SQLQuery.ReturnString("SELECT VID FROM VoucherMaster WHERE  VoucherReferenceNo='" + ddName.SelectedValue + "' AND VoucherRefType='LCInsurance'");
                    SQLQuery.ExecNonQry("UPDATE VoucherMaster SET Voucherpost='C' where VID='" + voucherMId + "'");
                    SQLQuery.ExecNonQry("UPDATE VoucherDetails SET ISApproved='C' where VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE VID='" + voucherMId + "')");
                }
                else
                {
                    voucherMId = SQLQuery.ReturnString("SELECT VID FROM VoucherMaster WHERE  VoucherReferenceNo='" + ddName.SelectedValue + "' AND VoucherRefType='LCInsurance' AND Voucherpost='A'");
                    if (voucherMId == "")
                    {
                        SQLQuery.ExecNonQry("DELETE VoucherMaster WHERE VID='" + voucherMId + "' ");
                        VoucherEntry.AutoVoucherEntry("11", description, accHeadID, "010101002", Convert.ToDecimal(txtActInsur.Text), ddName.SelectedValue, "LCInsurance", lName, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), "1");

                        voucherMId = SQLQuery.ReturnString("SELECT VID FROM VoucherMaster WHERE  VoucherReferenceNo='" + ddName.SelectedValue + "' AND VoucherRefType='LCInsurance'");
                        SQLQuery.ExecNonQry("UPDATE VoucherMaster SET Voucherpost='C' where VID='" + voucherMId + "'");
                        SQLQuery.ExecNonQry("UPDATE VoucherDetails SET ISApproved='C' where VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE VID='" + voucherMId + "')");
                    }
                    
                }
            }


        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }




    #endregion Insurance ///////////////
    #region : Bank Charges //////////////////////////////////////////////////////////////////////////// BANK CHARGES ////////////////////////

    protected void btnPanel4_Click(object sender, EventArgs e)
    {
        try
        {
            //ShowPanel(Panel5, LinkButton5);
            ShowPanel(Panel1, LinkButton1);

            step1l.Attributes.Remove("class");
            step1l.Attributes.Add("class", "step-dark-left");
            step8r.Attributes.Remove("class");
            step8r.Attributes.Add("class", "step-dark-right");

            CalculateInsurance();
            CalculateBankExp();

            //step5l.Attributes.Remove("class");
            //step5l.Attributes.Add("class", "step-dark-left");
            //step5r.Attributes.Remove("class");
            //step5r.Attributes.Add("class", "step-dark-right");
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    protected void txtBankExchRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateBankExp();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    private void CalculateBankExp()
    {
        EmptytoZero(txtBankExchRate);
        EmptytoZero(txtActBankInterAmt);
        EmptytoZero(txtBankComRate);
        EmptytoZero(txtBankProcessRate);
        EmptytoZero(txtBankVAT);
        EmptytoZero(txtBankAmendment);
        EmptytoZero(txtBankOther);
        EmptytoZero(txtBankttlCharge);
        EmptytoZero(txtBankInterestRate);
        EmptytoZero(txtBankTenor);
        EmptytoZero(txtCalcInterAmt);
        EmptytoZero(txtActBankInterAmt);
        EmptytoZero(txtSwift);

        string itemUSD = RunQuery.SQLQuery.ReturnString("SELECT CfrUSD FROM LC WHERE (LCNo = '" + ltrLC.Text + "')");//RunQuery.SQLQuery.ReturnString("SELECT SUM(CFRValue) AS Expr1 FROM LcItems WHERE (LCNo = '" + ltrLC.Text + "')");
        decimal bdtBase = Convert.ToDecimal(itemUSD) * Convert.ToDecimal(txtBankExchRate.Text);
        txtBankBDT.Text = bdtBase.ToString();

        decimal bCom = bdtBase * toDecmlPercent(txtBankComRate.Text);
        decimal bProcess = bdtBase * toDecmlPercent(txtBankProcessRate.Text);
        decimal bVat = Convert.ToDecimal(txtBankVAT.Text); //bdtBase * toDecmlPercent(txtBankVAT.Text);

        Label10.Text = Convert.ToDecimal(bdtBase * toDecmlPercent(txtBankComRate.Text)).ToString("#.##");
        Label11.Text = Convert.ToDecimal(bdtBase * toDecmlPercent(txtBankProcessRate.Text)).ToString("#.##");
        Label12.Text = txtBankVAT.Text; //Convert.ToDecimal(bdtBase * toDecmlPercent(txtBankVAT.Text)).ToString("#.##");

        decimal bTtlCharge = bCom + bProcess + bVat + Convert.ToDecimal(txtBankAmendment.Text) + Convert.ToDecimal(txtSwift.Text) + Convert.ToDecimal(txtBankOther.Text);
        txtBankttlCharge.Text = Convert.ToString(bTtlCharge.ToString("#.00"));

        decimal bInterest = Convert.ToDecimal(lblLTR.Text) * toDecmlPercent(txtBankInterestRate.Text);
        Label9.Text = Convert.ToString(bInterest);

        decimal interestTtl = bInterest * (Convert.ToDecimal(txtBankTenor.Text) / 360M);
        decimal calcIntAmt = bTtlCharge + interestTtl;
        txtCalcInterAmt.Text = interestTtl.ToString("#.00");
        txtBankTotalCalc.Text = calcIntAmt.ToString("#.00");

        RunQuery.SQLQuery.ExecNonQry(@"UPDATE LC_Bank_Calc SET 
                        CFRUSD=" + itemUSD + ", ExchRate=" + txtInsExRate.Text + ", CFRBDT=" + bdtBase + ", LTR=" + lblLTR.Text + ", Margin=" + lblMargin.Text +
                        ", CommRate=" + txtBankComRate.Text + ", CommAmt=" + bCom + ", ProcessingRate=" + txtBankProcessRate.Text + ", ProcessingAmt=" + bProcess +
                        ", VatRate=" + txtBankVAT.Text + ", VatAmt=" + bVat + ", AmendmentAmt=" + txtBankAmendment.Text + ", SwiftAmt=" + txtSwift.Text + " , OtherAmt=" + txtBankOther.Text +
                        ", TotalCharge=" + bTtlCharge + ", InterestRate=" + txtBankInterestRate.Text + ", InterestAmt=" + bInterest + ", Tenor=" + txtBankTenor.Text +
                        ", CalculatedInterest=" + calcIntAmt + ", CalcTotal=" + calcIntAmt + ", ActualInterest=" + txtActBankInterAmt.Text +
                        " WHERE LCNo='" + ltrLC.Text + "'");

        //GetbankExpData();

    }

    private decimal toDecmlPercent(string amt)
    {
        return Convert.ToDecimal(amt) / 100M;
    }

    private void GetBankExpData()
    {
        SqlCommand cmd7 = new SqlCommand(@"SELECT   CFRUSD, ExchRate, CFRBDT, LTR, Margin, CommRate, CommAmt, ProcessingRate, ProcessingAmt, VatRate, VatAmt, AmendmentAmt, OtherAmt, 
                                                    TotalCharge, InterestRate, InterestAmt, Tenor, CalculatedInterest, ActualInterest, SwiftAmt, CalcTotal
                                        FROM LC_Bank_Calc WHERE LCNo=@LCNo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd7.Parameters.Add("@LCNo", SqlDbType.VarChar).Value = ltrLC.Text;
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            //ActualTaxBase, VATRate, VATAmount, AITRate, AITAmount, DocumentProcessing, CSFee, OtherExpense, 
            lblBankCFR.Text = dr[0].ToString();
            txtBankExchRate.Text = dr[1].ToString();
            txtBankBDT.Text = dr[2].ToString();
            lblLTR.Text = dr[3].ToString();
            lblMargin.Text = dr[4].ToString();
            txtBankComRate.Text = dr[5].ToString();
            Label10.Text = dr[6].ToString();
            txtBankProcessRate.Text = dr[7].ToString();

            //CalculatedComTax, ActualComTax, CalculatedCustomTax, ActualCustomTax
            Label11.Text = dr[8].ToString();
            txtBankVAT.Text = dr[9].ToString();
            Label12.Text = dr[10].ToString();
            txtBankAmendment.Text = dr[11].ToString();
            txtBankOther.Text = dr[12].ToString();

            txtBankttlCharge.Text = dr[13].ToString();
            txtBankInterestRate.Text = dr[14].ToString();
            Label9.Text = dr[15].ToString();
            txtBankTenor.Text = dr[16].ToString();
            txtCalcInterAmt.Text = dr[17].ToString();
            txtActBankInterAmt.Text = dr[18].ToString();

            txtSwift.Text = dr[19].ToString();
            txtBankTotalCalc.Text = dr[18].ToString();

        }
    }


    #endregion Bank Charges////////////////



    private void SaveExpenses()
    {

        if (txtAmount.Text != "" && ddHead.SelectedValue != "")
        {
            SqlCommand cmd2 = new SqlCommand("INSERT INTO LC_Expenses (LCno, TypeID, HeadID, Expdate, Amount, Description, EntryBy)" +
                                        "VALUES (@LCno, @TypeID, @HeadID, @Expdate, @Amount, @Description, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            string lName = Page.User.Identity.Name.ToString();

            cmd2.Parameters.Add("@LCno", SqlDbType.NVarChar).Value = ddName.SelectedValue;
            cmd2.Parameters.Add("@TypeID", SqlDbType.NVarChar).Value = ddType.SelectedValue;
            cmd2.Parameters.Add("@HeadID", SqlDbType.NVarChar).Value = ddHead.SelectedValue;
            cmd2.Parameters.Add("@Expdate", SqlDbType.DateTime).Value = txtDate.Text;
            cmd2.Parameters.Add("@Amount", SqlDbType.Decimal).Value = txtAmount.Text;
            cmd2.Parameters.Add("@Description", SqlDbType.NVarChar).Value = txtDescription.Text;
            cmd2.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName.Trim();

            cmd2.Connection.Open();
            int success = cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();

            txtAmount.Text = "";
            lblMsg.Attributes.Add("class", "xerp_success");
            lblMsg.Text = "The expense successfully added to database...";
        }
        else
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: Please fillup all mendatory fields!";
        }
    }

    protected void LinkButton4_Click(object sender, EventArgs e)
    {
       
        try
        {
            ShowPanel(Panel4, LinkButton4);

            LinkButton4.Enabled = true;

            step4l.Attributes.Remove("class");
            step4l.Attributes.Add("class", "step-dark-left");
            step3r.Attributes.Remove("class");
            step3r.Attributes.Add("class", "step-dark-right");
            //step4r.Attributes.Remove("class");
            //step4r.Attributes.Add("class", "step-dark-right");
            //LinkButton5.Enabled = true;
            //LinkButton6.Enabled = true;
        }
        catch (Exception ex)
        {

            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }
    protected void LinkButton5_Click(object sender, EventArgs e)
    {
        ShowPanel(Panel5, LinkButton5);

        LinkButton5.Enabled = true;

        step5l.Attributes.Remove("class");
        step5l.Attributes.Add("class", "step-dark-left");
        step5r.Attributes.Remove("class");
        step5r.Attributes.Add("class", "step-dark-right");

    }
    protected void LinkButton6_Click(object sender, EventArgs e)
    {
        ShowPanel(Panel6, LinkButton6);

        LinkButton6.Enabled = true;

        step6l.Attributes.Remove("class");
        step6l.Attributes.Add("class", "step-dark-left");
        step6r.Attributes.Remove("class");
        step6r.Attributes.Add("class", "step-dark-right");

        CalcLCCosting();
    }
    protected void LinkButton8_OnClick(object sender, EventArgs e)
    {
        //    ShowPanel(Panel3, LinkButton7);

        //    LinkButton7.Enabled = true;

        //    step3l.Attributes.Remove("class");
        //    step3l.Attributes.Add("class", "step-dark-left");
        //    step3r.Attributes.Remove("class");
        //    step3r.Attributes.Add("class", "step-dark-right");
    }


    decimal total = 0;
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Amount"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[5].Text = "Total";
            e.Row.Cells[6].Text = Convert.ToString(total);
            total = 0;
        }
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label PID = GridView1.Rows[index].FindControl("Label1") as Label;
            lblEditESL.Text = PID.Text;

            SqlCommand cmd7 = new SqlCommand("SELECT TypeID, HeadID, Expdate, Amount, Description FROM [LC_Expenses] WHERE esl='" + PID.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                btnSave.Text = "Update";

                ddType.SelectedValue = dr[0].ToString();
                ddHead.DataBind();
                ddHead.SelectedValue = dr[1].ToString();
                txtDate.Text = Convert.ToDateTime(dr[2].ToString()).ToString("dd/MM/yyyy");
                txtAmount.Text = dr[3].ToString();
                string country = dr[4].ToString();
                txtDescription.Text = country;
            }
            cmd7.Connection.Close();

            lblMsg.Attributes.Add("class", "xerp_info");
            lblMsg.Text = "Edit mode activated ...";
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text == "Update")
            {
                RunQuery.SQLQuery.ExecNonQry("UPDATE LC_Expenses SET TypeID='" + ddType.SelectedValue + "', HeadID='" + ddHead.SelectedValue + "', Expdate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', Amount='" + txtAmount.Text + "', Description='" + txtDescription.Text + "' WHERE esl='" + lblEditESL.Text + "'");

                lblMsg.Attributes.Add("class", "xerp_info");
                lblMsg.Text = "Entry Updated Successfully!";
                btnSave.Text = "Save";
            }
            else
            {
                SaveExpenses();
            }

            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }





    private void CalcLCCosting()
    {
        try
        {
            SQLQuery.Empty2Zero(txtTotalCNFDuty);
            SQLQuery.Empty2Zero(txtLcImpCost);

            txtBankBdt2.Text = txtBankBDT.Text;
            //txtTotalCNFDuty2.Text = txtTotalCNFDuty.Text;//Comment date 17/5/2020
            txtTotalCNFDuty2.Text = SQLQuery.ReturnString(@"SELECT ISNULL(SUM(TotalDutyActual),0) FROM LC_Duty_Calc WHERE LCNo='" + ltrLC.Text + "'");
            txtActInsurance.Text = txtActInsur.Text;
            //txtCnfCharge.Text = txtCNFComm.Text; //Comment date 17/5/2020
            txtCnfCharge.Text = txtTotalAmtCnf.Text;
            txtBankCost.Text = txtBankttlCharge.Text;
            txtBInterest.Text = txtActBankInterAmt.Text;

            string otherExpense = SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM LC_Expenses WHERE LCno='" + ddName.SelectedValue + "'");
            string transportExpense = SQLQuery.ReturnString("Select ISNULL(SUM(TotalTransportAmt),0) FROM LC_Transport WHERE LCNo='" + ddName.SelectedValue + "'");
            txtMisc.Text = (Convert.ToDecimal(otherExpense) + Convert.ToDecimal(transportExpense)).ToString();

            txtLcImpCost.Text = Convert.ToString(Conv2Dec(txtTotalCNFDuty2) + Conv2Dec(txtActInsurance) + Conv2Dec(txtCnfCharge) + Conv2Dec(txtBankCost) + Conv2Dec(txtBInterest) + Conv2Dec(txtMisc));

            SQLQuery.ExecNonQry("DELETE LC_Items_Costing where LCNo='" + ltrLC.Text + "'");


            //Item Import Coting Calculation

            SqlDataAdapter da;
            SqlDataReader dr;
            DataSet ds;
            int recordcount = 0;
            int ic = 0;

            //pid, InvoiceNo, Date, PartyName, ResellerName, Type, BankName, Server, Amount, CRate, ChargeAmount, CurrencyName, CurrencyRate, Dr, Cr, Balance, 
            //                 Remarks, TrType, TrName, EntryBy, EntryDate, VoucherNo
            SqlCommand cmd2 = new SqlCommand(@"SELECT  EntryID, ItemCode, ((SELECT BrandName from Brands WHERE BrandID= LcItems.ItemSizeID) +' - '+ Thickness +' - '+ Measurement) AS Desc1, 
                                            qty, UnitPrice, CFRValue
                            FROM [LcItems] WHERE ([LCNo] = @LCNo)  ORDER BY [EntryID] ASC", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2.Parameters.Add("@LCNo", SqlDbType.VarChar).Value = ltrLC.Text;

            da = new SqlDataAdapter(cmd2);
            ds = new DataSet("Board");

            cmd2.Connection.Open();
            da.Fill(ds, "Board");

            dr = cmd2.ExecuteReader();
            recordcount = ds.Tables[0].Rows.Count;


            if (recordcount > 0)
            {
                do
                {
                    string itemCode = ds.Tables[0].Rows[ic]["ItemCode"].ToString();
                    string itemName = RunQuery.SQLQuery.ReturnString("Select ItemName from Products where ProductID='" + itemCode + "'");
                    string desc = ds.Tables[0].Rows[ic]["Desc1"].ToString();
                    string qty = ds.Tables[0].Rows[ic]["qty"].ToString();
                    string unit = RunQuery.SQLQuery.ReturnString("Select UnitType from Products where ProductID='" + itemCode + "'");
                    string price = ds.Tables[0].Rows[ic]["UnitPrice"].ToString();
                    string itemCFR = ds.Tables[0].Rows[ic]["CFRValue"].ToString();

                    decimal iBankBDT = Convert.ToDecimal(txtBankExchRate.Text) * Convert.ToDecimal(itemCFR);
                    decimal itemCfrBdt = Convert.ToDecimal(itemCFR) * Conv2Dec(txtCustomExRate);

                    //decimal cDuty = (Convert.ToDecimal(txtCDAmt.Text)) * itemCfrBdt / Conv2Dec(txtAVActual);
                    decimal cVat = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("Select VATAmt from  LC_Items_Duty where ItemCode='" + itemCode + "' AND LCNo='" + ltrLC.Text + "'"));
                    decimal cAit = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("Select AITAmt from  LC_Items_Duty where ItemCode='" + itemCode + "' AND LCNo='" + ltrLC.Text + "'"));
                    decimal cAtv = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("Select ATVAmt from  LC_Items_Duty where ItemCode='" + itemCode + "' AND LCNo='" + ltrLC.Text + "'"));

                    decimal cDuty = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("Select TotalDutyActual from  LC_Items_Duty where ItemCode='" + itemCode + "' AND LCNo='" + ltrLC.Text + "'")); //(Conv2Dec(txtTtlActuDuty) - cVat - cAit - cAtv) * itemCfrBdt / Conv2Dec(txtAVActual);
                    decimal itemVA = cVat + cAtv; //(cVat * itemCfrBdt / Conv2Dec(txtAVActual)) + (cAtv * itemCfrBdt / Conv2Dec(txtAVActual));
                    decimal itemVAA = itemVA + cAit;
                    //decimal cAtvItem = Convert.ToDecimal(txtATVAmt.Text); 
                    //decimal cDutyVA = (cDuty+) * itemCfrBdt / Conv2Dec(txtAVActual);

                    decimal cnfComm = Conv2Dec(txtCnfCharge) * iBankBDT / Conv2Dec(txtBankBdt2);
                    decimal insurance = Conv2Dec(txtActInsur) * iBankBDT / Conv2Dec(txtBankBdt2);
                    //decimal cnfCharge = Conv2Dec(txtTaxbasActual) * iBankBDT / Conv2Dec(txtBankBdt2);//comment date 17/5/2020
                    decimal cnfCharge = Conv2Dec(txtCnfCharge);
                    decimal lcOpCost = Conv2Dec(txtBankttlCharge) * iBankBDT / Conv2Dec(txtBankBdt2);
                    decimal bankInterest = Conv2Dec(txtActBankInterAmt) * iBankBDT / Conv2Dec(txtBankBdt2);
                    decimal others = Conv2Dec(txtMisc) * iBankBDT / Conv2Dec(txtBankBdt2);

                    decimal iTotal = iBankBDT + cDuty + cnfComm + insurance + cnfCharge + lcOpCost + bankInterest + others;
                    decimal costKgVAA = (iTotal - itemVAA) / Convert.ToDecimal(qty);
                    decimal costKgVA = (iTotal - itemVA) / Convert.ToDecimal(qty);

                    RunQuery.SQLQuery.ExecNonQry(@"INSERT INTO LC_Items_Costing (LCNo, ItemCode, ItemName, ItemDescription, qty, unit, BankBDT, CustomDuty, VatAtv, VatAtvAit, CnfComTax, Insurance, CnfCharge, LcOpCost,                                                                              BankInterest, Others, TotalImportCost, CostPerUnit, CostPerUnitVAA, CostPerUnitVA)" +
                                            "VALUES ('" + ltrLC.Text + "', '" + itemCode + "', '" + itemName + "', '" + desc + "', '" + qty + "', '" + unit + "', '" + iBankBDT + "', '" + cDuty + "', '" + itemVA + "', '" + itemVAA + "', '" + cnfComm + "', '" + insurance + "', '" + cnfCharge + "', '" + lcOpCost + "', '" + bankInterest + "', '" + others + "', '" + iTotal + "',  '" + costKgVAA + "', '" + costKgVAA + "', '" + costKgVA + "')");

                    ic++;

                } while (ic < recordcount);
            }
            else
            {
                //GridView1.DataSource = null;
            }

            GridView3.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
        }
    }
    private decimal Conv2Dec(TextBox tb)
    {
        return Convert.ToDecimal(tb.Text);
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            ShowPanel(Panel6, LinkButton6);

            //CalculateInsurance();
            //CalculateBankExp();
            CalcLCCosting();

            step6l.Attributes.Remove("class");
            step6l.Attributes.Add("class", "step-dark-left");
            step6r.Attributes.Remove("class");
            step6r.Attributes.Add("class", "step-dark-right");
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }
    protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CalcDuty();

            int index = Convert.ToInt32(GridView2.SelectedIndex);
            Label lblItemName = GridView2.Rows[index].FindControl("Label1") as Label;
            lblItemEntryId.Text = lblItemName.Text;

            string iCode = RunQuery.SQLQuery.ReturnString("Select ItemCode from LcItems where EntryID='" + lblItemEntryId.Text + "'");

            Label lblTotalUsd = GridView2.Rows[index].FindControl("lblTotal") as Label;
            txtiTtlUSD.Text = lblTotalUsd.Text;
            decimal totalBDT = Convert.ToDecimal(Conv2Dec(txtiTtlUSD) * Conv2Dec(txtCustomExRate));
            txtiTtlBDT.Text = totalBDT.ToString("#.##");

            decimal insuranceAmt = totalBDT * (1 / 100M);//1% insurence
            txtInsurAmount.Text = insuranceAmt.ToString("#.##"); //dr[4].ToString();


            string isExist = SQLQuery.ReturnString("Select EntryID from LC_Items_Duty where EntryID='" + lblItemName.Text + "'");
            if (isExist == "")
            {
                SQLQuery.Empty2Zero(txtInsurAmount);
                SQLQuery.ExecNonQry(@"INSERT INTO   LC_Items_Duty (EntryID, LCNo, ItemCode, InsuranceAmount)" +
                                        "VALUES ('" + lblItemEntryId.Text + "', '" + ltrLC.Text + "', '" + iCode + "', '" + txtInsurAmount.Text + "')");

            }

            LoadDutyData(lblItemName.Text);
            CalcAV();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }

    private void LoadDutyData(string entryId)
    {
        try
        {
            SqlCommand cmd7 = new SqlCommand(@"Select InsuranceAmount, PenaltyDesc, AV_Calculated, AV_Actual, CustomsDutyRate, CustomsDutyAmt, RDRate, RDAmt, 
                         SDRate, SDAmt, SurChargeRate, SurChargeAmt, VATRate, VATAmt, AITRate, AITAmt, ATVRate, ATVAmt, TotalDutyCalculated, TotalDutyActual, PenaltyAmt, ATAmt, DFCVATFP FROM [LC_Items_Duty] WHERE EntryID=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Parameters.Add("@sl", SqlDbType.VarChar).Value = entryId;
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                txtInsurPercent.Text = dr[0].ToString();
                txtOtherDesc.Text = dr[1].ToString();
                txtAVCalc.Text = dr[2].ToString();

                txtAVActual.Text = dr[3].ToString();
                txtCDRate.Text = dr[4].ToString();
                txtCDAmt.Text = dr[5].ToString();
                txtRDRate.Text = dr[6].ToString();
                txtRDAmt.Text = dr[7].ToString();

                txtSDRate.Text = dr[8].ToString();
                txtSDAmt.Text = dr[9].ToString();
                txtSurChRate.Text = dr[10].ToString();
                txtSurChAmt.Text = dr[11].ToString();
                txtVATRate.Text = dr[12].ToString();

                txtVATAmt.Text = dr[13].ToString();
                txtAITRate.Text = dr[14].ToString();
                txtAITAmt.Text = dr[15].ToString();
                txtATAmt.Text = dr["ATAmt"].ToString();
                txtATVRate.Text = dr[16].ToString();
                txtATVAmt.Text = dr[17].ToString();
                txtDFCVATFP.Text = dr["DFCVATFP"].ToString();

                txtTtlCalcDuty.Text = dr[18].ToString();
                //txtTtlActuDuty.Text = dr[19].ToString();
                txtTtlActuDuty.Text = dr["TotalDutyCalculated"].ToString();
                txtOtherAmount.Text = dr[20].ToString();

                cmd7.Connection.Close();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }


    protected void txtTotalTruck_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateTransport();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    protected void txtTruckRate_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateTransport();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    protected void txtTransportDescription_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateTransport();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    private void CalculateTransport()
    {
        string lName = Page.User.Identity.Name.ToString();
        EmptytoZero(txtTotalTruck);
        EmptytoZero(txtTruckRate);
        EmptytoZero(txtTotalTransportAmt);

        decimal totalTransportAmt = Convert.ToDecimal(txtTotalTruck.Text) * Convert.ToDecimal(txtTruckRate.Text);
        string existLcNo = SQLQuery.ReturnString(@"SELECT TransportID FROM LC_Transport WHERE LCNo='" + ddName.SelectedValue + "'");
        if (existLcNo == "")
        {

            SqlCommand cmd2 = new SqlCommand("INSERT INTO LC_Transport (LCNo, PartyID, TotalTruck, Rate, TotalTransportAmt, Description, EntryBy)" +
                                        "VALUES (@LCNo, @PartyID, @TotalTruck, @Rate, @TotalTransportAmt, @Description, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.Add("@LCNo", SqlDbType.NVarChar).Value = ddName.SelectedValue;
            cmd2.Parameters.Add("@PartyID", SqlDbType.Int).Value = ddTransportAgency.SelectedValue;
            cmd2.Parameters.Add("@TotalTruck", SqlDbType.Int).Value = txtTotalTruck.Text;
            cmd2.Parameters.Add("@Rate", SqlDbType.Decimal).Value = txtTruckRate.Text;
            cmd2.Parameters.Add("@TotalTransportAmt", SqlDbType.Decimal).Value = totalTransportAmt;
            cmd2.Parameters.Add("@Description", SqlDbType.NVarChar).Value = txtTransportDescription.Text;
            cmd2.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName.Trim();

            cmd2.Connection.Open();
            int success = cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
        }
        else
        {
            RunQuery.SQLQuery.ExecNonQry(@"UPDATE LC_Transport SET PartyID='" + ddTransportAgency.SelectedValue + "', TotalTruck='" + txtTotalTruck.Text + "',  Rate='" + txtTruckRate.Text + "', TotalTransportAmt='" + Convert.ToDecimal(totalTransportAmt) + "', Description='" + txtTransportDescription.Text + "', EntryBy='" + lName + "'  WHERE LCNo= '" + ddName.SelectedValue + "'");
        }

        txtTotalTransportAmt.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(TotalTransportAmt),0) FROM LC_Transport WHERE TransportID='" + existLcNo + "' ");
    }

    protected void btnCNFCommNew_OnClick(object sender, EventArgs e)
    {
        try
        {
            string isTrIdExist = SQLQuery.ReturnString("SELECT TrID FROM Transactions WHERE InvNo='" + ddName.SelectedValue + "' AND TrGroup='ImportLC' AND TrType='CNF'");
            string descripton = "CNF agent" + ddCNFAgent.SelectedItem.Text + " LC#" + ltrLC.Text + " " + txtDescriptionCnf.Text;
            if (isTrIdExist == "")
            {
                //Party Transaction
                PartyTransction(ddCNFAgent.SelectedValue, ddCNFAgent.SelectedItem.Text, txtTotalAmtCnf.Text, descripton, "CNF");
            }
            else
            {
                SQLQuery.ExecNonQry("UPDATE Transactions SET Description='" + descripton + "', Cr='" + Convert.ToDecimal(txtTotalAmtCnf.Text) + "' WHERE TrID='" + isTrIdExist + "'");
            }
            ShowPanel(panelTransport, LinkButton7);
            try
            {
                SqlCommand cmd7 = new SqlCommand(@"SELECT TOP (200) TransportID, LCNo, PartyID, TotalTruck, Rate, TotalTransportAmt, Description, EntryBy, EntryDate
                        FROM LC_Transport WHERE LCNo=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Parameters.Add("@sl", SqlDbType.VarChar).Value = ddName.SelectedValue;
                cmd7.Connection.Open();
                SqlDataReader dr = cmd7.ExecuteReader();
                if (dr.Read())
                {

                    txtTotalTruck.Text = dr["TotalTruck"].ToString();
                    txtTruckRate.Text = dr["Rate"].ToString();
                    txtTotalTransportAmt.Text = dr["TotalTransportAmt"].ToString();
                    txtTransportDescription.Text = dr["Description"].ToString();

                    EditField.Attributes.Remove("class");
                    EditField.Attributes.Add("class", "control-group");

                    cmd7.Connection.Close();
                }

            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.Message.ToString();
            }
            string lName = Page.User.Identity.Name.ToString();
            string description;
            string accHeadID = RunQuery.SQLQuery.ReturnString("Select AccountsHeadID FROM LC where LCNo='" + ddName.SelectedItem.Text + "'");
            string accHeadName = RunQuery.SQLQuery.ReturnString("SELECT H.AccountsHeadName FROM LC  INNER JOIN HeadSetup As H ON LC.AccountsHeadID = H.AccountsHeadID WHERE (LC.LCNo = '" + ddName.SelectedItem.Text + "')");
            string lcValue = RunQuery.SQLQuery.ReturnString("Select CfrUSD FROM LC where LCNo='" + ddName.SelectedItem.Text + "'");
            string voucherMId = SQLQuery.ReturnString("SELECT VID FROM VoucherMaster WHERE  VoucherReferenceNo='" + ddName.SelectedValue + "' AND VoucherRefType='LCCNF'");
            if (Convert.ToDecimal(txtTotalAmtCnf.Text) > 0)
            {
                description = "CnF Duty Paid to " + ddCNFAgent.SelectedItem.Text + " for LC# " + ddName.SelectedItem.Text + ", LC value : " + lcValue + " USD.";
                //VoucherEntry.TransactionEntry(ddName.SelectedItem.Text, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), accHeadID, accHeadName, description, txtTotalAmtCnf.Text, "0", "0", "ImportLC", "LC", "1122334455", lName, "1");
                if (voucherMId == "")
                {
                    VoucherEntry.AutoVoucherEntry("11", description, accHeadID, "010101002", Convert.ToDecimal(txtTotalAmtCnf.Text), ddName.SelectedValue, "LCCNF", lName, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), "1");
                    voucherMId = SQLQuery.ReturnString("SELECT VID FROM VoucherMaster WHERE  VoucherReferenceNo='" + ddName.SelectedValue + "' AND VoucherRefType='LCCNF'");
                    SQLQuery.ExecNonQry("UPDATE VoucherMaster SET Voucherpost='C' where VID='" + voucherMId + "'");
                    SQLQuery.ExecNonQry("UPDATE VoucherDetails SET ISApproved='C' where VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE VID='" + voucherMId + "')");
                }
                else
                {
                    SQLQuery.ExecNonQry("DELETE VoucherMaster WHERE VID='" + voucherMId + "' ");
                    VoucherEntry.AutoVoucherEntry("11", description, accHeadID, "010101002", Convert.ToDecimal(txtTotalAmtCnf.Text), ddName.SelectedValue, "LCCNF", lName, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd"), "1");
                    voucherMId = SQLQuery.ReturnString("SELECT VID FROM VoucherMaster WHERE  VoucherReferenceNo='" + ddName.SelectedValue + "' AND VoucherRefType='LCCNF'");
                    SQLQuery.ExecNonQry("UPDATE VoucherMaster SET Voucherpost='C' where VID='" + voucherMId + "'");
                    SQLQuery.ExecNonQry("UPDATE VoucherDetails SET ISApproved='C' where VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE VID='" + voucherMId + "')");
                }
            }
            

            step7l.Attributes.Remove("class");
            step7l.Attributes.Add("class", "step-dark-left");
            step4r.Attributes.Remove("class");
            step4r.Attributes.Add("class", "step-dark-right");
            //step1r.Attributes.Remove("class");
            //step1r.Attributes.Add("class", "step-dark-right");

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }

    }

    protected void txtDescriptionCnf_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateCnfNew();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    private void CalculateCnfNew()
    {
        string lName = Page.User.Identity.Name.ToString();
        EmptytoZero(txtPortCharge);
        EmptytoZero(txtShippingCharge);
        EmptytoZero(txtReceiptCnf);
        EmptytoZero(txtMiscellaneousCnf);
        EmptytoZero(txtOtherChargeCnf);
        EmptytoZero(txtCommissionCnf);
        EmptytoZero(txtTotalAmtCnf);

        decimal totalCnfAmt = Convert.ToDecimal(txtPortCharge.Text) + Convert.ToDecimal(txtShippingCharge.Text) + Convert.ToDecimal(txtReceiptCnf.Text) + Convert.ToDecimal(txtMiscellaneousCnf.Text) + Convert.ToDecimal(txtOtherChargeCnf.Text) + Convert.ToDecimal(txtCommissionCnf.Text);
        string existLcNo = SQLQuery.ReturnString(@"SELECT CnfID FROM LC_CNF WHERE LCNo='" + ddName.SelectedValue + "'");
        if (existLcNo == "")
        {

            SqlCommand cmd2 = new SqlCommand("INSERT INTO [dbo].[LC_CNF] ([LCNo], [PartyID], [PortCharge], [ShipingCharge], [ReceiptAmt], [Miscellaneous], [OtherCharge],[Commission],[TotalAmount], [Description],[EntryBy])" +
                                        "VALUES (@LCNo, @PartyID, @PortCharge, @ShipingCharge, @ReceiptAmt, @Miscellaneous, @OtherCharge, @Commission,@TotalAmount, @Description, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.Add("@LCNo", SqlDbType.NVarChar).Value = ddName.SelectedValue;
            cmd2.Parameters.Add("@PartyID", SqlDbType.Int).Value = ddCNFAgent.SelectedValue;
            cmd2.Parameters.Add("@PortCharge", SqlDbType.Decimal).Value = txtPortCharge.Text;
            cmd2.Parameters.Add("@ShipingCharge", SqlDbType.Decimal).Value = txtShippingCharge.Text;
            cmd2.Parameters.Add("@ReceiptAmt", SqlDbType.Decimal).Value = txtReceiptCnf.Text;
            cmd2.Parameters.Add("@Miscellaneous", SqlDbType.Decimal).Value = txtMiscellaneousCnf.Text;
            cmd2.Parameters.Add("@OtherCharge", SqlDbType.Decimal).Value = txtOtherChargeCnf.Text;
            cmd2.Parameters.Add("@Commission", SqlDbType.Decimal).Value = txtCommissionCnf.Text;
            cmd2.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = totalCnfAmt;
            cmd2.Parameters.Add("@Description", SqlDbType.NVarChar).Value = txtDescriptionCnf.Text;
            cmd2.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName.Trim();

            cmd2.Connection.Open();
            int success = cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
        }
        else
        {
            RunQuery.SQLQuery.ExecNonQry(@"UPDATE LC_CNF SET PartyID='" + ddCNFAgent.SelectedValue + "', PortCharge='" + txtPortCharge.Text + "',  ShipingCharge='" + txtShippingCharge.Text + "',ReceiptAmt='" + txtReceiptCnf.Text + "',Miscellaneous='" + txtMiscellaneousCnf.Text + "',OtherCharge='" + txtOtherChargeCnf.Text + "',Commission='" + txtCommissionCnf.Text + "', TotalAmount='" + Convert.ToDecimal(totalCnfAmt) + "', Description='" + txtDescriptionCnf.Text + "', EntryBy='" + lName + "'  WHERE LCNo= '" + ddName.SelectedValue + "'");
        }

        txtTotalAmtCnf.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(TotalAmount),0) FROM LC_CNF WHERE CnfID='" + existLcNo + "' ");
    }

    private void PartyTransction(string partyId, string partyName, string totalAmount, string description, string partyType)
    {
        //Party Transaction
        string lName = Page.User.Identity.Name.ToString();
        string accHead = RunQuery.SQLQuery.ReturnString("Select AccHeadID FROM Settings_Transaction where TransactionType='ImportLC'");
        Accounting.VoucherEntry.TransactionEntry(ddName.SelectedValue, txtDate.Text, partyId, partyName, description, "0", totalAmount, "0", "ImportLC", partyType, accHead, lName, "1");
    }

    protected void txtPortCharge_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateCnfNew();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    protected void txtShippingCharge_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateCnfNew();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    protected void txtReceiptCnf_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateCnfNew();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    protected void txtMiscellaneousCnf_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateCnfNew();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    protected void txtOtherChargeCnf_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateCnfNew();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

    protected void txtCommissionCnf_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            CalculateCnfNew();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
            txtAVActual.Focus();
        }
    }

   


    protected void Button1_OnClick(object sender, EventArgs e)
    {
        //GridView2.DataBind();
        //GridView2.SelectedIndex = -1;

        //ShowPanel(Panel1, LinkButton1);

        //step1l.Attributes.Remove("class");
        //step1l.Attributes.Add("class", "step-dark-left");
        //step1r.Attributes.Remove("class");
        //step1r.Attributes.Add("class", "step-dark-right");

        //CalculateInsurance();

        ShowPanel(Panel3, LinkButton3);

        LinkButton1.Enabled = true;

        step3l.Attributes.Remove("class");
        step3l.Attributes.Add("class", "step-dark-left");
        step1r.Attributes.Remove("class");
        step1r.Attributes.Add("class", "step-dark-right");

        CalculateInsurance();
    }
}