<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Task List: Delinquent</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
    <script type="text/jscript" src="../../assets/js/jquery.js"></script>
    <script type="text/javascript">
        // Constants
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-print">
    <div style="background:White">
    <div class="row">
        <br />
        <div class="span3">
            <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
            <h1>Task List</h1>
            <h2>Delinquent Flow</h2>
        </div>
        <div class="span9">
            <%= com.sp.rmmc.collections.views.Navigations.sub_main_nav_select("Task List") %>
            <%= com.sp.rmmc.collections.views.Navigations.task_list_nav_select("Delinquent Flow")%>
        </div>
    </div>
    </div>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
            <p><img src="../../assets/img/collections_flow.png" class="img-rounded" alt="RMMC Collections Flow" /> </p>
            <p><a href="DelinquentFlow.aspx?type=ANY&letter=ANY&inspection=ANY&calldate=ANY&callresult=ANY">All 3-Month or Less Delinquent</a></p>
            <p><b>Mail Letters</b><br />
               <a href="DelinquentFlow.aspx?type=ANY&letter=VALID&inspection=ANY&calldate=ANY&callresult=ANY">With Mail letter</a><br />
               <a href="DelinquentFlow.aspx?type=ANY&letter=INVALID&inspection=ANY&calldate=ANY&callresult=ANY">With No Mail letter</a><br />
            </p>
            <p><b>Inspections</b><br />
               <a href="DelinquentFlow.aspx?type=ANY&letter=VALID&inspection=VALID&calldate=ANY&callresult=ANY">With Inspection</a><br />
               <a href="DelinquentFlow.aspx?type=ANY&letter=VALID&inspection=INVALID&calldate=ANY&callresult=ANY">With No Inspection</a><br />
            </p>
            <p><b>Calls</b><br />
               <a href="DelinquentFlow.aspx?type=ANY&letter=VALID&inspection=ANY&calldate=VALID&callresult=ANY">With Call</a><br />
               <a href="DelinquentFlow.aspx?type=ANY&letter=VALID&inspection=ANY&calldate=INVALID&callresult=ANY">With No Call</a><br />
            </p>
            <p><b>Call Results</b><br />
               <a href="DelinquentFlow.aspx?type=ANY&letter=VALID&inspection=ANY&calldate=VALID&callresult=CONTACT">With Contact</a><br />
               <a href="DelinquentFlow.aspx?type=ANY&letter=VALID&inspection=ANY&calldate=VALID&callresult=NO CONTACT">With No Contact</a><br />
            </p>
            <p class="text-error"><asp:Label ID="e" runat="server" Text="" ></asp:Label></p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
