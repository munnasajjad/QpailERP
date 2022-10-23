using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using RunQuery;

namespace Oxford.XerpReports
{
    public partial class DailyPrdnAllShift : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadGridData();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        private void LoadGridData()
        {
            //string lName = Page.User.Identity.Name.ToString();
            //string prjID = "1";
            SQLQuery.ExecNonQry("DELETE  FROM DailyPrdnAllShift");
            string machineNo = Convert.ToString(Request.QueryString["machineNo"]);
            string type = Convert.ToString(Request.QueryString["type"]);
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);

            DataTable dt2 = BindItemGrid(dateFrom, dateTo, type);
            DataTable dt1 = BindPrdnGridAll("",dateFrom, dateTo, type);
            DataTableReader dr2 = dt1.CreateDataReader();
            XerpDataSet dsx = new XerpDataSet();
            //dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.DailyPrdnAllShift);

            if (type == "AllShift")
            {
                //Sub report
                //DataTable dt1x = SQLQuery.ReturnDataTable(@"SELECT (SELECT BrandName  FROM Brands  WHERE (BrandID = a.PackSize)) AS PackSize,     (SELECT ItemName    FROM Products    WHERE (ProductID = a.ItemID)) AS Product, SUM(FinalProduction) AS PrdnQty, SUM(FinalKg) AS RMCons, SUM(Rejection) AS RejQty FROM PrdPlasticCont AS a WHERE (ActProduction > 0) AND (Date >='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ) AND (Date <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "') GROUP BY PackSize, ItemID Order by ItemID");
                //DataTableReader dr2x = dt1x.CreateDataReader();
                ////XerpDataSet dsxx = new XerpDataSet();
                //dsx.Load(dr2x, LoadOption.OverwriteChanges, dsx.PrdnSummery);


                DataTable dt1x = SQLQuery.ReturnDataTable(@"SELECT    ShiftsA, MachineNoA, Machine, ProductName, ColorA, CycleTimeA, RunningA, ProjectedA, ActualA, WastageA, WeightA, WastePercentA, ShiftsB, CycleTimeB, RunningB, ProjectedB, ActualB, 
                            WastageB, WeightB, WastePercentB, ShiftsC, CycleTimeC, RunningC, ProjectedC, ActualC, WastageC, WeightC, WastePercentC, RunningT, ProjectedT, ActualT, WastageT, WeightT, WastePercentT
                            FROM DailyPrdnAllShift  ORDER BY MachineNoA");
                DataTableReader dr2x = dt1x.CreateDataReader();
                //XerpDataSet dsxx = new XerpDataSet();
                dsx.Load(dr2x, LoadOption.OverwriteChanges, dsx.DailyPrdnAllShift);

                rpt.Load(Server.MapPath("CrptDailyPrdnAllShift.rpt"));
            }
            //else
            //{
            //    rpt.Load(Server.MapPath("rptDailyPrdnSheet.rpt"));
            //}
            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");

            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx);
            SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            CrystalReportViewer1.ReportSource = rpt;
        }

        private DataTable BindPrdnGridAll(string machineNo, string dateFrom, string dateTo, string type)
        {
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            //dt1.Columns.Add("ShiftsA");
            //dt1.Columns.Add("Machine");
            //dt1.Columns.Add("ProductName");
            //dt1.Columns.Add("ColorA");
            //dt1.Columns.Add("MachineNoA");
            //dt1.Columns.Add("CycleTimeA");
            //dt1.Columns.Add("RunningA");
            //dt1.Columns.Add("ProjectedA");
            //dt1.Columns.Add("ActualA");
            //dt1.Columns.Add("wastageA");
            //dt1.Columns.Add("WeightA");

            //dt1.Columns.Add("ShiftsB");
            //dt1.Columns.Add("ColorB");
            //dt1.Columns.Add("CycleTimeB");
            //dt1.Columns.Add("RunningB");
            //dt1.Columns.Add("ProjectedB");
            //dt1.Columns.Add("ActualB");
            //dt1.Columns.Add("wastageB");
            //dt1.Columns.Add("WeightB");

            //dt1.Columns.Add("ShiftsC");
            //dt1.Columns.Add("ColorC");
            //dt1.Columns.Add("CycleTimeC");
            //dt1.Columns.Add("RunningC");
            //dt1.Columns.Add("ProjectedC");
            //dt1.Columns.Add("ActualC");
            //dt1.Columns.Add("wastageC");
            //dt1.Columns.Add("WeightC");


            //dt1.Columns.Add("RunningT");
            //dt1.Columns.Add("ProjectedT");
            //dt1.Columns.Add("ActualT");
            //dt1.Columns.Add("wastageT");
            //dt1.Columns.Add("WeightT");


            string query1st = " (", query2nd = " (", query3rd = " (";
            int i = 0, i2 = 0, i3 = 0;
            int is2ndShift = 0;
            string prevSection = "";
            DataTable dt1xy = SQLQuery.ReturnDataTable(@"SELECT Departmentid, DepartmentName, Description, EntryBy, EntryDate, ProjectID, Section from Shifts WHERE Departmentid IN (Select Shift from PrdPlasticCont
                WHERE ((PrdPlasticCont.Date >='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ) AND (PrdPlasticCont.Date <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "'))) Order by Section, Departmentid");
            foreach (DataRow drx3 in dt1xy.Rows)
            {
                string section = drx3["Section"].ToString();
                if (prevSection != section)
                {
                    if (i > 0)
                    {
                        query1st += " OR ";
                    }

                    query1st += "Shift='" + drx3["Departmentid"] + "'";
                    i++;
                    prevSection = section;
                    is2ndShift = 1;
                }
                else if (is2ndShift == 1)
                {
                    if (i2 > 0)
                    {
                        query2nd += " OR ";
                    }

                    query2nd += "Shift='" + drx3["Departmentid"] + "'";
                    i2++;
                    is2ndShift = 0;
                }
                else
                {
                    if (i3 > 0)
                    {
                        query3rd += " OR ";
                    }

                    query3rd += "Shift='" + drx3["Departmentid"] + "'";
                    i3++;
                }

            }
            query1st += ") ";
            query2nd += ") ";
            query3rd += ") ";


            DataTable dt1x = SQLQuery.ReturnDataTable(@"SELECT DISTINCT Machines.mid, Machines.MachineNo, Machines.Description
                FROM            Machines INNER JOIN PrdPlasticCont ON Machines.mid = PrdPlasticCont.MachineNo
                WHERE        (PrdPlasticCont.Date >='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ) AND (PrdPlasticCont.Date <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "') Order by Machines.mid");

            foreach (DataRow drx3 in dt1x.Rows)
            {
                DataTable dt3 = SQLQuery.ReturnDataTable(@"SELECT DISTINCT ItemID, (SELECT BrandName FROM Brands WHERE (BrandID = PrdPlasticCont.PackSize)) + ' ' + (SELECT ItemName FROM Products WHERE (ProductID = PrdPlasticCont.ItemID)) + ' ' +
                         (SELECT BrandName FROM CustomerBrands WHERE (BrandID = PrdPlasticCont.Brand)) AS ProductName, Brand, PackSize, (SELECT DepartmentName from Colors WHERE Departmentid=PrdPlasticCont.Color) as Color FROM PrdPlasticCont
                WHERE    (PrdPlasticCont.Date >='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ) AND (PrdPlasticCont.Date <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "' AND (PrdPlasticCont.MachineNo = '" + drx3["mid"] + "')) Order by ProductName");

                foreach (DataRow drx4 in dt3.Rows)
                {

                    SQLQuery.ExecNonQry("INSERT INTO [DailyPrdnAllShift] ([MachineNoA], [Machine], [ProductID], [ProductName], [BrandID], [PackSizeID], [ColorA])" +
                                                         " VALUES ('" + drx3["MachineNo"] + "', '" + drx3["Description"] + "', '" + drx4["ItemID"] + "', '" + drx4["ProductName"] + "', '" + drx4["Brand"] + "', '" + drx4["PackSize"] + "','" + drx4["Color"] + "')");

                    //dr1 = dt1.NewRow();
                    //1st shift

                    if (query1st.Trim() == "()")
                    {
                        query1st = "Shift=0";
                    }

                    DataTable dt41 = SQLQuery.ReturnDataTable(@"SELECT (SELECT 'Shift - ' + DepartmentName FROM Shifts WHERE (Departmentid = PrdPlasticCont.Shift)) AS Shift, 
                         (Select Description from Machines WHERE mid=PrdPlasticCont.MachineNo) AS Machine, (Select MachineNo from Machines WHERE mid=PrdPlasticCont.MachineNo) AS MachineNo,
                         AVG(CONVERT(decimal, PrdPlasticCont.CycleTime)) AS CycleTime, (SUM(PrdPlasticCont.WorkingHour) + SUM(PrdPlasticCont.WorkingMin) / 60) AS Running, SUM(PrdPlasticCont.CalcProduction) AS Projected, 
                         SUM(PrdPlasticCont.FinalProduction) AS Actual, SUM(PrdPlasticCont.Rejection) AS wastage, SUM(CalcProduction) AS CalcProduction, SUM(ActProduction) AS ActProduction,
                         SUM(Rejection) AS Rejection, SUM(NetProduction) AS NetProduction, SUM(ItemWeight) AS ItemWeight, SUM(TotalWeight) AS TotalWeight, SUM(ActualWeight) AS ActualWeight, ItemID AS ProductID,
                         (SELECT BrandName FROM Brands WHERE (BrandID = PrdPlasticCont.PackSize)) + ' ' + (SELECT ItemName FROM Products WHERE (ProductID = PrdPlasticCont.ItemID)) + ' ' +
                         (SELECT BrandName FROM CustomerBrands WHERE (BrandID = PrdPlasticCont.Brand)) AS ProductName, (SELECT DepartmentName from Colors WHERE Departmentid=PrdPlasticCont.Color) as Color,
                         SUM(FinalProduction)  AS FinalProduction, SUM(PrdPlasticCont.FinalKg) AS Weight, SUM(WasteWeight) AS WasteWeight, SUM(NonusableWeight) AS NonusableWeight, Brand, PackSize
                         FROM PrdPlasticCont WHERE " + query1st + " AND (PrdPlasticCont.Date >='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ) AND (PrdPlasticCont.Date <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "' AND (PrdPlasticCont.ItemID = '" + drx4["ItemID"] + "')  AND (PrdPlasticCont.MachineNo = '" + drx3["mid"] + "')) GROUP BY Shift, MachineNo, ItemID, PackSize, Brand, Color ");

                    if (dt41.Rows.Count > 0)
                    {
                        foreach (DataRow drx01 in dt41.Rows)
                        {
                            string pid = SQLQuery.ReturnString("Select pid FROM DailyPrdnAllShift WHERE MachineNoA='" + drx01["MachineNo"] + "' AND ProductID='" + drx01["ProductID"] + "' AND ColorA='" + drx01["Color"] + "' AND BrandID='" + drx01["Brand"] + "' AND PackSizeID='" + drx01["PackSize"] + "'");
                            if (pid != "")
                            {
                                SQLQuery.ExecNonQry("UPDATE DailyPrdnAllShift SET ShiftsA='" + drx01["Shift"] + "', CycleTimeA='" + drx01["CycleTime"] + "', RunningA='" + enpty2zero(drx01["Running"].ToString()) + "', ProjectedA='" + drx01["Projected"] + "', ActualA='" + drx01["Actual"] + "', WastageA='" + drx01["wastage"] + "', WeightA='" + drx01["Weight"] + "' WHERE pid='" + pid + "'");
                            }
                            //dr1["ShiftsA"] = drx01["Shift"];
                            //dr1["Machine"] = drx01["Machine"];
                            //dr1["ProductName"] = drx01["ProductName"];
                            //dr1["ColorA"] = drx01["Color"];
                            //dr1["MachineNoA"] = drx01["MachineNo"];
                            //dr1["CycleTimeA"] = drx01["CycleTime"];
                            //dr1["RunningA"] = enpty2zero(drx01["Running"].ToString());
                            //dr1["ProjectedA"] = drx01["Projected"];
                            //dr1["ActualA"] = drx01["Actual"];
                            //dr1["wastageA"] = drx01["wastage"];
                            //dr1["WeightA"] = drx01["Weight"];

                        }

                    }
                    else
                    {
                        //dr1["ShiftsA"] = "0";
                        //dr1["Machine"] = "0";
                        //dr1["ProductName"] = "0";
                        //dr1["ColorA"] = "0";
                        //dr1["MachineNoA"] = "0";
                        //dr1["CycleTimeA"] = "0";
                        //dr1["RunningA"] = "0";
                        //dr1["ProjectedA"] = "0";
                        //dr1["ActualA"] = "0";
                        //dr1["wastageA"] = "0";
                        //dr1["WeightA"] = "0";
                    }



                    //2nd shift


                    if (query2nd.Trim() == "()")
                    {
                        query2nd = "Shift=0";
                    }

                    DataTable dt42 = SQLQuery.ReturnDataTable(@"SELECT (SELECT 'Shift - ' + DepartmentName FROM Shifts WHERE (Departmentid = PrdPlasticCont.Shift)) AS Shift, 
                         (Select Description from Machines WHERE mid=PrdPlasticCont.MachineNo) AS Machine, (Select MachineNo from Machines WHERE mid=PrdPlasticCont.MachineNo) AS MachineNo,
                         AVG(CONVERT(decimal, PrdPlasticCont.CycleTime)) AS CycleTime, (SUM(PrdPlasticCont.WorkingHour) + SUM(PrdPlasticCont.WorkingMin) / 60) AS Running, SUM(PrdPlasticCont.CalcProduction) AS Projected, 
                         SUM(PrdPlasticCont.FinalProduction) AS Actual, SUM(PrdPlasticCont.Rejection) AS wastage, SUM(CalcProduction) AS CalcProduction, SUM(ActProduction) AS ActProduction,
                         SUM(Rejection) AS Rejection, SUM(NetProduction) AS NetProduction, SUM(ItemWeight) AS ItemWeight, SUM(TotalWeight) AS TotalWeight, SUM(ActualWeight) AS ActualWeight, ItemID AS ProductID, 
                         (SELECT BrandName FROM Brands WHERE (BrandID = PrdPlasticCont.PackSize)) + ' ' + (SELECT ItemName FROM Products WHERE (ProductID = PrdPlasticCont.ItemID)) + ' ' +
                         (SELECT BrandName FROM CustomerBrands WHERE (BrandID = PrdPlasticCont.Brand)) AS ProductName, (SELECT DepartmentName from Colors WHERE Departmentid=PrdPlasticCont.Color) as Color,
                         SUM(FinalProduction) AS FinalProduction, SUM(PrdPlasticCont.FinalKg) AS Weight, SUM(WasteWeight) AS WasteWeight, SUM(NonusableWeight) AS NonusableWeight, Brand, PackSize
                         FROM PrdPlasticCont WHERE " + query2nd + " AND (PrdPlasticCont.Date >='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ) AND (PrdPlasticCont.Date <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "' AND (PrdPlasticCont.ItemID = '" + drx4["ItemID"] + "')  AND (PrdPlasticCont.MachineNo = '" + drx3["mid"] + "')) GROUP BY Shift, MachineNo, ItemID, PackSize, Brand, Color ");

                    if (dt42.Rows.Count > 0)
                    {
                        foreach (DataRow drx02 in dt42.Rows)
                        {
                            string pid = SQLQuery.ReturnString("Select pid FROM DailyPrdnAllShift WHERE MachineNoA='" + drx02["MachineNo"] + "' AND ProductID='" + drx02["ProductID"] + "' AND ColorA='" + drx02["Color"] + "' AND BrandID='" + drx02["Brand"] + "' AND PackSizeID='" + drx02["PackSize"] + "'");
                            if (pid != "")
                            {
                                SQLQuery.ExecNonQry("UPDATE DailyPrdnAllShift SET ShiftsB='" + drx02["Shift"] + "', CycleTimeB='" + drx02["CycleTime"] + "', RunningB='" + enpty2zero(drx02["Running"].ToString()) + "', ProjectedB='" + drx02["Projected"] + "', ActualB='" + drx02["Actual"] + "', WastageB='" + drx02["wastage"] + "', WeightB='" + drx02["Weight"] + "' WHERE pid='" + pid + "'");
                            }
                            //else
                            //{
                            //    SQLQuery.ExecNonQry("INSERT INTO [DailyPrdnAllShift] ([ShiftsB], [MachineNoB], [MachineB], [ProductIDB], [ProductNameB], [ColorB], [CycleTimeB], [RunningB], [ProjectedB], [ActualB], [WastageB], [WeightB])" + " " +
                            //                        "VALUES ('" + drx02["Shift"] + "', '" + drx02["MachineNo"] + "', '" + drx02["Machine"] + "', '" + drx02["ProductID"] + "', '" + drx02["ProductName"] + "', '" + drx02["Color"] + "', '" + drx02["CycleTime"] + "', '" + enpty2zero(drx02["Running"].ToString()) + "', '" + drx02["Projected"] + "', '" + drx02["Actual"] + "', '" + drx02["wastage"] + "','" + drx02["Weight"] + "')");
                            //}

                            //dr1["ShiftsB"] = drx02["Shift"];
                            //dr1["ColorB"] = drx02["Color"];
                            //dr1["CycleTimeB"] = drx02["CycleTime"];
                            //dr1["RunningB"] = enpty2zero(drx02["Running"].ToString());
                            //dr1["ProjectedB"] = drx02["Projected"];
                            //dr1["ActualB"] = drx02["Actual"];
                            //dr1["wastageB"] = drx02["wastage"];
                            //dr1["WeightB"] = drx02["Weight"];


                        }
                    }
                    else
                    {
                        //dr1["ShiftsB"] = "0";
                        //dr1["ColorB"] = "0";
                        //dr1["CycleTimeB"] = "0";
                        //dr1["RunningB"] = "0";
                        //dr1["ProjectedB"] = "0";
                        //dr1["ActualB"] = "0";
                        //dr1["wastageB"] = "0";
                        //dr1["WeightB"] = "0";
                    }

                    //3rd shift

                    if (query3rd.Trim() == "()")
                    {
                        query3rd = "Shift=0";
                    }

                    DataTable dt43 = SQLQuery.ReturnDataTable(@"SELECT (SELECT 'Shift - ' + DepartmentName FROM Shifts WHERE (Departmentid = PrdPlasticCont.Shift)) AS Shift, 
                         (Select Description from Machines WHERE mid=PrdPlasticCont.MachineNo) AS Machine, (Select MachineNo from Machines WHERE mid=PrdPlasticCont.MachineNo) AS MachineNo,
                         AVG(CONVERT(decimal, PrdPlasticCont.CycleTime)) AS CycleTime, (SUM(PrdPlasticCont.WorkingHour) + SUM(PrdPlasticCont.WorkingMin) / 60) AS Running, SUM(PrdPlasticCont.CalcProduction) AS Projected, 
                         SUM(PrdPlasticCont.FinalProduction) AS Actual, SUM(PrdPlasticCont.Rejection) AS wastage, SUM(CalcProduction) AS CalcProduction, SUM(ActProduction) AS ActProduction,
                         SUM(Rejection) AS Rejection, SUM(NetProduction) AS NetProduction, SUM(ItemWeight) AS ItemWeight, SUM(TotalWeight) AS TotalWeight, SUM(ActualWeight) AS ActualWeight, ItemID AS ProductID, 
                         (SELECT BrandName FROM Brands WHERE (BrandID = PrdPlasticCont.PackSize)) + ' ' + (SELECT ItemName FROM Products WHERE (ProductID = PrdPlasticCont.ItemID)) + ' ' +
                         (SELECT BrandName FROM CustomerBrands WHERE (BrandID = PrdPlasticCont.Brand)) AS ProductName, (SELECT DepartmentName from Colors WHERE Departmentid=PrdPlasticCont.Color) as Color,
                         SUM(FinalProduction) AS FinalProduction, SUM(PrdPlasticCont.FinalKg) AS Weight, SUM(WasteWeight) AS WasteWeight, SUM(NonusableWeight) AS NonusableWeight, Brand, PackSize
                         FROM PrdPlasticCont WHERE " + query3rd + " AND (PrdPlasticCont.Date >='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ) AND (PrdPlasticCont.Date <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "' AND (PrdPlasticCont.ItemID = '" + drx4["ItemID"] + "')  AND (PrdPlasticCont.MachineNo = '" + drx3["mid"] + "')) GROUP BY Shift, MachineNo, ItemID, PackSize, Brand, Color ");

                    if (dt43.Rows.Count > 0)
                    {
                        foreach (DataRow drx03 in dt43.Rows)
                        {
                            string pid = SQLQuery.ReturnString("Select pid FROM DailyPrdnAllShift WHERE MachineNoA='" + drx03["MachineNo"] + "' AND ProductID='" + drx03["ProductID"] + "' AND ColorA='" + drx03["Color"] + "' AND BrandID='" + drx03["Brand"] + "' AND PackSizeID='" + drx03["PackSize"] + "'");
                            if (pid != "")
                            {
                                SQLQuery.ExecNonQry("UPDATE DailyPrdnAllShift SET ShiftsC='" + drx03["Shift"] + "', CycleTimeC='" + drx03["CycleTime"] + "', RunningC='" + enpty2zero(drx03["Running"].ToString()) + "', ProjectedC='" + drx03["Projected"] + "', ActualC='" + drx03["Actual"] + "', WastageC='" + drx03["wastage"] + "', WeightC='" + drx03["Weight"] + "' WHERE pid='" + pid + "'");
                            }
                            //else
                            //{
                            //    SQLQuery.ExecNonQry("INSERT INTO [DailyPrdnAllShift] ([ShiftsC], [MachineNoC], [MachineC], [ProductIDC], [ProductNameC], [ColorC], [CycleTimeC], [RunningC], [ProjectedC], [ActualC], [WastageC], [WeightC])" +
                            //                                     " VALUES ('" + drx03["Shift"] + "', '" + drx03["MachineNo"] + "', '" + drx03["Machine"] + "', '" + drx03["ProductID"] + "', '" + drx03["ProductName"] + "', '" + drx03["Color"] + "', '" + drx03["CycleTime"] + "', '" + enpty2zero(drx03["Running"].ToString()) + "', '" + drx03["Projected"] + "', '" + drx03["Actual"] + "', '" + drx03["wastage"] + "','" + drx03["Weight"] + "')");
                            //}
                            //dr1["ShiftsC"] = drx03["Shift"];
                            //dr1["ColorC"] = drx03["Color"];
                            //dr1["CycleTimeC"] = drx03["CycleTime"];
                            //dr1["RunningC"] = enpty2zero(drx03["Running"].ToString());
                            //dr1["ProjectedC"] = drx03["Projected"];
                            //dr1["ActualC"] = drx03["Actual"];
                            //dr1["wastageC"] = drx03["wastage"]; 
                            //dr1["WeightC"] = drx03["Weight"];

                        }
                    }
                    else
                    {
                        //dr1["ShiftsC"] = "0";
                        //dr1["ColorC"] = "0";
                        //dr1["CycleTimeC"] = "0";
                        //dr1["RunningC"] = "0";
                        //dr1["ProjectedC"] = "0";
                        //dr1["ActualC"] = "0";
                        //dr1["wastageC"] = "0";
                        //dr1["WeightC"] = "0";
                    }


                    //total shift

                    SQLQuery.ReturnString("UPDATE DailyPrdnAllShift SET RunningT= RunningA + RunningB + RunningC");
                    SQLQuery.ReturnString("UPDATE DailyPrdnAllShift SET ProjectedT= ProjectedA + ProjectedB + ProjectedC");
                    SQLQuery.ReturnString("UPDATE DailyPrdnAllShift SET ActualT= ActualA + ActualB + ActualC");
                    SQLQuery.ReturnString("UPDATE DailyPrdnAllShift SET WastageT= WastageA + WastageB + WastageC");
                    SQLQuery.ReturnString("UPDATE DailyPrdnAllShift SET WeightT= WeightA + WeightB + WeightC");

                    //dr1["ShiftsB"] = drx5["Shift"];
                    //dr1["ColorB"] = drx5["Color"];
                    //dr1["BrandB"] = drx5["Brand"];
                    //dr1["CycleTimeT"] = Convert.ToDecimal(dr1["CycleTimeA"]) + Convert.ToDecimal(dr1["CycleTimeB"]) + Convert.ToDecimal(dr1["CycleTimeC"]);
                    //dr1["RunningT"] = Convert.ToDecimal(dr1["RunningA"]) + Convert.ToDecimal(dr1["RunningB"]) + Convert.ToDecimal(dr1["RunningC"]);
                    //dr1["ProjectedT"] = Convert.ToDecimal(dr1["ProjectedA"]) + Convert.ToDecimal(dr1["ProjectedB"]) + Convert.ToDecimal(dr1["ProjectedC"]);
                    //dr1["ActualT"] = Convert.ToDecimal(dr1["ActualA"]) + Convert.ToDecimal(dr1["ActualB"]) + Convert.ToDecimal(dr1["ActualC"]);
                    //dr1["wastageT"] = Convert.ToDecimal(dr1["wastageA"]) + Convert.ToDecimal(dr1["wastageB"]) + Convert.ToDecimal(dr1["wastageC"]);
                    //dr1["WeightT"] = Convert.ToDecimal(dr1["WeightA"]) + Convert.ToDecimal(dr1["WeightB"]) + Convert.ToDecimal(dr1["WeightC"]);

                    //SQLQuery.ExecNonQry(InsertQuery);
                    //dt1.Rows.Add(dr1);


                }
            }

            SQLQuery.ExecNonQry("DELETE  FROM DailyPrdnAllShift WHERE ProjectedT<='0'");
            return dt1;
        }

        private string enpty2zero(string empty)
        {
            if (string.IsNullOrEmpty(empty))
            {
                return "0";
            }
            else
            {
                return empty;
            }
        }


        private DataTable BindItemGrid(string dateFrom, string dateTo, string type)
        {
            DataSet ds = new DataSet();
            try
            {
                if (dateFrom != "")
                {
                    dateFrom = " AND PrdPlasticCont.Date>='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ";
                }

                if (dateTo != "")
                {
                    dateTo = " AND PrdPlasticCont.Date<='" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "' ";
                }

                string query = "   where PrdPlasticCont.ActProduction>0 " + dateFrom + dateTo;
                string url = "";// SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";

                if (type == "AllShift")
                {
                    query = @"SELECT    ShiftsA, MachineNoA, Machine, ProductName, ColorA, CycleTimeA, RunningA, ProjectedA, ActualA, WastageA, WeightA, WastePercentA, ShiftsB, CycleTimeB, RunningB, ProjectedB, ActualB, 
                            WastageB, WeightB, WastePercentB, ShiftsC, CycleTimeC, RunningC, ProjectedC, ActualC, WastageC, WeightC, WastePercentC, RunningT, ProjectedT, ActualT, WastageT, WeightT, WastePercentT
                            FROM DailyPrdnAllShift " + query + @" 
                            ORDER BY MachineNoA ";

                }
                else
                {
                    //                    query = @"SELECT        (SELECT        Company
                    //                          FROM            Party
                    //                          WHERE        (PartyID = PrdPlasticCont.CustomerID)) AS Customer,
                    //                             (SELECT        BrandName
                    //                               FROM            Brands
                    //                               WHERE        (BrandID = PrdPlasticCont.PackSize)) + ' ' +
                    //                             (SELECT        ItemName
                    //                               FROM            Products
                    //                               WHERE        (ProductID = PrdPlasticCont.ItemID)) + ' ' +
                    //                             (SELECT        BrandName
                    //                               FROM            CustomerBrands
                    //                               WHERE        (BrandID = PrdPlasticCont.Brand)) AS ProductName,
                    //                             (SELECT        'Shift - ' + DepartmentName AS Expr1
                    //                               FROM            Shifts
                    //                               WHERE        (Departmentid = PrdPlasticCont.Shift)) AS Shifts,
                    //                             (SELECT        DepartmentName
                    //                               FROM            Colors
                    //                               WHERE        (Departmentid = PrdPlasticCont.Color)) AS Color, SUM(PrdPlasticCont.FinalProduction) AS Qty, SUM(PrdPlasticCont.FinalKg) AS Weight, AVG(CONVERT(decimal, PrdPlasticCont.CycleTime)) 
                    //                         AS CycleTime, SUM(PrdPlasticCont.WorkingHour + PrdPlasticCont.WorkingMin / 60) AS Running, SUM(PrdPlasticCont.CalcProduction) AS Projected, SUM(PrdPlasticCont.FinalProduction) AS Actual, 
                    //                         SUM(PrdPlasticCont.Rejection) AS wastage, SUM(PrdPlasticCont.Rejection) * 100 / SUM(PrdPlasticCont.ActProduction) AS wastePercent, Machines_1.MachineNo AS Machine, Machines_1.Description AS Brand
                    //FROM            PrdPlasticCont INNER JOIN
                    //                         Machines AS Machines_1 ON PrdPlasticCont.MachineNo = Machines_1.mid  " + query + @"
                    //GROUP BY Machines_1.MachineNo, PrdPlasticCont.MachineNo, PrdPlasticCont.CustomerID, PrdPlasticCont.Brand, PrdPlasticCont.PackSize, PrdPlasticCont.ItemID, PrdPlasticCont.Shift, PrdPlasticCont.Color, Machines_1.Description
                    //ORDER BY Machines_1.MachineNo, PrdPlasticCont.Shift";

                }

                ds = SQLQuery.ReturnDataSet(query);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        ReportDocument rpt = new ReportDocument();


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