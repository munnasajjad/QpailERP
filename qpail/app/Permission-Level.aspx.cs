using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Permission_Level : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //ddMonth.DataBind();
            //txtYear.Text = DateTime.Now.ToString("yyyy");
            //ddClass.DataBind();
            //ddSection.DataBind();
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
                Label LevelID = gvRow.FindControl("LevelID") as Label;
                TextBox CanInsert = gvRow.FindControl("CanInsert") as TextBox;
                TextBox CanRead = gvRow.FindControl("CanRead") as TextBox;
                TextBox CanUpdate = gvRow.FindControl("CanUpdate") as TextBox;
                TextBox CanDelete = gvRow.FindControl("CanDelete") as TextBox;

                SQLQuery.Empty2Zero(CanInsert); SQLQuery.Empty2Zero(CanRead);
                SQLQuery.Empty2Zero(CanUpdate); SQLQuery.Empty2Zero(CanDelete);

                SQLQuery.ExecNonQry(@"Update UserLevel set CanRead='" + CanRead.Text + "', CanInsert='" + CanInsert.Text + "', CanUpdate='" + CanUpdate.Text + "', CanDelete='" + CanDelete.Text + "' WHERE LevelID='" + LevelID.Text + "'");

                //string invNo = "AttendanceMonth-" + "-" + lblTID.Text;
                //InsertTransaction(invNo, EmployeeInfoID.Text, SQLQuery.ReturnString("Select EName from EmployeeInfo where EmployeeInfoID='" + EmployeeInfoID.Text + "'"), lblSalary.Text);

            }
            lblMsg.Attributes.Add("class", "xerp_success");
            lblMsg.Text = "Security Info Updated Successfully.";
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }

    private void InsertTransaction(string invNo, string eid, string ename, string amount)
    {
        //SQLQuery.ExecNonQry("Delete Transactions where InvoiceNo='" + invNo + "'");


        //string expDate = Convert.ToDateTime(txtYear.Text + "-" + ddMonth.SelectedValue + "-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
        //string desc = "Salary for the Month" + ddMonth.SelectedItem.Text + "-" + txtYear.Text;

        //Accounting.VoucherEntry.TransactionEntry(invNo, "Salary", expDate, eid, ename, "Employee", desc, "0", amount, Page.User.Identity.Name.ToString());

    }

    protected void ddClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddSection.DataBind();
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
        {/*
            //chk if month already finalized
            string mName = ddMonth.SelectedItem.Text + "-" + txtYear.Text;
            string 
            = SQLQuery.ReturnString("select Finalize from Months where MonthName='" + mName + "'");
            if (isExist == "")
            {
                SQLQuery.ExecNonQry("INSERT INTO Months (MonthName, Finalize) VALUES ('" + mName + "', 0)");
            }
            else if (isExist == "0")
            {
                DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT EmployeeInfoID, Salary FROM EmployeeInfo WHERE SectionID='" +
                                                      ddSection.SelectedValue + "' AND IsActive=1 AND Type='Regular' order by EmpSerial, EName");
                foreach (DataRow drx in dtx.Rows)
                {
                    string sid = drx["EmployeeInfoID"].ToString();
                    string salary = drx["Salary"].ToString();
                    isExist = SQLQuery.ReturnString(@"SELECT sl FROM AttendanceMonth WHERE EmployeeInfoID='" + sid +
                                              "' AND Month='" + ddMonth.SelectedValue + "' AND Year='" + txtYear.Text + "'  ");
                    if (isExist == "")
                    {
                        SQLQuery.ExecNonQry("Insert into AttendanceMonth (EmployeeInfoID, Month, Year, Salary) VALUES ('" + sid +
                                            "','" + ddMonth.SelectedValue + "','" + txtYear.Text + "','" + salary + "') ");
                    }
                }
            }

            txtWorkDays.Text = SQLQuery.ReturnString(@"SELECT TOP(1) WorkDays FROM AttendanceMonth WHERE Section='" + ddSection.SelectedValue + "' AND Month='" + ddMonth.SelectedValue + "' AND Year='" + txtYear.Text + "' Order by sl desc ");
            if (txtWorkDays.Text == "")
            {
                txtWorkDays.Text = SQLQuery.ReturnString(@"SELECT TOP(1) WorkDays FROM AttendanceMonth WHERE Month='" + ddMonth.SelectedValue + "' AND Year='" + txtYear.Text + "'  Order by sl desc ");
            }
            */
            string query = @"SELECT LevelID, LevelName, CanRead, CanInsert, CanUpdate, CanDelete 
                                    FROM [UserLevel]  order by LevelID";

            ds = SQLQuery.ReturnDataSet(query);
            if (ds.Tables[0].Rows.Count > 0)
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
