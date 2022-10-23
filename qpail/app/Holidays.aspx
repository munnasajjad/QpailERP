<%@ Page Title="Holidays" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Holidays.aspx.cs" Inherits="app_Holidays" %>

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
                    <h3 class="page-title">Setup Holidays
                    </h3>
                </div>
            </div>
            <div class="row">
                
                <div class="col-md-6 ">
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Setup a New Holiday
                            </div>
                            
                        </div>
                        <div class="portlet-body form">
                            
                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass=""></asp:Label>
                                <asp:Label ID="lblId" runat="server" Visible="False"></asp:Label>
                                
                                <div class="form-group">
                                    <label>Date of Holiday: </label>
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="Date of Holiday" />
                                    <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy" Enabled="True" TargetControlID="txtDate"></asp:CalendarExtender>
                                </div>
                                
                                        <div class="control-group">
                                            <asp:Label ID="Label5" runat="server" Text="Holiday Type: "></asp:Label>
                                            <asp:DropDownList ID="ddType" runat="server">
                                                <asp:ListItem>Public Holiday</asp:ListItem>
                                                <asp:ListItem>Official Holiday</asp:ListItem>
                                                <asp:ListItem>Religious Festival (For Bonus)</asp:ListItem>
                                                <asp:ListItem>Optional Holiday</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                
                                <div class="form-group">
                                    <label>Holiday Name: </label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" EnableViewState="true" placeholder="Holiday Name" />
                                </div>

                                <div class="form-actions">
                                    <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click1" />
                                    <asp:Button ID="btnClear" CssClass="btn default" runat="server" Text="Cancel" OnClick="btnClear_Click1" />
                                </div>

                                
                                    
                                <legend>Weekly Holiday</legend>
                                <div class="form-group center">
                                        <asp:CheckBox ID="chkSaturday" runat="server" Text="Saturday"  Width="100%" AutoPostBack="True" OnCheckedChanged="chkSaturday_OnCheckedChanged" />
                                        <asp:CheckBox ID="chkSunday" runat="server" Text="Sunday"  Width="100%"  AutoPostBack="True" OnCheckedChanged="chkSaturday_OnCheckedChanged" />
                                        <asp:CheckBox ID="ChkMonday" runat="server" Text="Monday"  Width="100%"  AutoPostBack="True" OnCheckedChanged="chkSaturday_OnCheckedChanged" />
                                        <asp:CheckBox ID="chkTuesday" runat="server" Text="Tuesday"  Width="100%"  AutoPostBack="True" OnCheckedChanged="chkSaturday_OnCheckedChanged" />
                                        <asp:CheckBox ID="chkWednesday" runat="server" Text="Wednesday"  Width="100%"  AutoPostBack="True" OnCheckedChanged="chkSaturday_OnCheckedChanged" />
                                        <asp:CheckBox ID="chkThursday" runat="server" Text="Thursday"  Width="100%"  AutoPostBack="True" OnCheckedChanged="chkSaturday_OnCheckedChanged" />
                                        <asp:CheckBox ID="chkFriday" runat="server" Text="Friday"  Width="100%"  AutoPostBack="True" OnCheckedChanged="chkSaturday_OnCheckedChanged" />
                                    </div>
                                
                            </div>
                        </div>
                    </div>
                </div>




                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Saved Holidays
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


                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="25"
                                    DataSourceID="SqlDataSource1" Width="100%"  DataKeyNames="sl"  OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
                                    <Columns>                
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GradeID" SortExpression="GradeID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("sl") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="GroupName" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="Weekday" HeaderText="Weekday" SortExpression="Description" />
                                        <asp:BoundField DataField="DayName" HeaderText="Holiday Name" SortExpression="ProjectID"  />
                                        <asp:BoundField DataField="HolidayType" HeaderText="Holiday Type"  SortExpression="Description" />
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
                                    SelectCommand="SELECT sl, HolidayType, DayName, Date, Weekday FROM [Holidays] ORDER BY [Date] DESC"
                                    DeleteCommand="Delete FROM [PrdnPowerPressDetails]  where PrdnID='0'  "></asp:SqlDataSource>

                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>


