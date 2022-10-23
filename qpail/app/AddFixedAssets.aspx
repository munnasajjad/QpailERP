<%@ Page Title="AddFixedAssets" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="AddFixedAssets.aspx.cs" Inherits="Fixedassets" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .col-md-4 .control-group input, .col-md-4 .control-group select {
            width: 100%;
        }

        .col-md-4 .control-group label {
            padding-bottom: 4px;
        }

        ::selection {
            background: #a4dcec;
        }

        ::-moz-selection {
            background: #a4dcec;
        }

        ::-webkit-selection {
            background: #a4dcec;
        }

        ::-webkit-input-placeholder { /* WebKit browsers */
            color: #ccc;
            font-style: italic;
        }

        :-moz-placeholder { /* Mozilla Firefox 4 to 18 */
            color: #ccc;
            font-style: italic;
        }

        ::-moz-placeholder { /* Mozilla Firefox 19+ */
            color: #ccc;
            font-style: italic;
        }

        :-ms-input-placeholder { /* Internet Explorer 10+ */
            color: #ccc !important;
            font-style: italic;
        }

        br {
            display: block;
            line-height: 2.2em;
        }

        #searchfield {
            display: block;
            width: 100%;
            text-align: center;
            margin-bottom: 35px;
        }

            #searchfield span {
                display: inline-block;
                background: #eeefed;
                padding: 0;
                margin: 0;
                padding: 5px;
                border-radius: 3px;
                margin: 5px 0 0 0;
            }

                #searchfield span .biginput {
                    width: 600px;
                    height: 40px;
                    padding: 0 10px 0 10px;
                    background-color: #fff;
                    border: 1px solid #c8c8c8;
                    border-radius: 3px;
                    color: #aeaeae;
                    font-weight: normal;
                    font-size: 1.5em;
                    -webkit-transition: all 0.2s linear;
                    -moz-transition: all 0.2s linear;
                    transition: all 0.2s linear;
                }

                    #searchfield span .biginput:focus {
                        color: #858585;
                    }

        .autocomplete-suggestions {
            border: 1px solid #999;
            background: #fff;
            cursor: default;
            overflow: auto;
        }

        .autocomplete-suggestion {
            padding: 10px 5px;
            font-size: 1em;
            white-space: nowrap;
            overflow: hidden;
        }

        .autocomplete-selected {
            background: #f0f0f0;
        }

        .autocomplete-suggestions strong {
            font-weight: normal;
            color: #3399ff;
        }


        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td {
            border: 1px solid #ddd;
            color: GrayText;
        }

        input#ctl00_BodyContent_chkMerge, height_fix {
            height: 17px !important;
            top: -3px !important;
            margin-top: -5px !important;
        }

        bottom_fix {
            margin-bottom: -15px !important;
        }

        #ctl00_BodyContent_txtCurrentQty, #ctl00_BodyContent_txtCurrentKg, #ctl00_BodyContent_txtQty, #ctl00_BodyContent_txtStock, #ctl00_BodyContent_txtWeight {
            text-align: center;
            font-weight: bold;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {

        })
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>


            <div class="col-md-12">
                <!-- BEGIN SAMPLE FORM PORTLET-->
                <div class="portlet box green ">
                    <div class="portlet-title">
                        <div class="caption">
                            <i class="fa fa-reorder"></i>Fixed Assets Stock Register
                        </div>
                    </div>
                    <div class="portlet-body form">
                        <div class="form-horizontal" role="form">
                            <div class="form-body">
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>
                                <asp:LoginName ID="LoginName1" runat="server" Visible="false" />
                                <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>

                                <div class="col-md-12">
                                    <div class="row">

                                        <asp:Panel ID="pnlAdd" runat="server" DefaultButton="btnAdd">

                                            <div class="col-md-4 hidden">
                                                <div class="control-group">
                                                    <label class="control-label full-wdth">Entry Date : </label>

                                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                                        Enabled="True" TargetControlID="txtDate">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="control-label full-wdth">Fixed Assets Type </label>
                                                    <asp:DropDownList ID="ddGroup" runat="server" DataSourceID="SqlDataSource1" AutoPostBack="true"
                                                        DataTextField="GroupName" DataValueField="GroupSrNo" OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] WHERE (DepreciationType = '1') OR (DepreciationType = '2') ORDER BY [GroupSrNo]"></asp:SqlDataSource>

                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label">Item Type </label>

                                                    <asp:DropDownList ID="ddSubGroup" runat="server" DataSourceID="SqlDataSource3" DataTextField="CategoryName" DataValueField="CategoryID" AutoPostBack="True" OnSelectedIndexChanged="ddSubGroup_OnSelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT CategoryID, CategoryName FROM [ItemSubGroup] where (GroupID = @GroupID) ">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="ddGroup" Name="GroupID" PropertyName="SelectedValue" Type="String" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                </div>
                                            </div>


                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label">Grade </label>

                                                    <asp:DropDownList ID="ddGrade" runat="server" DataSourceID="SqlDataSource5"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddGrade_SelectedIndexChanged"
                                                        DataTextField="GradeName" DataValueField="GradeID">
                                                    </asp:DropDownList>

                                                    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT GradeID, GradeName from ItemGrade where CategoryID = @CategoryID ORDER BY [GradeName]">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="ddSubGroup" Name="CategoryID" PropertyName="SelectedValue" Type="String" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label">Category </label>

                                                    <asp:DropDownList ID="ddCategory" runat="server" DataSourceID="SqlDataSource8"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged"
                                                        DataTextField="CategoryName" DataValueField="CategoryID">
                                                    </asp:DropDownList>

                                                    <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT CategoryID, CategoryName FROM [Categories] where GradeID = @GradeID ORDER BY [CategoryName]">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="ddGrade" Name="GradeID" PropertyName="SelectedValue" />
                                                        </SelectParameters>

                                                    </asp:SqlDataSource>
                                                </div>
                                            </div>


                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label">Asset Type </label>

                                                    <asp:DropDownList ID="ddProduct" runat="server" DataSourceID="SqlDataSource4"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddProduct_SelectedIndexChanged"
                                                        DataTextField="ItemName" DataValueField="ProductID">
                                                    </asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE [CategoryID] = @CategoryID ORDER BY [ItemName]">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="ddCategory" Name="CategoryID" PropertyName="SelectedValue" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                    <%--SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE ([CategoryID] IN (Select CategoryID from Categories where GradeID IN (Select GradeID from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=2 AND ProjectID=1)))) ORDER BY [ItemName]"></asp:SqlDataSource>--%>
                                                </div>
                                            </div>

                                            <div class="col-md-4 hidden">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">Model No.</label>
                                                    <asp:TextBox ID="txtModel" runat="server" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">Date of Purchase</label>
                                                    <asp:TextBox ID="txtPurchaseDate" runat="server" AutoPostBack="True" OnTextChanged="txtPurchaseDate_OnTextChanged" />
                                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                        Enabled="True" TargetControlID="txtPurchaseDate">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="control-group" style="margin-top: 26px;">
                                                    <%--<label class="col-sm-12 control-label full-wdth">Item Code :</label>--%>
                                                    <asp:CheckBox ID="cbAuto" runat="server" Text="Auto Code" Checked="True" AutoPostBack="True" OnCheckedChanged="cbAuto_OnCheckedChanged" />
                                                    <asp:TextBox ID="txtItemCode" ReadOnly="True" runat="server" Width="59%" />

                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">Description</label>
                                                    <asp:TextBox ID="txtSpec" runat="server" />
                                                </div>
                                            </div>

                                            <div class="col-md-4 hidden">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">Warranty</label>
                                                    <asp:TextBox ID="txtWarranty" runat="server" />
                                                </div>
                                            </div>



                                            <div class="col-md-4 hidden">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">
                                                        Current FixedAssets Qty.(<asp:Literal ID="Literal1" runat="server" Visible="False" />pcs)</label>
                                                    <asp:TextBox ID="txtCurrentQty" runat="server" />
                                                </div>
                                            </div>

                                            <%--<div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">
                                                        Current FixedAssets Weight <asp:Literal ID="Literal5" runat="server" Visible="False" />kg</label>
                                                    <asp:TextBox ID="txtCurrentKg" runat="server"  />
                                                </div>
                                            </div>--%>


                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="control-label full-wdth">Manufacturer Name:</label>

                                                    <asp:TextBox ID="txtManufacture" runat="server" CssClass="form-control" placeholder="Manufacturer Name" />
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="control-label">
                                                    <label class="control-label full-wdth">Source</label>

                                                    <asp:DropDownList ID="ddSource" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddSource_OnSelectedIndexChanged" CssClass="form-control select2me">
                                                        <asp:ListItem>Local Purchase</asp:ListItem>

                                                        <asp:ListItem>Import using L/C</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                        SelectCommand="SELECT [AccountsHeadName],[AccountsHeadID] FROM [HeadSetup] ORDER BY [AccountsHeadName]"></asp:SqlDataSource>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="control-label full-wdth">LC No./  Invoice:</label>

                                                    <asp:TextBox ID="txtLCNo" runat="server" CssClass="form-control" placeholder="LC No. FTT" />
                                                </div>
                                            </div>
                                            <div class="col-md-4 hidden">
                                                <div class="control-group">
                                                    <label class="control-label full-wdth">Location:</label>

                                                    <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" placeholder="Location" />
                                                </div>
                                            </div>


                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">
                                                        Qty.(<asp:Literal ID="ltrUnit" runat="server" Visible="False" />pcs)</label>
                                                    <asp:TextBox ID="txtQty" OnTextChanged="txtQty_OnTextChanged" AutoPostBack="True" runat="server" onkeyup="calTotal()" />
                                                    <asp:Literal ID="ltrCQty" runat="server" Visible="False" />

                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtQty">
                                                    </asp:FilteredTextBoxExtender>
                                                </div>
                                            </div>

                                            <asp:Panel runat="server" ID="pnlUSD" Visible="False">

                                                <div class="col-md-4">
                                                    <div class="control-label">
                                                        <label class="control-label full-wdth">LC Account Head</label>

                                                        <asp:DropDownList ID="ddlchead" runat="server" DataValueField="AccountsHeadID" DataTextField="AccountsHeadName" DataSourceID="SqlDataSource6" AutoPostBack="True" CssClass="form-control select2me">
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                            SelectCommand="SELECT [AccountsHeadName],[AccountsHeadID] FROM [HeadSetup] ORDER BY [AccountsHeadName]"></asp:SqlDataSource>

                                                        <label class="control-label full-wdth">Depreciation Head</label>

                                                        <asp:DropDownList ID="DropDownList3" runat="server" DataValueField="AccountsHeadID" DataTextField="AccountsHeadName" AutoPostBack="True" CssClass="form-control select2me" DataSourceID="SqlDataSource9"></asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource9" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                            SelectCommand="SELECT        ControlAccount.ControlAccountsName, HeadSetup.AccountsHeadID, HeadSetup.AccountsHeadName
