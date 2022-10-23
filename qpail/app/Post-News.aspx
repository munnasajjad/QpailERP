<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Post-News.aspx.cs" Inherits="AdminCentral_Post_News" %>
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
						<h6>Post News update to show on Dashboard</h6>
					</div>
					<div class="widget_content">
						<h3>News Posting</h3>
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
									<label class="field_title">News Headline</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtHeading" runat="server" tabindex="0" class="tip_top"  title="Headline" ></asp:TextBox>
									    <asp:DropDownList ID="ddInvNo" runat="server" data-placeholder="--- Select ---"  Visible="false"
                                             style="width:60%; min-width:150px" Height="25px" class="chzn-select" >
                                        </asp:DropDownList>
										<%--<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [OrderStatement] FROM [Orders] where IsActive='true'"></asp:SqlDataSource>--%>
									            </div>
								</div>
								</li>
																
									<li>
								        <div class="form_grid_12">
									        <label class="field_title">News Details</label>
									        <div class="form_input">
                                                <asp:TextBox ID="txtMsgBody" runat="server" Width="100%"  tabindex="5"  AutoFocus="False" />
									        </div>
								        </div>
								    </li>					
								
								<li>
								<div class="form_grid_12">
									<div class="form_input">
									
									    <asp:Button ID="btnSave" runat="server" Text="Post News" TabIndex="11"
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
							</ul>
						</div>
					</div>
				</div>
			</div>

    
</asp:Content>

