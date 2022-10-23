<%@ Page Title="User Permissions" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Settings-User-Permission.aspx.cs" Inherits="app_Settings_User_Permission" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .panel input[type=checkbox], .panel input[type=radio] {
            padding: 0;
            width: 30px !important;
            border: none !important;
            height: 22px !important;
            box-shadow: none !important;
            margin-left: 12px;
        }

        .panel label {
            padding: 11px 0;
            vertical-align: super;
        }

        .portlet-body table tr:nth-child(odd) {
            background: #DDEBF7 !important;
        }

        td {
            vertical-align: middle !important;
        }

        table {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">Menu Permissions</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="lrlSavedBox" runat="server">Select User</asp:Literal>
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

                                <asp:Label ID="lblid" runat="server" Visible="False"></asp:Label>

                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="LID"
                                    OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" >

                                    <Columns>
                                           <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" />

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="sl" SortExpression="sl" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("LID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="LoginUserName" HeaderText="Login ID" SortExpression="LoginUserName" ReadOnly="True" />
                                        <asp:BoundField DataField="EmployeeInfoID" HeaderText="Employee" SortExpression="EmployeeInfoID" />
                                        <asp:BoundField DataField="UserLevel" HeaderText="Permission Level" SortExpression="UserLevel" />
                                        <asp:BoundField DataField="UserLevel" HeaderText="Permission Level" SortExpression="UserLevel" />


                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT LID, [LoginUserName],
                                    (Select EName from EmployeeInfo where EmployeeInfoID= Logins.[EmployeeInfoID]) as EmployeeInfoID,
                                    (Select LevelName from userlevel where levelid= Logins.[UserLevel]) as UserLevel
                                    FROM [Logins] where LoginUserName<>'rony' ORDER BY [LoginUserName]"
                                    DeleteCommand="delete Logins where lid=0"></asp:SqlDataSource>

                                <asp:Label ID="txtCurrentPosition" runat="server" Text="" Visible="False"></asp:Label>

                            </div>
                        </div>
                    </div>
                </div>


                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Set menu permission
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>

                                    <div class="control-group span12 col-md-12">
                                        <label class="control-label">Login Id: </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtUserX" CssClass="span12 m-wrap" runat="server" ReadOnly="True"></asp:TextBox>

                                        </div>
                                    </div>

                                    <div class="control-group  span12 col-md-12">
                                        <label class="control-label">Employee Name: </label>
                                        <div class="controls">

                                            <asp:DropDownList ID="ddEmployeX" runat="server"
                                                CssClass="form-control"
                                                DataSourceID="SqlDataSource40x" DataTextField="EName"
                                                DataValueField="EmployeeInfoID">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource40x" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [EName], EmployeeInfoID FROM [EmployeeInfo] where DepartmentID=@DepartmentID ORDER BY [EName]">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddCounterX" Name="DepartmentID" PropertyName="SelectedValue" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>

                                        </div>
                                    </div>

                                    <div class="control-group span12 col-md-12">
                                        <label class="control-label">Department: </label>
                                        <div class="controls">

                                            <asp:DropDownList ID="ddCounterX" runat="server"
                                                CssClass="form-control"
                                                DataSourceID="SqlDataSource7x" DataTextField="DepartmentName"
                                                DataValueField="Departmentid">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource7x" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT Departmentid, DepartmentName FROM [Departments] ORDER BY [DepartmentName]"></asp:SqlDataSource>


                                        </div>
                                    </div>

                                    <div class="control-group span12 col-md-12">
                                        <label class="control-label">Permission Level: </label>
                                        <div class="controls">

                                            <asp:DropDownList ID="ddLevelX" runat="server" CssClass="form-control">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource7p" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT LevelID, LevelName FROM [UserLevel] WHERE LevelID<@LevelID ORDER BY [LevelID]"></asp:SqlDataSource>

                                        </div>
                                    </div>
                                    <legend> <asp:CheckBox ID="CheckBox1" runat="server" Text="SALES" AutoPostBack="True" OnCheckedChanged="CheckBox1_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel1">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox2" runat="server" Text="Setups" />
                                                <asp:CheckBox ID="CheckBox3" runat="server" Text="POS" />
                                               <asp:CheckBox ID="CheckBox4" runat="server" Text="Collections" />
                                               <asp:CheckBox ID="salesBOQCheckBox" runat="server" Text="SalesBOQRepots" />
                                                <asp:CheckBox ID="CheckBox5" runat="server" Text="Reports" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <legend> <asp:CheckBox ID="CheckBox6" runat="server" Text="PURCHASE"  AutoPostBack="True" OnCheckedChanged="CheckBox6_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel2">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox7" runat="server" Text="Setups" />
                                                <asp:CheckBox ID="CheckBox8" runat="server" Text="Local Purchase" />
                                               <asp:CheckBox ID="CheckBox9" runat="server" Text="LC" />
                                                <asp:CheckBox ID="CheckBox10" runat="server" Text="Purchase Reports" />
                                                <asp:CheckBox ID="CheckBox10m" runat="server" Text="LC Reports" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <legend> <asp:CheckBox ID="CheckBox11" runat="server" Text="INVENTORY"  AutoPostBack="True" OnCheckedChanged="CheckBox11_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel3">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox12" runat="server" Text="Product Setup" />
                                                <asp:CheckBox ID="CheckBox13" runat="server" Text="Warehouses" />
                                               <asp:CheckBox ID="CheckBox14" runat="server" Text="Adjustment" />
                                                <asp:CheckBox ID="CheckBox15" runat="server" Text="Transfer" />
                                                <asp:CheckBox ID="CheckBox135" runat="server" Text="Reports" />
                                            </div>
                                        </div>
                                </asp:Panel>
                                <legend> <asp:CheckBox ID="ChkInventoryNew" runat="server" Text="INVENTORY NEW"  AutoPostBack="True" OnCheckedChanged="ChkInventoryNew_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel10">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="ChkStockTransaction" runat="server" Text="Stock Transaction" />
                                                <asp:CheckBox ID="ChkStockHeadLedger" runat="server" Text="Stock Head Ledger" />
                                               
                                            </div>
                                        </div>
                                </asp:Panel>
                                    <legend> <asp:CheckBox ID="CheckBox16" runat="server" Text="PRODUCTION"  AutoPostBack="True" OnCheckedChanged="CheckBox16_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel4">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox17" runat="server" Text="Setups" />
                                                <asp:CheckBox ID="CheckBox18" runat="server" Text="Production" />
                                                <asp:CheckBox ID="CheckBox20" runat="server" Text="Reports" />
                                            </div>
                                        </div>
                                </asp:Panel>
                                    <legend> <asp:CheckBox ID="CheckBox21" runat="server" Text="ACCOUNTS"  AutoPostBack="True" OnCheckedChanged="CheckBox21_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel5">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox22" runat="server" Text="Setups" />
                                               <asp:CheckBox ID="CheckBox24" runat="server" Text="Voucher" />
                                                <asp:CheckBox ID="CheckBox25" runat="server" Text="Reports" />
                                            </div>
                                        </div>
                                </asp:Panel>
                                    <legend> <asp:CheckBox ID="CheckBox26" runat="server" Text="PAYROLL" AutoPostBack="True" OnCheckedChanged="CheckBox26_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel7">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox27" runat="server" Text="Setups" />
                                                <asp:CheckBox ID="CheckBox28" runat="server" Text="Data Entry" />
                                                <asp:CheckBox ID="CheckBox30" runat="server" Text="Reports" />
                                            </div>
                                        </div>
                                </asp:Panel>
                                    <legend> <asp:CheckBox ID="CheckBox31" runat="server" Text="CRM"  AutoPostBack="True" OnCheckedChanged="CheckBox31_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel6">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox34" runat="server" Text="Document Library" />
                                               <asp:CheckBox ID="CheckBox32" runat="server" Text="Setups" />
                                                <asp:CheckBox ID="CheckBox33" runat="server" Text="Data Entry" />
                                            </div>
                                        </div>
                                </asp:Panel>
                                    <legend> <asp:CheckBox ID="CheckBox36" runat="server" Text="FACTS MODULE"  AutoPostBack="True" OnCheckedChanged="CheckBox36_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel8">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox37" runat="server" Text="Declarations" />
                                                <asp:CheckBox ID="CheckBox38" runat="server" Text="Consumptions" />
                                               <asp:CheckBox ID="CheckBox39" runat="server" Text="Measurements" />
                                            </div>
                                        </div>
                                </asp:Panel>
                                    <legend> <asp:CheckBox ID="CheckBox41" runat="server" Text="ADMIN"  AutoPostBack="True" OnCheckedChanged="CheckBox41_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel9">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox42" runat="server" Text="Setups" />
                                                <asp:CheckBox ID="CheckBox43" runat="server" Text="Notice & Messages" />
                                               <asp:CheckBox ID="CheckBox44" runat="server" Text="Users & Security" />
                                                <asp:CheckBox ID="CheckBox45" runat="server" Text="Admin Reports" />
                                                <asp:CheckBox ID="CheckBox45d" runat="server" Text="Maintenance" />
                                            </div>
                                        </div>
                                </asp:Panel>
                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Update" OnClick="btnSave_OnClick" />
                                        <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" OnClick="btnClear_OnClick" />
                                    </div>



                            </div>
                        </div>
                    </div>
                </div>



            </div>
            </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

