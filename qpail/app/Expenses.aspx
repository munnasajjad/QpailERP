<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Expenses.aspx.cs" Inherits="app_Expenses" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">
 <%--Employee Setup--%>
<fieldset>
    <legend>Daily Expenses Entry </legend>
    
    &nbsp;<table class="table1">
    <tr>
            <td class="label">
                <asp:Label ID="lblDate" runat="server" Text="Date of Expense:"></asp:Label>
            </td>
            <td class="textbox">
                <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
                    Enabled="True" TargetControlID="txtDate"  Format="dd/MM/yyyy">
                </asp:CalendarExtender>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr><%--Department ID--%>
            <td class="label">
                <asp:Label ID="lblDid" runat="server" Text="Expense Head: "></asp:Label>
            </td>
            <td class="textbox">
                <asp:DropDownList ID="ddHeadName" runat="server" DataSourceID="SqlDataSource1" 
                    DataTextField="ExpHeadName" DataValueField="ExpHeadName"  >
                </asp:DropDownList>
                </td>
            <td>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" 
                    SelectCommand="SELECT [ExpHeadName] FROM [ExpenseHeads]">
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr><%--Department Name--%>
            <td class="label">
                <asp:Label ID="lblDeptName" runat="server" Text="Description of Expense: "></asp:Label>
            </td>
            <td class="textbox">
                <asp:TextBox ID="txtDescription" runat="server" Width="256px"></asp:TextBox>
                </td>
            <td>
                &nbsp;</td>
        </tr>

        <tr><%--Department Name--%>
            <td class="label">
                <asp:Label ID="Label1" runat="server" Text="Exp. Amount: "></asp:Label>
            </td>
            <td class="textbox">
                <asp:TextBox ID="txtAmount" runat="server"  onKeyUp="document.getElementById('btnSave').disabled = false" ></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtAmount">
                </asp:FilteredTextBoxExtender>
                </td>
            <td>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
        </tr>
        <tr><%--Department Name--%>
            <td class="label">
                &nbsp;</td>
            <td class="textbox">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="104px" 
                    onclick="btnSave_Click"  />
                </td>
            <td>
                &nbsp;</td>
        </tr>
        

        <tr>
            <td class="label">
                &nbsp;</td>
            <td class="textbox">
                <asp:Label ID="lblMsg" CssClass="msg" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        
        <tr>
            <td class="label">
                &nbsp;</td>
            <td class="textbox">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        </table>
</fieldset> 



<fieldset>
<legend>Todays Expenses</legend>
    <asp:GridView ID="GridView1" runat="server" Width="100%" AllowSorting="True" 
        AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" 
        BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" 
        GridLines="Vertical" DataSourceID="SqlDataSource3">
        <Columns>
            <asp:BoundField DataField="ExpenseID" HeaderText="ExpenseID" 
                SortExpression="ExpenseID" />
            <asp:BoundField DataField="ExpHeadName" HeaderText="ExpHeadName" 
                SortExpression="ExpHeadName" />
            <asp:BoundField DataField="Description" HeaderText="Description" 
                SortExpression="Description" />
            <asp:BoundField DataField="Amount" HeaderText="Amount" 
                SortExpression="Amount" />
        </Columns>
        <FooterStyle BackColor="#CCCCCC" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="#CCCCCC" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
        ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
        
        
        
        SelectCommand="SELECT ExpenseID, ExpHeadName, Description, Amount FROM Expenses WHERE (ExpDate = @expdate)">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtDate" Name="expdate" 
                PropertyName="Text" Type="DateTime" />
        </SelectParameters>
    </asp:SqlDataSource>

</fieldset>






</asp:Content>

