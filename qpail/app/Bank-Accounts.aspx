<%@ Page Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Bank-Accounts.aspx.cs" Inherits="app_Bank_Accounts" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">


    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <%--<asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>--%>


            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">Bank Account Information</h3>
                </div>
            </div>
            <div class="row">


                <div class="col-md-6 ">
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Bank Account Details
                            </div>

                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">
                                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>


                                <div id="EditField" runat="server">
                                    <label>Edit Info For: </label>
                                    <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource4"
                                        DataTextField="ACNo" DataValueField="ACID" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [ACID], ACNo FROM [BankAccounts] ORDER BY [ACNo]"></asp:SqlDataSource>
                                </div>


                                <div class="control-group">
                                    <asp:Label ID="Label12" runat="server" Text="Account Type :  "></asp:Label>
                                    <asp:DropDownList ID="ddType" runat="server">
                                        <asp:ListItem>CD</asp:ListItem>
                                        <asp:ListItem>OD</asp:ListItem>
                                        <asp:ListItem>Savings</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [AccountsHeadName], [AccountsHeadID] FROM [HeadSetup] WHERE ([ControlAccountsID] = @ControlAccountsID)">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="010102" Name="ControlAccountsID" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label4" runat="server"
                                        Text="Account Name : "></asp:Label>
                                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                </div>


                                <div class="control-group">
                                    <asp:Label ID="lblEname" runat="server" Text="A/C Number: "></asp:Label>
                                    <asp:TextBox ID="txtACno" runat="server"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label5" runat="server" Text="Bank Name :  "></asp:Label>
                                    <asp:DropDownList ID="ddBank" runat="server"
                                        DataSourceID="SqlDataSource2" DataTextField="BankName"
                                        DataValueField="BankId">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BankId], [BankName] FROM [Banks] WHERE ([Type] = @Type) ORDER BY [BankName]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="bank" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label3" runat="server" Text="Zone/ City :  "></asp:Label>
                                    <asp:DropDownList ID="ddZone" runat="server"
                                        DataSourceID="SqlDataSource3" DataTextField="AreaName"
                                        DataValueField="AreaID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT AreaID,[AreaName] FROM [Areas] ORDER BY [AreaName]"></asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="lblPermanent"
                                        runat="server" Text="Branch Address : "></asp:Label>
                                    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" CssClass="form-control"
                                        Height="60px" ></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label13" runat="server" Text="Email :  "></asp:Label>
                                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="lblCno" runat="server" Text="Contact No. :  "></asp:Label>
                                    <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789-," TargetControlID="txtMobile">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label1" runat="server" Text="Openning Balance :  "></asp:Label>
                                    <asp:TextBox ID="txtBalance" runat="server" Text="0"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789-." TargetControlID="txtBalance">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="form-actions">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnEdit" runat="server" Text="Cancel" OnClick="btnClear_Click1" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>



                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Bank Details
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">
                                <asp:GridView ID="GridView1" runat="server" Width="100%" AllowSorting="True"
                                    AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                                    BorderStyle="Solid" BorderWidth="1px" CellPadding="3"  OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                    DataSourceID="SqlDataSource5" ForeColor="Black" GridLines="Vertical" DataKeyNames="ACNo">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ProductID" SortExpression="ACID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("ACID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />--%>
                                        <asp:BoundField DataField="ACName" HeaderText="AC Name" SortExpression="ACName" />
                                        <asp:BoundField DataField="ACNo" HeaderText="AC No." ReadOnly="True" SortExpression="ACNo" />
                                        <asp:BoundField DataField="Bank" HeaderText="Bank" SortExpression="Bank" />
                                        <asp:BoundField DataField="Zone" HeaderText="Zone" SortExpression="Zone" />
                                        <asp:BoundField DataField="OpBalance" HeaderText="Op.Balance" SortExpression="OpBalance" />

                                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                    </Columns>
                                </asp:GridView>

                                <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT ACID, (select AccountsHeadName  FROM HeadSetup where AccountsHeadID=a.[TypeID]) as Type, [ACName], [ACNo], (SELECT BankName FROM Banks where BankId=a.[BankID]) as Bank, (SELECT AreaName FROM Areas where AreaID=a.[ZoneID]) as Zone, [OpBalance] FROM [BankAccounts] a ORDER BY [ACNo]"></asp:SqlDataSource>

                            </div>
                        </div>

                    </div>

                </div>


            </div>



        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

