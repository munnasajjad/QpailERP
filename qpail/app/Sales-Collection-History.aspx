<%@ Page Title="Sales Collection History" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Sales-Collection-History.aspx.cs" Inherits="app_Sales_Collection_History" %>

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
    text-align: center;
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


            <h3 class="page-title">Sales Collection History</h3>

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
                                            <th>Collection Date From</th>
                                            <th> </th>
                                           <th>Collection Date To</th>
                                            <td> </td> 
                                           <th> </th>
                                       </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddCustomer" runat="server" CssClass="form-control" DataSourceID="SqlDataSource3" 
                                                    DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="true">
                                                    <asp:ListItem>---ALL---</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'customer') ORDER BY [Company]"></asp:SqlDataSource>

                                            </td>
                                       
                                            <td> &nbsp; </td>
                                            <td>
                                                <asp:TextBox ID="txtInvDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtInvDate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                            </td>
                                      
                                            <td> &nbsp; </td>
                                            <td>
                                                <asp:TextBox ID="txtPODate" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtPODate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                            </td>
                                        
                                            <td></td>
                                            <td style="text-align: center;vertical-align: middle;">
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
                                    <legend>Sales Collection History/ Invoice Payment History </legend>
                                    
                                    <div class="table-responsive">
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="160%" CssClass="table-hover" 
                                            BorderStyle="Solid" BorderWidth="1px" CellPadding="7" ForeColor="Black" GridLines="Vertical"
                                            DataKeyNames="CID" AllowPaging="True" PageSize="25" OnPageIndexChanging="GridView1_OnPageIndexChanging" 
                                            OnRowDataBound="GridView1_OnRowDataBound"  RowStyle-CssClass="odd gradeX" ShowFooter="False">


                                            <Columns>

                                                <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="PaymentDate" HeaderText="Chq. Passing Date" SortExpression="email"  DataFormatString="{0:d}"/>
                                                <asp:BoundField DataField="CollectionNo" HeaderText="Collection ID" SortExpression="phone" />
                                                <asp:BoundField DataField="ChqNo" HeaderText="Chq.No." SortExpression="phone" />
                                                <asp:BoundField DataField="ChqDate" HeaderText="Chq.Date" SortExpression="phone"   DataFormatString="{0:d}"/>
                                                <asp:BoundField DataField="BankName" HeaderText="Bank Name" SortExpression="phone" />
                                                <asp:TemplateField HeaderText="Inv.No." SortExpression="Name">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="Label1" runat="server" Text='<%# Bind("InvNo") %>' NavigateUrl='<%# Bind("link") %>' Target="_blank"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="InvoiceDate" HeaderText="Inv.Date" SortExpression="address" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="MaturityDate" HeaderText="Maturity Date" SortExpression="email"  DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />

                                                <asp:BoundField DataField="OverdueDays" HeaderText="Cycle Days" SortExpression="email" />
                                                <asp:BoundField DataField="InvoiceTotal" HeaderText="Inv.Amount (Tk.)" SortExpression="Name" ReadOnly="True" />
                                                <asp:BoundField DataField="VATAmount" HeaderText="VAT Amt (Tk.)" SortExpression="address" />
                                                <asp:BoundField DataField="PayableAmount" HeaderText="Net Amount (Tk.)" SortExpression="phone" />
                                                
                                                <asp:BoundField DataField="TDSRate" HeaderText="TDS Rate %" SortExpression="email" />
                                                <asp:BoundField DataField="TDSAmount" HeaderText="TDS Amount" SortExpression="Name" ReadOnly="True" />
                                                <asp:BoundField DataField="VDSrate" HeaderText="VDS Rate %" SortExpression="address" />
                                                <asp:BoundField DataField="VDSAmount" HeaderText="VDS Amount" SortExpression="phone" />
                                                

                                                <asp:BoundField DataField="NetPayable" HeaderText="Net Payable (Tk.)" SortExpression="phone" />
                                                <asp:BoundField DataField="CollectedAmount" HeaderText="Collected (Tk.)" SortExpression="phone" />

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
            
            <div class="grid_6 hidden">

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
            </div>




        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="Button1" runat="server" Text="Export to Excel" OnClick="Button1_OnClick"  />

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

