using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Oxford.app
{
    public partial class Particulars : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDiag.Text != "" && btnSave.Text == "Save")
                {
                    RunQuery.SQLQuery.ExecNonQry("Insert into Particulars (Particularsname, Detail) VALUES ('" + txtDiag.Text + "','" + txtDetails.Text + "')");
                    txtDiag.Text = ""; txtDetails.Text = "";
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Successfully Added new particular";
                }
                else if (txtDiag.Text != "" && btnSave.Text == "Update")
                {
                    RunQuery.SQLQuery.ExecNonQry("Update Particulars set Particularsname='" + txtDiag.Text + "', Detail='" + txtDetails.Text + "' WHERE Particularsid='" + DropDownList1.SelectedValue + "'");
                    txtDiag.Text = ""; txtDetails.Text = "";

                    EditField.Attributes.Remove("class");
                    EditField.Attributes.Add("class", "hidden");
                    btnSave.Text = "Save";
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Successfully updated particular info";
                }
                else { lblMsg.Text = "Please Type the particular Name Correctly"; }

                GridView2.DataBind();

            }
            catch (Exception ex)
            {
                lblMsg.Text = "ERROR: " + ex.Message.ToString();
            }
        }



        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList1.DataBind();
                int index = Convert.ToInt32(GridView2.SelectedIndex);
                Label CrID = GridView2.Rows[index].FindControl("Label1") as Label;
                DropDownList1.SelectedValue = CrID.Text;
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
            SqlCommand cmd7 = new SqlCommand("SELECT  Particularsname, Detail FROM [Particulars] WHERE Particularsid='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                EditField.Attributes.Remove("class");
                EditField.Attributes.Add("class", "form-group");
                btnSave.Text = "Update";

                txtDiag.Text = dr[0].ToString();
                txtDetails.Text = dr[1].ToString();
            }
            cmd7.Connection.Close();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditMode();
        }
    }
}