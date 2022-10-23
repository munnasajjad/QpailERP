<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Oxford.Login" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8">
    <title>Pathshala: School Management System</title>  
    <meta name="description" content=""> 
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1"> 
    <link rel="stylesheet" href="css/app.v1.css"> 
    <link rel="stylesheet" href="css/font.css" cache="false"> 
    <!--[if lt IE 9]> <script src="js/ie/respond.min.js" cache="false"></script> 
        <script src="js/ie/html5.js" cache="false"></script> <script src="js/ie/fix.js" cache="false"></script> 
    <![endif]-->

    <script type="text/javascript">
        function change_case() {
            this.value = this.value.toLowerCase().trim();
        }

        $('.Login .Textbox').focus(function () {
            $(this).attr('class', 'Hover');
        });

        $('.Login .Textbox').blur(function () {
            $(this).attr('class', 'Textbox');
        });

        $(document).ready(function () {
            $(":text.Textbox")(function () {
                $(this).addClass('focus');
            }).blur(function () {
                $(this).removeClass('focus');
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <section id="content" class="m-t-lg wrapper-md animated fadeInUp">
            <a class="nav-brand" href="Login.aspx">Digital Pathshala</a>
            <span style="text-align:center;display: block;">School Management System</span>
            <div class="row m-n">
                <div class="col-md-4 col-md-offset-4 m-t-lg">
                    <section class="panel">
                        <div class="panel-body">
                        <asp:Panel DefaultButton="Login1$LoginButton" runat="server" ID="pnlLogin">

                            <asp:Login ID="Login1" runat="server" TitleText="Login to your Account"
                                OnLoggedIn="Login1_LoggedIn" OnLoginError="Login1_LoginError" Width="100%"
                                FailureText="Invalid Username or Password" OnLoggingIn="Login1_LoggingIn">
                                <LayoutTemplate>

                                    <header class="panel-heading text-center">Sign in </header>
                        
                            <div class="form-group">
                                <label class="control-label">User ID</label>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                <asp:TextBox ID="UserName" runat="server" CssClass="form-control"></asp:TextBox>                                
                            </div>
                            <div class="form-group">
                                <label class="control-label">Password</label>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>                                
                            </div>
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." /></label>                                
                            </div>
                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" 
                                Text="Log In" ValidationGroup="Login1" CssClass="btn btn-info" />
                            <div class="line line-dashed"></div>
                             <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>

                                    
                                </LayoutTemplate>
                            </asp:Login>

                        </asp:Panel>
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                           </div>  
                    </section>
                </div>
            </div>
        </section>
        <!-- footer -->
        <footer id="footer">
            <div class="text-center padder clearfix">
                <p><small>&copy; 2015 <a href="//xtremebd.com">Extreme Solutions</a> </small> </p>
            </div>
        </footer>
        <!-- / footer -->
        <script src="css/app.v1.js"></script>
        <!-- Bootstrap -->
        <!-- app -->
    </form>
</body>
</html>
