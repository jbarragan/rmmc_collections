<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Delinquent.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Task List: Delinquent</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
    <script type="text/jscript" src="../../assets/js/jquery.js"></script>
    <script type="text/javascript">
        // Constants
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-print">
    <div style="background:White">
    <div class="row">
        <br />
        <div class="span3">
            <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
            <h1>Task List</h1>
            <h2>Delinquent Queue</h2>
        </div>
        <div class="span9">
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select("Task List") %>
            <%= com.sp.rmmc.collections.views.Navigations.task_list_nav_select("Delinquent Queue") %>
        </div>
    </div>
    </div>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2>3-Month (<%= in_three_month_delinquent.Count %> UPB: <%= in_three_month_delinquent_total.ToString("C") %>)
        <a class="btn btn-primary" onclick="var e = document.getElementById('in_three_month_delinquent_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
        <a class="btn btn-inverse" onclick="var e = document.getElementById('in_three_month_delinquent_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a>
        <asp:Button ID="btn3MonthDelinquentToCSV" runat="server" Text="CSV" 
                CssClass="btn btn-info" autoPostBack="true" 
                onclick="btn3MonthDelinquentToCSV_Click"/><br />

        </h2>
        <span id="in_three_month_delinquent_conditions" style="display:none"><%=com.sp.rmmc.collections.models.Delinquent.in_three_month_delinquent_conditions() %></span>
        <div id="in_three_month_delinquent_table" style="display:none">
        <h3>FHA (<%= in_three_month_delinquent_fha.Count%> UPB: <%= in_three_month_delinquent_fha_total.ToString("C") %>)<a class="btn btn-primary" onclick="var e = document.getElementById('in_three_month_delinquent_fha'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="in_three_month_delinquent_fha" style="display:none">
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>Loan Type</th>
                <th>Due Date</th>
                <th>Stop Description</th>
                <th>Due Date First Payment</th>
                <th>Total Payment</th>
                <th>Unapplied Balance</th>
                <th>Late Charge</th>
                <th>Principal Balance</th>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.Delinquent d in in_three_month_delinquent_fha)
               {
            %>
            <tr>
                <td><%= d.loan_id.ToString()%></td>
                <td><%= d.loan.loan_name%></td>
                <td><%= d.loan.loan_type%></td>
                <td><%= d.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= d.default_reason_code %></td>
                <td><%= d.due_date_first_payment.ToShortDateString()%></td>
                <td>NA</td>
                <td><%= d.unapplied_bal.ToString("C") %></td>
                <td><%= d.late_chrg_due_amt.ToString("C") %></td>
                <td><%= d.loan.prin_bal.ToString("C") %></td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        <h3>VA (<%= in_three_month_delinquent_va.Count %>  UPB: <%= in_three_month_delinquent_va_total.ToString("C") %>)<a class="btn btn-primary" onclick="var e = document.getElementById('in_three_month_delinquent_va'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="in_three_month_delinquent_va" style="display:none">
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>Loan Type</th>
                <th>Due Date</th>
                <th>Stop Description</th>
                <th>Due Date First Payment</th>
                <th>Total Payment</th>
                <th>Unapplied Balance</th>
                <th>Late Charge</th>
                <th>Principal Balance</th>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.Delinquent d in in_three_month_delinquent_va)
               {
            %>
            <tr>
                <td><%= d.loan_id.ToString()%></td>
                <td><%= d.loan.loan_name%></td>
                <td><%= d.loan.loan_type%></td>
                <td><%= d.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= d.default_reason_code %></td>
                <td><%= d.due_date_first_payment.ToShortDateString()%></td>
                <td>NA</td>
                <td><%= d.unapplied_bal.ToString("C") %></td>
                <td><%= d.late_chrg_due_amt.ToString("C") %></td>
                <td><%= d.loan.prin_bal.ToString("C") %></td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        <h3>CONV (<%= in_three_month_delinquent_conv.Count %>  UPB: <%= in_three_month_delinquent_conv_total.ToString("C") %>)<a class="btn btn-primary" onclick="var e = document.getElementById('in_three_month_delinquent_conv'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="in_three_month_delinquent_conv" style="display:none">
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>Loan Type</th>
                <th>Due Date</th>
                <th>Stop Description</th>
                <th>Due Date First Payment</th>
                <th>Total Payment</th>
                <th>Unapplied Balance</th>
                <th>Late Charge</th>
                <th>Principal Balance</th>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.Delinquent d in in_three_month_delinquent_conv)
               {
            %>
            <tr>
                <td><%= d.loan_id.ToString()%></td>
                <td><%= d.loan.loan_name%></td>
                <td><%= d.loan.loan_type%></td>
                <td><%= d.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= d.default_reason_code %></td>
                <td><%= d.due_date_first_payment.ToShortDateString()%></td>
                <td>NA</td>
                <td><%= d.unapplied_bal.ToString("C") %></td>
                <td><%= d.late_chrg_due_amt.ToString("C") %></td>
                <td><%= d.loan.prin_bal.ToString("C") %></td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="span12">
            <div><a href="../Default.aspx">Back to Home</a></div>
        </div>
    </div>
    </div>
    </form>
</body>
</html>
