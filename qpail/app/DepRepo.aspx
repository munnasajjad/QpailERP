<%@ Page Title="Depreciation Report" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="DepRepo.aspx.cs" Inherits="app_DepRepo" %>
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

        .table1  {
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


            <h3 class="page-title">Statement of property, plant & equipment</h3>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">

                <div class="col-lg-12">
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
                                            <th>Report Type</th>
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
                                            <td></td>
                                            <td>
                                                <asp:DropDownList ID="ddreporttype" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="0">Group Wise Report</asp:ListItem>
                                                    <asp:ListItem Value="1">Item Wise Report</asp:ListItem>

                                                    
                                                </asp:DropDownList>
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

                <iframe id="if1" runat="server" height="800px" width="100%" ></iframe>


                <div class="col-lg-12">
                    <section class="panel">

                        <div id="Div1">
                            <div>

                                <fieldset>
                                    <legend class="hidden"><asp:Literal ID="ltrQR" runat="server"></asp:Literal> </legend>

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
                                        <td><br/><br/><br/><br/><br/><hr /></td>
                                        <td><br/><br/><br/><br/><br/><hr /></td>
                                        <td><br/><br/><br/><br/><br/><hr /></td>
                                    </tr>
                                    <tr>
                                       <%-- <td>Account Manager</td>
                                        <td>Managing Director</td>
                                        <td>Chairman</td>--%>
                                    </tr>
                                     <tr>
                                        <td><br/><br/><br/><br/><br/><hr /></td>
                                        <td><br/><br/><br/><br/><br/><hr /></td>
                                        <td><br/><br/><br/><br/><br/><hr /></td>
                                    </tr>
                                </table>

                            </div>
                        </div>
                        <%--End Body Contants--%>
                    </section>
                </div>
                
            </div>



    </ContentTemplate>
    </asp:UpdatePanel>
    


</asp:Content>

