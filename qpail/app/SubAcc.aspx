<%@ Page  Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="SubAcc.aspx.cs" Inherits="Application_SubAcc" %>

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
                        <asp:Literal ID="ltrFrmName" runat="server" Text="Subsidiary Accounts" />
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="ltrSubFrmName" runat="server" Text="Create Subsidiary Accounts" />
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                                <div class="control-group">
                                    <label class="control-label">Accounts Group </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddGroup" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Literal ID="lblGID" runat="server"></asp:Literal>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Subsidiary A/C ID  </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtSAID" runat="server" Enabled="false" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                                        <asp:Literal ID="lblOldSubAcId" runat="server" Visible="false"></asp:Literal>
                                        <%--<asp:CalendarExtender ID="ce2" runat="server" 
                                    Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>--%>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Subsidiary A/C Name</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtSubAcc" runat="server" title="Write Sub-Account Name and press Enter" CssClass="span6 m-wrap"></asp:TextBox>
                                    </div>
                                </div>


                                <div class="form-actions">
                                    <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save New Head" OnClick="btnSave_Click" />
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


                <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="Literal1" runat="server" Text="Create Subsidiary Accounts" />
                            </div>
                        </div>
                        <div class="portlet-body form">


                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table-responsive"
                                    OnRowDeleting="GridView1_RowDeleting" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                    DataSourceID="SqlDataSource1">
                                    <Columns>

                                        <asp:TemplateField HeaderText="SrNo" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Head ID" SortExpression="AccountsID">

                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("AccountsID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="GroupName" HeaderText="Group Name"
                                            SortExpression="GroupName" />
                                        <asp:BoundField DataField="AccountsName" HeaderText="Accounts Name"
                                            SortExpression="AccountsName" />


                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />

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

                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                    SelectCommand="SELECT a.AccountsID, b.GroupName, a.AccountsName FROM Accounts a join AccountGroup b on a.GroupID=b.GroupID order by a.AccountsID"
                                    DeleteCommand="Delete Accounts where AccountsID='0'"></asp:SqlDataSource>

                            </div>
                        </div>
                    </div>

                 </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

