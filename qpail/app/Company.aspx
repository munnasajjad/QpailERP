<%@ Page Title="Company Information" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Company.aspx.cs" Inherits="app_Company" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>


    <h3 class="page-title">Company Information</h3>


    <div class="row">
        <div class="col-md-6">
            <div class="portlet box red">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Company Details
                    </div>
                </div>
                <div class="portlet-body form">

                    <div class="form-body">


                        <%--UID--%>
                        &nbsp;<asp:Label ID="lblMsg" CssClass="msg" runat="server"></asp:Label>



                        <div class="control-group hidden">

                            <asp:Label ID="lblEid" runat="server" Text="Company ID : " Visible="false"></asp:Label>
                            <%--<asp:LinkButton ID="LinkButton1"
                                runat="server" OnClick="LinkButton1_Click">Edit Company</asp:LinkButton>--%>

                            <asp:Label ID="lblErrLoad" runat="server" CssClass="msg"></asp:Label>
                            <asp:HiddenField runat="server" ID="CompanyIdHiddenField"/>
                        </div>

                        <div class="control-group">

                            <asp:Label ID="lblEname" runat="server" Text="Company Name : "></asp:Label>

                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                            <%--<asp:DropDownList ID="ddName" runat="server" AppendDataBoundItems="True"
                                AutoPostBack="True" DataSourceID="SqlDataSource2" Class=""
                                DataTextField="HEAD3" DataValueField="HEAD3"
                                OnSelectedIndexChanged="ddName_SelectedIndexChanged" Visible="False">
                                <asp:ListItem>---Select---</asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                SelectCommand="SELECT [CompanyName] FROM [Company] ORDER BY [CompanyName]"></asp:SqlDataSource>--%>


                        </div>
                        <div class="control-group">

                            <asp:Label ID="Label4" runat="server"
                                Text="Project Name : "></asp:Label>

                            <asp:TextBox ID="txtProjectName" runat="server"></asp:TextBox>
                        </div>

                        <div class="control-group">

                            <asp:Label ID="lblfather" runat="server"
                                Text="Speciality : "></asp:Label>

                            <asp:TextBox ID="txtSpeciality" runat="server"></asp:TextBox>
                        </div>
                        <%--<div class="control-group">

                            <asp:Label ID="Label3" runat="server" Text="Zone/ City :  "></asp:Label>

                            <asp:DropDownList ID="DropDownList1" runat="server"
                                DataSourceID="SqlDataSource3" DataTextField="AreaName"
                                DataValueField="AreaName">
                            </asp:DropDownList>
                            <%--<asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>  --%>
                            <%--<asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                SelectCommand="SELECT [AreaName] FROM [Areas] ORDER BY [AreaName]"></asp:SqlDataSource>
                        </div>--%>
                        <div class="control-group">

                            <asp:Label ID="lblPermanent"
                                runat="server" Text="Address : "></asp:Label>

                            <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Height="60px" Width="70%"></asp:TextBox>
                        </div>

                        <div class="control-group">
                            <asp:Label ID="Label13" runat="server" Text="Email :  "></asp:Label>
                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                        </div>

                        <div class="control-group">
                            <asp:Label ID="lblCno" runat="server" Text="Mobile No. :  "></asp:Label>

                            <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="+0123456789-," TargetControlID="txtMobile">
                            </asp:FilteredTextBoxExtender>
                        </div>

                        <%--<div class="control-group">
                            <asp:Label ID="Label5" runat="server" Text="FAX :  "></asp:Label>
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                        </div>

                        <div class="control-group">
                            <asp:Label ID="Label6" runat="server" Text="Skype / IM :  "></asp:Label>
                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                        </div>--%>

                        <div class="control-group">
                            <asp:Label ID="Label7" runat="server" Text="Authority :  "></asp:Label>
                            <asp:TextBox ID="txtAuthority" runat="server"></asp:TextBox>
                        </div>
                        
                        <div class="control-group">
                                        <label class="control-label">Upload Logo: </label>
                                        <div class="controls">
                                            <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" CssClass="form-control"/>
                                    <%--<iframe src="Docs/PO-Upload.aspx" width="320" height="20" scrolling="no" seamless="seamless"></iframe>--%>
                                        </div>
                                        </div>
                        <%--<div class="control-group">
                            <asp:Label ID="Label12" runat="server" Text="Company Type :  "></asp:Label>

                            <asp:DropDownList ID="txtCountry" runat="server">
                                <asp:ListItem Value="C">Customer</asp:ListItem>
                                <asp:ListItem Value="S">Supplier</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="control-group">

                            <asp:Label ID="Label2" runat="server" Text="Credit Limit:  "></asp:Label>

                            <asp:TextBox ID="txtCredit" runat="server" Text="0"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789-." TargetControlID="txtCredit">
                            </asp:FilteredTextBoxExtender>
                        </div>
                        <div class="control-group">

                            <asp:Label ID="Label1" runat="server" Text="Openning Balance :  "></asp:Label>

                            <asp:TextBox ID="txtBalance" runat="server" Text="0"></asp:TextBox>
                        </div>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789-." TargetControlID="txtBalance">
                        </asp:FilteredTextBoxExtender>--%>


                        <div class="form-actions">

                            <asp:Button ID="Button1" runat="server" Text="Save Company"
                                OnClick="Button1_Click" />
                            <asp:Button ID="btnEdit" runat="server" Text="Edit Company" OnClick="btnEdit_Click" />
                            <asp:Button ID="btnClear" CssClass="btn default" runat="server" Text="Cancel" OnClick="btnClear_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>



        <div class="col-md-6 ">
            <div class="portlet box green">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Saved Data
                    </div>
                    <div class="tools">
                        <a href="" class="collapse"></a>
                        <a href="#portlet-config" data-toggle="modal" class="config"></a>
                        <a href="" class="reload"></a>
                        <a href="" class="remove"></a>
                    </div>
                </div>
                <div class="portlet-body form">

                    <div class="form-body">


                        <asp:GridView ID="GridView1" runat="server" Width="100%" AllowSorting="True"
                            AutoGenerateColumns="False" DataSourceID="SqlDataSource1" ForeColor="Black" GridLines="Vertical" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged">
                            
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="20px" HeaderText="SL">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CompanyID" SortExpression="CompanyID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("CompanyID") %>' Visible="False"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:BoundField DataField="CompanyName" HeaderText="CompanyName"
                                    SortExpression="CompanyName" />
                                <asp:BoundField DataField="ProjectName" HeaderText="Project"
                                    SortExpression="ProjectName" />
                                <asp:BoundField DataField="CompanySpeciality" HeaderText="Speciality"
                                    SortExpression="CompanySpeciality" />
                                <asp:BoundField DataField="CompanyAddress" HeaderText="Address"
                                    SortExpression="CompanyAddress" />
                                <asp:BoundField DataField="AuthorityName" HeaderText="Authority"
                                    SortExpression="AuthorityName" />
                                <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                                                ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Select to Edit" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand="SELECT CompanyID, CompanyName, ProjectName, CompanySpeciality, CompanyAddress, AuthorityName FROM [Company] ORDER BY [CompanyName]"></asp:SqlDataSource>


                    </div>
                </div>
            </div>
        </div>

    </div>

</asp:Content>


