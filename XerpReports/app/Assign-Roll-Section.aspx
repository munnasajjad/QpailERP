<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="Assign-Roll-Section.aspx.cs" Inherits="Oxford.app.Assign_Roll_Section" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodycontent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <%--<asp:DropDownList name="" ID="DropDownList1" runat="server" 
                                            data-placeholder="--- Select ---" Style=" min-width: 150px"
                                             TabIndex="2">
                                            <asp:ListItem>Purchase</asp:ListItem>
                                            <asp:ListItem>Payment</asp:ListItem>
                                            <asp:ListItem>Adjustment</asp:ListItem>
                                        </asp:DropDownList>--%>

          
    
 <section class="col-lg-6"> 
 <section class="panel"> 
     
    <fieldset>
        <legend>Roll Number and Class Assign</legend>
        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            
            <tr id="EditField" runat="server" >
                <td>Edit Class info</td>
                <td>
                    <asp:DropDownList name="" ID="ddClass" runat="server"  AutoPostBack="true"
                        DataSourceID="SqlDataSource2" DataTextField="name" DataValueField="sl"  CssClass="form-control"
                        onselectedindexchanged="ddClass_SelectedIndexChanged" >
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                        SelectCommand="SELECT [sl], [Name] FROM [Class] order by sl">
                    </asp:SqlDataSource>
                </td>
            </tr>

                    </table>    

                        <div class="form_container left_label field_set">

                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="sl"
                                DataSourceID="SqlDataSource4" OnDataBound="GridView2_DataBound">

                                <Columns>
                                    <asp:TemplateField HeaderText="" SortExpression="TID" Visible="False">                                        
                                        <ItemTemplate>
                                            <asp:Label ID="lblTID" runat="server" Text='<%# Bind("sl") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:BoundField DataField="StudentID" HeaderText="Student ID" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" ReadOnly="True" SortExpression="TransactionType" >
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="StudentNameE" HeaderText="Student Name" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" ReadOnly="True" SortExpression="TransactionType" >
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FatherNameE" HeaderText="Father Name" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" ReadOnly="True" SortExpression="TransactionType" >
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Roll No." SortExpression="RollNumber" >
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRollNo" runat="server" Text='<%# Bind("RollNumber") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" HorizontalAlign="Right" />
                                    </asp:TemplateField>


                                    
                                    <asp:TemplateField HeaderText="Section" SortExpression="LinkAccountHeadID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSection" runat="server" Text='<%# Bind("Section") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="500px" HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Section" SortExpression="LinkAccountHeadName">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddSection" runat="server" Width="100%"
                                                DataSourceID="SqlDataSource01" DataTextField="name"
                                                DataValueField="sl">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource01" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                                SelectCommand="SELECT  sl, name FROM [Section] where Class=@Class ORDER BY [name]">
                                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddClass" Name="Class" PropertyName="SelectedValue" />
                                </SelectParameters>
                                            </asp:SqlDataSource>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UpdateDate" HeaderText="UpdateDate" SortExpression="UpdateDate" Visible="False" />
                                    <asp:BoundField DataField="UpdateBy" HeaderText="UpdateBy" SortExpression="UpdateBy" Visible="False" />
                                </Columns>
                            </asp:GridView>

                            <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                SelectCommand="SELECT sl, StudentID, StudentNameE, FatherNameE, RollNumber, Section FROM [Students] WHERE Class=@Class order by RollNumber">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddClass" Name="Class" PropertyName="SelectedValue" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            
                                    <div class="form_grid_12">
                                        <div class="form_input">

                                            <asp:Button ID="btnSave" runat="server" Text="Save"
                                                class="btn_small btn_blue" OnClick="btnSave_Click" />

                                        </div>
                                    </div>
                                </div>


        </fieldset>
                 

                </section>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
