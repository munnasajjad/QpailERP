using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using RunQuery;

public partial class app_Party : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //txtName.Focus();
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            LoadFormInfo();
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

    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox) { (Parent as TextBox).Text = string.Empty; } else { foreach (Control c in Parent.Controls) ClearControls(c); }
    }
    private void LoadFormInfo()
    {
        string projectID = SQLQuery.ReturnString("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + Page.User.Identity.Name.ToString() + "'");
        string formtype = base.Request.QueryString["type"];
        lblProject.Text = projectID;

        if (formtype == "customer")
        {
            Page.Title = "Customers";
            ltrFrmName.Text = "Customer Info";
            ltrSubFrmName.Text = "Setup a new Customer";
            ddPartyType.SelectedValue = formtype;
            lblZone.Text = "Sales Zone";
            lblReferrer.Text = "Sales Representative";
            lblRefItems.Text = "Selling Products";

            divCountry.Attributes.Remove("class");
            divCountry.Attributes.Add("class", "control-group");
            MatuDaysField.Attributes.Remove("class");
            MatuDaysField.Attributes.Add("class", "control-group");
            CreditLimitField.Attributes.Remove("class");
            CreditLimitField.Attributes.Add("class", "control-group");
            divOpBalance.Attributes.Remove("class");
            divOpBalance.Attributes.Add("class", "control-group");
            PopulateSR("sr", projectID);

            TermsPanel.Visible = true;
        }
        else if (formtype == "supplier")
        {
            Page.Title = "Manufacturers";
            ltrFrmName.Text = "Manufacturers Info";
            ltrSubFrmName.Text = "Setup a new Manufacturer";
            ddPartyType.SelectedValue = formtype;
            lblZone.Text = "Zone/City";
            lblReferrer.Text = "Reffered By";
            PopulateSR("Ref", projectID);

            lblCompany.Text = "Manufacturer Name";
            //ZoneField.Attributes.Remove("class");
            //ZoneField.Attributes.Add("class", "control-group");
        }
        else if (formtype == "agents")
        {
            Page.Title = "LC Agents";
            ltrFrmName.Text = "Agents Info";
            ltrSubFrmName.Text = "Setup LC Agents";
            ddPartyType.SelectedValue = formtype;
            lblZone.Text = "Location/City";
            lblCompany.Text = "LC Agent Name";
            PopulateSR("Ref", projectID);
            //ZoneField.Attributes.Remove("class");
            //ZoneField.Attributes.Add("class", "control-group");
        }
        else if (formtype == "cnf")
        {
            Page.Title = "CNF Agents";
            ltrFrmName.Text = "C&F Agents";
            ltrSubFrmName.Text = "Setup a new C&F Agent";
            ddPartyType.SelectedValue = formtype;
            lblZone.Text = "lblCompany";
            lblCompany.Text = "C&F Agent Name";
            PopulateSR("Ref", projectID);
            divOpBalance.Attributes.Remove("class");
            divOpBalance.Attributes.Add("class", "control-group");
        }
        else if (formtype == "transport")
        {
            Page.Title = "Transport Agents";
            ltrFrmName.Text = "Transport Agents";
            ltrSubFrmName.Text = "Setup a new Transport  Agent";
            ddPartyType.SelectedValue = formtype;
            lblZone.Text = "lblCompany";
            lblCompany.Text = "Transport Agent Name";
            PopulateSR("Ref", projectID);
            divOpBalance.Attributes.Remove("class");
            divOpBalance.Attributes.Add("class", "control-group");
        }
        else if (formtype == "insurance")
        {
            div1.Visible = false;
            divIncCode.Visible = true;
            Page.Title = "Insurance Company";
            ltrFrmName.Text = "Insurance Company";
            ltrSubFrmName.Text = "Setup a new Insurance Company";
            ddPartyType.SelectedValue = formtype;
            lblZone.Text = "lblCompany";
            lblCompany.Text = "Insurance Company Name";
            PopulateSR("Ref", projectID);
            divOpBalance.Attributes.Remove("class");
            divOpBalance.Attributes.Add("class", "control-group");
        }
        else if (formtype == "vendor")
        {
            Page.Title = "Suppliers";
            ltrFrmName.Text = "Suppliers/Vendors";
            ltrSubFrmName.Text = "Setup a new Vendor";
            ddPartyType.SelectedValue = formtype;
            lblZone.Text = "Purchase Zone";
            lblReferrer.Text = "Referred By";
            lblRefItems.Text = "Purchasing Items";

            divCountry.Attributes.Remove("class");
            divCountry.Attributes.Add("class", "control-group");
            divOpBalance.Attributes.Remove("class");
            divOpBalance.Attributes.Add("class", "control-group");
            PopulateSR("Ref", projectID);
        }

        txtCompany.Focus();
    }

    private void PopulateSR(string type, string prjID)
    {
        string query = "SELECT ReferrersID,[ReferrerName] FROM [Referrers] WHERE (([projectID] = " + prjID + ") AND ([Type] = 'sr')) ORDER BY [ReferrerName]";
        if (type == "Ref")
        {
            query = "SELECT ReferrersID,[ReferrerName] FROM [Referrers] WHERE (([projectID] = " + prjID + ") AND ([Type] = 'Ref')) ORDER BY [ReferrerName]";
        }
        SqlCommand cmd = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader Grouplist = cmd.ExecuteReader();

        ddReferrerID.DataSource = Grouplist;
        ddReferrerID.DataValueField = "ReferrersID";
        ddReferrerID.DataTextField = "ReferrerName";
        ddReferrerID.DataBind();

        cmd.Connection.Close();
        cmd.Connection.Dispose();
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCompany.Text != "")
            {
                if (btnSave.Text == "Update" && ddName.Visible == true)
                {
                    UpdateInfo();
                    RunQuery.SQLQuery.ActivityLog(Request.QueryString["type"], "Update", "Customer ID: " + ddName.SelectedValue + ", Name:" + txtCompany.Text,
                        Page.User.Identity.Name.ToString());

                    RefreshForm();

                    Notify("Updated Successfully", "success", lblMsg);
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Info successfully updated for " + ddName.SelectedItem.Text;
                }
                else
                {
                    Saveinfo();
                    RunQuery.SQLQuery.ActivityLog(Request.QueryString["type"], "Save", "Customer Name: " + txtCompany.Text,
                        Page.User.Identity.Name.ToString());

                    RefreshForm();

                    txtBalance.Text = "0";
                    txtCredit.Text = "0";
                    txtMatuirity.Text = "0";

                    Notify("Saved Successfully", "success", lblMsg);
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "New Party Saved Successfully.";
                }
            }
            else
            {
                Notify("Please Enter Name of the Party", "warn", lblMsg);
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
        int projectID = Convert.ToInt32(Session["ProjectID"].ToString());
        string lName = Page.User.Identity.Name.ToString();

            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO Party (Type, Company, ReferrenceItems, Zone, Referrer, Address, Phone, Email, Fax, IM, Website, ContactPerson, MobileNo, MatuirityDays, CreditLimit, InsuranceCode, OpBalance, ProjectID, EntryBy)" +
                                    "VALUES (@Type, @Company, '" + txtReferrenceItems.Text + "', @Zone, @Referrer, @Address, @Phone, @Email, @Fax, @IM, @Website, @ContactPerson, @MobileNo, @MatuirityDays, @CreditLimit, @InsuranceCode, @OpBalance, @ProjectID, @EntryBy)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@Type", formtype);
            cmd.Parameters.AddWithValue("@Company", txtCompany.Text);
            cmd.Parameters.AddWithValue("@Zone", ddZone.SelectedValue);
            cmd.Parameters.AddWithValue("@Referrer", ddReferrerID.SelectedValue);

            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
            cmd.Parameters.AddWithValue("@Phone", txtLandPhone.Text);
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
            cmd.Parameters.AddWithValue("@Fax", txtFax.Text);

            cmd.Parameters.AddWithValue("@IM", txtIM.Text);
            cmd.Parameters.AddWithValue("@Website", txtWebsite.Text);
            cmd.Parameters.AddWithValue("@ContactPerson", txtName.Text);
            cmd.Parameters.AddWithValue("@MobileNo", txtMobile.Text);
        //
        SQLQuery.Empty2Zero(txtMatuirity);
        SQLQuery.Empty2Zero(txtCredit);
        SQLQuery.Empty2Zero(txtInsuranceCode);
        SQLQuery.Empty2Zero(txtBalance);
        cmd.Parameters.AddWithValue("@MatuirityDays", Convert.ToInt32(txtMatuirity.Text));
            cmd.Parameters.AddWithValue("@CreditLimit", Convert.ToInt32(txtCredit.Text));
            cmd.Parameters.AddWithValue("@InsuranceCode", txtInsuranceCode.Text);
            cmd.Parameters.AddWithValue("@OpBalance", Convert.ToDecimal(txtBalance.Text));
            cmd.Parameters.AddWithValue("@ProjectID", projectID);
            cmd.Parameters.AddWithValue("@EntryBy", lName);

            cnn.Open();
            int Success = cmd.ExecuteNonQuery();
            cnn.Close();

        string pid = SQLQuery.ReturnString("Select MAX(PartyID) from party");
            SQLQuery.ExecNonQry("UPDATE Party SET VatRegNo='" + txtVatRegNo.Text + "', TDS_Terms ='" + txtTDSTerms.Text + "', AccHeadTDS ='" + ddAccHeadTDS.SelectedValue + "', AccHeadVDS ='" + ddAccHeadVDS.SelectedValue + "', VDS_Terms='" + txtVDSTerms.Text + "' WHERE PartyID=(SELECT MAX(PartyID) from Party where PartyID= '" + pid + "')");

        if (formtype == "customer")
        {
            string zoneId =SQLQuery.ReturnString("Select AreaID from Areas where AreaName='" + ddZone.SelectedValue + "'");
            SQLQuery.ExecNonQry("INSERT INTO DeliveryPoints (CustomerID, DeliveryPointName, ZoneID, Address, PhoneNo, Email, Contactperson, MobileNo, VehicleFare, projectID, EntryBy) VALUES ('"+ pid + "', '" + ddZone.SelectedItem.Text + "', '" + zoneId + "', '" + txtAddress.Text + "', '" + txtVatRegNo.Text + "', '" + txtEmail.Text + "', '" + txtName.Text + "', '" + txtMobile.Text + "', '0', '1', '" + lName + "')");
        }
    }
    protected void ddName_SelectedIndexChanged(object sender, EventArgs e)
    {
        EditMode();
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        ddName.SelectedValue = lblItemName.Text;

        EditMode();
    }

    private void EditMode()
    {
        try
        {
            SqlCommand cmd7 = new SqlCommand("SELECT Company, Zone, Referrer, Address, Phone, Email, Fax, IM, Website, ContactPerson, MobileNo, CreditLimit, OpBalance, MatuirityDays, ReferrenceItems, TDS_Terms, VDS_Terms, VatRegNo, AccHeadTDS, AccHeadVDS, InsuranceCode FROM [Party] WHERE PartyID=@PartyID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Parameters.Add("@PartyID", SqlDbType.VarChar).Value = ddName.SelectedValue;
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                btnSave.Text = "Update";

                txtCompany.Text = dr[0].ToString();
                string zone = dr[1].ToString();
                if(zone!="")
                {
                    ddZone.SelectedValue = zone;
                }
                ddReferrerID.SelectedValue = dr[2].ToString();
                txtAddress.Text = dr[3].ToString();
                txtLandPhone.Text = dr[4].ToString();
                txtEmail.Text = dr[5].ToString();
                txtFax.Text = dr[6].ToString();
                txtIM.Text = dr[7].ToString();

                txtWebsite.Text = dr[8].ToString();
                txtName.Text = dr[9].ToString();
                txtMobile.Text = dr[10].ToString();
                txtCredit.Text = dr[11].ToString();
                txtBalance.Text = dr[12].ToString();
                txtMatuirity.Text = dr["MatuirityDays"].ToString();
                txtReferrenceItems.Text = dr[14].ToString();

                txtTDSTerms.Text = dr[15].ToString();
                txtVDSTerms.Text = dr[16].ToString();
                txtVatRegNo.Text = dr[17].ToString();
                //bool isEx=  SQLQuery.ReturnString("SELECT AccountsHeadID FROM WHERE(AccountsHeadID = '" + dr["AccHeadTDS"].ToString() + "')")
                //if ()
                //{
                //    ddAccHeadTDS.Enabled = false;
                //}

                txtInsuranceCode.Text = dr["InsuranceCode"].ToString();
                ddAccHeadTDS.SelectedValue = dr["AccHeadTDS"].ToString();
                ddAccHeadVDS.SelectedValue = dr["AccHeadVDS"].ToString();
                
                btnDelete.Visible = true;
                ltrSubFrmName.Text = "Edit Party";
                Notify("Info loaded in edit mode.", "info", lblMsg);
                //EditField.Attributes.Remove("class");
                //EditField.Attributes.Add("class", "control-group"); SQLQuery.ReturnString(""

                GridView1.DataBind();
            }
            cmd7.Connection.Close();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }


    private void UpdateInfo()
    {
        string lName = Page.User.Identity.Name.ToString();
        //get old data first
        string oldCompany = "", oldZone = "", oldBal = "", oldDueLimit = "", oldRef = "";
        SqlCommand cmd7 = new SqlCommand("SELECT Company, Zone, Referrer, Address, Phone, Email, Fax, IM, Website, ContactPerson, MobileNo, CreditLimit, OpBalance, InsuranceCode FROM [Party] WHERE PartyID=@PartyID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Parameters.Add("@PartyID", SqlDbType.VarChar).Value = ddName.SelectedValue;
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


        //Create Sql Connection
        SqlConnection cnn = new SqlConnection();
        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        //Create Sql Command
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE Party SET Company =@Company, ReferrenceItems='" + txtReferrenceItems.Text + "', Zone =@Zone, Referrer =@Referrer, Address =@Address, Phone =@Phone, Email =@Email, Fax =@Fax, IM =@IM, Website =@Website, ContactPerson =@ContactPerson, MobileNo =@MobileNo, CreditLimit =@CreditLimit, InsuranceCode=@InsuranceCode, OpBalance =@OpBalance, MatuirityDays =@MatuirityDays, LastUpdateDate=@LastUpdateDate WHERE PartyID='" + ddName.SelectedValue + "'";
        cmd.CommandType = CommandType.Text;
        cmd.Connection = cnn;

        cmd.Parameters.AddWithValue("@Company", txtCompany.Text);
        cmd.Parameters.AddWithValue("@Zone", ddZone.SelectedValue);
        cmd.Parameters.AddWithValue("@Referrer", ddReferrerID.SelectedValue);

        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
        cmd.Parameters.AddWithValue("@Phone", txtLandPhone.Text);
        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
        cmd.Parameters.AddWithValue("@Fax", txtFax.Text);

        cmd.Parameters.AddWithValue("@IM", txtIM.Text);
        cmd.Parameters.AddWithValue("@Website", txtWebsite.Text);
        cmd.Parameters.AddWithValue("@ContactPerson", txtName.Text);
        cmd.Parameters.AddWithValue("@MobileNo", txtMobile.Text);

        cmd.Parameters.AddWithValue("@MatuirityDays", Convert.ToInt32(txtMatuirity.Text));
        cmd.Parameters.AddWithValue("@CreditLimit", Convert.ToInt32(txtCredit.Text));
        cmd.Parameters.AddWithValue("@InsuranceCode", txtInsuranceCode.Text);
        cmd.Parameters.AddWithValue("@OpBalance", Convert.ToDecimal(txtBalance.Text));
        cmd.Parameters.AddWithValue("@LastUpdateDate", DateTime.Now);

        cnn.Open();
        int Success = cmd.ExecuteNonQuery();
        cnn.Close();

        RunQuery.SQLQuery.ExecNonQry("UPDATE Party SET VatRegNo='" + txtVatRegNo.Text + "', TDS_Terms ='" + txtTDSTerms.Text + "', AccHeadTDS ='" + ddAccHeadTDS.SelectedValue + "', AccHeadVDS ='" + ddAccHeadVDS.SelectedValue + "', VDS_Terms='" + txtVDSTerms.Text + "' WHERE PartyID='" + ddName.SelectedValue + "'");

        //Update History
        SqlCommand cmd2 = new SqlCommand("INSERT INTO PartyUpdateHistory (PartyID, OldCompanyName, NewCompanyName, OldZone, NewZone, OldReferrer, NewReferrer, OldCreditLimit, NewCreditLimit, OldOpBalance, NewOpBalance, UpdateBy)" +
                                    "VALUES ('" + ddName.SelectedValue + "', '" + oldCompany + "', '" + txtCompany.Text + "', '" + oldZone + "', '" + ddZone.SelectedValue + "', '" + oldRef + "', '" + ddReferrerID.SelectedItem.Text + "', '" + oldDueLimit + "', '" + txtCredit.Text + "', '" + oldBal + "', '" + txtBalance.Text + "', '" + lName + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        string formtype = base.Request.QueryString["type"];
        if (formtype == "customer")
        {
            string zoneId = SQLQuery.ReturnString("Select AreaID from Areas where AreaName='" + ddZone.SelectedValue + "'");
            SQLQuery.ExecNonQry("UPDATE DeliveryPoints set PhoneNo='" + txtVatRegNo.Text + "' where CustomerID='" + ddName.SelectedValue + "' AND ZoneID='" + zoneId + "'");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        RefreshForm();
        Notify("Action Cancelled!", "warn", lblMsg);
    }

    private void RefreshForm()
    {
        ClearControls(Form);
        GridView1.DataBind();
        ddName.DataBind();
        GridView1.EditIndex = -1;
        EditField.Attributes.Remove("class");
        EditField.Attributes.Add("class", "control-group hidden");
        btnSave.Text = "Save Info";

        btnDelete.Visible = false;
        LoadFormInfo();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            SqlCommand cmd =new SqlCommand("SELECT HeadName FROM Transactions WHERE  TrType='Customer' AND HeadID ='" + ddName.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            string isFound = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            //SqlCommand cmdc = new SqlCommand("SELECT HeadName FROM Transactions WHERE  TransactionGroup='Party' AND HeadID ='" + ddName.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            //cmdc.Connection.Open();
            //string isFound2 = Convert.ToString(cmdc.ExecuteScalar());
            //cmdc.Connection.Close();

            if (isFound == "")
            {
                string lName = Page.User.Identity.Name.ToString();
                //get old data first
                string oldCompany = "", oldZone = "", oldBal = "", oldDueLimit = "", oldRef = "";
                SqlCommand cmd7 =new SqlCommand(
                        "SELECT Company, Zone, Referrer, Address, Phone, Email, Fax, IM, Website, ContactPerson, MobileNo, CreditLimit, OpBalance FROM [Party] WHERE PartyID=@PartyID",
                        new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Parameters.Add("@PartyID", SqlDbType.VarChar).Value = ddName.SelectedValue;
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

                SqlCommand cmd3 = new SqlCommand("Delete Party where (PartyID= '" + ddName.SelectedValue + "')",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd3.Connection.Open();
                cmd3.ExecuteNonQuery();
                cmd3.Connection.Close();

                //Delete History
                SqlCommand cmd2 =new SqlCommand(
                        "INSERT INTO PartyUpdateHistory (PartyID, OldCompanyName, NewCompanyName, OldZone, NewZone, OldReferrer, NewReferrer, OldCreditLimit, NewCreditLimit, OldOpBalance, NewOpBalance, UpdateBy)" +
                        "VALUES ('" + ddName.SelectedValue + "', '" + oldCompany + "', 'Deleted', '" + oldZone +
                        "', 'Deleted', '" + oldRef + "', 'Deleted', '" + oldDueLimit + "', '0', '" + oldBal +
                        "', '0', '" + lName + "')",
                        new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();

                RefreshForm();
                Notify("Party Info Deleted!", "warn", lblMsg);
            }
            else
            {
                Notify("This party already has previous transactions! Party can not be deleted...", "error", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }
}



