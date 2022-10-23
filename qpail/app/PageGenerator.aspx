<%@ Page Title="Page Generator" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="PageGenerator.aspx.cs" Inherits="Application_PageGenerator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">

    <asp:Label ID="lblMsg"  runat ="server" EnableViewState="false" />

    <h2>Generate Page with CRUD Operations</h2>
            <div class="col-lg-8">
                <section class="panel">
    Table Name:
    <asp:DropDownList ID="ddTableName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddTableName_SelectedIndexChanged"></asp:DropDownList>
<br />Columns
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" >
        <Columns>
            <asp:BoundField DataField="ORDINAL_POSITION" HeaderText="SL" />
            <asp:BoundField DataField="COLUMN_NAME" HeaderText="COLUMN_NAME" />
            <asp:BoundField DataField="DATA_TYPE" HeaderText="DATA_TYPE" />
            <asp:BoundField DataField="IS_NULLABLE" HeaderText="IS_NULLABLE" />
            <asp:BoundField DataField="CHARACTER_MAXIMUM_LENGTH" HeaderText="MAX_LENGTH" />
            <asp:BoundField DataField="COLUMN_DEFAULT" HeaderText="DEFAULT" />
        </Columns>
    </asp:GridView>
<br />Relations
    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" >
        <Columns>

            <asp:TemplateField HeaderText="Primary Table">                
                <ItemTemplate>
                    <asp:Label ID="lblPTable" runat="server" Text='<%# Bind("Referenced_Table_Name") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Value Column">                
                <ItemTemplate>
                    <asp:Label ID="lblPColumn" runat="server" Text='<%# Bind("Referenced_Column_As_FK") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="DataTextField">
                <ItemTemplate>
                <asp:DropDownList ID="ddColumns" runat="server" Width="100%" ></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Linked Column">                
                <ItemTemplate>
                    <asp:Label ID="lblRefColumn" runat="server" Text='<%# Bind("Referencing_Column_Name") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    Main Menu:
    <asp:DropDownList ID="ddMainMenu" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddTableName_SelectedIndexChanged"></asp:DropDownList>
    
    Form Name:
    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>

    <asp:Button ID="btnSave" runat="server" Text="Generate" OnClick="btnSave_Click" />

                    </section>
                </div>
</asp:Content>

