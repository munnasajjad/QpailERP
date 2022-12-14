<%@ Page Title="Machine Wise Production" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="MachineWiseProduction.aspx.cs" Inherits="app_MachineWiseProduction" %>

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

            <div class="row">


                <div class="col-md-12 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box green ">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Plastic Container Production by Date
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

                                                <div class="col-md-4" runat="server">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label">Machine No. : </label>
                                                        <asp:DropDownList ID="ddMachineNo" runat="server" DataSourceID="SqlDataSource1"
                                                            DataTextField="MachineName" DataValueField="mid" AppendDataBoundItems="True">
                                                            <asp:ListItem Value="">--- all ---</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT mid, CONCAT(MachineNo,' ',Description) AS MachineName FROM Machines"></asp:SqlDataSource>
                                                    </div>
                                                </div>
                                                <div class="col-md-4" runat="server">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label">Type : </label>
                                                        <asp:DropDownList ID="ddType" runat="server" DataSourceID="SqlDataSource2"
                                                            DataTextField="Type" DataValueField="Type" AppendDataBoundItems="True">
                                                            <asp:ListItem Value="">--- all ---</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT DISTINCT Type FROM VwMachineWiseProduction"></asp:SqlDataSource>
                                                    </div>
                                                </div>

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


                                                <div class="col-md-8">
                                                    <div class="control-group">
                                                        <div style="margin-top: 29px">
                                                            <asp:Button ID="btnSearch" runat="server" Text="Detail Report" Width="120px" OnClick="btnAdd_Click" />
                                                            <asp:Button ID="btnExport" runat="server" Width="130px" Text="Summery Report" OnClick="btnExport_OnClick" />
                                                            <asp:Button ID="btnAllShift" runat="server" Width="120px" Text="All Shift" OnClick="btnAllShift_OnClick" />
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
                        <%--<div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Daily Production Report
                    </div>
                    <div class="tools">
                        <a href="" class="collapse"></a>
                        <a href="#portlet-config" data-toggle="modal" class="config"></a>
                        <a href="" class="reload"></a>
                        <a href="" class="remove"></a>
                    </div>
                </div>--%>
                        <div class="portlet-body form">



                            <iframe id="if1" runat="server" height="800px" width="100%"></iframe>


                            <div class="table-responsive">
                                <asp:Literal ID="ltrtotal" runat="server"></asp:Literal>


                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="True" SelectedRowStyle-BackColor="LightBlue"
                                    OnRowDataBound="ItemGrid_RowDataBound" CssClass="tbl_default zebra table" Visible="False">

                                    <Columns>

                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>.
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" />
                                        </asp:TemplateField>

                                    </Columns>

                                    <SelectedRowStyle BackColor="LightBlue" />

                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT TOP(100) [EntryDate], [EntryType],
                            (Select Purpose from Purpose where pid=stock.Purpose) AS Purpose,
                                    [InvoiceID], [RefNo], [ProductName],
                                    (SELECT  [Company] FROM [Party] WHERE [PartyID]= Stock.Customer) AS Customer,
                                    (SELECT [BrandName] FROM [CustomerBrands] WHERE BrandID=Stock.BrandID) AS BrandID,
                                    (SELECT [BrandName] FROM [Brands] WHERE BrandID=Stock.SizeId) AS SizeId,
                                    (SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=Stock.Color) AS Color,
                                    (SELECT [spec] FROM [Specifications] WHERE  CAST(id AS nvarchar)=Stock.Spec) AS Spec,
                                    [InQuantity], [OutQuantity], [InWeight], [OutWeight], Remark FROM [Stock] WHERE ([ProductID] = @ProductID)
                                   ORDER BY [EntryDate] DESC, [EntryID] DESC">
                                    <SelectParameters>
                                        <%--<asp:ControlParameter ControlID="ddProduct" Name="ProductID" PropertyName="SelectedValue" Type="String" />
                                        <asp:ControlParameter ControlID="ddType" Name="ItemType" PropertyName="SelectedValue" Type="String" />--%>
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


