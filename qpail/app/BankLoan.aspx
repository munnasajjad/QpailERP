<%@ Page Title="Bank Loans" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="BankLoan.aspx.cs" Inherits="app_BankLoan" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
       
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                 Sys.Application.add_load(callJquery);
            </script>            
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">
                        <asp:Literal ID="ltrFrmName" runat="server" Text="Bank Loan" />
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="ltrSubFrmName" runat="server" Text="Bank Loan Setup" />
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                <asp:HiddenField runat="server" ID="LoanIdHField"/>
                                <%-- <div class="control-group">
                                    <label class="control-label">Financial Year</label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddGroup" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Literal ID="lblGID" runat="server"></asp:Literal>
                                    </div>
                                </div>--%>
                                <div class="control-group">
                                <label class="control-label" Visible="False" >Loan Type:<br /><asp:LinkButton ID="lbLLoanType" runat="server" OnClick="lbLLoanType_OnClick" Visible="False">New</asp:LinkButton>
                                        </label>
                                    <%--<asp:ListItem Value="020101">CC</asp:ListItem>
                                        <asp:ListItem Value="020201">UPAS</asp:ListItem>
                                        <asp:ListItem Value="020203">HPSM</asp:ListItem>
                                        <asp:ListItem Value="020202">Other Loan</asp:ListItem>
                                        <asp:ListItem Value="020204">LATR</asp:ListItem>--%>
                                    <asp:DropDownList ID="ddLoanType" runat="server" DataSourceID="SqlDataSource1" DataValueField="Id" DataTextField="LoanTypes" AutoPostBack="True"> </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" 
                                        SelectCommand="SELECT Id, LoanTypes FROM BankLoanTypes ORDER BY LoanTypes">
                                    </asp:SqlDataSource>
                                    <asp:TextBox ID="txtLoanType" runat="server" Visible="False" />
                                    </div>

                                <div class="form-group">
                                    <label class="control-label">A/C Head Name:</Label>
                                    
                                    <asp:DropDownList ID="ddAcheadId" runat="server" DataSourceID="SqlDataSource3" 
                                        DataTextField="AccountsHeadName" DataValueField="AccountsHeadID" CssClass="form-control select2me" AutoPostBack="True" OnSelectedIndexChanged="ddAcheadId_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" 
                                        SelectCommand="SELECT LoanTypes.LoanTypeId, LoanTypes.LoanType, LoanTypes.AccountsHeadID, HeadSetup.AccountsHeadName
                                        FROM LoanTypes INNER JOIN HeadSetup ON LoanTypes.AccountsHeadID = HeadSetup.AccountsHeadID WHERE LoanTypes.LoanType=@LoanType ORDER BY HeadSetup.AccountsHeadName">
                                         <SelectParameters>
                                            <asp:ControlParameter ControlID="ddLoanType" Name="LoanType" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    
                                </div>
                                <div class="form-group">
                                    <label class="control-label">Bank Account :</label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddBancAcc" runat="server" DataSourceID="SqlDataSource17" DataTextField="Bank" DataValueField="ACID" CssClass="form-control select2me">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource17" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT ACID, (Select [BankName] FROM [Banks] where [BankId]=a.BankID)  +' ('+Address +') '+ +' - '+ACNo +' - '+ ACName AS Bank from BankAccounts a ORDER BY [ACName]"></asp:SqlDataSource>
                                    </div>
                                </div>
                                <%--<div class="form-group">
                                    <label class="control-label">Accounts Head Name:</Label>
                                    
                                    <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource3" 
                                        DataTextField="AccountsHeadName" DataValueField="AccountsHeadID" CssClass="form-control select2me" AutoPostBack="True" OnSelectedIndexChanged="ddAcheadId_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" 
                                        SelectCommand="SELECT AccountsHeadID, [AccountsHeadName] FROM [HeadSetup] WHERE (ControlAccountsID='020101' OR ControlAccountsID='020201')">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddLoanType" Name="ControlAccountsID" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    
                                </div>--%>
                                <%--<div class="form-group">
                                    <label class="control-label">Accounts Head Name:</Label>
                                    
                                    <asp:DropDownList ID="ddAcheadId" runat="server" DataSourceID="SqlDataSource3" 
                                        DataTextField="AccountsHeadName" DataValueField="AccountsHeadID" CssClass="form-control select2me" AutoPostBack="True" OnSelectedIndexChanged="ddAcheadId_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" 
                                        SelectCommand="SELECT AccountsHeadID, [AccountsHeadName] FROM [HeadSetup] WHERE ControlAccountsID=@ControlAccountsID ">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddLoanType" Name="ControlAccountsID" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    
                                </div>--%>
                                <div class="control-group">
                                    <label class="control-label">Code :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtCode" runat="server" title="Write your desire Code and press Enter" CssClass="span6 m-wrap"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <asp:Label  runat="server" Text="Received Date :" Font-Bold="True"></asp:Label>
                                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalenderExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDate"></asp:CalendarExtender></div>
                                <div class="control-group">
                                    <label class="control-label">Interest Rate :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtInterest" runat="server" title="Write Interest Rate and press Enter" CssClass="span6 m-wrap" Width="35%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtInterest" ErrorMessage="Interest Rate is required." Font-Size="15px" ForeColor="Red" ToolTip="Interest Rate is required."  >Interest Rate is required.</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Duration :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtDuration" runat="server" title="Write Duration and press Enter" CssClass="span6 m-wrap" Width="35%" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDuration" ErrorMessage="Mobile is required." Font-Size="15px" ForeColor="Red" ToolTip="Duration is required."  >Duration is required.</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Principal Amount in taka :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtRevAmount" runat="server" title="Write Duration and press Enter" CssClass="span6 m-wrap" Width="35%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Static" runat="server" ControlToValidate="txtRevAmount" ErrorMessage="Mobile is required." Font-Size="15px" ForeColor="Red" ToolTip="Principal Amount is required."  >Principal Amount is required.</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Loan Balance :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtBalance" runat="server" ReadOnly="true" title="Write your desire Code and press Enter" CssClass="span6 m-wrap"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Note :</label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtNote" runat="server" title="Write Duration and press Enter" CssClass="span6 m-wrap"></asp:TextBox>
                                    </div>
                                </div>
                                
                                <div class="control-group">
                                    <label class="control-label">Status : </label>
                                    <div class="controls">
                                        <asp:CheckBox ID="cbStatus" runat="server" Checked="False" Text="Is Paid" />

                                        <%--<asp:CheckBox ID="chkDisable" CssClass="radiobtn" runat="server" Text="Disable" Checked="true"  />--%>

                                    </div>
                                </div>
                                <div class="control-group" Visible="False">
                                    <%--<label class="control-label" Visible="False">Status :</label>--%>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddStatus" runat="server" Visible="False">
                                            <asp:ListItem Value="0">Paid</asp:ListItem>
                                        <asp:ListItem Value="0">Pending</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-actions">
                                    <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" />
                                </div>

                            </div>

                        </div>

                    </div>

                </div>


                <div class="col-md-7">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="Literal1" runat="server" Text="Bank Loans Record" />
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="table-responsive">

                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"
                                    OnRowDeleting="GridView1_RowDeleting" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataSourceID="SqlDataSource2" Width="100%" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="id" InsertVisible="False" SortExpression="id" Visible="False">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="AccountsHeadName" HeaderText="AC Head Name" SortExpression="ACHeadId"  />

                                        <asp:BoundField DataField="code" HeaderText="code" SortExpression="code" ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ReceivedDate" HeaderText="Received Date" SortExpression="ReceivedDate" DataFormatString={0:d} />
                                        <asp:BoundField DataField="InterestRate" HeaderText="Interest Rate" SortExpression="InterestRate"  />
                                        <asp:BoundField DataField="Duration" HeaderText="Duration" SortExpression="Duration" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>


                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" 
                                    SelectCommand="SELECT        BankLoan.Id, BankLoan.LoanType, BankLoan.ACHeadId, BankLoan.Code, BankLoan.ReceivedDate, BankLoan.InterestRate, BankLoan.Duration, BankLoan.Rcvamount, BankLoan.Note, BankLoan.Status, 
                         HeadSetup.AccountsHeadName
FROM            BankLoan INNER JOIN
                         HeadSetup ON BankLoan.ACHeadId = HeadSetup.AccountsHeadID
  WHERE BankLoan.LoanType=@LoanTypes ORDER BY BankLoan.Id DESC">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddLoanType" Name="LoanTypes" PropertyName="SelectedValue" />
                                        </SelectParameters>

                                </asp:SqlDataSource>
                                                                
                            </div>
                        </div>
                    </div>

                 </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>