<%@ Page Title="IML Print" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="IML-Output.aspx.cs" Inherits="app_IML_Output" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        $(document).ready(function () {
            $('#<%=txtCalculated.ClientID%>').attr("disabled", true);
            $('#<%=txtFinalProd.ClientID%>').attr("disabled", true);
            $('#<%=txtNetWeight.ClientID%>').attr("disabled", true);
            $('#<%=txtActualWeight.ClientID%>').attr("disabled", true);
            calInvTotal();
        });

        function calInvTotal() {

            var txtCycleTime = document.getElementById("<%= txtCycleTime.ClientID %>").value;
            var txtCavity = document.getElementById("<%= txtCavity.ClientID %>").value;
            var txtHour = document.getElementById("<%= txtHour.ClientID %>").value;
            var txtMin = document.getElementById("<%= txtMin.ClientID %>").value;
            var txtTimeWaist = document.getElementById("<%= txtTimeWaist.ClientID %>").value;
            var stdPrdn = document.getElementById("<%= txtStdHour.ClientID %>").value;

            ttl = (txtHour * 3600) * parseFloat(txtCavity) / parseFloat(txtCycleTime);
            ttl = ttl + (txtMin * 60) * parseFloat(txtCavity) / parseFloat(txtCycleTime);

            var ttl2 = parseInt(ttl);
            $('#<%=txtproduced.ClientID%>').val(ttl2.toString());
            $('#<%=txtCalculated.ClientID%>').val(ttl2.toString());

            //var ttlProjected = (stdPrdn * 1) * ((txtHour * 1) + (txtMin/60))* parseFloat(txtCavity) ;
            var ttlProjected = ((txtHour * 60 * 60) + (txtMin * 60)) / parseFloat(txtCycleTime);
            //$('#<%=txtCalculated.ClientID%>').val(parseInt(ttlProjected).toString());

            var txtproduced = document.getElementById("<%= txtproduced.ClientID %>").value;
            var txtRejected = document.getElementById("<%= txtRejected.ClientID %>").value;
            ttl = (ttl * 1) - (txtRejected * 1);
            $('#<%=txtFinalProd.ClientID%>').val(parseInt(ttl).toFixed(0).toString());

            var txtWeightPc = document.getElementById("<%= txtWeightPc.ClientID %>").value;
            var ttlw = parseInt(ttl).toFixed(0).toString() * (txtWeightPc * 1) / 1000;
            $('#<%=txtNetWeight.ClientID%>').val(parseFloat(ttlw).toFixed(2).toString());

            ttlw = (txtproduced * 1) * (txtWeightPc * 1) / 1000;
            $('#<%=txtActualWeight.ClientID%>').val(parseFloat(ttlw).toFixed(2).toString());

            $('#<%=txtFinalOutput.ClientID%>').val(parseInt(ttl).toString());
            ttlw = (txtRejected * 1) * (txtWeightPc * 1) / 1000;


            var nonUsableWestage = document.getElementById("<%= txtNonUsable.ClientID %>").value;
            var ttlw1 = ttlw - parseFloat(nonUsableWestage);
            $('#<%=txtTotalWestage.ClientID%>').val(parseFloat(ttlw1).toFixed(2).toString());

            var txtWeightPc2 = document.getElementById("<%= txtFinalOutput.ClientID %>").value;
            ttlw = (txtWeightPc2 * 1) * (txtWeightPc * 1) / 1000;
            $('#<%=txtOutputKg.ClientID%>').val(parseFloat(ttlw).toFixed(2).toString());

        }
        $("#<%= txtCycleTime.ClientID %>").keydown(function () {
            calInvTotal();
        });

   <%--     function calReusableWesage() {
             var actPrWKg = document.getElementById("<%= txtActualWeight.ClientID %>").value;
            var finalOutputKg = document.getElementById("<%= txtOutputKg.ClientID %>").value;
            var ttl = parseFloat(actPrWKg) - parseFloat(finalOutputKg);

            var nonUsableWestage = document.getElementById("<%= txtNonUsable.ClientID %>").value;

            var ttlReusable = ttl - parseFloat(nonUsableWestage);
            $('#<%=txtTotalWestage.ClientID%>').val(ttlReusable.toString());

        }--%>
    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lblUser" runat="server" Visible="false"></asp:Label>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>


            <h3 class="page-title">IML Print</h3>

            <%-- ID of Power Press is 7 --%>

            <div class="row">
                <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Daily Production Entry
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                <asp:Label ID="lblPrId" runat="server" Text="" Visible="False"></asp:Label>
                                <div class="control-group">
                                    <asp:Label ID="Label4" runat="server" Text="Production Date :"></asp:Label>
                                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                                        Enabled="True" TargetControlID="txtDate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Section: </label>
                                    <asp:DropDownList ID="ddSection" runat="server" DataSourceID="SqlDataSource22"
                                        DataTextField="SName" DataValueField="SID" AutoPostBack="true" OnSelectedIndexChanged="ddSection_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource22" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [SID], [SName] FROM [Sections] WHERE sid=7 or sid=14 or sid=15 or (sid>=16 and sid<=18)   ORDER BY [SName]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="5" Name="DepartmentID" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group hidden">
                                    <label class="control-label">Godown : </label>
                                    <asp:DropDownList ID="ddGodown" runat="server" DataSourceID="SqlDataSource15"
                                        DataTextField="StoreName" DataValueField="WID" CssClass="form-control"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddGodown_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource15" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [WID], [StoreName] FROM [Warehouses]"></asp:SqlDataSource>
                                </div>

                                <div class="control-group hidden">
                                    <label class="control-label">Location : </label>
                                    <asp:DropDownList ID="ddLocation" runat="server" DataSourceID="SqlDataSource16"
                                        DataTextField="AreaName" DataValueField="AreaID" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource16" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT AreaID,[AreaName] FROM [WareHouseAreas] WHERE Warehouse=@Warehouse">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddGodown" Name="Warehouse" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label17" runat="server" Text="Shift: "></asp:Label>
                                    <asp:DropDownList ID="ddShift" runat="server" DataSourceID="SqlDataSource11"
                                        DataTextField="DepartmentName" DataValueField="Departmentid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource11" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Shifts] where Section=@Section ORDER BY [DepartmentName]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddSection" Name="Section" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label6" runat="server" Text="Machine No.: "></asp:Label>
                                    <asp:DropDownList ID="ddMachine" runat="server" DataSourceID="SqlDataSource2"
                                        DataTextField="MachineNo" DataValueField="mid" AutoPostBack="True" OnSelectedIndexChanged="ddMachine_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT CONCAT(MachineNo,'  ',Description) AS MachineNo, mid FROM Machines WHERE Section=@Section ORDER BY MachineNo">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddSection" Name="Section" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="Label7" runat="server" Text="Mould No.: "></asp:Label>
                                    <asp:DropDownList ID="ddMould" runat="server" DataSourceID="SqlDataSource4"
                                        DataTextField="MouldName" DataValueField="Id">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [MouldName], [Id] FROM [MouldInfo] ORDER BY Id DESC "></asp:SqlDataSource>
                                </div>

                                <%-- <div class="control-group">
                                    <asp:Label ID="Label7" runat="server" Text="Line# : "></asp:Label>
                                    <asp:DropDownList ID="ddLine" runat="server" DataSourceID="SqlDataSource4"
                                        DataTextField="DepartmentName" DataValueField="Departmentid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Lines] where Section=@Section ORDER BY [DepartmentName]"><SelectParameters>
                                            <asp:ControlParameter ControlID="ddSection" Name="Section" PropertyName="SelectedValue" />
                                        </SelectParameters></asp:SqlDataSource>
                                </div>--%>

                                <div class="control-group hidden">
                                    <asp:Label ID="Label9" runat="server" Text="Purpose :"></asp:Label>
                                    <asp:DropDownList ID="ddPurpose" runat="server" DataSourceID="SqlDataSource1"
                                        DataTextField="Purpose" DataValueField="pid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [pid], [Purpose] FROM [Purpose] order by Purpose">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="lblDeptName" runat="server" Text="For Company: "></asp:Label>
                                    <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource8"
                                        DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddCustomer_OnSelectedIndexChanged">

                                        <%--  <asp:ListItem Value="">--- all ---</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY Company ASC">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>

                                <div class="control-group hidden">
                                    <asp:Label ID="Label21" runat="server" Text="Brand Name: "></asp:Label>
                                    <asp:DropDownList ID="ddBrand" runat="server" DataSourceID="SqlDataSource5" DataTextField="BrandName" DataValueField="BrandID" AppendDataBoundItems="True">
                                        <%--<asp:ListItem Value="">--- all ---</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], [BrandName] FROM [CustomerBrands] WHERE (([CustomerID] = @CustomerID) AND ([ProjectID] = @ProjectID)) ORDER BY BrandName ASC">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label2" runat="server" Text="Pack Size: "></asp:Label>
                                    <asp:DropDownList ID="ddSize" runat="server" DataSourceID="SqlDataSource6" DataTextField="BrandName" DataValueField="BrandID"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddOperation_OnSelectedIndexChanged">
                                        <%--<asp:ListItem Value="">--- all ---</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] WHERE ([ProjectID] = @ProjectID) ORDER BY BrandName ASC">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label12" runat="server" Text="Operation Name: "></asp:Label>
                                    <asp:DropDownList ID="ddOperation" runat="server" DataSourceID="SqlDataSource9x"
                                        DataTextField="DepartmentName" DataValueField="Departmentid" AutoPostBack="True" OnSelectedIndexChanged="ddOperation_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource9x" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Satges] where Section=@Section ORDER BY [DepartmentName]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddSection" Name="Section" PropertyName="SelectedValue" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="form-group">
                                    <asp:Label ID="Label13" runat="server" Width="29%" Text="Operator: "></asp:Label>
                                    <asp:DropDownList ID="ddOperator" runat="server" DataSourceID="SqlDataSource10" AutoPostBack="true" CssClass="form-control select2me"
                                        DataTextField="EName" DataValueField="EmployeeInfoID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource10" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [EmployeeInfoID], [EName] FROM [EmployeeInfo] WHERE ([SectionID] = '7') ORDER BY [EName] DESC"></asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <label>IML Grade : </label>
                                    <asp:DropDownList ID="ddSpec" runat="server" DataSourceID="SqlDataSource18" DataTextField="GradeName" DataValueField="GradeID"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddSpec_OnSelectedIndexChanged" AppendDataBoundItems="True">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource18" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [GradeID], [GradeName] FROM [ItemGrade] WHERE ([CategoryID] = @CategoryID)">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="33" Name="CategoryID" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>


                                <div class="control-group">
                                    <asp:Label ID="Label26" runat="server" Text="IML Category : "></asp:Label>
                                    <asp:DropDownList ID="ddColorCategory" runat="server" DataSourceID="SqlDataSource13"
                                        DataTextField="CategoryName" DataValueField="CategoryID" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddColorCategory_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource13" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT CategoryID, CategoryName FROM Categories WHERE (GradeID = @GradeID) ORDER BY [CategoryName]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddSpec" Name="GradeID" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="form-group">
                                    <label>IML Item Name: </label>
                                    <asp:DropDownList ID="ddIMLItem" runat="server" CssClass="form-control select2me" AppendDataBoundItems="True" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddIMLItem_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource14" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT ProductID, ItemName FROM [Products] where CategoryID=@CategoryID ORDER BY [ItemName]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddColorCategory" Name="CategoryID" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                    <span style="width: 70%; color: green; float: right">
                                        <asp:Literal ID="ltrColorStock" runat="server">Available  Stock: Qnty(Kg).... </asp:Literal>
                                    </span>

                                </div>




                                <div class="control-group">
                                    <asp:Label ID="Label3" runat="server" Text="Cycle Time (Sec.) : "></asp:Label>
                                    <asp:TextBox ID="txtCycleTime" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtCycleTime">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label10" runat="server" Text="Cavity (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtCavity" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtCavity">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label5" runat="server" Text="Working Hour & Minutes : "></asp:Label>
                                    <asp:TextBox ID="txtHour" runat="server" Width="35%" onkeyup="calInvTotal()" placeholder="Hours"></asp:TextBox>
                                    <asp:TextBox ID="txtMin" runat="server" Width="34%" onkeyup="calInvTotal()" placeholder="Minutes"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtHour">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label8" runat="server" Text="Time Waste (Min) : "></asp:Label>
                                    <asp:TextBox ID="txtTimeWaist" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtTimeWaist">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label16" runat="server" Text="Reason of Time Waste  : "></asp:Label>
                                    <asp:TextBox ID="txtReason" runat="server"></asp:TextBox>
                                </div>

                                <div class="control-group hidden">
                                    <asp:Label ID="Label14e" runat="server" Text="Std. Hourly Production (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtStdHour" runat="server" Text="0" onkeyup="calInvTotal()"></asp:TextBox>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="Label14" runat="server" Text="Projected Production (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtCalculated" runat="server"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label11" runat="server" Text="Actual Production (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtproduced" runat="server" OnTextChanged="txtproduced_OnTextChanged"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtproduced">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label15" runat="server" Text="Rejection (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtRejected" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtRejected">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label18" runat="server" Text="Net Production(PCS) : "></asp:Label>
                                    <asp:TextBox ID="txtFinalProd" runat="server"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label22" runat="server" Text="Weight/Pc (gm) : "></asp:Label>
                                    <asp:TextBox ID="txtWeightPc" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtWeightPc">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="txtNetWeight1" runat="server" Text="Net Prdn Weight (Kg) : "></asp:Label>
                                    <asp:TextBox ID="txtNetWeight" runat="server"></asp:TextBox>

                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label19" runat="server" Text="Actual Prdn Weight(Kg): "></asp:Label>
                                    <asp:TextBox ID="txtActualWeight" runat="server"></asp:TextBox>
                                </div>



                                <%-- INPUT RATIO --%>

                                <a name="ItemDetails"></a>
                                <legend style="margin-bottom: 6px;">Dozing/ Mixing (%)</legend>
                                <asp:LinkButton ID="lnkDozing" OnClick="lnkDozing_OnClick" runat="server">Show</asp:LinkButton>
                                <asp:LinkButton ID="lnkHide" OnClick="lnkHide_OnClick" runat="server">Hide</asp:LinkButton>

                                <asp:Panel ID="pnlDozing" runat="server" AutoPostBack="True" DefaultButton="btnAdd">

                                    <%--  <div class="control-group hidden">
                                        <asp:Label ID="Label25" runat="server" Text="Purpose :"></asp:Label>
                                        <asp:DropDownList ID="ddPurpose2" runat="server" DataSourceID="SqlDataSource1mm"
                                            DataTextField="Purpose" DataValueField="pid" AutoPostBack="True" OnSelectedIndexChanged="ddPurpose2_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource1mm" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [pid], [Purpose] FROM [Purpose] order by Purpose">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>--%>

                                    <div class="control-group">
                                        <label class="control-label">Item Group : </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddGroup" runat="server" DataSourceID="SqlDataSource12" AutoPostBack="true"
                                                DataTextField="GroupName" DataValueField="GroupSrNo" OnSelectedIndexChanged="ddGroup_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] WHERE GroupSrNo=1 OR GroupSrNo=7 ORDER BY [GroupSrNo]"></asp:SqlDataSource>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label>Item Sub-group : </label>
                                        <asp:DropDownList ID="ddSubGrp" runat="server" CssClass="form-control"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddSubGrp_OnSelectedIndexChanged">
                                        </asp:DropDownList>

                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Item Grade :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddGradeRaw" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddGradeRaw_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Item Category :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddCategoryRaw" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddCategoryRaw_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label>Item Name :  </label>
                                        <asp:DropDownList ID="ddItemNameRaw" runat="server" AutoPostBack="true" CssClass="form-control select2me"
                                            OnSelectedIndexChanged="ddItemNameRaw_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>
                                        <span style="width: 70%; color: green; float: right">
                                            <asp:Literal ID="ltrLastInfo" runat="server">Stock Qty.: </asp:Literal>
                                        </span>
                                    </div>


                                    <div class="control-group">
                                        <asp:Label ID="Label1" runat="server" Text="Consumption (%) : "></asp:Label>
                                        <asp:TextBox ID="txtInputQtyKG" runat="server"></asp:TextBox>
                                        <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtInputQtyKG">
                                        </asp:FilteredTextBoxExtender>--%>
                                    </div>

                                    <%--<div class="control-group">
                                    <asp:Label ID="Label19" runat="server" Text="Input Sheet Qty.(PCS) : "></asp:Label>
                                    <asp:TextBox ID="txtInputQtyPCS" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtInputQtyPCS">
                                    </asp:FilteredTextBoxExtender>
                                </div>--%>

                                    <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>

                                    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" CssClass="right block" />
                                </asp:Panel>

                                <div style="clear: both"></div>

                                <div class="table-responsive">
                                    <asp:Label ID="lblEid" runat="server" Text="" Visible="False"></asp:Label>

                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridView2_OnRowDataBound"
                                        Width="150%" OnSelectedIndexChanged="GridView2_OnSelectedIndexChanged" OnRowDeleting="GridView2_OnRowDeleting">
                                        <Columns>

                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
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

                                            <asp:TemplateField HeaderText="pid" InsertVisible="False" SortExpression="pid" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" />
                                            <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                                            <asp:BoundField DataField="IMLGrade" HeaderText="IML Grade" SortExpression="IMLGrade" />
                                            <asp:BoundField DataField="IMLCategory" HeaderText="IML Category" SortExpression="IMLCategory" />
                                            <asp:BoundField DataField="IMLItemName" HeaderText="IML ItemName" SortExpression="IMLItemName" />
                                            <asp:BoundField DataField="ProductName" HeaderText="Product/ Component" SortExpression="ProductName" />
                                            <asp:BoundField DataField="TotalWeight" HeaderText="Consumption (%)" SortExpression="TotalWeight" />
                                            <asp:BoundField DataField="ConsumedWeight" HeaderText="Consumption (kg)" SortExpression="ConsumedWeight" />

                                        </Columns>
                                    </asp:GridView>

                                    <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT Id, PrdnID, Department, Purpose, SizeId, BrandID, ItemGroup, SubGroup, Grade, Category, IMLGrade, IMLCategory, IMLItemName, ProductID, ProductName, TotalWeight, ConsumedWeight, EntryBy, EntryDate
FROM                            IMLInput WHERE PrdnID =''"
                                        DeleteCommand="Delete FROM [IMLInput] where PrdnID='0'  ">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblUser" Name="EntryBy" PropertyName="Text" />
                                            <asp:ControlParameter ControlID="lblPrId" Name="PrdnID" PropertyName="Text" />
                                            <asp:ControlParameter ControlID="lblMachineNo" Name="MachineNo" PropertyName="Text" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:Label ID="lblMachineNo" runat="server" Text="0" Visible="False"></asp:Label>

                                </div>

                                <asp:Label ID="lblTotalWeight" runat="server" Text=""></asp:Label>



                                <a name="ItemDetails"></a>
                                <legend style="margin-bottom: 6px;">Production Output</legend>
                                <%--<asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>--%>

                                <div class="form-group">
                                    <label>Item Sub-group : </label>
                                    <asp:DropDownList ID="ddOutSubGroup" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddOutSubGroup_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Item Grade :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddOutGrade" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddOutGrade_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Item Category :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddOutCategory" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddCategoryRaw_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label>Processed Item :  </label>
                                    <asp:DropDownList ID="ddOutItem" runat="server" AutoPostBack="true" CssClass="form-control select2me"
                                        OnSelectedIndexChanged="ddOutItem_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Color :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddColor" runat="server"
                                            DataSourceID="SqlDataSource9" DataTextField="DepartmentName" DataValueField="Departmentid">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource9" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [DepartmentName], [Departmentid] FROM [Colors] ORDER BY [DepartmentName]"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label27" runat="server" Text="IML Use (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtIMLUse" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtproduced">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label20" runat="server" Text="Final Output (PCS) : "></asp:Label>
                                    <asp:TextBox ID="txtFinalOutput" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtFinalOutput">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label24" runat="server" Text="Final Output (Kg) : "></asp:Label>
                                    <asp:TextBox ID="txtOutputKg" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtFinalOutput">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <label>Reusable Item :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddWasteStock" runat="server" DataSourceID="SqlDataSource12x" DataTextField="ItemName" DataValueField="ProductID">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource12x" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT ProductID, ItemName FROM Products WHERE (CategoryID IN (SELECT CategoryID FROM Categories WHERE (GradeID IN (SELECT GradeID FROM ItemGrade WHERE (CategoryID = 28)))))"></asp:SqlDataSource>
                                    </div>
                                </div>


                                <div class="control-group">
                                    <asp:Label ID="Label23" runat="server" Text="Nonusable Weight (kg.) : "></asp:Label>
                                    <asp:TextBox ID="txtNonUsable" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtNonUsable">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="Label13x" runat="server" Text="Reusable Westage (kg.) : "></asp:Label>
                                    <asp:TextBox ID="txtTotalWestage" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtTotalWestage">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Remarks : </label>
                                    <asp:TextBox ID="txtRemark" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>

                                <div class="form-actions">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />

                                </div>

                            </div>

                        </div>
                    </div>
                </div>


                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Productions List
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

                                <div class="table-responsive">

                                    <asp:GridView ID="GridView1" runat="server" Width="600%" AllowSorting="True"
                                        AutoGenerateColumns="False" BackColor="White" Borderpayor="#999999"
                                        BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black"
                                        GridLines="Vertical" DataSourceID="SqlDataSource3" AllowPaging="True" PageSize="15" OnPageIndexChanging="GridView1_OnPageIndexChanging"
                                        OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">

                                        <Columns>
                                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
                                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />

                                                    <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                    </asp:ConfirmButtonExtender>
                                                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                        PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                    <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                        <b style="color: red">Entry will be deleted!</b><br />
                                                        Are you sure you want to delete the item from entry list?
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
                                                    <%#Container.DataItemIndex+1 %>.
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="ProductID" SortExpression="ProductID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("pid") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="ProductionID" HeaderText="ProductionID" SortExpression="ProductionID" />
                                            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="Shift" HeaderText="Shift" SortExpression="Shift" />

                                            <asp:BoundField DataField="MachineNo" HeaderText="Machine" SortExpression="MachineNo" />
                                            <asp:BoundField DataField="MouldNo" HeaderText="Mould No" SortExpression="MouldNo" />

                                            <asp:BoundField DataField="Purpose" HeaderText="Purpose" SortExpression="Purpose" />
                                            <asp:BoundField DataField="CustomerID" HeaderText="Customer" SortExpression="CustomerID" />
                                            <asp:BoundField DataField="Brand" HeaderText="Brand" SortExpression="Brand" />
                                            <asp:BoundField DataField="PackSize" HeaderText="PackSize" SortExpression="PackSize" />
                                            <asp:BoundField DataField="Operation" HeaderText="Operation" SortExpression="Operation" />
                                            <asp:BoundField DataField="OperatorID" HeaderText="Operator" SortExpression="OperatorID" />
                                            <asp:BoundField DataField="CycleTime" HeaderText="Cycle Time" SortExpression="CycleTime" />
                                            <asp:BoundField DataField="CavityPcs" HeaderText="Cavity" SortExpression="CavityPcs" />
                                            <asp:BoundField DataField="WorkingHour" HeaderText="Working Hour" SortExpression="WorkingHour" />
                                            <asp:BoundField DataField="TimeWaste" HeaderText="Time Waste" SortExpression="TimeWaste" />
                                            <asp:BoundField DataField="ReasonWaist" HeaderText="Reason Waist" SortExpression="ReasonWaist" />
                                            <asp:BoundField DataField="CalcProduction" HeaderText="Calc.Prdn" SortExpression="CalcProduction" />
                                            <asp:BoundField DataField="ActProduction" HeaderText="Act.Prdn" SortExpression="ActProduction" />
                                            <asp:BoundField DataField="Rejection" HeaderText="Rejection" SortExpression="Rejection" />
                                            <asp:BoundField DataField="NetProduction" HeaderText="Net Prdn" SortExpression="NetProduction" />
                                            <asp:BoundField DataField="ItemWeight" HeaderText="Item Weight" SortExpression="ItemWeight" />
                                            <asp:BoundField DataField="TotalWeight" HeaderText="Total Weight" SortExpression="TotalWeight" />
                                            <asp:BoundField DataField="ItemSubGroup" HeaderText="Item SubGroup" SortExpression="ItemSubGroup" />
                                            <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" />
                                            <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                                            <asp:BoundField DataField="ItemName" HeaderText="ItemName" SortExpression="ItemName" />
                                            <asp:BoundField DataField="Color" HeaderText="Color" SortExpression="Color" />
                                            <asp:BoundField DataField="FinalProduction" HeaderText="Final Prdn" SortExpression="FinalProduction" />
                                            <asp:BoundField DataField="WasteWeight" HeaderText="Waste Weight" SortExpression="WasteWeight" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
                                            <asp:BoundField DataField="EntryBy" HeaderText="EntryBy" SortExpression="EntryBy" />

                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT pid, ProductionID, Date, MachineNo, MouldNo, Shift, Operation, OperatorID, Purpose, CustomerID, Brand, PackSize, ItemSubGroup, Grade, Category, ItemID, Color, CycleTime, CavityPcs, WorkingHour, TimeWaste,ReasonWaist, CalcProduction, ActProduction, Rejection, NetProduction, ItemWeight, TotalWeight, ActualWeight, ItemName, FinalProduction, WasteWeight, ReUsableItem, Remarks, EntryBy FROM IMLOutput WHERE  ProductionID <> ' ' ORDER BY Date DESC"
                                        DeleteCommand="Delete IMLOutput where pid='0'"></asp:SqlDataSource>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <asp:Label runat="server" ID="lblEditId" Visible="False" Text=" "></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
