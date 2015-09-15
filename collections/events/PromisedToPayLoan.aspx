<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromisedToPayLoan.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Collections - Promised To Pay Loan</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Promised To Pay Loan</h1>
        <h2><%= "Current" %> <!-- (this.history == null) ?  "Current" : "History: " + history.history_text %>--></h2>
        <p></p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select("Events") %>
            <%= com.sp.rmmc.collections.views.Navigations.events_nav_select("Promised To Pay Loan", version)%>
        </div>
    </div>
    <!-- Example row of columns -->    
    <div class="row">
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
    
    <% if (this.loan_collection != null)
       {
    %>
    <div class="row">
        <div class="span12">
        <h2>Promised To Pay Information for loan <%=this.loan_collection.loan_id %></h2>
        <table class="table table-striped">
            <tr>
                <th>Condition</th>
                <th>Result</th>
            </tr>
            <tr>
                <% 
                this.individual_condition = this.loan_collection.loan.due_date_next_payment.date < this.event_date;
                this.overall_condition = this.overall_condition && this.individual_condition;
                %>
                <td>Is Delinquent? <%= this.loan_collection.loan.due_date_next_payment.ToString() %></td> 
                <td style="<%= this.individual_condition ? "background-color: LightGreen;" : "background-color: IndianRed;" %>"><%= this.individual_condition ? "true" : "false"%></td>
            </tr>
            <tr>
                <% 
                this.individual_condition = (this.accepted_bfs.Count > 0);
                this.overall_condition = this.overall_condition && this.individual_condition;
                %>
                <td>Is loan Collection or Demand?</td>
                <td style="<%= this.individual_condition ? "background-color: LightGreen;" : "background-color: IndianRed;" %>"><%= this.individual_condition ? "true" : "false"%></td>
            </tr>
            <tr>
                <% 
                this.individual_condition = this.loan_collection.isMortgageStatusValid();
                this.overall_condition = this.overall_condition && this.individual_condition;
                %>
                <td>Is Mortgage Status Valid? ('42', '11', '12', '', '67', '98') <%= this.loan_collection.mortgage_status %></td>
                <td style="<%= this.individual_condition ? "background-color: LightGreen;" : "background-color: IndianRed;" %>"><%= this.individual_condition ? "true" : "false"%></td>
            </tr>
            <tr>
                <% 
                this.individual_condition = this.loan_collection.promise_to_pay_events.last_memo_notify_dt.date < this.event_date.AddDays(1) && this.loan_collection.promise_to_pay_events.last_memo_notify_dt.isNull == false;
                this.overall_condition = this.overall_condition && this.individual_condition;
                %>
                <td>Is Latest Promised To Pay Notify Date active? <%= this.loan_collection.promise_to_pay_events.last_memo_notify_dt.ToString() %></td>
                <td style="<%= this.individual_condition ? "background-color: LightGreen;" : "background-color: IndianRed;" %>"><%= this.individual_condition ? "true" : "false"%></td>
            </tr>
            <tr>
                <% 
                this.individual_condition = (this.loan_collection.promise_to_pay_events.last_memo_notify_dt.date > this.loan_collection.promise_to_pay_events.last_right_party_contact_create_dt.date || this.loan_collection.promise_to_pay_events.last_right_party_contact_create_dt.isNull );
                this.overall_condition = this.overall_condition && this.individual_condition;
                %>
                <td>Is Latest "Right Party Contact" before Promised To Pay Notify Date? <%= this.loan_collection.promise_to_pay_events.last_right_party_contact_create_dt.ToString() %></td>
                <td style="<%= this.individual_condition ? "background-color: LightGreen;" : "background-color: IndianRed;" %>"><%= this.individual_condition ? "true" : "false"%></td>
            </tr>
            <tr>
                <% 
                this.individual_condition = (this.loan_collection.promise_to_pay_events.last_memo_notify_dt.date > this.loan_collection.promise_to_pay_events.no_contact_memo_031_create_dt.date || this.loan_collection.promise_to_pay_events.no_contact_memo_031_create_dt.isNull);
                this.overall_condition = this.overall_condition && this.individual_condition;
                %>
                <td>Is Latest "No Contact Memo" (031) before Promised To Pay Notify Date? <%= this.loan_collection.promise_to_pay_events.no_contact_memo_031_create_dt.ToString() %></td>
                <td style="<%= this.individual_condition ? "background-color: LightGreen;" : "background-color: IndianRed;" %>"><%= this.individual_condition ? "true" : "false"%></td>
            </tr>
            <tr><td><b>For Normal Promised To Pay List</b></td></tr>
            <tr>
                <% 
                this.individual_condition = (this.loan_collection.promise_to_pay_events.last_promise_to_pay_memo_create_dt.date > this.loan_collection.promise_to_pay_events.last_reg_paid_dt.date );
                %>
                <td>
                    Is Latest Regular Payment Date after Promised To Pay Created Date?
                    Last Regular Payment Date: <%= this.loan_collection.promise_to_pay_events.last_reg_paid_dt.ToString()%> -
                    Promised To Pay Memo Created Date: <%= this.loan_collection.promise_to_pay_events.last_promise_to_pay_memo_create_dt.ToString()%> 
                </td>
                <td style="<%= this.individual_condition ? "background-color: LightGreen;" : "background-color: IndianRed;" %>"><%= this.individual_condition ? "true" : "false"%></td>
            </tr>
            <tr>
                <% 
                this.individual_condition = this.overall_condition && this.individual_condition;
                %>
                <td>In Normal Promised To Pay Events List?</td>
                <td style="<%= this.individual_condition ? "background-color: LightGreen;" : "background-color: IndianRed;" %>"><%= this.individual_condition ? "true" : "false"%></td>
            </tr>
            <tr><td><b>For Beta Promised To Pay List</b></td></tr>
            <tr>
                <% 
                this.individual_condition = (this.loan_collection.promise_to_pay_events.last_promise_to_pay_memo_create_dt.date > this.loan_collection.promise_to_pay_events.last_current_paid_dt.date );
                %>
                <td>
                    Is Latest Current Payment Date after Promised To Pay Created Date?
                    Last Regular Payment Date: <%= this.loan_collection.promise_to_pay_events.last_current_paid_dt.ToString()%> -
                    Promised To Pay Memo Created Date: <%= this.loan_collection.promise_to_pay_events.last_promise_to_pay_memo_create_dt.ToString()%> 
                </td>
                <td style="<%= this.individual_condition ? "background-color: LightGreen;" : "background-color: IndianRed;" %>"><%= this.individual_condition ? "true" : "false"%></td>
            </tr>
            <tr>
                <% 
                this.individual_condition = this.overall_condition && this.individual_condition;
                %>
                <td>In Beta Promised To Pay Events List?</td>
                <td style="<%= this.individual_condition ? "background-color: LightGreen;" : "background-color: IndianRed;" %>"><%= this.individual_condition ? "true" : "false"%></td>
            </tr>
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
