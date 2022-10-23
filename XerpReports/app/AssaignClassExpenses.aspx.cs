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
    public partial class AssaignClassExpenses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                ddGroup.DataBind();
                ddHead.DataBind();
                BingClassDD();
                GridView1.DataBind();
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

        private void BingClassDD()
        {
            ddClass.Items.Clear();
            ListItem lst = new ListItem("--- all ---", "");
            ddClass.Items.Insert(ddClass.Items.Count, lst);
            ddClass.DataBind();
            GetTutionFee();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
                if (txtAmount.Text=="")
                {
                    txtAmount.Text = "0";
                }

                string className = ddClass.SelectedValue;
                //if (ddClass.SelectedValue == "--- all ---") //Assign to all class
                //{
                //    className = "";
                //}


                if (btnSave.Text == "Save")
                {
                    string isExist =RunQuery.SQLQuery.ReturnString("SELECT EID from AssignExpense where Class='" +
                                                       className + "' AND ExpHead='" +
                                                       ddHead.SelectedValue + "' ");

                    if (isExist == "")
                    {
                        RunQuery.SQLQuery.ExecNonQry(
                            "Insert into AssignExpense ( Class, ExpDate, ExpGroup, ExpHead, Description, Amount, EntryBy) Values ('" +
                            className + "', '" + dt + "', '" + ddGroup.SelectedValue + "', '" +
                            ddHead.SelectedValue + "', '" + txtDescription.Text + "', '" + txtAmount.Text + "', '" +
                            Page.User.Identity.Name.ToString() + "')");

                        if (ddClass.SelectedValue == "") //Assign to all class
                        {
                            decimal amt = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("Select Amount from CollectionHeads Where sl='" + ddHead.SelectedValue + "'"));

                            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT [sl], [Name], TutionFee FROM [Class] ");

                            foreach (DataRow drx in dtx.Rows)
                            {
                                string pid = drx["sl"].ToString();
                                string SenderName = drx["Name"].ToString();
                                if (ddGroup.SelectedValue == "1")
                                {
                                    amt = Convert.ToDecimal(drx["TutionFee"].ToString());
                                }
                                InsertData(pid, amt);
                            }
                        }
                        else
                        {
                            if (txtAmount.Text != "")
                            {
                                InsertData(ddClass.SelectedValue, Convert.ToDecimal(txtAmount.Text));
                            }
                            else
                            {
                                Notify("Invalid Amount", "error", lblMsg);
                            }
                        }

                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Saved Successfully";
                    }
                    else
                    {
                        lblMsg.Attributes.Add("class", "xerp_error");
                        lblMsg.Text = "Unable to Save: The entry already exist!";
                    }
                }
                else
                {
                    RunQuery.SQLQuery.ExecNonQry("UPDATE AssignExpense SET Class='" + ddClass.SelectedValue +
                                                 "', ExpDate='" + dt + "', ExpGroup='" + ddGroup.SelectedValue +
                                                 "', ExpHead='" + ddHead.SelectedValue + "', Description='" +
                                                 txtDescription.Text + "', Amount='" + txtAmount.Text +
                                                 "', UpdateBy='" + Page.User.Identity.Name.ToString() +
                                                 "'  where (sl ='" + DropDownList1.SelectedValue + "')");

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Updated Successfully";
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

        private void InsertData(string className, decimal amt)
        {
            string dt1 = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
            //decimal amt = Convert.ToDecimal(txtAmount.Text);

            DataTable dt = RunQuery.SQLQuery.ReturnDataTable("Select  sl, StudentID from Students where Class='" + className + "' order by RollNumber");

            foreach (DataRow row in dt.Rows)
            {
                string sl = row["sl"].ToString();
                string sid = row["StudentID"].ToString();
                decimal discount = GetDiscount(sid, amt);

                RunQuery.SQLQuery.ExecNonQry(
                            "Insert into BillingTmp (StudentID, ExpDate, CollectionGroup, CollectionHead, Description, Amount, Discount, Received, Due, EntryBy) Values ('" +
                            sid + "', '" + dt1 + "', '" + ddGroup.SelectedValue + "', '" +
                            ddHead.SelectedValue + "', '" + txtDescription.Text + "', '" + amt + "', '" + discount + "', '0', '" + Convert.ToString(amt - discount) + "', '" +
                            Page.User.Identity.Name.ToString() + "')");
            }

        }
        private decimal GetDiscount(string sid, decimal amt)
        {
            decimal discountAmt = 0;
            SqlCommand cmd7 = new SqlCommand("SELECT Top(1) DiscountAmount, DiscountPerchantage FROM [Discounts] WHERE StudentID='" +
                    sid + "' AND DiscountHead='" + ddHead.SelectedValue + "' Order By did desc", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
            SqlCommand cmd7 = new SqlCommand("SELECT sl, name, NameNumeric, TutionFee, ClassTeacher FROM [Class] WHERE sl='" + DropDownList1.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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


                ddClass.SelectedValue = dr[1].ToString();
                //txtNameNumeric.Text = dr[2].ToString();
                //txtTutionFee.Text = dr[3].ToString();
                //ddClassTeacher.SelectedValue = dr[4].ToString();
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
            if (ddGroup.SelectedValue == "1")
            {
                if (ddClass.SelectedValue == "") //Assign to all class
                {
                    txtAmount.Enabled = false;
                    txtAmount.Text = "";
                }
                else
                {
                    txtAmount.Enabled = true;
                    txtAmount.Text = RunQuery.SQLQuery.ReturnString("Select TutionFee from Class Where sl='" + ddClass.SelectedValue + "'");
                }
            }
            else
            {
                txtAmount.Enabled = true; txtAmount.Text = RunQuery.SQLQuery.ReturnString("Select Amount from CollectionHeads Where sl='" + ddHead.SelectedValue + "'");
            }
        }

        protected void ddClass_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GetTutionFee();
            GridView1.DataBind();
        }
    }
}