<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Sales-Xclusive.aspx.cs" Inherits="app_Sales_Xclusive" %>

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
    </style>



    <script type="text/javascript">
        $(document).ready(function () {

            $('#<%=txtAmount.ClientID%>').attr('readonly', true);
            $('#<%=txtTotal.ClientID%>').attr('readonly', true);
            $('#<%=txtPay.ClientID%>').attr('readonly', true);
        })

        function calTotal() {
            var rate = $('#<%=txtRate.ClientID%>').val();
            var qty = $('#<%=txtQty.ClientID%>').val();

            var amount = parseFloat(rate) * parseFloat(qty);
            $('#<%=txtAmount.ClientID%>').val(amount.toString());
        }

        function calInvTotal() {
            var ttl = $('#<%=txtTotal.ClientID%>').val();
            var disc = $('#<%=txtDiscount.ClientID%>').val();
            var vat = $('#<%=txtVat.ClientID%>').val();

            var pay = parseFloat(ttl) - parseFloat(disc) + (parseFloat(ttl)*parseFloat(vat) /100);
            $('#<%=txtPay.ClientID%>').val(pay.toString());
        }

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
                        <i class="fa fa-reorder"></i>Sales Entry
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
                                <label class="control-label full-wdth">Customer Name :  </label>
                                <div class="controls">
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
                            
                            <div class="control-group">
                                <label class="control-label full-wdth">Order No. : </label>
                                <div class="controls">
                                    <asp:ListBox ID="lbOrders" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource6" DataTextField="OrderID" DataValueField="OrderID"
                                         OnSelectedIndexChanged="lbOrders_SelectedIndexChanged" SelectionMode="Multiple" Height="80px"></asp:ListBox>
                                    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [OrderID] FROM [Orders] WHERE ([CustomerName] = @CustomerName)">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddCustomer" Name="CustomerName" PropertyName="SelectedValue" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <span style="width:200px; color:green;"  >Press <em>Ctrl+Click</em> for multiple selection</span>
                                </div>
                                
                            </div>

                            <div class="control-group">
                                <label class="control-label full-wdth">Selected Order(s) : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtInv" runat="server" ReadOnly="true" CssClass="form-control" placeholder="Invoice No."></asp:TextBox>
                                    <asp:DropDownList ID="ddOrders" runat="server" DataSourceID="SqlDataSource1" DataTextField="Company" DataValueField="PartyID"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged" Visible="false">
                                    </asp:DropDownList>
                                </div>                                
                            </div>

                            <div class="control-group hidden">
                                <label class="control-label full-wdth">Order Date : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtDate">
                                    </asp:CalendarExtender>
                                </div>
                            </div>

                            <div class="control-group hidden">
                                <label class="control-label full-wdth">Delivery Date : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtDeliveryDate">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                            

                            <div class="control-group hidden">
                                <label class="control-label full-wdth">Delivery Location:</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="" />
                                </div>
                            </div>


                            <hr />

                            <div class="form-group">
                                <label class="col-sm-6 control-label">Total Amount: </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtTotal" runat="server" CssClass="form-control" placeholder="Total Amount" Text="0" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-6 control-label">Discount : </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control" placeholder="(-)" onkeyup="calInvTotal()" Text="0" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-6 control-label">VAT (%) : </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtVat" runat="server" CssClass="form-control" placeholder="(%)" onkeyup="calInvTotal()" Text="0" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-6 control-label">Payable : </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtPay" runat="server" CssClass="form-control" placeholder="Payable"></asp:TextBox>
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
                        <i class="fa fa-reorder"></i>Items in Order(s)
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

                            <asp:LoginName ID="LoginName1" runat="server" Visible="false" />
                            <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>

                            <div class="col-md-12">
                                <div class="row">

                                    <asp:Panel ID="pnlAdd" runat="server" DefaultButton="btnAdd" Visible="false">

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


                                        <div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Grade </label>

                                                <asp:DropDownList ID="ddGrade" runat="server" DataSourceID="SqlDataSource5"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddProduct_SelectedIndexChanged"
                                                    DataTextField="ItemName" DataValueField="ProductID">
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE ([CategoryID] IN (Select CategoryID from Categories where GradeID IN (Select GradeID from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=2 AND ProjectID=1)))) ORDER BY [ItemName]"></asp:SqlDataSource>
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
                                                    SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE ([CategoryID] IN (Select CategoryID from Categories where GradeID IN (Select GradeID from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=2 AND ProjectID=1)))) ORDER BY [ItemName]"></asp:SqlDataSource>

                                            </div>
                                        </div>


                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label full-wdth">Quantity in
                                                    <asp:Literal ID="ltrUnit" runat="server" /></label>
                                                <asp:TextBox ID="txtQty" runat="server" onkeyup="calTotal()" />
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtQty">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Rate </label>
                                                <asp:TextBox ID="txtRate" runat="server" onkeyup="calTotal()" />
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtQty">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Amount : </label>
                                                <asp:TextBox ID="txtAmount" runat="server" />
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtQty">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <asp:Button ID="btnAdd" runat="server" Text="Add" Width="100px" OnClick="btnAdd_Click" />
                                            </div>
                                        </div>


                                    </asp:Panel>
                                    

                                </div>
                            </div>


                            <div class="form-group">
                                <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>

                            </div>


                            <div class="table-responsive">


                                <asp:GridView ID="ItemGrid" runat="server" AutoGenerateColumns="False"
                                    OnRowDataBound="ItemGrid_RowDataBound" CssClass="table table-striped table-hover table-bordered"
                                    OnRowDeleting="ItemGrid_RowDeleting" BackColor="Red" BorderColor="#DEDFDE"
                                    BorderStyle="None" BorderWidth="1px" CellPadding="1" ForeColor="Black" RowStyle-BackColor="#A1DCF2" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                                    GridLines="Vertical">

                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntryId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>

                                        <asp:BoundField ItemStyle-Width="150px" DataField="ProductName" HeaderText="Product Name">
                                            <ItemStyle Width="40%"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQty" runat="server" Text='<%# Bind("Quantity") %>' CssClass="qtySpin" ReadOnly="true" Width="50px"></asp:TextBox>
                                                <asp:HiddenField ID="hidQty" Value='<%# Bind("Quantity") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>

                                        <asp:BoundField ItemStyle-Width="150px" DataField="UnitType" HeaderText="Unit">
                                            <ItemStyle Width="5%"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPrice" runat="server" Text='<%# Bind("UnitCost") %>' CssClass="amtSpin" ReadOnly="true" Width="80px"></asp:TextBox>
                                                <asp:HiddenField ID="hidPrice" Value='<%# Bind("UnitCost") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubTotal" runat="server" Text='<%# Bind("ItemTotal") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />

                                    </Columns>

                                    <RowStyle BackColor="#F7F7DE" />
                                    <FooterStyle BackColor="#CCCC99" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </div>

                                Total Quantity: <asp:Literal ID="ltrQty" runat="server"></asp:Literal>



                            <legend>Consumed Items</legend>
                            
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                    OnRowDataBound="ItemGrid_RowDataBound" CssClass="table table-striped table-hover table-bordered"
                                    OnRowDeleting="ItemGrid_RowDeleting" BackColor="Red" BorderColor="#DEDFDE"
                                    BorderStyle="None" BorderWidth="1px" CellPadding="1" ForeColor="Black" RowStyle-BackColor="#A1DCF2" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                                    GridLines="Vertical">

                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntryId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>

                                        <asp:BoundField ItemStyle-Width="150px" DataField="ProductName" HeaderText="Product Name">
                                            <ItemStyle Width="40%"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQty" runat="server" Text='<%# Bind("Quantity") %>' CssClass="qtySpin" ReadOnly="true" Width="50px"></asp:TextBox>
                                                <asp:HiddenField ID="hidQty" Value='<%# Bind("Quantity") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>

                                        <asp:BoundField ItemStyle-Width="150px" DataField="UnitType" HeaderText="Unit">
                                            <ItemStyle Width="5%"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPrice" runat="server" Text='<%# Bind("UnitCost") %>' CssClass="amtSpin" ReadOnly="true" Width="80px"></asp:TextBox>
                                                <asp:HiddenField ID="hidPrice" Value='<%# Bind("UnitCost") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubTotal" runat="server" Text='<%# Bind("ItemTotal") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />

                                    </Columns>

                                    <RowStyle BackColor="#F7F7DE" />
                                    <FooterStyle BackColor="#CCCC99" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>




                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


            
        </ContentTemplate>
    </asp:UpdatePanel>
    


</asp:Content>

