<%@ Page Language="C#" AutoEventWireup="true" CodeFile="L610.aspx.cs" Inherits="lossmitigation_Default" %>

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
        <h1>L610 Report</h1>
        <p>L610: Forbearance Loans - Fannie Mae - 30 days Delinquent</p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.common.views.CommonNav.main_nav_select(this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select_section("Reports", this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.collection_reports_nav_select_section("End of Month Reports", version, this.section)%>
            <%= com.sp.rmmc.collections.views.Navigations.end_of_month_reports_nav_select_section("MBA Quarterly Reports", version, this.section)%>
            <%= com.sp.rmmc.collections.views.Navigations.mba_quarterly_reports_nav_select_section("L610", version, this.section)%>
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

        <h3><%= this.current_cnv.title %></h3>
        <p><%= this.current_cnv.getMetrics()%></p>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('current_cnv').style.display = 'block'">Show Details</button>&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('current_cnv').style.display = 'none'">Hide Details</button>
        <div id="current_cnv" style="display:block">
            <asp:LinkButton ID="lbExportCurrentCNV" runat="server" Text="Export" onclick="lbExport_ClickCurrentCNV"></asp:LinkButton>
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
                <% foreach (com.sp.rmmc.collections.models.BaseForbearance f in current_cnv)
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

        <h3><%= this.previous_cnv.title%></h3>
        <p><%= this.previous_cnv.getMetrics()%></p>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('previous_cnv').style.display = 'block'">Show Details</button>&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('previous_cnv').style.display = 'none'">Hide Details</button>
        <div id="previous_cnv" style="display:block">
            <asp:LinkButton ID="lbPreviousCNV" runat="server" Text="Export" onclick="lbExport_ClickPreviousCNV"></asp:LinkButton>
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
                <% foreach (com.sp.rmmc.collections.models.BaseForbearance f in previous_cnv)
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
