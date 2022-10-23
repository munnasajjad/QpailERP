<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Cash-Statement.aspx.cs" Inherits="AdminCentral_Cash_Statement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        <%--table tr td 
        {
            min-height:20px;
            font-size:13px;
            cel
            }
    --%>
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="grid_12 full_block">
				<div class="widget_wrap">
					<div class="widget_top">
						<span class="h_icon list_image"></span>
						<h6>Cash Flow Statement</h6>
					</div>
					<div class="widget_content">
						<h3>Cash Statement</h3>
						
						
						<asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
						
                        <asp:GridView ID="GridView1" runat="server"  class="display data_tbl"  Width="100%">
                        </asp:GridView>

					</div>
			</div>

    </div>
</asp:Content>

