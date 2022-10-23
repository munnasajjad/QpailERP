<%@ Page Title="LC Preview" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="LC-Preview.aspx.cs" Inherits="app_LC_Preview" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .controls {
            padding-top: 11px;
        }

        table#ctl00_BodyContent_GridView2 {
            border: 2px solid #666;
            max-width: 200% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="Server">
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

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>


            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">
                        <asp:Literal ID="ltrFrmName" runat="server" Text="LC# " />
                        <asp:Label ID="lblLCNo" runat="server" />
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="portlet box red">
                        <%--<div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="ltrSubFrmName" runat="server" Text="Create New LC" />
                            </div>
                        </div>--%>
                        <div class="portlet-body form">
                            <div class="form-body">

                                <asp:Label ID="txtEditLcNo" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblSl" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblMsgNav" runat="server" EnableViewState="false"></asp:Label>
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                                <div id="Div1" runat="server" class="control-group hidden">
                                    <b style="color: #E84940">
                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                        <asp:Label ID="Label4" runat="server"></asp:Label>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT sl, [LCNo] FROM [LC] WHERE (([ProjectID] = @ProjectID) AND ([IsActive] = 'A')) ORDER BY [sl]">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="ProjectID" SessionField="ProjectID" Type="Int32" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </b>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">L/C Type :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblLCType" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Open Date :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblOpDate" runat="server" />
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">General Item Type :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="txtReferrence" runat="server" />
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">For Company/Dept :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblDept" runat="server" />
                                    </div>
                                </div>


                                <div class="control-group">
                                    <label class="control-label">Expiry Date :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblExDate" runat="server" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Ship Date :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblShipDate" runat="server" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Arrival  Date :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblArrivalDt" runat="server" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Port Delivery  Date :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblDeliveryDt" runat="server" />
                                    </div>
                                </div>


                                <div class="control-group">
                                    <label class="control-label">Manufacturer :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblSupplier" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Country of Origin :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblCountry" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Agent/ Shipper :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblagent" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Insurance Company :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblInsurance" runat="server"></asp:Literal>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">CNF Agent :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblCNF" runat="server"></asp:Literal>

                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Bank Account :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblBank" runat="server"></asp:Literal>

                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="col-md-6">
                    <div class="portlet box red">

                        <div class="portlet-body form">
                            <div class="form-body">

                                <div class="control-group">
                                    <label class="control-label">Bank Exchange Rate :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblBankExcRate" runat="server" />
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Custom Exch Rate :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblCustExRate" runat="server" />
                                    </div>
                                </div>

                                <%--<div class="control-group">
                            <label class="control-label">Qnty Unit :  </label>
                            <div class="controls">
                                <asp:Label ID="TextBox15" runat="server" enabled="True"></label>
                            </div>
                        </div>--%>

                                <div class="control-group">
                                    <label class="control-label">Total Qnty :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblTtlQty" runat="server" />
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Freight USD :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblFreight" runat="server" />
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">CFR USD :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblCfrUsd" runat="server" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">CFR BDT :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblCfrBdt" runat="server" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Bank BDT :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblBankBdt" runat="server" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">L/C Margin :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblMargin" runat="server" />
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">LTR :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblLTR" runat="server" />
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Remarks:  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblRemark" runat="server" />
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Mode of Transport :  </label>
                                    <div class="controls">
                                        <asp:Literal ID="lblMode" runat="server"></asp:Literal>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label"></label>
                                    <div class="controls">
                                        <h5 id="lblStatusColor" runat="server">
                                            <asp:Literal ID="lblStatus" runat="server" EnableViewState="false"></asp:Literal></h5>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>



            </div>
            
            
            
            
            
            


                <legend style="margin-bottom: 4px; margin-top: 5px;">Item Details</legend>
            <div class="table-responsive">
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource10" Width="400%" CssClass="table xtable-b">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="20px">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>

                            <ItemStyle Width="20px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:BoundField DataField="iGroup" HeaderText="Item Group" SortExpression="SupplierName"></asp:BoundField>
                        <asp:BoundField DataField="subgroup" HeaderText="Item Sub-Group" SortExpression="InvoiceNo" />

                        <asp:BoundField DataField="Grade" HeaderText="Item Grade" SortExpression="SupplierName"></asp:BoundField>
                        <asp:BoundField DataField="Category" HeaderText="Item Category" SortExpression="InvoiceNo" />

                        <asp:TemplateField HeaderText="Item" SortExpression="Product">
                            <ItemTemplate>
                                <asp:Label ID="Label2v" runat="server" Text='<%# Bind("Product") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HS Code" SortExpression="HSCode">
                            <ItemTemplate>
                                <asp:Label ID="Label1x" runat="server" Text='<%# Bind("HSCode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Size" SortExpression="Size">
                            <ItemTemplate>
                                <asp:Label ID="Label1xz" runat="server" Text='<%# Bind("Size") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Model No." SortExpression="Size">
                            <ItemTemplate>
                                <asp:Label ID="gdfg" runat="server" Text='<%# Bind("Measurement") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Serial#" SortExpression="Size">
                            <ItemTemplate>
                                <asp:Label ID="gd4fg" runat="server" Text='<%# Bind("Thickness") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="QntyPerPack" HeaderText="Warrentry" SortExpression=""></asp:BoundField>
                        <asp:BoundField DataField="NoOfPacks" HeaderText="Specification" SortExpression=""></asp:BoundField>
                        <asp:BoundField DataField="FullDescription" HeaderText="Description" SortExpression="SupplierName"></asp:BoundField>

                        <asp:TemplateField HeaderText="QTY" SortExpression="QTY1">
                            <ItemTemplate>
                                <asp:Label ID="QTY9" runat="server" Text='<%# Bind("QTY1") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Price (USD)" SortExpression="UnitPrice">
                            <ItemTemplate>
                                <asp:Label ID="UnitPrice" runat="server" Text='<%# Bind("UnitPrice") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CFR (USD)" SortExpression="CFRValue">
                            <ItemTemplate>
                                <asp:Label ID="CFRValue" runat="server" Text='<%# Bind("CFRValue") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Loading" HeaderText="Loading(%)" SortExpression="SupplierName"></asp:BoundField>
                        <asp:BoundField DataField="Loaded" HeaderText="Loaded" SortExpression="InvoiceNo" />
                        <asp:BoundField DataField="LandingPercent" HeaderText="Landing(%)" SortExpression="SupplierName"></asp:BoundField>
                        <asp:BoundField DataField="LandingAmt" HeaderText="Landing Amt" SortExpression="InvoiceNo" />

                        <asp:BoundField DataField="TotalUSD" HeaderText="Total USD" SortExpression="SupplierName"></asp:BoundField>
                        <asp:BoundField DataField="TotalBDT" HeaderText="Total BDT" SortExpression="InvoiceNo" />
                    </Columns>
                </asp:GridView>
            </div>

            <asp:SqlDataSource ID="SqlDataSource10" runat="server"
                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                               SelectCommand="SELECT EntryID,
                (SELECT GroupName FROM ItemGroup WHERE GroupSrNo =(SELECT GroupID FROM ItemSubGroup WHERE CategoryID =(SELECT CategoryID FROM [ItemGrade] where GradeID=(SELECT GradeID FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=a.ItemCode))))) As iGroup, 
                (SELECT CategoryName FROM ItemSubGroup WHERE CategoryID =(SELECT CategoryID FROM [ItemGrade] where GradeID=(SELECT GradeID FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=a.ItemCode)))) As Subgroup, 
                (SELECT GradeName FROM [ItemGrade] where GradeID=(SELECT GradeID FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=a.ItemCode))) As Grade, 
                (SELECT CategoryName FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=a.ItemCode)) As Category, 
                (Select ItemName from Products where ProductID=a.ItemCode) as Product,                      
                [HSCode], (Select BrandName from Brands where BrandID=a.ItemSizeID) as Size, NoOfPacks,
                Measurement, (Select spec from Specifications where id=a.Spec) as Spec, Thickness, QntyPerPack,
                CONVERT(varchar(10), qty) +' '+(Select UnitType from Products where ProductID=a.ItemCode) As QTY1, UnitPrice,  [CFRValue],
                Loading, Loaded, LandingPercent, LandingAmt, TotalUSD, TotalBDT, UnitCostBDT , FullDescription
                FROM [LcItems] a Where LCNo= (Select LCNo from lc where sl=@sl) ORDER BY [EntryID]"
                DeleteCommand="DELETE LcItems WHERE EntryID=@EntryID"
                UpdateCommand="">
                <SelectParameters>
                    <asp:ControlParameter ControlID="lblSl" Name="sl" PropertyName="Text" Type="String" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="EntryID" />
                </DeleteParameters>
            </asp:SqlDataSource>

            <div class="table-responsive">
                <legend style="margin-bottom: 4px; margin-top: 5px;">Status Update History</legend>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource2" Width="100%" CssClass="table xtable-b">
                    <Columns>
                        <asp:BoundField DataField="EntryDate" HeaderText="EntryDate" SortExpression="EntryDate" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="Subject" HeaderText="Subject" SortExpression="Subject"></asp:BoundField>

                        <asp:BoundField DataField="ImgDetail" HeaderText="Description" SortExpression="ImgDetail" />
                        <asp:TemplateField HeaderText="Document File" SortExpression="Img">
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" Target="_blank" runat="server" Text="Open File"
                                    NavigateUrl='<%#"./" + Eval("Img") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="EntryBy" HeaderText="EntryBy" SortExpression="EntryBy" />
                    </Columns>
                    <EmptyDataTemplate>
                        <center>No update history records found!</center>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>

            <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                SelectCommand="SELECT [Subject], [ImgDetail], [Img], [EntryDate], [EntryBy] FROM [ImportentDocuments] WHERE (([DocType] = @DocType) AND ([LinkID] = @LinkID)) ORDER BY [EntryDate]">
                <SelectParameters>
                    <asp:Parameter DefaultValue="LC" Name="DocType" Type="String" />
                    <asp:ControlParameter ControlID="lblSl" Name="LinkID" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
            
            
            
            

                <legend style="margin-bottom: 4px; margin-top: 5px;">LC Amendment History</legend>
               
            <div class="table-responsive">
                
                                    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" 
                                        DataSourceID="SqlDataSource13" Width="100%"  CssClass="table xtable-b" >
                                        
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle Width="20px"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DataType" HeaderText="Description" SortExpression="DataType" />
                                            <asp:BoundField DataField="OldData" HeaderText="Old Data" SortExpression="OldData" />
                                            <asp:BoundField DataField="NewData" HeaderText="New Data" SortExpression="NewData" />
                                            <asp:BoundField DataField="EntryBy" HeaderText="Edited By" SortExpression="EntryBy" />
                                            <asp:BoundField DataField="Entrydate" HeaderText="Edit Date" SortExpression="Entrydate" DataFormatString="{0:d}" />
                                        </Columns>
                                        
                                    </asp:GridView>
                                    
                                    <asp:SqlDataSource ID="SqlDataSource13" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [LCNo], [DataType], [OldData], [NewData], [EntryBy], [Entrydate] FROM [LC_Amendment] WHERE ([LCNo] = @LCNo) order by sl">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblLCNo" Name="LCNo" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    

                 <%--<asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource13" Width="100%" CssClass="table xtable-b">
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
                    <EmptyDataTemplate>
                        <center>No amendment history records found!</center>
                    </EmptyDataTemplate>
                </asp:GridView>


                <asp:SqlDataSource ID="SqlDataSource13" runat="server"
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                    SelectCommand="SELECT sl, EntryDate, CfrUSD, CfrBDT, ExpiryDate, ShipDate
                                            FROM [LcHistory] a Where LCNo=@LCNo ORDER BY [sl]"
                    DeleteCommand="DELETE LcItems WHERE EntryID=@EntryID"
                    UpdateCommand="">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblLCNo" Name="LCNo" PropertyName="Text" Type="String" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="EntryID" />
                    </DeleteParameters>
                </asp:SqlDataSource>--%>
            </div>




                <legend style="margin-bottom: 4px; margin-top: 5px;">Customs Duty Information for Items</legend>

            <div class="table-responsive">
                <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource4" Width="300%" CssClass="table xtable-b">
                    <Columns>
                        <asp:BoundField DataField="ItemCode" HeaderText="Item" SortExpression="ItemCode" />
                        <asp:BoundField DataField="InsuranceAmount" HeaderText="Insurance Amount" SortExpression="InsuranceAmount" />
                        <asp:BoundField DataField="PenaltyDesc" HeaderText="Penalty/Others Description" SortExpression="PenaltyDesc" />
                        <asp:BoundField DataField="PenaltyAmt" HeaderText="Penalty/Others Amount" SortExpression="PenaltyAmt" />
                        <asp:BoundField DataField="AV_Calculated" HeaderText="AV Calculated" SortExpression="AV_Calculated" />
                        <asp:BoundField DataField="AV_Actual" HeaderText="AV Actual" SortExpression="AV_Actual" />
                        <asp:BoundField DataField="CustomsDutyRate" HeaderText="CD Rate" SortExpression="CustomsDutyRate" />
                        <asp:BoundField DataField="CustomsDutyAmt" HeaderText="CD Amt" SortExpression="CustomsDutyAmt" />
                        <asp:BoundField DataField="RDRate" HeaderText="RD Rate" SortExpression="RDRate" />
                        <asp:BoundField DataField="RDAmt" HeaderText="RD Amt" SortExpression="RDAmt" />
                        <asp:BoundField DataField="SDRate" HeaderText="SD Rate" SortExpression="SDRate" />
                        <asp:BoundField DataField="SDAmt" HeaderText="SD Amt" SortExpression="SDAmt" />
                        <asp:BoundField DataField="SurChargeRate" HeaderText="Sur-Charge Rate" SortExpression="SurChargeRate" />
                        <asp:BoundField DataField="SurChargeAmt" HeaderText="Sur-Charge Amt" SortExpression="SurChargeAmt" />
                        <asp:BoundField DataField="VATRate" HeaderText="VAT Rate" SortExpression="VATRate" />
                        <asp:BoundField DataField="VATAmt" HeaderText="VAT Amt" SortExpression="VATAmt" />
                        <asp:BoundField DataField="AITRate" HeaderText="AIT Rate" SortExpression="AITRate" />
                        <asp:BoundField DataField="AITAmt" HeaderText="AIT Amt" SortExpression="AITAmt" />
                        <asp:BoundField DataField="ATVRate" HeaderText="ATV Rate" SortExpression="ATVRate" />
                        <asp:BoundField DataField="ATVAmt" HeaderText="ATV Amt" SortExpression="ATVAmt" />

                        <asp:BoundField DataField="TotalDutyCalculated" HeaderText="Total Duty Calculated" SortExpression="TotalDutyCalculated" />
                        <asp:BoundField DataField="TotalDutyActual" HeaderText="Total Duty Actual" SortExpression="TotalDutyActual" />

                    </Columns>
                    <EmptyDataTemplate>
                        <center>No costing calculation records found!</center>
                    </EmptyDataTemplate>
                </asp:GridView>


                <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                    SelectCommand="SELECT  (Select ItemName from Products where ProductID=a.ItemCode) as ItemCode, [InsuranceAmount], [PenaltyDesc], [PenaltyAmt], [AV_Calculated], [AV_Actual], [CustomsDutyRate], [CustomsDutyAmt], [RDRate], [RDAmt], [SDRate], [SDAmt], [SurChargeRate], [SurChargeAmt], [VATRate], [VATAmt], [AITRate], [AITAmt], [ATVRate], [ATVAmt], [TotalDutyCalculated], [TotalDutyActual] FROM [LC_Items_Duty] a WHERE ([LCNo] = @LCNo)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblLCNo" Name="LCNo" PropertyName="Text" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>






                <legend style="margin-bottom: 4px; margin-top: 5px;">CNF Commission Tax</legend>

            <div class="table-responsive">
                <asp:GridView ID="GridView6" runat="server" AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource5" Width="200%" CssClass="table xtable-b">
                    <Columns>
                        <asp:BoundField DataField="CalculatedTaxBase" HeaderText="CalculatedTaxBase" SortExpression="CalculatedTaxBase" />
                        <asp:BoundField DataField="ActualTaxBase" HeaderText="ActualTaxBase" SortExpression="ActualTaxBase" />
                        <asp:BoundField DataField="VATRate" HeaderText="VATRate" SortExpression="VATRate" />
                        <asp:BoundField DataField="VATAmount" HeaderText="VATAmount" SortExpression="VATAmount" />
                        <asp:BoundField DataField="AITRate" HeaderText="AITRate" SortExpression="AITRate" />
                        <asp:BoundField DataField="AITAmount" HeaderText="AITAmount" SortExpression="AITAmount" />
                        <asp:BoundField DataField="DocumentProcessing" HeaderText="DocumentProcessing" SortExpression="DocumentProcessing" />
                        <asp:BoundField DataField="CSFee" HeaderText="CSFee" SortExpression="CSFee" />
                        <asp:BoundField DataField="OtherExpense" HeaderText="OtherExpense" SortExpression="OtherExpense" />
                        <asp:BoundField DataField="CalculatedComTax" HeaderText="CalculatedComTax" SortExpression="CalculatedComTax" />
                        <asp:BoundField DataField="ActualComTax" HeaderText="ActualComTax" SortExpression="ActualComTax" />
                        <asp:BoundField DataField="CalculatedCustomTax" HeaderText="Custom Duty" SortExpression="CalculatedCustomTax" />
                        <asp:BoundField DataField="ActualCustomTax" HeaderText="ActualCustomTax" SortExpression="ActualCustomTax" />
                        <asp:BoundField DataField="TotalCNFCharge" HeaderText="TotalCNFCharge" SortExpression="TotalCNFCharge" />

                    </Columns>
                    <EmptyDataTemplate>
                        <center>No cnf tax records found!</center>
                    </EmptyDataTemplate>
                </asp:GridView>


                <asp:SqlDataSource ID="SqlDataSource5" runat="server"
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                    SelectCommand="SELECT [CalculatedTaxBase], [ActualTaxBase], [VATRate], [VATAmount], [AITRate], [AITAmount], [DocumentProcessing], [CSFee], [OtherExpense], [CalculatedComTax], [ActualComTax], (SELECT  TotalDutyActual FROM   LC_Duty_Calc  WHERE (LCNo = @LCNo)) as [CalculatedCustomTax], [ActualCustomTax], [TotalCNFCharge] FROM [LC_CNFTax_Calc] WHERE ([LCNo] = @LCNo)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblLCNo" Name="LCNo" PropertyName="Text" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>





                <legend style="margin-bottom: 4px; margin-top: 5px;">Insurance</legend>

            <div class="table-responsive">
                <asp:GridView ID="GridView7" runat="server" AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource6" Width="200%" CssClass="table xtable-b">
                    <Columns>
                        <asp:BoundField DataField="BaseUSD" HeaderText="BaseUSD" SortExpression="BaseUSD" />
                        <asp:BoundField DataField="ExchRate" HeaderText="ExchRate" SortExpression="ExchRate" />
                        <asp:BoundField DataField="BaseBDT" HeaderText="BaseBDT" SortExpression="BaseBDT" />
                        <asp:BoundField DataField="VatAmt" HeaderText="VatAmt" SortExpression="VatAmt" />
                        <asp:BoundField DataField="MarineRate" HeaderText="MarineRate" SortExpression="MarineRate" />
                        <asp:BoundField DataField="MarineAmt" HeaderText="MarineAmt" SortExpression="MarineAmt" />
                        <asp:BoundField DataField="WarSrccRate" HeaderText="WarSrccRate" SortExpression="WarSrccRate" />
                        <asp:BoundField DataField="WarSrccAmt" HeaderText="WarSrccAmt" SortExpression="WarSrccAmt" />
                        <asp:BoundField DataField="VatRate" HeaderText="VatRate" SortExpression="VatRate" />
                        <asp:BoundField DataField="StampDutyRate" HeaderText="StampDutyRate" SortExpression="StampDutyRate" />
                        <asp:BoundField DataField="StampDutyAmount" HeaderText="StampDutyAmount" SortExpression="StampDutyAmount" />
                        <asp:BoundField DataField="CalculatedInsuranceAmt" HeaderText="CalculatedInsuranceAmt" SortExpression="CalculatedInsuranceAmt" />
                        <asp:BoundField DataField="ActualInsuranceAmt" HeaderText="ActualInsuranceAmt" SortExpression="ActualInsuranceAmt" />

                    </Columns>
                    <EmptyDataTemplate>
                        <center>No Insurance records found!</center>
                    </EmptyDataTemplate>
                </asp:GridView>


                <asp:SqlDataSource ID="SqlDataSource6" runat="server"
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                    SelectCommand="SELECT [BaseUSD], [ExchRate], [BaseBDT], [VatAmt], [MarineRate], [MarineAmt], [WarSrccRate], [WarSrccAmt], [VatRate], [StampDutyRate], [StampDutyAmount], [CalculatedInsuranceAmt], [ActualInsuranceAmt] FROM [LC_Insur_Calc] WHERE ([LCNo] = @LCNo)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblLCNo" Name="LCNo" PropertyName="Text" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>



                <legend style="margin-bottom: 4px; margin-top: 5px;">Bank Charges</legend>

            <div class="table-responsive">
                <asp:GridView ID="GridView8" runat="server" AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource7" Width="200%" CssClass="table xtable-b">
                    <Columns>
                        <asp:BoundField DataField="CFRUSD" HeaderText="CFRUSD" SortExpression="CFRUSD" />
                        <asp:BoundField DataField="ExchRate" HeaderText="ExchRate" SortExpression="ExchRate" />
                        <asp:BoundField DataField="CFRBDT" HeaderText="CFRBDT" SortExpression="CFRBDT" />
                        <asp:BoundField DataField="LTR" HeaderText="LTR" SortExpression="LTR" />
                        <asp:BoundField DataField="Margin" HeaderText="Margin" SortExpression="Margin" />
                        <asp:BoundField DataField="CommRate" HeaderText="CommRate" SortExpression="CommRate" />
                        <asp:BoundField DataField="CommAmt" HeaderText="CommAmt" SortExpression="CommAmt" />
                        <asp:BoundField DataField="ProcessingRate" HeaderText="ProcessingRate" SortExpression="ProcessingRate" />
                        <asp:BoundField DataField="ProcessingAmt" HeaderText="ProcessingAmt" SortExpression="ProcessingAmt" />
                        <asp:BoundField DataField="VatRate" HeaderText="VatRate" SortExpression="VatRate" />
                        <asp:BoundField DataField="VatAmt" HeaderText="VatAmt" SortExpression="VatAmt" />
                        <asp:BoundField DataField="AmendmentAmt" HeaderText="AmendmentAmt" SortExpression="AmendmentAmt" />
                        <asp:BoundField DataField="SwiftAmt" HeaderText="SwiftAmt" SortExpression="SwiftAmt" />

                        <asp:BoundField DataField="OtherAmt" HeaderText="OtherAmt" SortExpression="OtherAmt" />
                        <asp:BoundField DataField="TotalCharge" HeaderText="TotalCharge" SortExpression="TotalCharge" />
                        <asp:BoundField DataField="InterestRate" HeaderText="InterestRate" SortExpression="InterestRate" />
                        <asp:BoundField DataField="InterestAmt" HeaderText="InterestAmt" SortExpression="InterestAmt" />
                        <asp:BoundField DataField="Tenor" HeaderText="Tenor" SortExpression="Tenor" />
                        <asp:BoundField DataField="CalculatedInterest" HeaderText="CalculatedInterest" SortExpression="CalculatedInterest" />
                        <asp:BoundField DataField="CalcTotal" HeaderText="CalcTotal" SortExpression="CalcTotal" />
                        <asp:BoundField DataField="ActualInterest" HeaderText="ActualInterest" SortExpression="ActualInterest" />

                    </Columns>
                    <EmptyDataTemplate>
                        <center>No Bank Charges records found!</center>
                    </EmptyDataTemplate>
                </asp:GridView>


                <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                    SelectCommand="SELECT [CFRUSD], [ExchRate], [CFRBDT], [LTR], [Margin], [CommRate], [CommAmt], [ProcessingRate], [ProcessingAmt], [VatRate], [VatAmt], [AmendmentAmt], [SwiftAmt], [OtherAmt], [TotalCharge], [InterestRate], [InterestAmt], [Tenor], [CalculatedInterest], [CalcTotal], [ActualInterest] FROM [LC_Bank_Calc] WHERE ([LCNo] = @LCNo)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblLCNo" Name="LCNo" PropertyName="Text" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>




                <legend style="margin-bottom: 4px; margin-top: 5px;">Other Expenses</legend>

            <div class="table-responsive">
                <asp:GridView ID="GridView9" runat="server" AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource8" Width="100%" CssClass="table xtable-b">
                    <Columns>
                        <asp:BoundField DataField="Expdate" HeaderText="Date" SortExpression="Expdate" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="TypeID" HeaderText="Exp. Type" SortExpression="TypeID" />
                        <asp:BoundField DataField="HeadID" HeaderText="Exp. Head" SortExpression="HeadID" />
                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />

                    </Columns>
                    <EmptyDataTemplate>
                        <center>No Bank Charges records found!</center>
                    </EmptyDataTemplate>
                </asp:GridView>


                <asp:SqlDataSource ID="SqlDataSource8" runat="server"
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                    SelectCommand="SELECT (Select ExpTypeName from ExpTypes WHERE ExpTypesID=a.TypeID) as [TypeID],  
                    (Select HeadName from ExpenseHeads WHERE HeadID=a.HeadID) as [HeadID], [Expdate], [Description], [Amount] 
                    FROM [LC_Expenses] a WHERE ([LCno] = @LCno) ORDER BY [esl]">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblSl" Name="LCno" PropertyName="Text" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>


            <%--<div class="table-responsive">
                <legend style="margin-bottom: 4px; margin-top: 5px;">Other Expenses</legend>
                
                <asp:GridView ID="GridView10" runat="server" AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource9" Width="200%"  CssClass="table xtable-b">
                    <Columns>
                                        <asp:BoundField DataField="TypeID" HeaderText="TypeID" SortExpression="TypeID" />
                                        <asp:BoundField DataField="HeadID" HeaderText="HeadID" SortExpression="HeadID" />
                                        <asp:BoundField DataField="Expdate" HeaderText="Expdate" SortExpression="Expdate" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />

                    </Columns>
                    <EmptyDataTemplate>
                        <center>No Bank Charges records found!</center>
                    </EmptyDataTemplate>
                </asp:GridView>


                <asp:SqlDataSource ID="SqlDataSource9" runat="server"
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                     SelectCommand="SELECT [TypeID], [HeadID], [Expdate], [Description], [Amount] FROM [LC_Expenses] WHERE ([LCno] = @LCno) ORDER BY [esl]">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="lblLCNo" Name="LCno" PropertyName="Text" Type="String" />
                                    </SelectParameters>
                </asp:SqlDataSource>
                </div>--%>




                <legend style="margin-bottom: 4px; margin-top: 5px;">LC Item Costing Information</legend>

            <div class="table-responsive">
                <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource3" Width="300%" CssClass="table xtable-b">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="20px">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>

                            <ItemStyle Width="20px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" />
                        <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                        <asp:BoundField DataField="ItemCode" HeaderText="Item Code" SortExpression="ItemCode" Visible="false" />
                        <asp:BoundField DataField="ItemName" HeaderText="Item Name" SortExpression="ItemName" />
                        <asp:BoundField DataField="qty" HeaderText="Qty." SortExpression="qty" />
                        <asp:BoundField DataField="unit" HeaderText="Unit" SortExpression="unit" />
                        <asp:BoundField DataField="BankBDT" HeaderText="Bank BDT" SortExpression="BankBDT" />
                        <asp:BoundField DataField="CustomDuty" HeaderText="Total Custom Duty" SortExpression="CustomDuty" />
                        <asp:BoundField DataField="VatAtv" HeaderText="VAT+ ATV" SortExpression="VatAtv" />
                        <asp:BoundField DataField="VatAtvAit" HeaderText="VAT+ ATV+ AIT" SortExpression="VatAtvAit" />
                        <asp:BoundField DataField="CnfComTax" HeaderText="CNF Com.Tax" SortExpression="CnfComTax" />
                        <asp:BoundField DataField="Insurance" HeaderText="Insurance" SortExpression="Insurance" />
                        <asp:BoundField DataField="CnfCharge" HeaderText="CNF Charge" SortExpression="CnfCharge" />
                        <asp:BoundField DataField="LcOpCost" HeaderText="LC Op.Cost" SortExpression="LcOpCost" />
                        <asp:BoundField DataField="BankInterest" HeaderText="Bank Interest" SortExpression="BankInterest" />
                        <asp:BoundField DataField="Others" HeaderText="Others" SortExpression="Others" />
                        <asp:BoundField DataField="TotalImportCost" HeaderText="Total Import Cost" SortExpression="TotalImportCost" />
                        <asp:BoundField DataField="CostPerUnit" HeaderText="Cost/Unit" SortExpression="CostPerUnit" Visible="false" />
                        <asp:BoundField DataField="CostPerUnitVAA" HeaderText="Cost/Unit w/o VAT+ATV+ATI" SortExpression="CostPerUnitVAA" />
                        <asp:BoundField DataField="CostPerUnitVA" HeaderText="Cost/Unit w/o VAT+ATV" SortExpression="CostPerUnitVA" />

                    </Columns>
                    <EmptyDataTemplate>
                        <center>No costing calculation records found!</center>
                    </EmptyDataTemplate>
                </asp:GridView>


                <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                    SelectCommand="SELECT 
                                                (SELECT GradeName FROM [ItemGrade] where GradeID=(SELECT GradeID FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=LC_Items_Costing.ItemCode))) As Grade, 
                                                (SELECT CategoryName FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=LC_Items_Costing.ItemCode)) As Category, 
                                                [ItemCode], [ItemName], [qty], [unit], [BankBDT], [CustomDuty], [VatAtv], [VatAtvAit], [CnfComTax], [Insurance], [CnfCharge], [LcOpCost], [BankInterest], [Others], 
                    [TotalImportCost], [CostPerUnit], [CostPerUnitVAA], [CostPerUnitVA] FROM [LC_Items_Costing] WHERE ([LCNo] = @LCNo) ORDER BY [EntryID]">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblLCNo" Name="LCNo" PropertyName="Text" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

            <div class="form-actions">
                <asp:Button ID="btnFirst" CssClass="btn blue" runat="server" Text="<< First" OnClick="btnFirst_Click" />
                <asp:Button ID="btnPrevious" CssClass="btn blue" runat="server" Text="< Previous" OnClick="btnPrevious_Click" />
                <asp:Button ID="btnNext" CssClass="btn blue" runat="server" Text="Next >" OnClick="btnNext_Click" />
                <asp:Button ID="btnLast" CssClass="btn blue" runat="server" Text="Last >>" OnClick="btnLast_Click" />

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>


