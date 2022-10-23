<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Accounts-Control.aspx.cs" Inherits="Operator_Accounts_Control" %>

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
    
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="" ></asp:Label>
                                           
                       <div class="control-group">
                            <label class="control-label">Sub A/C Name : </label>                            
                                 <asp:DropDownList ID="ddSub" runat="server" AutoPostBack="True"  CssClass="form-control select2me" 
                                    onselectedindexchanged="ddSub_SelectedIndexChanged">
                                </asp:DropDownList>                                
                                <asp:Label ID="SubID" runat="server"></asp:Label> 
                       </div>
                                            
                       <div class="control-group">
                            <label class="control-label">Control Account ID : </label>
                            <asp:TextBox ID="txtNid" runat="server" Enabled="false"  CssClass="form-control"  runat="server"></asp:TextBox>
                        </div>
                                   
                       <div class="control-group">
                            <label class="control-label">Control A/C Name : </label>
                            <asp:TextBox ID="txtControl" runat="server" title="Write Sub-Account Name and press Enter"  CssClass="form-control" ></asp:TextBox>
                        </div>
                             
                        
                        <div class="form-actions">
                            <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save Control Head" OnClick="btnSave_Click" />                                   
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

                

                                
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSource1">
        <Columns>
            <asp:BoundField DataField="ControlAccountsID" HeaderText="C.Accounts ID" 
                SortExpression="ControlAccountsID" />
            <asp:BoundField DataField="AccountsName" HeaderText="Accounts Name" 
                SortExpression="AccountsName" />
            <asp:BoundField DataField="ControlAccountsName" HeaderText="Control Accounts Name" 
                SortExpression="ControlAccountsName" />
        </Columns>
    </asp:GridView>
      <br />
        
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"                     
            SelectCommand="SELECT a.ControlAccountsID, b.AccountsName, a.ControlAccountsName FROM ControlAccount a join Accounts b on a.AccountsID=b.AccountsID order by a.AccountsID">
        </asp:SqlDataSource>




                    
                                </div>
                           </div>

                    </div>

                </div> 

                   
             </div>         
             


</asp:Content>

