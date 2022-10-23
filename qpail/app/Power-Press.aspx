<%@ Page Title="Power Press" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Power-Press.aspx.cs" Inherits="app_Power_Press" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        $(document).ready(function () {
            //$('#ctl00_BodyContent_txtInputQtyKG').attr("disabled", true);
            $('#ctl00_BodyContent_TextBox5').attr("disabled", true);
            $('#ctl00_BodyContent_txtFinalProd').attr("disabled", true);
            $('#<%=txtTotalPack.ClientID%>').attr("disabled", true);
            $('#<%=txtEfficiency.ClientID%>').attr("disabled", true);
            $('#<%=txtFinalProd.ClientID%>').attr("disabled", true);
        });

        function calInvTotal() {
            var shtQty = document.getElementById("<%= txtInputQtyPCS.ClientID %>").value;
            var pkSht = document.getElementById("<%= txtPackPerSheet.ClientID %>").value;
            var ttl = parseFloat(shtQty * 1) * parseFloat(pkSht * 1);
            $('#<%=txtTotalPack.ClientID%>').val(ttl.toString());

            var ratio = document.getElementById("<%=txtWeightRatio.ClientID %>").value;
            var ttl2 = parseFloat(shtQty * 1) * parseFloat(ratio * 1);
            $('#<%=txtInputWeight.ClientID%>').val(ttl2.toString());

            //var ttl3 = (ttl2 * 1000) / (ttl * 1);
            //$('#<%=txtWeightPerPack.ClientID%>').val(ttl3.toString());

            shtQty = document.getElementById("<%= txtReusableSheetUsed.ClientID %>").value;
            pkSht = document.getElementById("<%= txtReusablePackPerSheet.ClientID %>").value;
            ttl = (ttl + (parseFloat(shtQty * 1) * parseFloat(pkSht * 1)));
            $('#<%=txtSheetQty.ClientID%>').val(ttl.toString());


            //var prd = document.getElementById("<%= txtRejected.ClientID %>").value;
            //ttl = parseFloat(ttl) - parseFloat(prd);
            //$('#<%=txtFinalProd.ClientID%>').val(ttl.toString());

            //document.getElementById("<%= txtRejected.ClientID %>").value = parseInt(txtTotal) - parseInt(txtDiscount) - parseInt(txtService);
            calInvTotal2();
            calInvTotal3();
        }

        function calInvTotal2() {
            var shtQty = document.getElementById("<%= txtSheetQty.ClientID %>").value;
            var pkSht = document.getElementById("<%= txtRejUsed.ClientID %>").value;
            var ttl = parseFloat(shtQty) + parseFloat(pkSht);
            $('#<%=txtFinalInput.ClientID%>').val(ttl.toString());
        }

        function calInvTotal3() {
            var shtQty = document.getElementById("<%= txtFinalInput.ClientID %>").value;
            var pkSht = document.getElementById("<%= txtRejected.ClientID %>").value;
            var ttl = parseFloat(shtQty) - parseFloat(pkSht);
            $('#<%=txtFinalProd.ClientID%>').val(ttl.toString());
            calEfficiency();
        }

        function calEfficiency() {
            var shtQty = document.getElementById("<%= txtFinalProd.ClientID %>").value;
            var pkSht = document.getElementById("<%= txtFinalProd.ClientID %>").value;
            var pkSht2 = document.getElementById("<%= txtStdPrdn.ClientID %>").value;
            var ttl = parseFloat(shtQty) / parseFloat(pkSht) / parseFloat(pkSht2);
            $('#<%=txtEfficiency.ClientID%>').val(ttl.toString());
        }
    </script>


