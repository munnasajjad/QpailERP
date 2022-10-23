<%@ Page Title="LCVoucher" Language="C#" MasterPageFile="~/app/MasterPage.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="LCVoucher.aspx.cs" Inherits="app_LCVoucher" %>

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
                <h3 class="page-title">LC Vouchers Entry</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">

                <div class="portlet box blue">
                    <div class="portlet-title">
                        <div class="caption">
                            <i class="fa fa-reorder"></i>
                            <asp:Literal ID="Literal2" runat="server" Text="LC Voucher Info" />
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

                            <%--<div class="control-group">
                                    <label class="control-label">LC No. :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtLCNo" runat="server"></asp:TextBox>
                                    </div>
                                </div>--%>
                            <asp:Panel ID="Panel1" runat="server">

                                    <div class="form-group">
                                        <label class="control-label">
                                            LC No. :<br />
                                            <asp:LinkButton ID="lbLCNo" runat="server" OnClick="lbLCNo_OnClick">New</asp:LinkButton>
                                            
                                            <%--<asp:LinkButton ID="lbLCFilter" runat="server" OnClick="lbLCFilter_OnClick">Show-all</asp:LinkButton>--%>
                                        </label>
                                        <asp:DropDownList ID="ddLCNo" runat="server" DataSourceID="SqlDataSource8" DataTextField="LCNo" DataValueField="LCNo"
                                             CssClass="select2me" Width="70%" AutoPostBack="True" OnSelectedIndexChanged="ddLCNo_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [sl], [LCNo] FROM [LC]  WHERE IsActive<>'D' ORDER BY [sl] DESC"></asp:SqlDataSource>

                                        <asp:TextBox ID="txtLCNo" runat="server" Visible="False" CssClass="form-control" />
                                    </div>
                                </asp:Panel>
                            

                            <div class="control-group">
                                <label class="control-label">LC Voucher Date : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtDate" CssClass="span6 m-wrap" runat="server"
                                        AutoPostBack="True" OnTextChanged="txtDate_OnTextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtDate">
                                    </asp:CalendarExtender>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label">LC Voucher Type : </label>
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
                                    <label class="control-label">Amount (Tk.)</label>
                                    <div class="controls">

                                        <asp:TextBox ID="txtAmount" runat="server" Width="100px"></asp:TextBox>
                                        <%--<img src="../Libs/Calculator/calculator.png" onclick="calculator()" />--%>
                                        <asp:FilteredTextBoxExtender ID="txtOpBalance_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtAmount">
                                        </asp:FilteredTextBoxExtender>

                                        <div class="col-md-4">
                                            <asp:Button ID="btnAdd" runat="server" Text="Add to Grid" OnClick="btnAdd_Click" />
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
                                        GridLines="Vertical">
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
                                            <asp:TemplateField HeaderText="Amount" SortExpression="CrID">
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
                                <label class="control-label">Total Amount : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtTTL" CssClass="span6 m-wrap" runat="server" Text="0" Enabled="false"></asp:TextBox>
                                </div>
                            </div>

                            

                            <a name="ItemDetails"></a>
                            <legend style="margin-bottom: 6px;">LC Item Details :</legend>
                            <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>
                            

                            <div class="control-group hidden">
                                <asp:Label ID="Label9" runat="server" Text="Purpose :"></asp:Label>
                                <asp:DropDownList ID="ddPurpose" runat="server" DataSourceID="SqlDataSource1mm"
                                    DataTextField="Purpose" DataValueField="pid">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1mm" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [pid], [Purpose] FROM [Purpose] order by Purpose">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>

                            <div class="control-group">
                                <label class="control-label">Group : </label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddGroup" runat="server" DataSourceID="SqlDataSource6" AutoPostBack="true"
                                        DataTextField="GroupName" DataValueField="GroupSrNo" OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] WHERE GroupSrNo<>2 AND GroupSrNo<>3 ORDER BY [GroupSrNo]"></asp:SqlDataSource>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label">Sub-group :  </label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddSubGrp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddSubGrp_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label">Grade :  </label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddGrade" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddGrade_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label">Category :  </label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label">Item :  </label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                    </asp:DropDownList>

                                    <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>
                                    <%--<span style="width: 70%; color: green; float: right">
                                        <asp:Literal ID="ltrLastInfo" runat="server" EnableViewState="False">Recent Purchase Info: </asp:Literal>
                                    </span>--%>
                                </div>
                            </div>
                            
                            <div class="control-group">
                                    <label class="control-label">HS Code :  </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtHSCode" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                           <asp:Panel ID="pnlSpec" runat="server">

                                    <div class="control-group">
                                        <label class="control-label">
                                            Specification:<br />
                                            <asp:LinkButton ID="lbSpec" runat="server" OnClick="lbSpec_OnClick">New</asp:LinkButton>
                                            | 
                                                        <asp:LinkButton ID="lbFilter" runat="server" OnClick="lbFilter_OnClick">Show-all</asp:LinkButton>
                                        </label>
                                        <asp:DropDownList ID="ddSpec" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddSpec_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource14" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [id], [spec] FROM [Specifications] ORDER BY [spec]"></asp:SqlDataSource>

                                        <asp:TextBox ID="txtSpec" runat="server" Visible="False" />
                                    </div>
                                </asp:Panel>


                                <div id="pkSizeField" runat="server" class="control-group">
                                    <label class="control-label">Pack Size :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddSize" runat="server" DataSourceID="SqlDataSource11"
                                            DataTextField="BrandName" DataValueField="BrandID">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT BrandID, BrandName FROM [Brands] ORDER BY [DisplaySl]"></asp:SqlDataSource>

                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">
                                            <asp:Literal ID="Literal3" runat="server" Text=" Quantity (Pcs) : " />
                                        </label>
                                        <asp:TextBox ID="txtWeight" runat="server" />
                                    </div>
                                </div>
                            

                                    <div class="control-group">
                                        <label class="control-label">
                                            <asp:Literal ID="ltrSerial" runat="server" Text="Serial No. : " />
                                        </label>
                                        <asp:TextBox ID="txtSerial" runat="server" />
                                    </div>

                                <asp:Panel ID="PanelMachine" runat="server" Visible="False">
                                    
                                    <div class="control-group">
                                        <label class="control-label">
                                            <asp:Literal ID="Literal4" runat="server" Text="Model No. : " />
                                        </label>
                                        <asp:TextBox ID="txtModel" runat="server" />
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">
                                            <asp:Literal ID="Literal5" runat="server" Text="Specification : " />
                                        </label>
                                        <asp:TextBox ID="txtSpecification" runat="server" />
                                    </div>

                                </asp:Panel>

                                    <div class="control-group">
                                        <label class="control-label">
                                            <asp:Literal ID="ltrWarrenty" runat="server" Text="Warrentry : " />
                                        </label>
                                        <asp:TextBox ID="txtWarrenty" runat="server" />
                                    </div>



                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="Literal6" runat="server" Text="Manufacturer" />:
                                    </label>
                                    <asp:TextBox ID="txtManufacturer" runat="server" />
                                </div>

                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="Literal7" runat="server" Text="Country of Origin" />
                                        :
                                    </label>
                                    <asp:TextBox ID="txtCountry" runat="server" />
                                </div>
                                <%--<div class="control-group">
                                    <label class="control-label">Ref./Spec</label>
                                    <asp:TextBox ID="txtRef" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>--%>

                                <%--<div id="SectionField" runat="server" class="control-group hidden">
                                    <label class="control-label">For Section :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddSection" runat="server" DataSourceID="SqlDataSource22"
                                            DataTextField="SName" DataValueField="SID">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource22" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [SID], [SName] FROM [Sections] WHERE ([DepartmentID] = @DepartmentID) ORDER BY [SName]">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="5" Name="DepartmentID" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>--%>


                                <div class="control-group">
                                    <label class="control-label">Quantity (<asp:Literal ID="ltrUnitType" runat="server" />) : </label>
                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" onkeyup="calTtl()" width="35%"></asp:TextBox>

                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtQuantity">
                                    </asp:FilteredTextBoxExtender>

                                </div>
                            <div class="control-group">
                                    <label class="control-label">Amount :  </label>
                                    <asp:TextBox ID="txtRate" runat="server" CssClass="form-control" onkeyup="calTtl()" Width="35%"></asp:TextBox>
                                    <%--<asp:TextBox ID="txtSubTotal" runat="server" CssClass="form-control" ReadOnly="True" Width="35%"></asp:TextBox>--%>
                                    <asp:Button ID="Button1" runat="server" CssClass="button" Text="Add to grid" OnClick="btnAddItem_Click" />
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtRate">
                                    </asp:FilteredTextBoxExtender>

                                </div>


                                <div class="control-group">
                                    <%--<label class="control-label">Stock Type : </label>--%>
                                    <asp:DropDownList ID="ddStockType" runat="server" CssClass="hidden">
                                        <asp:ListItem Value="Raw">Raw Stock</asp:ListItem>
                                        <asp:ListItem Value="Fixed">Fixed Assets</asp:ListItem>
                                        <asp:ListItem Value="Temporary">Temporary Item</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:Button ID="Button1" runat="server" CssClass="button" Text="Add to grid" OnClick="btnAddItem_Click" />--%>
                                </div>


                                <div style="clear: both"></div>


                                <div class="table-responsive">

                                <asp:GridView ID="ItemGrid" runat="server" AutoGenerateColumns="False" OnRowDeleting="ItemGrid_RowDeleting"
                                    Width="250%" DataKeyNames="EntryID" OnRowDataBound="ItemGrid_RowDataBound" AutoGenerateSelectButton="True" OnSelectedIndexChanged="ItemGrid_OnSelectedIndexChanged">

                                    <Columns>

                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="EntryID" SortExpression="EntryID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("EntryID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Purpose" SortExpression="EntryID">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1h" runat="server" Text='<%# Bind("Purpose") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Grade" SortExpression="Product">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2p" runat="server" Text='<%# Bind("Grade") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Category" SortExpression="Product">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2o" runat="server" Text='<%# Bind("Category") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Product" SortExpression="Product">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Product") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Pk Size" SortExpression="Size">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1xz" runat="server" Text='<%# Bind("Size") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Thickness(mm)/ Serial#" SortExpression="HSCode">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1x" runat="server" Text='<%# Bind("Thickness") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Spec" SortExpression="HSCode">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSpec" runat="server" Text='<%# Bind("Spec") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model/ Measurement" SortExpression="HSCode">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1x1" runat="server" Text='<%# Bind("Measurement") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="QTY" SortExpression="QTY1">
                                            <ItemTemplate>
                                                <asp:Label ID="QTY9" runat="server" Text='<%# Bind("QTY1") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit Price (USD)" SortExpression="UnitPrice">
                                            <ItemTemplate>
                                                <asp:Label ID="UnitPrice" runat="server" Text='<%# Bind("UnitPrice") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Value (USD)" SortExpression="CFRValue">
                                            <ItemTemplate>
                                                <asp:Label ID="CFRValue" runat="server" Text='<%# Bind("CFRValue") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <%--<asp:CommandField ButtonType="Image" ControlStyle-Width="24px" DeleteImageUrl="~/App_Themes/Blue/img/error-s.png" ShowDeleteButton="True"  SelectImageUrl="~/app/images/edit.png" ShowSelectButton="true" />--%>

                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>

                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">Item will be deleted permanently!</b><br />
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


                                <asp:SqlDataSource ID="SqlDataSource9" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT EntryID, 
                                                (Select Purpose from Purpose where pid=a.Purpose) as Purpose, 
                                                (Select GradeName from ItemGrade where GradeID=a.GradeId) as Grade, 
                                                (Select CategoryName from Categories where CategoryID=a.CategoryId) as Category, 
                                                (Select ItemName from Products where ProductID=a.ItemCode) as Product,                      
                                                [Thickness], (Select BrandName from Brands where BrandID=a.ItemSizeID) as Size, Measurement, 
                                                (Select spec from Specifications where id=a.spec) as spec, NoOfPacks ,
                                                CONVERT(varchar(10), qty) +' '+(Select UnitType from Products where ProductID=a.ItemCode) As QTY1, UnitPrice,  [CFRValue] 
                                                FROM [LcItems] a Where  LCNo='' ORDER BY [EntryID]"
                                    DeleteCommand="DELETE LcItems WHERE EntryID=@EntryID"
                                    UpdateCommand="">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddLCNo" Name="LCNo" PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                    <DeleteParameters>
                                        <asp:Parameter Name="EntryID" />
                                    </DeleteParameters>
                                </asp:SqlDataSource>
                            </div>
                            

                                <div class="full_block right bold">
                                    <br />
                                Total item amount: <asp:Label ID="lblItemAmount" runat="server" ></asp:Label>
                                 <br /> <br />
                                </div>
                            <div class="control-group">
                                    <label class="control-label">Status : </label>
                                    <div class="controls">
                                        <asp:CheckBox ID="cbStatus" runat="server" Checked="False" Text="Finalized" AutoPostBack="True" OnCheckedChanged="cbStatus_OnCheckedChanged" />
                                        
                                        <%--<asp:CheckBox ID="chkDisable" CssClass="radiobtn" runat="server" Text="Disable" Checked="true"  />--%>

                                    </div>
                                </div>

                            <div class="control-group">
                                    <label class="control-label">Stock-in Godown : </label>
                                    <asp:DropDownList ID="ddGodown" runat="server" Visible="False" DataSourceID="SqlDataSource7"
                                        DataTextField="StoreName" DataValueField="WID" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT WID,[StoreName] FROM [Warehouses]"></asp:SqlDataSource>
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
                            <asp:Literal ID="Literal1" runat="server" Text="LC Vouchers List" />
                        </div>
                    </div>
                    <div class="portlet-body form">


                        <span style="color: Maroon;">
                            <asp:Label ID="Label2" runat="server" EnableViewState="false"></asp:Label>
                            <asp:Label ID="lblErrLoad" runat="server"></asp:Label>
                            <asp:Label ID="lblProject" runat="server" Visible="false" />
                            <asp:Label ID="lblInvoice" runat="server" Text="" Visible="false" />
                        </span>

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
                                <asp:BoundField DataField="VoucherDate" HeaderText="Voucher Date" DataFormatString="{0:d}"
                                    ItemStyle-HorizontalAlign="Center" SortExpression="VoucherDate" />
                                <asp:BoundField DataField="VoucherDescription" HeaderText="Particular"
                                    ItemStyle-HorizontalAlign="Center" SortExpression="VoucherDescription" />
                                <asp:BoundField DataField="VoucherAmount" HeaderText="Amount"
                                    ItemStyle-HorizontalAlign="Right" SortExpression="VoucherAmount" />


                                <asp:TemplateField ShowHeader="False" ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" Visible="False" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit Voucher" />
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
                            SelectCommand="SELECT VID, [VoucherNo], [VoucherReferenceNo], [VoucherDate], [VoucherDescription], [VoucherAmount] FROM [VoucherMaster] 
                            WHERE ([VoucherReferenceNo] = @VoucherReferenceNo) AND Voucherpost<>'C' AND (VoucherNo Not Like 'Auto%') ORDER BY [VID] DESC"
                            DeleteCommand="Delete [VoucherDetails] where [SerialNo]=0">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddLCNo" Name="VoucherReferenceNo"
                                    PropertyName="SelectedValue" />
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


