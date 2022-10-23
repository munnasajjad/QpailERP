<%@ Page Title="Branch Setup" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="BankBranch.aspx.cs" Inherits="app_BankBranch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>

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


            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">Bank Branch 
                    </h3>
                </div>
            </div>
            <div class="row">


                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Bank Branch Setup
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

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                <asp:HiddenField runat="server" ID="BranchIdHField"/>
                                

                                <div class="form-group">
                                    <label class="control-label">Bank Name :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddBank" runat="server" DataSourceID="SqlDataSource6" DataTextField="BankName" DataValueField="BankId" CssClass="form-control select2me" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [BankId], [BankName] FROM [Banks] WHERE ([Type] = @Type) ORDER BY [BankName]">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="bank" Name="Type" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label>Branch Name: </label>
                                    <asp:TextBox ID="txtBranch" runat="server" CssClass="form-control" EnableViewState="true" placeholder="Branch Name" />
                                </div>

                                <div class="form-group">
                                    <label>Branch Address : </label>
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Branch Address" />
                                </div>
                                <div class="form-group">
                                    <label>Phone : </label>
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Phone" />
                                    <asp:FilteredTextBoxExtender ID="txtOpBalance_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789+" TargetControlID="txtPhone">
                                        </asp:FilteredTextBoxExtender>
                                </div>

                                <div class="form-actions">
                                    <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click1" />
                                    <asp:Button ID="btnClear" CssClass="btn default" runat="server" Text="Cancel" OnClick="btnClear_Click1" />
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


                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="BranchName" OnRowDeleting="GridView1_OnRowDeleting">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="BranchID" SortExpression="BranchID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("BranchID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bank Name" SortExpression="BankName" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("BankName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BranchName" SortExpression="BranchName">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("BranchName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Branch Address" SortExpression="BranchAddress">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1x" runat="server" Text='<%# Bind("BranchAddress") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Phone" SortExpression="Phone">
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Phone") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">Bank branch will be deleted permanently!</b><br />
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
                                    SelectCommand="SELECT bb.BranchID , Banks.BankName, bb.BranchName, bb.BranchAddress, bb.Phone, bb.EntryBy, bb.EntryDate FROM BankBranch AS bb INNER JOIN Banks ON bb.BankID = Banks.BankId WHERE (Banks.BankId=@BankId)"
                                    DeleteCommand="Delete BankBranch where BranchID='0'">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddBank" Name="BankId" PropertyName="SelectedValue"/>
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
