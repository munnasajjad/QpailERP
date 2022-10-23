<%@ Page Title="Order Delivery" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Order-Delivery.aspx.cs" Inherits="app_Order_Delivery" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .col-md-4 .control-group input, .col-md-4 .control-group select {
            width: 100%;
        }

        .col-md-4 .control-group label {
            padding-bottom: 4px;
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
        input#ctl00_BodyContent_TextBox1 {
                width: 66px;
                height: 21px;
                float: right;
                margin: 0 0;
}
    </style>

    <script>
        $(document).ready(function () {

        });

        function calInvTotal() {
            var ttl = document.getElementById("<%= txtTotal.ClientID %>").value;
            var vat = document.getElementById("<%= txtVat.ClientID %>").value;
            var pay = parseFloat(ttl) + parseFloat(vat);
            $('#<%=txtPay.ClientID%>').val(pay.toString());
            //$('.vatamt').html("Total VAT Amount: " + parseFloat(ttl) * parseFloat(vat) / 100);
        }

        $(window).load(function () {
            jScript();

            //readonly inputs
            $('#<%=txtTotal.ClientID%>').attr('readonly', true);
            $('#<%=txtPay.ClientID%>').attr('readonly', true);
        });

        function jScript() {
            $(".txtMult input").keyup(multInputs);
            function multInputs() {
                var mult1 = 0; var mult2 = 0; var mult3 = 0; var ttlVat = 0; var ttlCont = 0;
                // for each row:
                $("tr.txtMult").each(function () {
                    // get the values from this row:
                    var $val1 = $('.val1', this).val();
                    var $val2 = $('.val2', this).val();
                    var $val3 = $('.val3', this).val();
                    var $val4 = $('.val4', this).val(); //pc per carton
                    var $val7 = $('.val7', this).val(); //vat percent
                    var $total1 = ($val1 * 1) * ($val2 * 1);//amount
                    var $total2 = ($val1 * 1) * ($val3 * 1)/1000;//weight
                    var $total3 = parseInt((($val1 * 1) / ($val4 * 1))+0.99);// no of carton/bag
                    var $total7 = ($total1 * 1) * ($val7 * 1) / 100;// vat amount
                    // set total for the row
                    $('.multTotal1', this).text($total1);
                    $('.multTotal2', this).text($total2);
                    $('.multTotal5', this).text($total3);
                    $('.multTotal7', this).text($total7);
                    mult1 += $total1;//Total Price
                    mult2 += $total2;//Total Weight ctl00_BodyContent_ltrWeight
                    mult3 += ($val1 * 1);//Total Qty ctl00_BodyContent_ltrQty
                    ttlVat += ($total7 * 1);
                    ttlCont += ($total3 * 1);
                });
                $("#ctl00_BodyContent_txtTotal").val(mult1);
                $("#ctl00_BodyContent_ltrWeight").text(mult2);
                $("#ctl00_BodyContent_ltrQty").text(mult3);
                $('#<%=txtVat.ClientID%>').val(ttlVat.toString());
                $('#<%=txtCarton.ClientID%>').val(ttlCont.toString());
                calInvTotal();
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">        </asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
        <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
            <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
        </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(jScript);
                Sys.Application.add_load(callJquery);
            </script>




    <div class="row">
        <div class="col-md-4 ">
            <!-- BEGIN SAMPLE FORM PORTLET-->
            <div class="portlet box blue">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Order Delivery
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
                                    <span style="text-align:center; width: 100%; padding: 6px 0;">Last Invoice No.: <asp:Literal ID="ltrLastInv" runat="server"></asp:Literal></span>
                                </div>
                            </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Date : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>

                            <div class="control-group">
                                <label class="control-label full-wdth">Sales Mode :  </label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddSalesMode" runat="server" DataSourceID="SqlDataSource3"
                                        DataTextField="OrderType" DataValueField="OrderType"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddSalesMode_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT Distinct OrderType FROM [Orders] WHERE (DeliveryStatus='P' OR DeliveryStatus='A') AND OrderType <> 'Direct' AND  OrderType <> 'PI' order by OrderType desc ">
                                    </asp:SqlDataSource>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label full-wdth">Customer Name :  </label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource1" DataTextField="Company" DataValueField="PartyID"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                            </div>

                            <div class="control-group">
                                <asp:Label ID="lblPONo" runat="server" Text="PO No." CssClass="control-label full-wdth" />
                                <div class="controls">
                                    <asp:ListBox ID="lvOrders" runat="server" AutoPostBack="True"
                                         OnSelectedIndexChanged="lbOrders_SelectedIndexChanged" SelectionMode="Multiple" Height="80px"></asp:ListBox>
                                    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [OrderID], OrderID+'  Dated: '+ (CONVERT(varchar,OrderDate,103)) AS InvDetail  FROM [Orders] WHERE CustomerName=@CustomerName AND ([DeliveryStatus] = 'A' OR [DeliveryStatus] = 'P') AND OrderType=@OrderType">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddCustomer" Name="CustomerName" PropertyName="SelectedValue" />
                                                    <asp:Parameter DefaultValue="D" Name="DeliveryStatus" Type="String" />
                                                    <asp:ControlParameter ControlID="ddSalesMode" Name="OrderType" PropertyName="SelectedValue" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                    <span style="width:300px; color:green;"  >
                                        <asp:Literal ID="ltrInv" runat="server">Press <em>Ctrl+Click</em> for multiple selection</asp:Literal></span>
                             </div>
                            </div>

                            <div class="control-group hidden">
                                <label class="control-label full-wdth">Selected Order(s) : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtInv" runat="server" ReadOnly="true" CssClass="form-control" placeholder="Invoice No."></asp:TextBox>
                                </div>
                            </div>

                            <div class="control-group">
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
                                       <asp:DropDownList ID="ddAddress" runat="server" DataSourceID="SqlDataSource4"
                                           DataTextField="DeliveryPointName" DataValueField="DeliveryPointsID" >
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [DeliveryPointsID], [DeliveryPointName] FROM [DeliveryPoints] WHERE CustomerID=@CustomerID ORDER BY [DeliveryPointName]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label full-wdth">Transport Type & No. : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtTransport" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                </div>
                            </div>



                            <asp:Panel ID="pnlPO" runat="server" Visible="false">

                            <div class="control-group">
                                <label class="control-label full-wdth">Invoice Maturity Days : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtMDays" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtMDays">
                                                </asp:FilteredTextBoxExtender>
                                </div>
                            </div>


                            <div class="control-group">
                                <label class="control-label full-wdth">Overdue Date: </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtOverDueDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtOverDueDate">
                                    </asp:CalendarExtender>
                                </div>
                            </div>

                                </asp:Panel>

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
                                <label class="control-label full-wdth">Remarks : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label full-wdth">Delivery From :  </label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddWarehouse" runat="server" DataSourceID="SqlDataSource2" DataTextField="StoreName" DataValueField="WID" >
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [WID], [StoreName] FROM [Warehouses] ORDER BY [StoreName]">
                                    </asp:SqlDataSource>
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
                                <label class="control-label full-wdth">VAT <asp:TextBox ID="TextBox1" runat="server"  onkeyup="calInvTotal()"/> </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtVat" runat="server" CssClass="form-control" placeholder="0.00" Text="0" />
                                    <span class="vatamt" style="text-align:center; width: 100%; padding: 6px 0;"></span>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label full-wdth">Payable : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtPay" runat="server" CssClass="form-control" placeholder="Payable"></asp:TextBox>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label full-wdth">Total no. of Cartons/Packs : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCarton" runat="server" CssClass="form-control" Text="" />
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label full-wdth">Challan/Slip# & Date :</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtChallanNo" runat="server" CssClass="form-control" />
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
                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtChallanDate">
                                    </asp:CalendarExtender>
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
                        <asp:CheckBox ID="chkDO" runat="server" Text="Challan" Visible="False" />

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

                            <div class="form-group">
                                <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>

                            </div>

                            <asp:Panel ID="Panel1" runat="server">
                            <div class="table-responsive">

                                <asp:GridView ID="ItemGrid" runat="server" AutoGenerateColumns="False"
                                    OnRowDataBound="ItemGrid_RowDataBound" CssClass="table-striped table-hover table-bordered"
                                    Width="150%" GridLines="Vertical" >

                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="20px"  CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntryId" runat="server" Text='<%# Bind("iCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%"  CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Product Name">
                                            <ItemTemplate>
                                                <asp:TextBox ID="lblPName" runat="server" Width="100%" Text='<%# Bind("ProductName") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" CssClass="xerp_vmiddle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Order Qty.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" Text='<%# Bind("qty") %>'></asp:Label>
                                                <asp:HiddenField ID="hidQty" Value='<%# Bind("qty") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Delivered Qty.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldQty" runat="server" Text='<%# Bind("dQty") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%"  CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Invoice Qty.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtpQty" runat="server" Text='<%# Bind("pQty") %>' Width="40px" CssClass="val1" ></asp:TextBox>  <%--onkeyup="updateGrid2()"  --%>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%"  CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Width="20px"  HeaderText="Unit">
                                            <ItemTemplate>
                                                PCS
                                            </ItemTemplate>
                                            <ItemStyle Width="20px"  CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPrice" runat="server" Text='<%# Bind("rate", "{0:0.00}") %>' Width="50px" CssClass="val2"></asp:TextBox>
                                                <asp:HiddenField ID="hidPrice" Value='<%# Bind("rate") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%"  CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubTotal" runat="server" CssClass="multTotal1" Text='<%# Bind("amt") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%"  CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="VAT (%)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtVatPercent" runat="server" Text='<%# Bind("VATPercent") %>'  Width="50px" CssClass="val7"></asp:TextBox>
                                                <asp:HiddenField ID="hidVatPercent" Value='<%# Bind("rate") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%"  CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="VAT Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVatAmount" runat="server" CssClass="multTotal7" Text='<%# Bind("VATAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%"  CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="I.Wght gm.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtItemWeight" runat="server" Text='<%# Bind("UnitWeight", "{0:0.000}") %>' Width="50px" CssClass="val3"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%"  CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Ttl Wght gm.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalWeight" runat="server" Text='<%# Bind("TotalWeight") %>' CssClass="multTotal2"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%"  CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Qty/Carton">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQtyPack" runat="server" Text='<%# Bind("QtyPack") %>' Width="40px"  CssClass="val4" ></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%"  CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total Cartons/Packs">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyPackTotal" runat="server" Text='<%# Bind("QtyPack") %>'  CssClass="multTotal5"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%"  CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Challan#">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtItemChallanNo" runat="server" Text='' Width="70px"  CssClass="val4x" ></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="xerp_absolute_centre" />
                                        </asp:TemplateField>

                                        <%--<asp:CommandField ButtonType="Button" ShowDeleteButton="True" />--%>

                                    </Columns>

                                    <RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
                                    <FooterStyle BackColor="#CCCC99" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </div>


                                    Total Quantity: <asp:Label ID="ltrQty" runat="server"></asp:Label> &nbsp; &nbsp;
                                    Total Weight: <asp:Label ID="ltrWeight" runat="server"></asp:Label> kg.

                                <asp:Button ID="btnEdit" runat="server" Text="Consumed Items" CssClass="btn_blue right" Visible="false" />

                                </asp:Panel>

                            <asp:Panel ID="pnlItemsConsumed" runat="server" Visible="false">

                            <legend>Consumed Items For Finished Goods</legend>

                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                    OnRowDataBound="ItemGrid_RowDataBound" CssClass="table table-striped table-hover table-bordered"
                                     BackColor="Red" BorderColor="#DEDFDE"
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
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>

                                        <asp:BoundField ItemStyle-Width="150px" DataField="ProductName" HeaderText="Product Name">
                                            <ItemStyle Width="30%"></ItemStyle>
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
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubTotal" runat="server" Text='<%# Bind("ItemTotal") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>

                                    </Columns>

                                    <RowStyle BackColor="#F7F7DE" />
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
    </div>



        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>

