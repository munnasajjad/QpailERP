<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="app_Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2>Profile</h2>
    <p>Feature will be available soon...</p>




    <asp:ChangePassword ID="ChangePassword1" runat="server" CancelDestinationPageUrl="Default.aspx" ContinueDestinationPageUrl="Default.aspx" Width="100%">
        <ChangePasswordTemplate>
            <ul>

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



</asp:Content>

