<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Accounts-Head.aspx.cs" Inherits="Operator_Accounts_Head" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
    
    
            <div class="row">
				<div class="col-md-12">					
					<h3 class="page-title">
					 Control Accounts
					</h3>					
				</div>
			</div>
			<div class="row">
				<div class="col-md-6 ">
					<div class="portlet box red">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> Control Accounts Setup
							</div>
						</div>
						<div class="portlet-body form">
							
					    <div class="form-body">										
    
                        <span style="color:Maroon;"><asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="" ></asp:Label></span> 
                                           
                       <div class="control-group">
                            <label class="control-label">Group Name : </label>                            
                                <asp:DropDownList ID="ddGroup" runat="server" AutoPostBack="True"  CssClass="form-control select2me" 
                                    onselectedindexchanged="ddGroup_SelectedIndexChanged">
                                </asp:DropDownList>                
                                <asp:Label ID="lblGroup" runat="server"></asp:Label>                
                       </div>

                     <asp:Panel ID="bothpanel" runat="server" Visible="false">
                       <div class="control-group">
                            <label class="control-label">Expense Type : </label>
                            <asp:RadioButton ID="RadioButton1" runat="server" Checked="true" Text="HO Only" GroupName="rd" />
                            <asp:RadioButton ID="RadioButton2" runat="server" Checked="false" Text="HO+Branch (Both)" GroupName="rd" />                
                        </div>
                     </asp:Panel>
                                       
                       <div class="control-group">
                            <label class="control-label">Sub-Accounts : </label>
                            <asp:DropDownList ID="ddSub" runat="server" AutoPostBack="True"  CssClass="form-control select2me" 
                                onselectedindexchanged="ddSub_SelectedIndexChanged">
                            </asp:DropDownList>                
                            <asp:Label ID="lblSub" runat="server"></asp:Label>                
                        </div>
                                   
                       <div class="control-group">
                            <label class="control-label">Control Accounts : </label>
                            <asp:DropDownList ID="ddControl" runat="server" AutoPostBack="True"  CssClass="form-control select2me" 
                                onselectedindexchanged="ddControl_SelectedIndexChanged">
                            </asp:DropDownList>                
                            <asp:Label ID="lblControl" runat="server"></asp:Label>
                        </div>
                                                            
                       <div class="control-group">
                            <label class="control-label">A/C Head ID : </label>
                            <asp:TextBox ID="txtHeadID" runat="server" Enabled="False" CssClass="form-control" ></asp:TextBox>
                        </div>
                                                               
                       <div class="control-group">
                            <label class="control-label">A/C Head Name : </label>                         
                            <asp:TextBox ID="txtHeadName" runat="server" CssClass="form-control" ></asp:TextBox>
                        </div>
                                                                                                                       
                       <div class="control-group">
                            <label class="control-label">Openning Balance : </label>                         
                             <asp:TextBox ID="txtOpBalance" runat="server" CssClass="form-control" ></asp:TextBox>
                              <%--<asp:FilteredTextBoxExtender ID="txtOpBalance_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtOpBalance">
                              </asp:FilteredTextBoxExtender>--%>
                        </div>

                       <div class="control-group">
                            <label class="control-label">Date of Initialization : </label>                                                     
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" ></asp:TextBox>
                            <%--<asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                Enabled="True" TargetControlID="txtDate">
                            </asp:CalendarExtender>--%>
                        </div>
                               

                        
                        <div class="form-actions">
                            <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save New Head" OnClick="btnSave_Click" />                                
                            <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" />                              
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
                                    
                                   <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" Width="100%" AutoGenerateDeleteButton="True"
                                            AutoGenerateEditButton="True"
                                  DataKeyNames="AccountsHeadName"   DataSourceID="SqlDataSource1">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Head ID" SortExpression="AccountsHeadID">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("AccountsHeadID") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("AccountsHeadID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="GroupName" HeaderText="Group Name" ReadOnly="true"
                                            SortExpression="GroupName" />
                                        <asp:BoundField DataField="AccountsName" HeaderText="Accounts Name"  ReadOnly="true"
                                            SortExpression="AccountsName" />
                                        <asp:BoundField DataField="ControlAccountsName"  ReadOnly="true"
                                            HeaderText="Control Account" SortExpression="Control Accounts" />
                                        <asp:BoundField DataField="AccountsHeadName" HeaderText="Accounts Head Name"   ReadOnly="true"
                                            SortExpression="AccountsHeadName" />
                                        <asp:TemplateField HeaderText="Op.Balance" SortExpression="Op.Balance">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" Width="50px" runat="server" Text='<%# Bind("AccountsOpeningBalance") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("AccountsOpeningBalance") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                  <br />
        
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                        SelectCommand="SELECT HeadSetup.AccountsHeadID, AccountGroup.GroupName, Accounts.AccountsName, ControlAccount.ControlAccountsName, HeadSetup.AccountsHeadName, HeadSetup.AccountsOpeningBalance FROM AccountGroup INNER JOIN HeadSetup INNER JOIN ControlAccount ON HeadSetup.ControlAccountsID = ControlAccount.ControlAccountsID INNER JOIN Accounts ON HeadSetup.AccountsID = Accounts.AccountsID ON AccountGroup.GroupID = HeadSetup.GroupID  order by AccountGroup.GroupID, HeadSetup.AccountsHeadID" 
                                        DeleteCommand="Delete From HeadSetup  where AccountsHeadName=@AccountsHeadName" UpdateCommand="Update HeadSetup set AccountsOpeningBalance =@AccountsOpeningBalance where  AccountsHeadID=@AccountsHeadID">
                                        <DeleteParameters>
                                            <asp:Parameter Name="AccountsHeadName"  />
                                        </DeleteParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="AccountsOpeningBalance" />
                                            <asp:Parameter Name="AccountsHeadID" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>

                    
                                </div>
                           </div>

                    </div>

                </div> 

                   
             </div>         
             
    

</asp:Content>

