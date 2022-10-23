using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Operator_Employee_Close : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            populateActiveID();
        }
    }

    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    //Loading Active Employee ID in Combobox
    private void populateActiveID()
    {
        SqlCommand cmd = new SqlCommand("Select * from [EmployeeInfo] where isActive=1 order by EmpSerial", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();

        SqlDataReader EmpIdlist = cmd.ExecuteReader();

        ddEmpId.DataSource = EmpIdlist;
        ddEmpId.DataValueField = "EmpSerial";
        ddEmpId.DataTextField = "EmpSerial";
        ddEmpId.DataBind();

        cmd.Connection.Close();
        cmd.Connection.Dispose();
        PopulateEmpDetails();
    }
    //Loading Closed Employee ID in Combobox
    private void populateClosedID()
    {
        SqlCommand cmd = new SqlCommand("Select * from [EmployeeInfo] where isActive=0", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();

        SqlDataReader EmpIdlist = cmd.ExecuteReader();

        ddEmpId.DataSource = EmpIdlist;
        ddEmpId.DataValueField = "EmpSerial";
        ddEmpId.DataTextField = "EmpSerial";
        ddEmpId.DataBind();

        cmd.Connection.Close();
        cmd.Connection.Dispose();
    }

    //Loading Employee Details
    public void PopulateEmpDetails()
    {
        SqlCommand cmd = new SqlCommand("SELECT EName, Salary, EmpSerial FROM EmployeeInfo WHERE (EmpSerial = @EID)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.Parameters.Add("@EID", SqlDbType.Int).Value = ddEmpId.SelectedValue;

        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            txtName.Text = dr[0].ToString();
            txtSalary.Text = dr[1].ToString();
            txtCardSl.Text = dr[2].ToString();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
    }
    protected void ddEmpId_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateEmpDetails();
    }
    protected void ddStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddStatus.SelectedValue == "Re-joined")
        {
            ddEmpId.Items.Clear();
            populateClosedID();
        }
        else
        {
            ddEmpId.Items.Clear();
            populateActiveID();
        }
    }

    //Clearing text boxes
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox)
        { (Parent as TextBox).Text = string.Empty; }
        else
        {
            foreach (Control c in Parent.Controls)
                ClearControls(c);
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Update EmployeeInfo set Salary=@Salary, EmpSerial= @CardSerial, isActive=@Closed where EmpSerial=@EID";
            //cmd.Parameters.Add("@EID", SqlDbType.Int).Value = ddStatus.SelectedValue;
            cmd.Connection = cnn;

            //Parameter Declaration
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@EID", SqlDbType.Int);
            param[1] = new SqlParameter("@Salary", SqlDbType.Decimal);
            param[2] = new SqlParameter("@CardSerial", SqlDbType.VarChar);
            param[3] = new SqlParameter("@Closed", SqlDbType.Int);

            param[0].Value = ddEmpId.SelectedValue;
            param[1].Value = txtSalary.Text;
            param[2].Value = txtCardSl.Text;
            //int status = Convert.ToInt32(txtCardSl.Text);
            int status;

            if (ddStatus.SelectedValue == "Re-joined")
            {
                status = 1;
            }
            else
            {
                status = 0;
            }
            param[3].Value = status;

            for (int i = 0; i < param.Length; i++)
            {
                cmd.Parameters.Add(param[i]);
            }

            cnn.Open();
            int success = cmd.ExecuteNonQuery();
            cnn.Close();

            if (success > 0)
            {
                lblMsg.Text = "Successfuly Changed the employee status";
                ClearControls(Page);
                ddEmpId.Items.Clear();
                populateActiveID();
            }
            else
            {
                lblMsg.Text = "An error has occured";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Invalid New ID: The ID you are trying is already exist";// +ex;
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand("Delete from [EmployeeInfo] where EmpSerial='" + ddEmpId.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();
        ddEmpId.Items.Clear();
        populateActiveID();
        MessageBox("Employee Deleted Successfully.");

    }

}
