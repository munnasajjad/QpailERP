<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Accounts-Group.aspx.cs" Inherits="app_Accounts_Group" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">Items Groups <%--<small>form controls and more</small>--%>
                    </h3>
                </div>
            </div>
            <div class="row">


                <div class="col-md-6">
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
                            
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">


                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="GroupName" HeaderText="Group Name"
                                            SortExpression="GroupName" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                        <asp:BoundField DataField="ProjectID" HeaderText="P.ID" SortExpression="ProjectID" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [GroupName], [Description], [ProjectID] FROM [ItemGroup] ORDER BY [GroupName]"></asp:SqlDataSource>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>


