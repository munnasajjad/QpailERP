<%@ Page Title="Duty Shifts" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="hrm-shift.aspx.cs" Inherits="app_hrm_shift" %>

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
                    <h3 class="page-title">Duty Shifts
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Add New Duty Shifts
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                                <div id="EditField" runat="server">
                                    <label>Edit Info For: </label>
                                    <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3"
                                        DataTextField="ShiftName" DataValueField="sid"
                                        AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sid], ShiftName FROM [EmpShifts] ORDER BY [ShiftName]"></asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Rule Name: </label>
                                    <asp:DropDownList ID="ddSection" runat="server" DataSourceID="SqlDataSource22"
                                        DataTextField="GroupName" DataValueField="GroupSrNo" AutoPostBack="true" 
                                        OnSelectedIndexChanged="ddSection_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource22" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [GroupName], GroupSrNo FROM [EmpTypes] WHERE IsShiftingDuty=1 ORDER BY [GroupName]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="5" Name="DepartmentID" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Shift Name: </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtDept" CssClass="span6 m-wrap" runat="server" placeholder="Shift Name"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label>Timing: </label>
                                    <div class="controls">
                                    <asp:TextBox ID="txtCheckIn" runat="server" CssClass="" Width="34%" placeholder="From (Check-in Time)" />
                                    <asp:TextBox ID="txtCheckOut" runat="server" CssClass="" Width="35%" placeholder="To (Check-out Time)" />
                                </div></div>

                                <div class="form-group">
                                    <label>Remarks: </label>
                                    <div class="controls">
                                    <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" placeholder="" />
                                </div></div>

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

                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="sid"
                                    DataSourceID="SqlDataSource1" Width="100%" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Departmentid" InsertVisible="False" SortExpression="Departmentid" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("sid") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ShiftName" HeaderText="Shifts Name" SortExpression="DepartmentName" />
                                        <asp:BoundField DataField="InTime" HeaderText="In-Time" SortExpression="Description" DataFormatString="{0:hh:mm tt}"  />
                                        <asp:BoundField DataField="OutTime" HeaderText="Out-Time" SortExpression="Description" DataFormatString="{0:hh:mm tt}"  />
                                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT sid, [ShiftName], [InTime], [OutTime] FROM [EmpShifts]  WHERE GroupSrNo=@Section  ORDER BY [sid]">
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
