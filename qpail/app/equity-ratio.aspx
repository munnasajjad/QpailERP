<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="equity-ratio.aspx.cs" Inherits="app_equity_ratio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content  ContentPlaceHolderID="BodyContent" Runat="Server">
    <legend style="font-size:medium">Account &#8680 Report &#8680 Equity Ratio</legend>
    <div class="col-lg-6">
        <table>
            <tr>
                <td>Financial Year :</td>
                <td>
                    <asp:TextBox ID="txtFinancialYear" runat="server" CssClass="form-control"/>
                    
                </td>
                </tr>
        </table>
    </div>
    
    
    
    

</asp:Content>


