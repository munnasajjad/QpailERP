using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Oxford.app
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ltrtotal.Text = "0"; 
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string SClass = " ";
                if (ddClass.SelectedValue != "---ALL---")
                {
                    SClass = " AND Class='" + ddClass.SelectedValue + "'";
                }
                
                string sid = " ";
                if (txtStudentId.Text != "")
                {
                    sid = " AND  (StudentID LIKE '%" + txtStudentId.Text + "%')";
                }

                string sNameB = " ";
                if (txtStudentNameB.Text != "")
                {
                    sNameB = " AND  (StudentNameB LIKE '%" + txtStudentNameB.Text + "%')";
                }

                string sNameE = " ";
                if (txtStudentNameE.Text != "")
                {
                    sNameE = " AND  (StudentNameE LIKE '%" + txtStudentNameE.Text + "%')";
                }

                string rollNo = " ";
                if (txtRollNo.Text != "")
                {
                    rollNo = " AND  (RollNumber = '" + txtRollNo.Text + "')";
                }

                string sex = " ";
                if (ddGender.SelectedValue != "---ALL---")
                {
                    sex = " AND Gender='" + ddGender.SelectedValue + "'";
                }

                string religion = " ";
                if (ddReligion.SelectedValue != "---ALL---")
                {
                    religion = " AND Religion='" + ddReligion.SelectedValue + "'";
                }
                
                string year = " ";
                if (txtSession.Text != "")
                {
                    if (txtSession.Text.Length == 4)
                    {
                        year = " AND     (DATEPART(year, ApplicationDate) = " + txtSession.Text + ")";
                    }
                    else
                    {
                        lblMsg.Attributes.Add("class", "xerp_error");
                        lblMsg.Text = "Invalid Admission Year!";
                    }
                }

                string query=SClass+sid+sNameB+sNameE+rollNo+sex+religion+year;
                DataSet ds = RunQuery.SQLQuery.ReturnDataSet("Select sl, StudentID, StudentNameB, StudentNameE,(SELECT name FROM Class where sl=Students.Class) AS Class, RollNumber from Students WHERE sl<>0" + query);
                ltrtotal.Text = ds.Tables[0].Rows.Count.ToString();
                GridView1.DataSource = ds.Tables[0];
                GridView1.DataBind();
            }
            catch(Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.ToString();
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label PID = GridView1.Rows[index].FindControl("Label1") as Label;

                Response.Redirect("./Admission.aspx?ID=" + PID.Text);
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.ToString();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddClass.DataBind();
            ddGender.DataBind();
            ddReligion.DataBind();
            txtRollNo.Text = "";
            txtSession.Text = "";
            txtStudentId.Text = "";
            txtStudentNameB.Text = "";
            txtStudentNameE.Text = "";
            
            GridView1.DataSource = null;
            GridView1.DataBind();
            ltrtotal.Text = "0"; 

        }

    }
}