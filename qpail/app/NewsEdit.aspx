<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="NewsEdit.aspx.cs" Inherits="app_NewsEdit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">


<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<fieldset>
    <legend>News Update/Media Coverage Edit</legend>
    <h2>Old News Update/Delete</h2>
    &nbsp;<table class="table1">
        <tr><%--Department ID--%>
            <td class="label">
                <asp:Label ID="lblDid" runat="server" Text="News Head Line: "></asp:Label>
            </td>
            <td class="textbox">
               
                <asp:DropDownList ID="ddProject" runat="server" Width="505px" 
                    AutoPostBack="True" onselectedindexchanged="ddProject_SelectedIndexChanged">
                </asp:DropDownList>
                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr><%--Department Name--%>
            <td class="label">
                <asp:Label ID="lblDeptName" runat="server" Text="News Detail: "></asp:Label>
            </td>
            <td class="textbox">
                <asp:TextBox ID="msgMPO" runat="server" Width="100%" />
                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="label">
                &nbsp;</td>
            <td class="textbox">
                <asp:Button ID="btnSave" runat="server" Text="Update" 
                    onclick="btnSave_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" 
                    onclick="btnDelete_Click" />
                <asp:Label ID="lblMsg" CssClass="msg" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        
       
        </table>
</fieldset> 




</asp:Content>

