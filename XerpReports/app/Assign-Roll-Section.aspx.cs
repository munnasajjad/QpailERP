using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Oxford.app
{
    public partial class Assign_Roll_Section : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }
        protected void GridView2_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in GridView2.Rows)
            {
                DropDownList ddSection = gvRow.FindControl("ddSection") as DropDownList;
                //DropDownList ddAccHeadCr = gvRow.FindControl("ddAccHeadCr") as DropDownList;

                Label lblSection = gvRow.FindControl("lblSection") as Label;
                //Label Label2c = gvRow.FindControl("Label2c") as Label;

                if (lblSection.Text != "")
                {
                    ddSection.SelectedValue = lblSection.Text;
                }
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
                    DropDownList ddSection = gvRow.FindControl("ddSection") as DropDownList;
                    Label lblTID = gvRow.FindControl("lblTID") as Label;
                    TextBox txtRollNo = gvRow.FindControl("txtRollNo") as TextBox;

                    RunQuery.SQLQuery.ExecNonQry(@"Update Students set RollNumber='" + txtRollNo.Text + "', Section='" + ddSection.SelectedValue + "' WHERE sl='" + lblTID.Text + "'");

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Student Info Updated Successfully.";
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
            GridView2.DataBind();
        }
    }
}