<%@ Page Title="Database  Backup" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="Backup.aspx.cs" Inherits="Application_Backup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .center {
            text-align: center;
        }
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">


    <div class="row">
        <div class="col-md-12 ">
            <div class="portlet-body form">
                <div class="table-responsive">


                    <table cellpadding="10" cellspacing="10" style="border: solid 10px #4C5566; background-color: Skyblue;"
                        width="90%" align="center">
                        <tr>
                            <td style="height: 35px; background-color: #4C5566; font-weight: bold; font-size: 16pt; color: whitesmoke"
                                align="center">Database  Backup &amp; Download
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txtDB" runat="server" CssClass="form-control center" Width="100%" Text="extreme_db"></asp:TextBox>
                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnBackup" runat="server" Text="Run Backup Process" OnClick="btnBackup_OnClick" Width="150px" Height="40px" />
                            </td>

                        </tr>
                    </table>
                    
                </div>
            </div>
        </div>
    </div>



</asp:Content>

