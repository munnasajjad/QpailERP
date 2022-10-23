<%@ Page Title="Purchase Return" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Purchase-Return.aspx.cs" Inherits="app_Purchase_Return" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

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

        #ctl00_BodyContent_txtAmount, #ctl00_BodyContent_txtRate, #ctl00_BodyContent_txtQty, #ctl00_BodyContent_txtStock {
            text-align: center;
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
                <div class="col-md-12">
                    <h3 class="page-title">Purchase Return
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Purchase Return Entry
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                            <div class="form-horizontal" role="form">

                                <div class="form-body">

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Return Activies :</label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddMode" runat="server"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddMode_OnSelectedIndexChanged">
                                                <asp:ListItem Value="1">Item Sent to Supplier</asp:ListItem>
                                                <asp:ListItem Value="2">Sent Item Replaced</asp:ListItem>
                                                <asp:ListItem Value="3">Money Back (Balance Adjustment)</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <%--<div id="Div2" runat="server" class="control-group">
                                    <asp:Label ID="Label14" runat="server" Text="Supplier Item Category :  "></asp:Label>
                                    <asp:DropDownList ID="ddSuppCategory" runat="server" AutoPostBack="True"
                                        DataSourceID="SqlDataSource9" DataTextField="BrandName"
                                        DataValueField="BrandID" OnSelectedIndexChanged="ddSuppCategory_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource9" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], BrandName FROM [RefItems] ORDER BY [BrandName]"></asp:SqlDataSource>
                                </div>--%>

                                    <div class="form-group">
                                        <label style="width: 100%">Purchase From:</label>
                                        <asp:Label ID="vBalance" CssClass="help-block" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddVendor" runat="server" DataSourceID="SqlDataSource2z" DataTextField="Company"
                                            CssClass="select2me" Width="100%" DataValueField="PartyID">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2z" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type)  ORDER BY [Company]">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="vendor" Name="Type" Type="String" />
                                                <%--<asp:ControlParameter ControlID="ddSuppCategory" Name="Category" PropertyName="SelectedValue" />--%>
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>


                                    <div class="control-group">
                                        <label class="control-label full-wdth">Invoive No. : </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddOrders" runat="server"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddOrders_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span style="width: 70%; color: green; display: none;">
                                                <asp:Literal ID="ltrBillNo" runat="server" EnableViewState="False">Recent Purchase Info: </asp:Literal>
                                            </span>
                                            <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [InvNo] FROM [Purchase] WHERE SupplierID=@SupplierID ORDER BY PID DESC">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddVendor" Name="SupplierID" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Invoice Date : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY" ReadOnly="true"></asp:TextBox>
                                            <%-- <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtDate">
                                            </asp:CalendarExtender>--%>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Challan No. : </label>
                                        <asp:TextBox ID="txtChallanNo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Challan Date : </label>
                                        <asp:TextBox ID="txtChallanDate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        <%--<asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtChallanDate">
                                    </asp:CalendarExtender>--%>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">
                                            <asp:Literal ID="ltrDate" runat="server" Text="Send Date : "></asp:Literal></label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtReturnDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtReturnDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Product :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddProduct" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddProduct_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [Id], [ItemName]+' '+(SELECT [spec] FROM [Specifications] where [id]=PurchaseDetails.SizeRef) as ItemName FROM [PurchaseDetails] WHERE InvNo=@InvNo ORDER BY [ItemName]">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddOrders" Name="InvNo" PropertyName="SelectedValue" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>

                                            <span style="width: 90%; color: green; text-align: center;">
                                                <asp:Literal ID="txtPurchasedQty" runat="server" EnableViewState="False">Recent Purchase Info: </asp:Literal>
                                            </span>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">
                                            <asp:Literal ID="ltrQty" runat="server" Text="Return Quantity : "></asp:Literal></label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtQty">
                                            </asp:FilteredTextBoxExtender>
                                        </div>
                                    </div>

                                    <asp:Panel ID="PnlAmount" runat="server" Visible="False">

                                        <div class="control-group">
                                            <label class="control-label full-wdth">
                                                <asp:Literal ID="ltrAmt" runat="server" Text="Adjust Amount : "></asp:Literal></label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FiltereddfgdfgTextBoxExtender6"
                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtAmount">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>

                                    </asp:Panel>


                                    <asp:Panel ID="pnlStock" runat="server" Visible="False">

                                        <div class="control-group">
                                            <label class="control-label full-wdth">
                                                <asp:Literal ID="ltrStock" runat="server" Text="Stock Out From : "></asp:Literal>
                                            </label>
                                            <div class="controls">
                                                <asp:DropDownList ID="ddGodown" runat="server" DataSourceID="SqlDataSource7" DataTextField="StoreName" DataValueField="WID">
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [WID], [StoreName] FROM [Warehouses] ORDER BY [StoreName]"></asp:SqlDataSource>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label full-wdth">From / Location : </label>
                                            <asp:DropDownList ID="ddLocation" runat="server" DataSourceID="SqlDataSource15" DataTextField="AreaName" DataValueField="AreaID" CssClass="form-control">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource15" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT AreaID, [AreaName] FROM [WareHouseAreas] WHERE Warehouse=@Warehouse">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddGodown" Name="Warehouse" PropertyName="SelectedValue" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                    </asp:Panel>

                                    <%--<div class="control-group">
                                        <label class="control-label full-wdth">Replacement Stock :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddReplaceStock" runat="server" DataSourceID="SqlDataSource7" DataTextField="StoreName" DataValueField="WID">
                                            </asp:DropDownList>
                                        </div>
                                    </div>--%>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Remarks/ Return Note : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                        </div>
                                    </div>


                                    <hr />

                                    <div class="form-group hidden">
                                        <label class="col-sm-6 control-label">Total Amount: </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtTotal" runat="server" CssClass="form-control" placeholder="Total Amount" Text="0" />
                                        </div>
                                    </div>

                                    <div class="form-group hidden">
                                        <label class="col-sm-6 control-label">Discount : </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control" placeholder="(-)" onkeyup="calInvTotal()" Text="0" />
                                        </div>
                                    </div>

                                    <div class="form-group hidden">
                                        <label class="col-sm-6 control-label">VAT (%) : </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtVat" runat="server" CssClass="form-control" placeholder="(%)" onkeyup="calInvTotal()" Text="0" />
                                        </div>
                                    </div>

                                    <div class="form-group hidden">
                                        <label class="col-sm-6 control-label">Payable : </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtPay" runat="server" CssClass="form-control" placeholder="Payable"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--<div style="height:120px;">&nbsp;</div>--%>
                                </div>
                            </div>

                            <div class="form-actions">
                                <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click" />
                                <%--<asp:Button ID="btnEdit" CssClass="btn red" runat="server" Text="Edit" />
                        <asp:Button ID="btnPrint" CssClass="btn purple" runat="server" Text="Print" />--%>
                                <%--<asp:Button ID="btnCancel" CssClass="btn default" runat="server" Text="Cancel Order" OnClick="btnCancel_Click" />--%>
                            </div>

                        </div>
                    </div>
                </div>


                <div class="col-md-8 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box green ">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Return History for the Invoice
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

                                    <asp:LoginName ID="LoginName1" runat="server" Visible="false" />
                                    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>


                                    <div class="form-group">
                                        <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>
                                    </div>


                                    <div class="table-responsive">


                                        <asp:GridView ID="ItemGrid" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered"
                                            BackColor="Red" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="1" ForeColor="Black" RowStyle-BackColor="#A1DCF2"
                                            HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" GridLines="Vertical" SelectedRowStyle-BackColor="LightBlue" DataSourceID="SqlDataSource2">

                                            <Columns>

                                                <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="ReturnDate" HeaderText="Return Date" SortExpression="ReturnDate" DataFormatString="{0:d}"></asp:BoundField>

                                                <asp:BoundField DataField="Grade" HeaderText="Item Grade" SortExpression="SupplierName"></asp:BoundField>

                                                <asp:BoundField DataField="Category" HeaderText="Item Category" SortExpression="InvoiceNo" />
                                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" SortExpression="ItemName" />
                                                <asp:BoundField DataField="Qty" HeaderText="Qty.(Send/Receive)" SortExpression="Qty" />

                                            </Columns>
                                            <EmptyDataTemplate>
                                                <p>This purchase order has no item returned!</p>
                                            </EmptyDataTemplate>
                                            <RowStyle BackColor="#F7F7DE" />
                                            <FooterStyle BackColor="#CCCC99" />
                                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                            <SelectedRowStyle BackColor="SkyBlue" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [ReturnDate], [SupplierName], 
                                            (SELECT GradeName FROM [ItemGrade] where GradeID=(SELECT GradeID FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=PurchaseReturn.ItemCode))) As Grade, 
                                                (SELECT CategoryName FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=PurchaseReturn.ItemCode)) As Category, 
                                            [InvoiceNo], [ItemName], [Qty] FROM [PurchaseReturn] Where InvoiceNo=@InvoiceNo ORDER BY [eid] DESC">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddOrders" Name="InvoiceNo" PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
