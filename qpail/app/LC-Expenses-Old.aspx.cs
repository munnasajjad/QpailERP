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
using RunQuery;

public partial class app_LC_Expenses_Old : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToShortDateString();
            ddName.DataBind();
            ddType.DataBind();
            ddHead.DataBind();

            LoadLCData();
            GridView1.DataBind();
        }
    }
    protected void ddName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadLCData();
    }
    protected void ddType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddHead.DataBind();
        GridView1.DataBind();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveExpenses();
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }
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


    private void LoadLCData()
    {
        try
        {
            SqlCommand cmd7 = new SqlCommand("Select LCNo, OpenDate, Category, LCType, HSCode, LcRef, LcFor, ShipDate, ExpiryDate, SupplierID, Origin, AgentID, InsuranceID, CnfID, BankId, LcMargin, LTR, BankExcRate, CustomExcRate, QtyUnit, TotalQty, Freight, CfrUSD, CfrBDT, Remarks, TransportMode, LCCloseBy, LCClosedate, ProjectID, EntryBy FROM [LC] WHERE sl=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Parameters.Add("@sl", SqlDbType.VarChar).Value = ddName.SelectedValue;
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                //btnSave.Text = "Update";
                lblLCNo.Text = dr[0].ToString();
                lblOpDate.Text = Convert.ToDateTime(dr[1].ToString()).ToShortDateString();
                lblGrp.Text = SQLQuery.ReturnString("Select GroupName FROM ItemGroup where GroupSrNo = " + dr[2].ToString());
                lblLCType.Text = dr[3].ToString();
                //txtHSCode.Text = dr[4].ToString();
                //txtReferrence.Text = dr[5].ToString();
                lblDept.Text = dr[6].ToString();
                lblShipDate.Text = Convert.ToDateTime(dr[7].ToString()).ToShortDateString();
                lblExDate.Text = Convert.ToDateTime(dr[8].ToString()).ToShortDateString();
                lblSupplier.Text = SQLQuery.ReturnString("Select Company FROM Party where PartyID = " + dr[9].ToString());
                lblCountry.Text = dr[10].ToString();
                lblagent.Text = SQLQuery.ReturnString("Select Company FROM Party where PartyID = " + dr[11].ToString());
                ddInsurance.Text = SQLQuery.ReturnString("Select BankName FROM Banks where BankId = " + dr[12].ToString());
                txtCNF.Text = SQLQuery.ReturnString("Select Company FROM Party where PartyID = " + dr[13].ToString());
                lblBank.Text = SQLQuery.ReturnString("Select BankName FROM Banks where BankId = " + dr[14].ToString());
                txtMargin.Text = dr[15].ToString();
                txtLTR.Text = dr[16].ToString();
                txtBExRate.Text = dr[17].ToString();
                txtCExRate.Text = dr[18].ToString();
                //txtQty.Text = dr[19].ToString();
                txtTtlQty.Text = dr[20].ToString();
                txtFreight.Text = dr[21].ToString();
                txtCfrUSD.Text = dr[22].ToString();
                txtCfrBDT.Text = dr[23].ToString();
                txtRemarks.Text = dr[24].ToString();
                ddMode.Text = dr[25].ToString();

                //btnCancel.Visible = true;
                //ltrSubFrmName.Text = "Edit Party";
                //lblMsg.Attributes.Add("class", "xerp_info");
                //lblMsg.Text = "Info loaded in edit mode.";
                EditField.Attributes.Remove("class");
                EditField.Attributes.Add("class", "control-group");

                //GridView1.DataBind();
                //pnl.Update();
            }
            cmd7.Connection.Close();

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }



    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
}