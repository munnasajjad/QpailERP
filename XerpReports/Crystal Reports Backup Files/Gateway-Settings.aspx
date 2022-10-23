<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="Gateway-Settings.aspx.cs" Inherits="Oxford.app.Gateway_Settings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>



    <div class="col-lg-12">
        <section class="panel">

            <div id="Div2">
                <div>

                    <fieldset>
                        <legend> SMS Gateway Settings</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="5">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            
                            <tr>
                                <td><asp:RadioButton ID="rbRoute" runat="server" Text=" Route SMS" GroupName="g" /></td>
                                <td>Host/ IP: <asp:TextBox ID="txtHost" runat="server"></asp:TextBox></td>
                                <td>Sender Name: <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                                <td>User Id: <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                                <td>Password: <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:RadioButton ID="rbBulk" runat="server" Text=" Bulk SMS" GroupName="g"/></td>
                                <td>Host/ IP: <asp:TextBox ID="txtHost2" runat="server"></asp:TextBox></td>
                                <td>Sender Name: <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox></td>
                                <td>User Id: <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox></td>
                                <td>Password: <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox></td>
                            </tr>
                             <tr>
                                <td><asp:RadioButton ID="rbExtreme" runat="server" Text=" Extreme SMS" GroupName="g"/></td>
                                <td>Host/ IP: <asp:TextBox ID="txtHost3" runat="server"></asp:TextBox></td>
                                <td>Sender Name: <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox></td>
                                <td>User Id: <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox></td>
                                <td>Password: <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                              <td align="right" colspan="5">
                                  <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_OnClick" />
                                </td>
                            </tr>

                        </table>
                    </fieldset>


                </div>
            </div>
           
        </section>
    </div>



    

</asp:Content>
