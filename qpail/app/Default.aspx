<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="AdminCentral_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script src="DataTables/jquery.dataTables.min.js"></script>

    <link rel="stylesheet" type="text/css" href="//cdn.datatables.net/1.10.4/css/jquery.dataTables.min.css">--%>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style>
        .small-box-info {
            background: #1795de;
            color: #fff;
            border-radius: 5px;
        }

        .inner {
            padding: 12px 10px;
        }

        .labeltlt {
            display: inline-block;
            margin-left: 10px;
            width: 60%;
        }

        .labeltlt p {
            font-size: 13px;
            margin-bottom: 5px;
        }

        .value {
            display: inline-block;
            margin-left: 10px;
        }

        .value p {
            font-weight: 600;
            font-size: 20px;
            margin-bottom: 5px;
        }

        .icon-box {
            width: 60px;
            float: left;
            background: #3cd7f3;
            border-radius: 50%;
            height: 60px;
        }

        
    </style>

</asp:Content>

<asp:Content ContentPlaceHolderID="deshboard" runat="server">


    <div class="page_title">
        <div class="live_news">
            <marquee class="smooth_m" behavior="scroll" scrollamount="3" direction="left" width="100%" onmouseout="this.start()" onmouseover="this.stop()">
	    <h3>
	        <%--Welcome to Message Courier Service Limited. Best of the bests courier service and money transfer company in Bangladesh.--%>
	        <asp:Label ID="lblNews" runat="server" Text=""></asp:Label>
	    </h3></marquee>
        </div>
    </div>
    <div class="switch_bar">
        <ul>
            <li id="OrderEntry" runat="server"><a href="Order-Entry.aspx"><span class="stats_icon graphic_design_sl"></span><span class="label">Order Entry</span></a></li>
            <li id="menuID171" runat="server"><a href="Order-Delivery.aspx"><span class="stats_icon upcoming_work_sl"></span><span class="label">Delivery</span></a></li>
            <li id="menuID167" runat="server"><a href="Collection.aspx"><span class="stats_icon bank_sl"></span><span class="label">Collection</span></a></li>
            <li id="menuID168" runat="server"><a href="Purchase.aspx"><span class="stats_icon administrative_docs_sl"></span><span class="label">Purchase</span></a></li>
            <li id="menuID169" runat="server"><a href="Payment.aspx"><span class="stats_icon billing_sl"></span><span class="label">Payment</span></a></li>
            <li id="menuID170" runat="server"><a href="Products.aspx"><span class="stats_icon archives_sl"></span><span class="label">Products</span></a></li>

            <li id="menuID172" runat="server" visible="False"><a href="Employee-Production.aspx"><span class="stats_icon communication_sl"></span><span class="label">Production</span></a></li>
            <li id="menuID173" runat="server" visible="False"><a href="Stock-in.aspx"><span class="stats_icon current_work_sl"></span><span class="label">Stock-in</span></a></li>
            <li id="menuID174" runat="server"><a href="Emp-Daily-Attn.aspx"><span class="stats_icon customers_sl"></span><span class="label">Attendance</span></a></li>
            <li id="menuID" runat="server"><a href="LC-Expenses.aspx"><span class="stats_icon category_sl"></span><span class="label">LC Exp.</span></a></li>
            <li id="menuID175" runat="server"><a href="VoucherEntry.aspx"><span class="stats_icon address_sl"></span><span class="label">Voucher</span></a></li>
            <li id="menuID176" runat="server"><a href="#"><span class="stats_icon issue_sl"></span><span class="label">Reports</span></a></li>
        </ul>
    </div>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <span class="clear"></span>
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="uPanel" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="uPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>

            <div class="row" style="margin-top: 20px;">
                <div class="col-md-12">
                    <div class="col-md-3">
                        <div class="small-box-info">
                            <div class="inner">
                                <div class="icon-box">
                                    <i class="fa fa-money" style="font-size: 28px;top: 27%;position: relative;left: 25%;"></i>
                                </div>
                                <div class="labeltlt">
                                    <p>TOTAL PURCHASE</p>
                                </div>
                                <div class="value">
                                    <p>৳ 0.00</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="small-box-info">
                            <div class="inner">
                                <div class="icon-box">
                                    <i class="fa fa-shopping-cart" style="font-size: 28px;top: 25%;position: relative;left: 25%;"></i>
                                </div>
                                <div class="labeltlt">
                                    <p>TOTAL SALES</p>
                                </div>
                                <div class="value">
                                    <p>৳ 0.00</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="small-box-info">
                            <div class="inner">
                                <div class="icon-box">
                                    <i class="fa fa-exclamation" style="font-size: 28px;top: 27%;position: relative;left: 41%;"></i>
                                </div>
                                <div class="labeltlt">
                                    <p>PURCHASE DUE</p>
                                </div>
                                <div class="value">
                                    <p>৳ 0.00</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="small-box-info">
                            <div class="inner">
                                <div class="icon-box">
                                    <i class="fa fa-file" style="font-size: 28px;top: 27%;position: relative;left: 30%;"></i>
                                </div>
                                <div class="labeltlt">
                                    <p>INVOICE DUE</p>
                                </div>
                                <div class="value">
                                    <p>৳ 0.00</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" style="margin-top: 20px;">
                <div class="col-md-12">
                    <div class="col-md-3">
                        <div class="small-box-info">
                            <div class="inner">
                                <div class="icon-box">
                                    <i class="fa fa-window-minimize" style="font-size: 28px;top: 15%;position: relative;left: 25%;"></i>
                                </div>
                                <div class="labeltlt">
                                    <p>EXPENSE</p>
                                </div>
                                <div class="value">
                                    <p>৳ 0.00</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="small-box-info">
                            <div class="inner">
                                <div class="icon-box">
                                    <i class="fa fa-shopping-cart" style="font-size: 28px;top: 25%;position: relative;left: 25%;"></i>
                                </div>
                                <div class="labeltlt">
                                    <p>TOTAL SALES</p>
                                </div>
                                <div class="value">
                                    <p>৳ 0.00</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="small-box-info">
                            <div class="inner">
                                <div class="icon-box">
                                    <i class="fa fa-exclamation" style="font-size: 28px;top: 27%;position: relative;left: 41%;"></i>
                                </div>
                                <div class="labeltlt">
                                    <p>PURCHASE DUE</p>
                                </div>
                                <div class="value">
                                    <p>৳ 0.00</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="small-box-info">
                            <div class="inner">
                                <div class="icon-box">
                                    <i class="fa fa-file" style="font-size: 28px;top: 27%;position: relative;left: 30%;"></i>
                                </div>
                                <div class="labeltlt">
                                    <p>INVOICE DUE</p>
                                </div>
                                <div class="value">
                                    <p>৳ 0.00</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="grid_12">
                <div class="widget_wrap">
                    <div class="widget_top">
                        <span class="h_icon list_images"></span>
                        <h6>Task List</h6>
                    </div>

                    <div class="">

                        <table style="width: 100%">

                            <tr>
                                <td colspan="8">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td style="vertical-align: middle;">
                                    <asp:Label ID="Label3" runat="server" Text="Task Details:"></asp:Label>
                                </td>
                                <td colspan="6">
                                    <asp:TextBox ID="txtDetail" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Deadline:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Priority:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddType" runat="server">
                                        <asp:ListItem>Low</asp:ListItem>
                                        <asp:ListItem>Medium</asp:ListItem>
                                        <asp:ListItem>High</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save Task" OnClick="btnSave_Click" />
                                    <asp:Label ID="lblMsg" runat="server" Text="" EnableViewState="false"></asp:Label>
                                </td>
                            </tr>

                        </table>



                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource2"
                            CssClass="zebra table" BorderWidth="1px" Width="100%" CellPadding="3" OnSelectedIndexChanged="GridView2_SelectedIndexChanged"
                            OnRowDeleting="GridView2_RowDeleting" DataKeyNames="tid" AllowSorting="True">
                            <RowStyle ForeColor="#000066" />
                            <Columns>

                                <asp:TemplateField HeaderText="CrID" SortExpression="CrID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("tid") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="SrNo" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="TaskDetails" HeaderText="TaskDetails" SortExpression="TaskDetails" />
                                <asp:BoundField DataField="DeadLine" HeaderText="DeadLine" SortExpression="DeadLine" DataFormatString="{0:d}" />
                                <asp:TemplateField HeaderText="Priority" SortExpression="Priority">
                                    <ItemTemplate>
                                        <asp:Label ID="Label12" runat="server" CssClass='<%# Bind("pcss") %>' Text='<%# Bind("Priority") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" CssClass='<%# Bind("scss") %>' Text='<%# Bind("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="EntryBy" HeaderText="EntryBy" SortExpression="EntryBy" />


                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/approve.gif" Text="Approve" ToolTip="Approve" />
                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.gif" Text="Delete" ToolTip="Delete" />

                                        <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                        </asp:ConfirmButtonExtender>
                                        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                            PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                        <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                            <b style="color: red">Item will be deleted permanently!</b><br />
                                            Are you sure you want to delete the item from order list?
                                                            <br />
                                            <br />
                                            <div style="text-align: right;">
                                                <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                            </div>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <EmptyDataTemplate>
                                You Dont Have any tasks yet!
                            </EmptyDataTemplate>
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="Green" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="Black" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand="SELECT tid, [TaskDetails], [DeadLine], 'badge_style b_'+[Priority] as pcss, [Priority],'badge_style b_'+[Status] as scss, [Status], [EntryBy] FROM [Tasks] WHERE ([Status] &lt;&gt; @Status) ORDER BY [tid]"
                            DeleteCommand="Update Tasks set Status='inactive' where tid=@tid">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="inactive" Name="Status" Type="String" />
                            </SelectParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="tid" />
                            </DeleteParameters>
                        </asp:SqlDataSource>

                    </div>
                </div>
            </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


    <span class="clear"></span>






    <%--<div class="grid_6">
				<div class="widget_wrap">
					<div class="widget_top">
						<span class="h_icon graph"></span>
						<h6>Recent Orders</h6>
					</div>
					<div class="widget_content">
						<div class="stat_block">
							<h4>4,355 people visited this site</h4>
							<table>
							<tbody>
							<tr>
								<td>
									Visitors
								</td>
								<td>
									3000
								</td>
								<td class="min_chart">
									<span class="bar">20,30,50,200,250,280,350</span>
								</td>
							</tr>
							<tr>
								<td>
									Unique Visitors
								</td>
								<td>
									2000
								</td>
								<td class="min_chart">
									<span class="line">20,30,50,200,250,280,350</span>
								</td>
							</tr>
							<tr>
								<td>
									New Visitors
								</td>
								<td>
									1000
								</td>
								<td class="min_chart">
									<span class="line">20,30,50,200,250,280,350</span>
								</td>
							</tr>
							</tbody>
							</table>
							<div class="stat_chart">
								<div class="pie_chart">
									<span class="inner_circle">1/1.5</span>
									<span class="pie">1/1.5</span>
								</div>
								<div class="chart_label">
									<ul>
										<li><span class="new_visits"></span>New Visitors: 7000</li>
										<li><span class="unique_visits"></span>Unique Visitors: 3000</li>
									</ul>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
			<div class="grid_6">
				<div class="widget_wrap">
					<div class="widget_top">
						<span class="h_icon documents"></span>
						<h6>Recent Users</h6>
					</div>
					<div class="widget_content">
						<div class="user_list">
							<div class="user_block">
								<div class="info_block">
									<div class="widget_thumb">
										<img src="../images/user-thumb1.png" width="40" height="40" alt="User">
									</div>
									<ul class="list_info">
										<li><span>Name: <i><a href="#">Zara Zarin</a></i></span></li>
										<li><span>IP: 194.132.12.1 Date: 13th Jan 2012</span></li>
										<li><span>User Type: Paid, <i>Package Name:</i><b>Gold</b></span></li>
									</ul>
								</div>
								<ul class="action_list">
									<li><a class="p_edit" href="#">Edit</a></li>
									<li><a class="p_del" href="#">Delete</a></li>
									<li><a class="p_reject" href="#">Suspend</a></li>
									<li class="right"><a class="p_approve" href="#">Approve</a></li>
								</ul>
							</div>
							<div class="user_block">
								<div class="info_block">
									<div class="widget_thumb">
										<img src="../images/user-thumb1.png" width="40" height="40" alt="user">
									</div>
									<ul class="list_info">
										<li><span>Name: <i><a href="#">Zara Zarin</a></i></span></li>
										<li><span>IP: 194.132.12.1 Date: 13th Jan 2012</span></li>
										<li><span>User Type: Paid, <i>Package Name:</i><b>Gold</b></span></li>
									</ul>
								</div>
								<ul class="action_list">
									<li><a class="p_edit" href="#">Edit</a></li>
									<li><a class="p_del" href="#">Delete</a></li>
									<li><a class="p_reject" href="#">Suspend</a></li>
									<li class="right"><a class="p_approve" href="#">Approve</a></li>
								</ul>
							</div>
							<div class="user_block">
								<div class="info_block">
									<div class="widget_thumb">
										<img src="../images/user-thumb1.png" width="40" height="40" alt="user">
									</div>
									<ul class="list_info">
										<li><span>Name: <i><a href="#">Zara Zarin</a></i></span></li>
										<li><span>IP: 194.132.12.1 Date: 13th Jan 2012</span></li>
										<li><span>User Type: Paid, <i>Package Name:</i><b>Gold</b></span></li>
									</ul>
								</div>
								<ul class="action_list">
									<li><a class="p_edit" href="#">Edit</a></li>
									<li><a class="p_del" href="#">Delete</a></li>
									<li><a class="p_reject" href="#">Suspend</a></li>
									<li class="right"><a class="p_approve" href="#">Approve</a></li>
								</ul>
							</div>
						</div>
					</div>
				</div>
			</div>
			<span class="clear"></span>--%>
    <div class="span_12">
        <div class="widget_wrap">
            <div class="widget_top">
                <span class="h_icon list_images"></span>
                <%--<h6>Todays Production List</h6>--%>
                <h6>Pending matured Bill List</h6>
                <asp:Label ID="Label2" runat="server" Text="Customer :"></asp:Label>
                <asp:DropDownList ID="ddParties" runat="server" DataSourceID="SqlDataSource3"
                    DataTextField="Company" DataValueField="PartyID" Width="400px" AppendDataBoundItems="True" CssClass="select2me" OnSelectedIndexChanged="ddParties_OnSelectedIndexChanged" AutoPostBack="True">
                    <asp:ListItem Value="0">---Select---</asp:ListItem>
                    <asp:ListItem Value="O404O">Pending matured bill list by company</asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                    ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                    SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'customer') ORDER BY [Company]"></asp:SqlDataSource>
            </div>
            <div class="widget_content1">
                <!--<asp:GridView ID="GridView1" runat="server" CssClass="table full-wdth" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="ProductionID" HeaderText="ProductionID" SortExpression="ProductionID"  />
                        <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="SectionName" HeaderText="Section Name" SortExpression="SectionName" />
                        <asp:BoundField DataField="ForCompany" HeaderText="For Company" SortExpression="ForCompany" />
                        <asp:BoundField DataField="ItemName" HeaderText="Item Name" SortExpression="ItemName" />
                        <asp:BoundField DataField="ItemWeight" HeaderText="Item Weight" SortExpression="ItemWeight" />
                        <asp:BoundField DataField="InputQty" HeaderText="Input Qty" SortExpression="InputQty" />
                        <asp:BoundField DataField="WorkingHour" HeaderText="Working Hr" SortExpression="WorkingHour" />
                        <asp:BoundField DataField="FinalProduction" HeaderText="Final Prdn" SortExpression="FinalProduction" />
                        <asp:BoundField DataField="TimeWaste" HeaderText="Time Waste" SortExpression="TimeWaste" />
                        <asp:BoundField DataField="ReasonWaist" HeaderText="Reason Waist" SortExpression="ReasonWaist" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                    SelectCommand="SELECT [ProductionID], [Date], [SectionName], [SectionID], [ForCompany], [ItemName], [ItemWeight], [InputQty], [WorkingHour], [FinalProduction], [TimeWaste], [ReasonWaist] FROM [Production] ORDER BY [pid]">
                </asp:SqlDataSource>-->

                <asp:GridView ID="GridView4" runat="server" CssClass="table full-wdth" AutoGenerateColumns="False" Width="100%" AllowPaging="True" PageSize="20" DataKeyNames="InvNo"
                    OnPageIndexChanging="GridView4_OnPageIndexChanging" OnRowDataBound="GridView4_OnRowDataBound" ShowFooter="True">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl#">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                                <asp:HiddenField ID="matuirityDaysHiddenField" runat="server" Value='<%# Eval("MatuirityDays") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="Company" HeaderText="Company" SortExpression="Company" />
                        <asp:BoundField DataField="InvDate" HeaderText="Invoice Date" SortExpression="InvDate" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="InvNo" HeaderText="Invoice No" SortExpression="InvNo" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="InvoiceTotal" HeaderText="Invoice Total" SortExpression="InvoiceTotal" />
                        <asp:BoundField DataField="MaturedDate" HeaderText="Matured Date" SortExpression="MaturedDate" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Total crossed days">
                            <ItemTemplate>
                                <asp:Label ID="lblDeliveryDaysCount" runat="server" Text='<%# Bind("DeliveryDaysCount") %>'></asp:Label>
                                <asp:HiddenField ID="deliveryDaysCountHiddenField" runat="server" Value='<%# Eval("DeliveryDaysCount") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                        </asp:TemplateField>

                    </Columns>
                    <FooterStyle BackColor="#f3f3f3" Font-Bold="True" ForeColor="#23A6F0" BorderStyle="Solid" />
                    <PagerStyle CssClass="gvpaging"></PagerStyle>
                </asp:GridView>

                <%--<h3>Task list with label badge</h3>
						<p>
							 Cras erat diam, consequat quis tincidunt nec, eleifend a turpis. Aliquam ultrices feugiat metus, ut imperdiet erat mollis at. Curabitur mattis risus sagittis nibh lobortis vel.
						</p>
						<table class="display" id="action_tbl">
						<thead>
						<tr>
							<th class="center">
								<input name="checkbox" type="checkbox" value="" class="checkall">
							</th>
							<th>
								 Id
							</th>
							<th>
								 Task
							</th>
							<th>
								 Dead Line
							</th>
							<th>
								 Priority
							</th>
							<th>
								 Status
							</th>
							<th>
								 Complete Date
							</th>
							<th>
								 Action
							</th>
						</tr>
						</thead>
						<tbody>
						<tr>
							<td class="center tr_select ">
								<input name="checkbox" type="checkbox" value="">
							</td>
							<td>
								<a href="#">01</a>
							</td>
							<td>
								<a href="#" class="t-complete">Pellentesque ut massa ut ligula ... </a>
							</td>
							<td class="sdate center">
								 1st FEB 2012
							</td>
							<td class="center">
								<span class="badge_style b_high">High</span>
							</td>
							<td class="center">
								<span class="badge_style b_done">Done</span>
							</td>
							<td class="center sdate">
								 3rd FEB 2012
							</td>
							<td class="center">
								<span><a class="action-icons c-edit" href="#" title="Edit">Edit</a></span><span><a class="action-icons c-delete" href="#" title="delete">Delete</a></span><span><a class="action-icons c-approve" href="#" title="Approve">Done</a></span>
							</td>
						</tr>
						<tr>
							<td class="center tr_select ">
								<input name="checkbox" type="checkbox" value="">
							</td>
							<td>
								<a href="#">02</a>
							</td>
							<td>
								<a href="#" class="t-complete">Nulla non ante dui, sit amet ... </a>
							</td>
							<td class="sdate center">
								 1st FEB 2012
							</td>
							<td class="center">
								<span class="badge_style b_low">Low</span>
							</td>
							<td class="center">
								<span class="badge_style b_done">Done</span>
							</td>
							<td class="center sdate">
								 3rd FEB 2012
							</td>
							<td class="center">
								<span><a class="action-icons c-edit" href="#" title="Edit">Edit</a></span><span><a class="action-icons c-delete" href="#" title="delete">Delete</a></span><span><a class="action-icons c-approve" href="#" title="Approve">Done</a></span>
							</td>
						</tr>
						<tr>
							<td class="center tr_select ">
								<input name="checkbox" type="checkbox" value="">
							</td>
							<td>
								<a href="#">03</a>
							</td>
							<td>
								<a href="#" class="t-complete">Aliquam eu pellentesque... </a>
							</td>
							<td class="sdate center">
								 1st FEB 2012
							</td>
							<td class="center">
								<span class="badge_style b_medium">Medium</span>
							</td>
							<td class="center">
								<span class="badge_style b_done">Done</span>
							</td>
							<td class="center sdate">
								 3rd FEB 2012
							</td>
							<td class="center">
								<span><a class="action-icons c-edit" href="#" title="Edit">Edit</a></span><span><a class="action-icons c-delete" href="#" title="delete">Delete</a></span><span><a class="action-icons c-approve" href="#" title="Approve">Done</a></span>
							</td>
						</tr>
						<tr>
							<td class="center tr_select">
								<input name="checkbox" type="checkbox" value="">
							</td>
							<td>
								<a href="#">04</a>
							</td>
							<td>
								<a href="#">Maecenas egestas alique... </a>
							</td>
							<td class="sdate center">
								 1st FEB 2012
							</td>
							<td class="center">
								<span class="badge_style b_high">High</span>
							</td>
							<td class="center">
								<span class="badge_style b_pending">Pending</span>
							</td>
							<td class="center sdate">
								 -
							</td>
							<td class="center">
								<span><a class="action-icons c-edit" href="#" title="Edit">Edit</a></span><span><a class="action-icons c-delete" href="#" title="delete">Delete</a></span><span><a class="action-icons c-approve" href="#" title="Approve">Done</a></span>
							</td>
						</tr>
						</tbody>
						<tfoot>
						<tr>
							<th class="center">
								<input name="checkbox" type="checkbox" value="" class="checkall">
							</th>
							<th>
								 Id
							</th>
							<th>
								 Task
							</th>
							<th>
								 Dead Line
							</th>
							<th>
								 Priority
							</th>
							<th>
								 Status
							</th>
							<th>
								 Complete Date
							</th>
							<th>
								 Action
							</th>
						</tr>
						</tfoot>
						</table>--%>
            </div>
        </div>
    </div>

    <span class="clear"></span>

</asp:Content>

