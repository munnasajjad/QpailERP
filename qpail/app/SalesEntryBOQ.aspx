<%@ Page Title="Sales Entry (BOQ)" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="SalesEntryBOQ.aspx.cs" Inherits="app_SalesEntryBOQ" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .col-md-4 .control-group input, .col-md-4 .control-group select {
            width: 100%;
        }

        .col-md-8 {
            padding-right: 4px;
            padding-left: 4px;
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

        table tr td input {
            text-align: center;
        }

        .vatamt {
            text-align: center;
            width: 100% !important;
            padding: 6px 0;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#<%=txtTotal.ClientID%>').attr('readonly', true);
            $('#<%=txtPay.ClientID%>').attr('readonly', true);

            $("[id*=ItemGrid]input[type=text][id*=txtpQty]").change(function (e) {
                updateGrid(e);
            })
            $("[id*=ItemGrid]input[type=text][id*=txtpQty]").keyup(function (e) {
                updateGrid(e);
            })

            $("[id*=ItemGrid]input[type=text][id*=txtPrice]").change(function (e) {
                updateGrid(e);
            })
            $("[id*=ItemGrid]input[type=text][id*=txtPrice]").keyup(function (e) {
                updateGrid(e);
            })

            $("[id*=ItemGrid]input[type=text][id*=txtItemWeight]").change(function (e) {
                updateGrid(e);
            })
            $("[id*=ItemGrid]input[type=text][id*=txtItemWeight]").keyup(function (e) {
                updateGrid(e);
            })
        })

        function updateGrid(e) {
            var price = $(e.target).closest('tr').find("input[type=text][id*=txtPrice]").val();
            var quantity = $(e.target).closest('tr').find("input[type=text][id*=txtQty]").val();
            var total = parseInt(price) * parseInt(quantity);
            $(e.target).closest('tr').find("[id*=lblSubTotal]").text(total);
            $(e.target).closest('tr').find("[id*=hidQty]").text(quantity);

            GrossTotal();
            var iCode = $(e.target).closest('tr').find("[id*=lblEntryId]").text();
            $.ajax({
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                url: 'WebMethods.aspx/UpdateOrderGrid',
                data: "{'iCode':'" + iCode + "','iQty':'" + quantity + "','iPrice':'" + price + "'}",
                async: false,

                success: function (response) {

                },
                failure: function (response) {
                    alert(response.e);
                },
                error: function (response) {
                    alert(response.e);
                }
            });
        }
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

            var pay = parseFloat(ttl) - parseFloat(disc) + (parseFloat(ttl) * parseFloat(vat) / 100);
            $('#<%=txtPay.ClientID%>').val(pay.toString());
            $('.vatamt').html("Total VAT Amount: " + parseFloat(ttl) * parseFloat(vat) / 100);
        }

        var gross;
        function GrossTotal() {
            gross = 0;
            $("[id*=ItemGrid][id*=lblSubTotal]").each(function (index, item) {
                gross = gross + parseInt($(item).text());
            });
            document.getElementById("<%= txtTotal.ClientID %>").value = gross;
            Grandtotal();
        }

        function Grandtotal() {
            var txtTotal = document.getElementById("<%= txtTotal.ClientID %>").value;
            //var txtDiscount = document.getElementById("<%= txtDiscount.ClientID %>").value;
            //var txtPaid = document.getElementById("<%= txtPay.ClientID %>").value;
            //document.getElementById("<%= txtPay.ClientID %>").value = parseInt(txtTotal) - parseInt(txtDiscount) - parseInt(txtService);
        }

        $(window).load(function () {
            jScript();
        });

        function jScript() {
            $(".txtMult input").keyup(multInputs);
            function multInputs() {
                var mult1 = 0; var mult2 = 0; var mult3 = 0; var ttlVat = 0; var ttlCont = 0;
                // for each row:
                $("tr.txtMult").each(function () {
                    // get the values from this row:
                    var $valConsumptionQuantity = $('.valConsumptionQuantity', this).val();
                    var $valRate = $('.valRate', this).val();
                    var $valExcessQuantity = $('.valExcessQuantity', this).val();
                    var $totalConsumptionQuantity = ($valConsumptionQuantity * 1) + ($valExcessQuantity * 1);
                    var $totalAmount = ($totalConsumptionQuantity * 1) * ($valRate * 1);//amount

                    // set total for the row
                    $('.valConsumptionAmount', this).text($totalAmount);
                    //$('.multTotal2', this).text($total2);
                    //$('.multTotal5', this).text($total3);
                    //$('.multTotal7', this).text($total7);
                    //mult1 += $total1;//Total Price
                    //mult2 += $total2;//Total Weight ctl00_BodyContent_ltrWeight
                    //mult3 += ($val1 * 1);//Total Qty ctl00_BodyContent_ltrQty
                    //ttlVat += ($total7 * 1);
                    //ttlCont += ($total3 * 1);
                });
                <%--$("#ctl00_BodyContent_txtTotal").val(mult1);
                $("#ctl00_BodyContent_ltrWeight").text(mult2);
                $("#ctl00_BodyContent_ltrQty").text(mult3);
                $('#<%=txtVat.ClientID%>').val(ttlVat.toString());
                $('#<%=txtCarton.ClientID%>').val(ttlCont.toString());
                calInvTotal();--%>
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
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
            </script>

            <div class="row">
                <div class="col-md-4 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Direct Sales
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
                                        <label class="control-label full-wdth">Invoice No.:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" placeholder="" />
                                            <span style="text-align: center; width: 100%; padding: 6px 0;">Last Invoice No.:
                                                <asp:Literal ID="ltrLastInv" runat="server"></asp:Literal></span>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Date : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">PO No.:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtPoNo" runat="server" CssClass="form-control" placeholder="" />
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">PO Date : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtPODate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtPODate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="control-label full-wdth">Customer Name :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddCustomer" runat="server" CssClass="select2me" DataSourceID="SqlDataSource1" DataTextField="Company" DataValueField="PartyID"
                                                AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                                <SelectParameters>
                                                    <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                    </div>

                                    <div class="control-group hidden">
                                        <label class="control-label full-wdth">Period: </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtPeriod" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY TO DD/MM/YYYY"></asp:TextBox>

                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Date of Delivery: </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtDeliveryDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Time of Delivery : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTime" runat="server" CssClass="form-control" placeholder="hh:mm AM"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Delivery Point:</label>
                                        <div class="controls">
                                            <%--<asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="" />--%>
                                            <asp:DropDownList ID="ddAddress" runat="server" DataSourceID="SqlDataSource3"
                                                DataTextField="DeliveryPointName" DataValueField="DeliveryPointsID">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [DeliveryPointsID], [DeliveryPointName] FROM [DeliveryPoints] WHERE CustomerID=@CustomerID ORDER BY [DeliveryPointName]">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>

                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">VAT Challan No. :</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtVatNo" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">VAT Challan Date :</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtChallanDate" runat="server" CssClass="form-control" />
                                            <%--<asp:Button runat="server" id="btnC" Text="C"/>--%>
                                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                                Animated="True" PopupPosition="TopRight" TargetControlID="txtChallanDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Transport Type & No. : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTransport" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group hidden">
                                        <label class="control-label full-wdth">Invoice Maturity Days : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtMDays" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtMDays">
                                            </asp:FilteredTextBoxExtender>
                                        </div>
                                    </div>

                                    <div class="control-group hidden">
                                        <label class="control-label full-wdth">Overdue Date: </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtOverDueDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtOverDueDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>

                                    <%--<div class="control-group">
                                <label class="control-label full-wdth">Overdue Days : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtOverDueDays" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7"
                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtMDays">
                                                </asp:FilteredTextBoxExtender>
                                </div>
                            </div>--%>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Lid : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Delivery From :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddWarehouse" runat="server" DataSourceID="SqlDataSource2" DataTextField="StoreName" DataValueField="WID">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [WID], [StoreName] FROM [Warehouses] ORDER BY [StoreName]"></asp:SqlDataSource>
                                        </div>
                                    </div>

                                    <%--<hr />--%>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Total Amount: </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTotal" runat="server" CssClass="form-control" placeholder="Total Amount" Text="0" />
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">VAT (%) : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtVat" runat="server" CssClass="form-control" placeholder="(%)" onkeyup="calInvTotal()" Text="15" />
                                            <%--<span class="vatamt" style="text-align:center; width: 100%; padding: 6px 0;"></span>--%>
                                            <asp:Label ID="lblVatAmt" runat="server" CssClass="vatamt" />
                                        </div>
                                    </div>

                                    <div class="control-group hidden">
                                        <label class="control-label full-wdth">Discount : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control" placeholder="(%)" onkeyup="calInvTotal()" Text="0" />
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Payable : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtPay" runat="server" CssClass="form-control" placeholder="Payable"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">No. of Carton/ Bag/ Pack : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCarton" runat="server" CssClass="form-control" Text="" />
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Manufacturing Date :</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtManufacturingDate" runat="server" CssClass="form-control" />
                                            <asp:CalendarExtender ID="CalendarExtender6" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtManufacturingDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Expiry Date :</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" />
                                            <asp:CalendarExtender ID="CalendarExtender7" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtExpiryDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>

                                    <div class="control-group hidden">
                                        <label class="control-label full-wdth">Challan/Slip# & Date :</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtChallanNo" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Invoicing Company: </label>
                                        <div class="controls">

                                            <asp:DropDownList ID="ddLevelX" runat="server" CssClass="form-control">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource7p" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT LevelID, LevelName FROM [UserLevel] WHERE LevelID<@LevelID ORDER BY [LevelID]"></asp:SqlDataSource>
                                        </div>
                                    </div>
                                    <%--<div style="height:120px;">&nbsp;</div>--%>
                                </div>
                            </div>

                            <div class="form-actions">
                                <asp:CheckBox ID="chkPrint" runat="server" Text="Print Invoice" Checked="true" />
                                <asp:CheckBox ID="chkNonVat" runat="server" Text="Non-VAT" Checked="False" />
                                <asp:CheckBox ID="chkDO" runat="server" Text="Challan" Visible="False" />

                                <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click" />
                                <%--<asp:Button ID="btnEdit" CssClass="btn red" runat="server" Text="Edit" />
                        <asp:Button ID="btnPrint" CssClass="btn purple" runat="server" Text="Print" />--%>
                                <asp:Button ID="btnCancel" CssClass="btn default" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                            </div>

                            <div style="height: 150px">&nbsp;</div>

                        </div>
                    </div>
                </div>

                <div class="col-md-8" style="padding-left: 4px; padding-right: 4px">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box green ">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                Items in Order
                                <asp:LinkButton ID="lbRefresh" runat="server" OnClick="lbRefresh_OnClick">Refresh</asp:LinkButton>
                            </div>
                            <div class="tools">
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal" role="form">
                                <div class="form-body">

                                    <asp:LoginName ID="LoginName1" runat="server" Visible="false" />
                                    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>

                                    <div class="col-md-12" style="padding-left: 4px; padding-right: 4px;">
                                        <div class="form-group">
                                            <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>
                                        </div>

                                        <div class="col-md-12">
                                            <div class="row">
                                                <asp:Panel ID="pnlAdd" runat="server" DefaultButton="btnAdd">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="col-sm-12 control-label">Product Name </label>
                                                            <asp:DropDownList ID="ddProductName" runat="server" DataSourceID="SqlDataSource10" DataTextField="ProductName" DataValueField="pid"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddProductName_OnSelectedIndexChanged" CssClass="select2me" Width="100%">
                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSource10" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                SelectCommand="SELECT [pid], [ProductName]+' --- '+[ProductCode]  As ProductName, [ProductID] FROM [FinishedProducts] WHERE ([CompanyID] = @CompanyID)">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="ddCustomer" Name="CompanyID" PropertyName="SelectedValue" Type="String" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-sm-12 control-label">Packaging Item</label>
                                                            <asp:DropDownList ID="ddPackingItems" runat="server" DataSourceID="SqlDataSource11" DataTextField="ItemName" DataValueField="ItemCode" CssClass="select2me" Width="100%">
                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                SelectCommand="SELECT        vw_FIFO.ItemCode, Products.ItemName
FROM            vw_FIFO INNER JOIN
                         Products ON vw_FIFO.ItemCode = Products.ProductID
WHERE        (vw_FIFO.Type = 'Poly Production Output')">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="ddCustomer" Name="CompanyID" PropertyName="SelectedValue" Type="String" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-sm-12 control-label">Quantity per bag : </label>
                                                            <asp:TextBox ID="txtQuantityPerBag" CssClass="form-control" runat="server"/>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <%--<label class="col-sm-12 control-label">Product Name </label>--%>
                                                            <asp:GridView ID="ComponentGrid" runat="server" AutoGenerateColumns="False" GridLines="Vertical">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Id" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEntryId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="5%" CssClass="xerp_absolute_centre" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-Width="20px" HeaderText="Component">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="labelItemCode" runat="server" Width="100%" Text='<%# Bind("ItemCode") %>' CssClass="hidden"></asp:TextBox>
                                                                            <asp:DropDownList ID="ddComponent" runat="server" DataSourceID="SqlDataSource9" DataTextField="ItemName" DataValueField="ItemCode" AppendDataBoundItems="True"
                                                                                AutoPostBack="False" CssClass="select2me" Width="80%">
                                                                                <asp:ListItem Value="0">--- N/A ---</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:SqlDataSource ID="SqlDataSource9" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                                SelectCommand="SELECT DISTINCT F.ItemCode, ISNULL(B.BrandName,'')+'  '+ISNULL(C.BrandName,'')+'  '+ P.ItemName AS ItemName FROM tblFifo AS F 
                                                                                LEFT OUTER JOIN Products AS P ON P.ProductID = F.ItemCode LEFT OUTER JOIN CustomerBrands AS C ON F.BrandId = C.BrandID LEFT OUTER JOIN Brands AS B ON F.SizeId = B.BrandID 
                                                                                WHERE F.InType = 'SemiFinished' AND F.OutTypeId =  ''">
                                                                                <%--<SelectParameters>
                                                                                        <asp:ControlParameter ControlID="ddCustomer" Name="CompanyID" PropertyName="SelectedValue" Type="String" />
                                                                                    </SelectParameters>--%>
                                                                            </asp:SqlDataSource>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Quantity">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtComponentQuantity" runat="server" Text='<%# Bind("Quantity") %>' CssClass="qtySpin" Width="50px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <RowStyle BackColor="#F7F7DE" />
                                                                <FooterStyle BackColor="#CCCC99" />
                                                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                                                <HeaderStyle Font-Bold="True" ForeColor="Black" />
                                                                <AlternatingRowStyle BackColor="White" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-4 hidden">
                                                        <div class="control-group">
                                                            <label class="col-sm-12 control-label full-wdth">Pack Size </label>
                                                            <asp:DropDownList ID="ddSize" runat="server" DataSourceID="SqlDataSource4" DataTextField="BrandName" DataValueField="BrandID">
                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] WHERE ([ProjectID] = @ProjectID) order by DisplaySl">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4 hidden">
                                                        <div class="control-group">
                                                            <label class="col-sm-12 control-label">Brand </label>
                                                            <asp:DropDownList ID="ddBrand" runat="server" DataSourceID="SqlDataSource5" DataTextField="BrandName" DataValueField="BrandID">
                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                SelectCommand="SELECT [BrandID], [BrandName] FROM [CustomerBrands] WHERE (([CustomerID] = @CustomerID) AND ([ProjectID] = @ProjectID)) Order by BrandName">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                                                    <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-4 hidden">
                                                        <div class="control-group bottom_fix" style="margin-bottom: -15px !important;">
                                                            <label class="col-sm-12 control-label">Grade </label>
                                                            <asp:DropDownList ID="ddGrade" runat="server" DataSourceID="SqlDataSource7"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddGrade_SelectedIndexChanged"
                                                                DataTextField="GradeName" DataValueField="GradeID">
                                                            </asp:DropDownList>
                                                            <label class="col-sm-12 control-label" for="ctl00_BodyContent_chkMerge" style="top: -3px!important">Merge</label>
                                                            <asp:CheckBox ID="chkMerge" runat="server" CssClass="height_fix" />
                                                            <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                SelectCommand="SELECT GradeID,GradeName from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=2 AND ProjectID=1) ORDER BY [GradeName]"></asp:SqlDataSource>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4 hidden">
                                                        <div class="control-group bottom_fix" style="margin-bottom: -15px !important;">
                                                            <label class="col-sm-12 control-label">Category </label>
                                                            <asp:DropDownList ID="ddCategory" runat="server" DataSourceID="SqlDataSource6"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged"
                                                                DataTextField="CategoryName" DataValueField="CategoryID">
                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                SelectCommand="SELECT CategoryID, CategoryName FROM [Categories] where GradeID = @GradeID ORDER BY [CategoryName]">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="ddGrade" Name="GradeID" PropertyName="SelectedValue" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4 hidden">
                                                        <div class="control-group">
                                                            <label class="col-sm-12 control-label">Product </label>
                                                            <asp:DropDownList ID="ddProduct" runat="server" DataSourceID="SqlDataSource8"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddProduct_SelectedIndexChanged"
                                                                DataTextField="ItemName" DataValueField="ProductID">
                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE [CategoryID] = @CategoryID ORDER BY [ItemName]">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="ddCategory" Name="CategoryID" PropertyName="SelectedValue" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                            <%--SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE ([CategoryID] IN (Select CategoryID from Categories where GradeID IN (Select GradeID from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=2 AND ProjectID=1)))) ORDER BY [ItemName]"></asp:SqlDataSource>--%>
                                                        </div>
                                                    </div>

                                                    <div style="font-weight: bold;">
                                                        Available Stock :
                                                        <asp:Label ID="lblAvailableStock" runat="server" Style="color: red; font-size: 14px;"></asp:Label>
                                                        Pcs.
                                                    </div>

                                                    <div class="col-md-4">
                                                        <div class="control-group">
                                                            <label class="col-sm-12 control-label full-wdth">Available in Stock</label>
                                                            <asp:TextBox ID="txtStock" runat="server" ReadOnly="true" />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4">
                                                        <div class="control-group">
                                                            <label class="col-sm-12 control-label full-wdth">
                                                                Quantity(<asp:Literal ID="ltrUnit" runat="server" />)</label>
                                                            <asp:TextBox ID="txtQty" runat="server" onkeyup="calTotal()" />
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtQty">
                                                            </asp:FilteredTextBoxExtender>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4">
                                                        <div class="control-group">
                                                            <label class="col-sm-12 control-label">Rate </label>
                                                            <asp:TextBox ID="txtRate" runat="server" onkeyup="calTotal()" ReadOnly="true" />
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtRate">
                                                            </asp:FilteredTextBoxExtender>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4">
                                                        <div class="control-group">
                                                            <label class="col-sm-12 control-label">Amount : </label>
                                                            <asp:TextBox ID="txtAmount" runat="server" ReadOnly="true" />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4 hidden">
                                                        <div class="control-group">
                                                            <label class="col-sm-12 control-label full-wdth">Weight/unit (gm) </label>
                                                            <asp:TextBox ID="txtWeight" runat="server" Enabled="True" />
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtWeight">
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
                                                    <div class="col-md-8">
                                                        <div class="control-group">
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>

                                    <asp:Panel ID="Panel1" runat="server">
                                        <div class="portlet box green">
                                            <div class="portlet-title">
                                                <div class="caption">
                                                    <i class="fa fa-reorder"></i>Added item list
                                                </div>
                                            </div>
                                            <div class="portlet-body form">
                                                <div class="table-responsive">

                                                    <asp:GridView ID="ItemGrid" runat="server" AutoGenerateColumns="False"
                                                        OnRowDataBound="ItemGrid_RowDataBound" CssClass="table-striped table-hover table-bordered"
                                                        Width="100%" GridLines="Vertical" OnRowDeleting="ItemGrid_RowDeleting" OnSelectedIndexChanged="ItemGrid_SelectedIndexChanged">

                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-Width="20px">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20px" CssClass="xerp_absolute_centre" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Id" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEntryId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" CssClass="xerp_absolute_centre" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Product Name">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblPName" runat="server" Text='<%# Bind("ProductName") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40%" CssClass="xerp_vmiddle" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Invoice Qty.">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtpQty" runat="server" Text='<%# Bind("Quantity") %>' Width="40px" ReadOnly="true"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="7%" CssClass="xerp_absolute_centre" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="20px" HeaderText="Unit">
                                                                <ItemTemplate>
                                                                    PCS
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20px" CssClass="xerp_absolute_centre" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Rate">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPrice" runat="server" Text='<%# Bind("UnitCost", "{0:0.00000000}") %>' Width="50px" ReadOnly="true"></asp:TextBox>
                                                                    <asp:HiddenField ID="hidPrice" Value='<%# Bind("UnitCost", "{0:0.00000000}") %>' runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" CssClass="xerp_absolute_centre" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSubTotal" runat="server" Text='<%# Bind("ItemTotal", "{0:0.00000000}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" CssClass="xerp_absolute_centre" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="I.Wght">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtItemWeight" runat="server" Text='<%# Bind("UnitWeight", "{0:0.000}") %>' Width="60px" ReadOnly="true"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" CssClass="xerp_absolute_centre" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Ttl Wght">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotalWeight" runat="server" Text='<%# Bind("TotalWeight") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" CssClass="xerp_absolute_centre" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Qty/Carton">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtQtyPack" runat="server" Text='<%# Bind("QtyPack") %>' Width="40px"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" CssClass="xerp_absolute_centre" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Challan#">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtItemChallanNo" runat="server" Text='' Width="70px" CssClass="val4x"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="xerp_absolute_centre" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Select" />
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
                                                                <ItemStyle Width="95px" />
                                                            </asp:TemplateField>

                                                        </Columns>

                                                        <RowStyle BackColor="#F7F7DE" />
                                                        <FooterStyle BackColor="#CCCC99" />
                                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                        <SelectedRowStyle BackColor="#BDE5F8" Font-Bold="True" ForeColor="#1889CB" />
                                                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                                        <AlternatingRowStyle BackColor="White" />
                                                    </asp:GridView>
                                                </div>

                                                Total Quantity:
                                                <asp:Literal ID="ltrQty" runat="server"></asp:Literal>
                                                &nbsp; &nbsp;
                                                    Total Weight:
                                                <asp:Literal ID="ltrWeight" runat="server"></asp:Literal>
                                                kg.

                                                    <asp:Button ID="btnEdit" runat="server" Text="Consumed Items" CssClass="btn_blue right" Visible="false" />
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlItemsConsumed" runat="server">
                                        <legend>Cost of Goods Sold</legend>
                                        <asp:GridView ID="ConsumedItemGrid" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-striped table-hover table-bordered"
                                            BackColor="Red" BorderColor="#DEDFDE"
                                            BorderStyle="None" BorderWidth="1px" CellPadding="1" ForeColor="Black" RowStyle-BackColor="#A1DCF2" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                                            GridLines="Vertical" OnRowDeleting="ConsumedItemGrid_OnRowDeleting" OnSelectedIndexChanged="ConsumedItemGrid_OnSelectedIndexChanged">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labelComponentId" runat="server" Text='<%# Bind("ComponentId") %>'></asp:Label>
                                                        <asp:Label ID="labelComponentName" runat="server" Text='<%# Bind("ComponentName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>

                                                <asp:BoundField ItemStyle-Width="80px" DataField="ComponentName" HeaderText="Component Name">
                                                    <ItemStyle Width="25%"></ItemStyle>
                                                </asp:BoundField>
                                                <%--<asp:BoundField ItemStyle-Width="150px" DataField="CompanyName" HeaderText="Company Name">
                                                    <ItemStyle Width="30%"></ItemStyle>
                                                </asp:BoundField>--%>

                                                <asp:TemplateField HeaderText="Delivered Quantity">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQty" runat="server" Text='<%# Bind("ConsumptionQuantity") %>' CssClass="valConsumptionQuantity" ReadOnly="true" Width="50px"></asp:TextBox>
                                                        <asp:HiddenField ID="hidQty" Value='<%# Bind("ConsumptionQuantity") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Unit Price">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPrice" runat="server" Text='<%# Bind("Rate") %>' CssClass="valRate" ReadOnly="true" Width="80px"></asp:TextBox>
                                                        <asp:HiddenField ID="hidPrice" Value='<%# Bind("Rate") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Excess Quantity (+/-)">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtExcessQuantity" runat="server" Text='<%# Bind("ExcessQuantity") %>' CssClass="valExcessQuantity" Width="80px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Consumed Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubTotal" runat="server" Text='<%# Bind("ConsumedAmount") %>' CssClass="valConsumptionAmount"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%# Bind("Remarks") %>' CssClass="amtSpin" Width="230px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" />
                                                </asp:TemplateField>

                                                <%-- <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Select" />
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
                                                    <ItemStyle Width="95px" />
                                                </asp:TemplateField>--%>
                                            </Columns>

                                            <RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
                                            <FooterStyle BackColor="#CCCC99" />
                                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



