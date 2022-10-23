<%@ Page Title="Finished Items (New)" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Finished-Items2.aspx.cs" Inherits="app_Finished_Items2" %>

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
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script> --%>


            <div class="row">

                <div class="col-md-6">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box green ">
                        
                        <div class="portlet-body form">
                            <div class="form-horizontal" role="form">
                                <div class="form-body">
                                    <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:LoginName ID="LoginName1" runat="server" Visible="false" />
                                    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>
                                    <asp:Label runat="server" ID="lblMsg" EnableViewState="False"></asp:Label>
                                    <%--<div class="col-md-4">
                                <div class="row">--%>

                                    <asp:Panel ID="pnlAdd" runat="server" DefaultButton="btnSave">

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
                
                
                 <asp:panel ID="pnlView2" runat="server" class="col-md-6" Visible="true">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>List of Finished Products Name
                                <asp:LinkButton ID="lbRefresh" runat="server" OnClick="lbRefresh_OnClick">Refresh</asp:LinkButton>
                            </div>
                            <%--<div class="tools">
                                <a href="#" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="#" class="reload"></a>
                                <a href="#" class="remove"></a>
                            </div>--%>
                        </div>
                        <div class="portlet-body form">
                            <div class="table-responsive">

                                <%--<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectedRowStyle-BackColor="LightBlue" DataKeyNames="pid" 
                                     OnRowDeleting="GridView1_OnRowDeleting">
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
                                         <%--<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Select" /> 
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
                                <%--<asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [pid], [ProductCode], [ProductName], [UnitPrice], [UnitWeight] FROM [FinishedProducts] WHERE ([CompanyID] = @CompanyID)"
                                    DeleteCommand="Delete FinishedProducts where pid='0'" >
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddCustomer" Name="CompanyID" PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>--%>
                                
                                
                                
                                <asp:ListView ID="SimpleGrid" runat="server">
                                    <LayoutTemplate>
                                        <table>
                                            <thead>
                                            <tr>
                                                <th>ProductId</th>
                                                <th>Product Code</th>
                                                <th>Product Name</th>
                                                <th>Unit Price</th>
                                                <th>Unit Weight</th>
                                                
                                            </tr>
                                            </thead>
                                            <tbody id="itemPlaceHolder" runat="server"></tbody>
                                        </table>
                                    </LayoutTemplate>

                                    <ItemTemplate>
                                        <tr style="height:30px;">
                                            <td><%# Eval("pid") %></td>
                                            <td><%# Eval("ProductCode") %></td>
                                            <td><%# Eval("ProductName") %></td>
                                            <td><%# Eval("UnitPrice") %></td>
                                            <td><%# Eval("UnitWeight") %></td>

                                        </tr>
                                        <tr style="background-color: #ddd; height:5px;">
                                            <td></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>


                        </div>
                    </div>
                </asp:panel>
                </div>
  
    
</asp:Content>

