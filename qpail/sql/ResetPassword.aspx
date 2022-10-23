<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="Super_Admin_ResetPassword" %>

<html>
<head>
    <style type="text/css">
        .bigRed {
            color: red;
            font-weight: 900;
            margin-left: 10px;
        }

        .green1 {
            color: forestgreen;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <h2>Reset User Password</h2>
        <fieldset>
            <legend>Change Email ID of an Account</legend>

            &nbsp;<table class="table1">
                <tr>
                    <td>
                        <asp:Label ID="lblDid" runat="server" Text="User ID: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUid" runat="server"></asp:TextBox>
                        <asp:Button ID="btnSave0" runat="server" Text="Load" OnClick="btnSave0_Click" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Name: "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblName" CssClass="bigRed green1" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDid0" runat="server" Text="Current Email Address: "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCurrEmail" CssClass="bigRed green1" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Transaction Password: "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTrpass" CssClass="bigRed" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="lblDeptName" runat="server" Text="Reset Login Password: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Update Password"
                            OnClick="btnSave_Click" />
                        <asp:Label ID="lblMsg" CssClass="msg" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </fieldset>
    </form>
</body>
</html>
