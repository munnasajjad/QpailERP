using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using RunQuery;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;

//namespace RunQuery
//{
public class XEngine
{

    public static string NormalizeText(string txt)
    {
        string result = "";
        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        String[] s = Regex.Split(txt.Replace("_", " "), @"(?<!^)(?=[A-Z])");
        foreach (string word in s)
        {
            result += ti.ToTitleCase(word.Trim() + " ");
        }
        return result.Trim().Replace(" Id","");
    }
    public static string GenerateDropDown(string ddFieldName)
    {
        string pageLayout = "" + Environment.NewLine;
        pageLayout += "<tr>" + Environment.NewLine;
        pageLayout += "<td>" + NormalizeText(ddFieldName) + "</td>" + Environment.NewLine;
        pageLayout += "<td>" + Environment.NewLine;
        pageLayout += "<asp:DropDownList ID=\"dd" + ddFieldName.Replace(" ", "") + "\" runat=\"server\" CssClass=\"form-control select2me\"" + Environment.NewLine;
        pageLayout += "     AutoPostBack=\"True\" OnSelectedIndexChanged=\"dd" + ddFieldName + "_SelectedIndexChanged\">" + Environment.NewLine;
        pageLayout += "</asp:DropDownList>" + Environment.NewLine;
        pageLayout += "</td>" + Environment.NewLine;
        pageLayout += "</tr>" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        return pageLayout;
    }
    public static string BindDDList(string ddFieldName, string txtField, string valField, string tableName)
    {
        string pageLayout = "" + Environment.NewLine;
        pageLayout += "private void bindDD" + ddFieldName + "()" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        if (txtField == valField)
        {
            pageLayout += "SQLQuery.PopulateDropDown(\"Select " + txtField + " from " + tableName + "\", " + "dd" + ddFieldName + ", \"" + valField + "\", \"" + txtField + "\");" + Environment.NewLine;
        }
        else
        {
            pageLayout += "SQLQuery.PopulateDropDown(\"Select " + txtField + "," + valField + " from " + tableName + "\", " + "dd" + ddFieldName + ", \"" + valField + "\", \"" + txtField + "\");" + Environment.NewLine;
        }
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        return pageLayout + GenerateDDListEvents(ddFieldName);
    }
    //public static string GetFkTables(string tableName)
    //{
    //    DataTable dt = SQLQuery.ReturnDataTable(@"SELECT c.*
    //                                                FROM information_schema.columns c
    //                                                WHERE c.table_schema = 'dbo'    --or whatever
    //                                                AND c.table_name = '" + tableName + @"'
    //                                                ORDER BY c.ORDINAL_POSITION");
    //}
    public static string GenerateDDListEvents(string ddFieldName)
    {
        string pageLayout = "" + Environment.NewLine;
        pageLayout += "protected void dd" + ddFieldName + "_SelectedIndexChanged(object sender, EventArgs e)" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "BindGrid();" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        return pageLayout;
    }
    public static string GenerateCheckBox(string cbFieldName)
    {
        string pageLayout = "" + Environment.NewLine;
        pageLayout += "<tr>" + Environment.NewLine;
        pageLayout += "<td>" + NormalizeText(cbFieldName) + "</td>" + Environment.NewLine;
        pageLayout += "<td>" + Environment.NewLine;
        pageLayout += "<asp:CheckBox ID=\"cb" + cbFieldName.Replace(" ", "") + "\" runat=\"server\" " + Environment.NewLine;
        pageLayout += "     AutoPostBack=\"False\" >" + Environment.NewLine;
        pageLayout += "</asp:CheckBox>" + Environment.NewLine;
        pageLayout += "</td>" + Environment.NewLine;
        pageLayout += "</tr>" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        return pageLayout;
    }
    public static string GenerateTextBox(string name, string isNullable, string type, out string include2PageLoad)//Types: Int (Filter TextBox), DateTime (Calender), 
    {
        include2PageLoad = "";
        string pageLayout = "" + Environment.NewLine;
        pageLayout += "<tr>" + Environment.NewLine;
        pageLayout += "<td>" + NormalizeText(name) + "</td>" + Environment.NewLine;
        pageLayout += "<td>" + Environment.NewLine;
        pageLayout += "<asp:TextBox ID=\"txt" + name + "\" runat=\"server\" CssClass=\"form-control\"></asp:TextBox>" + Environment.NewLine;
        if (type == "int")
        {
            pageLayout += "<asp:FilteredTextBoxExtender ID=\"ftb" + name + "\" runat=\"server\" FilterType=\"Numbers\" ValidChars=\"0123456789\" TargetControlID=\"txt" + name + "\" />" + Environment.NewLine;
        }
        else if (type == "decimal")
        {
            pageLayout += "<asp:FilteredTextBoxExtender ID=\"ftb" + name + "\" runat=\"server\" FilterType=\"Custom, Numbers\" ValidChars=\".0123456789\" TargetControlID=\"txt" + name + "\" />" + Environment.NewLine;
        }
        else if (type == "datetime")
        {
            include2PageLoad = "txt" + name + ".Text=" + "DateTime.Now.ToString(\"dd/MM/yyyy\");" + Environment.NewLine;
            pageLayout += "<asp:CalendarExtender ID=\"ce" + name + "\" runat=\"server\" Format=\"dd/MM/yyyy\" TargetControlID=\"txt" + name + "\" />" + Environment.NewLine;
        }

        if (isNullable != "YES")
        {
            pageLayout += "<asp:RequiredFieldValidator ID=\"rfv" + name + "\" ControlToValidate=\"txt" + name + "\" Display=\"Dynamic\" SetFocusOnError=\"True\" runat=\"server\" ErrorMessage=\"Enter " + NormalizeText(name) + " field \" Style=\"margin-left: 30%\"></asp:RequiredFieldValidator>" + Environment.NewLine;
        }

        pageLayout += "</td>" + Environment.NewLine;
        pageLayout += "</tr>" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;

        return pageLayout;
    }

