<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Events.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Collections - Events</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Events</h1>
        <h2><%= "Current" %> <!-- (this.history == null) ?  "Current" : "History: " + history.history_text %>--></h2>
        <p></p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select("") %>
        </div>
    </div>
    <!-- Example row of columns -->    
    <div class="row">
        <div class="span4">
            Loan Officer(s):&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlLoanOfficer" runat="server" AutoPostBack="true"
                onselectedindexchanged="ddlLoanOfficer_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="span4">
            Loan Type:&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlLoanType" runat="server" AutoPostBack="true"
                onselectedindexchanged="ddlLoanType_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="span4">            
            Loan Number:
            <div class="input-append">
                <asp:TextBox ID="txtLoanId" runat="server" Width="100px"></asp:TextBox>  
                <asp:Button ID="btnSearch" 
                    runat="server"
                    Text="Find"
                    CssClass="btn btn-primary"
                    autoPostBack="true" onclick="btnSearch_Click"/><br />
            </div>
            <p class="text-error"><asp:Label ID="lblError" runat="server" Text="" ></asp:Label></p>        
        </div>
    </div>    
    <div class="row">
        <div class="span4">
            Event Type:&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlEventType" runat="server" AutoPostBack="true" 
                onselectedindexchanged="ddlEventType_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </div>
    
    <div class="row">
        <div class="span12">
        <h2>Events (<%= events.Count %>)</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Foreclosure Stage</th>
                <!--<th>Officer(s)</th>-->
                <th>Event Name</th>
                <th>Event Type</th>
                <th>Event Description</th>
            </tr>
            <% foreach (com.sp.rmmc.common.models.events.Event e in events)
               {
            %>
            <tr class="<%= e.getEventStatusColor() %>">
                <td><%= e.loan.loan_id.ToString()%></td>
                <td><%= e.parent_type %></td>
                <!-- Foreclosure Officer<td></td> -->
                <td><%= e.name %> </td>
                <td><%= (e.type == 0 ? "Warning" : "Alert") %></td>
                <td><%= e.description %></td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
    </div>

    </div>
    </div>
    </form>
</body>
</html>
