<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DelinquentFlow.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Task List: Delinquent Flow</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
    <script type="text/jscript" src="../../assets/js/jquery.js"></script>
    <script type="text/javascript">
        // Constants
    </script>
    <style type="text/css">
        td.is-success {
            color: #468847;
            background-color: #dff0d8 !important;
        }
        td.is-unsuccess {
            color: #884647;
            background-color: #f0dfd8 !important;
        }
    </style>
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
            <h2>Delinquent Flow</h2>
        </div>
        <div class="span9">
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select("Task List") %>
            <%= com.sp.rmmc.collections.views.Navigations.task_list_nav_select("Delinquent Grid") %>
        </div>
    </div>
    </div>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2>3-Month or less Delinquent (<%= in_three_month_delinquent.Count %> UPB: <%= in_three_month_delinquent_total.ToString("C") %>)</h2>
        <div id="in_three_month_delinquent_table">
        <h3></h3>
        <div id="in_three_month_delinquent">
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Type</th>
                <th>Due Date</th>
                <th>Last Mail Letter Date</th>
                <th>Last Inspection Date</th>
                <th>Last Call Date</th>
                <th>Call Result</th>
            </tr>
            <tr>
                <td></td>
                <td><asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" Width="70px"
                        onselectedindexchanged="ddlSelectedIndexChanged"></asp:DropDownList></td>                        
                <td></td>
                <td><asp:DropDownList ID="ddlLastMailLetter" runat="server" AutoPostBack="true" Width="70px"
                        onselectedindexchanged="ddlSelectedIndexChanged"></asp:DropDownList></td>
                <td><asp:DropDownList ID="ddlInspection" runat="server" AutoPostBack="true" Width="70px"
                        onselectedindexchanged="ddlSelectedIndexChanged"></asp:DropDownList></td>
                <td><asp:DropDownList ID="ddlLastCall" runat="server" AutoPostBack="true" Width="70px"
                        onselectedindexchanged="ddlSelectedIndexChanged"></asp:DropDownList></td>
                <td><asp:DropDownList ID="ddlLastCallResult" runat="server" AutoPostBack="true" Width="70px"
                        onselectedindexchanged="ddlSelectedIndexChanged"></asp:DropDownList></td>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.Delinquent d in in_three_month_delinquent)
               {
            %>
            <tr>
                <td><%= d.loan_id.ToString()%></td>
                <td><%= d.loan.loan_type%></td>
                <td><%= d.loan.due_date_next_payment.ToShortDateString() %></td>
                <td class="<%= d.last_mail_letter_valid() == false ? "is-unsuccess" : "is-success" %>"><%= d.last_mail_letter.ToShortDateString() %> </td>
                <td class="<%= d.last_inspection_valid() == false ? "is-unsuccess" : "is-success" %>"><%= d.last_inspection_memo_date.ToShortDateString() %> </td>
                <td class="<%= d.last_call_date_valid() == false ? "is-unsuccess" : "is-success" %>"><%= d.last_bc_call_memo_date.ToShortDateString() %> </td>
                <td class="<%= d.last_call_result_color() %>"><%= d.last_call_result() %> </td>
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
