<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/app/Layout.Master" CodeBehind="New-User.aspx.cs" Inherits="Oxford.app.New_User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">

    

            <div class="col-lg-6">
                <section class="panel">


    <h2>Open a New Admin Account</h2>
    <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" CssClass="table table-striped m-b-none text-sm"
        oncreateduser="CreateUserWizard1_CreatedUser" LoginCreatedUser="False">
        <WizardSteps>
            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
            </asp:CreateUserWizardStep>
            <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
            </asp:CompleteWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>
                    </section>
                </div>
</asp:Content>

