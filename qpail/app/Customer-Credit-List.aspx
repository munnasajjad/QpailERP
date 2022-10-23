<%@ Page Title="Customer Credit List" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Customer-Credit-List.aspx.cs" Inherits="app_Customer_Credit_List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <!-- BEGIN PAGE LEVEL STYLES -->
    <link rel="stylesheet" type="text/css" href="assets/plugins/bootstrap-datepicker/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="assets/plugins/select2/select2.css" />
    <link rel="stylesheet" type="text/css" href="assets/plugins/select2/select2-metronic.css" />
    <link rel="stylesheet" href="assets/plugins/data-tables/DT_bootstrap.css" />

    <!-- END PAGE LEVEL STYLES -->
    <style type="text/css">
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
            }
            .table1 .form-control, .table1 select {
                width: 100%;
            }

        table#ctl00_BodyContent_GridView1 {
            /*min-width: 1200px;*/
        }

            table#ctl00_BodyContent_GridView1 tr {
                height: 20px;
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


            <h3 class="page-title">Customer Credit List</h3>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">

                <div class="col-lg-12">
                    <section class="panel">

                        <div class="table-responsive">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-hover"
                                BorderStyle="Solid" BorderWidth="1px" CellPadding="7" ForeColor="Black" GridLines="Vertical"
                                AllowPaging="True" PageSize="100" OnPageIndexChanging="GridView1_OnPageIndexChanging"
                                OnRowDataBound="GridView1_OnRowDataBound" RowStyle-CssClass="odd gradeX" ShowFooter="True">


                                <Columns>

                                    <asp:TemplateField ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Company" SortExpression="Name" ItemStyle-Width="30%" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="Label1" runat="server" Text='<%# Bind("Company") %>' NavigateUrl='<%# Bind("link") %>' Target="_blank"></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="imQty" HeaderText="Imatured Invoice Quantity" SortExpression="address" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                    <asp:BoundField DataField="mQty" HeaderText="Matured Invoice Quantity" SortExpression="address" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />

                                    <asp:BoundField DataField="mDays" HeaderText="Invoice Maturity Days" SortExpression="address" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                    <asp:BoundField DataField="avgOverDays" HeaderText="Avg. Overdue Days" SortExpression="phone" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                    <asp:BoundField DataField="cLimit" HeaderText="Credit Limit" SortExpression="address" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" ItemStyle-VerticalAlign="Middle" />
                                    <asp:BoundField DataField="immatured" HeaderText="Immatured Balance" SortExpression="email" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" ItemStyle-VerticalAlign="Middle" />

                                    <asp:BoundField DataField="matured" HeaderText="Matured Balance" SortExpression="Name" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" ItemStyle-VerticalAlign="Middle" />
                                    <asp:BoundField DataField="Balance" HeaderText="Net Balance" SortExpression="phone" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-VerticalAlign="Middle" />
                                </Columns>
                                <FooterStyle BackColor="#f3f3f3" Font-Bold="True" ForeColor="#23A6F0" BorderStyle="Solid" HorizontalAlign="Right" />
                                <PagerStyle CssClass="gvpaging"></PagerStyle>
                            </asp:GridView>
                            Total Rows Found:
                            <asp:Literal ID="ltrtotal" runat="server"></asp:Literal>
                        </div>

                    </section>
                </div>

            </div>




        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

