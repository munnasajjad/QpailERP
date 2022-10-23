<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Emp-Monthly-Attn.aspx.cs" Inherits="app_Emp_Monthly_Attn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
    
<h2>Attendence Finalization </h2>
<fieldset>
    <legend>Finalize Attendences with Corrections</legend>
    &nbsp;<table class="table1">
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
                    <asp:Label ID="In" runat="server" Text="Office In-Time"></asp:Label>
                </td>
                <td class="textbox">
                    <asp:TextBox ID="txtIn" runat="server">08:00:00</asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
             </tr>
            <tr>
                <td class="label">
                    <asp:Label ID="Out" runat="server" Text="Office Out-Time"></asp:Label>
                </td>
                <td class="textbox">
                    <asp:TextBox ID="txtOut" runat="server">17:00:00</asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
                </tr>
            
            <tr>
                <td class="label">
                    &nbsp;</td>
                <td class="textbox">
                    <asp:Label ID="lblerror" runat="server"></asp:Label>
                </td>
            <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="label">
                    &nbsp;</td>
                <td class="textbox">
        
    <asp:GridView ID="Attendence" runat="server" AutoGenerateColumns="False" 
                        onrowdatabound="Attendence_RowDataBound" AllowPaging="True" 
                        AllowSorting="True" DataKeyNames="EID" DataSourceID="AttDataSource">
                    <Columns>
                        <asp:TemplateField HeaderText="Card No" SortExpression="EID">
                            <EditItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("EID") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("EID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee Name" SortExpression="EName">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("EName") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("EName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="In Time" SortExpression="InTime">
                        <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("InTime") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Width="70px" Text='<%# Bind("InTime") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Out Time" SortExpression="OutTime">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("OutTime") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("OutTime") %>' 
                                    Width="69px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OT Hour" SortExpression="OTHour">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Text=""></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOT" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField />
                    </Columns>
    </asp:GridView>
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
                    <asp:Label ID="lblSerial" runat="server" Text="Attendence Date: "></asp:Label>
                </td>
                <td class="textbox">
                    <asp:TextBox ID="txtDate" runat="server" Width="153px"></asp:TextBox>
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txtDate" runat="server" />
                </td>
            <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="label">
                    &nbsp;</td>
                <td class="textbox">
                    <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />
                    <asp:Label ID="Label4" runat="server"></asp:Label>
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
        </table>
        <div>
            <asp:SqlDataSource ID="AttDataSource" runat="server" 
                ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" 
                SelectCommand="select EID, EName, InTime, OutTime from EmployeeInfo inner join Cdata on EmployeeInfo.CardSerial= Cdata.EmployeeID"></asp:SqlDataSource>
        </div>
</fieldset>

</asp:Content>

