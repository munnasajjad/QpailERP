<%@ Page Title="Statement of Comprehensive Income" Language="C#" AutoEventWireup="true" MasterPageFile="~/app/MasterPage.master"  CodeFile="IncomeStatement.aspx.cs" Inherits="Application_Reports_IncomeStatement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

   <style type="text/css">
        label {
            padding-top: 6px;
            text-align: right;
        }

        .table1 {
            width: 100%;
        }

            .table1 th {
                vertical-align: middle;
                font-weight: 700;
                    text-align: center;
            }

            .table1 .form-control, .table1 select {
                width: 100%;
            }
            
            .table {
                width: 50% !important;
            }

        table#ctl00_BodyContent_GridView1 {
            /*min-width: 1200px;*/
        }

            table#ctl00_BodyContent_GridView1 tr {
                height: 20px;
            }

            tr td:last-child {
                text-align: right;
}
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">
    
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


            <h3 class="page-title">Statement of Comprehensive Income</h3>

            <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


            <div class="row">

                <div class="col-lg-12">
                    <section class="panel">
                        <%--Body Contants--%>
                        <div id="Div2">
                            <div>

                                <fieldset>
                                    <%--<legend>Search Terms</legend>--%>
                                    <table border="0" width="50%" style="width: 50%" class="table1">
                                        <tr>
                                           
                                            <th>Date From</th>
                                            <th></th>
                                            <th>Date To</th>
                                            <%--<th></th>
                                             <th>View By</th>
                                            <th></th>--%>
                                            <th></th>
                                        </tr>
                                        <tr>
                                           <td>
                                                <asp:TextBox ID="txtdateFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtdateFrom" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>&nbsp; </td>
                                            <td>
                                                <asp:TextBox ID="txtdateTo" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="txtdateTo" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <%--<td></td>
                                             <td>
                                                <asp:DropDownList ID="ddType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                    <asp:ListItem>Control Accounts</asp:ListItem>
                                                    <asp:ListItem>Subsidiary Accounts</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'customer') ORDER BY [Company]"></asp:SqlDataSource>

                                            </td>

                                            <td>&nbsp; </td>--%>
                                            <td style="text-align: center; vertical-align: middle;">
                                                <asp:Button ID="btnSearch" CssClass="btn btn-s-md btn-primary" runat="server" Text="SHOW" OnClick="btnSearch_OnClick" />
                                                <%--<input id ="printbtn" type="button" class="btn btn-s-md btn-primary" value="PRINT" onclick="window.print();" >--%>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <br/>
                                <fieldset>
                                    <%--<legend>Search Terms</legend>--%>
                                    <table border="0" width="50%" style="width: 50%" class="table1">
                                        <%--<tr>
                                           
                                            <th>Date From</th>
                                            <th></th>
                                            <th>Date To</th>
                                            <th></th>
                                        </tr>--%>
                                        <tr>
                                            <td style="vertical-align: middle; text-align: right;"><b>Financial Year 1 : </b></td>
                                         <td>
                                             <asp:DropDownList ID="ddYear1" runat="server" DataSourceID="SqlDataSource3" DataTextField="Financial_Year" DataValueField="Financial_Year" CssClass="form-control"></asp:DropDownList>
                                             <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" SelectCommand="SELECT [Financial_Year] FROM [tblFinancial_Year]"></asp:SqlDataSource>
                                         </td>
                                            <td style="vertical-align: middle; text-align: right;"><b>Financial Year 2 : </b></td>
                                         <td>
                                             <asp:DropDownList ID="ddYear2" runat="server" DataSourceID="SqlDataSource2" DataTextField="Financial_Year" DataValueField="Financial_Year" CssClass="form-control"></asp:DropDownList>
                                             <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:qpailerpConnectionString %>" SelectCommand="SELECT [Financial_Year] FROM [tblFinancial_Year]"></asp:SqlDataSource>
                                         </td>
                                            <td style="text-align: center; vertical-align: middle;">
                                                <asp:Button ID="btnShow2" CssClass="btn btn-s-md btn-primary" runat="server" Text="SHOW" OnClick="btnShow2_OnClick" />
                                                <%--<input id ="printbtn" type="button" class="btn btn-s-md btn-primary" value="PRINT" onclick="window.print();" >--%>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>

                            </div>
                        </div>

                    </section>
                </div>

                <iframe id="if1" runat="server" height="800px" width="100%" ></iframe>


                <div class="col-lg-12">
                    <section class="panel">

                        <div id="Div1">
                            <div>

                                <fieldset>
                                    <legend class="hidden"><asp:Literal ID="ltrQR" runat="server"></asp:Literal> </legend>

                                    <div class="table-responsive">
                                        
                                        
                                        
                                    <asp:Literal ID="ltrResult" runat="server"></asp:Literal>
                                        

                                    </div>
                                </fieldset>
                                
                                <%--<div style="margin-top: 100px">&nbsp;
                                    <br/><br/><br/><br/><br/>
                                    <hr />
                                    <br/><br/><br/><br/><br/>
                                </div>--%>
                                

                                <%--<table width="70%">
                                     <tr>
                                        <td><br/><br/><br/><br/><br/><hr /></td>
                                        <td><br/><br/><br/><br/><br/><hr /></td>
                                        <td><br/><br/><br/><br/><br/><hr /></td>
                                    </tr>
                                    <tr>
                                        <td>Account Manager</td>
                                        <td>Managing Director</td>
                                        <td>Chairman</td>
                                    </tr>
                                </table>--%>

                            </div>
                        </div>
                        <%--End Body Contants--%>
                    </section>
                </div>
                
            </div>



	<%--		
