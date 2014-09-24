<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Collections - Home</title>
    <link href="../assets/css/bootstrapLM.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Collections</h1>
        <p>Home</p>
    </div>
    <%= com.sp.rmmc.collections.views.Navigations.main_nav %>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
            <p><a href="TaskList/Delinquent.aspx">Task List</a></p>
            <p class="text-error"><asp:Label ID="e" runat="server" Text="" ></asp:Label></p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
