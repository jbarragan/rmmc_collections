<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModificationAnalysisReport.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Collections - End Of Month Reports - Modification Analysis Report</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>End of Month Reports - Mods Completed Report </h1>
        <p></p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.common.views.CommonNav.main_nav_select(this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select_section("Reports", this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.collection_reports_nav_select_section("End of Month Reports", version, this.section)%>
            <%= com.sp.rmmc.collections.views.Navigations.end_of_month_reports_nav_select_section("Modification Analysis Report", version, this.section)%>
        </div>
    </div>
    <!-- Example row of columns -->    
    <div class="row">
    </div>
    <a name="list"></a>
    <div class="row">
        <div class="span12">
        <h1>Modification Analysis Report (<%= all.Count %>)</h1>
        <br /><a href="#documentation">Documentation</a>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <div style="display:block;">
            <asp:LinkButton ID="LinkButton1" runat="server" Text="Export" onclick="lbExport_ClickAll"></asp:LinkButton>        
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Name</th>
                    <th>Mod Date</th>
                    <th>Alert Desc</th>
                    <th>Modification Stop Desc</th>
                    <th>Loan Payment Stop Desc</th>
                    <th>Next Analysis Date</th>
                    <th># Months Shortage</th>
                    <th># Months Deficiency</th>
                </tr>
                <% foreach (com.sp.rmmc.collections.models.BaseModification bf in all)
                   {
                %>
                <tr>
                    <td><%= bf.loan_id.ToString()%></td>
                    <td><%= bf.loan.loan_name%></td>
                    <td><%= bf.loan_mod_date.ToShortDateString()%></td>
                    <td><%= bf.modification_alert_desc %></td>
                    <td><%= bf.modification_stop_desc %></td>
                    <td><%= bf.loan_payment_stop_desc %></td>
                    <td><%= bf.next_ti_analysis_date.ToShortDateString()%></td>
                    <td><%= bf.months_spread_shortage %></td>
                    <td><%= bf.months_spread_deficiency %></td>
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
            <b>Modification Analysis Documentation</b>
            </h3>
            <a href="#list">View List</a><br /><br />
            <a href="images/MODIFICATIONANALYSISDATE.doc">Requirements</a>
        </p>
        </div>
    </div>
    
    </div>
    </div>
    </form>
</body>
</html>
