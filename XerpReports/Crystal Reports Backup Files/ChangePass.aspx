<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="ChangePass.aspx.cs" Inherits="Oxford.app.ChangePass" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        label {
            display: block;
        }
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bodycontent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>


    <div class="grid_4">
        <div class="widget_wrap">
            <div class="widget_top">
                <span class="h_icon list_image"></span>
                <h6>Change Password</h6>
            </div>
            <div class="widget_content">
                <div class="form_container left_label">


                    <asp:ChangePassword ID="ChangePassword1" runat="server" CancelDestinationPageUrl="Default.aspx" ContinueDestinationPageUrl="Default.aspx" Width="100%">
                        <ChangePasswordTemplate>
                            <ul class="col-md-4">

                                <li>
                                    <div class="form_grid_12">
                                        <label class="field_title">
                                            <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Current Password:</asp:Label></label>
                                        <div class="form_input">

                                            <asp:TextBox ID="CurrentPassword" runat="server" class="tip_top" title="Your Current password" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server"
                                                ControlToValidate="CurrentPassword" ErrorMessage="Password is required."
                                                ToolTip="Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </li>

                                <li>
                                    <div class="form_grid_12">
                                        <label class="field_title">
                                            <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">New Password:</asp:Label></label>
                                        <div class="form_input">

                                            <asp:TextBox ID="NewPassword" runat="server" class="tip_top" title="Set New Password" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server"
                                                ControlToValidate="NewPassword" ErrorMessage="New Password is required."
                                                ToolTip="New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </li>

                                <li>
                                    <div class="form_grid_12">
                                        <label class="field_title">
                                            <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label></label>
                                        <div class="form_input">

                                            <asp:TextBox ID="ConfirmNewPassword" runat="server" class="tip_top" title="Confirm New password" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server"
                                                ControlToValidate="ConfirmNewPassword"
                                                ErrorMessage="Confirm New Password is required."
                                                ToolTip="Confirm New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form_grid_12">
                                        <label class="field_title"></label>
                                        <div class="form_input">
                                            <asp:CompareValidator ID="NewPasswordCompare" runat="server"
                                                ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                                Display="Dynamic"
                                                ErrorMessage="The Confirm New Password must match the New Password entry."
                                                ValidationGroup="ChangePassword1"></asp:CompareValidator>

                                            <span align="center" style="color: Red;">
                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                            </span>
                                        </div>
                                    </div>
                                </li>

                                <li>
                                    <div class="form_grid_12">
                                        <div class="form_input">
                                            <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False"
                                                Visible="false" CommandName="Cancel" Text="Cancel" />

                                            <asp:Button ID="ChangePasswordPushButton" runat="server"
                                                CommandName="ChangePassword" Text="Change Password"
                                                ValidationGroup="ChangePassword1" />
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </ChangePasswordTemplate>
                    </asp:ChangePassword>

                </div>
            </div>
        </div>
    </div>

</asp:Content>

