
<%@ Page Title="Collection History" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="Coll-History.aspx.cs" Inherits="Oxford.app.Coll_History" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodycontent" runat="server">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            
    <section class="col-lg-12">
        <section class="panel">

            <h2>Collection History</h2>
            <table>
                <tr class="report">
                    
                    <td style="width: 20%;">Collection Group : 
                        <asp:DropDownList ID="ddGroup" runat="server" AutoPostBack="true" CssClass="form-control"
                                        DataSourceID="SqlDataSource7" DataTextField="name" DataValueField="sl" AppendDataBoundItems="True" 
                                        OnSelectedIndexChanged="ddGroup_OnSelectedIndexChanged">
                            <asp:ListItem>--- all ---</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [Name] FROM [CollectionTypes] order by sl"></asp:SqlDataSource>


                    </td>
                    <td style="width: 20%;">Collection Head : 
                         <asp:DropDownList ID="ddHead" runat="server" CssClass="form-control" AutoPostBack="true" AppendDataBoundItems="True"
                                        DataSourceID="SqlDataSource1" DataTextField="name" DataValueField="sl" OnSelectedIndexChanged="ddHead_OnSelectedIndexChanged">
                         <asp:ListItem>--- all ---</asp:ListItem>           
                         </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [Name] FROM [CollectionHeads] WHERE ([GroupID] = @GroupID) ORDER BY [Name]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddGroup" Name="GroupID" PropertyName="SelectedValue" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:Label ID="lblSl" runat="server" Text="" Visible="false"></asp:Label>
                        

                    </td>
               
                    <td style="width: 20%;">&nbsp; Date From: &nbsp;&nbsp;
        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control"></asp:TextBox></td>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    <td style="width: 20%;">&nbsp; Date To: 
        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control"></asp:TextBox></td>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtDateTo" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    <td style="width: 20%;">
                        <asp:Button ID="btnShow" runat="server" Text="Show Ledger" CssClass="btn btn-s-md btn-primary bottom"  OnClick="btnShow_Click" />
                        <%--<asp:Button ID="btnPrint" runat="server" Text="Print Ledger" 
            onclick="btnPrint_Click" />--%>
                    </td>
                </tr>
            </table>


            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

            <div class="table-responsive">

                <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                    BorderWidth="1px" Width="100%" CssClass="table grid" CellPadding="3" AutoGenerateColumns="False"
                    ShowFooter="true" AllowSorting="true" OnRowDataBound="GridView1_RowDataBound">
                    <RowStyle ForeColor="#000066" />
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="20px">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Date" SortExpression="Date">
                            <ItemTemplate>
                                <asp:Label ID="Label21" runat="server" Text='<%# Bind("EntryDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                     
                        <asp:TemplateField HeaderText="Col. Group" SortExpression="ExpGroup">
                            <ItemTemplate>
                                <asp:Label ID="Label3a" runat="server" Text='<%# Bind("ExpGroup") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Col. Head" SortExpression="ResellerName">
                            <ItemTemplate>
                                <asp:Label ID="ResellerName" runat="server" Text='<%# Bind("AccountsHeadID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" SortExpression="Server">
                            <ItemTemplate>
                                <asp:Label ID="Label3s" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount" SortExpression="Currency">
                            <ItemTemplate>
                                <asp:Label ID="Label3d" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                       
                    </Columns>
                    <FooterStyle BackColor="#AAAAAA" Font-Bold="True" ForeColor="Black" BorderStyle="Solid" />

                    <EmptyDataTemplate>
                        No Records Found!
                    </EmptyDataTemplate>
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5DCFF3" Font-Bold="True" ForeColor="White" />
                </asp:GridView>

                <%--<asp:SqlDataSource ID="SqlDataSource1" runat="server"
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                    SelectCommand="SELECT [TrDate], [Description], [Dr], [Cr], [Balance] FROM [Transactions] WHERE ([HeadName] = @HeadName) ORDER BY [TrID] DESC">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddParties" Name="PID"
                            PropertyName="SelectedValue" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>--%>

                <asp:Label ID="lblSummery" runat="server" Text="" CssClass="right" EnableViewState="false"></asp:Label>

            </div>

            <fieldset>
                <%--<legend>Cheque in Hand</legend>--%>

                <asp:GridView ID="GridView2" Width="100%" runat="server">
                </asp:GridView>
                <%--<input type="button" value="Print" onclick="PrintElem('.table-responsive')" />--%>

            </fieldset>
            
            </section>
            </div>
            

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

