<%@ Page Title="Party Detail" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Party-Detail.aspx.cs" Inherits="app_Party_Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
     .grid_container li {
    font-size: large;
    line-height: 42px;
    padding: 30px;
    font-family: 'courier new', 'courier';
}
    .grid_container li span {
    font-weight: bold;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">
    
    <asp:ListView ID="ListView1" runat="server" DataSourceID="SqlDataSource1">
        
        <EmptyDataTemplate>
            No data was returned.
        </EmptyDataTemplate>
        
        <ItemSeparatorTemplate>
<br /><hr /><br/>
        </ItemSeparatorTemplate>
        <ItemTemplate>
            <li style="background-color: #DCDCDC;color: #000000;">PartyID:
                <asp:Label ID="PartyIDLabel" runat="server" Text='<%# Eval("PartyID") %>' />
                <br />
                Type:
                <b><asp:Label ID="TypeLabel" runat="server" Text='<%# Eval("Type") %>' /></b>
                <br />
                <b></b>Category:
                <asp:Label ID="CategoryLabel" runat="server" Text='<%# Eval("Category") %>' />
                <br />
                <b></b>Company:
                <asp:Label ID="CompanyLabel" runat="server" Text='<%# Eval("Company") %>' />
                <br />
                <b></b>Referrence Items:
                <asp:Label ID="ReferrenceItemsLabel" runat="server" Text='<%# Eval("ReferrenceItems") %>' />
                <br />
                <b></b>Zone:
                <asp:Label ID="ZoneLabel" runat="server" Text='<%# Eval("Zone") %>' />
                <br />
                <b></b>Referrer:
                <asp:Label ID="ReferrerLabel" runat="server" Text='<%# Eval("Referrer") %>' />
                <br />
                <b></b>Address:
                <asp:Label ID="AddressLabel" runat="server" Text='<%# Eval("Address") %>' />
                <br />
                <b></b>Phone:
                <asp:Label ID="PhoneLabel" runat="server" Text='<%# Eval("Phone") %>' />
                <br />
                <b></b>Email:
                <asp:Label ID="EmailLabel" runat="server" Text='<%# Eval("Email") %>' />
                <br />
                <b></b>Fax:
                <asp:Label ID="FaxLabel" runat="server" Text='<%# Eval("Fax") %>' />
                <br />
                <b></b>IM:
                <asp:Label ID="IMLabel" runat="server" Text='<%# Eval("IM") %>' />
                <br />
                <b></b>Website:
                <asp:Label ID="WebsiteLabel" runat="server" Text='<%# Eval("Website") %>' />
                <br />
                <b></b>Contact Person:
                <asp:Label ID="ContactPersonLabel" runat="server" Text='<%# Eval("ContactPerson") %>' />
                <br />
                <b></b>Mobile No:
                <asp:Label ID="MobileNoLabel" runat="server" Text='<%# Eval("MobileNo") %>' />
                <br />
                <b></b>Matuirity Days:
                <asp:Label ID="MatuirityDaysLabel" runat="server" Text='<%# Eval("MatuirityDays") %>' />
                <br />
                <b></b>Credit Limit:
                <asp:Label ID="CreditLimitLabel" runat="server" Text='<%# Eval("CreditLimit") %>' />
                <br />
                <b></b>Op. Balance:
                <asp:Label ID="OpBalanceLabel" runat="server" Text='<%# Eval("OpBalance") %>' />
                <br />
                <b></b>Photo URL:
                <asp:Label ID="PhotoURLLabel" runat="server" Text='<%# Eval("PhotoURL") %>' />
                <br />
                
                <b></b>Entry By:
                <asp:Label ID="EntryByLabel" runat="server" Text='<%# Eval("EntryBy") %>' />
                <br />
                <b></b>Entry Date:
                <asp:Label ID="EntryDateLabel" runat="server" Text='<%# Eval("EntryDate") %>' />
                <br />
                <b></b>Last Update By:
                <asp:Label ID="LastUpdateByLabel" runat="server" Text='<%# Eval("LastUpdateBy") %>' />
                <br />
                <b></b>Last Update Date:
                <asp:Label ID="LastUpdateDateLabel" runat="server" Text='<%# Eval("LastUpdateDate") %>' />
                <br />
                <b></b>Remarks:
                <asp:Label ID="RemarksLabel" runat="server" Text='<%# Eval("Remarks") %>' />
                <br />
                <b></b>TDS Terms:
                <asp:Label ID="TDS_TermsLabel" runat="server" Text='<%# Eval("TDS_Terms") %>' />
                <br />
                <b></b>VDS Terms:
                <asp:Label ID="VDS_TermsLabel" runat="server" Text='<%# Eval("VDS_Terms") %>' />
                <br />
            </li>
        </ItemTemplate>
        <LayoutTemplate>
            <ul id="itemPlaceholderContainer" runat="server" style="font-family: Verdana, Arial, Helvetica, sans-serif;">
                <li runat="server" id="itemPlaceholder" />
            </ul>
            <div style="text-align: center;background-color: #CCCCCC;font-family: Verdana, Arial, Helvetica, sans-serif;color: #000000;">
            </div>
        </LayoutTemplate>
        
    </asp:ListView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
        SelectCommand="SELECT PartyID, Type, Category, Company, ReferrenceItems, Zone, Referrer, Address, Phone, Email, Fax, IM, Website, ContactPerson, MobileNo, MatuirityDays, 
                         CreditLimit, OpBalance, PhotoURL, ProjectID, EntryBy, EntryDate, LastUpdateBy, LastUpdateDate, Remarks, TDS_Terms, VDS_Terms
 FROM [Party] WHERE ([PartyID] = @PartyID)">
        <SelectParameters>
            <asp:QueryStringParameter Name="PartyID" QueryStringField="pid" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" Runat="Server">
</asp:Content>

