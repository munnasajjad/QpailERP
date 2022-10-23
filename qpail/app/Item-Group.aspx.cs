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

public partial class Operator_Item_Group : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //txtDept.Text = "Drinks3";
        //if (!IsPostBack)
        //{
        //    populateControl();
        //}
        divCcontroller.Visible = ddDepreciation.SelectedValue == "2";
        divCcontroller2.Visible = ddDepreciation.SelectedValue == "2";
        
       

    }


    //private void populateControl()
    //{
    //    SqlCommand cmd = new SqlCommand("Select ControlAccountsID, ControlAccountsName from ControlAccount", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
    //    //cmd.Parameters.Add("@AccID", SqlDbType.VarChar).Value = ddSub.SelectedValue;
    //    cmd.Connection.Open();
    //    SqlDataReader Datalist = cmd.ExecuteReader();

    //    ddControl.DataSource = Datalist;
    //    ddControl.DataValueField = "ControlAccountsID";
    //    ddControl.DataTextField = "ControlAccountsName";
    //    ddControl.DataBind();

    //    cmd.Connection.Close();
    //    cmd.Connection.Dispose();
    //}

    private void ExecuteInsert()
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            int prjId = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Connection.Close();

            string itemExist = SQLQuery.ReturnString("SELECT GroupName FROM ItemGroup WHERE GroupName ='" + txtDept.Text + "'");

            if (itemExist != txtDept.Text)
            {
                SqlCommand cmd2 = new SqlCommand("INSERT INTO ItemGroup (GroupName,ControlAccountsID, Description, ProjectID, EntryBy,DepreciationType,Depreciationvalue) VALUES (@GroupName,@ControlAccountsID, @Description, @ProjectID, @EntryBy,@DepreciationType,@Depreciationvalue)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2.Parameters.AddWithValue("@GroupName", txtDept.Text);
                cmd2.Parameters.AddWithValue("@ControlAccountsID", ddControl.SelectedValue);
                cmd2.Parameters.AddWithValue("@Description", txtDesc.Text);
                cmd2.Parameters.AddWithValue("@ProjectID", prjId);
                cmd2.Parameters.AddWithValue("@EntryBy", lName);
                cmd2.Parameters.AddWithValue("@DepreciationType", ddDepreciation.SelectedValue);
                cmd2.Parameters.AddWithValue("@Depreciationvalue", txtDepreciationvalue.Text);
                
                cmd2.Connection.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Connection.Close();
                lblMsg.Text = "New Item Group Added Successfully";
                lblMsg.Attributes.Add("class", "xerp_success");
            }
            else
            {
                lblMsg.Text = "ERROR: Info already exist!";
                lblMsg.Attributes.Add("class", "xerp_error");
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "ERROR: " + ex.ToString();
            lblMsg.Attributes.Add("class", "xerp_error");
        }
        finally
        {
            GridView1.DataBind();
            txtDept.Text = "";
            txtDesc.Text = "";
        }

    }

   


    protected void btnSave_Click1(object sender, EventArgs e)
    {
        if (txtDept.Text != "")
        {

            if (btnSave.Text == "Save")
            {
                ExecuteInsert();
            }
            else if (btnSave.Text == "Update")
            {
                ExecuteUpdate();
                GridView1.DataBind();
            }
        }
        else
        {
            lblMsg.Text = "Please type Item Group Name";
        }
    }
    protected void btnClear_Click1(object sender, EventArgs e)
    {
        txtDept.Text = "";
        txtDesc.Text = "";
    }

    //Messagebox For Alerts    
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }







    //____________________________________________________________________________________________________________

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    private void ExecuteUpdate()
    {
        SqlCommand cmd2 = new SqlCommand("UPDATE ItemGroup SET GroupName='" + txtDept.Text + "', ControlAccountsID='" + ddControl.SelectedValue + "', Description='" + txtDesc.Text + "' , DepreciationType='"+ddDepreciation.SelectedValue+ "', Depreciationvalue='"+txtDepreciationvalue.Text+"' WHERE GroupSrNo='" + GroupIdHField.Value + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2.Connection.Open();
       int rowAffected= cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();
        cmd2.Connection.Dispose();
    }

    private void EditMode()
    {
        SqlCommand cmd7 = new SqlCommand("SELECT [GroupName], [ControlAccountsID], [Description],[DepreciationType],[Depreciationvalue] FROM [ItemGroup] WHERE  GroupSrNo='" + GroupIdHField.Value + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            //EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";

            txtDept.Text = dr["GroupName"].ToString();
            ddControl.DataBind();
            ddControl.SelectedValue = dr["ControlAccountsID"].ToString();
            ddDepreciation.DataBind();
            ddDepreciation.SelectedValue = dr["DepreciationType"].ToString();
            if (ddDepreciation.SelectedValue == "2")
            {
                divCcontroller.Visible = true;
                divCcontroller2.Visible = true;
            }
            txtDesc.Text = dr["Description"].ToString();
            txtDepreciationvalue.Text = dr["Depreciationvalue"].ToString();

        }
        cmd7.Connection.Close();
    }


    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label lblGroupId = GridView1.Rows[index].FindControl("Label1") as Label;
        GroupIdHField.Value = lblGroupId.Text;
        EditMode();
        Notify("Edit mode activated ...", "info", lblMsg);
    }
    //new update

    protected void ddDepreciation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        divCcontroller.Visible = ddDepreciation.SelectedValue== "2";
        divCcontroller2.Visible = ddDepreciation.SelectedValue == "2";

    }
}