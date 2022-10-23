<%@ Page Title="Finished Products" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Finished-Items.aspx.cs" Inherits="app_Finished_Items" %>

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

        #ctl00_BodyContent_txtAmount, #ctl00_BodyContent_txtRate, #ctl00_BodyContent_txtQty, #ctl00_BodyContent_txtStock {
            text-align: center;
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

   <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate> --%>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>


            <div class="row">

                <div class="col-md-4">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box green ">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Finished Products Name
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal" role="form">
                                <div class="form-body">
                                    <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:LoginName ID="LoginName1" runat="server" Visible="false" />
                                    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>

                                    <%--<div class="col-md-4">
                                <div class="row">--%>

                                    <asp:Panel ID="pnlAdd" runat="server" DefaultButton="btnAdd">

                                        <div class="form-group">
                                            <label class="control-label">Pack Size </label>

                                            <asp:DropDownList ID="ddSize" runat="server" CssClass="form-control select2me"  AutoPostBack="True" OnSelectedIndexChanged="ddBrand_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                            <%--<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] WHERE ([ProjectID] = @ProjectID) order by DisplaySl">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>--%>

                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-12 control-label">Company</label>
                                            <asp:DropDownList ID="ddCustomer" runat="server" 
                                                AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged" CssClass="form-control select2me">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-12 control-label">Brand </label>
                                            <asp:DropDownList ID="ddBrand" runat="server" 
                                                AutoPostBack="True" OnSelectedIndexChanged="ddBrand_OnSelectedIndexChanged" CssClass="form-control select2me">
                                            </asp:DropDownList>
                                            
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-12 control-label">Subgroup </label>
                                            <asp:DropDownList ID="ddSubGroup" runat="server" 
                                                AutoPostBack="True" OnSelectedIndexChanged="ddSubGroup_OnSelectedIndexChanged" CssClass="form-control select2me">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-12 control-label">Grade </label>
                                            <asp:DropDownList ID="ddGrade" runat="server" 
                                                AutoPostBack="true" OnSelectedIndexChanged="ddGrade_SelectedIndexChanged" CssClass="form-control select2me">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-12 control-label">Category </label>
                                            <asp:DropDownList ID="ddCategory" runat="server"  CssClass="form-control select2me"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group">
                                            <label class="control-label">Product Type</label>
                                            <asp:DropDownList ID="ddProduct" runat="server"  CssClass="form-control select2me"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddProduct_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group">
                                            <label class="control-label">Product Code</label>
                                            <asp:TextBox ID="txtPrdCode" runat="server" CssClass="form-control" />
                                        </div>

                                        <div class="form-group">
                                            <label class="control-label">
                                                Finished Product Name
                                                <asp:Literal ID="ltrUnit" runat="server" Visible="False" /></label>
                                            <asp:TextBox ID="txtPrdName" runat="server" CssClass="form-control" />
                                        </div>
                                        
                                        <div class="form-group">
                                            <label class="col-sm-12 control-label">A/c Head Container </label>
                                            <asp:DropDownList ID="ddContainer" runat="server" CssClass="form-control select2me">
                                                <%--<asp:ListItem Value="0">---Select---</asp:ListItem>--%>
                                            </asp:DropDownList>
                                            <%--<asp:SqlDataSource ID="SqlDataSource22" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="Select AccountsHeadID, (Select ControlAccountsName from ControlAccount where ControlAccountsID=[HeadSetup].ControlAccountsID)+' > '+AccountsHeadName as AccountsHeadName from [HeadSetup]  Order by AccountsHeadID">
                                            </asp:SqlDataSource>--%>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-12 control-label">A/c Head Lid </label>
                                            <asp:DropDownList ID="ddLid" runat="server" CssClass="form-control select2me">
                                                <%--<asp:ListItem Value="0">---Select---</asp:ListItem>--%>
                                            </asp:DropDownList>
                                            <%--<asp:SqlDataSource ID="SqlDataSource23" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="Select AccountsHeadID, (Select ControlAccountsName from ControlAccount where ControlAccountsID=[HeadSetup].ControlAccountsID)+' > '+AccountsHeadName as AccountsHeadName from [HeadSetup]  Order by AccountsHeadID">
                                            </asp:SqlDataSource>--%>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-12 control-label">A/c Head Handle </label>
                                            <asp:DropDownList ID="ddHandle" runat="server" CssClass="form-control select2me">
                                                <%--<asp:ListItem Value="0">---Select---</asp:ListItem>--%>
                                            </asp:DropDownList>
                                            <%--<asp:SqlDataSource ID="SqlDataSource24" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="Select AccountsHeadID, (Select ControlAccountsName from ControlAccount where ControlAccountsID=[HeadSetup].ControlAccountsID)+' > '+AccountsHeadName as AccountsHeadName from [HeadSetup]  Order by AccountsHeadID">
                                            </asp:SqlDataSource>--%>
                                        </div>
                                        
                                        <div class="form-group">
                                            <label class="col-sm-12 control-label">A/c Head Sticker </label>
                                            <asp:DropDownList ID="ddAcHeadSticker" runat="server" CssClass="form-control select2me">
                                                <%--<asp:ListItem Value="0">---Select---</asp:ListItem>--%>
                                            </asp:DropDownList>
                                            <%--<asp:SqlDataSource ID="SqlDataSource25" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="Select AccountsHeadID, (Select ControlAccountsName from ControlAccount where ControlAccountsID=[HeadSetup].ControlAccountsID)+' > '+AccountsHeadName as AccountsHeadName from [HeadSetup]  Order by AccountsHeadID">
                                            </asp:SqlDataSource>--%>
                                        </div>

                                        <div class="form-group">
                                            <label class="control-label">Unit Price (tk.)</label>
                                            <asp:TextBox ID="txtUnitPrice" runat="server" onkeyup="calTotal()" CssClass="form-control" />
                                            <asp:Literal ID="ltrCQty" runat="server" Visible="False" />

                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtUnitPrice">
                                            </asp:FilteredTextBoxExtender>
                                        </div>

                                        <div class="form-group">
                                            <label class="control-label">Unit Weight (gm.)</label>
                                            <asp:TextBox ID="txtWeight" runat="server" CssClass="form-control" />
                                            <asp:Literal ID="Literal4" runat="server" Visible="False" />

                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtWeight">
                                            </asp:FilteredTextBoxExtender>
                                        </div>

                                        <div class="control-group">
                                            <div style="margin-top: 29px">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" OnClick="btnSave_Click" />

                                            </div>
                                        </div>

                                    </asp:Panel>


                                </div>
                            </div>



                        </div>
                    </div>
                </div>

                <div class="col-md-8 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box green ">
                        <%--<div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Per Unit Raw material consumption (Bill of Materials)
                    </div>

                </div>--%>
                        <div class="portlet-body form">
                            <div class="form-horizontal" role="form">
                                <div class="form-body">
                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnAdd">

                                        <asp:Label runat="server" ID="lblMsg" EnableViewState="False"></asp:Label>

                                        <legend>Associate accounts head for auto voucher entry</legend>
                                        <div class="form-group">
                                            <label class="control-label">Raw material consumed (dr.)</label>
                                            <asp:DropDownList ID="ddRMCHead" runat="server"
                                                CssClass="select2me" Width="70%">
                                            </asp:DropDownList>
                                            <%--<asp:SqlDataSource ID="SqlDataSource21" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT AccountsHeadID, AccountsHeadName FROM [HeadSetup]  WHERE IsActive='1' AND ControlAccountsID  ='040201' ORDER BY [AccountsHeadID] DESC"></asp:SqlDataSource>--%>
                                        </div>

                                        <div class="form-group">
                                            <label class="control-label">Inventories (cr.)</label>
                                            <asp:DropDownList ID="ddInventoriesHead" runat="server" CssClass="select2me" Width="70%">
                                            </asp:DropDownList>
                                            <%--<asp:SqlDataSource ID="SqlDataSource20" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT AccountsHeadID, AccountsHeadName FROM [HeadSetup] WHERE IsActive='1' AND ControlAccountsID='010106' ORDER BY [AccountsHeadID] DESC"></asp:SqlDataSource>--%>
                                        </div>


                                        <legend>Per unit raw material consumption (Bill of Materials)</legend>
                                        <div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label>Purpose :</label>
                                                <asp:DropDownList ID="ddPurposeRaw" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddPurpose_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <%--<asp:SqlDataSource ID="SqlDataSource13" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [pid], [Purpose] FROM [Purpose] order by Purpose"></asp:SqlDataSource>--%>
                                            </div>
                                        </div>

                                        <div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Group</label>
                                                <asp:DropDownList ID="ddGroup" runat="server"  AutoPostBack="true"
                                                     OnSelectedIndexChanged="ddGroup_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <%--<asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] WHERE GroupSrNo<>2 ORDER BY [GroupSrNo]"></asp:SqlDataSource>--%>

                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Subgroup </label>

                                                <asp:DropDownList ID="ddSubGroupRaw" runat="server" 
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddSubGrpRaw_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <%--<asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT CategoryID, CategoryName FROM [ItemSubGroup] where (GroupID = @GroupID) ">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddGroup" Name="GroupID" PropertyName="SelectedValue" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>--%>
                                            </div>
                                        </div>


                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Grade </label>

                                                <asp:DropDownList ID="ddGradeRaw" runat="server" 
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddGradeRaw_OnSelectedIndexChanged">
                                                </asp:DropDownList>

                                                <%--<asp:SqlDataSource ID="SqlDataSource10" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT GradeID,GradeName from ItemGrade where CategoryID = @CategoryID ORDER BY [GradeName]">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddSubGroupRaw" Name="CategoryID" PropertyName="SelectedValue" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>--%>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Category </label>

                                                <asp:DropDownList ID="ddCategoryRaw" runat="server" 
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddCategoryRaw_OnSelectedIndexChanged">
                                                </asp:DropDownList>

                                                <%--<asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT CategoryID, CategoryName FROM [Categories] where GradeID = @GradeID ORDER BY [CategoryName]">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddGradeRaw" Name="GradeID" PropertyName="SelectedValue" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>--%>
                                            </div>
                                        </div>


                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Product </label>

                                                <asp:DropDownList ID="ddProductRaw" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddProductRaw_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <%--<asp:SqlDataSource ID="SqlDataSource14" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE [CategoryID] = @CategoryID ORDER BY [ItemName]">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddCategoryRaw" Name="CategoryID" PropertyName="SelectedValue" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>--%>
                                                <%--SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE ([CategoryID] IN (Select CategoryID from Categories where GradeID IN (Select GradeID from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=2 AND ProjectID=1)))) ORDER BY [ItemName]"></asp:SqlDataSource>--%>
                                            </div>
                                        </div>


                                        <div class="col-md-4" id="ddTypeArea" runat="server">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label full-wdth">Stock Type</label>
                                                <asp:DropDownList ID="ddTypeRaw" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddType_OnSelectedIndexChanged">
                                                    <asp:ListItem>Raw Sheet</asp:ListItem>
                                                    <asp:ListItem>Processed Sheet</asp:ListItem>
                                                    <asp:ListItem>Printed Sheet</asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                        </div>


                                        <asp:Panel ID="pnlExtra" runat="server" Visible="False">

                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label">Company:</label>
                                                    <asp:DropDownList ID="ddCustomerRaw" runat="server"  AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddCustomerRaw_OnSelectedIndexChanged">

                                                        <asp:ListItem Value="">--- all ---</asp:ListItem>

                                                    </asp:DropDownList>
                                                    <%--<asp:SqlDataSource ID="SqlDataSource15" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                                        <SelectParameters>
                                                            <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>--%>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label">Brand: </label>
                                                    <asp:DropDownList ID="ddBrandRaw" runat="server"  AppendDataBoundItems="True"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddBrand_OnSelectedIndexChanged">
                                                        <asp:ListItem Value="">--- all ---</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--<asp:SqlDataSource ID="SqlDataSource16" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT [BrandID], [BrandName] FROM [CustomerBrands] WHERE (([CustomerID] = @CustomerID) AND ([ProjectID] = @ProjectID)) Order by BrandName">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="ddCustomerRaw" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>--%>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">Pack-Size</label>
                                                    <asp:DropDownList ID="ddSizeRaw" runat="server" 
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddSize_OnSelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <%--<asp:SqlDataSource ID="SqlDataSource17" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] WHERE ([ProjectID] = @ProjectID) order by DisplaySl">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>--%>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="control-label">Color :  </label>
                                                    <asp:DropDownList ID="ddColorRaw" runat="server" 
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddColor_OnSelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <%--<asp:SqlDataSource ID="SqlDataSource18" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT [DepartmentName], [Departmentid] FROM [Colors] ORDER BY [DepartmentName]"></asp:SqlDataSource>--%>
                                                </div>
                                            </div>

                                        </asp:Panel>



                                        <asp:Panel ID="pnlSpec" runat="server">

                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">
                                                        Specification:
                                                        <asp:LinkButton ID="lbSpec" runat="server" OnClick="lbSpec_OnClick">New</asp:LinkButton>
                                                        |
                                                        <asp:LinkButton ID="lbFilter" runat="server" OnClick="lbFilter_OnClick">Show-all</asp:LinkButton>
                                                    </label>
                                                    <asp:DropDownList ID="ddSpecRaw" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddSpec_OnSelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <%--<asp:SqlDataSource ID="SqlDataSource19" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT [id], [spec] FROM [Specifications] ORDER BY [spec]"></asp:SqlDataSource>--%>

                                                    <asp:TextBox ID="txtSpec" runat="server" Visible="False" />
                                                </div>
                                            </div>
                                        </asp:Panel>

                                        <%--<div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label full-wdth">
                                                    Current Stock Qty.(<asp:Literal ID="Literal1" runat="server" Visible="False" />pcs)</label>
                                                <asp:TextBox ID="txtCurrentQty" runat="server" ReadOnly="True" />
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label full-wdth">
                                                    Current Stock Weight
                                                    (<asp:Literal ID="Literal5" runat="server" Visible="False" />kg)</label>
                                                <asp:TextBox ID="txtCurrentKg" runat="server" ReadOnly="True" />
                                            </div>
                                        </div>--%>


                                        <div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label full-wdth">
                                                    Consumption Qty.(<asp:Literal ID="Literal2" runat="server" Visible="False" />pcs)</label>
                                                <asp:TextBox ID="txtQtyRaw" runat="server" onkeyup="calTotal()" />
                                                <asp:Literal ID="Literal3" runat="server" Visible="False" />

                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789" TargetControlID="txtQtyRaw">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label full-wdth">
                                                    Consumption (<asp:Literal ID="Literal6" runat="server" Visible="False" />%)</label>
                                                <asp:TextBox ID="txtWeightRaw" runat="server" />
                                                <asp:Literal ID="Literal7" runat="server" Visible="False" />

                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtWeightRaw">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>

                                        <%--<div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label full-wdth">Unit Price</label>
                                                <asp:TextBox ID="txtPrice" runat="server" />
                                                <asp:Literal ID="Literal8" runat="server" Visible="False" />
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtPrice">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>--%>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <%--<div style="margin-top: 29px">--%>
                                                    <asp:Button ID="btnAdd" runat="server" Text="Add" Width="100px" OnClick="btnAdd_Click" />

                                                    <%--Total Consumption:--%>
                                                    <asp:Literal ID="ltrQty" runat="server"></asp:Literal>
                                                <%--</div>--%>
                                            </div>
                                        </div>

                                    </asp:Panel>



                                </div>
                            </div>





                        </div>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="ItemGrid" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" SelectedRowStyle-BackColor="LightBlue"
                            OnRowDataBound="ItemGrid_RowDataBound" OnRowDeleting="ItemGrid_RowDeleting" OnSelectedIndexChanged="ItemGrid_SelectedIndexChanged">

                            <Columns>
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                        <asp:Label ID="lblEntryId" runat="server" CssClass="hidden" Text='<%# Bind("Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="RMCHead" HeaderText="RMC Head" ReadOnly="true"></asp:BoundField>
                                <asp:BoundField DataField="InventoryHead" HeaderText="Inventory Head" ReadOnly="true"></asp:BoundField>

                                <asp:BoundField DataField="Purpose" HeaderText="Purpose" ReadOnly="true"></asp:BoundField>
                                <asp:BoundField DataField="ProductName" HeaderText="Product Name" ReadOnly="true"></asp:BoundField>

                                <asp:TemplateField HeaderText="Current Stock Qty." Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lbliQtyCurrent" runat="server" Text='<%# Bind("DeliveredQty") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:BoundField DataField="Customer" HeaderText="Customer" />
                                <asp:BoundField DataField="BrandID" HeaderText="Brand" />
                                <asp:BoundField DataField="SizeId" HeaderText="Pack Size" />
                                <asp:BoundField DataField="Color" HeaderText="Color" />--%>
                                <asp:BoundField DataField="Spec" HeaderText="Spec" ReadOnly="true"></asp:BoundField>

                                <asp:TemplateField HeaderText="Cons. Qty.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbliQty" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Cons. %">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnitWeight" runat="server" Text='<%# Bind("UnitWeight") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Qty. Added" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyBalance" runat="server" Text='<%# Bind("QtyBalance") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>

                                <asp:BoundField ItemStyle-Width="100px" DataField="UnitType" HeaderText="Unit" ReadOnly="true" Visible="False">
                                    <ItemStyle Width="100px"></ItemStyle>
                                </asp:BoundField>

                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <%--<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Select" />--%>
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
                                    <ItemStyle Width="40px" />
                                </asp:TemplateField>


                            </Columns>

                        </asp:GridView>

                    </div>

                </div>

            </div>




            <div class="row">
                <div class="col-md-12 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>List of Finished Products Name
                                <asp:LinkButton ID="lbRefresh" runat="server" OnClick="lbRefresh_OnClick">Refresh</asp:LinkButton>
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="table-responsive">

                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectedRowStyle-BackColor="LightBlue" DataKeyNames="pid" 
                                     OnRowDeleting="GridView1_OnRowDeleting" >

                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                        <asp:Label ID="lblDeleteId" runat="server" CssClass="hidden" Text='<%# Bind("pid") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                        <asp:BoundField DataField="pid" HeaderText="pid" SortExpression="pid" InsertVisible="False" ReadOnly="True"></asp:BoundField>
                                        <asp:BoundField DataField="ProductCode" HeaderText="ProductCode" SortExpression="ProductCode" />
                                        <asp:BoundField DataField="ProductName" HeaderText="ProductName" SortExpression="ProductName" />
                                        <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" SortExpression="UnitPrice" />
                                        <asp:BoundField DataField="UnitWeight" HeaderText="UnitWeight" SortExpression="UnitWeight" />

                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <%--<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Select" />--%>
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
                                    <ItemStyle Width="40px" />
                                </asp:TemplateField>

                                    </Columns>
                                    <SelectedRowStyle BackColor="LightBlue" />

                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [pid], [ProductCode], [ProductName], [UnitPrice], [UnitWeight] FROM [FinishedProducts] WHERE ([CompanyID] = @CompanyID)"
                                    DeleteCommand="Delete FinishedProducts where pid='0'" >
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddCustomer" Name="CompanyID" PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>


                        </div>
                    </div>
                </div>
            </div>

            <%--<a href="Stock-Adj-Finished-Direct.aspx">Set Direct</a>--%>
        </ContentTemplate>
    </asp:UpdatePanel>




</asp:Content>


