<%@ Page Title="Search Invoice" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Search-Invoice.aspx.cs" Inherits="app_Search_Invoice" %>

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
        .table1 td {
    vertical-align: middle;
}
.table1 .form-control, .table1 select {
    width: 100%;
}
table#ctl00_BodyContent_GridView1 {
    min-width: 1200px;
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


            <%--<h3 class="page-title">Search Invoice</h3>--%>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">

                <div class="col-lg-4">
                    <section class="panel">
                        <%--Body Contants--%>
                        <div id="Div2">
                            <div>

                                <fieldset>
                                    <legend>Search Terms</legend>
                                    <table border="0" width="100%" style="width:100%" class="table1">
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Label ID="lblError" runat="server" Text="" EnableViewState="false" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="40%" >Customer</td>
                                            <td>
                                                <asp:DropDownList ID="ddCustomer" runat="server" CssClass="form-control" DataSourceID="SqlDataSource3" 
                                                    DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="true">
                                                    <asp:ListItem>---ALL---</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'customer') ORDER BY [Company]"></asp:SqlDataSource>

                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Invoice No.</td>
                                            <td>
                                                <asp:TextBox ID="txtInvNo" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Invoice Date</td>
                                            <td>
                                                <asp:TextBox ID="txtInvDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtInvDate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>PO/ LC No.</td>
                                            <td>
                                                <asp:TextBox ID="txtPoNo" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>PO Date</td>
                                            <td>
                                                <asp:TextBox ID="txtPODate" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtPODate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td>Invoice Mode </td>
                                            <td>
                                                <asp:DropDownList ID="ddSalesMode" runat="server" DataSourceID="SqlDataSource2" 
                                                    DataTextField="OrderType" DataValueField="OrderType" AutoPostBack="true"  CssClass="form-control"  AppendDataBoundItems="true">
                                                    <asp:ListItem>---ALL---</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                        SelectCommand="SELECT Distinct OrderType FROM [Orders]  order by OrderType desc ">
                                    </asp:SqlDataSource>
                                                
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Delivery Godown </td>
                                            <td>
                                                 <asp:DropDownList ID="ddWarehouse" runat="server"  CssClass="form-control"  DataSourceID="SqlDataSource1" DataTextField="StoreName" DataValueField="WID"  AppendDataBoundItems="true">
                                                    <asp:ListItem>---ALL---</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [WID], [StoreName] FROM [Warehouses] ORDER BY [StoreName]">
                                    </asp:SqlDataSource>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Challan No.</td>
                                            <td>
                                                <asp:TextBox ID="txtChallanNo" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>VAT Challan No.</td>
                                            <td>
                                                <asp:TextBox ID="txtVatChallan" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr style="background: none">
                                            <td></td>
                                            <td style="text-align: center;">
                                                <asp:Button ID="btnSearch" CssClass="btn btn-s-md btn-primary" runat="server" Text="Search" OnClick="btnSearch_OnClick" />
                                                <asp:Button ID="btnReset" CssClass="btn btn-s-md btn-white" runat="server" Text="Reset" OnClick="btnReset_OnClick" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>


                            </div>
                        </div>

                    </section>
                </div>




                <div class="col-lg-8">
                    <section class="panel">

                        <div id="Div1">
                            <div>

                                <fieldset>
                                    <legend>Search Result </legend>


                                    Total Search Result:
                                <asp:Literal ID="ltrtotal" runat="server"></asp:Literal>

                                    <div class="table-responsive">
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="200%" CssClass="table-hover"  
                                            BorderStyle="Solid" BorderWidth="1px" CellPadding="7" ForeColor="Black" GridLines="Vertical"
                                            DataKeyNames="SaleID" AllowPaging="True" PageSize="25" OnPageIndexChanging="GridView1_OnPageIndexChanging" >


                                            <Columns>

                                                <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Inv.No" SortExpression="Name">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="Label1" runat="server" Text='<%# Bind("InvNo") %>' NavigateUrl='<%# Bind("link") %>' Target="_blank"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="InvDate" HeaderText="Inv.Date" SortExpression="address" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="phone" />
                                                <asp:BoundField DataField="PONo" HeaderText="PO#" SortExpression="email" />
                                                <asp:BoundField DataField="PODate" HeaderText="PO.Date" SortExpression="email"  DataFormatString="{0:d}"/>

                                                <asp:BoundField DataField="InvoiceTotal" HeaderText="Inv.Amount (Tk.)" SortExpression="Name" ReadOnly="True" />
                                                <asp:BoundField DataField="VATAmount" HeaderText="VAT (Tk.)" SortExpression="address" />
                                                <asp:BoundField DataField="PayableAmount" HeaderText="Net Amount (Tk.)" SortExpression="phone" />
                                                <asp:BoundField DataField="ChallanNo" HeaderText="Challan#" SortExpression="email" />

                                                <asp:BoundField DataField="VatChalNo" HeaderText="VAT Chal. No." SortExpression="phone" />
                                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="email" />

                                                <%--<asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" Visible="false" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">Item will be deleted permanently!</b><br />
                                                    Are you sure you want to delete the item from order list?
                                                            <br />
                                                    <br />
                                                    <div style="text-align: right;">
                                                        <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                        <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                    </div>
                                                </asp:Panel>

                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                            </Columns>
                                            
                                            <PagerStyle CssClass="gvpaging"></PagerStyle>
                                        </asp:GridView>

                                    </div>
                                </fieldset>



                            </div>
                        </div>
                        <%--End Body Contants--%>
                    </section>
                </div>





            </div>




        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

