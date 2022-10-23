using System;
using System.Collections.Generic;
using System.Linq;
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
using RunQuery;


/// <summary>
/// Summary description for Accounting
/// </summary>
namespace Accounting
{
    public class VoucherEntry
    {/*
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
        */
		
        public static decimal SupplierBalance(string partyId)
        {
            string opBal = SQLQuery.ReturnString("SELECT ISNULL(OpBalance,0) from Party where PartyID='" + partyId + "' ");
            string purchased = SQLQuery.ReturnString("SELECT ISNULL(SUM(Dr),0)-ISNULL(SUM(Cr),0)  from Transactions where HeadID='" + partyId + "' and TrType = 'Supplier' ");
            decimal bal = Convert.ToDecimal(purchased) + Convert.ToDecimal(opBal);
            return bal;
        }

        public static decimal CustomerBalance(string partyId)
        {
            string opBal = SQLQuery.ReturnString("SELECT ISNULL(OpBalance,0) from Party where PartyID='" + partyId + "' ");
            string purchased = SQLQuery.ReturnString("SELECT ISNULL(SUM(Dr),0)-ISNULL(SUM(Cr),0)  from Transactions where HeadID='" + partyId + "' and TrType = 'Customer' ");
            decimal bal = Convert.ToDecimal(purchased) + Convert.ToDecimal(opBal);
            return bal;
        }
        public static string AutoVoucherEntry(string tid, string vDescription, string DrId, string CrId, decimal amount, string invNo, string voucherType, string entryBy, string entryDate, string type)
        {
            string drName = SQLQuery.ReturnString("SELECT AccountsHeadName FROM [HeadSetup] WHERE AccountsHeadID='" + DrId+"'");
            string crName = SQLQuery.ReturnString("SELECT AccountsHeadName FROM [HeadSetup] WHERE AccountsHeadID='" + CrId + "'");
            if (vDescription == "")
            {
                vDescription = "Auto Voucher Posting for " + SQLQuery.ReturnString("SELECT TransactionType FROM [AccLink] WHERE TID=" + tid) + " #" + invNo;
            }
            
            string particular = SQLQuery.ReturnString("SELECT ParticularId FROM [AccLink] WHERE TID=" + tid);
            string voucherNo = "";
            
            if (type == "1") //insert
            {
                 voucherNo = "Auto-" + DateTime.Now.Year.ToString() + "-" + RunQuery.SQLQuery.ReturnString("Select ISNULL(MAX(VID),0)+1001 From VoucherMaster");
                InsertVoucherMaster(voucherNo, vDescription, particular, DrId, CrId, amount, amount, entryBy, entryDate, invNo, voucherType);
            }
            else if (type == "0") //Cancel or Delete
            {
                voucherNo =SQLQuery.ReturnString("Select VoucherNo from VoucherMaster where ParticularID='" + particular + "' AND VoucherReferenceNo='" + invNo + "' AND Voucherpost='A'");
                RunQuery.SQLQuery.ExecNonQry("Update VoucherDetails set ISApproved='C' where VoucherNo='" + voucherNo + "'");
                RunQuery.SQLQuery.ExecNonQry("Update VoucherMaster set Voucherpost='C' where VoucherNo='" + voucherNo + "'");
            }
            else //Update
            {
                RunQuery.SQLQuery.ExecNonQry("Update VoucherMaster set VoucherDate='" + entryDate + "' where  VoucherNo='" + invNo + "'");
                RunQuery.SQLQuery.ExecNonQry("Update VoucherDetails set AccountsHeadID='" + DrId + "', AccountsHeadName='" + drName + "', VoucherDR='" + amount + "'  where VoucherDR>0 AND VoucherNo='" + invNo + "'");
                RunQuery.SQLQuery.ExecNonQry("Update VoucherDetails set AccountsHeadID='" + CrId + "', AccountsHeadName='" + crName + "', VoucherCR='" + amount + "'  where VoucherCR>0 AND VoucherNo='" + invNo + "'");
            }

            return voucherNo;
        }
        public static void InsertVoucherMaster(string VoucherNo, string description, string particular, string acHeadDr, string acHeadCr, decimal dr, decimal cr, string entryBy, string entryDate, string invNo, string voucherType)
        {
            decimal amt = dr;
            if (cr > 0)
            {
                amt = cr;
            }

            SqlCommand cmd2x = new SqlCommand("INSERT INTO VoucherMaster (VoucherNo, VoucherDate, VoucherDescription, ParticularID, VoucherEntryBy, VoucherAmount, VoucherReferenceNo, VoucherRefType)" +
                                                "VALUES (@VoucherNo, @VoucherDate, @VoucherDescription, @ParticularID, @VoucherEntryBy, @VoucherAmount, @VoucherReferenceNo, @VoucherRefType)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2x.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = VoucherNo;
            cmd2x.Parameters.Add("@VoucherDate", SqlDbType.DateTime).Value = Convert.ToDateTime(entryDate).ToString("yyyy-MM-dd");
            cmd2x.Parameters.Add("@VoucherDescription", SqlDbType.VarChar).Value = description;
            cmd2x.Parameters.Add("@ParticularID", SqlDbType.NVarChar).Value = particular;
            cmd2x.Parameters.Add("@VoucherEntryBy", SqlDbType.VarChar).Value = entryBy;
            cmd2x.Parameters.Add("@VoucherAmount", SqlDbType.Decimal).Value = amt;
            cmd2x.Parameters.Add("@VoucherReferenceNo", SqlDbType.NVarChar).Value = invNo;
            cmd2x.Parameters.Add("@VoucherRefType", SqlDbType.NVarChar).Value = voucherType;

            cmd2x.Connection.Open();
            int success = cmd2x.ExecuteNonQuery();
            cmd2x.Connection.Close();

            if (cr > 0)
            {
                InsertVoucherDetails(VoucherNo, description, acHeadCr, 0, cr, entryDate);
            }
            if (dr > 0)
            {
                InsertVoucherDetails(VoucherNo, description, acHeadDr, dr, 0, entryDate);
            }
        }

        public static string AutoVoucherEntryFinishedGoods(string tid, string description, string drId, string crId, decimal amount, decimal kg, decimal pcs, string rate, string invNo, string entryBy, string entryDate, string type, string linrNo)
        {
            string drName = SQLQuery.ReturnString("SELECT AccountsHeadName FROM [HeadSetup] WHERE AccountsHeadID='" + drId + "'");
            string crName = SQLQuery.ReturnString("SELECT AccountsHeadName FROM [HeadSetup] WHERE AccountsHeadID='" + crId + "'");
            string vDescription = "Auto Voucher Posting for " + SQLQuery.ReturnString("SELECT TransactionType FROM [AccLink] WHERE TID=" + tid) + " #" + invNo;
            string particular = SQLQuery.ReturnString("SELECT ParticularId FROM [AccLink] WHERE TID=" + tid);
            string voucherNo = "";
            
            if (type == "1") //insert
            {
                voucherNo = "Auto-" + DateTime.Now.Year.ToString() + "-" + RunQuery.SQLQuery.ReturnString("Select ISNULL(MAX(VID),0)+1001 From VoucherMaster");
                InsertVoucherMasterFinishedGoods(voucherNo, description, particular, drId, crId, amount, amount, kg, kg, pcs, pcs, rate, entryBy, entryDate, invNo, linrNo);
            }
            else if (type == "0") //Delete
            {
                voucherNo = SQLQuery.ReturnString("Select VoucherNo from VoucherMaster where ParticularID='" + particular + "' AND VoucherReferenceNo='" + invNo + "' AND Voucherpost='A'");
                RunQuery.SQLQuery.ExecNonQry("Update VoucherDetails set ISApproved='C' where VoucherNo='" + voucherNo + "'");
                RunQuery.SQLQuery.ExecNonQry("Update VoucherMaster set Voucherpost='C' where VoucherNo='" + voucherNo + "'");
            }
            else //Update
            {
                RunQuery.SQLQuery.ExecNonQry("Update VoucherMaster set VoucherDate='" + entryDate + "' where  VoucherNo='" + invNo + "'");
                RunQuery.SQLQuery.ExecNonQry("Update VoucherDetails set AccountsHeadID='" + drId + "', AccountsHeadName='" + drName + "', VoucherDR='" + amount + "'  where VoucherDR>0 AND VoucherNo='" + invNo + "'");
                RunQuery.SQLQuery.ExecNonQry("Update VoucherDetails set AccountsHeadID='" + crId + "', AccountsHeadName='" + crName + "', VoucherCR='" + amount + "'  where VoucherCR>0 AND VoucherNo='" + invNo + "'");
            }

            return voucherNo;
        }

        public static void InsertVoucherMasterFinishedGoods(string voucherNo, string description, string particular, string acHeadDr, string acHeadCr, decimal dr, decimal cr, decimal inKg, decimal outKg, decimal inPcs, decimal outPcs, string rate, string entryBy, string entryDate, string invNo, string lineNo)
        {
            decimal amt = dr;
            if (cr > 0)
            {
                amt = cr;
            }

            SqlCommand cmd2X = new SqlCommand("INSERT INTO VoucherMaster (VoucherNo, VoucherDate, VoucherDescription, ParticularID, VoucherEntryBy, VoucherAmount, VoucherReferenceNo)" +
                                                "VALUES (@VoucherNo, @VoucherDate, @VoucherDescription, @ParticularID, @VoucherEntryBy, @VoucherAmount, @VoucherReferenceNo)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2X.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = voucherNo;
            cmd2X.Parameters.Add("@VoucherDate", SqlDbType.DateTime).Value = Convert.ToDateTime(entryDate).ToString("yyyy-MM-dd");
            cmd2X.Parameters.Add("@VoucherDescription", SqlDbType.VarChar).Value = description;
            cmd2X.Parameters.Add("@ParticularID", SqlDbType.NVarChar).Value = particular;
            cmd2X.Parameters.Add("@VoucherEntryBy", SqlDbType.VarChar).Value = entryBy;
            cmd2X.Parameters.Add("@VoucherAmount", SqlDbType.Decimal).Value = amt;
            cmd2X.Parameters.Add("@VoucherReferenceNo", SqlDbType.NVarChar).Value = invNo;

            cmd2X.Connection.Open();
            int success = cmd2X.ExecuteNonQuery();
            cmd2X.Connection.Close();

            if (cr > 0)
            {
                //InsertStockDetails(voucherNo, description, acHeadCr, 0, cr, entryDate);
                InsertStockDetails(voucherNo, description, acHeadDr, cr, 0, inKg, 0, inPcs, 0, rate, entryDate, lineNo);
            }
            if (dr > 0)
            {
                //InsertStockDetails(voucherNo, description, acHeadDr, dr, 0, entryDate);
                InsertStockDetails(voucherNo, description, acHeadCr, 0, dr, 0, outKg, 0, outPcs, rate, entryDate, lineNo);
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

        public static void InsertStockDetails(string invNo, string description, string acHeadId, decimal dr, decimal cr, decimal inKg, decimal outKg, decimal inPcs, decimal outPcs, string rate, string entryDate, string lineNo)
        {
            //string acHeadName = RunQuery.SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup where AccountsHeadID='" + acHeadId + "'");
            description = description + " #" + invNo;
            string acHeadName = RunQuery.SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup where AccountsHeadID='" + acHeadId + "'");
            if (acHeadName == "")
            {
                acHeadName = RunQuery.SQLQuery.ReturnString("SELECT ProductName FROM FinishedProducts WHERE pid ='" + acHeadId + "'");
            }
            SqlCommand cmd2 = new SqlCommand("INSERT INTO VoucherDetails (VoucherNo, VoucherRowDescription, AccountsHeadID, AccountsHeadName, VoucherDR, VoucherCR, InQty, OutQty, InPcs, OutPcs, Rate, EntryDate, projectName)" +
                                             "VALUES (@VoucherNo, @VoucherRowDescription, @AccountsHeadID, @AccountsHeadName, @VoucherDR, @VoucherCR, @InQty, @OutQty, @InPcs, @OutPcs, '" + rate + "', @EntryDate, '"+lineNo+"')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = invNo;
            cmd2.Parameters.Add("@VoucherRowDescription", SqlDbType.VarChar).Value = HttpUtility.HtmlDecode(description);
            cmd2.Parameters.Add("@AccountsHeadID", SqlDbType.VarChar).Value = acHeadId;
            cmd2.Parameters.Add("@AccountsHeadName", SqlDbType.VarChar).Value = acHeadName;
            cmd2.Parameters.Add("@VoucherDR", SqlDbType.Decimal).Value = dr;
            cmd2.Parameters.Add("@VoucherCR", SqlDbType.Decimal).Value = cr;
            cmd2.Parameters.Add("@InQty", SqlDbType.Decimal).Value = inKg;
            cmd2.Parameters.Add("@OutQty", SqlDbType.Decimal).Value = outKg;
            cmd2.Parameters.Add("@InPcs", SqlDbType.Decimal).Value = inPcs;
            cmd2.Parameters.Add("@OutPcs", SqlDbType.Decimal).Value = outPcs;
            cmd2.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = Convert.ToDateTime(entryDate).ToString("yyyy-MM-dd");

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
        }

        public static void InsertVoucherWithQty(string voucherNo, string description, string particular, string acHeadDr, string acHeadCr, decimal amount, string entryBy, string entryDate, string invNo, string qty, string Rate)
        {
            SqlCommand cmd2x = new SqlCommand("INSERT INTO VoucherMaster (VoucherNo, VoucherDate, VoucherDescription, ParticularID, VoucherEntryBy, VoucherAmount, VoucherReferenceNo)" +
                                                "VALUES (@VoucherNo, @VoucherDate, @VoucherDescription, @ParticularID, @VoucherEntryBy, @VoucherAmount, @VoucherReferenceNo)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2x.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = voucherNo;
            cmd2x.Parameters.Add("@VoucherDate", SqlDbType.DateTime).Value = Convert.ToDateTime(entryDate).ToString("yyyy-MM-dd");
            cmd2x.Parameters.Add("@VoucherDescription", SqlDbType.VarChar).Value = description;
            cmd2x.Parameters.Add("@ParticularID", SqlDbType.NVarChar).Value = particular;
            cmd2x.Parameters.Add("@VoucherEntryBy", SqlDbType.VarChar).Value = entryBy;
            cmd2x.Parameters.Add("@VoucherAmount", SqlDbType.Decimal).Value = amount;
            cmd2x.Parameters.Add("@VoucherReferenceNo", SqlDbType.NVarChar).Value = invNo;

            cmd2x.Connection.Open();
            cmd2x.ExecuteNonQuery();
            cmd2x.Connection.Close();

            InsertVoucherDetailsWithQty(voucherNo, description, acHeadDr, amount, 0, entryDate, qty, "0", Rate);
            InsertVoucherDetailsWithQty(voucherNo, description, acHeadCr, 0, amount, entryDate, "0", qty,  Rate);
        }

        public static void InsertVoucherDetailsWithQty(string invNo, string description, string acHeadID, decimal dr, decimal cr, string entryDate, string qtyIn, string qtyOut, string Rate)
        {
            string acHeadName = RunQuery.SQLQuery.ReturnString("Select AccountsHeadName from HeadSetup where AccountsHeadID='" + acHeadID + "'");
            SqlCommand cmd2y = new SqlCommand("INSERT INTO VoucherDetails (VoucherNo, VoucherRowDescription, AccountsHeadID, AccountsHeadName, VoucherDR, VoucherCR, EntryDate,InQty,OutQty,Rate)" +
                                              "VALUES (@VoucherNo, @VoucherRowDescription, @AccountsHeadID, @AccountsHeadName, @VoucherDR, @VoucherCR, @EntryDate, '"+ qtyIn + "', '"+ qtyOut + "', '"+ Rate + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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


        /*
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
        */
        public static void TransactionEntry(string InvNo, string TrDate, string HeadID, string HeadName, string Description, string Dr, string Cr, string Balance, string TrGroup, string TrType, string AccHeadID, string EntryBy, string ProjectID)
        {
            TrDate = Convert.ToDateTime(TrDate).ToString("yyyy-MM-dd");
            SqlCommand cmd4 = new SqlCommand("INSERT INTO Transactions (InvNo, TrDate, HeadID, HeadName, Description, Dr, Cr, Balance, TrGroup, TrType, AccHeadID, EntryBy, ProjectID)" +
                                                        " VALUES (@InvNo, @TrDate, @HeadID, @HeadName, @Description, @Dr, @Cr, @Balance, @TrGroup, @TrType, @AccHeadID, @EntryBy, @ProjectID)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd4.Parameters.AddWithValue("@InvNo", InvNo);
            cmd4.Parameters.AddWithValue("@TrDate", Convert.ToDateTime(TrDate));
            cmd4.Parameters.AddWithValue("@HeadID", HeadID);
            cmd4.Parameters.AddWithValue("@HeadName", HeadName);
            cmd4.Parameters.AddWithValue("@Description", Description);

            cmd4.Parameters.AddWithValue("@Dr", Convert.ToDecimal(Dr));
            cmd4.Parameters.AddWithValue("@Cr", Convert.ToDecimal(Cr));
            cmd4.Parameters.AddWithValue("@Balance", 0);
            cmd4.Parameters.AddWithValue("@TrGroup", TrGroup);
            cmd4.Parameters.AddWithValue("@TrType", TrType);

            cmd4.Parameters.AddWithValue("@AccHeadID", AccHeadID);
            cmd4.Parameters.AddWithValue("@EntryBy", EntryBy);
            cmd4.Parameters.AddWithValue("@ProjectID", "1");

            cmd4.Connection.Open();
            int succ = cmd4.ExecuteNonQuery();
            cmd4.Connection.Close();
        }

        public static void StockEntry(string InvoiceID, string EntryType, string RefNo, string SizeID, string BrandID, string ProductID, string ProductName, string WarehouseID, string LocationID, string ItemGroup, string InQuantity, string OutQuantity, string InWeight, string OutWeight, string ItemSerialNo, string Remark, string Status, string StockLocation, string EntryBy)
        {
            //Item entry to stock
            SqlCommand cmd3 = new SqlCommand("INSERT INTO Stock (InvoiceID, EntryType, RefNo, SizeID, BrandID, ProductID, ProductName, WarehouseID, LocationID, ItemGroup, InQuantity, OutQuantity, InWeight, OutWeight, ItemSerialNo, Remark, Status, StockLocation, EntryBy)" +
                                                        " VALUES (@InvoiceID, @EntryType, @RefNo, @SizeID, @BrandID, @ProductID, @ProductName, @WarehouseID, @LocationID, @ItemGroup, @InQuantity, @OutQuantity, @InWeight, @OutWeight, @ItemSerialNo, @Remark, @Status, @StockLocation, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd3.Parameters.AddWithValue("@InvoiceID", InvoiceID);
            cmd3.Parameters.AddWithValue("@EntryType", EntryType);
            cmd3.Parameters.AddWithValue("@RefNo", RefNo);
            cmd3.Parameters.AddWithValue("@SizeID", SizeID);
            cmd3.Parameters.AddWithValue("@BrandID", BrandID);
            cmd3.Parameters.AddWithValue("@ProductID", ProductID);

            cmd3.Parameters.AddWithValue("@ProductName", ProductName);
            cmd3.Parameters.AddWithValue("@WarehouseID", WarehouseID);
            cmd3.Parameters.AddWithValue("@LocationID", LocationID);
            cmd3.Parameters.AddWithValue("@ItemGroup", ItemGroup);

            cmd3.Parameters.AddWithValue("@InQuantity", Convert.ToDecimal(InQuantity));
            cmd3.Parameters.AddWithValue("@OutQuantity", Convert.ToDecimal(OutQuantity));
            cmd3.Parameters.AddWithValue("@InWeight", Convert.ToDecimal(InWeight));
            cmd3.Parameters.AddWithValue("@OutWeight", Convert.ToDecimal(OutWeight));

            cmd3.Parameters.AddWithValue("@ItemSerialNo", ItemSerialNo);
            cmd3.Parameters.AddWithValue("@Remark", Remark);
            cmd3.Parameters.AddWithValue("@Status", Status);
            cmd3.Parameters.AddWithValue("@StockLocation", StockLocation);
            cmd3.Parameters.AddWithValue("@EntryBy", EntryBy);

            cmd3.Connection.Open();
            cmd3.ExecuteNonQuery();
            cmd3.Connection.Close();

        }

        public static void NetProfit4balance(decimal amt)
        {
            //AutoVoucherEntry("5", "010101001", "010104001", Convert.ToDecimal(txtReceived.Text), colID, lName, Convert.ToDateTime(txtColDate.Text).ToString("yyyy-MM-dd"), "1");
        }
        public static decimal CalculateItemCosting(decimal rate, decimal vat, decimal othExpense, decimal discount, decimal totalAmount, decimal itemAmount, decimal qnty)
        {
            decimal cost = rate + (((vat + othExpense - discount) * itemAmount) / (totalAmount * qnty));
            return cost;
        }
        public static decimal CalculateItemDiscount(decimal rate, decimal vat, decimal othExpense, decimal discount, decimal totalAmount, decimal itemAmount, decimal qnty)
        {
            decimal cost = rate + (((vat + othExpense - discount) * itemAmount) / (totalAmount * qnty));
            return cost;
        }
        /*
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
        }*/
    }
}