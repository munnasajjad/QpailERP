<%@ Page Title="Menu Structure" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="MenuStructure.aspx.cs" Inherits="app_MenuStructure" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>


            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">Menu Structure</h3>
                </div>
            </div>

            <div class="row">

                <div class="col-md-5">
                    <div class="portlet box">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Menu Structure
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                <asp:Label ID="lblID" runat="server" Visible="false"></asp:Label>

                                <div class="control-group">
                                    <label class="field_title">Form Group :</label>
                                    <asp:DropDownList ID="ddFormGroup" runat="server" DataSourceID="SqlDataSource3" AutoPostBack="True" OnSelectedIndexChanged="ddFormGroup_OnSelectedIndexChanged" DataTextField="MenuGroup" DataValueField="MenuGroup">
                                       </asp:DropDownList>
                                     <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT DISTINCT [MenuGroup] FROM [MenuSubGroup] WHERE ([IsBlocked] = @IsBlocked) ">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="1" Name="IsBlocked" Type="Int32" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div class="control-group">
                                    <label class="field_title">Form Sub-Group :</label>
                                    <asp:DropDownList ID="ddSubGroup" runat="server"
                                        DataSourceID="SqlDataSource2" DataTextField="MenuSubGroup" DataValueField="sl" AutoPostBack="True" OnSelectedIndexChanged="ddSubGroup_OnSelectedIndexChanged">
                                        
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [MenuSubGroup] FROM [MenuSubGroup] WHERE ([IsBlocked] =1) AND [MenuGroup]=@MenuGroup">
                                        <SelectParameters>
                                            <asp:ControlParameter Name="MenuGroup" ControlID="ddFormGroup" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>
                                
                                <div class="control-group">
                                    <label>Form Name (Title)</label>
                                        <asp:TextBox ID="txtName" runat="server" ></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <label>Page Name (.aspx)</label>
                                        <asp:TextBox ID="txtSubject" runat="server" ></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <label>Menu Control ID</label>
                                    <asp:TextBox ID="txtMsgBody2" runat="server"  ></asp:TextBox>
                                </div>
                                <div class="control-group">
                                    <label>Priority</label>
                                    <asp:TextBox ID="txtPriority" runat="server" Width="35%" Text="1" ReadOnly="True"></asp:TextBox>
                                    <asp:CheckBox runat="server" ID="chkActive" Text="Active" Checked="True"/>
                                </div>
                                <div class="control-group hidden">
                                    <asp:Label ID="lblPhoto" runat="server" Text="Attachment: "></asp:Label>
                                    <asp:FileUpload ID="FileUpload2" runat="server" ClientIDMode="Static" CssClass="form-control" Width="45%" />
                                    <asp:Image ID="imgPhoto" runat="server" Width="60px" />
                                </div>
                                
                                <div class="form-actions">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" class="btn blue" OnClick="btnSave_Click" />
                                        <asp:Button ID="Button1" runat="server" Text="Clear Form" class="btn blue" />

                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="col-md-7">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box green ">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                List of Menu Items
                            </div>
                            <div class="tools">
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal" role="form">
                                <div class="table-responsive">
                                    
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="SL"
                                    OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">

                                    <Columns>

                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="sl" SortExpression="sl" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SL") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="MenuGroup" HeaderText="Menu Group" SortExpression="LoginUserName" ReadOnly="True" />
                                        <asp:BoundField DataField="MenuSubGroup" HeaderText="Menu SubGroup" SortExpression="EmployeeInfoID" />
                                        <asp:BoundField DataField="FormName" HeaderText="Form Name" SortExpression="UserLevel" />
                                        
                                        <asp:BoundField DataField="PageName" HeaderText="Page Name" SortExpression="LoginUserName" ReadOnly="True" />
                                        <asp:BoundField DataField="HTMLControlID" HeaderText="HTML Control ID" SortExpression="EmployeeInfoID" />
                                        <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="EmployeeInfoID" />
                                        <asp:BoundField DataField="IsBlocked" HeaderText="Active" SortExpression="UserLevel" />

                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.gif" Text="Delete" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">This entry will be deleted permanently!</b><br />
                                                    Are you sure you want to delete this ?
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
                                    SelectCommand="SELECT SL, MenuGroup, (Select MenuSubGroup from MenuSubGroup where sl=MenuStructure.MenuSubGroup) as MenuSubGroup,  FormName, PageName, HTMLControlID, Priority, IsBlocked FROM [MenuStructure] where  MenuGroup=@MenuGroup AND MenuSubGroup=@MenuSubGroup ORDER BY [Priority]"
                                    DeleteCommand="delete Logins where lid=0">
                                    <SelectParameters>
                                        <asp:ControlParameter Name="MenuGroup" ControlID="ddFormGroup" PropertyName="SelectedValue" />
                                        <asp:ControlParameter Name="MenuSubGroup" ControlID="ddSubGroup" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                    
                                    <asp:Label ID="lblUser" runat="server" Text="" Visible="False"></asp:Label>
                                    <asp:Label ID="lblProjectID" runat="server" Visible="False"></asp:Label>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>




</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="server">
</asp:Content>



