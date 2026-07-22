using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for Navigations
/// </summary>
namespace com.sp.rmmc.collections.views
{
    public class Navigations
    {
        public Navigations()
        {
        }

        public static string main_nav =
    @"<div class=""navbar"">
       <div class=""navbar-inner"">
        <ul class=""nav"">
          <li  class=""active"">
            <a href=""Default.aspx"">Main Home</a>
          </li>
          <li>
            <a href=""TaskList/Default.aspx"">Task List</a>
          </li>
          <li>
            <a href=""events/CollectionQueues.aspx"">Collection Queues</a>
          </li>
          <li>
            <a href=""events/CollectionReports.aspx"">Collection Reports</a>
          </li>
        </ul>
       </div>
      </div>";

        public static string sub_main_nav =
    @"<div class=""navbar"">
       <div class=""navbar-inner"">
        <ul class=""nav"">
          <li>
            <a href=""../Default.aspx"">Main Home</a>
          </li>
          <li>
            <a href=""../TaskList/Delinquent.aspx"">Task List</a>
          </li>
        </ul>
       </div>
      </div>";

        private static string nav_start = @"<div class=""navbar""><div class=""navbar-inner""><ul class=""nav"">";
        private static string nav_end = @"</ul></div></div>";
        class Nav
        {
            public string link = "";
            public string text = "";
            public List<string> ad_users = new List<string>();
            public List<string> ad_departments = new List<string>();
            public List<string> ad_groups = new List<string>();

