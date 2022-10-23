<%@ Page Title="Purchase Payment" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Payment.aspx.cs" Inherits="app_Payment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>



<asp:content id="Content1" contentplaceholderid="head" runat="Server">
    <style type="text/css">
        th {
            min-width: 80px;
        }

        td {
            text-align: center;
        }

        table#ctl00_BodyContent_ItemGrid tr th:first-child {
            max-width: 20px !important;
        }
    </style>
    <script>
        
        $(document).ready(function () {

            //$("input[type=text][id*=txtEid]").attr("disabled", true);
            jScript();
        });

        $(window).load(function () {

            jScript();
            calTotal();

        });

        function jScript() {
            $(".txtMult input").keyup(multInputs);
            $('input:checkbox').change(multInputs);

            function multInputs() {
                $("tr.txtMult").each(function() {
                        var $payTtl = $('.payTtl', this).text();
                        var $val1 = $('.val1', this).val();
                        var $total2 = ($payTtl * 1) - ($val1 * 1);
                        $('.netPay', this).text($total2.toFixed(2));
                    });

            }
        }

        function calTotal() {
           
      }

    </script>


</asp:content>

<asp:content id="Content4" contentplaceholderid="BodyContent" runat="Server">

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


            <h3 class="page-title">Purchase Payment</h3>
            <%--Payment From Members--%>

            <div class="row">
                <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Payment To Supplier
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">
                                <div class="control-group">
                                    <asp:Label ID="Label4" runat="server" Text="Entry Date :"></asp:Label>
                                    <asp:TextBox ID="txtColDate" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                                        Enabled="True" TargetControlID="txtColDate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="Label3" runat="server" Text="Payment Type: "></asp:Label>
                                    <asp:DropDownList ID="ddPaidTo" runat="server">
                                        <asp:ListItem>Advanced Payment</asp:ListItem>
                                        <asp:ListItem>Previous Bill</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label>Party Name: </label>
                                    <%--<asp:Label  ID="lblDeptName" runat="server" Text=""></asp:Label>--%>
                                    <asp:DropDownList ID="ddVendor" runat="server" DataSourceID="SqlDataSource1" CssClass="select2me" Width="70%"
                                        DataTextField="Company" DataValueField="PartyID" AutoPostBack="True" OnSelectedIndexChanged="ddVendor_OnSelectedIndexChanged">
                                    </asp:DropDownList><br />
                                    <span style="width: 100%; color: #0673B4; text-align: right;">
                                        <asp:Literal ID="lblBalance" runat="server"></asp:Literal>
                                    </span>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="vendor" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label6" runat="server" Text="Invoice No. : "></asp:Label>
                                    <asp:ListBox ID="lvOrders" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource6"
                                        DataTextField="InvDetail" DataValueField="InvNo"
                                        OnSelectedIndexChanged="lvOrders_OnSelectedIndexChanged" SelectionMode="Multiple" Height="80px"></asp:ListBox>
                                    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [InvNo], InvNo+': Bill# '+ BillNo+'- Dt: '+ (CONVERT(varchar,OrderDate,103) +'') AS InvDetail FROM [Purchase] WHERE ([SupplierID] = @SupplierID) AND SupplySource<>'LC' AND IsApproved='P' order by InvNo Desc">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddVendor" Name="SupplierID" PropertyName="SelectedValue" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <span style="width: 100%; color: green; text-align: right" id="instruction">
                                        <asp:Literal ID="ltrInv" runat="server">Press <em>Ctrl+Click</em> for multiple selection</asp:Literal></span>
                                </div>

                                <div class="table-responsive" id="tblInstruction" runat="server">


                                    <asp:GridView ID="ItemGrid" runat="server" AutoGenerateColumns="False" DataKeyNames="InvoiceNo" GridLines="Vertical">

                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle Width="20px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Invoice#">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("InvoiceNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:d}" />
                                         
                                            <asp:TemplateField HeaderText="Item Total">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceTotal" runat="server" Text='<%# Bind("ItemTotal") %>' Width="50px" CssClass="invTtl"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="VatService" HeaderText="VAT" />
                                            <asp:TemplateField HeaderText="Payable Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPayableAmount" runat="server" Text='<%# Bind("PurchaseTotal") %>' Width="50px" CssClass="payTtl"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="TDS Amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblTDSRate" runat="server" Text='<%# Bind("TDSAmount") %>' Width="100px" AutoPostBack="True" OnTextChanged="lblTDSRate_OnTextChanged" CssClass="val1"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="TDS Amount" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTDSAmt" runat="server" CssClass="multTotal1" Text='<%# Bind("TDSAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" CssClass="xerp_absolute_centre" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Net Payable">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNetPayable" runat="server" Text='<%# Bind("NetPayable") %>' CssClass="netPay"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Previous Payment">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCollectedAmount" runat="server" Text='<%# Bind("PreviousPayment") %>' CssClass="collAmt"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Payable">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCollectable"  runat="server" Text='<%# Bind("Payable") %>' CssClass="collAmt2"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Paid Amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblDueAmount" runat="server" AutoPostBack="True" OnTextChanged="lblDueAmount_OnTextChanged" Width="100px" CssClass="val3"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <EmptyDataTemplate></EmptyDataTemplate>
                                        <RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
                                        <FooterStyle BackColor="#CCCC99" />
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                        <SelectedRowStyle BackColor="SkyBlue" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="#1889CB" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>

                                <div class="control-group" id="divInvoiceAmount" runat="server">
                                    <asp:Label ID="Label14" runat="server" Text="Item Total : "></asp:Label>
                                    <asp:TextBox ID="txtInvAmt" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="control-group" id="divVATAmount" runat="server">
                                    <asp:Label ID="Label15" runat="server" Text="VAT Amount : "></asp:Label>
                                    <asp:TextBox ID="txtVATAmt" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="control-group" id="divPayableAmount" runat="server">
                                    <asp:Label ID="Label7" runat="server" Text="Payable Amount : "></asp:Label>
                                    <asp:TextBox ID="txtTotal" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                                </div>

                                <asp:Panel ID="paidpanel" runat="server" Visible="False">

                                    <div class="control-group" id="divPaidAmount" runat="server">
                                        <asp:Label ID="Label16" runat="server" Text="Paid Amount : "></asp:Label>
                                        <asp:TextBox ID="txtPaid" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                                    </div>

                                    <div class="control-group" id="divDueAmount" runat="server">
                                        <asp:Label ID="Label17" runat="server" Text="Payable : "></asp:Label>
                                        <asp:TextBox ID="txtDue" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </asp:Panel>


                                <div class="control-group" runat="server" id="divTDSAmount">
                                    <asp:Label ID="Label8" runat="server" Text="TDS Amount : "></asp:Label>
                                    <asp:TextBox ID="txtTDSAmount" runat="server" Text="0" onkeyup="calTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtTDSAmount">
                                    </asp:FilteredTextBoxExtender>
                                </div>


                                <div class="control-group">
                                    <asp:Label ID="Label1" runat="server" Text="Payment Mode: "></asp:Label>
                                    <asp:DropDownList ID="ddMode" runat="server" Width="120px"
                                        AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        <asp:ListItem>Cheque</asp:ListItem>
                                        <asp:ListItem>Cash</asp:ListItem>
                                        <asp:ListItem>Barter</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <asp:Panel ID="chqpanel" runat="server" Visible="true">

                                    <div class="control-group">
                                        <label class="control-label">Bank Account :  </label>
                                        <asp:DropDownList ID="ddBank" runat="server" DataSourceID="SqlDataSource8"
                                            DataTextField="BankName" DataValueField="ACID">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT ACID, (Select [BankName] FROM [Banks] where [BankId]=a.BankID) +' - '+ACNo +' - '+ACName AS BankName from BankAccounts a ORDER BY [ACName]"></asp:SqlDataSource>
                                    </div>
                                       <div class="control-group">
                                <asp:Label ID="Label9" runat="server" Text="Bank Branch : "></asp:Label>
                                <asp:TextBox ID="txtBranch" runat="server"></asp:TextBox>
                            </div>
                                    <div class="control-group">
                                        <asp:Label ID="Label2" runat="server" Text="Cheque No. : "></asp:Label>
                                        <asp:TextBox ID="txtDetail" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="control-group">
                                        <asp:Label ID="lblDate" runat="server" Text="Cheque Date:"></asp:Label>
                                        <asp:TextBox ID="txtChqDate" runat="server"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server"
                                            Enabled="True" TargetControlID="txtChqDate" Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </div>
                                </asp:Panel>

                                   <div class="control-group" runat="server" id="divInvcDueAmt">
                            <asp:Label ID="Label10" runat="server" Text="Invoice Payable Amount : "></asp:Label>
                            <asp:TextBox ID="txtInvTotal" runat="server" OnTextChanged="txtInvTotal_OnTextChanged" Text="0"></asp:TextBox>
                        </div>
                                  <div class="control-group">
                            <asp:Label ID="Label5" runat="server" Text="Amount : "></asp:Label>
                            <asp:TextBox ID="txtReceived" runat="server" Text="0"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtTotal">
                            </asp:FilteredTextBoxExtender>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Remarks : </label>
                                    <asp:TextBox ID="txtRemark" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>

                                <div class="form-actions">
                                     <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                    <asp:Button ID="btnSave" runat="server" Text="Save Payment" OnClick="btnSave_Click" />
                                </div>

                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Payments History
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

                                <asp:GridView ID="GridView2" runat="server">
                                </asp:GridView>

                                <asp:GridView ID="GridView1" runat="server" Width="100%" AllowSorting="True"
                                    AutoGenerateColumns="False" BackColor="White" Borderpayor="#999999"
                                    BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black"
                                    GridLines="Vertical" DataSourceID="SqlDataSource3" DataKeyNames="PaymentNo">
                                    <Columns>
                                        <asp:BoundField DataField="PaymentNo" HeaderText="Payment No." SortExpression="PaymentNo" ReadOnly="True" />
                                        <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" SortExpression="PaymentDate" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="PaidAmount" HeaderText="Paid Amount" SortExpression="PaidAmount" />
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="PurchaseInvNo" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [PaymentNo], [PaymentDate], [PartyName], [PaidAmount], Remark FROM [Payment] WHERE PartyID=@PartyID">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddVendor" Name="PartyID" PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                </fieldset>


	<asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div id="blur">&nbsp;</div>
            <div id="progress">
                Update in progress. Please wait ...

            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:content>
