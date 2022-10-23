using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using RunQuery;

public partial class app_Emp_Edit_Attn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate.Text = "01/" + DateTime.Now.ToString("MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ddClass.DataBind();
            ddSection.DataBind();
            ddStudent.DataBind();
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
        try
        {
            string sid = ddStudent.SelectedValue;
            DataSet ds = new DataSet();
            DateTime dtFrom = Convert.ToDateTime(txtDate.Text);
            DateTime dtTo = Convert.ToDateTime(txtDateTo.Text);

            while (dtFrom <= dtTo)
            {
                string isExist = SQLQuery.ReturnString(@"SELECT sl FROM Attendance WHERE StudentSl='" + sid + "' AND AttendanceDate='" + dtFrom.ToString("yyyy/MM/dd") + "'  ");
                if (isExist == "")
                {
                    SQLQuery.ExecNonQry("Insert into Attendance (StudentSl, AttendanceDate) VALUES ('" + sid + "','" + dtFrom.ToString("yyyy/MM/dd") + "') ");
                }
                dtFrom.AddDays(1);
            }

            string query = @"SELECT b.sl, b.AttendanceDate, a.StudentID, a.IDCardNo, a.StudentNameE, a.FatherNameE, a.RollNumber, a.PhoneMobile, a.Section,  
                                    b.InTime, b.OutTime, b.Status, b.InSMS, b.OutSMS FROM [Students] a Join Attendance b on a.sl=b.StudentSl  
                                    WHERE b.StudentSl='" + sid + "' AND a.IsActive=1 AND b.AttendanceDate>='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy/MM/dd") + "'  AND b.AttendanceDate<='" + Convert.ToDateTime(txtDateTo.Text).ToString("yyyy/MM/dd") + "' order by sl";

            ds = SQLQuery.ReturnDataSet(query);
            GridView2.DataSource = ds.Tables[0];
            GridView2.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_warning");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }

        //return ds.Tables[0];
    }

    protected void btnDefault_Click(object sender, EventArgs e)
    {
        BindItemGrid();
    }
}
