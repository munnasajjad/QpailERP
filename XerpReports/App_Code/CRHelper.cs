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
using System.IO;
/// <summary>
/// Summary description for RunQuery
/// </summary>
/// 

namespace CRHelper
{
    public class GetRespose
    {

        public static void AddImageColumn(DataTable objDataTable, string strFieldName)
        {
            try
            {
                DataColumn objDataColumn = new DataColumn(strFieldName, Type.GetType("System.Byte[]"));
                objDataTable.Columns.Add(objDataColumn);
            }
            catch (Exception ex)
            {

            }
        }
        public static void LoadImage(DataRow objDataRow, string strImageField, string FilePath)
        {
            try
            {
                FileStream fs = new FileStream(FilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                byte[] Image = new byte[fs.Length];
                fs.Read(Image, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                objDataRow[strImageField] = Image;
            }
            catch (Exception ex)
            {

            }
        }

    }

}