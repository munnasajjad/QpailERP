<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Fact-Consum-Stat.aspx.cs" Inherits="app_Fact_Consum_Stat" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
    .xerp_text_area{
          width: 100%;
  border: 3px solid #cccccc;
  padding: 10px;
    }
    span#ctl00_ContentPlaceHolder1_ltrBody {
  width: 100%;
  border: 3px solid #cccccc;
  padding: 10px;
}
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>


            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">
                        <asp:Literal ID="ltrPageTitle" runat="server" Text="Raw Material Consumption Statistics"></asp:Literal>
                    </h3>
                </div>
            </div>
            <div class="row">


                <div class="col-md-12 ">
                    <div class="portlet box red">
                        


                        <div class="portlet-body form">
                            <div class="form-body">


                                <div class="col-md-4">
                                    <div class="control-group">
                                        <label class="col-sm-12 control-label">Item Type </label>

                                        <asp:DropDownList ID="ddType" runat="server" DataSourceID="SqlDataSource4"
                                            DataTextField="CategoryName" DataValueField="CategoryID"
                                            OnSelectedIndexChanged="ddType_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT CategoryID, CategoryName FROM [ItemSubGroup] where GroupID=2 ORDER BY [CategoryName]"></asp:SqlDataSource>
                                    </div>
                                </div>


                                <div class="col-md-4">
                                    <div class="control-group bottom_fix" style="margin-bottom: -15px !important;">
                                        <label class="col-sm-12 control-label">Category </label>

                                        <asp:DropDownList ID="ddCategory" runat="server" DataSourceID="SqlDataSource5"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged"
                                            DataTextField="CategoryName" DataValueField="CategoryID">
                                        </asp:DropDownList>

                                        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT CategoryID, CategoryName FROM Categories WHERE (GradeID IN (SELECT CategoryID FROM ItemGrade WHERE (CategoryID = @CategoryID))) ORDER BY [CategoryName]">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddType" Name="CategoryID" PropertyName="SelectedValue" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>


                                <div class="col-md-4">
                                    <div class="control-group">
                                        <label class="col-sm-12 control-label">Pack Size </label>

                                        <asp:DropDownList ID="ddSize" runat="server" DataSourceID="SqlDataSource2" DataTextField="BrandName" DataValueField="BrandID"
                                            OnSelectedIndexChanged="ddSize_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                            SelectCommand="SELECT [BrandID], [BrandName] FROM [Brands] WHERE ([ProjectID] = @ProjectID) order by DisplaySl">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblProject" Name="ProjectID" PropertyName="Text" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                    </div>
                                </div>



                
                                <div class="col-md-12">
                                    <div class="control-group">
                    <asp:Label ID="lblMsg"  runat="server" EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblProject" runat="server" Text="1" Visible="false"></asp:Label>
                
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="control-group">
                                        <label class="field_title">Declaration
                                            <asp:LinkButton ID="lbEdit" runat="server" OnClick="lbEdit_Click">Edit</asp:LinkButton>
                                            
                                            
                                        </label>
                                        <div class="form_input">
                                            <asp:Label ID="ltrBody" runat="server" CssClass="text xerp_text_area"></asp:Label>
                                            <asp:TextBox ID="txtMsgBody" runat="server" Width="100%" Height="450px" TabIndex="5" AutoFocus="False" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="form_grid_12">
                                        <div class="form_input">

                                            <asp:Button ID="btnSave" runat="server" Text="Save Info" TabIndex="11" class="btn_small btn_blue" OnClick="btnSave_Click" />

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
</asp:Content>

