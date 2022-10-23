<%--<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MasterPage.master" CodeFile="Sale-Item-byBrand-Report.aspx.cs" Inherits="app_Sale_Item_byBrand_Report" %>--%>

<%@ Page Title="Sale by Brand" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Sale-Item-byBrand-Report.aspx.cs" Inherits="app_Sale_Item_byBrand_Report" %>
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
            float: right;
            width: 100px;
        }
        .dataTables_wrapper label {
            width: 100%;
        }
        input#ctl00_BodyContent_CheckBox1 {
            float: right;
        }

        .form-group label, .control-group label, .control-group span {
            width: 100px !important;
            padding: 9px 0;
        }

        input#ctl00_BodyContent_txtDateFrom, input#ctl00_BodyContent_txtDateTo {
            width: 150px;
        }

        span.chkbox {
            float: right;
        }
        .table7 {
            border: 3px solid #ccc;
            width: 50%;
            margin: 10px;
            font-weight: 700;
            color: #666;
            text-align: right;
            line-height: 18px;
        }

            .table7 td {
                padding: 5px;
            }
    </style>

    <script type="text/javascript" src="https://cdn.datatables.net/1.10.10/js/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.10/css/jquery.dataTables.min.css">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".tbl_default").prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]] //value:item pair
            });
        });

        $(window).load(function () {
            //jScript();
        });

        function jScript() {
            $(".tbl_default").prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]] //value:item pair
            });
        }

    </script>


    <style type="text/css">
        .col-md-4 .control-group input, .col-md-4 .control-group select {
            width: 100%;
        }

        .col-md-4 .control-group label {
            padding-bottom: 4px;
        }

        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td {
            border: 1px solid #ddd;
            color: GrayText;
        }

        input#ctl00_BodyContent_chkMerge, height_fix {
            height: 17px !important;
            top: -3px !important;
            margin-top: -5px !important;
        }

        bottom_fix {
            margin-bottom: -15px !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">        </asp:ScriptManager>

    <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
        <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
            <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
        </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
             <script type="text/javascript" language="javascript">
                 Sys.Application.add_load(callJquery);
            </script>
	--%>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">
                <div class="col-md-12">
                    <!-- BEGIN EXAMPLE TABLE PORTLET -->
                    <div class="portlet box light-grey" style="min-height: 355px;">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-globe"></i>Sale by Brand
                            </div>

                        </div>

                        <asp:Label ID="lblProject" runat="server" Text="1" Visible="false"></asp:Label>

                        <div class="portlet-body form">
                            <div class="form-body col-md-12">

                                <div class="form-group col-md-4">
                                    <label class="control-label col-md-4">Company : </label>
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="ddCustomer" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource2"
                                            DataTextField="PartyName" DataValueField="CustomerID" AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT DISTINCT a.CustomerID, (Select Company from Party where PartyID=a.CustomerID) AS PartyName FROM Sales a ORDER BY [PartyName]">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>
                                
                                <div class="form-group col-md-4">
                                    <label class="control-label col-md-4">Brand : </label>
                                      <div class="col-md-8">
                                        <asp:DropDownList ID="ddBrand" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" DataTextField="BrandName" DataValueField="BrandID">
                                                </asp:DropDownList>
                                                     <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT [BrandID], [BrandName] FROM [CustomerBrands] WHERE (([CustomerID] = @CustomerID) AND ([ProjectID] = @ProjectID)) Order by BrandName">
                                                          <SelectParameters>
                                                            <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                                              <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                          <asp:CheckBox ID="CheckBox2" runat="server" Text="All Customers" AutoPostBack="true" OnCheckedChanged="CheckBox2_CheckedChanged" CssClass="chkbox" TextAlign="Right" />
                                          
                                                    </div>
                                                </div>

                                

                                <div class="form-group col-md-4">
                                    <label class="control-label col-md-4">Grade : </label>
                                    <div class="col-md-8">
                                                        <asp:DropDownList ID="ddGrade" runat="server" DataSourceID="SqlDataSource5"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddGrade_OnSelectedIndexChanged"
                                                            DataTextField="GradeName" DataValueField="GradeID"  CssClass="form-control select2me" >
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT GradeID,GradeName from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=2 AND ProjectID=1) ORDER BY [GradeName]"></asp:SqlDataSource>
                                                    </div>
                                                </div>

                                <div class="form-group col-md-4">
                                    <label class="control-label col-md-4">Pack Size : </label>
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="ddItem" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource6"
                                            DataTextField="BrandName" DataValueField="BrandID" AutoPostBack="true" OnSelectedIndexChanged="ddItem_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Text="Show Pack Sizes" AutoPostBack="true" OnCheckedChanged="CheckBox1_CheckedChanged" CssClass="chkbox" TextAlign="Right" />
                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT BrandID, BrandName FROM Brands  ORDER BY DisplaySl">
                                        </asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <label class="control-label col-md-3">Date Range: </label>
                                    <div class="col-md-8">
                                        <div class="input-group input-large date-picker input-daterange" data-date="10/11/2012" data-date-format="dd/mm/yyyy">
                                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" name="from" />
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy" />
                                            <span class="input-group-addon">to</span>
                                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" name="to" />
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtDateTo" Format="dd/MM/yyyy" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-2">

                                    <%--<asp:CheckBox ID="ChkAll" runat="server" Text="All Items" />--%>

                                    <asp:Button ID="btnShow" runat="server" Text="Show History" CssClass="btn blue" OnClick="btnShow_Click" />
                                    <asp:Button ID="btnExport" runat="server" Width="120px" Text="Export" OnClick="btnExport_OnClick" />
                                    <%--<asp:Button ID="btnPrint" runat="server" Width="100px" Text="Print All" OnClick="btnPrint_OnClick" />--%>
                                </div>

                            </div>

                            <iframe id="if1" runat="server" height="800px" width="100%" ></iframe>

                            <div class="form-body col-md-12 table-responsive">


                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="GridView1_OnPageIndexChanging" Visible="False"
                                    DataKeyNames="InvNo" RowStyle-CssClass="odd gradeX" OnRowDataBound="GridView1_RowDataBound" CssClass="tbl_default zebra table" Font-Names="Arial, Sans-serif" font-Size="11px"  >

                            <Columns>

                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>.
                                    </ItemTemplate>
                                    <ItemStyle Width="20px" />
                                </asp:TemplateField>

                                        <asp:BoundField DataField="ProductName" HeaderText="Product" SortExpression="Nine2Twelve" />

                                        <asp:BoundField DataField="Quantity" HeaderText="Qty.(Pcs.)" SortExpression="Twelve2Fifteen" />
                                        <asp:BoundField DataField="UnitCost" HeaderText="Price (Tk.)" SortExpression="Fifteen2Eighteen" />
                                        <asp:BoundField DataField="ItemTotal" HeaderText="Item Total (Tk.)" SortExpression="TwentyOne2TwentyFour" />
                                        <asp:BoundField DataField="VAT" HeaderText="VAT (Tk.)" SortExpression="Amount" />
                                        <asp:BoundField DataField="TotalAmt" HeaderText="Total Amount (Tk.)" SortExpression="Amount" />
                                        <asp:BoundField DataField="UnitWeight" HeaderText="U.Weight (gm.)" SortExpression="Eighteen2TwentyOne" />
                                        <asp:BoundField DataField="TotalWeight" HeaderText="Total Weight (kg.)" SortExpression="Total" />
                                        <asp:BoundField DataField="EntryDate" HeaderText="Date" SortExpression="Six2Nine" />
                                        <asp:TemplateField HeaderText="Inv.No." SortExpression="sDate">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="Label1" runat="server" Text='<%# Bind("InvNo") %>' NavigateUrl='<%# Bind("LINK") %>' Target="_blank"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                        </asp:GridView>

                                <%--<asp:GridView ID="GridView1" class="table table-striped table-bordered table-hover dataTable" aria-describedby="sample_1_info"
                                    runat="server" AutoGenerateColumns="False" PageSize="10" AllowPaging="True"
                                    ShowFooter="True" AllowSorting="True" Width="120%" OnPageIndexChanging="GridView1_OnPageIndexChanging"
                                    DataKeyNames="InvNo" RowStyle-CssClass="odd gradeX" OnRowDataBound="GridView1_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="ProductName" HeaderText="Product" SortExpression="Nine2Twelve" />

                                        <asp:BoundField DataField="Quantity" HeaderText="Qty.(Pcs.)" SortExpression="Twelve2Fifteen" />
                                        <asp:BoundField DataField="UnitCost" HeaderText="Price (Tk.)" SortExpression="Fifteen2Eighteen" />
                                        <asp:BoundField DataField="ItemTotal" HeaderText="Item Total (Tk.)" SortExpression="TwentyOne2TwentyFour" />
                                        <asp:BoundField DataField="VAT" HeaderText="VAT (Tk.)" SortExpression="Amount" />
                                        <asp:BoundField DataField="TotalAmt" HeaderText="Total Amount (Tk.)" SortExpression="Amount" />
                                        <asp:BoundField DataField="UnitWeight" HeaderText="U.Weight (gm.)" SortExpression="Eighteen2TwentyOne" />
                                        <asp:BoundField DataField="TotalWeight" HeaderText="Total Weight (kg.)" SortExpression="Total" />
                                        <asp:BoundField DataField="EntryDate" HeaderText="Date" SortExpression="Six2Nine" />
                                        <asp:TemplateField HeaderText="Inv.No." SortExpression="sDate">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="Label1" runat="server" Text='<%# Bind("InvNo") %>' NavigateUrl='<%# Bind("LINK") %>' Target="_blank"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#f3f3f3" Font-Bold="True" ForeColor="#23A6F0" BorderStyle="Solid" />
                                    <RowStyle CssClass="odd gradeX" />
                                </asp:GridView>--%>

                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [InvNo], (Select DefaultLink from Settings_Project where sid=1)+'XerpReports/frmPO.aspx?inv='+InvNo AS LINK, [OrderDate], [CustomerName], [TotalAmount], [Discount], [ServiceCharge], [CollectedAmount], [PaidAmount], [Due], [BillNo] FROM [Sales] WHERE ([ProjectID] = @ProjectID) ORDER BY [OrderID] DESC">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="1" Name="ProjectID" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                            </div>


                        </div>
                        <!-- END EXAMPLE TABLE PORTLET-->
                    </div>
                </div>



            </div>


           <%-- <div class="grid_6"> 

                <table class="table7">
                    <tr>
                        <td>Grand Total Quantity (Pcs.): </td> 
                        <td><asp:Literal ID="ltrQty" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Grand Item Total (Tk.): </td>
                        <td><asp:Literal ID="ltrItemLoad" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Grand Total VAT (Tk.): </td>
                        <td><asp:Literal ID="ltrTotalVat" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Grand Total Amount (Tk.): </td>
                        <td><asp:Literal ID="ltrGTAmt" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Grand Total Weight (kg.): </td>
                        <td><asp:Literal ID="ltrTotalWeight" runat="server" /></td>
                    </tr>
                </table>
            </div>--%>




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

