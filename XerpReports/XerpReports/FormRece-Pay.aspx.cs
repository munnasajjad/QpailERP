using CrystalDecisions.CrystalReports.Engine;
using RunQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Oxford.XerpReports
{
    public partial class FormRece_Pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
                LoadGridData();
            //}
            //catch (Exception ex)
            //{
            //    Response.Write(ex.ToString());
            //}
        }

        ReportDocument rpt = new ReportDocument();
        private void LoadGridData()
        {
            //decimal total2 = (decimal)0.0;
            //decimal qty2 = (decimal)0.0;
            //decimal vat2 = (decimal)0.0;
            //decimal TotalSales2 = (decimal)0.0;
            //decimal TotalWeight2 = (decimal)0.0;

            string lName = Page.User.Identity.Name.ToString();
            string prjID = "1";
            string dateForm = Convert.ToString(Request.QueryString["y1_OpDate"]);
            string dateTo = Convert.ToString(Request.QueryString["y1_ClDate"]);
            
            
            //string opYear = Convert.ToString(Request.QueryString["OpYear"]);
            //string clYear = Convert.ToString(Request.QueryString["ClYear"]);
            //string y1 = Convert.ToString(Request.QueryString["y1"]);
            //string y2 = Convert.ToString(Request.QueryString["y2"]);



           // Search(dateForm, dateTo);
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT ReceiptType, ReceiptsHead, RYear1, RYear2, PaymentsType, PaymentsHead, PYear1, PYear2 FROM ReceiptPayment 
                       Order by Id");

            DataTableReader dr2 = dtx.CreateDataReader();

            XerpDataSet dsx = new XerpDataSet();
            dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.ReceiptPayment);


            rpt.Load(Server.MapPath("CrptRecePay1.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateForm).ToString("dd MMMM yyyy") + " To " + Convert.ToDateTime(dateTo).ToString("dd MMMM yyyy");
            //string datefield = "opDate " + Convert.ToDateTime(opDate).ToString(" dd/MM/yyyy") + " clDate " + Convert.ToDateTime(clDate).ToString(" dd/MM/yyyy");
            //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx); 
            SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@opYear", dateForm);
            rpt.SetParameterValue("@clYear", dateTo);
            //rpt.SetParameterValue("@dateFrom", clDate2);
            //rpt.SetParameterValue("@clDate2", clDate2);
            
            //CrystalReportViewer1.ReportSource = rpt;
            rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, false, "CrptRecePay1.rpt");

        }

        private string Search(string dateFrom, string dateTo)
        {
            try
            {
                SQLQuery.ExecNonQry("Delete ReceiptPayment");
                string acGroupPrev = "", acSubPrev = "", acControlPrev = "", acHeadPrev = "";
                string acType = "", acSub = "", acControl = "", acHead = "";
                string acGroupTxt = "", acSubTxt = "", acControlTxt = "", acHeadTxt = "";
                decimal grandTotalDr = 0; decimal grandTotalCr = 0;

                Opclbalance("Opening balances:", "010101001", "Dr", dateFrom, dateTo);//Cash
                Opclbalance("Opening balances:", "010101002", "Dr", dateFrom, dateTo);//Bank
                //RHeadBalance("", "010101002", "Dr", dateFrom, dateTo);
                BankBalance(dateFrom, dateTo, "Dr");
                ReceiptfromCustomer(dateFrom, dateTo);

                //SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
                // VALUES ('Interest Received','a) Bank Deposits','" + 0 + "','" + 0 + "')");

                //SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
                //  VALUES ('','b) Loans, Advances','" + 0 + "','" + 0 + "')");

                RHeadBalance("Interest Received", "a) Bank Deposits", "Dr", dateFrom, dateTo);
                RHeadBalance(" ", "b) Loans, Advances", "Dr", dateFrom, dateTo);
                ////Short Term Loan
                DataTable dt = SQLQuery.ReturnDataTable("SELECT EntryID, GroupID, AccountsID, ControlAccountsID, AccountsHeadID, AccountsHeadName, OpBalDr, OpBalCr, QtyDr, QtyCr, OpPcsDr, OpPcsCr, Rate, AccountsOpeningBalance, ProjectID, OpDate, ServerDateTime,Emark, IsActive FROM HeadSetup WHERE (ControlAccountsID = '020101')OR(ControlAccountsID = '020201')OR(ControlAccountsID = '020202') AND (OpDate >='" + dateFrom + "') AND (OpDate <='" + dateTo + "')");
                foreach (DataRow drow in dt.Rows)
                {
                    Repaymenttype("Loans and Borrowings", drow["AccountsHeadID"].ToString(), "Dr", dateFrom, dateTo);

                }

                ////Misc. Receipts
                dt = SQLQuery.ReturnDataTable("SELECT EntryID, GroupID, AccountsID, ControlAccountsID, AccountsHeadID, AccountsHeadName, OpBalDr, OpBalCr, QtyDr, QtyCr, OpPcsDr, OpPcsCr, Rate, AccountsOpeningBalance, ProjectID, OpDate, ServerDateTime,Emark, IsActive FROM HeadSetup WHERE(ControlAccountsID = '030201') AND OpDate>= '" + dateFrom + "' AND OpDate<= '" + dateTo + "' ");
                foreach (DataRow drow in dt.Rows)
                {
                    Repaymenttype("Misc. Receipts", drow["AccountsHeadID"].ToString(), "Dr", dateFrom, dateTo);
                }


                //////Credit part
                ////Expenses
                dt = SQLQuery.ReturnDataTable("SELECT EntryID, GroupID, AccountsID, ControlAccountsID, AccountsHeadID, AccountsHeadName, OpBalDr, OpBalCr, QtyDr, QtyCr, OpPcsDr, OpPcsCr, Rate, AccountsOpeningBalance, ProjectID, OpDate, ServerDateTime,Emark, IsActive FROM HeadSetup WHERE(ControlAccountsID = '040101')OR(ControlAccountsID = '040202') AND OpDate >='" + dateFrom + "' AND OpDate <='" + dateTo + "'");
                foreach (DataRow drow in dt.Rows)
                {
                    Repaymenttype("Expenses", drow["AccountsHeadID"].ToString(), "Cr", dateFrom, dateTo);

                }
                // Expenditure on Fixed Assets and capital work-in-progress
                // ExpenditureonFixedAsset(dateTo, clDate2);


                /* SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([PaymentsType],[PaymentsHead],[PYear1],[PYear2]) 
                                 VALUES ('Expenditure on Fixed Assets and capital work-in-progress','a)  Purchase of Fixed Assets','" + 0 + "','" + 0 + "')");*/
                RHeadBalance("Expenditure on Fixed Assets and capital work-in-progress", "a)  Purchase of Fixed Assets", "Cr", dateFrom, dateTo);
                /*
                SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([PaymentsType],[PaymentsHead],[PYear1],[PYear2]) 
                                VALUES ('','b) Expenditure on Work in  Progress','" + 0 + "','" + 0 + "')");*/

                RHeadBalance("", "b) Expenditure on Work in  Progress", "Cr", dateFrom, dateTo);

                // RHeadBalance("Expenditure on Fixed Assets and capital work-in-progress", "010101002", "Cr", dateTo, clDate2);


               // RHeadBalance("Repayment of Loans/Borrowings", "010101002", "Cr", dateFrom, dateTo);

                dt = SQLQuery.ReturnDataTable("SELECT EntryID, GroupID, AccountsID, ControlAccountsID, AccountsHeadID, AccountsHeadName, OpBalDr, OpBalCr, QtyDr, QtyCr, OpPcsDr, OpPcsCr, Rate, AccountsOpeningBalance, ProjectID, OpDate, ServerDateTime,Emark, IsActive FROM HeadSetup WHERE(ControlAccountsID = '020101')OR(ControlAccountsID = '020201')OR(ControlAccountsID = '020202') AND (OpDate >='" + dateFrom + "') AND (OpDate <='" + dateTo + "')");
                foreach (DataRow drow in dt.Rows)
                {
                    Repaymenttype("Repayment of Loans/Borrowings", drow["AccountsHeadID"].ToString(), "Cr", dateFrom, dateTo);
                }

                //RHeadBalance("Deposits and Advances", "010101002", "Cr", dateTo, clDate2);
                dt = SQLQuery.ReturnDataTable("SELECT EntryID, GroupID, AccountsID, ControlAccountsID, AccountsHeadID, AccountsHeadName, OpBalDr, OpBalCr, QtyDr, QtyCr, OpPcsDr, OpPcsCr, Rate, AccountsOpeningBalance, ProjectID, OpDate, ServerDateTime,Emark, IsActive FROM HeadSetup WHERE(ControlAccountsID = '010105')OR(ControlAccountsID = '010102')OR(ControlAccountsID = '010131')OR(ControlAccountsID = '010160')OR(ControlAccountsID = '010124')OR(ControlAccountsID = '010128')OR(ControlAccountsID = '010119')OR(ControlAccountsID = '010118')OR(ControlAccountsID = '010126')OR(ControlAccountsID = '010132')OR(ControlAccountsID = '010120')OR(ControlAccountsID = '010129')OR(ControlAccountsID = '010117')OR(ControlAccountsID = '010121')OR(ControlAccountsID = '010122')OR(ControlAccountsID = '010127')OR(ControlAccountsID = '010130')OR(ControlAccountsID = '010116')OR(ControlAccountsID = '010125')OR(ControlAccountsID = '010123') AND (OpDate >='" + dateFrom + "') AND (OpDate <='" + dateTo + "')");
                foreach (DataRow drow in dt.Rows)
                {
                    Repaymenttype("Deposits and Advances", drow["AccountsHeadID"].ToString(), "Cr", dateFrom, dateTo);
                }

                Opclbalance("Closing Balances", "010101001", "Cr", dateFrom, dateTo);
                Opclbalance("Closing Balances", "010101002", "Cr", dateFrom, dateTo);
                //PHeadBalance("", "010101002", "Cr", dateTo, clDate2);
                BankBalance(dateFrom, dateTo, "Cr");


            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            return "";
        }

        private void Opclbalance(string rType, string headId, string btype, string dateFrom, string dateTo)
        {
            decimal returnValue = 0;
            //if (headId == "010101001")
            //{
            //    headId = "010101001";
            //}
            if (btype == "Dr")
            {
                
                decimal op1Bal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(OpBalDr),0) - isnull(sum(OpBalCr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID] = '" + headId + "')"));
                decimal preBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(VoucherDR),0) - isnull(sum(VoucherCR),0) FROM [VoucherDetails] WHERE ([AccountsHeadID] = '" + headId + "') and   EntryDate < '" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "'  and ISApproved='A'"));
                decimal currBal = op1Bal + preBal;
               // if (opBal != 0 || y1Bal != 0 || y2Bal != 0)
                {

                    string headName = SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup WHERE AccountsHeadID='" + headId + "'");
                    SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
                                VALUES ('" + rType + "','" + headName.Replace("'", "''") + "','" + currBal + "','" + currBal + "')");
                }
            }
            else
            {

                decimal op1Bal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(OpBalDr),0) - isnull(sum(OpBalCr),0)  FROM [HeadSetup] WHERE ([AccountsHeadID] = '" + headId + "')"));
                decimal preBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(VoucherDR),0) - isnull(sum(VoucherCR),0) FROM [VoucherDetails] WHERE ([AccountsHeadID] = '" + headId + "') and   EntryDate <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "'  and ISApproved='A'"));
                decimal currBal = op1Bal + preBal;
                // if (opBal != 0 || y1Bal != 0 || y2Bal != 0)
                {
                    string headName = SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup WHERE AccountsHeadID='" + headId + "'");

                    string isExist = SQLQuery.ReturnString("Select MIN(id) from ReceiptPayment WHERE PaymentsHead IS NULL");
                    if (isExist != "")
                    {
                        SQLQuery.ExecNonQry(@"UPDATE [ReceiptPayment] SET [PaymentsType]='" + rType + "',[PaymentsHead]='" + headName.Replace("'", "''") + "',[PYear1]='" + currBal + "',[PYear2]='" + currBal + "' WHERE id='" + isExist + "' ");
                    }
                    else
                    {
                        SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([PaymentsType],[PaymentsHead],[PYear1],[PYear2]) 
                                VALUES ('" + rType + "','" + headName.Replace("'", "''") + "','" + currBal + "','" + currBal + "')");
                    }
                }
            }

        }

        private void RHeadBalance(string rType, string headId, string btype, string dateFrom, string dateTo)
        {
            decimal returnValue = 0;
            //if (headId== "010101001")
            //{
            //    headId = "010101001";
            //}
            if (btype == "Dr")
            {
                decimal opBal = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0)-ISNULL(SUM(OpBalCr),0) FROM HeadSetup WHERE (AccountsHeadID = '" + headId + "') AND OpDate>= '"+ dateFrom + "' AND OpDate<= '"+ dateTo + "' "));
                decimal y1Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0)-ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A' AND VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));
                decimal y2Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0)-ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A' AND VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));
                if (opBal != 0 || y1Bal != 0 || y2Bal != 0)
                {

                    string headName = SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup WHERE AccountsHeadID='" + headId + "'");
                    SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
                                VALUES ('" + rType + "','" + headName.Replace("'", "''") + "','" + y1Bal + "','" + y2Bal + "')");
                }
            }
            else
            {
                decimal opBal = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0)-ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE (AccountsHeadID = '" + headId + "') AND OpDate>= '" + dateFrom + "' AND OpDate<= '" + dateTo + "' "));
                decimal y1Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0)-ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A'  and VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));
                decimal y2Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0)-ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A'  and VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));
                if (opBal != 0 || y1Bal != 0 || y2Bal != 0)
                {
                    string headName = SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup WHERE AccountsHeadID='" + headId + "'");

                    string isExist = SQLQuery.ReturnString("Select MIN(id) from ReceiptPayment WHERE PaymentsHead IS NULL");
                    if (isExist != "")
                    {
                        SQLQuery.ExecNonQry(@"UPDATE [ReceiptPayment] SET [PaymentsType]='" + rType + "',[PaymentsHead]='" + headName.Replace("'", "''") + "',[PYear1]='" + y1Bal + "',[PYear2]='" + y2Bal + "' WHERE id='" + isExist + "' ");
                    }
                    else
                    {
                        SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([PaymentsType],[PaymentsHead],[PYear1],[PYear2]) 
                                VALUES ('" + rType + "','" + headName.Replace("'", "''") + "','" + y1Bal + "','" + y2Bal + "')");
                    }
                }
            }

        }
        private void Repaymenttype(string rType, string headId, string btype, string dateFrom, string dateTo)
        {
            decimal returnValue = 0;
            //if (headId== "010101001")
            //{
            //    headId = "010101001";
            //}
            if (btype == "Dr")
            {
                decimal opBal = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalCr),0)-ISNULL(SUM(OpBalDr),0) FROM HeadSetup WHERE (AccountsHeadID = '" + headId + "') AND OpDate>= '" + dateFrom + "' AND OpDate<= '" + dateTo + "' "));
                decimal y1Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A'  and VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));
                decimal y2Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherCR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A'  and VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));
                
                if (opBal != 0 || y1Bal != 0 || y2Bal != 0)
                {

                    string headName = SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup WHERE AccountsHeadID='" + headId + "'");
                    SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
                                VALUES ('" + rType + "','" + headName.Replace("'", "''") + "','" + y1Bal + "','" + y2Bal + "')");
                }
            }
            else
            {
                decimal opBal = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(OpBalDr),0)-ISNULL(SUM(OpBalCr),0) FROM HeadSetup WHERE (AccountsHeadID = '" + headId + "') AND OpDate>= '" + dateFrom + "' AND OpDate<= '" + dateTo + "' "));
                decimal y1Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A' AND VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));
                decimal y2Bal = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(VoucherDR),0) FROM VoucherDetails WHERE AccountsHeadID = '" + headId + "' and ISApproved ='A' AND VoucherNo IN (SELECT VoucherNo FROM  VoucherMaster WHERE (VoucherDate >='" + dateFrom + "') AND VoucherDate <='" + dateTo + "')"));
                if (opBal != 0 || y1Bal != 0 || y2Bal != 0)
                {
                    string headName = SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup WHERE AccountsHeadID='" + headId + "'");

                    string isExist = SQLQuery.ReturnString("Select MIN(id) from ReceiptPayment WHERE PaymentsHead IS NULL");
                    if (isExist != "")
                    {
                        SQLQuery.ExecNonQry(@"UPDATE [ReceiptPayment] SET [PaymentsType]='" + rType + "',[PaymentsHead]='" + headName.Replace("'", "''") + "',[PYear1]='" + y1Bal + "',[PYear2]='" + y2Bal + "' WHERE id='" + isExist + "' ");
                    }
                    else
                    {
                        SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([PaymentsType],[PaymentsHead],[PYear1],[PYear2]) 
                                VALUES ('" + rType + "','" + headName.Replace("'", "''") + "','" + y1Bal + "','" + y2Bal + "')");
                    }
                }
            }

        }

        private void BankBalance(string dateFrom, string dateTo, string type)
        {
            decimal returnValue = 0;
            if (type == "Dr")
            {
                type = "Dr";
            }

            DataTable dt = SQLQuery.ReturnDataTable("SELECT ACID, TypeID, ACName, ACNo, BankID, ZoneID, Address, Email, ContactNo, OpBalance, ProjectID, EntryBy, EntryDate FROM BankAccounts WHERE (EntryDate >='" + dateFrom+ "') AND (EntryDate <='" + dateTo + "')");
            foreach (DataRow drow in dt.Rows)
            {

                string bid = drow["ACID"].ToString();
                string balop = drow["OpBalance"].ToString();
                string Bid = drow["BankId"].ToString();
                string acNo = drow["ACNo"].ToString();
                decimal opBal = Convert.ToDecimal(balop);

                // decimal currBal1 = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate >= '" + dateFrom + "') AND  (TrDate <= '" + dateTo + "')"));
                // decimal currBal2 = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate >= '" + dateFrom + "') AND  (TrDate <= '" + dateTo + "')"));
                 decimal currBal1 = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate <= '" + dateFrom + "')"));
                 decimal currBal2 = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate <= '" + dateTo + "')"));
                if ( currBal1 != 0 || currBal2 != 0)
                {
                    string headName = SQLQuery.ReturnString("Select BankName from Banks WHERE BankId='" + Bid + "'") +
                                      "- " + acNo;
                    if (type == "Dr")
                    {
                        SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
                                VALUES ('Bank balances','" + headName + "','" + currBal1 + "','" + currBal1 + "')");
                    }
                    else
                    {
                        SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([PaymentsType],[PaymentsHead],[PYear1],[PYear2]) 
                                VALUES ('Bank balances','" + headName + "','" + currBal2 + "','" + currBal2 + "')");
                    }
                }
            }
        }
        //private void PBankBalance(string dateFrom, string dateTo, string type)
        //{
        //    decimal returnValue = 0;

        //    DataTable dt = SQLQuery.ReturnDataTable("SELECT ACID, TypeID, ACName, ACNo, BankID, ZoneID, Address, Email, ContactNo, OpBalance, ProjectID, EntryBy, EntryDate FROM BankAccounts");
        //    foreach (DataRow drow in dt.Rows)
        //    {

        //        string bid = drow["ACID"].ToString();
        //        string balop = drow["OpBalance"].ToString();
        //        string Bid = drow["BankId"].ToString();
        //        string acNo = drow["ACNo"].ToString();
        //        decimal opBal = Convert.ToDecimal(balop);

        //        decimal currBal1 = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate < '" + dateFrom + "')"));
        //        decimal currBal2 = opBal + Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " + " where TrType = 'Bank' " + " AND HeadID ='" + bid + "' AND  (TrDate < '" + dateTo + "')"));

        //        string headName = SQLQuery.ReturnString("Select BankName from Banks WHERE BankId='" + Bid + "'") + "- " + acNo;
        //        if (type == "Cr")
        //        {
        //            SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([PaymentsType],[PaymentsHead],[PYear1],[PYear2]) 
        //                        VALUES ('b) Bank balances','" + headName + "','" + currBal1 + "','" + currBal2 + "')");
        //        }
        //        else
        //        {
        //            SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([PaymentsType],[PaymentsHead],[PYear1],[PYear2]) 
        //                        VALUES ('b) Bank balances','" + headName + "','" + currBal1 + "','" + currBal2 + "')");
        //        }
        //    }
        //}
        private void ReceiptfromCustomer(string dateFrom, string dateTo)
        {
            decimal returnValue = 0;

            DataTable dt = SQLQuery.ReturnDataTable("SELECT PartyID, Type, Category, Company, ReferrenceItems, Zone, Referrer, Address, Phone, Email, Fax, IM, Website, ContactPerson, MobileNo, MatuirityDays, CreditLimit, OpBalance, PhotoURL, ProjectID, EntryBy, EntryDate,LastUpdateBy, LastUpdateDate, Remarks, VatRegNo, TDS_Terms, AccHeadTDS, VDS_Terms FROM Party WHERE (Type = 'customer') AND (EntryDate >='" + dateFrom + "') AND (EntryDate <='" + dateTo + "')");
            foreach (DataRow drow in dt.Rows)
            {
                string pid = drow["PartyID"].ToString();
                string balop = drow["OpBalance"].ToString();
                decimal opBal = Convert.ToDecimal(balop);

                decimal currBal1 = opBal +Convert.ToDecimal(SQLQuery.ReturnString("SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " +
                                           " where TrType = 'customer' " + " AND HeadID ='" + pid + "' AND (TrDate >= '" +dateFrom + "') AND (TrDate <= '" +dateFrom + "')"));
                decimal currBal2 = opBal +
                                   Convert.ToDecimal(
                                       SQLQuery.ReturnString(
                                           "SELECT isnull(sum(Dr),0)-isnull(sum(Cr),0) FROM [Transactions] " +
                                           " where TrType = 'customer' " + " AND HeadID ='" + pid + "' AND (TrDate >= '" +dateTo + "') AND (TrDate <= '" +dateTo + "')"));
                if (opBal != 0 || currBal1 != 0 || currBal2 != 0)
                {
                    string headName = drow["Company"].ToString();
                    SQLQuery.ExecNonQry(@"INSERT INTO[dbo].[ReceiptPayment]([ReceiptType],[ReceiptsHead],[RYear1],[RYear2]) 
                                VALUES ('Receipt from Customer','" + headName + "','" + currBal1 + "','" + currBal2 +
                                        "')");
                }
            }
        }
        private void ExpenditureonFixedAsset(string dateTo1, string dateTo2)
        {
            decimal returnValue = 0;

            DataTable dt = SQLQuery.ReturnDataTable("SELECT PartyID, Type, Category, Company, ReferrenceItems, Zone, Referrer, Address, Phone, Email, Fax, IM, Website, ContactPerson, MobileNo, MatuirityDays, CreditLimit, OpBalance, PhotoURL, ProjectID, EntryBy, EntryDate,LastUpdateBy, LastUpdateDate, Remarks, VatRegNo, TDS_Terms, AccHeadTDS, VDS_Terms FROM Party WHERE (Type = 'customer')");
            foreach (DataRow drow in dt.Rows)
            {
                // PHeadBalance("Expenditure on Fixed Assets and capital work-in-progress", drow["AccountsHeadID"].ToString(), "Cr", dateFrom, dateTo);
            }
        }
        protected void CrystalReportViewer1_OnUnload(object sender, EventArgs e)
        {
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();
        }
        protected override void OnUnload(EventArgs e)
        {
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();
        }
    }
}