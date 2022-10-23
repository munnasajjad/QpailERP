<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/app/MasterPage.master"  CodeFile="Stock-out.aspx.cs" Inherits="app_Stock_out" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">
                        

    <div class="container-fluid">
    <div class="row-fluid">
               <div class="span12">                  
                   <h3 class="page-title">Old Parts Stock-Out</h3>
                   <div class="portlet box blue">
                     <div class="portlet-title">
                        <h4>Stock Out Parts Entry</h4>
                         <div class="tools">
                           <a href="javascript:;" class="collapse"></a>
                           <a href="#portlet-config" data-toggle="modal" class="config"></a>
                           <a href="javascript:;" class="reload"></a>
                           <a href="javascript:;" class="remove"></a>
                        </div>
                     </div>
                       <div class="portlet-body form">

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="" ></asp:Label>
                               
   
         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>

     <table class="table1">
                  
         <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Stock Out Location : "></asp:Label>
                </td>
                <td>    
                    <asp:DropDownList ID="ddCustomer" runat="server" AppendDataBoundItems="true" Width="100%"
                         CssClass="span6 m-wrap chosen" AutoPostBack="True" onselectedindexchanged="DropDownList2_SelectedIndexChanged">
                    </asp:DropDownList>
                    
                </td>                            
                <td>
                    
                    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT DISTINCT [Warehouse] FROM [Stock] WHERE OutQuantity>0 AND ([Status] &lt;&gt; @Status) ORDER BY [Warehouse]">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="New" Name="Status" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                               
                </td>
            </tr>

         
         <tr>
            <td>
                <asp:Label ID="lblNatID" runat="server" Text="Product Name : "></asp:Label>
            </td>
            <td>
                <%--<ajaxToolkit:ComboBox ID="ComboBox1" runat="server" AutoPostBack="False" DropDownStyle="DropDown" AutoCompleteMode="Suggest"></ajaxToolkit:ComboBox>--%>
                <asp:DropDownList ID="ddProduct" runat="server"  Width="100%"  CssClass="span6 m-wrap chosen" AutoPostBack="true" OnSelectedIndexChanged="ddProduct_SelectedIndexChanged1" >
                </asp:DropDownList>
                </td>
            <td>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT DISTINCT [PartName] FROM [Stock] WHERE Warehouse=@Warehouse AND OutQuantity>0 AND ([Status] &lt;&gt; @Status) ORDER BY [PartName]">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddCustomer" Name="Warehouse" PropertyName="SelectedValue" />
                        <asp:Parameter DefaultValue="New" Name="Status" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>


        <tr><%--Origin --%>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Serial# : "></asp:Label>
            </td>
            <td>
               <asp:DropDownList ID="ddSl" runat="server" AutoPostBack="true"  Width="100%"  CssClass="span6 m-wrap chosen" OnSelectedIndexChanged="ddSl_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [ItemSerialNo] FROM [Stock] WHERE ([Status] &lt;&gt; @Status) ORDER BY [EntryID]">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="New" Name="Status" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>


                </td>
            <td>
                &nbsp;</td>
        </tr>

         <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Text="Validity : "></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblSummery" runat="server" EnableViewState="false" ></asp:Label>
                </td>
            <td>
                &nbsp;</td>
        </tr>

         <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Invoice No. : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtInv" runat="server" ReadOnly="True"></asp:TextBox>
                </td>
            <td>
                &nbsp;</td>
        </tr>
         <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text="P. Bill No. : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtBill" runat="server" ReadOnly="True"></asp:TextBox>
                </td>
            <td>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
             </td>
        </tr>

        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Purchase From : "></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddVendor" runat="server" DataSourceID="SqlDataSource3" Enabled="false"
                                    DataTextField="Company" DataValueField="Company" AppendDataBoundItems="True" AutoPostBack="true"
                                     Class="chzn-select"  >
                                    <asp:ListItem>Select</asp:ListItem>
                                </asp:DropDownList>
            </td>
            <td>
                <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                    SelectCommand="SELECT [Company] FROM [Party] where Type='vendor'  ORDER BY [Company] ">
                                </asp:SqlDataSource>
             </td>
        </tr>
        
        <tr>
        <td>
                            <label class="control-label">Changing Place</label>
                            </td><td>
                                </div>
                        </div>
                                <asp:DropDownList ID="DropDownList3" runat="server" Width="200px" CssClass="span6 m-wrap">
                                    <asp:ListItem>Warehouse</asp:ListItem>
                                </asp:DropDownList>
            </td>
                        <td> 
                            
            </td>
        </tr>
        
        
        <tr>
        <td>
                            <label class="control-label">Item Status</label>
                            </td><td>
                                </div>
                        </div>
                                <asp:DropDownList ID="DropDownList4" runat="server" Width="200px" CssClass="span6 m-wrap">
                                    <asp:ListItem>Old(Waist)</asp:ListItem>
                                    <asp:ListItem>Old-Repaired(Usable)</asp:ListItem>
                                    <asp:ListItem>New</asp:ListItem>
                                </asp:DropDownList>
            </td>
                        <td> 
                            
            </td>
        </tr>
        
         <tr>
             <td>
                 <asp:Label ID="lblNatID2" runat="server" Text="Item Value : "></asp:Label>
             </td>
             <td>
                 <asp:TextBox ID="txtPrice" runat="server"></asp:TextBox>
                 <asp:FilteredTextBoxExtender ID="txtPrice_FilteredTextBoxExtender" 
                     runat="server" TargetControlID="txtPrice" ValidChars="0123456789.">
                 </asp:FilteredTextBoxExtender>
             </td>
             <td>
                 &nbsp;</td>
         </tr>

         

    </table>
    
    </ContentTemplate> 
