<%@ Page Title="PO By Date Range" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="PO-Date-Range.aspx.cs" Inherits="app_PO_Date_Range" %>

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

    </script>


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


    <h3 class="page-title">PO By Date Range</h3>

    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
    <br />
    <div class="col-lg-12">
        <div class="row">

            <div class="col-md-4">
                <div class="control-group">
                    <label class="col-sm-12 control-label">Customer : &nbsp; </label>

                    <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource3"
                        DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="true">
                        <asp:ListItem>---ALL---</asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'customer') ORDER BY [Company]"></asp:SqlDataSource>
                </div>
            </div>

            <div class="col-md-3">
                <div class="control-group">
                    <label class="col-sm-12 control-label">Date From  &nbsp; </label>

                    <asp:TextBox ID="txtInvDate" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtInvDate" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </div>
            </div>

            <div class="col-md-3">
                <div class="control-group">
                    <label class="col-sm-12 control-label">Date To  &nbsp; </label>

                    <asp:TextBox ID="txtPODate" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtPODate" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </div>
            </div>

            <div class="col-md-2">
                <div class="control-group">
                    <div style="margin-top: 0px">
                        <asp:Button ID="btnSearch" CssClass="btn btn-s-md btn-primary" runat="server" Text="SHOW" OnClick="btnSearch_OnClick" />
                        <asp:Button ID="btnReset" CssClass="btn btn-danger" runat="server" Text="Reset" OnClick="btnReset_OnClick" Visible="False" />
                    </div>
                </div>
            </div>

        </div>
    </div>



    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="tbl_default zebra table"
        BorderStyle="Solid" BorderWidth="1px" CellPadding="7" ForeColor="Black" GridLines="Vertical"
        DataKeyNames="OrderSl" AllowPaging="True" PageSize="100" OnPageIndexChanging="GridView1_OnPageIndexChanging"
        OnRowDataBound="GridView1_OnRowDataBound" RowStyle-CssClass="odd gradeX" ShowFooter="False">


        <Columns>

            <asp:TemplateField ItemStyle-Width="20px">
                <ItemTemplate>
                    <%#Container.DataItemIndex+1 %>
                </ItemTemplate>
                <ItemStyle Width="20px" />
            </asp:TemplateField>

            <asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="phone" />
            <asp:TemplateField HeaderText="PO.No" SortExpression="Name">
                <ItemTemplate>
                    <asp:HyperLink ID="Label1" runat="server" Text='<%# Bind("PoNo") %>' NavigateUrl='<%# Bind("link") %>' Target="_blank"></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="OrderDate" HeaderText="PO.Date" SortExpression="address" DataFormatString="{0:d}" />

            <asp:BoundField DataField="DeliveryAddress" HeaderText="Del.Address" SortExpression="email" DataFormatString="{0:d}" />

            <asp:BoundField DataField="TotalAmount" HeaderText="PO.Amount" SortExpression="Name" ReadOnly="True" />
            <asp:BoundField DataField="Vat" HeaderText="VAT(%)" SortExpression="address" />
            <asp:BoundField DataField="PayableAmount" HeaderText="Net Amount" SortExpression="phone" />

            <asp:BoundField DataField="status" HeaderText="C.Status" SortExpression="email" />
            <asp:BoundField DataField="DeliveryStatus" HeaderText="Del.Status" SortExpression="phone" />

        </Columns>
        <FooterStyle BackColor="#f3f3f3" Font-Bold="True" ForeColor="#23A6F0" BorderStyle="Solid" />
        <PagerStyle CssClass="gvpaging"></PagerStyle>
    </asp:GridView>


    <%--Total Rows Found: <asp:Literal ID="ltrtotal" runat="server"></asp:Literal>--%>


        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

