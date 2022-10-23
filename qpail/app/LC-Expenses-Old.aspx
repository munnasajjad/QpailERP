<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="LC-Expenses-Old.aspx.cs" Inherits="app_LC_Expenses_Old" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
    <style type="text/css">
        .ui-accordion .ui-accordion-content {
            padding: 1em 0 !important;
        }

        .ui-accordion-content .form-group label, .ui-accordion-content .control-group label, .ui-accordion-content .control-group span {
            font-size: 11px;
            font-weight: normal;
            padding: 0;
            line-height: 32px;
            width: 40%;
        }

        .ui-accordion-content .control-group input {
            width: 60% !important;
            font-size: 11px;
            height: 28px;
            margin: 2px 0;
        }

        .ui-accordion-content .control-group:nth-child(odd) {
            background: #eee;
        }

        .ui-accordion-content .control-group {
            margin-bottom: 0;
            padding: 2px 1em;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">LC Expenses
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>LC Expenses Entry
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">
                                <asp:Label ID="lblMsg" runat="server"></asp:Label>


                                <div id="EditField" runat="server" class="control-group">
                                    <b style="color: #E84940">
                                        <asp:Label ID="lblEname" runat="server" Text="LC Number :"></asp:Label>
                                        <asp:DropDownList ID="ddName" runat="server" AppendDataBoundItems="True"
                                            AutoPostBack="True" DataSourceID="SqlDataSource1x"
                                            DataTextField="LCNo" DataValueField="sl"
                                            OnSelectedIndexChanged="ddName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource1x" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT sl, [LCNo] FROM [LC] WHERE (([ProjectID] = @ProjectID) AND ([IsActive] = 'A')) ORDER BY [sl] desc">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="ProjectID" SessionField="ProjectID" Type="Int32" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </b>
                                </div>


                                <div class="form-group">
                                    <label>Expense Type : </label>
                                    <asp:DropDownList ID="ddType" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource1"
                                        DataTextField="AccountsHeadName" DataValueField="AccountsHeadID" AutoPostBack="true" OnSelectedIndexChanged="ddType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT AccountsHeadID,[AccountsHeadName] FROM [HeadSetup] WHERE ([ControlAccountsID] = '040110') ORDER BY [AccountsHeadName]"></asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Expense Head :  </label>
                                    <div class="controls">
                                        <div class="controls">
                                            <asp:DropDownList ID="ddHead" runat="server" DataSourceID="SqlDataSource17" DataTextField="HeadName" DataValueField="HeadID">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource17" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [HeadID], [AccHeadID], [HeadName] FROM [ExpenseHeads] WHERE ([AccHeadID] = @AccHeadID) ORDER BY [HeadName]">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddType" Name="AccHeadID" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Expense Date :  </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtDate" runat="server" Enabled="True"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                            Enabled="True" TargetControlID="txtDate">
                                        </asp:CalendarExtender>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Amount (BDT) :  </label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtAmount" runat="server" Enabled="True"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtAmount">
                                        </asp:FilteredTextBoxExtender>

                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Exp. Description :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtDescription" runat="server" Enabled="True"></asp:TextBox>
                                    </div>
                                </div>


                                <div class="form-actions">
                                    <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save Expense" OnClick="btnSave_Click" />
                                </div>


                                <div class="control-group">

                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowDeleting="GridView1_RowDeleting"
                                        DataSourceID="SqlDataSource10" Width="100%" DataKeyNames="esl">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>

                                                <ItemStyle Width="20px"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="sl" SortExpression="sl" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("esl") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Expense Date" SortExpression="Expdate">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1x" runat="server" Text='<%# Bind("Expdate","{0:d}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Head Name" SortExpression="Size">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1xz" runat="server" Text='<%# Bind("HeadName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount" SortExpression="QTY1">
                                                <ItemTemplate>
                                                    <asp:Label ID="CFRBDT" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" SortExpression="QTY1">
                                                <ItemTemplate>
                                                    <asp:Label ID="desc2" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField HeaderText="Ship Date" SortExpression="CFRValue">
                                    <ItemTemplate>
                                        <asp:Label ID="ShipDate" runat="server" DataFormatString="{0:d}" Text='<%# Bind("ShipDate","{0:d}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>


                                    <asp:SqlDataSource ID="SqlDataSource10" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT esl, Expdate, (SELECT HeadName FROM ExpenseHeads where HeadID=  LC_Expenses.HeadID) as HeadName, Amount, Description
                             from  LC_Expenses Where LCNo=@LCNo AND TypeID=@TypeID ORDER BY [esl]"
                                        DeleteCommand="DELETE LcItems WHERE EntryID=@EntryID"
                                        UpdateCommand="">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddName" Name="LCNo" PropertyName="SelectedValue" Type="String" />
                                            <asp:ControlParameter ControlID="ddType" Name="TypeID" PropertyName="SelectedValue" Type="String" />
                                        </SelectParameters>
                                        <DeleteParameters>
                                            <asp:Parameter Name="EntryID" />
                                        </DeleteParameters>
                                    </asp:SqlDataSource>


                                </div>

                            </div>
                        </div>
                    </div>
                </div>




                <%--Right Panel--%>





                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Expense List
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">


                                <div id="accordion">

                                    <h3>LC General Info</h3>
                                    <div>


                                        <div id="Div2" runat="server" class="control-group">
                                            <label class="control-label">LC/TT No.</label>
                                            <div class="controls">
                                                <asp:TextBox ID="lblLCNo" runat="server" />
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">L/C Type :  </label>
                                            <div class="controls">
                                                <div class="controls">
                                                    <asp:TextBox ID="lblLCType" runat="server" Enabled="false" />

                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Open Date :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="lblOpDate" runat="server" />
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Item group :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="lblGrp" runat="server" Enabled="false" />
                                            </div>
                                        </div>

                                        <%--<div class="control-group">
                            <label class="control-label">Item Name :  </label>
                            <div class="controls">
                                <asp:TextBox ID="lblItem" runat="server" enabled="True"></label>
                            </div>
                        </div>

                        <div class="control-group">
                            <label class="control-label">HS Code :  </label>
                            <div class="controls">
                                <asp:TextBox ID="TextBox4" runat="server" enabled="True"></label>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">Reference/ Pack Size :  </label>
                            <div class="controls">
                                <asp:TextBox ID="TextBox5" runat="server" enabled="True"></label>
                            </div>
                        </div>--%>
                                        <div class="control-group">
                                            <label class="control-label">For Company/Dept :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="lblDept" runat="server" />
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Expiry Date :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="lblExDate" runat="server" />
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Ship Date :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="lblShipDate" runat="server" />
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Manufacturer :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="lblSupplier" runat="server" Enabled="false" />
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Country of Origin :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="lblCountry" runat="server" Enabled="false" />
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Agent/ Shipper :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="lblagent" runat="server" Enabled="false" />
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">Total Weight/Qnty :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtTtlQty" runat="server" Enabled="false" />
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Freight Amt :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtFreight" runat="server" Enabled="false" />
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Mode of Transport :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="ddMode" runat="server" Enabled="false" />
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Remark :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtRemarks" runat="server" Enabled="false" />
                                            </div>
                                        </div>





                                    </div>





                                    <%--Bank Expenses--%>

                                    <h3>Bank Expenses</h3>
                                    <div>

                                        <div class="control-group">
                                            <label class="control-label">Bank Account No. :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="lblBank" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">L/C Margin :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtMargin" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">CFR USD :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtCfrUSD" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Bank Exch Rate :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtBExRate" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">CFR BDT :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtCfrBDT" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Total Expense :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="ttlBankExp" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Rate(%) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox49" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Value :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox50" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">LTR :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtLTR" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Interest  Rate (%) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox52" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Tenor :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox53" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Interest Amount :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox54" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <h3>Custom/Duty Expenses</h3>
                                    <div>

                                        <legend style="margin-bottom: 6px;">Duty Details</legend>

                                        <div class="control-group">
                                            <label class="control-label">Loading :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox9" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Loadfed Value :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtCardSl" runat="server" ToolTip="Pick or Type in MM/DD/YYYY"></asp:TextBox>

                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Landing 1%(1.01) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtName" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Cus Exchange Rate :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtCExRate" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Insurance(1%) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox26" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Other Duties /Penalty :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox27" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Assessable Vaue (Cal) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox28" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Assessable Vaue (Cus) AV :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox29" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Duty Head :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox30" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Duty Rate(%) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox31" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Duty ( Cal) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox32" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Duty ( Cus) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox33" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <%--
                                <legend style="margin-bottom: 6px;">List of Expenses</legend>
                                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False"
                                    DataSourceID="SqlDataSource13" Width="253px">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DepartmentName" HeaderText="Department Name"
                                            SortExpression="DepartmentName" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource13" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [DepartmentName] FROM [Departments] ORDER BY [DepartmentName]"></asp:SqlDataSource>
                                        --%>
                                    </div>
                                    <h3>Insurance Expenses</h3>
                                    <div>

                                        <div class="control-group">
                                            <label class="control-label">Insurance Name :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="ddInsurance" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">110% of CFR Value :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox38" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Exch Rate :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox39" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Commi Cal Base Value :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox40" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Commi Head :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox41" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Rate (%) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox42" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Total Ins. Commi :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox43" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Actual Insurance  :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox44" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <h3>CNF Expenses</h3>
                                    <div>

                                        <div class="control-group">
                                            <label class="control-label">CNF Name :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="txtCNF" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>

                                        <legend style="margin-bottom: 6px;">CNF  Commission Tax</legend>

                                        <div class="control-group">
                                            <label class="control-label">CNF Tax Basis :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox1" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Tax Base ( Cal) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox2" runat="server" ToolTip="Pick or Type in MM/DD/YYYY"></asp:TextBox>

                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Tax Base (Cus) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox3" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">CNF Comm Tax  Head :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">CNF  Comm Tax Rate(%) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">CNF Com  Tax (Cal) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Assessable Vaue (Cal) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">CNF Com Tax (Cus) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Total Cus Tax with CNF :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>

                                            </div>
                                        </div>

                                        <legend style="margin-bottom: 6px;">List of Expenses</legend>

                                        <div class="control-group">
                                            <label class="control-label">110% of CFR Value :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Exch Rate :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Commi Cal Base Value :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Commi Head :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox14" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Rate ( %) :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox15" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Total Ins. Commi :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox16" runat="server"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Actual Insurance  :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox17" runat="server"></asp:TextBox>

                                            </div>
                                        </div>

                                    </div>
                                    <h3>LC Costing</h3>
                                    <div>
                                        <div class="control-group">
                                            <label class="control-label">CFR BDT :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox18" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Cus+CNF  Duty :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox19" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Actual Insurance :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox20" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Total CNF Charge :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox21" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">L/C  Procng Cost/Charge  :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox22" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Port/ Shipping :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox23" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Transport :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox24" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Labor :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox25" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Bank Inteest :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox34" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Miscellaneous Cost :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox35" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">LC Import Cost :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox36" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Unit Cost :  </label>
                                            <div class="controls">
                                                <asp:TextBox ID="TextBox37" runat="server" Enabled="True"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>
                </div>

            </div>


        </ContentTemplate>
    </asp:UpdatePanel>


    
             <script>
                 $(function () {
                     $("#accordion").accordion({
                         heightStyle: "content"
                     });
                 });

                 //On UpdatePanel Refresh
                 var prm = Sys.WebForms.PageRequestManager.getInstance();
                 if (prm != null) {
                     prm.add_endRequest(function (sender, e) {
                         if (sender._postBackSettings.panelsToUpdate != null) {
                             $("#accordion").accordion({
                                 heightStyle: "content"
                             });
                             $("#tabs").tabs();
                         }
                     });
                 };
    </script>



</asp:Content>


