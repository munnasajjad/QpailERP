<%@ Page Title="System Settings" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="System-Settings.aspx.cs" Inherits="app_System_Settings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .portlet-body table tr:nth-child(odd) {
            background: #DDEBF7 !important;
        }

        td {
            vertical-align: middle !important;
        }

        table {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

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
                    <h3 class="page-title">System Settings</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Sales Settings
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>

                                <div class="control-group">
                                    <label class="control-label">Invoice Number Generation: </label>
                                    <div class="controls">
                                        <asp:RadioButton ID="rbAuto" runat="server" Text="Auto" GroupName="g" Checked="True" />
                                        <asp:RadioButton ID="rbManual" runat="server" Text="Manual" GroupName="g" />
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Report Header Templates: </label>
                                    <div class="controls">

                                        <asp:DropDownList ID="ddLevelX" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddLevelX_OnSelectedIndexChanged">
                                        </asp:DropDownList>

                                        <asp:SqlDataSource ID="SqlDataSource7p" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT LevelID, LevelName FROM [UserLevel] WHERE LevelID<@LevelID ORDER BY [LevelID]"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Template Company Name: </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtCompanyName" CssClass="span12 m-wrap" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Company Address: </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtAddress" CssClass="span12 m-wrap" runat="server" TextMode="MultiLine" Width="70%" Height="100px"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-actions">
                                    <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Update" OnClick="btnSave_OnClick" />
                                    <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" OnClick="btnClear_OnClick" />
                                </div>


                            </div>
                        </div>
                    </div>
                </div>


                <%--<div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="lrlSavedBox" runat="server">Saved Users</asp:Literal>
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

                                <asp:Label ID="lblid" runat="server" Visible="False"></asp:Label>

                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="LID">

                                    <Columns>

                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="sl" SortExpression="sl" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("LID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="LoginUserName" HeaderText="Login ID" SortExpression="LoginUserName" ReadOnly="True" />
                                        <asp:BoundField DataField="EmployeeInfoID" HeaderText="Employee ID" SortExpression="EmployeeInfoID" />
                                        <asp:BoundField DataField="UserLevel" HeaderText="Permission Level" SortExpression="UserLevel" />
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">This entry will be deleted permanently!</b><br />
                                                    Are you sure you want to delete this ?
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
                                    SelectCommand="SELECT LID, [LoginUserName], [EmployeeInfoID], (Select LevelName from userlevel where levelid= Logins.[UserLevel]) as UserLevel FROM [Logins] where LoginUserName<>'rony' ORDER BY [LoginUserName]"
                                    DeleteCommand="delete Logins where lid=0"></asp:SqlDataSource>

                                <asp:Label ID="txtCurrentPosition" runat="server" Text="" Visible="False"></asp:Label>

                            </div>
                        </div>
                    </div>
                </div>--%>
            </div>
            </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

