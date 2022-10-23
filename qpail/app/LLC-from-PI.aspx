<%@ Page Title="Create LLC from PI" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="LLC-from-PI.aspx.cs" Inherits="app_LLC_from_PI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .col-md-4 .control-group input, .col-md-4 .control-group select {
            width: 100%;
        }

        .col-md-4 .control-group label {
            padding-bottom: 4px;
        }

        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td {
            border: 1px solid #ddd;
            color: GrayText;
        }

        input#ctl00_BodyContent_chkMerge, height_fix {
            height: 17px !important;
            top: -3px !important;
            margin-top: -5px !important;
        }

        bottom_fix {
            margin-bottom: -15px !important;
        }

        #ctl00_BodyContent_txtAmount, #ctl00_BodyContent_txtRate, #ctl00_BodyContent_txtQty, #ctl00_BodyContent_txtStock {
            text-align: center;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">        </asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
        <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
            <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
        </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
             <script type="text/javascript" language="javascript">
                 Sys.Application.add_load(callJquery);
            </script>
	
			


            <div class="row">
                <div class="col-md-4 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Create LLC from PI
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                            <div class="form-horizontal" role="form">

                                <div class="form-body">

                                    

                                    <div class="control-group">
                                        <label class="control-label full-wdth">Customer Name :  </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddCustomer" runat="server" DataSourceID="SqlDataSource1" DataTextField="Company" DataValueField="PartyID"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = @Type) ORDER BY [Company]">
                                                <SelectParameters>
                                                    <asp:Parameter DefaultValue="customer" Name="Type" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                    </div>


                                    <div class="control-group">
                                        <label class="control-label full-wdth">PI No. : </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddOrders" runat="server" DataSourceID="SqlDataSource6" DataTextField="OrderID" DataValueField="OrderID"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddOrders_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [OrderID] FROM [Orders] WHERE CustomerName=@CustomerName AND ([DeliveryStatus] = @DeliveryStatus) AND OrderType='PI'">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddCustomer" Name="CustomerName" PropertyName="SelectedValue" />
                                                    <asp:Parameter DefaultValue="P" Name="DeliveryStatus" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                    </div>
                                    
                                     <%--<div class="control-group">
                                        <label class="control-label full-wdth">Edit PI No. : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtEditPI" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>--%>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">PI Date : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>
                                    
                                    <div class="control-group">
                                        <label class="control-label full-wdth">LLC No. : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtLCNo" runat="server" CssClass="form-control" ></asp:TextBox>                                            
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label full-wdth">LLC Date : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtLCDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtLCDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label full-wdth">LLC Expiration Date : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtLcExpDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtLcExpDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>
                                    

                                   <div class="control-group">
                                    <label class="control-label full-wdth">Issuing Bank :  </label>
                                        <asp:DropDownList ID="ddIssueBank" runat="server" DataSourceID="SqlDataSource7" DataTextField="BankName" DataValueField="BankId">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="Select BankId, [BankName] FROM [Banks] where [Type]= 'bank'"></asp:SqlDataSource>
                                    <%--<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT ACID, (Select [BankName] FROM [Banks] where [BankId]=a.BankID) +' - '+ACNo +' - '+ACName AS Bank from BankAccounts a ORDER BY [ACName]"></asp:SqlDataSource>--%>
                                </div>
                                    <div class="control-group">
                                        <label class="control-label full-wdth">Issuer Bank Branch : </label>
                                        <asp:TextBox ID="txtBranch" runat="server"></asp:TextBox>
                                    </div>
                                                                
                                <div class="control-group">
                                    <label class="control-label full-wdth">Advising Bank Account :  </label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddBank" runat="server" DataSourceID="SqlDataSource8" 
                                            DataTextField="Bank" DataValueField="ACID" AppendDataBoundItems="true">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT ACID, (Select [BankName] FROM [Banks] where [BankId]=a.BankID) +' - '+ACNo +' - '+ACName AS Bank from BankAccounts a ORDER BY [ACName]"></asp:SqlDataSource>
                                    </div>
                                </div>

                                    <%--<div class="control-group">
                                        <label class="control-label full-wdth">LC No. : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" ></asp:TextBox>                                            
                                        </div>
                                    </div>--%>
                                    
                                    <div class="control-group">
                                        <label class="control-label full-wdth">Delivery Date (Asked) : </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtDeliveryDate">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>
                                    
                                    <div class="control-group">
                                        <label class="control-label full-wdth">Delivery Location:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="" />
                                        </div>
                                    </div>
                                    
                                    <div class="control-group">
                                        <label class="control-label full-wdth">LLC Terms & Conditions:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtTerms" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" Width="310px" placeholder="" />
                                        </div>
                                    </div>
                                    
                                    <div class="control-group">
                                        <label class="control-label full-wdth">Upload LLC Documents: </label>
                                        <div class="controls">
                                    <iframe src="Docs/PO-Upload.aspx" width="320" height="20" scrolling="no" seamless="seamless"></iframe>
                                        </div>
                                        </div>


                                    <hr />

                                    <div class="form-group">
                                        <label class="col-sm-6 control-label">Total Amount: </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtTotal" runat="server" CssClass="form-control" placeholder="Total Amount" Text="0" />
                                        </div>
                                    </div>

                                    <div class="form-group hidden">
                                        <label class="col-sm-6 control-label">Discount : </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control" placeholder="(-)" onkeyup="calInvTotal()" Text="0" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-6 control-label">VAT (%) : </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtVat" runat="server" CssClass="form-control" placeholder="(%)" onkeyup="calInvTotal()" Text="0" />
                                            
                                        </div>
                                    </div>
                                    <span class="vatamt" style="text-align:center; width: 100%; padding: 6px 0; margin-left:110px;"></span>

                                    <div class="form-group">
                                        <label class="col-sm-6 control-label">Payable : </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtPay" runat="server" CssClass="form-control" placeholder="Payable"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--<div style="height:120px;">&nbsp;</div>--%>
                                </div>
                            </div>

                            <div class="form-actions">
                                <%--<asp:CheckBox ID="chkPrint" runat="server" Text="Print" />--%>
                                <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Create LLC" OnClick="btnSave_Click" />
                                <%--<asp:Button ID="btnEdit" CssClass="btn red" runat="server" Text="Edit" />
                        <asp:Button ID="btnPrint" CssClass="btn purple" runat="server" Text="Print" />
                                <asp:Button ID="btnCancel" CssClass="btn default" runat="server" Text="Cancel Order" OnClick="btnCancel_Click" />--%>
                            </div>

                        </div>
                    </div>
                </div>


                <div class="col-md-8 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box green ">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Item Order Details
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal" role="form">
                                <div class="form-body">
                                    
                                    <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:LoginName ID="LoginName1" runat="server" Visible="false" />
                                    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>

                                    <div class="col-md-12">
                                        <div class="row">

                                            <asp:Panel ID="pnlAdd" runat="server" DefaultButton="btnAdd" Visible="True">

                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label full-wdth">Pack Size </label>

                                                        <asp:DropDownList ID="ddSize" runat="server" DataSourceID="SqlDataSource2" DataTextField="BrandName" DataValueField="BrandID">
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] WHERE ([ProjectID] = @ProjectID) order by DisplaySl">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>

                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label">Brand </label>

                                                        <asp:DropDownList ID="ddBrand" runat="server" DataSourceID="SqlDataSource3" DataTextField="BrandName" DataValueField="BrandID">
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT [BrandID], [BrandName] FROM [CustomerBrands] WHERE (([CustomerID] = @CustomerID) AND ([ProjectID] = @ProjectID))">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="ddCustomer" Name="CustomerID" PropertyName="SelectedValue" Type="String" />
                                                                <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                </div>


                                                <div class="col-md-4">
                                                    <div class="control-group bottom_fix" style="margin-bottom: -15px !important;">
                                                        <label class="col-sm-12 control-label">Grade </label>

                                                        <asp:DropDownList ID="ddGrade" runat="server" DataSourceID="SqlDataSource5"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddGrade_SelectedIndexChanged"
                                                            DataTextField="GradeName" DataValueField="GradeID">
                                                        </asp:DropDownList>

                                                        <label class="col-sm-12 control-label" for="ctl00_BodyContent_chkMerge" style="top: -3px!important">Merge</label>

                                                        <asp:CheckBox ID="chkMerge" runat="server" CssClass="height_fix" />

                                                        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT GradeID,GradeName from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=2 AND ProjectID=1) ORDER BY [GradeName]"></asp:SqlDataSource>
                                                    </div>
                                                </div>

                                                
                                                <div class="col-md-4">
                                                    <div class="control-group bottom_fix" style="margin-bottom: -15px !important;">
                                                        <label class="col-sm-12 control-label">Category </label>

                                                        <asp:DropDownList ID="ddCategory" runat="server" DataSourceID="SqlDataSource10"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddCategory_OnSelectedIndexChanged"
                                                            DataTextField="CategoryName" DataValueField="CategoryID">
                                                        </asp:DropDownList>

                                                        <asp:SqlDataSource ID="SqlDataSource10" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT CategoryID, CategoryName FROM [Categories] where GradeID = @GradeID ORDER BY [CategoryName]">
                                                        <SelectParameters>
                                                                <asp:ControlParameter ControlID="ddGrade" Name="GradeID" PropertyName="SelectedValue" />
                                                            </SelectParameters>

                                                        </asp:SqlDataSource>
                                                    </div>
                                                </div>


                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label">Product </label>

                                                        <asp:DropDownList ID="ddProduct" runat="server" DataSourceID="SqlDataSource4"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddProduct_SelectedIndexChanged"
                                                            DataTextField="ItemName" DataValueField="ProductID">
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                            SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE [CategoryID]=@CategoryID ORDER BY [ItemName]">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="ddCategory" Name="CategoryID" PropertyName="SelectedValue" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                        <%--SelectCommand="SELECT ProductID, [ItemName] FROM [Products] WHERE ([CategoryID] IN (Select CategoryID from Categories where GradeID IN (Select GradeID from ItemGrade where CategoryID in (Select CategoryID from ItemSubGroup where GroupID=2 AND ProjectID=1)))) ORDER BY [ItemName]"></asp:SqlDataSource>--%>
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label full-wdth">Available in Stock</label>
                                                        <asp:TextBox ID="txtStock" runat="server" ReadOnly="true" />
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label full-wdth">
                                                            Order Qty.(<asp:Literal ID="ltrUnit" runat="server" />)</label>
                                                        <asp:TextBox ID="txtQty" runat="server" onkeyup="calTotal()" />
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtQty">
                                                        </asp:FilteredTextBoxExtender>
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label">Rate </label>
                                                        <asp:TextBox ID="txtRate" runat="server" onkeyup="calTotal()" />
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtRate">
                                                        </asp:FilteredTextBoxExtender>
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label">Amount : </label>
                                                        <asp:TextBox ID="txtAmount" runat="server" ReadOnly="true" />
                                                    </div>
                                                </div>
                                                                                                
                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <label class="col-sm-12 control-label full-wdth">Weight/unit (gm) </label>
                                                        <asp:TextBox ID="txtWeight" runat="server"  />
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtWeight">
                                                        </asp:FilteredTextBoxExtender>
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="control-group">
                                                        <div style="margin-top: 29px">
                                                            <asp:Button ID="btnAdd" runat="server" Text="Add" Width="100px" OnClick="btnAdd_Click" />

                                                        </div>
                                                    </div>
                                                </div>


                                            </asp:Panel>


                                        </div>
                                    </div>



                                   <div style="text-align:right;">
                                    Total Quantity: <asp:Literal ID="ltrQty" runat="server"></asp:Literal> &nbsp; &nbsp; 
                                    Total Weight: <asp:Literal ID="ltrWeight" runat="server"></asp:Literal> kg.
                                
                                    </div>

                                </div>
                                
                                        <asp:GridView ID="ItemGrid" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered"
                                             BackColor="Red" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="1" ForeColor="Black" RowStyle-BackColor="#A1DCF2" 
                                            HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" DataKeyNames="Id" GridLines="Vertical" SelectedRowStyle-BackColor="LightBlue"
                                            OnRowDataBound="ItemGrid_RowDataBound" OnRowDeleting="ItemGrid_RowDeleting" OnSelectedIndexChanged="ItemGrid_SelectedIndexChanged" >

                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                        <asp:Label ID="lblEntryId" runat="server" CssClass="hidden" Text='<%# Bind("Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px"></ItemStyle>
                                                </asp:TemplateField>

                                                <asp:BoundField ItemStyle-Width="150px" DataField="ProductName" HeaderText="Product Name" ReadOnly="true">
                                                    <ItemStyle Width="35%"></ItemStyle>
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="Quantity">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbliQty" runat="server" Text='<%# Bind("Quantity") %>' ></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtQty" runat="server" Text='<%# Bind("Quantity") %>' CssClass="qtySpin" Width="50px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>

                                                <asp:BoundField ItemStyle-Width="150px" DataField="UnitType" HeaderText="Unit" ReadOnly="true">
                                                    <ItemStyle Width="5%"></ItemStyle>
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbliPrc" runat="server" Text='<%# Bind("UnitCost") %>' ></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtPrice" runat="server" Text='<%# Bind("UnitCost") %>' CssClass="amtSpin" Width="60px"></asp:TextBox>                                                        
                                                    </EditItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubTotal" runat="server" Text='<%# Bind("ItemTotal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="Weight/Unit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUWeight" runat="server" Text='<%# Bind("UnitWeight") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtUWeight" runat="server" Text='<%# Bind("UnitWeight") %>' CssClass="amtSpin" Width="60px"></asp:TextBox>                                                        
                                                    </EditItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>

                                                
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Select" />
                                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />
                                                        
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

                                            <RowStyle BackColor="#F7F7DE" />
                                            <FooterStyle BackColor="#CCCC99" />
                                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                            <SelectedRowStyle BackColor="SkyBlue" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%--<script src="../js/jquery-latest.min.js"></script>--%>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#<%=txtAmount.ClientID%>').attr('readonly', true);
            $('#<%=txtTotal.ClientID%>').attr('readonly', true);
            $('#<%=txtPay.ClientID%>').attr('readonly', true);

            $("[id*=ItemGrid]input[type=text][id*=txtQty]").change(function (e) {
                updateGrid(e);
            })
            $("[id*=ItemGrid]input[type=text][id*=txtPrice]").change(function (e) {
                updateGrid(e);
            })

            $("[id*=ItemGrid]input[type=text][id*=txtQty]").keyup(function (e) {
                updateGrid(e);
            })

            $("[id*=ItemGrid]input[type=text][id*=txtPrice]").keyup(function (e) {
                updateGrid(e);
            })
        })

        function updateGrid(e) {
            var price = $(e.target).closest('tr').find("input[type=text][id*=txtPrice]").val();
            var quantity = $(e.target).closest('tr').find("input[type=text][id*=txtQty]").val();
            var total = parseInt(price) * parseInt(quantity);
            $(e.target).closest('tr').find("[id*=lblSubTotal]").text(total);
            $(e.target).closest('tr').find("[id*=hidQty]").text(quantity);

            GrossTotal();
            var iCode = $(e.target).closest('tr').find("[id*=lblEntryId]").text();
            $.ajax({
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                url: 'WebMethods.aspx/UpdateOrderGrid',
                data: "{'iCode':'" + iCode + "','iQty':'" + quantity + "','iPrice':'" + price + "'}",
                async: false,

                success: function (response) {

                },
                failure: function (response) {
                    alert(response.e);
                },
                error: function (response) {
                    alert(response.e);
                }
            });
        }
        function calTotal() {
            var rate = $('#<%=txtRate.ClientID%>').val();
            var qty = $('#<%=txtQty.ClientID%>').val();

            var amount = parseFloat(rate) * parseFloat(qty);
            $('#<%=txtAmount.ClientID%>').val(amount.toString());
        }

        function calInvTotal() {
            var ttl = $('#<%=txtTotal.ClientID%>').val();
            var disc = $('#<%=txtDiscount.ClientID%>').val();
            var vat = $('#<%=txtVat.ClientID%>').val();

            var pay = parseFloat(ttl) - parseFloat(disc) + (parseFloat(ttl) * parseFloat(vat) / 100);
            $('#<%=txtPay.ClientID%>').val(pay.toString());
            $('.vatamt').html("VAT Amount: " + parseFloat(ttl) * parseFloat(vat) / 100);
        }

        var gross;
        function GrossTotal() {
            gross = 0;
            $("[id*=ItemGrid][id*=lblSubTotal]").each(function (index, item) {
                gross = gross + parseInt($(item).text());
            });
            document.getElementById("<%= txtTotal.ClientID %>").value = gross;
            Grandtotal();
        }

        function Grandtotal() {
            var txtTotal = document.getElementById("<%= txtTotal.ClientID %>").value;
            //document.getElementById("<%= txtPay.ClientID %>").value = parseInt(txtTotal) - parseInt(txtDiscount) - parseInt(txtService);

        }

    </script>



        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>

