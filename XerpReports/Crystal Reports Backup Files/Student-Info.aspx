<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Student-Info.aspx.cs" Inherits="Oxford.app.Student_Info" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Info</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link rel="stylesheet" href="../css/app.v1.css">
    
    <style type="text/css">
        form#form1 table td:first-child {
           
            font-weight: bold;
            color: #808080;
            margin: 10px;
        }

        /*form#form1 table {
            width: 100%;
            font-size: 15px;
        }

        form#form1 {
            max-width: 1000px;
            margin: 0 auto;
        }*/

        tr td {
            padding: 5px 5px;
            line-height: 22px;
        }
    </style>

    <style type="text/css" media="print">
        @page {
            size: auto; /* auto is the initial value */
            margin: 0mm; /* this affects the margin in the printer settings */
        }

        #print-button {
            display: none;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        
        <%--<br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />--%>
        <div>
            <h2 class="text-center table-bordered">Students Detail Information</h2>

            <asp:ListView ID="ListView1" runat="server" DataKeyNames="StudentID" DataSourceID="SqlDataSource1">

                <EmptyDataTemplate>
                    <span>No data was returned.</span>
                </EmptyDataTemplate>

                <ItemTemplate>


                    <table style="width: 100%; margin-left: 50px">
                        <tbody>
                            <tr>
                                <td>

                                    <table>
                                        <tbody>


                                            <tr>
                                                <td>Student ID:</td>
                                                <td>
                                                    <asp:Label ID="StudentIDLabel" runat="server" Text='<%# Eval("StudentID") %>' />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Student Name (B):</td>
                                                <td>
                                                    <asp:Label ID="StudentNameBLabel" runat="server" Text='<%# Eval("StudentNameB") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>StudentName (E):</td>
                                                <td>
                                                    <asp:Label ID="StudentNameELabel" runat="server" Text='<%# Eval("StudentNameE") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Class:</td>
                                                <td>
                                                    <asp:Label ID="ClassLabel" runat="server" Text='<%# Eval("Class") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Fathers Name (B):</td>
                                                <td>
                                                    <asp:Label ID="FatherNameBLabel" runat="server" Text='<%# Eval("FatherNameB") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Fathers Name (E):</td>
                                                <td>
                                                    <asp:Label ID="FatherNameELabel" runat="server" Text='<%# Eval("FatherNameE") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Phone Office:</td>
                                                <td>
                                                    <asp:Label ID="PhoneOfficeLabel" runat="server" Text='<%# Eval("PhoneOffice") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Present Address:</td>
                                                <td>
                                                    <asp:Label ID="PresentAddressLabel" runat="server" Text='<%# Eval("PresentAddress") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Mothers Income:</td>
                                                <td>
                                                    <asp:Label ID="MotherIncomeLabel" runat="server" Text='<%# Eval("MotherIncome") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Mothers Occupation:</td>
                                                <td>
                                                    <asp:Label ID="MotherOccupationLabel" runat="server" Text='<%# Eval("MotherOccupation") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Mother Name E:</td>
                                                <td>
                                                    <asp:Label ID="MotherNameELabel" runat="server" Text='<%# Eval("MotherNameE") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Mother Name B:</td>
                                                <td>
                                                    <asp:Label ID="MotherNameBLabel" runat="server" Text='<%# Eval("MotherNameB") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Father Income:</td>
                                                <td>
                                                    <asp:Label ID="FatherIncomeLabel" runat="server" Text='<%# Eval("FatherIncome") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Father Occupation:</td>
                                                <td>
                                                    <asp:Label ID="FatherOccupationLabel" runat="server" Text='<%# Eval("FatherOccupation") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>DOB:</td>
                                                <td>
                                                    <asp:Label ID="DOBLabel" runat="server" Text='<%# Eval("DOB") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Guardian Occupation:</td>
                                                <td>
                                                    <asp:Label ID="GuardianOccupationLabel" runat="server" Text='<%# Eval("GuardianOccupation") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Guardian Address:</td>
                                                <td>
                                                    <asp:Label ID="GuardianAddressLabel" runat="server" Text='<%# Eval("GuardianAddress") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Guardian Name:</td>
                                                <td>
                                                    <asp:Label ID="GuardianNameLabel" runat="server" Text='<%# Eval("GuardianName") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Guardian Relation:</td>
                                                <td>
                                                    <asp:Label ID="GuardianRelationLabel" runat="server" Text='<%# Eval("GuardianRelation") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Permanent Address:</td>
                                                <td>
                                                    <asp:Label ID="PermanentAddressLabel" runat="server" Text='<%# Eval("PermanentAddress") %>' />
                                                </td>
                                            </tr>


                                            <tr>
                                                <td>Mobile:</td>
                                                <td>
                                                    <asp:Label ID="PhoneMobileLabel" runat="server" Text='<%# Eval("PhoneMobile") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Phone Home:</td>
                                                <td>
                                                    <asp:Label ID="PhoneHouseLabel" runat="server" Text='<%# Eval("PhoneHouse") %>' />
                                                </td>
                                            </tr>
                                            
                                            
                                        </tbody>
                                    </table>


                                </td>
                                <td style="width: 5%"></td>
                                <td>


                                    <table style="vertical-align: top;">
                                        <tbody>


                                            <tr>
                                                <td>Hobby:</td>
                                                <td>
                                                    <asp:Label ID="HobbyLabel" runat="server" Text='<%# Eval("Hobby") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>BloodGroup:</td>
                                                <td>
                                                    <asp:Label ID="BloodGroupLabel" runat="server" Text='<%# Eval("BloodGroup") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>VaccineInfo:</td>
                                                <td>
                                                    <asp:Label ID="VaccineInfoLabel" runat="server" Text='<%# Eval("VaccineInfo") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>ApplicationDate:</td>
                                                <td>
                                                    <asp:Label ID="ApplicationDateLabel" runat="server" Text='<%# Eval("ApplicationDate") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Nationality:</td>
                                                <td>
                                                    <asp:Label ID="NationalityLabel" runat="server" Text='<%# Eval("Nationality") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Religion:</td>
                                                <td>
                                                    <asp:Label ID="ReligionLabel" runat="server" Text='<%# Eval("Religion") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>ExSchool:</td>
                                                <td>
                                                    <asp:Label ID="ExSchoolLabel" runat="server" Text='<%# Eval("ExSchool") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>StudentAge:</td>
                                                <td>
                                                    <asp:Label ID="StudentAgeLabel" runat="server" Text='<%# Eval("StudentAge") %>' />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>EntryDate:</td>
                                                <td>
                                                    <asp:Label ID="EntryDateLabel" runat="server" Text='<%# Eval("EntryDate") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>EntryBy:</td>
                                                <td>
                                                    <asp:Label ID="EntryByLabel" runat="server" Text='<%# Eval("EntryBy") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Gender:</td>
                                                <td>
                                                    <asp:Label ID="GenderLabel" runat="server" Text='<%# Eval("Gender") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Section:</td>
                                                <td>
                                                    <asp:Label ID="SectionLabel" runat="server" Text='<%# Eval("Section") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Session:</td>
                                                <td>
                                                    <asp:Label ID="SessionLabel" runat="server" Text='<%# Eval("Session") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Roll Number:</td>
                                                <td>
                                                    <asp:Label ID="RollNumberLabel" runat="server" Text='<%# Eval("RollNumber") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Students Photo:</td>
                                                <td></td>
                                            </tr>
                                            <tr>

                                                <td colspan="2">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl='<%# Eval("StudentPhoto") %>' Width="150px" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Students Signature:</td>
                                                <td>
                                                    <asp:Image ID="Image3" runat="server" ImageUrl='<%# Eval("StudentSignature") %>' Width="150px"/>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>Guardians Signature:</td>
                                                <td>
                                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("GuardianSignature") %>' Width="150px"/>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>


                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <hr />
                    </span>
                </ItemTemplate>
                <LayoutTemplate>
                    <div id="itemPlaceholderContainer" runat="server" style="">
                        <span runat="server" id="itemPlaceholder" />
                    </div>
                    <div style="">
                    </div>
                </LayoutTemplate>


            </asp:ListView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                SelectCommand="SELECT TOP(1) [StudentID], [StudentNameB], [StudentNameE], 
                (SELECT name from Class where sl=Students.Class) as Class, 
                [FatherNameB], [FatherNameE], [PhoneOffice], [PresentAddress], [MotherIncome], [MotherOccupation], [MotherNameE], [MotherNameB], [FatherIncome], [FatherOccupation], [DOB], [GuardianOccupation], [GuardianAddress], [GuardianName], [GuardianRelation], [PermanentAddress], [PhoneMobile], [PhoneHouse], [Hobby], [BloodGroup], [VaccineInfo], [ApplicationDate], [Nationality], [Religion], [ExSchool], [StudentAge], [StudentSignature], [EntryDate], [EntryBy], [Gender], 
                (SELECT name from Section where sl=Students.Section) as Section, 
                [Session], [RollNumber], [StudentPhoto], [GuardianSignature] FROM [Students] WHERE ([sl] = @sl)">
                <SelectParameters>
                    <asp:QueryStringParameter Name="sl" QueryStringField="sl" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>

        <input type="button" id="print-button" value="Print" onclick="window.print();" />

    </form>
</body>
</html>
