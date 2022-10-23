<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="AccLink.aspx.cs" Inherits="Oxford.app.AccLink" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodycontent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <%--<asp:DropDownList name="" ID="DropDownList1" runat="server" 
                                            data-placeholder="--- Select ---" Style=" min-width: 150px"
                                             TabIndex="2">
                                            <asp:ListItem>Purchase</asp:ListItem>
                                            <asp:ListItem>Payment</asp:ListItem>
                                            <asp:ListItem>Adjustment</asp:ListItem>
                                        </asp:DropDownList>--%>

            <div class="grid_12 full_block">
                <div class="widget_wrap">
                    <div class="widget_top">
                        <span class="h_icon list_image"></span>
                        <h6>Fix Accounts Heads</h6>
                    </div>
                    <div class="widget_content">


                        <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                        <div class="form_container left_label field_set">

                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="TransactionType"
                                DataSourceID="SqlDataSource4" OnDataBound="GridView2_DataBound">

                                <Columns>
                                    <asp:TemplateField HeaderText="TID" SortExpression="TID" Visible="False">                                        
                                        <ItemTemplate>
                                            <asp:Label ID="lblTID" runat="server" Text='<%# Bind("TID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TransactionType" HeaderText="Transaction Type" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" ReadOnly="True" SortExpression="TransactionType" >
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="LinkAccountHeadID" SortExpression="LinkAccountHeadID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("HeadIdDr") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="500px" HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Link Account-Head(Dr.)" SortExpression="LinkAccountHeadName">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddAccHeadDr" runat="server" Width="100%"
                                                DataSourceID="SqlDataSource0" DataTextField="AccountsHeadName"
                                                DataValueField="AccountsHeadID">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource0" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                SelectCommand="SELECT AccountsHeadID, [AccountsHeadName] FROM [HeadSetup] ORDER BY [AccountsHeadName]"></asp:SqlDataSource>

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="LinkAccountHeadID" SortExpression="LinkAccountHeadID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2c" runat="server" Text='<%# Bind("HeadIdCr") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="500px" HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Link Account-Head(Cr.)" SortExpression="LinkAccountHeadName">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddAccHeadCr" runat="server" Width="100%"
                                                DataSourceID="SqlDataSource01" DataTextField="AccountsHeadName"
                                                DataValueField="AccountsHeadID">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource01" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                SelectCommand="SELECT AccountsHeadID, [AccountsHeadName] FROM [HeadSetup] ORDER BY [AccountsHeadName]"></asp:SqlDataSource>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UpdateDate" HeaderText="UpdateDate" SortExpression="UpdateDate" Visible="False" />
                                    <asp:BoundField DataField="UpdateBy" HeaderText="UpdateBy" SortExpression="UpdateBy" Visible="False" />
                                </Columns>
                            </asp:GridView>

                            <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                SelectCommand="SELECT TID, TransactionType, HeadIdDr, DrHeadName, HeadIdCr, CrHeadName, UpdateDate, UpdateBy FROM [AccLink] WHERE IsActive=1 order by TID"></asp:SqlDataSource>


                            <ul class="btn_area">
                                <li>
                                    <div class="form_grid_12">
                                        <div class="form_input">

                                            <asp:Button ID="btnSave" runat="server" Text="Save Accounts Setup"
                                                class="btn_small btn_blue" OnClick="btnSave_Click" />

                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


