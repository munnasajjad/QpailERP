<%@ Page Title="Assign Fees" Language="C#" MasterPageFile="~/app/Layout.Master" AutoEventWireup="true" CodeBehind="AssaignClassExpenses.aspx.cs" Inherits="Oxford.app.AssaignClassExpenses" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    <asp:UpdatePanel ID="uPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <script type="text/javascript" language="javascript">
                 Sys.Application.add_load(callJquery);
            </script>


    <div class="col-lg-6">
        <section class="panel">
            <%--Body Contants--%>
            <div id="Div2">
                <div>

                    <fieldset>
                        <legend>Assign Fees to Classes</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>

                            <tr id="EditField" runat="server" class="hidden">
                                <td>Edit Class Expense</td>
                                <td>
                                    <asp:DropDownList name="" ID="DropDownList1" runat="server" AutoPostBack="true"
                                        DataSourceID="SqlDataSource2" DataTextField="text1" DataValueField="eid"
                                        OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [eid], 
                                        ((select name from Class where sl=AssignExpense.Class)+'-'+(select name from CollectionHeads where sl=AssignExpense.ExpHead)) AS text1
                                         FROM [AssignExpense] order by eid"></asp:SqlDataSource>
                                </td>
                            </tr>
                            
                            
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Assign Date  : "></asp:Label>
                                </td>
                                <td>
                                     <asp:TextBox ID="txtDate" runat="server"  CssClass="form-control"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                        Enabled="True" TargetControlID="txtDate">
                                                    </asp:CalendarExtender>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>Class Name</td>
                                <td>
                                    <asp:DropDownList ID="ddClass" runat="server" CssClass="form-control"
                                        DataSourceID="SqlDataSource3" DataTextField="name" DataValueField="sl" AutoPostBack="True" OnSelectedIndexChanged="ddClass_OnSelectedIndexChanged"  AppendDataBoundItems="True" >
                                        <asp:ListItem Value="">--- all ---</asp:ListItem></asp:DropDownList>
                                   
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [Name] FROM [Class] order by sl"></asp:SqlDataSource>
                                </td>
                                <td>
                                    <%--<asp:CheckBox ID="CheckBox1" runat="server"></asp:CheckBox>
                                    All Classes--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Expense Group : "></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddGroup" runat="server" AutoPostBack="true" CssClass="form-control"
                                        DataSourceID="SqlDataSource7" DataTextField="name" DataValueField="sl"
                                        OnSelectedIndexChanged="ddGroup_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [Name] FROM [CollectionTypes] order by sl"></asp:SqlDataSource>

                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Expense Head : "></asp:Label>

                                </td>
                                <td>
                                    <asp:DropDownList ID="ddHead" runat="server" CssClass="form-control" AutoPostBack="true"
                                        DataSourceID="SqlDataSource4" DataTextField="name" DataValueField="sl" OnSelectedIndexChanged="ddHead_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [Name] FROM [CollectionHeads] WHERE ([GroupID] = @GroupID) ORDER BY [Name]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddGroup" Name="GroupID" PropertyName="SelectedValue" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:Label ID="lblSl" runat="server" Text="" Visible="false"></asp:Label>
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
                                    <asp:TextBox ID="txtAmount" runat="server" Width="100px" CssClass="form-control"></asp:TextBox>
                                    <span style="color: red">
                                        <asp:Literal ID="ltrNotice" runat="server"></asp:Literal></span>
                                    <%--<img src="../Libs/Calculator/calculator.png" onclick="calculator()" />--%>
                                    <asp:FilteredTextBoxExtender ID="txtOpBalance_FilteredTextBoxExtender"
                                        runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="-0123456789." TargetControlID="txtAmount">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td></td>
                            </tr>


                            <tr style="background: none">
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnClear" type="reset" CssClass="btn btn-s-md btn-primary" runat="server" Text="Clear" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>


                </div>
            </div>
            <%--End Body Contants--%>
        </section>
    </div>




    <div class="col-lg-6">
        <section class="panel">

            <div id="Div1">
                <div>

                    <fieldset>
                        <legend>Class History</legend>
                        
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table-responsive" Width="100%"
                            AllowSorting="True"
                            DataSourceID="SqlDataSource1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="eid">
                            
                            <Columns>

                                <asp:TemplateField HeaderText="sl" SortExpression="eid" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("eid") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:BoundField DataField="ExpDate" HeaderText="Date" DataFormatString="{0:d}" SortExpression="ExpDate" />
                                <asp:BoundField DataField="Class1" HeaderText="Class" SortExpression="Class" ReadOnly="True" />
                                <asp:BoundField DataField="ExpHead" HeaderText="Exp. Head" SortExpression="ExpHead" />
                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                                
                                <%--<asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" />
                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" />

                                        <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                        </asp:ConfirmButtonExtender>
                                        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                            PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                        <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                            <b style="color: red">This entry will be deleted permanently!</b><br />
                                            Are you sure you want to delete this ?
                                                            <br />
                                            <br />
                                            <div style="text-align: right;">
                                                <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                            </div>
                                        </asp:Panel>

                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand="SELECT eid, Class,
                            (select name from Class where sl=AssignExpense.Class) as Class1,
                            ExpDate,(select name from CollectionHeads where sl=AssignExpense.ExpHead) as ExpHead, 
                            Description, Amount 
                             FROM [AssignExpense] where Class=@Class ORDER BY [eid] DESC"
                            DeleteCommand="DELETE FROM Class WHERE (eid = @eid)">
                            <DeleteParameters>
                                <asp:Parameter Name="eid" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddClass" Name="Class" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        
                    </fieldset>
                    
                </div>
            </div>
            <%--End Body Contants--%>
        </section>
    </div>

            
            </ContentTemplate>
        </asp:UpdatePanel>

</asp:Content>
