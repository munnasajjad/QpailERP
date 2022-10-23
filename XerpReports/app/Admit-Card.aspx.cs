using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Oxford.app
{
    public partial class Admit_Card : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.AddDays(7).ToShortDateString();
                ddClass.DataBind();
                //ddStudent.DataBind();
                GridView1.DataBind();
            }
        }

        protected void ddClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddStudent.DataBind(); GridView1.DataBind();
        }

        protected void lbAll_Click(object sender, EventArgs e)
        {
            PrintStudentID("");
        }

        protected void lbClass_Click(object sender, EventArgs e)
        {
            PrintStudentID(" WHERE Class='" + ddClass.SelectedValue + "'");
        }

        protected void lbStudent_Click(object sender, EventArgs e)
        {
            //PrintStudentID(" WHERE StudentID='" + ddStudent.SelectedValue + "'");
        }

        private void PrintStudentID(string query)
        {
            try
            {
                RunQuery.SQLQuery.ExecNonQry("DELETE tmpIdCard");

                DataSet ds = RunQuery.SQLQuery.ReturnDataSet("Select StudentID, StudentNameE, FatherNameE,(SELECT name FROM Class where sl=Students.Class) AS Class, RollNumber, Section, Session, StudentPhoto from Students " + query);
                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows) //Each Items Will be looped
                {
                    string studentId = row["StudentID"].ToString();
                    string studentName = row["StudentNameE"].ToString();
                    string fatherName = txtDate.Text; //row["FatherNameE"].ToString();

                    string className = row["Class"].ToString();
                    string roll = row["RollNumber"].ToString();
                    string section = row["Section"].ToString();
                    string photo = "../" + row["StudentPhoto"].ToString();
                    string session = txtExam.Text; //row["Session"].ToString();

                    RunQuery.SQLQuery.ExecNonQry(@"Insert into tmpIdCard (StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl) 
                               Values ('" + studentId + "','" + studentName + "','" + fatherName + "','" + className + "','" + roll + "','" + section + "','" + session + "','" + photo + "')");

                }

                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('./Reports/rptAdmitCard.aspx','_newtab');", true);

            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }

    }
}