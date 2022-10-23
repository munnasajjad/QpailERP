<%@ Page  Language="C#" AutoEventWireup="true" CodeFile="PO-Print.aspx.cs" Inherits="app_PO_Print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Order </title>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            var param1 = new Date();
            var param2 = param1.getDate() + '/' + (param1.getMonth() + 1) + '/' + param1.getFullYear() + ' ' + param1.getHours() + ':' + param1.getMinutes() + ':' + param1.getSeconds();
            $('#lblTime').text(param2)
        })
    </script>
    <style type="text/css">
       td span {
    font-weight: bold;
    padding-left: 20px;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <%--<div class="breadcrumbs">
        	<div class="contentBox">
        		<h2 class="pageTitle">Krone</h2>
                <ul id="breadcrumb">
                	
                </ul>
            </div>    
        </div>--%>

        <div class="contentBox">
            <!--Static content-->
            <div class="staticBox row services_infoA">


                <script language="javascript" type="text/javascript">
                    function callPrint() {
                        w = window.open("printwelcome.htm", "", "location=0,status=1,scrollbars=1,width=1000,height=1100");
                        w.moveTo(0, 0);
                    }
                </script>
                <table style="width: 100%" align="center" bgcolor="white">

                    <tr>
                        <td id="printT" align="center">
                            <table id="TABLE1" bgcolor="white" cellpadding="0" cellspacing="0" class="infoTable"
                                onclick="return TABLE1_onclick()" style="font-size: small; border-right: #c0c0c0 5px solid; border-top: #c0c0c0 5px solid; border-left: #c0c0c0 5px solid; width: 89%; border-bottom: #c0c0c0 5px solid;">
                                <tr style="vertical-align: middle;">
                                    <td colspan="6" width="100%" align="center" style="text-align: center;">
                                        <!-- Logo -->
                                        <div id="logo">
                                            <h1>Purchase Order</h1>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="6" style="width: 25%;">
                                        <span><span style="font-size: 12pt"><strong>
                                            <input type="hidden" name="ctl00$ContentPlaceHolder1$HiddenField1" id="ctl00_ContentPlaceHolder1_HiddenField1" value="1218" />
                                            <br />
                                            <span style="text-decoration: underline">Order#<asp:Literal ID="ltrOrderNo" runat="server" ></asp:Literal>
                                            
                                            <asp:Literal ID="ltrDetail" runat="server"></asp:Literal></span></strong>
                                            <br />
                                            <asp:Literal ID="ltrDeliveryStatus" runat="server" ></asp:Literal>
                                            
                                        </span></span>
                                    </td>
                                </tr>
                                <%--<tr style="padding-left: 10px; height: 25px;">
                        <td align="left" class="lblinfo" colspan="6" style="width: 25%">
                            &nbsp;Dear <asp:Label ID="lblName" runat="server" Text=""></asp:Label>,
                            <br />
                        </td>
                    </tr>
                    <tr style="padding-left: 10px; height: 25px;">
                        <td align="left" class="lblinfo" colspan="6" style="width: 25%; padding-left: 70px;">
                            Thanks for keeping in touch and earning with vertex Ltd. 
                            <br />
                        </td>
                    </tr>
                    <tr style="padding-left: 10px; height: 25px;">
                        <td align="left" class="lblinfo" colspan="6" style="width: 25%; padding-left: 70px;">
                            Below is the payment detail as per withdrawal request from you, if any detail is incorrect, please inform
                            us as soon as possible.
                            <br />
                        </td>
                    </tr>--%>
                                <tr style="padding-left: 10px; height: 25px;">
                                    <td align="left" class="lblinfo" colspan="6" style="width: 25%; padding-left: 70px;">
                                        <%--Below is Your details are:--%>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="6">
                                        <table width="80%">
                                            <tr>
                                                <td align="left" style="width: 20%">Customer Name
                                                </td>
                                                <td class="lblinfo" style="width: 3px; height: 21px">:
                                                </td>
                                                <td align="left" style="width: 30%">
                                                    <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 20%">Order Amount
                                                </td>
                                                <td class="lblinfo" style="font-size: 12pt; width: 4px; height: 21px">:
                                                </td>
                                                <td align="left" class="lblinfo" colspan="1" style="width: 30%;">
                                                    <asp:Label ID="lblPoAmount" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 20%">PO Date
                                                </td>
                                                <td class="lblinfo" style="width: 3px; height: 21px">:
                                                </td>
                                                <td align="left" class="lblinfo" style="width: 30%;">
                                                    <asp:Label ID="lblPoDate" runat="server"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 20%">
                                                    <asp:Literal ID="vatPercent" runat="server" />% VAT Amount
                                                </td>
                                                <td class="lblinfo" style="font-size: 12pt; width: 4px; height: 21px">:
                                                </td>
                                                <td align="left" class="lblinfo" colspan="1" style="width: 30%;">
                                                    <asp:Label ID="lblVatAmount" runat="server"></asp:Label>.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="lblinfo" style="width: 20%">Delivery Date (Asked)
                                                </td>
                                                <td class="lblinfo" style="width: 3px; height: 21px">:
                                                </td>
                                                <td align="left" class="lblinfo" style="width: 30%;">
                                                    <asp:Label ID="lblDelDate" runat="server"></asp:Label>
                                                </td>
                                                <td align="left" class="lblinfo" style="width: 20%">Net Payable Amount
                                                </td>
                                                <td class="lblinfo" style="font-size: 12pt; width: 4px; height: 21px">:
                                                </td>
                                                <td align="left" class="lblinfo" colspan="1" style="width: 30%;">
                                                    <asp:Label ID="lblNetAmount" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" style="width: 25%;">&nbsp; &nbsp;
                                    </td>
                                </tr>
                                <tr style="padding-left: 10px; height: 25px;">
                                    <td align="left" class="lblinfo" colspan="6" style=" padding-left: 70px;">
                                        <i>Order Item Details: </i>
                                            
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" CssClass="table">
                                            <Columns>
                                                
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                                <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" />
                                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
                                                <asp:BoundField DataField="UnitType" HeaderText="Unit Type" SortExpression="UnitType" />
                                                <asp:BoundField DataField="UnitCost" HeaderText="Unit Cost" SortExpression="UnitCost" />
                                                <asp:BoundField DataField="UnitWeight" HeaderText="Unit Weight" SortExpression="UnitWeight" />
                                                <asp:BoundField DataField="ItemTotal" HeaderText="Item Total" SortExpression="ItemTotal" />
                                                <asp:BoundField DataField="TotalWeight" HeaderText="Total Weight" SortExpression="TotalWeight" />
                                                <asp:BoundField DataField="DeliveredQty" HeaderText="Delivered Qty." SortExpression="DeliveredQty" />
                                                <asp:BoundField DataField="QtyBalance" HeaderText="Qty. Balance" SortExpression="QtyBalance" />

                                                <asp:TemplateField HeaderText="Delivery Invoice#" SortExpression="Name">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="Label1" runat="server" Text='<%# Bind("DeliveryInvoice") %>' NavigateUrl='<%# Bind("link") %>' Target="_blank"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [ProductName], [UnitCost], [UnitWeight], [Quantity], [ItemTotal], [TotalWeight], [UnitType], [DeliveredQty], [QtyBalance], [DeliveryInvoice], (Select DefaultLink from  Settings_Project where sid=1)+'XerpReports/frmInvoice.aspx?inv='+DeliveryInvoice as link FROM [OrderDetails] WHERE ([OrderID]= (Select OrderID from Orders where OrderSl = @OrderID))">
                                            <SelectParameters>
                                                <asp:QueryStringParameter Name="OrderID" QueryStringField="poid" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        
                                        
                                        <br />
                                    </td>
                                </tr>
                                <tr style="">
                                    <td align="left" class="lblinfo" colspan="6" style="">
                                       
                                        <br />
                                         <i>Order Delivery History: </i>
                                            
                                        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource3" CssClass="table" DataKeyNames="InvNo">
                                            <Columns>
                                                
                                                <asp:BoundField DataField="InvNo" HeaderText="Inv.No." SortExpression="InvNo" ReadOnly="True" HeaderStyle-Width="7%" />
                                                <asp:BoundField DataField="InvDate" HeaderText="Delivery Date" SortExpression="PONo" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="PONo" HeaderText="PO No." SortExpression="PONo"  HeaderStyle-Width="10%"  />
                                                <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" HeaderStyle-Width="25%" />
                                                <asp:BoundField DataField="qty" HeaderText="Quantity" SortExpression="Quantity" />
                                                <%--<asp:BoundField DataField="UnitType" HeaderText="Unit Type" SortExpression="UnitType" />--%>
                                                <asp:BoundField DataField="UnitCost" HeaderText="Unit Cost" SortExpression="UnitCost" />
                                                <asp:BoundField DataField="UnitWeight" HeaderText="Unit Weight" SortExpression="UnitWeight" />
                                                
                                                <asp:BoundField DataField="ItemTotal" HeaderText="Total Amt" SortExpression="ItemTotal" />
                                                <asp:BoundField DataField="VAT" HeaderText="VAT Amt" SortExpression="VAT" />
                                                <asp:BoundField DataField="NetAmount" HeaderText="Net Amount" SortExpression="NetAmount" />
                                                <asp:BoundField DataField="TotalWeight" HeaderText="Total Weight" SortExpression="TotalWeight" />
                                                <%--<asp:BoundField DataField="TotalCarton" HeaderText="Total Carton" SortExpression="TotalCarton" />--%>
                                                <asp:BoundField DataField="QtyPerCarton" HeaderText="Qty./ Carton" SortExpression="QtyPerCarton" />
                                            </Columns>
                                            <EmptyDataTemplate><i>No delivery Record Found!</i></EmptyDataTemplate>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT Sales.InvNo, Sales.InvDate, Sales.PONo, SaleDetails.ProductName, Convert(varchar, SaleDetails.Quantity)+' '+ SaleDetails.UnitType as qty, SaleDetails.UnitCost, SaleDetails.UnitWeight, SaleDetails.NetAmount, SaleDetails.TotalWeight, SaleDetails.VAT, SaleDetails.VatPercent, SaleDetails.ItemTotal, SaleDetails.TotalCarton, SaleDetails.QtyPerCarton
                                             FROM SaleDetails INNER JOIN Sales ON SaleDetails.InvNo = Sales.InvNo WHERE (Sales.PONo LIKE '%'+@PONo+'%') ORDER BY SaleDetails.EntryDate">
                                             <SelectParameters>
                                                <asp:ControlParameter Name="PONo" ControlID="ltrOrderNo" PropertyName="Text" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:Label ID="lblPoNo" runat="server" Text=""></asp:Label>
                                        <br />
                                    </td>
                                </tr>
                                <tr style="padding-left: 10px;">
                                    <td align="left" colspan="4" style="width: 50%; padding-left: 20px;">&nbsp;
                        <br />
                                        <br />
                                        <hr style="width: 160px; float: left;" />
                                        <br />
                                        <i>Receiver Signature</i><br />
                                        <asp:Label ID="lblsig" runat="server"></asp:Label><br />

                                    </td>
                                    <td align="right" colspan="2" style="padding-right: 20px;">
                                        <br />
                                        <br />
                                        <hr style="width: 160px; float: right;" />
                                        <br />
                                        <i>Authorized Signature</i><br />

                                    </td>
                                </tr>
                                <tr>
                                    <td id="tdButton" align="center" colspan="6" valign="middle" style="width: 25%">Print Date:
                                        <script type="text/javascript">                                            document.getElementById('lblTime').value = (new Date()).format('dd/MM/yyyy HH:mm:ss');</script>
                                        <asp:Label ID="lblTime" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </td>
                    </tr>
                </table>




            </div>
        </div>

    </form>
</body>
</html>
