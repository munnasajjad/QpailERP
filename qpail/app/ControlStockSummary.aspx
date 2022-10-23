<%@ Page Title="Control Stock Summary" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="ControlStockSummary.aspx.cs" Inherits="app_ControlStockSummary" %>


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
        <asp:Literal ID="ltrFrmName" runat="server" Text="Control Stock Summary" />
    </h3>
    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
    <div class="row">

        <div class="col-md-12">
            <div class="portlet box red">


                <div id="qPanel">
                    <div id="datePanel">
                        <asp:Label ID="Label2" runat="server" Text="Control Head Name :"></asp:Label>
                        <asp:DropDownList ID="ddParties" runat="server" DataSourceID="SqlDataSource2"
                            DataTextField="ControlAccountsName" DataValueField="ControlAccountsID" Width="400px" AppendDataBoundItems="True" CssClass="select2me"
                             AutoPostBack="True">
                            <asp:ListItem>---Select---</asp:ListItem>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                            SelectCommand="SELECT [ControlAccountsName], [ControlAccountsID] FROM [ControlAccount] ORDER BY [ControlAccountsName]"></asp:SqlDataSource>

                        &nbsp; &nbsp;        
                        <asp:Label ID="lblDate" runat="server" Text="Date From: "></asp:Label>

                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="control" Width="100px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>

                        &nbsp; &nbsp;
                <asp:Label ID="Label1" runat="server" Text="Date To: "></asp:Label>

                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="control" Width="100px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtDateTo" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        &nbsp;&nbsp;
    
    <asp:Button ID="btnShow" runat="server" Text="Show" class="btn btn-s-md btn-primary" OnClick="btnShow_OnClick" />
                        <%--<input id ="printbtn" type="button" class="btn btn-s-md btn-primary" value="PRINT" onclick="window.print();" >--%>
                        <asp:CompareValidator ID="dateValidator" runat="server" Font-Size="9px" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateFrom" ErrorMessage="Invalid 'FROM' Date!">
                        </asp:CompareValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" Font-Size="9px"
                            Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateTo" ErrorMessage="Invalid 'TO' Date!">
                        </asp:CompareValidator>
                        <asp:Label ID="Label3" runat="server" EnableViewState="false"></asp:Label>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <iframe id="if1" runat="server" height="800px" width="100%" ></iframe>
    </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>



