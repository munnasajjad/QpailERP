<%@ Page Title="Shareholders Equity" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="ShareholdersEquity.aspx.cs" Inherits="app_ShareholdersEquity" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .table-responsive {
            margin-bottom: 0px;
            margin-right: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">
                        <asp:Literal ID="ltrFrmName" runat="server" Text="Shareholders Equity" />
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="ltrSubFrmName" runat="server" Text="Create Shareholders Equity" />
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                                <%-- <div class="control-group">
                                    <label class="control-label">Financial Year</label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddGroup" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Literal ID="lblGID" runat="server"></asp:Literal>
                                    </div>
                                </div>--%>
                                <div class="control-group">
                                <asp:Label class="control-group"  runat="server" Text="Financial Year :" Font-Bold="True"></asp:Label>
                                    <asp:DropDownList ID="ddFinYear" runat="server" DataSourceID="SqlDataSource6" DataTextField="Financial_Year" DataValueField="Financial_Year" AutoPostBack="true" OnSelectedIndexChanged="ddFinYear_SelectedIndexChanged" ></asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" SelectCommand="SELECT [Financial_Year] FROM [tblFinancial_Year]"></asp:SqlDataSource>
                                    </div>
                                <div class="control-group">
                                    <asp:Label  runat="server" Text="OP Date :" Font-Bold="True"></asp:Label>
                                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalenderExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDate"></asp:CalendarExtender>                                </div>
                                <div class="control-group hidden">
                                    <label class="control-label">OP Date</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtSAID" runat="server" Enabled="false" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                                        <asp:Literal ID="lblOldSubAcId" runat="server" Visible="false"></asp:Literal>
                                       <%--  <asp:CalendarExtender ID="ce2" runat="server" 
                                    Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>--%>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">OP Balance :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtTextBox5" runat="server" title="Write OP Balance and press Enter" CssClass="span6 m-wrap"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label ID="Label3" class="control-label">No. OF Shares :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtSubAcc" runat="server" title="Write No. OF Shares and press Enter" CssClass="span6 m-wrap"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Revaluation of Non-current Asset :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtTextBox6" runat="server" title="Write Revaluation of Non-current Asset and press Enter" CssClass="span6 m-wrap"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Tax Holiday Reserve :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtTextBox2" runat="server" title="Write Tax Holiday Reserve and press Enter" CssClass="span6 m-wrap"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Retained Earning :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtTextBox1" runat="server" title="Write Retained Earning and press Enter" CssClass="span6 m-wrap"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Total :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtTextBox3" runat="server" title="Write Total and press Enter" CssClass="span6 m-wrap"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Net Profit for the year :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtTextBox4" runat="server" title="Write Net Profit for the year and press Enter" CssClass="span6 m-wrap"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label"> Divident Paid :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtTextBox7" runat="server" title="Write Divident Paid and press Enter" CssClass="span6 m-wrap"></asp:TextBox>
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


                <div class="col-md-12">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="Literal1" runat="server" Text="Created Shareholders Equity" />
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="table-responsive">

                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table-responsive"
                                    OnRowDeleting="GridView1_RowDeleting" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                    DataSourceID="SqlDataSource2" Width="100%" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="SrNo" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SN" InsertVisible="False" SortExpression="id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinYear" HeaderText="Financial Year"
                                            SortExpression="FinYear" InsertVisible="False" ReadOnly="True" />

                                        <asp:BoundField DataField="OPDate" HeaderText="OP Date" SortExpression="OPDate" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="OPBalance" HeaderText="OP Balance" SortExpression="OPBalance" DataFormatString="{0:#,##,###.00}" />
                                        <asp:BoundField DataField="NoOFShares" HeaderText="No OF Shares" SortExpression="NoOFShares" DataFormatString="{0:#,##,###.00}" />
                                        <asp:BoundField DataField="RevaluationOfNonCurrentAsset" HeaderText="Revaluation Of Non Current Asset" SortExpression="RevaluationOfNonCurrentAsset" DataFormatString="{0:#,##,###.00}" />
                                        <asp:BoundField DataField="TaxHolidayReserve" HeaderText="Tax Holiday Reserve" SortExpression="TaxHolidayReserve" DataFormatString="{0:#,##,###.00}" />
                                        <asp:BoundField DataField="RetainedEarning" HeaderText="Retained Earning" SortExpression="RetainedEarning" DataFormatString="{0:#,##,###.00}" />
                                        <asp:BoundField DataField="NetProfitForTheYear" HeaderText="Net Profit For The Year" SortExpression="NetProfitForTheYear" DataFormatString="{0:#,##,###.00}"></asp:BoundField>
                                        <asp:BoundField DataField="DividentPaid" HeaderText="Divident Paid" SortExpression="DividentPaid" DataFormatString="{0:#,##,###.00}" />
                                        <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:#,##,###.00}" />
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" Visible="false" />

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

                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" SelectCommand="SELECT [id], [FinYear], [OPDate], [OPBalance], [NoOFShares], [RevaluationOfNonCurrentAsset], [TaxHolidayReserve], [RetainedEarning], [NetProfitForTheYear], [DividentPaid], [Total] FROM [Shareholdersequitydb]"></asp:SqlDataSource>

                                
                            </div>
                        </div>
                    </div>

                 </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>