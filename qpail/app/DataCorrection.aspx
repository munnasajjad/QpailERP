<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="DataCorrection.aspx.cs" Inherits="app_DataCorrection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">
    <div class="col-md-6">
        <div class="control-group">
            
            <div style="margin-top: 29px">
                
                <asp:GridView runat="server" ID="GV1" Width="100%"/>

                <asp:Button ID="btnCorrection" runat="server" Text="Correction" Width="100px" OnClick="btnCorrection_OnClick" />
                
                <asp:Button ID="Button1" runat="server" Text="ControlAC_VTemp" Width="100px" OnClick="btnCorrection_OnClick" />
                
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

