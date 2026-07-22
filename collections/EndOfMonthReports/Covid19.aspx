<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Covid19.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Collections - End Of Month Reports - ALL Loss Mitigation Codes</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>End of Month Reports - COVID-19</h1>
        <h2><%= (this.history == null) ?  "Current" : "History: " + history.history_text %></h2>
        <p></p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.common.views.CommonNav.main_nav_select(this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select_section("Reports", this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.collection_reports_nav_select_section("End of Month Reports", version, this.section)%>
            <%= com.sp.rmmc.collections.views.Navigations.end_of_month_reports_nav_select_section("COVID-19", version, this.section)%>
        </div>
    </div>
    <!-- Example row of columns -->    
    <div class="row">
    </div>
    
    <div class="row">
        <div class="span12">
        <h2>COVID-19 Loans (<%= accepted_bfs.Count%>)</h2>
        <asp:LinkButton ID="LinkButton1" runat="server" Text="Export" onclick="lbExport_Click"></asp:LinkButton>
        
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>Loan Type</th>
                <th>Mortgage Status Code</th>
                <th>Default Reason Code</th>
                <th>Mortgage Status Date</th>
                <th>Due Date</th>
                <th>End Date</th>
                <th>Monthly Payment Amount</th>
                <th>Unpaid Principal Balance</th>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.BaseCollection bf in accepted_bfs)
               {
            %>
            <tr>
                <td><%= bf.loan_id.ToString()%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.mortgage_status %></td>
                <td><%= bf.ci_reason_code %></td>
                <td><%= bf.mortgage_status_date.ToShortDateString() %></td>
                <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= bf.program_end_dt.ToShortDateString()%></td>
                <td><%= bf.monthly_payment_amount.ToString("C")%></td>
                <td><%= bf.loan.prin_bal.ToString("C")%></td>
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
