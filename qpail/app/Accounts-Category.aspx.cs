using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class app_Accounts_Category : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            populateGroup();
            popGID();
        }
    }

    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        //if (Parent is TextBox)
        //{ (Parent as TextBox).Text = string.Empty; }
        //else
        //{
        //    foreach (Control c in Parent.Controls)
        //        ClearControls(c);
        //}
    }



    private void populateGroup()
    {
        SqlCommand cmd = new SqlCommand("Select GroupID, GroupName from AccountGroup", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader Grouplist = cmd.ExecuteReader();

        ddGroup.DataSource = Grouplist;
        ddGroup.DataValueField = "GroupName";
        ddGroup.DataTextField = "GroupName";
        ddGroup.DataBind();

        cmd.Connection.Close();
        cmd.Connection.Dispose();
    }

    private void popGID()
    {
        SqlCommand cmd = new SqlCommand("Select GroupID, GroupName from AccountGroup where GroupName= @Group", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.Parameters.Add("@Group", SqlDbType.VarChar).Value = ddGroup.SelectedValue;

        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            lblGID.Text = dr[0].ToString();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtSubAcc.Text != "")
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

            SqlCommand cmd = new SqlCommand("Select max(convert(int, right(AccountsID, 2)) + 1) from Accounts where GroupID=@GroupID", cnn);
            cmd.Parameters.Add("@GroupID", SqlDbType.VarChar, 2).Value = lblGID.Text;

            cnn.Open();
            Int32 maxID = (Int32)cmd.ExecuteScalar();
            string groupID = lblGID.Text;
            string subGroupID = groupID + "0" + maxID;

            cmd.CommandText = "Insert into Accounts (AccountsID, AccountsName, GroupID)" +
                                            "VALUES (@AID, @SubGroup, @AccGroupID)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@AID", SqlDbType.VarChar, 4);
            param[1] = new SqlParameter("@SubGroup", SqlDbType.VarChar, 80);
            param[2] = new SqlParameter("@AccGroupID", SqlDbType.VarChar, 2);

            param[0].Value = subGroupID;
            param[1].Value = txtSubAcc.Text;
            param[2].Value = groupID;
            //Looping the variables
            for (int i = 0; i < param.Length; i++)
            {
                cmd.Parameters.Add(param[i]);
            }


            int success = cmd.ExecuteNonQuery();

            if (success > 0)
            {
                txtSubAcc.Text = "";
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

    protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        popGID();
    }
}