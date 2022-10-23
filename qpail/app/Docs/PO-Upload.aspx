<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PO-Upload.aspx.cs" Inherits="app_Docs_PO_Upload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script runat="server">  
         
</script> 
    <script type="text/javascript">  
    function showConfirmation() {  
        document.getElementById('lblMsg').innerText = 'upload complete.';
    }
    
</script> 
</head>
<body>
    <form id="form1" enctype="multipart/form-data" method="post" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Always">
        <ContentTemplate>


        <%--<div class="control-group">
            <label class="control-label full-wdth">Upload PO Document:</label>
            <div class="controls">--%>

            <div>
       <asp:AsyncFileUpload  Width="300px"
            ID="AsyncFileUpload1"   
            runat="server"   
            OnUploadedComplete="AsyncFileUpload1_UploadComplete"  
            OnClientUploadComplete="showConfirmation" 
            /><asp:Label ID="lblMsg" runat="server" Text="loaded"></asp:Label>                                                                                       
            <%--</div>
        </div>

            --%>

        </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    

    </form>
</body>
    
</html>
