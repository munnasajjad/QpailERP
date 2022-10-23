<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ledger.aspx.cs" Inherits="Application_Reports_Ledger" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Party Ledger</title>
    <script src="../../js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../../js/jquery-ui-1.10.0.custom.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="content">
        <h2>
            Check Party Ledger
        </h2>
        <table width="100%">
            <tr>
                <td>
                    Party Name
                </td>
                <td>
                    <asp:DropDownList ID="ddParties" runat="server" DataSourceID="SqlDataSource2" DataTextField="Name"
                        DataValueField="Name" Width="220px" AppendDataBoundItems="True"
                        OnSelectedIndexChanged="ddParties_SelectedIndexChanged">
                        <asp:ListItem>---Select---</asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                        SelectCommand="SELECT [Name] FROM [Party] ORDER BY [Name]"></asp:SqlDataSource>
                </td>
                <td>
                    Date From:
                    <asp:TextBox ID="txtDateFrom" runat="server" Width="80px"></asp:TextBox>
                </td>
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtDateFrom"
                    Format="dd/MM/yyyy">
                </asp:CalendarExtender>
                <td>
                    Date To:
                    <asp:TextBox ID="txtDateTo" runat="server" Width="80px"></asp:TextBox>
                </td>
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtDateTo"
                    Format="dd/MM/yyyy">
                </asp:CalendarExtender>
                <td>
                    <asp:Button ID="btnShow" runat="server" Text="Show Ledger" OnClick="btnShow_Click" />
                </td>
                <td>
                    <asp:Button ID="btnPrint" runat="server" Text="Print Ledger" OnClick="btnPrint_Click" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CCCCCC"
            BorderStyle="None" BorderWidth="1px" Width="100%" CellPadding="3">
            <RowStyle ForeColor="#000066" />
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <EmptyDataTemplate>
                No Records Found For the Party!
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
            SelectCommand="SELECT [TrDate], [Description], [Dr], [Cr], [Balance] FROM [Transactions] WHERE ([HeadName] = @HeadName) ORDER BY [TrID] DESC">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddParties" Name="HeadName" PropertyName="SelectedValue"
                    Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <fieldset>
            <legend>Cheque in Hand</legend>
            <asp:GridView ID="GridView2" Width="100%" runat="server">
            </asp:GridView>
        </fieldset>
    </div>
    </form>
    <script src="../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">        $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); </script>
</body>
</html>