FROM            ControlAccount INNER JOIN
                         HeadSetup ON ControlAccount.ControlAccountsID = HeadSetup.ControlAccountsID
WHERE        (ControlAccount.ControlAccountsName = 'Manufacturing expenses')"></asp:SqlDataSource>
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="control-label full-wdth">Unit Price (USD):</label>

                                                        <asp:TextBox ID="txtRateUSD" runat="server" AutoPostBack="True" OnTextChanged="txtRateUSD_OnTextChanged" Text="0" CssClass="form-control" placeholder="Value" value="0" />

                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="control-label full-wdth">Total Value(USD):</label>

                                                        <asp:TextBox ID="txtTotalUSD" AutoPostBack="True" runat="server" Text="0" CssClass="form-control" placeholder="Value" />
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtTotalUSD">
                                                        </asp:FilteredTextBoxExtender>
                                                    </div>
                                                </div>
                                            </asp:Panel>

                                            <asp:Panel runat="server" ID="PanelBDT" Visible="true">
                                                <div class="col-md-4">
                                                    <div class="control-label">
                                                        <label class="control-label full-wdth">Payment Account Head</label>

                                                        <asp:DropDownList ID="DropDownList1" runat="server" DataValueField="AccountsHeadID" DataTextField="AccountsHeadName" DataSourceID="SqlDataSource7" AutoPostBack="True" CssClass="form-control select2me" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                            SelectCommand="SELECT ControlAccount.ControlAccountsName, HeadSetup.AccountsHeadName, HeadSetup.AccountsHeadID FROM    ControlAccount INNER JOIN    HeadSetup ON ControlAccount.AccountsID = HeadSetup.AccountsID AND ControlAccount.ControlAccountsID = HeadSetup.ControlAccountsID  WHERE  (ControlAccount.ControlAccountsName = 'Cash & Cash equivalent')"></asp:SqlDataSource>
                                                        <asp:DropDownList ID="DropDownList2" runat="server" Visible="false" DataValueField="ACID" DataTextField="Bank" DataSourceID="SqlDataSource11" AutoPostBack="True" CssClass="form-control select2me"></asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                            SelectCommand="SELECT ACID, (Select [BankName] FROM [Banks] where [BankId]=a.BankID) +' - '+ACNo +' - '+ACName AS Bank from BankAccounts a ORDER BY [ACName]"></asp:SqlDataSource>
                                                    </div>
                                                    </div>

                                                <div class="col-md-4">
                                                    
                                                        <div class="control-label">
                                                            <label class="control-label">Depreciation Head</label>
                                                            <asp:DropDownList ID="DropDownList4" runat="server" DataValueField="AccountsHeadID" DataTextField="AccountsHeadName" AutoPostBack="True" CssClass="form-control select2me" DataSourceID="SqlDataSource10"></asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSource10" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                                SelectCommand="SELECT        ControlAccount.ControlAccountsName, HeadSetup.AccountsHeadID, HeadSetup.AccountsHeadName
                                                        FROM ControlAccount INNER JOIN 
                                                                                 HeadSetup ON ControlAccount.ControlAccountsID = HeadSetup.ControlAccountsID
                                                        WHERE (ControlAccount.ControlAccountsName = 'Manufacturing expenses')"></asp:SqlDataSource>
                                                        </div>
                                                    
                                                </div>

                                            </asp:Panel>

                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">Unit Purchase Price (BDT)</label>
                                                    <asp:TextBox ID="txtRateBDT" runat="server" AutoPostBack="True" Text="0" OnTextChanged="txtRateBDT_OnTextChanged" value="0" />
                                                    <asp:Literal ID="Literal4" runat="server" Visible="False" />

                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtRateBDT">
                                                    </asp:FilteredTextBoxExtender>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="control-label full-wdth">Total Cost Amount (BDT):</label>

                                                    <asp:TextBox ID="txtAmountBDT" AutoPostBack="True" ReadOnly="True" runat="server" CssClass="form-control" placeholder="Asset cost(BDT)" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="control-label full-wdth">Current Value (BDT) :<asp:Literal runat="server" ID="ltrDepRate"></asp:Literal>
                                                    </label>

                                                    <asp:TextBox ID="txtCurrentValue" ReadOnly="True" runat="server" Text="0" CssClass="form-control" placeholder="Current Rate" Value="0" />
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtCurrentValue">
                                                    </asp:FilteredTextBoxExtender>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <div style="margin-top: 29px">
                                                        <asp:Button ID="btnAdd" runat="server" Text="Add" Width="100px" OnClick="btnAdd_Click" />

                                                    </div>
                                                </div>
                                            </div>


                                        </asp:Panel>

                                    </div>
                                </div>

                                <%--   Total Adjustment--%>
                                <asp:Literal ID="ltrQty" runat="server"></asp:Literal>

                            </div>
                        </div>

                    </div>
                </div>
            </div>





            <div class="row">
                <div class="col-md-12 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Fixed Assets Register Items
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="table-responsive">

                                <asp:GridView ID="ItemGrid2" CssClass="table table-hover" BorderStyle="Solid"
                                    BorderWidth="1px"
                                    CellPadding="7"
                                    ForeColor="Black"
                                    GridLines="Vertical" AllowPaging="True"
                                    PageSize="25" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" SelectedRowStyle-BackColor="LightBlue"
                                    OnRowDataBound="ItemGrid_RowDataBound" OnPageIndexChanging="ItemGrid2_OnPageIndexChanging" OnRowDeleting="ItemGrid_RowDeleting" OnSelectedIndexChanged="ItemGrid_SelectedIndexChanged" Width="1038px" RowStyle-CssClass="odd gradeX" ShowFooter="True">

                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                                <asp:Label ID="lblEntryId" runat="server" CssClass="hidden" Text='<%# Bind("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="InDate" HeaderText="Date of Purchase" />
                                        <asp:BoundField DataField="ItemCode" HeaderText="Item Code" />
                                        <asp:BoundField DataField="ProductID" HeaderText="Item Name" />
                                        <asp:BoundField DataField="ProductName" HeaderText="Item Details" />
                                        <%--<asp:BoundField DataField="BrandID" HeaderText="Model"/>
                                        <asp:BoundField DataField="CompanyFor" HeaderText="Warranty"/>--%>

                                        <%--<asp:BoundField ItemStyle-Width="150px" DataField="ProductName" HeaderText="Product Name" ReadOnly="true">
                                            </asp:BoundField>--%>

                                        <%--<asp:TemplateField HeaderText="Current FixedAssets Qty." Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbliQtyCurrent" runat="server" Text='<%# Bind("DeliveredQty") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>


                                        <asp:BoundField DataField="PurchaseCost" HeaderText="Asset cost(BDT)" DataFormatString="{0:N}" />
                                        <%--<asp:BoundField DataField="CurrentValue" HeaderText="Current Rate" />--%>

                                        <%--  <asp:TemplateField HeaderText="Unit Price">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnitWeight" runat="server" Text='<%# Bind("UnitCost") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="Qty.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbliQty" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit Voucher" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Cancel Voucher" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">Selected Item will be Deleted!</b><br />
                                                    Are you sure you want to remove the item from Fixed Assets list?
                                                            <br />
                                                    <br />
                                                    <div style="text-align: right;">
                                                        <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                        <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                    </div>
                                                </asp:Panel>

                                            </ItemTemplate>
                                            <ItemStyle Width="60px" />
                                        </asp:TemplateField>
                                        <%--<asp:CommandField ButtonType="Image" ShowSelectButton="True" ShowDeleteButton="True" DeleteImageUrl="~/app/images/delete.png" SelectImageUrl="~/app/images/edit.png" />--%>
                                    </Columns>

                                    <SelectedRowStyle BackColor="LightBlue" />

                                </asp:GridView>

                            </div>


                        </div>
                    </div>
                </div>
            </div>




        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>


