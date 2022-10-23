﻿<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Oxford.app.Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodycontent" runat="server">




    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

    <asp:UpdatePanel ID="uPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

<section class="scrollable wrapper">
                <div class="row">
                    <div class="col-lg-8">

                        <%--<img src="./images/for.jpg" />--%>

                        <section class="panel">
                            <asp:TextBox ID="txtDetail" runat="server" TextMode="MultiLine" CssClass="form-control input-lg no-border"
                                Rows="2" placeholder="Add your tasks & schedules ..."></asp:TextBox>
                            <asp:Label ID="lblMsg" runat="server" Text="" EnableViewState="false"></asp:Label>
                            <footer class="panel-footer bg-light lter">
                                <asp:Button ID="btnSave" runat="server" Text="Save Task" CssClass="btn btn-info pull-right" OnClick="btnSave_Click" />
                                <ul class="nav nav-pills">
                                    <li>
                                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                            Enabled="True" TargetControlID="txtDate">
                                        </asp:CalendarExtender>
                                    </li>
                                    <li>
                                        <asp:DropDownList ID="ddType" runat="server" CssClass="form-control m-b">
                                            <asp:ListItem>Low</asp:ListItem>
                                            <asp:ListItem>Medium</asp:ListItem>
                                            <asp:ListItem>High</asp:ListItem>
                                        </asp:DropDownList>
                                    </li>
                                    <li></li>
                                </ul>
                            </footer>
                        </section>


                        <section class="panel"> 
                            <header class="panel-heading"> 
                                <asp:Label ID="lblPending" runat="server" CssClass="label bg-danger pull-right"></asp:Label>
                                 Tasks </header> 

                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource2" 
                            CssClass="table table-striped m-b-none text-sm" Width="100%" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                            DataKeyNames="tid" AllowSorting="True">
                            <Columns>

                                <asp:TemplateField HeaderText="CrID" SortExpression="CrID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("tid") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="SrNo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25px">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="TaskDetails" HeaderText="Task Details" SortExpression="TaskDetails" />
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
                                <asp:BoundField DataField="EntryBy" HeaderText="EntryBy" SortExpression="EntryBy" Visible="false" />


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
                            <EmptyDataTemplate>
                                You Dont Have any tasks yet!
                            </EmptyDataTemplate>
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


