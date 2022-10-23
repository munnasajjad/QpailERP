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
    public partial class Expense_Entry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                ddGroup.DataBind();
                ddHead.DataBind();
                GetTutionFee();
                GridView1.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAmount.Text != "")
                {
                    string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");

                    if (btnSave.Text == "Save")
                    {

                        RunQuery.SQLQuery.ExecNonQry(
                            "Insert into Expenses ( ExpDate, ExpGroup, ExpHead, Description, Amount, EntryBy) Values ('" +
                            dt + "', '" + ddGroup.SelectedValue + "', '" +
                            ddHead.SelectedValue + "', '" + txtDescription.Text + "', '" + txtAmount.Text + "', '" +
                            Page.User.Identity.Name.ToString() + "')");
                        //InsertData();

                        txtDescription.Text = "";
                        txtAmount.Text = "";
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Saved Successfully";

                    }
                    else
                    {
                        RunQuery.SQLQuery.ExecNonQry("UPDATE Expenses SET ExpDate='" + dt + "', ExpGroup='" + ddGroup.SelectedValue +
                                                     "', ExpHead='" + ddHead.SelectedValue + "', Description='" +
                                                     txtDescription.Text + "', Amount='" + txtAmount.Text +
                                                     "', UpdateBy='" + Page.User.Identity.Name.ToString() +
                                                     "'  where (eid ='" + DropDownList1.SelectedValue + "')");

                        txtDescription.Text = "";
                        txtAmount.Text = "";
                        btnSave.Text = "Save";

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
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Unable to Save: " + ex.ToString();
            }
            finally
            {
                GridView1.DataBind();
                //ClearControls(Form);
                btnSave.Text = "Save";
                EditField.Attributes.Add("class", "form-group hidden");
            }
        }

        //private void InsertData()
        //{
        //    string dt1 = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
        //    decimal amt = Convert.ToDecimal(txtAmount.Text);

        //    DataTable dt = RunQuery.SQLQuery.ReturnDataTable("Select  sl, StudentID from Students where Class='" + ddClass.SelectedValue + "' order by RollNumber");

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        string sl = row["sl"].ToString();
        //        string sid = row["StudentID"].ToString();
        //        decimal discount = GetDiscount(sid, amt);

        //        RunQuery.SQLQuery.ExecNonQry(
        //                    "Insert into BillingTmp (StudentID, ExpDate, ExpenseGroup, ExpenseHead, Description, Amount, Discount, Received, Due, EntryBy) Values ('" +
        //                    sid + "', '" + dt1 + "', '" + ddGroup.SelectedValue + "', '" +
        //                    ddHead.SelectedValue + "', '" + txtDescription.Text + "', '" + amt + "', '" + discount + "', '0', '" + Convert.ToString(amt - discount) + "', '" +
        //                    Page.User.Identity.Name.ToString() + "')");
        //    }

        //}
        private decimal GetDiscount(string sid, decimal amt)
        {
            decimal discountAmt = 0;
            SqlCommand cmd7 =
                new SqlCommand(
                    "SELECT Top(1) DiscountAmount, DiscountPerchantage FROM [Discounts] WHERE StudentID='" +
                    sid + "' AND DiscountHead='" + ddHead.SelectedValue + "' Order By did desc",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                discountAmt = Convert.ToDecimal(dr[0].ToString());
                if (discountAmt == 0)
                {
                    discountAmt = Convert.ToDecimal(dr[1].ToString());
                    discountAmt = amt * discountAmt / 100M;
                }
            }
            cmd7.Connection.Close();
            return discountAmt;
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
            SqlCommand cmd7 = new SqlCommand("SELECT ExpDate, ExpGroup, ExpHead, Description, Amount FROM [Expenses] WHERE eid='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                //EditField.Attributes.Remove("class");
                //EditField.Attributes.Add("class", "form-group");
                btnSave.Text = "Update";

                txtDate.Text = dr[0].ToString();
                ddGroup.SelectedValue = dr[1].ToString();
                ddHead.DataBind();
                ddHead.SelectedValue = dr[2].ToString();

                txtDescription.Text = dr[3].ToString();
                txtAmount.Text = dr[4].ToString();
            }
            cmd7.Connection.Close();
        }


        protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddHead.DataBind();
            GetTutionFee();
        }
        protected void ddHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTutionFee();
        }


        private void GetTutionFee()
        {
            txtAmount.Text = RunQuery.SQLQuery.ReturnString("Select Amount from ExpenseHeads Where sl='" + ddHead.SelectedValue + "'");
        }

        protected void btnClear_OnClick(object sender, EventArgs e)
        {
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ddGroup.DataBind();
            ddHead.DataBind();
            GetTutionFee();
            GridView1.DataBind();
            txtDescription.Text = "";
            txtAmount.Text = "";
            btnSave.Text = "Save";
        }
    }
}