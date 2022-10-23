<%@ Page Title="WAREHOUSES" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Warehouses.aspx.cs" Inherits="Operator_Warehouses" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">WAREHOUSES
                    </h3>
                </div>
            </div>
            <div class="row">


                <div class="col-md-6 ">
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Store/Warehouses Setup
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                                <div id="EditField" runat="server">
                                    <label>Edit Info For: </label>
                                    <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" DataTextField="StoreName" DataValueField="StoreName" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [StoreName] FROM [Warehouses] ORDER BY [StoreName]"></asp:SqlDataSource>
                                </div>

                                <div class="form-group">
                                    <label>Zone: </label>
                                    <asp:DropDownList ID="ddZone" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource2" DataTextField="AreaName" DataValueField="AreaName">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT DISTINCT [Country], [AreaName] FROM [Areas] WHERE ([AreaType] = 'sales') ORDER BY [AreaName]"></asp:SqlDataSource>
                                </div>

                                <div class="form-group">
                                    <label>Warehouse Name: </label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" EnableViewState="true" placeholder="Warehouse Name" />
                                </div>

                                <div class="form-group">
                                    <label>Location/Address: </label>
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Location" TextMode="MultiLine" />
                                </div>

                                <div class="form-group">
                                    <label>Owner Info: </label>
                                    <asp:TextBox ID="txtOwner" runat="server" CssClass="form-control" placeholder="Owner Info" TextMode="MultiLine" />
                                </div>

                                <div class="form-group">
                                    <label>Guards Info/Remarks </label>
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" placeholder="" TextMode="MultiLine" />
                                </div>

                                <div class="form-actions">
                                    <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click1" />
                                    <asp:Button ID="btnClear" CssClass="btn default" runat="server" Text="Cancel" OnClick="btnClear_Click1" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>




                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Saved Data
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                                r
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

                                <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="WID">
                                    <Columns>
                                        <asp:BoundField DataField="WID" HeaderText=" "
                                            SortExpression="WID" InsertVisible="False" ReadOnly="True" Visible="false" />
                                        <asp:BoundField DataField="Zone" HeaderText="Zone" SortExpression="Zone" />
                                        <asp:BoundField DataField="StoreName" HeaderText="Warehouse Name" SortExpression="Store Name" />
                                        <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                                        <asp:BoundField DataField="Owner" HeaderText="Owner" SortExpression="Owner" />
                                        <asp:BoundField DataField="Description" HeaderText="Remarks" SortExpression="Description" />

                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [WID], Zone, [StoreName], Address, Owner, Description, [ProjectID] FROM [Warehouses] Where ProjectID=@ProjectID AND IsSisterConcern<>'1' ORDER BY [WID]">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" />
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


