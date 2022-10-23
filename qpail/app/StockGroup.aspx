<%@ Page Title="Group" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="StockGroup.aspx.cs" Inherits="app_StockGroup" %>

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
					 Group 
					</h3>					
				</div>
			</div>
			<div class="row">


				<div class="col-md-6 ">
					<div class="portlet box red">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i>Group Setup
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
                                        <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" DataTextField="CategoryName" DataValueField="CategoryName" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [CategoryName] FROM [ItemSubGroup] ORDER BY [CategoryName]"></asp:SqlDataSource>
									</div>
                                    
                                    <div class="form-group hidden">
										<label>Item Group: </label>
                                        <asp:DropDownList ID="ddCategory" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource2" DataTextField="GroupName" DataValueField="GroupSrNo"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] WHERE GroupSrNo='11' ORDER BY [GroupSrNo]"></asp:SqlDataSource>
									</div>


                                    <div class="form-group">
										<label>Group: </label>
										<asp:TextBox ID="txtDept" runat="server" CssClass="form-control" EnableViewState="true" placeholder="Group Name" />									
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
                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="CategoryName" OnRowDeleting="GridView1_OnRowDeleting">
                    <Columns>                
                <asp:TemplateField ItemStyle-Width="40px">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                         <asp:TemplateField HeaderText="GradeID" SortExpression="GradeID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("CategoryID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Group Name" SortExpression="GroupN" Visible="False" >                            
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("GroupN") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Group" SortExpression="CategoryName">
                            <ItemTemplate>
                                <asp:Label ID="Label11x" runat="server" Text='<%# Bind("CategoryName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" SortExpression="Description">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Select to Edit" />
                                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />

                                                    <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                    </asp:ConfirmButtonExtender>
                                                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                        PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                    <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                        <b style="color: red">Item will be deleted permanently!</b><br /><br />
                                                        Are you sure to delete the item from list?
                                                            <br /> <br /> <br/> <br/>
                                                        <div style="text-align: right;">
                                                            <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                            <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                        </div>
                                                    </asp:Panel>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                    </Columns>
                </asp:GridView>


                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"                     
                    SelectCommand="SELECT CategoryID, (Select GroupName from ItemGroup where GroupSrNo=a.GroupID) as GroupN, [CategoryName], [Description] FROM [ItemSubGroup] a Where GroupID=@GroupID ORDER BY [CategoryName]"
                    DeleteCommand="Delete FROM [PrdnPowerPressDetails]  where PrdnID='0'  "> 
                    <SelectParameters>
                            <asp:ControlParameter ControlID="ddCategory" Name="GroupID" PropertyName="SelectedValue" Type="String" />
                       </SelectParameters>
                </asp:SqlDataSource>
            
            
                                </div>
               </div>

        </div>

    </div> 

                   
 </div>         

            
        </ContentTemplate>
    </asp:UpdatePanel>
    

</asp:Content>


