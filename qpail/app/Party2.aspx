<%@ Page Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Party2.aspx.cs" Inherits="app_Party2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>



            <h3 class="page-title">
                <asp:Literal ID="ltrFrmName" runat="server" Text="Party Information Entry" />
            </h3>

            <div class="row">
                <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="ltrSubFrmName" runat="server" Text="Party Information Entry" />
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                <asp:Label ID="lblEid" runat="server" Text="Code No.#: " Visible="false"></asp:Label>
                                <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>


                                <div id="EditField" runat="server" class="control-group hidden">
                                    <b style="color: #E84940">
                                        <asp:Label ID="lblEname" runat="server" Text="Edit Info For"></asp:Label>
                                        <asp:DropDownList ID="ddName" runat="server" AppendDataBoundItems="True"
                                            AutoPostBack="True" DataSourceID="SqlDataSource2" Class=""
                                            DataTextField="Company" DataValueField="PartyID"
                                            OnSelectedIndexChanged="ddName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT PartyID, [Company] FROM [Party] WHERE (([ProjectID] = @ProjectID) AND ([Type] = @Type)) ORDER BY [Company]">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="ProjectID" SessionField="ProjectID" Type="Int32" />
                                                <asp:QueryStringParameter Name="Type" QueryStringField="type" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </b>
                                </div>

                                <div id="Div2" runat="server" class="control-group">
                                    <asp:Label ID="Label14" runat="server" Text="General Item Type :  "></asp:Label>
                                    <asp:DropDownList ID="ddCategory" runat="server" AutoPostBack="True"
                                        DataSourceID="SqlDataSource5" DataTextField="BrandName"
                                        DataValueField="BrandID" OnSelectedIndexChanged="ddCategory_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource5" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [BrandID], BrandName FROM [RefItems] ORDER BY [BrandName]"></asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="lblCompany" runat="server" Text="Company Name "></asp:Label>
                                    <asp:TextBox ID="txtCompany" runat="server"></asp:TextBox>
                                </div>

                                <%--For Manufacturer Only--%>
                                <%--<div class="control-group">
                        <asp:Label ID="Label9" runat="server" Text="Agent Name "></asp:Label>
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </div>--%>


                                <div id="div1" runat="server" class="control-group">
                                    <asp:Label ID="lblRefItems" runat="server" Text="Referrence Items"></asp:Label>
                                    <asp:TextBox ID="txtReferrenceItems" runat="server"></asp:TextBox>
                                </div>

                                <div id="divCountry" runat="server" class="control-group hidden">
                                    <asp:Label ID="Label11" runat="server" Text="Zone :  "></asp:Label>
                                    <asp:DropDownList ID="DropDownList1" runat="server"
                                        DataSourceID="SqlDataSource3" DataTextField="AreaName"
                                        DataValueField="AreaName">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [AreaName] FROM [Areas] ORDER BY [AreaName]"></asp:SqlDataSource>
                                </div>


                                <%--For Manufacturer Only--%>

                                <div id="ZoneField" runat="server" class="control-group hidden">
                                    <asp:Label ID="lblZone" runat="server" Text="Sales Zone :  "></asp:Label>
                                    <asp:DropDownList ID="ddZone" runat="server"
                                        DataSourceID="SqlDataSource3" DataTextField="AreaName"
                                        DataValueField="AreaName">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [AreaName] FROM [Areas] ORDER BY [AreaName]"></asp:SqlDataSource>
                                </div>


                                <div class="control-group">
                                    <asp:Label ID="lblReferrer" runat="server" Text="Referrer :  "></asp:Label>
                                    <asp:DropDownList ID="ddReferrerID" runat="server">
                                    </asp:DropDownList>
                                </div>


                                <div class="control-group">
                                    <asp:Label ID="lblPermanent" runat="server" Text="Address : "></asp:Label>
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="" TextMode="MultiLine" Height="60px" />
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label4" runat="server"
                                        Text="Land Phone : "></asp:Label>
                                    <asp:TextBox ID="txtLandPhone" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label13" runat="server" Text="Email :  "></asp:Label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label5" runat="server" Text="FAX :  "></asp:Label>
                                    <asp:TextBox ID="txtFax" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label6" runat="server" Text="Skype / IM :  "></asp:Label>
                                    <asp:TextBox ID="txtIM" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label7" runat="server" Text="Website :  "></asp:Label>
                                    <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>


                                <div class="control-group">
                                    <asp:Label ID="Label8" runat="server"
                                        Text="Contact Person : "></asp:Label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="lblCno" runat="server" Text="Mobile No. :  "></asp:Label>
                                    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789-," TargetControlID="txtMobile">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="control-group hidden">
                                    <asp:Label ID="Label12" runat="server" Text="Party Type :  "></asp:Label>

                                    <asp:DropDownList ID="ddPartyType" runat="server">
                                        <asp:ListItem Value="Customer">Customer</asp:ListItem>
                                        <asp:ListItem Value="Supplier">Supplier</asp:ListItem>
                                        <asp:ListItem Value="agents">Agents</asp:ListItem>
                                        <asp:ListItem Value="cnf">CNF</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div id="MatuDaysField" runat="server" class="control-group hidden">
                                    <asp:Label ID="Label3" runat="server" Text="Credit Matuirity Days:  "></asp:Label>
                                    <asp:TextBox ID="txtMatuirity" runat="server" Text="0"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtMatuirity">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div id="CreditLimitField" runat="server" class="control-group hidden">
                                    <asp:Label ID="Label2" runat="server" Text="Due/Credit Limit:  "></asp:Label>
                                    <asp:TextBox ID="txtCredit" runat="server" Text="0"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789-." TargetControlID="txtCredit">
                                    </asp:FilteredTextBoxExtender>
                                </div>

                                <div id="divOpBalance" runat="server" class="control-group hidden">
                                    <asp:Label ID="Label1" runat="server" Text="Openning Balance :  "></asp:Label>
                                    <asp:TextBox ID="txtBalance" runat="server" Text="0"></asp:TextBox>
                                </div>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                    runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789-." TargetControlID="txtBalance">
                                </asp:FilteredTextBoxExtender>


                                <%--Terms Panel--%>
                                <asp:Panel ID="TermsPanel" runat="server" Visible="False">

                                    <div class="control-group">
                                        <asp:Label ID="Label15" runat="server" Text="VAT Reg. No. : "></asp:Label>
                                        <asp:TextBox ID="txtVatRegNo" runat="server" CssClass="form-control" />
                                    </div>

                                    <div class="control-group hidden">
                                        <asp:Label ID="Label9" runat="server" Text="TDS Terms : "></asp:Label>
                                        <asp:TextBox ID="txtTDSTerms" runat="server" CssClass="form-control" placeholder="" TextMode="MultiLine" Height="60px" />
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-12 control-label">A/c Head TDS </label>
                                        <asp:DropDownList ID="ddAccHeadTDS" runat="server" AutoPostBack="True" AppendDataBoundItems="True" CssClass="form-control select2me" DataSourceID="SqlDataSource22" DataTextField="AccountsHeadName" DataValueField="AccountsHeadID">
                                            <asp:ListItem Value="0">---Select---</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource22" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="Select AccountsHeadID, (Select ControlAccountsName from ControlAccount where ControlAccountsID='010114')+' > '+AccountsHeadName as AccountsHeadName from [HeadSetup] Order by AccountsHeadID"></asp:SqlDataSource>
                                    </div>

                                    <div class="control-group hidden">
                                        <asp:Label ID="Label10" runat="server" Text="VDS Terms : "></asp:Label>
                                        <asp:TextBox ID="txtVDSTerms" runat="server" CssClass="form-control" placeholder="" TextMode="MultiLine" Height="60px" />
                                    </div>

                                </asp:Panel>


                                <div class="form-actions">
                                    <asp:Button ID="btnSave" runat="server" Text="Save Info" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />

                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" Visible="false" />
                                    <asp:ConfirmButtonExtender ID="cbe" runat="server" TargetControlID="btnDelete" ConfirmText="Are you sure to Delete?" />
                                    <%--<asp:ModalPopupExtender id="ModalPopupExtender1" runat="server" 
	                                            cancelcontrolid="btnCancel2" okcontrolid="btnOkay" 
	                                            targetcontrolid="btnDelete" popupcontrolid="Panel1" 
	                                            popupdraghandlecontrolid="PopupHeader" drag="true" 
	                                            backgroundcssclass="ModalPopupBG"></asp:ModalPopupExtender>
                        <asp:panel id="Panel1" style="display: none" runat="server">
	                        <div class="HellowWorldPopup">
                                        <div class="PopupHeader" id="PopupHeader">Header</div>
                                        <div class="PopupBody">
                                            <p>This is a simple modal dialog</p>
                                        </div>
                                        <div class="Controls">
                                            <input id="btnOkay" type="button" value="Done" />
                                            <input id="btnCancel2" type="button" value="Cancel" />
		                        </div>
                                </div>
                        </asp:panel>--%>
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

                                <div class="table-responsive">

                                    <asp:GridView ID="GridView1" runat="server" Width="250%" AllowSorting="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                        AutoGenerateColumns="False" DataSourceID="SqlDataSource1" ForeColor="Black" GridLines="Vertical">
                                        <Columns>
                                            <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                                            <asp:TemplateField ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="PartyID" SortExpression="PartyID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("PartyID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="RefItems" HeaderText="Category"
                                                SortExpression="Category" />
                                            <asp:BoundField DataField="Company" HeaderText="Company"
                                                SortExpression="Company" />
                                            <asp:BoundField DataField="Zone" HeaderText="Zone"
                                                SortExpression="Zone" Visible="false" />
                                            <asp:BoundField DataField="ReferrenceItems" HeaderText="Ref. Items"
                                                SortExpression="ReferrenceItems" />
                                            <asp:BoundField DataField="Address" HeaderText="Address"
                                                SortExpression="Address" />
                                            <asp:BoundField DataField="Phone" HeaderText="Phone"
                                                SortExpression="Phone" />
                                            <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" SortExpression="ContactPerson" />
                                            <asp:BoundField DataField="MobileNo" HeaderText="MobileNo" SortExpression="MobileNo" />

                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT (Select BrandName from RefItems where BrandID=Party.Category) as RefItems, PartyID, [Company], [Zone], [ReferrenceItems], [Address], [Phone], [ContactPerson], [MobileNo] FROM [Party] WHERE (([Type] = @Type)) order by Company"
                                        FilterExpression="RefItems LIKE '{0}%'">
                                        <SelectParameters>
                                            <asp:QueryStringParameter Name="Type" QueryStringField="type" Type="String" />
                                        </SelectParameters>
                                        <FilterParameters>
                                            <asp:ControlParameter ControlID="txtCat" Name="RefItems" PropertyName="Text" />
                                        </FilterParameters>
                                    </asp:SqlDataSource>
                                    <asp:TextBox runat="server" ID="txtCat" Visible="False"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>


