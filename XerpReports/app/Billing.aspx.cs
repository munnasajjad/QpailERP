using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace Oxford.app
{
    public partial class Billing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
            btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");

            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToShortDateString();
                InvIDNo();

                ddClass.DataBind();
                ddStudent.DataBind();
                GetStudentDetails();

                ddGroup.DataBind();
                ddHead.DataBind();
                GetTutionFee();

                GridView3.DataBind();
                //PopulateAcHeads();
                //lblUser.Text = Page.User.Identity.Name.ToString();
                //btnSave.Enabled = false;
            }
        }
        //Messagebox For Alerts
        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        public string InvIDNo()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (max(VID),0)+1001 )) from BillingMaster", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Connection.Open();
                string InvNo = Convert.ToString(cmd.ExecuteScalar());
                string year = Convert.ToString(Convert.ToDateTime(txtDate.Text).Year); // "2011"; // DateTime.Now.Year.ToString();
                InvNo = "R" + year.Substring(0,2) + "" + InvNo;
                txtVID.Text = InvNo;
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                return InvNo;
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Invalid Date";
                SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (max(VID),0)+1001 )) from BillingMaster", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Connection.Open();
                string InvNo = Convert.ToString(cmd.ExecuteScalar());
                string year = Convert.ToString(DateTime.Now.Year);
                InvNo = "R" + year.Substring(0, 2) + "" + InvNo;txtVID.Text = InvNo;
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                return InvNo;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string isExist = RunQuery.SQLQuery.ReturnString("Select CollectionHead FROM BillingTmp where StudentID='" + ddStudent.SelectedValue + "' AND IsPaid='P' AND CollectionHead='" + ddHead.SelectedValue + "' AND SerialNo <> '" + lblSrl.Text + "'");
                if(isExist=="")
                {
                    if (txtAmount.Text != "")
                    {
                        if (btnAdd.Text == "Add Data to Grid")
                        {
                            InsertData();
                        }
                        else
                        {
                            ExecuteUpdate();
                            btnAdd.Text = "Add Data to Grid";
                            lblMsg.Attributes.Add("class", "xerp_success");
                            lblMsg.Text = "Entry updated successfully";
                        }

                        GridView2.DataBind();
                    }
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_warning");
                    lblMsg.Text = "ERROR: Head Already exist!";
                }


            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = ex.Message.ToString();
                txtAmount.Focus();
            }
        }

        private void InsertData()
        {
            string lName = Page.User.Identity.Name.ToString();
            decimal amt = Convert.ToDecimal(txtAmount.Text);
            decimal discount = GetDiscount(ddStudent.SelectedValue, amt);

            SqlCommand cmd2 = new SqlCommand(@"INSERT INTO BillingTmp (StudentID, CollectionGroup, CollectionHead, Description, Amount, Discount, Due, ExpDate, EntryBy)
                                               VALUES (@StudentID, '" + ddGroup.SelectedValue + "', @CollectionHead, '" + txtDescription.Text + "', @Amount, '" + discount + "', '" + Convert.ToString(amt - discount) + "', @ExpDate, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.AddWithValue("@StudentID", ddStudent.SelectedValue);
            cmd2.Parameters.AddWithValue("@CollectionHead", ddHead.SelectedValue);
            cmd2.Parameters.AddWithValue("@Amount", txtAmount.Text);
            cmd2.Parameters.AddWithValue("@ExpDate", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            cmd2.Parameters.AddWithValue("@EntryBy", lName);
            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
        }

        private void ExecuteUpdate()
        {
            string lName = Page.User.Identity.Name.ToString();
            decimal amt = Convert.ToDecimal(txtAmount.Text);
            decimal discount = GetDiscount(ddStudent.SelectedValue, amt);

            SqlCommand cmd2 = new SqlCommand("UPDATE BillingTmp SET StudentID='" + ddStudent.SelectedValue + "', CollectionGroup='" + ddGroup.SelectedValue + "', CollectionHead='" + ddHead.SelectedValue + "', Description='" + txtDescription.Text + "', Amount=@Amount, Discount=@Discount, Due=@Due  where (SerialNo ='" + lblSrl.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2.Parameters.AddWithValue("@Amount", amt);
            cmd2.Parameters.AddWithValue("@Discount", discount);
            cmd2.Parameters.AddWithValue("@Due", Convert.ToString(amt - discount));
            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
            cmd2.Connection.Dispose();
        }
        private decimal GetDiscount(string sid, decimal amt){
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

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            txtTTL.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Due),0) FROM BillingTmp where StudentID='" + ddStudent.SelectedValue + "' AND IsPaid='P'");
        }
        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.RowIndex);
                Label lblItemCode = GridView2.Rows[index].FindControl("lblSl") as Label;

                RunQuery.SQLQuery.ExecNonQry("DELETE BillingTmp WHERE SerialNo=" + lblItemCode.Text);

                GridView2.DataBind();
                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "Entry removed successfully ...";

            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.Message.ToString();
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string lName = Page.User.Identity.Name.ToString();
                //string trAccHeadDr = RunQuery.SQLQuery.ReturnString("Select HeadIdDr from AccLink where (TID =5)");
                //string trAccHeadCr = RunQuery.SQLQuery.ReturnString("Select HeadIdCr from AccLink where (TID =6)");

                //if (trAccHeadCr != "" && trAccHeadDr != "")
                //{
                    //decimal ttl = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) FROM BillingTmp where StudentID='" + ddStudent.SelectedValue + "' AND IsPaid='P'"));
                    //if (ttl > 0)
                    //{
                        //string voucherNo = Accounting.VoucherEntry.AutoVoucherEntry("6", trAccHeadDr, trAccHeadCr, Convert.ToDecimal(txtTTL.Text), "1", lName);

                string invID = saveData(InvIDNo());
                        //RunQuery.SQLQuery.ExecNonQry("UPDATE BillingTmp SET  IsPaid='A' where StudentID='" + ddStudent.SelectedValue + "' AND IsPaid='P'");
                        InvIDNo();
                        //btnSave.Enabled = false;
                        GridView2.DataBind();
                        GridView1.DataBind();
                        txtTTL.Text = "0";

                        if (chkPrint.Checked)
                        {
                            string url = "./Billing.aspx?Inv=" + invID;
                            ResponseHelper.Redirect(url, "_blank", "menubar=0,width=800,height=600");
                            Response.Write("<script>window.open(" + url + ", '_blank' );</script>");
                            //Response.Redirect = "Purchase.aspx";
                        }

                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "New Receipt Saved Successfully.";
                    //}
                    //else
                    //{
                    //    lblMsg.Attributes.Add("class", "xerp_error");
                    //    lblMsg.Text = "ERROR: No data was found to save!";
                    //}
                //}
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.ToString();
            }
        }

        private string saveData(string invNo)
        {
            SqlCommand cmd2x = new SqlCommand("INSERT INTO BillingMaster (InvoiceNo, InvoiceDate, StudentID, StudentName, Class, Roll, Section, InvoiceAmount, AmountInWords,  Remarks, EntryBy)" +
                                                                "VALUES (@InvoiceNo, @InvoiceDate, @StudentID, @StudentName, @Class, @Roll, @Section, @InvoiceAmount, @AmountInWords, @Remarks, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            string lName = Page.User.Identity.Name.ToString();
            cmd2x.Parameters.Add("@InvoiceNo", SqlDbType.VarChar).Value = invNo;
            cmd2x.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = txtDate.Text;

            cmd2x.Parameters.Add("@StudentID", SqlDbType.NVarChar).Value = ddStudent.SelectedValue;
            cmd2x.Parameters.Add("@StudentName", SqlDbType.NVarChar).Value = ddStudent.SelectedItem.Text;
            cmd2x.Parameters.Add("@Class", SqlDbType.NVarChar).Value = ddClass.SelectedItem.Text;
            cmd2x.Parameters.Add("@Roll", SqlDbType.NVarChar).Value =RunQuery.SQLQuery.ReturnString("Select RollNumber from Students where StudentID='" + ddStudent.SelectedValue + "'");
            cmd2x.Parameters.Add("@Section", SqlDbType.NVarChar).Value = RunQuery.SQLQuery.ReturnString("Select (Select name from Section where sl=Students.Section) AS Section from Students where StudentID='" + ddStudent.SelectedValue + "'");

            cmd2x.Parameters.Add("@InvoiceAmount", SqlDbType.NVarChar).Value =  txtTTL.Text;
            cmd2x.Parameters.Add("@AmountInWords", SqlDbType.VarChar).Value = RunQuery.SQLQuery.NumberToWords(Convert.ToInt32(Convert.ToDouble(txtTTL.Text)))+" Taka Only.";
            cmd2x.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = txtRemarks.Text;
            cmd2x.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;

            //if (ddVendor.SelectedValue != "Select" && ddCustomer.SelectedValue != "Select" &&  ddProduct.SelectedValue != "Select")
            //{
            cmd2x.Connection.Open();
            int success = cmd2x.ExecuteNonQuery();
            cmd2x.Connection.Close();
            
            
            //Saving Gridview Data
            //string acHead; string acHeadID; string description; string amount; string dr; string cr;
            //string vDetail = "";// ddParticular.SelectedValue;

            foreach (GridViewRow row in GridView2.Rows)
            {
                Label lblSl = row.FindControl("lblSl") as Label;
                Label CollectionGroup = row.FindControl("CollectionGroup") as Label;
                Label acHeadID = row.FindControl("acHeadID") as Label;
                Label acHeadName = row.FindControl("CollectionHead") as Label;
                Label lblDesc = row.FindControl("Description") as Label;
                Label lblAmt = row.FindControl("lblAmt") as Label;
                Label lblDisc = row.FindControl("lblDisc") as Label;
                TextBox txtReceived = row.FindControl("txtReceived") as TextBox;
                
                //acHeadID = lblHeadDr.Text;
                string desc = HttpUtility.HtmlDecode(CollectionGroup.Text) + ": " + HttpUtility.HtmlDecode(acHeadName.Text);
                if (lblDesc.Text != "")
                {
                    desc = desc + " (" + HttpUtility.HtmlDecode(lblDesc.Text) + ")";
                }

                SqlCommand cmd2y = new SqlCommand("INSERT INTO BillingDetails (InvoiceNo, Description, AccountsHeadID, AccountsHeadName, InvoiceDR, InvoiceCR, EntryDate, EntryID)" +
                                                    "VALUES (@BillingNo, @BillingRowDescription, @AccountsHeadID, @AccountsHeadName, @BillingDR, @BillingCR, @EntryDate, '" + lblSl.Text + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd2y.Parameters.Add("@BillingNo", SqlDbType.VarChar).Value = invNo;
                cmd2y.Parameters.Add("@BillingRowDescription", SqlDbType.VarChar).Value = desc;
                cmd2y.Parameters.Add("@AccountsHeadID", SqlDbType.VarChar).Value = acHeadID.Text;
                cmd2y.Parameters.Add("@AccountsHeadName", SqlDbType.VarChar).Value = acHeadName.Text;
                cmd2y.Parameters.Add("@BillingDR", SqlDbType.Decimal).Value = txtReceived.Text;
                cmd2y.Parameters.Add("@BillingCR", SqlDbType.Decimal).Value = "0";
                cmd2y.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = txtDate.Text;
                cmd2y.Connection.Open();
                cmd2y.ExecuteNonQuery();
                cmd2y.Connection.Close();

                //Dr insert
                string isPaid = "A";
                decimal due = Convert.ToDecimal(lblAmt.Text) - Convert.ToDecimal(lblDisc.Text);
                if (due >= Convert.ToDecimal(txtReceived.Text))
                {
                    isPaid = "P";
                    due = due - Convert.ToDecimal(txtReceived.Text);
                }
                else
                {
                    due = 0;
                }

                RunQuery.SQLQuery.ReturnString("UPDATE BillingTmp Set IsPaid='" + isPaid + "', Received='" + txtReceived.Text + "', Due='" + due + "', InvoiceNo='" + invNo + "' WHERE SerialNo='" + lblSl.Text + "'");
                //TransactionEntry(acHeadID, lblDr.Text, "0", invNo);
            }
            
            return invNo;
        }

        private void TransactionEntry(string acHeadID, string dr, string cr, string invNo)
        {
            //string lName = Page.User.Identity.Name.ToString();
            //string controlId = RunQuery.SQLQuery.ReturnString("Select ControlAccountsID from HeadSetup where AccountsHeadID='" + acHeadID + "'");
            ////Auto Transaction Entry: Party/Vendor (020101), Customer (), Cash (010101), Bank (010102)
            //if (controlId == "020101")//Payables/Liabilities
            //{
            //    string supplierId = RunQuery.SQLQuery.ReturnString("Select PID from Vendors where SyncAccountHead='" + acHeadID + "'");
            //    Accounting.BillingEntry.TransactionEntry(txtDate.Text, supplierId, "", "", "", "0", "0", "", "0", dr, cr, "0", "Auto Entry From Billing Entry", "Vendor", "Purchase", lName, "", "0", invNo);
            //}
            //else if (controlId == "010105")
            //{
            //    string customerId = RunQuery.SQLQuery.ReturnString("Select PID from Resellers where SyncAccountHead='" + acHeadID + "'");
            //    Accounting.BillingEntry.TransactionEntry(txtDate.Text, customerId, "", "", "", "0", "0", "", "0", dr, cr, "0", "Auto Entry From Billing Entry", "Customer", "Sales", lName, "", "0", invNo);
            //}
            //else if (controlId == "010101")
            //{
            //    string bankId = RunQuery.SQLQuery.ReturnString("Select PID from VACCBankCash where SyncAccountHead='" + acHeadID + "'");
            //    Accounting.BillingEntry.TransactionEntry(txtDate.Text, "", "", "Cash", bankId, "0", "0", "", "0", dr, cr, "0", "Auto Entry From Billing Entry", "Bank", "Deposit", lName, "", "0", invNo);
            //}
            //else if (controlId == "010102")
            //{
            //    string bankId = RunQuery.SQLQuery.ReturnString("Select PID from VACCBankCash where SyncAccountHead='" + acHeadID + "'");
            //    Accounting.BillingEntry.TransactionEntry(txtDate.Text, "", "", "Bank", bankId, "0", "0", "", "0", dr, cr, "0", "Auto Entry From Billing Entry", "Bank", "Deposit", lName, "", "0", invNo);
            //}
        }

        private void PopulateDropDown(DropDownList ddl, string query1, string query2)
        {


        }

        public static class ResponseHelper
        {
            public static void Redirect(string url, string target, string windowFeatures)
            {
                HttpContext context = HttpContext.Current;

                if ((String.IsNullOrEmpty(target) ||
                    target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                    String.IsNullOrEmpty(windowFeatures))
                {

                    context.Response.Redirect(url);
                }
                else
                {
                    Page page = (Page)context.Handler;
                    if (page == null)
                    {
                        throw new InvalidOperationException(
                            "Cannot redirect to new window outside Page context.");
                    }
                    url = page.ResolveClientUrl(url);

                    string script;
                    if (!String.IsNullOrEmpty(windowFeatures))
                    {
                        script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                    }
                    else
                    {
                        script = @"window.open(""{0}"", ""{1}"");";
                    }

                    script = String.Format(script, url, target, windowFeatures);
                    ScriptManager.RegisterStartupScript(page,
                        typeof(Page),
                        "Redirect",
                        script,
                        true);
                }
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            //PopulateAcHeads();
        }
        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(GridView2.SelectedIndex);
                Label Label2 = GridView2.Rows[index].FindControl("lblSl") as Label;
                lblSrl.Text = Label2.Text;

                //PopulateAcHeads();

                SqlCommand cmd7 = new SqlCommand("SELECT CollectionGroup, CollectionHead, Description, Amount  FROM [BillingTmp] WHERE SerialNo ='" + lblSrl.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                SqlDataReader dr = cmd7.ExecuteReader();

                if (dr.Read())
                {
                    btnAdd.Text = "Update";
                    ddGroup.SelectedValue = dr[0].ToString();
                    ddHead.DataBind();
                    ddHead.SelectedValue = dr[1].ToString();
                    txtDescription.Text = dr[2].ToString();
                    txtAmount.Text = Convert.ToString(dr[3].ToString());

                    lblMsg.Attributes.Add("class", "xerp_info");
                    lblMsg.Text = "Entry loaded in edit mode";
                }

                cmd7.Connection.Close();
                //pan.Update();
            }
            catch (Exception ex)
            {
                lblMsg.CssClass = "xerp_error";
                lblMsg.Text = "ERROR: " + ex.ToString();
            }
        }

        protected void ddParticular_SelectedIndexChanged(object sender, EventArgs e)
        {
            //PopulateAcHeads();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtDate.Text = DateTime.Now.ToShortDateString();
            InvIDNo();
            //ddParticular.DataBind();
            //PopulateAcHeads();

            //RunQuery.SQLQuery.ExecNonQry("Delete BillingTmp where EntryBy='" + Page.User.Identity.Name.ToString() + "'");
        }

        protected void ddClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddStudent.DataBind();
            GetStudentDetails();
            GetTutionFee();
        }
    
        protected void ddStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetStudentDetails();
        }
        private void GetStudentDetails()
        {
            ltrID.Text = "<b>ID: " + ddStudent.SelectedValue + "</b>, &nbsp; "+
                "Sec.: " + RunQuery.SQLQuery.ReturnString("Select (Select name from Section where sl=Students.Section) AS Section from Students where StudentID='" + ddStudent.SelectedValue + "'") + ", &nbsp; " +
                "Roll: " + RunQuery.SQLQuery.ReturnString("Select RollNumber from Students where StudentID='" + ddStudent.SelectedValue + "'") + ", &nbsp; " +
                            "   <br/>Father: " + RunQuery.SQLQuery.ReturnString("Select FatherNameE from Students where StudentID='" + ddStudent.SelectedValue + "'");
            GridView2.DataBind();
            GridView3.DataBind();
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
                string isPaid = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(Amount),0) from BillingTmp where CollectionHead='" + ddHead.SelectedValue + "' AND StudentID='" + ddStudent.SelectedValue + "' AND EntryDate>='" + DateTime.Now.Year + "-01-01' AND IsPaid='A'");
                decimal amt=Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("Select TutionFee from Class Where sl='" + ddClass.SelectedValue + "'"));

                if (Convert.ToDecimal(isPaid) == 0)
                {
                    txtAmount.Text = amt.ToString();
                }
                else if (Convert.ToDecimal(isPaid) >= amt)
                {
                    txtAmount.Text = Convert.ToString(amt - Convert.ToDecimal(isPaid));
                    ltrNotice.Text = "This fee has already been paid!";
                }
                else
                {
                    txtAmount.Text = Convert.ToString(amt - Convert.ToDecimal(isPaid));
                    ltrNotice.Text = "This month is partially paid!";
                }
            }
            else
            {
                txtAmount.Text = RunQuery.SQLQuery.ReturnString("Select Amount from CollectionHeads Where sl='" + ddHead.SelectedValue + "'");
            }
        }
    }
}