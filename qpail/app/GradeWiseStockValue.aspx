<%@ Page Title="Grade Wise Stock (Value)" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="GradeWiseStockValue.aspx.cs" Inherits="app_GradeWiseStockValue" %>


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


            <h3 class="page-title">Grade Wise Stock</h3>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

            
             
             <div class="row">


        <div class="col-md-12 ">
            <!-- BEGIN SAMPLE FORM PORTLET-->
            <div class="portlet box green ">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Grade Wise Stock
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
                            <asp:Label ID="Label2" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>
                            <div class="col-md-12">
                                <div class="row">

                                    <asp:Panel ID="pnlAdd" runat="server" DefaultButton="btnSearch">
                                       
                                          <div class="col-md-4">
                                            <div class="control-group ">
                                                <label class="col-sm-12 control-label">Group : </label>
                                           
                                                <asp:DropDownList ID="ddGroup" runat="server" DataSourceID="SqlDataSource1" AutoPostBack="true"
                                                    DataTextField="GroupName" DataValueField="GroupSrNo" CssClass="form-control">
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] WHERE GroupSrNo<>2 ORDER BY [GroupSrNo]"></asp:SqlDataSource>
                                               
                                        </div>
                                        </div>
                                        

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Category : </label>

                                                 <asp:DropDownList ID="ddSubGroup" runat="server" DataSourceID="SqlDataSource2" DataTextField="CategoryName" DataValueField="CategoryID"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddSubGroup_OnSelectedIndexChanged"  CssClass="form-control">

                                                    <%--<asp:ListItem>--- all ---</asp:ListItem>--%>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT CategoryID, CategoryName FROM [ItemSubGroup] WHERE (GroupID = @GroupID) ">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddGroup" Name="GroupID" PropertyName="SelectedValue" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>


                                        <div class="col-md-4">
                                            <div class="control-group ">
                                                <label class="col-sm-12 control-label">Grade : </label>
                                           <asp:DropDownList ID="ddGrade"  runat="server"
                                                AutoPostBack="True" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        </div>


                                      

                                      


                                        <div class="col-md-4" runat="server">
                                            <div class="control-group">
                                                <label class="control-label ">Date From: </label>
                                                
                                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </div>
                                        </div>


                                        <div class="col-md-4" runat="server">
                                            <div class="control-group">
                                                <label class="control-label ">Date To : </label>
                                               <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtDateTo" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="control-group">
                                                <div style="margin-top: 29px;margin-left: 80px">
                                                    <asp:Button ID="btnSearch"  style="width:70px" CssClass="btn btn-s-md btn-primary" runat="server" Text="Search" OnClick="btnShow_Click" />
                                                <asp:Button ID="btnReset" style="width:70px" CssClass="btn btn-s-md btn-danger" runat="server" Text="Reset" OnClick="btnReset_OnClick" />
                                                <%--<asp:Button ID="btnPrint" runat="server" Width="100px" Text="Print All" OnClick="btnPrint_OnClick" />--%>
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

             <iframe id="if1" runat="server" height="800px" width="100%"></iframe>
            <div class="col-lg-12">
                    <section class="panel">

                        <div id="Div1">
                            <div>

                                <fieldset>
                                    <%--<legend>Query Result </legend>--%>

                                    <div class="table-responsive">
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-hover" Visible="False"
                                            BorderStyle="Solid" BorderWidth="1px" CellPadding="7" ForeColor="Black" GridLines="Vertical" AllowPaging="True" PageSize="100" OnPageIndexChanging="GridView1_OnPageIndexChanging"
                                            OnRowDataBound="GridView1_OnRowDataBound" RowStyle-CssClass="odd gradeX" ShowFooter="True" AllowSorting="True">

                                            <Columns>

                                                <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>.
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="TrDate" HeaderText="Date" SortExpression="ProductName" DataFormatString="{0:d}" ItemStyle-Width="120px" />
                                                <asp:TemplateField HeaderText="Description" SortExpression="Name">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="Label1" runat="server" Text='<%# Bind("Description") %>' NavigateUrl='<%# Bind("link") %>' Target="_blank"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%--<asp:BoundField DataField="Description" HeaderText="Description" SortExpression="ProductName" />--%>
                                                <asp:BoundField DataField="Dr" HeaderText="Dr." ReadOnly="True" SortExpression="QtyBalance" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="Cr" HeaderText="Cr." SortExpression="ProductName" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="Balance" HeaderText="Balance" ReadOnly="True" SortExpression="QtyBalance" DataFormatString="{0:N2}" />
                                            </Columns>

                                            <FooterStyle BackColor="#f3f3f3" Font-Bold="True" ForeColor="#23A6F0" BorderStyle="Solid" />
                                            <PagerStyle CssClass="gvpaging"></PagerStyle>
                                            <RowStyle CssClass="odd gradeX" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT DISTINCT ProductName, SUM(QtyBalance) AS QtyBalance FROM OrderDetails WHERE (BrandID IN (SELECT BrandID FROM CustomerBrands WHERE (OrderDetails.QtyBalance &gt; 0) AND (CustomerID = @CustomerID))) GROUP BY ProductName">
                                            <SelectParameters>
                                             <%--   <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" />--%>
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <%-- Total Rows Found:--%>
                                        <asp:Literal ID="ltrtotal" runat="server"></asp:Literal>
                                    </div>
                                </fieldset>



                            </div>
                        </div>
                        <%--End Body Contants--%>
                    </section>
                </div>

        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>
