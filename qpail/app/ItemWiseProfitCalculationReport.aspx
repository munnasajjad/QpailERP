<%@ Page Title="Item Wise Profit" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="ItemWiseProfitCalculationReport.aspx.cs" Inherits="app_ItemWiseProfitCalculationReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">

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

    <div class="row">


        <div class="col-md-12 ">
            <!-- BEGIN SAMPLE FORM PORTLET-->
            <div class="portlet box green ">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Item Wise Profit Report
                    </div>
                    <div class="tools">
                        <a href="" class="collapse"></a>
                        <a href="#portlet-config" data-toggle="modal" class="config"></a>
                        <a href="" class="reload"></a>
                        <a href="" class="remove"></a>
                    </div>
                </div>
                <div class="portlet-body form">
                    <div class="form-horizontal" role="form">
                        <div class="form-body">
                            <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>
                            <asp:LoginName ID="LoginName1" runat="server" Visible="false" />
                            <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>
                            <div class="col-md-12">
                                <div class="row">
                                    <asp:Panel ID="pnlAdd" runat="server" DefaultButton="btnSearch">
                                        <%--<div class="col-md-4">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">Godown : </label>
                                                <asp:DropDownList ID="ddGodown" runat="server" DataSourceID="SqlDataSource6"
                                                    DataTextField="StoreName" DataValueField="WID" CssClass="form-control" AppendDataBoundItems="True"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddGodown_SelectedIndexChanged">
                                                    <asp:ListItem>--- all ---</asp:ListItem>

                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource6" runat="server"
                                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [WID], [StoreName] FROM [Warehouses]"></asp:SqlDataSource>
                                            </div>
                                        </div>--%>
                                        <div class="col-md-4" id="tinPlate" runat="server">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Customer : </label>
                                                <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource2"
                                                    DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="True">
                                                    <asp:ListItem Value="">--- all ---</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Subgroup : </label>
                                                <asp:DropDownList ID="ddSubGroup" runat="server" DataSourceID="SqlDataSource3" DataTextField="CategoryName" DataValueField="CategoryID"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddSubGroup_OnSelectedIndexChanged">
                                                    <asp:ListItem Value="">--- all ---</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT CategoryID, CategoryName FROM [ItemSubGroup]"></asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Grade : </label>
                                                <asp:DropDownList ID="ddGrade" runat="server"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddGrade_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label>Brand :</label>
                                                <asp:DropDownList ID="ddBrand" runat="server" DataSourceID="SqlDataSource6" DataTextField="BrandName" DataValueField="BrandID" AppendDataBoundItems="True">
                                                    <asp:ListItem Value="">--- all ---</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [BrandID], [BrandName] FROM [CustomerBrands] WHERE (([CustomerID] = @CustomerID)) ORDER BY BrandName ASC">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label>Pack Size :</label>
                                                <asp:DropDownList ID="ddSize" runat="server" DataSourceID="SqlDataSource4" DataTextField="BrandName" DataValueField="BrandID"
                                                    AppendDataBoundItems="True">
                                                    <%--AutoPostBack="True" OnSelectedIndexChanged="ddCustomer_OnSelectedIndexChanged">--%>
                                                    <asp:ListItem Value="">--- all ---</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] ORDER BY DisplaySl"></asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Category : </label>
                                                <asp:DropDownList ID="ddCategory" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddcategory_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-md-4">

                                            <div class="control-group">
                                                <label class="control-label">Item :  </label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddItemName" runat="server">
                                                        <asp:ListItem Value="">--- all ---</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="col-md-4" id="Div1" runat="server">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Color : </label>
                                                <asp:DropDownList ID="ddColor" runat="server" DataSourceID="SqlDataSource5"
                                                    DataTextField="DepartmentName" DataValueField="Departmentid" AppendDataBoundItems="True">

                                                    <asp:ListItem>--- all ---</asp:ListItem>

                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Colors] ORDER BY [DepartmentName]">
                                                    <%--<SelectParameters>
                                                                    <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                                                </SelectParameters>--%>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>


                                        <%--<div class="col-md-4 hidden" id="tinPlate" runat="server">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Tin-Plate Type </label>

                                                <asp:DropDownList ID="ddType" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddSubGroup_OnSelectedIndexChanged" AppendDataBoundItems="True">
                                                    <asp:ListItem>Raw Sheet</asp:ListItem>
                                                    <asp:ListItem>Processed Sheet</asp:ListItem>
                                                    <asp:ListItem>Printed Sheet</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>--%>

                                        <div class="col-md-4" runat="server">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">Date Range (From-To) : </label>
                                                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY" Width="50%"></asp:TextBox>
                                                <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY" Width="50%"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                    Enabled="True" TargetControlID="txtDate">
                                                </asp:CalendarExtender>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                    Enabled="True" TargetControlID="txtDateTo">
                                                </asp:CalendarExtender>
                                            </div>
                                        </div>


                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <div style="margin-top: 29px">
                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" Width="100px" OnClick="btnAdd_Click" />
                                                </div>
                                            </div>
                                        </div>

                                    </asp:Panel>

                                </div>
                            </div>


                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>




    <div class="row">
        <div class="col-md-12 ">
            <!-- BEGIN SAMPLE FORM PORTLET-->
            <div class="portlet box blue">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Current Stock
                    </div>
                    <div class="tools">
                        <a href="" class="collapse"></a>
                        <a href="#portlet-config" data-toggle="modal" class="config"></a>
                        <a href="" class="reload"></a>
                        <a href="" class="remove"></a>
                    </div>
                </div>
                <div class="portlet-body form">



                    <iframe id="if1" runat="server" height="800px" width="100%"></iframe>


                    <div class="table-responsive">
                        <asp:Literal ID="ltrtotal" runat="server"></asp:Literal>

                    </div>

                </div>
            </div>
        </div>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
