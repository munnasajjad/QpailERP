<%@ Page Title="Matured Bill Statement" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="MaturedBillStatement.aspx.cs" Inherits="app_MaturedBillStatement" %>

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
        <asp:Literal ID="ltrFrmName" runat="server" Text="Matured Bill Statement" />
    </h3>

    <div class="row">
        <div class="col-md-12">
            <div class="portlet box red">


                <div id="qPanel">
                    <div id="datePanel">
                        <asp:Label ID="Label2" runat="server" Text="Customer:"></asp:Label>
                        <asp:DropDownList ID="ddParties" runat="server" DataSourceID="SqlDataSource2"
                            DataTextField="Company" DataValueField="PartyID" Width="400px" AppendDataBoundItems="True" CssClass="select2me">
                            <asp:ListItem>---Select---</asp:ListItem>
                             <asp:ListItem Value="O404O">Pending matured bill list by company</asp:ListItem>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                            SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'customer') ORDER BY [Company]"></asp:SqlDataSource>

                        &nbsp; &nbsp; 
                        &nbsp; &nbsp;
                        &nbsp;&nbsp;
    
                        <asp:Button ID="btnShow" runat="server" Text="Show" class="btn btn-s-md btn-primary" OnClick="btnShow_OnClick" />

                        <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                    </div>
                </div>
            </div>
        </div>
    </div>


    <iframe id="if1" runat="server" height="800px" width="100%"></iframe>


</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

