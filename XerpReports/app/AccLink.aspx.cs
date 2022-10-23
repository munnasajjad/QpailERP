using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Oxford.app
{
    public partial class AccLink : System.Web.UI.Page
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
                DropDownList ddAccHeadDr = gvRow.FindControl("ddAccHeadDr") as DropDownList;
                DropDownList ddAccHeadCr = gvRow.FindControl("ddAccHeadCr") as DropDownList;

                Label Label2 = gvRow.FindControl("Label2") as Label;
                Label Label2c = gvRow.FindControl("Label2c") as Label;

                if (Label2.Text != "")
                {
                    ddAccHeadDr.SelectedValue = Label2.Text;
                }
                if (Label2c.Text != "")
                {
                    ddAccHeadCr.SelectedValue = Label2c.Text;
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow gvRow in GridView2.Rows)
                {
                    DropDownList ddAccHeadDr = gvRow.FindControl("ddAccHeadDr") as DropDownList;
                    DropDownList ddAccHeadCr = gvRow.FindControl("ddAccHeadCr") as DropDownList;

                    Label lblTID = gvRow.FindControl("lblTID") as Label;

                    RunQuery.SQLQuery.ExecNonQry(@"Update AccLink set HeadIdDr='" + ddAccHeadDr.SelectedValue + "', DrHeadName='" + ddAccHeadDr.SelectedItem.Text + "', HeadIdCr='" + ddAccHeadCr.SelectedValue + "', CrHeadName='" + ddAccHeadCr.SelectedItem.Text + "', UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "',  UpdateBy='" + Page.User.Identity.Name.ToString() + "' WHERE TID='" + lblTID.Text + "'");

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Accounts Link Updated Successfully.";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "ERROR: " + ex.Message.ToString();
            }
        }
    }
}