<%@ Page Title="Fixed Asset Position" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="FixedAssetPosition.aspx.cs" Inherits="app_FixedAssetPosition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.10/js/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.10/css/jquery.dataTables.min.css">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".tbl_default").prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]] //value:item pair
            });
        });

        $(window).load(function () {
            //jScript();
        });

        function jScript() {
            $(".tbl_default").prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]] //value:item pair
            });
        }

    </script>


    <style type="text/css">
        .col-md-4 .control-group input, .col-md-4 .control-group select {
            width: 100%;
        }

        .col-md-4 .control-group label {
            padding-bottom: 4px;
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
    </style>

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
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>--%>

    <div class="row">


        <div class="col-md-12 ">
            <!-- BEGIN SAMPLE FORM PORTLET-->
            <div class="portlet box green ">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i> Fixed Assets Position
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

                                    <asp:Panel ID="pnlAdd" runat="server" DefaultButton="btnSearch">
                                        <%--<div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">Godown : </label>
                                                <asp:DropDownList ID="ddGodown" runat="server" DataSourceID="SqlDataSource6"
                                                    DataTextField="StoreName" DataValueField="WID" CssClass="form-control" AppendDataBoundItems="True"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddGodown_SelectedIndexChanged">
                                                    <asp:ListItem>--- all ---</asp:ListItem>

                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource6" runat="server"
                                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [WID], [StoreName] FROM [Warehouses]"></asp:SqlDataSource>
                                            </div>
                                        </div>--%>

                                        <%--<div class="col-md-4">
                                            <div class="control-group">
                                                <label>Purpose :</label>
                                                <asp:DropDownList ID="ddPurpose" runat="server" DataSourceID="SqlDataSource13" AppendDataBoundItems="True"
                                                    DataTextField="Purpose" DataValueField="pid" AutoPostBack="True" OnSelectedIndexChanged="ddPurpose_OnSelectedIndexChanged">

                                                    <asp:ListItem>--- all ---</asp:ListItem>

                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource13" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [pid], [Purpose] FROM [Purpose] order by Purpose"></asp:SqlDataSource>
                                            </div>
                                        </div>--%>

                                        <%--<div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Group</label>
                                                <asp:DropDownList ID="ddGroup" runat="server" DataSourceID="SqlDataSource1" AutoPostBack="true"
                                                    DataTextField="GroupName" DataValueField="GroupSrNo" >
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup]  WHERE GroupSrNo>3 AND GroupSrNo<>4 AND GroupSrNo<>5 AND GroupSrNo<>7 AND GroupSrNo<>'11' ORDER BY [GroupSrNo]"></asp:SqlDataSource>

                                            </div>
                                        </div>--%>

                                        <%--<div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Subgroup </label>

                                                <asp:DropDownList ID="ddSubGroup" runat="server" DataSourceID="SqlDataSource3" DataTextField="CategoryName" DataValueField="CategoryID"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddSubGroup_OnSelectedIndexChanged" AppendDataBoundItems="True">--%>

                                                    <%--<asp:ListItem>--- all ---</asp:ListItem>--%>
                                                <%--</asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT CategoryID, CategoryName FROM [ItemSubGroup] where (GroupID = @GroupID) ">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddGroup" Name="GroupID" PropertyName="SelectedValue" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="col-md-4" id="tinPlate" runat="server">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">Fixed Assets Type</label>
                                                <asp:DropDownList ID="DropDownList1" runat="server"  Width="100%" Height="30%" DataSourceID="SqlDataSource2" DataTextField="GroupName" DataValueField="GroupName" AutoPostBack="True" ></asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] WHERE GroupSrNo>3 AND GroupSrNo<>5 AND GroupSrNo<>7 AND GroupSrNo<>4 AND GroupSrNo<>'11' ORDER BY [GroupSrNo]"></asp:SqlDataSource>
                                                </div>
                                            </div>--%>
                                        
                                        <%-- New added Code --%>
                                        <div class="col-md-4 hidden">
                                                <div class="control-group">
                                                <label class="control-label full-wdth">Entry Date : </label>
                                                
                                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                                        Enabled="True" TargetControlID="TextBox1">
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
                                                    <label class="col-sm-12 control-label">Item </label>

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
                                            <div class="col-md-4 hidden">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">Date of Purchase</label>
                                                    <asp:TextBox ID="txtPurchaseDate" runat="server" />
                                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                        Enabled="True" TargetControlID="txtPurchaseDate">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                            <div class="col-md-4 hidden">
                                                <div class="control-group" style="margin-top: 26px;" >
                                                    <%--<label class="col-sm-12 control-label full-wdth">Item Code :</label>--%>
                                                    <asp:CheckBox ID="cbAuto" runat="server" Text="Auto Code" Checked="True" AutoPostBack="True"  />
                                                    <asp:TextBox ID="txtItemCode"  ReadOnly="True" runat="server"  Width="59%" />
                                                    
                                                </div>
                                            </div>

                                            <div class="col-md-4 hidden">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">Item Details</label>
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


                                            <div class="col-md-4 hidden">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">
                                                        Adjustment Qty.(<asp:Literal ID="ltrUnit" runat="server" Visible="False" />pcs)</label>
                                                    <asp:TextBox ID="txtQty" runat="server" onkeyup="calTotal()" />
                                                    <asp:Literal ID="ltrCQty" runat="server" Visible="False" />

                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtQty">
                                                    </asp:FilteredTextBoxExtender>
                                                </div>
                                            </div>

                                            <div class="col-md-4 hidden">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">Unit Price</label>
                                                    <asp:TextBox ID="txtPrice" runat="server" Text="0" />
                                                    <asp:Literal ID="Literal4" runat="server" Visible="False" />

                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtPrice">
                                                    </asp:FilteredTextBoxExtender>
                                                </div>
                                            </div>
                                             
                                            <div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">Adjustment Remark :</label>
                                                
                                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Adjustment Remark" />
                                                </div>
                                            </div>
                                              <div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">Purchase Rate :</label>
                                                
                                                    <asp:TextBox ID="txtPurchaseCost" runat="server" CssClass="form-control" placeholder="Purchase Rate" />
                                                </div>
                                            </div>
                                              <div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">Current Rate :</label>
                                                
                                                    <asp:TextBox ID="txtCurrentValue" runat="server" CssClass="form-control" placeholder="Current Rate" />
                                                </div>
                                            </div>
                                             <div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">Rate :</label>
                                                
                                                    <asp:TextBox ID="txtRate" runat="server" Text="0" CssClass="form-control" placeholder="Rate" />
                                                </div>
                                            </div>
                                        <%-- New added Code --%>
                                        <div class="col-md-4" runat="server">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">As on Date:</label>
                                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                    Enabled="True" TargetControlID="txtDateFrom">
                                                </asp:CalendarExtender>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <div style="margin-top: 29px">
                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" Width="100px" OnClick="btnAdd_Click" />

                                                    <asp:Button ID="btnExport" runat="server" Width="120px" Text="Export to Excel" OnClick="btnExport_OnClick" />
                                                </div>
                                            </div>
                                        </div>

                                    </asp:Panel>
                                    
                                     <iframe id="if1" runat="server" height="800px" width="100%" ></iframe>

                                </div>
                            </div>


                        </div>
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
                        <i class="fa fa-reorder"></i>Current Stock
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
                        <asp:Literal ID="ltrtotal" runat="server"></asp:Literal>

                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="True" SelectedRowStyle-BackColor="LightBlue" Visible="False"
                            OnRowDataBound="ItemGrid_RowDataBound" CssClass="tbl_default zebra table">

                            <Columns>

                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>. 
                                    </ItemTemplate>
                                    <ItemStyle Width="20px" />
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>

                    </div>

                </div>
            </div>
        </div>
    </div>

    <%--<a href="javascript:window.print()"><img src="../images/print.png" alt="print report" id="print-button" width="40px"></a>--%>


   <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>




</asp:Content>