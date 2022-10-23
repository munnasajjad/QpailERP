<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ALLEntries.aspx.cs" Inherits="Application_Reports_ALLEntries" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Daily Vouchers</title>
</head>
<body>
    <form id="form1" runat="server">
    
<div id="content">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

        <div id="ComInfo">
            <asp:Image ID="Image1" runat="server" />
            <p>
                <b>
                    <asp:Label ID="lblName" runat="server" ></asp:Label></b><br />
                <asp:Label ID="lblArress" runat="server" ></asp:Label>
            </p>
            <h3>Date-wise Voucher Searching</h3>
        </div>
         <div id="qPanel">
        <div id="datePanel">
    <table><tr>
    <td>Date From</td><td>
        <asp:TextBox ID="txtDateFrom" runat="server"></asp:TextBox>
        
        <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
                    Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
        </td>
        <td> </td>
   
    <%--</tr><tr>--%>
    <td>Date To</td><td>
        <asp:TextBox ID="txtDateTo" runat="server"></asp:TextBox>
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
                    Enabled="True" TargetControlID="txtDateTo" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
            </td>    
    <%--</tr>
    <tr>--%><td>
        &nbsp;</td><td>
        <asp:Button ID="btnShow" runat="server" Text="Show" onclick="btnShow_Click" /></td></tr>
    </table>
    
   </div>
    </div>
<%--</div>--%>
<br />
<br />
    
    <asp:UpdatePanel ID="pan" runat="server"><ContentTemplate>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" 
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" AutoGenerateEditButton="True" 
        BorderWidth="1px" Width="100%"
        CellPadding="3">
            <RowStyle ForeColor="#000066" />
            <Columns>
                <asp:BoundField DataField="VoucherNo" HeaderText="VoucherNo" SortExpression="VoucherNo" />
                <asp:BoundField DataField="VoucherDate" HeaderText="VoucherDate"  SortExpression="VoucherDate" />
                <asp:BoundField DataField="AccountsHeadName" HeaderText="AccountsHeadName" SortExpression="AccountsHeadName" />
                <asp:BoundField DataField="VoucherDR" HeaderText="VoucherDR" SortExpression="VoucherDR" DataFormatString="{0:N}" />
                <asp:BoundField DataField="VoucherCR" HeaderText="VoucherCR" SortExpression="VoucherCR"  DataFormatString="{0:N}"/>
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <EmptyDataTemplate>
                No Voucher Found within given date range!
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"         
        
        SelectCommand="SELECT b.VoucherNo, a.VoucherDate, b.AccountsHeadName, b.VoucherDR, b.VoucherCR FROM VoucherDetails AS b INNER JOIN VoucherMaster AS a ON a.VoucherNo = b.VoucherNo WHERE (a.VoucherDate &gt;= @DateFrom) AND (a.VoucherDate &lt;= @DateTo) AND ISApproved<>'C' ORDER BY a.VoucherDate, b.SerialNo" 
        
        UpdateCommand="Update [VoucherDetails] set [VoucherDR]=@VoucherDR, [VoucherCR]=@VoucherCR where [VoucherNo]=@VoucherNo and [AccountsHeadID]=@AccountsHeadID">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtDateFrom" Name="DateFrom" Type="DateTime" PropertyName="Text" />
                <asp:ControlParameter ControlID="txtDateTo" Name="DateTo" PropertyName="Text" Type="DateTime" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="VoucherDR" />
                <asp:Parameter Name="VoucherCR" />
                <asp:Parameter Name="VoucherNo" />
                <asp:Parameter Name="AccountsHeadID" />
            </UpdateParameters>
        </asp:SqlDataSource>
        </ContentTemplate>  </asp:UpdatePanel>
    </div>

    </form>
</body>
</html>
