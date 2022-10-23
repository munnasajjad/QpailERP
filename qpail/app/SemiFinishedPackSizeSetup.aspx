<%@ Page Title="Semi Finished Pack Size Setup" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="SemiFinishedPackSizeSetup.aspx.cs" Inherits="app_SemiFinishedProductSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">Semi Finished Pack Size Setup<%--<small>form controls and more</small>--%>
                    </h3>
                </div>
            </div>
            <div class="row">

                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Semi Finished Pack Size Setup
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSave">
                                <div class="form-body">

                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                                    <div id="EditField" runat="server">
                                        <label>Edit Info For: </label>
                                        <asp:DropDownList ID="ddSemiFinishedPackSize" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" DataTextField="Size" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddSemiFinishedPackSize_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [Id], [Size] FROM [SemiFinishedPackSize] ORDER BY [Size] ASC"></asp:SqlDataSource>
                                    </div>

                                    <div class="form-group">
                                        <label>Pack Size : </label>
                                        <asp:TextBox ID="txtPackSize" runat="server" CssClass="form-control" EnableViewState="true" placeholder="Type/Name/Size" />
                                    </div>

                                    <div class="form-group">
                                        <label>Description: </label>
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" placeholder="Description" />
                                    </div>


                                    <%--<div id="divSerial" runat="server">
                                        <label>Serial: </label>
                                        <asp:TextBox ID="txtSerial" runat="server" CssClass="form-control" placeholder="Serial" />
                                    </div>--%>

                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click1" />
                                        <asp:Button ID="btnClear" CssClass="btn default" runat="server" Text="Cancel" OnClick="btnClear_Click1" />
                                    </div>

                                </div>
                            </asp:Panel>

                        </div>
                    </div>
                </div>
                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Saved Semi Finished Pack Size
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
                                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="Id">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL" ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                                <asp:Label ID="idLabel" Visible="False" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pack Size" SortExpression="Size">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Size") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" SortExpression="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="DisplaySl" SortExpression="DisplaySl">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2x" runat="server" Text='<%# Bind("DisplaySl") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [Id], [Size], [Description] FROM [SemiFinishedPackSize] ORDER BY Size ASC"
                                    UpdateCommand=""></asp:SqlDataSource>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>




