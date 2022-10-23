<%@ Page Title="" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="YearlyProcess.aspx.cs" Inherits="Oxford.app.YearlyProcess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
   
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
             <script type="text/javascript" language="javascript">
                 Sys.Application.add_load(callJquery);
            </script>


    <div class="col-lg-6">
        <section class="panel">
            
            <div id="Div2">
                <div>

                    <fieldset>
                        <legend>Yearly Process</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <label class="control-label">Class : </label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddClass" runat="server" data-placeholder="--- Select ---" TabIndex="1"
                                        OnSelectedIndexChanged="ddClass_OnSelectedIndexChanged" AutoPostBack="true" CssClass="form-control" DataSourceID="SqlDataSource4" DataTextField="Name" DataValueField="sl">
                                    </asp:DropDownList>

                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [Name] FROM [Class] ORDER BY [Name]"></asp:SqlDataSource>

                                </td>
                            </tr>

                            <tr>
                                <td>Old Session</td>
                                <td>
                                    <asp:DropDownList name="" ID="ddSession" runat="server"
                                        DataSourceID="SqlDataSource3" DataTextField="Session" DataValueField="Session"  CssClass="form-control" 
                                        OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT DISTINCT [Session] FROM [Students] WHERE ([Class] = @Class)">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddClass" Name="Class" PropertyName="SelectedValue" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>

                            <tr>
                                <td>New Class</td>
                                <td>
                                    <asp:DropDownList name="" ID="ddNewClass" runat="server" AutoPostBack="true"  CssClass="form-control" 
                                        DataSourceID="SqlDataSource6" DataTextField="name" DataValueField="sl"
                                        OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource6" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [Name] FROM [Class] order by sl"></asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td>New Session</td>
                                <td>
                                    <asp:TextBox ID="txtSession" runat="server"  CssClass="form-control" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Clear Settings & Data</td>
                                <td>
                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Discounts" /> &nbsp; 
                                    <asp:CheckBox ID="CheckBox2" runat="server" Text="Accounts" />
                                </td>
                            </tr>
                            <tr style="background: none">
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnClear" type="reset" CssClass="btn btn-s-md btn-primary" runat="server" Text="Clear" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>


                </div>
            </div>
            <%--End Body Contants--%>
        </section>
    </div>




    <div class="col-lg-6">
        <section class="panel">

            <div id="Div1">
                <div>

                    <fieldset>
                        <legend>Yearly Process History</legend>




                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table"
                            DataSourceID="SqlDataSource1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">


                            <Columns>


                                <asp:BoundField DataField="OldClass" HeaderText="Old Class" SortExpression="OldClass" />
                                <asp:BoundField DataField="OldSession" HeaderText="Old Session" SortExpression="OldSession" />
                                <asp:BoundField DataField="NewClass" HeaderText="New Class" SortExpression="NewClass" />
                                <asp:BoundField DataField="NewSession" HeaderText="New Session" SortExpression="NewSession" />
                                <asp:BoundField DataField="ClearData" HeaderText="Clear Data" SortExpression="ClearData" />


                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand="SELECT [OldClass], [OldSession], [NewClass], [NewSession], [ClearData] FROM [YearProcess] ORDER BY [sl] DESC">
                        </asp:SqlDataSource>


                    </fieldset>



                </div>
            </div>
            <%--End Body Contants--%>
        </section>
    </div>
            
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
