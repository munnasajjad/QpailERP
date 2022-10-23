<%@ Page Title="Setup Brands" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Item-Brand.aspx.cs" Inherits="Operator_Item_Brand" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false" ></asp:Label>	

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            
            <div class="row">
				<div class="col-md-12">					
					<h3 class="page-title">
					 Items Brands
					</h3>					
				</div>
			</div>
			<div class="row">


				<div class="col-md-6 ">
					<div class="portlet box red">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> Items Brands Setup
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
										
										<asp:Label ID="lblMsg" runat="server" EnableViewState="false"  ></asp:Label>	
                                                                        
                                    <div id="EditField" runat="server">
										<label>Edit Info For: </label>
                                        <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" DataTextField="CategoryName" DataValueField="CategoryName" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [CategoryName] FROM [Categories] ORDER BY [CategoryName]"></asp:SqlDataSource>
									</div>
                                    
                                    <div class="form-group hidden">
										<label>Item Group: </label>
                                        <asp:DropDownList ID="ddCategory" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource2" DataTextField="GroupName" DataValueField="GroupName">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [GroupName] FROM [ItemGroup] ORDER BY [GroupName]"></asp:SqlDataSource>
									</div>


                                    <div class="form-group">
										<label>Brand Name: </label>
										<asp:TextBox ID="txtDept" runat="server" CssClass="form-control" EnableViewState="true" placeholder="Items Grade/Sections/Categories Name" />									
									</div>

                                    <div class="form-group">
										<label> Manufacturer: </label>
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
                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="CategoryName">
                    <Columns>
                                        
                <asp:TemplateField ItemStyle-Width="40px">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                        <asp:TemplateField HeaderText="Group Name" SortExpression="GroupName" Visible="false">                            
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("GroupName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Grade/Sections/Category" SortExpression="CategoryName">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("CategoryName") %>'></asp:Label>
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
                    SelectCommand="SELECT GroupName, [CategoryName], [Description] FROM [Categories] ORDER BY [CategoryName]"
                    UpdateCommand=""> 
                </asp:SqlDataSource>
            
            
                                </div>
               </div>

        </div>

    </div> 

                   
 </div>         

            
        </ContentTemplate>
    </asp:UpdatePanel>
    

</asp:Content>


