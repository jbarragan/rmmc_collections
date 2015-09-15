<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Collections - <%= workflow %></title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1><%= workflow %> <%= ( sublist != "ALL" ? " - " + this.sublist : "") %> <%= ( event_type != "ALL" ? " - " + this.event_type : "" ) %></h1>
        <h2><%= "" %> <!-- //(this.history == null) ?  "Current" : "History: " + history.history_text %> --></h2>
        <p></p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select("EPD") %>
        </div>
    </div>
    <!-- Example row of columns -->
    <%if (this.workflow == "EPD")
      {
    %>
    <div class="row">
        <div class="span12">
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of first payment</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Occupancy</th>
                <th>Number of times 30-day or more late</th>
                <th>Number of times 60-day or more late</th>
                <th>Details</th>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.BaseEPD bf in bfs)
               {
            %>
            <tr> 
                <td><a href="http://localrmmc/devintranet/risk/LoanDetails.aspx?loanNumber=<%= bf.loan_id.ToString() %>"><%= bf.loan_id.ToString()%></a></td>
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.due_date_first_payment.ToShortDateString()%></td>
                <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= bf.loan.prin_bal.ToString("C")%></td>
                <td><%= bf.loan.int_rate.ToString("0.000")%></td>
                <td><%= bf.loan.occupancy_code %></td>
                <td><%= bf.count_of_30_day_late_payments %></td>
                <td><%= bf.count_of_60_day_late_payments %></td>
                <td><a href="http://localrmmc/devintranet/risk/LoanDetails.aspx?loanNumber=<%= bf.loan_id.ToString() %>">Details</a></td>
                
             </tr>
            <%
               }
            %>
        </table>
        </div>
    </div>    
    <%
        }
    %>
    
    </div>
    </div>
    </form>
</body>
</html>
