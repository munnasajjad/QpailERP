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
using RunQuery;

namespace Stock
{
    public class Inventory
    {
        public static string GetProductName(string pid)
        {
            pid = SQLQuery.ReturnString("Select ItemName from Products where ProductID='" + pid + "'");
            return pid;
        }

        public static string StockEnabled()
        {
            string catID = SQLQuery.ReturnString("SELECT ShortDescription FROM Settings_Project WHERE SettingType ='Stock Link'");
            return catID;
        }

        public static string GetItemGroup(string productId)
        {
            string catID = SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + productId + "'");
            string grdID = SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
            string subID = SQLQuery.ReturnString("SELECT CategoryID FROM ItemGrade WHERE GradeID ='" + grdID + "'");
            string grpID = SQLQuery.ReturnString("SELECT GroupID FROM ItemSubGroup WHERE CategoryID ='" + subID + "'");
            return grpID;
        }
        public static string GetItemSubGroup(string productId)
        {
            string catID = SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + productId + "'");
            string grdID = SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
            string subID = SQLQuery.ReturnString("SELECT CategoryID FROM ItemGrade WHERE GradeID ='" + grdID + "'");
            return subID;
        }
        public static string GetItemGrade(string productId)
        {
            string catID = SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + productId + "'");
            string grdID = SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
            return grdID;
        }

        public static string GetItemCategory(string productId)
        {
            string catID = SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + productId + "'");
            return catID;
        }

        public static string AvailableNonPrintedQty(string purpose, string itemType, string productId, string godownId, string locationId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InQuantity)-SUM(OutQuantity),0) FROM Stock WHERE ProductID='" + productId + "' AND ItemType='" + itemType + "' AND WarehouseID='" + godownId + "' AND LocationID = '" + locationId + "'");
            return catID;
        }
        public static string AvailableNonPrintedWeight(string purpose, string itemType, string productId, string godownId, string locationId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InWeight)-SUM(OutWeight),0) FROM Stock WHERE ProductID='" + productId + "' AND ItemType='" + itemType + "' AND WarehouseID='" + godownId + "' AND LocationID = '" + locationId + "'");
            return catID;
        }
        public static string LastNonprintedPrice(string purpose, string itemType, string productId)
        {
            string catID = "0";
            try
            {
                catID = SQLQuery.ReturnString("SELECT Top(1) Price FROM Stock WHERE ProductID='" + productId + "' AND ItemType='" + itemType +
                                         "' AND price>0 order by EntryID desc ");
            }
            catch (Exception xx) { }
            return catID;
        }
        public static string PlasticRawWeight(string purpose, string productId, string godownId, string locationId)
        {
            //string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InWeight)-SUM(OutWeight),0) FROM Stock where  ProductID='" + productId + "' AND WarehouseID='" + godownId + "' AND LocationID = '" + locationId + "'");
            string catID = SQLQuery.ReturnString("SELECT ISNULL(MAX(InKg), 0) FROM tblFifo WHERE ItemCode='" + productId + "' AND GodownId='" + godownId + "' AND LocationId = '" + locationId + "' AND OutTypeId = ''");
            return catID;
        }
        public static string IMLRawWeight(string productId, string godownId, string locationId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InWeight)-SUM(OutWeight),0) FROM Stock WHERE ProductID='" + productId + "' AND WarehouseID='" + godownId + "' AND LocationID = '" + locationId + "' ");
            return catID;
        }
        public static string PlasticRawWeightMachine(string purpose, string productId, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InWeight)-SUM(OutWeight),0) FROM MachineStock WHERE  ProductID='" + productId + "' AND MachineID='" + godownId + "' ");
            return catID;
        }
        public static string QtyinMachineStock(string purpose, string productId, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InQuantity)-SUM(OutQuantity),0) FROM MachineStock WHERE   ProductID='" + productId + "' AND MachineID='" + godownId + "' ");
            return catID;
        }
        public static string QtyinStock(string purpose, string productId, string godownId, string locationId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InQuantity)-SUM(OutQuantity),0) FROM Stock WHERE ProductID='" + productId + "' AND WarehouseID='" + godownId + "' AND LocationID = '" + locationId + "'");
            return catID;
        }
        public static string LastPlasticRawPrice(string purpose, string productId)
        {
            string catID = "0";
            try
            {
                catID = SQLQuery.ReturnString("SELECT TOP(1) Price FROM Stock WHERE ProductID='" + productId + "' AND price>0 ORDER BY EntryID DESC ");
            }
            catch (Exception xx) { }
            return catID;
        }
        public static string LastPlasticRawPriceMachine(string purpose, string productId)
        {
            string catID = "0";
            try
            {
                catID = SQLQuery.ReturnString("SELECT TOP(1) Price FROM MachineStock WHERE ProductID='" + productId + "' AND price>0 order by EntryID desc ");
            }
            catch (Exception xx) { }
            return catID;
        }
        public static string NonUsableFixedAssestsQty(string productId, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InQuantity)-SUM(OutQuantity),0) FROM  FixedAssets WHERE  ProductID='" + productId + "' AND  WarehouseID='" + godownId + "' ");
            return catID;
        }
        public static string NonUsableQty(string productId, string godownId, string locationId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InQuantity)-SUM(OutQuantity),0) FROM Stock WHERE ProductID='" + productId + "' AND  WarehouseID='" + godownId + "' AND LocationID = '" + locationId + "' ");
            return catID;
        }
        public static string NonUsableWeight(string productId, string godownId, string locationId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InWeight)-SUM(OutWeight),0) FROM Stock WHERE ProductID='" + productId + "' AND WarehouseID='" + godownId + "' AND LocationID = '" + locationId + "' ");
            return catID;
        }

        public static decimal RawWeightQtyRatio(string purpose, string itemType, string productId, string godownId, string locationId)
        {
            decimal avQty = Convert.ToDecimal(AvailableNonPrintedQty(purpose, itemType, productId, godownId, locationId));
            decimal avWeight = Convert.ToDecimal(AvailableNonPrintedWeight(purpose, itemType, productId, godownId, locationId));
            if (avQty > 0)
            {
                return (avWeight / avQty);
            }
            else
            {
                return 0;
            }
        }

        public static string AvailablePrintedQty(string purpose, string itemType, string productId, string customer, string brand, string packSize, string color, string godownId, string locationId)
        {
            string catID = SQLQuery.ReturnString(
                        "SELECT ISNULL(SUM(InQuantity)-SUM(OutQuantity),0) FROM Stock WHERE ProductID='" +
                        productId + "' AND ItemType='" + itemType + "' AND Customer='" +
                        customer + "' AND  BrandID='" + brand + "' AND SizeId='" +
                        packSize + "'  AND Color='" +
                        color + "'  AND WarehouseID='" + godownId + "' AND LocationID = '" + locationId + "' ");
            return catID;
        }
        public static string AvailablePrintedWeight(string purpose, string itemType, string productId, string customer, string brand, string packSize, string color, string godownId, string locationId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InWeight)-SUM(OutWeight),0) FROM Stock WHERE ProductID='" +
                        productId + "' AND ItemType='" + itemType + "' AND Customer='" +
                        customer + "' AND  BrandID='" + brand + "' AND SizeId='" +
                        packSize + "'  AND Color='" +
                        color + "'  AND WarehouseID='" + godownId + "' AND LocationID = '" + locationId + "'");
            return catID;
        }
        public static string PrintedWeight(string purpose, string itemType, string productId, string customer, string brand, string packSize, string color, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InWeight)-SUM(OutWeight),0) FROM Stock WHERE ProductID='" +
                        productId + "' AND ItemType='" + itemType + "' AND Customer='" +
                        customer + "' AND  BrandID='" + brand + "' AND SizeId='" +
                        packSize + "'  AND Color='" +
                        color + "'  AND WarehouseID='" + godownId + "' ");
            return catID;
        }
        public static string AvailableInkWeight(string productId, string specification, string godownId, string locationID)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InWeight)-SUM(OutWeight),0) FROM Stock WHERE ProductID='" + productId + "' AND Spec='" + specification + "' AND  WarehouseID='" + godownId + "' AND LocationID = '" + locationID + "' ");
            return catID;
        }
        public static string LastInkPrice(string productId, string specification)
        {
            string catID = "0";
            try
            {
                catID = SQLQuery.ReturnString("SELECT Top(1) Price FROM Stock WHERE  ProductID='" + productId + "' AND Spec='" + specification + "' AND price>0 order by EntryID desc ");
            }
            catch (Exception xx) { }
            return catID;
        }
        public static string AvailableProcessedQty(string purpose, string itemType, string productId, string customer, string brand, string packSize, string color, string godownId, string locationId)
        {
            string catID = SQLQuery.ReturnString(
                        "SELECT ISNULL(SUM(InQuantity)-SUM(OutQuantity),0) FROM Stock WHERE ProductID='" +
                        productId + "' AND ItemType='" + itemType + "' AND Customer='" +
                        customer + "' AND  BrandID='" + brand + "' AND SizeId='" +
                        packSize + "'  AND Color='" +
                        color + "'  AND WarehouseID='" + godownId + "' AND LocationID = '" + locationId + "' ");
            return catID;
        }
        public static decimal AvailableProcessedItemQuantity(string itemCode, string inType, string sizeId, string colorId)
        {
            //string catID = SQLQuery.ReturnString(
            //            "SELECT ISNULL(SUM(InQuantity)-SUM(OutQuantity),0) FROM MachineStock WHERE Purpose='" + purpose + "' AND  ProductID='" +
            //            productId + "' AND ItemType='" + itemType + "' AND Customer='" +
            //            customer + "'  AND SizeId='" +
            //            packSize + "'  AND Color='" +
            //            color + "'  AND WarehouseID='" + godownId + "' AND LocationID = '" + locationId + "' ");
            DataTable availablePCSDataTable = SQLQuery.ReturnDataTable("SELECT MAX(InPcs) AS InPcs, InDate FROM tblFifo WHERE ItemCode = '" + itemCode + "' AND InType='" + inType + "' AND SizeId='" + sizeId + "' AND ColorId='" + colorId + "' AND OutType = '' AND OutTypeId = '' GROUP BY InDate");
            decimal availablePCSInFifo = 0;
            foreach (DataRow availablePCSDataTableRow in availablePCSDataTable.Rows)
            {
                availablePCSInFifo += Convert.ToDecimal(availablePCSDataTableRow["InPcs"].ToString());
            }
            return availablePCSInFifo;
        }
        public static decimal AvailableProcessedItemWeight(string itemCode, string inType, string sizeId, string colorId)
        {
            //string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InWeight)-SUM(OutWeight),0) FROM MachineStock WHERE  Purpose='" + purpose + "' AND   ProductID='" +
            //                                     productId + "' AND ItemType='" + itemType + "' AND Customer='" +
            //                                     customer + "' AND SizeId='" +
            //                                     packSize + "'  AND Color='" +
            //                                     color + "'  AND WarehouseID='" + godownId + "' AND LocationID = '" + locationId + "' ");
            decimal totalQuantity = Convert.ToDecimal(SQLQuery.ReturnString(
                "SELECT ISNULL(SUM(InKg),0) AS TotalInkg FROM tblFifo WHERE ItemCode='" + itemCode + "' AND InType='" + inType + "' AND SizeId='" +
                sizeId + "'  AND ColorId='" + colorId + "' AND OutType = '' AND OutTypeId = ''"));
            return totalQuantity;
        }
        public static decimal AvailableSemiFinishedItemQuantity(string itemCode, string inType, string sizeId)
        {
            DataTable availablePCSDataTable = SQLQuery.ReturnDataTable("SELECT MAX(InPcs) AS InPcs, InDate FROM tblFifo WHERE ItemCode = '" + itemCode + "' AND InType='" + inType + "' AND SizeId='" + sizeId + "' AND OutType = '' AND OutTypeId = '' GROUP BY InDate");
            decimal availablePCSInFifo = 0;
            foreach (DataRow availablePCSDataTableRow in availablePCSDataTable.Rows)
            {
                availablePCSInFifo += Convert.ToDecimal(availablePCSDataTableRow["InPcs"].ToString());
            }
            return availablePCSInFifo;
        }
        public static decimal AvailableSemiFinishedItemWeight(string itemCode, string inType, string sizeId)
        {
            decimal totalQuantity = Convert.ToDecimal(SQLQuery.ReturnString(
                "SELECT ISNULL(SUM(InKg),0) AS TotalInkg FROM tblFifo WHERE ItemCode='" + itemCode + "' AND InType='" + inType + "' AND SizeId='" +
                sizeId + "' AND OutType = '' AND OutTypeId = ''"));
            return totalQuantity;
        }

        public static string AvailableProcessedWeight(string purpose, string itemType, string productId, string customer, string brand, string packSize, string color, string godownId, string locationId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(SUM(InWeight)-SUM(OutWeight),0) FROM Stock WHERE ProductID='" +
                        productId + "' AND ItemType='" + itemType + "' AND Customer='" + customer + "' AND  BrandID='" + brand + "' AND SizeId='" + packSize + "'  AND Color='" +
                        color + "'  AND WarehouseID='" + godownId + "' AND LocationID = '" + locationId + "'");
            return catID;
        }


        public static void SaveToStock(string purpose, string InvoiceID, string EntryType, string RefNo, string SizeID, string Customer, string BrandID, string color, string spec, string ProductID, string ProductName, string ItemType, string WarehouseID, string LocationID, string ItemGroup, decimal InQuantity, decimal OutQuantity, decimal unitPrice, decimal InWeight, decimal OutWeight, string ItemSerialNo, string Remark, string Status, string StockLocation, string EntryBy, string EntryDate)
        {

            if (InQuantity > 0 || OutQuantity > 0 || InWeight > 0 || OutWeight > 0)
            {
                //Item entry to stock
                SqlCommand cmd3 = new SqlCommand(
                    "INSERT INTO Stock ( Purpose, InvoiceID, EntryType, RefNo, SizeID, Customer, BrandID, Color, spec, ProductID, ProductName, ItemType, WarehouseID, LocationID, ItemGroup, InQuantity, OutQuantity, Price, InWeight, OutWeight, ItemSerialNo, Remark, Status, StockLocation, EntryBy, EntryDate)" +
                    " VALUES ('" + purpose +
                    "', @InvoiceID, @EntryType, @RefNo, @SizeID, @Customer, @BrandID, @Color, '" + spec +
                    "', @ProductID, @ProductName, @ItemType, @WarehouseID, @LocationID, @ItemGroup, @InQuantity, @OutQuantity, " +
                    unitPrice + ", @InWeight, @OutWeight, @ItemSerialNo, @Remark, @Status, @StockLocation, @EntryBy, '" +
                    EntryDate + "')",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd3.Parameters.AddWithValue("@InvoiceID", InvoiceID);
                cmd3.Parameters.AddWithValue("@EntryType", EntryType);
                cmd3.Parameters.AddWithValue("@RefNo", RefNo);
                cmd3.Parameters.AddWithValue("@SizeID", SizeID);
                cmd3.Parameters.AddWithValue("@Customer", Customer);
                cmd3.Parameters.AddWithValue("@BrandID", BrandID);
                cmd3.Parameters.AddWithValue("@Color", color);
                cmd3.Parameters.AddWithValue("@ProductID", ProductID);

                cmd3.Parameters.AddWithValue("@ProductName", ProductName);
                cmd3.Parameters.AddWithValue("@ItemType", ItemType);
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

                if (InQuantity > 0 || InWeight > 0)
                {
                    //Stock.Inventory.FifoInsert(ProductID, DateTime.Now.ToString("yyyy-MM-dd"), "", "", "0", Convert.ToDecimal(txtOutputKg.Text), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "Stock Out", outId, outValue);
                }
                else
                {
                    //Stock.Inventory.FifoInsert(ProductID, DateTime.Now.ToString("yyyy-MM-dd"), "", "", "0", Convert.ToDecimal(txtOutputKg.Text), Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"), "Stock Out", outId, outValue);
                }
            }
        }
        public static void SaveToMachineStock(string purpose, string InvoiceID, string EntryType, string RefNo, string SizeID, string Customer, string BrandID, string color, string spec, string ProductID, string ProductName, string ItemType, string WarehouseID, string LocationID, string MachineID, string ItemGroup, decimal InQuantity, decimal OutQuantity, decimal unitPrice, decimal InWeight, decimal OutWeight, string ItemSerialNo, string Remark, string Status, string StockLocation, string EntryBy, string EntryDate)
        {
            if (InQuantity > 0 || OutQuantity > 0 || InWeight > 0 || OutWeight > 0)
            {
                //Item entry to stock 
                SqlCommand cmd3 = new SqlCommand(@"INSERT INTO MachineStock (Purpose, InvoiceID, EntryType, RefNo, SizeID, Customer, BrandID, Color, spec, ProductID, ProductName, ItemType, WarehouseID, LocationID, MachineID, ItemGroup, InQuantity, OutQuantity, Price, InWeight, OutWeight, ItemSerialNo, Remark, Status, StockLocation, EntryBy, EntryDate)" +
                        " VALUES ('" + purpose + "', @InvoiceID, @EntryType, @RefNo, @SizeID, @Customer, @BrandID, @Color, '" + spec +
                        "', @ProductID, @ProductName, @ItemType, @WarehouseID, @LocationID, @MachineID, @ItemGroup, @InQuantity, @OutQuantity, " + unitPrice + ", @InWeight, @OutWeight, @ItemSerialNo, @Remark, @Status, @StockLocation, @EntryBy, '" + EntryDate + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd3.Parameters.AddWithValue("@InvoiceID", InvoiceID);
                cmd3.Parameters.AddWithValue("@EntryType", EntryType);
                cmd3.Parameters.AddWithValue("@RefNo", RefNo);
                cmd3.Parameters.AddWithValue("@SizeID", SizeID);
                cmd3.Parameters.AddWithValue("@Customer", Customer);
                cmd3.Parameters.AddWithValue("@BrandID", BrandID);
                cmd3.Parameters.AddWithValue("@Color", color);
                cmd3.Parameters.AddWithValue("@ProductID", ProductID);
                cmd3.Parameters.AddWithValue("@ProductName", ProductName);
                cmd3.Parameters.AddWithValue("@ItemType", ItemType);
                cmd3.Parameters.AddWithValue("@WarehouseID", WarehouseID);
                cmd3.Parameters.AddWithValue("@LocationID", LocationID);
                cmd3.Parameters.AddWithValue("@MachineID", MachineID);
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
        }

        public static void SaveToStock(string purpose, string InvoiceID, string EntryType, string RefNo, string SizeID, string Customer, string BrandID, string color, string spec, string ProductID, string ProductName, string ItemType, string WarehouseID, string LocationID, string ItemGroup, decimal InQuantity, decimal OutQuantity, decimal unitPrice, decimal InWeight, decimal OutWeight, string ItemSerialNo, string Remark, string Status, string StockLocation, string EntryBy, string EntryDate, string Description)
        {
            if (InQuantity > 0 || OutQuantity > 0 || InWeight > 0 || OutWeight > 0)
            {
                //Item entry to stock
                SqlCommand cmd3 = new SqlCommand(@"INSERT INTO Stock ( Purpose, InvoiceID, EntryType, RefNo, SizeID, Customer, BrandID, Color, spec, ProductID, ProductName, ItemType, WarehouseID, LocationID, ItemGroup, InQuantity, OutQuantity, Price, InWeight, OutWeight, ItemSerialNo, Remark, Status, StockLocation, EntryBy, EntryDate, Description)" +
                        " VALUES ('" + purpose + "', @InvoiceID, @EntryType, @RefNo, @SizeID, @Customer, @BrandID, @Color, '" + spec +
                        "', @ProductID, @ProductName, @ItemType, @WarehouseID, @LocationID, @ItemGroup, @InQuantity, @OutQuantity, " + unitPrice + ", @InWeight, @OutWeight, @ItemSerialNo, @Remark, @Status, @StockLocation, @EntryBy, '" + EntryDate + "', '" + Description + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd3.Parameters.AddWithValue("@InvoiceID", InvoiceID);
                cmd3.Parameters.AddWithValue("@EntryType", EntryType);
                cmd3.Parameters.AddWithValue("@RefNo", RefNo);
                cmd3.Parameters.AddWithValue("@SizeID", SizeID);
                cmd3.Parameters.AddWithValue("@Customer", Customer);
                cmd3.Parameters.AddWithValue("@BrandID", BrandID);
                cmd3.Parameters.AddWithValue("@Color", color);
                cmd3.Parameters.AddWithValue("@ProductID", ProductID);

                cmd3.Parameters.AddWithValue("@ProductName", ProductName);
                cmd3.Parameters.AddWithValue("@ItemType", ItemType);
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


        }
        /*
        public static decimal FifoInsert(string itemCode, string inDate, string inType, string inTypeId, string inValue,
            decimal InKg, string outDate, string outType, string outTypeId, string outValue)
        {
            decimal amt = 0;
            if (inDate == "NULL")
            {
                inDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
            if (outDate == "NULL")
            {
                outDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }

            inDate = Convert.ToDateTime(inDate).ToString("yyyy-MM-dd HH:mm");
            outDate = Convert.ToDateTime(outDate).ToString("yyyy-MM-dd HH:mm");


            if (inTypeId == "") //Stock out
            {
                while (InKg > 0)
                {
                    decimal rate = Convert.ToDecimal(SQLQuery.ReturnString("Select InValue from tblFifo WHERE id=(Select MIN(ID) from tblFifo WHERE ItemCode='" + itemCode + "' AND OutTypeId='') "));
                    amt += rate;
                        SQLQuery.ExecNonQry("Update  [tblFifo] set   [OutDate]= '" + outDate + "', [OutTypeId]= '" +
                                            outTypeId + "', [OutType]='" + outType + "', [OutValue]='" + outValue +
                                            "'  WHERE Id=(Select MIN(ID) from tblFifo WHERE ItemCode='" + itemCode +
                                            "' AND OutTypeId='' )");
                        InKg--;

                    if (InKg < 1)
                    {
                        amt += (InKg*rate);
                        InKg = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(FracInHand),0) FROM tblFifo WHERE ItemCode='" + itemCode + "'"))- InKg;
                        //Zeroing all fraction in hand value
                        SQLQuery.ExecNonQry("UPDATE tblFifo SET FracInHand='0' WHERE ItemCode='" + itemCode + "' ");

                        SQLQuery.ExecNonQry("UPDATE tblFifo SET FracInHand='-" + InKg +
                                            "' WHERE Id=(Select MAX(id) from tblFifo WHERE ItemCode='" + itemCode +
                                            "') ");
                    }
                }
                
            }

            if (outTypeId == "") //Stock-In / Purchase 1st Time
            {
                InKg +=
                    Convert.ToDecimal(
                        SQLQuery.ReturnString("SELECT ISNULL(SUM(FracInHand),0) FROM tblFifo WHERE ItemCode='" +
                                              itemCode + "'"));
                //Zeroing all fraction in hand value
                SQLQuery.ExecNonQry("UPDATE tblFifo SET FracInHand='0' WHERE ItemCode='" + itemCode + "' ");

                while (InKg > 0)
                {
                    SQLQuery.ExecNonQry(
                        "INSERT INTO [tblFifo] ([ItemCode], [InDate], [InType],[InTypeId], [InValue],[InKg], [OutDate], [OutType], [OutTypeId],[OutValue])" +
                        " VALUES ('" + itemCode + "', '" + inDate + "', '" + inType + "', '" + inTypeId + "', '" +
                        inValue + "', '" + InKg + "', '" + outDate + "', '" + outType + "', '" + outTypeId + "', '" +
                        outValue + "')");
                    InKg--;

                    if (InKg < 1)
                    {
                        SQLQuery.ExecNonQry("UPDATE tblFifo SET FracInHand='" + InKg +
                                            "' WHERE Id=(Select MAX(id) from tblFifo WHERE ItemCode='" + itemCode +
                                            "') ");
                    }

                }


                //}
                //else
                //{
                //    while (toOutQty > 0)
                //    {
                //        SQLQuery.ExecNonQry("Update  [tblFifo] set   [OutDate]= '" + outDate + "', [OutType]='" + outType + "',[OutValue]='" + outValue + "' WHERE Id=(Select MIN(ID) from tblFifo)");
                //        toOutQty--;
                //    }
                //}

                //string query = "Select ItemCode,InDate,InType,InTypeId,InValue,InKg,OutDate,OutType,OutTypeId,OutValue,Profit from tblFifo where InTypeId='' AND OutTypeId<>'' ";
                //DataTable dt = SQLQuery.ReturnDataTable(query);


            }
            return amt;
        } */

        public static decimal FifoInsert(string itemCode, string transactionType, string companyId, string sizeId, string colorId, string brandId, string inDate, string inType, string inTypeId, decimal inValue,
            decimal InKg, decimal InPcs, string godownId, string locationId, string outDate, string outType, string outTypeId, string outValue)
        {
            decimal amt = 0;
            if (inDate == "NULL")
            {
                inDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
            if (outDate == "NULL")
            {
                outDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }

            inDate = Convert.ToDateTime(inDate).ToString("yyyy-MM-dd HH:mm");
            outDate = Convert.ToDateTime(outDate).ToString("yyyy-MM-dd HH:mm");


            //Stock out
            if (inTypeId == "")
            {
                if (transactionType == "ProductionOutput" || transactionType == "InkConsumption")
                {
                    while (InKg > 0)
                    {
                        string rate = SQLQuery.ReturnString("SELECT InValue FROM tblFifo WHERE Id=(Select MIN(ID) FROM tblFifo WHERE ItemCode='" + itemCode + "' AND OutTypeId='') ");
                        if (rate == "")
                        {
                            rate = "0";
                        }
                        amt += Convert.ToDecimal(rate);
                        SQLQuery.ExecNonQry("UPDATE [tblFifo] SET [OutDate]= '" + outDate + "', [OutTypeId]= '" +
                                            outTypeId + "', [OutType]='" + outType + "', [OutValue]='" + rate +
                                            "'  WHERE Id=(Select MIN(ID) FROM tblFifo WHERE ItemCode='" + itemCode + "' AND OutTypeId='')");
                        InKg--;

                        if (InKg < 1)
                        {
                            amt += (InKg * Convert.ToDecimal(rate));
                            InKg = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(FracInHand),0) FROM tblFifo WHERE ItemCode='" + itemCode + "'")) - InKg;
                            //Zeroing all fraction in hand value
                            SQLQuery.ExecNonQry("UPDATE tblFifo SET FracInHand='0' WHERE ItemCode='" + itemCode + "'");
                            SQLQuery.ExecNonQry("UPDATE tblFifo SET FracInHand='" + InKg + "' WHERE Id=(Select MAX(id) from tblFifo WHERE ItemCode='" + itemCode + "') ");
                        }
                    }
                }
                else if (transactionType == "Production-ScreenPrinting" || transactionType == "Production-HTFPrinting" || transactionType == "RawMaterialsConsumption")
                {
                    while (InPcs > 0)
                    {
                        string rate = SQLQuery.ReturnString("SELECT InValue FROM tblFifo WHERE Id=(Select MIN(ID) FROM tblFifo WHERE ItemCode='" + itemCode + "' AND InType = 'ProcessedItem' AND OutTypeId='') ");
                        if (rate == "")
                        {
                            rate = "0";
                        }
                        amt += Convert.ToDecimal(rate);
                        SQLQuery.ExecNonQry("UPDATE [tblFifo] SET [OutDate]= '" + outDate + "', [OutTypeId]= '" +
                                            outTypeId + "', [OutType]='" + outType + "', [OutValue]='" + rate +
                                            "'  WHERE Id=(Select MIN(ID) FROM tblFifo WHERE ItemCode='" + itemCode + "' AND InType = 'ProcessedItem' AND OutTypeId='')");
                        InPcs--;

                        if (InPcs < 1)
                        {
                            amt += (InPcs * Convert.ToDecimal(rate));
                            InPcs = Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(FracInHand),0) FROM tblFifo WHERE ItemCode='" + itemCode + "' AND InType = 'ProcessedItem'")) - InPcs;
                            //Zeroing all fraction in hand value
                            SQLQuery.ExecNonQry("UPDATE tblFifo SET FracInHand='0' WHERE ItemCode='" + itemCode + "' AND InType = 'ProcessedItem'");
                            SQLQuery.ExecNonQry("UPDATE tblFifo SET FracInHand='" + InPcs + "' WHERE Id=(Select MAX(id) from tblFifo WHERE ItemCode='" + itemCode + "' AND InType = 'ProcessedItem')");
                        }
                    }
                }
                //Stock-In / Purchase 1st Time
                //}
                //else
                //{
                //    while (toOutQty > 0)
                //    {
                //        SQLQuery.ExecNonQry("Update  [tblFifo] set   [OutDate]= '" + outDate + "', [OutType]='" + outType + "',[OutValue]='" + outValue + "' WHERE Id=(Select MIN(ID) from tblFifo)");
                //        toOutQty--;
                //    }
                //}

                //string query = "Select ItemCode,InDate,InType,InTypeId,InValue,InPcs,OutDate,OutType,OutTypeId,OutValue,Profit from tblFifo where InTypeId='' AND OutTypeId<>'' ";
                //DataTable dt = SQLQuery.ReturnDataTable(query);


            }
            if (outTypeId == "")
            {
                InPcs += Convert.ToDecimal(SQLQuery.ReturnString("SELECT ISNULL(SUM(FracInHand),0) FROM tblFifo WHERE ItemCode='" + itemCode + "'"));
                //Zeroing all fraction in hand value
                SQLQuery.ExecNonQry("UPDATE tblFifo SET FracInHand='0' WHERE ItemCode='" + itemCode + "'");

                if (inType != "SemiFinished")
                {
                    if (inType == "RawMaterials")
                    {
                        while (InKg > 0)
                        {
                            SQLQuery.ExecNonQry(
                                "INSERT INTO [tblFifo] ([ItemCode],[TransactionType],[CompanyId],[SizeId],[ColorId],[BrandId],[InDate], [InType],[InTypeId],[InValue],[InKg],[InPcs],[GodownId],[LocationId],[OutDate],[OutType],[OutTypeId],[OutValue])" +
                                " VALUES ('" + itemCode + "','" + transactionType + "','" + companyId + "','" + sizeId + "','" + colorId +
                                "','" + brandId + "', '" + inDate + "', '" + inType + "', '" + inTypeId + "', '" +
                                inValue + "', '" + InKg + "','" + InPcs + "','" + godownId + "','" + locationId + "','" + outDate + "', '" + outType + "', '" + outTypeId + "', '" + outValue + "')");
                            InKg--;

                            if (InKg < 1)
                            {
                                SQLQuery.ExecNonQry("UPDATE tblFifo SET FracInHand='" + InKg +
                                                    "' WHERE Id=(Select MAX(id) from tblFifo WHERE ItemCode='" + itemCode + "') ");
                            }
                        }
                    }
                    else if (inType == "ProcessedItem")
                    {
                        while (InPcs > 0)
                        {
                            SQLQuery.ExecNonQry("INSERT INTO [tblFifo] ([ItemCode],[TransactionType],[CompanyId],[SizeId],[ColorId],[BrandId],[InDate], [InType],[InTypeId],[InValue],[InKg],[InPcs],[GodownId],[LocationId],[OutDate],[OutType],[OutTypeId],[OutValue])" +
                                                " VALUES ('" + itemCode + "','" + transactionType + "','" + companyId + "','" + sizeId + "','" + colorId + "','" + brandId + "', '" + inDate + "', '" + inType + "', '" + inTypeId + "', '" +
                                                inValue + "', '" + InKg + "','" + InPcs + "','" + godownId + "','" + locationId + "','" + outDate + "', '" + outType + "', '" + outTypeId + "', '" + outValue + "')");
                            InPcs--;

                            if (InPcs < 1)
                            {
                                SQLQuery.ExecNonQry("UPDATE tblFifo SET FracInHand='" + InPcs +
                                                    "' WHERE Id=(Select MAX(id) from tblFifo WHERE ItemCode='" + itemCode + "') ");
                            }
                        }
                    }
                }
                else if(outType== "Poly Production Output")
                {
                    //If Poly
                    while (InPcs>0)
                    {
                        //decimal inValueWIthPreviousValue = inValue + Convert.ToDecimal(finishedRow["OutValue"]);                        //decimal inValueWIthPreviousValue = inValue + Convert.ToDecimal(finishedRow["OutValue"]);
                        SQLQuery.ExecNonQry("INSERT INTO [tblFifo] ([ItemCode],[TransactionType],[SizeId],[ColorId],[BrandId],[InDate], [InType],[InTypeId],[InValue],[InKg],[InPcs],[GodownId],[LocationId],[OutDate],[OutType],[OutTypeId],[OutValue])" +
                                            " VALUES ('" + itemCode + "','" + transactionType + "','" + sizeId + "','" + colorId + "','" + brandId + "', '" + inDate + "', '" + outType + "', '" + inTypeId + "', '" +
                                            inValue + "', '" + InKg + "','" + InPcs + "','" + godownId + "','" + locationId + "','" + outDate + "', '', '" + outTypeId + "', '" + outValue + "')");
                        InPcs--;


                    }
                }
                else
                {
                    //If SemiFinished
                    //decimal inValueWithScrineenPrintValue = 0;
                    DataTable semiFinished = SQLQuery.ReturnDataTable(@"SELECT Id, ItemCode, TransactionType, SizeId, ColorId, BrandId, InDate, InType, InTypeId, InValue, InKg, InPcs, OutDate, OutType, OutTypeId, OutValue, FracInHand
                    FROM tblFifo WHERE OutTypeId='" + inTypeId + "'");
                    foreach (DataRow finishedRow in semiFinished.Rows)
                    {
                        //decimal inValueWIthPreviousValue = inValue + Convert.ToDecimal(finishedRow["OutValue"]);                        //decimal inValueWIthPreviousValue = inValue + Convert.ToDecimal(finishedRow["OutValue"]);
                        SQLQuery.ExecNonQry("INSERT INTO [tblFifo] ([ItemCode],[TransactionType],[SizeId],[ColorId],[BrandId],[InDate], [InType],[InTypeId],[InValue],[InKg],[InPcs],[GodownId],[LocationId],[OutDate],[OutType],[OutTypeId],[OutValue])" +
                                            " VALUES ('" + itemCode + "','" + transactionType + "','" + sizeId + "','" + colorId + "','" + brandId + "', '" + inDate + "', '" + inType + "', '" + inTypeId + "', '" +
                                            inValue + "', '" + InKg + "','" + InPcs + "','" + godownId + "','" + locationId + "','" + outDate + "', '" + outType + "', '" + outTypeId + "', '" + outValue + "')");
                        InPcs--;
                    }
                }
            }
            return amt;
        }
    }
}