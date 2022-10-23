<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="PurInvoice.aspx.cs" Inherits="app_PurInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

    <div class="row">
        <div class="col-xs-6">
            <h1>
                <a href="#">
                    <img src="logo.png">
                    Logo here
                </a>
            </h1>
        </div>
        <div class="col-xs-6 text-right">
            <h1>PURCHASE INVOICE</h1>
            <h1><small>Invoice # <asp:Literal ID="ltrInv" runat="server"/></small></h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-5">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>From: <a href="#"><asp:Literal ID="ltrCompany" runat="server"/></a></h4>
                </div>
                <div class="panel-body">
                    <p>
                        <asp:Literal ID="Literal1" runat="server"/>
                        <br>
                        <asp:Literal ID="Literal2" runat="server"/>
                        <br>
                        <asp:Literal ID="Literal3" runat="server"/>
                        <br>
                    </p>
                </div>
            </div>
        </div>
        <div class="col-xs-5 col-xs-offset-2 text-right">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>To : <a href="#">Client Name</a></h4>
                </div>
                <div class="panel-body">
                    <p>
                        Address
                        <br>
                        details
                        <br>
                        more
                        <br>
                    </p>
                </div>
            </div>
        </div>
    </div>
    <!-- / end client details section -->
    <table class="table table-bordered">
        <thead>
            <tr>
                <th>
                    <h4>Service</h4>
                </th>
                <th>
                    <h4>Description</h4>
                </th>
                <th>
                    <h4>Hrs/Qty</h4>
                </th>
                <th>
                    <h4>Rate/Price</h4>
                </th>
                <th>
                    <h4>Sub Total</h4>
                </th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Article</td>
                <td><a href="#">Title of your article here</a></td>
                <td class="text-right">-</td>
                <td class="text-right">$200.00</td>
                <td class="text-right">$200.00</td>
            </tr>
            <tr>
                <td>Template Design</td>
                <td><a href="#">Details of project here</a></td>
                <td class="text-right">10</td>
                <td class="text-right">75.00</td>
                <td class="text-right">$750.00</td>
            </tr>
            <tr>
                <td>Development</td>
                <td><a href="#">WordPress Blogging theme</a></td>
                <td class="text-right">5</td>
                <td class="text-right">50.00</td>
                <td class="text-right">$250.00</td>
            </tr>
        </tbody>
    </table>
    <div class="row text-right">
        <div class="col-xs-2 col-xs-offset-8">
            <p>
                <strong>Sub Total :
                    <br>
                    TAX :
                    <br>
                    Total :
                    <br>
                </strong>
            </p>
        </div>
        <div class="col-xs-2">
            <strong>$1200.00
                <br>
                N/A
                <br>
                $1200.00
                <br>
            </strong>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-5">
            <div class="panel panel-info">
                <div class="panel-heading">
                    <h4>Bank details</h4>
                </div>
                <div class="panel-body">
                    <p>Your Name</p>
                    <p>Bank Name</p>
                    <p>SWIFT : --------</p>
                    <p>Account Number : --------</p>
                    <p>IBAN : --------</p>
                </div>
            </div>
        </div>
        <div class="col-xs-7">
            <div class="span7">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        <h4>Contact Details</h4>
                    </div>
                    <div class="panel-body">
                        <p>
                            Email : you@example.com
                            <br>
                            <br>
                            Mobile : --------
                            <br>
                            <br>
                            Twitter : <a href="https://twitter.com/tahirtaous">@TahirTaous</a>
                        </p>
                        <h4>Payment should be made by Bank Transfer</h4>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

