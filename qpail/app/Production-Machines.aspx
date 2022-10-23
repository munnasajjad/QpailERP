
<%@ Page Title="Machines" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Production-Machines.aspx.cs" Inherits="app_Production_Machines" %>
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
                    <h3 class="page-title">Machines
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Add Machine for Production
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                                    <div id="EditField" runat="server">
                                        <label>Edit Info For: </label>
                                        <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3"
                                            DataTextField="Machine" DataValueField="mid"
                                            AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [mid], Machine FROM [Machines] ORDER BY [Machine]"></asp:SqlDataSource>
                                    </div>


                                <div class="control-group">
                                    <label class="control-label">Section: </label>
                                    <asp:DropDownList ID="ddSection" runat="server" DataSourceID="SqlDataSource22"
                                        DataTextField="SName" DataValueField="SID" AutoPostBack="true" OnSelectedIndexChanged="ddSection_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource22" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [SID], [SName] FROM [Sections] WHERE ([DepartmentID] = @DepartmentID) AND IsPrdSection=1 ORDER BY [SName]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="5" Name="DepartmentID" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <%--<div class="control-group">
                                    <label class="control-label">Machine Name: </label>
                                    <asp:DropDownList ID="ddMachine" runat="server" DataTextField="Machine" DataValueField="ProductID"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddMachine_OnSelectedIndexChanged" >
                                    </asp:DropDownList>
                                    <span style="width: 70%; color: green; float: right">
                                            <asp:Literal ID="ltrLastInfo" runat="server" ></asp:Literal>
                                        </span>
                                </div>--%>


                                <div class="control-group">
                                    <label class="control-label">Machine No.: </label>
                                    <div class="controls">
                                      <%--<span style="width: 10%; font-weight: bold;text-align: center">MC# </span> --%>
                                        <asp:TextBox ID="txtDept" CssClass="span6 m-wrap" Width="60%" runat="server"></asp:TextBox>
                                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtDept">
                                    </asp:FilteredTextBoxExtender>--%>
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
                                <asp:Literal ID="lrlSavedBox" runat="server">Saved Departments</asp:Literal>
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
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("mid") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="MachineName" HeaderText="Machine Name" SortExpression="DepartmentName" />--%>
                                        <asp:BoundField DataField="MachineNo" HeaderText="Machine No." SortExpression="DepartmentName" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [mid], [Machine], MachineName, MachineNo, [Description] FROM [Machines] WHERE Section=@Section ORDER BY [MachineNo]">
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
