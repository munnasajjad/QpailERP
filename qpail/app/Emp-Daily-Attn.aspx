<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Emp-Daily-Attn.aspx.cs" Inherits="app_Emp_Daily_Attn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        img#ctl00_BodyContent_Image1 {
            margin-right: 10px;
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


            <h3 class="page-title">Daily Attendance</h3>

            <div class="row">
                <div class="col-md-12">
                <section class="panel">

                    <%--<fieldset>
                        <legend>Daily Attendance</legend>--%>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                </td>
                            </tr>

                            <tr id="EditField" runat="server">
                                <td style="text-align: right; vertical-align: middle;">Date: </td>
                                <td>
                                     <asp:TextBox ID="txtDate" runat="server"  CssClass="form-control"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                        Enabled="True" TargetControlID="txtDate">
                                                    </asp:CalendarExtender>
                                </td>
                                <td style="text-align: right; vertical-align: middle;">Department: </td>
                                <td>
                                    <asp:DropDownList name="" ID="ddClass" runat="server" AutoPostBack="true"  Width="200px"
                                        DataSourceID="SqlDataSource1" DataTextField="DepartmentName" DataValueField="Departmentid" CssClass="form-control col-lg-6"
                                        OnSelectedIndexChanged="ddClass_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Departments] order by Departmentid"></asp:SqlDataSource>
                                </td>
                                <td style="text-align: right; vertical-align: middle;">Section: </td>
                                <td style="width: 100px;">
                                    <asp:DropDownList name="" ID="ddSection" runat="server" AutoPostBack="true" Width="200px"
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
                                    <%--<asp:TextBox ID="txtSession" runat="server" AutoPostBack="true" OnTextChanged="ddClass_SelectedIndexChanged" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtSession1"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtSession">
                                    </asp:FilteredTextBoxExtender>--%>
                                </td>
                                
                                <td style="text-align: center;"> <asp:Button ID="btnDefault" runat="server" Text="View Attendance" CssClass="btn btn-default" OnClick="btnDefault_Click" /> </td>
                            </tr>

                        </table>

                        <div class="form_container left_label field_set">

                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="sl"
                                 OnDataBound="GridView2_DataBound" CssClass="" AllowSorting="True">

                                <Columns>

                                    <asp:TemplateField ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="" SortExpression="TID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTID" runat="server" Text='<%# Bind("sl") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="" SortExpression="EmployeeInfoID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="EmployeeInfoID" runat="server" Text='<%# Bind("EmployeeInfoID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:BoundField DataField="EmpSerial" HeaderText="Code" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" ReadOnly="True" SortExpression="TransactionType">
                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="EName" HeaderText="Employee Name" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" ReadOnly="True" SortExpression="TransactionType">
                                        <ItemStyle HorizontalAlign="Center" Width="40%" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="In Time" SortExpression="RollNumber">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtInTime" runat="server" CssClass="text-center" Text='<%# Bind("InTime") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Out Time" SortExpression="LinkAccountHeadID" >
                                        <ItemTemplate>
                                            <asp:TextBox ID="lblOutTime" runat="server" CssClass="text-center" Text='<%# Bind("OutTime") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Status" SortExpression="LinkAccountHeadName">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddStatus" runat="server" Width="100%"
                                                DataSourceID="SqlDataSource01" DataTextField="AttnName" 
                                                DataValueField="AttnValue"  AppendDataBoundItems="True"  SelectedValue='<%# Bind("Status") %>' >
                                                <asp:ListItem Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource01" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                SelectCommand="SELECT  AttnName, AttnValue FROM [AttnTypes]  ORDER BY [sl]">
                                                <%--<SelectParameters>
                                                    <asp:ControlParameter ControlID="ddClass" Name="Class" PropertyName="SelectedValue" />
                                                </SelectParameters>--%>
                                            </asp:SqlDataSource>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                SelectCommand="SELECT sl, EmployeeInfoID, IDCardNo, StudentNameE, FatherNameE, RollNumber, PhoneMobile, Section FROM [Students] WHERE Class=@Class
                                 AND Section=@Section AND IsActive=1 order by RollNumber, StudentNameE">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddClass" Name="Class" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="ddSection" Name="Section" PropertyName="SelectedValue" />
                                </SelectParameters>
                            </asp:SqlDataSource>

                            <div class="form_grid_12">
                                <div class="form_input right">

                                    <asp:Button ID="btnSave" runat="server" Text="Save Attendance"
                                        class="btn btn-success btn_blue" OnClick="btnSave_Click" />

                                </div>
                            </div>
                        </div>


                    </fieldset>


                </section>
            </div>



        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
