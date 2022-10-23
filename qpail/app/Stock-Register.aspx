<%@ Page Title="Stock Register" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Stock-Register.aspx.cs" Inherits="app_Stock_Register" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">
    
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>



    <div class="row">
        <div class="col-md-12 ">
            <!-- BEGIN SAMPLE FORM PORTLET-->
            <div class="portlet box blue">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i> Stock Register
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


                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectedRowStyle-BackColor="LightBlue"
                             DataSourceID="SqlDataSource12">

                            <Columns>
                                  
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>. 
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" SortExpression="EntryDate"></asp:BoundField>

                                <asp:BoundField DataField="EntryType" HeaderText="EntryType" SortExpression="EntryType" />
                                <%--<asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID" SortExpression="InvoiceID" />--%>
                                <asp:BoundField DataField="Purpose" HeaderText="Purpose" SortExpression="Purpose" />
                                <asp:BoundField DataField="RefNo" HeaderText="RefNo" SortExpression="RefNo" />
                                <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" />

                                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"></asp:BoundField>

                                <asp:BoundField DataField="BrandID" HeaderText="Brand" SortExpression="BrandID" />
                                <asp:BoundField DataField="SizeID" HeaderText="Size" SortExpression="SizeID" />
                                <asp:BoundField DataField="Color" HeaderText="Color" SortExpression="Color" />
                                <asp:BoundField DataField="Spec" HeaderText="Spec" SortExpression="Spec" />

                                <asp:BoundField DataField="InQuantity" HeaderText="In Qty" SortExpression="InQuantity" />
                                <asp:BoundField DataField="OutQuantity" HeaderText="Out Qty" SortExpression="OutQuantity" />
                                <asp:BoundField DataField="InWeight" HeaderText="In Weight" SortExpression="InWeight" />
                                <asp:BoundField DataField="OutWeight" HeaderText="Out Weight" SortExpression="OutWeight" />
                                <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                            </Columns>

                            <SelectedRowStyle BackColor="LightBlue" />

                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand="SELECT TOP(100) [EntryDate], [EntryType], 
                            (Select Purpose from Purpose where pid=stock.Purpose) AS Purpose,
                             [InvoiceID], [RefNo], [ProductName], 
                                    (SELECT  [Company] FROM [Party] WHERE [PartyID]= Stock.Customer) AS Customer,
(SELECT [BrandName] FROM [CustomerBrands] WHERE BrandID=Stock.BrandID) AS BrandID,
(SELECT [BrandName] FROM [Brands] WHERE BrandID=Stock.SizeId) AS SizeId,
(SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=Stock.Color) AS Color, 
                            (SELECT [spec] FROM [Specifications] WHERE  CAST(id AS nvarchar)=Stock.Spec) AS Spec, 
                                    [InQuantity], [OutQuantity], [InWeight], [OutWeight], Remark FROM [Stock] 
                                   ORDER BY  [EntryID] DESC">
                            
                        </asp:SqlDataSource>
                    </div>


                </div>
            </div>
        </div>
    </div>




    </ContentTemplate>
    </asp:UpdatePanel>




</asp:Content>



<asp:Content ID="Content5" ContentPlaceHolderID="Foot" Runat="Server">
</asp:Content>

