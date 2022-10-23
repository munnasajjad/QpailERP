<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Item-Names.aspx.cs" Inherits="app_Item_Names" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false" ></asp:Label>	
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>


            
            <div class="row">
				<div class="col-md-12">					
					<h3 class="page-title">
					 Items Names <%--<small>form controls and more</small>--%>
					</h3>					
				</div>
			</div>
			<div class="row">


				<div class="col-md-6 ">
					<div class="portlet box red">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> <asp:Literal ID="ltrFrmName" runat="server" Text="Items Name Setup"></asp:Literal>
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
							<%--<form role="form">--%>
								<div class="form-body">
										
										<asp:Label ID="lblMsg" runat="server" EnableViewState="false" ></asp:Label>	
                                                                        
                                    <div id="EditField" runat="server">
										<label>Edit Info For: </label>
                                        <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" DataTextField="Name" DataValueField="Name" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [Name] FROM [Items] ORDER BY [Name]"></asp:SqlDataSource>
									</div>
                                    
                                    <div class="form-group">
										<label>Item Group: </label>
                                        <asp:DropDownList ID="ddGroup" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource21" DataTextField="GroupName" DataValueField="GroupName">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource21" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [GroupName] FROM [ItemGroup] ORDER BY [GroupName]"></asp:SqlDataSource>
									</div>
                                    
                                    <div class="form-group">
										<label>Item Sub-group: </label>
                                        <asp:DropDownList ID="ddSubGrp" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource22" DataTextField="CategoryName" DataValueField="CategoryName">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource22" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [CategoryName] FROM [ItemSubGroup] ORDER BY [CategoryName]"></asp:SqlDataSource>
									</div>

                                    <div class="form-group">
										<label>Grade/Sections/Category: </label>
                                        <asp:DropDownList ID="ddCategory" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource2" DataTextField="CategoryName" DataValueField="CategoryName">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [CategoryName] FROM [Categories] ORDER BY [CategoryName]"></asp:SqlDataSource>
									</div>
                                    
                                    <div class="form-group">
										<label>Unit Type: </label>
                                        <asp:DropDownList ID="ddUnit" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource5" DataTextField="UnitName" DataValueField="UnitName">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [UnitName] FROM [Units] ORDER BY [UnitName]"></asp:SqlDataSource>
									</div>


                                    <div class="form-group">
										<label>Item Name: </label>
										<asp:TextBox ID="txtName" runat="server" CssClass="form-control" EnableViewState="true" placeholder="Item Name" />									
									</div>

                                    <div class="form-group">
										<label>Item Name Description: </label>
										<asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" placeholder="Description" />											
									</div>

                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" onclick="btnSave_Click1" />
                                        <asp:Button ID="btnClear" CssClass="btn default" runat="server" Text="Cancel" onclick="btnClear_Click1" /> 
								    </div>

                                </div>
							<%--</form>  --%>                         
            
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
                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="name">
                    <Columns>
                        
                <asp:TemplateField ItemStyle-Width="40px">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                        <asp:BoundField DataField="GroupName" HeaderText="Group"  SortExpression="GroupName" ReadOnly="true" />
                        <asp:BoundField DataField="ItemType" HeaderText="Sub-group" SortExpression="ItemType" ReadOnly="true"  />
                        <asp:BoundField DataField="CategoryName" HeaderText="Grade/Category" SortExpression="CategoryName"  ReadOnly="true" />
                        
                        <asp:TemplateField HeaderText="Item Name" SortExpression="name">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UnitType" HeaderText="U.Type" SortExpression="UnitType" ReadOnly="true"  />
                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                    </Columns>
                </asp:GridView>


                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"                     
                    SelectCommand="SELECT GroupName, ItemType, CategoryName, Name, UnitType FROM [Items] ORDER BY GroupName, ItemType, CategoryName, Name"
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


