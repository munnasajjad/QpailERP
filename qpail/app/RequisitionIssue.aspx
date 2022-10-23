<%@ Page Title="Requisition Issue Entry" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="RequisitionIssue.aspx.cs" Inherits="app_RequisitionIssue" %>

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


            <div class="row">
                <div class="col-md-4 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Stock In-Out (Issue & Receive)
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                            <div class="form-horizontal" role="form">

                                <div class="form-body">

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Requistion Date : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>

                                    <%--                            <div class="control-group">
                                <label class="control-label full-wdth">Issue/ Receive :</label>
                                <div class="controls">
                                    <%--<asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="" />
                                <asp:DropDownList ID="ddRemark" runat="server"  CssClass="form-control">
                                    <asp:ListItem>ISSUE</asp:ListItem>
                                    <asp:ListItem>RECEIVE</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>--%>
                                    <div class="control-group">
                                        <label class="control-label full-wdth">From (Stock-out Godown) : </label>
                                        <asp:DropDownList ID="ddFromGodown" runat="server" DataSourceID="SqlDataSource6"
                                            DataTextField="StoreName" DataValueField="WID" CssClass="form-control"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddFromGodown_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [WID], [StoreName] FROM [Warehouses]"></asp:SqlDataSource>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">From Location : </label>
                                        <asp:DropDownList ID="ddFromLocation" runat="server" DataSourceID="SqlDataSource16"
                                            DataTextField="AreaName" DataValueField="AreaID" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddFromLocation_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource16" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT AreaID,[AreaName] FROM [WareHouseAreas] where Warehouse=@Warehouse">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddFromGodown" Name="Warehouse" PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">To (Stock-in Godown) : </label>
                                        <asp:DropDownList ID="ddGodown" runat="server" DataSourceID="SqlDataSource6"
                                            DataTextField="StoreName" DataValueField="WID" CssClass="form-control"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddGodown_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [WID], [StoreName] FROM [Warehouses]"></asp:SqlDataSource>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Received By/ Location : </label>
                                        <asp:DropDownList ID="ddLocation" runat="server" DataSourceID="SqlDataSource15"
                                            DataTextField="AreaName" DataValueField="AreaID" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource15" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT AreaID,[AreaName] FROM [WareHouseAreas] where Warehouse=@Warehouse">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddGodown" Name="Warehouse" PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Remarks : </label>
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>

                                    </div>

                                    <%--<div style="height:120px;">&nbsp;</div>--%>
                                </div>
                            </div>

                            <div class="form-actions">
                                <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click" />
                                <%--<asp:Button ID="btnEdit" CssClass="btn red" runat="server" Text="Edit" />
                        <asp:Button ID="btnPrint" CssClass="btn purple" runat="server" Text="Print" />--%>
                                <asp:Button ID="btnCancel" CssClass="btn default" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                            </div>

                        </div>
                    </div>
                </div>





                <div class="col-md-8 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box green ">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Item Details
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

                                    <div class="col-md-12">
                                        <div class="row">

                                            <asp:Panel ID="pnlAdd" runat="server" DefaultButton="btnAdd">

                                                <%--<div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label full-wdth">Pack Size </label>

                                                <asp:DropDownList ID="ddSize" runat="server" DataSourceID="SqlDataSource2" DataTextField="BrandName" DataValueField="BrandID">
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] WHERE ([ProjectID] = @ProjectID) order by DisplaySl">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                            </div>
                                        </div>--%>


                                                <div class="col-md-4 hidden">
                                                    <div class="control-group" >
                                                        <label class="col-md-12 control-label">Purpose :</label>
                                                        <asp:DropDownList ID="ddPurpose" runat="server" DataSourceID="SqlDataSource13"
                                                            DataTextField="Purpose" DataValueField="pid" AutoPostBack="True" OnSelectedIndexChanged="ddPurpose_OnSelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource13" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT [pid], [Purpose] FROM [Purpose] ORDER BY Purpose ASC"></asp:SqlDataSource>
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="col-md-12 control-label">Group</label>
                                                        <asp:DropDownList ID="ddGroup" runat="server" DataSourceID="SqlDataSource1" AutoPostBack="true"
                                                            DataTextField="GroupName" DataValueField="GroupSrNo" OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] WHERE GroupSrNo<>2 AND GroupSrNo<>3 AND GroupSrNo <>'11' ORDER BY [GroupSrNo]"></asp:SqlDataSource>

                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label">Subgroup </label>

                                                        <asp:DropDownList ID="ddSubGroup" runat="server" DataSourceID="SqlDataSource3" DataTextField="CategoryName" DataValueField="CategoryID"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddSubGroup_OnSelectedIndexChanged">
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
                                                        <label class="col-sm-12 control-label">Product </label>

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

                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="control-label full-wdth">Description : </label>
                                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="col-md-4" id="ddTypeArea" runat="server">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label full-wdth">Stock Type</label>
                                                        <asp:DropDownList ID="ddType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddType_OnSelectedIndexChanged">
                                                            <asp:ListItem>Raw Sheet</asp:ListItem>
                                                            <asp:ListItem>Processed Sheet</asp:ListItem>
                                                            <asp:ListItem>Printed Sheet</asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>


                                                <asp:Panel ID="pnlExtra" runat="server">

                                                    <div class="col-md-4">
                                                        <div class="control-group">
                                                            <label class="col-sm-12 control-label">Company:</label>
                                                            <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource2"
                                                                DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddCustomer_OnSelectedIndexChanged">

                                                                <asp:ListItem Value="">--- all ---</asp:ListItem>

                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                                                <SelectParameters>
                                                                    <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4">
                                                        <div class="control-group">
                                                            <label class="col-sm-12 control-label">Brand: </label>
                                                            <asp:DropDownList ID="ddBrand" runat="server" DataSourceID="SqlDataSource9" DataTextField="BrandName" DataValueField="BrandID" AppendDataBoundItems="True"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddBrand_OnSelectedIndexChanged">
                                                                <asp:ListItem Value="">--- all ---</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSource9" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                SelectCommand="SELECT [BrandID], [BrandName] FROM [CustomerBrands] WHERE (([CustomerID] = @CustomerID) AND ([ProjectID] = @ProjectID)) Order by BrandName">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                                                    <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4">
                                                        <div class="control-group">
                                                            <label class="col-sm-12 control-label full-wdth">Pack-Size</label>
                                                            <asp:DropDownList ID="ddSize" runat="server" DataSourceID="SqlDataSource10" DataTextField="BrandName" DataValueField="BrandID"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddSize_OnSelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSource10" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] WHERE ([ProjectID] = @ProjectID) order by DisplaySl">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4">
                                                        <div class="control-group">
                                                            <label class="control-label">Color :  </label>
                                                            <asp:DropDownList ID="ddColor" runat="server" DataSourceID="SqlDataSource11" DataTextField="DepartmentName" DataValueField="Departmentid"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddColor_OnSelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                SelectCommand="SELECT [DepartmentName], [Departmentid] FROM [Colors] ORDER BY [DepartmentName]"></asp:SqlDataSource>
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
                                                            <asp:DropDownList ID="ddSpec" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddSpec_OnSelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSource14" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                SelectCommand="SELECT [id], [spec] FROM [Specifications] ORDER BY [spec]"></asp:SqlDataSource>

                                                            <asp:TextBox ID="txtSpec" runat="server" Visible="False" />
                                                        </div>
                                                    </div>
                                                </asp:Panel>

                                                <div class="col-md-4">
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
                                                </div>


                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label full-wdth">
                                                           Out  Qty.(<asp:Literal ID="ltrUnit" runat="server" Visible="False" />pcs)</label>
                                                        <asp:TextBox ID="txtQty" runat="server" onkeyup="calTotal()" />
                                                        <asp:Literal ID="ltrCQty" runat="server" Visible="False" />

                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtQty">
                                                        </asp:FilteredTextBoxExtender>
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label full-wdth">
                                                            Out  Weight (<asp:Literal ID="Literal3" runat="server" Visible="False" />kg)</label>
                                                        <asp:TextBox ID="txtWeight" runat="server" />
                                                        <asp:Literal ID="Literal4" runat="server" Visible="False" />

                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtWeight">
                                                        </asp:FilteredTextBoxExtender>
                                                    </div>
                                                </div>

                                                <div class="col-md-4 hidden" id="pnlUnitPrice">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label full-wdth">Unit Price</label>
                                                        <asp:TextBox ID="txtPrice" runat="server" Text="0" />
                                                        <asp:Literal ID="Literal6" runat="server" Visible="False" />
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtPrice">
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

                                    Total :
                                    <asp:Literal ID="ltrQty" runat="server"></asp:Literal>

                                </div>
                            </div>


                            <div class="table-responsive">
                                <asp:GridView ID="ItemGrid" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" SelectedRowStyle-BackColor="LightBlue" Width="100%"
                                    OnRowDataBound="ItemGrid_RowDataBound" OnRowDeleting="ItemGrid_RowDeleting" OnSelectedIndexChanged="ItemGrid_SelectedIndexChanged">

                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                                <asp:Label ID="lblEntryId" runat="server" CssClass="hidden" Text='<%# Bind("Id") %>'></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" ReadOnly="true"></asp:BoundField>
                                        <asp:BoundField DataField="Manufacturer" HeaderText="Description" ReadOnly="true"></asp:BoundField>


                                        <asp:TemplateField HeaderText="Current Stock Qty." Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbliQtyCurrent" runat="server" Text='<%# Bind("DeliveredQty") %>'></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <%--<asp:BoundField DataField="Customer" HeaderText="Customer" />
                                <asp:BoundField DataField="BrandID" HeaderText="Brand" />
                                <asp:BoundField DataField="SizeId" HeaderText="Pack Size" />
                                <asp:BoundField DataField="Color" HeaderText="Color" />
                                <asp:BoundField  DataField="Spec" HeaderText="Spec" ReadOnly="true"></asp:BoundField>--%>

                                        <asp:TemplateField HeaderText="Qty.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbliQty" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Weight">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnitWeight" runat="server" Text='<%# Bind("UnitWeight") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>



                                        <asp:TemplateField HeaderText="Qty. Added" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyBalance" runat="server" Text='<%# Bind("QtyBalance") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField ItemStyle-Width="150px" DataField="UnitType" HeaderText="Unit" ReadOnly="true" Visible="False"></asp:BoundField>

                                        <asp:CommandField ButtonType="Image" ShowSelectButton="True" ShowDeleteButton="True"
                                            DeleteImageUrl="~/app/images/delete.png" SelectImageUrl="~/app/images/edit.png" />

                                    </Columns>

                                </asp:GridView>

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
                                <i class="fa fa-reorder"></i>Item Stock Register
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


                                <%--<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectedRowStyle-BackColor="LightBlue"
                            OnRowDataBound="GridView1_OnRowDataBound"  DataKeyNames="entryid"
                             DataSourceID="SqlDataSource12">--%>

                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectedRowStyle-BackColor="LightBlue"
                                    OnRowDataBound="ItemGrid_RowDataBound" OnRowDeleted="GridView1_OnRowDeleted" DataKeyNames="EntryID" DataSourceID="SqlDataSource12">


                                    <Columns>

                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                                <asp:Label ID="lblEntryId" runat="server" CssClass="hidden" Text='<%# Bind("EntryID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" SortExpression="EntryDate" DataFormatString="{0:d}"></asp:BoundField>

                                        <asp:BoundField DataField="EntryType" HeaderText="Description" SortExpression="EntryType" />
                                        <asp:BoundField DataField="Warehouse" HeaderText="Warehouse" SortExpression="InvoiceID" />
                                        <%--<asp:BoundField DataField="Purpose" HeaderText="Purpose" SortExpression="Purpose" />--%>
                                        <asp:BoundField DataField="RefNo" HeaderText="RefNo" SortExpression="RefNo" />
                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" />

                                        <%--<asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"></asp:BoundField>
                                <asp:BoundField DataField="BrandID" HeaderText="Brand" SortExpression="BrandID" />
                                <asp:BoundField DataField="SizeID" HeaderText="Size" SortExpression="SizeID" />
                                <asp:BoundField DataField="Color" HeaderText="Color" SortExpression="Color" />
                                <asp:BoundField DataField="Spec" HeaderText="Spec" SortExpression="Spec" />--%>

                                        <asp:BoundField DataField="InQuantity" HeaderText="In Qty" SortExpression="InQuantity" />
                                        <asp:BoundField DataField="OutQuantity" HeaderText="Out Qty" SortExpression="OutQuantity" />
                                        <asp:BoundField DataField="InWeight" HeaderText="In Weight" SortExpression="InWeight" />
                                        <asp:BoundField DataField="OutWeight" HeaderText="Out Weight" SortExpression="OutWeight" />
                                        <asp:BoundField DataField="price" HeaderText="Price" SortExpression="OutWeight" />
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />




                                        <%-- <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>

                                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />

                                                    <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                    </asp:ConfirmButtonExtender>
                                                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                        PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                    <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                        <b style="color: red">Item will be deleted permanently!</b><br />
                                                        Are you sure you want to delete the item from stock?
                                                            <br />
                                                        <br />
                                                        <div style="text-align: right;">
                                                            <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                            <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                        </div>
                                                    </asp:Panel>

                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                    </Columns>

                                    <SelectedRowStyle BackColor="LightBlue" />

                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT TOP(100) EntryID, [EntryDate], [EntryType],
                            (Select Purpose from Purpose where pid=stock.Purpose) AS Purpose,
                             [InvoiceID], [RefNo], [ProductName],
                                    (SELECT  [Company] FROM [Party] WHERE [PartyID]= Stock.Customer) AS Customer,
                                (SELECT [BrandName] FROM [CustomerBrands] WHERE BrandID=Stock.BrandID) AS BrandID,
                                (SELECT [BrandName] FROM [Brands] WHERE BrandID=Stock.SizeId) AS SizeId,
                                (SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=Stock.Color) AS Color,
                                (SELECT [StoreName] FROM [Warehouses] WHERE WID=Stock.WarehouseID) AS Warehouse,
                            (SELECT [spec] FROM [Specifications] WHERE  CAST(id AS nvarchar)=Stock.Spec) AS Spec,
                                    [InQuantity], [OutQuantity], [InWeight], [OutWeight], price, Remark FROM [Stock] WHERE ([ProductID] = @ProductID)
                                   ORDER BY [EntryDate] DESC, [EntryID] DESC"
                                    DeleteCommand="Delete Stock where EntryID=@EntryID">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddProduct" Name="ProductID" PropertyName="SelectedValue" Type="String" />
                                        <asp:ControlParameter ControlID="ddType" Name="ItemType" PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                    <DeleteParameters>
                                        <asp:Parameter Name="EntryID" Type="Int32" />
                                    </DeleteParameters>
                                </asp:SqlDataSource>
                            </div>


                        </div>
                    </div>
                </div>
            </div>




        </ContentTemplate>
    </asp:UpdatePanel>




</asp:Content>


