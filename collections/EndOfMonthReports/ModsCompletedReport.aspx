<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModsCompletedReport.aspx.cs" Inherits="lossmitigation_Default" %>

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
        <h1>End of Month Reports - Mods Completed Report </h1>
        <p></p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.common.views.CommonNav.main_nav_select(this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select_section("Reports", this.section, new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.collections.views.Navigations.collection_reports_nav_select_section("End of Month Reports", version, this.section)%>
            <%= com.sp.rmmc.collections.views.Navigations.end_of_month_reports_nav_select_section("Mods Completed Report", version, this.section)%>
        </div>
    </div>
    <!-- Example row of columns -->    
    <div class="row">
    </div>
    <a name="list"></a>
    <div class="row">
        <div class="span12">
        <h1>Mods Completed - <%= this.one_year_or_more.title %> ( <%= this.one_year_or_more.Count %> - <%= this.one_year_or_more.Sum.ToString("C") %>)</h1>
        <br /><a href="#documentation">Documentation</a>
        <h2><%= this.one_year_or_more_90plus.title %> (<%= this.one_year_or_more_90plus.Count %> - <%= this.one_year_or_more_90plus.Sum.ToString("C") %>)</h2>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('one_year_or_more_90plus').style.display = 'block'">Show Details</button>&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('one_year_or_more_90plus').style.display = 'none'">Hide Details</button>
        <div id="one_year_or_more_90plus" style="display:none">
            <asp:LinkButton ID="LinkButton1" runat="server" Text="Export" onclick="lbExport_ClickOneYearOrMore90"></asp:LinkButton>        
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>invagency</th>
                    <th>Loan Name</th>
                    <th>Loan Type</th>
                    <th>Mod Plan</th>
                    <th>Hamp?</th>
                    <th>Mod Date</th>
                    <th>Mod Eff Date</th>
                    <th>Due Date</th>
                    <th>Mod Term</th>
                    <th>Mod UPB</th>
                    <th>Mod Rate</th>
                    <th>Mod P&I</th>
                    <th>Mod T&I</th>
                    <th>Mod Mat Dt</th>
                    <th>Alert Desc</th>
                </tr>
                <% foreach (com.sp.rmmc.collections.models.BaseModification bf in one_year_or_more_90plus)
                   {
                %>
                <tr>
                    <td><%= bf.loan_id.ToString()%></td>
                    <td><%= bf.invagency%></td>
                    <td><%= bf.loan.loan_name%></td>
                    <td><%= bf.loan.loan_type%></td>
                    <td><%= bf.mod_plan_name%></td>
                    <td><%= bf.hamp_yn%></td>
                    <td><%= bf.loan_mod_date.ToShortDateString()%></td>
                    <td><%= bf.mod_eff_date.ToShortDateString()%></td>
                    <td><%= bf.loan.due_date_next_payment.ToString() %></td>
                    <td><%= bf.term == 0 ? "" : bf.term.ToString()%></td>
                    <td><%= bf.loan.prin_bal.ToString("C")%></td>
                    <td><%= bf.mod_rate == 0M ? "" : bf.mod_rate.ToString("#.000")%></td>
                    <td><%= bf.mod_prin_int.ToString("C")%></td>
                    <td><%= bf.mod_tax_ins.ToString("C")%></td>
                    <td><%= bf.mod_maturity_date.ToShortDateString()%></td>
                    <td><%= bf.alert_desc%></td>
                 </tr>
                <%
                   }
                %>
            </table>
        </div>
        <h2><%= this.one_year_or_more_60.title %> (<%= this.one_year_or_more_60.Count%> - <%= this.one_year_or_more_60.Sum.ToString("C")%>)</h2>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('one_year_or_more_60').style.display = 'block'">Show Details</button>&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('one_year_or_more_60').style.display = 'none'">Hide Details</button>
        <div id="one_year_or_more_60" style="display:none">
            <asp:LinkButton ID="LinkButton2" runat="server" Text="Export" onclick="lbExport_ClickOneYearOrMore60"></asp:LinkButton>
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>invagency</th>
                    <th>Loan Name</th>
                    <th>Loan Type</th>
                    <th>Mod Plan</th>
                    <th>Hamp?</th>
                    <th>Mod Date</th>
                    <th>Mod Eff Date</th>
                    <th>Due Date</th>
                    <th>Mod Term</th>
                    <th>Mod UPB</th>
                    <th>Mod Rate</th>
                    <th>Mod P&I</th>
                    <th>Mod T&I</th>
                    <th>Mod Mat Dt</th>
                    <th>Alert Desc</th>
                </tr>
                <% foreach (com.sp.rmmc.collections.models.BaseModification bf in one_year_or_more_60)
                   {
                %>
                <tr>
                    <td><%= bf.loan_id.ToString()%></td>
                    <td><%= bf.invagency%></td>
                    <td><%= bf.loan.loan_name%></td>
                    <td><%= bf.loan.loan_type%></td>
                    <td><%= bf.mod_plan_name%></td>
                    <td><%= bf.hamp_yn%></td>
                    <td><%= bf.loan_mod_date.ToShortDateString()%></td>
                    <td><%= bf.mod_eff_date.ToShortDateString()%></td>
                    <td><%= bf.loan.due_date_next_payment.ToString() %></td>
                    <td><%= bf.term == 0 ? "" : bf.term.ToString()%></td>
                    <td><%= bf.loan.prin_bal.ToString("C")%></td>
                    <td><%= bf.mod_rate == 0M ? "" : bf.mod_rate.ToString("#.000")%></td>
                    <td><%= bf.mod_prin_int.ToString("C")%></td>
                    <td><%= bf.mod_tax_ins.ToString("C")%></td>
                    <td><%= bf.mod_maturity_date.ToShortDateString()%></td>
                    <td><%= bf.alert_desc%></td>
                 </tr>
                <%
                   }
                %>
            </table>
        </div>
        <h2><%= this.one_year_or_more_30.title %> (<%= this.one_year_or_more_30.Count%> - <%= this.one_year_or_more_30.Sum.ToString("C")%>)</h2>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('one_year_or_more_30').style.display = 'block'">Show Details</button>&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('one_year_or_more_30').style.display = 'none'">Hide Details</button>
        <div id="one_year_or_more_30" style="display:none">
            <asp:LinkButton ID="LinkButton3" runat="server" Text="Export" onclick="lbExport_ClickOneYearOrMore30"></asp:LinkButton>
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>invagency</th>
                    <th>Loan Name</th>
                    <th>Loan Type</th>
                    <th>Mod Plan</th>
                    <th>Hamp?</th>
                    <th>Mod Date</th>
                    <th>Mod Eff Date</th>
                    <th>Due Date</th>
                    <th>Mod Term</th>
                    <th>Mod UPB</th>
                    <th>Mod Rate</th>
                    <th>Mod P&I</th>
                    <th>Mod T&I</th>
                    <th>Mod Mat Dt</th>
                    <th>Alert Desc</th>
                </tr>
                <% foreach (com.sp.rmmc.collections.models.BaseModification bf in one_year_or_more_30)
                   {
                %>
                <tr>
                    <td><%= bf.loan_id.ToString()%></td>
                    <td><%= bf.invagency%></td>
                    <td><%= bf.loan.loan_name%></td>
                    <td><%= bf.loan.loan_type%></td>
                    <td><%= bf.mod_plan_name%></td>
                    <td><%= bf.hamp_yn%></td>
                    <td><%= bf.loan_mod_date.ToShortDateString()%></td>
                    <td><%= bf.mod_eff_date.ToShortDateString()%></td>
                    <td><%= bf.loan.due_date_next_payment.ToString() %></td>
                    <td><%= bf.term == 0 ? "" : bf.term.ToString()%></td>
                    <td><%= bf.loan.prin_bal.ToString("C")%></td>
                    <td><%= bf.mod_rate == 0M ? "" : bf.mod_rate.ToString("#.000")%></td>
                    <td><%= bf.mod_prin_int.ToString("C")%></td>
                    <td><%= bf.mod_tax_ins.ToString("C")%></td>
                    <td><%= bf.mod_maturity_date.ToShortDateString()%></td>
                    <td><%= bf.alert_desc%></td>
                 </tr>
                <%
                   }
                %>
            </table>
        </div>
        <h2><%= this.one_year_or_more_current.title%> (<%= this.one_year_or_more_current.Count%> - <%= this.one_year_or_more_current.Sum.ToString("C")%>)</h2>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('one_year_or_more_current').style.display = 'block'">Show Details</button>&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('one_year_or_more_current').style.display = 'none'">Hide Details</button>
        <div id="one_year_or_more_current" style="display:none">
        <asp:LinkButton ID="LinkButton4" runat="server" Text="Export" onclick="lbExport_ClickOneYearOrMoreCurrent"></asp:LinkButton>
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>invagency</th>
                    <th>Loan Name</th>
                    <th>Loan Type</th>
                    <th>Mod Plan</th>
                    <th>Hamp?</th>
                    <th>Mod Date</th>
                    <th>Mod Eff Date</th>
                    <th>Due Date</th>
                    <th>Mod Term</th>
                    <th>Mod UPB</th>
                    <th>Mod Rate</th>
                    <th>Mod P&I</th>
                    <th>Mod T&I</th>
                    <th>Mod Mat Dt</th>
                    <th>Alert Desc</th>
                </tr>
                <% foreach (com.sp.rmmc.collections.models.BaseModification bf in one_year_or_more_current)
                   {
                %>
                <tr>
                    <td><%= bf.loan_id.ToString()%></td>
                    <td><%= bf.invagency%></td>
                    <td><%= bf.loan.loan_name%></td>
                    <td><%= bf.loan.loan_type%></td>
                    <td><%= bf.mod_plan_name%></td>
                    <td><%= bf.hamp_yn%></td>
                    <td><%= bf.loan_mod_date.ToShortDateString()%></td>
                    <td><%= bf.mod_eff_date.ToShortDateString()%></td>
                    <td><%= bf.loan.due_date_next_payment.ToString() %></td>
                    <td><%= bf.term == 0 ? "" : bf.term.ToString()%></td>
                    <td><%= bf.loan.prin_bal.ToString("C")%></td>
                    <td><%= bf.mod_rate == 0M ? "" : bf.mod_rate.ToString("#.000")%></td>
                    <td><%= bf.mod_prin_int.ToString("C")%></td>
                    <td><%= bf.mod_tax_ins.ToString("C")%></td>
                    <td><%= bf.mod_maturity_date.ToShortDateString()%></td>
                    <td><%= bf.alert_desc%></td>
                 </tr>
                <%
                   }
                %>
            </table>
         </div>
        <br /><br /><br /><br />
        <h1>Mods Completed - <%= this.with_in_one_year.title %> ( <%= this.with_in_one_year.Count %> - <%= this.with_in_one_year.Sum.ToString("C")%>)</h1>
        <br /><a href="#documentation">Documentation</a>
        <h2><%= this.with_in_one_year_90plus.title%> (<%= this.with_in_one_year_90plus.Count%> - <%= this.with_in_one_year_90plus.Sum.ToString("C")%>)</h2>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('with_in_one_year_90plus').style.display = 'block'">Show Details</button>&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('with_in_one_year_90plus').style.display = 'none'">Hide Details</button>
        <div id="with_in_one_year_90plus" style="display:none">
            <asp:LinkButton ID="LinkButton5" runat="server" Text="Export" onclick="lbExport_ClickWithYear90"></asp:LinkButton>        
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>invagency</th>
                    <th>Loan Name</th>
                    <th>Loan Type</th>
                    <th>Mod Plan</th>
                    <th>Hamp?</th>
                    <th>Mod Date</th>
                    <th>Mod Eff Date</th>
                    <th>Due Date</th>
                    <th>Mod Term</th>
                    <th>Mod UPB</th>
                    <th>Mod Rate</th>
                    <th>Mod P&I</th>
                    <th>Mod T&I</th>
                    <th>Mod Mat Dt</th>
                    <th>Alert Desc</th>
                </tr>
                <% foreach (com.sp.rmmc.collections.models.BaseModification bf in with_in_one_year_90plus)
                   {
                %>
                <tr>
                    <td><%= bf.loan_id.ToString()%></td>
                    <td><%= bf.invagency%></td>
                    <td><%= bf.loan.loan_name%></td>
                    <td><%= bf.loan.loan_type%></td>
                    <td><%= bf.mod_plan_name%></td>
                    <td><%= bf.hamp_yn%></td>
                    <td><%= bf.loan_mod_date.ToShortDateString()%></td>
                    <td><%= bf.mod_eff_date.ToShortDateString()%></td>
                    <td><%= bf.loan.due_date_next_payment.ToString() %></td>
                    <td><%= bf.term == 0 ? "" : bf.term.ToString()%></td>
                    <td><%= bf.loan.prin_bal.ToString("C")%></td>
                    <td><%= bf.mod_rate == 0M ? "" : bf.mod_rate.ToString("#.000")%></td>
                    <td><%= bf.mod_prin_int.ToString("C")%></td>
                    <td><%= bf.mod_tax_ins.ToString("C")%></td>
                    <td><%= bf.mod_maturity_date.ToShortDateString()%></td>
                    <td><%= bf.alert_desc%></td>
                 </tr>
                <%
                   }
                %>
            </table>
        </div>
        <h2><%= this.with_in_one_year_60.title%> (<%= this.with_in_one_year_60.Count%> - <%= this.with_in_one_year_60.Sum.ToString("C")%>)</h2>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('with_in_one_year_60').style.display = 'block'">Show Details</button>&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('with_in_one_year_60').style.display = 'none'">Hide Details</button>
        <div id="with_in_one_year_60" style="display:none">
            <asp:LinkButton ID="LinkButton6" runat="server" Text="Export" onclick="lbExport_ClickWithYear60"></asp:LinkButton>
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>invagency</th>
                    <th>Loan Name</th>
                    <th>Loan Type</th>
                    <th>Mod Plan</th>
                    <th>Hamp?</th>
                    <th>Mod Date</th>
                    <th>Mod Eff Date</th>
                    <th>Due Date</th>
                    <th>Mod Term</th>
                    <th>Mod UPB</th>
                    <th>Mod Rate</th>
                    <th>Mod P&I</th>
                    <th>Mod T&I</th>
                    <th>Mod Mat Dt</th>
                    <th>Alert Desc</th>
                </tr>
                <% foreach (com.sp.rmmc.collections.models.BaseModification bf in with_in_one_year_60)
                   {
                %>
                <tr>
                    <td><%= bf.loan_id.ToString()%></td>
                    <td><%= bf.invagency%></td>
                    <td><%= bf.loan.loan_name%></td>
                    <td><%= bf.loan.loan_type%></td>
                    <td><%= bf.mod_plan_name%></td>
                    <td><%= bf.hamp_yn%></td>
                    <td><%= bf.loan_mod_date.ToShortDateString()%></td>
                    <td><%= bf.mod_eff_date.ToShortDateString()%></td>
                    <td><%= bf.loan.due_date_next_payment.ToString() %></td>
                    <td><%= bf.term == 0 ? "" : bf.term.ToString()%></td>
                    <td><%= bf.loan.prin_bal.ToString("C")%></td>
                    <td><%= bf.mod_rate == 0M ? "" : bf.mod_rate.ToString("#.000")%></td>
                    <td><%= bf.mod_prin_int.ToString("C")%></td>
                    <td><%= bf.mod_tax_ins.ToString("C")%></td>
                    <td><%= bf.mod_maturity_date.ToShortDateString()%></td>
                    <td><%= bf.alert_desc%></td>
                 </tr>
                <%
                   }
                %>
            </table>
        </div>
        <h2><%= this.with_in_one_year_30.title%> (<%= this.with_in_one_year_30.Count%> - <%= this.with_in_one_year_30.Sum.ToString("C")%>)</h2>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('with_in_one_year_30').style.display = 'block'">Show Details</button>&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('with_in_one_year_30').style.display = 'none'">Hide Details</button>
        <div id="with_in_one_year_30" style="display:none">
            <asp:LinkButton ID="LinkButton7" runat="server" Text="Export" onclick="lbExport_ClickWithYear30"></asp:LinkButton>
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>invagency</th>
                    <th>Loan Name</th>
                    <th>Loan Type</th>
                    <th>Mod Plan</th>
                    <th>Hamp?</th>
                    <th>Mod Date</th>
                    <th>Mod Eff Date</th>
                    <th>Due Date</th>
                    <th>Mod Term</th>
                    <th>Mod UPB</th>
                    <th>Mod Rate</th>
                    <th>Mod P&I</th>
                    <th>Mod T&I</th>
                    <th>Mod Mat Dt</th>
                    <th>Alert Desc</th>
                </tr>
                <% foreach (com.sp.rmmc.collections.models.BaseModification bf in with_in_one_year_30)
                   {
                %>
                <tr>
                    <td><%= bf.loan_id.ToString()%></td>
                    <td><%= bf.invagency%></td>
                    <td><%= bf.loan.loan_name%></td>
                    <td><%= bf.loan.loan_type%></td>
                    <td><%= bf.mod_plan_name%></td>
                    <td><%= bf.hamp_yn%></td>
                    <td><%= bf.loan_mod_date.ToShortDateString()%></td>
                    <td><%= bf.mod_eff_date.ToShortDateString()%></td>
                    <td><%= bf.loan.due_date_next_payment.ToString() %></td>
                    <td><%= bf.term == 0 ? "" : bf.term.ToString()%></td>
                    <td><%= bf.loan.prin_bal.ToString("C")%></td>
                    <td><%= bf.mod_rate == 0M ? "" : bf.mod_rate.ToString("#.000")%></td>
                    <td><%= bf.mod_prin_int.ToString("C")%></td>
                    <td><%= bf.mod_tax_ins.ToString("C")%></td>
                    <td><%= bf.mod_maturity_date.ToShortDateString()%></td>
                    <td><%= bf.alert_desc%></td>
                 </tr>
                <%
                   }
                %>
            </table>
        </div>
        <h2><%= this.with_in_one_year_current.title%> (<%= this.with_in_one_year_current.Count%> - <%= this.with_in_one_year_current.Sum.ToString("C")%>)</h2>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('with_in_one_year_current').style.display = 'block'">Show Details</button>&nbsp;&nbsp;
        <button type="button" class="btn btn-info" onclick="document.getElementById('with_in_one_year_current').style.display = 'none'">Hide Details</button>
        <div id="with_in_one_year_current" style="display:none">
        <asp:LinkButton ID="LinkButton8" runat="server" Text="Export" onclick="lbExport_ClickWithYearCurrent"></asp:LinkButton>
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>invagency</th>
                    <th>Loan Name</th>
                    <th>Loan Type</th>
                    <th>Mod Plan</th>
                    <th>Hamp?</th>
                    <th>Mod Date</th>
                    <th>Mod Eff Date</th>
                    <th>Due Date</th>
                    <th>Mod Term</th>
                    <th>Mod UPB</th>
                    <th>Mod Rate</th>
                    <th>Mod P&I</th>
                    <th>Mod T&I</th>
                    <th>Mod Mat Dt</th>
                    <th>Alert Desc</th>
                </tr>
                <% foreach (com.sp.rmmc.collections.models.BaseModification bf in with_in_one_year_current)
                   {
                %>
                <tr>
                    <td><%= bf.loan_id.ToString()%></td>
                    <td><%= bf.invagency%></td>
                    <td><%= bf.loan.loan_name%></td>
                    <td><%= bf.loan.loan_type%></td>
                    <td><%= bf.mod_plan_name%></td>
                    <td><%= bf.hamp_yn%></td>
                    <td><%= bf.loan_mod_date.ToShortDateString()%></td>
                    <td><%= bf.mod_eff_date.ToShortDateString()%></td>
                    <td><%= bf.loan.due_date_next_payment.ToString() %></td>
                    <td><%= bf.term == 0 ? "" : bf.term.ToString()%></td>
                    <td><%= bf.loan.prin_bal.ToString("C")%></td>
                    <td><%= bf.mod_rate == 0M ? "" : bf.mod_rate.ToString("#.000")%></td>
                    <td><%= bf.mod_prin_int.ToString("C")%></td>
                    <td><%= bf.mod_tax_ins.ToString("C")%></td>
                    <td><%= bf.mod_maturity_date.ToShortDateString()%></td>
                    <td><%= bf.alert_desc%></td>
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
            <b>Mods Completed Documentation</b>
            </h3>
            <a href="#list">View List</a><br /><br />
            <a href="images/QuarterlyModsDelinquentReport.doc">Requirements</a>
        </p>
        <p>
            <b>Purpose:</b><br />
            Categorize Mods Completed.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/mods_delinquent_report.png" alt="report logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Due Date of Next Payment</b><br />
            DB: (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = " + loan_id_column + " order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment<br />
            <img src="images/due_date_of_next_payment.png" alt="Due Date of Next Payment"></img><br />
            <br /><br /><br />
            <b>Mod Effective Date</b><br />
            DB: (select top 1 mlmti.mod_eff_date from ms_loan_mod_term_items mlmti where mlmti.loan_id = base.loan_id order by mlmti.mod_eff_date desc) as mod_eff_date<br />
            <img src="images/mod_effective_date.png" alt="Modification Effective Date"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
    
    </div>
    </div>
    </form>
</body>
</html>
