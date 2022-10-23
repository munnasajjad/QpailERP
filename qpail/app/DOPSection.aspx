<%@ Page Title="DOP Section" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="DOPSection.aspx.cs" Inherits="app_DOPSection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        <%--function calFinishGoods() {
            var outputQty = document.getElementById("<%= txtFinalOutput.ClientID %>").value;
            var wastage = document.getElementById("<%= txtWastage.ClientID %>").value;
            var reusableQuantity = document.getElementById("<%= txtCrushingWastageQuantity.ClientID %>").value;

            var finshGoods = parseFloat(outputQty) - parseFloat(wastage) - parseFloat(reusableQuantity);
            $('#<%=txtReusableItemQuantity.ClientID%>').val(finshGoods.toString());
            //calInpTotal();
        }--%>
        function calFinishGoods() {
            var inputQuantity = document.getElementById("<%= txtInputQtyPCS.ClientID %>").value;
            var finalOutput = document.getElementById("<%= txtFinalOutput.ClientID %>").value;
            var wastage = document.getElementById("<%= txtCrushingWastageQuantity.ClientID %>").value;
            <%--var reusableQuantity = document.getElementById("<%= txtCrushingWastageQuantity.ClientID %>").value;--%>

            var finshGoods = parseFloat(inputQuantity) - parseFloat(finalOutput) - parseFloat(wastage);
            $('#<%=txtReusableItemQuantity.ClientID%>').val(finshGoods.toString());
            //calInpTotal();
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

            <h3 class="page-title">DOP Section</h3>

            <%-- ID of Power Press is 8 --%>

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

                                <a name="ItemDetails"></a>
                                <legend style="margin-bottom: 6px;">Production Input (Processed Item)</legend>
                                <%--<asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>--%>
                                <div class="control-group">
                                    <asp:Label ID="Label4" runat="server" Text="Production Date :"></asp:Label>
                                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                                        Enabled="True" TargetControlID="txtDate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </div>

                                <div class="control-group hidden">
                                    <asp:Label ID="Label24" runat="server" Text="For Company: "></asp:Label>
                                    <asp:DropDownList ID="ddInputCustomer" runat="server" DataSourceID="SqlDataSource16"
                                        DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="True"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddInputCustomer_OnSelectedIndexChanged">

                                        <asp:ListItem Value="">--- all ---</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource16" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <%-- 22/10/2020 zakir vai said that pack size will be included in production input section--%>
                                <div class="control-group hidden">
                                    <asp:Label ID="Label14" runat="server" Text="Pack Size: "></asp:Label>
                                    <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource20" DataTextField="BrandName" DataValueField="BrandID"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddSize_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource20" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] WHERE ([ProjectID] = @ProjectID) order by DisplaySl">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group hidden">
                                    <asp:Label ID="Label25" runat="server" Text="Brand Name: "></asp:Label>
                                    <asp:DropDownList ID="ddInputBrand" runat="server" DataSourceID="SqlDataSource17" DataTextField="BrandName" DataValueField="BrandID" AppendDataBoundItems="True"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddInputBrand_OnSelectedIndexChanged">
                                        <asp:ListItem Value="">--- all ---</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource17" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], [BrandName] FROM [CustomerBrands] WHERE (([CustomerID] = @CustomerID) AND ([ProjectID] = @ProjectID)) Order by BrandName">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddInputCustomer" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>

                                <div class="form-group hidden">
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

                                <div class="control-group hidden">
                                    <label class="control-label">Item Category :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddCategoryRaw" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddCategoryRaw_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Item Name : </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddItemNameRaw" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddItemNameRaw_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="Label29" runat="server" Text="Processed Item Pack Size: "></asp:Label>
                                    <asp:DropDownList ID="ddSemifinishedPackSize" runat="server" DataSourceID="SqlDataSource22" DataTextField="Size" DataValueField="Id"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddSemifinishedPackSize_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource22" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Id], [Size] FROM [SemiFinishedPackSize] WHERE ([ProjectID] = @ProjectID) ORDER BY Size">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">From Godown : </label>
                                    <asp:DropDownList ID="ddGodown" runat="server" DataSourceID="SqlDataSource19"
                                        DataTextField="StoreName" DataValueField="WID" AutoPostBack="True" OnSelectedIndexChanged="ddGodown_OnSelectedIndexChanged" CssClass="form-control" Enabled="False">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource19" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [WID], [StoreName] FROM Warehouses WHERE WID='8' "></asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Location : </label>
                                    <asp:DropDownList ID="ddLocation" runat="server" DataSourceID="SqlDataSource21"
                                        DataTextField="AreaName" DataValueField="AreaID" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddLocation_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource21" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT AreaID,[AreaName] FROM [WareHouseAreas] WHERE Warehouse=@Warehouse">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddGodown" Name="Warehouse" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group hidden">
                                    <label class="control-label">Wastage Godown : </label>
                                    <asp:DropDownList ID="ddWastageGodown" runat="server" DataSourceID="SqlDataSource2"
                                        DataTextField="StoreName" DataValueField="WID" AutoPostBack="True" CssClass="form-control" Enabled="False">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [WID], [StoreName] FROM Warehouses WHERE WID='5' "></asp:SqlDataSource>
                                </div>

                                <div class="control-group hidden">
                                    <label class="control-label">Wastage Location : </label>
                                    <asp:DropDownList ID="ddWastageGodownLocation" runat="server" DataSourceID="SqlDataSource4"
                                        DataTextField="AreaName" DataValueField="AreaID" CssClass="form-control" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT AreaID,[AreaName] FROM [WareHouseAreas] WHERE Warehouse=@Warehouse">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddWastageGodown" Name="Warehouse" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Color :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddColor2" runat="server" DataSourceID="SqlDataSource15" DataTextField="DepartmentName" DataValueField="Departmentid"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddColor2_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource15" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [DepartmentName], [Departmentid] FROM [Colors] ORDER BY [Departmentid]"></asp:SqlDataSource>
                                        <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>
                                        <span style="width: 70%; color: green; float: right">
                                            <asp:Literal ID="ltrCurrentStock" runat="server">Available  Stock: Qnty(Pcs).... Qnty(Kg).... </asp:Literal>
                                        </span>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label19" runat="server" Text="Input Qty.(PCS) : "></asp:Label>
                                    <asp:TextBox ID="txtInputQtyPCS" runat="server"></asp:TextBox>
                                    <%--  <asp:TextBox ID="txtInputQtyPCS" runat="server" onkeyup="calInvTotal()" ></asp:TextBox>--%>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtInputQtyPCS">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group hidden">
                                    <asp:Label ID="Label26" runat="server" Text="Weight/Pack (gm) : "></asp:Label>
                                    <asp:TextBox ID="txtWeightPc" runat="server"></asp:TextBox>
                                    <%--  <asp:TextBox ID="txtWeightPc" runat="server" onkeyup="calInvTotal()"></asp:TextBox>--%>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtWeightPc">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group hidden">
                                    <asp:Label ID="Label12" runat="server" Text="Input Quantity (kg) : "></asp:Label>
                                    <asp:TextBox ID="txtInputWeight" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtWeightPc">
                                    </asp:FilteredTextBoxExtender>
                                </div>


                                <a name="ItemDetails"></a>
                                <legend style="margin-bottom: 6px;">Production Output (Semi Finished)</legend>
                                <%--<asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>--%>

                                <asp:Panel ID="pnlTinBodyOutput" runat="server">

                                    <div class="control-group hidden">
                                        <asp:Label ID="Label9" runat="server" Text="Purpose :"></asp:Label>
                                        <asp:DropDownList ID="ddPurpose" runat="server" DataSourceID="SqlDataSource1"
                                            DataTextField="Purpose" DataValueField="pid" AutoPostBack="True" OnSelectedIndexChanged="ddPurpose_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [pid], [Purpose] FROM [Purpose] ORDER BY Purpose">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>

                                    <div class="control-group">
                                        <asp:Label ID="lblDeptName" runat="server" Text="For Company: "></asp:Label>
                                        <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource8"
                                            DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="True"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddCustomer_OnSelectedIndexChanged">
                                            <%--<asp:ListItem Value="">--- all ---</asp:ListItem>--%>
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
                                        <asp:DropDownList ID="ddBrand" runat="server" DataSourceID="SqlDataSource5" DataTextField="BrandName"
                                            DataValueField="BrandID" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddBrand_SelectedIndexChanged">
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
                                        <asp:DropDownList ID="ddSize" runat="server" DataSourceID="SqlDataSource6" DataTextField="BrandName" DataValueField="BrandID"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddSize_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] WHERE ([ProjectID] = @ProjectID) order by DisplaySl">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                    <div class="form-group hidden">
                                        <label>Item Sub-group : </label>
                                        <asp:DropDownList ID="ddOutSubGroup" CssClass="form-control select2me" runat="server">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Printed Item Grade :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddOutGrade" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddOutGrade_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Printed Item Category :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddOutCategory" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddOutCategory_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Printed Item Name:  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddOutItem" runat="server" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                </asp:Panel>


                                <div class="control-group hidden">
                                    <label class="control-label">Color :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddOutputColor" runat="server" DataSourceID="SqlDataSource13" DataTextField="DepartmentName" DataValueField="Departmentid" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource13" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [DepartmentName], [Departmentid] FROM [Colors] ORDER BY [DepartmentName]"></asp:SqlDataSource>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="Label20" runat="server" Text="Printing Output Qty (pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtFinalOutput" runat="server" onkeyup="calFinishGoods()" OnTextChanged="txtFinalOutput_OnTextChanged" AutoPostBack="True"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtFinalOutput">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                                <div class="control-group hidden">
                                    <asp:Label ID="Label27" runat="server" Text="Reject Handle Qty (pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtHandleReject" runat="server" onkeyup="calFinishGoods()"></asp:TextBox>
                                </div>
                                <div class="control-group hidden">
                                    <asp:Label ID="Label30" runat="server" Text="Wastage Qty (pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtWastage" runat="server" onkeyup="calFinishGoods()"></asp:TextBox>
                                </div>
                                <div class="control-group">
                                    <label>Crushing Wastage Item :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddCrushingWastageItem" runat="server" DataSourceID="SqlDataSource12" DataTextField="ItemName" DataValueField="ProductID" CssClass="">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT P.ProductID, CONCAT(P.ItemName,'>>',C.CategoryName)AS ItemName, C. CategoryID, C.GradeID, C.CategoryName, C.Description, C.ProjectID, I.GradeName FROM Categories AS C 
                                            INNER JOIN ItemGrade AS I ON C.GradeID = I.GradeID 
                                            INNER JOIN Products AS P ON P.CategoryID = C.CategoryID 
                                            WHERE I.CategoryID = 28 AND I.GradeID = 1070 ORDER BY P.ItemName ASC"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label5" runat="server" Text="Crushing Wastage Qty (pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtCrushingWastageQuantity" Text="0" runat="server" onkeyup="calFinishGoods()"></asp:TextBox>
                                </div>

                                <div class="control-group hidden">
                                    <asp:Label ID="Label6" runat="server" Text="Crushing Wastage Weight (kg) : "></asp:Label>
                                    <asp:TextBox ID="txtCrushingWastageWeight" runat="server" onkeyup="calFinishGoods()"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label7" runat="server" Text="Reusable Item : "></asp:Label>
                                    <asp:TextBox ID="txtReusableItem" runat="server" ReadOnly="True"></asp:TextBox>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="Label28" runat="server" Text="Reusable Qty (pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtReusableItemQuantity" runat="server" onkeyup="calFinishGoods()"></asp:TextBox>
                                </div>
                                <div class="control-group hidden">
                                    <asp:Label ID="Label3" runat="server" Text="Final Output (kg) : "></asp:Label>
                                    <asp:TextBox ID="txtFinalOutputKg" runat="server"></asp:TextBox>
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

                                    <asp:GridView ID="GridView1" runat="server" Width="250%" AllowSorting="True"
                                        AutoGenerateColumns="False" BackColor="White" Borderpayor="#999999"
                                        BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" OnRowDeleting="GridView1_OnRowDeleting"
                                        GridLines="Vertical" DataSourceID="SqlDataSource3" AllowPaging="True" PageSize="15" OnPageIndexChanging="GridView1_OnPageIndexChanging"
                                        OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged">

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


                                            <asp:BoundField DataField="Purpose" HeaderText="Purpose" SortExpression="Purpose" />
                                            <asp:BoundField DataField="CustomerID" HeaderText="Customer" SortExpression="CustomerID" />

                                            <asp:BoundField DataField="Brand" HeaderText="Brand" SortExpression="Brand" />
                                            <asp:BoundField DataField="PackSize" HeaderText="PackSize" SortExpression="PackSize" />
                                            <asp:BoundField DataField="ItemName" HeaderText="ItemName" SortExpression="ItemName" />
                                            <asp:BoundField DataField="FinalOutput" HeaderText="FinalOutput" SortExpression="FinalOutput" />
                                            <asp:BoundField DataField="FinalOutputKg" HeaderText="FinalOutputKg" SortExpression="FinalOutputKg" />
                                            <asp:BoundField DataField="RejHandleQty" HeaderText="RejHandleQty" SortExpression="RejHandleQty" />
                                            <asp:BoundField DataField="FinishGoodsQty" HeaderText="FinishGoodsQty" SortExpression="FinishGoodsQty" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [ProductionID], [Date], pid, 
                                        (SELECT Purpose FROM Purpose WHERE pid=PSP.Purpose) AS  [Purpose], 
                                       (SELECT Company FROM Party WHERE PartyID=PSP.CustomerID) AS  [CustomerID], 
                                       (SELECT BrandName FROM CustomerBrands WHERE BrandID=PSP.Brand) AS  [Brand], 
                                       (SELECT BrandName FROM Brands WHERE BrandID=PSP.PackSize) AS  [PackSize], 
                                       (SELECT ItemName FROM Products WHERE ProductID=PSP.ItemName) AS  [ItemName],     
                                         [FinalOutput], [FinalOutputKg],RejHandleQty,FinishGoodsQty, [Remarks] FROM [PrdScreenPrint] AS PSP ORDER BY [date] DESC, pid DESC"
                                        DeleteCommand="Delete PrdPlasticCont where pid='0'"></asp:SqlDataSource>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Label runat="server" ID="lblEditId" Visible="False" Text=" "></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
