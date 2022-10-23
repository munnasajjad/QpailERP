<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Item-Ingradiants.aspx.cs" Inherits="Operator_Item_Ingradiants" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false" ></asp:Label>	

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
            
            <div class="row">
				<div class="col-md-12">					
					<h3 class="page-title">
					 Item Ingradiants <small>Pre-assumptions of raw items consumptions </small>
					</h3>					
				</div>
			</div>
			<div class="row">

                
            <%--<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>

				<div class="col-md-6 ">
					<div class="portlet box red">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> Item Ingradiants Setup
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
										
										<asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="help-block" ></asp:Label>	
                                                                        
                                    <div class="form-group">
										<label>Finished Item Name : </label>
                                        <asp:DropDownList ID="ddFinished" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource2" DataTextField="name" DataValueField="ItemCode" AutoPostBack="true" OnSelectedIndexChanged="ddFinished_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT name, ItemCode FROM Items WHERE (ItemType = 'Finished Goods') AND (ProjectID = @ProjectID) ORDER BY name">
                                            <SelectParameters>                                                
                                                <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="Int32" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
									</div>
                                    

                                    <div class="form-group">
										<label>Raw/Semi Item Name : </label>
                                        <asp:DropDownList ID="ddRawItem" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" DataTextField="name" DataValueField="ItemCode"  AutoPostBack="true" OnSelectedIndexChanged="ddRawItem_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT ItemCode, name FROM Items WHERE (ItemType = 'Semi Finished Items' OR ItemType = 'Raw Materials')   AND (ProjectID = @ProjectID) ORDER BY name">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="Int32" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
									</div>


                                    <div class="form-group">
										<label>Required Quantity : </label>
										<asp:TextBox ID="txtDept" runat="server" CssClass="form-control" EnableViewState="true" placeholder="ie, 0.001 or 1.50 etc." />									
									</div>

                                    <div class="form-group">
										<label>Wastage (%): </label>
										<asp:TextBox ID="txtWastage" runat="server" CssClass="form-control" EnableViewState="true" placeholder="type 5 for 5% wastage of raw item" />									
									</div>


                                    <div class="form-group">
										<label>Unit Type : </label>
										<asp:TextBox ID="txtDesc" runat="server" ReadOnly="true" CssClass="form-control" placeholder="" />											
									</div>

                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Add" onclick="btnSave_Click1" />
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

  
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AutoGenerateDeleteButton="True" OnRowDeleting="GridView1_RowDeleting"
                    DataSourceID="SqlDataSource1" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="I.Code" SortExpression="RawItemCode">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("RawItemCode") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("RawItemCode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RawItemName" HeaderText="Raw Item Name" SortExpression="RawItemName" />
                        <asp:BoundField DataField="RequiredQuantity" HeaderText="Required Quantity" SortExpression="RequiredQuantity" />
                        <asp:BoundField DataField="UnitType" HeaderText="Unit Type" SortExpression="UnitType" />
                        <asp:BoundField DataField="Wastage" HeaderText="Wastage (%)" SortExpression="Wastage" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                    
                    SelectCommand="SELECT [RawItemCode], [RawItemName], [RequiredQuantity], Wastage, [UnitType], ProjectCode FROM [Ingradiants] WHERE (([ItemCode] = @ItemCode) AND ([ProjectCode] = @ProjectCode))" >
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddFinished" Name="ItemCode" PropertyName="SelectedValue" Type="String" />
                        <asp:ControlParameter ControlID="lblProject" Name="ProjectCode" PropertyName="Text" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            
            
                                </div>
               </div>

        </div>

    </div> 
                    
                                <%--</ContentTemplate> </asp:UpdatePanel>--%>
                                

                   
 </div>         


</asp:Content>


