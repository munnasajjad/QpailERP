<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/app/Layout.Master" CodeBehind="Search.aspx.cs" Inherits="Oxford.app.Search" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>


    <asp:UpdatePanel ID="uPanel" runat="server"  UpdateMode="Conditional">
        <%--<Triggers>
           <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>--%>
        <ContentTemplate>


            <div class="col-lg-4">
                <section class="panel">
                    <%--Body Contants--%>
                    <div id="Div2">
                        <div>

                            <fieldset>
                                <legend>Search Student Data</legend>
                                <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Label ID="lblMsg" runat="server" Text="" EnableViewState="false" />          
                                            <asp:Image ID="Image1" runat="server" Width="300px" EnableViewState="false" />            
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>শ্রেণি </td>
                                        <td>
                                            <asp:DropDownList ID="ddClass" runat="server" DataSourceID="SqlDataSource3" DataTextField="name" DataValueField="sl"
                                                  AppendDataBoundItems="true"> 
                                                <asp:ListItem>---ALL---</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [name], [sl] FROM [Class] ORDER BY [sl]"></asp:SqlDataSource>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Student ID</td>
                                        <td>
                                            <asp:TextBox ID="txtStudentId" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>শিক্ষার্থীর নামঃ (বাংলায়)</td>
                                        <td>
                                            <asp:TextBox ID="txtStudentNameB" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>(ইংরেজি)</td>
                                        <td>
                                            <asp:TextBox ID="txtStudentNameE" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>রোল নং</td>
                                        <td>
                                            <asp:TextBox ID="txtRollNo" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>লিঙ্গ </td>
                                        <td>
                                            <asp:DropDownList ID="ddGender" runat="server" > 
                                           <asp:ListItem>---ALL---</asp:ListItem>
                                                <asp:ListItem>ছেলে</asp:ListItem>
                                                <asp:ListItem>মেয়ে</asp:ListItem>
 </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>ধর্ম </td>
                                        <td>                                            
                                            <asp:DropDownList ID="ddReligion" runat="server" DataSourceID="SqlDataSource2" DataTextField="ReligionEnglish" DataValueField="ReligionEnglish"
                                                  AppendDataBoundItems="true"> 
                                                <asp:ListItem>---ALL---</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                                SelectCommand="SELECT [ReligionEnglish] FROM [Religions] ORDER BY [PID]"></asp:SqlDataSource>

                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>Admission Year</td>
                                        <td>
                                            <asp:TextBox ID="txtSession" runat="server" ></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr style="background: none">
                                        <td></td>
                                        <td style="text-align: center;">
                                            <asp:Button ID="btnSave"  CssClass="btn btn-s-md btn-primary"  runat="server" Text="Search" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnReset"  CssClass="btn btn-s-md btn-white"  runat="server" Text="Reset" OnClick="btnReset_Click"  />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>


                        </div>
                    </div>
                   
                </section>
            </div>




            <div class="col-lg-8">
                <section class="panel">

                    <div id="Div1">
                        <div>

                            <fieldset>
                                <legend>Search Result </legend>


                                Total Search Result: <asp:Literal ID="ltrtotal" runat="server"></asp:Literal>

                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                                     OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="sl">


                                    <Columns>


                                        <asp:TemplateField HeaderText="sl" SortExpression="sl" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("sl") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="StudentID" HeaderText="ID"
                                            SortExpression="StudentID" ReadOnly="True" />
                                        <asp:BoundField DataField="StudentNameB" HeaderText="শিক্ষার্থীর নাম"
                                            SortExpression="StudentNameB" />
                                        <asp:BoundField DataField="StudentNameE" HeaderText="Student Name"
                                            SortExpression="StudentNameE" />
                                        <asp:BoundField DataField="Class" HeaderText="Class"
                                            SortExpression="Class" />
                                        <asp:BoundField DataField="RollNumber" HeaderText="Roll"
                                            SortExpression="RollNumber" />



                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" Visible="false" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">Item will be deleted permanently!</b><br />
                                                    Are you sure you want to delete the item from order list?
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
                                    SelectCommand="SELECT StudentID, StudentNameB, StudentNameE, Class, FatherNameB, FatherNameE, FatherOccupation, FatherIncome, MotherNameB, MotherNameE, MotherOccupation, 
                      MotherIncome, PresentAddress, PhoneOffice, PhoneHouse, PhoneMobile, PermanentAddress, GuardianName, GuardianRelation, GuardianAddress, 
                      GuardianOccupation, DOB, StudentAge, ExSchool, Religion, Nationality, VaccineInfo, ApplicationDate, BloodGroup, Hobby, sl FROM [Students] where Class=@Class ORDER BY [sl] DESC"
                                    DeleteCommand="DELETE FROM VACCBankCash WHERE (sl = @sl)">
                                    <DeleteParameters>
                                        <asp:Parameter Name="sl" />
                                    </DeleteParameters>
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddClass" Name="Class" PropertyName="SelectedValue" />
                                    </SelectParameters>
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
