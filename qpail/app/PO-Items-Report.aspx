﻿<%@ Page Title="PO Items" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="PO-Items-Report.aspx.cs" Inherits="app_PO_Items_Report" %>

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


            <h3 class="page-title">PO Items</h3>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">

                <div class="col-lg-12">
                    <section class="panel">
                        <%--Body Contants--%>
                        <div id="Div2">
                            <div>

                                <fieldset>
                                    <%--<legend>Search Terms</legend>--%>
                                    <table border="0" width="100%" style="width:100%" class="table1">
                                       <tr>
                                            <th>Customer</th>
                                            <th> </th>
                                           <th>PO Status</th>
                                            <th> </th>
                                            <th>PO No. & Date</th>
                                            <th> </th>
                                           <%--<th>Order Date To</th>
                                            <th> </th> --%>
                                           <th> </th>
                                       </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddCustomer" runat="server" CssClass="form-control" DataSourceID="SqlDataSource3" 
                                                    DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddCustomer_OnSelectedIndexChanged">
                                                    <%--<asp:ListItem>---ALL---</asp:ListItem>--%>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'customer') ORDER BY [Company]"></asp:SqlDataSource>

                                            </td>
                                       
                                            <td> &nbsp; </td>
                                            
                                            <td>
                                                <asp:DropDownList ID="ddStatus" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddStatus_OnSelectedIndexChanged">
                                                    <asp:ListItem Value="P">Pending PO</asp:ListItem>
                                                    <asp:ListItem Value="A">Partially Delivered PO</asp:ListItem>
                                                    <asp:ListItem Value="D">Delivered PO</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td> &nbsp; </td>
                                            
                                            <td>
                                                <asp:DropDownList ID="ddPo" runat="server" CssClass="form-control" DataSourceID="SqlDataSource1" 
                                                    DataTextField="podt" DataValueField="PoNo" AutoPostBack="True" OnSelectedIndexChanged="ddPo_OnSelectedIndexChanged" >
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [PoNo], [PoNo]+' Date:'+(CONVERT(varchar,OrderDate,103) +'') AS podt FROM [Orders] WHERE ([CustomerName] = @CustomerName) AND DeliveryStatus=@DeliveryStatus ORDER BY [OrderDate] DESC">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddCustomer" Name="CustomerName" PropertyName="SelectedValue" />
                                                        <asp:ControlParameter ControlID="ddStatus" Name="DeliveryStatus" PropertyName="SelectedValue" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                            </td>
                                            <td> &nbsp; </td>
                                            <%--<td>
                                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                            </td>
                                      
                                            <td> &nbsp; </td>
                                            <td>
                                                <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtDateTo" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                            </td>
                                        
                                            <td></td>--%>
                                            <td style="text-align: center;vertical-align: middle;">
                                                <asp:Button ID="btnSearch" CssClass="btn btn-s-md btn-primary" runat="server" Text="Show" OnClick="btnSearch_OnClick" />
                                                <asp:Button ID="btnReset" CssClass="btn btn-s-md btn-danger" runat="server" Text="Preview" OnClick="btnReset_OnClick" />
                                                <%--<asp:Button ID="btnPrint" CssClass="btn btn-s-md btn-default" runat="server" Text="Print" OnClick="btnPrint_Click" />--%>
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
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"  CssClass="table table-hover" 
                                            BorderStyle="Solid" BorderWidth="1px" CellPadding="7" ForeColor="Black" GridLines="Vertical"
                                             AllowPaging="True" PageSize="25" OnPageIndexChanging="GridView1_OnPageIndexChanging" 
                                            OnRowDataBound="GridView1_OnRowDataBound"  RowStyle-CssClass="odd gradeX" ShowFooter="True">


                                            <Columns>

                                                <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>


                                                <%--<asp:TemplateField HeaderText="Inv.No" SortExpression="Name">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="Label1" runat="server" Text='<%# Bind("InvNo") %>' NavigateUrl='<%# Bind("link") %>' Target="_blank"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="address"  />
                                                <asp:BoundField DataField="UnitCost" HeaderText="Unit Cost" SortExpression="phone" />
                                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="email" />
                                                <asp:BoundField DataField="DeliveredQty" HeaderText="Delivered Qty." SortExpression="email"  />

                                                <asp:BoundField DataField="QtyBalance" HeaderText="Qty. Balance" SortExpression="Name"/>
                                                <asp:BoundField DataField="DeliveryInvoice" HeaderText="DeliveryInvoice" SortExpression="Name"  />
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
            
            <div class="grid_6">

                <table class="table7">
                    <tr>
                        <td>Total Quantity: </td>
                        <td><asp:Literal ID="ltrtotal" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Total Delivered: </td>
                        <td><asp:Literal ID="ltrQty" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Total Balance: </td>
                        <td><asp:Literal ID="ltrItemLoad" runat="server" /></td>
                    </tr>
                    <%--<tr>
                        <td>Grand Total VAT (Tk.): </td>
                        <td><asp:Literal ID="ltrTotalVat" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Grand Total Amount (Tk.): </td>
                        <td><asp:Literal ID="ltrGTAmt" runat="server" /></td>
                    </tr>--%>
                    
                </table>
            </div>




        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="Button1" runat="server" Text="Export to Excel" OnClick="Button1_OnClick"  />

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

