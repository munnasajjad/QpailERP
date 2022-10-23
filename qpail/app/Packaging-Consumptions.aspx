<%@ Page Title="Item Consumptions" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Packaging-Consumptions.aspx.cs" Inherits="app_Packaging_Consumptions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>


            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">Item Consumptions <small></small>
                    </h3>
                </div>
            </div>
            <div class="row">

                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Pre-assumptions of finished items consumptions
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <%--<form role="form">--%>
                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                                <div class="form-group">
                                    <label>Finished Product : </label>
                                    <asp:DropDownList ID="ddFinished" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource2"
                                        DataTextField="ItemName" DataValueField="ProductID"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddFinished_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE ([CategoryID] IN (Select CategoryID from Categories where GradeID IN (Select GradeID from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=2 AND ProjectID=1)))) ORDER BY [ItemName]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="Int32" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="form-group">
                                    <label>Processed Item : </label>
                                    <asp:DropDownList ID="ddRawItem" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource3"
                                        DataTextField="ItemName" DataValueField="ProductID"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddRawItem_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE ([CategoryID] IN (Select CategoryID from Categories where GradeID IN (Select GradeID from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=3 AND ProjectID=1)))) ORDER BY [ItemName]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="Int32" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>


                                <div class="form-group">
                                    <label>Required Qty. (<asp:Literal ID="ltrUnit" runat="server"></asp:Literal>) : </label>
                                    <asp:TextBox ID="txtDept" runat="server" CssClass="form-control" EnableViewState="true" placeholder="ie, 0.001" />
                                </div>

                                <div class="form-group">
                                    <label>Wastage (%): </label>
                                    <asp:TextBox ID="txtWastage" runat="server" CssClass="form-control" EnableViewState="true" placeholder="type 5 for 5% wastage" />
                                </div>

                                <div class="form-actions">
                                    <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Add" OnClick="btnSave_Click1" />
                                    <asp:Button ID="btnClear" CssClass="btn default" runat="server" Text="Cancel" OnClick="btnClear_Click1" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>


                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="lrlSavedBox" runat="server"></asp:Literal>
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


                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                    AutoGenerateDeleteButton="True" OnRowDeleting="GridView1_RowDeleting"
                                    DataSourceID="SqlDataSource1" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="I.Code" SortExpression="RawItemCode" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("IID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RawItemName" HeaderText="Raw Item Name" SortExpression="RawItemName" />
                                        <asp:BoundField DataField="RequiredQuantity" HeaderText="Req. Qty." SortExpression="RequiredQuantity" />
                                        <asp:BoundField DataField="UnitType" HeaderText="Unit Type" SortExpression="UnitType" />
                                        <asp:BoundField DataField="Wastage" HeaderText="Wastage (%)" SortExpression="Wastage" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT IID, [RawItemCode], [RawItemName], [RequiredQuantity], Wastage, [UnitType], ProjectCode FROM [Ingradiants] WHERE (([ItemCode] = @ItemCode) AND ([ProjectCode] = @ProjectCode))"
                                    DeleteCommand="Delete Ingradiants where IID=@IID">
                                    <DeleteParameters>
                                        <asp:Parameter Name="IID" />
                                    </DeleteParameters>
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddFinished" Name="ItemCode" PropertyName="SelectedValue" Type="String" />
                                        <asp:ControlParameter ControlID="lblProject" Name="ProjectCode" PropertyName="Text" Type="Int32" />
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


