<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="Layout.Master" CodeBehind="Student-List.aspx.cs" Inherits="Oxford.app.Student_List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>


    <asp:UpdatePanel ID="uPanel" runat="server" UpdateMode="Conditional">
        
        <ContentTemplate>


            <div class="col-lg-12">
                <section class="panel">
                    <%--Body Contants--%>
                    <div id="Div2">
                        <div>

                            <fieldset>
                                <legend>Print Student List</legend>



                                <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                            <asp:Image ID="Image1" runat="server" Width="300px" EnableViewState="false" Visible="False" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right">শ্রেণি : </td>
                                        <td>
                                            <asp:DropDownList ID="ddClass" runat="server" DataSourceID="SqlDataSource3" DataTextField="name" DataValueField="sl"
                                                AutoPostBack="true" OnSelectedIndexChanged="txtSession_TextChanged" CssClass="form-control">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [name], [sl] FROM [Class] ORDER BY [sl]"></asp:SqlDataSource>

                                        </td>
                                        <td>
                                            <input type="button" Class="btn btn-s-md btn-primary"  value="Print" onclick="PrintElem('.print-area')" />
                                            <%--<asp:Button ID="btnSave"  CssClass="btn btn-s-md btn-primary"  runat="server" Text="Print" OnClick="btnSave_Click" ValidationGroup="uPanel" />--%>
                                       </td>
                                    </tr>

                                </table>


                                <div class="print-area">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="tbl_default zebra table" BorderWidth="1px" Width="100%" CellPadding="3"
                                    DataSourceID="SqlDataSource1" DataKeyNames="sl">


                                    <Columns>
                                              
                <asp:TemplateField ItemStyle-Width="25px" FooterStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                    <ItemStyle CssClass="text-center" />
                </asp:TemplateField>

                                        <asp:BoundField DataField="StudentNameE" HeaderText="Student Name"
                                            SortExpression="StudentNameE" />
                                        <asp:BoundField DataField="StudentID" HeaderText="ID"
                                            SortExpression="StudentID" ReadOnly="True" />
                                        <asp:BoundField DataField="StudentNameE" HeaderText="Student Name"
                                            SortExpression="StudentNameE" />
                                        <asp:BoundField DataField="FatherNameE" HeaderText="Father Name"
                                            SortExpression="StudentNameE" />
                                        <asp:BoundField DataField="MotherNameE" HeaderText="Mother Name"
                                            SortExpression="MotherNameB" />
                                        
                                        
                                        <asp:BoundField DataField="MotherNameE" HeaderText="Mother Name"
                                            SortExpression="MotherNameB" />
                                        <asp:BoundField DataField="ClassName" HeaderText="Class"
                                            SortExpression="ClassName" />
                                        <asp:BoundField DataField="RollNumber" HeaderText="Roll No."
                                            SortExpression="RollNumber" />
                                        <asp:BoundField DataField="PhoneMobile" HeaderText="Mobile"
                                            SortExpression="PhoneMobile" />
                                        
                                        <asp:BoundField DataField="Due" HeaderText="Due"
                                            SortExpression="PhoneMobile" />

                                        <asp:TemplateField HeaderText="" SortExpression="sl" >
                                            <ItemTemplate>
                                                <%--<a href="Student-Info.aspx?sl='<%# Bind("sl") %>'" target="_blank"></a>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("sl") %>'></asp:Label>--%>
                                                <asp:HyperLink runat="server" NavigateUrl='<%# Eval("sl", "Student-Info.aspx?sl={0}") %>' Text='Full Profile' Target="_blank" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        


                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT StudentID, StudentNameB, StudentNameE, Class, FatherNameB, FatherNameE, FatherOccupation, FatherIncome, MotherNameB, MotherNameE, MotherOccupation, 
                      MotherIncome, PresentAddress, PhoneOffice, PhoneHouse, PhoneMobile, PermanentAddress, GuardianName, GuardianRelation, GuardianAddress, 
                                    (SELECT name from Class where sl=Students.Class) as ClassName,RollNumber,
                      GuardianOccupation, DOB, StudentAge, ExSchool, Religion, Nationality, VaccineInfo, ApplicationDate, BloodGroup, Hobby, sl,
                                     (SELECT SUM(Due) FROM BillingTmp WHERE (StudentID = Students.StudentID)) AS Due FROM [Students] where Class=@Class ORDER BY [RollNumber] "
                                    DeleteCommand="DELETE FROM VACCBankCash WHERE (sl = @sl)">
                                    <DeleteParameters>
                                        <asp:Parameter Name="sl" />
                                    </DeleteParameters>
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddClass" Name="Class" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                    </div>
                            </fieldset>



                        </div>
                    </div>
                    <%--End Body Contants--%>
                </section>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
