<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Stock-in.aspx.cs" Inherits="app_Stock_in" %>

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
     <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

    <div class="row">
        <div class="col-md-4 ">
            <!-- BEGIN SAMPLE FORM PORTLET-->
            <div class="portlet box blue">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Finished Products Stock-in
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
                                <label class="control-label full-wdth">Stock-in Date : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtDate">
                                    </asp:CalendarExtender>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label full-wdth">Stock-in Godown : </label>
                                <asp:DropDownList ID="ddGodown" runat="server" DataSourceID="SqlDataSource6"
                                    DataTextField="StoreName" DataValueField="WID" CssClass="form-control"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddGodown_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource6" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [WID], [StoreName] FROM [Warehouses]"></asp:SqlDataSource>
                            </div>

                            <div class="control-group">
                                <label class="control-label full-wdth">Location in Godown : </label>
                                <asp:DropDownList ID="ddLocation" runat="server" DataSourceID="SqlDataSource7"
                                    DataTextField="AreaName" DataValueField="AreaID" CssClass="form-control">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT AreaID,[AreaName] FROM [WareHouseAreas] where Warehouse=@Warehouse">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddGodown" Name="Warehouse" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>

                            <div class="control-group">
                                <label class="control-label full-wdth">Remark :</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="" />
                                </div>
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
                        <i class="fa fa-reorder"></i>Item Order Details
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

                                        <div class="col-md-4">
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
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">For</label>

                                                <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource1" DataTextField="Company" DataValueField="PartyID"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Brand </label>

                                                <asp:DropDownList ID="ddBrand" runat="server" DataSourceID="SqlDataSource3" DataTextField="BrandName" DataValueField="BrandID">
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [BrandID], [BrandName] FROM [CustomerBrands] WHERE (([CustomerID] = @CustomerID) AND ([ProjectID] = @ProjectID))">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                                        <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>


                                        <div class="col-md-4">
                                            <div class="control-group bottom_fix" style="margin-bottom: -15px !important;">
                                                <label class="col-sm-12 control-label">Grade </label>

                                                <asp:DropDownList ID="ddGrade" runat="server" DataSourceID="SqlDataSource5"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddGrade_SelectedIndexChanged"
                                                    DataTextField="GradeName" DataValueField="GradeID">
                                                </asp:DropDownList>

                                                <label class="col-sm-12 control-label" for="ctl00_BodyContent_chkMerge" style="top: -3px!important">Merge</label>

                                                <asp:CheckBox ID="chkMerge" runat="server" CssClass="height_fix" />

                                                <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT GradeID,GradeName from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=2 AND ProjectID=1) ORDER BY [GradeName]"></asp:SqlDataSource>
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
                                                    SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE [CategoryID] IN (Select CategoryID from Categories where GradeID=@GradeID AND ProjectID=1) ORDER BY [ItemName]">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddGrade" Name="GradeID" PropertyName="SelectedValue" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <%--SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE ([CategoryID] IN (Select CategoryID from Categories where GradeID IN (Select GradeID from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=2 AND ProjectID=1)))) ORDER BY [ItemName]"></asp:SqlDataSource>--%>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label full-wdth">
                                                    Stock-in Qty.(<asp:Literal ID="ltrUnit" runat="server" />)</label>
                                                <asp:TextBox ID="txtQty" runat="server" onkeyup="calTotal()" />
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtQty">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <%--<div style="margin-top: 29px">--%>
                                                <asp:Button ID="btnAdd" runat="server" Text="Add" Width="100px" OnClick="btnAdd_Click" />

                                                <%--</div>--%>
                                            </div>
                                        </div>


                                    </asp:Panel>




                                </div>
                            </div>

                            Total Quantity:
                            <asp:Literal ID="ltrQty" runat="server"></asp:Literal>
                            PCS
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

                                            <ItemStyle Width="20px"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:BoundField ItemStyle-Width="150px" DataField="ProductName" HeaderText="Product Name" ReadOnly="true">
                                            <ItemStyle Width="60%"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lbliQty" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtQty" runat="server" Text='<%# Bind("Quantity") %>' CssClass="qtySpin" Width="50px"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>

                                        <asp:BoundField ItemStyle-Width="150px" DataField="UnitType" HeaderText="Unit" ReadOnly="true">
                                            <ItemStyle Width="5%"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderText="Rate" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbliPrc" runat="server" Text='<%# Bind("UnitCost") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtPrice" runat="server" Text='<%# Bind("UnitCost") %>' CssClass="amtSpin" Width="60px"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Amount" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubTotal" runat="server" Text='<%# Bind("ItemTotal") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Weight/Unit" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUWeight" runat="server" Text='<%# Bind("UnitWeight") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtUWeight" runat="server" Text='<%# Bind("UnitWeight") %>' CssClass="amtSpin" Width="60px"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>


                                        <asp:CommandField ButtonType="Image" ShowSelectButton="True" ShowDeleteButton="True"
                                            DeleteImageUrl="~/app/images/delete.png" SelectImageUrl="~/app/images/edit.png" />

                                    </Columns>

                                </asp:GridView>

                            </div>
                    

                </div>
            </div>
        </div>
    </div>



    </ContentTemplate>
    </asp:UpdatePanel>

    
                                

</asp:Content>

