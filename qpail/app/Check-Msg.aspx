<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Check-Msg.aspx.cs" Inherits="AdminCentral_Check_Msg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    --%>

    <div class="grid_12 full_block">
        <div class="widget_wrap">
            <div class="widget_top">
                <span class="h_icon list_image"></span>
                <h6>Messages for 
                            <asp:Label ID="lblBranch" runat="server" Text="" ></asp:Label></h6>
            </div>
            <div class="widget_content">

                <div class="form_container left_label">

                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                        SelectCommand="SELECT MsgID, Sender, Receiver, Subject, BodyText, IsRead, IsFlag, EntryDate
                                             FROM [Messaging] where Receiver=@Receiver ORDER BY [MsgID] DESC">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="lblBranch" Name="Receiver" PropertyName="Text" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    

                    <br />
                    <asp:ListView ID="ListView1" runat="server" DataSourceID="SqlDataSource1">
                        <ItemTemplate>
                            <span style="background-color: #FFF8D0; color: #000000;">
                                <hr />
                                <h3>
                                    <asp:Label ID="HeadlineLabel" runat="server" Text='<%# Eval("Subject") %>' />
                                </h3>
                                <p>
                                    Send by: <b>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Sender") %>' /></b> on 
            <asp:Label ID="PublishDateLabel" runat="server" Text='<%# Eval("EntryDate") %>' />
                                </p>
                                <p>
                                    <asp:Label ID="FullNewsLabel" runat="server" Text='<%# Eval("BodyText") %>' />
                                </p>
                                <br />
                                <i><a href="#">Mark as read.</a></i>
                                <hr />
                            </span>
                        </ItemTemplate>
                       
                        <EmptyDataTemplate>
                            <span>No data was returned.</span>
                        </EmptyDataTemplate>
                       
                        <LayoutTemplate>
                            <div style="font-family: Verdana, Arial, Helvetica, sans-serif;"
                                id="itemPlaceholderContainer" runat="server">
                                <span id="itemPlaceholder" runat="server" />
                            </div>
                            <div style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;">
                                <asp:DataPager ID="DataPager1" runat="server">
                                    <Fields>
                                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True"
                                            ShowLastPageButton="True" />
                                    </Fields>
                                </asp:DataPager>
                            </div>
                        </LayoutTemplate>
                       
                    </asp:ListView>



                </div>
            </div>
        </div>
    </div>


</asp:Content>

