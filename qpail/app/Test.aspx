<%@ Page Title="Test" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="app_Test" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--<script type="text/javascript" src="https://code.jquery.com/jquery-1.11.3.min.js"></script>--%>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.10/js/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.10/css/jquery.dataTables.min.css">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".tbl_default").prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "lengthMenu": [[3, 5, 10, 25, -1], [3, 5, 10, 25, "All"]] //value:item pair
            });
        });
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">


<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
<ProgressTemplate>
<div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
    <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
</div>
</ProgressTemplate>
</asp:UpdateProgress>

<asp:UpdatePanel ID="pnl" runat="server">
<ContentTemplate>

<script type="text/javascript" language="javascript">
    Sys.Application.add_load(callJquery);
</script>

    <asp:TextBox ID="txtReceiver" runat="server"></asp:TextBox><br />
    <asp:TextBox ID="txtMessage" runat="server"></asp:TextBox><br />
    <asp:Button ID="btnSend" runat="server" Text="Button" OnClick="btnSend_OnClick" />
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>

</ContentTemplate>
</asp:UpdatePanel>


</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

