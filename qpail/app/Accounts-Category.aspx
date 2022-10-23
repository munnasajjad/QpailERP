<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Accounts-Category.aspx.cs" Inherits="app_Accounts_Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">

    

                  <h3 class="page-title">Sub-Accounts</h3>
	<div class="row">
		<div class="col-md-6">
			<div class="portlet box red">
				<div class="portlet-title">
					<div class="caption"><i class="fa fa-reorder"></i> Sub-Accounts Setup</div>
                        <div class="tools">
                           <a href="javascript:;" class="collapse"></a>
                           <a href="#portlet-config" data-toggle="modal" class="config"></a>
                           <a href="javascript:;" class="reload"></a>
                           <a href="javascript:;" class="remove"></a>
                        </div>
                     </div>

                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass="" ></asp:Label>
                    
                    <div class="portlet-body form">
                         <div class="form-horizontal">
                       
                       <div class="control-group">
                            <label class="control-label">Accounts Group </label>
                            <div class="controls">                                
                                <asp:DropDownList ID="ddGroup" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="ddGroup_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Label ID="lblGID" runat="server"></asp:Label>
                            </div>
                        </div>
                       
                       <div class="control-group">
                            <label class="control-label">Sub A/C ID  </label>
                            <div class="controls">                                
                                <asp:TextBox ID="txtSAID" runat="server" Enabled="false" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                               <%--<asp:CalendarExtender ID="ce2" runat="server" 
                                    Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>--%>
                            </div>
                        </div>
                        
                        <div class="control-group">
                            <label class="control-label">Sub A/C Name</label>
                            <div class="controls">                                
                                 <asp:TextBox ID="txtSubAcc" runat="server" title="Write Sub-Account Name and press Enter" CssClass="span6 m-wrap"  ></asp:TextBox>
                            </div>
                        </div>
               
               
                        <div class="form-actions">
                            <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save New Head" OnClick="btnSave_Click" />
                             <%--<asp:Button ID="btnDelete" runat="server" Text="Delete This Schedule"  CssClass="btn red" 
                                onclick="btnDelete_Click" />
                                <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnDelete"  ConfirmText="confirm to delete this schedule ??"> 
                                 </asp:ConfirmButtonExtender> --%>       
                            <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" />                              
                         </div>
             
             </div>

                  </div>
                  </div>

               </div>


				<div class="col-md-6 ">
					<div class="portlet box green">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-reorder"></i>Saved Data</div>
								<div class="tools">
									<a href="javascript:;" class="collapse"></a>
									<a href="#portlet-config" data-toggle="modal" class="config"></a>
									<a href="javascript:;" class="reload"></a>
									<a href="javascript:;" class="remove"></a>
								</div>
							</div>
							<div class="portlet-body">
             


<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSource1">
        <Columns>
            <asp:BoundField DataField="AccountsID" HeaderText="Head ID" 
                SortExpression="AccountsID" />
            <asp:BoundField DataField="GroupName" HeaderText="Group Name" 
                SortExpression="GroupName" />
            <asp:BoundField DataField="AccountsName" HeaderText="Accounts Name" 
                SortExpression="AccountsName" />
        </Columns>
    </asp:GridView>
      
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" 
            
        SelectCommand="SELECT a.AccountsID, b.GroupName, a.AccountsName FROM Accounts a join AccountGroup b on a.GroupID=b.GroupID order by a.AccountsID">
        </asp:SqlDataSource>

</div></div></div></div>

</div>


</asp:Content>

