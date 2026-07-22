<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NoContactList.aspx.cs" Inherits="lossmitigation_Default" %>

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
            <a name="list"></a>
    
    <%
                   if (this.workflow == "No Contact List" || this.workflow == "All")
                   {
    %>
    <div class="row">
        <div class="span12">
        <h2>No Contact List (<%= no_contact_list.Count%>)</h2>
        <asp:LinkButton ID="LinkButton1" runat="server" Text="Export" onclick="lbExportNoContactList_Click"></asp:LinkButton>        
        <br /><a href="#documentation">Documentation</a>
        <table class="table table-striped" runat="server" id="tblNoContactList"></table>
        </div>
    </div>
    <%
                   }
    %>
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <a name="documentation"></a>
            <b>No Contact List Documentation</b>
            </h3>
            <a href="#list">View List</a><br /><br />
            <a href="images/NOCONTACTLISTING.doc">Requirements</a>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if Delinquent Loan has not been contacted in current month.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/no_contact_list.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Last Memo Type</b><br />
            DB: (select top 1 mmt.memo_type_desc from ms_loan_memo mlm left join ms_memo_types mmt on mmt.memo_type_id = mlm.memo_type_id where mlm.loan_id = base.loan_id and mlm.memo_subject like 'BC%' order by mlm.memo_create_dt desc) as last_memo_type_desc<br />
            <img src="images/Last Memo Type.png" alt="Last Memo Type"></img><br />
            <br /><br /><br />
            <b>Last Memo Category</b><br />
            DB: (select top 1 mmt.memo_type_desc from ms_loan_memo mlm left join ms_memo_types mmt on mmt.memo_type_id = mlm.memo_type_id where mlm.loan_id = base.loan_id and mlm.memo_subject like 'BC%' order by mlm.memo_create_dt desc) as last_memo_type_desc<br />
            <img src="images/Last Memo Category.png" alt="Last Memo Category"></img><br />
            <br /><br /><br />
            <b>Last Memo Contact</b><br />
            DB: (select top 1 mmt.memo_type_desc from ms_loan_memo mlm left join ms_memo_types mmt on mmt.memo_type_id = mlm.memo_type_id where mlm.loan_id = base.loan_id and mlm.memo_subject like 'BC%' order by mlm.memo_create_dt desc) as last_memo_type_desc<br />
            <img src="images/Last Memo Contact Type.png" alt="Last Memo Contat Type"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
    
    </div>
    </div>
    </form>
</body>
</html>
