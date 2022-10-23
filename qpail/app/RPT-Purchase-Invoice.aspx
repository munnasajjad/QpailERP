<%@ Page Title="Purchase History By Invoice" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="RPT-Purchase-Invoice.aspx.cs" Inherits="app_RPT_Purchase_Invoice" %>

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

    <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>--%>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>


            <h3 class="page-title">Purchase By Date Range</h3>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">

                <div class="col-lg-12">
                    <section class="panel">
                        <%--Body Contants--%>
                        <div id="Div2">
                            <div>

                                <fieldset>
                                    <%--<legend>Search Terms</legend>--%>
                                    <table border="0" width="100%" style="width:100%" class="table1">
                                       <tr>
                                            <th>Supplier</th>
                                            <th> </th>
                                            <th>Invoice Date From</th>
                                            <th> </th>
                                           <th>Invoice Date To</th>
                                            <td> </td> 
                                           <th> </th>
                                       </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddCustomer" runat="server" CssClass="form-control" DataSourceID="SqlDataSource3" 
                                                    DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="true">
                                                    <asp:ListItem>---ALL---</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'vendor') ORDER BY [Company]"></asp:SqlDataSource>

                                            </td>
                                       
                                            <td> &nbsp; </td>
                                            <td>
                                                <asp:TextBox ID="txtInvDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtInvDate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                            </td>
                                      
                                            <td> &nbsp; </td>
                                            <td>
                                                <asp:TextBox ID="txtPODate" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtPODate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                            </td>
                                        
                                            <td></td>
                                            <td style="text-align: center;vertical-align: middle;">
                                                <asp:Button ID="btnSearch" CssClass="btn btn-s-md btn-primary" runat="server" Text="SHOW" OnClick="btnSearch_OnClick" />
                                                <asp:Button ID="btnReset" CssClass="btn btn-default" runat="server" Text="Reset" OnClick="btnReset_OnClick" />
                                                <asp:Button ID="btnExport" runat="server" Width="70px" Text="Export" OnClick="btnExport_OnClick" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>


                            </div>
                        </div>

                    </section>
                </div>




                <div class="col-lg-12">
                    <section class="panel">

                        <div id="Div1">
                            <div>

                                <fieldset>
                                    <legend>Query Result </legend>
                                    
                                    <div class="table-responsive">
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="150%" CssClass="table-hover zebra"  
                                            BorderStyle="Solid" BorderWidth="1px" CellPadding="7" ForeColor="Black" GridLines="Vertical"
                                            DataKeyNames="PID" AllowPaging="True" PageSize="100" OnPageIndexChanging="GridView1_OnPageIndexChanging" 
                                            OnRowDataBound="GridView1_OnRowDataBound"  RowStyle-CssClass="odd gradeX" ShowFooter="True">


                                            <Columns>

                                                <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Inv.No" SortExpression="Name">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="Label1" runat="server" Text='<%# Bind("InvNo") %>' NavigateUrl='<%# Eval("InvNo", "Purchase.aspx?type=edit&&inv={0}") %>' Target="_blank"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="PurchaseFor" HeaderText="PurchaseFor" SortExpression="address" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="OrderDate" HeaderText="OrderDate" SortExpression="phone"  DataFormatString="{0:d}"/>
                                                <asp:BoundField DataField="SupplySource" HeaderText="SupplySource" SortExpression="email" />
                                                <asp:BoundField DataField="BillNo" HeaderText="BillNo" SortExpression="email" />

                                                <asp:BoundField DataField="BillDate" HeaderText="BillDate" SortExpression="Name"  DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="SupplierName" HeaderText="SupplierName" SortExpression="address" />
                                                <asp:BoundField DataField="ChallanNo" HeaderText="ChallanNo" SortExpression="phone" />
                                                <asp:BoundField DataField="ItemTotal" HeaderText="ItemTotal" SortExpression="ItemTotal" DataFormatString="{0:#,##,###.00}" />

                                                <asp:BoundField DataField="PurchaseDiscount" HeaderText="Discount" SortExpression="phone" />
                                                <asp:BoundField DataField="VatService" HeaderText="Vat/ Service" SortExpression="ItemTotal" />
                                                <asp:BoundField DataField="otherExp" HeaderText="Other Exp" SortExpression="ItemTotal" />
                                                <asp:BoundField DataField="PurchaseTotal" HeaderText="PurchaseTotal" SortExpression="phone" DataFormatString="{0:#,##,###.00}" />
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
                                            <FooterStyle BackColor="#f3f3f3" Font-Bold="True" ForeColor="#23A6F0" BorderStyle="Solid" />
                                            <PagerStyle CssClass="gvpaging"></PagerStyle>
                                        </asp:GridView>
                                        Total Rows Found: <asp:Literal ID="ltrtotal" runat="server"></asp:Literal>
                                    </div>
                                </fieldset>



                            </div>
                        </div>
                        <%--End Body Contants--%>
                    </section>
                </div>





            </div>

    <a href="javascript:window.print()"><img src="../images/print.png" alt="print report" id="print-button" width="40px"></a>


        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

