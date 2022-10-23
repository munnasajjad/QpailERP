<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Currencies.aspx.cs" Inherits="Currencies" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
    /*table th
    {
        min-width:120px;
        color:#333333;
        }
        table td, table td input
    {
        text-align:center;
        font-size:15px;
        max-width:60px;
        }*/
        
</style>
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.0/css/bootstrap.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>


    <div class="grid_12 full_block">
				<div class="widget_wrap">
					<div class="widget_top">
						<span class="h_icon list_image"></span>
						<h6>Setup Currencies</h6>
					</div>
					<div class="widget_content col-md-6">
						<%--<h3>Setup Currencies</h3>--%>
                        						
						<span style="color:Red;">
						<asp:Label ID="lblMsg" CssClass="msg" runat="server" EnableViewState="false"></asp:Label>
						<asp:Label ID="lblErrLoad" runat="server" CssClass="msg" EnableViewState="false"></asp:Label>
						</span>
						
						<div class="form_container left_label">
							<ul>							    
								<li>
								<div class="form_grid_12">
									<label class="field_title">Currency Name</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtCName" runat="server" tabindex="2"  title="Currency Name"  class="limiter tip_top"></asp:TextBox>
										<%--<span class="input_instruction green">Deposit Detail: ie, DBBL-Uttara Branch.</span>--%>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Currency Symbol</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtSymble" runat="server" tabindex="3"  title="Currency Symbol"  class="tip_top" MaxLength="4"></asp:TextBox>
										<%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtAmount">
                                            </asp:FilteredTextBoxExtender>--%>
									</div>
								</div>
								</li>

                                <li>
								<div class="form_grid_12">
									<label class="field_title">General Rate </label>
									<div class="form_input">
                                        <asp:TextBox ID="txtRate" runat="server" tabindex="3"  title="Currency Rate"  class="limiter tip_top"></asp:TextBox>
                                        <asp:Label ID="Label1" runat="server" CssClass="input_instruction green" Text="Base Currency: BDT (tk.)"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtRate">
                                            </asp:FilteredTextBoxExtender>
									</div>
								</div>
								</li>
								
								<li>
								<div class="form_grid_12">
									<div class="form_input">
									
									    <asp:Button ID="btnSave" runat="server" Text="Save Currency Info" TabIndex="11"
                                            class="btn_small btn_blue"  onclick="btnSave_Click" />
                                        <%--<asp:Button ID="Button1" runat="server" Text="Clear Form" 
                                            class="btn_small btn_orange" />--%>
									
										<%--<button type="submit" class="btn_small btn_gray"><span>Submit</span></button>
										<button type="reset" class="btn_small btn_gray"><span>Reset</span></button>
										<button type="submit" class="btn_small btn_blue"><span>Sent New Order</span></button>
										<button type="reset" class="btn_small btn_blue"><span>Reset</span></button>
										<button type="submit" class="btn_small btn_orange"><span>Clear Form</span></button>
										<button type="reset" class="btn_small btn_orange"><span>Reset</span></button>--%>
									</div>
								</div>
								</li>
								
							</ul>
						</div>
					</div>

                    <div class="col-md-6 saved-data">
					<div class="portlet green">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i> SAVED CURRENCIES
							</div>
						</div>
						<div class="portlet-body form">
							
								<div class="form-body">

  
	<table cellspacing="0" rules="all" border="1" id="ctl00_BodyContent_GridView1" style="width:100%;border-collapse:collapse;">
		<tbody><tr>
			<th scope="col">&nbsp;</th><th scope="col">CURRENCY NAME</th><th scope="col">CURRENCY SYMBOL</th><th scope="col">GENERAL RATE</th>
		</tr><tr>
			<td><a href="javascript:__doPostBack('ctl00$BodyContent$GridView1','Edit$0')">Edit</a></td><td>
                                <span id="ctl00_BodyContent_GridView1_ctl02_Label1">Taka</span>
                            </td><td>
                                <span id="ctl00_BodyContent_GridView1_ctl02_Label2">tk.</span>
                            </td><td>
                                <span id="ctl00_BodyContent_GridView1_ctl02_Label3">75</span>
                            </td>
		</tr><tr>
			<td><a href="javascript:__doPostBack('ctl00$BodyContent$GridView1','Edit$0')">Edit</a></td><td>
                                <span id="Span1">Euro</span>
                            </td><td>
                                <span id="Span2">E</span>
                            </td><td>
                                <span id="Span3">8</span>
                            </td>
		</tr><tr>
			<td><a href="javascript:__doPostBack('ctl00$BodyContent$GridView1','Edit$0')">Edit</a></td><td>
                                <span id="Span4">Dollar</span>
                            </td><td>
                                <span id="Span5">$</span>
                            </td><td>
                                <span id="Span6">1</span>
                            </td>
		</tr>
	</tbody></table>
</div>
                
            
            
                                </div>
               </div>

        </div>

    </div>



                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    DataSourceID="SqlDataSource1" DataKeyNames="CrName">
                    <Columns>
                        
                <asp:TemplateField ItemStyle-Width="40px">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="CrName" HeaderText="Currency Name" 
                            SortExpression="CrName" ReadOnly="True" />
                        <asp:BoundField DataField="ShortCode" HeaderText="Symble" ReadOnly="true"
                            SortExpression="ShortCode" />
                        <asp:BoundField DataField="xRate" HeaderText="Exchange Rate" 
                            SortExpression="xRate" />
                                                    
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"                                        
                    SelectCommand="SELECT CrName, ShortCode, Country, xRate FROM [Currencies] ORDER BY [CrName] DESC" 
                    DeleteCommand="DELETE FROM Currencies WHERE (CrName = @CrName)"                     
                    UpdateCommand="UPDATE Currencies SET xRate=@xRate WHERE (CrName = @CrName)" >                    
                <DeleteParameters>
                        <asp:Parameter Name="CrName" />
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
           
                    </div>
</asp:Content>

