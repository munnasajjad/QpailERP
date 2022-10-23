<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Servers.aspx.cs" Inherits="AdminCentral_Servers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>


    <div class="grid_12 full_block">
        <div class="widget_wrap">
            <div class="widget_top">
                <span class="h_icon list_image"></span>
                <h6>Servers</h6>
            </div>
            <div class="widget_content">




                <asp:Label ID="lblMsg" CssClass="msg" runat="server" EnableViewState="false"></asp:Label>

                <div class="form_container left_label field_set">

                    <fieldset>
                        <legend> <b> »</b> Setup </legend>

                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ValidGroup" />

                        <ul>
                            

                            <li>
                                <div class="form_grid_12">
                                    <label class="field_title">IP Address</label>
                                    <div class="form_input">
                                        <asp:TextBox ID="txtIP" runat="server" TabIndex="1" class="limiter tip_top" title="IP Address"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtIP">
                                        </asp:FilteredTextBoxExtender>
                                    </div>
                                </div>
                            </li>
                            
                            <li>
                                    <div class="form_grid_12">
                                        <label class="field_title">Server Name/ Alias</label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtName" ErrorMessage="Alias is required." ForeColor="Red" ValidationGroup="ValidGroup" Text="***" />
                                        <div class="form_input">
                                            <asp:TextBox ID="txtName" runat="server" TabIndex="2" title="Server Name/ Alias" class="limiter tip_top"></asp:TextBox>

                                        </div>
                                    </div>
                                </li>

                            <li>
                                <div class="form_grid_12">
                                    <label class="field_title">Server Login ID
                                        <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtLogin" ErrorMessage="Login Info is required." ForeColor="Red" ValidationGroup="ValidGroup" Text="***" />
                                    </label>

                                    <div class="form_input">
                                        <asp:TextBox ID="txtLogin" runat="server" TabIndex="3" MaxLength="100" title="Server Login ID" ValidationGroup="ValidGroup" class="limiter tip_top"></asp:TextBox>
                                                                                
                                    </div>
                                </div>
                            </li>

                            <li>
                                <div class="form_grid_12">
                                    <label class="field_title">Login Password</label>
                                    <div class="form_input">
                                        <asp:TextBox ID="txtPassword" runat="server" TabIndex="4" class="limiter tip_top" title="Login Password"></asp:TextBox>

                                    </div>
                                </div>
                            </li>

                            <li>
                                <div class="form_grid_12">
                                    <label class="field_title">Database</label>
                                    <div class="form_input">
                                        <asp:TextBox ID="txtDB" runat="server" TabIndex="5" class="limiter tip_top" title="Database"></asp:TextBox>

                                    </div>
                                </div>
                            </li>
                            
                            <li>
                                <div class="form_grid_12">
                                    <label class="field_title">DB User
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDbUser" ErrorMessage="DB Info is Required." ForeColor="Red" ValidationGroup="ValidGroup" Text="***" />
                                    </label>


                                    <div class="form_input">
                                        <asp:TextBox ID="txtDbUser" runat="server" TabIndex="6" MaxLength="100" title="DB User" ValidationGroup="ValidGroup" class="limiter tip_top"></asp:TextBox>
                                                                                
                                    </div>
                                </div>
                            </li>

                            <li>
                                <div class="form_grid_12">
                                    <label class="field_title">DB Password</label>
                                    <div class="form_input">
                                        <asp:TextBox ID="txtDbPass" runat="server" TabIndex="7" class="limiter tip_top" title="DB Password"></asp:TextBox>

                                    </div>
                                </div>
                            </li>
                                                                                    
                            <li>
                                <div class="form_grid_12">
                                    <label class="field_title">
                                        <asp:Label ID="lblSend" runat="server" Text="Port"></asp:Label></label>
                                    <div class="form_input">

                                        <asp:TextBox ID="txtPort" runat="server" title="Server Port" class="tip_top" TabIndex="8"></asp:TextBox>
                                        
                                    </div>
                                </div>
                            </li>
                            
                            <li>
                                <%--<hr />--%>
                                <div class="form_grid_12">
                                    <label class="field_title">Enabled</label>
                                    <div class="form_input">
                                        <asp:DropDownList name="" ID="ddEnabled" runat="server"
                                            data-placeholder="--- Select ---" Style="width: 30%; min-width: 150px"
                                            class="chzn-select" TabIndex="9">
                                            <asp:ListItem>Yes</asp:ListItem>
                                            <asp:ListItem>No</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </li>

                            <li>
                                <div class="form_grid_12">
                                    <label class="field_title">Type</label>
                                    <div class="form_input">

                                        <asp:DropDownList ID="ddType" runat="server" data-placeholder="--- Select ---"
                                            Style="width: 30%; min-width: 150px" class="chzn-select"  TabIndex="10"
                                            DataSourceID="SqlDataSource2" DataTextField="TypeName" DataValueField="TypeName">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [TypeName] FROM [ServerTypes]"></asp:SqlDataSource>
                                        <%--<input name="filed01" type="text" tabindex="1" class="limiter"/>
										<span class="input_instruction green">This is an instruction</span>--%>
                                    </div>
                                </div>
                            </li>


                            <li>
                                <div class="form_grid_12">
                                    <label class="field_title">Purchase Date</label>
                                    <div class="form_input">
                                        <asp:TextBox ID="txtDate" runat="server" title="Start Date" TabIndex="11" class="tip_top"></asp:TextBox>
                                        <asp:CalendarExtender runat="server" ID="TextBox3_CalendarExtender" TargetControlID="txtDate" 
                                        Enabled="True" PopupPosition="BottomLeft" FirstDayOfWeek="Saturday" Format="dd/MM/yyyy" />

                                    </div>
                                </div>
                            </li>

                            <li>
                                <div class="form_grid_12">
                                    <label class="field_title">Remarks</label>
                                    <div class="form_input">
                                        <asp:TextBox ID="txtReamarks" runat="server" TabIndex="11" title="Remarks"  class="limiter tip_top"></asp:TextBox>
                                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtBalance">
                                        </asp:FilteredTextBoxExtender>--%>
                                    </div>
                                </div>
                            </li>

                        </ul>


                    </fieldset>

                    <ul class="btn_area">
                        <li>
                            <div class="form_grid_12">
                                <div class="form_input">
                                    
                                    <asp:Button ID="btnSave" runat="server" Text="Save New Server" TabIndex="12" CausesValidation="true" ValidationGroup="ValidGroup" 
                                        class="btn_small btn_blue" OnClick="btnSave_Click" />
                                    <asp:Button ID="Button1" runat="server" Text="Clear Form"
                                        class="btn_small btn_orange" OnClick="Button1_Click" />
                                    
                                </div>
                            </div>
                        </li>
                    </ul>

                    
