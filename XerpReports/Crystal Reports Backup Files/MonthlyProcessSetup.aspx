<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="MonthlyProcessSetup.aspx.cs" Inherits="Oxford.app.MonthlyProcessSetup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>


    
 <div class="col-lg-6"> 
 <section class="panel"> 
<%--Body Contants--%>
<div id="Div2">
    <div>

    <fieldset>
        <legend>Monthly Process Setup</legend>
        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                </td>
            </tr>

            <tr id="EditField" runat="server" class="hidden">
                <td>Edit Class info</td>
                <td>
                    <asp:DropDownList name="" ID="DropDownList1" runat="server"  AutoPostBack="true"
                        DataSourceID="SqlDataSource2" DataTextField="name" DataValueField="sl" 
                        onselectedindexchanged="DropDownList1_SelectedIndexChanged" >
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                        SelectCommand="SELECT [sl], [Name] FROM [Class] order by sl">
                    </asp:SqlDataSource>
                </td>
            </tr>


            <tr>
                <td>Month Name</td>
                <td>
                    <asp:DropDownList ID="ddMonth" runat="server"
                        DataSourceID="SqlDataSource3" DataTextField="Name" DataValueField="sl"></asp:DropDownList>
                    <%--<asp:DropDownList ID="ddMonth" runat="server" 
                        DataSourceID="SqlDataSource3" DataTextField="Name" DataValueField="sl"></asp:DropDownList>--%>
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                        SelectCommand="SELECT [sl], [Name] FROM [Months] ORDER BY [sl]">
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td>Year</td>
                <td>
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                </td>
            </tr>
            
            <tr style="background:none">
            <td></td>
                <td>
                    <asp:Button ID="btnSave"  CssClass="btn btn-s-md btn-primary"  runat="server" Text="Save" OnClick="btnSave_Click" />
                    <asp:Button ID="btnClear" type="reset"  CssClass="btn btn-s-md btn-primary"  runat="server" Text="Clear" />
                </td>
            </tr>
        </table>
    </fieldset>

    
    </div>
    </div>
<%--End Body Contants--%>

 
 </section>
 </div>



    
 <div class="col-lg-6"> 
 <section class="panel"> 
 
<div id="Div1">
    <div>

<fieldset>
        <legend>Saved Data</legend>
        

        

                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource1" onselectedindexchanged="GridView1_SelectedIndexChanged" DataKeyNames="sl">
                           
                            
                    <Columns>
                        
                
                                <asp:TemplateField HeaderText="sl" SortExpression="sl" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("sl") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                        <asp:BoundField DataField="Name" HeaderText="Name" 
                            SortExpression="Name" ReadOnly="True" />
                        <asp:BoundField DataField="NameNumeric" HeaderText="NameNumeric" 
                            SortExpression="NameNumeric" />
                        <asp:BoundField DataField="TutionFee" HeaderText="Tution Fee" 
                            SortExpression="TutionFee" />
                        <asp:BoundField DataField="ClassTeacher" HeaderText="ClassTeacher"
                            SortExpression="ClassTeacher" />
                                

                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">This entry will be deleted permanently!</b><br />
                                                    Are you sure you want to delete this ?
                                                            <br />
                                                    <br />
                                                    <div style="text-align: right;">
                                                        <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                        <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                    </div>
                                                </asp:Panel>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                                    
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"                                        
                    SelectCommand="SELECT sl, name, NameNumeric, TutionFee, (select name from Teachers where sl=Class.ClassTeacher) as ClassTeacher FROM [Class] ORDER BY [sl] DESC"
                    DeleteCommand="DELETE FROM Class WHERE (sl = @sl)"    >              
                <DeleteParameters>
                        <asp:Parameter Name="sl" />
                    </DeleteParameters>
                </asp:SqlDataSource>   


    </fieldset>



    </div>
    </div>
<%--End Body Contants--%>

 
 </section>
 </div>

</asp:Content>
