<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Collections - End Of Month Reports - <%= workflow %></title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>ACH - Current Loans with ACH Restrict Autopay Draft </h1>
        <h2><%= (this.history == null) ?  "Current" : "History: " + history.history_text %></h2>
        <p></p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.common.views.CommonNav.main_nav_select(this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select_section("Reports", this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.collection_reports_nav_select_section("ACH", version, this.section)%>
        </div>
    </div>
    <!-- Example row of columns -->    
    <div class="row">
    </div>
    
    <div class="row">
        <div class="span12">
        <h2>Current Loans with ACH Restrict Autopay Draft (<%= accepted_bfs.Count%>)</h2>
        <asp:LinkButton ID="LinkButton1" runat="server" Text="Export" onclick="lbExport_Click"></asp:LinkButton>
        
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>Loan Type</th>
                <th>Due Date</th>
                <th>UPB</th>
                <th>Restrict Autopay Draft</th>
                <th>Stop Code</th>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.BaseCollection bf in accepted_bfs)
               {
            %>
            <tr>
                <td><%= bf.loan_id.ToString()%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= bf.loan.prin_bal.ToString("C")%></td>
                <td>Y</td>
                <td><%= bf.stop_code_id > 0 ? bf.stop_code_description  : "" %></td>
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
