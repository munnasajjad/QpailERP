<%@ Page Title="Collection from LC" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Collection-LC.aspx.cs" Inherits="app_Collection_LC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #ctl00_BodyContent_chkInvoices {
            width: 65%;
        }

            #ctl00_BodyContent_chkInvoices label {
                width: 79%;
            }

        span#ctl00_BodyContent_lblChecked {
            width: 100%;
            text-align: right;
            padding-right: 30px;
            color: green;
        }
        .portlet-body table td {
    padding: 0!important;
}
    </style>



    <script>

        $(window).load(function () {
            calTotal();
        });

        function calTotal() {
            var due = $('#<%=txtDue.ClientID%>').val();
            var adjust = $('#<%=txtAdjust.ClientID%>').val();
            var amount2 = parseFloat(due) + parseFloat(adjust * 1);
            $('#<%=txtCollection.ClientID%>').val(amount2.toString());
        }
    </script>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">


    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

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
                Sys.Application.add_load(callJquery);
            </script>





            <h3 class="page-title">Collection from LLC</h3>
            <%--Collection From Members--%>

            <div class="row">
                <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>LLC Payment Received
                            </div>
                        </div>
                        <div class="portlet-body form">

            <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>

                            <div class="form-body">
                                <div class="control-group">
                                    <asp:Label ID="Label3" runat="server" Text="Date of Collection:"></asp:Label>
                                    <asp:TextBox ID="txtColDate" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                                        Enabled="True" TargetControlID="txtColDate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <%--<asp:Button ID="btnShow" runat="server" CssClass="button" Text="Collection History"
                                        OnClick="btnShow_Click" />--%>
                                </div>


                                <div class="control-group">
                                    <asp:Label ID="lblDeptName" runat="server" Text="Collection From: "></asp:Label>
                                    <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource1"
                                        DataTextField="Company" DataValueField="PartyID" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Company], [PartyID] FROM [Party] WHERE ([Type] = @Type) ORDER BY Company">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>


                                <div class="control-group">
                                    <asp:Label ID="Label4" runat="server" Text="LLC No. : "></asp:Label>
                                    <asp:DropDownList ID="lvOrders" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource4"
                                        DataTextField="InvDetail" DataValueField="LcNo"
                                        OnSelectedIndexChanged="lbOrders_SelectedIndexChanged">
                                    </asp:DropDownList>

                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [LcNo], LcNo+'  LC Date: '+ (CONVERT(varchar,LcDate,103) +'') AS InvDetail  FROM [PI] WHERE LcNo IN (SELECT OrderID  FROM [Orders] WHERE CustomerName=  @CustomerName) AND ([LcNo] <> '') AND IsActive=1">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddCustomer" Name="CustomerName" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>


                                <div class="control-group">
                                    <asp:Label ID="Label9" runat="server" Text="Invoice No. : "></asp:Label>
                                    <asp:CheckBoxList ID="chkInvoices" runat="server" DataSourceID="SqlDataSource5" DataTextField="InvDetail" DataValueField="InvNo"
                                        AutoPostBack="True" OnSelectedIndexChanged="chkInvoices_OnSelectedIndexChanged">
                                    </asp:CheckBoxList>

                                    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [InvNo], InvNo+'  Date: '+ CONVERT(varchar,CONVERT(date, InvDate)) AS InvDetail  FROM [Sales] WHERE ([PONo] = @PONo)">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lvOrders" Name="PONo" PropertyName="SelectedValue" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                    <asp:Label ID="lblChecked" runat="server" Text="" ></asp:Label>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label6" runat="server" Text="PI No. : "></asp:Label>
                                    <asp:TextBox ID="txtLLCNo" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label11" runat="server" Text="PI Date : "></asp:Label>
                                    <asp:TextBox ID="txtPIDate" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label7" runat="server" Text="LLC Date : "></asp:Label>
                                    <asp:TextBox ID="txtLLCDate" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label10" runat="server" Text="LLC Expire Date : "></asp:Label>
                                    <asp:TextBox ID="txtLcExpDate" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label12" runat="server" Text="Issuer Bank : "></asp:Label>
                                    <asp:TextBox ID="txtIssuerBank" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label5" runat="server" Text="LC Amount : "></asp:Label>
                                    <asp:TextBox ID="txtTotal" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label1" runat="server" Text="Item Delivered Amount : "></asp:Label>
                                    <asp:TextBox ID="txtDeliverd" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label14" runat="server" Text="Paid Amount : "></asp:Label>
                                    <asp:TextBox ID="txtPrePaid" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label2" runat="server" Text="Due Amount : "></asp:Label>
                                    <asp:TextBox ID="txtDue" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label13" runat="server" Text="Advising Bank Account :  : "></asp:Label>
                                    <asp:DropDownList ID="ddBank" runat="server" DataSourceID="SqlDataSource2"
                                        DataTextField="Bank" DataValueField="ACID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT ACID, (Select [BankName] FROM [Banks] where [BankId]=a.BankID) +' - '+ACNo +' - '+ACName AS Bank from BankAccounts a ORDER BY [ACName]"></asp:SqlDataSource>

                                </div>


                                <div class="control-group">
                                    <asp:Label ID="Label15" runat="server" Text="Adjust Amount (+/-) : "></asp:Label>
                                    <asp:TextBox ID="txtAdjust" runat="server" Text="0" ></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789.-" TargetControlID="txtAdjust">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="lblAmtText" runat="server" Text="Received Amount : "></asp:Label>
                                    <asp:TextBox ID="txtCollection" runat="server" Text="0"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789.-" TargetControlID="txtCollection">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label8" runat="server" Text="Collection Note/ Remark  : "></asp:Label>
                                    <asp:TextBox ID="txtRemark" runat="server"></asp:TextBox>
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
                                <i class="fa fa-reorder"></i>LLC Collection History
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
                                    <label>Item Order Quantity : </label>
                                    <asp:Label ID="lblMatured" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <%--<div class="control-group">
                                    <label>Item Delivered Quantity : </label>
                                    <asp:Label ID="lblOverdue" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="control-group">
                                    <label>LC Amount : </label>
                                    <asp:Label ID="lblImitured" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="control-group">
                                    <label>Collected Amount : </label>
                                    <asp:Label ID="lblPendingChq" runat="server" Text="0.00"></asp:Label>
                                </div>--%>
                                 <div class="control-group">
                                    <label>Adjust Amount : </label>
                                    <asp:Label ID="lblAdjustAmt" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <%--<div class="control-group">
                                    <label>Due Amount : </label>
                                    <asp:Label ID="lblCurrBalance" runat="server" Text="0.00"></asp:Label>
                                </div>--%>
                                
                                <legend>Previous Payment History</legend>
                                
                            <div class="table-responsive">

                                <asp:GridView ID="GridView1" runat="server" Width="100%" AllowSorting="True"
                                    AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                                    BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black"
                                    GridLines="Vertical" DataSourceID="SqlDataSource3" DataKeyNames="CollectionNo">
                                    <Columns>
                                        <asp:BoundField DataField="CollectionNo" HeaderText="CollectionNo"
                                            SortExpression="CollectionNo" ReadOnly="True" />
                                        <asp:BoundField DataField="CollectionDate" HeaderText="CollectionDate" DataFormatString="{0:d}"
                                            SortExpression="CollectionDate" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" SortExpression="CustomerName" />
                                        <asp:BoundField DataField="SalesInvoiceNo" HeaderText="SalesInvoiceNo" SortExpression="SalesInvoiceNo" />
                                        <asp:BoundField DataField="InvoiceAmt" HeaderText="InvoiceAmt" SortExpression="InvoiceAmt" />
                                        <asp:BoundField DataField="TotalAmt" HeaderText="TotalAmt" SortExpression="TotalAmt" />
                                        <asp:BoundField DataField="AdjustmentAmt" HeaderText="AdjustmentAmt" SortExpression="AdjustmentAmt" />
                                        <asp:BoundField DataField="CollectedAmt" HeaderText="CollectedAmt" SortExpression="CollectedAmt" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [CollectionNo], [CollectionDate], [CustomerName], [SalesInvoiceNo], [InvoiceAmt], [TotalAmt], [AdjustmentAmt], [CollectedAmt] FROM [Collection] WHERE (([CollType] = @CollType) AND ([SalesInvoiceNo] = @SalesInvoiceNo))">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="LLC" Name="CollType" Type="String" />
                                        <asp:ControlParameter ControlID="lvOrders" Name="SalesInvoiceNo"
                                            PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>


                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>

