<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="app_ResetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">
<h2>Reset User Password</h2>
<fieldset>
    <legend>Change Email ID of an Account</legend>
    
    &nbsp;<table class="table1">
        <tr><%--Department ID--%>
            <td>
                <asp:Label ID="lblDid" runat="server" Text="User ID: "></asp:Label>
            </td>
            <td class="textbox">
                <asp:TextBox ID="txtUid" runat="server"></asp:TextBox>
                <asp:Button ID="btnSave0" runat="server" Text="Load" onclick="btnSave0_Click" />
                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr><%--Department Name--%>
            <td>
                <asp:Label ID="lblDid0" runat="server" Text="Current Email Address: "></asp:Label>
            </td>
            <td class="textbox">
                <asp:Label ID="lblCurrEmail" CssClass="msg" runat="server"></asp:Label>
                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDeptName" runat="server" Text="New Password: "></asp:Label>
            </td>
            <td class="textbox">
                <asp:TextBox ID="txtPassword" runat="server" width="300px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        
        <tr>
            <td>
                &nbsp;</td>
            <td class="textbox">
                <asp:Button ID="btnSave" runat="server" Text="Save New Password" 
                    onclick="btnSave_Click" />
                <asp:Label ID="lblMsg" CssClass="msg" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        </table>
</fieldset> 

</asp:Content>

