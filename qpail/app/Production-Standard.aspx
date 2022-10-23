
<%@ Page Title="Production Standard" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Production-Standard.aspx.cs" Inherits="app_Production_Standard" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false" ></asp:Label>	

    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>

            
            <div class="row">
				<div class="col-md-12">					
					<h3 class="page-title">
					  Production Standard
					</h3>					
				</div>
			</div>
			<div class="row">


				<div class="col-md-6 ">
					<div class="portlet box red">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> Production Standard Setup
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
										
										<asp:Label ID="lblMsg" runat="server" EnableViewState="false" ></asp:Label>	
                                    <asp:Label ID="lblEntryId" runat="server" Visible="false" ></asp:Label>	
                                                                        
                                    <div id="EditField" runat="server">
										<label>Edit Info For: </label>
                                        <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" DataTextField="ItemName" DataValueField="ProductID" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [ProductID], ItemName FROM [Products] ORDER BY [ItemName]"></asp:SqlDataSource>
									</div>
                                                                        
                                    <div class="form-group">
										<label>Section : </label>
                                        <asp:DropDownList ID="ddSection" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource4"
                                        DataTextField="SName" DataValueField="SID" AutoPostBack="True" OnSelectedIndexChanged="ddSection_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [SID], [SName] FROM [Sections] WHERE ([DepartmentID] = @DepartmentID) ORDER BY [SName]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="5" Name="DepartmentID" Type="String" />
                                        </SelectParameters>
                                        </asp:SqlDataSource>
									</div>
                                    
                                    <div class="form-group">
										<label>Machine No. : </label>
                                        <asp:DropDownList ID="ddMachine" CssClass="form-control select2me" runat="server" 
                                            AutoPostBack="True" OnSelectedIndexChanged="ddSubGrp_SelectedIndexChanged" DataSourceID="SqlDataSource2"
                                        DataTextField="MachineNo" DataValueField="mid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [MachineNo], [mid] FROM [Machines] WHERE ([Section] = @Section)">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddSection" Name="Section" PropertyName="SelectedValue" Type="String" />
                                        </SelectParameters>
                                        </asp:SqlDataSource>
									</div>
                                    
                                 <div class="form-group">
                                    <label>For Company:  </label>
                                    <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource9" CssClass="form-control select2me" 
                                        DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddCustomer_OnSelectedIndexChanged">
                                        <asp:ListItem Value="">--- all ---</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource9" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                    <div class="form-group">
										<label>Sub-Group : </label>
                                        <asp:DropDownList ID="ddSubGrp" CssClass="form-control select2me" runat="server" 
                                            AutoPostBack="true" OnSelectedIndexChanged="ddSubGrp_OnSelectedIndexChanged" DataSourceID="SqlDataSource7n"
                                        DataTextField="CategoryName" DataValueField="CategoryID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource7n" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID='3' ORDER BY [CategoryName]"></asp:SqlDataSource>
                                                                             
									</div>
                                    
                                    <asp:Panel id="gcPanel" runat="server" Visible="True">
                                    <div class="form-group">
										<label>Item Grade : </label>
                                        <asp:DropDownList ID="ddGrade" CssClass="form-control select2me" runat="server" 
                                            AutoPostBack="true" OnSelectedIndexChanged="ddGrade_SelectedIndexChanged" DataSourceID="SqlDataSource6"
                                        DataTextField="GradeName" DataValueField="GradeID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource6" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID=@CategoryID ORDER BY [GradeName]">
                                         <SelectParameters>
                                            <asp:ControlParameter ControlID="ddSubGrp" Name="CategoryID" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                        </asp:SqlDataSource>
									</div>
                                    
                                    <div class="form-group">
										<label>Item Category : </label>
										<asp:DropDownList ID="ddcategory" CssClass="form-control select2me" runat="server" 
                                            AutoPostBack="True" OnSelectedIndexChanged="ddcategory_SelectedIndexChanged" DataSourceID="SqlDataSource5"
                                        DataTextField="CategoryName" DataValueField="CategoryID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource5" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT CategoryID, CategoryName FROM [Categories] where GradeID=@GradeID ORDER BY [CategoryName]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddGrade" Name="GradeID" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                        </asp:SqlDataSource>
									</div>
                                    </asp:Panel>
                                    

                                    <div class="form-group">
										<label>Pack Size : </label>
                                        <asp:DropDownList ID="ddPackSize" CssClass="form-control select2me" runat="server" 
                                            DataSourceID="SqlDataSource8" DataTextField="BrandName" DataValueField="BrandID"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddPackSize_OnSelectedIndexChanged" >
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource8" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] ORDER BY [DisplaySl]"></asp:SqlDataSource>
                                                                             
									</div>
                                    
                                    <div class="form-group">
										<label>Operation : </label>
                                        <asp:DropDownList ID="ddOperation" CssClass="form-control select2me" runat="server" 
                                            AutoPostBack="true" OnSelectedIndexChanged="ddOperation_OnSelectedIndexChanged" DataSourceID="SqlDataSource7"
                                        DataTextField="DepartmentName" DataValueField="Departmentid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                                       
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Satges] WHERE ([Section] = @Section)">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddSection" Name="Section" PropertyName="SelectedValue" Type="String" />
                                        </SelectParameters>
                                        </asp:SqlDataSource>
									</div>

                                                                        
                                    <div class="form-group">
										<label>Std. Production Per Hour: </label>
										<asp:TextBox ID="txtStdPrdn" runat="server" CssClass="form-control" EnableViewState="true" placeholder="" />									
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                runat="server" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtStdPrdn">
                                            </asp:FilteredTextBoxExtender>
									</div>
                                    
                                    <div class="form-group">
										<label>Remarks : </label>
										<asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" placeholder="" />											
									</div>
                                    
                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" onclick="btnSave_Click1" />
                                        <asp:Button ID="btnClear" CssClass="btn default" runat="server" Text="Cancel" onclick="btnClear_Click1" Visible="False" /> 
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

  
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                    DataSourceID="SqlDataSource1" Width="100%">
                    <Columns>                
                        <%--<asp:BoundField DataField="spid" HeaderText="spid" InsertVisible="False" ReadOnly="True" SortExpression="spid" />--%>
                        <asp:BoundField DataField="Section" HeaderText="Section" SortExpression="Section" />
                        <asp:BoundField DataField="MachineNo" HeaderText="Machine" SortExpression="MachineNo" />
                        <asp:BoundField DataField="ItemGrade" HeaderText="Grade" SortExpression="ItemGrade" />
                        <asp:BoundField DataField="ItemCategory" HeaderText="Category" SortExpression="ItemCategory" />
                        <asp:BoundField DataField="PackSize" HeaderText="Pk Size" SortExpression="PackSize" />
                        <asp:BoundField DataField="Operation" HeaderText="Operation" SortExpression="Operation" />
                        <asp:BoundField DataField="StdPrdn" HeaderText="Std.Prdn" SortExpression="StdPrdn" />
                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
                    </Columns>
                </asp:GridView>


                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:Connection_String %>"                     
                    SelectCommand="SELECT [spid], (SELECT [SName] FROM [Sections] WHERE SID=a.Section) AS [Section], 
                    (SELECT [MachineNo] FROM [Machines] WHERE mid=a.MachineNo) AS [MachineNo], 
                    (SELECT [GradeName] FROM [ItemGrade] WHERE GradeID=a.ItemGrade) AS [ItemGrade], 
                    (SELECT [CategoryName] FROM [Categories] WHERE CategoryID=a.ItemCategory) AS [ItemCategory], 
                    (SELECT [BrandName] FROM [Brands] WHERE BrandID=a.PackSize) AS [PackSize], 
                    (SELECT [DepartmentName] FROM [Satges] WHERE Departmentid=a.Operation) AS [Operation], [StdPrdn], [Remarks] FROM [ProductionStandard] a
                    where Section=@Section AND MachineNo=@MachineNo AND Company=@Company AND PackSize=@PackSize AND  Operation=@Operation ORDER BY [spid]">  
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddSection" Name="Section" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ddMachine" Name="MachineNo" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ddCustomer" Name="Company" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ddPackSize" Name="PackSize" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ddOperation" Name="Operation" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            
            
                                </div>
               </div>

        </div>

    </div> 

                   
 </div>         

            
        </ContentTemplate>
    </asp:UpdatePanel>
    

</asp:Content>


