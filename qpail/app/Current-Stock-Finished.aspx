﻿<%@ Page Title="Current Stock (Finished)" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Current-Stock-Finished.aspx.cs" Inherits="app_Current_Stock_Finished" %>

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
                        <i class="fa fa-reorder"></i>Current Stock (Finished)
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
                                        <div class="col-md-4">
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
                                        </div>
                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">Location : </label>
                                                <asp:DropDownList ID="ddLocation" runat="server" DataSourceID="SqlDataSource16"
                                                    DataTextField="AreaName" DataValueField="AreaID" CssClass="form-control" AppendDataBoundItems="True">
                                                    <asp:ListItem>--- all ---</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource16" runat="server"
                                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT AreaID,[AreaName] FROM [WareHouseAreas] WHERE Warehouse=@Warehouse">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddGodown" Name="Warehouse" PropertyName="SelectedValue" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label>Pack Size :</label>

                                                <asp:DropDownList ID="ddSize" runat="server" DataSourceID="SqlDataSource4" DataTextField="BrandName" DataValueField="BrandID"
                                                    AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddCustomer_OnSelectedIndexChanged">
                                                    <asp:ListItem Value="--- all ---">--- all ---</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] order by DisplaySl">

                                                </asp:SqlDataSource>

                                            </div>
                                        </div>

                                        <div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Group</label>
                                                <asp:DropDownList ID="ddGroup" runat="server" DataSourceID="SqlDataSource1" AutoPostBack="true"
                                                    DataTextField="GroupName" DataValueField="GroupSrNo">
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] WHERE GroupSrNo=2 ORDER BY [GroupSrNo]"></asp:SqlDataSource>

                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Subgroup </label>

                                                <asp:DropDownList ID="ddSubGroup" runat="server" DataSourceID="SqlDataSource3" DataTextField="CategoryName" DataValueField="CategoryID"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddSubGroup_OnSelectedIndexChanged" AppendDataBoundItems="True">

                                                    <%--<asp:ListItem>--- all ---</asp:ListItem>--%>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT CategoryID, CategoryName FROM [ItemSubGroup] where (GroupID = @GroupID) ">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddGroup" Name="GroupID" PropertyName="SelectedValue" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="col-md-4" id="tinPlate" runat="server">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Customer </label>
                                                <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource2"
                                                                DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddCustomer_OnSelectedIndexChanged">

                                                                <asp:ListItem Value="--- all ---">--- all ---</asp:ListItem>

                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                                                <SelectParameters>
                                                                    <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="col-md-4" id="dfsdf" runat="server">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Brand </label>

                                                <asp:DropDownList ID="ddBrand" runat="server" DataSourceID="SqlDataSource5" DataTextField="BrandName" DataValueField="BrandID"
                                                     AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddBrand_OnSelectedIndexChanged">
                                                            <asp:ListItem Value="--- all ---">--- all ---</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [BrandID], [BrandName] FROM [CustomerBrands] WHERE (([CustomerID] = @CustomerID) AND ([ProjectID] = @ProjectID))">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                                        <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="col-md-4" runat="server">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">Stock  (Up To) Date : </label>
                                                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                    Enabled="True" TargetControlID="txtDate">
                                                </asp:CalendarExtender>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="control-group">
                                                <div style="margin-top: 29px">
                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" Width="100px" OnClick="btnAdd_Click" />
                                                    <asp:Button ID="btnExport" runat="server" Width="120px" Text="Export to Excel" OnClick="btnExport_OnClick" />
                                                    <asp:Button ID="btnPrint" runat="server" Width="100px" Text="Print All" OnClick="btnPrint_OnClick" />
                                                </div>
                                            </div>
                                        </div>

                                    </asp:Panel>
                                    
                                    <iframe id="if1" runat="server" height="800px" width="100%" ></iframe>

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

                    <div class="table-responsive">
                        <asp:Literal ID="ltrtotal" runat="server"></asp:Literal>

                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="True" OnRowDataBound="ItemGrid_RowDataBound" CssClass="tbl_default zebra table" Font-Names="Arial, Sans-serif" font-Size="11px" Visible="False">

                            <Columns>

                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>.
                                    </ItemTemplate>
                                    <ItemStyle Width="20px" />
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>

                    </div>

                </div>
            </div>
        </div>
    </div>




    </ContentTemplate>
    </asp:UpdatePanel>




</asp:Content>


