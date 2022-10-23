<%@ Page Title="Fixed Assets Out" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="FixedAssetsOut.aspx.cs" Inherits="app_RemoveFixedAssets" %>

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
                            <i class="fa fa-reorder"></i>Fixed Assets Out 
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
                                          
                                            <div class="col-md-4" hidden="true">
                                                <div class="control-group" >
                                                    <label class="col-sm-12 control-label full-wdth">Item Code</label>
                                                    <asp:TextBox ID="txtItemCode" runat="server"  />
                                                    <%--<asp:DropDownList ID="ItemCode" runat="server" DataSourceID="SqlDataSource2" DataTextField="ItemCode" DataValueField="ItemCode"></asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" SelectCommand="SELECT [ItemCode] FROM [FixedAssets]"></asp:SqlDataSource>--%>
                                                </div>
                                            </div>
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
                                                    <label class="col-sm-12 control-label full-wdth">Stock Out Date :</label>
                                                    <asp:TextBox ID="txtPurchaseDate" runat="server" />
                                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                        Enabled="True" TargetControlID="txtPurchaseDate">
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
                                                        SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] WHERE GroupSrNo>3 AND GroupSrNo<>5 AND GroupSrNo<>7 AND GroupSrNo<>4 AND GroupSrNo<>'11' ORDER BY [GroupSrNo]"></asp:SqlDataSource>

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

                                           <div class="col-md-4 hidden">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">Model No.</label>
                                                    <asp:TextBox ID="txtModel" runat="server" />
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
                                                    <label class="col-sm-12 control-label">Item :</label>

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
                                                    <label class="col-sm-12 control-label full-wdth">Available Quantity.</label>
                                                    <asp:TextBox ID="txtAQty" runat="server"  ReadOnly="True" onkeyup="calTotal()"  />
                                                    </div>
                                            </div>
                                           <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">Item Code :</label>
                                                    <%--<asp:TextBox ID="TextBox1" runat="server"  />--%>
                                                    <asp:DropDownList ID="ddItemCode" runat="server" DataSourceID="SqlDataSource2" DataTextField="ItemCode" CssClass="control-group_select2me"  DataValueField="ItemCode" ></asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" 
                                                        SelectCommand="SELECT ProductID, [ItemCode] FROM [FixedAssets] WHERE ProductID=@ProductId AND DeliveredQty=0">
                                                     <SelectParameters>
                                       <asp:ControlParameter ControlID="ddProduct" Name="ProductId" PropertyName="SelectedValue" />
                                   </SelectParameters>
                                                    </asp:SqlDataSource>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">Reason of Stock Out</label>
                                                    <asp:TextBox ID="txtSpec" runat="server" />
                                                </div>
                                            </div>
                                            <div class="col-md-4 hidden">
                                                <div class="control-group">
                                                    <label class="col-sm-12 control-label full-wdth">
                                                        Stock Out Quantity.(<asp:Literal ID="ltrUnit" runat="server" Visible="False" />pcs)</label>
                                                    <asp:TextBox ID="txtDeliverQty" runat="server"  onkeyup="calTotal()" Text="1" />
                                                    <asp:Literal ID="ltrCQty" runat="server" Visible="False" />

                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtDeliverQty">
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
                                              <div class="col-md-4"  hidden="true">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">Purchase Rate :</label>
                                                
                                                    <asp:TextBox ID="txtPurchaseCost" runat="server" CssClass="form-control" placeholder="Purchase Rate" />
                                                </div>
                                            </div>
                                              <div class="col-md-4" hidden="true">
                                            <div class="control-group" >
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
                                            <div class="col-md-4">
                                                <div class="control-group">
                                                    <div style="margin-top: 29px">
                                                        <asp:Button ID="btnAdd" runat="server" Text="Stock Out" Width="100px" OnClick="btnAdd_Click" />

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
            </div>

            
            
            
            <div class="row">
                <div class="col-md-12 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Fixed Assets Stocked Out Items
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="table-responsive">

                                <asp:GridView ID="ItemGrid2" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" SelectedRowStyle-BackColor="LightBlue"
                                    AllowPaging="False" PageSize="5"
                                    OnRowDataBound="ItemGrid_RowDataBound" OnRowDeleting="ItemGrid_RowDeleting" OnSelectedIndexChanged="ItemGrid_SelectedIndexChanged" Width="903px" >
                                   
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                                <asp:Label ID="lblEntryId" runat="server" CssClass="hidden" Text='<%# Bind("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20px"/>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="ItemCode" HeaderText="Item Code"/>
                                        <asp:BoundField DataField="ProductID" HeaderText="Item Name" />
                                        <asp:BoundField DataField="DeliveredQty" HeaderText="DeliveredQty" SortExpression="DeliveredQty" />
                                        <asp:BoundField DataField="Remark" HeaderText="Reason of Stock Out" SortExpression="Remark" />

                                                                              
                                        <asp:BoundField DataField="OutDate" HeaderText="OutDate" SortExpression="OutDate" DataFormatString={0:d} />
                                        <asp:BoundField DataField="StockOutBy" HeaderText="StockOutBy" SortExpression="StockOutBy" />
                                        
                                        <%--<asp:BoundField DataField="BrandID" HeaderText="Model"/>
                                        <asp:BoundField DataField="CompanyFor" HeaderText="Warranty"/>--%>

                                        <%--<asp:BoundField ItemStyle-Width="150px" DataField="ProductName" HeaderText="Product Name" ReadOnly="true">
                                            </asp:BoundField>--%>

                                        <%--<asp:TemplateField HeaderText="Current FixedAssets Qty." Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbliQtyCurrent" runat="server" Text='<%# Bind("DeliveredQty") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                       
                                       <%--  <asp:TemplateField HeaderText="Unit Price">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnitWeight" runat="server" Text='<%# Bind("UnitCost") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                        
                                        <%--<asp:TemplateField HeaderText="Adjust Qty.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbliQty" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>

                                        <asp:CommandField ButtonType="Image" ShowSelectButton="False" ShowDeleteButton="False"
                                            DeleteImageUrl="~/app/images/delete.png" SelectImageUrl="~/app/images/edit.png" />
                                    
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

