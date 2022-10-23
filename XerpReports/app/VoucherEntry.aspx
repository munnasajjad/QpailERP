<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="VoucherEntry.aspx.cs" Inherits="Oxford.app.VoucherEntry" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--<link href="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0-beta.3/css/select2.min.css" />
        <script src="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0-beta.3/js/select2.min.js"></script>

    
        <link href="../Libs/Calculator/jquery.calculator.css" rel="stylesheet" />
<script src="../Libs/Calculator/jquery.calculator.min.js"></script>
    <script src="../Libs/Calculator/jquery.plugin.min.js"></script>--%>

    <script type="text/javascript">
        function ddchangeval(sel) {
            var route = $('#<%=ddParticular.ClientID%> option:selected').val();
            $('#<%=txtDescription.ClientID%>').val(route.toString());
        }

        //$(function () {
        //    $('#ctl00_BodyContent_txtAmount').calculator({
        //        showOn: 'button', buttonImageOnly: true, buttonImage: '../Libs/Calculator/calculator.png'
        //    });            
        //});

    </script>
    <%--<script type="text/javascript">
        $('select').select2();
        $(document).ready(function () {
            $('select').select2();
        });
    </script>--%>

    <style type="text/css">
        label {
            text-align: left !important;
        }

        .table1 {
            width: 100%;
        }
    </style>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="pan" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="grid_12 full_block">
                <div class="widget_wrap">
                    <div class="widget_top">
                        <span class="h_icon list_image"></span>
                        <h6>Voucher Entry</h6>
                    </div>

                    
<div class="grid_6 full_block">
                    <div class="widget_content">

                        <asp:Label ID="Label2" runat="server" EnableViewState="false"></asp:Label>

                        <div class="form_container left_label field_set">

                            <div class="grid_4 full_block">

                                <fieldset>
                                    <legend><b>»</b>Voucher Info</legend>



                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                                    <div class="portlet-body form">
                                        <div class="form-horizontal">

                                            <div class="control-group">
                                                <label class="control-label">Voucher No. : </label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtVID" runat="server" ReadOnly="true"></asp:TextBox>

                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Particular : </label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddParticular" runat="server"
                                                        DataSourceID="SqlDataSource1" DataTextField="Particularsname"
                                                        DataValueField="Detail" CssClass="span6" onchange="ddchangeval(this);">
                                                    </asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                                        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                        SelectCommand="SELECT Detail, [Particularsname] FROM [Particulars] ORDER BY [Particularsname]"></asp:SqlDataSource>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Entry Date : </label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtDate" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                        Enabled="True" TargetControlID="txtDate">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>

                                            <div class="form-actions">
                                                <asp:CheckBox ID="chkPrint" runat="server" Checked="false" Text="Print" Visible="false" />
                                                <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save Voucher" OnClick="btnSave_Click" />

                                                <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Refresh" />
                                            </div>

                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            </div>
                            </div>
</div>

