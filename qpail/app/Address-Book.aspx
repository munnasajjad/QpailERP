<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Address-Book.aspx.cs" Inherits="AdminCentral_Address_Book" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="grid_12 full_block">
				<div class="widget_wrap">
					<div class="widget_top">
						<span class="h_icon list_image"></span>
						<h6>Address Book</h6>
					</div>

						<h3>Search</h3>
						
						
						<asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
						
						<div class="col-md-12">

                        <div class="col-md-6">
                       <div class="control-group">
						    <label>Search By Contact Name:</label>
                                <asp:TextBox ID="txtDateFrom" runat="server"  title="Start Date"  tabindex="0" class="limiter tip_top" ></asp:TextBox>
                                </div>
                            </div>
                        <div class="col-md-6">
                       <div class="control-group">
						    <label>Search By Location: </label>
						    
                                <asp:TextBox ID="txtDateTo" runat="server" title="End Date"  tabindex="0" class="limiter tip_top" ></asp:TextBox>
                                </div>
                            </div>    
                         <div class="form-actions">                        
                                <asp:Button ID="btnShow" runat="server" CssClass="button" Text="Show" />
                                </div>
						</div>



					
                        <asp:GridView ID="GridView1" runat="server" class="display data_tbl" 
                            AutoGenerateColumns="False" GridLines="Horizontal" 
                            DataKeyNames="ContactName" DataSourceID="SqlDataSource1">
                            
                            <Columns>
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                                <asp:BoundField DataField="ContactName" HeaderText="ContactName" ReadOnly="True" 
                                    SortExpression="ContactName" />
                                <asp:BoundField DataField="District" HeaderText="District" 
                                    SortExpression="District" />
                                <asp:BoundField DataField="Address" HeaderText="Address" 
                                    SortExpression="Address" />
                                <asp:BoundField DataField="ContactNo" HeaderText="ContactNo" 
                                    SortExpression="ContactNo" />
                                <asp:BoundField DataField="CreditLimit" HeaderText="CreditLimit" 
                                    SortExpression="CreditLimit" />
                            </Columns>
                            
                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle/>
                            <EmptyDataTemplate>No Records Found.</EmptyDataTemplate>

                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"                                                      
                            
                            
                            SelectCommand="SELECT [Type], [ContactName], [District], [Address], [ContactNo], [CreditLimit] FROM [Contacts]">
                        </asp:SqlDataSource>
					
                        

					
			</div>
                   <span style=" width:100%; display:block; text-align:center; background-color:Silver; font-weight:bold;"> Contact/Centre: <asp:Label ID="Label1" runat="server" Text=""></asp:Label> </span> 
    </div>
</asp:Content>

