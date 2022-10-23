<%@ Page Language="C#" Title="Expense Groups" AutoEventWireup="true" MasterPageFile="Layout.Master" CodeBehind="Exp-Group.aspx.cs" Inherits="Oxford.app.Exp_Group" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>



    <asp:UpdatePanel ID="uPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>


            <div class="col-lg-6">
                <section class="panel">
                    <%--Body Contants--%>
                    <div id="Div2">
                        <div>

                            <fieldset>
                                <legend>Expense Groups</legend>
                                <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>

                                    <tr id="EditField" runat="server" class="hidden">
                                        <td>Edit info</td>
                                        <td>
                                            <asp:DropDownList name="" ID="DropDownList1" runat="server" AutoPostBack="true" CssClass="form-control"
                                                DataSourceID="SqlDataSource2" DataTextField="name" DataValueField="sl"
                                                OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [sl], [Name] FROM [ExpenseTypes] order by sl"></asp:SqlDataSource>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Expense Group</td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Description</td>
                                        <td>
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr style="display: none;">
                                        <td>Expense Type</td>
                                        <td>
                                            <asp:DropDownList ID="ddType" runat="server" CssClass="form-control">
                                                <asp:ListItem>Once a Year</asp:ListItem>
                                                <asp:ListItem>Always</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr style="background: none">
                                        <td></td>
                                        <td>
                                            <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnClear" type="reset" CssClass="btn btn-s-md btn-white" runat="server" Text="Clear" OnClick="btnClear_Click" />
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




                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-striped m-b-none text-sm"
                                    DataSourceID="SqlDataSource1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="sl">


                                    <Columns>

                                        <asp:TemplateField ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="40px" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="sl" SortExpression="sl" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("sl") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="Name" HeaderText="Name"
                                            SortExpression="Name" ReadOnly="True" />
                                        <asp:BoundField DataField="Remark" HeaderText="Description"
                                            SortExpression="Remark" />
                                        <asp:BoundField DataField="ReRun" HeaderText="Type" Visible="false"
                                            SortExpression="ReRun" />


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
                                    SelectCommand="SELECT sl, name, Remark, ReRun FROM [ExpenseTypes] ORDER BY [name]"
                                    DeleteCommand="DELETE FROM ExpenseTypes WHERE (sl = @sl)">
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


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
