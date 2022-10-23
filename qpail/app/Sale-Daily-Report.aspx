<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Sale-Daily-Report.aspx.cs" Inherits="Operator_Sale_Daily_Report" %>
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
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>



    <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="help-block"></asp:Label>


    <div class="row">
        <div class="col-md-12">
            <!-- BEGIN EXAMPLE TABLE PORTLET-->
            <div class="portlet box light-grey">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-globe"></i>Sale History by Time-Frame
                    </div>
                </div>
                <div class="portlet-body form">
                    <div class="form-body col-md-12">

                        <asp:Label ID="lblProject" runat="server" Text="1" Visible="false"></asp:Label>


                        <div class="form-group col-md-4">
                            <label class="control-label col-md-4">Items : </label>
                            <div class="col-md-8">
                                <asp:RadioButton ID="ChkAll" runat="server" Text="All Items" GroupName="grp" Checked="true" />
                                <asp:RadioButton ID="RadioButton1" runat="server" Text="Sold Only" GroupName="grp" />

                                <%--<asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource6" DataTextField="name" DataValueField="ItemCode" AutoPostBack="True" >
                                        
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [ItemCode], [name] FROM [Items] WHERE ([ProjectID] = @ProjectID) ORDER BY [name]">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>--%>
                            </div>
                        </div>

                        <div class="form-group col-md-6">
                            <label class="control-label col-md-3">Date Range: </label>
                            <div class="col-md-8">
                                <div class="input-group input-large date-picker input-daterange" data-date="10/11/2012" data-date-format="dd/mm/yyyy">
                                    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" name="from" />
                                    <span class="input-group-addon">to
                                    </span>
                                    <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" name="to" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-md-2">

                            <asp:Button ID="btnShow" runat="server" Text="Show History" CssClass="btn blue" OnClick="btnShow_Click" />
                            <%--<asp:Button ID="btnPrint" runat="server" Text="Print Ledger" onclick="btnPrint_Click" />--%>
                        </div>


                        <div class="col-md-12">
                            <hr />
                        </div>


                    </div>


<asp:UpdatePanel ID="upnl" runat="server" UpdateMode="Conditional"> <ContentTemplate>


                    <div class="form-body col-md-12">

                        <asp:GridView ID="GVrpt" class="table table-striped table-bordered table-hover dataTable" aria-describedby="sample_1_info" runat="server" AutoGenerateColumns="False"
                            ShowFooter="true" AllowSorting="true"
                            DataKeyNames="iName" RowStyle-CssClass="odd gradeX" OnRowDataBound="GVrpt_RowDataBound">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="iName" HeaderText="Item Name" ReadOnly="True" SortExpression="iName" />
                                <asp:BoundField DataField="Six2Nine" HeaderText="12AM-09AM" SortExpression="Six2Nine" />
                                <asp:BoundField DataField="Nine2Twelve" HeaderText="09AM-12PM" SortExpression="Nine2Twelve" />
                                <asp:BoundField DataField="Twelve2Fifteen" HeaderText="12PM-03PM" SortExpression="Twelve2Fifteen" />
                                <asp:BoundField DataField="Fifteen2Eighteen" HeaderText="03PM-06PM" SortExpression="Fifteen2Eighteen" />
                                <asp:BoundField DataField="Eighteen2TwentyOne" HeaderText="06PM-09PM" SortExpression="Eighteen2TwentyOne" />
                                <asp:BoundField DataField="TwentyOne2TwentyFour" HeaderText="09PM-12AM" SortExpression="TwentyOne2TwentyFour" />
                                <asp:BoundField DataField="Total" HeaderText="Total Qty." SortExpression="Total" />
                                <asp:BoundField DataField="Amount" HeaderText="Total Amount" SortExpression="Amount" />

                            </Columns>
                            <FooterStyle BackColor="#AAAAAA" Font-Bold="True" ForeColor="Black" BorderStyle="Solid"  />
                        </asp:GridView>

                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [InvNo], [OrderDate], [CustomerName], [TotalAmount], [Discount], [ServiceCharge], [CollectedAmount], [PaidAmount], [Due], [BillNo] FROM [Sales] WHERE ([ProjectID] = @ProjectID) ORDER BY [OrderID] DESC">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="1" Name="ProjectID" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        
                    </div>
    </ContentTemplate> </asp:UpdatePanel>

                </div>
                <!-- END EXAMPLE TABLE PORTLET-->
            </div>
        </div>



    </div>











    <script type="text/javascript" src="assets/plugins/data-tables/jquery.dataTables.js"></script>
    <script type="text/javascript" src="assets/plugins/data-tables/DT_bootstrap.js"></script>
    <script src="assets/scripts/custom/table-managed.js"></script>
    <script>
        jQuery(document).ready(function () {
            TableManaged.init();
        });
    </script>

</asp:Content>

