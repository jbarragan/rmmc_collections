<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Different Reason Codes.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Event - <%= this.fileName %></title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1><%= this.fileName %></h1>
        <p></p>
    </div>
    <div class="row">
        <div class="span12">
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select("Events") %>
            <%= com.sp.rmmc.collections.views.Navigations.events_nav_select("Different Reason Code Doc", "current") %>
        </div>
    </div>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <b>Different Reason Code</b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine difference between Reason Code entry and last contact memo reason code.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/collection_different_reason_code.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Reason Code From Credit Information</b><br />
            DB: ms_credit_information.default_reason_code<br />
            <img src="images/credit_information_default_reason_code.png" alt="Credit Information Default Reason Code"></img><br />
            <br /><br /><br />
            <b>Reason Code From Last Contact Memo</b><br />
            DB: (select top 1 ms_memo_categories.memo_category_desc from ms_loan_memo memo, ms_memo_categories, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_category_id = ms_memo_categories.memo_category_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_categories.memo_category_desc like '0%' and ms_memo_categories.memo_category_desc not like '031%'  order by memo_create_dt desc ) as reason_code_events_last_memo_reason_code<br />
            <img src="images/last_contact_memo_reason_code.png" alt="Last Memo Reason Code"></img><br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
