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
    public partial class Student_List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
            

            if (!IsPostBack)
            {
                ddClass.DataBind();
                GridView1.DataBind();
                string qstring = Request.QueryString["Id"];
                if (qstring != null)
                {
                    lblMsg.Attributes.Add("class", "xerp_info");
                    lblMsg.Text = "Edit mode activated ...";
                }

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
                
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = ex.Message.ToString();
                lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff6230");
            }
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label PID = GridView1.Rows[index].FindControl("Label1") as Label;
                
                lblMsg.Attributes.Add("class", "xerp_info");
                lblMsg.Text = "Edit mode activated ...";
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.ToString();
            }
        }

        protected void txtSession_TextChanged(object sender, EventArgs e)
        {
            //lblStudentID.Text = GenerateStudentID();
            GridView1.DataBind();
        }
    }
}