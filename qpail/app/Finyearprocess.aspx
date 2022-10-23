<%@ Page Title="Yearly Process" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Finyearprocess.aspx.cs" Inherits="app_Finyearprocess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

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
            

            <h3 class="page-title">Yearly Process</h3>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">

                <div class="col-lg-12">
                    <section class="panel">
                        <%--Body Contants--%>
                        <div id="Div2">

                            <asp:UpdateProgress ID="UpdateProgress1" runat="server"></asp:UpdateProgress>

                            <div>
                                <fieldset>
                                    
                                    <%--<legend>Search Terms</legend>--%>
                                    <table border="0" width="60%" style="width: 60%" class="table1">
                                        <tr>
                                            <td style="vertical-align: middle; text-align: center; font-size: 12px"><b>Financial Year : </b></td>
                                            <td>
                                                <asp:DropDownList ID="ddYear1" runat="server" CssClass="form-control"
                                                    DataSourceID="SqlDataSource3" DataTextField="Financial_Year" DataValueField="Financial_Year_Number" AutoPostBack="True" OnSelectedIndexChanged="ddYear1_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" SelectCommand="SELECT Financial_Year_Number, [Financial_Year] FROM [tblFinancial_Year]"></asp:SqlDataSource>
                                            </td>

                                            <td style="text-align: center; vertical-align: middle;">
                                                <asp:Button ID="btnShow2" CssClass="btn btn-s-md btn-primary" runat="server" Text="Run Process" OnClick="btnSearch_OnClick" />

                                            </td>
                                           
                                             <td>
                                                
                                           </td>
                                        </tr>
                                       </table>
                                </fieldset>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

