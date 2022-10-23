<%@ Page  Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Employee-Sectionas.aspx.cs" Inherits="app_Employee_Sectionas" %>
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
                    <h3 class="page-title">Employee Sections</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Employee Sections Setup
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-body">
                                
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                
                                    <div id="EditField" runat="server">
                                        <label>Edit Info For: </label>
                                        <asp:DropDownList ID="DropDownList1" CssClass="form-control" runat="server" DataSourceID="SqlDataSource2" 
                                            DataTextField="SName" DataValueField="SID" 
                                            AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [SName], [SID] FROM [Sections] ORDER BY [SName]"></asp:SqlDataSource>
                                    </div>
                                
                                <div class="control-group">
                                    <label class="control-label">Department Name: </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddDept" CssClass="form-control" runat="server" DataSourceID="SqlDataSource3" 
                                            DataTextField="DepartmentName" DataValueField="Departmentid" AutoPostBack="True" OnSelectedIndexChanged="ddDept_OnSelectedIndexChanged" >
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Departments] ORDER BY [DepartmentName]"></asp:SqlDataSource>
                                    </div>
                                </div>
                                
                                <div class="control-group">
                                    <label class="control-label">Section Name: </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtDept" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                
                                        <div class="control-group">
                                            <asp:Label ID="Label5" runat="server" Text="Rule Name: "></asp:Label>
                                            <asp:DropDownList ID="ddBasis" runat="server" DataSourceID="SqlDataSource5" DataTextField="GroupName" DataValueField="GroupSrNo">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [GroupName], [GroupSrNo] FROM [EmpTypes] ORDER BY [GroupSrNo]"></asp:SqlDataSource>
                                        </div>

                                <%--<asp:Panel ID="pnlMachine" runat="server" Visible="False">
                                <div class="control-group">
                                    <label class="control-label">Machine Section :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddSubGrp" runat="server" DataSourceID="SqlDataSource4" DataTextField="CategoryName" DataValueField="CategoryID" AppendDataBoundItems="True" >
                                            
                                            <asp:ListItem></asp:ListItem>
                                            
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [CategoryID], [CategoryName] FROM [ItemSubGroup] WHERE ([GroupID] = @GroupID) Order by CategoryName">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="5" Name="GroupID" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>
                                </asp:Panel>--%>
                                
                                

                                    <%--<div class="form-group">
                                        <label>Description: </label>
                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" placeholder="Description" />
                                    </div>--%>

                                <div class="form-actions">
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
                                <asp:Literal ID="lrlSavedBox" runat="server">Saved Sections</asp:Literal>
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
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="Dept" HeaderText="Department Name" SortExpression="DepartmentName" />
                                        <asp:BoundField DataField="SName" HeaderText="Section" SortExpression="Description" />
                                        <asp:BoundField DataField="SalaryRulesID" HeaderText="Duty Role" SortExpression="DepartmentName" />
                                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [SID], (Select DepartmentName FROM Departments where Departmentid= Sections.[DepartmentID]) as Dept, 
                                    (Select GroupName FROM EmpTypes where GroupSrNo= Sections.[SalaryRulesID]) as SalaryRulesID, [SName] FROM [Sections]  where DepartmentID=@DepartmentID AND IsPrdSection=0 ORDER BY [SName]">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddDept" Name="DepartmentID" PropertyName="SelectedValue" />
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
