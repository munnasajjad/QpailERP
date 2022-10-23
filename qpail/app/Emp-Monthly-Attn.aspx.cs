using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class app_Emp_Monthly_Attn : System.Web.UI.Page
{
    private decimal otHour;

    protected void Page_Load(object sender, EventArgs e)
    {
        //////////////////////////////////////
        ////// Loading Attendence
        //////////////////////////////////////
        /*-------- Transferred to GUI Edit
         * ------- Also Attendence_RowDataBound Event
        //Create Sql Connection
        SqlConnection cnn = new SqlConnection();
        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        //Create Sql Command
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select EID, EName, InTime, OutTime from EmployeeInfo inner join Cdata on EmployeeInfo.CardSerial= Cdata.EmployeeID";
        cmd.CommandType = CommandType.Text;
        cmd.Connection = cnn;

        //Inserting Employee & Cdata into Datagrid View 
        try
        {
            cnn.Open();
            Attendence.DataSource = cmd.ExecuteReader();
            Attendence.DataBind();

            cnn.Close();
            cnn.Dispose();
        }

        catch (Exception ex)
        {
            lblerror.Text = ex.Message + "Unable to read data from the table dbo.Cdata.";
        }
        */
        //txtDate.Text = DateTime.Now.AddDays(-1).ToShortDateString(); //DateTime.Today.Date.ToShortDateString().ToString();
        txtDate.Text = DateTime.Now.ToShortDateString(); //DateTime.Today.Date.ToShortDateString().ToString();

    }



    public void Attendence_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        //    if (!tablecopied)
        {
            //        OriginalDataTable = ((DataRowView)e.Row.DataItem).Row.Table.Copy();
            //        ViewState["OriginalValuesDataTable"] = OriginalDataTable;
            //        tablecopied = true;


            //foreach (GridViewRow row in Attendence.Rows)
            //{
            //Label lblOutTime = (Label)e.Row.FindControl("TextBox2");
            Label lblOT = (Label)e.Row.FindControl("lblOT");

            string outTime = ((TextBox)e.Row.FindControl("TextBox2")).Text;
            otHour = Convert.ToDateTime(outTime).TimeOfDay.Hours - Convert.ToDateTime(txtOut.Text).TimeOfDay.Hours;

            //TimeSpan Diff = Convert.ToDateTime(lblOutTime.Text)TimeOfDay-Convert.ToDateTime(txtOut.Text))TimeOfDay;
            lblOT.Text = otHour.ToString();
            //}

        }
    }


    //////////////////////////////////////
    ////// Saving Attendence and overtime
    //////////////////////////////////////

    protected void btnSave_Click(object sender, EventArgs e)
    {

        try
        {
            //Create Sql Connection
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

            //Create Sql Command
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO AttendenceFinal (AttDate, OfficeInTime, OfficeOutTime, EID, EName, InTime, OutTime, OTHours)" +
                                                         " VALUES(@Date, @OffIn, @OffOut, @EID, @Ename, @In, @Out, @OT)";
            // string sql = "INSERT INTO AttendenceFinal (Date, OfficeInTime, OfficeOutTime, EID, EName, InTime, OutTime, OTHours)" +
            //                                             " VALUES(@Date, @OffIn, @OffOut, @EID, @Ename, @In, @Out, @OT)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cnn;

            //Parameters Declaration
            SqlParameter[] prm = new SqlParameter[8];

            prm[0] = new SqlParameter("@Date", SqlDbType.Date);
            prm[1] = new SqlParameter("@OffIn", SqlDbType.Time);
            prm[2] = new SqlParameter("@OffOut", SqlDbType.Time);
            prm[3] = new SqlParameter("@EID", SqlDbType.Int, 100);
            prm[4] = new SqlParameter("@Ename", SqlDbType.VarChar, 100);
            prm[5] = new SqlParameter("@in", SqlDbType.Time);
            prm[6] = new SqlParameter("@Out", SqlDbType.Time);
            prm[7] = new SqlParameter("@OT", SqlDbType.Decimal);

            // Variables Declaration


            foreach (GridViewRow row in Attendence.Rows)
            {
                try
                {
                    //Converting OT Data
                    Label lblOT = (Label)row.FindControl("lblOT");
                    string outTime = ((TextBox)row.FindControl("TextBox2")).Text;
                    otHour = Convert.ToDateTime(outTime).TimeOfDay.Hours - Convert.ToDateTime(txtOut.Text).TimeOfDay.Hours;
                    lblOT.Text = otHour.ToString();


                    string aDate = txtDate.Text.ToString();
                    string offIn = txtIn.Text.ToString();
                    string offOut = txtOut.Text.ToString();
                    string eId = ((Label)row.FindControl("lblID")).Text;
                    string eName = ((Label)row.FindControl("lblName")).Text;
                    string inTime = ((TextBox)row.FindControl("TextBox1")).Text;
                    //string outTime = ((TextBox)row.FindControl("TextBox2")).Text;
                    string OT = lblOT.Text;

                    prm[0].Value = aDate;
                    prm[1].Value = offIn;
                    prm[2].Value = offOut;
                    prm[3].Value = eId;
                    prm[4].Value = eName;
                    prm[5].Value = inTime;
                    prm[6].Value = outTime;
                    prm[7].Value = OT;

                    /*** Looping Fields ***/
                    //for (int i = 0; i <= Attendence.Rows.Count-1; i++)
                    for (int i = 0; i < prm.Length; i++)
                    {
                        cmd.Parameters.Add(prm[i]);
                    }

                    cnn.Open();
                    int success = cmd.ExecuteNonQuery();
                    cnn.Close();

                    if (success > 0)
                    {
                        Label4.Text = "Successfully Updated Data";
                        cmd.Parameters.Clear();
                    }
                }
                finally
                {

                }
            }

        }
        finally
        {
            if (Label4.ToString().Length > 2)
            {
                SqlCommand cmd2 = new SqlCommand("DROP TABLE [dbo].[Cdata]", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd2.Connection.Open();

                cmd2.ExecuteReader();

                cmd2.Connection.Close();
                cmd2.Connection.Dispose();
            }
            else
            {
                Label4.Text = "Please Upload employee data to get the attendence.";
            }
        }

    }


}
