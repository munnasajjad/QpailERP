<%@ Page Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Accounts-Chart-Report.aspx.cs" Inherits="Operator_Accounts_Chart_Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        table {
            border-collapse: collapse;
        }

        td {
            padding: 10px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">


    <div id="content">

        <div id="ComInfo" class="hidden">
            <asp:Image ID="Image1" runat="server" />
            <p>
                <b>
                    <asp:Label ID="lblName" runat="server"></asp:Label></b><br />
                <asp:Label ID="lblArress" runat="server"></asp:Label>
            </p>

        </div>
        <h3 class="page-title" >Chart of Accounts</h3>
        <h4></h4>

        <div class="table-responsive">

            <asp:Literal ID="ltrBody" runat="server"></asp:Literal>

            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                SelectCommand="SELECT HeadSetup.AccountsHeadID, AccountGroup.GroupName, Accounts.AccountsName, ControlAccount.ControlAccountsName, HeadSetup.AccountsHeadName, HeadSetup.AccountsOpeningBalance, HeadSetup.OpBalDr, HeadSetup.OpBalCr FROM AccountGroup INNER JOIN HeadSetup INNER JOIN ControlAccount ON HeadSetup.ControlAccountsID = ControlAccount.ControlAccountsID INNER JOIN Accounts ON HeadSetup.AccountsID = Accounts.AccountsID ON AccountGroup.GroupID = HeadSetup.GroupID order by AccountGroup.GroupID, HeadSetup.AccountsHeadID"
                DeleteCommand="Delete From HeadSetup  where AccountsHeadName=@AccountsHeadName">
                <DeleteParameters>
                    <asp:Parameter Name="AccountsHeadName" />
                </DeleteParameters>
            </asp:SqlDataSource>

            <asp:GridView ID="GridView1" runat="server" Width="100%" CssClass="table-bordered table-striped"
                AllowSorting="True" AutoGenerateColumns="False" Visible="False"
                DataKeyNames="AccountsHeadID" DataSourceID="SqlDataSource1">

                <Columns>
                    <asp:BoundField DataField="AccountsHeadID" HeaderText="A/C Head ID" ReadOnly="True" SortExpression="AccountsHeadID" />
                    <asp:BoundField DataField="GroupName" HeaderText="A/C Group" SortExpression="GroupName" />
                    <asp:BoundField DataField="AccountsName" HeaderText="Sub A/C" SortExpression="AccountsName" />
                    <asp:BoundField DataField="ControlAccountsName" HeaderText="Control A/C" SortExpression="ControlAccountsName" />
                    <asp:BoundField DataField="AccountsHeadName" HeaderText="Accounts Head" SortExpression="AccountsHeadName" />
                    <asp:BoundField DataField="OpBalDr" HeaderText="Op.Bal (Dr.)" SortExpression="AccountsOpeningBalance" />
                    <asp:BoundField DataField="OpBalCr" HeaderText="Op.Bal (Cr.)" SortExpression="AccountsOpeningBalance" />
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

