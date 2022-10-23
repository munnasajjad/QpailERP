<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Voucher.aspx.cs" Inherits="Application_Reports_Voucher" %>

<!DOCTYPE html">
<html>
<head id="Head1" runat="server">
    <title></title>

</head>
<body style="font-family: Calibri">
    <form id="form1" runat="server">
        <asp:PlaceHolder ID="PlaceholderPdf" runat="server"></asp:PlaceHolder>

        <div border="0" cellspacing="1" cellpadding="1" width="100%" style="font-family: Calibri, Arial, Tahoma; font-size: 10px; border-color: Gray;">
            <div>
                <div width="70%">

                    <strong style="font-size: 16px;">
                        <asp:Label ID="lblName" runat="server" Text=""></asp:Label></strong><br />
                    <asp:Label ID="lblArress" runat="server" Text=""></asp:Label>
                    <br />
                    &nbsp;
                </div>

                <div width="30%">
                    <p style="font-size: 20px; font-weight: bold; padding: 20px; display: block; margin: 20px 20px; text-align: center;">
                        Voucher
                    </p>

                </div>
            </div>
            <div>
                <div></div>
                <div></div>
            </div>
            <div>
                <div>
                    <p>
                        <strong>Voucher ID :</strong>
                        <asp:HyperLink ID="lblInv" runat="server" Text=""></asp:HyperLink>
                        <%--<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="www.google.com">HyperLink</asp:HyperLink>--%>
                    </p>
                </div>
                <div>
                    <p>
                        <strong>Voucher Date : </strong>
                        <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
                    </p>
                </div>
            </div>
            <div>
                <div>
                    <p>
                        <strong>Transaction Particular : </strong>
                        <asp:Label ID="lblDesc" runat="server" Text=""></asp:Label>
                    </p>
                </div>
                <div>
                    <p>
                        <strong>Approve Date : </strong>
                        <asp:Label ID="lblDDate" runat="server" Text=""></asp:Label>
                    </p>
                </div>
            </div>
            <div>
                <div width="100%" colspan="2">
                    <p>
                        &nbsp;
                        <br />
                        &nbsp;
                    </p>
                </div>
            </div>
            <div>
                <div width="100%" colspan="2">

                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                        DataSourceID="SqlDataSource1" BackColor="White" BorderColor="#CCCCCC" VerticalAlign="Middle"
                        BorderStyle="None" BorderWidth="0px" CellPadding="2" ForeColor="Black"
                        GridLines="Horizontal">
                        <Columns>
                            <asp:BoundField DataField="AccountsHeadName" HeaderText="Acc Head Name" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="500px" HeaderStyle-Font-Bold="true" HeaderStyle-BackColor="Gray"
                                SortExpression="AccountsHeadName" />
                            <asp:BoundField DataField="VoucherRowDescription" HeaderText="Entry Description" ItemStyle-VerticalAlign="Middle" HeaderStyle-Font-Bold="true" HeaderStyle-BackColor="Gray"
                                SortExpression="VoucherRowDescription" ItemStyle-Width="50%" />

                            <asp:BoundField DataField="VoucherDR" HeaderText="Dr." ItemStyle-VerticalAlign="Middle" HeaderStyle-Font-Bold="true" HeaderStyle-BackColor="Gray" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderStyle-VerticalAlign="Middle" DataFormatString="{0:N}"
                                SortExpression="VoucherDR" />
                            <asp:BoundField DataField="VoucherCR" HeaderText="Cr." ItemStyle-VerticalAlign="Middle" HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderStyle-VerticalAlign="Middle"  DataFormatString="{0:N}" SortExpression="VoucherCR" />
                            <%--<asp:TemplateField HeaderText="Cr.">
                                <ItemTemplate>
                                     <asp:Label runat="server" ID="lblCr" Text='<%#Bind("VoucherCR") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                        <PagerStyle BackColor="White" ForeColor="Black" VerticalAlign="Middle" HorizontalAlign="Right" />
                        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" VerticalAlign="Middle" ForeColor="White" />
                        <HeaderStyle />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                        SelectCommand="SELECT [AccountsHeadName], [VoucherRowDescription], [VoucherDR], [VoucherCR] FROM [VoucherDetails] WHERE ([VoucherNo] = @VoucherNo)">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="VoucherNo" QueryStringField="inv" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                </div>
            </div>
            <div>
                <div width="70%">
                    <p align="right">
                        <b>Transaction Amount : </b>
                        <asp:Label ID="lblTtlAmount" runat="server" Text="" />
                        Tk.
                    </p>
                </div>

                <div width="85%">
                    <p align="right">
                        Entry By :
                    <asp:Label ID="lblEntryBy" runat="server" Text=""></asp:Label>
                    </p>
                </div>

            </div>
            <div>
                <div width="70%">
                    <p align="right">Approved By :
                        <asp:Label ID="lblAppBy" runat="server" Text=""></asp:Label></p>
                </div>

            </div>

            <div>
                <div width="30%">
                    <p>
                        --------------------------------<br />
                        Authorized Signature<br />
                        <i>
                            <asp:Label ID="lblPDate" runat="server" Text="" /></i>
                    </p>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