</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>



            <h3 class="page-title">Power Press</h3>

            <%-- ID of Power Press is 1 --%>

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
                                <asp:Label ID="lblPrId" runat="server" Visible="False"></asp:Label>
                                <div class="control-group">
                                    <asp:Label ID="Label4" runat="server" Text="Production Date :"></asp:Label>
                                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                                        Enabled="True" TargetControlID="txtDate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </div>

                                <div class="control-group hidden">
                                    <asp:Label ID="Label9" runat="server" Text="Purpose :"></asp:Label>
                                    <asp:DropDownList ID="ddPurpose" runat="server" DataSourceID="SqlDataSource1mm"
                                        DataTextField="Purpose" DataValueField="pid" AutoPostBack="True" OnSelectedIndexChanged="ddPurpose_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1mm" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
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

                                        <asp:ListItem Value="">--- all ---</asp:ListItem>

                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label21" runat="server" Text="Brand Name: "></asp:Label>


                                    <asp:DropDownList ID="ddBrand" runat="server" DataSourceID="SqlDataSource5" DataTextField="BrandName" DataValueField="BrandID" AppendDataBoundItems="True">
                                        <asp:ListItem Value="">--- all ---</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], [BrandName] FROM [CustomerBrands] WHERE (([CustomerID] = @CustomerID) AND ([ProjectID] = @ProjectID)) Order by BrandName">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label2" runat="server" Text="Pack Size: "></asp:Label>
                                    <asp:DropDownList ID="ddSize" runat="server" DataSourceID="SqlDataSource6" DataTextField="BrandName" DataValueField="BrandID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] WHERE ([ProjectID] = @ProjectID) order by DisplaySl">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>



                                <a name="ItemDetails"></a>
                                <legend style="margin-bottom: 6px;">Production Input</legend>
                                <%--<asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>--%>

                                <div class="control-group">
                                    <asp:Label ID="Label6" runat="server" Text="Machine No.: "></asp:Label>
                                    <asp:DropDownList ID="ddMachine" runat="server" DataSourceID="SqlDataSource2"
                                        DataTextField="MachineNo" DataValueField="mid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT CONCAT(MachineNo,'  ',Description) AS MachineNo, mid FROM Machines WHERE Section=1 ORDER BY MachineNo"></asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label7" runat="server" Text="Line# : "></asp:Label>
                                    <asp:DropDownList ID="ddLine" runat="server" DataSourceID="SqlDataSource4"
                                        DataTextField="DepartmentName" DataValueField="Departmentid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Lines] where Section=1 ORDER BY [DepartmentName]"></asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label32" runat="server" Text="For Company: "></asp:Label>
                                    <asp:DropDownList ID="ddCustomer2" runat="server" DataSourceID="SqlDataSource82"
                                        DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddCustomer2_OnSelectedIndexChanged">

                                        <asp:ListItem Value="">--- all ---</asp:ListItem>

                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource82" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label33" runat="server" Text="Brand Name: "></asp:Label>
                                    <asp:DropDownList ID="ddBrand2" runat="server" DataSourceID="SqlDataSource52" DataTextField="BrandName" DataValueField="BrandID" AppendDataBoundItems="True"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddBrand2_OnSelectedIndexChanged">
                                        <asp:ListItem Value="">--- all ---</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource52" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], [BrandName] FROM [CustomerBrands] WHERE (([CustomerID] = @CustomerID) AND ([ProjectID] = @ProjectID)) Order by BrandName">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddCustomer2" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label34" runat="server" Text="Pack Size: "></asp:Label>
                                    <asp:DropDownList ID="ddSize2" runat="server" DataSourceID="SqlDataSource62" DataTextField="BrandName" DataValueField="BrandID" AutoPostBack="True" OnSelectedIndexChanged="ddSize2_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource62" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] WHERE ([ProjectID] = @ProjectID) order by DisplaySl">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="form-group">
                                    <label>Item Sub-group : </label>
                                    <asp:DropDownList ID="ddSubGrp" CssClass="form-control select2me" runat="server">
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

                                <div class="control-group">
                                    <label class="control-label">Item Name :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddItemNameRaw" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddItemNameRaw_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <asp:Panel ID="PnlColor" runat="server" Visible="False">

                                    <div class="control-group">
                                        <label class="control-label">Color :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddColor" runat="server" DataSourceID="SqlDataSource9" DataTextField="DepartmentName" DataValueField="Departmentid"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddColor_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource9" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [DepartmentName], [Departmentid] FROM [Colors] ORDER BY [DepartmentName]"></asp:SqlDataSource>

                                        </div>
                                    </div>
                                </asp:Panel>

                                <div class="control-group">
                                    <label class="control-label">Stock Type :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddstockType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddstockType_OnSelectedIndexChanged">

                                            <asp:ListItem>Processed Sheet</asp:ListItem>
                                            <asp:ListItem>Printed Sheet</asp:ListItem>

                                        </asp:DropDownList>
                                        <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>
                                        <span style="width: 70%; color: green; float: right">
                                            <asp:Literal ID="ltrLastInfo" runat="server">Stock Qty.: </asp:Literal>
                                        </span>

                                    </div>
                                </div>

                                <div class="control-group hidden">
                                    <label class="control-label full-wdth">Stock Godown : </label>
                                    <asp:DropDownList ID="ddGodown" runat="server" DataSourceID="SqlDataSource12"
                                        DataTextField="StoreName" DataValueField="WID" CssClass="form-control"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddGodown_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource12" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [WID], [StoreName] FROM [Warehouses] WHERE WID = '4'"></asp:SqlDataSource>
                                </div>

                                <div class="control-group hidden">
                                    <label class="control-label full-wdth">From / Location : </label>
                                    <asp:DropDownList ID="ddLocation" runat="server" DataSourceID="SqlDataSource15" DataTextField="AreaName" DataValueField="AreaID" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource15" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT AreaID, [AreaName] FROM [WareHouseAreas] WHERE Warehouse=@Warehouse">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddGodown" Name="Warehouse" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <%--<div class="control-group">
                                    <asp:Label ID="Label1" runat="server" Text="Input Weight (kg.) : "></asp:Label>
                                    <asp:TextBox ID="txtInputQtyKG" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtInputQtyKG">
                                    </asp:FilteredTextBoxExtender>
                                </div>--%>

                                <div class="control-group">
                                    <asp:Label ID="Label19" runat="server" Text="Input Sheet Qty.(PCS) : "></asp:Label>
                                    <asp:TextBox ID="txtInputQtyPCS" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtInputQtyPCS">
                                    </asp:FilteredTextBoxExtender>
                                </div>


                                <div class="control-group">
                                    <asp:Label ID="Label14" runat="server" Text="Pack /Sheet : "></asp:Label>
                                    <asp:TextBox ID="txtPackPerSheet" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtPackPerSheet">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label3" runat="server" Text="Total Input Pack : "></asp:Label>
                                    <asp:TextBox ID="txtTotalPack" runat="server"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label24" runat="server" Text="Input Sheet Weight (Kg): "></asp:Label>
                                    <asp:TextBox ID="txtInputWeight" runat="server"></asp:TextBox>
                                    <asp:TextBox ID="txtWeightRatio" runat="server" CssClass="hidden"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13"
                                        runat="server" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtInputWeight">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label25" runat="server" Text="Weight Per Pack (gm) : "></asp:Label>
                                    <asp:TextBox ID="txtWeightPerPack" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtWeightPerPack">
                                    </asp:FilteredTextBoxExtender>
                                </div>


                                <div class="form-group">
                                    <label>Reusable Sheet Item Name :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddReusableSheetItem" runat="server" DataSourceID="SqlDataSource1" CssClass="select2me form-control"
                                            DataTextField="ItemName" DataValueField="ProductID" AutoPostBack="True" OnSelectedIndexChanged="ddReusableSheetItem_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT        ProductID, ItemName FROM            Products WHERE        (CategoryID IN (SELECT        CategoryID FROM            Categories WHERE        (GradeID IN (SELECT        GradeID FROM            ItemGrade WHERE        (CategoryID = 29))))) order by ItemName"></asp:SqlDataSource>

                                        <asp:Label ID="Label26" runat="server" Text="0" Visible="False"></asp:Label>
                                        <span style="width: 70%; color: green; float: right">
                                            <asp:Literal ID="ltrReUsableSheet" runat="server">Available Pack ...pcs ... kg </asp:Literal>
                                        </span>
                                    </div>
                                </div>



                                <div class="control-group">
                                    <asp:Label ID="Label27" runat="server" Text="Reusable Sheet Used (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtReusableSheetUsed" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtRejUsed">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label28" runat="server" Text="Pack /Sheet : "></asp:Label>
                                    <asp:TextBox ID="txtReusablePackPerSheet" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtPackPerSheet">
                                    </asp:FilteredTextBoxExtender>
                                </div>





                                <a name="OpDetails"></a>
                                <legend style="margin-bottom: 6px;">Operation Details</legend>
                                <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>

                                <div class="control-group">
                                    <asp:Label ID="Label11x" runat="server" Text="Total Input Pack (Pcs): "></asp:Label>
                                    <asp:TextBox ID="txtSheetQty" runat="server" onkeyup="calInvTotal2()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8"
                                        runat="server" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtSheetQty">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="form-group">
                                    <label>Reusable Pack Item Name :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddReusableUsed" runat="server" DataSourceID="SqlDataSource12r" CssClass="select2me form-control"
                                            DataTextField="ItemName" DataValueField="ProductID" AutoPostBack="True" OnSelectedIndexChanged="ddReusableUsed_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource12r" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT        ProductID, ItemName FROM            Products WHERE        (CategoryID IN (SELECT        CategoryID FROM            Categories WHERE        (GradeID IN (SELECT        GradeID FROM            ItemGrade WHERE        (CategoryID = 29))))) order by ItemName"></asp:SqlDataSource>

                                        <asp:Label ID="lblWeightPerPc" runat="server" Text="0" Visible="False"></asp:Label>
                                        <span style="width: 70%; color: green; float: right">
                                            <asp:Literal ID="ltrReUsable" runat="server"> </asp:Literal>
                                        </span>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label11" runat="server" Text="Reusable Pack Used (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtRejUsed" runat="server" onkeyup="calInvTotal2()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtRejUsed">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label18" runat="server" Text="Available Pack Qty (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtFinalInput" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtFinalInput">
                                    </asp:FilteredTextBoxExtender>
                                </div>


                                <div class="control-group">
                                    <asp:Label ID="Label12" runat="server" Text="Operation Name: "></asp:Label>
                                    <asp:DropDownList ID="ddOperation" runat="server" DataSourceID="SqlDataSource9x"
                                        DataTextField="DepartmentName" DataValueField="Departmentid" AutoPostBack="True" OnSelectedIndexChanged="ddOperation_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource9x" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Satges] where Section='1' ORDER BY [DepartmentName]"></asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label22" runat="server" Text="Std Prdn Per Hr. : "></asp:Label>
                                    <asp:TextBox ID="txtStdPrdn" runat="server" ReadOnly="True" onkeyup="calEfficiency()"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label13" runat="server" Text="Operator: "></asp:Label>
                                    <asp:DropDownList ID="ddOperator" runat="server" DataSourceID="SqlDataSource10"
                                        DataTextField="EName" DataValueField="EmployeeInfoID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource10" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [EmployeeInfoID], [EName] FROM [EmployeeInfo] WHERE ([SectionID] = '1') ORDER BY [EName]"></asp:SqlDataSource>
                                </div>


                                <div class="control-group">
                                    <asp:Label ID="Label15" runat="server" Text="Rejection (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtRejected" runat="server" onkeyup="calInvTotal3()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtRejected">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label10" runat="server" Text="Net Production (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtFinalProd" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtFinalProd">
                                    </asp:FilteredTextBoxExtender>
                                </div>



                                <div class="control-group">
                                    <asp:Label ID="Label5" runat="server" Text="Working hr. : "></asp:Label>
                                    <asp:TextBox ID="txtHour" runat="server" onkeyup="calEfficiency()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtHour">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label8" runat="server" Text="Time Waste (Min) : "></asp:Label>
                                    <asp:TextBox ID="txtTimeWaist" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtTimeWaist">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label16" runat="server" Text="Reason of Time Waste  : "></asp:Label>
                                    <asp:TextBox ID="txtReason" runat="server"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label17" runat="server" Text="Shift: "></asp:Label>
                                    <asp:DropDownList ID="ddShift" runat="server" DataSourceID="SqlDataSource11"
                                        DataTextField="DepartmentName" DataValueField="Departmentid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource11" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Shifts] where Section='1' ORDER BY [DepartmentName]"></asp:SqlDataSource>
                                </div>

                                <%-- <div class="control-group">
                                    <asp:Label ID="Label18" runat="server" Text="Final Production(PCS) : "></asp:Label>
                                    <asp:TextBox ID="txtFinalProd" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtFinalProd">
                                    </asp:FilteredTextBoxExtender>
                                </div>--%>

                                <div class="form-group">
                                    <label class="control-label">Westage Item :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddReuseWaste" runat="server" DataSourceID="SqlDataSource12y" DataTextField="ItemName" DataValueField="ProductID" CssClass="form-control select2me">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource12y" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT ProductID, ItemName FROM Products WHERE (CategoryID IN (SELECT CategoryID FROM Categories WHERE (GradeID IN (SELECT GradeID FROM ItemGrade WHERE        (CategoryID = 29)))))"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label23" runat="server" Text="Reusable Waste (PCS) : "></asp:Label>
                                    <asp:TextBox ID="txtReUsable" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtReUsable">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label29" runat="server" Text="Weight Per Pack (gm) : "></asp:Label>
                                    <asp:TextBox ID="txtReusableWeightPerPack" runat="server" ToolTip=""></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtReusableWeightPerPack">
                                    </asp:FilteredTextBoxExtender>
                                </div>


                                <div class="control-group">
                                    <label class="control-label">Operator Efficiency : </label>
                                    <asp:TextBox ID="txtEfficiency" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>




                                <asp:Panel ID="ringPanel" runat="server">

                                    <div class="form-group">
                                        <label class="control-label">Westage Item (Ring) :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddRingWasteItem" runat="server" DataSourceID="SqlDataSource14" DataTextField="ItemName" DataValueField="ProductID" CssClass="form-control select2me">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource14" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT ProductID, ItemName FROM Products WHERE (CategoryID IN (SELECT CategoryID FROM Categories WHERE (GradeID IN (SELECT GradeID FROM ItemGrade WHERE        (CategoryID = 29)))))"></asp:SqlDataSource>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <asp:Label ID="Label30" runat="server" Text="Reusable Waste (PCS) : "></asp:Label>
                                        <asp:TextBox ID="txtRingWaste" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtRingWaste">
                                        </asp:FilteredTextBoxExtender>
                                    </div>

                                    <div class="control-group">
                                        <asp:Label ID="Label31" runat="server" Text="Weight Per Pack (gm) : "></asp:Label>
                                        <asp:TextBox ID="txtRingWeightPerPack" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtRingWeightPerPack">
                                        </asp:FilteredTextBoxExtender>
                                    </div>

                                </asp:Panel>



                                <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" CssClass="right block" />

                                <div style="clear: both"></div>

                                <div class="table-responsive">
                                    <asp:Label ID="lblEid" runat="server" Text="" Visible="False"></asp:Label>

                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridView2_OnRowDataBound"
                                        DataSourceID="SqlDataSource7" Width="250%" OnSelectedIndexChanged="GridView2_OnSelectedIndexChanged" OnRowDeleting="GridView2_OnRowDeleting" DataKeyNames="Id">
                                        <Columns>

                                            <asp:TemplateField ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                    <asp:Label ID="Label1" runat="server" CssClass="hidden" Text='<%# Bind("id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


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

                                            <asp:BoundField DataField="InputQty" HeaderText="Input Qty" SortExpression="InputQty" />
                                            <asp:BoundField DataField="ReusableItemUsed" HeaderText="Reusable Item" SortExpression="ReusableItemUsed" />
                                            <asp:BoundField DataField="RejectedUsed" HeaderText="Rej. Qty Used" SortExpression="RejectedUsed" />
                                            <asp:BoundField DataField="FinalInputQty" HeaderText="Final Input Qty" SortExpression="FinalInputQty" />
                                            <asp:BoundField DataField="OperationName" HeaderText="Operation Name" SortExpression="OperationName" />
                                            <asp:BoundField DataField="StdProduction" HeaderText="Std Production" SortExpression="StdProduction" />
                                            <asp:BoundField DataField="Operator" HeaderText="Operator" SortExpression="Operator" />
                                            <asp:BoundField DataField="Rejected" HeaderText="Rejected" SortExpression="Rejected" />
                                            <asp:BoundField DataField="NetPrdnSheet" HeaderText="Net Prdn Sheet" SortExpression="NetPrdnSheet" />
                                            <asp:BoundField DataField="WorkingHr" HeaderText="Working Hr." SortExpression="WorkingHr" />
                                            <asp:BoundField DataField="TimeWaste" HeaderText="Time Waste" SortExpression="TimeWaste" />
                                            <asp:BoundField DataField="Reason" HeaderText="Reason" SortExpression="Reason" />
                                            <asp:BoundField DataField="Shift" HeaderText="Shift" SortExpression="Shift" />
                                            <asp:BoundField DataField="ReusableItem" HeaderText="Reusable Item" SortExpression="ReusableItem" />
                                            <asp:BoundField DataField="ReUsableQty" HeaderText="ReUsable Qty" SortExpression="ReUsableQty" />
                                            <asp:BoundField DataField="NonUsableWasteWeight" HeaderText="NonUsable Weight" SortExpression="NonUsableWasteWeight" />

                                            <asp:BoundField DataField="OperatorEfficiency" HeaderText="Operator Efficiency" SortExpression="OperatorEfficiency" />

                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT Id, InputQty, 
                                        (Select ItemName from Products where ProductID=a.ReusableItemUsed) AS ReusableItemUsed, RejectedUsed, FinalInputQty,
                                        (Select DepartmentName from Satges where Departmentid=a.OperationName) AS OperationName, StdProduction, Operator, Rejected, NetPrdnSheet, 
                                        WorkingHr, TimeWaste, Reason, Shift,
                                        (Select ItemName from Products where ProductID=a.ReusableItem) AS  ReusableItem, ReUsableQty, NonUsableWasteWeight, OperatorEfficiency FROM [PrdnPowerPressDetails] a where PrdnID=''  ORDER BY [id]"
                                        DeleteCommand="Delete FROM [PrdnPowerPressDetails]  where PrdnID='0'  "></asp:SqlDataSource>

                                </div>

                                Total Input:
                                <asp:Label ID="lblTotalInputWeight" runat="server" Text="0"></asp:Label>kg. 
                                , Usable Weight:
                                <asp:Label ID="lblTotalOutputWeight" runat="server" Text="0"></asp:Label>kg.
                                , Nonusable Weight:
                                <asp:Label ID="lblNonusableWeight" runat="server" Text="0"></asp:Label>kg.
                                <asp:Label ID="lblLastPrdnPack" runat="server" Text="" Visible="False"></asp:Label>

                                <a name="ItemDetails"></a>
                                <legend style="margin-bottom: 6px;">Production Output</legend>
                                <%--<asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>--%>

                                <div class="form-group">
                                    <label>Item Sub-group : </label>
                                    <asp:DropDownList ID="ddOutSubGroup" CssClass="form-control select2me" runat="server">
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
                                            OnSelectedIndexChanged="ddOutCategory_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Processed Item :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddOutItem" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddItemNameRaw_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Color :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddOutputColor" runat="server" DataSourceID="SqlDataSource13" DataTextField="DepartmentName" DataValueField="Departmentid">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource13" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [DepartmentName], [Departmentid] FROM [Colors] ORDER BY [DepartmentName]"></asp:SqlDataSource>

                                    </div>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label20" runat="server" Text="Final Output (PCS) : "></asp:Label>
                                    <asp:TextBox ID="txtFinalOutput" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtFinalOutput">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <%--<div class="control-group">
                                    <asp:Label ID="Label13x" runat="server" Text="Total Westage (kg.) : "></asp:Label>
                                    <asp:TextBox ID="txtTtlWaste" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtTtlWaste">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                                
                                <div class="control-group">
                                    <label class="control-label">Westage Item :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddWasteStock" runat="server" DataSourceID="SqlDataSource12x" DataTextField="ItemName" DataValueField="ProductID">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource12x" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT        ProductID, ItemName FROM            Products WHERE        (CategoryID IN (SELECT        CategoryID FROM            Categories WHERE        (GradeID IN (SELECT        GradeID FROM            ItemGrade WHERE        (CategoryID = 29)))))"></asp:SqlDataSource>
                                    </div>
                                </div>--%>

                                <div class="control-group">
                                    <label class="control-label">Department Efficiency : </label>
                                    <asp:TextBox ID="txtDeptEfficiency" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>


                                <div class="control-group">
                                    <label class="control-label">Remarks : </label>
                                    <asp:TextBox ID="txtRemark" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>

                                <div class="form-actions">
                                    <asp:Button ID="btnSave" runat="server" Text="Save"
                                        OnClick="btnSave_Click" />

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

                                    <asp:GridView ID="GridView1" runat="server" Width="350%" AllowSorting="True"
                                        AutoGenerateColumns="False" BackColor="White" Borderpayor="#999999"
                                        BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black"
                                        GridLines="Vertical" DataSourceID="SqlDataSource3" PageSize="15" AllowPaging="True" OnRowDeleting="GridView1_OnRowDeleting">
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

                                            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="ProductionID" HeaderText="Production ID" SortExpression="ProductionID" />
                                            <asp:BoundField DataField="MachineNo" HeaderText="Machine No" SortExpression="MachineNo" />
                                            <asp:BoundField DataField="LineNumber" HeaderText="Line Number" SortExpression="LineNumber" />

                                            <asp:BoundField DataField="Purpose" HeaderText="Purpose" SortExpression="Purpose" />
                                            <asp:BoundField DataField="CustomerID" HeaderText="Company" SortExpression="CustomerID" />

                                            <asp:BoundField DataField="Brand" HeaderText="Brand" SortExpression="Brand" />
                                            <asp:BoundField DataField="PackSize" HeaderText="Pack Size" SortExpression="PackSize" />
                                            <%--<asp:BoundField DataField="ItemSubGroup" HeaderText="SubGroup" SortExpression="ItemSubGroup" />
                                            <asp:BoundField DataField="ItemGrade" HeaderText="Grade" SortExpression="ItemGrade" />
                                            <asp:BoundField DataField="ItemCategory" HeaderText="Category" SortExpression="ItemCategory" />--%>
                                            <asp:BoundField DataField="ItemName" HeaderText="Item Name" SortExpression="ItemName" />
                                            <asp:BoundField DataField="Color" HeaderText="Color" SortExpression="Color" />
                                            <asp:BoundField DataField="StockType" HeaderText="Stock Type" SortExpression="StockType" />
                                            <asp:BoundField DataField="InputSheetQty" HeaderText="Input Sheet Qty" SortExpression="InputSheetQty" />
                                            <asp:BoundField DataField="PackPerSheet" HeaderText="Pack Per Sheet" SortExpression="PackPerSheet" />
                                            <asp:BoundField DataField="TotalInputPack" HeaderText="Total Input Pack" SortExpression="TotalInputPack" />
                                            <asp:BoundField DataField="InputWeight" HeaderText="Input Weight" SortExpression="InputWeight" />
                                            <asp:BoundField DataField="WeightPerPack" HeaderText="Weight/Pack" SortExpression="WeightPerPack" />
                                            <asp:BoundField DataField="OutputItem" HeaderText="Output Item" SortExpression="OutputItem" />
                                            <asp:BoundField DataField="FinalOutput" HeaderText="Final Output" SortExpression="FinalOutput" />
                                            <asp:BoundField DataField="DepartmentEfficiency" HeaderText="Dept. Efficiency" SortExpression="DepartmentEfficiency" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Date], [ProductionID],  pid,
                                        (Select MachineNo from Machines where mid=a.MachineNo) AS [MachineNo], 
                                    (Select DepartmentName from Lines where Departmentid=a.LineNumber) AS [LineNumber],
                                       (Select Purpose from Purpose where pid=a.Purpose) AS  [Purpose], 
                                       (Select Company from Party where PartyID=a.CustomerID) AS  [CustomerID], 
                                       (Select BrandName from CustomerBrands where BrandID=a.Brand) AS  [Brand], 
                                       (Select BrandName from Brands where BrandID=a.PackSize) AS  [PackSize], 
                                       (Select ItemName from Products where ProductID=a.ItemName) AS  [ItemName], 
                                        
                                        [Color], [StockType], [InputSheetQty], [PackPerSheet], [TotalInputPack], [InputWeight], [WeightPerPack], [OutputItem], [FinalOutput], [DepartmentEfficiency]  FROM [PrdnPowerPress] a ORDER BY [Date] desc, pid desc"
                                        DeleteCommand="Delete PrdnShearing where pid=0">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="Offset Printing" Name="Section" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>
                                </fieldset>
                                
                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
