<%@ Page Title="General Item Type" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Ref-Items.aspx.cs" Inherits="app_Ref_Items" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false" ></asp:Label>	

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            
            <div class="row">
				<div class="col-md-12">					
					<h3 class="page-title">
					    General Item Type
					</h3>					
				</div>
			</div>
			<div class="row">


				<div class="col-md-6 ">
					<div class="portlet box red">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i>General Item Type
							</div>
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
										
										<asp:Label ID="lblMsg" runat="server" EnableViewState="false" ></asp:Label>	
                                                                        
                                    <div id="EditField" runat="server">
										<label>Edit Info For: </label>
                                        <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" 
                                            DataTextField="BrandName" DataValueField="BrandID" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT BrandID, [BrandName] FROM [RefItems]"></asp:SqlDataSource>
									</div>
                                    
                                    
                                    <div class="form-group">
										<label>Category Name: </label>
										<asp:TextBox ID="txtDept" runat="server" CssClass="form-control" EnableViewState="true" placeholder="Category Name" />									
									</div>

                                    <div class="form-group">
										<label>Description: </label>
										<asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" placeholder="Description" />											
									</div>

                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" onclick="btnSave_Click1" />
                                        <asp:Button ID="btnClear" CssClass="btn default" runat="server" Text="Cancel" onclick="btnClear_Click1" /> 
								    </div>

                                </div>
							
            
                   </div>
                </div>
            </div> 
                                                      
                
				<div class="col-md-6 ">
					<div class="portlet box green">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> Saved Data
							</div>
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

  
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="BrandName">
                    <Columns>                
                <asp:TemplateField ItemStyle-Width="20px">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                    </asp:TemplateField>

                        <asp:TemplateField HeaderText="Category" SortExpression="BrandID" Visible="false">                            
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("BrandID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Category Name" SortExpression="BrandName">
                            <ItemTemplate>
                                <asp:Label ID="Label1x" runat="server" Text='<%# Bind("BrandName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" SortExpression="Description">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                    </Columns>
                </asp:GridView>


                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"                     
                    SelectCommand="SELECT BrandID,  [BrandName], [Description] FROM [RefItems] a  ORDER BY [BrandName]"> 
                                                           
                </asp:SqlDataSource>
            
            
                                </div>
               </div>

        </div>

    </div> 

                   
 </div>         

            
        </ContentTemplate>
    </asp:UpdatePanel>
    

</asp:Content>
