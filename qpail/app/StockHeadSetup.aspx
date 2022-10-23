<%@ Page Title="Head Setup" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="StockHeadSetup.aspx.cs" Inherits="app_StockHeadSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false" ></asp:Label>	
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">        </asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
        <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
            <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
        </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
     <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>

            
            <div class="row">
				<div class="col-md-12">					
					<h3 class="page-title">
					  Head
					</h3>					
				</div>
			</div>
			<div class="row">


				<div class="col-md-6 ">
					<div class="portlet box red">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> Head Setup
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
                                        <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" DataTextField="ItemName" DataValueField="ProductID" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [ProductID], ItemName FROM [Products] ORDER BY [ItemName]"></asp:SqlDataSource>
									</div>
                                                                        
                                    <div class="form-group hidden">
										<label>Item Group: </label>
                                        <asp:DropDownList ID="ddGroup" CssClass="form-control select2me" runat="server" 
                                            AutoPostBack="true" OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                        </asp:DropDownList>                                        
									</div>
                                    
                                    <div class="form-group">
										<label>Group : </label>
                                        <asp:DropDownList ID="ddSubGrp" CssClass="form-control select2me" runat="server" 
                                            AutoPostBack="true" OnSelectedIndexChanged="ddSubGrp_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                       
									</div>
                                    
                                    <div class="form-group">
										<label>Subsidiary : </label>
                                        <asp:DropDownList ID="ddGrade" CssClass="form-control select2me" runat="server" 
                                            AutoPostBack="true" OnSelectedIndexChanged="ddGrade_SelectedIndexChanged" >
                                        </asp:DropDownList>                                        
									</div>


                                    <div class="form-group">
										<label>Control : </label>
										<asp:DropDownList ID="ddcategory" CssClass="form-control select2me" runat="server" 
                                            AutoPostBack="true" OnSelectedIndexChanged="ddcategory_SelectedIndexChanged" >
                                        </asp:DropDownList>
									</div>
                                                                        
                                    <div class="form-group">
										<label>Head Name : </label>
										<asp:TextBox ID="txtName" runat="server" CssClass="form-control" EnableViewState="true" placeholder="Head Name" />									
									</div>
                                    
                                    <div class="form-group">
										<label>A/C Head Name : </label>
										<asp:DropDownList ID="ddAccHeadName" CssClass="form-control select2me" runat="server" 
                                            AutoPostBack="true" >
                                        </asp:DropDownList>
									</div>
                                    
                                    <%--<div class="form-group hidden">
										<label>Unit Type : </label>
                                        <asp:DropDownList ID="ddUnit" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource6" DataTextField="UnitName" DataValueField="UnitName">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [UnitName] FROM [Units] ORDER BY [UnitName]"></asp:SqlDataSource>
									</div>--%>

                                    <div class="form-group hidden">
										<label>Item Description: </label>
										<asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" placeholder="Description" />											
									</div>
                                    
                                    <div class="form-group hidden">
                                            <label>Standard Weight <asp:Literal ID="ltrUnit" runat="server" Text="(KG)" /></label>
                                            <asp:TextBox ID="txtWeight" runat="server"  CssClass="form-control" placeholder="Weight per Unit" Text="0" />	
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                runat="server" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtWeight">
                                            </asp:FilteredTextBoxExtender>
                                    </div>
                                    
                                    <div class="control-group">
                                    <label class="control-label">Opening Kg.-Pcs. : </label>
                                    <asp:TextBox ID="txtKg" runat="server" Width="35%" Enabled="True" Text="0" CssClass="form-control"></asp:TextBox>
                                    <asp:TextBox ID="txtPcs" runat="server" Width="35%" Text="0" CssClass="form-control" onkeyup="calTtl()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtKg">
                                    </asp:FilteredTextBoxExtender>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtPcs">
                                    </asp:FilteredTextBoxExtender>

                                </div>
                                    
                                    <div class="form-group">
                                    <label class="control-label">Price (Tk.) : </label>
                                        <asp:TextBox ID="txtPrice" runat="server" Text="0" CssClass="form-control" onkeyup="calTtl()" Width="35%"></asp:TextBox>
                                        <%--<asp:DropDownList ID="ddPriceCalType" runat="server" Visible="False" Width="35%" AutoPostBack="True" >
                                        <asp:ListItem Value="1">Per Kg.</asp:ListItem>
                                        <asp:ListItem Value="2">Per Qty.</asp:ListItem>
                                    </asp:DropDownList>--%>
                                        <asp:DropDownList ID="ddUnit" CssClass="form-control select2me" runat="server" Width="35%" DataSourceID="SqlDataSource2" DataTextField="UnitName" DataValueField="UnitName">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [UnitName] FROM [Units] ORDER BY [UnitName]"></asp:SqlDataSource>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtPrice">
                                    </asp:FilteredTextBoxExtender>
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
                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="ProductID" OnRowDeleting="GridView1_OnRowDeleting">
                    <Columns>                
                <asp:TemplateField ItemStyle-Width="20px">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                        <asp:TemplateField HeaderText="ProductID" SortExpression="ProductID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("ProductID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Control" SortExpression="Category" >                            
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Category") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Head Name" SortExpression="ItemName">
                            <ItemTemplate>
                                <asp:Label ID="Label1x" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Opening Kg." SortExpression="OpeningKg">
                            <ItemTemplate>
                                <asp:Label ID="Label1xz" runat="server" Text='<%# Bind("OpeningKg") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Opening Pcs." SortExpression="OpeningPcs">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("OpeningPcs") %>'></asp:Label>
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
                    SelectCommand="SELECT ProductID, 
                    (Select CategoryName from Categories where CategoryID=a.CategoryID) as Category,                      
                    [ItemName], UnitType, [Description], OpeningKg, OpeningPcs FROM [Products] a Where CategoryID=@CategoryID ORDER BY [ItemName]"
                    DeleteCommand="Delete FROM [PrdnPowerPressDetails]  where PrdnID='0'  ">  
                    <SelectParameters>
                            <asp:ControlParameter ControlID="ddcategory" Name="CategoryID" PropertyName="SelectedValue" Type="String" />
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


