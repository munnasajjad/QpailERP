<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Campaign.aspx.cs" Inherits="app_Campaign" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">

    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="container-fluid">
            <div class="row-fluid">
               <div class="span12"> 
                  <h3 class="page-title">
                     Task Scheduler & Campaign Management
                     <%--<small>form components and widgets</small>--%>
                  </h3>
               </div>
            </div>
            <div class="row-fluid">
               <div class="span12">
                  <!-- BEGIN SAMPLE FORM PORTLET-->   
                  <div class="portlet box blue">
                     <div class="portlet-title">
                        <h4><i class="icon-reorder"></i>Task Scheduler & Campaign Management</h4>
                        <div class="tools">
                           <a href="javascript:;" class="collapse"></a>
                           <a href="#portlet-config" data-toggle="modal" class="config"></a>
                           <a href="javascript:;" class="reload"></a>
                           <a href="javascript:;" class="remove"></a>
                        </div>
                     </div>

                    <div class="portlet-body form">
                       
                        <div class="control-group">
                            <label class="control-label">Start Date</label>
                            <div class="controls">                                
                                 <asp:TextBox ID="txtDate" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
                                    Enabled="True" TargetControlID="txtDate" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </div>
                        </div>
                        
                        
                        <div class="control-group">
                            <label class="control-label">Related Department</label>
                            <div class="controls">                                
                                <asp:DropDownList ID="ddDepartment" runat="server" CssClass="span6 m-wrap" DataSourceID="SqlDataSource3" DataTextField="DepartmentName" DataValueField="DepartmentName">        </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [DepartmentName] FROM [Departments] ORDER BY [DepartmentName]"></asp:SqlDataSource>
                
                            </div>
                        </div>
                        

                        <div class="control-group">
                            <label class="control-label">Task Type</label>
                            <div class="controls">                                
                                 <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource5"  CssClass="span6 m-wrap" 
                                    DataTextField="CampaignType" DataValueField="CampaignType" Class="chzn-select"  >                   
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource5" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"                     
                                    SelectCommand="SELECT [CampaignType] FROM [CampaignTypes]">
                                </asp:SqlDataSource>
                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">Related Project</label>
                            <div class="controls">                                
                                 <asp:DropDownList ID="ddProject" runat="server" DataSourceID="SqlDataSource5x"  CssClass="span6 m-wrap" 
                                    DataTextField="ProjectName" DataValueField="ProjectName" Class="chzn-select"  >                   
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource5x" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"                     
                                    SelectCommand="SELECT [ProjectName] FROM [Projects] ORDER BY [ProjectName]">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="Completed" Name="Status" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">Campaign Details</label>
                            <div class="controls">                                
                                <asp:TextBox ID="txtDetails" CssClass="span6 m-wrap" TextMode="MultiLine" runat="server"></asp:TextBox>
                                <%--<input type="text" class="span6 m-wrap tooltips" data-trigger="hover" data-original-title="Tooltip text goes here. Tooltip text goes here." />    --%>
                            </div>
                        </div>

                        
                        <div class="control-group">
                            <label class="control-label">Remarks</label>
                            <div class="controls">                                
                                <asp:TextBox ID="txtContact" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                                <%--<input type="text" class="span6 m-wrap tooltips" data-trigger="hover" data-original-title="Tooltip text goes here. Tooltip text goes here." />    --%>
                            </div>
                        </div>

                        
                        <div class="control-group">
                            <label class="control-label">End Date</label>
                            <div class="controls">                                
                                 <asp:TextBox ID="txtEDate" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
                                    Enabled="True" TargetControlID="txtEDate" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </div>
                        </div>
                        


                        <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="" ></asp:Label>
                        
                        <div class="form-actions">
                            <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save Campaign" OnClick="btnSave_Click" />
                            <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" OnClick="btnClear_Click" />
                              
                           </div>

                    </div>

                  </div>

               </div>

            </div>

        <div class="row-fluid">
               <div class="span12">
                  <div class="portlet box yellow">
							<div class="portlet-title">
								<h4><i class="icon-coffee"></i>Saved Data</h4>
								<div class="tools">
									<a href="javascript:;" class="collapse"></a>
									<a href="#portlet-config" data-toggle="modal" class="config"></a>
									<a href="javascript:;" class="reload"></a>
									<a href="javascript:;" class="remove"></a>
								</div>
							</div>
							<div class="portlet-body">

                                <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
                                    <Columns>
                                        <asp:BoundField HeaderText="Campaign Start Date" DataField="StartDate" SortExpression="ReqDate" />
                                        <asp:BoundField HeaderText="Subject" DataField="Subject" SortExpression="Subject" />
                                        <asp:BoundField HeaderText="Department" DataField="Department" SortExpression="Department" />
                                        <asp:BoundField HeaderText="Project" DataField="ProjectInterested" SortExpression="ProjectInterested" />
                                        <asp:BoundField HeaderText="Campaign Details" DataField="CampaignDetails" SortExpression="CampaignDetails" />
                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" SortExpression="Remarks" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [StartDate], [Subject], [Department], [ProjectInterested], [CampaignDetails], [Remarks] FROM [Campaigns]"></asp:SqlDataSource>



							</div>
						</div>
               </div>
            </div>



    </div>









</asp:Content>

