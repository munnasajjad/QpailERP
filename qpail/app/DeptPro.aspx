<%@ Page Title="DepreciationProcess" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="DeptPro.aspx.cs" Inherits="app_DeptPro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">
    
   <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
      <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>

    <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
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
            </script>--%>





            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <h3 class="page-title">Depreciation Process</h3>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">

                        <div class="portlet box blue">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>
                                    <asp:Literal ID="Literal2" runat="server" Text="Fixed Assets Info" />
                                </div>
                            </div>
                            <div class="portlet-body form">
                                <div class="form-horizontal">

                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                                    
                                    <div class="control-group">
                                         <label class="control-label">Depriciation Item Type : </label>
                                        <asp:RadioButton ID="rbtMonthly" runat="server" GroupName="g" Text="Monthly Items" AutoPostBack="true" OnCheckedChanged="rbtMonthly_CheckedChanged" />
                                        <asp:RadioButton ID="rbtYearly" runat="server" GroupName="g" Text="Yearly Items" AutoPostBack="true" OnCheckedChanged="rbtMonthly_CheckedChanged" />
                                    </div>

                                      <div class="control-group">
                                        <label class="control-label">Last Process Date : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtLastProcessDate" CssClass="span6 m-wrap" runat="server" Enabled="false" ></asp:TextBox>
                                              <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                        Enabled="True" TargetControlID="txtLastProcessDate">
                                                    </asp:CalendarExtender>
                                        </div>
                                    </div>
                                   <div class="control-group">
                                        <label class="control-label">Present Process Date:   </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtPresentProcessDate" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtPresentProcessDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>


                                        <div class="table-responsive">
                                       
                                            
                                     
                                    <br />

                                    <div class="form-actions">
                                        <asp:CheckBox ID="chkPrint" runat="server" Checked="false" Text="Print" Visible="false" />
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Run Process" OnClick="btnSave_Click" />

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
</div>


                    <div class="col-md-6">
                        <div class="portlet box red">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>
                                    <asp:Literal ID="Literal1" runat="server" Text="Fixed Assests List" />
                                </div>
                            </div>
                            <div class="portlet-body form">

                                 <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"                                            
                                            BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE"
                                            GridLines="Vertical" Width="100%" >
                                            <RowStyle BackColor="#F7F7DE" />
                                            <Columns>
                                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                                <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate" />
                                                <asp:BoundField DataField="Depreciation" HeaderText="Deprecistion (%)" SortExpression="ItemDeprecistion" />                                                
                                                <asp:TemplateField HeaderText="Depriciated Rate" >
                                                    <ItemTemplate>
                                                            <asp:TextBox ID="txtValue" runat="server" Text='<%#Eval("NewValue","{0:N}")%>'></asp:TextBox>
                                                        
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-012.3456789" TargetControlID="txtValue">
                                                            </asp:FilteredTextBoxExtender>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#CCCC99" />
                                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                            <SelectedRowStyle BackColor="#FFCA2C" Font-Bold="True" ForeColor="#615B5B" />
                                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="#106AAB" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                               


                            </div>
                        </div>
                    </div>

                </div>
            </div>

</asp:Content>

