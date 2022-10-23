<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="js-error.aspx.cs" Inherits="js_error" %>
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
<div id="error_404">
	<div class="error_container">
		<div class="error_info">
			<div class="error_code">
				501
			</div>
			<div class="error_meta">
				<span>Oops!</span>Error...
			</div>
			<span class="clear"></span>
		</div>
		<div class="error_content">
			<div class="error_message">
				<span>We are sorry</span> Your Session has been expired.
			</div>
			<div class="home_link">
				<a href="Default.aspx" title="GO TO HOME">Home</a>
			</div>
			<span class="clear"></span>
		</div>
		<div class="error_instruction">
			<div class="instruction_list">
				<h3>Lost? We suggest...</h3>
				<ol>
				    <li>Your Session might be expired.<a href="Login.aspx">Login Again.</a></li>
					<li>Checking the web address for typos.</li>
					<li>The page requested could not be found.</li>
					<li>You are using backdated web-browser. <a href="Latest.aspx">Get latest Web Browsers.</a></li>
					
				</ol>
			</div>
			<div class="mark_icon">
				?
			</div>
			<span class="clear"></span>
		</div>
		
	</div>
</div>
</asp:Content>