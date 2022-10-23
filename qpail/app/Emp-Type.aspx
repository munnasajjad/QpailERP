<%@ Page Title="Employee Rules & Regulations" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Emp-Type.aspx.cs" Inherits="app_Emp_Type" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">        </asp:ScriptManager>

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
                    <h3 class="page-title">Employee Rules & Regulations</h3>
                </div>
            </div>
            <div class="row">


                <div class="col-md-6 ">
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Duty Type & Salary Basis Setup
                            </div>
                            
                        </div>
                        <div class="portlet-body form">
                            
                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass=""></asp:Label>
                                <asp:Label ID="lblId" runat="server" Visible="False"></asp:Label>
                                
                                        <div class="control-group">
                                            <asp:Label ID="Label5" runat="server" Text="Duty Group: "></asp:Label>
                                            <asp:DropDownList ID="ddBasis" runat="server">
                                                <asp:ListItem>General Duty</asp:ListItem>
                                                <asp:ListItem>Supervisors</asp:ListItem>
                                                <asp:ListItem>Workers</asp:ListItem>
                                                <asp:ListItem>Driver & Guard</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                
                                <div class="form-group">
                                    <label>Rule Name: </label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" EnableViewState="true" placeholder="Enter Rule Name" />
                                </div>

                                <div class="form-group hidden">
                                    <label>Description: </label>
                                    <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" placeholder="Description" />
                                </div>
                                
                                <div class="form-group">
                                    <label>Duty Hour: </label>
                                    <asp:TextBox ID="txtGeneralHour" runat="server" CssClass="form-control" Width="34%" placeholder="General Duty Hour" />
                                    <asp:TextBox ID="txtMinHour" runat="server" CssClass="form-control" Width="35%" placeholder="Minimum Duty Hour" />
                                    </div>

                                <div class="form-group">
                                    <label>Timing: </label>
                                    <asp:TextBox ID="txtCheckIn" runat="server" CssClass="form-control" Width="34%" placeholder="From (Check-in Time)" />
                                    <asp:TextBox ID="txtCheckOut" runat="server" CssClass="form-control" Width="35%" placeholder="To (Check-out Time)" />
                                </div>
                                
                                <div class="form-group">
                                    <label>Grace Time (Minutes): </label>
                                    <asp:TextBox ID="txtLateGraceMinutes" runat="server" CssClass="form-control" placeholder="Maximum Late Time in Minutes" />
                                </div>
                                
                                <div class="form-group">
                                    <label>Fooding Allowance : </label>
                                    <asp:TextBox ID="txtFooding" runat="server" CssClass="form-control" placeholder="Daily Attendance Tiffin Amount in Taka" />
                                </div>
                                
                                <div class="form-group">
                                    <label>Attendance Bonus : </label>
                                    <asp:TextBox ID="txtAttnBonus" runat="server" CssClass="form-control" placeholder="Full Attendance Bonus in Taka" />
                                </div>

                                    <div class="form-group">
                                        <label>Other Settings: </label>
                                        <asp:CheckBox ID="chkShift" runat="server" Text="Shifting Duty"  Width="100%"  />
                                        <asp:CheckBox ID="chkOT" runat="server" Text="Overtime Allowed"  Width="100%" />
                                        <asp:CheckBox ID="ChkHouseRent" runat="server" Text="House Rent Allowed"  Width="100%"  />
                                        <asp:CheckBox ID="chkHoliday" runat="server" Text="Holiday Allowance"  Width="100%"  />
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
                                    DataSourceID="SqlDataSource1" Width="100%"  DataKeyNames="GroupSrNo"  OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
                                    <Columns>                
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GradeID" SortExpression="GradeID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("GroupSrNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:BoundField DataField="GroupName" HeaderText="Costing Head" SortExpression="GroupName" />
                                        <asp:BoundField DataField="MinDutyHour" HeaderText="Min.Duty Hour" SortExpression="Description" />
                                        <asp:BoundField DataField="GeneralDutyHour" HeaderText="G.Duty Hour" SortExpression="ProjectID"  />
                                        <asp:BoundField DataField="CheckinTime" HeaderText="Check-in Time" DataFormatString="{0:hh:mm tt}" SortExpression="Description" />
                                        <asp:BoundField DataField="CheckoutTime" HeaderText="Check-out Time" DataFormatString="{0:hh:mm tt}" SortExpression="ProjectID"  />
                                        <asp:BoundField DataField="GraceMinutes" HeaderText="Grace Minutes" SortExpression="Description" />
                                        <asp:BoundField DataField="Fooding" HeaderText="Fooding Allowance" SortExpression="ProjectID"  />
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Select to Edit" />
                                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />

                                                    <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                    </asp:ConfirmButtonExtender>
                                                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                        PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                    <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                        <b style="color: red">Item will be deleted permanently!</b><br /><br />
                                                        Are you sure to delete the item from list?
                                                            <br /> <br /> <br/> <br/>
                                                        <div style="text-align: right;">
                                                            <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                            <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                        </div>
                                                    </asp:Panel>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT GroupSrNo, GroupName, MinDutyHour, GeneralDutyHour, CheckinTime, CheckoutTime, GraceMinutes, Fooding FROM [EmpTypes] ORDER BY [GroupName]"
                                    DeleteCommand="Delete FROM [PrdnPowerPressDetails]  where PrdnID='0'  "></asp:SqlDataSource>

                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>


