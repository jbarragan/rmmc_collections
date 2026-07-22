<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NationStarPdf.aspx.cs" Inherits="lossmitigation_Default" %>

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
        <h2><%= workflow %> (<%= delinquencies.Count%>)</h2>
        <br /><a href="#documentation">Documentation</a><br />
        <table class="table table-striped" runat="server" id="tblBackruptcies"></table>
        <asp:TextBox ID="loanNumberTxt" runat="server" Visible=false></asp:TextBox>
        <asp:Button ID="generatePDFBtn" runat="server" onclick="generatePDFBtn_Click" Text="Generate PDF"  Visible=false />
        <br /><asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
        <a name="list"></a>
        </div>
    </div>
    
    <br /><br /><br /><br /><br />
    <div class="row" style="display:none;">
        <div class="span12">
        <p>
            <h3>
            <a name="documentation"></a>
            <b>EOM Investor Reporting - Fannie Mae Delinquency Report Documentation</b>
            </h3>
            <a href="#list">View List</a><br /><br />
            <a href="images/FannieMaeDelinquencyReport.doc">Requirements</a>
        </p>
        <p>
            <b>Purpose:</b><br />
            Report Fannie Mae Delinquent loans.
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Due Date of Next Payment</b><br />
            DB: (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment<br />
            <img src="images/due_date_of_next_payment.png" alt="Due Date of Next Payment"><br />
            <br /><br /><br />
            <b>Mortgage Status</b><br />
            DB: (select top 1 ms_credit_information.mortgage_status from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.mortgage_status ) as mortgage_status<br />
            <img src="images/mortgage_status.png" alt="Due Date of Next Payment"><br />
            <br /><br /><br />
            <b>Default Reason Code</b><br />
            DB: (select top 1 ms_credit_information.default_reason_code from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.default_reason_code ) as ci_reason_code<br />
            <img src="images/default_reason_code.png" alt="Due Date of Next Payment"><br />
            <br /><br /><br />
            <b>Investor Code</b><br />
            DB: (select top 1 ms_investor_loan.inv_cd from ms_investor_loan where ms_investor_loan.loan_id = " + loan_id_column + " order by ms_investor_loan.inv_cd ) as loan_inv_cd<br />
            <img src="images/investor_code.png" alt="Investor Code"><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
    
    </div>
    </div>
    </form>
</body>
</html>
