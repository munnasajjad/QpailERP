using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_LC_Preview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string lName = Page.User.Identity.Name.ToString();

                SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Connection.Open();
                cmd.Connection.Close();
                
                //Load Old LC
                int loadSl = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(max(sl),0) from LC"));
                string previewLc = Request.QueryString["ref"];
                if (previewLc!=null)
                {
                    lblLCNo.Text = previewLc;
                    loadSl = Convert.ToInt32(SQLQuery.ReturnString("Select sl from LC where LCNo='" + previewLc + "'"));
                }
                
                LoadOldData(loadSl);
                GridView2.DataBind();
                GridView3.DataBind();
                GridView4.DataBind();
                
                //string formtype = Request.QueryString["type"];
                //if (formtype == "edit")
                //{
                //    Page.Title = "LC Amendment";

                //    ltrFrmName.Text = "LC Amendment";\

                //    RightPanel2.Visible = true;
                //    GridView3.DataBind();
                //}

            }

        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
            lblMsg.Text = ex.ToString();
        }
        finally
        {
            
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

    private void LoadOldData(int lcSl)
    {
        try
        {
            SqlCommand cmd7 = new SqlCommand("Select sl, LCNo, LCType, OpenDate, Category, LCItem, HSCode, LcRef, LcFor, ExpiryDate, ShipDate, SupplierID, Origin, AgentID, InsuranceID, CnfID, BankId, " +
                    "LcMargin, LTR, BankExcRate, CustomExcRate, QtyUnit, TotalQty, Freight, CfrUSD, CfrBDT, Remarks, TransportMode, IsActive, LCCloseBy, LCClosedate, BankBDT, " +
                    " ArrivalDate, DeliveryDate, Status FROM [LC] WHERE sl=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
                txtReferrence.Text = SQLQuery.ReturnString("Select BrandName from RefItems where BrandID='"+dr[5].ToString()+"'") ;
                lblDept.Text = dr[8].ToString();
                lblExDate.Text = Convert.ToDateTime(dr[9].ToString()).ToShortDateString();
                lblShipDate.Text = Convert.ToDateTime(dr[10].ToString()).ToShortDateString();

                lblSupplier.Text = SQLQuery.ReturnString("Select Company FROM Party where PartyID = " + dr[11].ToString());
                lblCountry.Text = dr[12].ToString();
                lblagent.Text = SQLQuery.ReturnString("Select Company FROM Party where PartyID = " + dr[13].ToString());
                lblInsurance.Text =
                    SQLQuery.ReturnString("Select BankName FROM Banks where BankId = " + dr[14].ToString());
                lblCNF.Text = SQLQuery.ReturnString("Select Company FROM Party where PartyID = " + dr[15].ToString());

                string acc = SQLQuery.ReturnString("Select (Select BankName FROM Banks where BankId =BankAccounts.BankID)+' - '+ ACNo +' - '+ACName FROM BankAccounts where ACID = " + dr[16].ToString());
                lblBank.Text = acc;

                lblMargin.Text = dr[17].ToString();
                lblLTR.Text = dr[18].ToString();
                lblBankExcRate.Text = dr[19].ToString();
                lblCustExRate.Text = dr[20].ToString();
                //lblqt = dr[20].ToString();
                lblTtlQty.Text = dr[22].ToString();
                lblFreight.Text = dr[23].ToString();
                lblCfrUsd.Text = dr[24].ToString();
                lblCfrBdt.Text = dr[25].ToString();

                lblRemark.Text = dr[26].ToString();
                lblMode.Text = dr[27].ToString();

                //string isActive = dr[28].ToString();
                //if (isActive == "A")
                //{
                //    isActive = "Pending Shipment";
                //}
                //else if (isActive == "P")
                //{
                //    isActive = "On Port";
                //}
                //else if (isActive == "D")
                //{
                //    isActive = "Delivered";
                //}
                //else if (isActive == "S")
                //{
                //    isActive = "Closed by " + dr[29].ToString();
                //    lblStatusColor.Attributes.Add("class", "xerp_green");
                //}
                //else if (isActive == "C")
                //{
                //    isActive = "Canceled by " + dr[29].ToString();
                //    lblStatusColor.Attributes.Add("class", "xerp_red");
                //}

                //lblStatus.Text = dr[34].ToString(); 
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

                GridView4.DataBind();
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

}