<div class="grid_6 full_block">


                            <div class="grid_8 full_block">

                                <fieldset class="field_set">
                                    <legend><b>»</b> Accounts Head Entry</legend>

                                    <asp:Panel ID="pnl3" runat="server" DefaultButton="btnAdd">

                                        <table class="table1" width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label9" runat="server" Text="A/C Head : "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddAccHead" runat="server"></asp:DropDownList>
                                                    <asp:Label ID="lblSl" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblUser" runat="server" Visible="false"></asp:Label>
                                                    <%--<asp:DropDownList ID="ddAccHead" runat="server"  class="chzn-select chzn-done" 
                                                        DataSourceID="SqlDataSource2" DataTextField="AccountsHeadName"
                                                        DataValueField="AccountsHeadID">
                                                    </asp:DropDownList>--%>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnRefresh" runat="server" Text="R..." OnClick="btnRefresh_Click" Width="35px" />
                                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                                        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                        SelectCommand="SELECT AccountsHeadID, [AccountsHeadName] FROM [HeadSetup] WHERE IsFixed=0 ORDER BY [AccountsHeadName]"></asp:SqlDataSource>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>

                                                    <asp:Label ID="Label8" runat="server" Text="Description : "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>


                                            <tr>
                                                <td>Amount</td>
                                                <td>
                                                    <asp:TextBox ID="txtAmount" runat="server" Width="100px"></asp:TextBox>
                                                    <%--<img src="../Libs/Calculator/calculator.png" onclick="calculator()" />--%>
                                                    <asp:FilteredTextBoxExtender ID="txtOpBalance_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtAmount">
                                                    </asp:FilteredTextBoxExtender>
                                                    Tk.</td>
                                                <td>&nbsp;</td>
                                            </tr>


                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="RadioButton1" CssClass="radiobtn" runat="server" Text="Dr." Checked="true" GroupName="g" /></td>
                                                            <td>
                                                                <asp:RadioButton ID="RadioButton2" CssClass="radiobtn" runat="server" Text="Cr." GroupName="g" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTTL" runat="server" Visible="false"></asp:TextBox>
                                                </td>
                                            </tr>


                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>

                                                    <asp:Button ID="btnAdd" runat="server" Text="Add Data to Grid" OnClick="btnAdd_Click" />
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>


                                            <tr>
                                                <td colspan="3">

                                                    <asp:GridView ID="GridView2" runat="server"
                                                        OnRowDataBound="GridView2_RowDataBound" Width="100%" AutoGenerateColumns="False"
                                                        OnRowDeleting="GridView2_RowDeleting" OnSelectedIndexChanged="GridView2_SelectedIndexChanged" BackColor="White" BorderColor="#DEDFDE"
                                                        BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                                        GridLines="Vertical" DataSourceID="SqlDataSource4">
                                                        <RowStyle BackColor="#F7F7DE" />
                                                        <Columns>

                                                            <asp:TemplateField ItemStyle-Width="40px">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Sl." SortExpression="CrID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSl" runat="server" Text='<%# Bind("SerialNo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Head_ID" SortExpression="CrID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblHeadId" runat="server" Text='<%# Bind("AccountsHeadID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Head Name" SortExpression="CrID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblHeadName" runat="server" Text='<%# Bind("AccountsHeadName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Description" SortExpression="CrID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("VoucherRowDescription") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dr." SortExpression="CrID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDr" runat="server" Text='<%# Bind("VoucherDR") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cr." SortExpression="CrID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCr" runat="server" Text='<%# Bind("VoucherCR") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
                                                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />

                                                                    <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                                    </asp:ConfirmButtonExtender>
                                                                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                                        PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                                    <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                                        <b style="color: red">Entry will be deleted!</b><br />
                                                                        Are you sure you want to delete the item from entry list?
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
                                                        <FooterStyle BackColor="#CCCC99" />
                                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="#615B5B" />
                                                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="#106AAB" />
                                                        <AlternatingRowStyle BackColor="White" />
                                                    </asp:GridView>
                                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT [SerialNo], [VoucherRowDescription], [AccountsHeadID], [AccountsHeadName], [VoucherDR], [VoucherCR] FROM [VoucherTmp] WHERE ([EntryBy] = @EntryBy) ORDER BY [SerialNo]">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="lblUser" Name="EntryBy" PropertyName="Text" Type="String" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                </td>
                                            </tr>
                                        </table>

                                    </asp:Panel>

                                </fieldset>
                            </div>
                            </div>
                        </div>

                    </div>
                </div>

            </div>

            <div class="row-fluid">
                <div class="span12">
                    <div class="portlet box yellow">
                        <div class="portlet-title">
                            <h4 style="text-align: center;"><i class="icon-coffee"></i>Todays All Vouchers</h4>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="javascript:;" class="reload"></a>
                                <a href="javascript:;" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body">

                            <asp:GridView ID="GridView1" runat="server" Width="100%" AllowSorting="True"
                                AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                                BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black"
                                GridLines="Vertical" DataSourceID="SqlDataSource3">
                                <Columns>
                                    <asp:TemplateField HeaderText="Voucher No." SortExpression="VoucherNo">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%#"./reports/voucher.aspx/?inv=" + Eval("VoucherNo") %>'>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("VoucherNo") %>'></asp:Label>
                                            </asp:HyperLink>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="VoucherDate" HeaderText="Voucher Date" DataFormatString="{0:d}"
                                        ItemStyle-HorizontalAlign="Center" SortExpression="VoucherDate" />
                                    <asp:BoundField DataField="VoucherDescription" HeaderText="Particular"
                                        ItemStyle-HorizontalAlign="Center" SortExpression="VoucherDescription" />
                                    <asp:BoundField DataField="VoucherAmount" HeaderText="Amount"
                                        ItemStyle-HorizontalAlign="Right" SortExpression="VoucherAmount" />
                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" />
                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#106AAB" Font-Bold="True" ForeColor="#CC0000" />
                                <AlternatingRowStyle BackColor="#CCCCCC" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                SelectCommand="SELECT [VoucherNo], [VoucherDate], [VoucherDescription], [VoucherAmount] FROM [VoucherMaster] WHERE ([VoucherDate] = @VoucherDate) ORDER BY [VID] DESC">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="txtDate" Name="VoucherDate"
                                        PropertyName="Text" Type="DateTime" />
                                </SelectParameters>
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

