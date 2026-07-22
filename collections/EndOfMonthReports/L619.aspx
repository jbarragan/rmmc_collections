
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="L619.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Collections - End Of Month Reports - Bankruptcy Report</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>L619 Report</h1>
        <p>Description: L619 Ginne Mae-COVID-19 related Forbearance that were Current when Placed in Forbearance.</p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.common.views.CommonNav.main_nav_select(this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select_section("Reports", this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.collection_reports_nav_select_section("End of Month Reports", version, this.section)%>
            <%= com.sp.rmmc.collections.views.Navigations.end_of_month_reports_nav_select_section("MBA Quarterly Reports", version, this.section)%>
            <%= com.sp.rmmc.collections.views.Navigations.mba_quarterly_reports_nav_select_section("L619", version, this.section)%>
        </div>
    </div>
    <!-- Example row of columns -->    
    <div class="row">
    </div>
    <a name="list"></a>

    
    <div class="row">
        <div class="span12">
        <br /><a href="#documentation">Documentation</a>
        <h1><%= this.all_current.getQuarterTitle() %></h1>
        <p><%= this.all_current.getMetrics() %></p>
        <asp:LinkButton ID="lbExportAllCurrent" runat="server" Text="Export" onclick="lbExport_ClickAllCurrent"></asp:LinkButton>
        <h3><%= this.current_fha.title %></h3>
        <p><%= this.current_fha.getMetrics() %></p>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('current_fha').style.display = 'block'">Show Details</button>&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('current_fha').style.display = 'none'">Hide Details</button>
        <div id="current_fha" style="display:none">
            <asp:LinkButton ID="lbExportCurrentFHA" runat="server" Text="Export" onclick="lbExport_ClickCurrentFHA"></asp:LinkButton>
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Name</th>
                    <th>Loan Type</th>
                    <th>UPB</th>
                    <th>Forbearance Plan</th>
                    <th>Forbearance Start Date</th>
                    <th>Forbearance Borrower Request</th>
                    <th>Alert Desc</th>
                </tr>
                <% foreach (com.sp.rmmc.collections.models.BaseForbearance f in current_fha)
                   {
                %>
                <tr>
                    <td><%= f.loan_id.ToString()%></td>
                    <td><%= f.loan.loan_name%></td>
                    <td><%= f.loan.loan_type%></td>
                    <td><%= f.loan.prin_bal.ToString("C") %></td>
                    <td><%= f.forbearance_plan_name%></td>
                    <td><%= f.min_due_date.ToShortDateString() %></td>
                    <td><%= f.borrower_request_date.ToShortDateString() %></td>
                    <td><%= f.alert_desc%></td>
                 </tr>
                <%
                   }
                %>
            </table>
        </div>

        <h3><%= this.current_va.title %></h3>
        <p><%= this.current_va.getMetrics()%></p>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('current_va').style.display = 'block'">Show Details</button>&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('current_va').style.display = 'none'">Hide Details</button>
        <div id="current_va" style="display:none">
            <asp:LinkButton ID="lbCurrentVA" runat="server" Text="Export" onclick="lbExport_ClickCurrentVA"></asp:LinkButton>       
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Name</th>
                    <th>Loan Type</th>
                    <th>UPB</th>
                    <th>Forbearance Plan</th>
                    <th>Forbearance Start Date</th>
                    <th>Forbearance Borrower Request</th>
                    <th>Alert Desc</th>
                </tr>
                <% foreach (com.sp.rmmc.collections.models.BaseForbearance f in current_va)
                   {
                %>
                <tr>
                    <td><%= f.loan_id.ToString()%></td>
                    <td><%= f.loan.loan_name%></td>
                    <td><%= f.loan.loan_type%></td>
                    <td><%= f.loan.prin_bal.ToString("C") %></td>
                    <td><%= f.forbearance_plan_name%></td>
                    <td><%= f.min_due_date.ToShortDateString() %></td>
                    <td><%= f.borrower_request_date.ToShortDateString() %></td>
                    <td><%= f.alert_desc%></td>
                 </tr>
                <%
                   }
                %>
            </table>
        </div>
        </div>
     </div>

     <div class="row">
        <div class="span12">
        <br /><a href="#documentation">Documentation</a>
        <h1><%= this.all_previous.getQuarterTitle()%></h1>
        <p><%= this.all_previous.getMetrics() %></p>
        <asp:LinkButton ID="LinkButton1" runat="server" Text="Export" onclick="lbExport_ClickAllCurrent"></asp:LinkButton>
        <h3><%= this.previous_fha.title%></h3>
        <p><%= this.previous_fha.getMetrics()%></p>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('previous_fha').style.display = 'block'">Show Details</button>&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('previous_fha').style.display = 'none'">Hide Details</button>
        <div id="previous_fha" style="display:none">
            <asp:LinkButton ID="lbPreviousFHA" runat="server" Text="Export" onclick="lbExport_ClickPreviousFHA"></asp:LinkButton>
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Name</th>
                    <th>Loan Type</th>
                    <th>UPB</th>
                    <th>Forbearance Plan</th>
                    <th>Forbearance Start Date</th>
                    <th>Forbearance Borrower Request</th>
                    <th>Alert Desc</th>
                </tr>
                <% foreach (com.sp.rmmc.collections.models.BaseForbearance f in previous_fha)
                   {
                %>
                <tr>
                    <td><%= f.loan_id.ToString()%></td>
                    <td><%= f.loan.loan_name%></td>
                    <td><%= f.loan.loan_type%></td>
                    <td><%= f.loan.prin_bal.ToString("C") %></td>
                    <td><%= f.forbearance_plan_name%></td>
                    <td><%= f.min_due_date.ToShortDateString() %></td>
                    <td><%= f.borrower_request_date.ToShortDateString() %></td>
                    <td><%= f.alert_desc%></td>
                 </tr>
                <%
                   }
                %>
            </table>
        </div>

        <h3><%= this.previous_va.title%></h3>
        <p><%= this.previous_va.getMetrics()%></p>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('previous_va').style.display = 'block'">Show Details</button>&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('previous_va').style.display = 'none'">Hide Details</button>
        <div id="previous_va" style="display:none">
            <asp:LinkButton ID="lbExportPreviousVA" runat="server" Text="Export" onclick="lbExport_ClickPreviousVA"></asp:LinkButton>
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Name</th>
                    <th>Loan Type</th>
                    <th>UPB</th>
                    <th>Forbearance Plan</th>
                    <th>Forbearance Start Date</th>
                    <th>Forbearance Borrower Request</th>
                    <th>Alert Desc</th>
                </tr>
                <% foreach (com.sp.rmmc.collections.models.BaseForbearance f in previous_va)
                   {
                %>
                <tr>
                    <td><%= f.loan_id.ToString()%></td>
                    <td><%= f.loan.loan_name%></td>
                    <td><%= f.loan.loan_type%></td>
                    <td><%= f.loan.prin_bal.ToString("C") %></td>
                    <td><%= f.forbearance_plan_name%></td>
                    <td><%= f.min_due_date.ToShortDateString() %></td>
                    <td><%= f.borrower_request_date.ToShortDateString() %></td>
                    <td><%= f.alert_desc%></td>
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
            <b>Documentation</b>
            </h3>
            <a href="#list">View List</a><br /><br />
            <a href="images/MBAQuarterlyForbearanceReport.doc">Requirements</a>
        </p>
        </div>
    </div>
    
    </div>
    </div>
    </form>
</body>
</html>
