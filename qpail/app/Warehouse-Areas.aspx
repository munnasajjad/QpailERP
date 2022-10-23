<%@ Page Title="WAREHOUSE AREAS" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Warehouse-Areas.aspx.cs" Inherits="app_Warehouse_Areas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false" ></asp:Label>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <div class="row">
				<div class="col-md-12">
					<h3 class="page-title">
					 WAREHOUSE AREAS
					</h3>
				</div>
			</div>
			<div class="row">


				<div class="col-md-6 ">
					<div class="portlet box red">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> Warehouse Areas Setup
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

										<asp:Label ID="lblMsg" runat="server" EnableViewState="false"  ></asp:Label>

                                    <div id="EditField" runat="server">
										<label>Edit Info For: </label>
                                        <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" DataTextField="AreaName" DataValueField="AreaName" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT AreaID, [AreaName] FROM [WareHouseAreas] ORDER BY [AreaName]"></asp:SqlDataSource>
									</div>

                                    <div class="form-group">
										<label>Warehouse: </label>
                                        <asp:DropDownList ID="ddWarehouse" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource2"
                                            DataTextField="StoreName" DataValueField="WID" AutoPostBack="True" OnSelectedIndexChanged="ddWarehouse_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT WID, [StoreName] FROM [Warehouses]  where ProjectID=1 ORDER BY [StoreName]"></asp:SqlDataSource>
									</div>


                                    <div class="form-group">
										<label>Area Name: </label>
										<asp:TextBox ID="txtDept" runat="server" CssClass="form-control" EnableViewState="true" placeholder="Area Name" />
									</div>

                                    <div class="form-group">
										<label> Description: </label>
										<asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" placeholder="Description" />
									</div>

                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" onclick="btnSave_Click1" />
                                        <asp:Button ID="btnClear" CssClass="btn default" runat="server" Text="Cancel" onclick="btnClear_Click1" />
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
                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="AreaID">
                    <Columns>
                <asp:TemplateField ItemStyle-Width="40px">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                        <asp:TemplateField HeaderText="Warehouse" SortExpression="Warehouse" >
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Warehouse") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Area Name" SortExpression="AreaName">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("AreaName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" SortExpression="Description">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                    </Columns>
                </asp:GridView>


                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                    SelectCommand="SELECT AreaID, (Select StoreName from Warehouses WHERE WID=WareHouseAreas.Warehouse) AS Warehouse, [AreaName], [Description] FROM [WareHouseAreas] WHERE Warehouse=@Warehouse ORDER BY  Warehouse, [AreaName]">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddWarehouse" Name="Warehouse" PropertyName="SelectedValue" />
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


