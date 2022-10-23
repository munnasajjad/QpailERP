<%@ Page Title="Item Setup" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Item-Setup.aspx.cs" Inherits="Operator_Item_Setup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {

            $("input[type=text][id*=txtCode]").attr("disabled", true);

            $("input[type=checkbox][id*=chkCode]").click(function () {
                if (this.checked)
                    $("input[type=text][id*=txtCode]").attr("disabled", true);
                else
                    $("input[type=text][id*=txtCode]").attr("disabled", false);
            });                
        });
    </script>

    <style type="text/css">
        label{font-weight:bold; color:chocolate;}
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false" ></asp:Label>	

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <%--<asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
            
            <div class="row">
				<div class="col-md-12">					
					<h3 class="page-title">
					    Items Setup <%--<small>form controls and more</small>--%>
					</h3>					
				</div>
			</div>
			<div class="row">


				<div class="col-md-6 ">
					<div class="portlet box red">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> Items Setup
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
										<asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="help-block" ></asp:Label>	
                                        
                                    
                                    <div id="EditField" runat="server">
										<label>Edit Item Info for: </label>
                                        <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource6" DataTextField="name" DataValueField="ItemCode" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [ItemCode], [name] FROM [Items] WHERE ([ProjectID] = @ProjectID) ORDER BY [name]">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
									</div>
                                    

                                    <div class="form-group col-md-12">
										<label>Item Type: </label>
                                        <asp:DropDownList ID="ddType" CssClass="form-control select2me" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddType_SelectedIndexChanged">
                                            <asp:ListItem>Finished Goods</asp:ListItem>
                                            <asp:ListItem>Raw Materials</asp:ListItem>
                                            <asp:ListItem>Trading Items</asp:ListItem>
                                            <asp:ListItem>Semi Finished Items</asp:ListItem>
                                        </asp:DropDownList>                                        
									</div>


                                    <div class="form-group col-md-6">
										<label>Item Group: </label>
                                        <asp:DropDownList ID="ddGroup" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource2" DataTextField="GroupName" DataValueField="GroupName" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [GroupName] FROM [ItemGroup] ORDER BY [GroupName]"></asp:SqlDataSource>
									</div>

                                    
                                    <div class="form-group col-md-6">
										<label>Category Name: </label>
                                        <asp:DropDownList ID="ddCategory" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" DataTextField="CategoryName" DataValueField="CategoryName">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [CategoryName] FROM [Categories] WHERE ([GroupName] = @GroupName) ORDER BY [CategoryName]">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddGroup" Name="GroupName" PropertyName="SelectedValue" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
									</div>

                                    
                                    <div class="form-group col-md-6">
										<label>Brand Name: </label>
                                        <asp:DropDownList ID="ddBrand" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource4" DataTextField="BrandName" DataValueField="BrandName">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [BrandName] FROM [Brands] ORDER BY [BrandName]"></asp:SqlDataSource>
									</div>

                                    
                                    <div class="form-group col-md-6">
										<label>Unit Type: </label>
                                        <asp:DropDownList ID="ddUnit" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource5" DataTextField="UnitName" DataValueField="UnitName">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [UnitName] FROM [Units] ORDER BY [UnitName]"></asp:SqlDataSource>
									</div>

                                    
                                    <div class="form-group  col-md-12">
                                        
										    <label>Item Code: </label>
									        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control iCode" EnableViewState="true" placeholder="Menu Item Code" />	
                                                                               
                                            <asp:CheckBox ID="chkCode" runat="server" Text="Auto generated" Checked="true" />								
                                        
									</div>

                                    <div class="form-group col-md-6">
										<label>Item Name: </label>
										<asp:TextBox ID="txtName" runat="server" CssClass="form-control" EnableViewState="true" placeholder="Menu Item Name" />									
									</div>

                                    <div class="form-group col-md-6">
										<label>Price : </label>
										<asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" placeholder="Price" />											
									</div>
                                                                        
                                    <div class="form-group col-md-6">
										<label>Status : </label>
                                        <asp:RadioButton ID="rdYes" runat="server" Text="Active" GroupName="onsale" Checked="true" />
                                        <asp:RadioButton ID="rdNo" runat="server" Text="Inactive" GroupName="onsale" />
									</div>
                                    
                                    <div class="form-group">
										<label> &nbsp; </label>                  
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
								<i class="fa fa-reorder"></i> <asp:Literal ID="lblType" runat="server"></asp:Literal>
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

  

  <%--<asp:UpdatePanel ID="grdPanel" runat="server" UpdateMode="Conditional"><ContentTemplate>--%>

                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="True" OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating"
                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="name" AllowSorting="True">
                    <Columns>                
                <asp:TemplateField ItemStyle-Width="40px">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                        <asp:TemplateField HeaderText="I.Code" SortExpression="ItemCode">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="GroupName" HeaderText="Group"  SortExpression="GroupName" ReadOnly="true" />
                        <asp:BoundField DataField="CategoryName" HeaderText="Category" SortExpression="CategoryName"  ReadOnly="true" />
                        <asp:BoundField DataField="Brand" HeaderText="Brand" SortExpression="Brand" ReadOnly="true"  />
                        <asp:BoundField DataField="UnitType" HeaderText="U.Type" SortExpression="UnitType" ReadOnly="true"  />
                        <asp:TemplateField HeaderText="Item Name" SortExpression="name">
                            <EditItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="price" HeaderText="Price" SortExpression="price"  ReadOnly="true" />
                    </Columns>
                </asp:GridView>

      <%--</ContentTemplate></asp:UpdatePanel>--%>

                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                    
                                 SelectCommand="SELECT [ItemCode], [GroupName], [CategoryName], [Brand], [UnitType], [name], [price] FROM [Items] Where ItemType=@ItemType and ProjectID=@ProjectID ORDER BY [GroupName]">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddType" Name="ItemType" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" />
                    </SelectParameters>
                    
                                    </asp:SqlDataSource>
            
            
                                </div>
               </div>

        </div>

    </div> 

                   
 </div>         

            
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    

</asp:Content>


