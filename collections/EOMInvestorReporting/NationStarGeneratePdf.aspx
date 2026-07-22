<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NationStarGeneratePdf.aspx.cs" Inherits="lossmitigation_Default" %>

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
        <h2><%=  "Current"  %></h2>
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
        <br /><asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
        <a name="list"></a>
        </div>
    </div>
    
    </div>
    </div>
    </form>
</body>
</html>