<div id="content">
        <div id="ComInfo">
            <asp:Image ID="Image1" runat="server" />
            <p>
                <b>
                    <asp:Label ID="lblName" runat="server" ></asp:Label></b><br />
                <asp:Label ID="lblArress" runat="server" ></asp:Label>
            </p>
            <h3>Revenue Statement</h3>
        </div>
    
    <div id="qPanel">
        <div id="datePanel">
    <asp:Label ID="lblDate" runat="server" Text="Date From: "></asp:Label>            
           
                <asp:TextBox ID="txtDateF" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
                    Enabled="True" TargetControlID="txtDateF"  Format="dd/MM/yyyy">
                </asp:CalendarExtender>
               
    &nbsp; &nbsp;
    <asp:Label ID="Label1" runat="server" Text="Date To: "></asp:Label>            
           
                <asp:TextBox ID="txtDateT" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
                    Enabled="True" TargetControlID="txtDateT"  Format="dd/MM/yyyy">
                </asp:CalendarExtender>
   &nbsp;&nbsp;
    
    <asp:Button ID="btnLoad" runat="server" Text="Show" OnClick="btnLoad_Click" />
    
     <asp:CompareValidator id="dateValidator" runat="server" Font-Size="9px"
                  Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateF"
                  ErrorMessage="Invalid 'FROM' Date!">
                </asp:CompareValidator>
    <asp:CompareValidator id="CompareValidator1" runat="server" Font-Size="9px"
                  Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateT"
                  ErrorMessage="Invalid 'TO' Date!">
                </asp:CompareValidator>
   <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
        
        </div>
    </div>

<br />

        <asp:GridView ID="GridView2" runat="server"
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
        BorderWidth="1px" Width="100%" AutoGenerateColumns="False"
        CellPadding="3" >
            <RowStyle ForeColor="#000066" />
            <Columns>
            
            <asp:TemplateField HeaderText="SL">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
                <ItemStyle Width="2%" />
            </asp:TemplateField>
            
                <asp:BoundField HeaderText="Date" DataField="EntryDate" DataFormatString="{0:dd-MM-yyyy}" />
                <asp:BoundField HeaderText="Head Name" DataField="AccountsHeadName" />
                <asp:BoundField HeaderText="Transaction Description" 
                    DataField="VoucherRowDescription" />
                <asp:BoundField HeaderText="Amount" DataField="Amount" />
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <EmptyDataTemplate>
                No Income Data Found !
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        
    <asp:Panel ID="pnl" runat="server" Visible="false">
    <h4>Expenses</h4>
        <asp:GridView ID="GridView1" runat="server"
        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
        BorderWidth="1px" Width="100%"
        CellPadding="3" >
            <RowStyle ForeColor="#000066" />
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <EmptyDataTemplate>
                You Dont Have any Expenses!
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        </asp:Panel>
        
        <p style="text-align:center; font-weight:bold;">
        
        Total Income From <asp:Label ID="lblDtFm" runat="server"/> 
        To <asp:Label ID="lblDtTo" runat="server"/> : 
            <asp:Label ID="lblBalance" runat="server" Text=""></asp:Label>
        </p>
        
        
    </div>--%>

    </ContentTemplate>
    </asp:UpdatePanel>
    


</asp:Content>

