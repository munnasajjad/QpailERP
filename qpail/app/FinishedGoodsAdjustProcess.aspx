<%@ Page Title="Finished Goods Adjustment Process" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="FinishedGoodsAdjustProcess.aspx.cs" Inherits="app_FinishedGoodsAdjustProdess" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <h3 class="page-title">Finished Goods Adjustment Process</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">

                <div class="portlet box blue">
                    <div class="portlet-title">
                        <div class="caption">
                            <i class="fa fa-reorder"></i>
                            <asp:Literal ID="Literal2" runat="server" Text="Stock Transaction Info" />
                            <asp:LinkButton ID="lbRefresh" runat="server" >Refresh</asp:LinkButton>
                        </div>
                    </div>
                    <div class="portlet-body form">
                        <div class="form-horizontal">

                            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                            <div class="control-group hidden">
                                <label class="control-label">Voucher No. : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtVID" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            
                            <div class="control-group">
                                <label class="control-label">Particulars : </label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddParticular" runat="server" DataTextField="Particularsname"
                                        DataValueField="Particularsid" CssClass="span6" onchange="ddchangeval(this);"
                                        AutoPostBack="true" >
                                        <%--<asp:ListItem Value="0101">Stock issue to production floor</asp:ListItem>
                                        <asp:ListItem Value="0102">Mixing in the production floor</asp:ListItem>
                                        <asp:ListItem Value="0102">Transfer to machine</asp:ListItem>
                                        <asp:ListItem Value="0103">Semi-finished production</asp:ListItem>--%>
                                        <asp:ListItem Value="0104">Finished goods output</asp:ListItem>
                                        <%--<asp:ListItem Value="0105">Recycle transfer to store</asp:ListItem>
                                        <asp:ListItem Value="0106">Reject goods transfer to crush</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <%--<asp:Panel ID="pnl3" runat="server" DefaultButton="btnAdd">--%>
                                <div class="form-group">
                                    <label class="control-label">Finished goods control name :</label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddControlDr" runat="server" CssClass="form-control select2me" DataSourceID="SqlDataSource4" DataValueField="ControlAccountsID" DataTextField="name" AutoPostBack="True" >
                                        </asp:DropDownList>

                                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                            SelectCommand="Select ControlAccountsID, (Select AccountsName from Accounts where AccountsID=ControlAccount.AccountsID)+' > '+ControlAccountsName as name from ControlAccount where PrdnLinkId=@Particular">
                                            <SelectParameters>
						                    <asp:ControlParameter ControlID="ddParticular" Name="Particular" PropertyName="SelectedValue" />
					                     </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="control-label"><%--A/C Head To (Dr.) :--%> &nbsp;</label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddAccHeadDr" runat="server" CssClass="form-control select2me" DataSourceID="SqlDataSource2" DataValueField="AccountsHeadID" DataTextField="AccountsHeadName" AutoPostBack="True" >
                                        </asp:DropDownList>
                                        <%--<asp:DropDownList ID="ddHead5Dr" runat="server" CssClass="form-control select2me"></asp:DropDownList>--%>
                                        <asp:Label ID="lblSl" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lblUser" runat="server" Visible="false"></asp:Label>

                                        <asp:Button ID="btnRefresh" runat="server" Text="R..." Width="35px" Visible="False" />
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                            SelectCommand="Select AccountsHeadID, AccountsHeadName from HeadSetup WHERE ControlAccountsID=@ControlId">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddControlDr" Name="ControlId" PropertyName="SelectedValue"/>
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>
                                
                            <%--</asp:Panel>--%>
                            <asp:TextBox ID="txtEditVoucherNo" runat="server" Visible="false" Text=""></asp:TextBox>
                            <br />


                            

                            <div class="form-actions">
                                <asp:CheckBox ID="chkPrint" runat="server" Checked="false" Text="Print" Visible="false" />
                                <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Run Process" OnClick="btnSave_OnClick" />
                                <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Reload" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            

            

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" Runat="Server">
</asp:Content>

