<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Trial-Balance.aspx.cs" Inherits="Application_Reports_Trial_Balance" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Trial Balance</title>
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
            <h3>Trial Balance</h3>
        </div>
    
    <%--<h2 style="float:right; color:#fff; display:inline-block; position:fixed; top:60px; max-width: 930px;">Profit & Loss Account</h2>--%>
    
    <div id="qPanel">
        <div id="datePanel">
    <asp:Label ID="lblDate" runat="server" Text="As on"></asp:Label>            
           
                <asp:TextBox ID="txtDateF" runat="server" Visible="false" ></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                Enabled="True" TargetControlID="txtDateF"  Format="dd/MM/yyyy">
                </asp:CalendarExtender>
   
    <asp:Label ID="Label1" runat="server" Text="Date: "></asp:Label>            
           
                <asp:TextBox ID="txtDateT" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
                    Enabled="True" TargetControlID="txtDateT"  Format="dd/MM/yyyy">
                </asp:CalendarExtender>
    &nbsp; &nbsp;&nbsp;
    
    <asp:Button ID="btnLoad" runat="server" Text="Generate Trial Balance" OnClick="btnLoad_Click" />
    
   </div>
    </div>

    
   <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
<br />


<h4>Asset</h4>
    
    
        <asp:GridView ID="GridView2" runat="server"
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
        BorderWidth="1px" Width="100%"
        CellPadding="3" >
            <RowStyle ForeColor="#000066" />
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <EmptyDataTemplate>
                No Asset Data Found !
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
 <p style="text-align:center; font-weight:bold;">Total Asset Value : 
            <asp:Label ID="lblTotalAsset" runat="server" Text=""></asp:Label>
        </p>       
    
    <h4>Liabilities</h4>
        <asp:GridView ID="GridView1" runat="server"
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
        BorderWidth="1px" Width="100%"
        CellPadding="3" >
            <RowStyle ForeColor="#000066" />
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <EmptyDataTemplate>
                You Dont Have any Liabilities!
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <p style="text-align:center; font-weight:bold;">Total Liabilities : 
            <asp:Label ID="lblTotalLiab" runat="server" Text=""></asp:Label>
        </p>     
          
    <h4>Income</h4>
        <asp:GridView ID="grvIncome" runat="server"
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
        BorderWidth="1px" Width="100%"
        CellPadding="3" >
            <RowStyle ForeColor="#000066" />
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <EmptyDataTemplate>
                You Dont Have any Income!
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <p style="text-align:center; font-weight:bold;">Total Income : 
            <asp:Label ID="lblIncomeTotal" runat="server" Text=""></asp:Label>
        </p>  
    <h4>Expenses</h4>
        <asp:GridView ID="grvExpense" runat="server"
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
        </asp:GridView>
        <p style="text-align:center; font-weight:bold;">Total Expense : 
            <asp:Label ID="lblExpenseTotal" runat="server" Text=""></asp:Label>
        </p>  

    
    <h4>Equity</h4>
        <asp:GridView ID="GridView3" runat="server"
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
        BorderWidth="1px" Width="100%"
        CellPadding="3" >
            <RowStyle ForeColor="#000066" />
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <EmptyDataTemplate>
                No Capital Records Found!!
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        
        
        <p style="text-align:center; font-weight:bold;">Total Capital:
            <asp:Label ID="lblTotalCapital" runat="server" Text=""></asp:Label>
        </p>
        <hr />
        
        <p style="text-align:center; font-weight:bold;">Net Worth (Assets - Liabilities) : 
            <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
        </p>
        <p style="text-align:center; font-weight:bold;">Net Balance: (Assets - Liabilities - Equity) : 
            <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
        </p>
        
        
        
    </div>


    </form>
</body>
</html>
