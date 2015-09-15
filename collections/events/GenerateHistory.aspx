<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GenerateHistory.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - History</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>History</h1>
        <h2><%= (this.history == null) ?  "Current" : "History: " + history.history_text %></h2>
        <p></p>
    </div>
    <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select("Events") %>
    <%= com.sp.rmmc.collections.views.Navigations.events_nav_select("History", version)%>
        
    <div class="row">
        <div class="span2"></div>
        <div class="span8">
            <h2>Select History:</h2>&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlHistories" runat="server" AutoPostBack="true"
                onselectedindexchanged="ddlHistories_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="span2">
        </div>        
        </div>
    </div>   
    <div class="row">
        <div class="span2"></div>
        <div class="span8">
            <h2>Create a new History</h2>
            Label: <asp:TextBox runat="server" AutoPostBack="true" ID="txtLabel"></asp:TextBox>
            <asp:Button ID="btnSave" 
                    runat="server"
                    Text="Save"
                    CssClass="btn btn-primary"
                    autoPostBack="true" onclick="btnSave_Click" />
            
            <p class="text-error"><asp:Label ID="lblError" runat="server" Text="" ></asp:Label></p>
            <p class="text-success"><asp:Label ID="lblSuccess" runat="server" Text="" ></asp:Label></p>
        </div>
    </div>

    </div>
    </div>
    </form>
</body>
</html>
