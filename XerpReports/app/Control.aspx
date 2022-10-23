<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="Control.aspx.cs" Inherits="Oxford.app.Control" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="upnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>


            <div class="container-fluid">
                <div class="row-fluid">
                    <div class="span12">
                        <h3 class="page-title">Control Accounts
                     <%--<small>form components and widgets</small>--%>
                        </h3>
                    </div>
                </div>
<div class="grid_6 full_block">
                <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORM PORTLET-->
                        <div class="portlet box blue">
                            <div class="portlet-title">
                                <h4><i class="icon-reorder"></i>Control Accounts Setup</h4>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                    <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                    <a href="javascript:;" class="reload"></a>
                                    <a href="javascript:;" class="remove"></a>
                                </div>
                            </div>

                            <asp:Label ID="lblMsg" runat="server" EnableViewState="false" ></asp:Label>

                            <div class="portlet-body form">
                                <div class="form-horizontal">

                                    <div class="control-group">
                                        <label class="control-label">Sub A/C Name : </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddSub" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddSub_SelectedIndexChanged">
                                            </asp:DropDownList>

                                            <asp:Label ID="SubID" runat="server"></asp:Label>

                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Control A/C ID :</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtNid" runat="server" Enabled="false" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                                            <asp:Label ID="lblOldSubAcId" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Control A/C Name</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtControl" runat="server" title="Write Sub-Account Name and press Enter" CssClass="span6 m-wrap"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save Control Head" OnClick="btnSave_Click" />
                                        <%--<asp:Button ID="btnDelete" runat="server" Text="Delete This Schedule"  CssClass="btn red" 
                                onclick="btnDelete_Click" />
                                <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnDelete"  ConfirmText="confirm to delete this schedule ??"> 
                                 </asp:ConfirmButtonExtender> --%>
                                        <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" />
                                    </div>

                                </div>

                            </div>

                        </div>

                    </div>
                    </div>
                    </div>

                    
<div class="grid_6 full_block">

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

                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"
                                        OnRowDeleting="GridView2_RowDeleting" OnSelectedIndexChanged="GridView2_SelectedIndexChanged"
                                        DataSourceID="SqlDataSource1">
                                        <Columns>
                                            
                                <asp:TemplateField HeaderText="SrNo" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                </asp:TemplateField>

                                            <asp:TemplateField HeaderText="C.Accounts ID" SortExpression="ControlAccountsID">                                                
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("ControlAccountsID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AccountsName" HeaderText="Accounts Name"
                                                SortExpression="AccountsName" />
                                            <asp:BoundField DataField="ControlAccountsName" HeaderText="Control Accounts Name"
                                                SortExpression="ControlAccountsName" />

                                            
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">A/C Head will be deleted permanently!</b><br />
                                                    Are you sure you want to delete the item from order list?
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
                                    <br />

                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                        SelectCommand="SELECT a.ControlAccountsID, b.AccountsName, a.ControlAccountsName FROM ControlAccount a join Accounts b on a.AccountsID=b.AccountsID order by a.AccountsID"></asp:SqlDataSource>

                                </div>
                            </div>
                        </div>
                    </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

