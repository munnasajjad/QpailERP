<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Unlock.aspx.cs" Inherits="app_Unlock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">
<h2>Check Restricted Users</h2>
<fieldset>
    <legend>Lock/Unlock Members Account</legend>
    
    &nbsp;<table class="table1">
        <tr><%--Department ID--%>
            <td>
                <asp:Label ID="lblDid" runat="server" Text="User ID: "></asp:Label>
            </td>
            <td class="textbox">
                <asp:TextBox ID="txtUid" runat="server"></asp:TextBox>
                <asp:Button ID="btnSave0" runat="server" Text="Check" onclick="btnSave0_Click" />
                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr><%--Department Name--%>
            <td>
                <asp:Label ID="lblDid0" runat="server" Text="Current Status: "></asp:Label>
            </td>
            <td class="textbox">
                <asp:Label ID="lblCurrEmail" CssClass="msg" runat="server"></asp:Label>
                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDeptName" runat="server" Text="New Status: "></asp:Label>
            </td>
            <td class="textbox">
                <asp:RadioButton ID="RadioButton1" runat="server" GroupName="g" Text="Unblock" />
                <asp:RadioButton ID="RadioButton3" runat="server" GroupName="g" Text="Unblock All" Checked="True"  />
                <asp:RadioButton ID="RadioButton2" runat="server" GroupName="g" Text="Block" />
                
            </td>
            <td>
                &nbsp;</td>
        </tr>
        
        <tr>
            <td>
                &nbsp;</td>
            <td class="textbox">
                <asp:Button ID="btnSave" runat="server" Text="Unlock All Users" 
                    onclick="btnSave_Click" />
                <asp:Label ID="lblMsg" CssClass="msg" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        
        <tr>
            <td>
                &nbsp;</td>
            <td class="textbox">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        </table>
</fieldset> 
<h2>List of Blocked Users</h2>
    <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1">
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT UserName, Email, LastLockoutDate

FROM vw_aspnet_MembershipUsers 

WHERE IsLockedOut = 1"></asp:SqlDataSource>
</asp:Content>

