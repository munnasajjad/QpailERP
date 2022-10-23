<%@ Page Title="Messaging" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Post-Message.aspx.cs" Inherits="AdminCentral_Post_Message" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 
    <asp:ScriptManager ID="ScriptManager1" runat="server">        </asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
        <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
            <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
        </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
             <script type="text/javascript" language="javascript">
                 Sys.Application.add_load(callJquery);
            </script>
	
			
    
            <div class="grid_6 full_block">
				<div class="widget_wrap">
					<div class="widget_top">
						<span class="h_icon list_image"></span>
						<h6>Send message to users</h6>
					</div>
					<div class="widget_content">
						
						<asp:Label ID="lblMsg" CssClass="msg" runat="server" EnableViewState="false"></asp:Label>
						
						<div class="form_container left_label">
							<ul>						    
							    
							    <li>
								<div class="form_grid_12">
									<label class="field_title">Receiver User ID</label>
									<div class="form_input">
									    <asp:DropDownList ID="ddUsers" runat="server" data-placeholder="--- Select ---"  
                                             style="width:60%; min-width:150px" Height="25px" class="form-control select2me" 
                                            DataSourceID="SqlDataSource2" DataTextField="LoginUserName" 
                                            DataValueField="LoginUserName" >
                                        </asp:DropDownList>
										<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [LoginUserName] FROM [Logins] order by [LoginUserName]"></asp:SqlDataSource>
									            </div>
								</div>
								</li>
								
								
								<li>
								<div class="form_grid_12">
									<label class="field_title">Subject</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtSubject" runat="server" tabindex="0"   title="Subject" ></asp:TextBox>
										</div>
                                    <br /> <br/>
								</div>
								</li>
																
									<li>
								        <div class="form_grid_12">
									        <%--<div class="form_input">--%>
                                                <asp:TextBox ID="txtMsgBody" runat="server" Width="490px" class="form-control" Height="380px" tabindex="5"  AutoFocus="False" />

									        <%--</div>--%>
								        </div>
								    </li>					
								
								<li>
								<div class="form_grid_12">
									<div class="form_input">
									
									    <asp:Button ID="btnSave" runat="server" Text="Send Message" class="btn_small btn_blue"  onclick="btnSave_Click" />
                                        <asp:Button ID="Button1" runat="server" Text="Clear Form" class="btn_small btn_orange" />
									
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
            

        <div class="col-md-6 ">
            <!-- BEGIN SAMPLE FORM PORTLET-->
            <div class="portlet box green ">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>
                        List of Send Messages
                    </div>
                    <div class="tools">
                        
                    </div>
                </div>
                <div class="portlet-body form">
                    <div class="form-horizontal" role="form">
                        <div class="form-body">
                            <asp:GridView ID="GridView1" runat="server" DataKeyNames="MsgID" DataSourceID="SqlDataSource1" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="Receiver" HeaderText="Receiver" SortExpression="Receiver" />
                                    <asp:BoundField DataField="Subject" HeaderText="Subject" SortExpression="Subject" />
                                    <asp:BoundField DataField="EntryDate" HeaderText="EntryDate" SortExpression="EntryDate" DataFormatString="{0:d}" />
                                    <asp:TemplateField HeaderText="MsgID"  SortExpression="MsgID">
                                      <ItemTemplate>
                                            <asp:HyperLink ID="Label1" runat="server" Text="Edit" Target="_blank" NavigateUrl='<%# Bind("MsgID") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                SelectCommand="SELECT [Receiver], [Subject], [EntryDate], [MsgID] FROM [Messaging] WHERE ([Sender] = @Sender) ORDER BY [EntryDate] DESC"
                                DeleteCommand="Delete Messaging where MsgID=@MsgID" >
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="Label1" Name="Sender" PropertyName="Text" Type="String" />
                                </SelectParameters>
                                <DeleteParameters>
                                    <asp:Parameter  Name="MsgID"  Type="String" />
                                </DeleteParameters>
                            </asp:SqlDataSource>
                            
                            <asp:Label ID="Label1" runat="server" Text="" Visible="False"></asp:Label>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    


    
</asp:Content>

