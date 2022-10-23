<%@ Page Language="C#" Title=" Billing & Receipt" AutoEventWireup="true" MasterPageFile="~/app/Layout.Master" CodeBehind="Billing.aspx.cs" Inherits="Oxford.app.Billing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <script type="text/javascript">

         $(window).load(function () {
             jScript();

             //readonly inputs
             $('#<%=txtTTL.ClientID%>').attr('readonly', true);
         });

         function jScript() {

             $(".txtMult input").keyup(multInputs);

             function multInputs() {
                 var mult1 = 0; 
                 // for each row:
                 $("tr.txtMult").each(function () {
                     // get the values from this row:
                     var $val1 = $('.val1', this).val();
                     mult1 += ($val1 * 1);//Total Qty ctl00_BodyContent_ltrQty
                 });
                 $("#ctl00_bodycontent_txtTTL").val(mult1);}
         }

            
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>


    <asp:UpdatePanel ID="uPanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>
            
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(jScript);
            </script>


            <div class="col-lg-6">
                <section class="panel">

                                <fieldset>
                                    <legend><b>»</b> Billing & Receipt</legend>

                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                        <asp:Label ID="Label2" runat="server" EnableViewState="false"></asp:Label>

                                    <div class="portlet-body form">
                                        <div class="form-horizontal">

                                            <div class="control-group">
                                                <label class="control-label">Invoice No. : </label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtVID" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Invoice Date : </label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtDate" runat="server"  CssClass="form-control"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                        Enabled="True" TargetControlID="txtDate">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Class : </label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddClass" runat="server" data-placeholder="--- Select ---" TabIndex="1" 
                                                        OnSelectedIndexChanged="ddClass_SelectedIndexChanged" AutoPostBack="true"  CssClass="form-control" DataSourceID="SqlDataSource1" DataTextField="Name" DataValueField="sl">
                                                    </asp:DropDownList>

                                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT [sl], [Name] FROM [Class] ORDER BY [Name]"></asp:SqlDataSource>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Student : </label>
                                                <div class="controls">
                                                    <asp:DropDownList ID="ddStudent" runat="server" data-placeholder="--- Select ---" TabIndex="1" 
                                                        DataSourceID="SqlDataSource5" DataTextField="StudentNameE" DataValueField="StudentID" CssClass="form-control"
                                                        OnSelectedIndexChanged="ddStudent_SelectedIndexChanged"  AutoPostBack="true">
                                                    </asp:DropDownList>

                                                    <asp:SqlDataSource ID="SqlDataSource5" runat="server"
                                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT [StudentNameE], [StudentID] FROM [Students] WHERE ([Class] = @Class) ORDER BY [StudentNameE]">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="ddClass" Name="Class" PropertyName="SelectedValue" Type="String" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                    <asp:Literal ID="ltrID" runat="server" ></asp:Literal>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Collected Amount : </label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtTTL" runat="server" CssClass="form-control" Text="0" ></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Remarks : </label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtRemarks"  CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label"> </label>
                                                <div class="controls">
                                                <asp:CheckBox ID="chkPrint" runat="server" Checked="false" Text="Print" Visible="false" />
                                                <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" Text="Save Collection" OnClick="btnSave_Click" />
                                                <asp:Button ID="btnClear" CssClass="btn btn-s-md btn-white" runat="server" Text="Cancel" OnClick="btnClear_Click" />
                                                    </div>
                                            </div>

                                    </div>
                                </fieldset>

                        <div class="row-fluid">
                            <div class="span12">
                                <div class="portlet box yellow">
                                    <div class="portlet-title">
                                        <h4 style="text-align: center;"><i class="icon-coffee"></i>Todays All Collections</h4>
                                        <div class="tools">
                                            <a href="javascript:;" class="collapse"></a>
                                            <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                            <a href="javascript:;" class="reload"></a>
                                            <a href="javascript:;" class="remove"></a>
                                        </div>
                                    </div>
                                    <div class="portlet-body">

                                        <asp:GridView ID="GridView1" runat="server" Width="100%" AllowSorting="True"
                                            AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                                            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black"
                                            GridLines="Vertical" DataSourceID="SqlDataSource3">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Invoice No." SortExpression="VoucherNo">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%#"./Reports/rptInvoice.aspx?inv=" + Eval("InvoiceNo") %>'>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("InvoiceNo") %>'></asp:Label>
                                                        </asp:HyperLink>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:d}"
                                                    ItemStyle-HorizontalAlign="Center" SortExpression="VoucherDate" Visible="False" />
                                                <asp:BoundField DataField="StudentID" HeaderText="Student ID" ItemStyle-HorizontalAlign="Center" SortExpression="StudentID" />
                                                <asp:BoundField DataField="StudentName" HeaderText="Student Name" ItemStyle-HorizontalAlign="Center" SortExpression="StudentID" />
                                                <asp:BoundField DataField="Class" HeaderText="Class" ItemStyle-HorizontalAlign="Center" SortExpression="StudentID" />
                                                <asp:BoundField DataField="Roll" HeaderText="Roll" ItemStyle-HorizontalAlign="Center" SortExpression="StudentID" />
                                                <asp:BoundField DataField="Section" HeaderText="Sec." ItemStyle-HorizontalAlign="Center" SortExpression="StudentID" />
                                                <asp:BoundField DataField="InvoiceAmount" HeaderText="Amount" ItemStyle-HorizontalAlign="Right" SortExpression="InvoiceAmount" />
                                            </Columns>
                                            <FooterStyle BackColor="#CCCCCC" />
                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#484073" Font-Bold="True" ForeColor="#ffffff" />
                                            <AlternatingRowStyle BackColor="#CCCCCC" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
                                            SelectCommand="SELECT VID, InvoiceNo, InvoiceDate, StudentID, StudentName, Class, Roll, Section, InvoiceAmount, Remarks FROM [BillingMaster] WHERE ([InvoiceDate] = @InvoiceDate) ORDER BY [VID] DESC">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="txtDate" Name="InvoiceDate"
                                                    PropertyName="Text" Type="DateTime" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                    </div>
                                </div>
                            </div>
                        </div>
                </section>
            </div>

            <div class="col-lg-6">
                <section class="panel">

                        <fieldset class="field_set">
                            <legend><b>»</b> Collection</legend>

                            <asp:Panel ID="pnl3" runat="server" DefaultButton="btnAdd">

                                <table class="table1" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" Text="Collection Group : "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddGroup" runat="server" AutoPostBack="true" CssClass="form-control" 
                                        DataSourceID="SqlDataSource7" DataTextField="name" DataValueField="sl"
                                        OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [Name] FROM [CollectionTypes] order by sl"></asp:SqlDataSource>

                                        </td>
                                        <td>
                                            <asp:Button ID="btnRefresh" runat="server" Text="R..." OnClick="btnRefresh_Click" CssClass="btn" Width="35px" />                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Collection Head : "></asp:Label>

                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddHead" runat="server" CssClass="form-control" AutoPostBack="true"
                                                DataSourceID="SqlDataSource2" DataTextField="name" DataValueField="sl" OnSelectedIndexChanged="ddHead_SelectedIndexChanged">

                                            </asp:DropDownList>                                            
                                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT [sl], [Name] FROM [CollectionHeads] WHERE ([GroupID] = @GroupID) ORDER BY [Name]">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="ddGroup" Name="GroupID" PropertyName="SelectedValue" Type="String" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                            <asp:Label ID="lblSrl" runat="server" Text="" Visible="false"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="Description : "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>


                                    <tr>
                                        <td>Amount</td>
                                        <td>
                                            <asp:TextBox ID="txtAmount" runat="server" Width="100px"  CssClass="form-control" ></asp:TextBox>
                                            <span style="color:red"><asp:Literal ID="ltrNotice" runat="server" EnableViewState="False"></asp:Literal></span>
                                            <%--<img src="../Libs/Calculator/calculator.png" onclick="calculator()" />--%>
                                            <asp:FilteredTextBoxExtender ID="txtOpBalance_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtAmount">
                                            </asp:FilteredTextBoxExtender>
                                           </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>
                                            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-s-md btn-info" Text="Add Data to Grid" OnClick="btnAdd_Click" />
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>


                                    <tr>
                                        <td colspan="3">

                                            <asp:GridView ID="GridView2" runat="server"
                                                OnRowDataBound="GridView2_RowDataBound" Width="100%" AutoGenerateColumns="False"
                                                OnRowDeleting="GridView2_RowDeleting" OnSelectedIndexChanged="GridView2_SelectedIndexChanged" BackColor="White" BorderColor="#DEDFDE"
                                                BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                                GridLines="Vertical" DataSourceID="SqlDataSource4">
                                                <RowStyle BackColor="#F7F7DE" />
                                                <Columns>

                                                    <asp:TemplateField ItemStyle-Width="40px">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="40px" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Sl." SortExpression="CrID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSl" runat="server" Text='<%# Bind("SerialNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="C.Group" SortExpression="CrID" >
                                                        <ItemTemplate>
                                                            <asp:Label ID="CollectionGroup" runat="server" Text='<%# Bind("CollectionGroup") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText="C.Head" SortExpression="CrID" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="acHeadID" runat="server" Text='<%# Bind("CollectionHead") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="C.Head" SortExpression="CrID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="CollectionHead" runat="server" Text='<%# Bind("CollectionHeadName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                     <asp:TemplateField HeaderText="Description" SortExpression="CrID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Description" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Amount" SortExpression="CrID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAmt" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText="Discount" SortExpression="CrID" >
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDisc" runat="server" Text='<%# Bind("Discount") %>' ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Received" SortExpression="CrID">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtReceived" runat="server" Text='<%# Bind("Due") %>' Width="60px" CssClass="val1 text-right"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
                                                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />

                                                            <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                            </asp:ConfirmButtonExtender>
                                                            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                                PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                            <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                                <b style="color: red">Entry will be deleted!</b><br />
                                                                Are you sure you want to delete the item from entry list?
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
                                                <RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
                                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                <SelectedRowStyle BackColor="#EEF7F2" Font-Bold="True" ForeColor="#615B5B" />
                                                <HeaderStyle BackColor="#96D9FF" Font-Bold="True" ForeColor="white" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT SerialNo, 
                                                (Select Name from CollectionTypes where sl=BillingTmp.CollectionGroup) AS CollectionGroup, CollectionHead,
                                                (Select Name from CollectionHeads where sl=BillingTmp.CollectionHead) AS CollectionHeadName, 
                                                Description, Amount, Discount, Received, Due FROM [BillingTmp] WHERE ([StudentID] = @StudentID) AND IsPaid='P' ORDER BY [SerialNo]">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddStudent" Name="StudentID" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </td>
                                    </tr>
                                </table>

                            </asp:Panel>

                        </fieldset>
                   
                        <div class="row-fluid">
                            <div class="span12">
                                <div class="portlet box yellow">
                                    <div class="portlet-title">
                                        <h4 style="text-align: center;"><i class="icon-coffee"></i>Previous Collection History</h4>                                        
                                    </div>
                                    <div class="portlet-body">

                                        <asp:GridView ID="GridView3" runat="server" Width="100%" AllowSorting="True"
                                            AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                                            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black"
                                            GridLines="Vertical" DataSourceID="SqlDataSource6">
                                            <Columns>
                                                
                                                    <asp:TemplateField ItemStyle-Width="40px">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="40px" />
                                                    </asp:TemplateField>

                                                <asp:BoundField DataField="EntryDate" HeaderText="Date" DataFormatString="{0:d}"
                                                    ItemStyle-HorizontalAlign="Center" SortExpression="EntryDate" />
                                                <asp:BoundField DataField="CollectionGroup" HeaderText="C.Type"
                                                    ItemStyle-HorizontalAlign="Center" SortExpression="CollectionGroup" />
                                                <asp:BoundField DataField="CollectionHead" HeaderText="C.Head"
                                                    ItemStyle-HorizontalAlign="Right" SortExpression="CollectionHead" />
                                                <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                                            </Columns>
                                            <FooterStyle BackColor="#CCCCCC" />
                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="#F7F7DE" />
                                            <AlternatingRowStyle BackColor="#CCCCCC" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [EntryDate], 
                                                (Select Name from CollectionTypes where sl=BillingTmp.CollectionGroup) AS CollectionGroup, 
                                                (Select Name from CollectionHeads where sl=BillingTmp.CollectionHead) AS CollectionHead, [Amount] FROM [BillingTmp] WHERE  ([StudentID] = @StudentID) AND IsPaid='A' ORDER BY [SerialNo]">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddStudent" Name="StudentID" PropertyName="SelectedValue" Type="String" />                                                
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                    </div>
                                </div>
                            </div>
                        </div>

                </section>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
