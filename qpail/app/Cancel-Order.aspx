<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Cancel-Order.aspx.cs" Inherits="AdminCentral_Cancel_Order" %>
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
						<h6>Cancel Order from Pending Delivery</h6>
					</div>
					<div class="widget_content">
						<h3>Cancel Order</h3>
						<%--<p>
							 Cras erat diam, consequat quis tincidunt nec, eleifend a turpis. Aliquam ultrices feugiat metus, ut imperdiet erat mollis at. Curabitur mattis risus sagittis nibh lobortis vel.
						</p>--%>
						<p style="float:right; text-align:right; color:Red;">
						<asp:Label ID="lblMsg" CssClass="msg" runat="server" EnableViewState="false"></asp:Label>
						<asp:Label ID="lblErrLoad" runat="server" CssClass="msg" EnableViewState="false"></asp:Label>
						</p>
						
						<div class="form_container left_label">
							<ul>
							    
								<li>
								<div class="form_grid_12">
									<label class="field_title">Order Name:</label>
									<div class="form_input">
                                        
									    <asp:DropDownList ID="ddInvNo" runat="server" data-placeholder="--- Select ---" 
                                             style="width:60%; min-width:150px" Height="25px" class="chzn-select" >
                                        </asp:DropDownList>
										<%--<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [OrderStatement] FROM [Orders] where IsActive='true'"></asp:SqlDataSource>--%>
									            <asp:Button ID="btnLoad"  class="btn_small btn_blue"   runat="server" 
                                                    Text="Load Order" onclick="btnLoad_Click" />
									</div>
								</div>
								</li>
																
									<li>
								        <div class="form_grid_12">
									        <label class="field_title">Invoice No.</label>
									        <div class="form_input">
                                                <asp:TextBox ID="InvNo" ReadOnly="true" runat="server" tabindex="1" class="tip_top"  title="Enter Pin Code" ></asp:TextBox>										        									            
                                                <asp:Label ID="lblDue" runat="server" Text="" EnableViewState="false" Font-Bold="True" Font-Size="16"></asp:Label>
									        </div>
								        </div>
								    </li>					
								
								<li>
								<div class="form_grid_12">
									<label class="field_title">Order Description</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtDescription" ReadOnly="true" runat="server" tabindex="2"  title="Order Description"  class="limiter tip_top"></asp:TextBox>
										
									</div>
								</div>
								</li>
								
								<li>
								<div class="form_grid_12">
									<label class="field_title">Sender Name</label>
									<div class="form_input">
                                        <asp:TextBox ReadOnly="true" ID="txtSender" runat="server" tabindex="3"  title="Sender Name"  class="limiter tip_top"></asp:TextBox>
										
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Sender Mobile No.</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtSMobile" runat="server" tabindex="3"  title="Sender Mobile No."  class="limiter tip_top"></asp:TextBox>
										
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Receiver Name</label>
									<div class="form_input">
                                        <asp:TextBox ReadOnly="true" ID="txtReceiver" runat="server" tabindex="3"  title="Receiver Name"  class="limiter tip_top"></asp:TextBox>
										
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Receiver Mobile No.</label>
									<div class="form_input">
                                        <asp:TextBox ReadOnly="true" ID="txtRMobile" runat="server" tabindex="3"  title="Receiver Mobile No."  class="limiter tip_top"></asp:TextBox>
										
									</div>
								</div>
								</li>
								
								<li>
								<div class="form_grid_12">
									<label class="field_title">Amount To Pay</label>
									<div class="form_input">
                                        <asp:TextBox ReadOnly="true" ID="txtPaid" runat="server" tabindex="3"  title="(+)Pay or (-)Due Amount"  class="limiter tip_top"></asp:TextBox>
										
									</div>
								</div>
								</li>
								
								<asp:Panel ID="Panel1" runat="server" Visible="false">    
								<li>                                                                    
								    <div class="form_grid_12">
									<label class="field_title">Collected Amount</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtCollected" ReadOnly="true" runat="server" tabindex="10" title="Collected Amount"  class="limiter tip_top"></asp:TextBox>
										<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtCollected">
                                        </asp:FilteredTextBoxExtender>
									</div>
								</div>								
								</li>
								</asp:Panel>
								
								<li>
								<div class="form_grid_12">
									<div class="form_input">
									
									    <asp:Button ID="btnSave" runat="server" Text="Delete Order" TabIndex="11"
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
                                        <asp:TextBox ID="txtPin" runat="server" tabindex="10" title="Collected Amount"  
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

