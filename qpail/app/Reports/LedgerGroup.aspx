<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LedgerGroup.aspx.cs" Inherits="Application_Reports_LedgerGroup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Account Group Ledger</title>
    

    <script src="../../js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../../js/jquery-ui-1.10.0.custom.min.js" type="text/javascript"></script>

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
            <h3>A/C Group Ledger</h3>
        </div>
    
    <%--<h2 style="float:right; color:#fff; display:inline-block; position:fixed; top:60px; max-width: 930px;">Profit & Loss Account</h2>--%>
    
    <div id="qPanel">
        <div id="datePanel">
        <asp:Label ID="Label2" runat="server" Text="A/C Group :"></asp:Label> 
        <asp:DropDownList ID="ddParties" runat="server" DataSourceID="SqlDataSource2" 
                    DataTextField="GroupName" DataValueField="GroupID" 
            Width="200px" AppendDataBoundItems="True"   
            onselectedindexchanged="ddParties_SelectedIndexChanged" AutoPostBack="True">
            <asp:ListItem>---Select---</asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"                     
                    
                
                SelectCommand="SELECT [GroupID], [GroupName] FROM [AccountGroup] ORDER BY [GroupName]"></asp:SqlDataSource>
                
    &nbsp; &nbsp;        
    <asp:Label ID="lblDate" runat="server" Text="Date From: "></asp:Label>            
           
                <asp:TextBox ID="txtDateFrom" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
                    Enabled="True" TargetControlID="txtDateFrom"  Format="dd/MM/yyyy">
                </asp:CalendarExtender>
               
    &nbsp; &nbsp;
    <asp:Label ID="Label1" runat="server" Text="Date To: "></asp:Label>            
           
                <asp:TextBox ID="txtDateTo" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
                    Enabled="True" TargetControlID="txtDateTo"  Format="dd/MM/yyyy">
                </asp:CalendarExtender>
   &nbsp;&nbsp;
    
    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" />
    
     <asp:CompareValidator id="dateValidator" runat="server" Font-Size="9px"
                  Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateFrom"
                  ErrorMessage="Invalid 'FROM' Date!">
                </asp:CompareValidator>
    <asp:CompareValidator id="CompareValidator1" runat="server" Font-Size="9px"
                  Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateTo"
                  ErrorMessage="Invalid 'TO' Date!">
                </asp:CompareValidator>
   <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
        
        </div>
    </div>
<%--</div>--%>
<br />
<%--<h4>Revenue</h4>--%>
    
    
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
        
    
    <h4> </h4>
        <asp:GridView ID="GridView1" runat="server"  AllowSorting="True" 
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
        BorderWidth="1px" Width="100%" AutoGenerateColumns="false"
        CellPadding="3" >
            <RowStyle ForeColor="#000066" />
            <Columns>
                <asp:BoundField DataField="EntryDate" HeaderText="Entry Date"  DataFormatString="{0:d}" />
                <asp:BoundField DataField="AccountsHeadName" HeaderText="Accounts Head Name" />
                <asp:BoundField DataField="Description" HeaderText="Voucher Description" />
                <asp:BoundField DataField="VoucherDR" HeaderText="Dr." />
                <asp:BoundField DataField="VoucherCR" HeaderText="Cr." />
                <asp:BoundField DataField="Balance" HeaderText="Balance" />
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <EmptyDataTemplate>
                You Dont Have any Expenses!
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        
        
        <%--<p style="text-align:center; font-weight:bold;">Profit/Loss: (+/-) : 
            <asp:Label ID="lblBalance" runat="server" Text=""></asp:Label>
        </p>--%>
        
        
    </div>


    </form>
    
    
    <script src="../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">        $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); </script>

    
</body>
</html>
