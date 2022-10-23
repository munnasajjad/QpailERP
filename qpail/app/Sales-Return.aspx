<%@ Page Title="Sales Return" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Sales-Return.aspx.cs" Inherits="Sales_Return" %>

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
    <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>




            <div class="row">
                <div class="col-md-4 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Sales Return
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
                                        <label class="control-label full-wdth">Return From :</label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource1" DataTextField="Company" DataValueField="PartyID"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                                <SelectParameters>
                                                    <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Invoice No. : </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddOrders" runat="server" DataSourceID="SqlDataSource6" DataTextField="InvNo" DataValueField="InvNo"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddOrders_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [InvNo] FROM [Sales] WHERE CustomerID=@CustomerID AND ([IsActive] =1) ORDER BY SaleID DESC">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
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
                                        <label class="control-label full-wdth">Return Date : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtReturnDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtReturnDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Product Returned :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddProduct" runat="server" DataSourceID="SqlDataSource8" DataTextField="ProductName" DataValueField="ID"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddProduct_OnSelectedIndexChanged" >
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [Id], [ProductName] FROM [SaleDetails] where InvNo=@InvNo ORDER BY [ProductName]">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddOrders" Name="InvNo" PropertyName="SelectedValue" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Return Quantity : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtQty">
                                                </asp:FilteredTextBoxExtender>
                                        </div>
                                    </div>


                                    <div class="control-group">
                                        <label class="control-label full-wdth">Wastage Stock :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddWasteStock" runat="server" DataSourceID="SqlDataSource7" DataTextField="StoreName" DataValueField="WID">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [WID], [StoreName] FROM [Warehouses] ORDER BY [StoreName]"></asp:SqlDataSource>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Replacement Stock :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddReplaceStock" runat="server" DataSourceID="SqlDataSource7" DataTextField="StoreName" DataValueField="WID">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

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
                                <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save Sales Return" OnClick="btnSave_Click" />
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
                                            HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" GridLines="Vertical" SelectedRowStyle-BackColor="LightBlue" DataSourceID="SqlDataSource2"
                                            >

                                            <Columns>
                                                
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" />
                                        </asp:TemplateField>

                                                <asp:BoundField DataField="ReturnDate" HeaderText="Return Date" SortExpression="ReturnDate">
                                                </asp:BoundField>

                                                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" SortExpression="CustomerName">
                                                </asp:BoundField>

                                                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No." SortExpression="InvoiceNo" />
                                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" SortExpression="ItemName" />
                                                <asp:BoundField DataField="Qty" HeaderText="Return Qty." SortExpression="Qty" />

                                            </Columns>
                                            <EmptyDataTemplate>
                                                <p>This sales order has no item returned!</p>
                                            </EmptyDataTemplate>
                                            <RowStyle BackColor="#F7F7DE" />
                                            <FooterStyle BackColor="#CCCC99" />
                                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                            <SelectedRowStyle BackColor="SkyBlue" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [ReturnDate], [CustomerName], [InvoiceNo], [ItemName], [Qty] FROM [SaleReturn] 
                                                            Where InvoiceNo=@InvoiceNo ORDER BY [eid] DESC">
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

