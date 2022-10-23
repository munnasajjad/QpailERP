<%@ Page Title="Supplier Ledger" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Supplier-Ladger.aspx.cs" Inherits="app_Supplier_Ladger" %>

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


            <h3 class="page-title">Supplier Ledger</h3>

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
                                            <%--<th>Supplier Category</th>
                                            <th> </th>--%>
                                           <th>Supplier Name</th>
                                            <th> </th>
                                            <th>Order Date From</th>
                                            <th> </th>
                                           <th>Order Date To</th>
                                            <td> </td> 
                                           <th> </th>
                                       </tr>
                                        <tr>
                                           <%-- <td style="vertical-align: middle;">
                                              
                                                 <asp:DropDownList ID="ddSuppCategory" runat="server" AutoPostBack="True"
                                        DataSourceID="SqlDataSource9" DataTextField="BrandName" CssClass="select2me" 
                                        DataValueField="BrandID" OnSelectedIndexChanged="ddSuppCategory_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource9" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], BrandName FROM [RefItems] ORDER BY [BrandName]"></asp:SqlDataSource>
                              
                                            </td>
                                       
                                            <td> &nbsp; </td>--%>
                                            
                                            <td style="vertical-align: middle;">
                                               <asp:DropDownList ID="ddCustomer" runat="server" CssClass="select2me"  DataSourceID="SqlDataSource2z" DataTextField="Company"
                                        DataValueField="PartyID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2z" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'vendor')  ORDER BY [Company]">
                                       
                                    </asp:SqlDataSource>

                                            </td>
                                            <td> &nbsp; </td>
                                            <td>
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
                                        
                                            <td></td>
                                            <td style="text-align: center;vertical-align: middle;">
                                                <asp:Button ID="btnSearch" CssClass="btn btn-s-md btn-primary" runat="server" Text="Search" OnClick="btnShow_Click" />
                                                <asp:Button ID="btnReset" CssClass="btn btn-s-md btn-danger" runat="server" Text="Reset" OnClick="btnReset_OnClick" />
                                                <%--<asp:Button ID="btnPrint" CssClass="btn btn-s-md btn-default" runat="server" Text="Print" OnClick="btnPrint_Click" />--%>
                                                <%--<input id ="printbtn" type="button" class="btn btn-s-md btn-primary" value="PRINT" onclick="window.print();" >--%>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>


                            </div>
                        </div>

                    </section>
                </div>



                <iframe id="if1" runat="server" height="800px" width="100%" ></iframe>
                <div class="col-lg-12 hidden">
                    <section class="panel">

                        <div id="Div1">
                            <div>

                                <fieldset>
                                    <legend>Query Result </legend>
                                    
                                    <div class="table-responsive">
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-hover"  
                                            BorderStyle="Solid" BorderWidth="1px" CellPadding="7" ForeColor="Black" GridLines="Vertical" AllowPaging="True" PageSize="100" OnPageIndexChanging="GridView1_OnPageIndexChanging" 
                                            OnRowDataBound="GridView1_OnRowDataBound"  RowStyle-CssClass="odd gradeX" ShowFooter="True"  AllowSorting="True">
                                            
                                            <Columns>
                                                
                                            <asp:TemplateField ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                
                                                <asp:BoundField DataField="TrDate" HeaderText="Date" SortExpression="ProductName" DataFormatString="{0:d}" />
                                                <asp:TemplateField HeaderText="Description" SortExpression="Name">
                                                    <ItemTemplate>
                                                       <%-- <asp:HyperLink ID="Label1" runat="server" Text='<%# Bind("Description") %>' NavigateUrl='<%# Bind("link") %>' Target="_blank"></asp:HyperLink>--%>
                                                       <asp:Label ID="Label1" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%--<asp:BoundField DataField="Description" HeaderText="Description" SortExpression="ProductName" />--%>
                                                <asp:BoundField DataField="Dr" HeaderText="Dr." ReadOnly="True" SortExpression="QtyBalance" />
                                                <asp:BoundField DataField="Cr" HeaderText="Cr." SortExpression="ProductName" />
                                                <asp:BoundField DataField="Balance" HeaderText="Balance" ReadOnly="True" SortExpression="QtyBalance" />
                                            </Columns>
                                            
                                            <FooterStyle BackColor="#f3f3f3" Font-Bold="True" ForeColor="#23A6F0" BorderStyle="Solid" />
                                            <PagerStyle CssClass="gvpaging"></PagerStyle>
                                            <RowStyle CssClass="odd gradeX" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT DISTINCT ProductName, SUM(QtyBalance) AS QtyBalance FROM OrderDetails WHERE (BrandID IN (SELECT BrandID FROM CustomerBrands WHERE (OrderDetails.QtyBalance &gt; 0) AND (CustomerID = @CustomerID))) GROUP BY ProductName">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <%--Total Rows Found: --%><asp:Literal ID="ltrtotal" runat="server"></asp:Literal>
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

