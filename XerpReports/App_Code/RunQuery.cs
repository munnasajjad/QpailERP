﻿using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using Oxford;

/// <summary>
/// Summary description for RunQuery
/// </summary>
/// 

namespace RunQuery
{
    public class SQLQuery
    {
        public static string ReturnString(string query)
        {
            string result = "";

                SqlCommand cmd = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Connection.Open();
                result = Convert.ToString(cmd.ExecuteScalar());
                cmd.Connection.Close();
                return result;
        }


        public static void PopulateDropDown(string query, DropDownList ddGenerate, string value, string text)
        {
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            SqlDataReader Grouplist = cmd.ExecuteReader();

            ddGenerate.DataSource = Grouplist;
            ddGenerate.DataValueField = value;
            ddGenerate.DataTextField = text;
            ddGenerate.DataBind();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
        public static void PopulateMultiDropDown(DropDownList ddGenerate, string query1)
        {
            DataSet ds = new DataSet();
            SqlDataReader dr;
            int recordcount = 0;

            string query = "SELECT (Select GroupName from AccountGroup where GroupID=a.GroupID) as GroupName , AccountsHeadID, [AccountsHeadName] FROM [HeadSetup] a WHERE IsActive=1 ORDER BY GroupName, [AccountsHeadName]";
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            
            cmd.Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Products");
            cmd.Connection.Close();

            recordcount = ds.Tables[0].Rows.Count;
            DataTable dt = ds.Tables["Products"];

            if (recordcount > 0)
            {
                string oldGroupName = "";
                ListItem item1 = new ListItem();

                foreach (DataRow row in dt.Rows) //Each Items Will be looped
                {
                    string GroupName = row["GroupName"].ToString();
                    string AccountsHeadID = row["AccountsHeadID"].ToString();
                    string AccountsHeadName = row["AccountsHeadName"].ToString();

                    if (oldGroupName != GroupName)
                    {
                        oldGroupName = GroupName;
                        item1 = new ListItem(" ________ " + GroupName + " ________");
                        ddGenerate.Items.Add(item1);
                    }

                    item1 = new ListItem(AccountsHeadName, AccountsHeadID);
                    item1.Attributes["OptionGroup"] = GroupName;
                    ddGenerate.Items.Add(item1);

                }
                ddGenerate.DataBind();
            }
            
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }


        //Convert Gridview to datatable
        //DataTable dt = GetDataTable(GridView1);
        public static DataTable GetDataTable(GridView dtg)
        {
            DataTable dt = new DataTable();
            if (dtg.HeaderRow != null)
            {
                for (int i = 0; i < dtg.HeaderRow.Cells.Count; i++)
                {
                    dt.Columns.Add(dtg.HeaderRow.Cells[i].Text);
                }
            }
            foreach (GridViewRow row in dtg.Rows)
            {
                DataRow dr;
                dr = dt.NewRow();

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dr[i] = row.Cells[i].Text.Replace(" ", "");
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static DataTable ReturnDataTable(String Query)
        {
            SqlCommand cmd2y = new SqlCommand(Query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2y.Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd2y);
            DataSet ds = new DataSet("Board");
            da.Fill(ds, "Board");
            cmd2y.Connection.Close();

            DataTable citydt = ds.Tables["Board"];
            return citydt;
        }

        //Sql query to dataset
        public static DataSet ReturnDataSet(String Query)
        {
            DataSet ds = new DataSet();
                String connStr = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand objCommand = new SqlCommand(Query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(objCommand);
                    da.Fill(ds);
                    da.Dispose();
                }
            return ds;
        }

        public static void ExecNonQry(string query)
        {
            SqlCommand cmd7 = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();
            cmd7.Connection.Dispose();
        }


        public static string UploadImage(string description, FileUpload FileUpload1, string filePath, string savePath, string entryBy, string photoType)
        {
            string pid=ReturnString("Select ISNULL(MAX(PhotoID),0)+1 from Photos");
            string tExt = Path.GetFileName(FileUpload1.PostedFile.ContentType);
            string fileName = pid + "." + tExt;
            ExecNonQry("Insert into Photos (Description, PhotoURL, EntryBy, PhotoType) VALUES ('" + description + "','./Uploads/Photos/" + fileName + "','" + entryBy + "','" + photoType + "')");
            pid = ReturnString("Select ISNULL(MAX(PhotoID),0) from Photos");

            string strFullPath = filePath + fileName;

            if (File.Exists(strFullPath))
            {
                File.Delete(strFullPath);
            }
            var file = FileUpload1.PostedFile.InputStream;
            System.Drawing.Image img = System.Drawing.Image.FromStream(file, false, false);
            img.Save(savePath + fileName);
            
            return pid;
        }

        public static string ToAgeString(DateTime dob)
        {
            DateTime dt = DateTime.Today;

            int days = dt.Day - dob.Day;
            if (days < 0)
            {
                dt = dt.AddMonths(-1);
                days += DateTime.DaysInMonth(dt.Year, dt.Month);
            }

            int months = dt.Month - dob.Month;
            if (months < 0)
            {
                dt = dt.AddYears(-1);
                months += 12;
            }

            int years = dt.Year - dob.Year;

            return string.Format("{0} year{1}, {2} month{3} and {4} day{5}",
                                 years, (years == 1) ? "" : "s",
                                 months, (months == 1) ? "" : "s",
                                 days, (days == 1) ? "" : "s");
        }

        public static string ReturnInvNo(string tableName, string colName, string colEval)
        {
            SqlCommand cmd = new SqlCommand("Select ISNULL(MAX("+colName+"),0)+1001 from "+tableName, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            string invNo = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            string isExist = ReturnString("Select " + colEval + " from " + tableName + " where (" + colEval + " like '%" + invNo+"%')");
            while (isExist!="")
            {
                invNo = Convert.ToString(Convert.ToInt32(invNo) + 1);
                isExist = ReturnString("Select " + colEval + " from " + tableName + " where (" + colEval + " like '%" + invNo + "%')");
            }

            return invNo;
        }

        public static string NumberToWords(int number)
        {
            if (number == 0) { return "zero"; }
            if (number < 0) { return "minus " + NumberToWords(Math.Abs(number)); }
            string words = "";
            if ((number / 10000000) > 0) { words += NumberToWords(number / 10000000) + " Crore "; number %= 10000000; }
            if ((number / 100000) > 0) { words += NumberToWords(number / 100000) + " Lac "; number %= 100000; }
            if ((number / 1000) > 0) { words += NumberToWords(number / 1000) + " Thousand "; number %= 1000; }
            if ((number / 100) > 0) { words += NumberToWords(number / 100) + " Hundred "; number %= 100; }
            if (number > 0)
            {
                if (words != "") { words += "and "; }
                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "seventy", "Eighty", "Ninety" };
                if (number < 20) { words += unitsMap[number]; }
                else { words += tensMap[number / 10]; if ((number % 10) > 0) { words += "-" + unitsMap[number % 10]; } }
            }
            return words;//+" Taka Only.";
        }

        public static string SendSMS(string mob, string msg, string type)
        {
            try
            {

            string reqText = "";
            if (mob.Length < 13)
            {
                mob = "88" + mob;
            }
            
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1) pid, GatewayName, HostIP, SenderName, UserID, Password FROM SMSGateway WHERE (IsActive = '1')");

            foreach (DataRow drx in dtx.Rows)
            {
                string pid = drx["pid"].ToString();
                string hostIP = drx["HostIP"].ToString();
                string sender = drx["SenderName"].ToString();
                string userID = drx["UserID"].ToString();
                string password = drx["Password"].ToString();

                if (pid=="1") //Route SMS
                {
                    reqText = hostIP + "bulksms/bulksms?username=" + userID + "&password=" + password + "&type=0&dlr=1&destination=" + mob + "&source=" + sender + "&message=" + msg;
                }
                else if (pid == "2") //Bulk SMS
                {
                    reqText = hostIP + "api.php?username=" + userID + "&password=" + password + "&number=" + mob + "&sender=" + sender + "&type=0&message=" + msg;
                }
                else if (pid == "3") //Extreme SMS : http://107.20.199.106/api/v3/sendsms/plain?user=extremebd&password=jrhM0DIy&sender=xtremebd&SMSText=messagetext&GSM=8801817251582
                {
                    reqText = hostIP + "api/v3/sendsms/plain?user=" + userID + "&password=" + password + "&sender=" + sender + "&SMSText=" + msg + "&GSM=" + mob;
                }
            }

            reqText= Convert.ToString(WebRequest.Create(reqText).GetResponse());
            return reqText;

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public static decimal GetDeprAmt(string productId, decimal purchAmt, DateTime pDate)
        {
            string depPercent =
                SQLQuery.ReturnString("SELECT Convert(int, Depreciation) FROM Products Where ProductID ='" + productId + "'");
            if (depPercent == "" || depPercent == "0")
            {
                depPercent = SQLQuery.ReturnString("SELECT Depreciationvalue FROM ItemGroup WHERE GroupSrNo= (Select GroupId from vwProducts WHERE id='" + productId + "') ");
            }

            decimal monthlyPercent = (Convert.ToDecimal(depPercent)) / 12M;
            decimal monthlyAmt = purchAmt * (monthlyPercent / 100M);
            int monthsApart = 12 * (DateTime.Now.Year - pDate.Year) + DateTime.Now.Month - pDate.Month;
            int qty = Math.Abs(monthsApart);

            decimal total = qty * monthlyAmt;
            return Math.Round(purchAmt - total);
        }

        public static string FormatBDNumber(string q)
        {
            var bdCulture = CultureInfo.CreateSpecificCulture("bn-BD");
            return string.Format(bdCulture, "{0:N2}", Convert.ToDouble(q));
        }
        public static string FormatBDNumber(decimal q)
        {
            var bdCulture = CultureInfo.CreateSpecificCulture("bn-BD");
            return string.Format(bdCulture, "{0:N2}", Convert.ToDouble(q));
        }

        public static void LoadrptHeader(XerpDataSet dsx, ReportDocument rpt)
        {
            DataTable dtx2 = ReturnDataTable(@"SELECT TOP (1) CompanyID, CompanyName, MobileNo, Email, ProjectName, CompanySpeciality, CompanyAddress, AuthorityName, Logo, Photo FROM Company WHERE Active='1'");
            DataTableReader drx = dtx2.CreateDataReader();
            dsx.Load(drx, LoadOption.OverwriteChanges, dsx.Company);
            //rpt.Subreports["CrptHeader.rpt"].SetDataSource((DataTable) dsx.Company);
        }
        public static void LoadrptHeaderNew(XerpDataSet dsx, ReportDocument rpt)
        {
            DataTable dtx2 = ReturnDataTable(@"  SELECT TOP (1) [company_code], [company_name], [company_phone], [company_email],  [company_website], [company_address], [company_fax], [company_logo] FROM Company WHERE [ysndefault]='0'");
            DataTableReader drx = dtx2.CreateDataReader();
            dsx.Load(drx, LoadOption.OverwriteChanges, dsx.Company);
            rpt.Subreports["CrptHeader.rpt"].SetDataSource((DataTable) dsx.Company);
        }
    }
}

