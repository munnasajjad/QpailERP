<%@ Page Title="Sales-Statement" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Sales-Statement.aspx.cs" Inherits="app_Sales_Statement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


<!-- BEGIN PAGE LEVEL STYLES -->
<link rel="stylesheet" type="text/css" href="assets/plugins/bootstrap-datepicker/css/datepicker.css" />
<link rel="stylesheet" type="text/css" href="assets/plugins/select2/select2.css" />
<link rel="stylesheet" type="text/css" href="assets/plugins/select2/select2-metronic.css" />
<link rel="stylesheet" href="assets/plugins/data-tables/DT_bootstrap.css" />

<!-- END PAGE LEVEL STYLES -->
<style type="text/css">
label {
padding-top: 6px;
text-align: right;
}

.table1 {
width: 100%;
}

.table1 th {
    vertical-align: middle;
    font-weight: 700;
}

.table1 .form-control, .table1 select {
    width: 100%;
}

table#ctl00_BodyContent_GridView1 {
/*min-width: 1200px;*/
}

table#ctl00_BodyContent_GridView1 tr {
    height: 20px;
}
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

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


<h3 class="page-title">Customers Sales Summery</h3>

<asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


<div id="rptcontent" class="row">

    <div class="col-lg-12">
        <section class="panel">
            <%--Body Contants--%>
            <div id="Div2">
                <div>

                    <fieldset>
                        <%--<legend>Search Terms</legend>--%>
                        <table border="0" width="100%" style="width: 100%" class="table1">
                            <tr>
                                <%--<th>Customer</th>
                                <th></th>--%>
                                <th>Sale Date From</th>
                                <th></th>
                                <th>Sale Date To</th>
                                <th></th>
                                <th></th>
                            </tr>
                            <tr>
                                <%--<td>
                                    <asp:DropDownList ID="ddCustomer" runat="server" CssClass="form-control" DataSourceID="SqlDataSource3"
                                        DataTextField="Company" DataValueField="PartyID" AppendDataBoundItems="true">
                                        <asp:ListItem>---ALL---</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'customer') ORDER BY [Company]"></asp:SqlDataSource>

                                </td>

                                <td>&nbsp; </td>--%>
                                <td>
                                    <asp:TextBox ID="txtdateFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtdateFrom" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>

                                <td>&nbsp; </td>
                                <td>
                                    <asp:TextBox ID="txtdateTo" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtdateTo" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>

                                <td></td>
                                <td style="text-align: center; vertical-align: middle;">
                                    <asp:Button ID="btnSearch" CssClass="btn btn-s-md btn-primary" runat="server" Text="SHOW" OnClick="btnSearch_OnClick" />
                                </td>
                                
                            </tr>
                        </table>
                        
                        <iframe id="if1" runat="server" height="2200px" width="100%" ></iframe>

                    </fieldset>


                </div>
            </div>

        </section>
    </div>





</div>




</ContentTemplate>
</asp:UpdatePanel>


</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

