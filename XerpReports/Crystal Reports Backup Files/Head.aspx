<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="Head.aspx.cs" Inherits="Oxford.app.Head" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodycontent" runat="server">
    
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="upnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>


            <div class="grid_12 full_block">
                <div class="widget_wrap">
                    <div class="widget_top">
                        <span class="h_icon list_image"></span>
                        <h6>Accounts Head Setup</h6>
                    </div>
                    <div class="widget_content">



                        <span style="color: Maroon;">
                            <asp:Label ID="lblMsg" runat="server" EnableViewState="false" ></asp:Label></span>

                        <div class="form_container left_label field_set">
                            <fieldset>
                                <legend><b>»</b> Accounts Head </legend>


                                <table class="table1">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblEid" runat="server" Text="Group Name : "></asp:Label>
                                        </td>
                                        <td style="margin-left: 160px">
                                            <asp:DropDownList ID="ddGroup" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                            </asp:DropDownList>

                                            <asp:Label ID="lblGroup" runat="server" Visible="false"></asp:Label>

                                        </td>
                                        <td></td>
                                    </tr>


                                    <%--<asp:Panel ID="bothpanel" runat="server" Visible="false">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="Expense Type : "></asp:Label>
                                        </td>
                                        <td style="margin-left: 160px">
                                            <asp:RadioButton ID="RadioButton1" runat="server" Checked="true" Text="HO Only" GroupName="rd" />
                                            <asp:RadioButton ID="RadioButton2" runat="server" Checked="false" Text="HO+Counter (Both)" GroupName="rd" />
                                        </td>
                                        <td></td>
                                    </tr>

                                </asp:Panel>--%>


                                    <tr>
                                        <td>
                                            <asp:Label ID="lblEdu0" runat="server" Text="Subsidiary Accounts : "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddSub" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddSub_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblSub" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td><asp:Button ID="btnSubRefresh" CssClass="btn" runat="server" Text="R..." OnClick="btnSubRefresh_Click" Width="35px" /> </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Control Accounts"></asp:Label>
                                        </td>
                                        <td>

                                            <asp:DropDownList ID="ddControl" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddControl_SelectedIndexChanged">
                                            </asp:DropDownList>

                                            <asp:Label ID="lblControl" runat="server" Visible="false"></asp:Label>

                                        </td>
                                        <td><asp:Button ID="btnControlRefresh" runat="server" Text="R..." OnClick="btnControlRefresh_Click" Width="35px" /> </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="A/C Head ID : "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHeadID" runat="server" Enabled="False"></asp:TextBox>
                                            <asp:Label ID="lblHeadID" runat="server" Visible="false"></asp:Label>
                                            
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkDisable" CssClass="radiobtn" runat="server" Text="Disable" Checked="true" Visible="false" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="A/C Head Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHeadName" runat="server"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>


                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Openning Balance : "></asp:Label>
                                        </td>
                                        <td>
                                            <table>
                                                        <tr>
                                                            <td>
                                            <asp:TextBox ID="txtOpBalance" runat="server" Width="100px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtOpBalance_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtOpBalance">
                                            </asp:FilteredTextBoxExtender>
                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="RadioButton1" CssClass="radiobtn" runat="server" Text="Dr." Checked="true" GroupName="g" /></td>
                                                            <td>
                                                                <asp:RadioButton ID="RadioButton2" CssClass="radiobtn" runat="server" Text="Cr." GroupName="g" /></td>
                                                        </tr>
                                                    </table>

                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="Date of Initialization : "></asp:Label>

                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtDate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>

                                </table>

                                <ul class="btn_area">
                                    <li>
                                        <div class="form_grid_12">
                                            <div class="form_input">

                                                <asp:Button ID="btnSave" CssClass="btn" runat="server" Text="Save New Head" OnClick="btnSave_Click" />
                                                <%--<asp:Button ID="btnDelete" runat="server" Text="Delete This Schedule"  CssClass="btn red" 
                                onclick="btnDelete_Click" />
                                <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnDelete"  ConfirmText="confirm to delete this schedule ??"> 
                                 </asp:ConfirmButtonExtender> --%>
                                                <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" OnClick="btnClear_Click" />
                                            </div>

                                        </div>

                                    </li>
                                </ul>
                                <%--</div>

                </div>

            </div>--%>
                            </fieldset>


                            <fieldset>
                                <legend>List of Accounts</legend>
                                <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>
                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" Width="100%"
                                    CssClass="table-responsive"
                                    DataKeyNames="AccountsHeadID" DataSourceID="SqlDataSource1"
                                    OnRowDataBound="GridView2_RowDataBound" OnSelectedIndexChanged="GridView2_SelectedIndexChanged"  OnRowDeleting="GridView2_RowDeleting">
                                    <Columns>

                                        <asp:TemplateField ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="40px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Head ID" SortExpression="AccountsHeadID">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("AccountsHeadID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="GroupName" HeaderText="Group Name" ReadOnly="true"
                                            SortExpression="GroupName" />
                                        <asp:BoundField DataField="AccountsName" HeaderText="Accounts Name" ReadOnly="true"
                                            SortExpression="AccountsName" />
                                        <asp:BoundField DataField="ControlAccountsName" ReadOnly="true"
                                            HeaderText="Control Account" SortExpression="Control Accounts" />
                                        <asp:BoundField DataField="AccountsHeadName" HeaderText="Accounts Head Name" ReadOnly="true"
                                            SortExpression="AccountsHeadName" />

                                        <asp:TemplateField HeaderText="Op.Balance (Dr.)" SortExpression="">                                            
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("OpBalDr") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Op.Balance (Cr.)" SortExpression="Op.Balance">                                            
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("OpBalCr") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/AdminCentral/images/edit.png" Text="Select" ToolTip="Edit" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/AdminCentral/images/delete.png" Text="Delete" ToolTip="Delete" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">A/C Head will be deleted permanently!</b><br />
                                                    Are you sure you want to delete the item from order list?
                                                            <br />
                                                    <br />
                                                    <div style="text-align: right;">
                                                        <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                        <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                    </div>
                                                </asp:Panel>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                                <br />

                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT HeadSetup.AccountsHeadID, AccountGroup.GroupName, Accounts.AccountsName, ControlAccount.ControlAccountsName, HeadSetup.AccountsHeadName, HeadSetup.AccountsOpeningBalance
                                    , HeadSetup.OpBalDr, HeadSetup.OpBalCr FROM AccountGroup INNER JOIN HeadSetup INNER JOIN ControlAccount ON HeadSetup.ControlAccountsID = ControlAccount.ControlAccountsID INNER JOIN Accounts ON HeadSetup.AccountsID = Accounts.AccountsID ON AccountGroup.GroupID = HeadSetup.GroupID 
                                    where HeadSetup.ControlAccountsID=@ControlAccountsID order by AccountGroup.GroupID, HeadSetup.AccountsHeadID"
                                    
                                    UpdateCommand="Update HeadSetup set AccountsOpeningBalance =@AccountsOpeningBalance where  AccountsHeadID=@AccountsHeadID">
                                    
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddControl" Name="ControlAccountsID" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                    
                                    <UpdateParameters>
                                        <asp:Parameter Name="AccountsOpeningBalance" />
                                        <asp:Parameter Name="AccountsHeadID" />
                                        
                                    </UpdateParameters>
                                </asp:SqlDataSource>

                            </fieldset>


                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
