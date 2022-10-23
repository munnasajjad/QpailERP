<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="SMS-Template.aspx.cs" Inherits="Oxford.app.SMS_Template" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>

            
            <div class="col-lg-6">
                <section class="panel">
                    <%--Body Contants--%>
                    <div id="Div2">
                        <div>

                            <fieldset>
                                <legend>SMS Templates</legend>
                                <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>

                                    <tr id="EditField" runat="server" class="hidden">
                                        <td>Edit Template</td>
                                        <td>
                                            <asp:DropDownList name="" ID="DropDownList1" runat="server" AutoPostBack="true" CssClass="form-control"
                                                DataSourceID="SqlDataSource2" DataTextField="name" DataValueField="sl"
                                                OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [sl], [Name] FROM SMSTemplate order by sl"></asp:SqlDataSource>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Title</td>
                                        <td>
                                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>SMS Body</td>
                                        <td>
                                            <asp:TextBox ID="txtText" runat="server" CssClass="form-control" Rows="4" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>


                                    <tr style="background: none">
                                        <td></td>
                                        <td>
                                            <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnClear" type="reset" CssClass="btn btn-s-md btn-white" runat="server" Text="Clear" />
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
                                <legend>Saved Templates</legend>




                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-striped m-b-none text-sm"
                                    DataSourceID="SqlDataSource1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="sl"
                                    OnRowDeleting="GridView1_RowDeleting">


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

                                        <asp:BoundField DataField="Name" HeaderText="Title"
                                            SortExpression="Name" ReadOnly="True" />

                                        <asp:BoundField DataField="SmsBody" HeaderText="SMS Text"
                                            SortExpression="SmsBody" />

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
                                    SelectCommand="SELECT sl, name, SmsBody FROM SMSTemplate ORDER BY name,[sl] DESC"
                                    DeleteCommand="DELETE FROM SMSTemplate WHERE (sl = @sl)">
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
