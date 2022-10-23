<%@ Page Title="Items Groups" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Item-Group.aspx.cs" Inherits="Operator_Item_Group" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

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
                    <h3 class="page-title">Items Groups <%--<small>form controls and more</small>--%>
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 ">
                     <asp:HiddenField runat="server" ID="GroupIdHField"></asp:HiddenField>
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Items Group Setup
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass=""></asp:Label>

                                <div class="form-group">
                                    <label>Group Name: </label>
                                    <asp:TextBox ID="txtDept" runat="server" CssClass="form-control" EnableViewState="true" placeholder="Enter Item Group" />
                                </div>
                                <div class="form-group">
                                    <label class="control-label">Depreciation Type:</label>
                                    <asp:DropDownList ID="ddDepreciation" runat="server" CssClass="form-control select2me" AutoPostBack="True" OnSelectedIndexChanged="ddDepreciation_OnSelectedIndexChanged">
                                        <asp:ListItem Value="0">Not Applicable</asp:ListItem>
                                        <%--<asp:ListItem Value="1">Monthly</asp:ListItem>--%>
                                        <asp:ListItem Value="2">Yearly</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                               
                                <div class="form-group" runat="server" id="divCcontroller" >
                                    <label class="control-label">Control A/C For Depreciation: </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddControl" runat="server" DataTextField="name" DataValueField="ControlAccountsID" CssClass="form-control select2me" 
                                            DataSourceID="SqlDataSource3" AppendDataBoundItems="True" >
                                            <asp:ListItem>None</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT ControlAccountsID,(SELECT  AccountsName FROM Accounts WHERE (AccountsID = ControlAccount.AccountsID)) + ' > ' + ControlAccountsName AS name, ControlAccountsName FROM            ControlAccount
WHERE        (AccountsID = '0101') OR (AccountsID = '0102')
ORDER BY ControlAccountsID"></asp:SqlDataSource>
                                    </div>
                                </div>
                                 <div class="form-group" runat="server" id="divCcontroller2">
                                    <label class="control-label">Yearly Depreciation(%)</label>
                                    <asp:TextBox ID="txtDepreciationvalue" runat="server" CssClass="form-control" AutoPostBack="True" EnableViewState="true" placeholder="Enter Depreciation Rate" />
                                </div>
                                <div class="form-group">
                                    <label>Description: </label>
                                    <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" placeholder="Description" />
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
                                <i class="fa fa-reorder"></i>Saved Groups
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
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                    DataSourceID="SqlDataSource2" Width="100%" AllowSorting="True" DataKeyNames="GroupName">
                                    <Columns>
                                         <asp:TemplateField ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GroupId" SortExpression="GroupSrNo" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("GroupSrNo") %>'></asp:Label>
                                               
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="GroupName" HeaderText="Group Name"
                                            SortExpression="GroupName" ReadOnly="True" />
                                       

                                        <asp:BoundField DataField="ControlAccountsID" HeaderText="ControlAccountsID" SortExpression="ControlAccountsID" />
                                        <asp:BoundField DataField="Depreciationvalue" HeaderText="Yearly Depreciation(%)" SortExpression="Depreciationvalue" />
                                        <%--<asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />--%>

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
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT * FROM [ItemGroup]"></asp:SqlDataSource>
                                <asp:AccessDataSource ID="AccessDataSource1" runat="server"></asp:AccessDataSource>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [GroupSrNo], [GroupName], [ControlAccountsID], [Description], [Depreciationvalue],[ProjectID] FROM [ItemGroup] ORDER BY [GroupSrNo]"></asp:SqlDataSource>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


