using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Oxford.PathshalaTableAdapters;

namespace Oxford.app
{
    public partial class SMS_Template : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        //Message & Notify For Alerts
        private void Notify(string msg, string type)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
            //Types: success, info, warn, error
            lblMsg.Attributes.Add("class", "xerp_" + type);
            lblMsg.Text = msg;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string isExist = RunQuery.SQLQuery.ReturnString("Select name from SMSTemplate where name='" + txtTitle.Text + "'");
                if (txtTitle.Text != "" && isExist == "")
                {
                    if (btnSave.Text == "Save")
                    {
                        SqlConnection cnn = new SqlConnection();
                        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

                        //string lName = txtTeacherID.Text;
                        //lName = lName.Trim();
                        //Create Sql Command
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "Insert into SMSTemplate (name, SmsBody) Values (@name, @Class)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = cnn;

                        cmd.Parameters.AddWithValue("@Name", txtTitle.Text);
                        cmd.Parameters.AddWithValue("@Class", txtText.Text);

                        cnn.Open();
                        int Success = cmd.ExecuteNonQuery();
                        cnn.Close();

                        txtTitle.Text = "";
                        txtText.Text = "";

                        if (Success > 0)
                        {
                            Notify("New Template Saved Successfully.", "success");
                        }
                    }
                    else
                    {
                        SqlCommand cmd2 = new SqlCommand("UPDATE SMSTemplate SET Name=@Name, SmsBody=@Class where (sl ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                        cmd2.Parameters.AddWithValue("@Name", txtTitle.Text);
                        cmd2.Parameters.AddWithValue("@Class", txtText.Text);

                        cmd2.Connection.Open();
                        cmd2.ExecuteNonQuery();
                        cmd2.Connection.Close();
                        cmd2.Connection.Dispose();

                        btnSave.Text = "Save";
                        EditField.Attributes.Add("class", "form-group hidden");

                        txtTitle.Text = "";
                        txtText.Text = "";

                        Notify("Updated Successfully", "success");
                    }

                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_error");
                    Notify("ERROR: Data already exist!", "error");
                }
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Unable to Save: " + ex.ToString();
            }
            finally
            {

                GridView1.DataBind();
            }
        }



        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditMode();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList1.DataBind();
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label PID = GridView1.Rows[index].FindControl("Label1") as Label;
                DropDownList1.SelectedValue = PID.Text;
                EditMode();

                Notify("Edit mode activated ...", "info");

            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.ToString();
            }
        }

        private void EditMode()
        {
            SqlCommand cmd7 = new SqlCommand("SELECT sl, name, SmsBody FROM SMSTemplate WHERE sl='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                EditField.Attributes.Remove("class");
                EditField.Attributes.Add("class", "form-group");
                btnSave.Text = "Update";

                //txtDept.Text = dr[0].ToString();
                //txtDesc.Text = dr[1].ToString();
                //ddSubGrp.SelectedValue = dr[2].ToString();


                txtTitle.Text = dr[1].ToString();
                txtText.Text = dr[2].ToString();
            }
            cmd7.Connection.Close();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label lblsl = (Label)GridView1.Rows[e.RowIndex].FindControl("Label1");
            string sl = lblsl.Text, msg = "";

            RunQuery.SQLQuery.ExecNonQry("DELETE FROM SMSTemplate WHERE (sl = '" + sl + "')");
            GridView1.DataBind();
            
            Notify("Template deleted successfully.", "warn");
        }
    }
}