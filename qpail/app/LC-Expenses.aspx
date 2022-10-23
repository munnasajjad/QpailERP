<%@ Page Title="LC Costing" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="LC-Expenses.aspx.cs" Inherits="app_LC_Expenses" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/screen.css" rel="stylesheet" />


    <style type="text/css">
  
        .step-dark-left, .step-light-left {
            line-height: 26px !important;
            padding: 6px 10px 6px 20px !important;
            font-size: 14px !important;
        }

        table, table#ctl00_BodyContent_GridView2 {
            border-collapse: collapse;
        }

            table, th, td, table#ctl00_BodyContent_GridView2 {
                border: 1px solid black;
            }

        .form-group label, .control-group label {
            width: 20% !important;
        }

        td span, td input {
            width: 100% !important;
        }

        .control-group input, .control-group select {
            width: 29% !important;
            margin-right: 10px;
        }

        input#ctl00_BodyContent_txtOtherDesc {
            width: 182px !important;
        }

        input#ctl00_BodyContent_txtOtherAmount {
            width: 100px !important;
        }

        .ActiveStep {
            color: gold !important;
        }

        div#content {
            margin-top: -37px !important;
        }

        table#ctl00_BodyContent_GridView2 input {
            text-align: center;
        }
    </style>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
                Sys.Application.add_load(callJquery);
            </script>


            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">
                        <asp:Literal ID="ltrLC" runat="server"></asp:Literal>
                    </h3>

                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box red">

                        <div class="portlet-body form">

                            <div class="form-body">


                                <!--  start step-holder -->
                                <div id="step-holder">

                                    <div class="step-dark-left" id="Div9" runat="server">
                                        <asp:LinkButton ID="LinkButton0" runat="server" CssClass="ActiveStep" OnClick="LinkButton0_Click">LC Info.</asp:LinkButton>
                                    </div>
                                    
                                     <div class="step-dark-right" id="Div10" runat="server">&nbsp;</div>
                                    <div class="step-no" id="step3" runat="server">1</div>
                                    <div class="step-light-left" id="step3l" runat="server">
                                        <asp:LinkButton ID="LinkButton3" runat="server" OnClick="LinkButton3_Click">Insurance</asp:LinkButton>
                                    </div>
                                    
                                      <div class="step-light-right" id="step1r" runat="server">&nbsp;</div>
                                    <div class="step-no-off" id="Div1" runat="server">2</div>
                                    <div class="step-light-left" id="step4l" runat="server">
                                        <asp:LinkButton ID="LinkButton4" runat="server"  Enabled="False" OnClick="LinkButton4_Click">Bank Charges</asp:LinkButton>
                                    </div>

                                    <div class="step-light-right hidden" id="step2r" runat="server">&nbsp;</div>
                                    <div class="step-no-off hidden" id="step2" runat="server">2</div>
                                    <div class="step-light-left hidden" id="step2l" runat="server">
                                        <asp:LinkButton ID="LinkButton2" runat="server" Enabled="false" OnClick="LinkButton2_Click">CNF Comm. Tax</asp:LinkButton>
                                    </div>                                    

                                   
                                    
                                    <div class="step-light-right" id="step3r" runat="server">&nbsp;</div>
                                    <div class="step-no-off">3</div>
                                    <div class="step-light-left" id="step1l" runat="server">
                                        <asp:LinkButton ID="LinkButton1" runat="server" Enabled="false" OnClick="LinkButton1_Click">Duty</asp:LinkButton>
                                    </div>
                                    
                                    <div class="step-light-right" id="step8r" runat="server">&nbsp;</div>
                                    <div class="step-no-off">4</div>
                                    <div class="step-light-left" id="step8l" runat="server">
                                        <asp:LinkButton ID="LinkButton8" runat="server"  Enabled="False" OnClick="LinkButton8_OnClick">CNF Comm.</asp:LinkButton>
                                    </div>
                                    
                                    <div class="step-light-right" id="step7r" runat="server">&nbsp;</div>
                                    <div class="step-no-off">5</div>
                                    <div class="step-light-left" id="step7l" runat="server">
                                        <asp:LinkButton ID="LinkButton7" runat="server"  Enabled="False" OnClick="LinkButton7_OnClick">Transport</asp:LinkButton>
                                    </div>
                                    

                                   <%--  <div class="step-light-right" id="step1r" runat="server">&nbsp;</div>
                                    <div class="step-no-off" id="Div1" runat="server">5</div>
                                    <div class="step-light-left" id="step4l" runat="server">
                                        <asp:LinkButton ID="LinkButton4" runat="server"  Enabled="False" OnClick="LinkButton4_Click">Bank Charges</asp:LinkButton>
                                    </div>--%>


                                    <div class="step-light-right" id="step4r" runat="server">&nbsp;</div>
                                    <div class="step-no-off" id="Div5" runat="server">6</div>
                                    <div class="step-light-left" id="step5l" runat="server">
                                        <asp:LinkButton ID="LinkButton5" runat="server"  Enabled="False" OnClick="LinkButton5_Click">Other Expenses </asp:LinkButton>
                                    </div>

                                    
                                    <div class="step-light-right" id="step5r" runat="server">&nbsp;</div>
                                    <div class="step-no-off" id="step4" runat="server">7</div>
                                    <div class="step-light-left" id="step6l" runat="server">
                                        <asp:LinkButton ID="LinkButton6" runat="server" Enabled="False" OnClick="LinkButton6_Click">LC Costing</asp:LinkButton>
                                    </div>
                                    <div class="step-light-round" id="step6r" runat="server">&nbsp;</div>
                                    <div class="clear">
                                        
                                    </div>
                                </div>

                                <!--  end step-holder -->

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                            </div>
                        </div>
                    </div>
                </div>
            </div>



            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box red">
                        <div class="portlet-body form">

                            <div class="form-body">
                                <asp:Label ID="lblMsg1" runat="server" EnableViewState="false"></asp:Label>


                                <%--General Info--%>
                                <asp:Panel ID="Panel0" runat="server">

                                    <div id="EditField" runat="server" class="control-group">
                                        <b style="color: #E84940">
                                            <label id="lblEname" runat="server">LC Number</label>
                                            <asp:DropDownList ID="ddName" runat="server" AppendDataBoundItems="True"
                                                AutoPostBack="True" DataSourceID="SqlDataSource1x"
                                                DataTextField="LCNo" DataValueField="sl"
                                                OnSelectedIndexChanged="ddName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource1x" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT sl, [LCNo] FROM [LC] WHERE ([IsActive] = 'A') ORDER BY [sl] desc"></asp:SqlDataSource>
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

                                    <div class="form-actions">
                                        <asp:Button ID="Button1" CssClass="btn blue" runat="server" Text="Next" OnClick="Button1_OnClick" />
                                    </div>


                                </asp:Panel>

                                

                                <%--Insurance--%>

                                <asp:Panel ID="Panel3" runat="server" Visible="false">


                                    <div class="control-group">
                                        <label class="control-label">Insurance Name :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtInsurance" runat="server" Enabled="True"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="InsuranceIdHField"/>
                                        </div>
                                    </div>


                                    <div class="control-group">
                                        <label class="control-label">Base USD (110% of CFR)  :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblAVInsurance" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">USD Exch. Rate :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtInsExRate" runat="server" OnTextChanged="txtInsExRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Insurance Base (BDT) :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtInsurBDT" runat="server" ReadOnly="true" />
                                        </div>
                                    </div>

                                    <fieldset><legend>Insurance Commission Calculations</legend></fieldset>


                                    <div class="control-group">
                                        <label class="control-label">Marine  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtMarine" runat="server" OnTextChanged="txtInsExRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="Label7" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">War+SRCC  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtWarSRCC" runat="server" OnTextChanged="txtInsExRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="Label8" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">VAT (%)  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtVATInsur" runat="server" OnTextChanged="txtInsExRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="Label5" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Stamp Duty  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtStampDuty" runat="server" OnTextChanged="txtInsExRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="Label6" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>


                                    <div class="control-group ">
                                        <label class="control-label">Calculated Insurance Amt : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCalcInsur" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    
                                      <div class="control-group">
                                        <label class="control-label">Discount on (Marine+War+SRCC) (%) :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtRevatOnPremium" Text="0" runat="server" AutoPostBack="true" OnTextChanged="txtRevatOnPremium_OnTextChanged"></asp:TextBox>
                                            <asp:Label ID="LblDiscountAmt" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Actual Insurance Amount :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtActInsur" runat="server" OnTextChanged="txtInsExRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtActInsur">
                                            </asp:FilteredTextBoxExtender>
                                        </div>
                                    </div>


                                    <div class="form-actions">
                                        <asp:Button ID="btnPanel3" CssClass="btn blue" runat="server" Text="Next" OnClick="btnPanel3_Click" />
                                    </div>

                                </asp:Panel>



                                <%--Duty Info--%>

                                <asp:Panel ID="Panel1" runat="server" Visible="false">

                                    <fieldset><legend>Assessable Value Calculation</legend></fieldset>

                                    <div class="control-group2">
                                        <div class="portlet-body form">

                                            <div class="form-body" style="margin-bottom: 20px">

                                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView2_SelectedIndexChanged"
                                                    DataSourceID="SqlDataSource9" Width="100%" DataKeyNames="EntryID" RowStyle-VerticalAlign="Middle">
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

                                                        <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" />
                                                        <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />

                                                        <asp:TemplateField HeaderText="Product Name" SortExpression="Product">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Product") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" />
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
                                                                <asp:Label ID="lblCFRValue" runat="server" Text='<%# Bind("CFRValue") %>'></asp:Label>
                                                            </ItemTemplate>

                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Loading">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLoading" runat="server" Text='<%# Bind("Loading") %>' Width="100%"
                                                                    OnTextChanged="txtQty_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="7%" CssClass="xerp_absolute_centre" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Loaded" SortExpression="UnitPrice">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLoaded" runat="server" Text='<%# Bind("Loaded") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Landing(%)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLanding" runat="server" Text='<%# Bind("LandingPercent") %>' Width="100%"
                                                                    OnTextChanged="txtQty_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="8%" CssClass="xerp_absolute_centre" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Total (USD)" SortExpression="UnitPrice">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("TotalUSD") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="30px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Approve" ToolTip="Select" Width="12px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                    </Columns>
                                                    <SelectedRowStyle BackColor="#FCD209" Font-Bold="True" ForeColor="#D64102" />
                                                </asp:GridView>


                                                <asp:SqlDataSource ID="SqlDataSource9" runat="server"
                                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT EntryID, (SELECT GradeName FROM [ItemGrade] where GradeID=(SELECT GradeID FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=a.ItemCode))) As Grade, 
                                                (SELECT CategoryName FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=a.ItemCode)) As Category, (Select ItemName from Products where ProductID=a.ItemCode) as Product,                      
                                                                                    [HSCode], (Select BrandName from Brands where BrandID=a.ItemSizeID) as Size, Measurement, 
                                                                                    CONVERT(varchar(10), qty) +' '+(Select UnitType from Products where ProductID=a.ItemCode) As QTY1, UnitPrice,  [CFRValue],
                                                                                                        Loading, Loaded, LandingPercent, LandingAmt, TotalUSD, TotalBDT
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

                                    <asp:Label ID="lblItemEntryId" runat="server" Text="" Visible="false"></asp:Label>

                                    <div class="control-group">
                                        <label class="control-label">Item Total (USD) :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtiTtlUSD" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Custom Exch Rate :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCustomExRate" runat="server" OnTextChanged="txtQty_TextChanged" AutoPostBack="true" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Item Total (BDT)  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtiTtlBDT" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Insurance Amount (1%) :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtInsurPercent" runat="server" OnTextChanged="txtDuties_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="txtInsurAmount" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Other Duties/Penalties :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtOtherDesc" runat="server" placeholder="Description" Width="100px"></asp:TextBox>

                                            <asp:TextBox ID="txtOtherAmount" runat="server" placeholder="Amount" OnTextChanged="txtDuties_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="control-group">
                                        <label class="control-label">Assessable Value (Calculated) :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtAVCalc" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="control-group">
                                        <label class="control-label">Assessable Value (Actual) :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtAVActual" runat="server" OnTextChanged="txtDuties_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>


                                    <fieldset><legend>Duty Calculations</legend></fieldset>
                                    <asp:Label ID="lblMsg2" runat="server" Text="" EnableViewState="false"></asp:Label>


                                    <div class="control-group">
                                        <%--<label class="control-label">CD (Customs Duty) Rate (%) :  </label>--%>
                                        <label class="control-label">CD (Customs Duty) :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCDRate" runat="server" OnTextChanged="txtDuties_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="txtCDAmt" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">RD (Regulatory Duty) :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtRDRate" runat="server" OnTextChanged="txtDuties_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="txtRDAmt" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">SD (Supplementary Duty)  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtSDRate" runat="server" OnTextChanged="txtDuties_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="txtSDAmt" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group hidden">
                                        <label class="control-label">Sur-Charge Rate  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtSurChRate" runat="server" OnTextChanged="txtDuties_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="txtSurChAmt" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <%--<label class="control-label">VAT (%)  :  </label>--%>
                                        <label class="control-label">VAT :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtVATRate" runat="server" OnTextChanged="txtDuties_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="txtVATAmt" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">AIT   :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtAITRate" runat="server" OnTextChanged="txtDuties_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="txtAITAmt" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>
                                    
                                    <div class="control-group">
                                        <label class="control-label">AT :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtATAmt" runat="server" Text="0.00" OnTextChanged="txtATAmt_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">ATV   :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtATVRate" runat="server" OnTextChanged="txtDuties_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="txtATVAmt" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>
                                     <div class="control-group">
                                        <label class="control-label">DF/CVAT/FP  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDFCVATFP" runat="server" Text="0.00" OnTextChanged="txtDFCVATFP_OnTextChanged"  AutoPostBack="true"></asp:TextBox>
                                            <%--<asp:Label ID="lblDFCVATFP" runat="server" Text="0.00"></asp:Label>--%>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Total Calculated Duty  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTtlCalcDuty" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Actual Duty Amount :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTtlActuDuty" runat="server" OnTextChanged="txtTtlActuDuty_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-actions">
                                        <asp:Button ID="btnSaveDutyN" CssClass="btn blue" runat="server" Text="Next" OnClick="btnSaveDutyN_Click" />
                                    </div>

                                </asp:Panel>

                                

                                <%--CNF Comm Tax--%>

                                <asp:Panel ID="Panel2" runat="server" Visible="false">

                                    <div class="control-group">
                                        <label class="control-label">CNF Name :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCNF" runat="server" Enabled="True"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Assessable Vaue (AV)  :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblAVActual" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Custom Duty  :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblCDuty" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">CNF Tax Basis  :  </label>
                                        <div class="controls">
                                            <asp:Literal ID="lblCTB" runat="server" Text=""></asp:Literal>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Calculated Tax Base :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTaxBasis" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Actual Tax Base (Customs) :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTaxbasActual" runat="server" OnTextChanged="txtTaxbasActual_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <fieldset><legend>CNF Commission Tax Calculations</legend></fieldset>

                                    <div class="control-group">
                                        <label class="control-label">VAT (%)  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCtbVAT" runat="server" OnTextChanged="txtTaxbasActual_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="Label3" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">AIT (%)  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCtbAIT" runat="server" OnTextChanged="txtTaxbasActual_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="Label4" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Document Processing :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDocProcessing" runat="server" OnTextChanged="txtTaxbasActual_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">C.S. Fee  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCSFee" runat="server" OnTextChanged="txtTaxbasActual_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Other Expense  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCnfOtherExp" runat="server" OnTextChanged="txtTaxbasActual_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Calculated CNF Com. Tax: </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCalculatedCNFCom" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Actual CNF Com. Tax (Cus)  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtActualCNFCom" runat="server" OnTextChanged="txtTaxbasActual_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Total Custom Tax (Duty+CNF) :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTotalCNFDuty" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="form-actions">
                                        <asp:Button ID="btnPanel2" CssClass="btn blue" runat="server" Text="Next" OnClick="btnPanel2_Click" />
                                    </div>

                                </asp:Panel>



                                <%--Transport--%>

                                <asp:Panel ID="panelTransport" runat="server" Visible="false">

                                    <div class="control-group">
                                        <label class="control-label">Transport Agent : </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddTransportAgency" runat="server" DataSourceID="SqlDataSource3"
                                                DataTextField="Company" DataValueField="PartyID">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                               SelectCommand="SELECT PartyID, Company FROM [Party] WHERE Type='transport'"/>
                                        </div>
                                    </div>
                                    
                                    <fieldset><legend>Transport Calculations</legend></fieldset>
                                    <br/>

                                    <div class="control-group">
                                        <label class="control-label">Total Truck  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTotalTruck" runat="server" OnTextChanged="txtTotalTruck_OnTextChanged" AutoPostBack="true" Text="0"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Rate  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTruckRate" runat="server" OnTextChanged="txtTruckRate_OnTextChanged" AutoPostBack="true" Text="0.00"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Total Amount  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTotalTransportAmt" runat="server" OnTextChanged="txtInsExRate_TextChanged" AutoPostBack="true" Text="0.00"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Description :  </label>
                                        <div class="controls">
                                            <asp:TextBox TextMode="MultiLine" OnTextChanged="txtTransportDescription_OnTextChanged" AutoPostBack="True" ID="txtTransportDescription" runat="server" Width="29%"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-actions">
                                        <asp:Button ID="btnSaveTransport" CssClass="btn blue" runat="server" Text="Next" OnClick="btnSaveTransport_OnClick" />
                                    </div>

                                </asp:Panel>
                                
                                <%--CNF Commission new--%>

                                <asp:Panel ID="pnlCNFCommission" runat="server" Visible="false">

                                    <div class="control-group">
                                        <label class="control-label">CNF Agent : </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddCNFAgent" runat="server" DataSourceID="SqlDataSource4"
                                                DataTextField="Company" DataValueField="PartyID">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                               SelectCommand="SELECT PartyID, Company FROM [Party] WHERE Type='cnf'"/>
                                        </div>
                                    </div>
                                    
                                    <fieldset><legend>CNF Commission Entry</legend></fieldset>
                                    <br/>
                                      
                                    <div class="control-group">
                                        <label class="control-label">Port Charge  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtPortCharge" runat="server" OnTextChanged="txtPortCharge_OnTextChanged"  AutoPostBack="true" Text="0"></asp:TextBox>
                                        </div>
                                    </div>
                                    
                                     <div class="control-group">
                                        <label class="control-label">Shipping Charge  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtShippingCharge" runat="server" OnTextChanged="txtShippingCharge_OnTextChanged"  AutoPostBack="true" Text="0"></asp:TextBox>
                                        </div>
                                    </div>
                                    
                                    <div class="control-group">
                                        <label class="control-label">Receipt Amount  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtReceiptCnf" runat="server" OnTextChanged="txtReceiptCnf_OnTextChanged"  AutoPostBack="true" Text="0"></asp:TextBox>
                                        </div>
                                    </div>
                                  
                                    <div class="control-group">
                                        <label class="control-label">Miscellaneous Amount  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtMiscellaneousCnf" runat="server" OnTextChanged="txtMiscellaneousCnf_OnTextChanged"  AutoPostBack="true" Text="0"></asp:TextBox>
                                        </div>
                                    </div>
                                   
                                    <div class="control-group">
                                        <label class="control-label">Other Charge  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtOtherChargeCnf" runat="server" OnTextChanged="txtOtherChargeCnf_OnTextChanged"  AutoPostBack="true" Text="0"></asp:TextBox>
                                        </div>
                                    </div>

                                     <div class="control-group">
                                        <label class="control-label">Commission  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCommissionCnf" runat="server" OnTextChanged="txtCommissionCnf_OnTextChanged"  AutoPostBack="true" Text="0"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Total Amount  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTotalAmtCnf" runat="server"   AutoPostBack="true" Text="0.00"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Description :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDescriptionCnf" TextMode="MultiLine" OnTextChanged="txtTransportDescription_OnTextChanged" AutoPostBack="True"  runat="server" Width="29%"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-actions">
                                        <asp:Button ID="btnCNFCommNew" CssClass="btn blue" runat="server" Text="Next" OnClick="btnCNFCommNew_OnClick" />
                                    </div>

                                </asp:Panel>


                                <%--Bank--%>

                                <asp:Panel ID="Panel4" runat="server" Visible="false">



                                    <div class="control-group">
                                        <label class="control-label">CFR USD  :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblBankCFR" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">BanK Exch Rate :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtBankExchRate" runat="server" OnTextChanged="txtBankExchRate_TextChanged" AutoPostBack="true" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">FOB/ Bank BDT :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtBankBDT" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="control-group">
                                        <label class="control-label">LTR  :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblLTR" runat="server" Text="" ReadOnly="true"></asp:Label>
                                        </div>
                                    </div>


                                    <div class="control-group">
                                        <label class="control-label">Margin  :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblMargin" runat="server" Text="" ReadOnly="true"></asp:Label>
                                        </div>
                                    </div>

                                    <fieldset><legend>LC Opening Charges</legend></fieldset>

                                    <div class="control-group">
                                        <label class="control-label">Commission (%)  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtBankComRate" runat="server" OnTextChanged="txtBankExchRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="Label10" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Processing (%)  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtBankProcessRate" runat="server" OnTextChanged="txtBankExchRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="Label11" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>


                                    <div class="control-group">
                                        <label class="control-label">VAT Amount  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtBankVAT" runat="server" OnTextChanged="txtBankExchRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="Label12" runat="server" Text="0.00" Visible="false"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Amendment: </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtBankAmendment" runat="server" OnTextChanged="txtBankExchRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Swift Charge  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtSwift" runat="server" OnTextChanged="txtBankExchRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Other  :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtBankOther" runat="server" OnTextChanged="txtBankExchRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Total Charges : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtBankttlCharge" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>



                                    <fieldset><legend>Bank Interest</legend></fieldset>

                                    <div class="control-group">
                                        <label class="control-label">Interest  Rate (%): </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtBankInterestRate" runat="server" OnTextChanged="txtBankExchRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="Label9" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Tenor: </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtBankTenor" runat="server" OnTextChanged="txtBankExchRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Calculated Interest Amount : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCalcInterAmt" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group hidden">
                                        <label class="control-label">Total Bank Charge : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtBankTotalCalc" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Bank Interest (Actual) :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtActBankInterAmt" runat="server" OnTextChanged="txtBankExchRate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="form-actions">

                                        <asp:Button ID="btnPanel4" CssClass="btn blue" runat="server" Text="Next" OnClick="btnPanel4_Click" />
                                    </div>

                                </asp:Panel>




                                <%-- CNF --%>

                                <asp:Panel ID="Panel5" runat="server" Visible="false">



                                    <div class="control-group">
                                        <label class="control-label">CNF Commission Taxes :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtCNFComm" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>


                                    <fieldset><legend>Other Expenses</legend></fieldset>

                                    <div class="control-group">
                                        <label class="control-label">Expense Date :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDate" runat="server" Enabled="True"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Expense Type : </label>

                                        <div class="controls">
                                            <asp:DropDownList ID="ddType" runat="server" DataSourceID="SqlDataSource1"
                                                DataTextField="ExpTypeName" DataValueField="ExpTypesID" AutoPostBack="true" OnSelectedIndexChanged="ddType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                               SelectCommand="SELECT ExpTypesID,[ExpTypeName] FROM [ExpTypes] "/>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Expense Head :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddHead" runat="server" DataSourceID="SqlDataSource17" DataTextField="HeadName" DataValueField="HeadID">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource17" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [HeadID], [AccHeadID], [HeadName] FROM [ExpenseHeads] WHERE ([AccHeadID] = @AccHeadID) ORDER BY [HeadName]">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddType" Name="AccHeadID" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Exp. Description :</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDescription" runat="server" Enabled="True"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Amount (BDT) :  </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtAmount" runat="server" Enabled="True"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtAmount">
                                            </asp:FilteredTextBoxExtender>

                                        </div>
                                    </div>


                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click" />
                                        <asp:Label ID="lblEditESL" runat="server" Text="" Visible="false"></asp:Label>
                                    </div>


                                    <div class="table-responsive">

                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowDeleting="GridView1_RowDeleting" AllowSorting="true"
                                            DataSourceID="SqlDataSource10" DataKeyNames="esl" ShowFooter="true" OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>

                                                    <ItemStyle Width="20px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="sl" SortExpression="sl" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("esl") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Expense Date" SortExpression="Expdate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1x" runat="server" Text='<%# Bind("Expdate","{0:d}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Expense Type" SortExpression="ExpType">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Labe7" runat="server" Text='<%# Bind("ExpType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Expense Head" SortExpression="HeadName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1xz" runat="server" Text='<%# Bind("HeadName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" SortExpression="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="desc2" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="CFRBDT" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="Ship Date" SortExpression="CFRValue">
                                    <ItemTemplate>
                                        <asp:Label ID="ShipDate" runat="server" DataFormatString="{0:d}" Text='<%# Bind("ShipDate","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>



                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Approve" ToolTip="Edit" />
                                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.gif" Text="Delete" ToolTip="Delete" />

                                                        <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                        </asp:ConfirmButtonExtender>
                                                        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                            PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                        <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                            <b style="color: red">Item will be deleted permanently!</b><br />
                                                            Are you sure you want to delete the item from order list?<br />
                                                            <br />
                                                            <div style="text-align: right;">
                                                                <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                                <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                            </div>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="24px"></ItemStyle>
                                                </asp:TemplateField>


                                            </Columns>
                                            <HeaderStyle BackColor="Gold" Font-Bold="True" ForeColor="#23A6F0" BorderStyle="Solid" />
                                            <FooterStyle BackColor="#23A6F0" Font-Bold="True" ForeColor="White" BorderStyle="Solid" />

                                            <EmptyDataTemplate>
                                                No Expense History Records Found!
                                            </EmptyDataTemplate>
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        </asp:GridView>


                                        <asp:SqlDataSource ID="SqlDataSource10" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT esl, Expdate, 
                                            (SELECT [AccountsHeadName] FROM [HeadSetup] where AccountsHeadID=  LC_Expenses.TypeID) as ExpType, 
                                            (SELECT HeadName FROM ExpenseHeads where HeadID=  LC_Expenses.HeadID) as HeadName, 
                                            Amount, Description from  LC_Expenses Where LCNo=@LCNo ORDER BY [esl]"
                                            DeleteCommand="DELETE LcItems WHERE EntryID=@EntryID"
                                            UpdateCommand="">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddName" Name="LCNo" PropertyName="SelectedValue" Type="String" />
                                            </SelectParameters>
                                            <DeleteParameters>
                                                <asp:Parameter Name="EntryID" />
                                            </DeleteParameters>
                                        </asp:SqlDataSource>


                                    </div>

                                    <div class="form-actions">

                                        <asp:Button ID="btnNext" CssClass="btn blue" runat="server" Text="Next" OnClick="btnNext_Click" />
                                    </div>


                                </asp:Panel>





                                <%--Costing--%>

                                <asp:Panel ID="Panel6" runat="server" Visible="false">


                                    <h3>LC Costing</h3>
                                    <div>
                                        <div class="control-group">
                                            <label class="control-label">CFR BDT :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtCfrBDT" runat="server" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">FOB/ Bank BDT :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtBankBdt2" runat="server" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Cus+CNF Duty :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtTotalCNFDuty2" runat="server" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Actual Insurance :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtActInsurance" runat="server" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Total CNF Charge :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtCnfCharge" runat="server" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">L/C Opening Cost  :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtBankCost" runat="server" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                        <%-- <div class="control-group">
                                            <label class="control-label">Port/ Shipping :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtShipping" runat="server" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                       <div class="control-group">
                                            <label class="control-label">Transport :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtTransport" runat="server" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Labor :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox25" runat="server" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>--%>
                                        <div class="control-group">
                                            <label class="control-label">Bank Interest :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtBInterest" runat="server" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Miscellaneous Cost :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtMisc" runat="server" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">LC Import Cost :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtLcImpCost" runat="server" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                        <%--<div class="control-group">
                                            <label class="control-label">Unit Cost :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox37" runat="server" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>--%>
                                    </div>

                                    <fieldset><legend>Item Import Cost Calculations</legend></fieldset>
                                    <div class="control-group">
                                        <div class="portlet-body form">

                                            <div class="table-responsive">

                                                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                                    DataSourceID="SqlDataSource2" Width="200%">
                                                    <Columns>
                                                        <%--<asp:CommandField ButtonType="Image" ControlStyle-Width="24px" DeleteImageUrl="~/App_Themes/Blue/img/error-s.png" ShowDeleteButton="True"  SelectImageUrl="~/app/images/edit.png" ShowSelectButton="true" />--%>

                                                        <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" />
                                                        <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />

                                                        <asp:BoundField DataField="ItemCode" HeaderText="Item Code" SortExpression="ItemCode" Visible="false" />
                                                        <asp:BoundField DataField="ItemName" HeaderText="Item Name" SortExpression="ItemName" />
                                                        <asp:BoundField DataField="qty" HeaderText="Qty." SortExpression="qty" />
                                                        <asp:BoundField DataField="unit" HeaderText="Unit" SortExpression="unit" />
                                                        <asp:BoundField DataField="BankBDT" HeaderText="FOB/ Bank BDT" SortExpression="BankBDT" />
                                                        <asp:BoundField DataField="CustomDuty" HeaderText="Total Custom Duty" SortExpression="CustomDuty" />
                                                        <asp:BoundField DataField="VatAtv" HeaderText="VAT+ ATV" SortExpression="VatAtv" />
                                                        <asp:BoundField DataField="VatAtvAit" HeaderText="VAT+ ATV+ AIT" SortExpression="VatAtvAit" />
                                                        <asp:BoundField DataField="CnfComTax" HeaderText="CNF Com.Tax" SortExpression="CnfComTax" />
                                                        <asp:BoundField DataField="Insurance" HeaderText="Insurance" SortExpression="Insurance" />
                                                        <asp:BoundField DataField="CnfCharge" HeaderText="CNF Commission" SortExpression="CnfCharge" />
                                                        <asp:BoundField DataField="LcOpCost" HeaderText="LC Op.Cost" SortExpression="LcOpCost" />
                                                        <asp:BoundField DataField="BankInterest" HeaderText="Bank Interest" SortExpression="BankInterest" />
                                                        <asp:BoundField DataField="Others" HeaderText="Others" SortExpression="Others" />
                                                        <asp:BoundField DataField="TotalImportCost" HeaderText="Total Import Cost" SortExpression="TotalImportCost" />
                                                        <asp:BoundField DataField="CostPerUnit" HeaderText="Cost/Unit" SortExpression="CostPerUnit" Visible="false" />
                                                        <asp:BoundField DataField="CostPerUnitVAA" HeaderText="Cost/Unit w/o VAT+ATV+ATI" SortExpression="CostPerUnitVAA" />
                                                        <asp:BoundField DataField="CostPerUnitVA" HeaderText="Cost/Unit w/o VAT+ATV" SortExpression="CostPerUnitVA" />


                                                    </Columns>
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


                                </asp:Panel>











































































                                <%--Right Panel





                                <div class="col-md-5">
                                    <div class="portlet box green">
                                        <div class="portlet-title">
                                            <div class="caption">
                                                <i class="fa fa-reorder"></i>LC Details
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


                                                <div id="accordion">

                                                    <h3>LC General Info</h3>
                                                    <div>


                                                        <div id="Div2" runat="server" class="control-group">
                                                            <label class="control-label">LC/TT No.</label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="lblLCNo" runat="server" ReadOnly="true" />
                                                            </div>
                                                        </div>




                                                    </div>





                                                   

                                                    <h3>Bank Expenses</h3>
                                                    <div>

                                                        <div class="control-group">
                                                            <label class="control-label">Bank Account No. :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="lblBank" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="control-group">
                                                            <label class="control-label">L/C Margin :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtMargin" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="control-group">
                                                            <label class="control-label">CFR USD :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtCfrUSD" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Bank Exch Rate :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtBExRate" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">CFR BDT :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtCfrBDT" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Total Expense :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="ttlBankExp" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Rate(%) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox49" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Value :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox50" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">LTR :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtLTR" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Interest  Rate (%) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox52" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Tenor :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox53" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Interest Amount :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox54" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <h3>Custom/Duty Expenses</h3>
                                                    <div>

                                                        <legend style="margin-bottom: 6px;">Duty Details</legend>

                                                        <div class="control-group">
                                                            <label class="control-label">Loading :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox9" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="control-group">
                                                            <label class="control-label">Loadfed Value :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtCardSl" runat="server" ToolTip="Pick or Type in MM/DD/YYYY"></asp:TextBox>

                                                            </div>
                                                        </div>

                                                        <div class="control-group">
                                                            <label class="control-label">Landing 1%(1.01) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtName" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="control-group">
                                                            <label class="control-label">Cus Exchange Rate :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="txtCExRate" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Insurance(1%) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox26" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Other Duties /Penalty :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox27" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Assessable Vaue (Cal) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox28" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Assessable Vaue (Cus) AV :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox29" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Duty Head :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox30" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Duty Rate(%) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox31" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Duty ( Cal) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox32" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Duty ( Cus) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox33" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                    
                                <legend style="margin-bottom: 6px;">List of Expenses</legend>
                                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False"
                                    DataSourceID="SqlDataSource13" Width="253px">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DepartmentName" HeaderText="Department Name"
                                            SortExpression="DepartmentName" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource13" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [DepartmentName] FROM [Departments] ORDER BY [DepartmentName]"></asp:SqlDataSource>
                                                      
                                                    </div>
                                                    <h3>Insurance Expenses</h3>
                                                    <div>

                                                        <div class="control-group">
                                                            <label class="control-label">110% of CFR Value :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox38" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Exch Rate :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox39" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Commi Cal Base Value :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox40" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Commi Head :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox41" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Rate (%) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox42" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Total Ins. Commi :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox43" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Actual Insurance  :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox44" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <h3>CNF Expenses</h3>
                                                    <div>

                                                        <legend style="margin-bottom: 6px;">CNF  Commission Tax</legend>

                                                        <div class="control-group">
                                                            <label class="control-label">CNF Tax Basis :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox1" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="control-group">
                                                            <label class="control-label">Tax Base ( Cal) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox2" runat="server" ToolTip="Pick or Type in MM/DD/YYYY"></asp:TextBox>

                                                            </div>
                                                        </div>

                                                        <div class="control-group">
                                                            <label class="control-label">Tax Base (Cus) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox3" runat="server" Enabled="True"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="control-group">
                                                            <label class="control-label">CNF Comm Tax  Head :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">CNF  Comm Tax Rate(%) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">CNF Com  Tax (Cal) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Assessable Vaue (Cal) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">CNF Com Tax (Cus) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Total Cus Tax with CNF :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>

                                                        <legend style="margin-bottom: 6px;">List of Expenses</legend>

                                                        <div class="control-group">
                                                            <label class="control-label">110% of CFR Value :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Exch Rate :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Commi Cal Base Value :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Commi Head :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox14" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Rate ( %) :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox15" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Total Ins. Commi :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox16" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="control-group">
                                                            <label class="control-label">Actual Insurance  :  </label>
                                                            <div class="controls">
                                                                <asp:TextBox ID="TextBox17" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>


