<%@ Page Title="Poly Dimension" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="PolyDimension.aspx.cs" Inherits="app_PolyDimension246" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>--%>
    <script type="text/javascript" language="javascript">
        Sys.Application.add_load(callJquery);
    </script>

    <div class="col-lg-6">
        <section class="panel">

            <fieldset>
                <legend>Poly Dimension</legend>
                <div border="0" class="membersinfo tdfirstright bg-green" width="100%">
                    <div>
                        <div align="center" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                            <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                        </div>
                    </div>

                    <div class="form-group">
                        <label>Company: </label>
                        <asp:DropDownList ID="ddCompany" runat="server" CssClass="form-control select2me" DataSourceID="SqlDataSource1" DataTextField="Company" DataValueField="PartyID" AutoPostBack="true" OnSelectedIndexChanged="ddCompany_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [PartyID], [Company] FROM [Party] ORDER BY Company ASC"></asp:SqlDataSource>
                    </div>

                    <div class="control-group">
                        <label>Pack Size:</label>
                        <asp:DropDownList ID="ddPackSize" runat="server"
                            DataSourceID="SqlDataSource2" DataTextField="PackSize" DataValueField="Id">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand="SELECT Id, PackSize FROM PolyFinishedGoodsPackSize WHERE CompanyID=@CompanyID ORDER BY PackSize">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddCompany" Name="CompanyID" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>

                    <div class="form-group">
                        <label>Dimension : </label>
                        <asp:TextBox ID="txtDimensionName" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDimensionName" ControlToValidate="txtDimensionName" Display="Dynamic" SetFocusOnError="True" runat="server" ErrorMessage="Enter Dimension Name field " Style="margin-left: 30%"></asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <label>Standard Weight : </label>
                        <asp:TextBox ID="txtStandardWeight" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="form-group hidden">
                        <label>ProjectId: </label>
                        <asp:TextBox ID="txtProjectID" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="form-group hidden">
                        <label>DisplaySL: </label>
                        <asp:TextBox ID="txtDisplaySl" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="ftbDisplaySl" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtDisplaySl" />
                    </div>

                    <div style="background: none">
                        <div style="padding-left: 70%">
                            <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" Text="Save" OnClick="btnSave_OnClick" />
                            <asp:Button ID="btnClear" type="reset" CssClass="btn btn-s-md btn-white" runat="server" Text="Cancel" OnClick="btnClear_OnClick" />
                        </div>
                    </div>
                </div>
            </fieldset>
        </section>
    </div>

    <div class="col-lg-6">
        <section class="panel">
            <fieldset>
                <legend>Saved Data</legend>
                <div class="table-responsive">
                    <asp:GridView Width="140%" ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                        BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                        GridLines="Vertical" DataKeyNames="Id" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
                        <Columns>
                            <asp:TemplateField HeaderText="#Sl" ItemStyle-Width="20px">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>.
                                            <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("Id") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Company" HeaderText="Company" SortExpression="Company" />
                            <asp:BoundField DataField="PackSize" HeaderText="PackSize" SortExpression="PackSize" />
                            <asp:BoundField DataField="DimensionName" HeaderText="Dimension Name" SortExpression="DimensionName" />
                            <asp:BoundField DataField="StandardWeight" HeaderText="Standard Weight" SortExpression="StandardWeight" />
                            <asp:BoundField DataField="ProjectID" HeaderText="Project ID" Visible="False" SortExpression="ProjectID" />
                            <asp:BoundField DataField="DisplaySl" HeaderText="Display Sl" SortExpression="DisplaySl" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/edit.png" Text="Select" />
                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif" Text="Delete" />
                                    <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
                                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                        PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                    <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                        <b style="color: red">This entry will be deleted permanently!</b><br />
                                        Are you sure you want to delete this ?<br />
                                        <br />
                                        <div style="text-align: right;">
                                            <asp:Button ID="ButtonOk" runat="server" CssClass="btn btn-success" Text="OK" />
                                            <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                        </div>
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <SelectedRowStyle BackColor="#EEF7F2" Font-Bold="True" ForeColor="#615B5B" />
                        <HeaderStyle BackColor="#FF6600" Font-Bold="True" ForeColor="#222" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </div>

            </fieldset>
        </section>
    </div>

<%--    </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>





