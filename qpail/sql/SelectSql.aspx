<%@ Page Language="C#" Theme="Blue" AutoEventWireup="true" CodeFile="SelectSql.aspx.cs" Inherits="sql_SelectSql" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Generate C# CRUD</title>

    <script src="clipboard.js"></script>
    <script>
        function copyTxt() {
            clipboard.copy(document.getElementById("lblResult").innerText);
        }
    </script>

    <style>
        textarea#txtSqlText, .result {
            font-family: courier new, courier;
            padding: 14px;
            font-size: 16px;
            line-height: 18px;
            text-align: center;
			border-radius: 5px;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <table style="margin: 0 auto; text-align: center;">
            <tbody>
                <tr>
                    <td>
                        <asp:RadioButton ID="RadioButton1" runat="server" Text="Select" GroupName="g" Checked="True" /></td>
                    <td>
                        <asp:RadioButton ID="RadioButton2" runat="server" Text="Insert" GroupName="g" /></td>
                    <td>
                        <asp:RadioButton ID="RadioButton3" runat="server" Text="Update" GroupName="g" /></td>
                    <td>
                        <asp:RadioButton ID="RadioButton4" runat="server" Text="Delete" GroupName="g" /></td>
                </tr>
                <tr><td colspan="4"> &nbsp; </td></tr>
                <tr>
                    <td>Table Name:</td> <td>
                        <asp:TextBox ID="txtTable" runat="server"></asp:TextBox></td>
                    <td>Data key:</td> <td>
                        <asp:TextBox ID="txtKey" runat="server"></asp:TextBox></td>
                </tr>
            </tbody>
        </table>

        <div class="result">

            <b>
                <asp:Label ID="Label1" runat="server" Text="Values to loop (only column names)"></asp:Label></b>
            <asp:TextBox ID="txtSqlText" runat="server" TextMode="MultiLine" Width="100%" Height="150px"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Generate Values" OnClick="Button1_OnClick" />
            <asp:Button ID="Button2" runat="server" Text="Copy" OnClientClick="copyTxt()" />
            <br />
            <br />
            <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
        </div>
    </form>

</body>
</html>
