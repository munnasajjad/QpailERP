<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Pending-Delivery.aspx.cs" Inherits="Cells_Pending_Delivery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="grid_12 full_block">
				<div class="widget_wrap">
					<div class="widget_top">
						<span class="h_icon list_image"></span>
						<h6>Pending Order List</h6>
					</div>
					<div class="widget_content">
						<h3>Cancel Order</h3>
						<p> </p>

					
                        <asp:GridView ID="GridView1" runat="server" class="display data_tbl" 
                            AutoGenerateColumns="False" DataSourceID="SqlDataSource1" GridLines="Horizontal">
                            <Columns>
                                <asp:BoundField DataField="InvNo" HeaderText="Inv.No." SortExpression="InvNo" />
                                <asp:BoundField DataField="TransactionType" HeaderText="Tr.Type" 
                                    SortExpression="TransactionType" />
                                <asp:BoundField DataField="PuchaseDate" HeaderText="Order Date" 
                                    SortExpression="PuchaseDate" />
                                <asp:BoundField DataField="PurchaseFrom" HeaderText="Sender" 
                                    SortExpression="PurchaseFrom" />
                                <asp:BoundField DataField="SenderMobile" HeaderText="S. Mobile" 
                                    SortExpression="SenderMobile" />
                                <asp:BoundField DataField="PurchaseTaka" HeaderText="Amount Sent" 
                                    SortExpression="PurchaseTaka" />
                                <asp:BoundField DataField="SalesTo" HeaderText="Receiver" 
                                    SortExpression="SalesTo" />
                                <asp:BoundField DataField="ReceiverMobile" HeaderText="R.Mobile" 
                                    SortExpression="ReceiverMobile" />
                                <asp:BoundField DataField="Description" HeaderText="Description" 
                                    SortExpression="Description" />
                                <asp:BoundField DataField="SenderBranchID" HeaderText="Sender Branch" 
                                    SortExpression="SenderBranchID" />
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle/>


                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                            SelectCommand="SELECT [InvNo], [PuchaseDate], [PurchaseFrom], [SenderMobile], [PurchaseTaka], [SalesTo], [ReceiverMobile], [Description], [SenderBranchID], [TransactionType] FROM [Orders] ORDER BY [TransactionType]"></asp:SqlDataSource>
					


					</div>
			</div>

    </div>
</asp:Content>

