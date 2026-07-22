<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CollectionQueues.aspx.cs" Inherits="lossmitigation_Default" %>

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
            <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Collections", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select("Collection Queues") %>
            <%= com.sp.rmmc.collections.views.Navigations.collection_queues_nav_select(workflow, version) %>
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
        if (this.workflow == "Old Collections template")
        {
    %>
       <div class="row">
        <div class="span12">
        <a name="default_list"></a>
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
        <a href="#collector_list">Collector List</a><br />
        <% if (this.workflow == "FHA VA 17-day Call Listing" ||
                this.workflow == "Fannie Mae 17 day Call Listing" ||
                this.workflow == "Fannie Mae 17-day MCM Call Listing" ||
                this.workflow == "HUD 3 Month Call Listing" ||
                this.workflow == "Fannie Mae 3 Month Call Listing" ||
                this.workflow == "VA 3 Month Call Listing" ||
                this.workflow == "FHA 60 Day Call Listing" ||
                this.workflow == "Fannie Mae 2 Month Call Listing" ||
                this.workflow == "VA 2 Month Call Listing")
           { %><a href="#documentation">Documentation</a><br /><% } %>
        <asp:LinkButton ID="lbExport" runat="server" Text="Export" onclick="lbExport_Click"></asp:LinkButton>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <% if (this.workflow == "FHA VA 17-day Call Listing" ||
                        this.workflow == "Fannie Mae 17 day Call Listing" ||
                        this.workflow == "Fannie Mae 17-day MCM Call Listing" ||
                        this.workflow == "HUD 3 Month Call Listing" ||
                        this.workflow == "Fannie Mae 3 Month Call Listing" ||
                        this.workflow == "VA 3 Month Call Listing" ||
                        this.workflow == "FHA 60 Day Call Listing" ||
                        this.workflow == "Fannie Mae 2 Month Call Listing" ||
                        this.workflow == "VA 2 Month Call Listing")
                   { %>
                <th>Promised to Pay</th>
                <% } %>
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
                <% if (this.workflow == "FHA VA 17-day Call Listing" ||
                        this.workflow == "Fannie Mae 17 day Call Listing" ||
                        this.workflow == "Fannie Mae 17-day MCM Call Listing" ||
                        this.workflow == "HUD 3 Month Call Listing" ||
                        this.workflow == "Fannie Mae 3 Month Call Listing" ||
                        this.workflow == "VA 3 Month Call Listing" ||
                        this.workflow == "FHA 60 Day Call Listing" ||
                        this.workflow == "Fannie Mae 2 Month Call Listing" ||
                        this.workflow == "VA 2 Month Call Listing")
                   { %>
                <td><%= (bf.last_memo_promise_to_pay_notify_dt.isNull == false && bf.last_memo_promise_to_pay_notify_dt.date > DateTime.Today) ? bf.last_memo_promise_to_pay_notify_dt.ToShortDateString() : ""%></td>
                <% } %>
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.collector.collector%></td>
                <% if (this.workflow == "HUD 3 Month Call Listing" ||
                       this.workflow == "VA 3 Month Call Listing")
                   { %>
                <td><%= bf.last_property_inspection_date.ToString()%></td>
            <% } %>
             </tr>
            <%
               }
            %>
           </table>
        <%
        }
            if (this.workflow == "Fannie Mae 17 day Call Listing" ||
                this.workflow == "Fannie Mae 17-day MCM Call Listing" ||
                this.workflow == "FHA VA 17-day Call Listing" ||
                this.workflow == "HUD 3 Month Call Listing" ||
                this.workflow == "VA 3 Month Call Listing" ||
                this.workflow == "FHA 60 Day Call Listing" ||
                this.workflow == "VA 2 Month Call Listing")
        {
    %>
       <div class="row">
        <div class="span12">
        <a name="default_list"></a>
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
        <a href="#documentation">Documentation</a><br />
        <asp:LinkButton ID="lbExport2" runat="server" Text="Export" onclick="lbExport_Click"></asp:LinkButton>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>Promised to Pay</th>
                <th>Investor</th>
                <th>Last Attempted Call Date</th>
                <th># of Attempts</th>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.BaseCollection bf in bfs)
               { 
            %>
            <tr> 
                <td><%= bf.loan_id.ToString()%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= ( bf.last_memo_promise_to_pay_notify_dt.isNull == false && bf.last_memo_promise_to_pay_notify_dt.date > DateTime.Today ) ? bf.last_memo_promise_to_pay_notify_dt.ToShortDateString() : "" %></td>
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.last_attempted_call_dt.ToShortDateString() %></td>
                <td><%= bf.current_month_attempted_calls %></td>
             </tr>
            <%
               }
            %>
        </table>
        </div>
      </div>
        <%
        }
            if (this.workflow == "Fannie Mae 3 Month Call Listing" ||
                this.workflow == "Fannie Mae 2 Month Call Listing")
        {
    %>
       <div class="row">
        <div class="span12">
        <a name="default_list"></a>
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
        <a href="#documentation">Documentation</a><br />
        <asp:LinkButton ID="LinkButton5" runat="server" Text="Export" onclick="lbExport_Click"></asp:LinkButton>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>Promised to Pay</th>
                <th>Investor</th>
                <th>Last Attempted Call Date</th>
                <th># of Attempts</th>
                <th>PMI Co.</th>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.BaseCollection bf in bfs)
               { 
            %>
            <tr> 
                <td><%= bf.loan_id.ToString()%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= ( bf.last_memo_promise_to_pay_notify_dt.isNull == false && bf.last_memo_promise_to_pay_notify_dt.date > DateTime.Today ) ? bf.last_memo_promise_to_pay_notify_dt.ToShortDateString() : "" %></td>
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.last_attempted_call_dt.ToShortDateString() %></td>
                <td><%= bf.current_month_attempted_calls %></td>
                <td><%= bf.pmi_company_name %></td>
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

    
<% if (this.workflow == "FHA 60 Day Call Listing")
   { %>
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <a name="documentation"></a>
            <b>FHA 60-day Calling List Documentation</b>
            </h3>
            <a href="#default_list">Default List</a><br />
            <a href="#collector_list">Collector List</a><br />
        </p>
        <p>
            <b>Purpose:</b><br />
            Identify Calling list for FHA 60-day delinquent loans.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/fha_60_day_calling_list.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Due Date of Next Payment</b><br />
            DB: (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment<br />
            <img src="images/due_date_of_next_payment.png" alt="Due Date of Next Payment"></img><br />
            <br /><br /><br />
            <b>Loan Type</b><br />
            DB: (select top 1 ms_loan_info.loan_type from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_type ) as ms_loan_type<br />
            <img src="images/fha_loan_type.png" alt="Loan Type"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
<%
   }
%>

<% if (this.workflow == "FHA VA 17-day Call Listing")
   { %>
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <a name="documentation"></a>
            <b>FHA VA 17-day Calling List Documentation</b>
            </h3>
            <a href="#default_list">Default List</a><br />
            <a href="#collector_list">Collector List</a><br />
        </p>
        <p>
            <b>Purpose:</b><br />
            Identify Calling list for FHA VA 17-day delinquent loans.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/fha_va_17_day_calling_list.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Due Date of Next Payment</b><br />
            DB: (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment<br />
            <img src="images/due_date_of_next_payment.png" alt="Due Date of Next Payment"></img><br />
            <br /><br /><br />
            <b>Loan Type</b><br />
            DB: (select top 1 ms_loan_info.loan_type from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_type ) as ms_loan_type<br />
            <img src="images/fha_loan_type.png" alt="Loan Type"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
<%
   }
%>

<% if (this.workflow == "Fannie Mae 17 day Call Listing")
   { %>
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <a name="documentation"></a>
            <b>Fannie Mae 17-day Calling List Documentation</b>
            </h3>
            <a href="#default_list">Default List</a><br />
            <a href="#collector_list">Collector List</a><br />
        </p>
        <p>
            <b>Purpose:</b><br />
            Identify Calling list for Fannie Mae 17-day delinquent loans.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/fannie_mae_17_day_calling_list.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Due Date of Next Payment</b><br />
            DB: (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment<br />
            <img src="images/due_date_of_next_payment.png" alt="Due Date of Next Payment"></img><br />
            <br /><br /><br />
            <b>Loan Type</b><br />
            DB: (select top 1 ms_loan_info.loan_type from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_type ) as ms_loan_type<br />
            <img src="images/fha_loan_type.png" alt="Loan Type"></img><br />
            <br /><br /><br />
            <b>Loan Plan Name</b><br />
            DB: (select top 1 mli.loan_plan_name from ms_loan_information mli where mli.loan_id = base.loan_id order by mli.loan_plan_name desc)  as loan_plan_name<br />
            <img src="images/loan_plan_name.png" alt="Loan Plan Name"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
<%
   }
%>


<% if (this.workflow == "Fannie Mae 17-day MCM Call Listing")
   { %>
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <a name="documentation"></a>
            <b>Fannie Mae 17-day MCM Calling List Documentation</b>
            </h3>
            <a href="#default_list">Default List</a><br />
            <a href="#collector_list">Collector List</a><br />
        </p>
        <p>
            <b>Purpose:</b><br />
            Identify Calling list for Fannie Mae 17-day delinquent loans.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/fannie_mae_17_day_mcm_calling_list.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Due Date of Next Payment</b><br />
            DB: (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment<br />
            <img src="images/due_date_of_next_payment.png" alt="Due Date of Next Payment"></img><br />
            <br /><br /><br />
            <b>Loan Type</b><br />
            DB: (select top 1 ms_loan_info.loan_type from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_type ) as ms_loan_type<br />
            <img src="images/fha_loan_type.png" alt="Loan Type"></img><br />
            <br /><br /><br />
            <b>Loan Plan Name</b><br />
            DB: (select top 1 mli.loan_plan_name from ms_loan_information mli where mli.loan_id = base.loan_id order by mli.loan_plan_name desc)  as loan_plan_name<br />
            <img src="images/loan_plan_name.png" alt="Loan Plan Name"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
<%
   }
%>

<% if (this.workflow == "Fannie Mae 2 Month Call Listing")
   { %>
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <a name="documentation"></a>
            <b>Fannie Mae 2 Month Calling List Documentation</b>
            </h3>
            <a href="#default_list">Default List</a><br />
            <a href="#collector_list">Collector List</a><br />
        </p>
        <p>
            <b>Purpose:</b><br />
            Identify Calling list for Fannie Mae 2 Month delinquent loans.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/conv_2_month_calling_list.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Due Date of Next Payment</b><br />
            DB: (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment<br />
            <img src="images/due_date_of_next_payment.png" alt="Due Date of Next Payment"></img><br />
            <br /><br /><br />
            <b>Loan Type</b><br />
            DB: (select top 1 ms_loan_info.loan_type from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_type ) as ms_loan_type<br />
            <img src="images/fha_loan_type.png" alt="Loan Type"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
<%
   }
%>

<% if (this.workflow == "VA 2 Month Call Listing")
   { %>
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <a name="documentation"></a>
            <b>VA 2 Month Calling List Documentation</b>
            </h3>
            <a href="#default_list">Default List</a><br />
            <a href="#collector_list">Collector List</a><br />
        </p>
        <p>
            <b>Purpose:</b><br />
            Identify Calling list for VA 2 Month delinquent loans.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/va_2_month_calling_list.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Due Date of Next Payment</b><br />
            DB: (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment<br />
            <img src="images/due_date_of_next_payment.png" alt="Due Date of Next Payment"></img><br />
            <br /><br /><br />
            <b>Loan Type</b><br />
            DB: (select top 1 ms_loan_info.loan_type from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_type ) as ms_loan_type<br />
            <img src="images/fha_loan_type.png" alt="Loan Type"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
<%
   }
%>

<% if (this.workflow == "HUD 3 Month Call Listing")
   { %>
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <a name="documentation"></a>
            <b>HUD 3 Month Calling List Documentation</b>
            </h3>
            <a href="#default_list">Default List</a><br />
            <a href="#collector_list">Collector List</a><br />
        </p>
        <p>
            <b>Purpose:</b><br />
            Identify Calling list for HUD 3 Month delinquent loans.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/hud_3_month_calling_list.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Due Date of Next Payment</b><br />
            DB: (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment<br />
            <img src="images/due_date_of_next_payment.png" alt="Due Date of Next Payment"></img><br />
            <br /><br /><br />
            <b>Loan Type</b><br />
            DB: (select top 1 ms_loan_info.loan_type from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_type ) as ms_loan_type<br />
            <img src="images/fha_loan_type.png" alt="Loan Type"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
<%
   }
%>

<% if (this.workflow == "Fannie Mae 3 Month Call Listing")
   { %>
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <a name="documentation"></a>
            <b>Fannie Mae 3 Month Calling List Documentation</b>
            </h3>
            <a href="#default_list">Default List</a><br />
            <a href="#collector_list">Collector List</a><br />
        </p>
        <p>
            <b>Purpose:</b><br />
            Identify Calling list for Fannie Mae 3 Month delinquent loans.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/conv_3_month_calling_list.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Due Date of Next Payment</b><br />
            DB: (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment<br />
            <img src="images/due_date_of_next_payment.png" alt="Due Date of Next Payment"></img><br />
            <br /><br /><br />
            <b>Loan Type</b><br />
            DB: (select top 1 ms_loan_info.loan_type from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_type ) as ms_loan_type<br />
            <img src="images/fha_loan_type.png" alt="Loan Type"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
<%
   }
%>

<% if (this.workflow == "VA 3 Month Call Listing")
   { %>
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <a name="documentation"></a>
            <b>VA 3 Month Calling List Documentation</b>
            </h3>
            <a href="#default_list">Default List</a><br />
            <a href="#collector_list">Collector List</a><br />
        </p>
        <p>
            <b>Purpose:</b><br />
            Identify Calling list for VA 3 Month delinquent loans.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/va_3_month_calling_list.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Due Date of Next Payment</b><br />
            DB: (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment<br />
            <img src="images/due_date_of_next_payment.png" alt="Due Date of Next Payment"></img><br />
            <br /><br /><br />
            <b>Loan Type</b><br />
            DB: (select top 1 ms_loan_info.loan_type from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.loan_type ) as ms_loan_type<br />
            <img src="images/fha_loan_type.png" alt="Loan Type"></img><br />
            <br /><br /><br />
        </p>
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
