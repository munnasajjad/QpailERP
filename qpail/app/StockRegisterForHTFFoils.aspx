<%@ Page Title="Stock Register For HTF Foils" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="StockRegisterForHTFFoils.aspx.cs" Inherits="app_StockRegisterForHTFFoils" %>

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
            text-align: left;
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


            <h3 class="page-title">Stock Register For HTF Foils</h3>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">

                <div class="col-lg-12">
                    <div class="portlet box green ">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>HTF Foils Stock
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
                                                <div class="col-md-4" id="Div2" runat="server">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label">Customer : </label>
                                                        <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource1"
                                                            DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddCustomer_OnSelectedIndexChanged">

                                                            <asp:ListItem>--- all ---</asp:ListItem>

                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                                            <SelectParameters>
                                                                <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                </div>
                                                <div class="col-md-4" runat="server">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label">Godown : </label>
                                                        <asp:DropDownList ID="ddGodown" runat="server" DataSourceID="SqlDataSource6"
                                                            DataTextField="StoreName" DataValueField="WID" AppendDataBoundItems="True" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT WID, StoreName FROM Warehouses">
                                                            <SelectParameters>
                                                                <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                </div>
                                                <div class="col-md-4" id="tinPlate" runat="server">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label">Grade : </label>
                                                        <asp:DropDownList ID="ddGrade" runat="server" DataSourceID="SqlDataSource5" AutoPostBack="true" OnSelectedIndexChanged="ddGrade_OnSelectedIndexChanged" DataTextField="GradeName" DataValueField="GradeID"></asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT GradeID, GradeName FROM ItemGrade WHERE CategoryID='33' ORDER BY [GradeName]"></asp:SqlDataSource>
                                                    </div>
                                                </div>

                                                <div class="col-md-4" id="Div3" runat="server">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label">Category : </label>
                                                        <asp:DropDownList ID="ddCategory" runat="server" DataSourceID="SqlDataSource8"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged"
                                                            DataTextField="CategoryName" DataValueField="CategoryID">
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT CategoryID, CategoryName FROM [Categories] where GradeID = @GradeID ORDER BY [CategoryName]">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="ddGrade" Name="GradeID" PropertyName="SelectedValue" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                </div>


                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label>Ink Name :</label>
                                                        <asp:DropDownList ID="ddInkName" runat="server" DataSourceID="SqlDataSource4"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddInkName_OnSelectedIndexChanged"
                                                            DataTextField="ItemName" DataValueField="ProductID">
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE [CategoryID] = @CategoryID ORDER BY [ItemName]">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="ddCategory" Name="CategoryID" PropertyName="SelectedValue" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                </div>

                                                <div class="col-md-4" runat="server">
                                                    <div class="control-group">
                                                        <label class="control-label full-wdth">Date From: </label>
                                                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                                            Enabled="True" TargetControlID="txtDateFrom">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>

                                                <div class="col-md-4" runat="server">
                                                    <div class="control-group">
                                                        <label class="control-label full-wdth">Date To: </label>
                                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy"
                                                            Enabled="True" TargetControlID="txtDateTo">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <div style="margin-top: 29px">
                                                            <asp:Button ID="btnSearch" runat="server" Text="Search" Width="100px" OnClick="btnSearch_Click" />
                                                            <%--<asp:Button ID="btnExport" runat="server" Width="120px" Text="Export to Excel" OnClick="btnExport_OnClick" />--%>
                                                            <%--<asp:Button ID="btnSearch" CssClass="btn btn-s-md btn-primary" runat="server" Text="Search" OnClick="btnSearch_Click" />--%>
                                                            <%--<asp:Button ID="btnReset" CssClass="btn btn-s-md btn-danger" runat="server" Text="Reset" OnClick="btnReset_OnClick" />--%>
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
                <div class="col-lg-12">
                    <section class="panel">

                        <div id="Div1">
                            <div>
                                <fieldset>
                                    <legend>Query Result </legend>

                                    <div class="table-responsive">
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-hover"
                                            BorderStyle="Solid" BorderWidth="1px" CellPadding="7" ForeColor="Black" GridLines="Vertical" AllowPaging="True" PageSize="100" OnPageIndexChanging="GridView1_OnPageIndexChanging"
                                            OnRowDataBound="GridView1_OnRowDataBound" RowStyle-CssClass="odd gradeX" ShowFooter="True" AllowSorting="True">

                                            <Columns>

                                                <asp:TemplateField ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" SortExpression="ItemName" />
                                                <asp:BoundField DataField="OpeningStock" HeaderText="Opening Stock" SortExpression="OpeningStock" />
                                                <asp:BoundField DataField="Received" HeaderText="Received" SortExpression="Received" />
                                                <asp:BoundField DataField="Issued" HeaderText="Issued" SortExpression="Issued" />
                                                <asp:BoundField DataField="Balanced" HeaderText="Balanced" SortExpression="Balanced" />

                                                <%--<asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="QtyBalance" />
                                                <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="QtyBalance" />
                                                <asp:BoundField DataField="Used" HeaderText="Used" SortExpression="QtyBalance" />
                                                <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="QtyBalance" />--%>
                                            </Columns>

                                            <FooterStyle BackColor="#f3f3f3" Font-Bold="True" ForeColor="#23A6F0" BorderStyle="Solid" />
                                            <PagerStyle CssClass="gvpaging"></PagerStyle>
                                            <RowStyle CssClass="odd gradeX" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT DISTINCT ProductName, SUM(QtyBalance) AS QtyBalance FROM OrderDetails WHERE (BrandID IN (SELECT BrandID FROM CustomerBrands WHERE (OrderDetails.QtyBalance &gt; 0) AND (CustomerID = @CustomerID))) GROUP BY ProductName">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddInkName" Name="CustomerID" PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        Total Rows Found:
                                        <asp:Literal ID="ltrtotal" runat="server"></asp:Literal>
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
