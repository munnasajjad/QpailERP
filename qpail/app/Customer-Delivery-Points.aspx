<%@ Page Title="Delivery Points" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Customer-Delivery-Points.aspx.cs" Inherits="app_Customer_Delivery_Points" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-title">Delivery Points
            </h3>
        </div>
    </div>
    <div class="row">


        <div class="col-md-6 ">
            <div class="portlet box red">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Product Delivery Locations
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

                        <asp:Label ID="lblMsg" runat="server" EnableViewState="false" ></asp:Label>

                        <div id="EditField" runat="server">
                            <label>Edit Info For: </label>
                            <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" DataTextField="DeliveryPointName" DataValueField="DeliveryPointsID" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT DeliveryPointsID, [DeliveryPointName] FROM [DeliveryPoints] ORDER BY [DeliveryPointName]"></asp:SqlDataSource>
                        </div>

                        
                        <div class="form-group">
							<label>Customer Company : </label>
                            <asp:DropDownList ID="ddCompany" CssClass="form-control select2me" runat="server"
                                 DataSourceID="SqlDataSource2" DataTextField="Company" DataValueField="PartyID"
                                AutoPostBack="True" OnSelectedIndexChanged="ddCompany_OnSelectedIndexChanged" >
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT PartyID, [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                </SelectParameters>
                            </asp:SqlDataSource>
						</div>


                        <div class="form-group">
                            <label>Delivery Point: </label>
                            <asp:TextBox ID="txtLocation" runat="server"  CssClass="form-control" ></asp:TextBox>
                        </div>
                        
                        <div class="control-group">
                            <asp:Label ID="Label4" runat="server"
                                Text="VAT Registration No : "></asp:Label>
                            <asp:TextBox ID="txtLandPhone" runat="server"></asp:TextBox>
                        </div>

                        <div id="ZoneField" runat="server" class="control-group">
                            <asp:Label ID="lblZone" runat="server" Text="Sales Zone :  "></asp:Label>
                            <asp:DropDownList ID="ddZone" runat="server"
                                DataSourceID="SqlDataSource4" DataTextField="AreaName"
                                DataValueField="AreaID">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                SelectCommand="SELECT AreaID, [AreaName] FROM [Areas] Where ProjectID=1 ORDER BY [AreaName]"></asp:SqlDataSource>
                        </div>

                        <div class="control-group">
                            <asp:Label ID="lblPermanent" runat="server" Text="Address : "></asp:Label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="" TextMode="MultiLine" Height="60px" />
                        </div>

                        <div class="control-group">
                            <asp:Label ID="Label13" runat="server" Text="Email :  "></asp:Label>
                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                        </div>

                        <div class="control-group">
                            <asp:Label ID="Label8" runat="server"
                                Text="Contact Person : "></asp:Label>
                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                        </div>

                        <div class="control-group">
                            <asp:Label ID="lblCno" runat="server" Text="Mobile No. :  "></asp:Label>
                            <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789-," TargetControlID="txtMobile">
                            </asp:FilteredTextBoxExtender>
                        </div>
                        
                        <div class="control-group">
                            <asp:Label ID="Label5" runat="server" Text="Transport Fare :  "></asp:Label>
                            <asp:TextBox ID="txtFare" Text="0" runat="server"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789-," TargetControlID="txtFare">
                            </asp:FilteredTextBoxExtender>
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
                    </div>
                </div>
                <div class="portlet-body form">

                    <div class="form-body">


                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting"
                            DataSourceID="SqlDataSource1" DataKeyNames="DeliveryPointsID" Width="100%">
                            <Columns>                
                <asp:TemplateField ItemStyle-Width="20px">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="DeliveryPointsID" SortExpression="DeliveryPointsID" Visible="false">                            
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("DeliveryPointsID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                <%--<asp:BoundField DataField="Company" HeaderText="Company" ReadOnly="True" SortExpression="Company" />--%>
                                <asp:BoundField DataField="DeliveryPointName" HeaderText="Delivery Point" SortExpression="DeliveryPointName" />
                                <asp:BoundField DataField="Zone" HeaderText="Zone" ReadOnly="True" SortExpression="Zone" />
                                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                            
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Select to Edit" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">Item will be deleted permanently!</b><br />
                                                    <br />
                                                    Are you sure to delete the item from list?
                                                            <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <div style="text-align: right;">
                                                        <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                        <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                    </div>
                                                </asp:Panel>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                            </Columns>
                        </asp:GridView>


                        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand="SELECT DeliveryPointsID, DeliveryPointName, (Select AreaName from Areas where AreaID=DeliveryPoints.ZoneID) as Zone, Address FROM [DeliveryPoints] WHERE CustomerID=@CustomerID ORDER BY [DeliveryPointName]"
                            DeleteCommand="Delete DeliveryPoints where DeliveryPointsID=0" >
                              <SelectParameters>
                                <asp:ControlParameter Name="CustomerID" ControlID="ddCompany" PropertyName="SelectedValue" />
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