<%--<asp:TemplateField ItemStyle-Width="40px">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>--%>


                    <fieldset>
                        <legend>Report/ List</legend>

                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource3" CssClass="tbl_default zebra">
                            <Columns>
                                
                <asp:TemplateField ItemStyle-Width="40px">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                                <asp:BoundField DataField="IPAddress" HeaderText="IP" SortExpression="IPAddress" />
                                <asp:BoundField DataField="Alias" HeaderText="Alias" SortExpression="Alias" />
                                <asp:BoundField DataField="LoginID" HeaderText="Login" SortExpression="LoginID" />
                                <asp:BoundField DataField="Password" HeaderText="Pass" SortExpression="Password" />
                                                                
                                <asp:BoundField DataField="DatabaseName" HeaderText="DB Name" SortExpression="DatabaseName" />
                                <asp:BoundField DataField="DBUser" HeaderText="User" SortExpression="DBUser" />
                                <asp:BoundField DataField="DBPassword" HeaderText="Pass" SortExpression="DBPassword" />

                                <asp:BoundField DataField="Port" HeaderText="Port" SortExpression="Port" />
                                <asp:BoundField DataField="Enabled" HeaderText="Active" SortExpression="Enabled" />
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                                <asp:BoundField DataField="OpDate"  DataFormatString="{0:d}"  HeaderText="Date" SortExpression="OpDate" />
                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />

                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT   IPAddress, Alias, LoginID, Password, DatabaseName, DBUser, DBPassword, Port, Enabled, Type, OpDate, Remarks FROM Servers"></asp:SqlDataSource>
                    </fieldset>


                </div>
            </div>
        </div>
    </div>


</asp:Content>


