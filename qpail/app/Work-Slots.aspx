<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Work-Slots.aspx.cs" Inherits="app_Work_Slots" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <script type="text/javascript">

        $(window).load(function () {
            jScript();
        });

        function jScript() {
            var bankExRate = $('#<%=txtPrice.ClientID%>').val();
            var cusExRate = $('#<%=txtConsumption.ClientID%>').val();
            var cfrBDT = parseFloat(bankExRate) * parseFloat(cusExRate);

            $('#<%=txtTotal.ClientID%>').val(cfrBDT.toString());
            $('#<%=txtTotal.ClientID%>').attr('readonly', true);
            $('#<%=txtNetTotal.ClientID%>').attr('readonly', true);
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(jScript);
                Sys.Application.add_load(callJquery);
            </script>
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">Sample Library
                    </h3>
                </div>
            </div>
            <div class="row">


                <div class="col-md-6 ">
                    <div class="portlet box blue">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Product Details
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

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                <asp:Label ID="lblId" runat="server" Visible="False"></asp:Label>

                                <div class="form-group">
                                    <label>Item Name: </label>
                                    <asp:DropDownList ID="ddItem" CssClass="form-control select2me" runat="server"
                                        DataSourceID="SqlDataSource2" DataTextField="GroupName" DataValueField="GroupSrNo"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddItem_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT DISTINCT [GroupSrNo], [GroupName] FROM [itemnamebm] ORDER BY [GroupName]"></asp:SqlDataSource>
                                </div>

                                <div class="form-group">
                                    <label>Program Name: </label>
                                    <asp:DropDownList ID="ddProgram" CssClass="form-control select2me" runat="server" 
                                        DataSourceID="SqlDataSource3" DataTextField="GroupName" DataValueField="GroupSrNo"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddItem_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT DISTINCT [GroupSrNo], [GroupName] FROM [programnamebm] ORDER BY [GroupName]"></asp:SqlDataSource>
                                </div>

                                <div class="form-group">
                                    <label>Fabrication: </label>
                                    <asp:DropDownList ID="ddFavrication" CssClass="form-control select2me" runat="server" 
                                        DataSourceID="SqlDataSource4" DataTextField="GroupName" DataValueField="GroupSrNo"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddItem_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT DISTINCT [GroupSrNo], [GroupName] FROM [fabricationnamebm] ORDER BY [GroupName]"></asp:SqlDataSource>
                                </div>


                                <div class="form-group">
                                    <label>Product Code: </label>
                                    <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" placeholder="Code" />
                                </div>
                                <div class="form-group">
                                    <label>Item Size: </label>
                                    <asp:DropDownList ID="ddSize" CssClass="form-control select2me" runat="server" 
                                        DataSourceID="SqlDataSource5" DataTextField="GroupName" DataValueField="GroupSrNo">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource5" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT DISTINCT [GroupSrNo], [GroupName] FROM [itemsizebm] ORDER BY [GroupName]"></asp:SqlDataSource>
                                </div>


                                <div class="form-group">
                                    <label>Product Unit: </label>
                                    <asp:DropDownList ID="ddUnit" CssClass="form-control select2me" runat="server"
                                        DataSourceID="SqlDataSource6" DataTextField="GroupName" DataValueField="GroupSrNo">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource6" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT DISTINCT [GroupSrNo], [GroupName] FROM [itemunitbm] ORDER BY [GroupName]"></asp:SqlDataSource>
                                </div>


                                <legend>Costing Info</legend>

                                <div class="form-group">
                                    <label>Costing Group: </label>
                                    <asp:DropDownList ID="ddGroup" CssClass="form-control select2me" runat="server"
                                        DataSourceID="SqlDataSource7" DataTextField="GroupName" DataValueField="GroupSrNo"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddGroup_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT DISTINCT [GroupSrNo], [GroupName] FROM [CostingGroups] ORDER BY [GroupName]"></asp:SqlDataSource>
                                </div>

                                <div class="form-group">
                                    <label>Costing Head: </label>
                                    <asp:DropDownList ID="ddhead" CssClass="form-control select2me" runat="server" DataSourceID="SqlDataSource8" DataTextField="HeadName" DataValueField="GroupSrNo">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource8" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT DISTINCT [GroupSrNo], [HeadName] FROM [CostingHeads] WHERE GroupName=@GroupName ORDER BY [HeadName]">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddGroup" Name="GroupName" PropertyName="SelectedvALUE" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>


                                <div class="form-group">
                                    <label>Unit for Costing: </label>
                                    <asp:DropDownList ID="ddCostUnit" CssClass="form-control select2me" runat="server"
                                        DataSourceID="SqlDataSource6" DataTextField="GroupName" DataValueField="GroupSrNo">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource9" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT DISTINCT [GroupSrNo], [GroupName] FROM [itemunitbm] ORDER BY [GroupName]"></asp:SqlDataSource>
                                </div>

                                <div class="form-group">
                                    <label>Unit Price: </label>
                                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder=" " onkeyup="jScript()" />
                                </div>

                                <div class="form-group">
                                    <label>Consumption: </label>
                                    <asp:TextBox ID="txtConsumption" runat="server" CssClass="form-control" placeholder="" onkeyup="jScript()" />
                                </div>

                                <div class="form-group">
                                    <label>Total Cost: </label>
                                    <asp:TextBox ID="txtTotal" runat="server" CssClass="form-control" placeholder="" />
                                </div>
                                
                                
                                <div class="form-group form-actions">
                                    <asp:Button ID="btnAdd" CssClass="btn blue" runat="server" Text="Add Costing Info" OnClick="btnAdd_OnClick" />

                                </div>
                                
                                <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>
                                <asp:Label ID="lblSl" runat="server" Visible="False" Text=""></asp:Label>
                                <asp:Label ID="lblCodeSl" runat="server" Visible="False" Text=" "></asp:Label>

                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"
                                    DataSourceID="SqlDataSource10" Width="100%" DataKeyNames="sl" OnRowCommand="FireRowCommand"
                                    OnSelectedIndexChanged="GridView2_OnSelectedIndexChanged" OnRowDeleting="GridView2_OnRowDeleting" OnRowDataBound="GridView2_OnRowDataBound">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GradeID" SortExpression="GradeID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("sl") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CostingGroupId" HeaderText="Group" SortExpression="Zone" />
                                        <asp:BoundField DataField="CostingHeadId" HeaderText="Head" SortExpression="Store Name" />
                                        <asp:BoundField DataField="UnitID" HeaderText="Unit" SortExpression="Owner" />
                                        <asp:BoundField DataField="UnitPrice" HeaderText="Price" SortExpression="Address" />

                                        <asp:BoundField DataField="Consumption" HeaderText="Consum." SortExpression="Owner" ItemStyle-HorizontalAlign="Right"  />
                                        <asp:BoundField DataField="TotalCost" HeaderText="Total Cost" SortExpression="Address" ItemStyle-HorizontalAlign="Right" />

                                        <asp:TemplateField ShowHeader="False" >
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Select to Edit" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />
                                                <asp:ImageButton ID="btnMoveUp" runat="server" CausesValidation="False" CommandName="MoveUp" CommandArgument='<%# Eval("sl") %>' ImageUrl="~/app/images/up.svg" Width="24px" ToolTip="Move to Up" />
                                                <asp:ImageButton ID="btnMoveDown" runat="server" CausesValidation="False" CommandName="MoveDown" CommandArgument='<%# Eval("sl") %>' ImageUrl="~/app/images/down.svg" Width="24px" ToolTip="Move to Down" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server"  DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">Item will be deleted permanently!</b><br />
                                                    <br />Are you sure to delete the item from list?<br /><br /><br /><br />
                                                    <div style="text-align: right;">
                                                        <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                        <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                    </div>
                                                </asp:Panel>
                                                
                                            </ItemTemplate>
                                            <ItemStyle Width="112px"></ItemStyle>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource10" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [sl], (Select GroupName from CostingGroups where GroupSrNo = ItemCostingDetail.CostingGroupId) as CostingGroupId, 
                                      (Select HeadName from CostingHeads where GroupSrNo = ItemCostingDetail.CostingHeadId) as CostingHeadId, 
                                      (Select GroupName from itemunitbm where GroupSrNo = ItemCostingDetail.UnitID) as UnitID, UnitPrice, Consumption, TotalCost, DisplaySerial 
                                                    FROM [ItemCostingDetail] Where ProductCode=@ProductCode ORDER BY [DisplaySerial]"
                                    DeleteCommand="Delete FROM [PrdnPowerPressDetails]  where PrdnID='0'  ">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="lblCodeSl" Name="ProductCode" PropertyName="Text" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                   

                                <legend>Others Info</legend>

                                <div class="form-group">
                                    <label>Net Cost: </label>
                                    <asp:TextBox ID="txtNetTotal" runat="server" CssClass="form-control" />
                                </div>

                                <div class="form-group">
                                    <label>Sample Photo: </label>
                                    <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank">
                                        <asp:Image ID="Image1" runat="server" Width="100%" EnableViewState="false" Visible="False" />
                                    </asp:HyperLink>
                                    <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" CssClass="form-control" />
                                </div>

                                <div class="form-group">
                                    <label>Remark/ Notes: </label>
                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" Rows="5" TextMode="MultiLine" placeholder="Description" />
                                </div>


                                <div class="form-actions">
                                    <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click1" />
                                    <asp:Button ID="btnClear" CssClass="btn default" runat="server" Text="Cancel" OnClick="btnClear_Click1" />
                                </div>

                            </div>
                        </div>
                    </div>

                </div>





                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Saved Data
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

                                <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                    DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="CID" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GradeID" SortExpression="GradeID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("cid") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ProductCode" HeaderText="Product Code" SortExpression="Zone" />
                                        <asp:BoundField DataField="Details" HeaderText="Description" SortExpression="Store Name" />
                                        <%--<asp:BoundField DataField="Owner" HeaderText="Amount" SortExpression="Owner" />
                                        <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />--%>

                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Select to Edit" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.png" Text="Delete" ToolTip="Delete" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">Item will be deleted permanently!</b><br />
                                                    <br />
                                                    Are you sure to delete the item from list?
                                                            <br />
                                                    <br />
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
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT [CID], ProductCode, Details, Size, Units, PhotoId FROM [itemcostingbm] Where ItemId=@ItemId AND ProgramId=@ProgramId AND FabricationId=@FabricationId ORDER BY [CID]"
                                    DeleteCommand="Delete FROM [PrdnPowerPressDetails]  where PrdnID='0'  ">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddItem" Name="ItemId" PropertyName="SelectedValue" />
                                        <asp:ControlParameter ControlID="ddProgram" Name="ProgramId" PropertyName="SelectedValue" />
                                        <asp:ControlParameter ControlID="ddFavrication" Name="FabricationId" PropertyName="SelectedValue" />
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


