<%@ Page Title="Production A/C Linkup" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Production-AccountsLinkup.aspx.cs" Inherits="app_Production_AccountsLinkup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">
                        <asp:Literal ID="ltrFrmName" runat="server" Text="Production Accounts Linkup" />
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="ltrSubFrmName" runat="server" Text="Update Control Accounts" />
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

							
							
                            <asp:Label ID="lblMsg" runat="server" EnableViewState="false" ></asp:Label>

                            <div class="portlet-body form">
                                <div class="form-horizontal">
                                    <div class="control-group">
                                <label class="control-label">Particulars : </label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddParticular" runat="server" DataTextField="Particularsname"
                                        DataValueField="Particularsid" CssClass="span6" AutoPostBack="true" OnSelectedIndexChanged="ddParticular_OnSelectedIndexChanged">
                                        <asp:ListItem Value="0">--- All ---</asp:ListItem>
                                        <asp:ListItem Value="0101">Stock issue to production floor</asp:ListItem>
                                        <asp:ListItem Value="0102">Transfer to machine</asp:ListItem>
                                        <asp:ListItem Value="0103">Semi-finished production</asp:ListItem>
                                        <asp:ListItem Value="0104">Finished goods output</asp:ListItem>
                                        <asp:ListItem Value="0105">Recycle transfer to store</asp:ListItem>
                                        <asp:ListItem Value="0106">Reject goods transfer to crush</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                                <div class="control-group">
                                    <label class="control-label">Accounts Group : </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddGroup" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddGroup_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Literal ID="lblGID" runat="server"></asp:Literal>
                                    </div>
                                </div>

                                    <div class="control-group">
                                        <label class="control-label">Sub A/C Name : </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddSub" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddSub_SelectedIndexChanged">
                                            </asp:DropDownList>

                                            <asp:Literal ID="SubID" runat="server"></asp:Literal>

                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Control A/C ID :</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtNid" runat="server" Enabled="false" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                                            <asp:Label ID="lblOldSubAcId" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Control A/C Name :</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtControl" runat="server" title="Write Sub-Account Name and press Enter" CssClass="span6 m-wrap"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Update" OnClick="btnSave_Click" />
                                        <%--<asp:Button ID="btnDelete" runat="server" Text="Delete This Schedule"  CssClass="btn red" 
                                onclick="btnDelete_Click" />
                                <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnDelete"  ConfirmText="confirm to delete this schedule ??"> 
                                 </asp:ConfirmButtonExtender> --%>
                                        <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" />
                                    </div>

                                </div>

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

                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" Width="100%"
                                        OnRowDeleting="GridView2_RowDeleting" OnSelectedIndexChanged="GridView2_SelectedIndexChanged"
                                        DataSourceID="SqlDataSource1">
                                        <Columns>
                                            
                                <asp:TemplateField HeaderText="SrNo" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                </asp:TemplateField>

                                            <asp:TemplateField HeaderText="C.Accounts ID" SortExpression="ControlAccountsID">                                                
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("ControlAccountsID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AccountsName" HeaderText="Accounts Name"
                                                SortExpression="AccountsName" />
                                            <asp:BoundField DataField="ControlAccountsName" HeaderText="Control Accounts Name"
                                                SortExpression="ControlAccountsName" />

                                            
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" Visible="False" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">A/C Head will be deleted permanently!</b><br />
                                                    Are you sure you want to delete the item from order list?
                                                            <br />
                                                    <br />
                                                    <div style="text-align: right;">
                                                        <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                        <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                    </div>
                                                </asp:Panel>

                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        </Columns>
                                    </asp:GridView>
                                    <br />

                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT a.ControlAccountsID, b.AccountsName, a.ControlAccountsName FROM ControlAccount a join Accounts b on a.AccountsID=b.AccountsID
                                         where a.AccountsID=@AccountsID AND a.PrdnLinkId=@PrdnLinkId  order by a.ControlAccountsName "
                                        DeleteCommand="Delete ControlAccount where ControlAccountsID=0">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddSub" Name="AccountsID" PropertyName="SelectedValue" />
                                            <asp:ControlParameter ControlID="ddParticular" Name="PrdnLinkId" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>
                            </div>
                        </div>
                    </div>
           

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


