<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BankruptcyReport.aspx.cs" Inherits="lossmitigation_Default" %>

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
        <h1>End of Month Reports - Bankruptcy Report </h1>
        <h2><%= (this.history == null) ?  "Current" : "History: " + history.history_text %></h2>
        <p></p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.common.views.CommonNav.main_nav_select(this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select_section("Reports", this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.collection_reports_nav_select_section("End of Month Reports", version, this.section)%>
            <%= com.sp.rmmc.collections.views.Navigations.end_of_month_reports_nav_select_section("Bankruptcy Report", version, this.section)%>
        </div>
    </div>
    <!-- Example row of columns -->    
    <div class="row">
    </div>
    <a name="list"></a>
    <div class="row">
        <div class="span12">
        <h2>Bankruptcy Report (<%= accepted_bfs.Count%>)</h2>
        <asp:LinkButton ID="LinkButton1" runat="server" Text="Export" onclick="lbExport_Click"></asp:LinkButton>
        <br /><a href="#documentation">Documentation</a>
        
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Name</th>
                <th>Due Date</th>
                <th>Mortgage Status Code</th>
                <th>Loan Stop Code</th>
            </tr>
            <% foreach (com.sp.rmmc.collections.models.BaseCollection bf in accepted_bfs)
               {
            %>
            <tr>
                <td><%= bf.loan_id.ToString()%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.due_date_next_payment.ToString() %></td>
                <td><%= bf.mortgage_status == "67" ? "67 Chapter 13 Bankruptcy" : "" %></td>
                <td><%= bf.bankrupcty_ch_13_stop_code_id == 11 ? "56 Bankruptcy Chapter 13" : 
                        bf.bankrupcty_ch_13_stop_code_id == 1 ? "53 Bankruptcy Chapter 13" :
                        bf.bankrupcty_ch_13_stop_code_id == 14 ? "55 Bankruptcy Chapter 7" : ""%></td>
             </tr>
            <%
               }
            %>
        </table>
        </div>
    </div>

    
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <a name="documentation"></a>
            <b>Bankruptcy Report Documentation</b>
            </h3>
            <a href="#list">View List</a><br /><br />
            <a href="images/EOMBankruptcyReport.doc">Requirements</a>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if Non-Delinquent Loan has been reported as Bankruptcy.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/bankruptcy_report.png" alt="report logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Due Date of Next Payment</b><br />
            DB: (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment<br />
            <img src="images/due_date_of_next_payment.png" alt="Due Date of Next Payment"></img><br />
            <br /><br /><br />
            <b>Mortgage Status</b><br />
            DB: (select top 1 ms_credit_information.mortgage_status from ms_credit_information where ms_credit_information.loan_id = base.loan_id order by ms_credit_information.mortgage_status ) as mortgage_status<br />
            <img src="images/bankruptcy_motgage_status.png" alt="Mortgage Status"></img><br />
            <br /><br /><br />
            <b>Loan Stop Code</b><br />
            DB: (select top 1 mlsc.stop_code_id from ms_loan_stop_code mlsc where mlsc.stop_code_id = 11 and mlsc.loan_id = base.loan_id order by mlsc.stop_code_id ) as bankrupcty_ch_13_stop_code_id<br />
            <img src="images/bankruptcy_stop_code.png" alt="Loan Stop Code"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
    
    </div>
    </div>
    </form>
</body>
</html>
