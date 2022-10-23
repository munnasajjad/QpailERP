<%@ Page Title="LC Finalization" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="LC-Finalization.aspx.cs" Inherits="app_LC_Finalization" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        #ctl00_BodyContent_ItemGrid td, #ctl00_BodyContent_ItemGrid th {
            font-size: 12px !important;
            line-height: 20px;
            padding: 4px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>--%>

    <script type="text/javascript" language="javascript">
        Sys.Application.add_load(callJquery);
    </script>


    <div class="row">
        <div class="col-md-12">
            <h3 class="page-title">LC Item Received
            </h3>
        </div>
    </div>
    <div class="row">
        <div class="col-md-5">
            <div class="portlet box red">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Finalize LC Received Items
                    </div>
                </div>
                <div class="portlet-body form">

                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>



                    <div class="control-group hidden">
                        <label class="control-label full-wdth">Task :  </label>
                        <div class="controls">
                            <asp:RadioButton ID="rbClose" runat="server" Checked="True" GroupName="g" Text="Close LC" AutoPostBack="True" OnCheckedChanged="rbClose_OnCheckedChanged" />
                            <asp:RadioButton ID="rbReopen" runat="server" Checked="False" GroupName="g" Text="Re-Open for Edit" AutoPostBack="True" OnCheckedChanged="rbClose_OnCheckedChanged" />
                            <asp:Label ID="lblLoadType" runat="server" Text="A" Visible="False"></asp:Label>
                        </div>
                    </div>



                    <div id="EditField" runat="server" class="control-group">
                        <b style="color: #E84940">
                            <label id="lblEname" runat="server">LC Number</label>
                            <asp:DropDownList ID="ddName" runat="server"
                                AutoPostBack="True" DataSourceID="SqlDataSource1x"
                                DataTextField="LCNo" DataValueField="sl"
                                OnSelectedIndexChanged="ddName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1x" runat="server"
                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                SelectCommand="SELECT sl, [LCNo] FROM [LC] WHERE ([IsActive] = @IsActive) ORDER BY [sl] desc">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="lblLoadType" DefaultValue="A" Name="IsActive" PropertyName="Text" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </b>
                    </div>


                    <div class="control-group">
                        <label class="control-label">L/C Type :  </label>
                        <div class="controls">
                            <div class="controls">
                                <asp:TextBox ID="lblLCType" runat="server" Enabled="false" />

                            </div>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">Open Date :  </label>
                        <div class="controls">
                            <asp:TextBox ID="lblOpDate" runat="server" Enabled="false" />
                        </div>
                    </div>

                    <%--<div class="control-group">
                                        <label class="control-label">Item group :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="lblGrp" runat="server" Enabled="false" />
                                        </div>
                                    </div>--%>

                    <div class="control-group">
                        <label class="control-label">For Company/Dept :  </label>
                        <div class="controls">
                            <asp:TextBox ID="lblDept" runat="server" Enabled="false" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">Expiry Date :  </label>
                        <div class="controls">
                            <asp:TextBox ID="lblExDate" runat="server" Enabled="false" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">Ship Date :  </label>
                        <div class="controls">
                            <asp:TextBox ID="lblShipDate" runat="server" Enabled="false" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">Arrival  Date :  </label>
                        <div class="controls">
                            <asp:TextBox ID="lblArrivalDt" runat="server" Enabled="false" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">Port Delivery  Date :  </label>
                        <div class="controls">
                            <asp:TextBox ID="lblDeliveryDt" runat="server" Enabled="false" />
                        </div>
                    </div>



                    <div class="control-group">
                        <label class="control-label">Manufacturer :  </label>
                        <div class="controls">
                            <asp:TextBox ID="lblSupplier" runat="server" Enabled="false" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">Country of Origin :  </label>
                        <div class="controls">
                            <asp:TextBox ID="lblCountry" runat="server" Enabled="false" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">Agent/ Shipper :  </label>
                        <div class="controls">
                            <asp:TextBox ID="lblagent" runat="server" Enabled="false" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">CNF Agent :  </label>
                        <div class="controls">
                            <asp:TextBox ID="txtCNF" runat="server" Enabled="false" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">Insurance :  </label>
                        <div class="controls">
                            <asp:TextBox ID="txtInsurance" runat="server" Enabled="false" />
                        </div>
                    </div>


                    <div class="control-group">
                        <label class="control-label">Total Weight/Qnty :  </label>
                        <div class="controls">
                            <asp:TextBox ID="txtTtlQty" runat="server" Enabled="false" />
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Freight Amt :  </label>
                        <div class="controls">
                            <asp:TextBox ID="txtFreight" runat="server" Enabled="false" />
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Mode of Transport :  </label>
                        <div class="controls">
                            <asp:TextBox ID="ddMode" runat="server" Enabled="false" />
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Remark :  </label>
                        <div class="controls">
                            <asp:TextBox ID="txtRemarks" runat="server" Enabled="false" />
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Stock-in Godown : </label>
                        <asp:DropDownList ID="ddGodown" runat="server" DataSourceID="SqlDataSource5"
                            DataTextField="StoreName" DataValueField="WID" AutoPostBack="True" CssClass="form-control">
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

                    <div class="form-actions">
                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_OnClick" />
                    </div>


                </div>
            </div>
        </div>




        <div class="col-md-7 ">
            <div class="portlet box green">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>LC Items to Stock-in
                    </div>
                    <div class="tools">
                        <a href="" class="collapse"></a>
                        <a href="#portlet-config" data-toggle="modal" class="config"></a>
                        <a href="" class="reload"></a>
                        <a href="" class="remove"></a>
                    </div>
                </div>

                <div class="table-responsive">
                    <asp:Literal ID="ltrLC" runat="server" Visible="False"></asp:Literal>
                    <asp:GridView ID="ItemGrid" runat="server" AutoGenerateColumns="False" AllowSorting="true" OnRowDataBound="GridView1_OnRowDataBound"
                        DataSourceID="SqlDataSource2" ShowFooter="True" CssClass="table-striped table-hover table-bordered" Width="300%">
                        <Columns>
                            <%--<asp:CommandField ButtonType="Image" ControlStyle-Width="24px" DeleteImageUrl="~/App_Themes/Blue/img/error-s.png" ShowDeleteButton="True"  SelectImageUrl="~/app/images/edit.png" ShowSelectButton="true" />--%>

                            <asp:TemplateField ItemStyle-Width="20px">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" />
                            <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />

                            <asp:TemplateField HeaderText="ItemCode" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="ItemCode" HeaderText="Item Code" SortExpression="ItemCode" Visible="false" />
                            <asp:BoundField DataField="ItemName" HeaderText="Item Name" SortExpression="ItemName" />
                            <asp:BoundField DataField="qty" HeaderText="Qty." SortExpression="qty" />
                            <asp:TemplateField HeaderText="Received Qty" SortExpression="qty" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtReceivedQty" runat="server" Text='<%# Bind("qty") %>' ReadOnly="False" Width="60px" CssClass="val1"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="unit" HeaderText="Unit" SortExpression="unit" />
                            <asp:BoundField DataField="BankBDT" HeaderText="FOB/ Bank BDT" SortExpression="BankBDT" />
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

                        <FooterStyle CssClass="" HorizontalAlign="Right" BorderStyle="Solid" Font-Bold="True"></FooterStyle>
                    </asp:GridView>


                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                        SelectCommand="SELECT 
                                                (SELECT GradeName FROM [ItemGrade] where GradeID=(SELECT GradeID FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=LC_Items_Costing.ItemCode))) As Grade, 
                                                (SELECT CategoryName FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=LC_Items_Costing.ItemCode)) As Category, 
                                                [ItemCode], [ItemName], [qty], [unit], [BankBDT], [CustomDuty], [VatAtv], [VatAtvAit], [CnfComTax], [Insurance], [CnfCharge], [LcOpCost], [BankInterest], [Others], [TotalImportCost], [CostPerUnit], [CostPerUnitVAA], [CostPerUnitVA] FROM [LC_Items_Costing] WHERE ([LCNo] = @LCNo) ORDER BY [EntryID]">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ltrLC" Name="LCNo" PropertyName="Text" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                </div>
            </div>
        </div>
    </div>
   <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>


</asp:Content>


