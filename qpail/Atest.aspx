<%@ Page Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="Atest.aspx.cs" Inherits="Atest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="col-lg-6">
        <asp:Label ID="lblMsg" runat="server" EnableViewState="false" />
        <asp:HiddenField id="lableIdHField" runat="server"/>
        <table>
            <tr>
                <td>Name:</td>
                <td>
                    <asp:DropDownList ID="ddName" runat="server" DataSourceID="SqlDataSource1" DataTextField="CompanyName" DataValueField="CompanyName"></asp:DropDownList></td>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT         TeamTest.Address AS Expr2, TeamTest.Date AS Expr3, TeamTest.EntryBy AS Expr4, TeamTest.*, TeamTest.Name AS Expr1, TeamTest.id AS Expr5, Company.CompanyName
FROM            TeamTest INNER JOIN
                         Company ON TeamTest.id = Company.CompanyID"></asp:SqlDataSource>

            </tr>
            <tr>
                <td>Address:</td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Date:</td>
                <td>
                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" />
                    <asp:CalendarExtender ID="CalendarExtender2"
                        runat="server" Format="dd/MM/yyyy" TargetControlID="txtDate">
                    </asp:CalendarExtender>
                </td>

            </tr>
            <tr>

                <td>
                    <asp:Button ID="btnInsert" runat="server" Text="Insert" CssClass="btn btn-success" OnClick="btnInsert_Click" /></td>
            </tr>
        </table>
    </div>
    <div class="col-lg-6">
        <section class="panel">
            <legend>Show My Data</legend>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Height="143px" OnRowDeleting="GridView1_RowDeleting1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                <Columns>
                    <asp:TemplateField HeaderText="ID" SortExpression="id">
                        
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/edit.png" Text="Select" />
                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif" Text="Delete" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <%--<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [Name], [Address], [Date] FROM [TeamTest]"></asp:SqlDataSource>--%>
        </section>
    </div>

</asp:Content>


<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .panel {
            height: 291px;
            width: 756px;
        }
    </style>
</asp:Content>



