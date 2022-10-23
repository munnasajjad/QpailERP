using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Oxford.app
{
    public partial class Exp_Heads : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        //Code for Clearing the form
        //public static void ClearControls(Control Parent)
        //{
        //    if (Parent is TextBox)
        //    { (Parent as TextBox).Text = string.Empty; }
        //    else
        //    {
        //        foreach (Control c in Parent.Controls)
        //            ClearControls(c);
        //    }
        //}


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text != "")
                {
                    if (btnSave.Text == "Save")
                    {
                        if (txtAmount.Text == "")
                        {
                            txtAmount.Text = "0";
                        }

                        SqlConnection cnn = new SqlConnection();
                        cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "Insert into ExpenseHeads (GroupID, Name, Description, Amount, Entryby) Values (@GroupID, @Name, @Description, @Amount, @Entryby)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = cnn;

                        cmd.Parameters.AddWithValue("@GroupID", ddGroup.SelectedValue);
                        cmd.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                        cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                        cmd.Parameters.AddWithValue("@EntryBy", Page.User.Identity.Name.ToString());

                        cnn.Open();
                        int Success = cmd.ExecuteNonQuery();
                        cnn.Close();
                        if (Success > 0)
                        {
                            lblMsg.Text = "Successfully Saved!";
                            lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff6230");
                            ClearControls();
                        }

                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE ExpenseHeads SET GroupID=@GroupID, Name=@Name, Description=@Description, Amount=@Amount where (sl ='" + DropDownList1.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                        cmd.Parameters.AddWithValue("@GroupID", ddGroup.SelectedValue);
                        cmd.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                        cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                        cmd.Connection.Dispose();


                        ClearControls();

                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Updated Successfully";
                    }
                }
                else
                {
                    //MessageBox("Empty data is not accepted.");
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Unable to Save: " + ex.ToString();
            }
            finally
            {
                GridView1.DataBind();
            }
        }

        private void ClearControls()
        {
            btnSave.Text = "Save";
            EditField.Attributes.Add("class", "form-group hidden");
            GridView1.DataBind();
            txtAmount.Text = "";
            txtDescription.Text = "";
            txtName.Text = "";
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

                lblMsg.Attributes.Add("class", "xerp_info");
                lblMsg.Text = "Edit mode activated ...";
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.ToString();
            }
        }

        private void EditMode()
        {
            SqlCommand cmd7 = new SqlCommand("SELECT GroupID, Name, Description, Amount FROM [ExpenseHeads] WHERE sl='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                EditField.Attributes.Remove("class");
                EditField.Attributes.Add("class", "form-group");
                btnSave.Text = "Update";

                ddGroup.SelectedValue = dr[0].ToString();
                txtName.Text = dr[1].ToString();
                txtDescription.Text = dr[2].ToString();
                txtAmount.Text = dr[3].ToString();
            }
            cmd7.Connection.Close();
        }

        protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        protected void btnClear_OnClick(object sender, EventArgs e)
        {
            ClearControls();
            lblMsg.Attributes.Remove("class");
            lblMsg.Attributes.Add("class", "xerp_info");
            lblMsg.Text = "Action Cancelled!";
        }
    }
}