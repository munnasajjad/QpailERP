<%@ Page Title="CategoryWise Stock" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="CategoryWiseStock.aspx.cs" Inherits="app_CategoryWiseStock" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <script type="text/javascript" src="https://cdn.datatables.net/1.10.10/js/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.10/css/jquery.dataTables.min.css">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".tbl_default").prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]] //value:item pair
            });
        });

        $(window).load(function () {
            //jScript();
        });

        function jScript() {
            $(".tbl_default").prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]] //value:item pair
            });
        }

    </script>


    <style type="text/css">
        .col-md-4 .control-group input, .col-md-4 .control-group select {
            width: 100%;
        }

        .col-md-4 .control-group label {
            padding-bottom: 4px;
        }

        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td {
            border: 1px solid #ddd;
            color: GrayText;
        }

        input#ctl00_BodyContent_chkMerge, height_fix {
            height: 17px !important;
            top: -3px !important;
            margin-top: -5px !important;
        }

        bottom_fix {
            margin-bottom: -15px !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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


        <div class="col-md-12 ">
            <!-- BEGIN SAMPLE FORM PORTLET-->
            <div class="portlet box green ">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Raw Items Specifications
                    </div>
                    <div class="tools">
                        <a href="" class="collapse"></a>
                        <a href="#portlet-config" data-toggle="modal" class="config"></a>
                        <a href="" class="reload"></a>
                        <a href="" class="remove"></a>
                    </div>
                </div>
                <div class="portlet-body form">
                    <div class="form-horizontal" role="form">
                        <div class="form-body">
                            <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>
                            <asp:LoginName ID="LoginName1" runat="server" Visible="false" />
                            <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>

                            <div class="col-md-12">
                                <div class="row">

                                    <asp:Panel ID="pnlAdd" runat="server" DefaultButton="btnSearch">
                                        <div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">Machine : </label>
                                                <asp:DropDownList ID="ddGodown" runat="server" DataSourceID="SqlDataSource6"
                                                    DataTextField="MachineNo" DataValueField="mid" CssClass="form-control" AppendDataBoundItems="True"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddGodown_SelectedIndexChanged">
                                                    <asp:ListItem>--- all ---</asp:ListItem>

                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource6" runat="server"
                                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT mid,MachineNo FROM  Machines"></asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label>Purpose :</label>
                                                <asp:DropDownList ID="ddPurpose" runat="server" DataSourceID="SqlDataSource13" AppendDataBoundItems="True"
                                                    DataTextField="Purpose" DataValueField="pid" AutoPostBack="True" OnSelectedIndexChanged="ddPurpose_OnSelectedIndexChanged">

                                                    <asp:ListItem>--- all ---</asp:ListItem>

                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource13" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [pid], [Purpose] FROM [Purpose] order by Purpose"></asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="col-md-4 hidden">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Group</label>
                                                <asp:DropDownList ID="ddGroup" runat="server" DataSourceID="SqlDataSource1" AutoPostBack="true"
                                                    DataTextField="GroupName" DataValueField="GroupSrNo">
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT GroupSrNo, [GroupName] FROM [ItemGroup] WHERE GroupSrNo<>2 ORDER BY [GroupSrNo]"></asp:SqlDataSource>

                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Subgroup </label>

                                                <asp:DropDownList ID="ddSubGroup" runat="server"  DataSourceID="SqlDataSource3" DataTextField="CategoryName" DataValueField="CategoryID"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddSubGroup_OnSelectedIndexChanged" AppendDataBoundItems="True">

                                                    <%--<asp:ListItem>--- all ---</asp:ListItem>--%>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT CategoryID, CategoryName FROM [ItemSubGroup] where (GroupID = @GroupID) ">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddGroup" Name="GroupID" PropertyName="SelectedValue" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>


                                        <div class="col-md-4">
                                            <div class="control-group hidden">
                                                <label class="col-sm-12 control-label">Grade : </label>
                                            <asp:DropDownList ID="ddGrade"  runat="server"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddGrade_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        </div>


                                        <div class="col-md-4">
                                            <div class="control-group hidden">
                                                <label class="col-sm-12 control-label">Category : </label>
                                            <asp:DropDownList ID="ddcategory" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddcategory_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        </div>

                                        <div class="col-md-4" id="tinPlate" runat="server">
                                            <div class="control-group">
                                                <label class="col-sm-12 control-label">Tin-Plate Type </label>

                                                <asp:DropDownList ID="ddType" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddSubGroup_OnSelectedIndexChanged" AppendDataBoundItems="True">
                                                    <asp:ListItem>Raw Sheet</asp:ListItem>
                                                    <asp:ListItem>Processed Sheet</asp:ListItem>
                                                    <asp:ListItem>Printed Sheet</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>


                                        <div class="col-md-4" runat="server">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">Date From: </label>
                                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                                    Enabled="True" TargetControlID="txtDateFrom">
                                                </asp:CalendarExtender>
                                            </div>
                                        </div>


                                        <div class="col-md-4" runat="server">
                                            <div class="control-group">
                                                <label class="control-label full-wdth">Stock  (Up To) Date : </label>
                                                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                    Enabled="True" TargetControlID="txtDate">
                                                </asp:CalendarExtender>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="control-group">
                                                <div style="margin-top: 29px">
                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" Width="100px" OnClick="btnAdd_Click" />
                                                    <asp:Button ID="btnExport" runat="server" Width="120px" Text="Export to Excel" OnClick="btnExport_OnClick" />
                                                    <%--<asp:Button ID="btnPrint" runat="server" Width="100px" Text="Print All" OnClick="btnPrint_OnClick" />--%>
                                                </div>
                                            </div>
                                        </div>

                                    </asp:Panel>

                                </div>
                            </div>


                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <iframe id="if1" runat="server" height="800px" width="100%" ></iframe>


    <div class="row">
        <div class="col-md-12 ">
            <!-- BEGIN SAMPLE FORM PORTLET-->
            <div class="portlet box blue">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Current Stock
                    </div>
                    <div class="tools">
                        <a href="" class="collapse"></a>
                        <a href="#portlet-config" data-toggle="modal" class="config"></a>
                        <a href="" class="reload"></a>
                        <a href="" class="remove"></a>
                    </div>
                </div>
                <div class="portlet-body form">

                    <div class="table-responsive">
                        <asp:Literal ID="ltrtotal" runat="server"></asp:Literal>


                        <asp:SqlDataSource ID="InkGridSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand="SELECT DISTINCT ProductID, (SELECT GradeName FROM ItemGrade WHERE (GradeID = (SELECT GradeID FROM Categories WHERE (CategoryID = (SELECT CategoryID FROM Products WHERE (ProductID = MachineStock.ProductID)))))) AS Grade, (SELECT CategoryName FROM Categories AS Categories_2 WHERE (CategoryID = (SELECT CategoryID FROM Products AS Products_3 WHERE (ProductID = MachineStock.ProductID)))) AS Category, (SELECT ItemName FROM Products AS Products_2 WHERE (ProductID = MachineStock.ProductID)) AS Product, (SELECT spec FROM Specifications WHERE (CAST(id AS nvarchar) = MachineStock.Spec)) AS Spec, ISNULL(SUM(InWeight) - SUM(OutWeight), 0) AS Weight FROM MachineStock WHERE (ProductID IN (SELECT ProductID FROM Products AS Products_1 WHERE (CategoryID IN (SELECT CategoryID FROM Categories AS Categories_1 WHERE (GradeID IN (SELECT GradeID FROM ItemGrade AS ItemGrade_1 WHERE (CategoryID = 10))))))) GROUP BY ProductID, Spec"></asp:SqlDataSource>



                        <asp:SqlDataSource ID="NonPrintedGridSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT DISTINCT ProductID, (SELECT GradeName FROM ItemGrade WHERE (GradeID = (SELECT GradeID FROM Categories WHERE (CategoryID = (SELECT CategoryID FROM Products WHERE (ProductID = Stock.ProductID)))))) AS Grade, (SELECT CategoryName FROM Categories AS Categories_2 WHERE (CategoryID = (SELECT CategoryID FROM Products AS Products_3 WHERE (ProductID = Stock.ProductID)))) AS Category, (SELECT ItemName FROM Products AS Products_2 WHERE (ProductID = Stock.ProductID)) AS Product, (SELECT Purpose FROM Purpose WHERE (CAST(pid AS nvarchar) = Stock.Purpose)) AS Purpose, ItemType, ISNULL(SUM(InQuantity) - SUM(OutQuantity), 0) AS Quantity, ISNULL(SUM(InWeight) - SUM(OutWeight), 0) AS Weight FROM Stock WHERE (ProductID IN (SELECT ProductID FROM Products AS Products_1 WHERE (CategoryID IN (SELECT CategoryID FROM Categories AS Categories_1 WHERE (GradeID IN (SELECT GradeID FROM ItemGrade AS ItemGrade_1 WHERE (CategoryID = @CategoryID) AND (ItemType = @ItemType))))))) GROUP BY ProductID, Purpose, ItemType">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddSubGroup" Name="CategoryID" PropertyName="SelectedValue" />
                                <asp:ControlParameter ControlID="ddType" Name="ItemType" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:SqlDataSource>




                       
                        <asp:SqlDataSource ID="PrintedGridSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand=" SELECT Distinct ProductID,
                                                    (SELECT GradeName FROM ItemGrade WHERE (GradeID = (Select GradeID from Categories where CategoryID=(SELECT CategoryID FROM Products WHERE (ProductID = MachineStock.ProductID)) ))) AS Grade,
                                                    (SELECT CategoryName FROM Categories WHERE (CategoryID =(SELECT CategoryID FROM Products WHERE (ProductID = MachineStock.ProductID)) )) AS Category,
                                                    (SELECT ItemName FROM Products WHERE (ProductID = MachineStock.ProductID))  AS Product,

                                                    (SELECT Company FROM Party WHERE  CAST(PartyID AS nvarchar)=MachineStock.Customer) AS Company,
                                                    (SELECT BrandName FROM CustomerBrands WHERE  CAST(BrandID AS nvarchar)=MachineStock.BrandID) AS Brand,
                                                    (SELECT BrandName FROM Brands WHERE  CAST(BrandID AS nvarchar)=MachineStock.SizeID) AS PackSize,
                                                    (SELECT DepartmentName FROM Colors WHERE  CAST(Departmentid AS nvarchar)=MachineStock.Color) AS Color,

                                                    (SELECT [Purpose] FROM [Purpose] WHERE  CAST(pid AS nvarchar)=MachineStock.Purpose) AS Purpose, ItemType,
                                                                                        ISNULL(sum(InQuantity)-Sum(OutQuantity),0) AS Quantity,
                                                                                        ISNULL(sum(InWeight)-Sum(OutWeight),0) AS Weight FROM [MachineStock]
									                                                    Where ProductID IN (SELECT ProductID FROM Products WHERE (CategoryID IN
                                                    (SELECT CategoryID FROM Categories WHERE (GradeID IN (SELECT GradeID FROM ItemGrade WHERE (ItemType = 'Printed Sheet'))))))
									                                                    Group By ProductID, Purpose, ItemType,Customer,BrandID,SizeID,Color"></asp:SqlDataSource>




                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="True" SelectedRowStyle-BackColor="LightBlue" Visible="False"
                            OnRowDataBound="ItemGrid_RowDataBound" CssClass="tbl_default zebra table">

                            <Columns>
                                
                                

                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>.
                                    </ItemTemplate>
                                    <ItemStyle Width="20px" />
                                </asp:TemplateField>
                              
                            </Columns>

                            <SelectedRowStyle BackColor="LightBlue" />

                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand="SELECT TOP(100) [EntryDate], [EntryType],
                            (Select Purpose from Purpose where pid=MachineStock.Purpose) AS Purpose,
                                    [InvoiceID], [RefNo], [ProductName],
                                    (SELECT  [Company] FROM [Party] WHERE [PartyID]= MachineStock.Customer) AS Customer,
                                    (SELECT [BrandName] FROM [CustomerBrands] WHERE BrandID=MachineStock.BrandID) AS BrandID,
                                    (SELECT [BrandName] FROM [Brands] WHERE BrandID=MachineStock.SizeId) AS SizeId,
                                    (SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=MachineStock.Color) AS Color,
                                    (SELECT [spec] FROM [Specifications] WHERE  CAST(id AS nvarchar)=MachineStock.Spec) AS Spec,
                                    [InQuantity], [OutQuantity], [InWeight], [OutWeight], Remark FROM [MachineStock] WHERE ([ProductID] = @ProductID)
                                   ORDER BY [EntryDate] DESC, [EntryID] DESC">
                            <SelectParameters>
                                <%--<asp:ControlParameter ControlID="ddProduct" Name="ProductID" PropertyName="SelectedValue" Type="String" />
                                        <asp:ControlParameter ControlID="ddType" Name="ItemType" PropertyName="SelectedValue" Type="String" />--%>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>

                </div>
            </div>
        </div>
    </div>




    </ContentTemplate>
    </asp:UpdatePanel>




</asp:Content>
