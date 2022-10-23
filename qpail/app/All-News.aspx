<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="All-News.aspx.cs" Inherits="AdminCentral_All_News" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    --%>
    
            <div class="grid_12 full_block">
				<div class="widget_wrap">
					<div class="widget_top">
						<span class="h_icon list_image"></span>
						<h6>My News Postings</h6>
					</div>
					<div class="widget_content">
						<h3>All News </h3>
						<%--<p>
							 Cras erat diam, consequat quis tincidunt nec, eleifend a turpis. Aliquam ultrices feugiat metus, ut imperdiet erat mollis at. Curabitur mattis risus sagittis nibh lobortis vel.
						</p>--%>
						<p style="float:right; text-align:right; color:Red;">
						<asp:Label ID="lblMsg" CssClass="msg" runat="server" EnableViewState="false"></asp:Label>
						<asp:Label ID="lblErrLoad" runat="server" CssClass="msg" EnableViewState="false"></asp:Label>
						</p>
						
						<div class="form_container left_label">
							
							
				<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
            
                    SelectCommand="SELECT [Headline], [PublishDate], [FullNews] FROM [NewsUpdates] where Msgfor='News' ORDER BY [MsgID] DESC">
			</asp:SqlDataSource>
    <br />
    <asp:ListView ID="ListView1" runat="server" DataSourceID="SqlDataSource1">
        <ItemTemplate>
            <span style="background-color: #FFF8D0;color: #000000;">
            <hr />
            <h3>
            <asp:Label ID="HeadlineLabel" runat="server" Text='<%# Eval("Headline") %>' />
            </h3>            
           <p>
            <asp:Label ID="PublishDateLabel" runat="server" Text='<%# Eval("PublishDate") %>' />
            </p>
            <p>
            <asp:Label ID="FullNewsLabel" runat="server" Text='<%# Eval("FullNews") %>' />
            </p>
            <br />
            <hr />
            </span>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <span style="background-color: #FFF8DC;">
            <hr />
            <h3>
            <asp:Label ID="HeadlineLabel" runat="server" Text='<%# Eval("Headline") %>' />
            </h3>            
           <p>
            <asp:Label ID="PublishDateLabel" runat="server" Text='<%# Eval("PublishDate") %>' />
            </p>
            <p>
            <asp:Label ID="FullNewsLabel" runat="server" Text='<%# Eval("FullNews") %>' />
            </p>
            <br />
            <hr />
            </span>
        </AlternatingItemTemplate>
        <EmptyDataTemplate>
            <span>No data was returned.</span>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <span style="">Headline:
            <asp:TextBox ID="HeadlineTextBox" runat="server" 
                Text='<%# Bind("Headline") %>' />
            <br />
            PublishDate:
            <asp:TextBox ID="PublishDateTextBox" runat="server" 
                Text='<%# Bind("PublishDate") %>' />
            <br />
            FullNews:
            <asp:TextBox ID="FullNewsTextBox" runat="server" 
                Text='<%# Bind("FullNews") %>' />
            <br />
            <asp:Button ID="InsertButton" runat="server" CommandName="Insert" 
                Text="Insert" />
            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                Text="Clear" />
            <br />
            <br />
            </span>
        </InsertItemTemplate>
        <LayoutTemplate>
                <div style="font-family: Verdana, Arial, Helvetica, sans-serif;" 
                ID="itemPlaceholderContainer" runat="server">
                    <span ID="itemPlaceholder" runat="server" />
                </div>
            <div style="text-align: center;background-color: #CCCCCC;font-family: Verdana, Arial, Helvetica, sans-serif;color: #000000;">
                <asp:DataPager ID="DataPager1" runat="server">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" 
                            ShowLastPageButton="True" />
                    </Fields>
                </asp:DataPager>
            </div>
            </LayoutTemplate>
            <EditItemTemplate>
                <span style="background-color: #008A8C;color: #FFFFFF;">Headline:
                <asp:TextBox ID="HeadlineTextBox" runat="server" 
                    Text='<%# Bind("Headline") %>' />
                <br />
                PublishDate:
                <asp:TextBox ID="PublishDateTextBox" runat="server" 
                    Text='<%# Bind("PublishDate") %>' />
                <br />
                FullNews:
                <asp:TextBox ID="FullNewsTextBox" runat="server" 
                    Text='<%# Bind("FullNews") %>' />
                <br />
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" 
                    Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                    Text="Cancel" />
                <br />
                <br />
                </span>
            </EditItemTemplate>
            <SelectedItemTemplate>
                <span style="background-color: #008A8C;font-weight: bold;color: #FFFFFF;">
                Headline:
                <asp:Label ID="HeadlineLabel" runat="server" Text='<%# Eval("Headline") %>' />
                <br />
                PublishDate:
                <asp:Label ID="PublishDateLabel" runat="server" 
                    Text='<%# Eval("PublishDate") %>' />
                <br />
                FullNews:
                <asp:Label ID="FullNewsLabel" runat="server" Text='<%# Eval("FullNews") %>' />
                <br />
                <br />
                </span>
            </SelectedItemTemplate>
    </asp:ListView>
							
							
							
						</div>
					</div>
				</div>
			</div>

    
</asp:Content>

