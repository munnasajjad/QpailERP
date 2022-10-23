<%@ Page Title="Production Sections" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Production-Sections.aspx.cs" Inherits="app_Production_Sections" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>


            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">Production Sections</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Production Sections Setup
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

                                <div class="control-group hidden">
                                    <label class="control-label">Department Name: </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddDept" CssClass="form-control" runat="server" DataSourceID="SqlDataSource3"
                                            DataTextField="DepartmentName" DataValueField="Departmentid" AutoPostBack="True" OnSelectedIndexChanged="ddDept_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Departments] where Departmentid=5 ORDER BY [DepartmentName]"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Production Section Name</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtDept" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                
                                <legend>Stock Location</legend>
                                <div class="control-group">
                                    <label class="control-label">Godown : </label>
                                    <asp:DropDownList ID="ddGodown" runat="server" DataSourceID="SqlDataSource6"
                                        DataTextField="StoreName" DataValueField="WID" CssClass="form-control"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddGodown_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource6" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [WID], [StoreName] FROM [Warehouses]"></asp:SqlDataSource>
                                </div>
                                
                                <div class="control-group">
                                    <label class="control-label">Location : </label>
                                    <asp:DropDownList ID="ddLocation" runat="server" DataSourceID="SqlDataSource15"
                                        DataTextField="AreaName" DataValueField="AreaID" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource15" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT AreaID,[AreaName] FROM [WareHouseAreas] where Warehouse=@Warehouse">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddGodown" Name="Warehouse" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                
                                <legend>Wastage Stock Location</legend>
                                <div class="control-group">
                                    <label class="control-label">Godown : </label>
                                    <asp:DropDownList ID="ddWastageGodown" runat="server" DataSourceID="SqlDataSource5"
                                        DataTextField="StoreName" DataValueField="WID" CssClass="form-control"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddGodown_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource5" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [WID], [StoreName] FROM [Warehouses]"></asp:SqlDataSource>
                                </div>
                                
                                <div class="control-group">
                                    <label class="control-label">Location : </label>
                                    <asp:DropDownList ID="ddWastageLocation" runat="server" DataSourceID="SqlDataSource7"
                                        DataTextField="AreaName" DataValueField="AreaID" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT AreaID,[AreaName] FROM [WareHouseAreas] where Warehouse=@Warehouse">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddWastageGodown" Name="Warehouse" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <%--<div class="control-group">
                                            <asp:Label ID="Label5" runat="server" Text="Salary Basis: "></asp:Label>
                                            <asp:DropDownList ID="ddBasis" runat="server" DataSourceID="SqlDataSource5" DataTextField="GroupName" DataValueField="GroupSrNo">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [GroupName], [GroupSrNo] FROM [EmpTypes] ORDER BY [GroupSrNo]"></asp:SqlDataSource>
                                        </div>--%>

                                <legend>Relation with Machine Inventory</legend>

                                <asp:Panel ID="pnlMachine" runat="server">
                                    <div class="form-group">
                                        <label class="control-label">Subgroup:</label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddSubGrp" runat="server" DataSourceID="SqlDataSource4" AutoPostBack="True" OnSelectedIndexChanged="ddSubGrp_OnSelectedIndexChanged"
                                                CssClass="form-control select2me" DataTextField="CategoryName" DataValueField="CategoryID">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [CategoryID], [CategoryName] FROM [ItemSubGroup] WHERE ([GroupID] = '5') Order by CategoryName">
                                                <SelectParameters>
                                                    <asp:Parameter DefaultValue="5" Name="GroupID" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label>Grade : </label>
                                        <asp:DropDownList ID="ddGrade" CssClass="form-control select2me" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddGrade_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="form-group">
                                        <label>Category : </label>
                                        <asp:DropDownList ID="ddcategory" CssClass="form-control select2me" runat="server"
                                            OnSelectedIndexChanged="ddcategory_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>

                                </asp:Panel>



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

                                        <asp:BoundField DataField="Dept" HeaderText="Department Name" SortExpression="DepartmentName" Visible="False" />
                                        <asp:BoundField DataField="SName" HeaderText="Section" SortExpression="Description" />
                                        <asp:BoundField DataField="MachineLinkItemSubgroupID" HeaderText="Sub-Group" SortExpression="DepartmentName" />
                                        <asp:BoundField DataField="GradeId" HeaderText="Grade" SortExpression="DepartmentName" />
                                        <asp:BoundField DataField="CategoryId" HeaderText="Category" SortExpression="DepartmentName" />
                                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [SID], (Select DepartmentName FROM Departments where Departmentid= Sections.[DepartmentID]) as Dept, 
                                      (Select CategoryName FROM ItemSubGroup where CategoryID= Sections.[MachineLinkItemSubgroupID]) as MachineLinkItemSubgroupID, 
                                    (Select GradeName FROM ItemGrade where GradeID= Sections.[GradeId]) as GradeId, 
                                    (Select CategoryName FROM Categories where CategoryID= Sections.[CategoryId]) as CategoryId, 
                                    [SName] FROM [Sections]  where DepartmentID=@DepartmentID  AND IsPrdSection=1 ORDER BY [SName]">
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
