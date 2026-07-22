<%@ Page Language="C#" AutoEventWireup="true" CodeFile="11AccountStatus.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Collections - End Of Month Reports - 11 Account Status</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>End of Month Reports - 11 Account Status</h1>
        <p></p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.common.views.CommonNav.main_nav_select(this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select_section("Reports", this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.collection_reports_nav_select_section("End of Month Reports", version, this.section)%>
            <%= com.sp.rmmc.collections.views.Navigations.end_of_month_reports_nav_select_section("11 Account Status", version, this.section)%>
        </div>
    </div>
    <!-- Example row of columns -->    
    <div class="row">
    </div>
    <a name="list"></a>
    <div class="row">
        <div class="span12">
        <h1>11 Account Status (<%= this.accepted_bfs.Count %>)</h1>
        <br /><a href="#documentation">Documentation</a>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <div style="display:block;">
            <asp:LinkButton ID="LinkButton1" runat="server" Text="Export" onclick="lbExport_ClickAll"></asp:LinkButton>        
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Name</th>
                    <th>Due Date</th>
                    <th>Account Status</th>
                </tr>
                <% foreach (com.sp.rmmc.collections.models.Base11AccountStatus bf in accepted_bfs)
                   {
                %>
                <tr>
                    <td><%= bf.loan_id.ToString()%></td>
                    <td><%= bf.loan.loan_name%></td>
                    <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
                    <td><%= bf.account_status_desc%></td>
                 </tr>
                <%
                   }
                %>
            </table>
        </div>
        </div>
    </div>

    <br /><br /><br /><br /><br />
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <a name="documentation"></a>
            <b>11 Account Status Listing Documentation</b>
            </h3>
            <a href="#list">View List</a><br /><br />
            <a href="images/Requirements_11_AccountStatus.docx">Requirements</a>
        </p>
        </div>
    </div>
    
    </div>
    </div>
    </form>
</body>
</html>
