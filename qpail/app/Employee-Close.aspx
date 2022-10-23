<%@ Page Title="Promotion/ Termination" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Employee-Close.aspx.cs" Inherits="Operator_Employee_Close" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    
    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>
            

    <div class="row">
		<div class="col-md-12">					
			<h3 class="page-title">
				Turn on/off Employee
			</h3>					
		</div>
	</div>
	<div class="row">
		<div class="col-md-6 middle_of_page">
			<div class="portlet box red">
				<div class="portlet-title">
					<div class="caption">
						<i class="fa fa-reorder"></i> Promotion/Termination
					</div>
				</div>
				<div class="portlet-body form">
							
						<div class="form-body">
										<asp:Label ID="lblMsg" runat="server"></asp:Label>
                            
                       <div class="control-group">
                            <label class="control-label">Employee Status :  </label>
                            <div class="controls">                                
                                <asp:DropDownList ID="ddStatus" runat="server" 
                                    AutoPostBack="True" onselectedindexchanged="ddStatus_SelectedIndexChanged">
                                    <asp:ListItem Value="Closed">Close Employee</asp:ListItem>
                                    <asp:ListItem>Re-joined</asp:ListItem>
                                </asp:DropDownList>
                               
                            </div>
                        </div>
                        
                                
                       <div class="control-group">
                            <label class="control-label">Employee Code :  </label>
                            <div class="controls">                                
                                 <asp:DropDownList ID="ddEmpId" runat="server" AppendDataBoundItems="True" 
                                    AutoPostBack="True" onselectedindexchanged="ddEmpId_SelectedIndexChanged" 
                                    Width="120px">
                                </asp:DropDownList>
                               
                            </div>
                        </div>
                            
                       <div class="control-group">
                            <label class="control-label">Employee Name :  </label>
                            <div class="controls">                                
                                <asp:TextBox ID="txtName" runat="server" Enabled="False"></asp:TextBox>                               
                            </div>
                        </div>
                            
                       <div class="control-group">
                            <label class="control-label">Salary :  </label>
                            <div class="controls">                                
                                <asp:TextBox ID="txtSalary" runat="server"></asp:TextBox>
                               
                            </div>
                        </div>
                            
                       <div class="control-group">
                            <label class="control-label">Employee Serial :  </label>
                            <div class="controls">                                
                                <asp:TextBox ID="txtCardSl" runat="server" ToolTip="Pick or Type in MM/DD/YYYY" ></asp:TextBox>
                               
                            </div>
                        </div>
                        



    
                            
                         <div class="form-actions">
                            <asp:Button ID="btnUpdate" CssClass="btn blue" runat="server" Text="Update Employee" OnClick="btnUpdate_Click" />
                            <asp:Button ID="btnDelete" CssClass="btn" runat="server" Text="Delete Employee" OnClick="btnDelete_Click" />                              
                         </div>
                        
            
							</div>
						</div>                        
               </div>
            </div>

                


        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
