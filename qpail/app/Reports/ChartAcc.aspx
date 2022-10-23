<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChartAcc.aspx.cs" Inherits="Application_Reports_ChartAcc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Chart of Accounts</title>
</head>
<body>
    <form id="form1" runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>


<div id="content">
   <%-- <div id="rlogo">--%>
        <div id="ComInfo">
            <asp:Image ID="Image1" runat="server" />
            <p>
                <b>
                    <asp:Label ID="lblName" runat="server" ></asp:Label></b><br />
                <asp:Label ID="lblArress" runat="server" ></asp:Label>
            </p>
    <h3>Chart of Accounts</h3>
    
    <%--<div id="qPanel">
    <asp:Label ID="lblDate" runat="server" Text="Date From: "></asp:Label>            
           
                <asp:TextBox ID="txtDateF" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
                    Enabled="True" TargetControlID="txtDateF"  Format="dd/MM/yyyy">
                </asp:CalendarExtender>
    &nbsp; &nbsp;
    <asp:Label ID="Label1" runat="server" Text="Date To: "></asp:Label>            
           
                <asp:TextBox ID="txtDateT" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
                    Enabled="True" TargetControlID="txtDateT"  Format="dd/MM/yyyy">
                </asp:CalendarExtender>
    &nbsp; &nbsp;&nbsp;
    
    <asp:Button ID="btnLoad" runat="server" Text="Show History" OnClick="btnLoad_Click" />
    
    </div>--%>
</div>
<h4></h4>
    
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" 
            
        
        
        
        SelectCommand="SELECT HeadSetup.AccountsHeadID, AccountGroup.GroupName, Accounts.AccountsName, ControlAccount.ControlAccountsName, HeadSetup.AccountsHeadName, HeadSetup.AccountsOpeningBalance, HeadSetup.OpBalDr, HeadSetup.OpBalCr FROM AccountGroup INNER JOIN HeadSetup INNER JOIN ControlAccount ON HeadSetup.ControlAccountsID = ControlAccount.ControlAccountsID INNER JOIN Accounts ON HeadSetup.AccountsID = Accounts.AccountsID ON AccountGroup.GroupID = HeadSetup.GroupID order by AccountGroup.GroupID, HeadSetup.AccountsHeadID" 
        DeleteCommand="Delete From HeadSetup  where AccountsHeadName=@AccountsHeadName">
            <DeleteParameters>
                <asp:Parameter Name="AccountsHeadName"  />
            </DeleteParameters>
        </asp:SqlDataSource>
    
        <asp:GridView ID="GridView2" runat="server"
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
        BorderWidth="1px" Width="100%"
        CellPadding="3" AllowSorting="True" AutoGenerateColumns="False" 
        DataKeyNames="AccountsHeadID" DataSourceID="SqlDataSource1" >
            <RowStyle ForeColor="#000066" />
            <Columns>
                <asp:BoundField DataField="AccountsHeadID" HeaderText="A/C Head ID" 
                    ReadOnly="True" SortExpression="AccountsHeadID" />
                <asp:BoundField DataField="GroupName" HeaderText="A/C Group" 
                    SortExpression="GroupName" />
                <asp:BoundField DataField="AccountsName" HeaderText="Sub A/C" 
                    SortExpression="AccountsName" />
                <asp:BoundField DataField="ControlAccountsName" 
                    HeaderText="Control A/C" SortExpression="ControlAccountsName" />
                <asp:BoundField DataField="AccountsHeadName" HeaderText="Accounts Head" 
                    SortExpression="AccountsHeadName" />
                <asp:BoundField DataField="OpBalDr" 
                    HeaderText="Op.Bal (Dr.)" SortExpression="AccountsOpeningBalance" />
                <asp:BoundField DataField="OpBalCr" 
                    HeaderText="Op.Bal (Cr.)" SortExpression="AccountsOpeningBalance" />
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <EmptyDataTemplate>
                No Income Data Found !
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        
    
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
        
        
        <p style="text-align:center; font-weight:bold;">Total Head Count : 
            <asp:Label ID="lblBalance" runat="server" Text=""></asp:Label>
        </p>
        
        
    </div>


    </form>
</body>
</html>
