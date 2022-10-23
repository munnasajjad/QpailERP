<%@ Page Title="Head Setup" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Head.aspx.cs" Inherits="Application_Head" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <script type="text/javascript">
    function calTtl() {
            var rate = $('#<%=txtRate.ClientID%>').val();
            var qty = $('#<%=txtQty.ClientID%>').val();
            var cfrBDT = parseFloat(rate) * parseFloat(qty) * 1;
            $('#<%=txtOpBalance.ClientID%>').val(cfrBDT.toString());
    }

    </script>
 <style>
     
      span.radiobtn {
          margin: 0px !important;
          padding: 0 !important;
      }
 </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--<asp:UpdatePanel ID="upnl" runat="server" UpdateMode="Always">
        <ContentTemplate>
             <script type="text/javascript" language="javascript">
                 Sys.Application.add_load(callJquery);
            </script>--%>



            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">
                        <asp:Literal ID="ltrFrmName" runat="server" Text="Accounts Head" />
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="ltrSubFrmName" runat="server" Text="Create Accounts Head" />
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


                                <div class="control-group hidden">
                                    <label class="control-label">Head Type :  </label>
                                    <div class="controls">
                                        <asp:RadioButton ID="rbUser" runat="server" Checked="true" Text="User Defined" GroupName="rd" />
                                        <asp:RadioButton ID="rbSystem" runat="server" Checked="false" Text="System (Locked)" GroupName="rd" />
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Account Group : </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddGroup" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                        </asp:DropDownList>

                                        <asp:Label ID="lblGroup" runat="server" Visible="false"></asp:Label>

                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Subsidiary Account :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddSub" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddSub_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblSub" runat="server" Visible="false"></asp:Label>
                                        <asp:Button ID="btnSubRefresh" CssClass="btn" runat="server" Text="R..." OnClick="btnSubRefresh_Click" Width="35px" Visible="False" />

                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="control-label">Control Account : </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddControl" runat="server" CssClass="form-control select2me" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddControl_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblControl" runat="server" Visible="false"></asp:Label>
                                        <asp:Button ID="btnControlRefresh" runat="server" Text="R..." OnClick="btnControlRefresh_Click" Width="35px" Visible="False"/>


                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">A/C Head ID : </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtHeadID" runat="server" Enabled="False"></asp:TextBox>
                                        <asp:Label ID="lblHeadID" runat="server" Visible="false"></asp:Label>

                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">A/C Head Name : </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtHeadName" runat="server"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Opening Stock Qty/Kg.-Rate-Pcs: </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtQty" runat="server" Width="93.33" onkeyup="calTtl()"></asp:TextBox>
                                        <asp:TextBox ID="txtRate" runat="server" Text="0" Width="93.33" onkeyup="calTtl()"></asp:TextBox>
                                        <asp:TextBox ID="txtPcs" runat="server" Text="0" Width="93.33"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtRate">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtQty">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtPcs">
                                        </asp:FilteredTextBoxExtender>
                                    </div>
                                </div>
                                

                                <div class="control-group">
                                    <label class="control-label">Opening Balance : </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtOpBalance" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtOpBalance_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtOpBalance">
                                        </asp:FilteredTextBoxExtender>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label"></label>
                                    <div class="controls">
                                        <asp:RadioButton ID="RadioButton1" CssClass="radiobtn" runat="server" Text="Dr." Checked="true" GroupName="g" />
                                        <asp:RadioButton ID="RadioButton2" CssClass="radiobtn" runat="server" Text="Cr." GroupName="g" />

                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Date of Initialization :  </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Format="dd/MM/yyyy" Enabled="True" TargetControlID="txtDate">
                                        </asp:CalendarExtender>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Status : </label>
                                    <div class="controls">
                                        <asp:CheckBox ID="cbStatus" runat="server" Checked="True" Text="Active" />

                                        <%--<asp:CheckBox ID="chkDisable" CssClass="radiobtn" runat="server" Text="Disable" Checked="true"  />--%>

                                    </div>
                                </div>


                                <div class="form-actions">
                                    <asp:Button ID="btnSave" CssClass="btn" runat="server" Text="Save New Head" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" OnClick="btnClear_Click" />
                                </div>

                            </div>
                        </div>
                    </div>

                </div>

                <div class="col-md-7">
                    <div class="portlet box red">

                        <div class="portlet-body form">

                            <div class="form-body">

                                <legend>List of Account Heads</legend>
                                <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>

                                <div class="table-responsive">

                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" Width="100%"
                                        CssClass="table-responsive"
                                        DataKeyNames="AccountsHeadID" DataSourceID="SqlDataSource1"
                                        OnRowDataBound="GridView2_RowDataBound" OnSelectedIndexChanged="GridView2_SelectedIndexChanged" OnRowDeleting="GridView2_RowDeleting" AllowSorting="True">
                                        <Columns>

                                            <asp:TemplateField ItemStyle-Width="20px" HeaderText="Sl.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>.
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Head ID" SortExpression="AccountsHeadID">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("AccountsHeadID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="GroupName" HeaderText="Acc. Group" ReadOnly="true" SortExpression="GroupName" Visible="False" />
                                            <asp:BoundField DataField="AccountsName" HeaderText="Sub. Account" ReadOnly="true" SortExpression="AccountsName"  Visible="False" />
                                            <asp:BoundField DataField="ControlAccountsName" ReadOnly="true" HeaderText="Control Account" SortExpression="Control Accounts"  Visible="False" />
                                            <asp:BoundField DataField="AccountsHeadName" HeaderText="Accounts Head" ReadOnly="true"
                                                SortExpression="AccountsHeadName" />

                                            <asp:TemplateField HeaderText="Op.Bal (Dr.)" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("OpBalDr") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Op.Bal (Cr.)" SortExpression="Op.Balance">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("OpBalCr") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="60px">
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
                                    <br />

                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT HeadSetup.AccountsHeadID, AccountGroup.GroupName, Accounts.AccountsName, ControlAccount.ControlAccountsName, HeadSetup.AccountsHeadName, HeadSetup.AccountsOpeningBalance
                                    , HeadSetup.OpBalDr, HeadSetup.OpBalCr FROM AccountGroup INNER JOIN HeadSetup INNER JOIN ControlAccount ON HeadSetup.ControlAccountsID = ControlAccount.ControlAccountsID INNER JOIN Accounts ON HeadSetup.AccountsID = Accounts.AccountsID ON AccountGroup.GroupID = HeadSetup.GroupID
                                    where HeadSetup.ControlAccountsID=@ControlAccountsID order by AccountGroup.GroupID, HeadSetup.AccountsHeadID"
                                        UpdateCommand="Update HeadSetup set AccountsOpeningBalance =@AccountsOpeningBalance where  AccountsHeadID=@AccountsHeadID"
                                        DeleteCommand="DELETE HeadSetup WHERE EntryID=0" >

                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddControl" Name="ControlAccountsID" PropertyName="SelectedValue" />
                                        </SelectParameters>

                                        <UpdateParameters>
                                            <asp:Parameter Name="AccountsOpeningBalance" />
                                            <asp:Parameter Name="AccountsHeadID" />

                                        </UpdateParameters>
                                    </asp:SqlDataSource>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

