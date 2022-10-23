<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="Send-SMS.aspx.cs" Inherits="Oxford.app.Send_SMS" %>

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

                    <div id="Div2">
                        <div>

                            <fieldset>
                                <legend>Send SMS to Students</legend>
                                <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>SMS Template</td>
                                        <td>
                                            <asp:DropDownList ID="ddTemplate" runat="server" data-placeholder="--- Select ---" TabIndex="1" CssClass="btn btn-sm btn-white dropdown-toggle"
                                                DataSourceID="SqlDataSource3" DataTextField="name" DataValueField="sl" AutoPostBack="True" OnSelectedIndexChanged="ddTemplate_OnSelectedIndexChanged">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [name], [sl] FROM [SMSTemplate] ORDER BY [name]"></asp:SqlDataSource>

                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td colspan="3">SMS Text
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtSMS" runat="server" TextMode="MultiLine" Rows="4" MaxLength="150"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="2">Send to All Students</td>

                                        <td>
                                            <asp:LinkButton ID="lbAll" runat="server" CssClass="btn btn-sm btn-default" OnClick="lbAll_Click">Send SMS</asp:LinkButton>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td>Send to Class</td>
                                        <td>
                                            <asp:DropDownList ID="ddClass" runat="server" data-placeholder="--- Select ---" TabIndex="1" CssClass="btn btn-sm btn-white dropdown-toggle"
                                                OnSelectedIndexChanged="ddClass_SelectedIndexChanged" AutoPostBack="true" DataSourceID="SqlDataSource5" DataTextField="Name" DataValueField="sl">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource5" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [sl], [Name] FROM [Class] ORDER BY [Name]"></asp:SqlDataSource>

                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lbClass" runat="server" CssClass="btn btn-sm btn-default" OnClick="lbClass_Click">Send SMS</asp:LinkButton>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Send to Student</td>
                                        <td>
                                            <asp:DropDownList ID="ddStudent" runat="server" data-placeholder="--- Select ---" TabIndex="1" CssClass="btn btn-sm btn-white dropdown-toggle"
                                                DataSourceID="SqlDataSource2" DataTextField="StudentNameE" DataValueField="StudentID">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [StudentNameE], [StudentID] FROM [Students] WHERE ([Class] = @Class) ORDER BY [StudentNameE]">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddClass" Name="Class" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>

                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lbStudent" runat="server" CssClass="btn btn-sm btn-default" OnClick="lbStudent_Click">Send SMS</asp:LinkButton>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Send to Others</td>
                                        <td>
                                            <asp:TextBox ID="txtOther" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtOpBalance_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="+0123456789" TargetControlID="txtOther">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lbOthers" runat="server" CssClass="btn btn-sm btn-default" OnClick="lbOthers_OnClick">Send SMS</asp:LinkButton>
                                        </td>
                                    </tr>

                                </table>


                            </fieldset>


                        </div>
                    </div>

                </section>
            </div>




            <div class="col-lg-6">
                <section class="panel">

                    <div id="Div1">
                        <div>

                            <fieldset>
                                <legend>List of Students</legend>




                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                                    DataSourceID="SqlDataSource1" DataKeyNames="sl">


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
                                        <asp:BoundField DataField="PhoneMobile" HeaderText="Mobile"
                                            SortExpression="PhoneMobile" />


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
                </section>
            </div>

            <%--<div class="col-lg-12">
                <section class="panel">

                    <div style="-webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; word-wrap: break-word;">
                        <asp:Label ID="lblMob" runat="server" CssClass="wrapper"></asp:Label>
                    </div>
            
            </section>

            </div>--%>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
