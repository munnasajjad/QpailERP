<%@ Page Title="Bank Balance Report" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Bank-Book.aspx.cs" Inherits="app_Bank_Book" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">


    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
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
            </script>--%>


    <h3 class="page-title">Bank Balance Report</h3>

    <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>


    <div class="row">
        <div class="col-lg-12">
            <table border="0"  style="width: 75%" class="table1">
                                        <tr>
                                            <th>As on</th>
                                            <th></th>
                                        </tr>
                                        <tr>
                                           <td>
                                                <asp:TextBox ID="txtOpening" runat="server" CssClass="form-control" Width="100%" Enabled="True"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtOpening" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            
                                            <td style="text-align: center; vertical-align: middle;">
                                                <asp:Button ID="btnShow2" CssClass="btn btn-s-md btn-primary" runat="server" Text="Show Report" OnClick="btnShow2_OnClick" />
                                                
                                            </td>
                                        </tr>
                                      
                                    </table>

            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table-bordered table">
                <Columns>
                    <asp:BoundField DataField="BankName" HeaderText="Bank Name" SortExpression="ACName" />
                    <asp:BoundField DataField="ACName" HeaderText="AC Name" SortExpression="ACName" />
                    <asp:BoundField DataField="ACNo" HeaderText="AC No." SortExpression="ACNo" />
                    <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="balance" ItemStyle-HorizontalAlign="Right" />

                    <%--<asp:BoundField DataField="DrHeadName" HeaderText="DrHeadName" SortExpression="DrHeadName" />
                    <asp:BoundField DataField="HeadIdCr" HeaderText="HeadIdCr" SortExpression="HeadIdCr" />
                    <asp:BoundField DataField="CrHeadName" HeaderText="CrHeadName" SortExpression="CrHeadName" />
                    <asp:BoundField DataField="UpdateDate" HeaderText="UpdateDate" SortExpression="UpdateDate" />
                    <asp:BoundField DataField="UpdateBy" HeaderText="UpdateBy" SortExpression="UpdateBy" />
                    <asp:BoundField DataField="SyncBalanceDr" HeaderText="SyncBalanceDr" SortExpression="SyncBalanceDr" />
                    <asp:BoundField DataField="SyncBalanceCr" HeaderText="SyncBalanceCr" SortExpression="SyncBalanceCr" />
                    <asp:BoundField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                    <asp:BoundField DataField="ParticularId" HeaderText="ParticularId" SortExpression="ParticularId" />--%>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT * FROM [AccLink]"></asp:SqlDataSource>
            

        </div>
    </div>



</asp:Content>

