<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Credit-Approval.aspx.cs" Inherits="AdminCentral_Credit_Approval" %>
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
						<h6>Credit-Limit Approval for Agents</h6>
					</div>
					<div class="widget_content">
						<h3>Credit-Limit Approval</h3>
						
						<p style="float:right; text-align:right; color:Red;">
						<asp:Label ID="lblMsg" CssClass="msg" runat="server" EnableViewState="false"></asp:Label>
						<asp:Label ID="lblErrLoad" runat="server" CssClass="msg" EnableViewState="false"></asp:Label>
						</p>
						
						<div class="form_container left_label">
							<ul>
							    	<asp:UpdatePanel ID="updatePanel1" runat="server">  <ContentTemplate>			
									<li>
								        <div class="form_grid_12">
									        <label class="field_title">Receiver Agent</label>
									        <div class="form_input">
                                                <asp:DropDownList ID="ddBranch" runat="server" data-placeholder="--- Select ---" 
                                                        style="width:60%; min-width:150px" Height="26px" AutoPostBack="True" 
                                                    onselectedindexchanged="ddBranch_SelectedIndexChanged">
                                                </asp:DropDownList>
										<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            
                                                    SelectCommand="SELECT [AgentName] FROM [Credit4Approval] WHERE ([IsApproved] = @IsApproved) ORDER BY [CLID]">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="false" Name="IsApproved" Type="Boolean" />
                                            </SelectParameters>
                                                </asp:SqlDataSource>
									
                                                </div>
								        </div>
								    </li>					
								</ContentTemplate> </asp:UpdatePanel>
								
								
								<li>
								<div class="form_grid_12">
									<label class="field_title">Detail</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtDescription" runat="server" tabindex="2"  title="Collection Description"  class="limiter tip_top"></asp:TextBox>
										<span class="input_instruction green">Detail/ Remarks.</span>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Approved Credit-Limit</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtAmount" runat="server" tabindex="3"  title="Approved Amount"  class="limiter tip_top"></asp:TextBox>
										<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtAmount">
                                            </asp:FilteredTextBoxExtender>
									</div>
								</div>
								</li>
								
								<li>
								<div class="form_grid_12">
									<div class="form_input">
									
									    <asp:Button ID="btnSave" runat="server" Text="Approve Cr. Limit" TabIndex="11"
                                            class="btn_small btn_blue"  onclick="btnSave_Click" />
                                        <asp:Button ID="Button1" runat="server" Text="Clear Form" 
                                            class="btn_small btn_orange" />
									
										<%--<button type="submit" class="btn_small btn_gray"><span>Submit</span></button>
										<button type="reset" class="btn_small btn_gray"><span>Reset</span></button>
										<button type="submit" class="btn_small btn_blue"><span>Sent New Order</span></button>
										<button type="reset" class="btn_small btn_blue"><span>Reset</span></button>
										<button type="submit" class="btn_small btn_orange"><span>Clear Form</span></button>
										<button type="reset" class="btn_small btn_orange"><span>Reset</span></button>--%>
									</div>
								</div>
								</li>
								<asp:Panel ID="Panel2" runat="server" Visible="false">    
								<li>                                                                    
								    <div class="form_grid_12">
									<label class="field_title">Invoice No.</label>
									<div class="form_input">
                                        <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>
                                        <asp:TextBox ID="InvNo" runat="server" tabindex="10" title="Collected Amount"  
                                            class="limiter tip_top" Font-Bold="True"></asp:TextBox>
										<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtCollected">
                                        </asp:FilteredTextBoxExtender>
									</div>
								</div>								
								</li>
								</asp:Panel>
								
							</ul>
                            <asp:GridView ID="GridView1" runat="server" Width="100%">
                            </asp:GridView>
						</div>
					</div>
				</div>
			</div>

</asp:Content>

