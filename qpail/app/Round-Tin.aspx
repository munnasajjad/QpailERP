<%@ Page Title="Production : Round Tin" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Round-Tin.aspx.cs" Inherits="app_Round_Tin" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        $(document).ready(function () {
            $('#<%=txtInputQtyKG.ClientID%>').attr("disabled", true);
             $('#<%=txtNetPrdn.ClientID%>').attr("disabled", true);
             //$('#ctl00_BodyContent_txtTotalPack').attr("disabled", true);
         });

        function calInvTotal() {
            var shtQty = document.getElementById("<%= txtWeightPc.ClientID %>").value;
             var rejUsed = document.getElementById("<%= txtInputQtyPCS.ClientID %>").value;
             var ttl = (shtQty * 1) * (rejUsed * 1) / 1000;
             $('#<%=txtInputQtyKG.ClientID%>').val(ttl.toString());
            calInpTotal();
        }

        function calInpTotal() {
            var shtQty = document.getElementById("<%= txtproduced.ClientID %>").value;
             var rejUsed = document.getElementById("<%= txtRejected.ClientID %>").value;
             var ttl = (shtQty * 1) - (rejUsed * 1);
             $('#<%=txtNetPrdn.ClientID%>').val(ttl.toString());

             //var rejUsed2 = document.getElementById("<%= txtRejected.ClientID %>").value;
             //ttl = ttl - parseFloat(rejUsed2);
             //$('#<%=txtproduced.ClientID%>').val(ttl.toString());
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


            <h3 class="page-title">Round Tin</h3>
            <%--Payment From Members--%>

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

                                <div class="form-group">
                                    <label>Operator: </label>
                                    <asp:DropDownList ID="ddOperator" runat="server" DataSourceID="SqlDataSource10"
                                        DataTextField="EName" DataValueField="EmployeeInfoID" CssClass="form-control select2me">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource10" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [EmployeeInfoID], [EName] FROM [EmployeeInfo] WHERE ([SectionID] = '3') ORDER BY [EName]"></asp:SqlDataSource>
                                </div>


                                <div class="form-group hidden">
                                    <label>Purpose :</label>
                                    <asp:DropDownList ID="ddPurpose" runat="server" DataSourceID="SqlDataSource1" CssClass="form-control select2me"
                                        DataTextField="Purpose" DataValueField="pid" AutoPostBack="True" OnSelectedIndexChanged="ddPurpose_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [pid], [Purpose] FROM [Purpose] order by Purpose">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="form-group">
                                    <label>For Company:</label>
                                    <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource8" CssClass="form-control select2me"
                                        DataTextField="Company" DataValueField="PartyID" AutoPostBack="True" OnSelectedIndexChanged="ddCustomer_OnSelectedIndexChanged" AppendDataBoundItems="True">
                                        <asp:ListItem Value="">--- all ---</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>

                                <div class="form-group">
                                    <label>Brand Name: </label>
                                    <asp:DropDownList ID="ddBrand" runat="server" DataSourceID="SqlDataSource5" DataTextField="BrandName" DataValueField="BrandID" AppendDataBoundItems="True"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddBrand_OnSelectedIndexChanged" CssClass="form-control select2me">
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

                                <div class="form-group">
                                    <label>Pack Size:</label>
                                    <asp:DropDownList ID="ddSize" runat="server" DataSourceID="SqlDataSource6" DataTextField="BrandName" DataValueField="BrandID"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddBrand_OnSelectedIndexChanged" CssClass="form-control select2me">
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

                                <div class="form-group">
                                    <label>Machine No</label>
                                    <asp:DropDownList ID="ddMachine" runat="server" DataSourceID="SqlDataSource2" CssClass="form-control select2me"
                                        DataTextField="MachineNo" DataValueField="mid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT CONCAT(MachineNo,'  ',Description) AS MachineNo, mid FROM Machines WHERE Section=3 ORDER BY MachineNo"></asp:SqlDataSource>
                                </div>

                                <div class="form-group">
                                    <label>Line# : </label>
                                    <asp:DropDownList ID="ddLine" runat="server" DataSourceID="SqlDataSource4" CssClass="form-control select2me"
                                        DataTextField="DepartmentName" DataValueField="Departmentid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Lines] where Section=3 ORDER BY [DepartmentName]"></asp:SqlDataSource>
                                </div>

                                <div class="form-group">
                                    <label>Shift:</label>
                                    <asp:DropDownList ID="ddShift" runat="server" DataSourceID="SqlDataSource11" CssClass="form-control select2me"
                                        DataTextField="DepartmentName" DataValueField="Departmentid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource11" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Shifts] where Section='3' ORDER BY [DepartmentName]"></asp:SqlDataSource>
                                </div>


                                <div class="form-group">
                                    <label>Item Sub-group : </label>
                                    <asp:DropDownList ID="ddOutSubGroup" runat="server" CssClass="form-control select2me">
                                    </asp:DropDownList>

                                </div>

                                <div class="form-group">
                                    <label class="control-label">Item Grade :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddOutGrade" runat="server" AutoPostBack="true" CssClass="form-control select2me"
                                            OnSelectedIndexChanged="ddOutGrade_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="control-label">Item Category :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddOutCategory" runat="server" AutoPostBack="true" CssClass="form-control select2me"
                                            OnSelectedIndexChanged="ddOutCategory_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="control-label">Processed Item :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddOutItem" runat="server" AutoPostBack="true" CssClass="form-control select2me"
                                            OnSelectedIndexChanged="ddOutItem_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Godown : </label>
                                    <asp:DropDownList ID="ddGodown" runat="server" DataSourceID="SqlDataSource19"
                                        DataTextField="StoreName" DataValueField="WID" AutoPostBack="True" OnSelectedIndexChanged="ddGodown_OnSelectedIndexChanged" CssClass="form-control" Enabled="False">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource19" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [WID], [StoreName] FROM Warehouses WHERE WID='4' "></asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Location : </label>
                                    <asp:DropDownList ID="ddLocation" runat="server" DataSourceID="SqlDataSource21"
                                        DataTextField="AreaName" DataValueField="AreaID" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource21" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT AreaID,[AreaName] FROM [WareHouseAreas] WHERE Warehouse=@Warehouse">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddGodown" Name="Warehouse" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="form-group">
                                    <label>Color :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddColor" runat="server" DataSourceID="SqlDataSource9" DataTextField="DepartmentName" DataValueField="Departmentid"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddBrand_OnSelectedIndexChanged" CssClass="form-control select2me">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource9" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [DepartmentName], [Departmentid] FROM [Colors] ORDER BY [DepartmentName]"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label>For Company:</label>
                                    <asp:DropDownList ID="ddCompany2" runat="server" DataSourceID="SqlDataSource13" CssClass="form-control select2me"
                                        DataTextField="Company" DataValueField="PartyID" AutoPostBack="True" OnSelectedIndexChanged="DropDownList13_OnSelectedIndexChanged" AppendDataBoundItems="True">
                                        <asp:ListItem Value="">--- all ---</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource13" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>

                                <div class="form-group">
                                    <label>Brand Name: </label>
                                    <asp:DropDownList ID="ddBrand2" runat="server" DataSourceID="SqlDataSource14" CssClass="form-control select2me"
                                        DataTextField="BrandName" DataValueField="BrandID" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddBrand_OnSelectedIndexChanged">
                                        <asp:ListItem Value="">--- all ---</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource14" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], [BrandName] FROM [CustomerBrands] WHERE (([CustomerID] = @CustomerID) AND ([ProjectID] = @ProjectID)) Order by BrandName">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddCompany2" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>

                                <div class="form-group">
                                    <label>Pack Size: </label>
                                    <asp:DropDownList ID="ddInputPack" runat="server" DataSourceID="SqlDataSource6" DataTextField="BrandName" DataValueField="BrandID"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddBrand_OnSelectedIndexChanged" CssClass="form-control select2me">
                                    </asp:DropDownList>
                                    <span style="width: 70%; color: green; float: right">
                                        <asp:Literal ID="ltrLastInfo" runat="server">Available  Stock: Qnty(Pcs).... Qnty(Kg).... </asp:Literal>
                                    </span>

                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label22" runat="server" Text="Weight/Pc. (gm.) : "></asp:Label>
                                    <asp:TextBox ID="txtWeightPc" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtWeightPc">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label19" runat="server" Text="Input Qty.(PCS) : "></asp:Label>
                                    <asp:TextBox ID="txtInputQtyPCS" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtInputQtyPCS">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label1" runat="server" Text="Input Weight (kg.) : "></asp:Label>
                                    <asp:TextBox ID="txtInputQtyKG" runat="server"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label11" runat="server" Text="Production (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtproduced" runat="server" onkeyup="calInpTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtproduced">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label15" runat="server" Text="Rejection (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtRejected" runat="server" onkeyup="calInpTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtRejected">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label3" runat="server" Text="Net Production (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtNetPrdn" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtNetPrdn">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="form-group">
                                    <label>Reusable Waste Item  :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddReusableUsed" runat="server" DataSourceID="SqlDataSource12r" DataTextField="ItemName" DataValueField="ProductID" CssClass="form-control select2me">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource12r" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT        ProductID, ItemName FROM            Products WHERE        (CategoryID IN (SELECT        CategoryID FROM            Categories WHERE        (GradeID IN (SELECT        GradeID FROM            ItemGrade WHERE        (CategoryID = 29)))))"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label13x" runat="server" Text="Reusable Waste (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtReusableWasteQty" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtReusableWasteQty">
                                    </asp:FilteredTextBoxExtender>
                                </div>



                                <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" CssClass="right block" />

                                <div style="clear: both"></div>
                                <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>

                                <div style="clear: both"></div>

                                <div class="table-responsive">
                                    <asp:Label ID="lblEid" runat="server" Text="" Visible="False"></asp:Label>

                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridView2_OnRowDataBound"
                                        DataSourceID="SqlDataSource7" Width="250%" OnSelectedIndexChanged="GridView2_OnSelectedIndexChanged" OnRowDeleting="GridView2_OnRowDeleting">
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

                                            <asp:TemplateField HeaderText="pid" InsertVisible="False" SortExpression="pid" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Subgroup" HeaderText="Subgroup" SortExpression="Subgroup" />
                                            <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" />
                                            <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                                            <asp:BoundField DataField="Item" HeaderText="Item" SortExpression="Item" />
                                            <asp:BoundField DataField="Color" HeaderText="Color" SortExpression="Color" />
                                            <asp:BoundField DataField="WeightPerPc" HeaderText="WeightPerPc" SortExpression="WeightPerPc" />
                                            <asp:BoundField DataField="InputQty" HeaderText="InputQty" SortExpression="InputQty" />
                                            <asp:BoundField DataField="InputWeight" HeaderText="InputWeight" SortExpression="InputWeight" />


                                            <asp:BoundField DataField="Production" HeaderText="Production" SortExpression="Production" />
                                            <asp:BoundField DataField="Rejected" HeaderText="Rejected Pcs" SortExpression="Rejected" />
                                            <asp:BoundField DataField="NetProduction" HeaderText="NetProduction" SortExpression="NetProduction" />
                                            <asp:BoundField DataField="ReusableWasteQty" HeaderText="Reusable Waste Pcs" SortExpression="NetProduction" />
                                            <asp:BoundField DataField="NonusableWaste" HeaderText="Nonusable Waste (kg)" SortExpression="NetProduction" />


                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT id, 
                                        (Select CategoryName from ItemSubGroup where CategoryID=a.Subgroup) AS [Subgroup], 
                                        (SELECT [GradeName] FROM [ItemGrade] WHERE [GradeID] = a.Grade) AS [Grade],
                                        (SELECT [CategoryName] FROM [Categories] WHERE [CategoryID] = a.Category) AS  [Category], 
                                         (SELECT [ItemName] FROM [Products] WHERE [ProductID] = a.Item) AS  [Item],
                                          (SELECT [DepartmentName] FROM [Colors] WHERE [Departmentid] = a.Color) AS  [Color], [WeightPerPc], [InputQty], [InputWeight], [Production], [Rejected], [NetProduction],  ReusableWasteItem, ReusableWasteQty, NonusableWaste FROM [PrdnRoundTinDetails] a WHERE ([PrdnID] = '') ORDER BY [Id]"
                                        DeleteCommand="Delete from PrdnRoundTinDetails where id=0"></asp:SqlDataSource>

                                </div>

                                <asp:Label ID="lblTotalWeight" runat="server" Text=""></asp:Label>





                                <a name="OpDetails"></a>
                                <legend style="margin-bottom: 6px;">Finished Product Output</legend>




                                <div class="form-group">
                                    <label>Item Sub-group : </label>
                                    <asp:DropDownList ID="ddSubGrp" CssClass="form-control select2me" runat="server">
                                    </asp:DropDownList>

                                </div>

                                <div class="form-group">
                                    <label class="control-label">Item Grade :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddGradeRaw" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddGradeRaw_SelectedIndexChanged" CssClass="form-control select2me">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="control-label">Item Category :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddCategoryRaw" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddCategoryRaw_SelectedIndexChanged" CssClass="form-control select2me">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="control-label">Item Name :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddItemNameRaw" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddItemNameRaw_SelectedIndexChanged" CssClass="form-control select2me">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>
                                        <%--<span style="width: 70%; color: green; float: right">
<asp:Literal ID="ltrLastInfo" runat="server">Available  Stock: Qnty(Pcs).... Qnty(Kg).... </asp:Literal>                                            
                                        </span>--%>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label10" runat="server" Text="Final Output Qty (Pcs) : "></asp:Label>
                                    <asp:TextBox ID="txtFinalQty" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtFinalQty">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label12" runat="server" Text="Weight Per Pc (gm) : "></asp:Label>
                                    <asp:TextBox ID="txtWeightPerPc" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtFinalQty">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label14" runat="server" Text="Total Output Weight (kg) : "></asp:Label>
                                    <asp:TextBox ID="txtFinalProd" runat="server" onkeyup="calInvTotal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtFinalQty">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label5" runat="server" Text="Working hr. : "></asp:Label>
                                    <asp:TextBox ID="txtHour" runat="server"></asp:TextBox>
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
                                    <asp:Label ID="Label16" runat="server" Text="Reason of Time Waste : "></asp:Label>
                                    <asp:TextBox ID="txtReason" runat="server"></asp:TextBox>
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

                                    <asp:GridView ID="GridView1" runat="server" Width="450%" AllowSorting="True"
                                        AutoGenerateColumns="False" BackColor="White" Borderpayor="#999999"
                                        BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black"
                                        GridLines="Vertical" DataSourceID="SqlDataSource3" AllowPaging="True" PageSize="15"
                                        OnRowDeleting="GridView1_OnRowDeleting">
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
                                            <asp:BoundField DataField="Shift" HeaderText="Shift" SortExpression="Shift" />

                                            <asp:BoundField DataField="Operator" HeaderText="Operator" SortExpression="Operator" />
                                            <asp:BoundField DataField="Purpose" HeaderText="Purpose" SortExpression="Purpose" />

                                            <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" SortExpression="CustomerID" />
                                            <asp:BoundField DataField="Brand" HeaderText="Brand" SortExpression="Brand" />
                                            <asp:BoundField DataField="PackSize" HeaderText="PackSize" SortExpression="PackSize" />
                                            <asp:BoundField DataField="ItemSubGroup" HeaderText="ItemSubGroup" SortExpression="ItemSubGroup" />
                                            <asp:BoundField DataField="ItemGrade" HeaderText="ItemGrade" SortExpression="ItemGrade" />
                                            <asp:BoundField DataField="ItemCategory" HeaderText="ItemCategory" SortExpression="ItemCategory" />
                                            <asp:BoundField DataField="ItemName" HeaderText="ItemName" SortExpression="ItemName" />
                                            <asp:BoundField DataField="FinalOutputQty" HeaderText="FinalOutputQty" SortExpression="FinalOutputQty" />
                                            <asp:BoundField DataField="WeightPerPc" HeaderText="WeightPerPc" SortExpression="WeightPerPc" />
                                            <asp:BoundField DataField="OutputWeight" HeaderText="OutputWeight" SortExpression="OutputWeight" />
                                            <asp:BoundField DataField="WorkingHour" HeaderText="WorkingHour" SortExpression="WorkingHour" />
                                            <asp:BoundField DataField="TimeWaste" HeaderText="TimeWaste" SortExpression="TimeWaste" />
                                            <asp:BoundField DataField="ReasonWaist" HeaderText="ReasonWaist" SortExpression="ReasonWaist" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Date], ProductionID, pid,
                                        
                                        (Select MachineNo from Machines where mid=a.MachineNo) AS [MachineNo], 
                                    (Select DepartmentName from Lines where Departmentid=a.LineNumber) AS [LineNumber],
                                       (Select DepartmentName from Shifts where Departmentid=a.Shift) AS   [Shift],  
                                        (Select EName from EmployeeInfo where EmployeeInfoID=a.Operator) AS  [Operator], 
                                       (Select Purpose from Purpose where pid=a.Purpose) AS  [Purpose], 
                                       (Select Company from Party where PartyID=a.CustomerID) AS  [CustomerID], 
                                       (Select BrandName from CustomerBrands where BrandID=a.Brand) AS  [Brand], 
                                       (Select BrandName from Brands where BrandID=a.PackSize) AS  [PackSize], 
                                        (Select CategoryName from ItemSubGroup where CategoryID=a.ItemSubGroup) AS [ItemSubGroup], 
                                        (SELECT [GradeName] FROM [ItemGrade] WHERE [GradeID] = a.ItemGrade) AS [ItemGrade],
                                        (SELECT [CategoryName] FROM [Categories] WHERE [CategoryID] = a.ItemCategory) AS  [ItemCategory], 
                                         
                                       (Select ItemName from Products where ProductID=a.ItemName) AS  [ItemName], 
                                        
                                        [FinalOutputQty], [WeightPerPc], [OutputWeight], [WorkingHour], [TimeWaste], [ReasonWaist], [Remarks] FROM [PrdnRoundTin] a ORDER BY [Date] desc"
                                        DeleteCommand="Delete PrdnShearing where pid=0"></asp:SqlDataSource>

                                </div>

                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
