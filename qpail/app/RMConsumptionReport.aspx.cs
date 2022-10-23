using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_RMConsumptionReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtdateFrom.Text = "01/" + DateTime.Now.AddMonths(0).ToString("MM/yyyy");
            txtdateTo.Text = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01").AddDays(-1).ToString("dd/MM/yyyy");

            //GenerateCoA();
        }
    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        //GenerateCoA();
        string dt1 = Convert.ToDateTime(txtdateFrom.Text).ToString("yyyy-MM-dd");
        string dt2 = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");
        string urlx = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "XerpReports/RMConsumptionReport.aspx?dateFrom=" + dt1 + "&dateTo=" + dt2;
        if1.Attributes.Add("src", urlx);
    }

    private void GenerateCoA()
    {
        string body = "<table class='table-bordered table-striped'><tr><th>A/C Group</th><th>Sub A/C</th><th>Control A/C</th><th>A/C Head ID</th><th>Accounts Head</th><th>Balance (Dr.)</th><th>Balance (Cr.)</th></tr>  ";
        string acGroupPrev = "", acSubPrev = "", acControlPrev = "", acHeadPrev = "";
        string acGroup = "", acSub = "", acControl = "", acHead = "";
        string acGroupTxt = "", acSubTxt = "", acControlTxt = "", acHeadTxt = "";
        decimal grandTotalDr = 0; decimal grandTotalCr = 0;

        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT sl, GroupID, GroupName, ProjectID FROM AccountGroup Order by sl");

        foreach (DataRow drx in dtx.Rows)
        {
            decimal totalDr = 0; decimal totalCr = 0;
            string acGroupID = drx["GroupID"].ToString();
            acGroup = drx["GroupName"].ToString();

            int subCount = Convert.ToInt32(SQLQuery.ReturnString(@"SELECT COUNT(AccountsID) FROM Accounts WHERE GroupID='" + acGroupID + "'"));
            DataTable dtx2 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT sl, GroupID, AccountsID, AccountsName, ProjectID FROM Accounts WHERE GroupID='" + acGroupID + "' Order by sl");

            foreach (DataRow drx2 in dtx2.Rows)
            {
                string acSubID = drx2["AccountsID"].ToString();
                acSub = drx2["AccountsName"].ToString();

                //int controlCount = Convert.ToInt32(SQLQuery.ReturnString(@"SELECT COUNT(ControlAccountsID) FROM ControlAccount WHERE AccountsID='" + acSubID + "'"));
                //if (controlCount == 0)
                //{
                //    body += "<tr>" + comparePrevValue(acGroup, acGroupPrev) + comparePrevValue(acSub, acSubPrev) + "<td> </td><td> </td><td> </td><td> </td><td> </td></tr>";
                //}

                DataTable dtx3 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT sl, ControlAccountsID, ControlAccountsName, ProjectID FROM ControlAccount  WHERE AccountsID='" + acSubID + "'  Order by sl");
                foreach (DataRow drx3 in dtx3.Rows)
                {
                    string acControlID = drx3["ControlAccountsID"].ToString();
                    acControl = drx3["ControlAccountsName"].ToString();

                    //int headCount = Convert.ToInt32(SQLQuery.ReturnString(@"SELECT COUNT(AccountsHeadID) FROM HeadSetup WHERE ControlAccountsID='" + acControlID + "'"));
                    //if (headCount == 0)
                    //{
                    //    body += "<tr>" + comparePrevValue(acGroup, acGroupPrev) + comparePrevValue(acSub, acSubPrev) + comparePrevValue(acControl, acControlPrev) + "<td> </td><td> </td><td> </td><td> </td></tr>";
                    //    acGroupPrev = acGroup;
                    //    acSubPrev = acSub;
                    //    acControlPrev = acControl;
                    //}
                    //else
                    //{
                    DataTable dtx4 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT EntryID, AccountsHeadID, AccountsHeadName, OpBalDr, OpBalCr, ProjectID FROM HeadSetup WHERE ControlAccountsID='" + acControlID + "'  Order by EntryID");

                    foreach (DataRow drx4 in dtx4.Rows)
                    {
                        string acHeadID = drx4["AccountsHeadID"].ToString();
                        acHead = drx4["AccountsHeadName"].ToString();

                        /*
                        if (acHeadID == "010101002")//Banks
                        {
                            //body += "<tr>" + comparePrevValue(acGroup, acGroupPrev) + comparePrevValue(acSub, acSubPrev) + comparePrevValue(acControl, acControlPrev) + "<td>" + acHeadID + "</td><td>" + acHead + "</td><td> </td><td> </td></tr>";
                            DataTable dtx4x = SQLQuery.ReturnDataTable(@"SELECT ACID, (Select [BankName] FROM [Banks] where [BankId]=a.BankID) +' - '+ACNo +' - '+ACName AS Bank from BankAccounts a ORDER BY [ACName]");

                            foreach (DataRow drx4x in dtx4x.Rows)
                            {
                                string accID = drx4x["ACID"].ToString();
                                string acName = drx4x["Bank"].ToString();
                                string balance = SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions]  where TrType = 'Bank'  AND HeadID ='" + accID + "'");
                                body += "<tr>" + comparePrevValue(acGroup, acGroupPrev) + comparePrevValue(acSub, acSubPrev) + comparePrevValue(acControl, acControlPrev) + "<td>" + "010101002" + "</td><td>" + acName + "</td><td class='a-right'>" + SQLQuery.FormatBDNumber(balance) + " </td><td>0.00</td></tr>";

                            }
                        }
                        else
                        {*/
                        decimal balDr = Convert.ToDecimal(HeadBalance(acHeadID, "Dr"));
                        decimal balCr = Convert.ToDecimal(HeadBalance(acHeadID, "Cr"));

                        if (balDr > balCr)
                        {
                            balDr = balDr - balCr;
                            balCr = 0;
                        }
                        else if (balCr > balDr)
                        {
                            balCr = balCr - balDr;
                            balDr = 0;
                        }

                        if (balDr > 0 || balCr > 0)
                        {
                            body += "<tr>" + comparePrevValue(acGroup, acGroupPrev) + comparePrevValue(acSub, acSubPrev) + comparePrevValue(acControl, acControlPrev) + "<td>" + acHeadID + "</td><td>" + acHead + "</td><td class='a-right'>" + SQLQuery.FormatBDNumber(balDr) + " </td><td>" + SQLQuery.FormatBDNumber(balCr) + "</td></tr>";
                            totalDr += Convert.ToDecimal(HeadBalance(acHeadID, "Dr"));
                            totalCr += Convert.ToDecimal(HeadBalance(acHeadID, "Cr"));
                        }
                        //}
                        acGroupPrev = acGroup;
                        acSubPrev = acSub;
                        acControlPrev = acControl;

                    }
                    //}
                    acControlPrev = acControl;
                }
                acSubPrev = acSub;
            }
            acGroupPrev = acGroup;
            body += "<tr style='background-color: #D3D3D3;border-bottom: 6px solid #0071C8;'><td></td><td></td><td></td><td></td><td class='a-right'><b>Total " + acGroup + ":</b></td><td class='a-right'>" + SQLQuery.FormatBDNumber(totalDr) + " </td><td class='a-right'>" + SQLQuery.FormatBDNumber(totalCr) + "</td></tr>";
            grandTotalDr += totalDr; grandTotalCr += totalCr;
        }
        body += "<tr style='background-color: #0071C8;border-bottom: 6px solid #FF0; Color:white'><td></td><td></td><td></td><td></td><td class='a-right'><b>Grand Total :</b></td><td class='a-right'>" + SQLQuery.FormatBDNumber(grandTotalDr) + " </td><td class='a-right'>" + SQLQuery.FormatBDNumber(grandTotalCr) + "</td></tr>";

        body += "</table>";
        ltrBody.Text = body;
    }

    private string comparePrevValue(string newValue, string prevValue)
    {
        string result = "<td><b>" + newValue + "</b></td>";
        if (newValue == prevValue)
        {
            result = "<td style='border-top-style: hidden;'> </td>";
        }
        return result;
    }
    //private string fifthLevelHeads(string tableName, string iDColumn, string nameColumn, string balanceColumn)
    //{
    //    string result = "<td><b>" + newValue + "</b></td>";
    //    if (newValue == prevValue)
    //    {
    //        result = "<td style='border-top-style: hidden;'> </td>";
    //    }
    //    return result;
    //}


    private string HeadBalance(string cid, string btype)
    {
        string dateTo = Convert.ToDateTime(txtdateTo.Text).ToString("yyyy-MM-dd");
        decimal returnValue = 0;

        if (btype == "Dr")
        {
            returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE (AccountsHeadID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
            returnValue += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE (AccountsHeadID = '" + cid + "') and ISApproved ='A' and EntryDate<='" + dateTo + "'"));
        }
        else
        {
            returnValue = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0)  FROM HeadSetup WHERE (AccountsHeadID = '" + cid + "') AND OpDate <='" + dateTo + "'"));
            returnValue += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0)  FROM VoucherDetails WHERE (AccountsHeadID = '" + cid + "') and ISApproved ='A' and EntryDate<='" + dateTo + "'"));
        }
        return returnValue.ToString();
    }


    /*
        private void GenerateExpenses()
        {
            SqlCommand cmd2 = new SqlCommand("SELECT DISTINCT AccountsHeadName, SUM(VoucherDR) - SUM(VoucherCR) AS Expense_Amount FROM VoucherDetails WHERE (AccountsHeadID IN (SELECT AccountsHeadID FROM HeadSetup WHERE (GroupID = '04'))) and ISApproved ='A' and EntryDate>=@dateFrom and EntryDate<=@dateTo GROUP BY AccountsHeadName", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = txtDateF.Text;
            cmd2.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text);
            //cmd2.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateT.Text).AddDays(+1);
            //cmd2.Parameters.Add("@SalesTo", SqlDbType.VarChar).Value = DropDownList1.SelectedValue;

            cmd2.Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            cmd2.Connection.Close();
            DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            da.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();

        } * */
}