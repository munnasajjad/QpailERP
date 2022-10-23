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
    public partial class DiscountToStudents : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
            if (!IsPostBack)
            {
                ddClass.DataBind();
                ddStudent.DataBind();
                GetStudentDetails();
                RadioButton1.Checked = true;
                ddGroup.DataBind();
                BingHeadDD();
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string discountType = "Taka";
                decimal amount = Convert.ToDecimal(txtAmount.Text);
                decimal percent = 0;

                if(RadioButton2.Checked)
                {
                    discountType = "Percent";
                    amount = 0;
                    percent = Convert.ToDecimal(txtAmount.Text);
                }

                //if (discountType == "Percent")
                //{
                //    if 
                //}

                    if (btnSave.Text == "Save" && ddStudent.SelectedValue != "" && ddHead.SelectedValue != "")
                    {
                        if (ddHead.SelectedValue == "--- all ---") //Assign to all discount fees
                        {
                            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT [sl], [Name] FROM [CollectionHeads] WHERE ([GroupID] = '"+ddGroup.SelectedValue+"')");

                            foreach (DataRow drx in dtx.Rows)
                            {
                                string pid = drx["sl"].ToString();
                                string SenderName = drx["Name"].ToString();
                                SaveDiscount(discountType, pid, amount.ToString(), percent.ToString());
                            }
                        }
                        else
                        {
                            SaveDiscount(discountType, ddHead.SelectedValue, amount.ToString(), percent.ToString());
                        }
                    }
                    else
                    {
                        RunQuery.SQLQuery.ExecNonQry("UPDATE Discounts SET StudentID='" + ddStudent.SelectedValue + "', DiscountType='" + discountType + "', DiscountGroup='" + ddGroup.SelectedValue + "',  DiscountHead='" + ddHead.SelectedValue + "', DiscountAmount='" + amount + "',  DiscountPerchantage=" + percent + ", Description= '" + txtDescription.Text + "'   where (did ='" + lblOldEntryId.Text + "')");
                        
                        //ClearControls(Form);
                        btnSave.Text = "Save";
                        //EditField.Attributes.Add("class", "form-group hidden");

                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "Updated Successfully";
                    }
                //}
                //else
                //{
                //    //MessageBox("Empty data is not accepted.");
                //}
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

        private void SaveDiscount(string discountType, string colHead, string amount, string percent)
        {

            string isExist = RunQuery.SQLQuery.ReturnString("Select DiscountHead from Discounts where StudentID='" + ddStudent.SelectedValue + "' AND DiscountHead='" + ddHead.SelectedValue + "'");
            if (isExist == "")
            {
                RunQuery.SQLQuery.ExecNonQry(@"Insert into Discounts (StudentID, DiscountType, DiscountGroup, DiscountHead, DiscountAmount, DiscountPerchantage, EntryBy, Description) 
                                                         Values ('" + ddStudent.SelectedValue + "', '" + discountType + "', '" + ddGroup.SelectedValue + "', '" + colHead + "', '" + amount + "', " + percent + ", '" + Page.User.Identity.Name.ToString() + "', '" + txtDescription.Text + "')");

                lblMsg.Text = "Discount Saved!";
            }
            else
            {
                lblMsg.Text = "ERROR : This discount is already exist for the student!";
                lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff6230");
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
                lblOldEntryId.Text = PID.Text;
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
            SqlCommand cmd7 = new SqlCommand("SELECT StudentID, DiscountType, DiscountGroup, DiscountHead, DiscountAmount, DiscountPerchantage FROM [Discounts] WHERE did='" + lblOldEntryId.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            SqlDataReader dr = cmd7.ExecuteReader();
            if (dr.Read())
            {
                btnSave.Text = "Update";
                ddStudent.SelectedValue = dr[0].ToString();

                RadioButton1.Checked = true;
                txtAmount.Text = dr[4].ToString();
                string dType= dr[1].ToString();
                if (dType == "Percent")
                {
                    RadioButton2.Checked=true;
                    txtAmount.Text = dr[5].ToString();  
                }

                ddGroup.SelectedValue = dr[2].ToString();
                ddHead.SelectedValue = dr[3].ToString();
            }
            cmd7.Connection.Close();
        }

        protected void ddClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddStudent.DataBind();
            GetStudentDetails();
        }
        protected void ddStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetStudentDetails();
        }
        private void GetStudentDetails()
        {
            ltrID.Text = "<b>ID: " + ddStudent.SelectedValue + "</b>, &nbsp; " +
                "Sec.: " + RunQuery.SQLQuery.ReturnString("Select (Select name from Section where sl=Students.Section) AS Section from Students where StudentID='" + ddStudent.SelectedValue + "'") + ", &nbsp; " +
                "Roll: " + RunQuery.SQLQuery.ReturnString("Select RollNumber from Students where StudentID='" + ddStudent.SelectedValue + "'") + ", &nbsp; " +
                            "   <br/>Father: " + RunQuery.SQLQuery.ReturnString("Select FatherNameE from Students where StudentID='" + ddStudent.SelectedValue + "'");
            
            GridView1.DataBind();
        }

        protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            BingHeadDD();
        }

        private void BingHeadDD()
        {
            ddHead.Items.Clear();
            ListItem lst = new ListItem("--- all ---", "--- all ---");
            ddHead.Items.Insert(ddHead.Items.Count, lst);
            ddHead.DataBind();

            GetTutionFee();
        }
        protected void ddHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetTutionFee();
        }

        private void GetTutionFee()
        {
            
        }
    }
}