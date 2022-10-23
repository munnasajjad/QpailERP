<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="Teachers.aspx.cs" Inherits="Oxford.app.Teachers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:updatepanel id="upl" runat="server" UpdateMode="Conditional">
        <Triggers>
           <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>
                          
    <div class="col-lg-6">
        <section class="panel">
            <%--Body Contants--%>
            <div id="Div2">
                <div>

                    <fieldset>
                        <legend>Add new Teacher</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                </td>
                            </tr>
                            <tr id="EditField" runat="server" class="hidden">
                                <td>Edit Entry</td>
                                <td>
                                    <asp:DropDownList name="" ID="DropDownList1" runat="server" AutoPostBack="true"
                                        DataSourceID="SqlDataSource2" DataTextField="name" DataValueField="sl" CssClass="form-control" 
                                        OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [Name] FROM [Teachers] order by sl"></asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td>Teacher ID</td>
                                <td>
                                    <asp:TextBox ID="txtTeacherID" runat="server" CssClass="form-control" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Name</td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Designation</td>
                                <td>
                                    <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Education Qualification</td>
                                <td>
                                    <asp:TextBox ID="txtEducation" runat="server" CssClass="form-control" ></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>Date of Birth</td>
                                <td>
                                    <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" ></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                        Enabled="True" TargetControlID="txtDOB">
                                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>Gender</td>
                                <td>
                                    <asp:DropDownList ID="ddGender" runat="server" CssClass="form-control" >
                                        <asp:ListItem>Male</asp:ListItem>
                                        <asp:ListItem>Female</asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td>Address</td>
                                <td>
                                    <asp:TextBox ID="txtaddress" runat="server" CssClass="form-control" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Phone Number</td>
                                <td>
                                    <asp:TextBox ID="txtphone" runat="server" CssClass="form-control" ></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" ValidChars="+0123456789" TargetControlID="txtphone">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>Email address</td>
                                <td>
                                    <asp:TextBox ID="txtemail" runat="server"  CssClass="form-control" ></asp:TextBox>
                                </td>
                            </tr>


                            <tr style="padding-right: 5px;">
                                <td>Photo </td>
                                <td>
                                    <asp:Image ID="Image1" runat="server" Width="200px" EnableViewState="false"   />
                                    <asp:FileUpload ID="FileUpload1" runat="server"  ClientIDMode="Static" CssClass="form-control"/>
                                  

                                </td>
                            </tr>
                            <tr style="background: none">
                                <td></td>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnSave"  CssClass="btn btn-s-md btn-primary"  runat="server" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnClear" type="reset"  CssClass="btn btn-s-md btn-white"  runat="server" Text="Clear" OnClick="btnClear_Click" />
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
                        <legend>Teachers Record</legend>




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
                                <asp:BoundField DataField="Address" HeaderText="Address"
                                    SortExpression="address" />
                                <asp:BoundField DataField="Phone" HeaderText="Phone"
                                    SortExpression="phone" />
                                <asp:BoundField DataField="Email" HeaderText="Email"
                                    SortExpression="email" />


                                <asp:TemplateField ShowHeader="False" ItemStyle-Width="90px">
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
                            SelectCommand="SELECT sl, TeacherID, Name, DOB, Gender, address, phone, email, Photo FROM [Teachers] ORDER BY [sl] DESC"
                            DeleteCommand="DELETE FROM Teachers WHERE (sl = @sl)">
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
    </asp:updatepanel>

</asp:Content>
