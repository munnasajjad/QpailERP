<%@ Page Title="LC Amendment" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="LC-Edit.aspx.cs" Inherits="app_LC_Edit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <script type="text/javascript">
        $(document).ready(function () {

            $('#<%=txtCfrBDT.ClientID%>').attr('readonly', true);
            $('#<%=txtBankBDT.ClientID%>').attr('readonly', true);
            //$('#<%=txtMargin.ClientID%>').attr('readonly', true);
            $('#<%=txtLTR.ClientID%>').attr('readonly', true);
            $('#<%=txtCFR.ClientID%>').attr('readonly', true);
        })

        function calCFRBDT() {            
            var bankExRate = $('#<%=txtBExRate.ClientID%>').val();
            var cusExRate = $('#<%=txtCExRate.ClientID%>').val();
            var cfrUSD = $('#<%=txtCfrUSD.ClientID%>').val();

            var cfrBDT = parseFloat(cfrUSD) * parseFloat(cusExRate);
            var bankBDT = parseFloat(cfrUSD) * parseFloat(bankExRate);
            
            $('#<%=txtCfrBDT.ClientID%>').val(cfrBDT.toString());
            $('#<%=txtBankBDT.ClientID%>').val(bankBDT.toString());

            var lcMargin = parseFloat($('#<%=txtMargin.ClientID%>').val()); //bankBDT * 0.1;
            var LTR = bankBDT - lcMargin;
            //$('#<%=txtMargin.ClientID%>').val(lcMargin.toString());
            $('#<%=txtLTR.ClientID%>').val(LTR.toString());
        }


        function calItemCFR() {
            var qty = $('#<%=txtQty.ClientID%>').val();
            var rate = $('#<%=txtPrice.ClientID%>').val();

            var ttl = parseFloat(qty) * parseFloat(rate);
            $('#<%=txtCFR.ClientID%>').val(ttl.toString());
        }

    </script>

    <style type="text/css">
         td span, td input {
  width: 100%!important;
}
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">




