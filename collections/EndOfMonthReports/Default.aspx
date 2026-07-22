<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>
            <% if (this.section == "QuarterReports")
               { %>
                Quarter Reports - <%= workflow%>
            <% }
              else
               { %>
               Collections - End Of Month Reports - <%= workflow%>
            <% } %>
    </title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>
        
            <% if (this.section == "QuarterReports")
               { %>
                Quarter Reports - <%= workflow%>
            <% }
              else
               { %>
                End of Month Reports - <%= workflow %>
            <% } %>
        </h1>
        <h2><%= (this.history == null) ?  "Current" : "History: " + history.history_text %></h2>
        <p></p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.common.views.CommonNav.main_nav_select(this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <% if (this.section == "QuarterReports")
               { %>
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select_section("Quarter Reports", this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.collection_quarter_reports_nav_select_section(workflow, version, this.section)%>
            <% }
               else
               { %>
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select_section("Reports", this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.collection_reports_nav_select_section("End of Month Reports", version, this.section)%>
            <%= com.sp.rmmc.collections.views.Navigations.end_of_month_reports_nav_select_section(workflow, version, this.section)%>
            <% } %>
        </div>
    </div>
    <!-- Example row of columns -->    
    <div class="row">
    </div>
    
    <%
                   if (this.workflow == "Bankruptcies" || this.workflow == "All")
                   {
    %>
    <div class="row">
        <div class="span12">
        <h2>Bankruptcies (<%= bankruptcies.Count%>)</h2>
        <asp:LinkButton ID="LinkButton1" runat="server" Text="Export" onclick="lbExportBankruptcies_Click"></asp:LinkButton>
        <table class="table table-striped" runat="server" id="tblBackruptcies"></table>
        </div>
    </div>
    <%
                   }
    %>
    
    
    <%
      if (this.workflow == "Demands" || this.workflow == "All")
      {
    %>
    <div class="row">
        <div class="span12">
        <h2>Demands (<%= demands.Count%>)</h2>
        <asp:LinkButton ID="LinkButton2" runat="server" Text="Export" onclick="lbExportDemands_Click"></asp:LinkButton>
        <table class="table table-striped" runat="server" id="tblDemands"></table>
        </div>
    </div>
    <%
        }
    %>
    
    
    <%
        if (this.workflow == "Foreclosures" || this.workflow == "All")
      {
    %>
    <div class="row">
        <div class="span12">
        <h2>Foreclosures (<%= foreclosures.Count%>)</h2>
        <asp:LinkButton ID="LinkButton3" runat="server" Text="Export" onclick="lbExportForeclosures_Click"></asp:LinkButton>
        
        <table class="table table-striped" runat="server" id="tblForeclosures"></table>
        </div>
    </div>
    <%
        }
    %>

    <%
        if (this.workflow == "FNMA")
      {
    %>
    <div class="row">
        <div class="span12">
        <h2>FNMA (<%= accepted_bfs.Count%>)</h2>
        <asp:LinkButton ID="LinkButton4" runat="server" Text="Export" onclick="lbExportFNMA_Click"></asp:LinkButton>
        
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>UPB</th>
            </tr>
            <% decimal total = 0.0M;
               foreach (com.sp.rmmc.collections.models.BaseCollection bf in accepted_bfs)
               {
                   total += bf.loan.prin_bal;
            %>
            <tr>
                <td><%= bf.loan_id.ToString()%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.prin_bal.ToString("C")%></td>
             </tr>
            <%
               }
            %>
        <tr class="info"><td>Total</td><td><%=accepted_bfs.Count%></td><td><%=total.ToString("C") %></td></tr>
        </table>
        </div>
    </div>
    <%
        }
    %>

    <%
        if (this.workflow == "GNMA")
      {
    %>
    <div class="row">
        <div class="span12">
        <h2>GNMA (<%= accepted_bfs.Count%>)</h2>
        <asp:LinkButton ID="LinkButton5" runat="server" Text="Export" onclick="lbExportGNMA_Click"></asp:LinkButton>
        <table class="table table-striped">
        
        <%  decimal total = 0.0M;
            int total_count = 0;
            foreach (CollectionGroup group in groupGNMAByLoanTypes(accepted_bfs))
            {
                if (group.total_count == 0) { continue; }       
        %>
            <tr class="success"><td colspan="4"><b><%=group.name %></b></td></tr>
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>Loan Type</th>
                <th>UPB</th>
            </tr>
            <% 
                foreach (com.sp.rmmc.collections.models.BaseCollection bf in group)
                {
                    total += bf.loan.prin_bal;
                    total_count++;
            %>
            <tr>
                <td><%= bf.loan_id.ToString()%></td>
                <td><%= bf.loan.loan_name %></td>
                <td><%= bf.loan.loan_type %></td>
                <td><%= bf.loan.prin_bal.ToString("C")%></td>
            </tr>
            <%
                }
            %>
        <tr class="info"><td colspan="2"><b><%= group.name %></b></td><td><b><%=group.Count%></b></td><td><b><%=group.total_amount.ToString("C")%></b></td></tr>
        <% } %>
        
        <tr class="success"><td colspan="4"><b>Summary</b></td></tr>
        <%
          foreach (CollectionGroup group in groupGNMAByLoanTypes(accepted_bfs))
          {
              if (group.total_count == 0) { continue; }       
        %>
        <tr class="info"><td colspan="2"><b><%= group.name%></b></td><td><b><%=group.Count%></b></td><td><b><%=group.total_amount.ToString("C")%></b></td></tr>
        <% } %>
        <tr class="info"><td colspan="2"><b>Total</b></td><td><b><%=total_count %></b></td><td><b><%=total.ToString("C")%></b></td></tr>
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
