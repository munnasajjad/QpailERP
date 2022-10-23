<%@ Page Title="Monthly Purchase" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Item-Purchase-History.aspx.cs" Inherits="app_Item_Purchase_History" %>

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
            /*text-align: right;*/
            float: right;
            /*width: 100px;*/
        }

        input#ctl00_BodyContent_CheckBox1 {
            float: right;
        }

        .form-group label, .control-group label, .control-group span {
            /*width: 100px !important;*/
            padding: 9px 0;
        }

        input#ctl00_BodyContent_txtDateFrom, input#ctl00_BodyContent_txtDateTo {
            width: 90px;
            padding: 6px 6px;
        }

        span.chkbox {
            float: right;
        }

        .table7 {
            border: 3px solid #ccc;
            width: 94%;
            margin: 10px;
            font-weight: 700;
            color: #666;
            text-align: left;
            line-height: 18px;
        }

            .table7 td {
                padding: 5px;
            }

        input#ctl00_BodyContent_btnShow {
            float: right;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>



            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">
                <div class="col-md-4">
                    <!-- BEGIN EXAMPLE TABLE PORTLET -->
                    <div class="portlet box light-grey">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-globe"></i>Item Purchase History
                            </div>

                        </div>

                        <asp:Label ID="lblProject" runat="server" Text="1" Visible="false"></asp:Label>

                        <%--<div class="form-group">
                                    <label>Company : </label>
                                        <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource2" Width="69%" CssClass="select2me"
                                            DataTextField="PartyName" DataValueField="CustomerID" AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT DISTINCT a.CustomerID, (Select Company from Party where PartyID=a.CustomerID) AS PartyName FROM Sales a ORDER BY [PartyName]">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                </div>--%>


                        <div class="form-group">
                            <label>Group : </label>
                            <asp:DropDownList ID="ddGroup" runat="server" DataSourceID="SqlDataSource3" AutoPostBack="true" Width="69%" CssClass="select2me"
                                DataTextField="GroupName" DataValueField="GroupSrNo" OnSelectedIndexChanged="ddGroup_SelectedIndexChanged" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">---all---</asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] WHERE GroupSrNo<>2 AND GroupSrNo<>3 AND GroupSrNo<>'11' ORDER BY [GroupSrNo]"></asp:SqlDataSource>

                        </div>

                        <div class="form-group">
                            <label class="control-label">Sub-group :  </label>
                            <asp:DropDownList ID="ddSubGrp" runat="server" AutoPostBack="true" Width="69%" CssClass="select2me" OnSelectedIndexChanged="ddSubGrp_SelectedIndexChanged" AppendDataBoundItems="True">
                            </asp:DropDownList>

                        </div>

                        <div class="form-group">
                            <label class="control-label">Grade :  </label>
                            <asp:DropDownList ID="ddGrade" runat="server" AutoPostBack="true" Width="69%" CssClass="select2me" OnSelectedIndexChanged="ddGrade_SelectedIndexChanged" AppendDataBoundItems="True">
                            </asp:DropDownList>

                        </div>

                        <div class="form-group">
                            <label class="control-label">Category :  </label>
                            <asp:DropDownList ID="ddCategory" runat="server" AutoPostBack="true" Width="69%" CssClass="select2me" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged" AppendDataBoundItems="True">
                            </asp:DropDownList>

                        </div>

                        <div class="form-group">
                            <label class="control-label">Item :  </label>
                            <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="true" Width="69%" CssClass="select2me" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged" AppendDataBoundItems="True">
                            </asp:DropDownList>

                            <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>
                            <%--<span style="width: 70%; color: green; float: right">
                                            <asp:Literal ID="ltrLastInfo" runat="server" EnableViewState="False">Recent Purchase Info: </asp:Literal>
                                        </span>--%>
                        </div>

                        <div class="form-group">
                            <label>Date Range: </label>
                            <div class="input-group input-large date-picker input-daterange" data-date="10/11/2012" data-date-format="dd/mm/yyyy">
                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" name="from" />
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy" />
                                <span class="input-group-addon">to</span>
                                <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" name="to" />
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtDateTo" Format="dd/MM/yyyy" />

                            </div>
                        </div>
                        <div class="form-group">

                            <%--<asp:CheckBox ID="ChkAll" runat="server" Text="All Items" />--%>
                            <asp:Button ID="btnExport" runat="server" Width="120px" Text="Export" OnClick="btnExport_OnClick" />
                            <asp:Button ID="btnShow" runat="server" Text="Show History" CssClass="btn blue" OnClick="btnShow_Click" />
                            <%--<asp:Button ID="btnPrint" runat="server" Text="Print Ledger" onclick="btnPrint_Click" />--%>
                        </div>

                        <div class="hidden">

                            <table class="table7">
                                <tr>
                                    <td>Grand Total Quantity (Pcs.): </td>
                                    <td>
                                        <asp:Literal ID="ltrQty" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>Grand Item Total (Tk.): </td>
                                    <td>
                                        <asp:Literal ID="ltrItemLoad" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>Grand Total VAT (Tk.): </td>
                                    <td>
                                        <asp:Literal ID="ltrTotalVat" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>Grand Total Amount (Tk.): </td>
                                    <td>
                                        <asp:Literal ID="ltrGTAmt" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>Grand Total Weight (kg.): </td>
                                    <td>
                                        <asp:Literal ID="ltrTotalWeight" runat="server" /></td>
                                </tr>
                            </table>
                        </div>
                    </div>

                </div>

                <iframe id="if1" runat="server" height="800px" width="100%" ></iframe>

                <div class="form-body col-md-8">
                    <div class="table-responsive">

                        <asp:GridView ID="GVrpt" class="zebra" aria-describedby="sample_1_info" Visible="False"
                            runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            ShowFooter="True" AllowSorting="True" Width="200%" OnPageIndexChanging="GVrpt_OnPageIndexChanging" RowStyle-CssClass="odd gradeX" OnRowDataBound="GVrpt_RowDataBound">
                            <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" />
                                        </asp:TemplateField>
                                <asp:BoundField DataField="BillNo" HeaderText="LC/Inv No." SortExpression="BillNo" />
                                <asp:BoundField DataField="BillDate" HeaderText="LC/Inv Date" SortExpression="BillDate" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="SupplierName" HeaderText="Supplier Name" SortExpression="SupplierName" />
                                <asp:BoundField DataField="ChallanNo" HeaderText="D.Challan#" SortExpression="ChallanNo" Visible="False" />
                                <asp:BoundField DataField="ChallanDate" HeaderText="D.Ch.Date" SortExpression="ChallanDate" DataFormatString="{0:d}"  Visible="False"/>
                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" SortExpression="ItemName" />
                                <asp:BoundField DataField="Specification" HeaderText="Spec." SortExpression="Specification" />
                                <asp:BoundField DataField="Qty" HeaderText="Qty." SortExpression="Qty" />
                                <asp:BoundField DataField="UnitType" HeaderText="Unit" SortExpression="UnitType" Visible="False" />
                                <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" />
                                
                                <asp:BoundField DataField="SubTotal" HeaderText="SubTotal" SortExpression="SubTotal" Visible="False"/>
                                <asp:BoundField DataField="ItemDisc" HeaderText="ItemDisc (TK.)" SortExpression="ItemDisc" />
                                <asp:BoundField DataField="ItemVAT" HeaderText="ItemVAT (TK.)" SortExpression="ItemVAT" />
                                <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                                
                                <asp:BoundField DataField="PurchaseDiscount" HeaderText="PurchaseDiscount (TK.)" SortExpression="PurchaseDiscount" />
                                <asp:BoundField DataField="OtherExp" HeaderText="OtherExp (TK.)" SortExpression="OtherExp" />
                                <asp:BoundField DataField="PurchaseTotal" HeaderText="PurchaseTotal" SortExpression="PurchaseTotal" />

                                <%--<asp:BoundField DataField="Manufacturer" HeaderText="Manufacturer" SortExpression="Manufacturer" />
                                <asp:BoundField DataField="CountryOfOrigin" HeaderText="Origin" SortExpression="Origin" />
                                <asp:BoundField DataField="PackSize" HeaderText="Pack Size" SortExpression="PackSize" />
                                <asp:BoundField DataField="Warrenty" HeaderText="Warrenty" SortExpression="Warrenty" />
                                <asp:BoundField DataField="SerialNo" HeaderText="Serial#" SortExpression="SerialNo" />
                                <asp:BoundField DataField="ModelNo" HeaderText="Model#" SortExpression="ModelNo" />--%>
                                <asp:BoundField DataField="SizeRef" HeaderText="SizeRef" SortExpression="SizeRef" Visible="False"/>
                                <asp:BoundField DataField="PriceWithoutVAT" HeaderText="Cost W/O VAT" SortExpression="PriceWithoutVAT" Visible="False" />
                                <asp:BoundField DataField="PriceWithVAT" HeaderText="Cost+VAT" SortExpression="PriceWithVAT" Visible="False" />
                            </Columns>
                            <FooterStyle BackColor="#f3f3f3" Font-Bold="True" ForeColor="#23A6F0" BorderStyle="Solid" />
                            <RowStyle CssClass="odd gradeX" />
                        </asp:GridView>

                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand="SELECT Purchase.BillNo, Purchase.BillDate, Party.Company, Purchase.SupplierName, Purchase.ChallanNo, Purchase.ChallanDate, PurchaseDetails.ItemName,
                         PurchaseDetails.Manufacturer, PurchaseDetails.CountryOfOrigin, PurchaseDetails.PackSize, PurchaseDetails.Warrenty, PurchaseDetails.SerialNo,
                         PurchaseDetails.ModelNo, PurchaseDetails.Specification, PurchaseDetails.UnitType, PurchaseDetails.SizeRef, PurchaseDetails.Qty, PurchaseDetails.Price, PurchaseDetails.SubTotal, PurchaseDetails.ItemDisc, PurchaseDetails.ItemVAT,
                         PurchaseDetails.Total, PurchaseDetails.PriceWithVAT, PurchaseDetails.PriceWithoutVAT
FROM            Party INNER JOIN
                         Purchase ON Party.PartyID = Purchase.SupplierID CROSS JOIN
                         PurchaseDetails
where PurchaseDetails.ItemCode=@ItemCode">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddItemName" Name="ItemCode" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <!-- END EXAMPLE TABLE PORTLET-->
                    </div>
                </div>

            </div>



            </div>

    <%--<a href="javascript:window.print()"><img src="../images/print.png" alt="print report" id="print-button" width="40px"></a>--%>


        </ContentTemplate>
    </asp:UpdatePanel>






    <%--<script type="text/javascript" src="assets/plugins/data-tables/jquery.dataTables.js"></script>
    <script type="text/javascript" src="assets/plugins/data-tables/DT_bootstrap.js"></script>
    <script src="assets/scripts/custom/table-managed.js"></script>
    <script>
        jQuery(document).ready(function () {
            TableManaged.init();
        });
    </script>--%>
</asp:Content>

