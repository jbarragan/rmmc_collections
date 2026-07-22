<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CollectionReports.aspx.cs" Inherits="lossmitigation_Default" %>

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
        <h1><%= workflow %> <%= ( sublist != "ALL" ? " - " + this.sublist : "") %> <%= ( event_type != "ALL" ? "" + "" : "" ) %></h1>
        <h2><%= (this.history == null) ?  "Current" : "History: " + history.history_text %></h2>
        <p></p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.common.views.CommonNav.main_nav_select(this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select_section("Reports", this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.collection_reports_nav_select_section(workflow, version, this.section)%>
        </div>
    </div>
    <!-- Example row of columns -->    
    <div class="row">
        <div class="span4">
            Collector(s):&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlCollectors" runat="server" AutoPostBack="true"
                onselectedindexchanged="ddlCollectors_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="span4">
            Loan Type:&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlLoanType" runat="server" AutoPostBack="true"
                onselectedindexchanged="ddlLoanType_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="span4">
            Event Type:&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlEventType" runat="server" AutoPostBack="true"
                onselectedindexchanged="ddlEventType_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </div>
    
    <%
      if (this.workflow == "Different Reason Codes")
      {
    %>
    <div class="row">
        <div class="span12">
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
        <asp:LinkButton ID="LinkButton1" runat="server" Text="Export" onclick="lbExportReasonCode_Click"></asp:LinkButton>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Occupancy</th>
                <th>Reason Code</th>
                <th>Memo Reason Code</th>
                <th>031 Date</th>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.BaseCollection bf in bfs)
               {
            %>
            <tr class="<%= event_type == "Different Reason Codes" ? "" :  bf.getEventStatusColor() %>"> 
                <td><%= bf.loan_id.ToString()%></td>
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= bf.loan.prin_bal.ToString("C")%></td>
                <td><%= bf.loan.int_rate.ToString("0.000")%></td>
                <td><%= bf.loan.occupancy_code%></td>
                <td><%= bf.reason_code != null ? bf.reason_code.reason_code : "NA"%></td>
                <td><%= bf.reason_code != null ? bf.reason_code.last_memo_reason_code : "NA"%></td>
                <td><%= bf.reason_code != null ? bf.reason_code.memo_create_dt.ToString() : "NA"%></td>
             </tr>
            <%
               }
            %>
        </table>
        </div>
    </div>    
    <%
     }
     if (this.workflow == "Promised To Pay")
     {
    %>
    <div class="row">
        <div class="span12">
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
        <asp:LinkButton ID="LinkButton2" runat="server" Text="Export" onclick="lbExportPromisedToPay_Click"></asp:LinkButton>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>Notify me date on Memo</th>
                <th>Last 031 Date</th>
                <th>UPB</th>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.BaseCollection bf in bfs)
               {
            %>
            <tr> 
                <td><%= bf.loan_id.ToString()%></td>
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= (bf.promise_to_pay_events == null) ? "NA" : bf.promise_to_pay_events.last_memo_notify_dt.ToString() %></td>
                <td><%= (bf.promise_to_pay_events == null) ? "NA" : bf.promise_to_pay_events.no_contact_memo_031_create_dt.ToString() %></td>
                <td><%= bf.loan.prin_bal.ToString("C")%></td>
             </tr>
            <%
               }
            %>
        </table>
        </div>
    </div>
    <%
        }
        if (this.workflow == "4-Month Delinquent In Collections")
        {
           %>
       <div class="row">
        <div class="span12">
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
        <asp:LinkButton ID="LinkButton4" runat="server" Text="Export" onclick="lbExport_Click"></asp:LinkButton>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>Loan Type</th>
                <th>Due Date of next payment</th>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.BaseCollection bf in bfs)
               {
            %>
            <tr> 
                <td><%= bf.loan_id.ToString()%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
             </tr>
            <%
               }
            %>
        </table>
        </div>
      </div>
    <%
        }   
        if (this.workflow == "4-Month Delinquent NOT In Collections")
        {
    %>
       <div class="row">
        <div class="span12">
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
        <asp:LinkButton ID="LinkButton3" runat="server" Text="Export" onclick="lbExport_Click"></asp:LinkButton>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>Loan Type</th>
                <th>Due Date of next payment</th>
                <th>Where is the LOAN?</th>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.BaseCollection bf in bfs)
               {
            %>
            <tr> 
                <td><%= bf.loan_id.ToString()%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= bf.filter_reason %></td>
             </tr>
            <%
               }
            %>
        </table>
        </div>
      </div>
    <%
        }
        if (this.workflow == "FHA VA 17-day Call Listing" ||
            this.workflow == "Fannie Mae 17 day Call Listing" ||
            this.workflow == "Fannie Mae 17-day MCM Call Listing" ||
            this.workflow == "HUD 3 Month Call Listing" ||
            this.workflow == "Fannie Mae 3 Month Call Listing" ||
            this.workflow == "VA 3 Month Call Listing" ||
            this.workflow == "FHA 60 Day Call Listing" ||
            this.workflow == "Fannie Mae 2 Month Call Listing" ||
            this.workflow == "VA 2 Month Call Listing")
        {
    %>
       <div class="row">
        <div class="span12">
        <a name="default_list"></a>
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
        <a href="#collector_list">Collector List</a><br />
        <asp:LinkButton ID="lbExport" runat="server" Text="Export" onclick="lbExport_Click"></asp:LinkButton>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>Investor</th>
                <th>Collector</th>
                <% if (this.workflow == "HUD 3 Month Call Listing" ||
            this.workflow == "VA 3 Month Call Listing")
               { %>
                <th>Last Inspection</th>
            <% } %>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.BaseCollection bf in bfs)
               { 
            %>
            <tr> 
                <td><%= bf.loan_id.ToString()%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.collector.collector %></td>
                <% if (this.workflow == "HUD 3 Month Call Listing" ||
                       this.workflow == "VA 3 Month Call Listing")
               { %>
                <td><%= bf.last_property_inspection_date.ToString() %></td>
            <% } %>
             </tr>
            <%
               }
            %>
        </table>
        </div>
      </div>
      <div class="row">
        <div class="span12">
        <a name="collector_list"></a>
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
        <a href="#default_list">Default List</a>
        <table class="table table-striped">
            <% foreach (string collector in collector_loans.Keys)
               { 
            %>
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>Investor</th>
                <th>Collector</th>
            </tr>
            <tr class="info"><td colspan="5">Collector: <%=collector%> (<%= collector_loans[collector].Count %>)</td></tr>

                <% foreach (com.sp.rmmc.collections.models.BaseCollection bf in collector_loans[collector])
                   { 
                %>
                <tr> 
                    <td><%= bf.loan_id.ToString()%></td>
                    <td><%= bf.loan.loan_name%></td>
                    <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
                    <td><%= bf.loan.loan_type%></td>
                    <td><%= bf.collector.collector%></td>
                 </tr>
                <%
                   }
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
