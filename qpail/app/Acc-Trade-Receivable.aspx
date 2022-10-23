<%@ Page Title="Trade Receivables" Language="C#" UICulture="bn-BD" Culture="bn-BD" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Acc-Trade-Receivable.aspx.cs" Inherits="app_Acc_Trade_Receivable" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
    </style>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    

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


            <h3 class="page-title">Trade Receivables</h3>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">

                <div class="col-lg-12">
                    <section class="panel">
                                                
                                               

                                <fieldset>
                                    <table border="0"  class="table1">
                                        <tr>
                                           <td style="vertical-align: middle; text-align: right;"><b> As on : </b>
                                            </td>
                                            <td>&nbsp; </td>
                                            <td>
                                                 <asp:TextBox ID="txtdateTo1" runat="server"  CssClass="form-control"></asp:TextBox>
                                                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                                                <asp:CalendarExtender ID="CalendarExtender54" runat="server" TargetControlID="txtdateTo1" Format="dd/MM/yyyy">
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

                        <div class="table-responsive col-lg-6">
                        <asp:GridView ID="GridView1" runat="server" CssClass="table zebra" ShowFooter="True"
                            AutoGenerateColumns="False" OnRowDataBound="GridView1_OnRowDataBound">

                                            <Columns>
                            <asp:BoundField DataField="Customer" HeaderText="Customer Name"  SortExpression="Customer" />
                            <asp:BoundField DataField="Balance" HeaderText="Balance" ReadOnly="True" SortExpression="Balance" DataFormatString="{0:N2}"  ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/>

                                            </Columns>
                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                        </asp:GridView>
</div>

                    </section>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>

