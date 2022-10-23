<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PL.aspx.cs" Inherits="Application_Reports_PL" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PL Accounts</title>
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
            <h3>Profit & Loss Accounts</h3>
        </div>
    
    <%--<h2 style="float:right; color:#fff; display:inline-block; position:fixed; top:60px; max-width: 930px;">Profit & Loss Account</h2>--%>
    
    <div id="qPanel">
        <div id="datePanel">
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
   &nbsp;&nbsp;
    
    <asp:Button ID="btnLoad" runat="server" Text="Show" OnClick="btnLoad_Click" />
    
     <asp:CompareValidator id="dateValidator" runat="server" Font-Size="9px"
                  Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateF"
                  ErrorMessage="Invalid 'FROM' Date!">
                </asp:CompareValidator>
    <asp:CompareValidator id="CompareValidator1" runat="server" Font-Size="9px"
                  Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateT"
                  ErrorMessage="Invalid 'TO' Date!">
                </asp:CompareValidator>
   <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
        
        </div>
    </div>
<%--</div>--%>
<br />
<h4>Revenue</h4>
    
    
        <asp:GridView ID="GridView2" runat="server"
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
        BorderWidth="1px" Width="100%"
        CellPadding="3" >
            <RowStyle ForeColor="#000066" />
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <EmptyDataTemplate>
                No Income Data Found !
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        
    
    <h4>Expenses</h4>
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
        </asp:GridView>
        
        
        <p style="text-align:center; font-weight:bold;">Profit/Loss: (+/-) : 
            <asp:Label ID="lblBalance" runat="server" Text=""></asp:Label>
        </p>
        
        
    </div>


    </form>
</body>
</html>
