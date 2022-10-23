<%@ Page Language="C#" AutoEventWireup="true" CodeFile="unauthorized.aspx.cs" Inherits="unauthorized" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <link href="base.css" rel="stylesheet" type="text/css" />
    <link href="mocha.css" rel="stylesheet" type="text/css" />
    <title>Access Denied</title>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    <h3>Secured Area: Access Denied - <a href="Login.aspx?ReturnUrl=%2fSpider%2fcareer.aspx">Login</a> Required</h3>
        <a href="Login.aspx?ReturnUrl=%2fSpider%2fcareer.aspx"><img src="images/access-denied.jpg" /></a>
        <p align="center">You must <a href="Login.aspx?ReturnUrl=%2fSpider%2fcareer.aspx">Login</a> first to access this page</p>
    </div>
    </form>
</body>
</html>
