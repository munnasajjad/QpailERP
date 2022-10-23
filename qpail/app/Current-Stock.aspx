<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Current-Stock.aspx.cs" Inherits="app_Current_Stock" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div class="container-fluid">
            <div class="row-fluid">
               <div class="span12"> 
                  <h3 class="page-title">
                   Current Stock
                     <%--<small>form components and widgets</small>--%>
                  </h3>
               </div>
            </div>
            <div class="row-fluid">
               <div class="span12">
                  <div class="portlet box green">
							<div class="portlet-title">
								<h4><i class="icon-bar-chart"></i> Warehouse Items List</h4>
								<div class="tools">
									<a href="javascript:;" class="collapse"></a>
									<a href="#portlet-config" data-toggle="modal" class="config"></a>
									<a href="javascript:;" class="reload"></a>
									<a href="javascript:;" class="remove"></a>
								</div>
							</div>
							<div class="portlet-body">
                       
    
    <div class="controls"> Warehouse:                               
                                <asp:DropDownList ID="DropDownList2" Width="250px" 
            runat="server" CssClass="span6 m-wrap" DataSourceID="SqlDataSource3" PlaceHolder="Coach & Warehouse"
            DataTextField="Warehouse" DataValueField="Warehouse" AutoPostBack="True" 
            onselectedindexchanged="DropDownList2_SelectedIndexChanged">
                                        </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT DISTINCT [Warehouse] FROM [Stock] WHERE OutQuantity<1 ORDER BY [Warehouse]"></asp:SqlDataSource>
                               
    
    <%--&nbsp; &nbsp; &nbsp;
        <asp:Label ID="lblDate" runat="server" Text="Date From: "></asp:Label>            
           
                <asp:TextBox ID="txtDateF" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
                    Enabled="True" TargetControlID="txtDateF"  Format="dd/MM/yyyy">
                </asp:CalendarExtender>
    &nbsp; &nbsp; &nbsp; 
    <asp:Label ID="Label1" runat="server" Text="Date To: "></asp:Label>            
           
                <asp:TextBox ID="txtDateT" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
                    Enabled="True" TargetControlID="txtDateT"  Format="dd/MM/yyyy">
                </asp:CalendarExtender>
    &nbsp; &nbsp; &nbsp; --%>
                
                
    <asp:Button ID="btnLoad" runat="server" Text="Show" CssClass="btn blue"  OnClick="btnLoad_Click" />
</div>
    
        <asp:GridView ID="GridView1" runat="server"
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
        BorderWidth="1px" Width="100%" AutoGenerateColumns="False"
        CellPadding="3" onrowdatabound="GridView1_RowDataBound" >
            <RowStyle ForeColor="#000066" />
            <Columns>
            
            <asp:TemplateField>
                <ItemTemplate>
                     <%#Container.DataItemIndex+1 %>
                </ItemTemplate>
            </asp:TemplateField>
            
                    <asp:BoundField DataField="PartName" HeaderText="Part Name" ReadOnly="True" SortExpression="PartName" />
                    <asp:BoundField DataField="Warehouse" HeaderText="Warehouse" ReadOnly="True" SortExpression="Warehouse" />
                    <asp:BoundField DataField="ItemSerialNo" HeaderText="Item Serial No" ReadOnly="True" SortExpression="ItemSerialNo" />
                    <asp:BoundField DataField="Warrenty" HeaderText="Warrenty" ReadOnly="True" SortExpression="Warrenty" />
                    <asp:BoundField DataField="Status" HeaderText="Status" ReadOnly="True" SortExpression="Status" />                    
                    
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <EmptyDataTemplate>
                No Data Found!
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
            
            
        
        SelectCommand="SELECT [InvNo], [HeadName], [Description], [Dr], [Cr], [TrDate] FROM [Transactions] ORDER BY [TrID] DESC">
        </asp:SqlDataSource>        
    
						</div>
               </div>
               
               
     <%--<div style=" position:relative; float:right; margin:20px 0px 40px auto; width:600px; font-weight:bold; color:#006699; height:200px;">
            <div class="row-fluid">
					<div class="span6">
						<!-- BEGIN SAMPLE TABLE PORTLET-->
						<div class="portlet box red">
							<div class="portlet-title">
								<h4><i class="icon-cogs"></i>Report Summery</h4>								
							</div>
							<div class="portlet-body">
								<table class="table table-hover">									
									<tbody>
										<tr><td>Total Item Qty : </td><td>
                    <asp:Label ID="Label2" runat="server" Text="" EnableViewState="false"></asp:Label></td> </tr>    
                <tr><td> Item Value : </td><td>
                    <asp:Label ID="Label3" runat="server" Text="" EnableViewState="false"></asp:Label></td> </tr>    
                               
									</tbody>
								</table>
							</div>
						</div>
						<!-- END SAMPLE TABLE PORTLET-->
					</div>

           
           
        </div>
        
    
						</div>--%>
               
                 <%--
                <div class="portlet box yellow">
							<div class="portlet-title">
								<h4><i class="icon-bar-chart"></i> Schedule Status</h4>
								<div class="tools">
									<a href="javascript:;" class="collapse"></a>
									<a href="#portlet-config" data-toggle="modal" class="config"></a>
									<a href="javascript:;" class="reload"></a>
									<a href="javascript:;" class="remove"></a>
								</div>
							</div>
							<div class="portlet-body">

   
    
      <asp:GridView ID="GridView2" runat="server"
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
        BorderWidth="1px" Width="100%"
        CellPadding="3" >
            <RowStyle ForeColor="#000066" />
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <EmptyDataTemplate>
                You Dont Have any Transaction!
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
            
            
        
        SelectCommand="SELECT [InvNo], [HeadName], [Description], [Dr], [Cr], [TrDate] FROM [Transactions] ORDER BY [TrID] DESC">
        </asp:SqlDataSource>
        
    
						</div>
               </div>
               
            </div>
    </div>

    
</div>

--%>

                   </div></div></div>

</asp:Content>

