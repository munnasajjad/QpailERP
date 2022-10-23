<%@ Page Title="Bank Loans Report" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="BankLoansReport.aspx.cs" Inherits="app_BankLoansReport" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
    </style>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>


    <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
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
            </script>--%>


    <h3 class="page-title">Bank Loans Report</h3>

    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


    <div class="row">

        <div class="col-lg-12">
            <section class="panel">



                <fieldset>
                    <table border="0" class="table1">
                        <tr>
                            <td style="vertical-align: middle; text-align: right;"><b>Loan Type : </b>
                            </td>
                            <td>&nbsp; </td>
                            <td>
                                <asp:DropDownList ID="ddLoanType" runat="server" CssClass="form-control" Width="100px" DataSourceID="SqlDataSource1" DataTextField="LoanTypes" DataValueField="Id" AutoPostBack="True" OnSelectedIndexChanged="ddLoanType_OnSelectedIndexChanged"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" 
                                    SelectCommand="SELECT Id, LoanTypes FROM BankLoanTypes ORDER BY LoanTypes"></asp:SqlDataSource>
                            </td>
                            <td>&nbsp; </td>
                            <td style="vertical-align: middle; text-align: right;"><b>Loan Code :</b>
                            </td>
                            <td>&nbsp; </td>
                            <td>
                                <asp:DropDownList ID="ddLoanCode" runat="server" CssClass="form-control" Width="100px" AppendDataBoundItems="True">
                                </asp:DropDownList>

                            </td>
                            <td>&nbsp; </td>
                            <td style="vertical-align: middle; text-align: right;"><b>From : </b>
                                <td>
                                    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender54" runat="server" TargetControlID="txtDateFrom" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            <%--<td>To</td>--%>
                            <td style="vertical-align: middle; text-align: right;"><b>To : </b>
                                <td>
                                    <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateTo" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            <td>&nbsp; </td>
                            <td style="text-align: center; vertical-align: middle;">
                                <asp:Button ID="btnSearch" CssClass="btn btn-s-md btn-primary" runat="server" Text=" SHOW" OnClick="btnSearch_OnClick" />
                                <%--<input id ="printbtn" type="button" class="btn btn-s-md btn-primary" value="PRINT" onclick="window.print();" >--%>
                            </td>
                        </tr>
                    </table>
                </fieldset>

                <iframe id="if1" runat="server" height="800px" width="100%"></iframe>

                <div class="table-responsive col-lg-6">
                    <asp:GridView ID="GridView1" runat="server" CssClass="table zebra" ShowFooter="True"
                        AutoGenerateColumns="False" OnRowDataBound="GridView1_OnRowDataBound" DataSourceID="SqlDataSource2" Visible="False">

                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" InsertVisible="False" ReadOnly="True" />
                            <asp:BoundField DataField="LoanType" HeaderText="LoanType" SortExpression="LoanType" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />

                            <asp:BoundField DataField="ACHeadId" HeaderText="ACHeadId" SortExpression="ACHeadId" />
                            <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" />
                            <asp:BoundField DataField="ReceivedDate" HeaderText="ReceivedDate" SortExpression="ReceivedDate" />
                            <asp:BoundField DataField="InterestRate" HeaderText="InterestRate" SortExpression="InterestRate" />
                            <asp:BoundField DataField="Duration" HeaderText="Duration" SortExpression="Duration" />
                            <asp:BoundField DataField="Rcvamount" HeaderText="Rcvamount" SortExpression="Rcvamount" />

                        </Columns>
                        <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" SelectCommand="SELECT [Id], [LoanType], [ACHeadId], [Code], [ReceivedDate], [InterestRate], [Duration], [Rcvamount] FROM [BankLoan]"></asp:SqlDataSource>
                </div>

            </section>
        </div>
    </div>
    <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
