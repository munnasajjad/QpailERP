using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace Oxford.app
{
    public partial class Send_SMS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddClass.DataBind();
                ddStudent.DataBind();
                ddTemplate.DataBind();
                txtSMS.Text =RunQuery.SQLQuery.ReturnString("Select SmsBody from SMSTemplate where sl='" + ddTemplate.SelectedValue + "'");
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
      
        protected void ddClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddStudent.DataBind(); GridView1.DataBind();
        }

        protected void lbAll_Click(object sender, EventArgs e)
        {
            BulkSMStoStudents("SELECT PhoneMobile FROM Students WHERE (IsActive = '1')");
        }

        private void BulkSMStoStudents(string query)
        {
            try
            {
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(query);
            string mobs = "";
            int i = 1;
            foreach (DataRow drx in dtx.Rows)
            {
                string cell = drx["PhoneMobile"].ToString();
                if (cell.Length > 10)
                {
                    if (cell.Length < 13)
                    {
                        cell = "88" + cell;
                    }
                    if (dtx.Rows.Count != i)
                    {
                        mobs = mobs + cell + ",";
                    }
                    else
                    {
                        mobs = mobs + cell;
                    }
                }
                i++;}

            SendSMS(mobs);
            }
            catch (Exception ex)
            {
                Notify(ex.Message.ToString(), "error", lblMsg);
                lblMsg.Text = ex.ToString();
            }
        }

        protected void lbClass_Click(object sender, EventArgs e)
        {
            BulkSMStoStudents("SELECT PhoneMobile FROM Students WHERE (IsActive = '1') AND (Class='"+ddClass.SelectedValue+"')");
        }

        protected void lbStudent_Click(object sender, EventArgs e)
        {
            string cell =RunQuery.SQLQuery.ReturnString("Select PhoneMobile from Students where StudentID='" + ddStudent.SelectedValue + "'");
            if (cell.Length < 13){
                cell = "88" + cell;
            }
            SendSMS(cell);
        }

        private void SendSMS(string mob)
        {
            try
            {
                RunQuery.SQLQuery.SendSMS(mob, txtSMS.Text, "0");
                //lblMob.Text = mob;
                Notify("Message Send Successfully.", "success", lblMsg);
            }
            catch (Exception ex)
            {
                Notify(ex.Message.ToString(), "error", lblMsg);
                lblMsg.Text = ex.ToString();
            }
        }

        protected void ddTemplate_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            txtSMS.Text = RunQuery.SQLQuery.ReturnString("Select SmsBody from SMSTemplate where sl='" + ddTemplate.SelectedValue + "'");}

        protected void lbOthers_OnClick(object sender, EventArgs e)
        {
            string cell = txtOther.Text;
            if (cell.Length < 13)
            {
                cell = "88" + cell;
            }
            SendSMS(cell);
        }
    }
}