<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Emp-Salary-Process.aspx.cs" Inherits="app_Emp_Salary_Process" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">

<h2>Attendence Finalization </h2>
<fieldset>
    <legend>Date-wise Attendence Correction </legend>
    &nbsp;<table class="table1">
            <tr>
                <td class="label">
                    <asp:Label ID="lblSerial" runat="server" Text="Final For the Month of: "></asp:Label>
                </td>
                <td class="textbox">
                    <asp:DropDownList ID="ddMonth" runat="server">
                    </asp:DropDownList>
                
                </td>
            <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="label">
                    &nbsp;</td>
                <td class="textbox">
                    <asp:Button ID="btnSave" runat="server" Text="Process The Month Salary" 
                        onclick="btnSave_Click" />
                        
                        <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnSave" ConfirmText="Are you sure to finalize the Salary of the month? Once its done, can't be undone.">
                </asp:ConfirmButtonExtender>  
                </td>
            <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="label">
                    &nbsp;</td>
                <td class="textbox">
                    &nbsp;</td>
            <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="label">
                    &nbsp;</td>
                <td class="textbox">
                    &nbsp;</td>
            <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="label">
                    &nbsp;</td>
                <td class="textbox">
                    &nbsp;</td>
            <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="label">
                    &nbsp;</td>
                <td class="textbox">
                    &nbsp;</td>
            <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="label">
                    &nbsp;</td>
                <td class="textbox">
                    &nbsp;</td>
            <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="label">
                    &nbsp;</td>
                <td class="textbox">
                    &nbsp;</td>
            <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="label">
                    &nbsp;</td>
                <td class="textbox">
                    &nbsp;</td>
            <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="label">
                    &nbsp;</td>
                <td class="textbox">
                    &nbsp;</td>
            <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="label">
                    &nbsp;</td>
                <td class="textbox">
                    &nbsp;</td>
            <td>
                &nbsp;</td>
            </tr>
        </table>
</fieldset>


</asp:Content>

