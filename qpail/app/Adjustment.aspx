
<%@ Page Title="Adjustment" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Adjustment.aspx.cs" Inherits="app_Adjustment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
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


            

            <h3 class="page-title">Balance Adjustment</h3>


            <div class="row">
                <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Adjustment with Suppliers
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">
                                <div class="control-group">
                                    <asp:Label ID="Label3" runat="server" Text="Date of Adjustment:"></asp:Label>
                                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                                        Enabled="True" TargetControlID="txtDate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="lblDeptName" runat="server" Text="Adjustment For: "></asp:Label>
                                    <asp:DropDownList ID="ddParties" runat="server" DataSourceID="SqlDataSource1"
                                        DataTextField="Company" DataValueField="PartyID" AutoPostBack="True" OnSelectedIndexChanged="ddParties_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [Company], [PartyID] FROM [Party] WHERE ([Type] = @Type)">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="vendor" Name="Type" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="Label1" runat="server" Text="Current Balance "></asp:Label> 
                                    <asp:Label id="ltrBalance" runat="server"/> 
                                </div>
                                
                                <div class="control-group">
                                    <asp:Label ID="Label5" runat="server" Text="New Balance : "></asp:Label>
                                    <asp:TextBox ID="txtCollection" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789.-" TargetControlID="txtCollection">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="Label2" runat="server" Text="Remark/ Reason : "></asp:Label>
                                    <asp:TextBox ID="txtDetail" runat="server"></asp:TextBox>
                                </div>
                                <div class="form-actions">
                                    <asp:Button ID="btnSave" runat="server" Text="Save Payment"
                                        OnClick="btnSave_Click" />
                                    <asp:Label ID="lblMsg" CssClass="msg" runat="server"></asp:Label>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>



                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Recent Adjustments
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
                                    AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                                    BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black"
                                    GridLines="Vertical" DataSourceID="SqlDataSource3">
                                    <Columns>
                                        <asp:BoundField DataField="AdjustmentDate" HeaderText="Date" SortExpression="ColType" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="PartyName" HeaderText="Party Name" SortExpression="PartyName" />
                                        <asp:BoundField DataField="CurrentBalance" HeaderText="Current Balance" SortExpression="ChqDetail" />
                                        <asp:BoundField DataField="NewBalance" HeaderText="New Balance" SortExpression="ChqDate" />
                                        <asp:BoundField DataField="PaidAmount" HeaderText="Adjust Amount" SortExpression="PaidAmount" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT TOP(50) AdjustmentDate, (Select Company from Party where PartyID=Adjustment.PartyName) AS PartyName, CurrentBalance, NewBalance, PaidAmount FROM [Adjustment]  ORDER BY [AdjustmentID] DESC">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="txtDate" Name="AdjustmentDate"
                                            PropertyName="Text" Type="DateTime" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                </fieldset>



	<asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div id="blur">&nbsp;</div>
            <div id="progress">
                Update in progress. Please wait ...

            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>

