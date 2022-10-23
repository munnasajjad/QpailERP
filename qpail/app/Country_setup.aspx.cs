using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class app_Country_setup : System.Web.UI.Page
{
    public string GetConnection()
    {
        //return the value of Connection String
        return System.Configuration.ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
    }

    private void ExecuteInsert(String Designation)
    {
        // Connection Varioable cnn
        SqlConnection cnn = new SqlConnection(GetConnection());

        string sql = "INSERT INTO COUNTRY (COUNTRY) VALUES"
        + "(@COUNTRY)";

        try
        {
            //openning Connection
            cnn.Open();
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@COUNTRY", SqlDbType.VarChar, 50);

            // matching columns by inserting column heads
            param[0].Value = Designation;

            //Command-parameter relationship
            for (int i = 0; i < param.Length; i++)
            {
                cmd.Parameters.Add(param[i]);
            }

            //Command Execution
            cmd.CommandType = CommandType.Text;
            //cmd.ExecuteNonQuery();
            cmd.ExecuteScalar();
        }

            //Exception Handling.
        catch (System.Data.SqlClient.SqlException ex)
        {
            string msg = "Error in Save: ";
            msg += ex.Message;
            throw new Exception(msg);
        }

        // Connection Close
        finally
        {
            cnn.Close();
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (txtDept.Text != "")
        {
            ExecuteInsert(txtDept.Text);
            lblMsg.Text = "New Department Added Successfully";
            if (txtDept.Text != "")
            {
                txtDept.Text = "";
            }
            GridView1.DataBind();
        }
        else
        {
            lblMsg.Text = "Please write a Department Name";
        }

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        //ClearControls(Page);
        Response.Redirect("Dashboard.aspx");
    }
}
