<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="DiscountToStudents.aspx.cs" Inherits="Oxford.app.DiscountToStudents" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>


    <asp:UpdatePanel ID="uPanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>


            <div class="col-lg-6">
                <section class="panel">

                    <asp:Label ID="lblOldEntryId" runat="server" Text="" Visible="false"></asp:Label>
                    <fieldset>
                        <legend>Assign Discount (By Students)</legend>


                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <label class="control-label">Class : </label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddClass" runat="server" data-placeholder="--- Select ---" TabIndex="1"
                                        OnSelectedIndexChanged="ddClass_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" DataSourceID="SqlDataSource4" DataTextField="Name" DataValueField="sl">
                                    </asp:DropDownList>

                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [Name] FROM [Class] ORDER BY [Name]"></asp:SqlDataSource>

                                </td>
                                <td></td>
                            </tr>


                            <tr>
                                <td>

                                    <label class="control-label">Student : </label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddStudent" runat="server" data-placeholder="--- Select ---" TabIndex="1"
                                        DataSourceID="SqlDataSource5" DataTextField="StudentNameE" DataValueField="StudentID" CssClass="form-control"
                                        OnSelectedIndexChanged="ddStudent_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>

                                    <asp:SqlDataSource ID="SqlDataSource5" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [StudentNameE], [StudentID] FROM [Students] WHERE ([Class] = @Class) ORDER BY [StudentNameE]">
                                        <SelectParameters><asp:ControlParameter ControlID="ddClass" Name="Class" PropertyName="SelectedValue" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:Literal ID="ltrID" runat="server"></asp:Literal>
                                </td>
                                <td></td>
                            </tr>

                            <tr>
                                <td>
                                    <label class="control-label">Collection Group</label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddGroup" runat="server" AutoPostBack="true" CssClass="form-control"
                                        DataSourceID="SqlDataSource7" DataTextField="name" DataValueField="sl"
                                        OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [Name] FROM [CollectionTypes] order by sl"></asp:SqlDataSource>

                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Collection Head : "></asp:Label>

                                </td>
                                <td>
                                    <asp:DropDownList ID="ddHead" runat="server" CssClass="form-control" AutoPostBack="true"
                                        DataSourceID="SqlDataSource2" DataTextField="name" DataValueField="sl" OnSelectedIndexChanged="ddHead_SelectedIndexChanged" AppendDataBoundItems="True" >
                                        <asp:ListItem>--- all ---</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [Name] FROM [CollectionHeads] WHERE ([GroupID] = @GroupID) ORDER BY [Name]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddGroup" Name="GroupID" PropertyName="SelectedValue" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:Label ID="lblSl" runat="server" Text="" Visible="false"></asp:Label>
                                </td>
                                <td></td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="Description/ Remarks : "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>

                            <tr>
                                <td>Discount Amount</td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="RadioButton1" runat="server" GroupName="g" Text="Taka" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadioButton2" runat="server" GroupName="g" Text="Percentage" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtAmount" runat="server" Width="100px" CssClass="form-control"></asp:TextBox>
                                                <span style="color: red">
                                                    <asp:Literal ID="ltrNotice" runat="server"></asp:Literal></span>
                                                <asp:FilteredTextBoxExtender ID="txtOpBalance_FilteredTextBoxExtender2"
                                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtAmount">
                                                </asp:FilteredTextBoxExtender>

                                            </td>
                                        </tr>

                                    </table>


                                </td>
                                <td></td>
                            </tr>

                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-s-md btn-info" Text="Save" OnClick="btnSave_Click" />
                                </td>
                                <td>&nbsp;</td>
                            </tr>

                        </table>
                    </fieldset>

                </section>
            </div>




            <div class="col-lg-6">
                <section class="panel">

                    <div id="Div1">
                        <div>

                            <fieldset>
                                <legend>Discounts for the Student</legend>




                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                                    DataSourceID="SqlDataSource1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="did">


                                    <Columns>

                                        <asp:TemplateField HeaderText="sl" SortExpression="sl" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("did") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="Ctype" HeaderText="C.type"
                                            SortExpression="Name" ReadOnly="True" />
                                        <asp:BoundField DataField="CHead" HeaderText="Head"
                                            SortExpression="NameNumeric" />
                                        <asp:BoundField DataField="Description" HeaderText="Description"
                                            SortExpression="TutionFee" />
                                        <asp:BoundField DataField="Amount" HeaderText="Discount"
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
                                    SelectCommand="SELECT did, 
                         (Select Name from CollectionTypes where sl= Discounts.[DiscountGroup]) AS Ctype , 
                         (Select Name from CollectionHeads where sl= Discounts.[DiscountHead]) AS CHead , 
                           [Description],
                          [Description], CONVERT(NVarchar, [DiscountAmount]+ [DiscountPerchantage]) +' '+ [DiscountType] AS Amount FROM [Discounts]  WHERE ([StudentID] = @StudentID) ORDER BY [did]">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddStudent" Name="StudentID" PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                            </fieldset>

                        </div>
                    </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
