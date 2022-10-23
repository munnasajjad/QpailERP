<%@ Page Title="Party Info" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Supplier-Info.aspx.cs" Inherits="Operator_Supplier_Info" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <style type="text/css">
        label{font-weight:bold; color:#35AA47;}
    </style>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">
    
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    
            <div class="row">
				<div class="col-md-12">					
					<h3 class="page-title">
					   Suppliers Setup
					</h3>					
				</div>
			</div>
			<div class="row">                                
				<div class="col-md-6 ">
					<div class="portlet box red">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> Suppliers Info
							</div>
							
						</div>
						<div class="portlet-body form">
								<div class="form-body">
										
                                    <div class="form-group">
                                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                        <asp:Label ID="lblEid" runat="server" Text="Code No.#: " Visible="false"></asp:Label>            
                                        <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click" Visible="false">Edit Party</asp:LinkButton>                
                                        <asp:Label ID="lblErrLoad" runat="server"></asp:Label>
                                        <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>
                                    </div>

                                    <div class="form-group col-md-6">
										<label> Name:  </label>    
                                        <asp:TextBox ID="txtName" runat="server"  CssClass="form-control" ></asp:TextBox>
                                        <asp:DropDownList ID="ddName" runat="server" AppendDataBoundItems="True" 
                                            AutoPostBack="True" DataSourceID="SqlDataSource2"  CssClass="form-control select2me" 
                                            DataTextField="VarSupplierName" DataValueField="VarSupplierName" 
                                            onselectedindexchanged="ddName_SelectedIndexChanged" Visible="False" >
                                            <asp:ListItem>---Select---</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [VarSupplierName] FROM [Party] ORDER BY [VarSupplierName]"></asp:SqlDataSource>   
									</div>

                                    
                                    <div class="form-group col-md-6">
										<label>Company </label>    
                                        <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control" ></asp:TextBox> 
									</div>


                                    <div class="form-group col-md-12">
										<label>Location </label>    
                                        <asp:DropDownList ID="ddArea" runat="server"  CssClass="form-control select2me" 
                                            DataSourceID="SqlDataSource3" DataTextField="PlaceName" 
                                            DataValueField="PlaceName">
                    
                                        </asp:DropDownList>
                
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [PlaceName] FROM [Places] ORDER BY [PlaceName]"></asp:SqlDataSource>
                
									</div>


                                    <div class="form-group col-md-12">
										<label>Address </label>    
                                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine"  CssClass="form-control"  ></asp:TextBox> 
									</div>


                                    <div class="form-group col-md-6">
										<label>Email </label>    
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ></asp:TextBox>        
									</div>


                                    <div class="form-group col-md-6">
										<label>Contact No. </label>    
                                         <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" ></asp:TextBox>  
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789-," TargetControlID="txtMobile">
                                        </asp:FilteredTextBoxExtender>              
									</div>

                                    <asp:Panel ID="hdn" runat="server" Visible="false">                                        
                                <%--<div class="form-group">
										<label> &nbsp; </label>    
                                                      
									</div>--%>
                                        <asp:Label ID="Label12" runat="server" Text="Party Type :  "></asp:Label>
            
                                        <asp:DropDownList ID="txtCountry" runat="server">                    
                                            <asp:ListItem Value="S">Supplier</asp:ListItem>
                                            <asp:ListItem Value="C">Customer</asp:ListItem>
                                        </asp:DropDownList>
               
                                        <asp:Label ID="Label2" runat="server" Text="Credit Limit:"></asp:Label>
           
                                        <asp:TextBox ID="txtCredit" runat="server" Text="0"></asp:TextBox>  
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789-." TargetControlID="txtCredit">
                                        </asp:FilteredTextBoxExtender>
             
                                    </asp:Panel>

                                    
                                    <div class="form-group col-md-12">
										<label>Openning Balance </label>    
                                        <asp:TextBox ID="txtBalance" runat="server" Text="0" CssClass="form-control" ></asp:TextBox>  
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789-." TargetControlID="txtBalance">
                                            </asp:FilteredTextBoxExtender>              
									</div>
                                    <div class="form-group"> <label> &nbsp; </label>    </div>
                                    <div class="form-actions">
                                        <asp:Button ID="Button1" runat="server" Text="Save Party" CssClass="btn blue" 
                                                onclick="Button1_Click" />
                                        <asp:Button ID="btnEdit" runat="server" Text="Edit Party"  CssClass="btn"
                                        onclick="btnEdit_Click" />
                                    </div>



                                </div>
						</div>

					</div>
    </div> 
                                                      
                
				<div class="col-md-6 ">
					<div class="portlet box green">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> <asp:Literal ID="lblType" runat="server" Text="Saved Parties"></asp:Literal>
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


    <asp:GridView ID="GridView1" runat="server" Width="100%" AllowSorting="True" 
        AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" 
        BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
        DataSourceID="SqlDataSource1" ForeColor="Black" GridLines="Vertical">
        <Columns>
            <asp:BoundField DataField="VarSupplierName" HeaderText="Name" 
                SortExpression="VarSupplierName" />
            <asp:BoundField DataField="VarOrganizationName" HeaderText="Company" 
                SortExpression="VarOrganizationName" />
            <asp:BoundField DataField="VarSupplierAddress" HeaderText="Address" 
                SortExpression="VarSupplierAddress" />
            <asp:BoundField DataField="VarPhoneNumber" HeaderText="Contact" 
                SortExpression="VarPhoneNumber" />
            <asp:BoundField DataField="varSupplierType" HeaderText="Type" 
                SortExpression="varSupplierType" />
        </Columns>
        <FooterStyle BackColor="#CCCCCC" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="#CCCCCC" />
    </asp:GridView>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
        
        SelectCommand="SELECT [VarSupplierName], [VarOrganizationName], [VarSupplierAddress], [VarPhoneNumber], [varSupplierType] FROM [Party] Where ProjectID=@ProjectID ORDER BY [VarSupplierName], [varSupplierType]">
        <SelectParameters>
            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" />
        </SelectParameters>
                                    </asp:SqlDataSource>



                      </div>
                   </div>
                        </div>
</div>
                </div>
</asp:Content>