</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-title">
                <asp:Literal ID="ltrFrmName" runat="server" Text="LC Amendment" />
            </h3>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="portlet box red">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>
                        <asp:Literal ID="ltrSubFrmName" runat="server" Text="Create New LC" />
                    </div>
                </div>
                <div class="portlet-body form">

                    <div class="form-body">
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                        <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>

                        <div id="EditField" runat="server" class="control-group hidden">
                            <b style="color: #E84940">
                                <asp:Label ID="lblEname" runat="server" Text="Amendment LC# "></asp:Label>
                                <asp:DropDownList ID="ddName" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" DataSourceID="SqlDataSource1x"
                                    DataTextField="LCNo" DataValueField="sl"
                                    OnSelectedIndexChanged="ddName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1x" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT sl, [LCNo] FROM [LC] WHERE (([ProjectID] = @ProjectID) AND ([IsActive] = 'A')) ORDER BY [sl] desc">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="ProjectID" SessionField="ProjectID" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </b>
                        </div>

                        <div id="LCNoInput" runat="server" class="control-group hidden">
                            <label class="control-label">LC/TT No.</label>
                            <div class="controls">
                                <asp:TextBox ID="txtNo" runat="server" Enabled="True"></asp:TextBox>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">L/C Type :  </label>
                            <div class="controls">
                                <div class="controls">
                                    <asp:DropDownList ID="ddType" runat="server">
                                        <asp:ListItem>LC</asp:ListItem>
                                        <asp:ListItem>TT</asp:ListItem>
                                        <asp:ListItem>UPAS</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">Open Date :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtDate" runat="server" Enabled="True"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                    Enabled="True" TargetControlID="txtDate">
                                </asp:CalendarExtender>
                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">Category :  </label>
                            <div class="controls">
                                <asp:DropDownList ID="ddGroup" runat="server" DataSourceID="SqlDataSource2"
                                    DataTextField="GroupName" DataValueField="GroupSrNo" AutoPostBack="true" OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                    SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] Where GroupSrNo<>2 AND GroupSrNo<>3 ORDER BY [GroupSrNo]"></asp:SqlDataSource>
                            </div>
                        </div>
                        
                        <div class="control-group">
                            <label class="control-label">For Company/Dept :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtFor" runat="server" Enabled="True"></asp:TextBox>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">Expiry Date :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtExpiryDate" runat="server" />
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                    Enabled="True" TargetControlID="txtExpiryDate">
                                </asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">Ship Date :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtShipDate" runat="server" />
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                    Enabled="True" TargetControlID="txtShipDate">
                                </asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">Manufacturer :  </label>
                            <div class="controls">
                                <asp:DropDownList ID="ddManufacturer" runat="server" DataSourceID="SqlDataSource3" DataTextField="Company" DataValueField="PartyID">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="supplier" Name="Type" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">Country of Origin :  </label>
                            <div class="controls">
                                <asp:DropDownList ID="ddCountry" runat="server" DataSourceID="SqlDataSource4" DataTextField="country" DataValueField="country">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [country] FROM [Countries] ORDER BY [country]"></asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">Agent/ Shipper :  </label>
                            <div class="controls">
                                <asp:DropDownList ID="ddAgent" runat="server" DataSourceID="SqlDataSource5" DataTextField="Company" DataValueField="PartyID">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="agents" Name="Type" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">Insurance Company :  </label>
                            <div class="controls">
                                <asp:DropDownList ID="ddInsurance" runat="server" DataSourceID="SqlDataSource6" DataTextField="BankName" DataValueField="BankId">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [BankId], [BankName] FROM [Banks] WHERE ([Type] = @Type) ORDER BY [BankName]">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="insurance" Name="Type" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">CNF Agent :  </label>
                            <div class="controls">
                                <asp:DropDownList ID="ddCNF" runat="server" DataSourceID="SqlDataSource7" DataTextField="Company" DataValueField="PartyID">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="cnf" Name="Type" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">Bank Account :  </label>
                            <div class="controls">
                                <asp:DropDownList ID="ddBank" runat="server" DataSourceID="SqlDataSource8" DataTextField="Bank" DataValueField="ACID">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT ACID, (Select [BankName] FROM [Banks] where [BankId]=a.BankID) +' - '+ACNo +' - '+ACName AS Bank from BankAccounts a ORDER BY [ACName]"></asp:SqlDataSource>

                            </div>
                        </div>
                        
                        <div class="control-group">
                            <label class="control-label">Bank Exchange Rate :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtBExRate" runat="server" Enabled="True"></asp:TextBox>
                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">Custom Exch Rate :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtCExRate" runat="server" Enabled="True"></asp:TextBox>
                            </div>
                        </div>

                        <%--<div class="control-group">
                            <label class="control-label">Qnty Unit :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtQty" runat="server" Enabled="True"></asp:TextBox>
                            </div>
                        </div>--%>

                        <div class="control-group">
                            <label class="control-label">Total Qnty :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtTtlQty" runat="server" Enabled="True"></asp:TextBox>
                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">Freight USD :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtFreight" runat="server" Enabled="True"></asp:TextBox>
                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">CFR USD :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtCfrUSD" runat="server" Enabled="True"></asp:TextBox>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">CFR BDT :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtCfrBDT" runat="server" Enabled="True"></asp:TextBox>
                            </div>
                        </div>
                        
                                <div class="control-group">
                                    <label class="control-label">FOB/ Bank BDT :  </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtBankBDT" runat="server" Enabled="True"></asp:TextBox>
                                    </div>
                                </div>
                        <div class="control-group">
                            <label class="control-label">L/C Margin :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtMargin" runat="server" Enabled="True"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtMargin">
                                </asp:FilteredTextBoxExtender>

                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">LTR :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtLTR" runat="server" Enabled="True"></asp:TextBox>
                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">Remarks:  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtRemarks" runat="server" Enabled="True"></asp:TextBox>
                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">Mode of Transport :  </label>
                            <div class="controls">
                                <asp:DropDownList ID="ddMode" runat="server">
                                    <asp:ListItem>Air</asp:ListItem>
                                    <asp:ListItem>Road</asp:ListItem>
                                    <asp:ListItem>Sea</asp:ListItem>
                                </asp:DropDownList>

                            </div>
                        </div>

                        <a name="ItemDetails"></a>
                        <legend style="margin-bottom: 6px;">Item Details</legend>

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
                                <asp:DropDownList ID="ddItemName" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        
                        <div class="control-group">
                            <label class="control-label">HS Code :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtHSCode" runat="server" Enabled="True"></asp:TextBox>
                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">Pack Size :  </label>
                            <div class="controls">
                                <asp:DropDownList ID="ddSize" runat="server" DataSourceID="SqlDataSource11"
                                    DataTextField="BrandName" DataValueField="BrandID">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT BrandID, BrandName FROM [Brands] ORDER BY [DisplaySl]"></asp:SqlDataSource>
                            </div>
                        </div>
                        
                                <div id="measurementField" runat="server"  class="control-group">
                                    <label class="control-label">
                                         <asp:Literal ID="ltrMeasurement" runat="server" Text="Measurement : "></asp:Literal> </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtMeasure" runat="server" Enabled="True"></asp:TextBox>
                                    </div>
                                </div>

                        <div class="control-group">
                            <label class="control-label">
                                Quantity/Weight:
                                <asp:Literal ID="ltrUnitType" runat="server"></asp:Literal>
                            </label>
                            <div class="controls">
                                <asp:TextBox ID="txtQty" runat="server"  onkeyup="calItemCFR()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtQty">
                                </asp:FilteredTextBoxExtender>

                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">Unit Price USD :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtPrice" runat="server"  onkeyup="calItemCFR()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtPrice">
                                </asp:FilteredTextBoxExtender>

                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">CFR Value USD:  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtCFR" runat="server" Enabled="True"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtCFR">
                                </asp:FilteredTextBoxExtender>

                                <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add to grid" OnClick="btnAdd_Click" />
                            </div>
                        </div>
                        <asp:Label ID="lblMsg2" EnableViewState="false" runat="server" Text=""></asp:Label>
                        <div class="control-group">
                            <div class="portlet-body form">

                                <div class="form-body">
                                    
                                <div class="table-responsive">
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowDeleting="GridView1_RowDeleting"
                                        DataSourceID="SqlDataSource9" Width="150%" DataKeyNames="EntryID" RowStyle-VerticalAlign="Middle">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EntryID" SortExpression="EntryID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("EntryID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Product" SortExpression="Product" ItemStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("Product") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="HS Code" SortExpression="HSCode" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1x" runat="server" Text='<%# Bind("HSCode") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           
                                                    <asp:TemplateField HeaderText="Pk Size" SortExpression="Size" >
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1xz" runat="server" Text='<%# Bind("Size") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Thickness" SortExpression="HSCode" >
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1xb" runat="server" Text='<%# Bind("Thickness") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Measurement" SortExpression="HSCode" >
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1x1" runat="server" Text='<%# Bind("Measurement") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                            <asp:TemplateField HeaderText="QTY" SortExpression="QTY1">
                                                <ItemTemplate>
                                                    <asp:Label ID="QTY9" runat="server" Text='<%# Bind("QTY1") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Price(USD)" SortExpression="UnitPrice">
                                                <ItemTemplate>
                                                    <asp:Label ID="UnitPrice" runat="server" Text='<%# Bind("UnitPrice") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CFR (USD)" SortExpression="CFRValue">
                                                <ItemTemplate>
                                                    <asp:Label ID="CFRValue" runat="server" Text='<%# Bind("CFRValue") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Approve" ToolTip="Edit" Width="24px" />
                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.gif" Text="Delete" ToolTip="Delete" Width="24px" />

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
                                        </Columns>
                                    </asp:GridView>


                                    <asp:SqlDataSource ID="SqlDataSource9" runat="server"
                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand="SELECT EntryID, (Select ItemName from Products where ProductID=a.ItemCode) as Product,                      
                    [HSCode], (Select BrandName from Brands where BrandID=a.ItemSizeID) as Size, Measurement, Thickness,
                    CONVERT(varchar(10), qty) +' '+(Select UnitType from Products where ProductID=a.ItemCode) As QTY1, UnitPrice,  [CFRValue] 
                    FROM [LcItems] a Where LCNo= (Select LCNo from lc where sl=@sl) ORDER BY [EntryID]"
                            DeleteCommand="DELETE LcItems WHERE EntryID=@EntryID"
                            UpdateCommand="">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddName" Name="sl" PropertyName="SelectedValue" Type="String" />
                            </SelectParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="EntryID" />
                            </DeleteParameters>
                        </asp:SqlDataSource>
                                    
                                    
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-actions">
                            <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save New LC" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" CssClass="btn" runat="server" Text="Cancel LC" Visible="false" />
                        </div>

                        <asp:Label ID="lblLogin" runat="server" Text="" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
        </div>








        <%--Ammt History--%>


        <div class="col-md-6 ">
            <div class="portlet box green">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i> 
                        Amendment history for LC# <asp:Literal ID="ltrFormTitle2" runat="server"></asp:Literal>
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


                        <asp:Label ID="lblSl" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblMsgNav" runat="server" EnableViewState="false"></asp:Label>

                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" OnRowDeleting="GridView1_RowDeleting"
                            DataSourceID="SqlDataSource10" Width="100%" DataKeyNames="sl">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>

                                    <ItemStyle Width="20px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="sl" SortExpression="sl" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("sl") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Ammt. Date" SortExpression="HSCode">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1x" runat="server" Text='<%# Bind("EntryDate","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CFR USD" SortExpression="Size">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1xz" runat="server" Text='<%# Bind("CfrUSD") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CFR BDT" SortExpression="QTY1">
                                    <ItemTemplate>
                                        <asp:Label ID="CFRBDT" runat="server" Text='<%# Bind("CfrBDT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expiry Date" SortExpression="UnitPrice">
                                    <ItemTemplate>
                                        <asp:Label ID="ExpiryDate" runat="server" DataFormatString="{0:d}" Text='<%# Bind("ExpiryDate","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ship Date" SortExpression="CFRValue">
                                    <ItemTemplate>
                                        <asp:Label ID="ShipDate" runat="server" DataFormatString="{0:d}" Text='<%# Bind("ShipDate","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>


                        <asp:SqlDataSource ID="SqlDataSource10" runat="server"
                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand="SELECT sl, EntryDate, CfrUSD, CfrBDT, ExpiryDate, ShipDate
                                            FROM [LcHistory] a Where LCNo=@LCNo ORDER BY [sl]"
                            DeleteCommand="DELETE LcItems WHERE EntryID=@EntryID"
                            UpdateCommand="">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ltrFormTitle2" Name="LCNo" PropertyName="Text" Type="String" />
                            </SelectParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="EntryID" />
                            </DeleteParameters>
                        </asp:SqlDataSource>


                        <div class="form-actions">

                        </div>






                    </div>
                </div>
            </div>
        </div>

    </div>
            
            
        </ContentTemplate>
    </asp:UpdatePanel>
    

</asp:Content>


