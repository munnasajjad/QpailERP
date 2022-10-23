<%@ Page Title="Ink-Specifications" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Ink-Specifications.aspx.cs" Inherits="app_Ink_Specifications" %>

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

            
            <div class="row">
                <div class="col-md-12 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Ink Specifications
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
                                     DataSourceID="SqlDataSource12" DataKeyNames="id">

                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>.
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="id" HeaderText="id" SortExpression="id" InsertVisible="False" ReadOnly="True" Visible="False">
                                        </asp:BoundField>

                                        <asp:BoundField DataField="spec" HeaderText="Specification" SortExpression="spec" />
                                        <asp:BoundField  DataField="EntryBy" HeaderText="EntryBy" SortExpression="EntryBy" ReadOnly="True" Visible="False"/>
                                        <asp:BoundField DataField="EntryDate" HeaderText="EntryDate" SortExpression="EntryDate" ReadOnly="True" Visible="False"/>
                                        
                                        <asp:CommandField ButtonType="Image"  ShowEditButton="True" EditImageUrl="~/app/images/edit.png"
                                            CancelImageUrl="~/app/images/back32.png" UpdateImageUrl="~/app/images/approve.gif" />

                                    </Columns>

                                    <SelectedRowStyle BackColor="LightBlue" />

                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                    SelectCommand="SELECT [id], [spec], [EntryBy], [EntryDate] FROM [Specifications] ORDER BY [spec]" 
                                    DeleteCommand="DELETE FROM [Specifications] WHERE [id] = @original_id " 
                                    UpdateCommand="UPDATE [Specifications] SET [spec] = @spec WHERE [id] = @id">
                                    
                                    <DeleteParameters>
                                        <asp:Parameter Name="original_id" Type="Int32" />
                                    </DeleteParameters>
                                   
                                    <UpdateParameters>
                                        <asp:Parameter Name="id" Type="Int32" />
                                        <asp:Parameter Name="spec" Type="String" />
                                    </UpdateParameters>
                                    
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

