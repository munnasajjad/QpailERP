<%@ Page Title="Voucher Entry by Quantity" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Voucher-Qty.aspx.cs" Inherits="app_Voucher_Qty" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--<link href="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0-beta.3/css/select2.min.css" />
        <script src="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0-beta.3/js/select2.min.js"></script>


        <link href="../Libs/Calculator/jquery.calculator.css" rel="stylesheet" />
<script src="../Libs/Calculator/jquery.calculator.min.js"></script>
    <script src="../Libs/Calculator/jquery.plugin.min.js"></script>--%>

    <script type="text/javascript">
        $(document).ready(function () {
            //GroupDropDownList();
        });
        function ddchangeval(sel) {
            var route = $('#<%=ddParticular.ClientID%> option:selected').val();
            //$('#<%=txtDescription.ClientID%>').val(route.toString());
            //GroupDropDownList();
        }



        function calTtl() {
            var rate = $('#<%=txtRate.ClientID%>').val();
            var qty = $('#<%=txtQty.ClientID%>').val();
            var cfrBDT = parseFloat(rate) * parseFloat(qty) * 1;
            $('#<%=txtAmount.ClientID%>').val(cfrBDT.toString());
        }
        //jQuery(window).load(function() {
        //    GroupDropDownList();
        //});

        function GroupDropDownList() {
            //Create groups for dropdown list
            $("select#ctl00_BodyContent_ddAccHeadDr option[optiongroup='Assets']").wrapAll("<optgroup label='Assets'>");
            $("select#ctl00_BodyContent_ddAccHeadDr option[optiongroup='Expenses']").wrapAll("<optgroup label='Expenses'>");
            $("select#ctl00_BodyContent_ddAccHeadDr option[optiongroup='Equity']").wrapAll("<optgroup label='Equity'>");
            $("select#ctl00_BodyContent_ddAccHeadDr option[optiongroup='Incomes']").wrapAll("<optgroup label='Incomes'>");
            $("select#ctl00_BodyContent_ddAccHeadDr option[optiongroup='Liabilities']").wrapAll("<optgroup label='Liabilities'>");

            $("select#ctl00_BodyContent_ddAccHeadCr option[optiongroup='Assets']").wrapAll("<optgroup label='Assets'>");
            $("select#ctl00_BodyContent_ddAccHeadCr option[optiongroup='Expenses']").wrapAll("<optgroup label='Expenses'>");
            $("select#ctl00_BodyContent_ddAccHeadCr option[optiongroup='Equity']").wrapAll("<optgroup label='Equity'>");
            $("select#ctl00_BodyContent_ddAccHeadCr option[optiongroup='Incomes']").wrapAll("<optgroup label='Incomes'>");
            $("select#ctl00_BodyContent_ddAccHeadCr option[optiongroup='Liabilities']").wrapAll("<optgroup label='Liabilities'>");
        }

        //$(function () {
        //    $('#ctl00_BodyContent_txtAmount').calculator({
        //        showOn: 'button', buttonImageOnly: true, buttonImage: '../Libs/Calculator/calculator.png'
        //    });
        //});

    </script>


    <style type="text/css">
        label {
            text-align: left !important;
        }

        .table1 {
            width: 100%;
        }

        optgroup {
            color: #23A6F0;
        }

        #ctl00_BodyContent_btnAdd {
            margin-top: 2px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>--%>





            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <h3 class="page-title">Vouchers With Quantity Entry</h3>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">

                        <div class="portlet box blue">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>
                                    <asp:Literal ID="Literal2" runat="server" Text="Voucher Info" />
                                    <asp:LinkButton ID="lbRefresh" runat="server" OnClick="lbRefresh_OnClick">Refresh</asp:LinkButton>
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
                                        <label class="control-label">Voucher Date : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDate" CssClass="span6 m-wrap" runat="server"
                                                AutoPostBack="True" OnTextChanged="txtDate_OnTextChanged"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Voucher Type : </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddParticular" runat="server"
                                                DataSourceID="SqlDataSource1" DataTextField="Particularsname"
                                                DataValueField="Particularsid" CssClass="span6" onchange="ddchangeval(this);"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddParticular_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                SelectCommand="SELECT Particularsid, [Particularsname] FROM [Particulars] ORDER BY [Particularsname]"></asp:SqlDataSource>
                                        </div>
                                    </div>


                                    <asp:Panel ID="pnl3" runat="server" DefaultButton="btnAdd">
                                        <div class="form-group">
                                            <label class="control-label">A/C (Dr.) :</label>
                                            <div class="controls">
                                                <asp:DropDownList ID="ddControlDr" runat="server" CssClass="form-control select2me" onchange="ddchangeval(this);"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddControlDr_OnSelectedIndexChanged">
                                                </asp:DropDownList>

                                                <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                    SelectCommand="SELECT AccountsHeadID, [AccountsHeadName] FROM [HeadSetup] WHERE IsFixed=0 ORDER BY [AccountsHeadName]"></asp:SqlDataSource>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label"><%--A/C Head To (Dr.) :--%> &nbsp;</label>
                                            <div class="controls">
                                                <asp:DropDownList ID="ddAccHeadDr" runat="server" CssClass="form-control select2me" onchange="ddchangeval(this);"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddAccHeadDr_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddHead5Dr" runat="server" CssClass="form-control select2me" Visible="False"></asp:DropDownList>
                                                <asp:Label ID="lblSl" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblUser" runat="server" Visible="false"></asp:Label>

                                                <asp:Button ID="btnRefresh" runat="server" Text="R..." OnClick="btnRefresh_Click" Width="35px" Visible="False" />
                                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                    SelectCommand="SELECT AccountsHeadID, [AccountsHeadName] FROM [HeadSetup] WHERE IsFixed=0 ORDER BY [AccountsHeadName]"></asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="control-label">A/C (Cr.) :</label>
                                            <div class="controls">
                                                <asp:DropDownList ID="ddControlCr" runat="server" CssClass="form-control select2me" onchange="ddchangeval(this);"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddControlCr_OnSelectedIndexChanged">
                                                </asp:DropDownList>

                                                <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                    SelectCommand="SELECT AccountsHeadID, [AccountsHeadName] FROM [HeadSetup] WHERE IsFixed=0 ORDER BY [AccountsHeadName]"></asp:SqlDataSource>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label">&nbsp;</label>
                                            <div class="controls">
                                                <asp:DropDownList ID="ddAccHeadCr" runat="server" CssClass="form-control select2me" onchange="ddchangeval(this);"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddAccHeadCr_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddHead5Cr" runat="server" CssClass="form-control select2me" Visible="False"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Description :</label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Rate-Qty-Amount:</label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtRate" runat="server" Width="20%" onkeyup="calTtl()"></asp:TextBox>
                                                <asp:TextBox ID="txtQty" runat="server" Width="20%" onkeyup="calTtl()"></asp:TextBox>
                                                <asp:TextBox ID="txtAmount" runat="server" Width="30%" ReadOnly="True"></asp:TextBox>
                                                <%--<img src="../Libs/Calculator/calculator.png" onclick="calculator()" />--%>
                                                <asp:FilteredTextBoxExtender ID="txtOpBalance_FilteredTextBoxExtender"
                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtRate">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtQty">
                                                </asp:FilteredTextBoxExtender>

                                                <div class="right">
                                                    <asp:Button ID="btnAdd" runat="server" Text="Add to Grid" OnClick="btnAdd_Click" Width="100px" />
                                                </div>
                                            </div>
                                        </div>


                                        <div class="form-actions">
                                        </div>



                                        <div class="table-responsive">
                                        <asp:GridView ID="GridView2" runat="server"
                                            OnRowDataBound="GridView2_RowDataBound" Width="120%" AutoGenerateColumns="False"
                                            OnRowDeleting="GridView2_RowDeleting" OnSelectedIndexChanged="GridView2_SelectedIndexChanged"
                                            BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE"
                                            GridLines="Vertical" >
                                            <RowStyle BackColor="#F7F7DE" />
                                            <Columns>

                                                <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Sl." SortExpression="CrID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSl" runat="server" Text='<%# Bind("SerialNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="A/C Head (Dr.)" SortExpression="CrID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHeadNameDr" runat="server" Text='<%# Bind("AccountsHeadDrName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="A/C Head From (Cr.)" SortExpression="CrID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHeadNameCr" runat="server" Text='<%# Bind("AccountsHeadCrName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Description" SortExpression="CrID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("VoucherRowDescription") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rate" SortExpression="CrID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantity" SortExpression="CrID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total" SortExpression="CrID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDr" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
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
                                            <SelectedRowStyle BackColor="#FFCA2C" Font-Bold="True" ForeColor="#615B5B" />
                                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="#106AAB" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                            </div>
                                        <%--<asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [SerialNo], [VoucherRowDescription], AccountsHeadDr, AccountsHeadDrName, AccountsHeadCr, AccountsHeadCrName, Amount
                                            FROM [VoucherTmp] WHERE ([EntryBy] = @EntryBy) AND VoucherNo=@VoucherNo ORDER BY [SerialNo]"
                                            DeleteCommand="Delete [VoucherDetails] where [SerialNo]=0">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblUser" Name="EntryBy" PropertyName="Text" Type="String" />
                                                <asp:ControlParameter ControlID="txtEditVoucherNo" Name="VoucherNo" PropertyName="Text" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>--%>

                                    </asp:Panel>
                                     <asp:TextBox ID="txtEditVoucherNo" runat="server" Visible="false" Text=""></asp:TextBox>
                                    <br />


                                    <div class="control-group">
                                        <label class="control-label">Total Quantity & Amount : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTQty" CssClass="span6 m-wrap" runat="server" Text="0" Enabled="false"></asp:TextBox>
                                            <asp:TextBox ID="txtTTL" CssClass="span6 m-wrap" runat="server" Text="0" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-actions">
                                        <asp:CheckBox ID="chkPrint" runat="server" Checked="false" Text="Print" Visible="false" />
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save Voucher" OnClick="btnSave_Click" />

                                        <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Reload" OnClick="btnClear_Click" />
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
                                    <asp:Literal ID="Literal1" runat="server" Text="Vouchers List" />
                                </div>
                            </div>
                            <div class="portlet-body form">


                                <asp:GridView ID="GridView1" runat="server" Width="100%" AllowSorting="True"
                                    AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                                    BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black"
                                    GridLines="Vertical" DataSourceID="SqlDataSource3"
                                    OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
                                    <Columns>

                                                <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>

                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                               <asp:Label ID="lblID" runat="server" Text='<%# Bind("VID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Voucher No." SortExpression="VoucherNo">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%#"./reports/voucher.aspx?inv=" + Eval("VoucherNo") %>'>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("VoucherNo") %>'></asp:Label>
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="VoucherDate" HeaderText="Date" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" SortExpression="VoucherDate" />
                                        <asp:BoundField DataField="VoucherDescription" HeaderText="Particular" ItemStyle-HorizontalAlign="Center" SortExpression="VoucherDescription" />
                                        <asp:BoundField DataField="Qty" HeaderText="Total Qty" ItemStyle-HorizontalAlign="Center" SortExpression="Qty" />
                                        <asp:BoundField DataField="VoucherAmount" HeaderText="Amount" ItemStyle-HorizontalAlign="Right" SortExpression="VoucherAmount" DataFormatString="{0:N}" />



                                                <asp:TemplateField ShowHeader="False" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit Voucher" />
                                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Cancel Voucher" />

                                                        <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                        </asp:ConfirmButtonExtender>
                                                        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                            PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                        <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                            <b style="color: red">Entered voucher will be cancelled!</b><br />
                                                            Are you sure you want to remove the item from vouchers list?
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
                                    <FooterStyle BackColor="#CCCCCC" />
                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#106AAB" Font-Bold="True" ForeColor="#CC0000" />
                                    <AlternatingRowStyle BackColor="#CCCCCC" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                    SelectCommand="SELECT VID, [VoucherNo], [VoucherDate], [VoucherDescription], [VoucherAmount], (Select SUM(Qty) from VoucherTmp WHERE VoucherNo=VoucherMaster.VoucherNo) AS Qty FROM [VoucherMaster] WHERE ([VoucherDate] = @VoucherDate) AND Voucherpost<>'C' AND (VoucherNo Not Like 'Auto%')  ORDER BY [VID] DESC"
                                    DeleteCommand="Delete [VoucherDetails] where [SerialNo]=0">
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

            <%--<div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <h3 class="page-title">Vouchers List

                        </h3>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <!-- BEGIN SAMPLE FORM PORTLET-->
                        <div class="portlet box blue">

                            <div class="portlet-body form">
                                <div class="form-horizontal">

                                </div>
                            </div>
                        </div>
                    </div>

                </div>

            </div>--%>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

