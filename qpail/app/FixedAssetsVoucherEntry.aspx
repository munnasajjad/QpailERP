<%@ Page Title="Fixed Assets Entry" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="FixedAssetsVoucherEntry.aspx.cs" Inherits="app_FixedAssetsVoucher239" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>

            <script>
                function myFunction() {
                    var message, x;
                    message = document.getElementById("p01");
                    message.innerHTML = "";
                    x = document.getElementById("txtEcoLife").value;
                    try {
                        if (x == "") throw "empty";
                        if (isNaN(x)) throw "not a number";
                        x = Number(x);
                        if (x < 5) throw "too low";
                        if (x > 10) throw "too high";
                    }
                    catch (err) {
                        message.innerHTML = "Input is " + err;
                    }
                }
            </script>

            <div class="col-lg-6">
                <section class="panel">

                    <fieldset>
                        <legend>Fixed Assets Entry</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>


                            <tr>
                                <td>Purchase Type</td>
                                <td>
                                    <asp:DropDownList ID="ddlist" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlist_OnSelectedIndexChanged">
                                        <asp:ListItem Value="LC">LC</asp:ListItem>
                                        <asp:ListItem Value="Local">Local</asp:ListItem>
                                        <asp:ListItem Value="Transfer">Transfer</asp:ListItem>
                                        <asp:ListItem Value="Other">Other</asp:ListItem>
                                    </asp:DropDownList>


                                </td>
                            </tr>
                            <asp:Panel id="extrapanel" runat="server" >
                            <tr>
                                <td>LC No :</td>
                                <td>
                                    <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Country Of Origin :</td>
                                <td>
                                    <asp:TextBox ID="TextBox5" runat="server"  CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Control Head Asset Type:</td>
                                <td>
                                    <asp:DropDownList ID="ddAccountsHeadName" runat="server" CssClass="form-control" DataSourceID="SqlDataSource1" DataTextField="ControlAccountsName" DataValueField="ControlAccountsName"></asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" SelectCommand="SELECT ControlAccountsName FROM ControlAccount WHERE (ControlAccountsID = '010205')"></asp:SqlDataSource>
                                </td>
                            </tr>
                            </asp:Panel>

                            <tr>
                                <td>Account Head :</td>
                                <td>
                                    <asp:DropDownList ID="ddAccountHead" runat="server" CssClass="form-control select2me" DataSourceID="SqlDataSource2" DataTextField="AccountsHeadName" DataValueField="AccountsHeadName"></asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" SelectCommand="SELECT HeadSetup.AccountsHeadName FROM AccountGroup INNER JOIN HeadSetup INNER JOIN ControlAccount ON HeadSetup.ControlAccountsID = ControlAccount.ControlAccountsID INNER JOIN Accounts ON HeadSetup.AccountsID = Accounts.AccountsID ON AccountGroup.GroupID = HeadSetup.GroupID where HeadSetup.ControlAccountsID='010205' order by AccountGroup.GroupID, HeadSetup.AccountsHeadID"></asp:SqlDataSource>
                                </td>
                            </tr>


                            <tr>
                                <td>Purchase Date :</td>
                                <td>
                                    <asp:TextBox ID="txtPurchaseDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="cePurchaseDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtPurchaseDate" />
                                </td>
                            </tr>


                            <tr>
                                <td>Depreciation Method</td>
                                <td>
                                    <asp:TextBox ID="txtDepMethod" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>


                            <tr>
                                <td>Economic usefull Life</td>
                                <td>
                                    <asp:TextBox ID="txtEcoLife" runat="server" onkeyup="myFunction()" CssClass="form-control"></asp:TextBox>
                                    <p id="p01"></p>
                                    <asp:FilteredTextBoxExtender ID="ftbEcoLife" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtEcoLife" />

                                </td>
                            </tr>


                            <tr>
                                <td>System Process Run</td>
                                <td>
                                    <asp:DropDownList ID="ddProcesstype" runat="server" CssClass="form-control">
                                        <asp:ListItem>Monthly</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>


                            <tr>
                                <td>Quantity :</td>
                                <td>
                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" onclick="myFunction()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbAmount" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtAmount" />
                                </td>
                            </tr>
                            <tr>
                                <td>Serial No :</td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" onclick="myFunction()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtAmount" />
                                </td>
                            </tr>
                            <tr>
                                <td>Entry Date :</td>
                                <td>
                                    <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" onclick="myFunction()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtAmount" />
                                </td>
                            </tr>
                            <tr>
                                <td>Amount :</td>
                                <td>
                                    <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" onclick="myFunction()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtAmount" />
                                </td>
                            </tr>


                            <tr style="background: none">
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" Text="Save" OnClick="btnSave_OnClick" />
                                    <asp:Button ID="btnClear" type="reset" CssClass="btn btn-s-md btn-white" runat="server" Text="Cancel" OnClick="btnClear_OnClick" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </section>
            </div>

            <div class="col-lg-6">
                <section class="panel">
                    <fieldset>
                        <legend>Saved Data</legend>
                        <div class="table-responsive">
                            <asp:GridView Width="180%" ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                GridLines="Vertical" DataKeyNames="Id" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
                                <Columns>

                                    <asp:TemplateField HeaderText="#Sl" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>.
                                            <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("Id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PurchaseType" HeaderText="Purchase Type" SortExpression="PurchaseType" />
                                    <asp:BoundField DataField="AccountsHeadName" HeaderText="Accounts Head Name" SortExpression="AccountsHeadName" />
                                    <asp:BoundField DataField="AccountHead" HeaderText="Account Head" SortExpression="AccountHead" />
                                    <asp:BoundField DataField="PurchaseDate" HeaderText="Purchase Date" DataFormatString="{0:d}" SortExpression="PurchaseDate" />
                                    <asp:BoundField DataField="DepMethod" HeaderText="Dep Method" SortExpression="DepMethod" />
                                    <asp:BoundField DataField="EcoLife" HeaderText="Eco Life" SortExpression="EcoLife" />
                                    <asp:BoundField DataField="SystemProcessRun" HeaderText="System Process Run" SortExpression="SystemProcessRun" />
                                    <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/edit.png" Text="Select" />
                                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif" Text="Delete" />
                                            <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
                                            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                            <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                <b style="color: red">This entry will be deleted permanently!</b><br />
                                                Are you sure you want to delete this ?<br />
                                                <br />
                                                <div style="text-align: right;">
                                                    <asp:Button ID="ButtonOk" runat="server" CssClass="btn btn-success" Text="OK" />
                                                    <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                </div>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                <SelectedRowStyle BackColor="#EEF7F2" Font-Bold="True" ForeColor="#615B5B" />
                                <HeaderStyle BackColor="#FF6600" Font-Bold="True" ForeColor="#222" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </div>

                    </fieldset>
                </section>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>





