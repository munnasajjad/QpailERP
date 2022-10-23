<%@ Page Title="Cheque Rcpt/Payment" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Cheque-Receipt-Payment.aspx.cs" Inherits="app_Cheque_Receipt_Payment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
.control {
    color: #444;
    line-height: 26px;
    box-sizing: border-box;
    display: inline-block;
    margin: 0;
    position: relative;
    vertical-align: middle;
    background-color: #fff;
    border: 1px solid #aaa;
    border-radius: 4px;
    text-align: center;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <h3 class="page-title">
        <asp:Literal ID="ltrFrmName" runat="server" Text="Cheque Receipt Payment Statement" />
    </h3>

    <div class="row">
        <div class="col-md-12">
            <div class="portlet box red">


                <div id="qPanel">
                    <div id="datePanel">
                        <asp:Label ID="Label2" runat="server" Text="Report Type :"></asp:Label>
                        <asp:DropDownList ID="ddParties" runat="server" Width="400px" CssClass="select2me"
                            OnSelectedIndexChanged="ddParties_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="Collection">Receipt Statement</asp:ListItem>
                            <asp:ListItem Value="Payment">Payment Statement</asp:ListItem>
                        </asp:DropDownList>
                        
                        &nbsp; &nbsp;        
                        <asp:Label ID="lblDate" runat="server" Text="Date From: "></asp:Label>

                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="control" Width="100px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                            Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        &nbsp; &nbsp;
                <asp:Label ID="Label1" runat="server" Text="Date To: "></asp:Label>
                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="control" Width="100px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                            Enabled="True" TargetControlID="txtDateTo" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        &nbsp;&nbsp;
    
    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" class="btn btn-s-md btn-primary"/>
                        <%--<input id ="printbtn" type="button" class="btn btn-s-md btn-primary" value="PRINT" onclick="window.print();" >--%>
                        <asp:CompareValidator ID="dateValidator" runat="server" Font-Size="9px"
                            Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateFrom"
                            ErrorMessage="Invalid 'FROM' Date!">
                        </asp:CompareValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" Font-Size="9px"
                            Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateTo"
                            ErrorMessage="Invalid 'TO' Date!">
                        </asp:CompareValidator>
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                    </div>
                </div>
            </div>
        </div>
    </div>


    <iframe id="if1" runat="server" height="800px" width="100%" ></iframe>

    <asp:GridView ID="GridView2" runat="server" 
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
        BorderWidth="1px" Width="100%"
        CellPadding="3">
        <RowStyle ForeColor="#000066" />
        <FooterStyle BackColor="White" ForeColor="#000066" />
        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
        <EmptyDataTemplate>
            No Data Found !
        </EmptyDataTemplate>
        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
    </asp:GridView>


    <h4></h4>
    <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped zebra" ForeColor="#046FAF" Visible="False" 
       AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="GridView1_OnRowDataBound" OnPageIndexChanging="GridView1_OnPageIndexChanging">
        <Columns>
            <asp:TemplateField ItemStyle-Width="20px">
    <ItemTemplate>
        <%#Container.DataItemIndex+1 %>. 
    </ItemTemplate>
</asp:TemplateField>    
                <asp:BoundField DataField="TrDate" HeaderText="TrDate" SortExpression="VoucherNo" ItemStyle-Width="80px" />
                <asp:BoundField DataField="Description" HeaderText="Description"  SortExpression="VoucherDate" />
                <asp:BoundField DataField="Dr" HeaderText="Dr." SortExpression="AccountsHeadName"  DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="Cr" HeaderText="Cr." SortExpression="VoucherDR" DataFormatString="{0:N2}"  ItemStyle-HorizontalAlign="Right"   />
                <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="VoucherCR" DataFormatString="{0:N2}"  ItemStyle-HorizontalAlign="Right"   />
            </Columns>

        <RowStyle ForeColor="#000066" />
        <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="True" BorderStyle="Double" HorizontalAlign="Right" />
        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
        <EmptyDataTemplate>
            You Dont Have any Expenses!
        </EmptyDataTemplate>
        <%--<SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />--%>
    </asp:GridView>



</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

