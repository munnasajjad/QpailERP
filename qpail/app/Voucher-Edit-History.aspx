<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Voucher-Edit-History.aspx.cs" Inherits="app_Voucher_Edit_History" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        table {
            border-collapse: collapse;
        }

        td {
            padding: 0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


    <div id="content">

        <div id="ComInfo" class="hidden">
            <asp:Image ID="Image1" runat="server" />
            <p>
                <b>
                    <asp:Label ID="lblName" runat="server"></asp:Label></b><br />
                <asp:Label ID="lblArress" runat="server"></asp:Label>
            </p>

        </div>
        <h3 class="page-title">Voucher Edit/Delete History</h3>
        
       
         
                                    <%--<legend>Search Terms</legend>--%>
                                    <table border="0" width="100%" style="width: 100%" class="table1">
                                        <tr>
                                           <th>Type</th>
                                            <th>Date From</th>                                                                                                                            
                                            
                                            <th>Date To</th>
                                             <th></th>
                                        </tr>
                                        <tr>
                                            <td style=" vertical-align: top; text-align: justify">
                                                <asp:RadioButton ID="rbEdit"  runat="server" Checked="True" Text="Edited Vouchers" GroupName="g" /><br />
                                                <asp:RadioButton ID="rbDelete" runat="server"  Text="Deleted Vouchers" GroupName="g" />          
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtdateFrom" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtdateFrom" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            
                                            <td>
                                                <asp:TextBox ID="txtdateTo" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="txtdateTo" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                         
                                            <td style="text-align: center; vertical-align: top;">
                                                <asp:Button ID="btnSearch" CssClass="btn btn-s-md btn-primary" runat="server" Text="SHOW" OnClick="btnSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>


        <div class="table-responsive">

            <asp:Literal ID="ltrBody" runat="server"></asp:Literal>
            

            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                SelectCommand="SELECT [VID], [VoucherNo], [VoucherDate], [VoucherDescription], [VoucherEntryBy], [VoucherEntryDate],[VoucherPostby], [VoucherAmount], [VoucherEntryTime] FROM [VoucherMasterEditHistory]">
            </asp:SqlDataSource>

            <asp:GridView ID="GridView1" runat="server" Width="100%" CssClass="table table-striped table-bordered"
                AllowSorting="True" AutoGenerateColumns="False" >

                <Columns>
                    <asp:BoundField DataField="VID" HeaderText="VID" SortExpression="VID" />
                    <asp:TemplateField HeaderText="Voucher No." SortExpression="VoucherNo">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%#"./reports/voucher.aspx?inv=" + Eval("VoucherNo") %>'>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("VoucherNo") %>'></asp:Label>
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                    <asp:BoundField DataField="VoucherDate" HeaderText="V. Date" SortExpression="VoucherDate" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="VoucherDescription" HeaderText="Description" SortExpression="VoucherDescription" />
                    <asp:BoundField DataField="VoucherEntryBy" HeaderText="Entry By" SortExpression="VoucherEntryBy" /> 
                    <asp:BoundField DataField="VoucherPostby" HeaderText="Delete By" SortExpression="VoucherPostby" />
                    <asp:BoundField DataField="VoucherAmount" HeaderText="Amount" SortExpression="VoucherAmount" />
                    <asp:BoundField DataField="VoucherEntryTime" HeaderText="Time" SortExpression="VoucherEntryTime"  />
                </Columns>
                <EmptyDataTemplate>
                    No Income Data Found !
                </EmptyDataTemplate>
            </asp:GridView>

            <iframe id="if1" runat="server" height="800px" width="100%" ></iframe>
            <%--<h4>Expenses</h4>
        <asp:GridView ID="GridView1" runat="server"
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
        BorderWidth="1px" Width="100%"
        CellPadding="3" >
            <RowStyle ForeColor="#000066" />
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <EmptyDataTemplate>
                You Dont Have any Expenses!
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>--%>
        </div>
        <p style="text-align: center; font-weight: bold;">
             
            <asp:Label ID="lblBalance" runat="server" Text=""></asp:Label>
        </p>


    </div>


</asp:Content>

