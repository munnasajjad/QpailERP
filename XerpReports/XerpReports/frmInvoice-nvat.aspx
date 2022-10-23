<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmInvoice-nvat.aspx.cs" Inherits="Oxford.XerpReports.frmInvoice_nvat" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/app.v1.css" rel="stylesheet" />
      <style>
        body{font: 13px 'Segoe UI', Tahoma, Arial, Helvetica, sans-serif;}        
    </style>
</head><body>
    <form id="form1" runat="server">
    <div>
        
        <h2>Print Challan: <asp:Literal runat="server" ID="challanNo"></asp:Literal> </h2>

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
