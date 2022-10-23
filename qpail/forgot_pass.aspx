<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="forgot_pass.aspx.cs" Inherits="forgot_pass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $(window).resize(function () {
                $('.login_container').css({
                    position: 'absolute',
                    left: ($(window).width() - $('.login_container').outerWidth()) / 2,
                    top: ($(window).height() - $('.login_container').outerHeight()) / 2
                });
            });
            // To initially run the function:
            $(window).resize();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div id="login_page">
        <div class="login_container">
            <div class="login_header blue_lgel">
                 <ul class="login_branding">
                    <li>
                        <div class="logo_small">
                            <asp:Image ID="imgLogo" runat="server" Width="99px" Height="35px" />
                        </div>
                        <span style="padding: 3px;"><asp:Literal ID="ltrDeveloper" runat="server"></asp:Literal></span>
                    </li>
                    <li class="right go_to"><a href="Login.aspx" title="Go to Login Page" class="home">Go To Main Site</a></li>
                </ul>
            </div>

            <div class="forgot_pass">
                <h3 class="blue_d">Forgot Password</h3>

                <ul>
                    <asp:Panel ID="Panel1" runat="server">

                        <li class="login_user">
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="textbox" PlaceHolder="Login ID"></asp:TextBox>
                        </li>

                    </asp:Panel>


                    <asp:Panel ID="Panel2" runat="server" Visible="false">

                        <li>A password reset link has been sent to
                                <asp:Literal ID="Literal2" runat="server" /><br>
                            <br>
                            Note: For security reasons, your email address is encrypted and not shown above. The password recovery mail has been sent to the registered email address on records with us.
                        </li>

                    </asp:Panel>
                </ul>
            </div>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="forgot_btn blue_lgel"  />

            
                        <div class='error'>
                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        </div>
        </div>
    </div>


</asp:Content>

