<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="Login-History.aspx.cs" Inherits="Oxford.app.Login_History" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodycontent" Runat="Server">

<div class="grid_12 full_block">
				<div class="widget_wrap">
					<div class="widget_top">
						<span class="h_icon list_image"></span>
						<h6>User Activities</h6>
					</div>
					<div class="widget_content">
						<h3>Login History</h3>
						
						
						<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                        </asp:ToolkitScriptManager>
						
						<table width="70%">
						<tr>
						<td width="50px"> </td>
						    <td>Date From:</td>
						    <td>
                                <asp:TextBox ID="txtDateFrom" runat="server"  title="Start Date"  tabindex="0" class="limiter tip_top" Width="170px" Height="23px" ></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtDateFrom"  Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td> </td>
						    <td>Date To: </td>
						    
						    <td>
                                <asp:TextBox ID="txtDateTo" runat="server" title="End Date"  tabindex="0" class="limiter tip_top" Width="170px" Height="23px" ></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtDateTo"  Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            
                            <td>                                
                                <asp:Button ID="btnShow" runat="server" Text="Show" onclick="btnShow_Click" />
                            </td>
                            
						</tr>
						<tr>
						<td width="20px"> &nbsp;</td>
						    <td>&nbsp;</td>
						    <td>
                                &nbsp;</td>
                            <td> &nbsp;</td>
						    <td>&nbsp;</td>
                            <td> &nbsp;</td>
                            <td>                                
                                &nbsp;</td>
                            
						</tr>
						</table>



					
                        <asp:GridView ID="GridView1" runat="server" class="display data_tbl" 
                            AutoGenerateColumns="True" GridLines="Horizontal">
                            
                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle/>
                            <EmptyDataTemplate>No Records Found.</EmptyDataTemplate>

                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"                                                      
                            
                            SelectCommand="SELECT [TrDate], [Description], [Dr], [Cr], [Balance] FROM [Transactions] WHERE ([HeadName] = @HeadName) and   TrDate >= @DateFrom AND TrDate <= @DateTo ORDER BY [TrID] DESC">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="Label1" Name="HeadName" PropertyName="Text"
                                    Type="String" />
                                <asp:ControlParameter ControlID="txtDateFrom" Name="DateFrom"  Type="DateTime" PropertyName="Text" />
                                <asp:ControlParameter ControlID="txtDateTo" Name="DateTo" Type="DateTime" PropertyName="Text" />
                            </SelectParameters>
                        </asp:SqlDataSource>
					
                        

					</div>
					
			</div>
                   <span style=" width:100%; display:block; text-align:center; background-color:Silver; font-weight:bold;"> Branch/Centre: <asp:Label ID="Label1" runat="server" Text=""></asp:Label> </span> 
    </div>
</asp:Content>

