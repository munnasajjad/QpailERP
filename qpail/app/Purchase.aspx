<%@ Page Language="C#" MasterPageFile="~/app/MasterPage.master" MaintainScrollPositionOnPostback="true"
    AutoEventWireup="true" CodeFile="Purchase.aspx.cs" Inherits="app_Purchase" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function calTtl() {
            var bankExRate = $('#<%=txtQuantity.ClientID%>').val();
            var cusExRate = $('#<%=txtRate.ClientID%>').val();
            var cfrBDT = parseFloat(bankExRate) * parseFloat(cusExRate) * 1;
            $('#<%=txtSubTotal.ClientID%>').val(cfrBDT.toString());

            var disct = $('#<%=txtItemDisc.ClientID%>').val();
            var vat = $('#<%=txtItemVAT.ClientID%>').val();
            cfrBDT = cfrBDT - (disct * 1) + (vat * 1);
            $('#<%=txtAmt.ClientID%>').val(cfrBDT.toString());
        }

        function calGTtl() {
            var txtPDiscount = $('#<%=txtPDiscount.ClientID%>').val();
            var txtVat = $('#<%=txtVat.ClientID%>').val();
            var txtPtotal = $('#<%=lblItemAmount.ClientID%>').html();
            var g = $('#<%=lblOthersTotal.ClientID%>').html();

            var gTtl = parseFloat(txtPtotal) - parseFloat(txtPDiscount) + parseFloat(txtVat) + parseFloat(g);
            $('#<%=txtGTotal.ClientID%>').val(gTtl.toString());
        }
        function calQtyTtl() {
            var bankExRate = $('#<%=txtWarrenty.ClientID%>').val();
            var cusExRate = $('#<%=txtSerial.ClientID%>').val();
            var cfrBDT = parseFloat(bankExRate) * parseFloat(cusExRate) * 1;
            $('#<%=txtQuantity.ClientID%>').val(cfrBDT.toString());

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">


    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <%--Triggers must be needed for file upload--%>
    <asp:UpdatePanel ID="pnl" runat="server">

        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
                Sys.Application.add_load(validate);
            </script>



            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">
                        <asp:Literal ID="ltrHead" runat="server" Text="Purchase Entry"></asp:Literal></h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Purchase Details
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

                                <span style="color: Maroon;">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblErrLoad" runat="server"></asp:Label>
                                    <asp:Label ID="lblProject" runat="server" Visible="false" />
                                    <asp:Label ID="lblInvoice" runat="server" Text="" Visible="false" />
                                </span>

                                <div id="divinvoice" runat="server" class="control-group hidden">
                                    <asp:Label ID="Label3" runat="server" Text="Invoice to Edit :  " Font-Bold="True"></asp:Label>
                                    <asp:DropDownList ID="ddInvoice" runat="server" AutoPostBack="True"
                                        DataSourceID="SqlDataSource10" DataTextField="Supplier"
                                        DataValueField="InvNo" OnSelectedIndexChanged="ddInvoice_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource10" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [InvNo], InvNo+': '+(CONVERT(varchar,OrderDate,103) +' - ')+SupplierName as Supplier FROM [Purchase] ORDER BY [PID] desc"></asp:SqlDataSource>
                                </div>


                                <div class="control-group">
                                    <label class="control-label">For Company/Dept : </label>
                                    <asp:TextBox ID="txtFor" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Purchase Date : </label>
                                    <asp:TextBox ID="txtOrderDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtOrderDate">
                                    </asp:CalendarExtender>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Supply Source : </label>
                                    <asp:DropDownList ID="ddType" runat="server">
                                        <asp:ListItem>Local Purchase</asp:ListItem>
                                        <asp:ListItem>LC</asp:ListItem>
                                        <asp:ListItem>TT</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">LC/TT/Invoice No : </label>
                                    <asp:TextBox ID="txtBill" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">LC/TT/Invoice Date : </label>
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtDate">
                                    </asp:CalendarExtender>
                                </div>



                                <%--<div id="Div2" runat="server" class="control-group">
                                    <asp:Label ID="Label14" runat="server" Text="Supplier Category :  "></asp:Label>
                                    <asp:DropDownList ID="ddSuppCategory" runat="server" AutoPostBack="True"
                                        DataSourceID="SqlDataSource9" DataTextField="BrandName"
                                        DataValueField="BrandID" OnSelectedIndexChanged="DropDownList2_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource9" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], BrandName FROM [RefItems] ORDER BY [BrandName]"></asp:SqlDataSource>
                                </div>--%>

                                <div class="form-group">
                                    <label class="control-label">Supplier/Purchase From:</label>
                                    <asp:Label ID="vBalance" CssClass="help-block" runat="server" Text="" Visible="false"></asp:Label>
                                    <asp:DropDownList ID="ddVendor" runat="server" DataSourceID="SqlDataSource2" CssClass="select2me" Width="70%" DataTextField="Company"
                                        DataValueField="PartyID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type)  ORDER BY [Company]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="vendor" Name="Type" Type="String" />
                                            <%--<asp:ControlParameter ControlID="ddSuppCategory" Name="Category" PropertyName="SelectedValue" />--%>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Delivery Challan No : </label>
                                    <asp:TextBox ID="txtChallanNo" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Delivery Challan Date : </label>
                                    <asp:TextBox ID="txtChallanDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtChallanDate">
                                    </asp:CalendarExtender>
                                </div>

                                <a name="ItemDetails"></a>
                                <legend style="margin-bottom: 6px;">Item Details :</legend>
                                <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>


                                <div class="control-group hidden">
                                    <asp:Label ID="Label9" runat="server" Text="Purpose :" Font-Bold="True"></asp:Label>
                                    <asp:DropDownList ID="ddPurpose" runat="server" DataSourceID="SqlDataSource1mm"
                                        DataTextField="Purpose" DataValueField="pid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1mm" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [pid], [Purpose] FROM [Purpose] order by Purpose">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Group : </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddGroup" runat="server" DataSourceID="SqlDataSource3" AutoPostBack="true"
                                            DataTextField="GroupName" DataValueField="GroupSrNo" OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] WHERE GroupSrNo<>2 AND GroupSrNo<>'11' ORDER BY [GroupSrNo]"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Sub-group :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddSubGrp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddSubGrp_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Grade :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddGrade" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddGrade_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Category :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Item :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                        </asp:DropDownList>

                                        <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>
                                        <span style="width: 70%; color: green; float: right">
                                            <asp:Literal ID="ltrLastInfo" runat="server" EnableViewState="False">Recent Purchase Info: </asp:Literal>
                                        </span>
                                    </div>
                                </div>

                                <asp:Panel ID="pnlSpec" runat="server">

                                    <div class="control-group">
                                        <label class="control-label">
                                            Specification:<br />
                                            <asp:LinkButton ID="lbSpec" runat="server" OnClick="lbSpec_OnClick">New</asp:LinkButton>
                                            |
                                                        <asp:LinkButton ID="lbFilter" runat="server" OnClick="lbFilter_OnClick">Show-all</asp:LinkButton>
                                        </label>
                                        <asp:DropDownList ID="ddSpec" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddSpec_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource14" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [id], [spec] FROM [Specifications] ORDER BY [spec]"></asp:SqlDataSource>

                                        <asp:TextBox ID="txtSpec" runat="server" Visible="False" />
                                    </div>
                                </asp:Panel>


                                <div id="pkSizeField" runat="server" class="control-group">
                                    <label class="control-label">Pack Size :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddSize" runat="server" DataSourceID="SqlDataSource11"
                                            DataTextField="BrandName" DataValueField="BrandID">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT BrandID, BrandName FROM [Brands] ORDER BY [DisplaySl]"></asp:SqlDataSource>

                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">
                                            <asp:Literal ID="Literal3" runat="server" Text=" Quantity (Pcs) : " />
                                        </label>
                                        <asp:TextBox ID="txtWeight" runat="server" />
                                    </div>
                                </div>



                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="ltrSerial" runat="server" Text="Serial No : " />
                                    </label>
                                    <asp:TextBox ID="txtSerial" runat="server" onkeyup="calQtyTtl()" />
                                </div>

                                <asp:Panel ID="PanelMachine" runat="server" Visible="False">

                                    <div class="control-group">
                                        <label class="control-label">
                                            <asp:Literal ID="Literal4" runat="server" Text="Model No. : " />
                                        </label>
                                        <asp:TextBox ID="txtModel" runat="server" />
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">
                                            <asp:Literal ID="Literal5" runat="server" Text="Specification : " />
                                        </label>
                                        <asp:TextBox ID="txtSpecification" runat="server" />
                                    </div>

                                </asp:Panel>

                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="ltrWarrenty" runat="server" Text="Warrentry : " />
                                    </label>
                                    <asp:TextBox ID="txtWarrenty" runat="server" onkeyup="calQtyTtl()" />
                                </div>



                                <div class="form-group">
                                    <label class="control-label">
                                        <asp:Literal ID="Literal1" runat="server" Text="Manufacturer" />:
                                    </label>
                                    <%-- <asp:TextBox ID="txtManufacturer" runat="server" />--%>
                                    <asp:DropDownList ID="ddManufacturer" runat="server" DataSourceID="SqlDataSource13"
                                        CssClass="select2me" Width="70%" DataTextField="Company" DataValueField="PartyID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource13" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'supplier')  ORDER BY [Company]"></asp:SqlDataSource>

                                </div>

                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="Literal2" runat="server" Text="Country of Origin" />
                                        :
                                    </label>
                                    <asp:TextBox ID="txtCountry" runat="server" />
                                </div>
                                <%--<div class="control-group">
                                    <label class="control-label">Ref./Spec</label>
                                    <asp:TextBox ID="txtRef" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>--%>

                                <div id="SectionField" runat="server" class="control-group hidden">
                                    <label class="control-label">For Section :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddSection" runat="server" DataSourceID="SqlDataSource22"
                                            DataTextField="SName" DataValueField="SID">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource22" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [SID], [SName] FROM [Sections] WHERE ([DepartmentID] = @DepartmentID) ORDER BY [SName]">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="5" Name="DepartmentID" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>


                                <div class="control-group">
                                    <label class="control-label">Quantity (<asp:Literal ID="ltrUnitType" runat="server" />) : </label>
                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" onkeyup="calTtl()"></asp:TextBox>

                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtQuantity">
                                    </asp:FilteredTextBoxExtender>

                                </div>

                                <div class="control-group">
                                    <label class="control-label">Rate :  </label>
                                    <asp:TextBox ID="txtRate" runat="server" CssClass="form-control" onkeyup="calTtl()" Width="35%"></asp:TextBox>
                                    <asp:TextBox ID="txtSubTotal" runat="server" CssClass="form-control" ReadOnly="True" Width="35%"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtRate">
                                    </asp:FilteredTextBoxExtender>

                                </div>

                                <div class="control-group">
                                    <label class="control-label">Discount on Item (TK) : </label>
                                    <asp:TextBox ID="txtItemDisc" runat="server" Text="0" CssClass="form-control" onkeyup="calTtl()"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">VAT on Item (TK) : </label>
                                    <asp:TextBox ID="txtItemVAT" runat="server" Text="0" CssClass="form-control" onkeyup="calTtl()"></asp:TextBox>
                                </div>



                                <div class="control-group">
                                    <label class="control-label">Amount(Tk) : </label>
                                    <asp:TextBox ID="txtAmt" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>

                                </div>



                                <div class="control-group">
                                    <%--<label class="control-label">Stock Type : </label>--%>
                                    <asp:DropDownList ID="ddStockType" runat="server" CssClass="hidden">
                                        <asp:ListItem Value="Raw">Raw Stock</asp:ListItem>
                                        <asp:ListItem Value="Fixed">Fixed Assets</asp:ListItem>
                                        <asp:ListItem Value="Temporary">Temporary Item</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add to grid" OnClick="btnAdd_Click" />
                                </div>


                                <div style="clear: both"></div>


                                <div class="table-responsive">
                                    <asp:Label ID="lblEid" runat="server" Text="" Visible="False"></asp:Label>

                                    <asp:GridView ID="ItemGrid" runat="server" AutoGenerateColumns="False" CssClass="table-striped table-hover table-bordered" Width="200%"
                                        BackColor="Red" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="1" ForeColor="Black" RowStyle-BackColor="#A1DCF2"
                                        HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" DataKeyNames="Id" GridLines="Vertical" SelectedRowStyle-BackColor="LightBlue"
                                        OnRowDataBound="ItemGrid_RowDataBound" OnRowDeleting="ItemGrid_RowDeleting" OnSelectedIndexChanged="ItemGrid_SelectedIndexChanged">

                                        <Columns>
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
                                            </asp:TemplateField>


                                            <asp:TemplateField ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                    <asp:Label ID="lblEntryId" runat="server" CssClass="hidden" Text='<%# Bind("Id") %>'></asp:Label>
                                                </ItemTemplate>

                                                <ItemStyle Width="20px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Purpose" SortExpression="EntryID">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1h" runat="server" Text='<%# Bind("Purpose") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Grade">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1u" runat="server" Text='<%# Bind("Grade") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Category">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1uiu" runat="server" Text='<%# Bind("Category") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Product Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>

                                            <asp:BoundField ItemStyle-Width="150px" DataField="ModelNo" HeaderText="Model No." />
                                            <%--<asp:BoundField ItemStyle-Width="150px" DataField="Specification" HeaderText="Specification"/>--%>

                                            <asp:TemplateField HeaderText="Specification">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSpec" runat="server" Text='<%# Bind("Specification") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Qty.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbliQty" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>

                                            <asp:BoundField ItemStyle-Width="150px" DataField="UnitType" HeaderText="Unit" ReadOnly="true">
                                                <ItemStyle Width="5%"></ItemStyle>
                                            </asp:BoundField>

                                            <asp:TemplateField HeaderText="Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbliPrc" runat="server" Text='<%# Bind("Price") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubTotal" runat="server" Text='<%# Bind("Total") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="PriceWithVAT" HeaderText="Price With VAT" SortExpression="PriceWithVAT" />
                                            <asp:BoundField DataField="PriceWithoutVAT" HeaderText="Price Without VAT" SortExpression="PriceWithoutVAT" />
                                        </Columns>

                                        <RowStyle BackColor="#F7F7DE" />
                                        <FooterStyle BackColor="#CCCC99" />
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                        <SelectedRowStyle BackColor="SkyBlue" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="#1889CB" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>

                                </div>
                                <div class="full_block right bold">
                                    <br />
                                    Total item amount:
                                    <asp:Label ID="lblItemAmount" runat="server"></asp:Label>
                                    <br />
                                    <br />
                                </div>

                                <div class="control-group">
                                    <%--<br />
                                    <br />--%>
                                    <legend><b>Purchase other cost & information.</b></legend>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Discount on Purchase : </label>
                                    <asp:TextBox ID="txtPDiscount" runat="server" Text="0" CssClass="form-control" onkeyup="calGTtl()"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">VAT/Service Charge : </label>
                                    <asp:TextBox ID="txtVat" runat="server" Text="0" CssClass="form-control" onkeyup="calGTtl()"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <label class="control-label span12">Other Expenses : </label>
                                    <asp:DropDownList ID="ddotherExp" CssClass="form-control" runat="server" DataSourceID="SqlDataSource6" DataTextField="HeadName" DataValueField="HeadID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT HeadID, [HeadName] FROM [ExpenseHeads] Where AccHeadID='040110037'"></asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <label class="control-label span12">&nbsp; </label>
                                    <asp:TextBox ID="txtOtherExpenseAmount" runat="server" Text="0" CssClass="span6" Width="50%"></asp:TextBox>
                                    <asp:Button ID="btnOthExp" runat="server" Text="add" Width="80px" OnClick="btnOthExp_OnClick"></asp:Button>
                                </div>
                                <div class="control-group">
                                    <%--<label class="control-label span12">&nbsp; </label>--%>
                                    <asp:GridView ID="GridExpenses" runat="server" AutoGenerateColumns="False" AutoGenerateDeleteButton="True" Width="100%" DataKeyNames="eid"
                                        OnRowDeleting="GridExpenses_OnRowDeleting" OnRowDataBound="GridExpenses_OnRowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="eid" SortExpression="Amount" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEntryId" runat="server" Text='<%# Bind("eid") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ExpHeadName" HeaderText="Exp.Head Name" SortExpression="ExpHeadName" />
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" ItemStyle-HorizontalAlign="Right" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT EID, ExpHeadName, Amount FROM PurchaseExpenses WHERE InvoiceNo=@InvoiceNo ORDER BY eid"
                                        DeleteCommand="DELETE FROM PurchaseExpenses WHERE (eid = @eid)">
                                        <DeleteParameters>
                                            <asp:Parameter Name="eid" />
                                        </DeleteParameters>
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblInvoice" Name="InvoiceNo" PropertyName="Text" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>

                                <div class="full_block right bold">
                                    <br />
                                    Total other expense amount:
                                    <asp:Label ID="lblOthersTotal" runat="server"></asp:Label>
                                    <br />
                                    <br />
                                </div>



                                <div class="control-group">
                                    <label class="control-label">Purchase Total (Tk) : </label>
                                    <%--<asp:TextBox ID="txtPtotal" runat="server" ReadOnly="true" Text="0" CssClass="form-control hidden" ></asp:TextBox>--%>
                                    <asp:TextBox ID="txtGTotal" runat="server" ReadOnly="true" Text="0" CssClass="form-control"></asp:TextBox>
                                </div>

                                <%--<div class="form-group">
                                            <label class="control-label">Inventories Head (dr.)</label>
                                            <asp:DropDownList ID="ddInventoriesHead" runat="server" DataSourceID="SqlDataSource20" DataTextField="AccountsHeadName" DataValueField="AccountsHeadID"
                                                CssClass="select2me" Width="70%">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource20" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT AccountsHeadID, AccountsHeadName FROM [HeadSetup]  WHERE IsActive='1' AND (ControlAccountsID  ='010106' OR ControlAccountsID  ='010205') ORDER BY [AccountsHeadID] DESC"></asp:SqlDataSource>
                                        </div>--%>

                                <div class="control-group">
                                    <label class="control-label">Transport Type & No : </label>
                                    <asp:TextBox ID="txtTransportType" runat="server" />
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Stock-in Godown : </label>
                                    <asp:DropDownList ID="ddGodown" runat="server" DataSourceID="SqlDataSource5"
                                        DataTextField="StoreName" DataValueField="WID" CssClass="form-control"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddGodown_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource5" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT WID,[StoreName] FROM [Warehouses]"></asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Location in Godown : </label>
                                    <asp:DropDownList ID="ddLocation" runat="server" DataSourceID="SqlDataSource4"
                                        DataTextField="AreaName" DataValueField="AreaID" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT AreaID,[AreaName] FROM [WareHouseAreas] where Warehouse=@Warehouse">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddGodown" Name="Warehouse" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <asp:Panel ID="chqpanel" runat="server" Visible="False">
                                    <legend>Payment Information</legend>
                                    <div class="control-group">
                                        <asp:Label ID="Label1" runat="server" Text="Payment Mode: "></asp:Label>
                                        <asp:DropDownList ID="ddMode" runat="server" Width="174px"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddMode_SelectedIndexChanged">
                                            <asp:ListItem>Cheque</asp:ListItem>
                                            <asp:ListItem>Cash</asp:ListItem>
                                            <asp:ListItem Selected="True">Credit</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Bank Name :  </label>
                                        <asp:DropDownList ID="ddBank" runat="server" DataSourceID="SqlDataSource8"
                                            DataTextField="BankName" DataValueField="ACID">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT ACID, (Select [BankName] FROM [Banks] where [BankId]=a.BankID) +' - '+ACNo +' - '+ACName AS BankName from BankAccounts a ORDER BY [ACName]"></asp:SqlDataSource>
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

                                <div class="control-group" id="CollField" runat="server">
                                    <asp:Label ID="lblAmt" runat="server" Text="Cheque Amount : "></asp:Label>
                                    <asp:TextBox ID="txtPaid" runat="server" Text="0"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789.-" TargetControlID="txtPaid">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Remarks : </label>
                                    <asp:TextBox ID="txtRemark" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>

                                <%--<div class="control-group">
                                    <label class="control-label">Upload Document : </label>
                                    <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" CssClass="form-control" />
                                </div>--%>
                                <asp:CheckBox runat="server" ID="revateCheckBox" Text=" Is Rebate?" AutoPostBack="True" OnCheckedChanged="revateCheckBox_OnCheckedChanged" />
                                <asp:Panel runat="server" ID="rebateVatPanel" Visible="False">
                                    <legend>Rebate Vat</legend>
                                    <div class="form-group">
                                        <label class="control-label">Dr:</label>
                                        <asp:Label ID="Label4" CssClass="help-block" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddDr" runat="server" DataSourceID="SqlDataSource17" CssClass="select2me" Width="70%" DataTextField="AccountsHeadName"
                                            DataValueField="AccountsHeadID">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource17" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT HeadSetup.AccountsHeadID, AccountGroup.GroupName, Accounts.AccountsName, ControlAccount.ControlAccountsName, HeadSetup.AccountsHeadName, HeadSetup.AccountsOpeningBalance
                                    , HeadSetup.OpBalDr, HeadSetup.OpBalCr FROM AccountGroup INNER JOIN HeadSetup INNER JOIN ControlAccount ON HeadSetup.ControlAccountsID = ControlAccount.ControlAccountsID INNER JOIN Accounts ON HeadSetup.AccountsID = Accounts.AccountsID ON AccountGroup.GroupID = HeadSetup.GroupID WHERE AccountGroup.GroupID = '01'
                                    ORDER BY AccountGroup.GroupID, HeadSetup.AccountsHeadID">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="vendor" Name="Type" Type="String" />
                                                <%--<asp:ControlParameter ControlID="ddSuppCategory" Name="Category" PropertyName="SelectedValue" />--%>
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label">Cr:</label>
                                        <asp:Label ID="Label5" CssClass="help-block" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddCr" runat="server" DataSourceID="SqlDataSource15" CssClass="select2me" Width="70%" DataTextField="AccountsHeadName"
                                            DataValueField="AccountsHeadID">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource15" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT HeadSetup.AccountsHeadID, AccountGroup.GroupName, Accounts.AccountsName, ControlAccount.ControlAccountsName, HeadSetup.AccountsHeadName, HeadSetup.AccountsOpeningBalance
                                    , HeadSetup.OpBalDr, HeadSetup.OpBalCr FROM AccountGroup INNER JOIN HeadSetup INNER JOIN ControlAccount ON HeadSetup.ControlAccountsID = ControlAccount.ControlAccountsID INNER JOIN Accounts ON HeadSetup.AccountsID = Accounts.AccountsID ON AccountGroup.GroupID = HeadSetup.GroupID WHERE AccountGroup.GroupID = '04'
                                    ORDER BY AccountGroup.GroupID, HeadSetup.AccountsHeadID">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="vendor" Name="Type" Type="String" />
                                                <%--<asp:ControlParameter ControlID="ddSuppCategory" Name="Category" PropertyName="SelectedValue" />--%>
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Amount : </label>
                                        <asp:TextBox ID="txtRebateVAT" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>

                                </asp:Panel>
                                <div class="form-actions">
                                    <asp:Button ID="btnSave" runat="server" Text="Save Purchase Order" CssClass="btn blue" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>



                <div class="col-md-6 ">
                    <div class="portlet box green">

                        <asp:Panel runat="server" ID="pnlReturnHistory" Visible="False">
                            <div class="table-responsive">

                                <legend>Item Return History</legend>
                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="table " Width="150%"
                                    BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="1" ForeColor="Black" RowStyle-BackColor="#A1DCF2"
                                    HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="#96D9FF" GridLines="Vertical" SelectedRowStyle-BackColor="LightBlue" DataSourceID="SqlDataSource9">

                                    <Columns>

                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" />
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="ReturnDate" HeaderText="Return Date" SortExpression="ReturnDate" DataFormatString="{0:d}"></asp:BoundField>

                                        <asp:BoundField DataField="Grade" HeaderText="Item Grade" SortExpression="SupplierName" Visible="False"></asp:BoundField>

                                        <asp:BoundField DataField="Category" HeaderText="Item Category" SortExpression="InvoiceNo" />
                                        <asp:BoundField DataField="ItemName" HeaderText="Item Name" SortExpression="ItemName" />
                                        <asp:BoundField DataField="Qty" HeaderText="Qty.(Send/ Receive)" SortExpression="Qty" />

                                    </Columns>
                                    <EmptyDataTemplate>
                                        <p>This purchase order has no item returned!</p>
                                    </EmptyDataTemplate>
                                    <RowStyle BackColor="#F7F7DE" />
                                    <FooterStyle BackColor="#CCCC99" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                    <SelectedRowStyle BackColor="SkyBlue" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="Gray" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource9" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [ReturnDate], [SupplierName],
                                            (SELECT GradeName FROM [ItemGrade] where GradeID=(SELECT GradeID FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=PurchaseReturn.ItemCode))) As Grade,
                                                (SELECT CategoryName FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=PurchaseReturn.ItemCode)) As Category,
                                            [InvoiceNo], [ItemName], [Qty] FROM [PurchaseReturn] Where InvoiceNo=@InvoiceNo ORDER BY [eid] DESC">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddInvoice" Name="InvoiceNo" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                            </div>
                        </asp:Panel>



                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Purchase List
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

                                <div class="table-responsive">
                                    <asp:GridView ID="ItemGrid2" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource12"
                                        Width="300%" Visible="False">
                                        <Columns>
                                            <asp:BoundField DataField="Itemgroup" HeaderText="Itemgroup" SortExpression="Itemgroup" />
                                            <asp:BoundField DataField="SubGroup" HeaderText="SubGroup" SortExpression="SubGroup" />
                                            <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" />
                                            <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                                            <asp:BoundField DataField="ItemName" HeaderText="ItemName" SortExpression="ItemName" />
                                            <asp:BoundField DataField="pcs" HeaderText="pcs" SortExpression="pcs" />
                                            <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" />
                                            <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" />
                                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                                            <asp:BoundField DataField="Manufacturer" HeaderText="Manufacturer" SortExpression="Manufacturer" />
                                            <asp:BoundField DataField="CountryOfOrigin" HeaderText="CountryOfOrigin" SortExpression="CountryOfOrigin" />
                                            <asp:BoundField DataField="PackSize" HeaderText="PackSize" SortExpression="PackSize" />
                                            <asp:BoundField DataField="Warrenty" HeaderText="Warrenty" SortExpression="Warrenty" />
                                            <asp:BoundField DataField="SerialNo" HeaderText="SerialNo" SortExpression="SerialNo" />
                                            <asp:BoundField DataField="UnitType" HeaderText="UnitType" SortExpression="UnitType" />
                                            <asp:BoundField DataField="SizeRef" HeaderText="SizeRef" SortExpression="SizeRef" />
                                            <asp:BoundField DataField="StockType" HeaderText="StockType" SortExpression="StockType" />
                                            <asp:BoundField DataField="StockLocation" HeaderText="StockLocation" SortExpression="StockLocation" />
                                            <asp:BoundField DataField="PerviousDeliveredQty" HeaderText="PerviousDeliveredQty" SortExpression="PerviousDeliveredQty" />
                                            <asp:BoundField DataField="QtyBalance" HeaderText="QtyBalance" SortExpression="QtyBalance" />
                                            <asp:BoundField DataField="PriceWithVAT" HeaderText="PriceWithVAT" SortExpression="PriceWithVAT" />
                                            <asp:BoundField DataField="PriceWithoutVAT" HeaderText="PriceWithoutVAT" SortExpression="PriceWithoutVAT" />
                                            <asp:BoundField DataField="ReturnQty" HeaderText="ReturnQty" SortExpression="ReturnQty" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [Itemgroup], [SubGroup], [Grade], [Category], [ItemName], [pcs], [Qty], [Price], [Total], [Manufacturer], [CountryOfOrigin], [PackSize], [Warrenty], [SerialNo], [UnitType], [SizeRef], [StockType], [StockLocation], [PerviousDeliveredQty], [QtyBalance], [PriceWithVAT], [PriceWithoutVAT], [ReturnQty] FROM [PurchaseDetails] WHERE ([InvNo] = @InvNo) ORDER BY [Id]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblInvoice" Name="InvNo" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>


                                    <asp:GridView ID="GridView1" runat="server" Width="150%" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
                                        <Columns>
                                            <asp:BoundField DataField="OrderDate" HeaderText="Pur. Date" SortExpression="EntryDate" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="InvNo" HeaderText="InvNo" SortExpression="InvNo" />
                                            <asp:BoundField DataField="BillNo" HeaderText="LC/TT/Inv#" SortExpression="InvNo" />
                                            <asp:BoundField DataField="SupplierName" HeaderText="PurchaseFrom" SortExpression="PurchaseFrom" />
                                            <asp:BoundField DataField="ItemTotal" HeaderText="Item Total" SortExpression="PurchaseTotal" DataFormatString="{0:N}" />
                                            <asp:BoundField DataField="PurchaseDiscount" HeaderText="Discount" SortExpression="PurchaseTotal" />
                                            <asp:BoundField DataField="VatService" HeaderText="Vat/Service" SortExpression="PurchaseTotal" DataFormatString="{0:N}" />
                                            <asp:BoundField DataField="OtherExp" HeaderText="Others" SortExpression="PurchaseTotal" />
                                            <asp:BoundField DataField="PurchaseTotal" HeaderText="P.Total" SortExpression="PurchaseTotal" DataFormatString="{0:N}" />

                                        </Columns>
                                        <FooterStyle BackColor="#CCCCCC" />
                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle Font-Bold="True" ForeColor="Black" />
                                        <AlternatingRowStyle BackColor="#ffffff" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT TOP(20) [InvNo], BillNo, [SupplierName],  ItemTotal, PurchaseDiscount, VatService, OtherExp,  [PurchaseTotal], [OrderDate] FROM [Purchase] Order by PID desc">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="txtDate" Name="PuchaseDate"
                                                PropertyName="Text" Type="DateTime" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                            </div>
                        </div>

                    </div>

                </div>


            </div>



        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>

