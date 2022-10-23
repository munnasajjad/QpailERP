<%@ Page Title="Permission Level" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Permission-Level.aspx.cs" Inherits="app_Permission_Level" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        img#ctl00_BodyContent_Image1 {
            margin-right: 10px;
        }

        td {
            vertical-align: middle;
        }
    </style>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
        });
    </script>

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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>


            <h3 class="page-title">Permission Level</h3>

            <div class="row">
                <div class="col-md-12">
                    <section class="panel">
<asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                        <%--<fieldset>
                        <legend>Daily Attendance</legend>--%>
                        <%--<table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="4">

                                </td>
                            </tr>

                            <tr id="EditField" runat="server">
                                <td style="text-align: right; vertical-align: middle;">Month: </td>
                                <td>
                                    <asp:DropDownList name="" ID="ddMonth" runat="server" Width="150px"
                                        DataSourceID="SqlDataSource2" DataTextField="MonthName" DataValueField="MonthID" CssClass="form-control col-lg-6"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddSection_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT MonthID, MonthName FROM [Month] order by MonthID"></asp:SqlDataSource>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtYear" runat="server" CssClass="form-control"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddSection_OnSelectedIndexChanged"></asp:TextBox>

                                </td>
                                <td style="text-align: right; vertical-align: middle; width: 73px;">Work Days: </td>
                                <td>
                                    <asp:TextBox ID="txtWorkDays" runat="server" CssClass="form-control"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddSection_OnSelectedIndexChanged"></asp:TextBox>

                                </td>
                                <td style="text-align: right; vertical-align: middle;">Department: </td>
                                <td>
                                    <asp:DropDownList name="" ID="ddClass" runat="server" AutoPostBack="true" Width="150px"
                                        DataSourceID="SqlDataSource1" DataTextField="DepartmentName" DataValueField="Departmentid" CssClass="form-control col-lg-6"
                                        OnSelectedIndexChanged="ddClass_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Departments] order by Departmentid"></asp:SqlDataSource>
                                </td>
                                <td style="text-align: right; vertical-align: middle;">Section: </td>
                                <td style="width: 100px;">
                                    <asp:DropDownList name="" ID="ddSection" runat="server" AutoPostBack="true" Width="150px"
                                        DataSourceID="SqlDataSource3" DataTextField="SName" DataValueField="SID" CssClass="form-control"
                                        OnSelectedIndexChanged="ddSection_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [SID], [SName] FROM [Sections] where DepartmentID=@Class AND IsPrdSection=0 order by SName">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddClass" Name="Class" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>

                                <td style="text-align: center;">
                                    <asp:Button ID="btnDefault" runat="server" Text="View Attendance" CssClass="btn btn-default" OnClick="btnDefault_Click" />
                                </td>
                            </tr>

                        </table>--%>

                        <div class="form_container left_label field_set">

                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="LevelID"
                                OnDataBound="GridView2_DataBound" CssClass="" AllowSorting="True">

                                <Columns>

                                    <asp:TemplateField ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="" SortExpression="TID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="LevelID" runat="server" Text='<%# Bind("LevelID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Role Name" SortExpression="EmployeeInfoID" >
                                        <ItemTemplate>
                                            <asp:Label ID="EmployeeInfoID" runat="server" Text='<%# Bind("LevelName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Insert" SortExpression="LinkAccountHeadID">
                                        <ItemTemplate>
                                            <asp:TextBox ID="CanInsert" runat="server" CssClass="text-center" Text='<%# Bind("CanInsert") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Edit Mode" SortExpression="RollNumber">
                                        <ItemTemplate>
                                            <asp:TextBox ID="CanRead" runat="server" CssClass="text-center" Text='<%# Bind("CanRead") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Update" SortExpression="RollNumber">
                                        <ItemTemplate>
                                            <asp:TextBox ID="CanUpdate" runat="server" CssClass="text-center" Text='<%# Bind("CanUpdate") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Delete" SortExpression="LinkAccountHeadID">
                                        <ItemTemplate>
                                            <asp:TextBox ID="CanDelete" runat="server" CssClass="text-center" Text='<%# Bind("CanDelete") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                SelectCommand="SELECT sl, EmployeeInfoID, IDCardNo, StudentNameE, FatherNameE, RollNumber, PhoneMobile, Section FROM [Students] WHERE Class=@Class
                                 AND Section=@Section AND IsActive=1 order by RollNumber, StudentNameE">
                                <%--<SelectParameters>
                                    <asp:ControlParameter ControlID="ddClass" Name="Class" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="ddSection" Name="Section" PropertyName="SelectedValue" />
                                </SelectParameters>--%>
                            </asp:SqlDataSource>

                            <div class="form_grid_12">


                                <div class="form_input right">
<span style="color: red">Value: <b>1= Granted, 0= Denied</b></span>
                                    <asp:Button ID="btnSave" runat="server" Text="Save Settings"
                                        class="btn btn-success btn_blue" OnClick="btnSave_Click" />

                                </div>
                            </div>
                        </div>



                    </section>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
