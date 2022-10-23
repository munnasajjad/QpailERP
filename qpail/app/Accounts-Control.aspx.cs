using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Operator_Accounts_Control : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            populateSub();
            popSubID();
        }
    }

    private void populateSub()
    {
        SqlCommand cmd = new SqlCommand("Select AccountsID, AccountsName from Accounts", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader Datalist = cmd.ExecuteReader();

        ddSub.DataSource = Datalist;
        ddSub.DataValueField = "AccountsName";
        ddSub.DataTextField = "AccountsName";
        ddSub.DataBind();

        cmd.Connection.Close();
        cmd.Connection.Dispose();
    }

    private void popSubID()
    {
        SqlCommand cmd = new SqlCommand("Select AccountsID, AccountsName from Accounts where AccountsName= @Acc", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.Parameters.Add("@Acc", SqlDbType.VarChar).Value = ddSub.SelectedValue;

        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            SubID.Text = dr[0].ToString();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtControl.Text != "")
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

            SqlCommand cmd = new SqlCommand("Select max(convert(int, right(AccountsID, 4)) + 1) from Accounts where AccountsID=@AccID", cnn);
            cmd.Parameters.Add("@AccID", SqlDbType.VarChar).Value = SubID.Text;

            cnn.Open();
            Int32 maxID = (Int32)cmd.ExecuteScalar();
            string accID = SubID.Text;
            string subGroupID = accID + "0" + maxID;

            cmd.CommandText = "Insert into ControlAccount (ControlAccountsID, ControlAccountsName, AccountsID)" +
                                            "VALUES (@AID, @SubGroup, @AccGroupID)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@AID", SqlDbType.VarChar, 6);
            param[1] = new SqlParameter("@SubGroup", SqlDbType.VarChar, 80);
            param[2] = new SqlParameter("@AccGroupID", SqlDbType.VarChar, 4);

            param[0].Value = subGroupID;
            param[1].Value = txtControl.Text;
            param[2].Value = accID;
            //Looping the variables
            for (int i = 0; i < param.Length; i++)
            {
                cmd.Parameters.Add(param[i]);
            }


            int success = cmd.ExecuteNonQuery();

            if (success > 0)
            {
                txtControl.Text = "";
                lblMsg.Text = "Successfully Added new Sub-Accounts";
            }
            else
            {
                lblMsg.Text = "Failed to save new data";
            }
            cnn.Close();

        }
        else { lblMsg.Text = "Please Type the Accounts Name Correctly"; }

    }

    protected void ddSub_SelectedIndexChanged(object sender, EventArgs e)
    {
        popSubID();
    }

}