            public string link_html(string selected_text, ADUser ad_user)
            {
                string s = "";
                if (with_access(ad_user) == false) return s;
                string active_class = "";
                if (selected_text.ToUpper() == text.ToUpper()) active_class = @" class=""active""";
                s += @"<li" + active_class + @"><a href=""" + link + @""">" + text + "</a></li>";
                return s;
            }

            public bool with_access(ADUser ad_user)
            {
                if (ad_user == null) return true;
                if (this.ad_users.Count == 0 && this.ad_departments.Count == 0 && this.ad_groups.Count == 0) return true;
                if (ad_user.inAnyUser(this.ad_users)) return true;
                if (ad_user.inAnyDepartments(this.ad_departments)) return true;
                if (ad_user.inAnyGroups(this.ad_groups)) return true;
                return false;
            }
        }

        public static string sub_main_nav_select(string active)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "../Default.aspx"; nav.text = "Main Home"; navs.Add(nav);
            nav = new Nav(); nav.link = "../TaskList/Delinquent.aspx"; nav.text = "Task List"; navs.Add(nav);
            nav = new Nav(); nav.link = "../Events/CollectionQueues.aspx"; nav.text = "Collection Queues"; navs.Add(nav);
            nav = new Nav(); nav.link = "../Events/CollectionReports.aspx"; nav.text = "Collection Reports"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../Events/Default.aspx"; nav.text = "Events"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../epd/Default.aspx"; nav.text = "EPD"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../EndOfMonthReports/Default.aspx"; nav.text = "End of Month Reports"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../ims/Default.aspx"; nav.text = "IMS"; navs.Add(nav);
            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }

        public static string sub_main_nav_select_section(string active, string section, ADUser ad_user)
        {
            string s = nav_start;
            if (section == "" || section == "Collections")
            {
                List<Nav> navs = new List<Nav>();
                Nav nav = new Nav(); nav.link = "../Default.aspx"; nav.text = "Main Home"; navs.Add(nav);
                nav = new Nav(); nav.link = "../TaskList/Delinquent.aspx"; nav.text = "Task List"; navs.Add(nav);
                nav = new Nav(); nav.link = "../Events/CollectionQueues.aspx"; nav.text = "Collection Queues"; navs.Add(nav);
                nav = new Nav(); nav.link = "../Events/CollectionReports.aspx"; nav.text = "Reports"; navs.Add(nav);
                //nav = new Nav(); nav.link = "../Events/Default.aspx"; nav.text = "Events"; navs.Add(nav);
                //nav = new Nav(); nav.link = "../epd/Default.aspx"; nav.text = "EPD"; navs.Add(nav);
                //nav = new Nav(); nav.link = "../EndOfMonthReports/Default.aspx"; nav.text = "End of Month Reports"; navs.Add(nav);
                //nav = new Nav(); nav.link = "../ims/Default.aspx"; nav.text = "IMS"; navs.Add(nav);
                string active_class = "";
                foreach (Nav n in navs)
                {
                    if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                    s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
                }
            }
            if (section == "Foreclosures" || section == "QuarterReports" )
            {
                List<Nav> navs = new List<Nav>();
                Nav nav = new Nav(); nav.link = "../../foreclosures/Default.aspx"; nav.text = "Main Home"; navs.Add(nav);
                nav = secure_nav(); nav.link = "../../foreclosures/Workflow/Default.aspx"; nav.text = "Workflow"; navs.Add(nav);
                nav = secure_nav(); nav.link = "../../foreclosures/Metrics/Claims.aspx"; nav.text = "Metrics"; navs.Add(nav);
                nav.ad_departments.Add("mis");
                nav.ad_groups.Add("servicing_fc"); nav.ad_groups.Add("foreclosure_metrics");
                nav = secure_nav(); nav.link = "../../foreclosures/Utilities/PreservationCodes.aspx"; nav.text = "Utilities"; navs.Add(nav);
                nav = secure_nav(); nav.link = "../../foreclosures/Management/Gl1806Summary.aspx"; nav.text = "Management"; navs.Add(nav);
                nav = secure_nav(); nav.link = "../../foreclosures/Default.aspx"; nav.text = "Dashboard"; navs.Add(nav);
                nav = new Nav(); nav.link = "../Events/CollectionReports.aspx?section=Foreclosures"; nav.text = "Reports"; navs.Add(nav);
                nav = secure_nav(); nav.link = "../../foreclosures/QuarterReports/Default.aspx"; nav.text = "Quarter Reports"; navs.Add(nav);
                foreach (Nav n in navs)
                {
                    s += n.link_html(active, ad_user);
                }
            }
            if (section == "Management")
            {
                List<Nav> navs = new List<Nav>();
                Nav nav = secure_nav(); nav.link = "../../servicing_management/Default.aspx"; nav.text = "Main Home"; navs.Add(nav);
                nav = secure_nav(); nav.link = "../../servicing_management/Metrics1806/Default.aspx"; nav.text = "Metrics 1806"; navs.Add(nav);
                nav = secure_nav(); nav.link = "../../servicing_management/Metrics1807/Default.aspx"; nav.text = "Metrics 1807"; navs.Add(nav);
                nav = secure_nav(); nav.link = "../../servicing_management/Metrics1808/Gl1808Summary.aspx"; nav.text = "Metrics 1808"; navs.Add(nav);
                nav = secure_nav(); nav.link = "../../servicing_management/pmi/Default.aspx"; nav.text = "PMI"; navs.Add(nav);
                nav = new Nav(); nav.link = "../Events/CollectionReports.aspx?section=Management"; nav.text = "Reports"; navs.Add(nav);
                foreach (Nav n in navs)
                {
                    s += n.link_html(active, ad_user);
                }
            }
            return s + nav_end;
        }


        private static Nav secure_nav()
        {
            Nav nav = new Nav();
            nav.ad_groups.Add("gl1806");
            nav.ad_departments.Add("mis");
            return nav;
        }

        public static string events_nav_select(string active, string version)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "Default.aspx?version=" + version; nav.text = "Reason Codes"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=Promised%20To%20Pay%20Expired&version=" + version; nav.text = "Promised To Pay"; navs.Add(nav);
            //nav = new Nav(); nav.link = "Default.aspx?workflow=Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=Using%20Current%20Promised%20To%20Pay%20Expired&version=" + version; nav.text = "Beta Promised To Pay using last current"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=4-Month%20Delinquent%20In%20Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "4-Month Delinquency Report In Collections"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=4-Month%20Delinquent%20NOT%20In%20Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "4-Month Delinquency Report NOT In Collections"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=FHA%20VA%2017-day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "FHA VA 17-day Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Fannie%20Mae%2017%20day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 17 day Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Fannie%20Mae%2017-day%20MCM%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 17-day MCM Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=FHA%2060%20Day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "FHA 60 Day Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Fannie%20Mae%202%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 2 Month Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=VA%202%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "VA 2 Month Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=HUD%203%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "HUD 3 Month Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Fannie%20Mae%203%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 3 Month Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=VA%203%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "VA 3 Month Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "History.aspx?version=" + version; nav.text = "History"; navs.Add(nav);
            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }

        public static string collection_queues_nav_select(string active, string version)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?sublist=ALL&loan_officer=ALL&loan_type=ALL&workflow=Promised%20To%20Pay&version=" + version; nav.text = "Promised To Pay"; navs.Add(nav);
            //nav = new Nav(); nav.link = "Default.aspx?workflow=Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=Using%20Current%20Promised%20To%20Pay%20Expired&version=" + version; nav.text = "Beta Promised To Pay using last current"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=4-Month%20Delinquent%20In%20Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "4-Month Delinquency Report In Collections"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=4-Month%20Delinquent%20NOT%20In%20Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "4-Month Delinquency Report NOT In Collections"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=FHA%20VA%2017-day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "FHA VA 17-day Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=Fannie%20Mae%2017%20day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 17 day Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=Fannie%20Mae%2017-day%20MCM%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 17-day MCM Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=FHA%2060%20Day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "FHA 60 Day Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=Fannie%20Mae%202%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 2 Month Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=VA%202%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "VA 2 Month Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=HUD%203%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "HUD 3 Month Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=Fannie%20Mae%203%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 3 Month Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=VA%203%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "VA 3 Month Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/History.aspx?version=" + version; nav.text = "History"; navs.Add(nav);
            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }

        public static string collection_reports_nav_select(string active, string version)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "../events/CollectionReports.aspx?workflow=4-Month%20Delinquent%20In%20Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "4-Month Delinquent In Collections"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/CollectionReports.aspx?workflow=4-Month%20Delinquent%20NOT%20In%20Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "4-Month Delinquent NOT In Collections"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/CollectionReports.aspx?sublist=ALL&loan_officer=ALL&loan_type=ALL&workflow=Different%20Reason%20Codes&version=" + version; nav.text = "Different Reason Codes"; navs.Add(nav);
            //nav = new Nav(); nav.link = "Default.aspx?workflow=Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=Using%20Current%20Promised%20To%20Pay%20Expired&version=" + version; nav.text = "Beta Promised To Pay using last current"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=FHA%20VA%2017-day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "FHA VA 17-day Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=Fannie%20Mae%2017%20day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 17 day Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=Fannie%20Mae%2017-day%20MCM%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 17-day MCM Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=FHA%2060%20Day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "FHA 60 Day Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=Fannie%20Mae%202%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 2 Month Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=VA%202%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "VA 2 Month Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=HUD%203%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "HUD 3 Month Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=Fannie%20Mae%203%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 3 Month Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=VA%203%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "VA 3 Month Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/History.aspx?version=" + version; nav.text = "History"; navs.Add(nav);
            nav = new Nav(); nav.link = "../EndOfMonthReports/Default.aspx"; nav.text = "End of Month Reports"; navs.Add(nav);
            nav = new Nav(); nav.link = "../ims/Default.aspx"; nav.text = "IMS"; navs.Add(nav);
            nav = new Nav(); nav.link = "../epd/Default.aspx"; nav.text = "EPD"; navs.Add(nav);
            nav = new Nav(); nav.link = "../ach/Default.aspx"; nav.text = "ACH"; navs.Add(nav);

            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }

        public static string collection_reports_nav_select_section(string active, string version, string section)
        {
            string version_and_section = "&version=" + version + "&section=" + section;
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "../events/CollectionReports.aspx?workflow=4-Month%20Delinquent%20In%20Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=ALL" + version_and_section; nav.text = "4-Month Delinquent In Collections"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/CollectionReports.aspx?workflow=4-Month%20Delinquent%20NOT%20In%20Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=ALL" + version_and_section; nav.text = "4-Month Delinquent NOT In Collections"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/CollectionReports.aspx?sublist=ALL&loan_officer=ALL&loan_type=ALL&workflow=Different%20Reason%20Codes" + version_and_section; nav.text = "Different Reason Codes"; navs.Add(nav);
            //nav = new Nav(); nav.link = "Default.aspx?workflow=Collections&sublist=ALL&loan_officer=ALL&loan_type=ALL&event_type=Using%20Current%20Promised%20To%20Pay%20Expired&version=" + version; nav.text = "Beta Promised To Pay using last current"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=FHA%20VA%2017-day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "FHA VA 17-day Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=Fannie%20Mae%2017%20day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 17 day Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=Fannie%20Mae%2017-day%20MCM%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 17-day MCM Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=FHA%2060%20Day%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "FHA 60 Day Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=Fannie%20Mae%202%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 2 Month Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=VA%202%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "VA 2 Month Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=HUD%203%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "HUD 3 Month Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=Fannie%20Mae%203%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "Fannie Mae 3 Month Call Listing"; navs.Add(nav);
            //nav = new Nav(); nav.link = "../events/CollectionQueues.aspx?workflow=VA%203%20Month%20Call%20Listing&sublist=ALL&collector=ALL&loan_type=ALL&event_type=ALL&version=" + version; nav.text = "VA 3 Month Call Listing"; navs.Add(nav);
            nav = new Nav(); nav.link = "../events/History.aspx?p=param" + version_and_section; nav.text = "History"; navs.Add(nav);
            nav = new Nav(); nav.link = "../EndOfMonthReports/Default.aspx?p=param" + version_and_section; nav.text = "End of Month Reports"; navs.Add(nav);
            nav = new Nav(); nav.link = "../ims/Default.aspx?p=param" + version_and_section; nav.text = "IMS"; navs.Add(nav);
            nav = new Nav(); nav.link = "../epd/Default.aspx?p=param" + version_and_section; nav.text = "EPD"; navs.Add(nav);
            nav = new Nav(); nav.link = "../ach/Default.aspx?p=param" + version_and_section; nav.text = "ACH"; navs.Add(nav);
            nav = new Nav(); nav.link = "../EOMInvestorReporting/Default.aspx?p=param" + version_and_section; nav.text = "EOM Investor Reporting"; navs.Add(nav);

            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }

        public static string collection_quarter_reports_nav_select_section(string active, string version, string section)
        {
            string version_and_section = "&version=" + version + "&section=" + section;
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "../../foreclosures/QuarterReports/Default.aspx"; nav.text = "Loss Mitigation In Process"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=FNMA" + version_and_section; nav.text = "FNMA"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=GNMA" + version_and_section; nav.text = "GNMA"; navs.Add(nav);

            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }

        public static string end_of_month_reports_nav_select(string active, string version)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "Default.aspx?version=" + version; nav.text = "All"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Bankruptcies&version=" + version; nav.text = "Bankruptcies"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Demands&version=" + version; nav.text = "Demands"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Foreclosures&version=" + version; nav.text = "Foreclosures"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=FNMA&version=" + version; nav.text = "FNMA"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=GNMA&version=" + version; nav.text = "GNMA"; navs.Add(nav);
            nav = new Nav(); nav.link = "LMCodes.aspx?workflow=All&version=" + version; nav.text = "LM Codes"; navs.Add(nav);
            nav = new Nav(); nav.link = "MissingOccupancyCode.aspx?workflow=All&version=" + version; nav.text = "Missing Occupancy Code"; navs.Add(nav);
            nav = new Nav(); nav.link = "MissingReasonCode.aspx?workflow=All&version=" + version; nav.text = "Missing Default Reason Code"; navs.Add(nav);
            nav = new Nav(); nav.link = "Huddel42.aspx?workflow=All&version=" + version; nav.text = "HUDDEL42"; navs.Add(nav);
            nav = new Nav(); nav.link = "MissingMortgageStatusCode.aspx?workflow=All&version=" + version; nav.text = "Missing Mortgage Status Code"; navs.Add(nav);
            nav = new Nav(); nav.link = "MissingMortgageStatusDate.aspx?workflow=All&version=" + version; nav.text = "Missing Mortgage Status Date"; navs.Add(nav);

            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }

        public static string end_of_month_reports_nav_select_section(string active, string version, string section)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "Default.aspx?version=" + version + "&section=" + section; nav.text = "All"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Bankruptcies&version=" + version + "&section=" + section; nav.text = "Bankruptcies"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Demands&version=" + version + "&section=" + section; nav.text = "Demands"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=Foreclosures&version=" + version + "&section=" + section; nav.text = "Foreclosures"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=FNMA&version=" + version + "&section=" + section; nav.text = "FNMA"; navs.Add(nav);
            nav = new Nav(); nav.link = "Default.aspx?workflow=GNMA&version=" + version + "&section=" + section; nav.text = "GNMA"; navs.Add(nav);
            nav = new Nav(); nav.link = "LMCodes.aspx?workflow=All&version=" + version + "&section=" + section; nav.text = "LM Codes"; navs.Add(nav);
            nav = new Nav(); nav.link = "AllLMCodes.aspx?workflow=All&version=" + version + "&section=" + section; nav.text = "ALL LM Codes"; navs.Add(nav);

            nav = new Nav(); nav.link = "Covid19.aspx?workflow=All&version=" + version; nav.text = "COVID-19"; navs.Add(nav);

            nav = new Nav(); nav.link = "MissingOccupancyCode.aspx?workflow=All&version=" + version + "&section=" + section; nav.text = "Missing Occupancy Code"; navs.Add(nav);
            nav = new Nav(); nav.link = "MissingReasonCode.aspx?workflow=All&version=" + version + "&section=" + section; nav.text = "Missing Default Reason Code"; navs.Add(nav);
            nav = new Nav(); nav.link = "Huddel42.aspx?workflow=All&version=" + version + "&section=" + section; nav.text = "HUDDEL42"; navs.Add(nav);
            nav = new Nav(); nav.link = "MissingMortgageStatusCode.aspx?workflow=All&version=" + version + "&section=" + section; nav.text = "Missing Mortgage Status Code"; navs.Add(nav);
            nav = new Nav(); nav.link = "MissingMortgageStatusDate.aspx?workflow=All&version=" + version + "&section=" + section; nav.text = "Missing Mortgage Status Date"; navs.Add(nav);
            nav = new Nav(); nav.link = "NoContactList.aspx?workflow=No Contact List&version=" + version + "&section=" + section; nav.text = "No Contact List"; navs.Add(nav);
            nav = new Nav(); nav.link = "BankruptcyReport.aspx?workflow=BankruptcyReport&version=" + version + "&section=" + section; nav.text = "Bankruptcy Report"; navs.Add(nav);
            nav = new Nav(); nav.link = "ModsCompletedReport.aspx?workflow=ModsCompletedReport&version=" + version + "&section=" + section; nav.text = "Mods Completed Report"; navs.Add(nav);
            nav = new Nav(); nav.link = "L605.aspx"; nav.text = "MBA Quarterly Reports"; navs.Add(nav);
            nav = new Nav(); nav.link = "ModificationAnalysisReport.aspx"; nav.text = "Modification Analysis Report"; navs.Add(nav);
            nav = new Nav(); nav.link = "SpreadDeficiencyShortage.aspx"; nav.text = "Spread Deficiency/Shortage"; navs.Add(nav);
            nav = new Nav(); nav.link = "11AccountStatus.aspx"; nav.text = "11 Account Status"; navs.Add(nav);
            

            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }

        public static string mba_quarterly_reports_nav_select_section(string active, string version, string section)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "L605.aspx"; nav.text = "L605"; navs.Add(nav);
            nav = new Nav(); nav.link = "L606.aspx"; nav.text = "L606"; navs.Add(nav);
            nav = new Nav(); nav.link = "L607.aspx"; nav.text = "L607"; navs.Add(nav);
            nav = new Nav(); nav.link = "L608.aspx"; nav.text = "L608"; navs.Add(nav);
            nav = new Nav(); nav.link = "L609.aspx"; nav.text = "L609"; navs.Add(nav);
            nav = new Nav(); nav.link = "L610.aspx"; nav.text = "L610"; navs.Add(nav);
            nav = new Nav(); nav.link = "L611.aspx"; nav.text = "L611"; navs.Add(nav);
            nav = new Nav(); nav.link = "L612.aspx"; nav.text = "L612"; navs.Add(nav);
            nav = new Nav(); nav.link = "L613.aspx"; nav.text = "L613"; navs.Add(nav);
            nav = new Nav(); nav.link = "L619.aspx"; nav.text = "L619"; navs.Add(nav);
            nav = new Nav(); nav.link = "L620.aspx"; nav.text = "L620"; navs.Add(nav);
            nav = new Nav(); nav.link = "L621.aspx"; nav.text = "L621"; navs.Add(nav);
            nav = new Nav(); nav.link = "L622.aspx"; nav.text = "L622"; navs.Add(nav);
            nav = new Nav(); nav.link = "L623.aspx"; nav.text = "L623"; navs.Add(nav);

            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }

        public static string eom_investor_reporting_nav_select_section(string active, string version, string section)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "Default.aspx?version=" + version + "&section=" + section; nav.text = "Fannie Mae Delinquency Report"; navs.Add(nav);
            nav = new Nav(); nav.link = "FNMAHAMPReporting.aspx?version=" + version + "&section=" + section; nav.text = "FNMA HAMP Reporting"; navs.Add(nav);
            nav = new Nav(); nav.link = "NationStarPdf.aspx?version=" + version + "&section=" + section; nav.text = "Nation Star PDF"; navs.Add(nav);

            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }

        public static string task_list_nav =
    @"<div class=""navbar"">
       <div class=""navbar-inner"">
        <ul class=""nav"">
          <li>
            <a href=""Demand.aspx"">Demand Queue</a>
          </li>
          <li>
            <a href=""PreForeclosure.aspx"">Pre-Foreclosure Queue</a>
          </li>
          <li>
            <a href=""Foreclosure.aspx"">Foreclosure Queue</a>
          </li>
        </ul>
       </div>
      </div>";


        public static string task_list_nav_select(string active)
        {
            string s = nav_start;
            List<Nav> navs = new List<Nav>();
            Nav nav = new Nav(); nav.link = "Default.aspx"; nav.text = "Delinquent Flow"; navs.Add(nav);
            nav = new Nav(); nav.link = "Delinquent.aspx"; nav.text = "Delinquent Queue"; navs.Add(nav);
            nav = new Nav(); nav.link = "DelinquentFlow.aspx"; nav.text = "Delinquent Grid"; navs.Add(nav);
            string active_class = "";
            foreach (Nav n in navs)
            {
                if (active == n.text) active_class = @" class=""active"""; else active_class = "";
                s += @"<li" + active_class + @"><a href=""" + n.link + @""">" + n.text + "</a></li>";
            }
            return s + nav_end;
        }
    }
}