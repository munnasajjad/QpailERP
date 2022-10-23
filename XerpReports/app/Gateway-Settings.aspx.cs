using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace Oxford.app
{
    public partial class Gateway_Settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadElements();
            }
        }

        private void LoadElements()
        {
            //Route SMS
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1)  GatewayName, HostIP, SenderName, UserID, Password, IsActive FROM SMSGateway WHERE (pid = '1')");

            foreach (DataRow drx in dtx.Rows)
            {
                txtHost.Text = drx["HostIP"].ToString();
                TextBox1.Text = drx["SenderName"].ToString();
                TextBox2.Text = drx["UserID"].ToString();
                TextBox3.Text = drx["Password"].ToString();
                if (drx["IsActive"].ToString() == "1")
                {
                    rbRoute.Checked = true;
                }
            }

            //Bulk SMS
            dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1)  GatewayName, HostIP, SenderName, UserID, Password, IsActive FROM SMSGateway WHERE (pid = '2')");

            foreach (DataRow drx in dtx.Rows)
            {
                txtHost2.Text = drx["HostIP"].ToString();
                TextBox4.Text = drx["SenderName"].ToString();
                TextBox5.Text = drx["UserID"].ToString();
                TextBox6.Text = drx["Password"].ToString();
                if (drx["IsActive"].ToString() == "1")
                {
                    rbBulk.Checked = true;
                }
            }


            //Extreme SMS
            dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1)  GatewayName, HostIP, SenderName, UserID, Password, IsActive FROM SMSGateway WHERE (pid = '3')");

            foreach (DataRow drx in dtx.Rows)
            {
                txtHost3.Text = drx["HostIP"].ToString();
                TextBox8.Text = drx["SenderName"].ToString();
                TextBox9.Text = drx["UserID"].ToString();
                TextBox10.Text = drx["Password"].ToString();
                if (drx["IsActive"].ToString() == "1")
                {
                    rbExtreme.Checked = true;
                }
            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            try
            {
                //Route SMS
                RunQuery.SQLQuery.ExecNonQry("UPDATE SMSGateway SET IsActive=0");

                string isActive = "0";
                if (rbRoute.Checked == true)
                {
                    isActive = "1";
                }
                RunQuery.SQLQuery.ExecNonQry("UPDATE SMSGateway SET HostIP='" + txtHost.Text + "', SenderName='" + TextBox1.Text + "', UserID='" + TextBox2.Text + "', Password='" + TextBox3.Text + "', IsActive='" + isActive + "'  WHERE (pid = '1')");

                //Bulk SMS
                isActive = "0";
                if (rbBulk.Checked == true)
                {
                    isActive = "1";
                }
                RunQuery.SQLQuery.ExecNonQry("UPDATE SMSGateway SET HostIP='" + txtHost2.Text + "', SenderName='" + TextBox4.Text + "', UserID='" + TextBox5.Text + "', Password='" + TextBox6.Text + "', IsActive='" + isActive + "'  WHERE (pid = '2')");

                //Extreme SMS
                isActive = "0";
                if (rbExtreme.Checked == true)
                {
                    isActive = "1";
                }
                RunQuery.SQLQuery.ExecNonQry("UPDATE SMSGateway SET HostIP='" + txtHost3.Text + "', SenderName='" + TextBox8.Text + "', UserID='" + TextBox9.Text + "', Password='" + TextBox10.Text + "', IsActive='" + isActive + "'  WHERE (pid = '3')");

                lblMsg.Text = "Information updated successfully.";}
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }
    }
}