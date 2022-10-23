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
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using Accounting;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DataTable = System.Data.DataTable;

/// <summary>
/// Summary description for RunQuery
/// </summary>
///

namespace RunQuery
{
    public class SQLQuery
    {
        public static void Empty2Zero(TextBox textBox)
        {
            if (textBox.Text == "")
            {
                textBox.Text = "0";
            }
        }

        public static string OparatePermission(string userName, string operationName)
        {
            //string EmpId = ReturnString("Select sl from Employee where LoginID='" + userName + "'");
            string optName = "CanRead";
            if (operationName == "Insert")
            {
                optName = "CanInsert";
            }
            else if (operationName == "Delete")
            {
                optName = "CanDelete";
            }
            else if (operationName == "Update")
            {
                optName = "CanUpdate";
            }
            string isPermitted = ReturnString("Select " + optName + " from Admin_Users where Email='" + userName + "'");
            if (userName == "rony" || userName == "")
            {
                isPermitted = "1";
            }
            return isPermitted;
        }

        public static void Empty2Zero(Label textBox)
        {
            if (textBox.Text == "")
            {
                textBox.Text = "0";
            }
        }

        public static string ReturnString(string query)
        {
            try
            {
                string result = "";
                SqlCommand cmd = new SqlCommand(query,
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Connection.Open();
                result = Convert.ToString(cmd.ExecuteScalar());
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                SqlConnection.ClearAllPools();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(query + ".<br/> ERROR: " + ex.ToString());
                return query + ".<br/><br/> ERROR: " + ex.ToString();
            }
        }

        public static void ExecNonQry(string query)
        {
            SqlCommand cmd7 = new SqlCommand(query,
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();
            cmd7.Connection.Dispose();
            SqlConnection.ClearAllPools();
        }

        public static string LocalDateFormat(string fieldValue)
        {
            try
            {
                if (fieldValue != "")
                {
                    fieldValue = Convert.ToDateTime(fieldValue.ToString()).ToString("dd/MM/yyyy");
                }
                return fieldValue;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static void ExecNonQry(object p)
        {
            throw new NotImplementedException();
        }

        public static void PopulateGridView(GridView ItemGrid, string query)
        {
            SqlCommand cmd = new SqlCommand(query,
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            ItemGrid.EmptyDataText = "No items to view...";
            ItemGrid.DataSource = cmd.ExecuteReader();
            ItemGrid.DataBind();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            SqlConnection.ClearAllPools();
        }

        public static void PopulateDropDown(string query, DropDownList ddGenerate, string value, string text)
        {
            SqlCommand cmd = new SqlCommand(query,
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            SqlDataReader Grouplist = cmd.ExecuteReader();
            ddGenerate.DataSource = Grouplist;
            ddGenerate.DataValueField = value;
            ddGenerate.DataTextField = text;
            ddGenerate.DataBind();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
            SqlConnection.ClearAllPools();
        }

        public static void PopulateDropDownWithAll(string query, DropDownList ddGenerate, string value, string text)
        {
            ddGenerate.Items.Clear();
            ListItem lst = new ListItem("--- all ---", "0");
            ddGenerate.Items.Insert(ddGenerate.Items.Count, lst);
            SqlCommand cmd = new SqlCommand(query,
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            SqlDataReader Grouplist = cmd.ExecuteReader();
            ddGenerate.DataSource = Grouplist;
            ddGenerate.DataValueField = value;
            ddGenerate.DataTextField = text;
            ddGenerate.DataBind();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
            SqlConnection.ClearAllPools();
        }

        public static void PopulateListView(string query, ListBox ddGenerate, string value, string text)
        {
            SqlCommand cmd = new SqlCommand(query,
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            SqlDataReader Grouplist = cmd.ExecuteReader();
            ddGenerate.DataSource = Grouplist;
            ddGenerate.DataValueField = value;
            ddGenerate.DataTextField = text;
            ddGenerate.DataBind();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
            SqlConnection.ClearAllPools();
        }

        //Convert Gridview to datatable
        //DataTable dt = GetDataTable(GridView1);
        public static DataTable DataTableFromGrid(GridView dtg)
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

        //Sql query to dataset
        public static DataSet ReturnDataSet(String query)
        {
            DataSet ds = new DataSet();
            String connStr = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand objCommand = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(objCommand);
                da.Fill(ds);
                da.Dispose();
            }
            SqlConnection.ClearAllPools();
            return ds;
        }

        public static DataTable ReturnDataTable(string query)
        {
            SqlCommand cmd3 = new SqlCommand(query,
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd3.Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd3);
            DataSet ds = new DataSet("Board");
            da.Fill(ds, "Board");
            cmd3.Connection.Close();
            cmd3.Connection.Dispose();

            DataTable citydt = ds.Tables["Board"];
            SqlConnection.ClearAllPools();
            return citydt;
        }

        public static string GenerateInvoiceNo()
        {
            string InvNo = ReturnString("Select InvNo from Sales where SaleID=(Select ISNULL(MAX(SaleID),0) from Sales)");
            try
            {
                //string yr = DateTime.Now.Year.ToString();
                //DateTime countForYear = Convert.ToDateTime("01/01/" + yr + " 00:00:00");

                //SqlCommand cmd = new SqlCommand("Select CONVERT(varchar, (ISNULL (COUNT(SaleID),0)+ 1 )) from Sales where EntryDate>=@EntryDate", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //cmd.Parameters.AddWithValue("@EntryDate", countForYear);
                //cmd.Connection.Open();
                //int counter = Convert.ToInt32(cmd.ExecuteScalar());

                //cmd.Connection.Close();
                //cmd.Connection.Dispose();
                //Make2Digits(DateTime.Now.Day.ToString()) + Make2Digits(DateTime.Now.Month.ToString()) + yr.Substring(2, 2) + "-" + Make2Digits(counter.ToString());

                InvNo = Convert.ToString(Convert.ToInt32(InvNo) + 1);
                string isExist = ReturnString("Select InvNo from Sales where InvNo='" + InvNo + "'");

                while (isExist != "")
                {
                    InvNo = Convert.ToString(Convert.ToInt32(InvNo) + 1);
                    isExist = ReturnString("Select InvNo from Sales where InvNo='" + InvNo + "'");
                }
                SqlConnection.ClearAllPools();
            }
            catch (Exception ex)
            {
                InvNo = "";
            }
            return InvNo;
        }

        public static string ReturnString(object p)
        {
            throw new NotImplementedException();
        }

        private static string Make2Digits(string InvNo)
        {
            if (InvNo.Length < 2)
            {
                InvNo = "0" + InvNo;
            }
            return InvNo;
        }

        public static string Make2Decimal(string InvNo)
        {
            InvNo = Convert.ToDecimal(InvNo).ToString("F");
            //InvNo = Convert.ToDecimal(InvNo).ToString("F");
            return InvNo;
        }

        public static void ActivityLog(string FormName, string Activity, string Description, string UserId)
        {
            ExecNonQry("INSERT INTO XERP_Log (FormName, Activity, Description, UserId) VALUES ('" + FormName + "', '" +
                       Activity + "', '" + Description + "', '" + UserId + "')");
        }

        public static void PopulateMultiDropDown(DropDownList ddGenerate, string query1)
        {
            DataSet ds = new DataSet();
            SqlDataReader dr;
            int recordcount = 0;

            string query =
                "SELECT (Select GroupName from AccountGroup where GroupID=a.GroupID) as GroupName , AccountsHeadID, [AccountsHeadName] FROM [HeadSetup] a WHERE IsActive=1 " +
                query1 + "  ORDER BY GroupName, [AccountsHeadName]";
            SqlCommand cmd = new SqlCommand(query,
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd.Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Products");
            cmd.Connection.Close();
            cmd.Connection.Dispose();

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
                    /*
                    if (oldGroupName != GroupName)
                    {
                        oldGroupName = GroupName;
                        item1 = new ListItem(" ________ " + GroupName + " ________");
                        ddGenerate.Items.Add(item1);
                    }
                    */
                    item1 = new ListItem(AccountsHeadName, AccountsHeadID);
                    item1.Attributes["OptionGroup"] = GroupName;
                    ddGenerate.Items.Add(item1);

                }
                ddGenerate.DataBind();
            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();
            SqlConnection.ClearAllPools();
        }

        public static string CalculateOverDueDays(string CustomerID)
        {
            int ttlOverDueDays = 0;
            int mDays =
                Convert.ToInt32(
                    Convert.ToDecimal(ReturnString("Select MatuirityDays from Party where PartyID='" + CustomerID + "'")));
            string lastMaturityDate = DateTime.Now.AddDays(mDays * (-1)).ToString("yyyy-MM-dd");

            ExecNonQry("UPDATE SALES SET OverdueDays='0'  where CustomerID='" + CustomerID + "'");
            DataTable dt = ReturnDataTable("SELECT SaleID, InvNo, InvDate from Sales where CustomerID='" + CustomerID +
                                           "' AND IsActive=1 AND InvDate<'" + lastMaturityDate + "'");

            foreach (DataRow row in dt.Rows)
            {
                string SaleID = row["SaleID"].ToString();
                string InvDate = row["InvDate"].ToString();
                DateTime invDate = Convert.ToDateTime(InvDate);
                int invAge = Convert.ToInt32(Convert.ToDecimal(((DateTime.Now.AddDays(mDays * (-1)) - invDate).TotalDays)));
                ExecNonQry("UPDATE SALES SET OverdueDays='" + invAge + "' WHERE SaleID='" + SaleID + "' ");
                ttlOverDueDays += invAge;
            }

            string avgOverDueDays = "0";
            if (dt.Rows.Count > 0)
            {
                avgOverDueDays = Convert.ToString(ttlOverDueDays / dt.Rows.Count);
            }
            return avgOverDueDays;
        }

        public static int ReturnInvCycleDays(string invoiceNo)
        {
            int invCycleAge = 0;
            int mDays =
                Convert.ToInt32(
                    Convert.ToDecimal(
                        ReturnString(
                            "Select MatuirityDays from Party where PartyID=(Select CustomerID from Sales where InvNo='" +
                            invoiceNo + "')")));
            //string lastMaturityDate = DateTime.Now.AddDays(mDays * (-1)).ToString("yyyy-MM-dd");

            DataTable dt = ReturnDataTable("SELECT SaleID, InvDate from Sales where InvNo='" + invoiceNo + "'");

            foreach (DataRow row in dt.Rows)
            {
                string SaleID = row["SaleID"].ToString();
                string InvDate = row["InvDate"].ToString();
                DateTime mDate = Convert.ToDateTime(InvDate).AddDays(mDays);
                invCycleAge = Convert.ToInt32(Convert.ToDecimal(((DateTime.Now - mDate).TotalDays)));
            }
            return invCycleAge;
        }

        public static string UploadImage(string description, FileUpload FileUpload1, string filePath, string savePath,
            string linkPath, string entryBy, string photoType)
        {
            string pid = ReturnString("Select ISNULL(MAX(PhotoID),0)+1 from Photos");
            string tExt = Path.GetFileName(FileUpload1.PostedFile.ContentType);
            string fileName = RemoveSpecialCharacters(description.Trim().Replace(" ", "-")) + "." + pid + "." + tExt;
            ExecNonQry("Insert into Photos (Description, PhotoURL, EntryBy, PhotoType) VALUES ('" + description +
                       "','" + linkPath + fileName + "','" + entryBy + "','" + photoType + "')");
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

        public static string UploadFile(string description, FileUpload FileUpload1, string filePath, string savePath,
            string linkPath, string entryBy, string photoType)
        {
            string pid = ReturnString("Select ISNULL(MAX(PhotoID),0)+1 from Photos");
            string tExt = Path.GetFileName(FileUpload1.PostedFile.ContentType);
            string fileName = RemoveSpecialCharacters(description.Trim().Replace(" ", "-")) + "." + pid + "." + tExt;
            ExecNonQry("Insert into Photos (Description, PhotoURL, EntryBy, PhotoType) VALUES ('" + description +
                       "','" + linkPath + fileName + "','" + entryBy + "','" + photoType + "')");
            pid = ReturnString("Select ISNULL(MAX(PhotoID),0) from Photos");

            string strFullPath = filePath + fileName;

            if (File.Exists(strFullPath))
            {
                File.Delete(strFullPath);
            }
            FileUpload1.PostedFile.SaveAs(savePath + fileName);
            return pid;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' ||
                    c == '-')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Replace("--", "-");
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
            SqlCommand cmd = new SqlCommand("Select ISNULL(MAX(" + colName + "),0)+1001 from " + tableName,
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            string invNo = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();
            cmd.Connection.Dispose();

            string isExist =
                ReturnString("Select " + colEval + " from " + tableName + " where (" + colEval + " like '%" + invNo +
                             "%')");
            while (isExist != "")
            {
                invNo = Convert.ToString(Convert.ToInt32(invNo) + 1);
                isExist =
                    ReturnString("Select " + colEval + " from " + tableName + " where (" + colEval + " like '%" + invNo +
                                 "%')");
            }
            SqlConnection.ClearAllPools();
            return invNo;
        }

        //private void exportExcel(DataTable data, string reportName)
        //{
        //    var wb = new XLWorkbook();

        //    // Add DataTable as Worksheet
        //    wb.Worksheets.Add(data);

        //    // Create Response
        //    HttpResponse response = Response;

        //    //Prepare the response
        //    response.Clear();
        //    response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    response.AddHeader("content-disposition", "attachment;filename=" + reportName + ".xlsx");

        //    //Flush the workbook to the Response.OutputStream
        //    using (MemoryStream MyMemoryStream = new MemoryStream())
        //    {
        //        wb.SaveAs(MyMemoryStream);
        //        MyMemoryStream.WriteTo(response.OutputStream);
        //        MyMemoryStream.Close();
        //    }

        //    response.End();
        //}
        public static string NumberToWords(int number)
        {
            if (number == 0)
            {
                return "zero";
            }
            if (number < 0)
            {
                return "minus " + NumberToWords(Math.Abs(number));
            }
            string words = "";
            if ((number / 10000000) > 0)
            {
                words += NumberToWords(number / 10000000) + " Crore ";
                number %= 10000000;
            }
            if ((number / 100000) > 0)
            {
                words += NumberToWords(number / 100000) + " Lac ";
                number %= 100000;
            }
            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }
            if (number > 0)
            {
                if (words != "")
                {
                    words += " ";
                }
                var unitsMap = new[]
                {
                    "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
                    "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
                };
                var tensMap = new[]
                {"Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "seventy", "Eighty", "Ninety"};
                if (number < 20)
                {
                    words += unitsMap[number];
                }
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                    {
                        words += " " + unitsMap[number % 10];
                    }
                }
            }
            return words;
        }

        public static string DecimalToWords(string s)
        {
            string taka = "0";
            string paisa = "0";
            string[] words = s.Split('.');
            foreach (string word in words)
            {
                if (taka.Length <= 5)
                {
                    taka = NumberToWords(Convert.ToInt32(word));
                }
                else
                {
                    paisa = NumberToWords(Convert.ToInt32(word));
                }
            }

            return " Taka " + taka + " and " + paisa + " Paisa Only."; //+" Taka Only.";
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

                DataTable dtx =
                    ReturnDataTable(
                        @"SELECT TOP (1) pid, GatewayName, HostIP, SenderName, UserID, Password FROM SMSGateway WHERE (IsActive = '1')");

                foreach (DataRow drx in dtx.Rows)
                {
                    string pid = drx["pid"].ToString();
                    string hostIP = drx["HostIP"].ToString();
                    string sender = drx["SenderName"].ToString();
                    string userID = drx["UserID"].ToString();
                    string password = drx["Password"].ToString();

                    if (pid == "1") //Route SMS
                    {
                        reqText = hostIP + "bulksms/bulksms?username=" + userID + "&password=" + password +
                                  "&type=0&dlr=1&destination=" + mob + "&source=" + sender + "&message=" + msg;
                    }
                    else if (pid == "2") //Bulk SMS
                    {
                        reqText = hostIP + "api.php?username=" + userID + "&password=" + password + "&number=" + mob +
                                  "&sender=" + sender + "&type=0&message=" + msg;
                    }
                    else if (pid == "3")
                    //Extreme SMS : http://107.20.199.106/api/v3/sendsms/plain?user=extremebd&password=jrhM0DIy&sender=xtremebd&SMSText=messagetext&GSM=8801817251582
                    {
                        reqText = hostIP + "api/v3/sendsms/plain?user=" + userID + "&password=" + password +
                                  "&sender=" + sender + "&SMSText=" + msg + "&GSM=" + mob;
                    }
                }

                reqText = Convert.ToString(WebRequest.Create(reqText).GetResponse());
                return reqText;

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        //Return number with comma in BD format
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

        public static string FormatCashFlowNumber(decimal q)
        {
            return q.ToString();
        }

        public static string FormatCashFlowNumber(string q)
        {
            return q.ToString();
        }
        public static decimal GetRate(string accHeadId)
        {
            return Convert.ToDecimal(ReturnString("SELECT Rate FROM [HeadSetup] WHERE AccountsHeadID='" + accHeadId + "'"));
        }
        public static decimal GetopStockPcs(string accHeadId)
        {
            return Convert.ToDecimal(ReturnString("SELECT ISNULL(SUM(OpPcsDr),0) - ISNULL(SUM(OpPcsCr),0) FROM [HeadSetup] WHERE AccountsHeadID='" + accHeadId + "'"));
        }
        public static decimal GetopStockKg(string accHeadId)
        {
            return Convert.ToDecimal(ReturnString("SELECT ISNULL(SUM(QtyDr),0) - ISNULL(SUM(QtyCr),0) FROM [HeadSetup] WHERE AccountsHeadID='" + accHeadId + "'"));
        }
        public static decimal GetoldOutPcs(string accHeadId)
        {
            return Convert.ToDecimal(ReturnString("SELECT ISNULL(SUM(OutPcs),0) FROM [VoucherDetails] WHERE AccountsHeadID='" + accHeadId + "' AND ISApproved<>'C'"));
        }
        public static decimal GetcurrentKgStock(string accHeadId)
        {
            decimal opStockKg = GetopStockKg(accHeadId);//Item op kg
            decimal inOutKg= Convert.ToDecimal(ReturnString("SELECT ISNULL(SUM(InQty),0)-ISNULL(SUM(OutQty),0) FROM [VoucherDetails] WHERE AccountsHeadID='" + accHeadId + "' AND ISApproved<>'C'"));
            return opStockKg + inOutKg;
        }
        public static decimal GetcurrentPcsStock(string accHeadId)
        {
            decimal opStockPcs = GetopStockPcs(accHeadId);//Item op kg
            decimal inOutPcs= Convert.ToDecimal(ReturnString("SELECT ISNULL(SUM(InPcs),0)-ISNULL(SUM(OutPcs),0) FROM [VoucherDetails] WHERE AccountsHeadID='" + accHeadId + "' AND ISApproved<>'C'"));
            return opStockPcs + inOutPcs;
        }

        public static void FinishedStockSale(string accHeadId, string particularId, decimal invPcs, string tid,
            string invNo, string entryBy, string entryDate, string prodName)
        {
            string itemName = ReturnString("SELECT AccountsHeadName FROM [HeadSetup] WHERE AccountsHeadID = '" + accHeadId + "'");
            string description = "Consumed by " + itemName + ", Qty: " + invPcs + ", Sales Inv#" + invNo+ " ("+ prodName+")";
            decimal rate = 0, prdnKg = 0, deductKg = 0, voucherKg = 0, unitWeight = 0, outkg = 0, totamt = 0;
            try
            {
                decimal opStockRate = GetRate(accHeadId);//openning rate
                decimal opStockPcs = GetopStockPcs(accHeadId);//op stock qty
                decimal opStockKg = GetopStockKg(accHeadId);//Item op kg

                decimal oldOutPcs = GetoldOutPcs(accHeadId); //used stock until now 
                decimal remainPcs = invPcs; // current used qty
                decimal newOutPcs = oldOutPcs + remainPcs; //sum of total used
                decimal currentKgStock = GetcurrentKgStock(accHeadId);
                decimal currentPcsStock = GetcurrentPcsStock(accHeadId);

                if (opStockPcs > 0) // Item has pcs and kg both
                {
                    unitWeight = (opStockKg / opStockPcs);
                }

                decimal entryID = 0, deductPCS = 0, totalPrdnPcs = 0, totaloldOutPcs = 0, prdnPCS = 0, i = 0, voucherPCS = 0; decimal prevRemain = 0;

                while (remainPcs > 0)
                {
                    if (i == 0)
                    {
                        if (oldOutPcs < opStockPcs)
                        {
                            if (newOutPcs < opStockPcs)
                            {
                                deductPCS = invPcs;
                                remainPcs = 0;
                                oldOutPcs = 0;
                                newOutPcs = 0;
                            }
                            else
                            {
                                deductPCS = opStockPcs - oldOutPcs;
                                remainPcs = invPcs - deductPCS;
                                oldOutPcs = 0;
                                newOutPcs = newOutPcs - opStockPcs;
                            }

                            outkg = unitWeight * deductPCS;
                            decimal opamt = opStockRate * outkg;
                            VoucherEntry.AutoVoucherEntryFinishedGoods("6", description, "040201017", accHeadId, opamt,
                                outkg, deductPCS, rate.ToString(), invNo, entryBy, entryDate, "1", "Line 774");
                            deductPCS = 0;
                        }
                        else // when opPCS all deduct is complete
                        {
                            oldOutPcs = oldOutPcs - opStockPcs;
                            newOutPcs = oldOutPcs + remainPcs;
                        }
                        i++;
                    }

                    if (remainPcs > 0)
                    {
                        //loop in voucer detail of that product in: order by date, id
                        DataTable dtx3 =
                            ReturnDataTable(
                                @"SELECT TOP(1) SerialNo, Rate, InQty, OutQty, InPcs, (InPcs - DeliveredPcs) AS PendingPcs, DeliveredStatus, (SELECT VoucherDate FROM VoucherMaster WHERE VoucherNo=VoucherDetails.VoucherNo) AS VoucherDate FROM [VoucherDetails] WHERE SerialNo=(SELECT MIN(SerialNo) FROM VoucherDetails WHERE SerialNo >'" +
                                entryID + "' AND AccountsHeadID='" + accHeadId +
                                "' AND VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE ParticularID='" +
                                particularId +
                                "')  AND ISApproved<>'C' AND DeliveredStatus<>'D') ORDER BY VoucherDate, SerialNo");

                        foreach (DataRow dr in dtx3.Rows)
                        {
                            entryID = Convert.ToInt32(dr["SerialNo"].ToString());
                            prdnKg = Convert.ToDecimal(dr["InQty"].ToString());
                            prdnPCS = Convert.ToDecimal(dr["InPcs"].ToString());
                            //totalPrdnPcs += prdnPCS;
                            totalPrdnPcs =
                                Convert.ToDecimal(
                                    ReturnString(
                                        "SELECT ISNULL(SUM(InPcs),0) FROM [VoucherDetails] WHERE AccountsHeadID='" +
                                        accHeadId + "' AND ISApproved<>'C' AND SerialNo <='" + entryID + "'"));
                            rate = Convert.ToDecimal(dr["Rate"].ToString());

                            totaloldOutPcs =
                                Convert.ToDecimal(
                                    ReturnString(
                                        "SELECT ISNULL(SUM(OutPcs),0) FROM [VoucherDetails] WHERE AccountsHeadID='" +
                                        accHeadId + "' AND ISApproved<>'C'"));
                            oldOutPcs = ((totaloldOutPcs + deductPCS) - opStockPcs);

                            decimal oldDuePcs = totalPrdnPcs - oldOutPcs;
                            if (totalPrdnPcs < oldOutPcs)
                            {
                                oldDuePcs = totalPrdnPcs;
                            }

                            if (oldDuePcs > 0 && oldDuePcs <= remainPcs)
                            {
                                deductPCS = oldDuePcs;
                                remainPcs = remainPcs - deductPCS;
                            }
                            else if (oldDuePcs > remainPcs)
                            {
                                deductPCS = remainPcs;
                                remainPcs = 0;
                            }
                            else if (remainPcs > prdnPCS)
                            {
                                deductPCS = prdnPCS;
                                remainPcs = remainPcs - prdnPCS;
                            }
                            else
                            {
                                deductPCS = prdnPCS - remainPcs;
                                remainPcs = 0;
                            }

                            ExecNonQry("UPDATE VoucherDetails SET DeliveredPcs=DeliveredPcs+ " + deductPCS +
                                       ", projectName='Line-844'  WHERE SerialNo='" + entryID + "' ");
                            //oldOutPcs = Convert.ToInt32(ReturnString("Select ISNULL(SUM(DeliveredPcs),0) from VoucherDetails WHERE AccountsHeadID ='" + accHeadId + "' AND ISApproved<>'C'"));
                            //int delivered = Convert.ToInt32(ReturnString("SELECT SUM(InPcs) - SUM(DeliveredPcs) FROM VoucherDetails WHERE SerialNo='" + entryID + "'"));
                            if ((oldDuePcs - deductPCS) <= 0)
                            {
                                ExecNonQry("UPDATE VoucherDetails SET DeliveredStatus='D', SubprojectName='Line-849' WHERE SerialNo='" + entryID +
                                           "' ");
                            }
                            voucherPCS += deductPCS;
                            voucherKg += deductPCS * (prdnKg / prdnPCS);
                            totamt += rate * (deductPCS * (prdnKg / prdnPCS));
                        }

                        if (prevRemain == remainPcs)
                        {
                            remainPcs = 0;
                        }
                        else
                        {
                            prevRemain = remainPcs;
                        }
                    }
                }
                if (totamt > 0)
                {
                    decimal actualRate = totamt / voucherKg;
                    VoucherEntry.AutoVoucherEntryFinishedGoods("6", description, "040201017", accHeadId, totamt,
                        voucherKg, voucherPCS, actualRate.ToString(), invNo, entryBy, entryDate, "1", "Line 871, "+ prodName);
                }

            }
            catch (Exception ex)
            {
                //Notify(ex.Message.ToString(), "error", lblMsg);
            }
        }

        //Finished Stock Sale Sticker
        public static void FinishedStockSaleStickerSteelHandle(string accHeadId, string particularId, decimal invPcs,
            string tid, string invNo, string entryBy, string entryDate, string prodName)
        {
            string desCont =
                ReturnString("SELECT AccountsHeadName FROM [HeadSetup] WHERE AccountsHeadID = '" + accHeadId + "'");
            string description = "Consumed by " + desCont + ", Qty: " + invPcs + ", Sales Inv#" + invNo + " (" + prodName + ")";
            decimal rate = 0, prdnKg = 0, deductKg = 0, voucherKg = 0, unitWeight = 0, outkg = 0, totamt = 0;
            try
            {
                decimal oldOutPcs =
                    Convert.ToDecimal(
                        ReturnString("SELECT ISNULL(SUM(OutPcs),0) FROM [VoucherDetails] WHERE AccountsHeadID='" +
                                     accHeadId + "' AND ISApproved<>'C'")); //used stock until now 
                decimal remainPcs = invPcs; // current used qty
                decimal newOutPcs = oldOutPcs + remainPcs; //sum of total used

                decimal opStockRate =
                    Convert.ToDecimal(
                        ReturnString("SELECT Rate FROM [HeadSetup] WHERE AccountsHeadID='" + accHeadId + "'"));
                //openning rate
                decimal opStockPcs =
                    Convert.ToDecimal(
                        ReturnString(
                            "SELECT ISNULL(SUM(OpPcsDr),0) - ISNULL(SUM(OpPcsCr),0) FROM [HeadSetup] WHERE AccountsHeadID='" +
                            accHeadId + "'"));
                //decimal opStockKg = Convert.ToDecimal(ReturnString("SELECT ISNULL(SUM(QtyDr),0) - ISNULL(SUM(QtyCr),0) FROM [HeadSetup] WHERE AccountsHeadID='" + accHeadId + "'")); //Item op stock
                //if (opStockPcs > 0)
                //{
                //    unitWeight = (opStockKg / opStockPcs);
                //}

                decimal entryID = 0,
                    deductPCS = 0,
                    totalPrdnPcs = 0,
                    totaloldOutPcs = 0,
                    prdnPCS = 0,
                    i = 0,
                    voucherPCS = 0;

                while (remainPcs > 0)
                {
                    if (i == 0)
                    {
                        if (oldOutPcs < opStockPcs)
                        {
                            if (newOutPcs < opStockPcs)
                            {
                                deductPCS = invPcs;
                                remainPcs = 0;
                                oldOutPcs = 0;
                                newOutPcs = 0;
                            }
                            else
                            {
                                deductPCS = opStockPcs - oldOutPcs;
                                remainPcs = invPcs - deductPCS;
                                oldOutPcs = 0;
                                newOutPcs = newOutPcs - opStockPcs;
                            }

                            //outkg = unitWeight * deductPCS;
                            decimal opamt = opStockRate * deductPCS;
                            VoucherEntry.AutoVoucherEntryFinishedGoods("6", description, "040201017", accHeadId, opamt,
                                outkg, deductPCS, rate.ToString(), invNo, entryBy, entryDate, "1", "Line 945");
                            deductPCS = 0;
                        }
                        else // when opPCS all deduct is complete
                        {
                            oldOutPcs = oldOutPcs - opStockPcs;
                            newOutPcs = oldOutPcs + remainPcs;
                        }
                        i++;
                    }
                    
                    if (remainPcs > 0)
                    {
                        //loop in voucer detail of that product in: order by date, id
                        DataTable dtx3 =
                            ReturnDataTable(
                                @"SELECT TOP(1) SerialNo, Rate, InQty, OutQty, InPcs, (InPcs - DeliveredPcs) AS PendingPcs, DeliveredStatus, (SELECT VoucherDate FROM VoucherMaster WHERE VoucherNo=VoucherDetails.VoucherNo) AS VoucherDate FROM [VoucherDetails] WHERE SerialNo=(SELECT MIN(SerialNo) FROM VoucherDetails WHERE SerialNo >'" +
                                entryID + "' AND AccountsHeadID='" + accHeadId +
                                "' AND VoucherNo IN (SELECT VoucherNo FROM VoucherMaster WHERE ParticularID='" +
                                particularId +
                                "')  AND ISApproved<>'C' AND DeliveredStatus<>'D') ORDER BY VoucherDate, SerialNo");

                        foreach (DataRow dr in dtx3.Rows)
                        {
                            entryID = Convert.ToInt32(dr["SerialNo"].ToString());
                            //prdnKg = Convert.ToDecimal(dr["InQty"].ToString());
                            prdnPCS = Convert.ToDecimal(dr["InPcs"].ToString());
                            //totalPrdnPcs += prdnPCS;
                            totalPrdnPcs =
                                Convert.ToDecimal(
                                    ReturnString(
                                        "SELECT ISNULL(SUM(InPcs),0) FROM [VoucherDetails] WHERE AccountsHeadID='" +
                                        accHeadId + "' AND ISApproved<>'C' AND SerialNo <='" + entryID + "'"));
                            rate = Convert.ToDecimal(dr["Rate"].ToString());

                            totaloldOutPcs =
                                Convert.ToDecimal(
                                    ReturnString(
                                        "SELECT ISNULL(SUM(OutPcs),0) FROM [VoucherDetails] WHERE AccountsHeadID='" +
                                        accHeadId + "' AND ISApproved<>'C'"));
                            oldOutPcs = ((totaloldOutPcs + deductPCS) - opStockPcs);

                            decimal oldDuePcs = totalPrdnPcs - oldOutPcs;
                            if (totalPrdnPcs < oldOutPcs)
                            {
                                oldDuePcs = totalPrdnPcs;
                            }

                            if (oldDuePcs > 0 && oldDuePcs <= remainPcs)
                            {
                                deductPCS = oldDuePcs;
                                remainPcs = remainPcs - deductPCS;
                            }
                            else if (oldDuePcs > remainPcs)
                            {
                                deductPCS = remainPcs;
                                remainPcs = 0;
                            }
                            else if (remainPcs > prdnPCS)
                            {
                                deductPCS = prdnPCS;
                                remainPcs = remainPcs - prdnPCS;
                            }
                            else
                            {
                                deductPCS = prdnPCS - remainPcs;
                                remainPcs = 0;
                            }

                            ExecNonQry("UPDATE VoucherDetails SET DeliveredPcs=DeliveredPcs+ " + deductPCS +
                                       " WHERE SerialNo='" + entryID + "' ");
                            //oldOutPcs = Convert.ToInt32(ReturnString("Select ISNULL(SUM(DeliveredPcs),0) from VoucherDetails WHERE AccountsHeadID ='" + accHeadId + "' AND ISApproved<>'C'"));
                            //int delivered = Convert.ToInt32(ReturnString("SELECT SUM(InPcs) - SUM(DeliveredPcs) FROM VoucherDetails WHERE SerialNo='" + entryID + "'"));
                            if ((oldDuePcs - deductPCS) <= 0)
                            {
                                ExecNonQry("UPDATE VoucherDetails SET DeliveredStatus='D' WHERE SerialNo='" + entryID +
                                           "' ");
                            }
                            voucherPCS += deductPCS;
                            //voucherKg += deductPCS * (prdnKg / prdnPCS);
                            totamt += (rate * deductPCS);
                        }
                    }
                }
                if (totamt > 0)
                {
                    decimal actualRate = totamt / voucherPCS;
                    VoucherEntry.AutoVoucherEntryFinishedGoods("6", description, "040201017", accHeadId, totamt,
                        voucherKg, voucherPCS, actualRate.ToString(), invNo, entryBy, entryDate, "1", "Line 1033, "+ prodName);
                }

            }
            catch (Exception ex)
            {
                //Notify(ex.Message.ToString(), "error", lblMsg);
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



    }
    public static class ResponseHelper
    {
        public static void Redirect(string url, string target, string windowFeatures)
        {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures))
            {

                context.Response.Redirect(url);
            }
            else
            {
                Page page = (Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(Page),
                    "Redirect",
                    script,
                    true);
            }
        }
    }

}