</asp:UpdatePanel> 
    
    
    
<div class="form-actions">
<asp:Button ID="btnSave" runat="server" Text="Stock Out this item" onclick="btnSave_Click" CssClass="btn blue" />
<%--<asp:Button ID="btnPrint" runat="server" Text="Print Employee" PostBackUrl="~/HRM/rptAllEmployee.aspx" />--%>
<%--<asp:Button ID="Btn" runat="server" Text="Print-All" />--%>
    <asp:Button ID="Button2" runat="server" CssClass="btn orange" Text="Refresh" />

     
                           </div>




<fieldset>
    <legend>List of Old Items
    <%--<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSource1" AllowPaging="True" 
        AllowSorting="True" BackColor="White" BorderColor="#999999" BorderStyle="Solid" 
        BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">
        <Columns>
            <asp:BoundField DataField="PartName" HeaderText="PartName" SortExpression="PartName" />
            <asp:BoundField DataField="ItemSerialNo" HeaderText="ItemSerialNo" 
                SortExpression="ItemSerialNo" />
            <asp:BoundField DataField="Warehouse" HeaderText="Warehouse" 
                SortExpression="Warehouse" />
            <asp:BoundField DataField="Status" HeaderText="Status" 
                SortExpression="Status" />
            <asp:BoundField DataField="Warrenty" HeaderText="Warrenty" 
                SortExpression="Warrenty" />
            <asp:BoundField DataField="PurchaseBillNo" HeaderText="PurchaseBillNo" SortExpression="PurchaseBillNo" />
            <asp:BoundField DataField="EntryDate" HeaderText="EntryDate" SortExpression="EntryDate" />
        </Columns>
        <FooterStyle BackColor="#CCCCCC" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="#CCCCCC" />
    </asp:GridView>
    
</fieldset>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
            
            SelectCommand="SELECT [PartName], [ItemSerialNo], [Warehouse], [Status], [Warrenty], [PurchaseBillNo], [EntryDate] FROM [Stock] WHERE ([Status] &lt;&gt; @Status) ORDER BY [EntryID]">
    <SelectParameters>
        <asp:Parameter DefaultValue="New" Name="Status" Type="String" />
    </SelectParameters>
                           </asp:SqlDataSource>--%>
       
       
       
</div></div></div></div></div>

       

</asp:Content>

