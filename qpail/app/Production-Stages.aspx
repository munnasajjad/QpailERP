<%@ Page Title="Operations" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Production-Stages.aspx.cs" Inherits="app_Production_Stages" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    
    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>

    <asp:ScriptManager ID="ScriptManager5" runat="server"></asp:ScriptManager>
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">Operations
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Add New Operation (Production Stage)
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-body">
                                
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                
                                    <div id="EditField" runat="server">
                                        <label>Edit Info For: </label>
                                        <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" 
                                            DataTextField="DepartmentName" DataValueField="Departmentid" 
                                            AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [Departmentid], DepartmentName FROM [Satges] ORDER BY [DepartmentName]"></asp:SqlDataSource>
                                    </div>
                                
                                <div class="control-group">
                                    <label class="control-label">Section: </label>
                                    <asp:DropDownList ID="ddSection" runat="server" DataSourceID="SqlDataSource22"
                                        DataTextField="SName" DataValueField="SID" AutoPostBack="true" OnSelectedIndexChanged="ddSection_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource22" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [SID], [SName] FROM [Sections] WHERE ([DepartmentID] = @DepartmentID) and IsPrdSection='1' ORDER BY [SName]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="5" Name="DepartmentID" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Operation  Name: </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtDept" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                
                                <div class="control-group">
                                    <label class="control-label">Std. Production Per Hr. : </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtPrdHr" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                
                                    <div class="form-group">
                                        <label>Description: </label>
                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" placeholder="Description" />
                                    </div>

                                <div class="form-actions">
                                    <asp:CheckBox ID="chkFinal" runat="server" Text="Final Operation" Checked="false" />
                                    <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" OnClick="btnClear_Click" />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="lrlSavedBox" runat="server">Saved Operations</asp:Literal>
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
                                    DataSourceID="SqlDataSource1" Width="100%" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Departmentid" InsertVisible="False" SortExpression="Departmentid" Visible="False">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Departmentid") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Departmentid") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DepartmentName" HeaderText="Operation Name" SortExpression="DepartmentName" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [Departmentid], [DepartmentName], [Description] FROM [Satges]  WHERE Section=@Section  ORDER BY [DepartmentName]">
                                <SelectParameters>
                                        <asp:ControlParameter ControlID="ddSection" Name="Section" PropertyName="SelectedValue" />
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
