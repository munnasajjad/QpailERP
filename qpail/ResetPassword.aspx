<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" Theme="Blue" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="ResetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContect" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">
 <form id="form1" runat="server">
    <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" BackColor="#F7F7DE" 
        BorderColor="#CCCC99" BorderStyle="Solid" BorderWidth="1px" 
        Font-Names="Verdana" Font-Size="10pt" SuccessPageUrl="~/Login.aspx" 
        SuccessText="Your password has been sent to youe Email." 
        UserNameTitleText="Forgot Your SDL Password?" 
        onsendingmail="PasswordRecovery1_SendingMail">
        <MailDefinition From="spider.sdl@gmail.com" Subject="Password Reset Successful">
        </MailDefinition>
        <UserNameTemplate>
            <table border="0" cellpadding="1" cellspacing="0" 
                style="border-collapse:collapse;">
                <tr>
                    <td>
                        <table border="0" cellpadding="0">
                            <tr>
                                <td align="center" colspan="2" 
                                    style="color:White;background-color:#6B696B;font-weight:bold;">
                                    Forgot Your SDL Password?</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    User Name to receive password.</td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                        ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                        ToolTip="User Name is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color:Red;">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="Submit" 
                                        ValidationGroup="PasswordRecovery1" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </UserNameTemplate>
        <TitleTextStyle BackColor="#6B696B" Font-Bold="True" ForeColor="#FFFFFF" />
    
        <MailDefinition 
         From="sdl.spider@gmail.com" 
         IsBodyHtml="true" 
         Priority="Low" 
         BodyFileName="welcomeMPO.htm"
         Subject="New Password Request">
         <EmbeddedObjects>
            <asp:EmbeddedMailObject Name="LogoImage" Path="images/logo.png" />
         </EmbeddedObjects>
        </MailDefinition>
    
    </asp:PasswordRecovery>
</form>
</asp:Content>

