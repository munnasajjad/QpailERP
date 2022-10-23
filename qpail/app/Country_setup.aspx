<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Country_setup.aspx.cs" Inherits="app_Country_setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">
            <div class="row">
				<div class="col-md-12">					
					<h3 class="page-title">Country Setup</h3>					
				</div>
			</div>
			<div class="row">
				<div class="col-md-6 ">
					<div class="portlet box red">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> Add Country
							</div>
						</div>
						<div class="portlet-body form">
							
								<div class="form-body">
										
    
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="" ></asp:Label>
                    
                       
                       <div class="control-group">
                            <label class="control-label">Country Name: </label>
                            <div class="controls">                                
                                <asp:TextBox ID="txtDept" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                               
                            </div>
                        </div>
                       <div class="control-group">
                            <label class="control-label">Country Code: </label>
                            <div class="controls">                                
                                <asp:TextBox ID="txtCCode" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                               
                            </div>
                        </div>
                       <div class="control-group">
                            <label class="control-label">Country Nick: </label>
                            <div class="controls">                                
                                <asp:TextBox ID="TextBox1" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                               
                            </div>
                        </div>
                       <div class="control-group">
                            <label class="control-label">Dial Code: </label>
                            <div class="controls">                                
                                <asp:TextBox ID="TextBox2" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                               
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
								<i class="fa fa-reorder"></i> Saved Countries</div>
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
                        <asp:BoundField DataField="COUNTRY" HeaderText="COUNTRY Name" 
                            SortExpression="COUNTRY" />
                    </Columns>
                    <Columns>
                        <asp:BoundField DataField="NICK_NAME" HeaderText="Nick Name" 
                            SortExpression="NICK_NAME" />
                    </Columns>
                    <Columns>
                        <asp:BoundField DataField="DIAL_CODE" HeaderText="DIAL CODE" 
                            SortExpression="DIAL_CODE" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                    SelectCommand="SELECT [COUNTRY], [NICK_NAME], [DIAL_CODE] FROM [COUNTRY] ORDER BY [COUNTRY]"></asp:SqlDataSource>
            
            
							</div>
						</div>                        
               </div>
            </div>

                </div>

</asp:Content>

