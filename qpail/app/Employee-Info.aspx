<%@ Page Title="Employee Info" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Employee-Info.aspx.cs" Inherits="Operator_Employee_Info" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        img#ctl00_BodyContent_Image1 {
    margin-right: 10px;
}
    </style>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {

            //$("input[type=text][id*=txtEid]").attr("disabled", true);

            $("input[type=checkbox][id*=chkCode]").click(function () {
                if (this.checked)
                    $("input[type=text][id*=txtEid]").attr("disabled", true);
                else
                    $("input[type=text][id*=txtEid]").attr("disabled", false);
                $('#<%=txtEid.ClientID%>').focus();
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>


            <h3 class="page-title">Employee Information</h3>

            <div class="row">
                <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Employee Setup
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">



                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                <asp:Label ID="lblEid" runat="server" Visible="false"></asp:Label>

                                <div class="portlet-body form">
                                    <div class="form-horizontal">

                                        <div class="control-group">
                                            <asp:Label ID="Label1" runat="server" AssociatedControlID="ddDepartment">Department Name : </asp:Label>
                                            <asp:DropDownList ID="ddDepartment" runat="server" CssClass="span6 chosen"
                                                DataSourceID="SqlDataSource7" DataTextField="DepartmentName" DataValueField="Departmentid"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddDepartment_SelectedIndexChanged">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT Departmentid,[DepartmentName] FROM [Departments] ORDER BY [DepartmentName]"></asp:SqlDataSource>

                                        </div>


                                        <div class="control-group">
                                            <asp:Label ID="Label3" runat="server" AssociatedControlID="ddSection">Section Name : </asp:Label>
                                            <asp:DropDownList ID="ddSection" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddSection_OnSelectedIndexChanged"
                                                CssClass="span6 chosen" DataSourceID="SqlDataSource3" DataTextField="SName" DataValueField="SID">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [SID], [SName] FROM [Sections] WHERE ([DepartmentID] = @DepartmentID) AND IsPrdSection=0 ORDER BY [SName]">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddDepartment" Name="DepartmentID" PropertyName="SelectedValue" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>

                                        <div class="control-group">

                                            <asp:Label ID="lblDes" runat="server">Designation: </asp:Label>
                                            <asp:DropDownList ID="ddDesignation" runat="server"
                                                CssClass="span6 chosen" DataSourceID="SqlDataSource1" DataTextField="DesignationName"
                                                DataValueField="Designationid">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT Designationid, [DesignationName] FROM [Designations] ORDER BY [DesignationName]"></asp:SqlDataSource>

                                        </div>


                                        <legend>Employee Profile</legend>

                                        <div class="control-group">
                                            <asp:Label ID="lblEname" runat="server" Text="Employee Name: "></asp:Label>
                                            
                                            <asp:TextBox ID="txtEname" runat="server"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtEname" runat="server" ErrorMessage="Employee Name is Required" BackColor="YellowGreen"></asp:RequiredFieldValidator>   --%>
                                            <asp:Button ID="Button2" runat="server" Text="Search" Visible="false" />
                                            <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank">
                                            <asp:Image ID="Image1" runat="server" ImageAlign="Right" Width="120px" Visible="False" /></asp:HyperLink>
                                            <asp:HyperLink ID="hypCV" runat="server" Target="_blank"></asp:HyperLink>
                                        </div>

                                        <div class="control-group">
                                            <asp:Label ID="lblPhoto" runat="server" Text="Photograph: "></asp:Label>
                                            <asp:FileUpload ID="FileUpload2" runat="server" ClientIDMode="Static" CssClass="form-control" />
                                            <%--<asp:AsyncFileUpload
                                                ID="AsyncFileUpload1"
                                                runat="server" ClientIDMode="Static"
                                                OnUploadedComplete="AsyncFileUpload1_UploadComplete" />
                                            <asp:Image ID="ImgPhoto" runat="server" ImageAlign="Right" Width="100px" />--%>
                                        </div>

                                        <div class="control-group">
                                            <asp:Label ID="lblfather" runat="server" Text="Father's Name: "></asp:Label>
                                            <asp:TextBox ID="txtFather" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="control-group">
                                            <asp:Label ID="lblMother" runat="server" Text="Mother's Name: "></asp:Label>
                                            <asp:TextBox ID="txtMother" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="control-group">
                                            <asp:Label ID="Label4" runat="server" Text="National ID Card#: "></asp:Label>
                                            <asp:TextBox ID="txtNID" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            <label>Date of Birth: </label>
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="DOB" />
                                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy" Enabled="True" TargetControlID="txtDate"></asp:CalendarExtender>
                                        </div>

                                        <div class="control-group">
                                            <asp:Label ID="Label12" runat="server" Text="Marital Status: "></asp:Label>
                                            <asp:DropDownList ID="ddType" runat="server">
                                                <asp:ListItem>Unmarried</asp:ListItem>
                                                <asp:ListItem>Married</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="control-group">
                                            <asp:Label ID="Label13" runat="server" Text="Religion: "></asp:Label>
                                            <asp:DropDownList ID="DropDownList1" runat="server">
                                                <asp:ListItem>Islam</asp:ListItem>
                                                <asp:ListItem>Hindu</asp:ListItem>
                                                <asp:ListItem>Buddhist</asp:ListItem>
                                                <asp:ListItem>Christian</asp:ListItem>
                                                <asp:ListItem>Others</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="control-group">
                                            <asp:Label ID="lblCaddress" runat="server" Text="Contact Address: "></asp:Label>
                                            <asp:TextBox ID="txtCaddress" runat="server" TextMode="MultiLine" Width="344px" Height="95px" Rows="2" cols="20"></asp:TextBox>
                                        </div>
                                        <div class="control-group">
                                            <asp:Label ID="lblPermanent" runat="server" Text="Permanent Address: "></asp:Label>
                                            <asp:TextBox ID="txtPermanent" runat="server" TextMode="MultiLine" Width="344px" Height="95px" Rows="2" cols="20"></asp:TextBox>
                                        </div>
                                        <div class="control-group">
                                            <asp:Label ID="lblCno5" runat="server" Text="Nationality: "></asp:Label>
                                            <asp:TextBox ID="txtNational" runat="server" Text="Bangladeshi"></asp:TextBox>
                                        </div>

                                        <div class="control-group">
                                            <asp:Label ID="lblCno6" runat="server" Text="Mobile Number: "></asp:Label>
                                            <asp:TextBox ID="txtCno" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="control-group">
                                            <asp:Label ID="Label7" runat="server" Text="Email: "></asp:Label>
                                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="control-group">
                                            <asp:Label ID="lblAge" runat="server" Text="Age : "></asp:Label>
                                            <asp:TextBox ID="txtAge" runat="server"></asp:TextBox>
                                        </div>

                                        <%--<legend>Official Information</legend>--%>

                                        <div class="control-group">
                                            <asp:Label ID="lblEid2" runat="server" Text="Employee Code: "></asp:Label>
                                            <asp:TextBox ID="txtEid" runat="server" Width="150px"></asp:TextBox>
                                            <asp:CheckBox ID="chkCode" runat="server" Text="Auto Generated" Checked="False" />
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtEid">
                                            </asp:FilteredTextBoxExtender>
                                        </div>

                                        <div class="control-group">
                                            <asp:Label ID="Label9" runat="server" Text="Card Number: "></asp:Label>
                                            <asp:TextBox ID="txtCardNo" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtCardNo"></asp:FilteredTextBoxExtender>
                                        </div>


                                        <div class="control-group">
                                            <asp:Label ID="lblJoinDate" runat="server">Joining Date: </asp:Label>
                                            <asp:TextBox ID="txtJoinDate" runat="server" ToolTip="Pick or Type in MM/DD/YYYY"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtJoinDate">
                                            </asp:CalendarExtender>
                                        </div>

                                        <div class="control-group">
                                            <asp:Label ID="lblExp" runat="server" Text="Experience (Years) : "></asp:Label>
                                            <asp:TextBox ID="txtExp" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtSalary">
                                            </asp:FilteredTextBoxExtender>
                                        </div>


                                        <div class="control-group hidden">
                                            <asp:Label ID="Label5" runat="server" Text="Salary Basis: "></asp:Label>
                                            <asp:DropDownList ID="ddBasis" runat="server" DataSourceID="SqlDataSource4" DataTextField="GroupName" DataValueField="GroupSrNo">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [GroupName], [GroupSrNo] FROM [EmpTypes] ORDER BY [GroupSrNo]"></asp:SqlDataSource>
                                        </div>

                                        <div class="control-group">
                                            <asp:Label ID="Label2" runat="server">Gross Salary: </asp:Label>
                                            <asp:TextBox ID="txtSalary" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtSalary_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtSalary">
                                            </asp:FilteredTextBoxExtender>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <asp:Label ID="Label8" runat="server" Text="Education Summery : "></asp:Label>
                                        <asp:TextBox ID="txtEducation" runat="server" TextMode="MultiLine" Width="70%" Height="95px"></asp:TextBox>
                                    </div>

                                    <div class="control-group">
                                        <asp:Label ID="Label11" runat="server" Text="Skills Summery : "></asp:Label>
                                        <asp:TextBox ID="txtSkills" runat="server" TextMode="MultiLine" Width="70%" Height="95px"></asp:TextBox>
                                    </div>

                                    <div class="control-group">
                                        <asp:Label ID="Label6" runat="server" Text="Remarks : "></asp:Label>
                                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Width="70%" Height="95px"></asp:TextBox>
                                    </div>

                                    <div class="control-group">
                                        <asp:Label ID="Label10" runat="server" Text="Upload CV: "></asp:Label>
                                        <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" CssClass="form-control" />
                                    </div>

                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn blue" />
                                        <asp:Button ID="Button1" runat="server" OnClick="Button1_OnClick" CssClass="btn orange"
                                            Text="Reset" />


                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>

                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Saved Employees
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

                                <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-hover"
                                    AutoGenerateColumns="False" DataSourceID="SqlDataSource2" AllowSorting="True"
                                    OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                                    <Columns>
                                        
                                            
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>. 
                                    </ItemTemplate>
                                </asp:TemplateField>

                                        <asp:TemplateField HeaderText="EmployeeInfoID" InsertVisible="False" SortExpression="EmployeeInfoID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("EmployeeInfoID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="EName" HeaderText="Employee Name" SortExpression="EName" />
                                        <asp:BoundField DataField="EmpSerial" HeaderText="Serial"  />
                                        <asp:BoundField DataField="CardNo" HeaderText="Card Number" SortExpression="ContactNumber" />
                                        <asp:BoundField DataField="Section" HeaderText="Section"  />
                                        <asp:BoundField DataField="JoiningDate" HeaderText="Joining Date" SortExpression="JoiningDate" DataFormatString="{0:d}" visible="False" />
                                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT EmpSerial, CardNo, EmployeeInfoID, [EName], [ContactNumber],(Select SName from Sections where SID=EmployeeInfo.[SectionID]) As Section, [Salary], [JoiningDate] FROM [EmployeeInfo] where SectionID=@DepartmentID ORDER BY SectionID, [CardNo], EmpSerial">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddSection" Name="DepartmentID" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                    </div>
                </div>

            </div>






        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
