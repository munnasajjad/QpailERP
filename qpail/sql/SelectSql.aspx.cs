using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class sql_SelectSql : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_OnClick(object sender, EventArgs e)
    {
        string tableName = "TableName";
        if (txtTable.Text!="")
        {
            tableName = txtTable.Text;
        }
        string keyColumn = "sl";
        if (txtKey.Text != "")
        {
            keyColumn = txtKey.Text;
        }

        string s = txtSqlText.Text;
        string query = "DataTable dtx = SQLQuery.ReturnDataTable(@\"SELECT TOP (1) ", result = "";
        
        string[] words = s.Split(',');
        foreach (string word in words)
        {
            string txt = word.Trim().TrimEnd(',');
            txt = txt.Trim();

            query += txt + ", ";

            string controlID = txt;
            if (controlID.Substring(0,2)=="tx")
            {
                controlID = controlID + ".Text";
            }
            else if (controlID.Substring(0, 2) == "dd")
            {
                controlID = controlID + ".SelectedValue";
            }
            else
            {
                controlID = controlID + ".Text";
            }

            result += controlID + " = drx[\"" + txt + "\"].ToString();<br/>"; 
        }
        query = query.Trim().TrimEnd(',');
        query += " FROM [" + tableName + "] WHERE (" + keyColumn + "= '\" + " + keyColumn + " + \"')\");<br>";
        query += " foreach (DataRow drx in dtx.Rows)<br>{<br>" + result;
        query += " <br>}";

        lblResult.Text = query;

        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "copyTxt()", true);
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "copyTxt()", true);
    }
}