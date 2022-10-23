<%@ Page Title="Bank Accounts" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Bank.aspx.cs" Inherits="Bank" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">
                        <asp:Literal ID="ltrFrmName" runat="server" Text="Bank Info" />
                    </h3>
                </div>
            </div>
            <div class="row">


                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="ltrEntryForm" runat="server" Text="Bank Info" />
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

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false" ></asp:Label>

                                <div id="EditField" runat="server">
                                    <label>Edit Info For: </label>
                                    <asp:DropDownList ID="DropDownList1" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3" DataTextField="BankName" DataValueField="BankId" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT BankId, BankName FROM [Banks] where Type=@Type ORDER BY [BankName]">
                                        <SelectParameters>
                                            <asp:QueryStringParameter Name="Type" QueryStringField="type" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="form-group hidden">
                                    <label>
                                        <asp:Literal ID="Literal1" runat="server" Text="Bank Name" />
                                    </label>
                                    <asp:DropDownList ID="ddCategory" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource2" DataTextField="GroupName" DataValueField="GroupName">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [GroupName] FROM [ItemGroup] ORDER BY [GroupName]"></asp:SqlDataSource>
                                </div>


                                <div class="form-group">
                                    <label>
                                        <asp:Literal ID="ltrName" runat="server" Text="Bank Name" />
                                    </label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" EnableViewState="true" placeholder="" />
                                </div>
                                
                                <div class="form-group">
                                    <label>
                                        <asp:Literal ID="ltrPhone" runat="server" Text="Head-office Phone No." />
                                    </label>
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" EnableViewState="true" placeholder="" />
                                </div>
                                
                                <div class="form-group">
                                    <label>
                                        <asp:Literal ID="ltrAddress" runat="server" Text="Remark/Description:" />
                                    </label>
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="" />
                                </div>

                                <div class="form-group">
                                    <label>
                                        <asp:Literal ID="Literal2" runat="server" Text="Contact Person" />
                                    </label>
                                    <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" EnableViewState="true" placeholder="" />
                                </div>

                                <div class="form-group">
                                    <label>
                                        <asp:Literal ID="Literal3" runat="server" Text="Mobile Number" />
                                    </label>
                                    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" EnableViewState="true" placeholder="" />
                                </div>
                                
                                <div class="form-group">
                                    <label>
                                        <asp:Literal ID="ltrEmail" runat="server" Text="Email Address" />
                                    </label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" EnableViewState="true" placeholder="" />
                                </div>
                                
                                <div class="form-group">
                                    <label>
                                        <asp:Literal ID="Literal4" runat="server" Text="Website" />
                                    </label>
                                    <asp:TextBox ID="txtUrl" runat="server" CssClass="form-control" EnableViewState="true" placeholder="" />
                                </div>
                                
                                <div class="form-group">
                                    <label>
                                        <asp:Literal ID="Literal5" runat="server" Text="Swift Code" />
                                    </label>
                                    <asp:TextBox ID="txtSwift" runat="server" CssClass="form-control" EnableViewState="true" placeholder="" />
                                </div>
                                
                                <div class="form-actions">
                                    <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click1" />
                                    <asp:Button ID="btnClear" CssClass="btn default" runat="server" Text="Cancel" OnClick="btnClear_Click1" />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>


                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Saved Data
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


                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="BankId">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BankId" SortExpression="BankId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("BankId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" SortExpression="BankName">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("BankName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Address" SortExpression="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="URL" SortExpression="Description">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lblUrl" runat="server" NavigateUrl='<%# Bind("url") %>' Text="Link" Target="_blank"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                    </Columns>
                                </asp:GridView>


                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT BankId, BankName, Description, url FROM [Banks] where type=@type ORDER BY [BankName]"
                                    UpdateCommand="">
                                    <SelectParameters>
                                        <asp:QueryStringParameter Name="Type" QueryStringField="type" />
                                    </SelectParameters>
                                </asp:SqlDataSource>


                            </div>
                        </div>

                    </div>

                </div>


            </div>


        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>


