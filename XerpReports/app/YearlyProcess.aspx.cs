using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security;
using System.IO;

namespace Oxford.app
{
    public partial class YearlyProcess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddClass.DataBind();
                ddSession.DataBind();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(txtSession.Text) >= DateTime.Now.Year && Convert.ToInt32(txtSession.Text) < DateTime.Now.AddYears(2).Year)
                {
                    string isExist = RunQuery.SQLQuery.ReturnString("SELECT top(1) sl FROM Students  WHERE  Class='" + ddNewClass.SelectedValue + "' AND Session='" + txtSession.Text + "' ");

                    if (isExist == "")
                    {

                        string clearData = "";
                        if (txtSession.Text != "")
                        {
                            if (CheckBox1.Checked)
                            {
                                clearData = "Discount ";
                            }
                            if (CheckBox2.Checked)
                            {
                                clearData = clearData + "Finance ";
                            }

                            RunQuery.SQLQuery.ExecNonQry("UPDATE Students SET Class='" + ddNewClass.SelectedValue + "', Session='" + txtSession.Text + "' WHERE  Class='" + ddClass.SelectedValue + "' AND Session='" + ddSession.SelectedValue + "' ");

                            RunQuery.SQLQuery.ExecNonQry(
                                "Insert into YearProcess (OldClass, OldSession, NewClass, NewSession, ClearData, EntryBy) Values ('" + ddClass.SelectedItem.Text + "','" + ddSession.SelectedValue + "','" + ddNewClass.SelectedItem.Text + "','" + txtSession.Text + "','" + clearData + "','" + Page.User.Identity.Name.ToString() + "')");


                            txtSession.Text = "";
                            GridView1.DataBind();
                            Notify("Successfully Saved...", "success", lblMsg);
                        }
                        else
                        {
                            Notify("Please type session...", "error", lblMsg);
                        }
                    }
                    else
                    {
                        Notify("ERROR: Same Data Already Exist!", "error", lblMsg);
                    }
                }
                else
                {
                    Notify("ERROR: Invalid Session Year!", "error", lblMsg);
                    txtSession.Focus();
                }
            }
            catch (Exception ex)
            {
                Notify(ex.Message.ToString(), "error", lblMsg);
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
                //DropDownList1.DataBind();
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label PID = GridView1.Rows[index].FindControl("Label1") as Label;
                //DropDownList1.SelectedValue = PID.Text;
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
        {/*
            SqlCommand cmd7 = new SqlCommand("SELECT sl, name, NameNumeric, TutionFee, ClassTeacher FROM [Class] WHERE sl='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                //EditField.Attributes.Remove("class");
                //EditField.Attributes.Add("class", "form-group");
                btnSave.Text = "Update";

                //txtDept.Text = dr[0].ToString();
                //txtDesc.Text = dr[1].ToString();
                //ddSubGrp.SelectedValue = dr[2].ToString();

            }
            cmd7.Connection.Close(); */
        }

        protected void ddClass_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ddSession.DataBind();
        }
    }
}