<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Branch-Setup.aspx.cs" Inherits="AdminCentral_Branch_Setup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    
            <div class="grid_12 full_block">
				<div class="widget_wrap">
					<div class="widget_top">
						<span class="h_icon list_image"></span>
						<h6>New Branch Setup Form</h6>
					</div>
					<div class="widget_content">
												
						<asp:Label ID="lblMsg" CssClass="msg" runat="server" EnableViewState="false"></asp:Label>
						 
						<div class="form_container left_label">
							<ul>							   
								<li>
								<div class="form_grid_12">
									<label class="field_title">Country</label>
									<div class="form_input">
                                        
									    <asp:DropDownList ID="ddType" runat="server" data-placeholder="--- Select ---" 
                                           style="width:30%; min-width:150px"   class="chzn-select" 
                                            DataSourceID="SqlDataSource2" DataTextField="country" DataValueField="country" >                                            
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [country] FROM [Countries]"></asp:SqlDataSource>
										<%--<input name="filed01" type="text" tabindex="1" class="limiter"/>
										<span class="input_instruction green">This is an instruction</span>--%>
									</div>
								</div>
								</li>
								
								
									<li>
								        <div class="form_grid_12">
									        <label class="field_title">Branch Name</label>
									        <div class="form_input">
                                                <asp:TextBox ID="txtName" runat="server" tabindex="1" class="limiter tip_top"  title="Branch Name" ></asp:TextBox>										        									            
									            
									        </div>
								        </div>
								    </li>					
								
								<li>
								<div class="form_grid_12">
									<label class="field_title">District</label>
									<div class="form_input">
                                        <asp:DropDownList name="" id="ddlArea" runat="server"
                                         data-placeholder="--- Select ---" style="width:40%; min-width:150px" 
                                            class="chzn-select" tabindex="2" DataSourceID="SqlDataSource1" 
                                            DataTextField="DistrictName" DataValueField="DistrictName" >
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [DistrictName] FROM [Districts]"></asp:SqlDataSource>
										
									</div>
								</div>
								</li>
                                <asp:Panel ID="Panel1" runat="server" Visible="false">                                
								<li>
								<div class="form_grid_12">
									<label class="field_title">Address</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtAddress" runat="server" tabindex="3"  title="Branch Location Address"  class="limiter tip_top"></asp:TextBox>
										
									</div>
								</div>
								</li>
								</asp:Panel>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Contact Numbers</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtMobile" runat="server" tabindex="4" MaxLength="100"  title="Contact Numbers of the Branch"  class="limiter tip_top"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789, " TargetControlID="txtMobile">
                                        </asp:FilteredTextBoxExtender>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMobile" ErrorMessage="Mobile is required." Font-Size="15px" ForeColor="Red" ToolTip="Mobile is required." ValidationGroup="ValidGroup">Mobile is required.</asp:RequiredFieldValidator>
									</div>
								</div>
								</li>
								
								
								<li>
								<div class="form_grid_12">
									<label class="field_title">
                                        <asp:Label ID="lblSend" runat="server" Text="Email Address"></asp:Label></label>
									<div class="form_input">                                      

                                            <asp:TextBox ID="txtEmail" runat="server" title="Email Address" class="tip_top"
                                                 TabIndex="5" ></asp:TextBox>
                                            
                                                                                    
									</div>
								</div>
								</li>
								<%--</ContentTemplate> </asp:UpdatePanel>--%>
																
								<li>
								<div class="form_grid_12">
									<label class="field_title">Credit Limit</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtLimit" runat="server" title="Credit Limit"  tabindex="7" class="tip_top"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtLimit">
                                        </asp:FilteredTextBoxExtender>
										
									</div>
								</div>
								</li>
								
								<li>
								<div class="form_grid_12">
									<label class="field_title">Openning Balance</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtBalance" runat="server" tabindex="8" title="Openning Balance"  class="tip_top"></asp:TextBox>
										<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtBalance">
                                        </asp:FilteredTextBoxExtender>
									</div>
								</div>
								</li>
								
								
								<li>
								<div class="form_grid_12 btn-area">
									<div class="form_input">
									
									    <asp:Button ID="btnSave" runat="server" Text="Save New Branch" TabIndex="11"
                                           class="btn_small btn_blue"  onclick="btnSave_Click" />
                                        <asp:Button ID="Button1" runat="server" Text="Clear Form" 
                                            class="btn_small btn_orange" onclick="Button1_Click" />
									
										<%--<button type="submit" class="btn_small btn_gray"><span>Submit</span></button>
										<button type="reset" class="btn_small btn_gray"><span>Reset</span></button>
										<button type="submit" class="btn_small btn_blue"><span>Sent New Order</span></button>
										<button type="reset" class="btn_small btn_blue"><span>Reset</span></button>
										<button type="submit" class="btn_small btn_orange"><span>Clear Form</span></button>
										<button type="reset" class="btn_small btn_orange"><span>Reset</span></button>--%>
									</div>
								</div>
								</li>
								
							</ul>
						</div>
					</div>
				</div>
			</div>

    
</asp:Content>


