<%@ Page  Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Particulars.aspx.cs" Inherits="Application_Particulars" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            
            

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <h3 class="page-title">Particulars for vouchers entry
                     <%--<small>form components and widgets</small>--%>
                </h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <!-- BEGIN SAMPLE FORM PORTLET-->
                <div class="portlet box blue">
                    
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false" ></asp:Label>

                    <div class="portlet-body form">
                        <div class="form-horizontal">


                            <div id="EditField" runat="server" class="hidden">
                                <div class="control-group">
                                    <label class="field_title">Edit Currency</label>
                                    <div class="form_input">

                                        <asp:DropDownList name="" ID="DropDownList1" runat="server" AutoPostBack="true"
                                            DataSourceID="SqlDataSource2" DataTextField="Particularsname" DataValueField="Particularsid"
                                            OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT Particularsid, Particularsname FROM [Particulars] order by Particularsname"></asp:SqlDataSource>
                                    </div>
                                </div>
                            </div>


                            <div class="control-group">
                                <label class="control-label">Particular Name :</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtDiag" runat="server" title="" CssClass="span6 m-wrap"></asp:TextBox>
                                </div>
                            </div>

                            <div class="control-group ">
                                <label class="control-label">Particular Detail : </label>
                                <div class="controls">
                                    <asp:TextBox ID="txtDetails" runat="server" ></asp:TextBox>

                                </div>
                            </div>


                            <div class="form-actions">
                                <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click" />
                                
                                <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" />
                            </div>

                        </div>

                    </div>

                </div>

            </div>

            <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="Literal1" runat="server" Text="Saved Data" />
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                SelectCommand="SELECT [Particularsid],Detail, [Particularsname] FROM [Particulars]"></asp:SqlDataSource>


                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView2_SelectedIndexChanged"
                                DataKeyNames="Particularsid" DataSourceID="SqlDataSource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="CrID" SortExpression="CrID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Particularsid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="SrNo" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="Particularsname" HeaderText="Particular Name"
                                        SortExpression="Particularsname" />
                                    <asp:BoundField DataField="Detail" HeaderText="Particular Details"
                                        InsertVisible="False" ReadOnly="True" SortExpression="Particularsid" />



                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" />

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>

                        </div>
                    </div>
                </div>
            </div>
        </div>
   
            
                    </ContentTemplate>
                    </asp:UpdatePanel>

</asp:Content>

