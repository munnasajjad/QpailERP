<%@ Page Title="Items Units" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Item-Unit.aspx.cs" Inherits="Operator_Item_Unit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">

    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>--%>
            
            <div class="row">
				<div class="col-md-12">					
					<h3 class="page-title">
					 Items Units <%--<small>form controls and more</small>--%>
					</h3>					
				</div>
			</div>
			<div class="row">


				<div class="col-md-6 ">
					<div class="portlet box blue">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> Menu Items Units Setup
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
							<%--<form role="form">--%>
								<div class="form-body">
										
										<asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="help-block" ></asp:Label>	

                                    <div class="form-group">
										<label>Unit Name: </label>
										<asp:TextBox ID="txtDept" runat="server" CssClass="form-control" placeholder="ie- KG, Pcs, cm, ltr etc." />									
									</div>

                                    <div class="form-group">
										<label>Unit Description: </label>
										<asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" placeholder="Description" />											
									</div>

                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" onclick="btnSave_Click1" />
                                        <asp:Button ID="btnClear" CssClass="btn default" runat="server" Text="Cancel" onclick="btnClear_Click1" /> 
								    </div>

                                </div>
							<%--</form>  --%>                         
            
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

  
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="UnitName">
                    <Columns>                       
                <asp:TemplateField ItemStyle-Width="40px">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>                 
                        <asp:BoundField DataField="UnitName" HeaderText="UnitName" SortExpression="UnitName" ReadOnly="True" />
                        <asp:BoundField DataField="Company" HeaderText="Description" 
                            SortExpression="Company" InsertVisible="False" ReadOnly="True" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                    
                                 SelectCommand="SELECT [Company], [UnitName] FROM [Units] ORDER BY [UnitName]"></asp:SqlDataSource>
            
            
                                </div>
               </div>

        </div>

    </div> 

                   
 </div>         

            
       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    

</asp:Content>