    public static void SaveASPXFile(string friendlyName, string formName, string docPath, string leftTexts, string rightTexts, string id, int count)
    {
        string pageLayout = "<%@ Page Title=\"" + friendlyName + "\" Language=\"C#\" MasterPageFile=\"~/app/MasterPage.Master\" AutoEventWireup=\"true\" CodeFile=\"" + formName + ".aspx.cs\" Inherits=\"app_" + formName + "\" %>" + Environment.NewLine;
        pageLayout += "<%@ Register Assembly=\"AjaxControlToolkit\" Namespace=\"AjaxControlToolkit\" TagPrefix=\"asp\" %>" + Environment.NewLine;
        pageLayout += "<asp:Content ID=\"Content1\" ContentPlaceHolderID=\"head\" runat=\"Server\">" + Environment.NewLine;
        pageLayout += "</asp:Content>" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "<asp:Content ID=\"Content2\" ContentPlaceHolderID=\"BodyContent\" runat=\"Server\">" + Environment.NewLine;
        pageLayout += "<asp:ScriptManager ID=\"ScriptManager1\" runat=\"server\"> </asp:ScriptManager>" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "<asp:UpdateProgress ID=\"updProgress\" AssociatedUpdatePanelID=\"pnl\" runat=\"server\">" + Environment.NewLine;
        pageLayout += "<ProgressTemplate>" + Environment.NewLine;
        pageLayout += "<div id=\"IMGDIV\" style=\"position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;\">" + Environment.NewLine;
        pageLayout += "<img src=\"../images/loader.gif\" alt=\"Processing... Please Wait.\" /></div>" + Environment.NewLine;
        pageLayout += "</ProgressTemplate>" + Environment.NewLine;
        pageLayout += "</asp:UpdateProgress>" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "<asp:UpdatePanel ID=\"pnl\" runat=\"server\">" + Environment.NewLine;
        
        pageLayout += "<Triggers>" + Environment.NewLine;
        pageLayout += "<asp:PostBackTrigger ControlID=\"btnSave\" />" + Environment.NewLine;
        pageLayout += "</Triggers>" + Environment.NewLine;

        pageLayout += "<ContentTemplate>" + Environment.NewLine;
        pageLayout += "<script type=\"text/javascript\" language=\"javascript\">" + Environment.NewLine;
        pageLayout += "Sys.Application.add_load(callJquery);" + Environment.NewLine;
        pageLayout += "</script>" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "<div class=\"col-lg-6\">" + Environment.NewLine;
        pageLayout += "<section class=\"panel\">" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "<fieldset>" + Environment.NewLine;
        pageLayout += "<legend>" + friendlyName + "</legend>" + Environment.NewLine;
        pageLayout += "<table border=\"0\" class=\"membersinfo tdfirstright bg-green\" width=\"100%\">" + Environment.NewLine;
        pageLayout += "<tr>" + Environment.NewLine;
        pageLayout += "<td align=\"center\" colspan=\"2\">" + Environment.NewLine;
        pageLayout += "<asp:Label ID=\"lblMsg\" runat=\"server\" EnableViewState=\"false\"></asp:Label>" + Environment.NewLine;
        pageLayout += "<asp:Label ID=\"lblId\" Visible=\"false\" runat=\"server\" Text=\"\"></asp:Label>" + Environment.NewLine;
        pageLayout += "</td></tr>" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;

        //All Left controls//////////////////////////
        pageLayout += leftTexts + Environment.NewLine;
        //End Left controls//////////////////////////

        pageLayout += "<tr style=\"background: none\">" + Environment.NewLine;
        pageLayout += "<td></td>" + Environment.NewLine;
        pageLayout += "<td>" + Environment.NewLine;
        pageLayout += "<asp:Button ID=\"btnSave\" CssClass=\"btn btn-s-md btn-primary\" runat=\"server\" Text=\"Save\" OnClick=\"btnSave_OnClick\" />" + Environment.NewLine;
        pageLayout += "<asp:Button ID=\"btnClear\" type=\"reset\" CssClass=\"btn btn-s-md btn-white\" runat=\"server\" Text=\"Cancel\" OnClick=\"btnClear_OnClick\" />" + Environment.NewLine;
        pageLayout += "</td>" + Environment.NewLine;
        pageLayout += "</tr>" + Environment.NewLine;
        pageLayout += "</table>" + Environment.NewLine;
        pageLayout += "</fieldset>" + Environment.NewLine;
        pageLayout += "</section>" + Environment.NewLine;
        pageLayout += "</div>" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "<div class=\"col-lg-6\">" + Environment.NewLine;
        pageLayout += "<section class=\"panel\">" + Environment.NewLine;
        pageLayout += "<fieldset>" + Environment.NewLine;
        pageLayout += "<legend>Saved Data</legend>" + Environment.NewLine;
        if (count > 5)
        {
            pageLayout += "<div class=\"table-responsive\">" + Environment.NewLine;
            pageLayout += "<asp:GridView width=\"" + count * 20 + "%\" ID=\"GridView1\" runat=\"server\" CssClass=\"table table-bordered table-striped\" AutoGenerateColumns=\"False\" BackColor=\"White\" BorderColor=\"#DEDFDE\"" + Environment.NewLine;
        }
        else
        {
            pageLayout += "<asp:GridView ID=\"GridView1\" runat=\"server\" CssClass=\"table table-bordered table-striped\" AutoGenerateColumns=\"False\" BackColor=\"White\" BorderColor=\"#DEDFDE\"" + Environment.NewLine;
        }

        pageLayout += "    BorderStyle=\"None\" BorderWidth=\"1px\" CellPadding=\"4\" ForeColor=\"Black\"" + Environment.NewLine;
        pageLayout += "    GridLines=\"Vertical\" DataKeyNames=\"" + id + "\" OnSelectedIndexChanged=\"GridView1_OnSelectedIndexChanged\" OnRowDeleting=\"GridView1_OnRowDeleting\">" + Environment.NewLine;
        pageLayout += "<Columns>" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "<asp:TemplateField HeaderText=\"#Sl\" ItemStyle-Width=\"20px\">" + Environment.NewLine;
        pageLayout += "<ItemTemplate>" + Environment.NewLine;
        pageLayout += "<%#Container.DataItemIndex+1 %>." + Environment.NewLine;
        pageLayout += "<asp:Label ID=\"Label1\" runat=\"server\" visible=\"false\" Text='<%# Bind(\"" + id + "\") %>'></asp:Label>" + Environment.NewLine;
        pageLayout += "</ItemTemplate>" + Environment.NewLine;
        pageLayout += "<ItemStyle Width=\"20px\" HorizontalAlign=\"Center\" />" + Environment.NewLine;
        pageLayout += "</asp:TemplateField>" + Environment.NewLine;

        //All Right controls//////////////////////////
        pageLayout += rightTexts + Environment.NewLine;
        //End Right controls//////////////////////////

        pageLayout += "<asp:TemplateField ItemStyle-HorizontalAlign=\"Center\" ShowHeader=\"False\">" + Environment.NewLine;
        pageLayout += "<ItemTemplate>" + Environment.NewLine;
        pageLayout += "<asp:ImageButton ID=\"ImageButton1\" runat=\"server\" CausesValidation=\"False\" CommandName=\"Select\" ImageUrl=\"~/images/edit.png\" Text=\"Select\" />" + Environment.NewLine;
        pageLayout += "<asp:ImageButton ID=\"ImageButton2\" runat=\"server\" CausesValidation=\"False\" CommandName=\"Delete\" ImageUrl=\"~/images/delete.gif\" Text=\"Delete\" />" + Environment.NewLine;
        pageLayout += "<asp:ConfirmButtonExtender TargetControlID=\"ImageButton2\" ID=\"confBtnDelete\" runat=\"server\" DisplayModalPopupID=\"ModalPopupExtender1\"></asp:ConfirmButtonExtender>" + Environment.NewLine;
        pageLayout += "<asp:ModalPopupExtender ID=\"ModalPopupExtender1\" runat=\"server\" TargetControlID=\"ImageButton2\"" + Environment.NewLine;
        pageLayout += "    PopupControlID=\"PNL\" OkControlID=\"ButtonOk\" CancelControlID=\"ButtonCancel\" BackgroundCssClass=\"modalBackground\" />" + Environment.NewLine;
        pageLayout += "<asp:Panel ID=\"PNL\" runat=\"server\" Style=\"display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;\">" + Environment.NewLine;
        pageLayout += "<b style=\"color: red\">This entry will be deleted permanently!</b><br />" + Environment.NewLine;
        pageLayout += "Are you sure you want to delete this ?<br /><br />" + Environment.NewLine;
        pageLayout += "<div style=\"text-align: right;\">" + Environment.NewLine;
        pageLayout += "<asp:Button ID=\"ButtonOk\" runat=\"server\" CssClass=\"btn btn-success\" Text=\"OK\" />" + Environment.NewLine;
        pageLayout += "<asp:Button ID=\"ButtonCancel\" CssClass=\"btn_small btn_orange\" runat=\"server\" Text=\"Cancel\" />" + Environment.NewLine;
        pageLayout += "</div>" + Environment.NewLine;
        pageLayout += "</asp:Panel>" + Environment.NewLine;
        pageLayout += "</ItemTemplate>" + Environment.NewLine;
        pageLayout += "</asp:TemplateField>" + Environment.NewLine;
        pageLayout += "</Columns>" + Environment.NewLine;
        pageLayout += "<RowStyle BackColor=\"#F7F7DE\" CssClass=\"txtMult\" />" + Environment.NewLine;
        pageLayout += "<PagerStyle BackColor=\"#F7F7DE\" ForeColor=\"Black\" HorizontalAlign=\"Right\" />" + Environment.NewLine;
        pageLayout += "<SelectedRowStyle BackColor=\"#EEF7F2\" Font-Bold=\"True\" ForeColor=\"#615B5B\" />" + Environment.NewLine;
        pageLayout += "<HeaderStyle BackColor=\"#FF6600\" Font-Bold=\"True\" ForeColor=\"#222\" />" + Environment.NewLine;
        pageLayout += "<AlternatingRowStyle BackColor=\"White\" />" + Environment.NewLine;
        pageLayout += "</asp:GridView>" + Environment.NewLine;
        if (count > 5)
        {
            pageLayout += "</div>" + Environment.NewLine;
        }
        pageLayout += "" + Environment.NewLine;
        pageLayout += "</fieldset>" + Environment.NewLine;
        pageLayout += "</section>" + Environment.NewLine;
        pageLayout += "</div>" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "</ContentTemplate>" + Environment.NewLine;
        pageLayout += "</asp:UpdatePanel>" + Environment.NewLine;
        pageLayout += "</asp:Content>" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;


        // Write the text to a new file named "WriteFile.txt".
        File.WriteAllText(Path.Combine(docPath, formName + ".aspx"), pageLayout);
    }

