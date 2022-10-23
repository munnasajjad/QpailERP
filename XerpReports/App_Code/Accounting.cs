using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

/// <summary>
/// Summary description for Accounting
/// </summary>
namespace Accounting
{
    public class VoucherEntry
    {
        public static void TransactionEntry(string eDate, string PartyName, string ResellerName, string Type, string BankName, string Amount, string ChargeAmount, string CurrencyName, string CurrencyRate, string Dr, string Cr, string Balance, string Remarks, string TrType, string TrName, string EntryBy, string Server, string CRate, string VoucherNo)
        {
            string invNo = TrName.Substring(0, 3) + "-" + RunQuery.SQLQuery.ReturnString("Select ISNULL(COUNT(pid),0)+1001 From VaccMaster where TrName='" + TrName + "'");

            int maxID = Convert.ToInt32(invNo.Substring(4, invNo.Length - 4));
            string isExist = RunQuery.SQLQuery.ReturnString("Select InvoiceNo from VaccMaster where InvoiceNo='" + invNo + "'");
            while (isExist != "")
            {
                maxID++;
                invNo = TrName.Substring(0, 3) + "-" + maxID.ToString();
                isExist = RunQuery.SQLQuery.ReturnString("Select InvoiceNo from VaccMaster where InvoiceNo='" + invNo + "'");
            }

            eDate = Convert.ToDateTime(eDate).ToString("yyyy-MM-dd");
            SqlCommand cmd4 = new SqlCommand("INSERT INTO VaccMaster (InvoiceNo, Date, PartyName, ResellerName, Type, BankName, Amount, ChargeAmount, CurrencyName, CurrencyRate, Dr, Cr, Balance, Remarks, TrType, TrName, EntryBy, Server, CRate, VoucherNo)" +
                                                        " VALUES (@InvoiceNo, @Date, @PartyName, @ResellerName, @Type, @BankName, @Amount, @ChargeAmount, @CurrencyName, @CurrencyRate, @Dr, @Cr, @Balance, @Remarks, @TrType, @TrName, @EntryBy, @Server, @CRate, '" + VoucherNo + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd4.Parameters.AddWithValue("@InvoiceNo", invNo);
            cmd4.Parameters.AddWithValue("@Date", eDate);
            cmd4.Parameters.AddWithValue("@PartyName", PartyName);
            cmd4.Parameters.AddWithValue("@ResellerName", ResellerName);
            cmd4.Parameters.AddWithValue("@Type", Type);
            cmd4.Parameters.AddWithValue("@BankName", BankName);

            cmd4.Parameters.AddWithValue("@Amount", Convert.ToDecimal(Amount));
            cmd4.Parameters.AddWithValue("@ChargeAmount", Convert.ToDecimal(ChargeAmount));
            cmd4.Parameters.AddWithValue("@CurrencyName", CurrencyName);
            cmd4.Parameters.AddWithValue("@CurrencyRate", CurrencyRate);

            cmd4.Parameters.AddWithValue("@Dr", Dr);
            cmd4.Parameters.AddWithValue("@Cr", Cr);
            cmd4.Parameters.AddWithValue("@Balance", Balance);
            cmd4.Parameters.AddWithValue("@Remarks", Remarks);
            cmd4.Parameters.AddWithValue("@TrType", TrType);
            cmd4.Parameters.AddWithValue("@TrName", TrName);
            cmd4.Parameters.AddWithValue("@EntryBy", EntryBy);
            cmd4.Parameters.AddWithValue("@Server", Server);
            cmd4.Parameters.AddWithValue("@CRate", Convert.ToDecimal(CRate));

            cmd4.Connection.Open();
            int succ = cmd4.ExecuteNonQuery();
            cmd4.Connection.Close();
        }


        public static string AutoVoucherEntry(string tid, string DrId, string CrId, decimal amount, string invNo, string entryBy)
        {            
            string DrName = RunQuery.SQLQuery.ReturnString("SELECT AccountsHeadName FROM [HeadSetup] WHERE AccountsHeadID='" + DrId+"'");
            string CrName = RunQuery.SQLQuery.ReturnString("SELECT AccountsHeadName FROM [HeadSetup] WHERE AccountsHeadID='" + CrId + "'");
            string vDescription = RunQuery.SQLQuery.ReturnString("SELECT TransactionType FROM [AccLink] WHERE TID=" + tid)+" (Auto)";
            
            if (invNo == "1") //insert
            {
                invNo = "Auto-" + DateTime.Now.Year.ToString() + "-" + RunQuery.SQLQuery.ReturnString("Select ISNULL(MAX(VID),0)+1001 From VoucherMaster");
                string particular = RunQuery.SQLQuery.ReturnString("SELECT ParticularId FROM [AccLink] WHERE TID=" + tid);
                InsertVoucherMaster(invNo, vDescription, particular, DrId, CrId, amount, amount, entryBy, DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else if (amount == 0) //Delete
            {
                RunQuery.SQLQuery.ExecNonQry("Update VoucherDetails set ISApproved='C' where VoucherNo='" + invNo + "'");
                RunQuery.SQLQuery.ExecNonQry("Update VoucherMaster set Voucherpost='C' where VoucherNo='" + invNo + "'");
            }
            else //Update
            {
                RunQuery.SQLQuery.ExecNonQry("Update VoucherDetails set AccountsHeadID='" + DrId + "', AccountsHeadName='" + DrName + "', VoucherDR='" + amount + "'  where VoucherDR>0 AND VoucherNo='" + invNo + "'");
                RunQuery.SQLQuery.ExecNonQry("Update VoucherDetails set AccountsHeadID='" + CrId + "', AccountsHeadName='" + CrName + "', VoucherCR='" + amount + "'  where VoucherCR>0 AND VoucherNo='" + invNo + "'");                
            }

            return invNo;
        }
        
        public static void InsertVoucherMaster(string invNo, string description, string particular, string acHeadDr, string acHeadCr, decimal dr, decimal cr, string entryBy, string entryDate)
        {
            decimal amt = dr;
            if (cr > 0)
            {
                amt = cr;
            }

            SqlCommand cmd2x = new SqlCommand("INSERT INTO VoucherMaster (VoucherNo, VoucherDate, VoucherDescription, ParticularID, VoucherEntryBy, VoucherAmount)" +
                                                "VALUES (@VoucherNo, @VoucherDate, @VoucherDescription, @ParticularID, @VoucherEntryBy, @VoucherAmount)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2x.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = invNo;
            cmd2x.Parameters.Add("@VoucherDate", SqlDbType.DateTime).Value = Convert.ToDateTime(entryDate).ToString("yyyy-MM-dd");
            cmd2x.Parameters.Add("@VoucherDescription", SqlDbType.VarChar).Value = description;
            cmd2x.Parameters.Add("@ParticularID", SqlDbType.NVarChar).Value = particular;
            cmd2x.Parameters.Add("@VoucherEntryBy", SqlDbType.VarChar).Value = entryBy;
            cmd2x.Parameters.Add("@VoucherAmount", SqlDbType.Decimal).Value = amt;

            cmd2x.Connection.Open();
            int success = cmd2x.ExecuteNonQuery();
            cmd2x.Connection.Close();

            if (cr > 0)
            {
                InsertVoucherDetails(invNo, description, acHeadCr, 0, cr, entryDate);
            }
            if (dr > 0)
            {
                InsertVoucherDetails(invNo, description, acHeadDr, dr, 0, entryDate);
            }
        }

        public static void InsertVoucherDetails(string invNo, string description, string acHeadID, decimal dr, decimal cr, string entryDate)
        {
            string acHeadName = RunQuery.SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup where AccountsHeadID='" + acHeadID + "'");
            SqlCommand cmd2y = new SqlCommand("INSERT INTO VoucherDetails (VoucherNo, VoucherRowDescription, AccountsHeadID, AccountsHeadName, VoucherDR, VoucherCR, EntryDate)" +
                                                "VALUES (@VoucherNo, @VoucherRowDescription, @AccountsHeadID, @AccountsHeadName, @VoucherDR, @VoucherCR, @EntryDate)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2y.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = invNo;
            cmd2y.Parameters.Add("@VoucherRowDescription", SqlDbType.VarChar).Value = HttpUtility.HtmlDecode(description);
            cmd2y.Parameters.Add("@AccountsHeadID", SqlDbType.VarChar).Value = acHeadID;
            cmd2y.Parameters.Add("@AccountsHeadName", SqlDbType.VarChar).Value = acHeadName;
            cmd2y.Parameters.Add("@VoucherDR", SqlDbType.Decimal).Value = dr;
            cmd2y.Parameters.Add("@VoucherCR", SqlDbType.Decimal).Value = cr;
            cmd2y.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = Convert.ToDateTime(entryDate).ToString("yyyy-MM-dd");

            cmd2y.Connection.Open();
            cmd2y.ExecuteNonQuery();
            cmd2y.Connection.Close();
        }

        public static void CancelVoucher(string invNo, string lName)
        {
            SqlCommand cmd2x = new SqlCommand("UPDATE VoucherMaster set Voucherpost='C', VoucherPostby=@VoucherPostby, Voucherpostdate=@Voucherpostdate where VoucherNo=@VoucherNo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2x.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = invNo;
            cmd2x.Parameters.Add("@VoucherPostby", SqlDbType.VarChar).Value = lName;
            cmd2x.Parameters.Add("@Voucherpostdate", SqlDbType.DateTime).Value = DateTime.Now;
            cmd2x.Connection.Open();
            cmd2x.ExecuteNonQuery();
            cmd2x.Connection.Close();

            SqlCommand cmd2 = new SqlCommand("UPDATE VoucherDetails set ISApproved='C' where VoucherNo=@VoucherNo", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = invNo;
            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
        }

        public static string CreateAccountsHead(string aName, string controlAc, string opBalDr, string opBalCr)
        {
            try
            {
                string maxID = RunQuery.SQLQuery.ReturnString("Select ISNULL(COUNT(EntryID),0)+1 from HeadSetup WHERE ControlAccountsID='" + controlAc + "'");

                if (maxID.Length < 2)
                {
                    maxID = "00" + maxID;
                }
                else if (maxID.Length < 3)
                {
                    maxID = "0" + maxID;
                }
                string AcHeadId = controlAc + maxID;
                string isExist = RunQuery.SQLQuery.ReturnString("Select AccountsHeadID from HeadSetup where AccountsHeadID='" + AcHeadId + "'");
                while (isExist != "")
                {
                    maxID = Convert.ToString(Convert.ToInt32(maxID) + 1);

                    if (maxID.Length < 2)
                    {
                        maxID = "00" + maxID;
                    }
                    else if (maxID.Length < 3)
                    {
                        maxID = "0" + maxID;
                    }
                    AcHeadId = controlAc + maxID;
                    isExist = RunQuery.SQLQuery.ReturnString("Select AccountsHeadID from HeadSetup where AccountsHeadID='" + AcHeadId + "'");
                }

                isExist = RunQuery.SQLQuery.ReturnString("Select Count(AccountsHeadID) from HeadSetup where AccountsHeadName Like '%" + aName + "%'");
                if(isExist!="0")
                {
                    aName = aName + " " + isExist;
                }

                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

                //Create Sql Command
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO HeadSetup (GroupID, AccountsID, ControlAccountsID, AccountsHeadID, AccountsHeadName, AccountsOpeningBalance, OpDate, Emark, OpBalDr, OpBalCr)" +
                                        "VALUES (@GroupID, @AccountsID, @ControlAccountsID, @AccountsHeadID, @AccountsHeadName, @AccountsOpeningBalance, @OpDate, @Emark, @OpBalDr, @OpBalCr)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cnn;

                //Parameter array Declaration
                SqlParameter[] param = new SqlParameter[8];

                param[0] = new SqlParameter("@GroupID", SqlDbType.VarChar, 2);
                param[1] = new SqlParameter("@AccountsID", SqlDbType.VarChar, 4);
                param[2] = new SqlParameter("@ControlAccountsID", SqlDbType.VarChar, 6);
                param[3] = new SqlParameter("@AccountsHeadID", SqlDbType.VarChar);
                param[4] = new SqlParameter("@AccountsHeadName", SqlDbType.VarChar);
                param[5] = new SqlParameter("@AccountsOpeningBalance", SqlDbType.Decimal);
                param[6] = new SqlParameter("@OpDate", SqlDbType.DateTime);
                param[7] = new SqlParameter("@Emark", SqlDbType.VarChar);

                param[0].Value = controlAc.Substring(0, 2);
                param[1].Value = controlAc.Substring(0, 4);
                param[2].Value = controlAc;
                param[3].Value = AcHeadId;
                param[4].Value = aName;
                param[5].Value = opBalCr;
                param[6].Value = DateTime.Now;
                cmd.Parameters.Add("@OpBalDr", SqlDbType.Decimal).Value = opBalDr;
                cmd.Parameters.Add("@OpBalCr", SqlDbType.Decimal).Value = opBalCr;
                param[7].Value = "Auto";

                /*** Looping Fields ***/
                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cnn.Open();
                int Success = cmd.ExecuteNonQuery();
                cnn.Close();

                return AcHeadId;
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        
        
        public static void SynchronizeAccounts()
        {
            //Sync Op Balances: Vendors
            string vendorDr= RunQuery.SQLQuery.ReturnString("SELECT HeadIdDr FROM [AccLink] WHERE TID=1");
            string vendorCr = RunQuery.SQLQuery.ReturnString("SELECT HeadIdCr FROM [AccLink] WHERE TID=1");
            string totalOpBalance = RunQuery.SQLQuery.ReturnString("SELECT SUM(OpBalance) FROM [Vendors]");
            string isExist = RunQuery.SQLQuery.ReturnString("SELECT VoucherNo FROM [VoucherMaster] WHERE VoucherDescription='Vendor Opening Balance (Auto Sync)'");

            if(isExist=="")
            {
                string invNo="Auto-"+DateTime.Now.Year.ToString()+"-"+RunQuery.SQLQuery.ReturnString("Select ISNULL(MAX(VID),0)+1001 From VoucherMaster");
                string particular = RunQuery.SQLQuery.ReturnString("SELECT ParticularId FROM [AccLink] WHERE TID=1");
                InsertVoucherMaster(invNo, "Vendor Opening Balance (Auto Sync)", particular, vendorDr, vendorCr, Convert.ToDecimal(totalOpBalance), Convert.ToDecimal(totalOpBalance), "Auto", DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else
            {
                RunQuery.SQLQuery.ExecNonQry("Update VoucherDetails set VoucherDR='" + totalOpBalance + "' where AccountsHeadID='" + vendorDr + "' AND  VoucherRowDescription='Vendor Opening Balance (Auto Sync)'");
                RunQuery.SQLQuery.ExecNonQry("Update VoucherDetails set VoucherCR='" + totalOpBalance + "' where AccountsHeadID='" + vendorCr + "' AND  VoucherRowDescription='Vendor Opening Balance (Auto Sync)'");
            }

            //Sync Op Balances: Customers
            vendorDr = RunQuery.SQLQuery.ReturnString("SELECT HeadIdDr FROM [AccLink] WHERE TID=4");
            vendorCr = RunQuery.SQLQuery.ReturnString("SELECT HeadIdCr FROM [AccLink] WHERE TID=4");
            totalOpBalance = RunQuery.SQLQuery.ReturnString("SELECT SUM(OpBalance) FROM [Resellers]");
            isExist = RunQuery.SQLQuery.ReturnString("SELECT VoucherNo FROM [VoucherMaster] WHERE VoucherDescription='Customer Opening Balance (Auto Sync)'");

            if (isExist == "")
            {
                string invNo = "Auto-" + DateTime.Now.Year.ToString() + "-" + RunQuery.SQLQuery.ReturnString("Select ISNULL(MAX(VID),0)+1001 From VoucherMaster");
                string particular = RunQuery.SQLQuery.ReturnString("SELECT ParticularId FROM [AccLink] WHERE TID=4");
                InsertVoucherMaster(invNo, "Customer Opening Balance (Auto Sync)", particular, vendorDr, vendorCr, Convert.ToDecimal(totalOpBalance), Convert.ToDecimal(totalOpBalance), "Auto", DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else
            {
                RunQuery.SQLQuery.ExecNonQry("Update VoucherDetails set VoucherDR='" + totalOpBalance + "' where AccountsHeadID='" + vendorDr + "' AND  VoucherRowDescription='Customer Opening Balance (Auto Sync)'");
                RunQuery.SQLQuery.ExecNonQry("Update VoucherDetails set VoucherCR='" + totalOpBalance + "' where AccountsHeadID='" + vendorCr + "' AND  VoucherRowDescription='Customer Opening Balance (Auto Sync)'");
            }

        }
        

        public static void VATServiceProcessing(string memberID, decimal amount, string description, string reqID)
        {
            //
            // TODO: Add constructor logic here
            //
            if (reqID == "")
            {
                SqlCommand cmd2 = new SqlCommand("SELECT ISNULL(MAX(AutoID) + 1, 0) FROM PayRequest", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd2.Connection.Open();
                int maxIndex = Convert.ToInt32(cmd2.ExecuteScalar());
                cmd2.Connection.Close();
                cmd2.Connection.Dispose();

                string ss = DateTime.Now.TimeOfDay.Seconds.ToString();
                string mm = DateTime.Now.TimeOfDay.Minutes.ToString();
                string hh = DateTime.Now.TimeOfDay.Hours.ToString();
                string dd = DateTime.Now.Day.ToString();
                string MM = DateTime.Now.Month.ToString();
                string yy = DateTime.Now.Year.ToString();
                reqID = dd + mm + hh + mm + ss + maxIndex;
            }

            amount = amount * 15 / 100;
            SqlCommand cmd2p = new SqlCommand("INSERT INTO PayRequest (RequestID, MemberID, AccountName, RequestBranch, AccountNo, Amount, ServiceCharge, IsApproved)" +
                                        "VALUES (@RequestID, @MemberID, @AccountName, @RequestBranch, @AccountNo, @Amount, @ServiceCharge, @ToDeposit)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2p.Parameters.Add("@RequestID", SqlDbType.VarChar).Value = reqID;
            cmd2p.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = memberID;
            cmd2p.Parameters.Add("@AccountName", SqlDbType.VarChar).Value = "VAT/Service Charge";
            cmd2p.Parameters.Add("@RequestBranch", SqlDbType.VarChar).Value = "False Entry"; //ddType.SelectedValue;
            cmd2p.Parameters.Add("@AccountNo", SqlDbType.VarChar).Value = reqID;
            cmd2p.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
            cmd2p.Parameters.Add("@ServiceCharge", SqlDbType.Decimal).Value = 0;
            cmd2p.Parameters.Add("@ToDeposit", SqlDbType.VarChar).Value = "I";

            cmd2p.Connection.Open();
            int success = cmd2p.ExecuteNonQuery();
            cmd2p.Connection.Close();

            SqlCommand cmd2x = new SqlCommand("INSERT INTO MemberExpenses (ExpHeadName, Description, Amount, MemberID, ExpenseID, RequestID)" +
                                                                      "VALUES (@ExpHeadName, @Description, @Amount, @MemberID, @ExpenseID, @RequestID)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2x.Parameters.Add("@ExpHeadName", SqlDbType.VarChar).Value = "VAT/Service Charge";
            cmd2x.Parameters.Add("@Description", SqlDbType.VarChar).Value = "VAT+Service: " + description;
            cmd2x.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
            cmd2x.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = memberID;
            cmd2x.Parameters.Add("@ExpenseID", SqlDbType.VarChar).Value = 22;
            cmd2x.Parameters.Add("@RequestID", SqlDbType.VarChar).Value = reqID;

            cmd2x.Connection.Open();
            int success2 = cmd2x.ExecuteNonQuery();
            cmd2x.Connection.Close();
        }
    }
}