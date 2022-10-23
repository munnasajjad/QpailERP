<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Setup_City.aspx.cs" Inherits="app_Setup_City" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">
            <div class="row">
				<div class="col-md-12">					
					<h3 class="page-title">City Setup</h3>					
				</div>
			</div>
			<div class="row">
				<div class="col-md-6 ">
					<div class="portlet box red">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> Add City
							</div>
						</div>
						<div class="portlet-body form">
							
								<div class="form-body">
										
    
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="" ></asp:Label>
                    
                       
                       <div class="control-group">
                            <label class="control-label">City Name: </label>
                            <div class="controls">                                
                                <asp:TextBox ID="txtDept" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                               
                            </div>
                        </div>
                       <div class="control-group">
                            <label class="control-label">Country: </label>
                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                <asp:DropDownList ID="ddName" runat="server" AppendDataBoundItems="True" 
                                    AutoPostBack="True" DataSourceID="SqlDataSource2"  Class=""  
                                    DataTextField="COUNTRY" DataValueField="COUNTRY" >
                                    <asp:ListItem>---Select---</asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                    SelectCommand="SELECT [COUNTRY] FROM [Country] ORDER BY [COUNTRY]"></asp:SqlDataSource>
                        </div>
                       <div class="control-group">
                            <label class="control-label">City Nick: </label>
                            <div class="controls">                                
                                <asp:TextBox ID="TextBox1" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                               
                            </div>
                        </div>
                        
  
  
                         <div class="form-actions">
                            <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save Department" OnClick="btnSave_Click" />
                             <%--<asp:Button ID="btnDelete" runat="server" Text="Delete This Schedule"  CssClass="btn red" 
                                onclick="btnDelete_Click" />
                                <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnDelete"  ConfirmText="confirm to delete this schedule ??"> 
                                 </asp:ConfirmButtonExtender> --%>       
                            <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" OnClick="btnClear_Click" />                              
                         </div>
                        
            
							</div>
						</div>                        
               </div>
            </div>

                         
                
                
                
				<div class="col-md-6 ">
					<div class="portlet box green">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> Saved Cities</div>
							<div class="tools">
								<a href="" class="collapse">
								</a>
								<a href="#portlet-config" data-toggle="modal" class="config">
								</a>
								<a href="" class="reload">
								</a>
								<a href="" class="remove">
								</a>
							</div>
						</div>
						<div class="portlet-body form">
							
								<div class="form-body">

   
  
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    DataSourceID="SqlDataSource1" Width="253px">
                    <Columns>
                        <asp:BoundField DataField="CITY" HeaderText="City Name" 
                            SortExpression="CITY" />
                    </Columns>
                    <Columns>
                        <asp:BoundField DataField="NICK_NAME" HeaderText="Nick Name" 
                            SortExpression="NICK_NAME" />
                    </Columns>
                    <Columns>
                        <asp:BoundField DataField="COUNTRY" HeaderText="COUNTRY" 
                            SortExpression="COUNTRY" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                    SelectCommand="SELECT [CITY], [NICK_NAME], [COUNTRY] FROM [CITY] ORDER BY [CITY]"></asp:SqlDataSource>
            
            
							</div>
						</div>                        
               </div>
            </div>

                </div>

</asp:Content>

