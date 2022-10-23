<%@ Page Title="Report Settings" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Acc-Rpt-Settings.aspx.cs" Inherits="app_Acc_Rpt_Settings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">


    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
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
            </script>--%>


    <h3 class="page-title">Report Settings</h3>

    <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>


    <div class="row">
        <div class="col-lg-12">
            
            <legend>Payment-Receipt Report Settings </legend>

            <asp:GridView ID="GridView1" DataSourceID="SqlDataSource1" runat="server" AutoGenerateColumns="False" CssClass="table-bordered table"
                         DataKeyNames="sl" >
                <Columns>
                    
                    
                    <asp:TemplateField ItemStyle-Width="40px">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="sl" InsertVisible="False" SortExpression="sl" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("sl") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="AccountsName" HeaderText="Subsidiary Accounts" SortExpression="ControlAccountsID" />
                    <asp:BoundField DataField="ControlAccountsID" HeaderText="Control Accounts ID" SortExpression="ControlAccountsID" />
                    <asp:BoundField DataField="ControlAccountsName" HeaderText="Control Accounts Name" SortExpression="ControlAccountsName" />
                    <asp:TemplateField HeaderText="Show on Receipt Side" SortExpression="ReceiptShow">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("ReceiptShow") %>'  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Show on Payment Side" SortExpression="ReceiptShow">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("PaymentShow") %>'  />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                               ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                               SelectCommand="SELECT        ControlAccount.sl, ControlAccount.ControlAccountsID, ControlAccount.ControlAccountsName, ControlAccount.ReceiptShow, ControlAccount.PaymentShow, Accounts.AccountsName
FROM            ControlAccount INNER JOIN
                         Accounts ON ControlAccount.AccountsID = Accounts.AccountsID
ORDER BY Accounts.AccountsID, ControlAccount.ControlAccountsID"></asp:SqlDataSource>
            
            <table border="0"  style="width: 75%" class="table1">
                <tr>
                    <th><%--As on--%></th>
                    <th></th>
                </tr>
                <tr>
                    <td>
                        <%--<asp:TextBox ID="txtOpening" runat="server" CssClass="form-control" Width="100%" Enabled="True"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtOpening" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>--%>
                    </td>
                                            
                    <td style="text-align: center; vertical-align: middle;">
                        <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" Text="Save" OnClick="btnSave_OnClick" />
                                                
                    </td>
                </tr>
                                      
            </table>


        </div>
    </div>



</asp:Content>

