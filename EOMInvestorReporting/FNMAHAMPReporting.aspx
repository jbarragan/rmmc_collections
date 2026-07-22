<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FNMAHAMPReporting.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>EOM Investor Reporting - <%= workflow %></title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>EOM Investor Reporting - <%= workflow %></h1>
        <h2><%= (this.history == null) ?  "Current" : "History: " + history.history_text %></h2>
        <p></p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.common.views.CommonNav.main_nav_select(this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select_section("Reports", this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.collection_reports_nav_select_section("EOM Investor Reporting", version, this.section)%>
            <%= com.sp.rmmc.collections.views.Navigations.eom_investor_reporting_nav_select_section(workflow, version, this.section)%>
        </div>
    </div>
    <!-- Example row of columns -->    
    <div class="row">
    </div>
    
    <div class="row">
        <div class="span12">
        <h2><%=this.workflow %> (<%= delinquencies.Count%>)</h2>
        <br /><a href="#documentation">Documentation</a><br />
        <asp:LinkButton ID="LinkButton1" runat="server" Text="Export" onclick="lbExport_Click"></asp:LinkButton>
        <a name="list"></a>
        <table class="table table-striped" runat="server" id="tblBackruptcies"></table>
        </div>
    </div>
    
    <br /><br /><br /><br /><br />
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <a name="documentation"></a>
            <b>EOM Investor Reporting - <%= this.workflow %> Documentation</b>
            </h3>
            <a href="#list">View List</a><br /><br />
            <a href="images/FNMAHAMPReporting.doc">Requirements</a>
        </p>
        <p>
            <b>Purpose:</b><br />
            Report Modified loans with FNMA HAMP Loan Stop code and Loan Modification Alert
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>FNMA HAMP Modification Loan Stop code</b><br />
            DB: (select top 1 msc.stop_description from ms_stop_code msc, ms_loan_stop_code mlsc where mlsc.loan_id = base.loan_id and msc.stop_description like 'FNMA-HAMP MODIFICATION%' and msc.stop_code_id = mlsc.stop_code_id order by msc.stop_description ) as stop_code_description_FNMA_HAMP_MODIFICATION<br />
            <img src="images/fnma_hamp_modification.png" alt="FNMA HAMP Modification Loan Stop code"><br />
            <br /><br /><br />
            <b>Loan Modification Alert</b><br />
            DB: (select top 1 mla.alert_type from ms_loan_alerts mla join ms_loan_information mli on mli.alert_id = mla.alert_id where mli.loan_id = base.loan_id order by mla.alert_type DESC ) as alert_type<br />
            <img src="images/loan_alert.png" alt="Loan Modification Alert"><br />
            <br /><br /><br />
            <b>Last Paid Due Date</b><br />
            DB: (select top 1 hist.due_dt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' order by paid_dt desc) as last_reg_due_dt<br />
            <img src="images/last_reg_due_dt.png" alt="Last Paid Due Date"><br />
            <br /><br /><br />
            <b>Last Paid Principal Amount</b><br />
            DB: (select top 1 hist.prin_amt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' order by paid_dt desc) as last_reg_prin_amt<br />
            <img src="images/last_reg_prin_amt.png" alt="Last Paid Principal Amount"><br />
            <br /><br /><br />
            <b>Last Paid Interest Amount</b><br />
            DB: (select top 1 hist.int_amt from ms_loan_history hist where hist.loan_id = base.loan_id and hist.trans_type_cd = 'REG' order by paid_dt desc) as last_reg_int_amt<br />
            <img src="images/last_reg_int_amt.png" alt="Last Paid Interest Amount"><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
    
    </div>
    </div>
    </form>
</body>
</html>
