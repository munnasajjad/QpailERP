<%@ Page Title="List Of Employees" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Employee-List-Report.aspx.cs" Inherits="Operator_Employee_List_Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <!-- BEGIN PAGE LEVEL STYLES -->
    <link rel="stylesheet" type="text/css" href="assets/plugins/bootstrap-datepicker/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="assets/plugins/select2/select2.css" />
    <link rel="stylesheet" type="text/css" href="assets/plugins/select2/select2-metronic.css" />
    <link rel="stylesheet" href="assets/plugins/data-tables/DT_bootstrap.css" />

    <!-- END PAGE LEVEL STYLES -->
    <style type="text/css">
        label {
            padding-top: 6px;
            text-align: right;
            float: right;
            width: 100px;
        }

        input#ctl00_BodyContent_CheckBox1 {
            float: right;
        }

        .form-group label, .control-group label, .control-group span {
            width: 100px !important;
            padding: 9px 0;
        }

        input#ctl00_BodyContent_txtDateFrom, input#ctl00_BodyContent_txtDateTo {
            width: 150px;
        }

        span.chkbox {
            float: right;
        }

        .table7 {
            border: 3px solid #ccc;
            width: 50%;
            margin: 10px;
            font-weight: 700;
            color: #666;
            text-align: right;
            line-height: 18px;
        }

            .table7 td {
                padding: 5px;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">        </asp:ScriptManager>

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
	
			

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">
                <div class="col-md-12">
                    <!-- BEGIN EXAMPLE TABLE PORTLET -->
                    <div class="portlet box light-grey" style="min-height: 355px;">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-globe"></i>Sales by Items
                            </div>

                        </div>

                        <asp:Label ID="lblProject" runat="server" Text="1" Visible="false"></asp:Label>

                        <div class="portlet-body form">
                            <div class="form-body col-md-12">

<%--                                <div class="form-group col-md-4">
                                    <label class="control-label col-md-4">Company : </label>
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="ddCustomer" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource2"
                                            DataTextField="PartyName" DataValueField="CustomerID" AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT DISTINCT a.CustomerID, (Select Company from Party where PartyID=a.CustomerID) AS PartyName FROM Sales a ORDER BY [PartyName]">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>
                                
                                                
                                <div class="form-group col-md-4">
                                    <label class="control-label col-md-4">Item Grade : </label>
                                    <div class="col-md-8">
                                                        <asp:DropDownList ID="ddGrade" runat="server" DataSourceID="SqlDataSource5"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddGrade_OnSelectedIndexChanged"
                                                            DataTextField="GradeName" DataValueField="GradeID"  CssClass="form-control select2me" >
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT GradeID,GradeName from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=2 AND ProjectID=1) ORDER BY [GradeName]"></asp:SqlDataSource>
                                                    </div>
                                                </div>
                                             
                                <div class="form-group col-md-4">
                                    <label class="control-label col-md-4">Item Name : </label>
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="ddItem" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource6"
                                            DataTextField="ProductName" DataValueField="ProductName" AutoPostBack="true" OnSelectedIndexChanged="ddItem_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Text="Show All Items" AutoPostBack="true" OnCheckedChanged="CheckBox1_CheckedChanged" CssClass="chkbox" TextAlign="Right" />
                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT DISTINCT ProductName FROM SaleDetails WHERE 
                                            (ProductID IN (Select ProductID from Products where CategoryID IN (SELECT CategoryID from Categories where GradeID=@GradeID))) AND (InvNo IN (SELECT InvNo FROM Sales WHERE (CustomerID = @CustomerID))) ORDER BY ProductName">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                                <asp:ControlParameter ControlID="ddGrade" Name="GradeID" PropertyName="SelectedValue" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <label class="control-label col-md-3">Date Range: </label>
                                    <div class="col-md-8">
                                        <div class="input-group input-large date-picker input-daterange" data-date="10/11/2012" data-date-format="dd/mm/yyyy">
                                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" name="from" />
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy" />
                                            <span class="input-group-addon">to</span>
                                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" name="to" />
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtDateTo" Format="dd/MM/yyyy" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-2">

                                    

                                    <asp:Button ID="btnShow" runat="server" Text="Show History" CssClass="btn blue" OnClick="btnShow_Click" />
                                    <asp:Button ID="btnPrint" runat="server" Text="Print Ledger" onclick="btnPrint_Click" />
                                </div>--%>

                            </div>



                            <div class="form-body col-md-12 table-responsive">

                                <asp:GridView ID="GVrpt" class="xtable-b table-hover" aria-describedby="sample_1_info"
                                    runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="30"
                                    ShowFooter="True" AllowSorting="True" Width="220%" OnPageIndexChanging="GVrpt_OnPageIndexChanging"
                                    DataKeyNames="EmpSerial" RowStyle-CssClass="odd gradeX"  DataSourceID="SqlDataSource1">
                                    <Columns>
                                         <asp:TemplateField ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>. 
                                            </ItemTemplate>
                                             <ItemStyle Width="40px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Code" SortExpression="CV">
                                            <ItemTemplate>
                                                 <asp:HyperLink ID="Label1x" runat="server" NavigateUrl='<%# Bind("link") %>' Text='<%# Bind("EmpSerial") %>' Target="_blank"></asp:HyperLink>
                                             </ItemTemplate>
                                         </asp:TemplateField>

                                        <asp:BoundField DataField="DepartmentID" HeaderText="DepartmentID" SortExpression="DepartmentID" />
                                        <asp:BoundField DataField="SectionID" HeaderText="SectionID" SortExpression="SectionID" />
                                        <asp:BoundField DataField="Designation" HeaderText="Designation" SortExpression="Designation" />
                                        <asp:BoundField DataField="CardNo" HeaderText="CardNo" SortExpression="CardNo" ItemStyle-HorizontalAlign="Center"  >
                                         <ItemStyle HorizontalAlign="Center" />
                                         </asp:BoundField>
                                        <asp:BoundField DataField="EName" HeaderText="Name" SortExpression="EName" />
                                        <asp:BoundField DataField="FathersName" HeaderText="Fathers Name" SortExpression="FathersName" />
                                        <asp:BoundField DataField="MothersName" HeaderText="Mothers Name" SortExpression="MothersName" />
                                        <asp:BoundField DataField="NID" HeaderText="NID" SortExpression="NID" />
                                        <asp:BoundField DataField="DateOfBirth" HeaderText="DOB" SortExpression="DateOfBirth" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="ContactAddress" HeaderText="Contact Address" SortExpression="ContactAddress" />
                                        <asp:BoundField DataField="PermanentAddress" HeaderText="Permanent Address" SortExpression="PermanentAddress" />
                                        <asp:BoundField DataField="ContactNumber" HeaderText="Contact Number" SortExpression="ContactNumber" />
                                        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                                        <asp:BoundField DataField="JoiningDate" HeaderText="Joining Date" SortExpression="JoiningDate"  DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="Education" HeaderText="Education" SortExpression="Education" />
                                        <asp:BoundField DataField="Skills" HeaderText="Skills" SortExpression="Skills" />
                                        <asp:BoundField DataField="Salary" HeaderText="Salary" SortExpression="Salary" />
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                                         <asp:TemplateField HeaderText="Photo" SortExpression="CV">
                                            <ItemTemplate>
                                                 <asp:HyperLink ID="Label1m" runat="server" NavigateUrl='<%# Bind("Photo") %>' Text="View Photo" Target="_blank"></asp:HyperLink>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                         <asp:TemplateField HeaderText="CV" SortExpression="CV">
                                            <ItemTemplate>
                                                 <asp:HyperLink ID="Label1" runat="server" NavigateUrl='<%# Bind("CV") %>' Text="View CV" Target="_blank"></asp:HyperLink>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#f3f3f3" Font-Bold="True" ForeColor="#23A6F0" BorderStyle="Solid" />
                                    <RowStyle CssClass="odd gradeX" />
                                </asp:GridView>

                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT EmployeeInfoID, 'Employee-Info.aspx?id='+(CONVERT(varchar,EmployeeInfoID)) as link , [EmpSerial], [CardNo], [EName], [FathersName], [MothersName], 
                                   (Select DepartmentName from Departments WHERE  Departmentid=a.DepartmentID) as [DepartmentID], 
                                   (Select SName from Sections WHERE  SID=a.SectionID) as  [SectionID],
                                    (Select DesignationName from Designations WHERE  Designationid=a.Designation) as  [Designation], 
                                    [NID], [DateOfBirth], [ContactAddress], [PermanentAddress], [ContactNumber], [Email], [JoiningDate], [Education], [Skills], [Salary], 
                                    [Remark], (Select PhotoURL from Photos where PhotoID=a.cv) as [CV],  (Select PhotoURL from Photos where PhotoID=a.Photo) as [Photo] FROM [EmployeeInfo] a WHERE ([IsActive] = @IsActive) ORDER BY [DepartmentID], [SectionID], [EName]">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="1" Name="IsActive" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                            </div>


                        </div>
                        <!-- END EXAMPLE TABLE PORTLET-->
                    </div>
                </div>



            </div>

            <div class="grid_6 hidden">

                <table class="table7">
                    <tr>
                        <td>Grand Total Quantity (Pcs.): </td>
                        <td><asp:Literal ID="ltrQty" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Grand Item Total (Tk.): </td>
                        <td><asp:Literal ID="ltrItemLoad" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Grand Total VAT (Tk.): </td>
                        <td><asp:Literal ID="ltrTotalVat" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Grand Total Amount (Tk.): </td>
                        <td><asp:Literal ID="ltrGTAmt" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Grand Total Weight (kg.): </td>
                        <td><asp:Literal ID="ltrTotalWeight" runat="server" /></td>
                    </tr>
                </table>
            </div>



        </ContentTemplate>
    </asp:UpdatePanel>






    <%--<script type="text/javascript" src="assets/plugins/data-tables/jquery.dataTables.js"></script>
    <script type="text/javascript" src="assets/plugins/data-tables/DT_bootstrap.js"></script>
    <script src="assets/scripts/custom/table-managed.js"></script>
    <script>
        jQuery(document).ready(function () {
            TableManaged.init();
        });
    </script>--%>
</asp:Content>

