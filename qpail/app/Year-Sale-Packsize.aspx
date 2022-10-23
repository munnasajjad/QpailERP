<%@ Page Title="Yearly Sales by Pack-Size" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Year-Sale-Packsize.aspx.cs" Inherits="app_Year_Sale_Packsize" %>

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
        <ContentTemplate>--%>
             <script type="text/javascript" language="javascript">
                 Sys.Application.add_load(callJquery);
            </script>
	
			

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">
                <div class="col-md-12">
                    <!-- BEGIN EXAMPLE TABLE PORTLET -->
                    <div class="portlet box light-grey" style="min-height: 355px;">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-globe"></i>Yearly Sales by Pack-Size
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
                                    <label class="control-label col-md-4">Item Grade : </label>
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
                                        <asp:CheckBox ID="CheckBox1" runat="server" Text="Show All Items" AutoPostBack="true" OnCheckedChanged="CheckBox1_CheckedChanged" CssClass="chkbox" TextAlign="Right" />
                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT BrandID, BrandName FROM Brands  ORDER BY DisplaySl">
                                        </asp:SqlDataSource>
                                    </div>
                                </div>
                                <%--<div class="form-group col-md-4">
                                    <label class="control-label col-md-4">Item Name : </label>
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="ddItem" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource6"
                                            DataTextField="ProductName" DataValueField="ProductName" AutoPostBack="true" OnSelectedIndexChanged="ddItem_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Text="Show All Items" AutoPostBack="true" OnCheckedChanged="CheckBox1_CheckedChanged" CssClass="chkbox" TextAlign="Right" />
                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT DISTINCT ProductName FROM SaleDetails WHERE 
                                            (ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID=@GradeID))) AND (InvNo IN (SELECT InvNo FROM Sales WHERE (CustomerID = @CustomerID))) ORDER BY ProductName">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                                <asp:ControlParameter ControlID="ddGrade" Name="GradeID" PropertyName="SelectedValue" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>--%>

                                <div class="form-group col-md-6">
                                    <label class="control-label col-md-3">Year : </label>
                                    <div class="col-md-8">
                                        <div class="input-group input-large date-picker input-daterange" data-date="10/11/2012" data-date-format="dd/mm/yyyy">
                                            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" name="from" />
                                            <%--<asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy" />
                                            <span class="input-group-addon">to</span>
                                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" name="to" />
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtDateTo" Format="dd/MM/yyyy" />--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-2">

                                    <%--<asp:CheckBox ID="ChkAll" runat="server" Text="All Items" />--%>

                                    <asp:Button ID="btnShow" runat="server" Text="Show History" CssClass="btn blue" OnClick="btnShow_Click" />
                                    <asp:Button ID="btnExport" runat="server" Width="120px" Text="Export" OnClick="btnExport_OnClick" />
                                    <%--<asp:Button ID="btnPrint" runat="server" Text="Print Ledger" onclick="btnPrint_Click" />--%>
                                </div>

                            </div>



                            <div class="form-body col-md-12 table-responsive">

                                <asp:GridView ID="GVrpt" class="table table-striped table-bordered table-hover dataTable" aria-describedby="sample_1_info"
                                    runat="server" AutoGenerateColumns="False" 
                                     RowStyle-CssClass="odd gradeX" OnRowDataBound="GVrpt_RowDataBound">
                                    <Columns>
                                        <%--<asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                        <asp:BoundField DataField="RowName" HeaderText="" SortExpression="Nine2Twelve" />
                                        <asp:BoundField DataField="January" HeaderText="January" SortExpression="Nine2Twelve" />
                                        <asp:BoundField DataField="February" HeaderText="February" SortExpression="Twelve2Fifteen" />
                                        <asp:BoundField DataField="March" HeaderText="March" SortExpression="Fifteen2Eighteen" />
                                        <asp:BoundField DataField="April" HeaderText="April" SortExpression="TwentyOne2TwentyFour" />
                                        <asp:BoundField DataField="May" HeaderText="May" SortExpression="Amount" />
                                       
                                         <asp:BoundField DataField="June" HeaderText="June" SortExpression="Amount" />
                                        <asp:BoundField DataField="July" HeaderText="July" SortExpression="Eighteen2TwentyOne" />
                                        <asp:BoundField DataField="August" HeaderText="August" SortExpression="Total" />
                                        <asp:BoundField DataField="September" HeaderText="September" SortExpression="Six2Nine" />
                                        
                                        <asp:BoundField DataField="October" HeaderText="October" SortExpression="Amount" />
                                        <asp:BoundField DataField="November" HeaderText="November" SortExpression="Eighteen2TwentyOne" />
                                        <asp:BoundField DataField="December" HeaderText="December" SortExpression="Total" />
                                        <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Six2Nine" />
                                       
                                    </Columns>
                                    <FooterStyle BackColor="#f3f3f3" Font-Bold="True" ForeColor="#23A6F0" BorderStyle="Solid" />
                                    <RowStyle CssClass="odd gradeX" />
                                </asp:GridView>

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

            <%--<div class="grid_6">

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

    <a href="javascript:window.print()"><img src="../images/print.png" alt="print report" id="print-button" width="40px"></a>

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

