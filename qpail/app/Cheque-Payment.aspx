<%@ Page Title="Payment Cheque Processing" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Cheque-Payment.aspx.cs" Inherits="app_Cheque_Payment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .control-group label {
            width: 20% !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>


            <h3 class="page-title">Cheque Processing</h3>

            <div class="row">
                <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Edit Cheque Status
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

                                <div class="control-group hidden">
                                    <asp:Label ID="Label3" runat="server" Text="Current Status: "></asp:Label>
                                    <asp:DropDownList ID="ddType" runat="server" Width="174px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddType_SelectedIndexChanged">
                                        <asp:ListItem Value="In Hand">Cheque to Party</asp:ListItem>
                                        <asp:ListItem Value="In Bank">Cheque in Bank</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label1" runat="server" Text="Cheque No.: "></asp:Label>
                                    <asp:DropDownList ID="ddChqNo" runat="server" Width="174px"
                                        AutoPostBack="True"
                                        OnSelectedIndexChanged="ddChqNo_SelectedIndexChanged" DataSourceID="SqlDataSource1" DataTextField="ChequeNo" DataValueField="ChequeID">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [ChequeNo], [ChequeID] FROM [Cheque] WHERE ([ChqStatus] = @ChqStatus) AND TrType='Payment'  ORDER BY [ChequeID]">
                                        <SelectParameters>

                                            <asp:ControlParameter ControlID="ddType" Name="ChqStatus" PropertyName="SelectedValue" Type="String" />

                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label2" runat="server" Text="Collection ID : "></asp:Label>
                                    <asp:TextBox ID="txtID" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label5" runat="server" Text="Bank Account No. : "></asp:Label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddBank" runat="server" DataSourceID="SqlDataSource8" DataTextField="Bank" DataValueField="ACID">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT ACID, (Select [BankName] FROM [Banks] where [BankId]=a.BankID) +' - '+ ACNo +' - '+ACName AS Bank from BankAccounts a ORDER BY [ACName]"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label6" runat="server" Text="Bank Branch : "></asp:Label>
                                    <asp:TextBox ID="txtBranch" runat="server" ReadOnly="True"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label4" runat="server" Text="Edit Cheque No. : "></asp:Label>
                                    <asp:TextBox ID="txtChqBank" runat="server"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label7" runat="server" Text="Cheque Date : "></asp:Label>
                                    <asp:TextBox ID="txtChqDate" runat="server"></asp:TextBox>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="Label8" runat="server" Text="Cheque Amount : "></asp:Label>
                                    <asp:TextBox ID="txtChqAmount" runat="server"></asp:TextBox>
                                </div>
                                
                                <div class="control-group">
                                    <asp:Label ID="Label9" runat="server" Text="Cheque Receiver Name : "></asp:Label>
                                    <asp:TextBox ID="txtCustomer" runat="server"></asp:TextBox>
                                </div>
                                
                                <div class="control-group">
                                    <asp:Label ID="lblDateText" runat="server" Text="Cheque Pass Date:"></asp:Label>
                                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                        Enabled="True" TargetControlID="txtDate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="Label10" runat="server" Text="Remark : "></asp:Label>
                                    <asp:TextBox ID="txtRemark" runat="server"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Status :  </label>
                                    <asp:RadioButton ID="radApprove" Text="Approved" runat="server" GroupName="gr" AutoPostBack="True" OnCheckedChanged="radApprove_OnCheckedChanged" />
                                    <asp:RadioButton ID="rbUpdate" Text="Update" runat="server" GroupName="gr" Checked="true" />
                                    <asp:RadioButton ID="radCancel" Text="Cancelled" runat="server" GroupName="gr" AutoPostBack="True" OnCheckedChanged="radCancel_OnCheckedChanged" />
                                </div>

                                <div class="form-actions">
                                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save Cheque Info" />
                                    <asp:Label ID="lblMsg" runat="server"></asp:Label>

                                    <asp:Label ID="lblDate2" runat="server" Text="" Visible="false"></asp:Label>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>



                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="ltrRightTitle" runat="server"></asp:Literal>
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

                                <asp:GridView ID="GridView1" runat="server" CssClass="gridview" Width="100%" AutoGenerateColumns="false"
                                    DataKeyNames="ChequeNo" DataSourceID="SqlDataSource9">
                                    <Columns>
                                        <asp:BoundField DataField="ChequeName" HeaderText="Vendor" SortExpression="ChequeName" />
                                        <asp:BoundField DataField="ChequeNo" HeaderText="Cheque No." SortExpression="ChequeNo" ReadOnly="True" />
                                        <asp:BoundField DataField="ChqBank" HeaderText="Bank Name" SortExpression="Bank" />
                                        <asp:BoundField DataField="ChqDate" HeaderText="Chq. Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ChqDate" />
                                        <asp:BoundField DataField="ChqAmt" HeaderText="Amount" SortExpression="ChqAmt" />
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" />
                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle Font-Bold="True" ForeColor="#23A6F0" />
                                    <AlternatingRowStyle BackColor="#CCCCCC" />
                                    <EmptyDataTemplate>
                                        <p style="text-align: center;">
                                            <br />
                                            No Data Found for Pending Cheque</p>
                                    </EmptyDataTemplate>

                                </asp:GridView>

                                <asp:SqlDataSource ID="SqlDataSource9" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [ChequeNo], 
                                    (SELECT BankName FROM Banks where BankId=Cheque.ChqBank) As ChqBank, [ChqDate], [ChqAmt], [ChequeName] FROM [Cheque] 
                                    WHERE (([TrType] = @TrType) AND ([ChqStatus] = @ChqStatus)) ORDER BY [ChqDate]">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="Payment" Name="TrType" Type="String" />
                                        <asp:ControlParameter ControlID="ddType" Name="ChqStatus" PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>


                                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        <div id="blur">&nbsp;</div>
                                        <div id="progress">
                                            Update in progress. Please wait ...

                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>

