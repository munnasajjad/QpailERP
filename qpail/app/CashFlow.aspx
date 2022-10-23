<%@ Page Title="Cash Flow" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="CashFlow.aspx.cs" Inherits="app_CashFlow" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        table {
            border-collapse: collapse;
        }

        td {
            padding: 10px !important;
        }

        label {
            padding-top: 6px;
            text-align: right;
        }

        .table1 {
            width: 100%;
        }

            .table1 th {
                vertical-align: middle;
                font-weight: 700;
                text-align: center;
            }

            .table1 .form-control, .table1 select {
                width: 100%;
            }

        .table {
            width: 50% !important;
        }

        table#ctl00_BodyContent_GridView1 {
            /*min-width: 1200px;*/
        }

            table#ctl00_BodyContent_GridView1 tr {
                height: 20px;
            }

        .a-right, tr td:last-child {
            text-align: right;
        }
    </style>

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
            <h3 class="page-title">Statement of Cash Flow</h3>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">

                <div class="col-lg-12">
                    <section class="panel">
                        <%--Body Contants--%>
                        <div id="Div2">
                            
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server"></asp:UpdateProgress>

                            <div>

                                <fieldset hidden="true">
                                    <%--<legend>Search Terms</legend>--%>
                                    <table border="0" width="100%" style="width: 100%" class="table1">
                                        <%--<tr>

                                            <th>Financial Year</th>
                                            <th></th>
                                            <th>Date To</th>
                                            <th></th>
                                        </tr>--%>
                                        <tr>
                                            <td>
                                                <div class="control-group">
                                                    <asp:Label class="control-group" runat="server" Text="Financial Year :" Font-Bold="True"></asp:Label>
                                                    <asp:DropDownList ID="ddFinancialYear" runat="server" 
                                                        DataSourceID="SqlDataSource6" DataTextField="Financial_Year" DataValueField="Financial_Year_Number"></asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>"
                                                        SelectCommand="SELECT [Financial_Year_Number], [Financial_Year] FROM [tblFinancial_Year]"></asp:SqlDataSource>
                                                </div>

                                                <asp:TextBox ID="txtdateFrom" runat="server" Visible="false" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtdateFrom" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>&nbsp; </td>
                                            <td>
                                                <asp:TextBox ID="txtdateTo" runat="server" Visible="false" CssClass="form-control"></asp:TextBox>
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
                                <fieldset>
                                    <%--<legend>Search Terms</legend>--%>
                                    <table border="0" width="60%" style="width: 60%" class="table1">
                                        <%--<tr>
                                           
                                            <th>Date From</th>
                                            <th></th>
                                            <th>Date To</th>
                                            <th></th>
                                        </tr>--%>
                                        <tr>
                                            <td style="vertical-align: middle; text-align: right;"><b>Financial Year 1 : </b></td>
                                            <td>
                                                <asp:DropDownList ID="ddYear1" runat="server"  CssClass="form-control" 
                                                   DataSourceID="SqlDataSource6" DataTextField="Financial_Year" DataValueField="Financial_Year_Number"></asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" SelectCommand="SELECT [Financial_Year] FROM [tblFinancial_Year]"></asp:SqlDataSource>
                                            </td>
                                            <td style="vertical-align: middle; text-align: right;"><b>Financial Year 2 : </b></td>
                                            <td>
                                                
                                            <td>
                                                <asp:DropDownList ID="ddYear2" runat="server" CssClass="form-control" DataSourceID="SqlDataSource6" DataTextField="Financial_Year" DataValueField="Financial_Year_Number"></asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" SelectCommand="SELECT [Financial_Year] FROM [tblFinancial_Year]"></asp:SqlDataSource>
                                            </td>
                                            <td style="text-align: center; vertical-align: middle;">
                                                <asp:Button ID="btnShow2" CssClass="btn btn-s-md btn-primary" runat="server" Text="SHOW" OnClick="btnShow2_OnClick" />
                                                <%--<input id ="printbtn" type="button" class="btn btn-s-md btn-primary" value="PRINT" onclick="window.print();" >--%>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>

                            </div>
                        </div>

                    </section>
                </div>

                <iframe id="if1" runat="server" height="800px" width="100%"></iframe>


                <div class="col-lg-12">
                    <section class="panel">

                        <div id="Div1">
                            <div>

                                <fieldset>
                                    <legend class="hidden">
                                        <asp:Literal ID="ltrQR" runat="server"></asp:Literal>
                                    </legend>

                                    <div class="table-responsive">

                                        <asp:Literal ID="ltrBody" runat="server"></asp:Literal>
                                </fieldset>

                                <%--<div style="margin-top: 100px">&nbsp;
                                    <br/><br/><br/><br/><br/>
                                    <hr />
                                    <br/><br/><br/><br/><br/>
                                </div>--%>


                                <table width="70%">
                                    <tr>
                                        <td>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <hr />
                                        </td>
                                        <td>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <hr />
                                        </td>
                                        <td>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <%-- <td>Account Manager</td>
                                        <td>Managing Director</td>
                                        <td>Chairman</td>--%>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <hr />
                                        </td>
                                        <td>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <hr />
                                        </td>
                                        <td>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <hr />
                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </div>
                        <%--End Body Contants--%>
                    </section>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

