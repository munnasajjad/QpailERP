<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Transfer-Approval.aspx.cs" Inherits="Cells_Transfer_Approval" %>
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
						<h6>Cash Transfer Approval</h6>
					</div>
					<div class="widget_content">
						<h3>Approve Cash Transfer</h3>
						<%--<p>
							 Cras erat diam, consequat quis tincidunt nec, eleifend a turpis. Aliquam ultrices feugiat metus, ut imperdiet erat mollis at. Curabitur mattis risus sagittis nibh lobortis vel.
						</p>--%>
						<p style="float:right; text-align:right; color:Red;">
						<asp:Label ID="lblMsg" CssClass="msg" runat="server" EnableViewState="false"></asp:Label>
						<asp:Label ID="lblErrLoad" runat="server" CssClass="msg" EnableViewState="false"></asp:Label>
                            <asp:Label ID="lblBranch" runat="server" Visible="false" Text=""></asp:Label>
						</p>
						
						<div class="form_container left_label">
							<ul>
							    
								<li>
								<div class="form_grid_12">
									<label class="field_title">Transfer ID:</label>
									<div class="form_input">
                                        
									    <asp:DropDownList ID="ddType" runat="server" data-placeholder="--- Select ---" 
                                             style="width:60%; min-width:100px" Height="25px" class="chzn-select" 
                                            DataSourceID="SqlDataSource3" DataTextField="TransferInvoiceID" 
                                            DataValueField="TransferInvoiceID" >
                                            
                                        </asp:DropDownList>
                                        <asp:Button ID="txtLoad" runat="server" Text="Load" onclick="txtLoad_Click" />
                                        
                                        
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            
                                            SelectCommand="SELECT [TransferInvoiceID] FROM [Transfers] WHERE (([IsApproved] = 'P') AND ([TransferTo] = @TransferTo)) ORDER BY [TransferID]">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblBranch" Name="TransferTo" PropertyName="Text" 
                                                    Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
									</div>
								</div>
								</li>
																
									<li>
								        <div class="form_grid_12">
									        <label class="field_title">Sender Branch</label>
									        <div class="form_input">
                                                <asp:DropDownList ID="ddBranch" runat="server" data-placeholder="--- Select ---" 
                                             style="width:60%; min-width:150px" Height="25px" class="chzn-select" 
                                                    DataSourceID="SqlDataSource2" DataTextField="BranchName" 
                                                    DataValueField="BranchName" Enabled="false" >
                                        </asp:DropDownList>
										<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [BranchName] FROM [Branches] ORDER BY [BranchName]"></asp:SqlDataSource>
									
                                                
                                                </div>
								        </div>
								    </li>					
								
								<li>
								<div class="form_grid_12">
									<label class="field_title">Description</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtDescription" runat="server" tabindex="2"  
                                            title="Transfer Description"  class="limiter tip_top" ReadOnly="True"></asp:TextBox>
										<span class="input_instruction green">Deposit Detail: ie, DBBL-Uttara Branch.</span>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Transfer Amount</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtAmount" runat="server" tabindex="3"  
                                            title="Transfer Amount"  class="limiter tip_top" ReadOnly="True"></asp:TextBox>
										<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtAmount">
                                            </asp:FilteredTextBoxExtender>
									</div>
								</div>
								</li>
								
								<li>
								<div class="form_grid_12">
									<div class="form_input">
									
									    <asp:Button ID="btnSave" runat="server" Text="Approve Transfer" TabIndex="11"
                                            class="btn_small btn_blue"  onclick="btnSave_Click" />
                                        <asp:Button ID="Button1" runat="server" Text="Delete Transfer" 
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
						</div>
					</div>
				</div>
			</div>

</asp:Content>