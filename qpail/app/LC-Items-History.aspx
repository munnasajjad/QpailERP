<%@ Page Title="LC Items Use Status" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="LC-Items-History.aspx.cs" Inherits="app_LC_Items_History" %>

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

        td {
            vertical-align: middle !important;
            padding-left: 5px;
            text-align: center;
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


            <h3 class="page-title">LC Items Use Status (FIFO)</h3>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">

                <div class="col-lg-12">
                    <section class="panel">
                        <%--Body Contants--%>
                        <div id="Div2">
                            <div>

                                <fieldset>
                                    <%--<legend>Search Terms</legend>--%>
                                    <table border="0" width="100%" style="width: 100%" class="table1">
                                        <tr>
                                            <th>Item Group</th>
                                            <th>Item Sub-Group</th>
                                            <th>Item Grade </th>
                                            <th>Category </th>
                                            <th>Product</th>
                                            <th>Spec</th>
                                            <%--<th> </th>
                                            <th>Invoice Date From</th>
                                            <th> </th>
                                           <th>Invoice Date To</th>--%>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddGroup" runat="server" DataSourceID="SqlDataSource2" CssClass="form-control"
                                                    DataTextField="GroupName" DataValueField="GroupSrNo" AutoPostBack="true" OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] Where GroupSrNo<>2 AND GroupSrNo<>3 AND GroupSrNo<>'11' ORDER BY [GroupSrNo]"></asp:SqlDataSource>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddSubGrp" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddSubGrp_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddGrade" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddGrade_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddCategory" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged">
                                                </asp:DropDownList></td>
                                            <td>
                                                <asp:DropDownList ID="ddItemName" runat="server" CssClass="form-control"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddItemName_OnSelectedIndexChanged"  AppendDataBoundItems="True">
                                                    <asp:ListItem>--- all ---</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT Distinct [ItemCode], (Select ItemName from Products where ProductID=LC_Items_Costing.ItemCode) As ItemName FROM  LC_Items_Costing  ORDER BY [ItemName]"></asp:SqlDataSource>

                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddSpec" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                    <asp:ListItem>--- all ---</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource14" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [id], [spec] FROM [Specifications] ORDER BY [spec]"></asp:SqlDataSource>
                                            </td>

                                        </tr>
                                    </table>
                                    <table border="0" style="width: 100%" class="table1">
                                        <tr>
                                            <td></td>
                                            <td>Item Receive Date From : </td>
                                            <td>
                                                <asp:TextBox ID="txtInvDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtInvDate" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td></td>
                                            <td>Item Receive Date To : </td>
                                            <td>
                                                <asp:TextBox ID="txtPODate" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtPODate" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>

                                            <td></td>
                                            <td style="text-align: center; vertical-align: middle;">
                                                <asp:Button ID="btnSearch" CssClass="btn btn-s-md btn-primary" runat="server" Text="SHOW" OnClick="btnSearch_OnClick" />
                                                <asp:Button ID="btnReset" CssClass="btn btn-default" runat="server" Text="Reset" OnClick="btnReset_OnClick" />
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
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="xtable-b"
                                            BorderStyle="Solid" BorderWidth="1px" CellPadding="7" ForeColor="Black" GridLines="Vertical"
                                            DataKeyNames="LCNo" AllowPaging="True" PageSize="25" OnPageIndexChanging="GridView1_OnPageIndexChanging"
                                            OnRowDataBound="GridView1_OnRowDataBound" RowStyle-CssClass="odd gradeX" ShowFooter="True">

                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="40px" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="LCNo" SortExpression="Name">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="Label1" runat="server" Text='<%# Bind("LCNo") %>' ></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ReceiveDate" HeaderText="Receive Date" SortExpression="address" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="Purpose" HeaderText="Purpose" />
                                                <asp:BoundField DataField="spec" HeaderText="spec" />
                                                <asp:BoundField DataField="qty" HeaderText="Quantity" SortExpression="phone" />
                                                <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" />
                                                <asp:BoundField DataField="TotalBDT" HeaderText="Total BDT" />
                                                <asp:BoundField DataField="UnitCostBDT" HeaderText="Unit Cost BDT" />
                                                <asp:BoundField DataField="UsedQty" HeaderText="Used Qty." />

                                            </Columns>
                                            <FooterStyle BackColor="#f3f3f3" Font-Bold="True" ForeColor="#23A6F0" BorderStyle="Solid" />
                                            <PagerStyle CssClass="gvpaging"></PagerStyle>
                                        </asp:GridView>
                                        <%--Total Rows Found: <asp:Literal ID="ltrtotal" runat="server"></asp:Literal>--%>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                        <%--End Body Contants--%>
                    </section>
                </div>





            </div>

            <%--<div class="grid_6">

                <table class="table7">
                    <tr>
                        <td>No. of Invoices: </td>
                        <td><asp:Literal ID="ltrtotal" runat="server" /></td>
                    </tr>
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

                </table>
            </div>--%>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="Button1" runat="server" Text="Export to Excel" OnClick="Button1_OnClick"  />

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

