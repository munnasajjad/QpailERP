<%@ Page Title="Bank Loan Types Setup" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="BankLoanTypes.aspx.cs" Inherits="app_BankloanTypes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
             <script type="text/javascript" language="javascript">
                 Sys.Application.add_load(callJquery);
            </script> 


            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <h3 class="page-title">Bank Loan Types Setup
                     <%--<small>form components and widgets</small>--%>
                        </h3>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <!-- BEGIN SAMPLE FORM PORTLET-->
                        <div class="portlet box blue">

                            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                            <asp:HiddenField runat="server" ID="LoanTypeIdHField"/>
                            <div class="portlet-body form">
                                <div class="form-horizontal">

                                    <div class="control-group">
                                        <label class="control-label">Loan Type :<br/>
                                       <asp:LinkButton ID="lbLLoanType" runat="server" OnClick="lbLLoanType_OnClick">New</asp:LinkButton>
                                      </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddLoanType" runat="server" DataSourceID="SqlDataSource2" DataTextField="LoanTypes" DataValueField="Id"></asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" 
                                                SelectCommand="SELECT [Id], [LoanTypes] FROM [BankLoanTypes]"></asp:SqlDataSource>
                                            <asp:TextBox ID="txtLoanType" runat="server" Visible="False" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="control-label">A/C Head Name : </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddAcheadId" runat="server" DataSourceID="SqlDataSource3"
                                                DataTextField="AccountsHeadName" DataValueField="AccountsHeadID" CssClass="form-control select2me" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>"
                                                SelectCommand="SELECT AccountsHeadID, [AccountsHeadName] FROM [HeadSetup] WHERE  (ControlAccountsID = '020101') OR (ControlAccountsID = '020201')"></asp:SqlDataSource>
                                        </div>
                                    </div>


                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click" />

                                        <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" />
                                    </div>

                                </div>

                            </div>

                        </div>

                    </div>

                    <div class="col-md-6">
                        <div class="portlet box red">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>
                                    <asp:Literal ID="Literal1" runat="server" Text="Saved Data" />
                                </div>
                            </div>
                            <div class="portlet-body form">

                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>"
                                    SelectCommand="SELECT        LoanTypes.LoanTypeId, LoanTypes.LoanType, BankLoanTypes.LoanTypes, LoanTypes.AccountsHeadID, LoanTypes.AccountsHeadName
FROM            LoanTypes INNER JOIN
                         BankLoanTypes ON LoanTypes.LoanType = BankLoanTypes.Id"></asp:SqlDataSource>


                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView2_SelectedIndexChanged" DataSourceID="SqlDataSource1" Width="387px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="LabelLoanTypeId" runat="server" Text='<%# Bind("LoanTypeId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="LoanTypes" HeaderText="LoanType" SortExpression="LoanTypes" />
                                        <asp:BoundField DataField="AccountsHeadName" HeaderText="AccountsHeadName" SortExpression="AccountsHeadName" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
