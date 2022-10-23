<%@ Page Title="Receipts And Payment Account" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Receipts-Payment.aspx.cs" Inherits="app_Receipts_Payment" %>
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
    <script type="text/javascript">
        $(document).ready(function () {
            $('.table-striped th:nth-child(1)').hide();
            $('.table-striped td:nth-child(1)').hide();
            $('.table-striped th:nth-child(2)').hide();
            $('.table-striped td:nth-child(2)').hide();
            $('.table-striped th:nth-child(3)').hide();
            $('.table-striped td:nth-child(3)').hide();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

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


            <h3 class="page-title">Receipts And Payment Account</h3>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>


            <div class="row">
                <div class="col-lg-12">
                    <section class="panel">
                        <%--Body Contants--%>
                        <div id="Div2">
                            <div>
                                <fieldset>
                                    <%--<legend>Search Terms</legend>--%>
                                    <table border="0"  style="width: 75%" class="table1">
                                        <tr>
                                            <th>Date From</th>
                                            <th>Date To</th>
                                            <th></th>
                                        </tr>
                                        <tr>
                                            <td style="display: none">
                                                <asp:RadioButton ID="showButton" runat="server" GroupName="g" Text="Show Report" Checked="True" AutoPostBack="True" OnCheckedChanged="RadioButton1_OnCheckedChanged" /><br/>
                                                <asp:RadioButton ID="runButton" runat="server" GroupName="g" Text="Run Process" AutoPostBack="True" OnCheckedChanged="RadioButton1_OnCheckedChanged"  />
                                            </td>
                                           <td>
                                                <asp:TextBox ID="txtOpening" runat="server" CssClass="form-control" Width="100%" Enabled="True"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtOpening" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            
                                            <td>
                                                <asp:TextBox ID="txtClosing" runat="server" CssClass="form-control" Width="100%" Enabled="True"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtClosing" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        <%--<tr>
                                            <td style="vertical-align: middle; text-align: right;"><b>Financial Year 1 : </b></td>
                                         <td>
                                             <asp:DropDownList ID="ddYear1" runat="server" DataSourceID="SqlDataSource3" DataTextField="Financial_Year" DataValueField="Financial_Year" CssClass="form-control"></asp:DropDownList>
                                             <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" SelectCommand="SELECT [Financial_Year] FROM [tblFinancial_Year]"></asp:SqlDataSource>
                                         </td>
                                            <td style="vertical-align: middle; text-align: right;"><b>Financial Year 2 : </b></td>
                                         <td>
                                             <asp:DropDownList ID="ddYear2" runat="server" DataSourceID="SqlDataSource2" DataTextField="Financial_Year" DataValueField="Financial_Year" CssClass="form-control"></asp:DropDownList>
                                             <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" SelectCommand="SELECT [Financial_Year] FROM [tblFinancial_Year]"></asp:SqlDataSource>
                                         </td>--%>
                                            <td style="text-align: center; vertical-align: middle;">
                                                <asp:Button ID="btnShow2" CssClass="btn btn-s-md btn-primary" runat="server" Text="Show Report" OnClick="btnSearch_OnClick" />
                                                
                                                <%--<input id ="printbtn" type="button" class="btn btn-s-md btn-primary" value="PRINT" onclick="window.print();" >--%>
                                            </td>
                                        </tr>
                                      
                                    </table>
                                </fieldset>
                            </div>
                        </div>

                    </section>
                </div>

                <div class="col-lg-12 hidden">
                    <section class="panel">
                        <%--Body Contants--%>
                        <div id="Div2">
                            <div>

                                <fieldset>
                                    <%--<legend>Search Terms</legend>--%>
                                    <table border="0" width="100%" style="width: 100%" class="table1">
                                        <tr>
                                           
                                            <th>Date From</th>
                                            <th></th>
                                            <th>Date To</th>
                                            <th></th>
                                        </tr>
                                        <tr>
                                           <td >
                                                <asp:TextBox ID="txtdateFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtdateFrom" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>&nbsp; </td>
                                            <td>
                                                <asp:TextBox ID="txtdateTo" runat="server" CssClass="form-control"></asp:TextBox>
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


                            </div>
                        </div>

                    </section>
                </div>

                <iframe id="if1" EnableViewState="False" runat="server" height="800px" width="100%"  style="" ></iframe>
                



    </ContentTemplate>
    </asp:UpdatePanel>
    


</asp:Content>

