using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using RunQuery;

public partial class app_Emp_Daily_Attn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ddClass.DataBind();
            ddSection.DataBind();
            BindItemGrid();
        }
    }

    protected void lHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }
    protected void GridView2_DataBound(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in GridView2.Rows)
        {
            //DropDownList ddSection = gvRow.FindControl("ddSection") as DropDownList;
            //DropDownList ddAccHeadCr = gvRow.FindControl("ddAccHeadCr") as DropDownList;

            //Label lblSection = gvRow.FindControl("lblSection") as Label;
            //Label Label2c = gvRow.FindControl("Label2c") as Label;

            //if (lblSection.Text != "")
            //{
            //    ddSection.SelectedValue = lblSection.Text;
            //}
            //if (Label2c.Text != "")
            //{
            //    ddAccHeadCr.SelectedValue = Label2c.Text;
            //}
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow gvRow in GridView2.Rows)
            {
                Label lblTID = gvRow.FindControl("lblTID") as Label;
                DropDownList ddStatus = gvRow.FindControl("ddStatus") as DropDownList;
                TextBox txtInTime = gvRow.FindControl("txtInTime") as TextBox;
                TextBox lblOutTime = gvRow.FindControl("lblOutTime") as TextBox;

                SQLQuery.ExecNonQry(@"Update Attendance set InTime='" + txtInTime.Text + "', OutTime='" + lblOutTime.Text + "', Status='" + ddStatus.SelectedValue + "' WHERE sl='" + lblTID.Text + "'");
                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Attendance Info Updated Successfully.";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    protected void ddClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddSection.DataBind();
        BindItemGrid();
    }

    protected void ddSection_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindItemGrid();
    }


    private void BindItemGrid()
    {
        DataSet ds = new DataSet();
        try
        {
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT EmployeeInfoID FROM EmployeeInfo WHERE SectionID='" + ddSection.SelectedValue + "' AND IsActive=1  order by EmpSerial, EName");
            foreach (DataRow drx in dtx.Rows)
            {
                string sid = drx["EmployeeInfoID"].ToString();
                string isExist = SQLQuery.ReturnString(@"SELECT sl FROM Attendance WHERE EmployeeInfoID='" + sid + "' AND AttendanceDate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy/MM/dd") + "'  ");
                if (isExist == "")
                {
                    SQLQuery.ExecNonQry("Insert into Attendance (EmployeeInfoID, AttendanceDate) VALUES ('" + sid + "','" + Convert.ToDateTime(txtDate.Text).ToString("yyyy/MM/dd") + "') ");
                }
            }

            string query = @"SELECT b.sl, a.EmployeeInfoID, a.EmpSerial, a.EName,  
                                    b.InTime, b.OutTime, b.Status, b.InSMS, b.OutSMS  
                                    FROM [EmployeeInfo] a Join Attendance b on a.EmployeeInfoID=b.sl  
                                    WHERE a.SectionID='" + ddSection.SelectedValue + "' AND a.IsActive=1 AND b.AttendanceDate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy/MM/dd") + "' order by a.EmpSerial, a.EName";

            ds = SQLQuery.ReturnDataSet(query);
            if (ds.Tables[0].Rows.Count>0)
            {
                GridView2.DataSource = ds.Tables[0];
            GridView2.DataBind();
            }
            
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_warning");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }

        //return ds.Tables[0];
    }

    protected void btnDefault_Click(object sender, EventArgs e)
    {
        BindItemGrid();
    }
}
