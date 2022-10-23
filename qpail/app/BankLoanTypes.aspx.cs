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

public partial class app_BankloanTypes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string isExist = "";
            if (btnSave.Text == "Save")
            {
                isExist = SQLQuery.ReturnString("SELECT AccountsHeadID FROM [LoanTypes] WHERE AccountsHeadID='" + ddAcheadId.SelectedValue + "'");
                if (isExist == "")
                {
                    InsertLoanType();
                    RunQuery.SQLQuery.ExecNonQry(@"INSERT INTO [dbo].[LoanTypes] ([LoanType],[AccountsHeadID],[AccountsHeadName])
VALUES  ('" + ddLoanType.SelectedValue + "','" + ddAcheadId.SelectedValue + "','" + ddAcheadId.SelectedItem.Text + "')");
                    txtLoanType.Text = "";
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Successfully Added new Loan Type";

                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_warn");
                    lblMsg.Text = "This head already exist";
                }
               
            }
            else if (txtLoanType.Text != "" && btnSave.Text == "Update")
            {
               
                isExist = SQLQuery.ReturnString("SELECT AccountsHeadID FROM [LoanTypes] WHERE AccountsHeadID='" + ddAcheadId.SelectedValue + "' AND LoanTypeId <>'" + LoanTypeIdHField.Value + "'");
                if (isExist == "")
                {
                    InsertLoanType();
                    RunQuery.SQLQuery.ExecNonQry("UPDATE [dbo].[LoanTypes]   SET [LoanType]='" + ddLoanType.SelectedValue +
                                             "', [AccountsHeadID]='" + ddAcheadId.SelectedValue +
                                             "', [AccountsHeadName]='" + ddAcheadId.SelectedItem.Text +
                                             "' WHERE LoanTypeId='" + LoanTypeIdHField.Value + "'");
                    txtLoanType.Text = "";
                    btnSave.Text = "Save";
                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Successfully updated Loan Type info";
                }
                else
                {
                    lblMsg.Attributes.Add("class", "xerp_warn");
                    lblMsg.Text = "This head already exist";
                }

            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_warn");
                lblMsg.Text = "Please Type the Loan Type Correctly";
            }
            GridView2.DataBind();

        }
        catch (Exception ex)
        {
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }



    protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            ddAcheadId.DataBind();
            int index = Convert.ToInt32(GridView2.SelectedIndex);
            Label loanTypeId = GridView2.Rows[index].FindControl("LabelLoanTypeId") as Label;
            LoanTypeIdHField.Value = loanTypeId.Text;
            if (loanTypeId != null) EditMode(loanTypeId.Text);

            lblMsg.Attributes.Add("class", "xerp_info");
            lblMsg.Text = "Edit mode activated ...";
         }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }

    private void EditMode(string loanTypeId)
    {
        SqlCommand cmd7 = new SqlCommand("SELECT LoanTypeId, LoanType, AccountsHeadID, AccountsHeadName  FROM LoanTypes WHERE LoanTypeId='" + loanTypeId + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            btnSave.Text = "Update";
            if (lbLLoanType.Text=="New")
            {
                ddLoanType.SelectedValue = dr["LoanType"].ToString();
            }
            else
            {
                txtLoanType.Text = SQLQuery.ReturnString(@"SELECT LoanTypes FROM BankLoanTypes WHERE Id ='" + dr["LoanType"].ToString() + "'");
            }
            
            ddAcheadId.SelectedValue = dr["AccountsHeadID"].ToString();

        }
        cmd7.Connection.Close();
    }

    protected void lbLLoanType_OnClick(object sender, EventArgs e)
    {
        if (lbLLoanType.Text == "New")
        {
            ddLoanType.Visible = false;
            txtLoanType.Visible = true;
            lbLLoanType.Text = "Cancel";
            txtLoanType.Focus();
        }
        else
        {
            ddLoanType.Visible = true;
            txtLoanType.Visible = false;
            lbLLoanType.Text = "New";
            ddLoanType.DataBind();
            ddLoanType.Focus();
        }

    }
    private void InsertLoanType()
    {
        string loanType = "";

        if (lbLLoanType.Text == "Cancel")
        {
            loanType = ddLoanType.SelectedValue;
            if (txtLoanType.Text != "" && lbLLoanType.Text == "Cancel")//Insert Loan Type
            {
                string loanTypeIdExist = RunQuery.SQLQuery.ReturnString("SELECT Id FROM BankLoanTypes WHERE LoanTypes='" + txtLoanType.Text.TrimStart().TrimEnd() + "'");
                if (btnSave.Text=="Save")
                {
                    if (loanTypeIdExist == "")
                    {
                        RunQuery.SQLQuery.ExecNonQry(@"INSERT INTO BankLoanTypes (LoanTypes, EntryBy) 
                                                   VALUES ('" + txtLoanType.Text.TrimStart().TrimEnd() + "', '" + Page.User.Identity.Name + "') ");
                        loanType = RunQuery.SQLQuery.ReturnString("SELECT MAX(Id) FROM BankLoanTypes");
                        ddLoanType.DataBind();
                        ddLoanType.SelectedValue = loanType;
                    }
                    else
                    {
                        ddLoanType.DataBind();
                        ddLoanType.SelectedValue = loanTypeIdExist;
                    }

                }
                else
                {
                    string loanTypeId= SQLQuery.ReturnString("SELECT LoanType FROM LoanTypes WHERE LoanTypeId= '" + LoanTypeIdHField.Value + "'");
                    SQLQuery.ExecNonQry(@"UPDATE BankLoanTypes SET LoanTypes='" + txtLoanType.Text.TrimStart().TrimEnd() + "' WHERE Id='" + loanTypeId + "'");
                    ddLoanType.DataBind();
                    ddLoanType.SelectedValue = loanTypeId;
                }
                
            }
            loanType = ddLoanType.SelectedValue;
        }
    }
}