using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Bank_Book : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtOpening.Text = DateTime.Now.ToString("dd/MM/yyyy");
            string date = Convert.ToDateTime(txtOpening.Text).AddDays(-1).ToString("yyyy-MM-dd");
            BankBalance(date);
        }
    }


    private void BankBalance(string dateFrom)
    {
        /*
        DataTable dt = SQLQuery.ReturnDataTable(
            @"SELECT        BankAccounts.ACID, BankAccounts.TypeID, BankAccounts.ACName, BankAccounts.ACNo, BankAccounts.BankID, BankAccounts.ZoneID, BankAccounts.Address, BankAccounts.Email, BankAccounts.ContactNo, 
                         BankAccounts.OpBalance, BankAccounts.ProjectID, BankAccounts.EntryBy, BankAccounts.EntryDate, ISNULL(SUM(Transactions.Dr), 0) - ISNULL(SUM(Transactions.Cr), 0) AS balance, Banks.BankName
FROM            BankAccounts INNER JOIN
                         Transactions ON BankAccounts.ACID = Transactions.HeadID INNER JOIN
                         Banks ON BankAccounts.BankID = Banks.BankId
WHERE        (BankAccounts.ACName NOT LIKE '%LATR%') AND (Transactions.TrType = 'Bank') AND (Transactions.TrDate <='" + dateFrom + @"')
GROUP BY BankAccounts.ACID, BankAccounts.TypeID, BankAccounts.ACName, BankAccounts.ACNo, BankAccounts.BankID, BankAccounts.ZoneID, BankAccounts.Address, BankAccounts.Email, BankAccounts.ContactNo, 
                         BankAccounts.OpBalance, BankAccounts.ProjectID, BankAccounts.EntryBy, BankAccounts.EntryDate, Banks.BankName
ORDER BY Banks.BankName, BankAccounts.ACName ");
        */

        DataTable dt=new DataTable();
        dt.Columns.Add("BankName");
        dt.Columns.Add("ACName");
        dt.Columns.Add("ACNo");
        dt.Columns.Add("Balance");


        DataTable dt2 = SQLQuery.ReturnDataTable("SELECT ACID, TypeID, ACName, ACNo, BankID, ZoneID, Address, Email, ContactNo, OpBalance, ProjectID, EntryBy, EntryDate FROM BankAccounts WHERE ACNAME NOT LIKE '%LATR%' Order by BankID ");
        foreach (DataRow drow in dt2.Rows)
        {
            DataRow dr = dt.NewRow();
            string bid = drow["ACID"].ToString();
            string balop = drow["OpBalance"].ToString();
            string Bid = drow["BankId"].ToString();
            string acNo = drow["ACNo"].ToString();
            decimal opBal = Convert.ToDecimal(balop);

            decimal currBal1 = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate <= '" + dateFrom + "') "));
            string headName = SQLQuery.ReturnString("Select BankName from Banks WHERE BankId='" + Bid + "'") +  "- " + acNo;

            dr["BankName"] = headName;
            dr["ACName"] = drow["ACName"];
            dr["ACNo"] = drow["ACNo"];
            dr["Balance"] =SQLQuery.FormatBDNumber(currBal1);
            dt.Rows.Add(dr);



        }

        GridView1.DataSource = dt;
        GridView1.DataBind();

        /*
        foreach (DataRow drow in dt.Rows)
        {
            string bid = drow["ACID"].ToString();
            string balop = drow["OpBalance"].ToString();
            string Bid = drow["BankId"].ToString();
            string acNo = drow["ACNo"].ToString();
            decimal opBal = Convert.ToDecimal(balop);

            decimal currBal1 = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate <= '" + dateFrom + "') "));
            
        }*/
    }


    protected void btnShow2_OnClick(object sender, EventArgs e)
    {
        string date = Convert.ToDateTime(txtOpening.Text).AddDays(-1).ToString("yyyy-MM-dd");
        BankBalance(date);
    }
}