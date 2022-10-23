<%@ Page Title="Sales Representatives" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Sales-Persons.aspx.cs" Inherits="app_Sales_Persons" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">


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



            <h3 class="page-title">
                <asp:Literal ID="ltrFrmName" runat="server" Text="Sales Representatives" /></h3>


            <div class="row">
                <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="ltrSubFrmName" runat="server" Text="Sales Representative Info" />
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                                <asp:Literal ID="lblEid" runat="server" Text="Sales Person ID.#: " Visible="false"></asp:Literal>

                                <asp:Literal ID="lblErrLoad" runat="server"></asp:Literal>

                                <div class="control-group">
                                    <asp:Label ID="lblEname" runat="server" Text="Name: "></asp:Label>
                                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                    <asp:DropDownList ID="ddName" runat="server" AppendDataBoundItems="True"
                                        AutoPostBack="True" DataSourceID="SqlDataSource2" Class=""
                                        DataTextField="VarSupplierName" DataValueField="VarSupplierName"
                                        OnSelectedIndexChanged="ddName_SelectedIndexChanged" Visible="False">
                                        <asp:ListItem>---Select---</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [VarSupplierName] FROM [Party] ORDER BY [VarSupplierName]"></asp:SqlDataSource>

                                </div>
                                <div class="control-group">
                                    <asp:Label ID="lblfather" runat="server" Text="Address : "></asp:Label>
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" Height="60px" TextMode="MultiLine"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label4" runat="server" Text="Phone No : "></asp:Label>
                                    <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label5" runat="server" Text="Mobile No : "></asp:Label>
                                    <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label6" runat="server" Text="Email : "></asp:Label>
                                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                </div>

                                <div id="DueLimitField" runat="server" class="control-group">
                                    <asp:Label ID="Label7" runat="server" Text="Due/Credit Limit : "></asp:Label>
                                    <asp:TextBox ID="txtCredit" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789-." TargetControlID="txtCredit">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div id="DesiField" runat="server" class="control-group">
                                    <asp:Label ID="Label1" runat="server" Text="Remark :  "></asp:Label>
                                    <asp:TextBox ID="txtRemark" runat="server"></asp:TextBox>
                                    <%--<asp:DropDownList ID="ddDesignation" runat="server"
                                DataSourceID="SqlDataSource4" DataTextField="DesignationName"
                                DataValueField="DesignationName">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                SelectCommand="SELECT [DesignationName] FROM [Designations] ORDER BY [DesignationName]"></asp:SqlDataSource>--%>
                                </div>

                                <div class="control-group hidden">
                                    <asp:Label ID="Label3" runat="server" Text="Area/Zone :  "></asp:Label>
                                    <asp:DropDownList ID="ddZone" runat="server"
                                        DataSourceID="SqlDataSource3x" DataTextField="AreaName"
                                        DataValueField="AreaName">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3x" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [AreaName] FROM [Areas] ORDER BY [AreaName]"></asp:SqlDataSource>
                                </div>


                                <div class="form-actions">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
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
                                <asp:Label ID="lblEditId" runat="server" ></asp:Label>

                                <asp:GridView ID="GridView1" runat="server" Width="100%" AllowSorting="True" DataKeyNames="ReferrersID" OnRowDeleting="GridView1_OnRowDeleting"
                                    AutoGenerateColumns="False" DataSourceID="SqlDataSource1" ForeColor="Black" GridLines="Vertical" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GradeID" SortExpression="GradeID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("ReferrersID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        

                                        <asp:BoundField DataField="ReferrerName" HeaderText="Name" SortExpression="ReferrerName" />
                                        <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                                        <asp:BoundField DataField="PhoneNo" HeaderText="Phone" SortExpression="PhoneNo" />
                                        <asp:BoundField DataField="MobileNo" HeaderText="Mobile" SortExpression="MobileNo" />
                                        <asp:BoundField DataField="Zone" HeaderText="Zone" SortExpression="Zone" Visible="false" />
                                        <asp:BoundField DataField="CreditLimit" HeaderText="Due Limit"
                                            SortExpression="CreditLimit" Visible="false" />


                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Select to Edit" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">Item will be deleted permanently!</b><br />
                                                    <br />
                                                    Are you sure to delete the item from list?
                                                            <br />
                                                    <br />
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
                                    SelectCommand="SELECT ReferrersID,[ReferrerName], [Address], [PhoneNo], [MobileNo], [CreditLimit], [Zone] FROM [Referrers] WHERE (([projectID] = @projectID) AND ([Type] = @Type)) ORDER BY [ReferrerName]"
                                    DeleteCommand="delete Referrers where ReferrersID=0" >
                                    <SelectParameters>
                                        <asp:SessionParameter Name="projectID" SessionField="ProjectID" Type="Int32" />
                                        <asp:QueryStringParameter DefaultValue="SR" Name="Type" QueryStringField="type" Type="String" />
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


