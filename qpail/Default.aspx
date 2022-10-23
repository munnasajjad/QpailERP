<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master"  Theme="Blue"  AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript">
    
        $('.Login .Textbox').focus(function() {
            $(this).attr('class', 'Hover');
        });

        $('.Login .Textbox').blur(function() {
            $(this).attr('class', 'Textbox');
        });

        $(document).ready(function() {
            $(":text.Textbox")(function() {
                $(this).addClass('focus');
            }).blur(function() {
                $(this).removeClass('focus');
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:panel defaultbutton="Login1$LoginButton" runat="server" ID="pnlLogin">

<div id="login_page">
	<div class="login_container">
		<div class="login_header blue_lgel">
			<ul class="login_branding">
				<li>
				<div class="logo_small">
					<img src="images/logo-bingo.png" width="99" height="35" alt="bingo">
				</div>
				<span style="padding:3px;">Developed By <a href="//xtremebd.com">Extreme Solutions</a></span>
				</li>
				<li class="right go_to"><a href="Default.aspx" title="Go to Main Site" class="home">Go To Main Site</a></li>
			</ul>
		</div>
		
		
		
		
<asp:Login ID="Login1" runat="server" TitleText="Login to your Account" 
        onloggedin="Login1_LoggedIn" PasswordRecoveryText="Reset My Password" 
        PasswordRecoveryUrl="ResetPassword.aspx" onloginerror="Login1_LoginError">
    <LayoutTemplate >
    
		
			<div class="login_form">
				<h3 class="blue_d">User Login</h3>
				<ul>
					<li class="login_user">
					 <asp:TextBox ID="UserName" runat="server"  CssClass="text_field" Width="250px"   placeholder="User ID"></asp:TextBox>
					 <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                    ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                    ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
					</li>
					<li class="login_pass">
					<asp:TextBox ID="Password" runat="server"  CssClass="text_field" Width="250px" TextMode="Password"   placeholder="Password"></asp:TextBox>
					<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                    ControlToValidate="Password" ErrorMessage="Password is required." 
                                    ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
					</li>
				</ul>
			</div>
			
			<asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" CssClass="login_btn blue_lgel"
                                    ValidationGroup="Login1" />
			<ul class="login_opt_link">
				<li><a href="forgot-pass.html">Forgot Password?</a></li>
				<li class="remember_me right">
				<asp:CheckBox ID="RememberMe" runat="server" Text="Remember Password." />
				
			</ul>
		
		
       <asp:AnimationExtender ID="animLogin" runat="server" TargetControlID="LoginButton">
            <Animations>
                <OnClick>
                    <Sequence>
                        <FadeOut Duration=".5" Fps="20" AnimationTarget="pnlLogin" />
                    </Sequence>
                </OnClick>
            </Animations>
            </asp:AnimationExtender>
                </LayoutTemplate>
            </asp:Login>
                
                

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
        
		
		
	</div>
</div>		
		</asp:panel>
			<%--Login Status--%>
        <asp:Panel ID="PanelError" runat="server" Visible="false">
        
		<%--<div class="login_success">
			<span class="icon"></span>Login Successfully
		</div>--%>
		<div class="login_invalid">
			<span class="icon"></span>
			
			<asp:Label ID="Label1" runat="server" Text=""></asp:Label>    
			<asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
			To Try Again <a href="Login.aspx">Click Here</a>
		</div>
        <%--Login Status--%></asp:Panel>
     

    
</asp:Content>