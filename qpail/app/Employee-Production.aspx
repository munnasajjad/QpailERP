<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Employee-Production.aspx.cs" Inherits="app_Employee_Production" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h3 class="page-title">Production Entry</h3>
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
                                <div class="control-group">
                                    <asp:Label ID="Label4" runat="server" Text="Date :"></asp:Label>
                                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                                        Enabled="True" TargetControlID="txtDate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </div>
                                
                                <div class="control-group">
                                    <asp:Label ID="Label1" runat="server" Text="Section: "></asp:Label>
                                    <asp:DropDownList ID="ddSection" runat="server" DataSourceID="SqlDataSource1"
                                        DataTextField="SName" DataValueField="SID" AutoPostBack="true" OnSelectedIndexChanged="ddSection_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [SID], [SName] FROM [Sections] WHERE ([DepartmentID] = @DepartmentID) ORDER BY [SName]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="5" Name="DepartmentID" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label6" runat="server" Text="Machine No.: "></asp:Label>
                                    <asp:DropDownList ID="ddMachine" runat="server" DataSourceID="SqlDataSource2"
                                        DataTextField="DepartmentName" DataValueField="Departmentid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [DepartmentName], [Departmentid] FROM [Machines] ORDER BY [DepartmentName]">
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label7" runat="server" Text="Line#/ Name: "></asp:Label>
                                    <asp:DropDownList ID="ddLine" runat="server" DataSourceID="SqlDataSource4"
                                        DataTextField="DepartmentName" DataValueField="Departmentid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Lines] ORDER BY [DepartmentName]">
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

                                <div class="control-group hidden">
                                    <asp:Label ID="Label9" runat="server" Text="Purpose :"></asp:Label>
                                    <asp:TextBox ID="txtPurpose" runat="server"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label10" runat="server" Text="Component/Item: "></asp:Label>
                                    <asp:DropDownList ID="ddProduct" runat="server" DataSourceID="SqlDataSource5"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddProduct_SelectedIndexChanged"
                                        DataTextField="ItemName" DataValueField="ProductID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE ([CategoryID] IN (Select CategoryID from Categories where GradeID IN (Select GradeID from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=3 AND ProjectID=1)))) ORDER BY [ItemName]"></asp:SqlDataSource>

                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label11" runat="server" Text="Color: "></asp:Label>
                                    <asp:DropDownList ID="ddColor" runat="server" DataSourceID="SqlDataSource7"
                                        DataTextField="DepartmentName" DataValueField="Departmentid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Colors] ORDER BY [DepartmentName]">
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label3" runat="server" Text="Process: "></asp:Label>
                                    <asp:DropDownList ID="ddProcess" runat="server">
                                        <asp:ListItem>Manual</asp:ListItem>
                                        <asp:ListItem>Auto</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="lblDeptName" runat="server" Text="For Company/Dept: "></asp:Label>
                                    <asp:TextBox ID="txtFor" runat="server"></asp:TextBox>

                                    <%--<asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource1x"
                                        DataTextField="Company" DataValueField="PartyID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1x" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>--%>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label12" runat="server" Text="Operation: "></asp:Label>
                                    <asp:DropDownList ID="ddOperation" runat="server" DataSourceID="SqlDataSource9"
                                        DataTextField="DepartmentName" DataValueField="Departmentid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource9" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Satges] ORDER BY [DepartmentName]">
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label13" runat="server" Text="Operator: "></asp:Label>
                                    <asp:DropDownList ID="ddOperator" runat="server" DataSourceID="SqlDataSource10"
                                        DataTextField="EName" DataValueField="EmployeeInfoID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource10" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [EmployeeInfoID], [EName] FROM [EmployeeInfo] WHERE ([SectionID] = @SectionID) ORDER BY [EName]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddSection" Name="SectionID" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="Label5" runat="server" Text="Hour : "></asp:Label>
                                    <asp:TextBox ID="txtHour" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtHour">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label14" runat="server" Text="Production(PCS) : "></asp:Label>
                                    <asp:TextBox ID="txtproduced" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtproduced">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label15" runat="server" Text="Rejection(PCS) : "></asp:Label>
                                    <asp:TextBox ID="txtRejected" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtRejected">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                                
                                <div class="control-group">
                                    <asp:Label ID="Label8" runat="server" Text="Time Waist(Min) : "></asp:Label>
                                    <asp:TextBox ID="txtTimeWaist" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtTimeWaist">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label16" runat="server" Text="Reason of Time Waist : "></asp:Label>
                                    <asp:TextBox ID="txtReason" runat="server"></asp:TextBox>

                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label17" runat="server" Text="Shift: "></asp:Label>
                                    <asp:DropDownList ID="ddShift" runat="server" DataSourceID="SqlDataSource11"
                                        DataTextField="DepartmentName" DataValueField="Departmentid">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource11" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Departmentid], [DepartmentName] FROM [Shifts] ORDER BY [DepartmentName]">
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label18" runat="server" Text="Final Production(PCS) : "></asp:Label>
                                    <asp:TextBox ID="txtFinalProd" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtFinalProd">
                                    </asp:FilteredTextBoxExtender>
                                </div>


                                <div class="control-group">
                                    <label class="control-label">Remarks : </label>
                                    <asp:TextBox ID="txtRemark" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>

                                <div class="form-actions">
                                    <asp:Button ID="btnSave" runat="server" Text="Save"
                                        OnClick="btnSave_Click" />
                                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
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

                                <asp:GridView ID="GridView1" runat="server" Width="100%" AllowSorting="True"
                                    AutoGenerateColumns="False" BackColor="White" Borderpayor="#999999"
                                    BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black"
                                    GridLines="Vertical" DataSourceID="SqlDataSource3">
                                    <Columns>
                                        <asp:BoundField DataField="Date" HeaderText="Date"
                                            SortExpression="Date" />
                                        <asp:BoundField DataField="SectionName" HeaderText="SectionName"
                                            SortExpression="SectionName" />
                                        <asp:BoundField DataField="MachineNo" HeaderText="MachineNo"
                                            SortExpression="MachineNo" />
                                        <asp:BoundField DataField="LineNumber" HeaderText="LineNumber"
                                            SortExpression="LineNumber" />
                                        <asp:BoundField DataField="ItemName" HeaderText="ItemName" SortExpression="ItemName" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" SortExpression="CustomerName" />
                                        <asp:BoundField DataField="Hour" HeaderText="Hour" SortExpression="Hour" />
                                        <asp:BoundField DataField="FinalProduction" HeaderText="FinalProduction" SortExpression="FinalProduction" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [Date], [SectionName], [MachineNo], [LineNumber], [ItemName], [CustomerName], [Hour], [FinalProduction] FROM [Production]"></asp:SqlDataSource>

                                </fieldset>



                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
