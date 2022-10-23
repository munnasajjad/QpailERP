<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Charges-Setup.aspx.cs" Inherits="AdminCentral_Charges_Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
    table th
    {
        min-width:120px;
        color:#333333;
        }
        table td, table td input
    {
        text-align:center;
        font-size:15px;
        max-width:60px;
        }
        
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="grid_12 full_block">
				<div class="widget_wrap">
					<div class="widget_top">
						<span class="h_icon list_image"></span>
						<h6>Set Service Charges</h6>
					</div>
					<div class="widget_content">
						<h3>Set Charges</h3>
						<p> </p>

    &nbsp;<table class="table1">
        <asp:Panel ID="Panel1" runat="server" Visible="false">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Plan Name: "></asp:Label>
            </td>
            <td class="style1">
                <asp:TextBox ID="txtPlans" runat="server" ></asp:TextBox>
                </td>
            <td>
                <asp:TextBox ID="txtDid" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr><%--Department Name--%>
            <td>
                <asp:Label ID="lblDeptName" runat="server" Text="Click Limit: "></asp:Label>
            </td>
            <td class="style1">
                <asp:TextBox ID="txtClick" runat="server" width="50px"></asp:TextBox>
                </td>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Service Charge on 1000: "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtService" runat="server" width="50px"></asp:TextBox>
                </td>
        </tr>
        <tr><%--Department Name--%>
            <td>
                <asp:Label ID="Label6" runat="server" Text="Click Value (for 01 Ad): "></asp:Label>
            </td>
            <td class="style1">
                <asp:TextBox ID="TextBox6" runat="server" width="50px"></asp:TextBox>
                </td>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Referral Bonus (%): "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtRef" runat="server" width="50px"></asp:TextBox>
                </td>
        </tr>
        <tr><%--Department Name--%>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Joining Amount: ($) "></asp:Label>
            </td>
            <td class="style1">
                <asp:TextBox ID="txtAmount" runat="server" width="50px"></asp:TextBox>
                </td>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Matching Bonus (%): "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtMatch" runat="server" width="50px"></asp:TextBox>
                </td>
        </tr>
        <tr><%--Department Name--%>
            <td>
                &nbsp;</td>
            <td class="style1">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save New Plan" onclick="btnSave_Click" />
                </td>
        </tr>
        <tr><%--Department Name--%>
            <td colspan="4">
                <asp:Label ID="lblMsg" CssClass="msg" runat="server"></asp:Label>
            </td>
        </tr>
        
        </asp:Panel>
        
        <tr>
            
            <td class="style1">

                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    DataSourceID="SqlDataSource1" RowStyle-Height="25px"
                    DataKeyNames="ServiceName" BackColor="White" Font-Size="15px"
                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                    ForeColor="Black" GridLines="Vertical" >
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="ServiceName" HeaderText="Service Name" 
                            SortExpression="ServiceName" ReadOnly="True" />
                        <asp:BoundField DataField="MinAmt" HeaderText="Min. Booking Amt" 
                            SortExpression="MinAmt" />
                        <asp:BoundField DataField="MinimumCharge" HeaderText="Min. Service Charge" 
                            SortExpression="MinimumCharge" />
                        <asp:BoundField DataField="MinChargeOnAmount" HeaderText="Min.Charge On Amt" 
                            SortExpression="MinChargeOnAmount" />
                        <asp:BoundField DataField="ServiceChargePercent" HeaderText="Service Charge on 1000" 
                            SortExpression="ServiceChargePercent" />
                        <asp:BoundField DataField="TTCharge" HeaderText="TT Charge" 
                            SortExpression="TTCharge" />
                            
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"                                        
                    SelectCommand="SELECT [ServiceName], [MinAmt], [MinimumCharge], [MinChargeOnAmount], [ServiceChargePercent], [TTCharge] FROM [Plans] ORDER BY [ServiceName] DESC" 
                    DeleteCommand="DELETE FROM Plans WHERE (ServiceName = @ServiceName)"                     
                    UpdateCommand="UPDATE Plans SET MinAmt=@MinAmt, MinimumCharge = @MinimumCharge, MinChargeOnAmount=@MinChargeOnAmount, ServiceChargePercent = @ServiceChargePercent, TTCharge=@TTCharge WHERE (ServiceName = @ServiceName)" >                    
                <DeleteParameters>
                        <asp:Parameter Name="ServiceName" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="MinAmt" />
                        <asp:Parameter Name="MinimumCharge" />
                        <asp:Parameter Name="ServiceChargePercent" />
                        <asp:Parameter Name="ServiceName" />
                        <asp:Parameter Name="MinChargeOnAmount" />
                        <asp:Parameter Name="TTCharge" />
                    </UpdateParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
        </table>

					</div>
			</div>

    </div>
</asp:Content>

