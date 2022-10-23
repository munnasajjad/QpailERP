<%@ Page Title="Sales Collection" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Collection.aspx.cs" Inherits="app_Collection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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

            $("input[type=checkbox][id*=cbAdvanceCollection]").click(function () {
                if (this.checked)
                    $("input[type=text][id*=txtReceived]").attr("readonly", false);
              //  $('#<%=txtInvTotal.ClientID%>').attr('readonly', false);
               else
                   $("input[type=text][id*=txtReceived]").attr("readonly", true);
                    //$('#<%=txtInvTotal.ClientID%>').attr('readonly', true);
           });
       });



        $(window).load(function () {
            jScript();
            calTotal();

        });

        function calTotal() {
            var due = $('#<%=txtCollection.ClientID%>').val();
            var adjust = $('#<%=txtAdjust.ClientID%>').val();
            var amount2 = parseFloat(due);// + parseFloat(adjust * 1);
            $('#<%=txtReceived.ClientID%>').val(amount2.toFixed(2));
        }

        function jScript() {
            $(".txtMult input").keyup(multInputs);
            $('input:checkbox').change(multInputs);
            function multInputs() {
                var mult1 = 0; var mult2 = 0; var mult3 = 0; var mult4 = 0; var ttlVat = 0; var ttlCont = 0;
                // for each row:
                $("tr.txtMult").each(function () {
                    // get the values from this row:
                    var $invTtl = $('.invTtl', this).text();
                    var $payTtl = $('.payTtl', this).text();

                    var $val1 = $('.val1', this).val();//TDS Rate
                    var $val2 = $('.val2', this).val();// VDSRate

                    var row = $(this);

                    var $tds = 0; var $val4 = $('.val4', this).val();// Bad debt Amount
                    if (row.find('input[type="checkbox"]').is(':checked')) {
                        //$tds = ($payTtl * 1) * ($val1 * 1) / 100;
                        $tds = ($val1 * 1);
                    } else
                        //$tds = ($invTtl * 1) * ($val1 * 1) / 100;
                        $tds = ($val1 * 1);
                    $('.multTotal1', this).text($tds.toFixed(2));

                    var $vds = 0;

                    if (row.find('input[type="checkbox"]').is(':checked')) {
                       // $vds = ($payTtl * 1) * ($val2 * 1) / 100;
                        $vds = ($val2 * 1);
                    }
                    else
                        //$vds = ($invTtl * 1) * ($val2 * 1) / 100;
                        $vds = ($val2 * 1);

                    $('.multTotal2', this).text($vds.toFixed(2));

                    //var $total1 = ($val1 * 1) * ($val2 * 1);//tds amount
                    var $total2 = ($payTtl * 1) - ($tds * 1) - ($vds * 1) - ($val4 * 1);
                    $('.netPay', this).text($total2.toFixed(2));

                    var $collAmt = $('.collAmt', this).text();

                    var $total3 = parseFloat(($total2 * 1) - ($collAmt * 1));
                    $('.collAmt2', this).text($total3.toFixed(2));

                    //$('.val3', this).val($total3);
                    var $val3 = $('.val3', this).val();// VDSRate

                    mult1 += $tds;//Total Price
                    mult2 += $vds;//Total Weight ctl00_BodyContent_ltrWeight
                    mult3 += parseFloat($val3);
                    mult4 += parseFloat($val4);
                    ttlCont += $total3;
                });
                $("#ctl00_BodyContent_txtTDS").val(mult1.toFixed(2));
                $("#ctl00_BodyContent_txtVDS").val(mult2.toFixed(2));
                $("#ctl00_BodyContent_txtCollection").val(mult3.toFixed(2));
                $("#ctl00_BodyContent_txtAdjust").val(mult4.toFixed(2));
                $('#<%=txtInvTotal.ClientID%>').val(ttlCont.toFixed(2));

                calTotal();
            }



            //readonly inputs
            $('#<%=txtCollection.ClientID%>').attr('readonly', true);
            $('#<%=txtAdjust.ClientID%>').attr('readonly', true);
            $('#<%=txtReceived.ClientID%>').attr('readonly', false);
            $('#<%=txtTDS.ClientID%>').attr('readonly', true);
            $('#<%=txtVDS.ClientID%>').attr('readonly', true);
            $('#<%=txtInvTotal.ClientID%>').attr('readonly', true);

<%--            var ckbox = $('#cbAdvanceCollection');
            $('input').on('click', function () {
                if (ckbox.is(':checked')) {
                    $('#<%=txtInvTotal.ClientID%>').attr('readonly', false);
                } else {
                    $('#<%=txtInvTotal.ClientID%>').attr('readonly', true);
                }
            });
        --%>
        }
    </script>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
        <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
            <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
        </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                 Sys.Application.add_load(jScript);
                 Sys.Application.add_load(callJquery);
    </script>--%>



    <h3 class="page-title">Sales Collections Entry</h3>
    <%--Collection From Members--%>


    <div class="row">
        <div class="col-md-6">
            <div class="portlet box red">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Collection from Parties
                    </div>
                </div>
                <div class="portlet-body form">
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>
                    <div class="form-body">

                        <div class="control-group">
                            <label class="control-label">Advance Collection : </label>
                            <div class="controls">
                                <asp:CheckBox ID="cbAdvanceCollection" runat="server" OnCheckedChanged="cbStatus_CheckedChanged" value="Advance" AutoPostBack="true" />
                            </div>
                        </div>


                        <div class="control-group">
                            <asp:Label ID="Label3" runat="server" Text="Date of Collection:"></asp:Label>
                            <asp:TextBox ID="txtColDate" runat="server" OnTextChanged="txtColDate_OnTextChanged" AutoPostBack="True"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                                Enabled="True" TargetControlID="txtColDate" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <%--<asp:Button ID="btnShow" runat="server" CssClass="button" Text="Collection History"
                                        OnClick="btnShow_Click" />--%>
                        </div>


                        <div class="form-group">
                            <%--<asp:Label ID="lblDeptName" runat="server" Text="Collection From: "></asp:Label>--%>
                            <label class="control-label">Collection From: </label>
                            <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource1" CssClass="form-control select2me"
                                DataTextField="Company" DataValueField="PartyID" AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                SelectCommand="SELECT [Company], [PartyID] FROM [Party] WHERE ([Type] = @Type) ORDER BY Company">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>


                        <div class="control-group" id="divOrders" runat="server">
                            <asp:Label ID="Label4" runat="server" Text="Invoice No. : "></asp:Label>
                            <asp:ListBox ID="lvOrders" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource6"
                                DataTextField="InvDetail" DataValueField="InvNo"
                                OnSelectedIndexChanged="lbOrders_SelectedIndexChanged" SelectionMode="Multiple" Height="150px"></asp:ListBox>

                            <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                SelectCommand="SELECT [InvNo], InvNo+' Dated: '+ (CONVERT(varchar,InvDate,103) +'') AS InvDetail FROM [Sales] WHERE ([CustomerID] = @CustomerID) AND SalesMode<>'LC' AND IsActive=1 order by SaleID">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <span style="width: 100%; color: green; text-align: right" id="instruction">
                                <asp:Literal ID="ltrInv" runat="server">Press <em>Ctrl+Click</em> for multiple selection</asp:Literal>
                            </span>
                        </div>

                        <div class="table-responsive" id="tblInstruction" runat="server">


                            <asp:GridView ID="ItemGrid" runat="server" AutoGenerateColumns="False" DataKeyNames="InvoiceNo" GridLines="Vertical">

                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="5px" HeaderText="#SL">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle Width="5px"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Invoice#">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("InvoiceNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="MatuirityDate" HeaderText="Matuirity Date" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="OverdueDays" HeaderText="Over-due Days" />
                                    <asp:TemplateField HeaderText="Item Total">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceTotal" runat="server" Text='<%# Bind("InvoiceTotal") %>' Width="50px" CssClass="invTtl"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="VATAmount" HeaderText="VAT" />
                                    <asp:TemplateField HeaderText="Payable Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPayableAmount" runat="server" Text='<%# Bind("PayableAmount") %>' Width="50px" CssClass="payTtl"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="TDS Amount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="lblTDSRate" runat="server" Text='<%# Bind("TDSRate") %>' Width="80px" CssClass="val1"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="VAT Included">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkTDS" runat="server" CssClass="chkTDS" onclick="jScript();"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="TDS Amount" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTDSAmt" runat="server" CssClass="multTotal1" Text='<%# Bind("TDSAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" CssClass="xerp_absolute_centre" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="VDS Amount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="lblVDSRate" runat="server" Text='<%# Bind("VDSRate") %>' Width="80px" CssClass="val2"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="VAT Included">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkVDS" runat="server" CssClass="chkVDS" onclick="jScript();" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="VDS Amount" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVDSAmt" runat="server" CssClass="multTotal2" Text='<%# Bind("VDSAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" CssClass="xerp_absolute_centre" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Bad Debt Amount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBadDebt" runat="server" Text="0" Width="100px" CssClass="val4"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Net Payable">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetPayable" runat="server" Text='<%# Bind("NetPayable") %>' CssClass="netPay"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Previous Collection">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCollectedAmount" runat="server" Text='<%# Bind("CollectedAmount") %>' CssClass="collAmt"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Receivable">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCollectable" runat="server" CssClass="collAmt2"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Collected Amount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="lblDueAmount" runat="server" Width="100px" CssClass="val3"></asp:TextBox>
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
                            <asp:Label ID="Label14" runat="server" Text="Invoice Amount : "></asp:Label>
                            <asp:TextBox ID="txtInvAmt" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                        </div>

                        <div class="control-group" id="divVATAmount" runat="server">
                            <asp:Label ID="Label15" runat="server" Text="VAT Amount : "></asp:Label>
                            <asp:TextBox ID="txtVATAmt" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                        </div>

                        <div class="control-group" id="divPayableAmount" runat="server">
                            <asp:Label ID="Label5" runat="server" Text="Payable Amount : "></asp:Label>
                            <asp:TextBox ID="txtTotal" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                        </div>

                        <asp:Panel ID="paidpanel" runat="server" Visible="False">

                            <div class="control-group" id="divPaidAmount" runat="server">
                                <asp:Label ID="Label16" runat="server" Text="Paid Amount : "></asp:Label>
                                <asp:TextBox ID="txtPaid" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                            </div>

                            <div class="control-group" id="divDueAmount" runat="server">
                                <asp:Label ID="Label17" runat="server" Text="Due Amount : "></asp:Label>
                                <asp:TextBox ID="txtDue" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                            </div>
                        </asp:Panel>

                        <%--<div class="control-group">
                                    <asp:Label ID="Label10" runat="server" Text="TDS Rate : "></asp:Label>
                                    <asp:TextBox ID="txtTDSRate" runat="server" Text="0" Width="174px" onkeyup="calTotal()"></asp:TextBox>
                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Incl. VAT"   onclick="calTotal();"  />
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtTDSRate">
                                    </asp:FilteredTextBoxExtender>
                                </div>--%>

                        <div class="control-group" runat="server" id="divTDSAmount">
                            <asp:Label ID="Label6" runat="server" Text="TDS Amount : "></asp:Label>
                            <asp:TextBox ID="txtTDS" runat="server" Text="0" onkeyup="calTotal()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtTDS">
                            </asp:FilteredTextBoxExtender>
                        </div>

                        <%--<div class="control-group">
                                    <asp:Label ID="Label11" runat="server" Text="VDS Rate : "></asp:Label>
                                    <asp:TextBox ID="txtVDSRate" runat="server" Text="0" Width="174px"  onkeyup="calTotal()" />
                                    
                                    <asp:CheckBox ID="CheckBox2" runat="server" Text="Incl. VAT"  onclick="calTotal();" />
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtVDSRate">
                                    </asp:FilteredTextBoxExtender>
                                </div>--%>

                        <div class="control-group" runat="server" id="divVDSAmount">
                            <asp:Label ID="Label7" runat="server" Text="VDS Amount : "></asp:Label>
                            <asp:TextBox ID="txtVDS" runat="server" Text="0" onkeyup="calTotal()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtVDS">
                            </asp:FilteredTextBoxExtender>
                        </div>

                        <div class="control-group">
                            <asp:Label ID="Label1" runat="server" Text="Collection Mode: "></asp:Label>
                            <asp:DropDownList ID="ddCollMode" runat="server" Width="174px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddCollMode_SelectedIndexChanged">
                                <asp:ListItem>Cheque</asp:ListItem>
                                <asp:ListItem>Cash</asp:ListItem>
                                <asp:ListItem>Other</asp:ListItem>
                                <%--<asp:ListItem>TT</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>

                        <%--                        <div class="control-group">
                             <asp:Label ID="LabelAmount" runat="server" Text="Amount : "></asp:Label>
                             <asp:TextBox ID="TextBoxAmount" runat="server"></asp:TextBox>
                        </div>--%>

                        <asp:Panel ID="otherPnlId" runat="server" Visible="false">
                            <div class="form-group">
                                <label class="control-label"> A/C Head :  </label>
                                <asp:DropDownList ID="ddAccHead" runat="server" DataSourceID="SqlDataSource4" DataTextField="AccountsHeadName" DataValueField="AccountsHeadID" CssClass="form-control select2me">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT AccountsHeadID, AccountsHeadName from HeadSetup WHERE (ControlAccountsID='020101' OR ControlAccountsID='020201')"></asp:SqlDataSource>
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="chqpanel" runat="server" Visible="true">
                            <div class="form-group">
                                <label class="control-label">Bank Name :  </label>
                                <asp:DropDownList ID="ddBank" runat="server" DataSourceID="SqlDataSource8" DataTextField="BankName" DataValueField="BankId" CssClass="form-control select2me">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="Select BankId, [BankName] FROM [Banks] where [Type]= 'bank'"></asp:SqlDataSource>
                                <%--<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT ACID, (Select [BankName] FROM [Banks] where [BankId]=a.BankID) +' - '+ACNo +' - '+ACName AS Bank from BankAccounts a ORDER BY [ACName]"></asp:SqlDataSource>--%>
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
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                    Enabled="True" TargetControlID="txtChqDate" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </div>
                            <div class="control-group">
                                <asp:Label ID="Label13" runat="server" Text="Custodian's Referrence : "></asp:Label>
                                <asp:TextBox ID="txtCustodian" runat="server"></asp:TextBox>
                            </div>

                            <div class="control-group">
                                <asp:Label ID="lblDateText" runat="server" Text="Cheque Pass Date:"></asp:Label>
                                <asp:TextBox ID="txtChqPassDate" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server"
                                    Enabled="True" TargetControlID="txtChqPassDate" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </div>

                            <div class="form-group">
                                <label class="control-label">Chq. Deposit Account :  </label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddChqDepositBank" runat="server" DataSourceID="SqlDataSource2" CssClass="form-control select2me"
                                        DataTextField="Bank" DataValueField="ACID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT ACID, (Select [BankName] FROM [Banks] where [BankId]=a.BankID) +' - '+ACNo +' - '+ACName AS Bank from BankAccounts a ORDER BY [ACName]"></asp:SqlDataSource>
                                </div>
                            </div>

                        </asp:Panel>

                        <div class="control-group" runat="server" id="divInvcDueAmt">
                            <asp:Label ID="Label10" runat="server" Text="Invoice Due Amount : "></asp:Label>
                            <asp:TextBox ID="txtInvTotal" runat="server" OnTextChanged="txtInvTotal_TextChanged" Text="0"></asp:TextBox>
                        </div>

                        <div class="control-group" runat="server" id="divPaidAmt">
                            <asp:Label ID="lblAmtText" runat="server" Text="Total Paid Amount : "></asp:Label>
                            <asp:TextBox ID="txtCollection" runat="server" Text="0"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtCollection">
                            </asp:FilteredTextBoxExtender>
                        </div>

                        <div class="control-group" runat="server" id="divAdjAmt">
                            <asp:Label ID="Label12" runat="server" Text="Adjust Amount (-) : "></asp:Label>
                            <asp:TextBox ID="txtAdjust" runat="server" Text="0" onkeyup="calTotal()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7"
                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789.-" TargetControlID="txtAdjust">
                            </asp:FilteredTextBoxExtender>
                        </div>

                        <div class="control-group">
                            <asp:Label ID="Label18" runat="server" Text="Act. Cheque Amount : "></asp:Label>
                            <asp:TextBox ID="txtReceived" runat="server" Text="0"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtCollection">
                            </asp:FilteredTextBoxExtender>
                        </div>

                        <div class="control-group">
                            <asp:Label ID="Label8" runat="server" Text="Collection Note/ Remark : "></asp:Label>
                            <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" Height="100px" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-actions">
                            <asp:Button ID="btnSave" runat="server" Text="Save Collection" OnClick="btnSave_Click" />
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-6 ">
            <div class="portlet box green">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Party Collection Status
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


                        <div class="control-group">
                            <label>Imatured Amount : </label>
                            <asp:Label ID="lblImitured" runat="server" Text="0.00" DataFormatString="{0:N}"></asp:Label>
                        </div>
                        <div class="control-group">
                            <label>Matured Amount : </label>
                            <asp:Label ID="lblMatured" runat="server" Text="0.00" DataFormatString="{0:N}"></asp:Label>
                        </div>
                        <div class="control-group">
                            <label>Net Balance : </label>
                            <asp:Label ID="lblCurrBalance" runat="server" Text="0.00" DataFormatString="{0:N}"></asp:Label>
                        </div>

                        <div class="control-group">
                            <label>Avg. Overdue Days : </label>
                            <asp:Label ID="lblOverdue" runat="server" Text="0.00"></asp:Label>
                        </div>
                        <div class="control-group">
                            <label>Pending Chq. Amount : </label>
                            <asp:Label ID="lblPendingChq" runat="server" Text="0.00"></asp:Label>
                        </div>
                        <legend>Pending Cheque List</legend>




                        <div class="table-responsive">

                            <asp:GridView ID="GridView1" runat="server" Width="150%" AllowSorting="True"
                                AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                                BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black"
                                GridLines="Vertical" DataKeyNames="ChequeNo" DataSourceID="SqlDataSource3">
                                <Columns>
                                    <asp:BoundField DataField="ChequeNo" HeaderText="Cheque No." SortExpression="ChequeNo" ReadOnly="True" />
                                    <asp:BoundField DataField="ChqDate" HeaderText="Chq. Date" DataFormatString="{0:dd/MM/yyyy}"
                                        SortExpression="ChqDate" />
                                    <asp:BoundField DataField="ChqStatus" HeaderText="Chq. Status" SortExpression="ChqStatus" />
                                    <asp:BoundField DataField="ChqAmt" HeaderText="Chq. Amount" SortExpression="ChqAmt" />
                                    <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                SelectCommand="SELECT [ChequeNo], [ChqDate], [ChqStatus], [ChqAmt], Remark FROM [Cheque] WHERE (([PartyID] = @PartyID) AND ([ChqStatus] &lt;&gt; @ChqStatus) AND ([ChqStatus] &lt;&gt; @ChqStatus2)) ORDER BY [ChqDate]">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddCustomer" Name="PartyID"
                                        PropertyName="SelectedValue" Type="String" />
                                    <asp:Parameter DefaultValue="Cancelled" Name="ChqStatus" Type="String" />
                                    <asp:Parameter DefaultValue="Passed" Name="ChqStatus2" Type="String" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>



                        <%--</fieldset>--%>
                    </div>
                </div>
            </div>
        </div>

    </div>

   <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>



</asp:Content>

