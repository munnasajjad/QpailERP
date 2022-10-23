using RunQuery;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_ShareholdersEquity : System.Web.UI.Page
{


    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ddFinYear.DataBind();
            MyClass();
            GridView1.DataBind();
        }
    }
    private void MyClass()
    {
        string isExist = SQLQuery.ReturnString("Select id from Shareholdersequitydb where FinYear='" + ddFinYear.SelectedValue + "'");
        if (isExist == "")
        {
            txtDate.Text = "";
            txtTextBox5.Text = "";
            txtSubAcc.Text = "";
            txtTextBox6.Text = "";
            txtTextBox2.Text = "";
            txtTextBox1.Text = "";
            txtTextBox3.Text = "";
            txtTextBox4.Text = "";
            txtTextBox7.Text = "";
            btnSave.Text = "Save";
        }
        else // Load Edit Mode
        {
            DataTable dt = SQLQuery.ReturnDataTable("SELECT FinYear,OPDate, OPBalance, NoOFShares, RevaluationOfNonCurrentAsset, TaxHolidayReserve, RetainedEarning, NetProfitForTheYear, DividentPaid, Total   FROM Shareholdersequitydb where FinYear ='" + ddFinYear.SelectedValue + "'");
            foreach (DataRow dtx in dt.Rows)
            {
                ddFinYear.SelectedValue = dtx["FinYear"].ToString();
                txtDate.Text = Convert.ToDateTime(dtx["OPDate"].ToString()).ToString("dd/MM/yyyy");
                txtTextBox5.Text = dtx["OPBalance"].ToString();
                txtSubAcc.Text = dtx["NoOFShares"].ToString();
                txtTextBox6.Text = dtx["RevaluationOfNonCurrentAsset"].ToString();
                txtTextBox2.Text = dtx["TaxHolidayReserve"].ToString();
                txtTextBox1.Text = dtx["RetainedEarning"].ToString();                
                txtTextBox4.Text = dtx["NetProfitForTheYear"].ToString();
                txtTextBox7.Text = dtx["DividentPaid"].ToString();
                txtTextBox3.Text = dtx["Total"].ToString();


            }
            btnSave.Text = "Update";
            lblMsg.Attributes.Add("class", "xerp_info");
            lblMsg.Text = "Edit mode activeted....";
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text == "Save")
            {
                if (txtTextBox5.Text != "")
                {
                    SQLQuery.ExecNonQry(@"INSERT INTO [dbo].[Shareholdersequitydb]
           ([FinYear],[OPDate],[OPBalance],[NoOFShares],[RevaluationOfNonCurrentAsset],[TaxHolidayReserve],[RetainedEarning],[Total],[NetProfitForTheYear],[DividentPaid])
     VALUES
           ('" + ddFinYear.SelectedValue + "','" + Convert.ToDateTime(txtDate.Text).ToString("yyy-MM-dd") + "','" +
                                        txtTextBox5.Text + "','" + txtSubAcc.Text + "','" + txtTextBox6.Text + "','" +
                                        txtTextBox2.Text + "','" + txtTextBox1.Text + "','" + txtTextBox3.Text + "','" +
                                        txtTextBox4.Text + "','" + txtTextBox7.Text + "')");
                }
                else
                {
                    lblMsg.Text = "Please fillup all mendatory fields...";
                }
            }
            
            else // UPDATE
            {
                SQLQuery.ExecNonQry(@"UPDATE [dbo].[Shareholdersequitydb]
                    SET [FinYear] = '"+ ddFinYear.SelectedValue + "',[OPDate] = '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "',[OPBalance] = '" + txtTextBox5.Text + "'      ,[NoOFShares] = '" + txtSubAcc.Text + "', [RevaluationOfNonCurrentAsset] ='"+ txtTextBox6.Text + "', [TaxHolidayReserve]= '"+ txtTextBox2.Text + "', [RetainedEarning]='"+ txtTextBox1.Text + "',[Total]='"+ txtTextBox3.Text + "',[NetProfitForTheYear]='"+ txtTextBox4.Text + "',[DividentPaid]='"+ txtTextBox7.Text + "' WHERE FinYear ='" + ddFinYear.SelectedValue + "'");
                //BindGrid();
                lblMsg.Attributes.Add("class", "xerp_success");
                lblMsg.Text = "Update successful!!!....";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.ToString();
            
        }
        finally
        {
            //ClearControls(Form);
            // GridView1.DataBind();
            //ddFinYear.DataBind();
            //txtTextBox7.Focus();
            MyClass();
            GridView1.DataBind();
        }
       
    }
    private void BindGrid()
    {
        GridView1.DataSource = SQLQuery.ReturnDataTable(@"SELECT OPDate, OPBalance, NoOFShares, RevaluationOfNonCurrentAsset, TaxHolidayReserve, RetainedEarning, NetProfitForTheYear, DividentPaid, Total   FROM Shareholdersequitydb");
        GridView1.DataBind();
    }



    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox) { (Parent as TextBox).Text = string.Empty; } else { foreach (Control c in Parent.Controls) ClearControls(c); }
    }
   
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;

        SQLQuery.ExecNonQry("DELETE FROM Shareholdersequitydb WHERE (id ='" + lblId.Text + "')");
        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Shareholders Equity data Circle deleted successfully";
        

    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label labelID = GridView1.Rows[index].FindControl("Label1") as Label;
            
            DataTable dt = SQLQuery.ReturnDataTable("SELECT FinYear,OPDate, OPBalance, NoOFShares, RevaluationOfNonCurrentAsset, TaxHolidayReserve, RetainedEarning, NetProfitForTheYear, DividentPaid, Total   FROM Shareholdersequitydb where id='" + labelID.Text + "'");
            foreach (DataRow dtx in dt.Rows)
            {
                ddFinYear.SelectedValue = dtx["FinYear"].ToString();
                txtDate.Text =Convert.ToDateTime(dtx["OPDate"].ToString()).ToString("dd/MM/yyyy");                
                txtTextBox5.Text = dtx["OPBalance"].ToString();
                txtSubAcc.Text   = dtx["NoOFShares"].ToString();
                txtTextBox6.Text = dtx["RevaluationOfNonCurrentAsset"].ToString();
                txtTextBox2.Text = dtx["TaxHolidayReserve"].ToString();
                txtTextBox1.Text = dtx["RetainedEarning"].ToString();
                txtTextBox3.Text = dtx["NetProfitForTheYear"].ToString();
                txtTextBox4.Text = dtx["DividentPaid"].ToString();
                txtTextBox7.Text = dtx["Total"].ToString();

            }
            btnSave.Text = "Update";
            lblMsg.Attributes.Add("class", "xerp_info");
            lblMsg.Text = "Edit mode activeted....";
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }
    private void ClearForm()
    {
        ddFinYear.Text = "";
        txtDate.Text = "";
        txtTextBox5.Text = "";
        txtSubAcc.Text = "";
        txtTextBox6.Text = "";
        txtTextBox2.Text = "";
        txtTextBox1.Text = "";
        txtTextBox3.Text = "";
        txtTextBox4.Text = "";
        txtTextBox7.Text = "";
        GridView1.DataBind();
        //upnl.Update();
        
    }


    protected void ddFinYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        MyClass();
    }
}
