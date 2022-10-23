<%@ Page Title="Control Accounts Balance" Language="C#" UICulture="bn-BD" Culture="bn-BD" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Control-Balance.aspx.cs" Inherits="app_Control_Balance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .table1 td {
            vertical-align: middle;
        }

    </style>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>


            <h3 class="page-title">Control Accounts Balance</h3>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">

                <div class="col-lg-12">
                    <section class="panel">


                                <fieldset>
                                    <table border="0"  class="table1" width="100%">
                                        <tr>
                                           <td style="vertical-align: middle; text-align: right;"><b> Control Accounts Name : </b></td>

                                            <TD><asp:DropDownList ID="ddParties" runat="server" DataSourceID="SqlDataSource2"
                            DataTextField="ControlAccountsName" DataValueField="ControlAccountsID"
                            Width="400px" AppendDataBoundItems="True" CssClass="select2me">
                            <asp:ListItem>---Select---</asp:ListItem>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                            SelectCommand="SELECT [ControlAccountsName],[ControlAccountsID] FROM [ControlAccount] ORDER BY [ControlAccountsName]"></asp:SqlDataSource>
</TD>

                                           <%--<td style="vertical-align: middle; text-align: right;"><b> Date From : </b></td>
                                            <td>
                                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>--%>

                                            <td>&nbsp; </td>
                                           <td style="vertical-align: middle; text-align: right;"><b>  As on Date : </b></td>

                                            <td>
                                                <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="txtdateTo" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>

                                            <td style="text-align: center; vertical-align: middle;">
                                                <asp:Button ID="btnSearch" CssClass="btn btn-s-md btn-primary" runat="server" Text="SHOW" OnClick="btnSearch_OnClick" />
                                                <%--<input id ="printbtn" type="button" class="btn btn-s-md btn-primary" value="PRINT" onclick="window.print();" >--%>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                        <iframe id="if1" runat="server" height="800px" width="100%" ></iframe>

                        <div class="table-responsive">
                        <asp:GridView ID="GridView1" runat="server" CssClass="table zebra" AutoGenerateColumns="False" OnRowDataBound="GVrpt_RowDataBound" ShowFooter="True" Visible="False">
                            <Columns>
                                <asp:BoundField DataField="Head" HeaderText="Head" SortExpression="Head" />
                                <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance" DataFormatString="{0:N}" />
                            </Columns>
                            <FooterStyle BorderStyle="Solid" Font-Bold="True"></FooterStyle>
                            </asp:GridView>
</div>

                    </section>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>

