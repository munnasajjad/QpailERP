﻿<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="VoucherCancel.aspx.cs" Inherits="Application_VoucherCancel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        
    </script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="pan" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="grid_12 full_block">
                <div class="widget_wrap">
                    <div class="widget_top">
                        <span class="h_icon list_image"></span>
                        <h6>Voucher Approval</h6>
                    </div>
                    <div class="widget_content">


                        <asp:Label ID="Label3" runat="server" EnableViewState="false"></asp:Label>

                        <div class="form_container left_label field_set">

                            <div class="grid_6 full_block">

                                <fieldset>
                                    <legend><b>»</b>Voucher Info</legend>



                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                                    <div class="portlet-body form">
                                        <div class="form-horizontal">

                                            <div class="control-group">
                                                <label class="control-label">
                                                    <asp:Label ID="Label4" runat="server" Text="Unapproved Voucher#"></asp:Label>
                                                </label>
                                                <div class="controls">

                                                    <asp:DropDownList ID="ddVID" runat="server" AutoPostBack="true" Width="200px">
                                                    </asp:DropDownList>

                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    <asp:Label ID="Label5" runat="server" Text="Particulars"></asp:Label>
                                                </label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddParticular" runat="server"
                                                        DataSourceID="SqlDataSource1" DataTextField="Particularsname"
                                                        DataValueField="ParticularsID" Width="200px">
                                                    </asp:DropDownList>

                                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                                        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                        SelectCommand="SELECT ParticularsID, [Particularsname] FROM [Particulars] ORDER BY [Particularsname]"></asp:SqlDataSource>

                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    <asp:Label ID="lblEdu0" runat="server" Text="Voucher Entry Date : "></asp:Label>
                                                </label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                                    <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                        Enabled="True" TargetControlID="txtDate">
                                                    </asp:CalendarExtender>

                                                </div>
                                            </div>


                                            <asp:GridView ID="GridView2" runat="server"
                                                OnRowDataBound="GridView2_RowDataBound" Width="100%"
                                                OnRowDeleting="GridView2_RowDeleting" BackColor="White" BorderColor="#DEDFDE"
                                                BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                                GridLines="Vertical" AutoGenerateColumns="False" DataSourceID="SqlDataSource4">
                                                <RowStyle BackColor="#F7F7DE" />
                                                <Columns>
                                                    <asp:BoundField DataField="AccountsHeadName" HeaderText="Accound Head"
                                                        SortExpression="AccountsHeadName" />
                                                    <asp:BoundField DataField="VoucherRowDescription"
                                                        HeaderText="Entry Description" SortExpression="VoucherRowDescription" />
                                                    <asp:BoundField DataField="VoucherDR" HeaderText="DR"
                                                        SortExpression="VoucherDR" />
                                                    <asp:BoundField DataField="VoucherCR" HeaderText="CR"
                                                        SortExpression="VoucherCR" />
                                                </Columns>
                                                <FooterStyle BackColor="#CCCC99" />
                                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
 
                <asp:SqlDataSource ID="SqlDataSource4" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"                                         
                    SelectCommand="SELECT [AccountsHeadName], [VoucherRowDescription], [VoucherDR], [VoucherCR] FROM [VoucherDetails] WHERE ([VoucherNo] = @VoucherNo) ORDER BY [SerialNo]">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddVID" Name="VoucherNo" 
                            PropertyName="SelectedValue" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                                            <div class="form-actions">
                                                <asp:Button ID="btnSave" runat="server" Text="Cencel Voucher" CssClass="btn blue"
                                                    OnClick="btnSave_Click" />
                                                <asp:Button ID="btnUpdate" runat="server" Text="Print" OnClick="btnUpdate_Click" CssClass="btn red" />
                                            </div>

                                        </div>
                        </div>
                    </div>
                </div>

                <div class="row-fluid">
                    <div class="span12">
                        <div class="portlet box yellow">
                            <div class="portlet-title">
                                <h4><i class="icon-coffee"></i>Cancelled Vouchers</h4>
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
                                        <asp:TemplateField HeaderText="VoucherNo" SortExpression="VoucherNo">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("VoucherNo") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%#"./reports/voucher.aspx/?inv=" + Eval("VoucherNo") %>'>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("VoucherNo") %>'></asp:Label>
                                                </asp:HyperLink>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="VoucherDate" HeaderText="VoucherDate"
                                            SortExpression="VoucherDate" />
                                        <asp:BoundField DataField="VoucherDescription" HeaderText="VoucherDescription"
                                            SortExpression="VoucherDescription" />
                                        <asp:BoundField DataField="VoucherAmount" HeaderText="VoucherAmount"
                                            SortExpression="VoucherAmount" />
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" />
                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="#CCCCCC" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                    SelectCommand="SELECT [VoucherNo], [VoucherDate], [VoucherDescription], [VoucherAmount] FROM [VoucherMaster] WHERE ([VoucherDate] = @VoucherDate) AND Voucherpost='C' ORDER BY [VID] DESC">
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

