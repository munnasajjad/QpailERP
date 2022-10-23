<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="Admission.aspx.cs" Inherits="Oxford.app.Admission" MaintainScrollPositionOnPostback="true" %>

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
                    <%--Body Contants--%>
                    <div id="Div2">
                        <div>

                            <fieldset>
                                <legend>ভর্তি (ADMISSION)</legend>



                                <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                            <asp:Image ID="Image1" runat="server" Width="300px"   />
                                        </td>
                                    </tr>

                                    <tr id="EditField" runat="server" class="hidden">
                                        <td>Edit Student info</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true"
                                                DataSourceID="SqlDataSource2" DataTextField="StudentNameE" DataValueField="sl"
                                                OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <%--<asp:DropDownList name="" ID="DropDownList1" runat="server"  AutoPostBack="true"
                        DataSourceID="SqlDataSource2" DataTextField="StudentNameE" DataValueField="sl" 
                        onselectedindexchanged="DropDownList1_SelectedIndexChanged" >
                    </asp:DropDownList>--%>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [sl], [StudentNameE] FROM [Students] order by StudentNameE"></asp:SqlDataSource>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>শ্রেণি </td>
                                        <td>
                                            <asp:DropDownList ID="ddClass" runat="server" DataSourceID="SqlDataSource3" DataTextField="name" DataValueField="sl"
                                                AutoPostBack="true" OnSelectedIndexChanged="txtSession_TextChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [name], [sl] FROM [Class] ORDER BY [sl]"></asp:SqlDataSource>

                                            <%-- <asp:ListItem>প্রথম শ্রেণি</asp:ListItem>
                                                <asp:ListItem>দ্বিতীয় শ্রেণি</asp:ListItem>
                                                <asp:ListItem>তৃতীয় শ্রেণি</asp:ListItem>
                                                <asp:ListItem>চতুর্থ শ্রেণি</asp:ListItem>
                                                <asp:ListItem>পঞ্চম শ্রেণি</asp:ListItem>
                                                <asp:ListItem>ষষ্ঠ শ্রেণি</asp:ListItem>
                                                <asp:ListItem>সপ্তম শ্রেণি</asp:ListItem>
                                                <asp:ListItem>অষ্টম শ্রেণি</asp:ListItem>
                                                <asp:ListItem>নবম শ্রেণি</asp:ListItem>
                                                <asp:ListItem>দশম শ্রেণি</asp:ListItem>
                                                <asp:ListItem>একাদশ শ্রেণি</asp:ListItem>
                                                <asp:ListItem>দ্বাদশ শ্রেণি</asp:ListItem>--%>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Session</td>
                                        <td>
                                            <asp:TextBox ID="txtSession" runat="server" AutoPostBack="true" OnTextChanged="txtSession_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Student ID</td>
                                        <td>
                                            <asp:Label ID="lblStudentID" runat="server">0001</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>শিক্ষার্থীর নামঃ (বাংলায়)</td>
                                        <td>
                                            <asp:TextBox ID="txtStudentNameB" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>শিক্ষার্থীর নামঃ (ইংরেজি) <font color="red">*</font>
                                            <asp:RequiredFieldValidator ID="FnameR" runat="server" ControlToValidate="txtStudentNameE" ErrorMessage="Student Name is required." ForeColor="Red" ToolTip="Name is required." ValidationGroup="uPanel">*</asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStudentNameE" runat="server" ValidationGroup="uPanel"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>পিতার নামঃ (বাংলায়)</td>
                                        <td>
                                            <asp:TextBox ID="txtFatherNameB" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>পিতার নামঃ (ইংরেজি) <font color="red">*</font>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFatherNameE" ErrorMessage="Fathers Name is required." ForeColor="Red" ToolTip="Fathers Name is required." ValidationGroup="uPanel">*</asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFatherNameE" runat="server" ValidationGroup="uPanel"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>পেশা</td>
                                        <td>
                                            <asp:TextBox ID="txtFatherOccupation" runat="server" Enabled="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>বার্ষিক আয়</td>
                                        <td>
                                            <asp:TextBox ID="txtFatherIncome" runat="server" Enabled="true"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" ValidChars=",0123456789" TargetControlID="txtFatherIncome">
                                        </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>মাতার  নামঃ (বাংলায়)</td>
                                        <td>
                                            <asp:TextBox ID="txtMotherNameB" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>(ইংরেজি)</td>
                                        <td>
                                            <asp:TextBox ID="txtMotherNameE" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>পেশা</td>
                                        <td>
                                            <asp:TextBox ID="txtMotherOccupation" runat="server" Enabled="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>বার্ষিক আয়</td>
                                        <td>
                                            <asp:TextBox ID="txtMotherIncome" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" ValidChars=",0123456789" TargetControlID="txtMotherIncome">
                                        </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>লিঙ্গ </td>
                                        <td>
                                            <asp:DropDownList ID="ddGender" runat="server">

                                                <asp:ListItem>Male</asp:ListItem>
                                                <asp:ListItem>Female</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>বর্তমান ঠিকানাঃ</td>
                                        <td>
                                            <asp:TextBox ID="txtPresentAddress" TextMode="MultiLine" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>ফোনঃ অফিস </td>
                                        <td>
                                            <asp:TextBox ID="txtPhoneOffice" runat="server"></asp:TextBox></td>
                                        <asp:FilteredTextBoxExtender ID="txtMobile_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" ValidChars="+0123456789" TargetControlID="txtPhoneOffice">
                                        </asp:FilteredTextBoxExtender>
                                    </tr>
                                    <tr>
                                        <td>বাসা</td>
                                        <td>
                                            <asp:TextBox ID="txtPhoneHouse" runat="server"></asp:TextBox></td>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                            Enabled="True" FilterType="Custom, Numbers" ValidChars="+0123456789" TargetControlID="txtPhoneHouse">
                                        </asp:FilteredTextBoxExtender>
                                    </tr>
                                    <tr>
                                        <td>মোবাইল <font color="red">*</font>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPhoneMobile" ErrorMessage="Mobile# is required." ForeColor="Red" ToolTip="Mobile# is required." ValidationGroup="uPanel">*</asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhoneMobile" runat="server" ValidationGroup="uPanel"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                Enabled="True" FilterType="Custom, Numbers" ValidChars="+0123456789" TargetControlID="txtPhoneMobile">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>স্থায়ী ঠিকানা</td>
                                        <td>
                                            <asp:TextBox ID="txtPermanentAddress" runat="server" Text=""></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>অভিভাবকের নামঃ</td>
                                        <td>
                                            <asp:TextBox ID="txtGuardianName" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>আবেদনকারীর সাথে সম্পর্ক </td>
                                        <td>
                                            <asp:TextBox ID="txtGuardianRelation" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>অভিভাবকের ঠিকানাঃ </td>
                                        <td>
                                            <asp:TextBox ID="txtGuardianAddress" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>অভিভাবকের পেশা</td>
                                        <td>
                                            <asp:TextBox ID="txtGuardianOccupation" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>শিক্ষার্থীর জন্ম তারিখঃ</td>
                                        <td>
                                            <asp:TextBox ID="txtDOB" runat="server"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                                Enabled="True" TargetControlID="txtDOB" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr style="display:none;">
                                        <td>বয়স</td>
                                        <td>
                                            <asp:TextBox ID="txtStudentAge" runat="server"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>যে বিদ্যালয় হতে এসেছে</td>
                                        <td>
                                            <asp:TextBox ID="txtExSchool" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>ধর্ম </td>
                                        <td>
                                            <asp:DropDownList ID="ddReligion" runat="server" DataSourceID="SqlDataSource4" DataTextField="ReligionEnglish" DataValueField="ReligionEnglish"
                                                AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [ReligionEnglish] FROM [Religions] ORDER BY [PID]"></asp:SqlDataSource>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>জাতীয়তা</td>
                                        <td>
                                            <asp:TextBox ID="txtNationality" runat="server" Text="Bangladeshi"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>টিকা সংক্রান্ত তথ্য </td>
                                        <td>
                                            <asp:TextBox ID="txtVaccineInfo" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>আবেদনের তারিখঃ</td>
                                        <td>
                                            <asp:TextBox ID="txtApplicationDate" runat="server"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                                                Enabled="True" TargetControlID="txtApplicationDate" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>রক্তের গ্রুপঃ</td>
                                        <td>
                                            <asp:TextBox ID="txtBloodGroup" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>সখঃ</td>
                                        <td>
                                            <asp:TextBox ID="txtHobby" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="padding-right: 5px;">
                                        <td>Student Photo </td>
                                        <td>
                                            <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" />
                                        </td>
                                    </tr>

                                    <tr style="padding-right: 5px;">
                                        <td>Student Signature</td>
                                        <td>
                                            <asp:FileUpload ID="FileUpload2" runat="server" ClientIDMode="Static" />
                                        </td>
                                    </tr>
                                    <tr style="padding-right: 5px;">
                                        <td>Guardian Signature</td>
                                        <td>
                                            <asp:FileUpload ID="FileUpload3" runat="server" ClientIDMode="Static" />
                                        </td>
                                    </tr>

                                    <tr style="background: none">
                                        <td></td>
                                        <td style="text-align: center;">
                                            <div style="color: Red">
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="uPanel" />
                                                <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                            </div>
                                            <asp:Button ID="btnSave"  CssClass="btn btn-s-md btn-primary"  runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="uPanel" />
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
                                <legend>List of Students</legend>




                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                                    DataSourceID="SqlDataSource1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="sl">


                                    <Columns>
                                              
                <asp:TemplateField ItemStyle-Width="25px" FooterStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                    <ItemStyle CssClass="text-center" />
                </asp:TemplateField>

                                        <asp:TemplateField HeaderText="sl" SortExpression="sl" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("sl") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="StudentID" HeaderText="ID"
                                            SortExpression="StudentID" ReadOnly="True" />
                                        <asp:BoundField DataField="StudentNameE" HeaderText="Student Name"
                                            SortExpression="StudentNameE" />
                                        <asp:BoundField DataField="FatherNameE" HeaderText="Father Name"
                                            SortExpression="StudentNameE" />
                                        <%--<asp:BoundField DataField="MotherNameB" HeaderText="Mother Name"
                                            SortExpression="MotherNameB" />--%>
                                        <asp:BoundField DataField="PhoneMobile" HeaderText="Mobile"
                                            SortExpression="PhoneMobile" />


                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" />

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