</section>


                    </div>

                    







                    <%--<div class="row">
                    <div class="col-lg-6 col-sm-6">
                        <section class="panel">
                            <div class="panel-body">
                                <div class="clearfix m-b"><small class="text-muted pull-right">5m ago</small> <a href="#" class="thumb-sm pull-left m-r">
                                    <img src="images/avatar.jpg" class="img-circle">
                                </a>
                                    <div class="clear"><a href="#"><strong>Jonathan Omish</strong></a> <small class="block text-muted">San Francisco, USA</small> </div>
                                </div>
                                <p>There are a few easy ways to quickly get started with Bootstrap... </p>
                                <small class=""><a href="#"><i class="icon-comment-alt"></i>Comments (25)</a> </small></div>
                            <footer class="panel-footer pos-rlt"><span class="arrow top"></span>
                                <form class="pull-out">
                                    <input type="text" class="form-control no-border input-lg text-sm" placeholder="Write a comment...">
                                </form>
                            </footer>
                        </section>
                    </div>
                    <div class="col-lg-6 col-sm-6">
                        <section class="panel">
                            <div class="panel-body">
                                <div class="clearfix m-b"><small class="text-muted pull-right">1hr ago</small> <a href="#" class="thumb-sm pull-left m-r">
                                    <img src="images/avatar_default.jpg" class="img-circle">
                                </a>
                                    <div class="clear"><a href="#"><strong>Mike Mcalidek</strong></a> <small class="block text-muted">Newyork, USA</small> </div>
                                </div>
                                <div class="pull-in bg-light clearfix m-b-n">
                                    <p class="m-t-sm m-b text-center animated bounceInDown"><i class="icon-map-marker text-danger icon-4x" data-toggle="tooltip" title="checked in at Newyork"></i></p>
                                </div>
                            </div>
                            <footer class="panel-footer pos-rlt"><span class="arrow top"></span>
                                <form class="pull-out">
                                    <input type="text" class="form-control no-border input-lg text-sm" placeholder="Write a comment...">
                                </form>
                            </footer>
                        </section>
                    </div>
                </div>
                <section class="panel no-borders hbox">
                    <aside class="bg-info lter r-l text-center v-middle">
                        <div class="wrapper"><i class="icon-dribbble icon-4x"></i>
                            <p class="text-muted"><em>dribbble invitation</em></p>
                        </div>
                    </aside>
                    <aside>
                        <div class="pos-rlt"><span class="arrow left hidden-xs"></span>
                            <div class="panel-body">
                                <div class="clearfix m-b"><small class="text-muted pull-right">2 days ago</small> <a href="#" class="thumb-sm pull-left m-r">
                                    <img src="images/avatar.jpg" class="img-circle">
                                </a>
                                    <div class="clear"><a href="#"><strong>Jonathan Omish</strong></a> <small class="block text-muted">San Francisco, USA</small> </div>
                                </div>
                                <p>Thank you for invite... </p>
                                <small class=""><a href="#"><i class="icon-comment-alt"></i>Comments (10)</a> </small></div>
                            <footer class="panel-footer">
                                <form class="pull-out b-t">
                                    <input type="text" class="form-control no-border input-lg text-sm" placeholder="Write a comment...">
                                </form>
                            </footer>
                        </div>
                    </aside>
                </section>
                <section class="panel no-borders hbox">
                    <aside>
                        <div class="pos-rlt"><span class="arrow right hidden-xs"></span>
                            <div class="panel-body">
                                <div class="clearfix m-b"><small class="text-muted pull-right">2 days ago</small> <a href="#" class="thumb-sm pull-left m-r">
                                    <img src="images/avatar.jpg" class="img-circle">
                                </a>
                                    <div class="clear"><a href="#"><strong>Jonathan Omish</strong></a> <small class="block text-muted">San Francisco, USA</small> </div>
                                </div>
                                <p>Flat design is more popular today. Google, Microsoft, Apple... </p>
                                <small class=""><a href="#"><i class="icon-share"></i>Share (10)</a> </small></div>
                            <footer class="panel-footer">
                                <form class="pull-out b-t">
                                    <input type="text" class="form-control no-border input-lg text-sm" placeholder="Write a comment...">
                                </form>
                            </footer>
                        </div>
                    </aside>
                    <aside class="bg-primary clearfix lter r-r text-right v-middle">
                        <div class="wrapper h3 font-thin">7 things you need to know about the flat design </div>
                    </aside>
                </section>
                <div class="text-center m-b"><i class="icon-spinner icon-spin"></i></div>
            </div>
            <div class="col-lg-4">
                <section class="panel bg-info lter no-borders">
                    <div class="panel-body"><a class="pull-right" href="#"><i class="icon-map-marker"></i></a><span class="h4">McLean, VA</span>
                        <div class="text-center padder m-t"><span class="h1"><i class="icon-cloud text-muted"></i>68°</span> </div>
                    </div>
                    <footer class="panel-footer lt">
                        <div class="row">
                            <div class="col-xs-4"><small class="text-muted block">Humidity</small> <span>56 %</span> </div>
                            <div class="col-xs-4"><small class="text-muted block">Precip.</small> <span>0.00 in</span> </div>
                            <div class="col-xs-4"><small class="text-muted block">Winds</small> <span>7 mp</span> </div>
                        </div>
                    </footer>
                </section>
                <section class="panel no-borders">
                    <header class="panel-heading bg-success lter"><span class="pull-right">Friday</span> <span class="h4">$540<br>
                        <small class="text-muted">+1.05(2.15%)</small> </span>
                        <div class="text-center padder m-b-n-sm m-t-sm">
                            <div class="sparkline" data-type="line" data-resize="true" data-height="65" data-width="100%" data-line-width="2" data-line-color="#fff" data-spot-color="#fff" data-fill-color="" data-highlight-line-color="#fff" data-spot-radius="3" data-data="[220,210,200,325,250,320,345,250,250,250,400,380]"></div>
                            <div class="sparkline inline" data-type="bar" data-height="45" data-bar-width="6" data-bar-spacing="10" data-bar-color="#92cf5c">9,9,11,10,11,10,12,10,9,10,11,9,8</div>
                        </div>
                    </header>
                    <div class="panel-body">
                        <div><span class="text-muted">Sales in June:</span> <span class="h3 block">$2500.00</span> </div>
                        <div class="row m-t-sm">
                            <div class="col-xs-4"><small class="text-muted block">From market</small> <span>$1500.00</span> </div>
                            <div class="col-xs-4"><small class="text-muted block">Referal</small> <span>$600.00</span> </div>
                            <div class="col-xs-4"><small class="text-muted block">Affiliate</small> <span>$400.00</span> </div>
                        </div>
                    </div>
                </section>
                <section class="panel">
                    <div class="text-center wrapper">
                        <div class="sparkline inline" data-type="pie" data-height="150" data-slice-colors="['#acdb83','#f2f2f2','#fb6b5b']">25000,23200,15000</div>
                    </div>
                    <ul class="list-group list-group-flush no-radius alt">
                        <li class="list-group-item"><span class="pull-right">25,000</span> <span class="label bg-success">1</span> .inc company </li>
                        <li class="list-group-item"><span class="pull-right">23,200</span> <span class="label bg-danger">2</span> Gamecorp </li>
                        <li class="list-group-item"><span class="pull-right">15,000</span> <span class="label bg-light">3</span> Neosoft company </li>
                    </ul>
                </section>
                <section class="panel clearfix">
                    <div class="panel-body"><a href="#" class="thumb pull-left m-r">
                        <img src="images/avatar.jpg" class="img-circle">
                    </a>
                        <div class="clear"><a href="#" class="text-info">@Mike Mcalidek <i class="icon-twitter"></i></a><small class="block text-muted">2,415 followers / 225 tweets</small> <a href="#" class="btn btn-xs btn-success m-t-xs">Follow</a> </div>
                    </div>
                </section>--%>
                </div>
                </div>
            </section>

        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