    public static string GenerateGridBoundField(string fieldName)
    {
        string pageLayout = "" + Environment.NewLine;
        pageLayout += "<asp:BoundField DataField=\"RoadCircle\" HeaderText=\"" + fieldName + "\" SortExpression=\"RoadCircle\" />" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        return pageLayout;
    }
    public static string GenerateGridTemplateField(string fieldName)
    {
        string pageLayout = "" + Environment.NewLine;
        pageLayout += "<asp:TemplateField HeaderText=\"sl\" SortExpression=\"sl\">" + Environment.NewLine;
        pageLayout += "<ItemTemplate>" + Environment.NewLine;
        pageLayout += "<asp:Label ID=\"Label1\" runat=\"server\" Text='<%# Bind(\"" + fieldName + "\") %>'></asp:Label>" + Environment.NewLine;
        pageLayout += "</ItemTemplate>" + Environment.NewLine;
        pageLayout += "</asp:TemplateField>" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        return pageLayout;
    }


    public static void SaveCSharpFile(string formName, string docPath, string pageLoad, string beforInsertQry, string insertQuery, string updateQuery, string selectQuery, string deleteQuery, string editingControls, string clearControls, string gridSQL, string ddlEvents)
    {
        string pageLayout = "";
        pageLayout += @"
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "public partial class app_" + formName + " : System.Web.UI.Page" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "protected void Page_Load(object sender, EventArgs e)" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "Page.Form.Attributes.Add(\"enctype\", \"multipart/form-data\");" + Environment.NewLine;
        pageLayout += "btnSave.Attributes.Add(\"onclick\", \" this.disabled = true; \" + ClientScript.GetPostBackEventReference(btnSave, null) + \";\");" + Environment.NewLine;
        pageLayout += "if (!IsPostBack){" + Environment.NewLine;
        pageLayout += pageLoad + Environment.NewLine;//Load Dates dropdowns & GridView1
        //pageLayout += "BindGrid();" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "private void Notify(string msg, string type, Label lblNotify)" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "ScriptManager.RegisterClientScriptBlock(this, GetType(), \"Sc\", \"$.notify('\" + msg + \"','\" + type + \"');\", true);" + Environment.NewLine;
        pageLayout += "//Types: success, info, warn, error" + Environment.NewLine;
        pageLayout += "lblNotify.Attributes.Add(\"class\", \"xerp_\" + type);" + Environment.NewLine;
        pageLayout += "lblNotify.Text = msg;" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        //Save Button   ************************** Dont forget to keep log records of ExcNonQry ********************
        pageLayout += "protected void btnSave_OnClick(object sender, EventArgs e)" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "try" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += beforInsertQry + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "string lName = Page.User.Identity.Name.ToString();" + Environment.NewLine;
        pageLayout += "if (btnSave.Text == \"Save\")" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "if (SQLQuery.OparatePermission(lName, \"Insert\") == \"1\")" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "RunQuery.SQLQuery.ExecNonQry(\"" + insertQuery + " \");" + Environment.NewLine;
        pageLayout += "ClearControls();" + Environment.NewLine;
        pageLayout += "Notify(\"Successfully Saved...\", \"success\", lblMsg);" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "else" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "Notify(\"You are not eligible to attempt this operation!\", \"warn\", lblMsg);" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "else" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "if (SQLQuery.OparatePermission(lName, \"Update\") == \"1\")" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "RunQuery.SQLQuery.ExecNonQry(\"" + updateQuery + " \");" + Environment.NewLine;
        pageLayout += "ClearControls();" + Environment.NewLine;
        pageLayout += "btnSave.Text = \"Save\";" + Environment.NewLine;
        pageLayout += "Notify(\"Successfully Updated...\", \"success\", lblMsg);" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "else" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "Notify(\"You are not eligible to attempt this operation!\", \"warn\", lblMsg);" + Environment.NewLine;
        //pageLayout += "}" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "catch (Exception ex)" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "Notify(ex.ToString(), \"error\", lblMsg);" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "finally" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "BindGrid();" + Environment.NewLine;//Bind grif
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;

        //Load Edit Mode
        pageLayout += "protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "try" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "string lName = Page.User.Identity.Name.ToString();" + Environment.NewLine;
        pageLayout += "if (SQLQuery.OparatePermission(lName, \"Read\") == \"1\")" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "int index = Convert.ToInt32(GridView1.SelectedIndex);" + Environment.NewLine;
        pageLayout += "Label lblEditId = GridView1.Rows[index].FindControl(\"Label1\") as Label;" + Environment.NewLine;
        pageLayout += "lblId.Text = lblEditId.Text;" + Environment.NewLine;
        pageLayout += "DataTable dt = SQLQuery.ReturnDataTable(\"" + selectQuery + "\");" + Environment.NewLine;
        pageLayout += "foreach (DataRow dtx in dt.Rows)" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += editingControls + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "btnSave.Text = \"Update\";" + Environment.NewLine;
        pageLayout += "Notify(\"Edit mode activated ...\", \"info\", lblMsg);" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "else" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "Notify(\"You are not authorized to select this data\", \"warn\", lblMsg);" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "catch (Exception ex)" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "Notify(ex.ToString(), \"error\", lblMsg);" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;

        //Delete a Row
        pageLayout += "protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "string lName = Page.User.Identity.Name.ToString();" + Environment.NewLine;
        pageLayout += "if (SQLQuery.OparatePermission(lName, \"Delete\") == \"1\")" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "int index = Convert.ToInt32(e.RowIndex);" + Environment.NewLine;
        pageLayout += "Label lblId = GridView1.Rows[index].FindControl(\"Label1\") as Label;" + Environment.NewLine;

        pageLayout += "RunQuery.SQLQuery.ExecNonQry(\"" + deleteQuery + " \");" + Environment.NewLine;
        pageLayout += "BindGrid();" + Environment.NewLine;
        pageLayout += "Notify(\"Successfully Deleted...\", \"success\", lblMsg);" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "else" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "Notify(\"You are not eligible to attempt this operation!\", \"warn\", lblMsg);" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;

        pageLayout += "protected void btnClear_OnClick(object sender, EventArgs e)" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "Response.Redirect(\"./Default.aspx\");" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;

        //Now load the Dropdown codes
        pageLayout += "private void BindGrid()" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += "DataTable dt = SQLQuery.ReturnDataTable(\"" + gridSQL + "\");" + Environment.NewLine;
        pageLayout += "GridView1.DataSource = dt;" + Environment.NewLine;
        pageLayout += "GridView1.DataBind();" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;

        pageLayout += ddlEvents + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;

        pageLayout += "private void ClearControls()" + Environment.NewLine;
        pageLayout += "{" + Environment.NewLine;
        pageLayout += clearControls + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;

        pageLayout += "" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "" + Environment.NewLine;
        pageLayout += "}" + Environment.NewLine;


        // Write the text to a new file named "WriteFile.txt".
        File.WriteAllText(Path.Combine(docPath, formName + ".aspx.cs"), pageLayout);
    }





}
//}