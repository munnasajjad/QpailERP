<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FromReceiptPayment.aspx.cs" Inherits="Oxford.XerpReports.FromReceiptPayment" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.7.725, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     <form id="form1" runat="server">
    <div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>


        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
           
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"  EnableDatabaseLogonPrompt="False" PrintMode="ActiveX"
            GroupTreeImagesFolderUrl="" Height="1202px" ToolbarImagesFolderUrl="" ToolPanelView="None" ToolPanelWidth="200px" Width="1104px" OnUnload="CrystalReportViewer1_OnUnload"/>
                
                </ContentTemplate>
                 <Triggers> <asp:PostBackTrigger ControlID="CrystalReportViewer1"/></Triggers>
            </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
