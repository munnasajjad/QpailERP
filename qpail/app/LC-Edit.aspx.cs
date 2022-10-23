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

public partial class app_LC_Edit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //txtName.Focus();
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            lblLogin.Text = lName;

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            LoadFormInfo();

            ddGroup.DataBind();
            string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
            GetProductList();
            GridView2.DataBind();
        }
    }
    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox) { (Parent as TextBox).Text = string.Empty; } else { foreach (Control c in Parent.Controls) ClearControls(c); }
    }
    private void LoadFormInfo()
    {
        string projectID = Convert.ToString(Session["ProjectID"]);
        string formtype = base.Request.QueryString["type"];
        lblProject.Text = projectID;

            EditField.Attributes.Remove("class");
            EditField.Attributes.Add("class", "control-group");
            LCNoInput.Attributes.Remove("class");
            LCNoInput.Attributes.Add("class", "control-group hidden");
            ltrFrmName.Text = "LC Amendment";
            ltrSubFrmName.Text = "Edit LC Information";

            btnSave.Text = "Update LC Info";
            ddName.DataBind();
            EditMode();        
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtNo.Text != "")
            {
                
                    UpdateInfo();
                    Saveinfo();
                    EditMode();
                    //RefreshForm();

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Info successfully updated for " + ddName.SelectedItem.Text;
                //}
                //else
                //{
                //    Saveinfo();
                //    RefreshForm();

                //    lblMsg.Attributes.Add("class", "xerp_success");
                //    lblMsg.Text = "New LC Saved Successfully.";
                //}
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Please Enter LC No.";
            }

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.Message.ToString();
        }
    }

    private void Saveinfo()
    {
        string formtype = base.Request.QueryString["type"];
        int projectID = Convert.ToInt32(lblProject.Text);
        string lName = Page.User.Identity.Name.ToString();
        
        SqlCommand cmd2 = new SqlCommand("UPDATE LcItems SET LCNo='" + txtNo.Text + "' where (LCNo ='' AND EntryBy ='" + lblLogin.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
    }

    protected void ddName_SelectedIndexChanged(object sender, EventArgs e)
    {
        EditMode();        
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //int index = Convert.ToInt32(GridView1.SelectedIndex);
        //Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        //ddName.SelectedValue = lblItemName.Text;

        //EditMode();
    }

    private void EditMode()
    {
        try
        {
            SqlCommand cmd7 = new SqlCommand("Select LCNo, OpenDate, Category, LCItem, HSCode, LcRef, LcFor, ShipDate, ExpiryDate, SupplierID, Origin, AgentID, InsuranceID, CnfID, BankId, LcMargin, LTR, BankExcRate, CustomExcRate, QtyUnit, TotalQty, Freight, CfrUSD, CfrBDT, Remarks, TransportMode, BankBDT, LCCloseBy, LCClosedate, ProjectID, EntryBy FROM [LC] WHERE sl=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Parameters.Add("@sl", SqlDbType.VarChar).Value = ddName.SelectedValue;
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                //btnSave.Text = "Update";
                txtNo.Text = dr[0].ToString();
                txtDate.Text = Convert.ToDateTime(dr[1].ToString()).ToShortDateString();
                ddGroup.SelectedValue = dr[2].ToString();
                //txtItem.Text = dr[3].ToString();
                txtHSCode.Text = dr[4].ToString();
                //txtReferrence.Text = dr[5].ToString();
                txtFor.Text = dr[6].ToString();
                txtShipDate.Text = Convert.ToDateTime(dr[7].ToString()).ToShortDateString();
                txtExpiryDate.Text = Convert.ToDateTime(dr[8].ToString()).ToShortDateString();
                ddManufacturer.SelectedValue = dr[9].ToString();
                ddCountry.SelectedValue = dr[10].ToString();
                ddAgent.SelectedValue = dr[11].ToString();
                ddInsurance.SelectedValue = dr[12].ToString();
                ddCNF.SelectedValue = dr[13].ToString();
                ddBank.SelectedValue = dr[14].ToString();
                txtMargin.Text = dr[15].ToString();
                txtLTR.Text = dr[16].ToString();
                txtBExRate.Text = dr[17].ToString();
                txtCExRate.Text = dr[18].ToString();
                txtQty.Text = dr[19].ToString();
                txtTtlQty.Text = dr[20].ToString();
                txtFreight.Text = dr[21].ToString();
                txtCfrUSD.Text = dr[22].ToString();
                txtCfrBDT.Text = dr[23].ToString();
                txtRemarks.Text = dr[24].ToString();
                ddMode.Text = dr[25].ToString();
                txtBankBDT.Text = dr[26].ToString();

                btnCancel.Visible = true;
                //ltrSubFrmName.Text = "Edit Party";
                lblMsg.Attributes.Add("class", "xerp_info");
                lblMsg.Text = "Info loaded in edit mode.";
                EditField.Attributes.Remove("class");
                EditField.Attributes.Add("class", "control-group");

                //GridView1.DataBind();
                //pnl.Update();
            }
            cmd7.Connection.Close();

            ltrFormTitle2.Text = txtNo.Text;
            GridView2.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }


    private void UpdateInfo()
    {
        string lName = Page.User.Identity.Name.ToString();
        //get old data first
        string oldCompany = "", oldZone = "", oldBal = "", oldDueLimit = "", oldRef = "";
        SqlCommand cmd7 = new SqlCommand("Select LCNo, LCType, OpenDate, Category, LcFor, ShipDate, ExpiryDate,"+
            " SupplierID, Origin, AgentID, InsuranceID, CnfID, BankId, LcMargin, LTR, BankExcRate, CustomExcRate, QtyUnit, TotalQty, Freight, CfrUSD, CfrBDT, Remarks, TransportMode, LCCloseBy, LCClosedate, ProjectID, EntryBy FROM [LC] WHERE sl=@sl", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Parameters.Add("@sl", SqlDbType.VarChar).Value = ddName.SelectedValue;
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO LChistory (LCNo, LCType, OpenDate, Category, LCItem, HSCode, LcRef, LcFor, ShipDate, ExpiryDate, SupplierID, Origin, AgentID, InsuranceID, CnfID, BankId, LcMargin, " +
                                                "LTR, BankExcRate, CustomExcRate, QtyUnit, TotalQty, Freight, CfrUSD, CfrBDT, BankBDT, Remarks, TransportMode, LCCloseBy, LCClosedate, ProjectID, EntryBy)" +
                                    "VALUES (@LCNo, @LCType, @OpenDate, @Category, @LCItem, @HSCode, @LcRef, @LcFor, @ShipDate, @ExpiryDate, @SupplierID, @Origin, @AgentID, @InsuranceID, @CnfID, @BankId, @LcMargin, " +
                                    "@LTR, @BankExcRate, @CustomExcRate, @QtyUnit, @TotalQty, @Freight, @CfrUSD, @CfrBDT,'"+txtBankBDT.Text+"', @Remarks, @TransportMode, @LCCloseBy, @LCClosedate, @ProjectID, @EntryBy)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@LCNo", dr[0].ToString());
            cmd.Parameters.AddWithValue("@LCType", dr[1].ToString());
            cmd.Parameters.AddWithValue("@OpenDate", Convert.ToDateTime(dr[2].ToString()));
            cmd.Parameters.AddWithValue("@Category", dr[3].ToString());
            cmd.Parameters.AddWithValue("@LCItem", "");//txtItem.Text);
            cmd.Parameters.AddWithValue("@HSCode", txtHSCode.Text);
            cmd.Parameters.AddWithValue("@LcRef", "");// txtReferrence.Text);
            cmd.Parameters.AddWithValue("@LcFor", dr[4].ToString());
            cmd.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(dr[5].ToString()));
            cmd.Parameters.AddWithValue("@ShipDate", Convert.ToDateTime(dr[6].ToString()));
            cmd.Parameters.AddWithValue("@SupplierID", dr[7].ToString());
            cmd.Parameters.AddWithValue("@Origin", dr[8].ToString());

            cmd.Parameters.AddWithValue("@AgentID", dr[9].ToString());
            cmd.Parameters.AddWithValue("@InsuranceID", dr[10].ToString());
            cmd.Parameters.AddWithValue("@CnfID", dr[11].ToString());
            cmd.Parameters.AddWithValue("@BankId", dr[12].ToString());
            cmd.Parameters.AddWithValue("@LcMargin",dr[13].ToString());
            cmd.Parameters.AddWithValue("@LTR", dr[14].ToString());

            cmd.Parameters.AddWithValue("@BankExcRate", dr[15].ToString());
            cmd.Parameters.AddWithValue("@CustomExcRate", dr[16].ToString());
            cmd.Parameters.AddWithValue("@QtyUnit", dr[17].ToString());
            cmd.Parameters.AddWithValue("@TotalQty", dr[18].ToString());
            cmd.Parameters.AddWithValue("@Freight", dr[19].ToString());

            cmd.Parameters.AddWithValue("@CfrUSD", dr[20].ToString());
            cmd.Parameters.AddWithValue("@CfrBDT", dr[21].ToString());
            cmd.Parameters.AddWithValue("@Remarks", dr[22].ToString());
            cmd.Parameters.AddWithValue("@TransportMode",dr[23].ToString());
            cmd.Parameters.AddWithValue("@LCCloseBy", "");
            cmd.Parameters.AddWithValue("@LCClosedate", "");
            cmd.Parameters.AddWithValue("@ProjectID", Convert.ToInt32(lblProject.Text));
            cmd.Parameters.AddWithValue("@EntryBy", lName);

            cnn.Open();
            int Success = cmd.ExecuteNonQuery();
            cnn.Close();


        }
        cmd7.Connection.Close();


        //Create Sql Connection
        SqlConnection cnn3 = new SqlConnection();
        cnn3.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        //Create Sql Command
        SqlCommand cmd3 = new SqlCommand();
        cmd3.CommandText = "UPDATE LC SET LCNo =@LCNo, LCType =@LCType, OpenDate =@OpenDate, Category =@Category, HSCode='"+txtHSCode.Text+"', LcFor =@LcFor, " +
             "ExpiryDate =@ExpiryDate, ShipDate =@ShipDate, SupplierID =@SupplierID, Origin =@Origin, AgentID =@AgentID, InsuranceID =@InsuranceID,"+
              "CnfID =@CnfID, BankId =@BankId, LcMargin =@LcMargin, LTR =@LTR, BankExcRate =@BankExcRate, CustomExcRate =@CustomExcRate," +
               "QtyUnit =@QtyUnit, TotalQty =@TotalQty, Freight =@Freight, CfrUSD =@CfrUSD, CfrBDT =@CfrBDT, BankBDT='"+txtBankBDT.Text+"', " +
            " Remarks =@Remarks, TransportMode =@TransportMode WHERE sl='" + ddName.SelectedValue + "'";
        cmd3.CommandType = CommandType.Text;
        cmd3.Connection = cnn3;

        cmd3.Parameters.AddWithValue("@LCNo", txtNo.Text);
        cmd3.Parameters.AddWithValue("@LCType", ddType.SelectedValue);
        cmd3.Parameters.AddWithValue("@OpenDate", Convert.ToDateTime(txtDate.Text));
        cmd3.Parameters.AddWithValue("@Category", ddGroup.SelectedValue);
        cmd3.Parameters.AddWithValue("@LcFor", txtFor.Text);
        cmd3.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(txtExpiryDate.Text));
        cmd3.Parameters.AddWithValue("@ShipDate", Convert.ToDateTime(txtShipDate.Text));
        cmd3.Parameters.AddWithValue("@SupplierID", ddManufacturer.SelectedValue);
        cmd3.Parameters.AddWithValue("@Origin", ddCountry.SelectedValue);

        cmd3.Parameters.AddWithValue("@AgentID", ddAgent.SelectedValue);
        cmd3.Parameters.AddWithValue("@InsuranceID", ddInsurance.SelectedValue);
        cmd3.Parameters.AddWithValue("@CnfID", ddCNF.SelectedValue);
        cmd3.Parameters.AddWithValue("@BankId", ddBank.SelectedValue);
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        RefreshForm();
        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Action Cancelled!";
    }

    private void RefreshForm()
    {
        ClearControls(Form);
        GridView1.DataBind();
        ddName.DataBind();
        //pnl.Update();
        //GridView1.EditIndex = -1;
        EditField.Attributes.Remove("class");
        EditField.Attributes.Add("class", "control-group hidden");

        btnCancel.Visible = false;
        LoadFormInfo();
    }


    protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='" + ddGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

        gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
        GetProductList();
    }
    protected void ddSubGrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGrp.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");

        gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
        GetProductList();
    }
    protected void ddGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
        GetProductList();
    }
    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetProductList();
    }

    private void GetProductList()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategory.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemName, "ProductID", "ItemName");

        ltrUnitType.Text = RunQuery.SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddItemName.SelectedValue + "'");

        //ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (txtQty.Text == "")
            {
                txtQty.Text = "0";
            }
            if (txtPrice.Text == "")
            {
                txtPrice.Text = "0";
            }
            if (txtCFR.Text == "")
            {
                txtCFR.Text = "0";
            }
            SqlCommand cmde = new SqlCommand("SELECT ItemCode FROM LcItems WHERE ItemCode ='" + ddItemName.SelectedValue + "' AND  LCNo ='" + ddName.SelectedItem.Text + "' AND EntryBy ='" + lblLogin.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmde.Connection.Open();
            string isExist = Convert.ToString(cmde.ExecuteScalar());
            cmde.Connection.Close();

            if (isExist == "")
            {
                SqlCommand cmd2 = new SqlCommand("INSERT INTO LcItems (LCNo, ItemCode, HSCode, ItemSizeID, Measurement, qty, UnitPrice, CFRValue, EntryBy) VALUES (@LCNo, @ItemCode, @HSCode, @ItemSizeID, @Measurement, @qty, @UnitPrice, @CFRValue, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.AddWithValue("@LCNo", ddName.SelectedItem.Text);
                cmd2.Parameters.AddWithValue("@ItemCode", ddItemName.SelectedValue);
                cmd2.Parameters.AddWithValue("@HSCode", "");// txtHSCode.Text);
                cmd2.Parameters.AddWithValue("@ItemSizeID", Convert.ToInt32(ddSize.SelectedValue));
                cmd2.Parameters.AddWithValue("@Measurement", txtMeasure.Text);

                cmd2.Parameters.AddWithValue("@qty", txtQty.Text);
                cmd2.Parameters.AddWithValue("@UnitPrice", txtPrice.Text);
                cmd2.Parameters.AddWithValue("@CFRValue", txtCFR.Text);
                cmd2.Parameters.AddWithValue("@EntryBy", lName);

                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();

                txtQty.Text = "";
                txtPrice.Text = "";
                txtCFR.Text = "";
            }
            else
            {
                lblMsg2.Attributes.Add("class", "xerp_warning");
                lblMsg2.Text = "ERROR: Item Already exist! Delete from grid first...";
            }

            ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
        }
        catch(Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "ERROR: "+ex.Message.ToString();
        }
        finally{
            GridView1.DataBind();
        }
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
    }


    private void LoadOldData(int lcSl)
    {
        try
        {
            GridView2.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Unable to load party. " + ex.Message.ToString();
        }
    }

}