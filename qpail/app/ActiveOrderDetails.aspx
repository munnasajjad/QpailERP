<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="ActiveOrderDetails.aspx.cs" Inherits="AdminCentral_ActiveOrderDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="grid_12 full_block">
				<div class="widget_wrap">
					<div class="widget_top">
						<span class="h_icon list_image"></span>
						<h6>Active Order List</h6>
					</div>
					<div class="widget_content">
						

<div style="width: 1032px;">
                        <asp:GridView ID="GridView1" runat="server" class="display data_tbl"   CssClass="table-responsive"
                            AutoGenerateColumns="False" DataSourceID="SqlDataSource1" GridLines="Horizontal" DataKeyNames="OrderID">
                            <Columns>
                                <asp:BoundField DataField="OrderID" HeaderText="Order ID" 
                                    SortExpression="OrderID" ReadOnly="True" />
                                <asp:BoundField DataField="OrderDate" HeaderText="Order Date" 
                                    SortExpression="OrderDate" />
                                <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" 
                                    SortExpression="DeliveryDate" />
                                <asp:BoundField DataField="Customer" HeaderText="Customer" 
                                    SortExpression="Customer" />
                                <asp:BoundField DataField="DeliveryAddress" HeaderText="Delivery Address" 
                                    SortExpression="DeliveryAddress" />
                                <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" 
                                    SortExpression="TotalAmount" />
                                <asp:BoundField DataField="Discount" HeaderText="Discount" 
                                    SortExpression="Discount" />
                                <asp:BoundField DataField="Vat" HeaderText="Vat" 
                                    SortExpression="Vat" />
                                <asp:BoundField DataField="PayableAmount" HeaderText="Total Amount" 
                                    SortExpression="PayableAmount" />
                                <asp:BoundField DataField="DeliveryStatus" HeaderText="DeliveryStatus" SortExpression="DeliveryStatus" />
                                <asp:BoundField DataField="EntryBy" HeaderText="EntryBy" 
                                    SortExpression="EntryBy" />
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle/>
                            <EmptyDataTemplate>No Records Found.</EmptyDataTemplate>

                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                            
                            
                            
                            
                            SelectCommand="SELECT [OrderID], [OrderDate], [DeliveryDate], (SELECT Company FROM Party WHERE (PartyID = Orders.CustomerName)) AS [Customer], [DeliveryAddress], [TotalAmount], [Discount], [Vat], [PayableAmount], [DeliveryStatus], [EntryBy] FROM [Orders] ORDER BY [OrderSl] DESC">
                        </asp:SqlDataSource>
					
                        

					</div>
					</div>
			</div>

    </div>
</asp:Content>

