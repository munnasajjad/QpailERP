<%@ Page Language="C#" MasterPageFile="~/app/MasterPage.master" MaintainScrollPositionOnPostback="false" AutoEventWireup="true" CodeFile="LC-Open.aspx.cs" Inherits="app_LC_Open" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .controls span {
            width: 69%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <script type="text/javascript">
        $(document).ready(function () {

            $('#<%=txtCfrBDT.ClientID%>').attr('readonly', true);
            $('#<%=txtBankBDT.ClientID%>').attr('readonly', true);
            //$('#<%=txtMargin.ClientID%>').attr('readonly', true);
            $('#<%=txtLTR.ClientID%>').attr('readonly', true);
            $('#<%=txtCFR.ClientID%>').attr('readonly', true);
            $('#<%=txtQty.ClientID%>').attr('readonly', true);
        });

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
        function calTotalWeight() {
            var qty = $('#<%=txtSerial.ClientID%>').val();
            var rate = $('#<%=txtWarrenty.ClientID%>').val();

            var ttl = parseFloat(qty) * parseFloat(rate);
            $('#<%=txtQty.ClientID%>').val(ttl.toString());
        }

    </script>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">


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
                        <asp:Literal ID="ltrFrmName" runat="server" Text="LC General Info" />
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
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>
                                <asp:Label ID="lblProject" runat="server" Visible="false" Text="1"></asp:Label>
                                <%--<asp:Label ID="lblInvoice" runat="server" Text="" Visible="false" />--%>

                                <div id="divinvoice" runat="server" class="control-group hidden">
                                    <b style="color: #E84940">
                                        <asp:Label ID="lblEname" runat="server" Text="Amendment LC# "></asp:Label>
                                        <asp:DropDownList ID="ddInvoice" runat="server" AppendDataBoundItems="True"
                                            AutoPostBack="True" DataSourceID="SqlDataSource1x"
                                            DataTextField="LCNo" DataValueField="sl"
                                            OnSelectedIndexChanged="ddInvoice_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource1x" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT sl, [LCNo] FROM [LC] WHERE (([IsActive] = 'A')) ORDER BY [sl] desc"></asp:SqlDataSource>
                                    </b>
                                </div>

                                <div id="LCNoInput" runat="server" class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="ltrLcNo" runat="server" Text="LC/TT No."></asp:Literal></label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtNo" runat="server" Enabled="True"></asp:TextBox>
                                        <asp:TextBox ID="txtEditLcNo" runat="server" Visible="False" Text=""></asp:TextBox>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">L/C Type :  </label>
                                    <div class="controls">
                                        <div class="controls">
                                            <asp:DropDownList ID="ddType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddType_OnSelectedIndexChanged">
                                                <asp:ListItem>LC</asp:ListItem>
                                                <asp:ListItem>TT</asp:ListItem>
                                                <asp:ListItem>FTT</asp:ListItem>
                                                <asp:ListItem>Pay Order</asp:ListItem>
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
                                    <label class="control-label">General Item Type :  </label>
                                    <div class="controls">
                                        <%--<asp:TextBox ID="txtItem" runat="server" Enabled="True"></asp:TextBox>--%>
                                        <asp:DropDownList ID="ddSuppCategory" runat="server" AutoPostBack="True"
                                            DataSourceID="SqlDataSource12" DataTextField="BrandName"
                                            DataValueField="BrandID" OnSelectedIndexChanged="ddSuppCategory_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource12" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [BrandID], BrandName FROM [RefItems] where BrandID<>'15' ORDER BY [BrandName]"></asp:SqlDataSource>
                                    </div>
                                </div>
                                <%--
                        <div class="control-group">
                            <label class="control-label">Reference/ Pack Size :  </label>
                            <div class="controls">
                                <asp:TextBox ID="txtReferrence" runat="server" Enabled="True"></asp:TextBox>
                            </div>
                        </div>--%>
                                <%--   <div class="control-group">
                                    <label class="control-label">For Company/Dept :  </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtFor" runat="server" Enabled="True"></asp:TextBox>
                                    </div>
                                </div>--%>

                                <div class="control-group">
                                    <label class="control-label">For Company :  </label>
                                    <div class="controls">
                                        <div class="controls">
                                            <asp:DropDownList ID="ddCompany" runat="server">
                                                <asp:ListItem>Q Pail Limited</asp:ListItem>
                                                <asp:ListItem>Xclusive Can Ltd.</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
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
                                    <label class="control-label">Arrival Date :  </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtArrivalDt" runat="server" />
                                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy"
                                            Enabled="True" TargetControlID="txtArrivalDt">
                                        </asp:CalendarExtender>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Port Delivery Date :  </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtDeliveryDt" runat="server" />
                                        <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd/MM/yyyy"
                                            Enabled="True" TargetControlID="txtDeliveryDt">
                                        </asp:CalendarExtender>
                                    </div>
                                </div>



                                <%-- <div id="Div3" runat="server" class="control-group">
                                    <asp:Label ID="Label14" runat="server" Text="Manufacturer  Category :  "></asp:Label>
                                    <asp:DropDownList ID="ddSuppCategory" runat="server" AutoPostBack="True"
                                        DataSourceID="SqlDataSource12" DataTextField="BrandName"
                                        DataValueField="BrandID" OnSelectedIndexChanged="ddSuppCategory_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource12" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], BrandName FROM [RefItems] ORDER BY [BrandName]"></asp:SqlDataSource>
                                </div>--%>

                                <div class="form-group">
                                    <label class="control-label">Manufacturer :  </label>

                                    <asp:DropDownList ID="ddManufacturer" runat="server" CssClass="select2me" Width="70%" DataSourceID="SqlDataSource3" DataTextField="Company" DataValueField="PartyID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type)  ORDER BY [Company]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="supplier" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

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


                                <%-- <div id="Div4" runat="server" class="control-group">
                                    <asp:Label ID="Label7" runat="server" Text="Agent Category :  "></asp:Label>
                                    <asp:DropDownList ID="ddAgentCategory" runat="server" AutoPostBack="True"
                                        DataSourceID="SqlDataSource12" DataTextField="BrandName"
                                        DataValueField="BrandID" OnSelectedIndexChanged="ddAgentCategory_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                     </div>--%>
                                <div class="form-group">
                                    <label class="control-label">Agent/ Shipper :  </label>

                                    <asp:DropDownList ID="ddAgent" runat="server" DataSourceID="SqlDataSource5" CssClass="select2me" Width="70%" DataTextField="Company" DataValueField="PartyID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type)  ORDER BY [Company]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="agents" Name="Type" Type="String" />

                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>
                                <div class="control-group">
                                    <label class="control-label">Insurance Company :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddInsurance" runat="server" DataSourceID="SqlDataSource6" DataTextField="Company" DataValueField="PartyID">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="insurance" Name="Type" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>

                                <div runat="server" class="control-group">
                                    <label class="control-label">Insurance No :  </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtInsurNo" runat="server" Enabled="True"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="control-group hidden">
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

                                <%-- <div class="control-group">
                                    <label class="control-label">Bank Name :</label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddBank" runat="server" DataSourceID="SqlDataSource8" DataTextField="Bank" DataValueField="ACID">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT ACID, (Select [BankName] FROM [Banks] where [BankId]=a.BankID)  +' ('+Address +') '+ +' - '+ACNo +' - '+ ACName AS Bank from BankAccounts a ORDER BY [ACName]"></asp:SqlDataSource>
                                    </div>
                                </div>--%>

                                <div class="form-group">
                                    <label class="control-label">Bank Name :</label>

                                    <asp:DropDownList ID="ddBank" runat="server" DataSourceID="SqlDataSource8" Width="70%" DataTextField="BankName" DataValueField="BankId" CssClass="select2me" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BankId], [BankName] FROM [Banks] WHERE ([Type] = @Type) ORDER BY [BankName]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="bank" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Bank Branch :</label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddBankBranch" runat="server" DataSourceID="SqlDataSource16" DataTextField="BranchName" DataValueField="BranchID" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource16" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT  BranchID,  BranchName  FROM BankBranch WHERE (BankID=@BankId)">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddBank" Name="BankId" PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Bank Account :</label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddBancAcc" runat="server" DataSourceID="SqlDataSource17" DataTextField="Bank" DataValueField="ACID" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource17" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT ACID, (Select [BankName] FROM [Banks] where [BankId]=a.BankID)  +' ('+Address +') '+ +' - '+ACNo +' - '+ ACName AS Bank from BankAccounts a ORDER BY [ACName]"></asp:SqlDataSource>
                                    </div>
                                </div>
                                <a name="ItemDetails"></a>
                                <legend style="margin-bottom: 6px;">Item Details</legend>

                                <div class="control-group hidden">
                                    <asp:Label ID="Label9" runat="server" Text="Purpose :"></asp:Label>
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
                                    <label class="control-label">Group :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddGroup" runat="server" DataSourceID="SqlDataSource2"
                                            DataTextField="GroupName" DataValueField="GroupSrNo" AutoPostBack="true" OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] Where GroupSrNo<>2 AND GroupSrNo<>3 AND GroupSrNo<>'11'  ORDER BY [GroupSrNo]"></asp:SqlDataSource>
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
                                        <asp:DropDownList ID="ddItemName" runat="server" OnSelectedIndexChanged="ddItemName_OnSelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">HS Code :  </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtHSCode" runat="server"></asp:TextBox>
                                    </div>
                                </div>

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
                                        <asp:TextBox ID="txtWeight" runat="server" Text="0" />
                                    </div>
                                </div>

                                <div id="thicknessField" runat="server" class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="ltrThickness" runat="server" Text="Thickness: "></asp:Literal>
                                    </label>
                                    <asp:TextBox ID="txtThickness" runat="server" Enabled="True"></asp:TextBox>
                                </div>
                            </div>

                            <div id="measurementField" runat="server" class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="ltrMeasurement" runat="server" Text="Measurement : "></asp:Literal>
                                </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtMeasure" runat="server" Enabled="True"></asp:TextBox>
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


                            <asp:Panel ID="PanelWarrenty" runat="server">
                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="ltrSerial" runat="server" Text="Model No. : " />
                                    </label>
                                    <asp:TextBox ID="txtSerial" runat="server" />
                                </div>

                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="ltrWarrenty" runat="server" Text="Warrentry : " />
                                    </label>
                                    <asp:TextBox ID="txtWarrenty" runat="server" />
                                </div>
                            </asp:Panel>


                            <div class="control-group">
                                <label class="control-label">
                                    Quantity (<asp:Literal ID="ltrUnitType" runat="server" ></asp:Literal>) :
                                
                                </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtQty" runat="server" Text="0"  onkeyup="calTotalWeight()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtQty">
                                    </asp:FilteredTextBoxExtender>

                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label">Unit Price USD :  </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtPrice" runat="server" Text="0" onkeyup="calItemCFR()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtPrice">
                                    </asp:FilteredTextBoxExtender>

                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label">Item Value USD:  </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCFR" runat="server" Text="0"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtCFR">
                                    </asp:FilteredTextBoxExtender>

                                    <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add to grid" OnClick="btnAdd_Click" />
                                </div>
                            </div>
                            <asp:Label ID="lblMsg2" EnableViewState="false" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblEntryId" runat="server" Text="" Visible="False"></asp:Label>
                            <div style="clear: both"></div>

                            <div class="table-responsive">

                                <asp:GridView ID="ItemGrid" runat="server" AutoGenerateColumns="False" OnRowDeleting="ItemGrid_RowDeleting"
                                    Width="250%" DataKeyNames="EntryID" OnRowDataBound="ItemGrid_RowDataBound" AutoGenerateSelectButton="True" OnSelectedIndexChanged="ItemGrid_OnSelectedIndexChanged">

                                    <Columns>

                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="EntryID" SortExpression="EntryID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("EntryID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Purpose" SortExpression="EntryID">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1h" runat="server" Text='<%# Bind("Purpose") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Grade" SortExpression="Product">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2p" runat="server" Text='<%# Bind("Grade") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Category" SortExpression="Product">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2o" runat="server" Text='<%# Bind("Category") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Product" SortExpression="Product">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Product") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Pk Size" SortExpression="Size">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1xz" runat="server" Text='<%# Bind("Size") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Thickness(mm)/ Serial#" SortExpression="HSCode">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1x" runat="server" Text='<%# Bind("Thickness") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Spec" SortExpression="HSCode">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSpec" runat="server" Text='<%# Bind("Spec") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model/ Measurement" SortExpression="HSCode">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1x1" runat="server" Text='<%# Bind("Measurement") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="QTY" SortExpression="QTY1">
                                            <ItemTemplate>
                                                <asp:Label ID="QTY9" runat="server" Text='<%# Bind("QTY1") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit Price (USD)" SortExpression="UnitPrice">
                                            <ItemTemplate>
                                                <asp:Label ID="UnitPrice" runat="server" Text='<%# Bind("UnitPrice") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Value (USD)" SortExpression="CFRValue">
                                            <ItemTemplate>
                                                <asp:Label ID="CFRValue" runat="server" Text='<%# Bind("CFRValue") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:CommandField ButtonType="Image" ControlStyle-Width="24px" DeleteImageUrl="~/App_Themes/Blue/img/error-s.png" ShowDeleteButton="True"  SelectImageUrl="~/app/images/edit.png" ShowSelectButton="true" />--%>

                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>

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

                                    </Columns>
                                </asp:GridView>


                                <asp:SqlDataSource ID="SqlDataSource9" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT EntryID, 
                                                (Select Purpose from Purpose where pid=a.Purpose) as Purpose, 
                                                (Select GradeName from ItemGrade where GradeID=a.GradeId) as Grade, 
                                                (Select CategoryName from Categories where CategoryID=a.CategoryId) as Category, 
                                                (Select ItemName from Products where ProductID=a.ItemCode) as Product,                      
                                                [Thickness], (Select BrandName from Brands where BrandID=a.ItemSizeID) as Size, Measurement, 
                                                (Select spec from Specifications where id=a.spec) as spec, NoOfPacks ,
                                                CONVERT(varchar(10), qty) +' '+(Select UnitType from Products where ProductID=a.ItemCode) As QTY1, UnitPrice,  [CFRValue] 
                                                FROM [LcItems] a Where  LCNo='' ORDER BY [EntryID]"
                                    DeleteCommand="DELETE LcItems WHERE EntryID=@EntryID"
                                    UpdateCommand="">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="txtEditLcNo" Name="LCNo" PropertyName="Text" Type="String" />
                                    </SelectParameters>
                                    <DeleteParameters>
                                        <asp:Parameter Name="EntryID" />
                                    </DeleteParameters>
                                </asp:SqlDataSource>
                            </div>

                            <a name="ItemDetails"></a>
                            <legend style="margin-bottom: 6px;">Rate & Amounts</legend>

                            <div class="control-group">
                                <label class="control-label">Bank Exchange Rate :  </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtBExRate" runat="server" Text="0" onkeyup="calCFRBDT()"></asp:TextBox>
                                </div>
                            </div>

                            <div class="control-group hidden">
                                <label class="control-label">Custom Exch Rate :  </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCExRate" runat="server" Text="0" onkeyup="calCFRBDT()"></asp:TextBox>
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
                                    <asp:TextBox ID="txtTtlQty" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label">Freight USD:  </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtFreight" runat="server" Text="0" Enabled="True"></asp:TextBox>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label">CFR USD :  </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCfrUSD" runat="server" Text="0" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">CFR BDT :  </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCfrBDT" runat="server" Text="0"></asp:TextBox>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label">FOB/ Bank BDT :  </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtBankBDT" runat="server" Text="0" Enabled="True"></asp:TextBox>
                                </div>
                            </div>

                              <div id="margin" runat="server" class="control-group">
                                <label class="control-label">L/C Margin :  </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtMarginPercent" runat="server" Width="35%" Placeholder="Margin percentage"></asp:TextBox>
                                    <asp:TextBox ID="txtMargin" runat="server" Text="0" Width="35%" onkeyup="calCFRBDT()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtMargin">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                            </div>

                            <%--<div class="control-group">
                                <label class="control-label">L/C Margin :  </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtMargin" runat="server" Text="0" Width="33%"></asp:TextBox>                                  
                                    <asp:TextBox ID="txtMarginFinal" Width="33%" runat="server" Text="0" Enabled="True"></asp:TextBox>

                                </div>
                            </div>--%>

                            <div id="ltr" runat="server" class="control-group">
                                <label class="control-label">LTR :  </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtLTR" runat="server" Text="0" Enabled="True"></asp:TextBox>
                                </div>
                            </div>

                             <div id="foreignBank" Visible="False" runat="server" class="control-group">
                                <label class="control-label">Foreign Bank :  </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtForeignBank" runat="server" Text="0" Enabled="True"></asp:TextBox>
                                </div>
                            </div>

                             <div class="control-group">
                                <label class="control-label">Opening Bank Charge :  </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtOpeningBankCrg" runat="server" Text="0" Enabled="True"></asp:TextBox>
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
                            
                            <legend style="margin-bottom: 6px;">(Advanced Against LC)</legend>

                            <div class="form-group">
                                <label class="control-label">Control Account  :</label>
                                
                                <asp:DropDownList ID="ddAdvanceAginstControl" runat="server" DataSourceID="SqlDataSource18"
                                    DataTextField="ControlAccountsName" DataValueField="ControlAccountsID"
                                    Width="70%" AppendDataBoundItems="True" CssClass="select2me" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource18" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                    SelectCommand="SELECT ControlAccountsID, ControlAccountsName FROM ControlAccount WHERE (ControlAccountsName LIKE 'Advance against LC%') ORDER BY [ControlAccountsName]"></asp:SqlDataSource>
                            </div>

                               <div class="form-group">
                                    <label class="control-label">Account Head :
                                         <asp:LinkButton ID="btnAccHead" runat="server" OnClick="btnHead_OnClick">New</asp:LinkButton>
                                    </label>
                                        <asp:DropDownList ID="ddAdvanceAginstHead" Width="70%" CssClass="select2me" runat="server" DataSourceID="SqlDataSource19" DataTextField="AccountsHeadName" DataValueField="AccountsHeadID" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource19" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT  EntryID, AccountsHeadID, AccountsHeadName   FROM HeadSetup WHERE (ControlAccountsID=@ControlAccountsID)">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddAdvanceAginstControl" Name="ControlAccountsID" PropertyName="SelectedValue" />
                                            </SelectParameters>
                                            
                                        </asp:SqlDataSource>
                                   <asp:TextBox ID="txtAccHead" class="form-control" runat="server" Visible="False" />
                                </div>



                            <div class="form-actions">
                                <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save New LC" OnClick="btnSave_Click" />
                                <asp:Button ID="btnDelete" CssClass="btn" runat="server" Text="Delete" Visible="false" />
                            </div>

                            <asp:Label ID="lblLogin" runat="server" Text="" Visible="false"></asp:Label>
                        </div>
                    </div>
                </div>






                <%--Saved Data navigation--%>

                <asp:Panel ID="RightPanel1" runat="server">
                    <div class="col-md-6 ">
                        <div class="portlet box green">
                            <div class="control-group">
                                <label class="control-label">Search by LC Number : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtSearchByLCNo" runat="server" placeholder="Search by Lc Number" Width="47%" Enabled="True"></asp:TextBox>
                                    <asp:Button ID="btnSearchLC" CssClass="btn blue" runat="server" Text="Search LC" Style="width: 20%; margin-top: 2px; margin-left: 6px;" OnClick="btnSearchLC_OnClick" />
                                </div>
                            </div>
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Saved Data 
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


                                    <div id="Div2" runat="server" class="control-group">
                                        <label class="control-label">LC/TT No.</label>
                                        <div class="controls">
                                            <asp:Label ID="lblLCNo" runat="server" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">L/C Type :  </label>
                                        <div class="controls">
                                            <div class="controls">
                                                <asp:Label ID="lblLCType" runat="server"></asp:Label>

                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Open Date :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblOpDate" runat="server" />
                                        </div>
                                    </div>

                                    <%--<div class="control-group">
                                    <label class="control-label">Group :  </label>
                                    <div class="controls">
                                        <asp:Label ID="lblGrp" runat="server"></asp:Label>
                                    </div>
                                </div>

                        <div class="control-group">
                            <label class="control-label">HS Code :  </label>
                            <div class="controls">
                                <asp:Label ID="lblHS" runat="server"/>
                            </div>
                        </div>--%>

                                    <%--<div class="control-group">
                            <label class="control-label">Item Name :  </label>
                            <div class="controls">
                                <asp:Label ID="lblItem" runat="server" enabled="True"></label>
                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">Reference/ Pack Size :  </label>
                            <div class="controls">
                                <asp:Label ID="TextBox5" runat="server" enabled="True"></label>
                            </div>
                        </div>--%>
                                    <div class="control-group">
                                        <label class="control-label">For Company :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblDept" runat="server" />
                                        </div>
                                    </div>


                                    <div class="control-group">
                                        <label class="control-label">Expiry Date :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblExDate" runat="server" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Ship Date :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblShipDate" runat="server" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Arrival  Date :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblArrivalDt" runat="server" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Port Delivery  Date :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblDeliveryDt" runat="server" />
                                        </div>
                                    </div>


                                    <div class="control-group">
                                        <label class="control-label">Manufacturer :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblSupplier" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Country of Origin :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblCountry" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Agent/ Shipper :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblagent" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Insurance Company :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblInsurance" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div runat="server" class="control-group">
                                        <label class="control-label">Insurance No :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblInsuranceNo" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="control-group hidden">
                                        <label class="control-label">CNF Agent :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblCNF" runat="server"></asp:Label>

                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Bank Name :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblBank" runat="server"></asp:Label>

                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Bank Branch :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblBankBranch" runat="server"></asp:Label>

                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Bank Account :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblBankAcc" runat="server"></asp:Label>

                                        </div>
                                    </div>

                                    <div class="table-responsive">
                                        <legend style="margin-bottom: 6px;">Item Details</legend>
                                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" OnRowDeleting="ItemGrid_RowDeleting"
                                            DataSourceID="SqlDataSource10" Width="200%" DataKeyNames="EntryID">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>

                                                    <ItemStyle Width="20px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="EntryID" SortExpression="EntryID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("EntryID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

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

                                                <asp:TemplateField HeaderText="Measurement" SortExpression="Size">
                                                    <ItemTemplate>
                                                        <asp:Label ID="gdfg" runat="server" Text='<%# Bind("Measurement") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Thickness" SortExpression="Size">
                                                    <ItemTemplate>
                                                        <asp:Label ID="gd4fg" runat="server" Text='<%# Bind("Thickness") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


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
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                    <asp:SqlDataSource ID="SqlDataSource10" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT EntryID,
                                            (SELECT GradeName FROM [ItemGrade] where GradeID=(SELECT GradeID FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=a.ItemCode))) As Grade, 
                                                (SELECT CategoryName FROM [Categories] where CategoryID= (SELECT CategoryID FROM [Products] where ProductID=a.ItemCode)) As Category, 
                                     (Select ItemName from Products where ProductID=a.ItemCode) as Product,                      
                                                    [HSCode], (Select BrandName from Brands where BrandID=a.ItemSizeID) as Size, Measurement, Thickness,
                                                    CONVERT(varchar(10), qty) +' '+(Select UnitType from Products where ProductID=a.ItemCode) As QTY1, UnitPrice,  [CFRValue] 
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


                                    <div class="control-group">
                                        <label class="control-label">Bank Exchange Rate :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblBankExcRate" runat="server" />
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Custom Exch Rate :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblCustExRate" runat="server" />
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
                                            <asp:Label ID="lblTtlQty" runat="server" />
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Freight USD :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblFreight" runat="server" />
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">CFR USD :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblCfrUsd" runat="server" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">CFR BDT :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblCfrBdt" runat="server" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">FOB/ Bank BDT :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblBankBdt" runat="server" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">L/C Margin :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblMargin" runat="server" />
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">LTR :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblLTR" runat="server" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Foreign Bank :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblForeignBank" runat="server" />
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Opening Bank Charge :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblOpeningBangCrg" runat="server" />
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Remarks:  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblRemark" runat="server" />
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Mode of Transport :  </label>
                                        <div class="controls">
                                            <asp:Label ID="lblMode" runat="server"></asp:Label>

                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label"></label>
                                        <div class="controls">
                                            <h5 id="lblStatusColor" runat="server">
                                                <asp:Literal ID="lblStatus" runat="server" EnableViewState="false"></asp:Literal></h5>

                                        </div>
                                    </div>


                                    <div class="form-actions">
                                        <asp:Button ID="btnFirst" CssClass="btn blue" runat="server" Text="<< First" OnClick="btnFirst_Click" />
                                        <asp:Button ID="btnPrevious" CssClass="btn blue" runat="server" Text="< Previous" OnClick="btnPrevious_Click" />
                                        <asp:Button ID="btnNext" CssClass="btn blue" runat="server" Text="Next >" OnClick="btnNext_Click" />
                                        <asp:Button ID="btnLast" CssClass="btn blue" runat="server" Text="Last >>" OnClick="btnLast_Click" />

                                    </div>






                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>




                <asp:Panel ID="RightPanel2" runat="server" Visible="False">
                    <div class="col-md-6 ">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>
                                    Amendment history for LC#
                                    <asp:Literal ID="ltrFormTitle2" runat="server"></asp:Literal>
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


                                    <asp:Label ID="Label5" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="Label6" runat="server" EnableViewState="false"></asp:Label>

                                    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" OnRowDeleting="ItemGrid_RowDeleting"
                                        DataSourceID="SqlDataSource13" Width="100%">

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
                                            <asp:ControlParameter ControlID="txtNo" Name="LCNo" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                    <%--Old Ammendment--%>
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowDeleting="ItemGrid_RowDeleting"
                                        DataSourceID="SqlDataSource15" Width="100%" DataKeyNames="sl" Visible="False">
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


                                    <asp:SqlDataSource ID="SqlDataSource15" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT sl, EntryDate, CfrUSD, CfrBDT, ExpiryDate, ShipDate
                                            FROM [LcHistory] a Where LCNo=@LCNo ORDER BY [sl]"
                                        DeleteCommand="DELETE LcItems WHERE EntryID=@EntryID"
                                        UpdateCommand="">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="txtEditLcNo" Name="LCNo" PropertyName="Text" Type="String" />
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
                </asp:Panel>


            </div>


        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>


